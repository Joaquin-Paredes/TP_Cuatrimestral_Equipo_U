using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;

namespace Gestión_Comercio_TP_Cuatrimestral_Equipo_U
{
    public partial class Registro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnCrearCuenta_Click(object sender, EventArgs e)
        {
            string correo = txtEmail.Text.Trim();
            string nombreUsuario = txtUsuario.Text.Trim();
            string contrasenia = txtContrasena.Text.Trim();
            string confirmar = txtConfirmarContrasena.Text.Trim();

            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(contrasenia))
            {
                lblMensaje.ForeColor = System.Drawing.Color.Red;
                lblMensaje.Text = "Completá el correo, el usuario y la contraseña.";
                return;
            }

            if (contrasenia != confirmar)
            {
                lblMensaje.ForeColor = System.Drawing.Color.Red;
                lblMensaje.Text = "Las contraseñas no coinciden.";
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string checkQuery = "SELECT COUNT(1) FROM Usuarios WHERE Email = @Email";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Email", correo);
                        int existe = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (existe > 0)
                        {
                            lblMensaje.ForeColor = System.Drawing.Color.Red;
                            lblMensaje.Text = "Ya existe una cuenta registrada con ese correo.";
                            return;
                        }
                    }

                    string insertQuery = "INSERT INTO Usuarios (NombreUsuario, Email, Contrasenia, EsAdmin, Estado) VALUES (@NombreUsuario, @Email, @Contrasenia, 0, 1)";
                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                        insertCmd.Parameters.AddWithValue("@Email", correo);
                        insertCmd.Parameters.AddWithValue("@Contrasenia", contrasenia);
                        insertCmd.ExecuteNonQuery();
                    }
                }

                lblMensaje.ForeColor = System.Drawing.Color.Green;
                lblMensaje.Text = "Cuenta creada correctamente. Ya podés iniciar sesión.";
            }
            catch (Exception ex)
            {
                lblMensaje.ForeColor = System.Drawing.Color.Red;
                lblMensaje.Text = "Error al registrar usuario: " + ex.Message;
            }
        }

    }
}