using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient; 

namespace Gestión_Comercio_TP_Cuatrimestral_Equipo_U
{
    public partial class Marcas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                SqlConnection conexion = new SqlConnection();
                SqlCommand comando = new SqlCommand();
                SqlDataReader lector;

                try
                {
                   
                    conexion.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["GestionComercioDB"].ConnectionString;

                   
                    comando.CommandType = System.Data.CommandType.Text;
                    comando.CommandText = "SELECT Id, Nombre FROM Marcas";
                    comando.Connection = conexion;

                   
                    conexion.Open();
                    lector = comando.ExecuteReader();

                    
                    dgvMarcas.DataSource = lector;
                    dgvMarcas.DataBind();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                  
                    conexion.Close();
                }
            }
        }
    }
}