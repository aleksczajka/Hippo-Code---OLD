<%@ Page Language="C#" MasterPageFile="~/SecondMasterPage.master" Inherits="BlogEvent" 
ValidateRequest="false" EnableEventValidation="false" AutoEventWireup="true" 
Title="Blog Event & Happenings | HippoHappenings"  
CodeFile="BlogEvent.aspx.cs" %>
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
                    VisibleStatusbar="false" Skin="Web20" ClientCallBackFunction="FillVenueJS" Height="500" ID="RadWindow1" 
                     runat="server">
                    </rad:RadWindow>
                </Windows> 
            </rad:RadWindowManager>
            <script type="text/javascript">
            function OnClientClose(oWnd, args)
            {
                if(args != null && args != undefined)
                {
                    if(args == "somtin")
                        window.location.reload();
                    else
                        window.location = args;
                }
            }
            
            function FillNewVen(oWnd, args)
            {
                if(args != null && args != undefined)
                {
                    if(args == "somtin")
                        window.location.reload();
                    else
                        window.location = args;
                }
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
<div style="padding-bottom: 100px;">

                        <div>
                            
                            <div class="topDiv">
                                <div style="float: left;">
                                    <asp:Label runat="server" ID="nameLabel" Text="<h1>Enter an Event</h1><br/>"></asp:Label>
                                </div>
                                <div style="float: left; padding-left: 5px;padding-top: 10px;">
                                    <asp:Image CssClass="HelpImage" runat="server"  ID="Image1" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
                                    <rad:RadToolTip ID="RadToolTip1" runat="server" Skin="Sunset" 
                                    Width="200px" ManualClose="true" ShowEvent="onclick" 
                                    Position="MiddleRight" RelativeTo="Element" TargetControlID="Image1">
                                    <div style="padding: 10px;"><label>All HTML tags will be removed except for links. 
                                    However, the header cannot contain links at all.</label></div>
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
                        <div style="float: left;">
    <asp:UpdatePanel runat="server" UpdateMode="conditional" ID="EverythingUpdatePanel">
    <Triggers>
        <asp:PostBackTrigger ControlID="PostItButton" />
    </Triggers>
<ContentTemplate>
                      <script type="text/javascript">
                        function ShowEveryDiv()
                        {
                            var theDiv = document.getElementById('Div3');
                            var loadDiv = document.getElementById('Div2');
                            
                            loadDiv.style.height = theDiv.offsetHeight +'px';
                            loadDiv.style.display = 'block';
                            
                            return true;
                        }
                    </script>
                    <div id="Div2" style="z-index: 10000;width: 670px;display: none;position: absolute;">
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
                    <div id="Div3">                
    <div id="topDiv6">
    <div style="float: left; width: 670px;padding-bottom: 10px;">
       <div> Import from craigslist <asp:TextBox runat="server" ID="CraigslistURLTextBox"></asp:TextBox>
            <asp:Button runat="server" ID="CLButton" OnClientClick="ShowEveryDiv()" Text="Import" OnClick="ImportFromCraigslist" />
            <label><asp:Label runat="server" ID="CLErrorLabel" ForeColor="red"></asp:Label></div></label>
            
            </div>
            <br />
            <div style="float: left; width: 670px;">
         Fill in manually
         </div>
        <div style="float: left; width: 670px;border: solid 1px #dedbdb;">
        <div>
            <%--<rad:RadTabStrip runat="server" ID="EventTabStrip" Height="27px"
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
            </rad:RadTabStrip>--%>
        </div>
        <%--<rad:RadMultiPage runat="server" ID="BlogEventPages" SelectedIndex="0">
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
                    <div id="WidthDetailsDiv" class="topDiv" style="width: 668px;">
                        <div style="padding-left: 10px;">
                            
                            <table width="100%">
                                <tr>
                                    <td valign="top" width="250px">
                                    
                                            <script type="text/javascript">
                                                        function CountCharsHeadline(texteditor, limit){
                                                            
                                                                var theDiv = document.getElementById("CharsDivHeadline");
                                                                var theText = texteditor;
                                                                theDiv.innerHTML = "characters left: "+ (limit - theText.value.length).toString();
                                                            
                                                        }
                                                    </script>
                                        <h2>Event Name<span class="Asterisk">* </span></h2>
                                        <span class="NavyLink12">max 70 characters
                                        &nbsp;&nbsp;&nbsp;
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <span style="color: Red;" id="CharsDivHeadline"></span></span>
                                        <asp:TextBox runat="server" onkeypress="CountCharsHeadline(this, 70)" Width="240" 
                                        ID="EventNameTextBox"></asp:TextBox>
                                        <br /><br /><br />
                                         
                                    </td>
                                    <td style=" padding-left: 32px;">
                                    
                                        
                                         <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
                                            <ContentTemplate>
                                            <div style="position: relative;">
                                 <script type="text/javascript">
                                                function ShowLocaleDiv()
                                                {
                                                    var theDiv = document.getElementById('WidthLocaleDiv');
                                                    var loadDiv = document.getElementById('LoadLocaleDiv');
                                                    
                                                    loadDiv.style.height = theDiv.offsetHeight +'px';
                                                    loadDiv.style.display = 'block';
                                                    
                                                    return true;
                                                }
                                            </script>
                                            <div id="LoadLocaleDiv" style="z-index: 10000;width: 350px;display: none;position: absolute;margin-left: -15px;">
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
                                        <div id="WidthLocaleDiv" style="width: 350px; float: left;">
                                        <div class="topDiv">
                                        <div style="float: left;">
                                            <h2>Locale<span class="Asterisk">* </span></h2>
                                        </div>
                                        <div style="float: left; padding-left: 5px;padding-top: 7px;">
                                            <asp:Image CssClass="HelpImage" runat="server"  ID="Image2" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
                                            <rad:RadToolTip ID="RadToolTip2" runat="server" Skin="Sunset"
                                            Width="200px" ManualClose="true" ShowEvent="onclick" 
                                            Position="MiddleRight" RelativeTo="Element" TargetControlID="Image2">
                                            <div style="padding: 10px;"><label>An event has to be associated 
                                            with a locale, be it an existing public location or a private location. To find if your public locale already exists, 
                                            select 'existing venue location', select your country and state, click go and select your locale. If you 
                                            find that the public venue does not exist <span class="NavyLink12UD" 
                                            onclick="OpenCreateVenue();">please create one. If your location, however, is private (like for a garage sale), select 'private/non-venue location'.</span></label></div>
                                            </rad:RadToolTip>
                                        </div>
                                    </div>
                                        <div>
                                            <asp:RadioButtonList OnSelectedIndexChanged="SwitchLocationPanels" runat="server" 
                                            onchange="ShowLocaleDiv()" onclick="ShowLocaleDiv()" ID="LocaleRadioButtonList" AutoPostBack="true" RepeatDirection="horizontal">
                                                <asp:ListItem Text="Existing Venue Location" Value="0" Selected="true"></asp:ListItem>
                                                <asp:ListItem Text="Private/non-venue Location" Value="1"></asp:ListItem>
                                            </asp:RadioButtonList>
                                            <div style="float: left; width: 300px ;padding-left: 10px; padding-bottom: 20px;">
                                            <asp:Panel runat="server" ID="OneTimeVenuePanel" Visible="false">
                                                <rad:RadTextBox runat="server" ID="addressTextBox" EmptyMessage="Address"></rad:RadTextBox><br />
                                                <rad:RadTextBox runat="server" ID="cityTextBox" EmptyMessage="City" Width="100px"></rad:RadTextBox><label>,</label> <rad:RadTextBox Width="50px" runat="server" ID="ZipTextBox" EmptyMessage="Zip"></rad:RadTextBox><br />
                                                <asp:DropDownList d runat="server" ID="privateCountryDropDown" onchange="ShowLocaleDiv()" OnSelectedIndexChanged="ChangePrivateStateAction" AutoPostBack="true" ></asp:DropDownList><br />
                                                <asp:DropDownList runat="server" OnSelectedIndexChanged="FillThePrice" AutoPostBack="true" onchange="ShowEveryDiv()" ID="privateStateDropDown"></asp:DropDownList>
                                                    <rad:RadTextBox runat="server" EmptyMessage="State" ID="privateStateTextBox"></rad:RadTextBox><br />
                                            </asp:Panel>
                                            <asp:Panel runat="server" ID="ExistingVenuePanel">
                                            <script type="text/javascript">
                                                   function fillDrop(theText, theID)
                                                   {
                                                        ShowEveryDiv();
                                                        var theDiv = document.getElementById('ctl00_ContentPlaceHolder1_TimeFrameDiv');
                                                        theDiv.innerHTML = theText.replace("&&", "'");
                                                        
                                                        var theidDiv = document.getElementById('ctl00_ContentPlaceHolder1_VenueIDDIV');
                                                        theidDiv.innerHTML = theID;
                                                        
                                                        var tooltip = Telerik.Web.UI.RadToolTip.getCurrent();
                                                        if (tooltip) tooltip.hide();
                                                        
                                                        __doPostBack('ctl00$ContentPlaceHolder1$LinkButton1',theID);
                                                   }
                                                   
                                                   function HideAllDivs()
                                                   {
                                                        var theDiv = document.getElementById('contentDivA');
                                                        
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivA');
                                                            theDiv.className = 'Blue12Link';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivB');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivB');
                                                            theDiv.className = 'Blue12Link';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivC');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivC');
                                                            theDiv.className = 'Blue12Link';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivD');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivD');
                                                            theDiv.className = 'Blue12Link';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivE');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivE');
                                                            theDiv.className = 'Blue12Link';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivF');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivF');
                                                            theDiv.className = 'Blue12Link';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivG');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivG');
                                                            theDiv.className = 'Blue12Link';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivH');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivH');
                                                            theDiv.className = 'Blue12Link';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivI');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivI');
                                                            theDiv.className = 'Blue12Link';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivJ');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivJ');
                                                            theDiv.className = 'Blue12Link';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivK');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivK');
                                                            theDiv.className = 'Blue12Link';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivL');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivL');
                                                            theDiv.className = 'Blue12Link';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivM');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivM');
                                                            theDiv.className = 'Blue12Link';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivN');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivN');
                                                            theDiv.className = 'Blue12Link';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivO');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivO');
                                                            theDiv.className = 'Blue12Link';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivP');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivP');
                                                            theDiv.className = 'Blue12Link';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivQ');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivQ');
                                                            theDiv.className = 'Blue12Link';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivR');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivR');
                                                            theDiv.className = 'Blue12Link';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivS');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivS');
                                                            theDiv.className = 'Blue12Link';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivT');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivT');
                                                            theDiv.className = 'Blue12Link';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivU');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivU');
                                                            theDiv.className = 'Blue12Link';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivV');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivV');
                                                            theDiv.className = 'Blue12Link';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivW');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivW');
                                                            theDiv.className = 'Blue12Link';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivX');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivX');
                                                            theDiv.className = 'Blue12Link';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivY');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivY');
                                                            theDiv.className = 'Blue12Link';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivZ');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivZ');
                                                            theDiv.className = 'Blue12Link';
                                                        }
                                                   }
                                                   
                                                   function SelectLetterDiv(letter)
                                                   {
                                                    HideAllDivs();
                                                    var theDiv = document.getElementById('contentDiv'+letter);
                                                    theDiv.style.display = 'block';
                                                    
                                                    theDiv = document.getElementById('titleDiv'+letter);
                                                    theDiv.className = "AddGreenLink";
                                                   }
                                                   
                                                   function OpenCreateVenue()
                                                   {
                                                        var win = $find("<%=RadWindow1.ClientID %>");
                                                        win.setUrl('CreateVenue.aspx');
                                                        win.show(); 
                                                        win.center(); 
                                                   }
                                                   function FillVenueJS(returnV, returnValue)
                                                   {
                                                        if(returnValue != null && returnValue != undefined)
                                                        {
                                                            ShowEveryDiv();
                                                            __doPostBack('ctl00$ContentPlaceHolder1$GetNewVenueButton','');
                                                        }
                                                   }
                                                   </script>
    <%--                                                   <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="conditional">
                                                <ContentTemplate>
    --%>                                                    <asp:LinkButton runat="server" ID="GetNewVenueButton" OnClientClick="ShowEveryDiv()" OnClick="GetNewVenue"></asp:LinkButton>
                                                            <asp:LinkButton runat="server" ID="LinkButton1" OnClientClick="ShowEveryDiv()" OnClick="SetNewVenue"></asp:LinkButton>
    <%--                                               
    </ContentTemplate>
                                                   </asp:UpdatePanel>
    --%>                                            
                                                <div style="padding-top: 5px;">
                                                    <asp:DropDownList runat="server" ID="VenueCountry" onchange="ShowLocaleDiv()" OnSelectedIndexChanged="ChangeVenueStateAction" AutoPostBack="true" ></asp:DropDownList>
                                                </div>
                                                <div style="float: left;padding-top: 5px;">
                                                    <asp:DropDownList runat="server" ID="VenueState"></asp:DropDownList>
                                                    
                                                    <asp:TextBox runat="server" ID="VenueStateTextBox"></asp:TextBox>
                                                    
                                                </div>
                                                <div style="float: left; padding-top: 2px;padding-left: 5px;">
                                                        <ctrl:SmallButton runat="server" CLIENT_LINK_CLICK="ShowLocaleDiv()" BUTTON_TEXT="Search Locales" id="GoButton" />
                                                        <%--<asp:DropDownList Visible="false" runat="server" ID="VenueDropDown" Visible="false">
                                                        </asp:DropDownList>--%>
                                                    </div>
                                                    <div>
                                                <div class="topDiv" style="float: left;">
                                                    <div style="float: right; padding-top: 8px;padding-right: 10px;">
                                                   
                                                        <rad:RadToolTip RelativeTo="Element" Position="topCenter" ManualClose="true" 
                                                        runat="server" ID="Tip1" TargetControlID="Div1" ShowEvent="OnClick">
                                                            stuff
                                                        </rad:RadToolTip>
                                                        <div runat="server" id="VenueIDDIV" style="display: none;"></div>
                                                        <div runat="server" style="cursor: pointer;width: 338px; height: 20px; margin-top: -3px;" id="Div1">
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <div style="float: left;border: solid 2px #33a923;padding: 2px;background-color: transparent;color: #6fa8bf; font-weight: bold; font-family: Arial;font-size: 12px;" runat="server" id="TimeFrameDiv">
                                                                        Select Locale >
                                                                    </div>
        <%--                                                            <img style="padding-top: 7px;float: left;" src="image/VenuesArrow.png" />
        --%>                                                    </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                  </div>
                                                 </div>
                                                </div>
                                            </asp:Panel>
                                            </div>
                                        </div>
                                                                               </div>
                                                      </div>
                                        </ContentTemplate>
                                         </asp:UpdatePanel>
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <td valign="top" colspan="2">
                                        <asp:UpdatePanel runat="server" UpdateMode="conditional">
                                            <ContentTemplate>
                                            <div style="position: relative;">
                                            <script type="text/javascript">
                                                function ShowDateDiv()
                                                {
                                                    var theDiv = document.getElementById('WidthDateDiv');
                                                    var loadDiv = document.getElementById('LoadDateDiv');
                                                    
                                                    loadDiv.style.height = theDiv.offsetHeight +'px';
                                                    loadDiv.style.display = 'block';
                                                    
                                                    return true;
                                                }
                                            </script>
                                            <div id="LoadDateDiv" style="z-index: 10000;width: 670px;display: none;position: absolute;margin-left: -15px;">
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
                                        <div id="WidthDateDiv" class="topDiv" style="border-top: solid 1px #dedbdb; width: 614px;margin-top: -10px; padding-top: 10px;">
                                            <div style="float: left; width: 215px;">
                                            <div class="topDiv">
                                                <div style="float: left;">
                                                    <h2>Event Occurance</h2>
                                                </div>
                                                <div style="float: left; padding-left: 5px;padding-top: 7px;">
                                                    <asp:Image CssClass="HelpImage" runat="server"  ID="Image3" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
                                                    <rad:RadToolTip ID="RadToolTip3" runat="server" Skin="Sunset" 
                                                     ManualClose="true" ShowEvent="onclick" Width="200px"
                                                    Position="MiddleRight" RelativeTo="Element" TargetControlID="Image3">
                                                    <div style="padding: 10px;"><label>Choose all dates this event occurs 
                                                    and re-occurs.</label></div>
                                                    </rad:RadToolTip>
                                                </div>
                                            </div>
                                                
                                               
                                                                         <label>Start Date & Time<span class="Asterisk">* </span></label> 
                                                                                <br />
                                                   <style type="text/css">
                                                    .RadCalendarPopup {
                                                            left:420px !important;
                                                            top: 300px !important;
                                                            position: fixed !important;
                                                            }
                                                   </style>
                                                <rad:RadDateTimePicker runat="server" ID="StartDateTimePicker" Skin="Web20"></rad:RadDateTimePicker><br />
                                                <label>End Date & Time</label><span class="Asterisk">* </span><br />
                                                <rad:RadDateTimePicker runat="server" ID="EndDateTimePicker" Skin="Web20"></rad:RadDateTimePicker><br />
                                            </div>
                                            <div style="float: left; width: 130px; padding-top: 50px;">
                                                 <ctrl:BlueButton ID="AddDateButton" runat="server" WIDTH="94px" CLIENT_LINK_CLICK="ShowDateDiv()" BUTTON_TEXT="Add Date >>" />
                                                 <ctrl:BlueButton ID="SubtractDateButton" runat="server" WIDTH="90px" CLIENT_LINK_CLICK="ShowDateDiv()"
                                                 BUTTON_TEXT="<< Remove" />
                                            </div>
                                            <div style="float: left; ">
                                                <h2>Your Dates:</h2>
        <%--                                        <span style="font-family: Arial; font-size: 10px; color: #666666;"> [choose all dates this event re-occurs]</span> 
        --%><%--                                        <rad:RadDateTimePicker runat="server" ID="ReoccuringRadDateTimePicker"></rad:RadDateTimePicker><asp:ImageButton runat="server" AlternateText="add date" OnClick="AddDate" ImageUrl="image/AddIcon.png" CssClass="BigWhiteButton" />
        --%>                                       
        <%--                                        <label>Your selections</label><br />
        --%>                                       
                                                            <asp:ListBox runat="server" EnableViewState="true" ID="DateSelectionsListBox" 
                                                              CssClass="ListBox" SelectionMode="single" Width="250px" Height="100px"></asp:ListBox>
                                            </div>
                                            <asp:Label runat="server" ID="DateErrorLabel" ForeColor="red" Font-Bold="true" Font-Names="Arial" Font-Size="12px"></asp:Label> 
                                        </div>
                                        
                                              
                                        </div>
                                        </ContentTemplate>
                                        </asp:UpdatePanel>
                                            
                                                                               
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <%--<br />
                             <label><span class="NavyLink12"><span class="Asterisk">* </span>required fields</span></label>
                            <br />--%>
                        </div>
                        
                        <%--<div class="topDiv" align="right" style="width: 85px; float: right;padding-right: 10px;">
                                <ctrl:BlueButton ID="DetailsOnwardsButton" WIDTH="76px" runat="server" CLIENT_LINK_CLICK="ShowDetailsDiv()" 
                                BUTTON_TEXT="Onwards" />
                             </div>--%>
                    </div>
                    </div>
                </asp:Panel>
            <%--</rad:RadPageView>
            <rad:RadPageView runat="server" ID="RadPageView1" TabIndex="1" >--%>
                <asp:Panel runat="server" ID="Panel2">
                    <div style="position: relative;">
                    <script type="text/javascript">
                                                function ShowDescDiv()
                                                {
                                                    var theDiv = document.getElementById('WidthDescDiv');
                                                    var loadDiv = document.getElementById('LoadDescDiv');
                                                    
                                                    loadDiv.style.height = theDiv.offsetHeight +'px';
                                                    loadDiv.style.display = 'block';
                                                    
                                                    return true;
                                                }
                                            </script>
                                            <div id="LoadDescDiv" style="z-index: 10000;width: 670px;display: none;position: absolute;">
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
                    <div id="WidthDescDiv" class="EventDiv" style=" width: 614px; border-top: solid 1px #dedbdb;">
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
                            <%--<asp:TextBox ID="DescriptionTextBox" runat="server" CssClass="AddCommentTextBox" Height="150px" 
                            TextMode="multiLine" Wrap="true"></asp:TextBox>--%>
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
                                                            
