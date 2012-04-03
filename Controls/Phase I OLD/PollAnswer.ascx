<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PollAnswer.ascx.cs" Inherits="Controls_PollAnswer" %>
<div class="EventDiv" style="width: 844px; height: 293px; background-color: #686868; padding: 10px; margin-bottom: 20px;">
    <div id="topDiv">
        <div style="width: 350px; float: left;" class="Paddings">
            <div style="height: 60px;">
                <div style="float: left; padding-right: 10px; padding-bottom: 10px;">
                    <asp:Image runat="server" ID="UserImage" Width="50px" Height="50px" />
                </div>
                
                <label>Posted By: </label><asp:HyperLink runat="server" ID="UserNameLabel" CssClass="AddLink"></asp:HyperLink><br />
                <label>Posted On: </label><asp:Label runat="server" ID="UserDateLabel" CssClass="AddLink"></asp:Label><br />
                <label><b>Answer: <asp:Label runat="server" ID="AnswerLabel"></asp:Label></b></label>
            </div>
            <div style="padding-right: 5px;">
                <label><asp:Label runat="server" CssClass="EventBody" ID="DetailedAnswerLabel"></asp:Label></label>
            </div>
        </div>
        <div class="Paddings FloatRight" >
        <asp:Panel runat="server" ID="MediaPanel" Visible="false">
            <div style="height: 245px; border: solid 3px #515151;">
                <asp:Literal runat="server" ID="MediaLiteral"></asp:Literal>
            </div>
        </asp:Panel>
        </div>
    </div>
</div>
