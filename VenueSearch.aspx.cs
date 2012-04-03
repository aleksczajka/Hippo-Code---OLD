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

public partial class VenueSearch : Telerik.Web.UI.RadAjaxPage
{
    protected enum TimeFrame { Beginning, Today, Tomorrow, ThisWeek, ThisWeekend, ThisMonth, NextDays, HighestPrice };

    protected void Page_Load(object sender, EventArgs e)
    {
        Session["Searching"] = "Venues";
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

        DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn);

        Data dat = new Data(isn);

        HtmlHead head = (HtmlHead)Page.Header;

        Literal lit = new Literal();
        lit.Text = "<script src=\"http://maps.google.com/maps?file=api&amp;v=2&amp;sensor=false&amp;key=ABQIAAAAjjoxQtYNtdn3Tc17U5-jbBR2Kk_H7gXZZZniNQ8L14X1BLzkNhQjgZq1k-Pxm8FxVhUy3rfc6L9O4g\" type=\"text/javascript\"></script>";
        head.Controls.Add(lit);

        #region Take Care of Buttons
        SearchButton.BUTTON_TEXT += "<img style=\"border: 0; padding-top: 3px;\" src=\"NewImages/search.png\"/>";
        SearchButton.SERVER_CLICK += SearchButton_Click;
        FilterButton.SERVER_CLICK += FilterResults;
        SmallButton1.BUTTON_TEXT += "<img style=\"border: 0; padding-top: 3px;\" src=\"NewImages/search.png\"/>";
        SmallButton1.SERVER_CLICK += SearchButton_Click;
        #endregion

        if (!IsPostBack)
        {
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
            lk.Href = "http://hippohappenings.com/venue-search";
            head.Controls.AddAt(0, lk);




            //Create keyword and description meta tags
            HtmlMeta hm = new HtmlMeta();
            HtmlMeta kw = new HtmlMeta();

            kw.Name = "keywords";
            kw.Content = "locale venue shop bar restaurant";

            head.Controls.AddAt(0, kw);

            hm.Name = "Description";
            hm.Content = "Find locales, venues, shops, bars and restaurants.";
            head.Controls.AddAt(0, hm);

            lk = new HtmlLink();
            lk.Href = "http://" + Request.Url.Authority + "/venue-search";
            lk.Attributes.Add("rel", "bookmark");
            head.Controls.AddAt(0, lk);

            EventTitle.Text = "<a style=\"text-decoration: none;\"  href=\"" +
                lk.Href + "\"><h1>Search Locales</h1></a>";

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
                    StateChanged(StateDropDown, new EventArgs());

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
                    StateChanged(StateDropDown, new EventArgs());
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
        Search();
    }

