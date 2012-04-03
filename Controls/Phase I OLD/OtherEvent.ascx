<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OtherEvent.ascx.cs" Inherits="Controls_OtherEvent" %>
<div style=" float: left;width: 284px; height: 80px; padding-top: 10px;">
    <div style="width: 30px; float: left;">
        <img src="image/CommentBlurb.png" /> 
    </div>
    <div style="width: 254px;">
        <asp:HyperLink runat="server" ID="TitleLabel" CssClass="OtherEventTitle"></asp:HyperLink><br />
        <asp:Label runat="server" ID="PresentedByLabel" CssClass="PresentedByTitle"></asp:Label><br />
        <div style="padding-left: 13px;">
            <asp:Label runat="server" ID="OtherEventSummaryLabel" CssClass="OtherEventSummary"></asp:Label>
        </div>
    </div>

</div>