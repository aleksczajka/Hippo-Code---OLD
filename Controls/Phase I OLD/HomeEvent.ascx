<%@ Control Language="C#" AutoEventWireup="true" CodeFile="HomeEvent.ascx.cs" Inherits="Controls_HomeEvent" %>
    <div style="padding-top: 20px;">
        <table align="center">
            <tr>
                <td width="50px" valign="top">
                    
                    <table id="theTable" runat="server" style="border: solid 1px #1fb6e7; cursor: pointer;" cellpadding="0" cellspacing="0" width="100%">
                        <tbody>
                            <tr>
                                <td align="center" bgcolor="#1b1b1b">
                                    <div>
                                        <asp:Label runat="server" ID="DayLabel" CssClass="HomeDay"></asp:Label>
                                    </div>
                                    <div>
                                        <asp:Label runat="server" ID="MonthLabel" CssClass="HomeMonth"></asp:Label>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" bgcolor="#1fb6e7">
                                    <div>
                                        <asp:Label runat="server" ID="DayNumberLabel" CssClass="HomeNumber"></asp:Label>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
                <td width="400px" valign="top">
                    <div>
                        <asp:HyperLink runat="server" ID="EventNameLabel" CssClass="HomeEvent"></asp:HyperLink>
                    </div>
                    <div>
                        <asp:Label runat="server" ID="SummaryLabel" CssClass="HomeSummary"></asp:Label>
                    </div>
                    <div>
                        <asp:HyperLink runat="server" CssClass="AddLink" ID="ReadMoreLink" Text="Read More..."></asp:HyperLink>
                    </div>
                </td>
            </tr>
        </table>
    </div>
