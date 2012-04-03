<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TermsAndConditionsOpen.aspx.cs" 
Inherits="TermsAndConditionsOpen" Title="Hippo Happenings Terms And Conditions" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
        <link href="~/StyleSheet.css" type="text/css" rel="stylesheet" />

</head>
<body>
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
                    oWindow.Close(); 
                }
              
                function Search() 
                { 
                    CloseWindow();
                } 
</script>
                    <a class="NavyLink" onclick="Search();" style="clear: both;text-decoration: underline; float: right;">close</a>
 <div class="EventDiv" style="line-height: 20px; padding-left: 50px; padding-top: 30px; padding-right: 50px;" >
<asp:Literal runat="server" ID="testLit"></asp:Literal>

    </div>
</body>
</html>

