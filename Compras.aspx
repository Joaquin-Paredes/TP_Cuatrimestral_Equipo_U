
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

        <asp:GridView ID="dgvCompras" runat="server"
            CssClass="table table-striped table-bordered"
            AutoGenerateColumns="False"
            ShowHeaderWhenEmpty="True"
            EmptyDataText="No hay compras para mostrar.">

            <Columns>
                <asp:BoundField DataField="NumeroCompra" HeaderText="N° Compra" />
                <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                <asp:BoundField DataField="Proveedor" HeaderText="Proveedor" />
                <asp:BoundField DataField="Producto" HeaderText="Producto" />
                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                <asp:BoundField DataField="PrecioUnitario" HeaderText="Precio Unitario" />
                <asp:BoundField DataField="Total" HeaderText="Total" />
            </Columns>

        </asp:GridView>

    </main>

</asp:Content>
