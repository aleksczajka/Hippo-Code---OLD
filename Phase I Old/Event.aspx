<%@ Page Language="C#" ValidateRequest="false" EnableSessionState="true" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
CodeFile="Event.aspx.cs" Inherits="Event" Title="Hippo Event" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register Src="~/Controls/Poll.ascx" TagName="Poll" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/TagCloud.ascx" TagName="TagCloud" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/PlayerXML/SongPlayer.ascx" TagName="Songs" TagPrefix="ctrl" %><%@ Register Src="~/Controls/AddToCalendar.ascx" TagName="AddTo" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/SendTxtToCell.ascx" TagName="SendTxt" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/Comments.ascx" TagName="Comments" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/SendMessage.ascx" TagName="SendMessage" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/SendEmail.ascx" TagName="SendEmail" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/HippoRating.ascx" TagName="HippoRating" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/FlagItem.ascx" TagName="Flag" TagPrefix="ctrl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<rad:RadWindowManager Behaviors="Close, Move, Resize,Maximize"
            ID="RadWindowManager"  Skin="Black" DestroyOnClose="false" runat="server">
            <Windows>
                <rad:RadWindow ID="RadWindow1" Modal="true" VisibleStatusbar="false"  VisibleTitlebar="false" 
                VisibleOnPageLoad="false" Height="526" Width="700" 
                Title="Send event information to your friends"
                     runat="server">
                </rad:RadWindow>  
                <rad:RadWindow Width="350" Modal="true" ReloadOnShow="true"
                ClientCallBackFunction="CallBackFunctionOwner"  IconUrl="./image/AddIcon.png"  VisibleTitlebar="false"
                EnableEmbeddedSkins="true" Skin="Office2007" Height="300" VisibleStatusbar="false" 
                ID="RadWindow2" Title="Owner Alert" runat="server">
                </rad:RadWindow>              
            </Windows>
        </rad:RadWindowManager>
         <rad:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="false" 
                KeepInScreenBounds="true" VisibleOnPageLoad="false"  Behaviors="none"
            ID="MessageRadWindowManager" ReloadOnShow="true" Modal="true"
            RestrictionZoneID="RestrictionZone"
            runat="server">
                <Windows>
                    <rad:RadWindow Width="600"
                    EnableEmbeddedSkins="true"  VisibleTitlebar="false"
                    VisibleStatusbar="false" Skin="Black" Height="500" ID="MessageRadWindow" 
                    Title="Alert" runat="server">
                    </rad:RadWindow>
                </Windows>
            </rad:RadWindowManager> 
<rad:RadAjaxPanel runat="server" EnableAJAX="true">


<rad:RadScriptBlock ID="RadScriptBlock1" runat="server">
<rad:RadCodeBlock ID="RadCodeBlock1" runat="server">
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

       //<![CDATA[



        function OpenRad(theID)
        {
            var win = $find("<%=RadWindow1.ClientID %>");
            win.setUrl('Communicate.aspx?ID='+theID);
            win.show(); 
            win.center(); 
               
         }
         

         function OpenEventModal(id, eventID)
        {
            var win = $find("<%=MessageRadWindow.ClientID %>");
            win.setUrl("EventModal.aspx?ID=" + id + "&eventID="+eventID);
            win.show(); 
            win.center(); 
         }
         
         function OpenOwner()
                 {
                    var win = $find("<%=RadWindow2.ClientID %>");
                    win.setUrl('Message.aspx');
                    win.show(); 
                    win.center();
                 }
                 
                 function CallBackFunctionOwner(radWindow, returnValue)
                {
                    
                    if(returnValue != null && returnValue != undefined)
                        window.location = returnValue;
                    
                    
                        
                    
                }
                
                
                                
                function  ReturnURL(theVar)
                {
                    var TA = document.getElementById('TweeterA');
                    TA.href = 'http://twitter.com/home?status=' + theVar;
                }
                
        //]]>
        
        
        function OpenMoreDates(){
            var theDiv2 = document.getElementById('MoreDatesName');
            var theDiv = document.getElementById('MoreDatesDiv');

            if(theDiv2.innerHTML == 'More Dates')
            {
                
                //div_animate();
                theDiv.style.display = 'block';
                theDiv2.innerHTML = 'Less Dates';
            }
            else
            {
                theDiv.style.display = 'none';
                theDiv2.innerHTML = 'More Dates';
            }
        }

 function div_animate()
	{
	div_height= document.getElementById('infoDiv').innerHTML;
	
	div_idnty = document.getElementById('MoreDatesDiv');
	if(div_idnty.style.height == "")
	    curr_div_h = 0;
	else
	    curr_div_h = parseInt(div_idnty.style.height.replace('px', ''));
 
 if(curr_div_h == undefined || curr_div_h == NaN)
 curr_div_h = 0;
 
	if (curr_div_h < div_height)
		{
		curr_div_h = curr_div_h + 5;
		div_idnty.style.height=curr_div_h+'px';
		setTimeout('div_animate()',0);
		}
	}
    </script>

