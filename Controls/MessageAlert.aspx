<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MessageAlert.aspx.cs" 
ValidateRequest="false" Inherits="MessageAlert" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register TagName="BlueButton" TagPrefix="ctrl" Src="~/Controls/BlueButton.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="~/StyleSheet.css" type="text/css" rel="stylesheet" />
</head>
<body>
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
                function CloseWindow(variable)
                {
                
                    var oWindow = GetRadWindow(); 
                    oWindow.Close(variable); 
                }
              
                function Search(variable) 
                { 
                    
                        CloseWindow(variable);
                } 
                
                function Done() 
                { 
                    if(Page_IsValid)
                    {
                        CloseWindow();
                        return true;
                    }else
                        return false;
                } 
                
                function AddToVenues()
                {
                    var theDiv = document.getElementById('TechDiv');
                    Coaches_MessageAlert.AddToVenue(theDiv.innerHTML, Callback_function);
                }
                
                function Callback_function(response)
                {
                    Search();
                }
         </script>
    <div class="EventDiv"><asp:Literal runat="server" ID="ScriptLiteral"></asp:Literal>
        <div>
            <label><asp:Label runat="server" ID="MessageLabel"></asp:Label></label>
            <asp:Panel runat="server" ID="MessagePanel" Visible="false"> 
                <h2>Subject:</h2>
            <div>
                <asp:Label ID="ErrorLabel" runat="server" Text="[Your messages will not be sent without a subject.]" Visible="false"></asp:Label>
            </div>
                <asp:Panel runat="server" ID="SubjectLabelPanel" Visible="false">
                    <label><asp:Label runat="server" ID="SubjectLabel"></asp:Label></label><br />
                </asp:Panel>
                <asp:Panel runat="server" ID="SubjectTextBoxPanel" Visible="false">
                    <asp:TextBox runat="server" ID="SubjectTextBox"></asp:TextBox>
                </asp:Panel>
                <h2>Message:</h2><br />
                <asp:TextBox runat="server" ID="MessageTextBox" TextMode="MultiLine" Wrap="true" Width="300px" Height="200px"></asp:TextBox><br />
                <br />
                <table width="100%">
                    <tr>
                        <td align="right" width="50%">
                            <div style="float: right;"> 
                                <ctrl:BlueButton runat="server" ID="SendItButton" BUTTON_TEXT="Send It" />
                            </div>
                        </td>
                        <td align="left" width="50%">
                            <ctrl:BlueButton CLIENT_CLICK="Search()" runat="server" ID="BlueButton1" BUTTON_TEXT="Forget It" />
                       </td>
                    </tr>
                </table>
                <asp:Label runat="server" CssClass="AddGreenLink" ID="EmailMessageLabel"></asp:Label>
            </asp:Panel>
            <asp:Panel runat="server" ID="EmailPanel" Visible="false"> 
                <div>
                    <label><b>Email Address[es]: (separated by semicolon)</b></label>
                    <asp:Image CssClass="HelpImage" runat="server"  ID="QuestionMark6" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
                        <rad:RadToolTip ID="RadToolTip5" runat="server" Skin="Sunset" 
                        Width="70px" ManualClose="true" ShowEvent="onclick" 
                        Position="BottomCenter" RelativeTo="Element" TargetControlID="QuestionMark6">
                        <label>   To include more than one address at once, separate the email addresses by a semicolon. Here's 
                        an example: hippo1@hippohappenings.com;hippo2@hippohappenings.com</label>
                        </rad:RadToolTip>
                    <br />
                    <div class="MsgDiv"><asp:TextBox runat="server" ID="YourEmailTextBox" ></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorEmail" runat="server" 
                    CssClass="AddGreenLink" ErrorMessage="*Email is required" 
                    ControlToValidate="YourEmailTextBox"></asp:RequiredFieldValidator> </div>   <br />
