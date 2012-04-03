<%@ Page Language="C#" MasterPageFile="~/SecondMasterPage.master" AutoEventWireup="true" 
CodeFile="EnterTripIntro.aspx.cs" Inherits="EnterTripIntro" Title="Enter Trip | HippoHappenings" %>
<%@ Register TagName="HippoTextBox" TagPrefix="ctrl" Src="~/Controls/HippoTextBox.ascx" %>
<%@ Register TagName="Ads" TagPrefix="ctrl" Src="~/Controls/Ads.ascx" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <div style="width: 820px; clear: both;">
 <div style="width: 430px; float: left;">
    <asp:Label runat="server" ID="EventTitle"></asp:Label>
         <span class="Text12">
            <asp:Label runat="server" ID="WelcomeLabel"></asp:Label>
          </span> <br />
   </div>
 </div>  
    <div style="width: 820px;clear: both;">
        <div style="padding-top: 40px;float: left;">
            <ctrl:Ads ID="Ads1" runat="server" />
            <div style="float: right;padding-right: 10px;">
                <a class="NavyLinkSmall" href="post-bulletin">+Add Bulletin</a>
            </div>
        </div>
    </div>
  <div class="topDiv" style="width: 847px;clear: both;margin-left: -18px;position: relative;top: 207px;">
                <div style="clear: both;" class="topDiv">
                    <div style="float: left;width: 6px;"><img src="NewImages/EventFooterTopLeft.png" /></div>
                    <div style=" height: 6px;width: 835px;float: left; background: url('NewImages/EventFooterTop.png'); background-repeat: repeat-x; background-position: top;">
                        &nbsp;
                    </div>
                    <div style="float: left;width: 6px;"><img src="NewImages/EventFooterTopRight.png" /></div>
                </div>
                <div style="clear: both; background-color: #ebe7e7;"> 
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td width="6px" style="background-image: url('NewImages/EventFooterLeft.png'); background-repeat: repeat-y;"></td>
                            <td>
                                <div class="Text12" style="width: 835px; background-color: #ebe7e7;">
                                    <div class="ContentFooter">
                                        <b>Hippo Adventures have the Following Features:</b>
                                            <ol>
                                                <li>
                                                    Get to know your city, share a scavenger hunt or promote your favorite hike.
                                                </li>
                                                <li>
                                                    Earn points toward being featured on our home page by posting trips. 
                                                </li>
                                                <li>
                                                    Post, Share, Text, Email, Discuss. 
                                                </li>
                                            </ol>
                                    </div>
                                </div>
                            </td>
                            <td width="6px" style="background-image: url('NewImages/EventFooterRight.png'); background-repeat: repeat-y;"></td>
                        </tr>
                    </table>                       
                </div>
                <div style="clear: both;" class="topDiv">
                    <div style="float: left;width: 6px;"><img src="NewImages/EventFooterBottomLeft.png" /></div>
                    <div style="height: 6px;width: 835px;float: left; background: url('NewImages/EventFooterBottom.png'); background-repeat: repeat-x; background-position: top;">
                        &nbsp;
                    </div>
                    <div style="float: left;width: 6px;"><img src="NewImages/EventFooterBottomRight.png" /></div>
                </div>
           </div>

</asp:Content>

