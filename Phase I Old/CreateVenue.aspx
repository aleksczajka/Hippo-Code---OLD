<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateVenue.aspx.cs" Inherits="CreateVenue" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagName="HippoTextBox" TagPrefix="ctrl" Src="~/Controls/HippoTextBox.ascx" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="~/StyleSheet.css" type="text/css" rel="stylesheet" />
</head>
<body style="background-color: #333333;">
    <form id="form1" runat="server">
             <asp:ScriptManager runat="server" ID="ScriptManager1" EnablePageMethods="true" ></asp:ScriptManager>

         <script type="text/javascript">
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
    <div class="EventDiv">
        <div style=""><asp:Literal runat="server" ID="EventLiteral"></asp:Literal>
               
              <asp:Panel runat="server" ID="LocationPanel">
                                                                   <span style="color: #ff6a01; font-family: Arial; font-size: 18px;">Enter New Venue</span>
                                
                                            <table cellpadding="0" cellspacing="0" style="padding-top: 10px;">
                                                <tr>
                                                    <td>
                                                        <label>Venue Name</label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-top: 5px;padding-left: 10px;padding-bottom: 10px;">
                                                        <ctrl:HippoTextBox ID="VenueNameTextBox" TEXTBOX_WIDTH="200" runat="server" 
                                                            CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                                                            
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>Location</label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                <td style="padding-top: 5px;padding-left: 10px;">
                                                    <label style="padding-right: 10px;">*Country</label><br />
                                                    <asp:DropDownList OnSelectedIndexChanged="ChangeState" AutoPostBack="true" runat="server" 
                                                    DataSourceID="SqlDataSourceCountry" DataTextField="country_name" DataValueField="country_id" 
                                                    ID="CountryDropDown"></asp:DropDownList>
                                                    <asp:SqlDataSource runat="server" ID="SqlDataSourceCountry" SelectCommand="SELECT * FROM Countries"
                                                    ConnectionString="<%$ ConnectionStrings:Connection %>"></asp:SqlDataSource>
                                                </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-top: 5px;padding-left: 10px;">
                                                        <table cellpadding="0" cellspacing="0">
                                            
                                                <asp:Panel runat="server" ID="USPanel">
                                                <tr>
                                                <td colspan="2">
                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="padding-top: 3px;">
                                                                <label>*Street No.</label>
                                                            </td>
                                                            <td style="padding-top: 3px;padding-left: 3px;">
                                                                <label>*Street Name</label>
                                                            </td>
                                                            <td style="padding-top: 3px;padding-left: 3px;">
                                                               
                                                            </td>
                                                            
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="StreetNumberTextBox" Width="70px"></asp:TextBox>
                                                            </td>
                                                            <td style="padding-left: 3px;">
                                                                <asp:TextBox runat="server" ID="StreetNameTextBox" Width="100px"></asp:TextBox>
                                                            </td>
                                                            <td style="padding-left: 3px;">
                                                                <asp:DropDownList runat="server" ID="StreetDropDown" Width="100px">
                                                                    <asp:ListItem Text="Select One..."></asp:ListItem>
                                                                    <asp:ListItem Text="Alley"></asp:ListItem>
                                                                    <asp:ListItem Text="Annex"></asp:ListItem>
                                                                    <asp:ListItem Text="Arcade"></asp:ListItem>
                                                                    <asp:ListItem Text="Avenue"></asp:ListItem>
                                                                    <asp:ListItem Text="Bayoo"></asp:ListItem>
                                                                    <asp:ListItem Text="Beach"></asp:ListItem>
                                                                    <asp:ListItem Text="Bend"></asp:ListItem>
                                                                    <asp:ListItem Text="Bluff"></asp:ListItem>
                                                                    <asp:ListItem Text="Bluffs"></asp:ListItem>
                                                                    <asp:ListItem Text="Bottom"></asp:ListItem>
                                                                    <asp:ListItem Text="Boulevard"></asp:ListItem>
                                                                    <asp:ListItem Text="Branch"></asp:ListItem>
                                                                    <asp:ListItem Text="Bridge"></asp:ListItem>
                                                                    <asp:ListItem Text="Brook"></asp:ListItem>
                                                                    <asp:ListItem Text="Brooks"></asp:ListItem>
                                                                    <asp:ListItem Text="Burg"></asp:ListItem>
                                                                    <asp:ListItem Text="Burgs"></asp:ListItem>
                                                                    <asp:ListItem Text="Bypass"></asp:ListItem>
                                                                    <asp:ListItem Text="Camp"></asp:ListItem>
                                                                    <asp:ListItem Text="Canyon"></asp:ListItem>
                                                                    <asp:ListItem Text="Cape"></asp:ListItem>
                                                                    <asp:ListItem Text="Causeway"></asp:ListItem>
                                                                    <asp:ListItem Text="Center"></asp:ListItem>
                                                                    <asp:ListItem Text="Centers"></asp:ListItem>
                                                                    <asp:ListItem Text="Circle"></asp:ListItem>
                                                                    <asp:ListItem Text="Circles"></asp:ListItem>
                                                                    <asp:ListItem Text="Cliff"></asp:ListItem>
                                                                    <asp:ListItem Text="Cliffs"></asp:ListItem>
                                                                    <asp:ListItem Text="Club"></asp:ListItem>
                                                                    <asp:ListItem Text="Common"></asp:ListItem>
                                                                    <asp:ListItem Text="Corner"></asp:ListItem>
                                                                    <asp:ListItem Text="Corners"></asp:ListItem>
                                                                    <asp:ListItem Text="Course"></asp:ListItem>
                                                                    <asp:ListItem Text="Court"></asp:ListItem>
                                                                    <asp:ListItem Text="Courts"></asp:ListItem>
                                                                    <asp:ListItem Text="Cove"></asp:ListItem>
                                                                    <asp:ListItem Text="Coves"></asp:ListItem>
                                                                    <asp:ListItem Text="Creek"></asp:ListItem>
                                                                    <asp:ListItem Text="Crescent"></asp:ListItem>
                                                                    <asp:ListItem Text="Crest"></asp:ListItem>
                                                                    <asp:ListItem Text="Crossing"></asp:ListItem>
                                                                    <asp:ListItem Text="Crossroad"></asp:ListItem>
                                                                    <asp:ListItem Text="Curve"></asp:ListItem>
                                                                    <asp:ListItem Text="Dale"></asp:ListItem>
                                                                    <asp:ListItem Text="Dam"></asp:ListItem>
                                                                    <asp:ListItem Text="Divide"></asp:ListItem>
                                                                    <asp:ListItem Text="Drive"></asp:ListItem>
                                                                    <asp:ListItem Text="Drives"></asp:ListItem>
                                                                    <asp:ListItem Text="Estate"></asp:ListItem>
                                                                    <asp:ListItem Text="Estates"></asp:ListItem>
                                                                    <asp:ListItem Text="Expressway"></asp:ListItem>
                                                                    <asp:ListItem Text="Extension"></asp:ListItem>
                                                                    <asp:ListItem Text="Extensions"></asp:ListItem>
                                                                    <asp:ListItem Text="Fall"></asp:ListItem>
                                                                    <asp:ListItem Text="Falls"></asp:ListItem>
                                                                    <asp:ListItem Text="Ferry"></asp:ListItem>
                                                                    <asp:ListItem Text="Field"></asp:ListItem>
                                                                    <asp:ListItem Text="Fields"></asp:ListItem>
                                                                    <asp:ListItem Text="Flat"></asp:ListItem>
                                                                    <asp:ListItem Text="Flats"></asp:ListItem>
                                                                    <asp:ListItem Text="Ford"></asp:ListItem>
                                                                    <asp:ListItem Text="Fords"></asp:ListItem>
                                                                    <asp:ListItem Text="Forest"></asp:ListItem>
                                                                    <asp:ListItem Text="Forge"></asp:ListItem>
                                                                    <asp:ListItem Text="Forges"></asp:ListItem>
                                                                    <asp:ListItem Text="Fork"></asp:ListItem>
                                                                    <asp:ListItem Text="Forks"></asp:ListItem>
                                                                    <asp:ListItem Text="Fort"></asp:ListItem>
                                                                    <asp:ListItem Text="Freeway"></asp:ListItem>
                                                                    <asp:ListItem Text="Garden"></asp:ListItem>
                                                                    <asp:ListItem Text="Gardens"></asp:ListItem>
                                                                    <asp:ListItem Text="Gateway"></asp:ListItem>
                                                                    <asp:ListItem Text="Glen"></asp:ListItem>
                                                                    <asp:ListItem Text="Glens"></asp:ListItem>
                                                                    <asp:ListItem Text="Green"></asp:ListItem>
                                                                    <asp:ListItem Text="Greens"></asp:ListItem>
                                                                    <asp:ListItem Text="Grove"></asp:ListItem>
                                                                    <asp:ListItem Text="Harbor"></asp:ListItem>
                                                                    <asp:ListItem Text="Harbors"></asp:ListItem>
                                                                    <asp:ListItem Text="Haven"></asp:ListItem>
                                                                    <asp:ListItem Text="Heights"></asp:ListItem>
                                                                    <asp:ListItem Text="Highway"></asp:ListItem>
                                                                    <asp:ListItem Text="Hill"></asp:ListItem>
                                                                    <asp:ListItem Text="Hills"></asp:ListItem>
                                                                    <asp:ListItem Text="Hollow"></asp:ListItem>
                                                                    <asp:ListItem Text="Inlet"></asp:ListItem>
                                                                    <asp:ListItem Text="Island"></asp:ListItem>
                                                                    <asp:ListItem Text="Islands"></asp:ListItem>
                                                                    <asp:ListItem Text="Isle"></asp:ListItem>
                                                                    <asp:ListItem Text="Junction"></asp:ListItem>
                                                                    <asp:ListItem Text="Junctions"></asp:ListItem>
                                                                    <asp:ListItem Text="Key"></asp:ListItem>
                                                                    <asp:ListItem Text="Keys"></asp:ListItem>
                                                                    <asp:ListItem Text="Knoll"></asp:ListItem>
                                                                    <asp:ListItem Text="Knolls"></asp:ListItem>
                                                                    <asp:ListItem Text="Lake"></asp:ListItem>
                                                                    <asp:ListItem Text="Lakes"></asp:ListItem>
                                                                    <asp:ListItem Text="Land"></asp:ListItem>
                                                                    <asp:ListItem Text="Landing"></asp:ListItem>
                                                                    <asp:ListItem Text="Lane"></asp:ListItem>
                                                                    <asp:ListItem Text="Light"></asp:ListItem>
                                                                    <asp:ListItem Text="Lights"></asp:ListItem>
                                                                    <asp:ListItem Text="Loaf"></asp:ListItem>
                                                                    <asp:ListItem Text="Lock"></asp:ListItem>
                                                                    <asp:ListItem Text="Locks"></asp:ListItem>
                                                                    <asp:ListItem Text="Lodge"></asp:ListItem>
                                                                    <asp:ListItem Text="Loop"></asp:ListItem>
                                                                    <asp:ListItem Text="Mall"></asp:ListItem>
                                                                    <asp:ListItem Text="Manor"></asp:ListItem>
                                                                    <asp:ListItem Text="Manors"></asp:ListItem>
                                                                    <asp:ListItem Text="Meadow"></asp:ListItem>
                                                                    <asp:ListItem Text="Meadows"></asp:ListItem>
                                                                    <asp:ListItem Text="Mews"></asp:ListItem>
                                                                    <asp:ListItem Text="Mill"></asp:ListItem>
                                                                    <asp:ListItem Text="Mills"></asp:ListItem>
                                                                    <asp:ListItem Text="Mission"></asp:ListItem>
                                                                    <asp:ListItem Text="Motorway"></asp:ListItem>
                                                                    <asp:ListItem Text="Mount"></asp:ListItem>
                                                                    <asp:ListItem Text="Mountain"></asp:ListItem>
                                                                    <asp:ListItem Text="Mountains"></asp:ListItem>
                                                                    <asp:ListItem Text="Neck"></asp:ListItem>
                                                                    <asp:ListItem Text="Orchard"></asp:ListItem>
                                                                    <asp:ListItem Text="Oval"></asp:ListItem>
                                                                    <asp:ListItem Text="Overpass"></asp:ListItem>
                                                                    <asp:ListItem Text="Park"></asp:ListItem>
                                                                    <asp:ListItem Text="Parks"></asp:ListItem>
                                                                    <asp:ListItem Text="Parkway"></asp:ListItem>
                                                                    <asp:ListItem Text="Parkways"></asp:ListItem>
                                                                    <asp:ListItem Text="Pass"></asp:ListItem>
                                                                    <asp:ListItem Text="Passage"></asp:ListItem>
                                                                    <asp:ListItem Text="Path"></asp:ListItem>
                                                                    <asp:ListItem Text="Pike"></asp:ListItem>
                                                                    <asp:ListItem Text="Pine"></asp:ListItem>
                                                                    <asp:ListItem Text="Pines"></asp:ListItem>
                                                                    <asp:ListItem Text="Place"></asp:ListItem>
                                                                    <asp:ListItem Text="Plain"></asp:ListItem>
                                                                    <asp:ListItem Text="Plains"></asp:ListItem>
                                                                    <asp:ListItem Text="Plaza"></asp:ListItem>
                                                                    <asp:ListItem Text="Point"></asp:ListItem>
                                                                    <asp:ListItem Text="Points"></asp:ListItem>
                                                                    <asp:ListItem Text="Port"></asp:ListItem>
                                                                    <asp:ListItem Text="Ports"></asp:ListItem>
                                                                    <asp:ListItem Text="Prairie"></asp:ListItem>
                                                                    <asp:ListItem Text="Radial"></asp:ListItem>
                                                                    <asp:ListItem Text="Ramp"></asp:ListItem>
                                                                    <asp:ListItem Text="Ranch"></asp:ListItem>
                                                                    <asp:ListItem Text="Rapid"></asp:ListItem>
                                                                    <asp:ListItem Text="Rapids"></asp:ListItem>
                                                                    <asp:ListItem Text="Rest"></asp:ListItem>
                                                                    <asp:ListItem Text="Ridge"></asp:ListItem>
                                                                    <asp:ListItem Text="Ridges"></asp:ListItem>
                                                                    <asp:ListItem Text="River"></asp:ListItem>
                                                                    <asp:ListItem Text="Road"></asp:ListItem>
                                                                    <asp:ListItem Text="Roads"></asp:ListItem>
                                                                    <asp:ListItem Text="Route"></asp:ListItem>
                                                                    <asp:ListItem Text="Row"></asp:ListItem>
                                                                    <asp:ListItem Text="Rue"></asp:ListItem>
                                                                    <asp:ListItem Text="Run"></asp:ListItem>
                                                                    <asp:ListItem Text="Shoal"></asp:ListItem>
                                                                    <asp:ListItem Text="Shoals"></asp:ListItem>
                                                                    <asp:ListItem Text="Shore"></asp:ListItem>
                                                                    <asp:ListItem Text="Shores"></asp:ListItem>
                                                                    <asp:ListItem Text="Skyway"></asp:ListItem>
                                                                    <asp:ListItem Text="Spring"></asp:ListItem>
                                                                    <asp:ListItem Text="Springs"></asp:ListItem>
                                                                    <asp:ListItem Text="Spur"></asp:ListItem>
                                                                    <asp:ListItem Text="Spurs"></asp:ListItem>
                                                                    <asp:ListItem Text="Square"></asp:ListItem>
                                                                    <asp:ListItem Text="Squares"></asp:ListItem>
                                                                    <asp:ListItem Text="Station"></asp:ListItem>
                                                                    <asp:ListItem Text="Stravenue"></asp:ListItem>
                                                                    <asp:ListItem Text="Stream"></asp:ListItem>
                                                                    <asp:ListItem Text="Street"></asp:ListItem>
                                                                    <asp:ListItem Text="Streets"></asp:ListItem>
                                                                    <asp:ListItem Text="Summit"></asp:ListItem>
                                                                    <asp:ListItem Text="Terrace"></asp:ListItem>
                                                                    <asp:ListItem Text="Throughway"></asp:ListItem>
                                                                    <asp:ListItem Text="Trace"></asp:ListItem>
                                                                    <asp:ListItem Text="Track"></asp:ListItem>
                                                                    <asp:ListItem Text="Trafficway"></asp:ListItem>
                                                                    <asp:ListItem Text="Trail"></asp:ListItem>
                                                                    <asp:ListItem Text="Tunnel"></asp:ListItem>
                                                                    <asp:ListItem Text="Turnpike"></asp:ListItem>
                                                                    <asp:ListItem Text="Underpass"></asp:ListItem>
                                                                    <asp:ListItem Text="Union"></asp:ListItem>
                                                                    <asp:ListItem Text="Valley"></asp:ListItem>
                                                                    <asp:ListItem Text="Valleys"></asp:ListItem>
                                                                    <asp:ListItem Text="Viaduct"></asp:ListItem>
                                                                    <asp:ListItem Text="View"></asp:ListItem>
                                                                    <asp:ListItem Text="Views"></asp:ListItem>
                                                                    <asp:ListItem Text="Village"></asp:ListItem>
                                                                    <asp:ListItem Text="Villages"></asp:ListItem>
                                                                    <asp:ListItem Text="Ville"></asp:ListItem>
                                                                    <asp:ListItem Text="Vista"></asp:ListItem>
                                                                    <asp:ListItem Text="Walk"></asp:ListItem>
                                                                    <asp:ListItem Text="Walks"></asp:ListItem>
                                                                    <asp:ListItem Text="Wall"></asp:ListItem>
                                                                    <asp:ListItem Text="Way"></asp:ListItem>
                                                                    <asp:ListItem Text="Ways"></asp:ListItem>
                                                                    <asp:ListItem Text="Well"></asp:ListItem>
                                                                    <asp:ListItem Text="Wells"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                       </tr>
                                                    </table>
                                                </td>
                                                </tr>
                                                </asp:Panel>
                                               
                                            
                                             <asp:Panel runat="server" ID="InternationalPanel" Visible="false">
                                               <tr>
                                                <td colspan="2">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <label>*Location</label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="LocationTextBox" Width="200px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                </tr>
                                                </asp:Panel>
                                                <tr>
                                                            <td colspan="2"  style="padding-top: 5px;">
                                                                <label>Suite/Apt</label>
                                                                <label>Apt No.</label>
                                                            </td>
                                                        </tr>
                                                       <tr>
                                                            <td colspan="2"  style="padding-top: 3px;">
                                                                <asp:DropDownList runat="server" ID="AptDropDown">
                                                                    <asp:ListItem Text="Suite" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="Apt" Value="2"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:TextBox runat="server" ID="AptNumberTextBox" Width="50px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                <tr>
                                                <td colspan="2" style="padding-top: 3px;">
                                                    <label>*State</label><br />
                                                    <asp:Panel runat="server" ID="StateTextBoxPanel">
                                                        <asp:TextBox ID="StateTextBox" runat="server" Width="100px" ></asp:TextBox>
                                                    </asp:Panel>
                                                    <asp:Panel runat="server" ID="StateDropDownPanel" Visible="false">
                                                        <asp:DropDownList runat="server" ID="StateDropDown"></asp:DropDownList>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="padding-top: 3px;">
                                                                <label>*City</label>
                                                            </td>
                                                            <td style="padding-top: 3px; padding-left: 3px;">
                                                                <label>*Zip</label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="VenueCityTextBox" runat="server" Width="100px" ></asp:TextBox>
                                                            </td>
                                                            <td style="padding-left: 3px;">
                                                                <asp:TextBox ID="ZipTextBox" runat="server" Width="100px" ></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                                    </td>
                                                </tr>
                                              
                                            </table>
                                            <asp:Label runat="server" ID="ErrorLabel" ForeColor="red"></asp:Label>
                                            <div style="padding-top: 30px; width: 100%; vertical-align: middle;" align="right">
                    <asp:Button runat="server" ID="Button3" Text="Create it" CssClass="SearchButton" 
                    OnClick="CreateNewVenue" onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" />
                    <asp:Button runat="server" ID="Button1" Text="Cancel" CssClass="SearchButton" 
                    OnClientClick="Search()" onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" />
                   
                
                    <%--<img ID="YesButton" onclick="SaveSetting(); Search();" 
                 src="image/YesButton.png" onmouseout="this.src='image/YesButton.png'"
                 onmouseover="this.src='image/YesButtonSelected.png'" />--%>
                
                </div>
                                        </asp:Panel>
               <asp:Panel runat="server" ID="ThankYouPanel" Visible="false">
                    <div style="color: #cccccc; font-family: Arial; font-size: 14px;" align="center">
                        You have created a new venue.
                        <asp:Label runat="server" ID="ThankYouLabel"></asp:Label>
                        <br /><br /><br />
                    <asp:Button runat="server" ID="Button2" Text="Done!" CssClass="SearchButton" 
                    OnClientClick="Search()" onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" />
                    </div>
               </asp:Panel>
                
                <asp:Label runat="server" ID="MessageLabel" Font-Names="Arial" Font-Size="12px" ForeColor="red"></asp:Label>
                <asp:Literal runat="server" ID="InvisiblesLiteral"></asp:Literal>
          </div>
    </div>
    </form>
</body>
</html>
