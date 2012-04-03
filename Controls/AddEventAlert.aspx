<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddEventAlert.aspx.cs" Inherits="AddEventAlert" %>
<%@ Register TagName="BlueButton" TagPrefix="ctrl" Src="~/Controls/BlueButton.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
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
                function CloseWindow(variable)
                {
                
                    var oWindow = GetRadWindow(); 
                    oWindow.Close(variable); 
                }
              
                function Search(variable) 
                { 
                    CloseWindow(variable);
                } 
                
                function AddToCalendar()
                {
                    var theDiv = document.getElementById('TechDiv');
                    AddEventAlert.AddTo(theDiv.innerHTML, Callback_function);
                }
                
                function Callback_function(response, res)
                {
                    Search(res);
                }
         </script>
    <div class="EventDiv">
    <asp:Literal runat="server" ID="ScriptLiteral"></asp:Literal>
        <div>
            <label><asp:Label runat="server" ForeColor="red" ID="MessageLabel"></asp:Label></label>
            <asp:Panel runat="server" ID="ContentPanel">
                <div align="center" style="padding-top: 30px;">
                    <span class="Text12">Do you want to add this event to your calendar? </span><br /><br />
<%--                    <label>How do you feel about going to this event?</label> 
                    <asp:DropDownList runat="server" ID="EventExcitmentDropDown" DataSourceID="LevelDataSource" DataTextField="ExcitmentLevel" DataValueField="ID"></asp:DropDownList>
                    <asp:SqlDataSource ID="LevelDataSource" runat="server" SelectCommand="SELECT * FROM Event_ExcitmentLevel" ConnectionString="<%$ ConnectionStrings:Connection%>"></asp:SqlDataSource>
                    <br /><br />
                    <asp:CheckBox runat="server" Text="Do you want people to conntact you regarding going to this event?" ID="ConnectCheckBox" />
                    <br /><br />
--%>                    <asp:Literal runat="server" ID="TechLiteral"></asp:Literal>
                </div>
            <div style="padding-top: 20px;">
                    <table width="100%" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="right">
                                <div style="float: right;padding-right: 4px;">
                                    <ctrl:BlueButton runat="server" BUTTON_TEXT="Yes, Please"  id="YesButton" />
                                </div>
                            </td>
                            <td align="left" style="padding-left: 4px;">
                                <ctrl:BlueButton runat="server" BUTTON_TEXT="No, Thanks" id="CloseButton" CLIENT_CLICK="Search()" />
                            </td>
                        </tr>
                       <%-- <tr>
                            <td colspan="2">
                                <img align="right" onclick="Search();" id="Img1" src="../image/DoneSonButton.png" 
                                   onmouseover="this.src='../image/DoneSonButtonSelected.png'" onmouseout="this.src='../image/DoneSonButton.png'" />
                            </td>
                        </tr>--%>
                        
                    </table>
                    <asp:Label runat="server" ID="ErrorLabel"></asp:Label>
                </div>
                          </asp:Panel>
            <asp:Panel runat="server" ID="SetPreferences" Visible="false">
                   <div class="EventDiv" align="center" style="padding-top: 0px;">
                    <span class="Text12">The event has been added to your calendar. <br />
                    Your communication preferences are set to 'Set per Event'. 
                    Would you like to turn on the communication on for this event?<br />
                    [You can always change this setting in 'My Calendar' along with other settings like custom Event Reminders by clicking on the event.]
                    
                    </span>
                    <br /><br />
                    
                    <div style="width: 144px;">
                        <div style="float:left;padding-right: 5px;">
                            <ctrl:BlueButton runat="server" ID="Button2" BUTTON_TEXT="Yes" />
                        </div>
                        <div style="float:left;">
                            <ctrl:BlueButton runat="server" ID="Button1" BUTTON_TEXT="No, Thanks" />
                        </div>
                    </div>

                </div>
            
            </asp:Panel>
              <asp:Panel runat="server" ID="ThankYouPanel" Visible="false">
                <div class="EventDiv" align="center" style="padding-top: 10px;">
                    <span class="Text12">The event has been added to your calendar.</span>
                    <br /><br /><br /><br />
                    <div style="width: 60px;" align="center">
                        <ctrl:BlueButton runat="server" ID="BlueButton1" BUTTON_TEXT="Done" CLIENT_CLICK="Search('anything');" />
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="ThankYouPanel2" Visible="false">
                <div class="EventDiv" align="center" style="padding-top: 30px;">
                    <span class="Text12">Your event has been added and your preferences have been set.</span>
                    <br /><br />
                    <div style="width: 60px;" align="center">
                        <ctrl:BlueButton runat="server" ID="ImageButton9" BUTTON_TEXT="Done" CLIENT_CLICK="Search('anything');" />
                    </div>
                </div>
            </asp:Panel>
          </div>
    </div>
    <asp:Literal runat="server" ID="Literal1"></asp:Literal>
    </form>
</body>
</html>
