<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Footer.ascx.cs" Inherits="Controls_Footer" %>



<div align="right" style="width: 890px;">
    <a target="_blank" href="http://www.facebook.com/#!/pages/HippoHappenings/141790032527996?ref=ts"><img name="thee Facebook" title="thee Facebook" style="border: 0; width: 41px;" src="NewImages/FacebookButton.png" /></a>
    &nbsp;&nbsp;
    <a target="_blank" href="http://twitter.com/HippoHappenings"><img style="border: 0; width: 41px;" name="thee Twitter" title="thee Twitter" src="NewImages/TwitterButton.png" /></a>
</div>
<div align="left" style="margin-left: 30px;width: 890px;margin-top: 200px;height: 360px; background-image: url('image/FooterBackground.png'); background-repeat: repeat-x;">
    <div style="padding-left: 20px; padding-top: 18px;">
        <div style="padding-bottom: 10px;">
            <asp:Label runat="server" ID="TitleLabel" CssClass="FooterTitle" Text="Other Events"></asp:Label>
        </div>
        <asp:Panel runat="server" ID="EventsPanel"> 
        </asp:Panel>
    </div>
</div>