<%--                    <asp:RegularExpressionValidator runat="server" CssClass="AddGreenLink" ErrorMessage="*Email is not valid" ValidationExpression="^[a-z0-9_\+-]+(\.[a-z0-9_\+-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*\.([a-z]{2,4})$" ControlToValidate="YourEmailTextBox"></asp:RegularExpressionValidator>
--%>                    <br />
                    <asp:Panel runat="server" visible="false">
                    <label><b>Subject:</b></label> <br />
                    <asp:Panel runat="server" ID="Panel2" >
                       <div class="MsgDiv"> <label><asp:Label runat="server" ID="EmailSubjectLabel"></asp:Label></label></div>
                    </asp:Panel><br />
                    </asp:Panel>
                    <label><b>Write a personal message to send along with the post information:</b></label><br />
                        <div class="MsgDiv"><asp:TextBox TextMode="MultiLine" Wrap="true" ID="EmailTextBox" runat="server" Width="300px" Height="200px"></asp:TextBox></div>
                    <br />
                    
                    <table width="100%">
                        <tr>
                            <td align="right">
                                <div style="float: right;">
                                    <ctrl:BlueButton runat="server" ID="BlueButton3" BUTTON_TEXT="Send It" />
                                </div>
                            </td>
                            <td align="left">
                                <ctrl:BlueButton CLIENT_CLICK="Search()" runat="server" ID="BlueButton4" BUTTON_TEXT="Never Mind" />
                         </td>
                        </tr>
                    </table>
                    <asp:Label CssClass="AddGreenLink" runat="server" ID="EmailSentLabel"></asp:Label>
                </div>

            </asp:Panel>
            <asp:Panel runat="server" ID="CalendarPanel" Visible="false">
                <div align="left">
                    <label>Add this event to your calendar </label><br /><br />
                    <label>How do you feel about going to this event?</label> 
                    <asp:DropDownList runat="server" ID="EventExcitmentDropDown" DataSourceID="LevelDataSource" DataTextField="ExcitmentLevel" DataValueField="ID"></asp:DropDownList>
                    <asp:SqlDataSource ID="LevelDataSource" runat="server" SelectCommand="SELECT * FROM Event_ExcitmentLevel" ConnectionString="<%$ ConnectionStrings:Connection%>"></asp:SqlDataSource>
                    <br /><br />
                    <asp:CheckBox runat="server" Text="Do you want people to conntact you regarding going to this event?" ID="ConnectCheckBox" />
                    <br /><br />
                    <table width="100%">
                        <tr>
                            <td align="right">
                                <asp:ImageButton runat="server" ID="AddCalendarButton" ImageUrl="~/image/MakeItSoButton.png" OnClick="AddTo" onmouseout="this.src='../image/MakeItSoButton.png'" onmouseover="this.src='../image/MakeItSoButtonSelected.png'"  />
                            </td>
                            <td align="left">
                                <asp:ImageButton runat="server" ID="ImageButton5" ImageUrl="~/image/DoneSonButton.png" OnClientClick="Search();" onmouseout="this.src='../image/DoneSonButton.png'" onmouseover="this.src='../image/DoneSonButtonSelected.png'"  />
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="VenuePanel" Visible="false">
                <div align="center" style="padding-top: 30px;">
                    <label>Add this venue to your favorites </label><br /><br />
                    <%--<img ID="ImageButton1" src="../image/MakeItSoButton.png"
                        onmouseout="this.src='../image/MakeItSoButton.png'" 
                        onmouseover="this.src='../image/MakeItSoButtonSelected.png'"  />--%>
                    <table width="100%">
                        <tr>
                            <td align="right" width="50%">
                                <div style="float: right;">
                                    <ctrl:BlueButton runat="server" ID="BlueButton5" BUTTON_TEXT="Yes" />
                                </div>
                            </td>
                            <td align="left">
                                <ctrl:BlueButton runat="server" ID="BlueButton6" BUTTON_TEXT="No" CLIENT_CLICK="Search()" />
                            </td>
                        </tr>
                    </table>
                   
<%--                    <asp:ImageButton runat="server" ID="ImageButton10" ImageUrl="~/image/MakeItSoButton.png" 
                    OnClick="AddVenue" onmouseout="this.src='../image/MakeItSoButton.png'" 
                    onmouseover="this.src='../image/MakeItSoButtonSelected.png'"  />
                    
                    <asp:ImageButton runat="server" ID="ImageButton4" ImageUrl="~/image/DoneSonButton.png" OnClientClick="Search();" onmouseout="this.src='../image/DoneSonButton.png'" onmouseover="this.src='../image/DoneSonButtonSelected.png'"  />
