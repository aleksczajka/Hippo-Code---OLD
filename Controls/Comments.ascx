<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Comments.ascx.cs" Inherits="Comments" %>
<%@ Register Src="~/Controls/Comment.ascx" TagName="Comment" TagPrefix="ctrl" %>
<%@ Register TagName="SmallButton" TagPrefix="ctrl" Src="~/Controls/SmallButton.ascx" %>

<div class="EventDiv CommentsTop">
    <div class="CommentsInner">
        <asp:Panel runat="server" ID="RegisterPanel" Visible="false">
            <div class="CommentsInnerInner">
                <span class="Text12">To post a comment and do much more <a class="NavyLink12UD" href="login">Log in</a>.</span>
            </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="CommentsTitlePanel" Visible="false">
            <h1>Comments</h1>
        </asp:Panel>
        
    </div>
    <asp:Panel runat="server" ID="CommentsPanel"></asp:Panel>
    <asp:Panel runat="server" ID="GotSayPanel" Width="430px">
        <div align="left" class="CommentTop">
            <h1>Got something to say?</h1>
            <div class="topDiv CommentInner">
                <div class="aboutLink">
                    <div class="FloatLeft"><img alt="Background Image" src="NewImages/BackLeftTopCorner.png" /></div>
                    <div class="CommentTopBack">
                        &nbsp;
                    </div>
                    <div class="FloatLeft"><img alt="Background Image" src="NewImages/BackRightTopCorner.png" /></div>
                </div>
                <div class="LocaleLink7"> 
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="15px" class="CommentMiddleBack"></td>
                            <td>
                                <div class="Text12 CommentMiddleInner">
                                    <asp:TextBox ID="CommentTextBox" runat="server" Width="398px" CssClass="NoBorderTextBox" TextMode="multiLine" Wrap="true"></asp:TextBox>
                                </div>
                            </td>
                            <td width="15px" class="CommentBottom"></td>
                        </tr>
                    </table>                       
                </div>
                <div class="aboutLink">
                    <div class="FloatLeft"><img alt="Background Image" src="NewImages/BackLeftBottomCorner.png" /></div>
                    <div class="CommentBottomInner">
                        &nbsp;
                    </div>
                    <div class="FloatLeft"><img alt="Background Image" src="NewImages/BackRightBottomCorner.png" /></div>
                </div>
           </div>
            
            
            <div class="CommentsBottom">
            <ctrl:SmallButton runat="server" ID="PostButton" BUTTON_TEXT="Post It!" />

           </div>
        </div>
    </asp:Panel>
</div>
