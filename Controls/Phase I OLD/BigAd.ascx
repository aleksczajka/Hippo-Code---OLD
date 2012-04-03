<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BigAd.ascx.cs" Inherits="Controls_BigAd" %>

<asp:Label runat="server" ForeColor="red" ID="Errorlabel"></asp:Label>

<div align="center" style="background-image: url(images/BigBuilBord.gif); width: 437px; height: 321px; margin-top: -30px;">
<img src="images/LocalBoard.png" style="padding-top: 10px; padding-bottom: 8px;"  />
    <asp:Timer ID="Timer1" OnTick="Timer1_Tick" runat="server" Interval="20000">
    </asp:Timer>
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" RenderMode="block" ChildrenAsTriggers="false" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel runat="server" ID="Template1Panel" Visible="false">
                <div style="height: 206px; width: 419px; background-color: #686868;">
                 <div class="topDiv" style="width: 419px; height: 206px; background-color: #686868;"> 
                   <div style="padding-top: 5px;padding-left: 3px; float: left;">
                   <a id="A1" runat="server" >
                   <div style="float: left;"><table width='212px' height='190px'
                    cellpadding="0" cellspacing="0"><tbody align="center"><tr>
                    <td valign="middle">
                   <asp:Image ID="CustomerImage" runat="server" ImageAlign="left" />
                   </td></tr></tbody></table></div>
                   </a>
                    </div>    
                   <div style="padding-top: 5px; padding-left: 5px; float:right; width: 190px; padding-right: 5px;">
                        <asp:HyperLink CssClass="AdTitle" runat="server" ID="TitleLabel" ></asp:HyperLink><br />
                        <asp:Label runat="server" CssClass="AdBody"  ID="BodyLabel"></asp:Label>
                        <div style="float: left; padding-right: 8px; height: 10px; width: 80px; padding-top: 5px; padding-left: 3px;">
                            <div style="float: left;padding-top: 5px; clear: none;"><img src="image/ReadMoreArrow.png" />
                            </div>
                            <div style="float: right;clear: none;"><asp:HyperLink ID="ReadMoreLink" CssClass="ReadMoreLink" 
                            runat="server" Text="Read More" ></asp:HyperLink>
                            </div>
                        </div>
                    </div>
                </div> 
            </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="Template2Panel" Visible="false">
                <div style="height: 206px; width: 419px; background-color: #686868;">
                 <div class="topDiv" style="width: 419px; height: 206px; background-color: #686868;"> 
                   <div style="padding-top: 5px;padding-left: 3px; float: left;">
                   <a id="A2" runat="server" >
                   <div style="float: left;"><table width='415px' height='190px'
                    cellpadding="0" cellspacing="0"><tbody align="center"><tr>
                    <td valign="middle">
                   <asp:Image ID="Image1" runat="server" ImageAlign="left" />
                   </td></tr></tbody></table></div>
                   </a>
                    </div> 
                </div> 
            </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>


<div style=" width: 419px;">
        <div align="center" style="float: left; padding: 3px; margin-left: 3px; margin-top: 15px; border: 2px solid white; background-color: rgb(27, 27, 27); clear: both; line-height: 10px; vertical-align: middle; font-family: Arial; font-size: 10px; color: rgb(204, 204, 204);">
            *A bulletin shows for 20sec, every 10minutes, for 24 hours to one particular viewer. <br>
            To post a bulettin go to <a style="color: rgb(31, 182, 231);" href="PostAnAd.aspx">Post An Ad</a>. 
            To specify what type of bulletins you'd like to see when logged in go to <a href="User.aspx?p=true#adPfs" style="color: rgb(31, 182, 231);">My Account</a>.
        </div>
    </div>
</div>