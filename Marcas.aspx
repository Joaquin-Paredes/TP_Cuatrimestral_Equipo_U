<%@ Page Title="Marcas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Marcas.aspx.cs" Inherits="Gestión_Comercio_TP_Cuatrimestral_Equipo_U.Marcas" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <h2 id="title">Lista de Marcas</h2>
        <hr />

        <!-- Nuestra tabla que mostrará las marcas de la base de datos -->
        <asp:GridView ID="dgvMarcas" runat="server" CssClass="table" AutoGenerateColumns="true">
        </asp:GridView>
    </main>
</asp:Content>