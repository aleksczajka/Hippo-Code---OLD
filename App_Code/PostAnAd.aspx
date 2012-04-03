<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ValidateRequest="false" 
AutoEventWireup="true" 
CodeFile="PostAnAd.aspx.cs" Inherits="PostAnAd" Title="Post local ad | HippoHappenings" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register TagName="HippoTextBox" TagPrefix="ctrl" Src="~/Controls/HippoTextBox.ascx" %>
<%@ Register Src="~/Controls/Ads.ascx" TagName="Ads" TagPrefix="ctrl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<%--<rad:RadAjaxPanel runat="server">--%>
 <rad:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="false" 
                KeepInScreenBounds="true" VisibleOnPageLoad="false"  Behaviors="none"
            ID="MessageRadWindowManager" ReloadOnShow="true" Modal="true" DestroyOnClose="true" RestrictionZoneID="RestrictionZone"
            runat="server">
                <Windows>
                    <rad:RadWindow Width="300" ClientCallBackFunction="OnClientClose" EnableEmbeddedSkins="true" 
                    VisibleStatusbar="false" Skin="Black" Height="300" ID="MessageRadWindow" Title="Alert" runat="server">
                    </rad:RadWindow>
                    <%--<rad:RadWindow Width="500" EnableEmbeddedSkins="true" 
                    VisibleStatusbar="false" NavigateUrl="MeaslesChart.aspx" Skin="Black" Height="400" ID="RadWindow1" Title="Alert" runat="server">
                    </rad:RadWindow>--%>
                </Windows>
            </rad:RadWindowManager>
            <script type="text/javascript">
            function OnClientClose(oWnd, args)
            {

                window.location = args;
            }
           
            </script>
            
            <%-- Style and scirpt from : http://mattberseth.com/blog/2007/08/disable_updatepanel_contents_d.html--%>
            <style type="text/css">
                .updateProgress {
                    
                    position:absolute;
                    height: 500px;
                    background-color:#1FB6E7; 
                    opacity: .1;
                    filter: alpha(opacity=10);
                    width: 600px;
                    z-index: 10000;
                }
                .insideUpdateProgress
                {
                    position: absolute;
                    height: 500px;
                    width: 600px;
                    margin-top:13px; 
                    opacity: 1 !important;
                    filter: alpha(opacity=100) !important;
                    z-index: 20000;
                }
                .updateProgressMessage {
                    margin:3px; 
                    font-family:Trebuchet MS; 
                    font-size:small; 
                    color: #ffffff;
                    font-weight: bold;
                    vertical-align: middle;
                    opacity: 1 !important;
                    filter: alpha(opacity=100) !important;
                }
                .background {
                    background-color:#1FB6E7; 
                    filter:alpha(opacity=10); 
                    opacity:0.1; 
                }        
            </style>
               <script type="text/javascript" language="javascript">
        
                                                var _updateProgressDiv;
                                                var _backgroundDiv;
                                                var _gridView;
                                               var isEnabled = true;
                                               function DisablePanel()
                                               {
                                                    isEnabled = false;
                                               }
                                               
                                                function pageLoad(sender, args){ 
                                                if(isEnabled){   
                                                    //  register for our events
                                                    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginRequest);
                                                    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);    
                                                    
                                                    //  get the updateprogressdiv
                                                    _updateProgressDiv = $get('updateProgressDiv');
                                                    _updateProgressDiv2 = $get('insideUpdateProgressDiv');
                                                    
                                                    //  fetch the gridview
                                                    _gridView = $get('<%= this.Panel7.ClientID %>');
                                                    //  create the div that we will position over the gridview
                                                    //  during postbacks
//                                                    _backgroundDiv = document.createElement('div');
//                                                    _backgroundDiv.style.display = 'none';
//                                                    _backgroundDiv.style.zIndex = 10000;
//                                                    _backgroundDiv.className = 'background';
//                                                    
//                                                    //  add the element to the DOM
//                                                    _gridView.parentNode.appendChild(_backgroundDiv);
                                                    }
                                                }        
                                                
                                                function beginRequest(sender, args){
                                                    // make it visible
                                                    _updateProgressDiv.style.display = '';	   
                                                    _updateProgressDiv2.style.display = '';	      
                                                    //_backgroundDiv.style.display = '';
                                                    
                                                    // get the bounds of both the gridview and the progress div
                                                    var gridViewBounds = Sys.UI.DomElement.getBounds(_gridView);
                                                    var updateProgressDivBounds = Sys.UI.DomElement.getBounds(_updateProgressDiv);
                                                               
                                                    //  center of gridview
                                                    var x = gridViewBounds.x + Math.round(gridViewBounds.width / 2) - Math.round(updateProgressDivBounds.width / 2);
                                                    var y = gridViewBounds.y + Math.round(gridViewBounds.height / 2) - Math.round(updateProgressDivBounds.height / 2);	    
                                                    
                                                    //  set the dimensions of the background div to the same as the gridview
//                                                    _backgroundDiv.style.width = gridViewBounds.width + 'px';
//                                                    _backgroundDiv.style.height = '500px';              

                                                    //	set the progress element to this position
                                                    Sys.UI.DomElement.setLocation(_updateProgressDiv, x, y);     
                                                    //  place the div over the gridview
                                                    //Sys.UI.DomElement.setLocation(_backgroundDiv, gridViewBounds.x, gridViewBounds.y);           
                                                }

                                                function endRequest(sender, args) {
                                                    // make it invisible
                                                    _updateProgressDiv.style.display = 'none';
                                                    _updateProgressDiv2.style.display = 'none';
                                                    //_backgroundDiv.style.display = 'none';
                                                }
                                                
                                                </script> 
                                                <asp:Label runat="server" ID="ErrorLabel" ForeColor="red"></asp:Label>
<div style="padding-bottom: 100px;">
<%--    <asp:Button runat="server" ID="DifferencesButton" OnClick="OpenMeasles"  Text="Differences between Free and Featured Classifieds" />
--%>    <asp:Panel runat="server" ID="LogInPanel" Visible="false">
    <div class="topDiv">
        <div style="float: left; width: 430px;">
            <table cellpadding="0" cellspacing="0" width="390px" style="padding-bottom: 0; margin: 0;">
    <tr>
        <td width="200px">
                <div style="font-family: Arial; font-size: 30px; color: #666666;"> 
              
            <asp:Label runat="server" ID="nameLabel" Text="Post An Ad "></asp:Label> 
            <asp:Label runat="server" ID="UserNameLabel" Visible="false"></asp:Label>
        <asp:Label runat="server" ID="isEdit" Visible="false"></asp:Label>
        <asp:Label runat="server" ID="adID" Visible="false"></asp:Label>
            </div>
        </td>
        <td>
           <%-- <div style="padding-left: 20px;">
                <asp:Panel runat="server" ID="InfoPanelFreeLabel" CssClass="Paddings" Width="390px" BackColor="#333333" Visible="false">
                 <asp:Label runat="server" CssClass="AddWhiteLink" ForeColor="#cccccc" Text=" You are posting a free ad. " ID="InfoLabel"></asp:Label>
                 <asp:LinkButton ID="LinkButton3" runat="server" OnClick="SelectFeaturedPanel" Text="Click here" CssClass="AddLink"></asp:LinkButton>
                 <asp:Label ID="Label1" runat="server" Text=" to change it to a featured ad." ForeColor="#cccccc"  CssClass="AddWhiteLink"></asp:Label>
                    
                </asp:Panel>
                <asp:Panel runat="server" ID="InfoPanelFeaturedLabel" CssClass="Paddings" Width="390px" BackColor="#333333" Visible="false">
                    <asp:Label runat="server" Text=" You are posting a featured ad. " ForeColor="#cccccc"  CssClass="AddWhiteLink" ID="Label2"></asp:Label>
                    <asp:LinkButton ID="LinkButton2" runat="server" OnClick="SelectFreePanel" Text="Click here" CssClass="AddLink"></asp:LinkButton>
                    <asp:Label ID="Label3" runat="server" Text=" to change it to a free ad. " ForeColor="#cccccc"  CssClass="AddWhiteLink"></asp:Label>
                </asp:Panel>
            </div>--%>
        </td>
        <td>
        
        </td>
    </tr>
