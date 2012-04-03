<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ValidateRequest="false" 
AutoEventWireup="true" CodeFile="ContactUs.aspx.cs" Inherits="ContactUs" Title="Contact Us on HippoHappenings" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register TagName="HippoTextBox" TagPrefix="ctrl" Src="~/Controls/HippoTextBox.ascx" %>
<%@ Register TagName="Ads" TagPrefix="ctrl" Src="~/Controls/Ads.ascx" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="topDiv">
        <div class="EventDiv" style="float: left; width: 430px;">
            <h1 style="margin: 0;">Contact Us</h1>
            <asp:Panel runat="server" ID="LogInPanel">
                <div class="AddWhiteLink">Please provide us with email address or 
                <a class="AddLink" href="Register.aspx">Register</a> or 
                <a class="AddLink" href="UserLogin.aspx">Log in</a>.</div>
                <br />
                <table>
                    <tr>
                       <td>
                        <label>Email: </label>
                       </td>
                       <td>
                       <asp:TextBox runat="server" ID="EmailTextBox"></asp:TextBox>
                       </td>
                    </tr>
                    <tr>
                       <td>
                        <label>Confirm Email: </label>
                       </td>
                       <td>
                        <asp:TextBox runat="server" ID="ConfirmTextBox"></asp:TextBox>
                       </td>
                    </tr>
                </table>
                
               <br />
                   <label>Tell us what you're contacting us about: </label><br />
                   <asp:TextBox runat="server" ID="TextBox1" Width="200"/><br />
                    <asp:Label runat="server" ID="Label1" CssClass="AddLink"></asp:Label>
                   <br /><label>Write your message:</label>
                   <asp:TextBox TextMode="multiLine" Wrap="true" runat="server" ID="TextBox2" Width="400px" Height="200px"></asp:TextBox>
                    <asp:Label runat="server" ID="Label2" CssClass="AddLink"></asp:Label>
                   <br /><br />
                   <asp:ImageButton runat="server" ID="ImageButton1" OnClick="SendItNotLoggedIn" ImageUrl="~/image/SendItButton.png" onmouseout="this.src='image/SendItButton.png'"
                                        onmouseover="this.src='image/SendItButtonSelected.png'" />
            
            </asp:Panel>
            <asp:Panel runat="server" ID="LoggedInPanel">
                   <br />
                   <label>Tell us what you're contacting us about: </label><br />
                   <asp:TextBox runat="server" ID="SubjectTextBox" Width="200"/><br />
                    <asp:Label runat="server" ID="SubjectRequired" CssClass="AddLink"></asp:Label>
                   <br /><label>Write your message:</label>
                   <asp:TextBox TextMode="multiLine" Wrap="true" runat="server" ID="MessageTextBox" Width="400px" Height="200px"></asp:TextBox>
                    <asp:Label runat="server" ID="MessageRequired" CssClass="AddLink"></asp:Label>
                   <br /><br />
                   <asp:ImageButton runat="server" ID="SubmitButton" OnClick="SendIt" ImageUrl="~/image/SendItButton.png" onmouseout="this.src='image/SendItButton.png'"
                                        onmouseover="this.src='image/SendItButtonSelected.png'" />
            </asp:Panel>
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
    </div>
    <rad:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="false" 
                KeepInScreenBounds="true" VisibleOnPageLoad="false"  Behaviors="none"
            ID="MessageRadWindowManager" ReloadOnShow="true" Modal="true" 
            DestroyOnClose="true" RestrictionZoneID="RestrictionZone"
            runat="server">
                <Windows>
                    <rad:RadWindow  Width="300" ClientCallBackFunction="OnClientClose" EnableEmbeddedSkins="true" 
                    VisibleStatusbar="false" Skin="Black" Height="200" ID="MessageRadWindow" 
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

