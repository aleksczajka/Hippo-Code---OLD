<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ValidateRequest="false" 
AutoEventWireup="true" 
CodeFile="EnterVenue.aspx.cs" 
Inherits="EnterVenue" Title="Enter local Venue | HippoHappenings" %>
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
            ID="MessageRadWindowManager" ReloadOnShow="true" Modal="true" DestroyOnClose="true" RestrictionZoneID="RestrictionZone"
            runat="server">
                <Windows>
                    <rad:RadWindow Width="300" ClientCallBackFunction="OnClientClose" EnableEmbeddedSkins="true" 
                    VisibleStatusbar="false" Skin="Black" Height="300" ID="MessageRadWindow" Title="Alert" runat="server">
                    </rad:RadWindow>
                </Windows>
            </rad:RadWindowManager>
            <script type="text/javascript">
            function OnClientClose(oWnd, args)
            {

                window.location = args;
            }
            </script>
<a id="top"></a>
<div style="padding-bottom: 100px;">
    <asp:Label runat="server" ID="ErrorLabel" ForeColor="Red"></asp:Label>
    <asp:Panel runat="server" ID="LoggedOutPanel">
        <div id="topDiv5">
            <div style="float: left; width: 390px;">
               
                            <span style="font-family: Arial; font-size: 30px; color: #cccccc;">Enter Venue</span>
                <%--<div class="AddLinkGoing" style="float: left;padding-left: 20px; padding-top: 30px;">
                   <label>You must be <asp:HyperLink ID="HyperLink1" runat="server" CssClass="AddLink" NavigateUrl="~/UserLogin.aspx" Text="logged in"></asp:HyperLink> to submit a new venue! 
                    <br />
                    Please <asp:HyperLink ID="HyperLink2" runat="server" CssClass="AddLink" NavigateUrl="~/Register.aspx" Text="register"></asp:HyperLink> if you do not have an account.
                   </label>
                </div>--%>
                <br /><br />
                <span style="font-family: Arial; font-size: 12px; color: #cccccc; line-height: 20px;">
            <asp:Label runat="server" ID="WelcomeLabel"></asp:Label>
          </span>
            </div>
            <div style="float: right;">
                <%-- <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" 
            codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0" 
            width="431" height="575" title="banner" wmode="transparent">  
            <param name="movie" value="gallery.swf" />  
            <param name="quality" value="high" />
            <param name="wmode" value="transparent"/>   
                <embed src="gallery.swf" quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer" 
                type="application/x-shockwave-flash" width="431" height="575" wmode="transparent">
                </embed>
            </object>--%>
            <ctrl:Ads runat="server" />
            </div>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="BigEventPanel">
    <div style="font-family: Arial; font-size: 30px; color: #cccccc; padding-bottom: 10px;">
        <asp:Label runat="server" ID="TitleLabel" Text="Enter a Venue"></asp:Label>
        <asp:Label runat="server" ID="UserNameLabel" Visible="false"></asp:Label>
    </div>
    <div id="topDiv6">
        <div style="float: left; width: 680px;">
        <div style="margin-left: 11px;">
            <rad:RadTabStrip runat="server" ID="EventTabStrip"
            Skin="HippoHappenings" CssClass="EventTabStrip" OnTabClick="TabClick"  Height="40px"
            EnableEmbeddedSkins="false" MultiPageID="BlogEventPages" SelectedIndex="0">
                <Tabs>
                    <rad:RadTab runat="server" PageViewID="TabOne" ImageUrl="images/DetailsGreenFull.gif" 
                    HoveredImageUrl="images/DetailsGreenFull.gif" TabIndex="0" 
                    DisabledImageUrl="images/DetailsGreenFull.gif" 
                    SelectedImageUrl="images/DetailsGreenFull.gif"  >

                    </rad:RadTab>
                    <rad:RadTab runat="server" Enabled="false"  PageViewID="RadPageView1" 
                    ImageUrl="images/DescriptionGreenFull.gif" 
                    HoveredImageUrl="images/DescriptionGreenFull.gif"  TabIndex="1"  
                    DisabledImageUrl="images/DescriptionGreenEmpty.gif" 
                    SelectedImageUrl="images/DescriptionGreenFull.gif" >
                        
                    </rad:RadTab>
                    <rad:RadTab runat="server"  Enabled="false"  PageViewID="RadPageView2" 
                    ImageUrl="images/MediaGreenFull.gif" 
                    HoveredImageUrl="images/MediaGreenFull.gif"  TabIndex="2" 
                    DisabledImageUrl="images/MediaGreenEmpty.gif" 
                    SelectedImageUrl="images/MediaGreenFull.gif" ></rad:RadTab>
                    <rad:RadTab runat="server"  Enabled="false"  PageViewID="RadPageView3" 
                    ImageUrl="images/CategoriesGreenFull.gif" 
                    HoveredImageUrl="images/CategoriesGreenFull.gif"  TabIndex="3" 
                    DisabledImageUrl="images/CategoriesGreenEmpty.gif" 
                    SelectedImageUrl="images/CategoriesGreenFull.gif" ></rad:RadTab>
                    <rad:RadTab runat="server"  Enabled="false"  PageViewID="RadPageView4" 
                    ImageUrl="images/PostGreenFull.gif" 
                    HoveredImageUrl="images/PostGreenFull.gif" 
                    DisabledImageUrl="images/PostGreenEmpty.gif"  TabIndex="4" 
                    SelectedImageUrl="images/PostGreenFull.gif" ></rad:RadTab>
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
                        <h1>Details</h1>
                            <table>
                                <tr>
                                    <td width="250px;">
                                    <label>All HTML tags will be removed except for links. However, the header cannot contain links at all.</label>
                                            <br /><br />
                                            <script type="text/javascript">
                                                    
