<%@ Page Language="C#" MasterPageFile="~/SecondMasterPage.master" ValidateRequest="false" 
AutoEventWireup="true" 
CodeFile="EnterVenue.aspx.cs" 
Inherits="EnterVenue" Title="Enter a Locale: venue, bar, club, place, reastaurant, museum, shop | HippoHappenings" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register Src="~/Controls/Ads.ascx" TagName="Ads" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/SmallButton.ascx" TagName="SmallButton" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/BlueButton.ascx" TagName="BlueButton" TagPrefix="ctrl" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

 <rad:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="false" 
                KeepInScreenBounds="true" VisibleOnPageLoad="false"  Behaviors="none"
            ID="MessageRadWindowManager" ReloadOnShow="true" Modal="true" 
            DestroyOnClose="true" RestrictionZoneID="RestrictionZone"
            runat="server">
                <Windows>
                    <rad:RadWindow Width="300" ClientCallBackFunction="OnClientClose" EnableEmbeddedSkins="true" 
                    VisibleStatusbar="false" Skin="Web20" Height="300" ID="MessageRadWindow" Title="Alert" runat="server">
                    </rad:RadWindow>
                    <rad:RadWindow Width="600" ClientCallBackFunction="FillVenueJS" 
                    VisibleStatusbar="false" Skin="Web20" Height="500" ID="RadWindow1" 
                     runat="server">
                    </rad:RadWindow>
                </Windows>
            </rad:RadWindowManager>
            <script type="text/javascript">
            function OnClientClose(oWnd, args)
            {

                window.location = args;
            }
           function OpenFeaturePricing(page)
           {
                var win = $find("<%=RadWindow1.ClientID %>");
                win.setUrl('PricingChart.aspx?P=' + page);
                win.show(); 
                win.center(); 
           }
            </script>
