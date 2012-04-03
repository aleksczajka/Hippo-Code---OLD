<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Ads.ascx.cs" Inherits="Controls_Ads" %>
<%@ Register TagName="Ad1" TagPrefix="ctrl" Src="~/Controls/Ad1.ascx" %>
<%@ Register TagName="Ad2" TagPrefix="ctrl" Src="~/Controls/Ad2.ascx" %>
<%@ Register TagName="Ad3" TagPrefix="ctrl" Src="~/Controls/Ad3.ascx" %>
<%@ Register TagName="Ad4" TagPrefix="ctrl" Src="~/Controls/Ad4.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<div style="background-image: url(images/BuilBord.gif); width: 431px; height: 640px;">
    <img src="images/LocalBoard.png" style="float: left; padding-left: 10px; padding-top: 10px; padding-bottom: 10px;" />

    <div align="center" style="width: 431px; margin-top: -30px;">
        <asp:Literal runat="server" ID="ErrorLit"></asp:Literal>
       
                <div>
                    <div style="float:left;padding-left: 8px;">
                         <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                        <ctrl:Ad1 runat="server" ID="Ad1" />
                        </ContentTemplate>
        </asp:UpdatePanel>
                    </div>
                    <div style="float: left;padding-left: 3px;">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                        <ctrl:Ad2 runat="server" ID="Ad2" />
                        </ContentTemplate>
        </asp:UpdatePanel>
                    </div>
                    <div style="float:left;padding-left: 8px;">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                        <ctrl:Ad3 runat="server" ID="Ad3" />
                         </ContentTemplate>
        </asp:UpdatePanel>
                    </div>
                    <div style="float: left;padding-left: 3px;"> 
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                        <ctrl:Ad4 runat="server" ID="Ad4" />
                        </ContentTemplate>
        </asp:UpdatePanel>
                    </div>
                </div>
            
    </div>
    <div style="padding-left: 5px; width: 419px;">
        <div align="center" style="float: left;padding: 3px; margin-left: 3px; margin-top: 8px;border: 2px solid white;background-color: #1b1b1b;clear: both; line-height: 10px;vertical-align: middle; font-family: Arial; font-size: 10px; color: #cccccc;">
            *A bulletin shows for 20sec, every 10minutes, for 24 hours to one particular viewer. <br />
            To post a bulettin go to <a href="PostAnAd.aspx" style="color: #1fb6e7;">Post An Ad</a>. 
            To specify what type of bulletins you'd like to see when logged in go to <a style="color: #1fb6e7;" href="User.aspx?p=true#adPfs">My Account</a>.
        </div>
    </div>
</div>