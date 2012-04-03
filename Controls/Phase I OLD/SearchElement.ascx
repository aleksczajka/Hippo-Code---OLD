<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchElement.ascx.cs" Inherits="Controls_SearchElement" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="true" 
    KeepInScreenBounds="true" VisibleOnPageLoad="false" Behaviors="none"
ID="MessageRadWindowManager" BackColor="#1b1b1b" Modal="true" DestroyOnClose="true" RestrictionZoneID="RestrictionZone"
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
    function OpenConnect()
    {
    
    }
</script>
    <asp:Literal runat="server" ID="SearchLiteral"></asp:Literal>
<asp:Literal runat="server" ID="BeginingLiteral"></asp:Literal>
            <div style="width: 50px; float: right;"><img alt="Click to contact this user about this event!" runat="server" onclick="OpenConnect()" src="image/CommentBlurb.png" ID="ConnectImageButton" visible="false" /></div>
<div style="float: left;"><asp:Panel runat="server" ID="RecomPanel" CssClass="FloatLeft"></asp:Panel>
    <asp:Literal runat="server" ID="ImageLiteral" Text=""></asp:Literal>
    <asp:HyperLink runat="server" ID="SearchLabel" CssClass="SearchLabel"></asp:HyperLink>
    <asp:Label runat="server" ID="DateLabel" CssClass="NumLabel"></asp:Label>
    <asp:Label runat="server" ID="VenueRealLabel"></asp:Label><asp:HyperLink runat="server" ID="VenueLabel" CssClass="VenueLabel"></asp:HyperLink><asp:Label runat="server" CssClass="NumLabel" ID="NumLabel"></asp:Label>
    <asp:Label runat="server" ID="ShortDescriptionLabel" CssClass="EventBody"></asp:Label>
    <asp:Literal runat="server" ID="TagsLiteral"></asp:Literal>
    </div>
</div>