</table>
               <%-- <div class="AddLinkGoing" style="float: left;padding-left: 20px; padding-top: 30px;">
                   <label>You must be <asp:HyperLink ID="HyperLink1" runat="server" CssClass="AddLink" NavigateUrl="~/login" Text="logged in"></asp:HyperLink> to submit a new venue! 
                    <br />
                    Please <asp:HyperLink ID="HyperLink2" runat="server" CssClass="AddLink" NavigateUrl="~/Register.aspx" Text="register"></asp:HyperLink> if you do not have an account.
                   </label>
                </div>--%>
                <br />
                <span style="font-family: Arial; font-size: 12px; color: #cccccc; line-height: 20px;">
            <asp:Label runat="server" ID="WelcomeLabel"></asp:Label>
          </span>
        </div>
        <div style="float: right;">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <ctrl:Ads ID="Ads1" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
        </div>

        <%--<div style="float: left; padding-left: 5px; width: 419px;">
            <div style="clear: both;">
                 <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" 
                    codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0" 
                    width="431" height="575" title="banner" wmode="transparent">  
                    <param name="movie" value="gallery.swf" />  
                    <param name="quality" value="high" />
                    <param name="wmode" value="transparent"/>   
                        <embed src="gallery.swf" quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer" 
                        type="application/x-shockwave-flash" width="431" height="575" wmode="transparent">
                        </embed>
                    </object>
            </div>
           
        </div>--%>
    </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="LoggedInPanel" Visible="false">
    <asp:Panel runat="server" ID="IntroPanel">
     <div class="EventDiv" style="width: 300px; float: right;">
           
             <label>
             <b>Disclaimer:</b> This ad space is strictly designated for users of the HippoHappenings site, 
             for peers by peers. Large corporations are not welcome to post an ad unless it is an advertisement 
             for a unique happening in the ad's specified area. Contact us 
             <a class="AddLink" href="mailto:Corporate@HippoHappenings.com">here</a> if you are a corporation seeking 
             to post on this site. To learn more about ad posting, read the full <a class="AddLink" 
             href="terms-and-conditions">terms of use</a> and <a class="AddLink" href="about#tag16">about page</a>.
             </label>
         </div>
        <div style="font-family: Arial; font-size: 30px; color: #666666; padding-bottom: 10px;">
            <asp:Label runat="server" Visible="false" ID="isFeatured"></asp:Label>
            <span style="font-family: Arial; font-size: 14px; color: White;">What are you looking to do?</span>
            <div style="padding-top: 20px; padding-left: 5px; margin-left: 30px;font-family: Arial; font-size: 12px; color: White;background: url('image/FreeBackground.png'); width: 700px; height: 110px; background-repeat: no-repeat; margin-top: 10px;">
                <div style="width: 400px;"><span style="color: White; font-weight: bold;">Want to post a <asp:LinkButton runat="server" ID="LinkButton0" CssClass="AddLink" Text="free" OnClick="SelectFreePanel"></asp:LinkButton> Ad? </span><span style="color: #cccccc;">It’s simple, and... it’s free!! Just fill out the form and                     your ad will be searchable from the ‘Ads/Clasiffieds’ link.</span><br /><br />
 <asp:Button runat="server" ID="Button3" Text="Post A Free Ad" CssClass="SearchButton" 
            OnClick="SelectFreePanel" onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" />
