<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdMessage.aspx.cs" Inherits="AdMessage" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
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
                    AdMessage.RemoveID(theDiv, idArray, Remove_CallBack);
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
                    
                    AdMessage.SendIt(msgText.value, userName.innerHTML, userID.innerHTML, eventName.innerHTML, idArray, msgBody.innerHTML);
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
        <div>
            <label><asp:Label runat="server" ID="MessageLabel"></asp:Label></label>
                <label><b>Who would you like to send this to?</b></label>
                <div style="padding-bottom: 15px; margin-top: 3px;">
                    <asp:Panel runat="server" ID="FriendsPanel" BorderStyle="solid" BorderWidth="1px" BorderColor="Black" BackColor="#363636" Height="120px" ScrollBars="Vertical" Width="100%">
                        <asp:Literal runat="server" ID="FriendLiteral"></asp:Literal>
                    </asp:Panel>
                </div>
                <table>
                    <tr>
                        <td valign="top" width="264px">
                            <label><b>Your selections:</b></label>
                            <div style="margin-top: 3px;">
                            <asp:Panel runat="server" ID="ChecksPanel" Height="240px" ScrollBars="Vertical">
                                <div id="checksDiv" style="height: 230px; background-color: #666666; padding: 5px;">
                                </div>
                            </asp:Panel></div>
                        </td>
                        <td valign="top" width="300px" style="padding-left: 20px;">
                           <table>
                            <tr height="180px"> 
                                <td valign="top">
                                    <label><b>Write a personal message to send along with the ad:</b></label><br/>
                                    <textarea  style="margin-top: 3px;border: 0;width: 300px; height: 180px; background-color: #666666; padding: 5px;" id="TextInput" ></textarea>
                                    <asp:Literal runat="server" ID="TextLiteral"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <div style="padding-top: 3px; width: 100%;" align="center">
                                        <img align="left" ID="SendItButton" onclick="SendMsg(); Search();" 
                                     src="image/SendItButton.png" onmouseout="this.src='image/SendItButton.png'"
                                     onmouseover="this.src='image/SendItButtonSelected.png'" />
                                     <img onclick="Search();" src="image/DoneSonButton.png" 
                                     onmouseover="this.src='image/DoneSonButtonSelected.png'" onmouseout="this.src='image/DoneSonButton.png'" />
                                    </div>
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
    </div>
    </form>
</body>
</html>
