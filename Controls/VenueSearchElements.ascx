<%@ Control Language="C#" AutoEventWireup="true" CodeFile="VenueSearchElements.ascx.cs" Inherits="VenueSearchElements" %>
<%@ Register Src="~/Controls/Pager.ascx" TagPrefix="ctrl" TagName="Pager" %>
<%@ Register Src="~/Controls/VenueSearchElement.ascx" TagPrefix="ctrl" TagName="VenueSearchElement" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%--<asp:UpdatePanel runat="server" ChildrenAsTriggers="true" UpdateMode="conditional">
    <ContentTemplate>--%>

        <asp:Panel runat="server" ID="SearchElementsPanel"></asp:Panel>
    
  <%--  </ContentTemplate>
</asp:UpdatePanel>--%>