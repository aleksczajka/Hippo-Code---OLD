<%@ Page Language="C#" ValidateRequest="false" 
AutoEventWireup="true" 
CodeFile="Changelocation.aspx.cs" 
Inherits="Changelocation" Title="Blog An Event" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register Src="~/Controls/BlueButton.ascx" TagName="BlueButton" TagPrefix="ctrl" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head  runat="server" id="HeadTag">
    <link type="text/css" href="StyleSheet.css" rel="stylesheet" />

</head>
<body id="bodytag" runat="server" style="padding: 0; margin: 0;">
    <form id="form1" runat="server">
<asp:ScriptManager runat="server"></asp:ScriptManager>
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
    function CloseWindow(something)
    {
    
        var oWindow = GetRadWindow(); 
        oWindow.Close(something); 
    }
  
    function Search(something) 
    { 
        CloseWindow(something);
    } 
</script>
<div align="center" style="padding: 20px;">
    <div style="color: #535252;">
            Welcome! <br /><br />Where are you located?
            <br /><br />
                <rad:RadComboBox Skin="WebBlue" runat="server" OnSelectedIndexChanged="ChangeState" 
                    DataSourceID="SqlDataSourceCountry" DataTextField="country_name" 
                    DataValueField="country_id" Width="150px" AutoPostBack="true"  ID="CountryDropDown">
                    </rad:RadComboBox>
            
                <asp:SqlDataSource runat="server" ID="SqlDataSourceCountry" SelectCommand="SELECT * FROM Countries"
                                ConnectionString="<%$ ConnectionStrings:Connection %>"></asp:SqlDataSource>

     </div>           
     <div>       
               

                        <div style="padding-top: 7px;">
                            <table cellspacing="0" >
                                <tr>
                                    
                                    <td>
                                        <asp:Panel runat="server" ID="StateTextBoxPanel">
                                            <rad:RadTextBox EmptyMessage="State" Skin="WebBlue"  ID="StateTextBox" Width="100" runat="server"></rad:RadTextBox>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="StateDropDownPanel" Visible="false">
                                            <div style="width: 100px;">
                                            <rad:RadComboBox Text="State" EnableItemCaching="true" EmptyMessage="State" runat="server" 
                                            Width="100px" Skin="WebBlue" ID="StateDropDown"></rad:RadComboBox>
                                            </div>
                                        </asp:Panel>
                                    </td>
                                    <td>
                                            <rad:RadTextBox EmptyMessage="City" Skin="WebBlue" Width="100" 
                                            runat="server" ID="CityTextBox"></rad:RadTextBox>
                                    </td>
                                </tr>
                                
                            </table>
                        </div>

    </div>       
        
        
        <asp:Label runat="server" ID="ErrorLabel" Font-Size="12px" ForeColor="Red" Text="<br /><br /><br />"></asp:Label>
        <div style="width: 165px;">
            <div style="float: left; padding-right: 20px;"><ctrl:BlueButton WIDTH="65px" CLIENT_CLICK="Search()" ID="BlueButton1" runat="server" BUTTON_TEXT="Cancel" /></div>
            <div style="float: left;"><ctrl:BlueButton ID="SelectButton" runat="server" WIDTH="60px" BUTTON_TEXT="Select" /></div>
        </div>
</div>

    <asp:Literal runat="server" ID="BottomScript"></asp:Literal>
</form>
</body>
</html>

