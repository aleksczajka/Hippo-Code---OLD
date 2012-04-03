<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SearchResults.aspx.cs" EnableEventValidation="false" Inherits="SearchResults" %>
<%@ Register Src="~/Controls/SearchElements.ascx" TagName="SearchElements" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/VenueSearchElements.ascx" TagName="VenueSearchElements" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/AdSearchElements.ascx" TagName="AdSearchElements" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/GroupSearchElements.ascx" TagName="GroupSearchElements" TagPrefix="ctrl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register Src="~/Controls/BigAd.ascx" TagName="BigAd" TagPrefix="ctrl" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Duration="1" NoStore="true" VaryByParam="*" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <script src="http://maps.google.com/maps?file=api&amp;v=2&amp;sensor=false&amp;key=ABQIAAAAjjoxQtYNtdn3Tc17U5-jbBR2Kk_H7gXZZZniNQ8L14X1BLzkNhQjgZq1k-Pxm8FxVhUy3rfc6L9O4g" type="text/javascript"></script>

    <link href="~/StyleSheet.css" type="text/css" rel="stylesheet" />
    <style type="text/css">
    .rtsLevel a
    {
        height: 32px !important;
    }
    .RadScheduler .rsMonthView .rsApt 
    {
        width: auto !important;
        float: left;
    }
    
    div#rsRow:after 
{
   content: "."; 
   display: block; 
   height: 0; 
   clear: both; 
   visibility: hidden;
}

/*hack for IE 6*/
* html div#rsRow {height: 1%;}

/*hack for IE 7*/
* + html div#rsRow {height: 1%;}

div.rsRow:after 
{
   content: "."; 
   display: block; 
   height: 0; 
   clear: both; 
   visibility: hidden;
}
    .RadScheduler .rsWrap .rsDateWrap
    {
        padding-right: 2px;
        float: left;
    }
    
    .RadScheduler .rsRow .rsWrap
    {
        float: left;
        height: 30px !important;
        width: auto !important;
        padding-right: 3px;
    }
    
    .RadScheduler .rsRow .rsWrap .rsApt
    {
        float: left;
    }
    
    .RadScheduler .rsRow
    {
        height: 50px;
    }
    
    .RadScheduler .rsApt
    {
       position: static !important;
       line-height: 15px !important
    }
    
</style>
</head>
<body style="background-color: #333333; margin-top: 0px;" onunload="GUnload();">
    <form id="form1" runat="server" style="background-color: #333333; ">
             <rad:RadScriptManager ID="ScriptManager" runat="server" />
