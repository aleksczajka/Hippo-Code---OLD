<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ValidateRequest="false" 
AutoEventWireup="true" 
CodeFile="InsertEvents.aspx.cs" 
Inherits="InsertEvents" Title="Blog An Event" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<rad:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="false" 
                KeepInScreenBounds="true" VisibleOnPageLoad="false"  Behaviors="none"
            ID="MessageRadWindowManager" Modal="true" RestrictionZoneID="RestrictionZone"
            runat="server">
                <Windows>
                    <rad:RadWindow Width="300" ClientCallBackFunction="OnClientClose" 
                    VisibleStatusbar="false" Skin="Black" Height="350" ID="MessageRadWindow" 
                     runat="server">
                    </rad:RadWindow>
                </Windows>
            </rad:RadWindowManager>
            <script type="text/javascript">
            function OnClientClose(oWnd, args)
            {

                window.location = args;
            }
           
            </script>
<%--<rad:RadAjaxPanel runat="server">--%>
 

<div style="padding-bottom: 100px;">

    <asp:FileUpload runat="server" ToolTip="Enter File To Upload" CssClass="FileUploadStyle" 
    ID="MainFileUpload" EnableViewState="true" />
    <asp:Button runat="server" PostBackUrl="#" OnClick="ClickItVenues" ID="UploadButton" 
    CssClass="ButtonBlue" Text="Upload" />


 <asp:Panel runat="server" ID="StatusPanelPanel" Visible="false">
                        <label>Upload Summary</label>
                        <asp:TextBox runat="server" Height="150px" Width="100%" ID="StatusPanel" TextMode="multiLine" Wrap="true"></asp:TextBox>
                        <asp:Button ID="Button2" runat="server" CssClass="ButtonBlue" Text="Close" OnClick="CloseSummary" />
                    </asp:Panel>
    </div>
    

<%--</rad:RadAjaxPanel>--%>
</asp:Content>

