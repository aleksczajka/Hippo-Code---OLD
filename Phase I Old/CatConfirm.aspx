<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CatConfirm.aspx.cs" Inherits="CatConfirm" %>
<%@ Register TagName="HippoTextBox" TagPrefix="ctrl" Src="~/Controls/HippoTextBox.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="~/StyleSheet.css" type="text/css" rel="stylesheet" />
</head>
<body style="background-color: #333333; scrollbar-base-color: #666666; scrollbar-shadow-color: #666666; scrollbar-highlight-color: #666666; scrollbar-darkshadow-color: #666666; scrollbar-3dlight-color: #666666; scrollbar-track-color: #999999; scrollbar-arrow-color: #363636;">
    <form id="form1" runat="server">
         
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
                function CloseWindow(args)
                {
                
                    var oWindow = GetRadWindow(); 
                    var oArg = args;
                    
                    //oArg.Name = args;
                    oWindow.close(args); 
                }
              
                function Search(args) 
                { 
                    CloseWindow(args);
                } 
                
                function Remove(theDiv)
                {
                    var d = document.getElementById('checksDiv');
                    var theInput = document.getElementById(theDiv);
                    d.removeChild(theInput);
                    
                    var response;
                    MessageAlert.RemoveID(theDiv, idArray, Remove_CallBack);
                }
                
                function Remove_CallBack(theID)
                { 
                    idArray.splice(theID.value, 1);
                    n = n-1;
                }
                
                function SendMsg()
                {
                    var msgText = document.getElementById('TextInput');
                    var userName = document.getElementById('userName');
                    var userID = document.getElementById('userID');
                    var eventName = document.getElementById('eventName');
                    var msgBody = document.getElementById('msgBody');
                    
                    MessageAlert.SendIt(msgText.value, userName.innerHTML, userID.innerHTML, eventName.innerHTML, idArray, msgBody.innerHTML);
                }
                function AddFriend(theName, i)
                {
                    if(document.getElementById('div' + i) == null)
                    {
                        idArray[n] = i;
                        n = n + 1;
                        var d = document.getElementById('checksDiv');
                        
                        var newdiv = document.createElement('div');
                        var divIdName = 'div' + i;
                        newdiv.setAttribute('id',divIdName);
                        newdiv.innerHTML = '<div style=\'float: left; width: 120px;\'><label>'+theName+'</label></div> <img src=\'image/DeleteCircle.png\' id=\'b'+i+'\' alt=\'Remove selection\' title=\'Remove selection\' onclick=\'Remove("div'+i+'");\' />';
                        d.appendChild(newdiv);
                    }
                }
         </script>
    <div class="EventDiv" align="center" style="vertical-align: middle; width: 250px; height: 100%;">
        <asp:Panel runat="server" ID="LocationPanel">
        <div style="width: 250px;" align="center">
            <label>Your account has been created. 
            To customize your site for your particular location select your Country, City and State 
            and click on 'Make it so'. All fields are required except for zip and radius.</label>
            <br /><br />
            <div style="clear: both;"  class="Pad">
                <table>
                     <tr>
                        <td colspan="2">
                            <label style="padding-right: 10px;">*Country</label><br />
                            <asp:DropDownList OnSelectedIndexChanged="ChangeState" AutoPostBack="true" runat="server" 
                            DataSourceID="SqlDataSourceCountry" DataTextField="country_name" DataValueField="country_id" ID="CountryDropDown"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="SqlDataSourceCountry" SelectCommand="SELECT * FROM Countries"
                            ConnectionString="<%$ ConnectionStrings:Connection %>"></asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td width="200px">
                            <label>*State</label>
                            <asp:Panel runat="server" ID="StateTextBoxPanel">
                                <ctrl:HippoTextBox ID="StateTextBox" runat="server" TEXTBOX_WIDTH="100" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                            </asp:Panel>
                            <asp:Panel runat="server" ID="StateDropDownPanel" Visible="false">
                                <asp:DropDownList runat="server" ID="StateDropDown"></asp:DropDownList>
                            </asp:Panel>
                        </td>
                        <td>
                            <label>*City</label><br />
                                <ctrl:HippoTextBox ID="CityTextBox" runat="server" TEXTBOX_WIDTH="100" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                        </td>
                    </tr>
                    <tr>
                        <td width="200px">
                            <label>Zip [Not Required]</label><br />
                                <ctrl:HippoTextBox ID="CatZipTextBox" runat="server" TEXTBOX_WIDTH="100" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                        </td>
                        <td>
                            <asp:Panel runat="server" ID="RadiusPanel">
                            <label>Radius [Not Required]</label><br />
                            <asp:DropDownList runat="server" ID="RadiusDropDown">
                                <asp:ListItem Value="0" Text="Just Zip Code (0 miles)"></asp:ListItem>
                                <asp:ListItem Value="1" Text="1 mile"></asp:ListItem>
                                <asp:ListItem Value="5" Text="5 miles"></asp:ListItem>
                                <asp:ListItem Value="10" Text="10 miles"></asp:ListItem>
                                <asp:ListItem Value="15" Text="15 miles"></asp:ListItem>
                                <asp:ListItem Value="30" Text="30 miles"></asp:ListItem>
                                <asp:ListItem Value="50" Text="50 miles"></asp:ListItem>
                            </asp:DropDownList>
                            </asp:Panel>
                        </td>
                    </tr>
                    
                </table>
            </div>
            <br />
            <div id="topDiv">
                <div style="float: left ;">
                <asp:ImageButton runat="server" ID="ImageButton1" ImageUrl="~/image/MakeItSoButton.png" 
                    OnClick="Save" onmouseout="this.src='image/MakeItSoButton.png'" 
                    onmouseover="this.src='image/MakeItSoButtonSelected.png'"  />
                   </div>
                   <div style="float: right;">   
                <asp:Literal runat="server" ID="VariablesLiteral"></asp:Literal>
                        </div> 
            </div>
            <br />
            <label><asp:Label runat="server" ID="MessageLabel" CssClass="AddGreenLink"></asp:Label></label>
            
        </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="DonePanel" Visible="false">
            <label>Done! Proceed to your user account.</label>
                <br /><br />
            <asp:Button runat="server" ID="Button3" Text="Proceed" CssClass="SearchButton" 
              OnClientClick="Search('User.aspx');" onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" />
        </asp:Panel>
    </div>
    </form>
</body>
</html>
