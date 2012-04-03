<%@ Page Language="C#" MasterPageFile="~/SecondMasterPage.master" ValidateRequest="false" 
AutoEventWireup="true" EnableEventValidation="false" 
CodeFile="PostAnAd.aspx.cs" Inherits="PostAnAd" Title="Post a Bulletin | HippoHappenings" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register Src="~/Controls/Ads.ascx" TagName="Ads" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/SmallButton.ascx" TagName="SmallButton" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/BlueButton.ascx" TagName="BlueButton" TagPrefix="ctrl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<rad:RadProgressManager ID="Radprogressmanager1" runat="server" EnableEmbeddedBaseStylesheet="False" EnableEmbeddedSkins="False" UniquePageIdentifier="112b806f-5e25-4f58-b795-09385fad96e5" />

<%--<rad:RadAjaxPanel runat="server">--%>
 <rad:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="false" 
                KeepInScreenBounds="true" VisibleOnPageLoad="false"  Behaviors="none"
            ID="MessageRadWindowManager" ReloadOnShow="true" Modal="true" RestrictionZoneID="RestrictionZone"
            runat="server">
                <Windows>
                    <rad:RadWindow Width="300" ClientCallBackFunction="OnClientClose" EnableEmbeddedSkins="true" 
                    VisibleStatusbar="false" Skin="Web20" Height="300" ID="MessageRadWindow" Title="Alert" runat="server">
                    </rad:RadWindow>
                    
                </Windows>
            </rad:RadWindowManager>
            <rad:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="false" 
                KeepInScreenBounds="true" VisibleOnPageLoad="false"  Behaviors="none"
            ID="RadWindowManager1" ReloadOnShow="true" Modal="true" RestrictionZoneID="RestrictionZone"
            runat="server">
                <Windows>
                    <rad:RadWindow Width="600" EnableEmbeddedSkins="true" 
                    VisibleStatusbar="false" Skin="Web20" 
                    Height="500" ID="RadWindow1" Title="Alert" runat="server">
                    </rad:RadWindow>
                </Windows>
            </rad:RadWindowManager>
            <script type="text/javascript">
            function OnClientClose(oWnd, args)
            {

                window.location = args;
            }
           function OpenRadBefore_Measles()
            {
                var win = $find("<%=RadWindow1.ClientID %>");
                win.setUrl("MeaslesChart.aspx");
                win.show(); 
                win.center(); 
                   
             }
             function OpenFeaturePricing(page)
               {
                    var win = $find("<%=RadWindow1.ClientID %>");
                    win.setUrl('PricingChart.aspx?P=' + page);
                    win.show(); 
                    win.center(); 
               }
            </script>
                       
               <script type="text/javascript" language="javascript">
        
                                                var _updateProgressDiv;
                                                var _backgroundDiv;
                                                var _gridView;
                                               var isEnabled = true;
                                               function DisablePanel()
                                               {
                                                    isEnabled = false;
                                               }
                                               
                                              
                                                
                                                </script> 
                                                

   <asp:Label runat="server" ID="ErrorLabel" ForeColor="red"></asp:Label>
   <asp:Panel runat="server" ID="LogInPanel" Visible="false">
   <div class="Text12">
    <div style="width: 820px; clear: both;">
         <div style="width: 430px; float: left;">
            <h1><asp:Label runat="server" ID="nameLabel" Text="Post A Bulletin"></asp:Label> </h1>
            <asp:Label runat="server" ID="UserNameLabel" Visible="false"></asp:Label>
            <asp:Label runat="server" ID="isEdit" Visible="false"></asp:Label>
            <asp:Label runat="server" ID="adID" Visible="false"></asp:Label>
            <asp:Label runat="server" ID="WelcomeLabel"></asp:Label>
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
        <div class="topDiv" style="width: 847px;clear: both;margin-left: -18px;position: relative;top: 207px;">
                <div style="clear: both;" class="topDiv">
                    <div style="float: left;width: 6px;"><img src="NewImages/EventFooterTopLeft.png" /></div>
                    <div style=" height: 6px;width: 835px;float: left; background: url('NewImages/EventFooterTop.png'); background-repeat: repeat-x; background-position: top;">
                        &nbsp;
                    </div>
                    <div style="float: left;width: 6px;"><img src="NewImages/EventFooterTopRight.png" /></div>
                </div>
                <div style="clear: both; background-color: #ebe7e7;"> 
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td width="6px" style="background-image: url('NewImages/EventFooterLeft.png'); background-repeat: repeat-y;"></td>
                            <td>
                                <div class="Text12" style="width: 835px; background-color: #ebe7e7;">
                                    <div class="ContentFooter">
                                        <b>Hippo Bulletin Tips</b>                                            <ol>                                                <li>                                                    Earn points toward being featured on our home page by posting bulletins whether featured or non-featured.                                                 </li>                                                <li>                                                    Post, Share, Text, Email.                                                 </li>                                                <li>                                                    Save bulletin searches and have them emailed to you when new bulletins arrive matching your criteria.                                                </li>                                                <li>                                                    Post a featured ad for as low as $3/day.                                                </li>                                            </ol>
                                    </div>
                                </div>
                            </td>
                            <td width="6px" style="background-image: url('NewImages/EventFooterRight.png'); background-repeat: repeat-y;"></td>
                        </tr>
                    </table>                       
                </div>
                <div style="clear: both;" class="topDiv">
                    <div style="float: left;width: 6px;"><img src="NewImages/EventFooterBottomLeft.png" /></div>
                    <div style="height: 6px;width: 835px;float: left; background: url('NewImages/EventFooterBottom.png'); background-repeat: repeat-x; background-position: top;">
                        &nbsp;
                    </div>
                    <div style="float: left;width: 6px;"><img src="NewImages/EventFooterBottomRight.png" /></div>
                </div>
           </div>
    </asp:Panel>
   <asp:Panel runat="server" ID="LoggedInPanel" Visible="false">
   <%--<asp:Panel runat="server" ID="IntroPanel">
        <div style="padding-bottom: 10px;">
            <asp:Label runat="server" Visible="false" ID="isFeatured"></asp:Label>
            <div class="topDiv Text12">
               
                    <h1>What are you looking to do?</h1>
               
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="padding-right: 75px;padding-top: 40px;" valign="middle">
                                <div style="padding: 8px;height: 18px; width: 203px; font-size: 14px;background: url('http://hippohappenings.com/NewImages/FeatureBulletinButton.png'); background-repeat: repeat-x;">
                                    <asp:LinkButton Text="Feature a Bulletin" runat="server" ID="ImageButton2" OnClick="SelectFeaturedPanel" CssClass="NavyLink"></asp:LinkButton>
                                </div>
                            </td>
                            <td width="510px" style="padding-top: 40px;" valign="middle">
                                <asp:Panel runat="server" ID="FreeFeaturedAdsPanel" Visible="false">
                                        <span class="Green12Link">
                                        All featured ads are now FREE</span>
                                    </asp:Panel><br />
                                    Your featured bulletin will show in a strip on our home page and all throughout our site. Your bulletin will also
                                     come up first on bulletin search results. In addition, members have the ability to receive emails with new bulletins
                                     matching their criteria via a saved bulletin search.
                                     
                                    <div style="float: right; padding-top: 5px;">
                                        <ctrl:BlueButton ID="BlueButton1" CLIENT_LINK_CLICK="OpenFeaturePricing('A')" runat="server" BUTTON_TEXT="Check Pricing >" />
                                    </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-right: 75px;padding-top: 35px;" valign="middle">
                                <div style="padding: 8px;height: 18px; width: 203px; font-size: 14px;background: url('http://hippohappenings.com/NewImages/FeatureBulletinButton.png'); background-repeat: repeat-x;">
                                    <asp:LinkButton runat="server" Text="Post A Free Bulletin" ID="Button3" OnClick="SelectFreePanel" CssClass="NavyLink"></asp:LinkButton>
                                </div>
                            </td>
                            <td width="510px" style="padding-top: 35px;" valign="middle">
                                <label>
                                    A free (non-featured) bulletin is a post that can be searched by our viewers
                                    from the Bulletin search page.        
                                </label>
                            </td>
                        </tr>
                        </table>
            </div>
            <div style="padding-top: 50px; padding-right: 10px;">
                <label>*Posting and Featuring of happenings and bulletins is open to everyone on HippoHappenings.com. We do, however, request that all happenings and bulletins be local or applicable locally; i.e. postings cannot be for purely commercial purposes applicable nationwide or globally. For the full Terms and Conditions on posting and featuring happenings please visit our
                <a class="NavyLink" href="terms-and-conditions">Terms & Condition Page.</a> For a list of allowed bulletins, please visit our and <a class="NavyLink" href="ProhibitedAds.aspx">Prohibited Ads</a> pages</label>
            </div>
        </div>
    </asp:Panel>--%>
                <div class="Text12">
        <asp:Panel runat="server" ID="TabsPanel">
        <div style="padding-bottom: 10px;">
            <div class="topDiv">
                    <div style="float: left;">
                        <h1><asp:Label runat="server" ID="TitleLabel" Text="Enter a bulletin"></asp:Label></h1>
                    </div>
                    <div style="float: left; padding-left: 5px;padding-top: 10px;">
                        <asp:Image CssClass="HelpImage" runat="server"  ID="Image8" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
                        <rad:RadToolTip ID="RadToolTip13" runat="server" Skin="Sunset" 
                        Width="200px" ManualClose="true" ShowEvent="onclick" 
                        Position="MiddleRight" RelativeTo="Element" TargetControlID="Image8">
                        <div style="padding: 10px;"><label>All HTML tags will be removed except for 
                        links. However, the header cannot contain links at all.</label></div>
                        </rad:RadToolTip>
                    </div>
                    <div style="float: right;">
                        <div style="float: right;">
                            <span id="siteseal"><script type="text/javascript" src="https://seal.godaddy.com/getSeal?sealID=VCTQw6UxQKW98668yOHTM1toSvv9HhCrsr4Zlfm5tU4bF5LIfad8lQ6XpssJ"></script><br/><a style="font-family: arial; font-size: 9px" href="https://www.godaddy.com/ssl/ssl-certificates.aspx" target="_blank">SSL</a></span>
                        </div>
                    </div>
                </div>
                
                <asp:Label runat="server" ID="Label1" Visible="false"></asp:Label>
            </div>
            <div id="topDiv6">
                <div>
                    <div style=" float: left; width: 670px;">
                        <div>
                            <rad:RadTabStrip runat="server" OnTabClick="TabClick" ID="AdTabStrip" Height="27px"
                            Skin="HippoHappenings" CssClass="EventTabStrip"
                            EnableEmbeddedSkins="false" MultiPageID="AdPostPages" SelectedIndex="0">
                                <Tabs>
                                    <rad:RadTab runat="server" PageViewID="TabOne" 
                                    ImageUrl="NewImages/DetailsOneFull.png"
                                    HoveredImageUrl="NewImages/DetailsOneFull.png" TabIndex="0" 
                                    DisabledImageUrl="NewImages/DetailsOneFull.png" 
                                    SelectedImageUrl="NewImages/DetailsOneFull.png">
                                    </rad:RadTab>
                                    <rad:RadTab runat="server"  PageViewID="TabTwo" Enabled="false"  
                                    ImageUrl="NewImages/MediaTwoFull.png" 
                                    HoveredImageUrl="NewImages/MediaTwoFull.png"  TabIndex="1" 
                                    DisabledImageUrl="NewImages/MediaTwoEmpty.png" 
                                    SelectedImageUrl="NewImages/MediaTwoFull.png" ></rad:RadTab>
                                    <rad:RadTab runat="server"  PageViewID="TabThree" Enabled="false"  
                                    ImageUrl="NewImages/CategoryThreeFull.png" 
                                    HoveredImageUrl="NewImages/CategoryThreeFull.png"  TabIndex="2" 
                                    DisabledImageUrl="NewImages/CategoryThreeEmpty.png" 
                                    SelectedImageUrl="NewImages/CategoryThreeFull.png" ></rad:RadTab>
                                    <rad:RadTab runat="server"  PageViewID="TabFour" Enabled="false"  
                                    ImageUrl="NewImages/FeatureFourFull.png" 
                                    HoveredImageUrl="NewImages/FeatureFourFull.png"  TabIndex="3" 
                                    DisabledImageUrl="NewImages/FeatureFourEmpty.png" 
                                    SelectedImageUrl="NewImages/FeatureFourFull.png" ></rad:RadTab>
                                    <rad:RadTab runat="server"  PageViewID="TabSix" Enabled="false"  
                                    ImageUrl="NewImages/PostItFiveFull.png" 
                                    HoveredImageUrl="NewImages/PostItFiveFull.png" 
                                    DisabledImageUrl="NewImages/PostItFiveEmpty.png"  TabIndex="4" 
                                    SelectedImageUrl="NewImages/PostItFiveFull.png" ></rad:RadTab>
                                </Tabs>
                            </rad:RadTabStrip>
                        </div>
                        
                        <div>
                        <rad:RadMultiPage runat="server" ID="AdPostPages" SelectedIndex="0">
                            <rad:RadPageView runat="server" ID="TabOne" TabIndex="0" >
                                <asp:Panel runat="server" ID="Panel1" DefaultButton="DetailsOnwardsButton">
                                    <div style="position: relative;">
                                    <script type="text/javascript">
                                                function ShowDetailsDiv()
                                                {
                                                    var theDiv = document.getElementById('WidthDetailsDiv');
                                                    var loadDiv = document.getElementById('LoadDetailsDiv');
                                                    
                                                    loadDiv.style.height = theDiv.offsetHeight +'px';
                                                    loadDiv.style.display = 'block';
                                                    
                                                    return true;
                                                }
                                                function HideDiv()
                                                {
                                                    var loadDiv = document.getElementById('LoadDetailsDiv');
                                                    
                                                    loadDiv.style.display = 'none';
                                                }
                                            </script>
                                            <div id="LoadDetailsDiv" style="z-index: 10000;width: 670px;display: none;position: absolute;">
                                                <div style="position: absolute; width: 100%; height: 100%; background-color: #09718F;opacity: .2;filter: alpha(opacity=20);"></div>
                                                <div style="position: absolute; width: 100%; height: 100%; ">
                                                    <table height="100%" width="100%" align="center">
                                                        <tr>
                                                            <td height="100%" width="100%" align="center" valign="middle">
                                                                <img src="image/ajax-loader.gif" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                    <div id="WidthDetailsDiv" class="topDiv" style="width: 668px; border: solid 1px #dedbdb;">
                                        <div class="EventDiv" style="padding-left: 10px; padding-bottom: 10px; clear: both;">
                                           
                                            <div id="topDiv">
                                            
                                                <div style="float: left; width: 330px;">
                                                    <table width="100%">
                                                        <tr>
                                                            <td valign="top" style="padding-right: 20px;">
                                                                <h2>Your Headline<span class="Asterisk">* </span></h2>
                                                                 <script type="text/javascript">
                                                                    function CountCharsHeadline(editor, e){
                                                                        
                                                                            var theDiv = document.getElementById("CharsDivHeadline");
                                                                            var theText = document.getElementById("ctl00_ContentPlaceHolder1_AdNameTextBox");
                                                                            theDiv.innerHTML = "characters left: "+ (70 - theText.value.length).toString();
                                                                        
                                                                    }
                                                                </script>
                                                                <span class="NavyLink12">max 70 characters &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                &nbsp;&nbsp;<span style="color: Red;" id="CharsDivHeadline"></span></span>
                                                                <asp:TextBox runat="server" onkeypress="CountCharsHeadline(event)" ID="AdNameTextBox" Width="300" ></asp:TextBox>
                                                           
                                                                <div>
                                                                    <div style="padding-top: 25px; ">
                                                                    <h2> Location of Your Bulletin </h2>
                                                                    <asp:UpdatePanel runat="server" UpdateMode="conditional">
                                                                        <ContentTemplate>
                                                                        <div style="position: relative;">
                                                                         <script type="text/javascript">
                                                                                function ShowLocDiv()
                                                                                {
                                                                                    var theDiv = document.getElementById('WidthLocDiv');
                                                                                    var loadDiv = document.getElementById('LoadLocDiv');
                                                                                    
                                                                                    loadDiv.style.height = theDiv.offsetHeight +'px';
                                                                                    loadDiv.style.display = 'block';
                                                                                    
                                                                                    return true;
                                                                                }
                                                                                function HideDiv()
                                                                                {
                                                                                    var loadDiv = document.getElementById('LoadLocDiv');
                                                                                    
                                                                                    loadDiv.style.display = 'none';
                                                                                }
                                                                            </script>
                                                                            <div id="LoadLocDiv" style="z-index: 9999;width: 300px;display: none;position: absolute;">
                                                                                <div style="position: absolute; width: 100%; height: 100%; background-color: #09718F;opacity: .2;filter: alpha(opacity=20);"></div>
                                                                                <div style="position: absolute; width: 100%; height: 100%; ">
                                                                                    <table height="100%" width="100%" align="center">
                                                                                        <tr>
                                                                                            <td height="100%" width="100%" align="center" valign="middle">
                                                                                                <img src="image/ajax-loader.gif" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                            </div>
                                                                            <div id="WidthLocDiv">
                                                                            <table cellpadding="5">
                                                                    
                                                                     <tr>
                                                                        <td colspan="2">
                                                                            <label>Country<span class="Asterisk">* </span></label><br />
                                                                            <asp:DropDownList onchange="ShowLocDiv()" OnSelectedIndexChanged="ChangeState" AutoPostBack="true" 
                                                                            runat="server"  ID="CountryDropDown"></asp:DropDownList>
                                                                            <asp:SqlDataSource runat="server" ID="SqlDataSourceCountry" SelectCommand="SELECT * FROM Countries"
                                                                            ConnectionString="<%$ ConnectionStrings:Connection %>"></asp:SqlDataSource>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td >
                                                                            <label>State<span class="Asterisk">* </span></label>
                                                                            <asp:Panel runat="server" ID="StateTextBoxPanel">
                                                                                <asp:TextBox runat="server" ID="StateTextBox" Width="100px"></asp:TextBox>
                                                                            </asp:Panel>
                                                                            <asp:Panel runat="server" ID="StateDropDownPanel" Visible="false">
                                                                                <asp:DropDownList onchange="ShowLocDiv()" runat="server" ID="StateDropDown" OnSelectedIndexChanged="ChangeCity" AutoPostBack="true"></asp:DropDownList>
                                                                            </asp:Panel>
                                                                        </td>
                                                                         <td style="padding-left: 10px;">
                                                                            <label>Major City<span class="Asterisk">* </span></label>
                                                                            <asp:Panel runat="server" ID="CityTextBoxPanel">
                                                                                <asp:TextBox ID="CityTextBox" runat="server" Width="100px" ></asp:TextBox>
                                                                            </asp:Panel>
                                                                            <asp:Panel runat="server" ID="CityDropDownPanel" Visible="false">
                                                                                <asp:DropDownList runat="server" ID="MajorCityDrop"></asp:DropDownList>
                                                                            </asp:Panel>
                                                                        </td>
                          
                                                                    </tr>
                                                                    <%--<tr>
                                                                        
                                                                        <td>
                                                                            <label>Zip <span class='NavyLink'>[Not Required]</span></label><br />
                                                                                <asp:TextBox ID="ZipTextBox" runat="server" Width="100"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                        <asp:Panel runat="server" ID="RadiusPanel">
                                                                        <label>Radius <span class='NavyLink'>[Not Required]</span></label><br />
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
                                                                    </tr>--%>
                                                                </table>
                                                                            </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                 
                                                               </div>
                                                                    
                                                                </div>
                                                           
                                                            </td>
                                                     
                                                            <td valign="top">
                                                                <h2>Your Description<span class="Asterisk">* </span></h2>
                                                                <div class="topDiv">
                                                                    <div style="float: left;">
                                                                            <span class="NavyLink">
                                                                            max 1000 characters &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                            <span style="color: Red;" id="CharsDivEditor"></span>
                                                                            </span>
                                                                         </div>
                                                               </div>  
                                                               <script type="text/javascript">
                                                            
                                                                function OnClientLoad(editor)
                                                                {
                                                                    editor.attachEventHandler("onkeyup", CountCharsEditor);
                                                                }  
                                                                function CountCharsEditor(editor)
                                                                {                                                                    
                                                                    var editor = (editor) ? editor : ((event) ? event : null);
                                                                    var node = (editor.target) ? editor.target : ((editor.srcElement) ? editor.srcElement : null);
                                                                    
                                                                    var theEditorBig = document.getElementById("ctl00_ContentPlaceHolder1_DescriptionTextBoxCenter");
                                                                    var theDiv = document.getElementById("CharsDivEditor");
                                                                    theDiv.innerHTML = navigator.appCodeName;
                                                                    if(navigator.appCodeName == 'Netscape')
                                                                    {
                                                                        theDiv.innerHTML = "characters left: " + (1000 - this.activeElement.textContent.length).toString();
                                                                    }
                                                                    else
                                                                    {
                                                                        var theEditor = $find("<%=DescriptionTextBox.ClientID %>");
                                                                        if(theEditor.get_contentArea().textContent != null)
                                                                            theDiv.innerHTML = "characters left: " + (1000 - theEditor.get_contentArea().textContent.length).toString();
                                                                        else
                                                                            theDiv.innerHTML = "characters left: " + (1000 - theEditor.get_contentArea().innerText.length).toString();
                                                                    }
                                                                }
                                                            </script>
                                                    <rad:RadEditor EditModes="Design" OnClientLoad="OnClientLoad" Skin="Vista" runat="server" 
                                                    ID="DescriptionTextBox" Height="200px" Width="300px" 
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
                                                              editor.pasteHtml(String.format("<a target='_blank' class='AddLink' href='{0}'>{1}</a>", args.url, args.name))
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
                                                   
                                                    
                                                   
                                                    
                                                    <label><span class="NavyLink12"><span class="Asterisk">* </span>required fields</span></label>
                                                </div>
                                            </div>
                                            
                                        </div>
                                     
                                        
                                             
                                       <div class="topDiv" align="right" style="width: 85px; float: right;padding-right: 10px;">
                                            <ctrl:BlueButton CLIENT_LINK_CLICK="ShowDetailsDiv()" WIDTH="76px"  ID="DetailsOnwardsButton" runat="server" 
                                            BUTTON_TEXT="Onwards" />
                                         </div>
                                    </div>
                                    </div>
                                </asp:Panel>
                            </rad:RadPageView>
                            <rad:RadPageView runat="server" ID="TabTwo" TabIndex="1" >
                                <asp:Panel runat="server" ID="Panel2" DefaultButton="MediaOnwardsButton">
                                <script type="text/javascript">
                                                function ShowMediaDiv()
                                                {
                                                    var theDiv = document.getElementById('WidthMediaDiv');
                                                    var loadDiv = document.getElementById('LoadMediaDiv');
                                                    
                                                    loadDiv.style.height = theDiv.offsetHeight +'px';
                                                    loadDiv.style.display = 'block';
                                                    
                                                    return true;
                                                }
                                                function HideDiv()
                                                {
                                                    var loadDiv = document.getElementById('LoadMediaDiv');
                                                    
                                                    loadDiv.style.display = 'none';
                                                }
                                            </script>
                                                    <div style="position: relative;">
                                            <div id="LoadMediaDiv" style="z-index: 10000;width: 670px;display: none;position: absolute;">
                                                <div style="position: absolute; width: 100%; height: 100%; background-color: #09718F;opacity: .2;filter: alpha(opacity=20);"></div>
                                                <div style="position: absolute; width: 100%; height: 100%; ">
                                                    <table height="100%" width="100%" align="center">
                                                        <tr>
                                                            <td height="100%" width="100%" align="center" valign="middle">
                                                                <img src="image/ajax-loader.gif" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                            <div id="WidthMediaDiv" class="topDiv" style="width: 668px; border: solid 1px #dedbdb;">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="buttonSubmit"/>
                            </Triggers>
                            <ContentTemplate>
                                <div style="position: relative;">
                                <script type="text/javascript">
                                    function ShowDiv()
                                    {
                                        var theDiv = document.getElementById('WidthDiv');
                                        var loadDiv = document.getElementById('LoadDiv');
                                        
                                        loadDiv.style.height = theDiv.offsetHeight +'px';
                                        loadDiv.style.display = 'block';
                                        
                                        return true;
                                    }
                                    function HideDiv()
                                    {
                                        var loadDiv = document.getElementById('LoadDiv');
                                        
                                        loadDiv.style.display = 'none';
                                    }
                                </script>
                                <div id="LoadDiv" style="width: 668px;display: none;position: absolute;">
                                    <div style="position: absolute; width: 100%; height: 100%; background-color: #09718F;opacity: .2;filter: alpha(opacity=20);"></div>
                                    <div style="position: absolute; width: 100%; height: 100%; ">
                                        <table height="100%" width="100%" align="center">
                                            <tr>
                                                <td height="100%" width="100%" align="center" valign="middle">
                                                    <img src="image/ajax-loader.gif" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <a href="#top"></a>
                                <div id="WidthDiv" class="topDiv">
                                <%--<div class="EventDiv" style="padding-top: 10px;padding-left: 10px;">
                                        <asp:CheckBox ID="MusicCheckBox" onclick="ShowDiv()" runat="server" Text="Include Music  <span class='NavyLink12'>[Upload up to 3. Required format is .mp3]</span>" OnCheckedChanged="EnableMusicPanel" AutoPostBack="true" />
                                            <asp:Panel runat="server" ID="MusicPanel" Visible="false">
                                                <div class="topDiv" style="clear: both; width: 600px;">
                                                    <div style="float: left; padding-left: 30px;">
                                                        <div style="float: left; padding-top: 4px;">
                                                            <asp:FileUpload runat="server" ID="MusicUpload" Width="230px" EnableViewState="true" />
                                                        </div>
                                                        <div style="float: left;padding-left: 5px;padding-top:3px;">
                                                        <asp:LinkButton OnClick="MusicUpload_Click" runat="server" ID="MusicUploadButton" Text="Upload"></asp:LinkButton>
                                                         <%--<ctrl:BlueButton runat="server" BUTTON_TEXT="Upload" CLIENT_LINK_CLICK="ShowMediaDiv()" ID="MusicUploadButton" />
                                                        </div>
                                                    </div>
                                                    <div style="float: right; padding-right: 70px;">
                                                        <asp:CheckBoxList CssClass="MusicCheckBoxes" ID="SongCheckList" runat="server"></asp:CheckBoxList>
                                                        <div style="padding-top: 10px;">
                                                            <ctrl:BlueButton ID="DeleteSongButton" CLIENT_LINK_CLICK="ShowDiv()" Visible="false" runat="server" BUTTON_TEXT="Nix It" />
                                                        </div>
                                                    </div>
                                                    
                                                </div>
                                            </asp:Panel>
                                    </div>--%>
                                   <script type="text/javascript">
                                            function showText()
                                            {
                                                var theDiv = document.getElementById('div5');theDiv.style.display = 'none';
                                            }
                                            function hideText()
                                            {
                                                var theDiv = document.getElementById('div5');theDiv.style.display = 'block';
                                            }
                                        </script>
                                    <div class="EventDiv" style="padding-left: 10px; width: 632px; clear: both;padding-top: 20px;">
                                        <asp:CheckBox onclick="ShowDiv()" ID="MainAttractionCheck" Text="Include Pictures and Videos <span  class='NavyLink12'>[20 max]</span><div style='float: right;padding-right: 400px;position: relative;'><div class='Text12' style='position: absolute;float: left;'><div id='div5' style='z-index: 10000; left: -100px; top: 20px;padding: 10px;position: absolute;display: none;background-color: white; width: 150px; border: solid 2px #6fa8bf;'>If you're uploading photos/images make sure you have specified a descriptive name for your file. It will help search engines, like google, find your page more effectively.</div><img onmouseout='showText()' onmouseover='hideText()' src='http://hippohappenings.com/NewImages/HelpIconNew.png' /></div></div>"   runat="server" AutoPostBack="true" OnCheckedChanged="EnableMainAttractionPanel" />
                                            <asp:Panel runat="server" ID="MainAttractionPanel" Visible="false" Enabled="false"> 
                                                <div class="topDiv" style="padding-left: 30px;">
                                                    <asp:RadioButtonList  onchange="ShowDiv()" runat="server" ID="MainAttracionRadioList" AutoPostBack="true" 
                                                    OnSelectedIndexChanged="ShowSliderOrVidPic">
                                                        <asp:ListItem Text="Add You Tube Video" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Add Picture" Value="1"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                    <div class="topDiv" style="padding-left: 30px; width: 510px; padding-bottom: 10px; padding-left: 30px;">
                                                        
                                                                <asp:Panel runat="server" ID="YouTubePanel" Visible="false">
                                                                    <div style=" border:  solid 1px #dedbdb; padding: 10px;">
                                                                        <div style="clear: both;">
                                                                            <label>Insert the ID of the YouTube video:</label><br />
                                                                            <div class="topDiv">
                                                                            <div style="float: left;padding-top: 4px;">
                                                                                <asp:TextBox runat="server" ID="YouTubeTextBox"></asp:TextBox>
                                                                            </div>
                                                                            <div style="float: left;padding-left: 5px;padding-right: 10px;">
                                                                                <ctrl:BlueButton CLIENT_LINK_CLICK="ShowDiv();return true;" WIDTH="66px"  ID="BlueButton3" runat="server" BUTTON_TEXT="Upload" />
                                                                            </div>
                                                                            <label>ex: if this is the url of your YouTube video <br />http://www.youtube.com/watch?v=<span style="color: Red;">YMLTwq1M2pw</span>  <--This is the ID</label>
                                                                             </div>
                                                                        </div>
                                                                     </div>
                                                                </asp:Panel>
                                                                
                                                         <div class="topDiv" style="padding-left: 30px;">
                                                                <asp:Panel runat="server" ID="PicturePanel" Visible="false">
                                                                    <div style=" border:  solid 1px #dedbdb;padding: 10px;">
                                                                        <div class="topDiv" id="Div1">
                                                                            <div style="float: left; padding-top: 4px;">
                                                                            
                                                                                <rad:RadUpload Skin="Web20" ID="PictureUpload" runat="server"
                                                                                    MaxFileInputsCount="20" />
                                                                                    
                                                                                <rad:RadProgressArea id="progressArea1" runat="server"/>
                                                                                
                                                                                <asp:Button id="buttonSubmit" OnClientClick="ShowDiv()" runat="server" CssClass="RadUploadSubmit" OnClick="PictureUpload_Click" text="Submit" />
                                                                            
                                                                            </div>
                                                                            <div style="float: left;padding-left: 5px; padding-top: 3px;">
                                                                            
                                                                          </div>
                                                                            
                                                                        </div>
                                                                    </div>
                                                                </asp:Panel>
                                                            </div>
                                                           
                                                   </div> 
                                                      
                                               </div>
                                                <asp:Panel runat="server" ID="UploadedVideosAndPics" Visible="false">
                                                    <div style="float: left; padding-left: 30px;" class="EventDiv topDiv">
                                                    <h2>Your Chosen Pics and Videos</h2>
                                                        <asp:CheckBoxList CssClass="MusicCheckBoxes" ID="PictureCheckList" runat="server"></asp:CheckBoxList>
                                                        <div style="padding-top: 10px;"></div>
                                                        <ctrl:BlueButton ID="PictureNixItButton" CLIENT_LINK_CLICK="ShowDiv()" runat="server"  WIDTH="54px"  BUTTON_TEXT="Nix It" />
                                                    </div>
                                                </asp:Panel>
                                            </asp:Panel>
                                    </div>
                                    </div>
                                </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                
                                    <div class="topDiv" align="right" style="padding-right: 50px;width: 660px;padding-top: 10px;clear:both;">
                                    
                                        <div style="float: right;">
                                            <ctrl:BlueButton ID="MediaOnwardsButton" WIDTH="76px"  CLIENT_LINK_CLICK="ShowMediaDiv()" runat="server" BUTTON_TEXT="Onwards" />
                                        </div>
                                        <div style="float: right;padding-right: 10px;">
                                            <ctrl:BlueButton ID="BlueButton4" CLIENT_LINK_CLICK="ShowMediaDiv()" WIDTH="87px" runat="server" BUTTON_TEXT="Backwards" />
                                        </div>
                                    </div>
                                </div>
                                </div>
                        </asp:Panel>
                            </rad:RadPageView>
                            <rad:RadPageView runat="server" ID="TabThree" TabIndex="2" >
                                
                                <asp:Panel runat="server" ID="Panel4" DefaultButton="BlueButton5"> 
                                <div style="position: relative;">         
                                    <script type="text/javascript">
                                                function ShowCatDiv()
                                                {
                                                    var theDiv = document.getElementById('WidthCatDiv');
                                                    var loadDiv = document.getElementById('LoadCatDiv');
                                                    
                                                    loadDiv.style.height = theDiv.offsetHeight +'px';
                                                    loadDiv.style.display = 'block';
                                                    
                                                    return true;
                                                }
                                            </script>
                                                    
                                            <div id="LoadCatDiv" style="z-index: 10000;width: 670px;display: none;position: absolute;">
                                                <div style="position: absolute; width: 100%; height: 100%; background-color: #09718F;opacity: .2;filter: alpha(opacity=20);"></div>
                                                <div style="position: absolute; width: 100%; height: 100%; ">
                                                    <table height="100%" width="100%" align="center">
                                                        <tr>
                                                            <td height="100%" width="100%" align="center" valign="middle">
                                                                <img src="image/ajax-loader.gif" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                    <div id="WidthCatDiv" class="topDiv" style="width: 668px; border: solid 1px #dedbdb;">
                                        <div style="padding-left: 10px; width: 600px; padding-top: 10px;">
                                            
                                            <div>
                                            <div class="topDiv">
                                                <div style="float: left;">
                                                    <h2>Select Categories for Your Bulletin<span class="Asterisk">* </span></h2>
                                                </div>
                                                <div style="float: left; padding-left: 5px;padding-top: 3px;">
                                                    <asp:Image CssClass="HelpImage" runat="server"  ID="Image2" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
                                                    <rad:RadToolTip ID="RadToolTip3" runat="server" Skin="Sunset" 
                                                    Width="200px" ManualClose="true" ShowEvent="onclick" 
                                                    Position="MiddleRight" RelativeTo="Element" TargetControlID="Image2">
                                                    <div style="padding: 10px;"><label>Selecting categories 
                                                    that most represent your bulletin will help your viewers find it 
                                                    faster via our search page.</label></div>
                                                    </rad:RadToolTip>
                                                </div>
                                            </div>
                                        <table>
                                            <tr>
                                                <td valign="top">
                                                        <div class="FloatLeft" style="width: 150px;">
                                                            <rad:RadTreeView  Width="150px" runat="server"  
                                                            ID="CategoryTree" DataFieldID="ID" DataSourceID="SqlDataSource1" 
                                                            DataFieldParentID="ParentID" Skin="Vista"
                                                            CheckBoxes="true">
                                                            <DataBindings>
                                                                 <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                                 TextField="Name" Expanded="false" ValueField="ID" />
                                                             </DataBindings>
                                                            </rad:RadTreeView>
                                                            <asp:SqlDataSource runat="server" ID="SqlDataSource1" 
                                                                SelectCommand="SELECT * FROM AdCategories WHERE ID <= 99 OR ID=221 OR ID=222 ORDER BY Name ASC"
                                                                ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                            </asp:SqlDataSource>
                                                        </div>
                                                        
                                                   
                                                </td>
                                                <td valign="top">
                                                        <div class="FloatLeft" style="width: 150px;">
                                                            <rad:RadTreeView  Skin="Vista" Width="150px" runat="server"  
                                                            ID="RadTreeView2" DataFieldID="ID" DataSourceID="SqlDataSource3" 
                                                            DataFieldParentID="ParentID"
                                                            CheckBoxes="true">
                                                            <DataBindings>
                                                                 <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                                 TextField="Name" Expanded="false" ValueField="ID"  />
                                                             </DataBindings>
                                                            </rad:RadTreeView>
                                                            <asp:SqlDataSource runat="server" ID="SqlDataSource3" 
                                                                SelectCommand="SELECT * FROM AdCategories WHERE ID > 99 AND ID <> 221 AND ID <> 222  ORDER BY Name ASC"
                                                                ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                            </asp:SqlDataSource>
                                                        </div>
                                                </td>
                                                <%--<td valign="top">
                                                    <div style=" width: 150px;">
                                                        <rad:RadTreeView Width="150px"  runat="server"  
                                                        ID="RadTreeView1"  Skin="Vista" DataFieldID="ID" DataSourceID="SqlDataSource2" 
                                                        DataFieldParentID="ParentID"
                                                        CheckBoxes="true">
                                                        <DataBindings>
                                                             <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                             TextField="Name" Expanded="false" ValueField="ID"  />
                                                         </DataBindings>
                                                        </rad:RadTreeView>
                                                        <asp:SqlDataSource runat="server" ID="SqlDataSource2" 
                                                            SelectCommand=""
                                                            ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                        </asp:SqlDataSource>
                                                    </div>
                                                </td>--%>
                                                <%--<td valign="top">
                                                    <div style=" width: 150px;">
                                                        <rad:RadTreeView Width="150px"  runat="server"  
                                                        ID="RadTreeView3"  Skin="Vista" DataFieldID="ID" DataSourceID="SqlDataSource4" 
                                                        DataFieldParentID="ParentID"
                                                        CheckBoxes="true">
                                                        <DataBindings>
                                                             <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                             TextField="Name" Expanded="false" ValueField="ID"  />
                                                         </DataBindings>
                                                        </rad:RadTreeView>
                                                        <asp:SqlDataSource runat="server" ID="SqlDataSource4" 
                                                            SelectCommand=""
                                                            ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                        </asp:SqlDataSource>
                                                    </div>
                                                </td>--%>
                                            </tr>
                                        </table>
                                                                        
                                        <div style=" width: 550px; clear: both;" class="topDiv">
                                            <div style="float: left; padding-top: 10px;">
                                                <label>Don't see a category you want? Suggest one!</label>
                                                <asp:TextBox runat="server" ID="CategoriesTextBox"></asp:TextBox>
                                            </div>
                                            <div style="padding-top: 9px; float: left; padding-left: 10px;">   
                                              <ctrl:BlueButton CLIENT_LINK_CLICK="ShowCatDiv()" ID="ImageButton11" runat="server"  WIDTH="122px" BUTTON_TEXT="Send Suggestion" />
                                            </div>
                                        </div>
                                         
                                         
                                         </div>
                                         <div class="topDiv" align="right" style="padding-right: 50px;width: 650px;clear: both;padding-top: 10px;">
                            
                                            <div style="float: right;">
                                                <ctrl:BlueButton CLIENT_LINK_CLICK="ShowCatDiv()" WIDTH="76px"  ID="BlueButton5" runat="server" BUTTON_TEXT="Onwards" />
                                            </div>
                                            <div style="float: right;padding-right: 10px;">
                                                <ctrl:BlueButton CLIENT_LINK_CLICK="ShowCatDiv()" ID="BlueButton6" WIDTH="87px" runat="server" BUTTON_TEXT="Backwards" />
                                            </div>
                                         </div>
                                    
                                    </div>
                                    </div>
                                    </div>
                                </asp:Panel>
                            </rad:RadPageView>
                            <rad:RadPageView runat="server" ID="RadPageView5" TabIndex="3" >
                
                                <asp:Panel runat="server" ID="Panel3" DefaultButton="BlueButton15">    
                                    <div style="position: relative;">     
                                   <script type="text/javascript">
                                                function ShowAllDiv()
                                                {
                                                    var theDiv = document.getElementById('WidthAllDiv');
                                                    var loadDiv = document.getElementById('LoadAllDiv');
                                                    
                                                    loadDiv.style.height = theDiv.offsetHeight +'px';
                                                    loadDiv.style.display = 'block';
                                                }
                                            </script>
                                            <div id="LoadAllDiv" style="z-index: 10000;width: 668px;display: none;position: absolute;">
                                                <div style="position: absolute; width: 100%; height: 100%; background-color: #09718F;opacity: .2;filter: alpha(opacity=20);"></div>
                                                <div style="position: absolute; width: 100%; height: 100%; ">
                                                    <table height="100%" width="100%" align="center">
                                                        <tr>
                                                            <td height="100%" width="100%" align="center" valign="middle">
                                                                <img src="image/ajax-loader.gif" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                    <div id="WidthAllDiv" class="Text12" style=" width: 668px; border: solid 1px #dedbdb;">
                                    
                                    <asp:UpdatePanel ID="FetUpdatePanel" runat="server" UpdateMode="conditional">
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="ImageButton5" />
                                            <asp:PostBackTrigger ControlID="BlueButton10" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <div style="position: relative;">
                                            <script type="text/javascript">
                                                function ShowFetDiv()
                                                {
                                                    var theDiv = document.getElementById('WidthFetDiv');
                                                    var loadDiv = document.getElementById('LoadFetDiv');
                                                    
                                                    loadDiv.style.height = theDiv.offsetHeight +'px';
                                                    loadDiv.style.display = 'block';
                                                }
                                                function HideFetDiv()
                                                {
                                                    var loadDiv = document.getElementById('LoadFetDiv');
                                                    
                                                    loadDiv.style.display = 'none';
                                                }
                                            </script>
                                            <div id="LoadFetDiv" style="width: 668px;display: none;position: absolute;">
                                                <div style="position: absolute; width: 100%; height: 100%; background-color: #09718F;opacity: .2;filter: alpha(opacity=20);"></div>
                                                <div style="position: absolute; width: 100%; height: 100%; ">
                                                    <table height="100%" width="100%" align="center">
                                                        <tr>
                                                            <td height="100%" width="100%" align="center" valign="middle">
                                                                <img src="image/ajax-loader.gif" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                            <div id="WidthFetDiv" class="topDiv" style="padding-left: 10px; width: 650px;">
                                            <div class="topDiv">
                                                <div style="float: left;">
                                                    <h1>Want To Feature Your Bulletin?</h1>
                                                </div>
                                                <div style="float: right;padding-top: 10px;">
                                                    <ctrl:BlueButton ID="BlueButton8" runat="server" CLIENT_LINK_CLICK="OpenFeaturePricing('A');" WIDTH="170px" BUTTON_TEXT="Featured Bulletin Pricing >" />
                                                </div>
                                            </div>
                                            <div align="center" style="padding-top: 30px;">
                                            <div class="FeaturePrice">
                                                <h2>It's only $<asp:Literal runat="server" ID="PriceLiteral"></asp:Literal> / day!</h2>
                                            </div>
                                                Your featured bulletin will be show in our bulletin strip throughout the site. It will also come up first 
                                                in our bulletin search results on the bulletin search page. In
                                                addition, any regular happenings (happy hour, Friday dance night, etc. ) happening at the locale will be
                                                featured on our home page 'Stuff To Do' strip. You can also feature a specific event at a locale; to do this
                                                complete the creation of your locale and then go to 'Add Event'.
                                                <%--<div class="PostFreeDiv" style="padding: 20px;">
                                                    <h2><asp:Panel runat="server" ID="PricePanel"></asp:Panel></h2>
                                                </div>--%>
                                            </div>
                                             <div align="center" class="topDiv" style="padding-top: 10px; padding-bottom: 30px;">
                                                <div style="width: 106px;">
                                                    <ctrl:BlueButton ID="BlueButton9" CLIENT_LINK_CLICK="ShowFetDiv()" WIDTH="107px" runat="server" BUTTON_TEXT="Yes, Definitely!" />
                                                </div>
                                                <div style="width: 106px;">
                                                    <ctrl:BlueButton ID="BlueButton10" CLIENT_LINK_CLICK="ShowFetDiv()" WIDTH="107px" runat="server" BUTTON_TEXT="No, Thank You" />
                                                </div>
                                             </div>   
                                            <asp:Panel runat="server" ID="FeaturePanel" Visible="false">
                                                <div>
                                                    <div class="topDiv FooterBottom">
                                                        <asp:Panel runat="server" ID="MajorPanel">
                                                           <b><div>Your major city is <asp:Label runat="server" ForeColor="Red" ID="MajorCity"></asp:Label>. If this is not correct,
                                                            make sure you provided the right major city.</div></b><br /><br />
                                                        </asp:Panel>
                                                        <div>
                                                            <asp:CheckBox runat="server" onclick="ShowFetDiv()" AutoPostBack="true" OnCheckedChanged="EnableAdMediaPanel" 
                                                            ID="BannerAdCheckBox" Text="<span class='SemiH2'>Include a Banner Image</span>" />
                                                        </div>
                                                        <div class="topDiv" style="border-top: solid 1px #dedbdb; width: 614px; padding-top: 10px;">
                                                        <asp:Panel runat="server" ID="AdMediaPanel" Visible="false" Enabled="false">
                                                            <div>
                                                                <div style="margin-left: 20px; clear: both; padding-top: 10px;">
                                                                    <h2>choose a template</h2>
                                                                </div>   
                                                                <div class="topDiv">
                                                                    <div style="margin-left: 20px;">
                                                                       <asp:RadioButtonList AutoPostBack="true" OnSelectedIndexChanged="ChangeFeaturedText" CssClass="AdTemplates" runat="server" ID="TemplateRadioList" RepeatDirection="Horizontal">
                                                                            <asp:ListItem onclick="ShowFetDiv()" Value="1" Selected="true" Text="<div style='float: right;'><img src='../NewImages/QuaterTemplate.png'/></div>"></asp:ListItem>
                                                                            <asp:ListItem onclick="ShowFetDiv()" Value="2" Text="<div style='float: right;'><img src='../NewImages/HalfTemplate.png'/></div>"></asp:ListItem>
                                                                            <asp:ListItem onclick="ShowFetDiv()" Value="3" Text="<div style='float: right;'><img src='../NewImages/FullTemplate.png'/></div>"></asp:ListItem>
                                                                       </asp:RadioButtonList>
                                                                    </div>
                                                                    <div style="margin-left: 20px; clear: both; padding-top: 10px;">
                                                                        <h2>choose an image</h2>
                                                                    </div> 
                                                                    <div style="margin-left: 20px;">
                                                                        <div style="float: left;">
                                                                            <asp:FileUpload runat="server"  ID="AdPictureUpload" Width="230px" EnableViewState="true" />
                                                                        </div>
                                                                        <div style="float: left;">
                                                                        <ctrl:BlueButton runat="server" CLIENT_LINK_CLICK="ShowFetDiv()" WIDTH="66px"  ID="ImageButton5" BUTTON_TEXT="Upload" />

                                                                        </div>
                                                                        <div style="float: right; padding-right: 70px;">
                                                                            <asp:CheckBoxList CssClass="MusicCheckBoxes" ID="AdPictureCheckList" runat="server"></asp:CheckBoxList>
                                                                            <ctrl:BlueButton CLIENT_LINK_CLICK="ShowFetDiv()" runat="server" ID="AdNixItButton"  WIDTH="54px"  Visible="false" BUTTON_TEXT="Nix It" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </asp:Panel>
                                                        </div>
                                                    </div>
                                                    <div class="topDiv">
                                                        <asp:LinkButton runat="server" ID="ChangeMaxText" OnClick="ChangeFeaturedText"></asp:LinkButton>
                                                                <div class="topDiv" style="padding-top: 10px;">
                                                                <div style="float: left;">
                                                                    <h2>Bulletin Summary<span class="Asterisk">*</span></h2>
                                                                </div>
                                                                <div style="float: left; padding-left: 5px;padding-top: 4px;">
                                                                    <asp:Image CssClass="HelpImage" runat="server"  ID="Image1" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
                                                                    <rad:RadToolTip ID="RadToolTip2" runat="server" Skin="Sunset" 
                                                                    Width="200px" ManualClose="true" ShowEvent="onclick" 
                                                                    Position="MiddleRight" RelativeTo="Element" TargetControlID="Image1">
                                                                        <div style="padding: 10px;"><label>
                                                                            This summary will show along with your chosen image in your bulletin throughout the site.
                                                                        </label></div>
                                                                    </rad:RadToolTip>
                                                                </div>
                                                            </div>
                                                            <div class="topDiv" style="border-top: solid 1px #dedbdb; width: 614px; padding-top: 10px;">
                                                                <asp:Panel Visible="false" runat="server" ID="FeaturedTextNotAllowedPanel">
                                                                <label>Featured text is not allowed for an ad template where the banner image fills up the entire ad.</label>
                                                                </asp:Panel>
                                                                <asp:Panel runat="server" ID="FeaturedTextPanel">
                                                                <div class="topDiv" style="padding-left: 30px;">
                                                                    <div style="float: left;">
                                                                        <script type="text/javascript">
                                                                        
                                                                            function CountChars(editor, e){
                                                                                
                                                                                    var theDiv = document.getElementById("CharsDiv");
                                                                                    var theText = document.getElementById("ctl00_ContentPlaceHolder1_SummaryTextBox");
                                                                                    theDiv.innerHTML = "characters left: "+ (250 - theText.value.length).toString();
                                                                                
                                                                            }
                                                                            function CountChars100(editor, e){
                                                                                
                                                                                    var theDiv = document.getElementById("CharsDiv");
                                                                                    var theText = document.getElementById("ctl00_ContentPlaceHolder1_SummaryTextBox");
                                                                                    theDiv.innerHTML = "characters left: "+ (100 - theText.value.length).toString();
                                                                                
                                                                            }
                                                                            function DisableEnter(evt){
                                                                                if(evt.keyCode == 13)
                                                                                    return false;
                                                                                else
                                                                                    CountChars(evt);
                                                                            }
                                                                        </script>
                                                                        
                                                                        <span class="NavyLink">
                                                                        <asp:Literal runat="server" ID="FeaturedTextLiteral" Text="max 250 characters &nbsp;&nbsp;&nbsp;">
                                                                        </asp:Literal><span style="color: Red;" id="CharsDiv"></span></span>
                                                                        <br />
                                                                        <textarea runat="server" id="SummaryTextBox" 
                                                                        style=" padding: 3px;font-family: Arial; font-size: 12px;  border: solid 1px #dedbdb; width: 200px; height: 100px;" 
                                                                        onkeypress="DisableEnter"></textarea>
                                                                       
                                                                    </div>
                                                                   
                                                                </div>
                                                                </asp:Panel>
                                                            </div>
                                                    </div>
                                                    <div class="topDiv">
                                                       
                                                        <div style="float: left;padding-top: 30px;">
                                                            <h2>Pick the dates you want your bulletin featured</h2>
                                                        </div>
                                                        <div style="clear: both; padding-bottom: 5px;">
                                                            *Days on which your bulletin has already been featured show up in gray below.
                                                            <asp:Label runat="server" ID="FeatureErrorLabel" ForeColor="red"></asp:Label>
                                                        </div>
                                                    </div>
                                                        <div class="topDiv" style="border-top: solid 1px #dedbdb; width: 614px; padding-top: 10px;">
                                                            <div style="float: left; width: 215px;">
                                                                
                                                                <rad:RadDatePicker runat="server" ID="FeaturedDatePicker" Skin="Web20"></rad:RadDatePicker><br />
                                                                <div style="padding-top: 5px;">[7 days max, for next 30 days]</div>
                                                            </div>
                                                            <div style="float: left; width: 130px; padding-top: 50px;">
                                                                 
                                                                    <ctrl:BlueButton CLIENT_LINK_CLICK="ShowFetDiv()" ID="BlueButton7" WIDTH="95px" runat="server" BUTTON_TEXT="Add Date >>" />
                                                                 
                                                                     <ctrl:BlueButton CLIENT_LINK_CLICK="ShowFetDiv()" WIDTH="90px" ID="BlueButton11" runat="server"
                                                                     BUTTON_TEXT="<< Remove" />
                                                                
                                                            </div>
                                                            <div style="float: left; ">
                                                                <h2>Your Dates:</h2>
                                                                            <asp:ListBox runat="server" EnableViewState="true" ID="FeatureDatesListBox" 
                                                                              CssClass="ListBox" SelectionMode="single" Width="250px" Height="100px"></asp:ListBox>
                                                            </div>
                                                            <asp:Label runat="server" ID="DaysErrorLabel" ForeColor="red"></asp:Label>
                                                        </div>
                                            
                                                </div>
                                                 <div>
                                                    <div class="topDiv">
                                                        <div style="float: left;">
                                                            <h2>Pick Your Search Terms</h2>
                                                        </div>
                                                        <div style="float: left; padding-left: 5px;padding-top: 4px;">
                                                            <asp:Image CssClass="HelpImage" runat="server"  ID="Image3" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
                                                            <rad:RadToolTip ID="RadToolTip1" runat="server" Skin="Sunset" 
                                                            Width="300px" ManualClose="true" ShowEvent="onclick" 
                                                            Position="MiddleRight" RelativeTo="Element" TargetControlID="Image3">
                                                            <div style="padding: 10px;line-height: 20px;"><label>Selecting no search terms will mean your bulletin will pop 
                                                            up in searches where no keywords are entered. If you select any keywords, your featured 
                                                            bulletin will pop up in searches where those keywords are entered and in searches 
                                                            where no keywords are entered as well. Each keyword is limited to four bulletins, 
                                                            and so, picking a 
                                                            keyword guarantees that your bulletin will show up in the top four results of a search with your keyword. 
                                                            In the searches without keywords, your bulletin will show up in the top four results chosen randomly 
                                                            per each time searched. Meaning, if you do not include search terms your bulletin is <u>not</u> 
                                                            guaranteed to show up in the first four results if there is over 4 featured bulletins for your location.<br /><br />
                                                            If this all sounds too complicated, strictly speaking, it is better to put in search terms!</label></div>
                                                            </rad:RadToolTip>
                                                        </div>
                                                    </div>
                                                        <div class="topDiv" style="border-top: solid 1px #dedbdb; width: 614px; padding-top: 10px;">
                                                            <div style="float: left; width: 215px;">
                                                                <asp:TextBox runat="server" ID="TermsBox"></asp:TextBox>
                                                            </div>
                                                            <div style="float: left; width: 130px; padding-top: 50px;">
                                                                 <ctrl:BlueButton CLIENT_LINK_CLICK="ShowFetDiv()" WIDTH="97px" ID="BlueButton12" runat="server" BUTTON_TEXT="Add Term >>" />
                                                                 <ctrl:BlueButton CLIENT_LINK_CLICK="ShowFetDiv()" WIDTH="90px" ID="BlueButton13" runat="server" 
                                                                 BUTTON_TEXT="<< Remove" />
                                                            </div>
                                                            <div style="float: left; ">
                                                                <h2>Your Terms:</h2>
                                                                            <asp:ListBox runat="server" EnableViewState="true" ID="SearchTermsListBox" 
                                                                              CssClass="ListBox" SelectionMode="single" Width="250px" Height="100px"></asp:ListBox>
                                                            </div>
                                                        </div>
                                                        <asp:Label runat="server" ID="TermsErrorLabel" ForeColor="red"></asp:Label>

                                            
                                                </div>
                                                <div>
                                                    <div class="topDiv">
                                                        <div style="float: left;">
                                                            <h2>Your Pricing</h2>
                                                        </div>
                                                        <div style="float: left; padding-left: 5px;padding-top: 4px;">
                                                            <asp:Image CssClass="HelpImage" runat="server"  ID="Image7" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
                                                            <rad:RadToolTip ID="RadToolTip8" runat="server" Skin="Sunset" 
                                                                Width="200px" ManualClose="true" ShowEvent="onclick" 
                                                                Position="MiddleRight" RelativeTo="Element" TargetControlID="Image7">
                                                                <div style="padding: 10px;"><label>Your price depends on the number of members in the location you are featuring the bulletin,
                                                                number of days, number of search terms, and number of existing featured bulletins on the days you 
                                                                have chosen. The more featured bulletins, the less it costs to feature a bulletin.</label></div>
                                                            </rad:RadToolTip>
                                                        </div>
                                                    </div>
                                                    
                                                    <table align="center" style="border: solid 1px #dedbdb;" width="100%">
                                                        <tr>
                                                            <th>Day</th>
                                                       
                                                            <th>Standard Rate / day</th>
                                                        
                                                            <th>Adjustment for Members / day</th>
                                                        
                                                            <th>Adjustment for Bulletin / day</th>
                                                     
                                                            <th>Price / day</th>
                                                        </tr>
                                                        <asp:Literal runat="server" Text="<td align='center'>0</td><td align='center'>$0</td><td align='center'>$0</td><td align='center'>$0</td><td align='center'>$0</td>" ID="PricingLiteral"></asp:Literal>
                                                        <tr>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td align="right"><b>Search Terms:</b></td>
                                                            <td align="center"><b>$<asp:Label runat="server" ID="NumSearchTerms" Text="0.00"></asp:Label></b></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td align="right"><b>Total:</b></td>
                                                            <td align="center"><b>$<asp:Label runat="server" ID="TotalLabel" Text="0.00"></asp:Label></b></td>
                                                        </tr>
                                                    </table>
                                                    <div style="float: right;padding-top: 10px;">
                                                        <ctrl:BlueButton CLIENT_LINK_CLICK="ShowFetDiv()" ID="BlueButton14" BUTTON_TEXT="Update" runat="server" />
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                        <div class="topDiv" align="right" style="padding-right: 50px;width: 660px;clear: both;padding-top: 10px;">
                                            
                                            <div style="float: right;">
                                                <ctrl:BlueButton ID="BlueButton15" CLIENT_LINK_CLICK="ShowAllDiv()" WIDTH="76px"  runat="server" BUTTON_TEXT="Onwards" />
                                            </div>
                                            <div style="float: right;padding-right: 10px;">
                                                <ctrl:BlueButton ID="BlueButton16" CLIENT_LINK_CLICK="ShowAllDiv()" WIDTH="87px" runat="server" BUTTON_TEXT="Backwards" />
                                            </div>
                                         </div>
                                    </div>
                                    </div> 
                                </asp:Panel>
                            </rad:RadPageView>
                            <rad:RadPageView runat="server" ID="TabSix" TabIndex="4" >
                              
                                <asp:Panel runat="server" ID="Panel5" DefaultButton="BlueButton17"> 
                                        <script type="text/javascript">
                                                function ShowPostDiv()
                                                {
                                                    var theDiv = document.getElementById('WidthPostDiv');
                                                    var loadDiv = document.getElementById('LoadPostDiv');
                                                    
                                                    loadDiv.style.height = theDiv.offsetHeight +'px';
                                                    loadDiv.style.display = 'block';
                                                    
                                                    return true;
                                                }
                                            </script>
                                     <div style="position: relative;">              
                                            <div id="LoadPostDiv" style="z-index: 10000;width: 670px;display: none;position: absolute;">
                                                <div style="position: absolute; width: 100%; height: 100%; background-color: #09718F;opacity: .2;filter: alpha(opacity=20);"></div>
                                                <div style="position: absolute; width: 100%; height: 100%; ">
                                                    <table height="100%" width="100%" align="center">
                                                        <tr>
                                                            <td height="100%" width="100%" align="center" valign="middle">
                                                                <img src="image/ajax-loader.gif" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                            <div  id="WidthPostDiv">
                                     <div class="Text12"  style="width: 668px; border: solid 1px #dedbdb;">
                                        <div style="padding-left: 10px;">
                                 
                                        
                                      
                                          <h1 class="SideColumn">You're Almost Done!</h1>
                                          <div style="width: 640px;padding-left: 10px;">
                                                Preview your post information below and read the Terms 
                                                and Conditions. Go backwards or click on the tabs if 
                                                you'd like to change anything. If you decided to 
                                                feature your bulletin, please fill in the payment information.
                                            </div>
                                        
                                        <asp:Panel runat="server" ID="PromoPanel" Visible="false">
                                            <table>
                                                <tr>
                                                    <td colspan="3">
                                                        <h1 class="SideColumn">Promo Code</h1>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="PromoCodeTextBox"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <ctrl:BlueButton ID="BlueButton1" WIDTH="76px"  runat="server" BUTTON_TEXT="Validate" />
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="PromoErrorLabel" ForeColor="red"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>    
                                            
                                        <asp:Panel runat="server" ID="PaymentPanel" Visible="false">
                                    <div style="border: solid 1px #dedbdb;clear: both;margin-top: 30px;margin-bottom: 30px;margin-right: 10px; padding: 10px; background-color: #eef7fb;">
                                        <h1 class="SideColumn">Payment Information</h1>
                                        <div align="center">
                                        <div align="center" style="width: 450px;color: #535252;padding: 5px; background-color: #ebe7e7; border: solid 1px #09718F;">
                                            <span class="Asterisk">*</span>Your payment will be charged when you click 'Post It' on this tab.<br />
                                            <span class="Asterisk">*</span>HippoHappenings does not store any credit card information in our systems.
                                        </div>
                                        </div>
                                        <div style="padding-left: 10px;">
                                            <table cellpadding="0" cellspacing="0" width="100%" class="PaymentTable">
                                                <tr>
                                                    <td colspan="2">
                                                        <table cellpadding="0" cellspacing="0" >
                                                            <tr>
                                                                <td colspan="2"><h2>Full name as it appears on your card</h2></td>
                                                            </tr>
                                                            <tr>
                                                                <td>First Name: <br /><asp:TextBox runat="server" ID="FirstNameTextBox"></asp:TextBox></td>
                                                                <td>Last Name: <br /><asp:TextBox runat="server" ID="LastNameTextBoxtBox"></asp:TextBox></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td valign="top">
                                                        <div>
                                                            <!-- PayPal Logo --><table border="0" cellpadding="10" cellspacing="0" align="center"><tr><td align="center"></td></tr>
                                                                <tr><td align="center"><a href="#" onclick="javascript:window.open('https://www.paypal.com/cgi-bin/webscr?cmd=xpt/Marketing/popup/OLCWhatIsPayPal-outside','olcwhatispaypal','toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, width=400, height=350');"><img  src="https://www.paypal.com/en_US/i/bnr/horizontal_solution_PPeCheck.gif" border="0" alt="Solution Graphics"></a></td></tr></table><!-- PayPal Logo -->
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <h2>Billing Address</h2>
                                                    </td>
                                                    <td>
                                                        <h2>Credit Card Information</h2>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        Country: <br /><asp:DropDownList runat="server" ID="BillingCountry" onchange="ShowFetDiv()" 
                                                        OnSelectedIndexChanged="ChangeStateAction" AutoPostBack="true" ></asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    Card Number: <br /><asp:TextBox EnableViewState="false" runat="server" ID="CardNumberTextBox"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    Card Type: <br /><asp:DropDownList runat="server" ID="CardTypeDropDown"></asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">Street Address: <br /><asp:TextBox runat="server" ID="BillingStreetAddressTextBox"></asp:TextBox></td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    Expiratin Date: <br /><asp:DropDownList runat="server" ID="ExpirationMonth">
                                                                        <asp:ListItem>1</asp:ListItem>
                                                                        <asp:ListItem>2</asp:ListItem>
                                                                        <asp:ListItem>3</asp:ListItem>
                                                                        <asp:ListItem>4</asp:ListItem>
                                                                        <asp:ListItem>5</asp:ListItem>
                                                                        <asp:ListItem>6</asp:ListItem>
                                                                        <asp:ListItem>7</asp:ListItem>
                                                                        <asp:ListItem>8</asp:ListItem>
                                                                        <asp:ListItem>9</asp:ListItem>
                                                                        <asp:ListItem>10</asp:ListItem>
                                                                        <asp:ListItem>11</asp:ListItem>
                                                                        <asp:ListItem>12</asp:ListItem>
                                                                    </asp:DropDownList> / <asp:DropDownList runat="server" ID="ExpirationYear">
                                                                        <asp:ListItem>2011</asp:ListItem>
                                                                        <asp:ListItem>2012</asp:ListItem>
                                                                        <asp:ListItem>2013</asp:ListItem>
                                                                        <asp:ListItem>2014</asp:ListItem>
                                                                        <asp:ListItem>2015</asp:ListItem>
                                                                        <asp:ListItem>2016</asp:ListItem>
                                                                        <asp:ListItem>2017</asp:ListItem>
                                                                        <asp:ListItem>2018</asp:ListItem>
                                                                        <asp:ListItem>2019</asp:ListItem>
                                                                        <asp:ListItem>2020</asp:ListItem>
                                                                        <asp:ListItem>2021</asp:ListItem>
                                                                        <asp:ListItem>2022</asp:ListItem>
                                                                        <asp:ListItem>2023</asp:ListItem>
                                                                        <asp:ListItem>2024</asp:ListItem>
                                                                        <asp:ListItem>2025</asp:ListItem>
                                                                        <asp:ListItem>2026</asp:ListItem>
                                                                        <asp:ListItem>2027</asp:ListItem>
                                                                        <asp:ListItem>2028</asp:ListItem>
                                                                        <asp:ListItem>2029</asp:ListItem>
                                                                        <asp:ListItem>2030</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    
                                                                    <div style="float: left;">
                                                                        CSC/CVC: <br /><asp:TextBox runat="server" EnableViewState="false" ID="CSVTextBox" Width="70px"></asp:TextBox>
                                                                    </div>
                                                                    <div style="float: left; padding-left: 5px;padding-top: 10px;">
                                                                        <asp:Image CssClass="HelpImage" runat="server"  ID="Image4" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
                                                                        <rad:RadToolTip ID="RadToolTip9" runat="server" Skin="Sunset" 
                                                                        Width="400px" ManualClose="true" ShowEvent="onclick" 
                                                                        Position="TopCenter" RelativeTo="Element" TargetControlID="Image8">
                                                                        <div style="padding: 10px;" align="center"><label>You can find your Card Security Code or Card Validation Code in the following spots:</label><br /><br />
                                                                        <a href="http://www.merchantplus.com" target="_blank"><IMG alt="Credit Cards - MerchantPlus.com" width="90%" height="90%" src="http://www.merchantplus.com/images/cc/cv_card.jpg" border="0"></a>
                                                                        </div>
                                                                        </rad:RadToolTip>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>City: <br /><asp:TextBox runat="server" ID="BillingCityTextBox"></asp:TextBox></td>
                                                                <td>State:  <br />
                                                                    <asp:DropDownList runat="server" AutoPostBack="true" onchange="ShowFetDiv()" OnSelectedIndexChanged="UpdateState" 
                                                                        ID="BillingStateDropDown"></asp:DropDownList>
                                                                
                                                                    <asp:TextBox runat="server" ID="BillingStateTextBox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        Zip Code:  <br /><asp:TextBox Width="100px" runat="server" ID="BillingZipCodeTextBox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                
                                            </table>
                                        </div>

                                    </div>
                                </asp:Panel>
                                
                                <div>
                                <h1 class="SideColumn">Terms and Conditions</h1>
                                <div style="padding-left: 10px;">
                                        <div style="margin-top: 10px;margin-bottom: 10px; border: solid 1px #dedbdb; width: 625px;">    
                                    <asp:Panel runat="server" CssClass="TermsPanel"  ScrollBars="Vertical" ID="TACTextBox" Height="100px" Width="605px"></asp:Panel>
                                    </div>                              <asp:CheckBox runat="server" ID="AgreeCheckBox" Text="Agree to the Terms & Conditions<span  class='Asterisk'>* </span>" />
                                </div>
                                </div>
                                        </div>
                                        <div>
                                            <asp:Literal runat="server" ID="PostLiteral"></asp:Literal>
                                        </div>
                                        <div class="topDiv" align="right" style="padding-right: 50px;width: 660px;">
                            
                                            <div style="float: right;">
                                                <ctrl:BlueButton ID="BlueButton17" CLIENT_LINK_CLICK="ShowPostDiv()" WIDTH="60px" runat="server" BUTTON_TEXT="Post It" />
                                            </div>
                                            <div style="float: right;padding-right: 10px;">
                                                <ctrl:BlueButton ID="BlueButton18" CLIENT_LINK_CLICK="ShowPostDiv()" WIDTH="87px" runat="server" BUTTON_TEXT="Backwards" />
                                            </div>
                                         </div>
                                    </div>
                                
                                <asp:Panel runat="server" ID="FeaturedPreviewPanel" Visible="false">
                                <br />
                                    <h2>Your bulletin as it will appear throughout the site 
                                    and in user's emails from saved searches.</h2>
                                    
                                    <asp:Literal runat="server" ID="FeaturedPreviewLiteral"></asp:Literal>
                                </asp:Panel>
                                <br />
                                    <h2>Your bulletin as it will appear on it's bulletin page.</h2>
                                    <div style="border:  solid 1px #dedbdb; width: 658px; margin-top: 20px; padding: 5px;">
                                        <asp:Panel runat="server" ID="EventPanel" Visible="false">
                                            <div style="padding-top: 5px;" class="topDiv">
                                            <div style="width: 570px; clear: both;">
                                                <h1><asp:Label runat="server" ID="ShowHeaderName"></asp:Label></h1>
                                                
                                            </div>
                                            <div style="width: 570px;" >
                                            
                                            
                                                <asp:Literal runat="server" ID="ShowVideoPictureLiteral">
                                                </asp:Literal>
                                                <div>
                                                    <div style="float: right; width: 430px;padding-left: 5px;padding-right: 10px;">
                                                    <asp:Panel runat="server" ID="RotatorPanel" Visible="false">
                                                        <rad:RadRotator runat="server" Skin="Vista" ScrollDuration="200" ID="Rotator1" 
                                                        ItemHeight="250" ItemWidth="412"  
                                                        Width="440px" Height="250px" RotatorType="Buttons">
                                                            
                                                        </rad:RadRotator>
                                                    </asp:Panel>
                                                    
                                                    
                                                    </div>
                                                    <asp:Label runat="server" ID="ShowVenueName" CssClass="Green12LinkNF"></asp:Label><br />
                                                    <asp:Label runat="server" ID="ShowDateAndTimeLabel" CssClass="DateTimeLabel"></asp:Label><br />
                                                     <asp:Label runat="server" ID="ShowRestOfDescription" CssClass="Text12"></asp:Label>
                                                
                                                
                                                    <asp:Label runat="server" ID="ShowDescription" CssClass="EventBody"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        
                                            
                                        </asp:Panel>
                                    </div>
                               
                                    </div>
                                    </div> 
                                </asp:Panel>
                             </rad:RadPageView>

                        </rad:RadMultiPage>
                        </div>
                    </div>
                </div>
                <div style="float: right;">
                    <asp:Panel runat="server" ID="MessagePanel" Visible="false">
                        <div align="center" style="width: 135px; border: solid 3px red; margin-right: -5px; 
                        padding: 5px; color: White; font-family: Arial;margin-top: 100px;">
                            <h2><span style="color: Red;">Attention!</span></h2> <br />
                            <asp:Label CssClass="Text12" runat="server" ID="YourMessagesLabel"></asp:Label>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </asp:Panel>
    </div>
    </asp:Panel>
    

<%--</rad:RadAjaxPanel>--%>
</asp:Content>

