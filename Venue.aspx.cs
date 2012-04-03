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
            }
            catch (Exception ex)
            {
            }

            GetOtherVenues();
            DoHours(false);
            DoHours(true);

            if (Request.QueryString["ID"] == null)
                Response.Redirect("~/home");
            string ID = Request.QueryString["ID"].ToString();

            TagCloud.THE_ID = int.Parse(ID);

            if (Session["User"] != null)
            {
                LoggedInPanel.Visible = true;
                LoggedOutPanel.Visible = false;
                DataSet dsComments;
                string commentPrefs = dat.GetData("SELECT * FROM UserPreferences WHERE UserID=" + Session["User"].ToString()).Tables[0].Rows[0]["CommentsPreferences"].ToString();
                if (commentPrefs == "1")
                {
                    dsComments = dat.GetData("SELECT VC.CommentDate AS theDate, * FROM Venue_Comments VC, Users U WHERE VC.UserID=U.User_ID AND VC.ID=" + ID + " ORDER BY VC.CommentDate ");
                }
                else
                {
                    dsComments = dat.GetData("SELECT DISTINCT U.ProfilePicture, U.User_ID, U.UserName, VC.Comment, VC.CommentDate AS theDate FROM Venue_Comments VC, Users U, User_Friends UF WHERE ((UF.UserID=" + Session["User"].ToString() + " AND UF.FriendID=U.User_ID AND U.User_ID=VC.UserID) OR (U.User_ID=" +
                                        Session["User"].ToString() + " AND U.User_ID=VC.UserID)) AND VC.ID=" + ID + " ORDER BY VC.CommentDate");
                }
                TheComments.DATA_SET = dsComments;
                TheComments.DataBind2(true);

               //Show edit link if use is logged in.
                EditLink.Visible = true;

                //Query whether current owner was delinquent on approve/reject changes
                //if (dat.IsOwnerDelinquent(Request.QueryString["ID"].ToString(), Request.IsLocal, "V"))
                //{
                //    //make the button visable
                //    OwnerPanel.Visible = true;
                //    Session["Message"] = "The ownership of this venue is <b>open</b>. <br/>The ownership became " +
                //        "open because the previous owner of this venue became un-responsive to rejecting/" +
                //        "approving user's changes to this venue.<br/>If you would like to become " +
                //        "the owner, click on the button below to go to the venue's edit page. <br/><br/> " +
                //        "Being the owner, you will have the privilage of having your <b>edits come though right " +
                //        "away.</b> Other participants' changes to this venue will have to be <b>approved by you.</b><br/><br/>" +
                //        "<div align=\"center\" style=\"padding-left: 110px;\"><div align=\"center\"><div style=\"cursor: pointer;cursor: pointer; float: left;padding-right: 10px;\">" +
                //            "<div class=\"topDiv\" style=\"float:left;\">" +
                //            "<img style=\"float: left;\" src=\"http://hippohappenings.com/NewImages/ButtonLeft.png\" height=\"27px\" />" +
                //            "<div style=\"font-size: 12px; text-decoration: none; padding-top: 5px;padding-left: 6px; padding-right: 6px;height: 27px;float: left;background: url('http://hippohappenings.com/NewImages/ButtonPixel.png'); background-repeat: repeat-x;\">" +
                //            "<a class=\"NavyLink\" onclick=\"Search('enter-locale?ID=" + Request.QueryString["ID"] +
                //            "');\">Edit</a></div>" +
                //            "<img style=\"float: left;\" src=\"http://hippohappenings.com/NewImages/ButtonRight.png\" height=\"27px\" />" +
                //            "</div>" +
                //            "</div>" +
                //            "</div>" +
                //            "<div align=\"center\"><div style=\"cursor: pointer;\">" +
                //            "<div class=\"topDiv\" style=\"float:left;\">" +
                //            "<img style=\"float: left;\" src=\"http://hippohappenings.com/NewImages/ButtonLeft.png\" height=\"27px\" />" +
                //            "<div style=\"font-size: 12px; text-decoration: none; padding-top: 5px;padding-left: 6px; padding-right: 6px;height: 27px;float: left;background: url('http://hippohappenings.com/NewImages/ButtonPixel.png'); background-repeat: repeat-x;\">" +
                //            "<a class=\"NavyLink\" onclick=\"Search();\">Close</a></div>" +
                //            "<img style=\"float: left;\" src=\"http://hippohappenings.com/NewImages/ButtonRight.png\" height=\"27px\" />" +
                //            "</div>" +
                //            "</div>" +
                //            "</div></div>";
                //}


                DataSet ds2 = dat.GetData("SELECT * FROM Venues WHERE ID=" + ID);

                DataView dv = new DataView(ds2.Tables[0], "", "", DataViewRowState.CurrentRows);

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
            }
            else
            {
                LoggedInPanel.Visible = false;
                LoggedOutPanel.Visible = true;
                DataSet dsComments = dat.GetData("SELECT VC.CommentDate AS theDate, * FROM Venue_Comments VC, Users U WHERE VC.UserID=U.User_ID AND VC.ID=" + ID + " ORDER BY VC.CommentDate ");
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

                        #region SEO
                        topTopLiteral.Text = "<a class=\"NavyLink12UD\" href=\"#" + dat.MakeNiceNameFull(ds.Tables[0].Rows[0]["Name"].ToString()) + "\">" +
                        dat.MakeNiceNameFull(ds.Tables[0].Rows[0]["Name"].ToString()).Replace("-", " ") + " From The Top</a>";

                        string theLink = "http://" + Request.Url.Authority + "/" +
                            dat.MakeNiceName(ds.Tables[0].Rows[0]["Name"].ToString()) +
                            "_" + ID.ToString() + "_Venue";

                        HtmlHead head = (HtmlHead)Page.Header;
                        HtmlLink lk = new HtmlLink();
                        lk.Href = theLink;
                        lk.Attributes.Add("rel", "bookmark");
                        head.Controls.AddAt(0, lk);

                        //Create keyword and description meta tags
                        HtmlMeta hm = new HtmlMeta();
                        HtmlMeta kw = new HtmlMeta();
                        HtmlMeta lg = new HtmlMeta();
                        HtmlLink cn = new HtmlLink();

                        cn.Attributes.Add("rel", "canonical");
                        cn.Href = theLink;
                        head.Controls.AddAt(0, cn);

                        kw.Name = "keywords";
                        hm.Name = "Description";
                        lg.Name = "language";
                        lg.Content = "English";
                        head.Controls.AddAt(0, lg);
                        char[] delimeter = { ' ' };

                        string[] keywords = dat.MakeNiceNameFull(ds.Tables[0].Rows[0]["Name"].ToString()).Replace("-", " ").Split(delimeter, StringSplitOptions.RemoveEmptyEntries);
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

                        hm.Content = dat.MakeNiceNameFull(dat.stripHTML(ds.Tables[0].Rows[0]["Content"].ToString()).Replace("   ", " ").Replace("  ", " ")).Replace("-", " ");
                        if (hm.Content.Length > 200)
                            hm.Content = hm.Content.Substring(0, 197) + "...";

                        head.Controls.AddAt(0, hm);

                        this.Title = kw.Content;

                        #endregion

                        VenueName.Text = "<a id=\"" + dat.MakeNiceNameFull(ds.Tables[0].Rows[0]["Name"].ToString()) + "\" class=\"aboutLink\" href=\"" +
                            theLink + "\"><h1>" + dat.BreakUpString(ds.Tables[0].Rows[0]["Name"].ToString(), 50) + "</h1></a>";
                        Session["Subject"] = "Re: " + ds.Tables[0].Rows[0]["Name"].ToString();
                        Session["CommentSubject"] = "Re: " + ds.Tables[0].Rows[0]["Name"].ToString();

                        string Venue = dat.BreakUpString(ds.Tables[0].Rows[0]["Name"].ToString(), 14);

                        string url = ds.Tables[0].Rows[0]["Web"].ToString();
                        if (url.Length > 8)
                        {
                            if (url.ToLower().Substring(0, 7) != "http://" && url.ToLower().Substring(0, 8) != "https://")
                                url = "http://" + url;
                        }

                        PhoneLabel.Text = "Phone: " + dat.BreakUpString(ds.Tables[0].Rows[0]["Phone"].ToString(), 15) + "<br/>Email: " +
                            dat.BreakUpString(ds.Tables[0].Rows[0]["Email"].ToString(), 15) + "<br/>Web: <a class='AddLink' target='_blank' href='" + url + "'>" +
                            dat.BreakUpString(ds.Tables[0].Rows[0]["Web"].ToString(), 17) + "</a>";

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
                                    eventHeader.CssClass = "NavyLink16UD";
                                    eventHeader.NavigateUrl = dat.MakeNiceName(dsEvents.Tables[0].Rows[i]["Header"].ToString())+"_" + dsEvents.Tables[0].Rows[i]["ID"].ToString()+"_Event";
                                    eventHeader.Text = dsEvents.Tables[0].Rows[i]["Header"].ToString() + "<br/>";

                                    Label dateStart = new Label();
                                    dateStart.CssClass = "Text12UDPd";
                                    dateStart.Text = DateTime.Parse(dsEvents.Tables[0].Rows[i]["Start"].ToString()).ToShortTimeString() + " - " +
                                        DateTime.Parse(dsEvents.Tables[0].Rows[i]["End"].ToString()).ToShortTimeString() + "<br/>";

                                    Label shortDescription = new Label();
                                    shortDescription.CssClass = "Text12Pd";
                                    shortDescription.Text = dsEvents.Tables[0].Rows[i]["ShortDescription"].ToString() + "<br/>";

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
                            Label eventHeader = new Label();
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
                                char[] delim4 = { ';' };
                                string[] youtokens = youtube.Split(delim4);
                                if (youtube != "")
                                {
                                    for (int i = 0; i < youtokens.Length; i++)
                                    {
                                        if (youtokens[i].Trim() != "")
                                        {
                                            Literal literal3 = new Literal();
                                            literal3.Text = "<div class=\"FloatLeft\"><object width=\"412\" height=\"250\"><param  name=\"wmode\" value=\"opaque\" ></param><param name=\"movie\" value=\"http://www.youtube.com/v/" + youtokens[i] +
                                                "\"></param><param name=\"allowFullScreen\" value=\"true\"></param><embed wmode=\"opaque\" src=\"http://www.youtube.com/v/" + youtokens[i] +
                                                "\" type=\"application/x-shockwave-flash\" allowfullscreen=\"true\" width=\"412\" height=\"250\"></embed></object></div>";
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

                                        for (int i = 0; i < dsSlider.Count; i++)
                                        {
                                            string[] tokens = dsSlider[i]["PictureName"].ToString().Split(delim3);

                                            //dsSlider.RowFilter = "RealPictureName='" + tokens[0] + "." + tokens[1] + "'";
                                            if (tokens.Length >= 2)
                                            {
                                                if (tokens[1].ToUpper() == "JPG" || tokens[1].ToUpper() == "JPEG" || tokens[1].ToUpper() == "GIF" || tokens[1].ToUpper() == "PNG")
                                                {
                                                    try
                                                    {
                                                        System.Drawing.Image image = System.Drawing.Image.FromFile(MapPath(".") +
                                                            "\\VenueFiles\\" + ID + "\\Slider\\" + dsSlider[i]["PictureName"].ToString());

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
                                                        + "VenueFiles/" + ID + "/Slider/" + dsSlider[i]["PictureName"].ToString() + "\" /></div>";
                                                        Telerik.Web.UI.RadRotatorItem r4 = new Telerik.Web.UI.RadRotatorItem();
                                                        r4.Controls.Add(literal4);
                                                        Rotator1.Items.Add(r4);
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                }
                                                else if (tokens[1].ToUpper() == "WMV")
                                                {
                                                    Literal literal4 = new Literal();
                                                    literal4.Text = "<div style=\"width: 410px; height: 250px;\" ><OBJECT stop=\"true\" loop=\"false\" controller=\"true\" wmode2=\"opaque\" wmode=\"transparent\" autoplay=\"false\" classid=\"clsid:02BF25D5-8C17-4B23-BC80-D3488ABDDC6B\" " +
                                                        "width=\"410\" height=\"250\" codebase=\"http://www.apple.com/qtactivex/qtplugin.cab\">" +
                                                        "<param name=\"src\" value=\"UserFiles/Events/" + ID + "/Slider/" + dsSlider[i]["PictureName"].ToString() + "\"></param>" +
                                                        "<param name=\"autoplay\" value=\"false\"></param><param name=\"wmode\" value=\"transparent\"></param>" +
                                                        "<param name=\"controller\" value=\"true\"></param>" +
                                                        "<param name=\"stop\" value=\"true\" ></param>" +
                                                        "<param name=\"loop\" value=\"false\"><param  name=\"wmode2\" value=\"opaque\" ></param>" +
                                                        "<EMBED stop=\"true\" wmode=\"transparent\" wmode2=\"opaque\" src=\"UserFiles/Events/" + ID + "/Slider/" + dsSlider[i]["PictureName"].ToString() + "\" width=\"410\" height=\"250\" autoplay=\"false\" " +
                                                        "controller=\"true\" loop=\"false\" bgcolor=\"#000000\" pluginspage=\"http://www.apple.com/quicktime/download/\">" +
                                                        "</EMBED>" +
                                                        "</OBJECT></div>";

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
                            SendMessage1.THE_TEXT = "Share on Hippo";
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
                        Response.Redirect("~/home");
                    }
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

        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString();
        }
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

    protected void GetOtherVenues()
    {
        try
        {
            string eventID = Request.QueryString["ID"];
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

            DataView dvEvent = dat.GetDataDV("SELECT * FROM Venues WHERE ID = " + eventID);

            string country = dvEvent[0]["Country"].ToString();
            string state = dvEvent[0]["State"].ToString();
            string city = dvEvent[0]["City"].ToString();

            DataView dvEventCategories = dat.GetDataDV("SELECT * FROM Venues V, Venue_Category VCM WHERE V.ID=VCM.ID AND V.ID=" + eventID);

            string similarQuery = "SELECT DISTINCT CASE WHEN V.DaysFeatured IS NULL THEN 'False' WHEN V.DaysFeatured LIKE '%" +
                isNow.Date.ToShortDateString() + "%' THEN 'True' ELSE 'False' END AS Featured, V.PostedOn, V.ID AS VenueID, V.Name, V.Content FROM Venues V, Venue_Category VCM WHERE " +
                " V.ID=VCM.VENUE_ID AND V.LIVE='True' AND V.Country=" + country +
                " AND V.State='" + state + "' AND V.City='" + city + "' AND V.ID <> " + eventID;

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
                    similarQuery += " VCM.CATEGORY_ID=" + row["CATEGORY_ID"].ToString();
                    gotfirst = true;
                }
                similarQuery += " ) ";
            }
            DataView dvSimilar = dat.GetDataDV(similarQuery + " ORDER BY Featured DESC, PostedOn ASC ");

            Literal lit = new Literal();

            DataSet dsLatsLongs = dat.GetData("SELECT Latitude, Longitude FROM ZipCodes WHERE Zip='" +
                                        dvEvent[0]["Zip"].ToString() + "'");

            //some zip codes don't exist in the database, find the closest one
            bool findClosest = false;
            int zipParam = int.Parse(dvEvent[0]["Zip"].ToString());
            if (dsLatsLongs.Tables.Count > 0)
            {
                if (dsLatsLongs.Tables[0].Rows.Count > 0)
                {

                }
                else
                {
                    findClosest = true;
                }
            }
            else
            {
                findClosest = true;
            }

            if (findClosest)
            {
                dsLatsLongs = null;
                
                while (dsLatsLongs == null)
                {
                    if (zipParam > 99999)
                        zipParam = 10000;
                    dsLatsLongs = dat.GetData("SELECT Latitude, Longitude FROM ZipCodes WHERE Zip='" + zipParam++ + "'");
                    if (dsLatsLongs.Tables.Count > 0)
                    {
                        if (dsLatsLongs.Tables[0].Rows.Count > 0)
                        {

                        }
                        else
                        {
                            dsLatsLongs = null;
                        }
                    }
                    else
                    {
                        dsLatsLongs = null;
                    }
                }
            }

            DataSet dsZips = dat.GetData("SELECT Zip FROM ZipCodes WHERE dbo.GetDistance(" + dsLatsLongs.Tables[0].Rows[0]["Longitude"].ToString() +
                ", " + dsLatsLongs.Tables[0].Rows[0]["Latitude"].ToString() + ", Longitude, Latitude) < 5");
            //some zip codes don't exist in the database, find the closest one
            string zip = "";
            string nonExistantZip = " OR V.Zip = '" + dvEvent[0]["Zip"].ToString() + "'";


            if (dsZips.Tables.Count > 0)
            {
                if (dsZips.Tables[0].Rows.Count > 0)
                {
                    zip = " AND (V.Zip = '" + dvEvent[0]["Zip"].ToString() + "' " + nonExistantZip;
                    for (int i = 0; i < dsZips.Tables[0].Rows.Count; i++)
                    {
                        zip += " OR V.Zip = '" + dsZips.Tables[0].Rows[i]["Zip"].ToString() + "' ";
                    }
                    zip += ") ";
                }
                else
                {
                    zip = " AND V.Zip='" + dvEvent[0]["Zip"].ToString() + "'";
                }
            }
            else
            {
                zip = " AND V.Zip='" + dvEvent[0]["Zip"].ToString() + "'";
            }

            string andNot = "";
            int similarCount = 0;
            foreach (DataRowView row in dvSimilar)
            {
                andNot += " AND V.ID <> " + row["VenueID"].ToString();
                similarCount++;
                if (similarCount == 6)
                    break;
            }

            string thisDayQuery = "SELECT DISTINCT CASE WHEN V.DaysFeatured IS NULL THEN 'False' WHEN V.DaysFeatured LIKE '%" +
            isNow.Date.ToShortDateString() + "%' THEN 'True' ELSE 'False' END AS Featured, V.PostedOn, V.ID AS VenueID, V.Name, V.Content FROM Venues V, " +
                "Venue_Category VCM WHERE " +
                " V.ID=VCM.VENUE_ID AND V.LIVE='True' AND V.Country=" + country +
                " AND V.ID <> " + eventID + zip + andNot + " ORDER BY Featured DESC, PostedOn ASC ";
       
            DataView dvDay = dat.GetDataDV(thisDayQuery);

            if (dvSimilar.Count > 0)
            {
                lit = new Literal();
                lit.Text = "<a id=\"Similar-Locales\" class=\"aboutLink\"><h1 class=\"SideColumn\">Similar Locales</h1><div class=\"Text12\" class=\"SimilarSide\"></a>";

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

            if (dvDay.Count > 0)
            {
                lit = new Literal();
                lit.Text = "<a id=\"Close-Locales\" class=\"aboutLink\"><h1 class=\"SideColumn\">Close Locales</h1><div class=\"Text12\" class=\"SimilarSide\"></a>";

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

        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString();
        }
    }

    protected void DrawEvents(DataView dvEvents, int cutOff)
    {
        string eventID = Request.QueryString["ID"];
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
                dvEvent = dat.GetDataDV("SELECT * FROM Venues V WHERE V.ID=" + row["VenueID"].ToString());
                lit.Text += "<div class=\"SimilarSide\">";
                lit.Text += "<a class=\"Green12LinkNF\" href=\"" + dat.MakeNiceName(row["Name"].ToString()) +
                    "_" + row["VenueID"].ToString() + "_Venue\">" + row["Name"].ToString() +
                    "</a> " + dat.BreakUpString(contentSub, 30) +
                    "... <a class=\"Blue12Link\" href=\"" + dat.MakeNiceName(row["Name"].ToString()) +
                    "_" + row["VenueID"].ToString() + "_Venue\">Read More</a>";
                lit.Text += "</div>";
                count++;
            }
        }

        OtherEventsPanel.Controls.Add(lit);
    }

    protected void DoHours(bool isEvent)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }

        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        DataView dv;
        if (isEvent)
        {
            dv = dat.GetDataDV("SELECT * FROM VenueEvents WHERE VenueID=" + Request.QueryString["ID"]);
        }
        else
        {
            dv = dat.GetDataDV("SELECT * FROM VenueHours WHERE VenueID=" + Request.QueryString["ID"]);
            HoursLabel.Text = "<h2>Hours</h2>";
        }

        char[] delim = { ';' };
        string[] tokens;
        int firstToken = 0;
        int secondToken = 0;
        int tokenCount = 0;
        string hoursText = "";
        string hoursTexts = "";
        bool isDash = false;
        bool allow = false;
        if (dv.Count > 0)
        {
            foreach (DataRowView row in dv)
            {
                if (isEvent)
                {
                    HoursLabel.Text += "<h2>" + row["EventName"].ToString() + "</h2>";
                }

                string starTime = TimeSpan.Parse(row["HourStart"].ToString()).Hours + ":" + TimeSpan.Parse(row["HourStart"].ToString()).Minutes.ToString();
                if (starTime.Substring(starTime.Length - 2, 2) == ":0")
                    starTime = starTime.Replace(":0", ":00");

                string endTime = TimeSpan.Parse(row["HourEnd"].ToString()).Hours + ":" + TimeSpan.Parse(row["HourEnd"].ToString()).Minutes.ToString();
                if (endTime.Substring(endTime.Length - 2, 2) == ":0")
                    endTime = endTime.Replace(":0", ":00");

                string ab = dat.GetHours(row["Days"].ToString());

                if (isEvent)
                {
                    HoursLabel.Text += ab.Substring(5, ab.Length - 5) + starTime +
                        " - " + endTime;
                }
                else
                {
                    HoursLabel.Text += "<h2>" + ab.Substring(5, ab.Length - 5).Replace("<br/>",
                        "</h2>") + starTime + " - " + endTime;
                }
            }
        }
    }

    protected void GoToEdit(object sender, EventArgs e)
    {
        try
        {
            string eventID = Request.QueryString["ID"].ToString();
            HttpCookie cookie = new HttpCookie("venueEvent" + eventID);
            cookie.Value = "True";
            cookie.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(cookie);

            Response.Redirect("enter-locale?edit=true&ID=" + eventID);
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString();
        }
    }
}
