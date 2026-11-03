using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gestión_Comercio_TP_Cuatrimestral_Equipo_U
{
    public partial class Productos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDesplegables();
                CargarGrilla();
            }
        }

        private void CargarDesplegables()
        {
            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            // Cargar Marcas
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SELECT Id, Nombre FROM Marcas", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ddlMarca.DataSource = dt;
                ddlMarca.DataTextField = "Nombre";
                ddlMarca.DataValueField = "Id";
                ddlMarca.DataBind();
            }

            // Cargar Categorías
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SELECT Id, Nombre FROM Categorias", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ddlCategoria.DataSource = dt;
                ddlCategoria.DataTextField = "Nombre";
                ddlCategoria.DataValueField = "Id";
                ddlCategoria.DataBind();
            }
        }

        private void CargarGrilla()
        {
            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT P.Codigo, P.Nombre, M.Nombre AS Marca, C.Nombre AS Categoria, P.Precio, P.Stock " +
                               "FROM Productos P " +
                               "INNER JOIN Marcas M ON P.IdMarca = M.Id " +
                               "INNER JOIN Categorias C ON P.IdCategoria = C.Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                dgvProductos.DataSource = cmd.ExecuteReader();
                dgvProductos.DataBind();
            }
        }

        protected void dgvProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string codigo = dgvProductos.SelectedDataKey.Value.ToString();
            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT Codigo, Nombre, Precio, Stock, IdMarca, IdCategoria FROM Productos WHERE Codigo = @Codigo";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Codigo", codigo);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtCodigoProducto.Text = reader["Codigo"].ToString();
                    txtCodigoProducto.ReadOnly = true; // Bloquear clave primaria para evitar modificaciones erróneas

                    txtNombreProducto.Text = reader["Nombre"].ToString();
                    txtPrecio.Text = reader["Precio"].ToString();
                    txtStock.Text = reader["Stock"].ToString();

                    ddlMarca.SelectedValue = reader["IdMarca"].ToString();
                    ddlCategoria.SelectedValue = reader["IdCategoria"].ToString();
                }
                reader.Close();
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            // Validación: si el código está vacío, no ejecutamos la inserción
            if (string.IsNullOrWhiteSpace(txtCodigoProducto.Text))
            {
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "INSERT INTO Productos (Codigo, Nombre, Precio, Stock, IdMarca, IdCategoria) " +
                               "VALUES (@Codigo, @Nombre, @Precio, @Stock, @IdMarca, @IdCategoria)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Codigo", txtCodigoProducto.Text);
                cmd.Parameters.AddWithValue("@Nombre", txtNombreProducto.Text);

                // Parseo seguro de Precio y Stock
                decimal precio = 0;
                decimal.TryParse(txtPrecio.Text.Replace('.', ','), out precio);
                cmd.Parameters.AddWithValue("@Precio", precio);

                int stock = 0;
                int.TryParse(txtStock.Text, out stock);
                cmd.Parameters.AddWithValue("@Stock", stock);

                cmd.Parameters.AddWithValue("@IdMarca", int.Parse(ddlMarca.SelectedValue));
                cmd.Parameters.AddWithValue("@IdCategoria", int.Parse(ddlCategoria.SelectedValue));

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    CargarGrilla();
                    LimpiarControles();
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627 || ex.Number == 2601 || ex.Message.Contains("PRIMARY KEY"))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "duplicado", "alert('Atención: Ya existe un producto registrado con el código ingresado.');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "errorSQL", $"alert('Ocurrió un error en la base de datos: {ex.Message.Replace("'", "")}');", true);
                    }
                }
            }
        }

        protected void btnModificar_Click(object sender, EventArgs e)
        {
            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "UPDATE Productos SET Nombre = @Nombre, Precio = @Precio, Stock = @Stock, IdMarca = @IdMarca, IdCategoria = @IdCategoria " +
                               "WHERE Codigo = @Codigo";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Codigo", txtCodigoProducto.Text);
                cmd.Parameters.AddWithValue("@Nombre", txtNombreProducto.Text);

                // Parseo seguro de Precio y Stock
                decimal precio = 0;
                decimal.TryParse(txtPrecio.Text.Replace('.', ','), out precio);
                cmd.Parameters.AddWithValue("@Precio", precio);

                int stock = 0;
                int.TryParse(txtStock.Text, out stock);
                cmd.Parameters.AddWithValue("@Stock", stock);

                cmd.Parameters.AddWithValue("@IdMarca", int.Parse(ddlMarca.SelectedValue));
                cmd.Parameters.AddWithValue("@IdCategoria", int.Parse(ddlCategoria.SelectedValue));

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            CargarGrilla();
            LimpiarControles();
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Productos WHERE Codigo = @Codigo", conn);
                cmd.Parameters.AddWithValue("@Codigo", txtCodigoProducto.Text);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            CargarGrilla();
            LimpiarControles();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }

        private void LimpiarControles()
        {
            txtCodigoProducto.Text = "";
            txtCodigoProducto.ReadOnly = false; // Rehabilitar para nuevos registros
            txtNombreProducto.Text = "";
            txtPrecio.Text = "";
            txtStock.Text = "";
            if (ddlMarca.Items.Count > 0) ddlMarca.SelectedIndex = 0;
            if (ddlCategoria.Items.Count > 0) ddlCategoria.SelectedIndex = 0;
        }
    }
}