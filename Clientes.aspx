
<%@ Page Title="Clientes" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Clientes.aspx.cs" Inherits="Gestión_Comercio_TP_Cuatrimestral_Equipo_U.Clientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <h2>Gestión de Clientes</h2>
        <hr />

        <div class="card p-4 mb-4">

            <h4>Datos del Cliente</h4>

            <div class="row">

                <div class="col-md-4 mb-3">
                    <label class="form-label">Código de Cliente</label>
                    <asp:TextBox ID="txtCodigoCliente" runat="server"
                        CssClass="form-control" placeholder="Ingrese el código"></asp:TextBox>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">Nombre</label>
                    <asp:TextBox ID="txtNombre" runat="server"
                        CssClass="form-control" placeholder="Ingrese el nombre"></asp:TextBox>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">Apellido</label>
                    <asp:TextBox ID="txtApellido" runat="server"
                        CssClass="form-control" placeholder="Ingrese el apellido"></asp:TextBox>
                </div>

                <div class="col-md-4 mb-3">
                    <label class="form-label">DNI / CUIT</label>
                    <asp:TextBox ID="txtDocumento" runat="server"
                        CssClass="form-control" placeholder="Ingrese DNI o CUIT"></asp:TextBox>
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

                <div class="col-md-12 mb-3">
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

        <h4>Listado de Clientes</h4>

        <asp:GridView ID="dgvClientes" runat="server"
            CssClass="table table-striped table-bordered"
            AutoGenerateColumns="False"
            ShowHeaderWhenEmpty="True"
            EmptyDataText="No hay clientes para mostrar.">

            <Columns>
                <asp:BoundField DataField="Codigo" HeaderText="Código" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
                <asp:BoundField DataField="Documento" HeaderText="DNI / CUIT" />
                <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                <asp:BoundField DataField="Email" HeaderText="Correo Electrónico" />
                <asp:BoundField DataField="Domicilio" HeaderText="Domicilio" />
            </Columns>

        </asp:GridView>

    </main>

</asp:Content>
