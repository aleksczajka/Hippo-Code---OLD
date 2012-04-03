<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PasswordAlert.aspx.cs" Inherits="PasswordAlert" %>
<%@ Register Src="~/Controls/BlueButton.ascx" TagName="BlueButton" TagPrefix="ctrl" %>
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
               function GetRadWindow() 
                { 
                  var oWindow = null; 
                  if (window.radWindow) 
                     oWindow = window.radWindow; 
                  else if (window.frameElement.radWindow) 
                     oWindow = window.frameElement.radWindow; 
                  return oWindow; 
                }   
                function CloseWindow(args)
                {
                
                    var oWindow = GetRadWindow(); 
                    var oArg = args;
                    
                    //oArg.Name = args;
                    oWindow.close(args); 
                }
              
                function Search(args) 
                { 
                    CloseWindow(args);
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
    <div class="EventDiv" align="center" style="vertical-align: middle; width: 390px; height: 100%;">
        <div style="width: 390px;" align="center">
            <label>Please provide your email address. An email with the instructions to reset your password will be sent to you. </label>
            <br /><br />
            <asp:TextBox runat="server" ID="EmailTextBox" Width="200px"></asp:TextBox><br /><br />
            <label><asp:Label ForeColor="Red" runat="server" ID="MessageLabel"></asp:Label></label><br /><br />
            <table width="100%">
                <tr>
                    <td width="50%" align="right">
                        <div style="width: 70px;">
                            <ctrl:BlueButton runat="server" ID="SendItButton" BUTTON_TEXT="Send It" />
                        </div>
                    </td>
                    <td width="50%" align="left">
                        <ctrl:BlueButton runat="server" ID="BlueButton1" CLIENT_CLICK="Search()" BUTTON_TEXT="Done" />
                    </td>
                </tr>
            </table>
                
            </div>
        </div>
 
    </form>
</body>
</html>
