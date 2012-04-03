<%@ Page Language="C#" MasterPageFile="~/SecondMasterPage.master" Inherits="EnterTrip" 
ValidateRequest="false" EnableEventValidation="false" AutoEventWireup="true" Title="Enter an Adventure | HippoHappenings"  
CodeFile="EnterTrip.aspx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register Src="~/Controls/SmallButton.ascx" TagName="SmallButton" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/BlueButton.ascx" TagName="BlueButton" TagPrefix="ctrl" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<rad:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="false" 
                KeepInScreenBounds="true" VisibleOnPageLoad="false"  Behaviors="none"
            ID="MessageRadWindowManager" Modal="true" RestrictionZoneID="RestrictionZone"
            runat="server">
                <Windows>
                    <rad:RadWindow Width="300" ClientCallBackFunction="OnClientClose" 
                    VisibleStatusbar="false" Skin="Web20" Height="350" ID="MessageRadWindow" 
                     runat="server">
                    </rad:RadWindow>
                    <rad:RadWindow Width="600"
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
              
<%--<rad:RadAjaxPanel runat="server">--%>
 
<rad:RadProgressManager ID="Radprogressmanager1" runat="server" EnableEmbeddedBaseStylesheet="False" EnableEmbeddedSkins="False" UniquePageIdentifier="112b806f-5e25-4f58-b795-09385fad96e5" />


    <div>
        
        <div class="topDiv">
            <div style="float: left;">
                <asp:Label runat="server" ID="nameLabel" Text="<h1>Enter an Adventure</h1><br/>"></asp:Label>
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
        <asp:Label runat="server" ID="isEdit" Visible="false"></asp:Label>
        <asp:Label runat="server" ID="eventID" Visible="false"></asp:Label>
    </div>
    <div id="topDiv6">
        <div style="float: left; width: 670px;">
        <div>