<asp:Literal runat="server" ID="ScriptLiteral"></asp:Literal>
<script type="text/javascript">
        function selectCalendarDate(theDate, toolTipID)
        {
            ShowLoadingCalendar();
            selectSecondTab();
            //addvance to the month and show the desired tooltip
            
            var scheduler = $find('<%= RadScheduler1.ClientID %>');
            __doPostBack('ProgressCalendarButton',theDate+';'+toolTipID);
        
            //this method works but this is taken care of in AJAX above
//            var tooltip = $find('False2241520183');
//            tooltip.show();
        }
        
        function showTooltip(tooltipID)
        {
            var tooltip = $find(tooltipID);
            tooltip.show();
        }
        
        function selectFirstTab()
         {
            var firstTab = document.getElementById('RadPageView1');
            firstTab.style.display = 'block';
            var secondTab = document.getElementById('RadPageView2');
            secondTab.style.display = 'none';
            
            //close any open tooltip
            if(Telerik.Web.UI.RadToolTipController != null
                && Telerik.Web.UI.RadToolTipController != undefined)
            {
                var controller = Telerik.Web.UI.RadToolTipController.getInstance(); 
                if(controller != null && controller != undefined)
                {
                    var tooltip = controller.get_activeToolTip(); 
                    if(tooltip != null && tooltip != undefined)
                        tooltip.hide();
                }
            }
            var theTab = document.getElementById('RadTabStrip1');
            theTab.innerHTML = '<div class="rtsLevel rtsLevel1"> <ul class="rtsUL"><li class="rtsLI rtsFirst"><a class="rtsLink rtsSelected" href="javascript:selectFirstTab()" style="color: rgb(204, 204, 204); font-family: Arial; font-size: 18px;"><span class="rtsOut"><span class="rtsIn"><span class="rtsTxt">Map&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|</span></span></span></a></li><li class="rtsLI rtsLast"><a tabindex="1" class="rtsLink rtsAfter" href="javascript:selectSecondTab()" style="color: rgb(204, 204, 204); font-family: Arial; font-size: 18px;"><span class="rtsOut"><span class="rtsIn"><span class="rtsTxt">Calendar</span></span></span></a></li></ul> </div><input id="RadTabStrip1_ClientState" name="RadTabStrip1_ClientState" type="hidden">';
         }
         function selectSecondTab()
         {
            var firstTab = document.getElementById('RadPageView1');
            firstTab.style.display = 'none';
            var secondTab = document.getElementById('RadPageView2');
            secondTab.style.display = 'block';
            
            var theTab = document.getElementById('RadTabStrip1');
            theTab.innerHTML = '<div class="rtsLevel rtsLevel1"> <ul class="rtsUL"><li class="rtsLI rtsFirst"><a class="rtsLink" href="javascript:selectFirstTab()" style="color: rgb(204, 204, 204); font-family: Arial; font-size: 18px;"><span class="rtsOut"><span class="rtsIn"><span class="rtsTxt">Map&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|</span></span></span></a></li><li class="rtsLI rtsLast"><a tabindex="1" class="rtsLink rtsAfter rtsSelected" href="javascript:selectSecondTab()"  style="color: rgb(204, 204, 204); font-family: Arial; font-size: 18px;"><span class="rtsOut"><span class="rtsIn"><span class="rtsTxt">Calendar</span></span></span></a></li></ul> </div><input id="RadTabStrip1_ClientState" name="RadTabStrip1_ClientState" type="hidden">';
         
         }
    </script>
             <script type="text/javascript">
    var map = null;
    var geocoder = null;
    var gmarkers = []; 
    

           
       
    //From: http://code.google.com/apis/maps/documentation/javascript/v2/overlays.html#Markers
    // Create a base icon for all of our markers that specifies the
    // shadow, icon dimensions, etc.
    var baseIcon = new GIcon(G_DEFAULT_ICON);
    baseIcon.shadow = "http://www.google.com/mapfiles/shadow50.png";
    baseIcon.iconSize = new GSize(20, 34);
    baseIcon.shadowSize = new GSize(37, 34);
    baseIcon.iconAnchor = new GPoint(0, 0);
    baseIcon.infoWindowAnchor = new GPoint(9, 2);

    var baseIcon2 = new GIcon(G_DEFAULT_ICON);
    baseIcon2.iconSize = new GSize(9, 9);
    baseIcon2.shadowSize = new GSize(10, 10);
    baseIcon2.shadow = "http://hippohappenings.com/images/gShadow.png";
    baseIcon2.iconAnchor = new GPoint(4, 5);
    baseIcon2.infoWindowAnchor = new GPoint(9, 2);

    // Creates a marker whose info window displays the letter corresponding
    // to the given index.
    
