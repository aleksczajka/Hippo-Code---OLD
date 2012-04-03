<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AlarmMessage.aspx.cs" Inherits="AlarmMessage" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="~/AlertStyleSheet.css" type="text/css" rel="stylesheet" />
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
                <span style="color: #ff6a01; font-family: Arial; font-size: 18px;">Set An Alert for this Event</span>
               <div class="topDiv EventDiv Pad">
                         <div style="width: 320px; float: right; padding-top: 15px;">
                            <div>
                                               <asp:RadioButtonList runat="server" ID="AlertRadioList" RepeatDirection="horizontal">
                                                    <asp:ListItem Text="On" Selected="true"></asp:ListItem>
                                                    <asp:ListItem Text="Off"></asp:ListItem>
                                                </asp:RadioButtonList>

                            </div>
                            <div style="clear: both;">
                                <div style="float: left; padding-right: 2px;">
                                    <asp:CheckBox runat="server" ID="TimeCheck" /> 
                                </div>
                                <div style="float: left; padding-right: 5px; padding-bottom: 3px;">
                                    <asp:TextBox Width="50px" runat="server" ID="TimeTextBox"></asp:TextBox>
                                </div>
                                <div style="float: left; padding-right: 5px;">
                                    <asp:DropDownList runat="server" ID="TimeDropDown">
                                        <asp:ListItem Value="0" Text="Minutes"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Hours"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Days"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Weeks"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div style="float: left;">
                                    <label>before event occurs</label>
                                </div>
                            </div>
                            <div style="clear: both;">
                                <div style="float: left; padding-right: 2px;">
                                    <asp:CheckBox runat="server" ID="RepeatCheck" /> 
                                </div>
                                <div style="float: left; padding-right: 5px;">
                                    <label>repeat alert every</label>
                                </div>
                                <div style="float: left; padding-right: 5px; padding-bottom: 3px;">
                                    <asp:TextBox Width="50px" runat="server" ID="RepeatTextBox"></asp:TextBox>
                                </div>
                                <div style="float: left;">
                                    <asp:DropDownList runat="server" ID="RepeatDropDown">
                                        <asp:ListItem Value="0" Text="Minutes"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Hours"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Days"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Weeks"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div style="clear: both;">
                                <div style="float: left; padding-right: 2px;">
                                    <asp:CheckBox runat="server" ID="EndedCheck" /> 
                                </div>
                                <div style="float: left;">
                                    <label>alert me when event has ended</label>
                                </div>
                            </div>
                            <div style="clear: both;">
                                <div style="float: left; padding-right: 2px;">
                                    <asp:CheckBox runat="server" ID="EmailCheck" /> 
                                </div>
                                <div style="float: left;">
                                    <label>make this an email alert</label>
                                </div>
                            </div>
                            <div style="clear: both;">
                                <div style="float: left; padding-right: 2px;">
                                    <asp:CheckBox runat="server" ID="TextCheck" /> 
                                </div>
                                    <label>make this a text message alert <span style="line-height: 12px; font-family: Arial; color: #ff6a01;  font-size: 10px;">
                                    make sure you have a phone number and provider entered in 'My Preferences'
                                </span></label>
                               
                            </div>
                         </div>
                            <div style="clear: both; padding-top: 10px;">
                            <div style="float: left; line-height: 12px; font-family: Arial; color: #cccccc;  font-size: 10px;">
                                   For accurate alert calibration, please enter the 
                                   exact time in your location right now.
                                </div><br />
                                <div style="position: static; top: 0; left: 10; margin-top: -5px;">
                                    <telerik:RadTimePicker Skin="Black" TimeView-HorizontalAlign="Left" ID="TimePicker" runat="server"></telerik:RadTimePicker>
                                </div>
                                
                            </div>
                        </div>
               
                <div style="padding-top: 10px; width: 100%; vertical-align: middle;" align="right">
                    
                    <asp:Button runat="server" ID="Button1" Text="Done!" CssClass="SearchButton" 
                    OnClientClick="Search()" onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" />
                   <asp:Button runat="server" ID="Button3" Text="Save it" CssClass="SearchButton" 
                    OnClick="SaveSettingsDB" onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" />
                
                    <%--<img ID="YesButton" onclick="SaveSetting(); Search();" 
                 src="image/YesButton.png" onmouseout="this.src='image/YesButton.png'"
                 onmouseover="this.src='image/YesButtonSelected.png'" />--%>
                
                </div>
                            <div style="clear: both; position: absolute; font-weight: bold; font-size: 11px; line-height: 12px;">
                                <label><asp:RangeValidator ID="RangeValidator2" ForeColor="red" 
                                runat="server"  ControlToValidate="TimeTextBox" 
                                Type="Integer" MinimumValue="-999999" MaximumValue="999999" 
                                ErrorMessage="Time must be an integer and a value between -999999 and 999999. "></asp:RangeValidator>
                                </label>
                                <label><asp:RangeValidator ID="RangeValidator1" ForeColor="red" 
                                runat="server"  ControlToValidate="RepeatTextBox" Type="Integer" 
                                MinimumValue="-999999" MaximumValue="999999" ErrorMessage="Repeat time must be an integer and a value between -999999 and 999999."></asp:RangeValidator>
                                </label>
                            </div>
                <asp:Label runat="server" ID="MessageLabel" Font-Names="Arial" Font-Size="12px" ForeColor="red"></asp:Label>
                <asp:Literal runat="server" ID="InvisiblesLiteral"></asp:Literal>
          </div>
    </div>
    </form>
</body>
</html>
