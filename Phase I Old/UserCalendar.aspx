<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
CodeFile="UserCalendar.aspx.cs" Inherits="UserCalendar" Title="Hipp Happ User Calendar" Debug="true" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register TagName="HippoTextBox" TagPrefix="ctrl" Src="~/Controls/HippoTextBox.ascx" %>
<%@ Register Src="~/Controls/UserMessage.ascx" TagName="UserMessage" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/GlanceDay.ascx" TagName="GlanceDay" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/Pager.ascx" TagPrefix="ctrl" TagName="Pager" %>
<%@ Register Src="~/Controls/Comments.ascx" TagName="Comments" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/SearchElements.ascx" TagName="SearchElements" TagPrefix="ctrl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/Controls/HippoRating.ascx" TagName="HippoRating" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/BigAd.ascx" TagName="BigAd" TagPrefix="ctrl" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<rad:RadCodeBlock runat="server">
    <script type="text/javascript">

       //<![CDATA[
        function OpenToolTip1(sender, eventArgs)
        {
            var d = document.getElementById('RadToolTipWrapper_ctl00_ContentPlaceHolder1_toolTipE'+eventArgs.get_appointment().get_id());
            d.style.display = 'block';
        }
//        function onEventAppointmentClick(sender, eventArgs)
//        {
//            window.location = "Event.aspx?EventID=" + eventArgs.get_appointment().get_id();
//        }
        function OpenWindow()
        {
            UserCalendar.ShowMessage();
        }
        function DeleteEventCallback(radWindow, returnValue)
        {
            if(returnValue != undefined && returnValue != null){
                window.location.reload();
            }
        }
        function OpenRad(theDiv, theID, type, reoccurr)
        {
            CloseDiv(theDiv);
            var userID = document.getElementById('userID');
            var win = $find("<%=RadWindow1.ClientID %>");
            if(type == '1')
                win.setUrl('MessageAlert.aspx?A=e&ID='+theID);
            else
                win.setUrl('MessageAlert.aspx?A=ge&ID='+theID+'&O='+reoccurr);
            
            win.show(); 
            win.center(); 
         }
         function OpenRadDelete(theDiv, theID, type, re)
        {
            CloseDiv(theDiv);
            var userID = document.getElementById('userID');
            var win = $find("<%=RadWindow3.ClientID %>");
            if(type == '1')
                win.setUrl('DeleteEvent.aspx?type=delete&ID='+theID);
            else if(type == '2')
                win.setUrl('DeleteEvent.aspx?type=delete&ID='+theID+"&O="+re);
            else
                win.setUrl('DeleteEvent.aspx?type=delete&ID='+theID+"&O="+re+'&U=T');
            win.show(); 
            win.center(); 
         }
          function OpenRadDeleteAd(theDiv, theID)
        {
            CloseDiv(theDiv);
            var userID = document.getElementById('userID');
            var win = $find("<%=RadWindow3.ClientID %>");
            win.setUrl('DeleteAd.aspx?type=delete&ID='+theID);
            win.show(); 
            win.center(); 
         }
         function CloseDiv(theDiv)
         {
            var div = document.getElementById(theDiv);
            div.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.style.zIndex = '1000';
//            div.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.style.visibility = 'hidden';
//            div.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.style.visibility = 'hidden';
         }
         
         function DeleteEvent(theID)
         {
            var userID = document.getElementById('userID');
            UserCalendar.DeleteUserEvent(theID, userID.innerHTML, Delete_Callback);
         }
         
         function DeleteAd(theID)
         {
            var userID = document.getElementById('userID');
            UserCalendar.DeleteAd(theID, userID.innerHTML, DeleteAd_Callback);
         }
         
         function Delete_Callback(somtin,response)
         {
        
            var theID = response.value;
            scheduler = $find('<%= RadScheduler1.ClientID %>');
            var result = scheduler.get_appointments();
            var eventToRemove = result.findByID(theID);
            eventToRemove._allowDelete = true;
            eventToRemove._allowEdit = true;
//            result.remove(eventToRemove);
            scheduler.deleteAppointment(eventToRemove);
         }
         
         function DeleteAd_Callback(response)
         {
            var theID = response.value;
            scheduler = $find('<%= RadScheduler2.ClientID %>');
            var result = scheduler.get_appointments();
            var eventToRemove = result.findByID(theID);
            eventToRemove._allowDelete = true;
            eventToRemove._allowEdit = true;
//            result.remove(eventToRemove);
            scheduler.deleteAppointment(eventToRemove);
         }
         
         function HandleDelete(theID)
         {
            CloseDiv('div'+theID);
            var a = confirm('Are you sure you want to remove this event from your calendar?');
            if(a)
            {
                DeleteEvent(theID);
            }
         }
         
         function HandleDeleteAd(theID)
         {
            CloseDiv('div'+theID);
            var a = confirm('YOUR ARE ABOUT TO DELETE THIS AD FROM THE SITE! If this is a FEATURED AD, you will not get your money back once the ad is deleted! Are you sure you want to remove this ad from the site?');
            if(a)
            {
                DeleteAd(theID);
            }
         
//            CloseDiv('div'+theID);
//            
//            createCookie('eventID', theID, 1);
//            
//            var userID = document.getElementById('userID');
//            
//            createCookie('userID', userID.innerHTML, 1);
//            
//            
//           var win = $find("<%=RadWindow2.ClientID %>");
//            win.setUrl('Delete.aspx');
//            win.show(); 
//            win.center(); 
         }
         
         function createCookie(name,value,days) {
            if (days) {
                var date = new Date();
                date.setTime(date.getTime()+(days*24*60*60*1000));
                var expires = "; expires="+date.toGMTString();
            }
            else var expires = "";
            document.cookie = name+"="+value+expires+"; path=/";
        }
         
         function OpenCommunication(theDiv, theID)
        {
            CloseDiv(theDiv);
            var win = $find("<%=RadWindow2.ClientID %>");
            win.setUrl('EventCommunicate.aspx?EID='+theID);
            win.show(); 
            win.center(); 
               
         }
         
          function OpenAlarm(theDiv, theID, theID2)
        {
            CloseDiv(theDiv);
            var win = $find("<%=RadWindow4.ClientID %>");
            win.setUrl('AlarmMessage.aspx?EID='+theID+'&RID='+theID2);
            win.show(); 
            win.center(); 
         }
         
         function OpenAdMsg(theID)
         {
            CloseDiv('div'+theID);
            var win = $find("<%=RadWindow1.ClientID %>");
            win.setUrl('MessageAlert.aspx?A=a&AID='+theID);
            win.show(); 
            win.center(); 
               
         }
         
