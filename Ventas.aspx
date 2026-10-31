
<%@ Page Title="Ventas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Ventas.aspx.cs" Inherits="Gestión_Comercio_TP_Cuatrimestral_Equipo_U.Ventas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <h2>Gestión de Ventas</h2>
        <hr />

        <div class="card p-4 mb-4">

            <h4>Datos de la Venta</h4>

            <div class="row">

                <div class="col-md-4 mb-3">
                    <label class="form-label">Número de Venta</label>
                    <asp:TextBox ID="txtNumeroVenta" runat="server"
                        CssClass="form-control" placeholder="Ingrese el número"></asp:TextBox>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">Fecha</label>
                    <asp:TextBox ID="txtFecha" runat="server"
                        TextMode="Date" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">Cliente</label>
                    <asp:TextBox ID="txtCliente" runat="server"
                        CssClass="form-control" placeholder="Ingrese el cliente"></asp:TextBox>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">Vendedor</label>
                    <asp:TextBox ID="txtVendedor" runat="server"
                        CssClass="form-control" placeholder="Ingrese el vendedor"></asp:TextBox>
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

                <div class="col-md-6 mb-3">
                    <label class="form-label">Precio Unitario</label>
                    <asp:TextBox ID="txtPrecioUnitario" runat="server"
                        CssClass="form-control" placeholder="Ingrese el precio"></asp:TextBox>
                </div>

                <div class="col-md-6 mb-3">
                    <label class="form-label">Total</label>
                    <asp:TextBox ID="txtTotal" runat="server"
                        CssClass="form-control" placeholder="Total de la venta"></asp:TextBox>
                </div>

            </div>

            <div class="mt-2">
                <button type="button" class="btn btn-primary">Registrar Venta</button>
                <button type="button" class="btn btn-warning">Modificar</button>
                <button type="button" class="btn btn-danger">Anular Venta</button>
                <button type="button" class="btn btn-secondary">Limpiar</button>
            </div>

        </div>

        <h4>Listado de Ventas</h4>

        <asp:GridView ID="dgvVentas" runat="server"
            CssClass="table table-striped table-bordered"
            AutoGenerateColumns="False"
            ShowHeaderWhenEmpty="True"
            EmptyDataText="No hay ventas para mostrar.">

            <Columns>
                <asp:BoundField DataField="NumeroVenta" HeaderText="N° Venta" />
                <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                <asp:BoundField DataField="Cliente" HeaderText="Cliente" />
                <asp:BoundField DataField="Vendedor" HeaderText="Vendedor" />
                <asp:BoundField DataField="Producto" HeaderText="Producto" />
                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                <asp:BoundField DataField="PrecioUnitario" HeaderText="Precio Unitario" />
                <asp:BoundField DataField="Total" HeaderText="Total" />
            </Columns>

        </asp:GridView>

    </main>

</asp:Content>
