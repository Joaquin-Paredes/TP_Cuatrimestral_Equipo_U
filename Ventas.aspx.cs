using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestión_Comercio_TP_Cuatrimestral_Equipo_U
{
    public partial class Ventas : System.Web.UI.Page
    {
        private DataTable Carrito
        {
            get
            {
                if (ViewState["Carrito"] == null)
                {
                    DataTable tabla = new DataTable();

                    tabla.Columns.Add("Producto", typeof(string));
                    tabla.Columns.Add("Cantidad", typeof(int));
                    tabla.Columns.Add("PrecioUnitario", typeof(decimal));
                    tabla.Columns.Add("Subtotal", typeof(decimal));

                    ViewState["Carrito"] = tabla;
                }
                return (DataTable)ViewState["Carrito"];
            }
            set
            {
                ViewState["Carrito"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
                CargarCarrito();
            }
        }

        private void CargarCarrito()
        {
            dgvCarrito.DataSource = Carrito;
            dgvCarrito.DataBind();
            ActualizarTotal();
        }

        private void ActualizarTotal()
        {
            decimal total = 0;
            foreach (DataRow fila in Carrito.Rows)
            {
                total += Convert.ToDecimal(fila["Subtotal"]);
            }
            txtTotal.Text = total.ToString("N2", CultureInfo.CurrentCulture);
        }

        protected void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            string producto = txtProducto.Text.Trim();
            int cantidad;
            decimal precio;

            if (string.IsNullOrWhiteSpace(producto))
            {
                MostrarMensaje("Debe ingresar un producto.", false);
                return;
            }

            if (!int.TryParse(txtCantidad.Text.Trim(), out cantidad) || cantidad <= 0)
            {
                MostrarMensaje("Ingrese una cantidad válida.", false);
                return;
            }

            if (!decimal.TryParse(txtPrecioUnitario.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out precio) &&
                !decimal.TryParse(txtPrecioUnitario.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out precio))
            {
                MostrarMensaje("Ingrese un precio unitario válido.", false);
                return;
            }

            if (precio < 0)
            {
                MostrarMensaje("El precio no puede ser negativo.", false);
                return;
            }

            decimal subtotal = cantidad * precio;

            DataTable tabla = Carrito;
            DataRow nuevaFila = tabla.NewRow();

            nuevaFila["Producto"] = producto;
            nuevaFila["Cantidad"] = cantidad;
            nuevaFila["PrecioUnitario"] = precio;
            nuevaFila["Subtotal"] = subtotal;

            tabla.Rows.Add(nuevaFila);
            Carrito = tabla;

            CargarCarrito();

            txtProducto.Text = "";
            txtCantidad.Text = "";
            txtPrecioUnitario.Text = "";

            MostrarMensaje("Producto agregado al carrito correctamente.", true);
        }

        protected void dgvCarrito_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable tabla = Carrito;

            if (e.RowIndex >= 0 && e.RowIndex < tabla.Rows.Count)
            {
                tabla.Rows.RemoveAt(e.RowIndex);
                Carrito = tabla;
                CargarCarrito();
                MostrarMensaje("Producto eliminado del carrito.", true);
            }

            e.Cancel = true;
        }

        protected void btnRegistrarVenta_Click(object sender, EventArgs e)
        {
            if (Carrito.Rows.Count == 0)
            {
                MostrarMensaje("El carrito está vacío. Agregue productos antes de registrar la venta.", false);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNumeroVenta.Text) || string.IsNullOrWhiteSpace(txtCliente.Text))
            {
                MostrarMensaje("El número de venta y el cliente son obligatorios.", false);
                return;
            }

            decimal totalVenta = 0;
            if (!decimal.TryParse(txtTotal.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out totalVenta))
            {
                MostrarMensaje("Error al calcular el total de la venta.", false);
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlTransaction transaccion = conn.BeginTransaction();

                try
                {
                    string queryVenta = @"
                        INSERT INTO Ventas (NumeroVenta, Fecha, Cliente, Vendedor, Total) 
                        VALUES (@NumeroVenta, @Fecha, @Cliente, @Vendedor, @Total);
                        SELECT SCOPE_IDENTITY();";

                    SqlCommand cmdVenta = new SqlCommand(queryVenta, conn, transaccion);
                    cmdVenta.Parameters.AddWithValue("@NumeroVenta", txtNumeroVenta.Text.Trim());
                    cmdVenta.Parameters.AddWithValue("@Fecha", txtFecha.Text);
                    cmdVenta.Parameters.AddWithValue("@Cliente", txtCliente.Text.Trim());
                    cmdVenta.Parameters.AddWithValue("@Vendedor", txtVendedor.Text.Trim());
                    cmdVenta.Parameters.AddWithValue("@Total", totalVenta);

                    cmdVenta.ExecuteNonQuery();

                    string queryDetalle = @"
                        INSERT INTO DetalleVenta (NumeroVenta, Producto, Cantidad, PrecioUnitario, Subtotal) 
                        VALUES (@NumeroVenta, @Producto, @Cantidad, @PrecioUnitario, @Subtotal)";

                    foreach (DataRow fila in Carrito.Rows)
                    {
                        SqlCommand cmdDetalle = new SqlCommand(queryDetalle, conn, transaccion);
                        cmdDetalle.Parameters.AddWithValue("@NumeroVenta", txtNumeroVenta.Text.Trim());
                        cmdDetalle.Parameters.AddWithValue("@Producto", fila["Producto"].ToString());
                        cmdDetalle.Parameters.AddWithValue("@Cantidad", Convert.ToInt32(fila["Cantidad"]));
                        cmdDetalle.Parameters.AddWithValue("@PrecioUnitario", Convert.ToDecimal(fila["PrecioUnitario"]));
                        cmdDetalle.Parameters.AddWithValue("@Subtotal", Convert.ToDecimal(fila["Subtotal"]));

                        cmdDetalle.ExecuteNonQuery();
                    }

                    transaccion.Commit();

                    MostrarMensaje("Venta registrada exitosamente.", true);
                    LimpiarPantallaVenta();
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    MostrarMensaje("Error al registrar la venta: " + ex.Message, false);
                }
            }
        }

        private void LimpiarPantallaVenta()
        {
            txtNumeroVenta.Text = "";
            txtCliente.Text = "";
            txtVendedor.Text = "";
            txtProducto.Text = "";
            txtCantidad.Text = "";
            txtPrecioUnitario.Text = "";
            Carrito = null;
            CargarCarrito();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarPantallaVenta();
            MostrarMensaje("", true);
        }

        private void MostrarMensaje(string mensaje, bool correcto)
        {
            lblMensajeCarrito.Text = mensaje;
            lblMensajeCarrito.CssClass = correcto ? "fw-bold text-success" : "fw-bold text-danger";
        }
    }
}