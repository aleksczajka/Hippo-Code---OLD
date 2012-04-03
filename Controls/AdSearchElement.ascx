<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AdSearchElement.ascx.cs" Inherits="AdSearchElement" %>
<asp:Literal runat="server" ID="BeginingLiteral"></asp:Literal>
    <asp:Literal runat="server" ID="EventImageLiteral" Text=""></asp:Literal>
    <div style="clear: right;"><asp:HyperLink runat="server" ID="SearchLabel" CssClass="Navy14Link"></asp:HyperLink></div>
    <div style="clear: right;">
        <asp:Label runat="server" ID="ShortDescriptionLabel" CssClass="Text11"></asp:Label>
    </div>
    <div style="clear: right;">
        <asp:Literal runat="server" ID="TagsLiteral"></asp:Literal>
    </div>
</div>