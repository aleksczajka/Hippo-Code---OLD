<%@ Page Language="C#" ValidateRequest="false" EnableSessionState="true" 
MasterPageFile="~/SecondMasterPage.master" AutoEventWireup="true" 
CodeFile="Event.aspx.cs" Inherits="Event" Title="Hippo Event" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register Src="~/Controls/TagCloud.ascx" TagName="TagCloud" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/PlayerXML/SongPlayer.ascx" TagName="Songs" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/AddToCalendar.ascx" TagName="AddTo" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/SendTxtToCell.ascx" TagName="SendTxt" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/Comments.ascx" TagName="Comments" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/SendMessage.ascx" TagName="SendMessage" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/SendEmail.ascx" TagName="SendEmail" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/HippoRating.ascx" TagName="HippoRating" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/FlagItem.ascx" TagName="Flag" TagPrefix="ctrl" %>
<%@ Register TagName="Ads" TagPrefix="ctrl" Src="~/Controls/Ads.ascx" %>
<%@ Register TagName="BlueButton" TagPrefix="ctrl" Src="~/Controls/BlueButton.ascx" %>
<%@ Register TagName="SmallButton" TagPrefix="ctrl" Src="~/Controls/SmallButton.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<rad:RadWindowManager Behaviors="Close, Move, Resize,Maximize"
            ID="RadWindowManager"  Skin="Web20" DestroyOnClose="false" runat="server">
            <Windows>
                <rad:RadWindow ID="RadWindow1" ClientCallBackFunction="clientCommCallback" Modal="true" VisibleStatusbar="false"  VisibleTitlebar="false" 
                VisibleOnPageLoad="false" Height="526" Skin="Web20" Width="700" 
                Title="Send event information to your friends"
                     runat="server">
                </rad:RadWindow>  
                <rad:RadWindow Width="350" Modal="true" ReloadOnShow="true"
                ClientCallBackFunction="CallBackFunctionOwner"  IconUrl="./image/AddIcon.png"  VisibleTitlebar="false"
                EnableEmbeddedSkins="true" Skin="Web20" Height="300" VisibleStatusbar="false" 
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

