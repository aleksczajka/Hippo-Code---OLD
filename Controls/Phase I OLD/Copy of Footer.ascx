<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Copy of Footer.ascx.cs" Inherits="Controls_Footer" %>
<%@ Register Src="~/Controls/OtherEvent.ascx" TagName="OtherEvent" TagPrefix="ctrl" %>
<div align="left" style="margin-top: 200px;height: 360px; background-image: url('image/FooterBackground.png'); background-repeat: repeat-x;">
    <div style="padding-left: 20px; padding-top: 18px;">
        <div style="padding-bottom: 10px;">
            <asp:Label runat="server" ID="TitleLabel" CssClass="FooterTitle" Text="Other Events"></asp:Label>
        </div>
        <asp:Panel runat="server" ID="EventsPanel"> 
        </asp:Panel>
    </div>
</div>