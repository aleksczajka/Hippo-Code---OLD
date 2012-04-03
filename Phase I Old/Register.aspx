<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Register" Title="Register on Hippo Happenings" %>
<%@ Register TagName="HippoTextBox" TagPrefix="ctrl" Src="~/Controls/HippoTextBox.ascx" %>
<%@ Register Src="~/Controls/Ads.ascx" TagName="Ads" TagPrefix="ctrl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

 <rad:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="false" 
                KeepInScreenBounds="true" VisibleOnPageLoad="false"  Behaviors="none"
            ID="MessageRadWindowManager" ReloadOnShow="true" Modal="true" DestroyOnClose="true" 
            RestrictionZoneID="RestrictionZone"
            runat="server">
                <Windows>
                    <rad:RadWindow Width="300" ClientCallBackFunction="OnClientClose" EnableEmbeddedSkins="true" 
                    VisibleStatusbar="false" Skin="Black" Height="200" ID="MessageRadWindow" Title="Your email has been sent!" runat="server">
                    </rad:RadWindow>
                </Windows>
            </rad:RadWindowManager>
             <rad:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="true" 
    KeepInScreenBounds="true" VisibleOnPageLoad="false" Behaviors="Close"
ID="RadWindowManager1" BackColor="#1b1b1b" Modal="true" DestroyOnClose="false" 
RestrictionZoneID="RestrictionZone"
runat="server">
    <Windows>
        <rad:RadWindow Width="730"  ReloadOnShow="true" IconUrl="./image/AddIcon.png"  VisibleTitlebar="false"
        EnableEmbeddedSkins="true" Skin="Office2007" Height="550" VisibleStatusbar="false" 
        ID="RadWindow1" Title="Add to Favorites" runat="server">
        </rad:RadWindow>
    </Windows>
</rad:RadWindowManager>   
            <script type="text/javascript">
            function OnClientClose(oWnd, args)
            {

                window.location = args;
            }
           function OpenRad()
        {
            var win = $find("<%=RadWindow1.ClientID %>");
            win.setUrl('TermsAndConditionsOpen.aspx');
            win.show(); 
            win.center(); 
               
         }
            </script>
 <div id="topDiv2" style="width: 870px;">
    <div id="topDiv" class="RegisterDiv" style="float: left; width: 440px;"><span style="font-family: Arial; font-size: 30px; color: White;">Create an account</span><br /><br />
         
      <div id="topDiv3" style="clear:both;">
          <div style="float: left;">
          <div style="float: left;padding-right: 105px;padding-bottom: 5px;" align="center">
              <label>Enter an email address for verification. 
              You will receive an email with a link to set up your account.</label>
          </div>
              <br /><br />
                <div style="float: left; width: 170px;">
                    <label> <b >Email</b></label> <br />
                    <ctrl:HippoTextBox runat="server" ID="EmailTextBox" TEXTBOX_WIDTH="150" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                </div>
                <div style="float: left; width: 170px;">
                    <label> <b>Confirm Email</b> </label><br />
                    <ctrl:HippoTextBox runat="server" ID="ConfirmEmailTextBox" TEXTBOX_WIDTH="150" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" /><br />
                </div>
         </div>
          <div style="float: left; padding-left: 20px;"><asp:Label runat="server" CssClass="ErrorLabel" ID="MessageLabel"></asp:Label></div>
      </div>
      <div style="width: 440px;">
          <br />  
          <asp:UpdatePanel runat="server">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="CheckImageButton" EventName="Click" />
            </Triggers>
            <ContentTemplate>
                <asp:ImageButton runat="server" ID="CheckImageButton" ImageUrl="image/Check.png" OnClick="ChangeCheckImage" /><label> I have read and agree to the <a class="AddLink" onclick="OpenRad();">terms and conditions</a></label>
            </ContentTemplate>
          </asp:UpdatePanel>
          
          <div align="right"><asp:ImageButton runat="server" ID="MakeItSoButton" ImageUrl="~/image/MakeItSoButton.png" onmouseout="this.src='image/MakeItSoButton.png'" onmouseover="this.src='image/MakeItSoButtonSelected.png'" OnClick="MakeItSo" /></div>
      </div><br /><br />
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

</asp:Content>

