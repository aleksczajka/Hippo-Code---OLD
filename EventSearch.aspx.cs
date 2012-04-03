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
using Telerik.Web.UI;
using System.Diagnostics;

public partial class EventSearch : Telerik.Web.UI.RadAjaxPage
{
    protected enum TimeFrame { Beginning, Today, Tomorrow, ThisWeek, ThisWeekend, ThisMonth, NextDays, HighestPrice };

    protected void Page_Load(object sender, EventArgs e)
    {
        ////Page.Trace.IsEnabled = true;
        ////Page.Trace.TraceMode = TraceMode.SortByTime;

        Page.Trace.Write("Started Page Load");

        //if (Request.QueryString["page"] == null)
        //    Response.Redirect("event-search?page=1");
        Session["Searching"] = "Events";
        MessageLabel.Text = "";
        
        DataSet ds;
        BottomScriptLiteral.Text = "<script type=\"text/javascript\">initialize1();</script>";
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        DateTime isn = DateTime.Now;

        DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn);

        Data dat = new Data(isn);
        HtmlHead head = (HtmlHead)Page.Header;

        Literal lit = new Literal();
        lit.Text = "<script src=\"http://maps.google.com/maps?file=api&amp;v=2&amp;sensor=false&amp;key=ABQIAAAAjjoxQtYNtdn3Tc17U5-jbBR2Kk_H7gXZZZniNQ8L14X1BLzkNhQjgZq1k-Pxm8FxVhUy3rfc6L9O4g\" type=\"text/javascript\"></script>";
        head.Controls.Add(lit);

        Page.Trace.Write("Before Buttons");

        #region Take Care of Buttons
        SearchButton.BUTTON_TEXT += "<img style=\"border: 0; padding-top: 3px;\" src=\"NewImages/search.png\"/>";
        //SearchButton.BUTTON_TEXT = "<div class=\"NavyLink12UD\">Search</div>";
        SearchButton.SERVER_CLICK += SearchButton_Click;
        //TodayButton.BUTTON_TEXT = "<div><img style=\"border: 0; padding-top: 3px;\" src=\"NewImages/TodayText.png\"/></div>";
        //TodayButton.BUTTON_TEXT = "Today";
        //TodayButton.ADD_STYLE = "width: 74%; ";
        //TomorrowButton.BUTTON_TEXT = "<div><img style=\"border: 0; padding-top: 3px;\" src=\"NewImages/TomorrowText.png\"/></div>";
        //TomorrowButton.BUTTON_TEXT = "Tomorrow";
        //TomorrowButton.ADD_STYLE = "width: 81%; ";
        //ThisWeekButton.BUTTON_TEXT = "<div><img style=\"border: 0; padding-top: 3px;\" src=\"NewImages/ThisWeekText.png\"/></div>";
        //ThisWeekButton.BUTTON_TEXT = "This Week";
        //ThisWeekButton.ADD_STYLE = "width: 74%; ";
        //ThisWeekendButton.BUTTON_TEXT = "<div><img style=\"border: 0; padding-top: 3px;\" src=\"NewImages/ThisWeekendText.png\"/></div>";
        //ThisWeekendButton.BUTTON_TEXT = "This Weekend";
        //ThisWeekendButton.ADD_STYLE = "width: 81%; ";
        //ThisMonthButton.BUTTON_TEXT = "<div><img style=\"border: 0; padding-top: 3px;\" src=\"NewImages/ThisMonth.png\"/></div>";
        //ThisMonthButton.BUTTON_TEXT = "This Month";
        //ThisMonthButton.ADD_STYLE = "width: 74%; ";
        //TodayButton.SERVER_CLICK += SetToday;
        //TomorrowButton.SERVER_CLICK += SetTomorrow;
        //ThisWeekButton.SERVER_CLICK += SetThisWeek;
        //ThisWeekendButton.SERVER_CLICK += SetThisWeekend;
        //ThisMonthButton.SERVER_CLICK += SetThisMonth;
        FilterButton.SERVER_CLICK += FilterResults;
        SmallButton1.BUTTON_TEXT += "<img style=\"border: 0; padding-top: 3px;\" src=\"NewImages/search.png\"/>";
        SmallButton1.SERVER_CLICK += SearchButton_Click;
        #endregion

        Page.Trace.Write("After Buttons");

        ////Page.Trace.Write("Finished Initialization");

        if (!IsPostBack)
        {
            HttpCookie timeCookie = Request.Cookies["SetTime"];
            if (timeCookie != null)
            {
                timeCookie = new HttpCookie("BrowserDate");
                timeCookie.Value = "All%20Future%20Events";
                Response.Cookies.Add(timeCookie);
                TimeFrameDiv.InnerText = "All Future Events";
            }

            Session["HighestPrice"] = null;
            Session.Remove("HighestPrice");

            Session["SetVenue"] = null;
            Session.Remove("SetVenue");
            Session["SearchDS"] = null;
            Session.Remove("SearchDS");

            NumsLabel.Text = "";
            NoResultsPanel.Visible = true;
            MapPanel.Visible = false;

            TopLiteral.Text = "<script type=\"text/javascript\">function initialize1(){map = null;}</script>";
            ScriptLiteral.Text = "";

            HtmlLink lk = new HtmlLink();
            lk.Attributes.Add("rel", "canonical");
            lk.Href = "http://hippohappenings.com/event-search";
            head.Controls.AddAt(0, lk);


            

            //Create keyword and description meta tags
            HtmlMeta hm = new HtmlMeta();
            HtmlMeta kw = new HtmlMeta();

            kw.Name = "keywords";
            kw.Content = "local events happenings going on neighborhood local find search";

            head.Controls.AddAt(0, kw);

            hm.Name = "Description";
            hm.Content = "Find local events, happenings, going on in your neighborhood on HippoHappenings, the site which fosters communities.";
            head.Controls.AddAt(0, hm);

            lk = new HtmlLink();
            lk.Href = "http://" + Request.Url.Authority + "/event-search";
            lk.Attributes.Add("rel", "bookmark");
            head.Controls.AddAt(0, lk);

            EventTitle.Text = "<a style=\"text-decoration: none;\"  href=\"" +
                lk.Href + "\"><h1>Search Events</h1></a>";

            //Button button = (Button)dat.FindControlRecursive(this, "EventLink");
            //button.CssClass = "NavBarImageEventSelected";

            ////Page.Trace.Write("Finished not Postback");
        }
        else
        {
            if (Session["SetVenue"] != null)
            {
                char[] delim = { ';' };

                VenueDiv.InnerHtml = Session["SetVenue"].ToString().Split(delim)[1];
            }

            //if (Session["SetTime"] != null)
            //{
            //    TimeFrameDiv.InnerHtml = Session["SetTime"].ToString();
            //}
        }

        if (!IsPostBack)
        {
            ////Page.Trace.Write("Started not postback 2");
            Page.Trace.Write("Before State Country Load");
            CountryDropDown.SelectedValue = "223";

            DataSet ds3 = dat.GetData("SELECT * FROM State WHERE country_id=" + CountryDropDown.SelectedValue);

            StateDropDownPanel.Visible = true;
            StateTextBoxPanel.Visible = false;
            StateDropDown.DataSource = ds3;
            StateDropDown.DataTextField = "state_2_code";
            StateDropDown.DataValueField = "state_id";
            StateDropDown.DataBind();

            StateDropDown.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("State", "-1"));

            StateDropDownPanel.Visible = true;
            StateTextBoxPanel.Visible = false;

            Page.Trace.Write("After State Country Load");

            DataSet ds4 = dat.GetData("SELECT CASE WHEN SUBSTRING(Name, 1, 4) = 'The' THEN SUBSTRING(Name, 5, "+
                "LEN(Name)-4) ELSE Name END AS Name1, * FROM Venues WHERE Country=" + 
                CountryDropDown.SelectedValue + " AND State='" + StateDropDown.SelectedItem.Text + 
                "' ORDER BY Name1 ASC");

            if (ds4.Tables.Count > 0)
                if (ds4.Tables[0].Rows.Count > 0)
                {
                    Session["LocationVenues"] = ds4;

                    fillVenues(ds4);
                }

            Page.Trace.Write("After Venues Load");

