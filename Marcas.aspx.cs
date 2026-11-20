using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;

namespace Gestión_Comercio_TP_Cuatrimestral_Equipo_U
{
    public partial class Marcas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) { CargarGrilla(); }
        }

        private void CargarGrilla()
        {
            int estado = chkVerInactivos.Checked ? 0 : 1;
            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SELECT Codigo, Nombre FROM Marcas WHERE Estado = @Estado", conn);
                cmd.Parameters.AddWithValue("@Estado", estado);
                conn.Open();
                dgvMarcas.DataSource = cmd.ExecuteReader();
                dgvMarcas.DataBind();
            }
        }

        protected void dgvMarcas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvMarcas.SelectedRow != null)
            {
                txtCodigoMarca.Text = dgvMarcas.SelectedRow.Cells[0].Text;
                txtCodigoMarca.ReadOnly = true;
                txtNombreMarca.Text = HttpUtility.HtmlDecode(dgvMarcas.SelectedRow.Cells[1].Text);
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Marcas (Codigo, Nombre, Estado) VALUES (@Codigo, @Nombre, 1)", conn);
                cmd.Parameters.AddWithValue("@Codigo", txtCodigoMarca.Text);
                cmd.Parameters.AddWithValue("@Nombre", txtNombreMarca.Text);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MostrarMensaje("Marca agregada correctamente.", true);
                    LimpiarCampos();
                }
                catch (Exception)
                {
                    MostrarMensaje("Error al agregar la marca.", false);
                }
            }
            CargarGrilla();
        }

        protected void btnModificar_Click(object sender, EventArgs e)
        {
            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Marcas SET Nombre = @Nombre WHERE Codigo = @Codigo AND Estado = 1", conn);
                cmd.Parameters.AddWithValue("@Codigo", txtCodigoMarca.Text);
                cmd.Parameters.AddWithValue("@Nombre", txtNombreMarca.Text);
                try
                {
                    conn.Open();
                    int filas = cmd.ExecuteNonQuery();
                    if (filas > 0)
                    {
                        MostrarMensaje("Marca modificada correctamente.", true);
                        LimpiarCampos();
                    }
                    else
                    {
                        MostrarMensaje("No se encontró la marca.", false);
                    }
                }
                catch (Exception)
                {
                    MostrarMensaje("Error al modificar la marca.", false);
                }
            }
            CargarGrilla();
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigoMarca.Text))
            {
                MostrarMensaje("Seleccione una marca para eliminar.", false);
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Marcas SET Estado = 0 WHERE Codigo = @Codigo AND Estado = 1", conn);
                cmd.Parameters.AddWithValue("@Codigo", txtCodigoMarca.Text);
                try
                {
                    conn.Open();
                    int filas = cmd.ExecuteNonQuery();
                    if (filas > 0)
                    {
                        MostrarMensaje("Marca eliminada correctamente.", true);
                        LimpiarCampos();
                    }
                    else
                    {
                        MostrarMensaje("No se encontró la marca.", false);
                    }
                }
                catch (Exception)
                {
                    MostrarMensaje("Error al eliminar la marca.", false);
                }
            }
            CargarGrilla();
        }

        protected void chkVerInactivos_CheckedChanged(object sender, EventArgs e)
        {
            LimpiarCampos();
            CargarGrilla();
        }

        protected void btnReactivar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigoMarca.Text))
            {
                MostrarMensaje("Seleccione una marca inactiva para reactivar.", false);
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Marcas SET Estado = 1 WHERE Codigo = @Codigo", conn);
                cmd.Parameters.AddWithValue("@Codigo", txtCodigoMarca.Text);
                try
                {
                    conn.Open();
                    int filas = cmd.ExecuteNonQuery();
                    if (filas > 0)
                    {
                        MostrarMensaje("Marca reactivada correctamente.", true);
                        chkVerInactivos.Checked = false;
                        LimpiarCampos();
                    }
                    else
                    {
                        MostrarMensaje("No se encontró la marca.", false);
                    }
                }
                catch (Exception)
                {
                    MostrarMensaje("Error al reactivar la marca.", false);
                }
            }
            CargarGrilla();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            MostrarMensaje("", true);
        }

        private void LimpiarCampos()
        {
            txtCodigoMarca.Text = "";
            txtCodigoMarca.ReadOnly = false;
            txtNombreMarca.Text = "";
            dgvMarcas.SelectedIndex = -1;
        }

        private void MostrarMensaje(string mensaje, bool correcto)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.CssClass = correcto ? "ms-3 fw-bold text-success" : "ms-3 fw-bold text-danger";
        }
    }
}