<%@ Page Language="C#" MasterPageFile="~/SecondMasterPage.master" AutoEventWireup="true" CodeFile="Ad.aspx.cs" Inherits="Ad" Title="View Ad" %>
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
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<rad:RadAjaxPanel runat="server">
<div class="topDiv">
    <div class="AdWrapper">
       <div class="InnerWrapper">
        <asp:Label runat="server" ID="ErrorLabel" ForeColor="red"></asp:Label>
         <div id="topDiv">
                <div class="InnerWrapper" align="left">
                    <div class="Wrapper">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td><asp:Label runat="server" ID="ShowHeaderName"></asp:Label></td>
                            </tr>
                        </table>
                        <asp:Panel runat="server" ID="LoggedInPanel" Visible="false">
                            <div>
                                <div class="LinksWrapper">
                                    <asp:Panel runat="server" Height="20px" ID="ContactPanel"></asp:Panel>
                                </div>
                                <div class="LinksWrapper">
                                    <ctrl:Flag ID="Flag2" runat="server" />
                                </div>
                                <div class="LinksWrapper">
                                    <ctrl:SendTxt runat="server" ID="SendTxtID" />
                                </div>
                                <div class="LinksWrapper">
                                    <ctrl:SendEmail ID="SendEmail1" runat="server" THE_TEXT="Send Email with this Info" />
                                </div>
                                <div class="FloatLeft">
                                    <asp:LinkButton runat="server" Font-Underline="false" 
                                        Visible="false" OnClick="GoToAd" ID="EditAdLink" Text="Edit Bulletin" 
                                        CssClass="NavyLink12"></asp:LinkButton>
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="LoggedOutPanel" runat="server">
                            <div>
                                <span class="Text12">To send email/text with bulletin 
                                            information, contact this user, etc., <a href="login" class="NavyLink12UD">Log in</a>.</span>
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
                                
                                   
                            </div>
                            <asp:Panel runat="server" Height="20px" ID="SongPanel"></asp:Panel>

                            <asp:Literal runat="server" ID="ShowDescription" ></asp:Literal>
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
            <asp:Panel runat="server" ID="OtherAdsPanel"></asp:Panel>
            
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
                <a class="NavyLinkSmall" href="post-bulletin">+Add Bulletin</a>
            </div>
        </div>
    </div>--%>
    <div class="AdsWrapper">
        <div class="CloudsWrapper">
            <asp:Panel runat="server" ID="MoreEventsPanel"></asp:Panel>
            <div class="CloudsInnerWrapper">
                <ctrl:TagCloud runat="server" TAG_TYPE="AD" ID="TagCloud" />
            </div>
        </div>
    </div>
    
    <asp:Literal runat="server" ID="ScriptLiteral"></asp:Literal>
    
 </div>
 <asp:Literal runat="server" ID="topTopLiteral"></asp:Literal>
<%--<div class="topDiv AdsFooterWrapper">
                
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
                            <td class="AdsFooterInnerD" width="6px"></td>
                            <td>
                                <div class="Text12" class="AdsFooterInnerE">
                                    <div class="ContentFooter">
                                        <b>Hippo Bulletin Tips</b>                                            <ol>                                                <li>                                                    Earn points toward being featured on our home page by posting bulletins whether featured or non-featured.                                                 </li>                                                <li>                                                    Post, Share, Text, Email.                                                 </li>                                                <li>                                                    Save bulletin searches and have them emailed to you when new bulletins arrive matching your criteria.                                                </li>                                                <li>                                                    Post a featured ad for as low as $3/day.                                                </li>                                            </ol>
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

     <%--<script type="text/javascript">
            ReturnURL();
        </script>--%>
</rad:RadAjaxPanel>
</asp:Content>

