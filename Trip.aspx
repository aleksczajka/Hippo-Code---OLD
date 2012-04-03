<%@ Page Language="C#" ValidateRequest="false" EnableSessionState="true" 
MasterPageFile="~/SecondMasterPage.master" AutoEventWireup="true" 
CodeFile="Trip.aspx.cs" Inherits="Trip" Title="Hippo Adventure" %>
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
<%@ Register Src="~/Controls/ContactAd.ascx" TagName="ContactAd" TagPrefix="ctrl" %>
<%@ Register TagName="Ads" TagPrefix="ctrl" Src="~/Controls/Ads.ascx" %>
<%@ Register TagName="BlueButton" TagPrefix="ctrl" Src="~/Controls/BlueButton.ascx" %>
<%@ Register TagName="SmallButton" TagPrefix="ctrl" Src="~/Controls/SmallButton.ascx" %>
<%@ MasterType TypeName="SecondMasterPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<rad:RadWindowManager Behaviors="Close, Move, Resize,Maximize"
            ID="RadWindowManager"  Skin="Web20" DestroyOnClose="false" runat="server">
            <Windows>
                <rad:RadWindow ID="RadWindow1" Modal="true" VisibleStatusbar="false"  VisibleTitlebar="false" 
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
<rad:RadAjaxPanel runat="server" EnableAJAX="true">
<script type="text/javascript">
    var map = null;
    var geocoder = null;
    var directionsPanel;
    var directions;
    </script>
<asp:Literal runat="server" ID="DirectionsLiteral"></asp:Literal>
<rad:RadScriptBlock ID="RadScriptBlock1" runat="server">
<rad:RadCodeBlock ID="RadCodeBlock1" runat="server">
<script type="text/javascript">

    
    function initialize() {
      if (GBrowserIsCompatible()) 
      {
            //map.setCenter(new GLatLng(37.4419, -122.1419), 13);
            
            geocoder = new GClientGeocoder();
            PlotMapDirections();
            //resizeMap(map);
      }
    }
function getCookie(c_name)
            {
                if (document.cookie.length>0)
                  {
                  c_start=document.cookie.indexOf(c_name + "=");
                  if (c_start!=-1)
                    {
                    c_start=c_start + c_name.length+1;
                    c_end=document.cookie.indexOf(";",c_start);
                    if (c_end==-1) c_end=document.cookie.length;
                    return unescape(document.cookie.substring(c_start,c_end));
                    }
                  }
                return "";
            }
            function GoToDirections()
            {
                var ToAddress = getCookie('addressParameter').replace(/@&/g, ' ').replace(/ /g, '+');
                var theInput = document.getElementById('FromInput');
                var FromAddress = theInput.value.replace(/,/g, ' ').replace(/ /g, '+');

                window.open('http://mapof.it/'+FromAddress+'/'+ToAddress);
            }
            
            
      var gmarkers = [];
      var htmls = [];
      var i = 0;
      var points = [];
      
      
      function createMarker(address,name,html) 
      {
          geocoder.getLatLng(address.replace(/@&/g, " "), 
          
            function(point)
              {
                    points[i] = point;
                    var marker = new GMarker(point);
                    GEvent.addListener(marker, "click", function() {marker.openInfoWindowHtml(html);});
                    gmarkers[i] = marker;
                    htmls[i] = html;
                    i++;
                    map.addOverlay(marker);
                    map.setCenter(point,15);
                    marker.openInfoWindowHtml(html);
              }
          );
      }


      function myclick(i) {
        gmarkers[i].openInfoWindowHtml(htmls[i]);
      }

            
    function showAddress(address, tryCount, wholeAddress, repeatCount) {
       
       // Create our "tiny" marker icon
        var blueIcon = new GIcon(G_DEFAULT_ICON);
		
		// Set up our GMarkerOptions object
		markerOptions = { icon:blueIcon };

       var temp = wholeAddress.replace(/@&/g, " ").split(" ");
      if (geocoder) {
        geocoder.getLatLng(
          address.replace(/@&/g, " "),
          function(point) {
            if (!point) 
            {
              if(tryCount > temp.length && repeatCount == 2)
              {
                alert(address.replace(/@&/g, " ") + tryCount+" not found. Try correcting this address.");
              }
              else
              {
                  var addition = 1;
                  if(tryCount > temp.length || repeatCount == 2)
                  {
                    if(repeatCount == 1)
                    {
                        tryCount = 0;
                    }
                    
                    var nextAddress = "";
                      var i;
                      for(i=0;i<temp.length;i++)
                      {
                            if(i==temp.length-1)
                            {
                                nextAddress += temp[i] + " ";
                            }
                            else
                            {
                                if(i != temp.length - (tryCount+1) && i != temp.length - (tryCount+2))
                                {
                                    
                                    nextAddress += temp[i]+" ";
                                }
                            }
                      }
                      showAddress(nextAddress, tryCount + 1, wholeAddress, 2);
                    
                  }
                  else
                  {
                      var nextAddress = "";
                      var i;
                      for(i=0;i<temp.length;i++)
                      {
                            if(i==temp.length-1)
                            {
                                nextAddress += temp[i] + " ";
                            }
                            else
                            {
                                if(i != temp.length - (tryCount+1))
                                {
                                    nextAddress += temp[i]+" ";
                                }
                            }
                      }
                      showAddress(nextAddress, tryCount + 1, wholeAddress, 1);
                  }
              }
            } 
            else {
//              if(setCent)
//                  map.setCenter(point, 12);
              map.setCenter(point, 15);
              var marker = createMarker(address, name, html);
              map.addOverlay(marker);
            }
          }
        );
      }
    }

    </script>
