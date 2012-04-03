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

public partial class TripSearch : Telerik.Web.UI.RadAjaxPage
{
    protected enum TimeFrame { Beginning, Today, Tomorrow, ThisWeek, ThisWeekend, ThisMonth, NextDays, HighestPrice };

    protected void Page_Load(object sender, EventArgs e)
    {
        MessageLabel.Text = "";
        
        DataSet ds;

        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn);
        HtmlHead head = (HtmlHead)Page.Header;

        Literal lit = new Literal();
        lit.Text = "<script src=\"http://maps.google.com/maps?file=api&amp;v=2&amp;sensor=false&amp;key=ABQIAAAAjjoxQtYNtdn3Tc17U5-jbBR2Kk_H7gXZZZniNQ8L14X1BLzkNhQjgZq1k-Pxm8FxVhUy3rfc6L9O4g\" type=\"text/javascript\"></script>";
        head.Controls.Add(lit);

        #region Take Care of Buttons
        SearchButton.BUTTON_TEXT += "<img style=\"border: 0; padding-top: 3px;\" src=\"NewImages/search.png\"/>";
        SearchButton.SERVER_CLICK += SearchButton_Click;
        //TodayButton.BUTTON_TEXT = "<img style=\"border: 0; padding-top: 3px;\" src=\"NewImages/TodayText.png\"/>";
        //TomorrowButton.BUTTON_TEXT = "<img style=\"border: 0; padding-top: 3px;\" src=\"NewImages/TomorrowText.png\"/>";
        //ThisWeekButton.BUTTON_TEXT = "<img style=\"border: 0; padding-top: 3px;\" src=\"NewImages/ThisWeekText.png\"/>";
        //ThisWeekendButton.BUTTON_TEXT = "<img style=\"border: 0; padding-top: 3px;\" src=\"NewImages/ThisWeekendText.png\"/>";
        //ThisMonthButton.BUTTON_TEXT = "<img style=\"border: 0; padding-top: 3px;\" src=\"NewImages/ThisMonth.png\"/>";
        //TodayButton.SERVER_CLICK += SetToday;
        //TomorrowButton.SERVER_CLICK += SetTomorrow;
        //ThisWeekButton.SERVER_CLICK += SetThisWeek;
        //ThisWeekendButton.SERVER_CLICK += SetThisWeekend;
        //ThisMonthButton.SERVER_CLICK += SetThisMonth;
        FilterButton.SERVER_CLICK += FilterResults;
        SmallButton1.BUTTON_TEXT += "<img style=\"border: 0; padding-top: 3px;\" src=\"NewImages/search.png\"/>";
        SmallButton1.SERVER_CLICK += SearchButton_Click;
        SmallButton2.SERVER_CLICK += SearchButton_Click;
        SmallButton2.BUTTON_TEXT += "<img style=\"border: 0; padding-top: 3px;\" src=\"NewImages/search.png\"/>";
        #endregion

        if (!IsPostBack)
        {

            HttpCookie timeCookie = Request.Cookies["SetTime"];
            if (timeCookie != null)
            {
                timeCookie = new HttpCookie("BrowserDate");
                timeCookie.Value = "All%20Future%20Events";
                Response.Cookies.Add(timeCookie);
                TimeFrameDiv.InnerText = "All Future Trips";
            }

            Session["HighestPrice"] = null;
            Session.Remove("HighestPrice");

            Session["SearchDS"] = null;
            Session.Remove("SearchDS");

            NumsLabel.Text = "";
            NoResultsPanel.Visible = true;
            MapPanel.Visible = false;

            TopLiteral.Text = "<script type=\"text/javascript\">function initialize1(){map = null;}</script>";
            BottomScriptLiteral.Text = "";
            ScriptLiteral.Text = "";

            HtmlLink lk = new HtmlLink();
            lk.Attributes.Add("rel", "canonical");
            lk.Href = "http://hippohappenings.com/trip-search";
            head.Controls.AddAt(0, lk);


            

            //Create keyword and description meta tags
            HtmlMeta hm = new HtmlMeta();
            HtmlMeta kw = new HtmlMeta();

            kw.Name = "keywords";
            kw.Content = "local trips sights sightseeing landmarks adventures monuments";

            head.Controls.AddAt(0, kw);

            hm.Name = "Description";
            hm.Content = "Find local trips and adventures posted by your peers and experience more out of your city.";
            head.Controls.AddAt(0, hm);

            lk = new HtmlLink();
            lk.Href = "http://" + Request.Url.Authority + "/trip-search";
            lk.Attributes.Add("rel", "bookmark");
            head.Controls.AddAt(0, lk);

            EventTitle.Text = "<a style=\"text-decoration: none;\" href=\"" +
                lk.Href + "\"><h1>Search Adventures</h1></a>";

            //Button button = (Button)dat.FindControlRecursive(this, "EventLink");
            //button.CssClass = "NavBarImageEventSelected";
        }

