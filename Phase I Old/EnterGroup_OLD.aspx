<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ValidateRequest="false" 
AutoEventWireup="true" 
CodeFile="EnterGroup_OLD.aspx.cs" 
Inherits="EnterGroup" Title="Enter a Group | HippoHappenings" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register TagName="HippoTextBox" TagPrefix="ctrl" Src="~/Controls/HippoTextBox.ascx" %>
<%@ Register Src="~/Controls/Ads.ascx" TagName="Ads" TagPrefix="ctrl" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<%--<rad:RadAjaxPanel runat="server" >--%>
<%--<asp:UpdatePanel runat="server" >
<Triggers>
    <asp:PostBackTrigger ControlID="ImageButton6" />
    <asp:PostBackTrigger ControlID="ImageButton5" />
</Triggers>--%>
<%--<ContentTemplate>--%>
 <rad:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="false" 
                KeepInScreenBounds="true" VisibleOnPageLoad="false"  Behaviors="none"
            ID="MessageRadWindowManager" ReloadOnShow="true" Modal="true" RestrictionZoneID="RestrictionZone"
            runat="server">
                <Windows>
                    <rad:RadWindow Width="300" ClientCallBackFunction="OnClientClose" EnableEmbeddedSkins="true" 
                    VisibleStatusbar="false" Skin="Black" Height="300" ID="MessageRadWindow" Title="Alert" runat="server">
                    </rad:RadWindow>
                    
                    <rad:RadWindow Width="530" ClientCallBackFunction="FillFriends" EnableEmbeddedSkins="true" 
                    VisibleStatusbar="false" Skin="Black" Height="500" ID="RadWindow1" Title="Alert" runat="server">
                    </rad:RadWindow>
                </Windows>
            </rad:RadWindowManager>
            <script type="text/javascript">
            function OnClientClose(oWnd, args)
            {
                window.location = args;
            }
            function OpenFriends()
                {
                    var win = $find("<%=RadWindow1.ClientID %>");
                    win.setUrl('SelectFriendMembers.aspx');
                    win.show(); 
                    win.center(); 
                 }
                 function OpenMembers()
                {
                    var win = $find("<%=RadWindow1.ClientID %>");
                    win.setUrl('SearchMembers.aspx');
                    win.show(); 
                    win.center(); 
                 }
            function FillFriends(retV, returnValue)
                { 
                   __doPostBack('ctl00$ContentPlaceHolder1$MemberInvitesButton', '');
                }
            </script>
