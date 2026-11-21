
using System;
using System.Data;
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
                MostrarMensaje("Debe ingresar un producto.");
                return;
            }

            if (!int.TryParse(txtCantidad.Text.Trim(), out cantidad) || cantidad <= 0)
            {
                MostrarMensaje("Ingrese una cantidad válida.");
                return;
            }

            if (!decimal.TryParse(txtPrecioUnitario.Text.Trim(),
                    NumberStyles.Number,
                    CultureInfo.CurrentCulture, out precio) &&
                !decimal.TryParse(txtPrecioUnitario.Text.Trim(),
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture, out precio))
            {
                MostrarMensaje("Ingrese un precio unitario válido.");
                return;
            }

            if (precio < 0)
            {
                MostrarMensaje("El precio no puede ser negativo.");
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

            MostrarMensaje("Producto agregado al carrito correctamente.");
        }

        
        protected void dgvCarrito_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable tabla = Carrito;

            if (e.RowIndex >= 0 && e.RowIndex < tabla.Rows.Count)
            {
                tabla.Rows.RemoveAt(e.RowIndex);

                Carrito = tabla;

                CargarCarrito();

                MostrarMensaje("Producto eliminado del carrito.");
            }

            e.Cancel = true;
        }

      
        private void MostrarMensaje(string mensaje)
        {
            string mensajeSeguro = mensaje
                .Replace("\\", "\\\\")
                .Replace("'", "\\'")
                .Replace("\r", " ")
                .Replace("\n", " ");

            lblMensajeCarrito.Text = mensajeSeguro;
        }
    }
}
