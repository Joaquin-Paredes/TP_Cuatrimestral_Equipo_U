using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;

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
                    "SELECT Codigo, Nombre FROM Marcas ORDER BY Nombre", conn);

                DataTable dtMarcas = new DataTable();
                daMarcas.Fill(dtMarcas);

                ddlMarca.DataSource = dtMarcas;
                ddlMarca.DataTextField = "Nombre";
                ddlMarca.DataValueField = "Codigo";
                ddlMarca.DataBind();

                ddlMarca.Items.Insert(0, new ListItem("Seleccione una marca", ""));

                ddlFiltroMarca.DataSource = dtMarcas;
                ddlFiltroMarca.DataTextField = "Nombre";
                ddlFiltroMarca.DataValueField = "Codigo";
                ddlFiltroMarca.DataBind();

                ddlFiltroMarca.Items.Insert(0, new ListItem("Todas las marcas", ""));

                SqlDataAdapter daCategorias = new SqlDataAdapter(
                    "SELECT Codigo, Nombre FROM Categorias ORDER BY Nombre", conn);

                DataTable dtCategorias = new DataTable();
                daCategorias.Fill(dtCategorias);

                ddlCategoria.DataSource = dtCategorias;
                ddlCategoria.DataTextField = "Nombre";
                ddlCategoria.DataValueField = "Codigo";
                ddlCategoria.DataBind();

                ddlCategoria.Items.Insert(0, new ListItem("Seleccione una categoría", ""));

                ddlFiltroCategoria.DataSource = dtCategorias;
                ddlFiltroCategoria.DataTextField = "Nombre";
                ddlFiltroCategoria.DataValueField = "Codigo";
                ddlFiltroCategoria.DataBind();

                ddlFiltroCategoria.Items.Insert(0, new ListItem("Todas las categorías", ""));
            }
        }

        private void CargarGrilla()
        {
            int estado = chkVerInactivos.Checked ? 0 : 1;
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
                    INNER JOIN Marcas M ON P.MarcaCodigo = M.Codigo
                    INNER JOIN Categorias C ON P.CategoriaCodigo = C.Codigo
                    WHERE
                        P.Estado = @Estado AND
                        M.Estado = 1 AND
                        C.Estado = 1 AND
                        (@Texto = '' OR P.Codigo LIKE '%' + @Texto + '%'
                        OR P.Nombre LIKE '%' + @Texto + '%')
                        AND (@MarcaCodigo = '' OR P.MarcaCodigo = @MarcaCodigo)
                        AND (@CategoriaCodigo = '' OR P.CategoriaCodigo = @CategoriaCodigo)
                    ORDER BY P.Nombre";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.Add("@Estado", SqlDbType.Bit).Value = estado;

                cmd.Parameters.Add("@Texto", SqlDbType.VarChar, 120).Value =
                    txtBuscar.Text.Trim();

                cmd.Parameters.Add("@MarcaCodigo", SqlDbType.VarChar, 20).Value =
                    ddlFiltroMarca.SelectedValue;

                cmd.Parameters.Add("@CategoriaCodigo", SqlDbType.VarChar, 20).Value =
                    ddlFiltroCategoria.SelectedValue;

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

            ddlFiltroMarca.SelectedIndex = 0;
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
                        P.MarcaCodigo,
                        P.CategoriaCodigo
                    FROM Productos P
                    INNER JOIN Marcas M ON P.MarcaCodigo = M.Codigo
                    INNER JOIN Categorias C ON P.CategoriaCodigo = C.Codigo
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

                        string marcaCodigo = reader["MarcaCodigo"].ToString();
                        string categoriaCodigo = reader["CategoriaCodigo"].ToString();

                        if (ddlMarca.Items.FindByValue(marcaCodigo) != null)
                            ddlMarca.SelectedValue = marcaCodigo;

                        if (ddlCategoria.Items.FindByValue(categoriaCodigo) != null)
                            ddlCategoria.SelectedValue = categoriaCodigo;

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
            out string marcaCodigo,
            out string categoriaCodigo)
        {
            precio = 0;
            stock = 0;
            stockMinimo = 0;
            marcaCodigo = ddlMarca.SelectedValue;
            categoriaCodigo = ddlCategoria.SelectedValue;

            if (string.IsNullOrWhiteSpace(txtCodigoProducto.Text) ||
                string.IsNullOrWhiteSpace(txtNombreProducto.Text))
            {
                MostrarMensaje("Debe ingresar el código y el nombre del producto.", false);
                return false;
            }

            if (!decimal.TryParse(txtPrecio.Text.Trim(), NumberStyles.Number,
                    CultureInfo.CurrentCulture, out precio) &&
                !decimal.TryParse(txtPrecio.Text.Trim(), NumberStyles.Number,
                    CultureInfo.InvariantCulture, out precio))
            {
                MostrarMensaje("Ingrese un precio válido.", false);
                return false;
            }

            if (precio < 0)
            {
                MostrarMensaje("El precio no puede ser negativo.", false);
                return false;
            }

            if (!int.TryParse(txtStock.Text, out stock) || stock < 0)
            {
                MostrarMensaje("Ingrese una cantidad de stock válida.", false);
                return false;
            }

            if (!int.TryParse(txtStockMinimo.Text, out stockMinimo) || stockMinimo < 0)
            {
                MostrarMensaje("Ingrese un stock mínimo válido.", false);
                return false;
            }

            if (string.IsNullOrWhiteSpace(marcaCodigo))
            {
                MostrarMensaje("Seleccione una marca.", false);
                return false;
            }

            if (string.IsNullOrWhiteSpace(categoriaCodigo))
            {
                MostrarMensaje("Seleccione una categoría.", false);
                return false;
            }

            return true;
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            decimal precio;
            int stock;
            int stockMinimo;
            string marcaCodigo;
            string categoriaCodigo;

            if (!ValidarProducto(out precio, out stock, out stockMinimo,
                out marcaCodigo, out categoriaCodigo))
                return;

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"
                    INSERT INTO Productos
                    (Codigo, Nombre, Precio, Stock, StockMinimo, MarcaCodigo, CategoriaCodigo)
                    VALUES
                    (@Codigo, @Nombre, @Precio, @Stock, @StockMinimo, @MarcaCodigo, @CategoriaCodigo)";

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

                cmd.Parameters.Add("@MarcaCodigo", SqlDbType.VarChar, 20).Value = marcaCodigo;
                cmd.Parameters.Add("@CategoriaCodigo", SqlDbType.VarChar, 20).Value = categoriaCodigo;

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MostrarMensaje("Producto agregado correctamente.", true);

                    LimpiarControles();
                    CargarGrilla();
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627 || ex.Number == 2601)
                    {
                        MostrarMensaje("Ya existe un producto registrado con ese código.", false);
                    }
                    else
                    {
                        MostrarMensaje("Ocurrió un error al agregar el producto.", false);
                    }
                }
            }
        }

        protected void btnModificar_Click(object sender, EventArgs e)
        {
            decimal precio;
            int stock;
            int stockMinimo;
            string marcaCodigo;
            string categoriaCodigo;

            if (!ValidarProducto(out precio, out stock, out stockMinimo,
                out marcaCodigo, out categoriaCodigo))
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
                        MarcaCodigo = @MarcaCodigo,
                        CategoriaCodigo = @CategoriaCodigo
                    WHERE Codigo = @Codigo AND Estado = 1";

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

                cmd.Parameters.Add("@MarcaCodigo", SqlDbType.VarChar, 20).Value = marcaCodigo;
                cmd.Parameters.Add("@CategoriaCodigo", SqlDbType.VarChar, 20).Value = categoriaCodigo;

                conn.Open();

                int filas = cmd.ExecuteNonQuery();

                if (filas > 0)
                {
                    MostrarMensaje("Producto modificado correctamente.", true);

                    LimpiarControles();
                    CargarGrilla();
                }
                else
                {
                    MostrarMensaje("No se encontró el producto que desea modificar.", false);
                }
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigoProducto.Text))
            {
                MostrarMensaje("Seleccione un producto para eliminar.", false);
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "UPDATE Productos SET Estado = 0 WHERE Codigo = @Codigo AND Estado = 1";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.Add("@Codigo", SqlDbType.VarChar, 20).Value =
                    txtCodigoProducto.Text.Trim();

                conn.Open();

                int filas = cmd.ExecuteNonQuery();

                if (filas > 0)
                {
                    MostrarMensaje("Producto eliminado correctamente.", true);

                    LimpiarControles();
                    CargarGrilla();
                }
                else
                {
                    MostrarMensaje("No se encontró el producto que desea eliminar.", false);
                }
            }
        }

        protected void chkVerInactivos_CheckedChanged(object sender, EventArgs e)
        {
            LimpiarControles();
            CargarGrilla();
        }

        protected void btnReactivar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigoProducto.Text))
            {
                MostrarMensaje("Seleccione un producto inactivo de la lista para reactivarlo.", false);
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "UPDATE Productos SET Estado = 1 WHERE Codigo = @Codigo";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.Add("@Codigo", SqlDbType.VarChar, 20).Value = txtCodigoProducto.Text.Trim();

                try
                {
                    conn.Open();
                    int filas = cmd.ExecuteNonQuery();

                    if (filas > 0)
                    {
                        MostrarMensaje("Producto reactivado correctamente.", true);
                        chkVerInactivos.Checked = false;
                        LimpiarControles();
                        CargarGrilla();
                    }
                    else
                    {
                        MostrarMensaje("No se encontró el producto.", false);
                    }
                }
                catch (SqlException)
                {
                    MostrarMensaje("Error al reactivar el producto.", false);
                }
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
            MostrarMensaje("", true);
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

        private void MostrarMensaje(string mensaje, bool correcto)
        {
            lblMensaje.Text = HttpUtility.HtmlEncode(mensaje);
            lblMensaje.CssClass = correcto ? "ms-3 fw-bold text-success" : "ms-3 fw-bold text-danger";
        }
    }
}