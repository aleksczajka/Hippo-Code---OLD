<%@ Page Language="C#" MasterPageFile="~/SecondMasterPage.master" AutoEventWireup="true" 
CodeFile="TripCategorySearch.aspx.cs" Inherits="TripCategorySearch" Title="Trip Category Search | HippoHappenings" %>
<%@ Register Src="~/Controls/TripSearchElements.ascx" TagName="SearchElements" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/Ads.ascx" TagName="Ads" TagPrefix="ctrl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <div class="EventDiv" style="float: left;"><asp:Label Width="418px" 
        runat="server" ID="SearchResultsTitleLabel"></asp:Label>
            <div align="left"  
            style="font-family: Arial; width: 418px; font-size: 12px; padding-bottom: 10px;">
                [all adventures here are for your location only]<br />
            </div>
            <div align="right" style=" padding-bottom: 20px;font-family: Arial; width: 418px; font-size: 12px;">
            <asp:Label runat="server" ID="NumResultsLabel"></asp:Label></div>
            <ctrl:SearchElements runat="server" ID="SearchElements" />
        </div>
        <div style="float: left;padding-top: 30px;">
            <div style="clear: both;">
                <ctrl:Ads ID="Ads1" runat="server" />
                <div style="float: right;padding-bottom: 100px;padding-right: 10px;">
            <a class="NavyLinkSmall">+Add Bulletin</a>
        </div>
            </div>
        </div>
</asp:Content>

