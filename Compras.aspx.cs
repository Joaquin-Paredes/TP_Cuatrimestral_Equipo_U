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
            List<CompraItem> lista = new List<CompraItem>();
            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = @"
                        SELECT c.NumeroCompra, c.Fecha, c.Proveedor, d.Producto, d.Cantidad, d.PrecioUnitario, d.Subtotal
                        FROM Compras c
                        INNER JOIN DetalleCompra d ON c.NumeroCompra = d.NumeroCompra
                        WHERE c.Estado = @Estado
                        ORDER BY c.Fecha DESC, c.NumeroCompra DESC";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Estado", chkMostrarEliminadas.Checked ? 0 : 1);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new CompraItem
                            {
                                NumeroCompra = reader["NumeroCompra"].ToString(),
                                Fecha = Convert.ToDateTime(reader["Fecha"]),
                                ProveedorNombre = reader["Proveedor"].ToString(),
                                ProductoNombre = reader["Producto"].ToString(),
                                Cantidad = Convert.ToInt32(reader["Cantidad"]),
                                PrecioUnitario = Convert.ToDecimal(reader["PrecioUnitario"]),
                                Total = Convert.ToDecimal(reader["Subtotal"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al cargar el listado: " + ex.Message, false);
            }

            return lista;
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

        protected void chkMostrarEliminadas_CheckedChanged(object sender, EventArgs e)
        {
            pnlDetalleCompra.Visible = false;
            CargarGrilla();
        }

        protected void dgvCompras_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow fila = dgvCompras.SelectedRow;
            if (fila == null) return;

            string numero = HttpUtility.HtmlDecode(fila.Cells[1].Text);
            string fechaTexto = HttpUtility.HtmlDecode(fila.Cells[2].Text);
            string proveedor = HttpUtility.HtmlDecode(fila.Cells[3].Text);
            string producto = HttpUtility.HtmlDecode(fila.Cells[4].Text);
            string cantidad = HttpUtility.HtmlDecode(fila.Cells[5].Text);
            string total = HttpUtility.HtmlDecode(fila.Cells[7].Text);

            lblDetNumero.Text = numero;
            lblDetFecha.Text = fechaTexto;
            lblDetProveedor.Text = proveedor;
            lblDetProducto.Text = producto;
            lblDetCantidad.Text = cantidad;
            lblDetTotal.Text = total;

            txtNumeroCompra.Text = numero;
            txtProveedor.Text = proveedor;

            DateTime fechaParsed;
            if (DateTime.TryParseExact(fechaTexto, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaParsed))
            {
                txtFechaCompra.Text = fechaParsed.ToString("yyyy-MM-dd");
            }

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
                    CargarGrilla();
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    MostrarMensaje("Error al registrar la compra: " + ex.Message, false);
                }
            }
        }

        protected void btnModificar_Click(object sender, EventArgs e)
        {
            string numeroCompra = txtNumeroCompra.Text.Trim();
            string proveedor = txtProveedor.Text.Trim();
            DateTime fecha;

            if (string.IsNullOrWhiteSpace(numeroCompra) || string.IsNullOrWhiteSpace(proveedor))
            {
                MostrarMensaje("Ingrese el número de compra y el proveedor para modificar.", false);
                return;
            }

            if (!DateTime.TryParse(txtFechaCompra.Text, out fecha))
            {
                MostrarMensaje("Ingrese una fecha válida.", false);
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = "UPDATE Compras SET Fecha = @Fecha, Proveedor = @Proveedor WHERE NumeroCompra = @NumeroCompra AND Estado = 1";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Fecha", fecha);
                    cmd.Parameters.AddWithValue("@Proveedor", proveedor);
                    cmd.Parameters.AddWithValue("@NumeroCompra", numeroCompra);

                    int filas = cmd.ExecuteNonQuery();
                    if (filas > 0)
                    {
                        CargarGrilla();
                        MostrarMensaje("Compra modificada correctamente.", true);
                    }
                    else
                    {
                        MostrarMensaje("No se encontró una compra activa con ese número.", false);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al modificar la compra: " + ex.Message, false);
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            string numeroCompra = txtNumeroCompra.Text.Trim();

            if (string.IsNullOrWhiteSpace(numeroCompra))
            {
                MostrarMensaje("Ingrese el número de compra que desea eliminar.", false);
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = "UPDATE Compras SET Estado = 0 WHERE NumeroCompra = @NumeroCompra AND Estado = 1";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@NumeroCompra", numeroCompra);

                    int filas = cmd.ExecuteNonQuery();
                    if (filas > 0)
                    {
                        LimpiarCampos();
                        pnlDetalleCompra.Visible = false;
                        CargarGrilla();
                        MostrarMensaje("Compra eliminada correctamente.", true);
                    }
                    else
                    {
                        MostrarMensaje("No se encontró una compra activa con ese número.", false);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al eliminar la compra: " + ex.Message, false);
            }
        }

        protected void btnReactivar_Click(object sender, EventArgs e)
        {
            string numeroCompra = txtNumeroCompra.Text.Trim();

            if (string.IsNullOrWhiteSpace(numeroCompra))
            {
                MostrarMensaje("Ingrese el número de compra que desea reactivar.", false);
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = "UPDATE Compras SET Estado = 1 WHERE NumeroCompra = @NumeroCompra AND Estado = 0";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@NumeroCompra", numeroCompra);

                    int filas = cmd.ExecuteNonQuery();
                    if (filas > 0)
                    {
                        LimpiarCampos();
                        pnlDetalleCompra.Visible = false;
                        CargarGrilla();
                        MostrarMensaje("Compra reactivada correctamente.", true);
                    }
                    else
                    {
                        MostrarMensaje("No se encontró una compra inactiva con ese número.", false);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al reactivar la compra: " + ex.Message, false);
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