<a id="top"></a>
<rad:RadProgressManager ID="Radprogressmanager1" runat="server" EnableEmbeddedBaseStylesheet="False" EnableEmbeddedSkins="False" UniquePageIdentifier="112b806f-5e25-4f58-b795-09385fad96e5" />


    <asp:Label runat="server" ID="ErrorLabel" ForeColor="Red"></asp:Label>
    <asp:Panel runat="server" ID="LoggedOutPanel">
        <div class="Text12">
        <div style="width: 820px; clear: both;">
         <div style="width: 430px; float: left;">
            <h1>Enter locale</h1>
            <asp:Label runat="server" ID="WelcomeLabel"></asp:Label>
         </div>
        </div>  
        <%--<div style="width: 820px;clear: both;">
            <div style="padding-top: 40px;float: left;">
                <ctrl:Ads ID="Ads1" runat="server" />
                <div style="float: right;padding-right: 10px;">
                    <a class="NavyLinkSmall" href="post-bulletin">+Add Bulletin</a>
                </div>
            </div>
        </div>--%>
        </div>
        <%--<div class="topDiv" style="width: 847px;clear: both;margin-left: -18px;position: relative;top: 207px;">
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
                                        <b>Hippo Locale Tips</b>                                            <ol>                                                <li>                                                    Earn points toward being featured on our home page by posting more locales.                                                 </li>                                                <li>                                                    Post, Share, Add to favorites, text, email, discuss.                                                 </li>                                                <li>                                                    Receive notifications of new events posted in your favorite locale.                                                </li>                                                <li>                                                    Edit locales you own or submit changes to locales owned by others.                                                </li>                                                <li>                                                    Feature a locale for as little as $2/day.                                                 </li>                                            </ol>
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
           </div>--%>
    </asp:Panel>
    <asp:Panel runat="server" ID="BigEventPanel">
    <div class="Text12">
    <div style="padding-bottom: 10px;">
    <div class="topDiv">
            <div style="float: left;">
                <h1><asp:Label runat="server" ID="TitleLabel" Text="Enter a locale"></asp:Label></h1>
            </div>
            <div style="float: left; padding-left: 5px;padding-top: 10px;">
                <asp:Image CssClass="HelpImage" runat="server"  ID="Image1" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
                <rad:RadToolTip ID="RadToolTip1" runat="server" Skin="Sunset" 
                Width="200px" ManualClose="true" ShowEvent="onclick" 
                Position="MiddleRight" RelativeTo="Element" TargetControlID="Image1">
                <div style="padding: 10px;"><label>All HTML tags will be removed except for 
                links. However, the header cannot contain links at all.</label></div>
                </rad:RadToolTip>
            </div>
            <%--<div style="float: right;">
                <div style="float: right;">
                    <span id="siteseal"><script type="text/javascript" src="https://seal.godaddy.com/getSeal?sealID=VCTQw6UxQKW98668yOHTM1toSvv9HhCrsr4Zlfm5tU4bF5LIfad8lQ6XpssJ"></script><br/><a style="font-family: arial; font-size: 9px" href="https://www.godaddy.com/ssl/ssl-certificates.aspx" target="_blank">SSL</a></span>
                </div>
            </div>--%>
        </div>
        
        <asp:Label runat="server" ID="UserNameLabel" Visible="false"></asp:Label>
    </div>
    <div id="topDiv6">
        <div style="float: left; width: 670px;">
        <div>
            <%--<rad:RadTabStrip runat="server" ID="EventTabStrip"
            Skin="HippoHappenings" CssClass="EventTabStrip" OnTabClick="TabClick"  Height="27px"
            EnableEmbeddedSkins="false" MultiPageID="BlogEventPages" SelectedIndex="0">
                <Tabs>
                    <rad:RadTab runat="server" PageViewID="TabOne" 
                    ImageUrl="NewImages/DetailsOneFull.png" 
                    HoveredImageUrl="NewImages/DetailsOneFull.png" TabIndex="0" 
                    DisabledImageUrl="NewImages/DetailsOneFull.png" 
                    SelectedImageUrl="NewImages/DetailsOneFull.png" >

                    </rad:RadTab>
                    <rad:RadTab runat="server" Enabled="false"  PageViewID="RadPageView1" 
                    ImageUrl="NewImages/DescriptionTwoFull.png" 
                    HoveredImageUrl="NewImages/DescriptionTwoFull.png"  TabIndex="1"  
                    DisabledImageUrl="NewImages/DescriptionTwoEmpty.png" 
                    SelectedImageUrl="NewImages/DescriptionTwoFull.png"  >
                    </rad:RadTab>
                    <rad:RadTab runat="server" Enabled="false"   PageViewID="RadPageView2" 
                    ImageUrl="NewImages/MediaThreeFull.png" 
                    HoveredImageUrl="NewImages/MediaThreeFull.png"   TabIndex="2" 
                    DisabledImageUrl="NewImages/MediaThreeEmpty.png" 
                    SelectedImageUrl="NewImages/MediaThreeFull.png"  ></rad:RadTab>
                    <rad:RadTab runat="server" Enabled="false"   PageViewID="RadPageView3" 
                    ImageUrl="NewImages/CategoriesFourFull.png" 
                    HoveredImageUrl="NewImages/CategoriesFourFull.png"  TabIndex="3" 
                    DisabledImageUrl="NewImages/CategoriesFourEmpty.png" 
                    SelectedImageUrl="NewImages/CategoriesFourFull.png"  ></rad:RadTab>
                    <rad:RadTab runat="server" Enabled="false"   PageViewID="RadPageView4" 
                    ImageUrl="NewImages/HoursFiveFull.png" 
                    HoveredImageUrl="NewImages/HoursFiveFull.png"  TabIndex="4" 
                    DisabledImageUrl="NewImages/HoursFiveEmpty.png" 
                    SelectedImageUrl="NewImages/HoursFiveFull.png"  ></rad:RadTab>
                    <%--<rad:RadTab runat="server"  Enabled="false"  PageViewID="RadPageView5" 
                    ImageUrl="NewImages/FeatureSixFull.png" 
                    HoveredImageUrl="NewImages/FeatureSixFull.png"  TabIndex="5" 
                    DisabledImageUrl="NewImages/FeatureSixEmpty.png" 
                    SelectedImageUrl="NewImages/FeatureSixFull.png" ></rad:RadTab>
                    <rad:RadTab runat="server"  Enabled="false"  PageViewID="RadPageView6" 
                    ImageUrl="NewImages/PostItSevenFull.png" 
                    HoveredImageUrl="NewImages/PostItSevenFull.png" 
                    DisabledImageUrl="NewImages/PostItSevenEmpty.png"   TabIndex="6" 
                    SelectedImageUrl="NewImages/PostItSevenFull.png"  ></rad:RadTab>
                </Tabs>
            </rad:RadTabStrip>--%>
        </div>
        <%--<rad:RadMultiPage runat="server" ID="BlogEventPages" SelectedIndex="0">--%>
            <%--<rad:RadPageView runat="server" ID="TabOne" TabIndex="0" >--%>
                <div style="position: relative;">
                <asp:Panel runat="server" ID="Panel1">
                <script type="text/javascript">
                                                function ShowDiv()
                                                {
                                                    var theDiv = document.getElementById('WidthDiv');
                                                    var loadDiv = document.getElementById('LoadDiv');
                                                    
                                                    loadDiv.style.height = theDiv.offsetHeight +'px';
                                                    loadDiv.style.display = 'block';
                                                    
                                                    return true;
                                                }
                                            </script>
                                            <div id="LoadDiv" style="z-index: 10000;width: 670px;display: none;position: absolute;">
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
                    <div id="WidthDiv" class="topDiv" style="width: 668px; border-left: solid 1px #dedbdb; border-right: solid 1px #dedbdb; border-top: solid 1px #dedbdb;">
                        <div class="EventDiv" style="padding-left: 10px;float: left;">
                     
                            <table>
                                <tr>
                                    <td width="250px;">
                                            <script type="text/javascript">
                                              function CountCharsHeadline(editor, e){
                                                    
                                                        var theDiv = document.getElementById("CharsDivHeadline");
                                                        var theText = document.getElementById("ctl00_ContentPlaceHolder1_VenueNameTextBox");
                                                        theDiv.innerHTML = "characters left: "+ (70 - theText.value.length).toString();
                                                    
                                                }
                                            </script>
                                        <h2>Name<span class="Asterisk">* </span></h2><br />
                                        <span class="NavyLink12"> 
                                        max 70 characters  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    &nbsp;&nbsp;<span style="color: Red;" id="CharsDivHeadline"></span></span><br />
                                        <asp:TextBox runat="server" onkeypress="CountCharsHeadline(event)" ID="VenueNameTextBox" Width="200"  ></asp:TextBox>
                                       <br /><br /><br />
                                        <h2>Phone</h2><asp:TextBox runat="server" ID="PhoneTextBox"></asp:TextBox><br />
                                        <h2>Email</h2><asp:TextBox runat="server" ID="EmailTextBox"></asp:TextBox><br />
                                        <h2>Web Site</h2><asp:TextBox runat="server" ID="WebSiteTextBox"></asp:TextBox><br />
                                         <br /> <label><span class="NavyLink12"><span class="Asterisk">* </span>required fields</span></label>
                               </td>
    
                                    <td valign="top" width="330px">
                                    <asp:UpdatePanel runat="server" UpdateMode="conditional">
                                        <ContentTemplate>
                                    <script type="text/javascript">
                                                function ShowLocDiv()
                                                {
                                                    var theDiv = document.getElementById('WidthLocDiv');
                                                    var loadDiv = document.getElementById('LoadLocDiv');
                                                    
                                                    loadDiv.style.height = theDiv.offsetHeight +'px';
                                                    loadDiv.style.display = 'block';
                                                    
                                                    return true;
                                                }
                                            </script>
                                            <div id="LoadLocDiv" style="z-index: 10000;width: 400px;display: none;position: absolute;">
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
                                            
                                    <div id="WidthLocDiv" style="padding-left: 25px; padding-top: 30px;">
                                    <h2>Location<span class="Asterisk">* </span></h2>
                                        <table cellpadding="0" cellspacing="5">
                                            <tr>
                                                
                                                
                                                <td colspan="2">
                                                <asp:Panel runat="server" ID="USPanel">
                                                
                                                    
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <label>Street No.<span class="Asterisk">* </span></label>
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
                                                
                                                </asp:Panel>
                                                <asp:Panel runat="server" ID="InternationalPanel" Visible="false">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                               <label>Location<span class="Asterisk">* </span></label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="LocationTextBox" Width="200px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    </asp:Panel>
                                                </td>
                                                
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
                                            <tr>
                                                <td>
                                                    <label style="padding-right: 10px;">Country<span class="Asterisk">* </span></label><br />
                                                    <asp:DropDownList onchange="ShowLocDiv()" OnSelectedIndexChanged="ChangeState" AutoPostBack="true" runat="server" DataSourceID="SqlDataSourceCountry" DataTextField="country_name" DataValueField="country_id" ID="CountryDropDown"></asp:DropDownList>
                                                    <asp:SqlDataSource runat="server" ID="SqlDataSourceCountry" SelectCommand="SELECT * FROM Countries"
                                                    ConnectionString="<%$ ConnectionStrings:Connection %>"></asp:SqlDataSource>
                                                </td>
                                                
                                                <td style="padding-left: 3px;">
                                                    <label>State<span class="Asterisk">* </span></label><br />
                                                    <asp:Panel runat="server" ID="StateTextBoxPanel">
                                                        <asp:TextBox ID="StateTextBox" runat="server" Width="100" ></asp:TextBox>
                                                    </asp:Panel>
                                                    <asp:Panel runat="server" ID="StateDropDownPanel" Visible="false">
                                                        <asp:DropDownList runat="server" ID="StateDropDown"></asp:DropDownList>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <label>City<span class="Asterisk">* </span></label>
                                                            </td>
                                                            <td style="padding-left: 3px;">
                                                                <label>Zip<span class="Asterisk">* </span></label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
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
                                        </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    </td>
                                </tr>
                                
                                
                            </table>
                        </div>
                        <%--<div class="topDiv" align="right" style="width: 85px; float: right;padding-right: 10px;">
                                <ctrl:BlueButton ID="DetailsOnwardsButton" WIDTH="76px"  runat="server" CLIENT_LINK_CLICK="ShowDiv()"
                                BUTTON_TEXT="Onwards" />
                             </div>--%>
                    </div>
                </asp:Panel>
                </div>
           <%-- </rad:RadPageView>--%>
            <%--<rad:RadPageView runat="server" ID="RadPageView1" TabIndex="1" >--%>
            <div class="EventDiv" id="Div5" style=" width: 614px; border-top: solid 1px #dedbdb;">
                <asp:Panel runat="server" ID="Panel2">
               <div style="position: relative;">
                <script type="text/javascript">
                                                function ShowDesDiv()
                                                {
                                                    var theDiv = document.getElementById('WidthDesDiv');
                                                    var loadDiv = document.getElementById('LoadDesDiv');
                                                    
                                                    loadDiv.style.height = theDiv.offsetHeight +'px';
                                                    loadDiv.style.display = 'block';
                                                    
                                                    return true;
                                                }
                                            </script>
                                            <div id="LoadDesDiv" style="z-index: 10000;width: 670px;display: none;position: absolute;">
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
                    <div id="WidthDesDiv" class="topDiv" style="width: 668px; border-left: solid 1px #dedbdb; border-right: solid 1px #dedbdb;">
                        <div style="padding-bottom: 20px; width: 668px;">
                            
                            
                            <div style="padding-left: 10px;">
                               
                            
                                <h2>Description<span class="Asterisk">* </span></h2><span class="NavyLink12"> min 50 characters
                                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                  <span style="color: Red;" id="CharsDivEditor"></span>
                                </span><br />
                           
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
                                                                    if(navigator.appCodeName == 'Netscape')
                                                                    {
                                                                        if(this.activeElement.textContent.length > 50)
                                                                            theDiv.innerHTML = "characters needed: 0";
                                                                        else
                                                                            theDiv.innerHTML = "characters needed: "+ (50 - this.activeElement.textContent.length).toString();
                                                                    }
                                                                    else
                                                                    {
                                                                        var theEditor = $find("<%=DescriptionTextBox.ClientID %>");
                                                                        if(theEditor.get_contentArea().textContent != null)
                                                                        {
                                                                        if(theEditor.get_contentArea().textContent.length > 50)
                                                                            theDiv.innerHTML = "characters needed: 0";
                                                                        else
                                                                            theDiv.innerHTML = "characters needed: "+ (50 - theEditor.get_contentArea().textContent.length).toString();
                                                                        }else
                                                                        {
                                                                            if(theEditor.get_contentArea().innerText.length > 50)
                                                                                theDiv.innerHTML = "characters needed: 0";
                                                                            else
                                                                                theDiv.innerHTML = "characters needed: "+ (50 - theEditor.get_contentArea().innerText.length).toString();
                                                                        }
                                                                    }
                                                                }
                                                            </script>
                                                            

                                                    <rad:RadEditor EditModes="Design" OnClientLoad="OnClientLoad" 
                                                    Skin="Vista" runat="server" 
                                                    ID="DescriptionTextBox" Height="110px" Width="400px" 
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
                            </div>
                        </div>
                        
                        <%--<div class="topDiv" align="right" style="padding-right: 50px;width: 660px;">
                            
                            <div style="float: right;">
                                <ctrl:BlueButton ID="ImageButton2" CLIENT_LINK_CLICK="ShowDesDiv()"  WIDTH="76px" runat="server" BUTTON_TEXT="Onwards" />
                            </div>
                            <div style="float: right;padding-right: 10px;">
                                <ctrl:BlueButton ID="DescriptionOnwardsButton" CLIENT_LINK_CLICK="ShowDesDiv()" WIDTH="87px" runat="server" BUTTON_TEXT="Backwards" />
                            </div>
                         </div>--%>
                    </div>
               </div>
                </asp:Panel>
                </div>
            <%--</rad:RadPageView>--%>
            <%--<rad:RadPageView runat="server" ID="RadPageView2" TabIndex="2" >--%>
            <div class="EventDiv" id="Div1" style=" width: 614px; border-top: solid 1px #dedbdb;">
                <asp:Panel runat="server" ID="Panel3">
                    <div style="position: relative;">
                    <script type="text/javascript">
                                                function ShowMediaDiv()
                                                {
                                                    var theDiv = document.getElementById('WidthMediaDiv');
                                                    var loadDiv = document.getElementById('LoadMediaDiv');
                                                    
                                                    loadDiv.style.height = theDiv.offsetHeight +'px';
                                                    loadDiv.style.display = 'block';
                                                    
                                                    return true;
                                                }
                                            </script>
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
                    <div id="WidthMediaDiv" class="topDiv" style="width: 668px; border-left: solid 1px #dedbdb; border-right: solid 1px #dedbdb; padding-bottom: 20px;">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="buttonSubmit"/>
                            </Triggers>
                            <ContentTemplate>
                            <a href="#top"></a>
                            <script type="text/javascript">
                                                function ShowInDiv()
                                                {
                                                    var theDiv = document.getElementById('WidthInDiv');
                                                    var loadDiv = document.getElementById('LoadInDiv');
                                                    
                                                    loadDiv.style.height = theDiv.offsetHeight +'px';
                                                    loadDiv.style.display = 'block';
                                                    
                                                    return true;
                                                }
                                            </script>
                                            <div id="LoadInDiv" style="z-index: 10000;width: 670px;display: none;position: absolute;">
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
                            <div id="WidthInDiv" class="EventDiv">
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
                               <div class="topDiv" style="padding-top: 10px;padding-left: 10px;">
                                    <asp:CheckBox ID="MainAttractionCheck" onclick="ShowInDiv()" Text="Include Pictures and Videos <span  class='NavyLink12'>[20 max]</span><div style='float: right;padding-right: 400px;position: relative;'><div class='Text12' style='position: absolute;float: left;'><div id='div5' style='z-index: 10000; left: -100px; top: 20px;padding: 10px;position: absolute;display: none;background-color: white; width: 150px; border: solid 2px #6fa8bf;'>If you're uploading photos/images make sure you have specified a descriptive name for your file. It will help search engines, like google, find your page more effectively.</div><img onmouseout='showText()' onmouseover='hideText()' src='http://hippohappenings.com/NewImages/HelpIconNew.png' /></div></div>"  runat="server" AutoPostBack="true" OnCheckedChanged="EnableMainAttractionPanel" />
                                        <asp:Panel runat="server" ID="MainAttractionPanel" Enabled="false" Visible="false"> 
                                            <div style="padding-left: 30px;" class="topDiv">
                                                <asp:RadioButtonList onchange="ShowInDiv()" runat="server" ID="MainAttracionRadioList" AutoPostBack="true" 
                                                OnSelectedIndexChanged="ShowSliderOrVidPic">
                                                    <asp:ListItem Text="Add YouTube Video" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Add Picture" Value="1"></asp:ListItem>
                                                </asp:RadioButtonList>
                                                <div class="topDiv" style="padding-left: 30px; width: 510px; padding-bottom: 10px; padding-left: 30px;">
                                                    <asp:Panel runat="server" ID="VideoPanel" Visible="false">
                                                        <div style="padding-left: 20px;">
                                                            <asp:Panel runat="server" ID="UploadPanel" Visible="false">
                                                                    <div style=" border:  solid 1px #dedbdb;padding: 10px;">
                                                                            <div id="topDiv3">
                                                                                <div style="float: left; padding-top: 4px;">
                                                                                    <asp:FileUpload  runat="server" ID="VideoUpload" Width="230px" EnableViewState="true" />
                                                                                </div>
                                                                                <div style="float: left;padding-left: 5px; padding-top: 3px;">
                                                                                <ctrl:BlueButton ID="ImageButton6" runat="server" WIDTH="66px" CLIENT_LINK_CLICK="ShowInDiv()" BUTTON_TEXT="Upload" />
                                                                            </div>
                                                                            </div>
                                                                        </div>
                                                                
                                                            </asp:Panel>
                                                            <asp:Panel runat="server" ID="YouTubePanel">
                                                            <div style=" border:  solid 1px #dedbdb; padding: 10px;">
                                                                            <div style="clear: both;">
                                                                                <label>Insert the ID of the YouTube video:</label><br />
                                                                                <div class="topDiv">
                                                                                <div style="float: left;padding-top: 4px;">
                                                                                    <asp:TextBox runat="server" ID="YouTubeTextBox"></asp:TextBox>
                                                                                </div>
                                                                                <div style="float: left;padding-left: 5px;padding-right: 10px;">
                                                                                    <ctrl:BlueButton ID="ImageButton7" CLIENT_LINK_CLICK="ShowInDiv()" WIDTH="66px"  runat="server" BUTTON_TEXT="Upload" />
                                                                                </div>
                                                                                <label>ex: if this is the url of your YouTube video <br />http://www.youtube.com/watch?v=<span style="color: Red;">YMLTwq1M2pw</span>  <--This is the ID</label>
                                                                                </div>
                                                                            </div>
                                                                         </div>
                                                            </asp:Panel>
                                                            
                                                         </div>
                                                     </asp:Panel> 
                                                     <div style="padding-left: 30px;" class="topDiv">
                                                            <asp:Panel runat="server" ID="PicturePanel" Visible="false">
                                                                <div style=" border:  solid 1px #dedbdb;padding: 10px;">
                                                                            <div class="topDiv">
                                                                                <div style="float: left; padding-top: 4px;">
                                                                                
                                                                                    <rad:RadUpload Skin="Web20" ID="PictureUpload" runat="server"
                                                                                        MaxFileInputsCount="20" />
                                                                                        
                                                                                    <rad:RadProgressArea id="progressArea1" runat="server"/>
                                                                                    
                                                                                    <asp:Button id="buttonSubmit" OnClientClick="ShowInDiv()" runat="server" CssClass="RadUploadSubmit" OnClick="PictureUpload_Click" text="Submit" />
                                                                                
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
                                                            <ctrl:BlueButton CLIENT_LINK_CLICK="ShowInDiv()" ID="PictureNixItButton"  WIDTH="54px"  runat="server" BUTTON_TEXT="Nix It" />
                                                        </div>
                                                    </asp:Panel>
                                          
                                        </asp:Panel>
                                    
                                </div>
                            </div>
                         </ContentTemplate>
                       </asp:UpdatePanel>
                        <%--<div class="topDiv" align="right" style="padding-right: 50px;width: 660px;padding-top: 10px;clear:both;">
                            
                            <div style="float: right;">
                                <ctrl:BlueButton ID="ImageButton3" runat="server"  WIDTH="76px" CLIENT_LINK_CLICK="ShowMediaDiv()" BUTTON_TEXT="Onwards" />
                            </div>
                            <div style="float: right;padding-right: 10px;">
                                <ctrl:BlueButton ID="MediaOnwardsButton" CLIENT_LINK_CLICK="ShowMediaDiv()" WIDTH="87px" runat="server" BUTTON_TEXT="Backwards" />
                            </div>
                         </div>--%>
                    
                    </div>
                    </div>
                </asp:Panel>
                </div>
           <%-- </rad:RadPageView>--%>

            <%--<rad:RadPageView runat="server" ID="RadPageView3" TabIndex="3" >--%>
                <div class="EventDiv" id="Div2" style=" width: 614px; border-top: solid 1px #dedbdb;">
                <asp:Panel runat="server" ID="Panel4">          
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
                    <div id="WidthCatDiv" class="topDiv" style="width: 668px; border-left: solid 1px #dedbdb; border-right: solid 1px #dedbdb;">
                        <div style="padding-left: 10px; width: 668px;">
                  
                        <div class="topDiv">
                                <div style="float: left;">
                                    <h2>Select Categories for Your Locale<span class="Asterisk">* </span></h2>
                                </div>
                                <div style="float: left; padding-left: 5px;padding-top: 3px;">
                                    <asp:Image CssClass="HelpImage" runat="server"  ID="Image5" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
                                    <rad:RadToolTip ID="RadToolTip6" runat="server" Skin="Sunset" 
                                    Width="200px" ManualClose="true" ShowEvent="onclick" 
                                    Position="MiddleRight" RelativeTo="Element" TargetControlID="Image5">
                                    <div style="padding: 10px;"><label>Selecting categories that most 
                                    represent your locale will help your viewers find it faster via our 
                                    search page.</label></div>
                                    </rad:RadToolTip>
                                </div>
                            </div>
                        <div style="padding-left: 20px;">
                          
                        <table>
                                        <tr>
                                            <td valign="top">
                                                    <%--<asp:CheckBoxList runat="server" ID="CategoriesCheckBoxes" 
                                                    RepeatColumns="4" RepeatDirection="horizontal">
                                                    
                                                    </asp:CheckBoxList>--%>
                                                    <div class="FloatLeft" style="width: 150px;">
                                                        <rad:RadTreeView Width="150px" runat="server"  
                                                        ID="CategoryTree" DataFieldID="ID" Skin="Vista" 
                                                        DataSourceID="SqlDataSource1" CheckChildNodes="false"
                                                        DataFieldParentID="ParentID" CheckBoxes="true">
                                                        
                                                        <DataBindings>
                                                            
                                                             <rad:RadTreeNodeBinding Checkable="true" Checked="false"
                                                             TextField="Name" Expanded="false" ValueField="ID" />
                                                         </DataBindings>
                                                        </rad:RadTreeView>
                                                        <asp:SqlDataSource runat="server" ID="SqlDataSource1" 
                                                            SelectCommand="SELECT * FROM VenueCategories WHERE  LIVE = 'True' AND  ((ParentID IN (SELECT ID FROM VenueCategories WHERE (Name < 'Pa' ) AND (ParentID IS NULL))) OR ((Name < 'Pa') AND (ParentID IS NULL))) ORDER BY Name ASC"
                                                            ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                        </asp:SqlDataSource>
                                                    </div>
                                                    
                                               
                                            </td>
                                            <td valign="top">
                                                <asp:CheckBoxList runat="server" ID="CheckBoxList1" 
                                                    RepeatColumns="4" RepeatDirection="horizontal">
                                                    
                                                    </asp:CheckBoxList>
                                                    <div class="FloatLeft" style="width: 150px;">
                                                        <rad:RadTreeView Width="170px" runat="server"  
                                                        ID="RadTreeView2" Skin="Vista"  DataFieldID="ID" DataSourceID="SqlDataSource3"
                                                        DataFieldParentID="ParentID"
                                                        CheckBoxes="true">
                                                        <DataBindings>
                                                             <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                             TextField="Name" Expanded="false" ValueField="ID"  />
                                                         </DataBindings>
                                                        </rad:RadTreeView>
                                                        <asp:SqlDataSource runat="server" ID="SqlDataSource3" 
                                                            SelectCommand="SELECT * FROM VenueCategories WHERE  LIVE = 'True' AND  ((ParentID IN (SELECT ID FROM VenueCategories WHERE (Name >= 'Pa' ) AND (ParentID IS NULL))) OR ((Name >= 'Pa') AND (ParentID IS NULL))) ORDER BY Name ASC"
                                                            ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                        </asp:SqlDataSource>
                                                    </div>
                                            </td>
                                        </tr>
                                    </table>                        </div>
                                    <div style="width: 550px; clear: both;">
                            <div style="float: left; padding-top: 10px;">
                                <label>Don't see a category you want? Suggest one!</label>
                                <asp:TextBox runat="server" ID="CategoriesTextBox"></asp:TextBox>
                            </div>
                            <div style="padding-top: 9px; float: left; padding-left: 10px;">   
                              <ctrl:BlueButton ID="ImageButton11" runat="server" CLIENT_LINK_CLICK="ShowCatDiv()"  WIDTH="122px" BUTTON_TEXT="Send Suggestion" />
                            </div>
                        </div>
                      
                        </div>
                        <%--<div class="topDiv" align="right" style="padding-right: 50px;width: 660px;clear: both;padding-top: 10px;">
                            
                            <div style="float: right;">
                                <ctrl:BlueButton ID="ImageButton9" runat="server" WIDTH="76px"  BUTTON_TEXT="Onwards" CLIENT_LINK_CLICK="ShowCatDiv()" />
                            </div>
                            <div style="float: right;padding-right: 10px;">
                                <ctrl:BlueButton ID="CategoryOnwardButton" runat="server" WIDTH="87px" CLIENT_LINK_CLICK="ShowCatDiv()" BUTTON_TEXT="Backwards" />
                            </div>
                         </div>--%>
                    
                    </div>
                   </div>
                </asp:Panel>
                </div>
           <%-- </rad:RadPageView>--%>
            <%--<rad:RadPageView runat="server" ID="RadPageView6" TabIndex="4" >--%>
                <div class="EventDiv" id="Div3" style=" width: 614px; border-top: solid 1px #dedbdb;">
                <asp:Panel runat="server" ID="Panel7">     
                <div></div>     
                   <div style="position: relative;">
                   <script type="text/javascript">
                                                function ShowAllDiv()
                                                {
                                                    var theDiv = document.getElementById('WidthAllDiv');
                                                    var loadDiv = document.getElementById('LoadAllDiv');
                                                    
                                                    loadDiv.style.height = theDiv.offsetHeight +'px';
                                                    loadDiv.style.display = 'block';
                                                    
                                                    return true;
                                                }
                                            </script>
                                            <div id="LoadAllDiv" style="z-index: 10000;width: 670px;display: none;position: absolute;">
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
                    <div id="WidthAllDiv" class="Text12" style="width: 668px; border-left: solid 1px #dedbdb; border-right: solid 1px #dedbdb;">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="conditional">
                        <ContentTemplate>
                        <script type="text/javascript">
                                                function ShowEvDiv()
                                                {
                                                    var theDiv = document.getElementById('WidthEvDiv');
                                                    var loadDiv = document.getElementById('LoadEvDiv');
                                                    
                                                    loadDiv.style.height = theDiv.offsetHeight +'px';
                                                    loadDiv.style.display = 'block';
                                                    
                                                    return true;
                                                }
                                            </script>
                                            <div id="LoadEvDiv" style="z-index: 10000;width: 670px;display: none;position: absolute;">
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
                            <div id="WidthEvDiv">
                            <div style="padding-left: 10px; width: 650px;">
                            <div class="topDiv">
                            <div style="clear: both;">
                                <h1 style="float: left;clear: none;">Optional: </h1><label style="float: left; padding-left: 10px;">hours and regular events will make your local show up on the home page and on the events search page.</label>
                                </div>
                                <div style="float: left;clear: both;">
                                    <h2>Hours</h2>
                                </div>
                            </div>
                            <div style="padding-top: 10px;border-bottom: solid 1px #dedbdb;padding-bottom: 10px;">
                                <div style="font-size: 10px; clear: both;">Select all regular days and hours for your locale. To select multiple days for one entry: hit Ctrl + Click.</div>
                                <div class="topDiv" style="width: 650px;clear: both;">
                                    <div style="float: left; width: 100px; padding-right: 20px;">
                                        <h2>Days</h2>
                                        <asp:ListBox runat="server" Width="80px" ID="DaysListBox" SelectionMode="Multiple">
                                            <asp:ListItem Text="Mon" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Tue" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Wed" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="Thr" Value="4"></asp:ListItem>
                                            <asp:ListItem Text="Fri" Value="5"></asp:ListItem>
                                            <asp:ListItem Text="Sat" Value="6"></asp:ListItem>
                                            <asp:ListItem Text="Sun" Value="7"></asp:ListItem>
                                        </asp:ListBox>
                                        <br />
                                        
                                    </div>
                                    <div style="float: left; width: 100px; padding-right: 40px;">
                                        <h2>Start Time</h2>
                                        <rad:RadTimePicker Skin="Web20" runat="server" ID="StartTimePicker"></rad:RadTimePicker>
                                        <br />
                                        <h2>End Time</h2>
                                        <rad:RadTimePicker Skin="Web20" runat="server" ID="EndTimePicker"></rad:RadTimePicker>
                                    </div>
                                    <div align="center" style="float: left; width: 105px; padding-right: 40px;padding-top: 30px;">
                                         
                                         <div style="width: 101px;">
                                             <ctrl:BlueButton ID="AddHoursButton" WIDTH="102px" runat="server" CLIENT_LINK_CLICK="ShowEvDiv()"
                                             BUTTON_TEXT="Add Hours >>" />
                                         </div>
                                         <div style="width: 89px;">
                                             <ctrl:BlueButton ID="RemoveHoursButton" WIDTH="90px" runat="server" CLIENT_LINK_CLICK="ShowEvDiv()"
                                             BUTTON_TEXT="<< Remove" />
                                         </div>
                                    </div>
                                    <div style="float: left; ">
                                        <h2>Your Chosen Hours:</h2>
                                        <asp:ListBox runat="server" EnableViewState="true" ID="HoursListBox" 
                                          CssClass="ListBox" SelectionMode="single" Width="230px" Height="100px"></asp:ListBox>
                                    </div>
                                </div>
                                <div>
                                    <asp:Label runat="server" ID="HoursErrorLabel" ForeColor="red"></asp:Label>
                                </div>
                            </div>  
                            </div>
                            <div style="padding-left: 10px; width: 650px;">
                            <div class="topDiv">
                                <div style="float: left;">
                                    <h2>Regular Events</h2>
                                </div>
                            </div>
                            <div style="padding-top: 30px;">
                                <div style="font-size: 10px; clear: both;">Select all regular events for your locale (ex: Happy Hour). To select multiple days for one entry: hit Ctrl + Click.</div>

                                <div class="topDiv" style="width: 650px;">
                                    <div style="float: left; width: 100px; padding-bottom: 10px;">
                                        <h2>Event Name</h2>
                                        <asp:TextBox runat="server" ID="RegularEventNameTextBox" Width="80px"></asp:TextBox>

                                        <h2>Days</h2>
                                        <asp:ListBox runat="server" Width="80px" ID="RegularDaysListBox" SelectionMode="Multiple">
                                            <asp:ListItem Text="Mon" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Tue" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Wed" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="Thr" Value="4"></asp:ListItem>
                                            <asp:ListItem Text="Fri" Value="5"></asp:ListItem>
                                            <asp:ListItem Text="Sat" Value="6"></asp:ListItem>
                                            <asp:ListItem Text="Sun" Value="7"></asp:ListItem>
                                        </asp:ListBox>
                                    </div>
                                    <div style="float: left; width: 120px;">
                                        <h2>Start Time</h2>
                                        <rad:RadTimePicker runat="server" Skin="Web20" ID="RadTimePicker1"></rad:RadTimePicker>
                                        <br />
                                        <h2>End Time</h2>
                                        <rad:RadTimePicker runat="server" Skin="Web20" ID="RadTimePicker2"></rad:RadTimePicker>
                                    </div>
                                    <div align="center" style="float: left; width: 130px; padding-top: 30px;">
                                         <div style="width: 97px;">
                                             <ctrl:BlueButton ID="AddEventButton" WIDTH="102px"  runat="server" CLIENT_LINK_CLICK="ShowEvDiv()"
                                             BUTTON_TEXT="Add Event >>" />
                                         </div>
                                         <div style="width: 89px;">
                                             <ctrl:BlueButton ID="RemoveEventButton" WIDTH="90px" runat="server" CLIENT_LINK_CLICK="ShowEvDiv()"
                                             BUTTON_TEXT="<< Remove" />
                                         </div>
                                    </div>
                                    <div style="float: left; ">
                                        <h2>Your Chosen Regular Events:</h2>
                                        <asp:ListBox runat="server" EnableViewState="true" ID="RegularEventsListbox" 
                                          CssClass="ListBox" SelectionMode="single" Width="280px" Height="100px"></asp:ListBox>
                                    </div>
                                </div>
                            </div>  
                                <div>
                                    <asp:Label runat="server" ID="EventsErrorLabel" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                           </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                        <%--<div class="topDiv" align="right" style="padding-right: 50px;width: 660px;clear: both;padding-top: 10px;">
                            
                            <div style="float: right;">
                                <ctrl:BlueButton ID="BlueButton19" CLIENT_LINK_CLICK="ShowAllDiv()" WIDTH="76px"  runat="server" BUTTON_TEXT="Onwards" />
                            </div>
                            <div style="float: right;padding-right: 10px;">
                                <ctrl:BlueButton ID="BlueButton20" CLIENT_LINK_CLICK="ShowAllDiv()" WIDTH="87px" runat="server" BUTTON_TEXT="Backwards" />
                            </div>
                         </div>--%>
                    </div>
                    </div>
                </asp:Panel>
                </div>
            <%--</rad:RadPageView>--%>
            <%--<rad:RadPageView runat="server" ID="RadPageView5" TabIndex="3" >
                
                <asp:Panel runat="server" ID="Panel6" DefaultButton="CategoryOnwardButton">          
                    <div style="position: relative;">
                   <script type="text/javascript">
                                                function ShowFetAllDiv()
                                                {
                                                    var theDiv = document.getElementById('WidthFetAllDiv');
                                                    var loadDiv = document.getElementById('LoadFetAllDiv');
                                                    
                                                    loadDiv.style.height = theDiv.offsetHeight +'px';
                                                    loadDiv.style.display = 'block';
                                                    
                                                    return true;
                                                }
                                            </script>
                                            <div id="LoadFetAllDiv" style="z-index: 10000;width: 670px;display: none;position: absolute;">
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
                    <div id="WidthFetAllDiv" class="Text12" style=" width: 668px; border: solid 1px #dedbdb;">
                    <asp:UpdatePanel ID="FetUpdatePanel" runat="server" UpdateMode="conditional">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="BlueButton10" />
                        </Triggers>
                        <ContentTemplate>
                            <script type="text/javascript">
                                                function ShowFetDiv()
                                                {
                                                    var theDiv = document.getElementById('WidthFetDiv');
                                                    var loadDiv = document.getElementById('LoadFetDiv');
                                                    
                                                    loadDiv.style.height = theDiv.offsetHeight +'px';
                                                    loadDiv.style.display = 'block';
                                                    
                                                    return true;
                                                }
                                            </script>
                                            <div id="LoadFetDiv" style="z-index: 10000;width: 670px;display: none;position: absolute;">
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
                            <div id="WidthFetDiv" style="padding-left: 10px; width: 650px;">
                            <div class="topDiv">
                                <div style="float: left;">
                                    <h1>Want To Feature Your Locale?</h1>
                                </div>
                                <div style="float: right;padding-top: 10px;">
                                    <ctrl:BlueButton ID="BlueButton8" runat="server" CLIENT_CLICK="OpenFeaturePricing('V');"  WIDTH="166px" BUTTON_TEXT="Featured Locale Pricing >" />
                                </div>
                            </div>
                            <div align="center" class="topDiv" style="padding-top: 30px;">
                                <div class="FeaturePrice">
                                    <h2>It's only $<asp:Literal runat="server" ID="PriceLiteral"></asp:Literal> / day!</h2>
                                </div>
                                Your featured locale will come up first in our locale search results on the locale search page. In
                                addition, any regular happenings (happy hour, Friday dance night, etc. ) happening at the locale will be
                                featured on our home page 'Stuff To Do' strip. You can also feature a specific event at a locale; to do this
                                complete the creation of your locale and then go to 'Add Event'.
                                <%--<div class="PostFreeDiv" style="padding: 20px;">
                                    <h2><asp:Panel runat="server" ID="PricePanel"></asp:Panel></h2>
                                </div>
                            </div>
                             <div align="center" class="topDiv"  style="padding-top: 10px; padding-bottom: 30px;">
                                <div style="width: 106px;">
                                    <ctrl:BlueButton ID="BlueButton9" CLIENT_LINK_CLICK="ShowFetDiv()" runat="server" WIDTH="107px" BUTTON_TEXT="Yes, Definitely!" />
                                </div>
                                <div style="width: 106px;">
                                    <ctrl:BlueButton ID="BlueButton10" CLIENT_LINK_CLICK="ShowFetDiv()" runat="server" WIDTH="107px" BUTTON_TEXT="No, Thank You" />
                                </div>
                             </div>   
                            <asp:Panel runat="server" ID="FeaturePanel" Visible="false">
                                <div>
                                    <div class="topDiv FooterBottom">
                                       <asp:Panel runat="server" ID="MajorPanel">
                                           <b><div>Your major city is <asp:Label runat="server" ForeColor="Red" ID="MajorCity"></asp:Label>. If this is not correct,
                                            make sure the zip code you provided for the location is correct.</div></b><br /><br />
                                        </asp:Panel>
                                        <div style="float: left;">
                                            <h2>Pick the dates you want your locale featured</h2>
                                        </div>
                                        <div style="clear: both; padding-bottom: 5px;">
                                            *Days on which your locale has already been featured show up in gray below.
                                            <asp:Label runat="server" ID="FeatureErrorLabel" ForeColor="red"></asp:Label>
                                        </div>
                                    </div>
                                        <div class="topDiv" style="border-top: solid 1px #dedbdb; width: 614px; padding-top: 10px;">
                                            <div style="float: left; width: 215px;">
                                                
                                                <rad:RadDatePicker runat="server" ID="FeaturedDatePicker" Skin="Web20"></rad:RadDatePicker><br />
                                                <div style="padding-top: 5px;">[7 days max, for next 30 days]</div>
                                            </div>
                                            <div align="center" style="float: left; width: 130px; padding-top: 50px;">
                                                 
                                                    <ctrl:BlueButton ID="BlueButton1" runat="server" WIDTH="95px" CLIENT_LINK_CLICK="ShowFetDiv()" BUTTON_TEXT="Add Date >>" />
                                                 
                                                     <ctrl:BlueButton ID="BlueButton4" WIDTH="90px" runat="server" CLIENT_LINK_CLICK="ShowFetDiv()"
                                                     BUTTON_TEXT="<< Remove" />
                                                
                                            </div>
                                            <div style="float: left; ">
                                                <h2>Your Dates:</h2>
                                                            <asp:ListBox runat="server" EnableViewState="true" ID="FeatureDatesListBox" 
                                                              CssClass="ListBox" SelectionMode="single" Width="250px" Height="100px"></asp:ListBox>
                                            </div>
                                        </div>
                            
                                </div>
                                 <div>
                                    <div class="topDiv">
                                        <div style="float: left;">
                                            <h2>Pick Your Search Terms</h2>
                                        </div>
                                        <div style="float: left; padding-left: 5px;padding-top: 4px;">
                                            <asp:Image CssClass="HelpImage" runat="server"  ID="Image6" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
                                            <rad:RadToolTip ID="RadToolTip7" runat="server" Skin="Sunset" 
                                            Width="300px" ManualClose="true" ShowEvent="onclick" 
                                            Position="MiddleRight" RelativeTo="Element" TargetControlID="Image6">
                                            <div style="padding: 10px;line-height: 20px;"><label>Selecting no search terms will mean your locale will pop 
                                            up in searches where no keywords are entered. If you select any keywords, your featured 
                                            locale will pop up in searches where those keywords are entered 
                                            and in searches where no keywords are entered as well. Each keyword is limited to four locales, 
                                            and so, picking a 
                                            keyword guarantees that your locale will show up in the top four results of a search with your keyword. 
                                            In the searches without keywords, your locale will show up in the top four results chosen randomly 
                                            per each time searched. Meaning, if you do not include search terms your locale is <u>not</u> 
                                            guaranteed to show up in the first four results if there is over 4 featured locales for your location.<br /><br />
                                            If this all sounds too complicated, strictly speaking, it is better to put in search terms!</label></div>
                                            </rad:RadToolTip>
                                        </div>
                                    </div>
                                        <div class="topDiv" style="border-top: solid 1px #dedbdb; width: 614px; padding-top: 10px;">
                                            <div style="float: left; width: 215px;">
                                                <asp:TextBox runat="server" ID="TermsBox"></asp:TextBox>
                                            </div>
                                            <div align="center" style="float: left; width: 130px; padding-top: 50px;">
                                                 <ctrl:BlueButton ID="BlueButton5" WIDTH="97px" runat="server" CLIENT_LINK_CLICK="ShowFetDiv()" BUTTON_TEXT="Add Term >>" />
                                                 <ctrl:BlueButton ID="BlueButton6" WIDTH="90px" runat="server" CLIENT_LINK_CLICK="ShowFetDiv()"
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
                                                <div style="padding: 10px;"><label>Your price depends on the number of members in the location you are featuring the locale,
                                                number of days, number of search terms, and number of existing featured locales on the days you 
                                                have chosen. The more featured locales, the less it costs to feature a locale.</label></div>
                                            </rad:RadToolTip>
                                        </div>
                                    </div>
                                    
                                    <table align="center" style="border: solid 1px #dedbdb;" width="100%">
                                        <tr>
                                            <th>Day</th>
                                       
                                            <th>Standard Rate / day</th>
                                        
                                            <th>Adjustment for Members / day</th>
                                        
                                            <th>Adjustment for Locales / day</th>
                                     
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
                                        <ctrl:BlueButton ID="BlueButton7" BUTTON_TEXT="Update" runat="server" CLIENT_LINK_CLICK="ShowFetDiv()" />
                                    </div>
                                </div>
                                
                            </asp:Panel>
                        </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                        <div class="topDiv" align="right" style="padding-right: 50px;width: 660px;clear: both;padding-top: 10px;">
                            
                            <div style="float: right;">
                                <ctrl:BlueButton ID="BlueButton2" runat="server" CLIENT_LINK_CLICK="ShowFetAllDiv()" WIDTH="76px"  BUTTON_TEXT="Onwards" />
                            </div>
                            <div style="float: right;padding-right: 10px;">
                                <ctrl:BlueButton ID="BlueButton3" runat="server" CLIENT_LINK_CLICK="ShowFetAllDiv()" WIDTH="87px" BUTTON_TEXT="Backwards" />
                            </div>
                         </div>
                    </div>
                    </div>
                </asp:Panel>
            </rad:RadPageView>--%>
            <%--<rad:RadPageView runat="server" ID="RadPageView4" TabIndex="4" >--%>
            <div class="EventDiv" id="Div4" style=" width: 614px; border-top: solid 1px #dedbdb;">
                <asp:Panel runat="server" ID="Panel5" DefaultButton="PostItButton"> 
                   <div style="position: relative;">
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
                   <div id="WidthPostDiv">
                    <div class="topDiv" style="width: 668px; border-left: solid 1px #dedbdb; border-right: solid 1px #dedbdb; border-bottom: solid 1px #dedbdb;">
                    <div style="padding-left: 10px;">
                        <%--<h1 class="SideColumn">You're Almost Done!</h1>
                            <div style="width: 640px;padding-left: 10px;">
                                Preview your post information below and read the Terms 
                                and Conditions. Go backwards if you'd like to change anything. 
                                <%--If you decided to feature your locale, 
                                please fill in the payment information.
                            </div>--%>
                        <div>

                        <div style="width: 640px;padding-left: 10px;">
                         <asp:Panel runat="server" ID="OwnerPanel">
                            <div style="padding-bottom: 10px; padding-top: 10px;">
                            <asp:CheckBox runat="server" ID="OwnerCheckBox" Text="Check here if you would like to be the 'Owner' of this locale."  />

                            <asp:Image CssClass="HelpImage" runat="server"  ID="QuestionMark6" ImageUrl="NewImages/HelpIconNew.png"></asp:Image>
                                                                           <rad:RadToolTip ID="RadToolTip5" runat="server" Skin="Sunset" Width="200px" Height="200px" ManualClose="true" ShowEvent="onclick" Position="MiddleRight" RelativeTo="Element" TargetControlID="QuestionMark6">
                                    <div style="padding: 10px;"><label>
                                    When you become the owner of this locale, you will be 
                                    responsible for approving any change request users post before the 
                                    changes go live. Being the owner, you have 4 days to approve/reject the changes that 
                                    are submitted by other users. If you do not reply, your ownership 
                                    will be forfeited and this locale's ownership will be up for grabs to everyone.
                                    You do not have to be the 'Owner' of this locale, 
                                    but keep in mind that anyone will be able to freely edit this locale 
                                    until someone chooses to become the owner.
                                    </label></div>
                                    </rad:RadToolTip>
                            </div>
                         </asp:Panel>
                         </div>
                            <%--<asp:Panel runat="server" ID="PaymentPanel" Visible="false">
                                    <div style="border: solid 1px #dedbdb;clear: both;margin-top: 10px;margin-bottom: 30px;margin-right: 10px; padding: 10px; background-color: #eef7fb;">
                                        <h1 class="SideColumn">Payment Information</h1>
                                        <div align="center">
                                        <div align="center" style="width: 450px;color: #535252;padding: 5px; background-color: #ebe7e7; border: solid 1px #09718F;">
                                            <span class="Asterisk">*</span>Your payment will be charged when you click 'Post It' on the next tab.<br />
                                            <span class="Asterisk">*</span>HippoHappenings does not store any credit card information in our systems.
                                        </div>
                                        </div>
                                        <div>
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
                                                                        <asp:Image CssClass="HelpImage" runat="server"  ID="Image8" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
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
                                </asp:Panel>--%>
                                <asp:Label runat="server" ID="PriceErrorLabel" ForeColor="red"></asp:Label>
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
                        <div class="topDiv" align="right" style="padding-right: 50px;width: 650px;">
                            
                            <div style="float: right;">
                                <ctrl:BlueButton ID="PostItButton" runat="server" CLIENT_LINK_CLICK="ShowPostDiv()" WIDTH="60px" BUTTON_TEXT="Post It" />
                            </div>
                            <%--<div style="float: right;padding-right: 10px;">
                                <ctrl:BlueButton ID="ImageButton12" runat="server" CLIENT_LINK_CLICK="ShowPostDiv()" WIDTH="87px" BUTTON_TEXT="Backwards" />
                            </div>--%>
                         </div>
                    </div>
                    </div>
                    </div>
                    </div>
                </asp:Panel>
                </div>
             <%--</rad:RadPageView>

        </rad:RadMultiPage>--%>
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
    </div>
    </asp:Panel>
    

</asp:Content>