<%--                                                            <asp:TextBox runat="server" ID="SummaryTextBox" BackColor="#686868" Width="200" Height="100" TextMode="multiLine" Wrap="true"></asp:TextBox>
--%>                                                         <%-- <rad:RadEditor OnClientLoad="OnClientLoad" EditModes="Design" Skin="Black" runat="server" 
                                                            ID="SummaryTextBox" Width="200" Height="103"
                                                                EnableResize="False"  ToolsFile="toolsfile.xml" 
                                                                StripFormattingOptions="MSWordRemoveAll" 
                                                                StripFormattingOnPaste="MSWordRemoveAll">
                                                                <CssFiles>
                                                                <rad:EditorCssFile Value="contentarea2.css" />
                                                               </CssFiles>
                                                            </rad:RadEditor>--%>
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
                                                                
                                                          
                                                          if (elem == "A" || elem == "a")
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
                                                              editor.pasteHtml(String.format("<a target='_blank' class='NavyLink12UD' href='{0}'>{1}</a>", args.url, args.name))
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
                       <div style="padding-left: 10px; padding-top: 20px; padding-bottom: 20px; width: 550px;">
                       <script type="text/javascript">
                                    
//                                                                            function OnClientLoad(editor)
//                                                                            {
//                                                                                editor.attachEventHandler("onkeyup", CountChars);
//                                                                                editor.attachEventHandler("onkeypress", function(evt){if(evt.keyCode == 13)return false;});
//                                                                            }  
                                function CountChars(editor, e){
                                    
                                    var theDiv = document.getElementById("CharsDiv");
                                        var theText = document.getElementById("ctl00_ContentPlaceHolder1_ShortDescriptionTextBox");
                                        theDiv.innerHTML = "characters left: "+ (150 - theText.value.length).toString();
                                    
                                }
                            </script>
                            <%--<div class="topDiv">
                                <div style="float: left;">
                                    <h2>Short Description<span class="Asterisk">*</span></h2>
                                </div>
                                <div style="float: left; padding-left: 5px;padding-top: 7px;">
                                    <asp:Image CssClass="HelpImage" runat="server"  ID="Image4" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
                                    <rad:RadToolTip ID="RadToolTip4" runat="server" Skin="Sunset"
                                    Width="200px" ManualClose="true" ShowEvent="onclick" 
                                    Position="MiddleRight" RelativeTo="Element" TargetControlID="Image4">
                                    <div style="padding: 10px;"><label>The short description will appear in search 
                                    results and locale calendars.</label></div>
                                    </rad:RadToolTip>
                                </div>
                            </div>
                            <span  class="NavyLink12"> max 150 characters 
                              &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style="color: Red;" id="CharsDiv"></span></span><br />
                            <asp:TextBox ID="ShortDescriptionTextBox" onkeypress="CountChars(event)" Width="536" runat="server"></asp:TextBox>
                            --%>
                        </div>
                        <%--<div class="topDiv" align="right" style="padding-right: 50px;width: 660px;">
                            
                            <div style="float: right;">
                                <ctrl:BlueButton ID="DescriptionOnwardsButton" CLIENT_LINK_CLICK="ShowDescDiv()" WIDTH="76px"  runat="server" BUTTON_TEXT="Onwards" />
                            </div>
                            <div style="float: right;padding-right: 10px;">
                                <ctrl:BlueButton ID="ImageButton2" runat="server" WIDTH="87px" CLIENT_LINK_CLICK="ShowDescDiv()" BUTTON_TEXT="Backwards" />
                            </div>
                         </div>--%>
                    </div>
                    </div>
                </asp:Panel>
            <%--</rad:RadPageView>

            <rad:RadPageView runat="server" ID="RadPageView2" TabIndex="2" >--%>
                <div style="position: relative;">
                        <asp:Panel runat="server" ID="Panel3">
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
                            <div id="WidthMediaDiv" class="topDiv" style="width: 668px;">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="buttonSubmit"/>
                                <asp:PostBackTrigger ControlID="MusicUploadButton" />
                            </Triggers>
                            <ContentTemplate>
                                <a href="#top"></a>
                                <script type="text/javascript">
                                                function ShowMDiv()
                                                {
                                                    var theDiv = document.getElementById('WidthMDiv');
                                                    var loadDiv = document.getElementById('LoadMDiv');
                                                    
                                                    loadDiv.style.height = theDiv.offsetHeight +'px';
                                                    loadDiv.style.display = 'block';
                                                    
                                                    return true;
                                                }
                                            </script>
                                            <div id="LoadMDiv" style="z-index: 10000;width: 670px;display: none;position: absolute;">
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
                                    <div id="WidthMDiv" class="topDiv" style=" border-top: solid 1px #dedbdb;width: 614px;">
                                        <div class="EventDiv" style="padding-top: 10px;padding-left: 10px;">
                                            <asp:CheckBox ID="MusicCheckBox" onclick="ShowMDiv()" runat="server" Text="Include Music  <span class='NavyLink12'>[Upload up to 3. Required format is .mp3]</span>" OnCheckedChanged="EnableMusicPanel" AutoPostBack="true" />
                                                <asp:Panel runat="server" ID="MusicPanel" Visible="false">
                                                    <div class="topDiv" style="clear: both; width: 600px;">
                                                        <div style="float: left; padding-left: 30px;">
                                                            <div style="float: left; padding-top: 4px;">
                                                                <asp:FileUpload runat="server" ID="MusicUpload" Width="230px" EnableViewState="true" />
                                                            </div>
                                                            <div style="float: left;padding-left: 5px;padding-top:3px;">
                                                             <ctrl:BlueButton CLIENT_LINK_CLICK="ShowMDiv()" runat="server" WIDTH="66px" BUTTON_TEXT="Upload" ID="MusicUploadButton" />
                                                           
                                                            </div>

                                                        </div>
                                                        <div style="float: right; padding-right: 70px;">
                                                            <asp:CheckBoxList CssClass="MusicCheckBoxes" ID="SongCheckList" runat="server"></asp:CheckBoxList>
                                                            <div style="padding-top: 10px;">
                                                                <ctrl:BlueButton CLIENT_LINK_CLICK="ShowMDiv()" ID="DeleteSongButton"   WIDTH="54px"  Visible="false" runat="server" BUTTON_TEXT="Nix It" />
                                                            </div>
                                                        </div>
                                                        
                                                    </div>
                                                </asp:Panel>
                                        </div>
                                   
                                        <div style="padding-left: 10px; width: 632px; clear: both;padding-top: 20px;">
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
                                        <asp:CheckBox ID="MainAttractionCheck" onclick="ShowMDiv()" Text="Include Pictures and Videos <span  class='NavyLink12'>[20 max]</span><div style='float: right;padding-right: 400px;position: relative;'><div class='Text12' style='position: absolute;float: left;'><div id='div5' style='z-index: 10000; left: -100px; top: 20px;padding: 10px;position: absolute;display: none;background-color: white; width: 150px; border: solid 2px #6fa8bf;'>If you're uploading photos/images make sure you have specified a descriptive name for your file. It will help search engines, like google, find your page more effectively.</div><img onmouseout='showText()' onmouseover='hideText()' src='http://hippohappenings.com/NewImages/HelpIconNew.png' /></div></div>" 
                                        runat="server" AutoPostBack="true" OnCheckedChanged="EnableMainAttractionPanel" />
                                            <asp:Panel runat="server" ID="MainAttractionPanel" Visible="false" Enabled="false"> 
                                                <div style="padding-left: 30px;">
                                                    <asp:RadioButtonList onchange="ShowMDiv()" runat="server" ID="MainAttracionRadioList" AutoPostBack="true" 
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
                                                                            <ctrl:BlueButton ID="ImageButton6" WIDTH="66px"  CLIENT_LINK_CLICK="ShowMDiv()" runat="server" BUTTON_TEXT="Upload" />
                                                                            
            <%--                                                                <asp:ImageButton runat="server" ID="ImageButton6" 
                                                                                PostBackUrl="BlogEvent.aspx#top" OnClick="VideoUpload_Click"
                                                                                 ImageUrl="image/MakeItSoButton.png" 
                                                                                onmouseout="this.src='image/MakeItSoButton.png'" 
                                                                                onmouseover="this.src='image/MakeItSoButtonSelected.png'"  />
            --%>                                                                </div>
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
                                                                                <ctrl:BlueButton CLIENT_LINK_CLICK="ShowMDiv()" WIDTH="66px"  ID="ImageButton7" runat="server" BUTTON_TEXT="Upload" />
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
                                                                                
                                                                                <asp:Button id="buttonSubmit" OnClientClick="ShowMDiv()" runat="server" CssClass="RadUploadSubmit" OnClick="PictureUpload_Click" text="Submit" />
                                                                            
                                                                                <%--<asp:FileUpload runat="server" ID="PictureUpload" Width="230px" EnableViewState="true" />--%>
                                                                            </div>
                                                                            <div style="float: left;padding-left: 5px; padding-top: 3px;">
                                                                            <%--<ctrl:BlueButton ID="ImageButton5" runat="server" BUTTON_TEXT="Upload" />--%>
                                                                            
            <%--                                                                    <asp:ImageButton runat="server" ID="ImageButton5" 
                                                                                PostBackUrl="BlogEvent.aspx#top" OnClick="PictureUpload_Click"
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
                                                        <ctrl:BlueButton ID="PictureNixItButton" CLIENT_LINK_CLICK="ShowMDiv()"  WIDTH="54px"  runat="server" BUTTON_TEXT="Nix It" />
                                                    </div>
                                                </asp:Panel>
                                            </asp:Panel>
                                            <br /><br />
                                    </div>
                                    </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                    <%--<div class="topDiv" align="right" style="padding-right: 50px;width: 660px;padding-top: 10px;clear:both;">
                                    
                                        <div style="float: right;">
                                            <ctrl:BlueButton ID="MediaOnwardsButton" CLIENT_LINK_CLICK="ShowMediaDiv()" WIDTH="76px"  runat="server" BUTTON_TEXT="Onwards" />
                                        </div>
                                        <div style="float: right;padding-right: 10px;">
                                            <ctrl:BlueButton ID="ImageButton3" WIDTH="87px" CLIENT_LINK_CLICK="ShowMediaDiv()" runat="server" BUTTON_TEXT="Backwards" />
                                        </div>
                                    </div>--%>
                                </div>
                        </asp:Panel>
                 </div>   
            <%--</rad:RadPageView>

            <rad:RadPageView runat="server" ID="RadPageView3" TabIndex="3" >--%>
                
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
                    <div class="EventDiv" id="WidthCatDiv" style=" width: 614px; border-top: solid 1px #dedbdb;">
                        <div style="padding-left: 10px; width: 550px;">
                            
                            <div>
                            <div class="topDiv">
                                <div style="float: left;">
                                    <h2>Select Categories for Your Event<span class="Asterisk">* </span></h2>
                                </div>
                                <div style="float: left; padding-left: 5px;padding-top: 3px;">
                                    <asp:Image CssClass="HelpImage" runat="server"  ID="Image5" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
                                    <rad:RadToolTip ID="RadToolTip6" runat="server" Skin="Sunset" 
                                    Width="200px" ManualClose="true" ShowEvent="onclick" 
                                    Position="MiddleRight" RelativeTo="Element" TargetControlID="Image5">
                                    <div style="padding: 10px;"><label>Selecting categories that most represent 
                                    your event will help your viewers find it faster via our search page.</label></div>
                                    </rad:RadToolTip>
                                </div>
                            </div>
                            <table>
                                        <tr>
                                            <td valign="top">
                                                    <%--<asp:CheckBoxList runat="server" ID="CategoriesCheckBoxes" 
                                                    RepeatColumns="4" RepeatDirection="horizontal">
                                                    
                                                    </asp:CheckBoxList>--%>
                                                    <div class="FloatLeft" style="width: 150px;">
                                                        <rad:RadTreeView Skin="Vista"  Width="150px" runat="server"  
                                                        ID="CategoryTree" DataFieldID="ID" DataSourceID="SqlDataSource1" 
                                                        DataFieldParentID="ParentID"
                                                        CheckBoxes="true">
                                                        <DataBindings>
                                                             <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                             TextField="Name" Expanded="false" ValueField="ID" />
                                                         </DataBindings>
                                                        </rad:RadTreeView>
                                                        <asp:SqlDataSource runat="server" ID="SqlDataSource1" 
                                                            SelectCommand="SELECT * FROM EventCategories WHERE ID < 26 OR ID=52 OR ID=53 OR ID= 34 OR ID=40 OR ID=50 OR ID=51 OR ID=35 OR ID=47 OR ID=56 OR ID=57 OR ID=58 OR ID=61 OR ID=62 ORDER BY Name ASC"
                                                            ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                        </asp:SqlDataSource>
                                                    </div>
                                                    
                                           
                                            </td>
                                            <td valign="top">
                                                <asp:CheckBoxList runat="server" ID="CheckBoxList1" 
                                                    RepeatColumns="4" RepeatDirection="horizontal">
                                                    
                                                    </asp:CheckBoxList>
                                                    <div class="FloatLeft" style="width: 150px;">
                                                        <rad:RadTreeView Skin="Vista"  Width="150px" runat="server"  
                                                        ID="RadTreeView2" DataFieldID="ID" DataSourceID="SqlDataSource3"
                                                        DataFieldParentID="ParentID"
                                                        CheckBoxes="true">
                                                        <DataBindings>
                                                             <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                             TextField="Name" Expanded="false" ValueField="ID"  />
                                                         </DataBindings>
                                                        </rad:RadTreeView>
                                                        <asp:SqlDataSource runat="server" ID="SqlDataSource3" 
                                                            SelectCommand="SELECT * FROM EventCategories WHERE ID >= 26 AND  NOT(ID= 52) AND  NOT(ID= 53) AND NOT(ID= 34) AND NOT(ID=40) AND NOT(ID=50) AND NOT(ID=51) AND NOT (ID=62) AND NOT (ID=61) AND NOT (ID=35) AND NOT (ID=47) AND NOT (ID=56) AND NOT (ID=57) AND NOT (ID=58) ORDER BY Name ASC"
                                                            ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                        </asp:SqlDataSource>
                                                    </div>
                                            </td>
                                            <%--<td valign="top">
                                                <div style=" width: 150px; color: #cccccc;">
                                                    <rad:RadTreeView Skin="Vista"  ForeColor="#cccccc"  Width="150px"  runat="server"  
                                                    ID="RadTreeView1" DataFieldID="ID" 
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
                                                <div style=" width: 150px; color: #cccccc;">
                                                    <rad:RadTreeView Skin="Vista"  ForeColor="#cccccc" Width="150px"  runat="server"  
                                                    ID="RadTreeView3" DataFieldID="ID"
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
                                                     
                        <div style="width: 550px;" class="topDiv">
                            <div style="padding-top: 10px;float: left;padding-bottom: 10px;">
                                <label>Don't see a category you want? Suggest one!</label>
                                <asp:TextBox runat="server" ID="CategoriesTextBox"></asp:TextBox>
                            </div>
                            <div style="padding-top: 9px;padding-left: 10px;float: left;padding-bottom: 10px;">   
                              <ctrl:BlueButton ID="ImageButton11" CLIENT_LINK_CLICK="ShowCatDiv()" runat="server"   WIDTH="122px"  BUTTON_TEXT="Send Suggestion" />
                            </div>
                        </div>
                        </div>
                                                     </div>
                        <%--<div class="topDiv" align="right" style="padding-right: 50px;width: 660px;clear: both;padding-top: 10px;">
                            
                            <div style="float: right;">
                                <ctrl:BlueButton ID="CategoryOnwardButton" CLIENT_LINK_CLICK="ShowCatDiv()" WIDTH="76px"  runat="server" BUTTON_TEXT="Onwards" />
                            </div>
                            <div style="float: right;padding-right: 10px;">
                                <ctrl:BlueButton ID="ImageButton9" CLIENT_LINK_CLICK="ShowCatDiv()" WIDTH="87px" runat="server" BUTTON_TEXT="Backwards" />
                            </div>
                         </div>--%>
                            
                            
                         
                    
                    </div>
                  </div>
                </asp:Panel>
            <%--</rad:RadPageView>
            <rad:RadPageView runat="server" ID="RadPageView4" TabIndex="4" >--%>
                <asp:Panel runat="server" ID="Panel6" Visible="false">          
                   <div style="position: relative;">
                    <div class="Text12" style=" width: 668px;">
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
                    <div id="WidthAllDiv">
                    <asp:UpdatePanel runat="server" ID="FetUpdatePanel" UpdateMode="conditional">
                        <%--<Triggers>
                            <asp:PostBackTrigger ControlID="BlueButton10" />
                            <asp:PostBackTrigger ControlID="BlueButton9" />
                        </Triggers>--%>
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
                            <div id="WidthFetDiv" style="padding-left: 10px; width: 614px; border-top: solid 1px #dedbdb;">
                            <div class="topDiv">
                                <%--<div style="float: left;padding-top: 10px; padding-right: 5px;">
                                    <asp:CheckBox runat="server" AutoPostBack="true" OnCheckedChanged="CheckFeatured" ID="FeatureCheckBox"  />    
                                </div>--%>
                                <div style="float: left;">
                                    <h1>Want To Feature Your Event?</h1>
                                </div>
                                <%--<div style="float: left; padding-left: 5px;padding-top: 10px;">
                                    <asp:Image CssClass="HelpImage" runat="server"  ID="Image6" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
                                    <rad:RadToolTip ID="RadToolTip7" runat="server" Skin="Sunset" 
                                    Width="200px" ManualClose="true" ShowEvent="onclick" 
                                    Position="MiddleRight" RelativeTo="Element" TargetControlID="Image6">
                                    <label></label>
                                    </rad:RadToolTip>
                                </div>--%>
                                <div style="float: right;padding-top: 10px;">
                                    <ctrl:BlueButton ID="BlueButton8" runat="server" CLIENT_CLICK="OpenFeaturePricing('E');"  WIDTH="160px" BUTTON_TEXT="Featured Event Pricing >" />
                                </div>
                            </div>
                            <div align="center" style="padding-top: 30px;">
                                <div class="FeaturePrice">
                                    <h2>It's only $<asp:Literal runat="server" ID="PriceLiteral"></asp:Literal> / day!</h2>
                                </div>
                                Your featured event will come up first in our event search results on the event search page. In addition, your event will be shown first on our home page 'Stuff To Do' strip if it falls under the dates a viewer specifies (i.e. Today, Tomorrow, etc.). You can always choose to feature your event later, after you have posted it, by going to the 'Searches and Pages' page from your My Account page. 
                                
                            </div>
                             <div align="center" class="topDiv" style="padding-top: 30px; padding-bottom: 30px;">
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
                                           <b><div>Your major city is <asp:Label runat="server" ForeColor="Red" ID="MajorCity"></asp:Label>. If this is not correct and you have 
                                           created a new venue, make sure the zip code on that venue is correct.</div></b><br /><br />
                                        </asp:Panel>
                                        <div style="float: left;">
                                            <h2>Pick the dates you want your event featured</h2>
                                        </div>
                                        <div style="clear: both; padding-bottom: 5px;">
                                            *Days on which your event has already been featured show up in gray below.
                                        </div>
                                    </div>
                                        <div class="topDiv" style="border-top: solid 1px #dedbdb; width: 614px; padding-top: 10px;">
                                            <div style="float: left; width: 215px;position: relative;">
                                                
                                                <rad:RadDatePicker runat="server" ID="FeaturedDatePicker" Skin="Web20"></rad:RadDatePicker><br />
                                                <div style="padding-top: 5px;">[7 days max, for next 30 days]</div>
                                            </div>
                                            <div style="float: left; width: 130px; padding-top: 50px;">
                                                 
                                                    <ctrl:BlueButton ID="BlueButton1" runat="server" CLIENT_LINK_CLICK="ShowFetDiv()" BUTTON_TEXT="Add Date >>" />
                                                 
                                                     <ctrl:BlueButton ID="BlueButton4" runat="server" CLIENT_LINK_CLICK="ShowFetDiv()" 
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
                                            <div style="padding: 10px;line-height: 20px;"><label>Selecting no search terms will mean your event will pop 
                                            up in searches where no keywords are entered. If you select any keywords, your featured 
                                            event will pop up in searches where those keywords are entered and in searches 
                                            where no keywords are entered as well. Each keyword is limited to four events, and so, picking a 
                                            keyword guarantees that your event will show up in the top four results of a search with your keyword. 
                                            In the searches without keywords, your event will show up in the top four results chosen randomly 
                                            per each time searched. Meaning, if you do not include search terms your event is <u>not</u> 
                                            guaranteed to show up in the first four results if there is over 4 featured events for your location.<br /><br />
                                            If this all sounds too complicated, strictly speaking, it is better to put in search terms!
                                            </label></div>
                                            </rad:RadToolTip>
                                        </div>
                                    </div>
                                        <div class="topDiv" style="border-top: solid 1px #dedbdb; width: 614px; padding-top: 10px;">
                                            <div style="float: left; width: 215px;">
                                                <asp:TextBox runat="server" ID="TermsBox"></asp:TextBox>
                                            </div>
                                            <div style="float: left; width: 130px; padding-top: 50px;">
                                                 <ctrl:BlueButton ID="BlueButton5" runat="server" CLIENT_LINK_CLICK="ShowFetDiv()" BUTTON_TEXT="Add Term >>" />
                                                 <ctrl:BlueButton ID="BlueButton6" runat="server" CLIENT_LINK_CLICK="ShowFetDiv()"
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
                                <div class="topDiv">
                                    <div class="topDiv">
                                        <div style="float: left;">
                                            <h2>Your Pricing</h2>
                                        </div>
                                        <div style="float: left; padding-left: 5px;padding-top: 4px;">
                                            <asp:Image CssClass="HelpImage" runat="server"  ID="Image7" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
                                            <rad:RadToolTip ID="RadToolTip8" runat="server" Skin="Sunset" 
                                                Width="200px" ManualClose="true" ShowEvent="onclick" 
                                                Position="MiddleRight" RelativeTo="Element" TargetControlID="Image7">
                                                <div style="padding: 10px;"><label>Your price depends on the number of members in the location you are featuring the event,
                                                number of days, number of search terms, and number of existing featured events on the days you 
                                                have chosen. The more featured events, the less it costs to feature an event.</label></div>
                                            </rad:RadToolTip>
                                        </div>
                                    </div>
                                    
                                    <table align="center" style="border: solid 1px #dedbdb;" width="100%">
                                        <tr>
                                            <th>Day</th>
                                       
                                            <th>Standard Rate / day</th>
                                        
                                            <th>Adjustment for Members / day</th>
                                        
                                            <th>Adjustment for Events / day</th>
                                     
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
                                        <ctrl:BlueButton ID="BlueButton7" BUTTON_TEXT="Update" runat="server" CLIENT_LINK_CLICK="ShowEveryDiv()" />
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
                                <ctrl:BlueButton ID="BlueButton3" CLIENT_LINK_CLICK="ShowAllDiv()" WIDTH="87px" runat="server" BUTTON_TEXT="Backwards" />
                            </div>
                         </div>--%>
                     </div>
                    </div>
                   </div>
                </asp:Panel>
            <%--</rad:RadPageView>
            <rad:RadPageView runat="server" ID="RadPageView5" TabIndex="5" >--%>
                <asp:Panel runat="server" ID="Panel5" DefaultButton="PostItButton"> 
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
                        <div class="Text12"  style="width: 668px;">
                         
                            <div>
