<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JoinUs.aspx.cs" Inherits="JoinUs" %>
<%@ OutputCache NoStore="true" Duration="1" VaryByParam="none" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>

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
                function CloseWindow(variable)
                {
                
                    var oWindow = GetRadWindow(); 
                    oWindow.Close(variable); 
                }
              
                function Search(variable) 
                { 
                        CloseWindow(variable);
                } 
         </script>
    <div align="center" class="EventDiv radconfirm" style="font-family: Arial; font-size: 12px; 
    background-color: #333333; color: #666666;height: 100px; padding: 10px;">   
   <asp:Panel runat="server" ID="NotSignedInPanel">
    <div class="EventDiv" align="center" style="padding-top: 30px;">
            <label>You must be signed in to join this group. We'll take you there.</label>
            <br /><br /><br /><br />
            <asp:Button OnClientClick="Search('UserLogin.aspx');" runat="server" ID="Button2" 
                            Text="Let's go" CssClass="SearchButton" 
         onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
        onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" /> 
               <asp:Button OnClientClick="Search();" runat="server" ID="Button4" 
                            Text="Cancel" CssClass="SearchButton" 
         onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
        onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" /> 
        </div>
   </asp:Panel> 
    <asp:Panel ID="RemovePanel" runat="server">  
        <div style="float: left;"><h1 class="Blue">Request to join the group</h1></div>

        <div class="topDiv" style="padding-left: 30px; clear: both; padding-top: 10px;">
            <table>
                <tr>
                    <td>
                        <label>Your Message to the Host: </label><div style="float: left; padding-left: 10px; padding-top: 5px;" ><asp:Image CssClass="HelpImage" runat="server"  ID="Image2" ImageUrl="~/image/helpIcon.png"></asp:Image>
                                        <rad:RadToolTip ID="RadToolTip2" runat="server" Skin="Black" Width="200px" Height="200px" 
                                        ManualClose="true" ShowEvent="onclick" Position="MiddleRight" RelativeTo="Element" 
                                        TargetControlID="Image2">
                                        <label>
                                            Tell the host why you want to join the group and who you are. A message will be sent
                                            to the host for them to approve or reject your request.
                                        </label>
                            </rad:RadToolTip></div>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="SubjectTextBox" Height="100px" TextMode="MultiLine" Width="300px"></asp:TextBox>
                    </td>
                </tr>
            </table>
            
       </div>
        <div class="EventDiv" align="center" style="padding-top: 50px;">
        <asp:Button OnClick="PostThread" runat="server" ID="Button3" 
                            Text="Send it" CssClass="SearchButton" 
         onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
        onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" /> 
               <asp:Button OnClientClick="Search();" runat="server" ID="Button1" 
                            Text="Cancel" CssClass="SearchButton" 
         onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
        onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" /> 
        <br />  
        <asp:Label runat="server" ID="ErrorLabel" ForeColor="red" Font-Bold="true"></asp:Label>
        </div>    
    </asp:Panel>   
    <asp:Panel ID="AutomaticPanel" runat="server" Visible="false">  
        <div style="float: left;"><h1 class="Blue">Join the group you want?</h1></div>

        <div class="EventDiv" align="center" style="padding-top: 50px;">
        <asp:Button OnClick="AutomaticJoin" runat="server" ID="Button5" 
                            Text="Yes!" CssClass="SearchButton" 
         onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
        onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" /> 
               <asp:Button OnClientClick="Search();" runat="server" ID="Button6" 
                            Text="Cancel" CssClass="SearchButton" 
         onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
        onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" /> 
        <br />  
        <asp:Label runat="server" ID="Label1" ForeColor="red" Font-Bold="true"></asp:Label>
        </div>    
    </asp:Panel>  
    <asp:Panel runat="server" ID="ThankYouPanel" Visible="false">
        <div class="EventDiv" align="center" style="padding-top: 30px;">
            <label><asp:Label runat="server" ID="ThankYouLabel" Text="Your request has been sent!"></asp:Label></label>
            <br /><br /><br /><br />
            <asp:ImageButton runat="server" ID="ImageButton9" 
            ImageUrl="~/image/DoneSonButton.png"
            onmouseout="this.src='../image/DoneSonButton.png'" 
            onmouseover="this.src='../image/DoneSonButtonSelected.png'"  />
        </div>
    </asp:Panel>
                     </div>  
                             <asp:Literal runat="server" ID="InvisiblesLiteral"></asp:Literal>
         
    </form>
</body>
</html>
