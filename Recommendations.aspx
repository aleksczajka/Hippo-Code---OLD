<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Recommendations.aspx.cs" Inherits="Recommendations" %>
<%@ Register Src="~/Controls/Pager.ascx" TagPrefix="ctrl" TagName="Pager" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
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
                function CloseWindow(args)
                {
                
                    var oWindow = GetRadWindow(); 
                    var oArg = args;
                    
                    //oArg.Name = args;
                    oWindow.close(args); 
                }
              
                function Search(args) 
                { 
                    var oWindow = GetRadWindow(); 
                    var oArg = args;
                    
                    //oArg.Name = args;
                    oWindow.close(args); 
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
    <div class="EventDiv" style="vertical-align: middle; height: 100%;">
        <div>
        <a class="AddLink" style="float: right;text-decoration: underline;" onclick="Search();">close</a>
            <label><asp:Label runat="server" ID="MessageLabel"></asp:Label></label>
            <label>Choose Time Frame: </label>
            <asp:DropDownList runat="server" AutoPostBack="true" ID="TimeFrameDropDown" OnSelectedIndexChanged="ChangeDS">
                <asp:ListItem Value="0">(Default: Next Six Months)</asp:ListItem>
                <asp:ListItem Value="6">All</asp:ListItem>
                <asp:ListItem Value="1">This Week</asp:ListItem>
                <asp:ListItem Value="2">This Weekend</asp:ListItem>
                <asp:ListItem Value="3">Next Weekend</asp:ListItem>
                <asp:ListItem Value="4">Next Week</asp:ListItem>
                <asp:ListItem Value="5">Next Two Weeks</asp:ListItem>
                <asp:ListItem Value="7">This Month</asp:ListItem>
                <asp:ListItem Value="8">Next Two Months</asp:ListItem>
            </asp:DropDownList>
            <br /><br />
            <asp:Panel runat="server" Width="710px" ID="RecommendedEvents"></asp:Panel>
        </div>
    </div>
    </form>
</body>
</html>
