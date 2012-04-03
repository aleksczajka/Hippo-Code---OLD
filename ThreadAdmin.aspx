<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ThreadAdmin.aspx.cs" Inherits="ThreadAdmin" %>
<%@ OutputCache Duration="1" NoStore="true" VaryByParam="*" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="~/StyleSheet.css" type="text/css" rel="stylesheet" />
</head>
<body style="background-color: #333333; scrollbar-base-color: #666666; scrollbar-shadow-color: #666666; scrollbar-highlight-color: #666666; scrollbar-darkshadow-color: #666666; scrollbar-3dlight-color: #666666; scrollbar-track-color: #999999; scrollbar-arrow-color: #363636;">
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
         <rad:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="false" 
                KeepInScreenBounds="true" VisibleOnPageLoad="false"  Behaviors="none"
            ID="MessageRadWindowManager" ReloadOnShow="true" Modal="true" RestrictionZoneID="RestrictionZone"
            runat="server">
                <Windows>
                    <rad:RadWindow Width="530" ClientCallBackFunction="FillFriends" EnableEmbeddedSkins="true" 
                    VisibleStatusbar="false" Skin="Black" Height="500" ID="RadWindow1" Title="Alert" runat="server">
                    </rad:RadWindow>
                </Windows>
            </rad:RadWindowManager>
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
    
    <asp:Panel ID="InactivatePanel" runat="server">  
        <div style="float: left;"><h1 class="Blue">Thread Admin</h1></div>

        <div class="topDiv" style="padding-left: 30px; clear: both; padding-top: 10px;">
            <table>
                <tr>
                    <td>
                        <label>By inactivating a thread you are preventing anyone from posting anymore comments to the thread.
                        However, the thread will still be viewable. </label>
                    </td>
                </tr>
            </table>
            
       </div>
        <div class="EventDiv" align="center" style="padding-top: 50px;">
        <asp:Button OnClick="InactivateThread" runat="server" ID="Button3" 
                            Text="Inactivate" CssClass="SearchButton" 
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
    <asp:Panel ID="ActivePanel" runat="server">  
        <div style="float: left;"><h1 class="Blue">Thread Admin</h1></div>

        <div class="topDiv" style="padding-left: 30px; clear: both; padding-top: 10px;">
            <table>
                <tr>
                    <td>
                        <label>By making the thread active, you are allowing all members to post comments to it. </label>
                    </td>
                </tr>
            </table>
            
       </div>
        <div class="EventDiv" align="center" style="padding-top: 50px;">
        <asp:Button OnClick="ActivateThread" runat="server" ID="Button7" 
                            Text="Activate" CssClass="SearchButton" 
         onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
        onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" /> 
               <asp:Button OnClientClick="Search();" runat="server" ID="Button8" 
                            Text="Cancel" CssClass="SearchButton" 
         onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
        onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" /> 
        <br />  
        <asp:Label runat="server" ID="Label3" ForeColor="red" Font-Bold="true"></asp:Label>
        </div>    
    </asp:Panel>   
    <asp:Panel ID="HidePanel" runat="server">  
        <div style="float: left;"><h1 class="Blue">Thread Admin</h1></div>

        <div class="topDiv" style="padding-left: 30px; clear: both; padding-top: 10px;">
            <table>
                <tr>
                    <td>
                        <label>By hiding a thread you are removing it from the list of group's threads for any members.
                        However, it will still be viewable to each host.
                        You will be able to 'show' the thread at a later date.</label>
                    </td>
                </tr>
            </table>
            
       </div>
        <div class="EventDiv" align="center" style="padding-top: 50px;">
        <asp:Button OnClick="HideThread" runat="server" ID="Button2" 
                            Text="Hide" CssClass="SearchButton" 
         onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
        onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" /> 
               <asp:Button OnClientClick="Search();" runat="server" ID="Button4" 
                            Text="Cancel" CssClass="SearchButton" 
         onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
        onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" /> 
        <br />  
        <asp:Label runat="server" ID="Label1" ForeColor="red" Font-Bold="true"></asp:Label>
        </div>    
    </asp:Panel> 
    <asp:Panel ID="ShowPanel" runat="server">  
        <div style="float: left;"><h1 class="Blue">Thread Admin</h1></div>

        <div class="topDiv" style="padding-left: 30px; clear: both; padding-top: 10px;">
            <table>
                <tr>
                    <td>
                        <label>By showing the thread you are making it visible to all members of the group.</label>
                    </td>
                </tr>
            </table>
            
       </div>
        <div class="EventDiv" align="center" style="padding-top: 50px;">
        <asp:Button OnClick="ShowThread" runat="server" ID="Button9" 
                            Text="Show" CssClass="SearchButton" 
         onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
        onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" /> 
               <asp:Button OnClientClick="Search();" runat="server" ID="Button10" 
                            Text="Cancel" CssClass="SearchButton" 
         onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
        onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" /> 
        <br />  
        <asp:Label runat="server" ID="Label4" ForeColor="red" Font-Bold="true"></asp:Label>
        </div>    
    </asp:Panel>  
    <asp:Panel ID="DeletePanel" runat="server">  
        <div style="float: left;"><h1 class="Blue">Thread Admin</h1></div>

        <div class="topDiv" style="padding-left: 30px; clear: both; padding-top: 10px;">
            <table>
                <tr>
                    <td>
                        <label>By deleting the thread, you are removing it from the site permanently. </label>
                    </td>
                </tr>
            </table>
            
       </div>
        <div class="EventDiv" align="center" style="padding-top: 50px;">
        <asp:Button OnClick="DeleteThread" runat="server" ID="Button5" 
                            Text="Delete" CssClass="SearchButton" 
         onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
        onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" /> 
               <asp:Button OnClientClick="Search();" runat="server" ID="Button6" 
                            Text="Cancel" CssClass="SearchButton" 
         onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
        onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" /> 
        <br />  
        <asp:Label runat="server" ID="Label2" ForeColor="red" Font-Bold="true"></asp:Label>
        </div>    
    </asp:Panel> 
    <asp:Panel runat="server" ID="ThankYouPanel" Visible="false">
        <div class="EventDiv" align="center" style="padding-top: 30px;">
            <label><asp:Label runat="server" ID="ThankYouLabel"></asp:Label></label>
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
