<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AdsAndClassifieds.aspx.cs" Inherits="AdsAndClassifieds" Title="Ads And Classifieds" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ImageButton runat="server" ID="PostAnAd" OnClick="GoTo" CommandArgument="A" AlternateText="Post An Ad" />
</asp:Content>