<%--<asp:ImageButton runat="server" ID="HomeLink" ImageUrl="~/image/PostAnAdButton.png" 
onmouseout="this.src='image/PostAnAdButton.png'" OnClick="SelectFreePanel" 
onmouseover="this.src='image/PostAnAdButtonSelected.png'"   />--%>
                    </div>
                    
            </div>
            <asp:Panel runat="server" ID="FreeFeaturedAdsPanel" Visible="false">                    <img src="image/TheStar5.png" width="50px" height="50px" /><span style="color: White; font-weight: bold;">Featured ads are now free!</span>                </asp:Panel>
            <div style="padding-top: 3px; padding-left: 5px; margin-left: 30px;font-family: Arial; font-size: 12px; color: White;background: url('image/FeaturedBackground.png'); width: 700px; height: 110px; background-repeat: no-repeat;">
                                <div style="width: 450px;"><span style="color: White; font-weight: bold;">Want to post a <asp:LinkButton ID="LinkButton1" runat="server" CssClass="AddLink" Text="featured" OnClick="SelectFeaturedPanel"></asp:LinkButton> Ad? </span> <span style="color: #cccccc;">Feature your ad and have it be displayed    throughout the site! The cost is $0.01 per user in normal ad space and $0.04 per user in big ad space. Each ad is show to a user for 20 seconds, repeating every 20 minutes for an entire day. In addition, only with this option, users who prefer to get new ads straight to their email address will be sent these featured ads. And also, only with featured ads you get Ad Statistics! for each one of your ads.</span><br /><br />
                <asp:ImageButton runat="server" ID="ImageButton2" ImageUrl="~/image/FeaturedAdButton.png"  onmouseout="this.src='image/FeaturedAdButton.png'" OnClick="SelectFeaturedPanel" onmouseover="this.src='image/FeaturedAdButtonSelected.png'"   />
                </div>
            </div>
        </div>
    </asp:Panel>
        <asp:Panel runat="server" ID="TabsPanel" Visible="false">
            <div id="topDiv6">
                <div>
                    <div style=" float: left; width: 683px;">
                        <div style="padding-left: 11px;">
                            <rad:RadTabStrip runat="server" OnTabClick="TabClick" ID="AdTabStrip" Height="40px"
                            Skin="HippoHappenings" CssClass="EventTabStrip"
                            EnableEmbeddedSkins="false" MultiPageID="AdPostPages" SelectedIndex="0">
                                <Tabs>
                                    <rad:RadTab runat="server" PageViewID="TabOne" 
                                    ImageUrl="images/DetailsOrangeFull.gif"
                                    HoveredImageUrl="images/DetailsOrangeFull.gif" TabIndex="0" 
                                    DisabledImageUrl="images/DetailsOrangeFull.gif" 
                                    SelectedImageUrl="images/DetailsOrangeFull.gif">
                                    </rad:RadTab>
                                    <rad:RadTab runat="server"  PageViewID="TabTwo" Enabled="false"  
                                    ImageUrl="images/MediaOrangeFull.gif" 
                                    HoveredImageUrl="images/MediaOrangeFull.gif"  TabIndex="1" 
                                    DisabledImageUrl="images/MediaOrangeEmpty.gif" 
                                    SelectedImageUrl="images/MediaOrangeFull.gif" ></rad:RadTab>
                                    <rad:RadTab runat="server"  PageViewID="TabThree" Enabled="false"  
                                    ImageUrl="images/AdMediaOrangeFull.gif" 
                                    HoveredImageUrl="images/AdMediaOrangeFull.gif"  TabIndex="2" 
                                    DisabledImageUrl="images/AdMediaOrangeEmpty.gif" 
                                    SelectedImageUrl="images/AdMediaOrangeFull.gif" ></rad:RadTab>
                                    <rad:RadTab runat="server"  PageViewID="TabFour" Enabled="false"  
                                    ImageUrl="images/CategoriesOrangeFull.gif" 
                                    HoveredImageUrl="images/CategoriesOrangeFull.gif"  TabIndex="3" 
                                    DisabledImageUrl="images/CategoriesOrangeEmpty.gif" 
                                    SelectedImageUrl="images/CategoriesOrangeFull.gif" ></rad:RadTab>
                                    <rad:RadTab runat="server" PageViewID="TabPostingDetails" Enabled="false"  
                                    ImageUrl="images/PostingDetailsOrangeFull.gif" 
                                    HoveredImageUrl="images/PostingDetailsOrangeFull.gif" TabIndex="4" 
                                    DisabledImageUrl="images/PostingDetailsOrangeEmpty.gif" 
                                    SelectedImageUrl="images/PostingDetailsOrangeFull.gif" ></rad:RadTab>
                                    <rad:RadTab runat="server"  PageViewID="TabFive" Enabled="false"  
                                    ImageUrl="images/PaymentInfoOrangeFullOver.gif" 
                                    HoveredImageUrl="images/PaymentInfoOrangeFullOver.gif"  TabIndex="5" 
                                    DisabledImageUrl="images/PaymentInfoOrangeEmptyOver.gif" 
                                    SelectedImageUrl="images/PaymentInfoOrangeFullOver.gif" ></rad:RadTab>
                                    <rad:RadTab runat="server"  PageViewID="TabSix" Enabled="false"  
                                    ImageUrl="images/PostingOrangeFull.gif" 
                                    HoveredImageUrl="images/PostingOrangeFull.gif" 
                                    DisabledImageUrl="images/PostingOrangeEmpty.gif"  TabIndex="6" 
                                    SelectedImageUrl="images/PostingOrangeFull.gif" ></rad:RadTab>
                                </Tabs>
                            </rad:RadTabStrip>
                        </div>
                        
                        <div>
                        <rad:RadMultiPage runat="server" ID="AdPostPages" SelectedIndex="0">
                            <rad:RadPageView runat="server" ID="TabOne" TabIndex="0" >
                                <asp:Panel runat="server" ID="Panel1" DefaultButton="DetailsOnwardsButton">
                                    <div style="background: url('image/TabsBackgroundTop.png'); background-repeat: no-repeat; width: 668px; height: 4px;">
                                    </div>
                                    <div id="topDiv3" style="background: url('image/TabsBackground.png'); background-repeat: repeat-y; width: 668px; padding-top: 10px;">
                                        <div class="EventDiv" style="padding-left: 30px; float: left; padding-bottom: 10px; margin-right: 50px;">
                                            <div id="topDiv">
                                            <h1>Details</h1>
                                                <div style="float: left; width: 330px;">
                                                <label>All HTML tags will be removed except for links and line breaks. However, the header cannot contain links at all.</label>
                                                <br /><br />
                                                    <label><span style='color: #1fb6e7; font-weight: bold;'>* </span>Your Headline</label><br />
                                                    <span style="font-family: Arial; font-size: 10px; font-weight: bold; color: #ff770d;">max 70 characters</span>
                                                    <ctrl:HippoTextBox runat="server" ID="AdNameTextBox" TEXTBOX_WIDTH="300" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                                                   <br /><br /><br />
                                                    <label><span style='color: #1fb6e7; font-weight: bold;'>* </span>Your Description</label>&nbsp;&nbsp;<asp:Image CssClass="HelpImage" runat="server"  ID="Image3" ImageUrl="~/image/helpIcon.png"></asp:Image>
                                                    <div class="topDiv">
                                                        <div style="float: left;">
                                                         <rad:RadToolTip ID="RadToolTip3" runat="server" Skin="Black"  Width="300px" ManualClose="true" 
                                                         ShowEvent="onclick" Position="MiddleRight" RelativeTo="Element" TargetControlID="Image3">
                                                            <label>
                                                            This text is what people will read when they visit your ad page, so be as descriptive as you need to be.
                                                               </label>
                                                            </rad:RadToolTip>
                                                                <br />
                                                                <span style="font-family: Arial; font-size: 10px; font-weight: bold; color: #ff770d;">max 1000 characters</span>
                                                             </div>
                                                                <div style="padding-left: 95px; float: left;">
                                                                    <asp:Button runat="server" ID="Button2" Text="Insert break" CssClass="SearchButton" OnClick="InsertBreak"
                                                                        onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                                        onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" />
                                                                </div>
                                                    </div>
                                                   
                                                    <asp:TextBox  runat="server" Height="100px" TextMode="MultiLine" ID="DescriptionTextBox"  Width="300px"></asp:TextBox>
                                                    <%--<asp:RegularExpressionValidator runat="server" ControlToValidate="DescriptionTextBox" 
                                                    ErrorMessage="Description field cannot have any HTML tags asside from links." 
                                                    ValidationExpression="<\/*?(?![^>]*?\b(?:a|img)\b)[^>]*?>"></asp:RegularExpressionValidator>--%><br /><br /><br />
                                                    <label><span style='color: #ff770d; font-weight: bold;'><span style='color: #1fb6e7; font-weight: bold;'>* </span>reqiured fields</span></label>
                                                </div>
                                                <div style="float: right; width: 200px;padding-left: 30px;">
                                               
                                                <asp:Panel runat="server" ID="FeaturedStartDatePanel">
                                                    <asp:Label runat="server" ID="DateCannotEditLabel" CssClass="AddGreenLink"></asp:Label>
                                                    <label><span style='color: #1fb6e7; font-weight: bold;'>* </span>Start Date & Time</label>
                                                    <asp:Image CssClass="HelpImage" runat="server"  ID="Image2" ImageUrl="~/image/helpIcon.png"></asp:Image>
                                             <rad:RadToolTip ID="RadToolTip8" runat="server" Skin="Black"  Width="300px" ManualClose="true" 
                                             ShowEvent="onclick" Position="MiddleRight" RelativeTo="Element" TargetControlID="Image2">
                                                <label>
                                                    Even though you can select the date on which your ad will
                                                    start posting throughout the site, there are users who will see your ad through email automatically after it is posted. Here's why: <br />Users have the ability to save ad searches and make them live. Making them live means they will receive an email with
                                                    ads matching the saved search criteria. Users also have the ability to configure how many ads the email will have
                                                    before the email is sent. Since there is a certain amount of ads needed before the email is sent out, and, since we
                                                    have no way of knowing how many ads will be posted in the future matching the same criteria, we cannot provide semi-accurate
                                                    estimate of how many users will be available to receive your ad as an email in the future. And so, we show in further tabs only the accurate
                                                    estimate of how many users will automatically see your ad in an email <b>Today.</b>.
                                                </label>
                                                </rad:RadToolTip>
                                                </asp:Panel>
                                                <asp:Panel runat="server" ID="NormalStartDatePanel">
                                                    <label><span style='color: #1fb6e7; font-weight: bold;'>* </span>Start Date & Time [Your ad doesn't have to start being searchable today.]</label>
                                                </asp:Panel>
                                                    <br />
                                                    <rad:RadDateTimePicker runat="server" ID="StartDateTimePicker">
                                                        <Calendar  runat="server" AutoPostBack="false">
                                                            
                                                        </Calendar>
                                                    </rad:RadDateTimePicker><br />
                                                    <div style="height: 30px;"></div>
                                                    <asp:Panel runat="server" ID="EndDatePanel" Visible="false">
                                                        <label><span style='color: #1fb6e7; font-weight: bold;'>* </span>Number of Days to keep ad on site</label><br />
                                                        <asp:DropDownList runat="server" ID="DaysDropDown">
                                                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                            <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                            <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                                            <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                                            <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                                            <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                            <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                            <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                            <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                                            <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                                            <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                            <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                                            <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                                            <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                                            <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                            <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                                            <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                                            <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                                            <asp:ListItem Text="24" Value="24"></asp:ListItem>
                                                            <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                                            <asp:ListItem Text="26" Value="26"></asp:ListItem>
                                                            <asp:ListItem Text="27" Value="27"></asp:ListItem>
                                                            <asp:ListItem Text="28" Value="28"></asp:ListItem>
                                                            <asp:ListItem Text="29" Value="29"></asp:ListItem>
                                                            <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                            
                                                            
                                                            
                                                        </asp:DropDownList>
                                                    </asp:Panel>
                                                    <asp:Panel runat="server" ID="DaysExplanationPanel" Width="200px" Visible="false">
                                                        <label><span style='color: #1fb6e7; font-weight: bold;'>* </span>The length of time your featured ad runs will be determined later on by the number of views you request.</label>
                                                    </asp:Panel>
                                                </div>
                                            </div>
                                            <div style="width: 200px; float: left;">
                                        </div>
                                        </div>
                                     
                                        
                                             
                                       
                                        <div align="right" style="vertical-align: bottom; width: 600px;">
                                                <asp:ImageButton runat="server" ID="DetailsOnwardsButton" OnClick="Onwards" ImageUrl="image/OnwardsButton.png" 
                                                onmouseout="this.src='image/OnwardsButton.png'" onmouseover="this.src='image/OnwardsButtonSelected.png'"  />
                                        </div>
                                    </div>
                                    <div align="center" style=" margin-left: 11px ;background-color: #515151 ;height: 4px; width: 640px;"></div>
                                </asp:Panel>
                            </rad:RadPageView>
                            <rad:RadPageView runat="server" ID="TabTwo" TabIndex="1" >
                                <asp:Panel runat="server" ID="Panel2" DefaultButton="MediaOnwardsButton">
                                    <div style="background: url('image/TabsBackgroundTop.png'); background-repeat: no-repeat; width: 668px; height: 4px;">
                                    </div>
                                    <div style="background: url('image/TabsBackground.png'); background-repeat: repeat-y;width: 668px;">
                                       
                                        <div class="EventDiv" style="clear:both;">
                                        
                                        <div style="padding-left: 30px; width: 632px; padding-bottom: 30px; padding-top: 10px;">
                                        <h1>Media</h1>
                                    <asp:CheckBox ID="MusicCheckBox" runat="server" Text="Music <span style='color: #ff770d;'>[Can upload up to 3. Required format is .mp3]</span>" 
                                     />
                                        <asp:Panel runat="server" ID="MusicPanel">
                                            <div class="topDiv" style="clear: both; width: 500px;">
                                                <div style="float: left; padding-left: 30px;">
                                                    <div style="float: left; padding-top: 4px;">
                                                        <asp:FileUpload runat="server" ID="MusicUpload" Width="230px" EnableViewState="true" />
                                                    </div>
                                                    <div style="float: left;">
                                                    <asp:Button runat="server" ID="MusicUploadButton" Text="Upload!" 
                                                     CssClass="SearchButton"  PostBackUrl="post-bulletin"
                                                     OnClick="MusicUpload_Click"
                                                     onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                     onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                     onclientclick="this.value = 'Working...';" 
                                                     />
    <%--                                                    <asp:ImageButton runat="server" ID="MusicUploadButton" 
                                                         OnClick="MusicUpload_Click" PostBackUrl="post-bulletin"
                                                         ImageUrl="image/MakeItSoButton.png" 
                                                        onmouseout="this.src='image/MakeItSoButton.png'" onmouseover="this.src='image/MakeItSoButtonSelected.png'"  />
    --%>                                                </div>

                                                </div>
                                                <div style="float: right; padding-right: 70px;">
                                                    <asp:CheckBoxList CssClass="MusicCheckBoxes" ID="SongCheckList" runat="server"></asp:CheckBoxList>
                                                    <asp:ImageButton Visible="false" runat="server" OnClick="NixIt" ID="DeleteSongButton" ImageUrl="image/NixItButton.png" 
                                                    onmouseout="this.src='image/NixItButton.png'" onmouseover="this.src='image/NixItButtonSelected.png'"  />
                                                   
                                                </div>
                                                
                                            </div>
                                        </asp:Panel>
                                </div>
                                            <div style="padding-left: 30px; width: 632px; clear: both; padding-top: 10px;">
                                                <asp:CheckBox ID="MainAttractionCheck" Checked="true" Text="Include a Media Slider" runat="server" AutoPostBack="true" OnCheckedChanged="EnableMainAttractionPanel" />
                                                    <asp:Panel runat="server" ID="MainAttractionPanel"> 
                                                        <div class="topDiv" style="padding-left: 30px;">
                                                            <%--<asp:RadioButtonList runat="server" ID="MainAttractionRadioList" AutoPostBack="true" OnSelectedIndexChanged="ShowSliderOrVidPic">
                                                                <asp:ListItem Text="Main Video or Main Picture" Value="0" Selected="true"></asp:ListItem>
                                                                <asp:ListItem Text="Slider of Pictures [Can upload up to 20. Will be resized to 200px by 200px]" Value="1"></asp:ListItem>
                                                            </asp:RadioButtonList>--%>
                                                            <div class="topDiv" style="padding-left: 30px; border: solid 4px #515151; width: 500px; padding-bottom: 10px; padding-left: 30px;">
                                                                <asp:Panel runat="server" ID="PictureVideoPanel">
                                                                    <asp:RadioButtonList runat="server" ID="VideoPictureRadioList" OnSelectedIndexChanged="ShowVideoOrPicture" AutoPostBack="true">
                                                                        <asp:ListItem Text="Picture" Value="0" Selected="true"></asp:ListItem>
                                                                        <asp:ListItem Text="YouTube Video" Value="1"></asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                    <div style="padding-left: 30px;">
                                                                        <asp:Panel runat="server" ID="PicturePanel">
                                                                            <div id="Div1" class="topDiv">
                                                                                <div style="float: left; padding-top: 4px;">
                                                                                    <asp:FileUpload runat="server" ID="PictureUpload" Width="230px" EnableViewState="true" />
                                                                                </div>
                                                                                <div style="float: left;">
                                                                                <asp:Button runat="server" ID="ImageButton1" Text="Upload!" 
                                                                                     CssClass="SearchButton"  PostBackUrl="post-bulletin"
                                                                                     OnClick="PictureUpload_Click"
                                                                                     onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                                                     onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                                                     onclientclick="this.value = 'Working...';" 
                                                                                     />
                                                                                   <%-- <asp:ImageButton runat="server" ID="ImageButton1" 
                                                                                    PostBackUrl="post-bulletin" OnClick="PictureUpload_Click"
                                                                                     ImageUrl="image/MakeItSoButton.png" 
                                                                                    onmouseout="this.src='image/MakeItSoButton.png'" 
                                                                                    onmouseover="this.src='image/MakeItSoButtonSelected.png'"  />--%>
                                                                                </div>
                                                                                <%--<div style="float: right; padding-right: 70px;">
                                                                                    <asp:CheckBoxList CssClass="MusicCheckBoxes" ID="PictureCheckList" runat="server"></asp:CheckBoxList>
                                                                                    <asp:ImageButton Visible="false" runat="server" OnClick="PictureNixIt" ID="PictureNixItButton" ImageUrl="image/NixItButton.png" 
                                                                                    onmouseout="this.src='image/NixItButton.png'" onmouseover="this.src='image/NixItButtonSelected.png'"  />
                                                                                </div>--%>
                                                                            </div>
                                                                        </asp:Panel>
                                                                    </div>
                                                                    <div style="padding-left: 30px;" class="topDiv">
                                                                        <asp:Panel runat="server" ID="VideoPanel" Visible="false">
                                                                
                                                                    <div style="padding-left: 20px;" class="topDiv">
                                                                        <%--<asp:RadioButtonList runat="server" ID="VideoRadioList" AutoPostBack="true" OnSelectedIndexChanged="ShowVideo">
                                                                            <asp:ListItem Text="Upload a Video" Value="0"></asp:ListItem>
                                                                            <asp:ListItem Text="Paste a You Tube Link" Value="1"></asp:ListItem>
                                                                        </asp:RadioButtonList>--%>
                                                                        <%--<asp:Panel runat="server" ID="UploadPanel" Visible="false">
                                                                            <div id="Div2">
                                                                                <div style="float: left; padding-top: 4px;">
                                                                                    <asp:FileUpload runat="server" ID="VideoUpload" Width="230px" EnableViewState="true" />
                                                                                </div>
                                                                                <div style="float: left;">
                                                                                <asp:ImageButton runat="server" ID="ImageButton4" 
                                                                                    PostBackUrl="post-bulletin" OnClick="VideoUpload_Click"
                                                                                     ImageUrl="image/MakeItSoButton.png" 
                                                                                    onmouseout="this.src='image/MakeItSoButton.png'" onmouseover="this.src='image/MakeItSoButtonSelected.png'"  />
                                                                                </div>
                                                                            </div>
                                                                        </asp:Panel>--%>
                                                                        <asp:Panel runat="server" ID="YouTubePanel">
                                                            <label>Insert the ID of the YouTube video:</label><br />
                                                                            <div style="clear: both;">
                                                                               <label> http://www.youtube.com/watch?v=<span style="color: Red;">YMLTwq1M2pw</span>  <--This is the ID</label>
                                                                                <div style="float: left; padding-top: 4px;">
                                                                                    <asp:TextBox runat="server" ID="YouTubeTextBox"></asp:TextBox>
                                                                                </div>
                                                                                <div style="float: left;">
                                                                                <asp:Button runat="server" ID="ImageButton10" Text="Upload!" 
                                                                                     CssClass="SearchButton"  PostBackUrl="post-bulletin"
                                                                                     OnClick="YouTubeUpload_Click"
                                                                                     onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                                                     onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                                                     onclientclick="this.value = 'Working...';" 
                                                                                     />
     <%--                                                                               <asp:ImageButton runat="server" ID="ImageButton10" 
                                                                                        PostBackUrl="post-bulletin" OnClick="YouTubeUpload_Click"
                                                                                         ImageUrl="image/MakeItSoButton.png" 
                                                                                        onmouseout="this.src='image/MakeItSoButton.png'" onmouseover="this.src='image/MakeItSoButtonSelected.png'"  />
    --%>                                                                            </div>
                                                                            </div>
                                                                <label>ex: if this is the url of your YouTube video <br />http://www.youtube.com/watch?v=<span style="color: Red;">YMLTwq1M2pw</span>  <--This is the ID</label>
                                                                        </asp:Panel>
                                                                        <div style="float: right; padding-right: 70px; clear: both;">
                                                                            <asp:CheckBoxList CssClass="MusicCheckBoxes" ID="VideoCheckList" runat="server"></asp:CheckBoxList>
                                                                            <asp:ImageButton Visible="false" runat="server" OnClick="VideoNixIt" ID="VideoNixItButton" ImageUrl="image/NixItButton.png" 
                                                                            onmouseout="this.src='image/NixItButton.png'" onmouseover="this.src='image/NixItButtonSelected.png'"  />
                                                                        </div>
                                                                     </div>
                                                                </asp:Panel>
                                                                    </div>
                                                                </asp:Panel>
                                                                <asp:Panel runat="server" ID="SliderPanel" Visible="false">
                                                                    <div>
                                                                        
                                                                        <%--<div id="Div3" style=" padding-left: 30px; clear: both; padding-top: 10px;">
                                                                            <div style="float: left; padding-top: 4px;">
                                                                                <asp:FileUpload runat="server" ID="SliderFileUpload" Width="230px" EnableViewState="true" />
                                                                            </div>
                                                                            <div style="float: left;">
                                                                            <asp:ImageButton runat="server" ID="ImageButton14" 
                                                                                PostBackUrl="post-bulletin#top" OnClick="SliderUpload_Click"
                                                                                 ImageUrl="image/MakeItSoButton.png" 
                                                                                onmouseout="this.src='image/MakeItSoButton.png'" onmouseover="this.src='image/MakeItSoButtonSelected.png'"  />
                                                                            </div>
                                                                        </div>--%>
                                                                        
                                                                    </div>
                                                                </asp:Panel>
                                                            </div>
                                                        </div>
                                                        <div style="color: #cccccc;font-family: Arial;float: left; padding-right: 70px;" class="EventDiv topDiv">
                                            Your Chosen Pics and Videos <br />
                                                                        <asp:CheckBoxList CssClass="MusicCheckBoxes" ID="PictureCheckList" runat="server"></asp:CheckBoxList>
                                                                        <asp:ImageButton Visible="false" runat="server" OnClick="PictureNixIt" ID="PictureNixItButton" ImageUrl="image/NixItButton.png" 
                                                                        onmouseout="this.src='image/NixItButton.png'" onmouseover="this.src='image/NixItButtonSelected.png'"  />
                                                                    </div>
                                                    </asp:Panel>
                                            </div>
                                        </div>
                                    
                                        <div align="right" style=" padding-top: 30px; padding-right: 50px; clear:both;">
                                            <asp:ImageButton runat="server" ID="ImageButton16" OnClick="Backwards" ImageUrl="image/BackwardsButton.png" 
                                            onmouseout="this.src='image/BackwardsButton.png'" onmouseover="this.src='image/BackwardsButtonSelected.png'"  />
                                            <asp:ImageButton runat="server" ID="MediaOnwardsButton" OnClick="Onwards" ImageUrl="image/OnwardsButton.png" 
                                            onmouseout="this.src='image/OnwardsButton.png'" onmouseover="this.src='image/OnwardsButtonSelected.png'"  />
                                         </div>
                                    
                                    </div>
                                    <div align="center" style=" margin-left: 11px ;background-color: #515151 ;height: 4px; width: 640px;"></div>
                                </asp:Panel>
                            </rad:RadPageView>
                            <rad:RadPageView runat="server" ID="TabThree" TabIndex="2" >
                                <asp:Panel runat="server" ID="Panel3" DefaultButton="AdMediaOnwardsButton">
                                    <div style="background: url('image/TabsBackgroundTop.png'); background-repeat: no-repeat; width: 668px; height: 4px;">
                                    </div>
                                    <div style="background: url('image/TabsBackground.png'); background-repeat: repeat-y;width: 668px;">
                                        
                                        <div class="EventDiv">
                                            <asp:Panel  runat="server" ID="FeaturedAdMediaPanel">
                                            <div style="padding-left: 30px; width: 632px; clear: both; padding-top: 10px;">
                                                <div id="topDiv2">
                                                <h1>Ad Media</h1>
                                                    <div>
                                                        <asp:CheckBox runat="server" AutoPostBack="true" OnCheckedChanged="EnableAdMediaPanel" 
                                                        ID="BannerAdCheckBox" Text="Include a Banner Ad <span style='color: #ff770d;'>[will be sized to fit 100px by 100px]</span>" />
                                                    </div>
                                                    <asp:Panel runat="server" ID="AdMediaPanel" Visible="false" Enabled="false">
                                                        <div style="margin-left: 30px;">
                                                            <div style="float: left; padding-top: 4px;">
                                                                <asp:FileUpload runat="server"  ID="AdPictureUpload" Width="230px" EnableViewState="true" />
                                                            </div>
                                                            <div style="float: left;">
                                                            <asp:Button runat="server" ID="ImageButton5" Text="Upload!" 
                                                                 CssClass="SearchButton"  PostBackUrl="post-bulletin#top"
                                                                 OnClick="AdPictureUpload_Click"
                                                                 onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                                 onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                                 onclientclick="this.value = 'Working...';" 
                                                                 />
    <%--                                                            <asp:ImageButton runat="server" ID="ImageButton5" 
                                                                PostBackUrl="post-bulletin#top" OnClick="AdPictureUpload_Click"
                                                                 ImageUrl="image/MakeItSoButton.png" 
                                                                onmouseout="this.src='image/MakeItSoButton.png'" 
                                                                onmouseover="this.src='image/MakeItSoButtonSelected.png'"  />
    --%>                                                        </div>
                                                            <div style="float: right; padding-right: 70px;">
                                                                <asp:CheckBoxList CssClass="MusicCheckBoxes" ID="AdPictureCheckList" runat="server"></asp:CheckBoxList>
                                                                <asp:ImageButton Visible="false" runat="server" OnClick="AdPictureNixIt" ID="AdNixItButton" ImageUrl="image/NixItButton.png" 
                                                                onmouseout="this.src='image/NixItButton.png'" onmouseover="this.src='image/NixItButtonSelected.png'"  />
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                </div>
                                                
                                            </div>
                                            
                                            <div style="width: 570px;padding-left: 50px;"><br />
                                            <hr />
                                                    <label><span style='color: #1fb6e7; font-weight: bold;'>* </span>Featured Ad Summary </label>&nbsp;&nbsp;
                                                    
                                                    <asp:Image CssClass="HelpImage" runat="server"  ID="Image4" ImageUrl="~/image/helpIcon.png"></asp:Image>
                                             <rad:RadToolTip ID="RadToolTip9" runat="server" Skin="Black"  Width="300px" ManualClose="true" 
                                             ShowEvent="onclick" Position="MiddleRight" RelativeTo="Element" TargetControlID="Image4">
                                                <label>
                                                    This is the text that will be seen beneath the ad's headline on the ad that shows thoughout the site.
                                                    It needs to be short enough to fit in the ad. </label>
                                                </rad:RadToolTip>
                                                    <br />
                                                    <span style="font-family: Arial; font-size: 10px; color: #ff770d; font-weight: bold;">max 250 characters</span>
                                                    <br /><asp:TextBox runat="server" ID="SummaryTextBox" Width="200" Height="100" TextMode="multiLine" Wrap="true"></asp:TextBox>
                                                <hr />
                                                <asp:Image CssClass="HelpImage" runat="server"  ID="QuestionMark6" ImageUrl="~/image/helpIcon.png"></asp:Image>
                                                                               <rad:RadToolTip ID="RadToolTip5" runat="server" Skin="Black" Width="200px" Height="200px" ManualClose="true" ShowEvent="onclick" Position="MiddleRight" RelativeTo="Element" TargetControlID="QuestionMark6">
                                        <label>  This will determine which users see your ad. If you don't include the Zip Code, this means that users who don't have a specific zip code assigned will see your ad. </label>
                                        </rad:RadToolTip>
                                                </div>
                                               
                                                
                                                
                                             
                                         </asp:Panel>
                                        <div style="width: 570px;padding-left: 50px; padding-top: 5px;" >
    <label><span style="font-weight: bold;"> Choose the location to put your ad under.  </span></label>
                                             <table>
                                                 <tr>
                                                    <td colspan="4">
                                                        <label style="padding-right: 10px;"><span style='color: #1fb6e7; font-weight: bold;'>* </span>Country</label><br />
                                                        <asp:DropDownList OnSelectedIndexChanged="ChangeState" AutoPostBack="true" 
                                                        runat="server"  ID="CountryDropDown"></asp:DropDownList>
                                                        <asp:SqlDataSource runat="server" ID="SqlDataSourceCountry" SelectCommand="SELECT * FROM Countries"
                                                        ConnectionString="<%$ ConnectionStrings:Connection %>"></asp:SqlDataSource>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td >
                                                        <label><span style='color: #1fb6e7; font-weight: bold;'>* </span>State</label>
                                                        <asp:Panel runat="server" ID="StateTextBoxPanel">
                                                            <ctrl:HippoTextBox ID="StateTextBox" runat="server" TEXTBOX_WIDTH="100" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                                                        </asp:Panel>
                                                        <asp:Panel runat="server" ID="StateDropDownPanel" Visible="false">
                                                            <asp:DropDownList runat="server" ID="StateDropDown"></asp:DropDownList>
                                                        </asp:Panel>
                                                    </td>
                                                    <td>
                                                        <label><span style='color: #1fb6e7; font-weight: bold;'>* </span>City</label><br />
                                                            <asp:TextBox ID="CityTextBox" runat="server" Width="100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <label>Zip <span style='color: #ff770d;'>[Not Required]</span></label><br />
                                                            <asp:TextBox ID="ZipTextBox" runat="server" Width="100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    <asp:Panel runat="server" ID="RadiusPanel">
                                                    <label>Radius <span style='color: #ff770d;'>[Not Required]</span></label><br />
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
                                                
                                           
                                           
                                        </div>
                                        
                                        <div align="right" style=" padding-top: 30px; padding-right: 50px;">
                                            <asp:ImageButton runat="server" ID="ImageButton3" OnClick="Backwards" ImageUrl="image/BackwardsButton.png" 
                                            onmouseout="this.src='image/BackwardsButton.png'" onmouseover="this.src='image/BackwardsButtonSelected.png'"  />
                                            <asp:ImageButton runat="server" ID="AdMediaOnwardsButton" OnClick="Onwards" ImageUrl="image/OnwardsButton.png" 
                                            onmouseout="this.src='image/OnwardsButton.png'" onmouseover="this.src='image/OnwardsButtonSelected.png'"  />
                                         </div>
                                    
                                    </div>
                                    <div align="center" style=" margin-left: 11px ;background-color: #515151 ;height: 4px; width: 640px;"></div>
                                </asp:Panel>
                            </rad:RadPageView>
                            <rad:RadPageView runat="server" ID="TabFour" TabIndex="3" >
                                
                                <asp:Panel runat="server" ID="Panel4" DefaultButton="CategoryOnwardButton">          
                                    <div style="background: url('image/TabsBackgroundTop.png'); background-repeat: no-repeat; width: 668px; height: 4px;">
                                    </div>
                                    <div class="EventDiv" style="background: url('image/TabsBackground.png'); background-repeat: repeat-y;  width: 668px;">
                                        <div style="padding-left: 30px; width: 600px; padding-top: 10px;">
                                            <h1>Categories</h1>
                                            <label><span style="font-weight: bold;"><span style='color: #1fb6e7; font-weight: bold;'>* </span>Choose Categories to put your classfied ad under. </span><asp:Image CssClass="HelpImage" runat="server"  ID="QuestionMark5" ImageUrl="~/image/helpIcon.png"></asp:Image></label>
                                            <br /><br />
                                              <rad:RadToolTip ID="RadToolTip4" runat="server" Skin="Black" Width="200px" Height="200px" ManualClose="true" ShowEvent="onclick" Position="MiddleRight" RelativeTo="Element" TargetControlID="QuestionMark5">
                                        <label> These will help your viewers find your ad faster. For a featured ad, your ad will first be shown to users in the categories you choose. <br /> <b>Note:</b> the number of users in each category is only based on the location you have specified in the previous tab.</label>
                                        </rad:RadToolTip>
                                        <table>
                                            <tr>
                                                <td valign="top">
                                                        <%--<asp:CheckBoxList runat="server" ID="CategoriesCheckBoxes" 
                                                        RepeatColumns="4" RepeatDirection="horizontal">
                                                        
                                                        </asp:CheckBoxList>--%>
                                                        <div class="FloatLeft" style="width: 150px;">
                                                            <rad:RadTreeView ForeColor="#cccccc" Width="150px" runat="server"  
                                                            ID="CategoryTree" DataFieldID="ID" DataSourceID="SqlDataSource1" 
                                                            DataFieldParentID="ParentID"
                                                            CheckBoxes="true">
                                                            <DataBindings>
                                                                 <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                                 TextField="Name" Expanded="false" ValueField="ID" />
                                                             </DataBindings>
                                                            </rad:RadTreeView>
                                                            <asp:SqlDataSource runat="server" ID="SqlDataSource1" 
                                                                SelectCommand="SELECT * FROM AdCategories WHERE ID <= 99 ORDER BY Name ASC"
                                                                ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                            </asp:SqlDataSource>
                                                        </div>
                                                        
                                                   
                                                </td>
                                                <td valign="top">
                                                    <%--<asp:CheckBoxList runat="server" ID="CheckBoxList1" 
                                                        RepeatColumns="4" RepeatDirection="horizontal">
                                                        
                                                        </asp:CheckBoxList>--%>
                                                        <div class="FloatLeft" style="width: 150px;">
                                                            <rad:RadTreeView ForeColor="#cccccc"  Width="150px" runat="server"  
                                                            ID="RadTreeView2" DataFieldID="ID" DataSourceID="SqlDataSource3" 
                                                            DataFieldParentID="ParentID"
                                                            CheckBoxes="true">
                                                            <DataBindings>
                                                                 <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                                 TextField="Name" Expanded="false" ValueField="ID"  />
                                                             </DataBindings>
                                                            </rad:RadTreeView>
                                                            <asp:SqlDataSource runat="server" ID="SqlDataSource3" 
                                                                SelectCommand="SELECT * FROM AdCategories WHERE ID > 99 ORDER BY Name ASC"
                                                                ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                            </asp:SqlDataSource>
                                                        </div>
                                                </td>
                                                <td valign="top">
                                                    <div style=" width: 150px;">
                                                        <rad:RadTreeView Width="150px"  runat="server"  
                                                        ID="RadTreeView1" ForeColor="#cccccc"  DataFieldID="ID" DataSourceID="SqlDataSource2" 
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
                                                </td>
                                                <td valign="top">
                                                    <div style=" width: 150px;">
                                                        <rad:RadTreeView Width="150px"  runat="server"  
                                                        ID="RadTreeView3" ForeColor="#cccccc"  DataFieldID="ID" DataSourceID="SqlDataSource4" 
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
                                                </td>
                                            </tr>
                                        </table>
                                        
    <%--                                        <asp:CheckBoxList runat="server" ID="CategoriesCheckBoxes" RepeatColumns="4" RepeatDirection="horizontal"></asp:CheckBoxList>
    --%>                                    </div>
                                        <div style="padding-left: 30px; width: 550px; clear: both;" class="topDiv">
                                            <div style="float: left; padding-top: 10px;">
                                                <label>Don't see a category you want? Suggest one!</label>
                                                <asp:TextBox runat="server" ID="CategoriesTextBox"></asp:TextBox>
                                            </div>
                                            
                                            <div style="padding-top: 5px;float: right;">
                                                <asp:ImageButton runat="server" ID="ImageButton11" 
                                                    OnClick="SuggestCategoryClick"
                                                                             ImageUrl="image/MakeItSoButton.png" 
                                                                            onmouseout="this.src='image/MakeItSoButton.png'" onmouseover="this.src='image/MakeItSoButtonSelected.png'"  />
                                            </div>
                                        </div>
                                         
                                         <asp:Panel runat="server" ID="CatDisplayPanel" Visible="false">
                                         <div style="padding-left: 30px; width: 600px; padding-top: 10px;">
                                         <hr />
                                        <label>Do you want your ad to display to non-selected categories? <asp:Image CssClass="HelpImage" runat="server"  ID="QuestionMark3" ImageUrl="~/image/helpIcon.png"></asp:Image></label>
                                            <asp:RadioButtonList runat="server" ID="DisplayCheckList">
                                                <asp:ListItem Selected="true" Text="Display to non-categories" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Display only to selected categories" Value="2"></asp:ListItem>
                                            </asp:RadioButtonList>
                                            <rad:RadToolTip ID="RadToolTip1" runat="server" Skin="Black" Width="300px" Height="200px" ManualClose="true" ShowEvent="onclick" Position="MiddleRight" RelativeTo="Element" TargetControlID="QuestionMark3">
                                            <label>Your ad will always be first shown to people in the categories you specify. However, you can also choose to display the ad in categories you have not selected. This means your ad will be shown to people who have not signed in, haven't chosen their favorite categories, or chose categories that simply do not have many ads assigned to them.
                                            <br />We have added this choice for ads in categories that not many people select as their favorite. Utlimately, you could be waiting a long time for your ad to be show to the number of users you specify. 
                                            <br />Keep in mind that your add will always be shown only in the location you selected. </label>
                                            </rad:RadToolTip>
                                            <hr />
                                            </div>
                                         </asp:Panel>
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
                            <rad:RadPageView runat="server" ID="TabPostingDetails" TabIndex="4" >
                                
                                <asp:Panel runat="server" ID="Panel6" DefaultButton="CategoryOnwardButton">          
                                    <div style="background: url('image/TabsBackgroundTop.png'); background-repeat: no-repeat; width: 668px; height: 4px;">
                                    </div>
                                    <div class="EventDiv" style="background: url('image/TabsBackground.png'); background-repeat: repeat-y;  width: 668px;">
                                        <div style="padding-left: 30px; width: 600px; padding-top: 10px;">
                                      
                                      <asp:UpdatePanel runat="server" ID="topAdUpdatePanel">
                                      <Triggers>
                                            <asp:AsyncPostBackTrigger EventName="Click" ControlID="CalculateButton" />
                                            <asp:AsyncPostBackTrigger EventName="SelectedIndexChanged" ControlID="AdPlacementList" />
                                        </Triggers>
                                        <ContentTemplate>
                                            
                                         <asp:Panel ID="Panel7" runat="server" >

                                            <h1>Posting Details</h1>
                                            
                                       <div>
                                            
                                            <div class="topDiv" style="clear: both;">
                                            <div style="float: left; padding-right: 20px;"><label>
                                            <h1 class="Green">Select your Ad Placement</h1></label>
                                            </div>
                                            <div style="float: left;">
                                            <asp:Image CssClass="HelpImage" runat="server"  ID="QuestionMark8" ImageUrl="~/image/helpIcon.png"></asp:Image>     
                                            <rad:RadToolTip ID="RadToolTip6" runat="server" Skin="Black" Width="350px" 
                                            Height="200px" ManualClose="true" ShowEvent="onclick" Position="MiddleRight" 
                                            RelativeTo="Element" TargetControlID="QuestionMark8">
                                            
                                            <label>
                                                There are 2 different places for the ad to exist: <br /><b>The Big Ad space</b>
                                                , which resides on the user page and on the calendar page (the highest traffic area places which also have the longest time of stay for each single user), and the <b>Normal Ad spaces,</b> reside on more pages and number in groups of fours. 
                                                <br /><br />The Big Ad costs $0.04/user since 
                                                it is bigger, each ad on it stays for 20 seconds repeating every 10min to each user for a full 24 hours. Each Big Ad is placed by itslef without the distraction of other ads around it. 
                                                The Normal 
                                                Ad space, is smaller than the Big Ad space. The normal ads stay on for 20sec repeating every 10min to each user and are surounded by 3 other ads of same size.
                                            </label>
                                        </rad:RadToolTip>
                                            </div>
                                            </div><br />
                                        <asp:Label runat="server" ID="NotEditLabel" CssClass="AddGreenLink"></asp:Label>
                                            <asp:RadioButtonList runat="server" ID="AdPlacementList" AutoPostBack="true" 
                                            OnSelectedIndexChanged="ChangePrice">
                                                <asp:ListItem Value="0.01" Selected="true" Text="Normal Ad  <span style='color: #ff770d;'>[$0.01/user]</span>"></asp:ListItem>
                                                <asp:ListItem Value="0.04" Text="Big Ad  <span style='color: #ff770d;'>[$0.04/user]</span>"></asp:ListItem>
                                            </asp:RadioButtonList>
                                       </div>
                                       <%--<hr />
                                        <label>Here is a break down of <strong>Projected Available Users on your Start Date</strong>. These are the users that will see your ad broken down by 1. users who will see it because they are in your chosen location and have chosen the same categories, and 2. Users who will see it only because they are in your chosen location and have no other ads to see on that day.
                                        We recommend you change the date your ad begins or the categories it is under if there is little or no users to see your ad with your current preferences.</label>
                                        <asp:Image CssClass="HelpImage" runat="server"  ID="QuestionMark4" ImageUrl="~/image/helpIcon.png"></asp:Image>
                                        <br />
                                        <%--<div style="padding-left: 10px;">
                                            <asp:Literal runat="server" ID="CategoriesLiteral"></asp:Literal>
                                            <table style="border: solid 1px #628e02; padding-left: 10px;">
                                                <tr>
                                                    <td><label><b>Total Users in Categories:</b></label></td>
                                                    <td><label><asp:Label runat="server" ID="TotalUsers"></asp:Label></label></td>
                                                </tr>
                                            </table>
                                            <asp:Literal runat="server" ID="TotalUserNonCatLiteral"></asp:Literal>
                                        </div>
                                        
                                                                <rad:RadToolTip ID="RadToolTip3" runat="server" Skin="Black" Width="300px" ManualClose="true" ShowEvent="onclick" Position="MiddleRight" RelativeTo="Element" TargetControlID="QuestionMark4">
                                        <label>The total number of users you have chosen is calculated by first selecting all users in your selected location. From these users, we take away the ones that are not in your chosen categories. <br />
                                             <b>Bear in mind:</b> if the number of Total Users for the categories is less than the number of users you choose to post the ad to, your ad will be also shown to users in the location you specified that are not in the categories you chose [this is the second 
                                             number you see in the boxes below].
                                             It is highly probable that the user counts could change. Please <a class="AddLink" target="_blank" href="about#UserCalculation">read the full algorithm </a>of how this count is calculated, and
                                            how it is possible for the number of users to change. 
                                        </label>
                                        </rad:RadToolTip>--%>
                                        
                                        <asp:Label Visible="false" runat="server" ID="TotalUsers"></asp:Label>
                                        <div class="topDiv">
                                              <%--<asp:UpdatePanel runat="server" ID="ParentUpdatePanel" >
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="AdPlacementList" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                                <ContentTemplate>
                                             
                                                    <asp:Panel runat="server" ID="CategoryDaysPanel"></asp:Panel>
                                                </ContentTemplate>
                                              </asp:UpdatePanel>--%>
                                              
                                        <div id="updateProgressDiv" class="updateProgress" style="display:none">
                                            
                                        </div>
                                        <div align="center" id="insideUpdateProgressDiv"  style="display:none;" 
                                        class="insideUpdateProgress" >
                                            <div style="position: relative; top: 200;">
                                            <img src="image/ajax-loaderBig.gif" />
                                            <span class="updateProgressMessage">Loading ...</span>
                                            </div>
                                        </div>
                                        </div>
                                        
                                        
                                        
                                        
                                        <hr />
                                        <div style="line-height: 20px;">
                                        <div class="topDiv" style="clear: both;">
                                        <div style="float: left; padding-right: 20px;"><label>
                                            <h1 class="Blue">Breakdown of your chosen demographic</h1></label>
                                            </div>
                                            </div><br />
                                        <table border="solid 1px gree;">
                                            <tr>
                                                <td width="30px">
                                                <asp:Label CssClass="AddLink" runat="server" ID="NumTotalUsers"></asp:Label>                                             
                                                </td>
                                                <td valign="top">
                                                    <rad:RadToolTip ID="RadToolTip10" runat="server" Skin="Black" Width="300px" ManualClose="true" 
                                             ShowEvent="onclick" Position="MiddleRight" RelativeTo="Element" TargetControlID="Image5">
                                        <label>
    This number is a count of the users on record with us who match the location and categories you have specified. We cannot provide the actual number of users who will see your ad for a several reasons: We cannot say how many people will log into the site; we cannot control how many users change their category preferences for viewing ads; new users who fit your criteria might register with the site after you post your ad; and existing users could already have too many ads assigned to them. This is explained more comprehensively on the <a class="AddLink" href="about">About page.</a>

                                        </label>
                                        </rad:RadToolTip><label> users fall under your category and location specifications </label><asp:Image CssClass="HelpImage" runat="server"  ID="Image5" ImageUrl="~/image/helpIcon.png"></asp:Image>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" CssClass="AddLink" ID="LocationUserLabel"></asp:Label>
                                                </td>
                                                <td valign="top">
                                       <label> additional users will potentially see your ad if they have no other ads to see because they are in your specific chosen location.</label>
                                                </td>
                                            </tr>
                                            <asp:Panel runat="server" ID="EmailUsersPanel">
                                            <tr>
                                                <td>
                                                    <asp:Label CssClass="AddLink" runat="server" ID="EmailUsersLabel"></asp:Label>
                                                </td>
                                                <td valign="top">
                                            <label>
                                             users will see your ad automatically through email <b>Today!</b></label>
                                             <asp:Image CssClass="HelpImage" runat="server"  ID="Image1" ImageUrl="~/image/helpIcon.png"></asp:Image>
                                             <rad:RadToolTip ID="RadToolTip7" runat="server" Skin="Black" Width="300px" ManualClose="true" 
                                             ShowEvent="onclick" Position="MiddleRight" RelativeTo="Element" TargetControlID="Image1">
                                        <label>
                                            Users have the ability to save ad searches and make them live. Making them live means they will receive an email with
                                            ads matching the saved search criteria. Users also have the ability to configure how many ads the email will have
                                            before the email is sent. Since there is a certain amount of ads needed before the email is sent out, and, since we
                                            have no way of knowing how many ads will be posted in the future matching the same criteria, we cannot provide semi-accurate
                                            estimate of how many users will be available to receive your ad as an email in the future. And so, we show here the accurate
                                            estimate of how many users will automatically see your ad in an email only for today.
                                        </label>
                                        </rad:RadToolTip>
                                        </td>
                                        </tr>
                                        </asp:Panel>
                                           
                                            
                                        </table>
                                        
                                        <br />
                                        <style type="text/css">
                                        #CatDiv{
                                                background:none;
                                                
                                                display:block;
                                                overflow:hidden;
                                                border-style:solid;
                                        }
                                        </style>
                                        <script type="text/javascript">
                                                 function OpenCatDiv()
                                                    {
                                                        var CatDiv = document.getElementById('CatDiv');
                                                        
                                                        if(CatDiv.style.height == '0px'){
                                                            div_animate();
                                                            
                                                        }else{
                                                            CatDiv.style.height = '0px';
                                                        }
                                                    
                                                    }
                                                    
                                                 function div_animate()
	                                                {
	                                                div_height= 150;
                                                	
	                                                div_idnty = document.getElementById('CatDiv');
	                                                curr_div_h = parseInt(div_idnty.style.height.replace('px', ''));
                                                 
                                                 if(curr_div_h == undefined)
                                                 curr_div_h = 0;
                                                 
	                                                if (curr_div_h < div_height)
		                                                {
		                                                curr_div_h = curr_div_h + 5;
		                                                div_idnty.style.height=curr_div_h+'px';
		                                                setTimeout('div_animate()',0);
		                                                }
	                                                }
                                            </script>
                                        
                                         <label><a class="AddLink" style="text-decoration: underline;" onclick="OpenCatDiv()">* Before you post, please keep in mind *</a></label>
                                                 <div id="CatDiv" style="height: 0px; border: 0; width: 420px;">
                                                    <label><span style='color: #ff770d; font-weight: bold;'>* </span>These numbers are not valid for when you are editing the ad.<br />
                                                
                                                     <span style='color: #ff770d; font-weight: bold;'>* </span>Because of the uncertainty of the number of potential users 
                                                     that would fit your ad categories, we cannot guarantee the number of views that your ad will receive. 
                                                     We suggest that you start with a small purchase, monitor your views in the Ad Statistics page, 
                                                     and if you find that your ad is being seen by many viewers, you can always purchase more views.
                                                    
                                                    </label>
                                                 </div>
                                         
                                          
                                        <asp:Panel runat="server" ID="LocationDaysPanel"></asp:Panel>
                                        </div>
                                        <%--<hr />
                                        <div>
                                            
                                            <label>Email Feed. </label>
                                            <asp:Image CssClass="HelpImage" runat="server"  ID="Image33" ImageUrl="~/image/helpIcon.png"></asp:Image>     
                                            <rad:RadToolTip ID="RadToolTip7" runat="server" Skin="Black" Width="350px" Height="200px" 
                                            ManualClose="true" ShowEvent="onclick" Position="MiddleRight" RelativeTo="Element" 
                                            TargetControlID="Image33">
                                            <label>
                                                As an even more proactive approach to your advertising, users will be able to get an email with new ads 
                                                they are interested in that are matching your criteria. They will only get this email whenever they 
                                            </label>
                                        </rad:RadToolTip><br />
                                        <asp:Label runat="server" ID="Label4" CssClass="AddGreenLink"></asp:Label>
                                            <asp:CheckBox runat="server" ID="EmailFeedCheckBox" Text="Include Email Feed [add $0.005/user]" />
                                       </div>--%>
                                        <hr />
                                        
                                            <div class="topDiv" style="clear: both;">
                                            <div style="float: left;"><label>
                                            <h1 class="Green">Choose your Views</h1></label>
                                            </div><div style="float: left; padding-left: 30px;">
                                            <asp:Image CssClass="HelpImage" runat="server"  ID="Image6" ImageUrl="~/image/helpIcon.png"></asp:Image>
                                                                        
                                                                        
                                                                    <rad:RadToolTip ID="RadToolTip11" runat="server" Skin="Black" Width="200px" Height="200px" ManualClose="true" ShowEvent="onclick" Position="MiddleRight" RelativeTo="Element" TargetControlID="Image6">
                                                                        <div class="EventDiv" style="font-family: Arial !important;font-size: 14px !important;color: #cccccc !important;">
                                                                            <label><asp:Label runat="server" ID="FreeLabel"></asp:Label></label>
                                                                        </div>
                                                                        </rad:RadToolTip>
                                                                        </div>
                                                                        </div>
                                                                        <br />
                                            <asp:Panel runat="server" ID="FreeFeaturedAdPanel2">
                                            
                                            <table>
                                                <tr>
                                                    <td>
                                                                <asp:Panel runat="server" ID="PricePanel">
                                                                
                                                               
                                                        <asp:UpdatePanel runat="server" ID="PriceUpdatePanel">
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger EventName="Click" ControlID="CalculateButton" />
                                                                <asp:AsyncPostBackTrigger EventName="SelectedIndexChanged" ControlID="AdPlacementList" />
                                                            </Triggers>
                                                            <ContentTemplate>
                                                          
                                                        <table>
                                                            <asp:Panel runat="server" ID="PaidForPanel" Visible="false">
                                                                <tr>
                                                                    <td>
                                                                        <label class="AddGreeLink">Number of users you have already paid for: </label>
                                                                    </td>
                                                                
                                                                    <td>
                                                                        <asp:Label CssClass="AddGreenLink" runat="server" ID="NumberOfPaidUsers"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </asp:Panel>
                                                            <tr>
                                                                <td>
                                                                    
                                                                    <asp:Panel runat="server" Visible="false">
                                                                    <label><b>Your price is purely calculated based on this number. </b><asp:Image CssClass="HelpImage" Visible="false" runat="server"  ID="QuestionMark1" ImageUrl="~/image/helpIcon.png"></asp:Image>
                                                                       </asp:Panel> 
                                                                        
                                                                    </label>
                                                                    <rad:RadToolTip ID="RadToolTip2" runat="server" Skin="Black" Width="200px" Height="200px" ManualClose="true" ShowEvent="onclick" Position="MiddleRight" RelativeTo="Element" TargetControlID="QuestionMark1">
                                                                        <div class="EventDiv" style="font-family: Arial !important;font-size: 14px !important;color: #cccccc !important;">Your price will be calulated based on the number of users you want your ad to be displayed to. Your ad will stop running
                                                                        when that particular number of users have seen your ad. The price for each user is $0.01. For example, if you choose 1000 users, your cost will be $10.
                                                                        <strong>Bear in mind: </strong>if you choose a large number of viewers and there simply is not enough users to see your ad, your ad could be running on the site for a very long time without having been seen.
                                                                        </div>
                                                                        </rad:RadToolTip>
                                                                    
                                                                </td>
                                                                <td>
                                                                    <div class="topDiv" style="clear: both;">
                                                                        <div style="float: left;">
                                                                    <asp:Panel runat="server" ID="UserNumberPanel">
                                                                        <asp:Label runat="server" ID="UserNumberLabel"></asp:Label>
                                                                    <asp:TextBox runat="server" ID="UsersTextBox" Text="0" Width="100px"></asp:TextBox>
                                                               </asp:Panel>
                                                               </div><div style="float: left;padding-left: 10px;">
                                                                
                                                                    <asp:Button runat="server" ID="CalculateButton" OnClick="CalculatePrice" Text="Calculate Price" />
                                                                        </div>
                                                                        <div style="float: left;padding-left: 10px;">
                                                                    <label>Your Price: </label><asp:Label CssClass="AddLinkBig" Text="$0.00" runat="server" ID="YourPriceLabel"></asp:Label>
                                                                        </div>
                                                                    </div>
                                                                                                                                        <asp:RangeValidator Font-Names="Arial" ForeColor="Red" Visible="false" runat="server" 
                                                                    ID="FreeValidator" ControlToValidate="UsersTextBox" MaximumValue="50" MinimumValue="0" Type="Integer" ErrorMessage="User count can only be between 0 and 50."></asp:RangeValidator>

                                                                </td>
                                                            </tr>
                                                     <tr>
                                                        <td></td>
                                                        <td>
                                                        </td>
                                                     </tr>
                                                     
                                                     <tr>
                                                        <td align="right"></td>
                                                        <td>
                                                        </td>
                                                     </tr>   
                                                            <%--<tr>
                                                                <td>
                                                                    <label>Refundable or Non-Refundable payment option <asp:Label CssClass="AddLink" runat="server" ID="QuestionMark2" Text="what is this?"></asp:Label><br />
                                                                    
                                                                        
                                                                    </label>
                                                                <rad:RadToolTip ID="RadToolTip1" runat="server" Skin="Black" Width="200px" Height="200px" ManualClose="true" ShowEvent="onclick" Position="MiddleRight" RelativeTo="Element" TargetControlID="QuestionMark2">
                                                                            <label>Refundable ad charge is $0.015 per user, while the non-refundable ad charge is $0.01 per user. The amount refunded will be calculated based on the number of people who have see your ad up to the date and time you notify Hippo Happenings of your request to refund the ad.<br />
                                                                    An ad can only be refunded if it falls under the </label><a class="AddLink" href=\"Adterms-and-conditions\">Refund Terms and Conditions</a>.
                                                                        </rad:RadToolTip>
                                                                    <asp:CheckBox runat="server" ID="RefundableCheckBox" Text="Refundable Ad" />
                                                                </td>
                                                            </tr>--%>
                                                        </table>
                                                            
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>  
                                                        
                                                         </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                            </asp:Panel>
                                            <asp:Panel runat="server" ID="FreeFeaturedAdPanel3" Visible="false">
                                                <div class="EventDiv">
                                                    <table >
                                                        <tr><td valign="middle">
                                                        <img src="image/TheStar5.png" width="50px" height="50px" />
                                                        </td>
                                                            <td valign="middle">
                                                    <label><span style="color: White; font-weight: bold;">Since featured ads are free during this time frame, your view limit is set to the specified number above. Normally, you would choose the number of users you would like to display your ad to.</span>
                                                    </label>
                                                    </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </asp:Panel>
                                             </asp:Panel>
                                        </ContentTemplate>
                                      </asp:UpdatePanel>
                                        <div align="right" style=" padding-top: 30px; padding-right: 50px; clear: both;">
                                            <asp:ImageButton runat="server" ID="ImageButton6" OnClick="Backwards" ImageUrl="image/BackwardsButton.png" 
                                            onmouseout="this.src='image/BackwardsButton.png'" onmouseover="this.src='image/BackwardsButtonSelected.png'"  />
                                            <asp:ImageButton runat="server" ID="ImageButton13" OnClick="Onwards" ImageUrl="image/OnwardsButton.png" 
                                            onmouseout="this.src='image/OnwardsButton.png'" onmouseover="this.src='image/OnwardsButtonSelected.png'"  />
                                         </div>
                                        </div>
                                    </div>
                                    <div align="center" style=" margin-left: 11px ;background-color: #515151 ;height: 4px; width: 640px;"></div>
                                </asp:Panel>
                            </rad:RadPageView>
                            <rad:RadPageView runat="server" ID="TabFive" TabIndex="5" >
                                
                                <asp:Panel runat="server" ID="Panel13" DefaultButton="CategoryOnwardButton">          
                                    <div style="background: url('image/TabsBackgroundTop.png'); background-repeat: no-repeat; width: 668px; height: 4px;">
                                    </div>
                                    <div class="EventDiv" style="background: url('image/TabsBackground.png'); background-repeat: repeat-y;  width: 668px;">
                                        <div style="padding-left: 30px; width: 550px; padding-top: 10px;">
                                            <h1>Payment Info</h1>
                                            
                                            <asp:Panel Visible="false" runat="server" ID="FreeFeaturedAdPanel4">
                                                <div class="EventDiv">
                                                    <table >
                                                        <tr><td valign="middle">
                                                        <img src="image/TheStar5.png" width="50px" height="50px" />
                                                        </td>
                                                            <td valign="middle">
                                                    <label><span style="color: White; font-weight: bold;">Nothing to see here. Featured ads are free during this time frame.</span>
                                                    </label>
                                                    </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                    
                                        <div align="right" style=" padding-top: 30px; padding-right: 50px; clear: both;">
                                            <asp:ImageButton runat="server" ID="ImageButton7" OnClick="Backwards" ImageUrl="image/BackwardsButton.png" 
                                            onmouseout="this.src='image/BackwardsButton.png'" onmouseover="this.src='image/BackwardsButtonSelected.png'"  />
                                            <asp:ImageButton runat="server" ID="ImageButton8" OnClick="Onwards" ImageUrl="image/OnwardsButton.png" 
                                            onmouseout="this.src='image/OnwardsButton.png'" onmouseover="this.src='image/OnwardsButtonSelected.png'"  />
                                         </div>
                                    
                                    </div>
                                    <div align="center" style=" margin-left: 11px ;background-color: #515151 ;height: 4px; width: 640px;"></div>
                                </asp:Panel>
                            </rad:RadPageView>
                            <rad:RadPageView runat="server" ID="TabSix" TabIndex="6" >
                                <asp:Panel runat="server" ID="Panel5" DefaultButton="PostItButton"> 
                                    <div style="background: url('image/TabsBackgroundTop.png'); background-repeat: no-repeat; width: 668px; height: 4px;">
                                    </div>
                                    <div class="EventDiv"  style="background: url('image/TabsBackground.png'); background-repeat: repeat-y; width: 668px;">
                                        <div style="padding-left: 30px; width: 550px; padding-top: 10px;">
                                        <h1>Post It</h1>
                                            <label><span style="font-weight: bold;">You're Almost Done!</span> <br /> <span style='color: #ff770d;'>Preview your post below and read the Terms and Conditions. Go Backwards! if you'd like to change anything!</span></label>
                                            <asp:Panel runat="server" ID="FeaturedWeWouldPanel">
                                            <br /><br /><label>We would also like to add that, in an effort to save our users a view, the user whom posted the ad will not see it in their ad rotation nor in their saved ad search emails.</label>
                                        </asp:Panel>
                                        </div>
                                        <div style="padding-left: 30px;">
                                            <br />
                                            <label>Terms & Conditions</label>
                                      <div style="margin-top: 10px;margin-bottom: 10px;border: solid 2px #1B1B1B; padding-top: 20px; padding-bottom: 20px; background-color: #666666; width: 600px;">    
                                <asp:Panel runat="server" CssClass="TermsPanel"  ScrollBars="Vertical" ID="TACTextBox" Height="100px" Width="550px"></asp:Panel>
                                </div>  
                                            <asp:CheckBox runat="server" ID="AgreeCheckBox" Text="<span style='color: #1fb6e7; font-weight: bold;'>* </span>Agree to the Terms & Conditions" />
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
                                
                                <asp:Panel runat="server" ID="FeaturedPreviewPanel" Visible="false">
                                    <div class="EventDiv"><h2>Your ad as it will appear throughout the site 
                                    and in user's emails from saved searches.</h2>
                                    </div>
                                    <asp:Literal runat="server" ID="FeaturedPreviewLiteral"></asp:Literal>
                                </asp:Panel>
                                       <div class="EventDiv"><h2>Your ad as it will appear on it's ad page.</h2></div>
                                    <div style=" background-color: #1b1b1b; border: solid 4px #515151; width: 840px; margin-left: 11px; margin-top: 20px; padding: 5px;">
                                        <asp:Panel runat="server" ID="EventPanel" Visible="false">
                                            <h1 class="EventHeader"><asp:Label runat="server" ID="ShowHeaderName"></asp:Label></h1>
                                           
                                            <div id="topDiv5" style="padding-top: 5px;">
                                                <div style="float: left; width: 420px;">
                                                     <asp:Label runat="server" ID="ShowDescription" CssClass="EventBody"></asp:Label>
                                                </div>
                                                <div style="float:left; width: 420px;">
                                                    <asp:Literal runat="server" ID="ShowVideoPictureLiteral">
                                                    </asp:Literal>
                                                    <asp:Panel runat="server" ID="RotatorPanel" Visible="false">
                                                        <rad:RadRotator runat="server" ScrollDuration="200" ID="Rotator1" ItemHeight="250" ItemWidth="400"  
                                                        Width="440px" Height="250px" RotatorType="Buttons">
                                                            
                                                            <%--<ItemTemplate>
                                                                <div style="border: solid 2px blue; padding: 3px; margin: 3px;">
                                                                    <img width="184px" height="184px" src='<%# Container.DataItem %>' />
                                                                </div>            
                                                            </ItemTemplate>--%>
                                                        </rad:RadRotator>
                                                    </asp:Panel>
                                                </div>
                                            </div>
                                            
                                        </asp:Panel>
                                    </div>
                               
                                    
                                </asp:Panel>
                             </rad:RadPageView>

                        </rad:RadMultiPage>
                        </div>
                    </div>
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
    </asp:Panel>
    </div>

<%--</rad:RadAjaxPanel>--%>
</asp:Content>

