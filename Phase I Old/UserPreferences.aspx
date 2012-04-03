<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UserPreferences.aspx.cs" Inherits="UserPreferences" Title="Your Preferences" %>
<%@ Register Src="~/Controls/Comments.ascx" TagName="Comments" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/SearchElements.ascx" TagName="SearchElements" TagPrefix="ctrl" %>
<%@ Register TagName="HippoTextBox" TagPrefix="ctrl" Src="~/Controls/HippoTextBox.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div id="topDiv" class="EventDiv">
        <telerik:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="true" 
                KeepInScreenBounds="true" VisibleOnPageLoad="false"  Behaviors="none"
            ID="MessageRadWindowManager" Modal="true" RestrictionZoneID="RestrictionZone"
            runat="server">
                <Windows>
                    <telerik:RadWindow Width="300" EnableEmbeddedSkins="true" 
                    VisibleStatusbar="false" Skin="Black" Height="200" ID="MessageRadWindow" 
                    Title="Alert" runat="server">
                    </telerik:RadWindow>
                </Windows>
            </telerik:RadWindowManager>
        <a id="top"></a>
       <asp:Label runat="server" CssClass="EventHeader" ID="UserNameLabel"></asp:Label> 
       <div style="width: 600px; " align="right" >
        <div style="padding-right: 110px;">
            <asp:Label runat="server" ID="ErrorLabel" ForeColor="Red"></asp:Label>
        <label>Save your changes: </label></div><asp:ImageButton runat="server" ID="ImageButton1" ImageUrl="~/image/MakeItSoButton.png" 
                    OnClick="Save" onmouseout="this.src='image/MakeItSoButton.png'" 
                    onmouseover="this.src='image/MakeItSoButtonSelected.png'"  /></div>
        <div class="Prefs" style=" width: 800px ;float: left; padding-top: 5px; padding-bottom: 100px;">
             
            <div style=" width: 400px ;height: 171px;background-image: url('image/FriendBackground.png'); background-repeat: no-repeat;">
                <div style="padding-left: 10px; padding-top: 10px; float: left;">
                    <asp:Image runat="server" ID="FriendImage"  Width="150px" Height="150" />
                </div>
                <div style="float: left; padding-left: 10px; padding-top: 10px;  padding-right:40px;" class="FriendDiv">
                    <table>
                        <tr>
                            <td>
                                <label>Age</label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" BackColor="#666666" BorderColor="#1b1b1b" ID="AgeTextBox" Width="80px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>Sex</label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" BackColor="#666666" BorderColor="#1b1b1b"  ID="SexTextBox" Width="80px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>Location</label>
                            </td>
                            <td>
                               <asp:TextBox runat="server" BackColor="#666666" BorderColor="#1b1b1b"  ID="LocationTextBox" Width="80px" />
                            </td>
                        </tr>
                        
                    </table>


                   <div>
                                <label>Events Posted</label>
                            
                               <asp:Label CssClass="AddLinkBig" runat="server" ID="EventsLabel"></asp:Label>
                    </div>
                   <div>
                                <label>Events Attended</label>
                           
                               <asp:Label CssClass="AddLinkBig" runat="server" ID="AttendedLabel"></asp:Label>
                   </div>
               
  
                </div>
            </div>
            <div class="Pad">
                <br />
                <asp:Label ID="Label7" runat="server" Text="Calendar Preferences" CssClass="PreferencesTitle">Upload a new Photo</asp:Label><br />
                <div style="padding-left: 20px;">
                    <asp:FileUpload runat="server" ID="PictureUpload" EnableViewState="true" />
                    <asp:ImageButton runat="server" PostBackUrl="#top" ID="AdsLink" ImageUrl="~/image/MakeItSoButton.png" 
                    OnClick="UploadPhoto" onmouseout="this.src='image/MakeItSoButton.png'" 
                    onmouseover="this.src='image/MakeItSoButtonSelected.png'"  />
                </div>
            </div>
            <div style="border-bottom: dotted 1px black;">
                <asp:Label ID="Label11" runat="server" Text="Filter Location" CssClass="PreferencesTitleAlt"></asp:Label><span style="font-style: italic; font-family: Arial; font-size: 14px; color: #666666;"> (Events and Ads on this site are filtered base on this preference. You will still be able to search on everything else, however, your recommendations and the ads you see will be base on this preference.)</span>
            </div>
            <div style="clear: both;"  class="Pad">
                <table>
                     <tr>
                        <td colspan="2">
                            <label style="padding-right: 10px;">*Country</label><br />
                            <asp:DropDownList OnSelectedIndexChanged="ChangeState" AutoPostBack="true" runat="server" DataSourceID="SqlDataSourceCountry" DataTextField="country_name" DataValueField="country_id" ID="CountryDropDown"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="SqlDataSourceCountry" SelectCommand="SELECT * FROM Countries"
                            ConnectionString="<%$ ConnectionStrings:Connection %>"></asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            <label>State</label>
                            <asp:Panel runat="server" ID="StateTextBoxPanel">
                                <ctrl:HippoTextBox ID="StateTextBox" runat="server" TEXTBOX_WIDTH="100" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                            </asp:Panel>
                            <asp:Panel runat="server" ID="StateDropDownPanel" Visible="false">
                                <asp:DropDownList runat="server" ID="StateDropDown"></asp:DropDownList>
                            </asp:Panel>
                        </td>
                        <td>
                            <label>City</label><br />
                                <ctrl:HippoTextBox ID="CityTextBox" runat="server" TEXTBOX_WIDTH="100" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="border-bottom: dotted 1px black;">
                <asp:Label runat="server" Text="Profile Preferences" CssClass="PreferencesTitleAlt"></asp:Label>
            </div>
            <div style="clear: both;"  class="Pad">
                <div style="float:left;">
                    <asp:RadioButtonList runat="server" ID="PublicPrivateCheckList">
                        <asp:ListItem Value="1">Make Public <span style="font-style: italic; font-family: Arial; font-size: 14px; color: #666666;">(others will be able to see if you are going to an event and see your full profile)</span></asp:ListItem>
                        <asp:ListItem Value="2">Show Only To Friends</asp:ListItem>
                        <asp:ListItem Value="3">Make Private</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
            <div style="border-bottom: dotted 1px black; clear: both; padding-top: 10px;">
                <asp:Label ID="Label1" runat="server" Text="Communication Preferences" CssClass="PreferencesTitle"></asp:Label>
            </div>
            <div  class="Pad">
                <asp:RadioButtonList runat="server" ID="CommunicationPrefsRadioList">
                    <asp:ListItem Value="1">On for Everyone <span style="font-style: italic; font-family: Arial; font-size: 14px; color: #666666;">(this means users can contact you to carpool, etc. when you have added an event to your calendar. Great choice for meeting new friends!)</span></asp:ListItem>
                    <asp:ListItem Value="2">I Will Set Per Event</asp:ListItem>
                    <asp:ListItem Value="3">On Only for Friends</asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div style="border-bottom: dotted 1px black; clear: both; padding-top: 10px;">
                <asp:Label ID="Label2" runat="server" CssClass="PreferencesTitleAlt"> Category Interests <span style="font-style: italic; font-family: Arial; font-size: 14px; color: #666666;">(Ads and Searches will be filtered based on the categories you select)</span></asp:Label>
            </div>
            <div style="padding-left: 50px;">
                <asp:RadioButtonList runat="server" ID="CategoriesOnOffRadioList" RepeatDirection="horizontal">
                    <asp:ListItem Value="1" Selected="true">On </asp:ListItem>
                    <asp:ListItem Value="2">Off  <span style="font-style: italic; font-family: Arial; font-size: 14px; color: #666666;">(You can keep your category choices saved here and simply choose to turn them on or off.)</span></asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div  class="Pad">
                
