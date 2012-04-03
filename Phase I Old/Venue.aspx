<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ValidateRequest="false" AutoEventWireup="true" 
CodeFile="Venue.aspx.cs" Inherits="Venue" Title="Hippo Venue" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register Src="~/Controls/Poll.ascx" TagName="Poll" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/TagCloud.ascx" TagName="TagCloud" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/PlayerXML/SongPlayer.ascx" TagName="Songs" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/AddToFavorites.ascx" TagName="AddTo" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/SendTxtToCell.ascx" TagName="SendTxt" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/Comments.ascx" TagName="Comments" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/SendMessage.ascx" TagName="SendMessage" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/GlanceDay.ascx" TagName="GlanceDay" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/SendEmail.ascx" TagName="SendEmail" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/HippoRating.ascx" TagName="HippoRating" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/FlagItem.ascx" TagName="Flag" TagPrefix="ctrl" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<rad:RadAjaxPanel runat="server">
<rad:RadCodeBlock runat="server">
<style type="text/css">
#CatDiv{
        background:none;
        
        display:block;
        overflow:hidden;
        border-style:solid;
}
</style>
<script type="text/javascript">
(function() {
var s = document.createElement('SCRIPT'), s1 = document.getElementsByTagName('SCRIPT')[0];
s.type = 'text/javascript';
s.async = true;
s.src = 'http://widgets.digg.com/buttons.js';
s1.parentNode.insertBefore(s, s1);
})();
</script>
<script type="text/javascript">
//    var map = new GMap2(document.getElementById("map_canvas"));
//    map.setCenter(new GLatLng(37.4419, -122.1419), 13);
//    map.setUIToDefault();
//    var myGeographicCoordinates = new GLatLng(myLatitude, myLongitude)
//    
    function OpenCatDiv()
    {
        var CatDiv = document.getElementById('CatDiv');
        
        if(CatDiv.style.height == '0px'){
            var moreless = document.getElementById('morelessA');
            moreless.innerHTML = ' less...';
            div_animate();
            
        }else{
            var moreless = document.getElementById('morelessA');
            moreless.innerHTML = ' more...';
            CatDiv.style.height = '0px';
        }
    
    }
    
 function div_animate()
	{
	div_height= document.getElementById('infoDiv').innerHTML;
	
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

    
    function OnMapClose(radWindow)
                {                  
//                   var oValue = radWindow.argument;
//                   var oArea = document.getElementById("InfoArea");
//                   oArea.value = oValue;
                   window.location.reload();
                    //Another option for passing a callback value
                    //Set the radWindow.argument property in the dialog
                    //And read it here --> var oValue = radWindow.argument;                                        
                    //Do cleanup if necessary
                } 
                
                function CallBackFunction(radWindow, returnValue)
                {                  
//                   var oValue = radWindow.argument;
//                   var oArea = document.getElementById("InfoArea");
//                   oArea.value = oValue;
                   window.location.reload();
                    //Another option for passing a callback value
                    //Set the radWindow.argument property in the dialog
                    //And read it here --> var oValue = radWindow.argument;                                        
                    //Do cleanup if necessary
                } 
                
                function CallBackFunctionOwner(radWindow, returnValue)
                {
                if(returnValue != null && returnValue != undefined)
                    window.location = returnValue;
                }
                
                function OpenRad_Map()
                {
                    var win = $find("<%=MessageRadWindow.ClientID %>");
                    win.setUrl('MapAlert.aspx');
                    win.show(); 
                    win.center(); 
                       
                 }
                 
                 function OpenOwner()
                 {
                    var win = $find("<%=RadWindow1.ClientID %>");
                    win.setUrl('Message.aspx');
                    win.show(); 
                    win.center();
                 }
                 
                 function  ReturnURL()
                {
                    var TA = document.getElementById('TweeterA');
                    TA.href = 'http://twitter.com/home?status=' + window.location;
                }
</script>
</rad:RadCodeBlock>
 <rad:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="true" 
    KeepInScreenBounds="true" VisibleOnPageLoad="false" Behaviors="Close"
ID="MessageRadWindowManager" BackColor="#1b1b1b" Modal="true" DestroyOnClose="false" 
RestrictionZoneID="RestrictionZone"
runat="server">
    <Windows>
        <rad:RadWindow Width="730"  ReloadOnShow="true"
        ClientCallBackFunction="CallBackFunction"  IconUrl="./image/AddIcon.png"  VisibleTitlebar="false"
        EnableEmbeddedSkins="true" Skin="Office2007" Height="550" VisibleStatusbar="false" 
        ID="MessageRadWindow" Title="Add to Favorites" runat="server">
        </rad:RadWindow>
        <rad:RadWindow Width="350"  ReloadOnShow="true"
        ClientCallBackFunction="CallBackFunctionOwner"  IconUrl="./image/AddIcon.png"  VisibleTitlebar="false"
        EnableEmbeddedSkins="true" Skin="Office2007" Height="300" VisibleStatusbar="false" 
        ID="RadWindow1" Title="Owner Alert" runat="server">
        </rad:RadWindow>
     
    </Windows>
</rad:RadWindowManager>   
<asp:Panel runat="server" ID="TopPanel" CssClass="FloatLeft" Visible="false">
<asp:Label runat="server" ID="ErrorLabel" ForeColor="red"></asp:Label>
        <div class="topDiv" style="margin-top: -30px; margin-left: -20px;background-image: url(image/NavBarBackground.gif); background-repeat: repeat-x;height: 25px;vertical-align: middle;">
        <div style="background:url(image/HeaderRound.gif) no-repeat bottom right;height: 25px; vertical-align: middle;">
        <asp:Panel runat="server" ID="ReturnPanel" CssClass="FloatLeft" Visible="false">
        <a style="text-decoration:none; border: 0;  color: #ffffff; font-family: Arial; font-size: 11px; font-weight: bold;" href="VenueSearch.aspx#search">
        <img style="float: left;border:0; padding: 6px;" src="image/BackArrow.png" name="Back to search results" alt="Back to search results" />
        <span style="float: left; position: relative; top: 5px;height: 25px; vertical-align: middle;"> Back to search results&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </span></a>
        </asp:Panel>
        <div style="float: left; margin-top: 5px;">
            <span style=" text-decoration: none; color: #ffffff; font-family: Arial; font-size: 11px; font-weight: bold;height: 25px; vertical-align: middle;">
            <asp:HyperLink runat="server" ForeColor="white" Font-Underline="false" 
            Visible="false" ID="EditLink" Text=" |&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Edit Venue&nbsp" 
                 CssClass="FloatLeft"></asp:HyperLink>
            </span>
        </div>
        <asp:Panel runat="server" ID="OwnerPanel" Visible="false">
            <div style="float: left; margin-top: 5px;">
                <span style=" text-decoration: none; color: #ffffff; font-family: Arial; font-size: 11px; font-weight: bold;height: 25px; vertical-align: middle;">
                <a runat="server" OnClick="OpenOwner();" style="color: White; float: left; text-decoration: none;"
                 ID="HyperLink1">&nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Venue Ownership is Open</a>
                </span>
            </div>
        </asp:Panel>
        </div></div>
        <%--<div style="margin-top: -20px; z-index: 4000; float: right; width: 600px; height: 20px;">
            
                 <asp:HyperLink runat="server" CssClass="AddGreenLink FloatLeft" ID="link" 
                 Text="<< Return to search results" NavigateUrl="~/EventSearch.aspx#search"></asp:HyperLink>
            </div>--%>
            </asp:Panel>
     <div id="topDiv">
            
            <div style="float: left;width: 420px; padding-right: 10px;" align="left">
                <h1 class="EventHeader"><asp:Label runat="server" ID="VenueName"></asp:Label></h1>
                
                
                <div style="padding-bottom: 20px;"><asp:Label runat="server" CssClass="AddressLabel" ID="AddressLabel"></asp:Label><br />
                <asp:Label runat="server" CssClass="AddressLabel" ID="CityState"></asp:Label>
                <label class="AddWhiteLink" style="font-weight: normal;">(<a class="AddLink" onclick="OpenRad_Map();">Map It!</a>)</label>
                <br />
                <asp:Label runat="server" CssClass="AddressLabel" ID="PhoneLabel"></asp:Label>
                <br />
                <ctrl:HippoRating ID="HippoRating1" runat="server" />
                <asp:Literal runat="server" ID="CategoriesLiteral"></asp:Literal>
                </div>
                <div class="topDiv" style="padding-bottom: 30px;">
                <asp:Panel runat="server" ID="CalendarPanel"></asp:Panel><br />
                <asp:Panel runat="server" ID="LoggedInPanel" Visible="false">
                    <ctrl:SendTxt runat="server" ID="SendTxtID" />
                    <ctrl:SendEmail runat="server" THE_TEXT="Send Email with this Info" />
                    <ctrl:Flag ID="Flag1" runat="server" />
                </asp:Panel>
                <asp:Panel ID="LoggedOutPanel" runat="server">
                    
                    <div class="EventDiv">
                        <label class="AddGreenLink">To edit this venue, send email/text with this venue's 
                        information as well as flag this item <a href="Register.aspx" class="AddLink">
                        Register</a> or <a href="UserLogin.aspx" class="AddLink">Log in</a></label>
                    </div>
                       
                </asp:Panel>
                <asp:Panel runat="server" ID="OnlyHippoPanel" Visible="false">
                    <div style="padding-top: 5px;">
                        <script type="text/javascript" src="http://widgets.amung.us/classic.js"></script><script type="text/javascript">WAU_classic('k3nyx4trfy6a')</script>
                    </div>
                </asp:Panel>
                </div>
 <table>
                <tr>
                    <td style="padding-top: 10px;">
                    <div class="addthis_toolbox addthis_default_style ">
                    <a href="http://www.addthis.com/bookmark.php?v=250&amp;username=xa-4cfc36bb62b28389" class="addthis_button_compact">Share</a>
                    <span class="addthis_separator">|</span>
                    <a class="addthis_button_preferred_1"></a>
                    <a class="addthis_button_preferred_2"></a>
                    <a class="addthis_button_preferred_3"></a>
                    <a class="addthis_button_preferred_4"></a>
                    </div>
                    <script type="text/javascript" src="http://s7.addthis.com/js/250/addthis_widget.js#username=xa-4cfc36bb62b28389"></script>
                    <!-- AddThis Button END -->
                    </td>
                </tr>
             </table>
                            <asp:Literal runat="server" ID="DiggLiteral"></asp:Literal>
                                        <br /><br />
                <asp:Label runat="server" ID="ShowDescriptionBegining" Width="430px" CssClass="EventBody"></asp:Label>
                
            </div>
           <div style="float: left; padding-left: 5px; width: 419px;">
                <div class="WhiteHeader">
                    Today at a glance <asp:HyperLink CssClass="AddLink" runat="server" ID="CalendarLink">(click here for the full event's calendar)</asp:HyperLink>
                </div>
                <div>
                    
                    <div style="border: solid 4px White; width: 400px;padding: 10px; background-color: #2b92d7;">
                        <asp:Panel runat="server" ID="EventsPanel"></asp:Panel>
                    </div>
                    
                </div>
                <div style="padding-top: 20px; clear: both;">
                    <asp:Label runat="server" ID="MediaLabel" CssClass="WhiteHeader"></asp:Label>
                    <asp:Literal runat="server" ID="ShowVideoPictureLiteral">
                    </asp:Literal>
                    <asp:Panel runat="server" ID="RotatorPanel">
                       
                        <rad:RadRotator runat="server" AutoPostBack="false"  
                        AppendDataBoundItems="false" ScrollDuration="200" ID="Rotator1" 
                        ItemHeight="250" ItemWidth="400"  
                        Width="440px" Height="250px" RotatorType="Buttons">
                            
                            <%--<ItemTemplate>
                                <div style="border: solid 2px blue; padding: 3px; margin: 3px;">
                                    <img width="184px" height="184px" src='<%# Container.DataItem %>' />
                                </div>            
                            </ItemTemplate>--%>
                        </rad:RadRotator>
                    </asp:Panel>
                </div>
                <div style="float: right; padding-top: 20px;">
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
                        <%--<ctrl:Ads ID="Ads1" runat="server" />--%>
                    </div>
                    
                </div>
                <%--<div style="padding-top: 28px; clear: both;">
                    <ctrl:TagCloud runat="server" TAG_TYPE="VENUE" ID="TagCloud" />
                </div>--%>
        </div>
        </div>
        
        <div>
            <ctrl:Comments ID="TheComments" runat="server" />
        </div>
        <script type="text/javascript">
            ReturnURL();
        </script>
</rad:RadAjaxPanel>

</asp:Content>

