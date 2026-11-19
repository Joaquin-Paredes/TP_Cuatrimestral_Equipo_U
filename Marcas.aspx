<%@ Page Title="Marcas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Marcas.aspx.cs" Inherits="Gestión_Comercio_TP_Cuatrimestral_Equipo_U.Marcas" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <h2>Gestión de Marcas</h2>
        <hr />

        <div class="mb-3">
            <asp:CheckBox ID="chkVerInactivos" runat="server" Text=" Mostrar marcas eliminadas" AutoPostBack="true" CssClass="form-check-label fw-bold text-danger" OnCheckedChanged="chkVerInactivos_CheckedChanged" />
        </div>

        <div class="card p-4 mb-4">

            <h4>Datos de la Marca</h4>

            <div class="mb-3">
                <label class="form-label">Código de Marca</label>
                <asp:TextBox ID="txtCodigoMarca" runat="server"
                    CssClass="form-control" placeholder="Ingrese el código"></asp:TextBox>
            </div>

            <div class="mb-3">
                <label class="form-label">Nombre de Marca</label>
                <asp:TextBox ID="txtNombreMarca" runat="server"
                    CssClass="form-control" placeholder="Ingrese el nombre"></asp:TextBox>
            </div>

            <div class="mt-2 d-flex align-items-center flex-wrap gap-2">
                <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btn btn-primary" OnClick="btnAgregar_Click" />
                <asp:Button ID="btnModificar" runat="server" Text="Modificar" CssClass="btn btn-warning" OnClick="btnModificar_Click" />
                <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-danger" OnClick="btnEliminar_Click" />
                <asp:Button ID="btnReactivar" runat="server" Text="Reactivar" CssClass="btn btn-success" OnClick="btnReactivar_Click" />
                <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-secondary" OnClick="btnLimpiar_Click" />
                <asp:Label ID="lblMensaje" runat="server" CssClass="ms-3 fw-bold"></asp:Label>
            </div>

        </div>

        <h4>Listado de Marcas</h4>

        <asp:GridView ID="dgvMarcas" runat="server"
            CssClass="table table-striped table-bordered"
            AutoGenerateColumns="False"
            DataKeyNames="Codigo"
            OnSelectedIndexChanged="dgvMarcas_SelectedIndexChanged"
            EmptyDataText="No hay marcas para mostrar.">
            <Columns>
                <asp:BoundField DataField="Codigo" HeaderText="Código" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:CommandField ShowSelectButton="True" SelectText="Seleccionar" ControlStyle-CssClass="btn btn-sm btn-outline-primary" />
            </Columns>
        </asp:GridView>

    </main>

</asp:Content>