<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AnswerPoll.aspx.cs" Inherits="AnswerPoll" Title="Answer Poll" %>
<%@ Register Src="~/Controls/PollAnswers.ascx" TagName="PollAnswers" TagPrefix="ctrl" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <a href="#top"></a>
    <div class="EventDiv">
        <div style="width: 530px; padding-bottom: 10px; float: right;"> <b><a style="color: #1fb6e7; font-family: Arial; font-size: 10px; text-decoration: none;" href="./PostAPoll.aspx">Add your own poll question...</a></b></div>
        <asp:Label runat="server" CssClass="EventHeader" ID="PollTitleLabel">Today's Poll:</asp:Label>
   <br />
        <div style="padding-bottom: 30px;"><asp:Label runat="server" CssClass="PollQuestion" ID="PollQuestionLabel"></asp:Label><br /></div>
        <br />
        <asp:Panel runat="server" ID="AnsweredPanel" Visible="false">
                <div align="left" class="AddLinkGoing" style="padding-bottom: 50px;">
                    <label>Answer Summary: (<asp:Label runat="server" ID="TotalUsersLabel" CssClass="AddLink"></asp:Label> Users Answered)</label> <br />
                    <asp:Label runat="server" CssClass="AddLink" ID="Ans1NumLabel"></asp:Label><label><asp:Label runat="server" ID="Ans1Label"></asp:Label></label><br />
                    <asp:Label runat="server" CssClass="AddLink" ID="Ans2NumLabel"></asp:Label><label><asp:Label runat="server" ID="Ans2Label"></asp:Label></label><br />
                    <asp:Label runat="server" CssClass="AddLink" ID="Ans3NumLabel"></asp:Label><label><asp:Label runat="server" ID="Ans3Label"></asp:Label></label><br />
                </div>
            </asp:Panel>
        <asp:Panel runat="server" ID="PollPanel" Visible="false">
            <div style="padding-top: 20px;background-repeat: no-repeat;background-image: url(image/AnswerPollBackgroun.png); width: 502px; height: 421px; padding-left: 25px;">
                <label>Answer:</label>
                <asp:RadioButtonList runat="server" ID="AnswerRadioList" RepeatDirection="vertical" RepeatColumns="1"></asp:RadioButtonList>
                <br />
                <label>Eleaborate on your answer:</label><br />
                <asp:TextBox runat="server" Height="100" Width="410" ID="AnswerTextBox"></asp:TextBox><br />
                <div style="padding-top: 20px; padding-bottom: 20px; height: 90px;">
                    <label>Prove It:</label>
                    <asp:RadioButtonList runat="server" AutoPostBack="true" ID="MediaRadioList" RepeatDirection="Horizontal" OnSelectedIndexChanged="ChangePanels">
                        <asp:ListItem Value="0">Upload Picture</asp:ListItem>
                        <asp:ListItem Value="1">Upload Video</asp:ListItem>
                        <asp:ListItem Value="2">Upload YouTube Video</asp:ListItem>
                    </asp:RadioButtonList>
                        <asp:Panel runat="server" ID="PicturePanel" Visible="false">
                            <div style="float: left; padding-left: 30px;">
                                <div style="float: left; padding-top: 4px;">
                                    <asp:FileUpload runat="server" ID="PictureFileUpload" Width="230px" EnableViewState="true" />
                                </div>
                                <div style="float: left;">
                                    <asp:ImageButton runat="server" ID="PictureUploadButton" 
                                      PostBackUrl="AnswerPoll.aspx#top" OnClick="PictureUpload"
                                     ImageUrl="image/MakeItSoButton.png" 
                                    onmouseout="this.src='image/MakeItSoButton.png'" onmouseover="this.src='image/MakeItSoButtonSelected.png'"  />
                                </div>
                                <br /><br />
                                <asp:Label runat="server" ID="MusicErrorLabel" CssClass="ErrorLabel"></asp:Label>
                            </div>
                        </asp:Panel>
                   
                        <asp:Panel runat="server" ID="VideoPanel" Visible="false">                               
                            <div style="float: left; padding-left: 30px;">
                                <div style="float: left; padding-top: 4px;">
                                    <asp:FileUpload runat="server" ID="VideoFileUpload" Width="230px" EnableViewState="true" />
                                </div>
                                <div style="float: left;">
                                    <asp:ImageButton runat="server" ID="VideoUploadButton" 
                                      PostBackUrl="AnswerPoll.aspx#top" OnClick="VideoUpload"
                                     ImageUrl="image/MakeItSoButton.png" 
                                    onmouseout="this.src='image/MakeItSoButton.png'" onmouseover="this.src='image/MakeItSoButtonSelected.png'"  />
                                </div>
                                <br /><br />
                                <asp:Label runat="server" ID="Label1" CssClass="ErrorLabel"></asp:Label>
                            </div>
                        </asp:Panel>  
                           
                        <asp:Panel runat="server" ID="YouTubePanel" Visible="false">
                            <div style="float: left; padding-left: 30px;">
                                <label> http://www.youtube.com/watch?v=<span style="color: Red;">YMLTwq1M2pw</span>  <--This is the ID</label>
                                <div style="float: left; padding-top: 4px;">
                                    <asp:TextBox runat="server" ID="YouTubeTextBox"></asp:TextBox>
                                </div>
                                <div style="float: left;">
                                    <asp:ImageButton runat="server" ID="YouTubeButton"  OnClick="YouTubeUpload"
                                         ImageUrl="image/MakeItSoButton.png" 
                                        onmouseout="this.src='image/MakeItSoButton.png'" onmouseover="this.src='image/MakeItSoButtonSelected.png'"  />
                                </div>
                            </div>
                        </asp:Panel>
                        
                </div>
                <div align="right" style="float: right; padding-right: 40px; width: 300px;"><asp:Label runat="server" CssClass="AddLink" ID="StatusLabel"></asp:Label></div>
               <asp:ImageButton runat="server" OnClick="PostAnswer" ID="OnwardsButton" ImageUrl="~/image/OnwardsButton.png"  onmouseout="this.src='image/OnwardsButton.png'" onmouseover="this.src='image/OnwardsButtonSelected.png'"   />
                
            </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="YourAnswerPanel" Visible="false">
            
            <div id="topDiv" class="EventDiv" >
                <div style="width: 400px; float: left;" class="Paddings">
                   <label><b>You Answered:</b><asp:Label runat="server" ID="ShortAnswerLabel"></asp:Label> <br />
                   <label><b>On:</b><asp:Label runat="server" ID="DateLabel"></asp:Label> <br />
                   <asp:Label CssClass="EventBody" runat="server" ID="AnswerLabel"></asp:Label></label>
                </div>
                <div style="height: 245px; border: solid 3px #515151;">
                    <asp:Literal runat="server" ID="MediaLiteral"></asp:Literal>
                </div>
            </div>
            
        </asp:Panel>
        <div style="padding-top: 70px;">
            <ctrl:PollAnswers runat="server" ID="PollAnswers" />
        </div>
    </div>
</asp:Content>