<a id="top"></a>
<div style="padding-bottom: 100px;">
    <asp:Label runat="server" ID="ErrorLabel" ForeColor="Red"></asp:Label>
    <asp:Panel runat="server" ID="LoggedOutPanel">
        <div id="topDiv5">
            <div style="float: left; width: 390px;">
               
                            <span style="font-family: Arial; font-size: 30px; color: #666666;">Enter Group</span>
             
                <br /><br />
                <span style="font-family: Arial; font-size: 12px; color: #cccccc; line-height: 20px;">
            <asp:Label runat="server" ID="WelcomeLabel"></asp:Label>
          </span>
            </div>
            <div style="float: right;">
      
            <ctrl:Ads runat="server" />
            </div>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="BigEventPanel">
    <div style="font-family: Arial; font-size: 30px; color: #666666; padding-bottom: 10px;">
        <asp:Label runat="server" ID="TitleLabel" Text="Enter Group"></asp:Label>
        <asp:Label runat="server" ID="UserNameLabel" Visible="false"></asp:Label>
    </div>
    <div id="topDiv6">
        <div style="float: left; width: 680px;">
        <div style="margin-left: 11px;">
            <rad:RadTabStrip runat="server" ID="EventTabStrip"
            Skin="HippoHappenings" CssClass="EventTabStrip" OnTabClick="TabClick"  Height="40px"
            EnableEmbeddedSkins="false" MultiPageID="BlogEventPages" SelectedIndex="0">
                <Tabs>
                    <rad:RadTab runat="server" PageViewID="TabOne" ImageUrl="images/DetailsPurpleFull.gif" 
                    HoveredImageUrl="images/DetailsPurpleFull.gif" TabIndex="0" 
                    DisabledImageUrl="images/DetailsPurpleFull.gif" 
                    SelectedImageUrl="images/DetailsPurpleFull.gif"  >

                    </rad:RadTab>
                    <rad:RadTab runat="server" Enabled="false"  PageViewID="RadPageView1" 
                    ImageUrl="images/LocationPurpleFull.gif" 
                    HoveredImageUrl="images/LocationPurpleFull.gif"  TabIndex="1"  
                    DisabledImageUrl="images/LocationPurpleEmpty.gif" 
                    SelectedImageUrl="images/LocationPurpleFull.gif" >
                        
                    </rad:RadTab>
                    <rad:RadTab runat="server"  Enabled="false"  PageViewID="RadPageView2" 
                    ImageUrl="images/MembersPurpleFull.gif" 
                    HoveredImageUrl="images/MembersPurpleFull.gif"  TabIndex="2" 
                    DisabledImageUrl="images/MembersPurpleEmpty.gif" 
                    SelectedImageUrl="images/MembersPurpleFull.gif" ></rad:RadTab>
                    <rad:RadTab runat="server"  Enabled="false"  PageViewID="RadPageView5" 
                    ImageUrl="images/MediaPurpleFull.gif" 
                    HoveredImageUrl="images/MediaPurpleFull.gif"  TabIndex="3" 
                    DisabledImageUrl="images/MediaPurpleEmpty.gif" 
                    SelectedImageUrl="images/MediaPurpleFull.gif" ></rad:RadTab>
                    <rad:RadTab runat="server"  Enabled="false"  PageViewID="RadPageView3" 
                    ImageUrl="images/CategoriesPurpleFull.gif" 
                    HoveredImageUrl="images/CategoriesPurpleFull.gif"  TabIndex="4" 
                    DisabledImageUrl="images/CategoriesPurpleEmpty.gif" 
                    SelectedImageUrl="images/CategoriesPurpleFull.gif" ></rad:RadTab>
                    <rad:RadTab runat="server"  Enabled="false"  PageViewID="RadPageView4" 
                    ImageUrl="images/PostItPurpleFull.gif" 
                    HoveredImageUrl="images/PostItPurpleFull.gif" 
                    DisabledImageUrl="images/PostItPurpleEmpty.gif"  TabIndex="5" 
                    SelectedImageUrl="images/PostItPurpleFull.gif" ></rad:RadTab>
                </Tabs>
            </rad:RadTabStrip>
        </div>
        <rad:RadMultiPage runat="server" ID="BlogEventPages" SelectedIndex="0">
            <rad:RadPageView runat="server" ID="TabOne" TabIndex="0" >
                
                <asp:Panel runat="server" ID="Panel1" DefaultButton="DetailsOnwardsButton">
                    <div style="background: url('image/TabsBackgroundTop.png'); background-repeat: no-repeat; width: 668px; height: 4px;">
                    </div>
                    <div id="topDiv" style="background: url('image/TabsBackground.png'); background-repeat: repeat-y; width: 668px; padding-top: 10px;">
                        <div class="EventDiv" style="padding-left: 30px;float: left;">
                        
                            <table>
                                <tr>
                                    <td width="250px;" valign="top">
                                    <h1>Details</h1>
                                    <label>All HTML tags will be removed except for links. However, the header cannot contain links at all.</label>
                                            <br /><br />
                                            <script type="text/javascript">
                                                    
                                                function CountCharsHeadline(editor, e){
                                                    
                                                        var theDiv = document.getElementById("CharsDivHeadline");
                                                        var theText = document.getElementById("ctl00_ContentPlaceHolder1_VenueNameTextBox_TheTextBox");
                                                        theDiv.innerHTML = "characters left: "+ (70 - theText.value.length).toString();
                                                    
                                                }
                                            </script>
                                        <label><span style='color: #1fb6e7; font-weight: bold;'>* </span>Name</label><br />
                                        <span style="font-family: Arial; font-size: 10px; color: #ff770d; font-weight: bold;"> 
                                        max 70 characters  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    &nbsp;&nbsp;<span style="color: Red;" id="CharsDivHeadline"></span></span><br />
                                        <ctrl:HippoTextBox runat="server" ON_CLIENT_KEYPRESS="CountCharsHeadline(event)"  ID="VenueNameTextBox" TEXTBOX_WIDTH="200" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                                       <br /><br /><br />
                                       
                                       <label>Main Group Image</label><br />
                                       <span style="font-family: Arial; font-size: 10px; color: #cccccc; font-weight: bold;"> [resized to 200x200]</span>
                                       <asp:Panel runat="server" ID="Panel6">
                                                            <div id="Div2">
                                                                <div style="float: left; padding-top: 4px;">
                                                                    <asp:FileUpload runat="server" ID="MainImageUpload" Width="230px" EnableViewState="true" />
                                                                </div>
                                                                <div style="float: left;">
                                                                <asp:Button runat="server" ID="Button1" Text="Upload!" 
                                                                     CssClass="SearchButton" 
                                                                     OnClick="MainPicUpload"
                                                                     onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                                     onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                                     onclientclick="this.value = 'Working...';" 
                                                                     />
                                                                <%--<asp:ImageButton runat="server" ID="ImageButton6" OnClick="VideoUpload_Click"
                                                                     ImageUrl="image/MakeItSoButton.png" 
                                                                    onmouseout="this.src='image/MakeItSoButton.png'" 
                                                                    onmouseover="this.src='image/MakeItSoButtonSelected.png'"  />--%>
                                                                </div>

                                                            </div>
                                                            

                                                        </asp:Panel>
                                       </td>
                                     <td style="padding-left: 50px;">
                           <label><span style='color: #1fb6e7; font-weight: bold;'>* </span>Description</label><br /><span style="font-family: Arial; font-size: 10px; color: #ff770d; font-weight: bold;"> max 420 characters
                                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                  <span style="color: Red;" id="CharsDivEditor"></span>
                                </span><br />
                                
                                                            
                                                    <rad:RadEditor EditModes="Design" OnClientLoad="OnClientLoad" 
                                                    Skin="Black" runat="server" 
                                                    ID="DescriptionTextBox" Height="200px" Width="240px"
                                                        EnableResize="False"  ToolsFile="toolsfile.xml" 
                                                        StripFormattingOptions="MSWordRemoveAll" 
                                                        StripFormattingOnPaste="MSWordRemoveAll">
                                                        <CssFiles>
                                                        <rad:EditorCssFile Value="contentarea.css" />
                                                       </CssFiles>
                                                        <Tools>
                                                           <rad:EditorToolGroup>
                                                               <rad:EditorTool Name="InsertLink" Text="Insert Link" />
                                                           </rad:EditorToolGroup>
                                                       </Tools>
                                                    </rad:RadEditor>
                                                    <script type="text/javascript">
                                                        Telerik.Web.UI.Editor.CommandList["InsertLink"] = function(commandName, editor, args)
                                                        {
                                                          var elem = editor.getSelectedElement(); //returns the selected element.
                                                                
                                                          if (elem.tagName == "A")
                                                          {
                                                               editor.selectElement(elem);
                                                               argument = elem;
                                                          }
                                                          else
                                                          {
                                                             var content = editor.getSelectionHtml();
                                                             var link = editor.get_document().createElement("A");
                                                             link.innerHTML = content;         
                                                             argument = link;
                                                          }

                                                         
                                                          var myCallbackFunction = function(sender, args)
                                                          {
                                                               
                                                                editor.pasteHtml(String.format("<a target='_blank' class='AddLink' href='{1}'>{0}</a>", args.name, args.url))
                                                          }
                                                          editor.showExternalDialog(
                                                               'InsertLink.html',
                                                               argument,
                                                               270,
                                                               300,
                                                               myCallbackFunction,
                                                               null,
                                                               'Insert Link',
                                                               true,
                                                               Telerik.Web.UI.WindowBehaviors.Close + Telerik.Web.UI.WindowBehaviors.Move,
                                                               false,
                                                               false);
                                                        };
                                                    </script>
                            <script type="text/javascript">
                                                            
                                                                function OnClientLoad(editor)
                                                                {
                                                                    editor.attachEventHandler("onkeyup", CountCharsEditor);
                                                                }  
                                                                function CountCharsEditor(editor)
                                                                {                                                                    
                                                                    var editor = (editor) ? editor : ((event) ? event : null);
                                                                    var node = (editor.target) ? editor.target : ((editor.srcElement) ? editor.srcElement : null);
                                                                    
                                                                    var theEditorBig = document.getElementById("ctl00_ContentPlaceHolder1_DescriptionTextBox_contentIframe");
                                                                    var theDiv = document.getElementById("CharsDivEditor");
                                                                    theDiv.innerHTML = navigator.appCodeName;
                                                                    if(navigator.appName == 'Netscape')
                                                                    {
                                                                        if(this.activeElement.textContent.length > 420)
                                                                            theDiv.innerHTML = "characters left: 0";
                                                                        else
                                                                            theDiv.innerHTML = "characters left: "+ (420 - this.activeElement.textContent.length).toString();
                                                                    }
                                                                    else
                                                                    {
                                                                        var theEditor = $find("<%=DescriptionTextBox.ClientID %>");
                                                                        if(theEditor.get_contentArea().innerText.length > 420)
                                                                            theDiv.innerHTML = "characters left: 0";
                                                                        else
                                                                            theDiv.innerHTML = "characters left: "+ (420 - theEditor.get_contentArea().innerText.length).toString();
                                                                    }
                                                                }
                                                            </script>
   
   <%--                               <label><span style='color: #ff770d; font-weight: bold;'><span style='color: #1fb6e7; font-weight: bold;'>* </span>reqiured fields</span></label>   
                                        <label style="padding-right: 10px;">Location</label><br />
                                        <asp:TextBox runat="server" ID="SpecificLocationTextBox"></asp:TextBox> <label style="font-family: Arial; font-style:italic;font-size: 12px; color: #666666;">[ex: Multnomah, Downtown, sw, etc. This will help your users find your venue faster]</label>
    --%>                                
    
    </td>
    
                                    
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <br />
                                                            <div style="clear: both;">
                                                                    <asp:CheckBoxList CssClass="MusicCheckBoxes" ID="MainImageCheck" runat="server"></asp:CheckBoxList>
                                                                    <asp:ImageButton Visible="false" runat="server" OnClick="MainImageNixIt" ID="ImageButton1" ImageUrl="image/NixItButton.png" 
                                                                    onmouseout="this.src='image/NixItButton.png'" onmouseover="this.src='image/NixItButtonSelected.png'"  />
                                                            </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <label><span style='color: #1fb6e7; font-weight: bold;'>* </span>Choose Two Base Colors For Your Group Page</label>
                                        <asp:RadioButtonList runat="server" ID="ColorSchemeRadioList" AutoPostBack="true" OnSelectedIndexChanged="ChangeColorPanel">
                                            <asp:ListItem Selected="true" Text="Default" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Custom" Value="1"></asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:Panel runat="server" ID="ColorPanel" Visible="false">
                                            <rad:RadColorPicker runat="server" ID="RadColorPicker1" 
                                            PaletteModes="All"  CssClass="ColorPickerPreview">
                                            </rad:RadColorPicker>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                
                            </table>
                        </div>
                        <div align="right" style="vertical-align: bottom; width: 600px;">
                                <asp:ImageButton runat="server" ID="DetailsOnwardsButton" OnClick="Onwards" ImageUrl="image/OnwardsButton.png" 
                                onmouseout="this.src='image/OnwardsButton.png'" onmouseover="this.src='image/OnwardsButtonSelected.png'"  />
                             </div>
                    </div>
                    <div align="center" style=" margin-left: 11px ;background-color: #515151 ;height: 4px; width: 640px;"></div>
                </asp:Panel>
            </rad:RadPageView>
            <rad:RadPageView runat="server" ID="RadPageView1" TabIndex="1" >
                <asp:Panel runat="server" ID="Panel2" DefaultButton="DescriptionOnwardsButton">
                    <div style="background: url('image/TabsBackgroundTop.png'); background-repeat: no-repeat; width: 668px; height: 4px;">
                    </div>
                    <div  class="EventDiv" style="background: url('image/TabsBackground.png'); background-repeat: repeat-y; width: 668px;">
                        <div style="padding-top: 10px; padding-bottom: 20px; width: 650px;">
                            
                                    <div style="padding-left: 25px;">
                                    <h1>Location</h1>
                                        <table cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td valign="top" style="padding-bottom: 15px;">
                                                <h1 class="Blue"><span style='color: #1fb6e7; font-weight: bold;'>* </span>Group Head-Quarters</h1>
                                                
                                                     <table cellpadding="0" cellspacing="5">
                                            <tr>
                                                <td colspan="2">
                                                    <label style="padding-right: 10px;"><span style='color: #1fb6e7; font-weight: bold;'>* </span>Country</label><br />
                                                    <asp:DropDownList OnSelectedIndexChanged="ChangeState" AutoPostBack="true" runat="server" DataSourceID="SqlDataSourceCountry" DataTextField="country_name" DataValueField="country_id" ID="CountryDropDown"></asp:DropDownList>
                                                    <asp:SqlDataSource runat="server" ID="SqlDataSourceCountry" SelectCommand="SELECT * FROM Countries"
                                                    ConnectionString="<%$ ConnectionStrings:Connection %>"></asp:SqlDataSource>
                                                
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <label><span style='color: #1fb6e7; font-weight: bold;'>* </span>State</label><br />
                                                            </td>
                                                            <td>
                                                                <label><span style='color: #1fb6e7; font-weight: bold;'>* </span>City</label>
                                                            </td>
                                                            <td style="padding-left: 3px;">
                                                                <label>Zip</label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Panel runat="server" ID="StateTextBoxPanel">
                                                                    <ctrl:HippoTextBox ID="StateTextBox" runat="server" TEXTBOX_WIDTH="100" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                                                                </asp:Panel>
                                                                <asp:Panel runat="server" ID="StateDropDownPanel" Visible="false">
                                                                    <asp:DropDownList runat="server" ID="StateDropDown"></asp:DropDownList>
                                                                </asp:Panel>
                                                            </td>
                                                            <td style="padding-left: 3px;">
                                                                <asp:TextBox ID="CityTextBox" runat="server" WIDTH="100"></asp:TextBox>
                                                            </td>
                                                            <td style="padding-left: 3px;">
                                                                <asp:TextBox ID="ZipTextBox" runat="server" WIDTH="100" ></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            </table>
                                                </td>
                                                
                                                <td style="padding-left: 20px; padding-bottom: 15px;">
                                                <div style="float: left;"><h1 class="Green">Group's Contact Info </h1></div> 
                                                    <div style="clear: both;">
                                                        <label>Phone</label><br /><asp:TextBox runat="server" ID="PhoneTextBox"></asp:TextBox><br />
                                                        <label>Email</label><br /><asp:TextBox runat="server" ID="EmailTextBox"></asp:TextBox><br />
                                                        <label>Web Site</label><br /><asp:TextBox runat="server" ID="WebSiteTextBox"></asp:TextBox><br />
                                                    </div>
                                                </td>
                                            </tr>
                                       
                                            <tr>
                                                <td valign="top">
                                                <div style="float: left;"><h1 class="Green">Group's Specific Address</h1></div>
                                                     <table>
                                            <tr>
                                                <asp:Panel runat="server" ID="USPanel">
                                                <td colspan="2">
                                        
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <label>Street No.</label>
                                                            </td>
                                                            <td style="padding-left: 3px;">
                                                                <label>Street Name</label>
                                                            </td>
                                                            <td>
                                                               
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="StreetNumberTextBox" Width="70px"></asp:TextBox>
                                                            </td>
                                                            <td style="padding-left: 3px;">
                                                                <asp:TextBox runat="server" ID="StreetNameTextBox" Width="100px"></asp:TextBox>
                                                            </td>
                                                            <td style="padding-left: 3px;">
                                                                <asp:DropDownList runat="server" ID="StreetDropDown" Width="100px">
                                                                    <asp:ListItem Text="Select One..."></asp:ListItem>
                                                                    <asp:ListItem Text="Alley"></asp:ListItem>
                                                                    <asp:ListItem Text="Annex"></asp:ListItem>
                                                                    <asp:ListItem Text="Arcade"></asp:ListItem>
                                                                    <asp:ListItem Text="Avenue"></asp:ListItem>
                                                                    <asp:ListItem Text="Bayoo"></asp:ListItem>
                                                                    <asp:ListItem Text="Beach"></asp:ListItem>
                                                                    <asp:ListItem Text="Bend"></asp:ListItem>
                                                                    <asp:ListItem Text="Bluff"></asp:ListItem>
                                                                    <asp:ListItem Text="Bluffs"></asp:ListItem>
                                                                    <asp:ListItem Text="Bottom"></asp:ListItem>
                                                                    <asp:ListItem Text="Boulevard"></asp:ListItem>
                                                                    <asp:ListItem Text="Branch"></asp:ListItem>
                                                                    <asp:ListItem Text="Bridge"></asp:ListItem>
                                                                    <asp:ListItem Text="Brook"></asp:ListItem>
                                                                    <asp:ListItem Text="Brooks"></asp:ListItem>
                                                                    <asp:ListItem Text="Burg"></asp:ListItem>
                                                                    <asp:ListItem Text="Burgs"></asp:ListItem>
                                                                    <asp:ListItem Text="Bypass"></asp:ListItem>
                                                                    <asp:ListItem Text="Camp"></asp:ListItem>
                                                                    <asp:ListItem Text="Canyon"></asp:ListItem>
                                                                    <asp:ListItem Text="Cape"></asp:ListItem>
                                                                    <asp:ListItem Text="Causeway"></asp:ListItem>
                                                                    <asp:ListItem Text="Center"></asp:ListItem>
                                                                    <asp:ListItem Text="Centers"></asp:ListItem>
                                                                    <asp:ListItem Text="Circle"></asp:ListItem>
                                                                    <asp:ListItem Text="Circles"></asp:ListItem>
                                                                    <asp:ListItem Text="Cliff"></asp:ListItem>
                                                                    <asp:ListItem Text="Cliffs"></asp:ListItem>
                                                                    <asp:ListItem Text="Club"></asp:ListItem>
                                                                    <asp:ListItem Text="Common"></asp:ListItem>
                                                                    <asp:ListItem Text="Corner"></asp:ListItem>
                                                                    <asp:ListItem Text="Corners"></asp:ListItem>
                                                                    <asp:ListItem Text="Course"></asp:ListItem>
                                                                    <asp:ListItem Text="Court"></asp:ListItem>
                                                                    <asp:ListItem Text="Courts"></asp:ListItem>
                                                                    <asp:ListItem Text="Cove"></asp:ListItem>
                                                                    <asp:ListItem Text="Coves"></asp:ListItem>
                                                                    <asp:ListItem Text="Creek"></asp:ListItem>
                                                                    <asp:ListItem Text="Crescent"></asp:ListItem>
                                                                    <asp:ListItem Text="Crest"></asp:ListItem>
                                                                    <asp:ListItem Text="Crossing"></asp:ListItem>
                                                                    <asp:ListItem Text="Crossroad"></asp:ListItem>
                                                                    <asp:ListItem Text="Curve"></asp:ListItem>
                                                                    <asp:ListItem Text="Dale"></asp:ListItem>
                                                                    <asp:ListItem Text="Dam"></asp:ListItem>
                                                                    <asp:ListItem Text="Divide"></asp:ListItem>
                                                                    <asp:ListItem Text="Drive"></asp:ListItem>
                                                                    <asp:ListItem Text="Drives"></asp:ListItem>
                                                                    <asp:ListItem Text="Estate"></asp:ListItem>
                                                                    <asp:ListItem Text="Estates"></asp:ListItem>
                                                                    <asp:ListItem Text="Expressway"></asp:ListItem>
                                                                    <asp:ListItem Text="Extension"></asp:ListItem>
                                                                    <asp:ListItem Text="Extensions"></asp:ListItem>
                                                                    <asp:ListItem Text="Fall"></asp:ListItem>
                                                                    <asp:ListItem Text="Falls"></asp:ListItem>
                                                                    <asp:ListItem Text="Ferry"></asp:ListItem>
                                                                    <asp:ListItem Text="Field"></asp:ListItem>
                                                                    <asp:ListItem Text="Fields"></asp:ListItem>
                                                                    <asp:ListItem Text="Flat"></asp:ListItem>
                                                                    <asp:ListItem Text="Flats"></asp:ListItem>
                                                                    <asp:ListItem Text="Ford"></asp:ListItem>
                                                                    <asp:ListItem Text="Fords"></asp:ListItem>
                                                                    <asp:ListItem Text="Forest"></asp:ListItem>
                                                                    <asp:ListItem Text="Forge"></asp:ListItem>
                                                                    <asp:ListItem Text="Forges"></asp:ListItem>
                                                                    <asp:ListItem Text="Fork"></asp:ListItem>
                                                                    <asp:ListItem Text="Forks"></asp:ListItem>
                                                                    <asp:ListItem Text="Fort"></asp:ListItem>
                                                                    <asp:ListItem Text="Freeway"></asp:ListItem>
                                                                    <asp:ListItem Text="Garden"></asp:ListItem>
                                                                    <asp:ListItem Text="Gardens"></asp:ListItem>
                                                                    <asp:ListItem Text="Gateway"></asp:ListItem>
                                                                    <asp:ListItem Text="Glen"></asp:ListItem>
                                                                    <asp:ListItem Text="Glens"></asp:ListItem>
                                                                    <asp:ListItem Text="Green"></asp:ListItem>
                                                                    <asp:ListItem Text="Greens"></asp:ListItem>
                                                                    <asp:ListItem Text="Grove"></asp:ListItem>
                                                                    <asp:ListItem Text="Harbor"></asp:ListItem>
                                                                    <asp:ListItem Text="Harbors"></asp:ListItem>
                                                                    <asp:ListItem Text="Haven"></asp:ListItem>
                                                                    <asp:ListItem Text="Heights"></asp:ListItem>
                                                                    <asp:ListItem Text="Highway"></asp:ListItem>
                                                                    <asp:ListItem Text="Hill"></asp:ListItem>
                                                                    <asp:ListItem Text="Hills"></asp:ListItem>
                                                                    <asp:ListItem Text="Hollow"></asp:ListItem>
                                                                    <asp:ListItem Text="Inlet"></asp:ListItem>
                                                                    <asp:ListItem Text="Island"></asp:ListItem>
                                                                    <asp:ListItem Text="Islands"></asp:ListItem>
                                                                    <asp:ListItem Text="Isle"></asp:ListItem>
                                                                    <asp:ListItem Text="Junction"></asp:ListItem>
                                                                    <asp:ListItem Text="Junctions"></asp:ListItem>
                                                                    <asp:ListItem Text="Key"></asp:ListItem>
                                                                    <asp:ListItem Text="Keys"></asp:ListItem>
                                                                    <asp:ListItem Text="Knoll"></asp:ListItem>
                                                                    <asp:ListItem Text="Knolls"></asp:ListItem>
                                                                    <asp:ListItem Text="Lake"></asp:ListItem>
                                                                    <asp:ListItem Text="Lakes"></asp:ListItem>
                                                                    <asp:ListItem Text="Land"></asp:ListItem>
                                                                    <asp:ListItem Text="Landing"></asp:ListItem>
                                                                    <asp:ListItem Text="Lane"></asp:ListItem>
                                                                    <asp:ListItem Text="Light"></asp:ListItem>
                                                                    <asp:ListItem Text="Lights"></asp:ListItem>
                                                                    <asp:ListItem Text="Loaf"></asp:ListItem>
                                                                    <asp:ListItem Text="Lock"></asp:ListItem>
                                                                    <asp:ListItem Text="Locks"></asp:ListItem>
                                                                    <asp:ListItem Text="Lodge"></asp:ListItem>
                                                                    <asp:ListItem Text="Loop"></asp:ListItem>
                                                                    <asp:ListItem Text="Mall"></asp:ListItem>
                                                                    <asp:ListItem Text="Manor"></asp:ListItem>
                                                                    <asp:ListItem Text="Manors"></asp:ListItem>
                                                                    <asp:ListItem Text="Meadow"></asp:ListItem>
                                                                    <asp:ListItem Text="Meadows"></asp:ListItem>
                                                                    <asp:ListItem Text="Mews"></asp:ListItem>
                                                                    <asp:ListItem Text="Mill"></asp:ListItem>
                                                                    <asp:ListItem Text="Mills"></asp:ListItem>
                                                                    <asp:ListItem Text="Mission"></asp:ListItem>
                                                                    <asp:ListItem Text="Motorway"></asp:ListItem>
                                                                    <asp:ListItem Text="Mount"></asp:ListItem>
                                                                    <asp:ListItem Text="Mountain"></asp:ListItem>
                                                                    <asp:ListItem Text="Mountains"></asp:ListItem>
                                                                    <asp:ListItem Text="Neck"></asp:ListItem>
                                                                    <asp:ListItem Text="Orchard"></asp:ListItem>
                                                                    <asp:ListItem Text="Oval"></asp:ListItem>
                                                                    <asp:ListItem Text="Overpass"></asp:ListItem>
                                                                    <asp:ListItem Text="Park"></asp:ListItem>
                                                                    <asp:ListItem Text="Parks"></asp:ListItem>
                                                                    <asp:ListItem Text="Parkway"></asp:ListItem>
                                                                    <asp:ListItem Text="Parkways"></asp:ListItem>
                                                                    <asp:ListItem Text="Pass"></asp:ListItem>
                                                                    <asp:ListItem Text="Passage"></asp:ListItem>
                                                                    <asp:ListItem Text="Path"></asp:ListItem>
                                                                    <asp:ListItem Text="Pike"></asp:ListItem>
                                                                    <asp:ListItem Text="Pine"></asp:ListItem>
                                                                    <asp:ListItem Text="Pines"></asp:ListItem>
                                                                    <asp:ListItem Text="Place"></asp:ListItem>
                                                                    <asp:ListItem Text="Plain"></asp:ListItem>
                                                                    <asp:ListItem Text="Plains"></asp:ListItem>
                                                                    <asp:ListItem Text="Plaza"></asp:ListItem>
                                                                    <asp:ListItem Text="Point"></asp:ListItem>
                                                                    <asp:ListItem Text="Points"></asp:ListItem>
                                                                    <asp:ListItem Text="Port"></asp:ListItem>
                                                                    <asp:ListItem Text="Ports"></asp:ListItem>
                                                                    <asp:ListItem Text="Prairie"></asp:ListItem>
                                                                    <asp:ListItem Text="Radial"></asp:ListItem>
                                                                    <asp:ListItem Text="Ramp"></asp:ListItem>
                                                                    <asp:ListItem Text="Ranch"></asp:ListItem>
                                                                    <asp:ListItem Text="Rapid"></asp:ListItem>
                                                                    <asp:ListItem Text="Rapids"></asp:ListItem>
                                                                    <asp:ListItem Text="Rest"></asp:ListItem>
                                                                    <asp:ListItem Text="Ridge"></asp:ListItem>
                                                                    <asp:ListItem Text="Ridges"></asp:ListItem>
                                                                    <asp:ListItem Text="River"></asp:ListItem>
                                                                    <asp:ListItem Text="Road"></asp:ListItem>
                                                                    <asp:ListItem Text="Roads"></asp:ListItem>
                                                                    <asp:ListItem Text="Route"></asp:ListItem>
                                                                    <asp:ListItem Text="Row"></asp:ListItem>
                                                                    <asp:ListItem Text="Rue"></asp:ListItem>
                                                                    <asp:ListItem Text="Run"></asp:ListItem>
                                                                    <asp:ListItem Text="Shoal"></asp:ListItem>
                                                                    <asp:ListItem Text="Shoals"></asp:ListItem>
                                                                    <asp:ListItem Text="Shore"></asp:ListItem>
                                                                    <asp:ListItem Text="Shores"></asp:ListItem>
                                                                    <asp:ListItem Text="Skyway"></asp:ListItem>
                                                                    <asp:ListItem Text="Spring"></asp:ListItem>
                                                                    <asp:ListItem Text="Springs"></asp:ListItem>
                                                                    <asp:ListItem Text="Spur"></asp:ListItem>
                                                                    <asp:ListItem Text="Spurs"></asp:ListItem>
                                                                    <asp:ListItem Text="Square"></asp:ListItem>
                                                                    <asp:ListItem Text="Squares"></asp:ListItem>
                                                                    <asp:ListItem Text="Station"></asp:ListItem>
                                                                    <asp:ListItem Text="Stravenue"></asp:ListItem>
                                                                    <asp:ListItem Text="Stream"></asp:ListItem>
                                                                    <asp:ListItem Text="Street"></asp:ListItem>
                                                                    <asp:ListItem Text="Streets"></asp:ListItem>
                                                                    <asp:ListItem Text="Summit"></asp:ListItem>
                                                                    <asp:ListItem Text="Terrace"></asp:ListItem>
                                                                    <asp:ListItem Text="Throughway"></asp:ListItem>
                                                                    <asp:ListItem Text="Trace"></asp:ListItem>
                                                                    <asp:ListItem Text="Track"></asp:ListItem>
                                                                    <asp:ListItem Text="Trafficway"></asp:ListItem>
                                                                    <asp:ListItem Text="Trail"></asp:ListItem>
                                                                    <asp:ListItem Text="Tunnel"></asp:ListItem>
                                                                    <asp:ListItem Text="Turnpike"></asp:ListItem>
                                                                    <asp:ListItem Text="Underpass"></asp:ListItem>
                                                                    <asp:ListItem Text="Union"></asp:ListItem>
                                                                    <asp:ListItem Text="Valley"></asp:ListItem>
                                                                    <asp:ListItem Text="Valleys"></asp:ListItem>
                                                                    <asp:ListItem Text="Viaduct"></asp:ListItem>
                                                                    <asp:ListItem Text="View"></asp:ListItem>
                                                                    <asp:ListItem Text="Views"></asp:ListItem>
                                                                    <asp:ListItem Text="Village"></asp:ListItem>
                                                                    <asp:ListItem Text="Villages"></asp:ListItem>
                                                                    <asp:ListItem Text="Ville"></asp:ListItem>
                                                                    <asp:ListItem Text="Vista"></asp:ListItem>
                                                                    <asp:ListItem Text="Walk"></asp:ListItem>
                                                                    <asp:ListItem Text="Walks"></asp:ListItem>
                                                                    <asp:ListItem Text="Wall"></asp:ListItem>
                                                                    <asp:ListItem Text="Way"></asp:ListItem>
                                                                    <asp:ListItem Text="Ways"></asp:ListItem>
                                                                    <asp:ListItem Text="Well"></asp:ListItem>
                                                                    <asp:ListItem Text="Wells"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                </asp:Panel>
                                                <asp:Panel runat="server" ID="InternationalPanel" Visible="false">
                                                <td colspan="2">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                               <label><span style='color: #1fb6e7; font-weight: bold;'>* </span>Location</label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="LocationTextBox" Width="200px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                </asp:Panel>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="padding-top: 3px;">
                                                    <asp:DropDownList runat="server" ID="AptDropDown">
                                                        <asp:ListItem Text="Suite" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Apt" Value="2"></asp:ListItem>
                                                    </asp:DropDownList>
                                               
                                                    <asp:TextBox runat="server" ID="AptNumberTextBox" Width="50px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            
                                        </table>
                                                </td>
                                                <td style="padding-left: 20px;">
                                                <div style="float: left;"><h1 class="Blue">Host's Location Instructions</h1></div>
                                        <table>
                                            <tr>
                                                <td><span style="font-family: Arial; font-size: 10px; color: #ff770d; font-weight: bold;"> max 300 characters
                                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                  <span style="color: Red;" id="CharsDivEditor2"></span>
                                </span><br />
                                <script type="text/javascript">
                                                            
                                                                function OnClientLoad2(editor)
                                                                {
                                                                    editor.attachEventHandler("onkeyup", CountCharsEditor2);
                                                                }  
                                                                function CountCharsEditor2(editor)
                                                                {                                                                    
                                                                    var editor = (editor) ? editor : ((event) ? event : null);
                                                                    var node = (editor.target) ? editor.target : ((editor.srcElement) ? editor.srcElement : null);
                                                                    
                                                                    var theEditorBig = document.getElementById("ctl00_ContentPlaceHolder1_HostInstructions_contentIframe");
                                                                    var theDiv = document.getElementById("CharsDivEditor2");
                                                                    theDiv.innerHTML = navigator.appCodeName;
                                                                    if(navigator.appName == 'Netscape')
                                                                    {
                                                                        if(this.activeElement.textContent.length > 300)
                                                                            theDiv.innerHTML = "characters left: 0";
                                                                        else
                                                                            theDiv.innerHTML = "characters left: "+ (300 - this.activeElement.textContent.length).toString();
                                                                    }
                                                                    else
                                                                    {
                                                                        var theEditor = $find("<%=HostInstructions.ClientID %>");
                                                                        if(theEditor.get_contentArea().innerText.length > 300)
                                                                            theDiv.innerHTML = "characters left: 0";
                                                                        else
                                                                            theDiv.innerHTML = "characters left: "+ (300 - theEditor.get_contentArea().innerText.length).toString();
                                                                    }
                                                                }
                                                            </script>
                                                            
                                                    <rad:RadEditor EditModes="Design" OnClientLoad="OnClientLoad2" 
                                                    Skin="Black" runat="server" 
                                                    ID="HostInstructions" Height="160px" Width="240px"
                                                        EnableResize="False"  ToolsFile="toolsfile.xml" 
                                                        StripFormattingOptions="MSWordRemoveAll" 
                                                        StripFormattingOnPaste="MSWordRemoveAll">
                                                        <CssFiles>
                                                        <rad:EditorCssFile Value="contentarea.css" />
                                                       </CssFiles>
                                                        <Tools>
                                                           <rad:EditorToolGroup>
                                                               <rad:EditorTool Name="InsertLink" Text="Insert Link" />
                                                           </rad:EditorToolGroup>
                                                       </Tools>
                                                    </rad:RadEditor>
                                                    <script type="text/javascript">
                                                        Telerik.Web.UI.Editor.CommandList["InsertLink"] = function(commandName, editor, args)
                                                        {
                                                          var elem = editor.getSelectedElement(); //returns the selected element.
                                                                
                                                          if (elem.tagName == "A")
                                                          {
                                                               editor.selectElement(elem);
                                                               argument = elem;
                                                          }
                                                          else
                                                          {
                                                             var content = editor.getSelectionHtml();
                                                             var link = editor.get_document().createElement("A");
                                                             link.innerHTML = content;         
                                                             argument = link;
                                                          }

                                                         
                                                          var myCallbackFunction = function(sender, args)
                                                          {
                                                              editor.pasteHtml(String.format("<a target='_blank' class='AddLink' href='{1}'>{0}</a>", args.name, args.url))
                                                          }
                                                          editor.showExternalDialog(
                                                               'InsertLink.html',
                                                               argument,
                                                               270,
                                                               300,
                                                               myCallbackFunction,
                                                               null,
                                                               'Insert Link',
                                                               true,
                                                               Telerik.Web.UI.WindowBehaviors.Close + Telerik.Web.UI.WindowBehaviors.Move,
                                                               false,
                                                               false);
                                                        };
                                                    </script>
                                                </td>
                                            </tr>
                                        </table>
                                                </td>
                                            </tr>
                                        </table>
                                        
                                        </div>
                                 
                            
                        </div>
                        <div align="right" style=" padding-top: 30px; padding-right: 50px;">
                            <asp:ImageButton runat="server" ID="ImageButton2" OnClick="Backwards" ImageUrl="image/BackwardsButton.png" 
                            onmouseout="this.src='image/BackwardsButton.png'" onmouseover="this.src='image/BackwardsButtonSelected.png'"  />
                            <asp:ImageButton runat="server" ID="DescriptionOnwardsButton" OnClick="Onwards" ImageUrl="image/OnwardsButton.png" 
                            onmouseout="this.src='image/OnwardsButton.png'" onmouseover="this.src='image/OnwardsButtonSelected.png'"  />
                         </div>
                    </div>
                    <div align="center" style=" margin-left: 11px ;background-color: #515151 ;height: 4px; width: 640px;"></div>
                </asp:Panel>
            </rad:RadPageView>

            <rad:RadPageView runat="server" ID="RadPageView2" TabIndex="2" >
                <asp:Panel runat="server" ID="Panel3" DefaultButton="MediaOnwardsButton">
                    <div style="background: url('image/TabsBackgroundTop.png'); background-repeat: no-repeat; width: 668px; height: 4px;">
                    </div>
                    <div style="background: url('image/TabsBackground.png'); background-repeat: repeat-y;width: 668px;">
                        <a href="#top"></a>
                        <div class="EventDiv">
                           <div style="padding-left: 30px; width: 622px; clear: both; padding-top: 10px;" class="topDiv">
                               <div style="float: left;"><h1><span style='color: #1fb6e7; font-weight: bold;'>* </span>Invite Members</h1></div>
                               <div style="float: left; padding-left: 10px; padding-top: 5px;" ><asp:Image CssClass="HelpImage" runat="server"  ID="Image2" ImageUrl="~/image/helpIcon.png"></asp:Image>
                                                <rad:RadToolTip ID="RadToolTip2" runat="server" Skin="Black" Width="200px" Height="200px" 
                                                ManualClose="true" ShowEvent="onclick" Position="MiddleRight" RelativeTo="Element" 
                                                TargetControlID="Image2">
                                    <label>Once you create this group, a message will be sent to the users you list here asking them to accept this membership.
                                    If you do not want to ask these users for membership now, you can always assign members later on by editing the group page. If you are editing the group, your group members that you already sent
                                    an invitation to will not show here. However, if you select these members again, another invite will be sent to them once you update the group.</label>
                                    </rad:RadToolTip></div>
                               <div style="padding-left: 10px; clear: both;">
                                    <label onclick="OpenMembers()"><span style='color: #ff770d; font-weight: bold; cursor: pointer;'>search members </span><span style="color: White;">|</span></label>  <label onclick="OpenFriends()"><span style='color: #ff770d; font-weight: bold; cursor: pointer;'>invite from friends</span></label>
                                     <asp:UpdatePanel runat="server" UpdateMode="conditional">
                                        <ContentTemplate>
                                     <table>
                                        <tr>
                                            <td valign="top">
                                               <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
                                                    <ContentTemplate>--%>
                                                        <asp:LinkButton runat="server" ID="MemberInvitesButton" OnClick="FillMemberInvites"></asp:LinkButton>
                                                    
                                                        <asp:ListBox AutoPostBack="true" OnSelectedIndexChanged="SelectTitle" runat="server" Width="300px" Height="100px" ID="MembersListBox"></asp:ListBox>
                                               <%-- </ContentTemplate>
                                                </asp:UpdatePanel>--%>
                                                <asp:Button runat="server" ID="AddButton" Text="Remove" 
                                                 CssClass="SearchButton"
                                                 OnClick="RemoveMember"
                                                 onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                 onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                 onclientclick="this.value = 'Working...';" 
                                                 />
                                            </td>
                                            <td style="padding-left: 10px;" valign="top">
                                                <div style="margin-top: -20px;">
                                                    <label>Select a member and assign them a title and description [if you so desire]</label><br />
                                                    <label>Title</label><br />
                                                    <script type="text/javascript">
                                                            