</rad:RadCodeBlock>
</rad:RadScriptBlock>
   
    <asp:Label runat="server" ID="ErrorLabel" ForeColor="red"></asp:Label>
        <asp:Panel CssClass="FloatLeft" runat="server" ID="TopPanel" Visible="false">
        <div class="topDiv" style="margin-top: -30px; margin-left: -20px;background-image: url(image/NavBarBackground.gif); background-repeat: repeat-x;height: 25px;vertical-align: middle;">
        <div style="background:url(image/HeaderRound.gif) no-repeat bottom right;height: 25px; vertical-align: middle;">
        <asp:Panel runat="server" ID="ReturnPanel" CssClass="FloatLeft" Visible="false">
        <a style="text-decoration:none; border: 0;  color: #ffffff; font-family: Arial; font-size: 11px; font-weight: bold;" href="EventSearch.aspx?searched=true#search">
        <img style="float: left;border:0; padding: 6px;" src="image/BackArrow.png" name="Back to search results" alt="Back to search results" />
        <span style="float: left; position: relative; top: 5px;height: 25px; vertical-align: middle;"> Back to search results&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </span></a>
        </asp:Panel>
        <div style="float: left; margin-top: 5px;">
            <span style=" text-decoration: none; color: #ffffff; font-family: Arial; font-size: 11px; font-weight: bold;height: 25px; vertical-align: middle;">
            <asp:LinkButton runat="server" ForeColor="white" Font-Underline="false" 
            Visible="false" OnClick="GoToEdit" ID="EditLink" Text="|&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Edit Event" 
                 CssClass="FloatLeft"></asp:LinkButton>
            </span>
        </div>
        <asp:Panel runat="server" ID="OwnerPanel" Visible="false">
            <div style="float: left; margin-top: 5px;">
                <span style=" text-decoration: none; color: #ffffff; font-family: Arial; font-size: 11px; font-weight: bold;height: 25px; vertical-align: middle;">
                <a runat="server" OnClick="OpenOwner();" style="color: White; float: left; text-decoration: none;"
                 ID="HyperLink1">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Event Ownership is Open</a>
                </span>
            </div>
        </asp:Panel>
        <div style="float: left; margin-top: 5px;">
        <asp:Label Visible="false" ID="PassedLink" runat="server" CssClass="AddGreenLink" Text="&nbsp;&nbsp;This event has passed"></asp:Label>
        </div>
        </div>
        </div>
        <%--<div style="margin-top: -20px; z-index: 4000; float: right; width: 600px; height: 20px;">
            
                 <asp:HyperLink runat="server" CssClass="AddGreenLink FloatLeft" ID="link" 
                 Text="<< Return to search results" NavigateUrl="~/EventSearch.aspx#search"></asp:HyperLink>
            </div>--%>
            </asp:Panel>
            
        
     <div id="topDiv">
            <div style="float: left;width: 420px; padding-right: 10px;" align="left">
                <table>
                    <tr>
                        <td><asp:Panel runat="server" CssClass="FloatLeft" ID="RecomPanel"></asp:Panel></td>
                        <td><h1 class="EventHeader"><asp:Label runat="server" ID="EventName"></asp:Label></h1></td>
                    </tr>
                </table>
                
              <div>
                        <rad:RadPanelBar ID="RadPanel1" Width="420" runat="server">
                            <Items>
                                <rad:RadPanelItem Width="150px" Text="Event Updates">
                                    <Items>
                                        <rad:RadPanelItem Width="420" ></rad:RadPanelItem>
                                    </Items>
                                </rad:RadPanelItem>
                            </Items>
                        </rad:RadPanelBar>
              </div>     
                <asp:HyperLink runat="server" ID="VenueName" CssClass="VenueName"></asp:HyperLink><br />
                <asp:Label runat="server" ID="DateAndTimeLabel" CssClass="DateTimeLabel"></asp:Label><br />
                <ctrl:HippoRating ID="HippoRating1" runat="server" />
             
             <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td>
                        <asp:Panel runat="server" Height="20px" ID="SongPanel"></asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel runat="server" Height="20px" ID="CalendarSharePanel"></asp:Panel>
                    </td>
                </tr>
                <asp:Panel runat="server" ID="LoggedInPanel" Visible="false">
                    <tr>
                        <td>
                            <ctrl:SendTxt runat="server" ID="SendTxtID" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <ctrl:SendEmail ID="SendEmail1" runat="server" THE_TEXT="Send Email with this Info" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <ctrl:Flag ID="Flag1" runat="server" />
                        </td>
                    </tr>
                    
                </asp:Panel>
                <tr>
                    <td style="padding-top: 30px;">
                        <asp:Literal runat="server" ID="BuyAtTix"></asp:Literal>
                    </td>
                </tr>
                <asp:Panel ID="LoggedOutPanel" runat="server">
                    <tr>
                        <td>
                            <div class="EventDiv">
                                <label class="AddGreenLink">To edit this event, send email/text with event information as well as flag this item <a href="Register.aspx" class="AddLink">Register</a> or <a href="UserLogin.aspx" class="AddLink">Log in</a></label>
                            </div>
                        </td>
                    </tr>
                </asp:Panel>
                <asp:Panel runat="server" ID="OnlyHippoPanel" Visible="false">
                    <tr>
                        <td>
                            <div style="padding-top: 5px;">
                                <script type="text/javascript" src="http://widgets.amung.us/classic.js"></script><script type="text/javascript">WAU_classic('k3nyx4trfy6a')</script>
                            </div>
                        </td>
                    </tr>
                </asp:Panel>
             </table>
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
           <!-- AddThis Button BEGIN -->


                
                            <asp:Literal runat="server" ID="DiggLiteral"></asp:Literal>
                        
                
                <br /><br />
             <asp:Label runat="server" ID="ShowDescriptionBegining" CssClass="EventBody"></asp:Label>
                
            </div>
           <div  style="float: left; padding-left: 5px; width: 430px;">
                <div class="topDiv" style="padding-top: 20px; clear: both;">
                    <div style="font-family: Arial; font-size: 16px; color: White; clear: both;">
                    <div style="float: left;">
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td valign="middle">
                                    <asp:Label CssClass="EventGoersLabel" runat="server" ID="NumberPeopleLabel">
                                    </asp:Label> are going to this event 
                                    <asp:Label runat="server" CssClass="EventGoersLabel" ID="NumberFriendsLabel"></asp:Label>
                                </td>
                                <td valign="middle">
                                    <asp:Literal runat="server" ID="CommunicateLiteral"></asp:Literal>
                                </td>
                            </tr>
                        </table>
                    </div>
                        
                    </div>
                    
                    <div style="float: left; padding-top: 10px;">
                    <asp:Literal runat="server" ID="ShowVideoPictureLiteral">
                    </asp:Literal>
                    <asp:Panel runat="server" ID="RotatorPanel">
                        <rad:RadRotator runat="server" ScrollDuration="200" ID="Rotator1" 
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
                </div>
                
                <div style="padding-top: 28px; clear: both;">
                    <ctrl:TagCloud runat="server" TAG_TYPE="EVENT" ID="TagCloud" />
                </div>
                <div style="float: right; padding-top: 20px;">
                    <div style="clear: both;">
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
                        <%--<ctrl:Ads ID="Ads1" runat="server" />--%>
                    </div>
                    
                </div>
        </div>
        </div>
        
        <div>
            <ctrl:Comments ID="TheComments" runat="server" />
        </div>
        <asp:Literal runat="server" ID="ScriptLiteral"></asp:Literal>
</rad:RadAjaxPanel>
</asp:Content>

