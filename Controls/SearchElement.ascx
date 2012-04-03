<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchElement.ascx.cs" Inherits="SearchElement" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="true" 
    KeepInScreenBounds="true" VisibleOnPageLoad="false" Behaviors="none"
ID="MessageRadWindowManager" BackColor="#1b1b1b" Modal="true" RestrictionZoneID="RestrictionZone"
runat="server">
    <Windows>
        <telerik:RadWindow Width="400" EnableEmbeddedSkins="true" Skin="Vista"
         Height="500" VisibleStatusbar="false" VisibleTitlebar="false" ID="MessageRadWindow" 
         Title="Connect with this user" 
         runat="server">
        </telerik:RadWindow>
    </Windows>
</telerik:RadWindowManager>
<script type="text/javascript">
    var baseIcon = new GIcon(G_DEFAULT_ICON);
    function OpenConnect(varID, subject)
    {
        var win = $find("<%=MessageRadWindow.ClientID %>");
        win.setUrl("Controls/MessageAlert.aspx?T=Connect&A=friend&ID=" + varID+"&Subject="+subject);
        win.show(); 
        win.center(); 
    }
</script>
<div style="line-height: 14px;position: relative;" class="topDiv">
    <asp:Literal runat="server" ID="SearchLiteral"></asp:Literal>
<asp:Literal runat="server" ID="BeginingLiteral"></asp:Literal>

            <div style="position: absolute; right: 0px;width: 50px; cursor: pointer;"><img
            alt="Click to contact this user about this event!" runat="server"
            src="../NewImages/CommentBlurb.png" ID="ConnectImageButton" visible="false" /></div>

<asp:Panel runat="server" ID="RecomPanel" CssClass="FloatLeft"></asp:Panel>

    <asp:Literal runat="server" ID="ImageLiteral" Text=""></asp:Literal>
    <asp:Literal runat="server" ID="EventImageLiteral" Text=""></asp:Literal>
    <div style="clear: right;"><asp:HyperLink runat="server" ID="SearchLabel" CssClass="Navy14Link"></asp:HyperLink></div>
    <div class="TextNormal" style="clear: right; font-style: italic;"><asp:Label runat="server" ID="DateLabel"></asp:Label>
    <asp:Label runat="server" ID="VenueRealLabel"></asp:Label><span class="TextNormal"> at </span><asp:HyperLink runat="server" ID="VenueLabel" CssClass="NavyLink12"></asp:HyperLink></div><asp:Label runat="server" CssClass="TextNormal" ID="NumLabel"></asp:Label><asp:Label runat="server" ID="ShortDescriptionLabel" CssClass="Text11"></asp:Label>
    <asp:Literal runat="server" ID="TagsLiteral"></asp:Literal>
   
</div>
</div>