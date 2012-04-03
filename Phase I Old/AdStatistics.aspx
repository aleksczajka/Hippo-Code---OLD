<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" 
AutoEventWireup="true" CodeFile="AdStatistics.aspx.cs" Inherits="AdStatistics"
 Title="Ad Statistics | HippoHappenings" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Charting" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register TagName="HippoTextBox" TagPrefix="ctrl" Src="~/Controls/HippoTextBox.ascx" %>
<%@ Register Src="~/Controls/UserMessage.ascx" TagName="UserMessage" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/GlanceDay.ascx" TagName="GlanceDay" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/Pager.ascx" TagPrefix="ctrl" TagName="Pager" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <rad:RadWindowManager Behaviors="Close, Move, Resize,Maximize"
            ID="RadWindowManager" Modal="true" Skin="Black" DestroyOnClose="false" runat="server">
            <Windows>
                <rad:RadWindow ID="RadWindow1" VisibleStatusbar="false"  
                VisibleTitlebar="false" VisibleOnPageLoad="false" Height="506" Width="560" 
                Title="Send event information to your friends"
                     runat="server">
                </rad:RadWindow>
                <rad:RadWindow ID="RadWindow2"  VisibleStatusbar="false" BorderWidth="0" 
                VisibleTitlebar="false" VisibleOnPageLoad="false" Height="200" Width="438" 
                Title="Set Communication Preferences" runat="server">
                </rad:RadWindow>
                
            </Windows>
            
        </rad:RadWindowManager>
    <div class="EventDiv" style="width: 900px;">
 
        <h1 class="searches">Your Ad Statistics</h1>
      
        <div style="padding-left: 30px;"><label>Select one of your ads to view statistics</label>
        <asp:DropDownList runat="server" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="GetAdChart" ID="AdDropDown"></asp:DropDownList>
        </div>
        <br /><br /><br /><br />
      <asp:Panel runat="server" Visible="false" ID="ChartPanel" Width="853px">
        <table width="853px">
            <tr>
                <td valign="top" width="200px" style="padding-bottom: 20px;">
                    <label><asp:Label runat="server" ID="TitleLabel"></asp:Label></label>
                </td>
            </tr>
            <tr>
                <td>
                   <rad:RadChart ID="ConsumerRadChart" Skin="DeepRed" Width="853px" 
                        DefaultType="Bar" runat="server"  Height="400px">
                           <Legend Appearance-Position-Auto="true" Appearance-Position-AlignedPosition="Right" Appearance-Dimensions-Width="200px"></Legend>
                           <PlotArea>
                            <XAxis Step="1" AxisLabel-Visible="true" ><AxisLabel></AxisLabel></XAxis>
                            <YAxis Step="1" AxisLabel-Visible="true"><AxisLabel><Appearance></Appearance>
                            <TextBlock Appearance-TextProperties-Color="white" 
                            Text="Number of Views"></TextBlock></AxisLabel></YAxis>

                            
                            <EmptySeriesMessage Visible="True">
                                <Appearance Visible="True">
                                    <Border Visible="False" />
                                </Appearance>
                                <TextBlock Text="There is no data available">
                                </TextBlock>
                            </EmptySeriesMessage>
                        </PlotArea>
                    </rad:RadChart>
                </td>
            </tr>
            <tr>
                <td>
                     <label><span style="font-size: 12px;">[<b>remember:</b> Since one particular user can be viewing your ad for multiple reasons, the numbers above the bars
                    on the right are not a direct correlation to the number of users whom have see your ad. 
                    For example, one user can be viewing your ad because it is in their favorite category 'Music'
                    and because it is in their favorite category 'Art'. And so, this one user will be counted once
                    in the 'Music' bar and once in the 'Art' bar.]</span></label>

                </td>
            </tr>
            <tr>
                <td style="padding-top: 60px;">
                    <div class="EventDiv" style="color: #cccccc;">
                        <span style="font-size: 18px; font-weight: bold;">User Statistics Details</span>
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="ErrorLabel" ForeColor="Red"></asp:Label>
                                <table cellpadding="10px" style="font-size: 12px;border: solid 1px #1fb6e7; 
                                background-color: #1b1b1b;"><tr>
                                    <td>
                                        <span style="font-size: 14px; font-weight: bold;"></span>
                                    </td>
                                    <td>
                                        <span style="font-size: 14px; font-weight: bold; text-decoration: underline;">
                                            <asp:LinkButton runat="server" ID="ReasonLinkButton" OnClick="SortReason" 
                                            Text="Reason For View" CssClass="AddOrangeLink"></asp:LinkButton>
                                        </span>
                                    </td>
                                    <td>
                                        <span style="font-size: 14px; font-weight: bold; text-decoration: underline;">
                                            <asp:LinkButton runat="server" CssClass="AddOrangeLink" ID="DateLinkButton" OnClick="SortDate" 
                                            Text="Date"></asp:LinkButton>
                                        </span>
                                    </td>
                                    <td>
                                        <span style="font-size: 14px; font-weight: bold; text-decoration: underline;">
                                            <asp:LinkButton runat="server" CssClass="AddOrangeLink" ID="EmailLinkButton" OnClick="SortEmail" 
                                            Text="Email View"></asp:LinkButton>
                                        </span>
                                    </td>
                                    </tr>
                                    <asp:Panel runat="server" ID="PanelofUsers"></asp:Panel>
                                
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
        </table>
      </asp:Panel>
      
    </div>

</asp:Content>

