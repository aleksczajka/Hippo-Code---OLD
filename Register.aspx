<%@ Page Language="C#" MasterPageFile="~/SecondMasterPage.master" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Register" Title="Register on Hippo Happenings" %>
<%@ Register Src="~/Controls/Ads.ascx" TagName="Ads" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/BlueButton.ascx" TagName="BlueButton" TagPrefix="ctrl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

 <rad:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="false" 
                KeepInScreenBounds="true" VisibleOnPageLoad="false"  Behaviors="none"
            ID="MessageRadWindowManager" ReloadOnShow="true" Modal="true" DestroyOnClose="true" 
            RestrictionZoneID="RestrictionZone"
            runat="server">
                <Windows>
                    <rad:RadWindow Width="300" ClientCallBackFunction="OnClientClose" EnableEmbeddedSkins="true" 
                    VisibleStatusbar="false" Skin="Web20" Height="200" ID="MessageRadWindow" Title="Your email has been sent!" runat="server">
                    </rad:RadWindow>
                </Windows>
            </rad:RadWindowManager>
             <rad:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="true" 
    KeepInScreenBounds="true" VisibleOnPageLoad="false" Behaviors="Close"
ID="RadWindowManager1" BackColor="#1b1b1b" Modal="true" DestroyOnClose="false" 
RestrictionZoneID="RestrictionZone"
runat="server">
    <Windows>
        <rad:RadWindow Width="730"  ReloadOnShow="true" IconUrl="./image/AddIcon.png"  VisibleTitlebar="false"
        EnableEmbeddedSkins="true" Skin="Web20" Height="550" VisibleStatusbar="false" 
        ID="RadWindow1" Title="Add to Favorites" runat="server">
        </rad:RadWindow>
    </Windows>
</rad:RadWindowManager>   
            <script type="text/javascript">
            function OnClientClose(oWnd, args)
            {

                window.location = args;
            }
           function OpenRad()
        {
            var win = $find("<%=RadWindow1.ClientID %>");
            win.setUrl('TermsAndConditionsOpen.aspx');
            win.show(); 
            win.center(); 
               
         }
            </script>
<div class="topDiv">
 <div class="Text12" style="width: 820px; clear: both;padding-bottom: 30px;">
         <div>
            <h1>Create an account</h1>
            
          <table>
            <tr>
                
                <td valign="top" width="350px">
                   <label>Enter an email address for verification. <br />
              You will receive an email with a link to set up your account.</label>
                    
                </td>
                <td valign="top" style="padding-left: 20px;">
<div id="topDiv3" style="clear:both;">
          <div style="float: left;">

                <div style="float: left; width: 170px;">
                    <label> <b >Email</b></label> <br />
                    <asp:TextBox runat="server" ID="EmailTextBox" Width="150px" ></asp:TextBox>
                </div>
                <div style="float: left; width: 170px;">
                    <label> <b>Confirm Email</b> </label><br />
                    <asp:TextBox runat="server" ID="ConfirmEmailTextBox" Width="150" /><br />
                </div>
                     </div><br /><br /><br />
                      <div style="float: left;"><asp:Label runat="server" ForeColor="Red" ID="MessageLabel"></asp:Label></div>
                  </div>
                  <div style="width: 440px;">
                      <br />  
                      <asp:CheckBox runat="server" ID="TermsCheckBox" Text="I have read and agree to the " /><a class="NavyLink12UD" onclick="OpenRad();">terms and conditions</a>
                      <br />  <br />  
                      <ctrl:BlueButton runat="server" ID="MakeItSoButton" BUTTON_TEXT="Make It So" />
                  </div>                </td>
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
                     ...and much more. See full description of HippoHappenings on our <a class="NavyLink" href="about">About Page</a>.
                         
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

</asp:Content>

