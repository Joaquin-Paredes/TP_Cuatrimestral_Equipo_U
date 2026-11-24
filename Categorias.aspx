<%@ Page Title="Categorías" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Categorias.aspx.cs" Inherits="Gestión_Comercio_TP_Cuatrimestral_Equipo_U.Categorias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <h2>Gestión de Categorías</h2>
        <hr />
        
        <div class="mb-3">
            <asp:CheckBox ID="chkVerInactivos" runat="server" Text=" Mostrar categorías eliminadas" AutoPostBack="true" CssClass="form-check-label fw-bold text-danger" OnCheckedChanged="chkVerInactivos_CheckedChanged" />
        </div>

        <div class="card p-4 mb-4">

            <h4>Datos de la Categoría</h4>

            <div class="row">

                <div class="col-md-6 mb-3">
                    <label class="form-label">Código de Categoría</label>
                    <asp:TextBox ID="txtCodigoCategoria" runat="server"
                        CssClass="form-control" placeholder="Ingrese el código"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvCodigoCategoria" runat="server"
                        ControlToValidate="txtCodigoCategoria"
                        ErrorMessage="El código de categoría es obligatorio."
                        CssClass="text-danger small"
                        Display="Dynamic" />
                </div>

                <div class="col-md-6 mb-3">
                    <label class="form-label">Nombre de Categoría</label>
                    <asp:TextBox ID="txtNombreCategoria" runat="server"
                        CssClass="form-control" placeholder="Ingrese el nombre"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvNombreCategoria" runat="server"
                        ControlToValidate="txtNombreCategoria"
                        ErrorMessage="El nombre de categoría es obligatorio."
                        CssClass="text-danger small"
                        Display="Dynamic" />
                </div>

                <div class="col-md-12 mb-3">
                    <label class="form-label">Descripción</label>
                    <asp:TextBox ID="txtDescripcion" runat="server"
                        CssClass="form-control" TextMode="MultiLine"
                        Rows="3" placeholder="Ingrese una descripción"></asp:TextBox>
                </div>

            </div>

            <div class="mt-2 d-flex align-items-center flex-wrap gap-2">
               <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btn btn-primary" OnClick="btnAgregar_Click" />
               <asp:Button ID="btnModificar" runat="server" Text="Modificar" CssClass="btn btn-warning" OnClick="btnModificar_Click" />
               <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-danger" OnClick="btnEliminar_Click" CausesValidation="false" />
               <asp:Button ID="btnReactivar" runat="server" Text="Reactivar" CssClass="btn btn-success" OnClick="btnReactivar_Click" CausesValidation="false" />
               <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-secondary" OnClick="btnLimpiar_Click" CausesValidation="false" />
               <asp:Label ID="lblMensaje" runat="server" CssClass="ms-3 fw-bold"></asp:Label>
            </div>

        </div>

        <h4>Listado de Categorías</h4>

        <div class="mb-3">
    <asp:TextBox ID="txtBuscarNombreDinamico" runat="server"
        ClientIDMode="Static"
        CssClass="form-control"
        placeholder="Buscar por nombre..."
        AutoPostBack="true"
        OnTextChanged="txtBuscarNombreDinamico_TextChanged"
        onkeydown="guardarScroll()" />
</div>

        <asp:GridView ID="dgvCategorias" runat="server"
            CssClass="table table-striped table-bordered"
            AutoGenerateColumns="False"
            DataKeyNames="Codigo"
            ShowHeaderWhenEmpty="True"
            EmptyDataText="No hay categorías para mostrar."
            OnSelectedIndexChanged="dgvCategorias_SelectedIndexChanged">

            <Columns>
                <asp:BoundField DataField="Codigo" HeaderText="Código" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                <asp:CommandField ShowSelectButton="True" SelectText="Seleccionar" ControlStyle-CssClass="btn btn-sm btn-outline-primary" />
            </Columns>

        </asp:GridView>

    </main>

    <script>

    function guardarScroll() {
        sessionStorage.setItem("scrollCategorias", window.scrollY);
    }

    window.addEventListener("load", function () {
        const pos = sessionStorage.getItem("scrollCategorias");
        if (pos !== null) {
            window.scrollTo(0, parseInt(pos, 10));
            sessionStorage.removeItem("scrollCategorias");
        }
    });
</script>

</asp:Content>