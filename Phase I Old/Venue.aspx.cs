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

public partial class Venue : Telerik.Web.UI.RadAjaxPage
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
        try
        {
            Session["RedirectTo"] = Request.Url.AbsoluteUri;
           
            bool fillUserData = false;
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            try
            {


                if (Session["User"] != null)
                {
                    fillUserData = true;
                }
                else
                {
                    Button calendarLink = (Button)dat.FindControlRecursive(this, "CalendarLink");
                    calendarLink.Visible = false;
                }
            }
            catch (Exception ex)
            {
            }

            if (Request.QueryString["ID"] == null)
                Response.Redirect("~/Home.aspx");
            string ID = Request.QueryString["ID"].ToString();

            if (Session["User"] != null)
            {
                LoggedInPanel.Visible = true;
                LoggedOutPanel.Visible = false;
                DataSet dsComments;
                string commentPrefs = dat.GetData("SELECT * FROM UserPreferences WHERE UserID=" + Session["User"].ToString()).Tables[0].Rows[0]["CommentsPreferences"].ToString();
                if (commentPrefs == "1")
                {
                    dsComments = dat.GetData("SELECT VC.CommentDate AS theDate, * FROM Venue_Comments VC, Users U WHERE VC.UserID=U.User_ID AND VC.VenueID=" + ID + " ORDER BY VC.CommentDate ");
                }
                else
                {
                    dsComments = dat.GetData("SELECT DISTINCT U.ProfilePicture, U.User_ID, U.UserName, VC.Comment, VC.CommentDate AS theDate FROM Venue_Comments VC, Users U, User_Friends UF WHERE ((UF.UserID=" + Session["User"].ToString() + " AND UF.FriendID=U.User_ID AND U.User_ID=VC.UserID) OR (U.User_ID=" +
                                        Session["User"].ToString() + " AND U.User_ID=VC.UserID)) AND VC.VenueID=" + ID + " ORDER BY VC.CommentDate");
                }
                TheComments.DATA_SET = dsComments;
                TheComments.DataBind2(true);

               //Show edit link if use is logged in.
                EditLink.NavigateUrl = "EnterVenue.aspx?ID=" + ID;
                EditLink.Visible = true;
                TopPanel.Visible = true;

                //Query whether current owner was delinquent on approve/reject changes
                if (dat.IsOwnerDelinquent(Request.QueryString["ID"].ToString(), Request.IsLocal, "V"))
                {
                    //make the button visable
                    TopPanel.Visible = true;
                    OwnerPanel.Visible = true;
                    Session["Message"] = "The ownership of this venue is <b>open</b>. <br/>The ownership became " +
                        "open because the previous owner of this venue became un-responsive to rejecting/" +
                        "approving user's changes to this venue.<br/>If you would like to become " +
                        "the owner, click on the button below to go to the venue's edit page. <br/><br/> " +
                        "Being the owner, you will have the privilage of having your <b>edits come though right " +
                        "away.</b> Other participants' changes to this venue will have to be <b>approved by you.</b><br/><br/>" +
                        "<button style=\"cursor: pointer;font-size: 11px;font-weight: bold;margin-top: 20px; padding-bottom: 4px;height: 30px; width: 112px;background-color: transparent; " +
                        "color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat: " +
                        "no-repeat; border: 0;\" onclick=\"Search('EnterVenue.aspx?ID=" + Request.QueryString["ID"].ToString() +
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
                LoggedInPanel.Visible = false;
                LoggedOutPanel.Visible = true;
                DataSet dsComments = dat.GetData("SELECT VC.CommentDate AS theDate, * FROM Venue_Comments VC, Users U WHERE VC.UserID=U.User_ID AND VC.VenueID=" + ID + " ORDER BY VC.CommentDate ");
                TheComments.DATA_SET = dsComments;
                TheComments.DataBind2(false);
            }

            DataSet ds = dat.GetData("SELECT * FROM Venues WHERE ID=" + ID);

            Session["FlagID"] = ID;
            Session["FlagType"] = "V";

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (bool.Parse(ds.Tables[0].Rows[0]["Live"].ToString()))
                    {
                        //Get venue's categories
                        DataSet dscat = dat.GetData("SELECT DISTINCT VC.Name, VC.ID FROM Venue_Category V_C, VenueCategories VC WHERE " +
                            "V_C.Venue_ID=" + ID + " AND V_C.Category_ID=VC.ID ");

                        if (dscat.Tables.Count > 0)
                        {
                            if (dscat.Tables[0].Rows.Count > 0)
                            {
                                CategoriesLiteral.Text = "<label class=\"AddWhiteLinkSmall\">Categories:</label> ";
                                int charcount = 0;
                                bool charReached = false;
                                int currentcount = 0;
                                int leftovercount = 0;
                                for (int u = 0; u < dscat.Tables[0].Rows.Count; u++)
                                {
                                    CategoriesLiteral.Text += "<a class=\"AddLinkSmall\" href=\""+dat.MakeNiceName(dscat.Tables[0].Rows[u]["Name"].ToString())+"_Venue_Category\">" + dscat.Tables[0].Rows[u]["Name"].ToString() + "</a> ";
                                    charcount += dscat.Tables[0].Rows[u]["Name"].ToString().Length + 1;
                                    if (dscat.Tables[0].Rows[u]["Name"].ToString().ToLower().Contains("happy hour"))
                                    {
                                        CategoriesLiteral.Text += "<label class=\"AddLinkSmallUn\"> (see venue calendar) </label>";
                                        charcount += 23;

                                        if (charReached)
                                        {
                                            leftovercount += 23;
                                        }
                                    }

                                    if (!charReached)
                                    {
                                        if (charcount > 145)
                                        {
                                            charReached = true;
                                            leftovercount = dscat.Tables[0].Rows[u]["Name"].ToString().Length + 1;
                                        }
                                        else
                                        {
                                            currentcount = CategoriesLiteral.Text.Length;
                                        }
                                    }
                                    else
                                    {
                                        leftovercount += dscat.Tables[0].Rows[u]["Name"].ToString().Length + 1;
                                    }
                                }

                                //if the list is too long, add a 'more' link drop down.
                                if (charReached)
                                {
                                    string beginning = "<div>" + CategoriesLiteral.Text.Substring(0, currentcount - 1) +
                                        " <a onclick=\"OpenCatDiv();\" id=\"morelessA\" class=\"AddWhiteLinkSmall\"> more...</a></div>";


                                    int divHeight = leftovercount / 82;

                                    if (divHeight == 0)
                                        divHeight = 20;
                                    else
                                        divHeight = (divHeight + 1) * 20;


                                    string hiddenDiv = "<div style=\"display: none;\" id=\"infoDiv\">" + divHeight.ToString() +
                                        "</div><div style=\"height: 0px; border: 0; width: 420px;\" id=\"CatDiv\">" + 
                                        CategoriesLiteral.Text.Substring(currentcount, CategoriesLiteral.Text.Length - currentcount) + "</div>";

                                    CategoriesLiteral.Text = beginning + hiddenDiv;
                                }


                            }
                        }

                        string theLink = "http://" + Request.Url.Authority + "/" +
                            dat.MakeNiceName(dat.BreakUpString(ds.Tables[0].Rows[0]["Name"].ToString(), 14)) +
                            "_" + ID.ToString() + "_Venue";

                //        DiggLiteral.Text = " <table> "+
                //    "<tr>"+
                //        "<td valign=\"bottom\" style=\"padding-right: 10px;\">"+
                //            "<a name=\"fb_share\" type=\"button\" href=\"http://www.facebook.com/sharer.php\">Share</a><script src=\"http://static.ak.fbcdn.net/connect.php/js/FB.Share\" type=\"text/javascript\"></script>"+
                //        "</td>"+
                //        "<td valign=\"bottom\" style=\"padding-right: 10px;\">"+
                //         "   <a style=\"border: 0; padding: 0; margin: 0;\" id=\"TweeterA\" title=\"Click to send this page to Twitter!\" target=\"_blank\" rel=\"nofollow\"><img style=\"border: 0; padding: 0; margin: 0;\" src=\"http://twitter-badges.s3.amazonaws.com/twitter-a.png\" alt=\"Share on Twitter\"/></a>"+
                //        "</td>"+
                //        "<td valign=\"bottom\" style=\"padding-right: 10px;\">"+
                //        "    <a href=\"javascript:void(window.open('http://www.myspace.com/Modules/PostTo/Pages/?u='+encodeURIComponent(document.location.toString()),'ptm','height=450,width=440').focus())\">"+
                //        "        <img src=\"http://cms.myspacecdn.com/cms/ShareOnMySpace/small.png\" border=\"0\" alt=\"Share on MySpace\" />"+
                //        "    </a>"+
                //        "</td>"+
                //        "<td valign=\"bottom\" style=\"padding-right: 10px;\"><a alt=\"Digg it!\" class=\"DiggThisButton DiggIcon\" id=\"dibbButt\"" +
                //        "href='http://digg.com/submit?phase=2&url=" + "http://" + Request.Url.Authority + "/" +
                //            dat.MakeNiceName(ds.Tables[0].Rows[0]["Name"].ToString()) +
                //            "_" + ID.ToString() + "_Venue" +
                //            "' target=\"_blank\">Digg</a></td> "+
                        
                //        "<td valign=\"bottom\" style=\"padding-right: 10px;\">" +
                            
                //            "<a href=\"http://delicious.com/save\" onclick=\"window.open('http://delicious.com/save?v=5&noui&jump=close&url='+encodeURIComponent(location.href)+'&title='+encodeURIComponent(document.title), 'delicious','toolbar=no,width=550,height=550'); return false;\">" +
                //             "   <img border=\"0\" src=\"http://static.delicious.com/img/delicious.small.gif\" height=\"10\" width=\"10\" alt=\"Delicious\" />" +
                //            "</a>" +
                //        "</td>" +
                //        "<td>" +
                //        "     <script src=\"http://www.stumbleupon.com/hostedbadge.php?s=4\"></script>" +
                //        "</td>" +
                //    "</tr>" +
                //"</table>";

                        HtmlHead head = (HtmlHead)Page.Header;
                        HtmlLink lk = new HtmlLink();
                        lk.Href = theLink;
                        lk.Attributes.Add("rel", "bookmark");
                        head.Controls.AddAt(0, lk);

                        VenueName.Text = "<a style=\"text-decoration: none; color: white;\" href=\"" + 
                            theLink + "\">" + dat.BreakUpString(ds.Tables[0].Rows[0]["Name"].ToString(), 14) + "</a>";
                        Session["Subject"] = "Re: " + ds.Tables[0].Rows[0]["Name"].ToString();
                        Session["CommentSubject"] = "Re: " + ds.Tables[0].Rows[0]["Name"].ToString();



                        string Venue = dat.BreakUpString(ds.Tables[0].Rows[0]["Name"].ToString(), 14);
                        //TagCloud.THE_ID = int.Parse(ID);




                        PhoneLabel.Text = "Phone: " + ds.Tables[0].Rows[0]["Phone"].ToString() + "<br/>Email: " +
                            ds.Tables[0].Rows[0]["Email"].ToString() + "<br/>Web: <a class='AddLink' target='_blank' href='" + ds.Tables[0].Rows[0]["Web"].ToString() + "'>" + 
                            ds.Tables[0].Rows[0]["Web"].ToString()+"</a>";

                        DataSet dsCountry = new DataSet();
                        string country = "";
                        if (ds.Tables[0].Rows[0]["Country"] != null)
                        {
                            dsCountry = dat.GetData("SELECT country_2_code FROM Countries WHERE country_id=" + ds.Tables[0].Rows[0]["Country"].ToString());
                            if (dsCountry.Tables[0].Rows.Count > 0)
                            {
                                country = dsCountry.Tables[0].Rows[0]["country_2_code"].ToString();
                            }
                        }
                        if (country.ToLower() == "us")
                        {
                            try
                            {
                                AddressLabel.Text = dat.GetAddress(ds.Tables[0].Rows[0]["Address"].ToString(), false);
                            }
                            catch (Exception ex1)
                            {
                                AddressLabel.Text = "";
                            }
                        }
                        else
                        {

                            AddressLabel.Text = dat.GetAddress(ds.Tables[0].Rows[0]["Address"].ToString(), true);



                        }

                        //if (ds.Tables[0].Rows[0]["Country"].ToString() == "222")
                        //{
                        //    CityState.Text = ds.Tables[0].Rows[0]["City"].ToString();
                        //}
                        //else
                        //{
                            CityState.Text = ds.Tables[0].Rows[0]["City"].ToString() + " " +
                            ds.Tables[0].Rows[0]["State"].ToString() + " " + ds.Tables[0].Rows[0]["Zip"].ToString();
                        //}

                        HttpCookie cookie2 = new HttpCookie("addressParameter");
                        HttpCookie cookiename = new HttpCookie("addressParameterName");
                        if (country.ToLower() == "uk")
                        {
                            //VenueName.Text + "@&" + 
                            cookie2.Value = AddressLabel.Text + "@&" +
                                ds.Tables[0].Rows[0]["City"].ToString() + ", " +
                                ds.Tables[0].Rows[0]["Zip"].ToString() + ", " + country;

                            cookiename.Value = dat.BreakUpString(ds.Tables[0].Rows[0]["Name"].ToString(), 14);
                        }
                        else
                        {
                            //VenueName.Text + "@&" + 
                            cookie2.Value = AddressLabel.Text + "@&" + CityState.Text + "@&" + country;
                            cookiename.Value = dat.BreakUpString(ds.Tables[0].Rows[0]["Name"].ToString(), 14);
                        }
                        cookie2.Expires = DateTime.Now.Add(new TimeSpan(1, 0, 0));
                        cookiename.Expires = DateTime.Now.Add(new TimeSpan(1, 0, 0));

                        Response.Cookies.Add(cookie2);
                        Response.Cookies.Add(cookiename);

                        string content = ds.Tables[0].Rows[0]["Content"].ToString();

                        Session["messageText"] = "Venue: " + dat.BreakUpString(ds.Tables[0].Rows[0]["Name"].ToString(), 14) + ". Address: " + AddressLabel.Text + ", " + CityState.Text;
                        Session["messageEmail"] = "Venue: " + dat.BreakUpString(ds.Tables[0].Rows[0]["Name"].ToString(), 14) + " \n\r Address: " + AddressLabel.Text + " \n\r Location: " +
                            CityState.Text + " \n\r " + content;

                        ShowDescriptionBegining.Text = dat.BreakUpString(content, 60);


                        //Create keyword and description meta tags
                        HtmlMeta hm = new HtmlMeta();
                        HtmlMeta kw = new HtmlMeta();



                        kw.Name = "keywords";
                        kw.Content = dat.BreakUpString(ds.Tables[0].Rows[0]["Name"].ToString(), 14) + ", " + AddressLabel.Text;

                        DataView dvCats = dat.GetDataDV("SELECT DISTINCT C.ID, C.Name AS CategoryName, " +
                            "VC.tagSize FROM Venue_Category VC, VenueCategories C WHERE VC.CATEGORY_ID=C.ID " +
                            "AND VC.VENUE_ID=" + ID);

                        string justCats = "";

                        for (int i = 0; i < dvCats.Count; i++)
                        {
                            kw.Content += ", " + dvCats[i]["CategoryName"].ToString();
                            justCats += dvCats[i]["CategoryName"].ToString() + " ";
                        }

                        head.Controls.AddAt(0, kw);

                        hm.Name = "Description";
                        hm.Content = ShowDescriptionBegining.Text + ", " + kw.Content;
                        head.Controls.AddAt(0, hm);

                        this.Title = dat.BreakUpString(ds.Tables[0].Rows[0]["Name"].ToString(), 14) + " | " + justCats + " | HippoHappenings";



                        CalendarLink.NavigateUrl = dat.MakeNiceName(dat.BreakUpString(ds.Tables[0].Rows[0]["Name"].ToString(), 14)) + "_" + ID + "_Calendar";

                        //Media Categories: NONE: 0, Picture: 1, Video: 2, YouTubeVideo: 3, Slider: 4
                        int mediaCategory = int.Parse(ds.Tables[0].Rows[0]["mediaCategory"].ToString());

                        DataSet dsEvents = dat.GetData("SELECT EO.DateTimeStart AS Start, EO.DateTimeEnd AS [End], " +
                            "E.Header, E.ID, E.ShortDescription FROM Event_Occurance EO, Events E WHERE " +
                            "(CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '.' + CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + " +
                            "'.' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart))) = (CONVERT(NVARCHAR,DAY(GETDATE())) + " +
                            "'.' + CONVERT(NVARCHAR,MONTH(GETDATE())) + '.' + CONVERT(NVARCHAR,YEAR(GETDATE())))" +
                            "AND EO.EventID=E.ID AND E.Venue=" + ID + " ORDER BY Start");
                        bool noEvents = false;
                        EventsPanel.Controls.Clear();
                        if (dsEvents.Tables.Count > 0)
                            if (dsEvents.Tables[0].Rows.Count > 0)
                            {
                                int eventCount = dsEvents.Tables[0].Rows.Count;

                                for (int i = 0; i < eventCount; i++)
                                {
                                    HyperLink eventHeader = new HyperLink();
                                    eventHeader.CssClass = "CalendarHeader";
                                    eventHeader.NavigateUrl = dat.MakeNiceName(dsEvents.Tables[0].Rows[i]["Header"].ToString())+"_" + dsEvents.Tables[0].Rows[i]["ID"].ToString()+"_Event";
                                    eventHeader.Text = dsEvents.Tables[0].Rows[i]["Header"].ToString() + "<br/>";

                                    Label dateStart = new Label();
                                    dateStart.CssClass = "CalendarTime";
                                    dateStart.Text = DateTime.Parse(dsEvents.Tables[0].Rows[i]["Start"].ToString()).ToShortTimeString() + " - " +
                                        DateTime.Parse(dsEvents.Tables[0].Rows[i]["End"].ToString()).ToShortTimeString() + "<br/>";

                                    Label shortDescription = new Label();
                                    shortDescription.CssClass = "CalendarDescription";
                                    shortDescription.Text = dsEvents.Tables[0].Rows[i]["ShortDescription"].ToString() + "<br/><br/>";

                                    EventsPanel.Controls.Add(eventHeader);
                                    EventsPanel.Controls.Add(dateStart);
                                    EventsPanel.Controls.Add(shortDescription);
                                }
                            }
                            else
                            {
                                noEvents = true;
                            }
                        else
                        {
                            noEvents = true;
                        }

                        if (noEvents)
                        {
                            HyperLink eventHeader = new HyperLink();
                            eventHeader.CssClass = "CalendarHeader";
                            eventHeader.Text = "There are no events at " + dat.BreakUpString(ds.Tables[0].Rows[0]["Name"].ToString(), 14) + " today.";
                            EventsPanel.Controls.Add(eventHeader);
                        }



                        //Media Categories: NONE: 0, Picture: 1, Video: 2, YouTubeVideo: 3, Slider: 4
                        string youtube = ds.Tables[0].Rows[0]["YouTubeVideo"].ToString();
                        Rotator1.Items.Clear();
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
                                            literal3.Text = "<div style=\"float:left;\"><object width=\"400\" height=\"250\"><param  name=\"wmode\" value=\"opaque\" ></param><param name=\"movie\" value=\"http://www.youtube.com/v/" + youtokens[i] +
                                                "\"></param><param name=\"allowFullScreen\" value=\"true\"></param><embed wmode=\"opaque\" src=\"http://www.youtube.com/v/" + youtokens[i] +
                                                "\" type=\"application/x-shockwave-flash\" allowfullscreen=\"true\" width=\"400\" height=\"250\"></embed></object></div>";
                                            Telerik.Web.UI.RadRotatorItem r3 = new Telerik.Web.UI.RadRotatorItem();
                                            r3.Controls.Add(literal3);
                                            Rotator1.Items.Add(r3);
                                        }
                                    }
                                }
                                DataView dsSlider = dat.GetDataDV("SELECT * FROM Venue_Slider_Mapping WHERE VenueID=" + ID);
                                if (dsSlider.Count > 0)
                                    {
                                        char[] delim = { '\\' };
                                        char[] delim3 = { '.' };
                                        string[] fileArray = System.IO.Directory.GetFiles(MapPath(".") + "\\VenueFiles\\" + ID + "\\Slider");

                                        string[] finalFileArray = new string[fileArray.Length];

                                        for (int i = 0; i < dsSlider.Count; i++)
                                        {
                                            int length = fileArray[i].Split(delim).Length;
                                            finalFileArray[i] = "http://" + Request.Url.Authority + "/HippoHappenings/VenueFiles/" +
                                                ID + "/Slider/" + dsSlider[i]["PictureName"].ToString();
                                            string[] tokens = dsSlider[i]["PictureName"].ToString().Split(delim3);

                                            //dsSlider.RowFilter = "RealPictureName='" + tokens[0] + "." + tokens[1] + "'";
                                            if (tokens.Length >= 2)
                                            {
                                                if (tokens[1].ToUpper() == "JPG" || tokens[1].ToUpper() == "JPEG" || tokens[1].ToUpper() == "GIF" || tokens[1].ToUpper() == "PNG")
                                                {
                                                    System.Drawing.Image image = System.Drawing.Image.FromFile(MapPath(".") +
                                                        "\\VenueFiles\\" + ID + "\\Slider\\" + dsSlider[i]["PictureName"].ToString());


                                                    int width = 400;
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
                                                    literal4.Text = "<div style=\"width: 400px; height: 250px;background-color: black;\"><img align=\"middle\" style=\"cursor: pointer; margin-left: " + ((400 - newIntWidth) / 2).ToString() + "px; margin-top: " + ((250 - newHeight) / 2).ToString() + "px;\" height=\"" + newHeight + "px\" width=\"" + newIntWidth + "px\" onclick=\"OpenEventModal(" + i.ToString() + ", " + ID + ");\" height=\"" + height.ToString() + "px\" width=\"" + width.ToString() + "px\" src=\""
                                                        + "VenueFiles/" + ID + "/Slider/" + dsSlider[i]["PictureName"].ToString()+ "\" /></div>";
                                                    Telerik.Web.UI.RadRotatorItem r4 = new Telerik.Web.UI.RadRotatorItem();
                                                    r4.Controls.Add(literal4);
                                                    Rotator1.Items.Add(r4);
                                                }
                                                else if (tokens[1].ToUpper() == "WMV")
                                                {
                                                    Literal literal4 = new Literal();
                                                    literal4.Text = "<div style=\"float:left;\"><OBJECT wmode=\"transparent\"  classid='clsid:02BF25D5-8C17-4B23-BC80-D3488ABDDC6B' " +
                                                        "width=\"400\" height=\"250\" codebase='http://www.apple.com/qtactivex/qtplugin.cab'>" +
                                                        "<param name='src' value=\"VenueFiles/" + ID + "/Slider/" + fileArray[i].Split(delim)[length - 1].ToString() + "\">" +
                                                        "<param name='autoplay' value=\"false\"><param name=\"wmode\" value=\"transparent\"/>" +
                                                        "<param name='controller' value=\"true\">" +
                                                        "<param name='loop' value=\"false\"><param  name=\"wmode2\" value=\"opaque\" ></param>" +
                                                        "<EMBED wmode=\"transparent\" wmode2=\"opaque\" src=\"VenueFiles/" + ID + "/Slider/" + fileArray[i].Split(delim)[length - 1].ToString() + "\" width=\"400\" height=\"250\" autoplay=\"false\" " +
                                                        "controller=\"true\" loop=\"false\" bgcolor=\"#000000\" pluginspage='http://www.apple.com/quicktime/download/'>" +
                                                        "</EMBED>" +
                                                        "</OBJECT></div> ";

                                                    //literal4.Text = "<div style=\"float:left;\"><embed  height=\"250px\" width=\"400px\" src=\""
                                                    //    + "VenueFiles/" + ID + "/Slider/" + fileArray[i].Split(delim)[length - 1].ToString() + "\" /></div>";
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
                            ASP.controls_addtofavorites_ascx AddTo1 = new ASP.controls_addtofavorites_ascx();
                            AddTo1.VENUE_ID = int.Parse(ID);

                            ASP.controls_sendmessage_ascx SendMessage1 = new ASP.controls_sendmessage_ascx();
                            SendMessage1.THE_TEXT = "Share this with a friend";
                            SendMessage1.RE_LABEL = "Re: " + Session["UserName"].ToString() +
                                " would like to share the venue '\"" + dat.BreakUpString(ds.Tables[0].Rows[0]["Name"].ToString(), 14) + "\"' with you.";
                            SendMessage1.TYPE = "v";
                            SendMessage1.ID = int.Parse(ID);

                            CalendarPanel.Controls.Add(AddTo1);
                            CalendarPanel.Controls.Add(SendMessage1);
                        }


                        this.Title = dat.BreakUpString(ds.Tables[0].Rows[0]["Name"].ToString(), 14);
                    }
                    else
                    {
                        Response.Redirect("~/Home.aspx");
                    }
                }
                else
                {
                    Response.Redirect("~/Home.aspx");
                }
            }
            else
            {
                Response.Redirect("~/Home.aspx");
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
                        TopPanel.Width = 280;
                    }
                    else
                    {
                        TopPanel.Width = 80;
                        
                    }
                    EditLink.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Edit Venue";
                }
                else
                {
                    if (OwnerPanel.Visible)
                    {
                        TopPanel.Width = 60;
                    }
                    else
                    {
                        TopPanel.Width = 0;
                    }
                    HyperLink1.InnerHtml = "&nbsp;&nbsp;&nbsp;&nbsp;Venue Ownership is Open";
                }
            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString();
        }
    }


    protected void Page_Init(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        //Button button = (Button)dat.FindControlRecursive(this, "VenuesLink");
        //button.CssClass = "NavBarImageVenueSelected";
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

    protected void GoToEdit(object sender, EventArgs e)
    {
        string eventID = Request.QueryString["ID"].ToString();
        HttpCookie cookie = new HttpCookie("venueEvent" + eventID);
        cookie.Value = "True";
        cookie.Expires = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(1);
        Response.Cookies.Add(cookie);

        Response.Redirect("EnterVenue.aspx?edit=true&ID=" + eventID);
    }
}