    protected void Search()
    {
        try
        {
            HttpCookie cookie = Request.Cookies["BrowserDate"];

            DateTime isn = DateTime.Now;

            if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
                isn = DateTime.Now;
            DateTime isNow = isn;
            Data dat = new Data(isn); 
            string featureDate = isNow.Month.ToString() + "/" + isNow.Day.ToString() + "/" + isNow.Year.ToString();

            string resultsStr = "";

            char[] delim = { ' ' };
            string[] tokens;

            KeywordsTextBox.Text = dat.stripHTML(KeywordsTextBox.Text);
            CityTextBox.Text = dat.stripHTML(CityTextBox.Text.Trim());
            StateTextBox.Text = dat.stripHTML(StateTextBox.Text.Trim());
            string message = "";
            bool goOn = true;
            string temp = "";
            string featureSearchTerms = "";
            string allSearchTerms = "";
            string featureString = "";

            string includeSearchTerms = "";
            if (KeywordsTextBox.Text.Trim() != "")
            {
                tokens = KeywordsTextBox.Text.Trim().Split(delim);
                resultsStr += " for '" + KeywordsTextBox.Text.Trim() + "'";
                for (int i = 0; i < tokens.Length; i++)
                {
                    featureSearchTerms += " EST.SearchTerms LIKE '%;" + tokens[i].Replace("'", "''") + ";%' ";

                    temp += " V.Name LIKE @search" + i.ToString();

                    if (i + 1 != tokens.Length)
                    {
                        featureSearchTerms += " AND ";
                        temp += " AND ";
                    }
                }
            }

            if (temp != "")
            {
                allSearchTerms = " AND ((" + temp + ") OR (V.ID=EST.VenueID AND SearchDate = '" + featureDate +
                    "' AND " + featureSearchTerms + "))";

                includeSearchTerms = ", VenueSearchTerms EST ";
            }

            

            string country = " AND V.Country=" + CountryDropDown.SelectedValue;

            string state = "";
            string stateParam = "";
            string city = "";
            string cityParam = "";

            string zip = "";
            string zipParameter = "";
            int zipParam = 0;
            string nonExistantZip = "";

            ZipTextBox.Text = dat.stripHTML(ZipTextBox.Text.Trim());
            CityTextBox.Text = dat.stripHTML(CityTextBox.Text.Trim());
            StateTextBox.Text = dat.stripHTML(StateTextBox.Text.Trim());

            bool isZip = false;

            if (ZipTextBox.Text.Trim() != "")
                isZip = true;

            if (isZip)
            {
                if (ZipTextBox.Text.Trim() != "")
                {
                    resultsStr += " in " + ZipTextBox.Text.Trim() + " ";
                    //do only if United States and not for international zip codes
                    if (RadiusDropPanel.Visible)
                    {
                        //Get all zips within the specified radius
                        resultsStr += " within " + RadiusDropDown.SelectedItem.Text;
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
                            message = "You must at the very least include a zip code or State and City.";
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
            else
            {
                if (CityTextBox.Text != "")
                {
                    city = " AND V.City LIKE @city ";
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
                        state = " AND V.State=@state ";
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

                
            }

            string resultsCats = "";
            if (goOn)
            {
                //SearchPanel.Items[0].Expanded = false;
                //SearchPanel.Items[1].Expanded = true;

                string rating = "";
                string addRating = "";
                //if (RatingDropDown.SelectedValue != "-1" && RatingDropDown.SelectedValue != "0")
                //{
                //    rating = " AND V.Rating >= " + RatingDropDown.SelectedItem.Text;
                //    resultsStr += ", lowest rating: " + RatingDropDown.SelectedItem.Text;
                //}

                string addCat = "";

                #region Do Categories
                string categories = "";
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
                                    childCategories += " OR VC.CATEGORY_ID = " + CategoryTree.Nodes[i].Nodes[h].Value;

                                    if (CategoryTree.Nodes[i].Nodes[h].Nodes.Count > 0)
                                    {
                                        for (int k = 0; k < CategoryTree.Nodes[i].Nodes[h].Nodes.Count; k++)
                                        {
                                            childCategories += " OR VC.CATEGORY_ID = " + CategoryTree.Nodes[i].Nodes[h].Nodes[k].Value;
                                        }
                                    }
                                }
                            }

                            categories += " AND ( V.ID IN (SELECT VC.VENUE_ID FROM Venue_Category VC " +
                                "WHERE VC.CATEGORY_ID = " + CategoryTree.Nodes[i].Value + childCategories + " )) ";


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
                            categories += " AND ( V.ID IN (SELECT VC.VENUE_ID FROM Venue_Category VC " +
                                "WHERE VC.CATEGORY_ID = " + CategoryTree.Nodes[i].Nodes[n].Value + " )) ";


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
                                 childCategories += " OR VC.CATEGORY_ID = " + RadTreeView1.Nodes[i].Nodes[h].Value;

                                 if (RadTreeView1.Nodes[i].Nodes[h].Nodes.Count > 0)
                                 {
                                     for (int k = 0; k < RadTreeView1.Nodes[i].Nodes[h].Nodes.Count; k++)
                                     {
                                         childCategories += " OR VC.CATEGORY_ID = " + RadTreeView1.Nodes[i].Nodes[h].Nodes[k].Value;
                                     }
                                 }
                             }
                         }

                         categories += " AND ( V.ID IN (SELECT VC.VENUE_ID FROM Venue_Category VC " +
                             "WHERE VC.CATEGORY_ID = " + RadTreeView1.Nodes[i].Value + childCategories + " )) ";


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
                             categories += " AND ( V.ID IN (SELECT VC.VENUE_ID FROM Venue_Category VC " +
                                 "WHERE VC.CATEGORY_ID = " + RadTreeView1.Nodes[i].Nodes[n].Value + " )) ";


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
                                 childCategories += " OR VC.CATEGORY_ID = " + RadTreeView2.Nodes[i].Nodes[h].Value;

                                 if (RadTreeView2.Nodes[i].Nodes[h].Nodes.Count > 0)
                                 {
                                     for (int k = 0; k < RadTreeView2.Nodes[i].Nodes[h].Nodes.Count; k++)
                                     {
                                         childCategories += " OR VC.CATEGORY_ID = " + RadTreeView2.Nodes[i].Nodes[h].Nodes[k].Value;
                                     }
                                 }
                             }
                         }

                         categories += " AND ( V.ID IN (SELECT VC.VENUE_ID FROM Venue_Category VC " +
                             "WHERE VC.CATEGORY_ID = " + RadTreeView2.Nodes[i].Value + childCategories + " )) ";


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
                             categories += " AND ( V.ID IN (SELECT VC.VENUE_ID FROM Venue_Category VC " +
                                 "WHERE VC.CATEGORY_ID = " + RadTreeView2.Nodes[i].Nodes[n].Value + " )) ";


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

                #endregion

                resultsStr += resultsCats;

                //if (categories != "")
                //    categories += " ) ";

                

                string searchStr = "SELECT DISTINCT V.Featured, V.DaysFeatured, V.ID AS VID, V.Name, " +
                    "V.City, V.State, V.Address FROM Venues V " + addCat + includeSearchTerms +
                    " WHERE V.Live='True' " + country + state + city + zip + allSearchTerms + categories + " ORDER BY V.Name ";
                //ErrorLabel.Text = searchStr;
                
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                SqlCommand cmd = new SqlCommand(searchStr, conn);
                if (KeywordsTextBox.Text.Trim() != "")
                {
                    tokens = KeywordsTextBox.Text.Trim().Split(delim);
                    for (int i = 0; i < tokens.Length; i++)
                    {
                        cmd.Parameters.Add("@search" + i.ToString(), SqlDbType.NVarChar).Value = "%" + tokens[i] + "%";
                    }
                }
                if (!RadiusDropPanel.Visible)
                {
                    cmd.Parameters.Add("@zip", SqlDbType.NVarChar).Value = zipParameter;
                }
                if (state != "")
                    cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = stateParam;
                if (city != "")
                    cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = "%" + cityParam + "%";


                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                conn.Close();

                //ErrorLabel.Text = searchStr;
                Session["SearchDS"] = ds;
                Session["Searching"] = "Venues";
                bool setZero = false;

                if (ds.Tables.Count > 0)
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //SearchElements2.VENUE_DS = ds;
                        //SearchElements2.DataBind2();

                        //int eventCount = ds.Tables[0].Rows.Count;

                        //if (dat.SearchCutOffNumber < eventCount)
                        //{
                        //    SearchPanel.Items[1].Text = "<div style='position: relative;'><span style='font-family: Arial; font-size: 20px; color: White;'>Search Results: </span><div style='position: absolute; right: 5px; top: 3px;'>(Search cut off at " + dat.SearchCutOffNumber.ToString() + ")</div></div>";

                        //}
                        //else
                        //{

                        //    SearchPanel.Items[1].Text = "<div style='position: relative;'><span style='font-family: Arial; font-size: 20px; color: White;'>Search Results: </span><div style='position: absolute; right: 5px; top: 3px;'>(" + ds.Tables[0].Rows.Count + " Records Found)</div></div>";
                        //}


                    }
                    else
                    {
                        setZero = true;
                    }
                else
                    setZero = true;

