<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PlayerControl.ascx.cs" 
Inherits="Controls_PlayerControl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<script type="text/javascript" src="Controls/swfobject.js"></script>
<script type="text/javascript">
              function DoIt(){
                   var so = new SWFObject("Controls/playerMultipleList.swf", "mymovie", "200", "150", "7", "#FFFFFF");  
                   so.addVariable("autoPlay","no") 
                   so.addVariable("playlistPath","Controls/PlayList.xml") 
                   so.write("flashPlayer"); 
               }    
            </script> 
<a href="#" class="AddLink" id="txtLink" onclick="return false;">Listen to Music</a>
    <rad:RadToolTip ID="ToolTip1" BackColor="Red" runat="server" ManualClose="true" TargetControlID="txtLink"
     Animation="Fade" ShowDelay="100" RelativeTo="element" Skin="Sunset"
     AutoCloseDelay="10000" Position="middleright" OnClientBeforeShow="DoIt()" IsClientID="true" ShowEvent="OnClick">
		<div id="flashPlayer" style="padding: 10px;">
              This text will be replaced by the flash music player.
            </div>
            <asp:Label runat="server" ID="ErrorLabel"></asp:Label>
    </rad:RadToolTip> 