//                                                                            function OnClientLoad(editor)
//                                                                            {
//                                                                                editor.attachEventHandler("onkeyup", CountChars);
//                                                                                editor.attachEventHandler("onkeypress", function(evt){if(evt.keyCode == 13)return false;});
//                                                                            }  
                                                        function CountCharsTMem(editor, e){
                                                            
                                                                var theDiv = document.getElementById("Span2");
                                                                var theText = document.getElementById("<%=MemberTitleTextBox.ClientID %>");
                                                                theDiv.innerHTML = "characters left: "+ (15 - theText.value.length).toString();
                                                            
                                                        }
                                                    </script>
                                                    <span style="font-family: Arial; font-size: 10px; color: #ff770d; font-weight: bold;">max 15 characters
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <span style="color: Red;" id="Span2"></span></span>
                                                    <asp:TextBox runat="server" ID="MemberTitleTextBox"></asp:TextBox><br />
                                                    
                                                    <script type="text/javascript">
                                                            
//                                                                            function OnClientLoad(editor)
//                                                                            {
//                                                                                editor.attachEventHandler("onkeyup", CountChars);
//                                                                                editor.attachEventHandler("onkeypress", function(evt){if(evt.keyCode == 13)return false;});
//                                                                            }  
                                                        function CountCharsDMem(editor, e){
                                                            
                                                                var theDiv = document.getElementById("Span1");
                                                                var theText = document.getElementById("<%=MemberDescriptionTextBox.ClientID %>");
                                                                theDiv.innerHTML = "characters left: "+ (150 - theText.value.length).toString();
                                                            
                                                        }
                                                    </script>
                                                    <label>Description</label><br /><span style="font-family: Arial; font-size: 10px; color: #ff770d; font-weight: bold;">max 150 characters
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <span style="color: Red;" id="Span1"></span></span>
                                                    <asp:TextBox runat="server" TextMode="MultiLine" Height="50px" ID="MemberDescriptionTextBox"></asp:TextBox><br />
                                                    <div style="clear: both;">
                                                    <div style="float: left; padding-top: 6px;"><asp:CheckBox runat="server" ID="SharedHostingCheckBox" Text="Share hosting" />
                                                    </div>
                                                     <div style="float: left;padding-left: 10px; padding-top: 5px;" ><asp:Image CssClass="HelpImage" runat="server"  ID="Image1" ImageUrl="~/image/helpIcon.png"></asp:Image>
                                                   <rad:RadToolTip ID="RadToolTip1" runat="server" Skin="Black" Width="200px" Height="200px" 
                                                    ManualClose="true" ShowEvent="onclick" Position="MiddleRight" RelativeTo="Element" 
                                                    TargetControlID="Image1">
                                        <label>By sharing hosting with this user, you give them privilages of editing the group page, posting new group events and
                                        inserting a sticky instant message to the group on the group and event pages. As a creator of this group, you already have these privilages.</label>
                                        </rad:RadToolTip></div></div>
                                        <div style="clear: both;">
                                                    <asp:Button runat="server" ID="Button2" Text="Assign" 
                                                     CssClass="SearchButton"
                                                     OnClick="AssignTitle"
                                                     onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                     onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                     onclientclick="this.value = 'Working...';" 
                                                     />
                                                     </div><br />
                                                     <asp:Label runat="server" Font-Size="11px" ID="MembererrorLabel" ForeColor="red"></asp:Label>
                                                 </div>
                                            </td>
                                        </tr>
                                        
                                    </table>
                                    
                                   
                                        </ContentTemplate>
                                     </asp:UpdatePanel>
                               </div>
                            </div>
                        </div>
                    
                        <div align="right" style=" padding-top: 30px; padding-right: 50px;">
                            <asp:ImageButton runat="server" ID="ImageButton3" OnClick="Backwards" ImageUrl="image/BackwardsButton.png" 
                            onmouseout="this.src='image/BackwardsButton.png'" onmouseover="this.src='image/BackwardsButtonSelected.png'"  />
                            <asp:ImageButton runat="server" ID="MediaOnwardsButton" OnClick="Onwards" ImageUrl="image/OnwardsButton.png" 
                            onmouseout="this.src='image/OnwardsButton.png'" onmouseover="this.src='image/OnwardsButtonSelected.png'"  />
                         </div>
                    
                    </div>
                    <div align="center" style=" margin-left: 11px ;background-color: #515151 ;height: 4px; width: 640px;"></div>
                </asp:Panel>
            </rad:RadPageView>
