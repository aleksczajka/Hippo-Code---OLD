<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SearchResults_OLD.aspx.cs" Inherits="SearchResults" %>
<%@ Register Src="~/Controls/SearchElements.ascx" TagName="SearchElements" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/VenueSearchElements.ascx" TagName="VenueSearchElements" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/AdSearchElements.ascx" TagName="AdSearchElements" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/GroupSearchElements.ascx" TagName="GroupSearchElements" TagPrefix="ctrl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register Src="~/Controls/BigAd.ascx" TagName="BigAd" TagPrefix="ctrl" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="~/AlertStyleSheet.css" type="text/css" rel="stylesheet" />
</head>
<body style="background-color: #333333;">
    <form id="form1" runat="server">
             <asp:ScriptManager runat="server" ID="ScriptManager1" EnablePageMethods="true" ></asp:ScriptManager>

             <script type="text/javascript">
                var idArray = new Array();
                var n = 0;
               function GetRadWindow() 
                { 
                  var oWindow = null; 
                  if (window.radWindow) 
                     oWindow = window.radWindow; 
                  else if (window.frameElement.radWindow) 
                     oWindow = window.frameElement.radWindow; 
                  return oWindow; 
                }   
                function CloseWindow(returnV)
                {
                
                    var oWindow = GetRadWindow(); 
                    oWindow.Close(returnV); 
                }
              
                function Search() 
                { 
                    CloseWindow();
                } 
              
         </script>
     <div style="top: 0px;z-index: 4000; background-color: #333333; position: fixed; height: 230px; width: 100%;">
         <div style=" z-index: 5000;">
            <asp:Label runat="server" CssClass="ResultsLabel" ID="ResultsLabel"></asp:Label>
            </div>
     </div>
     <div style="width: 100%;z-index: 4000; top: 10px;position: fixed; margin-top: 40px; padding-bottom: 5px; padding-right: 5px;">
                    
                            <ctrl:BigAd ID="BigAd1" IS_WINDOW="true" runat="server" />
           
     </div>
     <div style="width: 420px;top: 230px;z-index: 5000;position: fixed; background-color: #333333;height: 30px;"> 
             <div style="float: left;">  <asp:DropDownList runat="server" AutoPostBack="true"  
            OnSelectedIndexChanged="SortResults" ID="SortDropDown">
                <asp:ListItem Text="Sort By..." Value="-1"></asp:ListItem>
                <asp:ListItem Text="Date Up" Value="DateTimeStart ASC"></asp:ListItem>
                <asp:ListItem Text="Date Down" Value="DateTimeStart DESC"></asp:ListItem>
                <asp:ListItem Text="Title Up" Value="Header ASC"></asp:ListItem>
                <asp:ListItem Text="Title Down" Value="Header DESC"></asp:ListItem>
                <asp:ListItem Text="Venue Up" Value="Name ASC"></asp:ListItem>
                <asp:ListItem Text="Venue Down" Value="Name DESC"></asp:ListItem>
            </asp:DropDownList>   
            </div>
            <div style="padding-left: 20px; float: right">
                <asp:Label runat="server" ID="NumsLabel" CssClass="AddWhiteLink"></asp:Label>
            </div>
     </div>   
    <div class="EventDiv" style="padding-top: 250px;">
        <asp:Panel runat="server" ID="Panel7" Width="440px"><asp:Label runat="server" ForeColor="Red" ID="ErrorLabel"></asp:Label>
                <asp:Literal runat="server" ID="HelpUsLiteral"></asp:Literal>
              <ctrl:SearchElements Visible="false" runat="server" ID="EventSearchElements" />
              <ctrl:VenueSearchElements runat="server" ID="VenueSearchElements" Visible="false" />
              <ctrl:AdSearchElements runat="server" ID="AdSearchElements" Visible="false" />
              <ctrl:GroupSearchElements runat="server" ID="GroupSearchElements" Visible="false" />
              <br /><br />
         </asp:Panel>
    </div>
    </form>
</body>
</html>
