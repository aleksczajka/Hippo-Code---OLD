<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SmallButton.ascx.cs" Inherits="SmallButton" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>

<div align="center">
    <asp:Literal runat="server" ID="BegLiteral"></asp:Literal>
    <div class="topDiv" style="clear: both;">
        <img style="float: left;" src="NewImages/SmallButtonLeft.png" height="24px" />
        <asp:Literal runat="server" ID="StyleLit"></asp:Literal>
            <asp:Literal runat="server" ID="JustTextLiteral"></asp:Literal>
            <asp:LinkButton runat="server" ID="ButtonText" CssClass="NavyLink12"></asp:LinkButton>
        </div>
        <img style="float: left;" src="NewImages/SmallButtonRight.png" height="24px" />
    </div>
    </div>
</div>