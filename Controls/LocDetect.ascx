<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LocDetect.ascx.cs" Inherits="LocDetect" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<script src="js/TotalJS.js" type="text/javascript"></script>
<script type="text/javascript">timedout();</script> 
<div class="LocDetect">
<div style="padding-bottom: 10px;">
    <rad:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="false" 
                    KeepInScreenBounds="true" VisibleOnPageLoad="false"  Behaviors="none"
                ID="MessageRadWindowManager" ReloadOnShow="true" Modal="true"
                RestrictionZoneID="RestrictionZone"
                runat="server">
                    <Windows>
                        <rad:RadWindow Width="300"
                        EnableEmbeddedSkins="true"  VisibleTitlebar="false"
                        VisibleStatusbar="false" NavigateUrl="../Changelocation.aspx" Skin="Web20" 
                        Height="320" ID="MessageRadWindow" ClientCallBackFunction="CallBackFunctionOwner" 
                        Title="Alert" runat="server">

                        </rad:RadWindow>
                    </Windows>
                </rad:RadWindowManager> 
                
                <script type="text/javascript">
                    function OpenLocRad()
                    {
                        var win = $find("<%=MessageRadWindow.ClientID %>");
                        win.show(); 
                        win.center(); 
                    }
                    function CallBackFunctionOwner(radWindow, returnValue)
                    {
                        if(returnValue != null && returnValue != undefined)
                            window.location.reload();
                    }
                </script>
    <asp:Panel runat="server" ID="NotLoggedIn">
        <asp:Label ID="ErrorLabel" runat="server" ForeColor="Red"></asp:Label>
        <div class="topDiv" style="font-family: Arial; font-size: 18px;" align="center">
            We have detected your location as <asp:Label runat="server" ID="LocationLabel"></asp:Label>. 
            If this is not correct <a onclick="OpenLocRad()" class="NavyLink" style="text-decoration: underline;">change your location</a>, or 
            <a href="../login" class="NavyLink" style="text-decoration: underline;">log in</a> for a customized experience. 
        </div>
    </asp:Panel>
</div>
<div class="fblike">
        <div id="fb-root"></div><script src="http://connect.facebook.net/en_US/all.js#appId=229624373718810&amp;xfbml=1"></script><fb:like href="http://hippohappenings.com" send="true" layout="button_count" width="100" show_faces="true" font=""></fb:like>
    </div>
</div>