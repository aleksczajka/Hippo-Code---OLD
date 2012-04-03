<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MapAlert.aspx.cs" Inherits="MapAlert" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <script src="http://maps.google.com/maps?file=api&amp;v=2&amp;sensor=false&amp;key=ABQIAAAAjjoxQtYNtdn3Tc17U5-jbBR2Kk_H7gXZZZniNQ8L14X1BLzkNhQjgZq1k-Pxm8FxVhUy3rfc6L9O4g" type="text/javascript"></script>
    <link href="~/StyleSheet.css" type="text/css" rel="stylesheet" />
</head>
<body onload="initialize();" onunload="GUnload()" style="background-color: #333333; scrollbar-base-color: #666666; scrollbar-shadow-color: #666666; scrollbar-highlight-color: #666666; scrollbar-darkshadow-color: #666666; scrollbar-3dlight-color: #666666; scrollbar-track-color: #999999; scrollbar-arrow-color: #363636;">
    <form id="form1" runat="server">
<script type="text/javascript">
    var map = null;
    var geocoder = null;

    function initialize() {
      if (GBrowserIsCompatible()) 
      {
        map = new GMap2(document.getElementById("map_canvas"));
        map.addControl(new GSmallMapControl());
        map.addControl(new GMapTypeControl());

        if(map != null && map != undefined){
            //map.setCenter(new GLatLng(37.4419, -122.1419), 13);
            map.setUIToDefault();
            geocoder = new GClientGeocoder();
            var address = getCookie('addressParameter')
            showAddress(address, 0, address, 1);
        }
      }
    }
    
    //From: http://code.google.com/apis/maps/documentation/javascript/v2/overlays.html#Markers
    // Create a base icon for all of our markers that specifies the
    // shadow, icon dimensions, etc.
    var baseIcon = new GIcon(G_DEFAULT_ICON);
    baseIcon.shadow = "http://www.google.com/mapfiles/shadow50.png";
    baseIcon.iconSize = new GSize(20, 34);
    baseIcon.shadowSize = new GSize(37, 34);
    baseIcon.iconAnchor = new GPoint(9, 34);
    baseIcon.infoWindowAnchor = new GPoint(9, 2);

    // Creates a marker whose info window displays the letter corresponding
    // to the given index.
    function createMarker(point, index) {
      // Create a lettered icon for this point using our icon class
      var letter = String.fromCharCode("A".charCodeAt(0) + index);
      var letteredIcon = new GIcon(baseIcon);
      letteredIcon.image = "http://www.google.com/mapfiles/marker" + letter + ".png";

      // Set up our GMarkerOptions object
      markerOptions = { icon:letteredIcon };
      var marker = new GMarker(point, markerOptions);

      GEvent.addListener(marker, "click", function() {
        marker.openInfoWindowHtml("Marker <b>" + letter + "</b>");
      });
      return marker;
    }

function getCookie(c_name)
            {
                if (document.cookie.length>0)
                  {
                  c_start=document.cookie.indexOf(c_name + "=");
                  if (c_start!=-1)
                    {
                    c_start=c_start + c_name.length+1;
                    c_end=document.cookie.indexOf(";",c_start);
                    if (c_end==-1) c_end=document.cookie.length;
                    return unescape(document.cookie.substring(c_start,c_end));
                    }
                  }
                return "";
            }
            function GoToDirections()
            {
                var ToAddress = getCookie('addressParameter').replace(/@&/g, ' ').replace(/ /g, '+');
                var FromAddress = document.form1.FromInput.value.replace(/,/g, ' ').replace(/ /g, '+');

                window.open('http://mapof.it/'+FromAddress+'/'+ToAddress);
            }
    function showAddress(address, tryCount, wholeAddress, repeatCount) {
       
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
                alert(address.replace(/@&/g, " ") + tryCount+" not found. Try correcting this address.");
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
                      showAddress(nextAddress, tryCount + 1, wholeAddress, 2);
                    
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
                      showAddress(nextAddress, tryCount + 1, wholeAddress, 1);
                  }
              }
            } 
            else {
              map.setCenter(point, 15);
              //var marker = createMarker(point, 1);
              var marker = new GMarker(point, markerOptions);
              map.addOverlay(marker);
              marker.openInfoWindowHtml("<div class='AddLink'>"+getCookie('addressParameterName') +'<br/>'+ address.replace(/@&/g, "<br/>")+"</div>");
            }
          }
        );
      }
    }
function GetRadWindow() 
                { 
                  var oWindow = null; 
                  if (window.radWindow) 
                     oWindow = window.radWindow; 
                  else if (window.frameElement.radWindow) 
                     oWindow = window.frameElement.radWindow; 
                  return oWindow; 
                }   
                function CloseWindow()
                {
                
                    var oWindow = GetRadWindow(); 
                    oWindow.Close(); 
                }
              
                function Search() 
                { 
                    CloseWindow();
                } 
    </script>
        <table width="700px" height="490px" cellpadding="0" cellspacing="0">
            <tr>
                <td style="padding-bottom: 10px;">
                    <label class="AddWhiteLink">Directions From: </label><input onkeypress="{if (event.keyCode==13)GoToDirections();}" type="text" id="FromInput" maxlength="100" />
                    <a style="text-decoration: underline;" class="AddLink" onclick="GoToDirections();">go</a>
                </td>
                <td align="right" style="padding-bottom: 7px; margin: 0;">
                    <a class="AddLink" onclick="window.print();" style="text-decoration: underline;">print</a>
                    &nbsp;&nbsp;&nbsp;
                    <a class="AddLink" onclick="Search();" style="text-decoration: underline;">close</a>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="map_canvas" style="width: 700px; height: 470px; display: block;"></div>
                </td>
            </tr>
        </table>
   
    </form>
</body>
</html>
