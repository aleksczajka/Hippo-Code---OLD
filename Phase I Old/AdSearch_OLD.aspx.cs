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
using System.Collections.Generic;
using Telerik.Web.UI;

public partial class AdSearch : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["Searching"] = "Ads";
        string ctrlname = Request.Params.Get("__EVENTTARGET");
        Telerik.Web.UI.RadComboBox StateDropDown = (Telerik.Web.UI.RadComboBox)locationSearchPanel.Items[0].Items[0].FindControl("StateDropDown");
        Telerik.Web.UI.RadTextBox StateTextBox = (Telerik.Web.UI.RadTextBox)locationSearchPanel.Items[0].Items[0].FindControl("StateTextBox");
        Telerik.Web.UI.RadTextBox CityTextBox = (Telerik.Web.UI.RadTextBox)locationSearchPanel.Items[0].Items[0].FindControl("CityTextBox");
        TextBox ZipTextBox = (TextBox)locationSearchPanel.Items[1].Items[0].FindControl("ZipTextBox");

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

        HtmlLink lk = new HtmlLink();
        HtmlHead head = (HtmlHead)Page.Header;
        lk.Attributes.Add("rel", "canonical");
        lk.Href = "http://hippohappenings.com/AdSearch.aspx";
        head.Controls.AddAt(0, lk);

        Session["s"] = "asfsd";
        if (!IsPostBack)
        {
            Session["SavedClicked"] = null;
            Session.Remove("SavedClicked");
        }
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

        //Create keyword and description meta tags
        HtmlMeta hm = new HtmlMeta();
        HtmlMeta kw = new HtmlMeta();



        kw.Name = "keywords";
        kw.Content = "ad, classified, neighborhood, community, local, find, search";

        head.Controls.AddAt(0, kw);

        hm.Name = "Description";
        hm.Content = "Find/search local ad in your neighborhood on HippoHappenings, the site which fosters communities.";
        head.Controls.AddAt(0, hm);

        lk = new HtmlLink();
        lk.Href = "http://" + Request.Url.Authority + "/AdSearch.aspx";
        lk.Attributes.Add("rel", "bookmark");
        head.Controls.AddAt(0, lk);

        EventName.Text = "<a style=\"text-decoration: none; color: white;\" href=\"http://" + Request.Url.Authority + "/AdSearch.aspx\">Search Classifieds</a>";

                Panel StateDropDownPanel = (Panel)locationSearchPanel.Items[0].Items[0].FindControl("StateDropDownPanel");
                Panel StateTextBoxPanel = (Panel)locationSearchPanel.Items[0].Items[0].FindControl("StateTextBoxPanel");
                //ASP.controls_hippotextbox_ascx SearchTextBox = (ASP.controls_hippotextbox_ascx)SearchPanel.Items[0].Items[0].FindControl("SearchTextBox");
                //Telerik.Web.UI.RadTextBox StateTextBox = (Telerik.Web.UI.RadTextBox)locationSearchPanel.Items[0].Items[0].FindControl("StateTextBox");
                //Telerik.Web.UI.RadComboBox VenueDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("VenueDropDown");
                //Label MessageLabel = (Label)SearchPanel.Items[0].Items[0].FindControl("MessageLabel");
                //Label Label1 = (Label)SearchPanel.Items[0].Items[0].FindControl("Label1");
                MessageLabel.Text = "";
                Label1.Text = "";
            
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

                if (Session["User"] != null)
                {
                    DataView dvUser = dat.GetDataDV("SELECT CatCity, CatState, CatCountry, CatZip, Radius FROM  UserPreferences WHERE UserID=" +
                        Session["User"].ToString());
                    //Telerik.Web.UI.RadTextBox CityTextBox = (Telerik.Web.UI.RadTextBox)locationSearchPanel.Items[0].Items[0].FindControl("CityTextBox");
                    CountryDropDown.SelectedValue = dvUser[0]["CatCountry"].ToString();
                    ChangeState(CountryDropDown, new EventArgs());
                    if (StateTextBoxPanel.Visible)
                        StateTextBox.Text = dvUser[0]["CatState"].ToString();
                    else
                        StateDropDown.Items.FindItemByText(dvUser[0]["CatState"].ToString()).Selected = true;

                    CityTextBox.Text = dvUser[0]["CatCity"].ToString();
                    //ChangeState(StateDropDown, new EventArgs());

                    if (dvUser[0]["CatZip"] != null)
                    {
                        if (dvUser[0]["CatZip"].ToString().Trim() != "")
                        {

                            locationSearchPanel.Items[1].Expanded = true;
                            //TextBox ZipTextBox = (TextBox)locationSearchPanel.Items[1].Items[0].FindControl("ZipTextBox");

                            Panel RadiusTitlePanel = (Panel)locationSearchPanel.Items[1].Items[0].FindControl("RadiusTitlePanel");
                            Panel RadiusDropPanel = (Panel)locationSearchPanel.Items[1].Items[0].FindControl("RadiusDropPanel");

                            char[] delim = { ';' };
                            string[] tokens = dvUser[0]["CatZip"].ToString().Split(delim);

                            if (tokens.Length > 1)
                            {
                                ZipTextBox.Text = tokens[1].Trim();

                                DropDownList RadiusDropDown = (DropDownList)locationSearchPanel.Items[1].Items[0].FindControl("RadiusDropDown");

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

        
            
            try
            {
                

                //if (Session["User"] != null)
                //{
                //    LoggedInPanel.Visible = true;
                //    LogInPanel.Visible = false;

                //}
                //else
                //{
                //    LoggedInPanel.Visible = false;
                //    LogInPanel.Visible = true;
                //}
            }
            catch (Exception ex)
            {
                //Response.Redirect("~/UserLogin.aspx");
            }

            MessageRadWindowManager.VisibleOnPageLoad = false;
        
    }

    protected void FillSavedSearch()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet dsSearches = dat.GetData("SELECT * FROM SavedAdSearches WHERE ID=" + Request.QueryString["search"].ToString());
        DataView dvSearches = new DataView(dsSearches.Tables[0], "", "", DataViewRowState.CurrentRows);
        DataSet dsCats = dat.GetData("SELECT * FROM SavedAdSearches_Categories WHERE SearchID=" +
                Request.QueryString["search"].ToString());
        DataView dvCates = new DataView(dsCats.Tables[0], "", "", DataViewRowState.CurrentRows);

        if (dvSearches.Count > 0)
        {
            //Telerik.Web.UI.RadPanelBar locationSearchPanel = (Telerik.Web.UI.RadPanelBar)SearchPanel.Items[0].Items[0].FindControl("locationSearchPanel");
            //ASP.controls_adsearchelements_ascx SearchElements2 = (ASP.controls_adsearchelements_ascx)SearchPanel.Items[1].Items[0].FindControl("SearchElements2");
            //Telerik.Web.UI.RadComboBox CountryDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("CountryDropDown");
            Telerik.Web.UI.RadComboBox StateDropDown = (Telerik.Web.UI.RadComboBox)locationSearchPanel.Items[0].Items[0].FindControl("StateDropDown");
            Panel StateDropDownPanel = (Panel)locationSearchPanel.Items[0].Items[0].FindControl("StateDropDownPanel");
            Panel StateTextBoxPanel = (Panel)locationSearchPanel.Items[0].Items[0].FindControl("StateTextBoxPanel");
            //ASP.controls_hippotextbox_ascx SearchTextBox = (ASP.controls_hippotextbox_ascx)SearchPanel.Items[0].Items[0].FindControl("SearchTextBox");
            Telerik.Web.UI.RadTextBox StateTextBox = (Telerik.Web.UI.RadTextBox)locationSearchPanel.Items[0].Items[0].FindControl("StateTextBox");
            //Telerik.Web.UI.RadTreeView CategoryTree = (Telerik.Web.UI.RadTreeView)SearchPanel.Items[0].Items[0].FindControl("CategoryTree");
            //Telerik.Web.UI.RadTreeView RadTreeView1 = (Telerik.Web.UI.RadTreeView)SearchPanel.Items[0].Items[0].FindControl("RadTreeView1");
            TextBox ZipTextBox = (TextBox)locationSearchPanel.Items[1].Items[0].FindControl("ZipTextBox");
            //Label MessageLabel = (Label)SearchPanel.Items[0].Items[0].FindControl("MessageLabel");
            Panel RadiusTitlePanel = (Panel)locationSearchPanel.Items[1].Items[0].FindControl("RadiusTitlePanel");
            Panel RadiusDropPanel = (Panel)locationSearchPanel.Items[1].Items[0].FindControl("RadiusDropPanel");
            DropDownList RadiusDropDown = (DropDownList)locationSearchPanel.Items[1].Items[0].FindControl("RadiusDropDown");
            Telerik.Web.UI.RadTextBox CityTextBox = (Telerik.Web.UI.RadTextBox)locationSearchPanel.Items[0].Items[0].FindControl("CityTextBox");

            if (dvSearches[0]["Keywords"] != null)
                if (dvSearches[0]["Keywords"].ToString().Trim() != "")
                {
                    SearchTextBox.THE_TEXT = dvSearches[0]["Keywords"].ToString().Trim();
                }
            if (dvSearches[0]["Country"] != null)
                if (dvSearches[0]["Country"].ToString().Trim() != "")
                {
                    CountryDropDown.SelectedValue = dvSearches[0]["Country"].ToString().Trim();
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
                        StateDropDown.Items.FindItemByText(dvSearches[0]["State"].ToString().Trim()).Selected = true;
                    }
                    else
                    {
                        StateTextBoxPanel.Visible = true;
                        StateDropDownPanel.Visible = false;
                        StateTextBox.Text = dvSearches[0]["State"].ToString().Trim();
                    }
                }
            
            if (dvSearches[0]["City"] != null)
                if (dvSearches[0]["City"].ToString().Trim() != "")
                {
                    CityTextBox.Text = dvSearches[0]["City"].ToString().Trim();
                }

            bool isZip = false;

            if (dvSearches[0]["Zip"] != null)
                if (dvSearches[0]["Zip"].ToString().Trim() != "")
                {
                    char[] delim = { ';' };
                    string[] tokens = dvSearches[0]["Zip"].ToString().Trim().Split(delim);

                    ZipTextBox.Text = tokens[1].Trim();

                    isZip = true;

                    locationSearchPanel.Items[0].Expanded = false;
                    locationSearchPanel.Items[1].Expanded = true;
                }

            if (!isZip)
            {
                locationSearchPanel.Items[0].Expanded = true;
                locationSearchPanel.Items[1].Expanded = false;
            }

            if (dvSearches[0]["Radius"] != null)
                if (dvSearches[0]["Radius"].ToString().Trim() != "")
                {
                    RadiusDropDown.SelectedValue = dvSearches[0]["Radius"].ToString().Trim();
                }

            if (dvCates.Count > 0)
            {
                
                FillCategories(dsCats, ref CategoryTree);
                FillCategories(dsCats, ref RadTreeView1);
              
            }

            DoSearch();
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

    protected void Page_Init(object sender, EventArgs e)
    {
            //Literal lit = new Literal();
            //lit.Text = "<style type=\"text/css\">.RadComboBox_Black, .RadComboBox_Black " +
            //    ".rcbInputCell .rcbInput, .RadComboBoxDropDown_Black{border: 1px solid #D76012;}"+
            //    "html body .RadInput_Black .riTextBox, html body .RadInputMgr_Black{border: 1px solid #D76012 !important;}</style>";

            //Page.Header.Controls.Add(lit);

        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.Date.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        MessageRadWindowManager.VisibleOnPageLoad = false;
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        //Button button = (Button)dat.FindControlRecursive(this, "AdsLink");
        //button.CssClass = "NavBarImageAdSelected";

        //CheckBoxList CategoriesCheckBoxes = (CheckBoxList)dat.FindControlRecursive(this, "CategoriesCheckBoxes");
        

        //DataSet ds = dat.GetData("SELECT * FROM Categories");
        //CategoriesCheckBoxes.DataSource = ds;
        //CategoriesCheckBoxes.DataTextField = "CategoryName";
        //CategoriesCheckBoxes.DataValueField = "ID";
        //CategoriesCheckBoxes.DataBind();


        string cookieName = FormsAuthentication.FormsCookieName;
        HttpCookie authCookie = Context.Request.Cookies[cookieName];

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
                //Ads1.DATA_SET = ds2;
                //Ads1.MAIN_AD_DATA_SET = dat.RetrieveMainAds(Session["User"].ToString());
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

    protected void GoToLogin(object sender, EventArgs e)
    {
        Session["RedirectTo"] = "User.aspx?p=t#pfs";
        Response.Redirect("User.aspx?p=t#pfs");
    }

    protected void SaveSearch(object sender, EventArgs e)
    {
        string message = "";
        Encryption encrypt = new Encryption();
        if (Session["SavedClicked"] == null)
        {
            HttpCookie cookie = Request.Cookies["BrowserDate"];
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            
            SqlConnection conn = dat.GET_CONNECTED;
            //Label MessageLabel = (Label)SearchPanel.Items[0].Items[0].FindControl("MessageLabel");
            //Label Label1 = (Label)SearchPanel.Items[0].Items[0].FindControl("Label1");
            if (Session["User"] == null)
            {
                Session["RedirectTo"] = "AdSearch.aspx";

                message = "You must be logged in to do this. We'll take you to the login screen.";

                MessageRadWindow.NavigateUrl = "Message.aspx?message=" + encrypt.encrypt(message + "<br/><br/><br/><button onclick=\"Search('UserLogin.aspx');\" style=\" cursor: pointer;font-weight: bold;padding:0;padding-bottom: 4px; font-size: 11px;height: 30px; width: 112px;background-color: transparent; color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat:no-repeat; border: 0;\"" +
                                        "onmouseout=\"this.style.backgroundImage='url(image/PostButtonNoPost.png)'\" onmouseover=\"this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'\" >OK</button><button onclick=\"Search();\" style=\" cursor: pointer;font-weight: bold;padding:0;padding-bottom: 4px; font-size: 11px;height: 30px; width: 112px;background-color: transparent; color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat:no-repeat; border: 0;\"" +
                                        "onmouseout=\"this.style.backgroundImage='url(image/PostButtonNoPost.png)'\" onmouseover=\"this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'\" >CANCEL</button>");
                MessageRadWindow.Visible = true;
                MessageRadWindowManager.VisibleOnPageLoad = true;
                //MessageLabel.Text = message;
                //SearchPanel.Items[1].Expanded = false;
                //SearchPanel.Items[0].Expanded = true;
            }
            else
            {
                //ASP.controls_adsearchelements_ascx SearchElements2 = (ASP.controls_adsearchelements_ascx)SearchPanel.Items[1].Items[0].FindControl("SearchElements2");

                //Telerik.Web.UI.RadPanelBar locationSearchPanel = (Telerik.Web.UI.RadPanelBar)SearchPanel.Items[0].Items[0].FindControl("locationSearchPanel");

                //Telerik.Web.UI.RadComboBox CountryDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("CountryDropDown");
                Telerik.Web.UI.RadComboBox StateDropDown = (Telerik.Web.UI.RadComboBox)locationSearchPanel.Items[0].Items[0].FindControl("StateDropDown");
                Panel StateDropDownPanel = (Panel)locationSearchPanel.Items[0].Items[0].FindControl("StateDropDownPanel");
                Panel StateTextBoxPanel = (Panel)locationSearchPanel.Items[0].Items[0].FindControl("StateTextBoxPanel");
                //ASP.controls_hippotextbox_ascx SearchTextBox = (ASP.controls_hippotextbox_ascx)SearchPanel.Items[0].Items[0].FindControl("SearchTextBox");
                Telerik.Web.UI.RadTextBox StateTextBox = (Telerik.Web.UI.RadTextBox)locationSearchPanel.Items[0].Items[0].FindControl("StateTextBox");

                //Telerik.Web.UI.RadTreeView CategoryTree = (Telerik.Web.UI.RadTreeView)SearchPanel.Items[0].Items[0].FindControl("CategoryTree");
                //Telerik.Web.UI.RadTreeView RadTreeView1 = (Telerik.Web.UI.RadTreeView)SearchPanel.Items[0].Items[0].FindControl("RadTreeView1");
                TextBox ZipTextBox = (TextBox)locationSearchPanel.Items[1].Items[0].FindControl("ZipTextBox");
                Panel RadiusTitlePanel = (Panel)locationSearchPanel.Items[1].Items[0].FindControl("RadiusTitlePanel");
                Panel RadiusDropPanel = (Panel)locationSearchPanel.Items[1].Items[0].FindControl("RadiusDropPanel");
                DropDownList RadiusDropDown = (DropDownList)locationSearchPanel.Items[1].Items[0].FindControl("RadiusDropDown");
                Telerik.Web.UI.RadTextBox CityTextBox = (Telerik.Web.UI.RadTextBox)locationSearchPanel.Items[0].Items[0].FindControl("CityTextBox");
                string country = " AND A.CatCountry=" + CountryDropDown.SelectedValue;

                string city = "";
                string cityParam = "";
                string zip = "";
                string zipParameter = "";
                string state = "";
                string stateParam = "";
                int zipParam = 0;

                bool goOn = true;
                
                bool isZip = false;

                if (locationSearchPanel.Items[1].Expanded)
                {
                    isZip = true;
                }

                if (isZip)
                {
                    if (ZipTextBox.Text.Trim() != "")
                    {

                        //do only if United States and not for international zip codes
                        if (RadiusDropPanel.Visible)
                        {
                            //Get all zips within the specified radius

                            if (int.TryParse(ZipTextBox.Text.Trim(), out zipParam))
                            {
                                if (RadiusDropDown.SelectedValue == "0")
                                    zip = ";" + zipParam.ToString() + ";";
                                else
                                    zip = GetAllZipsInRadius(true);
                            }
                            else
                            {
                                goOn = false;
                                message = "Zip code is not in the right format.";
                            }
                        }
                        else
                        {
                            zip = " AND A.Zip = @zip ";
                            zipParameter = ZipTextBox.Text.Trim();
                        }
                    }
                    else
                    {
                        goOn = false;
                        message = "You must at the very least include a zip code.";
                    }
                }
                else
                {



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
                            message = "You must at the very least include a zip code or State and City.";
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
                            message = "You must at the very least include a zip code or State and City.";
                        }
                    }

                    if (CityTextBox.Text != "")
                    {
                        city = " AND A.CatCity=@city ";
                        cityParam = CityTextBox.Text.ToLower();
                    }
                    else
                    {
                        goOn = false;
                        message = "You must at the very least include a zip code or State and City.";
                    }
                }

                if (goOn)
                {
                    string radius = "";

                    if (RadiusDropPanel.Visible)
                        radius = RadiusDropDown.SelectedValue;
                    else
                        radius = "NULL";

                    string command = "INSERT INTO SavedAdSearches (Keywords, State, Country, UserID, Live, City, Zip, Radius) " +
                        "VALUES('" + SearchTextBox.THE_TEXT.Trim().Replace("'", "''") + "','" + stateParam.Replace("'", "''") +
                        "', " + CountryDropDown.SelectedItem.Value + ", '" + Session["User"].ToString() + "', 1, '" +
                        cityParam.Replace("'", "''") + "', '" + zip + "', " + radius + ")";
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
                    
                    message = "Search Saved";
                    MessageRadWindow.NavigateUrl = "Message.aspx?message=" + encrypt.encrypt(message + "<br/><br/><savedadsearch>ID:" + ID2 + "</savedadsearch><br/><button onclick=\"Search('AdSearch.aspx');\" style=\" cursor: pointer;font-weight: bold;padding:0;padding-bottom: 4px; font-size: 11px;height: 30px; width: 112px;background-color: transparent; color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat:no-repeat; border: 0;\"" +
                                            "onmouseout=\"this.style.backgroundImage='url(image/PostButtonNoPost.png)'\" onmouseover=\"this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'\" >OK</button><button onclick=\"Search('SearchesAndPages.aspx');\" style=\" cursor: pointer;font-weight: bold;padding:0;padding-bottom: 4px; font-size: 11px;height: 30px; width: 112px;background-color: transparent; color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat:no-repeat; border: 0;\"" +
                                            "onmouseout=\"this.style.backgroundImage='url(image/PostButtonNoPost.png)'\" onmouseover=\"this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'\" >go to searches</button>");
                    MessageRadWindow.Visible = true;
                    MessageRadWindowManager.VisibleOnPageLoad = true;
                }
                else
                {
                    MessageLabel.Text = message;
                    Label1.Text = message;
                    //SearchPanel.Items[1].Expanded = false;
                    //SearchPanel.Items[0].Expanded = true;
                }
            }
        }
        else
        {
            message = "Search Saved";
            MessageRadWindow.NavigateUrl = "Message.aspx?message=" + encrypt.encrypt(message + "<br/><br/><savedadsearch>ID:" + Session["SavedSearchID"].ToString() + "</savedadsearch><br/><button onclick=\"Search('AdSearch.aspx');\" style=\" cursor: pointer;font-weight: bold;padding:0;padding-bottom: 4px; font-size: 11px;height: 30px; width: 112px;background-color: transparent; color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat:no-repeat; border: 0;\"" +
                                    "onmouseout=\"this.style.backgroundImage='url(image/PostButtonNoPost.png)'\" onmouseover=\"this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'\" >OK</button><button onclick=\"Search('SearchesAndPages.aspx');\" style=\" cursor: pointer;font-weight: bold;padding:0;padding-bottom: 4px; font-size: 11px;height: 30px; width: 112px;background-color: transparent; color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat:no-repeat; border: 0;\"" +
                                    "onmouseout=\"this.style.backgroundImage='url(image/PostButtonNoPost.png)'\" onmouseover=\"this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'\" >go to searches</button>");
            MessageRadWindow.Visible = true;
            MessageRadWindowManager.VisibleOnPageLoad = true;
        }
    }

    protected void SaveSearchCategories(string ID, ref Telerik.Web.UI.RadTreeView CategoryTree)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        List<Telerik.Web.UI.RadTreeNode> list = (List<Telerik.Web.UI.RadTreeNode>)CategoryTree.GetAllNodes();
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        foreach(Telerik.Web.UI.RadTreeNode node in list)
        {
            if (node.Checked)
            {
                //SELECT [ID],[SearchID],[CategoryID] from [HippoHappenings].[dbo].[SavedAdSearches_Categories] 
                dat.Execute("INSERT INTO SavedAdSearches_Categories (SearchID, CategoryID) VALUES("+ID+", "+node.Value+")");
            }
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
        DoSearch();


    }

    protected string GetAllZipsInRadius(bool justString)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        //Telerik.Web.UI.RadPanelBar locationSearchPanel = (Telerik.Web.UI.RadPanelBar)SearchPanel.Items[0].Items[0].FindControl("locationSearchPanel");
        TextBox ZipTextBox = (TextBox)locationSearchPanel.Items[1].Items[0].FindControl("ZipTextBox");
        Panel RadiusDropPanel = (Panel)locationSearchPanel.Items[1].Items[0].FindControl("RadiusDropPanel");
        DropDownList RadiusDropDown = (DropDownList)locationSearchPanel.Items[1].Items[0].FindControl("RadiusDropDown");

        //do only if United States and not for international zip codes
        string zip = "";
        string nonExistantZip = "";
        if (ZipTextBox.Text.Trim() != "")
        {
            if (RadiusDropPanel.Visible)
            {
                //Get all zips within the specified radius
                int zipParam = 0;
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
                        zip = " AND A.CatZip LIKE '%;" + zipParam.ToString() + ";%' ";
                        nonExistantZip = " OR A.CatZip LIKE '%;" + zipParam.ToString() + ";%'";
                        if(justString)
                        {
                            zip = ";"+zipParam+";";
                        }
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


                            if (justString)
                            {
                                if (zip.Trim() == "")
                                    zip += ";";
                                zip += zipParam.ToString() + ";";
                            }
                            else
                                zip = " AND (A.CatZip LIKE '%;" + zipParam.ToString() + ";%' " + nonExistantZip;

                            for (int i = 0; i < dsZips.Tables[0].Rows.Count; i++)
                            {
                                if (justString)
                                    zip += dsZips.Tables[0].Rows[i]["Zip"].ToString() + ";";
                                else
                                    zip += " OR A.CatZip LIKE '%;" + dsZips.Tables[0].Rows[i]["Zip"].ToString() + ";%' ";
                            }
                            if(!justString)
                                zip += ") ";
                        }
                        else
                        {
                            if(!justString)
                                zip = " AND A.CatZip LIKE '%;" + ZipTextBox.Text.Trim() + ";%'";
                        }
                    }
                    else
                    {
                        if(!justString)
                            zip = " AND A.CatZip LIKE '%;" + ZipTextBox.Text.Trim() + ";%'";
                    }


                }
                else
                {
                    if (justString)
                    {
                        zip = ";" + ZipTextBox.Text.Trim() + ";";
                    }
                    else
                        zip = " AND A.CatZip LIKE '%;" + ZipTextBox.Text.Trim() + ";%' ";
                }
            }
            else
            {
                if (justString)
                {
                    zip = ";" + ZipTextBox.Text.Trim() + ";";
                }
                else
                    zip = " AND A.CatZip LIKE '%;" + ZipTextBox.Text.Trim() + ";%' ";
            }
        }
        else
        {
            zip = null;
        }

        return zip;
    }

    protected void DoSearch()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        //ASP.controls_adsearchelements_ascx SearchElements2 = (ASP.controls_adsearchelements_ascx)SearchPanel.Items[1].Items[0].FindControl("SearchElements2");
        //Label MessageLabel = (Label)SearchPanel.Items[0].Items[0].FindControl("MessageLabel");
        try
        {
            //SearchElements2.Clear();
            //Telerik.Web.UI.RadPanelBar locationSearchPanel = (Telerik.Web.UI.RadPanelBar)SearchPanel.Items[0].Items[0].FindControl("locationSearchPanel");

            Telerik.Web.UI.RadTextBox StateTextBox = (Telerik.Web.UI.RadTextBox)locationSearchPanel.Items[0].Items[0].FindControl("StateTextBox");
            //Telerik.Web.UI.RadComboBox CountryDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("CountryDropDown");
            Telerik.Web.UI.RadComboBox StateDropDown = (Telerik.Web.UI.RadComboBox)locationSearchPanel.Items[0].Items[0].FindControl("StateDropDown");
            Telerik.Web.UI.RadTextBox CityTextBox = (Telerik.Web.UI.RadTextBox)locationSearchPanel.Items[0].Items[0].FindControl("CityTextBox");
            //ASP.controls_hippotextbox_ascx SearchTextBox = (ASP.controls_hippotextbox_ascx)SearchPanel.Items[0].Items[0].FindControl("SearchTextBox");
            Panel StateDropDownPanel = (Panel)locationSearchPanel.Items[0].Items[0].FindControl("StateDropDownPanel");
            Panel StateTextBoxPanel = (Panel)locationSearchPanel.Items[0].Items[0].FindControl("StateTextBoxPanel");
            //Telerik.Web.UI.RadTreeView CategoryTree = (Telerik.Web.UI.RadTreeView)SearchPanel.Items[0].Items[0].FindControl("CategoryTree");
            //Telerik.Web.UI.RadTreeView RadTreeView1 = (Telerik.Web.UI.RadTreeView)SearchPanel.Items[0].Items[0].FindControl("RadTreeView1");

            TextBox ZipTextBox = (TextBox)locationSearchPanel.Items[1].Items[0].FindControl("ZipTextBox");
            DropDownList RadiusDropDown = (DropDownList)locationSearchPanel.Items[1].Items[0].FindControl("RadiusDropDown");
            Panel RadiusTitlePanel = (Panel)locationSearchPanel.Items[1].Items[0].FindControl("RadiusTitlePanel");
            Panel RadiusDropPanel = (Panel)locationSearchPanel.Items[1].Items[0].FindControl("RadiusDropPanel");
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));


            char[] delim = { ' ' };
            string[] tokens;

            SearchTextBox.THE_TEXT = dat.stripHTML(SearchTextBox.THE_TEXT);

            string resultsStr = "";

            tokens = SearchTextBox.THE_TEXT.Split(delim);
            string temp = "";
            if (SearchTextBox.THE_TEXT.Trim() != "")
            {
                for (int i = 0; i < tokens.Length; i++)
                {
                    temp += " (A.Description LIKE @search" + i.ToString() +
                        " OR A.Header LIKE @search" + i.ToString() + ") ";

                    if (i + 1 != tokens.Length)
                        temp += " OR ";
                }
                resultsStr += "'" + SearchTextBox.THE_TEXT + "' ";
            }
            if (temp != "")
                temp = " AND " + temp;
            string country = " AND A.CatCountry=" + CountryDropDown.SelectedValue;

            string city = "";
            string cityParam = "";
            string zip = "";
            string zipParameter = "";
            string state = "";
            string stateParam = "";
            int zipParam = 0;

            bool goOn = true;
            string message = "";
            bool isZip = false;

            if (locationSearchPanel.Items[1].Expanded && ZipTextBox.Text.Trim() != "")
            {
                isZip = true;
            }
            else
            {
                if (CityTextBox.Text.Trim() == "" && ZipTextBox.Text.Trim() != "")
                    isZip = true;
            }

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
                            zip = GetAllZipsInRadius(false);   
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
                    message = "You must at the very least include a zip code.";
                }
            }
            else
            {



                if (StateDropDownPanel.Visible)
                {
                    if (StateDropDown.SelectedValue != "-1")
                    {
                        state = " AND A.CatState=@state ";
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
                        state = " AND A.CatState=@state ";
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
                    city = " AND A.CatCity=@city ";
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

                string categories = "";
                string addCat = "";
                for (int i = 0; i < CategoryTree.Nodes.Count; i++)
                {
                    if (CategoryTree.Nodes[i].Checked)
                    {
                        if (categories == "")
                        {
                            addCat = ", Ad_Category_Mapping ACM ";
                            categories += " AND A.Ad_ID=ACM.AdID AND (";
                            categories += " ACM.CategoryID = " + CategoryTree.Nodes[i].Value;
                        }
                        else
                            categories += " OR ACM.CategoryID = " + CategoryTree.Nodes[i].Value;

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
                                categories += " OR ACM.CategoryID = " + CategoryTree.Nodes[i].Nodes[h].Value;

                                if (CategoryTree.Nodes[i].Nodes[h].Nodes.Count > 0)
                                {
                                    for (int k = 0; k < CategoryTree.Nodes[i].Nodes[h].Nodes.Count; k++)
                                    {
                                        categories += " OR ACM.CategoryID = " + CategoryTree.Nodes[i].Nodes[h].Nodes[k].Value;
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
                                addCat = ", Ad_Category_Mapping ACM ";
                                categories += " AND A.Ad_ID=ACM.AdID AND (";
                                categories += " ACM.CategoryID = " + CategoryTree.Nodes[i].Nodes[n].Value;
                            }
                            else
                                categories += " OR ACM.CategoryID = " + CategoryTree.Nodes[i].Nodes[n].Value;

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
                            addCat = ", Ad_Category_Mapping ACM ";
                            categories += " AND A.Ad_ID=ACM.AdID AND (";
                            categories += " ACM.CategoryID = " + RadTreeView1.Nodes[i].Value;
                        }
                        else
                            categories += " OR ACM.CategoryID = " + RadTreeView1.Nodes[i].Value;

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
                                categories += " OR ACM.CategoryID = " + RadTreeView1.Nodes[i].Nodes[h].Value;

                                if (RadTreeView1.Nodes[i].Nodes[h].Nodes.Count > 0)
                                {
                                    for (int k = 0; k < RadTreeView1.Nodes[i].Nodes[h].Nodes.Count; k++)
                                    {
                                        categories += " OR ACM.CategoryID = " + RadTreeView1.Nodes[i].Nodes[h].Nodes[k].Value;
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
                                addCat = ", Ad_Category_Mapping ACM ";
                                categories += " AND A.Ad_ID=ACM.AdID AND (";
                                categories += " ACM.CategoryID = " + RadTreeView1.Nodes[i].Nodes[n].Value;
                            }
                            else
                                categories += " OR ACM.CategoryID = " + RadTreeView1.Nodes[i].Nodes[n].Value;

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

                string searchStr = "SELECT DISTINCT A.Ad_ID, A.Header, A.DateAdded FROM Ads A " + addCat +
                    ", Ad_Calendar AC WHERE AC.AdID=A.Ad_ID AND " +
                    "((A.Featured = 'False' AND AC.DateTimeStart <= CONVERT(DATETIME,'" +
                    DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")) +
                    "') AND AC.DateTimeEnd > CONVERT(DATETIME,'" +
                    DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")) +
                    "')) OR (A.Featured='True' AND A.NumCurrentViews < A.NumViews AND AC.DateTimeStart <= CONVERT(DATETIME,'" +
                    DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")) +
                    "'))) AND A.Live='True' " +
                    temp + country + state + city + zip + categories + " ORDER BY A.DateAdded ";
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());

                if (conn.State != ConnectionState.Open)
                    conn.Open();
                SqlCommand cmd = new SqlCommand(searchStr, conn);
                for (int i = 0; i < tokens.Length; i++)
                {
                    cmd.Parameters.Add("@search" + i.ToString(), SqlDbType.NVarChar).Value = "%" + tokens[i] + "%";
                }
                if (state != "")
                    cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = stateParam;
                if (city != "")
                    cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = cityParam;
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                conn.Close();

                bool setZero = false;
                //SearchPanel.Items[0].Expanded = false;
                //SearchPanel.Items[1].Expanded = true;
                if (ds.Tables.Count > 0)
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //SearchElements2.AD_DS = ds;
                        //SearchElements2.DataBind2();
                        Session["SearchDS"] = ds;
                        Session["Searching"] = "Ads";
                        Session["SessionSearch"] = ds;
                        Session["SearchResults"] = "Results " + resultsStr;

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

                if (setZero)
                {
                    //SearchPanel.Items[1].Text = "<div style='position: relative;'><span style='font-family: Arial; font-size: 20px; color: White;'>Search Results: </span><div style='position: absolute; right: 5px; top: 3px;'>(0 Records Found)</div></div>";
                    //SortDropDown.Visible = false;
                    Session["SearchDS"] = null;
                    RadWindow RadWindow3 = Master.RAD_WINDOW_3;
                    RadWindow3.Top = 10;
                    RadWindow3.Left = 765;
                    RadWindow3.Width = 480;
                    RadWindow3.Height = 620;
                    RadWindow3.VisibleOnPageLoad = true;
                }
                else
                {
                    //SortDropDown.Visible = true;
                    RadWindow RadWindow3 = Master.RAD_WINDOW_3;
                    RadWindow3.Top = 10;
                    RadWindow3.Left = 765;
                    RadWindow3.Width = 480;
                    RadWindow3.Height = 620;
                    RadWindow3.VisibleOnPageLoad = true;

                }
                //Label ResultsLabel = (Label)SearchPanel.Items[1].Items[0].FindControl("ResultsLabel");

                //ResultsLabel.Text = "Results " + resultsStr;

                //Save search location base on IP if user not logged in.

               
                if (Session["User"] == null)
                {
                    dat.SetLocationForIP(CountryDropDown.SelectedValue, cityParam, stateParam);
                }
            }
            else
            {
                MessageLabel.Text = message;
                //SearchPanel.Items[1].Expanded = false;
                //SearchPanel.Items[0].Expanded = true;
                //SearchButton.PostBackUrl = "~/VenueSearch.aspx#SearchTag";
            }
        }
        catch (Exception ex)
        {
            MessageLabel.Text = ex.ToString();
        }
    }

    //protected void SearchAdvanced(object sender, EventArgs e)
    //{
    //    SearchTextBox.THE_TEXT = "";
    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //    SearchElements.Clear();
    //    RadPanelBar1.CollapseAllItems();
    //    ASP.controls_hippotextbox_ascx adname = (ASP.controls_hippotextbox_ascx)dat.FindControlRecursive(this, "EventNameTextBox");

    //    string queryBegin = "SELECT DISTINCT * FROM Ads A ";
    //    string query = "";

    //    char[] delim = { ' ' };
    //    string[] tokens;
    //    ImageButton CheckImageButton = (ImageButton)dat.FindControlRecursive(this, "CheckImageButton");
    //    CheckBoxList CategoriesCheckBoxes = (CheckBoxList)dat.FindControlRecursive(this, "CategoriesCheckBoxes");
    //    bool wasAdded = false;
    //    if (CheckImageButton.ImageUrl == "image/CheckSelected.png")
    //    {
            
    //        for (int i = 0; i < CategoriesCheckBoxes.Items.Count; i++)
    //        {
               
    //            if (CategoriesCheckBoxes.Items[i].Selected)
    //            {
    //                if(!wasAdded)
    //                    queryBegin += ", Ad_Category_Mapping ACM WHERE A.Ad_ID=ACM.AdID ";
    //                if (query == "")
    //                {
    //                    query += " AND (";
    //                    query += " ACM.CategoryID = " + CategoriesCheckBoxes.Items[i].Value;
    //                }
    //                else
    //                    query += " OR ACM.CategoryID = " + CategoriesCheckBoxes.Items[i].Value;
    //            }


    //        }
    //        if (query != "")
    //            query += " ) ";

    //    }

    //    string temp = "";
    //    if (query == "")
    //        query = " WHERE A.Live='True' ";
    //    else
    //        query += " AND  A.Live='True' AND ";
    //    if (adname.THE_TEXT != "")
    //    {
    //        tokens = adname.THE_TEXT.Split(delim);
    //        temp = " ( ";
    //        for (int i = 0; i < tokens.Length; i++)
    //        {
    //            temp += " A.Header LIKE @p" + i.ToString();

    //            if (i + 1 != tokens.Length)
    //                temp += " OR ";
    //        }

    //        temp += " ) ";
    //        query += temp;
    //    }


    //    if (query != "")
    //    {
            

    //        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());

    //        if (conn.State != ConnectionState.Open)
    //            conn.Open();
    //        SqlCommand cmd = new SqlCommand(queryBegin + query, conn);
    //        tokens = adname.THE_TEXT.Split(delim);
    //        for (int i = 0; i < tokens.Length; i++)
    //        {
    //            cmd.Parameters.Add("@p" + i.ToString(), SqlDbType.NVarChar).Value = "%" + tokens[i] + "%";
    //        }
    //        DataSet ds = new DataSet();
    //        SqlDataAdapter da = new SqlDataAdapter(cmd);
    //        da.Fill(ds);
    //        conn.Close();

    //        ViewState["SearchDS"] = ds;

    //        if (ds.Tables.Count > 0)
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                SearchElements.AD_DS = ds;
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

    protected void ChangeState(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        //Telerik.Web.UI.RadPanelBar locationSearchPanel = (Telerik.Web.UI.RadPanelBar)SearchPanel.Items[0].Items[0].FindControl("locationSearchPanel");

        //Telerik.Web.UI.RadComboBox DateDropDown = (Telerik.Web.UI.RadComboBox)locationSearchPanel.Items[0].Items[0].FindControl("DateDropDown");


        //Telerik.Web.UI.RadComboBox CountryDropDown = (Telerik.Web.UI.RadComboBox)SearchPanel.Items[0].Items[0].FindControl("CountryDropDown");
        Telerik.Web.UI.RadComboBox StateDropDown = (Telerik.Web.UI.RadComboBox)locationSearchPanel.Items[0].Items[0].FindControl("StateDropDown");
        Panel StateDropDownPanel = (Panel)locationSearchPanel.Items[0].Items[0].FindControl("StateDropDownPanel");
        Panel StateTextBoxPanel = (Panel)locationSearchPanel.Items[0].Items[0].FindControl("StateTextBoxPanel");

        Panel RadiusTitlePanel = (Panel)locationSearchPanel.Items[1].Items[0].FindControl("RadiusTitlePanel");
        Panel RadiusDropPanel = (Panel)locationSearchPanel.Items[1].Items[0].FindControl("RadiusDropPanel");
        Panel USLabelPanel = (Panel)locationSearchPanel.Items[1].Items[0].FindControl("USLabelPanel"); 
        //ASP.controls_hippotextbox_ascx SearchTextBox = (ASP.controls_hippotextbox_ascx)SearchPanel.Items[1].FindControl("SearchTextBox");

        if (CountryDropDown.SelectedValue != "223")
        {
            RadiusTitlePanel.Visible = false;
            RadiusDropPanel.Visible = false;
            USLabelPanel.Visible = false;
        }
        else
        {
            RadiusTitlePanel.Visible = true;
            RadiusDropPanel.Visible = true;
            USLabelPanel.Visible = true;
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

    protected void GoTo(object sender, EventArgs e)
    {
        Response.Redirect("PostAnAd.aspx");
    }
}
