using System;
using System.Data.SqlClient;
using System.Configuration;

namespace Gestión_Comercio_TP_Cuatrimestral_Equipo_U
{
    public partial class Marcas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) { CargarGrilla(); }
        }

        private void CargarGrilla()
        {
            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Marcas", conn);
                conn.Open();
                dgvMarcas.DataSource = cmd.ExecuteReader();
                dgvMarcas.DataBind();
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Marcas (Codigo, Nombre) VALUES (@Codigo, @Nombre)", conn);
                cmd.Parameters.AddWithValue("@Codigo", txtCodigoMarca.Text);
                cmd.Parameters.AddWithValue("@Nombre", txtNombreMarca.Text);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            CargarGrilla();
        }

        protected void btnModificar_Click(object sender, EventArgs e)
        {
            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Marcas SET Nombre = @Nombre WHERE Codigo = @Codigo", conn);
                cmd.Parameters.AddWithValue("@Codigo", txtCodigoMarca.Text);
                cmd.Parameters.AddWithValue("@Nombre", txtNombreMarca.Text);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            CargarGrilla();
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Marcas WHERE Codigo = @Codigo", conn);
                cmd.Parameters.AddWithValue("@Codigo", txtCodigoMarca.Text);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            CargarGrilla();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtCodigoMarca.Text = "";
            txtNombreMarca.Text = "";
        }
    }
}