        if (!IsPostBack)
        {
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


            try
            {
                if (Session["User"] != null)
                {
                    DataView dvUser = dat.GetDataDV("SELECT CatCity, CatState, CatCountry, CatZip, Radius FROM  UserPreferences WHERE UserID=" +
                        Session["User"].ToString());
                    CountryDropDown.SelectedValue = dvUser[0]["CatCountry"].ToString();
                    ChangeState(CountryDropDown, new EventArgs());
                    if (StateTextBoxPanel.Visible)
                        StateTextBox.Text = dvUser[0]["CatState"].ToString();
                    else
                        StateDropDown.Items.FindItemByText(dvUser[0]["CatState"].ToString()).Selected = true;

                    CityTextBox.Text = dvUser[0]["CatCity"].ToString();

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
                }
                else
                {
                    if (Session["LocCountry"] == null || Session["LocState"] == null || Session["LocCity"] == null)
                    {
                        dat.IP2Location();
                    }

                    CountryDropDown.SelectedValue = Session["LocCountry"].ToString().Trim();
                    ChangeState(CountryDropDown, new EventArgs());
                    if (StateTextBoxPanel.Visible)
                        StateTextBox.Text = Session["LocState"].ToString().Trim();
                    else
                        StateDropDown.Items.FindItemByText(Session["LocState"].ToString().Trim()).Selected = true;

                    CityTextBox.Text = Session["LocCity"].ToString();
                }

                Search();
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.ToString();
            }
        }
    }

    protected void SearchButton_Click(object sender, EventArgs e)
    {
        int intOut = 0;

        Session["SetTime"] = "ThisMonth";

        if (int.TryParse(TimeRad.SelectedValue, out intOut))
        {
            Session["SetTime"] = "Next ";
        }
        else
        {
            DateTime dateOut = new DateTime();
            if (RadDatePicker2.DateInput.SelectedDate != null)
            {
                if (DateTime.TryParse(RadDatePicker2.DateInput.SelectedDate.Value.ToString(), out dateOut))
                {
                    Session["SetTime"] = "ThisDate";
                }
            }
        }
        Search();
    }

    protected void Search()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        string message = "";

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn);

            string resultsStr = "";
            CityTextBox.Text = dat.stripHTML(CityTextBox.Text.Trim());
            StateTextBox.Text = dat.stripHTML(StateTextBox.Text.Trim());
            try
            {
                TripDuration.Text = dat.stripHTML(TripDuration.Text.Trim());
                decimal decOut = 0.00M;
                bool goOn = true;
                if (TripDuration.Text.Trim() != "")
                {
                    if (!decimal.TryParse(TripDuration.Text, out decOut))
                        goOn = false;
                }

                if (goOn)
                {
                    string featureDate = isNow.Month.ToString() + "/" + isNow.Day.ToString() + "/" + isNow.Year.ToString();

                    KeywordsTextBox.Text = dat.stripHTML(KeywordsTextBox.Text);

                    char[] delim = { ' ' };
                    string[] tokens;

                    tokens = KeywordsTextBox.Text.Split(delim);
                    string temp = "";
                    string featureSearchTerms = "";
                    string allSearchTerms = "";
                    string includeSearchTerms = "";
                    if (KeywordsTextBox.Text.Trim() != "")
                    {
                        for (int i = 0; i < tokens.Length; i++)
                        {
                            temp += " E.Header LIKE @search" + i.ToString();

                            featureSearchTerms += " EST.SearchTerms LIKE '%;" + tokens[i].Replace("'", "''") + ";%' ";

                            if (i + 1 != tokens.Length)
                            {
                                temp += " AND ";
                                featureSearchTerms += " AND ";
                            }
                        }
                        resultsStr += "'" + KeywordsTextBox.Text + "' ";
                    }
                    if (temp != "")
                    {
                        allSearchTerms = " AND ((" + temp + ") OR (E.ID=EST.TripID AND SearchDate = '" + featureDate +
                            "' AND " + featureSearchTerms + "))";

                        includeSearchTerms = ", TripSearchTerms EST ";

                    }

                    //if (temp != "")
                    //    temp = " AND " + temp;

                    //if (featureSearchTerms != "")
                    //    featureSearchTerms = " AND (" + featureSearchTerms + ") ";

                    bool isZip = false;


                    if (ZipTextBox.Text.Trim() != "")
                        isZip = true;



                    string country = " AND TDir.Country=" + CountryDropDown.SelectedValue;

                    string state = "";
                    string stateParam = "";
                    string city = "";
                    string cityParam = "";

                    string zip = "";
                    string zipParameter = "";
                    int zipParam = 0;
                    string nonExistantZip = "";

                    if (isZip)
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
                                        zip = " AND TDir.Zip = '" + zipParam.ToString() + "' ";
                                        nonExistantZip = " OR TDir.Zip = '" + zipParam.ToString() + "'";
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
                                            zip = " AND (TDir.Zip = '" + zipParam.ToString() + "' " + nonExistantZip;
                                            for (int i = 0; i < dsZips.Tables[0].Rows.Count; i++)
                                            {
                                                zip += " OR TDir.Zip = '" + dsZips.Tables[0].Rows[i]["Zip"].ToString() + "' ";
                                            }
                                            zip += ") ";
                                        }
                                        else
                                        {
                                            zip = " AND TDir.Zip='" + ZipTextBox.Text.Trim() + "'";
                                        }
                                    }
                                    else
                                    {
                                        zip = " AND TDir.Zip='" + ZipTextBox.Text.Trim() + "'";
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
                                zip = " AND TDir.Zip = @zip ";
                                zipParameter = ZipTextBox.Text.Trim();
                            }
                        }
                        else
                        {
                            goOn = false;
                            message = "You must at the very least include a zip code or State and City.";
                        }
                    }
                    else
                    {
                        if (CityTextBox.Text != "")
                        {
                            city = " AND TDir.City LIKE @city ";
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
                                state = " AND TDir.State=@state ";
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
                                state = " AND TDir.State=@state ";
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

                        HttpCookie timeCookie = Request.Cookies["SetTime"];
                        string test = "All%20Future%20Events";
                        if (timeCookie != null)
                            test = timeCookie.Value;
                        if (test.Trim() == "")
                            test = "All%20Future%20Events";

                        if (test != "")
                        {

                            string theDate = "";
                            int subtraction = 0;
                            string startDate = "";
                            string endDate = "";

                            DateTime StartDate = new DateTime();
                            DateTime EndDate = new DateTime();

                            string dayToday = isNow.Day.ToString();
                            string monthTody = isNow.Month.ToString();
                            string yearToday = isNow.Year.ToString();

                            string dateToday = monthTody + "/" + dayToday + "/" + yearToday;

                            string dayOfWeek = dat.getDayOfWeek(isNow.DayOfWeek);

                            string timeNow = isNow.TimeOfDay.ToString();

                            string dayStart = "";
                            string dayEnd = "";
                            string startMonth = "";
                            string endMonth = "";
                            string startYear = "";
                            string endYear = "";

                            string dateStart = "";
                            string dateEnd = "";

                            string startDayOfWeek = "";
                            string endDayOfWeek = "";

                            switch (test)
                            {
                                case "Today":
                                    resultsStr += " Today ";
                                    date = " (CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthStart)+'/'+CONVERT(NVARCHAR,TM.DayStart)+'/'+'" + yearToday +
                                        "') <= CONVERT(DATETIME, '" + dateToday + "') AND CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthEnd) +" +
                                        "'/'+CONVERT(NVARCHAR,TM.DayEnd)+'/'+'" + yearToday +
                                        "') >= CONVERT(DATETIME, '" + dateToday + "') AND TDS.Days LIKE '%" + dayOfWeek +
                                        "%' AND CONVERT(DATETIME, TDS.StartTime) <= CONVERT(DATETIME, '" + timeNow +
                                        "') AND CONVERT(DATETIME, TDS.EndTime) >= CONVERT(DATETIME, '" + timeNow + "') )";
                                    break;
                                case "Tomorrow":
                                    isNow = isNow.AddDays(1);
                                    dayToday = isNow.Day.ToString();
                                    monthTody = isNow.Month.ToString();
                                    yearToday = isNow.Year.ToString();

                                    dateToday = monthTody + "/" + dayToday + "/" + yearToday;

                                    dayOfWeek = dat.getDayOfWeek(isNow.DayOfWeek);

                                    timeNow = isNow.TimeOfDay.ToString();

                                    resultsStr += " Tomorrow ";
                                    date = " (CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthStart)+'/'+CONVERT(NVARCHAR,TM.DayStart)+'/'+'" + yearToday +
                                        "') <= CONVERT(DATETIME, '" + dateToday + "') AND CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthEnd) +" +
                                        "'/'+CONVERT(NVARCHAR,TM.DayEnd)+'/'+'" + yearToday +
                                        "') >= CONVERT(DATETIME, '" + dateToday + "')) AND TDS.Days LIKE '%" + dayOfWeek +
                                        "%'";
                                    break;
                                case "This%20Weekend":
                                    resultsStr += " This Weekend ";
                                    switch (DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).DayOfWeek)
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
                                    StartDate = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Subtract(TimeSpan.FromDays(subtraction));
                                    EndDate = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(2 - subtraction);

                                    dayStart = StartDate.Day.ToString();
                                    dayEnd = EndDate.Day.ToString();
                                    startMonth = StartDate.Month.ToString();
                                    endMonth = EndDate.Month.ToString();
                                    startYear = StartDate.Year.ToString();
                                    endYear = EndDate.Year.ToString();

                                    dateStart = startMonth + "/" + dayStart + "/" + startYear;
                                    dateEnd = endMonth + "/" + dayEnd + "/" + endYear;

                                    startDayOfWeek = dat.getDayOfWeek(StartDate.DayOfWeek);
                                    endDayOfWeek = dat.getDayOfWeek(EndDate.DayOfWeek);

                                    date = " (CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthStart)+'/'+CONVERT(NVARCHAR,TM.DayStart)+'/'+'" + startYear +
                                        "') <= CONVERT(DATETIME, '" + dateStart + "') AND CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthEnd) +" +
                                        "'/'+CONVERT(NVARCHAR,TM.DayEnd)+'/'+'" + endYear +
                                        "') >= CONVERT(DATETIME, '" + dateEnd + "')) AND (TDS.Days LIKE '%5%' OR TDS.Days LIKE '%6%' OR TDS.Days LIKE '%7%')";
                                    break;
                                case "This%20Week":
                                    resultsStr += " This Week ";
                                    switch (DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).DayOfWeek)
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
                                    StartDate = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Subtract(TimeSpan.FromDays(subtraction));
                                    EndDate = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(7.00 - subtraction);

                                    dayStart = StartDate.Day.ToString();
                                    dayEnd = EndDate.Day.ToString();
                                    startMonth = StartDate.Month.ToString();
                                    endMonth = EndDate.Month.ToString();
                                    startYear = StartDate.Year.ToString();
                                    endYear = EndDate.Year.ToString();

                                    dateStart = startMonth + "/" + dayStart + "/" + startYear;
                                    dateEnd = endMonth + "/" + dayEnd + "/" + endYear;

                                    startDayOfWeek = dat.getDayOfWeek(StartDate.DayOfWeek);
                                    endDayOfWeek = dat.getDayOfWeek(EndDate.DayOfWeek);

                                    date = " (CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthStart)+'/'+CONVERT(NVARCHAR,TM.DayStart)+'/'+'" + startYear +
                                        "') <= CONVERT(DATETIME, '" + dateStart + "') AND CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthEnd) +" +
                                        "'/'+CONVERT(NVARCHAR,TM.DayEnd)+'/'+'" + endYear +
                                        "') >= CONVERT(DATETIME, '" + dateEnd + "')) AND (TDS.Days LIKE '%1%' OR TDS.Days LIKE '%2%' OR TDS.Days LIKE '%3%' OR TDS.Days LIKE '%4%')";
                                    break;
                                case "Next%20Week":
                                    resultsStr += " Next Week ";
                                    switch (DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).DayOfWeek)
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
                                    StartDate = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Subtract(TimeSpan.FromDays(subtraction));
                                    EndDate = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(7.00 - subtraction);

                                    dayStart = StartDate.Day.ToString();
                                    dayEnd = EndDate.Day.ToString();
                                    startMonth = StartDate.Month.ToString();
                                    endMonth = EndDate.Month.ToString();
                                    startYear = StartDate.Year.ToString();
                                    endYear = EndDate.Year.ToString();

                                    dateStart = startMonth + "/" + dayStart + "/" + startYear;
                                    dateEnd = endMonth + "/" + dayEnd + "/" + endYear;

                                    startDayOfWeek = dat.getDayOfWeek(StartDate.DayOfWeek);
                                    endDayOfWeek = dat.getDayOfWeek(EndDate.DayOfWeek);

                                    date = " (CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthStart)+'/'+CONVERT(NVARCHAR,TM.DayStart)+'/'+'" + startYear +
                                        "') <= CONVERT(DATETIME, '" + dateStart + "') AND CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthEnd) +" +
                                        "'/'+CONVERT(NVARCHAR,TM.DayEnd)+'/'+'" + endYear +
                                        "') >= CONVERT(DATETIME, '" + dateEnd + "')) AND (TDS.Days LIKE '%1%' OR TDS.Days LIKE '%2%' OR TDS.Days LIKE '%3%' OR TDS.Days LIKE '%4%')";
                                    break;
                                case "This%20Month":
                                    resultsStr += " This Month ";

                                    dateToday = monthTody + "/" + DateTime.DaysInMonth(int.Parse(yearToday), int.Parse(monthTody)).ToString() + "/" + yearToday;
                                    date = " (CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthStart)+'/'+CONVERT(NVARCHAR,TM.DayStart)+'/'+'" + yearToday +
                                        "') <= CONVERT(DATETIME, '" + dateToday + " 23:59:59') AND CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthEnd) +" +
                                        "'/'+CONVERT(NVARCHAR,TM.DayEnd)+'/" + yearToday +
                                        "') >= CONVERT(DATETIME, '" + monthTody + "/1/" + yearToday + " 00:00:00'))";
                                    break;
                                case "All%20Future%20Events":
                                    resultsStr += " ";
                                    date = " ( CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthEnd) +" +
                                        "'/'+CONVERT(NVARCHAR,TM.DayEnd)+'/'+'" + yearToday +
                                        "') >= CONVERT(DATETIME, '" + dateToday + "'))";
                                    //CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthStart)+'/'+CONVERT(NVARCHAR,TM.DayStart)+'/'+'" + yearToday +
                                    //    "') <= CONVERT(DATETIME, '" + dateToday + "') AND
                                    break;
                                case "ThisDate":
                                    resultsStr += " on " + RadDatePicker2.DateInput.SelectedDate.Value.ToShortDateString() + " ";
                                    isNow = RadDatePicker2.DateInput.SelectedDate.Value;
                                    dayToday = isNow.Day.ToString();
                                    monthTody = isNow.Month.ToString();
                                    yearToday = isNow.Year.ToString();

                                    dateToday = monthTody + "/" + dayToday + "/" + yearToday;

                                    dayOfWeek = dat.getDayOfWeek(isNow.DayOfWeek);

                                    timeNow = isNow.TimeOfDay.ToString();
                                    date = " (CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthStart)+'/'+CONVERT(NVARCHAR,TM.DayStart)+'/'+'" + yearToday +
                                        "') <= CONVERT(DATETIME, '" + dateToday + "') AND CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthEnd) +" +
                                        "'/'+CONVERT(NVARCHAR,TM.DayEnd)+'/'+'" + yearToday +
                                        "') >= CONVERT(DATETIME, '" + dateToday + "')) AND TDS.Days LIKE '%" + dayOfWeek +
                                        "%'";
                                    break;
                                default:
                                    //Must be 'Next .. Days'
                                    if (test.Substring(0, 4) == "Next")
                                    {
                                        resultsStr += " Next " + TimeRad.SelectedValue + " Days ";
                                        StartDate = isn;
                                        EndDate = isn.AddDays(int.Parse(TimeRad.SelectedValue));

                                        dayStart = StartDate.Day.ToString();
                                        dayEnd = EndDate.Day.ToString();
                                        startMonth = StartDate.Month.ToString();
                                        endMonth = EndDate.Month.ToString();
                                        startYear = StartDate.Year.ToString();
                                        endYear = EndDate.Year.ToString();

                                        dateStart = startMonth + "/" + dayStart + "/" + startYear;
                                        dateEnd = endMonth + "/" + dayEnd + "/" + endYear;

                                        startDayOfWeek = dat.getDayOfWeek(StartDate.DayOfWeek);
                                        endDayOfWeek = dat.getDayOfWeek(EndDate.DayOfWeek);

                                        date = " (CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthStart)+'/'+CONVERT(NVARCHAR,TM.DayStart)" +
                                            "+'/'+'" + endYear +
                                            "') <= CONVERT(DATETIME, '" + EndDate.Month.ToString() + "/" + EndDate.Day.ToString() + "/" + EndDate.Year.ToString() + " 23:59:59') AND CONVERT(DATETIME,CONVERT" +
                                            "(NVARCHAR,TM.MonthEnd) +" +
                                            "'/'+CONVERT(NVARCHAR,TM.DayEnd)+'/'+'" + startYear +
                                            "') >= CONVERT(DATETIME, '" + StartDate.Month.ToString() + "/" + StartDate.Day.ToString() + "/" + StartDate.Year.ToString() + " 00:00:00'))";
                                    }
                                    else
                                    {
                                        DateTime dtime = DateTime.Parse(test.Replace("%20", " "));
                                        resultsStr += " " + dtime.ToShortDateString();
                                        date = " (CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthStart)+'/'+CONVERT(NVARCHAR,TM.DayStart)+'/'+'" + yearToday +
                                            "') <= CONVERT(DATETIME, '" + dtime.ToShortDateString() + "') AND CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthEnd) +" +
                                            "'/'+CONVERT(NVARCHAR,TM.DayEnd)+'/'+'" + yearToday +
                                            "') >= CONVERT(DATETIME, '" + dtime.ToShortDateString() + "')AND TDS.Days LIKE '%" + dat.getDayOfWeek(dtime.DayOfWeek) +
                                        "%')";
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
                            resultsStr += " Highest Price $" + itOut.ToString();
                        }
                        else
                        {
                            Session["HighestPrice"] = null;
                            Session.Remove("HighestPrice");
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

                                categories += " AND ( E.ID IN (SELECT ECM.TripID FROM Trip_Category ECM " +
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
                                    categories += " AND ( E.ID IN (SELECT ECM.TripID FROM Trip_Category ECM " +
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

                                categories += " AND ( E.ID IN (SELECT ECM.TripID FROM Trip_Category ECM " +
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
                                    categories += " AND ( E.ID IN (SELECT ECM.TripID FROM Trip_Category ECM " +
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

                                categories += " AND ( E.ID IN (SELECT ECM.TripID FROM Trip_Category ECM " +
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
                                    categories += " AND ( E.ID IN (SELECT ECM.TripID FROM Trip_Category ECM " +
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

                        string catHiTemp = categories + highestP + temp;


                        string featureString = "";
                        
                        //if (catHiTemp.Trim() != "")
                        //{
                        //    if (featureSearchTerms != "")
                        //    {
                        //        if (catHiTemp.Trim().Substring(0, 3).ToLower() == "and")
                        //            catHiTemp = catHiTemp.Trim().Substring(3, catHiTemp.Trim().Length - 3);
                        //        featureString = "AND (( " + catHiTemp + " ) OR " +
                        //        "(E.ID=EST.TripID AND SearchDate = '" + featureDate + "' " + featureSearchTerms +
                        //        "))";
                        //        includeSearchTerms = ", TripSearchTerms EST";
                        //    }
                        //    else
                        //    {
                        //        featureString = catHiTemp;
                        //    }
                        //}

                        string means = "";
                        foreach (ListItem item in MeansCheckList.Items)
                        {
                            if (item.Selected)
                            {
                                means += " OR E.Means LIKE '%" + item.Value + "%'";
                            }
                        }

                        if (means != "")
                        {
                            means = means.Trim().Substring(2, means.Trim().Length - 2);
                            means = " AND (" + means + ")";
                        }

                        string duration = "";
                        if (TripDuration.Text.Trim() != "")
                        {
                            duration = " AND CONVERT(DECIMAL(18,2),dbo.GetDuration(E.Duration, 1)) <= CONVERT(DECIMAL(18,2), '" + TripDuration.Text + "') ";

                            resultsStr += " duration " + TripDuration.Text + " hrs.";
                        }

                        string timeFrame = "";

                        if (DepartureTimePicker.DateInput.SelectedDate != null)
                        {
                            timeFrame = " AND CONVERT(DATETIME,'1/1/2000 ' + TDS.StartTime) >= CONVERT(DATETIME , '1/1/2000 '+'" + DepartureTimePicker.DateInput.SelectedDate.Value.ToShortTimeString() + "') ";
                        }

                        if (BackTimePicker.DateInput.SelectedDate != null)
                        {
                            timeFrame += " AND CONVERT(DATETIME,'1/1/2000 ' + TDS.EndTime) <= CONVERT(DATETIME , '1/1/2000 '+'" + BackTimePicker.DateInput.SelectedDate.Value.ToShortTimeString() + "') ";
                        }


                        string searchStr = "SELECT DISTINCT E.ID AS EID, '$'+CONVERT(NVARCHAR,E.MinPrice)+' - $'+CONVERT(NVARCHAR, E.MaxPrice)" +
                            "AS PriceRange, E.MinPrice AS Price, E.Means, dbo.GetDuration(E.Duration, 0) AS " +
                            "TimeFrame, dbo.GetDuration(E.Duration, 1) AS Duration, E.Header, " +
                            " E.Featured, E.DaysFeatured FROM TripDirections TDir, Trips E, TripMonths TM, TripDays TDS "
                              + includeSearchTerms + " WHERE TDir.TripID=E.ID AND TM.TripID=TDS.TripID AND TM.TripID=E.ID AND E.Live='True' " +
                            country + state + city + zip + " AND " + date + duration + means + timeFrame +
                            allSearchTerms + highestP + categories + " ORDER BY E.Header";
                        //ErrorLabel.Text = searchStr;
                        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
                        if (conn.State != ConnectionState.Open)
                            conn.Open();
                        SqlCommand cmd = new SqlCommand(searchStr, conn);

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
                        message = searchStr;
                        DataSet ds = new DataSet();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);

                        DataSet dsAll = ds;

                        Session["SearchDS"] = dsAll;
                        Session["Searching"] = "Trips";
                        Session["SearchResults"] = "Results " + resultsStr;
                        ResultsLabel.Text = "Search results " + resultsStr;
                        conn.Close();

                        DataView dv = new DataView(dsAll.Tables[0], "", "", DataViewRowState.CurrentRows);
                        DataView dvEvent;

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
                else
                {
                    DurationErrorLabel.Text = "Duration must be a number.";
                }
            }
            catch (Exception ex)
            {
                MessageLabel.Text += ex.ToString() + "<br/>" + message;
            }
    }

    protected void FillResults(DataSet ds)
    {
        TopLiteral.Text = "<script type=\"text/javascript\">function initialize1(){map = null;}</script>";
        BottomScriptLiteral.Text = "";
        ScriptLiteral.Text = "";

        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn); 

        DataColumn dc = new DataColumn("SearchNum");
        if (!ds.Tables[0].Columns.Contains("SearchNum"))
            ds.Tables[0].Columns.Add(dc);


        int numofpages = 10;

        int countOfUniqueVenues = 0;

        string sortString = "";
        if (Session["tripSortString"] != null)
        {
            sortString = Session["tripSortString"].ToString();
        }

        DataView dv = TakeCareOfFeaturedTrips(ds, sortString);

        string[] mapStrings = GetVenuesLiteral(dv, dat, numofpages);

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
    protected DataView TakeCareOfFeaturedTrips(DataSet ds, string sortString)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn); string featureDate = isNow.Month.ToString() + "/" + isNow.Day.ToString() + "/" + isNow.Year.ToString();

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
            FeaturedMapping[fCount++] = (int)row["EID"];
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
        int toFeature = 3;
        int normalRowCount = 0;
        int totalCount = dv.Count;
        int toNormal = 6;
        while (count < totalCount)
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

    protected string[] GetVenuesLiteral(DataView dvEvents, Data dat, int numOfRecordsPerPage)
    {
        int i = 0;

        string address = "";
        string lastAddress = "";
        string nameStr = "";
        string country = "US";
        bool isAddressSame = false;
        bool isFirstElement = true;

        int thecount = dvEvents.Count;
        int numberInArray = thecount / numOfRecordsPerPage;

        if (thecount % numOfRecordsPerPage != 0)
            numberInArray++;
        string oneStringRecord = "";

        int normalRecordCount = 0;
        int countInArray = 0;
        TopLiteral.Text = "";
        string theLiteral = "<script type=\"text/javascript\">function initialize" +
                (countInArray + 1).ToString() + "(){initMap(); "+
               " if(map != null && map != undefined) " +
               " { " +
               "     map.setUIToDefault(); " +
               "    geocoder = new GClientGeocoder(); var address;";
        string[] theArray = new string[numberInArray];
        int lastUsedCount = 0;
        Hashtable venueMapping = new Hashtable();
        Hashtable venuesNumMapping = new Hashtable();
        DataView dvRecords = new DataView();
        DataTable dt = new DataTable();
        DataColumn dc2 = new DataColumn("Address");
        dt.Columns.Add(dc2);
        dc2 = new DataColumn("Letter");
        dt.Columns.Add(dc2);

        string addToLit = "";
        string addToLitBottom = "";
        foreach (DataRowView row in dvEvents)
        {
            if (normalRecordCount % numOfRecordsPerPage == 0)
            {
                if (normalRecordCount != 0 && normalRecordCount != lastUsedCount)
                {
                    lastUsedCount = normalRecordCount;
                    //ErrorLabel.Text += "on script insert normal rec count: " + normalRecordCount.ToString();
                    dvRecords = new DataView(dt, "", "Letter ASC", DataViewRowState.CurrentRows);
                    //if (countInArray == 0)
                    //{
                    //    addToLit = "if (GBrowserIsCompatible()){ " +
                    //       " map = new GMap2(document.getElementById(\"map_canvas\")); " +
                    //       " map.addControl(new GMapTypeControl()); " +

                    //       " if(map != null && map != undefined) " +
                    //       " { " +
                    //       "     map.setUIToDefault(); " +
                    //       "    geocoder = new GClientGeocoder(); ";
                    //    addToLitBottom = "}}}";
                    //}
                    //else
                    //{
                    //    addToLit = " if(map != null && map != undefined) " +
                    //        " {";
                    //    addToLitBottom = "}}";
                    //}
                    addToLit = " if(map != null && map != undefined) " +
                            " {";
                    addToLitBottom = "}}";
                    theLiteral = "<script type=\"text/javascript\">function initialize" +
                (countInArray + 1).ToString() + "(){initMap(); " + addToLit + "var address;";
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
                                oneStringRecord += addToLitBottom + "</script>".Replace("   ",
                                                            " ").Replace("  ", " ");
                            }
                        }
                        else if (rowCount == dvRecords.Count - 1)
                        {
                            oneStringRecord += venueMapping[rowster["Address"].ToString()].ToString() +
                                                        "', false); " + addToLitBottom + "</script>".Replace("   ",
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

                lastAddress = "";
                isFirstElement = true;
                i = 0;
            }
            isAddressSame = false;
            address = "";
            DataView dvV = dat.GetDataDV("SELECT Top 1 * FROM Trips T, TripDirections TD WHERE " +
                "T.ID=TD.TripID AND T.ID=" + row["EID"].ToString() + " ORDER BY TD.ID");

            DataView dvCountry = dat.GetDataDV("SELECT * FROM countries WHERE country_id=" +
                dvV[0]["Country"].ToString());
            country = dvCountry[0]["country_2_code"].ToString();

            if (country.ToLower() == "us")
            {
                try
                {
                    address = dat.GetAddress(dvV[0]["Address"].ToString(), false);
                }
                catch (Exception ex1)
                {
                    address = "";
                }
            }
            else
            {
                address = dat.GetAddress(dvV[0]["Address"].ToString(), true);
            }


            if (lastAddress == address && !isFirstElement)
                isAddressSame = true;
            else
                isAddressSame = false;

            if (country.ToLower() == "uk")
            {
                if (venueMapping.ContainsKey(address))
                {
                    row["SearchNum"] = venuesNumMapping[address].ToString();
                    venueMapping[address] += ",<br/><div class=\\\"NavyLink12UD\\\" " +
                        "onclick=\\\"GoToThis(\\'" + "../" +
                        dat.MakeNiceName(dvV[0]["Header"].ToString()) + "_" +
                        dvV[0]["TripID"].ToString() + "_Trip" + "\\');\\\">" +
                        dvV[0]["Header"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + "</div>";
                }
                else
                {
                    if (!isFirstElement)
                    {
                        i++;
                        venuesNumMapping.Add(address, dat.GetImage(i.ToString()));
                        venueMapping.Add(address, "address =  '" + address.Replace("'", "''").Replace("(", " ").Replace(")", " ") +
                            dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                            dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                            country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; " +
                            "showAddressUS('',0, address, 0, address, 1, " + i.ToString() + ", '" +
                        "<div class=\\\"NavyLink12UD\\\" " +
                        "onclick=\\\"GoToThis(\\'" + "../" +
                        dat.MakeNiceName(dvV[0]["Header"].ToString()) + "_" +
                        dvV[0]["TripID"].ToString() + "_Trip" + "\\');\\\">" +
                        dvV[0]["Header"].ToString().Replace("'", " ").Replace("(",
                        " ").Replace(")", " ") + "</div> ");
                        row["Header"] = dat.GetImage(i.ToString());
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
                        venueMapping.Add(address, "address =  '" + address.Replace("'", "''").Replace("(", " ").Replace(")", " ") +
                            dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                            dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                            country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; " +
                            " showAddressUS('',0, address, 0, address, 1, " + i.ToString() + ", '" +
                        "<div class=\\\"NavyLink12UD\\\" " +
                        "onclick=\\\"GoToThis(\\'" + "../" +
                        dat.MakeNiceName(dvV[0]["Header"].ToString()) + "_" +
                        dvV[0]["TripID"].ToString() + "_Trip" + "\\');\\\">" +
                        dvV[0]["Header"].ToString().Replace("'", " ").Replace("(",
                        " ").Replace(")", " ") + "</div> ");
                    }
                }
            }
            else
            {
                if (venueMapping.ContainsKey(address))
                {
                    row["SearchNum"] = venuesNumMapping[address].ToString();
                    venueMapping[address] += ",<br/><div class=\\\"NavyLink12UD\\\" " +
                        "onclick=\\\"GoToThis(\\'" + "../" +
                        dat.MakeNiceName(dvV[0]["Header"].ToString()) + "_" +
                        dvV[0]["TripID"].ToString() + "_Trip" + "\\');\\\">" +
                        dvV[0]["Header"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + "</div>";
                }
                else
                {
                    if (!isFirstElement)
                    {
                        i++;
                        venuesNumMapping.Add(address, dat.GetImage(i.ToString()));
                        venueMapping.Add(address, "address =  '" + address +
                            dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                            dvV[0]["State"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                            dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                            country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; " +
                            "showAddressUS('',0, address, 0, address, 1, " + i.ToString() + ", '" +
                            "<div class=\\\"NavyLink12UD\\\" onclick=\\\"GoToThis(\\'" + "../" +
                            dat.MakeNiceName(dvV[0]["Header"].ToString()) + "_" +
                            dvV[0]["TripID"].ToString() + "_Trip" + "\\');\\\">" +
                            dvV[0]["Header"].ToString().Replace("'", " ").Replace("(",
                            " ").Replace(")", " ") + "</div> ");
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
                        venueMapping.Add(address, "address =  '" + address +
                            dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                            dvV[0]["State"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                            dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                            country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; " +
                            "showAddressUS('',0, address, 0, address, 1, " + i.ToString() + ", '" +
                            "<div class=\\\"NavyLink12UD\\\" onclick=\\\"GoToThis(\\'" + "../" +
                            dat.MakeNiceName(dvV[0]["Header"].ToString()) + "_" +
                            dvV[0]["TripID"].ToString() + "_Trip" + "\\');\\\">" +
                            dvV[0]["Header"].ToString().Replace("'", " ").Replace("(",
                            " ").Replace(")", " ") + "</div> ");
                    }
                }

            }
            lastAddress = address;
            isFirstElement = false;
            normalRecordCount++;
        }
        //if (thecount % numOfRecordsPerPage != 0)
        //{
        dvRecords = new DataView(dt, "", "Letter ASC", DataViewRowState.CurrentRows);
        //if (countInArray == 0)
        //{
        //    addToLit = "if (GBrowserIsCompatible()){ " +
        //       " map = new GMap2(document.getElementById(\"map_canvas\")); " +
        //       " map.addControl(new GMapTypeControl()); " +

        //       " if(map != null && map != undefined) " +
        //       " { " +
        //       "     map.setUIToDefault(); " +
        //       "    geocoder = new GClientGeocoder(); ";
        //    addToLitBottom = "}}}";
        //}
        //else
        //{
        //    addToLit = " if(map != null && map != undefined) " +
        //        " {";
        //    addToLitBottom = "}}";
        //}
        addToLit = " if(map != null && map != undefined) " +
                    " {";
        addToLitBottom = "}}";
        theLiteral = "<script type=\"text/javascript\">function initialize" +
    (countInArray + 1).ToString() + "(){initMap(); " + addToLit + "var address;";
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
                    oneStringRecord += addToLitBottom + "</script>".Replace("   ",
                                                " ").Replace("  ", " ");
                }
            }
            else if (theCount == dvRecords.Count - 1)
            {
                oneStringRecord += venueMapping[rowster["Address"].ToString()].ToString() +
                                            "', false); " + addToLitBottom + "</script>".Replace("   ",
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
        return theArray;
    }

    protected string[] GetEventLiteral(DataView dvEvents, Data dat, int numOfRecordsPerPage)
    {
        int thecount = dvEvents.Count;
        int numberInArray = thecount / numOfRecordsPerPage;

        if (thecount % numOfRecordsPerPage != 0)
            numberInArray++;
        string oneStringRecord = "";

        int normalRecordCount = 0;
        int countInArray = 0;
        string[] theArray = new string[numberInArray];

        try
        {
            Hashtable normalEventHash = new Hashtable();

            int i = 0;
            string funcCount = "";

            string theLiteral = "<script type=\"text/javascript\">function initialize" +
                (countInArray + 1).ToString() + "(){ " +
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
            foreach (DataRowView row in dvEvents)
            {
                if (normalRecordCount % numOfRecordsPerPage == 0)
                {
                    if (normalRecordCount != 0 && normalRecordCount != lastUsedCount)
                    {
                        lastUsedCount = normalRecordCount;
                        //ErrorLabel.Text += "on script insert normal rec count: " + normalRecordCount.ToString();
                        dvRecords = new DataView(dt, "", "Letter ASC", DataViewRowState.CurrentRows);
                        theLiteral = "<script type=\"text/javascript\">function initialize" +
                            (countInArray + 1).ToString() + "(){if(map != null && map != undefined) " +
                           " { var address;";
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


                if (!normalEventHash.Contains(row["EID"].ToString()))
                {
                    normalEventHash.Add(row["EID"].ToString(), normalRecordCount.ToString());

                    address = "";
                    DataView dvE = dat.GetDataDV("SELECT * FROM Events WHERE ID=" + row["EID"].ToString());
                    DataView dvV = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + dvE[0]["Venue"].ToString());

                    DataView dvCountry = dat.GetDataDV("SELECT * FROM countries WHERE country_id=" + dvE[0]["Country"].ToString());
                    country = dvCountry[0]["country_2_code"].ToString();

                    if (country.ToLower() == "us")
                    {
                        try
                        {
                            address = dat.GetAddress(dvV[0]["Address"].ToString(), false);
                        }
                        catch (Exception ex1)
                        {
                            address = "";
                        }
                    }
                    else
                    {
                        address = dat.GetAddress(dvV[0]["Address"].ToString(), true);
                    }

                    if (dvE[0]["Country"].ToString().ToLower() == "uk")
                    {
                        //VenueName.Text + "@&" + 
                        if (venueMapping.ContainsKey(address))
                        {
                            row["SearchNum"] = venuesNumMapping[address].ToString();
                            venueMapping[address] += "<br/><div class=\\\"NavyLink12UD\\\" style=\\\" float: left; cursor: pointer;\\\" onclick=\\\"GoToThis(\\'" + "../" +
                                    dat.MakeNiceName(dvE[0]["Header"].ToString()) + "_" +
                                    dvE[0]["ID"].ToString() + "_Event" + "\\');\\\">" +
                                    dat.CleanExcelString(dvE[0]["Header"].ToString().Replace("'",
                                    " ").Replace("(", " ").Replace(")", " ")) + "</div>";
                        }
                        else
                        {
                            if (!isFirstElement)
                            {
                                i++;
                                venuesNumMapping.Add(address, dat.GetImage(i.ToString()));
                                venueMapping.Add(address, "address =  '" + address.Trim().Replace("'", "''").Replace("(", " ").Replace(")", " ") + " " +
                                    dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                                    dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                                    country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; " +
                                    "showAddressUS('<span  class=\\\"Green12Link2UD\\\"  onclick=\\\"GoToThis(\\'" + "../" +
                                    dat.MakeNiceName(dvV[0]["Header"].ToString()) + "_" +
                                    dvV[0]["ID"].ToString() + "_Trip" + "\\');\\\">" +
                                    dvV[0]["Header"].ToString().Replace("'",
                                    " ").Replace("(", " ").Replace(")", " ") + "</span>', 0, address, 0, address, 1, " + i.ToString() + ", '" +
                                    "<div  class=\\\"NavyLink12UD\\\"  onclick=\\\"GoToThis(\\'" + "../" +
                                    dat.MakeNiceName(dvE[0]["Header"].ToString()) + "_" +
                                    dvE[0]["ID"].ToString() + "_Event" + "\\');\\\">" +
                                    dat.CleanExcelString(dvE[0]["Header"].ToString().Replace("'",
                                    " ").Replace("(", " ").Replace(")", " ")) + "</div>");

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
                                venueMapping.Add(address, "address =  '" + address.Trim().Replace("'", "''").Replace("(", " ").Replace(")", " ") + " " +
                                    dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                                    dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                                    country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; showAddressUS('<span  class=\\\"NavyLink12UD\\\"  onclick=\\\"GoToThis(\\'" + "../" +
                                    dat.MakeNiceName(dvV[0]["Header"].ToString()) + "_" +
                                    dvV[0]["ID"].ToString() + "_Trip" + "\\');\\\">" +
                                    dvV[0]["Header"].ToString().Replace("'",
                                    " ").Replace("(", " ").Replace(")", " ") + "</span>',0, address, 0, address, 1, " + i.ToString() + ", '" +
                                    "<div class=\\\"NavyLink12UD\\\"  onclick=\\\"GoToThis(\\'" + "../" +
                                    dat.MakeNiceName(dvE[0]["Header"].ToString()) + "_" +
                                    dvE[0]["ID"].ToString() + "_Event" + "\\');\\\">" +
                                    dat.CleanExcelString(dvE[0]["Header"].ToString().Replace("'",
                                    " ").Replace("(", " ").Replace(")", " ")) + "</div>");
                            }
                        }

                    }
                    else
                    {
                        //VenueName.Text + "@&" + 
                        if (venueMapping.ContainsKey(address))
                        {
                            row["SearchNum"] = venuesNumMapping[address].ToString();
                            venueMapping[address] += "<br/><div class=\\\"NavyLink12UD\\\"  onclick=\\\"GoToThis(\\'" + "../" +
                                    dat.MakeNiceName(dvE[0]["Header"].ToString()) + "_" +
                                    dvE[0]["ID"].ToString() + "_Event" + "\\');\\\">" +
                                    dat.CleanExcelString(dvE[0]["Header"].ToString().Replace("'",
                                    " ").Replace("(", " ").Replace(")", " ")) + "</div>";
                        }
                        else
                        {
                            if (!isFirstElement)
                            {
                                i++;
                                venuesNumMapping.Add(address, dat.GetImage(i.ToString()));
                                venueMapping.Add(address, "address =  '" + address.Trim().Replace("'", "''").Replace("(", " ").Replace(")", " ") + " " +
                                    dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                    dvV[0]["State"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                    dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                    country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; showAddressUS('<span class=\\\"NavyLink12UD\\\" onclick=\\\"GoToThis(\\'" + "../" +
                                    dat.MakeNiceName(dvV[0]["Header"].ToString()) + "_" +
                                    dvV[0]["ID"].ToString() + "_Trip" + "\\');\\\">" +
                                    dvV[0]["Header"].ToString().Replace("'",
                                    " ").Replace("(", " ").Replace(")", " ") + "</span>',0, address, 0, address, 1, " + i.ToString() + ", '" +
                                    "<div class=\\\"NavyLink12UD\\\" onclick=\\\"GoToThis(\\'" + "../" +
                                    dat.MakeNiceName(dvE[0]["Header"].ToString()) + "_" +
                                    dvE[0]["ID"].ToString() + "_Event" + "\\');\\\">" +
                                   dat.CleanExcelString(dvE[0]["Header"].ToString().Replace("'",
                                    " ").Replace("(", " ").Replace(")", " ")) + "</div>");
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
                                venueMapping.Add(address, "address =  '" + address.Trim().Replace("'", "''").Replace("(", " ").Replace(")", " ") + " " +
                                    dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                    dvV[0]["State"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                    dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                    country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; showAddressUS('<span class=\\\"NavyLink12UD\\\" onclick=\\\"GoToThis(\\'" + "../" +
                                    dat.MakeNiceName(dvV[0]["Header"].ToString()) + "_" +
                                    dvV[0]["ID"].ToString() + "_Trip" + "\\');\\\">" +
                                    dvV[0]["Header"].ToString().Replace("'",
                                    " ").Replace("(", " ").Replace(")", " ") + "</span>',0, address, 0, address, 1, " + i.ToString() + ", '" +
                                    "<div class=\\\"NavyLink12UD\\\"  onclick=\\\"GoToThis(\\'" + "../" +
                                    dat.MakeNiceName(dvE[0]["Header"].ToString()) + "_" +
                                    dvE[0]["ID"].ToString() + "_Event" + "\\');\\\">" +
                                    dat.CleanExcelString(dvE[0]["Header"].ToString().Replace("'",
                                    " ").Replace("(", " ").Replace(")", " ")) + "</div>");
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
            theLiteral = "<script type=\"text/javascript\">function initialize" +
                (countInArray + 1).ToString() + "(){if(map != null && map != undefined) " +
               " { var address;";
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
            ErrorLabel.Text += ex.ToString() + "; countInArray: " + countInArray +
                "; theArray.Length: " + theArray.Length + "; eventsCount: " + thecount.ToString() + "; normalRecordCount: " + normalRecordCount.ToString();
        }

        return theArray;
    }

    protected void SortResults(object sender, EventArgs e)
    {
        if (SortDropDown.SelectedItem.Value != "-1")
            Session["tripSortString"] = SortDropDown.SelectedItem.Value;
        else
            Session["tripSortString"] = null;

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

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
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