<table>
                                        <tr>
                                            <td valign="top">
                                                    <%--<asp:CheckBoxList runat="server" ID="CategoriesCheckBoxes" 
                                                    RepeatColumns="4" RepeatDirection="horizontal">
                                                    
                                                    </asp:CheckBoxList>--%>
                                                    <div class="FloatLeft" style="width: 200px;">
                                                        <rad:RadTreeView Width="200px" runat="server"  
                                                        ID="CategoryTree" DataFieldID="ID" ForeColor="#cccccc" Height="150px" 
                                                        Font-Size="14px" DataSourceID="SqlDataSource2" 
                                                        DataFieldParentID="ParentID"
                                                        CheckBoxes="true">
                                                        <DataBindings>
                                                             <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                             TextField="Name" Expanded="false" ValueField="ID" />
                                                         </DataBindings>
                                                        </rad:RadTreeView>
                                                        <asp:SqlDataSource runat="server" ID="SqlDataSource2" 
                                                            SelectCommand="SELECT * FROM AdCategories WHERE ID <= 17"
                                                            ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                        </asp:SqlDataSource>
                                                    </div>
                                                    
                                               
                                            </td>
                                            <td valign="top">
                                              
                                                    <div class="FloatLeft" style="width: 200px;">
                                                        <rad:RadTreeView Width="200px" runat="server"  
                                                        ID="RadTreeView2" DataFieldID="ID" Height="150px" ForeColor="#cccccc" Font-Size="14px"  DataSourceID="SqlDataSource3" 
                                                        DataFieldParentID="ParentID"
                                                        CheckBoxes="true">
                                                        <DataBindings>
                                                             <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                             TextField="Name" Expanded="false" ValueField="ID"  />
                                                         </DataBindings>
                                                        </rad:RadTreeView>
                                                        <asp:SqlDataSource runat="server" ID="SqlDataSource3" 
                                                            SelectCommand="SELECT * FROM AdCategories WHERE ID <= 24 AND ID > 17"
                                                            ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                        </asp:SqlDataSource>
                                                    </div>
                                            </td>
                                            <td valign="top">
                                                <div style=" width: 200px;">
                                                    <rad:RadTreeView Width="200px"  runat="server"  
                                                    ID="RadTreeView1" DataFieldID="ID" Height="150px"  ForeColor="#cccccc" Font-Size="14px"  DataSourceID="SqlDataSource4" 
                                                    DataFieldParentID="ParentID"
                                                    CheckBoxes="true">
                                                    <DataBindings>
                                                         <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                         TextField="Name" Expanded="false" ValueField="ID"  />
                                                     </DataBindings>
                                                    </rad:RadTreeView>
                                                    <asp:SqlDataSource runat="server" ID="SqlDataSource4" 
                                                        SelectCommand="SELECT * FROM AdCategories WHERE  ID <= 99 AND ID > 24"
                                                        ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                    </asp:SqlDataSource>
                                                </div>
                                            </td>
                                            <td valign="top">
                                                <div style=" width: 200px;">
                                                    <rad:RadTreeView Width="200px" Height="150px"  runat="server"  
                                                    ID="RadTreeView3" DataFieldID="ID" ForeColor="#cccccc" Font-Size="14px"  
                                                    DataSourceID="SqlDataSource5" 
                                                    DataFieldParentID="ParentID"
                                                    CheckBoxes="true">
                                                    <DataBindings>
                                                         <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                         TextField="Name" Expanded="false" ValueField="ID"  />
                                                     </DataBindings>
                                                    </rad:RadTreeView>
                                                    <asp:SqlDataSource runat="server" ID="SqlDataSource5" 
                                                        SelectCommand="SELECT * FROM AdCategories WHERE ID > 99"
                                                        ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                    </asp:SqlDataSource>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>            </div>
           <%-- <div style="border-bottom: dotted 1px black; clear: both; padding-top: 10px;">
                <asp:Label ID="Label3" runat="server" CssClass="PreferencesTitle">  </asp:Label>
            </div>--%>
            <div  class="Pad">
                <telerik:RadPanelBar CssClass="VenueCheckBoxes" EnableAjaxSkinRendering="false" Width="600px"  AllowCollapseAllItems="true"  runat="server" ID="VenuesRadPanel">
                    <Items>
                        <telerik:RadPanelItem SelectedCssClass="" Height="50px" CssClass="VenueCheckBoxes" Expanded="false">
                            <Items><telerik:RadPanelItem CssClass="VenueCheckBoxes" runat="server"></telerik:RadPanelItem></Items>
                        </telerik:RadPanelItem>
                    </Items>
                    
                </telerik:RadPanelBar>
            </div>
            <div style="border-bottom: dotted 1px black; clear: both; padding-top: 10px;">
                <asp:Label ID="Label4" runat="server" CssClass="PreferencesTitleAlt"> Billing Address <span style="font-style: italic; font-family: Arial; font-size: 14px; color: #666666;">(this is only used for purchased ads on the site)</span></asp:Label>
            </div>
            <div  class="Pad">
                <table>
                    <tr>
                        <td><label>Address</label></td>
                        <td  style="padding-left: 20px;" colspan="2">
                            <label>Country</label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <ctrl:HippoTextBox runat="server" ID="AddressTextBox" TEXTBOX_WIDTH="312" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                            
                        </td>
                        <td  style="padding-left: 20px;" colspan="2">
                            <asp:DropDownList OnSelectedIndexChanged="ChangeBillState" AutoPostBack="true" runat="server" DataSourceID="SqlDataSourceCountry" DataTextField="country_name" DataValueField="country_id" ID="BillCountryDropDown"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="SqlDataSource1" SelectCommand="SELECT * FROM Countries"
                            ConnectionString="<%$ ConnectionStrings:Connection %>"></asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>City</label>
                        </td>
                        <td  style="padding-left: 20px;">
                            <label>ZIP</label>
                        </td>
                        <td>
                            <label>State</label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <ctrl:HippoTextBox runat="server" ID="BillCityTextBox" TEXTBOX_WIDTH="312" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                        </td>
                        <td  style="padding-left: 20px;">
                            <ctrl:HippoTextBox runat="server" ID="ZipTextBox" TEXTBOX_WIDTH="100" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" /><br />
                        </td>
                        <td>
                            <asp:Panel runat="server" ID="BillStateTextPanel">
                                <ctrl:HippoTextBox ID="BillStateTextBox" runat="server" TEXTBOX_WIDTH="100" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                            </asp:Panel>
                            <asp:Panel runat="server" ID="BillStateDropPanel" Visible="false">
                                <asp:DropDownList runat="server" ID="BillStateDropDown"></asp:DropDownList>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
               

            </div>
            <div style="border-bottom: dotted 1px black; clear: both; padding-top: 10px;">
                <asp:Label ID="Label5" runat="server" CssClass="PreferencesTitle"> Phone Number & Provider <span style="font-style: italic; font-family: Arial; font-size: 14px; color: #666666;">(this is only used for texting event info)</span></asp:Label>
            </div>
            <div  class="Pad">
                <table>
                    <tr>
                        <td><label>Phone Number</label></td>
                        <td  style="padding-left: 20px;">
                            <label>Provider</label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <ctrl:HippoTextBox runat="server" ID="PhoneTextBox" TEXTBOX_WIDTH="312" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                            
                        </td>
                        <td  style="padding-left: 20px;">
                            <asp:DropDownList runat="server" ID="ProviderDropDown"></asp:DropDownList>
                        </td>
                    </tr>
                
                </table>
               

            </div>
            <div style="border-bottom: dotted 1px black; clear: both; padding-top: 10px;">
                <asp:Label ID="Label6" runat="server" CssClass="PreferencesTitleAlt"> Texting Preferences <span style="font-style: italic; font-family: Arial; font-size: 14px; color: #666666;">(send notifications when:)</span></asp:Label>
            </div>
            <div  class="Pad">
                <asp:CheckBoxList runat="server" ID="TextingCheckBoxList">
                    <asp:ListItem Value="1">When events in any of your categories have been added.</asp:ListItem>
                    <asp:ListItem Value="2">When one of your friends has added an event.</asp:ListItem>
                    <asp:ListItem Value="3">When an event is at a favorite venue.</asp:ListItem>
                </asp:CheckBoxList>
            </div>
            <div style="border-bottom: dotted 1px black; clear: both; padding-top: 10px;">
                <asp:Label ID="Label10" runat="server" CssClass="PreferencesTitle"> Email Preferences <span style="font-style: italic; font-family: Arial; font-size: 14px; color: #666666;">(send notifications when:)</span></asp:Label>
            </div>

            
            <div  class="Pad">
                                <label>Email</label>
          
               <asp:TextBox runat="server" BackColor="#666666" BorderColor="#1b1b1b"  ID="EmailTextBox" Width="200px" />

                <asp:CheckBoxList runat="server" ID="EmailCheckList">
                    <asp:ListItem Value="1">When events in any of your categories have been added.</asp:ListItem>
                    <asp:ListItem Value="2">When one of your friends has added an event.</asp:ListItem>
                    <asp:ListItem Value="3">When an event is at a favorite venue.</asp:ListItem>
                </asp:CheckBoxList>
            </div>
            <div style="border-bottom: dotted 1px black; clear: both; padding-top: 10px;">
                <asp:Label ID="Label9" runat="server" CssClass="PreferencesTitleAlt"> Comments Preferences <span style="font-style: italic; font-family: Arial; font-size: 14px; color: #666666;">(The comment list for events, venues, etc. can get long. You can decrease it's size by selecting to only view your friends' comments.)</span></asp:Label>
            </div>
            <div style="clear: both;"  class="Pad">
                <div style="float:left;">
                    <asp:RadioButtonList runat="server" ID="CommentsRadioList">
                        <asp:ListItem Value="1">See Everyone's </asp:ListItem>
                        <asp:ListItem Value="2">See Only Friends'</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
            <div style="border-bottom: dotted 1px black; clear: both; padding-top: 10px;">
                <asp:Label ID="Label8" runat="server" CssClass="PreferencesTitle"> Poll Answer Preferences <span style="font-style: italic; font-family: Arial; font-size: 14px; color: #666666;">(Answers to a poll could be a huge list. You can decrease it's size by selecting to only view your friends' answers.)</span></asp:Label>
            </div>
            <div style="clear: both;"  class="Pad">
                <div style="float:left;">
                    <asp:RadioButtonList runat="server" ID="PollRadioList">
                        <asp:ListItem Value="1">See Everyone's </asp:ListItem>
                        <asp:ListItem Value="2">See Only Friends'</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
            
            <div  style="width: 600px; " align="right"><asp:ImageButton runat="server" ID="ImageButton2" ImageUrl="~/image/MakeItSoButton.png" 
                    OnClick="Save" onmouseout="this.src='image/MakeItSoButton.png'" 
                    onmouseover="this.src='image/MakeItSoButtonSelected.png'"  /></div>
        </div>
      
    </div>
 
</asp:Content>

