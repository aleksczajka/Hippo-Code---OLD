using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.IO;

public partial class Ad : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }

        Session["RedirectTo"] = Request.Url.AbsoluteUri;
        bool fillUserData = false;
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        
        try
        {
            if (Session["User"] != null)
            {
                fillUserData = true;
                DataSet dsAd = dat.GetData("SELECT User_ID FROM Ads WHERE Ad_ID=" + Request.QueryString["AdID"].ToString());
                if (dsAd.Tables[0].Rows[0]["User_ID"].ToString() == Session["User"].ToString())
                {
                    EditAdLink.Visible = true;
                }
                else
                    EditAdLink.Visible = false;

                LoggedInPanel.Visible = true;
                LoggedOutPanel.Visible = false;
            }
            else
            {
                Button calendarLink = (Button)dat.FindControlRecursive(this, "CalendarLink");
                LoggedOutPanel.Visible = true;
                LoggedInPanel.Visible = false;
            }

            GetFeaturedBulletins();
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString();
        }


        int ID = int.Parse(Request.QueryString["AdID"].ToString());

        Session["FlagID"] = ID;
        Session["FlagType"] = "A";
        DataSet ds = dat.GetData("SELECT * FROM Ads A, Users U WHERE U.User_ID=A.User_ID AND A.Ad_ID=" + ID);
        Cache.Remove(Server.MapPath("Controls/PlayList.xml"));
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                TagCloud.THE_ID = ID;
                ShowHeaderName.Text = "<a id=\""+dat.MakeNiceNameFull(ds.Tables[0].Rows[0]["Header"].ToString())+"\" class=\"aboutLink\" href=\"http://" + Request.Url.Authority + "/" +
                    dat.MakeNiceName(ds.Tables[0].Rows[0]["Header"].ToString()) + 
                    "_" + ID.ToString() + "_Ad\"><h1>" +
                    dat.BreakUpString(ds.Tables[0].Rows[0]["Header"].ToString(), 50)+"</h1></a>";
                ShowDescription.Text = dat.BreakUpString(ds.Tables[0].Rows[0]["Description"].ToString(), 60);


                #region SEO
                //Create keyword and description meta tags
                topTopLiteral.Text = "<a class=\"NavyLink12UD\" href=\"#" + dat.MakeNiceNameFull(ds.Tables[0].Rows[0]["Header"].ToString()) + "\">" + 
                    dat.MakeNiceNameFull(ds.Tables[0].Rows[0]["Header"].ToString()).Replace("-", " ") + " From The Top</a>";

                HtmlMeta hm = new HtmlMeta();
                HtmlMeta kw = new HtmlMeta();
                HtmlMeta lg = new HtmlMeta();
                HtmlLink cn = new HtmlLink();
                HtmlHead head = (HtmlHead)Page.Header;

                cn.Attributes.Add("rel", "canonical");
                cn.Href = "http://" + Request.Url.Authority + "/" +
                    dat.MakeNiceName(ds.Tables[0].Rows[0]["Header"].ToString()) +
                    "_" + ID.ToString() + "_Ad";
                head.Controls.AddAt(0, cn);

                kw.Name = "keywords";
                hm.Name = "Description";

                lg.Name = "language";
                lg.Content = "English";
                head.Controls.AddAt(0, lg);

                char [] delimeter = {' '};
                string[] keywords = dat.MakeNiceNameFull(ds.Tables[0].Rows[0]["Header"].ToString()).Replace("-", " ").Split(delimeter, StringSplitOptions.RemoveEmptyEntries);
                int count = 0;
                foreach (string token in keywords)
                {
                    if (count < 16)
                    {
                        if (kw.Content != "")
                            kw.Content += " ";
                        kw.Content += token;
                        
                        count++;
                    }
                }
                head.Controls.AddAt(0, kw);

                hm.Content = dat.MakeNiceNameFull(dat.stripHTML(ds.Tables[0].Rows[0]["Description"].ToString()).Replace("   ", " ").Replace("  ", " ")).Replace("-", " ");
                if (hm.Content.Length > 200)
                    hm.Content = hm.Content.Substring(0, 197) + "...";

                head.Controls.AddAt(0, hm);

                this.Title = kw.Content;

                HtmlLink lk = new HtmlLink();
                lk.Href = "http://" + Request.Url.Authority + "/" + dat.MakeNiceName(dat.BreakUpString(ds.Tables[0].Rows[0]["Header"].ToString(), 14)) + "_" + ID.ToString() + "_Ad";
                lk.Attributes.Add("rel", "bookmark");
                head.Controls.AddAt(0, lk);
                #endregion


                DataView dvCats = dat.GetDataDV("SELECT DISTINCT C.ID, ACM.ID AS AID, C.Name AS CategoryName, ACM.tagSize FROM Ad_Category_Mapping ACM, AdCategories C WHERE ACM.CategoryID=C.ID AND ACM.AdID=" + ID + " ORDER BY ACM.ID");

                string justCats = "";

                for (int i = 0; i < dvCats.Count; i++)
                {
                    //kw.Content += ", " + dvCats[i]["CategoryName"].ToString();
                    justCats += dvCats[i]["CategoryName"].ToString() + " ";
                }

                

               // DiggLiteral.Text = " <table> " +
               //     "<tr>" +
               //       "  <td valign=\"bottom\" style=\"padding-right: 10px;\">" +
               //        "     <a name=\"fb_share\" type=\"button\" href=\"http://www.facebook.com/sharer.php\">Share</a><script src=\"http://static.ak.fbcdn.net/connect.php/js/FB.Share\" type=\"text/javascript\"></script>" +
               //        " </td>" +
               //        " <td valign=\"bottom\" style=\"padding-right: 10px;\">" +
               //        "     <a style=\"border: 0; padding: 0; margin: 0;\" id=\"TweeterA\" title=\"Click to send this page to Twitter!\" target=\"_blank\" rel=\"nofollow\"><img style=\"border: 0; padding: 0; margin: 0;\" src=\"http://twitter-badges.s3.amazonaws.com/twitter-a.png\" alt=\"Share on Twitter\"/></a>" +
               //        " </td>" +
               //        " <td valign=\"bottom\" style=\"padding-right: 10px;\">" +
               //        "     <a href=\"javascript:void(window.open('http://www.myspace.com/Modules/PostTo/Pages/?u='+encodeURIComponent(document.location.toString()),'ptm','height=450,width=440').focus())\">" +
               //        "         <img src=\"http://cms.myspacecdn.com/cms/ShareOnMySpace/small.png\" border=\"0\" alt=\"Share on MySpace\" />" +
               //        "     </a>" +
               //        " </td>" +
               //        "  <td valign=\"bottom\" style=\"padding-right: 10px;\"><a alt=\"Digg it!\" class=\"DiggThisButton DiggIcon\" id=\"dibbButt\"" +
               //         "href='http://digg.com/submit?phase=2&url=" + "http://" + Request.Url.Authority +
               //         "/" + dat.MakeNiceName(ds.Tables[0].Rows[0]["Header"].ToString()) + "_" + ID.ToString() + "_Ad" +
               //         "' target=\"_blank\">Digg</a></td>" +
               //       "  <td valign=\"bottom\" style=\"padding-right: 10px;\">" +

               //        "     <a href=\"http://delicious.com/save\" onclick=\"window.open('http://delicious.com/save?v=5&noui&jump=close&url='+encodeURIComponent(location.href)+'&title='+encodeURIComponent(document.title), 'delicious','toolbar=no,width=550,height=550'); return false;\">" +
               //        "         <img border=\"0\" src=\"http://static.delicious.com/img/delicious.small.gif\" height=\"10\" width=\"10\" alt=\"Delicious\" />" +
               //         "    </a>" +
               //        "</td>" +
               //        " <td>" +
               //         "     <script src=\"http://www.stumbleupon.com/hostedbadge.php?s=4\"></script>" +
               //        " </td>" +
               //    " </tr>" +
               //" </table>";

                Session["Subject"] = dat.BreakUpString(ds.Tables[0].Rows[0]["Header"].ToString(), 14);
                Session["messageText"] = "Bulletin: " +
                    dat.BreakUpString(ds.Tables[0].Rows[0]["Header"].ToString(), 14) + ".";

                Session["messageEmail"] = dat.BreakUpString(ds.Tables[0].Rows[0]["Header"].ToString(), 14) + " \n\r " + ShowDescription.Text;
                Session["EmailMessage"] = dat.BreakUpString(ds.Tables[0].Rows[0]["Header"].ToString(), 14) + " \n\r " + ShowDescription.Text;

                if (bool.Parse(ds.Tables[0].Rows[0]["hasSongs"].ToString()))
                {
                    DataSet dsSongs = dat.GetData("SELECT * FROM Ad_Song_Mapping WHERE AdID=" + ID);
                    ASP.controls_playerxml_songplayer_ascx songs = new ASP.controls_playerxml_songplayer_ascx();
                    int songCount = dsSongs.Tables[0].Rows.Count;

                    if (songCount > 2)
                    {
                        songs.SONG1 = dsSongs.Tables[0].Rows[0]["SongName"].ToString();
                        songs.SONG2 = dsSongs.Tables[0].Rows[1]["SongName"].ToString();
                        songs.SONG3 = dsSongs.Tables[0].Rows[2]["SongName"].ToString();
                        songs.SONG1_TITLE = dsSongs.Tables[0].Rows[0]["SongTitle"].ToString();
                        songs.SONG2_TITLE = dsSongs.Tables[0].Rows[1]["SongTitle"].ToString();
                        songs.SONG3_TITLE = dsSongs.Tables[0].Rows[2]["SongTitle"].ToString();
                    }
                    else if (songCount > 1)
                    {
                        songs.SONG1 = dsSongs.Tables[0].Rows[0]["SongName"].ToString();
                        songs.SONG2 = dsSongs.Tables[0].Rows[1]["SongName"].ToString();
                        songs.SONG3 = "";
                        songs.SONG1_TITLE = dsSongs.Tables[0].Rows[0]["SongTitle"].ToString();
                        songs.SONG2_TITLE = dsSongs.Tables[0].Rows[1]["SongTitle"].ToString();
                    }
                    else
                    {
                        songs.SONG1 = dsSongs.Tables[0].Rows[0]["SongName"].ToString();
                        songs.SONG2 = "";
                        songs.SONG3 = "";
                        songs.SONG1_TITLE = dsSongs.Tables[0].Rows[0]["SongTitle"].ToString();
                    }
                    songs.USER_NAME = ds.Tables[0].Rows[0]["UserName"].ToString();

                    SongPanel.Controls.Add(songs);

                    //XmlDocument xmldoc = new XmlDocument();
                    //xmldoc.Load(Server.MapPath("Controls/PlayList.xml"));

                    //Cache.Insert(Server.MapPath("Controls/PlayList.xml"), fil);

                }


                string youtube = ds.Tables[0].Rows[0]["YouTubeVideo"].ToString();
                int mediaCategory = int.Parse(ds.Tables[0].Rows[0]["mediaCategory"].ToString());
                //Media Categories: NONE: 0, Picture: 1, Video: 2, YouTubeVideo: 3, Slider: 4
                Rotator1.Items.Clear();
                switch (mediaCategory)
                {
                    case 0:
                        break;
                    case 1:
                        RotatorPanel.Visible = true;
                        char[] delim4 = { ';' };
                        string[] youtokens = youtube.Split(delim4);
                        if (youtube != "")
                        {
                            for (int i = 0; i < youtokens.Length; i++)
                            {
                                if (youtokens[i].Trim() != "")
                                {
                                    Literal literal3 = new Literal();
                                    //literal3.Text = "<object width=\"400\" height=\"250\"><param  name=\"wmode\" value=\"opaque\" ><param name=\"movie\" value=\"http://www.youtube.com/cp/vjVQa1PpcFOFUjhw1qTHaE09Z1e9QYKk9y1JrWf5VAc=\"></param><embed wmode=\"opaque\" src=\"http://www.youtube.com/cp/vjVQa1PpcFOFUjhw1qTHaE09Z1e9QYKk9y1JrWf5VAc=\" type=\"application/x-shockwave-flash\" width=\"400\" height=\"250\"></embed></object>";
                                    literal3.Text = "<div class=\"FloatLeft\"><object class=\"toHidde\" width=\"412\" height=\"250\"><param name=\"movie\" value=\"http://www.youtube.com/v/" + youtokens[i] +
                                        "\"></param><param  name=\"wmode2\" value=\"transparent\" ></param><param  name=\"wmode\" value=\"opaque\" ></param><param name=\"allowFullScreen\" value=\"true\"></param><embed src=\"http://www.youtube.com/v/" +
                                        youtokens[i] + "\" wmode=\"opaque\" wmode2=\"transparent\' type=\"application/x-shockwave-flash\" allowfullscreen=\"true\" width=\"412\" height=\"250\"></embed></object></div>";
                                    Telerik.Web.UI.RadRotatorItem r3 = new Telerik.Web.UI.RadRotatorItem();
                                    r3.Controls.Add(literal3);
                                    Rotator1.Items.Add(r3);
                                }
                            }
                        }
                        DataView dsSlider = dat.GetDataDV("SELECT * FROM Ad_Slider_Mapping WHERE AdID=" + ID);
                        DataSet dsUser = dat.GetData("SELECT * FROM Ads A, Users U WHERE A.Ad_ID=" + ID + " AND A.User_ID=U.User_ID");
                        string userName = dsUser.Tables[0].Rows[0]["UserName"].ToString();
                        if (dsSlider.Count > 0)
                        {
                            char[] delim = { '\\' };
                            char[] delim3 = { '.' };
                            string[] fileArray = System.IO.Directory.GetFiles(MapPath(".") + "\\UserFiles\\" +
                                userName + "\\AdSlider\\" + ID);

                            string[] finalFileArray = new string[fileArray.Length];

                            for (int i = 0; i < dsSlider.Count; i++)
                            {
                                int length = fileArray[i].Split(delim).Length;
                                finalFileArray[i] = "http://" + Request.Url.Authority + "/HippoHappenings/UserFiles/" + userName +
                                    "/AdSlider/" + ID + "/" + dsSlider[i]["PictureName"].ToString();
                                string[] tokens = dsSlider[i]["PictureName"].ToString().Split(delim3);


                                if (tokens.Length >= 2)
                                {
                                    if (tokens[1].ToUpper() == "JPG" || tokens[1].ToUpper() == "JPEG" || tokens[1].ToUpper() == "GIF" || tokens[1].ToUpper() == "PNG")
                                    {
                                        System.Drawing.Image image = System.Drawing.Image.FromFile(MapPath(".") + "\\UserFiles\\" +
                                userName + "\\AdSlider\\" + ID + "\\" + dsSlider[i]["PictureName"].ToString());


                                        int width = 410;
                                        int height = 250;

                                        int newHeight = 0;
                                        int newIntWidth = 0;

                                        //if image height is less than resize height
                                        if (height >= image.Height)
                                        {
                                            //leave the height as is
                                            newHeight = image.Height;

                                            if (width >= image.Width)
                                            {
                                                newIntWidth = image.Width;
                                            }
                                            else
                                            {
                                                newIntWidth = width;

                                                double theDivider = double.Parse(image.Width.ToString()) / double.Parse(newIntWidth.ToString());
                                                double newDoubleHeight = double.Parse(newHeight.ToString());
                                                newDoubleHeight = double.Parse(height.ToString()) / theDivider;
                                                newHeight = (int)newDoubleHeight;
                                            }
                                        }
                                        //if image height is greater than resize height...resize it
                                        else
                                        {
                                            //make height equal to the requested height.
                                            newHeight = height;

                                            //get the ratio of the new height/original height and apply that to the width
                                            double theDivider = double.Parse(image.Height.ToString()) / double.Parse(newHeight.ToString());
                                            double newDoubleWidth = double.Parse(newIntWidth.ToString());
                                            newDoubleWidth = double.Parse(image.Width.ToString()) / theDivider;
                                            newIntWidth = (int)newDoubleWidth;

                                            //if the resized width is still to big
                                            if (newIntWidth > width)
                                            {
                                                //make it equal to the requested width
                                                newIntWidth = width;

                                                //get the ratio of old/new width and apply it to the already resized height
                                                theDivider = double.Parse(image.Width.ToString()) / double.Parse(newIntWidth.ToString());
                                                double newDoubleHeight = double.Parse(newHeight.ToString());
                                                newDoubleHeight = double.Parse(image.Height.ToString()) / theDivider;
                                                newHeight = (int)newDoubleHeight;
                                            }
                                        }

                                        Literal literal4 = new Literal();
                                        string[] nameTokens = dsSlider[i]["RealPictureName"].ToString().Split(delim3);
                                        string realName = dat.MakeNiceName(nameTokens[0]).Replace("-", " ");
                                        literal4.Text = "<div class=\"RotatorImage\"><img alt=\"" + realName +
                                            "\" style=\"cursor: pointer; margin-left: " + ((412 - newIntWidth) / 2).ToString() + "px; margin-top: " + ((250 - newHeight) / 2).ToString() + "px;\" onclick=\"OpenEventModal(" + i.ToString() + ", " + ID + ");\" height=\"" + newHeight + "px\" width=\"" + newIntWidth + "px\" src=\""
                                            + "UserFiles/" + userName + "/AdSlider/" + ID + "/" +
                                            dsSlider[i]["PictureName"].ToString() + "\" /></div>";
                                        Telerik.Web.UI.RadRotatorItem r4 = new Telerik.Web.UI.RadRotatorItem();
                                        r4.Controls.Add(literal4);

                                        Rotator1.Items.Add(r4);
                                    }
                                    else if (tokens[1].ToUpper() == "WMV")
                                    {
                                        Literal literal4 = new Literal();
                                        literal4.Text = "<div style=\"width: 410px; height: 250px;\" ><OBJECT stop=\"true\" loop=\"false\" controller=\"true\" wmode2=\"opaque\" wmode=\"transparent\" autoplay=\"false\" classid=\"clsid:02BF25D5-8C17-4B23-BC80-D3488ABDDC6B\" " +
                                        "width=\"410\" height=\"250\" codebase=\"http://www.apple.com/qtactivex/qtplugin.cab\">" +
                                        "<param name=\"src\" value=\"UserFiles/" +
                                userName + "/AdSlider/" + ID + "/" + fileArray[i].Split(delim)[length - 1].ToString() + "\"></param>" +
                                        "<param name=\"autoplay\" value=\"false\"></param><param name=\"wmode\" value=\"transparent\"></param>" +
                                        "<param name=\"controller\" value=\"true\"></param>" +
                                        "<param name=\"stop\" value=\"true\" ></param>" +
                                        "<param name=\"loop\" value=\"false\"><param  name=\"wmode2\" value=\"opaque\" ></param>" +
                                        "<EMBED stop=\"true\" wmode=\"transparent\" wmode2=\"opaque\" src=\"UserFiles/" +
                                userName + "/AdSlider/" + ID + "/" + fileArray[i].Split(delim)[length - 1].ToString() + "\" width=\"410\" height=\"250\" autoplay=\"false\" " +
                                        "controller=\"true\" loop=\"false\" bgcolor=\"#000000\" pluginspage=\"http://www.apple.com/quicktime/download/\">" +
                                        "</EMBED>" +
                                        "</OBJECT></div>";
                                        Telerik.Web.UI.RadRotatorItem r4 = new Telerik.Web.UI.RadRotatorItem();
                                        r4.Controls.Add(literal4);
                                        Rotator1.Items.Add(r4);
                                    }

                                }
                            }
                        }
                        break;
                    default: break;

                }



                if (fillUserData)
                {
                    ASP.controls_contactad_ascx SendMessage1 = new ASP.controls_contactad_ascx();
                    SendMessage1.THE_TEXT = "Reply to Bulletin";
                    SendMessage1.RE_LABEL = "Re: " + dat.BreakUpString(ds.Tables[0].Rows[0]["Header"].ToString(), 14);
                    SendMessage1.TYPE = "Connect";
                    SendMessage1.ID = ID;

                    ContactPanel.Controls.Add(SendMessage1);

                    ASP.controls_sendmessage_ascx SendMessage33 = new ASP.controls_sendmessage_ascx();
                    SendMessage33.THE_TEXT = "Share this with a friend";
                    SendMessage33.RE_LABEL = "Re: " + Session["UserName"].ToString() +
                            " would like inquire about your ad \"" + dat.BreakUpString(ds.Tables[0].Rows[0]["Header"].ToString(), 14) + "\".";
                    SendMessage33.TYPE = "a";
                    SendMessage33.ID = ID;
                    ContactPanel.Controls.Add(SendMessage33);

                    Session["Subject"] = "Re: " + Session["UserName"].ToString() +
                            " would like inquire about your ad \"" + dat.BreakUpString(ds.Tables[0].Rows[0]["Header"].ToString(), 14) + "\".";
                }
                else
                {
                    Literal literal = new Literal();
                    literal.Text = "<label class=\"AddGreenLink\">You must be <a class=\"AddLink\" href=\"login\">logged in</a> to contact this person.</label>";

                    ContactPanel.Controls.Add(literal);
                }
                //}
                //else
                //{
                //    Response.Redirect("~/home");
                //}
            }
            else
            {
                Response.Redirect("~/home");
            }
        }
        else
        {
            Response.Redirect("~/home");
        }
        if (Rotator1.Items.Count == 0)
            RotatorPanel.Visible = false;
        else
        {
            RotatorPanel.Visible = true;
            if (Rotator1.Items.Count == 1)
            {
                RotatorPanel.CssClass = "HiddeButtons";
            }
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
    }

    protected void GoToAd(object sender, EventArgs args)
    {
        HttpCookie cookie2 = Request.Cookies["BrowserDate"];
        string AdID = Request.QueryString["AdID"].ToString();
        HttpCookie cookie = new HttpCookie("editAd" + AdID);
        cookie.Value = "True";
        cookie.Expires = DateTime.Parse(cookie2.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(1);
        Response.Cookies.Add(cookie);

        Response.Redirect("post-bulletin?edit=true&ID=" + AdID);

    }

    protected void GetFeaturedBulletins()
    {
        string eventID = Request.QueryString["AdID"];
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        DateTime isNow = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        DataView dvEvent = dat.GetDataDV("SELECT * FROM Ads WHERE Ad_ID = " + eventID);

        string country = dvEvent[0]["CatCountry"].ToString();
        string state = dvEvent[0]["CatState"].ToString();
        string city = dvEvent[0]["CatCity"].ToString();

        DataView dvEventCategories = dat.GetDataDV("SELECT * FROM Ads A, Ad_Category_Mapping ACM " +
            "WHERE A.Ad_ID=ACM.AdID AND A.Ad_ID=" + eventID);

        string adDate = ";" + isNow.Month.ToString() + "/" + isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";";

        string similarQuery = "SELECT DISTINCT A.DateAdded, A.Header, A.Description AS Content, A.Ad_ID AS AdID FROM Ads A, Ad_Category_Mapping ACM WHERE " +
            " A.Featured = 'True' AND A.Ad_ID=ACM.AdID AND A.CatCountry=" + country +
            " AND A.CatState='" + state + "' AND A.DatesOfAd LIKE '%" + adDate + "%' AND A.CatCity='" +
            city + "' AND A.Ad_ID <> " + eventID;

        if (dvEventCategories.Count != 0)
        {
            similarQuery += " AND (";
            bool gotfirst = false;
            foreach (DataRowView row in dvEventCategories)
            {
                if (gotfirst)
                {
                    similarQuery += " OR ";
                }
                similarQuery += " ACM.CategoryID=" + row["CategoryID"].ToString();
                gotfirst = true;
            }
            similarQuery += " ) ";
        }
        DataView dvSimilar = dat.GetDataDV(similarQuery + " ORDER BY DateAdded ASC");

        Literal lit = new Literal();

        if (dvSimilar.Count > 0)
        {
            lit = new Literal();
            lit.Text = "<a id=\"Similar-Bulletins\" class=\"aboutLink\"><h1 class=\"SideColumn\">Similar Featured Bulletins</h1></a><div class=\"Text12 SimilarSide\">";

            OtherAdsPanel.Controls.Add(lit);

            DrawBulletins(dvSimilar, 6);

            lit = new Literal();
            lit.Text = "</div>";

            OtherAdsPanel.Controls.Add(lit);
        }
    }

    protected void DrawBulletins(DataView dvEvents, int cutOff)
    {
        string eventID = Request.QueryString["AdID"];
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        DateTime isNow = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        Literal lit = new Literal();

        int count = 0;
        DataView dvEvent;
        string contentSub = "";
        foreach (DataRowView row in dvEvents)
        {
            if (count > cutOff - 1)
            {
                break;
            }
            else
            {
                contentSub = dat.stripHTML(row["Content"].ToString().Replace("<br>",
                    "").Replace("</br>", "").Replace("<br/>", "").Replace("<BR>", "").Replace("<BR/>",
                    "").Replace("<br />", "").Replace("<BR />", ""));
                if (dat.stripHTML(row["Content"].ToString().Replace("<br>",
                    "").Replace("</br>", "").Replace("<br/>", "").Replace("<BR>", "").Replace("<BR/>",
                    "").Replace("<br />", "").Replace("<BR />", "")).Length > 100)
                    contentSub = dat.stripHTML(row["Content"].ToString().Replace("<br>",
                        "").Replace("</br>", "").Replace("<br/>", "").Replace("<BR>", "").Replace("<BR/>",
                        "").Replace("<br />", "").Replace("<BR />", "")).Substring(0, 100);

                lit.Text += "<div class=\"SimilarSide\">";
                lit.Text += "<a class=\"Green12LinkNF\" href=\"" + dat.MakeNiceName(row["Header"].ToString()) +
                    "_" + row["AdID"].ToString() + "_Ad\">" + row["Header"].ToString() +
                    "</a>. " + dat.BreakUpString(contentSub, 30) +
                    "... <a class=\"Blue12Link\" href=\"" + dat.MakeNiceName(row["Header"].ToString()) +
                    "_" + row["AdID"].ToString() + "_Ad\">Read More</a>";
                lit.Text += "</div>";
                count++;
            }
        }

        OtherAdsPanel.Controls.Add(lit);
    }
}