//    function destroyAllmarkers()
//    {
//        var i;
//        for(i = 0;i<gmarkers.length;i++)
//        {
//            GEvent.clearInstanceListeners(gmarkers[0]) 
//            map.removeOverlay(gmarkers[0]);
//        }
//    }
    
    function createMarker(point, index, theName, theChoice) {
      // Create a lettered icon for this point using our icon class
      var letteredIcon;
      var zIndexStr = returnZNegative;
      if(theChoice == 0)
      {
          zIndexStr = returnZPositive;
          if(index > 25)
          {
              letteredIcon = new GIcon(baseIcon2);
              letteredIcon.image = "http://hippohappenings.com/images/gDot.png";
              // Set up our GMarkerOptions object
              markerOptions = { icon:letteredIcon, zIndexProcess:zIndexStr};
              var marker = new GMarker(point, markerOptions);
              GEvent.addListener(marker, "click", function() {
                marker.openInfoWindowHtml("<div class='AddLink' style=\"width: 200px;color: #666666; font-weight: normal;\">"+theName+"</div>");
              });
              gmarkers.push(marker); 
          }
          else
          {
              var letter = String.fromCharCode("A".charCodeAt(0) + index);
              
              letteredIcon = new GIcon(baseIcon);
              letteredIcon.image = "http://www.google.com/mapfiles/marker" + letter + ".png";
              
              // Set up our GMarkerOptions object
              markerOptions = { icon:letteredIcon, zIndexProcess:zIndexStr};
              var marker = new GMarker(point, markerOptions);
              
              if(gmarkers.length > index)
              {
                  GEvent.clearInstanceListeners(gmarkers[index]) 
                  map.removeOverlay(gmarkers[index]);
                     
                  GEvent.addListener(marker, "click", function() {
                    marker.openInfoWindowHtml("<div class='AddLink' style=\"width: 200px;color: #666666; font-weight: normal;\">"+theName+"</div>");
                  });
                    gmarkers[index] = marker;
              }
              else
              {
                  GEvent.addListener(marker, "click", function() {
                    marker.openInfoWindowHtml("<div class='AddLink' style=\"width: 200px;color: #666666; font-weight: normal;\">"+theName+"</div>");
                  });
                    gmarkers.push(marker); 
              }
              
          }
      }
      else
      {
          letteredIcon = new GIcon(baseIcon2);
          letteredIcon.image = "http://hippohappenings.com/images/gDot.png";
          // Set up our GMarkerOptions object
          markerOptions = { icon:letteredIcon, zIndexProcess:zIndexStr};
          var marker = new GMarker(point, markerOptions);
          GEvent.addListener(marker, "click", function() {
            marker.openInfoWindowHtml("<div class='AddLink' style=\"width: 200px;color: #666666; font-weight: normal;\">"+theName+"</div>");
          });
          gmarkers.push(marker); 
      }
      
      
      return marker;
    }
        
    function returnZNegative(marker, b)
    {
        return 1000;
    }
    
    function returnZPositive(marker, b)
    {
        return 2000;
    }
    
    function showAddressUS(theVenue, theChoice, address, tryCount, wholeAddress, repeatCount, theNumber, theName, centerIt) {
       
       // Create our "tiny" marker icon
        var blueIcon = new GIcon(G_DEFAULT_ICON);
        blueIcon.image = "http://hippohappenings.com/image/HippoMapPoint4.png";
		
		// Set up our GMarkerOptions object
		markerOptions = { icon:blueIcon };

       var temp = wholeAddress.replace(/@&/g, " ").split(" ");
      if (geocoder) {
        geocoder.getLatLng(
          address.replace(/@&/g, " "),
          function(point) {
            if (!point) 
            {
                showAddressUS(theVenue, theChoice, address, tryCount, wholeAddress, repeatCount, theNumber, theName, centerIt)
            } 
            else {
              if(centerIt)
              {
                map.setCenter(point, 11);
              }
              var marker = createMarker(point, theNumber, theName +"<br/><br/>at "+theVenue+"<br/>"+address.replace(/@&/g, " "), theChoice);
              //var marker = new GMarker(point, markerOptions);
              map.addOverlay(marker);
            }
          }
        );
      }
    }

    function myclick(i) 
    {
        var marker = gmarkers[i];
        GEvent.trigger(marker, "click");
    } 

    function showAddress(theVenue, theChoice, address, tryCount, wholeAddress, repeatCount, theNumber, theName, centerIt) {
       
       // Create our "tiny" marker icon
        var blueIcon = new GIcon(G_DEFAULT_ICON);
        blueIcon.image = "http://hippohappenings.com/image/HippoMapPoint4.png";
		
		// Set up our GMarkerOptions object
		markerOptions = { icon:blueIcon };

       var temp = wholeAddress.replace(/@&/g, " ").split(" ");
      if (geocoder) {
        geocoder.getLatLng(
          address.replace(/@&/g, " "),
          function(point) {
            if (!point) 
            {
              if(tryCount > temp.length && repeatCount == 2)
              {
                //alert(address.replace(/@&/g, " ") + tryCount+" not found. Try correcting this address.");
              }
              else
              {
                  var addition = 1;
                  if(tryCount > temp.length || repeatCount == 2)
                  {
                    if(repeatCount == 1)
                    {
                        tryCount = 0;
                    }
                    
                    var nextAddress = "";
                      var i;
                      for(i=0;i<temp.length;i++)
                      {
                            if(i==temp.length-1)
                            {
                                nextAddress += temp[i] + " ";
                            }
                            else
                            {
                                if(i != temp.length - (tryCount+1) && i != temp.length - (tryCount+2))
                                {
                                    
                                    nextAddress += temp[i]+" ";
                                }
                            }
                      }
                      showAddress(theVenue, theChoice, nextAddress, tryCount + 1, wholeAddress, 2, theNumber, theName, centerIt);
                    
                  }
                  else
                  {
                      var nextAddress = "";
                      var i;
                      for(i=0;i<temp.length;i++)
                      {
                            if(i==temp.length-1)
                            {
                                nextAddress += temp[i] + " ";
                            }
                            else
                            {
                                if(i != temp.length - (tryCount+1))
                                {
                                    nextAddress += temp[i]+" ";
                                }
                            }
                      }
                      showAddress(theVenue, theChoice, nextAddress, tryCount + 1, wholeAddress, 1,  theNumber, theName, centerIt);
                  }
              }
            } 
            else {
              if(centerIt)
              {
                map.setCenter(point, 11);
              }
              var marker = createMarker(point, theNumber, theName +"<br/><br/>at "+theVenue+"<br/>"+address.replace(/@&/g, " "), theChoice);
              //var marker = new GMarker(point, markerOptions);
              map.addOverlay(marker);
            }
          }
        );
      }
    }