//                                                                            function OnClientLoad(editor)
//                                                                            {
//                                                                                editor.attachEventHandler("onkeyup", CountChars);
//                                                                                editor.attachEventHandler("onkeypress", function(evt){if(evt.keyCode == 13)return false;});
//                                                                            }  
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
                                        <label>Phone</label><br /><asp:TextBox runat="server" ID="PhoneTextBox"></asp:TextBox><br />
                                        <label>Email</label><br /><asp:TextBox runat="server" ID="EmailTextBox"></asp:TextBox><br />
                                        <label>Web Site</label><br /><asp:TextBox runat="server" ID="WebSiteTextBox"></asp:TextBox><br />
                                         <br /> <label><span style='color: #ff770d; font-weight: bold;'><span style='color: #1fb6e7; font-weight: bold;'>* </span>reqiured fields</span></label>
<%--                                     
   <asp:RegularExpressionValidator runat="server" ValidationExpression="^(\()?(787|939)(\)|-)?([0-9]{3})(-)?([0-9]{4}|[0-9]{4})$" ControlToValidate="PhoneTextBox" ErrorMessage="Phone is not in the appropriate format."></asp:RegularExpressionValidator>
--%>    <%--                                 
                                        <label style="padding-right: 10px;">Location</label><br />
                                        <asp:TextBox runat="server" ID="SpecificLocationTextBox"></asp:TextBox> <label style="font-family: Arial; font-style:italic;font-size: 12px; color: #666666;">[ex: Multnomah, Downtown, sw, etc. This will help your users find your venue faster]</label>
    --%>                                </td>
    
                                    <td valign="top" width="330px">
                                    <div style="padding-left: 25px; padding-top: 30px;">
                                    <h1 class="Green"><span style='color: #1fb6e7; font-weight: bold;'>* </span>Location</h1>
                                        <table cellpadding="0" cellspacing="5">
                                            <tr>
                                                <asp:Panel runat="server" ID="USPanel">
                                                <td colspan="2">
                                                    
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <label><span style='color: #1fb6e7; font-weight: bold;'>* </span>Street No.</label>
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
                                            <tr>
                                                <td>
                                                    <label style="padding-right: 10px;"><span style='color: #1fb6e7; font-weight: bold;'>* </span>Country</label><br />
                                                    <asp:DropDownList OnSelectedIndexChanged="ChangeState" AutoPostBack="true" runat="server" DataSourceID="SqlDataSourceCountry" DataTextField="country_name" DataValueField="country_id" ID="CountryDropDown"></asp:DropDownList>
                                                    <asp:SqlDataSource runat="server" ID="SqlDataSourceCountry" SelectCommand="SELECT * FROM Countries"
                                                    ConnectionString="<%$ ConnectionStrings:Connection %>"></asp:SqlDataSource>
                                                </td>
                                                
                                                <td style="padding-left: 3px;">
                                                    <label><span style='color: #1fb6e7; font-weight: bold;'>* </span>State</label><br />
                                                    <asp:Panel runat="server" ID="StateTextBoxPanel">
                                                        <ctrl:HippoTextBox ID="StateTextBox" runat="server" TEXTBOX_WIDTH="100" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
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
                                                                <label><span style='color: #1fb6e7; font-weight: bold;'>* </span>City</label>
                                                            </td>
                                                            <td style="padding-left: 3px;">
                                                                <label><span style='color: #1fb6e7; font-weight: bold;'>* </span>Zip</label>
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
                        <div style="padding-left: 30px; padding-top: 10px; padding-bottom: 20px; width: 550px;">
                            
                            <h1>Description</h1>
                            <div style="padding-left: 20px;">
                               
                            
                                <label><span style='color: #1fb6e7; font-weight: bold;'>* </span>Description</label><br /><br /><span style="font-family: Arial; font-size: 10px; color: #ff770d; font-weight: bold;"> min 50 characters
                                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                  <span style="color: Red;" id="CharsDivEditor"></span>
                                </span><br />
