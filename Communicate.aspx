<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Communicate.aspx.cs" Inherits="Communicate" %>
<%@ Register TagName="BlueButton" TagPrefix="ctrl" Src="~/Controls/BlueButton.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="~/StyleSheet.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
         
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
                function CloseWindow(somtin)
                {
                
                    var oWindow = GetRadWindow(); 
                    oWindow.Close(somtin); 
                }
              
                function Search(somtin) 
                { 
                    CloseWindow(somtin);
                } 
                
                function Remove(theDiv)
                {
                    var d = document.getElementById('checksDiv');
                    var theInput = document.getElementById(theDiv);
                    d.removeChild(theInput);
                    
                    var response;
                    MessageAlert.RemoveID(theDiv, idArray, Remove_CallBack);
                }
                
                function Remove_CallBack(theID)
                { 
                    idArray.splice(theID.value, 1);
                    n = n-1;
                }
                
                function SendMsg()
                {
                    var msgText = document.getElementById('TextInput');
                    var userName = document.getElementById('userName');
                    var userID = document.getElementById('userID');
                    var eventName = document.getElementById('eventName');
                    var msgBody = document.getElementById('msgBody');
                    
                    MessageAlert.SendIt(msgText.value, userName.innerHTML, userID.innerHTML, eventName.innerHTML, idArray, msgBody.innerHTML);
                }
                function AddFriend(theName, i)
                {
                    if(document.getElementById('div' + i) == null)
                    {
                        idArray[n] = i;
                        n = n + 1;
                        var d = document.getElementById('checksDiv');
                        
                        var newdiv = document.createElement('div');
                        var divIdName = 'div' + i;
                        newdiv.setAttribute('id',divIdName);
                        newdiv.innerHTML = '<div style=\'float: left; width: 120px;\'><label>'+theName+'</label></div> <img src=\'image/DeleteCircle.png\' id=\'b'+i+'\' alt=\'Remove selection\' title=\'Remove selection\' onclick=\'Remove("div'+i+'");\' />';
                        d.appendChild(newdiv);
                    }
                }
         </script>
    <div class="Text12">
        <asp:Panel runat="server" ID="MessagePanel">
         <div>
            <label><asp:Label runat="server" ID="MessageLabel"></asp:Label></label>
                <h1>Communicate with others</h1>
  
                <span class="Text12">[Users who have their communication preferences set to 
                'OFF' for this event will not be shown here.]</span><br /><br />
                <table>
                    <tr>
                        <td valign="top" width="200px">
                            <span class="Text12"><b>Shift + click to select multiple recipients</b></span>
                            <div style="margin-top: 3px;">
                                <asp:ListBox runat="server" SelectionMode="Multiple" Width="200px" Height="330px" ID="UsersListBox">
                                
                                </asp:ListBox>
                            </div>
                        </td>
                        <td valign="top" width="400px" style="padding-left: 20px;">
                           <table>
                            <tr height="300px"> 
                                <td valign="top">
                                    <span class="Text12"><b>Write a personal message to send along with the event:</b></span><br/>
                                    <textarea runat="server" class="InputClass" style="margin-top: 3px;width: 400px; height: 180px; padding: 5px;" id="TextInput" ></textarea>
                                    <asp:Literal runat="server" ID="TextLiteral"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <div style="padding-top: 3px; width: 100%;" align="center">
                                        <table>
                                            <tr>
                                                <td>
                                                    <ctrl:BlueButton runat="server" ID="SendItButton" BUTTON_TEXT="Send It" />
                                                
                                                </td>
                                                <td>
                                                    <ctrl:BlueButton CLIENT_CLICK="Search()" runat="server" ID="Button1" BUTTON_TEXT="Never Mind" />                                   </div>
                                                </td>
                                            </tr>
                                        </table>
                                       
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="CommunicateLabel" CssClass="Green12Link"></asp:Label>
                                </td>
                            </tr>
                           </table>                            
                        </td>
                    </tr>
                </table>
                
                    
                
                <div id="allIDs" style="visibility: hidden; position: relative; top: 0;"></div>
                
             
                
                
               
                
                <div style="position: relative; top: 0;">
                <asp:Literal runat="server" ID="InvisiblesLiteral"></asp:Literal></div>
          </div>
                                        </asp:Panel>
              <asp:Panel runat="server" ID="ThankYouPanel" Visible="false">
                <div class="EventDiv" align="center" style="padding-top: 30px;">
                    <label>Your message has been sent!</label>
                    <br /><br /><br /><br />
                    <div style="float: right; width: 370px;">
                        <ctrl:BlueButton CLIENT_CLICK="Search()" runat="server" ID="BlueButton1" BUTTON_TEXT="Done" />                                   </div>
                    </div>
                </div>
            </asp:Panel>
    </div>
    <asp:Literal runat="server" ID="TechLiteral"></asp:Literal>
    </form>
</body>
</html>