--%>
                    <asp:Label runat="server" CssClass="AddGreenLink" ID="VenueMessageLabel"></asp:Label>
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="ThankYouPanel" Visible="false">
                <div class="EventDiv" align="center" style="padding-top: 30px;">
                    <label>Your message has been sent!</label>
                    <br /><br /><br /><br />
                    <div style="float: right; width: 215px;">
                        <ctrl:BlueButton runat="server" BUTTON_TEXT="Done" CLIENT_CLICK="Search();" />
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="TextPanel" Visible="false">
                        <div align="left">
                            <%--style=" background-color: #363636; border: solid 1px #1b1b1b;"--%>
                            <label>Phone Number:</label><br />
                            <asp:TextBox runat="server" ID="PhoneTextBox"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            CssClass="AddGreenLink" ErrorMessage="*Phone is required" 
                            ControlToValidate="PhoneTextBox"></asp:RequiredFieldValidator>    <br />
                             <asp:RangeValidator ID="RangeValidator1" runat="server" 
                             ControlToValidate="PhoneTextBox" 
                             ErrorMessage="*Enter number correctly and without hyphens or brackets." 
                             CssClass="AddLink" MaximumValue="9999999999" MinimumValue="0000000000" 
                             Type="double"></asp:RangeValidator><br />

                            <label>Provider:</label><br />
                            <asp:DropDownList runat="server" ID="ProvidersDropDown" DataSourceID="ProviderDataSource" DataTextField="Provider" DataValueField="Extension">
                            </asp:DropDownList><br /><br />
                            <label>Message:</label><br /><span style="font-family: Arial; font-size: 10px; color: #666666;">160 character limit</span><br />
                            <asp:TextBox Height="100px" Width="250px" runat="server" ID="TxtMessageTextBox" TextMode="MultiLine" Wrap="true"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="AddGreenLink" ErrorMessage="*Message is required" ControlToValidate="TxtMessageTextBox"></asp:RequiredFieldValidator>    
                            <asp:RegularExpressionValidator ValidationExpression=".{0,160}" CssClass="AddGreenLink" ErrorMessage="*Message must be less than 161 characters." runat="server" ControlToValidate="TxtMessageTextBox"></asp:RegularExpressionValidator>
                                <table style="width: 100%; padding-top: 50px;" align="center">
                                    <tr>
                                        <td align="right">
                                            <div style="float: right;">
                                                <ctrl:BlueButton runat="server" ID="BlueButton7" BUTTON_TEXT="Send It" />
                                            </div>
                                        </td>
                                        <td align="left">
                                            <ctrl:BlueButton runat="server" ID="BlueButton8" 
                                            BUTTON_TEXT="Never Mind" CLIENT_CLICK="Search()" />
                                      </td>
                                    </tr>
                                </table>
                           <br />

                            <asp:Label CssClass="AddGreenLink" runat="server" ID="PhoneMessage"></asp:Label>
                        </div>
                <asp:SqlDataSource ID="ProviderDataSource" runat="server" SelectCommand="SELECT * FROM PhoneProviders" ConnectionString="<%$ ConnectionStrings:Connection%>"></asp:SqlDataSource>

            </asp:Panel>
            <asp:Panel runat="server" ID="FlagPanel" Visible="false"> 
                <div>
                    <asp:Panel runat="server" ID="FlagTextPanel" Visible="false">
                    <label><b>Your Email Address:</b></label>
                    
                    
                    <br />
                    <div class="MsgDiv"><asp:TextBox runat="server" ID="FlagEmailTextBox" ></asp:TextBox>
                    </div>
                    <br />
                    </asp:Panel>
                    <label><b>Subject:</b></label> <br />
                    <asp:Panel runat="server" ID="Panel3" >
                       <div class="MsgDiv"> <label><asp:Label runat="server" ID="FlagSubject"></asp:Label></label></div>
                    </asp:Panel><br />
                    <label><b>Tell us why this page bothers you:</b></label><br />
                        <div class="MsgDiv"><asp:TextBox TextMode="MultiLine" Wrap="true" ID="TextBox2" runat="server" Width="300px" Height="200px"></asp:TextBox></div>
                    <br />
                    <table width="100%">
                        <tr>
                            <td align="right">
                                <div style="width: 60px;">
                                    <ctrl:BlueButton runat="server" ID="FlagButton" BUTTON_TEXT="Flag It" />
                                </div>
                            </td>
                            <td align="left">
                                <ctrl:BlueButton runat="server" CLIENT_CLICK="Search()" ID="BlueButton2" BUTTON_TEXT="Never Mind" />
                         </td>
                                      
                        </tr>
                    </table>
                    <asp:Label CssClass="AddGreenLink" runat="server" ID="Label2"></asp:Label>
                </div>

            </asp:Panel>
           <asp:Panel runat="server" ID="FlagLogInPanel" Visible="false">
            <label>You must be logged in to flag an item. Please <a class="AddLink" href="javascript:Search('login');">Log in</a> or <a class="AddLink" href="javascript:Search('Register.aspx');">Register</a>. </label>
           </asp:Panel>
            <%--<div style="padding-top: 5px;">
                    <asp:ImageButton runat="server" ID="CloseButton" ImageUrl="~/image/CancelButton.png" OnClientClick="Search();" onmouseout="this.src='../image/CancelButton.png'" onmouseover="this.src='../image/CancelButtonSelected.png'"  />
                </div>--%>
          </div>
    </div>
    <asp:Literal runat="server" ID="TechLiteral"></asp:Literal>
    </form>
</body>
</html>
