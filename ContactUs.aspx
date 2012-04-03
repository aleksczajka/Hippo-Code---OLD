<%@ Page Language="C#" MasterPageFile="~/SecondMasterPage.master" ValidateRequest="false" 
AutoEventWireup="true" CodeFile="ContactUs.aspx.cs" Inherits="ContactUs" Title="Contact Us on HippoHappenings" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register TagName="BlueButton" TagPrefix="ctrl" Src="~/Controls/BlueButton.ascx" %>
<%@ Register TagName="Ads" TagPrefix="ctrl" Src="~/Controls/Ads.ascx" %>

<%@ OutputCache Duration="1" NoStore="true" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

                                      <div class="Text12" style="float: left; padding-bottom: 20px;">
                                        <span style="font-size: 18px;">Contact Us</span><br /><br />
                                        <div style="position: relative;">
                                            <asp:Panel runat="server" ID="LogInPanel">
                                                <div class="Text12">Please provide us with an email address or 
                                                <a class="NavyLink" href="register">Register</a> or 
                                                <a class="NavyLink" href="login">Log in</a>.</div>
                                                <br />
                                                <table>
                                                    <tr>
                                                        <td valign="top" width="250px">
                                                                
                                                                        <label>Email: </label>
                                                                    <br />
                                                                       <asp:TextBox runat="server" ID="EmailTextBox"></asp:TextBox>
                                                                   <br /><br />
                                                                        <label>Confirm Email: </label>
                                                                       <br />
                                                                        <asp:TextBox runat="server" ID="ConfirmTextBox"></asp:TextBox>
                                                                      
                                                        
                                                            
                                                        </td>
                                                        <td valign="top" width="300px">
                                                            <label>Tell us what you're contacting us about: </label><br />
                                                            <asp:TextBox runat="server" ID="TextBox1" Width="200"/><br />
                                                            
                                                            <label>Write your message:</label><br />
                                                           <asp:TextBox TextMode="multiLine" Wrap="true" runat="server" ID="TextBox2" Width="300px" 
                                                           Height="50px"></asp:TextBox>
                                                            
                                                       
                                                        </td>
                                                        <td valign="bottom" width="220px" style="padding-left: 100px;">
                                                        
                                                            <ctrl:BlueButton ID="SendItButton2" runat="server" BUTTON_TEXT="Send It!" /><br /><br /><br />
                                                            <asp:Label runat="server" ID="Label1" ForeColor="red" CssClass="NavyLink"></asp:Label>
                                                            <asp:Label runat="server" ID="Label2" ForeColor="red" CssClass="NavyLink"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel runat="server" ID="LoggedInPanel">
                                                
                                                   <table>
                                                    <tr>
                                                        <td valign="top" width="300px">
                                                            <label>Tell us what you're contacting us about: </label><br />
                                                            <asp:TextBox runat="server" ID="SubjectTextBox" Width="200"/><br />
                                                            <asp:Label runat="server" ID="SubjectRequired" CssClass="AddLink"></asp:Label><br />
                                                            <label>Write your message:</label><br />
                                                           <asp:TextBox TextMode="multiLine" Wrap="true" runat="server" ID="MessageTextBox" Width="300px" 
                                                           Height="50px"></asp:TextBox>
                                                            <asp:Label runat="server" ID="MessageRequired" CssClass="AddLink"></asp:Label>
                                                       
                                                        </td>
                                                        <td valign="bottom" width="220px" style="padding-left: 100px;">
                                                            <ctrl:BlueButton ID="SendItButton" runat="server" BUTTON_TEXT="Send It!" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                  
                                            </asp:Panel>
                                            <%--<div style="position: absolute;right: 10px;top: 0; width: 230px;">
                                                <label>If you would prefer to talk to us, 
                                                please give us a call at <b>(541) 41 HIPPO</b> <br />[(541) 414-4776].</label>
                                            </div>--%>
                                        </div>
                                    </div>
                                    
                                    <%--<ctrl:Ads runat="server" />
                                    <div style="float: right;padding-bottom: 100px;">
                                        <a class="NavyLinkSmall" href="post-bulletin">+Add Bulletin</a>
                                    </div>--%>

      
        


    <rad:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="false" 
                KeepInScreenBounds="true" VisibleOnPageLoad="false"  Behaviors="none"
            ID="MessageRadWindowManager" ReloadOnShow="true" Modal="true" 
            DestroyOnClose="true" RestrictionZoneID="RestrictionZone"
            runat="server">
                <Windows>
                    <rad:RadWindow  Width="300" ClientCallBackFunction="OnClientClose" 
                    EnableEmbeddedSkins="true" 
                    VisibleStatusbar="false" Skin="Web20" Height="200" ID="MessageRadWindow" 
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

