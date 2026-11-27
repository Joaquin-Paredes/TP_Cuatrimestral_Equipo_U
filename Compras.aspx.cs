using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;

namespace Gestión_Comercio_TP_Cuatrimestral_Equipo_U
{
    public partial class Compras : System.Web.UI.Page
    {
        private class CompraItem
        {
            public string NumeroCompra { get; set; }
            public DateTime Fecha { get; set; }
            public string ProveedorNombre { get; set; }
            public string ProductoNombre { get; set; }
            public int Cantidad { get; set; }
            public decimal PrecioUnitario { get; set; }
            public decimal Total { get; set; }
        }

        private List<CompraItem> ObtenerCompras()
        {
            return new List<CompraItem>
            {
                new CompraItem
                {
                    NumeroCompra = "C-0001",
                    Fecha = DateTime.Today,
                    ProveedorNombre = "Distribuidora Norte",
                    ProductoNombre = "Teclado Mecánico",
                    Cantidad = 5,
                    PrecioUnitario = 25000m,
                    Total = 125000m
                }
            };
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFechaCompra.Text = DateTime.Now.ToString("yyyy-MM-dd");
                CargarGrilla();
            }
        }
        private void CargarGrilla()
        {

            string filtro = txtBuscarNombreDinamico.Text.Trim();
            var lista = ObtenerCompras();

            if (!string.IsNullOrEmpty(filtro))
            {
                lista = lista.Where(c =>
                    (!string.IsNullOrEmpty(c.ProveedorNombre) && c.ProveedorNombre.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (!string.IsNullOrEmpty(c.ProductoNombre) && c.ProductoNombre.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0)
                ).ToList();
            }

            dgvCompras.DataSource = lista;
            dgvCompras.DataBind();
        }

        protected void txtBuscarNombreDinamico_TextChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        protected void dgvCompras_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow fila = dgvCompras.SelectedRow;
            if (fila == null) return;

            lblDetNumero.Text = fila.Cells[1].Text;
            lblDetFecha.Text = fila.Cells[2].Text;
            lblDetProveedor.Text = fila.Cells[3].Text;
            lblDetProducto.Text = fila.Cells[4].Text;
            lblDetCantidad.Text = fila.Cells[5].Text;
            lblDetTotal.Text = fila.Cells[7].Text;

            pnlDetalleCompra.Visible = true;
        }

        protected void btnRegistrarCompra_Click(object sender, EventArgs e)
        {
            string numeroCompra = txtNumeroCompra.Text.Trim();
            string proveedor = txtProveedor.Text.Trim();
            string producto = txtProducto.Text.Trim();
            DateTime fecha;
            int cantidad;
            decimal precio;

            if (string.IsNullOrWhiteSpace(numeroCompra) || string.IsNullOrWhiteSpace(proveedor) || string.IsNullOrWhiteSpace(producto))
            {
                MostrarMensaje("El número de compra, el proveedor y el producto son obligatorios.", false);
                return;
            }

            if (!DateTime.TryParse(txtFechaCompra.Text, out fecha))
            {
                MostrarMensaje("Ingrese una fecha válida.", false);
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

            decimal total = cantidad * precio;
            txtTotalCompra.Text = total.ToString("N2", CultureInfo.CurrentCulture);

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlTransaction transaccion = conn.BeginTransaction();

                try
                {
                    string queryCompra = @"
                        INSERT INTO Compras (NumeroCompra, Fecha, Proveedor, Total)
                        VALUES (@NumeroCompra, @Fecha, @Proveedor, @Total)";

                    SqlCommand cmdCompra = new SqlCommand(queryCompra, conn, transaccion);
                    cmdCompra.Parameters.AddWithValue("@NumeroCompra", numeroCompra);
                    cmdCompra.Parameters.AddWithValue("@Fecha", fecha);
                    cmdCompra.Parameters.AddWithValue("@Proveedor", proveedor);
                    cmdCompra.Parameters.AddWithValue("@Total", total);
                    cmdCompra.ExecuteNonQuery();

                    string queryDetalle = @"
                        INSERT INTO DetalleCompra (NumeroCompra, Producto, Cantidad, PrecioUnitario, Subtotal)
                        VALUES (@NumeroCompra, @Producto, @Cantidad, @PrecioUnitario, @Subtotal)";

                    SqlCommand cmdDetalle = new SqlCommand(queryDetalle, conn, transaccion);
                    cmdDetalle.Parameters.AddWithValue("@NumeroCompra", numeroCompra);
                    cmdDetalle.Parameters.AddWithValue("@Producto", producto);
                    cmdDetalle.Parameters.AddWithValue("@Cantidad", cantidad);
                    cmdDetalle.Parameters.AddWithValue("@PrecioUnitario", precio);
                    cmdDetalle.Parameters.AddWithValue("@Subtotal", total);
                    cmdDetalle.ExecuteNonQuery();

                    transaccion.Commit();
                    MostrarMensaje("Compra registrada exitosamente.", true);
                    LimpiarCampos();
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    MostrarMensaje("Error al registrar la compra: " + ex.Message, false);
                }
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            MostrarMensaje("", true);
        }

        private void LimpiarCampos()
        {
            txtNumeroCompra.Text = "";
            txtFechaCompra.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtProveedor.Text = "";
            txtProducto.Text = "";
            txtCantidad.Text = "";
            txtPrecioUnitario.Text = "";
            txtTotalCompra.Text = "";
        }

        private void MostrarMensaje(string mensaje, bool correcto)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.CssClass = correcto ? "fw-bold text-success" : "fw-bold text-danger";
        }
    }
}