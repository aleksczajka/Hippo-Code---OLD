<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TagCloud.ascx.cs" Inherits="Controls_TagCloud" %>
<asp:Literal runat="server" ID="TopLiteral"></asp:Literal>
    <div align="left" class="WhiteHeader">
        <asp:Panel runat="server" ID="HeaderPanel">Ye old category cloud, behold!</asp:Panel>
    </div>
    <div align="left" style="color: #1fb6e7; font-family: Arial; font-weight: bold;">
        <asp:Literal runat="server" ID="TagsLiteral"></asp:Literal>
    </div>
</div>