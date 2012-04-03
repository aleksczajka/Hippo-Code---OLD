<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Header.ascx.cs" Inherits="Header" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register TagName="BlueButton" TagPrefix="ctrl" Src="~/Controls/BlueButton.ascx" %>
<script src="js/TotalJS.js" type="text/javascript"></script>
<script type="text/javascript">timedout();</script> 
<script type="text/javascript">
    function OpenDropDown()
    {
        var theDiv = document.getElementById('MyAccountDrop');
        
        if(theDiv.className == 'MyAccountTable')
        {
            theDiv.className = 'MyAccountTableOpen';
        }
        else
        {
            theDiv.className = 'MyAccountTable';
        }
    }
    function OpenLogin()
    {
        var theDiv = document.getElementById('AccountLogin');
        
        theDiv.style.display = 'block';
    }
    
    function HideLogin()
    {
        var theDiv = document.getElementById('AccountLogin');
        
        theDiv.style.display = 'none';
    }
</script>
<div class="topDiv TextNormal" align="center">
    <div class="HeaderBack" align="center">
        <div class="headerDiv">
        <div align="left" class="headerInside">
            <div class="Floaty"><a href="home">
            <img class="imgBorder" src="NewImages/CloudsHeader.png" alt="HippoHappenings Logo"></a></div>
            <div class="FloatyRight NoPad">
                
                <asp:Panel runat="server" ID="LoginPanel" Visible="false">
                    <div align="right">
                        <div class="MyAccount">
                           <div class="MyAccountLeft" onclick="window.location = 'my-account';">
                           </div>
                           <div class="MyAccountRight" onmouseover="OpenDropDown();" onclick="OpenDropDown();">
                           </div>
                        </div>
                        <table class="MyAccountTable" cellpadding="0" cellspacing="0" id="MyAccountDrop">
<%--                            <tr onmouseover="OpenDropDown();">
                                <td colspan="3"></td>
                            </tr>--%>
                            <tr>
                                <td onmouseover="OpenDropDown();" width="20px">&nbsp;</td>
                                <td>
                                    <div align="left" class="MyAccountDrop">
                                        <div class="MyAccountContent"><a class="NavyLink12" href="my-account">My Account</a></div>
                                        <div class="MyAccountContent"><a class="NavyLink12" href="my-pages">My Pages</a></div>
                                        <div class="MyAccountContent"><asp:LinkButton ID="LinkButton1" runat="server" CssClass="NavyLink12" OnClick="GoTo" CommandArgument="O" Text="Log Out"></asp:LinkButton></div>
                                    </div>
                                </td>
                                <td onmouseover="OpenDropDown();" width="20px"></td>
                            </tr>
                            <tr onmouseover="OpenDropDown();">
                                <td colspan="3" height="20px;">&nbsp;</td>
                            </tr>
                        </table>
                        
                    </div>
                </asp:Panel>
                <div style="position: relative;">
                    <asp:Panel runat="server" DefaultButton="FakeLoginButton">
                        <asp:Panel runat="server" ID="LoginButtonPanel" Visible="true">
                            <a onmouseover="OpenLogin()" href="login"><img alt="Log In" class="TopButton imgBorder" src="../NewImages/LogInButton.png" /></a>
                        </asp:Panel>
                            
                            <div style="z-index: 1000;background-color: #ebe7e7; border: solid 1px #09718f; display: none;position: absolute;" id="AccountLogin">
                                <table cellpadding="0" cellspacing="0">
                                    <tr ><td height="20px;">&nbsp;</td><td></td><td>&nbsp;</td></tr>
                                    <tr><td width="20px" onmouseout="HideLogin()">&nbsp;</td><td>
                                        <div style="padding: 10px;">
                                            <a class="NavyLink12UD" href="http://HippoHappenings.com/Register">Register</a><br />
                                            <h2>Log In</h2>
                                            <rad:RadTextBox runat="server" ID="LoginTextBox" EmptyMessage="UserName"></rad:RadTextBox>
                                            <rad:RadTextBox runat="server" ID="PswTextBox" TextMode="Password" EmptyMessage="Password"></rad:RadTextBox>
                                            <div style="float: right;"> 
                                                <ctrl:BlueButton runat="server" ID="LoginButton" BUTTON_TEXT="Log In" />
                                            </div>
                                            <div style="display: none;">
                                                <asp:Button runat="server" ID="FakeLoginButton" OnClick="LogIn" />
                                            </div>
                                            <div style="clear: both;">
                                                <asp:Label runat="server" ID="StatusLabel" ForeColor="red"></asp:Label>
                                            </div>
                                        </div>
                                    </td><td width="20px" onmouseout="HideLogin()">&nbsp;</td></tr>
                                    <tr onmouseout="HideLogin()"><td height="20px;">&nbsp;</td><td></td><td>&nbsp;</td></tr>
                                </table>
                                
                            </div>
                    </asp:Panel>
                </div>
            </div>
            <div class="footerSocial2">
