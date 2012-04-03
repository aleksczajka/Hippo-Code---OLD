<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
CodeFile="GroupEventCategorySearch.aspx.cs" Inherits="GroupEventCategorySearch" Title="Group Event Category search | HippoHappenings" %>
<%@ Register TagName="HippoTextBox" TagPrefix="ctrl" Src="~/Controls/HippoTextBox.ascx" %>
<%@ Register Src="~/Controls/GroupEventSearchElements.ascx" TagName="GroupEventSearchElements" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/Ads.ascx" TagName="Ads" TagPrefix="ctrl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

        <div class="EventDiv" style="float: left;"><h1 style="margin: 0;"><asp:Label Width="418px" runat="server" CssClass="EventHeader" ID="SearchResultsTitleLabel"></asp:Label></h1>
            <div class="EventDiv" style=" padding-bottom: 10px; clear: none;">
            <div align="left"  
            style="color: #999999; font-family: Arial; width: 418px; font-size: 12px; padding-bottom: 10px;">
                [all event on this page are for the future. to find events in 
                this category that have already happened, please use the group event search page.]<br />
            </div>
            <div align="right" style="color: #999999; font-family: Arial; width: 445px; font-size: 12px;">
            <asp:Label runat="server" ID="NumResultsLabel"></asp:Label></div></div>
            <ctrl:GroupEventSearchElements runat="server" ID="SearchElements" />
        </div>
        <div style="float: right;">
            <div style="clear: both;">
             <%--<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" 
            codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0" 
            width="431" height="575" title="banner" wmode="transparent">  
            <param name="movie" value="gallery.swf" />  
            <param name="quality" value="high" />
            <param name="wmode" value="transparent"/>   
                <embed src="gallery.swf" quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer" 
                type="application/x-shockwave-flash" width="431" height="575" wmode="transparent">
                </embed>
            </object>--%>
                 <ctrl:Ads ID="Ads1" runat="server" />
            </div>
        </div>
</asp:Content>