                Session["SearchResults"] = "Results " + resultsStr;

                ResultsLabel.Text = "Search results " + resultsStr;

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
                    NumsLabel.Text = ds.Tables[0].Rows.Count.ToString() + " Results Found";
                    NoResultsPanel.Visible = false;
                    Session["SearchDS"] = ds;
                    FillResults(ds);
                    MapPanel.Visible = true;
                    SortDropDown.Visible = true;
                }

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
            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text += ex.ToString();
        }
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
        DataColumn dc2 = new DataColumn("SearchNum");
        if (!ds.Tables[0].Columns.Contains("SearchNum"))
            ds.Tables[0].Columns.Add(dc2);

        int numofpages2 = 10;

        string sortString = "";
        if (Session["venueSortString"] != null)
        {
            sortString = Session["venueSortString"].ToString();
        }

        DataView dv = TakeCareOfFeaturedVenues(ds, sortString);

        string[] mapStrings2 = GetVenuesLiteral(dv, dat, numofpages2);

        VenueSearchElements.DO_MAP = true;
        VenueSearchElements.Visible = true;
        VenueSearchElements.VENUE_DS = ds;
        VenueSearchElements.MAP_STRINGS = mapStrings2;
        VenueSearchElements.NUM_OF_PAGES = numofpages2;
        VenueSearchElements.IS_WINDOW = true;
        VenueSearchElements.DataBind2();
    }

    /// <summary>
    /// TakeCareOfFeaturedEvents() functions make sure that featured events always come up on top.
    /// Up to 4 featured events are on top. So, first page will always contain the first four. 
    /// Next page the second four featured events, etc. 
    /// The order of the featured events cannot be biased. It has to be random. 
    /// On each run of this function, we assign random order numbers for the featured events.
    /// </summary>
    /// <param name="ds">The DataSet to order with featured events on top.</param>
    protected DataView TakeCareOfFeaturedVenues(DataSet ds, string sortString)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn);
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
            FeaturedMapping[fCount++] = (int)row["VID"];
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
                dv.RowFilter = "VID = " + FeaturedMapping[i];
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


        string theLiteral = "<script type=\"text/javascript\">function initialize" +
                (countInArray + 1).ToString() + "(){initMap(); " +
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
            DataView dvV = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + row["VID"].ToString());

            DataView dvCountry = dat.GetDataDV("SELECT * FROM countries WHERE country_id=" +
                dvV[0]["Country"].ToString());
            country = dvCountry[0]["country_2_code"].ToString();

            if (country.ToLower() == "us")
            {
                try
                {
                    
                    address = dat.GetAddress(dvV[0]["Address"].ToString(), false) + " " +
                        dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                            dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                            country.Replace("'", " ").Replace("(", " ").Replace(")", " ");
                    //ErrorLabel.Text += "<br/>" + address;
                }
                catch (Exception ex1)
                {
                    address = dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                            dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                            country.Replace("'", " ").Replace("(", " ").Replace(")", " ");
                }
            }
            else
            {
                address = dat.GetAddress(dvV[0]["Address"].ToString(), true) + " " +
                        dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                            dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                            country.Replace("'", " ").Replace("(", " ").Replace(")", " ");
                //ErrorLabel.Text += "<br/>" + address;
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
                    venueMapping[address] += ",<br/><div class=\\\"Green12LinkNFUD\\\" " +
                        "onclick=\\\"GoToThis(\\'" + "../" +
                        dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                        dvV[0]["ID"].ToString() + "_Venue" + "\\');\\\">" +
                        dvV[0]["Name"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + "</div>";
                }
                else
                {
                    if (!isFirstElement)
                    {
                        i++;
                        venuesNumMapping.Add(address, dat.GetImage(i.ToString()));
                        venueMapping.Add(address, "address =  '" + address.Replace("'", "''").Replace("(", " ").Replace(")", " ")+ 
                            "'; " +
                            "showAddressUS('',0, address, 0, address, 1, " + i.ToString() + ", '" +
                        "<div class=\\\"Green12LinkNFUD\\\" " +
                        "onclick=\\\"GoToThis(\\'" + "../" +
                        dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                        dvV[0]["ID"].ToString() + "_Venue" + "\\');\\\">" +
                        dvV[0]["Name"].ToString().Replace("'", " ").Replace("(",
                        " ").Replace(")", " ") + "</div> ");
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
                        venueMapping.Add(address, "address =  '" + address.Replace("'", "''").Replace("(",
                            " ").Replace(")", " ") + "'; " +
                            " showAddressUS('',0, address, 0, address, 1, " + i.ToString() + ", '" +
                        "<div class=\\\"Green12LinkNFUD\\\" " +
                        "onclick=\\\"GoToThis(\\'" + "../" +
                        dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                        dvV[0]["ID"].ToString() + "_Venue" + "\\');\\\">" +
                        dvV[0]["Name"].ToString().Replace("'", " ").Replace("(",
                        " ").Replace(")", " ") + "</div> ");
                    }
                }
            }
            else
            {
                if (venueMapping.ContainsKey(address))
                {
                    row["SearchNum"] = venuesNumMapping[address].ToString();
                    venueMapping[address] += ",<br/><div class=\\\"Green12LinkNFUD\\\" " +
                        "onclick=\\\"GoToThis(\\'" + "../" +
                        dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                        dvV[0]["ID"].ToString() + "_Venue" + "\\');\\\">" +
                        dvV[0]["Name"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + "</div>";
                }
                else
                {
                    if (!isFirstElement)
                    {
                        i++;
                        venuesNumMapping.Add(address, dat.GetImage(i.ToString()));
                        venueMapping.Add(address, "address =  '" + address + "'; " +
                            "showAddressUS('',0, address, 0, address, 1, " + i.ToString() + ", '" +
                            "<div class=\\\"Green12LinkNFUD\\\" onclick=\\\"GoToThis(\\'" + "../" +
                            dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                            dvV[0]["ID"].ToString() + "_Venue" + "\\');\\\">" +
                            dvV[0]["Name"].ToString().Replace("'", " ").Replace("(",
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
                        venueMapping.Add(address, "address =  '" + address + "'; " +
                            "showAddressUS('',0, address, 0, address, 1, " + i.ToString() + ", '" +
                            "<div class=\\\"Green12LinkNFUD\\\" onclick=\\\"GoToThis(\\'" + "../" +
                            dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                            dvV[0]["ID"].ToString() + "_Venue" + "\\');\\\">" +
                            dvV[0]["Name"].ToString().Replace("'", " ").Replace("(",
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

    protected void SortResults(object sender, EventArgs e)
    {
        if (SortDropDown.SelectedItem.Value != "-1")
            Session["venueSortString"] = SortDropDown.SelectedItem.Value;
        else
            Session["venueSortString"] = null;

        FillResults((DataSet)Session["SearchDS"]);
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

        }
        else
        {
            MessageLabel.Text = "You must at the very least include a zip code or State and City.";
            //SearchPanel.Items[1].Expanded = false;
            //SearchPanel.Items[0].Expanded = true;
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