            ////Page.Trace.Write("Started Try");
            try
            {
                if (Session["User"] != null)
                {
                    ////Page.Trace.Write("Location");

                    Page.Trace.Write("Before Location Load");

                    DataView dvUser = dat.GetDataDV("SELECT CatCity, CatState, CatCountry, CatZip, Radius FROM  UserPreferences WHERE UserID=" +
                        Session["User"].ToString());
                    CountryDropDown.SelectedValue = dvUser[0]["CatCountry"].ToString();
                    ChangeState(CountryDropDown, new EventArgs());
                    if (StateTextBoxPanel.Visible)
                        StateTextBox.Text = dvUser[0]["CatState"].ToString();
                    else
                        StateDropDown.Items.FindItemByText(dvUser[0]["CatState"].ToString()).Selected = true;

                    CityTextBox.Text = dvUser[0]["CatCity"].ToString();
                    StateChanged(StateDropDown, new EventArgs());

                    Page.Trace.Write("After Location Load");

                    ////Page.Trace.Write("before zip");

                    if (dvUser[0]["CatZip"] != null)
                    {
                        if (dvUser[0]["CatZip"].ToString().Trim() != "")
                        {

                            char[] delim = { ';' };
                            string[] tokens = dvUser[0]["CatZip"].ToString().Split(delim);

                            if (tokens.Length > 1)
                            {
                                ZipTextBox.Text = tokens[1].Trim();

                                if (dvUser[0]["Radius"] != null)
                                {
                                    if (dvUser[0]["Radius"].ToString().Trim() != "")
                                    {

                                        RadiusDropDown.SelectedValue = dvUser[0]["Radius"].ToString();
                                    }
                                }
                            }
                        }
                    }
                    ////Page.Trace.Write("after zip");
                    Page.Trace.Write("AFter zip");
                }
                else
                {
                    Page.Trace.Write("before get ip");
                    if (Session["LocCountry"] == null || Session["LocState"] == null || Session["LocCity"] == null)
                    {
                        dat.IP2Location();
                    }
                    Page.Trace.Write("after get ip");
                    CountryDropDown.SelectedValue = Session["LocCountry"].ToString().Trim();
                    ChangeState(CountryDropDown, new EventArgs());
                    if (StateTextBoxPanel.Visible)
                        StateTextBox.Text = Session["LocState"].ToString().Trim();
                    else
                        StateDropDown.Items.FindItemByText(Session["LocState"].ToString().Trim()).Selected = true;

                    CityTextBox.Text = Session["LocCity"].ToString();
                    StateChanged(StateDropDown, new EventArgs());
                    Page.Trace.Write("after");
                }
                Page.Trace.Write("before search");
                Search();
                Page.Trace.Write("after search");
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.ToString();
            }
            ////Page.Trace.Write("Ended try");
        }
        else
        {
            if (Session["LocationVenues"] != null)
                fillVenues((DataSet)Session["LocationVenues"]);
        }

        Page.Trace.Write("Ended Page Load");
    }

    protected void GoToLogin(object sender, EventArgs e)
    {
        Session["RedirectTo"] = "enter-event";
        Response.Redirect("enter-event");
    }

    protected void SearchButton_Click(object sender, EventArgs e)
    {
        int intOut = 0;

        //Session["SetTime"] = "All Future Events";

        //if (int.TryParse(TimeRad.SelectedValue, out intOut))
        //{
        //    Session["SetTime"] = "Next ";
        //}
        //else
        //{
        //    DateTime dateOut = new DateTime();
        //    if (RadDatePicker2.DateInput.SelectedDate != null)
        //    {
        //        if (DateTime.TryParse(RadDatePicker2.DateInput.SelectedDate.Value.ToString(), out dateOut))
        //        {
        //            Session["SetTime"] = "ThisDate";
        //        }
        //    }
        //}
        Search();
    }

    protected void Search()
    {
        ////Page.Trace.Write("Search Begin");
        string message = "";
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;

        Data dat = new Data(isn);

        //ResultsLabel.Text += isn.ToString() + "<br/>" + cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":");
        
        DateTime itBeNow = isn;
            string resultsStr = "";
            CityTextBox.Text = dat.stripHTML(CityTextBox.Text.Trim());
            StateTextBox.Text = dat.stripHTML(StateTextBox.Text.Trim());
            try
            {
                //SearchElements2.Clear();

                DateTime isNow = isn;
                string featureDate = isNow.Month.ToString() + "/" + isNow.Day.ToString() + "/" + isNow.Year.ToString();

                KeywordsTextBox.Text = dat.stripHTML(KeywordsTextBox.Text);

                char[] delim = { ' ' };
                string[] tokens;

                tokens = KeywordsTextBox.Text.Split(delim);
                string temp = "";
                string venTemp = "";
                string featureSearchTerms = "";
                string featureVenTerms = "";
                string allSearchTerms = "";
                string allVenSearchTerms = "";
                string featureString = "";
                string featureStringVen = "";
                string includeSearchTerms = "";
                string includeVenSearchTerms = "";
                if (KeywordsTextBox.Text.Trim() != "")
                {
                    for (int i = 0; i < tokens.Length; i++)
                    {
                        temp += " E.Header LIKE @search" + i.ToString();
                        venTemp += " VE.EventName LIKE @search" + i.ToString();

                        featureSearchTerms += " EST.SearchTerms LIKE '%;" + tokens[i].Replace("'", "''") + ";%' ";
                        featureVenTerms += " EST.SearchTerms LIKE '%;" + tokens[i].Replace("'", "''") + ";%' ";

                        if (i + 1 != tokens.Length)
                        {
                            temp += " AND ";
                            venTemp += " AND ";
                            featureVenTerms += " AND ";
                            featureSearchTerms += " AND ";
                        }
                    }
                    resultsStr += "'" + KeywordsTextBox.Text + "' ";
                }
                //if (temp != "")
                //{
                //    temp = " AND " + temp;
                //    venTemp = " AND " + venTemp;
                //}

                if (temp != "")
                {
                    allSearchTerms = " AND ((" + temp + ") OR (E.ID=EST.EventID AND SearchDate = '" + featureDate +
                        "' AND " + featureSearchTerms + "))";

                    includeSearchTerms = ", EventSearchTerms EST ";

                }

                if (venTemp != "")
                {
                    allVenSearchTerms = " AND ((" + venTemp + ") OR (E.ID=EST.VenueID AND SearchDate = '" + featureDate +
                        "' AND " + featureVenTerms + "))";

                    includeVenSearchTerms = " VenueSearchTerms EST, ";

                }

                //string catHiTemp = categories + highestP + temp;
                //string catTempVen = venTemp;

                


                //if (featureSearchTerms != "")
                //{
                //    featureSearchTerms = " AND (" + featureSearchTerms + ") ";
                //    featureVenTerms = " AND (" + featureVenTerms + ") ";
                //}

                bool isZip = false;


                if (ZipTextBox.Text.Trim() != "")
                        isZip = true;


                
                bool goOn = true;
                string country = " AND E.Country=" + CountryDropDown.SelectedValue;
                string countryVEN = " AND E.Country=" + CountryDropDown.SelectedValue;

                string state = "";
                string stateParam = "";
                string city = "";
                string cityParam = "";
                string cityVen = "";
                string stateVen = "";

                string zip = "";
                string zipVen = "";
                string zipParameter = "";
                int zipParam = 0;
                string nonExistantZip = "";

                string theVenue = "";

                if (isZip && Session["SetVenue"] == null)
                {
                    if (ZipTextBox.Text.Trim() != "")
                    {
                        resultsStr += "in " + ZipTextBox.Text.Trim() + " ";
                        //do only if United States and not for international zip codes
                        if (RadiusDropPanel.Visible)
                        {
                            resultsStr += " within " + RadiusDropDown.SelectedItem.Text;
                            //Get all zips within the specified radius

                            if (int.TryParse(ZipTextBox.Text.Trim(), out zipParam))
                            {
                                DataSet dsLatsLongs = dat.GetData("SELECT Latitude, Longitude FROM ZipCodes WHERE Zip='" +
                                    zipParam + "'");

                                //some zip codes don't exist in the database, find the closest one
                                bool findClosest = false;
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
                                    zip = " AND E.Zip = '" + zipParam.ToString() + "' ";
                                    zipVen = " AND E.Zip = '" + zipParam.ToString() + "' ";
                                    nonExistantZip = " OR V.Zip = '" + zipParam.ToString() + "'";
                                    while (dsLatsLongs == null)
                                    {
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

                                //get all the zip codes within the specified radius
                                DataSet dsZips = dat.GetData("SELECT Zip FROM ZipCodes WHERE dbo.GetDistance(" + dsLatsLongs.Tables[0].Rows[0]["Longitude"].ToString() +
                                    ", " + dsLatsLongs.Tables[0].Rows[0]["Latitude"].ToString() + ", Longitude, Latitude) < " + RadiusDropDown.SelectedValue);

                                if (dsZips.Tables.Count > 0)
                                {
                                    if (dsZips.Tables[0].Rows.Count > 0)
                                    {
                                        zip = " AND (E.Zip = '" + zipParam.ToString() + "' " + nonExistantZip;
                                        zipVen = " AND (e.Zip = '" + zipParam.ToString() + "' " + nonExistantZip;
                                        for (int i = 0; i < dsZips.Tables[0].Rows.Count; i++)
                                        {
                                            zip += " OR E.Zip = '" + dsZips.Tables[0].Rows[i]["Zip"].ToString() + "' ";
                                            zipVen += " OR E.Zip = '" + dsZips.Tables[0].Rows[i]["Zip"].ToString() + "' ";

                                        }
                                        zip += ") ";
                                        zipVen += ") ";
                                    }
                                    else
                                    {
                                        zip = " AND E.Zip='" + ZipTextBox.Text.Trim() + "'";
                                        zipVen = " AND E.Zip='" + ZipTextBox.Text.Trim() + "'";
                                    }
                                }
                                else
                                {
                                    zip = " AND E.Zip='" + ZipTextBox.Text.Trim() + "'";
                                    zipVen = " AND E.Zip='" + ZipTextBox.Text.Trim() + "'";
                                }



                            }
                            else
                            {
                                goOn = false;
                                message = "Zip code is not in the right format.";
                            }
                        }
                        else
                        {
                            zip = " AND E.Zip = @zip ";
                            zipVen = " AND E.Zip = @zip ";
                            zipParameter = ZipTextBox.Text.Trim();
                        }
                    }
                    else
                    {
                        goOn = false;
                        message = "You must at the very least include a zip code or State and City.";
                    }
                }
                else if(Session["SetVenue"] == null)
                {
                    if (CityTextBox.Text != "")
                    {
                        city = " AND E.City LIKE @city ";
                        cityVen = " AND E.City LIKE @city ";
                        cityParam = CityTextBox.Text.ToLower();
                        resultsStr += " in " + CityTextBox.Text.ToLower();
                    }
                    else
                    {
                        goOn = false;
                        message = "You must at the very least include a zip code or State and City.";
                    }

                    if (StateDropDownPanel.Visible)
                    {
                        if (StateDropDown.SelectedValue != "-1")
                        {
                            state = " AND E.State=@state ";
                            stateVen = " AND E.State=@state ";
                            stateParam = StateDropDown.SelectedItem.Text;
                            resultsStr += ", " + StateDropDown.SelectedItem.Text;
                        }
                        else
                        {
                            goOn = false;
                            message = "You must at the very least include a zip code or State and City.";
                        }
                    }
                    else
                    {
                        if (StateTextBox.Text != "")
                        {
                            state = " AND E.State=@state ";
                            stateVen = " AND E.State=@state ";
                            stateParam = StateTextBox.Text;
                            resultsStr += " in " + StateTextBox.Text;
                        }
                        else
                        {
                            goOn = false;
                            message = "You must at the very least include a zip code or State and City.";
                        }
                    }

                    
                }

                if (goOn)
                {
                    string date = "";
                    string dateVen = "";
                    HttpCookie timeCookie = Request.Cookies["SetTime"];
                    string test = "All%20Future%20Events";
                    if(timeCookie != null)
                        test = timeCookie.Value;
                    if (test.Trim() == "")
                        test = "All%20Future%20Events";
                    if (test != "")
                    {
                        string theDate = "";
                        int subtraction = 0;
                        string startDate = "";
                        string endDate = "";

                        //Variables for Locale regular events
                        
                        string dayOfWeek = dat.getDayOfWeek(itBeNow.DayOfWeek);
                        int dayOfWeekInt = int.Parse(dayOfWeek);
                       
                        switch (test)
                        {
                            case "Today":
                                dateVen = " AND Days LIKE '%" + dayOfWeek + "%'";
                                resultsStr += " Today ";
                                theDate = isn.ToShortDateString();
                                date = " AND (CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) " +
                                    " = CONVERT(DATETIME, '" + theDate + "') OR (CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + "+
                                    "CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) < "+
                                    "CONVERT(DATETIME, '" + theDate + "') AND CONVERT(NVARCHAR,MONTH(EO.DateTimeEnd)) + '/' + CONVERT(NVARCHAR,DAY(EO.DateTimeEnd)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeEnd)) > CONVERT(DATETIME, '" + theDate + "'))) ";
                                break;
                            case "Tomorrow":
                                dayOfWeekInt++;
                                if (dayOfWeekInt > 7)
                                    dayOfWeekInt = 1;
                                dateVen = " AND Days LIKE '%" + dayOfWeekInt.ToString() + "%'";
                                resultsStr += " Tomorrow ";
                                theDate = isn.AddDays(1.00).ToShortDateString();
                                date = " AND (CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) " +
                                    " = CONVERT(DATETIME, '" + theDate + "') OR (CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + " +
                                    "CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) < " +
                                    "CONVERT(DATETIME, '" + theDate + "') AND CONVERT(NVARCHAR,MONTH(EO.DateTimeEnd)) + '/' + CONVERT(NVARCHAR,DAY(EO.DateTimeEnd)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeEnd)) > CONVERT(DATETIME, '" + theDate + "')))";
                                break;
                            case "This%20Weekend":
                                dateVen = " AND (Days LIKE '%5%' OR Days LIKE '%6%' OR Days LIKE '%7%') ";
                                resultsStr += " This Weekend ";
                                switch (isn.DayOfWeek)
                                {
                                    case DayOfWeek.Friday:
                                        subtraction = 0;
                                        break;
                                    case DayOfWeek.Monday:
                                        subtraction = -4;
                                        break;
                                    case DayOfWeek.Saturday:
                                        subtraction = 1;
                                        break;
                                    case DayOfWeek.Sunday:
                                        subtraction = 2;
                                        break;
                                    case DayOfWeek.Thursday:
                                        subtraction = -1;
                                        break;
                                    case DayOfWeek.Tuesday:
                                        subtraction = -3;
                                        break;
                                    case DayOfWeek.Wednesday:
                                        subtraction = -2;
                                        break;
                                    default: break;
                                }
                                startDate = isn.Subtract(TimeSpan.FromDays(subtraction)).ToShortDateString();
                                endDate = isn.AddDays(2 - subtraction).ToShortDateString();
                                date = " AND (CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) "+
                                    "+ '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) " +
                                    " >= CONVERT(DATETIME, '" + startDate + "') AND CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + "+
                                    "'/' + CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) "+
                                    "<= CONVERT(DATETIME, '" + endDate + "') OR "+
                                    "(CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + "+
                                    "CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) < "+
                                    "CONVERT(DATETIME, '" + startDate + "') AND CONVERT(NVARCHAR,MONTH(EO.DateTimeEnd)) + '/' + "+
                                    "CONVERT(NVARCHAR,DAY(EO.DateTimeEnd)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeEnd)) > CONVERT(DATETIME, '" + startDate + "')))";
                                break;
                            case "This%20Week":
                                dateVen = "";
                                resultsStr += " This Week ";
                                switch (isn.DayOfWeek)
                                {
                                    case DayOfWeek.Friday:
                                        subtraction = 5;
                                        break;
                                    case DayOfWeek.Monday:
                                        subtraction = 1;
                                        break;
                                    case DayOfWeek.Saturday:
                                        subtraction = 6;
                                        break;
                                    case DayOfWeek.Sunday:
                                        subtraction = 0;
                                        break;
                                    case DayOfWeek.Thursday:
                                        subtraction = 4;
                                        break;
                                    case DayOfWeek.Tuesday:
                                        subtraction = 2;
                                        break;
                                    case DayOfWeek.Wednesday:
                                        subtraction = 3;
                                        break;
                                    default: break;
                                }
                                startDate = isn.Subtract(TimeSpan.FromDays(subtraction)).ToShortDateString();
                                endDate = isn.AddDays(7.00 - subtraction).ToShortDateString();
                                date = " AND (CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) " +
                                    " >= CONVERT(DATETIME, '" + startDate + "') AND CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + "+
                                    "CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) <= CONVERT(DATETIME, '" + endDate + "')OR "+
                                    "(CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + "+
                                    "CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) < "+
                                    "CONVERT(DATETIME, '" + startDate + "') AND CONVERT(NVARCHAR,MONTH(EO.DateTimeEnd)) + '/' + "+
                                    "CONVERT(NVARCHAR,DAY(EO.DateTimeEnd)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeEnd)) > CONVERT(DATETIME, '" + startDate + "')))";
                                break;
                            case "Next%20Week":
                                dateVen = "";
                                resultsStr += " Next Week ";
                                switch (isn.DayOfWeek)
                                {
                                    case DayOfWeek.Friday:
                                        subtraction = -2;
                                        break;
                                    case DayOfWeek.Monday:
                                        subtraction = -6;
                                        break;
                                    case DayOfWeek.Saturday:
                                        subtraction = -1;
                                        break;
                                    case DayOfWeek.Sunday:
                                        subtraction = -7;
                                        break;
                                    case DayOfWeek.Thursday:
                                        subtraction = -3;
                                        break;
                                    case DayOfWeek.Tuesday:
                                        subtraction = -5;
                                        break;
                                    case DayOfWeek.Wednesday:
                                        subtraction = -4;
                                        break;
                                    default: break;
                                }
                                startDate = isn.Subtract(TimeSpan.FromDays(subtraction)).ToShortDateString();
                                endDate = isn.AddDays(7.00 - subtraction).ToShortDateString();
                                date = " AND ((CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) " +
                                    " >= CONVERT(DATETIME, '" + startDate + "') AND CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) <= CONVERT(DATETIME, '" + endDate + "'))  OR "+
                                    "(CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + "+
                                    "CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) < "+
                                    "CONVERT(DATETIME, '" + startDate + "') AND CONVERT(NVARCHAR,MONTH(EO.DateTimeEnd)) + '/' + "+
                                    "CONVERT(NVARCHAR,DAY(EO.DateTimeEnd)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeEnd)) > CONVERT(DATETIME, '" + startDate + "'))) ";

                                break;
                            case "This%20Month":
                                dateVen = "";
                                resultsStr += " This Month ";
                                date = " AND (MONTH(EO.DateTimeStart) = '" + isn.Month +
                                    "' OR (EO.DateTimeStart < CONVERT(DATETIME,'" + isn.Month +
                                    "/" + DateTime.DaysInMonth(isn.Year, isn.Month).ToString() + "/" + 
                                    isn.Year + " 23:59:59') AND EO.DateTimeEnd >= CONVERT(DATETIME,'" +
                                    isn.Month + "/1/" + isn.Year + " 00:00:00')))";
                                break;
                            case "All%20Past%20Events":
                                dateVen = "";
                                resultsStr += " All Past Events ";
                                date = " AND CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) " +
                                    " <= CONVERT(DATETIME, '" + isn.ToString() + "') ";

                                break;
                            case "All%20Future%20Events":
                                dateVen = "";
                                resultsStr += " ";
                                date = " AND (EO.DateTimeStart  "+
                                    " >= CONVERT(DATETIME, '" + isn.ToString() + "') OR (EO.DateTimeEnd " +
                                    " >= CONVERT(DATETIME, '" + isn.ToString() + "')))";

                                break;
                            case "ThisDate":
                                resultsStr += " on " + RadDatePicker2.DateInput.SelectedDate.Value.ToShortDateString() + " ";
                                date = " AND CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) " +
                                        " = '" + RadDatePicker2.DateInput.SelectedDate.Value.ToShortDateString() + "'";
                                dateVen = " AND Days LIKE '%" + dat.getDayOfWeek(RadDatePicker2.DateInput.SelectedDate.Value.DayOfWeek) + "%' ";
                                break;
                            default:
                                //Must be 'Next .. Days'
                                if (test.Substring(0, 4) == "Next")
                                {
                                    resultsStr += " Next " + TimeRad.SelectedValue + " Days ";
                                    if (int.Parse(TimeRad.SelectedValue) >= 7)
                                    {
                                        dateVen = "";
                                    }
                                    else
                                    {
                                        dateVen += " AND (Days LIKE '%" + dayOfWeekInt.ToString() + "%' ";
                                        for (int i = 0; i < int.Parse(TimeRad.SelectedValue); i++)
                                        {
                                            dateVen += " OR Days LIKE '%" + (++dayOfWeekInt).ToString() + "%' ";
                                        }
                                        dateVen += " ) ";
                                    }
                                    //date = " AND CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) " +
                                    //    " >= CONVERT(DATETIME, '" + isn.ToString() + "') " +
                                    //    " AND CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) <= " +
                                    //    " CONVERT(DATETIME, '" + DateTime.Parse(cookie.Value.ToString().Replace("%20",
                                    //    " ").Replace("%3A", ":")).AddDays(double.Parse(TimeRad.SelectedValue)) + "')";
                                    DateTime StartDate = isNow;
                                    DateTime EndDate = isNow.AddDays(int.Parse(TimeRad.SelectedValue));

                                    date = " AND (EO.DateTimeStart " +
                                        " <= CONVERT(DATETIME, '" + EndDate.Month.ToString() + "/" + EndDate.Day.ToString() + "/" + 
                                        EndDate.Year.ToString() + " 23:59:59') " +
                                        " AND EO.DateTimeEnd >= " +
                                        " CONVERT(DATETIME, '" + StartDate.Month.ToString() + "/" + StartDate.Day.ToString() + "/" + StartDate.Year.ToString() + " 00:00:00'))";
                                }
                                else
                                {
                                    //Telerik.Web.UI.RadDatePicker radP = (Telerik.Web.UI.RadDatePicker)DateDropDown.Items[0].FindControl("RadDatePicker2");
                                    DateTime dtime = DateTime.Parse(test.Replace("%20", " "));
                                    resultsStr += " on " + dtime.Month.ToString() + "/" + dtime.Day.ToString() + "/" + dtime.Year.ToString() + " ";
                                    date = " AND CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) " +
                                            " <= '" + dtime.Month.ToString() + "/" + dtime.Day.ToString() + "/" + dtime.Year.ToString() + "' AND CONVERT(NVARCHAR,MONTH(EO.DateTimeEnd)) + '/' + CONVERT(NVARCHAR,DAY(EO.DateTimeEnd)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeEnd)) " +
                                            " >= '" + dtime.Month.ToString() + "/" + dtime.Day.ToString() + "/" + dtime.Year.ToString() + "'";
                                    dateVen = " AND Days LIKE '%" + dtime.Month.ToString() + "/" + dtime.Day.ToString() + "/" + dtime.Year.ToString() + "%' ";
                                }
                                break;
                        }


                    }

                    string highestP = "";
                    decimal itOut = 0.00M;
                    if (decimal.TryParse(HighestPriceInput.Value.Replace("$", ""), out itOut))
                    {
                        highestP = " AND ((E.MaxPrice <= " + itOut.ToString() + ") OR (E.MaxPrice >= " + itOut.ToString() + " AND E.MinPrice <= " + itOut.ToString() + ")" +
                            "OR (E.MaxPrice IS NULL AND E.MinPrice IS NULL)) ";
                        resultsStr += " Highest Price: $" + itOut.ToString();
                    }
                    else
                    {
                        Session["HighestPrice"] = null;
                        Session.Remove("HighestPrice");
                    }

                    string venue = "";
                    if (Session["SetVenue"] != null)
                    {
                        char[] delim2 = { ';' };
                        string[] toks = Session["SetVenue"].ToString().Split(delim2);
                        venue = " AND E.Venue = " + toks[0];
                        resultsStr += " at " + toks[1].Replace("%20", " ");
                        theVenue = " AND E.ID = " + toks[0];
                    }
                    string categories = "";
                    string addCat = "";
                    string resultsCats = "";
                    string childCategories = "";
                    for (int i = 0; i < CategoryTree.Nodes.Count; i++)
                    {
                        childCategories = "";
                        if (CategoryTree.Nodes[i].Checked)
                        {
                            if (CategoryTree.Nodes[i].Nodes.Count > 0)
                            {
                                for (int h = 0; h < CategoryTree.Nodes[i].Nodes.Count; h++)
                                {
                                    childCategories += " OR ECM.CategoryID = " + CategoryTree.Nodes[i].Nodes[h].Value;

                                    if (CategoryTree.Nodes[i].Nodes[h].Nodes.Count > 0)
                                    {
                                        for (int k = 0; k < CategoryTree.Nodes[i].Nodes[h].Nodes.Count; k++)
                                        {
                                            childCategories += " OR ECM.CategoryID = " + CategoryTree.Nodes[i].Nodes[h].Nodes[k].Value;
                                        }
                                    }
                                }
                            }

                            categories += " AND ( E.ID IN (SELECT ECM.EventID FROM Event_Category_Mapping ECM " +
                                "WHERE ECM.CategoryID = " + CategoryTree.Nodes[i].Value + childCategories + " )) ";


                            if (resultsCats == "")
                            {
                                resultsCats += ", categories: " + CategoryTree.Nodes[i].Text;
                            }
                            else
                            {
                                resultsCats += ", " + CategoryTree.Nodes[i].Text;
                            }



                        }

                        for (int n = 0; n < CategoryTree.Nodes[i].Nodes.Count; n++)
                        {
                            if (CategoryTree.Nodes[i].Nodes[n].Checked)
                            {
                                categories += " AND ( E.ID IN (SELECT ECM.EventID FROM Event_Category_Mapping ECM " +
                                "WHERE ECM.CategoryID = " + CategoryTree.Nodes[i].Nodes[n].Value + " )) ";


                                if (resultsCats == "")
                                {
                                    resultsCats += ", categories: " + CategoryTree.Nodes[i].Nodes[n].Text;
                                }
                                else
                                {
                                    resultsCats += ", " + CategoryTree.Nodes[i].Nodes[n].Text;
                                }

                            }
                        }
                    }

                    for (int i = 0; i < RadTreeView1.Nodes.Count; i++)
                    {
                        childCategories = "";
                        if (RadTreeView1.Nodes[i].Checked)
                        {
                            if (RadTreeView1.Nodes[i].Nodes.Count > 0)
                            {
                                for (int h = 0; h < RadTreeView1.Nodes[i].Nodes.Count; h++)
                                {
                                    childCategories += " OR ECM.CategoryID = " + RadTreeView1.Nodes[i].Nodes[h].Value;

                                    if (RadTreeView1.Nodes[i].Nodes[h].Nodes.Count > 0)
                                    {
                                        for (int k = 0; k < RadTreeView1.Nodes[i].Nodes[h].Nodes.Count; k++)
                                        {
                                            childCategories += " OR ECM.CategoryID = " + RadTreeView1.Nodes[i].Nodes[h].Nodes[k].Value;
                                        }
                                    }
                                }
                            }

                            categories += " AND ( E.ID IN (SELECT ECM.EventID FROM Event_Category_Mapping ECM " +
                                "WHERE ECM.CategoryID = " + RadTreeView1.Nodes[i].Value + childCategories + " )) ";


                            if (resultsCats == "")
                            {
                                resultsCats += ", categories: " + RadTreeView1.Nodes[i].Text;
                            }
                            else
                            {
                                resultsCats += ", " + RadTreeView1.Nodes[i].Text;
                            }



                        }

                        for (int n = 0; n < RadTreeView1.Nodes[i].Nodes.Count; n++)
                        {
                            if (RadTreeView1.Nodes[i].Nodes[n].Checked)
                            {
                                categories += " AND ( E.ID IN (SELECT ECM.EventID FROM Event_Category_Mapping ECM " +
                                "WHERE ECM.CategoryID = " + RadTreeView1.Nodes[i].Nodes[n].Value + " )) ";


                                if (resultsCats == "")
                                {
                                    resultsCats += ", categories: " + RadTreeView1.Nodes[i].Nodes[n].Text;
                                }
                                else
                                {
                                    resultsCats += ", " + RadTreeView1.Nodes[i].Nodes[n].Text;
                                }

                            }
                        }
                    }

                    for (int i = 0; i < RadTreeView2.Nodes.Count; i++)
                    {
                        childCategories = "";
                        if (RadTreeView2.Nodes[i].Checked)
                        {
                            if (RadTreeView2.Nodes[i].Nodes.Count > 0)
                            {
                                for (int h = 0; h < RadTreeView2.Nodes[i].Nodes.Count; h++)
                                {
                                    childCategories += " OR ECM.CategoryID = " + RadTreeView2.Nodes[i].Nodes[h].Value;

                                    if (RadTreeView2.Nodes[i].Nodes[h].Nodes.Count > 0)
                                    {
                                        for (int k = 0; k < RadTreeView2.Nodes[i].Nodes[h].Nodes.Count; k++)
                                        {
                                            childCategories += " OR ECM.CategoryID = " + RadTreeView2.Nodes[i].Nodes[h].Nodes[k].Value;
                                        }
                                    }
                                }
                            }

                            categories += " AND ( E.ID IN (SELECT ECM.EventID FROM Event_Category_Mapping ECM " +
                                "WHERE ECM.CategoryID = " + RadTreeView2.Nodes[i].Value + childCategories + " )) ";


                            if (resultsCats == "")
                            {
                                resultsCats += ", categories: " + RadTreeView2.Nodes[i].Text;
                            }
                            else
                            {
                                resultsCats += ", " + RadTreeView2.Nodes[i].Text;
                            }



                        }

                        for (int n = 0; n < RadTreeView2.Nodes[i].Nodes.Count; n++)
                        {
                            if (RadTreeView2.Nodes[i].Nodes[n].Checked)
                            {
                                categories += " AND ( E.ID IN (SELECT ECM.EventID FROM Event_Category_Mapping ECM " +
                                "WHERE ECM.CategoryID = " + RadTreeView2.Nodes[i].Nodes[n].Value + " )) ";


                                if (resultsCats == "")
                                {
                                    resultsCats += ", categories: " + RadTreeView2.Nodes[i].Nodes[n].Text;
                                }
                                else
                                {
                                    resultsCats += ", " + RadTreeView2.Nodes[i].Nodes[n].Text;
                                }

                            }
                        }
                    }

                    resultsStr += resultsCats;

                    //if (categories != "")
                    //    categories += " ) ";


                   
                    

                    string locationVen = "";
                    if (theVenue != "")
                        locationVen = theVenue;
                    else
                        locationVen = countryVEN + stateVen + cityVen + zipVen;

                    string searchStrVen = "SELECT DISTINCT 'V' AS Type, E.Featured, E.Name, VE.EventName + " +
                        "' at '+E.Name AS Header, E.DaysFeatured, E.ID AS EID, E.ID AS VID, " +
                            "E.Content FROM " + includeVenSearchTerms + " Venues E, VenueEvents VE "+
                            "WHERE VE.VenueID=E.ID " +
                            locationVen + dateVen + allVenSearchTerms +
                            " AND E.Live='True' ORDER BY E.Name ";

                    string searchStr = "SELECT DISTINCT 'E' AS Type, E.ID AS EID, E.Venue AS VID, E.Header, " +
                        " V.Name, E.Featured, E.ShortDescription AS Content, E.DaysFeatured FROM Events E, Venues V, " +
                        "Event_Occurance EO " + addCat + includeSearchTerms + " WHERE E.ID=EO.EventID AND E.Venue=V.ID " +
                        country + state + city + zip + date + venue + " AND E.Live='True' AND V.Live='True' " +
                        allSearchTerms + highestP + categories + " ORDER BY E.Header";

                    string searchStrAddress = "";
                    if (Session["SetVenue"] == null)
                    {
                        searchStrAddress = "SELECT DISTINCT 'E' AS Type, E.ID AS EID, 'NONE' AS VID, E.Header, E.Address AS 'Name', " +
                         " E.Featured, E.ShortDescription AS Content, E.DaysFeatured FROM Events E, " +
                        "Event_Occurance EO " + addCat + includeSearchTerms + " WHERE E.Venue IS NULL AND E.ID=EO.EventID " +
                        country + state + city + zip + date + " AND E.Live='True' " +
                        allSearchTerms + highestP + categories + " ORDER BY E.Header";
                    }

                    //Page.Trace.Write("before db call");

                    //ErrorLabel.Text = searchStr;
                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    SqlCommand cmd = new SqlCommand(searchStr, conn);
                    if (Session["SetVenue"] == null)
                    {
                        if (!RadiusDropPanel.Visible)
                        {
                            cmd.Parameters.Add("@zip", SqlDbType.NVarChar).Value = zipParameter;
                        }
                        for (int i = 0; i < tokens.Length; i++)
                        {
                            cmd.Parameters.Add("@search" + i.ToString(), SqlDbType.NVarChar).Value = "%" + tokens[i] + "%";
                        }
                        if (state != "")
                            cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = stateParam;
                        if (city != "")
                            cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = cityParam;
                    }
                    else
                    {
                        for (int i = 0; i < tokens.Length; i++)
                        {
                            cmd.Parameters.Add("@search" + i.ToString(), SqlDbType.NVarChar).Value = "%" + tokens[i] + "%";
                        }
                    }
                    message = searchStr;
                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    //Page.Trace.Write("right before first fill ds: "+message);
                    da.Fill(ds);
                    //Page.Trace.Write("right after first fill");

                    

                    DataSet dsAll = ds;

                    //Get Venue Events
                    if (categories == "")
                    {
                        cmd = new SqlCommand(searchStrVen, conn);
                        if (theVenue == "")
                        {
                            if (!RadiusDropPanel.Visible)
                            {
                                cmd.Parameters.Add("@zip", SqlDbType.NVarChar).Value = zipParameter;
                            }
                            for (int i = 0; i < tokens.Length; i++)
                            {
                                cmd.Parameters.Add("@search" + i.ToString(), SqlDbType.NVarChar).Value = "%" + tokens[i] + "%";
                            }
                            if (state != "")
                                cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = stateParam;
                            if (city != "")
                                cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = cityParam;
                        }
                        else
                        {
                            for (int i = 0; i < tokens.Length; i++)
                            {
                                cmd.Parameters.Add("@search" + i.ToString(), SqlDbType.NVarChar).Value = "%" + tokens[i] + "%";
                            }
                        }
                        message = searchStrVen;
                        DataSet dsVen = new DataSet();
                        da = new SqlDataAdapter(cmd);

                        //Page.Trace.Write("right before second fill");
                        da.Fill(dsVen);
                        //Page.Trace.Write("right after second fill");

                        //Page.Trace.Write("right before first merge");
                        dsAll = MergeResults(ds, dsVen);
                        //Page.Trace.Write("right after first merge");
                    }

                    if (Session["SetVenue"] == null)
                    {
                        cmd = new SqlCommand(searchStrAddress, conn);
                        if (!RadiusDropPanel.Visible)
                        {
                            cmd.Parameters.Add("@zip", SqlDbType.NVarChar).Value = zipParameter;
                        }
                        for (int i = 0; i < tokens.Length; i++)
                        {
                            cmd.Parameters.Add("@search" + i.ToString(), SqlDbType.NVarChar).Value = "%" + tokens[i] + "%";
                        }
                        if (state != "")
                            cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = stateParam;
                        if (city != "")
                            cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = cityParam;

                        DataSet dsAd = new DataSet();
                        da = new SqlDataAdapter(cmd);

                        //Page.Trace.Write("right bfore third fill");
                        da.Fill(dsAd);
                        //Page.Trace.Write("right after third fill");

                        //Page.Trace.Write("right before second merge");
                        dsAll = MergeResults(dsAll, dsAd);
                        //Page.Trace.Write("right after second merge");
                    }

                    //Page.Trace.Write("after construct dsAll");
                    Session["SearchDS"] = dsAll;
                    Session["Searching"] = "Events";
                    Session["SearchResults"] = "Results " + resultsStr;
                    ResultsLabel.Text = "Search results " + resultsStr;
                    conn.Close();

                    DataColumn dc = new DataColumn("DateTimeStart");
                    dsAll.Tables[0].Columns.Add(dc);
                    dc = new DataColumn("ReoccurrID");
                    dsAll.Tables[0].Columns.Add(dc);

                    DataView dv = new DataView(dsAll.Tables[0], "", "", DataViewRowState.CurrentRows);
                    DataView dvEvent;
                    bool doOverRow = false;
                    foreach (DataRowView rowster in dv)
                    {
                        if (rowster["Type"].ToString() == "E")
                        {
                            dvEvent = dat.GetDataDV("SELECT *, EO.ID AS ReoccurrID FROM Events E, " +
                                "Event_Occurance EO WHERE E.ID=EO.EventID AND E.ID=" + rowster["EID"].ToString() + " ORDER BY DateTimeStart ASC");

                            foreach (DataRowView row in dvEvent)
                            {
                                if (DateTime.Parse(row["DateTimeStart"].ToString()) >= DateTime.Now)
                                {
                                    rowster["DateTimeStart"] = row["DateTimeStart"];
                                    rowster["ReoccurrID"] = row["ReoccurrID"];
                                    break;
                                }
                            }

                            if (rowster["DateTimeStart"] == null)
                                doOverRow = true;
                            else if (rowster["DateTimeStart"].ToString().Trim() == "")
                                doOverRow = true;

                            if (doOverRow)
                            {
                                rowster["DateTimeStart"] = dvEvent[dvEvent.Count - 1]["DateTimeStart"].ToString();
                                rowster["ReoccurrID"] = dvEvent[dvEvent.Count - 1]["ReoccurrID"].ToString();
                            }
                        }
                    }

                    //Page.Trace.Write("Search: before fill results");
                    bool setZero = false;

                    if (dsAll.Tables.Count > 0)
                        if (dsAll.Tables[0].Rows.Count > 0)
                        {
                        }
                        else
                        {
                            setZero = true;
                        }
                    else
                        setZero = true;
                    

                    if (setZero)
                    {
                        //SearchPanel.Items[1].Text = "<div style='position: relative;'><span style='font-family: Arial; font-size: 20px; color: White;'>Search Results: </span><div style='position: absolute; right: 5px; top: 3px;'>(0 Records Found)</div></div>";
                        //SortDropDown.Visible = false;
                        NumsLabel.Text = "0 Results Found";
                        Session["SearchDS"] = null;
                        NoResultsPanel.Visible = true;
                        MapPanel.Visible = false;
                        SortDropDown.Visible = false;
                    }
                    else
                    {
                        NumsLabel.Text = dv.Count + " Results Found";
                        NoResultsPanel.Visible = false;
                        Session["SearchDS"] = dsAll;
                        FillResults(dsAll);
                        MapPanel.Visible = true;
                        SortDropDown.Visible = true;
                    }

                    //Page.Trace.Write("Search: after fill results");

                    //ResultsLabel.Text = "Results "+resultsStr;

                    //Save search location base on IP if user not logged in.
                    
                    if (Session["User"] != null)
                    {
                        dat.SetLocationForIP(CountryDropDown.SelectedValue, cityParam, stateParam);
                    }
                }
                else
                {
                    MessageLabel.Text = message;
                }
            }
            catch (Exception ex)
            {
                MessageLabel.Text += ex.ToString() + "<br/>" + message;
            }
    }

    protected DataSet MergeResults(DataSet ds1, DataSet ds2)
    {
        DataView dv1 = new DataView(ds1.Tables[0], "", "", DataViewRowState.CurrentRows);
        DataView dv2 = new DataView(ds2.Tables[0], "", "", DataViewRowState.CurrentRows);

        DataView dvToUse = dv1;
        DataView dvOther = dv2;
        int theMax = dv1.Count;
        if (theMax < dv2.Count)
        {
            theMax = dv2.Count;
            dvToUse = dv2;
            dvOther = dv1;
        }

        DataSet dsNEW = new DataSet();
        dsNEW.Tables.Add(new DataTable());
        DataColumn dc = new DataColumn("Header");
        dsNEW.Tables[0].Columns.Add(dc);
        dc = new DataColumn("Type");
        dsNEW.Tables[0].Columns.Add(dc);
        dc = new DataColumn("Featured");
        dsNEW.Tables[0].Columns.Add(dc);
        dc = new DataColumn("DaysFeatured");
        dsNEW.Tables[0].Columns.Add(dc);
        dc = new DataColumn("EID");
        dsNEW.Tables[0].Columns.Add(dc);
        dc = new DataColumn("VID");
        dsNEW.Tables[0].Columns.Add(dc);
        dc = new DataColumn("Content");
        dsNEW.Tables[0].Columns.Add(dc);
        dc = new DataColumn("Name");
        dsNEW.Tables[0].Columns.Add(dc);

        DataRow dr;
        for (int i = 0; i < theMax; i++)
        {
            dr = dsNEW.Tables[0].NewRow();
            dr["Header"] = dvToUse[i]["Header"];
            dr["Type"] = dvToUse[i]["Type"];
            dr["Featured"] = dvToUse[i]["Featured"];
            dr["DaysFeatured"] = dvToUse[i]["DaysFeatured"];
            dr["EID"] = dvToUse[i]["EID"];
            dr["VID"] = dvToUse[i]["VID"];
            dr["Content"] = dvToUse[i]["Content"];
            dr["Name"] = dvToUse[i]["Name"];
            dsNEW.Tables[0].Rows.Add(dr);
            if (dvOther.Count >= i + 1)
            {
                dr = dsNEW.Tables[0].NewRow();
                dr["Header"] = dvOther[i]["Header"];
                dr["Type"] = dvOther[i]["Type"];
                dr["Featured"] = dvOther[i]["Featured"];
                dr["DaysFeatured"] = dvOther[i]["DaysFeatured"];
                dr["EID"] = dvOther[i]["EID"];
                dr["VID"] = dvOther[i]["VID"];
                dr["Content"] = dvOther[i]["Content"];
                dr["Name"] = dvOther[i]["Name"];
                dsNEW.Tables[0].Rows.Add(dr);
            }
        }

        return dsNEW;
    }

    protected void FillResults(DataSet ds)
    {
        TopLiteral.Text = "<script type=\"text/javascript\">function initialize1(){map = null;}</script>";
        BottomScriptLiteral.Text = "";
        ScriptLiteral.Text = "";


        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn);

        Data dat = new Data(isn);

        DataColumn dc = new DataColumn("SearchNum");
        if (!ds.Tables[0].Columns.Contains("SearchNum"))
            ds.Tables[0].Columns.Add(dc);


        int numofpages = 10;

        int countOfUniqueVenues = 0;

        string sortString = "";
        if (Session["sortString"] != null)
        {
            sortString = Session["sortString"].ToString();
        }

        DataView dv = TakeCareOfFeaturedEvents(ds, sortString);

        string[] mapStrings = GetEventLiteral(dv, dat, numofpages);

        if (Session["CountUniqueVenues"] != null)
        {
            countOfUniqueVenues = int.Parse(Session["CountUniqueVenues"].ToString());
        }

        EventSearchElements.COUNT_UNIQUE_VENUES = countOfUniqueVenues;
        EventSearchElements.CUT_OFF = 200;
        EventSearchElements.Visible = true;
        EventSearchElements.EVENTS_DS = ds;
        EventSearchElements.DO_MAP = true;
        EventSearchElements.DO_CALENDAR = false;
        EventSearchElements.MAP_STRINGS = mapStrings;
        EventSearchElements.NUM_OF_PAGES = numofpages;
        EventSearchElements.DataBind2();
    }

    /// <summary>
    /// TakeCareOfFeaturedEvents() functions make sure that featured events always come up on top.
    /// Up to 4 featured events are on top. So, first page will always contain the first four. 
    /// Next page the second four featured events, etc. 
    /// The order of the featured events cannot be biased. It has to be random. 
    /// On each run of this function, we assign random order numbers for the featured events.
    /// </summary>
    /// <param name="ds">The DataSet to order with featured events on top.</param>
    protected DataView TakeCareOfFeaturedEvents(DataSet ds, string sortString)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn);

        Data dat = new Data(isn);

        DateTime isNow = isn;
        string featureDate = isNow.Month.ToString() + "/" + isNow.Day.ToString() + "/" + isNow.Year.ToString();

        DataColumn dc = new DataColumn("colOrder");
        dc.DataType = typeof(int);
        if (!ds.Tables[0].Columns.Contains("colOrder"))
        {
            ds.Tables[0].Columns.Add(dc);
        }
        
        DataView dv = new DataView(ds.Tables[0], "", "", DataViewRowState.CurrentRows);

        if (sortString != "")
            dv.Sort = sortString;

        int countPages = dv.Count / 10;
        if (countPages * 10 < dv.Count)
            countPages++;

        dv.RowFilter = "Featured = 'True' AND DaysFeatured LIKE '%;" + featureDate + ";%'";
        int countFeatured = dv.Count;

        //int countFeaturedPerPage = countFeatured / countPages;

        int[] FeaturedMapping = new int[countFeatured];

        Random rand = new Random();

        int fCount = 0;
        foreach (DataRowView row in dv)
        {
            FeaturedMapping[fCount++] = int.Parse(row["EID"].ToString());
        }

        //shuffle array
        int randInt = 0;
        int temp = 0;
        for (int i = 0; i < FeaturedMapping.Length; i++)
        {
            randInt = rand.Next(0, countFeatured - 1);
            temp = FeaturedMapping[randInt];
            FeaturedMapping[randInt] = FeaturedMapping[i];
            FeaturedMapping[i] = temp;
        }

        dv.RowFilter = "";

        int count = 0;
        int featuredTakenCareOf = 0;
        int toFeature = 0;
        int normalRowCount = 0;
        int totalCount = dv.Count;
        int toNormal = 0;
        while (count < totalCount)
        {
            if (countFeatured > 0)
            {
                if (countFeatured - featuredTakenCareOf >= 4)
                    toFeature = 4;
                else
                    toFeature = countFeatured - featuredTakenCareOf;

                for (int i = featuredTakenCareOf; i < featuredTakenCareOf + toFeature; i++)
                {
                    dv.RowFilter = "EID = " + FeaturedMapping[i];
                    dv[0]["colOrder"] = count;
                    count++;
                }
                featuredTakenCareOf += toFeature;
            }

            dv.RowFilter = "NOT(Featured = 'True' AND DaysFeatured LIKE '%;" + featureDate + ";%') OR DaysFeatured IS NULL";

            toNormal = 10 - toFeature;
            if (dv.Count - normalRowCount < toNormal)
            {
                toNormal = dv.Count - normalRowCount;
            }

            for (int j = normalRowCount; j < toNormal + normalRowCount; j++)
            {
                dv[j]["colOrder"] = count;
                count++;
            }
            normalRowCount += toNormal;
        }

        dv.RowFilter = "";
        dv.Sort = "colOrder ASC";

        return dv;
    }

    protected string[] GetEventLiteral(DataView dvEvents, Data dat, int numOfRecordsPerPage)
    {
        string message = "";

        int startIndex = 0;
        //int pageNum = int.Parse(Request.QueryString["page"].ToString());
        //int endIndex = numOfRecordsPerPage - 1;

        //startIndex = numOfRecordsPerPage * pageNum;

        //if (endIndex > dvEvents.Count - 1)
        //    endIndex = dvEvents.Count - 1;


        //int thecount = numOfRecordsPerPage;
        //if (endIndex != numOfRecordsPerPage - 1)
        //    thecount = endIndex - startIndex;

        int numberInArray = dvEvents.Count / numOfRecordsPerPage;

        if (dvEvents.Count % numOfRecordsPerPage != 0)
            numberInArray++;
        string oneStringRecord = "";

        int normalRecordCount = 0;
        int countInArray = 0;
        string[] theArray = new string[numberInArray];
        TopLiteral.Text = "";

        string temp = "";

        try
        {
            Hashtable normalEventHash = new Hashtable();

            int i = 0;
            string funcCount = "";

            string theLiteral = "<script type=\"text/javascript\">function initialize" +
                (countInArray + 1).ToString() + "(){ initMap(); " +
               " if(map != null && map != undefined) " +
               " { var address;";
            string address = "";
            string venue = "";
            bool isFirstElement = true;
            string country = "US";

            Hashtable venueMapping = new Hashtable();
            Hashtable venuesNumMapping = new Hashtable();
            DataView dvRecords = new DataView();
            DataTable dt = new DataTable();
            DataColumn dc2 = new DataColumn("Address");
            dt.Columns.Add(dc2);
            dc2 = new DataColumn("Letter");
            dt.Columns.Add(dc2);
            int lastUsedCount = 0;

            dvEvents.Sort = "colOrder ASC";

            foreach(DataRowView row in dvEvents)
            {
                temp = "";
                if (normalRecordCount % numOfRecordsPerPage == 0)
                {
                    if (normalRecordCount != 0 && normalRecordCount != lastUsedCount)
                    {
                        lastUsedCount = normalRecordCount;
                        //ErrorLabel.Text += "on script insert normal rec count: " + normalRecordCount.ToString();
                        dvRecords = new DataView(dt, "", "Letter ASC", DataViewRowState.CurrentRows);
                        theLiteral = "<script type=\"text/javascript\">\n\rfunction initialize" +
                            (countInArray + 1).ToString() + "(){initMap(); if(map != null && map != undefined)\n\r " +
                           " { \n\rvar address;\n\r";
                        oneStringRecord = theLiteral;
                        int rowCount = 0;
                        foreach (DataRowView rowster in dvRecords)
                        {
                            if (rowCount == 0)
                            {
                                oneStringRecord += venueMapping[rowster["Address"].ToString()].ToString() +
                                "', true);".Replace("   ",
                                " ").Replace("  ", " ");

                                if (rowCount == dvRecords.Count - 1)
                                {
                                    oneStringRecord += " }}</script>".Replace("   ",
                                                                " ").Replace("  ", " ");
                                }
                            }
                            else if (rowCount == dvRecords.Count - 1)
                            {
                                oneStringRecord += venueMapping[rowster["Address"].ToString()].ToString() +
                                                            "', false); }}</script>".Replace("   ",
                                                            " ").Replace("  ", " ");
                            }
                            else
                            {
                                oneStringRecord += venueMapping[rowster["Address"].ToString()].ToString() +
                                                            "', false);".Replace("   ",
                                                            " ").Replace("  ", " ");
                            }
                            rowCount++;
                        }
                        TopLiteral.Text += oneStringRecord;
                        theArray[countInArray] = oneStringRecord;
                        dt.Rows.Clear();
                        countInArray++;
                        venueMapping.Clear();
                        venuesNumMapping.Clear();
                    }

                    venue = "";
                    isFirstElement = true;
                    i = 0;
                }


                address = "";
                DataView dvV = new DataView();
                if (row["VID"].ToString() != "NONE")
                {
                    dvV = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + row["VID"].ToString());
                }
                else
                {
                    dvV = dat.GetDataDV("SELECT * FROM Events WHERE ID=" + row["EID"].ToString());
                }

                DataView dvCountry = dat.GetDataDV("SELECT * FROM countries WHERE country_id=" + dvV[0]["Country"].ToString());
                country = dvCountry[0]["country_2_code"].ToString();

                if (country.ToLower() == "us")
                {
                    try
                    {
                        if (row["VID"].ToString() != "NONE")
                        {
                            address = dat.GetAddress(dvV[0]["Address"].ToString(), false);
                        }
                        else
                        {
                            address = dvV[0]["Address"].ToString();
                        }
                        
                    }
                    catch (Exception ex1)
                    {
                        address = "";
                    }
                }
                else
                {
                    if (row["VID"].ToString() != "NONE")
                    {
                        address = dat.GetAddress(dvV[0]["Address"].ToString(), true);
                    }
                    else
                    {
                        address = dvV[0]["Address"].ToString();
                    }
                    
                }

                if (!normalEventHash.Contains(row["EID"].ToString()))
                {
                    normalEventHash.Add(row["EID"].ToString(), normalRecordCount.ToString());

                    if (dvV[0]["Country"].ToString().ToLower() == "222")
                    {
                        //VenueName.Text + "@&" + 
                        if (venueMapping.ContainsKey(address))
                        {
                            row["SearchNum"] = venuesNumMapping[address].ToString();
                            if (row["Type"].ToString() == "E")
                            {
                                temp = row["Header"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ");
                                if (temp.Length > 50)
                                    temp = temp.Substring(0, 50) + "...";
                                venueMapping[address] += "<br/><div class=\\\"NavyLink12UD\\\" style=\\\" float: left; cursor: pointer;\\\" onclick=\\\"GoToThis(\\'" + "../" +
                                        dat.MakeNiceName(row["Header"].ToString()) + "_" +
                                        row["EID"].ToString() + "_Event" + "\\');\\\">" +
                                        dat.CleanExcelString(temp) + "</div>";
                            }
                            else
                            {
                                temp = row["Header"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ");
                                if (temp.Length > 50)
                                    temp = temp.Substring(0, 50) + "...";
                                venueMapping[address] += "<br/><div class=\\\"NavyLink12UD\\\" style=\\\" float: left; cursor: pointer;\\\" onclick=\\\"GoToThis(\\'" + "../" +
                                       dat.MakeNiceName(row["Header"].ToString()) + "_" +
                                       row["EID"].ToString() + "_Venue" + "\\');\\\">" +
                                       dat.CleanExcelString(temp) + "</div>";
                            }
                        }
                        else
                        {
                            if (!isFirstElement)
                            {
                                i++;
                                venuesNumMapping.Add(address, dat.GetImage(i.ToString()));
                                if (row["Type"].ToString() == "E")
                                {
                                    temp = row["Header"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ");
                                    if (temp.Length > 50)
                                        temp = temp.Substring(0, 50) + "...";
                                    venueMapping.Add(address, "\n\raddress =  '" + address.Trim().Replace("'", "''").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                                        dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                                        country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; " +
                                        "\n\rshowAddressUS('<span  class=\\\"Green12Link2UD\\\"  onclick=\\\"GoToThis(\\'" + "../" +
                                        dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                                        row["VID"].ToString() + "_Venue" + "\\');\\\">" +
                                        dvV[0]["Name"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ") + "</span>', 0, address, 0, address, 1, " + i.ToString() + ", '" +
                                        "<div  class=\\\"NavyLink12UD\\\"  onclick=\\\"GoToThis(\\'" + "../" +
                                        dat.MakeNiceName(row["Header"].ToString()) + "_" +
                                        row["EID"].ToString() + "_Event" + "\\');\\\">" +
                                        dat.CleanExcelString(temp) + "</div>");
                                }
                                else
                                {
                                    temp = row["Header"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ");
                                    if (temp.Length > 50)
                                        temp = temp.Substring(0, 50) + "...";
                                    venueMapping.Add(address, "\n\raddress =  '" + address.Trim().Replace("'", "''").Replace("(", " ").Replace(")", " ") + " " +
                                       dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                                       dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                                       country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; " +
                                       "\n\rshowAddressUS('<span  class=\\\"Green12Link2UD\\\"  onclick=\\\"GoToThis(\\'" + "../" +
                                       dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                                       row["VID"].ToString() + "_Venue" + "\\');\\\">" +
                                       dvV[0]["Name"].ToString().Replace("'",
                                       " ").Replace("(", " ").Replace(")", " ") + "</span>', 0, address, 0, address, 1, " + i.ToString() + ", '" +
                                       "<div  class=\\\"NavyLink12UD\\\"  onclick=\\\"GoToThis(\\'" + "../" +
                                       dat.MakeNiceName(row["Header"].ToString()) + "_" +
                                       row["VID"].ToString() + "_Venue" + "\\');\\\">" +
                                       dat.CleanExcelString(temp) + "</div>");
                                }
                                row["SearchNum"] = dat.GetImage(i.ToString());
                                DataRow r = dt.NewRow();
                                r["Address"] = address;
                                r["Letter"] = dat.GetImage(i.ToString());
                                dt.Rows.Add(r);
                            }
                            else
                            {
                                DataRow r = dt.NewRow();
                                r["Address"] = address;
                                r["Letter"] = dat.GetImage(i.ToString());
                                dt.Rows.Add(r);
                                row["SearchNum"] = dat.GetImage(i.ToString());
                                venuesNumMapping.Add(address, dat.GetImage(i.ToString()));

                                temp = row["Header"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ");
                                if (temp.Length > 50)
                                    temp = temp.Substring(0, 50) + "...";

                                if (row["Type"].ToString() == "E")
                                {
                                    venueMapping.Add(address, "\n\raddress =  '" + address.Trim().Replace("'", "''").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                                        dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                                        country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; \n\rshowAddressUS('<span  class=\\\"Green12LinkNFUD\\\"  onclick=\\\"GoToThis(\\'" + "../" +
                                        dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                                        row["VID"].ToString() + "_Venue" + "\\');\\\">" +
                                        dvV[0]["Name"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ") + "</span>',0, address, 0, address, 1, " + i.ToString() + ", '" +
                                        "<div class=\\\"NavyLink12UD\\\"  onclick=\\\"GoToThis(\\'" + "../" +
                                        dat.MakeNiceName(row["Header"].ToString()) + "_" +
                                        row["EID"].ToString() + "_Event" + "\\');\\\">" +
                                        dat.CleanExcelString(temp) + "</div>");
                                }
                                else
                                {
                                    venueMapping.Add(address, "\n\raddress =  '" + address.Trim().Replace("'", "''").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                                        dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                                        country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; \n\rshowAddressUS('<span  class=\\\"Green12LinkNFUD\\\"  onclick=\\\"GoToThis(\\'" + "../" +
                                        dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                                        row["VID"].ToString() + "_Venue" + "\\');\\\">" +
                                        dvV[0]["Name"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ") + "</span>',0, address, 0, address, 1, " + i.ToString() + ", '" +
                                        "<div class=\\\"NavyLink12UD\\\"  onclick=\\\"GoToThis(\\'" + "../" +
                                        dat.MakeNiceName(row["Header"].ToString()) + "_" +
                                        row["VID"].ToString() + "_Venue" + "\\');\\\">" +
                                        dat.CleanExcelString(temp) + "</div>");
                                }
                            }
                        }

                    }
                    else
                    {
                        //VenueName.Text + "@&" + 
                        if (venueMapping.ContainsKey(address))
                        {
                            row["SearchNum"] = venuesNumMapping[address].ToString();
                            temp = row["Header"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ");
                                if (temp.Length > 50)
                                    temp = temp.Substring(0, 50) + "...";
                            if (row["Type"].ToString() == "E")
                            {
                                venueMapping[address] += "<br/><div class=\\\"NavyLink12UD\\\"  onclick=\\\"GoToThis(\\'" + "../" +
                                        dat.MakeNiceName(row["Header"].ToString()) + "_" +
                                        row["EID"].ToString() + "_Event" + "\\');\\\">" +
                                        dat.CleanExcelString(temp) + "</div>";
                            }
                            else
                            {
                                venueMapping[address] += "<br/><div class=\\\"NavyLink12UD\\\"  onclick=\\\"GoToThis(\\'" + "../" +
                                        dat.MakeNiceName(row["Header"].ToString()) + "_" +
                                        row["VID"].ToString() + "_Venue" + "\\');\\\">" +
                                        dat.CleanExcelString(temp) + "</div>";
                            }
                        }
                        else
                        {
                            if (!isFirstElement)
                            {
                                i++;
                                venuesNumMapping.Add(address, dat.GetImage(i.ToString()));
                                temp = row["Header"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ");
                                if (temp.Length > 50)
                                    temp = temp.Substring(0, 50) + "...";
                                if (row["Type"].ToString() == "E")
                                {
                                    string tmp = "\n\raddress =  '" + address.Trim().Replace("'", "''").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["State"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        country.Replace("'", " ").Replace("(", " ").Replace(")", " ");

                                    if (row["VID"].ToString() == "NONE")
                                    {
                                        tmp += "'; \n\rshowAddressUS('<span class=\\\"Green12LinkNFUD\\\">" +
                                        dvV[0]["Address"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ") + "</span>',0, address, 0, address, 1, " + i.ToString() + ", '" +
                                        "<div class=\\\"NavyLink12UD\\\" onclick=\\\"GoToThis(\\'" + "../" +
                                        dat.MakeNiceName(row["Header"].ToString()) + "_" +
                                        row["EID"].ToString() + "_Event" + "\\');\\\">" +
                                       dat.CleanExcelString(temp) + "</div>";
                                    }
                                    else
                                    {
                                        tmp += "'; \n\rshowAddressUS('<span class=\\\"Green12LinkNFUD\\\" onclick=\\\"GoToThis(\\'" + "../" +
                                        dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                                        row["VID"].ToString() + "_Venue" + "\\');\\\">" +
                                        dvV[0]["Name"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ") + "</span>',0, address, 0, address, 1, " + i.ToString() + ", '" +
                                        "<div class=\\\"NavyLink12UD\\\" onclick=\\\"GoToThis(\\'" + "../" +
                                        dat.MakeNiceName(row["Header"].ToString()) + "_" +
                                        row["EID"].ToString() + "_Event" + "\\');\\\">" +
                                       dat.CleanExcelString(temp) + "</div>";
                                    }

                                    venueMapping.Add(address, tmp);
                                }
                                else
                                {
                                    venueMapping.Add(address, "\n\raddress =  '" + address.Trim().Replace("'", "''").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["State"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; \n\rshowAddressUS('<span class=\\\"Green12LinkNFUD\\\" onclick=\\\"GoToThis(\\'" + "../" +
                                        dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                                        row["VID"].ToString() + "_Venue" + "\\');\\\">" +
                                        dvV[0]["Name"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ") + "</span>',0, address, 0, address, 1, " + i.ToString() + ", '" +
                                        "<div class=\\\"NavyLink12UD\\\" onclick=\\\"GoToThis(\\'" + "../" +
                                        dat.MakeNiceName(row["Header"].ToString()) + "_" +
                                        row["VID"].ToString() + "_Venue" + "\\');\\\">" +
                                       dat.CleanExcelString(temp) + "</div>");
                                }
                                DataRow r = dt.NewRow();
                                r["Address"] = address;
                                r["Letter"] = dat.GetImage(i.ToString());
                                dt.Rows.Add(r);
                                row["SearchNum"] = dat.GetImage(i.ToString());
                            }
                            else
                            {
                                temp = row["Header"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ");
                                if (temp.Length > 50)
                                    temp = temp.Substring(0, 50) + "...";

                                row["SearchNum"] = dat.GetImage(i.ToString());
                                DataRow r = dt.NewRow();
                                r["Address"] = address;
                                r["Letter"] = dat.GetImage(i.ToString());
                                dt.Rows.Add(r);
                                venuesNumMapping.Add(address, dat.GetImage(i.ToString()));
                                if (row["Type"].ToString() == "E")
                                {
                                    string tmp = "\n\raddress =  '" + address.Trim().Replace("'", "''").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["State"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'";

                                    if (row["VID"].ToString() == "NONE")
                                    {
                                        tmp += " \n\rshowAddressUS('<span class=\\\"Green12LinkNFUD\\\" >" +
                                        dvV[0]["Address"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ") + "</span>',0, address, 0, address, 1, " + i.ToString() + ", '" +
                                        "<div class=\\\"NavyLink12UD\\\"  onclick=\\\"GoToThis(\\'" + "../" +
                                        dat.MakeNiceName(row["Header"].ToString()) + "_" +
                                        row["EID"].ToString() + "_Event" + "\\');\\\">" +
                                        dat.CleanExcelString(temp) + "</div>";
                                    }
                                    else
                                    {
                                        tmp += " \n\rshowAddressUS('<span class=\\\"Green12LinkNFUD\\\" onclick=\\\"GoToThis(\\'" + "../" +
                                        dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                                        row["VID"].ToString() + "_Venue" + "\\');\\\">" +
                                        dvV[0]["Name"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ") + "</span>',0, address, 0, address, 1, " + i.ToString() + ", '" +
                                        "<div class=\\\"NavyLink12UD\\\"  onclick=\\\"GoToThis(\\'" + "../" +
                                        dat.MakeNiceName(row["Header"].ToString()) + "_" +
                                        row["EID"].ToString() + "_Event" + "\\');\\\">" +
                                        dat.CleanExcelString(temp) + "</div>";
                                    }

                                    venueMapping.Add(address, tmp);
                                }
                                else
                                {
                                    venueMapping.Add(address, "\n\raddress =  '" + address.Trim().Replace("'", "''").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["State"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; \n\rshowAddressUS('<span class=\\\"Green12LinkNFUD\\\" onclick=\\\"GoToThis(\\'" + "../" +
                                        dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                                        row["VID"].ToString() + "_Venue" + "\\');\\\">" +
                                        dvV[0]["Name"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ") + "</span>',0, address, 0, address, 1, " + i.ToString() + ", '" +
                                        "<div class=\\\"NavyLink12UD\\\"  onclick=\\\"GoToThis(\\'" + "../" +
                                        dat.MakeNiceName(row["Header"].ToString()) + "_" +
                                        row["VID"].ToString() + "_Venue" + "\\');\\\">" +
                                        dat.CleanExcelString(temp) + "</div>");
                                }
                            }
                        }

                    }
                    isFirstElement = false;
                    normalRecordCount++;
                    venue = address;
                }
                else
                {
                    if (venueMapping.ContainsKey(address))
                    {
                        temp = row["Header"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ");
                                if (temp.Length > 50)
                                    temp = temp.Substring(0, 50) + "...";
                        row["SearchNum"] = venuesNumMapping[address].ToString();
                        if (row["Type"].ToString() == "E")
                        {
                            venueMapping[address] += "<br/><div class=\\\"NavyLink12UD\\\"  onclick=\\\"GoToThis(\\'" + "../" +
                                    dat.MakeNiceName(row["Header"].ToString()) + "_" +
                                    row["EID"].ToString() + "_Event" + "\\');\\\">" +
                                    dat.CleanExcelString(temp) + "</div>";
                        }
                        else
                        {
                            venueMapping[address] += "<br/><div class=\\\"NavyLink12UD\\\"  onclick=\\\"GoToThis(\\'" + "../" +
                                    dat.MakeNiceName(row["Header"].ToString()) + "_" +
                                    row["VID"].ToString() + "_Venue" + "\\');\\\">" +
                                    dat.CleanExcelString(temp) + "</div>";
                        }
                    }
                    else
                    {
                        temp = row["Header"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ");
                                if (temp.Length > 50)
                                    temp = temp.Substring(0, 50) + "...";
                        if (!isFirstElement)
                        {
                            i++;
                            venuesNumMapping.Add(address, dat.GetImage(i.ToString()));
                            if (row["Type"].ToString() == "E")
                            {
                                string tmp = "\n\raddress =  '" + address.Trim().Replace("'", "''").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["State"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        country.Replace("'", " ").Replace("(", " ").Replace(")", " ");

                                if (row["VID"].ToString() == "NONE")
                                {
                                    tmp += "'; \n\rshowAddressUS('<span class=\\\"Green12LinkNFUD\\\">" +
                                    dvV[0]["Address"].ToString().Replace("'",
                                    " ").Replace("(", " ").Replace(")", " ") + "</span>',0, address, 0, address, 1, " + i.ToString() + ", '" +
                                    "<div class=\\\"NavyLink12UD\\\" onclick=\\\"GoToThis(\\'" + "../" +
                                    dat.MakeNiceName(row["Header"].ToString()) + "_" +
                                    row["EID"].ToString() + "_Event" + "\\');\\\">" +
                                   dat.CleanExcelString(temp) + "</div>";
                                }
                                else
                                {
                                    tmp += "'; \n\rshowAddressUS('<span class=\\\"Green12LinkNFUD\\\" onclick=\\\"GoToThis(\\'" + "../" +
                                    dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                                    row["VID"].ToString() + "_Venue" + "\\');\\\">" +
                                    dvV[0]["Name"].ToString().Replace("'",
                                    " ").Replace("(", " ").Replace(")", " ") + "</span>',0, address, 0, address, 1, " + i.ToString() + ", '" +
                                    "<div class=\\\"NavyLink12UD\\\" onclick=\\\"GoToThis(\\'" + "../" +
                                    dat.MakeNiceName(row["Header"].ToString()) + "_" +
                                    row["EID"].ToString() + "_Event" + "\\');\\\">" +
                                   dat.CleanExcelString(temp) + "</div>";
                                }

                                venueMapping.Add(address, tmp);
                            }
                            else
                            {
                                venueMapping.Add(address, "\n\raddress =  '" + address.Trim().Replace("'", "''").Replace("(", " ").Replace(")", " ") + " " +
                                    dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                    dvV[0]["State"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                    dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                    country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; \n\rshowAddressUS('<span class=\\\"Green12LinkNFUD\\\" onclick=\\\"GoToThis(\\'" + "../" +
                                    dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                                    row["VID"].ToString() + "_Venue" + "\\');\\\">" +
                                    dvV[0]["Name"].ToString().Replace("'",
                                    " ").Replace("(", " ").Replace(")", " ") + "</span>',0, address, 0, address, 1, " + i.ToString() + ", '" +
                                    "<div class=\\\"NavyLink12UD\\\" onclick=\\\"GoToThis(\\'" + "../" +
                                    dat.MakeNiceName(row["Header"].ToString()) + "_" +
                                    row["VID"].ToString() + "_Venue" + "\\');\\\">" +
                                   dat.CleanExcelString(temp) + "</div>");
                            }
                            DataRow r = dt.NewRow();
                            r["Address"] = address;
                            r["Letter"] = dat.GetImage(i.ToString());
                            dt.Rows.Add(r);
                            row["SearchNum"] = dat.GetImage(i.ToString());
                        }
                        else
                        {
                            row["SearchNum"] = dat.GetImage(i.ToString());
                            DataRow r = dt.NewRow();
                            r["Address"] = address;
                            r["Letter"] = dat.GetImage(i.ToString());
                            dt.Rows.Add(r);
                            venuesNumMapping.Add(address, dat.GetImage(i.ToString()));
                            if (row["Type"].ToString() == "E")
                            {
                                string tmp = "\n\raddress =  '" + address.Trim().Replace("'", "''").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["State"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'";

                                if (row["VID"].ToString() == "NONE")
                                {
                                    tmp += " \n\rshowAddressUS('<span class=\\\"Green12LinkNFUD\\\" >" +
                                    dvV[0]["Address"].ToString().Replace("'",
                                    " ").Replace("(", " ").Replace(")", " ") + "</span>',0, address, 0, address, 1, " + i.ToString() + ", '" +
                                    "<div class=\\\"NavyLink12UD\\\"  onclick=\\\"GoToThis(\\'" + "../" +
                                    dat.MakeNiceName(row["Header"].ToString()) + "_" +
                                    row["EID"].ToString() + "_Event" + "\\');\\\">" +
                                    dat.CleanExcelString(temp) + "</div>";
                                }
                                else
                                {
                                    tmp += " \n\rshowAddressUS('<span class=\\\"Green12LinkNFUD\\\" onclick=\\\"GoToThis(\\'" + "../" +
                                    dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                                    row["VID"].ToString() + "_Venue" + "\\');\\\">" +
                                    dvV[0]["Name"].ToString().Replace("'",
                                    " ").Replace("(", " ").Replace(")", " ") + "</span>',0, address, 0, address, 1, " + i.ToString() + ", '" +
                                    "<div class=\\\"NavyLink12UD\\\"  onclick=\\\"GoToThis(\\'" + "../" +
                                    dat.MakeNiceName(row["Header"].ToString()) + "_" +
                                    row["EID"].ToString() + "_Event" + "\\');\\\">" +
                                    dat.CleanExcelString(temp) + "</div>";
                                }

                                venueMapping.Add(address, tmp);
                            }
                            else
                            {
                                venueMapping.Add(address, "\n\raddress =  '" + address.Trim().Replace("'", "''").Replace("(", " ").Replace(")", " ") + " " +
                                    dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                    dvV[0]["State"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                    dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                    country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; \n\rshowAddressUS('<span class=\\\"Green12LinkNFUD\\\" onclick=\\\"GoToThis(\\'" + "../" +
                                    dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                                    row["VID"].ToString() + "_Venue" + "\\');\\\">" +
                                    dvV[0]["Name"].ToString().Replace("'",
                                    " ").Replace("(", " ").Replace(")", " ") + "</span>',0, address, 0, address, 1, " + i.ToString() + ", '" +
                                    "<div class=\\\"NavyLink12UD\\\"  onclick=\\\"GoToThis(\\'" + "../" +
                                    dat.MakeNiceName(row["Header"].ToString()) + "_" +
                                    row["VID"].ToString() + "_Venue" + "\\');\\\">" +
                                    dat.CleanExcelString(temp) + "</div>");
                            }
                        }
                    }
                    isFirstElement = false;
                    normalRecordCount++;
                    venue = address;
                }
            }
            //if (thecount % numOfRecordsPerPage != 0)
            //{
            dvRecords = new DataView(dt, "", "Letter ASC", DataViewRowState.CurrentRows);
            theLiteral = "<script type=\"text/javascript\">\n\rfunction initialize" +
                (countInArray + 1).ToString() + "(){initMap(); if(map != null && map != undefined) " +
               " { \n\rvar address;";
            oneStringRecord = theLiteral;
            int theCount = 0;
            foreach (DataRowView rowster in dvRecords)
            {
                if (theCount == 0)
                {
                    oneStringRecord += venueMapping[rowster["Address"].ToString()].ToString() +
                    "', true);".Replace("   ",
                    " ").Replace("  ", " ");

                    if (theCount == dvRecords.Count - 1)
                    {
                        oneStringRecord += " }}</script>".Replace("   ",
                                                    " ").Replace("  ", " ");
                    }
                }
                else if (theCount == dvRecords.Count - 1)
                {
                    oneStringRecord += venueMapping[rowster["Address"].ToString()].ToString() +
                                                "', false); }}</script>".Replace("   ",
                                                " ").Replace("  ", " ");
                }
                else
                {
                    oneStringRecord += venueMapping[rowster["Address"].ToString()].ToString() +
                                                "', false);".Replace("   ",
                                                " ").Replace("  ", " ");
                }
                theCount++;
            }
            TopLiteral.Text += oneStringRecord;
            theArray[countInArray] = oneStringRecord;
            //}


        }
        catch (Exception ex)
        {
            ErrorLabel.Text += ex.ToString() + message;
        }

        return theArray;
    }

    protected void SortResults(object sender, EventArgs e)
    {
        if (SortDropDown.SelectedItem.Value != "-1")
            Session["sortString"] = SortDropDown.SelectedItem.Value;
        else
            Session["sortString"] = null;

        FillResults((DataSet)Session["SearchDS"]);
    }

    protected DataSet MergeDS(DataSet ds, DataSet dsGroup)
    {
        DataTable dt = ds.Tables[0];

        DataRow newRow;
        foreach (DataRow row in dsGroup.Tables[0].Rows)
        {
            newRow = dt.NewRow();
            newRow["EID"] = row["EID"];
            newRow["DateTimeStart"] = row["DateTimeStart"];
            newRow["Header"] = row["Header"];
            newRow["isGroup"] = row["isGroup"];
            newRow["ReoccurrID"] = row["ReoccurrID"];
            newRow["CalendarKey"] = row["CalendarKey"];
            dt.Rows.Add(newRow);
        }

        return ds;
    }

    protected void ChangeImage1(object sender, EventArgs e)
    {
        ImageButton ImageButton1 = (ImageButton)FindControlRecursive(this, "ImageButton1");
        ImageButton ImageButton2 = (ImageButton)FindControlRecursive(this, "ImageButton2");

        if (ImageButton1.ImageUrl == "image/RadioButton.png")
        {
            ImageButton1.ImageUrl = "image/RadioButtonSelected.png";
            ImageButton2.ImageUrl = "image/RadioButton.png";
        }
        else
        {
            ImageButton1.ImageUrl = "image/RadioButton.png";
            ImageButton2.ImageUrl = "image/RadioButton.png";
        }
    }

    protected void ChangeImage2(object sender, EventArgs e)
    {
        ImageButton ImageButton2 = (ImageButton)FindControlRecursive(this, "ImageButton2");
        ImageButton ImageButton1 = (ImageButton)FindControlRecursive(this, "ImageButton1");

        if (ImageButton2.ImageUrl == "image/RadioButton.png")
        {
            ImageButton2.ImageUrl = "image/RadioButtonSelected.png";
            ImageButton1.ImageUrl = "image/RadioButton.png";
        }
        else
        {
            ImageButton2.ImageUrl = "image/RadioButton.png";
            ImageButton1.ImageUrl = "image/RadioButton.png";
        }
    }

    public Control FindControlRecursive(Control Root, string Id)
    {
        if (Root.ID == Id)
            return Root;

        foreach (Control Ctl in Root.Controls)
        {
            Control FoundCtl = FindControlRecursive(Ctl, Id);

            if (FoundCtl != null)
                return FoundCtl;
        }
        return null;
    }

    protected void ChangeState(object sender, EventArgs e)
    {


        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn);

        Data dat = new Data(isn);

        DataSet ds = dat.GetData("SELECT * FROM State WHERE country_id=" + CountryDropDown.SelectedValue);

        bool isTextBox = false;
        if (ds.Tables.Count > 0)
            if (ds.Tables[0].Rows.Count > 0)
            {
                StateDropDownPanel.Visible = true;
                StateTextBoxPanel.Visible = false;
                StateDropDown.DataSource = ds;
                StateDropDown.DataTextField = "state_2_code";
                StateDropDown.DataValueField = "state_id";
                StateDropDown.DataBind();

                StateDropDown.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("State", "-1"));

                DataSet ds2 = dat.GetData("SELECT  CASE WHEN SUBSTRING(Name, 1, 4) = 'The' THEN SUBSTRING(Name, "+
                    "5, LEN(Name)-4) ELSE Name END AS Name1, * FROM Venues WHERE Country=" + 
                    CountryDropDown.SelectedValue + " ORDER BY Name1 ASC");

                //VenueDropDown.Items.Clear();
                if (ds2.Tables.Count > 0)
                    if (ds2.Tables[0].Rows.Count > 0)
                    {
                        Session["LocationVenues"] = ds;

                        fillVenues(ds);
                    }
            }
            else
                isTextBox = true;
        else
            isTextBox = true;

        if (isTextBox)
        {
            StateTextBoxPanel.Visible = true;
            StateDropDownPanel.Visible = false;
        }

        //SearchPanel.Items[0].Expanded = true;
        //SearchPanel.Items[1].Expanded = false;
    }

    protected void StateChanged(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn);

        Data dat = new Data(isn);
        string country = " AND V.Country=" + CountryDropDown.SelectedValue;

        string state = "";
        string stateParam = "";
        string city = "";
        string cityParam = "";

        string zip = "";
        string zipParameter = "";
        int zipParam = 0;
        string nonExistantZip = "";

        bool goOn = true;

        bool gotNoState = true;

        if (StateDropDownPanel.Visible)
        {
            if (StateDropDown.SelectedValue != "-1")
            {
                gotNoState = false;
            }
        }
        else
        {
            if (StateTextBox.Text != "")
            {
                gotNoState = false;
            }
        }

        bool isZip = false;


            if (CityTextBox.Text.Trim() == "")
                isZip = true;


        if ((!isZip && (CityTextBox.Text.Trim() == "" || gotNoState)) || (ZipTextBox.Text.Trim() == "" && isZip))
        {
            goOn = false;
        }

        if (goOn)
        {
            




            if (isZip)
            {
                if (ZipTextBox.Text.Trim() != "")
                {
                    zip = " AND V.Zip = @zip ";
                    zipParameter = ZipTextBox.Text.Trim();
                    //do only if United States and not for international zip codes
                    if (RadiusDropPanel.Visible)
                    {
                        //Get all zips within the specified radius

                        if (int.TryParse(ZipTextBox.Text.Trim(), out zipParam))
                        {
                            DataSet dsLatsLongs = dat.GetData("SELECT Latitude, Longitude FROM ZipCodes WHERE Zip='" +
                                zipParam + "'");

                            //some zip codes don't exist in the database, find the closest one
                            bool findClosest = false;
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
                                zip = " AND V.Zip = '" + zipParam.ToString() + "' ";
                                nonExistantZip = " OR V.Zip = '" + zipParam.ToString() + "'";
                                while (dsLatsLongs == null)
                                {
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

                            //get all the zip codes within the specified radius
                            DataSet dsZips = dat.GetData("SELECT Zip FROM ZipCodes WHERE dbo.GetDistance(" + dsLatsLongs.Tables[0].Rows[0]["Longitude"].ToString() +
                                ", " + dsLatsLongs.Tables[0].Rows[0]["Latitude"].ToString() + ", Longitude, Latitude) < " + RadiusDropDown.SelectedValue);

                            if (dsZips.Tables.Count > 0)
                            {
                                if (dsZips.Tables[0].Rows.Count > 0)
                                {
                                    zip = " AND (V.Zip = '" + zipParam.ToString() + "' " + nonExistantZip;
                                    for (int i = 0; i < dsZips.Tables[0].Rows.Count; i++)
                                    {
                                        zip += " OR V.Zip = '" + dsZips.Tables[0].Rows[i]["Zip"].ToString() + "' ";
                                    }
                                    zip += ") ";
                                }
                            }



                        }
                        else
                        {
                            goOn = false;
                        }
                    }
                }
                else
                {
                    goOn = false;
                }
            }
            else
            {
                if (StateDropDownPanel.Visible)
                {
                    if (StateDropDown.SelectedValue != "-1")
                    {
                        state = " AND V.State=@state ";
                        stateParam = StateDropDown.SelectedItem.Text;
                    }
                }
                else
                {
                    if (StateTextBox.Text != "")
                    {
                        state = " AND V.State=@state ";
                        stateParam = StateTextBox.Text;
                    }
                }

                if (CityTextBox.Text != "")
                {
                    city = " AND V.City LIKE @city ";
                    cityParam = CityTextBox.Text.ToLower();
                }
            }



            string searchStr = "SELECT CASE WHEN SUBSTRING(Name, 1, 4) = 'The' THEN SUBSTRING(Name, 5, LEN(Name)-4) "+
                "ELSE Name END AS Name1, * FROM Venues V WHERE V.Live='True' " + country + state + city + zip + 
                " ORDER BY Name1 ASC";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
            if (conn.State != ConnectionState.Open)
                conn.Open();
            SqlCommand cmd = new SqlCommand(searchStr, conn);
            if (isZip)
            {
                cmd.Parameters.Add("@zip", SqlDbType.NVarChar).Value = zipParameter;
            }
            if (state != "")
                cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = stateParam;
            if (city != "")
                cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = "%" + cityParam + "%";

            DataSet ds2 = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds2);
            conn.Close();


            if (ds2.Tables.Count > 0)
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    Session["LocationVenues"] = ds2;

                    fillVenues(ds2);
                }
        }
        else
        {
            MessageLabel.Text = "You must at the very least include a zip code or State and City.";
            //SearchPanel.Items[1].Expanded = false;
            //SearchPanel.Items[0].Expanded = true;
        }
    }

    protected void fillVenues(DataSet ds)
    {
        try
        {
            DataView dv = new DataView(ds.Tables[0], "", "", DataViewRowState.CurrentRows);
            //HtmlGenericControl Div2 = (HtmlGenericControl)SearchPanel.Items[0].Items[0].FindControl("Div2");
            //Telerik.Web.UI.RadToolTip Tip1 = (Telerik.Web.UI.RadToolTip)SearchPanel.Items[0].Items[0].FindControl("Tip1");

            Div2.Visible = true;
            Tip1.Controls.Clear();
            if (ds.Tables.Count > 0)
                if (ds.Tables[0].Rows.Count > 0)
                {

                    //VenueDropDown.Visible = true;
                    //VenueDropDown.DataSource = ds;
                    //VenueDropDown.DataTextField = "Name";
                    //VenueDropDown.DataValueField = "ID";
                    //VenueDropDown.DataBind();

                    //VenueDropDown.Items.Insert(0, new ListItem("Select Venue >..", "-1"));
                    Literal litBeg = new Literal();
                    litBeg.Text = "<div style=\"max-width: 800px;\"><div style=\"clear: both;\">";
                    Literal litMid = new Literal();
                    litMid.Text = "";

                    char[] letters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
                    char[] lettersLow = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

                    int countLetters = 0;
                    int max = 0;

                    for (int j = 0; j < letters.Length; j++)
                    {
                        dv.RowFilter = "substring(Name1,1,1) = '" + lettersLow[j] + "' OR substring(Name1,1,1) = '" + letters[j] + "' ";


                        if (dv.Count > 0)
                        {
                            countLetters++;

                            string classL = "";
                            if (countLetters == 1)
                                classL = "Green12Link";
                            else
                                classL = "Blue12Link";
                            litBeg.Text += "<div style=\"float: left; padding: 5px;\" id='titleDiv" + letters[j] +
                                "' class=\"" + classL + "\"><a onclick=\"SelectLetterDiv('" +
                                letters[j] + "');\">" + letters[j] + "</a></div>";
                            string displayL = "";
                            if (countLetters == 1)
                                displayL = "block";
                            else
                                displayL = "none";
                            litMid.Text += "<div id='contentDiv" + letters[j] + "' style=\"display: " +
                                displayL + "; float: left;\">";
                            for (int i = 0; i < dv.Count; i++)
                            {
                                string borderN = "";
                                if (i != dv.Count - 1)
                                    borderN = "border-right: solid 1px white;";
                                litMid.Text += "<div style=\"" + borderN + "padding: 5px; float: left; width: 100px;\"><a onclick=\"fillVen('" +
                                    dv[i]["Name"].ToString().Replace("'", "&&") + "', " +
                                    dv[i]["ID"].ToString() + ");\" class=\"Blue12Link\">" +
                                    dv[i]["Name"].ToString() + "</a></div>";
                            }
                            litMid.Text += "</div>";

                            if (dv.Count > max)
                                max = dv.Count;
                        }
                    }
                    litBeg.Text += "</div><br/>";
                    litMid.Text += "</div>";

                    //Tip1.Width = countLetters * 34;
                    //Tip1.Height = max * 10;
                    Tip1.Controls.Add(litBeg);
                    Tip1.Controls.Add(litMid);
                }
                else
                {
                    Literal litMid = new Literal();
                    litMid.Text = "No venues found for this location. Create one my clicking on 'New Venue' radio button.";

                    Tip1.Controls.Add(litMid);

                }
            else
            {
                Literal litMid = new Literal();
                litMid.Text = "No venues found for this location. Create one my clicking on 'New Venue' radio button.";

                Tip1.Controls.Add(litMid);

            }
        }
        catch (Exception ex)
        {

        }
    }

    //protected void SortResults(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (Session["SearchDS"] != null)
    //        {
    //            DropDownList SortDropDown = (DropDownList)SearchPanel.Items[1].Items[0].FindControl("SortDropDown");
    //            //SearchElements SearchElements2 = (SearchElements)SearchPanel.Items[1].Items[0].FindControl("SearchElements2");

    //            if (SortDropDown.SelectedValue != "-1")
    //            {
    //                Session["sortString"] = SortDropDown.SelectedValue;

    //                HttpCookie cookie = Request.Cookies["BrowserDate"];
    //                if (cookie == null)
    //                {
    //                    cookie = new HttpCookie("BrowserDate");
    //                    cookie.Value = DateTime.Now.ToString();
    //                    cookie.Expires = DateTime.Now.AddDays(22);
    //                    Response.Cookies.Add(cookie);
    //                }
    //                Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //                DataSet ds;

    //                ds = (DataSet)Session["SearchDS"];
    //                SearchElements2.EVENTS_DS = ds;
    //                SearchElements2.IS_WINDOW = true;
    //                SearchElements2.SORT_STR = Session["sortString"].ToString();
    //                SearchElements2.DataBind2();
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
            
    //    }
    //}

    protected void setVenueSession(object sender, EventArgs e)
    {
        string parameter = Request.Params["__EVENTARGUMENT"];
        Session["SetVenue"] = parameter;
        //HtmlContainerControl VenueDiv = (HtmlContainerControl)SearchPanel.Items[0].Items[0].FindControl("VenueDiv");

        char[] delim = { ';' };
        VenueDiv.InnerHtml = parameter.Split(delim)[1];
    }

    protected void clearVenueSession(object sender, EventArgs e)
    {
        Session["SetVenue"] = null;
        VenueDiv.InnerHtml = "Select Locale &rArr;";
    }

    protected void setTimeSession(object sender, EventArgs e)
    {
        string parameter = Request.Form["__EVENTARGUMENT"];
        Session["SetTime"] = parameter;
        //HtmlContainerControl TimeFrameDiv = (HtmlContainerControl)SearchPanel.Items[0].Items[0].FindControl("TimeFrameDiv");
        TimeFrameDiv.InnerHtml = parameter;
    }

    protected void SelectNextDays(object sender, EventArgs e)
    {
        if (!(TimeRad.SelectedValue == null || TimeRad.SelectedValue == ""))
        {
            double tryDouble = 0.00;

            if (double.TryParse(TimeRad.SelectedValue, out tryDouble))
            {
                RadDatePicker2.Clear();
                Session["SetTime"] = "Next ";
                Search();
                
            }
        }
    }

    protected void SelectHighestPrice(object sender, EventArgs e)
    {
        if (HighestPriceInput.Value != "")
        {
            Session["HighestPrice"] = "notnull";
            Search();
        }
    }

    protected void SetToday(object sender, EventArgs e)
    {
        TimeRad.SelectedValue = "";
        RadDatePicker2.Clear();
        Session["SetTime"] = "Today";

        Search();
    }

    protected void SetTomorrow(object sender, EventArgs e)
    {
        TimeRad.SelectedValue = "";
        RadDatePicker2.Clear();
        Session["SetTime"] = "Tomorrow";
        Search();
    }

    protected void SetThisWeek(object sender, EventArgs e)
    {
        TimeRad.SelectedValue = "";
        RadDatePicker2.Clear();
        Session["SetTime"] = "ThisWeek";
        Search();
    }

    protected void SetThisWeekend(object sender, EventArgs e)
    {
        TimeRad.SelectedValue = "";
        RadDatePicker2.Clear();
        Session["SetTime"] = "ThisWeekend";
        Search();
    }

    protected void SetThisMonth(object sender, EventArgs e)
    {
        TimeRad.SelectedValue = "";
        RadDatePicker2.Clear();
        Session["SetTime"] = "ThisMonth";
        Search();
    }

    protected void SetDate(object sender, EventArgs e)
    {
        if (RadDatePicker2.DateInput.SelectedDate != null)
        {
            Session["SetTime"] = "ThisDate";
            Search();
            TimeRad.SelectedValue = "";
        }
    }

    protected void FilterResults(object sender, EventArgs e)
    {
        Search();
    }

    protected void ClearFilter(object sender, EventArgs e)
    {
        UnCheckTree(CategoryTree);
        UnCheckTree(RadTreeView1);
        UnCheckTree(RadTreeView2);
        Search();
    }

    protected void UnCheckTree(RadTreeView theTree)
    {
        foreach (RadTreeNode node in theTree.Nodes)
        {
            node.Checked = false;

            foreach (RadTreeNode child in node.Nodes)
            {
                child.Checked = false;
            }
        }
    }
}