<rad:RadPageView runat="server" ID="RadPageView5" TabIndex="3" >
               <asp:Panel runat="server" ID="Panel7" DefaultButton="MediaOnwardsButton2">
                    <div style="background: url('image/TabsBackgroundTop.png'); background-repeat: no-repeat; width: 668px; height: 4px;">
                    </div>
                    <div style="background: url('image/TabsBackground.png'); background-repeat: repeat-y;width: 668px;">
                        <a href="#top"></a>
                        <div class="EventDiv">
                           <div style="padding-left: 30px; width: 632px; clear: both; padding-top: 10px;" class="topDiv">
                               <h1>Media</h1>
                               <div style="padding-left: 20px;">
                                <asp:CheckBox ID="MainAttractionCheck" Text="Main Attraction <span style='color: #ff770d;'>[these will all be shown together in a rotator window. 20 is the max]</span>" runat="server" AutoPostBack="true" OnCheckedChanged="EnableMainAttractionPanel" />
                                    <asp:Panel runat="server" ID="MainAttractionPanel" Visible="false"> 
                                        <div style="padding-left: 30px;" class="topDiv">
                                            <div class="topDiv" style="padding-left: 30px; border: solid 4px #515151; width: 500px; padding-bottom: 10px; padding-left: 30px;">
                                            <asp:RadioButtonList runat="server" ID="MainAttracionRadioList" 
                                            AutoPostBack="true" RepeatDirection="horizontal" OnSelectedIndexChanged="ShowSliderOrVidPic">
                                                <asp:ListItem Text="Add YouTube Video" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Add Picture" Value="1"></asp:ListItem>
                                            </asp:RadioButtonList>
                                                <asp:Panel runat="server" ID="VideoPanel" Visible="false">
                                                    <div style="padding-left: 20px;">
                                                        <%--<asp:RadioButtonList runat="server" ID="VideoRadioList" AutoPostBack="true" 
                                                        OnSelectedIndexChanged="ShowVideo">
                                                            <asp:ListItem Text="Upload a Video" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Paste a You Tube Link" Value="1"></asp:ListItem>
                                                        </asp:RadioButtonList>--%>
                                                        <asp:Panel runat="server" ID="UploadPanel" Visible="false">
                                                            <div id="topDiv3">
                                                                   <div style="float: left; padding-top: 4px;">
                                                                    <asp:FileUpload runat="server" ID="VideoUpload" Width="230px" EnableViewState="true" />
                                                                </div>
                                                                    
                                                                <div style="float: left;">
                                                                <asp:Button runat="server" ID="ImageButton6" Text="Upload!" 
                                                                     CssClass="SearchButton" 
                                                                     OnClick="VideoUpload_Click"
                                                                     onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                                     onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                                     onclientclick="this.value = 'Working...';" 
                                                                     />
                                                                <%--<asp:ImageButton runat="server" ID="ImageButton6" OnClick="VideoUpload_Click"
                                                                     ImageUrl="image/MakeItSoButton.png" 
                                                                    onmouseout="this.src='image/MakeItSoButton.png'" 
                                                                    onmouseover="this.src='image/MakeItSoButtonSelected.png'"  />--%>
                                                                </div>
                                                            </div>
                                                        </asp:Panel>
                                                        <asp:Panel runat="server" ID="YouTubePanel">
                                                        <label>Insert the ID of the YouTube video:</label><br />
                                                                
                                                            <div class="topDiv" style="clear: both;">
                                                               <label> http://www.youtube.com/watch?v=<span style="color: Red;">YMLTwq1M2pw</span>  <--This is the ID</label>
                                                                <div style="float: left; padding-top: 4px;">
                                                                    <asp:TextBox runat="server" ID="YouTubeTextBox"></asp:TextBox>
                                                                </div>
                                                                <div style="float: left;">
                                                                <asp:Button runat="server" ID="ImageButton7" Text="Upload!" 
                                                                     CssClass="SearchButton" 
                                                                     OnClick="YouTubeUpload_Click"
                                                                     onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                                     onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                                     onclientclick="this.value = 'Working...';" 
                                                                     />
