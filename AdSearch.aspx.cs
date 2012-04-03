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
using System.Collections.Generic;

public partial class AdSearch : Telerik.Web.UI.RadAjaxPage
{
    protected enum TimeFrame { Beginning, Today, Tomorrow, ThisWeek, ThisWeekend, ThisMonth, NextDays, HighestPrice };

    protected void Page_Load(object sender, EventArgs e)
    {
        Session["Searching"] = "Ads";
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

        #region Take Care of Buttons
        SearchButton.BUTTON_TEXT += "<img style=\"border: 0; padding-top: 3px;\" src=\"NewImages/search.png\"/>";
        SearchButton.SERVER_CLICK += SearchButton_Click;
        FilterButton.SERVER_CLICK += FilterResults;
        SmallButton1.BUTTON_TEXT += "<img style=\"border: 0; padding-top: 3px;\" src=\"NewImages/search.png\"/>";
        SmallButton1.SERVER_CLICK += SearchButton_Click;
        SaveSearchButton.SERVER_CLICK += SaveSearch;
        #endregion

        if (!IsPostBack)
        {
            Session["SearchDS"] = null;
            Session.Remove("SearchDS");

            NumsLabel.Text = "";
            NoResultsPanel.Visible = true;

            TopLiteral.Text = "";
            BottomScriptLiteral.Text = "";
            ScriptLiteral.Text = "";

            HtmlLink lk = new HtmlLink();
            lk.Attributes.Add("rel", "canonical");
            lk.Href = "http://hippohappenings.com/ad-search";
            head.Controls.AddAt(0, lk);

            //Create keyword and description meta tags
            HtmlMeta hm = new HtmlMeta();
            HtmlMeta kw = new HtmlMeta();

            kw.Name = "keywords";
            kw.Content = "local bulletin ad classified";

            head.Controls.AddAt(0, kw);

            hm.Name = "Description";
            hm.Content = "Find local bulletins, ads and classifieds.";
            head.Controls.AddAt(0, hm);

            lk = new HtmlLink();
            lk.Href = "http://" + Request.Url.Authority + "/ad-search";
            lk.Attributes.Add("rel", "bookmark");
            head.Controls.AddAt(0, lk);

            EventTitle.Text = "<a style=\"text-decoration: none;\"  href=\"" +
                lk.Href + "\"><h1>Search Bulletins</h1></a>";

            //Button button = (Button)dat.FindControlRecursive(this, "EventLink");
            //button.CssClass = "NavBarImageEventSelected";
        }

        Session["s"] = "asfsd";
        if (!IsPostBack)
        {
            Session["SavedClicked"] = null;
            Session.Remove("SavedClicked");
        }

        Session["RedirectTo"] = "ad-search";

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

            StateDropDownPanel.Visible = true;
            StateTextBoxPanel.Visible = false;

            ChangeCity(StateDropDown, new EventArgs());

            try
            {
                if (Session["User"] != null)
                {
                    DataView dvUser = dat.GetDataDV("SELECT CatCity, CatState, CatCountry, CatZip, MajorCity FROM  UserPreferences WHERE UserID=" +
                        Session["User"].ToString());
                    CountryDropDown.ClearSelection();
                    CountryDropDown.SelectedValue = dvUser[0]["CatCountry"].ToString();
                    ChangeState(CountryDropDown, new EventArgs());
                    if (StateTextBoxPanel.Visible)
                        StateTextBox.Text = dvUser[0]["CatState"].ToString();
                    else
                    {
                        StateDropDown.ClearSelection();
                        StateDropDown.Items.FindItemByText(dvUser[0]["CatState"].ToString()).Selected = true;
                        ChangeCity(StateDropDown, new EventArgs());
                    }

                    if (dvUser[0]["CatCountry"].ToString() == "223")
                    {
                        if (dvUser[0]["CatCity"] != null)
                        {
                            MajorCityDrop.ClearSelection();
                            MajorCityDrop.Items.FindByValue(dvUser[0]["MajorCity"].ToString()).Selected = true;
                        }
                    }
                    else
                    {
                        if (dvUser[0]["CatCity"] != null)
                        {
                            CityTextBox.Text = dvUser[0]["CatCity"].ToString();
                        }
                    }

                    //if (dvUser[0]["CatZip"] != null)
                    //{
                    //    if (dvUser[0]["CatZip"].ToString().Trim() != "")
                    //    {

                    //        char[] delim = { ';' };
                    //        string[] tokens = dvUser[0]["CatZip"].ToString().Split(delim);

                    //        if (tokens.Length > 1)
                    //        {
                    //            ZipTextBox.Text = tokens[1].Trim();

                    //            if (dvUser[0]["Radius"] != null)
                    //            {
                    //                if (dvUser[0]["Radius"].ToString().Trim() != "")
                    //                {

                    //                    RadiusDropDown.SelectedValue = dvUser[0]["Radius"].ToString();
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
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
                    {
                        StateDropDown.ClearSelection();
                        StateDropDown.Items.FindItemByText(Session["LocState"].ToString().Trim()).Selected = true;
                    }

                    ChangeCity(StateDropDown, new EventArgs());
                }

                
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.ToString();
            }
        }

        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["search"] != null)
                {
                    FillSavedSearch();
                }
                else
                {
                    Search();
                }
            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString();
        }

        MessageRadWindowManager.VisibleOnPageLoad = false;
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

            string featureString = "";
            string includeSearchTerms = "";
            string message = "";
            bool goOn = true;
            string temp = "";
            string featureSearchTerms = "";
            string allSearchTerms = "";
            if (KeywordsTextBox.Text.Trim() != "")
            {
                tokens = KeywordsTextBox.Text.Trim().Split(delim);
                resultsStr += " for '" + KeywordsTextBox.Text.Trim() + "'";
                for (int i = 0; i < tokens.Length; i++)
                {
                    featureSearchTerms += " EST.SearchTerms LIKE '%;" + tokens[i].Replace("'", "''") + ";%' ";

                    temp += " E.Header LIKE @search" + i.ToString();

                    if (i + 1 != tokens.Length)
                    {
                        featureSearchTerms += " AND ";
                        temp += " AND ";
                    }
                }
            }

            if (temp != "")
            {
                allSearchTerms = " AND ((" + temp + ") OR (E.Ad_ID=EST.AdID AND SearchDate = '" + featureDate +
                    "' AND " + featureSearchTerms + "))";

                includeSearchTerms = ", AdSearchTerms EST ";

            }


            //if (featureSearchTerms != "")
            //    featureSearchTerms = " AND (" + featureSearchTerms + ") ";

            string country = " AND E.CatCountry=" + CountryDropDown.SelectedValue;

            string state = "";
            string stateParam = "";
            string city = "";
            string cityParam = "";

            string zip = "";
            string zipParameter = "";
            int zipParam = 0;
            string nonExistantZip = "";

            //ZipTextBox.Text = dat.stripHTML(ZipTextBox.Text.Trim());
            CityTextBox.Text = dat.stripHTML(CityTextBox.Text.Trim());
            StateTextBox.Text = dat.stripHTML(StateTextBox.Text.Trim());

            bool isZip = true;

            //if (ZipTextBox.Text.Trim() != "")
            //    isZip = true;

            //if (isZip)
            //{
            //    if (ZipTextBox.Text.Trim() != "")
            //    {
            //        resultsStr += " in " + ZipTextBox.Text.Trim() + " ";
            //        //do only if United States and not for international zip codes
            //        if (RadiusDropPanel.Visible)
            //        {
            //            //Get all zips within the specified radius
            //            resultsStr += " within " + RadiusDropDown.SelectedItem.Text;
            //            if (int.TryParse(ZipTextBox.Text.Trim(), out zipParam))
            //            {
            //                DataSet dsLatsLongs = dat.GetData("SELECT Latitude, Longitude FROM ZipCodes WHERE Zip='" +
            //                    zipParam + "'");

            //                //some zip codes don't exist in the database, find the closest one
            //                bool findClosest = false;
            //                if (dsLatsLongs.Tables.Count > 0)
            //                {
            //                    if (dsLatsLongs.Tables[0].Rows.Count > 0)
            //                    {

            //                    }
            //                    else
            //                    {
            //                        findClosest = true;
            //                    }
            //                }
            //                else
            //                {
            //                    findClosest = true;
            //                }

            //                if (findClosest)
            //                {
            //                    dsLatsLongs = null;
            //                    zip = " AND E.Cat.Zip = '" + zipParam.ToString() + "' ";
            //                    nonExistantZip = " OR E.CatZip = '" + zipParam.ToString() + "'";
            //                    while (dsLatsLongs == null)
            //                    {
            //                        dsLatsLongs = dat.GetData("SELECT Latitude, Longitude FROM ZipCodes WHERE Zip='" + zipParam++ + "'");
            //                        if (dsLatsLongs.Tables.Count > 0)
            //                        {
            //                            if (dsLatsLongs.Tables[0].Rows.Count > 0)
            //                            {

            //                            }
            //                            else
            //                            {
            //                                dsLatsLongs = null;
            //                            }
            //                        }
            //                        else
            //                        {
            //                            dsLatsLongs = null;
            //                        }
            //                    }
            //                }

            //                //get all the zip codes within the specified radius
            //                DataSet dsZips = dat.GetData("SELECT Zip FROM ZipCodes WHERE dbo.GetDistance(" + dsLatsLongs.Tables[0].Rows[0]["Longitude"].ToString() +
            //                    ", " + dsLatsLongs.Tables[0].Rows[0]["Latitude"].ToString() + ", Longitude, Latitude) < " + RadiusDropDown.SelectedValue);

            //                if (dsZips.Tables.Count > 0)
            //                {
            //                    if (dsZips.Tables[0].Rows.Count > 0)
            //                    {
            //                        zip = " AND (E.CatZip = '" + zipParam.ToString() + "' " + nonExistantZip;
            //                        for (int i = 0; i < dsZips.Tables[0].Rows.Count; i++)
            //                        {
            //                            zip += " OR E.CatZip = '" + dsZips.Tables[0].Rows[i]["Zip"].ToString() + "' ";
            //                        }
            //                        zip += ") ";
            //                    }
            //                    else
            //                    {
            //                        zip = " AND E.CatZip='" + ZipTextBox.Text.Trim() + "'";
            //                    }
            //                }
            //                else
            //                {
            //                    zip = " AND E.CatZip='" + ZipTextBox.Text.Trim() + "'";
            //                }


            //            }
            //            else
            //            {
            //                goOn = false;
            //                message = "You must at the very least include a zip code or State and City.";
            //            }
            //        }
            //        else
            //        {
            //            zip = " AND E.CatZip = @zip ";
            //            zipParameter = ZipTextBox.Text.Trim();
            //        }
            //    }
            //    else
            //    {
            //        goOn = false;
            //        message = "You must at the very least include a zip code or State and City.";
            //    }
            //}
            //else
            //{

            if (CountryDropDown.SelectedValue == "223")
            {
                city = " AND E.CatCity LIKE @city ";
                cityParam = MajorCityDrop.SelectedItem.Text.ToLower();
                resultsStr += " in " + MajorCityDrop.SelectedItem.Text.ToLower();
            }
            else
            {
                if (CityTextBox.Text != "")
                {
                    city = " AND E.CatCity LIKE @city ";
                    cityParam = CityTextBox.Text.ToLower();
                    resultsStr += " in " + CityTextBox.Text.ToLower();
                }
                else
                {
                    goOn = false;
                    message = "You must include the state and major city.";
                }
            }
            if (StateDropDownPanel.Visible)
            {
                if (StateDropDown.SelectedValue != "-1")
                {
                    state = " AND E.CatState=@state ";
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
                    state = " AND E.CatState=@state ";
                    stateParam = StateTextBox.Text;
                    resultsStr += ", " + StateTextBox.Text;
                }
                else
                {
                    goOn = false;
                    message = "You must at the very least include a zip code or State and City.";
                }
            }

            
            //}

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
                                childCategories += " OR VC.CategoryID = " + CategoryTree.Nodes[i].Nodes[h].Value;

                                if (CategoryTree.Nodes[i].Nodes[h].Nodes.Count > 0)
                                {
                                    for (int k = 0; k < CategoryTree.Nodes[i].Nodes[h].Nodes.Count; k++)
                                    {
                                        childCategories += " OR VC.CategoryID = " + CategoryTree.Nodes[i].Nodes[h].Nodes[k].Value;
                                    }
                                }
                            }
                        }

                        categories += " AND ( E.Ad_ID IN (SELECT VC.AdID FROM Ad_Category_Mapping VC " +
                            "WHERE VC.CategoryID = " + CategoryTree.Nodes[i].Value + childCategories + " )) ";


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
                            categories += " AND ( E.Ad_ID IN (SELECT VC.AdID FROM Ad_Category_Mapping VC " +
                            "WHERE VC.CategoryID = " + CategoryTree.Nodes[i].Nodes[n].Value + " )) ";


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
                                childCategories += " OR VC.CategoryID = " + RadTreeView1.Nodes[i].Nodes[h].Value;

                                if (RadTreeView1.Nodes[i].Nodes[h].Nodes.Count > 0)
                                {
                                    for (int k = 0; k < RadTreeView1.Nodes[i].Nodes[h].Nodes.Count; k++)
                                    {
                                        childCategories += " OR VC.CategoryID = " + RadTreeView1.Nodes[i].Nodes[h].Nodes[k].Value;
                                    }
                                }
                            }
                        }

                        categories += " AND ( E.Ad_ID IN (SELECT VC.AdID FROM Ad_Category_Mapping VC " +
                            "WHERE VC.CategoryID = " + RadTreeView1.Nodes[i].Value + childCategories + " )) ";


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
                            categories += " AND ( E.Ad_ID IN (SELECT VC.AdID FROM Ad_Category_Mapping VC " +
                            "WHERE VC.CategoryID = " + RadTreeView1.Nodes[i].Nodes[n].Value + " )) ";


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
                                childCategories += " OR VC.CategoryID = " + RadTreeView2.Nodes[i].Nodes[h].Value;

                                if (RadTreeView2.Nodes[i].Nodes[h].Nodes.Count > 0)
                                {
                                    for (int k = 0; k < RadTreeView2.Nodes[i].Nodes[h].Nodes.Count; k++)
                                    {
                                        childCategories += " OR VC.CategoryID = " + RadTreeView2.Nodes[i].Nodes[h].Nodes[k].Value;
                                    }
                                }
                            }
                        }

                        categories += " AND ( E.Ad_ID IN (SELECT VC.AdID FROM Ad_Category_Mapping VC " +
                            "WHERE VC.CategoryID = " + RadTreeView2.Nodes[i].Value + childCategories + " )) ";


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
                            categories += " AND ( E.Ad_ID IN (SELECT VC.AdID FROM Ad_Category_Mapping VC " +
                            "WHERE VC.CategoryID = " + RadTreeView2.Nodes[i].Nodes[n].Value + " )) ";


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

                //string catHiTemp = categories + temp;




                //if (catHiTemp.Trim() != "")
                //{
                //    if (featureSearchTerms != "")
                //    {
                //        if (catHiTemp.Trim().Substring(0, 3).ToLower() == "and")
                //        {
                //            catHiTemp = catHiTemp.Trim().Substring(3, catHiTemp.Trim().Length - 3);
                //        }

                //        if (catHiTemp.Trim().Substring(0, 3).ToLower() == "and")
                //            catHiTemp = catHiTemp.Trim().Substring(3, catHiTemp.Trim().Length - 3);
                //        featureString = "AND (( " + catHiTemp + " ) OR " +
                //        "(E.Ad_ID=EST.AdID AND SearchDate = '" + featureDate + "' " + featureSearchTerms +
                //        "))";
                //        includeSearchTerms = ", AdSearchTerms EST";
                //    }
                //    else
                //    {
                //        featureString = catHiTemp;
                //    }
                //}

                string dateOfAds = DateTime.Now.AddDays(-30).ToShortDateString();

                string searchStr = "SELECT DISTINCT TOP 200 E.Featured, E.Description, E.DatesOfAd, E.Ad_ID AS VID, E.Header " +
                    " FROM Ads E " + includeSearchTerms +
                    " WHERE E.DateAdded >= CONVERT(DATETIME, '" + dateOfAds +
                    "') AND E.Live='True' " + country + state + city + allSearchTerms + categories + " ORDER BY E.Header ";
                
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
                //if (!RadiusDropPanel.Visible)
                //{
                //    cmd.Parameters.Add("@zip", SqlDbType.NVarChar).Value = zipParameter;
                //}
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
                Session["Searching"] = "Ads";
                bool setZero = false;

                if (ds.Tables.Count > 0)
                    if (ds.Tables[0].Rows.Count > 0)
                    {
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
                    NumsLabel.Text = "0 Results Found";
                    Session["SearchDS"] = null;
                    NoResultsPanel.Visible = true;
                    SortDropDown.Visible = false;
                }
                else
                {
                    NumsLabel.Text = ds.Tables[0].Rows.Count.ToString() + " Results Found";
                    NoResultsPanel.Visible = false;
                    Session["SearchDS"] = ds;
                    FillResults(ds);
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
            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text += ex.ToString();
        }
    }

    protected void FillResults(DataSet ds)
    {
        TopLiteral.Text = "";
        BottomScriptLiteral.Text = "";
        ScriptLiteral.Text = "";

        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn); DataColumn dc2 = new DataColumn("SearchNum");
        if (!ds.Tables[0].Columns.Contains("SearchNum"))
            ds.Tables[0].Columns.Add(dc2);

        int numofpages2 = 10;

        string sortString = "";
        if (Session["adSortString"] != null)
        {
            sortString = Session["adSortString"].ToString();
        }
        
        DataView dv = TakeCareOfFeaturedVenues(ds, sortString);

        VenueSearchElements.Visible = true;
        VenueSearchElements.AD_DS = ds;
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
        DateTime isNow = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
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

        dv.RowFilter = "Featured = 'True' AND DatesOfAd LIKE '%;" + featureDate + ";%'";
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
                    dv.RowFilter = "VID = " + FeaturedMapping[i];
                    dv[0]["colOrder"] = count;
                    count++;
                }
                featuredTakenCareOf += toFeature;
            }
            dv.RowFilter = "NOT(Featured = 'True' AND DatesOfAd LIKE '%;" + featureDate + ";%')  OR DatesOfAd IS NULL";

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

    protected void SortResults(object sender, EventArgs e)
    {
        if (SortDropDown.SelectedItem.Value != "-1")
            Session["adSortString"] = SortDropDown.SelectedItem.Value;
        else
            Session["adSortString"] = null;

        FillResults((DataSet)Session["SearchDS"]);
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

        if (CountryDropDown.SelectedValue == "223")
        {
            CityDropDownPanel.Visible = true;
            StateDropDown.AutoPostBack = true;
            CityTextBoxPanel.Visible = false;
            ChangeCity(StateDropDown, new EventArgs());
        }
        else
        {
            CityDropDownPanel.Visible = false;
            CityTextBoxPanel.Visible = true;
            StateDropDown.AutoPostBack = false;
        }
    }

    protected void ChangeCity(object sender, EventArgs e)
    {
        try
        {
            if (CountryDropDown.SelectedValue == "223")
            {
                HttpCookie cookie = Request.Cookies["BrowserDate"];

                DateTime isn = DateTime.Now;

                if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
                    isn = DateTime.Now;
                DateTime isNow = isn;
                Data dat = new Data(isn);                
                DataSet ds = dat.GetData("SELECT *, MC.ID AS MCID FROM MajorCities MC, State S WHERE MC.State=S.state_name " +
                    "AND S.state_2_code='" + StateDropDown.SelectedItem.Text + "'");

                CityTextBoxPanel.Visible = false;
                CityDropDownPanel.Visible = true;
                MajorCityDrop.DataSource = ds;
                MajorCityDrop.DataTextField = "MajorCity";
                MajorCityDrop.DataValueField = "MCID";
                MajorCityDrop.DataBind();
            }
            else
            {
                CityDropDownPanel.Visible = false;
                CityTextBox.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString();
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
    
    protected void SaveSearch(object sender, EventArgs e)
    {
        string message = "";
        Encryption encrypt = new Encryption();
        if (Session["SavedClicked"] == null)
        {
            HttpCookie cookie = Request.Cookies["BrowserDate"];

            DateTime isn = DateTime.Now;

            if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
                isn = DateTime.Now;
            DateTime isNow = isn;
            Data dat = new Data(isn);
            SqlConnection conn = dat.GET_CONNECTED;
            if (Session["User"] == null)
            {
                Session["RedirectTo"] = "ad-search";

                MessageRadWindow.Height = 200;

                Session["Message"] = "You must be logged in to save your search. We'll take you to the login screen.<br/><br/><table><tr><td align=\"right\"><div style=\"padding-top: 20px;\" align=\"center\">" +
                    "<div style=\"width: 50px;\" onclick=\"Search('login')\">" +
                    "<div class=\"topDiv\" style=\"clear: both;\">" +
                      "  <img style=\"float: left;\" src=\"NewImages/ButtonLeft.png\" height=\"27px\" /> " +
                       " <div class=\"NavyLink\" style=\"font-size: 12px; text-decoration: none; padding-top: 5px;padding-left: 6px; padding-right: 6px;height: 27px;float: left;background: url('NewImages/ButtonPixel.png'); background-repeat: repeat-x;\">" +
                           " OK " +
                        "</div>" +
                       " <img style=\"float: left;\" src=\"NewImages/ButtonRight.png\" height=\"27px\" /> " +
                    "</div>" +
                    "</div>" +
                    "</div></td><td align=\"left\"><div style=\"padding-top: 20px;\" align=\"center\">" +
                    "<div style=\"width: 70px;\" onclick=\"Search()\">" +
                    "<div class=\"topDiv\" style=\"clear: both;\">" +
                      "  <img style=\"float: left;\" src=\"NewImages/ButtonLeft.png\" height=\"27px\" /> " +
                       " <div class=\"NavyLink\" style=\"font-size: 12px; text-decoration: none; padding-top: 5px;padding-left: 6px; padding-right: 6px;height: 27px;float: left;background: url('NewImages/ButtonPixel.png'); background-repeat: repeat-x;\">" +
                           " Cancel " +
                        "</div>" +
                       " <img style=\"float: left;\" src=\"NewImages/ButtonRight.png\" height=\"27px\" /> " +
                    "</div>" +
                    "</div>" +
                    "</div></td></tr></table>";

                MessageRadWindow.NavigateUrl = "Message.aspx";
                MessageRadWindow.Visible = true;
                MessageRadWindowManager.VisibleOnPageLoad = true;
                //MessageLabel.Text = message;
                //SearchPanel.Items[1].Expanded = false;
                //SearchPanel.Items[0].Expanded = true;
            }
            else
            {
                string country = " AND A.CatCountry=" + CountryDropDown.SelectedValue;

                string city = "";
                string cityParam = "";
                string zip = "";
                string zipParameter = "";
                string state = "";
                string stateParam = "";
                int zipParam = 0;

                //ZipTextBox.Text = dat.stripHTML(ZipTextBox.Text.Trim());
                CityTextBox.Text = dat.stripHTML(CityTextBox.Text.Trim());
                StateTextBox.Text = dat.stripHTML(StateTextBox.Text.Trim());
                bool goOn = true;

                //bool isZip = false;

                //if (ZipTextBox.Text.Trim() != "")
                //{
                //    isZip = true;
                //}

                //if (isZip)
                //{
                //    if (ZipTextBox.Text.Trim() != "")
                //    {

                //        //do only if United States and not for international zip codes
                //        if (RadiusDropPanel.Visible)
                //        {
                //            //Get all zips within the specified radius

                //            if (int.TryParse(ZipTextBox.Text.Trim(), out zipParam))
                //            {
                //                if (RadiusDropDown.SelectedValue == "0")
                //                    zip = ";" + zipParam.ToString() + ";";
                //                else
                //                    zip = GetAllZipsInRadius(true);
                //            }
                //            else
                //            {
                //                goOn = false;
                //                message = "Zip code is not in the right format.";
                //            }
                //        }
                //        else
                //        {
                //            zip = " AND A.Zip = @zip ";
                //            zipParameter = ZipTextBox.Text.Trim();
                //        }
                //    }
                //    else
                //    {
                //        goOn = false;
                //        message = "You must at the very least include a zip code.";
                //    }
                //}
                //else
                //{
                if (StateDropDownPanel.Visible)
                {
                    if (StateDropDown.SelectedValue != "-1")
                    {
                        state = " AND A.CatState=@state ";
                        stateParam = StateDropDown.SelectedItem.Text;
                    }
                    else
                    {
                        goOn = false;
                        message = "You must include the State and City.";
                    }

                    if (CountryDropDown.SelectedValue == "223")
                    {
                        city = " AND A.CatCity=@city ";
                        cityParam = MajorCityDrop.SelectedItem.Text;
                    }
                    else
                    {
                        if (CityTextBox.Text != "")
                        {
                            city = " AND A.CatCity=@city ";
                            cityParam = CityTextBox.Text.ToLower();
                        }
                        else
                        {
                            goOn = false;
                            message = "You must include the State and City.";
                        }
                    }
                }
                else
                {
                    if (StateTextBox.Text != "")
                    {
                        state = " AND A.CatState=@state ";
                        stateParam = StateTextBox.Text;
                    }
                    else
                    {
                        goOn = false;
                        message = "You must include the State and City.";
                    }
                    //}

                    if (CountryDropDown.SelectedValue == "223")
                    {
                        city = " AND A.CatCity=@city ";
                        cityParam = MajorCityDrop.SelectedItem.Text;
                    }
                    else
                    {
                        if (CityTextBox.Text != "")
                        {
                            city = " AND A.CatCity=@city ";
                            cityParam = CityTextBox.Text.ToLower();
                        }
                        else
                        {
                            goOn = false;
                            message = "You must include the State and City.";
                        }
                    }
                }

                if (goOn)
                {
                    //string radius = "";

                    //if (RadiusDropPanel.Visible)
                    //    radius = RadiusDropDown.SelectedValue;
                    //else
                    //    radius = "NULL";

                    string command = "INSERT INTO SavedAdSearches (Keywords, State, Country, UserID, Live, City) " +
                        "VALUES('" + KeywordsTextBox.Text.Trim().Replace("'", "''") + "','" + stateParam.Replace("'", "''") +
                        "', " + CountryDropDown.SelectedItem.Value + ", '" + Session["User"].ToString() + "', 1, '" +
                        cityParam.Replace("'", "''") + "')";
                    SqlCommand cmd2 = new SqlCommand(command, conn);
                    cmd2.CommandType = CommandType.Text;
                    cmd2.ExecuteNonQuery();


                    string ID2 = "";

                    SqlCommand cmd = new SqlCommand("SELECT @@IDENTITY AS ID", conn);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd);
                    DataSet ds3 = new DataSet();
                    da2.Fill(ds3);

                    ID2 = ds3.Tables[0].Rows[0]["ID"].ToString();

                    Session["SavedSearchID"] = ID2;

                    SaveSearchCategories(ID2, ref CategoryTree);
                    SaveSearchCategories(ID2, ref RadTreeView1);

                    Session["SavedClicked"] = true;

                    MessageRadWindow.Height = 300;

                    message = "Search Saved";
                    Session["Message"] = message + "<table><tr><td align=\"center\"><savedadsearch>ID:" + ID2 + "</savedadsearch></td></tr></table><table><tr><td align=\"right\"><div style=\"padding-top: 20px;\" align=\"center\">" +
                    "<div style=\"width: 50px;\" onclick=\"Search()\">" +
                    "<div class=\"topDiv\" style=\"clear: both;\">" +
                      "  <img style=\"float: left;\" src=\"NewImages/ButtonLeft.png\" height=\"27px\" /> " +
                       " <div class=\"NavyLink\" style=\"font-size: 12px; text-decoration: none; padding-top: 5px;padding-left: 6px; padding-right: 6px;height: 27px;float: left;background: url('NewImages/ButtonPixel.png'); background-repeat: repeat-x;\">" +
                           " OK " +
                        "</div>" +
                       " <img style=\"float: left;\" src=\"NewImages/ButtonRight.png\" height=\"27px\" /> " +
                    "</div>" +
                    "</div>" +
                    "</div></td><td align=\"left\"><div style=\"padding-top: 20px;\" align=\"center\">" +
                    "<div style=\"width: 150px;\" onclick=\"Search('SearchesAndPages.aspx')\">" +
                    "<div class=\"topDiv\" style=\"clear: both;\">" +
                      "  <img style=\"float: left;\" src=\"NewImages/ButtonLeft.png\" height=\"27px\" /> " +
                       " <div class=\"NavyLink\" style=\"font-size: 12px; text-decoration: none; padding-top: 5px;padding-left: 6px; padding-right: 6px;height: 27px;float: left;background: url('NewImages/ButtonPixel.png'); background-repeat: repeat-x;\">" +
                           " Go To Your Searches " +
                        "</div>" +
                       " <img style=\"float: left;\" src=\"NewImages/ButtonRight.png\" height=\"27px\" /> " +
                    "</div>" +
                    "</div>" +
                    "</div></td></tr></table>";
                    MessageRadWindow.NavigateUrl = "Message.aspx";
                    MessageRadWindow.Visible = true;
                    MessageRadWindowManager.VisibleOnPageLoad = true;
                }
                else
                {
                    MessageLabel.Text = message;
                }
            }
        }
        else
        {
            message = "Search Saved";

            MessageRadWindow.Height = 300;

            Session["Message"] = message + "<table><tr><td align=\"center\"><savedadsearch>ID:" + Session["SavedSearchID"].ToString() + "</savedadsearch></td></tr></table><table><tr><td align=\"right\"><div style=\"padding-top: 20px;\" align=\"center\">" +
                    "<div style=\"width: 50px;\" onclick=\"Search()\">" +
                    "<div class=\"topDiv\" style=\"clear: both;\">" +
                      "  <img style=\"float: left;\" src=\"NewImages/ButtonLeft.png\" height=\"27px\" /> " +
                       " <div class=\"NavyLink\" style=\"font-size: 12px; text-decoration: none; padding-top: 5px;padding-left: 6px; padding-right: 6px;height: 27px;float: left;background: url('NewImages/ButtonPixel.png'); background-repeat: repeat-x;\">" +
                           " OK " +
                        "</div>" +
                       " <img style=\"float: left;\" src=\"NewImages/ButtonRight.png\" height=\"27px\" /> " +
                    "</div>" +
                    "</div>" +
                    "</div></td><td align=\"left\"><div style=\"padding-top: 20px;\" align=\"center\">" +
                    "<div style=\"width: 150px;\" onclick=\"Search('SearchesAndPages.aspx')\">" +
                    "<div class=\"topDiv\" style=\"clear: both;\">" +
                      "  <img style=\"float: left;\" src=\"NewImages/ButtonLeft.png\" height=\"27px\" /> " +
                       " <div class=\"NavyLink\" style=\"font-size: 12px; text-decoration: none; padding-top: 5px;padding-left: 6px; padding-right: 6px;height: 27px;float: left;background: url('NewImages/ButtonPixel.png'); background-repeat: repeat-x;\">" +
                           " Go To Your Searches " +
                        "</div>" +
                       " <img style=\"float: left;\" src=\"NewImages/ButtonRight.png\" height=\"27px\" /> " +
                    "</div>" +
                    "</div>" +
                    "</div></td></tr></table>";

            MessageRadWindow.NavigateUrl = "Message.aspx";
            MessageRadWindow.Visible = true;
            MessageRadWindowManager.VisibleOnPageLoad = true;
        }
    }
    
    protected void SaveSearchCategories(string ID, ref Telerik.Web.UI.RadTreeView CategoryTree)
    {
        List<Telerik.Web.UI.RadTreeNode> list = (List<Telerik.Web.UI.RadTreeNode>)CategoryTree.GetAllNodes();
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn);
        
        foreach (Telerik.Web.UI.RadTreeNode node in list)
        {
            if (node.Checked)
            {
                //SELECT [ID],[SearchID],[CategoryID] from [HippoHappenings].[dbo].[SavedAdSearches_Categories] 
                dat.Execute("INSERT INTO SavedAdSearches_Categories (SearchID, CategoryID) VALUES(" + ID + ", " + node.Value + ")");
            }
        }
    }

    protected void FillSavedSearch()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn);        
        DataSet dsSearches = dat.GetData("SELECT * FROM SavedAdSearches WHERE ID=" + Request.QueryString["search"].ToString());
        DataView dvSearches = new DataView(dsSearches.Tables[0], "", "", DataViewRowState.CurrentRows);
        DataSet dsCats = dat.GetData("SELECT * FROM SavedAdSearches_Categories WHERE SearchID=" +
                Request.QueryString["search"].ToString());
        DataView dvCates = new DataView(dsCats.Tables[0], "", "", DataViewRowState.CurrentRows);

        if (dvSearches.Count > 0)
        {
            if (dvSearches[0]["Keywords"] != null)
                if (dvSearches[0]["Keywords"].ToString().Trim() != "")
                {
                    KeywordsTextBox.Text = dvSearches[0]["Keywords"].ToString().Trim();
                }
            if (dvSearches[0]["Country"] != null)
                if (dvSearches[0]["Country"].ToString().Trim() != "")
                {
                    CountryDropDown.ClearSelection();
                    CountryDropDown.SelectedValue = dvSearches[0]["Country"].ToString().Trim();
                    ChangeState(CountryDropDown, new EventArgs());
                }
            if (dvSearches[0]["State"] != null)
                if (dvSearches[0]["State"].ToString().Trim() != "")
                {
                    DataSet dsStates = dat.GetData("SELECT * FROM State WHERE country_id=" + dvSearches[0]["Country"].ToString().Trim());
                    DataView dvStates = new DataView(dsStates.Tables[0], "", "", DataViewRowState.CurrentRows);
                    if (dvSearches.Count > 0)
                    {
                        StateDropDown.DataSource = dsStates;
                        StateDropDown.DataTextField = "state_2_code";
                        StateDropDown.DataValueField = "state_id";
                        StateDropDown.DataBind();

                        StateDropDownPanel.Visible = true;
                        StateTextBoxPanel.Visible = false;
                        StateDropDown.ClearSelection();
                        StateDropDown.Items.FindItemByText(dvSearches[0]["State"].ToString().Trim()).Selected = true;
                        ChangeCity(StateDropDown, new EventArgs());
                    }
                    else
                    {
                        StateTextBoxPanel.Visible = true;
                        StateDropDownPanel.Visible = false;
                        StateTextBox.Text = dvSearches[0]["State"].ToString().Trim();
                    }
                }

            if (CountryDropDown.SelectedValue == "223")
            {
                MajorCityDrop.ClearSelection();
                MajorCityDrop.Items.FindByText(dvSearches[0]["City"].ToString().Trim()).Selected = true;
            }
            else
            {
                CityTextBox.Text = dvSearches[0]["City"].ToString().Trim();
            }

            //bool isZip = false;

            //if (dvSearches[0]["Zip"] != null)
            //    if (dvSearches[0]["Zip"].ToString().Trim() != "")
            //    {
            //        char[] delim = { ';' };
            //        string[] tokens = dvSearches[0]["Zip"].ToString().Trim().Split(delim);

            //        ZipTextBox.Text = tokens[1].Trim();

            //        isZip = true;
            //    }

            //if (dvSearches[0]["Radius"] != null)
            //    if (dvSearches[0]["Radius"].ToString().Trim() != "")
            //    {
            //        RadiusDropDown.SelectedValue = dvSearches[0]["Radius"].ToString().Trim();
            //    }

            if (dvCates.Count > 0)
            {

                FillCategories(dsCats, ref CategoryTree);
                FillCategories(dsCats, ref RadTreeView1);

            }

            Search();
        }
    }

    protected void FillCategories(DataSet dsContent, ref Telerik.Web.UI.RadTreeView treeView)
    {
        treeView.DataBind();
        if (treeView.Nodes.Count > 0)
        {
            if (dsContent.Tables.Count > 0)
                for (int i = 0; i < dsContent.Tables[0].Rows.Count; i++)
                {
                    Telerik.Web.UI.RadTreeNode node =
                        (Telerik.Web.UI.RadTreeNode)treeView.FindNodeByValue(dsContent.Tables[0].Rows[i]["CategoryID"].ToString());

                    if (node != null)
                        node.Checked = true;
                }
        }
    }

    //protected string GetAllZipsInRadius(bool justString)
    //{
    //    HttpCookie cookie = Request.Cookies["BrowserDate"];
    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

    //    //do only if United States and not for international zip codes
    //    string zip = "";
    //    string nonExistantZip = "";
    //    if (ZipTextBox.Text.Trim() != "")
    //    {
    //        if (RadiusDropPanel.Visible)
    //        {
    //            //Get all zips within the specified radius
    //            int zipParam = 0;
    //            if (int.TryParse(ZipTextBox.Text.Trim(), out zipParam))
    //            {
    //                DataSet dsLatsLongs = dat.GetData("SELECT Latitude, Longitude FROM ZipCodes WHERE Zip='" +
    //                    zipParam + "'");

    //                //some zip codes don't exist in the database, find the closest one
    //                bool findClosest = false;
    //                if (dsLatsLongs.Tables.Count > 0)
    //                {
    //                    if (dsLatsLongs.Tables[0].Rows.Count > 0)
    //                    {

    //                    }
    //                    else
    //                    {
    //                        findClosest = true;
    //                    }
    //                }
    //                else
    //                {
    //                    findClosest = true;
    //                }

    //                if (findClosest)
    //                {
    //                    dsLatsLongs = null;
    //                    zip = " AND A.CatZip LIKE '%;" + zipParam.ToString() + ";%' ";
    //                    nonExistantZip = " OR A.CatZip LIKE '%;" + zipParam.ToString() + ";%'";
    //                    if (justString)
    //                    {
    //                        zip = ";" + zipParam + ";";
    //                    }
    //                    while (dsLatsLongs == null)
    //                    {
    //                        dsLatsLongs = dat.GetData("SELECT Latitude, Longitude FROM ZipCodes WHERE Zip='" + zipParam++ + "'");
    //                        if (dsLatsLongs.Tables.Count > 0)
    //                        {
    //                            if (dsLatsLongs.Tables[0].Rows.Count > 0)
    //                            {

    //                            }
    //                            else
    //                            {
    //                                dsLatsLongs = null;
    //                            }
    //                        }
    //                        else
    //                        {
    //                            dsLatsLongs = null;
    //                        }
    //                    }
    //                }

    //                //get all the zip codes within the specified radius
    //                DataSet dsZips = dat.GetData("SELECT Zip FROM ZipCodes WHERE dbo.GetDistance(" + dsLatsLongs.Tables[0].Rows[0]["Longitude"].ToString() +
    //                    ", " + dsLatsLongs.Tables[0].Rows[0]["Latitude"].ToString() + ", Longitude, Latitude) < " + RadiusDropDown.SelectedValue);

    //                if (dsZips.Tables.Count > 0)
    //                {
    //                    if (dsZips.Tables[0].Rows.Count > 0)
    //                    {


    //                        if (justString)
    //                        {
    //                            if (zip.Trim() == "")
    //                                zip += ";";
    //                            zip += zipParam.ToString() + ";";
    //                        }
    //                        else
    //                            zip = " AND (A.CatZip LIKE '%;" + zipParam.ToString() + ";%' " + nonExistantZip;

    //                        for (int i = 0; i < dsZips.Tables[0].Rows.Count; i++)
    //                        {
    //                            if (justString)
    //                                zip += dsZips.Tables[0].Rows[i]["Zip"].ToString() + ";";
    //                            else
    //                                zip += " OR A.CatZip LIKE '%;" + dsZips.Tables[0].Rows[i]["Zip"].ToString() + ";%' ";
    //                        }
    //                        if (!justString)
    //                            zip += ") ";
    //                    }
    //                    else
    //                    {
    //                        if (!justString)
    //                            zip = " AND A.CatZip LIKE '%;" + ZipTextBox.Text.Trim() + ";%'";
    //                    }
    //                }
    //                else
    //                {
    //                    if (!justString)
    //                        zip = " AND A.CatZip LIKE '%;" + ZipTextBox.Text.Trim() + ";%'";
    //                }


    //            }
    //            else
    //            {
    //                if (justString)
    //                {
    //                    zip = ";" + ZipTextBox.Text.Trim() + ";";
    //                }
    //                else
    //                    zip = " AND A.CatZip LIKE '%;" + ZipTextBox.Text.Trim() + ";%' ";
    //            }
    //        }
    //        else
    //        {
    //            if (justString)
    //            {
    //                zip = ";" + ZipTextBox.Text.Trim() + ";";
    //            }
    //            else
    //                zip = " AND A.CatZip LIKE '%;" + ZipTextBox.Text.Trim() + ";%' ";
    //        }
    //    }
    //    else
    //    {
    //        zip = null;
    //    }

    //    return zip;
    //}
}
