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

                <div class="col-md-4 mb-3 position-relative">
                    <label class="form-label">Proveedor</label>
                    <asp:TextBox ID="txtProveedor" runat="server"
                        CssClass="form-control"
                        placeholder="Escriba o elija un proveedor..."
                        autocomplete="off"
                        onfocus="abrirListaProveedores()"
                        oninput="filtrarListaProveedores()"></asp:TextBox>

                    <div id="listaDesplegableProveedores"
                         class="list-group position-absolute w-100 shadow bg-white"
                         style="display: none; max-height: 220px; overflow-y: auto; z-index: 1050;">
                    </div>
                </div>

                <div class="col-md-4 mb-3 position-relative">
                    <label class="form-label">Producto</label>
                    <asp:TextBox ID="txtProducto" runat="server"
                        CssClass="form-control"
                        placeholder="Escriba o elija un producto..."
                        autocomplete="off"
                        onfocus="abrirListaProductos()"
                        oninput="filtrarListaProductos()"></asp:TextBox>

                    <div id="listaDesplegableProductos"
                         class="list-group position-absolute w-100 shadow bg-white"
                         style="display: none; max-height: 220px; overflow-y: auto; z-index: 1050;">
                    </div>
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

            <asp:Label ID="lblMensaje" runat="server" CssClass="fw-bold"></asp:Label>

            <div class="mt-2">
                <asp:Button ID="btnRegistrarCompra" runat="server" Text="Registrar Compra" CssClass="btn btn-primary" OnClick="btnRegistrarCompra_Click" />
                <asp:Button ID="btnModificar" runat="server" Text="Modificar" CssClass="btn btn-warning" OnClick="btnModificar_Click" />
                <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-danger" OnClick="btnEliminar_Click" />
                <asp:Button ID="btnReactivar" runat="server" Text="Reactivar" CssClass="btn btn-success" OnClick="btnReactivar_Click" />
                <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-secondary" OnClick="btnLimpiar_Click" />
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

        <div class="mb-3">
            <asp:CheckBox ID="chkMostrarEliminadas" runat="server"
                Text=" Mostrar compras eliminadas"
                AutoPostBack="true"
                OnCheckedChanged="chkMostrarEliminadas_CheckedChanged" />
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
                <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="ProveedorNombre" HeaderText="Proveedor" />
                <asp:BoundField DataField="ProductoNombre" HeaderText="Producto" />
                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                <asp:BoundField DataField="PrecioUnitario" HeaderText="Precio Unitario" DataFormatString="${0:N2}" />
                <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="${0:N2}" />
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
            let proveedoresDisponibles = [];

            function renderizarProveedores(texto = "") {
            const lista = document.getElementById("listaDesplegableProveedores");
            const input = document.getElementById("<%= txtProveedor.ClientID %>");
    if (!lista || !input) return;

    const q = texto.trim().toLowerCase();
    lista.innerHTML = "";

            const filtrados = proveedoresDisponibles.filter(p =>
    (p.nombre && p.nombre.toLowerCase().includes(q)) ||
    (p.cuit && p.cuit.toLowerCase().includes(q))
    );

    if (filtrados.length === 0) {
        lista.innerHTML = `<div class="list-group-item text-muted small">Sin proveedores coincidentes</div>`;
    return;
            }

            filtrados.forEach(p => {
                const btn = document.createElement("button");
    btn.type = "button";
    btn.className = "list-group-item list-group-item-action text-start py-2";
    btn.innerHTML = `
    <div class="fw-semibold">${p.nombre}</div>
    <small class="text-muted">CUIT: ${p.cuit}</small>
                `;
                btn.onclick = function () {
                    input.value = p.nombre;
                    lista.style.display = "none";
                };
                lista.appendChild(btn);
            });
        }

        function abrirListaProveedores() {
            const input = document.getElementById("<%= txtProveedor.ClientID %>");
            const lista = document.getElementById("listaDesplegableProveedores");
            if (!input || !lista) return;

            renderizarProveedores(input.value);
            lista.style.display = "block";
        }

        function filtrarListaProveedores() {
            const input = document.getElementById("<%= txtProveedor.ClientID %>");
            const lista = document.getElementById("listaDesplegableProveedores");
            if (!input || !lista) return;

            renderizarProveedores(input.value);
            lista.style.display = "block";
        }

        document.addEventListener("click", function (e) {
            const input = document.getElementById("<%= txtProveedor.ClientID %>");
    const lista = document.getElementById("listaDesplegableProveedores");
    if (!input || !lista) return;

    if (!input.contains(e.target) && !lista.contains(e.target)) {
        lista.style.display = "none";
            }
        });

        let productosDisponibles = [];

        function renderizarProductos(texto = "") {
            const lista = document.getElementById("listaDesplegableProductos");
            const input = document.getElementById("<%= txtProducto.ClientID %>");
            if (!lista || !input) return;

            const q = texto.trim().toLowerCase();
            lista.innerHTML = "";

            const filtrados = productosDisponibles.filter(p =>
                (p.nombre && p.nombre.toLowerCase().includes(q)) ||
                (p.codigo && p.codigo.toLowerCase().includes(q))
            );

            if (filtrados.length === 0) {
                lista.innerHTML = `<div class="list-group-item text-muted small">Sin productos coincidentes</div>`;
                return;
            }

            filtrados.forEach(p => {
                const btn = document.createElement("button");
                btn.type = "button";
                btn.className = "list-group-item list-group-item-action text-start py-2";
                btn.innerHTML = `
                        <div class="fw-semibold">${p.nombre}</div>
                        <small class="text-muted">${p.codigo ? "Cód: " + p.codigo : ""}</small>
                    `;
                btn.onclick = function () {
                    input.value = p.nombre;
                    lista.style.display = "none";
                };
                lista.appendChild(btn);
            });
        }

        function abrirListaProductos() {
            const input = document.getElementById("<%= txtProducto.ClientID %>");
                const lista = document.getElementById("listaDesplegableProductos");
                if (!input || !lista) return;

                renderizarProductos(input.value);
                lista.style.display = "block";
            }

            function filtrarListaProductos() {
                const input = document.getElementById("<%= txtProducto.ClientID %>");
                const lista = document.getElementById("listaDesplegableProductos");
                if (!input || !lista) return;

                renderizarProductos(input.value);
                lista.style.display = "block";
            }

            document.addEventListener("click", function (e) {
                const input = document.getElementById("<%= txtProducto.ClientID %>");
                const lista = document.getElementById("listaDesplegableProductos");
                if (!input || !lista) return;

                if (!input.contains(e.target) && !lista.contains(e.target)) {
                    lista.style.display = "none";
                }
            });
    </script>


</asp:Content>