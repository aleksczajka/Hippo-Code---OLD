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
using System.Data.SqlClient;
using System.Diagnostics;

public partial class Home : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Page.Trace.IsEnabled = true;
        //Page.Trace.TraceMode = TraceMode.SortByTime;

        try
        {
            HtmlMeta hm = new HtmlMeta();
            HtmlMeta kw = new HtmlMeta();

            HtmlHead head = (HtmlHead)Page.Header;

            hm.Name = "Description";
            hm.Content = "Find your local events, venues and classifieds all while " +
                     "ads from your peers, neighborhood, and community are displayed to you purely based on your interests." +
                     "Large corporations are not welcome!";
            head.Controls.AddAt(0, hm);


            HtmlMeta nMT = new HtmlMeta();
            nMT.Name = "google-site-verification";
            nMT.Content = "tw8rmOWW-DlZa-H4DZdGr201J5kC7NVLXUmk5oN8vFM";

            head.Controls.Add(nMT);
            
            HtmlLink lkl = new HtmlLink();
            lkl.Href = "ror.xml";
            lkl.Attributes.Add("rel", "alternate");
            lkl.Attributes.Add("type", "application/xml");
            lkl.Attributes.Add("title", "ROR");

            head.Controls.Add(lkl);

            HttpCookie cookie = Request.Cookies["BrowserDate"];
            if (cookie == null)
            {
                cookie = new HttpCookie("BrowserDate");
                cookie.Value = DateTime.Now.Date.ToString();
                cookie.Expires = DateTime.Now.AddDays(22);
                Response.Cookies.Add(cookie);
            }
            bool fillUserData = false;
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":").Replace("%28", "(").Replace("%29", ")")));

            kw.Name = "keywords";
            kw.Content = "events, ads, venues, post, search, find, local events, concerts, festivals, world, theatre, technology, " +
                "family, peers, neighborhood, classifieds";

            DataView dvCats = dat.GetDataDV("SELECT * FROM AdCategories");

            for (int i = 0; i < dvCats.Count; i++)
            {
                kw.Content += ", " + dvCats[i]["Name"].ToString();
            }

            dvCats = dat.GetDataDV("SELECT * FROM EventCategories");

            for (int i = 0; i < dvCats.Count; i++)
            {
                kw.Content += ", " + dvCats[i]["Name"].ToString();
            }


            dvCats = dat.GetDataDV("SELECT * FROM VenueCategories");

            for (int i = 0; i < dvCats.Count; i++)
            {
                kw.Content += ", " + dvCats[i]["Name"].ToString();
            }

            head.Controls.AddAt(0, kw);
            
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Context.Request.Cookies[cookieName];

            
                //logInPanel.Visible = Session["User"] == null;
                //loggedInPanel.Visible = Session["User"] != null;
           

            string country = "";
            string state = "";
            string city = "";
            string countryID = "";
            string stateID = "";
            string cityID = "";
            FormsAuthenticationTicket authTicket = null;
            try
            {
                string group = "";
                if (authCookie != null)
                {
                    authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    group = authTicket.UserData.ToString();
                }


                if (group.Contains("User"))
                {
                    Session["User"] = authTicket.Name;
                    DataSet ds1 = dat.GetData("SELECT * FROM Users U, UserPreferences UP WHERE U.User_ID=" +
                        authTicket.Name + " AND U.User_ID=UP.UserID ");
                    Session["UserName"] = ds1.Tables[0].Rows[0]["UserName"].ToString();

                    country = ds1.Tables[0].Rows[0]["CatCountry"].ToString();
                    countryID = country;

                    state = ds1.Tables[0].Rows[0]["CatState"].ToString();
                    city = ds1.Tables[0].Rows[0]["CatCity"].ToString();
                    stateID = state;
                    cityID = city;

                    //DataSet ds2 = dat.RetrieveAds(Session["User"].ToString(), false);
                    //DataSet dsMain = dat.RetrieveMainAds(Session["User"].ToString());   
                    //Ads1.DATA_SET = ds2;
                    //Ads1.MAIN_AD_DATA_SET = dsMain;

                    fillUserData = true;
                }
                else
                {
                    DataSet ds1 = dat.GetData("SELECT * FROM SearchIPs WHERE IP='" + dat.GetIP() + "'");

                    bool getAnotherDs1 = false;
                    if (ds1.Tables.Count > 0)
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            country = ds1.Tables[0].Rows[0]["Country"].ToString();
                            countryID = country;
                            state = ds1.Tables[0].Rows[0]["State"].ToString();
                            city = ds1.Tables[0].Rows[0]["City"].ToString();
                            stateID = state;
                            cityID = city;
                        }
                        else
                        {
                            getAnotherDs1 = true;
                        }
                    else
                    {
                        getAnotherDs1 = true;
                    }

                    if (getAnotherDs1)
                    {
                        ds1 = dat.GetData("SELECT * FROM Users U, UserPreferences UP WHERE " +
                            " U.User_ID=UP.UserID AND U.IPs LIKE '%" + dat.GetIP() + "%'");
                        if (ds1.Tables.Count > 0)
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                country = ds1.Tables[0].Rows[0]["CatCountry"].ToString();
                                countryID = country;
                                state = ds1.Tables[0].Rows[0]["CatState"].ToString();
                                city = ds1.Tables[0].Rows[0]["CatCity"].ToString();
                                stateID = state;
                                cityID = city;
                            }
                    }

                    //if (!IsPostBack)
                    //{
                    //    Ads1.DATA_SET = dat.RetrieveAllAds(false);
                    //    Ads1.MAIN_AD_DATA_SET = dat.RetrieveAllAds(true);
                    //}
                    Button calendarLink = (Button)dat.FindControlRecursive(this, "CalendarLink");
                    calendarLink.Visible = false;

                }
            }
            catch (Exception ex)
            {
                
            }

            DataSet ds;

            if (!IsPostBack)
            {
                LocationLabel.Text = "";
                if (country != "")
                    country = " AND E.Country = " + country;

                int c = 0;

                if (state != "")
                {

                    c++;
                }

                if (city != "")
                {

                    c++;
                }

                SqlDbType[] types = new SqlDbType[c];
                object[] data = new object[c];

                if (state != "")
                {
                    types[0] = SqlDbType.NVarChar;
                    data[0] = state;
                    state = " AND E.State=@p0 ";
                    if (city != "")
                    {
                        types[1] = SqlDbType.NVarChar;
                        data[1] = city;
                        city = " AND E.City=@p1 ";
                    }
                }
                else
                {
                    if (city != "")
                    {
                        types[0] = SqlDbType.NVarChar;
                        data[0] = city;
                        city = " AND E.City=@p0 ";
                    }
                }





                ds = dat.GetDataWithParemeters("SELECT DISTINCT TOP 10 EO.DateTimeStart, E.Header, E.Content, EO.EventID FROM Events E, Event_Occurance EO WHERE E.ID=EO.EventID " + country + state + city + " AND EO.DateTimeStart > '" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Date + "'", types, data);



                Session["HomeEvents"] = ds;
                LocationLabel.Text = "";
                if (country == "" && state == "" && city == "")
                {
                    LocationLabel.Text = " The World ";
                }
                else
                {
                    if (city != "")
                    {
                        LocationLabel.Text += cityID;
                        if (state != "")
                            LocationLabel.Text += ", " + stateID;
                    }
                    else
                    {
                        if (state != "")
                            LocationLabel.Text += stateID;
                    }
                }

                if (LocationLabel.Text == "")
                {
                    DataSet dsCountry = dat.GetData("SELECT * FROM Countries WHERE country_id=" + countryID);
                    LocationLabel.Text = dsCountry.Tables[0].Rows[0]["country_name"].ToString();
                }
            }
            else
            {
                EventPanel.Controls.Clear();
                LocationLabel.Text = "";
                ds = (DataSet)Session["HomeEvents"];

                if (countryID == "" && stateID == "" && cityID == "")
                {
                    LocationLabel.Text = " The World ";
                }
                else
                {
                    if (cityID != "")
                    {
                        LocationLabel.Text += cityID;
                        if (state != "")
                            LocationLabel.Text += ", " + stateID;
                    }
                    else
                    {
                        if (state != "")
                            LocationLabel.Text += stateID;
                    }
                }

                if (RadCalendar1.SelectedDate.ToShortDateString() != "1/1/0001")
                    LocationLabel.Text += " on " + RadCalendar1.SelectedDate.ToShortDateString();

            }

            ds = (DataSet)Session["HomeEvents"];

            if (ds != null)
            {

                if (ds.Tables.Count > 0)
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            DateTime date = DateTime.Parse(ds.Tables[0].Rows[i]["DateTimeStart"].ToString());
                            ASP.controls_homeevent_ascx eventH = new ASP.controls_homeevent_ascx();
                            eventH.DAY = date.DayOfWeek.ToString().Substring(0, 3);
                            eventH.DAY_NUMBER = date.Day.ToString();
                            eventH.MONTH = dat.GetMonth(date.Month.ToString()).Substring(0, 3);
                            eventH.EVENT_NAME = ds.Tables[0].Rows[i]["Header"].ToString();
                            if (ds.Tables[0].Rows[i]["Content"].ToString().Length > 150)
                                eventH.SUMMARY = dat.BreakUpString(ds.Tables[0].Rows[i]["Content"].ToString().Substring(0, 150), 67) + "...";
                            else
                                eventH.SUMMARY = dat.BreakUpString(ds.Tables[0].Rows[i]["Content"].ToString(), 67) + "...";
                            eventH.EVENT_ID = int.Parse(ds.Tables[0].Rows[i]["EventID"].ToString());
                            EventPanel.Controls.Add(eventH);
                        }

                    }
                    else
                    {

                        Label label = new Label();
                        label.CssClass = "EventBody";
                        label.Text = "No events are listed for this date. Sorry. But perhaps you could enter one... <a class=\"AddLink\" href=\"BlogEvent.aspx\">Enter Events</a>.";
                        EventPanel.Controls.Add(label);
                    }
                else
                {

                    Label label = new Label();
                    label.CssClass = "EventBody";
                    label.Text = "No events are listed for this date. Sorry. But perhaps you could enter one... <a class=\"AddLink\" href=\"BlogEvent.aspx\">Enter Events</a>.";
                    EventPanel.Controls.Add(label);
                }
            }
            else
            {

                Label label = new Label();
                label.CssClass = "EventBody";
                label.Text = "No events are listed for this date. Sorry. But perhaps you could enter one... <a class=\"AddLink\" href=\"BlogEvent.aspx\">Enter Events</a>.";
                EventPanel.Controls.Add(label);
            }

            #region Original Home page code
            //if (ds.Tables.Count > 0)
            //{
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {

            //        ASP.controls_footer_ascx thefooter = (ASP.controls_footer_ascx)dat.FindControlRecursive(this, "TheFooter");
            //        thefooter.EVENT_ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());

            //        string ID = ds.Tables[0].Rows[0]["ID"].ToString();
            //        DataSet dsDate = dat.GetData("SELECT * FROM Event_Occurance WHERE EventID=" + ID);
            //        DataSet dsVenue = dat.GetData("SELECT * FROM Venues WHERE ID=" + ds.Tables[0].Rows[0]["Venue"]);
            //        TagCloud.THE_ID = int.Parse(ID);


            //        DataSet dsComments = dat.GetData("SELECT C.BlogDate AS theDate, * FROM Comments C, Users U WHERE U.User_ID=C.UserID AND C.BlogID=" + ID +" ORDER BY C.BlogDate");
            //        TheComments.DATA_SET = dsComments;
            //        TheComments.DataBind2(true);

            //        if (bool.Parse(ds.Tables[0].Rows[0]["hasSongs"].ToString()))
            //        {
            //            DataSet dsSongs = dat.GetData("SELECT * FROM Event_Song_Mapping WHERE EventID=" + ID);
            //            ASP.controls_songplayer_ascx songs = new ASP.controls_songplayer_ascx();
            //            int songCount = dsSongs.Tables[0].Rows.Count;

            //            if (songCount > 2)
            //            {
            //                songs.SONG1 = dsSongs.Tables[0].Rows[0]["SongName"].ToString();
            //                songs.SONG2 = dsSongs.Tables[0].Rows[1]["SongName"].ToString();
            //                songs.SONG3 = dsSongs.Tables[0].Rows[2]["SongName"].ToString();
            //            }
            //            else if (songCount > 1)
            //            {
            //                songs.SONG1 = dsSongs.Tables[0].Rows[0]["SongName"].ToString();
            //                songs.SONG2 = dsSongs.Tables[0].Rows[1]["SongName"].ToString();
            //            }
            //            else
            //                songs.SONG1 = dsSongs.Tables[0].Rows[0]["SongName"].ToString();


            //            songs.USER_NAME = ds.Tables[0].Rows[0]["UserName"].ToString();

            //            SongPanel.Controls.Add(songs);
            //        }



            //        EventName.Text = ds.Tables[0].Rows[0]["Header"].ToString();
            //        Session["Subject"] = "Re: "+ds.Tables[0].Rows[0]["Header"].ToString();
            //        Session["CommentSubject"] = "Re: " + ds.Tables[0].Rows[0]["Header"].ToString();
            //        EventName.NavigateUrl = "~/Event.aspx?EventID=" + ID;
            //        Session["EventID"] = ID;
            //        VenueName.Text = dsVenue.Tables[0].Rows[0]["Name"].ToString();
            //        VenueName.NavigateUrl = "Venue.aspx?ID="+dsVenue.Tables[0].Rows[0]["ID"].ToString();
            //        DateTime date = (DateTime)dsDate.Tables[0].Rows[0]["DateTimeStart"];
            //        DateAndTimeLabel.Text = date.DayOfWeek.ToString() + ", " + GetMonth(date.Month.ToString()) + " " + date.Day + " " + date.Hour + ":" + date.Minute;
            //        string content = ds.Tables[0].Rows[0]["Content"].ToString();
            //        SendTxtID.MESSAGE = EventName.Text + " occurs at " + VenueName.Text + " on " + DateAndTimeLabel.Text;

            //        string href = Request.Url.AbsoluteUri;
            //        SendEmailID.MESSAGE = "EventName: <a class=\"AddLink\" href=\"" + href + 
            //            "\">" + EventName.Text + "</a> \n\r Venue: " + VenueName.Text + 
            //            " \n\r Date: " + DateAndTimeLabel.Text + " \n\r " + content;

            //        //if (fillUserData)
            //        //{
            //        //    DataSet ds2 = dat.GetData("SELECT EEL.ExcitmentLevel AS Level FROM User_Calendar UC, Event_ExcitmentLevel EEL WHERE UC.UserID="
            //        //        + Session["User"].ToString() + " AND UC.EventID = " + ID + " AND UC.ExcitmentID=EEL.ID ");

            //        //    bool addEvent = false;

            //        //    if (ds2.Tables.Count > 0)
            //        //        if (ds2.Tables[0].Rows.Count > 0)
            //        //        {
            //        //            Label label = new Label();
            //        //            label.CssClass = "AddLinkGoing";
            //        //            label.Text = "Guess What?!: you're going to this event and you are " + ds2.Tables[0].Rows[0]["Level"].ToString();
            //        //            CalendarPanel.Controls.Add(label);
            //        //        }
            //        //        else
            //        //            addEvent = true;
            //        //    else
            //        //        addEvent = true;

            //        //    if (addEvent)
            //        //    {
            //        //        ASP.controls_addtocalendar_ascx AddTo1 = new ASP.controls_addtocalendar_ascx();
            //        //        AddTo1.ID = "AddTo1";
            //        //        AddTo1.TEXT = "Add this event to calendar";
            //        //        AddTo1.EVENT_ID = int.Parse(ID);
            //        //        CalendarPanel.Controls.Add(AddTo1);
            //        //    }

            //        //}

            //        if (content.Length > 500)
            //        {
            //            ShowDescriptionBegining.Text = content.Substring(0, 500);
            //            int j = 500;
            //            if (content[500] != ' ')
            //            {

            //                while (content[j] != ' ')
            //                {
            //                    ShowDescriptionBegining.Text += content[j];
            //                    j++;
            //                }
            //            }
            //            ShowDescriptionBegining.Text = dat.BreakUpString(ShowDescriptionBegining.Text, 65);
            //            ShowRestOfDescription.Text = dat.BreakUpString(content.Substring(j), 65);
            //        }
            //        else
            //        {
            //            ShowDescriptionBegining.Text = dat.BreakUpString(content, 65);
            //            ShowRestOfDescription.Text = "";
            //        }


            //        //Media Categories: NONE: 0, Picture: 1, Video: 2, YouTubeVideo: 3, Slider: 4
            //        int mediaCategory = int.Parse(ds.Tables[0].Rows[0]["mediaCategory"].ToString());

            //        switch (mediaCategory)
            //        {
            //            case 0:
            //                break;
            //            case 1:
            //                ShowVideoPictureLiteral.Text = "<img style=\"float: left; padding-right: 10px; padding-top: 9px;\" height=\"250px\" width=\"440px\" src=\"UserFiles/" + ds.Tables[0].Rows[0]["Picture"].ToString() + "\" />";
            //                break;
            //            case 2:
            //                ShowVideoPictureLiteral.Text = "<div style=\"float:left; padding-top: 9px; padding-right: 10px;\"><embed  height=\"250px\" width=\"440px\" src=\"UserFiles/" + ds.Tables[0].Rows[0]["Video"].ToString() + "\" /></div>";
            //                break;
            //            case 3:
            //                ShowVideoPictureLiteral.Text = "<div style=\"float:left; padding-top: 9px; padding-right: 10px;\"><object width=\"440\" height=\"250\"><param name=\"movie\" value=\"http://www.youtube.com/v/" + ds.Tables[0].Rows[0]["YouTubeVideo"].ToString() + "\"></param><param name=\"allowFullScreen\" value=\"true\"></param><embed src=\"http://www.youtube.com/v/" + ds.Tables[0].Rows[0]["YouTubeVideo"].ToString() + "\" type=\"application/x-shockwave-flash\" allowfullscreen=\"true\" width=\"440\" height=\"250\"></embed></object></div>";
            //                break;
            //            case 4:
            //                ShowVideoPictureLiteral.Text = "";
            //                DataSet dsSlider = dat.GetData("SELECT * FROM Event_Slider_Mapping WHERE EventID=" + ID);
            //                if (dsSlider.Tables.Count > 0)
            //                    if (dsSlider.Tables[0].Rows.Count > 0)
            //                    {
            //                        char[] delim = { '\\' };
            //                        string[] fileArray = System.IO.Directory.GetFiles(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\Slider\\");

            //                        string[] finalFileArray = new string[fileArray.Length];

            //                        for (int i = 0; i < fileArray.Length; i++)
            //                        {
            //                            int length = fileArray[i].Split(delim).Length;
            //                            finalFileArray[i] = "http://" + Request.Url.Authority + "/HippoHappenings/UserFiles/" + Session["UserName"].ToString() + "/Slider/" + fileArray[i].Split(delim)[length - 1];
            //                        }
            //                        Rotator1.DataSource = finalFileArray;
            //                        Rotator1.DataBind();
            //                        RotatorPanel.Visible = true;
            //                    }
            //                break;
            //            default: break;
            //        }


            //        this.Title = EventName.Text;
            //    }
            //}
            #endregion
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString();
        }
    }

    //protected void Search(object sender, EventArgs e)
    //{
    //    if (SearchTextBox.THE_TEXT != "")
    //    {
    //        char[] delim = { ' ' };
    //        string[] tokens;

    //        tokens = SearchTextBox.THE_TEXT.Split(delim);
    //        string temp = "";
    //        for (int i = 0; i < tokens.Length; i++)
    //        {
    //            temp += " E.Header LIKE @search" + i.ToString();

    //            if (i + 1 != tokens.Length)
    //                temp += " AND ";
    //        }
    //        string searchStr = "SELECT DISTINCT E.ID AS EID, V.ID AS VID, * FROM Events E, Venues V, Event_Occurance EO WHERE E.ID=EO.EventID AND E.Venue=V.ID AND " + temp;
    //        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
    //        if (conn.State != ConnectionState.Open)
    //            conn.Open();
    //        SqlCommand cmd = new SqlCommand(searchStr, conn);
    //        for (int i = 0; i < tokens.Length; i++)
    //        {
    //            cmd.Parameters.Add("@search" + i.ToString(), SqlDbType.NVarChar).Value = "%" + tokens[i] + "%";
    //        }

    //        DataSet ds = new DataSet();
    //        SqlDataAdapter da = new SqlDataAdapter(cmd);
    //        da.Fill(ds);
    //        conn.Close();


    //        Session["EventSearchDS"] = ds;
    //    }
    //    Response.Redirect("EventSearch.aspx");
    //}

    protected void Page_Init(object sender, EventArgs e)
    {
        try
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
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Context.Request.Cookies[cookieName];

            if (!IsPostBack)
            {
                FormsAuthenticationTicket authTicket = null;
                try
                {
                    string group = "";
                    if (authCookie != null)
                    {
                        authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                        group = authTicket.UserData.ToString();
                    }


                    if (group.Contains("User"))
                    {
                        //DataSet ds2 = dat.RetrieveAds(Session["User"].ToString(), false);
                        //DataSet dsMain = dat.RetrieveMainAds(Session["User"].ToString());
                        //Ads1.DATA_SET = ds2;
                        //Ads1.MAIN_AD_DATA_SET = dsMain;
                    }
                    else
                    {
                        //Ads1.DATA_SET = dat.RetrieveAllAds(false);
                        //Ads1.MAIN_AD_DATA_SET = dat.RetrieveAllAds(true);
                    }
                }
                catch (Exception ex)
                {
                    //Response.Redirect("~/UserLogin.aspx");
                }
            }
            Button button = (Button)dat.FindControlRecursive(this, "HomeLink");
            button.CssClass = "NavBarImageHomeSelected";
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString();
        }
    }

    protected void GoToSearch(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        string cookieName = FormsAuthentication.FormsCookieName;
        HttpCookie authCookie = Context.Request.Cookies[cookieName];

        string country = "";
        string state = "";
        string city = "";

        FormsAuthenticationTicket authTicket = null;

        string group = "";
        if (authCookie != null)
        {
            authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            group = authTicket.UserData.ToString();
        }

        if (group.Contains("User"))
        {
            DataSet ds1 = dat.GetData("SELECT * FROM Users U, UserPreferences UP WHERE U.User_ID=" +
                authTicket.Name + " AND U.User_ID=UP.UserID ");

            if(ds1.Tables.Count > 0)
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    country = ds1.Tables[0].Rows[0]["CatCountry"].ToString();
                    state = ds1.Tables[0].Rows[0]["CatState"].ToString();
                    city = ds1.Tables[0].Rows[0]["CatCity"].ToString();
                }
        }
        else
        {
            DataSet ds1 = dat.GetData("SELECT * FROM SearchIPs WHERE IP='" + dat.GetIP() + "'");

            if(ds1.Tables.Count > 0)
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    country = ds1.Tables[0].Rows[0]["Country"].ToString();
                    state = ds1.Tables[0].Rows[0]["State"].ToString();
                    city = ds1.Tables[0].Rows[0]["City"].ToString();
                }

        }

        if (country != "")
            country = " AND E.Country = " + country;

        int c = 0;

        if (state != "")
        {

            c++;
        }

        if (city != "")
        {

            c++;
        }

        SqlDbType[] types = new SqlDbType[c];
        object[] data = new object[c];

        if (state != "")
        {
            types[0] = SqlDbType.NVarChar;
            data[0] = state;
            state = " AND E.State=@p0 ";
            if (city != "")
            {
                types[1] = SqlDbType.NVarChar;
                data[1] = city;
                city = " AND E.City=@p1 ";
            }
        }
        else
        {
            if (city != "")
            {
                types[0] = SqlDbType.NVarChar;
                data[0] = city;
                city = " AND E.City=@p0 ";
            }
        }





        DataSet ds = dat.GetDataWithParemeters("SELECT DISTINCT TOP 10 EO.DateTimeStart, E.Header, E.Content, EO.EventID FROM Events E, Event_Occurance EO WHERE E.ID=EO.EventID " + country + state + city + " AND CONVERT(NVARCHAR, MONTH(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR, DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR, YEAR(EO.DateTimeStart)) = '" + RadCalendar1.SelectedDate.ToShortDateString() + "'", types, data);

        EventPanel.Controls.Clear();
        if (ds.Tables.Count > 0)
            if (ds.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    DateTime date = DateTime.Parse(ds.Tables[0].Rows[i]["DateTimeStart"].ToString());
                    ASP.controls_homeevent_ascx eventH = new ASP.controls_homeevent_ascx();
                    eventH.DAY = date.DayOfWeek.ToString().Substring(0, 3);
                    eventH.DAY_NUMBER = date.Day.ToString();
                    eventH.MONTH = dat.GetMonth(date.Month.ToString()).Substring(0, 3);
                    eventH.EVENT_NAME = ds.Tables[0].Rows[i]["Header"].ToString();
                    if (ds.Tables[0].Rows[i]["Content"].ToString().Length > 150)
                        eventH.SUMMARY = dat.BreakUpString(ds.Tables[0].Rows[i]["Content"].ToString().Substring(0, 150), 67) + "...";
                    else
                        eventH.SUMMARY = dat.BreakUpString(ds.Tables[0].Rows[i]["Content"].ToString(), 67) + "...";
                    eventH.EVENT_ID = int.Parse(ds.Tables[0].Rows[i]["EventID"].ToString());
                    EventPanel.Controls.Add(eventH);

                    
                }
            }
            else
            {
                Label label = new Label();
                label.CssClass = "EventBody";
                label.Text = "No events are listed for this date. Sorry. But perhaps you could enter one... <a class=\"AddLink\" href=\"EnterEvent.aspx\">Enter Events</a>.";
                EventPanel.Controls.Add(label);
            }
        else
        {
            Label label = new Label();
            label.CssClass = "EventBody";
            label.Text = "No events are listed for this date. Sorry. But perhaps you could enter one... <a class=\"AddLink\" href=\"EnterEvent.aspx\">Enter Events</a>.";
            EventPanel.Controls.Add(label);
        }

        Session["HomeEvents"] = ds;
      
    }
}