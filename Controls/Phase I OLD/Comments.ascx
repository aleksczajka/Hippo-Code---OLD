<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Comments.ascx.cs" Inherits="Controls_Comments" %>
<%@ Register Src="~/Controls/Comment.ascx" TagName="Comment" TagPrefix="ctrl" %>

<div style="padding-top: 30px;" class="EventDiv">
    <div style="padding-bottom: 8px;">
        <asp:Panel runat="server" ID="RegisterPanel" Visible="false">
            <div style="padding-bottom: 20px;">
                <label class="AddGreenLink">To post a comment and do much more <a class="AddLink" href="Register.aspx">Register</a> or <a class="AddLink" href="UserLogin.aspx">Log in</a></label>
            </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="CommentsTitlePanel" Visible="false">
            <label style="font-size: 16px; font-family: Arial; color: White;">Comments</label>
        </asp:Panel>
        
    </div>
    <asp:Panel runat="server" ID="CommentsPanel"></asp:Panel>
    <asp:Panel runat="server" ID="ToSeePanel" Visible="false">
        <div style="padding-bottom: 30px; padding-top: 20px;">
            <label>To see all the comments got to </label><asp:HyperLink CssClass="AddLink" runat="server" ID="PageLink">this event's  page.</asp:HyperLink>
        </div>        
    </asp:Panel>
    <asp:Panel runat="server" ID="GotSayPanel" Width="430px">
        <div align="left" style="clear: both; padding-top: 20px;">
            <asp:Label runat="server" CssClass="SayLabel" ID="GotSayLabel">Got something to say?</asp:Label>
            <asp:TextBox ID="CommentTextBox" runat="server" Width="410px" CssClass="AddCommentTextBox" TextMode="multiLine" Wrap="true"></asp:TextBox>
            <div align="right" style="padding-top: 5px;">
            <button id="Button1" runat="server" onserverclick="PostIt" onclick="this.innerHTML = 'Working...';this.disabled=true;this.style.backgroundImage = 'url(image/BigNoTexthover.png)';" style=" cursor: pointer;font-weight: bold;padding:0;font-size: 16px;height: 37px; width: 122px;background-color: transparent; color: White; background-image: url('image/BigNoText.png'); background-repeat:no-repeat; border: 0; padding-bottom: 4px;" 
                onmouseout="this.style.backgroundImage='url(image/BigNoText.png)'" onmouseover="this.style.backgroundImage='url(image/BigNoTextHover.png)'" >Post It!</button>

<%--                <asp:ImageButton runat="server" ID="PostItButton" OnClick="PostIt" ImageUrl="~/image/PostItButton.png" 
                onmouseout="this.src='image/PostItButton.png'" onmouseover="this.src='image/PostItButtonSelected.png'" />
--%>            </div>
        </div>
    </asp:Panel>
</div>
