
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gestión_Comercio_TP_Cuatrimestral_Equipo_U.Dominio;

namespace Gestión_Comercio_TP_Cuatrimestral_Equipo_U
{
    public partial class Marcas : System.Web.UI.Page
    {
        // COMPROBAR SI EL USUARIO ES ADMINISTRADOR
        private bool EsAdministrador()
        {
            Usuario usuario = Session["Usuario"] as Usuario;

            return usuario != null && usuario.EsAdmin;
        }

        // PROTEGER OPERACIONES DE ADMINISTRADOR
        private bool RequiereAdministrador()
        {
            if (EsAdministrador())
                return true;

            Response.Redirect("~/Default.aspx", false);
            Context.ApplicationInstance.CompleteRequest();

            return false;
        }

        // INICIO DE LA PÁGINA
        protected void Page_Load(object sender, EventArgs e)
        {
            bool esAdmin = EsAdministrador();

            // Ocultar formulario a usuarios comunes
            pnlGestionMarca.Visible = esAdmin;

            // Solo administradores pueden ver marcas eliminadas
            chkVerInactivos.Visible = esAdmin;

            // Ocultar botón Seleccionar a usuarios comunes
            CommandField campoSeleccion = dgvMarcas.Columns[2] as CommandField;

            if (campoSeleccion != null)
                campoSeleccion.ShowSelectButton = esAdmin;

            if (!IsPostBack)
            {
                CargarGrilla();
            }
        }

