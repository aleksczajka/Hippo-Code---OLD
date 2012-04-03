<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" EnableViewState="false" AutoEventWireup="true" 
ValidateRequest="false" MaintainScrollPositionOnPostback="true"
CodeFile="Home.aspx.cs" Inherits="Home" smartnavigation="true"
Title="Happenings events trips adventures and locales in your city | HippoHappenings" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<%@ Register Src="~/Controls/AddToCalendar.ascx" TagName="AddTo" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/SendTxtToCell.ascx" TagName="SendTxt" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/Comments.ascx" TagName="Comments" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/TagCloud.ascx" TagName="TagCloud" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/PlayerXML/SongPlayer.ascx" TagName="Songs" TagPrefix="ctrl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register Src="~/Controls/SendEmail.ascx" TagName="SendEmail" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/HippoRating.ascx" TagName="HippoRating" TagPrefix="ctrl" %>
<%@ Register TagName="Ads" TagPrefix="ctrl" Src="~/Controls/Ads.ascx" %>
<%@ Register TagName="LocDetect" TagPrefix="ctrl" Src="~/Controls/LocDetect.ascx" %>
<%@ Register TagName="BlueButton" TagPrefix="ctrl" Src="~/Controls/BlueButton.ascx" %>
<%@ Register TagName="GreenButton" TagPrefix="ctrl" Src="~/Controls/GreenButton.ascx" %>
<%@ Register TagName="SmallButton" TagPrefix="ctrl" Src="~/Controls/SmallButton.ascx" %>

<%@ MasterType TypeName="MasterPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <%-- <rad:RadAjaxPanel runat="server" >  --%> 
 <script type="text/javascript">
    function setFormAction()
    {
        var a = document.getElementById('aspnetForm');
        a.action = 'https://www.paypal.com/cgi-bin/webscr';
    }
    function GoTo(url)
    {
        window.location = url;
    }
 </script>
 <asp:Label runat="server" ID="ErrorLabel" ForeColor="red"></asp:Label>  
            
                <ctrl:LocDetect ID="LocDetect1" runat="server" />
                
            
           
           <div class="topDiv Home1">
                <div class="FooterBottom">
                                <div class="FloatLeft"><img alt="Background Image" src="NewImages/HomeTopGreen.png" /></div>

<%--                    <div class="FloatLeft"><img alt="Background Image" src="NewImages/BackLeftTopCorner.png" /></div>
                    <div class="Home2">
                        
                    </div>
                    <div class="FloatLeft"><img alt="Background Image" src="NewImages/BackRightTopCorner.png" /></div>
--%>                </div>
                <div class="LocaleLink7"> 
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <div class="Master6">
                                
<%--                                    <h1 class="HomeH2">Post 'em. Find 'em. Share 'em. For Nada.</h1>
                                    <h2 class="HomeH2">												Promote them for pennies. </h2>
                                    
                                    
