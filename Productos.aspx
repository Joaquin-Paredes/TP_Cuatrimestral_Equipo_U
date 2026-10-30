
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
                        CssClass="form-control" placeholder="Ingrese el código"></asp:TextBox>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">Nombre</label>
                    <asp:TextBox ID="txtNombreProducto" runat="server"
                        CssClass="form-control" placeholder="Ingrese el nombre"></asp:TextBox>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">Marca</label>
                    <asp:TextBox ID="txtMarca" runat="server"
                        CssClass="form-control" placeholder="Ingrese la marca"></asp:TextBox>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">Categoría</label>
                    <asp:TextBox ID="txtCategoria" runat="server"
                        CssClass="form-control" placeholder="Ingrese la categoría"></asp:TextBox>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">Precio</label>
                    <asp:TextBox ID="txtPrecio" runat="server"
                        CssClass="form-control" placeholder="Ingrese el precio"></asp:TextBox>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">Stock</label>
                    <asp:TextBox ID="txtStock" runat="server"
                        CssClass="form-control" placeholder="Ingrese el stock"></asp:TextBox>
                </div>

            </div>

            <div class="mt-2">
                <button type="button" class="btn btn-primary">Agregar</button>
                <button type="button" class="btn btn-warning">Modificar</button>
                <button type="button" class="btn btn-danger">Eliminar</button>
                <button type="button" class="btn btn-secondary">Limpiar</button>
            </div>

        </div>

        <h4>Listado de Productos</h4>

        <asp:GridView ID="dgvProductos" runat="server"
            CssClass="table table-striped table-bordered"
            AutoGenerateColumns="False"
            ShowHeaderWhenEmpty="True"
            EmptyDataText="No hay productos para mostrar.">

            <Columns>
                <asp:BoundField DataField="Codigo" HeaderText="Código" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="Marca" HeaderText="Marca" />
                <asp:BoundField DataField="Categoria" HeaderText="Categoría" />
                <asp:BoundField DataField="Precio" HeaderText="Precio" />
                <asp:BoundField DataField="Stock" HeaderText="Stock" />
            </Columns>

        </asp:GridView>

    </main>

</asp:Content>
