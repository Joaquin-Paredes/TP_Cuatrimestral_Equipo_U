
using Gestión_Comercio_TP_Cuatrimestral_Equipo_U.Dominio;
using System;
using System.IO;
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

            Usuario usuarioActual = Session["Usuario"] as Usuario;
            if (usuarioActual != null)
            {
                ConfigurarMenuYPermisos(usuarioActual);
            }
        }

        private void ConfigurarMenuYPermisos(Usuario usuarioActual)
        {
            bool esAdmin = usuarioActual.EsAdmin;

            lblUsuarioLogueado.Text = usuarioActual.NombreUsuario;

            if (esAdmin)
            {
                lblRolUsuario.Text = "Admin";
                lblRolUsuario.CssClass = "badge bg-warning text-dark";
            }
            else
            {
                lblRolUsuario.Text = "Usuario";
                lblRolUsuario.CssClass = "badge bg-info text-dark";
            }

            liProductos.Visible = esAdmin;
            liMarcas.Visible = esAdmin;
            liCategorias.Visible = esAdmin;
            liClientes.Visible = esAdmin;
            liProveedores.Visible = esAdmin;

            liVentas.Visible = true;
            liCompras.Visible = true;

            if (!esAdmin)
            {
                string paginaActual = Path.GetFileNameWithoutExtension(Request.AppRelativeCurrentExecutionFilePath).ToLower();
                if (paginaActual == "productos" ||
                    paginaActual == "marcas" ||
                    paginaActual == "categorias" ||
                    paginaActual == "clientes" ||
                    paginaActual == "proveedores" ||
                    paginaActual == "default")
                {
                    Response.Redirect("~/Ventas.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
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
    