--%>                                    
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                            <script type="text/javascript">


                                                    function StartRotator()
                                                    {
                                                        var rotator = $find('<%= Rotator1.ClientID %>');
        //                                                rotator.showNext(Telerik.Web.UI.RotatorScrollDirection.Left);
                                                        var rotator1 = $find('<%= RadRotator1.ClientID %>');
                                                        var rotator2 = $find('<%= RadRotator2.ClientID %>');
                                                        
                                                        if(rotator != null && rotator1 != null && rotator2 != null)
                                                        {
                                                            var autoInt = window.setInterval(function() {
                                                                rotator.showNext(Telerik.Web.UI.RotatorScrollDirection.Left);
                                                                rotator1.showNext(Telerik.Web.UI.RotatorScrollDirection.Left);
                                                                rotator2.showNext(Telerik.Web.UI.RotatorScrollDirection.Left);
                                                            }, 10000);
                                                            
                                                            rotator.autoIntervalID = autoInt;
                                                            rotator1.autoIntervalID = autoInt;
                                                            rotator2.autoIntervalID = autoInt;
                                                        }
                                                    }
                                                    
                                                    function StopRotator()
                                                    {
                                                        var rotator = $find('<%= Rotator1.ClientID %>');
                                                        
                                                        if(rotator != null)
                                                        {
                                                            window.clearInterval(rotator.autoIntervalID);
                                                            rotator.autoIntervalID = null;
                                                        }
                                                        
                                                        rotator = $find('<%= RadRotator1.ClientID %>');
                                                        
                                                        if(rotator != null)
                                                        {
                                                            window.clearInterval(rotator.autoIntervalID);
                                                            rotator.autoIntervalID = null;
                                                        }
                                                        
                                                        rotator = $find('<%= RadRotator2.ClientID %>');
                                                        
                                                        if(rotator != null)
                                                        {
                                                            window.clearInterval(rotator.autoIntervalID);
                                                            rotator.autoIntervalID = null;
                                                        }
                                                    }
                                                    
                                                </script>
                                                <script type="text/javascript">
                                                function ShowDiv()
                                                {
                                                    var theDiv = document.getElementById('WidthDiv');
                                                    var loadDiv = document.getElementById('LoadDiv');
                                                    
                                                    //loadDiv.style.height = theDiv.offsetHeight +'px';
                                                    loadDiv.style.display = 'block';
                                                    
                                                    return true;
                                                }
                                                </script>
                                            
                                            <div class="topDiv Home3">
                                               <h1 class="Home" align="center"><asp:Label runat="server" ID="TimeFrameLabel"></asp:Label></h1>

                                        <table align="center" style="width: 820px;">
                                            <tbody><tr>
                                                <td align="center" width="250px">
                                                    <div class="MainName"><a class="MainName" href="venue-search">Regular Locale Events</a></div>
                                                </td>
                                                <td align="center" width="50px">
                                                </td>
                                                <td align="center" width="200px">
                                                    <div class="MainName"><a class="MainName" href="event-search">Events</a></div>
                                                </td>
                                                <td align="center" width="50px">
                                                </td>
                                                <td align="center">
                                                    <div class="MainName"><a class="MainName" href="trip-search">Adventures & Trips</a></div>
                                                </td>
                                            </tr>
                                            <%--<tr>
                                                <td align="center">
                                                    <div><a href="venue-search"><img class="StuffBox" src="NewImages/LocalesButton.png" alt="Find Locales"></a></div>
                                                </td>
                                                <td align="center">
                                                    <div class="MainName">+</div>
                                                </td>
                                                <td align="center">
                                                    <div><a href="event-search"><img class="StuffBox" src="NewImages/EventsButton.png" alt="Find Events"></a></div>
                                                </td>
                                                <td align="center">
                                                    <div class="MainName">=</div>
                                                </td>
                                                <td align="center">
                                                    <div><a href="trip-search"><img class="StuffBox" src="NewImages/AdventuresButton.png" alt="Find Adventures"></a></div>
                                                </td>
                                            </tr>--%>
                                        </tbody></table>
                                        <%--<div class="Home4">
                                            <ctrl:BlueButton runat="server" WIDTH="105px" CLIENT_CLICK="window.location = 'event-search'" BUTTON_TEXT="Go to an event" />
                                            <div class="Home5">
                                                <a class="NavyLinkSmall" href="blog-event">+Add Event</a>
                                            </div>
                                        </div>
                                        <div class="Home6">
                                            <ctrl:BlueButton ID="BlueButton1" runat="server" WIDTH="105px" CLIENT_CLICK="window.location = 'venue-search'" BUTTON_TEXT="Go to a Locale" />
                                            <div class="Home5">
                                                <a class="NavyLinkSmall" href="enter-locale">+Add Locale</a>
                                            </div>
                                        </div>
                                        <div class="Home7">
                                            <ctrl:BlueButton ID="BlueButton2" runat="server" WIDTH="135px" CLIENT_CLICK="window.location = 'trip-search'"  BUTTON_TEXT="Go on an Adventure" />
                                            <div class="Home5">
                                                <a class="NavyLinkSmall" href="enter-trip">+Add Adventure</a>
                                            </div>
                                        </div>--%>
                                        
                                        
                                    </div>
                                    <div class="Home10 topDiv">

                                                <div id="LoadDiv" class="Home11">
                                                    <div class="Home12"></div>
                                                    <div class="Home13">
                                                        <table height="100%" width="100%" align="center">
                                                            <tr>
                                                                <td height="100%" width="100%" align="center" valign="middle">
                                                                    <img alt="Loading Logo" src="image/ajax-loader.gif" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                                <div id="WidthDiv">
                                                <div onmouseover="StopRotator()" onmouseout="StartRotator()" class="Home14">
                                                    <div style="float: left;padding-right: 4px;">
                                                    <rad:RadRotator ID="Rotator1"  runat="server" WrapFrames="true"
                                                        Width="280px" Height="240px" 
                                                        ItemHeight="240px" ItemWidth="240px" RotatorType="buttons" 
                                                         ScrollDirection="Left,Right">
                                                    </rad:RadRotator>
                                                    </div>
                                                    <div style="float: left;padding-right: 4px;">
                                                    <rad:RadRotator ID="RadRotator1"  runat="server" WrapFrames="true"
                                                        Width="280px" Height="240px" 
                                                        ItemHeight="240px" ItemWidth="240px" RotatorType="buttons" 
                                                         ScrollDirection="Left,Right">
                                                    </rad:RadRotator>
                                                    </div>
                                                    <div style="float: left;">
                                                    <rad:RadRotator ID="RadRotator2"  runat="server" WrapFrames="true"
                                                        Width="280px" Height="240px" 
                                                        ItemHeight="240px" ItemWidth="240px" RotatorType="buttons" 
                                                         ScrollDirection="Left,Right">
                                                    </rad:RadRotator>
                                                    </div>
                                                </div>
                                                <asp:Panel runat="server" ID="CLPanel" Visible="false">
                                                    <div class="CLEvents">
                                                        <div class="MainName">Craigslist Events</div>
                                                        <div class="ClHome14">
                                                            <div style="float: left; padding-right: 40px;">
                                                            <rad:RadRotator ID="RadRotator3"  runat="server" WrapFrames="true"
                                                                Width="860px" Height="200px" Skin="Web20" 
                                                                ItemWidth="205px" ItemHeight="200px" RotatorType="buttons" 
                                                                 ScrollDirection="Left,Right">
                                                            </rad:RadRotator>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                                
                                                </div>    
                                                
                                         <div class="Home15 topDiv">
                                                    <div class="Home16">
                                                        <ctrl:SmallButton CLIENT_LINK_CLICK="ShowDiv();"  ID="TodayButton" runat="server" />
                                                    </div>
                                                    <div class="Home16">
                                                        <ctrl:SmallButton CLIENT_LINK_CLICK="ShowDiv();" ID="TomorrowButton" runat="server" />
                                                    </div>
                                                    <div class="Home16">
                                                        <ctrl:SmallButton CLIENT_LINK_CLICK="ShowDiv();" ID="ThisWeekButton" runat="server" />
                                                    </div>
                                                    <div class="Home16">
                                                        <ctrl:SmallButton CLIENT_LINK_CLICK="ShowDiv();" ID="ThisWeekendButton" runat="server" />
                                                    </div>
                                                    <div class="Home16">
                                                        <ctrl:SmallButton CLIENT_LINK_CLICK="ShowDiv();" ID="ThisMonthButton" runat="server" />
                                                    </div>
                                                    <div class="Home16">
                                                        <script type="text/javascript">
                                                            function parseCheck()
                                                            {
                                                                var theInput = document.getElementById('ctl00_ContentPlaceHolder1_NextDaysInput');
                                                                var isTr = !isNaN(parseFloat(theInput.value));
                                                                if(isTr)
                                                                {
                                                                    ShowDiv();
                                                                }
                                                                return isTr;
                                                            }
                                                        </script>
                                                        <div align="center">
                                                            
                                                                <div class="topDiv FooterBottom">
                                                                    <img alt="Events for next number of days" class="FloatLeft" src="NewImages/SmallButtonLeft.png" height="24px" />
                                                                    <div class="Home17">
                                                                        <div class="Home18" >
                                                                            <asp:ImageButton ID="ImageButton1" OnClick="SelectNextDays" OnClientClick="return parseCheck();" runat="server" ImageUrl="~/NewImages/NextText.png" />&nbsp;
                                                                        </div>
                                                                        <div class="Home19" >
                                                                            <input runat="server" id="NextDaysInput" type="text" 
                                                                                class="Text12 Home20" />&nbsp;
                                                                        </div>
                                                                        <div class="Home18" >
                                                                            <asp:ImageButton ID="ImageButton2" OnClick="SelectNextDays" OnClientClick="return parseCheck();"  runat="server" ImageUrl="~/NewImages/DaysText.png" />&nbsp;
                                                                        </div>
                                                                    </div>
                                                                    <img alt="Events for next number of days" class="FloatLeft" src="NewImages/SmallButtonRight.png" height="24px" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="FloatLeft">
                                                            <script type="text/javascript">
                                                                function parsePriceCheck()
                                                                {
                                                                    var theInput = document.getElementById('ctl00_ContentPlaceHolder1_HighestPriceInput');
                                                                    var isTr = !isNaN(parseFloat(theInput.value.replace('$', '')));
                                                                if(isTr)
                                                                {
                                                                    ShowDiv();
                                                                }
                                                                    return isTr;
                                                                }
                                                            </script>
                                                            <div align="center">
                                                                <div class="topDiv FooterBottom">
                                                                    <img alt="Events with highest price of" class="FloatLeft" src="NewImages/SmallButtonLeft.png" height="24px" />
                                                                    <div class="Home21">
                                                                        <div class="Home18" >
                                                                            <asp:ImageButton ID="ImageButton3" OnClick="SelectHighestPrice" OnClientClick="return parsePriceCheck();" 
                                                                            runat="server" ImageUrl="~/NewImages/HighestPriceText.png" />&nbsp;
                                                                        </div>
                                                                        <div class="Home19">
                                                                            <input runat="server" id="HighestPriceInput" type="text" 
                                                                                class="Text12 Home20" />&nbsp;
                                                                        </div>
                                                                    </div>
                                                                    <img alt="Events with highest price of" class="FloatLeft" src="NewImages/SmallButtonRight.png" height="24px" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                        <div class="HomeSearch topDiv FloatRight FooterBottom">
                                            <div class="topDiv">
                                                <a href="event-search" style="top: 10px;width: 600px; height: 50px;position: absolute;top: 0;left: 0;z-index: 10000;"></a>
                                                <div style="position: absolute;top: 0;left: 0;width: 600px;top: 10px;margin-top: 20px;">
                                                    <asp:Panel runat="server" DefaultButton="fakeSearchButton">
                                                        <div style="display: none;"><asp:Button runat="server" OnClick="GoToSearch" ID="fakeSearchButton" />
                                                            </div>
                                                        <div style="float: left;padding-right: 5px;padding-top: 5px;">
                                                            <label>Search More Events and In Your Zip Code: </label>
                                                        </div>
                                                        <div style="float: left;">
                                                            <rad:RadTextBox BorderColor="#09718F"  runat="server" EmptyMessage="Keywords" ID="KeywordsTextBox" Width="209px"></rad:RadTextBox>
                                                        </div>
                                                        <div style="float: left; padding-left: 4px;">
                                                            <ctrl:SmallButton ID="SearchButton"  runat="server" />
                                                        </div>
                                                    </asp:Panel>
                                                </div>
                                            </div>
                                       </div>       
                                            
                                            
                                    </div>
                                    </ContentTemplate>
                                        </asp:UpdatePanel>
                                    <%--<div class="topDiv FloatRight">
                                            <div class="PostFreeDiv">
                                                <div class="PostArrow">
                                                    <img alt="Promote your event / locale / trip for free" src="NewImages/BlackPostArrow.png" />
                                                </div>
                                                <h2>Your Event / Locale / Trip <br />                                                        goes up here automatically <br />                                                        <a href="Add">When you Post It!</a></h2><br />
                                                
                                            </div>
                                            
                                        </div>--%>
                                    <div class="TextHome Home22 topDiv">
                                        <div class="HomeLeft">
                                        <div style="float: left;">
                                            <script type="text/javascript">
                                                    function ShowSuggest()
                                                    {
                                                        var theDiv = document.getElementById('Div2');
                                                        var loadDiv = document.getElementById('Div1');
                                                        
                                                        loadDiv.style.height = theDiv.offsetHeight +'px';
                                                        loadDiv.style.width = theDiv.offsetWidth + 'px'
                                                        loadDiv.style.display = 'block';
                                                        
                                                        return true;
                                                    }
                                                    </script>
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                <div id="Div1" class="Home11Suggest">
                                                        <div class="Home12"></div>
                                                        <div class="Home13">
                                                            <table height="100%" width="100%" align="center">
                                                                <tr>
                                                                    <td height="100%" width="100%" align="center" valign="middle">
                                                                        <img alt="Loading Logo" src="image/ajax-loader.gif" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </div>
                                                    <div id="Div2" class="SuggestHome">
                                                        <label><h3>Suggest A Venue to Grab Events</h3></label>
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <rad:RadTextBox runat="server" ID="VenueSuggestTextBox" EmptyMessage="Name and Location"></rad:RadTextBox> <label>[required]</label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <rad:RadTextBox runat="server" ID="EmailTextBox" EmptyMessage="Email"></rad:RadTextBox> <label>[optional] we'll keep you updated</label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">
                                                                    <div style="width: 100px;">
                                                                        <ctrl:SmallButton ID="SmallButton1" CLIENT_LINK_CLICK="ShowSuggest()" BUTTON_TEXT="Suggest" runat="server" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label runat="server" ID="SuggestErrorLabel" ForeColor="red"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                                                                                                                                                                                                                                                    </div>                                                </ContentTemplate>
                                            </asp:UpdatePanel>                                        </div>                                        <div style="background-color: #A9F5F1;
                                            border: 1px solid;
                                            border-radius: 10px 10px 10px 10px;
                                            float: left;
                                            margin-left: 55px;
                                            margin-top: 50px;
                                            padding: 10px;                                            width: 280px;">                                            <a href="register" class="NavyLink Home23">Sign Up </a>or <a href="login" class="NavyLink Home23">Login</a> to<%--. You'll get to..<br />--%>                                            <ol>                                                <li>                                                    Post events, locales, adventures.                                                </li>                                                <li>                                                    Share, text, email, comment, rate.                                                </li>                                                <li>                                                    Communicate with people going to events.                                                </li>                                                <li>                                                    Get recommendations and notifications for your favoirte events and venues.                                                </li>                                            </ol>
                                        </div>
                                        

                                        </div>
                                          <div class="ActionButtons">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <ctrl:GreenButton CLIENT_CLICK="GoTo('add');" WIDTH="350px" ID="GreenButton1" runat="server" BUTTON_TEXT="Learn More about Events, Locales and Adventures" />
                                                    
