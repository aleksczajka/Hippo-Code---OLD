<%@ Page Language="C#" MasterPageFile="~/SecondMasterPage.master" AutoEventWireup="true" 
CodeFile="VenueCategorySearch.aspx.cs" Inherits="VenueCategorySearch" Title="Venue Category search | HippoHappenings" %>
<%@ Register TagName="HippoTextBox" TagPrefix="ctrl" Src="~/Controls/HippoTextBox.ascx" %>
<%@ Register Src="~/Controls/VenueSearchElements.ascx" TagName="VenueSearchElements" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/Ads.ascx" TagName="Ads" TagPrefix="ctrl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

        <div class="EventDiv" style="float: left;"><asp:Label Width="418px" runat="server" 
         ID="SearchResultsTitleLabel"></asp:Label>
            <div class="EventDiv" style=" padding-bottom: 20px; clear: none;"><div align="right" style="color: #999999; font-family: Arial; width: 445px; font-size: 12px;"><asp:Label runat="server" ID="NumResultsLabel"></asp:Label></div></div>
            <ctrl:VenueSearchElements runat="server" ID="SearchElements" />
        </div>
        <div style="float: left;padding-top: 30px;">                           
        <ctrl:Ads ID="Ads1" runat="server" />
        <div style="float: right;padding-bottom: 100px;padding-right: 10px;">
            <a class="NavyLinkSmall" href="post-bulletin">+Add Bulletin</a>
        </div>
        </div> 
</asp:Content>

