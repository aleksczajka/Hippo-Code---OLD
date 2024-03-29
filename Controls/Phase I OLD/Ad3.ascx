<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Ad3.ascx.cs" Inherits="Ad3" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>

<asp:Literal runat="server" ID="userLit"></asp:Literal>

<asp:Timer ID="Timer1" OnTick="Timer1_Tick" runat="server" Interval="20000">
</asp:Timer>

<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
        
    </Triggers>
    <ContentTemplate>
        <asp:Panel runat="server" ID="Template1Panel">
            <div style="float: left;width: 207px; background: transparent;"> 
            <div style="float: left;width: 207px; height: 262px; background-color: Transparent; background: url('image/AdBackground.png'); background-repeat: repeat-x;"> 
               <div  style="padding-top: 10px; clear: both; padding-left: 10px;">
                <asp:Panel runat="server" ID="ImagePanel"><div style="float: left;"><table width="100px" height="100px"
                    cellpadding="0" cellspacing="0"><tbody align="center"><tr><td valign="middle">
                <asp:Image ID="CustomerImage" CssClass="AdImage" Width="100" Height="100" runat="server" />
                </td></tr></tbody></table></div>
                </asp:Panel>
                    <div>
                       <asp:HyperLink CssClass="AdTitle"
                       runat="server"  ID="TitleLabel"></asp:HyperLink>
                    </div>
                </div>
                <div align="left" style="clear: both; padding-left: 12px; padding-right: 8px;padding-top: 1px;">
                    <asp:Label runat="server" CssClass="AdBody" ID="BodyLabel" ></asp:Label>
                </div>
                <div style="float: right; padding-right: 8px; height: 50px; width: 80px; padding-top: 2px;padding-bottom: 2px;">
                    <div style="float: left;padding-top: 5px; clear: none;"><img src="image/ReadMoreArrow.png" /></div>
                    <div style="float: right;clear: none;"><asp:HyperLink ID="ReadMoreLink" 
                    CssClass="ReadMoreLink" runat="server" Text="Read More" ></asp:HyperLink>
                    </div>
                </div>
                </div>
             </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="Template2Panel">
            <div style="float: left;width: 207px; height: 262px; background: transparent;"> 
            <div style="float: left;width: 207px; height: 262px; background-color: Transparent; background: url('image/AdBackground.png'); background-repeat: repeat-x;"> 
               <div  style="padding-top: 10px; clear: both;">
                <asp:Panel runat="server" ID="Panel2"><div align="center" style="float: left;">
                <table width="207px" height="140px"
                    cellpadding="0" cellspacing="0"><tbody align="center"><tr><td valign="middle">
                <asp:Image ID="Image1" Width="200" CssClass="AdImage" Height="140" runat="server" />
                </td></tr></tbody></table></div>
                </asp:Panel>
                    
                </div>
                <div style="padding-top: 5px; clear: both; padding-left: 6px;">
                       <asp:HyperLink CssClass="AdTitle"
                       runat="server"  ID="HyperLink1"></asp:HyperLink>
                    </div>
                <div align="left" style="clear: both; padding-left: 6px; padding-right: 8px;">
                    <asp:Label runat="server" CssClass="AdBody" ID="Label1" ></asp:Label>
                </div>
                <div style="float: right; padding-right: 8px; height: 50px; width: 80px; padding-top: 2px;padding-bottom: 2px;">
                    <div style="float: left;padding-top: 5px; clear: none;"><img src="image/ReadMoreArrow.png" /></div>
                    <div style="float: right;clear: none;"><asp:HyperLink ID="HyperLink2" 
                    CssClass="ReadMoreLink" runat="server" Text="Read More" ></asp:HyperLink>
                    </div>
                </div>
                </div>
             </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="Template3Panel">
            <div style="float: left;width: 207px; height: 262px; background: transparent;"> 
            <div style="float: left;width: 207px; height: 262px; background-color: Transparent; background: url('image/AdBackground.png'); background-repeat: repeat-x;"> 
               <div  style="padding-top: 10px; clear: both;">
                <asp:Panel runat="server" ID="Panel3"><div align="center" style="float: left;">
                <table width="207px" height="250px"
                    cellpadding="0" cellspacing="0"><tbody align="center"><tr><td valign="middle">
                <asp:Image ID="Image2" Width="200" CssClass="AdImage" Height="250" runat="server" />
                </td></tr></tbody></table></div>
                </asp:Panel>
                </div>
             </div>
             </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>


        
