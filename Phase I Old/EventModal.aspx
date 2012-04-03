<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EventModal.aspx.cs" Inherits="EventModal" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="~/StyleSheet.css" type="text/css" rel="stylesheet" />
</head>
<body style="background-color: #333333; scrollbar-base-color: #666666; scrollbar-shadow-color: #666666; scrollbar-highlight-color: #666666; scrollbar-darkshadow-color: #666666; scrollbar-3dlight-color: #666666; scrollbar-track-color: #999999; scrollbar-arrow-color: #363636;">
    <form id="form1" runat="server">
         <asp:ScriptManager runat="server"></asp:ScriptManager>
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
                function CloseWindow()
                {
                
                    var oWindow = GetRadWindow(); 
                    oWindow.Close(); 
                }
              
                function Search() 
                { 
                    CloseWindow();
                } 
                
               
                
              
         </script>
   <rad:RadRotator runat="server" ScrollDuration="200" ID="Rotator1" ItemHeight="250" ItemWidth="400"  
                        Width="440px" Height="250px" RotatorType="Buttons">
                            
                            <%--<ItemTemplate>
                                <div style="border: solid 2px blue; padding: 3px; margin: 3px;">
                                    <img width="184px" height="184px" src='<%# Container.DataItem %>' />
                                </div>            
                            </ItemTemplate>--%>
      
          
                  </rad:RadRotator>
                  <img align="right" onclick="Search();" id="CloseButton" src="image/CancelButton.png" 
             onmouseover="this.src='image/CancelButtonSelected.png'" 
             onmouseout="this.src='image/CancelButton.png'" />
    </form>
</body>
</html>