<%--                                                                    <asp:ImageButton runat="server" ID="ImageButton7"  
                                                                                OnClick="YouTubeUpload_Click"
                                                                         ImageUrl="image/MakeItSoButton.png" 
                                                                        onmouseout="this.src='image/MakeItSoButton.png'" 
                                                                        onmouseover="this.src='image/MakeItSoButtonSelected.png'"  />
--%>                                                                </div>
                                                            </div>
                                                            <label>ex: if this is the url of your YouTube video <br />http://www.youtube.com/watch?v=<span style="color: Red;">YMLTwq1M2pw</span>  <--This is the ID</label>
                                                        </asp:Panel>
                                                        
                                                     </div>
                                                 </asp:Panel> 
                                                 <div style="padding-left: 30px;" class="topDiv">
                                                        <asp:Panel runat="server" ID="PicturePanel" Visible="false">
                                                            <div id="topDiv2">
                                                                <div style="float: left; padding-top: 4px;">
                                                                    <asp:FileUpload runat="server" ID="PictureUpload" Width="230px" EnableViewState="true" />
                                                                </div>
                                                                <div style="float: left;">
                                                                <asp:Button runat="server" ID="ImageButton5" Text="Upload!" 
                                                                     CssClass="SearchButton" 
                                                                     OnClick="PictureUpload_Click"
                                                                     onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                                     onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                                     onclientclick="this.value = 'Working...';" 
                                                                     />
