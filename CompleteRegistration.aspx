<%@ Page Language="C#" MasterPageFile="~/SecondMasterPage.master" AutoEventWireup="true" 
CodeFile="CompleteRegistration.aspx.cs" Inherits="CompleteRegistration" Title="Complete HippoHappenings Account Registration" %>
<%@ Register Src="~/Controls/Ads.ascx" TagName="Ads" TagPrefix="ctrl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register Src="~/Controls/BlueButton.ascx" TagName="BlueButton" TagPrefix="ctrl" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

 <rad:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="false" 
                KeepInScreenBounds="true" VisibleOnPageLoad="false"  Behaviors="none"
            ID="MessageRadWindowManager" ReloadOnShow="true" Modal="true"
             RestrictionZoneID="RestrictionZone"
            runat="server">
                <Windows>
                    <rad:RadWindow Width="400" ClientCallBackFunction="OnClientClose" 
                    EnableEmbeddedSkins="true" 
                    VisibleStatusbar="false" Skin="Vista" Height="550" ID="MessageRadWindow" 
                    Title="Location Preferences" runat="server">
                    </rad:RadWindow>
                </Windows>
            </rad:RadWindowManager>
            <rad:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="true" 
    KeepInScreenBounds="true" VisibleOnPageLoad="false" Behaviors="Close"
