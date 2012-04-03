<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MessageAlert.aspx.cs" Inherits="MessageAlert" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagName="BlueButton" TagPrefix="ctrl" Src="~/Controls/BlueButton.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="~/StyleSheet.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server"></asp:ScriptManager>
 <telerik:RadAjaxPanel runat="server">      
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
                    
                    MessageAlert.SendIt(msgText.value, userName.innerHTML, userID.innerHTML, eventName.innerHTML, 
                    idArray, msgBody.innerHTML);
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
            <asp:Panel runat="server" ID="MessagePanel">
            <label><asp:Label runat="server" ID="MessageLabel"></asp:Label></label>
                <label><b>Who would you like to send this to?</b></label>
                <div style="padding-bottom: 15px; margin-top: 3px;">
                    <asp:Panel runat="server" ID="FriendsPanel" BorderStyle="solid" BorderWidth="1px" BorderColor="#d9d6d6" Height="120px" ScrollBars="Vertical" Width="100%">
                        
                    </asp:Panel>
                    <asp:LinkButton runat="server" CssClass="AddLink FloatRight" Text="Select All Friends" OnClick="SelectAllFriends"></asp:LinkButton>
                </div>
                <table width="100%">
                    <tr>
                        <td valign="top" width="264px">
                            <label><b>Your selections:</b></label>
                            <div style="margin-top: 3px;">
                            <asp:Panel runat="server" ID="ChecksPanel" BorderColor="#d9d6d6" BorderStyle="solid" BorderWidth="1px" Height="240px" ScrollBars="Vertical">
                                <%--<div id="checksDiv" style="height: 230px; background-color: #666666; padding: 5px;">
                                </div>--%>
                            </asp:Panel></div>
                        </td>
                        <td valign="top" width="300px" style="padding-left: 20px;">
                           <table>
                            <tr height="180px"> 
                                <td valign="top" colspan="2">
                                    <label><b>Write a personal message to send along with the event:</b></label><br/>
                                    <asp:TextBox TextMode="MultiLine" runat="server" CssClass="InputClass" ID="MessageInput"></asp:TextBox>
<%--                                    <textarea runat="server"  style="margin-top: 3px;border: 0;width: 300px; height: 180px; background-color: #666666; padding: 5px;" id="TextInput" ></textarea>
--%>                                    <asp:Literal runat="server" ID="TextLiteral"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td valign="middle">
                                    <div style="float: right;" align="center">
                                    <ctrl:BlueButton ID="SendItButton" runat="server" BUTTON_TEXT="Send It" />
                                    
                                    
                                    </div>
                                </td>
                                <td valign="middle">
                                    <ctrl:BlueButton ID="BlueButton1" CLIENT_CLICK="Search();" runat="server" BUTTON_TEXT="Forget It" />
      
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label runat="server" ID="MessageLabel2" CssClass="AddLink">
                                    </asp:Label>
                                </td>
                            </tr>
                           </table>
                            
                        </td>
                    </tr>
                </table>
                
                <div id="allIDs" style="visibility: hidden; position: relative; top: 0;"></div>
                
                <div style="position: relative; top: 0;">
                <asp:Literal runat="server" ID="InvisiblesLiteral"></asp:Literal></div>
           </asp:Panel>
           <asp:Panel runat="server" ID="ThankYouPanel" Visible="false">
                <div class="EventDiv" align="center" style="padding-top: 30px;">
                    <label>Your message has been sent!</label>
                    <br /><br /><br /><br />
                    <div style="float: right; width: 300px;">
                        <ctrl:BlueButton ID="BlueButton2" CLIENT_CLICK="Search();" runat="server" BUTTON_TEXT="Done" />
                    </div>
                </div>
            </asp:Panel>
          </div>
                     
    </div>
 </telerik:RadAjaxPanel>  
 <asp:Literal runat="server" ID="TechLiteral"></asp:Literal>
    </form>
</body>
</html>
