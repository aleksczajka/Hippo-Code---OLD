<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AdSearchElements.ascx.cs" Inherits="AdSearchElements" %>
<%@ Register Src="~/Controls/Pager.ascx" TagPrefix="ctrl" TagName="Pager" %>
<%@ Register Src="~/Controls/AdSearchElement.ascx" TagPrefix="ctrl" TagName="AdSearchElement" %>
<asp:Label runat="server" ID="ErrorLable" ForeColor="red"></asp:Label>
<asp:Panel runat="server" ID="SearchElementsPanel"></asp:Panel>