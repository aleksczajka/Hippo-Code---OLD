<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EnableAd.aspx.cs" Inherits="EnableAd" %>
<%@ Register TagName="BlueButton" TagPrefix="ctrl" Src="~/Controls/BlueButton.ascx" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="~/StyleSheet.css" type="text/css" rel="stylesheet" />
    <script src="js/TotalJS.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
         
         <script type="text/javascript">
                var idArray = new Array();
                var n = 0;
                function DeleteAd()
                 {
                    var userID = document.getElementById('userID');
                    var eventID = document.getElementById('eventID');
                    Delete.DeleteAd1(eventID.innerHTML, userID.innerHTML, DeleteAd_Callback);
                 }
                 
                 function DeleteAd_Callback(response)
                 {
                    Search();
                 }
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
    <div align="center" class="EventDiv radconfirm" style="font-family: Arial; font-size: 12px;height: 100px; padding: 10px;">   
    
    <asp:Panel ID="RemovePanel" runat="server">  
                        <asp:Literal runat="server" ID="userLiteral"></asp:Literal>
                         <table cellpadding="0" cellspacing="0" style="padding-top: 10px;">
                            <tr>
                                <td colspan="2" align="center">
                                    <label> Are you sure you want to re-enable this ad? <br />
                                     </label>
                                     <br /><br />
                                </td>
                            </tr>
                            <tr>
                             <td align="right" width="50%">
                           
                              <ctrl:BlueButton CLIENT_CLICK="Search()" runat="server" ID="BlueButton2" BUTTON_TEXT="Forget It" />
                        
                            </td>
                            <td align="left" style="padding-left: 5px;">
                            <ctrl:BlueButton  runat="server" ID="BlueButton3" BUTTON_TEXT="Yes" />

                            </tr>
                         </table>
                  </asp:Panel>    
                          <asp:Panel runat="server" ID="ThankYouPanel" Visible="false">
                <div class="EventDiv" align="center" style="padding-top: 20px;">
                    <label>Your ad has been enabled!</label>
                    <br /><br />
                    <ctrl:BlueButton CLIENT_CLICK="Search('something')" runat="server" ID="BlueButton1" BUTTON_TEXT="Done" />

                </div>
            </asp:Panel>
                     </div>  
                             <asp:Literal runat="server" ID="InvisiblesLiteral"></asp:Literal>
         
    </form>
</body>
</html>
