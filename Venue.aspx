<%@ Page Language="C#" MasterPageFile="~/SecondMasterPage.master" ValidateRequest="false" AutoEventWireup="true" 
CodeFile="Venue.aspx.cs" Inherits="Venue" Title="Hippo Venue" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register Src="~/Controls/TagCloud.ascx" TagName="TagCloud" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/PlayerXML/SongPlayer.ascx" TagName="Songs" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/AddToFavorites.ascx" TagName="AddTo" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/SendTxtToCell.ascx" TagName="SendTxt" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/Comments.ascx" TagName="Comments" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/SendMessage.ascx" TagName="SendMessage" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/SendEmail.ascx" TagName="SendEmail" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/HippoRating.ascx" TagName="HippoRating" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/FlagItem.ascx" TagName="Flag" TagPrefix="ctrl" %>
<%@ Register TagName="Ads" TagPrefix="ctrl" Src="~/Controls/Ads.ascx" %>
<%@ Register TagName="BlueButton" TagPrefix="ctrl" Src="~/Controls/BlueButton.ascx" %>
<%@ Register TagName="SmallButton" TagPrefix="ctrl" Src="~/Controls/SmallButton.ascx" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<style type="text/css">
#CatDiv{
        background:none;
        
        display:block;
        overflow:hidden;
        border-style:solid;
}
</style>
<script type="text/javascript">
    function OnMapClose(radWindow)
    {                  
       window.location.reload();
    } 
                
    function CallBackFunction(radWindow, returnValue)
    {                  
        if(returnValue != null && returnValue != undefined)
           window.location.reload();
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
</script>

 <rad:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="true" 
    KeepInScreenBounds="true" VisibleOnPageLoad="false" Behaviors="Close"
ID="MessageRadWindowManager" BackColor="#1b1b1b" Modal="true" DestroyOnClose="false" 
RestrictionZoneID="RestrictionZone"
runat="server">
    <Windows>
        <rad:RadWindow Width="730"  ReloadOnShow="true"
        ClientCallBackFunction="CallBackFunction"  IconUrl="./image/AddIcon.png"  VisibleTitlebar="false"
        EnableEmbeddedSkins="true" Skin="Web20" Height="550" VisibleStatusbar="false" 
        ID="MessageRadWindow" Title="Add to Favorites" runat="server">
        </rad:RadWindow>
        <rad:RadWindow Width="350"  ReloadOnShow="true"
        ClientCallBackFunction="CallBackFunctionOwner"  IconUrl="./image/AddIcon.png"  VisibleTitlebar="false"
        EnableEmbeddedSkins="true" Skin="Web20" Height="300" VisibleStatusbar="false" 
        ID="RadWindow1" Title="Owner Alert" runat="server">
        </rad:RadWindow>
     
    </Windows>
</rad:RadWindowManager>   

<div class="topDiv">
    <div class="AdsWrapper">
       <div class="InnerWrapper">
        <asp:Label runat="server" ID="ErrorLabel" ForeColor="red"></asp:Label>
         <div id="Div1">
                <div align="left">
                    <div class="Wrapper">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td><asp:Panel runat="server" CssClass="FloatLeft" ID="RecomPanel"></asp:Panel></td>
                                <td>
                                <div class="FloatLeft">
                                    <asp:Label runat="server" ID="VenueName"></asp:Label></div>
                                    <div class="RatingWrapper">
                                        <ctrl:HippoRating ID="HippoRating1" runat="server" />
                                    </div>
                                
                                
                                
                                </td>
                            </tr>
                        </table>
                        <asp:Panel runat="server" ID="LoggedInPanel" Visible="false">
                            <div class="FloatLeft">
                                <div class="LinksWrapper">
                                    <asp:Panel runat="server" Height="20px" ID="CalendarPanel"></asp:Panel>
                                </div>
                                <div class="LinksWrapper">
                                    <ctrl:Flag ID="Flag2" runat="server" />
                                </div>
                                <div class="LinksWrapper">
                                    <asp:Panel runat="server" ID="OwnerPanel" Visible="false">
                                            <a runat="server" OnClick="OpenOwner();" class="NavyLink12"
                                             ID="A1">Take Over Ownership</a>
                                    </asp:Panel>
                                </div>
                                <div class="LinksWrapper">
                                    <ctrl:SendTxt runat="server" ID="SendTxt1" />
                                </div>
                                <div class="LinksWrapper">
                                    <ctrl:SendEmail ID="SendEmail1" runat="server" THE_TEXT="Send Email with this Info" />
                                </div>
                                <div class="FloatLeft">
                                    <asp:LinkButton runat="server" Font-Underline="false" 
                                        Visible="false" OnClick="GoToEdit" ID="EditLink" Text="Edit/Feature" 
                                        CssClass="NavyLink12"></asp:LinkButton>
                                        <a runat="server" class="NavyLink12"
                                             ID="ContactOwnerLink">Contact Post Owner</a>
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="LoggedOutPanel" runat="server">
                            <div>
                                <span class="Text12">To edit this venue, send email/text with venue information as well as flag this item, <a href="UserLogin.aspx" class="NavyLink12UD">Log in</a>.</span>
                            </div>
                        </asp:Panel>
                        <div class="LocaleLink">
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
                      <div class="Text12 aboutLink">
                            <div class="RotatorWrapper">
                                    <asp:Panel runat="server" ID="RotatorPanel">
                                        <rad:RadRotator runat="server" ScrollDuration="200" ID="Rotator1" 
                                        ItemHeight="250" ItemWidth="412" Skin="Vista"  
                                        Width="440px" Height="250px" RotatorType="Buttons">
                                        </rad:RadRotator>
                                    </asp:Panel>
                                    <div class="WhiteHeader">
                                        <div class="LocaleLink2"><h1 class="Calendar">Today at a Glance</h1></div> 
                                        <div class="LocaleLink3">
                                            <asp:HyperLink CssClass="CalendarLink" runat="server" 
                                                ID="CalendarLink">view full calendar</asp:HyperLink>
                                        </div>
                                    </div>
                                    <div>
                                        <div class="topDiv LocaleLink4">
                                            <div  class="topDiv aboutLink">
                                                <div class="LocaleLink5"><img src="NewImages/CalendarTopLeft.png" /></div>
                                                <div class="LocaleLink6">
                                                    &nbsp;
                                                </div>
                                                <div class="LocaleLink5"><img src="NewImages/CalendarTopRight.png" /></div>
                                            </div>
                                            <div class="LocaleLink7"> 
                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td width="5px" class="LocaleLink8"></td>
                                                        <td>
                                                            <div class="Text12 LocaleLink9">
                                                                <div class="ContentFooter FriendPic">
                                                                     <asp:Panel runat="server" ID="EventsPanel"></asp:Panel>
                                                                </div>
                                                            </div>
                                                        </td>
                                                        <td width="5px" class="LocaleLink10"></td>
                                                    </tr>
                                                </table>                       
                                            </div>
                                            <div class="aboutLink" class="topDiv">
                                                <div class="LocaleLink5"><img src="NewImages/CalendarBottomLeft.png" /></div>
                                                <div class="LocaleLink11">
                                                    &nbsp;
                                                </div>
                                                <div class="LocaleLink5"><img src="NewImages/CalendarBottomRight.png" /></div>
                                            </div>
                                       </div>
                                    </div>
                            </div>
                            <asp:Label runat="server" ID="AddressLabel"></asp:Label><br />
                            <asp:Label runat="server" ID="CityState"></asp:Label>
                            <label class="LocaleLink12">(<a class="NavyLink12UD LocaleLink13" onclick="OpenRad_Map();">map it!</a>)</label>
                            <br />
                            <asp:Label runat="server" ID="PhoneLabel"></asp:Label>
                            
                            
                            
                            <br /><br />
                            <div><asp:Label runat="server" ID="HoursLabel"></asp:Label></div>
                            <asp:Panel runat="server" Height="20px" ID="SongPanel"></asp:Panel>
                      
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
            <asp:Panel runat="server" ID="MoreVenuesPanel"></asp:Panel>
            <div class="CloundsInnerWrapper">
                <ctrl:TagCloud runat="server" TAG_TYPE="VENUE" ID="TagCloud" />
            </div>
        </div>
    </div>
    
    <asp:Literal runat="server" ID="ScriptLiteral"></asp:Literal>
    
 </div>
 <asp:Literal runat="server" ID="topTopLiteral"></asp:Literal>
<%-- <div class="topDiv AdsFooterWrapper">
                <asp:Literal runat="server" ID="topTopLiteral"></asp:Literal>
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
                                <div class="Text12" class="AdsFooterInnerE">
                                    <div class="ContentFooter">
                                        <b>Hippo Locale Tips</b>                                            <ol>                                                <li>                                                    Earn points toward being featured on our home page by posting more locales.                                                 </li>                                                <li>                                                    Post, Share, Add to favorites, text, email, discuss.                                                 </li>                                                <li>                                                    Receive notifications of new events posted in your favorite locale.                                                </li>                                                <li>                                                    Edit locales you own or submit changes to locales owned by others.                                                </li>                                                <li>                                                    Feature a locale for as little as $2/day.                                                 </li>                                            </ol>
                                    </div>
                                </div>
                            </td>
                            <td width="6px" class="AdsFooterInnerF"></td>
                        </tr>
                    </table>                       
                </div>
                <div class="topDiv FooterBottom">
                    <div class="AdsFooterInnerB"><img src="NewImages/EventFooterBottomLeft.png" /></div>
                    <div class="FooterBottomInner">
                        &nbsp;
                    </div>
                    <div class="AdsFooterInnerB"><img src="NewImages/EventFooterBottomRight.png" /></div>
                </div>
           </div>--%>



</asp:Content>