<%--                                                                    <asp:ImageButton runat="server" ID="ImageButton5"  OnClick="PictureUpload_Click"
                                                                     ImageUrl="image/MakeItSoButton.png"
                                                                    onmouseout="this.src='image/MakeItSoButton.png'" onmouseover="this.src='image/MakeItSoButtonSelected.png'"  />
--%>                                                                </div>
                                                                
                                                            </div>
                                                        </asp:Panel>
                                                    </div>
                                                   
                                           </div> 
                                              
                                       </div>
                                        <div style="float: left; padding-right: 70px;" class="EventDiv">
                                        <asp:Panel runat="server" ID="PicsNVideosPanel" Visible="false">
                                        <br />
                                        <label>Your Pics and Videos </label><br />
                                            <div>
                                                <div style="float: left; padding-right: 20px;">
                                                    <asp:CheckBoxList CssClass="MusicCheckBoxes" ID="PictureCheckList" runat="server"></asp:CheckBoxList>
                                                    <br />
                                                    <asp:ImageButton runat="server" OnClick="SliderNixIt" ID="PictureNixItButton" ImageUrl="image/NixItButton.png" 
                                                    onmouseout="this.src='image/NixItButton.png'" onmouseover="this.src='image/NixItButtonSelected.png'"  />
                                               </div>
                                                <div style="float: left; padding-left: 30px; width: 250px;">
                                                   <script type="text/javascript">
                                                        function CountCharsCaption(editor, e){
                                                            
                                                                var theDiv = document.getElementById("Span3");
                                                                var theText = document.getElementById("<%=CaptionTextBox.ClientID %>");
                                                                theDiv.innerHTML = "characters left: "+ (200 - theText.value.length).toString();
                                                            
                                                        }
                                                    </script>
                                                    <label>Check a picture to add caption</label><br />
                                                    <span style="font-family: Arial; font-size: 10px; color: #ff770d; font-weight: bold;">max 200 characters
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <span style="color: Red;" id="Span3"></span></span><br />
                                                    <asp:TextBox runat="server" ID="CaptionTextBox" TextMode="MultiLine" Width="200px" Height="50px"></asp:TextBox>
                                                     <asp:Button runat="server" ID="Button3" Text="Add Caption" 
                                                     CssClass="SearchButton" 
                                                     OnClick="AddCaption"
                                                     onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                     onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                     onclientclick="this.value = 'Working...';" 
                                                     />
                                                     <asp:Button runat="server" ID="Button4" Text="Edit Caption" 
                                                     CssClass="SearchButton" 
                                                     OnClick="EditCaption"
                                                     onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                     onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                     onclientclick="this.value = 'Working...';" 
                                                     />
                                                </div>
                                            </div>
                                            </asp:Panel>
                                        </div>
                                    </asp:Panel>
                                    </div>
                            </div>
                        </div>
                    
                        <div align="right" style=" padding-top: 30px; padding-right: 50px;">
                            <asp:ImageButton runat="server" ID="ImageButton4" OnClick="Backwards" ImageUrl="image/BackwardsButton.png" 
                            onmouseout="this.src='image/BackwardsButton.png'" onmouseover="this.src='image/BackwardsButtonSelected.png'"  />
                            <asp:ImageButton runat="server" ID="MediaOnwardsButton2" OnClick="Onwards" ImageUrl="image/OnwardsButton.png" 
                            onmouseout="this.src='image/OnwardsButton.png'" onmouseover="this.src='image/OnwardsButtonSelected.png'"  />
                         </div>
                    
                    </div>
                    <div align="center" style=" margin-left: 11px ;background-color: #515151 ;height: 4px; width: 640px;"></div>
                </asp:Panel>
            </rad:RadPageView>
            <rad:RadPageView runat="server" ID="RadPageView3" TabIndex="3" >
                
                <asp:Panel runat="server" ID="Panel4" DefaultButton="CategoryOnwardButton">          
                    <div style="background: url('image/TabsBackgroundTop.png'); background-repeat: no-repeat; width: 668px; height: 4px;">
                    </div>
                    <div class="EventDiv" style="background: url('image/TabsBackground.png'); background-repeat: repeat-y;  width: 668px;">
                        <div style="padding-left: 30px; width: 550px; padding-top: 10px;">
                        <h1>Categories</h1>
                        <div style="padding-left: 20px;">
                            <label><span style="font-weight: bold;"><span style='color: #1fb6e7; font-weight: bold;'>* </span>Choose Categories to put your group under. </span></label>
                            <br /><br />
