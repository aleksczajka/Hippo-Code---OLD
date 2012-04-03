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
using System.Net;
using System.Collections.Generic;
using System.IO;

public partial class Event : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string eventID = GetEventID();
        string message = "";
        bool isCl = false;
        if (Request.QueryString["cl"] != null)
            if (bool.Parse(Request.QueryString["cl"]))
            {
                isCl = true;
                eventID = eventID.Replace("_", "/");
            }

        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        try
        {
            Session["RedirectTo"] = Request.Url.AbsoluteUri;

            bool fillUserData = false;

            try
            {
                if (Session["User"] != null)
                {
                    LoggedInPanel.Visible = true;
                    LoggedOutPanel.Visible = false;
                    

                    DataSet ds1 = dat.GetData("SELECT UserName FROM Users WHERE User_ID=" + Session["User"].ToString());
                    fillUserData = true;

                    if (!isCl)
                        EditLink.Visible = true;

                    ////Query whether current owner was delinquent on approve/reject changes
                    //if (dat.IsOwnerDelinquent(eventID, Request.IsLocal, "E"))
                    //{
                    //    //make the button visable
                    //    OwnerPanel.Visible = true;
                    //    Session["Message"] = "The ownership of this event is <b>open</b>. <br/>The ownership became " +
                    //        "open because the previous owner of this event became un-responsive to rejecting/" +
                    //        "approving user's changes to this event.<br/>If you would like to become " +
                    //        "the owner, click on the button below to go to the event's edit page. <br/><br/> " +
                    //        "Being the owner, you will have the privilage of having your <b>edits come though right " +
                    //        "away.</b> Other participants' changes to this event will have to be <b>approved by you.</b><br/><br/>" +
                    //        "<div align=\"center\" style=\"padding-left: 110px;\"><div align=\"center\"><div style=\"cursor: pointer;cursor: pointer; float: left;padding-right: 10px;\">" +
                    //        "<div class=\"topDiv\" style=\"float:left;\">"+
                    //        "<img style=\"float: left;\" src=\"http://hippohappenings.com/NewImages/ButtonLeft.png\" height=\"27px\" />"+
                    //        "<div style=\"font-size: 12px; text-decoration: none; padding-top: 5px;padding-left: 6px; padding-right: 6px;height: 27px;float: left;background: url('http://hippohappenings.com/NewImages/ButtonPixel.png'); background-repeat: repeat-x;\">"+
                    //        "<a class=\"NavyLink\" onclick=\"Search('blog-event?edit=true&ID=" + eventID +
                    //        "');\">Edit</a></div>" +
                    //        "<img style=\"float: left;\" src=\"http://hippohappenings.com/NewImages/ButtonRight.png\" height=\"27px\" />"+
                    //        "</div>"+
                    //        "</div>"+
                    //        "</div>"+
                    //        "<div align=\"center\"><div style=\"cursor: pointer;\">" +
                    //        "<div class=\"topDiv\" style=\"float:left;\">" +
                    //        "<img style=\"float: left;\" src=\"http://hippohappenings.com/NewImages/ButtonLeft.png\" height=\"27px\" />" +
                    //        "<div style=\"font-size: 12px; text-decoration: none; padding-top: 5px;padding-left: 6px; padding-right: 6px;height: 27px;float: left;background: url('http://hippohappenings.com/NewImages/ButtonPixel.png'); background-repeat: repeat-x;\">" +
                    //        "<a class=\"NavyLink\" onclick=\"Search();\">Close</a></div>" +
                    //        "<img style=\"float: left;\" src=\"http://hippohappenings.com/NewImages/ButtonRight.png\" height=\"27px\" />" +
                    //        "</div>" +
                    //        "</div>" +
                    //        "</div></div>";
                    //}
                }
                else
                {
                    LoggedOutPanel.Visible = true;
                    LoggedInPanel.Visible = false;
                    //Button calendarLink = (Button)dat.FindControlRecursive(this, "CalendarLink");
                    //calendarLink.Visible = false;
                    //EditLink.Visible = false;
                }

                if (!isCl)
                    GetOtherEvents();
            }
            catch (Exception ex)
            {
                ErrorLabel.Text += ex.ToString();
            }

            if (Request.QueryString["EventID"] == null)
                Response.Redirect("~/home");
            string ID = eventID;
            Session["EventID"] = ID;

            if (isCl)
            {
                HippoRating1.Visible = false;
                OwnerPanel.Visible = false;
                EditLink.Visible = false;
                PassedLink.Visible = false;
                PricePanel.Visible = false;
                RatingPanel.Visible = false;
                Flag1.Visible = false;
                ContactOwnerLink.Visible = false;
                ClGoingPanel.Visible = true;

                DateAndTimeLabel.Text = "";

                string URL = "http://newyork.craigslist.org" + eventID + ".html";
                message = URL;
                WebClient myClient = new WebClient();
                string webPageString = myClient.DownloadString(URL).Replace("\r", "").Replace("\n", "");

                HtmlAgilityPack.HtmlDocument a = new HtmlAgilityPack.HtmlDocument();
                a.LoadHtml(webPageString);

                string date = "";
                string link = "";
                string header = "";
                string email = "";

                #region Header
                IEnumerable<HtmlAgilityPack.HtmlNode> pNodes = a.DocumentNode.SelectNodes("//h2");
                foreach (HtmlAgilityPack.HtmlNode node in pNodes)
                {
                    header = node.InnerHtml;
                    break;
                }
                Session["ClHeader"] = header;
                #endregion

                #region Date
                DateTime DateTimeStart = new DateTime();
                DateTime DateTimeEnd = new DateTime();
                bool isEnd = false;
                dat.GetStartAndEndDate(out DateTimeStart, out DateTimeEnd, ref header, out isEnd);
                
                #endregion

                #region Description
                pNodes = a.DocumentNode.SelectNodes("//div[@id='userbody']");

                string description = "";

                foreach (HtmlAgilityPack.HtmlNode node in pNodes)
                {
                    description += node.OuterHtml;
                    break;
                }

                //Remove images
                description = dat.RemoveImages(description).Replace("<!-- START CLTAGS -->",
                    "").Replace("<!-- END CLTAGS -->", "").Replace("<a ", "<a class='NavyLink12UD' ");

                ShowDescriptionBegining.Text = description;
                Session["ClDescription"] = description;
                #endregion

                #region Email
                IEnumerable<HtmlAgilityPack.HtmlNode> nds = a.DocumentNode.ChildNodes[1].ChildNodes[1].Elements("a");
                foreach (HtmlAgilityPack.HtmlNode nd in nds)
                {
                    //if (nd.Name.ToLower() == "a")
                    //{
                    email = "<div style='clear: both;'><b>Email: " + nd.OuterHtml.Replace("<a", "<a target=\"_blank\" class=\"NavyLink12UD\"") + "</b></div>";
                    break;
                    //}
                }
                ClEmailLiteral.Text += "<b>craigslist event: <a class=\"NavyLink12UD\" target=\"_blank\" href='" + URL + "'>" + dat.BreakUpString(header, 14) + "</a></b><br/>" + email;
                #endregion

                #region Do Calendar Link
                string monthStart = DateTimeStart.Month.ToString();
                if (monthStart.Length == 1)
                    monthStart = "0" + monthStart;
                string dayStart = DateTimeStart.Day.ToString();
                if (dayStart.Length == 1)
                    dayStart = "0" + dayStart;

                string monthEnd = "";
                string dayEnd = "";
                string yearEnd = "";
                if (isEnd)
                {
                    monthEnd = DateTimeEnd.Month.ToString();
                    if (monthEnd.Length == 1)
                        monthEnd = "0" + monthEnd;
                    dayEnd = DateTimeEnd.Day.ToString();
                    if (dayEnd.Length == 1)
                        dayEnd = "0" + dayEnd;

                    yearEnd = DateTimeEnd.Year.ToString();
                }
                else
                {
                    monthEnd = monthStart;
                    dayEnd = dayStart;
                    yearEnd = DateTimeStart.Year.ToString();
                }

                string shortDesc = dat.stripHTML(description).Replace(" ", "+");

                if (shortDesc.Length > 1400)
                    shortDesc = shortDesc.Substring(0, 1400) + "...";

                string googleCalendarLink = "https://www.google.com/calendar/render?action=TEMPLATE&" +
                    "dates=" + DateTimeStart.Year.ToString() + monthStart + dayStart +
                        "T120000Z/" + yearEnd + monthEnd + dayEnd + "T130000Z" +
                    "&sprop=" +
                    "website:http://hippohappenings.com/" + dat.MakeNiceName(header) + "_CLHH" + GetEventID() + "_ClEvent" +
                    "&text=" +
                    header.Replace("/", "%2f").Replace(" ", "+") +
                    "&location=" +
                    "Location" +
                    "&sprop=name:" +
                    header.Replace("/", "%2f").Replace(" ", "+") +
                    "&details=" +
                    "craigslist+ad+http://hippohappenings.com/" + dat.MakeNiceName(header) + 
                    "_CLHH" + GetEventID() + "_ClEvent+" + shortDesc +
                    "&gsessionid=OK&sf=true&output=xml";
                CalendarLiteral.Text = "<a target=\"_blank\" class=\"NavyLink12UD\" href=\"" + googleCalendarLink +
                    "\" title=\"Add this Meetup to your Google calendar\">" +
                    "<span class=\"calOpt google\"></span>Add To Google Calendar</a>";
                //DateAndTimeLabel.Text = googleCalendarLink;\
                #endregion

                #region Images
                pNodes = a.DocumentNode.SelectNodes("//img");

                Rotator1.Items.Clear();

                foreach (HtmlAgilityPack.HtmlNode node in pNodes)
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(node.Attributes["src"].Value);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream receiveStream = response.GetResponseStream();
                    // read the stream
                    System.Drawing.Image image = System.Drawing.Image.FromStream(receiveStream);
                    receiveStream.Close();
                    response.Close();

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

                    string alt = "";
                    if (node.Attributes.Contains("alt"))
                    {
                        alt = node.Attributes["alt"].Value;
                    }
                    else
                    {
                        alt = dat.MakeNiceNameFull(header);
                    }

                    Literal literal4 = new Literal();
                    literal4.Text = "<div class=\"RotatorImage\"><img alt=\"" + alt +
                "\" style=\" margin-left: " + ((410 - newIntWidth) / 2).ToString() + "px; margin-top: " + ((250 - newHeight) / 2).ToString() + "px;\" height=\"" + newHeight + "px\" width=\"" + newIntWidth + "px\" src=\""
                        + node.Attributes["src"].Value + "\" /></div>";
                    Telerik.Web.UI.RadRotatorItem r4 = new Telerik.Web.UI.RadRotatorItem();
                    r4.Controls.Add(literal4);

                    Rotator1.Items.Add(r4);
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
                #endregion

                #region Going
                DataView dvGoing = dat.GetDataDV("SELECT * FROM ClEventGoing WHERE ClEventID = '" + GetEventID() + "'");
                if (dvGoing.Count > 0)
                    NumberPeopleLabel.Text = dvGoing.Count.ToString();
                else
                    GoingPanel.Visible = false;

                if (Session["User"] != null)
                {
                    DataView dvGoingUser = dat.GetDataDV("SELECT * FROM ClEventGoing WHERE UserID = " + Session["User"].ToString() + " AND ClEventID = '" + GetEventID() + "'");
                    if (dvGoingUser.Count > 0)
                    {
                        ImGoingPanel.Visible = true;
                        AreUGoingPanel.Visible = false;
                    }
                    else
                    {
                        AreUGoingPanel.Visible = true;
                        ImGoingPanel.Visible = false;
                    }
                }
                else
                {
                    AreUGoingPanel.Visible = false;
                    ImGoingPanel.Visible = false;
                }

                #endregion


                EventName.Text = "<a id=\"" + dat.MakeNiceNameFull(header) + "\" class=\"aboutLink\" href=\"http://" + Request.Url.Authority + "/" +
                                dat.MakeNiceName(header) +
                                "_CLHH" + GetEventID() + "_ClEvent\"><h1>" +
                                dat.BreakUpString(header, 50) + "</h1></a>";

                Session["Subject"] = "Re: " + header;
                Session["CommentSubject"] = "Re: " + header;


                #region Communicate
                if (Session["User"] != null)
                {
                    DataSet dsFriends = dat.GetData("SELECT DISTINCT UC.CLEventID, UC.UserID FROM User_Friends UF, ClEventGoing UC " +
                        "WHERE UC.CLEventID='" + GetEventID() + "' AND UC.UserID=UF.FriendID AND UF.UserID=" + Session["User"].ToString());

                    ASP.controls_smallbutton_ascx blueButton = new ASP.controls_smallbutton_ascx();
                    blueButton.CLIENT_LINK_CLICK = "OpenRad('" + eventID + "&cl=true');";
                    blueButton.BUTTON_TEXT = "Communicate with them";

                    int count = 0;
                    if (dsFriends.Tables.Count > 0)
                        if (dsFriends.Tables[0].Rows.Count > 0)
                        {
                            count = dsFriends.Tables[0].Rows.Count;
                        }

                    dvGoing = dat.GetDataDV("SELECT * FROM ClEventGoing WHERE ClEventID = '" + GetEventID() + "' AND UserID <> " + Session["User"].ToString());

                    if (dvGoing.Count > 0)
                        CommunicatePanel.Controls.Add(blueButton);


                    if (count > 0)
                    {
                        NumberFriendsLabel.Text = "(" + count.ToString() + " Friends)";
                    }
                }
                else
                {
                    Literal CommunicateLiteral = new Literal();
                    CommunicateLiteral.Text = "<div class=\"CommunicateWith\">To " +
                        "communicate with them, <a class=\"NavyLink aboutLink\" " +
                        "href=\"login\">log in</a>.</div>";
                    CommunicatePanel.Controls.Add(CommunicateLiteral);
                }
                #endregion

                #region Comments
                if (Session["User"] != null)
                {
                    DataSet dsComments;
                    string commentPrefs = dat.GetData("SELECT * FROM UserPreferences WHERE UserID=" + Session["User"].ToString()).Tables[0].Rows[0]["CommentsPreferences"].ToString();
                    if (commentPrefs == "1")
                    {
                        dsComments = dat.GetData("SELECT C.BlogDate AS theDate, * FROM ClEventComments C, Users U WHERE U.User_ID=C.UserID AND C.ClEventID='" + GetEventID() + "' ORDER BY C.BlogDate");
                    }
                    else
                    {
                        dsComments = dat.GetData("SELECT DISTINCT U.ProfilePicture, U.User_ID, U.UserName, C.Comment, C.BlogDate AS theDate FROM ClEventComments C, Users U, User_Friends UF WHERE ((UF.UserID=" + Session["User"].ToString() + " AND UF.FriendID=U.User_ID AND U.User_ID=C.UserID) OR (U.User_ID=" +
                            Session["User"].ToString() + " AND U.User_ID=C.UserID)) AND C.ClEventID='" + GetEventID() + "' ORDER BY C.BlogDate");
                    }
                    TheComments.DATA_SET = dsComments;
                    TheComments.DataBind2(true);
                }
                else
                {
                    DataSet dsComments = dat.GetData("SELECT C.BlogDate AS theDate, * FROM ClEventComments C, Users U WHERE U.User_ID=C.UserID AND C.ClEventID='" + GetEventID() + "' ORDER BY C.BlogDate");
                    TheComments.DATA_SET = dsComments;
                    TheComments.DataBind2(true);
                }
                #endregion


                #region SEO
                //Create keyword and description meta tags and title
                topTopLiteral.Text = "<a class=\"NavyLink12UD\" href=\"#" + dat.MakeNiceNameFull(header) + "\">" +
                    dat.MakeNiceNameFull(header).Replace("-", " ") + " From The Top</a>";

                HtmlMeta hm = new HtmlMeta();
                HtmlMeta kw = new HtmlMeta();
                HtmlMeta lg = new HtmlMeta();
                HtmlHead head = (HtmlHead)Page.Header;
                HtmlLink cn = new HtmlLink();

                cn.Attributes.Add("rel", "canonical");
                cn.Href = "http://" + Request.Url.Authority + "/" +
                                dat.MakeNiceName(header) +
                                "_CLHH" + GetEventID() + "_ClEvent";
                head.Controls.AddAt(0, cn);

                hm.Name = "Description";
                kw.Name = "keywords";
                lg.Name = "language";
                lg.Content = "English";
                head.Controls.AddAt(0, lg);

                char[] delimeter = { ' ' };
                string[] keywords = dat.MakeNiceNameFull(header).Replace("-",
                    " ").Split(delimeter, StringSplitOptions.RemoveEmptyEntries);
                int count2 = 0;
                foreach (string token in keywords)
                {
                    if (count2 < 16)
                    {
                        if (kw.Content != "")
                            kw.Content += " ";
                        kw.Content += token;

                        count2++;
                    }
                }
                head.Controls.AddAt(0, kw);

                hm.Content = dat.MakeNiceNameFull(dat.stripHTML(description).Replace("   ", " ").Replace("  ", " ")).Replace("-", " ");
                if (hm.Content.Length > 200)
                    hm.Content = hm.Content.Substring(0, 197) + "...";

                head.Controls.AddAt(0, hm);

                this.Title = kw.Content;

                HtmlLink lk = new HtmlLink();
                lk.Href = "http://" + Request.Url.Authority + "/" +
                                dat.MakeNiceName(header) +
                                "_CLHH" + GetEventID() + "_ClEvent";
                lk.Attributes.Add("rel", "bookmark");
                head.Controls.AddAt(0, lk);
                #endregion

                Session["messageText"] = dat.BreakUpString(header, 14);
                Session["messageEmail"] = "EventName: <a href=\"http://" + Request.Url.Authority + "/" +
                                dat.MakeNiceName(header) +
                                "_CLHH" + GetEventID() + "_ClEvent\">" +
                    dat.BreakUpString(header, 14) + "</a> <br/><br/>" + ShowDescriptionBegining.Text;



                if (fillUserData)
                {
                    ASP.controls_sendmessage_ascx SendMessage1 = new ASP.controls_sendmessage_ascx();
                    SendMessage1.THE_TEXT = "Share on Hippo";
                    SendMessage1.RE_LABEL = "Re: " + Session["UserName"].ToString() +
                        " would like to share \"" + dat.BreakUpString(header, 14) + "\" with you.";
                    SendMessage1.TYPE = "cl";
                    //SendMessage1.ID = int.Parse(ID);
                    SendMessage1.CL_ID = eventID;
                    CalendarSharePanel.Controls.Add(SendMessage1);

                    Session["Subject"] = "Re: " + Session["UserName"].ToString() +
                        " would like to share \"" + dat.BreakUpString(header, 14) + "\" with you.";

                }
            }
            else
            {
                if (Session["User"] != null)
                    dat.GetRecommendationIcons(eventID, ref RecomPanel);

                RecomPanel.Width = 10 * (RecomPanel.Controls.Count);

                DataSet ds = dat.GetData("SELECT * FROM Events WHERE ID=" + ID);

                DataView dv = new DataView(ds.Tables[0], "", "", DataViewRowState.CurrentRows);
                if (Session["User"] != null)
                {
                    if (dv[0]["Owner"].ToString() == Session["User"].ToString())
                    {
                        EditLink.Visible = true;
                        ContactOwnerLink.Visible = false;
                    }
                    else
                    {
                        if (dv[0]["Owner"] != null && dv[0]["Owner"].ToString() != "")
                        {
                            DataView dvU = dat.GetDataDV("SELECT * FROM Users WHERE User_ID=" + dv[0]["Owner"].ToString());
                            EditLink.Visible = false;
                            ContactOwnerLink.Visible = true;
                            ContactOwnerLink.HRef = dvU[0]["UserName"].ToString() + "_friend";
                        }
                        else
                        {
                            ContactOwnerLink.Visible = false;
                            EditLink.Visible = true;
                        }
                    }
                }
                else
                {
                    EditLink.Visible = false;
                    ContactOwnerLink.Visible = false;
                }

                //Overwrite everything if the event has passed
                if (dat.HasEventPassed(eventID))
                {
                    OwnerPanel.Visible = false;
                    EditLink.Visible = false;
                    PassedLink.Visible = true;
                }

                Session["FlagID"] = ID;
                Session["FlagType"] = "E";

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (bool.Parse(ds.Tables[0].Rows[0]["Live"].ToString()))
                        {
                            PricePanel.Visible = false;
                            if (ds.Tables[0].Rows[0]["MaxPrice"] != null)
                            {
                                if (ds.Tables[0].Rows[0]["MaxPrice"].ToString() != "")
                                {
                                    MinPrice.Text = ds.Tables[0].Rows[0]["MinPrice"].ToString().Replace(".00", "");
                                    MaxPrice.Text = ds.Tables[0].Rows[0]["MaxPrice"].ToString().Replace(".00", "");
                                    PricePanel.Visible = true;
                                }
                            }

                            EventName.Text = "<a id=\"" + dat.MakeNiceNameFull(ds.Tables[0].Rows[0]["Header"].ToString()) + "\" class=\"aboutLink\" href=\"http://" + Request.Url.Authority + "/" +
                                dat.MakeNiceName(ds.Tables[0].Rows[0]["Header"].ToString()) +
                                "_" + ID.ToString() + "_Event\"><h1>" +
                                dat.BreakUpString(ds.Tables[0].Rows[0]["Header"].ToString(), 50) + "</h1></a>";
                            Session["Subject"] = "Re: " + ds.Tables[0].Rows[0]["Header"].ToString();
                            Session["CommentSubject"] = "Re: " + ds.Tables[0].Rows[0]["Header"].ToString();
                            string UserName = ds.Tables[0].Rows[0]["UserName"].ToString();
                            DataSet dsDate = dat.GetData("SELECT * FROM Event_Occurance WHERE EventID=" + ID);
                            
                            TagCloud.THE_ID = int.Parse(ID);
                            NumberPeopleLabel.Text = dat.GetData("SELECT DISTINCT UserID FROM User_Calendar " +
                                "WHERE EventID=" + ID).Tables[0].Rows.Count.ToString();

                            if (Session["User"] != null)
                            {
                                DataSet dsFriends = dat.GetData("SELECT DISTINCT UC.EventID, UC.UserID FROM User_Friends UF, User_Calendar UC " +
                                    "WHERE UC.EventID=" +
                                    ID + " AND UC.UserID=UF.FriendID AND UF.UserID=" + Session["User"].ToString());


                                ASP.controls_smallbutton_ascx blueButton = new ASP.controls_smallbutton_ascx();
                                blueButton.CLIENT_LINK_CLICK = "OpenRad('" + ID + "');";
                                blueButton.BUTTON_TEXT = "Communicate with them";

                                CommunicatePanel.Controls.Add(blueButton);

                                int count = 0;
                                if (dsFriends.Tables.Count > 0)
                                    if (dsFriends.Tables[0].Rows.Count > 0)
                                    {
                                        count = dsFriends.Tables[0].Rows.Count;
                                    }

                                NumberFriendsLabel.Text = "(" + count.ToString() + " Friends)";
                            }
                            else
                            {
                                Literal CommunicateLiteral = new Literal();
                                CommunicateLiteral.Text = "<div class=\"CommunicateWith\">To " +
                                    "communicate with them, <a class=\"NavyLink aboutLink\" " +
                                    "href=\"login\">log in</a>.</div>";
                                CommunicatePanel.Controls.Add(CommunicateLiteral);
                            }

                            if (bool.Parse(ds.Tables[0].Rows[0]["hasSongs"].ToString()))
                            {
                                DataSet dsSongs = dat.GetData("SELECT * FROM Event_Song_Mapping WHERE EventID=" + ID);
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
                                    songs.SONG1_TITLE = dsSongs.Tables[0].Rows[0]["SongTitle"].ToString();
                                    songs.SONG2_TITLE = dsSongs.Tables[0].Rows[1]["SongTitle"].ToString();
                                }
                                else if (songCount == 1)
                                {
                                    songs.SONG1 = dsSongs.Tables[0].Rows[0]["SongName"].ToString();
                                    songs.SONG1_TITLE = dsSongs.Tables[0].Rows[0]["SongTitle"].ToString();
                                }

                                songs.USER_NAME = ds.Tables[0].Rows[0]["UserName"].ToString();

                                SongPanel.Controls.Add(songs);
                            }


                            

                            DateTime date;
                            DateTime endDate;
                            DateAndTimeLabel.Text = "";
                            string dateandtime = "";
                            bool dateGotten = false;
                            string moreDates = "";
                            DateTime DateTimeStart = DateTime.Parse(dsDate.Tables[0].Rows[0]["DateTimeStart"].ToString());
                            DateTime DateTimeEnd = DateTime.Parse(dsDate.Tables[0].Rows[0]["DateTimeEnd"].ToString());
                            bool isEnd = true;
                            for (int i = 0; i < dsDate.Tables[0].Rows.Count; i++)
                            {
                                date = DateTime.Parse(dsDate.Tables[0].Rows[i]["DateTimeStart"].ToString());
                                endDate = DateTime.Parse(dsDate.Tables[0].Rows[i]["DateTimeEnd"].ToString());

                                if ((endDate >= DateTime.Now && !dateGotten) || (i == dsDate.Tables[0].Rows.Count - 1 && !dateGotten))
                                {
                                    dateGotten = true;
                                    dateandtime += date.DayOfWeek.ToString() + ", " + GetMonth(date.Month.ToString()) + " " +
                                        date.Day + " " + date.ToShortTimeString() + " To " +
                                        endDate.DayOfWeek.ToString() + ", " + GetMonth(endDate.Month.ToString()) + " " +
                                        endDate.Day + " " + endDate.ToShortTimeString();
                                    DateAndTimeLabel.Text += GetMonth(date.Month.ToString()).Substring(0, 3) + ". " +
                                        date.Day + " at " + date.ToShortTimeString().Replace(":00", "").Replace("PM", "p").Replace("AM", "a");
                                }
                                else
                                {


                                    if (moreDates == "")
                                        moreDates = "<div onclick=\"OpenMoreDates();\" class=\"NavyLink\" id=\"MoreDatesName\">More Dates</div>" +
                                            "<div class=\"MoreNo\" id=\"infoDiv\">" +
                                            (dsDate.Tables[0].Rows.Count * 40).ToString() +
                                            "</div><div class=\"MoreNo\" " +
                                            "id=\"MoreDatesDiv\"> <div class=\"NavyLink InnerMore\" " +
                                                   "onclick=\"OpenMoreDates();\">close</div><br/><br/>";
                                    moreDates += date.DayOfWeek.ToString() + ", " + GetMonth(date.Month.ToString()) + " " +
                                        date.Day + " " + date.ToShortTimeString()
                                        + " <span class=\"ToSpan\">To</span> " +
                                        endDate.DayOfWeek.ToString() + ", " + GetMonth(endDate.Month.ToString()) + " " +
                                        endDate.Day + " " + endDate.ToShortTimeString() + "<br/>";
                                }



                            }
                            if (moreDates != "")
                                moreDates += "</div>";

                            DateAndTimeLabel.Text += moreDates;

                            string content = ds.Tables[0].Rows[0]["Content"].ToString();
                            string niceName = dat.MakeNiceName(ds.Tables[0].Rows[0]["Header"].ToString());


                            if (bool.Parse(ds.Tables[0].Rows[0]["Private"].ToString()))
                            {
                                VenueName.Text = ds.Tables[0].Rows[0]["Address"].ToString() + "<br/>" +
                                    ds.Tables[0].Rows[0]["City"].ToString() + ", " + ds.Tables[0].Rows[0]["State"].ToString() +
                                    " " + ds.Tables[0].Rows[0]["Zip"].ToString();

                                Session["messageText"] = dat.BreakUpString(ds.Tables[0].Rows[0]["Header"].ToString(), 14) + " occurs at " + ds.Tables[0].Rows[0]["Address"].ToString() + " on " + dateandtime;
                                Session["messageEmail"] = "EventName: <a href=\"http://hippohappenings.com/" + dat.MakeNiceName(dat.BreakUpString(ds.Tables[0].Rows[0]["Header"].ToString(), 14)) + "_" + ID + "_Event\">" +
                                    dat.BreakUpString(ds.Tables[0].Rows[0]["Header"].ToString(), 14) + "</a> <br/><br/> Venue: " + ds.Tables[0].Rows[0]["Address"].ToString() + " <br/><br/> Date: " +
                                    DateAndTimeLabel.Text + " <br/><br/> " + ShowDescriptionBegining.Text;
                            }
                            else
                            {
                                DataSet dsVenue = dat.GetData("SELECT * FROM Venues WHERE ID=" +
                                ds.Tables[0].Rows[0]["Venue"]);
                                VenueName.Text = dsVenue.Tables[0].Rows[0]["Name"].ToString();
                                VenueName.NavigateUrl = dat.MakeNiceName(dsVenue.Tables[0].Rows[0]["Name"].ToString()) +
                                    "_" + dsVenue.Tables[0].Rows[0]["ID"].ToString() + "_Venue";

                                Session["messageText"] = dat.BreakUpString(ds.Tables[0].Rows[0]["Header"].ToString(), 14) + " occurs at " + VenueName.Text + " on " + dateandtime;
                                Session["messageEmail"] = "EventName: <a href=\"http://hippohappenings.com/" + dat.MakeNiceName(dat.BreakUpString(ds.Tables[0].Rows[0]["Header"].ToString(), 14)) + "_" + ID + "_Event\">" +
                                    dat.BreakUpString(ds.Tables[0].Rows[0]["Header"].ToString(), 14) + "</a> <br/><br/> Venue: <a href=\"http://hippohappenings.com/" + dat.MakeNiceName(VenueName.Text) + "_" +
                                    dsVenue.Tables[0].Rows[0]["ID"].ToString() + "_Venue\">" + VenueName.Text + "</a> <br/><br/> Date: " +
                                    DateAndTimeLabel.Text + " <br/><br/> " + ShowDescriptionBegining.Text;
                            }

                            //ScriptLiteral.Text = "<script type=\"text/javascript\">ReturnURL('" + niceName.Replace("_", " ") + " at " + dat.MakeNiceName(dsVenue.Tables[0].Rows[0]["Name"].ToString()).Replace("_", " ") + " Tonight http://HippoHappenings.com/" + Request.QueryString["EventID"].ToString() + "_Event');</script>";

                            if (Session["User"] != null)
                            {
                                DataSet dsComments;
                                string commentPrefs = dat.GetData("SELECT * FROM UserPreferences WHERE UserID=" + Session["User"].ToString()).Tables[0].Rows[0]["CommentsPreferences"].ToString();
                                if (commentPrefs == "1")
                                {
                                    dsComments = dat.GetData("SELECT C.BlogDate AS theDate, * FROM Comments C, Users U WHERE U.User_ID=C.UserID AND C.BlogID=" + ID + " ORDER BY C.BlogDate");
                                }
                                else
                                {
                                    dsComments = dat.GetData("SELECT DISTINCT U.ProfilePicture, U.User_ID, U.UserName, C.Comment, C.BlogDate AS theDate FROM Comments C, Users U, User_Friends UF WHERE ((UF.UserID=" + Session["User"].ToString() + " AND UF.FriendID=U.User_ID AND U.User_ID=C.UserID) OR (U.User_ID=" +
                                        Session["User"].ToString() + " AND U.User_ID=C.UserID)) AND C.BlogID=" + ID + " ORDER BY C.BlogDate");
                                }
                                TheComments.DATA_SET = dsComments;
                                TheComments.DataBind2(true);
                            }
                            else
                            {
                                DataSet dsComments = dat.GetData("SELECT C.BlogDate AS theDate, * FROM Comments C, Users U WHERE U.User_ID=C.UserID AND C.BlogID=" + ID + " ORDER BY C.BlogDate");
                                TheComments.DATA_SET = dsComments;
                                TheComments.DataBind2(true);
                            }

                            ShowDescriptionBegining.Text = dat.BreakUpString(content, 20);

                            #region SEO
                            //Create keyword and description meta tags and title
                            topTopLiteral.Text = "<a class=\"NavyLink12UD\" href=\"#" + dat.MakeNiceNameFull(ds.Tables[0].Rows[0]["Header"].ToString()) + "\">" +
                                dat.MakeNiceNameFull(ds.Tables[0].Rows[0]["Header"].ToString()).Replace("-", " ") + " From The Top</a>";

                            HtmlMeta hm = new HtmlMeta();
                            HtmlMeta kw = new HtmlMeta();
                            HtmlMeta lg = new HtmlMeta();
                            HtmlHead head = (HtmlHead)Page.Header;
                            HtmlLink cn = new HtmlLink();

                            cn.Attributes.Add("rel", "canonical");
                            cn.Href = "http://" + Request.Url.Authority + "/" +
                                dat.MakeNiceName(ds.Tables[0].Rows[0]["Header"].ToString()) +
                                "_" + ID.ToString() + "_Event";
                            head.Controls.AddAt(0, cn);

                            hm.Name = "Description";
                            kw.Name = "keywords";
                            lg.Name = "language";
                            lg.Content = "English";
                            head.Controls.AddAt(0, lg);

                            char[] delimeter = { ' ' };
                            string[] keywords = dat.MakeNiceNameFull(ds.Tables[0].Rows[0]["Header"].ToString()).Replace("-", " ").Split(delimeter, StringSplitOptions.RemoveEmptyEntries);
                            int count2 = 0;
                            foreach (string token in keywords)
                            {
                                if (count2 < 16)
                                {
                                    if (kw.Content != "")
                                        kw.Content += " ";
                                    kw.Content += token;

                                    count2++;
                                }
                            }
                            head.Controls.AddAt(0, kw);

                            hm.Content = dat.MakeNiceNameFull(dat.stripHTML(ds.Tables[0].Rows[0]["Content"].ToString()).Replace("   ", " ").Replace("  ", " ")).Replace("-", " ");
                            if (hm.Content.Length > 200)
                                hm.Content = hm.Content.Substring(0, 197) + "...";

                            head.Controls.AddAt(0, hm);

                            this.Title = kw.Content;

                            HtmlLink lk = new HtmlLink();
                            lk.Href = "http://" + Request.Url.Authority + "/" +
                                dat.MakeNiceName(ds.Tables[0].Rows[0]["Header"].ToString()) +
                                "_" + ID.ToString() + "_Event";
                            lk.Attributes.Add("rel", "bookmark");
                            head.Controls.AddAt(0, lk);
                            #endregion


                            DataView dvCats = dat.GetDataDV("SELECT DISTINCT C.ID, ECM.ID AS EID, C.Name AS CategoryName, ECM.tagSize FROM Event_Category_Mapping ECM, EventCategories C WHERE ECM.CategoryID=C.ID AND ECM.EventID=" + eventID + " ORDER BY ECM.ID");

                            string justCats = "";

                            for (int i = 0; i < dvCats.Count; i++)
                            {
                                //kw.Content += ", " + dvCats[i]["CategoryName"].ToString();
                                justCats += dvCats[i]["CategoryName"].ToString() + " ";
                            }




                            

                            //Media Categories: NONE: 0, Picture: 1, Video: 2, YouTubeVideo: 3, Slider: 4
                            Rotator1.Items.Clear();
                            int mediaCategory = int.Parse(ds.Tables[0].Rows[0]["mediaCategory"].ToString());
                            string youtube = ds.Tables[0].Rows[0]["YouTubeVideo"].ToString();
                            switch (mediaCategory)
                            {
                                case 0:
                                    break;
                                case 1:
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
                                                literal3.Text = "<div class=\"YouTubeWrapper\"><object class=\"toHidde\" width=\"412\" " +
                                                    "height=\"250\"><param name=\"movie\" value=\"http://www.youtube.com/v/" + youtokens[i] +
                                                    "\"/><param  name=\"wmode2\" value=\"transparent\" /><param  name=\"wmode\" " +
                                                    "value=\"opaque\" /><param name=\"allowFullScreen\" value=\"true\"/><embed " +
                                                    "src=\"http://www.youtube.com/v/" +
                                                    youtokens[i] + "\" wmode=\"opaque\" wmode2=\"transparent\" type=\"application/x-shockwave" +
                                                    "-flash\" allowfullscreen=\"true\" width=\"412\" height=\"250\"/></object></div>";
                                                Telerik.Web.UI.RadRotatorItem r3 = new Telerik.Web.UI.RadRotatorItem();
                                                r3.Controls.Add(literal3);
                                                Rotator1.Items.Add(r3);
                                            }
                                        }
                                    }
                                    DataView dsSlider = dat.GetDataDV("SELECT * FROM Event_Slider_Mapping WHERE EventID=" + ID);
                                    if (dsSlider.Count > 0)
                                    {
                                        char[] delim = { '\\' };
                                        char[] delim3 = { '.' };
                                        //string[] fileArray = System.IO.Directory.GetFiles(MapPath(".") + "\\UserFiles\\Events\\" + ID + "\\Slider");

                                        //string[] finalFileArray = new string[fileArray.Length];

                                        for (int i = 0; i < dsSlider.Count; i++)
                                        {
                                            //int length = fileArray[i].Split(delim).Length;
                                            //finalFileArray[i] = "http://" + Request.Url.Authority + "/HippoHappenings/UserFiles/Events/" +
                                            //    ID + "/Slider/" + fileArray[i].Split(delim)[length - 1];
                                            if (bool.Parse(dsSlider[i]["ImgPathAbsolute"].ToString()))
                                            {
                                                string[] tokens = dsSlider[i]["PictureName"].ToString().Split(delim3);

                                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(dsSlider[i]["PictureName"].ToString());
                                                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                                Stream receiveStream = response.GetResponseStream();
                                                // read the stream
                                                System.Drawing.Image image = System.Drawing.Image.FromStream(receiveStream);
                                                receiveStream.Close();
                                                response.Close();


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
                                                    string realName = dat.MakeNiceName(nameTokens[0]).Replace("_", " ");
                                                    literal4.Text = "<div class=\"RotatorImage\"><img alt=\"" + realName +
                                                        "\" style=\" margin-left: " + ((410 - newIntWidth) / 2).ToString() + "px; margin-top: " + ((250 - newHeight) / 2).ToString() + "px;\" height=\"" + newHeight + "px\" width=\"" + newIntWidth + "px\" src=\""
                                                        + dsSlider[i]["PictureName"].ToString() + "\" /></div>";
                                                    Telerik.Web.UI.RadRotatorItem r4 = new Telerik.Web.UI.RadRotatorItem();
                                                    r4.Controls.Add(literal4);

                                                    Rotator1.Items.Add(r4);
                                            }
                                            else
                                            {
                                                string[] tokens = dsSlider[i]["PictureName"].ToString().Split(delim3);

                                                //dsSlider.RowFilter = "PictureName='" + tokens[0] + "." + tokens[1] + "'";
                                                if (tokens.Length >= 2 && dsSlider.Count > 0)
                                                {
                                                    if (tokens[1].ToUpper() == "JPG" || tokens[1].ToUpper() == "JPEG" || tokens[1].ToUpper() == "GIF" || tokens[1].ToUpper() == "PNG")
                                                    {
                                                        System.Drawing.Image image = System.Drawing.Image.FromFile(MapPath(".") +
                                                            "\\UserFiles\\Events\\" + ID + "\\Slider\\" + dsSlider[i]["PictureName"].ToString());


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
                                                        string realName = dat.MakeNiceName(nameTokens[0]).Replace("_", " ");
                                                        literal4.Text = "<div class=\"RotatorImage\"><img alt=\"" + realName +
                                                    "\" style=\" margin-left: " + ((410 - newIntWidth) / 2).ToString() + "px; margin-top: " + ((250 - newHeight) / 2).ToString() + "px;\" height=\"" + newHeight + "px\" width=\"" + newIntWidth + "px\" src=\""
                                                            + "UserFiles/Events/" + ID + "/Slider/" + dsSlider[i]["PictureName"].ToString() + "\" /></div>";
                                                        Telerik.Web.UI.RadRotatorItem r4 = new Telerik.Web.UI.RadRotatorItem();
                                                        r4.Controls.Add(literal4);

                                                        Rotator1.Items.Add(r4);
                                                    }
                                                    //else if (tokens[1].ToUpper() == "WMV")
                                                    //{
                                                    //    Literal literal4 = new Literal();
                                                    //    literal4.Text = "<div style=\"width: 410px; height: 250px;\" ><OBJECT stop=\"true\" loop=\"false\" controller=\"true\" wmode2=\"opaque\" wmode=\"transparent\" autoplay=\"false\" classid=\"clsid:02BF25D5-8C17-4B23-BC80-D3488ABDDC6B\" " +
                                                    //    "width=\"410\" height=\"250\" codebase=\"http://www.apple.com/qtactivex/qtplugin.cab\">" +
                                                    //    "<param name=\"src\" value=\"UserFiles/Events/" + ID + "/Slider/" + fileArray[i].Split(delim)[length - 1].ToString() + "\"></param>" +
                                                    //    "<param name=\"autoplay\" value=\"false\"></param><param name=\"wmode\" value=\"transparent\"></param>" +
                                                    //    "<param name=\"controller\" value=\"true\"></param>" +
                                                    //    "<param name=\"stop\" value=\"true\" ></param>" +
                                                    //    "<param name=\"loop\" value=\"false\"><param  name=\"wmode2\" value=\"opaque\" ></param>" +
                                                    //    "<EMBED stop=\"true\" wmode=\"transparent\" wmode2=\"opaque\" src=\"UserFiles/Events/" + ID + "/Slider/" + fileArray[i].Split(delim)[length - 1].ToString() + "\" width=\"410\" height=\"250\" autoplay=\"false\" " +
                                                    //    "controller=\"true\" loop=\"false\" bgcolor=\"#000000\" pluginspage=\"http://www.apple.com/quicktime/download/\">" +
                                                    //    "</EMBED>" +
                                                    //    "</OBJECT></div>";
                                                    //    Telerik.Web.UI.RadRotatorItem r4 = new Telerik.Web.UI.RadRotatorItem();
                                                    //    r4.Controls.Add(literal4);
                                                    //    Rotator1.Items.Add(r4);
                                                    //}
                                                }
                                            }

                                        }
                                    }
                                    break;
                                default: break;
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


                            if (fillUserData)
                            {
                                //ASP.controls_addtocalendar_ascx AddTo1 = new ASP.controls_addtocalendar_ascx();
                                //AddTo1.ID = "AddTo1";
                                //AddTo1.EVENT_ID = int.Parse(ID);
                                //AddTo1.DataBind2();

                                #region Do Calendar Link
                                string monthStart = DateTimeStart.Month.ToString();
                                if (monthStart.Length == 1)
                                    monthStart = "0" + monthStart;
                                string dayStart = DateTimeStart.Day.ToString();
                                if (dayStart.Length == 1)
                                    dayStart = "0" + dayStart;

                                string monthEnd = "";
                                string dayEnd = "";
                                string yearEnd = "";
                                if (isEnd)
                                {
                                    monthEnd = DateTimeEnd.Month.ToString();
                                    if (monthEnd.Length == 1)
                                        monthEnd = "0" + monthEnd;
                                    dayEnd = DateTimeEnd.Day.ToString();
                                    if (dayEnd.Length == 1)
                                        dayEnd = "0" + dayEnd;

                                    yearEnd = DateTimeEnd.Year.ToString();
                                }
                                else
                                {
                                    monthEnd = monthStart;
                                    dayEnd = dayStart;
                                    yearEnd = DateTimeStart.Year.ToString();
                                }

                                string shortDesc = dat.stripHTML(ShowDescriptionBegining.Text).Replace(" ", "+");

                                if (shortDesc.Length > 1400)
                                    shortDesc = shortDesc.Substring(0, 1400) + "...";

                                string googleCalendarLink = "https://www.google.com/calendar/render?action=TEMPLATE&" +
                                    "dates=" + DateTimeStart.Year.ToString() + monthStart + dayStart +
                                        "T120000Z/" + yearEnd + monthEnd + dayEnd + "T130000Z" +
                                    "&sprop=" +
                                    "website:http://hippohappenings.com/" + dat.MakeNiceName(ds.Tables[0].Rows[0]["Header"].ToString()) + "_CLHH" + GetEventID() + "_ClEvent" +
                                    "&text=" +
                                    ds.Tables[0].Rows[0]["Header"].ToString().Replace("/", "%2f").Replace(" ", "+") +
                                    "&location=" +
                                    "Location" +
                                    "&sprop=name:" +
                                    ds.Tables[0].Rows[0]["Header"].ToString().Replace("/", "%2f").Replace(" ", "+") +
                                    "&details=" +
                                    "craigslist+ad+http://hippohappenings.com/" + dat.MakeNiceName(ds.Tables[0].Rows[0]["Header"].ToString()) +
                                    "_" + GetEventID() + "_Event+" + shortDesc +
                                    "&gsessionid=OK&sf=true&output=xml";
                                CalendarLiteral.Text = "<a target=\"_blank\" class=\"NavyLink12UD\" href=\"" + googleCalendarLink +
                                    "\" title=\"Add this Meetup to your Google calendar\">" +
                                    "<span class=\"calOpt google\"></span>Add To Google Calendar</a>";
                                //DateAndTimeLabel.Text = googleCalendarLink;\
                                #endregion

                                ASP.controls_sendmessage_ascx SendMessage1 = new ASP.controls_sendmessage_ascx();
                                SendMessage1.THE_TEXT = "Share on Hippo";
                                SendMessage1.RE_LABEL = "Re: " + Session["UserName"].ToString() +
                                    " would like to share \"" + dat.BreakUpString(ds.Tables[0].Rows[0]["Header"].ToString(), 14) + "\" with you.";
                                SendMessage1.TYPE = "e";
                                SendMessage1.ID = int.Parse(ID);

                                //CalendarSharePanel.Controls.Add(AddTo1);
                                CalendarSharePanel.Controls.Add(SendMessage1);

                                Session["Subject"] = "Re: " + Session["UserName"].ToString() +
                                    " would like to share \"" + dat.BreakUpString(ds.Tables[0].Rows[0]["Header"].ToString(), 14) + "\" with you.";

                            }


                            //        DiggLiteral.Text = "<table>" +
                            //    "<tr>" +
                            //        "<td valign=\"bottom\" style=\"padding-right: 10px;\">" +
                            //         "   <a name=\"fb_share\" type=\"button\" href=\"http://www.facebook.com/sharer.php\">Share</a><script src=\"http://static.ak.fbcdn.net/connect.php/js/FB.Share\" type=\"text/javascript\"></script>" +
                            //       " </td>" +
                            //        "<td valign=\"bottom\" style=\"padding-right: 10px;\">" +
                            //        "    <a style=\"border: 0; padding: 0; margin: 0;\" id=\"TweeterA\" title=\"Click to send this page to Twitter!\" target=\"_blank\" rel=\"nofollow\"><img style=\"border: 0; padding: 0; margin: 0;\" src=\"http://twitter-badges.s3.amazonaws.com/twitter-a.png\" alt=\"Share on Twitter\"/></a>" +
                            //        "</td>" +
                            //        "<td valign=\"bottom\" style=\"padding-right: 10px;\">" +
                            //          "  <a href=\"javascript:void(window.open('http://www.myspace.com/Modules/PostTo/Pages/?u='+encodeURIComponent(document.location.toString()),'ptm','height=450,width=440').focus())\">" +
                            //          "      <img src=\"http://cms.myspacecdn.com/cms/ShareOnMySpace/small.png\" border=\"0\" alt=\"Share on MySpace\" />" +
                            //          "  </a>" +
                            //        "</td>" +
                            //        "<td valign=\"bottom\" style=\"padding-right: 10px;\"><a alt=\"Digg it!\" class=\"DiggThisButton DiggIcon\" id=\"dibbButt\"" +
                            //        "href='http://digg.com/submit?phase=2&url=" + "http://" +
                            //        Request.Url.Authority + "/" +
                            //        niceName +
                            //        "_" + ds.Tables[0].Rows[0]["ID"].ToString() + "_Event" +
                            //        "' target=\"_blank\">Digg</a></td>" +
                            //        "<td valign=\"bottom\" style=\"padding-right: 10px;\">" +

                            //            "<a href=\"http://delicious.com/save\" onclick=\"window.open('http://delicious.com/save?v=5&noui&jump=close&url='+encodeURIComponent(location.href)+'&title='+encodeURIComponent(document.title), 'delicious','toolbar=no,width=550,height=550'); return false;\">" +
                            //             "   <img border=\"0\" src=\"http://static.delicious.com/img/delicious.small.gif\" height=\"10\" width=\"10\" alt=\"Delicious\" />" +
                            //            "</a>" +
                            //        "</td>" +
                            //        "<td>" +
                            //          "  <script src=\"http://www.stumbleupon.com/hostedbadge.php?s=4\"></script>" +
                            //        "</td>" +
                            //    "</tr>" +
                            //"</table>";

                        }
                        else
                        {

                            EventName.Text = "This event has been disabled";
                        }

                    }
                    else
                    {
                        //Response.Redirect("~/home");
                    }
                }
                else
                {
                    //Response.Redirect("~/home");
                }
            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text += ex.ToString() + "<br/><br/>" + message;
            //Response.Redirect("~/home");
        }

    }

    protected string GetEventID()
    {
        //HttpRequest request = ((HttpApplication)(sender)).Request;
        //string requestPath = Request.Url.AbsolutePath.Substring(applicationPath.Length);
        string applicationPath = Request.ApplicationPath;
        if ((applicationPath == "/"))
        {
            applicationPath = String.Empty;
        }
        string requestPath = Request.Url.AbsolutePath.Substring(applicationPath.Length);
        string eventID = "";

        if (Request.QueryString["EventID"] == null)
        {
            int start = 6;
            string theInt = "";
            int theIntOut = 0;
            string theI = "";

            string tmp = "";
            for (int i = 0; i < requestPath.Length; i++)
            {
                tmp += requestPath[requestPath.Length - i - 1].ToString();
            }
            requestPath = tmp;
            theI = requestPath.Substring(start, 1);
            while (int.TryParse(theI, out theIntOut))
            {
                theInt = theIntOut.ToString() + theInt;
                theI = requestPath.Substring(++start, 1);
            }
            eventID = theInt;
        }
        else
        {
            eventID = Request.QueryString["EventID"].ToString();
        }

        return eventID;
    }

    private string GetMonth(string month)
    {
        switch (month)
        {
            case "1":
                return "January";
                break;
            case "2":
                return "Febuary";
                break;
            case "3":
                return "March";
                break;
            case "4":
                return "April";
                break;
            case "5":
                return "May";
                break;
            case "6":
                return "June";
                break;
            case "7":
                return "July";
                break;
            case "8":
                return "August";
                break;
            case "9":
                return "September";
                break;
            case "10":
                return "October";
                break;
            case "11":
                return "November";
                break;
            case "12":
                return "December";
                break;
            default:
                return "January";
                break;

        }
    }

    protected void GoToEdit(object sender, EventArgs args)
    {
        string eventID = GetEventID();

        Response.Redirect("blog-event?edit=true&ID="+eventID);

    }

    protected void GetOtherEvents()
    {
        string eventID = GetEventID();
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

        DataView dvEvent = dat.GetDataDV("SELECT * FROM Events WHERE ID = " + eventID);

        string country = dvEvent[0]["Country"].ToString();
        string state = dvEvent[0]["State"].ToString();
        string city = dvEvent[0]["City"].ToString();

        DataView dvEventCategories = dat.GetDataDV("SELECT * FROM Events E, Event_Category_Mapping ECM WHERE E.ID=ECM.EventID AND E.ID=" + eventID);

        string similarQuery = "SELECT DISTINCT E.PostedOn, CASE WHEN E.DaysFeatured IS NULL THEN 'False' WHEN E.DaysFeatured LIKE '%" +
            isNow.Date.ToShortDateString() + "%' THEN 'True' ELSE 'False' END AS Featured, EO.EventID, E.Venue, V.Name, E.Content, E.Header FROM Events E, Event_Category_Mapping ECM, Event_Occurance EO, Venues V WHERE " +
            "EO.EventID=E.ID AND EO.DateTimeStart > GETDATE() AND EO.DateTimeStart < DATEADD(month,2,GETDATE())" +
            " AND V.ID=E.Venue AND E.Live='True' AND E.ID=ECM.EventID AND E.Country=" + country +
            " AND E.State='" + state + "' AND E.City='" + city + "' AND E.ID <> " + eventID;
        
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
                similarQuery += " ECM.CategoryID=" + row["CategoryID"].ToString();
                gotfirst = true;
            }
            similarQuery += " ) ";
        }
        //ErrorLabel.Text += similarQuery;
        DataView dvSimilar = dat.GetDataDV(similarQuery + " ORDER BY Featured DESC, PostedOn ASC");

        string andNot = "";
        foreach (DataRowView row in dvSimilar)
        {
            andNot += " AND E.ID <> " + row["EventID"].ToString();
        }

        Literal lit = new Literal();

        DataView dvEvent2 = dat.GetDataDV("SELECT * FROM Events E, Event_Occurance EO WHERE E.ID=EO.EventID AND E.ID=" + eventID);

        string thisDayQuery = "SELECT DISTINCT E.PostedOn, EO.EventID, CASE WHEN E.DaysFeatured IS NULL THEN 'False' WHEN E.DaysFeatured LIKE '%" + 
            isNow.Date.ToShortDateString() + "%' THEN 'True' ELSE 'False' END AS Featured, E.Venue, V.Name, E.Content, E.Header FROM Events E, Event_Category_Mapping ECM, Event_Occurance EO, Venues V WHERE " +
            "EO.EventID=E.ID AND CONVERT(NVARCHAR,Month([DateTimeStart])) + '/' + CONVERT(NVARCHAR, DAY([DateTimeStart])) + " +
            "'/' + CONVERT(NVARCHAR, YEAR([DateTimeStart])) = CONVERT(NVARCHAR,Month(CONVERT(DATETIME,'" +
            dvEvent2[0]["DateTimeStart"].ToString() + "'))) + '/' + CONVERT(NVARCHAR, DAY(CONVERT(DATETIME,'" + dvEvent2[0]["DateTimeStart"].ToString() + "'))) + " +
            "'/' + CONVERT(NVARCHAR, YEAR(CONVERT(DATETIME,'" + dvEvent2[0]["DateTimeStart"].ToString() + "'))) " +
            " AND V.ID=E.Venue AND E.Live='True' AND E.ID=ECM.EventID AND E.Country=" + country +
            " AND E.State='" + state + "' AND E.City='" + city + "' AND E.ID <> " + eventID + andNot +
            " ORDER BY Featured DESC, PostedOn ASC";
        
        DataView dvDay = dat.GetDataDV(thisDayQuery);

        if (dvDay.Count > 0)
        {
            lit = new Literal();
            lit.Text = "<a id=\"Also-O-This-Day\" class=\"aboutLink\"><h1 " +
                "class=\"SideColumn\">Also On This Day</h1><div class=\"Text12 SimilarSide\"></a>";

            OtherEventsPanel.Controls.Add(lit);

            if (dvSimilar.Count >= 3)
            {
                DrawEvents(dvDay, 3);
            }
            else
            {
                DrawEvents(dvDay, 6 - dvSimilar.Count);
            }

            lit = new Literal();
            lit.Text = "</div>";

            OtherEventsPanel.Controls.Add(lit);
        }

        if (dvSimilar.Count > 0)
        {
            lit = new Literal();
            lit.Text = "<a id=\"Similar-Events\" class=\"aboutLink\"><h1 class=\"SideColumn\">Similar Events</h1><div class=\"Text12 SimilarSide\"></a>";

            OtherEventsPanel.Controls.Add(lit);

            if (dvDay.Count >= 3)
            {
                DrawEvents(dvSimilar, 3);
            }
            else
            {
                DrawEvents(dvSimilar, 6 - dvDay.Count);
            }

            lit = new Literal();
            lit.Text = "</div>";

            OtherEventsPanel.Controls.Add(lit);
        }
    }

    protected void DrawEvents(DataView dvEvents, int cutOff)
    {
        string eventID = GetEventID();
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
                dvEvent = dat.GetDataDV("SELECT * FROM Events E, Event_Occurance EO WHERE E.ID=EO.EventID AND E.ID=" + row["EventID"].ToString());
                lit.Text += "<div class=\"SimilarSide\">";
                lit.Text += "<a class=\"Blue12Link\" href=\"" + dat.MakeNiceName(row["Header"].ToString()) +
                    "_" + row["EventID"].ToString() + "_Event\">" + row["Header"].ToString() +
                    "</a> at " + "<a class=\"Green12LinkNF\" href=\"" + dat.MakeNiceName(row["Name"].ToString()) +
                    "_" + row["Venue"].ToString() + "_Venue\">" + row["Name"].ToString() + "</a> on " +
                    DateTime.Parse(dvEvent[0]["DateTimeStart"].ToString()).ToShortDateString() +
                    ". " + dat.BreakUpString(contentSub, 30) +
                    "... <a class=\"Blue12Link\" href=\"" + dat.MakeNiceName(row["Header"].ToString()) +
                    "_" + row["EventID"].ToString() + "_Event\">Read More</a>";
                lit.Text += "</div>";
                count++;
            }
        }

        OtherEventsPanel.Controls.Add(lit);
    }

    protected void GoingClick(object sender, EventArgs e)
    {
        Data dat = new Data(DateTime.Now);

        dat.Execute("INSERT INTO ClEventGoing (UserID, CLEventID) VALUES (" + Session["User"].ToString() + ", '" + GetEventID() + "')");

        Response.Redirect(Request.RawUrl);
    }
}
