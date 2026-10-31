using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

namespace Gestión_Comercio_TP_Cuatrimestral_Equipo_U
{
    public partial class Categorias : System.Web.UI.Page
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
                SqlCommand cmd = new SqlCommand("SELECT * FROM Categorias", conn);
                conn.Open();
                dgvCategorias.DataSource = cmd.ExecuteReader();
                dgvCategorias.DataBind();
            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Categorias (Codigo, Nombre, Descripcion) VALUES (@Codigo, @Nombre, @Desc)", conn);
                cmd.Parameters.AddWithValue("@Codigo", txtCodigoCategoria.Text);
                cmd.Parameters.AddWithValue("@Nombre", txtNombreCategoria.Text);
                cmd.Parameters.AddWithValue("@Desc", txtDescripcion.Text);
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
                SqlCommand cmd = new SqlCommand("UPDATE Categorias SET Nombre = @Nombre, Descripcion = @Desc WHERE Codigo = @Codigo", conn);
                cmd.Parameters.AddWithValue("@Codigo", txtCodigoCategoria.Text);
                cmd.Parameters.AddWithValue("@Nombre", txtNombreCategoria.Text);
                cmd.Parameters.AddWithValue("@Desc", txtDescripcion.Text);
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
                SqlCommand cmd = new SqlCommand("DELETE FROM Categorias WHERE Codigo = @Codigo", conn);
                cmd.Parameters.AddWithValue("@Codigo", txtCodigoCategoria.Text);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            CargarGrilla();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtCodigoCategoria.Text = "";
            txtNombreCategoria.Text = "";
            txtDescripcion.Text = "";
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                
                SqlCommand cmd = new SqlCommand("INSERT INTO Categorias (Codigo, Nombre, Descripcion) VALUES (@Codigo, @Nombre, @Desc)", conn);
                cmd.Parameters.AddWithValue("@Codigo", txtCodigoCategoria.Text);
                cmd.Parameters.AddWithValue("@Nombre", txtNombreCategoria.Text);
                cmd.Parameters.AddWithValue("@Desc", txtDescripcion.Text);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            CargarGrilla();
        }
    }
}