<table>
                                        <tr>
                                            <td valign="top">
                                                    <%--<asp:CheckBoxList runat="server" ID="CategoriesCheckBoxes" 
                                                    RepeatColumns="4" RepeatDirection="horizontal">
                                                    
                                                    </asp:CheckBoxList>--%>
                                                    <div class="FloatLeft" style="width: 150px;">
                                                        <rad:RadTreeView Width="200px" runat="server"  
                                                        ID="CategoryTree" DataFieldID="ID" 
                                                        ForeColor="#cccccc"
                                                        DataSourceID="SqlDataSource1" CheckChildNodes="false" CheckBoxes="true">
                                                        
                                                        <DataBindings>
                                                            
                                                             <rad:RadTreeNodeBinding Checkable="true" Checked="false"
                                                             TextField="GroupName" Expanded="false" ValueField="ID" />
                                                         </DataBindings>
                                                        </rad:RadTreeView>
                                                        <asp:SqlDataSource runat="server" ID="SqlDataSource1" 
                                                            SelectCommand="SELECT * FROM GroupCategories WHERE ID <= 12 ORDER BY GroupName ASC"
                                                            ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                        </asp:SqlDataSource>
                                                    </div>
                                                    
                                               
                                            </td>
                                            <td valign="top">
                                                <asp:CheckBoxList runat="server" ID="CheckBoxList1" 
                                                    RepeatColumns="4" RepeatDirection="horizontal">
                                                    
                                                    </asp:CheckBoxList>
                                                    <div class="FloatLeft" style="width: 150px;">
                                                        <rad:RadTreeView Width="200px" ForeColor="#cccccc" runat="server"  
                                                        ID="RadTreeView2" DataFieldID="ID" DataSourceID="SqlDataSource3"
                                                        DataFieldParentID="Parent"
                                                        CheckBoxes="true">
                                                        <DataBindings>
                                                             <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                             TextField="GroupName" Expanded="false" ValueField="ID"  />
                                                         </DataBindings>
                                                        </rad:RadTreeView>
                                                        <asp:SqlDataSource runat="server" ID="SqlDataSource3" 
                                                            SelectCommand="SELECT * FROM GroupCategories WHERE ID > 12 ORDER BY GroupName ASC"
                                                            ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                        </asp:SqlDataSource>
                                                    </div>
                                            </td>
                                        </tr>
                                    </table>                        </div>
                        <div style="padding-left: 30px; width: 550px; clear: both;">
                            <div style="float: left; padding-top: 10px;">
                                <label>Don't see a category you want? Suggest one!</label>
                                <asp:TextBox runat="server" ID="CategoriesTextBox"></asp:TextBox>
                            </div>
                            <div style="padding-top: 5px;float: right;">
                                <asp:ImageButton runat="server" ID="ImageButton11" OnClick="SuggestCategoryClick"
                                                             ImageUrl="image/MakeItSoButton.png" 
                                                            onmouseout="this.src='image/MakeItSoButton.png'" onmouseover="this.src='image/MakeItSoButtonSelected.png'"  />
                            </div>
                        </div>
                        </div>
                        <div align="right" style=" padding-top: 30px; padding-right: 50px; clear: both;">
                            <asp:ImageButton runat="server" ID="ImageButton9" OnClick="Backwards" ImageUrl="image/BackwardsButton.png" 
                            onmouseout="this.src='image/BackwardsButton.png'" onmouseover="this.src='image/BackwardsButtonSelected.png'"  />
                            <asp:ImageButton runat="server" ID="CategoryOnwardButton" OnClick="Onwards" ImageUrl="image/OnwardsButton.png" 
                            onmouseout="this.src='image/OnwardsButton.png'" onmouseover="this.src='image/OnwardsButtonSelected.png'"  />
                         </div>
                    
                    </div>
                    <div align="center" style=" margin-left: 11px ;background-color: #515151 ;height: 4px; width: 640px;"></div>
                </asp:Panel>
            </rad:RadPageView>
            <rad:RadPageView runat="server" ID="RadPageView4" TabIndex="5" >
                <asp:Panel runat="server" ID="Panel5" DefaultButton="PostItButton"> 
                    <div style="background: url('image/TabsBackgroundTop.png'); background-repeat: no-repeat; width: 668px; height: 4px;">
                    </div>
                    <div class="EventDiv"  style="background: url('image/TabsBackground.png'); background-repeat: repeat-y; width: 668px;">
                        <div style="padding-left: 30px; width: 550px; padding-top: 10px;">
                        <h1>Post It</h1>
                        </div>
                        <div style="padding-left: 50px;">
                                                    <label><span style="font-weight: bold;">You're Almost Done!</span> <br /><span style='color: #ff770d;'>Read the Terms and Conditions and Go Backwards! if you'd like to change anything!</span></label>

                         <asp:Panel runat="server" ID="OwnerPanel">
                            <asp:CheckBox runat="server" ID="PrivateCheckBox" Text="Check here if you would like to make this group private."  />

                            <asp:Image CssClass="HelpImage" runat="server"  ID="QuestionMark6" ImageUrl="~/image/helpIcon.png"></asp:Image>
                                                                           <rad:RadToolTip ID="RadToolTip5" runat="server" Skin="Black" Width="200px" Height="200px" ManualClose="true" ShowEvent="onclick" Position="MiddleRight" RelativeTo="Element" TargetControlID="QuestionMark6">
                                    <label>When a group is Public, any user can locate the group though the group search. Setting 
                                    the group to Private eliminates this privilage. Users will not be able to find your group unless you 
                                    provide them with the group's direct page address.
