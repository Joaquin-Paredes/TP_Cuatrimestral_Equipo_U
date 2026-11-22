
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
        }
    }
}
