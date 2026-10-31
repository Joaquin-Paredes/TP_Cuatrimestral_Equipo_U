
<%@ Page Title="Categorías" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Categorias.aspx.cs" Inherits="Gestión_Comercio_TP_Cuatrimestral_Equipo_U.Categorias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <h2>Gestión de Categorías</h2>
        <hr />

        <div class="card p-4 mb-4">

            <h4>Datos de la Categoría</h4>

            <div class="row">

                <div class="col-md-6 mb-3">
                    <label class="form-label">Código de Categoría</label>
                    <asp:TextBox ID="txtCodigoCategoria" runat="server"
                        CssClass="form-control" placeholder="Ingrese el código"></asp:TextBox>
                </div>

                <div class="col-md-6 mb-3">
                    <label class="form-label">Nombre de Categoría</label>
                    <asp:TextBox ID="txtNombreCategoria" runat="server"
                        CssClass="form-control" placeholder="Ingrese el nombre"></asp:TextBox>
                </div>

                <div class="col-md-12 mb-3">
                    <label class="form-label">Descripción</label>
                    <asp:TextBox ID="txtDescripcion" runat="server"
                        CssClass="form-control" TextMode="MultiLine"
                        Rows="3" placeholder="Ingrese una descripción"></asp:TextBox>
                </div>

            </div>

            <div class="mt-2">
               <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btn btn-primary" OnClick="btnAgregar_Click" />
               <asp:Button ID="btnModificar" runat="server" Text="Modificar" CssClass="btn btn-warning" OnClick="btnModificar_Click" />
               <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-danger" OnClick="btnEliminar_Click" />
               <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-secondary" OnClick="btnLimpiar_Click" />
            </div>

        </div>

        <h4>Listado de Categorías</h4>

        <asp:GridView ID="dgvCategorias" runat="server"
            CssClass="table table-striped table-bordered"
            AutoGenerateColumns="False"
            ShowHeaderWhenEmpty="True"
            EmptyDataText="No hay categorías para mostrar.">

            <Columns>
                <asp:BoundField DataField="Codigo" HeaderText="Código" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
            </Columns>

        </asp:GridView>

    </main>

</asp:Content>