<script type="text/javascript">

       //<![CDATA[

         
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
                
                
                                
//                function  ReturnURL(theVar)
//                {
//                    var TA = document.getElementById('TweeterA');
//                    TA.href = 'http://twitter.com/home?status=' + theVar;
//                }
                
        //]]>
    </script>

</rad:RadCodeBlock>
</rad:RadScriptBlock>
<div class="topDiv Text12">
    <div class="AdsWrapper">
       <div class="InnerWrapper">
        <asp:Label runat="server" ID="ErrorLabel" ForeColor="red"></asp:Label>
         <div class="topDiv" id="topDiv">
                <div class="topDiv FloatLeft" align="left">
                    <div class="topDiv Wrapper">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td><asp:Panel runat="server" CssClass="FloatLeft" ID="RecomPanel"></asp:Panel></td>
                                <td>
                                    <div class="topDiv">
                                        <div class="FloatLeft">
                                  
                                            <asp:Label runat="server" ID="EventName"></asp:Label>
                                            
                                        </div>
                                        <div class="TripWrapper">
                                            <asp:Label runat="server" CssClass="NavyLink" ID="WhatIsLabel" Text="what is an adventure?"></asp:Label>
                                            <rad:RadToolTip Skin="Sunset" ManualClose="true" Width="200px" 
                                                runat="server" Position="MiddleRight" RelativeTo="Element" ShowEvent="OnClick" TargetControlID="WhatIsLabel">
                                                <div class="Text12 Pad10">
                                                    The goal of this feature is to share any adventures that                                                     you've had and would like others to share in the experience. An adventure                                                    can be anything from a great adventure outdoors,                                                     a bunch of landmarks to visit one after the other                                                    due to their closeness or relation to one another,                                                     a museum attractions to visit in a specific order, or event just a list of
                                                    restaurants to visit in order for an appetizer, first course and second course respectively.
                                                </div>
                                            </rad:RadToolTip>
                                        </div>
                                        <div class="fbWrapper">
                                            <div id="fb-root"></div><script src="http://connect.facebook.net/en_US/all.js#appId=229624373718810&amp;xfbml=1"></script><asp:Literal runat="server" ID="fbLiteral"></asp:Literal>
                                        </div>
                                   </div>
                                </td>
                            </tr>
                        </table>
                        <asp:Panel runat="server" ID="LoggedInPanel" Visible="false">
                            <div>
                                <div class="FloatLeft">
                                    <asp:Panel runat="server" Height="20px" ID="ContactPanel"></asp:Panel>
                                </div>
                                <div class="LinksWrapper">
                                    <asp:Panel runat="server" Height="20px" ID="CalendarSharePanel"></asp:Panel>
                                </div>
                                <div class="LinksWrapper">
                                    <ctrl:Flag ID="Flag1" runat="server" />
                                </div>
                                <div class="LinksWrapper">
                                    <ctrl:SendTxt runat="server" ID="SendTxtID" />
                                </div>
                                <div class="LinksWrapper">
                                    <ctrl:SendEmail ID="SendEmail1" runat="server" THE_TEXT="Send Email" />
                                </div>
                                <div class="FloatLeft">
                                    <asp:LinkButton runat="server" Font-Underline="false" 
                                        Visible="false" OnClick="GoToEdit" ID="EditLink" Text="Edit/Feature" 
                                        CssClass="NavyLink12"></asp:LinkButton>
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="LoggedOutPanel" runat="server">
                            <div>
                                <span class="Text12">To edit this event, send email/text with event information as well as flag this item, <a href="login" class="NavyLink12UD">Log in</a>.</span>
                            </div>
                        </asp:Panel>
                        <div class="FooterBottom">
                                <span>--<span class="Green12LinkNF">We highly recommend checking the 
                                comments and suggestions for this adventure taken by others</span>--</span>
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
                            <asp:Panel runat="server" ID="PricePanel" Visible="false">
                                <b>Min:</b> $<asp:Label runat="server" ID="MinPrice"></asp:Label>, <b>Max:</b> $<asp:Label runat="server" ID="MaxPrice"></asp:Label>
                            </asp:Panel>
                            <asp:Label runat="server" ID="DurationLabel"></asp:Label><br />
                            <b>Months When To Go:</b> <asp:Label runat="server" ID="MonthsLabel"></asp:Label><br />
                            <b>Days When To Go:</b> <asp:Label runat="server" ID="DaysLabel"></asp:Label><br />
                            <b>Getting Around</b>
                            <asp:CheckBoxList CellPadding="0" CellSpacing="0" runat="server" ID="MeansCheckList" RepeatDirection="horizontal">
                                <asp:ListItem Text="Car" Value="1" Enabled="false"></asp:ListItem>
                                <asp:ListItem Text="Walking" Value="2" Enabled="false"></asp:ListItem>
                            </asp:CheckBoxList>
                            <asp:CheckBoxList CellPadding="0" CellSpacing="0"  runat="server" ID="MeansCheckList2" RepeatDirection="horizontal">
                                <asp:ListItem Text="Hiking" Value="3" Enabled="false"></asp:ListItem>
                                <asp:ListItem Text="Biking" Value="4" Enabled="false"></asp:ListItem>
                            </asp:CheckBoxList>
                            <asp:CheckBoxList CellPadding="0" CellSpacing="0"  runat="server" ID="MeansCheckList3" RepeatDirection="horizontal">
                                <asp:ListItem Text="Flying" Value="5" Enabled="false"></asp:ListItem>
                            </asp:CheckBoxList>
                            
                            <br />
                            <div class="TripOptain">
                                <h1 class="SideColumn">What You Should Hope To Obtain</h1>
                                <asp:Label runat="server" ID="ObtainLabel"></asp:Label>
                            </div>
                            <asp:Literal runat="server" ID="ShowDescriptionBegining" ></asp:Literal>
                            
                    </div>
                    <div class="topDiv FooterBottom">
                        <div class="TripBring">
                            <h1 class="SideColumn">What To Bring</h1>
                            <asp:Label runat="server" ID="BringLabel"></asp:Label>
                        </div>
                        <div class="TripDress">
                            <h1 class="SideColumn">How To Dress</h1>
                            <asp:Label runat="server" ID="DressLabel"></asp:Label>
                        </div>
                    </div>
                    <div class="topDiv TripDirections">
                        <div class="FloatLeft">
                            <h1 class="SideColumn">Directions</h1>
                            <asp:Label runat="server" ID="Directions"></asp:Label>
                        </div>
                     </div>
                     <div class="GooglyLinksTrip">
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
                    <div class="topDiv TripMap">
                        <div class="TripMapInner">
                            <h1 class="SideColumn">Google Directions</h1>
                            <asp:Literal runat="server" ID="GoogleDirectionsLiteral"></asp:Literal>
                        </div>
                        <div class="TripMapInnerB">
                            <asp:Literal runat="server" ID="MapLiteral"></asp:Literal>
                        </div>
                    </div>
                </div>
         </div>
        </div>
       <div class="OthersWrapper">
            <asp:Panel runat="server" ID="OtherEventsPanel"></asp:Panel>
            
       </div>
   </div>
    <%--<div class="AdsWrapper">
        <div class="AdsInnerWrapper">
            <ctrl:Ads ID="Ads1" runat="server" />
            <div class="AdsInnerInnerWrapper">
                <a class="NavyLinkSmall">+Add Bulletin</a>
            </div>
        </div>
    </div>--%>
    <div class="AdsWrapper">
        <div class="CommentsWrap">
            <ctrl:Comments ID="TheComments" runat="server" />
        </div>
        <div class="CommentsWrapper">
            <asp:Panel runat="server" ID="MoreEventsPanel"></asp:Panel>
            <div class="CloundsInnerWrapper">
                <ctrl:TagCloud runat="server" TAG_TYPE="TRIP" ID="TagCloud" />
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
                                        <b>Hippo Adventure Tips</b>
                                            <ol>
                                                <li>
                                                    Get to know your city, share a scavenger hunt or promote your favorite hike.
                                                </li>
                                                <li>
                                                    Earn points toward being featured on our home page by posting adventures. 
                                                </li>
                                                <li>
                                                    Post, Share, Text, Email, Discuss. 
                                                </li>
                                                <li>                                                    Feature an adventure for as little as $1/day.                                                 </li>
                                            </ol>
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
</rad:RadAjaxPanel>
</asp:Content>