<%--                <a id="facebook" target="_blank" href="http://www.facebook.com/#!/pages/HippoHappenings/141790032527996?ref=ts"><img alt="Facebook" name="thee Facebook" title="thee Facebook" class="facebookSocial" src="NewImages/FacebookButton.png" /></a>
--%>                

<a target="_blank" href="http://www.twitter.com/HippoHappenings" class="FloatRight"><img class="StuffBox" src="../NewImages/follow-twitter.png" alt="Follow HippoHappenings on Twitter"/></a>
<a href="http://www.facebook.com/#!/pages/HippoHappenings/141790032527996" target="_blank" class="FloatRight FacebookTop"><img border="0" src="../NewImages/follow-facebook.png" title="http://facebooklogin.ws/2009/12/facebook-login/" alt="http://facebooklogin.ws/2009/12/facebook-login/"></a>

<%--                <a id="twitter" target="_blank" href="http://twitter.com/HippoHappenings"><img alt="Twitter" class="facebookSocial" name="thee Twitter" title="thee Twitter" src="NewImages/TwitterButton.png" /></a>
--%>            </div>
        </div>
        </div>
        <div align="center" class="WholeNav">
                <div class="InnerNav">
                    <div class="NavWrapper">
                        <div onclick="this.style.cursor = 'pointer';"  class="NoPad Floaty">
                                 <h1 class="NoPad"><a id="home-Img" href="../home" class="NewNav">Home</a></h1>

    <%--                        <asp:Button CssClass="NavBarImageHome" runat="server" ID="HomeLink" OnClick="GoTo" CommandArgument="H" />
    --%>                    </div>
                        <%--<div style="padding: 0px; margin: 0px; float: left;">
                            <asp:Button CssClass="NavBarImageBlog" runat="server" ID="BlogAnEventLink" OnClick="GoTo" CommandArgument="B" />
                        </div>--%>
                        <div  class="NoPad Floaty">
                        <h1 class="NoPad"><a id="event-Img" onclick="this.style.cursor = 'pointer';" class="NewNav" href="event-search">Events</a></h1>
    <%--                        <asp:Button CssClass="NavBarImageEvent" runat="server" ID="EventLink" OnClick="GoTo" CommandArgument="E" />
    --%>                    </div>
                        <div onclick="this.style.cursor = 'pointer';"   class="NoPad Floaty">
                            <h1 class="NoPad"><a id="venue-Img" href="venue-search" class="NewNav">Locales</a></h1>
                        </div>
                        <div onclick="this.style.cursor = 'pointer';"  class="NoPad Floaty">
                            <h1 class="NoPad"><a id="adventure-Img" href="trip-search" class="NewNav">Adventures</a></h1>
                        </div>
                        <%--<div onclick="this.style.cursor = 'pointer';"  class="NoPad Floaty">
                                            <h1 class="NoPad"><a id="ad-Img" href="ad-search" class="NewNavGreen">Bulletins</a></h1>

                          <asp:Button CssClass="NavBarImageAd" runat="server" ID="AdsLink" OnClick="GoTo" CommandArgument="A" AlternateText="Ads And Clasifieds" />
                        </div>--%>
                        <%--<div onclick="this.style.cursor = 'pointer';"  class="NoPad Floaty">
                            <h1 class="NoPad"><a id="blog-Img" href="http://blog.hippohappenings.com" class="NewNavPurple"><img src="NewImages/BlogLink.png" /></a></h1>
                        </div>--%>
                       
                        <asp:Panel runat="server" ID="EventPanel" Visible="false">
                            <div onclick="this.style.cursor = 'pointer';"  class="NoPad FloatyRight">
                                            <h1 class="NoPad"><a href="blog-event" class="NewNav">+ Add Event</a></h1>

                            </div>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="VenuePanel" Visible="false">
                            <div onclick="this.style.cursor = 'pointer';"  class="NoPad FloatyRight">
                                            <h1 class="NoPad"><a href="enter-locale" class="NewNav">+ Add Locale</a></h1>

                            </div>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="AdPanel" Visible="false">
                            <div onclick="this.style.cursor = 'pointer';" class="NoPad FloatyRight">
                                            <h1 class="NoPad"><a href="post-bulletin" class="NewNavGreen">+ Add Bulletin</a></h1>

                            </div>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="TripPanel" Visible="false">
                            <div onclick="this.style.cursor = 'pointer';" class="NoPad FloatyRight">
                                            <h1 class="NoPad"><a href="enter-trip" class="NewNav">+ Add Adventure</a></h1>

                            </div>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="AboutPanel" Visible="false">
                            <div onclick="this.style.cursor = 'pointer';" class="NoPad FloatyRight">
                                           <h1 class="NoPad"><a href="about" class="NewNav">About</a></h1>

                            </div>
                        </asp:Panel>
                        <%--<asp:Panel runat="server" ID="HippoPanel" Visible="false">
                            <div onclick="this.style.cursor = 'pointer';" class="NoPad FloatyRight">
                                            <h1 class="NoPad"><a href="hippo-points" class="NewNav">What is a Hippo Boss?</a></h1>

                            </div>
                        </asp:Panel>--%>
                    </div>
                    <script type="text/javascript">
                        function setHome()
                        {
                            var home_Img = document.getElementById('home-Img');
                            home_Img.className = 'NewNavHover';
                        }
                        function setEvent()
                        {
                            var home_Img = document.getElementById('event-Img');
                            home_Img.className = 'NewNavHover';
                        }
                        function setVenue()
                        {
                            var home_Img = document.getElementById('venue-Img');
                            home_Img.className = 'NewNavHover';
                        }
                        function setAd()
                        {
                            var home_Img = document.getElementById('ad-Img');
                            home_Img.className = 'NewNavGreenHover';
                        }
                        function setTrip()
                        {
                            var home_Img = document.getElementById('adventure-Img');
                            home_Img.className = 'NewNavHover';
                        }
                        if(window.location.pathname == '/home')
                        {
                            var home_Img = document.getElementById('home-Img');
                            home_Img.className = 'NewNavHover';
                            home_Img.onmouseout = "setHome();";
                        }
                        else if(window.location.pathname == '/Event.aspx' || window.location.pathname.toLowerCase() == '/event-search')
                        {
                            var home_Img = document.getElementById('event-Img');
                            home_Img.className = 'NewNavHover';
                            home_Img.onmouseout = "setEvent();";
                        }
                        else if(window.location.pathname == '/Venue.aspx' || window.location.pathname.toLowerCase() == '/venue-search')
                        {
                            var home_Img = document.getElementById('venue-Img');
                            home_Img.className = 'NewNavHover';
                            home_Img.onmouseout = "setVenue();";
                        }
                        else if(window.location.pathname == '/Ad.aspx' || window.location.pathname.toLowerCase() == '/ad-search')
                        {
                            var home_Img = document.getElementById('ad-Img');
                            home_Img.className = 'NewNavGreenHover';
                            home_Img.onmouseout = "setAd()";
                        } 
                        else if(window.location.pathname == '/Trip.aspx' || window.location.pathname.toLowerCase() == '/trip-search')
                        {
                            var home_Img = document.getElementById('adventure-Img');
                            home_Img.className = 'NewNavHover';
                            home_Img.onmouseout = "setTrip()";
                        }

                    </script>
                </div>
          </div> 
        
    </div>

</div>