        // CARGAR GRILLA
        private void CargarGrilla()
        {
            int estado = EsAdministrador() && chkVerInactivos.Checked ? 0 : 1;

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"
                    SELECT Codigo, Nombre
                    FROM dbo.Marcas
                    WHERE Estado = @Estado
                    AND (@Nombre = '' OR Nombre LIKE '%' + @Nombre + '%')
                    ORDER BY Nombre";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.Add("@Estado", SqlDbType.Bit).Value = estado;
                cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 100).Value =
                    txtBuscarNombreDinamico.Text.Trim();

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    dgvMarcas.DataSource = reader;
                    dgvMarcas.DataBind();
                }
            }
        }

        // BUSCAR MARCAS
        protected void txtBuscarNombreDinamico_TextChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        // SELECCIONAR MARCA
        protected void dgvMarcas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!RequiereAdministrador())
                return;

            if (dgvMarcas.SelectedRow != null)
            {
                txtCodigoMarca.Text = dgvMarcas.SelectedRow.Cells[0].Text;
                txtCodigoMarca.ReadOnly = true;

                txtNombreMarca.Text = HttpUtility.HtmlDecode(
                    dgvMarcas.SelectedRow.Cells[1].Text);
            }
        }

        // AGREGAR MARCA
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            if (!RequiereAdministrador())
                return;

            if (string.IsNullOrWhiteSpace(txtCodigoMarca.Text))
            {
                MostrarMensaje("Debe ingresar el código de la marca.", false);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNombreMarca.Text))
            {
                MostrarMensaje("Debe ingresar el nombre de la marca.", false);
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
                        FROM dbo.Marcas
                        WHERE Codigo = @Codigo";

                    SqlCommand cmdExiste = new SqlCommand(consulta, conn);

                    cmdExiste.Parameters.Add("@Codigo", SqlDbType.VarChar, 20).Value =
                        txtCodigoMarca.Text.Trim();

                    int cantidad = Convert.ToInt32(cmdExiste.ExecuteScalar());

                    if (cantidad > 0)
                    {
                        MostrarMensaje("Ya existe una marca registrada con ese código.", false);
                        return;
                    }

                    // INSERTAR MARCA
                    string query = @"
                        INSERT INTO dbo.Marcas
                        (Codigo, Nombre, Estado)
                        VALUES
                        (@Codigo, @Nombre, 1)";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.Add("@Codigo", SqlDbType.VarChar, 20).Value =
                        txtCodigoMarca.Text.Trim();

                    cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 50).Value =
                        txtNombreMarca.Text.Trim();

                    cmd.ExecuteNonQuery();

                    MostrarMensaje("Marca agregada correctamente.", true);

                    LimpiarCampos();
                    CargarGrilla();
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627 || ex.Number == 2601)
                    {
                        MostrarMensaje("Ya existe una marca registrada con ese código.", false);
                    }
                    else
                    {
                        MostrarMensaje("Error al agregar la marca: " + ex.Message, false);
                    }
                }
            }
        }

        // MODIFICAR MARCA
        protected void btnModificar_Click(object sender, EventArgs e)
        {
            if (!RequiereAdministrador())
                return;

            if (string.IsNullOrWhiteSpace(txtCodigoMarca.Text) ||
                string.IsNullOrWhiteSpace(txtNombreMarca.Text))
            {
                MostrarMensaje("Debe seleccionar una marca e ingresar su nombre.", false);
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"
                    UPDATE dbo.Marcas
                    SET Nombre = @Nombre
                    WHERE Codigo = @Codigo AND Estado = 1";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.Add("@Codigo", SqlDbType.VarChar, 20).Value =
                    txtCodigoMarca.Text.Trim();

                cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 50).Value =
                    txtNombreMarca.Text.Trim();

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
                        MostrarMensaje("No se encontró la marca activa.", false);
                    }
                }
                catch (SqlException ex)
                {
                    MostrarMensaje("Error al modificar la marca: " + ex.Message, false);
                }
            }

            CargarGrilla();
        }

        // ELIMINAR MARCA
        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            if (!RequiereAdministrador())
                return;

            if (string.IsNullOrWhiteSpace(txtCodigoMarca.Text))
            {
                MostrarMensaje("Seleccione una marca para eliminar.", false);
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"
                    UPDATE dbo.Marcas
                    SET Estado = 0
                    WHERE Codigo = @Codigo AND Estado = 1";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.Add("@Codigo", SqlDbType.VarChar, 20).Value =
                    txtCodigoMarca.Text.Trim();

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
                        MostrarMensaje("No se encontró la marca activa.", false);
                    }
                }
                catch (SqlException ex)
                {
                    MostrarMensaje("Error al eliminar la marca: " + ex.Message, false);
                }
            }

            CargarGrilla();
        }

        // VER MARCAS INACTIVAS
        protected void chkVerInactivos_CheckedChanged(object sender, EventArgs e)
        {
            if (!RequiereAdministrador())
                return;

            LimpiarCampos();
            CargarGrilla();
        }

        // REACTIVAR MARCA
        protected void btnReactivar_Click(object sender, EventArgs e)
        {
            if (!RequiereAdministrador())
                return;

            if (string.IsNullOrWhiteSpace(txtCodigoMarca.Text))
            {
                MostrarMensaje("Seleccione una marca inactiva para reactivar.", false);
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"
                    UPDATE dbo.Marcas
                    SET Estado = 1
                    WHERE Codigo = @Codigo AND Estado = 0";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.Add("@Codigo", SqlDbType.VarChar, 20).Value =
                    txtCodigoMarca.Text.Trim();

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
                        MostrarMensaje("No se encontró la marca inactiva.", false);
                    }
                }
                catch (SqlException ex)
                {
                    MostrarMensaje("Error al reactivar la marca: " + ex.Message, false);
                }
            }

            CargarGrilla();
        }

        // LIMPIAR FORMULARIO
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            if (!RequiereAdministrador())
                return;

            LimpiarCampos();
            MostrarMensaje("", true);
        }

        // LIMPIAR CAMPOS
        private void LimpiarCampos()
        {
            txtCodigoMarca.Text = "";
            txtCodigoMarca.ReadOnly = false;
            txtNombreMarca.Text = "";
            txtBuscarNombreDinamico.Text = "";

            dgvMarcas.SelectedIndex = -1;
        }

        // MOSTRAR MENSAJES
        private void MostrarMensaje(string mensaje, bool correcto)
        {
            lblMensaje.Text = HttpUtility.HtmlEncode(mensaje);

            lblMensaje.CssClass = correcto
                ? "ms-3 fw-bold text-success"
                : "ms-3 fw-bold text-danger";
        }
    }
}
