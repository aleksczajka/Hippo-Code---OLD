<%@ Page Language="C#" MasterPageFile="~/Blog/MasterPage.master" AutoEventWireup="true" CodeFile="Article.aspx.cs" Inherits="Blog_Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<tr>
        <td class="TitleContent" align="center">
            <h1><asp:Literal runat="server" ID="TitleLiteral"></asp:Literal></h1>
        </td>
    </tr>
    <tr>
        <td class="MainContent" align="center" valign="top">
            <table>
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
                    <td class="ArticleContent" valign="top" align="center">
                        <asp:Image  runat="server" ID="MainImage" />
                        <h2 class="Article"><asp:Literal runat="server" ID="TagLineLiteral"></asp:Literal></h2>
                        <div class="ArticleMainContent" align="center">
                            <asp:Literal runat="server" ID="MainContentLiteral"></asp:Literal>
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
                <tr>
                    <td colspan="3" align="center">
                        <div class="GooglyLinks">
                            <script type="text/javascript"><!--
                                google_ad_client = "ca-pub-3961662776797219";
                                /* Hippo Link Unit */
                                google_ad_slot = "6130116935";
                                google_ad_width = 728;
                                google_ad_height = 15;
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

