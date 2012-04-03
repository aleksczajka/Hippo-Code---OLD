<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ValidateRequest="false" 
AutoEventWireup="true" CodeFile="Vote.aspx.cs" Inherits="Vote" Title="Vote! HippoHappenings" %>
<%@ Register Src="~/Controls/Ads.ascx" TagName="Ads" TagPrefix="ctrl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register TagName="HippoTextBox" TagPrefix="ctrl" Src="~/Controls/HippoTextBox.ascx" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="topDiv">
        <div class="EventDiv" style="float: left; width: 430px;">
            <h1 style="margin: 0;">Tell us what to build next!</h1>
            <label style="line-height: 16px;">
            HippoHappenings is working on the next phase of 
            functionality. 
            Some thing we already know our viewers would like, for others we would like to 
            know what you want to see.
            Tell us and we'll build it! (You will only be able to vote once.) </label>
            <asp:Panel runat="server" ID="LogInPanel">
                <div class="AddWhiteLink"><a class="AddLink" href="UserLogin.aspx">Log in</a> to Vote.</div>
            </asp:Panel>
            
            <div class="EventDiv" style="float: left; padding-top: 30px;">
                <asp:Panel runat="server" ID="FuncPanel">
                    
                </asp:Panel>
            </div>
        </div>
        
        <div style="float: right; padding-left: 5px; width: 419px; padding-right: 12px;">
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
    </div>
    <rad:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="false" 
                KeepInScreenBounds="true" VisibleOnPageLoad="false"  Behaviors="none"
            ID="MessageRadWindowManager" ReloadOnShow="true" Modal="true" DestroyOnClose="true" RestrictionZoneID="RestrictionZone"
            runat="server">
                <Windows>
                    <rad:RadWindow  Width="300" ClientCallBackFunction="OnClientClose" EnableEmbeddedSkins="true" 
                    VisibleStatusbar="false" Skin="Black" Height="300" ID="MessageRadWindow" Title="Alert" runat="server">
                    </rad:RadWindow>
                </Windows>
            </rad:RadWindowManager>
            <script type="text/javascript">
            function OnClientClose(oWnd, args)
            {

                window.location = args;
            }
           
            </script>
</asp:Content>

