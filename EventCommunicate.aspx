<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EventCommunicate.aspx.cs" Inherits="EventCommunicate" %>
<%@ Register Src="~/Controls/BlueButton.ascx" TagName="BlueButton" TagPrefix="ctrl" %>
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
                function CloseWindow()
                {
                
                    var oWindow = GetRadWindow(); 
                    oWindow.Close(); 
                }
              
                function Search() 
                { 
                    CloseWindow();
                } 
                function SaveSetting()
                {
                    var userID = document.getElementById('userID');
                    var eventID = document.getElementById('eventID');
                    var userSetting = document.getElementById('userSetting');
                    
                    EventCommunicate.SaveSettingDB(userID.innerHTML, eventID.innerHTML, userSetting.innerHTML);
                }
              
         </script>
    <div class="EventDiv">
        <div style=""><asp:Literal runat="server" ID="EventLiteral"></asp:Literal>
               <table>
                <tr>
                    <td><img src="image/Exclamation.png" /></td>
                    <td><label>Your communication preference for this event is set to </label> <asp:Literal ID="SettingLabel" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <label><asp:Label runat="server" ID="ChangeToLabel"></asp:Label></label>
                    </td>
                </tr>
               </table>
               
                <div style="padding-top: 10px; width: 100%; vertical-align: middle;" align="center">
                    <div style="width: 200px;float: left;padding-right: 10px;">
                        <div style="float: right;">
                            <ctrl:BlueButton runat="server" BUTTON_TEXT="YES" ID="YesButton" />
                        </div>
                    </div>
                    <div style="width: 100px;float: left;">
                        <div style="float: left;">
                            <ctrl:BlueButton runat="server" BUTTON_TEXT="Done" CLIENT_CLICK="Search()" ID="BlueButton1" />
                        </div>
                    </div>
                </div>
                <asp:Literal runat="server" ID="InvisiblesLiteral"></asp:Literal>
          </div>
    </div>
    </form>
</body>
</html>
