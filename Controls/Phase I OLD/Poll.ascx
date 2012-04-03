<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Poll.ascx.cs" Inherits="Controls_Poll" %>
<div align="center" style="height: 235px; width: 410px; border: solid 4px #515151; background: url('image/PollBackground.gif');">
    <div align="left" style="padding-left: 10px; padding-top: 5px;height: 30px; color: #1fb6e7; font-family: Arial; font-size: 20px; font-weight: bold;">
        <table width="100%">
            <tr>
                <td>
                    Today's Poll!
                </td>
                <td align="right">
                    <a style="color: #1fb6e7; font-family: Arial; font-size: 10px; text-decoration: none;" href="./PostAPoll.aspx">Add your own poll question...</a>
                </td>
            </tr>
        </table>
    </div>
    <div align="center" style="background: url('image/PollQuestionBackground.png'); width: 396px; height: 101px;">
        <div align="left" style="padding-top: 16px; padding-left: 13px;">
            <asp:Literal runat="server" ID="PollLiteral"></asp:Literal>
           
        </div>
    </div>
    <asp:Panel runat="server" ID="UserPanel" Visible="false">
        <div align="left" style="padding-left: 20px;" class="EventDiv">
            
           <asp:Literal runat="server" ID="AnswersLiteral"></asp:Literal>
        </div>
        <div align="center" style="padding-top: 10px;">
            <asp:ImageButton runat="server" ID="CastButton" ImageUrl="~/image/CastYourVoteButton.png"  onmouseout="this.src='image/CastYourVoteButton.png'" onmouseover="this.src='image/CastYourVoteButtonSelected.png'" OnClick="GoTo" />
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="AnsweredPanel" Visible="false">
        <div align="left" class="EventDiv" style="padding-left: 70px;">
            <label>Answers</label><br />
            <asp:Label runat="server" CssClass="AddLink" ID="Ans1NumLabel"></asp:Label><label><asp:Label runat="server" ID="Ans1Label"></asp:Label></label><br />
            <asp:Label runat="server" CssClass="AddLink" ID="Ans2NumLabel"></asp:Label><label><asp:Label runat="server" ID="Ans2Label"></asp:Label></label><br />
            <asp:Label runat="server" CssClass="AddLink" ID="Ans3NumLabel"></asp:Label><label><asp:Label runat="server" ID="Ans3Label"></asp:Label></label><br />
            <a href="AnswerPoll.aspx" class="AddLink">See Answers in Detail.</a>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="NonUserPanel">
        <div class="EventDiv" align="center" style="padding-left: 20px;">
           <label><asp:HyperLink runat="server" CssClass="AddLink" NavigateUrl="~/UserLogin.aspx" Text="Log in"></asp:HyperLink> to answer the poll and see the answers! </label>
        </div>
    </asp:Panel>
</div>