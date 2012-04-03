<%@ Page Language="C#" MasterPageFile="~/SecondMasterPage.master" AutoEventWireup="true" 
CodeFile="ResetPassword.aspx.cs" Inherits="ResetPassword" Title="Reset your Password" %>
<%@ Register Src="~/Controls/Ads.ascx" TagName="Ads" TagPrefix="ctrl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register TagName="BlueButton" TagPrefix="ctrl" Src="~/Controls/BlueButton.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript">
    function OpenRad()
        {
            var win = $find("<%=RadWindow1.ClientID %>");
            win.setUrl('PasswordAlert.aspx');
            win.show(); 
            win.center(); 
               
         }
</script>
 <asp:Panel runat="server" DefaultButton="SignInButton">
    <div class="Text12" style="float: left; padding-bottom: 20px;">
                                        <div id="topDiv2" style="width: 870px;">
        <div id="topDiv" class="RegisterDiv" style="float: left; width: 440px;"><h1>Reset your password</h1>
            <span style="font-family: Arial; font-size: 12px; line-height: 20px;">
            Reset password for <asp:Label ID="UserLabel" runat="server"></asp:Label>
          </span> <br /><br />
          <div id="topDiv3" style="clear:both;">
              <div style="float: left;">
                    <label>New Password</label> <br />
                    <asp:TextBox runat="server" ID="PasswordTextBox"></asp:TextBox>
                   <br /><label> Confirm Password</label> <br />
                   <asp:TextBox runat="server" ID="ConfirmPasswordTextBox"></asp:TextBox>
                    <asp:Label runat="server" ID="StatusLabel" ForeColor="Red"></asp:Label>
              </div>
              <div style="float: left; padding-left: 20px; padding-top: 50px;"><asp:Label runat="server" CssClass="ErrorLabel" ID="ErrorLabel"></asp:Label></div>
          </div>
          
          <div style="width: 440px;">
              <div align="right">
              <ctrl:BlueButton runat="server" BUTTON_TEXT="Reset" ID="SignInButton" />
                </div>
          </div>
         
        </div>

      </div>  
                                        
                                    </div>
                                    
                                    <ctrl:Ads ID="Ads2" runat="server" />
                                    <div style="float: right;padding-bottom: 100px;">
                                        <a class="NavyLinkSmall" href="post-bulletin">+Add Bulletin</a>
                                    </div>

     
      <rad:RadWindowManager Behaviors="none"
            ID="RadWindowManager" Modal="true" Skin="Black" DestroyOnClose="false" runat="server">
            <Windows>
                <rad:RadWindow ID="RadWindow1" VisibleStatusbar="false"  VisibleTitlebar="false" VisibleOnPageLoad="false" Height="250" Width="430"
                     runat="server">
                </rad:RadWindow>
            </Windows>
   </rad:RadWindowManager>
</asp:Panel>
</asp:Content>