var idArray = new Array();
                var n = 0;
               function GetRadWindow() 
                { 
                  var oWindow = null; 
                  if (window.radWindow) 
                     oWindow = window.radWindow; 
                  else if (window.frameElement.radWindow) 
                     oWindow = window.frameElement.radWindow; 
                  return oWindow; 
                }   
                function CloseWindow(returnV)
                {
                
                    var oWindow = GetRadWindow(); 
                    oWindow.Close(returnV); 
                }
              
                function Search() 
                { 
                    CloseWindow();
                }
    </script>

            
     <div class="topDiv" style="background-color: #333333; ">
        <div style="float: left; width: 420px;background-color: #333333; ">
            <asp:Label runat="server" CssClass="ResultsLabel" ID="ResultsLabel"></asp:Label>
<%--            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <ContentTemplate>
--%>                     <div style="width: 420px;background-color: #333333;height: 30px;"> 
                             <div style="float: left;">  <asp:DropDownList runat="server" AutoPostBack="true"  
                            OnSelectedIndexChanged="SortResults" ID="SortDropDown">
                                <asp:ListItem Text="Sort By..." Value="-1"></asp:ListItem>
                                <asp:ListItem Text="Date Up" Value="DateTimeStart ASC"></asp:ListItem>
                                <asp:ListItem Text="Date Down" Value="DateTimeStart DESC"></asp:ListItem>
                                <asp:ListItem Text="Title Up" Value="Header ASC"></asp:ListItem>
                                <asp:ListItem Text="Title Down" Value="Header DESC"></asp:ListItem>
                                <asp:ListItem Text="Venue Up" Value="Name ASC"></asp:ListItem>
                                <asp:ListItem Text="Venue Down" Value="Name DESC"></asp:ListItem>
                            </asp:DropDownList>   
                            </div>
                            <div style="padding-left: 20px; float: right">
                                <asp:Label runat="server" ID="NumsLabel" CssClass="AddWhiteLink"></asp:Label>
                            </div>
                     </div>   
                    <div class="EventDiv">
                        <asp:Panel runat="server" ID="Panel7" Width="440px"><asp:Label runat="server" ForeColor="Red" ID="ErrorLabel"></asp:Label>
                                <asp:Literal runat="server" ID="HelpUsLiteral"></asp:Literal>
                              <ctrl:SearchElements Visible="false" runat="server" ID="EventSearchElements" />
                              <ctrl:VenueSearchElements runat="server" ID="VenueSearchElements" Visible="false" />
                              <ctrl:AdSearchElements runat="server" ID="AdSearchElements" Visible="false" />
                              <ctrl:GroupSearchElements runat="server" ID="GroupSearchElements" Visible="false" />
                              <br /><br />
                         </asp:Panel>
                    </div>
<%--                </ContentTemplate>
            </asp:UpdatePanel>
