<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TripSearchElements.ascx.cs" Inherits="TripSearchElements" %>
<%@ Register Src="~/Controls/Pager.ascx" TagName="Pager" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/TripSearchElement.ascx" TagPrefix="ctrl" TagName="SearchElement" %>



            <asp:Label runat="server" ID="ErrorLabel" ForeColor="red"></asp:Label>
        <asp:Panel runat="server" ID="SearchElementsPanel"></asp:Panel>
