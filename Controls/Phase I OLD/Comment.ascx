<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Comment.ascx.cs" Inherits="Controls_Comment" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<telerik:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="false" 
    KeepInScreenBounds="true" VisibleOnPageLoad="false" Behaviors="none"
ID="MessageRadWindowManager" BackColor="#1b1b1b" Modal="true" DestroyOnClose="true" RestrictionZoneID="RestrictionZone"
runat="server">
    <Windows>
        <telerik:RadWindow Width="400" EnableEmbeddedSkins="true" IconUrl="image/ContactIcon.png" 
        Skin="Black" Height="450" VisibleStatusbar="false" ID="MessageRadWindow" Title="Sent Message" runat="server">
        </telerik:RadWindow>
    </Windows>
</telerik:RadWindowManager>
<div style="padding-bottom: 10px; width: 439px;">
<asp:Label runat="server" ID="ErrorLabel" ForeColor="Red"></asp:Label>
    <div style="background-image: url('image/CommentTop.png'); background-repeat: no-repeat;height: 5px;">
        &nbsp;
    </div>
    <div id="topDiv" style="font-size: 11px; font-family: Arial; color: #cccccc; line-height: 20px ;padding-left: 10px; 
    padding-right: 10px ; padding-bottom: 0px; background-image: url('image/CommentBackground.png'); 
    background-repeat: repeat-y;">
        <div style="float: left; padding-top: 10px; margin-top: -5px; height: 72px; width: 72px;">
        <table align="center" onmouseout="this.style.backgroundColor = '#666666';" onmouseover="this.style.backgroundColor = '#a5c13a';" valign="middle" cellpadding="0" cellspacing="1"  bgcolor="#666666">
        <tr>
        <td align="center" valign="middle">
            <table width="52px" align="center" height="52px" bgcolor="#333333" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="center">
                        <asp:ImageButton  runat="server" ID="ProfileImage"/>
                    </td>
                </tr>
            </table>
            
            </td>
            </tr>
            </table>
                </div>
            <asp:Panel runat="server" ID="CommentPanel"><div style="float: right;"><asp:ImageButton ToolTip="Send this person a message!" ImageUrl="../image/CommentBlurb.png" OnClick="Connect" runat="server" ID="ConnectButton" /></div></asp:Panel>
            Posted on <asp:Label runat="server" CssClass="CommentLabel" ID="DateLabel"></asp:Label><br />
            Posted by <asp:Label runat="server" CssClass="CommentLabel" ID="UserLabel"></asp:Label><br />
            <asp:Literal runat="server" ID="CommentLabel"></asp:Literal>
      
        
    </div>
    <div>
        <asp:Literal runat="server" ID="ImageLiteral"></asp:Literal>
    </div>
</div>