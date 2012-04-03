<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RateExperience.aspx.cs" Inherits="RateExperience" Title="Rate Hippo Happenings" %>
<%@ Register Src="~/Controls/Ads.ascx" TagName="Ads" TagPrefix="ctrl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register TagName="HippoTextBox" TagPrefix="ctrl" Src="~/Controls/HippoTextBox.ascx" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="topDiv">
        <div class="EventDiv" style="float: left; width: 430px;">
            <h1 style="margin-top: 0;">Rate Us</h1>
            <asp:Panel runat="server" ID="LogInPanel">
                <div class="AddGreenLink">You need to be logged in to rate Hippo Happenings. <br />Please <a class="AddLink" href="Register.aspx">Register</a> or <a class="AddLink" href="UserLogin.aspx">Log in</a>.</div>
            </asp:Panel>
            <asp:Panel runat="server" ID="ThankYouPanel" Visible="false">
                <asp:Label runat="server">Thank you for your feedback!</asp:Label>
            </asp:Panel>
            <asp:Panel runat="server" ID="LoggedInPanel">
                 
                   <label>How was your overall experience: </label><br />
                   <asp:RadioButtonList runat="server" ID="RatingRadioList">
                    <asp:ListItem Text="Very Poor" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                    <asp:ListItem Selected="true" Text="Hippo Hipptastic!" Value="4"></asp:ListItem>
                   </asp:RadioButtonList><br />
                    <asp:Label runat="server" ID="SubjectRequired" CssClass="AddLink"></asp:Label>
                   <label>Tell us what you liked about the experience OR let us know what we can do to make it better:</label>
                   <asp:TextBox TextMode="multiLine" Wrap="true" runat="server" ID="MessageTextBox" Width="400px" Height="100px"></asp:TextBox>
                    <asp:Label runat="server" ID="MessageRequired" CssClass="AddLink"></asp:Label>
                   <br /><br />
                   <asp:ImageButton runat="server" ID="SubmitButton" OnClick="SendIt" ImageUrl="~/image/SendItButton.png" onmouseout="this.src='image/SendItButton.png'"
                                        onmouseover="this.src='image/SendItButtonSelected.png'" />
            </asp:Panel>
        </div>
        
        <div style="float: right; padding-left: 5px; width: 419px; padding-right: 20px;">
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
            ID="MessageRadWindowManager" ReloadOnShow="true" Modal="true" 
             Left="200" Top="200"
            runat="server">
                <Windows>
                    <rad:RadWindow  Width="300" ClientCallBackFunction="OnClientClose" 
                    EnableEmbeddedSkins="true" Left="200" Top="200" 
                    VisibleStatusbar="false" Skin="Black" Height="300" ID="MessageRadWindow" 
                    Title="Alert" runat="server">
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

