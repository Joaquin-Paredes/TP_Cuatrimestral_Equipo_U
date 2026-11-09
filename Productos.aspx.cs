
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
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

            using (SqlConnection conn = new SqlConnection(connString))
            {
             
                SqlDataAdapter daMarcas = new SqlDataAdapter(
                    "SELECT Id, Nombre FROM Marcas ORDER BY Nombre", conn);

                DataTable dtMarcas = new DataTable();
                daMarcas.Fill(dtMarcas);

                ddlMarca.DataSource = dtMarcas;
                ddlMarca.DataTextField = "Nombre";
                ddlMarca.DataValueField = "Id";
                ddlMarca.DataBind();

                ddlMarca.Items.Insert(0, new ListItem("Seleccione una marca", "0"));

                ddlFiltroMarca.DataSource = dtMarcas;
                ddlFiltroMarca.DataTextField = "Nombre";
                ddlFiltroMarca.DataValueField = "Id";
                ddlFiltroMarca.DataBind();

                ddlFiltroMarca.Items.Insert(0, new ListItem("Todas las marcas", "0"));

                
                SqlDataAdapter daCategorias = new SqlDataAdapter(
                    "SELECT Id, Nombre FROM Categorias ORDER BY Nombre", conn);

                DataTable dtCategorias = new DataTable();
                daCategorias.Fill(dtCategorias);

                ddlCategoria.DataSource = dtCategorias;
                ddlCategoria.DataTextField = "Nombre";
                ddlCategoria.DataValueField = "Id";
                ddlCategoria.DataBind();

                ddlCategoria.Items.Insert(0, new ListItem("Seleccione una categoría", "0"));

                ddlFiltroCategoria.DataSource = dtCategorias;
                ddlFiltroCategoria.DataTextField = "Nombre";
                ddlFiltroCategoria.DataValueField = "Id";
                ddlFiltroCategoria.DataBind();

                ddlFiltroCategoria.Items.Insert(0, new ListItem("Todas las categorías", "0"));
            }
        }


        private void CargarGrilla()
        {
            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"
                    SELECT
                        P.Codigo,
                        P.Nombre,
                        M.Nombre AS Marca,
                        C.Nombre AS Categoria,
                        P.Precio,
                        P.Stock,
                        P.StockMinimo
                    FROM Productos P
                    INNER JOIN Marcas M ON P.IdMarca = M.Id
                    INNER JOIN Categorias C ON P.IdCategoria = C.Id
                    WHERE
                        (@Texto = '' OR P.Codigo LIKE '%' + @Texto + '%'
                        OR P.Nombre LIKE '%' + @Texto + '%')
                        AND (@IdMarca = 0 OR P.IdMarca = @IdMarca)
                        AND (@IdCategoria = 0 OR P.IdCategoria = @IdCategoria)
                    ORDER BY P.Nombre";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.Add("@Texto", SqlDbType.VarChar, 120).Value =
                    txtBuscar.Text.Trim();

                int idMarca = 0;
                int.TryParse(ddlFiltroMarca.SelectedValue, out idMarca);

                cmd.Parameters.Add("@IdMarca", SqlDbType.Int).Value = idMarca;

                int idCategoria = 0;
                int.TryParse(ddlFiltroCategoria.SelectedValue, out idCategoria);

                cmd.Parameters.Add("@IdCategoria", SqlDbType.Int).Value = idCategoria;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                dgvProductos.DataSource = dt;
                dgvProductos.DataBind();
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            OcultarDetalle();
            CargarGrilla();
        }


        protected void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            txtBuscar.Text = "";

            if (ddlFiltroMarca.Items.Count > 0)
                ddlFiltroMarca.SelectedIndex = 0;

            if (ddlFiltroCategoria.Items.Count > 0)
                ddlFiltroCategoria.SelectedIndex = 0;

            dgvProductos.SelectedIndex = -1;

            OcultarDetalle();
            CargarGrilla();
        }

  
        protected void dgvProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedDataKey == null)
                return;

            string codigo = dgvProductos.SelectedDataKey.Value.ToString();

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"
                    SELECT
                        P.Codigo,
                        P.Nombre,
                        M.Nombre AS Marca,
                        C.Nombre AS Categoria,
                        P.Precio,
                        P.Stock,
                        P.StockMinimo,
                        P.IdMarca,
                        P.IdCategoria
                    FROM Productos P
                    INNER JOIN Marcas M ON P.IdMarca = M.Id
                    INNER JOIN Categorias C ON P.IdCategoria = C.Id
                    WHERE P.Codigo = @Codigo";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.Add("@Codigo", SqlDbType.VarChar, 20).Value = codigo;

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                      
                        txtCodigoProducto.Text = reader["Codigo"].ToString();
                        txtCodigoProducto.ReadOnly = true;

                        txtNombreProducto.Text = reader["Nombre"].ToString();
                        txtPrecio.Text = reader["Precio"].ToString();
                        txtStock.Text = reader["Stock"].ToString();
                        txtStockMinimo.Text = reader["StockMinimo"].ToString();

                        string idMarca = reader["IdMarca"].ToString();
                        string idCategoria = reader["IdCategoria"].ToString();

                        if (ddlMarca.Items.FindByValue(idMarca) != null)
                            ddlMarca.SelectedValue = idMarca;

                        if (ddlCategoria.Items.FindByValue(idCategoria) != null)
                            ddlCategoria.SelectedValue = idCategoria;

                  
                        lblDetalleCodigo.Text = reader["Codigo"].ToString();
                        lblDetalleNombre.Text = reader["Nombre"].ToString();
                        lblDetalleMarca.Text = reader["Marca"].ToString();
                        lblDetalleCategoria.Text = reader["Categoria"].ToString();
                        lblDetallePrecio.Text = reader["Precio"].ToString();
                        lblDetalleStock.Text = reader["Stock"].ToString();
                        lblDetalleStockMinimo.Text = reader["StockMinimo"].ToString();

                        pnlDetalleProducto.Visible = true;
                    }
                }
            }
        }

        private bool ValidarProducto(
            out decimal precio,
            out int stock,
            out int stockMinimo,
            out int idMarca,
            out int idCategoria)
        {
            precio = 0;
            stock = 0;
            stockMinimo = 0;
            idMarca = 0;
            idCategoria = 0;

            if (string.IsNullOrWhiteSpace(txtCodigoProducto.Text) ||
                string.IsNullOrWhiteSpace(txtNombreProducto.Text))
            {
                MostrarMensaje("Debe ingresar el código y el nombre del producto.");
                return false;
            }

            if (!decimal.TryParse(txtPrecio.Text.Trim(), NumberStyles.Number,
                    CultureInfo.CurrentCulture, out precio) &&
                !decimal.TryParse(txtPrecio.Text.Trim(), NumberStyles.Number,
                    CultureInfo.InvariantCulture, out precio))
            {
                MostrarMensaje("Ingrese un precio válido.");
                return false;
            }

            if (precio < 0)
            {
                MostrarMensaje("El precio no puede ser negativo.");
                return false;
            }

            if (!int.TryParse(txtStock.Text, out stock) || stock < 0)
            {
                MostrarMensaje("Ingrese una cantidad de stock válida.");
                return false;
            }

            if (!int.TryParse(txtStockMinimo.Text, out stockMinimo) || stockMinimo < 0)
            {
                MostrarMensaje("Ingrese un stock mínimo válido.");
                return false;
            }

            if (!int.TryParse(ddlMarca.SelectedValue, out idMarca) || idMarca <= 0)
            {
                MostrarMensaje("Seleccione una marca.");
                return false;
            }

            if (!int.TryParse(ddlCategoria.SelectedValue, out idCategoria) || idCategoria <= 0)
            {
                MostrarMensaje("Seleccione una categoría.");
                return false;
            }

            return true;
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            decimal precio;
            int stock;
            int stockMinimo;
            int idMarca;
            int idCategoria;

            if (!ValidarProducto(out precio, out stock, out stockMinimo, out idMarca, out idCategoria))
                return;

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"
                    INSERT INTO Productos
                    (Codigo, Nombre, Precio, Stock, StockMinimo, IdMarca, IdCategoria)
                    VALUES
                    (@Codigo, @Nombre, @Precio, @Stock, @StockMinimo, @IdMarca, @IdCategoria)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.Add("@Codigo", SqlDbType.VarChar, 20).Value =
                    txtCodigoProducto.Text.Trim();

                cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 100).Value =
                    txtNombreProducto.Text.Trim();

                cmd.Parameters.Add("@Precio", SqlDbType.Decimal).Value = precio;
                cmd.Parameters["@Precio"].Precision = 18;
                cmd.Parameters["@Precio"].Scale = 2;

                cmd.Parameters.Add("@Stock", SqlDbType.Int).Value = stock;
                cmd.Parameters.Add("@StockMinimo", SqlDbType.Int).Value = stockMinimo;
                cmd.Parameters.Add("@IdMarca", SqlDbType.Int).Value = idMarca;
                cmd.Parameters.Add("@IdCategoria", SqlDbType.Int).Value = idCategoria;

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MostrarMensaje("Producto agregado correctamente.");

                    LimpiarControles();
                    CargarGrilla();
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627 || ex.Number == 2601)
                    {
                        MostrarMensaje("Ya existe un producto registrado con ese código.");
                    }
                    else
                    {
                        MostrarMensaje("Ocurrió un error al agregar el producto.");
                    }
                }
            }
        }

        protected void btnModificar_Click(object sender, EventArgs e)
        {
            decimal precio;
            int stock;
            int stockMinimo;
            int idMarca;
            int idCategoria;

            if (!ValidarProducto(out precio, out stock, out stockMinimo, out idMarca, out idCategoria))
                return;

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"
                    UPDATE Productos
                    SET Nombre = @Nombre,
                        Precio = @Precio,
                        Stock = @Stock,
                        StockMinimo = @StockMinimo,
                        IdMarca = @IdMarca,
                        IdCategoria = @IdCategoria
                    WHERE Codigo = @Codigo";

                SqlCommand cmd = new SqlCommand(query, conn);
               
                cmd.Parameters.Add("@Codigo", SqlDbType.VarChar, 20).Value =
                    txtCodigoProducto.Text.Trim();

                cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 100).Value =
                    txtNombreProducto.Text.Trim();

                cmd.Parameters.Add("@Precio", SqlDbType.Decimal).Value = precio;
                cmd.Parameters["@Precio"].Precision = 18;
                cmd.Parameters["@Precio"].Scale = 2;

                cmd.Parameters.Add("@Stock", SqlDbType.Int).Value = stock;
                cmd.Parameters.Add("@StockMinimo", SqlDbType.Int).Value = stockMinimo;
                cmd.Parameters.Add("@IdMarca", SqlDbType.Int).Value = idMarca;
                cmd.Parameters.Add("@IdCategoria", SqlDbType.Int).Value = idCategoria;

                conn.Open();

                int filas = cmd.ExecuteNonQuery();

                if (filas > 0)
                {
                    MostrarMensaje("Producto modificado correctamente.");

                    LimpiarControles();
                    CargarGrilla();
                }
                else
                {
                    MostrarMensaje("No se encontró el producto que desea modificar.");
                }
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigoProducto.Text))
            {
                MostrarMensaje("Seleccione un producto para eliminar.");
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "DELETE FROM Productos WHERE Codigo = @Codigo";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.Add("@Codigo", SqlDbType.VarChar, 20).Value =
                    txtCodigoProducto.Text.Trim();

                conn.Open();

                int filas = cmd.ExecuteNonQuery();

                if (filas > 0)
                {
                    MostrarMensaje("Producto eliminado correctamente.");

                    LimpiarControles();
                    CargarGrilla();
                }
                else
                {
                    MostrarMensaje("No se encontró el producto que desea eliminar.");
                }
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }

        private void LimpiarControles()
        {
            txtCodigoProducto.Text = "";
            txtCodigoProducto.ReadOnly = false;

            txtNombreProducto.Text = "";
            txtPrecio.Text = "";
            txtStock.Text = "";
            txtStockMinimo.Text = "";

            if (ddlMarca.Items.Count > 0)
                ddlMarca.SelectedIndex = 0;

            if (ddlCategoria.Items.Count > 0)
                ddlCategoria.SelectedIndex = 0;

            dgvProductos.SelectedIndex = -1;

            OcultarDetalle();
        }

        private void OcultarDetalle()
        {
            pnlDetalleProducto.Visible = false;
        }

        private void MostrarMensaje(string mensaje)
        {
            string mensajeSeguro = mensaje
                .Replace("\\", "\\\\")
                .Replace("'", "\\'")
                .Replace("\r", " ")
                .Replace("\n", " ");

            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                Guid.NewGuid().ToString(),
                "alert('" + mensajeSeguro + "');",
                true);
        }
    }
}
