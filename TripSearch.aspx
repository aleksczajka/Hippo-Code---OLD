<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ValidateRequest="false" 
AutoEventWireup="true" CodeFile="TripSearch.aspx.cs" EnableEventValidation="false" 
Inherits="TripSearch" Title="Search local trips sights landmarks adventures | HippoHappenings" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<%@ Register Src="~/Controls/TripSearchElements.ascx" TagName="SearchElements" TagPrefix="ctrl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register Src="~/Controls/Ads.ascx" TagName="Ads" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/LocDetect.ascx" TagName="LocDetect" TagPrefix="ctrl" %>
<%@ Register TagName="BlueButton" TagPrefix="ctrl" Src="~/Controls/BlueButton.ascx" %>
<%@ Register TagName="GreenButton" TagPrefix="ctrl" Src="~/Controls/GreenButton.ascx" %>
<%@ Register TagName="SmallButton" TagPrefix="ctrl" Src="~/Controls/SmallButton.ascx" %>
<%@ Register TagName="Pager" TagPrefix="ctrl" Src="~/Controls/Pager.ascx" %>
<%@ MasterType TypeName="MasterPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:Literal runat="server" ID="ScriptLiteral"></asp:Literal>
    <script type="text/javascript">
        function FillTime()
        {
            var theTime = getCookie('SetTime');
            
            if(theTime != null && theTime != undefined)
            {
                fillDrop(theTime);
            }
        }
        
        function OpenAdvanced()
        {
            var d = document.getElementById('AdvancedDiv');
            var c = document.getElementById('ArrowDiv');
            
            if(d.style.display == 'block')
            {
                d.style.display = 'none';
                c.innerHTML= '&rArr;';
            }
            else
            {
                d.style.display = 'block';
                c.innerHTML = '&dArr;';
            }
        }
        
        function OpenCategories()
        {
            var d = document.getElementById('FilterDiv');
            var c = document.getElementById('OpenDiv');
            var nameDiv = document.getElementById('NameDiv');
            
            if(d.style.display == 'block')
            {
                d.style.display = 'none';
                c.innerHTML= '&lArr; expand &rArr;';
                nameDiv.style['border'] = 'solid 1px #c9c4c4';
                nameDiv.style['padding'] = '2px';
                
                //setCookie("Collapsed",true,1);
            }
            else
            {
                d.style.display = 'block';
                c.innerHTML = '&rArr; collapse &lArr;';
                nameDiv.style['border'] = '';
                nameDiv.style['padding'] = '3px';
            }
        }
        