<rad:RadScriptBlock ID="RadScriptBlock1" runat="server">
<rad:RadCodeBlock ID="RadCodeBlock1" runat="server">
<script type="text/javascript">
function clientCommCallback(somtin, thisOne)
         {
            if(thisOne != null && thisOne != undefined)
            {
                window.location = thisOne;
            }
         }
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

                
        //]]>
        
        
        function OpenMoreDates(){
            var theDiv2 = document.getElementById('MoreDatesName');
            var theDiv = document.getElementById('MoreDatesDiv');

            if(theDiv2.innerHTML == 'More Dates')
            {
                theDiv.className = 'MoreMore';
                theDiv2.innerHTML = 'Less Dates';
            }
            else
            {
                theDiv.className = 'MoreNo';
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
<div class="topDiv">
    <div class="AdWrapper">
       <div class="InnerWrapper">
        <asp:Label runat="server" ID="ErrorLabel" ForeColor="red"></asp:Label>
         <div id="topDiv">
                <div class="FloatLeft" align="left">
                    <div class="Wrapper">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td><asp:Panel runat="server" CssClass="FloatLeft" ID="RecomPanel"></asp:Panel></td>
                                <td><div class="FloatLeft">
                                    <asp:Label runat="server" ID="EventName"></asp:Label></div>
                                    <asp:Panel runat="server" ID="RatingPanel">
                                        <div class="RatingWrapper">
                                            <ctrl:HippoRating ID="HippoRating1" runat="server" />
                                        </div>
                                    </asp:Panel>
                                    </td>
                            </tr>
                        </table>
                        <asp:Panel runat="server" ID="LoggedInPanel" Visible="false">
                            <div class="topDiv">
                                <div class="LinksWrapper">
                                    <asp:Panel runat="server" Height="20px" ID="CalendarSharePanel"></asp:Panel>
                                </div>
                                <div class="LinksWrapper">
                                    <ctrl:Flag ID="Flag1" runat="server" />
                                </div>
                                
                                <div class="LinksWrapper">
                                    <asp:Panel runat="server" ID="OwnerPanel" Visible="false">
                                            <a runat="server" OnClick="OpenOwner();" class="Green12LinkNFUD"
                                             ID="HyperLink1">Take Over Ownership</a>
                                    </asp:Panel>
                                </div>
                                <div class="LinksWrapper">
                                    <ctrl:SendTxt runat="server" ID="SendTxtID" />
                                </div>
                                <div class="LinksWrapper">
                                    <ctrl:SendEmail ID="SendEmail1" runat="server" THE_TEXT="Send Email with this Info" />
                                </div>
                                <div class="FloatLeft">
                                    
                                    <asp:LinkButton runat="server" Font-Underline="false" 
                                        Visible="false" OnClick="GoToEdit" ID="EditLink" Text="Edit/Feature Event" 
                                        CssClass="Green12LinkNFUD"></asp:LinkButton>
                                        
                                    <a runat="server" class="Green12LinkNFUD"
                                             ID="ContactOwnerLink">Contact Post Owner</a>
                                </div>
                                <div class="FloatLeft">
                                    <asp:Literal runat="server" ID="CalendarLiteral"></asp:Literal>
                                </div>
                                
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="LoggedOutPanel" runat="server">
                            <div class="topDiv">
                                <span class="Text12">To edit this event, send email/text with event information as well as flag this item, <a href="UserLogin.aspx" class="NavyLink12UD">Log in</a>.</span>
                            </div>
                        </asp:Panel>
                        <div class="LinksWrapper">
                            <asp:Label Visible="false" ID="PassedLink" runat="server" CssClass="Green12LinkNF" Text="This event has passed"></asp:Label>
                        </div>
                        <div class="GooglyContent">
                            <script type="text/javascript"><!--
                            google_ad_client = "ca-pub-3961662776797219";
                            /* Content Page Link Unit */
                            google_ad_slot = "9235555921";
                            google_ad_width = 468;
                            google_ad_height = 15;
                            //-->
                            </script>
                            <script type="text/javascript"
                            src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
                            </script>
                        </div>
                    </div>
                    <div class="Text12">
                            
                            <div class="RotatorWrapper">
                                    
                                    <asp:Panel runat="server" ID="RotatorPanel">
                                        <rad:RadRotator runat="server" ScrollDuration="200" ID="Rotator1" 
                                        ItemHeight="250" ItemWidth="412" Skin="Vista"  
                                        Width="440px" Height="250px" RotatorType="Buttons">
                                        </rad:RadRotator>
                                    </asp:Panel>
                                    <div align="right" class="CommunicateWrapper">
                                        
                                        <div class="CommunicateInnerWrapper">
                                            <asp:Panel runat="server" ID="CommunicatePanel"></asp:Panel>
                                        </div>
                                        <asp:Panel runat="server" ID="GoingPanel">There are <asp:Label CssClass="NavyLink" runat="server" ID="NumberPeopleLabel">
                                        </asp:Label> hippoers going to this event. <br /></asp:Panel>
                                        <asp:Label runat="server" CssClass="NavyLink" ID="NumberFriendsLabel"></asp:Label>
                                        <div>
                                            <asp:Panel runat="server" ID="ClGoingPanel" Visible="false">
                                                <asp:Panel runat="server" ID="AreUGoingPanel">
                                                    <asp:LinkButton OnClick="GoingClick" runat="server" ID="ClImGoingButton" 
                                                    CssClass="Green14LinkNFUDB" Text="+I'm Going"></asp:LinkButton>
                                                </asp:Panel>
                                                <asp:Panel runat="server" ID="ImGoingPanel">
                                                    <span class="Green14LinkNF">You're Going!</span>
                                                </asp:Panel>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                    <div class="ClInfo">
                                        
                                        <asp:Literal runat="server" ID="ClEmailLiteral"></asp:Literal>
                                    </div>
                                    
                            </div>
                            
                            <asp:HyperLink runat="server" ID="VenueName" CssClass="Green12LinkNFUD"></asp:HyperLink>
                            <div><asp:Label runat="server" ID="DateAndTimeLabel" CssClass="DateTimeLabel"></asp:Label>
                            </div>
                            <asp:Panel runat="server" ID="PricePanel" Visible="false">
                                Min: $<asp:Label runat="server" ID="MinPrice"></asp:Label>, Max: $<asp:Label runat="server" ID="MaxPrice"></asp:Label>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="SongPanel"></asp:Panel>
                            
                            
                            <asp:Literal runat="server" ID="ShowDescriptionBegining" ></asp:Literal>
                           <table>
                                <tr>
                                    <td class="AddThisWrapper">
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
                            
                    </div>
                    
                </div>
         </div>
        </div>
       <div class="OthersWrapper">
            <asp:Panel runat="server" ID="OtherEventsPanel"></asp:Panel>
            
       </div>
   </div>
   <div class="GooglyLinks2">
            <script type="text/javascript"><!--
                google_ad_client = "ca-pub-3961662776797219";
                /* Hippo Link Unit */
                google_ad_slot = "6130116935";
                google_ad_width = 728;
                google_ad_height = 15;
                //-->
                </script>
                <script type="text/javascript"
                src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
                </script>
        </div>
   <%-- <div class="AdsWrapper">
        <div class="AdsInnerWrapper">
            <ctrl:Ads ID="Ads1" runat="server" />
            <div class="AdsInnerInnerWrapper">
                <a class="NavyLinkSmall" href="PostAnAd.aspx">+Add Bulletin</a>
            </div>
        </div>
    </div>--%>
    <div class="AdsWrapper">
        <div class="InnerWrapper">
            <ctrl:Comments ID="TheComments" runat="server" />
        </div>
        <div class="CommentsWrapper">
            <asp:Panel runat="server" ID="MoreEventsPanel"></asp:Panel>
            <div class="CloundsInnerWrapper">
                <ctrl:TagCloud runat="server" TAG_TYPE="EVENT" ID="TagCloud" />
            </div>
        </div>
    </div>
    
    <asp:Literal runat="server" ID="ScriptLiteral"></asp:Literal>
 </div>
 <asp:Literal runat="server" ID="topTopLiteral"></asp:Literal>
 <%--<div class="topDiv AdsFooterWrapper" >
                
                <div class="topDiv AdsFooterInner">
                    <div class="AdsFooterInnerB"><img src="NewImages/EventFooterTopLeft.png" /></div>
                    <div class="AdsFooterInnerA">
                        &nbsp;
                    </div>
                    <div class="AdsFooterInnerB"><img src="NewImages/EventFooterTopRight.png" /></div>
                </div>
                <div class="AdsFooterInnerC"> 
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td width="6px" class="AdsFooterInnerD"></td>
                            <td>
                                <div class="Text12 AdsFooterInnerE">
                                    <div class="ContentFooter">
                                        <b>Hippo Event Tips</b>                                            <ol>                                                <li>                                                    Earn points toward being featured on our home page by posting events.                                                 </li>                                                <li>                                                    Post, Share, Add to calendar, Text, Email, Discuss.                                                 </li>                                                <li>                                                    Receive Event Recommendations based on your preferences.                                                </li>                                                <li>                                                    Receive notifications of new events posted by friends, in favorite venue, or in favorite category.                                                </li>                                                <li>                                                    Receive text/email alerts for events in your calendar.                                                 </li>                                                <li>                                                    Feature an Event for as little as $1/day.                                                 </li>                                            </ol>
                                    </div>
                                </div>
                            </td>
                            <td width="6px" class="AdsFooterInnerF"></td>
                        </tr>
                    </table>                       
                </div>
                <div class="topDiv aboutLink">
                    <div class="AdsFooterInnerB"><img src="NewImages/EventFooterBottomLeft.png" /></div>
                    <div class="FooterBottomInner">
                        &nbsp;
                    </div>
                    <div class="AdsFooterInnerB"><img src="NewImages/EventFooterBottomRight.png" /></div>
                </div>
           </div>--%>
</asp:Content>

