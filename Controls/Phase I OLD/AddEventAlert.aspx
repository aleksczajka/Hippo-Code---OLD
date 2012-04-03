<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddEventAlert.aspx.cs" Inherits="AddEventAlert" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="~/StyleSheet.css" type="text/css" rel="stylesheet" />
</head>
<body style="background-color: #333333;">
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
                
                function Callback_function(response)
                {
                    Search();
                }
         </script>
    <div class="EventDiv">
    <asp:Literal runat="server" ID="ScriptLiteral"></asp:Literal>
        <div>
            <label><asp:Label runat="server" ForeColor="red" ID="MessageLabel"></asp:Label></label>
            <asp:Panel runat="server" ID="ContentPanel">
                <div align="center" style="padding-top: 10px;">
                    <label class="AddGreenLink">Do you want to add this event to your calendar? </label><br /><br />
<%--                    <label>How do you feel about going to this event?</label> 
                    <asp:DropDownList runat="server" ID="EventExcitmentDropDown" DataSourceID="LevelDataSource" DataTextField="ExcitmentLevel" DataValueField="ID"></asp:DropDownList>
                    <asp:SqlDataSource ID="LevelDataSource" runat="server" SelectCommand="SELECT * FROM Event_ExcitmentLevel" ConnectionString="<%$ ConnectionStrings:Connection%>"></asp:SqlDataSource>
                    <br /><br />
                    <asp:CheckBox runat="server" Text="Do you want people to conntact you regarding going to this event?" ID="ConnectCheckBox" />
                    <br /><br />
--%>                    <asp:Literal runat="server" ID="TechLiteral"></asp:Literal>
                </div>
            <div style="padding-top: 5px;">
                    <table width="100%" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="right">
                                <asp:ImageButton runat="server" OnClick="Server_AddTo" 
                                ID="AddCalendarButton" ImageUrl="../image/YesButton.png" 
                                onmouseout="this.src='../image/YesButton.png'" 
                                onmouseover="this.src='../image/YesButtonSelected.png'" />
                            </td>
                            <td align="left">
                                <img onclick="Search();" id="CloseButton" src="../image/CancelButton.png" 
                                   onmouseover="this.src='../image/CancelButtonSelected.png'" 
                                   onmouseout="this.src='../image/CancelButton.png'" />
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
                    <label>The event has been added to your calendar. <br />
                    Your communication preferences are set to 'Set per Event'. 
                    Would you like to turn on the communication on for this event?
                    [You can always change this setting in 'My Calendar' along with other settings like custom Event Reminders by clicking on the event.]
                    
                    </label>
                    <br /><br />
                     <asp:Button runat="server" ID="Button2" Text="Hell Yeah!" CssClass="SearchButton" 
                        OnClick="SetPrefs"  onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" /> 
                    <asp:Button runat="server" ID="Button1" Text="No Way, Man!" CssClass="SearchButton" 
                    OnClick="UnSetPrefs" onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" /> 

                </div>
            
            </asp:Panel>
              <asp:Panel runat="server" ID="ThankYouPanel" Visible="false">
                <div class="EventDiv" align="center" style="padding-top: 10px;">
                    <label>Your message has been sent!</label>
                    <br /><br /><br /><br />
                    <asp:ImageButton runat="server" ID="ImageButton9" 
                    ImageUrl="~/image/DoneSonButton.png" OnClientClick="Search('anything');" 
                    onmouseout="this.src='../image/DoneSonButton.png'" 
                    onmouseover="this.src='../image/DoneSonButtonSelected.png'"  />

                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="ThankYouPanel2" Visible="false">
                <div class="EventDiv" align="center" style="padding-top: 30px;">
                    <label>Your event has been added, your preferences have been set!</label>
                    <br /><br />
                    <asp:ImageButton runat="server" ID="ImageButton1" 
                    ImageUrl="~/image/DoneSonButton.png" OnClientClick="Search('anything');" 
                    onmouseout="this.src='../image/DoneSonButton.png'" 
                    onmouseover="this.src='../image/DoneSonButtonSelected.png'"  />

                </div>
            </asp:Panel>
          </div>
    </div>
    </form>
</body>
</html>
