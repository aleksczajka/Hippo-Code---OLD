<%@ Page Language="C#" MasterPageFile="~/SecondMasterPage.master" AutoEventWireup="true" 
CodeFile="AdCategorySearch.aspx.cs" Inherits="AdCategorySearch" Title="Ad Category Search | HippoHappenings" %>
<%@ Register Src="~/Controls/AdSearchElements.ascx" TagName="AdSearchElements" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/Ads.ascx" TagName="Ads" TagPrefix="ctrl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

        <div class="EventDiv" style="width: 420px;float: left;">
        <asp:Label Width="420px" runat="server"  ID="SearchResultsTitleLabel"></asp:Label>
            <div class="EventDiv" style=" padding-bottom: 20px; clear: none;"><div align="right" style="color: #999999; font-family: Arial; width: 445px; font-size: 12px;"><asp:Label runat="server" ID="NumResultsLabel"></asp:Label></div></div>
            <ctrl:AdSearchElements runat="server" ID="SearchElements" />
        </div>
        <div style="float: left;padding-top: 30px;">                           
        <ctrl:Ads ID="Ads1" runat="server" />
        <div style="float: right;padding-bottom: 100px;padding-right: 10px;">
            <a class="NavyLinkSmall" href="post-bulletin">+Add Bulletin</a>
        </div>
        </div> 
</asp:Content>