//         function DoPostBack() 
//            {
//              __doPostBack('Button2','My Argument');     
//            }

//         
//         function __doPostBack(eventTarget, eventArgument) 
//         {
////            if (!theForm.onsubmit || (theForm.onsubmit() != false)) 
////            {
//                theForm.__EVENTTARGET.value = eventTarget;
//                theForm.__EVENTARGUMENT.value = eventArgument;
//                theForm.submit();
////            }
//         }
        //]]>
    </script>
</rad:RadCodeBlock>
   
    <div style="width: 900px; margin-left: -20px; z-index: 3000;">
    
        
        <div style="margin-left: 20px; padding-bottom: 30px; z-index: 3000;" id="topDiv">
            <asp:Label runat="server" ID="CalendarLabel" CssClass="EventHeader" Text="My Events Calendar"></asp:Label><br />
              <div class="topDiv">
              <div style="float: left;">
              <asp:Panel runat="server" ID="TextPanel">
                
                  <div style="width: 350px; float: left; color: #cccccc; font-size: 14px; font-family: Arial; padding-bottom: 10px;">
                  <label>Click on an event for information, send event to friends, modify event settings
                  , delete the event from your calendar, and go to the event's home page. 
                  To connect with people going to an event, go the event's home page by clicking 
                  on the event name on the pop-up.<span style="font-weight: bold; font-size: 14px;"> The calendar for ads you've posted is below!</span></label>
                    </div>
             </asp:Panel>  
                </div>
                <div style="float: right; padding-bottom: 5px; padding-right: 5px;">
                    
                            <ctrl:BigAd ID="BigAd1" runat="server" />
                      
                </div>
                                
            </div>
        </div>
        
     <rad:RadWindowManager Behaviors="Close, Move, Resize,Maximize"
            ID="RadWindowManager" Modal="true" Skin="Black" DestroyOnClose="false" runat="server">
            <Windows>
                <rad:RadWindow ID="RadWindow1" VisibleStatusbar="false"  
                VisibleTitlebar="false" VisibleOnPageLoad="false" Height="506" Width="560" 
                Title="Send event information to your friends"
                     runat="server">
                </rad:RadWindow>
                <rad:RadWindow ID="RadWindow2"  VisibleStatusbar="false" BorderWidth="0" 
                VisibleTitlebar="false" VisibleOnPageLoad="false" Height="200" Width="438" 
                Title="Set Communication Preferences" runat="server">
                </rad:RadWindow>
                <rad:RadWindow ID="RadWindow4" Behavior="close" VisibleStatusbar="false" BorderWidth="0" 
                VisibleTitlebar="false" VisibleOnPageLoad="false" Height="350" Width="500" 
                Title="Set Alarm" runat="server">
                </rad:RadWindow>
                <rad:RadWindow ID="RadWindow3"  VisibleStatusbar="false" BorderWidth="0" 
                VisibleTitlebar="false" ClientCallBackFunction="DeleteEventCallback" 
                VisibleOnPageLoad="false" Height="200" Width="438" 
                Title="Set Communication Preferences" runat="server">
                </rad:RadWindow>
            </Windows>
            
        </rad:RadWindowManager>
        <div class="topDiv" style="clear: both; padding-left: 10px; padding-bottom: 10px;">
                <rad:RadToolTip runat="server" Width="200px" Position="TopRight" RelativeTo="element" 
                 ID="iCalToolTip" ShowEvent="OnMouseOver"  
                TargetControlID="iCalHyperLink">
                    <div class="EventDiv">
                        <label>
                            Download this ics file to import your HippoHappenings calendar
                            to your iCal, iPhone, or Outlook.
                        </label>
                    </div>
                </rad:RadToolTip>
                <asp:HyperLink runat="server" Font-Size="10px" Visible="false" ID="iCalHyperLink" 
                Text="Export to iCal / iPhone / Outlook" CssClass="AddOrangeLink" 
                NavigateUrl="~/iCalHandler.ashx"></asp:HyperLink>
            </div>
        <div align="center" style="background-image: url('image/CalendarBackground2.png'); width: 900px; background-repeat: no-repeat; padding-bottom: 40px; z-index: 3000;">
           <rad:RadAjaxPanel runat="server" >
          <%--<asp:UpdatePanel runat="server" UpdateMode="conditional" ID="UpdatePanel1">
          <ContentTemplate>--%>
          <asp:Panel runat="server" ID="ToolTipPanel"></asp:Panel>
          <%--<div id="Container" onclick="__doPostBack('UpdatePanel1', '');">--%>
            <asp:Label runat="server" ID="ErrorLabRad1" ForeColor="red"></asp:Label>
            
            <rad:RadScheduler runat="server" ID="RadScheduler1" Width="850px" Height="564px" 
            OverflowBehavior="scroll"
                        Skin="Black" DataKeyField="ID" DataStartField="Start"
                        DataEndField="[End]" DataSubjectField="ShortHeader" DayEndTime="23:59:59"
                        DayStartTime="00:00:00" SelectedView="monthView"
                         ShowAllDayRow="false" OnAppointmentCreated="RadScheduler1_AppointmentCreated" 
                         AllowEdit="false" AllowDelete="false" ShowFullTime="true" StartInFullTime="true" 
                         DayView-WorkDayEndTime="23:59:59" DayView-WorkDayStartTime="00:00:00"
                         AllowInsert="false" >
                        <WeekView HeaderDateFormat="dd.MM" />
                        <TimelineView UserSelectable="false" />
                        <MonthView ShowResourceHeaders="true"/>
                        
                        <%--<ResourceTypes>
                            <rad:ResourceType DataSourceID="EventsDataSource" ForeignKeyField="PropertyID"
                                KeyField="PropertyID" Name="Property" TextField="Name" />
                        </ResourceTypes>--%>
                    </rad:RadScheduler>
                  <%--  </div>--%>
            
          <%--</ContentTemplate>
          </asp:UpdatePanel>--%>
          </rad:RadAjaxPanel>
        </div>
        <asp:Panel runat="server" ID="AdsCalendarPanel">
            <div style="margin-left: 20px; padding-bottom: 30px;" id="topDiv2">
                <asp:Label ID="Label1" runat="server" CssClass="EventHeader" Text="My Ads Calendar"></asp:Label><br />
                <div style="width: 350px; float: right; color: #cccccc; font-size: 14px; font-family: Arial; padding-bottom: 10px;">
                    <label>Here you will see the start and end times of all the ads you post on HippoHappenings! Featured 
                    ads have a purple bottom border. Their end date will always be seen one day after the start since 
                    we have no way of knowing when your featured ad will finish posting. Click on the ad to send 
                    info to your friends, delete the ad, and go to the ad's home page.</label>
                </div>
            </div>
            <div align="center" style="background-image: url('image/CalendarBackground2.png'); width: 900px; background-repeat: no-repeat; padding-bottom: 200px;">
         <rad:RadAjaxPanel ID="RadAjaxPanel1" runat="server" >
           <%-- <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="conditional">
          <ContentTemplate>--%>
           <asp:Panel runat="server" ID="ToolTipPanel2"></asp:Panel>
                <rad:RadScheduler runat="server" ID="RadScheduler2" Width="90%" Height="564px" 
                OverflowBehavior="scroll"
                            Skin="Black" DataKeyField="AdID" DataStartField="Start"
                            DataEndField="[End]" DataSubjectField="Header"
                             SelectedView="monthView"
                              AllowDelete="false"
                              OnAppointmentCreated="RadScheduler2_AppointmentCreated"  
                              AllowEdit="false" AllowInsert="false"  DayEndTime="23:59:59"
                        DayStartTime="00:00:00">
                            <WeekView HeaderDateFormat="dd.MM" />
                            <TimelineView UserSelectable="false" />
                            <MonthView ShowResourceHeaders="true"/>
                            <%--<ResourceTypes>
                                <rad:ResourceType DataSourceID="EventsDataSource" ForeignKeyField="PropertyID"
                                    KeyField="PropertyID" Name="Property" TextField="Name" />
                            </ResourceTypes>--%>
                        </rad:RadScheduler>
            <%-- </ContentTemplate>
             </asp:UpdatePanel>--%>
             </rad:RadAjaxPanel>
            </div>
            <asp:Literal runat="server" ID="userLiteral"></asp:Literal>
        </asp:Panel>
        
    </div>

</asp:Content>

