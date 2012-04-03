<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StartThread.aspx.cs" Inherits="StartThread" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
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
            function OnClientClose(oWnd, args)
            {
                window.location = args;
            }
            function OpenFriends()
                {
                    var win = $find("<%=RadWindow1.ClientID %>");
                    win.setUrl('SelectFriendMembers.aspx');
                    win.show(); 
                    win.center(); 
                 }
                 function OpenMembers()
                {
                    var win = $find("<%=RadWindow1.ClientID %>");
                    win.setUrl('SearchMembers.aspx');
                    win.show(); 
                    win.center(); 
                 }
            function FillFriends(retV, returnValue)
                { 
                   __doPostBack('MemberInvitesButton', '');
                }
            </script>
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
    
    <asp:Panel ID="RemovePanel" runat="server">  
        <div style="float: left;"><h1 class="Blue">Start a new Discussion Thread</h1></div>

        <div class="topDiv" style="padding-left: 30px; clear: both; padding-top: 10px;">
            <table>
                <tr>
                    <td>
                        <label>Thread Subject: </label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="SubjectTextBox" Width="300px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>Post the first comment [Not Required]:</label>
                    </td>
                    <td>
                        <asp:TextBox TextMode="MultiLine" Height="200px" runat="server" ID="CommentTextBox" Width="300px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>Include Picture/Video [Not Required]:</label>
                    </td>
                    <td>
                        <asp:RadioButtonList runat="server" ID="VidPicRadioList" AutoPostBack="true" 
                        OnSelectedIndexChanged="ShowVidPic" RepeatDirection="Horizontal">
                            <asp:ListItem Text="Picture" Value="1"></asp:ListItem>
                            <asp:ListItem Text="YouTube Video" Value="2"></asp:ListItem>
                        </asp:RadioButtonList>
                            <asp:Panel runat="server" ID="YouTubePanel" Visible="False">
                                <label>Insert the ID of the YouTube video:</label><br />
                                        
                                    <div class="topDiv" style="clear: both;">
                                       <label> http://www.youtube.com/watch?v=<span style="color: Red;">YMLTwq1M2pw</span>  <--This is the ID</label>
                                        <div style="float: left; padding-top: 4px;">
                                            <asp:TextBox runat="server" ID="YouTubeTextBox"></asp:TextBox>
                                        </div>
                                        <div style="float: left;">
                                        <asp:Button runat="server" ID="ImageButton7" Text="Upload!" 
                                             CssClass="SearchButton" 
                                             OnClick="YouTubeUpload_Click"
                                             onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                             onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                             onclientclick="this.value = 'Working...';" 
                                             />
<%--                                                                    <asp:ImageButton runat="server" ID="ImageButton7"  
                                                        OnClick="YouTubeUpload_Click"
                                                 ImageUrl="image/MakeItSoButton.png" 
                                                onmouseout="this.src='image/MakeItSoButton.png'" 
                                                onmouseover="this.src='image/MakeItSoButtonSelected.png'"  />
--%>                                                                </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel runat="server" ID="PicturePanel" Visible="false">
                                    <div id="topDiv2">
                                        <div style="float: left; padding-top: 4px;">
                                            <asp:FileUpload runat="server" ID="PictureUpload" Width="230px" EnableViewState="true" />
                                        </div>
                                        <div style="float: left;">
                                        <asp:Button runat="server" ID="ImageButton5" Text="Upload!" 
                                             CssClass="SearchButton" 
                                             OnClick="PictureUpload_Click"
                                             onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                             onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                             onclientclick="this.value = 'Working...';" 
                                             />
<%--                                                                    <asp:ImageButton runat="server" ID="ImageButton5"  OnClick="PictureUpload_Click"
                                             ImageUrl="image/MakeItSoButton.png"
                                            onmouseout="this.src='image/MakeItSoButton.png'" onmouseover="this.src='image/MakeItSoButtonSelected.png'"  />
--%>                                                                </div>
                                        
                                    </div>
                                </asp:Panel>
                                <asp:Panel runat="server" ID="PicsNVideosPanel" Visible="false">
                                        <br />
                                        <label>Your Picture or Video </label><br />
                                            <div>
                                                <div style="float: left; padding-right: 20px;">
                                                    <asp:CheckBoxList CssClass="MusicCheckBoxes" ID="PictureCheckList" runat="server"></asp:CheckBoxList>
                                                 
                                                    <asp:ImageButton runat="server" OnClick="SliderNixIt" ID="PictureNixItButton" ImageUrl="image/NixItButton.png" 
                                                    onmouseout="this.src='image/NixItButton.png'" onmouseover="this.src='image/NixItButtonSelected.png'"  />
                                               </div>
                                                
                                            </div>
                                            </asp:Panel>
                        <asp:Label runat="server" ID="MessageLabel" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
            
       </div>
        <div class="EventDiv" align="center" style="padding-top: 10px;">
        <asp:Button OnClick="PostThread" runat="server" ID="Button3" 
                            Text="Post Thread" CssClass="SearchButton" 
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
    <asp:Panel runat="server" ID="ThankYouPanel" Visible="false">
        <div class="EventDiv" align="center" style="padding-top: 30px;">
            <label>Your thread has been posted to the group!</label>
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