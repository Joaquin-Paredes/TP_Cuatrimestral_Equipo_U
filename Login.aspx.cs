using Gestión_Comercio_TP_Cuatrimestral_Equipo_U.Dominio;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Gestión_Comercio_TP_Cuatrimestral_Equipo_U
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            string usuarioIngresado = txtEmail.Text.Trim();
            string contrasenia = txtContrasena.Text.Trim();

            if (string.IsNullOrWhiteSpace(usuarioIngresado) || string.IsNullOrWhiteSpace(contrasenia))
            {
                lblMensaje.Text = "Completá el usuario o correo y la contraseña.";
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;
            string query = "SELECT Id, NombreUsuario, Contrasenia, EsAdmin FROM Usuarios WHERE (Email = @Usuario OR NombreUsuario = @Usuario) AND Contrasenia = @Contrasenia AND Estado = 1";

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Usuario", usuarioIngresado);
                    cmd.Parameters.AddWithValue("@Contrasenia", contrasenia);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Usuario usuarioLogueado = new Usuario();
                            usuarioLogueado.Id = Convert.ToInt32(reader["Id"]);
                            usuarioLogueado.NombreUsuario = reader["NombreUsuario"].ToString();
                            usuarioLogueado.Contrasenia = reader["Contrasenia"].ToString();
                            usuarioLogueado.EsAdmin = false;

                            if (reader["EsAdmin"] != DBNull.Value)
                            {
                                usuarioLogueado.EsAdmin = Convert.ToBoolean(reader["EsAdmin"]);
                            }

                            Session["Usuario"] = usuarioLogueado;
                            Response.Redirect("~/Ventas.aspx", false);
                            Context.ApplicationInstance.CompleteRequest();
                        }
                        else
                        {
                            lblMensaje.Text = "Usuario, correo o contraseña incorrectos.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al iniciar sesión: " + ex.Message;
            }
        }

        protected void btnMostrarRecuperar_Click(object sender, EventArgs e)
        {
            pnlLogin.Visible = false;
            pnlRecuperar.Visible = true;
            lblMensajeRecuperar.Text = "";
            txtEmailRecuperar.Text = "";
        }

        protected void btnVolverLogin_Click(object sender, EventArgs e)
        {
            pnlLogin.Visible = true;
            pnlRecuperar.Visible = false;
            lblMensaje.Text = "";
            txtEmail.Text = "";
            txtContrasena.Text = "";
        }

        protected void btnRecuperar_Click(object sender, EventArgs e)
        {
            string correo = txtEmailRecuperar.Text.Trim();

            if (string.IsNullOrWhiteSpace(correo))
            {
                lblMensajeRecuperar.Text = "Ingresá un correo electrónico.";
                lblMensajeRecuperar.ForeColor = System.Drawing.Color.Red;
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;
            string query = "SELECT Contrasenia FROM Usuarios WHERE Email = @Email";

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", correo);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string claveEncontrada = reader["Contrasenia"].ToString();
                            lblMensajeRecuperar.Text = "Tu contraseña es: " + claveEncontrada;
                            lblMensajeRecuperar.ForeColor = System.Drawing.Color.Green;
                        }
                        else
                        {
                            lblMensajeRecuperar.Text = "El correo no está registrado.";
                            lblMensajeRecuperar.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensajeRecuperar.Text = "Error al buscar: " + ex.Message;
                lblMensajeRecuperar.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}