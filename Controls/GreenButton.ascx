<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GreenButton.ascx.cs" Inherits="GreenButton" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>

<asp:Literal runat="server" ID="WrapperLiteral"></asp:Literal>
    <asp:Literal runat="server" ID="BegLiteral"></asp:Literal>
    <asp:Literal runat="server" ID="WidthLiteral"></asp:Literal>
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td valign="top">
                    <img alt="Background Image" src="NewImages/GreenButtonLeft.png" height="27px" />
                </td>
                <td valign="top">
                    <div class="Green1">
                        <asp:LinkButton runat="server" Text="" ID="ButtonText" CssClass="NavyLink"></asp:LinkButton>
                        <asp:Literal runat="server" ID="OnlyTextLiteral"></asp:Literal>
                    </div>
                </td>
                <td valign="top">
                    <img align="BackgroundImage" src="NewImages/GreenButtonRight.png" height="27px" />
                </td>
            </tr>
        </table>
        
    </div>
    </div>
</div>
<asp:Literal runat="server" ID="ScriptLiteral"></asp:Literal>