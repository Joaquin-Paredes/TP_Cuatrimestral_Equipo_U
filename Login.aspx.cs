using Gestión_Comercio_TP_Cuatrimestral_Equipo_U.Dominio;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestión_Comercio_TP_Cuatrimestral_Equipo_U
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            string correo = txtEmail.Text.Trim();
            string contrasenia = txtContrasena.Text.Trim();

            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(contrasenia))
            {
                lblMensaje.Text = "Completá el correo electrónico y la contraseña.";
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            
            string query = @"
                SELECT Id, NombreUsuario, Contrasenia, EsAdmin
                FROM Usuarios
                WHERE Email = @Email AND Contrasenia = @Contrasenia";

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", correo);
                    cmd.Parameters.AddWithValue("@Contrasenia", contrasenia);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Usuario usuarioLogueado = new Usuario
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                NombreUsuario = reader["NombreUsuario"].ToString(),
                                Contrasenia = reader["Contrasenia"].ToString(),
                                EsAdmin = reader["EsAdmin"] != DBNull.Value && Convert.ToBoolean(reader["EsAdmin"])
                            };

                            Session["Usuario"] = usuarioLogueado;
                            Response.Redirect("~/Default.aspx", false);
                            Context.ApplicationInstance.CompleteRequest();
                        }
                        else
                        {
                            lblMensaje.Text = "Correo o contraseña incorrectos.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al iniciar sesión: " + ex.Message;
            }
        }
    }
}
    
