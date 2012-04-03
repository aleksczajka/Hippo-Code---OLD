<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PostAPoll.aspx.cs" Inherits="PostAPoll" Title="Post A Poll" %>
<%@ Register Src="~/Controls/Ads.ascx" TagName="Ads" TagPrefix="ctrl" %>
<%@ Register TagName="HippoTextBox" TagPrefix="ctrl" Src="~/Controls/HippoTextBox.ascx" %>
<%@ Register Src="~/Controls/Poll.ascx" TagName="Poll" TagPrefix="ctrl" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div id="topDiv">
         <div style="float: left; width: 440px;">
                <asp:Label ID="Label1" runat="server" CssClass="EventHeader" Text="Create a poll"></asp:Label>
                <asp:Panel runat="server" ID="LoginPanel">
                    <div class="AddLinkGoing" align="center" style="padding-left: 20px; padding-top: 30px;">
                       <label>You must be <asp:HyperLink ID="HyperLink1" runat="server" CssClass="AddLink" NavigateUrl="~/UserLogin.aspx" Text="logged in"></asp:HyperLink> to submit a poll question! </label>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="PollPanel" Visible="false">
                    <div style="font-family: Arial; font-size: 12px; color: #cccccc; line-height: 20px;">
                    All questions will be chosen at random <br /><br />
                    Your question: <br />
                    <span style="font-family: Arial; font-size: 10px; color: #666666;">max 250 characters</span><br />
                    <asp:TextBox runat="server" ID="QuestionTextBox" Width="340px" Height="85px"></asp:TextBox><br />
                    
                    <br />
                    Possible Poll Answers<br />
                    <span style="font-family: Arial; font-size: 10px; color: #666666;">max 100 characters</span>
                    <ctrl:HippoTextBox runat="server" ID="Answer1TextBox" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                    <br /><br /><br />
                    <span style="font-family: Arial; font-size: 10px; color: #666666;">max 100 characters</span>
                   
                    <ctrl:HippoTextBox runat="server" ID="Answer2TextBox" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
            
                    <br /><br /><br />
                    <span style="font-family: Arial; font-size: 10px; color: #666666;">max 100 characters</span><br />
                    <ctrl:HippoTextBox runat="server" ID="Answer3TextBox" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                    <br /><br /><br />
                    <div align="right" style="clear: both; padding-right: 50px;"><asp:ImageButton runat="server" ID="AdsLink" OnClick="CreatePoll" AlternateText="Click to create the Poll!" ImageUrl="~/image/DoneSonButton.png"  onmouseout="this.src='image/DoneSonButton.png'" onmouseover="this.src='image/DoneSonButtonSelected.png'"  />
                    </div>
                    <div>
                        <asp:Label runat="server" ID="MessageLabel" CssClass="AddLink"></asp:Label>
                    </div>
                </div>
                </asp:Panel>
         </div>
         <div style="float: left; padding-left: 5px; width: 419px;">
            <div style="clear: both;">
             <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" 
            codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0" 
            width="431" height="575" title="banner" wmode="transparent">  
            <param name="movie" value="gallery.swf" />  
            <param name="quality" value="high" />
            <param name="wmode" value="transparent"/>   
                <embed src="gallery.swf" quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer" 
                type="application/x-shockwave-flash" width="431" height="575" wmode="transparent">
                </embed>
            </object>
                <%--<ctrl:Ads ID="Ads1" runat="server" />--%>
            </div>
            <div style="padding-left: 5px; width: 419px;">
                <div align="center" style=" padding-top: 13px ;clear: both ;vertical-align: middle; font-weight: bold;background-color: #686868; height: 27px; font-family: Arial; font-size: 12px; color: White;">
                    Want to have your ad featured? fill out your information <a href="PostAnAd.aspx" style="color: #1fb6e7; text-decoration: none;">here!</a>
                </div>
                <div style="padding-top: 15px;">
                    <ctrl:Poll ID="Poll1" runat="server"  />
                </div>
                
            </div>
        </div>
     </div>
</asp:Content>

