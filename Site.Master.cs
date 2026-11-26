
using Gestión_Comercio_TP_Cuatrimestral_Equipo_U.Dominio;
using System;
using System.Web;
using System.Web.UI;

namespace Gestión_Comercio_TP_Cuatrimestral_Equipo_U
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (Session["Usuario"] == null)
            {
               
                Response.Redirect("~/Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                CargarDatosUsuario();
            }
        }

        private void CargarDatosUsuario()
        {
            Usuario usuarioActual = Session["Usuario"] as Usuario;
            if (usuarioActual != null)
            {
                lblUsuarioLogueado.Text = usuarioActual.NombreUsuario;

                if (usuarioActual.EsAdmin)
                {
                    lblRolUsuario.Text = "Admin";
                    lblRolUsuario.CssClass = "badge bg-warning text-dark";
                }
                else
                {
                    lblRolUsuario.Text = "Usuario";
                    lblRolUsuario.CssClass = "badge bg-info text-dark";
                }
            }
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
    

