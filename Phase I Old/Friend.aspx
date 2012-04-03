<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Friend.aspx.cs" Inherits="Friend" Title="Friend Page" %>
<%@ Register Src="~/Controls/Comments.ascx" TagName="Comments" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/SearchElements.ascx" TagName="SearchElements" TagPrefix="ctrl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<telerik:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="false" 
    KeepInScreenBounds="true" VisibleOnPageLoad="false" Behaviors="none" VisibleStatusbar="true"
ID="MessageRadWindowManager" BackColor="#1b1b1b" Modal="true" DestroyOnClose="false" 
RestrictionZoneID="RestrictionZone"
runat="server">
    <Windows>
        <telerik:RadWindow Width="400" EnableEmbeddedSkins="true" IconUrl="image/ContactIcon.png" 
        Skin="Black" Height="450" VisibleStatusbar="false" VisibleTitlebar="false" ID="MessageRadWindow" Title="Send Message" 
        runat="server">
        </telerik:RadWindow>
    </Windows>
</telerik:RadWindowManager>
    <script type="text/javascript">
            function OpenRadBefore_SendMessage()
            {
                var idDiv = document.getElementById('idDiv');
                var win = $find("<%=MessageRadWindow.ClientID %>");
                var str = 'Controls/MessageAlert.aspx?T=Message&a=z&ID='+ idDiv.innerHTML;
                win.setUrl(str);
                win.show(); 
                win.center(); 
                
            }
            function OpenRad_SendMessage(response)
            {
                
                   
            }
    </script>
    <div id="topDiv" style="padding-bottom: 200px;">
       <asp:Label runat="server" CssClass="EventHeader" ID="UserNameLabel"></asp:Label>
        <div id="topDiv2">
            <div style="float: left; padding-top: 5px;">
                 
                <div style=" min-width: 365px; height: 171px;background-image: url('image/FriendBackground.png'); background-repeat: no-repeat;">
                    <div align="center" style="width: 152px; height: 152px;vertical-align: middle;padding-left: 10px; padding-top: 10px; float: left;">
                        <table cellpadding="0" bgcolor="#666666" cellspacing="0" width="100%" height="100%">
                            <tr>
                                <td valign="middle" align="center">
                                    <asp:Image runat="server" ID="FriendImage" />
                                </td>
                            </tr>
                        </table>
                        
                    </div>
                    <div style="float: left; padding-left: 10px; padding-top: 10px;  padding-right:40px;" class="FriendDiv">
                        <table><%--
                            <tr>
                                <td>
                                    <label>Age</label>
                                </td>
                                <td>
                                    <asp:Label CssClass="AddLinkBig" runat="server" ID="AgeTextBox"></asp:Label>
                                </td>
                            </tr>--%>
                            <tr>
                                <td>
                                    <label>Sex</label>
                                </td>
                                <td>
                                    <asp:Label CssClass="AddLinkBig" runat="server" ID="SexTextBox"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Location</label>
                                </td>
                                <td>
                                   <asp:Label CssClass="AddLinkBig" runat="server" ID="LocationTextBox"></asp:Label>
                                </td>
                            </tr>
                            <%--<tr>
                                <td>
                                    <label>Email</label>
                                </td>
                                <td>
                                   <asp:Label CssClass="AddLinkBig" runat="server" ID="EmailTextBox"></asp:Label>
                                </td>
                            </tr>--%>
                        </table>


                       <div>
                                    <label>Events Posted</label>
                                
                                   <asp:Label CssClass="AddLinkBig" runat="server" ID="EventsLabel"></asp:Label>
                        </div>
                       <div>
                                    <label>Events Attended</label>
                               
                                   <asp:Label CssClass="AddLinkBig" runat="server" ID="AttendedLabel"></asp:Label>
                       </div>
                   
      
                    </div>
                </div>
                <br />
                <div>
                    <asp:Literal runat="server" ID="ScriptLiteral"></asp:Literal>
                    <div style="height: 20px;"><div style="float: left;"><img src="./image/ContactIcon.png" /></div><label class="AddWhiteLink" onclick="OpenRadBefore_SendMessage();">Send a Message</label></div>
                    <asp:Panel runat="server" ID="CalendarPanel"><img src="image/CalendarIcon.png" /><asp:HyperLink runat="server" Text="View Friend's Calendar" ID="CalendarLink" CssClass="AddLink" ForeColor="White"></asp:HyperLink>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="AddAsFriendPanel">
                        <span style="color: White; font-weight: bold; font-size: 16px; font-family: Arial;">+</span>
                        <asp:LinkButton CssClass="AddLink" OnClick="AddAsFriend" Text="Add As Friend" runat="server" ID="txtLink"></asp:LinkButton>
                    </asp:Panel>
                    <asp:Label runat="server" CssClass="AddLink" ID="AddedFriendLabel"></asp:Label>
                </div>
                
                
                
            </div>
            <div style="float:right; padding-right: 10px;">
                <asp:Panel runat="server" ID="PrivacyPanel">
                    <asp:Label runat="server" ID="PrivacyLabel" CssClass="FriendEvent"></asp:Label>
                </asp:Panel>
                <asp:Panel runat="server" ID="EventsPanel">
                    <div style="float: left; width: 420px; border: solid 4px #515151;">
                        <div style="background-color: #333333; padding-left: 2px; padding-bottom: 5px; border-bottom: dotted 1px black;">
                            <asp:Label CssClass="FriendEvent" runat="server" ID="EventsTitle"></asp:Label>
                        </div>
                        <ctrl:SearchElements runat="server" ID="EventsCtrl" />
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>

</asp:Content>

