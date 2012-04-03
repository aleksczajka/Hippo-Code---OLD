<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddFriendAlert.aspx.cs" Inherits="AddFriendAlert" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <!--[if gte IE 6]><meta http-equiv="Page-Enter" content="BlendTrans(Duration=0.1)" /><![endif]-->
    <link href="~/StyleSheet.css" type="text/css" rel="stylesheet" />
</head>
<body style="background-color: #333333; scrollbar-base-color: #666666; scrollbar-shadow-color: #666666; scrollbar-highlight-color: #666666; scrollbar-darkshadow-color: #666666; scrollbar-3dlight-color: #666666; scrollbar-track-color: #999999; scrollbar-arrow-color: #363636;">
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server"></asp:ScriptManager>
<%-- <telerik:RadAjaxPanel runat="server" > --%>       
         <script type="text/javascript">
            function setCookie(c_name,value,expiredays)
            {
                var exdate=new Date();
                exdate.setDate(exdate.getDate()+expiredays);
                document.cookie=c_name+ "=" +escape(value)+
                ((expiredays==null) ? "" : ";expires="+exdate.toGMTString());
            }
            function getCookie(c_name)
            {
                if (document.cookie.length>0)
                  {
                  c_start=document.cookie.indexOf(c_name + "=");
                  if (c_start!=-1)
                    {
                    c_start=c_start + c_name.length+1;
                    c_end=document.cookie.indexOf(";",c_start);
                    if (c_end==-1) c_end=document.cookie.length;
                    return unescape(document.cookie.substring(c_start,c_end));
                    }
                  }
                return "";
            }
                function setScroll()
                {
                        document.getElementById('FriendsPanel').scrollTop = getCookie('scrollCookie');
                }
               
               function reSetScroll()
               {
                    setCookie('scrollCookie', 0, 3);
               }
               
               function getScroll()
               {
                    setCookie('scrollCookie', document.getElementById('FriendsPanel').scrollTop, 3);
               }
               
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
                    reSetScroll();
                    CloseWindow();
                } 
                
               function AddFriend(num)
               {
                    var numDiv = document.getElementById('div'+num);
                    numDiv.style.display = 'inline';
                    var numA = document.getElementById('a'+num);
                    numA.style.display = 'none';
                    AddFriendAlert.SendIt(num, callbackfunctionA);
               }
               
               function callbackfunctionA(num)
               {
                    var a = document.getElementById('messagesDiv');
                    a.innerHTML = num;
               }
         </script>
    <div class="MessageDiv" align="center">
   <%-- <asp:ScriptManager runat="server" ID="ScriptManager1" EnablePageMethods="true" ></asp:ScriptManager>
        <telerik:RadAjaxPanel runat="server">--%>
            <div><div id="messagesDiv"></div>
            <asp:Panel runat="server" DefaultButton="SearchButton">
            <label><asp:Label runat="server" ID="MessageLabel"></asp:Label></label>
                <asp:Label runat="server" ID="ErrorLabel" ForeColor="red"></asp:Label>
                <h1>Search for a Friend on Hippo Happenings </h1>
                <br />
                <asp:TextBox runat="server" ID="SearchTextBox" Width="150px"></asp:TextBox>&nbsp;<label>(By Name / Username / Email )</label>
                <br /><br />
                <div style="padding-bottom: 15px; margin-top: 3px;">
                    <asp:Panel align="left"  runat="server" ID="FriendsPanel" BorderStyle="solid" BorderWidth="1px" 
                    BorderColor="Black" BackColor="#363636" Height="250px" ScrollBars="Vertical" Width="100%">
                        
                    </asp:Panel>
                </div>
                <asp:ImageButton runat="server" ID="SearchButton" OnClientClick="reSetScroll();" OnClick="Search" ImageUrl="image/SearchButton.png" onmouseover="this.src='image/SearchButtonSelected.png'" onmouseout="this.src='image/SearchButton.png'" />
                <img runat="server" ID="ImageButton1" onclick="Search();" src="image/DoneSonButton.png" onmouseover="this.src='image/DoneSonButtonSelected.png'" onmouseout="this.src='image/DoneSonButton.png'" />
            </asp:Panel>
          </div>
       <%-- </telerik:RadAjaxPanel>--%>
    </div>
    <input id="__SAVESCROLL" name="__SAVESCROLL" value="0" type="hidden" runat="server" />

<script type="text/javascript">
    setScroll();
</script>
  <script type="text/javascript" src="SaveScroll.js"></script>

<%-- </telerik:RadAjaxPanel>--%>
    </form>
</body>
</html>
