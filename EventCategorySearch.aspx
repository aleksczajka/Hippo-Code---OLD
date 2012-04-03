<%@ Page Language="C#" MasterPageFile="~/SecondMasterPage.master" AutoEventWireup="true" 
CodeFile="EventCategorySearch.aspx.cs" Inherits="EventCategorySearch" Title="Event Category Search | HippoHappenings" %>
<%@ Register Src="~/Controls/SearchElements.ascx" TagName="SearchElements" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/Ads.ascx" TagName="Ads" TagPrefix="ctrl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="Text12" style="float: left; padding-bottom: 20px;">
        <div style="float: left;"><asp:Label Width="418px" 
        runat="server" ID="SearchResultsTitleLabel"></asp:Label>
            <div class="EventDiv" style=" padding-bottom: 10px; clear: none;"><div align="left"  
            style="font-family: Arial; width: 418px; font-size: 12px; padding-bottom: 10px;">
                [all events here are for your location only]<br />
            </div><div 
            align="right" style="font-family: Arial; width: 418px; font-size: 12px;">
            <asp:Label runat="server" ID="NumResultsLabel"></asp:Label></div></div>
            <ctrl:SearchElements runat="server" ID="SearchElements" />
        </div>
</div>
        <div style="float: left;">                           
        <ctrl:Ads ID="Ads1" runat="server" />
        <div style="float: right;padding-bottom: 100px;padding-right: 10px;">
            <a class="NavyLinkSmall" href="post-bulletin">+Add Bulletin</a>
        </div>
        </div> 
</asp:Content>