//        function CheckCollapsed()
//        {
//            var address = getCookie('Collapsed');
//            
//            if(address != null && address != undefined)
//            {
//                if(address == "true")
//                {
//                    var d = document.getElementById('FilterDiv');
//                    var c = document.getElementById('OpenDiv');
//                    var nameDiv = document.getElementById('NameDiv');
//                    d.style.display = 'none';
//                    c.innerHTML= '&lArr; expand &rArr;';
//                    nameDiv.style['border'] = 'solid 1px #c9c4c4';
//                    nameDiv.style['padding'] = '2px';
//                }
//            }
//        }
       
        
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
                marker.openInfoWindowHtml("<div class='Text12' style=\"width: 200px; font-weight: normal;\">"+theName+"</div>");
              });
              gmarkers[index] = marker; 
          }
          else
          {
              var letter = String.fromCharCode("A".charCodeAt(0) + index);
              
              letteredIcon = new GIcon(baseIcon);
              letteredIcon.image = "http://www.google.com/mapfiles/marker" + letter + ".png";
              
              // Set up our GMarkerOptions object
              markerOptions = { icon:letteredIcon, zIndexProcess:zIndexStr};
              var marker = new GMarker(point, markerOptions);
              
              if(gmarkers[index] != undefined && gmarkers[index] != null)
              {
                  GEvent.clearInstanceListeners(gmarkers[index]) 
                  map.removeOverlay(gmarkers[index]);
                     
                  GEvent.addListener(marker, "click", function() {
                    marker.openInfoWindowHtml("<div class='Text12' style=\"width: 200px;\">"+theName+"</div>");
                  });
                    gmarkers[index] = marker;
              }
              else
              {
                  GEvent.addListener(marker, "click", function() {
                    marker.openInfoWindowHtml("<div class='Text12' style=\"width: 200px;\">"+theName+"</div>");
                  });
                    gmarkers[index] = marker;
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
            marker.openInfoWindowHtml("<div class='Text12' style=\"width: 200px;\">"+theName+"</div>");
          });
          gmarkers[index] = marker;
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
              var marker = createMarker(point, theNumber, theName +"<br/>at "+theVenue+"<br/>"+address.replace(/@&/g, " "), theChoice);
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
              var marker = createMarker(point, theNumber, theName +"<br/>at "+theVenue+"<br/>"+address.replace(/@&/g, " "), theChoice);
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
        
        function GoToThis(goHere)
        {
            window.location = goHere;
        }
      
        function Search() 
        { 
            CloseWindow();
        }
        
    </script>
    
 <asp:Label runat="server" ID="ErrorLabel" ForeColor="red"></asp:Label>  
            
            <ctrl:LocDetect runat="server" />
            <div class="topDiv Text12" style="width: 860px;">
                <div style="clear: both;">
                    <div style="float: left;"><img src="NewImages/BackLeftTopCorner.png" /></div>
                    <div style=" height: 15px;width: 828px;float: left; background: url('NewImages/BackBorder.png'); background-repeat: repeat-x; background-position: top;">
                        &nbsp;
                    </div>
                    <div style="float: left;"><img src="NewImages/BackRightTopCorner.png" /></div>
                </div>
                <div style="clear: both; background-color: White;"> 
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="15px" style="background-image: url('NewImages/BackLeftBorder.png'); background-repeat: repeat-y;"></td>
                            <td>
                                <div class="topDiv" style="width: 820px; background-color: White; font-size: 18px; padding-left: 10px;padding-bottom: 5px;">
                                <asp:Panel ID="Panel2" runat="server" DefaultButton="fakeSearchButton">
                                    <div class="topDiv" style="min-height: 560px;">
                                        <div style="margin-left: -8px;min-height: 560px;float: left; padding-right: 10px; margin-right: 10px; border-right: solid 1px #dedbdb; width: 365px;">
                                            <asp:Label runat="server" ID="EventTitle">Search Adventures</asp:Label>
                                            <div class="topDiv">
                                                <div style="display: none;"><asp:Button runat="server" OnClick="SearchButton_Click" ID="fakeSearchButton" />
                                                        </div>
                                                <div style="float: left;padding-left: 55px;">
                                                    <rad:RadTextBox BorderColor="#09718F" runat="server" EmptyMessage="Keywords" ID="KeywordsTextBox" Width="209px"></rad:RadTextBox>
                                                </div>
                                                <div style="float: left; padding-left: 4px;">
                                                    <ctrl:SmallButton ID="SearchButton" runat="server" />
                                                </div>
                                            </div>
                                            <div class="topDiv">
                                                <asp:Label runat="server" ID="MessageLabel" CssClass="AddLink" ForeColor="Red"></asp:Label>
                                                <h1 class="SideColumn"><a onclick="OpenAdvanced();"><span style="text-decoration: underline;">Advanced <span id="ArrowDiv">&rArr;</span></span></a></h1>
                                                <div id="AdvancedDiv" style="display: none;padding-bottom: 20px;">
                                                    <h2>Details</h2>
                                                    <div style="padding-bottom: 10px; border: solid 1px #c9c4c4;">
                                                        <div class="topDiv Text12">
                                                            <div style="float: left; padding:5px;clear: both;">
                                                            
                                                                <table>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <h2>Time Frame</h2>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <div id="Div2" onclick="OpenCloseDiv();"  runat="server" style="cursor: pointer;text-decoration: underline;float: left;background-color: White;">
                                                                                When? &rArr;
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <input id="HiddenMode" type="hidden" runat="server" />
                                                                                    
                                                                                    <div style="display: none;" id="DataDiv"></div>
                                                                                     
                                                                                     <asp:Panel ID="Panel1" runat="server" Visible="false">

                                                                                </asp:Panel>
                                                                                    
                                                                                    <script type="text/javascript">function CloseDiv(){var theDiv=document.getElementById('DropDiv');theDiv.style.display='none'}function OpenCloseDiv(){var theDiv=document.getElementById('DropDiv');if(theDiv.style.display=='none')theDiv.style.display='block';else theDiv.style.display='none'}function fillDrop(theText){var theDiv=document.getElementById('ctl00_ContentPlaceHolder1_TimeFrameDiv');theDiv.innerHTML=theText;setCookie("SetTime",theText,1);CloseDiv()}
                                                                                    
                                                                                    function ondayselected(sender,e)
                                                                                    {
                                                                                        fillDrop('Next '+sender._originalText+' Days');
                                                                                    }
                                                                                    
                                                                                    function ondateselected(sender,e){if(e.getDate()!=null){fillDrop(e.getDate().toDateString())}}</script>
                                                                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                                                            <ContentTemplate>
                                                                                                <asp:LinkButton runat="server" ID="doTime" OnClick="setTimeSession"></asp:LinkButton>
                                                                                            </ContentTemplate>
                                                                                        </asp:UpdatePanel>
                                                                                    <div style="position: relative;">
                                                                                    <div id="DropDiv" class="TextNormal topDiv" style="height: 221px;outline: medium outset #09718F;background-color: White;padding: 10px;top: -100px; right: 70px;position: absolute;display: none; width: 110px;border: solid 1px #09718F;">
                                                                                                <div style="float: right;" class="NavyLink" onclick="CloseDiv()">X</div>
                                                                                                <div style="float: left; padding-bottom: 5px;"><a class="NavyLink" onclick="fillDrop('Today');">Today</a></div>
                                                                                                <div style="clear: both; padding-bottom: 5px;"><a class="NavyLink" onclick="fillDrop('Tomorrow');">Tomorrow</a></div>
                                                                                                <div style="clear: both; padding-bottom: 5px;"><a class="NavyLink" onclick="fillDrop('This Weekend');">This Weekend</a></div>
                                                                                                <div style="clear: both; padding-bottom: 5px;"><a class="NavyLink" onclick="fillDrop('This Week');">This Week</a></div>
                                                                                                <div style="clear: both; padding-bottom: 5px;"><a class="NavyLink" onclick="fillDrop('Next Week');">Next Week</a></div>
                                                                                                <div style="clear: both; padding-bottom: 5px;"><a class="NavyLink" onclick="fillDrop('This Month');">This Month</a></div><%--
                                                                                                <div style="clear: both; padding-bottom: 5px;"><a class="NavyLink" onclick="fillDrop('All Past Events');">All Past Events</a></div>--%>
                                                                                                <div style="clear: both; padding-bottom: 5px;"><a class="NavyLink" onclick="fillDrop('All Future Events');">All Future Trips</a></div>
                                                                                                <div style="clear: both; padding-bottom: 5px;">
                                                                                                    <div style=" padding-top: 5px;padding-bottom: 5px !important;" >Next 
                                                                                                        <rad:RadComboBox Width="50px" Skin="WebBlue" runat="server" OnClientSelectedIndexChanged="ondayselected" ID="TimeRad">
                                                                                                        <Items>
                                                                                                            <rad:RadComboBoxItem Value="1" Text="1" />
                                                                                                            <rad:RadComboBoxItem Value="2" Text="2"/>
                                                                                                            <rad:RadComboBoxItem Value="3" Text="3"/>
                                                                                                            <rad:RadComboBoxItem Value="4" Text="4"/>
                                                                                                            <rad:RadComboBoxItem Value="5" Text="5"/>
                                                                                                            <rad:RadComboBoxItem Value="6" Text="6"/>
                                                                                                            <rad:RadComboBoxItem Value="7" Text="7"/>
                                                                                                            <rad:RadComboBoxItem Value="8" Text="8"/>
                                                                                                            <rad:RadComboBoxItem Value="9" Text="9"/>
                                                                                                            <rad:RadComboBoxItem Value="10" Text="10"/>
                                                                                                            <rad:RadComboBoxItem Value="11" Text="11"/>
                                                                                                            <rad:RadComboBoxItem Value="12" Text="12"/>
                                                                                                            <rad:RadComboBoxItem Value="13" Text="13"/>
                                                                                                            <rad:RadComboBoxItem Value="14" Text="14"/>
                                                                                                            <rad:RadComboBoxItem Value="15" Text="15"/>
                                                                                                            <rad:RadComboBoxItem Value="16" Text="16"/>
                                                                                                            <rad:RadComboBoxItem Value="17" Text="17"/>
                                                                                                            <rad:RadComboBoxItem Value="18" Text="18"/>
                                                                                                            <rad:RadComboBoxItem Value="19" Text="19"/>
                                                                                                            <rad:RadComboBoxItem Value="20" Text="20"/>
                                                                                                            <rad:RadComboBoxItem Value="21" Text="21"/>
                                                                                                            <rad:RadComboBoxItem Value="22" Text="22"/>
                                                                                                            <rad:RadComboBoxItem Value="23" Text="23"/>
                                                                                                            <rad:RadComboBoxItem Value="24" Text="24"/>
                                                                                                            <rad:RadComboBoxItem Value="25" Text="25"/>
                                                                                                            <rad:RadComboBoxItem Value="26" Text="26"/>
                                                                                                            <rad:RadComboBoxItem Value="27" Text="27"/>
                                                                                                            <rad:RadComboBoxItem Value="28" Text="28"/>
                                                                                                            <rad:RadComboBoxItem Value="29" Text="29"/>
                                                                                                            <rad:RadComboBoxItem Value="30" Text="30"/>
                                                                                                            <rad:RadComboBoxItem Value="31" Text="31" />
                                                                                                        </Items>
                                                                                                        </rad:RadComboBox> Days
                                                                                                    </div>
                                                                                                </div>
                                                                                                <div style="clear: both;">
                                                                                                    <div class="SpecialCalendar">
                                                                                                       <rad:RadDatePicker Skin="WebBlue" Width="100px" ClientEvents-OnDateSelected="ondateselected" runat="server" ID="RadDatePicker2">
                                                                                                            <DateInput ID="DateInput2" runat="server" EmptyMessage="Select Date"></DateInput>
                                                                                                        </rad:RadDatePicker>
                                                                                                    </div>
                                                                                                </div>
                                                                                         </div>
                                                                                        <div class="TextNormal" runat="server" onblur="CloseDiv();" onclick="OpenCloseDiv();" 
                                                                                        style="cursor: pointer; font-family: Arial; font-size: 12px;" id="Div3">
                                                                                        
                                                                                                    <div runat="server" class="NavyLink" id="TimeFrameDiv">
                                                                                                        
                                                                                                    </div>
                                                                                                
                                                                                        </div>
                                                                                        <script type="text/javascript">
                                                                                            FillTime();
                                                                                        </script>
                                                                                     </div>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                   
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Earliest departure time
                                                                        </td>
                                                                        <td>
                                                                            <rad:RadTimePicker runat="server" ID="DepartureTimePicker"></rad:RadTimePicker>
                                                                        </td>
                                                                        
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Latest return time
                                                                        </td>
                                                                        <td>
                                                                            <rad:RadTimePicker runat="server" ID="BackTimePicker"></rad:RadTimePicker>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Maximum trip duration</td>
                                                                        <td><asp:TextBox runat="server" Width="83px" ID="TripDuration"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <asp:Label runat="server" ID="DurationErrorLabel" ForeColor="red"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <h2>Highest Price</h2>
                                                                        </td>
                                                                        <td>
                                                                                <script type="text/javascript">
                                                                                    function parsePriceCheck()
                                                                                    {
                                                                                        var theInput = document.getElementById('ctl00_ContentPlaceHolder1_HighestPriceInput');
                                                                                        
                                                                                        return !isNaN(parseFloat(theInput.value.replace('$', '')));
                                                                                       
                                                                                    }
                                                                                </script>
                                                                                    <div class="topDiv TextNormal">
                                                                                        
                                                                                        <div style="border: 0; float: left;">
                                                                                            $<input runat="server" id="HighestPriceInput" type="text" 
                                                                                                style="margin-top: 3px;width: 24px; border: solid 1px #09718F; height: 14px;" class="Text12" />&nbsp;
                                                                                        </div>
                                                                                    </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2" style="padding-top: 5px;">
                                                                            <div>
                                                                                <h2>Means of Getting Around</h2>
                                                                                <asp:CheckBoxList CellPadding="0" CellSpacing="0" runat="server" ID="MeansCheckList" RepeatDirection="horizontal">
                                                                                    <asp:ListItem Text="Car" Value="1"></asp:ListItem>
                                                                                    <asp:ListItem Text="Walking" Value="2"></asp:ListItem>
                                                                                    <asp:ListItem Text="Hiking" Value="3"></asp:ListItem>
                                                                                    <asp:ListItem Text="Biking" Value="4"></asp:ListItem>
                                                                                    <asp:ListItem Text="Flying" Value="5" ></asp:ListItem>
                                                                                </asp:CheckBoxList>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                
                                                            </div>
                                                        </div>
                                                        <div class="topDiv">
                                                             <div style="float: right; padding-right: 7px;">
                                                                <ctrl:SmallButton ID="SmallButton2" runat="server" />
                                                             </div>
                                                        </div>
                                                    </div>
                                                    
                                                    <h2>Location</h2>
                                                    <div style="padding-bottom: 10px; border: solid 1px #c9c4c4;">
                                                        <div class="topDiv">
                                                            <div style="float: left; width: 150px; padding:5px;clear: both;">
                                                                    <rad:RadComboBox Skin="WebBlue" runat="server" OnSelectedIndexChanged="ChangeState" 
                                                                        DataSourceID="SqlDataSourceCountry" DataTextField="country_name" 
                                                                        DataValueField="country_id" Width="150px" AutoPostBack="true"  ID="CountryDropDown">
                                                                        </rad:RadComboBox>
                                                                
                                                                    <asp:SqlDataSource runat="server" ID="SqlDataSourceCountry" SelectCommand="SELECT * FROM Countries"
                                                                                    ConnectionString="<%$ ConnectionStrings:Connection %>"></asp:SqlDataSource>
                                                        
                                                            </div>    
                                                            <div style="float: right;">
                                                            <div style="padding-top: 15px; padding-right: 5px;">
                                                                <table cellspacing="0"  width="100%" >
                                                                    <tr>
                                                                        <td align="left">
                                                                            <asp:Panel runat="server" ID="StateTextBoxPanel">
                                                                                <rad:RadTextBox EmptyMessage="State" Skin="WebBlue"  ID="StateTextBox" Width="100" runat="server"></rad:RadTextBox>
                                                                            </asp:Panel>
                                                                            <asp:Panel runat="server" ID="StateDropDownPanel" Visible="false">
                                                                                <div style="width: 100px;">
                                                                                <rad:RadComboBox Text="State" EnableItemCaching="true" EmptyMessage="State" runat="server" 
                                                                                Width="100px" Skin="WebBlue" ID="StateDropDown"></rad:RadComboBox>
                                                                                </div>
                                                                            </asp:Panel>
                                                                        </td>
                                                                        <td align="right">
                                                                                <rad:RadTextBox EmptyMessage="City" Skin="WebBlue" Width="100" 
                                                                                runat="server" ID="CityTextBox"></rad:RadTextBox>
                                                                        </td>
                                                                    </tr>
                                                                    
                                                                </table>
                                                                <div align="center" style="padding-top: 5px; padding-bottom: 5px;">
                                                                <img src="NewImages/or.png" />
                                                                </div>
                                                                <table cellspacing="0" style=" padding-bottom: 10px;"  width="100%" >
                                                                    <tr>
                                                                        <td>
                                                                            <rad:RadTextBox EmptyMessage="Zip(5 digits)" Skin="WebBlue"  ID="ZipTextBox" Width="70" runat="server"></rad:RadTextBox>
                                                                        </td>
                                                                        <asp:Panel runat="server" ID="RadiusDropPanel">
                                                                        <td width="140px">
                                                                            <rad:RadComboBox WIDTH="140px" Skin="WebBlue"  runat="server" ID="RadiusDropDown">
                                                                                <Items>
                                                                                    <rad:RadComboBoxItem Value="0" Text="Just Zip Code (0 miles)" />
                                                                                    <rad:RadComboBoxItem Value="1" Text="1 mile" />
                                                                                    <rad:RadComboBoxItem Value="5" Text="5 miles" />
                                                                                    <rad:RadComboBoxItem Value="10" Text="10 miles" />
                                                                                    <rad:RadComboBoxItem Value="15" Text="15 miles" />
                                                                                    <rad:RadComboBoxItem Value="30" Text="30 miles" />
                                                                                    <rad:RadComboBoxItem Value="50" Text="50 miles" />
                                                                                </Items>
                                                                            </rad:RadComboBox>
                                                                        </td>
                                                                        </asp:Panel>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                            </div>
                                                            
                                                        </div>
                                                        <div class="topDiv">
                                                             <div style="float: right; padding-right: 7px;">
                                                                <ctrl:SmallButton ID="SmallButton1" runat="server" />
                                                             </div>
                                                        </div>
                                                    </div>
                                                    
                                                    
                                                    
                                                </div>
                                                <div style="margin-left: -13px;">
                                                    <asp:Panel runat="server" ID="MapPanel" Visible="false">
                                                        <div style="width: 380px; height: 465px; padding-top: 3px;padding-left: 5px;">
                                                            <div id="map_canvas" style="border: solid 1px #535252;width: 380px; height: 465px; display: block;"></div>
                                                        </div> 
                                                    </asp:Panel> 
                                                </div>
                                            </div>
                                        </div>
                                        <div class="topDiv" style="float: left;width: 435px;">
                                            <%--<div class="topDiv" style="padding-bottom: 5px; clear: both;">
                                                <div style="float: left; padding-right: 4px;">
                                                    <ctrl:SmallButton ID="TodayButton" runat="server" />
                                                </div>
                                                <div style="float: left; padding-right: 4px;">
                                                    <ctrl:SmallButton ID="TomorrowButton" runat="server" />
                                                </div>
                                                <div style="float: left; padding-right: 4px;">
                                                    <ctrl:SmallButton ID="ThisWeekButton" runat="server" />
                                                </div>
                                                <div style="float: left; padding-right: 4px;">
                                                    <ctrl:SmallButton ID="ThisWeekendButton" runat="server" />
                                                </div>
                                                <div style="float: left; padding-right: 4px;">
                                                    <ctrl:SmallButton ID="ThisMonthButton" runat="server" />
                                                </div>
                                                <div style="float: left; padding-right: 4px;padding-bottom: 4px;">
                                                    <script type="text/javascript">
                                                        function parseCheck()
                                                        {
                                                            var theInput = document.getElementById('ctl00_ContentPlaceHolder1_NextDaysInput');
                                                            if(!isNaN(parseFloat(theInput.value)))
                                                            {
                                                                var d = document.getElementById('NextDaysInput'); 
                                                                return d.value != '';
                                                            }
                                                            else
                                                            {
                                                                return false;
                                                            }
                                                        }
                                                    </script>
                                                    <div align="center">
                                                        
                                                            <div class="topDiv" style="clear: both;">
                                                                <img style="float: left;" src="NewImages/SmallButtonLeft.png" height="24px" />
                                                                <div style="font-size: 12px; text-decoration: none; padding-left: 5px; padding-right: 5px;height: 24px;float: left;background: url('NewImages/SmallButtonPixel.png'); background-repeat: repeat-x;">
                                                                    <div style="border: 0; padding-top: 5px; float: left;" >
                                                                        <asp:ImageButton ID="ImageButton1" OnClick="SelectNextDays" OnClientClick="return parseCheck();" runat="server" ImageUrl="~/NewImages/NextText.png" />&nbsp;
                                                                    </div>
                                                                    <div style="border: 0; float: left;" >
                                                                        <input runat="server" id="NextDaysInput" type="text" 
                                                                            style="margin-top: 3px;width: 24px; border: solid 1px #3ed629; height: 14px;" class="Text12" />&nbsp;
                                                                    </div>
                                                                    <div style="border: 0; padding-top: 5px; float: left;" >
                                                                        <asp:ImageButton ID="ImageButton2" OnClick="SelectNextDays" OnClientClick="return parseCheck();"  runat="server" ImageUrl="~/NewImages/DaysText.png" />&nbsp;
                                                                    </div>
                                                                </div>
                                                                <img style="float: left;" src="NewImages/SmallButtonRight.png" height="24px" />
                                                            </div>
                                                        </div>
                                                </div>
                                                <div style="float: left;padding-bottom: 4px;">
                                                    <script language = "Javascript">
                                                        /**
                                                         * DHTML date validation script. Courtesy of SmartWebby.com (http://www.smartwebby.com/dhtml/)
                                                         */
                                                        // Declaring valid date character, minimum year and maximum year
                                                        var dtCh= "/";
                                                        var minYear=1900;
                                                        var maxYear=2100;

                                                        function isInteger(s){
	                                                        var i;
                                                            for (i = 0; i < s.length; i++){   
                                                                // Check that current character is number.
                                                                var c = s.charAt(i);
                                                                if (((c < "0") || (c > "9"))) return false;
                                                            }
                                                            // All characters are numbers.
                                                            return true;
                                                        }

                                                        function stripCharsInBag(s, bag){
	                                                        var i;
                                                            var returnString = "";
                                                            // Search through string's characters one by one.
                                                            // If character is not in bag, append to returnString.
                                                            for (i = 0; i < s.length; i++){   
                                                                var c = s.charAt(i);
                                                                if (bag.indexOf(c) == -1) returnString += c;
                                                            }
                                                            return returnString;
                                                        }

                                                        function daysInFebruary (year){
	                                                        // February has 29 days in any year evenly divisible by four,
                                                            // EXCEPT for centurial years which are not also divisible by 400.
                                                            return (((year % 4 == 0) && ( (!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28 );
                                                        }
                                                        function DaysArray(n) {
	                                                        for (var i = 1; i <= n; i++) {
		                                                        this[i] = 31
		                                                        if (i==4 || i==6 || i==9 || i==11) {this[i] = 30}
		                                                        if (i==2) {this[i] = 29}
                                                           } 
                                                           return this
                                                        }

                                                        function isDate(dtStr){
	                                                        var daysInMonth = DaysArray(12)
	                                                        var pos1=dtStr.indexOf(dtCh)
	                                                        var pos2=dtStr.indexOf(dtCh,pos1+1)
	                                                        var strMonth=dtStr.substring(0,pos1)
	                                                        var strDay=dtStr.substring(pos1+1,pos2)
	                                                        var strYear=dtStr.substring(pos2+1)
	                                                        strYr=strYear
	                                                        if (strDay.charAt(0)=="0" && strDay.length>1) strDay=strDay.substring(1)
	                                                        if (strMonth.charAt(0)=="0" && strMonth.length>1) strMonth=strMonth.substring(1)
	                                                        for (var i = 1; i <= 3; i++) {
		                                                        if (strYr.charAt(0)=="0" && strYr.length>1) strYr=strYr.substring(1)
	                                                        }
	                                                        month=parseInt(strMonth)
	                                                        day=parseInt(strDay)
	                                                        year=parseInt(strYr)
	                                                        if (pos1==-1 || pos2==-1){
		                                                        return false
	                                                        }
	                                                        if (strMonth.length<1 || month<1 || month>12){
		                                                        return false
	                                                        }
	                                                        if (strDay.length<1 || day<1 || day>31 || (month==2 && day>daysInFebruary(year)) || day > daysInMonth[month]){
		                                                        return false
	                                                        }
	                                                        if (strYear.length != 4 || year==0 || year<minYear || year>maxYear){
		                                                        return false
	                                                        }
	                                                        if (dtStr.indexOf(dtCh,pos2+1)!=-1 || isInteger(stripCharsInBag(dtStr, dtCh))==false){
		                                                        return false
	                                                        }
                                                        return true
                                                        }

                                                        </script>
                                                    <script type="text/javascript">
                                                            function dateCheck()
                                                            {
                                                                var d = document.getElementById('ctl00_ContentPlaceHolder1_RadDatePicker2_dateInput_text'); 
                                                                
                                                                if(d.value != 'Date')
                                                                {
                                                                    return isDate(d.value);
                                                                }
                                                                else
                                                                {
                                                                    return false;
                                                                }
                                                                
                                                            }
                                                        </script>
                                                    <div runat="server"
                                                        style="cursor: pointer;width: 124px; font-family: Arial; font-size: 12px;" id="Div1">
                                                        <div align="center">
                                                            <div class="topDiv" style="clear: both;">
                                                                <img style="float: left;" src="NewImages/SmallButtonLeft.png" height="24px" />
                                                                <div style="font-size: 12px; text-decoration: none; padding-left: 4px; padding-right: 4px;height: 24px;float: left;background: url('NewImages/SmallButtonPixel.png'); background-repeat: repeat-x;">
                                                                    <div runat="server" style="margin-top: 2px; cursor: pointer;width: 101px; height: 18px;" id="TimeFrameDiv">
                                                                        <div>
                                                                           <rad:RadDatePicker Width="80px" Skin="WebBlue" runat="server" ID="RadDatePicker2">
                                                                                <DateInput ID="DateInput1" runat="server" EmptyMessage="Date"></DateInput>
                                                                                
                                                                            </rad:RadDatePicker>&nbsp;&nbsp;
                                                                            <asp:LinkButton runat="server" OnClientClick="return dateCheck();" OnClick="SetDate" CssClass="NavyLink12UD" ID="SearchDateLinkButton" Text="<span style='color: #33a923;'>&#9654;</span>"></asp:LinkButton>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <img style="float: left;" src="NewImages/SmallButtonRight.png" height="24px" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                 </div>
                                                <div style="float: left;">
                                                        <script type="text/javascript">
                                                            function parsePriceCheck()
                                                            {
                                                                var theInput = document.getElementById('ctl00_ContentPlaceHolder1_HighestPriceInput');
                                                                
                                                                return !isNaN(parseFloat(theInput.value.replace('$', '')));
                                                               
                                                            }
                                                        </script>
                                                        <div align="center">
                                                            <div class="topDiv" style="clear: both;">
                                                                <img style="float: left;" src="NewImages/SmallButtonLeft.png" height="24px" />
                                                                <div style="font-size: 12px; text-decoration: none; padding-left: 4px; padding-right: 4px;height: 24px;float: left;background: url('NewImages/SmallButtonPixel.png'); background-repeat: repeat-x;">
                                                                    <div style="border: 0; padding-top: 5px; float: left;" >
                                                                        <asp:ImageButton ID="ImageButton3" OnClick="SelectHighestPrice" OnClientClick="return parsePriceCheck();" 
                                                                        runat="server" ImageUrl="~/NewImages/HighestPriceText.png" />&nbsp;
                                                                    </div>
                                                                    <div style="border: 0; float: left;">
                                                                        <input runat="server" id="HighestPriceInput" type="text" 
                                                                            style="margin-top: 3px;width: 24px; border: solid 1px #3ed629; height: 14px;" class="Text12" />&nbsp;
                                                                    </div>
                                                                </div>
                                                                <img style="float: left;" src="NewImages/SmallButtonRight.png" height="24px" />
                                                            </div>
                                                        </div>
                                                </div>
                                            </div>--%>
                                            <div style="padding-bottom: 20px;">
                                                <div id="NameDiv" class="topDiv" style=" border: solid 1px #c9c4c4; padding: 2px;">
                                                    <div style="float: left;"><h2>Filter your results on categories</h2></div>
                                                    <h2><div style="float: right;cursor: pointer; text-decoration: underline;" id="OpenDiv" onclick="OpenCategories();">&lArr; expand &rArr;</div></h2>
                                                </div>
                                                <div id="FilterDiv" style="display: none;">
                                                    <div style="border: solid 1px #c9c4c4; ">
                                                        <table>
                                                        <tr>
                                                            <td valign="top">
                                                                    <div class="FloatLeft" style="width: 150px;">
                                                                        <rad:RadTreeView  Width="150px" runat="server"  
                                                                        ID="CategoryTree" DataFieldID="ID" DataSourceID="SqlDataSource1" 
                                                                        DataFieldParentID="ParentID"  Skin="Vista"
                                                                        CheckBoxes="true">
                                                                        <DataBindings>
                                                                             <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                                             TextField="Name" Expanded="false" ValueField="ID" />
                                                                         </DataBindings>
                                                                        </rad:RadTreeView>
                                                                        <asp:SqlDataSource runat="server" ID="SqlDataSource1" 
                                                                            SelectCommand="SELECT * FROM TripCategories WHERE (ParentID IN (SELECT ID FROM TripCategories WHERE (Name < 'Fo') AND (ParentID IS NULL))) OR ((Name < 'Fo') AND (ParentID IS NULL)) ORDER BY Name ASC"
                                                                            ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                                        </asp:SqlDataSource>
                                                                    </div>
                                                                    
                                                               
                                                            </td>
                                                            <td valign="top">
                                                                <div style=" width: 130px;">
                                                                    <rad:RadTreeView Width="130px" runat="server"  
                                                                    ID="RadTreeView1" DataFieldID="ID" DataSourceID="SqlDataSource2" 
                                                                    DataFieldParentID="ParentID"  Skin="Vista"
                                                                    CheckBoxes="true">
                                                                    <DataBindings>
                                                                         <rad:RadTreeNodeBinding  Checkable="true" ValueField="ID" Checked="false" 
                                                                         TextField="Name" Expanded="false" />
                                                                     </DataBindings>
                                                                    </rad:RadTreeView>
                                                                    <asp:SqlDataSource runat="server" ID="SqlDataSource2" 
                                                                        SelectCommand="SELECT * FROM TripCategories WHERE (ParentID IN (SELECT ID FROM TripCategories WHERE (Name >= 'Fo' AND Name < 'Po') AND (ParentID IS NULL))) OR ((Name >= 'Fo' AND Name < 'Po') AND (ParentID IS NULL)) ORDER BY Name ASC"
                                                                        ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                                    </asp:SqlDataSource>
                                                                </div>
                                                            </td>
                                                            <td valign="top">
                                                                <div style=" width: 150px;">
                                                                    <rad:RadTreeView Width="150px" runat="server"  
                                                                    ID="RadTreeView2" DataFieldID="ID" DataSourceID="SqlDataSource3" 
                                                                    DataFieldParentID="ParentID"  Skin="Vista"
                                                                    CheckBoxes="true">
                                                                    <DataBindings>
                                                                         <rad:RadTreeNodeBinding  Checkable="true" ValueField="ID" Checked="false" 
                                                                         TextField="Name" Expanded="false" />
                                                                     </DataBindings>
                                                                    </rad:RadTreeView>
                                                                    <asp:SqlDataSource runat="server" ID="SqlDataSource3" 
                                                                        SelectCommand="SELECT * FROM TripCategories WHERE (ParentID IN (SELECT ID FROM TripCategories WHERE (Name >= 'Po') AND (ParentID IS NULL))) OR ((Name >= 'Po') AND (ParentID IS NULL)) ORDER BY Name ASC"
                                                                        ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                                    </asp:SqlDataSource>
                                                                </div>
                                                            </td>
                                                            
                                                        </tr>
                                                    </table>
                                                    </div>
                                                    <div class="topDiv">
                                                    <div style="float: right;padding-top: 5px; width: 170px;">
                                                            <%--<asp:LinkButton runat="server" CssClass="NavyLink12" Text="Filter" OnClick="FilterResults"></asp:LinkButton>--%>
                                                            <div style="float: left; padding-top: 7px; padding-right: 10px;">
                                                                <asp:LinkButton ID="LinkButton1" CssClass="NavyLink12" runat="server" Text="Clear Filter" OnClick="ClearFilter"></asp:LinkButton>
                                                            </div>
                                                            <div style="float: left;">
                                                                <ctrl:BlueButton runat="server" WIDTH="100px" ID="FilterButton" BUTTON_TEXT="Filter Results" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <%--<script type="text/javascript">
                                                    CheckCollapsed();
                                                </script>--%>
                                            </div>
                                            <div>
                                                <asp:Label runat="server" ID="TimeFrameLabel"></asp:Label>
                                                        <div style="float: left; width: 420px; ">
                                                            <asp:Label runat="server" CssClass="ResultsLabel" ID="ResultsLabel"></asp:Label>
                                                                     <div class="topDiv" style="width: 420px;padding-bottom: 10px;padding-top: 10px;"> 
                                                                             <div style="float: left;">  <asp:DropDownList Visible="false" runat="server" AutoPostBack="true"  
                                                                            OnSelectedIndexChanged="SortResults" ID="SortDropDown">
                                                                                <asp:ListItem Text="Sort By..." Value="-1"></asp:ListItem>
                                                                                <asp:ListItem Text="Price Up" Value="Price ASC"></asp:ListItem>
                                                                                <asp:ListItem Text="Price Down" Value="Price DESC"></asp:ListItem>
                                                                                <asp:ListItem Text="Duration Up" Value="Duration ASC"></asp:ListItem>
                                                                                <asp:ListItem Text="Duration Down" Value="Duration DESC"></asp:ListItem>
                                                                                <asp:ListItem Text="Title Up" Value="Header ASC"></asp:ListItem>
                                                                                <asp:ListItem Text="Title Down" Value="Header DESC"></asp:ListItem>
                                                                            </asp:DropDownList>   
                                                                            </div>
                                                                            <div style="padding-left: 20px; float: right">
                                                                                <asp:Label runat="server" ID="NumsLabel" CssClass="Green12Link"></asp:Label>
                                                                            </div>
                                                                     </div>   
                                                                    <div class="topDiv">
                                                                        <asp:Panel runat="server" ID="Panel7" Width="440px"><asp:Label runat="server" ForeColor="Red" ID="Label1"></asp:Label>
                                                                                <asp:Literal runat="server" ID="HelpUsLiteral"></asp:Literal>
                                                                              <ctrl:SearchElements Visible="false" runat="server" ID="EventSearchElements" />
                                                                              
                                                                         </asp:Panel>
                                                                    </div>
                                                                    </div>
                                                <asp:Panel runat="server" ID="NoResultsPanel" Visible="false">
                                                    <label>There are no trips matching your criteria, but, you can post them 
                                                    through our <a class="NavyLink12" href="enter-trip">Post an Adventure Page</a></label>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                        <div class="GooglySearch" align="center">
                                            <script type="text/javascript"><!--
                                                google_ad_client = "ca-pub-3961662776797219";
                                                /* Hippo Link Unit */
                                                google_ad_slot = "6130116935";
                                                google_ad_width = 728;
                                                google_ad_height = 15;
                                                //-->
                                                </script>
                                                <script type="text/javascript"
                                                src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
                                                </script>
                                        </div>
                                    </div>
                                   </asp:Panel> 
                                </div>
                            </td>
                            <td width="15px" style="background-image: url('NewImages/BackRightBorder.png'); background-repeat: repeat-y;"></td>
                        </tr>
                    </table> 
                </div>
                              
                <div style="clear: both;">
                    <div style="float: left;"><img src="NewImages/BackLeftBottomCorner.png" /></div>
                    <div style="width: 830px;float: left; background: url('NewImages/BackBottomBorder.png'); background-repeat: repeat-x; background-position: top;">
                        &nbsp;
                    </div>
                    <div style="float: left;"><img src="NewImages/BackRightBottomCorner.png" /></div>
                </div>
           </div>
           <%--<div class="topDiv" style="width: 847px;clear: both;margin-left: 7px;position: relative;top: -123px;">
                                        <div style="clear: both;" class="topDiv">
                                            <div style="float: left;width: 6px;"><img src="NewImages/EventFooterTopLeft.png" /></div>
                                            <div style=" height: 6px;width: 835px;float: left; background: url('NewImages/EventFooterTop.png'); background-repeat: repeat-x; background-position: top;">
                                                &nbsp;
                                            </div>
                                            <div style="float: left;width: 6px;"><img src="NewImages/EventFooterTopRight.png" /></div>
                                        </div>
                                        <div class="topDiv" style="clear: both; background-color: #ebe7e7;"> 
                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td width="6px" style="background-image: url('NewImages/EventFooterLeft.png'); background-repeat: repeat-y;"></td>
                                                    <td>
                                                        <div class="Text12" style="width: 835px; background-color: #ebe7e7;">
                                                            <div class="ContentFooter">
                                                                <b>Hippo Adventure Tips</b>
                                                                <ol>
                                                                    <li>
                                                                        Get to know your city, share a scavenger hunt or promote your favorite hike.
                                                                    </li>
                                                                    <li>
                                                                        Earn points toward being featured on our home page by posting trips. 
                                                                    </li>
                                                                    <li>
                                                                        Post, Share, Text, Email, Discuss. 
                                                                    </li>
                                                                    <li>                                                                        Feature an adventure for as little as $1/day.                                                                     </li>
                                                                </ol>
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
                                    </div>--%>
<asp:Literal runat="server" ID="TopLiteral" Text="<script type='text/javascript'>function initialize1(){map = null;}</script>"></asp:Literal>
     <script type="text/javascript">
     function initMap()
         {
            if (GBrowserIsCompatible())
             { 
                map = new GMap2(document.getElementById("map_canvas")); 
                map.setUIToDefault(); 
                geocoder = new GClientGeocoder(); 
                gmarkers = [10]; 
               }
         }
        initialize1();
        //initializeAll(); 
     </script>
     <asp:Literal runat="server" ID="BottomScriptLiteral"></asp:Literal>
</asp:Content>

