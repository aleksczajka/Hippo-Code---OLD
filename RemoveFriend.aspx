<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RemoveFriend.aspx.cs" Inherits="RemoveFriend" %>
<%@ OutputCache Duration="1" NoStore="true" VaryByParam="*" %>
<%@ Register TagName="BlueButton" TagPrefix="ctrl" Src="~/Controls/BlueButton.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="~/StyleSheet.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
         
         <script type="text/javascript">
              
               function GetRadWindow() 
                { 
                  var oWindow = null; 
                  if (window.radWindow) 
                     oWindow = window.radWindow; 
                  else if (window.frameElement.radWindow) 
                     oWindow = window.frameElement.radWindow; 
                  return oWindow; 
                }   
                function CloseWindow(toloc)
                {
                
                    var oWindow = GetRadWindow(); 
                    oWindow.Close(toloc); 
                }
              
                function Search(toloc) 
                { 
                        CloseWindow(toloc);
                }

         </script>
    <div align="center" class="EventDiv radconfirm" style="font-family: Arial; font-size: 12px; 
    height: 100px; padding: 10px;">   
    
    <asp:Panel ID="RemovePanel" runat="server">  
                        <asp:Literal runat="server" ID="userLiteral"></asp:Literal>
                         <table cellpadding="0" cellspacing="0" style="padding-top: 10px;">
                            <tr>
                                <td colspan="2" align="center" style="padding-bottom: 20px;">
                                    <label><asp:Label runat="server" ID="TheLabel"></asp:Label> </label>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                             <td colspan="2" align="center">
                             <table>
                                <tr>
                                    <td align="right" width="50%">
                                        <ctrl:BlueButton CLIENT_CLICK="Search()" runat="server" ID="BlueButton1" BUTTON_TEXT="Forget It" />
                                    </td>
                                    <td align="left" style="padding-left: 5px;">
                                        <ctrl:BlueButton runat="server" ID="BlueButton2" BUTTON_TEXT="Yes" />
                                    </td>
                                </tr>
                             </table>
                            </tr>
                         </table>
                  </asp:Panel>    
                          <asp:Panel runat="server" ID="ThankYouPanel" Visible="false">
                <div class="EventDiv" align="center" style="padding-top: 30px;">
                    <label>Your friend has been removed!</label>
                    <br /><br /><br /><br />
                    <ctrl:BlueButton runat="server" CLIENT_CLICK="Search('my-account')"  ID="BlueButton3" BUTTON_TEXT="Done" />


                </div>
            </asp:Panel>
                     </div>  
                             <asp:Literal runat="server" ID="InvisiblesLiteral"></asp:Literal>
         
    </form>
</body>
</html>
