
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.UI;

namespace Gestión_Comercio_TP_Cuatrimestral_Equipo_U
{
    public partial class Clientes : System.Web.UI.Page
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
                    SELECT
                        CUIT AS Codigo,
                        NombreCompleto AS Nombre,
                        Email,
                        Telefono,
                        Direccion AS Domicilio
                    FROM dbo.Clientes
                    WHERE Estado = @Estado
                    ORDER BY NombreCompleto";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.Add("@Estado", SqlDbType.Bit).Value = estado;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                dgvClientes.DataSource = dt;
                dgvClientes.DataBind();
            }
        }

     
        protected void dgvClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedDataKey == null)
                return;

            string cuit = dgvClientes.SelectedDataKey.Value.ToString();

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"
                    SELECT CUIT, NombreCompleto, Email, Telefono, Direccion
                    FROM dbo.Clientes
                    WHERE CUIT = @CUIT";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.Add("@CUIT", SqlDbType.VarChar, 20).Value = cuit;

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtCodigoCliente.Text = reader["CUIT"].ToString();
                        txtCodigoCliente.ReadOnly = true;

                        txtNombre.Text = reader["NombreCompleto"].ToString();
                        txtEmail.Text = reader["Email"] == DBNull.Value ? "" : reader["Email"].ToString();
                        txtTelefono.Text = reader["Telefono"] == DBNull.Value ? "" : reader["Telefono"].ToString();
                        txtDomicilio.Text = reader["Direccion"] == DBNull.Value ? "" : reader["Direccion"].ToString();
                    }
                }
            }
        }

        private bool ValidarCliente()
        {
            if (string.IsNullOrWhiteSpace(txtCodigoCliente.Text))
            {
                MostrarMensaje("Debe ingresar el CUIT del cliente.", false);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MostrarMensaje("Debe ingresar el nombre completo del cliente.", false);
                return false;
            }

            return true;
        }

  
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            if (!ValidarCliente())
                return;

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"
                    INSERT INTO dbo.Clientes
                    (CUIT, NombreCompleto, Email, Telefono, Direccion, Estado)
                    VALUES
                    (@CUIT, @NombreCompleto, @Email, @Telefono, @Direccion, 1)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.Add("@CUIT", SqlDbType.VarChar, 20).Value =
                    txtCodigoCliente.Text.Trim();

                cmd.Parameters.Add("@NombreCompleto", SqlDbType.VarChar, 100).Value =
                    txtNombre.Text.Trim();

                cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value =
                    string.IsNullOrWhiteSpace(txtEmail.Text)
                    ? (object)DBNull.Value : txtEmail.Text.Trim();

                cmd.Parameters.Add("@Telefono", SqlDbType.VarChar, 20).Value =
                    string.IsNullOrWhiteSpace(txtTelefono.Text)
                    ? (object)DBNull.Value : txtTelefono.Text.Trim();

                cmd.Parameters.Add("@Direccion", SqlDbType.VarChar, 150).Value =
                    string.IsNullOrWhiteSpace(txtDomicilio.Text)
                    ? (object)DBNull.Value : txtDomicilio.Text.Trim();

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MostrarMensaje("Cliente agregado correctamente.", true);

                    LimpiarControles();
                    CargarGrilla();
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627 || ex.Number == 2601)
                    {
                        MostrarMensaje("Ya existe un cliente registrado con ese CUIT.", false);
                    }
                    else
                    {
                        MostrarMensaje("Error al agregar el cliente: " + ex.Message, false);
                    }
                }
            }
        }

      
        protected void btnModificar_Click(object sender, EventArgs e)
        {
            if (!ValidarCliente())
                return;

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"
                    UPDATE dbo.Clientes
                    SET NombreCompleto = @NombreCompleto,
                        Email = @Email,
                        Telefono = @Telefono,
                        Direccion = @Direccion
                    WHERE CUIT = @CUIT AND Estado = 1";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.Add("@CUIT", SqlDbType.VarChar, 20).Value =
                    txtCodigoCliente.Text.Trim();

                cmd.Parameters.Add("@NombreCompleto", SqlDbType.VarChar, 100).Value =
                    txtNombre.Text.Trim();

                cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value =
                    string.IsNullOrWhiteSpace(txtEmail.Text)
                    ? (object)DBNull.Value : txtEmail.Text.Trim();

                cmd.Parameters.Add("@Telefono", SqlDbType.VarChar, 20).Value =
                    string.IsNullOrWhiteSpace(txtTelefono.Text)
                    ? (object)DBNull.Value : txtTelefono.Text.Trim();

                cmd.Parameters.Add("@Direccion", SqlDbType.VarChar, 150).Value =
                    string.IsNullOrWhiteSpace(txtDomicilio.Text)
                    ? (object)DBNull.Value : txtDomicilio.Text.Trim();

                conn.Open();

                int filas = cmd.ExecuteNonQuery();

                if (filas > 0)
                {
                    MostrarMensaje("Cliente modificado correctamente.", true);

                    LimpiarControles();
                    CargarGrilla();
                }
                else
                {
                    MostrarMensaje("No se encontró el cliente activo que desea modificar.", false);
                }
            }
        }

      
        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigoCliente.Text))
            {
                MostrarMensaje("Seleccione un cliente para eliminar.", false);
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"
                    UPDATE dbo.Clientes
                    SET Estado = 0
                    WHERE CUIT = @CUIT AND Estado = 1";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.Add("@CUIT", SqlDbType.VarChar, 20).Value =
                    txtCodigoCliente.Text.Trim();

                conn.Open();

                try
                {
                    int filas = cmd.ExecuteNonQuery();

                    if (filas > 0)
                    {
                        MostrarMensaje("Cliente eliminado correctamente.", true);

                        LimpiarControles();
                        CargarGrilla();
                    }
                    else
                    {
                        MostrarMensaje("No se encontró el cliente activo que desea eliminar.", false);
                    }
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 547)
                    {
                        MostrarMensaje("No se puede eliminar el cliente porque tiene registros relacionados.", false);
                    }
                    else
                    {
                        MostrarMensaje("Error al eliminar el cliente: " + ex.Message, false);
                    }
                }
            }
        }

     
        protected void chkVerInactivos_CheckedChanged(object sender, EventArgs e)
        {
            LimpiarControles();
            CargarGrilla();
        }

    
        protected void btnReactivar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigoCliente.Text))
            {
                MostrarMensaje("Seleccione un cliente inactivo para reactivar.", false);
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"
                    UPDATE dbo.Clientes
                    SET Estado = 1
                    WHERE CUIT = @CUIT AND Estado = 0";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.Add("@CUIT", SqlDbType.VarChar, 20).Value =
                    txtCodigoCliente.Text.Trim();

                try
                {
                    conn.Open();

                    int filas = cmd.ExecuteNonQuery();

                    if (filas > 0)
                    {
                        MostrarMensaje("Cliente reactivado correctamente.", true);

                        chkVerInactivos.Checked = false;
                        LimpiarControles();
                        CargarGrilla();
                    }
                    else
                    {
                        MostrarMensaje("No se encontró el cliente inactivo.", false);
                    }
                }
                catch (SqlException ex)
                {
                    MostrarMensaje("Error al reactivar el cliente: " + ex.Message, false);
                }
            }
        }

   
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
            MostrarMensaje("", true);
        }

        private void LimpiarControles()
        {
            txtCodigoCliente.Text = "";
            txtCodigoCliente.ReadOnly = false;

            txtNombre.Text = "";
            txtEmail.Text = "";
            txtTelefono.Text = "";
            txtDomicilio.Text = "";

            dgvClientes.SelectedIndex = -1;
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
