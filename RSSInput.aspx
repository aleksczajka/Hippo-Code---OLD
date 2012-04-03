<%@ Page Language="C#" MasterPageFile="~/SecondMasterPage.master" EnableViewState="false" AutoEventWireup="true" 
CodeFile="RSSInput.aspx.cs" Inherits="RSSInput" Title="About HippoHappenings" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <div class="TextNormal PagePad">
  
    RSS Feed: <asp:TextBox runat="server" ID="RSSTextBox"></asp:TextBox><asp:Button runat="server" Text="Get It" OnClick="GetRSSFeed" />
    <br />
    <asp:TextBox runat="server" ID="RSSDisplayTextBox" TextMode="MultiLine" Width="300px" Height="300px"></asp:TextBox>
  
    <br /><br />
    
    <asp:TextBox runat="server" ID="ElementsPathTextBox" Text="ElementsPath"></asp:TextBox>, 
    <asp:DropDownList runat="server" ID="VenuesDropDown"></asp:DropDownList>
    <asp:TextBox runat="server" ID="LinkPathTextBox" Text="LinkPath"></asp:TextBox>, 
    <asp:TextBox runat="server" ID="TitlePathTextBox" Text="TitlePath"></asp:TextBox>, 
    <asp:TextBox runat="server" ID="DescriptionPathTextBox" Text="DescriptionPath"></asp:TextBox>,
    <asp:TextBox runat="server" ID="DatePathTextBox" Text="DatePath"></asp:TextBox>,
    <asp:TextBox runat="server" ID="TimePathTextBox" Text="TimePath"></asp:TextBox>, 
    DateTimeTogether: <asp:DropDownList runat="server" ID="DateTimeTogetherDropDown">
    <asp:ListItem Text="True"></asp:ListItem>
    <asp:ListItem Text="False"></asp:ListItem>
    </asp:DropDownList>, FixDate: <asp:DropDownList runat="server" ID="FixDateDropDown">
    <asp:ListItem Text="True"></asp:ListItem>
    <asp:ListItem Text="False"></asp:ListItem>
    </asp:DropDownList>, ImgInContent: <asp:DropDownList runat="server" ID="ImgInContentDropDown">
    <asp:ListItem Text="True"></asp:ListItem>
    <asp:ListItem Text="False"></asp:ListItem>
    </asp:DropDownList>
    <asp:Button runat="server" Text="Upload" OnClick="Upload" />
    
    <br /><br />
    
    <asp:Button runat="server" Text="Upload all Upcoming RSS Feeds" OnClick="UploadRSSFeed" />
    <br />
    <asp:Label runat="server" ID="ErrorLabel"></asp:Label>
  </div>
</asp:Content>

