
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;

namespace Gestión_Comercio_TP_Cuatrimestral_Equipo_U
{
    public partial class Proveedores : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarGrilla();
            }
        }

        private void CargarGrilla(string cuit = "")
        {
            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                int estado = chkVerInactivos.Checked ? 0 : 1;

                string query = @"
                SELECT
                    CUIT,
                    RazonSocial,
                    Telefono,
                    Email,
                    Direccion
                FROM Proveedores 
                WHERE Estado = @Estado AND (@CUIT = '' OR CUIT LIKE '%' + @CUIT + '%')
                ORDER BY RazonSocial";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.Add("@Estado", SqlDbType.Bit).Value = estado;
                cmd.Parameters.Add("@CUIT", SqlDbType.VarChar, 20).Value = cuit.Trim();

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvProveedores.DataSource = dt;
                dgvProveedores.DataBind();
            }
        }


        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string cuit = txtBuscarCuit.Text.Trim();

            if (string.IsNullOrWhiteSpace(cuit))
            {
                MostrarMensaje("Ingrese un CUIT para buscar.", false);
                return;
            }

            LimpiarControles();

            CargarGrilla(cuit);

            if (CargarProveedorEnFormulario(cuit))
            {
                MostrarMensaje("Proveedor encontrado correctamente.", true);
            }
            else if (dgvProveedores.Rows.Count > 0)
            {
                MostrarMensaje("Se encontraron coincidencias. Seleccione un proveedor del listado.", true);
            }
            else
            {
                MostrarMensaje("No se encontró ningún proveedor con ese CUIT.", false);
            }
        }


        private bool CargarProveedorEnFormulario(string cuit)
        {
            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"
                    SELECT CUIT, RazonSocial, Email, Telefono, Direccion
                    FROM Proveedores
                    WHERE CUIT = @CUIT";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.Add("@CUIT", SqlDbType.VarChar, 20).Value = cuit;

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtCUIT.Text = reader["CUIT"].ToString();
                        txtCUIT.ReadOnly = true;

                        txtRazonSocial.Text = reader["RazonSocial"].ToString();

                        txtEmail.Text = reader["Email"] == DBNull.Value
                            ? "" : reader["Email"].ToString();

                        txtTelefono.Text = reader["Telefono"] == DBNull.Value
                            ? "" : reader["Telefono"].ToString();

                        txtDireccion.Text = reader["Direccion"] == DBNull.Value
                            ? "" : reader["Direccion"].ToString();

                        return true;
                    }
                }
            }

            return false;
        }


        protected void btnLimpiarBusqueda_Click(object sender, EventArgs e)
        {
            txtBuscarCuit.Text = "";

            LimpiarControles();

            CargarGrilla();

            MostrarMensaje("Se muestran todos los proveedores.", true);
        }


        protected void dgvProveedores_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvProveedores.SelectedDataKey == null)
                return;

            string cuit = dgvProveedores.SelectedDataKey.Value.ToString();

            if (CargarProveedorEnFormulario(cuit))
            {
                MostrarMensaje("Proveedor seleccionado correctamente.", true);
            }
        }


        private bool ValidarProveedor()
        {
            if (string.IsNullOrWhiteSpace(txtCUIT.Text))
            {
                MostrarMensaje("Debe ingresar el CUIT del proveedor.", false);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtRazonSocial.Text))
            {
                MostrarMensaje("Debe ingresar la razón social del proveedor.", false);
                return false;
            }

            return true;
        }


        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            if (!ValidarProveedor())
                return;

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"
                    INSERT INTO Proveedores
                    (CUIT, RazonSocial, Email, Telefono, Direccion)
                    VALUES
                    (@CUIT, @RazonSocial, @Email, @Telefono, @Direccion)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.Add("@CUIT", SqlDbType.VarChar, 20).Value =
                    txtCUIT.Text.Trim();

                cmd.Parameters.Add("@RazonSocial", SqlDbType.VarChar, 100).Value =
                    txtRazonSocial.Text.Trim();

                cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value =
                    string.IsNullOrWhiteSpace(txtEmail.Text)
                    ? (object)DBNull.Value : txtEmail.Text.Trim();

                cmd.Parameters.Add("@Telefono", SqlDbType.VarChar, 20).Value =
                    string.IsNullOrWhiteSpace(txtTelefono.Text)
                    ? (object)DBNull.Value : txtTelefono.Text.Trim();

                cmd.Parameters.Add("@Direccion", SqlDbType.VarChar, 150).Value =
                    string.IsNullOrWhiteSpace(txtDireccion.Text)
                    ? (object)DBNull.Value : txtDireccion.Text.Trim();

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MostrarMensaje("Proveedor agregado correctamente.", true);

                    LimpiarControles();
                    txtBuscarCuit.Text = "";
                    CargarGrilla();
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627 || ex.Number == 2601)
                    {
                        MostrarMensaje("Ya existe un proveedor registrado con ese CUIT.", false);
                    }
                    else
                    {
                        MostrarMensaje("Error al agregar el proveedor: " + ex.Message, false);
                    }
                }
            }
        }


        protected void btnModificar_Click(object sender, EventArgs e)
        {
            if (!ValidarProveedor())
                return;

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"
                    UPDATE Proveedores
                    SET RazonSocial = @RazonSocial,
                        Email = @Email,
                        Telefono = @Telefono,
                        Direccion = @Direccion
                    WHERE CUIT = @CUIT";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.Add("@CUIT", SqlDbType.VarChar, 20).Value =
                    txtCUIT.Text.Trim();

                cmd.Parameters.Add("@RazonSocial", SqlDbType.VarChar, 100).Value =
                    txtRazonSocial.Text.Trim();

                cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value =
                    string.IsNullOrWhiteSpace(txtEmail.Text)
                    ? (object)DBNull.Value : txtEmail.Text.Trim();

                cmd.Parameters.Add("@Telefono", SqlDbType.VarChar, 20).Value =
                    string.IsNullOrWhiteSpace(txtTelefono.Text)
                    ? (object)DBNull.Value : txtTelefono.Text.Trim();

                cmd.Parameters.Add("@Direccion", SqlDbType.VarChar, 150).Value =
                    string.IsNullOrWhiteSpace(txtDireccion.Text)
                    ? (object)DBNull.Value : txtDireccion.Text.Trim();

                conn.Open();

                int filas = cmd.ExecuteNonQuery();

                if (filas > 0)
                {
                    MostrarMensaje("Proveedor modificado correctamente.", true);

                    LimpiarControles();
                    txtBuscarCuit.Text = "";
                    CargarGrilla();
                }
                else
                {
                    MostrarMensaje("No se encontró el proveedor que desea modificar.", false);
                }
            }
        }


        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCUIT.Text))
            {
                MostrarMensaje("Seleccione un proveedor para eliminar.", false);
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "UPDATE Proveedores SET Estado = 0 WHERE CUIT = @CUIT";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.Add("@CUIT", SqlDbType.VarChar, 20).Value =
                    txtCUIT.Text.Trim();

                try
                {
                    conn.Open();

                    int filas = cmd.ExecuteNonQuery();

                    if (filas > 0)
                    {
                        MostrarMensaje("Proveedor eliminado correctamente.", true);

                        LimpiarControles();
                        txtBuscarCuit.Text = "";
                        CargarGrilla();
                    }
                    else
                    {
                        MostrarMensaje("No se encontró el proveedor que desea eliminar.", false);
                    }
                }
                catch (SqlException)
                {
                    MostrarMensaje("Error al eliminar el proveedor.", false);
                }
            }
        }

        protected void chkVerInactivos_CheckedChanged(object sender, EventArgs e)
        {
            LimpiarControles();
            txtBuscarCuit.Text = "";
            CargarGrilla();
        }

        protected void btnReactivar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCUIT.Text))
            {
                MostrarMensaje("Seleccione un proveedor inactivo de la lista para reactivarlo.", false);
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "UPDATE Proveedores SET Estado = 1 WHERE CUIT = @CUIT";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.Add("@CUIT", SqlDbType.VarChar, 20).Value = txtCUIT.Text.Trim();

                try
                {
                    conn.Open();
                    int filas = cmd.ExecuteNonQuery();

                    if (filas > 0)
                    {
                        MostrarMensaje("Proveedor reactivado correctamente.", true);

                        chkVerInactivos.Checked = false;
                        LimpiarControles();
                        txtBuscarCuit.Text = "";
                        CargarGrilla();
                    }
                    else
                    {
                        MostrarMensaje("No se encontró el proveedor.", false);
                    }
                }
                catch (SqlException)
                {
                    MostrarMensaje("Error al reactivar el proveedor.", false);
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
            txtCUIT.Text = "";
            txtCUIT.ReadOnly = false;

            txtRazonSocial.Text = "";
            txtEmail.Text = "";
            txtTelefono.Text = "";
            txtDireccion.Text = "";

            dgvProveedores.SelectedIndex = -1;
        }


        private void MostrarMensaje(string mensaje, bool correcto)
        {
            lblMensaje.Text = mensaje;

            lblMensaje.CssClass = correcto ? "ms-3 fw-bold text-success" : "ms-3 fw-bold text-danger";
        }
    }
}