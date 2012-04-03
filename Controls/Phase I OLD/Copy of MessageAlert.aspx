<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Copy of MessageAlert.aspx.cs" Inherits="Coaches_MessageAlert" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="~/StyleSheet.css" type="text/css" rel="stylesheet" />
</head>
<body style="background-color: #333333;">
    <form id="form1" runat="server">
         
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
    <div class="EventDiv">
        <div>
            <label><asp:Label runat="server" ID="MessageLabel"></asp:Label></label>
            <asp:Panel runat="server" ID="MessagePanel" Visible="false"> 
                <label>Subject:</label> <br />
            <div>
                <asp:Label ID="ErrorLabel" runat="server" Text="Your messages will not be sent without a subject." Visible="false" CssClass="AddLink"></asp:Label>
            </div>
                <asp:Panel runat="server" ID="SubjectLabelPanel" Visible="false">
                    <label><asp:Label runat="server" ID="SubjectLabel"></asp:Label></label><br /><br />
                </asp:Panel>
                <asp:Panel runat="server" ID="SubjectTextBoxPanel" Visible="false">
                    <asp:TextBox runat="server" ID="SubjectTextBox"></asp:TextBox>
                </asp:Panel>
                <label>Message:</label><br />
                <asp:TextBox runat="server" ID="MessageTextBox" TextMode="MultiLine" Wrap="true" Width="300px" Height="200px"></asp:TextBox><br />
                <asp:ImageButton runat="server" ID="SendItButton" OnClick="SendIt" OnClientClick="Search()" 
                 ImageUrl="~/image/SendItButton.png" onmouseout="this.src='../image/SendItButton.png'"
                 onmouseover="this.src='../image/SendItButtonSelected.png'" />
                
            </asp:Panel>
            <asp:Panel runat="server" ID="EmailPanel" Visible="false"> 
                <label>Your Email Address:</label><br />
                <asp:TextBox runat="server" ID="YourEmailTextBox"></asp:TextBox><br /><br />
                <label>Subject:</label> <br />
                <asp:Panel runat="server" ID="Panel2" >
                    <label><asp:Label runat="server" ID="EmailSubjectLabel"></asp:Label></label>
                </asp:Panel><br />
                <label>Message:</label><br />
                <asp:Panel CssClass="AspLabel" ScrollBars="Both" BackColor="White" runat="server" Width="300px" Height="200px">
                    <asp:Literal runat="server" ID="EmailTextBox"></asp:Literal>
                </asp:Panel><br />
                <asp:ImageButton runat="server" ID="ImageButton2" OnClick="SendEmail" OnClientClick="Search()" 
                 ImageUrl="~/image/SendItButton.png" onmouseout="this.src='../image/SendItButton.png'"
                 onmouseover="this.src='../image/SendItButtonSelected.png'" />
                
            </asp:Panel>
            <asp:Panel runat="server" ID="CalendarPanel" Visible="false">
                <div align="left">
                    <label class="AddGreenLink">Add this event to your calendar </label><br /><br />
                    <label>How do you feel about going to this event?</label> 
                    <asp:DropDownList runat="server" ID="EventExcitmentDropDown" DataSourceID="LevelDataSource" DataTextField="ExcitmentLevel" DataValueField="ID"></asp:DropDownList>
                    <asp:SqlDataSource ID="LevelDataSource" runat="server" SelectCommand="SELECT * FROM Event_ExcitmentLevel" ConnectionString="<%$ ConnectionStrings:Connection%>"></asp:SqlDataSource>
                    <br /><br />
                    <asp:CheckBox runat="server" Text="Do you want people to conntact you regarding going to this event?" ID="ConnectCheckBox" />
                    <br /><br />
                    <asp:ImageButton runat="server" ID="AddCalendarButton" OnClientClick="Search()" ImageUrl="~/image/MakeItSoButton.png" OnClick="AddTo" onmouseout="this.src='../image/MakeItSoButton.png'" onmouseover="this.src='../image/MakeItSoButtonSelected.png'"  />
                    <asp:Label runat="server" ID="Label1"></asp:Label>
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="VenuePanel" Visible="false">
                <div align="left">
                    <label class="AddGreenLink">Add this venue to your favorites </label><br /><br />
                    <asp:ImageButton runat="server" ID="ImageButton1" OnClientClick="Search()" ImageUrl="~/image/MakeItSoButton.png" OnClick="AddToVenue" onmouseout="this.src='../image/MakeItSoButton.png'" onmouseover="this.src='../image/MakeItSoButtonSelected.png'"  />
                    <asp:Label runat="server" ID="Label3"></asp:Label>
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="TextPanel" Visible="false">
                        <div align="left">
                            <%--style=" background-color: #363636; border: solid 1px #1b1b1b;"--%>
                            <label class="AddLink">Choose your number:</label>
                            <asp:TextBox runat="server" ID="PhoneTextBox"></asp:TextBox>
                            <asp:RangeValidator runat="server" ControlToValidate="PhoneTextBox" ErrorMessage="Phone number must be a number. Include it without any hyphens or brackets." CssClass="AddLink" MaximumValue="9999999999" MinimumValue="0000000000" Type="double"></asp:RangeValidator>
                            <br />
                            <label class="AddLink">Choose your provider:</label><br />
                            <asp:DropDownList runat="server" ID="ProvidersDropDown" DataSourceID="ProviderDataSource" DataTextField="Provider" DataValueField="Extension">
                            </asp:DropDownList><br /><br />
                            <label class="AddLink">Your Message:</label>
                            <asp:TextBox Height="100px" Width="250px" runat="server" ID="TxtMessageTextBox" TextMode="MultiLine" Wrap="true"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="SendButton" OnClientClick="Search()" ImageUrl="../image/SendItButton.png" Text="Send" OnClick="SendMessage"  onmouseout="this.src='../image/SendItButton.png'" onmouseover="this.src='../image/SendItButtonSelected.png'"  />
                            <asp:Label runat="server" ID="Label2"></asp:Label>
                        </div>
                <asp:SqlDataSource ID="ProviderDataSource" runat="server" SelectCommand="SELECT * FROM PhoneProviders" ConnectionString="<%$ ConnectionStrings:Connection%>"></asp:SqlDataSource>

            </asp:Panel>
            <div style="padding-top: 5px;">
                    <asp:Button runat="server" ID="CloseButton" CssClass="ButtonBlue" Text="Cancel" OnClientClick="Search()" />
                </div>
          </div>
    </div>
    </form>
</body>
</html>
