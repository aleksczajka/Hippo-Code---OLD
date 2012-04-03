<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RevokeMembership.aspx.cs" Inherits="RevokeMembership" %>
<%@ OutputCache Duration="1" NoStore="true" VaryByParam="*" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="~/StyleSheet.css" type="text/css" rel="stylesheet" />
</head>
<body style="background-color: #333333; scrollbar-base-color: #666666; scrollbar-shadow-color: #666666; scrollbar-highlight-color: #666666; scrollbar-darkshadow-color: #666666; scrollbar-3dlight-color: #666666; scrollbar-track-color: #999999; scrollbar-arrow-color: #363636;">
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
    background-color: #333333; color: #666666;height: 100px; padding: 10px;">   
    
    <asp:Panel ID="RemovePanel" runat="server">  
                        <asp:Literal runat="server" ID="userLiteral"></asp:Literal>
                         <table cellpadding="0" cellspacing="0" style="padding-top: 10px;">
                            <tr>
                                <td colspan="2" align="center" style="padding-bottom: 20px;">
                                    <label><asp:Label runat="server" Text=" Are you sure you want to remove yourself from this group?" ID="TheLabel"></asp:Label> </label>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                             <td colspan="2" align="center">
                              <asp:Button runat="server" ID="Button3" Text="Yes" CssClass="SearchButton" 
            OnClick="RemoveMember" onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" />
                                <%--<asp:ImageButton runat="server" ID="ImageButton2" OnClick="DeleteEventAction"
                                        ImageUrl="~/image/SendItButton.png" onmouseout="this.src='../image/SendItButton.png'"
                                        onmouseover="this.src='../image/SendItButtonSelected.png'" />--%>
                            
<asp:Button runat="server" ID="Button1" Text="Forget It!" CssClass="SearchButton" 
             OnClientClick="Search();"  onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" />                          </td>
                                    
                                
                            </tr>
                         </table>
                  </asp:Panel>    
                          <asp:Panel runat="server" ID="ThankYouPanel" Visible="false">
                <div class="EventDiv" align="center" style="padding-top: 30px;">
                    <label>Your event has been deleted!</label>
                    <br /><br /><br /><br />
                    <asp:ImageButton runat="server" ID="ImageButton9" 
                    ImageUrl="~/image/DoneSonButton.png" OnClientClick="Search('my-account');" 
                    onmouseout="this.src='../image/DoneSonButton.png'" 
                    onmouseover="this.src='../image/DoneSonButtonSelected.png'"  />

                </div>
            </asp:Panel>
                     </div>  
                             <asp:Literal runat="server" ID="InvisiblesLiteral"></asp:Literal>
         
    </form>
</body>
</html>
