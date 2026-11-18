
<%@ Page Title="Proveedores" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Proveedores.aspx.cs" Inherits="Gestión_Comercio_TP_Cuatrimestral_Equipo_U.Proveedores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <h2>Gestión de Proveedores</h2>
        <hr />

        <div class="card p-4 mb-4">
            <h4>Buscar Proveedor</h4>
            <div class="row align-items-end">
                <div class="col-md-5 mb-2">
                    <label class="form-label">CUIT / CUIL</label>
                    <asp:TextBox ID="txtBuscarCuit" runat="server" CssClass="form-control" placeholder="Ingrese el CUIT a buscar"></asp:TextBox>
                </div>
                <div class="col-md-7 mb-2">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary me-2" OnClick="btnBuscar_Click" />
                    <asp:Button ID="btnLimpiarBusqueda" runat="server" Text="Mostrar Todos" CssClass="btn btn-outline-secondary" OnClick="btnLimpiarBusqueda_Click" />
                </div>
            </div>
        </div>
    
        <div class="card p-4 mb-4">
            <h4>Datos del Proveedor</h4>

            <div class="row">
                <div class="col-md-4 mb-3">
                    <label class="form-label">CUIT</label>
                    <asp:TextBox ID="txtCUIT" runat="server" CssClass="form-control" placeholder="Ingrese el CUIT"></asp:TextBox>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">Razón Social</label>
                    <asp:TextBox ID="txtRazonSocial" runat="server" CssClass="form-control" placeholder="Ingrese la razón social"></asp:TextBox>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">Teléfono</label>
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" placeholder="Ingrese el teléfono"></asp:TextBox>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">Correo Electrónico</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Ingrese el correo"></asp:TextBox>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">Dirección</label>
                    <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control" placeholder="Ingrese la dirección"></asp:TextBox>
                </div>
            </div>

            <div class="mt-2 d-flex align-items-center flex-wrap gap-2">
                <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btn btn-primary" OnClick="btnAgregar_Click" />
                <asp:Button ID="btnModificar" runat="server" Text="Modificar" CssClass="btn btn-warning" OnClick="btnModificar_Click" />
                <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-danger" OnClick="btnEliminar_Click" />
                <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-secondary" OnClick="btnLimpiar_Click" />
                
                <asp:Label ID="lblMensaje" runat="server" CssClass="ms-3 fw-bold"></asp:Label>
            </div>
        </div>

        <h4>Listado de Proveedores</h4>

        <asp:GridView ID="dgvProveedores" runat="server"
            CssClass="table table-striped table-bordered"
            AutoGenerateColumns="False"
            DataKeyNames="CUIT"
            ShowHeaderWhenEmpty="True"
            EmptyDataText="No hay proveedores para mostrar." OnSelectedIndexChanged="dgvProveedores_SelectedIndexChanged">

            <Columns>
                <asp:BoundField DataField="CUIT" HeaderText="CUIT" />
                <asp:BoundField DataField="RazonSocial" HeaderText="Razón Social" />
                <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                <asp:BoundField DataField="Email" HeaderText="Correo Electrónico" />
                <asp:BoundField DataField="Direccion" HeaderText="Dirección" />
                <asp:CommandField ShowSelectButton="True" SelectText="Seleccionar" ControlStyle-CssClass="btn btn-sm btn-outline-primary" />
            </Columns>

        </asp:GridView>

    </main>

</asp:Content>