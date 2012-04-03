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

public partial class Event : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
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
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        try
        {
            Session["RedirectTo"] = Request.Url.AbsoluteUri;


            
            
            //Button button = (Button)dat.FindControlRecursive(this, "EventLink");
            //button.CssClass = "NavBarImageEventSelected";

            bool fillUserData = false;

            try
            {
                

                if (Session["User"] != null)
                {
                    if (Session["User"].ToString() == "1")
                        OnlyHippoPanel.Visible = true;
                    LoggedInPanel.Visible = true;
                    LoggedOutPanel.Visible = false;
                    dat.GetRecommendationIcons(eventID, ref RecomPanel);

                    RecomPanel.Width = 10 * (RecomPanel.Controls.Count);
                    
                    DataSet ds1 = dat.GetData("SELECT UserName FROM Users WHERE User_ID=" + Session["User"].ToString());
                    Session["UserName"] = ds1.Tables[0].Rows[0]["UserName"].ToString();
                    fillUserData = true;

                    EditLink.Visible = true;
                    TopPanel.Visible = true;

                    //Query whether current owner was delinquent on approve/reject changes
                    if (dat.IsOwnerDelinquent(eventID, Request.IsLocal, "E"))
                    {
                        //make the button visable
                        TopPanel.Visible = true;
                        OwnerPanel.Visible = true;
                        Session["Message"] = "The ownership of this event is <b>open</b>. <br/>The ownership became " +
                            "open because the previous owner of this event became un-responsive to rejecting/" +
                            "approving user's changes to this event.<br/>If you would like to become " +
                            "the owner, click on the button below to go to the event's edit page. <br/><br/> " +
                            "Being the owner, you will have the privilage of having your <b>edits come though right " +
                            "away.</b> Other participants' changes to this event will have to be <b>approved by you.</b><br/><br/>" +
                            "<button style=\"cursor: pointer;font-size: 11px;font-weight: bold;margin-top: 20px; padding-bottom: 4px;height: 30px; width: 112px;background-color: transparent; " +
                            "color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat: " +
                            "no-repeat; border: 0;\" onclick=\"Search('BlogEvent.aspx?edit=true&ID=" + eventID +
                            "');\" onmouseover=\"this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'\" " +
                            "onmouseout=\"this.style.backgroundImage='url(image/PostButtonNoPost.png)'\">Edit</button>" +
                            "<button style=\"cursor: pointer;font-size: 11px;font-weight: bold;margin-top: 20px; padding-bottom: 4px;height: 30px; width: 112px;background-color: transparent; " +
                            "color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat: " +
                            "no-repeat; border: 0;\" onclick=\"Search();\" " +
                            "onmouseover=\"this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'\" " +
                            "onmouseout=\"this.style.backgroundImage='url(image/PostButtonNoPost.png)'\">Close</button>";
                    }
                }
                else
                {
                    LoggedOutPanel.Visible = true;
                    LoggedInPanel.Visible = false;
                    //Button calendarLink = (Button)dat.FindControlRecursive(this, "CalendarLink");
                    //calendarLink.Visible = false;
                    //EditLink.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.ToString();
            }

            if (ReturnPanel.Visible)
            {
                if (EditLink.Visible)
                {
                    if (OwnerPanel.Visible)
                    {
                        TopPanel.Width = 430;
                    }
                    else
                    {
                        TopPanel.Width = 240;
                    }
                }
                else
                {
                    if (OwnerPanel.Visible)
                    {
                        TopPanel.Width = 300;
                    }
                    else
                    {
                        TopPanel.Width = 150;
                    }
                }
            }
            else
            {
                if (EditLink.Visible)
                {
                    if (OwnerPanel.Visible)
                    {
                        TopPanel.Width = 250;
                    }
                    else
                    {
                        TopPanel.Width = 80;

                    }
                    EditLink.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Edit Event";
                }
                else
                {
                    if (OwnerPanel.Visible)
                    {
                        TopPanel.Width = 160;
                    }
                    else
                    {
                        TopPanel.Width = 0;
                    }
                    HyperLink1.InnerHtml = "&nbsp;&nbsp;&nbsp;&nbsp;Event Ownership is Open";
                }
            }

            //Overwrite everything if the event has passed
            if (dat.HasEventPassed(eventID))
            {
                OwnerPanel.Visible = false;
                EditLink.Visible = false;
                PassedLink.Visible = true;
                if (ReturnPanel.Visible)
                {
                    TopPanel.Width = 300;
                    PassedLink.Text = "|&nbsp;&nbsp;&nbsp;&nbsp;This event has passed";
                }
                else
                {
                    TopPanel.Width = 125;
                }
            }

            if (Request.QueryString["EventID"] == null)
                Response.Redirect("~/Home.aspx");
            string ID = eventID;
            Session["EventID"] = ID;
            DataSet ds = dat.GetData("SELECT * FROM Events WHERE ID=" + ID);

            Session["FlagID"] = ID;
            Session["FlagType"] = "E";

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (bool.Parse(ds.Tables[0].Rows[0]["Live"].ToString()))
                    {


                        if(ds.Tables[0].Rows[0]["BuyAtTix"] != null)
                            if (ds.Tables[0].Rows[0]["BuyAtTix"].ToString().Trim() != "")
                            {
                                BuyAtTix.Text = "<div style=\"position: relative;\"><div class=\"AddLink\" style=\"padding-top: 5px;float: left;\">Buy Tickets at:&nbsp;&nbsp;</div><a style=\"float: left;background-image: url(http://b1.perfb.com/b1.php?ID=14235&amp;PURL=ticketsus.at/HippoHappenings); background-repeat: no-repeat; width: 110px; display: block; height: 50px; background-position: 0 -75px;\" target=\"_blank\" href=\"" +
                                    ds.Tables[0].Rows[0]["BuyAtTix"].ToString() +
                                    "\"></a></div>";
                            }
                        EventName.Text = "<a style=\"text-decoration: none; color: white;\" href=\"http://" + Request.Url.Authority + "/" +
                            dat.MakeNiceName(dat.BreakUpString(ds.Tables[0].Rows[0]["Header"].ToString(), 14)) +
                            "_" + ID.ToString() + "_Event\">" +
                            dat.BreakUpString(ds.Tables[0].Rows[0]["Header"].ToString(), 14) + "</a>";
                        Session["Subject"] = "Re: " + ds.Tables[0].Rows[0]["Header"].ToString();
                        Session["CommentSubject"] = "Re: " + ds.Tables[0].Rows[0]["Header"].ToString();
                        string UserName = ds.Tables[0].Rows[0]["UserName"].ToString();
                        DataSet dsDate = dat.GetData("SELECT * FROM Event_Occurance WHERE EventID=" + ID);
                        DataSet dsVenue = dat.GetData("SELECT * FROM Venues WHERE ID=" + 
                            ds.Tables[0].Rows[0]["Venue"]);
                        TagCloud.THE_ID = int.Parse(ID);
                        NumberPeopleLabel.Text = dat.GetData("SELECT DISTINCT UserID FROM User_Calendar " +
                            "WHERE EventID=" + ID).Tables[0].Rows.Count.ToString() + " People";

                        if (Session["User"] != null)
                        {
                            DataSet dsFriends = dat.GetData("SELECT DISTINCT UC.EventID, UC.UserID FROM User_Friends UF, User_Calendar UC " +
                                "WHERE UC.EventID=" +
                                ID + " AND UC.UserID=UF.FriendID AND UF.UserID=" + Session["User"].ToString());


                            CommunicateLiteral.Text = "<img style=\"float:right;\" src=\"image/CommunicateButton.png\" onmouseover=\"this.src='image/CommunicateButtonSelected.png'\" onmouseout=\"this.src='image/CommunicateButton.png'\" onclick=\"javascript:OpenRad('" +
                                ID + "');\"/>";

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
                            CommunicateLiteral.Text = "</td></tr><tr><td><div style=\"float: left; clear: both; width: 450px;\">To communicate with people going to this event, please <a class=\"AddLink\" href=\"UserLogin.aspx\">log in.</a></div>";
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
                            else if(songCount == 1)
                            {
                                songs.SONG1 = dsSongs.Tables[0].Rows[0]["SongName"].ToString();
                                songs.SONG1_TITLE = dsSongs.Tables[0].Rows[0]["SongTitle"].ToString();
                            }

                            songs.USER_NAME = ds.Tables[0].Rows[0]["UserName"].ToString();

                            SongPanel.Controls.Add(songs);
                        }



                        VenueName.Text = dsVenue.Tables[0].Rows[0]["Name"].ToString();
                        VenueName.NavigateUrl = dat.MakeNiceName(dsVenue.Tables[0].Rows[0]["Name"].ToString()) +
                            "_" + dsVenue.Tables[0].Rows[0]["ID"].ToString() + "_Venue";

                        DateTime date;
                        DateTime endDate;
                        DateAndTimeLabel.Text = "";
                        string dateandtime = "";
                        bool dateGotten = false;
                        string moreDates = "";
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
                                DateAndTimeLabel.Text += date.DayOfWeek.ToString() + ", " + GetMonth(date.Month.ToString()) + " " +
                                    date.Day + " " + date.ToShortTimeString()
                                    + " <span style=\"color: #cccccc; font-weight: bold;\">To</span> " +
                                    endDate.DayOfWeek.ToString() + ", " + GetMonth(endDate.Month.ToString()) + " " +
                                    endDate.Day + " " + endDate.ToShortTimeString();
                            }
                            else
                            {


                                if (moreDates == "")
                                    moreDates = "<div onclick=\"OpenMoreDates();\" style=\"cursor: pointer;\" class=\"AddLink\" id=\"MoreDatesName\">More Dates</div>" +
                                        "<div style=\"display: none;\" id=\"infoDiv\">" +
                                        (dsDate.Tables[0].Rows.Count * 40).ToString() +
                                        "</div><div style=\"display: none; background-color: #333333; padding: 10px;\" " +
                                        "id=\"MoreDatesDiv\"> <div class=\"AddLink\" style=\"float: right; cursor: pointer; font-style: none;\" " +
                                               "onclick=\"OpenMoreDates();\">close</div><br/><br/>";
                                moreDates += date.DayOfWeek.ToString() + ", " + GetMonth(date.Month.ToString()) + " " +
                                    date.Day + " " + date.ToShortTimeString()
                                    + " <span style=\"color: #cccccc; font-weight: bold;\">To</span> " +
                                    endDate.DayOfWeek.ToString() + ", " + GetMonth(endDate.Month.ToString()) + " " +
                                    endDate.Day + " " + endDate.ToShortTimeString() + "<br/>";
                            }

                            
                            
                        }
                        if (moreDates != "")
                            moreDates += "</div>";

                        DateAndTimeLabel.Text += moreDates;

                        string content = ds.Tables[0].Rows[0]["Content"].ToString();
                        string niceName = dat.MakeNiceName(ds.Tables[0].Rows[0]["Header"].ToString());
                        

                        ScriptLiteral.Text = "<script type=\"text/javascript\">ReturnURL('" + niceName.Replace("_", " ") + " at " + dat.MakeNiceName(dsVenue.Tables[0].Rows[0]["Name"].ToString()).Replace("_", " ") + " Tonight http://HippoHappenings.com/" + Request.QueryString["EventID"].ToString() + "_Event');</script>";

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

                        ShowDescriptionBegining.Text = dat.BreakUpString(content, 60);


                        //Create keyword and description meta tags and title
                        HtmlMeta hm = new HtmlMeta();
                        HtmlMeta kw = new HtmlMeta();

                        HtmlHead head = (HtmlHead)Page.Header;


                        kw.Name = "keywords";
                        kw.Content = dat.BreakUpString(ds.Tables[0].Rows[0]["Header"].ToString(), 14) + ", " + VenueName.Text + ", " +
                            dsVenue.Tables[0].Rows[0]["City"].ToString() + ", " +
                            dsVenue.Tables[0].Rows[0]["State"].ToString();


                        HtmlLink lk = new HtmlLink();
                        lk.Href = "http://" + Request.Url.Authority + "/" +
                            dat.MakeNiceName(dat.BreakUpString(ds.Tables[0].Rows[0]["Header"].ToString(), 14)) +
                            "_" + ID.ToString() + "_Event";
                        lk.Attributes.Add("rel", "bookmark");
                        head.Controls.AddAt(0, lk);


                        DataView dvCats = dat.GetDataDV("SELECT DISTINCT C.ID, ECM.ID AS EID, C.Name AS CategoryName, ECM.tagSize FROM Event_Category_Mapping ECM, EventCategories C WHERE ECM.CategoryID=C.ID AND ECM.EventID=" + eventID + " ORDER BY ECM.ID");

                        string justCats = "";

                        for (int i = 0; i < dvCats.Count; i++)
                        {
                            kw.Content += ", " + dvCats[i]["CategoryName"].ToString();
                            justCats += dvCats[i]["CategoryName"].ToString()+ " ";
                        }

                        head.Controls.AddAt(0, kw);

                        hm.Name = "Description";
                        hm.Content = ShowDescriptionBegining.Text + ", " + dat.BreakUpString(ds.Tables[0].Rows[0]["Header"].ToString(), 14) + ", " + VenueName.Text + ", " + kw.Content;
                        head.Controls.AddAt(0, hm);

                        this.Title = dat.BreakUpString(ds.Tables[0].Rows[0]["Header"].ToString(), 14) + " | " + justCats + " | HippoHappenings";



                        Session["messageText"] = dat.BreakUpString(ds.Tables[0].Rows[0]["Header"].ToString(), 14) + " occurs at " + VenueName.Text + " on " + dateandtime;
                        Session["messageEmail"] = "EventName: <a href=\"http://hippohappenings.com/" + dat.MakeNiceName(dat.BreakUpString(ds.Tables[0].Rows[0]["Header"].ToString(), 14)) + "_" + ID + "_Event\">" +
                            dat.BreakUpString(ds.Tables[0].Rows[0]["Header"].ToString(), 14) + "</a> <br/><br/> Venue: <a href=\"http://hippohappenings.com/" + dat.MakeNiceName(VenueName.Text) + "_" +
                            dsVenue.Tables[0].Rows[0]["ID"].ToString() + "_Venue\">" + VenueName.Text + "</a> <br/><br/> Date: " +
                            DateAndTimeLabel.Text + " <br/><br/> " + ShowDescriptionBegining.Text;

                        //Media Categories: NONE: 0, Picture: 1, Video: 2, YouTubeVideo: 3, Slider: 4
                        Rotator1.Items.Clear();
                        int mediaCategory = int.Parse(ds.Tables[0].Rows[0]["mediaCategory"].ToString());
                        string youtube = ds.Tables[0].Rows[0]["YouTubeVideo"].ToString();
                        switch (mediaCategory)
                        {
                            case 0:
                                break;
                            case 1:
                                ShowVideoPictureLiteral.Text = "";
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
                                            literal3.Text = "<div style=\"float:left; z-index: 1;\"><object class=\"toHidde\" width=\"400\" "+
                                                "height=\"250\"><param name=\"movie\" value=\"http://www.youtube.com/v/" + youtokens[i] +
                                                "\"/><param  name=\"wmode2\" value=\"transparent\" /><param  name=\"wmode\" "+
                                                "value=\"opaque\" /><param name=\"allowFullScreen\" value=\"true\"/><embed "+
                                                "src=\"http://www.youtube.com/v/" +
                                                youtokens[i] + "\" wmode=\"opaque\" wmode2=\"transparent\" type=\"application/x-shockwave"+
                                                "-flash\" allowfullscreen=\"true\" width=\"400\" height=\"250\"/></object></div>";
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
                                    string[] fileArray = System.IO.Directory.GetFiles(MapPath(".") + "\\UserFiles\\Events\\" + ID + "\\Slider");

                                    string[] finalFileArray = new string[fileArray.Length];

                                    for (int i = 0; i < fileArray.Length; i++)
                                    {
                                        int length = fileArray[i].Split(delim).Length;
                                        finalFileArray[i] = "http://" + Request.Url.Authority + "/HippoHappenings/UserFiles/Events/" +
                                            ID + "/Slider/" + fileArray[i].Split(delim)[length - 1];
                                        string[] tokens = fileArray[i].Split(delim)[length - 1].Split(delim3);

                                        dsSlider.RowFilter = "PictureName='" + tokens[0] + "." + tokens[1] + "'";
                                        if (tokens.Length >= 2 && dsSlider.Count > 0)
                                        {
                                            if (tokens[1].ToUpper() == "JPG" || tokens[1].ToUpper() == "JPEG" || tokens[1].ToUpper() == "GIF" || tokens[1].ToUpper() == "PNG")
                                            {
                                                System.Drawing.Image image = System.Drawing.Image.FromFile(MapPath(".") + "\\UserFiles\\Events\\" + ID + "\\Slider\\" + fileArray[i].Split(delim)[length - 1].ToString());


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
                                                literal4.Text = "<div style=\"width: 410px; height: 250px;background-color: black;\"><img style=\" margin-left: " + ((410 - newIntWidth) / 2).ToString() + "px; margin-top: " + ((250 - newHeight) / 2).ToString() + "px;\" height=\"" + newHeight + "px\" width=\"" + newIntWidth + "px\" src=\""
                                                    + "UserFiles/Events/" + ID + "/Slider/" + fileArray[i].Split(delim)[length - 1].ToString() + "\" /></div>";
                                                Telerik.Web.UI.RadRotatorItem r4 = new Telerik.Web.UI.RadRotatorItem();
                                                r4.Controls.Add(literal4);

                                                Rotator1.Items.Add(r4);
                                            }
                                            else if (tokens[1].ToUpper() == "WMV")
                                            {
                                                Literal literal4 = new Literal();
                                                literal4.Text = "<div style=\"width: 410px; height: 250px;\" ><OBJECT stop=\"true\" loop=\"false\" controller=\"true\" wmode2=\"opaque\" wmode=\"transparent\" autoplay=\"false\" classid=\"clsid:02BF25D5-8C17-4B23-BC80-D3488ABDDC6B\" " +
                                                "width=\"410\" height=\"250\" codebase=\"http://www.apple.com/qtactivex/qtplugin.cab\">" +
                                                "<param name=\"src\" value=\"UserFiles/Events/" + ID + "/Slider/" + fileArray[i].Split(delim)[length - 1].ToString() + "\"></param>" +
                                                "<param name=\"autoplay\" value=\"false\"></param><param name=\"wmode\" value=\"transparent\"></param>" +
                                                "<param name=\"controller\" value=\"true\"></param>" +
                                                "<param name=\"stop\" value=\"true\" ></param>" +
                                                "<param name=\"loop\" value=\"false\"><param  name=\"wmode2\" value=\"opaque\" ></param>" +
                                                "<EMBED stop=\"true\" wmode=\"transparent\" wmode2=\"opaque\" src=\"UserFiles/Events/" + ID + "/Slider/" + fileArray[i].Split(delim)[length - 1].ToString() + "\" width=\"410\" height=\"250\" autoplay=\"false\" " +
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
                            ASP.controls_addtocalendar_ascx AddTo1 = new ASP.controls_addtocalendar_ascx();
                            AddTo1.ID = "AddTo1";
                            AddTo1.EVENT_ID = int.Parse(ID);
                            AddTo1.DataBind2();

                            ASP.controls_sendmessage_ascx SendMessage1 = new ASP.controls_sendmessage_ascx();
                            SendMessage1.THE_TEXT = "Share this with a friend";
                            SendMessage1.RE_LABEL = "Re: " + Session["UserName"].ToString() +
                                " would like to share \"" + dat.BreakUpString(ds.Tables[0].Rows[0]["Header"].ToString(), 14) + "\" with you.";
                            SendMessage1.TYPE = "e";
                            SendMessage1.ID = int.Parse(ID);

                            CalendarSharePanel.Controls.Add(AddTo1);
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
                    //Response.Redirect("~/Home.aspx");
                }
            }
            else
            {
                //Response.Redirect("~/Home.aspx");
            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString();
            //Response.Redirect("~/Home.aspx");
        }

        DataSet dsUpdates = dat.GetData("SELECT * FROM EventUpdates WHERE EventID="+eventID);
        RadPanel1.Visible = false;
        RadPanel1.Items[0].Items[0].Text = "";
        for (int i = 0; i < dsUpdates.Tables[0].Rows.Count; i++)
        {
            RadPanel1.Visible = false;
            RadPanel1.Items[0].Items[0].Text += dsUpdates.Tables[0].Rows[i]["eventChange"].ToString() + "<br/><br/>";
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
        HttpCookie cookie2 = Request.Cookies["BrowserDate"];
        HttpCookie cookie = new HttpCookie("editEvent" + eventID);
        cookie.Value = "True";
        cookie.Expires = DateTime.Parse(cookie2.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(1);
        Response.Cookies.Add(cookie);

        Response.Redirect("EditEvent.aspx?&ID="+eventID);

    }

  
}