<%--                                                        <ctrl:GreenButton CLIENT_CLICK="GoTo('feature');" WIDTH="285px" ID="BlueButton3" runat="server" BUTTON_TEXT="Feature an Event, Locale, Adventure or Bulletin" />                                         
--%>                                                    </td>
                                                </tr>
                                            </table>
                                           
                                        </div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>                       
                </div>
                <div class="FooterBottom">
                <div class="FloatLeft"><img alt="Background Image" src="NewImages/HomeBottomGreen.png" /></div>
<%--                    <div class="FloatLeft"><img alt="Background Image" src="NewImages/BackLeftBottomCorner.png" /></div>
                    <div class="Home24">
                        &nbsp;
                    </div>
                    <div class="FloatLeft"><img alt="Background Image" src="NewImages/BackRightBottomCorner.png" /></div>
--%>                </div>
           </div>

           
           <%--<div class="topDiv Home25">
                <div class="FooterBottom">
                    <div class="FloatLeft"><img alt="Background Image" src="NewImages/BackLeftTopCorner.png" /></div>
                    <div class="Home2">
                        &nbsp;
                    </div>
                    <div class="FloatLeft"><img alt="Background Image" src="NewImages/BackRightTopCorner.png" /></div>
                </div>
                <div class="FloatLeft"> 
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="15px" class="Master5"></td>
                            <td>
                                <div class="Home26">
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
                            </td>
                            <td width="15px" class="Master8"></td>
                        </tr>
                    </table>                       
                </div>
                <div class="FooterBottom">
                    <div class="FloatLeft"><img alt="Background Image" src="NewImages/BackLeftBottomCorner.png" /></div>
                    <div class="Home29">
                        &nbsp;
                    </div>
                    <div class="FloatLeft"><img alt="Background Image" src="NewImages/BackRightBottomCorner.png" /></div>
                </div>
           </div>
           
           <div class="topDiv Home25">
                <div class="FooterBottom">
                    <div class="FloatLeft"><img alt="Background Image" src="NewImages/BackLeftTopCorner.png" /></div>
                    <div class="Home2">
                        &nbsp;
                    </div>
                    <div class="FloatLeft"><img alt="Background Image" src="NewImages/BackRightTopCorner.png" /></div>
                </div>
                <div class="FloatLeft"> 
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="15px" class="Master5"></td>
                            <td>
                                <div class="Home26">
                                
                                    <h1 class="Home">Local Bulletins</h1>
                                    <div class="FloatRight">
                                        <a class="NavyLinkSmall" href="post-bulletin">+Add Bulletin</a>
                                    </div>
                                   <ctrl:Ads runat="server" />
                                   <div class="Home27" align="center">
                                        <a class="Navy11Link" href="">Learn how to earn a free featured bulletin post.</a>
                                    </div>
                                    <div class="Home28" align="center">
                                        <a class="Navy11Link" href="add">Learn more about bulletins.</a>
                                    </div>
                                </div>
                            </td>
                            <td width="15px" class="Master8"></td>
                        </tr>
                    </table>                       
                </div>
                <div class="FooterBottom">
                    <div class="FloatLeft"><img alt="Background Image" src="NewImages/BackLeftBottomCorner.png" /></div>
                    <div class="Home29">
                        &nbsp;
                    </div>
                    <div class="FloatLeft"><img alt="Background Image" src="NewImages/BackRightBottomCorner.png" /></div>
                </div>
           </div>--%>
           
                      
          <%-- <div class="topDiv Home25" align="center">
                <script type="text/javascript"><!--
                google_ad_client = "ca-pub-3961662776797219";
                /* Horizontal Image */
                google_ad_slot = "3839144172";
                google_ad_width = 728;
                google_ad_height = 90;
                //-->
                </script>
                <script type="text/javascript"
                src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
                </script>
           </div>--%>
           
           <%--<div class="topDiv Home25">
                <div class="FooterBottom">
                    <div class="FloatLeft"><img alt="Background Image" src="NewImages/BackLeftTopCorner.png" /></div>
                    <div class="Home2">
                        &nbsp;
                    </div>
                    <div class="FloatLeft"><img alt="Background Image" src="NewImages/BackRightTopCorner.png" /></div>
                </div>
                <div class="FloatLeft"> 
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="15px" class="Master5"></td>
                            <td>
                                <div class="Home26">
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
                            </td>
                            <td width="15px" class="Master8"></td>
                        </tr>
                    </table>                       
                </div>
                <div class="FooterBottom">
                    <div class="FloatLeft"><img alt="Background Image" src="NewImages/BackLeftBottomCorner.png" /></div>
                    <div class="Home29">
                        &nbsp;
                    </div>
                    <div class="FloatLeft"><img alt="Background Image" src="NewImages/BackRightBottomCorner.png" /></div>
                </div>
           </div>--%>
           
           <%--<div class="topDiv Home25">
                <div class="FooterBottom">
                    <div class="FloatLeft"><img alt="Background Image" src="NewImages/BackLeftTopCorner.png" /></div>
                    <div class="Home2">
                        &nbsp;
                    </div>
                    <div class="FloatLeft"><img alt="Background Image" src="NewImages/BackRightTopCorner.png" /></div>
                </div>
                <div class="LocaleLink7"> 
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="15px" class="Master5"></td>
                            <td>
                                <div class="Home26">
                                
                                    <h1 class="Home">This Month's Hippo Bosses*</h1>
                                    
                                    <div class="FooterBottom Text12Dark">
                                        <asp:Literal runat="server" ID="MayorsLiteral"></asp:Literal>
                                    </div>
                                    <div class="TextNormal Home28" align="center">
                                        <div class="Home31">
                                        *To find out more about the Points System and to enroll for a chance to become next month’s Hippo Boss and be featured on our home page <br/><a class="NavyLink12 Home23" href="hippo-points">check out Hippo Boss and Hippo Points</a>.
                                        </div>
                                    </div>
                                </div>
                            </td>
                            <td width="15px" class="Master8"></td>
                        </tr>
                    </table>                       
                </div>
                <div class="FooterBottom">
                    <div class="FloatLeft"><img alt="Background Image" src="NewImages/BackLeftBottomCorner.png" /></div>
                    <div class="Home29">
                        &nbsp;
                    </div>
                    <div class="FloatLeft"><img alt="Background Image" src="NewImages/BackRightBottomCorner.png" /></div>
                </div>
           </div>--%>
           
            <div class="topDiv Home33">
            <asp:Label runat="server" ID="BottomErrorLabel" ForeColor="Red"></asp:Label>
            <%--<div style="float: left; width: 440px;">
            <div class="topDiv" style="float: left;width: 440px; ">
                <div style=" padding-top: 5px;">
                <div class="EventHeader" style="font-size: 16px;">Connect With Us &nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Sponsor Our Love of The Little Guy                 
                <div id="ImageDiv" style="width: 30px;float: right; padding-right: 20px;" runat="server">
                            <asp:Image runat="server" ToolTip="Tell me more" CssClass="HelpImage" 
                            AlternateText="Tell me more" ID="QuestionMark6" 
                            ImageUrl="~/image/helpIcon.png"></asp:Image></div>
                            <rad:RadToolTip Skin="Black" ID="RadToolTip1" runat="server" ManualClose="true" TargetControlID="ImageDiv" Position="MiddleRight" RelativeTo="element" ShowEvent="OnClick">
                                <div style="width: 350px; color: #cccccc; font-size: 14px; font-family: Arial; padding-bottom: 10px;">
                                  <label>
                                    Our approach is to exclude big corporations from posting classifieds on our site. 
                                    This gives small, local businesses the upper hand and provides 
                                    only relevant content to our site participants.
                                    We also feel, that, to have unique and broader-reaching content, 
                                    we need to make posting of events, venues, groups and non-featured classifieds
                                    all free. Eliminating the posting charge allows us to have content that 
                                    would otherwise never find it's way to the net.
                                    These approaches help our mission of binding individuals with their community.
                                    <br /><br />
                                    If you like and agree with our cause, sponsor us.
                                    <br />
                                    Read more on our <a class="AddLink" href="about">About Page</a>
                                  </label>
                                </div>
                            </rad:RadToolTip></div>
                <div>
                    <div style="padding-left: 20px;padding-top: 13px;height: 69px; width: 399px; background-image: url(images/ConnectBack.png); background-repeat: no-repeat;">
                        <a target="_blank" href="http://www.facebook.com/#!/pages/HippoHappenings/141790032527996?ref=ts"><img name="thee Facebook" title="thee Facebook" style="border: 0; width: 41px;" onmouseout="this.src = 'images/ConnectFacebook.png';" onmouseover="this.src = 'images/ConnectFacebookSelected2.png';" src="images/ConnectFacebook.png" /></a>
                        <a target="_blank" href="http://twitter.com/HippoHappenings"><img style="border: 0; width: 41px;" name="thee Twitter" title="thee Twitter"  onmouseout="this.src = 'images/ConnectTwitter.png';" onmouseover="this.src = 'images/ConnectTwitterSelected2.png';"  src="images/ConnectTwitter.png" /></a>
                        <a target="_blank" href="http://hippohappenings.com/I_Love_The_Hippo_23_Group"><img name="our own group on The Hippo" title="our own group on The Hippo" style="border: 0; width: 41px;" onmouseout="this.src = 'images/ConnectHippo.png';" onmouseover="this.src = 'images/ConnectHippoSelected2.png';"  src="images/ConnectHippo.png" /></a>
                        <a target="_blank" href="http://www.youtube.com/user/hippohappenings"><img name="YouTube Channel" title="YouTube Channel" style="border: 0;" onmouseout="this.src = 'images/YouTube.png';" onmouseover="this.src = 'images/YouTubeSelected.png';"  src="images/YouTube.png" /></a>
                        <div style="float: right;padding-right: 50px;">
                            
                            <div onclick="setFormAction()" style=" margin-top: -6px;float: left;">
                                <input type="hidden" name="cmd" value="_s-xclick">
                                <input type="hidden" name="hosted_button_id" value="B5GEW7HLXFNT8">
                                <input type="image" src="http://hippohappenings.com/images/Sponsor.png" border="0" name="submit" alt="PayPal - The safer, easier way to pay online!">
                                <img alt="" border="0" src="https://www.paypal.com/en_US/i/scr/pixel.gif" width="1" height="1">                        
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            </div>

            
            </div>--%>
            
        </div>
</asp:Content>

