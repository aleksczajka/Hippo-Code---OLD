<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MessageAlert.aspx.cs" Inherits="MessageAlert" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="~/StyleSheet.css" type="text/css" rel="stylesheet" />
</head>
<body style="background-color: #333333; scrollbar-base-color: #666666; scrollbar-shadow-color: #666666; scrollbar-highlight-color: #666666; scrollbar-darkshadow-color: #666666; scrollbar-3dlight-color: #666666; scrollbar-track-color: #999999; scrollbar-arrow-color: #363636;">
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
                    <asp:Panel runat="server" ID="FriendsPanel" BorderStyle="solid" BorderWidth="1px" BorderColor="Black" BackColor="#363636" Height="120px" ScrollBars="Vertical" Width="100%">
                        
                    </asp:Panel>
                    <asp:LinkButton runat="server" CssClass="AddLink FloatRight" Text="Select All Friends" OnClick="SelectAllFriends"></asp:LinkButton>
                </div>
                <table width="100%">
                    <tr>
                        <td valign="top" width="264px">
                            <label><b>Your selections:</b></label>
                            <div style="margin-top: 3px;">
                            <asp:Panel runat="server" ID="ChecksPanel" BackColor="#666666" Height="240px" ScrollBars="Vertical">
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
                                    <div style="padding-top: 3px; width: 100%;" align="center">
                                    <asp:Button OnClick="ServerSendMessage" runat="server" ID="Button1" 
                                    Text="Send It!" CssClass="SearchButton" 
                 onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" />  
                                    
                                        <%--<button runat="server" onserverclick="ServerSendMessage" ID="SendItButton"
                                     style="cursor: pointer;height: 40px; width: 120px;background-color: transparent; color: White; background-image: url('image/SendItButton.png'); background-repeat: no-repeat; border: 0;" 
                                     onmouseout="this.style.backgroundImage='url(image/SendItButton.png)'"
                                     onmouseover="this.style.backgroundImage='url(image/SendItButtonSelected.png)'" ></button>--%>
                                    </div>
                                </td>
                                <td valign="middle">
<asp:Button runat="server" ID="Button2" Text="Forget It!" CssClass="SearchButton" 
             OnClientClick="Search();"  onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" />         
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
                    <asp:ImageButton runat="server" ID="ImageButton9" 
                    ImageUrl="~/image/DoneSonButton.png" OnClientClick="Search();" 
                    onmouseout="this.src='../image/DoneSonButton.png'" 
                    onmouseover="this.src='../image/DoneSonButtonSelected.png'"  />

                </div>
            </asp:Panel>
          </div>
                     
    </div>
 </telerik:RadAjaxPanel>  
    </form>
</body>
</html>
