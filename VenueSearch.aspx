<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ValidateRequest="false" 
AutoEventWireup="true" CodeFile="VenueSearch.aspx.cs" EnableEventValidation="false" 
Inherits="VenueSearch" Title="Search locales, venues, shops and restaurants | HippoHappenings" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<%@ Register Src="~/Controls/VenueSearchElements.ascx" TagName="SearchElements" TagPrefix="ctrl" %>
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
              var marker = createMarker(point, theNumber, theName +"<br/>at "+address.replace(/@&/g, " "), theChoice);
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
                                <div class="topDiv" style="width: 820px; background-color: White; font-size: 18px; padding-left: 10px; padding-bottom: 5px;">
                                
                                    <div class="topDiv" style="min-height: 600px;">
                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="fakeSearchButton">
                                        <div style="margin-left: -8px;min-height: 600px;float: left; padding-right: 10px; margin-right: 10px; border-right: solid 1px #dedbdb; width: 365px;">
                                            <asp:Label runat="server" ID="EventTitle">Search Locales</asp:Label>
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
                                                <h1 class="SideColumn"><a onclick="OpenAdvanced();"><span style="text-decoration: underline;">Change Location <span id="ArrowDiv">&rArr;</span></span></a></h1>
                                                <div id="AdvancedDiv" style="display: none; padding-bottom: 20px;">
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
                                                                            <rad:RadComboBox Width="140px" Skin="WebBlue"  runat="server" ID="RadiusDropDown">
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
                                        </asp:Panel>
                                        <div class="topDiv" style="float: left;width: 435px;">
                                        
                                            <div>
                                                <asp:Label runat="server" ID="TimeFrameLabel"></asp:Label>
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
                                                                                            <div class="FloatLeft">
                                                                                                <rad:RadTreeView  Width="142px" runat="server"  
                                                                                                ID="CategoryTree" DataFieldID="ID" DataSourceID="SqlDataSource1" 
                                                                                                DataFieldParentID="ParentID"  Skin="Vista"
                                                                                                CheckBoxes="true">
                                                                                                <DataBindings>
                                                                                                     <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                                                                     TextField="Name" Expanded="false" ValueField="ID" />
                                                                                                 </DataBindings>
                                                                                                </rad:RadTreeView>
                                                                                                <asp:SqlDataSource runat="server" ID="SqlDataSource1" 
                                                                                                    SelectCommand="SELECT * FROM VenueCategories WHERE LIVE = 'True' AND ((ParentID IN (SELECT ID FROM VenueCategories WHERE (Name < 'Li') AND (ParentID IS NULL))) OR ((Name < 'Li') AND (ParentID IS NULL))) ORDER BY Name ASC"
                                                                                                    ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                                                                </asp:SqlDataSource>
                                                                                            </div>
                                                                                            
                                                                                       
                                                                                    </td>
                                                                                    <td valign="top">
                                                                                        <div style=" width: 140px;">
                                                                                            <rad:RadTreeView Width="140px" runat="server"  
                                                                                            ID="RadTreeView1" DataFieldID="ID" DataSourceID="SqlDataSource2" 
                                                                                            DataFieldParentID="ParentID"  Skin="Vista"
                                                                                            CheckBoxes="true">
                                                                                            <DataBindings>
                                                                                                 <rad:RadTreeNodeBinding  Checkable="true" ValueField="ID" Checked="false" 
                                                                                                 TextField="Name" Expanded="false" />
                                                                                             </DataBindings>
                                                                                            </rad:RadTreeView>
                                                                                            <asp:SqlDataSource runat="server" ID="SqlDataSource2" 
                                                                                                SelectCommand="SELECT * FROM VenueCategories WHERE LIVE = 'True' AND  ((ParentID IN (SELECT ID FROM VenueCategories WHERE (Name >= 'Li' AND Name <= 'Sa') AND (ParentID IS NULL))) OR ((Name >= 'Li' AND Name <= 'Sa') AND (ParentID IS NULL))) ORDER BY Name ASC"
                                                                                                ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                                                            </asp:SqlDataSource>
                                                                                        </div>
                                                                                    </td>
                                                                                    <td valign="top">
                                                                                        <div style=" width: 140px;">
                                                                                            <rad:RadTreeView Width="138px" runat="server"  
                                                                                            ID="RadTreeView2" DataFieldID="ID" DataSourceID="SqlDataSource3" 
                                                                                            DataFieldParentID="ParentID"  Skin="Vista"
                                                                                            CheckBoxes="true">
                                                                                            <DataBindings>
                                                                                                 <rad:RadTreeNodeBinding  Checkable="true" ValueField="ID" Checked="false" 
                                                                                                 TextField="Name" Expanded="false" />
                                                                                             </DataBindings>
                                                                                            </rad:RadTreeView>
                                                                                            <asp:SqlDataSource runat="server" ID="SqlDataSource3" 
                                                                                                SelectCommand="SELECT * FROM VenueCategories WHERE LIVE = 'True' AND  ((ParentID IN (SELECT ID FROM VenueCategories WHERE  (Name > 'Sa') AND (ParentID IS NULL))) OR ((Name > 'Sa') AND (ParentID IS NULL))) ORDER BY Name ASC"
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
                                                                <ctrl:BlueButton WIDTH="100px" runat="server" ID="FilterButton" BUTTON_TEXT="Filter Results" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                                            
                                                        
                                                        <div style="float: left; width: 420px; ">
                                                            <asp:Label runat="server" CssClass="ResultsLabel" ID="ResultsLabel"></asp:Label>
                                                                     <div class="topDiv" style="width: 420px;padding-bottom: 10px;padding-top: 10px;"> 
                                                                             <div style="float: left;">  <asp:DropDownList Visible="false" runat="server" AutoPostBack="true"  
                                                                            OnSelectedIndexChanged="SortResults" ID="SortDropDown">
                                                                                <asp:ListItem Text="Sort By..." Value="-1"></asp:ListItem>
                                                                                <asp:ListItem Text="Locale Name Up" Value="Name ASC"></asp:ListItem>
                                                                                <asp:ListItem Text="Locale Name Down" Value="Name DESC"></asp:ListItem>
                                                                            </asp:DropDownList>   
                                                                            </div>
                                                                            <div style="padding-left: 20px; float: right">
                                                                                <asp:Label runat="server" ID="NumsLabel" CssClass="Green12Link"></asp:Label>
                                                                            </div>
                                                                     </div>   
                                                                    <div class="topDiv">
                                                                        <asp:Panel runat="server" ID="Panel7" Width="440px"><asp:Label runat="server" ForeColor="Red" ID="Label1"></asp:Label>
                                                                                <asp:Literal runat="server" ID="HelpUsLiteral"></asp:Literal>
                                                                              <ctrl:SearchElements Visible="false" runat="server" ID="VenueSearchElements" />
                                                                              
                                                                         </asp:Panel>
                                                                    </div>
                                                                    </div>
                                                <asp:Panel runat="server" ID="NoResultsPanel" Visible="false">
                                                    <label>There are no locales matching your criteria, but, you can post them 
                                                    through our <a class="NavyLink12" href="enter-locale">Post a Locale Page</a></label>
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
           <%--<div class="topDiv" style="width: 847px;clear: both;margin-left:7px;position: relative;top: -138px;">
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
                                                                <b>Hippo Locale Tips</b>                                                                <ol>                                                                    <li>                                                                        Earn points toward being featured on our home page by posting more locales.                                                                     </li>                                                                    <li>                                                                        Post, Share, Add to favorites, text, email, discuss.                                                                     </li>                                                                    <li>                                                                        Receive notifications of new events posted in your favorite locale.                                                                    </li>                                                                    <li>                                                                        Edit locales you own or submit changes to locales owned by others.                                                                    </li>                                                                    <li>                                                                        Feature a locale for as little as $2/day.                                                                     </li>                                                                </ol>
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

