
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestión_Comercio_TP_Cuatrimestral_Equipo_U
{
    public partial class Categorias : System.Web.UI.Page
    {
      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarGrilla();
            }
        }

      
        private void CargarGrilla()
        {
            int estado = chkVerInactivos.Checked ? 0 : 1;

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"
                    SELECT Codigo, Nombre, Descripcion
                    FROM dbo.Categorias
                    WHERE Estado = @Estado
                    AND (@Nombre = '' OR Nombre LIKE '%' + @Nombre + '%')
                    ORDER BY Nombre";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.Add("@Estado", SqlDbType.Bit).Value = estado;
                cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 50).Value = txtBuscarNombreDinamico.Text.Trim();

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvCategorias.DataSource = dt;
                dgvCategorias.DataBind();
            }
        }
        protected void txtBuscarNombreDinamico_TextChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }


        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            // VALIDAR CAMPOS
            if (string.IsNullOrWhiteSpace(txtCodigoCategoria.Text) ||
                string.IsNullOrWhiteSpace(txtNombreCategoria.Text))
            {
                MostrarMensaje("Debe ingresar el código y el nombre de la categoría.", false);
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    // COMPROBAR SI EL CODIGO YA EXISTE
                    string consulta = @"
                SELECT COUNT(1)
                FROM dbo.Categorias
                WHERE Codigo = @Codigo";

                    SqlCommand cmdExiste = new SqlCommand(consulta, conn);

                    cmdExiste.Parameters.Add("@Codigo", SqlDbType.VarChar, 20).Value =
                        txtCodigoCategoria.Text.Trim();

                    int cantidad = Convert.ToInt32(cmdExiste.ExecuteScalar());

                    if (cantidad > 0)
                    {
                        MostrarMensaje("Ya existe una categoría registrada con ese código.", false);
                        return;
                    }

                    // INSERTAR CATEGORIA SI EL CODIGO NO EXISTE
                    string query = @"
                INSERT INTO dbo.Categorias
                (Codigo, Nombre, Descripcion, Estado)
                VALUES
                (@Codigo, @Nombre, @Desc, 1)";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.Add("@Codigo", SqlDbType.VarChar, 20).Value =
                        txtCodigoCategoria.Text.Trim();

                    cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 50).Value =
                        txtNombreCategoria.Text.Trim();

                    cmd.Parameters.Add("@Desc", SqlDbType.VarChar, 250).Value =
                        string.IsNullOrWhiteSpace(txtDescripcion.Text)
                        ? (object)DBNull.Value : txtDescripcion.Text.Trim();

                    cmd.ExecuteNonQuery();

                    MostrarMensaje("Categoría agregada correctamente.", true);

                    LimpiarCampos();
                    CargarGrilla();
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627 || ex.Number == 2601)
                    {
                        MostrarMensaje("Ya existe una categoría registrada con ese código.", false);
                    }
                    else
                    {
                        MostrarMensaje("Error al agregar la categoría: " + ex.Message, false);
                    }
                }
            }
        }



        protected void btnModificar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigoCategoria.Text) ||
                string.IsNullOrWhiteSpace(txtNombreCategoria.Text))
            {
                MostrarMensaje("Debe seleccionar una categoría e ingresar su nombre.", false);
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"
                    UPDATE dbo.Categorias
                    SET Nombre = @Nombre,
                        Descripcion = @Desc
                    WHERE Codigo = @Codigo
                    AND Estado = 1";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.Add("@Codigo", SqlDbType.VarChar, 20).Value =
                    txtCodigoCategoria.Text.Trim();

                cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 50).Value =
                    txtNombreCategoria.Text.Trim();

                cmd.Parameters.Add("@Desc", SqlDbType.VarChar, 250).Value =
                    string.IsNullOrWhiteSpace(txtDescripcion.Text)
                    ? (object)DBNull.Value : txtDescripcion.Text.Trim();

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
                        MostrarMensaje("No se encontró la categoría activa.", false);
                    }
                }
                catch (SqlException ex)
                {
                    MostrarMensaje("Error al modificar la categoría: " + ex.Message, false);
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
                string query = @"
                    UPDATE dbo.Categorias
                    SET Estado = 0
                    WHERE Codigo = @Codigo
                    AND Estado = 1";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.Add("@Codigo", SqlDbType.VarChar, 20).Value =
                    txtCodigoCategoria.Text.Trim();

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
                        MostrarMensaje("No se encontró la categoría activa.", false);
                    }
                }
                catch (SqlException ex)
                {
                    MostrarMensaje("Error al eliminar la categoría: " + ex.Message, false);
                }
            }

            CargarGrilla();
        }


        protected void chkVerInactivos_CheckedChanged(object sender, EventArgs e)
        {
            LimpiarCampos(); txtDescripcion.Text = "";
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
                string query = @"
                    UPDATE dbo.Categorias
                    SET Estado = 1
                    WHERE Codigo = @Codigo
                    AND Estado = 0";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.Add("@Codigo", SqlDbType.VarChar, 20).Value =
                    txtCodigoCategoria.Text.Trim();

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
                        MostrarMensaje("No se encontró la categoría inactiva.", false);
                    }
                }
                catch (SqlException ex)
                {
                    MostrarMensaje("Error al reactivar la categoría: " + ex.Message, false);
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
            txtCodigoCategoria.Text = "";
            txtCodigoCategoria.ReadOnly = false;

            txtNombreCategoria.Text = "";
            txtDescripcion.Text = "";
            txtBuscarNombreDinamico.Text = "";

            dgvCategorias.SelectedIndex = -1;
        }

      
        protected void dgvCategorias_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvCategorias.SelectedRow != null)
            {
                txtCodigoCategoria.Text = HttpUtility.HtmlDecode(dgvCategorias.SelectedRow.Cells[0].Text);
                txtCodigoCategoria.ReadOnly = true;

                txtNombreCategoria.Text = HttpUtility.HtmlDecode(dgvCategorias.SelectedRow.Cells[1].Text);
                txtDescripcion.Text = HttpUtility.HtmlDecode(dgvCategorias.SelectedRow.Cells[2].Text);
            }
        }

       
        private void MostrarMensaje(string mensaje, bool correcto)
        {
            lblMensaje.Text = HttpUtility.HtmlEncode(mensaje);

            lblMensaje.CssClass = correcto
                ? "ms-3 fw-bold text-success"
                : "ms-3 fw-bold text-danger";
        }
    }
}
