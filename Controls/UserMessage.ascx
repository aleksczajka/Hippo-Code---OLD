<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserMessage.ascx.cs" Inherits="Controls_UserMessage" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>


    <asp:Panel ID="Panel1" runat="server" BackColor="#333333" DefaultButton="SearchBottomButton">
    <script type="text/javascript">
        function AcceptFriend(num, numUser)
        {
            Controls_UserMessage.AcceptTheFriend(num, numUser);
        }
    </script>
        <div style="margin-bottom: 10px;">
            <table>
                <tr>
                    <td width="300px">
                        <asp:Label runat="server" CssClass="MessageBody" ID="MessageLabel"></asp:Label>
                        <asp:Panel runat="server" CssClass="MessageBody" ID="MessagePanel"></asp:Panel>
                    </td>
                    <td valign="top">
                        <table width="260px">
                            <tr>
                                <td align="right">
                                    <asp:Button Text="Delete message" runat="server" OnClick="ArchiveMessage" ID="DeleteButton" onmouseout="this.src='image/X.png'" onmouseover="this.src='image/XSelected.png'" />
                                </td>
                                
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:TextBox runat="server" ID="MessageTextBox" Height="170px" Width="210px"></asp:TextBox>
                                    
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:ImageButton OnClick="Reply" ID="SearchBottomButton" runat="server" ImageUrl="~/image/ReplyButton.png" onmouseout="this.src='image/ReplyButton.png'" onmouseover="this.src='image/ReplyButtonSelected.png'"  />
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label runat="server" ID="MessageLabel2" CssClass="AddLink"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            </div>
            
       
    </asp:Panel>
