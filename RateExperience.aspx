<%@ Page Language="C#" MasterPageFile="~/SecondMasterPage.master" AutoEventWireup="true" 
CodeFile="RateExperience.aspx.cs" Inherits="RateExperience" Title="Rate HippoHappenings" %>
<%@ Register Src="~/Controls/Ads.ascx" TagName="Ads" TagPrefix="ctrl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register TagName="BlueButton" TagPrefix="ctrl" Src="~/Controls/BlueButton.ascx" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="Text12" style="float: left; padding-bottom: 20px;">
        <div >
            <h1>Rate Us</h1>
            <asp:Panel runat="server" ID="LogInPanel">
                <div class="AddGreenLink">You need to be logged in to rate Hippo Happenings. <br />Please <a class="AddLink" href="Register.aspx">Register</a> or <a class="AddLink" href="login">Log in</a>.</div>
            </asp:Panel>
            <asp:Panel runat="server" ID="ThankYouPanel" Visible="false">
                <asp:Label runat="server">Thank you for your feedback!</asp:Label>
            </asp:Panel>
            <asp:Panel runat="server" ID="LoggedInPanel">
                 
                   <label>How was your overall experience: </label><br />
                   <asp:RadioButtonList runat="server" RepeatDirection="horizontal" ID="RatingRadioList">
                    <asp:ListItem Text="Very Poor" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                    <asp:ListItem Selected="true" Text="Hippo Hipptastic!" Value="4"></asp:ListItem>
                   </asp:RadioButtonList><br />
                    <asp:Label runat="server" ID="SubjectRequired" CssClass="AddLink"></asp:Label>
                   <label>Tell us what you liked about the experience OR let us know what we can do to make it better:</label>
                   <asp:TextBox TextMode="multiLine" Wrap="true" runat="server" ID="MessageTextBox" Width="400px" Height="100px"></asp:TextBox>
                    <asp:Label runat="server" ID="MessageRequired" CssClass="NavyLink" ForeColor="red"></asp:Label>
                   <br /><br />
                   <ctrl:BlueButton runat="server" ID="SubmitButton" BUTTON_TEXT="Submit Rating" />
            </asp:Panel>
        </div>
        
    </div>
                                
    <ctrl:Ads ID="Ads2" runat="server" />
    <div style="float: right;padding-bottom: 100px;">
        <a class="NavyLinkSmall" href="post-bulletin">+Add Bulletin</a>
    </div>
    <rad:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="false" 
                KeepInScreenBounds="true" VisibleOnPageLoad="false"  Behaviors="none"
            ID="MessageRadWindowManager" ReloadOnShow="true" Modal="true" 
             Left="200" Top="200"
            runat="server">
                <Windows>
                    <rad:RadWindow  Width="300" ClientCallBackFunction="OnClientClose" 
                    EnableEmbeddedSkins="true" Left="200" Top="200" 
                    VisibleStatusbar="false" Skin="Vista" Height="300" ID="MessageRadWindow" 
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

