<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
CodeFile="EventUpdate.aspx.cs" Inherits="EventUpdate" Title="Enter Event Update" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register TagName="HippoTextBox" TagPrefix="ctrl" Src="~/Controls/HippoTextBox.ascx" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<rad:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="false" 
                KeepInScreenBounds="true" VisibleOnPageLoad="false"  Behaviors="none"
            ID="MessageRadWindowManager" Modal="true" RestrictionZoneID="RestrictionZone"
            runat="server">
                <Windows>
                    <rad:RadWindow Width="300" ClientCallBackFunction="OnClientClose" 
                    VisibleStatusbar="false" Skin="Black" Height="350" ID="MessageRadWindow" 
                     runat="server">
                    </rad:RadWindow>
                </Windows>
            </rad:RadWindowManager>
            <script type="text/javascript">
            function OnClientClose(oWnd, args)
            {

                window.location = args;
            }
           
            </script>
<%--<rad:RadAjaxPanel runat="server">--%>
 
<div style="padding-bottom: 100px;">
<rad:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="false" 
                KeepInScreenBounds="true" VisibleOnPageLoad="false"  Behaviors="none"
            ID="RadWindowManager1" Modal="true" RestrictionZoneID="RestrictionZone"
            runat="server">
                <Windows>
                    <rad:RadWindow Width="300" ClientCallBackFunction="OnClientClose" 
                    VisibleStatusbar="false" Skin="Black" Height="350" ID="RadWindow1" 
                     runat="server">
                    </rad:RadWindow>
                </Windows>
            </rad:RadWindowManager>
            <script type="text/javascript">
            function OnClientClose(oWnd, args)
            {

                window.location = args;
            }
           
            </script>
    <div style="font-family: Arial; font-size: 30px; color: #666666; padding-bottom: 10px;">
        <asp:Label runat="server" ID="nameLabel" Text="Enter an update for the event "></asp:Label>
        <asp:Label runat="server" ID="UserNameLabel" Visible="false"></asp:Label>
        <asp:Label runat="server" ID="isEdit" Visible="false"></asp:Label>
        <asp:Label runat="server" ID="eventID" Visible="false"></asp:Label>
    </div>
    <div id="topDiv6" style="color: #cccccc; font-family: Arial; font-size: 14px;">
       
        <table>
            <tr>
                <td width="150px">
                    Update Text:
                </td>
                <td>
                    <asp:TextBox Width="300px" runat="server" Height="100px" ID="UpdateText" TextMode="MultiLine" Wrap="true"></asp:TextBox>
                </td> 
            </tr>
            <tr>
                <td colspan="2" align="right">
                  <button id="Button2" runat="server" onserverclick="SubmitIt" style=" cursor: pointer;font-weight: bold;padding:0;padding-bottom: 4px; font-size: 11px;height: 30px; width: 112px;background-color: transparent; color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat:no-repeat; border: 0;" 
                        onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" >Submit Changes</button>

                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="ErrorLabel" CssClass="AddGreenLink"></asp:Label>
                </td>
            </tr>
        </table>
            
    </div>
    </div>
<%--</rad:RadAjaxPanel>--%>
</asp:Content>

