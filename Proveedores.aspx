
<%@ Page Title="Proveedores" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Proveedores.aspx.cs" Inherits="Gestión_Comercio_TP_Cuatrimestral_Equipo_U.Proveedores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <h2>Gestión de Proveedores</h2>
        <hr />

        <div class="card p-4 mb-4">

            <h4>Datos del Proveedor</h4>

            <div class="row">

                <div class="col-md-4 mb-3">
                    <label class="form-label">Código de Proveedor</label>
                    <asp:TextBox ID="txtCodigoProveedor" runat="server"
                        CssClass="form-control" placeholder="Ingrese el código"></asp:TextBox>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">Razón Social</label>
                    <asp:TextBox ID="txtRazonSocial" runat="server"
                        CssClass="form-control" placeholder="Ingrese la razón social"></asp:TextBox>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">CUIT</label>
                    <asp:TextBox ID="txtCUIT" runat="server"
                        CssClass="form-control" placeholder="Ingrese el CUIT"></asp:TextBox>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">Teléfono</label>
                    <asp:TextBox ID="txtTelefono" runat="server"
                        CssClass="form-control" placeholder="Ingrese el teléfono"></asp:TextBox>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">Correo Electrónico</label>
                    <asp:TextBox ID="txtEmail" runat="server"
                        CssClass="form-control" placeholder="Ingrese el correo"></asp:TextBox>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">Domicilio</label>
                    <asp:TextBox ID="txtDomicilio" runat="server"
                        CssClass="form-control" placeholder="Ingrese el domicilio"></asp:TextBox>
                </div>

            </div>

            <div class="mt-2">
                <button type="button" class="btn btn-primary">Agregar</button>
                <button type="button" class="btn btn-warning">Modificar</button>
                <button type="button" class="btn btn-danger">Eliminar</button>
                <button type="button" class="btn btn-secondary">Limpiar</button>
            </div>

        </div>

        <h4>Listado de Proveedores</h4>

        <asp:GridView ID="dgvProveedores" runat="server"
            CssClass="table table-striped table-bordered"
            AutoGenerateColumns="False"
            ShowHeaderWhenEmpty="True"
            EmptyDataText="No hay proveedores para mostrar.">

            <Columns>
                <asp:BoundField DataField="Codigo" HeaderText="Código" />
                <asp:BoundField DataField="RazonSocial" HeaderText="Razón Social" />
                <asp:BoundField DataField="CUIT" HeaderText="CUIT" />
                <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                <asp:BoundField DataField="Email" HeaderText="Correo Electrónico" />
                <asp:BoundField DataField="Domicilio" HeaderText="Domicilio" />
            </Columns>

        </asp:GridView>

    </main>

</asp:Content>
