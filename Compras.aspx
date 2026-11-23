
<%@ Page Title="Compras" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Compras.aspx.cs" Inherits="Gestión_Comercio_TP_Cuatrimestral_Equipo_U.Compras" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <h2>Gestión de Compras</h2>
        <hr />

        <div class="card p-4 mb-4">

            <h4>Datos de la Compra</h4>

            <div class="row">

                <div class="col-md-4 mb-3">
                    <label class="form-label">Número de Compra</label>
                    <asp:TextBox ID="txtNumeroCompra" runat="server"
                        CssClass="form-control" placeholder="Ingrese el número"></asp:TextBox>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">Fecha</label>
                    <asp:TextBox ID="txtFechaCompra" runat="server"
                        TextMode="Date" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">Proveedor</label>
                    <asp:TextBox ID="txtProveedor" runat="server"
                        CssClass="form-control" placeholder="Ingrese el proveedor"></asp:TextBox>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">Producto</label>
                    <asp:TextBox ID="txtProducto" runat="server"
                        CssClass="form-control" placeholder="Ingrese el producto"></asp:TextBox>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">Cantidad</label>
                    <asp:TextBox ID="txtCantidad" runat="server"
                        CssClass="form-control" placeholder="Ingrese la cantidad"></asp:TextBox>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">Precio Unitario</label>
                    <asp:TextBox ID="txtPrecioUnitario" runat="server"
                        CssClass="form-control" placeholder="Ingrese el precio"></asp:TextBox>
                </div>

                <div class="col-md-6 mb-3">
                    <label class="form-label">Total de la Compra</label>
                    <asp:TextBox ID="txtTotalCompra" runat="server"
                        CssClass="form-control" placeholder="Ingrese el total"></asp:TextBox>
                </div>

            </div>

            <div class="mt-2">
                <button type="button" class="btn btn-primary">Registrar Compra</button>
                <button type="button" class="btn btn-warning">Modificar</button>
                <button type="button" class="btn btn-danger">Eliminar</button>
                <button type="button" class="btn btn-secondary">Limpiar</button>
            </div>

        </div>

        <h4>Listado de Compras</h4>

        <div class="mb-3">
    <asp:TextBox ID="txtBuscarNombreDinamico" runat="server"
        ClientIDMode="Static"
        CssClass="form-control"
        placeholder="Buscar por proveedor o producto..."
        AutoPostBack="true"
        OnTextChanged="txtBuscarNombreDinamico_TextChanged"
        onkeydown="guardarScroll()" />
</div>

        <asp:GridView ID="dgvCompras" runat="server"
            CssClass="table table-striped table-bordered"
            AutoGenerateColumns="False"
            OnSelectedIndexChanged="dgvCompras_SelectedIndexChanged"
            ShowHeaderWhenEmpty="True"
            EmptyDataText="No hay compras para mostrar.">

            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="Ver detalle" HeaderText="Acción" ControlStyle-CssClass="btn btn-sm btn-outline-primary" />
                <asp:BoundField DataField="NumeroCompra" HeaderText="N° Compra" />
                <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                <asp:BoundField DataField="ProveedorNombre" HeaderText="Proveedor" />
                <asp:BoundField DataField="ProductoNombre" HeaderText="Producto" />
                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                <asp:BoundField DataField="PrecioUnitario" HeaderText="Precio Unitario" />
                <asp:BoundField DataField="Total" HeaderText="Total" />
            </Columns>

        </asp:GridView>
        <asp:Panel ID="pnlDetalleCompra" runat="server" Visible="false" CssClass="card p-4 mt-4">
            <h4>Detalle de Compra</h4>
            <hr />
            <div class="row">
                <div class="col-md-4 mb-2"><strong>N° Compra:</strong> <asp:Label ID="lblDetNumero" runat="server" /></div>
                <div class="col-md-4 mb-2"><strong>Fecha:</strong> <asp:Label ID="lblDetFecha" runat="server" /></div>
                <div class="col-md-4 mb-2"><strong>Proveedor:</strong> <asp:Label ID="lblDetProveedor" runat="server" /></div>
                <div class="col-md-4 mb-2"><strong>Producto:</strong> <asp:Label ID="lblDetProducto" runat="server" /></div>
                <div class="col-md-4 mb-2"><strong>Cantidad:</strong> <asp:Label ID="lblDetCantidad" runat="server" /></div>
                <div class="col-md-4 mb-2"><strong>Total:</strong> <asp:Label ID="lblDetTotal" runat="server" /></div>
            </div>
        </asp:Panel>

    </main>

    <script>
    function guardarScroll() {
        sessionStorage.setItem("scrollCompras", window.scrollY);
    }

    window.addEventListener("load", function () {
        const pos = sessionStorage.getItem("scrollCompras");
        if (pos !== null) {
            window.scrollTo(0, parseInt(pos, 10));
            sessionStorage.removeItem("scrollCompras");
        }
    });
</script>

</asp:Content>