<%--                            <h1 class="SideColumn">You're Almost Done!</h1>
                                <div style="width: 640px;padding-left: 10px;">
                                    Preview your post information below and read the Terms 
                                    and Conditions. Go backwards if you'd like to change anything. If you decided to 
                                    feature your event, please fill in the payment information.
                                </div>--%>
                                <div style="width: 614px;border-top: solid 1px #dedbdb;border-bottom: solid 1px #dedbdb;">
                                    <asp:Panel runat="server" ID="OwnerPanel">
                                    <div style="padding-bottom: 10px; padding-top: 10px;padding-left: 10px;">
                                    <asp:CheckBox runat="server" ID="OwnerCheckBox" Text="Check here if you would like to be the 'Owner' of this event."  />
                                    <asp:Label runat="server" ID="PriceErrorLabel" ForeColor="red"></asp:Label>
                                    <asp:Image runat="server" CssClass="HelpImage" ID="QuestionMark6" ImageUrl="NewImages/HelpIconNew.png"></asp:Image>
                                                                                   <rad:RadToolTip ID="RadToolTip5" runat="server" Skin="Sunset" Width="200px" Height="200px" ManualClose="true" ShowEvent="onclick" Position="MiddleRight" RelativeTo="Element" TargetControlID="QuestionMark6">
                                            <div style="padding: 10px;"><label>When you become the owner of this event, you will be 
                                            responsible for approving any change request users post before the 
                                            changes go live. Being the owner, you have 7 days to approve/reject the changes that 
                                            are submitted by other users. If you do not reply, your ownership 
                                            will be forfeited and this event's ownership will be up for grabs to everyone.
                                            You do not have to be the 'Owner' of this event, 
                                            but keep in mind that anyone will be able to freely edit this event 
                                            until someone chooses to become the owner. 
                                            
                                                </label></div>
                                            </rad:RadToolTip>
                                    </div>
                                    </asp:Panel>
                                </div>
                                <asp:Panel runat="server" ID="PaymentPanel" Visible="false">
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
                                </asp:Panel>
                                
                                <div>
                                
                                <div style="padding-left: 10px;padding-top: 10px;">
                                    <h2 class="SideColumn">Terms and Conditions</h2>
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
                                    <ctrl:BlueButton ID="PostItButton" runat="server" CLIENT_LINK_CLICK="ShowPostDiv()" WIDTH="60px" BUTTON_TEXT="Post It" />
                                </div>
                                <%--<div style="float: right;padding-right: 10px;">
                                    <ctrl:BlueButton ID="ImageButton12" runat="server" WIDTH="87px" CLIENT_LINK_CLICK="ShowPostDiv()" BUTTON_TEXT="Backwards" />
                                </div>--%>
                             </div>
                        </div>                
                        <div style="width: 658px; margin-top: 20px; padding: 5px;">
                            <asp:Panel runat="server" ID="EventPanel" Visible="false">
                               <div style="padding-top: 5px;" class="topDiv">
                                    <div style="width: 570px; clear: both;">
                                        <h1><asp:Label runat="server" ID="ShowEventName"></asp:Label></h1>
                                        
                                    </div>
                                    <div style="width: 570px;" >
                                    
                                    
                                        <asp:Literal runat="server" ID="ShowVideoPictureLiteral">
                                        </asp:Literal>
                                        <div>
                                            <div style="float: right; width: 430px;padding-left: 5px;padding-right: 10px;">
                                            <asp:Panel runat="server" ID="RotatorPanel" Visible="false">
                                                <rad:RadRotator runat="server" Skin="Vista" ScrollDuration="200" 
                                                ID="Rotator1" ItemHeight="250" ItemWidth="412"  
                                                Width="440px" Height="250px" RotatorType="Buttons">
                                                </rad:RadRotator>
                                            </asp:Panel>
                                            
                                            
                                            </div>
                                            <asp:Label runat="server" ID="ShowVenueName" CssClass="Green12LinkNF"></asp:Label><br />
                                            <asp:Label runat="server" ID="ShowDateAndTimeLabel" CssClass="DateTimeLabel"></asp:Label><br />
                                             <asp:Label runat="server" ID="ShowRestOfDescription" CssClass="Text12"></asp:Label>
                                        
                                        
                                            <asp:Label runat="server" ID="ShowDescriptionBegining" CssClass="EventBody"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                
                            </asp:Panel>
                        </div>
                    </div>
                </asp:Panel>
           <%--  </rad:RadPageView>

        </rad:RadMultiPage>--%>
        </div>
        </div>
        </div>
</ContentTemplate>
</asp:UpdatePanel>
</div>
        <div style="position: absolute; left: 677px;">
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

