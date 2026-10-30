
<%@ Page Title="Marcas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Marcas.aspx.cs" Inherits="Gestión_Comercio_TP_Cuatrimestral_Equipo_U.Marcas" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <h2>Gestión de Marcas</h2>
        <hr />

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

            <div class="mt-2">
                <button type="button" class="btn btn-primary">Agregar</button>
                <button type="button" class="btn btn-warning">Modificar</button>
                <button type="button" class="btn btn-danger">Eliminar</button>
                <button type="button" class="btn btn-secondary">Limpiar</button>
            </div>

        </div>

        <h4>Listado de Marcas</h4>

        <asp:GridView ID="dgvMarcas" runat="server"
            CssClass="table table-striped table-bordered"
            AutoGenerateColumns="true"
            EmptyDataText="No hay marcas para mostrar.">
        </asp:GridView>

    </main>

</asp:Content>
