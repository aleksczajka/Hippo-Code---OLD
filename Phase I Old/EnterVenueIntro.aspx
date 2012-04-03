<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EnterVenueIntro.aspx.cs" Inherits="EnterVenueIntro" Title="Enter Venue | Hippo Happenings" %>
<%@ Register TagName="HippoTextBox" TagPrefix="ctrl" Src="~/Controls/HippoTextBox.ascx" %>
<%@ Register Src="~/Controls/Ads.ascx" TagName="Ads" TagPrefix="ctrl" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <div id="topDiv2" style="width: 870px;">
    <div id="topDiv" class="RegisterDiv" style="float: left; width: 440px;">
    <span style="font-family: Arial; font-size: 30px; color: #666666;"><asp:Label runat="server" ID="VenueTitle"></asp:Label></span><br />
         <br />
         <span style="font-family: Arial; font-size: 12px; color: #cccccc; line-height: 20px;">
            <asp:Label runat="server" ID="WelcomeLabel"></asp:Label>
          </span> <br /><br />
    </div>
    <div style="float: right; padding-right: 2px; width: 419px;">
        <div style="clear: both;">
         <%-- <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" 
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
  </div>  

</asp:Content>

