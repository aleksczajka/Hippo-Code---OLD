<%@ Page Language="C#" MasterPageFile="~/SecondMasterPage.master" AutoEventWireup="true" 
CodeFile="EditEvent.aspx.cs" Inherits="EditEvent" Title="Edit local Happening on HippoHappenings" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register TagName="HippoTextBox" TagPrefix="ctrl" Src="~/Controls/HippoTextBox.ascx" %>
<%@ Register Src="~/Controls/Ads.ascx" TagName="Ads" TagPrefix="ctrl" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<rad:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="false" 
                KeepInScreenBounds="true" VisibleOnPageLoad="false"  Behaviors="none"
            ID="MessageRadWindowManager" Modal="true" RestrictionZoneID="RestrictionZone"
            runat="server">
                <Windows>
                    <rad:RadWindow Width="300" ClientCallBackFunction="OnClientClose" 
                    VisibleStatusbar="false" Skin="Vista" Height="350" ID="MessageRadWindow" 
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

    
    <div class="topDiv">
    <div style="float: left;color: #cccccc; font-family: Arial; font-size: 14px; width: 400px;">
    <div style="font-family: Arial; font-size: 30px; color: #666666; padding-bottom: 10px;">
        <asp:Label runat="server" ID="nameLabel" Text="Editing an Event"></asp:Label>
        <asp:Label runat="server" ID="UserNameLabel" Visible="false"></asp:Label>
        <asp:Label runat="server" ID="isEdit" Visible="false"></asp:Label>
        <asp:Label runat="server" ID="eventID" Visible="false"></asp:Label>
    </div>
<%--        You have two options to edit the event
--%>        
        <%--<ol>
            <li>
                Submit a text description of an update to the event. For example, if the venue or date of the event has changed, you can explain this in your description.
                These updates will be visible from a dropdown on the event's page.   <br /> <br />                     
                <button id="Button1" runat="server" onserverclick="GoToEventEdit" style=" cursor: pointer;font-weight: bold;padding:0;padding-bottom: 4px; font-size: 11px;height: 30px; width: 112px;background-color: transparent; color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat:no-repeat; border: 0;" 
                        onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" >Submit Update</button>
<br /><br />
            </li>
            <li>--%>
                Submit changes to the events details as they appear on the site. This includes adding media and categories, modifying dates and editing the description. 
                Most of these changes will need to be approved by the author of the event. However, if you choose to add media to the event, these changes will
                automatically be added to the site. For the rest of the changes, you will be notified by email when each one of the changes is approved/rejected by the author.
                
               <br /><br />
               If you are the owner/author of the event, or if the event does not have an owner, your changes will be accepted automatically.
                <br /><br />
                <button id="Button2" runat="server" onserverclick="GoToLogin" style=" cursor: pointer;font-weight: bold;padding:0;padding-bottom: 4px; font-size: 11px;height: 30px; width: 112px;background-color: transparent; color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat:no-repeat; border: 0;" 
                        onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" >Submit Changes</button>

           <%-- </li>
        </ol>--%>
            
    </div>
    <div style="float: right;">
        <ctrl:Ads ID="Ads1" runat="server" />
    </div>
    </div>
    </div>
<%--</rad:RadAjaxPanel>--%>
</asp:Content>

