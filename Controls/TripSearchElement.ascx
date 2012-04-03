<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TripSearchElement.ascx.cs" Inherits="TripSearchElement" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="true" 
    KeepInScreenBounds="true" VisibleOnPageLoad="false" Behaviors="none"
ID="MessageRadWindowManager" BackColor="#1b1b1b" Modal="true" DestroyOnClose="true" RestrictionZoneID="RestrictionZone"
runat="server">
    <Windows>
        <telerik:RadWindow Width="400" EnableEmbeddedSkins="true" Skin="Black"
         Height="500" VisibleStatusbar="false" VisibleTitlebar="false" ID="MessageRadWindow" Title="Connect with this user" 
         runat="server">
        </telerik:RadWindow>
    </Windows>
</telerik:RadWindowManager>
<script type="text/javascript">
    var baseIcon = new GIcon(G_DEFAULT_ICON);
</script>
<div style="line-height: 14px;" class="topDiv">
    <asp:Literal runat="server" ID="SearchLiteral"></asp:Literal>
<asp:Literal runat="server" ID="BeginingLiteral"></asp:Literal>
            <div style="width: 50px; float: right;"><asp:ImageButton 
            AlternateText="Click to contact this user about this event!" runat="server" OnClick="Connect" ImageUrl="~/image/CommentBlurb.png" ID="ConnectImageButton" Visible="false" /></div>
<asp:Panel runat="server" ID="RecomPanel" CssClass="FloatLeft"></asp:Panel>
    <asp:Literal runat="server" ID="ImageLiteral" Text=""></asp:Literal>
    <asp:Literal runat="server" ID="EventImageLiteral" Text=""></asp:Literal>
    <div style="clear: right;"><asp:HyperLink runat="server" ID="SearchLabel" CssClass="Navy14Link"></asp:HyperLink></div>
    <div class="TextNormal" style="clear: right; font-style: italic;"><asp:Label runat="server" ID="PriceRangeLabel"></asp:Label>
    </div><asp:Label runat="server" ID="ShortDescriptionLabel" CssClass="Text11"></asp:Label>
    <asp:Literal runat="server" ID="TagsLiteral"></asp:Literal>
</div>
</div>
