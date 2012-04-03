<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PricingChart.aspx.cs" Inherits="PricingChart" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/Controls/BlueButton.ascx" TagName="BlueButton" TagPrefix="ctrl" %>
<%@ OutputCache Duration="1" VaryByParam="*" NoStore="true" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <script src="js/TotalJS.js" type="text/javascript"></script>
    <link href="~/StyleSheet.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
     
         <script type="text/javascript">
              
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
    <asp:Panel runat="server" DefaultButton="PricesButton">
        <div class="Text12">
            
            <h1><asp:Label runat="server" ID="TitleLabel"></asp:Label></h1>
            <label><asp:Label runat="server" ID="DescriptLabel"></asp:Label></label><br /><br />
                <h2>Choose the city and state</h2>
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="padding-left: 10px;">
                                    <h2 style="padding-right: 10px;">*Country</h2>
                                    <asp:DropDownList OnSelectedIndexChanged="ChangeState" AutoPostBack="true" runat="server" 
                                    DataSourceID="SqlDataSourceCountry" DataTextField="country_name" DataValueField="country_id" 
                                    ID="CountryDropDown"></asp:DropDownList>
                                    <asp:SqlDataSource runat="server" ID="SqlDataSourceCountry" SelectCommand="SELECT * FROM Countries"
                                    ConnectionString="<%$ ConnectionStrings:Connection %>"></asp:SqlDataSource>
                                </td>

                                <td style="padding-left: 10px;">
                                    <h2>State</h2>
                                    <asp:Panel runat="server" ID="StateTextBoxPanel">
                                        <asp:TextBox ID="StateTextBox" runat="server" Width="100px" ></asp:TextBox>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="StateDropDownPanel" Visible="false">
                                        <asp:DropDownList runat="server" AutoPostBack="true" OnSelectedIndexChanged="ChangeCity" ID="StateDropDown"></asp:DropDownList>
                                    </asp:Panel>
                                </td>
                                <td style="padding-left: 10px;">
                                    <h2>Major City</h2>
                                    <asp:Panel runat="server" ID="CityTextBoxPanel">
                                        <asp:TextBox ID="CityTextBox" runat="server" Width="100px" ></asp:TextBox>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="CityDropDownPanel" Visible="false">
                                        <asp:DropDownList AutoPostBack="true" OnSelectedIndexChanged="FillChart" runat="server" ID="MajorCityDrop"></asp:DropDownList>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="padding-top: 10px;padding-left: 10px;">
                                    <div style="float: left;">
                                        <ctrl:BlueButton runat="server" WIDTH="130px" ID="PricesButton" BUTTON_TEXT="Check Prices >" />
                                    </div>
                                </td>
                            </tr>
                       </table>
                       <asp:Label runat="server" ID="ErrorLabel" ForeColor="Red"></asp:Label>
                
            
                
               <asp:Panel runat="server" Visible="false" ID="PricesPanel">
               <br />
               <h2>Prices in <asp:Label runat="server" ID="LocationPricesLabel"></asp:Label> for Today</h2>
                <table align="center" style="border: solid 1px #dedbdb;">
                <tr>
                    <th>Number of Days</th>
                    <th>Standard Rate / day</th>
                    <th>Adjustment for Members / day</th>
                    <th><asp:Label runat="server" ID="AdjustmentLabel"></asp:Label></th>
                    <th>One Search Term (5c/day)</th>
                    <%--<th>Price / day</th>--%>
                    <th>Total Price</th>
                </tr>
                <tr>
                    <td align="center" ><asp:Label runat="server" ID="NumberDays1Label"></asp:Label></td>
                    <td align="center" ><asp:Label runat="server" ID="StandardRate1Label"></asp:Label></td>
                    <td align="center" ><asp:Label runat="server" ID="MembersAdjustment1Label"></asp:Label></td>
                    <td align="center" ><asp:Label runat="server" ID="EventsAdjustmentLabel"></asp:Label></td>
                    <td align="center" ><asp:Label runat="server" ID="SearchTerms1Label"></asp:Label></td>
                    <%--<td align="center" ><asp:Label runat="server" ID="Price1Label"></asp:Label></td>--%>
                    <td align="center" ><b><asp:Label runat="server" ID="PriceTotal1Label"></asp:Label></b></td>
                </tr>
                 <tr>
                    <td align="center" ><asp:Label runat="server" ID="NumberDays2Label"></asp:Label></td>
                    <td align="center" ><asp:Label runat="server" ID="StandardRate2Label"></asp:Label></td>
                    <td align="center" ><asp:Label runat="server" ID="MembersAdjustment2Label"></asp:Label></td>
                    <td align="center" ><asp:Label runat="server" ID="EventsAdjustment2Label"></asp:Label></td>
                    <td align="center" ><asp:Label runat="server" ID="SearchTerms2Label"></asp:Label></td>
                    <%--<td align="center" ><asp:Label runat="server" ID="Price2Label"></asp:Label></td>--%>
                    <td align="center" ><b><asp:Label runat="server" ID="PriceTotal2Label"></asp:Label></b></td>
               </tr>
                 <tr>
                    <td align="center" ><asp:Label runat="server" ID="NumberDays3Label"></asp:Label></td>
                    <td align="center" ><asp:Label runat="server" ID="StandardRate3Label"></asp:Label></td>
                    <td align="center" ><asp:Label runat="server" ID="MembersAdjustment3Label"></asp:Label></td>
                    <td align="center" ><asp:Label runat="server" ID="EventsAdjustment3Label"></asp:Label></td>
                    <td align="center" ><asp:Label runat="server" ID="SearchTerms3Label"></asp:Label></td>
                    <%--<td align="center" ><asp:Label runat="server" ID="Price3Label"></asp:Label></td>--%>
                    <td align="center" ><b><asp:Label runat="server" ID="PriceTotal3Label"></asp:Label></b></td>
                </tr>
                 <tr>
                    <td align="center" ><asp:Label runat="server" ID="NumberDays4Label"></asp:Label></td>
                    <td align="center" ><asp:Label runat="server" ID="StandardRate4Label"></asp:Label></td>
                    <td align="center" ><asp:Label runat="server" ID="MembersAdjustment4Label"></asp:Label></td>
                    <td align="center" ><asp:Label runat="server" ID="EventsAdjustment4Label"></asp:Label></td>
                    <td align="center" ><asp:Label runat="server" ID="SearchTerms4Label"></asp:Label></td>
                   <%-- <td align="center" ><asp:Label runat="server" ID="Price4Label"></asp:Label></td>--%>
                    <td align="center" ><b><asp:Label runat="server" ID="PriceTotal4Label"></asp:Label></b></td>
                </tr>
                </table>
                <div>
                    <span class="Asterisk">*</span>Actual prices will vary based on the number of increasing posts and members.
                </div>
               </asp:Panel>
               <table style="padding-top: 20px;" width="100%">
                <tr>
                    <td align="center">
                        <ctrl:BlueButton runat="server" WIDTH="100px" ID="BlueButton1" BUTTON_TEXT="Done" CLIENT_CLICK="Search();" />
                    </td>
                </tr>
               </table>
    </div> 
    </asp:Panel>
    </form>
</body>
</html>
