<%@ Page Language="C#" MasterPageFile="~/SecondMasterPage.master" AutoEventWireup="true" 
CodeFile="HippoError.aspx.cs" Inherits="HippoError" Title="HippoHappenings is Sad" %>
<%@ Register Src="~/Controls/Ads.ascx" TagName="Ads" TagPrefix="ctrl" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="topDiv">
        <div class="Text12" style="float: left; padding-bottom: 20px;">
            <div style="float: left;"><img src="image/200x200Hippo.png" /></div>
            <div style="float: left;padding-left: 50px;padding-top: 70px;" class="EventBody">
                We dreadfully need to inform you that you have encountered an error while surfing though our site.
                We apologize profusely. Please let us know what has happened by <asp:LinkButton CssClass="AddLink" runat="server" Text="writing to us." OnClick="GoTo"></asp:LinkButton>
            </div>
        </div>
        <ctrl:Ads ID="Ads1" runat="server" />
        <div style="float: right;padding-bottom: 100px;">
            <a class="NavyLinkSmall" href="PostAnAd.aspx">+Add Bulletin</a>
        </div>
    </div>
</asp:Content>

