<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InviteMembers.aspx.cs" Inherits="InviteMembers" %>
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
        <div style="float: left;"><h1 class="Blue">Search for users or Invite Friends to join your group</h1></div>
         <div style="float: left; padding-left: 10px; padding-top: 5px;" ><asp:Image CssClass="HelpImage" runat="server"  ID="Image2" ImageUrl="~/image/helpIcon.png"></asp:Image>
                                        <rad:RadToolTip ID="RadToolTip2" runat="server" Skin="Black" Width="200px" Height="200px" 
                                        ManualClose="true" ShowEvent="onclick" Position="MiddleRight" RelativeTo="Element" 
                                        TargetControlID="Image2">
                            <label>When you invite members, a message will be sent to the users you list here asking them to accept this membership.
                            If you have invited these users before, they will still get another invite now.</label>
                            </rad:RadToolTip></div>
        <div class="topDiv" style="padding-left: 30px; clear: both; padding-top: 100px;">
            
            <div style="float: left; clear: both;"><label onclick="OpenMembers()"><span style='color: #ff770d; font-weight: bold; cursor: pointer;'>search members </span><span style="color: White;">|</span></label>  <label onclick="OpenFriends()"><span style='color: #ff770d; font-weight: bold; cursor: pointer;'>invite from friends</span></label>
             </div>
             <div style="float: left;">
             <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true" UpdateMode="conditional">
             <Triggers>
                <asp:AsyncPostBackTrigger ControlID="MembersListBox" EventName="SelectedIndexChanged" />
                
            </Triggers>
                <ContentTemplate>
             <table>
                <tr>
                    <td valign="top">
                       
                                <asp:LinkButton runat="server" ID="MemberInvitesButton" OnClick="FillMemberInvites"></asp:LinkButton>
                            
                                <asp:ListBox AutoPostBack="true" OnSelectedIndexChanged="SelectTitle"
                                runat="server" Width="300px" Height="100px" ID="MembersListBox"></asp:ListBox>
                        <asp:Label runat="server" ID="ErrorLabelUpdate" ForeColor="red"></asp:Label>
                     
                        <asp:Button runat="server" ID="AddButton" Text="Remove" 
                         CssClass="SearchButton"
                         OnClick="RemoveMember"
                         onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                         onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                         onclientclick="this.value = 'Working...';" 
                         />
                    </td>
                    <td style="padding-left: 10px;" valign="top">
                        <div style="margin-top: -20px;">
                            <label>Select a member and assign them a title and description [if you so desire]</label><br />
                            <script type="text/javascript">
                                function CountCharsTMem(editor, e){
                                    
                                        var theDiv = document.getElementById("Span2");
                                        var theText = document.getElementById("<%=MemberTitleTextBox.ClientID %>");
                                        theDiv.innerHTML = "characters left: "+ (15 - theText.value.length).toString();
                                    
                                }
                            </script>
                                                    
                                                    
                            <label>Title</label><br />
                                                                                <span style="font-family: Arial; font-size: 10px; color: #ff770d; font-weight: bold;">max 15 characters
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <span style="color: Red;" id="Span2"></span></span><br />

                            <asp:TextBox runat="server" ID="MemberTitleTextBox"></asp:TextBox><br />
                            <script type="text/javascript">
                                                            
                                                        function CountCharsDMem(editor, e){
                                                            
                                                                var theDiv = document.getElementById("Span1");
                                                                var theText = document.getElementById("<%=MemberDescriptionTextBox.ClientID %>");
                                                                theDiv.innerHTML = "characters left: "+ (150 - theText.value.length).toString();
                                                            
                                                        }
                                                    </script>
                            <label>Description</label><br /><span style="font-family: Arial; font-size: 10px; color: #ff770d; font-weight: bold;">max 150 characters
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <span style="color: Red;" id="Span1"></span></span><br />
                            <asp:TextBox runat="server" ID="MemberDescriptionTextBox"></asp:TextBox><br />
                            <div style="clear: both;">
                            <div style="float: left; padding-top: 6px;"><asp:CheckBox runat="server" ID="SharedHostingCheckBox" Text="Share hosting" />
                            </div>
                             <div style="float: left;padding-left: 10px; padding-top: 5px;" ><asp:Image CssClass="HelpImage" runat="server"  ID="Image1" ImageUrl="~/image/helpIcon.png"></asp:Image>
                           <rad:RadToolTip ID="RadToolTip1" runat="server" Skin="Black" Width="200px" Height="200px" 
                            ManualClose="true" ShowEvent="onclick" Position="MiddleRight" RelativeTo="Element" 
                            TargetControlID="Image1">
                <label>By sharing hosting with this user, you give them privilages of editing the group page, posting new group events and
                inserting a sticky instant message to the group on the group and event pages. As a creator of this group, you already have these privilages.</label>
                </rad:RadToolTip></div></div>
                            <div style="float: left; clear: both;">
                                <asp:Button runat="server" ID="Button2" Text="Assign" 
                                 CssClass="SearchButton"
                                 OnClick="AssignTitle"
                                 onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                 onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                 onclientclick="this.value = 'Working...';" 
                                 />
                                 <br />
                                  <asp:Label runat="server" Font-Size="11px" ID="MembererrorLabel" ForeColor="red"></asp:Label>
                             </div>
                         </div>
                    </td>
                </tr>
                
            </table>
            
           
                </ContentTemplate>
             </asp:UpdatePanel>
             </div>
       </div>
        <div class="EventDiv" align="center" style="padding-top: 50px;">
        <asp:Button OnClick="CreateMembers" runat="server" ID="Button3" 
                            Text="Invite" CssClass="SearchButton" 
         onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
        onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" /> 
               <asp:Button OnClientClick="Search();" runat="server" ID="Button1" 
                            Text="Cancel" CssClass="SearchButton" 
         onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
        onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" />   
        <asp:Label runat="server" ID="ErrorLabel" ForeColor="red"></asp:Label>
        </div>    
    </asp:Panel>    
    <asp:Panel runat="server" ID="ThankYouPanel" Visible="false">
        <div class="EventDiv" align="center" style="padding-top: 30px;">
            <label>Your invites have been sent!</label>
            <br /><br /><br /><br />
            <asp:ImageButton runat="server" ID="ImageButton9" 
            ImageUrl="~/image/DoneSonButton.png" OnClientClick="Search();" 
            onmouseout="this.src='../image/DoneSonButton.png'" 
            onmouseover="this.src='../image/DoneSonButtonSelected.png'"  />
        </div>
    </asp:Panel>
                     </div>  
                             <asp:Literal runat="server" ID="InvisiblesLiteral"></asp:Literal>
         
    </form>
</body>
</html>