ID="RadWindowManager1" Modal="true" DestroyOnClose="false" 
RestrictionZoneID="RestrictionZone"
runat="server">
    <Windows>
        <rad:RadWindow Width="730"  ReloadOnShow="true" IconUrl="./image/AddIcon.png"  VisibleTitlebar="false"
        EnableEmbeddedSkins="true" Skin="Vista" Height="550" VisibleStatusbar="false" 
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
    <div class="TextNormal" style="width: 820px; clear: both;">
        <div id="topDiv" class="RegisterDiv"><h1>Create an account</h1>
                Complete creating your account by filling out the information below. <br />
                HippoHappenings does not share or sell any of your information with any third party.<br />
                If you should want to modify these in the future, you can do so via 'My Preferences' in your My Account page.
               <br /><br />
              <label> 
                                  Your Email: <asp:Label runat="server" CssClass="NavyLink" ID="EmailLabel"></asp:Label></label><br />
                                    <br />
          
          <asp:UpdatePanel runat="server" UpdateMode="Conditional">
          <ContentTemplate>
          <asp:Panel runat="server" DefaultButton="MakeItSoButton">
          <div id="topDiv3" style="clear:both;">
          <table>
            <tr>
                <td valign="top" style="padding-right: 40px;">
                    <table>
                    <tr>
                            <td valign="top">
                                <label style="padding-right: 10px;">Country<span class="Asterisk">*</span></label><br />
                                <asp:DropDownList OnSelectedIndexChanged="ChangeState" AutoPostBack="true" runat="server" 
                                DataSourceID="SqlDataSourceCountry" DataTextField="country_name" DataValueField="country_id" ID="CountryDropDown"></asp:DropDownList>
                                <asp:SqlDataSource runat="server" ID="SqlDataSourceCountry" SelectCommand="SELECT * FROM Countries"
                                ConnectionString="<%$ ConnectionStrings:Connection %>"></asp:SqlDataSource>
                            </td>
                        </tr>
                    <tr>
                    <td valign="top">
                <div style="clear: both;"  class="Pad">
                    <table>
                         
                        <tr>
                            <td width="150px">
                                <label>State<span class="Asterisk">*</span></label>
                                <asp:Panel runat="server" ID="StateTextBoxPanel">
                                    <asp:TextBox ID="StateTextBox" runat="server" WIDTH="100" ></asp:TextBox>
                                </asp:Panel>
                                <asp:Panel runat="server" ID="StateDropDownPanel" Visible="false">
                                    <asp:DropDownList runat="server" ID="StateDropDown" OnSelectedIndexChanged="ChangeCity"></asp:DropDownList>
                                </asp:Panel>
                            
                            </td>
                            <td>
                                <label>Specific City<span class="Asterisk">*</span></label><br />
                                    <asp:TextBox ID="CityTextBox" runat="server" WIDTH="100" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            
                            <td>
                                <asp:Panel runat="server" ID="MajorCityPanel">
                                <label>Major City<span class="Asterisk">*</span></label><br />
                                    <asp:DropDownList runat="server" ID="MajorCityDropDown"></asp:DropDownList>
                               </asp:Panel>
                            </td>
                            <td>
                                <label>Zip<span class="Asterisk">*</span></label><br />
                                    <asp:TextBox ID="CatZipTextBox" runat="server" WIDTH="100" ></asp:TextBox>
                                    <br />
                            <asp:RangeValidator ControlToValidate="CatZipTextBox" ForeColor="red" 
                            ErrorMessage="Zip must be a 5 digit number." runat="server" ID="ZipValidator" Type="Integer" MaximumValue="99999" MinimumValue="00000"></asp:RangeValidator>

                            </td>
                        </tr>
                        
                    </table>
                </div>
                <br />
                        </td>
                    </tr>
                   
                </table>
                </td>
                <td>
                    <table>
                        <tr>
                        <td valign="top" style="padding-right: 20px;">
                            <div class="topDiv">
                                <div style="float: left;">
                                    <label> First Name</label>
                                </div>
                                <div>
                                    <asp:Image runat="server" CssClass="HelpImage" ID="QuestionMark6" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
                                     <rad:RadToolTip ID="RadToolTip5" runat="server" Skin="Sunset" Width="200px" Height="200px" 
                                     ManualClose="true" ShowEvent="onclick" Position="MiddleRight" 
                                     RelativeTo="Element" 
                                     TargetControlID="QuestionMark6">
                                         <div style="padding: 10px;">
                                            <label>
                                                Your first name and last name are only used for the 
                                                benefit of your friends finding you on Hippo, however, you
                                                do not need to include them.
                                            </label>
                                         </div>
                                    </rad:RadToolTip>
                                </div>
                            </div>
                       
                           <asp:TextBox runat="server" ID="FirstNameTextBox" WIDTH="150"  ></asp:TextBox><br />
                          </td>
                       <td valign="top"><label> Last Name</label> <br />
                                        <asp:TextBox runat="server" ID="LastNameTextBox" WIDTH="150" ></asp:TextBox><br />
                       </td>
                       </tr>
                       <tr>
                        <td valign="top" style="padding-right: 20px;">
                        
                        
                                       <label>User name<span class="Asterisk">*</span></label> <br />
                                        <asp:TextBox runat="server" ID="UserNameTextBox" WIDTH="150" ></asp:TextBox><br />
                                       
                        </td>
                       <td valign="top">
                                       <label>How did you hear about us?<span class="Asterisk">*</span></label> <br />
                                        <asp:TextBox runat="server" ID="HowHeardTextBox" WIDTH="150" ></asp:TextBox>
                       </td>
                       </tr>
                       <tr>
                        <td valign="top" style="padding-right: 20px;">
                                       <label> Password<span class="Asterisk">*</span></label> <br />
                                       <asp:TextBox runat="server" TextMode="password" Width="150px" ID="PasswordTextBox"></asp:TextBox><br />
                         </td>
                         <td valign="top">
                           <label> Confirm Password<span class="Asterisk">*</span></label><br />
                            <asp:TextBox runat="server" ID="ConfirmPasswordTextBox" TextMode="password" Width="150"></asp:TextBox><br />

                            </td>
                        </tr> 
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div class="topDiv">
                        <div style="float: left;">
                            <asp:CheckBox runat="server" ID="BossCheckBox" Checked="true" Text="Sign up for Hippo Points & Hippo Boss" />
                        </div>
                        <div>
                            <asp:Image runat="server" CssClass="HelpImage" ID="Image1" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
                             <rad:RadToolTip ID="RadToolTip1" runat="server" Skin="Sunset" Width="300px" Height="200px" 
                             ManualClose="true" ShowEvent="onclick" Position="MiddleRight" 
                             RelativeTo="Element" 
                             TargetControlID="Image1">
                                 <div style="padding: 10px;">
                                    <label>
                                        You might have already see the Hippo Boss winners displayed on our home page. These folks are
                                        the winners of this months Hippo Points contest and therefore dubbed Hippo Bosses of the month.
                                        The Hippo Points are simple. Post the most happenings, trips, locales or bulletins on the Hippo 
                                        and get featured on our home page, in their city for a full month. The person who wins globaly, not just in their city,
                                        gets to be featured on Hippo home pages world wide.
                                    </label>
                                </div>
                            </rad:RadToolTip>
                        </div>
                    </div>
                </td>
            </tr>
          </table>
          </div>
          <div>
              <div class="topDiv">
                <div style="float: left;">
                    <asp:Label runat="server" ForeColor="Red" ID="MessageLabel"></asp:Label>
                </div>
                <div style="float: right;">
                    <ctrl:BlueButton runat="server" id="MakeItSoButton" BUTTON_TEXT="Create My Account"></ctrl:BlueButton>
                </div>
              </div>
          </div>
          </asp:Panel>
          </ContentTemplate>
          </asp:UpdatePanel>
        </div>
    </div>
    <div style="width: 820px;clear: both;">
        <div style="padding-top: 40px;float: left;">
            <ctrl:Ads ID="Ads2" runat="server" />
            <div style="float: right;padding-right: 10px;">
                <a class="NavyLinkSmall" href="PostAnAd.aspx">+Add Bulletin</a>
            </div>
        </div>
    </div>
 </div>

</asp:Content>

