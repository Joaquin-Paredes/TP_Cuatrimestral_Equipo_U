<%@ Page Title="Clientes" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Clientes.aspx.cs" Inherits="Gestión_Comercio_TP_Cuatrimestral_Equipo_U.Clientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <h2>Gestión de Clientes</h2>
        <hr />

        <div class="mb-3">
            <asp:CheckBox ID="chkVerInactivos" runat="server" Text=" Mostrar clientes eliminados" AutoPostBack="true" CssClass="form-check-label fw-bold text-danger" OnCheckedChanged="chkVerInactivos_CheckedChanged" />
        </div>

        <div class="card p-4 mb-4">
            <h4>Datos del Cliente</h4>

            <div class="row">

                <div class="col-md-6 mb-3">
                    <label class="form-label">CUIT</label>
                    <asp:TextBox ID="txtCodigoCliente" runat="server"
                        CssClass="form-control" placeholder="Ingrese el CUIT" />
                    <asp:RegularExpressionValidator ID="revCuitCliente" runat="server"
                        ControlToValidate="txtCodigoCliente"
                        ValidationExpression="^[0-9]+$"
                        ErrorMessage="El CUIT debe contener solo números."
                        CssClass="text-danger small"
                        Display="Dynamic" />

                    <asp:RequiredFieldValidator ID="rfvCuitCliente" runat="server"
                        ControlToValidate="txtCodigoCliente"
                        ErrorMessage="El CUIT es obligatorio."
                        CssClass="text-danger small"
                        Display="Dynamic" />

                </div>

                <div class="col-md-6 mb-3">
                    <label class="form-label">Nombre Completo</label>
                    <asp:TextBox ID="txtNombre" runat="server"
                        CssClass="form-control" placeholder="Ingrese nombre y apellido" />

                    <asp:RequiredFieldValidator ID="rfvNombreCliente" runat="server"
                        ControlToValidate="txtNombre"
                        ErrorMessage="El nombre completo es obligatorio."
                        CssClass="text-danger small"
                        Display="Dynamic" />
                </div>

                <div class="col-md-6 mb-3">
                    <label class="form-label">Correo Electrónico</label>
                    <asp:TextBox ID="txtEmail" runat="server"
                        CssClass="form-control" placeholder="Ingrese el correo" />
                    <asp:RegularExpressionValidator ID="revEmailCliente" runat="server"
                        ControlToValidate="txtEmail"
                        ValidationExpression="^\S+@\S+\.\S+$"
                        ErrorMessage="Ingrese un correo electrónico válido."
                        CssClass="text-danger small"
                        Display="Dynamic" />
                </div>

                <div class="col-md-6 mb-3">
                    <label class="form-label">Teléfono</label>
                    <asp:TextBox ID="txtTelefono" runat="server"
                        CssClass="form-control" placeholder="Ingrese el teléfono" />
                </div>

                <div class="col-md-12 mb-3">
                    <label class="form-label">Dirección</label>
                    <asp:TextBox ID="txtDomicilio" runat="server"
                        CssClass="form-control" placeholder="Ingrese la dirección" />
                </div>

            </div>

            <div class="mt-2 d-flex align-items-center flex-wrap gap-2">

                <asp:Button ID="btnAgregar" runat="server"
                    Text="Agregar" CssClass="btn btn-primary"
                    OnClick="btnAgregar_Click" />

                <asp:Button ID="btnModificar" runat="server"
                    Text="Modificar" CssClass="btn btn-warning"
                    OnClick="btnModificar_Click" />

                <asp:Button ID="btnEliminar" runat="server"
                    Text="Eliminar" CssClass="btn btn-danger"
                    OnClick="btnEliminar_Click"
                    CausesValidation="false" />
                
                <asp:Button ID="btnReactivar" runat="server" 
                    Text="Reactivar" CssClass="btn btn-success" 
                    OnClick="btnReactivar_Click" 
                    CausesValidation="false" />

                <asp:Button ID="btnLimpiar" runat="server"
                    Text="Limpiar" CssClass="btn btn-secondary"
                    OnClick="btnLimpiar_Click"
                    CausesValidation="false" />

                <asp:Label ID="lblMensaje" runat="server" CssClass="ms-3 fw-bold"></asp:Label>

            </div>
        </div>

        <h4>Listado de Clientes</h4>

        <div class="mb-3">
            <asp:TextBox ID="txtBuscarNombreDinamico" runat="server"
                ClientIDMode="Static"
                CssClass="form-control"
                placeholder="Buscar por nombre..."
                AutoPostBack="true"
                OnTextChanged="txtBuscarNombreDinamico_TextChanged"
                onkeydown="guardarScroll()" />
        </div>

        <div class="table-responsive">

            <asp:GridView ID="dgvClientes" runat="server"
                CssClass="table table-striped table-bordered"
                AutoGenerateColumns="False"
                DataKeyNames="Codigo"
                OnSelectedIndexChanged="dgvClientes_SelectedIndexChanged"
                ShowHeaderWhenEmpty="True"
                EmptyDataText="No hay clientes para mostrar.">

                <Columns>

                    <asp:CommandField ShowSelectButton="True"
                        SelectText="Seleccionar" HeaderText="Acción" ControlStyle-CssClass="btn btn-sm btn-outline-primary" />

                    <asp:BoundField DataField="Codigo" HeaderText="CUIT" />
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre Completo" />
                    <asp:BoundField DataField="Email" HeaderText="Correo Electrónico" />
                    <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                    <asp:BoundField DataField="Domicilio" HeaderText="Dirección" />

                </Columns>

            </asp:GridView>

        </div>

    </main>

    <script>
function guardarScroll() {
    sessionStorage.setItem("scrollClientes", window.scrollY);
}

window.addEventListener("load", function () {
    const pos = sessionStorage.getItem("scrollClientes");
    if (pos !== null) {
        window.scrollTo(0, parseInt(pos, 10));
        sessionStorage.removeItem("scrollClientes");
    }
});
</script>

</asp:Content>