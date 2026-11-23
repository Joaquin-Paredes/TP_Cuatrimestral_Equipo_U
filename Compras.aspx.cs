using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
    }
}