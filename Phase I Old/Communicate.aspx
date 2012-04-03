<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Communicate.aspx.cs" Inherits="Communicate" %>
<%@ OutputCache Duration="1" NoStore="true" VaryByParam="*" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="~/StyleSheet.css" type="text/css" rel="stylesheet" />
</head>
<body style="background-color: #333333; scrollbar-base-color: #666666; scrollbar-shadow-color: #666666; scrollbar-highlight-color: #666666; scrollbar-darkshadow-color: #666666; scrollbar-3dlight-color: #666666; scrollbar-track-color: #999999; scrollbar-arrow-color: #363636;">
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
                function CloseWindow()
                {
                
                    var oWindow = GetRadWindow(); 
                    oWindow.Close(); 
                }
              
                function Search() 
                { 
                    CloseWindow();
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
    <div class="MessageDiv">
        <asp:Panel runat="server" ID="MessagePanel">
         <div class="EventDiv">
            <label><asp:Label runat="server" ID="MessageLabel"></asp:Label></label>
                <h1>Communicate with others</h1>
  
                <label><span style="font-size: 10px; color: #cccccc;">[Users who have their communication preferences set to 
                'OFF' for this event will not be shown here.]</span></label>
                <table>
                    <tr>
                        <td valign="top" width="200px">
                            <label><b>Shift + click to select multiple recipients</b></label>
                            <div style="margin-top: 3px;">
                                <asp:ListBox runat="server" BackColor="#666666" Font-Names="Arial" Font-Bold="true" ForeColor="#cccccc" SelectionMode="Multiple" Width="200px" Height="350px" ID="UsersListBox">
                                
                                </asp:ListBox>
                            </div>
                        </td>
                        <td valign="top" width="400px" style="padding-left: 20px;">
                           <table>
                            <tr height="300px"> 
                                <td valign="top">
                                    <label><b>Write a personal message to send along with the event:</b></label><br/>
                                    <textarea runat="server"  style="color: #1fb6e7;margin-top: 3px;border: 0;width: 400px; height: 180px; background-color: #666666; padding: 5px;" id="TextInput" ></textarea>
                                    <asp:Literal runat="server" ID="TextLiteral"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <div style="padding-top: 3px; width: 100%;" align="center">
                                        <table>
                                            <tr>
                                                <td>
                                                 <asp:ImageButton ID="SendItButton" runat="server" OnClick="SendIt" 
                                     src="image/SendItButton.png" onmouseout="this.src='image/SendItButton.png'"
                                     onmouseover="this.src='image/SendItButtonSelected.png'" />
                                                </td>
                                                <td>
<asp:Button runat="server" ID="Button1" Text="Forget It!" CssClass="SearchButton" 
             OnClientClick="Search();"  onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" />                                     </div>
                                                </td>
                                            </tr>
                                        </table>
                                       
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="CommunicateLabel" CssClass="AddLink"></asp:Label>
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
                    <asp:ImageButton runat="server" ID="ImageButton9" 
                    ImageUrl="~/image/DoneSonButton.png" OnClientClick="Search();" 
                    onmouseout="this.src='../image/DoneSonButton.png'" 
                    onmouseover="this.src='../image/DoneSonButtonSelected.png'"  />

                </div>
            </asp:Panel>
    </div>
    </form>
</body>
</html>