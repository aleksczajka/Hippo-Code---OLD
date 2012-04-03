<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HelloHippoFriends.aspx.cs" Inherits="HelloHippoFriends" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Welcome From Hippo Happenings</title>
    <link type="text/css" href="StyleSheet.css" rel="stylesheet" />
</head>
<body style="padding: 0; margin: 0;">
    <form id="form1" runat="server" style="padding: 0; margin: 0;   ">
    <div align="center" style="background-color: #1b1b1b; height: 1000px; padding: 0; margin: 0; vertical-align: middle;">
            <img src="image/Logo.gif" />
            <br />
            <asp:Label CssClass="EventHeader" runat="server" >Welcome! Hippo Happenings is under construction and coming at ya soon!</asp:Label>
        
    </div>
    </form>
</body>
</html>
