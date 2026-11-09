
<%@ Page Title="Productos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" Inherits="Gestión_Comercio_TP_Cuatrimestral_Equipo_U.Productos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <main>

        <h2>Gestión de Productos</h2>
        <hr />

        <div class="card p-4 mb-4">

            <h4>Datos del Producto</h4>

            <div class="row">

                <div class="col-md-4 mb-3">
                    <label class="form-label">Código de Producto</label>
                    <asp:TextBox ID="txtCodigoProducto" runat="server"
                        CssClass="form-control"
                        placeholder="Ingrese el código">
                    </asp:TextBox>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">Nombre</label>
                    <asp:TextBox ID="txtNombreProducto" runat="server"
                        CssClass="form-control"
                        placeholder="Ingrese el nombre">
                    </asp:TextBox>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">Marca</label>
                    <asp:DropDownList ID="ddlMarca" runat="server"
                        CssClass="form-control">
                    </asp:DropDownList>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">Categoría</label>
                    <asp:DropDownList ID="ddlCategoria" runat="server"
                        CssClass="form-control">
                    </asp:DropDownList>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">Precio</label>
                    <asp:TextBox ID="txtPrecio" runat="server"
                        CssClass="form-control"
                        placeholder="Ingrese el precio">
                    </asp:TextBox>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">Stock actual</label>
                    <asp:TextBox ID="txtStock" runat="server"
                        CssClass="form-control"
                        placeholder="Ingrese el stock">
                    </asp:TextBox>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">Stock mínimo</label>
                    <asp:TextBox ID="txtStockMinimo" runat="server"
                        CssClass="form-control"
                        placeholder="Ingrese el stock mínimo">
                    </asp:TextBox>
                </div>

            </div>

            <div class="mt-2">

                <asp:Button ID="btnAgregar" runat="server"
                    Text="Agregar"
                    CssClass="btn btn-primary"
                    OnClick="btnAgregar_Click" />

                <asp:Button ID="btnModificar" runat="server"
                    Text="Modificar"
                    CssClass="btn btn-warning"
                    OnClick="btnModificar_Click" />

                <asp:Button ID="btnEliminar" runat="server"
                    Text="Eliminar"
                    CssClass="btn btn-danger"
                    OnClick="btnEliminar_Click"
                    CausesValidation="false" />

                <asp:Button ID="btnLimpiar" runat="server"
                    Text="Limpiar"
                    CssClass="btn btn-secondary"
                    OnClick="btnLimpiar_Click"
                    CausesValidation="false" />

            </div>

        </div>

        <div class="card p-4 mb-4">

            <h4>Buscar Productos</h4>

            <div class="row">

                <div class="col-md-4 mb-3">
                    <label class="form-label">Nombre o Código</label>
                    <asp:TextBox ID="txtBuscar" runat="server"
                        CssClass="form-control"
                        placeholder="Ingrese nombre o código">
                    </asp:TextBox>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">Filtrar por Marca</label>
                    <asp:DropDownList ID="ddlFiltroMarca" runat="server"
                        CssClass="form-control">
                    </asp:DropDownList>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">Filtrar por Categoría</label>
                    <asp:DropDownList ID="ddlFiltroCategoria" runat="server"
                        CssClass="form-control">
                    </asp:DropDownList>
                </div>

            </div>

            <div class="mt-2">

                <asp:Button ID="btnBuscar" runat="server"
                    Text="Buscar"
                    CssClass="btn btn-primary"
                    OnClick="btnBuscar_Click"
                    CausesValidation="false" />

                <asp:Button ID="btnLimpiarFiltros" runat="server"
                    Text="Limpiar filtros"
                    CssClass="btn btn-secondary"
                    OnClick="btnLimpiarFiltros_Click"
                    CausesValidation="false" />

            </div>

        </div>

        <h4>Listado de Productos</h4>

        <div class="table-responsive">

            <asp:GridView ID="dgvProductos" runat="server"
                CssClass="table table-striped table-bordered"
                AutoGenerateColumns="False"
                DataKeyNames="Codigo"
                OnSelectedIndexChanged="dgvProductos_SelectedIndexChanged"
                ShowHeaderWhenEmpty="True"
                EmptyDataText="No hay productos para mostrar.">

                <Columns>

                    <asp:CommandField
                        ShowSelectButton="True"
                        SelectText="Ver detalle"
                        HeaderText="Acción" />

                    <asp:BoundField
                        DataField="Codigo"
                        HeaderText="Código" />

                    <asp:BoundField
                        DataField="Nombre"
                        HeaderText="Nombre" />

                    <asp:BoundField
                        DataField="Marca"
                        HeaderText="Marca" />

                    <asp:BoundField
                        DataField="Categoria"
                        HeaderText="Categoría" />

                    <asp:BoundField
                        DataField="Precio"
                        HeaderText="Precio" />

                    <asp:BoundField
                        DataField="Stock"
                        HeaderText="Stock actual" />

                    <asp:BoundField
                        DataField="StockMinimo"
                        HeaderText="Stock mínimo" />

                </Columns>

            </asp:GridView>

        </div>

        <asp:Panel ID="pnlDetalleProducto" runat="server"
            Visible="false" CssClass="card p-4 mt-4">

            <h4>Detalle completo del Producto</h4>
            <hr />

            <div class="row">

                <div class="col-md-4 mb-3">
                    <strong>Código:</strong>
                    <asp:Label ID="lblDetalleCodigo" runat="server" />
                </div>

                <div class="col-md-4 mb-3">
                    <strong>Nombre:</strong>
                    <asp:Label ID="lblDetalleNombre" runat="server" />
                </div>

                <div class="col-md-4 mb-3">
                    <strong>Marca:</strong>
                    <asp:Label ID="lblDetalleMarca" runat="server" />
                </div>

                <div class="col-md-4 mb-3">
                    <strong>Categoría:</strong>
                    <asp:Label ID="lblDetalleCategoria" runat="server" />
                </div>

                <div class="col-md-4 mb-3">
                    <strong>Precio:</strong>
                    <asp:Label ID="lblDetallePrecio" runat="server" />
                </div>

                <div class="col-md-4 mb-3">
                    <strong>Stock actual:</strong>
                    <asp:Label ID="lblDetalleStock" runat="server" />
                </div>

                <div class="col-md-4 mb-3">
                    <strong>Stock mínimo:</strong>
                    <asp:Label ID="lblDetalleStockMinimo" runat="server" />
                </div>

            </div>

        </asp:Panel>

    </main>

</asp:Content>
