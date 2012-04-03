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

public partial class VenueSearch : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["Searching"] = "Venues";
        string ctrlname = Request.Params.Get("__EVENTTARGET");
        Telerik.Web.UI.RadComboBox StateDropDown = (Telerik.Web.UI.RadComboBox)locationSearchPanel.Items[0].Items[0].FindControl("StateDropDown");
        Telerik.Web.UI.RadTextBox StateTextBox = (Telerik.Web.UI.RadTextBox)locationSearchPanel.Items[0].Items[0].FindControl("StateTextBox");
        Telerik.Web.UI.RadTextBox CityTextBox = (Telerik.Web.UI.RadTextBox)locationSearchPanel.Items[0].Items[0].FindControl("CityTextBox");
        Telerik.Web.UI.RadTextBox ZipTextBox = (Telerik.Web.UI.RadTextBox)locationSearchPanel.Items[1].Items[0].FindControl("ZipTextBox");
        

        

        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet ds;

        

        //ASP.controls_venuesearchelements_ascx SearchElements2 = 
        //    (ASP.controls_venuesearchelements_ascx)SearchPanel.Items[1].Items[0].FindControl("SearchElements2");

        try
        {
            if (IsPostBack)
            {
                //if (Session["SearchDS"] != null)
                //{
                //    if (Session["Searching"].ToString() == "Venues")
                //    {
                //        ds = (DataSet)Session["SearchDS"];
                //        if (ds.Tables[0].Rows.Count > 0)
                //        {
                //            SearchElements2.VENUE_DS = ds;
                //            SearchElements2.DataBind2();

                //            SearchPanel.Items[0].Expanded = false;
                //            SearchPanel.Items[1].Expanded = true;

                //            int eventCount = ds.Tables[0].Rows.Count;

                //            if (dat.SearchCutOffNumber < eventCount)
                //            {
                //                SearchPanel.Items[1].Text = "<div style='position: relative;'><span style='font-family: Arial; font-size: 20px; color: White;'>Search Results: </span><div style='position: absolute; right: 5px; top: 3px;'>(Search cut off at " + dat.SearchCutOffNumber.ToString() + ")</div></div>";

                //            }
                //            else
                //            {

                //                SearchPanel.Items[1].Text = "<div style='position: relative;'><span style='font-family: Arial; font-size: 20px; color: White;'>Search Results: </span><div style='position: absolute; right: 5px; top: 3px;'>(" + ds.Tables[0].Rows.Count + " Records Found)</div></div>";
                //            }
                //        }
                //        else
                //        {
                //            SearchPanel.Items[0].Expanded = true;
                //            SearchPanel.Items[1].Expanded = false;
                //        }
                //    }
                //}
                //else
                //{
                //    SearchPanel.Items[0].Expanded = true;
                //    SearchPanel.Items[1].Expanded = false;
                //}
            }
        }
        catch (Exception ex)
        {
            //SearchPanel.Items[0].Expanded = true;
            //SearchPanel.Items[1].Expanded = false;
        }
        //Telerik.Web.UI.RadPanelBar locationSearchPanel = (Telerik.Web.UI.RadPanelBar)SearchPanel.Items[0].Items[0].FindControl("locationSearchPanel");

        //Telerik.Web.UI.RadComboBox DateDropDown = (Telerik.Web.UI.RadComboBox)locationSearchPanel.Items[0].Items[0].FindControl("DateDropDown");


        //Telerik.Web.UI.RadComboBox CountryDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("CountryDropDown");
        //Telerik.Web.UI.RadComboBox StateDropDown = (Telerik.Web.UI.RadComboBox)locationSearchPanel.Items[0].Items[0].FindControl("StateDropDown");
        Panel StateDropDownPanel = (Panel)locationSearchPanel.Items[0].Items[0].FindControl("StateDropDownPanel");
        Panel StateTextBoxPanel = (Panel)locationSearchPanel.Items[0].Items[0].FindControl("StateTextBoxPanel");
        //Telerik.Web.UI.RadComboBox VenueDropDown = (Telerik.Web.UI.RadComboBox)locationSearchPanel.Items[0].Items[0].FindControl("VenueDropDown");

        if (!IsPostBack)
        {
            HtmlLink lk = new HtmlLink();
            HtmlHead head = (HtmlHead)Page.Header;
            lk.Attributes.Add("rel", "canonical");
            lk.Href = "http://hippohappenings.com/VenueSearch.aspx";
            head.Controls.AddAt(0, lk);

            //Create keyword and description meta tags
            HtmlMeta hm = new HtmlMeta();
            HtmlMeta kw = new HtmlMeta();



            kw.Name = "keywords";
            kw.Content = "find local venue place, spot, neighborhood, community, search";

            head.Controls.AddAt(0, kw);

            hm.Name = "Description";
            hm.Content = "Find local venues in your neighborhood on HippoHappenings, the site which fosters communities.";
            head.Controls.AddAt(0, hm);

            lk = new HtmlLink();
            lk.Href = "http://" + Request.Url.Authority + "/VenueSearch.aspx";
            lk.Attributes.Add("rel", "bookmark");
            head.Controls.AddAt(0, lk);

            EventName.Text = "<a style=\"text-decoration: none; color: white;\" href=\"" +
                lk.Href + "\">Search Venues</a>";

            Page.Title = "Find local venues | HippoHappenings";

            CountryDropDown.SelectedValue = "223";

            DataSet ds3 = dat.GetData("SELECT * FROM State WHERE country_id=" + CountryDropDown.SelectedValue);

            StateDropDown.ClearSelection();
            StateDropDownPanel.Visible = true;
            StateTextBoxPanel.Visible = false;
            StateDropDown.DataSource = ds3;
            StateDropDown.DataTextField = "state_2_code";
            StateDropDown.DataValueField = "state_id";
            StateDropDown.DataBind();

            StateDropDown.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("State", "-1"));

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

                        locationSearchPanel.Items[1].Expanded = true;

                        //Panel RadiusTitlePanel = (Panel)locationSearchPanel.Items[1].Items[0].FindControl("RadiusTitlePanel");
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
            try
            {
                if (ctrlname == "ctl00_ContentPlaceHolder1_SearchButton")
                {
                    bool goodGo = false;
                    if (((StateDropDown.SelectedValue != "-1" || StateTextBox.Text.Trim() != "")
                        && CityTextBox.Text.Trim() != "") || ZipTextBox.Text.Trim() != "")
                    {

                        RadWindow RadWindow3 = Master.RAD_WINDOW_3;
                        RadWindow3.VisibleOnPageLoad = true;
                    }
                    else
                    {
                        //MessageLabel.Text = sender.ToString() + e.ToString();
                    }
                }
                //MessageLabel.Text = ctrlname;
            }
            catch (Exception ex)
            {
                MessageLabel.Text = ctrlname;
            }
        }

        try
        {
            if (Session["User"] != null)
            {
            }
            else
            {
                    
                
                //Button calendarLink = (Button)dat.FindControlRecursive(this, "CalendarLink");
                //calendarLink.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }

    //function only used for initially inserting zip code information into the database.
    protected void CreateZipDB()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        /*
             * From documentation found here: http://www.census.gov/geo/www/gazetteer/places2k.html
                The ZCTA file contains data for all 5 digit ZCTAs in the 50 states,
                         District of Columbia and Puerto Rico as of Census 2000. The file is plain ASCII text, one line per record.

                Columns 1-2: United States Postal Service State Abbreviation
                Columns 3-66: Name (e.g. 35004 5-Digit ZCTA - there are no post office names)
                Columns 67-75: Total Population (2000)
                Columns 76-84: Total Housing Units (2000)
                Columns 85-98: Land Area (square meters) - Created for statistical purposes only.
                Columns 99-112: Water Area (square meters) - Created for statistical purposes only.
                Columns 113-124: Land Area (square miles) - Created for statistical purposes only.
                Columns 125-136: Water Area (square miles) - Created for statistical purposes only.
                Columns 137-146: Latitude (decimal degrees) First character is blank or "-" denoting North or South latitude respectively
                Columns 147-157: Longitude (decimal degrees) First character is blank or "-" denoting East or West longitude respectively
             */

        
       //System.IO.FileStream fileStrm = System.IO.File.Open(Server.MapPath("~/UserFiles/zcta5.txt"), System.IO.FileMode.Open);
       //fileStrm.Flush();
       // fileStrm.Close();
       
        System.IO.StreamReader reader = new System.IO.StreamReader(Server.MapPath("~/VenueFiles/zcta5.txt"));
       Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            string line = reader.ReadLine();
            while (!string.IsNullOrEmpty(line))
            {
                int code;
                double lat;
                double lon;
                // skip lines that aren't valid
                if (Int32.TryParse(line.Substring(2, 5), out code) &&
                    double.TryParse(line.Substring(136, 10), out lat) &&
                    double.TryParse(line.Substring(146, 10), out lon)
                    )
                {
                    dat.Execute("INSERT INTO ZipCodes (State, Zip, Latitude, Longitude) VALUES('" + line.Substring(0, 2)
                        + "'," + line.Substring(2, 5) + ", '" + line.Substring(136, 10).Trim() + "', '" + line.Substring(147, 10).Trim() + "')");
                }
                line = reader.ReadLine();
            }
    }
    
    //protected void Page_Unload(object sender, EventArgs e)
    //{
    //    Session["AdsLoaded"] = null;
    //}

    protected void Page_Init(object sender, EventArgs e)
    {
            //Literal lit = new Literal();
            //lit.Text = "<style type=\"text/css\">.RadComboBox_Black, .RadComboBox_Black "+
            //    ".rcbInputCell .rcbInput, .RadComboBoxDropDown_Black{border: 1px solid #628E02;}"+
            //    "html body .RadInput_Black .riTextBox, html body .RadInputMgr_Black{border: 1px solid #628E02 !important;}</style>";

            //Page.Header.Controls.Add(lit);
    }

    protected void GoToLogin(object sender, EventArgs e)
    {
        Session["RedirectTo"] = Request.Url.AbsoluteUri;
        Response.Redirect("EnterVenueIntro.aspx");
    }

    protected void ChangeCheckImage(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        ImageButton CheckImageButton = (ImageButton)dat.FindControlRecursive(this, "CheckImageButton");

        if (CheckImageButton.ImageUrl == "image/Check.png")
            CheckImageButton.ImageUrl = "image/CheckSelected.png";
        else
        {
            CheckImageButton.ImageUrl = "image/Check.png";
            CheckBoxList CategoriesCheckBoxes = (CheckBoxList)dat.FindControlRecursive(this, "CategoriesCheckBoxes");
            for (int i = 0; i < CategoriesCheckBoxes.Items.Count; i++)
                CategoriesCheckBoxes.Items[i].Selected = false;
        }
    }

    protected void CheckCheckImage(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        ImageButton CheckImageButton = (ImageButton)dat.FindControlRecursive(this, "CheckImageButton");
        CheckImageButton.ImageUrl = "image/CheckSelected.png";
    }

    protected void Search(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        //ASP.controls_venuesearchelements_ascx SearchElements2 = (ASP.controls_venuesearchelements_ascx)SearchPanel.Items[1].Items[0].FindControl("SearchElements2");

        

        //Telerik.Web.UI.RadPanelBar locationSearchPanel = (Telerik.Web.UI.RadPanelBar)SearchPanel.Items[0].Items[0].FindControl("locationSearchPanel");

        //Telerik.Web.UI.RadComboBox DateDropDown = (Telerik.Web.UI.RadComboBox)locationSearchPanel.Items[0].Items[0].FindControl("DateDropDown");


        //Telerik.Web.UI.RadComboBox CountryDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("CountryDropDown");
        Telerik.Web.UI.RadComboBox StateDropDown = (Telerik.Web.UI.RadComboBox)locationSearchPanel.Items[0].Items[0].FindControl("StateDropDown");
        Panel StateDropDownPanel = (Panel)locationSearchPanel.Items[0].Items[0].FindControl("StateDropDownPanel");
        Panel StateTextBoxPanel = (Panel)locationSearchPanel.Items[0].Items[0].FindControl("StateTextBoxPanel");
        //Telerik.Web.UI.RadComboBox VenueDropDown = (Telerik.Web.UI.RadComboBox)locationSearchPanel.Items[0].Items[0].FindControl("VenueDropDown");

        Telerik.Web.UI.RadComboBox RadiusDropDown = (Telerik.Web.UI.RadComboBox)locationSearchPanel.Items[1].Items[0].FindControl("RadiusDropDown");
        //Label MessageLabel = (Label)SearchPanel.Items[0].Items[0].FindControl("MessageLabel");
        MessageLabel.Text = "";
        //Button SearchButton = (Button)SearchPanel.Items[0].Items[0].FindControl("SearchButton");

        //ASP.controls_hippotextbox_ascx SearchTextBox = (ASP.controls_hippotextbox_ascx)SearchPanel.Items[0].Items[0].FindControl("SearchTextBox");
        Telerik.Web.UI.RadTextBox StateTextBox = (Telerik.Web.UI.RadTextBox)locationSearchPanel.Items[0].Items[0].FindControl("StateTextBox");
        Telerik.Web.UI.RadTextBox CityTextBox = (Telerik.Web.UI.RadTextBox)locationSearchPanel.Items[0].Items[0].FindControl("CityTextBox");
        //Telerik.Web.UI.RadComboBox RatingDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("RatingDropDown");
        //Telerik.Web.UI.RadTreeView CategoryTree = (Telerik.Web.UI.RadTreeView)SearchPanel.Items[0].Items[0].FindControl("CategoryTree");
        //Telerik.Web.UI.RadTreeView RadTreeView1 = (Telerik.Web.UI.RadTreeView)SearchPanel.Items[0].Items[0].FindControl("RadTreeView1");
        Telerik.Web.UI.RadTextBox ZipTextBox = (Telerik.Web.UI.RadTextBox)locationSearchPanel.Items[1].Items[0].FindControl("ZipTextBox");

        //Panel RadiusTitlePanel = (Panel)locationSearchPanel.Items[1].Items[0].FindControl("RadiusTitlePanel");
        Panel RadiusDropPanel = (Panel)locationSearchPanel.Items[1].Items[0].FindControl("RadiusDropPanel");
        //Panel USLabelPanel = (Panel)locationSearchPanel.Items[1].Items[0].FindControl("USLabelPanel");

        //SearchElements2.Clear();

        string resultsStr = "";

            char[] delim = { ' ' };
            string[] tokens;

            SearchTextBox.THE_TEXT = dat.stripHTML(SearchTextBox.THE_TEXT);

            string message = "";
            bool goOn = true;
            string temp = "";
            if (SearchTextBox.THE_TEXT.Trim() != "")
            {
                tokens = SearchTextBox.THE_TEXT.Trim().Split(delim);
                resultsStr += " for '" + SearchTextBox.THE_TEXT.Trim() + "'";
                for (int i = 0; i < tokens.Length; i++)
                { 
                    temp += " AND V.Name LIKE @search" + i.ToString();
                }
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

            bool isZip = false;

            if (locationSearchPanel.Items[1].Expanded)
            {
                isZip = true;
            }



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
                for (int i = 0; i < CategoryTree.Nodes.Count; i++)
                {
                    if (CategoryTree.Nodes[i].Checked)
                    {
                        if (categories == "")
                        {
                            addCat = ", Venue_Category VC ";
                            categories += " AND  V.ID=VC.VENUE_ID  AND (";
                            categories += " VC.CATEGORY_ID = " + CategoryTree.Nodes[i].Value;
                        }
                        else
                            categories += " OR VC.CATEGORY_ID = " + CategoryTree.Nodes[i].Value;


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
                                categories += " OR VC.CATEGORY_ID = " + CategoryTree.Nodes[i].Nodes[h].Value;

                                if (CategoryTree.Nodes[i].Nodes[h].Nodes.Count > 0)
                                {
                                    for (int k = 0; k < CategoryTree.Nodes[i].Nodes[h].Nodes.Count; k++)
                                    {
                                        categories += " OR VC.CATEGORY_ID = " + CategoryTree.Nodes[i].Nodes[h].Nodes[k].Value;
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
                                addCat = ", Venue_Category VC ";
                                categories += " AND  V.ID=VC.VENUE_ID  AND (";
                                categories += " VC.CATEGORY_ID = " + CategoryTree.Nodes[i].Nodes[n].Value;
                            }
                            else
                                categories += " OR VC.CATEGORY_ID = " + CategoryTree.Nodes[i].Nodes[n].Value;


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
                            addCat = ", Venue_Category VC ";
                            categories += " AND  V.ID=VC.VENUE_ID  AND (";
                            categories += " VC.CATEGORY_ID = " + RadTreeView1.Nodes[i].Value;
                        }
                        else
                            categories += " OR VC.CATEGORY_ID = " + RadTreeView1.Nodes[i].Value;

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
                                categories += " OR VC.CATEGORY_ID = " + RadTreeView1.Nodes[i].Nodes[h].Value;

                                if (RadTreeView1.Nodes[i].Nodes[h].Nodes.Count > 0)
                                {
                                    for (int k = 0; k < RadTreeView1.Nodes[i].Nodes[h].Nodes.Count; k++)
                                    {
                                        categories += " OR VC.CATEGORY_ID = " + RadTreeView1.Nodes[i].Nodes[h].Nodes[k].Value;
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
                                addCat = ", Venue_Category VC ";
                                categories += " AND  V.ID=VC.VENUE_ID  AND (";
                                categories += " VC.CATEGORY_ID = " + RadTreeView1.Nodes[i].Nodes[n].Value;
                            }
                            else
                                categories += " OR VC.CATEGORY_ID = " + RadTreeView1.Nodes[i].Nodes[n].Value;

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

                string searchStr = "SELECT DISTINCT V.ID AS VID, V.Name, V.City, V.State, V.Address FROM Venues V " + addCat +
                    " WHERE V.Live='True' " + temp + country + state + city + rating + categories + zip +" ORDER BY V.Name ";
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                SqlCommand cmd = new SqlCommand(searchStr, conn);
                if (SearchTextBox.THE_TEXT.Trim() != "")
                {
                    tokens = SearchTextBox.THE_TEXT.Trim().Split(delim);
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
                //Label ResultsLabel = (Label)SearchPanel.Items[1].Items[0].FindControl("ResultsLabel");

                //ResultsLabel.Text = "Results " + resultsStr;
                //Save search location base on IP if user not logged in.

                if (Session["User"] != null)
                {
                    dat.SetLocationForIP(CountryDropDown.SelectedValue, cityParam, stateParam);
                }
                //SearchButton.PostBackUrl = "~/VenueSearch.aspx";
            }
            else
            {
                MessageLabel.Text = message;
                //SearchPanel.Items[1].Expanded = false;
                //SearchPanel.Items[0].Expanded = true;
            }
      
    }

    protected void ChangeState(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        //Telerik.Web.UI.RadPanelBar locationSearchPanel = (Telerik.Web.UI.RadPanelBar)SearchPanel.Items[0].Items[0].FindControl("locationSearchPanel");

        Telerik.Web.UI.RadComboBox DateDropDown = (Telerik.Web.UI.RadComboBox)locationSearchPanel.Items[0].Items[0].FindControl("DateDropDown");


        //Telerik.Web.UI.RadComboBox CountryDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("CountryDropDown");
        Telerik.Web.UI.RadComboBox StateDropDown = (Telerik.Web.UI.RadComboBox)locationSearchPanel.Items[0].Items[0].FindControl("StateDropDown");
        Panel StateDropDownPanel = (Panel)locationSearchPanel.Items[0].Items[0].FindControl("StateDropDownPanel");
        Panel StateTextBoxPanel = (Panel)locationSearchPanel.Items[0].Items[0].FindControl("StateTextBoxPanel");

        //Panel RadiusTitlePanel = (Panel)locationSearchPanel.Items[1].Items[0].FindControl("RadiusTitlePanel");
        Panel RadiusDropPanel = (Panel)locationSearchPanel.Items[1].Items[0].FindControl("RadiusDropPanel");
        //Panel USLabelPanel = (Panel)locationSearchPanel.Items[1].Items[0].FindControl("USLabelPanel"); 
        //ASP.controls_hippotextbox_ascx SearchTextBox = (ASP.controls_hippotextbox_ascx)SearchPanel.Items[1].FindControl("SearchTextBox");

        if (CountryDropDown.SelectedValue != "223")
        {
            //RadiusTitlePanel.Visible = false;
            RadiusDropPanel.Visible = false;
            //USLabelPanel.Visible = false;
        }
        else
        {
            //RadiusTitlePanel.Visible = true;
            RadiusDropPanel.Visible = true;
            //USLabelPanel.Visible = true;
        }

        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet ds = dat.GetData("SELECT * FROM State WHERE country_id=" + CountryDropDown.SelectedValue);

        bool isTextBox = false;
        if (ds.Tables.Count > 0)
            if (ds.Tables[0].Rows.Count > 0)
            {
                StateDropDown.Items.Clear();
                StateDropDown.ClearSelection();
                StateDropDownPanel.Visible = true;
                StateTextBoxPanel.Visible = false;
                StateDropDown.DataSource = ds;
                StateDropDown.DataTextField = "state_2_code";
                StateDropDown.DataValueField = "state_id";
                StateDropDown.DataBind();

                StateDropDown.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("State", "-1"));

               
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

    //protected void SearchAdvanced(object sender, EventArgs e)
    //{
    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //    SearchElements.Clear();
    //    RadPanelBar1.CollapseAllItems();
    //    ASP.controls_hippotextbox_ascx venuename = (ASP.controls_hippotextbox_ascx)dat.FindControlRecursive(this, "VenueNameTextBox");
    //    ASP.controls_hippotextbox_ascx addresskey = (ASP.controls_hippotextbox_ascx)dat.FindControlRecursive(this, "LocationTextBox");
    //    ASP.controls_hippotextbox_ascx eventname = (ASP.controls_hippotextbox_ascx)dat.FindControlRecursive(this, "EventNameTextBox");
    //    DropDownList QuadrantDropDown = (DropDownList)dat.FindControlRecursive(this, "QuadrantDropDown");

    //    string queryBegin = "SELECT DISTINCT V.ID AS VID, V.Name, V.Address, V.City, V.State FROM Venues V ";
    //    string query = "";

    //    string temp = "";
        
    //    ImageButton CheckImageButton = (ImageButton)dat.FindControlRecursive(this, "CheckImageButton");
    //    CheckBoxList CategoriesCheckBoxes = (CheckBoxList)dat.FindControlRecursive(this, "CategoriesCheckBoxes");
    //    if (CheckImageButton.ImageUrl == "image/CheckSelected.png")
    //    {
    //        queryBegin += ", Venue_Category VC "; 
    //        temp = " V.ID=VC.VENUE_ID ";
    //        for (int i = 0; i < CategoriesCheckBoxes.Items.Count; i++)
    //        {
    //            if (CategoriesCheckBoxes.Items[i].Selected)
    //            {
    //                if (temp == "")
    //                {
    //                    temp += " AND (";
    //                    temp += " VC.CATEGORY_ID = " + CategoriesCheckBoxes.Items[i].Value;
    //                }
    //                else
    //                    temp += " OR VC.CATEGORY_ID = " + CategoriesCheckBoxes.Items[i].Value;
    //            }


    //        }
    //        if (temp != "")
    //            temp += " ) ";

    //        if (query == "")
    //            query = temp;
    //        else
    //            query += " AND " + temp;
    //    }
    //    char[] delim = { ' ' };
    //    string[] tokens;

    //    if (venuename.THE_TEXT != "")
    //    {
    //        tokens = venuename.THE_TEXT.Split(delim);
    //        temp = " ( ";
    //        for (int i = 0; i < tokens.Length; i++)
    //        {
    //            temp += " V.Name LIKE @venueName"+i.ToString();

    //            if (i + 1 != tokens.Length)
    //                temp += " OR ";
    //        }

    //        temp += " ) ";
    //        if (query == "")
    //            query = temp;
    //        else
    //            query += " AND " + temp;
    //    }

    //    if (addresskey.THE_TEXT != "")
    //    {
    //        tokens = addresskey.THE_TEXT.Split(delim);
    //        temp = " ( ";
    //        for (int i = 0; i < tokens.Length; i++)
    //        {
    //            string c = "@address" + i.ToString();
    //            temp += " V.Address LIKE "+c+" OR V.City LIKE "+c+" OR State LIKE "+c;

    //            if (i + 1 != tokens.Length)
    //                temp += " OR ";
    //        }

    //        temp += " ) ";

    //        if (query == "")
    //            query = temp;
    //        else
    //            query += " AND " + temp;
    //    }

    //    if (eventname.THE_TEXT != "")
    //    {
    //        tokens = eventname.THE_TEXT.Split(delim);
    //        temp = " E.Venue=V.ID AND ( ";
    //        for (int i = 0; i < tokens.Length; i++)
    //        {
                
    //            temp += " E.Header LIKE @event"+i.ToString();

    //            if (i + 1 != tokens.Length)
    //                temp += " OR ";
    //        }

    //        temp += " ) ";
    //        queryBegin += ", Events E ";

    //        if (query == "")
    //            query = temp;
    //        else
    //            query += " AND " + temp;
    //    }

        



    //    if (query != "")
    //    {
            
    //        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
    //        if (conn.State != ConnectionState.Open)
    //            conn.Open();
    //        SqlCommand cmd = new SqlCommand(queryBegin + " WHERE V.Live='True' AND " + query, conn);
    //        tokens = venuename.THE_TEXT.Split(delim);
    //        for (int i = 0; i < tokens.Length; i++)
    //        {
    //            cmd.Parameters.Add("@venueName" + i.ToString(), SqlDbType.NVarChar).Value = "%" + tokens[i] + "%";
    //        }
    //        tokens = addresskey.THE_TEXT.Split(delim);
    //        for (int i = 0; i < tokens.Length; i++)
    //        {
    //            cmd.Parameters.Add("@address" + i.ToString(), SqlDbType.NVarChar).Value = "%" + tokens[i] + "%";
    //        }
    //        tokens = eventname.THE_TEXT.Split(delim);
    //        for (int i = 0; i < tokens.Length; i++)
    //        {
    //            cmd.Parameters.Add("@event" + i.ToString(), SqlDbType.NVarChar).Value = "%" + tokens[i] + "%";
    //        }
    //        DataSet ds = new DataSet();
    //        SqlDataAdapter da = new SqlDataAdapter(cmd);
    //        da.Fill(ds);
    //        conn.Close();

    //        ViewState["SearchDS"] = ds;
    //        if (ds.Tables.Count > 0)
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                SearchElements.VENUE_DS = ds;
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
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        ImageButton ImageButton1 = (ImageButton)dat.FindControlRecursive(this, "ImageButton1");
        ImageButton ImageButton2 = (ImageButton)dat.FindControlRecursive(this, "ImageButton2");

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
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        ImageButton ImageButton2 = (ImageButton)dat.FindControlRecursive(this, "ImageButton2");
        ImageButton ImageButton1 = (ImageButton)dat.FindControlRecursive(this, "ImageButton1");

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

    //protected void SortResults(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (Session["SearchDS"] != null)
    //        {
    //            DropDownList SortDropDown = (DropDownList)SearchPanel.Items[1].Items[0].FindControl("SortDropDown");
    //            ASP.controls_venuesearchelements_ascx SearchElements2 = (ASP.controls_venuesearchelements_ascx)SearchPanel.Items[1].Items[0].FindControl("SearchElements2");

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
    //                SearchElements2.VENUE_DS = ds;
    //                SearchElements2.IS_WINDOW = true;
    //                SearchElements2.DataBind2();
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}

}
