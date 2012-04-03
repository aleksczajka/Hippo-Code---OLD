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
using System.IO;
using System.Drawing.Imaging;

public partial class PostAnAd : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Request.IsSecureConnection)
        {
            string absoluteUri = Request.Url.AbsoluteUri;
            Response.Redirect(absoluteUri.Replace("http://", "https://"), true);
        }

        MessagePanel.Visible = false;
        YourMessagesLabel.Text = "";

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

            #region Take care of buttons
            DetailsOnwardsButton.SERVER_CLICK += Onwards;
            //MusicUploadButton.SERVER_CLICK += MusicUpload_Click;
            //DeleteSongButton.SERVER_CLICK += NixIt;
            BlueButton3.SERVER_CLICK += YouTubeUpload_Click;
            PictureNixItButton.SERVER_CLICK += PictureNixIt;
            MediaOnwardsButton.SERVER_CLICK += Onwards;
            BlueButton4.SERVER_CLICK += Backwards;
            ImageButton11.SERVER_CLICK += SuggestCategoryClick;
            BlueButton5.SERVER_CLICK += Onwards;
            BlueButton6.SERVER_CLICK += Backwards;
            BlueButton15.SERVER_CLICK += Onwards;
            BlueButton16.SERVER_CLICK += Backwards;
            BlueButton17.SERVER_CLICK += Onwards;
            BlueButton18.SERVER_CLICK += Backwards;
            BlueButton17.SERVER_CLICK += PostIt;
            BlueButton18.SERVER_CLICK += Backwards;
            BlueButton9.SERVER_CLICK += CheckedFeatured;
            BlueButton10.SERVER_CLICK += UnCheckedFeatured;
            BlueButton7.SERVER_CLICK += AddFeaturedDates;
            BlueButton11.SERVER_CLICK += RemoveFeaturedDates;
            BlueButton12.SERVER_CLICK += AddSearchTerm;
            BlueButton13.SERVER_CLICK += RemoveSearchTerm;
            BlueButton14.SERVER_CLICK += FillChart;
            ImageButton5.SERVER_CLICK += AdPictureUpload_Click;
            AdNixItButton.SERVER_CLICK += AdPictureNixIt;
            BlueButton1.SERVER_CLICK += ValidateCode;
            #endregion

            

            Session["RedirectTo"] = Request.Url.AbsoluteUri;
            if (!IsPostBack)
            {
                Session["UserAvailableDate"] = null;
                Session.Remove("UserAvailableDate");
                Session["RedirectTo"] = "post-bulletin";
                Session["categorySession"] = null;
                Session.Remove("categorySession");
            }

            DateTime isn = DateTime.Now;

            if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
                isn = DateTime.Now;
            DateTime isNow = isn;
            Data dat = new Data(isn);
            DataView dv = dat.GetDataDV("SELECT * FROM TermsAndConditions");
            Literal lit1 = new Literal();
            lit1.Text = "<div style=\"padding-bottom: 20px;\">" + dv[0]["Content"].ToString() + "</div>";
            TACTextBox.Controls.Add(lit1);

            if (Session["User"] != null)
            {
                LoggedInPanel.Visible = true;
            }
            else
            {
                Response.Redirect("~/login");
                WelcomeLabel.Text = "<h1 class=\"SideColumn\">You Need To Be <a class=\"NavyLink12\" href=\"login\">Logged In</a> to Post A Bulletin.</h1><br/>HippoHappenings's delivers content for users by users. " +
                "You have all the power to post events, bulletins, trips and venues. " +
                "Users are responsible for all posted content, so mind " +
                "what you post. Everyone has the power to flag content deemed expressly " +
                "offensive, illegal or corporate in nature. In order to make this possible, " +
                "and for us to maintain clean and manageable content, we require that you " +
                "<a class=\"AddLink\" href=\"Register.aspx\">create an account</a> with us. If you already have an account, <a class=\"AddLink\" href=\"login\">login</a> and post your bulletin!";
            }

            try
            {
                if (!IsPostBack)
                {
                    SummaryTextBox.Attributes.Add("onkeyup", "CountChars()");
                    HtmlLink lk = new HtmlLink();
                    HtmlHead head = (HtmlHead)Page.Header;
                    lk.Attributes.Add("rel", "canonical");
                    lk.Href = "https://hippohappenings.com/post-bulletin";
                    head.Controls.AddAt(0, lk);

                    HtmlMeta kw = new HtmlMeta();
                    kw.Name = "keywords";
                    kw.Content = "post local bulletin in your neighborhood for your community interest groups";

                    head.Controls.AddAt(0, kw);

                    HtmlMeta hm = new HtmlMeta();
                    hm.Name = "description";
                    hm.Content = "Post local bulletin in your neighborhood which will be posted in your community and interest groups";

                    head.Controls.AddAt(0, hm);

                    lk = new HtmlLink();
                    lk.Href = "http://" + Request.Url.Authority + "/post-bulletin";
                    lk.Attributes.Add("rel", "bookmark");
                    head.Controls.AddAt(0, lk);

                    DataSet dsCountry = dat.GetData("SELECT * FROM Countries");
                    CountryDropDown.DataSource = dsCountry;
                    CountryDropDown.DataTextField = "country_name";
                    CountryDropDown.DataValueField = "country_id";
                    CountryDropDown.DataBind();

                    CountryDropDown.SelectedValue = "223";

                    //Take Care of Billing Locaion
                    BillingCountry.DataSource = dsCountry;
                    BillingCountry.DataTextField = "country_name";
                    BillingCountry.DataValueField = "country_id";
                    BillingCountry.DataBind();

                    BillingCountry.SelectedValue = "223";
                    BillingStateTextBox.Text = "AL";
                    CardTypeDropDown.Items.Add(new ListItem("Visa", "Visa"));
                    CardTypeDropDown.Items.Add(new ListItem("MasterCard", "MasterCard"));
                    CardTypeDropDown.Items.Add(new ListItem("Discover", "Discover"));
                    CardTypeDropDown.Items.Add(new ListItem("American Express", "Amex"));
                    ChangeStateAction(BillingCountry, new EventArgs());

                    DataSet ds = dat.GetData("SELECT * FROM State WHERE country_id=223");

                    StateDropDownPanel.Visible = true;
                    StateTextBoxPanel.Visible = false;
                    StateDropDown.DataSource = ds;
                    StateDropDown.DataTextField = "state_2_code";
                    StateDropDown.DataValueField = "state_id";
                    StateDropDown.DataBind();

                    ChangeCity(StateDropDown, new EventArgs());
                }


                if (Session["User"] != null)
                {
                    LoggedInPanel.Visible = true;
                    LogInPanel.Visible = false;
                    if (Request.QueryString["edit"] != null)
                    {
                        if (bool.Parse(Request.QueryString["edit"].ToString()))
                        {
                            string IDi = Request.QueryString["ID"].ToString();

                            DataSet dsAd = dat.GetData("SELECT * FROM Ads WHERE Ad_ID=" + Request.QueryString["ID"].ToString());

                            if (Session["User"].ToString() == dsAd.Tables[0].Rows[0]["User_ID"].ToString())
                            {
                                isEdit.Text = "True";
                                adID.Text = IDi;
                                if (!IsPostBack)
                                {
                                    fillAd(IDi, true);
                                }

                                //IntroPanel.Visible = false;
                                TabsPanel.Visible = true;
                            }
                            else
                            {
                                Response.Redirect("home");
                            }



                        }
                        else
                        {
                            Response.Redirect("home");
                        }
                    }
                    else if (Request.QueryString["copy"] != null)
                    {
                        if (bool.Parse(Request.QueryString["copy"].ToString()))
                        {
                            string IDi = Request.QueryString["ID"].ToString();
                            DataSet dsAd = dat.GetData("SELECT * FROM Ads WHERE Ad_ID=" + Request.QueryString["ID"].ToString());
                            if (Session["User"].ToString() == dsAd.Tables[0].Rows[0]["User_ID"].ToString())
                            {
                                if (!IsPostBack)
                                {
                                    fillAd(IDi, false);
                                }

                                //IntroPanel.Visible = false;
                                TabsPanel.Visible = true;
                            }
                            else
                            {
                                Response.Redirect("home");
                            }

                        }
                    }
                }
                else
                {
                    LoggedInPanel.Visible = false;
                    LogInPanel.Visible = true;
                }
            }
            catch (Exception ex)
            {
                YourMessagesLabel.Text = ex.ToString();
                MessagePanel.Visible = true;
            }

            if (!IsPostBack)
            {
                Session.Remove("AdCategoriesSet");
                Session["AdCategoriesSet"] = null;

                ShowVideoPictureLiteral.Text = "";
            }

            #region Take Care of Feature Features
            //Limit feature calendar to 30 days
            FeaturedDatePicker.MinDate = DateTime.Now;
            FeaturedDatePicker.MaxDate = DateTime.Now.AddDays(30);

            foreach (ListItem item2 in FeatureDatesListBox.Items)
            {
                if (item2.Value == "Disabled")
                    item2.Attributes.Add("class", "DisabledListItem");
            }

            if (isEdit.Text.Trim() != "")
            {
                if (bool.Parse(isEdit.Text))
                {
                    DataSet dsAd2 = dat.GetData("SELECT * FROM Ads WHERE Ad_ID=" + Request.QueryString["ID"].ToString());

                    DateTime dateAdded = DateTime.Parse(dsAd2.Tables[0].Rows[0]["DateAdded"].ToString());
                    int daysPastI = daysPast(dateAdded);
                    DateTime dtNew = DateTime.Now.AddDays(30 - daysPastI);
                    if (dtNew < DateTime.Now)
                        FeaturedDatePicker.MaxDate = DateTime.Now;
                    else
                        FeaturedDatePicker.MaxDate = dtNew;
                }
            }
            #endregion
        }
        catch (Exception ex)
        {
            YourMessagesLabel.Text = ex.ToString();
            MessagePanel.Visible = true;
        }
    }

    protected int daysPast(DateTime oldDate)
    {
        long tickDiff = DateTime.Now.Ticks - oldDate.Ticks;
        tickDiff = tickDiff / 10000000; //have seconds now
        int _age = (int)(tickDiff / 86400);
        return _age;//should be days
    }

    protected void fillAd(string ID, bool isedit)
    {
        try
        {
            bool iscopy = true;
            if (Request.QueryString["copy"] == null)
                iscopy = false;
            else if (!bool.Parse(Request.QueryString["copy"].ToString()))
                iscopy = false;

            HttpCookie cookie = Request.Cookies["BrowserDate"];

            DateTime isn = DateTime.Now;

            if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
                isn = DateTime.Now;
            DateTime isNow = isn;
            Data dat = new Data(isn); DataSet dsEvent = dat.GetData("SELECT * FROM Ads WHERE Ad_ID=" + ID);
            AdNameTextBox.Text = dsEvent.Tables[0].Rows[0]["Header"].ToString();
            nameLabel.Text = "You are editing the bulletin: " + dsEvent.Tables[0].Rows[0]["Header"].ToString(); ;
            nameLabel.Visible = false;
            //Session["EffectiveUserName"] = Session["UserName"].ToString();

            DataSet dsCountry = dat.GetData("SELECT * FROM State WHERE country_id=" + dsEvent.Tables[0].Rows[0]["CatCountry"].ToString());           

            CountryDropDown.ClearSelection();
            CountryDropDown.SelectedValue = dsEvent.Tables[0].Rows[0]["CatCountry"].ToString();
            ChangeState(new object(), new EventArgs());
            bool isTextBox = false;

            if (dsEvent.Tables[0].Rows[0]["CatState"] != null)
            {

                if (StateDropDownPanel.Visible)
                {
                    StateDropDown.ClearSelection();
                    StateDropDown.Items.FindByText(dsEvent.Tables[0].Rows[0]["CatState"].ToString()).Selected = true;
                }
                else
                    StateTextBox.Text = dsEvent.Tables[0].Rows[0]["CatState"].ToString();

                ChangeCity(StateDropDown, new EventArgs());
            }

            if (dsEvent.Tables[0].Rows[0]["CatCountry"].ToString() == "223")
            {
                if (dsEvent.Tables[0].Rows[0]["CatCity"] != null)
                {
                    MajorCityDrop.ClearSelection();
                    MajorCityDrop.Items.FindByText(dsEvent.Tables[0].Rows[0]["CatCity"].ToString()).Selected = true;
                }
            }
            else
            {
                if (dsEvent.Tables[0].Rows[0]["CatCity"] != null)
                {
                    CityTextBox.Text = dsEvent.Tables[0].Rows[0]["CatCity"].ToString();
                }
            }

            DescriptionTextBox.Content = dsEvent.Tables[0].Rows[0]["Description"].ToString();


            if (bool.Parse(dsEvent.Tables[0].Rows[0]["Featured"].ToString()))
            {
                SummaryTextBox.InnerHtml = dsEvent.Tables[0].Rows[0]["FeaturedSummary"].ToString();
               AdPictureCheckList.Items.Clear();
                if (dsEvent.Tables[0].Rows[0]["FeaturedPicture"] != null)
                {
                    if (dsEvent.Tables[0].Rows[0]["FeaturedPicture"].ToString().Trim() != "")
                    {
                        ListItem newItem = new ListItem(dsEvent.Tables[0].Rows[0]["FeaturedPictureName"].ToString(),
                            dsEvent.Tables[0].Rows[0]["FeaturedPicture"].ToString());
                        AdPictureCheckList.Items.Add(newItem);

                        AdNixItButton.Visible = true;

                        BannerAdCheckBox.Checked = true;
                        AdMediaPanel.Visible = true;
                        AdMediaPanel.Enabled = true;
                    }
                    else
                    {
                        AdPictureCheckList.Items.Clear();

                        AdNixItButton.Visible = false;
                        BannerAdCheckBox.Checked = false;
                    }
                }
                else
                {
                    AdPictureCheckList.Items.Clear();

                    AdNixItButton.Visible = false;
                    BannerAdCheckBox.Checked = false;
                }
            }

            PictureCheckList.Items.Clear();
            string mediaCategory = dsEvent.Tables[0].Rows[0]["mediaCategory"].ToString();
            string youtube = dsEvent.Tables[0].Rows[0]["YouTubeVideo"].ToString();
            switch (mediaCategory)
            {
                case "1":
                    MainAttractionCheck.Checked = true;
                    char[] delim4 = { ';' };
                    char[] delim = { '\\' };
                    string[] youtokens = youtube.Split(delim4);
                    if (youtokens.Length > 0)
                    {
                        for (int i = 0; i < youtokens.Length; i++)
                        {
                            if (youtokens[i].Trim() != "")
                            {
                                ListItem newListItem = new ListItem("You Tube ID: " + youtokens[i],
                                    youtokens[i]);

                                PictureCheckList.Items.Add(newListItem);
                            }
                        }
                    }
                    string[] fileArray = System.IO.Directory.GetFiles(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString()
                        + "\\AdSlider\\" + ID);
                    DataView dvA = dat.GetDataDV("SELECT * FROM Ad_Slider_Mapping WHERE AdID=" + ID);
                    for (int i = 0; i < dvA.Count; i++)
                    {
                        string[] fileTokens = fileArray[i].Split(delim);
                        string nameFile = dvA[i]["PictureName"].ToString();


                        ListItem newListItem = new ListItem(dvA[i]["RealPictureName"].ToString(),
                                nameFile);

                            PictureCheckList.Items.Add(newListItem);
                        
                    }
                    UploadedVideosAndPics.Visible = true;
                    MainAttractionPanel.Visible = true;
                    MainAttractionPanel.Enabled = true;
                    YouTubePanel.Enabled = true;
                    PicturePanel.Enabled = true;
                    PictureCheckList.Enabled = true;
                    PictureNixItButton.Visible = true;
                    break;
                default: break;
            }

            //DataSet dsMusic = dat.GetData("SELECT * FROM Ad_Song_Mapping WHERE AdID=" + dsEvent.Tables[0].Rows[0]["Ad_ID"].ToString());
            ////SongCheckList.Items.Clear();
            //if (dsMusic.Tables.Count > 0)
            //    if (dsMusic.Tables[0].Rows.Count > 0)
            //    {
            //        MusicPanel.Enabled = true;
            //        for (int i = 0; i < dsMusic.Tables[0].Rows.Count; i++)
            //        {
            //            SongCheckList.Enabled = true;
            //            DeleteSongButton.Visible = true;
            //            MusicCheckBox.Checked = true;
            //            ListItem newItem = new ListItem(dsMusic.Tables[0].Rows[i]["SongTitle"].ToString(),
            //                dsMusic.Tables[0].Rows[i]["SongName"].ToString());

            //            SongCheckList.Items.Add(newItem);
            //        }
            //    }

            DataSet dsCategories = dat.GetData("SELECT * FROM Ad_Category_Mapping WHERE AdID=" + ID);
            Session["categorySession"] = null;
            //if (dsCategories.Tables.Count > 0)
            //    if (dsCategories.Tables[0].Rows.Count > 0)
            //    {
            //        for (int i = 0; i < dsCategories.Tables[0].Rows.Count; i++)
            //        {
            //            Telerik.Web.UI.RadTreeNode node =
            //                (Telerik.Web.UI.RadTreeNode)CategoryTree.Nodes.FindNodeByValue(dsCategories.Tables[0].Rows[i]["CategoryID"].ToString());
            //            if (node != null)
            //            {
            //                node.Checked = true;
            //            }
            //            else
            //            {

            //                //node = (Telerik.Web.UI.RadTreeNode)RadTreeView1.Nodes.FindNodeByValue(dsCategories.Tables[0].Rows[i]["CategoryID"].ToString());
            //                //if (node != null)
            //                //{
            //                //    node.Checked = true;
            //                //}
            //                //else
            //                //{

            //                    node = (Telerik.Web.UI.RadTreeNode)RadTreeView2.Nodes.FindNodeByValue(dsCategories.Tables[0].Rows[i]["CategoryID"].ToString());
            //                    if (node != null)
            //                    {
            //                        node.Checked = true;
            //                    }
            //                    //else
            //                    //{
            //                    //    node = (Telerik.Web.UI.RadTreeNode)RadTreeView3.Nodes.FindNodeByValue(dsCategories.Tables[0].Rows[i]["CategoryID"].ToString());
            //                    //    if (node != null)
            //                    //    {
            //                    //        node.Checked = true;
            //                    //    }

            //                    //}

            //                //}
            //            }
            //            //CategoriesCheckBoxes.Items.FindByValue(dsCategories.Tables[0].Rows[i]["CategoryID"].ToString()).Selected = true;
            //            //CategoriesCheckBoxes.Items.FindByValue(dsCategories.Tables[0].Rows[i]["CategoryID"].ToString()).Enabled = false;
            //        }
            //    }


            DataView dvAdControl = dat.GetDataDV("SELECT * FROM AdControl");

            if (bool.Parse(dsEvent.Tables[0].Rows[0]["Featured"].ToString()))
            {
                TemplateRadioList.SelectedValue = dsEvent.Tables[0].Rows[0]["Template"].ToString();
                ChangeFeaturedText(TemplateRadioList, new EventArgs());
            }
            DataView dv = new DataView(dsEvent.Tables[0], "", "", DataViewRowState.CurrentRows);
            FeatureDatesListBox.Items.Clear();
            if (bool.Parse(dv[0]["Featured"].ToString()))
            {
                char[] delim = { ';' };
                string[] tokens = dv[0]["DatesOfAd"].ToString().Split(delim, StringSplitOptions.RemoveEmptyEntries);
                ListItem item;
                foreach (string token in tokens)
                {
                    item = new ListItem(token);
                    item.Attributes.Add("class", "DisabledListItem");
                    item.Value = "Disabled";
                    FeatureDatesListBox.Items.Add(item);
                }
            }

            if (Request.QueryString["Feature"] != null)
            {
                if (bool.Parse(Request.QueryString["Feature"]))
                {
                    ChangeSelectedTab(0, 1);
                    ChangeSelectedTab(1, 2);
                    ChangeSelectedTab(2, 3);
                    FeaturePanel.Visible = true;
                }
            }

            setCategories();
        }
        catch (Exception ex)
        {
            YourMessagesLabel.Text = ex.ToString();
            MessagePanel.Visible = true;
        }
    }

    protected void TabClick(object sender, EventArgs e)
    {
        int nextIndex = AdTabStrip.SelectedIndex;
        short selectThisTab = 0;
        //ErrorLabel.Text = "nextindex: " + nextIndex.ToString();
        
        if (Session["PrevTab"] != null)
        {
            int selectedIndex = (int)Session["PrevTab"];
            //ErrorLabel.Text += "selectedindex: " + selectedIndex.ToString();
            Session["PrevTab"] = nextIndex;

            if (selectedIndex != nextIndex)
            {
                bool wasClean = OnwardsIT(false, selectedIndex);
                if (wasClean)
                {
                    //ErrorLabel.Text += ";was here 1;";
                    selectThisTab = short.Parse(nextIndex.ToString());

                }
                else
                {
                    selectThisTab = short.Parse(selectedIndex.ToString());
                    //ErrorLabel.Text += ";was here 2;";
                }

                if (MessagePanel.Visible)
                {
                    selectThisTab = short.Parse(selectedIndex.ToString());
                    //ErrorLabel.Text += ";was here 3;";
                }
            }
            else
            {
                selectThisTab = short.Parse(selectedIndex.ToString());
            }

            if (selectedIndex.ToString() == "4")
            {
                selectThisTab = short.Parse(nextIndex.ToString());
            }

            //ErrorLabel.Text += "selectthistab: " + selectThisTab.ToString();
        }
        else
        {
            Session["PrevTab"] = nextIndex;
            selectThisTab = short.Parse(nextIndex.ToString());
        }

        ChangeSelectedTab(0, selectThisTab);
    }

    protected void Onwards(object sender, EventArgs e)
    {
        OnwardsIT(true, AdTabStrip.SelectedIndex);
    }

    protected bool OnwardsIT(bool changeTab, int selectedIndex)
    {
        try
        {
            YourMessagesLabel.Text = "";
            MessagePanel.Visible = false;
            string message = "";
            Session["PrevTab"] = selectedIndex;
            HttpCookie cookie = Request.Cookies["BrowserDate"];

            DateTime isn = DateTime.Now;

            if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
                isn = DateTime.Now;
            DateTime isNow = isn;
            Data dat = new Data(isn); switch (selectedIndex)
            {
                case 0:
                    #region First Tab
                    DescriptionTextBox.Content = dat.StripHTML_LeaveLinks(DescriptionTextBox.Content.Replace("<div>", "<br/>").Replace("</div>", ""));
                    AdNameTextBox.Text = dat.stripHTML(AdNameTextBox.Text);

                    CityTextBox.Text = dat.stripHTML(CityTextBox.Text.Trim());
                    string city = "";
                    if (CountryDropDown.SelectedValue == "223")
                        city = MajorCityDrop.SelectedItem.Text;
                    else
                        city = CityTextBox.Text;
                    if (AdNameTextBox.Text.Trim().Length != 0)
                    {
                        if (DescriptionTextBox.Text.Length <= 1000 && DescriptionTextBox.Text.Trim() != "")
                        {
                            if (city != "")
                            {
                                if (dat.TrapKey(CityTextBox.Text, 1))
                                {
                                        if (changeTab)
                                            ChangeSelectedTab(0, 1);
                                        MessagePanel.Visible = false;
                                        FillLiteral();
                                        return true;
                                }
                                else
                                {
                                    YourMessagesLabel.Text += "*City can only contain letters. No special characters allowed.";
                                    MessagePanel.Visible = true;
                                    AdTabStrip.SelectedIndex = 0;
                                    AdPostPages.PageViews[0].Selected = true;
                                    return false;
                                }
                            }
                            else
                            {
                                YourMessagesLabel.Text += "*Must include the city.";
                                MessagePanel.Visible = true;
                                AdTabStrip.SelectedIndex = 0;
                                AdPostPages.PageViews[0].Selected = true;
                                return false;
                            }
                            
                        }
                        else
                        {
                            YourMessagesLabel.Text += "*The description must be included and be less than or equal to 1000 characters.";
                            MessagePanel.Visible = true;
                            AdTabStrip.SelectedIndex = 0;
                            AdPostPages.PageViews[0].Selected = true;
                            return false;
                        }
                    }
                    else
                    {
                        MessagePanel.Visible = true;
                        YourMessagesLabel.Text += "*Must include a Headline.";
                        if (DescriptionTextBox.Content.Length < 100)
                            YourMessagesLabel.Text += "*Make sure you say what you want your viewers to hear about your advertisment! The description must be at least 100 characters.";
                        AdTabStrip.SelectedIndex = 0;
                        AdPostPages.PageViews[0].Selected = true;
                        return false;
                    }

                    return false;
                    #endregion
                    break;
                case 1:
                    bool isEditing = false;
                    if (isEdit.Text.Trim() != "")
                        if (bool.Parse(isEdit.Text))
                            isEditing = true;
                    if (isEditing && Session["categorySession"] == null)
                    {
                        Session["categorySession"] = true;
                        DataView dsCategories = dat.GetDataDV("SELECT * FROM Ad_Category_Mapping WHERE AdID=" + adID.Text);

                        List<Telerik.Web.UI.RadTreeNode> list = (List<Telerik.Web.UI.RadTreeNode>)CategoryTree.GetAllNodes();
                        //List<Telerik.Web.UI.RadTreeNode> list2 = (List<Telerik.Web.UI.RadTreeNode>)RadTreeView1.GetAllNodes();
                        List<Telerik.Web.UI.RadTreeNode> list3 = (List<Telerik.Web.UI.RadTreeNode>)RadTreeView2.GetAllNodes();
                        //List<Telerik.Web.UI.RadTreeNode> list4 = (List<Telerik.Web.UI.RadTreeNode>)RadTreeView3.GetAllNodes();

                        if (dsCategories.Count > 0)
                        {
                            for (int i = 0; i < list.Count; i++)
                            {
                                dsCategories.RowFilter = "CategoryID=" + list[i].Value;

                                if (dsCategories.Count > 0)
                                    list[i].Checked = true;

                            }

                            //for (int i = 0; i < list2.Count; i++)
                            //{
                            //    dsCategories.RowFilter = "CategoryID=" + list2[i].Value;

                            //    if (dsCategories.Count > 0)
                            //        list2[i].Checked = true;

                            //}

                            //for (int i = 0; i < list4.Count; i++)
                            //{
                            //    dsCategories.RowFilter = "CategoryID=" + list4[i].Value;

                            //    if (dsCategories.Count > 0)
                            //        list4[i].Checked = true;

                            //}

                            for (int i = 0; i < list3.Count; i++)
                            {
                                dsCategories.RowFilter = "CategoryID=" + list3[i].Value;

                                if (dsCategories.Count > 0)
                                    list3[i].Checked = true;

                            }


                        }
                    }
                    if(changeTab)
                        ChangeSelectedTab(1, 2);
                    MessagePanel.Visible = false;
                    FillLiteral();
                    return true;
                    break;
                case 2:
                    #region Fourth Tab

                    if (CategorySelected())
                    {
                        if (changeTab)
                            ChangeSelectedTab(2, 3);

                        FillLiteral();
                        FindPrice();
                        return true;
                    }
                    else
                    {
                        MessagePanel.Visible = true;
                        YourMessagesLabel.Text = "Must include at least one category.";
                        AdTabStrip.SelectedIndex = 2;
                        AdPostPages.PageViews[2].Selected = true;

                        return false;
                    }

                    return false;
                    #endregion
                    break;
                case 3:
                    #region Third Tab
                    bool goOn = false;

                    SummaryTextBox.InnerHtml = dat.StripHTML_LeaveLinksNoBr(SummaryTextBox.InnerHtml);

                    int countOfDates = 0;

                    if (FeaturedTextPanel.Visible)
                    {
                        if (FeatureDatesListBox.Items.Count != 0)
                        {
                            if (TemplateRadioList.SelectedValue == "1")
                            {
                                if (SummaryTextBox.InnerHtml.Trim().Length > 250 || SummaryTextBox.InnerHtml.Trim().Length == 0)
                                {
                                    YourMessagesLabel.Text += "*Must include the summary for a featured ad. It must be less than 250 characters.";
                                    MessagePanel.Visible = true;
                                    AdTabStrip.SelectedIndex = 3;
                                    AdPostPages.PageViews[3].Selected = true;
                                    return false;
                                }
                                else
                                {
                                    goOn = true;
                                }
                            }
                            else
                            {
                                if (SummaryTextBox.InnerHtml.Trim().Length > 100 || SummaryTextBox.InnerHtml.Trim().Length == 0)
                                {
                                    YourMessagesLabel.Text += "*Must include the summary for a featured ad. It must be less than 100 characters.";
                                    MessagePanel.Visible = true;
                                    AdTabStrip.SelectedIndex = 3;
                                    AdPostPages.PageViews[3].Selected = true;
                                    return false;
                                }
                                else
                                {
                                    goOn = true;
                                }
                            }

                            
                        }
                        else
                        {
                            goOn = false;
                            Session["Featured"] = false;
                            YourMessagesLabel.Text = "You have selected to feature the bulletin, but not entered any specifics. If you do not want to feature the bulletin any more, please click 'No, Thank You'.";
                            MessagePanel.Visible = true;
                            return false;
                        }
                    }
                    else
                    {
                        goOn = true;
                    }

                    if (goOn)
                    {

                        if (FeaturePanel.Visible && BannerAdCheckBox.Checked && AdPictureCheckList.Items.Count == 0)
                        {
                            YourMessagesLabel.Text += "*Please include the banner image or un-check the 'Include a Banner Image' check box.";
                            MessagePanel.Visible = true;
                            AdTabStrip.SelectedIndex = 3;
                            AdPostPages.PageViews[3].Selected = true;
                            return false;
                        }
                        else
                        {
                            if (FeaturePanel.Visible && FeatureDatesListBox.Items.Count == 0)
                            {
                                YourMessagesLabel.Text += "*You have chosen to feature your bulletin, but have not provided any dates.";
                                MessagePanel.Visible = true;
                                AdTabStrip.SelectedIndex = 3;
                                AdPostPages.PageViews[3].Selected = true;
                                return false;
                            }
                            else
                            {
                                //Add case for if Paypal is filled in...
                                //Authorize Credit Card
                                decimal price = 0.00M;
                                if (FeaturePanel.Visible && TotalLabel.Text.Trim() != "")
                                {
                                    if (decimal.TryParse(TotalLabel.Text.Trim(), out price))
                                    {
                                        if (price != 0.00M)
                                        {
                                            goOn = true;
                                            Session["Featured"] = true;
                                        }
                                        else
                                        {
                                            if (SearchTermsListBox.Items.Count > 0 && price == 0.00M)
                                            {
                                                goOn = false;
                                                Session["Featured"] = false;
                                                message = "You have entered search terms, but, have not included any dates.";
                                            }
                                            else
                                            {
                                                goOn = true;
                                                Session["Featured"] = false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        goOn = true;
                                        Session["Featured"] = false;
                                    }
                                }
                                else
                                {
                                    goOn = true;
                                    Session["Featured"] = false;
                                }
                            }
                        }

                    }


                    if (goOn)
                    {
                        if (Session["AdCategoriesSet"] == null && Request.QueryString["ID"] != null)
                        {
                            setCategories();
                        }

                        MessagePanel.Visible = false;
                        if (changeTab)
                            ChangeSelectedTab(3, 4);

                        FillLiteral();
                        return true;
                    }
                    else
                    {
                        MessagePanel.Visible = true;
                        if (message != "")
                            YourMessagesLabel.Text = message;
                        else
                            YourMessagesLabel.Text = "You have selected to feature the adventure, but not entered any specifics. If you do not want to feature the adventure any more, please click 'No, Thank You'.";
                    }
                    return false;
                    #endregion
                    break;
                default: return false; break;
            }
        }
        catch (System.Web.HttpRequestValidationException ex)
        {
            MessagePanel.Visible = true;
            YourMessagesLabel.Text = "Any text input fields cannot contain HTML or script data.";
        }
        catch (Exception ex)
        {
            Label label = new Label();
            if (Session["command"] != null)
                label.Text = ex.ToString() + "<br/>" + Session["command"].ToString();
            else
                label.Text = ex.ToString();


            MessagePanel.Visible = true;
            MessagePanel.Controls.Add(label);
        }
        return false;
    }

    //protected string GetAllZipsInRadius(bool likes, bool isCat)
    //{
    //    HttpCookie cookie = Request.Cookies["BrowserDate"];
    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

    //    //do only if United States and not for international zip codes
    //    string zip = "";
    //    string nonExistantZip = "";

    //    string cat = "";
    //    if (isCat)
    //        cat = "Cat";

    //    if (ZipTextBox.Text.Trim() != "")
    //    {
    //        if (RadiusPanel.Visible)
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
    //                    zip = " AND UP." + cat + "Zip = '" + zipParam.ToString() + "' ";
    //                    if (likes)
    //                        zip = " AND UP." + cat + "Zip LIKE '%;" + zipParam.ToString() + ";%' ";
    //                    nonExistantZip = " OR UP." + cat + "Zip = '" + zipParam.ToString() + "'";
    //                    if (likes)
    //                        nonExistantZip = " OR UP." + cat + "Zip LIKE '%;" + zipParam.ToString() + ";%' ";
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
    //                        zip = " AND (UP." + cat + "Zip = '" + zipParam.ToString() + "' " + nonExistantZip;
    //                        if (likes)
    //                            zip = " AND (UP." + cat + "Zip LIKE '%;" + zipParam.ToString() + ";%' " + nonExistantZip;
    //                        for (int i = 0; i < dsZips.Tables[0].Rows.Count; i++)
    //                        {
    //                            if (likes)
    //                                zip += " OR UP." + cat + "Zip LIKE '%;" + dsZips.Tables[0].Rows[i]["Zip"].ToString() + ";%' ";
    //                            else
    //                                zip += " OR UP." + cat + "Zip = '" + dsZips.Tables[0].Rows[i]["Zip"].ToString() + "' ";
    //                        }
    //                        zip += ") ";
    //                    }
    //                    else
    //                    {
    //                        if (likes)
    //                            zip = " AND UP." + cat + "Zip LIKE '%;" + ZipTextBox.Text.Trim() + ";%' ";
    //                        else
    //                            zip = " AND UP." + cat + "Zip='" + ZipTextBox.Text.Trim() + "'";
    //                    }
    //                }
    //                else
    //                {
    //                    if (likes)
    //                        zip = " AND UP." + cat + "Zip LIKE '%;" + ZipTextBox.Text.Trim() + ";%' ";
    //                    else
    //                        zip = " AND UP." + cat + "Zip='" + ZipTextBox.Text.Trim() + "'";
    //                }


    //            }
    //            else
    //            {
    //                if (likes)
    //                    zip = " AND UP." + cat + "Zip LIKE '%;" + ZipTextBox.Text.Trim() + ";%' ";
    //                else
    //                    zip = " AND UP." + cat + "Zip = '" + ZipTextBox.Text.Trim() + "' ";
    //            }
    //        }
    //        else
    //        {
    //            if (likes)
    //                zip = " AND UP." + cat + "Zip LIKE '%;" + ZipTextBox.Text.Trim() + ";%' ";
    //            else
    //                zip = " AND UP." + cat + "Zip = '" + ZipTextBox.Text.Trim() + "' ";
    //        }
    //    }
    //    else
    //    {
    //        zip = null;
    //    }

    //    return zip;
    //}

    protected bool CategorySelected()
    {
        return OneCategorySelected(ref CategoryTree) || OneCategorySelected(ref RadTreeView2);
    }

    protected bool OneCategorySelected(ref Telerik.Web.UI.RadTreeView treeView)
    {
        List<Telerik.Web.UI.RadTreeNode> list = (List<Telerik.Web.UI.RadTreeNode>)treeView.GetAllNodes();

        foreach (Telerik.Web.UI.RadTreeNode node in list)
        {
            if (node.Checked)
                return true;
        }

        return false;
    }

    protected DataView GetSavedSearchesUsers(ref int firstDayCountEmail, bool isAll)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn); string state = "";
        if (StateDropDownPanel.Visible)
            state = StateDropDown.SelectedItem.Text;
        else
            state = StateTextBox.Text;

        string city = "";
        if (CityTextBox.Text.Trim() != "")
        {
            city = CityTextBox.Text.Trim();
        }
        else
        {
            city = null;
        }

        bool isNoUsers = false;

        string categories = "";

        string zip = "";
            //GetAllZipsInRadius(true, false);

        
        dat.returnCategoryIDString(CategoryTree, ref categories);
        //dat.returnCategoryIDString(RadTreeView1, ref categories);
        dat.returnCategoryIDString(RadTreeView2, ref categories);
        //dat.returnCategoryIDString(RadTreeView3, ref categories);
        string command = "";
         

        DataView dv = dat.CalculateUsersEmailAdCapacity(ref command, AdNameTextBox.Text.ToLower(),
            DescriptionTextBox.Content.ToLower(), city, zip, state, CountryDropDown.SelectedItem.Value,
            categories, ref firstDayCountEmail, ref isNoUsers, isAll);

        Session["command"] = command;
        return dv;
    }

    private void FillLiteral()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn);

        if (FeaturePanel.Visible)
        {
            string templateID = "";

            templateID = TemplateRadioList.SelectedValue;


            FeaturedPreviewPanel.Visible = true;
            int integer = 2;
            string username = dat.GetDataDV("select * from Users where user_id=" + Session["User"].ToString())[0]["UserName"].ToString();
            string email = "";
            if (AdPictureCheckList.Items.Count == 0 || !AdMediaPanel.Visible)
                email = GetEmailString(AdNameTextBox.Text, null,
                 null, SummaryTextBox.InnerHtml, username, ref integer, templateID);
            else
                email = GetEmailString(AdNameTextBox.Text, AdPictureCheckList.Items[0].Value,
                 null, SummaryTextBox.InnerHtml, username, ref integer, templateID);

        
                email += "</tr>";

            email = "<table>" + email + "</table>";
            FeaturedPreviewLiteral.Text = email;
        }


        EventPanel.Visible = true;
        ShowHeaderName.Text = AdNameTextBox.Text;
        ShowDescription.Text = dat.BreakUpString(DescriptionTextBox.Content, 60);


        Rotator1.Items.Clear();
        char[] delim = { '\\' };
        string[] fileArray;

        string[] finalFileArray = new string[PictureCheckList.Items.Count];

        if (System.IO.Directory.Exists(MapPath(".") + "\\UserFiles\\" +
             Session["UserName"].ToString() + "\\AdSlider\\"))
        {
            fileArray = System.IO.Directory.GetFiles(MapPath(".") + "\\UserFiles\\" +
                 Session["UserName"].ToString() + "\\AdSlider\\");

        }

       
        char[] delim2 = { '.' };
        for (int i = 0; i < PictureCheckList.Items.Count; i++)
        {
            Literal literal4 = new Literal();
            string[] tokens = PictureCheckList.Items[i].Value.ToString().Split(delim2);
            string toUse = "";
            if (tokens.Length >= 2)
            {
                if (tokens[1].ToUpper() == "JPG" || tokens[1].ToUpper() == "JPEG" || tokens[1].ToUpper() == "GIF" || tokens[1].ToUpper() == "PNG")
                {
                    System.Drawing.Image image = System.Drawing.Image.FromFile(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\AdSlider\\" + PictureCheckList.Items[i].Value.ToString());
                    toUse = "/UserFiles/" + Session["UserName"].ToString() + "/AdSlider/" + PictureCheckList.Items[i].Value.ToString();

                    int width = 412;
                    int height = 250;

                    int newHeight = image.Height;
                    int newIntWidth = image.Width;

                    literal4.Text = "<div style=\"width: 412px; height: 250px;\"><img style=\" margin-left: " + ((410 - newIntWidth) / 2).ToString() + "px; margin-top: " + ((250 - newHeight) / 2).ToString() + "px;\" height=\"" + newHeight + "px\" width=\"" + newIntWidth + "px\" src=\""
                                                    + toUse + "\" /></div>";

                }
                else if (tokens[1].ToUpper() == "WMV")
                {
                    literal4.Text = "<object><param  name=\"wmode\" value=\"opaque\" ></param><embed wmode=\"opaque\" height=\"250px\" width=\"410px\" src=\""
                        + "UserFiles/" + Session["UserName"].ToString() + "/AdSlider/" + PictureCheckList.Items[i].Value.ToString() +
                        "\" /></object>";
                }
            }
            else
            {
                literal4.Text = "<div style=\"float:left;\"><object width=\"412\" height=\"250\"><param  name=\"wmode\" value=\"opaque\" ></param><param name=\"movie\" value=\"http://www.youtube.com/v/" + PictureCheckList.Items[i].Value.ToString() + "\"></param><param name=\"allowFullScreen\" value=\"true\"></param><embed wmode=\"opaque\" src=\"http://www.youtube.com/v/" + PictureCheckList.Items[i].Value.ToString() + "\" type=\"application/x-shockwave-flash\" allowfullscreen=\"true\" width=\"412\" height=\"250\"></embed></object></div>";
            }

            Telerik.Web.UI.RadRotatorItem r4 = new Telerik.Web.UI.RadRotatorItem();
            r4.Controls.Add(literal4);
            Rotator1.Items.Add(r4);
        }

        if (Rotator1.Items.Count == 0)
            RotatorPanel.Visible = false;
        else
            RotatorPanel.Visible = true;

        if (Rotator1.Items.Count == 1)
            RotatorPanel.CssClass = "HiddeButtons";
        else
            RotatorPanel.CssClass = "";
    }

    protected void Backwards(object sender, EventArgs e)
    {
        BackwardsIT();
    }

    protected void BackwardsIT()
    {
        int selectedIndex = AdTabStrip.SelectedIndex;

        switch (selectedIndex)
        {
            case 1:
                ChangeSelectedTab(1, 0);
                break;
            case 2:
                ChangeSelectedTab(2, 1);
                break;
            case 3:
                ChangeSelectedTab(3, 2);
                //if (bool.Parse(isFeatured.Text))
                //    ChangeSelectedTab(3, 2);
                //else
                //    ChangeSelectedTab(3, 1);
                break;
            case 4:
                Session["UserAvailableDate"] = null;
                Session.Remove("UserAvailableDate");
                ChangeSelectedTab(4, 3);
                break;
            case 5:
                ChangeSelectedTab(5, 4);
                break;
            case 6:
                if (FeaturePanel.Visible)
                    ChangeSelectedTab(6, 5);
                else
                    ChangeSelectedTab(6, 3);
                break;
            default: break;
        }
    }
    
    protected void setCategories()
    {
        CategoryTree.DataBind();
        RadTreeView2.DataBind();
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn); DataView dvCategories = dat.GetDataDV("SELECT * FROM Ad_Category_Mapping WHERE AdID=" +
            Request.QueryString["ID"].ToString());


        if (dvCategories.Count > 0)
        {
            List<Telerik.Web.UI.RadTreeNode> list = (List<Telerik.Web.UI.RadTreeNode>)CategoryTree.GetAllNodes();
            //List<Telerik.Web.UI.RadTreeNode> list2 = (List<Telerik.Web.UI.RadTreeNode>)RadTreeView1.GetAllNodes();
            List<Telerik.Web.UI.RadTreeNode> list3 = (List<Telerik.Web.UI.RadTreeNode>)RadTreeView2.GetAllNodes();
            //List<Telerik.Web.UI.RadTreeNode> list4 = (List<Telerik.Web.UI.RadTreeNode>)RadTreeView3.GetAllNodes();

            for (int i = 0; i < list.Count; i++)
            {
                dvCategories.RowFilter = "CategoryID=" + list[i].Value;

                if (dvCategories.Count > 0)
                    list[i].Checked = true;

            }

            for (int i = 0; i < list3.Count; i++)
            {
                dvCategories.RowFilter = "CategoryID=" + list3[i].Value;

                if (dvCategories.Count > 0)
                    list3[i].Checked = true;

            }
        }

        Session["AdCategoriesSet"] = "notnull";
    }
    
    protected void EnableMainAttractionPanel(object sender, EventArgs e)
    {
        if (MainAttractionCheck.Checked)
        {
            MainAttractionPanel.Enabled = true;
            MainAttractionPanel.Visible = true;
        }
        else
        {
            MainAttractionPanel.Enabled = false;
            MainAttractionPanel.Visible = false;
        }
    }
    
    protected void EnableAdMediaPanel(object sender, EventArgs e)
    {
        if (BannerAdCheckBox.Checked)
        {
            AdMediaPanel.Enabled = true;
            AdMediaPanel.Visible = true;

            TemplateRadioList.SelectedValue = "1";

            FeaturedTextLiteral.Text = "max 250 characters &nbsp;&nbsp;&nbsp;";
            SummaryTextBox.Attributes.Add("onkeyup", "CountChars()");
            FeaturedTextPanel.Visible = true;
            FeaturedTextNotAllowedPanel.Visible = false;
        }
        else
        {
            AdMediaPanel.Enabled = false;
            AdMediaPanel.Visible = false;
            FeaturedTextLiteral.Text = "max 250 characters &nbsp;&nbsp;&nbsp;";
            SummaryTextBox.Attributes.Add("onkeyup", "CountChars()");
            FeaturedTextPanel.Visible = true;
            FeaturedTextNotAllowedPanel.Visible = false;
        }
    }
    
    //protected void SliderUpload_Click(object sender, EventArgs e)
    //{
    //    if (SliderFileUpload.HasFile && SliderCheckList.Items.Count < 20)
    //    {
    //        string root = MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\AdSlider\\";
            
    //            if (!System.IO.Directory.Exists(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString()))
    //            {
    //                System.IO.Directory.CreateDirectory(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString());
    //            }
    //            System.IO.Directory.CreateDirectory(root);
            
    //        char[] delim = { '.' };
    //        string[] tokens = SliderFileUpload.FileName.Split(delim);
    //        string fileName = "rename" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Ticks.ToString() + "." + tokens[1];
    //        SliderFileUpload.SaveAs(root + fileName);
    //        SliderCheckList.Items.Add(new ListItem(SliderFileUpload.FileName, fileName));
    //        SliderDeleteButton.Visible = true;
    //    }
    //}

    protected void PictureUpload_Click(object sender, EventArgs e)
    {
        DateTime isNow;
        if (PictureUpload.UploadedFiles.Count > 0)
        {
            foreach (Telerik.Web.UI.UploadedFile file in PictureUpload.UploadedFiles)
            {
                if (PictureCheckList.Items.Count < 20)
                {
                    isNow = DateTime.Now;
                    if (PictureCheckList.Items.Count == 0)
                        UploadedVideosAndPics.Visible = true;
                    char[] delim = { '.' };
                    string[] tokens = file.FileName.Split(delim);

                    if (tokens.Length >= 2)
                    {
                        if (tokens[1].ToUpper() == "JPG" || tokens[1].ToUpper() == "JPEG" || tokens[1].ToUpper() == "GIF" || tokens[1].ToUpper() == "PNG")
                        {

                            string fileName = "rename" + isNow.Ticks.ToString() + "." + tokens[1];
                            PictureCheckList.Items.Add(new ListItem(file.FileName, fileName));

                            if (!System.IO.Directory.Exists(MapPath(".").ToString() + "/UserFiles/" +
                                Session["UserName"].ToString() + "/AdSlider/"))
                            {
                                if (!System.IO.Directory.Exists(MapPath(".").ToString() + "/UserFiles/" +
                                Session["UserName"].ToString() + "/AdSlider/"))
                                {
                                    if (!System.IO.Directory.Exists(MapPath(".").ToString() + "/UserFiles/" +
                                Session["UserName"].ToString()))
                                    {
                                        System.IO.Directory.CreateDirectory(MapPath(".").ToString() + "/UserFiles/" +
                                Session["UserName"].ToString());
                                    }
                                }

                                System.IO.Directory.CreateDirectory(MapPath(".").ToString() + "/UserFiles/" +
                                Session["UserName"].ToString() + "/AdSlider/");
                            }

                            System.Drawing.Image img = System.Drawing.Image.FromStream(file.InputStream);

                            SaveThumbnail(img, true, MapPath(".").ToString() + "/UserFiles/" +
                                Session["UserName"].ToString() + "/AdSlider/" + fileName, "image/" +
                                tokens[1].ToLower());

                            //PictureUpload.SaveAs(MapPath(".").ToString() + "/UserFiles/" + 
                            //    Session["UserName"].ToString() + "/AdSlider/" + fileName);
                            PictureNixItButton.Visible = true;
                        }
                        else
                        {
                            YourMessagesLabel.Text = "No go! Pictures can only be .gif, .jpg, .jpeg, or .png.";
                            MessagePanel.Visible = true;
                        }
                    }
                }
            }
        }
    }
    
    protected void AdPictureUpload_Click(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn);
        if (AdPictureUpload.HasFile)
        {
            AdPictureCheckList.Items.Clear();
            char[] delim = { '.' };
            string[] tokens = AdPictureUpload.FileName.Split(delim);
            string fileName = "rename" + isn.Ticks.ToString() + "." + tokens[1];
            AdPictureCheckList.Items.Add( new ListItem(AdPictureUpload.FileName, fileName));
            if (!System.IO.Directory.Exists(MapPath(".").ToString() + "/UserFiles/" +
                Session["UserName"].ToString()))
                System.IO.Directory.CreateDirectory(MapPath(".").ToString() + "/UserFiles/" +
                Session["UserName"].ToString());

            System.Drawing.Image img = System.Drawing.Image.FromStream(AdPictureUpload.PostedFile.InputStream);

            SaveThumbnail(img, false, MapPath(".").ToString() + "/UserFiles/" +
                Session["UserName"].ToString() + "/" + fileName, "image/" + tokens[1].ToLower());

            AdNixItButton.Visible = true;
        }
    }

    protected void YouTubeUpload_Click(object sender, EventArgs e)
    {
        if (YouTubeTextBox.Text != "")
        {
            YouTubeTextBox.Text = YouTubeTextBox.Text.Trim().Replace("http://www.youtube.com/watch?v=", "");
            if (PictureCheckList.Items.Count < 20)
            {
                if (PictureCheckList.Items.Count == 0)
                    UploadedVideosAndPics.Visible = true;
                PictureCheckList.Items.Add(new ListItem("YouTube ID: " + YouTubeTextBox.Text, YouTubeTextBox.Text));
                PictureNixItButton.Visible = true;
            }
        }
    }
    
    //protected void ShowVideo(object sender, EventArgs e)
    //{
    //    if (VideoRadioList.SelectedValue == "0")
    //    {
    //        UploadPanel.Visible = true;
    //        YouTubePanel.Visible = false;
    //    }
    //    else
    //    {
    //        UploadPanel.Visible = false;
    //        YouTubePanel.Visible = true;
    //    }
    //}
    
    //protected void ShowSliderOrVidPic(object sender, EventArgs e)
    //{
    //    if (MainAttractionRadioList.SelectedValue == "0")
    //    {
    //        PictureVideoPanel.Visible = true;
    //        SliderPanel.Visible = false;
    //    }
    //    else
    //    {
    //        PictureVideoPanel.Visible = false;
    //        SliderPanel.Visible = true;
    //    }
    //}

    protected void ShowSliderOrVidPic(object sender, EventArgs e)
    {
        if (MainAttracionRadioList.SelectedValue == "0")
        {
            YouTubePanel.Visible = true;
            PicturePanel.Visible = false;
        }
        else
        {
            YouTubePanel.Visible = false;
            PicturePanel.Visible = true;
        }
    }

    //protected void NixIt(object sender, EventArgs e)
    //{
    //    int songCount = SongCheckList.Items.Count;
    //    CheckBoxList tempList = new CheckBoxList();
    //    for (int i = 0; i < songCount; i++)
    //    {
    //        if (!SongCheckList.Items[i].Selected)
    //            tempList.Items.Add(SongCheckList.Items[i]);
    //    }
    //    SongCheckList.Items.Clear();
    //    for (int j = 0; j < tempList.Items.Count; j++)
    //    {
    //        SongCheckList.Items.Add(tempList.Items[j]);
    //    }
    //    if (SongCheckList.Items.Count == 0)
    //        DeleteSongButton.Visible = false;
    //}

    //protected void EnableMusicPanel(object sender, EventArgs e)
    //{
    //    if (MusicCheckBox.Checked)
    //    {
    //        MusicPanel.Enabled = true;
    //        MusicPanel.Visible = true;
    //    }
    //    else
    //    {
    //        MusicPanel.Enabled = false;
    //        MusicPanel.Visible = false;
    //    }
    //}

    //protected void MusicUpload_Click(object sender, EventArgs e)
    //{
    //    HttpCookie cookie = Request.Cookies["BrowserDate"];
    //    if (!MusicCheckBox.Checked)
    //        MusicCheckBox.Checked = true;

        

    //    if (MusicUpload.HasFile && SongCheckList.Items.Count < 3)
    //    {
    //        char[] delim = { '.' };
    //        string[] tokens = MusicUpload.FileName.Split(delim);
    //        string fileName = "rename" + DateTime.Parse(cookie.Value.ToString().Replace("%20",
    //            " ").Replace("%3A", ":")).Ticks.ToString() + "." + tokens[1];
    //        string extension = fileName.Split(delim)[1];
    //        if (extension.ToUpper() == "MP3")
    //        {
    //            string root = MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\Songs\\";
    //            if (SongCheckList.Items.Count == 0)
    //            {
    //                if (!System.IO.Directory.Exists(MapPath(".") + "\\UserFiles\\" +
    //                    Session["UserName"].ToString()))
    //                {
    //                    System.IO.Directory.CreateDirectory(MapPath(".") + "\\UserFiles\\" +
    //                        Session["UserName"].ToString());
    //                }

    //                if (!System.IO.Directory.Exists(root))
    //                {
    //                    System.IO.Directory.CreateDirectory(root);
    //                }

    //            }
    //            MusicUpload.SaveAs(root + fileName);
    //            SongCheckList.Items.Add(new ListItem(MusicUpload.FileName, fileName));
    //            DeleteSongButton.Visible = true;
    //        }
    //        else
    //        {
    //            MessagePanel.Visible = true;
    //            YourMessagesLabel.Text += "The music file has to be .mp3 file.";
    //        }
    //    }
    //}
    
    protected void AdPictureNixIt(object sender, EventArgs e)
    {
        AdPictureCheckList.Items.Clear();
        AdNixItButton.Visible = false;
    }

    protected void PictureNixIt(object sender, EventArgs e)
    {
        int songCount = PictureCheckList.Items.Count;
        CheckBoxList tempList = new CheckBoxList();
        for (int i = 0; i < songCount; i++)
        {
            if (!PictureCheckList.Items[i].Selected)
                tempList.Items.Add(PictureCheckList.Items[i]);
            else
            {
                //System.IO.File.Delete(MapPath(".") + "/UserFiles/" + Session["UserName"].ToString() + "/ADSlider/" + PictureCheckList.Items[i].Value);
            }
        }
        PictureCheckList.Items.Clear();
        for (int j = 0; j < tempList.Items.Count; j++)
        {
            PictureCheckList.Items.Add(tempList.Items[j]);
        }
        if (PictureCheckList.Items.Count == 0)
            UploadedVideosAndPics.Visible = false;
    }
    
    protected void PostIt(object sender, EventArgs e)
    {
        object appSeshCountry;
        object appSeshState;
        object appSeshCity;
        object appSeshZip;
        object appSeshRadius;
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn);
        string state = "";

        AuthorizePayPal d = new AuthorizePayPal();

        if (AgreeCheckBox.Checked)
        {
            bool chargeCard = false;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
            conn.Open();

            //Add case for if Paypal is filled in...
            //Authorize Credit Card
            decimal price = 0.00M;
            bool goOn = false;
            string message = "";
            string transactionID = "";
            if (FeaturePanel.Visible && TotalLabel.Text.Trim() != "")
            {
                if (decimal.TryParse(TotalLabel.Text.Trim(), out price))
                {
                    if (price != 0.00M)
                    {
                        if (PaymentPanel.Visible)
                        {
                            if (FirstNameTextBox.Text.Trim() == "" || LastNameTextBoxtBox.Text.Trim() == "" ||
                               BillingStreetAddressTextBox.Text.Trim() == "" || BillingCityTextBox.Text.Trim() == "" ||
                               BillingZipCodeTextBox.Text.Trim() == "" || BillingStateTextBox.Text.Trim() == "" ||
                                CardNumberTextBox.Text.Trim() == "" || CSVTextBox.Text.Trim() == "")
                            {
                                goOn = false;
                                Session["Featured"] = false;
                                message = "Please fill in all of the billing information.";
                            }
                            else
                            {
                                goOn = false;
                                Session["Featured"] = false;
                                string country = dat.GetDataDV("SELECT country_2_code FROM Countries WHERE country_id=" + BillingCountry.SelectedValue)[0]["country_2_code"].ToString();
                                com.paypal.sdk.util.NVPCodec status = d.DoPayment("Authorization", TotalLabel.Text, CardTypeDropDown.SelectedValue, CardNumberTextBox.Text.Trim(),
                                    ExpirationMonth.SelectedItem.Text, ExpirationYear.SelectedItem.Text, CSVTextBox.Text.Trim(), FirstNameTextBox.Text.Trim(), LastNameTextBoxtBox.Text.Trim(),
                                    BillingStreetAddressTextBox.Text.Trim(), BillingCityTextBox.Text, BillingStateTextBox.Text, country, BillingZipCodeTextBox.Text.Trim(), dat.GetIP());
                                message = status.ToString();
                                string successORFailure = status["ACK"];
                                switch (successORFailure.ToLower())
                                {
                                    case "failure":
                                        goOn = false;
                                        Session["Featured"] = false;
                                        message = status["L_LONGMESSAGE0"];
                                        break;
                                    case "successwithwarning":
                                        goOn = false;
                                        Session["Featured"] = false;
                                        message = status["L_SHORTMESSAGE0"];
                                        if (message == "Transaction approved but with invalid CSC format.")
                                            message = "Your CVC/CSV format for this card is not valid.";
                                        break;
                                    case "success":
                                        transactionID = status["TRANSACTIONID"];
                                        Session["TransID"] = transactionID;
                                        goOn = true;
                                        chargeCard = true;
                                        Session["Featured"] = true;
                                        break;
                                    default:
                                        goOn = false;
                                        Session["Featured"] = false;
                                        message = "There was an internal problem. Please contact support at: help@hippohappenings.com. Please include as much detail as possible about what you are trying to do.";
                                        foreach (string key in status.Keys)
                                        {
                                            message += "key: " + key.ToString() + ", value: " + status[key].ToString() + "<br/>";
                                        }
                                        break;
                                }
                            }
                        }
                        else
                        {
                            goOn = true;
                            Session["Featured"] = true;
                        }
                    }
                    else
                    {
                        if (SearchTermsListBox.Items.Count > 0 && price == 0.00M)
                        {
                            goOn = false;
                            Session["Featured"] = false;
                            message = "You have entered search terms, but, have not included any dates.";
                        }
                        else
                        {
                            goOn = true;
                            Session["Featured"] = false;
                        }
                    }
                }
                else
                {
                    goOn = true;
                    Session["Featured"] = false;
                }
            }
            else
            {
                goOn = true;
                Session["Featured"] = false;
            }

            if (goOn)
            {
                bool isEdit = false;

                DateTime LastFetUpdate = DateTime.Now;

                if (Request.QueryString["edit"] != null)
                {
                    isEdit = bool.Parse(Request.QueryString["edit"].ToString());
                }

                string mediaCat = "0";
                if (PictureCheckList.Items.Count > 0)
                    mediaCat = "1";

                string command = "";

                DataSet dsEvent;
                bool wasFeatured = false;
                DataView dvRenu = new DataView();
                if (isEdit)
                {
                    dsEvent = dat.GetData("SELECT * FROM Ads WHERE Ad_ID=" + adID.Text);
                    wasFeatured = bool.Parse(dsEvent.Tables[0].Rows[0]["Featured"].ToString());
                    LastFetUpdate = DateTime.Parse(dsEvent.Tables[0].Rows[0]["LastFetUpdate"].ToString());
                    //numViews = int.Parse(dsEvent.Tables[0].Rows[0]["NumViews"].ToString());

                    string rad = "";

                    //if (RadiusPanel.Visible)
                    //    rad = ", Radius=@radius ";

                    dvRenu = dat.GetDataDV("SELECT * FROM Ads WHERE Ad_ID=" + adID.Text);

                    command = "UPDATE Ads SET LastFetUpdate=@fetUpdate, DatesOfAd=@dates, Template=@template, hasSongs=@songs, User_ID=@userID, FeaturedSummary=@featuredSummary , " +
                        "Description=@description, Featured=@featured, Header=@header, CountShown=@countShown,  mediaCategory=" + mediaCat + ", " +
                        "FeaturedPicture=@featuredPicture, FeaturedPictureName=@featuredPictureName, CatCountry=@catCountry, " +
                        "CatState=@catState, CatCity=@catCity  WHERE Ad_ID=" + adID.Text;
                }
                else
                {
                    string rad = "";
                    string radEnd = "";
                    //if (RadiusPanel.Visible)
                    //{
                    //    rad = ", Radius ";
                    //    radEnd = ", @radius ";
                    //}

                    command = "INSERT INTO Ads (LastFetUpdate, DatesOfAd, Template, hasSongs, User_ID, FeaturedSummary ,Description, Header, " +
                        "CountShown, Featured, mediaCategory, FeaturedPicture, FeaturedPictureName, CatCountry, CatState, CatCity, " +
                        " DateAdded) "
                        + " VALUES('" + DateTime.Now.ToString() + "', @dates, @template, @songs, @userID, @featuredSummary, @description, @header, @countShown, " +
                        "@featured, " + mediaCat + ", @featuredPicture, @featuredPictureName, @catCountry, @catState, @catCity, '" +
                        DateTime.Now.ToString() + "')";
                }


                SqlCommand cmd = new SqlCommand(command, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = DescriptionTextBox.Content;
                cmd.Parameters.Add("@userID", SqlDbType.Int).Value = int.Parse(Session["User"].ToString());

                string fetDays = "";
                bool newDates = false;
                foreach (ListItem item in FeatureDatesListBox.Items)
                {
                    fetDays += ";" + item.Text + ";";
                    if (item.Value != "Disabled")
                        newDates = true;
                }

                if (wasFeatured)
                {
                    if (newDates)
                        cmd.Parameters.Add("@fetUpdate", SqlDbType.DateTime).Value = DateTime.Now;
                    else
                        cmd.Parameters.Add("@fetUpdate", SqlDbType.DateTime).Value = LastFetUpdate;

                    cmd.Parameters.Add("@featured", SqlDbType.Bit).Value = true;

                    if (FeaturePanel.Visible)
                    {
                        cmd.Parameters.Add("@dates", SqlDbType.NVarChar).Value = fetDays;
                    }
                    else
                        cmd.Parameters.Add("@dates", SqlDbType.NVarChar).Value = dvRenu[0]["DatesOfAd"].ToString();
                }
                else
                {
                    cmd.Parameters.Add("@fetUpdate", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.Add("@featured", SqlDbType.Bit).Value = FeaturePanel.Visible;
                    if (FeaturePanel.Visible)
                    {
                        cmd.Parameters.Add("@dates", SqlDbType.NVarChar).Value = fetDays;
                    }
                    else
                    {
                        cmd.Parameters.Add("@dates", SqlDbType.NVarChar).Value = DBNull.Value;
                    }
                }

                if (FeaturePanel.Visible)
                {
                    if (BannerAdCheckBox.Checked)
                    {
                        cmd.Parameters.Add("@template", SqlDbType.Int).Value = TemplateRadioList.SelectedValue;

                    }
                    else
                    {
                        cmd.Parameters.Add("@template", SqlDbType.Int).Value = 1;
                    }
                }
                else
                {
                    if (wasFeatured)
                        cmd.Parameters.Add("@template", SqlDbType.Int).Value = dvRenu[0]["Template"].ToString();
                    else
                        cmd.Parameters.Add("@template", SqlDbType.Int).Value = DBNull.Value;
                }
                cmd.Parameters.Add("@songs", SqlDbType.Bit).Value = false;

                if (FeaturePanel.Visible)
                    cmd.Parameters.Add("@featuredSummary", SqlDbType.NVarChar).Value = SummaryTextBox.InnerHtml;
                else
                {
                    if (wasFeatured)
                        cmd.Parameters.Add("@featuredSummary", SqlDbType.NVarChar).Value = dvRenu[0]["FeaturedSummary"].ToString();
                    else
                        cmd.Parameters.Add("@featuredSummary", SqlDbType.NVarChar).Value = DBNull.Value;
                }

                cmd.Parameters.Add("@header", SqlDbType.NVarChar).Value = AdNameTextBox.Text;
                cmd.Parameters.Add("@countShown", SqlDbType.Int).Value = 0;

                if (FeaturePanel.Visible)
                    if (AdPictureCheckList.Items.Count > 0 && AdMediaPanel.Visible)
                    {
                        cmd.Parameters.Add("@featuredPicture", SqlDbType.NVarChar).Value = AdPictureCheckList.Items[0].Value;
                        cmd.Parameters.Add("@featuredPictureName", SqlDbType.NVarChar).Value = AdPictureCheckList.Items[0].Text;
                    }
                    else
                    {
                        cmd.Parameters.Add("@featuredPicture", SqlDbType.NVarChar).Value = DBNull.Value;
                        cmd.Parameters.Add("@featuredPictureName", SqlDbType.NVarChar).Value = DBNull.Value;
                    }
                else
                {
                    if (wasFeatured)
                    {
                        cmd.Parameters.Add("@featuredPicture", SqlDbType.NVarChar).Value = dvRenu[0]["FeaturedPicture"].ToString();
                        cmd.Parameters.Add("@featuredPictureName", SqlDbType.NVarChar).Value = dvRenu[0]["FeaturedPictureName"].ToString();
                    }
                    else
                    {
                        cmd.Parameters.Add("@featuredPicture", SqlDbType.NVarChar).Value = DBNull.Value;
                        cmd.Parameters.Add("@featuredPictureName", SqlDbType.NVarChar).Value = DBNull.Value;
                    }
                }

                if (CountryDropDown.SelectedIndex != -1)
                {
                    appSeshCountry = CountryDropDown.SelectedValue;
                    cmd.Parameters.Add("@catCountry", SqlDbType.Int).Value = CountryDropDown.SelectedValue;


                    if (StateDropDownPanel.Visible)
                    {
                        if (StateDropDown.SelectedIndex != -1)
                            state = StateDropDown.SelectedItem.Text;
                    }
                    else
                        state = StateTextBox.Text;

                    appSeshState = state;

                    if (state != "")
                        cmd.Parameters.Add("@catState", SqlDbType.NVarChar).Value = state;
                    else
                        cmd.Parameters.Add("@catState", SqlDbType.NVarChar).Value = DBNull.Value;

                    string city = "";
                    if (CityDropDownPanel.Visible)
                        city = MajorCityDrop.SelectedItem.Text;
                    else
                        city = CityTextBox.Text;
                    appSeshCity = city;
                    cmd.Parameters.Add("@catCity", SqlDbType.NVarChar).Value = city;
                }
                else
                {
                    cmd.Parameters.Add("@catCountry", SqlDbType.Int).Value = DBNull.Value;
                    cmd.Parameters.Add("@catState", SqlDbType.NVarChar).Value = DBNull.Value;
                    cmd.Parameters.Add("@catCity", SqlDbType.NVarChar).Value = DBNull.Value;
                }

                //Media Categories: NONE: 0, Slider: 1.
                bool isSlider = false;
                if (PictureCheckList.Items.Count > 0)
                    isSlider = true;


                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("SELECT @@IDENTITY AS ID", conn);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd);
                DataSet ds3 = new DataSet();
                da2.Fill(ds3);

                string ID = ds3.Tables[0].Rows[0]["ID"].ToString();

                if (isEdit)
                    ID = Request.QueryString["ID"].ToString();

                #region Take care of search terms
                if (FeaturePanel.Visible)
                {
                    string terms = "";
                    foreach (ListItem item in SearchTermsListBox.Items)
                    {
                        terms += ";" + item.Text + ";";
                    }
                    foreach (ListItem item in FeatureDatesListBox.Items)
                    {
                        if (item.Value != "Disabled")
                            dat.Execute("INSERT INTO AdSearchTerms (AdID, SearchTerms, SearchDate) VALUES(" + ID +
                                ", '" + terms.Replace("'", "''") + "', '" + item.Text + "')");
                    }
                }
                #endregion

                string theID = ID;
                if (isEdit)
                {
                    theID = adID.Text;
                }
                //if (MusicCheckBox.Checked)
                //{

                //    if (isEdit)
                //    {

                //        dat.Execute("DELETE FROM Ad_Song_Mapping WHERE AdID="+theID);
                //    }

                //    for (int i = 0; i < SongCheckList.Items.Count; i++)
                //    {
                //            cmd = new SqlCommand("INSERT INTO Ad_Song_Mapping (AdID, SongName, SongTitle) "+
                //                "VALUES(@eventID, @songName, @songTitle)", conn);
                //            cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = int.Parse(theID);
                //            cmd.Parameters.Add("@songName", SqlDbType.NVarChar).Value = SongCheckList.Items[i].Value.ToString();
                //            cmd.Parameters.Add("@songTitle", SqlDbType.NVarChar).Value = SongCheckList.Items[i].Text;
                //            cmd.ExecuteNonQuery();

                //            dat.Execute("UPDATE Ads SET hasSongs=1 WHERE Ad_ID="+theID);
                //    }
                //}

                if (isEdit)
                {
                    dat.Execute("DELETE FROM Ad_Slider_Mapping WHERE AdID=" + theID);
                }

                if (MainAttractionCheck.Checked)
                {

                    if (isSlider)
                    {



                        char[] delim2 = { '\\' };
                        string[] fileArray = System.IO.Directory.GetFiles(MapPath(".") + "\\UserFiles\\" +
                            Session["UserName"].ToString() + "\\AdSlider\\");

                        if (!System.IO.Directory.Exists(MapPath(".") + "\\UserFiles"))
                        {
                            System.IO.Directory.CreateDirectory(MapPath(".") + "\\UserFiles");
                            System.IO.Directory.CreateDirectory(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\");
                            System.IO.Directory.CreateDirectory(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\AdSlider");
                            System.IO.Directory.CreateDirectory(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\AdSlider\\" + theID);
                        }
                        else
                        {
                            if (!System.IO.Directory.Exists(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\"))
                            {
                                System.IO.Directory.CreateDirectory(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\");
                                System.IO.Directory.CreateDirectory(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\AdSlider");
                                System.IO.Directory.CreateDirectory(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\" + theID);
                            }
                            else
                            {
                                if (!System.IO.Directory.Exists(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\AdSlider"))
                                {
                                    System.IO.Directory.CreateDirectory(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\AdSlider");
                                    System.IO.Directory.CreateDirectory(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\AdSlider\\" + theID);
                                }
                                else
                                {
                                    if (!System.IO.Directory.Exists(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\AdSlider\\" + theID))
                                    {
                                        System.IO.Directory.CreateDirectory(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\AdSlider\\" + theID);
                                    }
                                }
                            }
                        }

                        string YouTubeStr = "";
                        char[] delim3 = { '.' };
                        for (int i = 0; i < PictureCheckList.Items.Count; i++)
                        {
                            int length = fileArray[i].Split(delim2).Length;
                            string[] tokens = PictureCheckList.Items[i].Value.ToString().Split(delim3);


                            if (tokens.Length >= 2)
                            {
                                if (tokens[1].ToUpper() == "JPG" || tokens[1].ToUpper() == "JPEG" || tokens[1].ToUpper() == "GIF" || tokens[1].ToUpper() == "PNG")
                                {
                                    if (!System.IO.File.Exists(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() +
                                        "\\AdSlider\\" + theID + "\\" + PictureCheckList.Items[i].Value))
                                    {
                                        System.IO.File.Copy(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() +
                                                                "\\AdSlider\\" + PictureCheckList.Items[i].Value,
                                                                MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() +
                                        "\\AdSlider\\" + theID + "\\" +
                                                                PictureCheckList.Items[i].Value);
                                    }



                                    cmd = new SqlCommand("INSERT INTO Ad_Slider_Mapping (AdID, PictureName, RealPictureName) " +
                                        "VALUES (@eventID, @picName, @realName)", conn);
                                    cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = theID;
                                    cmd.Parameters.Add("@picName", SqlDbType.NVarChar).Value = PictureCheckList.Items[i].Value;
                                    cmd.Parameters.Add("@realName", SqlDbType.NVarChar).Value = PictureCheckList.Items[i].Text;
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                YouTubeStr += PictureCheckList.Items[i].Value + ";";
                            }

                        }

                        //if (YouTubeStr != "")
                        //{
                        dat.Execute("UPDATE Ads SET YouTubeVideo='" + YouTubeStr + "' WHERE Ad_ID=" + theID);
                        //}

                    }
                }
                else
                {

                }

                if (isEdit)
                {
                    dat.Execute("DELETE FROM Ad_Category_Mapping WHERE AdID=" + theID);
                }

                CreateCategories(theID);

                string adFeatured = "";
                string adFeaturedEmail = "";

                DataSet dsUser = dat.GetData("SELECT Email, UserName FROM USERS WHERE User_ID=" +
                        Session["User"].ToString());

                try
                {
                    bool showMessage = false;
                    if (chargeCard)
                    {
                        Encryption encrypt = new Encryption();

                        //Charge Card
                        string country = dat.GetDataDV("SELECT country_2_code FROM Countries WHERE country_id=" + BillingCountry.SelectedValue)[0]["country_2_code"].ToString();
                        com.paypal.sdk.util.NVPCodec status = d.DoCaptureCode(transactionID, price.ToString(),
                            "B" + theID + isn.ToString(), "Capture Transaction for Featuring Bulletin '" +
                            dat.MakeNiceNameFull(AdNameTextBox.Text) + "'");
                        //message = status.ToString();
                        string successORFailure = status["ACK"];
                        switch (successORFailure.ToLower())
                        {
                            case "failure":
                                MessagePanel.Visible = true;
                                YourMessagesLabel.Text += status["L_LONGMESSAGE0"];
                                //MessagePanel.Visible = true;
                                //foreach (string key in status.Keys)
                                //{
                                //    YourMessagesLabel.Text += "key: '" + key + "', value: '" + status[key] + "' <br/>";
                                //}
                                break;
                            case "success":
                                showMessage = true;
                                break;
                            default:
                                MessagePanel.Visible = true;
                                foreach (string key in status.Keys)
                                {
                                    YourMessagesLabel.Text += "key: '" + key + "', value: '" + status[key] + "' <br/>";
                                }
                                break;
                        }
                    }
                    else
                    {
                        showMessage = true;
                    }

                    if (showMessage)
                    {

                        string emailBody = "Dear " + dsUser.Tables[0].Rows[0]["UserName"].ToString() + ", <br/><br/> you have successfully posted the bulletin \"" + AdNameTextBox.Text +
                            "\". <br/><br/> You can find this bulletin <a href=\"http://hippohappenings.com/" + dat.MakeNiceName(AdNameTextBox.Text) + "_" + theID + "_Ad\">here</a>. " + adFeaturedEmail +
                            "<br/><br/> To rate your experience posting this bulletin <a href=\"http://hippohappenings.com/RateExperience.aspx?Type=A&ID=" + theID + "\">please include your feedback here.</a>" +
                            "<br/><br/><br/>Have a HippoHappening Day!<br/><br/>";

                        if (isEdit)
                        {
                            emailBody = "Dear " + dsUser.Tables[0].Rows[0]["UserName"].ToString() + ", <br/><br/> you have successfully edited the bulletin \"" + AdNameTextBox.Text +
                                "\". <br/><br/> You can find this bulletin <a href=\"http://hippohappenings.com/" + dat.MakeNiceName(AdNameTextBox.Text) + "_" + theID + "_Ad\">here</a>. " + adFeaturedEmail +
                                "<br/><br/> To rate your experience editing this bulletin <a href=\"http://hippohappenings.com/RateExperience.aspx?Type=A&ID=" + theID + "\">please include your feedback here.</a>" +
                                "<br/><br/><br/>Have a HippoHappening Day!<br/><br/>";
                        }

                        if (!Request.IsLocal)
                        {
                            dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                        System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                                        dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "You have successfully posted the bulletin: " +
                                        AdNameTextBox.Text);
                        }
                        conn.Close();

                        Session["Message"] = "Your bulletin has been posted successfully!<br/> Here are your next steps: <br/>";

                        if (isEdit)
                        {
                            Session["Message"] = "Your bulletin has been edited successfully!<br/> Here are your next steps: <br/>";
                        }


                        //Clear cache so that the PlayerList.xml can be grabbed by the browser again.
                        ClearCache();

                        Session["Message"] += "<br/>" + "Go to <a class=\"AddLink\" onclick=\"Search('" + dat.MakeNiceName(AdNameTextBox.Text) + "_" + theID + "_Ad');\">your bulletin's</a> home page." + adFeatured + "<br/><br/> -<a class=\"AddLink\" onclick=\"Search('RateExperience.aspx?Type=A&ID=" + theID + "');\" >Rate </a>your user experience posting this bulletin.<br/>";
                        //MessageLiteral.Text = "<script type=\"text/javascript\">alert('" + message + "');</script>";
                        Encryption encrypt = new Encryption();
                        MessageRadWindow.NavigateUrl = "Message.aspx?message=" + encrypt.encrypt(Session["Message"].ToString() +
                            "<br/><br/><div align=\"center\">" +
                            "<div style=\"width: 50px;\" onclick=\"Search('home')\">" +
                            "<div class=\"topDiv\" style=\"clear: both;\">" +
                              "  <img style=\"float: left;\" src=\"NewImages/ButtonLeft.png\" height=\"27px\" /> " +
                               " <div class=\"NavyLink\" style=\"font-size: 12px; text-decoration: none; padding-top: 5px;padding-left: 6px; padding-right: 6px;height: 27px;float: left;background: url('NewImages/ButtonPixel.png'); background-repeat: repeat-x;\">" +
                                   " OK " +
                                "</div>" +
                               " <img style=\"float: left;\" src=\"NewImages/ButtonRight.png\" height=\"27px\" /> " +
                            "</div>" +
                            "</div>" +
                            "</div><br/>");
                        MessageRadWindow.Visible = true;
                        MessageRadWindowManager.VisibleOnPageLoad = true;

                        Session["categorySession"] = null;
                    }
                }
                catch (Exception ex)
                {
                    MessagePanel.Visible = true;
                    YourMessagesLabel.Text += ex.ToString();
                }
            }
            else
            {
                MessagePanel.Visible = true;
                YourMessagesLabel.Text = message;
            }
        }
        else
        {
            MessagePanel.Visible = true;
            YourMessagesLabel.Text += "You must agree to the terms and conditions.";
        }
    }

    protected string GetEmailString(string header, string picture, string ID, string description,
        string userName, ref int normalAdCount, string templateID)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn); string adtitle = "style=\"color: #7c7a7a;font-size: 14px;font-weight: bold;text-decoration: none;\"";
        string adbody = "style=\"color: #7c7a7a;font-size: 12px;font-weight: normal;\"";
        string readmorelink = "style=\"color: #09718F;text-decoration: none;cursor: pointer;\"";
        string email = "";

        string w = "";
        string h = "";

        if (normalAdCount % 2 == 0)
        {
            email += "<tr><td>";
        }
        else
        {
            email += "<td>";
        }
        email += "<div align=\"center\" style=\"position: relative;float: left;width: 198px; height: 262px;border: solid 1px #d9d6d6; " +
                        "margin-right: 2px; " +
            "\">";

        if (picture == null)
        {
            email += "<div style=\"padding-top: 10px; clear: both; padding-left: 1px;\">";
        }
        else
        {

            GetAdSize(out w, out h, picture, templateID);

            if (templateID == "1")
            {
                email += "<div style=\"padding-top: 10px; clear: both; padding-left: 1px;\"><div style=\"float: left;padding-right: 2px;\"><table width=\"100px\" height=\"100px\" " +
                                "cellpadding=\"0\" cellspacing=\"0\"><tbody align=\"center\"><tr><td valign=\"middle\">";
            }
            else if (templateID == "2")
            {
                email += "<div style=\"padding-top: 10px; clear: both;\"><div style=\"float: left;padding-right: 2px;\"><table width=\"198px\" height=\"140px\" " +
                                "cellpadding=\"0\" cellspacing=\"0\"><tbody align=\"center\"><tr><td valign=\"middle\"> ";
            }
            else
            {
                email += "<div style=\"clear: both;\"><div center=\"float\" style=\"float: left;\"><table width=\"198px\" height=\"262px\" " +
                                "cellpadding=\"0\" cellspacing=\"0\"><tbody align=\"center\"><tr><td valign=\"middle\"> ";
            }

            if (ID == null)
            {
                email += "<img alt=\"" + header + "\" " +
                    "name=\"" + header + "\" width='" + w + "px' height='" + h + "px' src=\"./UserFiles/" +
                    userName + "/" + picture + "\" /></td></tr></tbody></table></div>";
            }
            else
            {
                email += "<a href=\"http://HippoHappenings.com/" + dat.MakeNiceName(header) + "_" + ID + "_Ad" +
              "\" ><img style='border: 0;' alt=\"" + header + "\" " +
                    "name=\"" + header + "\" width='" + w + "px' height='" + h + "px' src=\"http://hippohappenings.com/UserFiles/" +
                    userName + "/" + picture + "\" /></a></td></tr></tbody></table></div>";
            }
            
        }

        if (ID == null)
        {
            if (templateID == "1")
            {
                email += "<div><a class=\"Text14\">" + dat.BreakUpString(header, 10) +
                                    "</a></div></div>" +
                        "<div align=\"center\" style=\"clear: both; padding-left: 2px; padding-right: 2px;" +
                        "padding-top: 1px;\"> " +
                        "<span class=\"Text12\">" +
                        dat.BreakUpString(description, 21) + "</span>" +

                        "</div>" +
                        "<div style=\"clear: none;\"><a class=\"ReadMoreHome2\">Read More</a>" +
                        "</div></div>";
            }
            else if (templateID == "2")
            {
                email += "</div><div style=\"padding-top: 5px; clear: both;  padding-right: 4px;padding-left: 4px;\"><a class=\"Text14\">" +
                    dat.BreakUpString(header, 21) +
                                    "</a></div>" +
                        "<div align=\"center\" style=\"clear: both; padding-left: 4px; padding-right: 4px;\"> " +
                        "<span class=\"Text12\">" +
                        dat.BreakUpString(description, 21) + "</span>" +
                        "</div>" +
                        "<div style=\"clear: none;\"><a class=\"ReadMoreHome2\">Read More</a>" +
                        "</div></div>";
            }
        }
        else
        {
            if (templateID == "1")
            {
                email += "<div><a href=\"../" + dat.MakeNiceName(header) +
                                    "_" + ID + "_Ad\" class=\"Text14\">" + dat.BreakUpString(header, 10) +
                                    "</a></div></div>" +
                        "<div align=\"center\" style=\"clear: both; padding-left: 2px; padding-right: 2px;" +
                        "padding-top: 1px;\"> " +
                        "<span class=\"Text12\">" +
                        dat.BreakUpString(description, 21) + "</span>" +

                        "</div>" +
                        "<div style=\"clear: none;\"><a class=\"ReadMoreHome2\" href=\"../" +
                        dat.MakeNiceName(header) +
                                    "_" + ID + "_Ad\">Read More</a>" +
                        "</div></div>";
            }
            else if (templateID == "2")
            {
                email += "</div><div style=\"padding-top: 5px; clear: both; padding-right: 4px;padding-left: 4px;\"><a href=\"../" +
                    dat.MakeNiceName(header) +
                                    "_" + ID + "_Ad\" class=\"Text14\">" + dat.BreakUpString(header, 21) +
                                    "</a></div>" +
                        "<div align=\"center\" style=\"clear: both; padding-left: 4px; padding-right: 4px;\"> " +
                        "<span class=\"Text12\">" +
                        dat.BreakUpString(description, 21) + "</span>" +
                        "</div>" +
                        "<div style=\"clear: none;\"><a class=\"ReadMoreHome2\" href=\"../" +
                        dat.MakeNiceName(header) +
                                    "_" + ID + "_Ad\">Read More</a>" +
                        "</div></div>";
            }
        }
        if (normalAdCount % 2 == 0)
        {
            email += "</td>";
        }
        else
        {
            email += "</td></tr>";
        }

        normalAdCount++;

        return email;
    }

    protected void GetAdSize(out string w, out string h, string picture, string templateID)
    {
        System.Drawing.Image image =
            System.Drawing.Image.FromFile(MapPath(".") + "\\UserFiles\\" +
            Session["UserName"].ToString() + "\\" + picture);

        int height = 100;
        int width = 100;

        if (templateID == "2")
        {
            height = 140;
            width = 190;
        }
        else if (templateID == "3")
        {
            height = 250;
            width = 198;
        }


        int newHeight = 0;
        int newIntWidth = 0;

        float newFloatHeight = 0.00F;
        float newFloatWidth = 0.00F;

        if (image.Height > height || image.Width > width)
        {


            if (image.Height >= image.Width)
            {
                //leave the height as is
                newHeight = height;

                float frak = float.Parse(newHeight.ToString()) / float.Parse(image.Height.ToString());

                newFloatWidth = image.Width * frak;
                newIntWidth = (int)newFloatWidth;
            }
            //if image height is greater than resize height...resize it
            else
            {
                newIntWidth = width;

                float frak = float.Parse(newIntWidth.ToString()) / float.Parse(image.Width.ToString());

                newFloatHeight = image.Height * frak;
                newHeight = (int)newFloatHeight;
            }

            w = newIntWidth.ToString();
            h = newHeight.ToString();
        }
        else
        {
            w = image.Width.ToString();
            h = image.Height.ToString();
        }
    }

    protected void CreateCategories(string ID)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn); GetCheckedCategories(ref CategoryTree, ID);
        //GetCheckedCategories(ref RadTreeView1, ID);
        GetCheckedCategories(ref RadTreeView2, ID);
        //GetCheckedCategories(ref RadTreeView3, ID);

    }

    protected void GetCheckedCategories(ref Telerik.Web.UI.RadTreeView CategoryTree, string ID)
    {
        List<Telerik.Web.UI.RadTreeNode> list = (List<Telerik.Web.UI.RadTreeNode>)CategoryTree.GetAllNodes();

        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn); string tagsize = "22";

        foreach (Telerik.Web.UI.RadTreeNode node in list)
        {
            if (node.Checked)
            {
                if (node.ParentNode != null)
                {
                    tagsize = "16";
                }
                else
                {
                    tagsize = "22";
                }

                dat.Execute("INSERT INTO Ad_Category_Mapping (CategoryID, AdID, tagSize) VALUES("
                            + node.Value + ", " + ID + ", "+tagsize+")");
            }
        }
    }

    protected string GetCheckedCategoriesString()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        List<Telerik.Web.UI.RadTreeNode> list = (List<Telerik.Web.UI.RadTreeNode>)CategoryTree.GetAllNodes();
        string categories = "";
        string addCat = "";
        int count = 0;
        foreach (Telerik.Web.UI.RadTreeNode node in list)
        {
            if (node.Checked)
            {
                categories += " UC.CategoryID=" + node.Value + " OR ";


                for (int i = 0; i < node.Nodes.Count;i++ )
                {
                    categories += " UC.CategoryID=" + node.Nodes[i].Value + " OR ";

                    for (int j = 0; j < node.Nodes[i].Nodes.Count; j++)
                    {
                        categories += " UC.CategoryID=" + node.Nodes[i].Nodes[j].Value + " OR ";
                    }
                }
            }
        }

        //list = (List<Telerik.Web.UI.RadTreeNode>)RadTreeView1.GetAllNodes();

        //foreach (Telerik.Web.UI.RadTreeNode node in list)
        //{
        //    if (node.Checked)
        //    {
        //        categories += " UC.CategoryID=" + node.Value + " OR ";


        //        for (int i = 0; i < node.Nodes.Count; i++)
        //        {
        //            categories += " UC.CategoryID=" + node.Nodes[i].Value + " OR ";

        //            for (int j = 0; j < node.Nodes[i].Nodes.Count; j++)
        //            {
        //                categories += " UC.CategoryID=" + node.Nodes[i].Nodes[j].Value + " OR ";
        //            }
        //        }
        //    }
        //}

        list = (List<Telerik.Web.UI.RadTreeNode>)RadTreeView2.GetAllNodes();

        foreach (Telerik.Web.UI.RadTreeNode node in list)
        {
            if (node.Checked)
            {
                categories += " UC.CategoryID=" + node.Value + " OR ";


                for (int i = 0; i < node.Nodes.Count; i++)
                {
                    categories += " UC.CategoryID=" + node.Nodes[i].Value + " OR ";

                    for (int j = 0; j < node.Nodes[i].Nodes.Count; j++)
                    {
                        categories += " UC.CategoryID=" + node.Nodes[i].Nodes[j].Value + " OR ";
                    }
                }
            }
        }

        //list = (List<Telerik.Web.UI.RadTreeNode>)RadTreeView3.GetAllNodes();

        //foreach (Telerik.Web.UI.RadTreeNode node in list)
        //{
        //    if (node.Checked)
        //    {
        //        categories += " UC.CategoryID=" + node.Value + " OR ";


        //        for (int i = 0; i < node.Nodes.Count; i++)
        //        {
        //            categories += " UC.CategoryID=" + node.Nodes[i].Value + " OR ";

        //            for (int j = 0; j < node.Nodes[i].Nodes.Count; j++)
        //            {
        //                categories += " UC.CategoryID=" + node.Nodes[i].Nodes[j].Value + " OR ";
        //            }
        //        }
        //    }
        //}

        return categories.Remove(categories.Length - 4, 4);
    }

    private void ChangeSelectedTab(int unselectIndex, short selectIndex)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn);
        if (CountryDropDown.SelectedValue == "223")
        {
            MajorCity.Text = MajorCityDrop.SelectedItem.Text;
        }
        else
        {
            MajorCity.Text = CityTextBox.Text;
        }
        if(selectIndex == 3)
            FindPrice();
        FillChart(new object(), new EventArgs());
        Session["PrevTab"] = int.Parse(selectIndex.ToString());
        AdTabStrip.Tabs[selectIndex].Enabled = true;
        AdTabStrip.Tabs[selectIndex].Selected = true;
        AdTabStrip.MultiPage.SelectedIndex = selectIndex;
        AdTabStrip.SelectedTab.TabIndex = selectIndex;
        AdTabStrip.SelectedIndex = selectIndex;
        AdTabStrip.TabIndex = selectIndex;
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

    protected void SuggestCategoryClick(object sender, EventArgs e)
    {
        if (CategoriesTextBox.Text.Trim() != "")
        {
            HttpCookie cookie = Request.Cookies["BrowserDate"];

            DateTime isn = DateTime.Now;

            if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
                isn = DateTime.Now;
            DateTime isNow = isn;
            Data dat = new Data(isn); MessagePanel.Visible = true;
            CategoriesTextBox.Text = dat.stripHTML(CategoriesTextBox.Text.Trim());
            YourMessagesLabel.Text = "Your category '" + CategoriesTextBox.Text +
                "' has been suggested. We'll send you an update when it has been approved.";


            CategoriesTextBox.Text = dat.StripHTML_LeaveLinks(CategoriesTextBox.Text);

            DataSet dsUser = dat.GetData("SELECT EMAIL, UserName FROM USERS WHERE User_ID=" +
                Session["User"].ToString());

            dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["categoryemail"],
                    System.Configuration.ConfigurationManager.AppSettings["emailName"],
                System.Configuration.ConfigurationManager.AppSettings["categoryemail"].ToString(),
                "Category has been suggested from 'post-bulletin'. The user ID who suggested " +
                "the category is userID: '" + Session["User"].ToString() + "'. The category suggestion is '" +
                CategoriesTextBox.Text + "'", "Hippo Happenings category suggestion");

            CategoriesTextBox.Text = "";
        }
        else
        {
            YourMessagesLabel.Text = "Please include the category.";
            MessagePanel.Visible = true;
        }
    }
    
    protected void ChangeState(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn); DataSet ds = dat.GetData("SELECT * FROM State WHERE country_id=" + CountryDropDown.SelectedValue);

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

    private static ImageCodecInfo GetEncoderInfo(string mimeType)
    {
        // Get image codecs for all image formats 
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

        // Find the correct image codec
        ImageCodecInfo a = codecs[0];
        for (int i = 0; i < codecs.Length; i++)
        {
            if (codecs[i].MimeType == mimeType)
            {
                return codecs[i];

            }
            if (codecs[i].MimeType == "jpeg")
                a = codecs[i];
        }
        return a;
    }

    private void SaveThumbnail(System.Drawing.Image image, bool isRotator, string path, string typeS)
    {
        int width = 410;
        int height = 250;

        if (!isRotator)
        {
            width = 198;
            height = 250;
        }
        int newHeight = 0;
        int newIntWidth = 0;

        float newFloatHeight = 0.00F;
        float newFloatWidth = 0.00F;

       
            //if image height is less than resize height
        if (height >= image.Height)
        {
            //leave the height as is
            newHeight = image.Height;

            if (width >= image.Width)
            {
                newIntWidth = image.Width;
                newFloatWidth = image.Width;
            }
            else
            {
                newIntWidth = width;

                double theDivider = double.Parse(image.Width.ToString()) / double.Parse(newIntWidth.ToString());
                double newDoubleHeight = double.Parse(newHeight.ToString());
                newDoubleHeight = double.Parse(height.ToString()) / theDivider;
                newHeight = (int)newDoubleHeight;
                newFloatHeight = (float)newDoubleHeight;
            }
        }
        //if image height is greater than resize height...resize it
        else
        {
            //make height equal to the requested height.
            newHeight = height;
            newFloatHeight = height;
            //get the ratio of the new height/original height and apply that to the width
            double theDivider = double.Parse(image.Height.ToString()) / double.Parse(newHeight.ToString());
            double newDoubleWidth = double.Parse(newIntWidth.ToString());
            newDoubleWidth = double.Parse(image.Width.ToString()) / theDivider;
            newIntWidth = (int)newDoubleWidth;
            newFloatWidth = (float)newDoubleWidth;
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
                newFloatHeight = (float)newDoubleHeight;
            }
        }
       

        //if (quality < 0 || quality > 100)
        //    throw new ArgumentOutOfRangeException("quality must be between 0 and 100.");


        //// Encoder parameter for image quality 
        //EncoderParameter qualityParam =
        //    new EncoderParameter(Encoder.Quality, 100);
        //// Jpeg image codec 
        //ImageCodecInfo jpegCodec = GetEncoderInfo(typeS);

        //EncoderParameters encoderParams = new EncoderParameters(1);
        //encoderParams.Param[0] = qualityParam;

        System.Drawing.Bitmap bmpResized = new System.Drawing.Bitmap(image, newIntWidth, newHeight);


        //System.Drawing.Image thumbnail = image.GetThumbnailImage(newIntWidth, newHeight,
        //    new System.Drawing.Image.GetThumbnailImageAbort(EmptyCallBack), IntPtr.Zero);
        //SaveJpeg(path, thumbnail, 10, typeS);



        bmpResized.Save(path);
        //thumbnail.Save(path);
    }

    public static void SaveJpeg(string path, System.Drawing.Image img, int quality, string typeS)
    {
        if (quality < 0 || quality > 100)
            throw new ArgumentOutOfRangeException("quality must be between 0 and 100.");


        // Encoder parameter for image quality 
        EncoderParameter qualityParam =
            new EncoderParameter(Encoder.Quality, quality);
        // Jpeg image codec 
        ImageCodecInfo jpegCodec = GetEncoderInfo(typeS);

        EncoderParameters encoderParams = new EncoderParameters(1);
        encoderParams.Param[0] = qualityParam;

        img.Save(path, jpegCodec, encoderParams);
    }

    private bool EmptyCallBack()
    {
        return false;
    }

    /// <summary>
    /// Method for clearing internet cache through code
    /// </summary>
    /// <param name="folder">Directory to empty</param>
    private void EmptyCacheFolder(DirectoryInfo folder)
    {
        //loop through all the files in the folder provided
        foreach (FileInfo file in folder.GetFiles())
        {
            //delete each file
            file.Delete();
        }
        //now we loop through all the sub directories in the directory provided
        foreach (DirectoryInfo subfolder in folder.GetDirectories())
        {
            //recursively delete all files and folders
            //in each sub directory
            EmptyCacheFolder(subfolder);
        }
    }

    /// <summary>
    /// Method passing the "internet cache" folder information
    /// to EmptyCacheFolder for emptying IE cache
    /// </summary>
    /// <returns></returns>
    public bool ClearCache()
    {
        //variable to hold our status
        bool isEmpty;
        try
        {
            //call EmptyCacheFolder passing the default internet cache
            //folder
            EmptyCacheFolder(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache)));
            //successful so return true
            isEmpty = true;
        }
        catch
        {
            //failed
            isEmpty = false;
        }
        //return status
        return isEmpty;
    }

    protected void InsertBreak(object sender, EventArgs e)
    {
        DescriptionTextBox.Content += "<br/>";
    }

    protected void ChangeFeaturedText(object sender, EventArgs e)
    {
        if (TemplateRadioList.SelectedValue == "1")
        {
            FeaturedTextLiteral.Text = "max 250 characters &nbsp;&nbsp;&nbsp;";
            SummaryTextBox.Attributes.Add("onkeyup", "CountChars()");
            FeaturedTextPanel.Visible = true;
            FeaturedTextNotAllowedPanel.Visible = false;
        }
        else if (TemplateRadioList.SelectedValue == "2")
        {
            FeaturedTextLiteral.Text = "max 100 characters &nbsp;&nbsp;&nbsp;";
            SummaryTextBox.Attributes.Add("onkeyup", "CountChars100()");
            FeaturedTextPanel.Visible = true;
            FeaturedTextNotAllowedPanel.Visible = false;
        }
        else
        {
            FeaturedTextPanel.Visible = false;
            FeaturedTextNotAllowedPanel.Visible = true;
        }
    }

    protected void CheckedFeatured(object sender, EventArgs e)
    {
        Session["Featured"] = true;
        FeaturePanel.Visible = true;
    }

    protected void UnCheckedFeatured(object sender, EventArgs e)
    {
        Session["Featured"] = false;
        FeaturePanel.Visible = false;
        ClearFeatured();
        FillChart(new object(), new EventArgs());
        OnwardsIT(true, AdTabStrip.SelectedIndex);
    }

    protected void ClearFeatured()
    {
        ListItemCollection col = new ListItemCollection();

        foreach (ListItem item in FeatureDatesListBox.Items)
        {
            col.Add(item);
        }

        int a = col.Count;
        for (int i = 0; i < a; i++)
        {
            if (col[i].Value != "Disabled")
            {
                FeatureDatesListBox.Items.Remove(col[i]);
            }
        }

        col = new ListItemCollection();

        foreach (ListItem item in SearchTermsListBox.Items)
        {
            col.Add(item);
        }

        a = col.Count;
        for (int i = 0; i < a; i++)
        {
            SearchTermsListBox.Items.Remove(col[i]);
        }
    }

    protected void AddFeaturedDates(object sender, EventArgs e)
    {
        try
        {
            string troubleTerms = "";
            bool lassThan7Enabled = true;
            int count = 0;
            foreach (ListItem item2 in FeatureDatesListBox.Items)
            {
                if (item2.Value != "Disabled")
                {
                    count++;
                    if (count == 7)
                    {
                        lassThan7Enabled = false;
                        break;
                    }
                }
            }
            if (FeaturedDatePicker.SelectedDate != null && lassThan7Enabled)
            {
                if (!FeatureDatesListBox.Items.Contains(new ListItem(FeaturedDatePicker.SelectedDate.Value.ToShortDateString()))
                    && !FeatureDatesListBox.Items.Contains(new ListItem(FeaturedDatePicker.SelectedDate.Value.ToShortDateString(), "Disabled")))
                {

                    if (!CheckFor4Bulletins(false, FeaturedDatePicker.SelectedDate.Value.ToShortDateString(), ref troubleTerms))
                    {
                        ListItem item = new ListItem(FeaturedDatePicker.SelectedDate.Value.ToShortDateString());
                        FeatureDatesListBox.Items.Add(item);
                        FillChart(new object(), new EventArgs());
                    }
                    else
                    {
                        TermsErrorLabel.Text = "There are already too many featured bulletins " +
                        "with the search term and date combination you are trying to add. " +
                        "The combination in question is: " + troubleTerms;
                    }
                }
                else
                {
                    TermsErrorLabel.Text = "Your bulletin is already featured for this date.";
                }
            }
            else
            {
                if (!lassThan7Enabled)
                    TermsErrorLabel.Text = "You can purchase 7 feature days at a time. You can always purchase more days by visiting the page again.";
            }
        }
        catch (Exception ex)
        {
            DaysErrorLabel.Text = ex.ToString();
        }
    }

    protected void RemoveFeaturedDates(object sender, EventArgs e)
    {
        if (FeatureDatesListBox.SelectedItem != null)
        {

            if (FeatureDatesListBox.SelectedItem.Value != "Disabled")
            {
                FeatureDatesListBox.Items.Remove(FeatureDatesListBox.SelectedItem);
                FillChart(new object(), new EventArgs());
            }
        }
    }

    protected void AddSearchTerm(object sender, EventArgs e)
    {
        TermsErrorLabel.Text = "";
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


        TermsBox.Text = dat.stripHTML(TermsBox.Text);
        string troubleTerms = "";
        if (TermsBox.Text.Trim() != "")
        {
            if (!TermsBox.Text.Contains(" "))
            {
                if (!SearchTermsListBox.Items.Contains(new ListItem(TermsBox.Text.Trim())))
                {
                    if (!CheckFor4Bulletins(true, TermsBox.Text.Trim(), ref troubleTerms))
                    {
                        SearchTermsListBox.Items.Add(new ListItem(TermsBox.Text.Trim()));
                        FillChart(new object(), new EventArgs());
                    }
                    else
                    {
                        TermsErrorLabel.Text = "There are already too many featured bulletins " +
                            "with the search term and date combination you are trying to add. " +
                            "The combination in question is: " + troubleTerms;
                    }
                }
                else
                {
                    TermsErrorLabel.Text = "You can only add the same search term once.";
                }
            }
            else
            {
                TermsErrorLabel.Text = "Search terms cannot contain spaces.";
            }
        }
        else
        {
            TermsErrorLabel.Text = "Please include a search term.";
        }
    }

    protected bool CheckFor4Bulletins(bool isTerm, string TermOrDateString, ref string troubleDateOrTerm)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn);
        DataView dv;
        bool termExists = false;
        foreach (ListItem itemDate in FeatureDatesListBox.Items)
        {
            //Check if the current term we're trying to bring in would exceed 4 with any date
            if (isTerm)
            {
                dv = dat.GetDataDV("SELECT * FROM AdSearchTerms WHERE SearchDate = '" + itemDate.Text +
                    "' AND SearchTerms LIKE '%;" + TermOrDateString + ";%'");

                if (dv.Count > 3)
                {
                    troubleDateOrTerm = " search term '" + TermOrDateString + "' and date '" + itemDate.Text + "' ";
                    termExists = true;
                    break;
                }
            }

            foreach (ListItem itemTerm in SearchTermsListBox.Items)
            {
                dv = dat.GetDataDV("SELECT * FROM AdSearchTerms WHERE SearchDate = '" + itemDate.Text +
                    "' AND SearchTerms LIKE '%;" + itemTerm.Text + ";%'");

                if (dv.Count > 3)
                {
                    troubleDateOrTerm = " search term '" + itemTerm.Text + "' and date '" + itemDate.Text + "' ";
                    termExists = true;
                    break;
                }

                //Check if the current date we're trying to bring in would exceed 4 with any term
                if (!isTerm)
                {
                    dv = dat.GetDataDV("SELECT * FROM AdSearchTerms WHERE SearchDate = '" + TermOrDateString +
                    "' AND SearchTerms LIKE '%;" + itemTerm.Text + ";%'");

                    if (dv.Count > 4)
                    {
                        troubleDateOrTerm = " search term '" + itemTerm.Text + "' and date '" + TermOrDateString + "' ";
                        termExists = true;
                        break;
                    }
                }
            }

            if (termExists)
                break;
        }

        return termExists;
    }

    protected void RemoveSearchTerm(object sender, EventArgs e)
    {
        if (SearchTermsListBox.SelectedItem != null)
        {
            SearchTermsListBox.Items.Remove(SearchTermsListBox.SelectedItem);
            FillChart(new object(), new EventArgs());
        }
    }

    protected void FillChart(object sender, EventArgs e)
    {
        try
        {
            PricingLiteral.Text = "<td align='center'>0</td><td align='center'>$0</td><td align='center'>$0</td><td align='center'>$0</td><td align='center'>$0</td>";

            bool hasNewFeaturedItems = false;
            int daysCount = 0;

            foreach (ListItem item in FeatureDatesListBox.Items)
            {
                if (item.Value != "Disabled")
                {
                    hasNewFeaturedItems = true;
                    daysCount++;
                }
            }

            if (hasNewFeaturedItems)
            {
                HttpCookie cookie = Request.Cookies["BrowserDate"];

                DateTime isn = DateTime.Now;

                if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
                    isn = DateTime.Now;
                DateTime isNow = isn;
                Data dat = new Data(isn);
                DataView dvChart = dat.GetDataDV("SELECT * FROM StandardBulletinPricing");

                string majorCityID = MajorCityDrop.SelectedValue;
                
                DataView dvCountMembers;
                string zips = "";
                if (CountryDropDown.SelectedValue == "223")
                {
                    //Get Major City
                    zips = dat.GetDataDV("SELECT AllZips FROM MajorCities WHERE ID=" + majorCityID)[0]["AllZips"].ToString();

                    dvCountMembers = dat.GetDataDV("SELECT COUNT(*) AS Count FROM UserPreferences WHERE MajorCity = " +
                        majorCityID);

                }
                else
                {
                    dvCountMembers = dat.GetDataDV("SELECT COUNT(*) AS Count FROM UserPreferences WHERE CatCity = '" +
                    CityTextBox.Text.Trim().Replace("'", "''") + "' AND CatCountry = " + CountryDropDown.SelectedValue +
                    " AND CatState = '" + StateDropDown.SelectedItem.Text + "'");

                }

                int indexToLookAt = daysCount - 1;

                if (daysCount > 3)
                    indexToLookAt = 3;

                decimal memberCap = decimal.Round(decimal.Parse(dvChart[indexToLookAt]["PerMemberCap"].ToString()), 3, MidpointRounding.AwayFromZero);
                decimal priceForMembers = 0.00M;

                if (decimal.Parse(dvChart[indexToLookAt]["PerMemberPricing"].ToString()) * decimal.Parse(dvCountMembers[0]["Count"].ToString()) > memberCap)
                {
                    priceForMembers = memberCap;
                }
                else
                {
                    priceForMembers = decimal.Round(decimal.Parse(dvChart[indexToLookAt]["PerMemberPricing"].ToString()) *
                        decimal.Parse(dvCountMembers[0]["Count"].ToString()), 3, MidpointRounding.AwayFromZero);
                }

                DataView dvCountEvents;
                DateTime dateDate;
                decimal eventCap = decimal.Round(decimal.Parse(dvChart[indexToLookAt]["PerBulletinCap"].ToString()), 3, MidpointRounding.AwayFromZero);
                decimal subtractionForEvents = 0.00M;
                decimal total = 0.00M;

                PricingLiteral.Text = "";

                foreach (ListItem item in FeatureDatesListBox.Items)
                {
                    if (item.Value != "Disabled")
                    {
                        dateDate = DateTime.Parse(item.Text);
                        if (CountryDropDown.SelectedValue == "223")
                        {
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Ads WHERE Featured='True' AND CatCity ='" + MajorCityDrop.SelectedItem.Text + "'" +
                                "AND DatesOfAd LIKE '%;" + dateDate.Month.ToString() + "/" +
                        dateDate.Day.ToString() + "/" + dateDate.Year.ToString() + ";%'");

                        }
                        else
                        {
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Ads WHERE Featured='True' AND CatCity = '" +
                        CityTextBox.Text.Trim().Replace("'", "''") + "' AND CatCountry = " + CountryDropDown.SelectedValue +
                        " AND CatState = '" + StateDropDown.SelectedItem.Text + "' AND DatesOfAd LIKE '%;" + dateDate.Month.ToString() + "/" +
                        dateDate.Day.ToString() + "/" + dateDate.Year.ToString() + ";%'");
                        }

                        

                        if (decimal.Parse(dvChart[indexToLookAt]["PerBulletinPricing"].ToString()) *
                            decimal.Parse(dvCountEvents[0]["Count"].ToString()) > eventCap)
                        {
                            subtractionForEvents = eventCap;
                        }
                        else
                        {
                            subtractionForEvents = decimal.Round(decimal.Parse(dvChart[indexToLookAt]["PerBulletinPricing"].ToString()) *
                                decimal.Parse(dvCountEvents[0]["Count"].ToString()), 3, MidpointRounding.AwayFromZero);
                        }

                        PricingLiteral.Text += "<tr><td align=\"center\" >" + item.Text + "</td><td align=\"center\">" +
                            decimal.Round(decimal.Parse(dvChart[indexToLookAt]["StandardBulletinPricing"].ToString()), 3, MidpointRounding.AwayFromZero).ToString() +
                            "</td><td align=\"center\">$" + priceForMembers.ToString() + "</td><td align=\"center\">-$" + subtractionForEvents.ToString() + "</td>" +
                            "<td align=\"center\">$" + decimal.Round((decimal.Parse(dvChart[indexToLookAt]["StandardBulletinPricing"].ToString()) +
                            priceForMembers - subtractionForEvents), 3, MidpointRounding.AwayFromZero).ToString() + "</td></tr>";
                        total += decimal.Round((decimal.Parse(dvChart[indexToLookAt]["StandardBulletinPricing"].ToString()) +
                            priceForMembers - subtractionForEvents), 3, MidpointRounding.AwayFromZero);
                    }
                }

                decimal searchTermTotal = 0.05M * SearchTermsListBox.Items.Count * daysCount;
                NumSearchTerms.Text = searchTermTotal.ToString();
                TotalLabel.Text = decimal.Round((total + searchTermTotal), 2, MidpointRounding.AwayFromZero).ToString();

                if (decimal.Round((total + searchTermTotal), 2, MidpointRounding.AwayFromZero) > 0.00M)
                {
                    PaymentPanel.Visible = true;
                    PromoPanel.Visible = true;
                }
                else
                {
                    PaymentPanel.Visible = false;
                    PromoPanel.Visible = false;
                }
            }
            else
            {
                PaymentPanel.Visible = false;
                PromoPanel.Visible = false;
                TotalLabel.Text = "0.00";
            }
        }
        catch (Exception ex)
        {
            DaysErrorLabel.Text = ex.ToString();
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
                Data dat = new Data(isn); DataSet ds = dat.GetData("SELECT *, MC.ID AS MCID FROM MajorCities MC, State S WHERE MC.State=S.state_name " +
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

    protected void ChangeStateAction(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn);
        DataSet ds = dat.GetData("SELECT * FROM State WHERE country_id=" + BillingCountry.SelectedValue);

        if (BillingCountry.SelectedValue == "223")
        {
            CardTypeDropDown.Items.Clear();
            CardTypeDropDown.Items.Add(new ListItem("Visa", "Visa"));
            CardTypeDropDown.Items.Add(new ListItem("MasterCard", "MasterCard"));
            CardTypeDropDown.Items.Add(new ListItem("Discover", "Discover"));
            CardTypeDropDown.Items.Add(new ListItem("American Express", "Amex"));
        }
        else if (BillingCountry.SelectedValue == "222")
        {
            CardTypeDropDown.Items.Clear();
            CardTypeDropDown.Items.Add(new ListItem("Visa, including Visa Electron and Visa Debit", "Visa"));
            CardTypeDropDown.Items.Add(new ListItem("MasterCard", "MasterCard"));
            CardTypeDropDown.Items.Add(new ListItem("Maestro, including Switch", "Maestro"));
            CardTypeDropDown.Items.Add(new ListItem("Solo", "Solo"));
        }
        else
        {
            CardTypeDropDown.Items.Clear();
            CardTypeDropDown.Items.Add(new ListItem("Visa", "Visa"));
            CardTypeDropDown.Items.Add(new ListItem("MasterCard", "MasterCard"));
        }

        bool isTextBox = false;
        if (ds.Tables.Count > 0)
            if (ds.Tables[0].Rows.Count > 0)
            {
                BillingStateDropDown.Visible = true;
                BillingStateTextBox.Visible = false;
                BillingStateDropDown.DataSource = ds;
                BillingStateDropDown.DataTextField = "state_2_code";
                BillingStateDropDown.DataValueField = "state_id";
                BillingStateDropDown.DataBind();
            }
            else
                isTextBox = true;
        else
            isTextBox = true;

        if (isTextBox)
        {
            BillingStateTextBox.Visible = true;
            BillingStateDropDown.Visible = false;
        }
    }

    protected void UpdateState(object sender, EventArgs e)
    {
        BillingStateTextBox.Text = BillingStateDropDown.SelectedItem.Text;
        FetUpdatePanel.Update();
    }

    protected void FindPrice()
    {
        DateTime isn = DateTime.Now;
        Data dat = new Data(isn);

        DataView dvChart = dat.GetDataDV("SELECT * FROM StandardBulletinPricing");

        string majorCityID = MajorCityDrop.SelectedValue;

        DataView dvCountMembers;
        string zips = "";
        if (CountryDropDown.SelectedValue == "223")
        {
            //Get Major City
            zips = dat.GetDataDV("SELECT AllZips FROM MajorCities WHERE ID=" + majorCityID)[0]["AllZips"].ToString();

            dvCountMembers = dat.GetDataDV("SELECT COUNT(*) AS Count FROM UserPreferences WHERE MajorCity = " +
                majorCityID);
        }
        else
        {
            dvCountMembers = dat.GetDataDV("SELECT COUNT(*) AS Count FROM UserPreferences WHERE CatCity = '" +
            CityTextBox.Text.Trim().Replace("'", "''") + "' AND CatCountry = " + CountryDropDown.SelectedValue +
            " AND CatState = '" + StateDropDown.SelectedItem.Text + "'");
        }

        int indexToLookAt = 0;

        decimal memberCap = decimal.Round(decimal.Parse(dvChart[indexToLookAt]["PerMemberCap"].ToString()), 3, MidpointRounding.AwayFromZero);
        decimal priceForMembers = 0.00M;

        if (decimal.Parse(dvChart[indexToLookAt]["PerMemberPricing"].ToString()) * decimal.Parse(dvCountMembers[0]["Count"].ToString()) > memberCap)
        {
            priceForMembers = memberCap;
        }
        else
        {
            priceForMembers = decimal.Round(decimal.Parse(dvChart[indexToLookAt]["PerMemberPricing"].ToString()) *
                decimal.Parse(dvCountMembers[0]["Count"].ToString()), 3, MidpointRounding.AwayFromZero);
        }

        DataView dvCountEvents;
        DateTime dateDate;
        decimal eventCap = decimal.Round(decimal.Parse(dvChart[indexToLookAt]["PerBulletinCap"].ToString()), 3, MidpointRounding.AwayFromZero);
        decimal subtractionForEvents = 0.00M;
        decimal total = 0.00M;

        dateDate = DateTime.Now;
        if (CountryDropDown.SelectedValue == "223")
        {
            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Ads WHERE Featured='True' AND CatCity ='" + MajorCityDrop.SelectedItem.Text + "'" +
                "AND DatesOfAd LIKE '%;" + dateDate.Month.ToString() + "/" +
                dateDate.Day.ToString() + "/" + dateDate.Year.ToString() + ";%'");

        }
        else
        {
            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Ads WHERE Featured='True' AND CatCity = '" +
                CityTextBox.Text.Trim().Replace("'", "''") + "' AND CatCountry = " + CountryDropDown.SelectedValue +
                " AND CatState = '" + StateDropDown.SelectedItem.Text + "' AND DatesOfAd LIKE '%;" + dateDate.Month.ToString() + "/" +
                dateDate.Day.ToString() + "/" + dateDate.Year.ToString() + ";%'");
        }



        if (decimal.Parse(dvChart[indexToLookAt]["PerBulletinPricing"].ToString()) *
            decimal.Parse(dvCountEvents[0]["Count"].ToString()) > eventCap)
        {
            subtractionForEvents = eventCap;
        }
        else
        {
            subtractionForEvents = decimal.Round(decimal.Parse(dvChart[indexToLookAt]["PerBulletinPricing"].ToString()) *
                decimal.Parse(dvCountEvents[0]["Count"].ToString()), 3, MidpointRounding.AwayFromZero);
        }

        total += decimal.Round((decimal.Parse(dvChart[indexToLookAt]["StandardBulletinPricing"].ToString()) +
            priceForMembers - subtractionForEvents), 3, MidpointRounding.AwayFromZero);


        string allTotal = decimal.Round(total, 2, MidpointRounding.AwayFromZero).ToString();

        PriceLiteral.Text = allTotal;
    }

    protected void ValidateCode(object sender, EventArgs e)
    {
        Data dat = new Data(DateTime.Now);
        PromoCodeTextBox.Text = dat.stripHTML(PromoCodeTextBox.Text.Trim());

        if (PromoCodeTextBox.Text != "")
        {
            string promocode = PromoCodeTextBox.Text;
            DataView dv = dat.GetDataDV("SELECT * FROM PromoCodes WHERE PromoCode = '" + promocode + "'");
            if (dv.Count > 0)
            {
                PaymentPanel.Visible = false;
                PromoErrorLabel.Text = "You're good to go!";
            }
            else
            {
                PaymentPanel.Visible = true;
                PromoErrorLabel.Text = "This code is not valid.";
            }
        }
    }
}
