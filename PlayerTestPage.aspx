<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlayerTestPage.aspx.cs" Inherits="PlayerTestPage" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <link href="~/StyleSheet.css" type="text/css" rel="stylesheet" />
</head>
<body style="background-color: #333333; scrollbar-base-color: #666666; scrollbar-shadow-color: #666666; scrollbar-highlight-color: #666666; scrollbar-darkshadow-color: #666666; scrollbar-3dlight-color: #666666; scrollbar-track-color: #999999; scrollbar-arrow-color: #363636;">
    <form id="form1" runat="server">
       <asp:ScriptManager runat="server"></asp:ScriptManager>  
<script type="text/javascript" src="Controls/swfobject.js"></script>
   
            <div id="flashPlayer" style="padding: 10px;">
              This text will be replaced by the flash music player.
            </div>

            <script type="text/javascript">
               var so = new SWFObject("Controls/playerMultipleList.swf", "mymovie", "200", "150", "3", "#FFFFFF");  
               so.addVariable("autoPlay","no")
               so.addVariable("playlistPath","Controls/PlayList.xml")
               so.write("flashPlayer");
            </script>    
            
      
            <asp:Label runat="server" ID="ErrorLabel"></asp:Label>
        
    </form>
</body>
</html>
