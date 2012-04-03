<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Ads.ascx.cs" Inherits="Ads" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<div class="topDiv" style="clear: both;">
<asp:Label runat="server" ID="ErrorLabel" ForeColor="Red"></asp:Label>
                                    <div class="FooterBottom" onmousewheel="">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <script type="text/javascript">


                                                    function StartRotator2()
                                                    {
                                                        var rotator = $find('<%= Rotator2.ClientID %>');
        //                                                rotator.showNext(Telerik.Web.UI.RotatorScrollDirection.Left);
                                                        if(rotator != undefined && rotator != null)
                                                        {
                                                            rotator.autoIntervalID = window.setInterval(function() {
                                                                rotator.showNext(Telerik.Web.UI.RotatorScrollDirection.Left);
                                                            }, 10000);
                                                        }
                                                        
                                                    }
                                                    
                                                    function StopRotator2()
                                                    {
                                                        var rotator = $find('<%= Rotator2.ClientID %>');
                                                        window.clearInterval(rotator.autoIntervalID);
                                                        rotator.autoIntervalID = null;
                                                    }
                                                    
                                                </script>
                                                
                                                <div onmouseover="StopRotator2()" onmouseout="StartRotator2()" 
                                                    class="AdsClass">
                                                    <rad:RadRotator ID="Rotator2" Skin="Black" runat="server" WrapFrames="true"
                                                        Width="848px" Height="264px" 
                                                        ItemHeight="262px" FrameDuration="15000" ItemWidth="202px" 
                                                        RotatorType="buttons" 
                                                         ScrollDirection="Left,Right">
                                                    </rad:RadRotator>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                       
                                    </div>
                                    
                                    <div class="topDiv FloatRight">
                                            <div class="PostFreeBulletin">
                                                <div class="PostArrowBulletin">
                                                    <img alt="Post bulletin for free" src="NewImages/BlackPostArrow.png" />
                                                </div>
                                                <h2>Your Bulletin/Announcement here free with<br />                                                code <span style="color: #35980e;">HIPPO255</span> <br />                                                        <a href="post-bulletin">Post it.</a></h2><br />
                                                
                                            </div>
                                            
                                        </div>
</div>
                                    
                                    