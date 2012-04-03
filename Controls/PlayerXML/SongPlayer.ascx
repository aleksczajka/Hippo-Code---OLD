<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SongPlayer.ascx.cs" Inherits="Controls_SongPlayer" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register Src="~/Controls/PlayerControl.ascx" TagName="Player" TagPrefix="ctrl" %>
<div style="padding-bottom: 20px;">
    
   <script type="text/javascript" src="Controls/swfobject.js"></script>

<a href="#" class="NavyLink12" id="txtLink" onclick="return false;"><b>Listen to Music</b></a>
    <rad:RadToolTip ID="ToolTip1" BackColor="Red" runat="server" ManualClose="true" TargetControlID="txtLink"
     Animation="Fade" ShowDelay="100" RelativeTo="element" Skin="Sunset"
     AutoCloseDelay="10000" Position="middleright" IsClientID="true" ShowEvent="OnClick">
		<div id="flashPlayer" style="padding: 10px;">
              This text will be replaced by the flash music player.
            </div>
            <script type="text/javascript">
             
                   var so = new SWFObject("Controls/PlayerXML/playerMultipleList.swf", "mymovie", "200", "150", "7", "#FFFFFF");  
                   so.addVariable("autoPlay","no") 
                   so.addVariable("playlistPath","Controls/PlayerXML/"+getCookie('PlayerXML')); 
                   so.write("flashPlayer"); 
                  
            </script> 
            <asp:Label runat="server" ID="ErrorLabel"></asp:Label>
    </rad:RadToolTip> 

    

</div>