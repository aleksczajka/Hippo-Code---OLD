<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BottomFooter.ascx.cs" Inherits="BottomFooter" %>
<%@ Register Src="~/Controls/OtherEvent.ascx" TagName="OtherEvent" TagPrefix="ctrl" %>
<div align="left" style="height: 70px; background-image: url('image/FooterBackground.png'); background-repeat: repeat-x;">
    <div style="padding-left: 20px;" id="BottomFooter">
        <a href="ContactUs.aspx">Contact Us</a> | <a href="TermsAndConditions.aspx">Terms of Use</a> | <a href="../ProhibitedAds.aspx">Prohibited Ads</a> | 
        <a href="PrivacyPolicy.aspx">Privacy Policy</a> | <a href="About.aspx">About</a> | <a href="Feedback.aspx">Feedback</a> | <a href="SiteMap.aspx">Site Map</a>
            <br /><br />
            <div id="copy">Copyright &copy; <%=DateTime.Now.Year %> HippoHappenings, LLC. All rights reserved.</div>
    </div>
</div>