</label>
                                    </rad:RadToolTip>
                         </asp:Panel>
                            <br />
                            <label>Terms & Conditions</label>
<div style="margin-top: 10px;margin-bottom: 10px;border: solid 2px #1B1B1B; padding-top: 20px; padding-bottom: 20px; background-color: #666666; width: 580px;">    
                            <asp:Panel runat="server" CssClass="TermsPanel"  ScrollBars="Vertical" ID="TACTextBox" Height="100px" Width="550px"></asp:Panel>
                            </div>  <%--                            <asp:TextBox runat="server" TextMode="MultiLine" Wrap="true" ID="TACTextBox" Height="100px" Width="600px"></asp:TextBox><br />
--%>                            <asp:CheckBox runat="server" ID="AgreeCheckBox" Text="<span style='color: #1fb6e7; font-weight: bold;'>* </span>Agree to the Terms & Conditions" />
                            
                            
                        </div>
                        <div>
                            <asp:Literal runat="server" ID="PostLiteral"></asp:Literal>
                        </div>
                        <div align="right" style=" padding-top: 30px; padding-right: 50px;">
                            <asp:ImageButton runat="server" ID="ImageButton12" OnClick="Backwards" ImageUrl="image/BackwardsButton.png" 
                            onmouseout="this.src='image/BackwardsButton.png'" onmouseover="this.src='image/BackwardsButtonSelected.png'"  />
                            <asp:ImageButton runat="server" ID="PostItButton" OnClick="PostIt" ImageUrl="image/PostItButton.png" 
                            onmouseout="this.src='image/PostItButton.png'" onmouseover="this.src='image/PostItButtonSelected.png'"  />
                         </div>
                    </div>
                    <div align="center" style=" margin-left: 11px ;background-color: #515151 ;height: 4px; width: 640px;"></div>
                
                </asp:Panel>
             </rad:RadPageView>

        </rad:RadMultiPage>
        </div>
        <div style="float: right;">
            <asp:Panel runat="server" ID="MessagePanel" Visible="false">
                <div style="width: 170px; border: solid 3px red; margin-right: 10px; padding: 5px; color: White; font-family: Arial;">
                    Your Messages <br />
                    <asp:Label CssClass="AddLink" runat="server" ID="YourMessagesLabel"></asp:Label>
                </div>
            </asp:Panel>
        </div>
    </div>
    </asp:Panel>
    </div>
             <%-- </ContentTemplate>--%>
<%--</asp:UpdatePanel>--%>
<%--</rad:RadAjaxPanel>--%>
</asp:Content>

