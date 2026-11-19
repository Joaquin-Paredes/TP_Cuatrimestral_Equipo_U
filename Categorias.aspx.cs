
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

namespace Gestión_Comercio_TP_Cuatrimestral_Equipo_U
{
    public partial class Categorias : System.Web.UI.Page
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
                SqlCommand cmd = new SqlCommand("SELECT Codigo, Nombre, Descripcion FROM Categorias WHERE Estado = @Estado", conn);
                cmd.Parameters.AddWithValue("@Estado", estado);
                conn.Open();
                dgvCategorias.DataSource = cmd.ExecuteReader();
                dgvCategorias.DataBind();
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Categorias (Codigo, Nombre, Descripcion, Estado) VALUES (@Codigo, @Nombre, @Desc, 1)", conn);
                cmd.Parameters.AddWithValue("@Codigo", txtCodigoCategoria.Text);
                cmd.Parameters.AddWithValue("@Nombre", txtNombreCategoria.Text);
                cmd.Parameters.AddWithValue("@Desc", txtDescripcion.Text);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MostrarMensaje("Categoría agregada correctamente.", true);
                    LimpiarCampos();
                }
                catch (Exception)
                {
                    MostrarMensaje("Error al agregar la categoría.", false);
                }
            }
            CargarGrilla();
        }

        protected void btnModificar_Click(object sender, EventArgs e)
        {
            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Categorias SET Nombre = @Nombre, Descripcion = @Desc WHERE Codigo = @Codigo AND Estado = 1", conn);
                cmd.Parameters.AddWithValue("@Codigo", txtCodigoCategoria.Text);
                cmd.Parameters.AddWithValue("@Nombre", txtNombreCategoria.Text);
                cmd.Parameters.AddWithValue("@Desc", txtDescripcion.Text);
                try
                {
                    conn.Open();
                    int filas = cmd.ExecuteNonQuery();
                    if (filas > 0)
                    {
                        MostrarMensaje("Categoría modificada correctamente.", true);
                        LimpiarCampos();
                    }
                    else
                    {
                        MostrarMensaje("No se encontró la categoría.", false);
                    }
                }
                catch (Exception)
                {
                    MostrarMensaje("Error al modificar la categoría.", false);
                }
            }
            CargarGrilla();
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigoCategoria.Text))
            {
                MostrarMensaje("Seleccione una categoría para eliminar.", false);
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Categorias SET Estado = 0 WHERE Codigo = @Codigo AND Estado = 1", conn);
                cmd.Parameters.AddWithValue("@Codigo", txtCodigoCategoria.Text);
                try
                {
                    conn.Open();
                    int filas = cmd.ExecuteNonQuery();
                    if (filas > 0)
                    {
                        MostrarMensaje("Categoría eliminada correctamente.", true);
                        LimpiarCampos();
                    }
                    else
                    {
                        MostrarMensaje("No se encontró la categoría.", false);
                    }
                }
                catch (Exception)
                {
                    MostrarMensaje("Error al eliminar la categoría.", false);
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
            if (string.IsNullOrWhiteSpace(txtCodigoCategoria.Text))
            {
                MostrarMensaje("Seleccione una categoría inactiva para reactivar.", false);
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Categorias SET Estado = 1 WHERE Codigo = @Codigo", conn);
                cmd.Parameters.AddWithValue("@Codigo", txtCodigoCategoria.Text);
                try
                {
                    conn.Open();
                    int filas = cmd.ExecuteNonQuery();
                    if (filas > 0)
                    {
                        MostrarMensaje("Categoría reactivada correctamente.", true);
                        chkVerInactivos.Checked = false;
                        LimpiarCampos();
                    }
                    else
                    {
                        MostrarMensaje("No se encontró la categoría.", false);
                    }
                }
                catch (Exception)
                {
                    MostrarMensaje("Error al reactivar la categoría.", false);
                }
            }
            CargarGrilla();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            MostrarMensaje("", true);
        }

        protected void dgvCategorias_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvCategorias.SelectedRow != null)
            {
                txtCodigoCategoria.Text = dgvCategorias.SelectedRow.Cells[0].Text;
                txtCodigoCategoria.ReadOnly = true;
                txtNombreCategoria.Text = HttpUtility.HtmlDecode(dgvCategorias.SelectedRow.Cells[1].Text);
                txtDescripcion.Text = HttpUtility.HtmlDecode(dgvCategorias.SelectedRow.Cells[2].Text);
            }
        }

        private void LimpiarCampos()
        {
            txtCodigoCategoria.Text = "";
            txtCodigoCategoria.ReadOnly = false;
            txtNombreCategoria.Text = "";
            txtDescripcion.Text = "";
            dgvCategorias.SelectedIndex = -1;
        }

        private void MostrarMensaje(string mensaje, bool correcto)
        {
            lblMensaje.Text = HttpUtility.HtmlEncode(mensaje);
            lblMensaje.CssClass = correcto ? "ms-3 fw-bold text-success" : "ms-3 fw-bold text-danger";
        }
    }
}