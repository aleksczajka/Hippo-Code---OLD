<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
CodeFile="UserLogin.aspx.cs" Inherits="UserLogin" Title="Log in to HippoHappenings" %>
<%@ Register TagName="HippoTextBox" TagPrefix="ctrl" Src="~/Controls/HippoTextBox.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register TagName="Ads" TagPrefix="ctrl" Src="~/Controls/Ads.ascx" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript">
    function OpenRad()
        {
            var win = $find("<%=RadWindow1.ClientID %>");
            win.setUrl('PasswordAlert.aspx');
            win.show(); 
            win.center(); 
               
         }
</script>
 <asp:Panel runat="server" DefaultButton="SignInButton">
     <div id="topDiv2" style="width: 870px;">
        <div id="topDiv" class="RegisterDiv" style="float: left; width: 440px;"><span style="font-family: Arial; font-size: 30px; color: White;">Log in to your account</span><br /><br />
            <div style="font-family: Arial; font-size: 12px; color: #cccccc; line-height: 20px;">Log in to your account or <a href="Register.aspx" class="AddLink">Register</a> if you haven't already. 
          </div> <br />
          <div id="topDiv3" style="clear:both;">
              <div style="float: left;">
                    <div style="float: left; width: 170px;">
                    <label><b>User name</b></label> <br />
                    <asp:TextBox runat="server" ID="UserNameTextBox" Width="150px"></asp:TextBox>
                   </div>
                   <div style="float: left; width: 170px;">
                   <label><b>Password</b></label> <br />
                   <asp:TextBox runat="server" TextMode="password" ID="PasswordTextBox" Width="150px"></asp:TextBox>
                   </div>
                   <br />
                    <asp:Label runat="server" ID="StatusLabel" ForeColor="Red"></asp:Label>
              </div>
              <div style="float: left; padding-left: 20px; padding-top: 50px;"><asp:Label runat="server" CssClass="ErrorLabel" ID="MessageLabel"></asp:Label></div>
          </div>
          
          <div style="width: 440px;">
              <div align="right"><asp:ImageButton runat="server" ID="SignInButton" ImageUrl="~/image/LoginButton.png" onmouseout="this.src='image/LoginButton.png'" onmouseover="this.src='image/LoginButtonSelected.png'" OnClick="MakeItSo" /></div>
          </div>
          <div>
            <a class="AddLink" href="javascript:OpenRad();">Forgot your password?</a> 
          </div>
          <br /><br />
      <div style="font-family: Arial; font-size: 20px; color: #1FB6E7; line-height: 20px;">
            With a user account you can...
         </div>
         <ul type="disc" style="font-family: Arial; font-size: 12px; color: #A5C13A; 
            line-height: 18px;padding:0px;margin:0px;padding-left: 30px;">
            <li style="padding-top: 10px;">
                <span style="color: #cccccc;"><span style="font-size: 16px; font-weight: bold;">Post events:</span> yours or your favorite venue's</span>
                <ul type="disc" style="color: #1fb6e7;">
                    <li>
                        <span style="color: #cccccc;">connect with people going to public events, make new frieds, organize carpools</span>
                    </li>
                    <li>
                        <span style="color: #cccccc;">add events to your calendar, get updates if anything has changed for the event, get alert remminders</span>
                    </li>
                    <li>
                       <span style="color: #cccccc;">get event recommendations based on your favorite event categories, favorite venues and events similar to ones you have gone to</span>
                    </li>
                    <li>
                        <span style="color: #cccccc;">edit any existing event that does not have an owner associated</span>
                    </li>
                </ul>
            </li>
            <li style="padding-top: 10px;">
                <span style="color: #cccccc;"><span style="font-size: 16px; font-weight: bold;">Post local venues:</span> tell us which one's are your favorites</span>
                <ul type="disc"  style="color: #1fb6e7;">
                    <li>
                        <span style="color: #cccccc;">edit any venue that does not have an owner associated</span>
                    </li>
                    <li>
                        <span style="color: #cccccc;">rate venues</span>
                    </li>
                    <li>
                        <span style="color: #cccccc;">mark as your favorite and see what events are going on in your favorite venues right on your My Account page</span>
                    </li>
                </ul>
            </li>
            <li style="padding-top: 10px;">
                <span style="color: #cccccc;"><span style="font-size: 16px; font-weight: bold;">Post local advertisments:</span> lost pet, garage sale, or anything local</span>
                 <ul type="disc"  style="color: #1fb6e7;">
                    <li>
                        <span style="color: #cccccc;">have your ads target users in any location with the categories you choose</span>
                    </li>
                    <li>
                        <span style="color: #cccccc;">filter the ads you see based on your favorite categories</span>
                    </li>
                    <li>
                        <span style="color: #cccccc;">save ad searches which will email you a classified of your interest as it is posted on our site... no longer the need to re-search every day for new posted ads in your favorite categories</span>
                    </li>
                </ul>
            </li>
            <li style="padding-top: 10px;">
                <span style="color: #cccccc;"><span style="font-size: 16px; font-weight: bold;">Post groups:</span> connect with like minded individuals</span>
                <ul type="disc"  style="color: #1fb6e7;">
                    <li>
                        <span style="color: #cccccc;">set up group events which could be public, private or exclusive</span>
                    </li>
                    <li>
                        <span style="color: #cccccc;">have a message board for easier communication</span>
                    </li>
                    <li>
                        <span style="color: #cccccc;">have involved discussions with discussion threads</span>
                    </li>
                </ul>
            </li>
         </ul>
         <br />
         <span style="font-family: Arial; font-size: 12px; color: #cccccc; line-height: 20px;">
             ...and much more. See full functionality listings on our <a class="AddLink" href="About.aspx">About Page</a>.
                 <br /><br />HippoHappenings will <b>NEVER</b> sell, trade or share any of your information to anyone.
         </span>
        </div>
        <div style="float: right; padding-right: 2px; width: 419px;">
            <div style="clear: both;">
             <%--<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" 
            codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0" 
            width="431" height="575" title="banner" wmode="transparent">  
            <param name="movie" value="gallery.swf" />  
            <param name="quality" value="high" />
            <param name="wmode" value="transparent"/>   
                <embed src="gallery.swf" quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer" 
                type="application/x-shockwave-flash" width="431" height="575" wmode="transparent">
                </embed>
            </object>--%>
                <ctrl:Ads ID="Ads1" runat="server" />
            </div>
        </div>
      </div>  
      <rad:RadWindowManager Behaviors="none"
            ID="RadWindowManager" Modal="true" Skin="Black" Left="10px" DestroyOnClose="false" runat="server">
            <Windows>
                <rad:RadWindow ID="RadWindow1" VisibleStatusbar="false" 
                VisibleTitlebar="false" VisibleOnPageLoad="false"  Left="10px" Height="250" Width="430"
                     runat="server">
                </rad:RadWindow>
            </Windows>
   </rad:RadWindowManager>
</asp:Panel>
</asp:Content>

