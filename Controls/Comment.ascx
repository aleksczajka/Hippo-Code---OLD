<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Comment.ascx.cs" Inherits="Comment" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<telerik:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="false" 
    KeepInScreenBounds="true" VisibleOnPageLoad="false" Behaviors="none"
ID="MessageRadWindowManager" BackColor="#1b1b1b" Modal="true" DestroyOnClose="true" RestrictionZoneID="RestrictionZone"
runat="server">
    <Windows>
        <telerik:RadWindow Width="400" EnableEmbeddedSkins="true" IconUrl="image/ContactIcon.png" 
        Skin="Web20" Height="450" VisibleStatusbar="false" ID="MessageRadWindow" Title="Sent Message" 
        runat="server">
        </telerik:RadWindow>
    </Windows>
</telerik:RadWindowManager>
<script type="text/javascript">
    function OpenCommunicate(theID)
        {
            var win = $find("<%=MessageRadWindow.ClientID %>");
            win.show(); 
            win.center(); 
               
         }
</script>
<div class="CommentPageTop">
<asp:Label runat="server" ID="ErrorLabel" ForeColor="Red"></asp:Label>
     <div class="topDiv CommentPageTopInner">
                <div class="aboutLink">
                    <div class="FloatLeft"><img alt="Background Image" src="NewImages/BackLeftTopCorner.png" /></div>
                    <div class="CommentPagerTopInnerInner">
                        &nbsp;
                    </div>
                    <div class="FloatLeft"><img alt="Background Image" src="NewImages/BackRightTopCorner.png" /></div>
                </div>
                <div class="LocaleLink7"> 
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="15px" class="CommentPageInner"></td>
                            <td>
                                <div class="Text12 CommentPagerInnerInner">
                                        <div>
                                            <div class="CommentPagerA">
                                            <table align="center" bgcolor="#dedbdb" valign="middle" cellpadding="0" cellspacing="1">
                                            <tr>
                                            <td align="center" valign="middle">
                                                <table width="52px" align="center" height="52px" bgcolor="#dedbdb" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="center">
                                                            <asp:ImageButton  runat="server" ID="ProfileImage"/>
                                                        </td>
                                                    </tr>
                                                </table>
                                                
                                                </td>
                                                </tr>
                                                </table>
                                                    </div>
                                                <div class="Text12" >
                                                    <asp:Panel runat="server" ID="CommentPanel"><div class="FloatRight"><img alt="Send a message" name="Send a message!" class="CommentPagerD" src="NewImages/CommentBlurb.png" onclick="OpenCommunicate();" ID="ConnectButton" /></div></asp:Panel>
                                                    Posted on <asp:Label runat="server" CssClass="Text12" ID="DateLabel"></asp:Label><br />
                                                    Posted by <h1 class="UserNameH1"><asp:HyperLink runat="server" CssClass="NavyLink12" ID="UserLabel"></asp:HyperLink></h1><br />
                                                    <asp:Literal runat="server" ID="CommentLabel"></asp:Literal>
                                                </div>
                                            
                                        </div>
                                    <div>
                                        <asp:Literal runat="server" ID="ImageLiteral"></asp:Literal>
                                    </div>
                                 </div>
                            </td>
                            <td width="15px" class="CommentPagerB"></td>
                        </tr>
                    </table>                       
                </div>
                <div class="aboutLink">
                    <div class="FloatLeft"><img alt="Background Image" src="NewImages/BackLeftBottomCorner.png" /></div>
                    <div class="CommentPagerC">
                        &nbsp;
                    </div>
                    <div class="FloatLeft"><img alt="Background Image" src="NewImages/BackRightBottomCorner.png" /></div>
                </div>
           </div>
</div>