<%--                                <asp:TextBox ID="DescriptionTextBox" ON_CLIENT_KEYPRESS="CountChars(event)" runat="server" CssClass="AddCommentTextBox" Height="150px" TextMode="multiLine" Wrap="true"></asp:TextBox>
--%>                            
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
                                                                        if(this.activeElement.textContent.length > 50)
                                                                            theDiv.innerHTML = "characters needed: 0";
                                                                        else
                                                                            theDiv.innerHTML = "characters needed: "+ (50 - this.activeElement.textContent.length).toString();
                                                                    }
                                                                    else
                                                                    {
                                                                        var theEditor = $find("<%=DescriptionTextBox.ClientID %>");
                                                                        if(theEditor.get_contentArea().innerText.length > 50)
                                                                            theDiv.innerHTML = "characters needed: 0";
                                                                        else
                                                                            theDiv.innerHTML = "characters needed: "+ (50 - theEditor.get_contentArea().innerText.length).toString();
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
                                                    Skin="Black" runat="server" 
                                                    ID="DescriptionTextBox" Height="200px" Width="580px"
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
                           <div style="padding-left: 30px; width: 632px; clear: both; padding-top: 10px;" class="topDiv">
                               <h1>Media</h1>
                               <div style="padding-left: 20px;">
                                <asp:CheckBox ID="MainAttractionCheck" Text="Main Attraction <span style='color: #ff770d;'>[these will all be shown together in a rotator window. 20 is the max]</span>" runat="server" AutoPostBack="true" OnCheckedChanged="EnableMainAttractionPanel" />
                                    <asp:Panel runat="server" ID="MainAttractionPanel" Enabled="false" Visible="false"> 
                                        <div style="padding-left: 30px;" class="topDiv">
                                            <asp:RadioButtonList runat="server" ID="MainAttracionRadioList" AutoPostBack="true" 
                                            OnSelectedIndexChanged="ShowSliderOrVidPic">
                                                <asp:ListItem Text="Add YouTube Video" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Add Picture" Value="1"></asp:ListItem>
                                            </asp:RadioButtonList>
                                            <div class="topDiv" style="padding-left: 30px; border: solid 4px #515151; width: 500px; padding-bottom: 10px; padding-left: 30px;">
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
                                        <div style="float: right; padding-right: 70px;" class="EventDiv">
                                        <label>Your Pics and Videos </label><br />
                                                                    <asp:CheckBoxList CssClass="MusicCheckBoxes" ID="PictureCheckList" runat="server"></asp:CheckBoxList>
                                                                    <asp:ImageButton Visible="false" runat="server" OnClick="SliderNixIt" ID="PictureNixItButton" ImageUrl="image/NixItButton.png" 
                                                                    onmouseout="this.src='image/NixItButton.png'" onmouseover="this.src='image/NixItButtonSelected.png'"  />
                                                                </div>
                                    </asp:Panel>
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

            <rad:RadPageView runat="server" ID="RadPageView3" TabIndex="3" >
                
                <asp:Panel runat="server" ID="Panel4" DefaultButton="CategoryOnwardButton">          
                    <div style="background: url('image/TabsBackgroundTop.png'); background-repeat: no-repeat; width: 668px; height: 4px;">
                    </div>
                    <div class="EventDiv" style="background: url('image/TabsBackground.png'); background-repeat: repeat-y;  width: 668px;">
                        <div style="padding-left: 30px; width: 550px; padding-top: 10px;">
                        <h1>Categories</h1>
                        <div style="padding-left: 20px;">
                            <label><span style="font-weight: bold;"><span style='color: #1fb6e7; font-weight: bold;'>* </span>Choose Categories to put your venue under. </span> <span style='color: #ff770d;'>[These will help your viewers find your venue faster]</span></label>
                            <br /><br />
<table>
                                        <tr>
                                            <td valign="top">
                                                    <%--<asp:CheckBoxList runat="server" ID="CategoriesCheckBoxes" 
                                                    RepeatColumns="4" RepeatDirection="horizontal">
                                                    
                                                    </asp:CheckBoxList>--%>
                                                    <div class="FloatLeft" style="width: 150px;">
                                                        <rad:RadTreeView Width="150px" runat="server"  
                                                        ID="CategoryTree" DataFieldID="ID" 
                                                        ForeColor="#cccccc"
                                                        DataSourceID="SqlDataSource1" CheckChildNodes="false"
                                                        DataFieldParentID="ParentID" CheckBoxes="true">
                                                        
                                                        <DataBindings>
                                                            
                                                             <rad:RadTreeNodeBinding Checkable="true" Checked="false"
                                                             TextField="Name" Expanded="false" ValueField="ID" />
                                                         </DataBindings>
                                                        </rad:RadTreeView>
                                                        <asp:SqlDataSource runat="server" ID="SqlDataSource1" 
                                                            SelectCommand="SELECT * FROM VenueCategories WHERE ID <= 92 ORDER BY Name ASC"
                                                            ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                        </asp:SqlDataSource>
                                                    </div>
                                                    
                                               
                                            </td>
                                            <td valign="top">
                                                <asp:CheckBoxList runat="server" ID="CheckBoxList1" 
                                                    RepeatColumns="4" RepeatDirection="horizontal">
                                                    
                                                    </asp:CheckBoxList>
                                                    <div class="FloatLeft" style="width: 150px;">
                                                        <rad:RadTreeView Width="150px" ForeColor="#cccccc" runat="server"  
                                                        ID="RadTreeView2" DataFieldID="ID" DataSourceID="SqlDataSource3"
                                                        DataFieldParentID="ParentID"
                                                        CheckBoxes="true">
                                                        <DataBindings>
                                                             <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                             TextField="Name" Expanded="false" ValueField="ID"  />
                                                         </DataBindings>
                                                        </rad:RadTreeView>
                                                        <asp:SqlDataSource runat="server" ID="SqlDataSource3" 
                                                            SelectCommand="SELECT * FROM VenueCategories WHERE ID > 92 ORDER BY Name ASC"
                                                            ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                        </asp:SqlDataSource>
                                                    </div>
                                            </td>
                                            <td valign="top">
                                                <div style=" width: 150px;">
                                                    <rad:RadTreeView Width="150px"  runat="server"  
                                                    ID="RadTreeView1" DataFieldID="ID" ForeColor="#cccccc" 
                                                    
                                                    DataFieldParentID="ParentID"
                                                    CheckBoxes="true">
                                                    <DataBindings>
                                                         <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                         TextField="Name" Expanded="false" ValueField="ID"  />
                                                     </DataBindings>
                                                    </rad:RadTreeView>
                                                    <asp:SqlDataSource runat="server" ID="SqlDataSource2" 
                                                        SelectCommand="SELECT * FROM VenueCategories "
                                                        ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                    </asp:SqlDataSource>
                                                </div>
                                            </td>
                                            <td valign="top">
                                                <div style=" width: 150px;">
                                                    <rad:RadTreeView Width="150px"  runat="server"  
                                                    ID="RadTreeView3" DataFieldID="ID" ForeColor="#cccccc" 
                                                    
                                                    DataFieldParentID="ParentID"
                                                    CheckBoxes="true">
                                                    <DataBindings>
                                                                         <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                                         TextField="Name" Expanded="false" ValueField="ID" />
                                                                     </DataBindings>
                                                    </rad:RadTreeView>
                                                    <asp:SqlDataSource runat="server" ID="SqlDataSource4" 
                                                        SelectCommand="SELECT * FROM VenueCategories"
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
                            <div style="padding-top: 16px; float: left; padding-left: 10px;">
                                <asp:LinkButton CssClass="AddLink" Text="Suggest" runat="server" 
                                ID="ImageButton11" OnClick="SuggestCategoryClick"
                                onmouseout="this.src='image/MakeItSoButton.png'" 
                                onmouseover="this.src='image/MakeItSoButtonSelected.png'"  />
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
            <rad:RadPageView runat="server" ID="RadPageView4" TabIndex="4" >
                <asp:Panel runat="server" ID="Panel5" DefaultButton="PostItButton"> 
                    <div style="background: url('image/TabsBackgroundTop.png'); background-repeat: no-repeat; width: 668px; height: 4px;">
                    </div>
                    <div class="EventDiv"  style="background: url('image/TabsBackground.png'); background-repeat: repeat-y; width: 668px;">
                        <div style="padding-left: 30px; width: 550px; padding-top: 10px;">
                        <h1>Post It</h1>
                        </div>
                        <div style="padding-left: 50px;">
                                                    <label><span style="font-weight: bold;">You're Almost Done!</span> <br /><span style='color: #ff770d;'>Preview your post below and read the Terms and Conditions. Go Backwards! if you'd like to change anything!</span></label>

                         <asp:Panel runat="server" ID="OwnerPanel">
                            <asp:CheckBox runat="server" ID="OwnerCheckBox" Text="Check here if you would like to be the 'Owner' of this venue."  />

                            <asp:Image CssClass="HelpImage" runat="server"  ID="QuestionMark6" ImageUrl="~/image/helpIcon.png"></asp:Image>
                                                                           <rad:RadToolTip ID="RadToolTip5" runat="server" Skin="Black" Width="200px" Height="200px" ManualClose="true" ShowEvent="onclick" Position="MiddleRight" RelativeTo="Element" TargetControlID="QuestionMark6">
                                    <label>When you become the owner of this venue, you will be 
                                    responsible for approving any change request users post before the 
                                    changes go live.  Being the owner, you have 4 days to approve/reject the changes that 
                                    are submitted by other users. If you do not reply, your ownership 
                                    will be forfeited and this venue's ownership will be up for grabs to everyone.
                                    You do not have to be the 'Owner' of this venue, 
                                    but keep in mind that anyone will be able to freely edit this venue 
                                    until someone chooses to become the owner.
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
                
                
                    <%--<div style=" background-color: #1b1b1b; border: solid 4px #515151; width: 650px; margin-left: 20px; margin-top: 20px; padding: 5px;">
                        <asp:Panel runat="server" ID="EventPanel" Visible="false">
                            <h1 class="EventHeader"><asp:Label runat="server" ID="ShowEventName"></asp:Label></h1>
                            <asp:Label runat="server" ID="ShowVenueName" CssClass="VenueName"></asp:Label><br />
                            <asp:Label runat="server" ID="ShowDateAndTimeLabel" CssClass="DateTimeLabel"></asp:Label><br /><br />
                            
                            
                            <div style="padding-top: 5px;">
                                <asp:Label runat="server" ID="ShowDescriptionBegining" CssClass="EventBody"></asp:Label>
                                <asp:Literal runat="server" ID="ShowVideoPictureLiteral">
                                </asp:Literal>
                                <asp:Panel runat="server" ID="RotatorPanel" Visible="false">
                                    <rad:RadRotator runat="server" ID="Rotator1" ItemHeight="200" ItemWidth="200"  Width="640px" Height="200px" RotatorType="Buttons" >
                                        <ItemTemplate>
                                            <div style="border: solid 2px blue; padding: 3px; margin: 3px;">
                                                <img width="184px" height="184px" src='<%# Container.DataItem %>' alt="Customer Image" />
                                            </div>            
                                        </ItemTemplate>
                                    </rad:RadRotator>
                                </asp:Panel>
                                 <asp:Label runat="server" ID="ShowRestOfDescription" CssClass="EventBody"></asp:Label>
                            </div>
                            
                        </asp:Panel>
                    </div>--%>
                    <div style=" background-color: #1b1b1b; border: solid 4px #515151; width: 850px; margin-left: 10px; margin-top: 20px; padding: 5px;">
                        <asp:Panel runat="server" ID="EventPanel" Visible="false">
                            <h1 class="EventHeader"><asp:Label runat="server" ID="ShowEventName"></asp:Label></h1>
                            <asp:Label runat="server" ID="ShowVenueName" CssClass="VenueName"></asp:Label><br />
                            <asp:Label runat="server" ID="ShowDateAndTimeLabel" CssClass="DateTimeLabel"></asp:Label><br /><br />
                            
                            
                            <div style="padding-top: 5px;" class="topDiv">
                                <div style="float: left; width: 420px;">
                                <asp:Label runat="server" ID="ShowDescriptionBegining" CssClass="EventBody"></asp:Label>
                                <asp:Literal runat="server" ID="ShowVideoPictureLiteral">
                                </asp:Literal>
                                </div>
                                <div style="float: left; width: 410px;">
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
                                 <asp:Label runat="server" ID="ShowRestOfDescription" CssClass="EventBody"></asp:Label>
                            </div>
                            
                        </asp:Panel>
                    </div>
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

