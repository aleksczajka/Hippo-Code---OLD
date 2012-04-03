<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InviteFriendAlert.aspx.cs" Inherits="InviteFriendAlert" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ OutputCache Duration="1" NoStore="true" VaryByParam="*" %>
<%--
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xmlns:fb="http://www.facebook.com/2008/fbml">


<head runat="server">
    <link href="~/InviteFriendsStyleSheet.css" type="text/css" rel="stylesheet" />
    
</head>
<body style="background-color: #333333; scrollbar-base-color: #666666; scrollbar-shadow-color: #666666; scrollbar-highlight-color: #666666; scrollbar-darkshadow-color: #666666; scrollbar-3dlight-color: #666666; scrollbar-track-color: #999999; scrollbar-arrow-color: #363636;">
    
    <form id="form1" runat="server" method="post">
    
     <script src="http://static.ak.connect.facebook.com/js/api_lib/v0.4/XdCommReceiver.js"  
         type="text/javascript"></script>  
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
                    oWindow.Close("null"); 
                }
              
                function Search() 
                { 
                    CloseWindow();
                } 
                
               function AddFriend(num)
               {
                    var numDiv = document.getElementById('div'+num);
                    numDiv.style.display = 'inline';
                    var numA = document.getElementById('a'+num);
                    numA.style.display = 'none';
                    AddFriendAlert.SendIt(num);
               }
               
               
                
                
                
                
              
         </script>
         
           <asp:ScriptManager runat="server" ID="ScriptManager1" EnablePageMethods="true" ></asp:ScriptManager>  
         
    <div class="MessageDiv" align="center">
        <div>
        <div style="float: right;"> <a class="AddLink" style="text-decoration: underline;" onclick="Search();">close</a></div>
            <asp:Label runat="server" ID="Infolabel" Visible="false"></asp:Label>
                <h1>Invite friends to join Hippo Happenings </h1> 
                
                 <rad:RadPanelBar runat="server" ID="SearchPanel" ExpandAnimation-Type="Linear" CollapseAnimation-Type="Linear" 
                 CollapseAnimation-Duration="1000" 
                  AllowCollapseAllItems="true" 
                 Width="740px"  ExpandMode="SingleExpandedItem">
            <Items>
                <rad:RadPanelItem Text="<span style='font-family: Arial; font-size: 20px; color: White;'>Using Friend's Email</span>">
                    <Items>
                        <rad:RadPanelItem runat="server" Expanded="false" Width="400px">
                            <Items>
                                <rad:RadPanelItem Width="400px"></rad:RadPanelItem>
                            </Items>
                            <ItemTemplate>
                                <div style="background-color: #333333;">
                               <h4>Include their emails:</h4><br />
                                <asp:Panel runat="server" ID="FriendPanel">
                                </asp:Panel><br />
                                <asp:LinkButton runat="server" ID="sadf" CssClass="AddLink" OnClick="AddMore" Text="Add More Emails"></asp:LinkButton><br /><br />
                                <asp:ImageButton runat="server" OnClick="SendIt" ID="SendButton" ImageUrl="~/image/SendItButton.png" onmouseover="this.src='image/SendItButtonSelected.png'" onmouseout="this.src='image/SendItButton.png'" />
                                <br />
                                <asp:Label runat="server" ID="MessageLabel" CssClass="AddGreenLink"></asp:Label>
                                         </div>
                                         
                                            </ItemTemplate>
                                        </rad:RadPanelItem>
                                    </Items>
                          
                </rad:RadPanelItem>
                <rad:RadPanelItem Width="740px" Text="<div style='position: relative;'><span style='font-family: Arial; font-size: 20px; color: White;'>Using Facebook </span></div>">
                    <Items>
                        <rad:RadPanelItem runat="server" Expanded="false" Width="740px">
                            <Items>
                                <rad:RadPanelItem ></rad:RadPanelItem>
                            </Items>
                            <ItemTemplate>
                                <div id="user" style="width: 740px; background-color: #333333;padding-bottom: 10px;">
                                   <label class="AddLink">Please </label> <fb:login-button length="long" onlogin="update_user_box();"></fb:login-button> <label class="AddLink">to invite your Facebook friends </label>
                                  </div>
                            </ItemTemplate>
                        </rad:RadPanelItem>
                    </Items>
                    
                </rad:RadPanelItem>
            </Items>
        </rad:RadPanelBar>
                
                
          </div>
    </div>
    

     </form>   

        
    <script type="text/javascript">
function update_user_box() {

          var user_box = document.getElementById("user");

          // add in some XFBML. note that we set useyou=false so it doesn't display "you"
          user_box.innerHTML =
            "<fb:serverfbml style='width: 740px;'>"+
            "<script type='text/fbml'>"+
            "<fb:fbml>"+
            "<fb:request-form action='http://hippohappenings.com/User.aspx'"+
            " method='POST' invite='true' type='HippoHappenings' "+
            " content=\"is a member "+
            "of HippoHappenings.com and would like to share that experience with you.  "+
            "To register, simply click on the 'Register' button below.<fb:req-choice "+
            "url='http://hippohappenings.com/Register.aspx' label='Register' />\" > "+
            " <fb:multi-friend-selector showborder='false'"+
            " actiontext='Invite your Facebook Friends to use HippoHappenings' /> "+
            " </fb:request-form>"+
            "</fb:fbml>"+
            "<\/script>"+
            "</fb:serverfbml>";

//              "<span>"
//            + "<fb:profile-pic uid='loggedinuser' facebook-logo='true'></fb:profile-pic>"
//            + "Welcome, <fb:name uid='loggedinuser' useyou='false'></fb:name>. You are signed in with your Facebook account."
//            + "</span>";

          // because this is XFBML, we need to tell Facebook to re-process the document 
          FB.XFBML.Host.parseDomTree();
        }
    </script>
   
             <script src="http://static.ak.connect.facebook.com/js/api_lib/v0.4/FeatureLoader.js.php"  
      type="text/javascript"></script>  
      <script type="text/javascript">FB.init("7538b5fd2033733a9c90fe5f414bbdd3","xd_receiver.htm",{"ifUserConnected" : update_user_box});</script>
   
</body>
</html>
