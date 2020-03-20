<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucMapaUbicacion.ascx.cs" Inherits="SAT.UserControls.wucMapaUbicacion" %>
<style>
    html, body, form, #mapa {
        height: 100%;
        margin: 0px;
        padding: 0px;
    }
</style>
<script src="https://maps.googleapis.com/maps/api/js?v=3.exp&key=AIzaSyBH8LH8WQEO7Y9BqT62mCWWV0WPYKQnaCY"></script>
<div id="mapa"></div>
<asp:Literal ID="ltr" runat="server" />