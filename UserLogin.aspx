<%@ Page Language="C#" MasterPageFile="~/SecondMasterPage.master" AutoEventWireup="true" 
CodeFile="UserLogin.aspx.cs" Inherits="UserLogin" Title="Log in to HippoHappenings" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register TagName="Ads" TagPrefix="ctrl" Src="~/Controls/Ads.ascx" %>
<%@ Register Src="~/Controls/BlueButton.ascx" TagName="BlueButton" TagPrefix="ctrl" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
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
 <asp:Panel runat="server" DefaultButton="LogInButton">
 <div class="topDiv">
 <div class="Text12" style="width: 820px; clear: both;padding-bottom: 30px;">
         <div>
            <h1>Log in to your account</h1>
            
          <table>
            <tr>
                
                <td valign="top" width="350px">
                    <label>Log in to your account or <a href="register" class="AddLink">Register</a> if you haven't already. 
                    </label> <br />
                    
                </td>
                <td valign="top">
                    <asp:Panel ID="Panel2" runat="server" DefaultButton="fakeSearchButton">
                    <div style="clear: both;">
                    <div style="float: left; width: 170px;">
                    <h2>User name</h2>
                    <asp:TextBox runat="server" ID="UserNameTextBox" Width="150px"></asp:TextBox>
                   </div>
                   <div style="float: left; width: 170px;">
                   <h2>Password</h2>
                   <asp:TextBox runat="server" TextMode="password" ID="PasswordTextBox" Width="150px"></asp:TextBox>
                   </div>
                   <div style="float: left; padding-top: 23px;">
                   <div style="display: none;"><asp:Button runat="server" OnClick="MakeItSo" ID="fakeSearchButton" />
                                                        </div>
                    <ctrl:BlueButton runat="server" BUTTON_TEXT="Log In" WIDTH="100px" ID="LogInButton" />
                   </div>
                   </div>
                   <div style="clear: both;">
                    <asp:Label runat="server" ID="StatusLabel" ForeColor="Red"></asp:Label><br />
                    <a class="NavyLink" href="javascript:OpenRad();">Forgot your password?</a> 
                    <div><asp:Label runat="server" CssClass="ErrorLabel" ID="MessageLabel"></asp:Label></div>
                    </div>
                    </asp:Panel>
                </td>
            </tr>
          </table>
          <br /><br />
          <h1>
            With a user account you can...
          </h1>
          <div align="center" style="padding-top: 5px;">*HippoHappenings <b>NEVER</b> sells, trades or shares any of your information 
          to anyone*</div>
         <table>
            <tr>
                <td valign="top" style="padding-top: 10px;padding-left: 30px;">
                    <ul type="disc" style="padding: 0; margin: 0;">
                        <li>
                            <h2>Post Events:</h2> yours or your favorite venue's
                            <ul type="disc" style="padding: 0; margin: 0;">
                                <li>
                                    <span>connect with people going to public events, make new friends, organize carpools</span>
                                </li>
                                <%--<li>
                                    <span>add events to your calendar, get updates if anything has changed for the event, get alert remminders</span>
                                </li>--%>
                                <li>
                                   <span>get event recommendations based on your favorite event categories, favorite venues and events similar to ones you have gone to</span>
                                </li>
                                <li>
                                    <span>edit any existing event that does not have an owner associated</span>
                                </li>
                            </ul>
                        </li>
                     </ul>
                </td>
                <td valign="top" style="padding-top: 10px;padding-left: 30px;">
                    <ul type="disc" style="padding: 0;margin: 0;">
                        <li>
                            <h2>Post Locales:</h2> tell us which one's are your favorites
                            <ul type="disc" style="padding: 0; margin: 0;">
                                <li>
                                    <span>edit any locale that does not have an owner associated</span>
                                </li>
                                <li>
                                    <span>rate locales</span>
                                </li>
                                <li>
                                    <span>mark as your favorite and see what events are going on in your favorite venues right on your My Account page</span>
                                </li>
                            </ul>
                        </li>
                    </ul>
                </td>
            </tr>
            <tr>
                
                <td colspan="2" align="center" valign="top" width="400px" style="padding-left: 30px;padding-top: 10px;">
                    <ul type="disc" style="padding: 0; margin: 0;width: 400px; text-align: left;">
                        <li>
                            <h2>Post Adventures:</h2>
                            <ul type="disc" style="padding: 0; margin: 0;">
                                <li>
                                    <span>share a favorite adventure or hike</span>
                                </li>
                                <li>
                                    <span>explore a new city through a scenic bike ride or famous landmarks</span>
                                </li>
                                <li>
                                    <span>get outside and find something to do other than shows and events</span>
                                </li>
                            </ul>
                        </li>
                     </ul>
                </td>
                <%--<td valign="top" style="padding-top: 10px;padding-left: 30px;">
                    <ul type="disc" style="padding: 0; margin: 0;">
                         <li>
                            <h2>Post Local Bulletins:</h2> lost pet, garage sale, or anything local
                             <ul type="disc" style="padding: 0; margin: 0;">
                                <li>
                                    <span>have your bulletin target users in any location with the categories you choose</span>
                                </li>
                                <li>
                                    <span>save a bulletin search and receive notifications of new bulletins matching your criteria</span>
                                </li>
                            </ul>
                        </li>
                    </ul>
                </td>--%>
            </tr>
         </table>
         <div style="padding-top: 20px;">
                     ...and much more. See full functionality listings on our <a class="NavyLink" href="about">About Page</a>.
                         
                 </div>
              </div>
        </div>  
        <%--<div style="width: 820px;clear: both;">
            <div style="padding-top: 40px;float: left;">
                <ctrl:Ads ID="Ads2" runat="server" />
                <div style="float: right;padding-right: 10px;">
                    <a class="NavyLinkSmall" href="post-bulletin">+Add Bulletin</a>
                </div>
            </div>
        </div>--%>
</div>
      <rad:RadWindowManager Behaviors="none"
            ID="RadWindowManager" Modal="true" Skin="Web20" Left="10px" DestroyOnClose="false" runat="server">
            <Windows>
                <rad:RadWindow ID="RadWindow1" VisibleStatusbar="false" 
                VisibleTitlebar="false" VisibleOnPageLoad="false"  Left="10px" Height="250" Width="430"
                     runat="server">
                </rad:RadWindow>
            </Windows>
   </rad:RadWindowManager>
</asp:Panel>
</asp:Content>

