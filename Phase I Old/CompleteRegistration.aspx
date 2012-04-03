<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
CodeFile="CompleteRegistration.aspx.cs" Inherits="CompleteRegister" Title="Complete Account Registration on HippoHappenings" %>
<%@ Register TagName="HippoTextBox" TagPrefix="ctrl" Src="~/Controls/HippoTextBox.ascx" %>
<%@ Register Src="~/Controls/Ads.ascx" TagName="Ads" TagPrefix="ctrl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
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
                    VisibleStatusbar="false" Skin="Black" Height="550" ID="MessageRadWindow" 
                    Title="Location Preferences" runat="server">
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
        EnableEmbeddedSkins="true" Skin="Office2007" Height="550" VisibleStatusbar="false" 
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
 <div id="topDiv2" style="width: 870px;">
    <div id="topDiv" class="RegisterDiv" style="float: left; width: 440px;"><span style="font-family: Arial; font-size: 30px; color: White;">Create an account</span><br />
         <span style="font-family: Arial; font-size: 12px; color: #cccccc; line-height: 20px;">
            Complete creating your account by filling out the information below. <br />
            The address is needed for filtering ads and events on the site. <br />
            Enter your First Name and Last Name if you want users to find you in the friend search. 
            HippoHappenings does not share or sell any of your information with any third party.
            You can always add or remove these fields later in your user preferences. 
            If you included one of these, you must include the other.
          </span> <br /><br />
          <label> 
                              Your Email: <asp:Label runat="server" ID="EmailLabel"></asp:Label></label><br />
                                <br />
      
      <asp:UpdatePanel runat="server" UpdateMode="Conditional">
      <ContentTemplate>
      <asp:Panel runat="server" DefaultButton="MakeItSoButton">
      <div id="topDiv3" style="clear:both;">
            <table>
                <tr>
                        <td>
                            <label style="padding-right: 10px;">*Country</label><br />
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
                            <label>*State</label>
                            <asp:Panel runat="server" ID="StateTextBoxPanel">
                                <ctrl:HippoTextBox ID="StateTextBox" runat="server" TEXTBOX_WIDTH="100" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                            </asp:Panel>
                            <asp:Panel runat="server" ID="StateDropDownPanel" Visible="false">
                                <asp:DropDownList runat="server" ID="StateDropDown"></asp:DropDownList>
                            </asp:Panel>
                        
                        </td>
                        <td>
                            <label>*City</label><br />
                                <ctrl:HippoTextBox ID="CityTextBox" runat="server" TEXTBOX_WIDTH="100" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                        </td>
                    </tr>
                    <tr>
                        <td width="150px">
                            <label>Zip [Not Required]</label><br />
                                <ctrl:HippoTextBox ID="CatZipTextBox" runat="server" TEXTBOX_WIDTH="100" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                        </td>
                        <td>
                            <asp:Panel runat="server" ID="RadiusPanel">
                            <label>Radius [Not Required]</label><br />
                            <asp:DropDownList runat="server" ID="RadiusDropDown">
                                <asp:ListItem Value="0" Text="Just Zip Code (0 miles)"></asp:ListItem>
                                <asp:ListItem Value="1" Text="1 mile"></asp:ListItem>
                                <asp:ListItem Value="5" Text="5 miles"></asp:ListItem>
                                <asp:ListItem Value="10" Text="10 miles"></asp:ListItem>
                                <asp:ListItem Value="15" Text="15 miles"></asp:ListItem>
                                <asp:ListItem Value="30" Text="30 miles"></asp:ListItem>
                                <asp:ListItem Value="50" Text="50 miles"></asp:ListItem>
                            </asp:DropDownList>
                            </asp:Panel>
                        </td>
                    </tr>
                    
                </table>
            </div>
            <br />
                    </td>
                    
                    
                </tr>
                <tr>
                    <td>
                        <div style="float: left;"><label> First Name</label> <br />
                                    <ctrl:HippoTextBox runat="server" ID="FirstNameTextBox" TEXTBOX_WIDTH="150" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" /><br />
                      <br /><label> Last Name</label> <br />
                                    <ctrl:HippoTextBox runat="server" ID="LastNameTextBox" TEXTBOX_WIDTH="150" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" /><br />
                    <br />
                                   <label>User name*</label> <br />
                                    <ctrl:HippoTextBox runat="server" ID="UserNameTextBox" TEXTBOX_WIDTH="150" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" /><br />
                                   
                                   <br />
                                   <label>How did you hear about us?*</label> <br />
                                    <ctrl:HippoTextBox runat="server" ID="HowHeardTextBox" TEXTBOX_WIDTH="150" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                                    <br />
                                   
                                   <br />
                                   <label> Password*</label> <br />
                                   <asp:TextBox runat="server" TextMode="password" Width="150px" ID="PasswordTextBox"></asp:TextBox><br />
                    <%--                <ctrl:HippoTextBox runat="server" ID="PasswordTextBox" TEXTBOX_WIDTH="150" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" /><br />
                    --%>              <br /> <label> Confirm Password*</label><br />
                    <asp:TextBox runat="server" ID="ConfirmPasswordTextBox" TextMode="password" Width="150"></asp:TextBox><br />
                    <%--                <ctrl:HippoTextBox runat="server" ID="ConfirmPasswordTextBox" TEXTBOX_WIDTH="150" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" /><br />
                    --%>          
<%--                          <br /><label> Who Referred You?* [Type in the UserName of the Referring user to give them credit in the ongoing contest. If you were not referred by anyone, type 'HippoHappenings'.]</label> <br />
                          <asp:TextBox runat="server" Width="150px" ID="ReferringUserName"></asp:TextBox><br />
--%>
                    
                    </div>
                    </td>
                </tr>
            </table>
          
          <div style="float: left; padding-left: 20px; padding-top: 50px;"><asp:Label runat="server" CssClass="ErrorLabel" ID="MessageLabel"></asp:Label></div>
      </div>
      <div style="width: 440px;">
          <br />  
<%--          <asp:UpdatePanel runat="server" >
            
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="CheckImageButton" EventName="Click" />
            </Triggers>
            <ContentTemplate>
            
           
          <asp:ImageButton runat="server" ID="CheckImageButton" ImageUrl="image/Check.png" OnClick="ChangeCheckImage" /><label> I have read and agree to the <a class="AddLink" onclick="OpenRad();">terms and conditions</a></label>
           </ContentTemplate>
          </asp:UpdatePanel>
--%>          
          <div align="right"><asp:ImageButton runat="server" ID="MakeItSoButton" ImageUrl="~/image/MakeItSoButton.png" onmouseout="this.src='image/MakeItSoButton.png'" onmouseover="this.src='image/MakeItSoButtonSelected.png'" OnClick="MakeItSo" /></div>
      </div>
      </asp:Panel>
      </ContentTemplate>
      </asp:UpdatePanel>
    </div>
    <div style="float: left; padding-left: 5px; width: 419px;">
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

</asp:Content>

