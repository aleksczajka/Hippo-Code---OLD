<%@ Page Language="C#" MasterPageFile="~/SecondMasterPage.master" AutoEventWireup="true" 
CodeFile="Friend.aspx.cs" Inherits="Friend" Title="Friend Page | HippoHappenings" %>
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
        Skin="Vista" Height="450" VisibleStatusbar="false" VisibleTitlebar="false" ID="MessageRadWindow" Title="Send Message" 
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
    <div id="topDiv TextNormal">
       <h1><asp:Label runat="server" ID="UserNameLabel"></asp:Label></h1>
        <div class="topDiv Text12">
            <div class="Friend1">
                 <div class="Friend2">
                     <div class="Friend3">
                        <div align="center" class="Friend4">
                                <table cellpadding="0" cellspacing="0" width="100%" height="100%">
                                    <tr>
                                        <td valign="middle" align="center" class="Friend5">
                                            <asp:Image runat="server" ID="FriendImage" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="Friend6">
                                <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="Friend7">
                                        <label>Sex</label>
                                    </td>
                                    <td class="Friend8">
                                        <asp:Label CssClass="NavyLink" runat="server" ID="SexTextBox"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Friend7">
                                        <label>Location</label>
                                    </td>
                                    <td class="Friend8">
                                       <asp:Label CssClass="NavyLink" runat="server" ID="LocationTextBox"></asp:Label>
                                    </td>
                                </tr>
                            </table>

                        <h2>Posts</h2>
                        <div class="Friend9">
                           <div class="Friend10"><asp:Label runat="server" ID="NumEventsLabel"></asp:Label></div>
                                <div class="Friend10"><asp:Label runat="server" ID="NumTripsLabel"></asp:Label></div>
                                <div class="Friend10"><asp:Label runat="server" ID="NumLocalesLabel"></asp:Label></div>
                                <div class="Friend10"><asp:Label runat="server" ID="NumAdsLabel"></asp:Label></div>
                            </div>
                    </div>
                            
                    </div>
                     <div class="TextNormal Friend11">
                                <h2><asp:Label runat="server" ID="UserNameLabel2"></asp:Label> Badges</h2>
                                <br />
                                <asp:Panel runat="server" ID="BadgesPanel"></asp:Panel>
                    </div>
                
                    <div class="TextNormal Friend12">
                    <asp:Literal runat="server" ID="ScriptLiteral"></asp:Literal>
                    <asp:Panel runat="server" ID="MessagePanel">
                        <div class="Friend13"><span class="NavyLink" onclick="OpenRadBefore_SendMessage();">Send a Message</span></div>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="CalendarPanel">
                        <div class="Friend13">
                            <asp:HyperLink runat="server" Text="View Friend's Calendar" ID="CalendarLink" CssClass="NavyLink"></asp:HyperLink>
                        </div>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="AddAsFriendPanel">
                        <div class="Friend13">
                            <asp:LinkButton CssClass="NavyLink" OnClick="AddAsFriend" Text="+ Add As Friend" runat="server" ID="txtLink"></asp:LinkButton>
                        </div>
                    </asp:Panel>
                    <div  class="Friend13">
                        <asp:Label runat="server" CssClass="Green12LinkNF" ID="AddedFriendLabel"></asp:Label>
                    </div>
                </div>
                </div>
                
                
            </div>
            <div class="Friend14">
                <asp:Panel runat="server" ID="PrivacyPanel">
                    <asp:Label runat="server" ID="PrivacyLabel" CssClass="NavyLink14"></asp:Label>
                </asp:Panel>
                <asp:Panel runat="server" ID="EventsPanel">
                    <div class="Friend15">
                        <div class="Friend16">
                            <asp:Label CssClass="NavyLink14" runat="server" ID="EventsTitle"></asp:Label>
                        </div>
                        <ctrl:SearchElements runat="server" ID="EventsCtrl" />
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>

</asp:Content>

