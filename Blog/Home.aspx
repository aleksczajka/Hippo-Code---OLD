<%@ Page Language="C#" MasterPageFile="~/Blog/MasterPage.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Blog_Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<tr>
        <td class="TitleContent" align="center">
            <h1>Hippo Blog</h1> <h2>&nbsp;- The Portal To What's Happening</h2>
        </td>
    </tr>
    <tr>
        <td class="MainContent" align="center" valign="top">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td class="LeftAds" valign="top">
                        <div>
                            <script type="text/javascript"><!--
                            google_ad_client = "ca-pub-3961662776797219";
                            /* Hippo 3rd */
                            google_ad_slot = "3544791719";
                            google_ad_width = 160;
                            google_ad_height = 600;
                            //-->
                            </script>
                            <script type="text/javascript"
                            src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
                            </script>
                        </div>
                    </td>
                    <td class="LeftColumn" valign="top">
                        <div class="BlueGradient">
                            <div class="GreenGradient">
                                <img src="../NewImages/BlogHippo.png" />
                                <br />
                                <h3>About</h3>
                                <div class="about">
                                    Lorem ipsum dolor sit amet, consectetur adipiscing elit. 
                                </div>
                                <h3>Contact</h3>
                                <div class="about">
                                    email us at: <a class="MailTo" href="mailto:blog@HippoHappenings.com">blog@HippoHappenings.com</a>
                                </div>
                            </div>
                        </div>
                    </td>
                    <td class="RightColumn" valign="top">
                        <asp:Literal runat="server" ID="FeaturedLiteral"></asp:Literal>
                        <div class="RestOfArticles">
                            <asp:Literal runat="server" ID="RestOfArticlesLiteral"></asp:Literal>
                        </div>
                    </td>
                    <td class="RightAds" valign="top">
                        <div>
                        <script type="text/javascript"><!--
                        google_ad_client = "ca-pub-3961662776797219";
                        /* Hippo 2nd */
                        google_ad_slot = "9877708768";
                        google_ad_width = 160;
                        google_ad_height = 600;
                        //-->
                        </script>
                        <script type="text/javascript"
                        src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
                        </script>
                    </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</asp:Content>
