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
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["Searching"] = "Events";
        MessageLabel.Text = "";
        //Page.Trace.IsEnabled = true;
        //Page.Trace.TraceMode = TraceMode.SortByTime;
        //HttpContext.Current.Trace.Warn("EventSearch: Page_Load Begin", string.Format("{0},{1},{2}",
        //                  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
        //                  "info",
        //                  "---Entering Event Search: Page_Load------"));
        

        //HttpContext.Current.Trace.Warn("EventSearch: Page_Load: 1", string.Format("{0},{1},{2}",
        //                  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
        //                  "info",
        //                  "---Entering Event Search: Page_Load------"));

        //HttpContext.Current.Trace.Warn("EventSearch: Page_Load: 2", string.Format("{0},{1},{2}",
        //                  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
        //                  "info",
        //                  "---Entering Event Search: Page_Load------"));

        DataSet ds;

        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));


        if (!IsPostBack)
        {
            Session["SetTime"] = "All Future Events";
            TimeFrameDiv.InnerHtml = Session["SetTime"].ToString();
            
            Session["SetVenue"] = null;
            Session["SearchDS"] = null;

            HtmlLink lk = new HtmlLink();
            HtmlHead head = (HtmlHead)Page.Header;
            lk.Attributes.Add("rel", "canonical");
            lk.Href = "http://hippohappenings.com/EventSearch.aspx";
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
            lk.Href = "http://" + Request.Url.Authority + "/EventSearch.aspx";
            lk.Attributes.Add("rel", "bookmark");
            head.Controls.AddAt(0, lk);

            EventTitle.Text = "<a style=\"text-decoration: none; color: white;\" href=\"" +
                lk.Href + "\">Search Events</a>";

            //Button button = (Button)dat.FindControlRecursive(this, "EventLink");
            //button.CssClass = "NavBarImageEventSelected";
        }
        else
        {
            string ctrlname = Request.Params.Get("__EVENTTARGET");

            try
            {

                if (ctrlname == "ctl00_ContentPlaceHolder1_SearchButton")
                {
                    Telerik.Web.UI.RadComboBox StateDropDown = (Telerik.Web.UI.RadComboBox)locationSearchPanel.Items[0].Items[0].FindControl("StateDropDown");
                    Telerik.Web.UI.RadTextBox StateTextBox = (Telerik.Web.UI.RadTextBox)locationSearchPanel.Items[0].Items[0].FindControl("StateTextBox");
                    Telerik.Web.UI.RadTextBox CityTextBox = (Telerik.Web.UI.RadTextBox)locationSearchPanel.Items[0].Items[0].FindControl("CityTextBox");
                    Telerik.Web.UI.RadTextBox ZipTextBox = (Telerik.Web.UI.RadTextBox)locationSearchPanel.Items[1].Items[0].FindControl("ZipTextBox");
                    bool goodGo = false;
                    if (((StateDropDown.SelectedValue != "-1" || StateTextBox.Text.Trim() != "")
                        && CityTextBox.Text.Trim() != "") || ZipTextBox.Text.Trim() != "")
                    {
                        RadWindow RadWindow3 = Master.RAD_WINDOW_3;
                        RadWindow3.VisibleOnPageLoad = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageLabel.Text = ctrlname;
            }
            if (Session["SetVenue"] != null)
            {
                char[] delim = { ';' };

                VenueDiv.InnerHtml = Session["SetVenue"].ToString().Split(delim)[1];
            }

            if (Session["SetTime"] != null)
            {
                TimeFrameDiv.InnerHtml = Session["SetTime"].ToString();
            }
            
            if (Session["SearchDS"] != null)
            {
                if (Session["Searching"].ToString() == "Events")
                {
                    //SearchPanel.Items[1].Selected = true;
                    //ds = (DataSet)Session["SearchDS"];
                    //SearchElements2.EVENTS_DS = ds;
                    //SearchElements2.DataBind2();

                    //SearchPanel.Items[0].Expanded = false;
                    //SearchPanel.Items[1].Expanded = true;

                    //int eventCount = ds.Tables[0].Rows.Count;

                    //if (dat.SearchCutOffNumber < eventCount)
                    //{
                    //    SearchPanel.Items[1].Text = "<div style='position: relative;'><span style='font-family: Arial; font-size: 20px; color: White;'>Search Results: </span><div style='position: absolute; right: 5px; top: 3px;'>(Search cut off at " + dat.SearchCutOffNumber + ")</div></div>";

                    //}
                    //else
                    //{

                    //    SearchPanel.Items[1].Text = "<div style='position: relative;'><span style='font-family: Arial; font-size: 20px; color: White;'>Search Results: </span><div style='position: absolute; right: 5px; top: 3px;'>(" + ds.Tables[0].Rows.Count + " Records Found)</div></div>";
                    //}
                }
                else
                {
                    //if (!SearchPanel.Items[0].Expanded)
                    //    SearchPanel.Items[0].Expanded = true;

                    //if (SearchPanel.Items[1].Expanded)
                    //    SearchPanel.Items[1].Expanded = false;
                }
            }
            else
            {
                //if (!SearchPanel.Items[0].Expanded)
                //    SearchPanel.Items[0].Expanded = true;

                //if (SearchPanel.Items[1].Expanded)
                //    SearchPanel.Items[1].Expanded = false;
            }
        }
        //}
        //Telerik.Web.UI.RadPanelBar locationSearchPanel = (Telerik.Web.UI.RadPanelBar)SearchPanel.Items[0].Items[0].FindControl("locationSearchPanel");

        //Telerik.Web.UI.RadComboBox DateDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("DateDropDown");

        //HttpContext.Current.Trace.Warn("EventSearch: Page_Load: 3", string.Format("{0},{1},{2}",
        //                  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
        //                  "info",
        //                  "---Entering Event Search: Page_Load------"));

        if (!IsPostBack)
        {


            //Telerik.Web.UI.RadComboBox CountryDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("CountryDropDown");
            Telerik.Web.UI.RadComboBox StateDropDown = (Telerik.Web.UI.RadComboBox)locationSearchPanel.Items[0].Items[0].FindControl("StateDropDown");
            Panel StateDropDownPanel = (Panel)locationSearchPanel.Items[0].Items[0].FindControl("StateDropDownPanel");
            Panel StateTextBoxPanel = (Panel)locationSearchPanel.Items[0].Items[0].FindControl("StateTextBoxPanel");
            //Telerik.Web.UI.RadComboBox VenueDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("VenueDropDown");

            //DateDropDown.Text = "Select Time Frame";
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

            DataSet ds4 = dat.GetData("SELECT CASE WHEN SUBSTRING(Name, 1, 4) = 'The' THEN SUBSTRING(Name, 5, "+
                "LEN(Name)-4) ELSE Name END AS Name1, * FROM Venues WHERE Country=" + 
                CountryDropDown.SelectedValue + " AND State='" + StateDropDown.SelectedItem.Text + 
                "' ORDER BY Name1 ASC");

            //VenueDropDown.Items.Clear();
            if (ds4.Tables.Count > 0)
                if (ds4.Tables[0].Rows.Count > 0)
                {
                    Session["LocationVenues"] = ds4;

                    fillVenues(ds4);
                }


            if (Session["User"] != null)
            {
                DataView dvUser = dat.GetDataDV("SELECT CatCity, CatState, CatCountry, CatZip, Radius FROM  UserPreferences WHERE UserID=" +
                    Session["User"].ToString());
                Telerik.Web.UI.RadTextBox StateTextBox = (Telerik.Web.UI.RadTextBox)locationSearchPanel.Items[0].FindControl("StateTextBox");
                Telerik.Web.UI.RadTextBox CityTextBox = (Telerik.Web.UI.RadTextBox)locationSearchPanel.Items[0].Items[0].FindControl("CityTextBox");
                CountryDropDown.SelectedValue = dvUser[0]["CatCountry"].ToString();
                ChangeState(CountryDropDown, new EventArgs());
                if (StateTextBoxPanel.Visible)
                    StateTextBox.Text = dvUser[0]["CatState"].ToString();
                else
                    StateDropDown.Items.FindItemByText(dvUser[0]["CatState"].ToString()).Selected = true;

                CityTextBox.Text = dvUser[0]["CatCity"].ToString();
                StateChanged(StateDropDown, new EventArgs());

                if (dvUser[0]["CatZip"] != null)
                {
                    if (dvUser[0]["CatZip"].ToString().Trim() != "")
                    {

                        locationSearchPanel.Items[1].Expanded = true;
                        Telerik.Web.UI.RadTextBox ZipTextBox = (Telerik.Web.UI.RadTextBox)locationSearchPanel.Items[1].Items[0].FindControl("ZipTextBox");

                        Panel RadiusTitlePanel = (Panel)locationSearchPanel.Items[1].Items[0].FindControl("RadiusTitlePanel");
                        Panel RadiusDropPanel = (Panel)locationSearchPanel.Items[1].Items[0].FindControl("RadiusDropPanel");

                        char[] delim = { ';' };
                        string[] tokens = dvUser[0]["CatZip"].ToString().Split(delim);

                        if (tokens.Length > 1)
                        {
                            ZipTextBox.Text = tokens[1].Trim();

                            Telerik.Web.UI.RadComboBox RadiusDropDown = (Telerik.Web.UI.RadComboBox)locationSearchPanel.Items[1].Items[0].FindControl("RadiusDropDown");

                            if (dvUser[0]["Radius"] != null)
                            {
                                if (dvUser[0]["Radius"].ToString().Trim() != "")
                                {

                                    RadiusDropDown.SelectedValue = dvUser[0]["Radius"].ToString();
                                }
                            }
                        }
                        else
                        {
                            locationSearchPanel.Items[0].Expanded = true;
                        }
                    }
                    else
                    {
                        locationSearchPanel.Items[0].Expanded = true;
                    }
                }
                else
                {
                    locationSearchPanel.Items[0].Expanded = true;
                }
            }
        }
        else
        {
            if (Session["LocationVenues"] != null)
                fillVenues((DataSet)Session["LocationVenues"]);
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        //Literal lit = new Literal();
        //lit.Text = "<style type=\"text/css\">.RadComboBox_Black, .RadComboBox_Black " +
        //    ".rcbInputCell .rcbInput, .RadComboBoxDropDown_Black{border: 1px solid #1FB6E7;}" +
        //    "html body .RadInput_Black .riTextBox, html body .RadInputMgr_Black{border: 1px solid #1FB6E7 !important;}</style>";

        //Page.Header.Controls.Add(lit);
 
    }

    protected void GoToLogin(object sender, EventArgs e)
    {
        Session["RedirectTo"] = "EnterEvent.aspx";
        Response.Redirect("EnterEvent.aspx");
    }

    //protected void ChangeDateDropDown(object sender, EventArgs e)
    //{
    //    //Telerik.Web.UI.RadComboBox DateDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("DateDropDown");
    //    Telerik.Web.UI.RadDatePicker rPick = (Telerik.Web.UI.RadDatePicker) DateDropDown.Items[0].FindControl("RadDatePicker2");
    //    DateDropDown.Text = rPick.DbSelectedDate.ToString();
    //    //SearchPanel.Items[0].Expanded = true;
    //    //SearchPanel.Items[1].Expanded = false;
    //}

    protected void ChangeCheckImage(object sender, EventArgs e)
    {
        ImageButton CheckImageButton = (ImageButton)FindControlRecursive(this, "CheckImageButton");

        if (CheckImageButton.ImageUrl == "image/Check.png")
            CheckImageButton.ImageUrl = "image/CheckSelected.png";
        else
        {
            CheckImageButton.ImageUrl = "image/Check.png";
            CheckBoxList CategoriesCheckBoxes = (CheckBoxList)FindControlRecursive(this, "CategoriesCheckBoxes");
            for (int i = 0; i < CategoriesCheckBoxes.Items.Count; i++)
                CategoriesCheckBoxes.Items[i].Selected = false;
        }
    }

    protected void CheckCheckImage(object sender, EventArgs e)
    {
        ImageButton CheckImageButton = (ImageButton)FindControlRecursive(this, "CheckImageButton");
        CheckImageButton.ImageUrl = "image/CheckSelected.png";
    }

    //protected void SetDateBoxToday(object sender, EventArgs e)
    //{

    //    Telerik.Web.UI.RadComboBox DateDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("DateDropDown");
    //    DateDropDown.Items[0].Text = "Today";

    //    //SearchPanel.Items[0].Expanded = true;
    //    //SearchPanel.Items[1].Expanded = false;
    //}

    //protected void SetSelectedDateServer(object sender, EventArgs e)
    //{
    //    Telerik.Web.UI.RadComboBox DateDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("DateDropDown");
    //    Telerik.Web.UI.RadDatePicker RadDatePicker2 = (Telerik.Web.UI.RadDatePicker)DateDropDown.Items[0].FindControl("RadDatePicker2");

    //    DateDropDown.Items[0].Text = RadDatePicker2.DbSelectedDate.ToString().Replace(" 12:00:00 AM","");

    //    //SearchPanel.Items[0].Expanded = true;
    //    //SearchPanel.Items[1].Expanded = false;
    //}

    //protected void SetDateBoxTomorrow(object sender, EventArgs e)
    //{

    //    Telerik.Web.UI.RadComboBox DateDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("DateDropDown");
    //    DateDropDown.Items[0].Text = "Tomorrow";

    //    //SearchPanel.Items[0].Expanded = true;
    //    //SearchPanel.Items[1].Expanded = false;
    //}

    //protected void SetDateBoxThisWeekend(object sender, EventArgs e)
    //{

    //    Telerik.Web.UI.RadComboBox DateDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("DateDropDown");
    //    DateDropDown.Items[0].Text = "This Weekend";

    //    //SearchPanel.Items[0].Expanded = true;
    //    //SearchPanel.Items[1].Expanded = false;
    //}

    //protected void SetDateBoxThisWeek(object sender, EventArgs e)
    //{

    //    Telerik.Web.UI.RadComboBox DateDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("DateDropDown");
    //    DateDropDown.Items[0].Text = "This Week";

    //    //SearchPanel.Items[0].Expanded = true;
    //    //SearchPanel.Items[1].Expanded = false;
    //}

    //protected void SetDateBoxNextWeek(object sender, EventArgs e)
    //{

    //    Telerik.Web.UI.RadComboBox DateDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("DateDropDown");
    //    DateDropDown.Items[0].Text = "Next Week";

    //    //SearchPanel.Items[0].Expanded = true;
    //    //SearchPanel.Items[1].Expanded = false;
    //}

    //protected void SetDateBoxThisMonth(object sender, EventArgs e)
    //{

    //    Telerik.Web.UI.RadComboBox DateDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("DateDropDown");
    //    DateDropDown.Items[0].Text = "This Month";

    //    //SearchPanel.Items[0].Expanded = true;
    //    //SearchPanel.Items[1].Expanded = false;
    //}

    //protected void SetDateBoxAllPastEvents(object sender, EventArgs e)
    //{

    //    Telerik.Web.UI.RadComboBox DateDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("DateDropDown");
    //    DateDropDown.Items[0].Text = "All Past Events";

    //    //SearchPanel.Items[0].Expanded = true;
    //    //SearchPanel.Items[1].Expanded = false;
    //}

    //protected void SetDateBoxAllFutureEvents(object sender, EventArgs e)
    //{

    //    Telerik.Web.UI.RadComboBox DateDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("DateDropDown");
    //    DateDropDown.Items[0].Text = "All Future Events";

    //    //SearchPanel.Items[0].Expanded = true;
    //    //SearchPanel.Items[1].Expanded = false;
    //}

    protected void Search(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
            //SearchPanel.Items[0].Expanded = false;
            //SearchPanel.Items[1].Expanded = true;

            //Telerik.Web.UI.RadPanelBar locationSearchPanel = (Telerik.Web.UI.RadPanelBar)SearchPanel.Items[0].Items[0].FindControl("locationSearchPanel");
        string message = "";
            Panel StateDropDownPanel = (Panel)locationSearchPanel.Items[0].Items[0].FindControl("StateDropDownPanel");
            //Label ResultsLabel = (Label)SearchPanel.Items[1].Items[0].FindControl("ResultsLabel");
            //SearchElements SearchElements2 = (SearchElements)SearchPanel.Items[1].Items[0].FindControl("SearchElements2");
            //ASP.controls_hippotextbox_ascx SearchTextBox = (ASP.controls_hippotextbox_ascx)SearchPanel.Items[0].Items[0].FindControl("SearchTextBox");
            //Telerik.Web.UI.RadComboBox DateDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("DateDropDown");
            //Telerik.Web.UI.RadComboBox CountryDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("CountryDropDown");
            Telerik.Web.UI.RadComboBox StateDropDown = (Telerik.Web.UI.RadComboBox)locationSearchPanel.Items[0].Items[0].FindControl("StateDropDown");
            //Telerik.Web.UI.RadComboBox VenueDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("VenueDropDown");
            Telerik.Web.UI.RadTextBox StateTextBox = (Telerik.Web.UI.RadTextBox)locationSearchPanel.Items[0].Items[0].FindControl("StateTextBox");
            Telerik.Web.UI.RadTextBox CityTextBox = (Telerik.Web.UI.RadTextBox)locationSearchPanel.Items[0].Items[0].FindControl("CityTextBox");
            //Telerik.Web.UI.RadComboBox RatingDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("RatingDropDown");
            //Telerik.Web.UI.RadTreeView CategoryTree = (Telerik.Web.UI.RadTreeView)SearchPanel.Items[0].Items[0].FindControl("CategoryTree");
            //Telerik.Web.UI.RadTreeView RadTreeView1 = (Telerik.Web.UI.RadTreeView)SearchPanel.Items[0].Items[0].FindControl("RadTreeView1");
            Telerik.Web.UI.RadComboBox RadiusDropDown = (Telerik.Web.UI.RadComboBox)locationSearchPanel.Items[1].Items[0].FindControl("RadiusDropDown");
            //Label MessageLabel = (Label)SearchPanel.Items[0].Items[0].FindControl("MessageLabel");
            //MessageLabel.Text = "";
            //Button SearchButton = (Button)SearchPanel.Items[0].Items[0].FindControl("SearchButton");
            Telerik.Web.UI.RadTextBox ZipTextBox = (Telerik.Web.UI.RadTextBox)locationSearchPanel.Items[1].Items[0].FindControl("ZipTextBox");

            Panel RadiusTitlePanel = (Panel)locationSearchPanel.Items[1].Items[0].FindControl("RadiusTitlePanel");
            Panel RadiusDropPanel = (Panel)locationSearchPanel.Items[1].Items[0].FindControl("RadiusDropPanel");
            Panel USLabelPanel = (Panel)locationSearchPanel.Items[1].Items[0].FindControl("USLabelPanel");

            //HtmlGenericControl TimeFrameDiv = (HtmlGenericControl)SearchPanel.Items[0].Items[0].FindControl("TimeFrameDiv");

            string resultsStr = "";

            try
            {
                //SearchElements2.Clear();

                Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

                SearchTextBox.THE_TEXT = dat.stripHTML(SearchTextBox.THE_TEXT);

                char[] delim = { ' ' };
                string[] tokens;

                tokens = SearchTextBox.THE_TEXT.Split(delim);
                string temp = "";
                if (SearchTextBox.THE_TEXT.Trim() != "")
                {
                    for (int i = 0; i < tokens.Length; i++)
                    {
                        temp += " E.Header LIKE @search" + i.ToString();

                        if (i + 1 != tokens.Length)
                            temp += " AND ";
                    }
                    resultsStr += "'" + SearchTextBox.THE_TEXT + "' ";
                }
                if (temp != "")
                    temp = " AND " + temp;

                bool isZip = false;

                if (locationSearchPanel.Items[1].Expanded && ZipTextBox.Text.Trim() != "")
                {
                    isZip = true;
                }
                else
                {
                    if (CityTextBox.Text.Trim() == "")
                        isZip = true;
                }

                
                bool goOn = true;
                string country = " AND V.Country=" + CountryDropDown.SelectedValue;

                string state = "";
                string stateParam = "";
                string city = "";
                string cityParam = "";

                string zip = "";
                string zipParameter = "";
                int zipParam = 0;
                string nonExistantZip = "";

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
                                    else
                                    {
                                        zip = " AND V.Zip='" + ZipTextBox.Text.Trim() + "'";
                                    }
                                }
                                else
                                {
                                    zip = " AND V.Zip='" + ZipTextBox.Text.Trim() + "'";
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
                            zip = " AND V.Zip = @zip ";
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
                    if (StateDropDownPanel.Visible)
                    {
                        if (StateDropDown.SelectedValue != "-1")
                        {
                            state = " AND V.State=@state ";
                            stateParam = StateDropDown.SelectedItem.Text;
                            resultsStr += " in " + StateDropDown.SelectedItem.Text;
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
                            state = " AND V.State=@state ";
                            stateParam = StateTextBox.Text;
                            resultsStr += " in " + StateTextBox.Text;
                        }
                        else
                        {
                            goOn = false;
                            message = "You must at the very least include a zip code or State and City.";
                        }
                    }

                    if (CityTextBox.Text != "")
                    {
                        city = " AND V.City LIKE @city ";
                        cityParam = CityTextBox.Text.ToLower();
                        resultsStr += ", " + CityTextBox.Text.ToLower();
                    }
                    else
                    {
                        goOn = false;
                        message = "You must at the very least include a zip code or State and City.";
                    }
                }

                if (goOn)
                {
                    string date = "";
                    if (Session["SetTime"] != null)
                    {
                        string test = Session["SetTime"].ToString();
                        string theDate = "";
                        int subtraction = 0;
                        string startDate = "";
                        string endDate = "";

                        resultsStr += ", time frame: " + test;

                        switch (test)
                        {
                            case "Today":
                                theDate = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).ToShortDateString();
                                date = " AND (CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) " +
                                    " = CONVERT(DATETIME, '" + theDate + "') OR (CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + "+
                                    "CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) < "+
                                    "CONVERT(DATETIME, '" + theDate + "') AND CONVERT(NVARCHAR,MONTH(EO.DateTimeEnd)) + '/' + CONVERT(NVARCHAR,DAY(EO.DateTimeEnd)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeEnd)) > CONVERT(DATETIME, '" + theDate + "'))) ";
                                break;
                            case "Tomorrow":
                                theDate = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(1.00).ToShortDateString();
                                date = " AND (CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) " +
                                    " = CONVERT(DATETIME, '" + theDate + "') OR (CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + " +
                                    "CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) < " +
                                    "CONVERT(DATETIME, '" + theDate + "') AND CONVERT(NVARCHAR,MONTH(EO.DateTimeEnd)) + '/' + CONVERT(NVARCHAR,DAY(EO.DateTimeEnd)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeEnd)) > CONVERT(DATETIME, '" + theDate + "')))";
                                break;
                            case "This Weekend":

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
                                startDate = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Subtract(TimeSpan.FromDays(subtraction)).ToShortDateString();
                                endDate = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(2 - subtraction).ToShortDateString();
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
                            case "This Week":
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
                                startDate = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Subtract(TimeSpan.FromDays(subtraction)).ToShortDateString();
                                endDate = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(7.00 - subtraction).ToShortDateString();
                                date = " AND (CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) " +
                                    " >= CONVERT(DATETIME, '" + startDate + "') AND CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + "+
                                    "CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) <= CONVERT(DATETIME, '" + endDate + "')OR "+
                                    "(CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + "+
                                    "CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) < "+
                                    "CONVERT(DATETIME, '" + startDate + "') AND CONVERT(NVARCHAR,MONTH(EO.DateTimeEnd)) + '/' + "+
                                    "CONVERT(NVARCHAR,DAY(EO.DateTimeEnd)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeEnd)) > CONVERT(DATETIME, '" + startDate + "')))";
                                break;
                            case "Next Week":
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
                                startDate = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Subtract(TimeSpan.FromDays(subtraction)).ToShortDateString();
                                endDate = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(7.00 - subtraction).ToShortDateString();
                                date = " AND ((CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) " +
                                    " >= CONVERT(DATETIME, '" + startDate + "') AND CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) <= CONVERT(DATETIME, '" + endDate + "'))  OR "+
                                    "(CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + "+
                                    "CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) < "+
                                    "CONVERT(DATETIME, '" + startDate + "') AND CONVERT(NVARCHAR,MONTH(EO.DateTimeEnd)) + '/' + "+
                                    "CONVERT(NVARCHAR,DAY(EO.DateTimeEnd)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeEnd)) > CONVERT(DATETIME, '" + startDate + "'))) ";

                                break;
                            case "This Month":
                                date = " AND (MONTH(EO.DateTimeStart) = '" + DateTime.Parse(cookie.Value.ToString().Replace("%20",
                                    " ").Replace("%3A", ":")).Month + "' OR (EO.DateTimeStart < CONVERT(DATETIME,'" + DateTime.Parse(cookie.Value.ToString().Replace("%20",
                                    " ").Replace("%3A", ":")).Month + "/1/" + DateTime.Parse(cookie.Value.ToString().Replace("%20",
                                    " ").Replace("%3A", ":")).Year + "') AND EO.DateTimeEnd > CONVERT(DATETIME,'" + DateTime.Parse(cookie.Value.ToString().Replace("%20",
                                    " ").Replace("%3A", ":")).Month + "/1/" + DateTime.Parse(cookie.Value.ToString().Replace("%20",
                                    " ").Replace("%3A", ":")).Year + "')))";
                                break;
                            case "All Past Events":
                                date = " AND CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) " +
                                    " <= CONVERT(DATETIME, '" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).ToString() + "') ";

                                break;
                            case "All Future Events":
                                date = " AND (CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + "+
                                    "CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + "+
                                    "'/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) " +
                                    " >= CONVERT(DATETIME, '" + DateTime.Parse(cookie.Value.ToString().Replace("%20",
                                    " ").Replace("%3A", ":")).ToString() + "') OR (CONVERT(NVARCHAR,MONTH(EO.DateTimeEnd)) + '/' + " +
                                    "CONVERT(NVARCHAR,DAY(EO.DateTimeEnd)) + " +
                                    "'/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeEnd)) " +
                                    " >= CONVERT(DATETIME, '" + DateTime.Parse(cookie.Value.ToString().Replace("%20",
                                    " ").Replace("%3A", ":")).ToString() + "')))";

                                break;
                            default:
                                //Must be 'Next .. Days'
                                if (test.Substring(0, 4) == "Next")
                                {
                                    date = " AND CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) " +
                                        " >= CONVERT(DATETIME, '" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).ToString() + "') " +
                                        " AND CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) <= " +
                                        " CONVERT(DATETIME, '" + DateTime.Parse(cookie.Value.ToString().Replace("%20",
                                        " ").Replace("%3A", ":")).AddDays(double.Parse(test.Replace("Next ",
                                        "").Replace(" Days", ""))).ToString() + "')";
                                }
                                else
                                {
                                    //Telerik.Web.UI.RadDatePicker radP = (Telerik.Web.UI.RadDatePicker)DateDropDown.Items[0].FindControl("RadDatePicker2");
                                    DateTime tempTime = DateTime.Parse(TimeFrameDiv.InnerHtml);
                                    theDate = tempTime.ToString().Replace(" 12:00:00 AM", "");
                                    date = " AND CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) " +
                                        " = CONVERT(DATETIME, '" + theDate + "') ";
                                }
                                break;
                        }


                    }

                    string rating = "";
                    if (RatingDropDown.SelectedValue != "-1" && RatingDropDown.SelectedItem.Text.ToLower() != "don't care")
                    {
                        rating = " AND E.StarRating >= " + RatingDropDown.SelectedItem.Text;

                        resultsStr += ", lowest rating: " + RatingDropDown.SelectedItem.Text;
                    }
                    string venue = "";
                    if (Session["SetVenue"] != null)
                    {
                        char[] delim2 = { ';' };
                        string[] toks = Session["SetVenue"].ToString().Split(delim2);
                        venue = " AND E.Venue = " + toks[0];
                        resultsStr += ", venue: " + toks[1].Replace("%20", " ");
                    }
                    string categories = "";
                    string addCat = "";
                    string resultsCats = "";
                    for (int i = 0; i < CategoryTree.Nodes.Count; i++)
                    {
                        if (CategoryTree.Nodes[i].Checked)
                        {
                            if (categories == "")
                            {
                                addCat = ", Event_Category_Mapping ECM ";
                                categories += " AND E.ID=ECM.EventID AND (";
                                categories += " ECM.CategoryID = " + CategoryTree.Nodes[i].Value;
                            }
                            else
                                categories += " OR ECM.CategoryID = " + CategoryTree.Nodes[i].Value;

                            if (resultsCats == "")
                            {
                                resultsCats += ", categories: " + CategoryTree.Nodes[i].Text;
                            }
                            else
                            {
                                resultsCats += ", " + CategoryTree.Nodes[i].Text;
                            }

                            if (CategoryTree.Nodes[i].Nodes.Count > 0)
                            {
                                for (int h = 0; h < CategoryTree.Nodes[i].Nodes.Count; h++)
                                {
                                    categories += " OR ECM.CategoryID = " + CategoryTree.Nodes[i].Nodes[h].Value;

                                    if (CategoryTree.Nodes[i].Nodes[h].Nodes.Count > 0)
                                    {
                                        for (int k = 0; k < CategoryTree.Nodes[i].Nodes[h].Nodes.Count; k++)
                                        {
                                            categories += " OR ECM.CategoryID = " + CategoryTree.Nodes[i].Nodes[h].Nodes[k].Value;
                                        }
                                    }
                                }
                            }
                        }

                        for (int n = 0; n < CategoryTree.Nodes[i].Nodes.Count; n++)
                        {
                            if (CategoryTree.Nodes[i].Nodes[n].Checked)
                            {
                                if (categories == "")
                                {
                                    addCat = ", Event_Category_Mapping ECM ";
                                    categories += " AND E.ID=ECM.EventID AND (";
                                    categories += " ECM.CategoryID = " + CategoryTree.Nodes[i].Nodes[n].Value;
                                }
                                else
                                    categories += " OR ECM.CategoryID = " + CategoryTree.Nodes[i].Nodes[n].Value;

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
                        if (RadTreeView1.Nodes[i].Checked)
                        {
                            if (categories == "")
                            {
                                addCat = ", Event_Category_Mapping ECM ";
                                categories += " AND E.ID=ECM.EventID AND (";
                                categories += " ECM.CategoryID = " + RadTreeView1.Nodes[i].Value;
                            }
                            else
                                categories += " OR ECM.CategoryID = " + RadTreeView1.Nodes[i].Value;

                            if (resultsCats == "")
                            {
                                resultsCats += ", categories: " + RadTreeView1.Nodes[i].Text;
                            }
                            else
                            {
                                resultsCats += ", " + RadTreeView1.Nodes[i].Text;
                            }

                            if (RadTreeView1.Nodes[i].Nodes.Count > 0)
                            {
                                for (int h = 0; h < RadTreeView1.Nodes[i].Nodes.Count; h++)
                                {
                                    categories += " OR ECM.CategoryID = " + RadTreeView1.Nodes[i].Nodes[h].Value;

                                    if (RadTreeView1.Nodes[i].Nodes[h].Nodes.Count > 0)
                                    {
                                        for (int k = 0; k < RadTreeView1.Nodes[i].Nodes[h].Nodes.Count; k++)
                                        {
                                            categories += " OR ECM.CategoryID = " + RadTreeView1.Nodes[i].Nodes[h].Nodes[k].Value;
                                        }
                                    }
                                }
                            }
                        }

                        for (int n = 0; n < RadTreeView1.Nodes[i].Nodes.Count; n++)
                        {
                            if (RadTreeView1.Nodes[i].Nodes[n].Checked)
                            {
                                if (categories == "")
                                {
                                    addCat = ", Event_Category_Mapping ECM ";
                                    categories += " AND E.ID=ECM.EventID AND (";
                                    categories += " ECM.CategoryID = " + RadTreeView1.Nodes[i].Nodes[n].Value;
                                }
                                else
                                    categories += " OR ECM.CategoryID = " + RadTreeView1.Nodes[i].Nodes[n].Value;

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

                    resultsStr += resultsCats;

                    if (categories != "")
                        categories += " ) ";

                    string searchStr = "SELECT DISTINCT E.ID AS EID, CONVERT(NVARCHAR, E.ID)+'E' AS HashID, V.ID AS VID, EO.DateTimeStart, E.Header, "+
                        "V.Name, E.EventGoersCount, 'False' AS isGroup, EO.ID AS ReoccurrID, 'False'+';'+CONVERT(NVARCHAR,E.ID)+';'+CONVERT(NVARCHAR, EO.ID) AS CalendarKey FROM Events E, Venues V, "
                        + "Event_Occurance EO " + addCat + " WHERE E.ID=EO.EventID AND E.Venue=V.ID " +
                        country + state + city + zip + date + rating + venue + categories +
                        " AND E.Live='True' AND V.Live='True' " + temp + " ORDER BY E.Header";

                    string groupSearchStr = "SELECT DISTINCT E.ID AS EID, EO.Country, CONVERT(NVARCHAR, E.ID)+'G'+"+
                        "CONVERT(NVARCHAR, EO.ID) AS HashID, EO.DateTimeStart, EO.ID AS ReoccurrID,  "+
                        "E.Name AS Header, 'True' AS isGroup, 'True'+';'+CONVERT(NVARCHAR,E.ID)+';'+"+
                        "CONVERT(NVARCHAR, EO.ID) AS CalendarKey  " +
                        " FROM GroupEvents E, "
                        + "GroupEvent_Occurance EO " + addCat.Replace("Event_Category_Mapping", "GroupEvent_Category") +
                        " WHERE E.LIVE='True' AND E.EventType = '1' AND E.ID=EO.GroupEventID " +
                        country.Replace("V.", "EO.") + state.Replace("V.", "EO.") + city.Replace("V.", "EO.") +
                        zip.Replace("V.", "EO.") + date + venue.Replace("E.", "EO.").Replace("Venue", "VenueID") +categories.Replace(".EventID",
                        ".GroupEvent_ID").Replace(".CategoryID", ".CATEGORY_ID") +
                        " " + temp.Replace(".Header", ".Name") + " ORDER BY E.Name";

                    //MessageLabel.Text = groupSearchStr;

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
                    message = searchStr;
                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                    DataSet dsAll = ds;
                    //Search Group Events
                    if (GroupCheckBox.Checked)
                    {
                        cmd = new SqlCommand(groupSearchStr, conn);
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
                        DataSet dsGroups = new DataSet();
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dsGroups);

                        if (dsAll.Tables[0].Rows.Count == 0)
                            dsAll = dsGroups;
                        else
                            dsAll = MergeDS(ds, dsGroups);
                    }

                    Session["SearchDS"] = dsAll;
                    Session["Searching"] = "Events";
                    Session["SearchResults"] = "Results " + resultsStr;
                    conn.Close();

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
                    
                    //DropDownList SortDropDown = (DropDownList)SearchPanel.Items[1].Items[0].FindControl("SortDropDown");

                    if (setZero)
                    {
                        //SearchPanel.Items[1].Text = "<div style='position: relative;'><span style='font-family: Arial; font-size: 20px; color: White;'>Search Results: </span><div style='position: absolute; right: 5px; top: 3px;'>(0 Records Found)</div></div>";
                        //SortDropDown.Visible = false;

                        Session["SearchDS"] = null;
                        RadWindow RadWindow3 = Master.RAD_WINDOW_3;
                        RadWindow3.Left = 178;
                        RadWindow3.Width = 910;
                        RadWindow3.Height = 550;
                        RadWindow3.Top = 100;
                        RadWindow3.VisibleOnPageLoad = true;
                    }
                    else
                    {
                        //SortDropDown.Visible = true;
                        RadWindow RadWindow3 = Master.RAD_WINDOW_3;
                        RadWindow3.Left = 178;
                        RadWindow3.Width = 910;
                        RadWindow3.Height = 550;
                        RadWindow3.Top = 100;
                        RadWindow3.VisibleOnPageLoad = true;
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
                    //SearchPanel.Items[1].Expanded = false;
                    //SearchPanel.Items[0].Expanded = true;
                    //SearchButton.PostBackUrl = "~/EventSearch.aspx#SearchTag";
                }
            }
            catch (Exception ex)
            {
                MessageLabel.Text += ex.ToString() + "<br/>" + message;
            }
    }

    protected DataSet MergeDS(DataSet ds, DataSet dsGroup)
    {
        DataTable dt = ds.Tables[0];

        DataRow newRow;
        foreach (DataRow row in dsGroup.Tables[0].Rows)
        {
            newRow = dt.NewRow();
            newRow["EID"] = row["EID"];
            newRow["HashID"] = row["HashID"];
            newRow["DateTimeStart"] = row["DateTimeStart"];
            newRow["Header"] = row["Header"];
            newRow["isGroup"] = row["isGroup"];
            newRow["ReoccurrID"] = row["ReoccurrID"];
            newRow["CalendarKey"] = row["CalendarKey"];
            dt.Rows.Add(newRow);
        }

        return ds;
    }

    //protected void SearchAdvanced(object sender, EventArgs e)
    //{
    //    SearchElements.Clear();
    //    RadPanelBar1.CollapseAllItems();
    //    ASP.controls_hippotextbox_ascx eventname = (ASP.controls_hippotextbox_ascx)FindControlRecursive(this, 
    //        "EventNameTextBox");

    //    string queryBegin = "SELECT DISTINCT E.ID AS EID, V.ID AS VID, E.Header, E.Venue, E.Address, E.EventGoersCount, "
    //        +" E.Expense, V.Name, EO.DateTimeStart, EO.DateTimeEnd FROM Events E, Venues V, Event_Occurance EO, "
    //        +"Event_Category_Mapping ECM WHERE E.ID = EO.EventID AND E.Live='True' AND V.Live='True' AND V.ID=E.Venue ";
    //    string query = "";
        
    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //    ImageButton CheckImageButton = (ImageButton)FindControlRecursive(this, "CheckImageButton");
    //    CheckBoxList CategoriesCheckBoxes = (CheckBoxList)FindControlRecursive(this, "CategoriesCheckBoxes");
    //    if (CheckImageButton.ImageUrl == "image/CheckSelected.png")
    //    {
    //        queryBegin += " AND ECM.EventID = E.ID ";
    //        for (int i = 0; i < CategoriesCheckBoxes.Items.Count;i++ )
    //        {
    //            if (CategoriesCheckBoxes.Items[i].Selected)
    //            {
    //                if (query == "")
    //                {
    //                    query += " AND (";
    //                    query += " ECM.CategoryID = " + CategoriesCheckBoxes.Items[i].Value;
    //                }else
    //                    query += " OR ECM.CategoryID = " + CategoriesCheckBoxes.Items[i].Value;
    //            }

                
    //        }
    //        if (query != "")
    //            query += " ) ";
    //    }
    //    char[] delim = { ' ' };
    //    string[] tokens;
    //    string temp = "";
    //    if (eventname.THE_TEXT != "")
    //    {
    //        tokens = eventname.THE_TEXT.Split(delim);
    //        temp = " ( ";
    //        for (int i = 0; i < tokens.Length; i++)
    //        {
    //            temp += " E.Header LIKE @header"+i.ToString();

    //            if (i + 1 != tokens.Length)
    //                temp += " OR ";
    //        }

    //        temp += " ) ";

    //            query += " AND " + temp;
    //    }


    //    Telerik.Web.UI.RadDateTimePicker startDate = (Telerik.Web.UI.RadDateTimePicker)FindControlRecursive(this, "StartDateTimePicker");
    //    if (startDate.DbSelectedDate != null)
    //    {
    //        query += " AND EO.DateTimeStart >= '" + startDate.DbSelectedDate.ToString()+"'";
    //    }
    //    Telerik.Web.UI.RadDateTimePicker endDate = (Telerik.Web.UI.RadDateTimePicker)FindControlRecursive(this, "EndDateTimePicker");
    //    if (endDate.DbSelectedDate != null)
    //    {
    //        query += " AND EO.DateTimeEnd <= '" + endDate.DbSelectedDate.ToString()+"'";
    //    }
    //    ImageButton ImageButton1 = (ImageButton)FindControlRecursive(this, "ImageButton1");
    //    ASP.controls_hippotextbox_ascx countTextBox = (ASP.controls_hippotextbox_ascx)FindControlRecursive(this, "CountTextBox");
    //    if (ImageButton1.ImageUrl == "image/RadioButtonSelected.png" && countTextBox.THE_TEXT != "")
    //    {
    //        try
    //        {
    //            int temp2 = int.Parse(countTextBox.THE_TEXT);
    //            query += " AND E.EventGoersCount <= " + countTextBox.THE_TEXT;
    //        }
    //        catch (Exception ex)
    //        {

    //        }
    //    }
    //    ImageButton ImageButton2 = (ImageButton)FindControlRecursive(this, "ImageButton2");
    //    if (ImageButton2.ImageUrl == "image/RadioButtonSelected.png" && countTextBox.THE_TEXT != "")
    //    {
    //        try
    //        {
    //            int temp2 = int.Parse(countTextBox.THE_TEXT);
    //            query += " AND E.EventGoersCount >= " + countTextBox.THE_TEXT;
    //        }
    //        catch (Exception ex)
    //        {

    //        }
    //    }
    //    DropDownList RatingDropDown = (DropDownList)FindControlRecursive(this, "RatingDropDown");
    //    if (RatingDropDown.SelectedItem.Text != "")
    //    {
    //        query += " AND E.StarRating >= "+RatingDropDown.SelectedItem.Text;
    //    }
    //    ASP.controls_hippotextbox_ascx VenueName = (ASP.controls_hippotextbox_ascx)FindControlRecursive(this, "VenueNameTextBox");

    //    if (VenueName.THE_TEXT != "")
    //    {
    //        tokens = VenueName.THE_TEXT.Split(delim);
    //        temp = " ( ";
    //        for (int i = 0; i < tokens.Length; i++)
    //        {
    //            temp += " V.Name LIKE @venue" + i.ToString();

    //            if (i + 1 != tokens.Length)
    //                temp += " OR ";
    //        }

    //        temp += " ) ";
          
    //            query += " AND " + temp;
    //    }


    //    ASP.controls_hippotextbox_ascx Location = (ASP.controls_hippotextbox_ascx)FindControlRecursive(this, "LocationTextBox");

    //    if (Location.THE_TEXT != "")
    //    {
    //        tokens = Location.THE_TEXT.Split(delim);
    //        temp = " ( ";
    //        for (int i = 0; i < tokens.Length; i++)
    //        {
    //            temp += " E.Address LIKE @address" + i.ToString();

    //            if (i + 1 != tokens.Length)
    //                temp += " OR ";
    //        }

    //        temp += " ) ";
           
    //            query += " AND " + temp;
    //    }

    //    ASP.controls_hippotextbox_ascx Min = (ASP.controls_hippotextbox_ascx)FindControlRecursive(this, "ExpenseMinTextBox");
    //    if (Min.THE_TEXT != "")
    //    {
    //        try
    //        {
    //            decimal temp3 = decimal.Parse(Min.THE_TEXT);
    //            query += " AND Expense >= " + Min.THE_TEXT;
    //        }
    //        catch (Exception ex)
    //        {

    //        }
    //    }
    //    ASP.controls_hippotextbox_ascx Max = (ASP.controls_hippotextbox_ascx)FindControlRecursive(this, "ExpenseMaxTextBox");
    //    if (Max.THE_TEXT != "")
    //    {
    //        try
    //        {
    //            decimal temp4 = decimal.Parse(Max.THE_TEXT);
    //            query += " AND Expense <= " + Max.THE_TEXT;
    //        }
    //        catch (Exception ex)
    //        {

    //        }
    //    }

    //    if (query != "")
    //    {
            
    //        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
    //        SqlCommand cmd = new SqlCommand(queryBegin+query, conn);
    //        tokens = eventname.THE_TEXT.Split(delim);
    //        for (int i = 0; i < tokens.Length; i++)
    //        {
    //            cmd.Parameters.Add("@header" + i.ToString(), SqlDbType.NVarChar).Value = "%" + tokens[i] + "%";
    //        }
    //        tokens = VenueName.THE_TEXT.Split(delim);
    //        for (int i = 0; i < tokens.Length; i++)
    //        {
    //            cmd.Parameters.Add("@venue" + i.ToString(), SqlDbType.NVarChar).Value = "%" + tokens[i] + "%";
    //        }
    //        tokens = Location.THE_TEXT.Split(delim);
    //        for (int i = 0; i < tokens.Length; i++)
    //        {
    //            cmd.Parameters.Add("@address" + i.ToString(), SqlDbType.NVarChar).Value = "%" + tokens[i] + "%";
    //        }
    //        DataSet ds = new DataSet();
    //        SqlDataAdapter da = new SqlDataAdapter(cmd);
    //        da.Fill(ds);

    //        ViewState["SearchDS"] = ds;
    //        if (ds.Tables.Count > 0)
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                SearchElements.EVENTS_DS = ds;
    //                SearchElements.DataBind2();

    //                NumResultsLabel.Text = "(" + ds.Tables[0].Rows.Count + " Records Found)"; ;

    //                SearchResultsTitleLabel.Text = "Search results:";
    //            }
    //            else
    //                NumResultsLabel.Text = "(0 Records Found)";
    //        else
    //            NumResultsLabel.Text = "(0 Records Found)";
    //    }
    //    else
    //        NumResultsLabel.Text = "(0 Records Found)";


    //}

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
        //Telerik.Web.UI.RadPanelBar locationSearchPanel = (Telerik.Web.UI.RadPanelBar)SearchPanel.Items[0].Items[0].FindControl("locationSearchPanel");

        Panel StateDropDownPanel = (Panel)locationSearchPanel.Items[0].Items[0].FindControl("StateDropDownPanel");
        Panel StateTextBoxPanel = (Panel)locationSearchPanel.Items[0].Items[0].FindControl("StateTextBoxPanel");

        //SearchElements SearchElements2 = (SearchElements)SearchPanel.Items[1].Items[0].FindControl("SearchElements2");
        //ASP.controls_hippotextbox_ascx SearchTextBox = (ASP.controls_hippotextbox_ascx)SearchPanel.Items[1].FindControl("SearchTextBox");
        //Telerik.Web.UI.RadComboBox DateDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("DateDropDown");
        //Telerik.Web.UI.RadComboBox CountryDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("CountryDropDown");
        Telerik.Web.UI.RadComboBox StateDropDown = (Telerik.Web.UI.RadComboBox)locationSearchPanel.Items[0].Items[0].FindControl("StateDropDown");
        //Telerik.Web.UI.RadComboBox VenueDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("VenueDropDown");
        Telerik.Web.UI.RadTextBox StateTextBox = (Telerik.Web.UI.RadTextBox)locationSearchPanel.Items[0].Items[0].FindControl("StateTextBox");
        Telerik.Web.UI.RadTextBox CityTextBox = (Telerik.Web.UI.RadTextBox)locationSearchPanel.Items[0].Items[0].FindControl("CityTextBox");
        //Telerik.Web.UI.RadComboBox RatingDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("RatingDropDown");
        //Telerik.Web.UI.RadTreeView CategoryTree = (Telerik.Web.UI.RadTreeView)SearchPanel.Items[0].Items[0].FindControl("CategoryTree");
        //Telerik.Web.UI.RadTreeView RadTreeView1 = (Telerik.Web.UI.RadTreeView)SearchPanel.Items[0].Items[0].FindControl("RadTreeView1");

        //Label MessageLabel = (Label)SearchPanel.Items[0].Items[0].FindControl("MessageLabel");
        //MessageLabel.Text = "";

        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
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
        //Telerik.Web.UI.RadPanelBar locationSearchPanel = (Telerik.Web.UI.RadPanelBar)SearchPanel.Items[0].Items[0].FindControl("locationSearchPanel");

        Panel StateDropDownPanel = (Panel)locationSearchPanel.Items[0].Items[0].FindControl("StateDropDownPanel");
        Panel StateTextBoxPanel = (Panel)locationSearchPanel.Items[0].Items[0].FindControl("StateTextBoxPanel");

        //SearchElements SearchElements2 = (SearchElements)SearchPanel.Items[1].Items[0].FindControl("SearchElements2");
        ASP.controls_hippotextbox_ascx SearchTextBox = (ASP.controls_hippotextbox_ascx)locationSearchPanel.Items[1].FindControl("SearchTextBox");
        //Telerik.Web.UI.RadComboBox DateDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("DateDropDown");
        //Telerik.Web.UI.RadComboBox CountryDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("CountryDropDown");
        Telerik.Web.UI.RadComboBox StateDropDown = (Telerik.Web.UI.RadComboBox)locationSearchPanel.Items[0].Items[0].FindControl("StateDropDown");
        //Telerik.Web.UI.RadComboBox VenueDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("VenueDropDown");
        Telerik.Web.UI.RadTextBox StateTextBox = (Telerik.Web.UI.RadTextBox)locationSearchPanel.Items[0].Items[0].FindControl("StateTextBox");
        Telerik.Web.UI.RadTextBox CityTextBox = (Telerik.Web.UI.RadTextBox)locationSearchPanel.Items[0].Items[0].FindControl("CityTextBox");
        //Telerik.Web.UI.RadComboBox RatingDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("RatingDropDown");
        //Telerik.Web.UI.RadTreeView CategoryTree = (Telerik.Web.UI.RadTreeView)SearchPanel.Items[0].Items[0].FindControl("CategoryTree");
        //Telerik.Web.UI.RadTreeView RadTreeView1 = (Telerik.Web.UI.RadTreeView)SearchPanel.Items[0].Items[0].FindControl("RadTreeView1");
        Telerik.Web.UI.RadTextBox ZipTextBox = (Telerik.Web.UI.RadTextBox)locationSearchPanel.Items[1].Items[0].FindControl("ZipTextBox");
                Panel RadiusTitlePanel = (Panel)locationSearchPanel.Items[1].Items[0].FindControl("RadiusTitlePanel");
        Panel RadiusDropPanel = (Panel)locationSearchPanel.Items[1].Items[0].FindControl("RadiusDropPanel");
        Panel USLabelPanel = (Panel)locationSearchPanel.Items[1].Items[0].FindControl("USLabelPanel");
        Telerik.Web.UI.RadComboBox RadiusDropDown = (Telerik.Web.UI.RadComboBox)locationSearchPanel.Items[1].Items[0].FindControl("RadiusDropDown");
        //Label MessageLabel = (Label)SearchPanel.Items[0].Items[0].FindControl("MessageLabel");
        //MessageLabel.Text = "";

        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

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

        if (locationSearchPanel.Items[1].Expanded && ZipTextBox.Text.Trim() != "")
        {
            isZip = true;
        }
        else
        {
            if (CityTextBox.Text.Trim() == "")
                isZip = true;
        }

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
                                classL = "AddGreenLink";
                            else
                                classL = "AddWhiteLink";
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
                                    dv[i]["ID"].ToString() + ");\" class=\"AddLink\">" +
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
        VenueDiv.InnerHtml = "Select Venue >";
    }

    protected void setTimeSession(object sender, EventArgs e)
    {
        string parameter = Request.Form["__EVENTARGUMENT"];
        Session["SetTime"] = parameter;
        //HtmlContainerControl TimeFrameDiv = (HtmlContainerControl)SearchPanel.Items[0].Items[0].FindControl("TimeFrameDiv");
        TimeFrameDiv.InnerHtml = parameter;
    }
}
