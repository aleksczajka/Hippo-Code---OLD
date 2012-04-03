<%@ Page Language="C#" EnableEventValidation="true" MasterPageFile="~/SecondMasterPage.master" 
AutoEventWireup="true" CodeFile="VenueCalendar.aspx.cs" Inherits="VenueCalendar" 
Title="HippoHappenings Locale Calendar" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register Src="~/Controls/UserMessage.ascx" TagName="UserMessage" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/GlanceDay.ascx" TagName="GlanceDay" TagPrefix="ctrl" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">

       //<![CDATA[

        
        function OpenRad2(theDiv, theID, type, reoccurr)
        {
            CloseDiv(theDiv);
            var userID = document.getElementById('userID');
            var win = $find("<%=RadWindow1.ClientID %>");
            if(type == 'E')
                win.setUrl('MessageAlert.aspx?A=e&ID='+theID);
            else
                win.setUrl('MessageAlert.aspx?A=ge&ID='+theID+'&O='+reoccurr);
            
            win.show(); 
            win.center(); 
               
         }
         function onEventAppointmentClick(sender, eventArgs)
        {
            window.location = 'Event.aspx?EventID=' + eventArgs.get_appointment().get_id();
        }
        
        function AddCallback(radwindow, returnvalue)
        {
            if(returnvalue != undefined && returnvalue != null)
                window.location.reload();
        }
        
         function AddToCalendar(theDiv, theID, type, reoccurr)
         {
            CloseDiv(theDiv);
            var userID = document.getElementById('userID');
            var win = $find("<%=RadWindow2.ClientID %>");
            if(type == 'E')
                win.setUrl('Controls/AddEventAlert.aspx?ID='+theID);
            else
                win.setUrl('Controls/AddEventAlert.aspx?ID='+theID+'&T=G&O='+reoccurr);
            win.show(); 
            win.center();
         }
         function CloseDiv(theDiv)
         {
            var div = document.getElementById(theDiv);
            div.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.style.zIndex = '1000';
         }
        //]]>
    </script>
    <asp:Panel runat="server" ID="ToolTipPanel"></asp:Panel>
    <rad:RadWindowManager Behaviors="Close, Move, Resize,Maximize"
            ID="RadWindowManager" Modal="true" Skin="Vista" DestroyOnClose="false" runat="server">
            <Windows>
                <rad:RadWindow ID="RadWindow1" VisibleStatusbar="false" VisibleTitlebar="false" 
                VisibleOnPageLoad="false" Height="506" Width="560" Title="Send event information to your friends"
                     runat="server">
                </rad:RadWindow>
                <rad:RadWindow ID="RadWindow2" ClientCallBackFunction="AddCallback" 
                VisibleStatusbar="false" VisibleTitlebar="false" 
                VisibleOnPageLoad="false" 
                Height="200" Width="300" Title="Add to calendar"
                     runat="server">
                </rad:RadWindow>
            </Windows>
            
        </rad:RadWindowManager>
    <div class="TextNormal" >
        <div id="topDiv">
            <h1><asp:Label runat="server" ID="CalendarHeading"></asp:Label></h1>
            <div style="width: 350px; float: right; padding-right: 20px;">
              <label><asp:Label runat="server" ID="IntroLabel"></asp:Label></label>
            </div>  
        </div>
        
        
        <div align="center" style="padding-top: 20px; padding-bottom: 150px;" >
            <rad:RadScheduler runat="server" ID="RadScheduler1" Width="90%" Height="100%"
            OverflowBehavior="scroll"
                        Skin="Web20" DataKeyField="ID" 
                        DataStartField="Start"
                        DataEndField="End" DataSubjectField="Header"
                        EnableViewState="true" DayEndTime="23:59:59"
                                                        DayStartTime="00:00:00"  SelectedView="monthView"
                         ShowAllDayRow="false" AllowDelete="false" 
                         OnAppointmentCreated="RadScheduler1_AppointmentCreated" 
                         AllowEdit="false" AllowInsert="false">
                        <WeekView HeaderDateFormat="dd.MM" />
                        <TimelineView UserSelectable="false" />
                        <MonthView ShowResourceHeaders="true"/>
                    </rad:RadScheduler>
       
            <asp:SqlDataSource runat="server" ID="EventsDataSource" ConnectionString="<%$ ConnectionStrings:Connection %>" SelectCommand="SELECT EO.DateTimeStart AS Start, EO.DateTimeEnd AS [End], E.Header, E.ID FROM User_Calendar UC, Events E, Event_Occurance EO WHERE EO.EventID=E.ID AND UC.EventID=E.ID AND UC.UserID=1"></asp:SqlDataSource>
        </div>

    </div>
 <div class="topDiv" style="width: 847px;clear: both;margin-left: -18px;position: absolute;bottom: 0px;margin-bottom: -8px;">
                <div style="clear: both;" class="topDiv">
                    <div style="float: left;width: 6px;"><img src="NewImages/EventFooterTopLeft.png" /></div>
                    <div style=" height: 6px;width: 835px;float: left; background: url('NewImages/EventFooterTop.png'); background-repeat: repeat-x; background-position: top;">
                        &nbsp;
                    </div>
                    <div style="float: left;width: 6px;"><img src="NewImages/EventFooterTopRight.png" /></div>
                </div>
                <div style="clear: both; background-color: #ebe7e7;"> 
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td width="6px" style="background-image: url('NewImages/EventFooterLeft.png'); background-repeat: repeat-y;"></td>
                            <td>
                                <div class="Text12" style="width: 835px; background-color: #ebe7e7;">
                                    <div class="ContentFooter">
                                        <b>Hippo Locale Tips</b>                                            <ol>                                                <li>                                                    Earn points toward being featured on our home page by posting more venues.                                                 </li>                                                <li>                                                    Post, Share, Add to favorites, text, email, discuss.                                                 </li>                                                <li>                                                    Receive notifications of new events posted in your favorite venue.                                                </li>                                                <li>                                                    Edit venues you own or submit changes to venues owned by others.                                                </li>                                            </ol>
                                    </div>
                                </div>
                            </td>
                            <td width="6px" style="background-image: url('NewImages/EventFooterRight.png'); background-repeat: repeat-y;"></td>
                        </tr>
                    </table>                       
                </div>
                <div style="clear: both;" class="topDiv">
                    <div style="float: left;width: 6px;"><img src="NewImages/EventFooterBottomLeft.png" /></div>
                    <div style="height: 6px;width: 835px;float: left; background: url('NewImages/EventFooterBottom.png'); background-repeat: repeat-x; background-position: top;">
                        &nbsp;
                    </div>
                    <div style="float: left;width: 6px;"><img src="NewImages/EventFooterBottomRight.png" /></div>
                </div>
           </div>
</asp:Content>

