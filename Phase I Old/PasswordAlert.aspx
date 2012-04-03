<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PasswordAlert.aspx.cs" Inherits="PasswordAlert" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
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
            <label>Please include your user name. An email with the instructions to reset your password will be sent to the email address associated with this account. </label>
            <br /><br />
            <asp:TextBox runat="server" ID="UserNameTextBox" Width="100px"></asp:TextBox><br />
            <label><asp:Label CssClass="AddGreenLink" runat="server" ID="MessageLabel"></asp:Label></label><br />
            <asp:ImageButton runat="server" ID="SendItButton" OnClick="SendIt" ImageUrl="image/SendItButton.png" onmouseover="this.src='image/SendItButtonSelected.png'" onmouseout="this.src='image/SendItButton.png'"/>
             <img  onclick="Search();" id="CloseButton" src="image/DoneSonButton.png" 
             onmouseover="this.src='image/DoneSonButtonSelected.png'" onmouseout="this.src='image/DoneSonButton.png'" />

        </div>
    </div>
    </form>
</body>
</html>