--%>        </div>
        <div style="float: left; width: 440px;padding-left: 3px;background-color: #333333; ">
            <rad:RadTabStrip runat="server" ID="RadTabStrip1" Orientation="HorizontalTop"
                 MultiPageID="RadMultiPage1" Skin="Telerik" SelectedIndex="0" >
                <Tabs>
                    <rad:RadTab Font-Size="18px" ForeColor="#cccccc" Font-Names="Arial" 
                    Text="Map&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|" TabIndex="0" PageViewID="RadPageView1">
                    </rad:RadTab>
                    <rad:RadTab Font-Size="18px" ForeColor="#cccccc" Font-Names="Arial" 
                    Text="<div onclick='selectSecondTab();'>Calendar</div>" TabIndex="1" 
                    PageViewID="RadPageView2">
                    </rad:RadTab>
                </Tabs>
            </rad:RadTabStrip>
            <rad:RadMultiPage runat="server" ID="RadMultiPage1" 
            SelectedIndex="0">
                    <rad:RadPageView  runat="server" ID="RadPageView1" TabIndex="0">
                        <div style="width: 440px; height: 464px; padding-top: 3px;padding-left: 5px;">
                            <div id="map_canvas" style="width: 440px; height: 464px; display: block;"></div>
                        </div>       
                    </rad:RadPageView>
                    <rad:RadPageView  runat="server" ID="RadPageView2" TabIndex="1">
                        <div style="padding-left: 5px;">
                        <div runat="server" id="DateDiv" style="display: none;"></div>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
                            <ContentTemplate>
                            <asp:Literal runat="server" ID="UpdatePanelScriptLiteral"></asp:Literal>
                            <script type="text/javascript" language="javascript">
                                function ShowLoadingCalendar()
                                {
                                    var d = document.getElementById('Div1');
                                    d.style.display = 'block';
                                }
                            </script> 
                                 <asp:Panel runat="server" ID="ToolTipPanel"></asp:Panel>
                                 <table width="100%">
                                    <tr>
                                        <td width="50px">
                                            <asp:LinkButton OnClientClick="ShowLoadingCalendar()" CssClass="AddOrangeLink" ID="LinkButton5" runat="server" Text="< prev" OnClick="DevanceMonth"></asp:LinkButton>
                                        </td>
                                        <td width="50px">
                                            <asp:LinkButton OnClientClick="ShowLoadingCalendar()" CssClass="AddOrangeLink"  runat="server" ID="LinkBzatton" Text="next >" OnClick="AdvanceMonth"></asp:LinkButton>
                                        </td>
                                        <td align="center">
                                            <asp:Label CssClass="EventHeader" runat="server" ID="MonthLabel"></asp:Label>
                                        </td>
                                    </tr>
                                 </table>
                                 <div class="topDiv" style=" position: relative;">
                                    <div align="center" id="Div1"  style="display: none;z-index: 3000; padding-left: 30px;margin-top: 50px; position: absolute; width: 400px; height: 200px;margin-left: -20px;" >
                                        <div style="float: left;opacity: .5; filter: alpha(opacity=50);height: 200px; width: 100px; background-color: #cccccc;">
                                        </div>
                                        <div style="float: left;height: 200px;width: 200px;">
                                            <div style=" float: left;opacity: .5; filter: alpha(opacity=50); background-color: #cccccc; width: 200px; height: 50px;"></div>
                                            <div style="padding-top: 30px;clear: both;position: relative;top: 45; height: 70px;background-color: #363636; opacity: 1; filter: alpha(opacity=100);">
                                                    <img src="image/ajax-loaderBig.gif" />
                                                    <span class="updateProgressMessage">Loading ...</span>
                                            </div>
                                            <div style="float: left;opacity: .5; filter: alpha(opacity=50); background-color: #cccccc; width: 200px; height: 50px;"></div>
                                        </div>
                                        <div style="float: left;opacity: .5; filter: alpha(opacity=50);height: 200px; width: 100px; background-color: #cccccc;"></div>
                                    </div>
                                </div>
                                     <div class="topDiv" style="width: 300px; float: left;">
                                        
                                        <asp:LinkButton runat="server" ID="ProgressCalendarButton" OnClick="ProgressCalendar"></asp:LinkButton>
                                         <rad:RadScheduler BorderWidth="1px" BorderStyle="solid" runat="server" 
                                         ID="RadScheduler1" Width="420px" Height="320px" 
                                                OverflowBehavior="expand" BorderColor="#1fb6e7" BackColor="#1b1b1b"
                                                Skin="Black" DataKeyField="CalendarKey"  DataStartField="DateTimeStart"
                                                DataEndField="DateTimeStart" DataSubjectField="CalendarNum" DayEndTime="23:59:59"
                                                DayStartTime="00:00:00" SelectedView="monthView"
                                                 ShowAllDayRow="false" OnAppointmentCreated="RadScheduler1_AppointmentCreated" 
                                                 AllowEdit="false" AllowDelete="false" ShowHeader="false" ShowFullTime="true" 
                                                 StartInFullTime="true" AllowInsert="false">
                                                <WeekView UserSelectable="false" />
                                                <DayView UserSelectable="false" />
                                                <TimelineView UserSelectable="false" />
                                                <MonthView UserSelectable="false" MinimumRowHeight="5"/>
                                         </rad:RadScheduler>
                                     </div>
                                    
                             </ContentTemplate>
                        </asp:UpdatePanel>
                        </div>
                    </rad:RadPageView>
            </rad:RadMultiPage>
        </div>
     </div>
     <asp:Literal runat="server" ID="TopLiteral"></asp:Literal>
     <script type="text/javascript">
         if (GBrowserIsCompatible())
             { 
                map = new GMap2(document.getElementById("map_canvas")); 
                map.addControl(new GMapTypeControl()); 
                 map.setUIToDefault();
               geocoder = new GClientGeocoder();
             }
        initialize1();
        //initializeAll(); 
     </script>
     <asp:Literal runat="server" ID="BottomScriptLiteral"></asp:Literal>
    </form>
</body>
</html>