<%--            <rad:RadTabStrip runat="server" ID="EventTabStrip" Height="27px"
            Skin="HippoHappenings" CssClass="EventTabStrip" 
            EnableEmbeddedSkins="false" OnTabClick="TabClick"  MultiPageID="BlogEventPages" SelectedIndex="0">
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
                    ImageUrl="NewImages/FeatureFiveFull.png" 
                    HoveredImageUrl="NewImages/FeatureFiveFull.png"  TabIndex="4" 
                    DisabledImageUrl="NewImages/FeatureFiveEmpty.png" 
                    SelectedImageUrl="NewImages/FeatureFiveFull.png"  ></rad:RadTab>
                    <rad:RadTab runat="server"  Enabled="false"  PageViewID="RadPageView5" 
                    ImageUrl="NewImages/PostItSixFull.png" 
                    HoveredImageUrl="NewImages/PostItSixFull.png" 
                    DisabledImageUrl="NewImages/PostItSixEmpty.png"  TabIndex="5" 
                    SelectedImageUrl="NewImages/PostItSixFull.png"  ></rad:RadTab>
                </Tabs>
            </rad:RadTabStrip>
        </div>
        <rad:RadMultiPage runat="server" ID="BlogEventPages" SelectedIndex="0">
            <rad:RadPageView runat="server" ID="TabOne" TabIndex="0" >--%>
                <asp:Panel runat="server" ID="Panel1">
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
                    <div id="WidthDetailsDiv" class="topDiv" style="width: 668px; border-left: solid 1px #dedbdb; border-right: solid 1px #dedbdb; border-top: solid 1px #dedbdb;">
                        <div style="padding-left: 10px;">
                            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                <script type="text/javascript">
                                                function ShowUpDiv()
                                                {
                                                    var theDiv = document.getElementById('WidthUpDiv');
                                                    var loadDiv = document.getElementById('LoadUpDiv');
                                                    
                                                    loadDiv.style.height = theDiv.offsetHeight +'px';
                                                    loadDiv.style.display = 'block';
                                                    
                                                    return true;
                                                }
                                            </script>
                                            <div id="LoadUpDiv" style="margin-left: -10px;z-index: 10000;width: 670px;display: none;position: absolute;">
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
                                            <div id="WidthUpDiv">
                            <table width="100%">
                                <tr>
                                    <td valign="top" width="250px" style="padding-bottom: 20px;">
                                            <script type="text/javascript">
                                                function CountCharsHeadline(editor, e){
                                                    
                                                        var theDiv = document.getElementById("CharsDivHeadline");
                                                        var theText = document.getElementById("ctl00_ContentPlaceHolder1_EventNameTextBox");
                                                        theDiv.innerHTML = "characters left: "+ (70 - theText.value.length).toString();
                                                }
                                            </script>
                                        <h2>Adventure Title<span class="Asterisk">* </span></h2>
                                        <span class="NavyLink12">max 70 characters
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <span style="color: Red;" id="CharsDivHeadline"></span></span><br />
                                        <asp:TextBox runat="server" onkeypress="CountCharsHeadline(event)" Width="240" 
                                        ID="EventNameTextBox"></asp:TextBox>
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <td valign="top" style="border-top: solid 1px #dedbdb;padding-bottom: 20px;">
                                        <div class="topDiv">
                                            <div style="float: left;">
                                                <h2>Directions<span class="Asterisk">*</span></h2>
                                            </div>
                                            <div style="float: left; padding-left: 5px;padding-top: 7px;">
                                                <asp:Image CssClass="HelpImage" runat="server"  ID="Image2" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
                                                <rad:RadToolTip ID="RadToolTip2" runat="server" Skin="Sunset"
                                                Width="200px" ManualClose="true" ShowEvent="onclick" 
                                                Position="MiddleRight" RelativeTo="Element" TargetControlID="Image2">
                                                <div style="padding: 10px;"><label>Directions are the most important description for an adventure. 
                                                Provide as much detail about the location as possible for each 
                                                step on the adventure. We will plot your directions systematically 
                                                from one point to the second point, second to third, etc. If you include only one
                                                address location, we'll map it as a single point with no 
                                                directions.</label></div>
                                                </rad:RadToolTip>
                                            </div>
                                        </div>
                                        <table>
                                            <tr>
                                                <td valign="top"><h2>Description for this step<span class="Asterisk">*</span></h2>
                                                <asp:TextBox runat="server" ID="DirectionTextBox" TextMode="multiLine" 
                                                Height="50px" Width="200px"></asp:TextBox>
                                                    <br />
                                                    <h2>Means of Getting <span style="text-decoration: underline;">To</span> this Step from The Previous<span class="Asterisk">*</span></h2>
                                                    
                                                    <div class="topDiv">
                                                    <div style="float: left;">
                                                        <asp:DropDownList runat="server" ID="WalkingDropDown">
                                                        <asp:ListItem Text="Walking" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Driving" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    </div>
                                                    <div style="float: left; padding-left: 5px;">
                                                        <asp:Image CssClass="HelpImage" runat="server"  ID="Image9" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
                                                        <rad:RadToolTip ID="RadToolTip5" runat="server" Skin="Sunset"
                                                        Width="200px" ManualClose="true" ShowEvent="onclick" 
                                                        Position="MiddleRight" RelativeTo="Element" TargetControlID="Image9">
                                                        <div style="padding: 10px;"><label>Select the means of getting to this step from the previous step. For the first step of the trip,
                                                        this choice will not matter since the viewers of your adventure page will be coming from different locations. 
                                                        We suggest including all your steps here and then, after you post your adventure, view
                                                        the google map and google directions we have plotted for you. If the means of getting around do not match what you expected, please
                                                        feel free to edit the adventure.</label></div>
                                                        </rad:RadToolTip>
                                                    </div>
                                                </div>
                                                    <br />
                                                    <h2>Location<span class="Asterisk">*</span></h2>
                                                    <table style="border: solid 1px #dedbdb;">
                                                        <tr>
                                                <td>
                                                    <h2>Country<span class="Asterisk">*</span></h2>
                                                    <asp:DropDownList OnSelectedIndexChanged="ChangeState" AutoPostBack="true" runat="server" 
                                                    DataSourceID="SqlDataSourceCountry" DataTextField="country_name" DataValueField="country_id" 
                                                    ID="CountryDropDown"></asp:DropDownList>
                                                    <asp:SqlDataSource runat="server" ID="SqlDataSourceCountry" SelectCommand="SELECT * FROM Countries"
                                                    ConnectionString="<%$ ConnectionStrings:Connection %>"></asp:SqlDataSource>
                                                </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table cellpadding="0" cellspacing="0">
                                            
                                                            <asp:Panel runat="server" ID="USPanel">
                                                            <tr>
                                                            <td colspan="2">
                                                                <table width="100%" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <h2>Street No.<span class="Asterisk">*</span></h2>
                                                                        </td>
                                                                        <td style="padding-left: 3px;">
                                                                            <h2>Street Name<span class="Asterisk">*</span></h2>
                                                                        </td>
                                                                        <td style="padding-left: 3px;">
                                                                           
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
                                                            </tr>
                                                            </asp:Panel>
                                                           
                                                        
                                                         <asp:Panel runat="server" ID="InternationalPanel" Visible="false">
                                                           <tr>
                                                            <td colspan="2">
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <h2>Location<span class="Asterisk">*</span></h2>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="LocationTextBox" Width="200px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            </tr>
                                                            </asp:Panel>
                                                            <tr>
                                                                        <td colspan="2">
                                                                            <div style="float: left; padding-right: 10px;"><h2>Suite/Apt</h2></div>
                                                                            <div style="float: left;"><h2>Apt No.</h2></div>
                                                                        </td>
                                                                    </tr>
                                                                   <tr>
                                                                        <td colspan="2">
                                                                            <asp:DropDownList runat="server" ID="AptDropDown">
                                                                                <asp:ListItem Text="Suite" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="Apt" Value="2"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <asp:TextBox runat="server" ID="AptNumberTextBox" Width="50px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                            <tr>
                                                            <td colspan="2">
                                                                <h2>State<span class="Asterisk">*</span></h2>
                                                                <asp:Panel runat="server" ID="StateTextBoxPanel">
                                                                    <asp:TextBox ID="StateTextBox" runat="server" Width="100px" ></asp:TextBox>
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
                                                                            <h2>City<span class="Asterisk">*</span></h2>
                                                                        </td>
                                                                        <td style="padding-left: 3px;">
                                                                            <h2>Zip<span class="Asterisk">*</span></h2>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="VenueCityTextBox" runat="server" Width="100px" ></asp:TextBox>
                                                                        </td>
                                                                        <td style="padding-left: 3px;">
                                                                            <asp:TextBox ID="ZipTextBox" runat="server" Width="100px" ></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                                </td>
                                                            </tr>
                                                          
                                                    </table>
                                                </td>
                                                <td>
                                                    <div align="center" style="float: left; width: 130px;">
                                                     
                                                        <ctrl:BlueButton ID="BlueButton11" runat="server" CLIENT_LINK_CLICK="ShowUpDiv()" WIDTH="125px" BUTTON_TEXT="Add Directions >>" />
                                                         <ctrl:BlueButton ID="BlueButton12" runat="server" CLIENT_LINK_CLICK="ShowUpDiv()" WIDTH="100px"
                                                         BUTTON_TEXT="<< Remove" />
                                                         <br /><br />
                                                         <ctrl:BlueButton ID="BlueButton13" runat="server" WIDTH="65px" CLIENT_LINK_CLICK="ShowUpDiv()"
                                                         BUTTON_TEXT="<< Edit" />
                                                         <ctrl:BlueButton ID="BlueButton14" runat="server" WIDTH="79px" CLIENT_LINK_CLICK="ShowUpDiv()"
                                                         BUTTON_TEXT="Modify >>" />
                                                        <asp:Label runat="server" ID="EditLabel" Visible="false"></asp:Label>
                                                    </div>
                                                </td>
                                                <td valign="top">
                                                    <h2>Your Directions</h2><br />
                                                    <asp:ListBox runat="server" ID="DirectionsListBox" Width="200px" 
                                                    Height="300px"></asp:ListBox>
                                                    <asp:Label runat="server" ID="DirectionsErrorLabel" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td  valign="top" style="border-top: solid 1px #dedbdb;padding-bottom: 20px;">
                                        <div class="topDiv">
                                            <div style="float: left;">
                                                <h2>Means of Getting Around<span class="Asterisk">*</span></h2>
                                            </div>
                                            <div style="float: left; padding-left: 5px;padding-top: 7px;">
                                                <asp:Image CssClass="HelpImage" runat="server"  ID="Image3" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
                                                <rad:RadToolTip ID="RadToolTip3" runat="server" Skin="Sunset"
                                                Width="200px" ManualClose="true" ShowEvent="onclick" 
                                                Position="MiddleRight" RelativeTo="Element" TargetControlID="Image3">
                                                <div style="padding: 10px;"><label>Check off all means of getting around that this adventure will entail. 
                                                This feature will help your viewers find your adventure based on their desire
                                                of transportation.</label></div>
                                                </rad:RadToolTip>
                                            </div>
                                        </div>
                                        <asp:CheckBoxList RepeatDirection="horizontal" runat="server" ID="MeansCheckList">
                                            <asp:ListItem Text="Car" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Walking" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Hiking" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="Biking" Value="4"></asp:ListItem>
                                            <asp:ListItem Text="Flying" Value="5"></asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-top: solid 1px #dedbdb;padding-bottom: 20px;">
                                        <div class="topDiv">
                                            <div style="float: left;">
                                                <h2>Durations of Your Adventure<span class="Asterisk">*</span></h2>
                                            </div>
                                            <div style="float: left; padding-left: 5px;padding-top: 7px;">
                                                <asp:Image CssClass="HelpImage" runat="server"  ID="Image8" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
                                                <rad:RadToolTip ID="RadToolTip9" runat="server" Skin="Sunset"
                                                Width="200px" ManualClose="true" ShowEvent="onclick" 
                                                Position="MiddleRight" RelativeTo="Element" TargetControlID="Image8">
                                                <div style="padding: 10px;"><label>Enter the maximum and 
                                                minimum amount of time your adventure could take.</label></div>
                                                </rad:RadToolTip>
                                            </div>
                                        </div>
                                        <table>
                                            <tr>
                                                <td style="padding-right: 20px;">
                                                    <label>Min Time</label> <span class="Asterisk">*</span><br />
                                                    <label>Hours:</label>&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox runat="server" ID="MinHoursTextBox"></asp:TextBox><br />
                                                    <label>Minutes: </label><asp:TextBox runat="server" ID="MinMinsTextBox"></asp:TextBox>
                                                    
                                                </td>
                                                <td>
                                                    <label>Max Time</label> <span class="Asterisk">*</span><br />
                                                    <label>Hours:</label> &nbsp;&nbsp;&nbsp;<asp:TextBox runat="server" ID="MaxHoursTextBox"></asp:TextBox><br />
                                                    <label>Minutes:</label> <asp:TextBox runat="server" ID="MaxMinsTextBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-top: solid 1px #dedbdb;padding-bottom: 20px;">
                                        <div class="topDiv">
                                            <div style="float: left;">
                                                <h2>Time Of Day & Days of the Week<span class="Asterisk">*</span></h2>
                                            </div>
                                            <div style="float: left; padding-left: 5px;padding-top: 7px;">
                                                <asp:Image CssClass="HelpImage" runat="server"  ID="Image11" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
                                                <rad:RadToolTip ID="RadToolTip12" runat="server" Skin="Sunset"
                                                Width="200px" ManualClose="true" ShowEvent="onclick" 
                                                Position="MiddleRight" RelativeTo="Element" TargetControlID="Image11">
                                                <div style="padding: 10px;"><label>Enter all days of the week and times 
                                                of day anyone can take this trip.</label></div>
                                                </rad:RadToolTip>
                                            </div>
                                        </div>
                                        <table width="100%">
                                            <tr>
                                                <td valign="top" width="40%">
                                                    <table>
                                                        <tr>
                                                            <td style="padding-right: 20px;">
                                                                <label>Day of The Week:</label><br />
                                                                <asp:ListBox runat="server" ID="DaysOfWeekListBox" SelectionMode="multiple">
                                                                    <asp:ListItem Value="1" Text="Monday"></asp:ListItem>
                                                                    <asp:ListItem Value="2" Text="Tuesday"></asp:ListItem>
                                                                    <asp:ListItem Value="3" Text="Wednesday"></asp:ListItem>
                                                                    <asp:ListItem Value="4" Text="Thursday"></asp:ListItem>
                                                                    <asp:ListItem Value="5" Text="Friday"></asp:ListItem>
                                                                    <asp:ListItem Value="6" Text="Saturday"></asp:ListItem>
                                                                    <asp:ListItem Value="7" Text="Sunday"></asp:ListItem>
                                                                </asp:ListBox>
                                                            </td>
                                                            <td>
                                                                <label>Start Time </label><span class="Asterisk">*</span><br />
                                                                <rad:RadTimePicker runat="server" ID="StartTimePicker"></rad:RadTimePicker><br />
                                                                <label>End Time </label><span class="Asterisk">*</span><br />
                                                                <rad:RadTimePicker runat="server" ID="EndTimePicker"></rad:RadTimePicker>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    
                                                </td>
                                                <td width="%20">
                                                    <div align="center" style="float: left; width: 130px;">
                                                     
                                                        <ctrl:BlueButton ID="BlueButton15" runat="server" WIDTH="96px" CLIENT_LINK_CLICK="ShowUpDiv()" BUTTON_TEXT="Add Time >>" />
                                                         <ctrl:BlueButton ID="BlueButton16" runat="server" WIDTH="90px" CLIENT_LINK_CLICK="ShowUpDiv()" 
                                                         BUTTON_TEXT="<< Remove" />
                                                    </div>
                                                </td>
                                                <td valign="top" width="%40">
                                                    <h2>Your Adventure's Days and Times</h2>
                                                    <asp:ListBox runat="server" ID="TimeListBox" Width="250px" 
                                                    Height="50px"></asp:ListBox><br />
                                                    <asp:Label runat="server" ID="TimeErrorLabel" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-top: solid 1px #dedbdb;padding-bottom: 20px;">
                                        <div class="topDiv">
                                            <div style="float: left;">
                                                <h2>Months Time Frame<span class="Asterisk">*</span></h2>
                                            </div>
                                            <div style="float: left; padding-left: 5px;padding-top: 7px;">
                                                <asp:Image CssClass="HelpImage" runat="server"  ID="Image10" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
                                                <rad:RadToolTip ID="RadToolTip11" runat="server" Skin="Sunset"
                                                Width="200px" ManualClose="true" ShowEvent="onclick" 
                                                Position="MiddleRight" RelativeTo="Element" TargetControlID="Image10">
                                                <div style="padding: 10px;"><label>Enter all the time frames of the year 
                                                this trip can be taken.</label></div>
                                                </rad:RadToolTip>
                                            </div>
                                        </div>
                                        <br />
                                        <table width="100%">
                                            <tr>
                                                <td valign="top" width="35%">
                                                    <table>
                                                        <tr>
                                                            <td style="padding-right: 20px;">
                                                                <label>Start Month:</label><br />
                                                                <asp:DropDownList runat="server" ID="MonthDropDown">
                                                                    <asp:ListItem Value="1" Text="January"></asp:ListItem>
                                                                    <asp:ListItem Value="2" Text="February"></asp:ListItem>
                                                                    <asp:ListItem Value="3" Text="March"></asp:ListItem>
                                                                    <asp:ListItem Value="4" Text="April"></asp:ListItem>
                                                                    <asp:ListItem Value="5" Text="May"></asp:ListItem>
                                                                    <asp:ListItem Value="6" Text="June"></asp:ListItem>
                                                                    <asp:ListItem Value="7" Text="July"></asp:ListItem>
                                                                    <asp:ListItem Value="8" Text="August"></asp:ListItem>
                                                                    <asp:ListItem Value="9" Text="September"></asp:ListItem>
                                                                    <asp:ListItem Value="10" Text="October"></asp:ListItem>
                                                                    <asp:ListItem Value="11" Text="November"></asp:ListItem>
                                                                    <asp:ListItem Value="12" Text="December"></asp:ListItem>
                                                                </asp:DropDownList><br /><br />
                                                                <label>End Month:</label><br />
                                                                <asp:DropDownList runat="server" ID="EndMonthDropDown">
                                                                    <asp:ListItem Value="1" Text="January"></asp:ListItem>
                                                                    <asp:ListItem Value="2" Text="February"></asp:ListItem>
                                                                    <asp:ListItem Value="3" Text="March"></asp:ListItem>
                                                                    <asp:ListItem Value="4" Text="April"></asp:ListItem>
                                                                    <asp:ListItem Value="5" Text="May"></asp:ListItem>
                                                                    <asp:ListItem Value="6" Text="June"></asp:ListItem>
                                                                    <asp:ListItem Value="7" Text="July"></asp:ListItem>
                                                                    <asp:ListItem Value="8" Text="August"></asp:ListItem>
                                                                    <asp:ListItem Value="9" Text="September"></asp:ListItem>
                                                                    <asp:ListItem Value="10" Text="October"></asp:ListItem>
                                                                    <asp:ListItem Value="11" Text="November"></asp:ListItem>
                                                                    <asp:ListItem Value="12" Text="December"></asp:ListItem>
                                                                </asp:DropDownList><br />
                                                            </td>
                                                            <td>
                                                                <label>Start Day:</label><br />
                                                                <asp:DropDownList runat="server" ID="StartDayDropDown">
                                                                    <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                                    <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                                    <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                                    <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                                    <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                                                    <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                                                    <asp:ListItem Value="7" Text="7"></asp:ListItem>
                                                                    <asp:ListItem Value="8" Text="8"></asp:ListItem>
                                                                    <asp:ListItem Value="9" Text="9"></asp:ListItem>
                                                                    <asp:ListItem Value="10" Text="10"></asp:ListItem>
                                                                    <asp:ListItem Value="11" Text="11"></asp:ListItem>
                                                                    <asp:ListItem Value="12" Text="12"></asp:ListItem>
                                                                    <asp:ListItem Value="13" Text="13"></asp:ListItem>
                                                                    <asp:ListItem Value="14" Text="14"></asp:ListItem>
                                                                    <asp:ListItem Value="15" Text="15"></asp:ListItem>
                                                                    <asp:ListItem Value="16" Text="16"></asp:ListItem>
                                                                    <asp:ListItem Value="17" Text="17"></asp:ListItem>
                                                                    <asp:ListItem Value="18" Text="18"></asp:ListItem>
                                                                    <asp:ListItem Value="19" Text="19"></asp:ListItem>
                                                                    <asp:ListItem Value="20" Text="20"></asp:ListItem>
                                                                    <asp:ListItem Value="21" Text="21"></asp:ListItem>
                                                                    <asp:ListItem Value="22" Text="22"></asp:ListItem>
                                                                    <asp:ListItem Value="23" Text="23"></asp:ListItem>
                                                                    <asp:ListItem Value="24" Text="24"></asp:ListItem>
                                                                    <asp:ListItem Value="25" Text="25"></asp:ListItem>
                                                                    <asp:ListItem Value="26" Text="26"></asp:ListItem>
                                                                    <asp:ListItem Value="27" Text="27"></asp:ListItem>
                                                                    <asp:ListItem Value="28" Text="28"></asp:ListItem>
                                                                    <asp:ListItem Value="29" Text="29"></asp:ListItem>
                                                                    <asp:ListItem Value="30" Text="30"></asp:ListItem>
                                                                    <asp:ListItem Value="31" Text="31"></asp:ListItem>
                                                                </asp:DropDownList><br /><br />
                                                                <label>End Day:</label><br />
                                                                <asp:DropDownList runat="server" ID="EndDayDropDown">
                                                                    <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                                    <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                                    <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                                    <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                                    <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                                                    <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                                                    <asp:ListItem Value="7" Text="7"></asp:ListItem>
                                                                    <asp:ListItem Value="8" Text="8"></asp:ListItem>
                                                                    <asp:ListItem Value="9" Text="9"></asp:ListItem>
                                                                    <asp:ListItem Value="10" Text="10"></asp:ListItem>
                                                                    <asp:ListItem Value="11" Text="11"></asp:ListItem>
                                                                    <asp:ListItem Value="12" Text="12"></asp:ListItem>
                                                                    <asp:ListItem Value="13" Text="13"></asp:ListItem>
                                                                    <asp:ListItem Value="14" Text="14"></asp:ListItem>
                                                                    <asp:ListItem Value="15" Text="15"></asp:ListItem>
                                                                    <asp:ListItem Value="16" Text="16"></asp:ListItem>
                                                                    <asp:ListItem Value="17" Text="17"></asp:ListItem>
                                                                    <asp:ListItem Value="18" Text="18"></asp:ListItem>
                                                                    <asp:ListItem Value="19" Text="19"></asp:ListItem>
                                                                    <asp:ListItem Value="20" Text="20"></asp:ListItem>
                                                                    <asp:ListItem Value="21" Text="21"></asp:ListItem>
                                                                    <asp:ListItem Value="22" Text="22"></asp:ListItem>
                                                                    <asp:ListItem Value="23" Text="23"></asp:ListItem>
                                                                    <asp:ListItem Value="24" Text="24"></asp:ListItem>
                                                                    <asp:ListItem Value="25" Text="25"></asp:ListItem>
                                                                    <asp:ListItem Value="26" Text="26"></asp:ListItem>
                                                                    <asp:ListItem Value="27" Text="27"></asp:ListItem>
                                                                    <asp:ListItem Value="28" Text="28"></asp:ListItem>
                                                                    <asp:ListItem Value="29" Text="29"></asp:ListItem>
                                                                    <asp:ListItem Value="30" Text="30"></asp:ListItem>
                                                                    <asp:ListItem Value="31" Text="31"></asp:ListItem>
                                                                </asp:DropDownList><br />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td width="25%">
                                                    <div align="center" style="float: left; width: 140px;">
                                                     
                                                        <ctrl:BlueButton ID="BlueButton17" runat="server" WIDTH="140px" CLIENT_LINK_CLICK="ShowUpDiv()"
                                                        BUTTON_TEXT="Add Month Frame >>" />
                                                         <ctrl:BlueButton ID="BlueButton18" runat="server" WIDTH="90px" CLIENT_LINK_CLICK="ShowUpDiv()"
                                                         BUTTON_TEXT="<< Remove" />
                                                    </div>
                                                </td>
                                                <td valign="top" width="40%">
                                                    <h2>Your Adventure's Month Time Frames</h2>
                                                    <asp:ListBox runat="server" ID="MonthsListBox" Width="250px" 
                                                    Height="50px"></asp:ListBox>
                                                    <asp:Label runat="server" ID="MonthsErrorLabel" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        
                    </div>
                  </div>
                </asp:Panel>
            <%--</rad:RadPageView>
            <rad:RadPageView runat="server" ID="RadPageView1" TabIndex="1" >--%>
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
                    <div id="WidthDesDiv" class="topDiv" style=" width: 668px;  border-left: solid 1px #dedbdb; border-right: solid 1px #dedbdb;">
                    
                        <div style="padding-left: 10px; width: 550px;">
                            <h2>Price Range</h2>
                                <table cellpadding="0" cellspacing="0" style="padding-bottom: 10px;">
                                    <tr>
                                        <td>
                                            <span class="NavyLink12">Min $</span>
                                        </td>
                                        <td style="padding-right: 10px;">
                                            <asp:TextBox Width="50px" runat="server" ID="MinTextBox"></asp:TextBox>
                                        </td>
                                        <td>
                                            <span class="NavyLink12">Max $</span>
                                        </td>
                                        <td>
                                            <asp:TextBox Width="50px" runat="server" ID="MaxTextBox"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            <h2>Description<span class="Asterisk">* </span></h2>
                            <span class="NavyLink12"> min 50
                             characters &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <span style="color: Red;" id="CharsDivEditor"></span></span><br />

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
                                                                        }
                                                                        else
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
                                                              editor.pasteHtml(String.format("<a target='_blank' class='NavyLink12' href='{0}'>{1}</a>", args.url, args.name))
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
                       <div style="padding-left: 10px; padding-top: 20px; width: 550px; padding-bottom: 20px;">
                       <script type="text/javascript">

                                function CountChars(editor, e){
                                    
                                    var theDiv = document.getElementById("CharsDiv");
                                        var theText = document.getElementById("ctl00_ContentPlaceHolder1_ShortDescriptionTextBox");
                                        theDiv.innerHTML = "characters left: "+ (150 - theText.value.length).toString();
                                    
                                }
                            </script>
                            <div class="topDiv">
                                <div style="float: left;">
                                    <h2>Short Description<span class="Asterisk">*</span></h2>
                                </div>
                                <div style="float: left; padding-left: 5px;padding-top: 7px;">
                                    <asp:Image CssClass="HelpImage" runat="server"  ID="Image4" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
                                    <rad:RadToolTip ID="RadToolTip4" runat="server" Skin="Sunset"
                                    Width="200px" ManualClose="true" ShowEvent="onclick" 
                                    Position="MiddleRight" RelativeTo="Element" TargetControlID="Image4">
                                    <div style="padding: 10px;"><label>The short description will appear 
                                    in search results and locale calendars.</label></div>
                                    </rad:RadToolTip>
                                </div>
                            </div>
                            <span  class="NavyLink12"> max 150 characters 
                              &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style="color: Red;" id="CharsDiv"></span></span><br />
                            <asp:TextBox ID="ShortDescriptionTextBox" onkeypress="CountChars(event)" Width="536" runat="server"></asp:TextBox>

                        </div>
                        <div style="padding-left: 10px; padding-bottom: 20px;  width: 550px;border-top: solid 1px #dedbdb;">
                            <h2>What To Bring On The Adventure<span class="Asterisk">*</span></h2>
                            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                <script type="text/javascript">
                                                function ShowBrignDiv()
                                                {
                                                    var theDiv = document.getElementById('WidthBrignDiv');
                                                    var loadDiv = document.getElementById('LoadBrignDiv');
                                                    
                                                    loadDiv.style.height = theDiv.offsetHeight +'px';
                                                    loadDiv.style.display = 'block';
                                                    
                                                    return true;
                                                }
                                            </script>
                                            <div id="LoadBrignDiv" style="margin-left: -10px;z-index: 10000;width: 670px;display: none;position: absolute;">
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
                                            <div id="WidthBrignDiv">
                                       <table>
                                            <tr>
                                                    <td>
                                                        <label>Item:</label> <asp:TextBox runat="server" ID="BringTextBox"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <div align="center" style=" width: 130px;">
                                                             <ctrl:BlueButton ID="BlueButton19" WIDTH="92px" runat="server" CLIENT_LINK_CLICK="ShowBrignDiv()"
                                                             BUTTON_TEXT="Add Item >>" />
                                                             <ctrl:BlueButton ID="BlueButton20" runat="server" WIDTH="90px" CLIENT_LINK_CLICK="ShowBrignDiv()"
                                                             BUTTON_TEXT="<< Remove" />
                                                            <asp:Label runat="server" ID="Label1" Visible="false"></asp:Label>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <asp:ListBox runat="server" ID="BringListBox" Width="300px" 
                                                        Height="100px"></asp:ListBox>
                                                        <asp:Label runat="server" ID="Label2" ForeColor="Red"></asp:Label>
                                                    </td>
                                                </tr>
                                       </table>
                                       </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div style="padding-left: 10px;border-top: solid 1px #dedbdb; padding-bottom: 20px; width: 550px;">
                            <table>
                                <tr>
                                    <td style="padding-right: 20px;">
                                        <h2>What One Should Obtain From This Adventure<span class="Asterisk">*</span></h2>
                                        <asp:TextBox runat="server" TextMode="MultiLine" Width="200px" Height="100px" ID="WhatObtainTextBox"></asp:TextBox>
                                    </td>
                                    <td>
                                        <h2>How To Dress For This Adventure<span class="Asterisk">*</span></h2>
                                        <asp:TextBox runat="server" TextMode="MultiLine" Width="200px" Height="100px" ID="HowDressTextBox"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                       <%-- <div class="topDiv" align="right" style="padding-right: 50px;width: 660px;">
                            
                            <div style="float: right;">
                                <ctrl:BlueButton CLIENT_LINK_CLICK="ShowDesDiv()" WIDTH="76px"  ID="DescriptionOnwardsButton" runat="server" BUTTON_TEXT="Onwards" />
                            </div>
                            <div style="float: right;padding-right: 10px;">
                                <ctrl:BlueButton CLIENT_LINK_CLICK="ShowDesDiv()" ID="ImageButton2" WIDTH="87px" runat="server" BUTTON_TEXT="Backwards" />
                            </div>
                         </div>--%>
                    </div>
                    </div>
                </asp:Panel>
                </div>
            <%--</rad:RadPageView>

            <rad:RadPageView runat="server" ID="RadPageView2" TabIndex="2" >--%>
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
                            <div id="WidthMediaDiv" class="topDiv" style="width: 668px; padding-bottom: 20px;border-left: solid 1px #dedbdb; border-right: solid 1px #dedbdb;">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="buttonSubmit"/>
                            </Triggers>
                            <ContentTemplate>
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
                                <a href="#top"></a>
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
                                    <div id="WidthInDiv" style="padding-left: 10px; width: 632px; clear: both;padding-top: 20px;">
                                        <asp:CheckBox ID="MainAttractionCheck" onclick="ShowInDiv()" Text="Include Pictures and Videos <span  class='NavyLink12'>[20 max]</span><div style='float: right;padding-right: 400px;position: relative;'><div class='Text12' style='position: absolute;float: left;'><div id='div5' style='z-index: 10000; left: -100px; top: 20px;padding: 10px;position: absolute;display: none;background-color: white; width: 150px; border: solid 2px #6fa8bf;'>If you're uploading photos/images make sure you have specified a descriptive name for your file. It will help search engines, like google, find your page more effectively.</div><img onmouseout='showText()' onmouseover='hideText()' src='http://hippohappenings.com/NewImages/HelpIconNew.png' /></div></div>"   runat="server" AutoPostBack="true" OnCheckedChanged="EnableMainAttractionPanel" />
                                            <asp:Panel runat="server" ID="MainAttractionPanel" Visible="false" Enabled="false"> 
                                                <div style="padding-left: 30px;">
                                                    <asp:RadioButtonList onchange="ShowInDiv()" runat="server" ID="MainAttracionRadioList" AutoPostBack="true" 
                                                    OnSelectedIndexChanged="ShowSliderOrVidPic">
                                                        <asp:ListItem Text="Add You Tube Video" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Add Picture" Value="1"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                    <div style="padding-left: 30px; width: 510px; padding-bottom: 10px; padding-left: 30px;">
                                                        <asp:Panel runat="server" ID="VideoPanel" Visible="false">
                                                            <div class="topDiv" style="padding-left: 20px;">
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
                                                                <asp:Panel runat="server" ID="YouTubePanel" Visible="false">
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
                                                         <div class="topDiv" style="padding-left: 30px;">
                                                                <asp:Panel runat="server" ID="PicturePanel" Visible="false">
                                                                    <div style=" border:  solid 1px #dedbdb;padding: 10px;">
                                                                        <div id="topDiv2">
                                                                            <div style="float: left; padding-top: 4px;">
                                                                            
                                                                                <rad:RadUpload Skin="Web20" ID="PictureUpload" runat="server"
                                                                                    MaxFileInputsCount="20" />
                                                                                    
                                                                                <rad:RadProgressArea id="progressArea1" runat="server"/>
                                                                                
                                                                                <asp:Button id="buttonSubmit" runat="server" CssClass="RadUploadSubmit"  OnClientClick="ShowInDiv()" OnClick="PictureUpload_Click" text="Submit" />
                                                                            
                                                                                <%--<asp:FileUpload runat="server" ID="PictureUpload" Width="230px" EnableViewState="true" />--%>
                                                                            </div>
                                                                            <div style="float: left;padding-left: 5px; padding-top: 3px;">
                                                                            <%--<ctrl:BlueButton ID="ImageButton5" runat="server" BUTTON_TEXT="Upload" />--%>
                                                                            
            <%--                                                                    <asp:ImageButton runat="server" ID="ImageButton5" 
                                                                                PostBackUrl="blog-event#top" OnClick="PictureUpload_Click"
                                                                                 ImageUrl="image/MakeItSoButton.png" 
                                                                                onmouseout="this.src='image/MakeItSoButton.png'" onmouseover="this.src='image/MakeItSoButtonSelected.png'"  />
            --%>                                                                </div>
                                                                            
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
                                                        <ctrl:BlueButton ID="PictureNixItButton" CLIENT_LINK_CLICK="ShowInDiv()"  WIDTH="54px"  runat="server" BUTTON_TEXT="Nix It" />
                                                    </div>
                                                </asp:Panel>
                                            </asp:Panel>
                                    </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                    <%--<div class="topDiv" align="right" style="padding-right: 50px;width: 660px;padding-top: 10px;clear:both;">
                                    
                                        <div style="float: right;">
                                            <ctrl:BlueButton ID="MediaOnwardsButton" CLIENT_LINK_CLICK="ShowMediaDiv()" WIDTH="76px"  runat="server" BUTTON_TEXT="Onwards" />
                                        </div>
                                        <div style="float: right;padding-right: 10px;">
                                            <ctrl:BlueButton ID="ImageButton3" CLIENT_LINK_CLICK="ShowMediaDiv()" WIDTH="87px" runat="server" BUTTON_TEXT="Backwards" />
                                        </div>
                                    </div>--%>
                                </div>
                        </div>
                        </asp:Panel>
                   </div>
            <%--</rad:RadPageView>

            <rad:RadPageView runat="server" ID="RadPageView3" TabIndex="3" >--%>
                <div class="EventDiv topDiv" id="Div2" style=" width: 614px; border-top: solid 1px #dedbdb;">
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
                    <div id="WidthCatDiv" class="EventDiv topDiv" style=" width: 668px;  border-left: solid 1px #dedbdb; border-right: solid 1px #dedbdb;">
                        <div style="padding-left: 10px; width: 550px;">
                            
                            <div>
                            <div class="topDiv">
                                <div style="float: left;">
                                    <h2>Select Categories for Your Adventure<span class="Asterisk">* </span></h2>
                                </div>
                                <div style="float: left; padding-left: 5px;padding-top: 3px;">
                                    <asp:Image CssClass="HelpImage" runat="server"  ID="Image5" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
                                    <rad:RadToolTip ID="RadToolTip6" runat="server" Skin="Sunset" 
                                    Width="200px" ManualClose="true" ShowEvent="onclick" 
                                    Position="MiddleRight" RelativeTo="Element" TargetControlID="Image5">
                                    <div style="padding: 10px;"><label>Selecting categories that most 
                                    represent your adventure will help your viewers find it faster via our search page.</label></div>
                                    </rad:RadToolTip>
                                </div>
                            </div>
                            <table>
                                        <tr>
                                            <td valign="top">
                                                    <div class="FloatLeft" style="width: 150px;">
                                                        <rad:RadTreeView Skin="Vista" Width="150px" runat="server"  
                                                        ID="CategoryTree" DataFieldID="ID" DataSourceID="SqlDataSource1" 
                                                        DataFieldParentID="ParentID" 
                                                        CheckBoxes="true">
                                                        <DataBindings>
                                                             <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                             TextField="Name" Expanded="false" ValueField="ID" />
                                                         </DataBindings>
                                                        </rad:RadTreeView>
                                                        <asp:SqlDataSource runat="server" ID="SqlDataSource1" 
                                                            SelectCommand="SELECT * FROM TripCategories WHERE (ParentID IN (SELECT ID FROM TripCategories WHERE (Name < 'Li') AND (ParentID IS NULL))) OR ((Name < 'Li') AND (ParentID IS NULL)) ORDER BY Name ASC"
                                                            ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                        </asp:SqlDataSource>
                                                    </div>
                                                    
                                           
                                            </td>
                                            <td valign="top">
                                                <asp:CheckBoxList runat="server" ID="CheckBoxList1" 
                                                    RepeatColumns="4" RepeatDirection="horizontal">
                                                    
                                                    </asp:CheckBoxList>
                                                    <div class="FloatLeft" style="width: 150px;">
                                                        <rad:RadTreeView Skin="Vista" Width="150px" runat="server"  
                                                        ID="RadTreeView2" DataFieldID="ID" DataSourceID="SqlDataSource3"
                                                        DataFieldParentID="ParentID"
                                                        CheckBoxes="true">
                                                        <DataBindings>
                                                             <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                             TextField="Name" Expanded="false" ValueField="ID"  />
                                                         </DataBindings>
                                                        </rad:RadTreeView>
                                                        <asp:SqlDataSource runat="server" ID="SqlDataSource3" 
                                                            SelectCommand="SELECT * FROM TripCategories WHERE (ParentID IN (SELECT ID FROM TripCategories WHERE (Name >= 'Li') AND (ParentID IS NULL))) OR ((Name >= 'Li') AND (ParentID IS NULL)) ORDER BY Name ASC"
                                                            ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                        </asp:SqlDataSource>
                                                    </div>
                                            </td>
                                        </tr>
                                    </table>       
                                                     
                        <div style="width: 550px; clear: both;">
                            <div style="float: left; padding-top: 10px;">
                                <label>Don't see a category you want? Suggest one!</label>
                                <asp:TextBox runat="server" ID="CategoriesTextBox"></asp:TextBox>
                            </div>
                            <div style="padding-top: 9px; float: left; padding-left: 10px;">   
                              <ctrl:BlueButton ID="ImageButton11" CLIENT_LINK_CLICK="ShowCatDiv()" runat="server"  WIDTH="122px" BUTTON_TEXT="Send Suggestion" />
                            </div>
                        </div>
                        </div>
                      </div>
                        <%--<div class="topDiv" align="right" style="padding-right: 50px;width: 660px;clear: both;padding-top: 10px;">
                            
                            <div style="float: right;">
                                <ctrl:BlueButton ID="CategoryOnwardButton" CLIENT_LINK_CLICK="ShowCatDiv()" WIDTH="76px"  runat="server" BUTTON_TEXT="Onwards" />
                            </div>
                            <div style="float: right;padding-right: 10px;">
                                <ctrl:BlueButton ID="ImageButton9" runat="server" CLIENT_LINK_CLICK="ShowCatDiv()" WIDTH="87px" BUTTON_TEXT="Backwards" />
                            </div>
                         </div>--%>
                    </div>
                    </div>
                </asp:Panel>
                </div>
            <%--</rad:RadPageView>
            <rad:RadPageView runat="server" ID="RadPageView4" TabIndex="4" >--%>
               <%-- <div style="position: relative;">
                <asp:Panel runat="server" ID="Panel6">          
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
                    <div id="WidthAllDiv" class="Text12" style=" width: 668px; border: solid 1px #dedbdb;">
                    <asp:UpdatePanel runat="server" ID="FetUpdatePanel" UpdateMode="conditional">
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
                                    <h1>Want To Feature Your Adventure?</h1>
                                </div>
                                <div style="float: right;padding-top: 10px;">
                                    <ctrl:BlueButton ID="BlueButton8" runat="server" CLIENT_CLICK="OpenFeaturePricing('T');"  WIDTH="186px" BUTTON_TEXT="Featured Adventure Pricing >" />
                                </div>
                            </div>
                            <div align="center" style="padding-top: 30px;">
                                <div class="FeaturePrice">
                                    <h2>It's only $<asp:Literal runat="server" ID="PriceLiteral"></asp:Literal> / day!</h2>
                                </div>
                                Your featured adventure will come up first in our adventure search results on the adventure search page. 
                                In addition, your adventure will be shown first on our home page 'Stuff To Do' strip if it falls 
                                under the dates a viewer specifies (i.e. Today, Tomorrow, etc.). You can always choose to 
                                feature your adventure later, after you have posted it, by going to the 'Searches and Pages' 
                                page from your My Account page.
                                <div class="PostFreeDiv" style="padding: 20px;">
                                    <h2><asp:Panel runat="server" ID="PricePanel"></asp:Panel></h2>
                                </div>
                            </div>
                             <div align="center" class="topDiv" style="padding-top: 10px; padding-bottom: 30px;">
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
                                            make sure the zip code you provided for the start location is correct.</div></b><br /><br />
                                        </asp:Panel>
                                        <div style="float: left;">
                                            <h2>Pick the dates you want your adventure featured</h2>
                                        </div>
                                        <div style="clear: both; padding-bottom: 5px;">
                                            *Days on which your adventure has already been featured show up in gray below.
                                        </div>
                                    </div>
                                        <div class="topDiv" style="border-top: solid 1px #dedbdb; width: 614px; padding-top: 10px;">
                                            <div style="float: left; width: 215px;">
                                                
                                                <rad:RadDatePicker runat="server" ID="FeaturedDatePicker" Skin="Web20"></rad:RadDatePicker><br />
                                                <div style="padding-top: 5px;">[7 days max, for next 30 days]</div>
                                            </div>
                                            <div style="float: left; width: 130px; padding-top: 50px;">
                                                 
                                                    <ctrl:BlueButton ID="BlueButton1" runat="server" WIDTH="95px" CLIENT_LINK_CLICK="ShowFetDiv()" BUTTON_TEXT="Add Date >>" />
                                                 
                                                     <ctrl:BlueButton ID="BlueButton4" runat="server" WIDTH="90px" CLIENT_LINK_CLICK="ShowFetDiv()"
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
                                            <div style="padding: 10px;line-height: 20px;"><label>Selecting no search terms will mean your adventure will pop 
                                            up in searches where no keywords are entered. If you select any keywords, your featured 
                                            adventure will pop up in searches where those keywords are entered and in searches where no 
                                            keywords are entered as well. Each keyword is limited to four adventures, and so, picking a 
                                            keyword guarantees that your adventure will show up in the top four results of a search with your keyword. 
                                            In the searches without keywords, your adventure will show up in the top four results chosen randomly 
                                            per each time searched. Meaning, if you do not include search terms your adventure is <u>not</u> 
                                            guaranteed to show up in the first four results if there is over 4 featured adventures for your location.<br /><br />
                                            If this all sounds too complicated, strictly speaking, it is better to put in search terms!</label></div>
                                            </rad:RadToolTip>
                                        </div>
                                    </div>
                                        <div class="topDiv" style="border-top: solid 1px #dedbdb; width: 614px; padding-top: 10px;">
                                            <div style="float: left; width: 215px;">
                                                <asp:TextBox runat="server" ID="TermsBox"></asp:TextBox>
                                            </div>
                                            <div style="float: left; width: 130px; padding-top: 50px;">
                                                 <ctrl:BlueButton ID="BlueButton5" runat="server" WIDTH="97px" CLIENT_LINK_CLICK="ShowFetDiv()" BUTTON_TEXT="Add Term >>" />
                                                 <ctrl:BlueButton ID="BlueButton6" runat="server" WIDTH="90px" CLIENT_LINK_CLICK="ShowFetDiv()" 
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
                                                <div style="padding: 10px;"><label>Your price depends on the number of members in the location you are featuring the adventure,
                                                number of days, number of search terms, and number of existing featured adventure on the days you 
                                                have chosen. The more featured adventure, the less 
                                                it costs to feature an adventure.</label></div>
                                            </rad:RadToolTip>
                                        </div>
                                    </div>
                                    
                                    <table align="center" style="border: solid 1px #dedbdb;" width="100%">
                                        <tr>
                                            <th>Day</th>
                                       
                                            <th>Standard Rate / day</th>
                                        
                                            <th>Adjustment for Members / day</th>
                                        
                                            <th>Adjustment for Trips / day</th>
                                     
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
                        <%--<div class="topDiv" align="right" style="padding-right: 50px;width: 660px;clear: both;padding-top: 10px;">
                            
                            <div style="float: right;">
                                <ctrl:BlueButton ID="BlueButton2" CLIENT_LINK_CLICK="ShowAllDiv()" WIDTH="76px"  runat="server" BUTTON_TEXT="Onwards" />
                            </div>
                            <div style="float: right;padding-right: 10px;">
                                <ctrl:BlueButton ID="BlueButton3" CLIENT_LINK_CLICK="ShowAllDiv()" runat="server" WIDTH="87px" BUTTON_TEXT="Backwards" />
                            </div>
                         </div>
                    </div>
                </asp:Panel>
                </div>
            </rad:RadPageView>
            <rad:RadPageView runat="server" ID="RadPageView5" TabIndex="5" >--%>
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
               <div class="EventDiv" id="Div3" style=" width: 614px; border-top: solid 1px #dedbdb;">
                <asp:Panel runat="server" ID="Panel5" DefaultButton="PostItButton"> 
                   
                   <div class="Text12"  style="width: 668px; border-left: solid 1px #dedbdb; border-right: solid 1px #dedbdb; border-bottom: solid 1px #dedbdb;">
                     
                        <div style="padding-left: 10px;">
                           <%-- <asp:Panel runat="server" ID="PaymentPanel" Visible="false">
                                    <div style="border: solid 1px #dedbdb;clear: both;margin-top: 10px;margin-bottom: 30px;margin-right: 10px; padding: 10px; background-color: #eef7fb;">
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
                                                                        <asp:Image CssClass="HelpImage" runat="server"  ID="Image12" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
                                                                        <rad:RadToolTip ID="RadToolTip10" runat="server" Skin="Sunset" 
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
                                
                            <div style="clear: both;">
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
                        <div class="topDiv" align="right" style="padding-right: 50px;width: 660px;clear: both;padding-top: 10px;">
                            
                            <div style="float: right;">
                                <ctrl:BlueButton ID="PostItButton" CLIENT_LINK_CLICK="ShowPostDiv()" runat="server" WIDTH="60px" BUTTON_TEXT="Post It" />
                            </div>
<%--                            <div style="float: right;padding-right: 10px;">
                                <ctrl:BlueButton ID="ImageButton12" CLIENT_LINK_CLICK="ShowPostDiv()" runat="server" WIDTH="87px" BUTTON_TEXT="Backwards" />
                            </div>
--%>                         </div>
                    </div>
                </asp:Panel>
                </div>
                </div>
                </div>
             <%--</rad:RadPageView>

        </rad:RadMultiPage>--%>
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
    

<%--</rad:RadAjaxPanel>--%>
</asp:Content>

