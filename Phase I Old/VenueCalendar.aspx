<%@ Page Language="C#" EnableEventValidation="true" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="VenueCalendar.aspx.cs" Inherits="VenueCalendar" Title="Hipp Happ Venue Calendar" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register TagName="HippoTextBox" TagPrefix="ctrl" Src="~/Controls/HippoTextBox.ascx" %>
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
            //div.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.style.visibility = 'hidden';
         }
        //]]>
    </script>
    <asp:Panel runat="server" ID="ToolTipPanel"></asp:Panel>
    <rad:RadWindowManager Behaviors="Close, Move, Resize,Maximize"
            ID="RadWindowManager" Modal="true" DestroyOnClose="false" runat="server">
            <Windows>
                <rad:RadWindow ID="RadWindow1" VisibleStatusbar="false" VisibleTitlebar="false" VisibleOnPageLoad="false" Height="506" Width="560" Title="Send event information to your friends"
                     runat="server">
                </rad:RadWindow>
                <rad:RadWindow ID="RadWindow2" ClientCallBackFunction="AddCallback" VisibleStatusbar="false" VisibleTitlebar="false" 
                VisibleOnPageLoad="false" 
                Height="300" Width="300" Title="Add to calendar"
                     runat="server">
                </rad:RadWindow>
            </Windows>
            
        </rad:RadWindowManager>
    <div style="width: 900px; margin-left: -20px;">
        <div style="margin-left: 20px; padding-bottom: 30px;" id="topDiv">
            <asp:Label runat="server" CssClass="EventHeader" ID="CalendarHeading"></asp:Label>
            <div style="width: 350px; float: right; color: #cccccc; font-size: 14px; font-family: Arial; padding-right: 20px;">
              <label><asp:Label runat="server" ID="IntroLabel"></asp:Label></label>
            </div>  
        </div>
        
        
        <div align="center" style="background-image: url('image/CalendarBackground2.png'); width: 900px; background-repeat: no-repeat; padding-bottom: 200px;">
            <rad:RadScheduler runat="server" ID="RadScheduler1" Width="90%" Height="564px" 
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
                        <%--<ResourceTypes>
                            <rad:ResourceType DataSourceID="EventsDataSource" ForeignKeyField="PropertyID"
                                KeyField="PropertyID" Name="Property" TextField="Name" />
                        </ResourceTypes>--%>
                    </rad:RadScheduler>
       
            <asp:SqlDataSource runat="server" ID="EventsDataSource" ConnectionString="<%$ ConnectionStrings:Connection %>" SelectCommand="SELECT EO.DateTimeStart AS Start, EO.DateTimeEnd AS [End], E.Header, E.ID FROM User_Calendar UC, Events E, Event_Occurance EO WHERE EO.EventID=E.ID AND UC.EventID=E.ID AND UC.UserID=1"></asp:SqlDataSource>
        </div>

    </div>
 
</asp:Content>

