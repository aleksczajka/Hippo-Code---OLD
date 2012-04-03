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
using System.Drawing.Imaging;

public partial class BlogEvent : Telerik.Web.UI.RadAjaxPage
{
    private string UserName;
    //protected SessionPageStatePersister PageStatePersister
    //{
    //    get { return new SessionPageStatePersister(this); }
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        Session["RedirectTo"] = Request.Url.AbsoluteUri;
        Session["Featured"] = false;
        if (Session["User"] == null)
        {
            Response.Redirect("~/login");
        }

        //if (!Request.IsSecureConnection)
        //{
        //    string absoluteUri = Request.Url.AbsoluteUri;
        //    Response.Redirect(absoluteUri.Replace("http://", "https://"), true);
        //}

        HtmlLink lk = new HtmlLink();
        HtmlHead head = (HtmlHead)Page.Header;
        lk.Attributes.Add("rel", "canonical");
        lk.Href = "https://hippohappenings.com/blog-event";
        head.Controls.AddAt(0, lk);

        HtmlMeta hm = new HtmlMeta();
        HtmlMeta kw = new HtmlMeta();

        kw.Name = "keywords";
        kw.Content = "Blog local events happening in your area";

        head.Controls.AddAt(0, kw);

        hm.Name = "Description";
        hm.Content = "Blog local events & happenings in your neighborhood and community";
        head.Controls.AddAt(0, hm);

        

        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        MessageRadWindowManager.VisibleOnPageLoad = false;

        DateErrorLabel.Text = "";

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn);

        DataView dvUser = dat.GetDataDV("SELECT * FROM UserPreferences WHERE UserID=" + Session["User"].ToString());

        DataView dv = dat.GetDataDV("SELECT * FROM TermsAndConditions");
        Literal lit1 = new Literal();
        lit1.Text = dv[0]["Content"].ToString();
        TACTextBox.Controls.Add(lit1);

        #region Take Care of Buttons
        AddDateButton.SERVER_CLICK += AddDate;
        SubtractDateButton.SERVER_CLICK += SubtractDate;
        //DetailsOnwardsButton.SERVER_CLICK += Onwards;
        GoButton.SERVER_CLICK += GetVenues;
        //ImageButton9.SERVER_CLICK += Backwards;
        //CategoryOnwardButton.SERVER_CLICK += Onwards;
        //ImageButton12.SERVER_CLICK += Backwards;
        PostItButton.SERVER_CLICK += PostIt;
        //ImageButton2.SERVER_CLICK += Backwards;
        //DescriptionOnwardsButton.SERVER_CLICK += Onwards;
        DeleteSongButton.SERVER_CLICK += NixIt;
        ImageButton6.SERVER_CLICK += VideoUpload_Click;
        ImageButton7.SERVER_CLICK += YouTubeUpload_Click;
        //ImageButton5.SERVER_CLICK += PictureUpload_Click;
        PictureNixItButton.SERVER_CLICK += SliderNixIt;
        //ImageButton3.SERVER_CLICK += Backwards;
        //MediaOnwardsButton.SERVER_CLICK += Onwards;
        ImageButton11.SERVER_CLICK += SuggestCategoryClick;
        MusicUploadButton.SERVER_CLICK += MusicUpload_Click;
        BlueButton9.SERVER_CLICK += CheckedFeatured;
        BlueButton10.SERVER_CLICK += UnCheckedFeatured;
        BlueButton1.SERVER_CLICK += AddFeaturedDates;
        BlueButton4.SERVER_CLICK += RemoveFeaturedDates;
        BlueButton5.SERVER_CLICK += AddSearchTerm;
        BlueButton6.SERVER_CLICK += RemoveSearchTerm;
        BlueButton7.SERVER_CLICK += FillChart;
        //BlueButton2.SERVER_CLICK += Onwards;
        //BlueButton3.SERVER_CLICK += Backwards;
        #endregion


        #region Take Care of Feature Features
        //Limit feature calendar to 30 days
        FeaturedDatePicker.MinDate = DateTime.Now;
        FeaturedDatePicker.MaxDate = DateTime.Now.AddDays(30);

        foreach (ListItem item2 in FeatureDatesListBox.Items)
        {
            if (item2.Value == "Disabled")
                item2.Attributes.Add("class", "DisabledListItem");
        }
        #endregion

        string dateTod = DateTime.Now.ToShortDateString() + " 00:00:00";

        StartDateTimePicker.MinDate = DateTime.Parse(dateTod);
        EndDateTimePicker.MinDate = DateTime.Parse(dateTod);
        YourMessagesLabel.Text = "";
        MessagePanel.Visible = false;
        if (!IsPostBack)
        {
            DataSet dsCountries = dat.GetData("SELECT * FROM Countries");
            VenueCountry.DataSource = dsCountries;
            VenueCountry.DataTextField = "country_name";
            VenueCountry.DataValueField = "country_id";
            VenueCountry.DataBind();

            VenueCountry.SelectedValue = "223";
            ChangeVenueState("223");
            

            
            ExistingVenuePanel.Visible = true;

            privateCountryDropDown.DataSource = dsCountries;
            privateCountryDropDown.DataTextField = "country_name";
            privateCountryDropDown.DataValueField = "country_id";
            privateCountryDropDown.DataBind();

            privateCountryDropDown.SelectedValue = dvUser[0]["CatCountry"].ToString();
            ChangePrivateVenueState("223");
            if(privateStateDropDown.Visible)
                privateStateDropDown.SelectedValue = dvUser[0]["CatState"].ToString();
            else
                privateStateTextBox.Text = dvUser[0]["CatState"].ToString();
            cityTextBox.Text = dvUser[0]["CatCity"].ToString();
            

            //Take Care of Billing Locaion
            BillingCountry.DataSource = dsCountries;
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


            ShowVideoPictureLiteral.Text = "";

            Session.Remove("Featured");
            Session["Featured"] = null;

            Session["NewVenue"] = null;
            Session.Remove("NewVenue");

            Session["EventCategoriesSet"] = null;
            Session.Remove("EventCategoriesSet");
            

            PostItButton.SERVER_CLICK += PostIt;
            PostItButton.SERVER_CLICK -= ModifyIt;
            DataSet ds = new DataSet();



            try
            {
                if (Session["User"] != null)
                {
                    Session["EffectiveUserName"] = Session["UserName"].ToString();

                    if (Request.QueryString["edit"] != null)
                    {
                        if (bool.Parse(Request.QueryString["edit"].ToString()))
                        {
                            string IDi = Request.QueryString["ID"].ToString();

                            isEdit.Text = "True";
                            eventID.Text = IDi;
                            if (dat.HasEventPassed(IDi))
                            {
                                DataView dvName = dat.GetDataDV("SELECT * FROM Events WHERE ID=" + IDi);
                                Response.Redirect(dat.MakeNiceName(dvName[0]["Header"].ToString()) + "_" + IDi + "_Event");
                            }
                            fillEvent(IDi, true);
                            //PostItButton.Click -= new ImageClickEventHandler(PostIt);
                            //PostItButton.Click += new ImageClickEventHandler(ModifyIt);



                        }
                    }
                    else if (Request.QueryString["copy"] != null)
                    {
                        if (bool.Parse(Request.QueryString["copy"].ToString()))
                        {
                            string IDi = Request.QueryString["ID"].ToString();
                            fillEvent(IDi, false);
                            DateSelectionsListBox.Items.Clear();
                        }
                    }

                    UserName = Session["UserName"].ToString();
                    UserNameLabel.Text = UserName;
                }
                else
                {
                    Response.Redirect("~/login");
                }
            }
            catch (Exception ex)
            {

                MessagePanel.Visible = true;
                YourMessagesLabel.Text = ex.ToString();
                //Session.Abandon();
                //FormsAuthentication.SignOut();
                //Response.Redirect("~/enter-event");
            }
            //YourMessagesLabel.Text = "";
            //MessagePanel.Visible = false;

            
        }
        else
        {
            if (Session["LocationVenues"] != null)
                fillVenues((DataSet)Session["LocationVenues"]);


            if (Session["NewVenue"] != null)
            {
                TimeFrameDiv.InnerHtml = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + Session["NewVenue"].ToString())[0]["Name"].ToString();
            }
        }

        if (Session["User"] == null)
        {
            Response.Redirect("home");
        }
    }

    protected bool EnableOwnerPanel(ref bool ownerUpForGrabs)
    {
        bool isOwner = false;
        ownerUpForGrabs = false;
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn);
        if (Request.QueryString["ID"] != null)
        {
            DataSet dsVenue = dat.GetData("SELECT * FROM Events WHERE ID=" + Request.QueryString["ID"].ToString());
            if (dsVenue.Tables[0].Rows[0]["Owner"] != null)
            {
                if (dsVenue.Tables[0].Rows[0]["Owner"].ToString().Trim() != "")
                {
                    if (dsVenue.Tables[0].Rows[0]["Owner"].ToString() == Session["User"].ToString())
                        isOwner = true;
                }
                else
                {
                    ownerUpForGrabs = true;
                }
            }
            else
            {
                ownerUpForGrabs = true;
            }
        }
        else
        {
            ownerUpForGrabs = true;
        }

        if (ownerUpForGrabs)
        {

            OwnerPanel.Visible = true;
        }
        else
        {
            if (isOwner)
            {
                OwnerPanel.Visible = true;
                OwnerCheckBox.Checked = true;
            }
            else
            {
                OwnerPanel.Visible = false;
            }
        }

        return isOwner;
    }

    protected void GetNewVenue(object sender, EventArgs e)
    {

        if (Session["NewVenue"] != null)
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

                DateTime isn = DateTime.Now;

                if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
                    isn = DateTime.Now;
                DateTime isNow = isn;
                Data dat = new Data(isn);
                DataView rowV = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + Session["NewVenue"].ToString());

                string country = rowV[0]["Country"].ToString();
                VenueCountry.SelectedValue = country;
                ChangeVenueStateAction(VenueCountry, new EventArgs());
                if (VenueState.Visible)
                    VenueState.Items.FindByText(rowV[0]["State"].ToString()).Selected = true;
                else
                    VenueStateTextBox.Text = rowV[0]["State"].ToString();
                GetThoseVenues();
                TimeFrameDiv.InnerHtml = rowV[0]["Name"].ToString();

                ChangeSelectedTab();
            }
            catch (Exception ex)
            {
                YourMessagesLabel.Text = ex.ToString();
                MessagePanel.Visible = true;
            }
        }
    }

    protected void fillEvent(string ID, bool isedit)
    {
        try
        {
            HttpCookie cookie = Request.Cookies["BrowserDate"];

            DateTime isn = DateTime.Now;

            if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
                isn = DateTime.Now;
            DateTime isNow = isn;
            Data dat = new Data(isn); DataSet dsEvent = dat.GetData("SELECT E.Owner AS TheOwner, * FROM Events E, Venues V, Event_Occurance EO WHERE E.Venue=V.ID AND EO.EventID=E.ID AND E.ID=" + ID + " ORDER BY EO.DateTimeStart ASC ");
            EventNameTextBox.Text = dsEvent.Tables[0].Rows[0]["Header"].ToString();

            DataView dv = new DataView(dsEvent.Tables[0], "", "", DataViewRowState.CurrentRows);

            bool isPrivate = bool.Parse(dv[0]["Private"].ToString());

            if (isedit)
            {
                nameLabel.Text = "<h1>You are submitting changes for Event: " + dsEvent.Tables[0].Rows[0]["Header"].ToString()+"</h1>";
            }
            Session["EffectiveUserName"] = dsEvent.Tables[0].Rows[0]["UserName"].ToString();

            if (dsEvent.Tables[0].Rows[0]["MinPrice"] != null)
            {
                if (dsEvent.Tables[0].Rows[0]["MinPrice"].ToString() != "")
                {
                    MinTextBox.Text = dsEvent.Tables[0].Rows[0]["MinPrice"].ToString();
                }
            }

            if (dsEvent.Tables[0].Rows[0]["MaxPrice"] != null)
            {
                if (dsEvent.Tables[0].Rows[0]["MaxPrice"].ToString() != "")
                {
                    MaxTextBox.Text = dsEvent.Tables[0].Rows[0]["MaxPrice"].ToString();
                }
            }

            if (isPrivate)
                LocaleRadioButtonList.SelectedValue = "1";
            else
                LocaleRadioButtonList.SelectedValue = "0";

            if (!isPrivate)
                VenueCountry.Items.FindByValue(dsEvent.Tables[0].Rows[0]["Country"].ToString()).Selected = true;
            else
                privateCountryDropDown.Items.FindByValue(dsEvent.Tables[0].Rows[0]["Country"].ToString()).Selected = true;

            DataSet dsCountry = dat.GetData("SELECT * FROM State WHERE country_id=" + dsEvent.Tables[0].Rows[0]["Country"].ToString());
            //ChangeVenueState(dsEvent.Tables[0].Rows[0]["Country"].ToString());
            if (dsCountry.Tables.Count > 0)
                if (dsCountry.Tables[0].Rows.Count > 0)
                {
                    if (isPrivate)
                    {
                        privateStateDropDown.Items.Clear();
                        privateStateDropDown.DataSource = dsCountry;
                        privateStateDropDown.DataTextField = "state_2_code";
                        privateStateDropDown.DataValueField = "state_id";
                        privateStateDropDown.DataBind();

                        privateStateDropDown.Items.FindByText(dsEvent.Tables[0].Rows[0]["State"].ToString()).Selected = true;
                    }
                    else
                    {
                        VenueState.Items.Clear();
                        VenueState.DataSource = dsCountry;
                        VenueState.DataTextField = "state_2_code";
                        VenueState.DataValueField = "state_id";
                        VenueState.DataBind();

                        VenueState.Items.FindByText(dsEvent.Tables[0].Rows[0]["State"].ToString()).Selected = true;
                    }
                }
                else
                {
                    if (isPrivate)
                    {
                        privateStateTextBox.Visible = true;
                        privateStateTextBox.Text = dsEvent.Tables[0].Rows[0]["State"].ToString();
                    }
                    else
                    {
                        VenueStateTextBox.Visible = true;
                        VenueStateTextBox.Text = dsEvent.Tables[0].Rows[0]["State"].ToString();
                    }
                }
            else
            {
                if (isPrivate)
                {
                    privateStateTextBox.Visible = true;
                    privateStateTextBox.Text = dsEvent.Tables[0].Rows[0]["State"].ToString();
                }
                else
                {
                    VenueStateTextBox.Visible = true;
                    VenueStateTextBox.Text = dsEvent.Tables[0].Rows[0]["State"].ToString();
                }
            }

            if (isPrivate)
            {
                addressTextBox.Text = dsEvent.Tables[0].Rows[0]["Address"].ToString();
                cityTextBox.Text = dsEvent.Tables[0].Rows[0]["City"].ToString();
                ZipTextBox.Text = dsEvent.Tables[0].Rows[0]["Zip"].ToString();
            }


            //check if there is already an owner of this venue
            bool ownerUpForGrabs = false;
            bool isOwner = EnableOwnerPanel(ref ownerUpForGrabs);

            if (!ownerUpForGrabs && !isOwner)
            {
                AddDateButton.ENABLED = false;

                SubtractDateButton.ENABLED = false;
                //StartDateTimePicker.Enabled = false;
                //EndDateTimePicker.Enabled = false;
            }

            if (!isPrivate)
                GetThoseVenues();

            string state = "";

            if (isPrivate)
            {
                if (privateStateDropDown.Visible)
                    state = privateStateDropDown.SelectedItem.Text;
                else
                    state = privateStateTextBox.Text;
            }
            else
            {
                if (VenueState.Visible)
                    state = VenueState.SelectedItem.Text;
                else
                    state = VenueStateTextBox.Text;
            }

            SqlDbType[] types = { SqlDbType.NVarChar };
            object[] data = { state };

            if (!isPrivate)
            {
                DataView dvV = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + dsEvent.Tables[0].Rows[0]["Venue"].ToString());

                //DataSet ds = dat.GetDataWithParemeters("SELECT * FROM Venues WHERE Country=" +
                //    dvV[0]["Country"].ToString() + " AND State='" + dvV[0]["State"].ToString() +
                //    "' ORDER BY Name ASC", types, data);


                //Session["LocationVenues"] = ds;

                //fillVenues(ds);

                Session["NewVenue"] = dvV[0]["ID"].ToString();


                TimeFrameDiv.InnerHtml = dvV[0]["Name"].ToString();
            }

           

            //StartDateTimePicker.DbSelectedDate = dsEvent.Tables[0].Rows[0]["DateTimeStart"].ToString();
            //EndDateTimePicker.DbSelectedDate = dsEvent.Tables[0].Rows[0]["DateTimeEnd"].ToString();

            bool isEdit = false;
            DateTime startDate;
            DateTime endDate;
            if (Request.QueryString["edit"] != null)
                isEdit = bool.Parse(Request.QueryString["edit"].ToString());

            
                DateSelectionsListBox.Items.Clear();
                for (int i = 0; i < dsEvent.Tables[0].Rows.Count; i++)
                {
                    startDate = new DateTime();
                    startDate = DateTime.Parse(dsEvent.Tables[0].Rows[i]["DateTimeStart"].ToString());
                    endDate = new DateTime();
                    endDate = DateTime.Parse(dsEvent.Tables[0].Rows[i]["DateTimeEnd"].ToString());
                    DateSelectionsListBox.Items.Add(startDate.Month.ToString() + "/" + startDate.Day.ToString() +
                        "/" + startDate.Year.ToString() + " " + startDate.TimeOfDay.Hours.ToString() + ":" +
                        startDate.TimeOfDay.Minutes.ToString() + " -- " +
                        endDate.Month.ToString() + "/" + endDate.Day.ToString() +
                        "/" + endDate.Year.ToString() + " " + endDate.TimeOfDay.Hours.ToString() + ":" +
                        endDate.TimeOfDay.Minutes.ToString());
                }

            
            DescriptionTextBox.Content = dsEvent.Tables[0].Rows[0]["Content"].ToString();
            //ShortDescriptionTextBox.Text = dsEvent.Tables[0].Rows[0]["ShortDescription"].ToString();

            string mediaCategory = dsEvent.Tables[0].Rows[0]["mediaCategory"].ToString();
            string youtube = dsEvent.Tables[0].Rows[0]["YouTubeVideo"].ToString();
            ListItem newListItem;
            switch (mediaCategory)
            {
                case "1":
                    MainAttractionCheck.Checked = true;
                    MainAttractionPanel.Enabled = true;
                    MainAttractionPanel.Visible = true;

                    char[] delim4 = { ';' };
                    char[] delim = { '\\' };
                    string[] youtokens = youtube.Split(delim4);
                    if (youtokens.Length > 0)
                    {
                        UploadedVideosAndPics.Visible = true;
                        if (isOwner || ownerUpForGrabs)
                            PictureNixItButton.Visible = true;
                        for (int i = 0; i < youtokens.Length; i++)
                        {
                            if (youtokens[i].Trim() != "")
                            {
                                newListItem = new ListItem("You Tube ID: " + youtokens[i], youtokens[i]);
                                if (!isOwner)
                                    newListItem.Enabled = false;
                                PictureCheckList.Items.Add(newListItem);
                            }
                        }
                    }
                    DataView dvEC = dat.GetDataDV("SELECT * FROM Event_Slider_Mapping WHERE EventID=" + ID);
                    if (dvEC.Count > 0 && (isOwner || ownerUpForGrabs))
                        PictureNixItButton.Visible = true;
                    
                    if (dvEC.Count > 0)
                    {
                        UploadedVideosAndPics.Visible = true;
                        for (int i = 0; i < dvEC.Count; i++)
                        {
                            if (i == 0)
                                UploadedVideosAndPics.Visible = true;

                            if (bool.Parse(dvEC[i]["ImgPathAbsolute"].ToString()))
                            {
                                newListItem = new ListItem(dvEC[i]["RealPictureName"].ToString(), dvEC[i]["PictureName"].ToString());
                                PictureCheckList.Items.Add(newListItem);
                            }
                            else
                            {
                                newListItem = new ListItem(dvEC[i]["RealPictureName"].ToString(), dvEC[i]["PictureName"].ToString());
                                if (!isOwner)
                                    newListItem.Enabled = false;
                                PictureCheckList.Items.Add(newListItem);
                            }
                        }
                    }
                    break;
                default: break;
            }

            DataSet dsMusic = dat.GetData("SELECT * FROM Event_Song_Mapping WHERE EventID=" + dsEvent.Tables[0].Rows[0]["ID"].ToString());

            if (dsMusic.Tables.Count > 0)
                if (dsMusic.Tables[0].Rows.Count > 0)
                {

                    MusicPanel.Enabled = true;

                    if (isOwner || ownerUpForGrabs)
                        DeleteSongButton.Visible = true;
                    for (int i = 0; i < dsMusic.Tables[0].Rows.Count; i++)
                    {
                        ListItem newItem = new ListItem(dsMusic.Tables[0].Rows[i]["SongTitle"].ToString(),
                            dsMusic.Tables[0].Rows[i]["SongName"].ToString());
                        if (!isOwner)
                            newItem.Enabled = false;
                        SongCheckList.Items.Add(newItem);
                    }


                }

            FeatureDatesListBox.Items.Clear();
            if (bool.Parse(dv[0]["Featured"].ToString()))
            {
                char[] delim = { ';' };
                string[] tokens = dv[0]["DaysFeatured"].ToString().Split(delim, StringSplitOptions.RemoveEmptyEntries);
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
                    //ChangeSelectedTab(0, 1);
                    //ChangeSelectedTab(1, 2);
                    //ChangeSelectedTab(2, 3);
                    //ChangeSelectedTab(3, 4);
                    FeaturePanel.Visible = true;
                    FindPrice();
                }
            }

            Session["EventCategoriesSet"] = null;
            SetCategories();
            
        }
        catch (Exception ex)
        {
            MessagePanel.Visible = true;
            YourMessagesLabel.Text = ex.ToString();
        }
    }

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

    protected void SetCategories()
    {
        CategoryTree.DataBind();
        RadTreeView2.DataBind();

        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn); DataView dvCategories = dat.GetDataDV("SELECT * FROM Event_Category_Mapping WHERE EventID=" +
            Request.QueryString["ID"].ToString());

        try
        {
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
            Session["EventCategoriesSet"] = null;
            //Session["EventCategoriesSet"] = "notnull";
        }
        catch (Exception ex)
        {
            MessagePanel.Visible = true;
            YourMessagesLabel.Text = ex.ToString();
        }
    }

    protected void ChangeVenueStateAction(object sender, EventArgs e)
    {
        ChangeVenueState(VenueCountry.SelectedValue);
    }
    
    protected void ChangeVenueState(string countryID)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn); DataSet ds = dat.GetData("SELECT * FROM State WHERE country_id=" + countryID);

        
         Div1.Visible = false;
        //VenueDropDown.Visible = false;
        

        bool isTextBox = false;
        if (ds.Tables.Count > 0)
            if (ds.Tables[0].Rows.Count > 0)
            {
                VenueState.Visible = true;
                VenueStateTextBox.Visible = false;
                VenueState.DataSource = ds;
                VenueState.DataTextField = "state_2_code";
                VenueState.DataValueField = "state_id";
                VenueState.DataBind();
            }
            else
                isTextBox = true;
        else
            isTextBox = true;

        if (isTextBox)
        {
            VenueStateTextBox.Visible = true;
            VenueState.Visible = false;
        }
    }

    protected void ChangePrivateStateAction(object sender, EventArgs e)
    {
        ChangePrivateVenueState(privateCountryDropDown.SelectedValue);
        FindPrice();
    }

    protected void ChangePrivateVenueState(string countryID)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn); DataSet ds = dat.GetData("SELECT * FROM State WHERE country_id=" + countryID);

        bool isTextBox = false;
        if (ds.Tables.Count > 0)
            if (ds.Tables[0].Rows.Count > 0)
            {
                privateStateDropDown.Visible = true;
                privateStateTextBox.Visible = false;
                privateStateDropDown.DataSource = ds;
                privateStateDropDown.DataTextField = "state_2_code";
                privateStateDropDown.DataValueField = "state_id";
                privateStateDropDown.DataBind();
            }
            else
                isTextBox = true;
        else
            isTextBox = true;

        if (isTextBox)
        {
            privateStateTextBox.Visible = true;
            privateStateDropDown.Visible = false;
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

    //protected void ChangeState(object sender, EventArgs e)
    //{
    //    HttpCookie cookie = Request.Cookies["BrowserDate"];
    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //    DataSet ds = dat.GetData("SELECT * FROM State WHERE country_id="+CountryDropDown.SelectedValue);

    //    bool isTextBox = false;
    //    if (ds.Tables.Count > 0)
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            StateDropDownPanel.Visible = true;
    //            StateTextBoxPanel.Visible = false;
    //            StateDropDown.DataSource = ds;
    //            StateDropDown.DataTextField = "state_2_code";
    //            StateDropDown.DataValueField = "state_id";
    //            StateDropDown.DataBind();
    //        }
    //        else
    //            isTextBox = true;
    //    else
    //        isTextBox = true;

    //    if (isTextBox)
    //    {
    //        StateTextBoxPanel.Visible = true;
    //        StateDropDownPanel.Visible = false;
    //    }

    //    if (CountryDropDown.SelectedValue == "223")
    //    {
    //        USPanel.Visible = true;
    //        InternationalPanel.Visible = false;
    //    }
    //    else
    //    {
    //        USPanel.Visible = false;
    //        InternationalPanel.Visible = true;
    //    }
    //}
    
    protected void ClearEverything()
    {
        ShowDescriptionBegining.Text = "";
        ShowVideoPictureLiteral.Text = "";
        ShowRestOfDescription.Text = "";
        Rotator1.Controls.Clear();
        ShowDateAndTimeLabel.Text = "";
        ShowVenueName.Text = "";
        ShowEventName.Text = "";
    }
    
    //protected void AddDate(object sender, EventArgs e)
    //{
    //    if (ReoccuringRadDateTimePicker.DbSelectedDate != null)
    //    {
    //        DateSelectionsListBox.Items.Add(ReoccuringRadDateTimePicker.DbSelectedDate.ToString());
    //    }
    //}

    protected void SubtractDate(object sender, EventArgs e)
    {
        List<ListItem> items = new List<ListItem>();
        for (int i = 0; i < DateSelectionsListBox.Items.Count; i++)
        {
            if (DateSelectionsListBox.Items[i].Selected)
                items.Add(DateSelectionsListBox.Items[i]);

        }

        for (int i = 0; i < items.Count; i++)
        {
            DateSelectionsListBox.Items.Remove(items[i]);
        }
    }

    protected void PostIt(object sender, EventArgs e)
    {
        MessagePanel.Visible = false;
        YourMessagesLabel.Text = "";

        string problem = "";
        bool hasEditChanged = false;
        bool chargeCard = false;
        AuthorizePayPal d = new AuthorizePayPal();
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn); string email = "";
        try
        {
            if (OnwardsIT())
            {
                //Add case for if Paypal is filled in...
                //Authorize Credit Card
                bool goOn = false;
                string message = "";
                decimal price = 0.00M;
                string transactionID = "";

                

                if (AgreeCheckBox.Checked)
                {
                    if (TotalLabel.Text.Trim() != "")
                    {
                        if (decimal.TryParse(TotalLabel.Text.Trim(), out price))
                        {
                            if (price != 0.00M)
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
                                            chargeCard = true;
                                            transactionID = status["TRANSACTIONID"];
                                            Session["TransID"] = transactionID;
                                            goOn = true;
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
                                Session["Featrued"] = false;
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

                        string textEmail = "";
                        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
                        conn.Open();

                        string mediaCat = "0";
                        if (PictureCheckList.Items.Count > 0)
                            mediaCat = "1";


                        bool isOwner = false;

                        string ownerID = "";
                        bool ownerUpForGrabs = false;
                        bool wasFeatured = false;
                        bool isEditing = false;
                        if (isEdit.Text != "")
                        {
                            isEditing = bool.Parse(isEdit.Text);
                            ownerUpForGrabs = dat.IsOwnerUpForGrabs(Request.QueryString["ID"].ToString(),
                                ref ownerID, ref isOwner, false);
                        }

                        bool hadSongs = false;
                        DataSet dsEvent = new DataSet();
                        DataView dvEvent = new DataView();
                        string theCat = "NULL";
                        if (isEditing)
                        {
                            dsEvent = dat.GetData("SELECT * FROM Events WHERE ID=" + eventID.Text);
                            dvEvent = dat.GetDataDV("SELECT * FROM Events WHERE ID=" + eventID.Text);
                            wasFeatured = bool.Parse(dsEvent.Tables[0].Rows[0]["Featured"].ToString());

                            hadSongs = bool.Parse(dsEvent.Tables[0].Rows[0]["hasSongs"].ToString());

                            if (dsEvent.Tables[0].Rows[0]["MediaCategory"].ToString() != mediaCat)
                            {
                                theCat = mediaCat;
                                hasEditChanged = true;
                            }
                        }

                        string addAdressBeg = "";
                        string addAddressEnd = "";
                        string addAddressUpdate = "";

                        string privInsertBeg = ", Private ";
                        string privInsertEnd = ", 'False' ";
                        string privUpdate = ", Private = 'False' ";

                        
                        

                        if (LocaleRadioButtonList.SelectedValue == "1")
                        {
                            addAdressBeg = ", Address ";
                            addAddressEnd = ", @address";
                            addAddressUpdate = ", Address = @address ";

                            privInsertBeg = ", Private ";
                            privInsertEnd = ", 'True' ";
                            privUpdate = ", Private = 'True' ";

                        }

                        string command = "";
                        if (isEditing)
                        {
                            if (isOwner || ownerUpForGrabs)
                            {
                                string sngs = "";
                                if (!hadSongs)
                                    sngs = "hasSongs=@songs,";

                                command = "UPDATE Events SET DaysFeatured=@daysFet, " + addAddressUpdate + privUpdate + "Featured=@fet, MinPrice=@min, MaxPrice=@max, Owner=@owner, [Content]=@content, Header=@header, " +
                                    "Venue=@venue,SponsorPresenter=@sponsor, " + sngs + " mediaCategory=" + mediaCat + ", " +
                                    "ShortDescription=@shortDescription, Country=@country, State=@state, " +
                                    "Zip=@zip, City=@city, LastEditOn=@dateP WHERE ID=" + Request.QueryString["ID"].ToString();
                            }
                            else
                            {
                                command = "INSERT INTO EventRevisions (DaysFeatured," + addAdressBeg + privInsertBeg + "Featured,MinPrice, MaxPrice, EventID, [Content], " +
                                    "Header, Venue, modifierID, "
                                + "ShortDescription, Country, State, Zip, City, DATE)"
                                    + " VALUES(@daysFet, " + addAddressEnd + privInsertEnd + "@fet,@min, @max, " + eventID.Text + ", @content,@header, @venue, @userName, @shortDescription"
                                + ", @country, @state, @zip, @city, '" + isn.ToString() + "')";

                                dsEvent = dat.GetData("SELECT * FROM Events WHERE ID=" + eventID.Text);
                            }
                        }
                        else
                        {
                            command = "INSERT INTO Events (DaysFeatured" + addAdressBeg + privInsertBeg + ", Featured,MinPrice, MaxPrice, Owner, [Content], " +
                                 "Header, Venue, EventGoersCount, SponsorPresenter, hasSongs, mediaCategory, UserName, "
                             + "ShortDescription, Country, State, Zip, City, StarRating, PostedOn, LastEditOn)"
                                 + " VALUES(@daysFet" + addAddressEnd + privInsertEnd + ", @fet, @min, @max, @owner, @content, @header, @venue, "
                                 + " @eventGoers, @sponsor, @songs, " + mediaCat + ", @userName, @shortDescription"
                             + ", @country, @state, @zip, @city, 0, @dateP, @dateP)";
                        }

                        SqlCommand cmd = new SqlCommand(command, conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("@dateP", SqlDbType.DateTime).Value = DateTime.Now;

                        string fetDays = "";
                        foreach (ListItem item in FeatureDatesListBox.Items)
                        {
                            fetDays += ";" + item.Text + ";";
                        }

                        if (wasFeatured)
                        {
                            cmd.Parameters.Add("@fet", SqlDbType.Bit).Value = true;

                            if (FeaturePanel.Visible)
                            {
                                cmd.Parameters.Add("@daysFet", SqlDbType.NVarChar).Value = fetDays;
                            }
                            else
                                cmd.Parameters.Add("@daysFet", SqlDbType.NVarChar).Value = dvEvent[0]["DaysFeatured"].ToString();
                        }
                        else
                        {
                            cmd.Parameters.Add("@fet", SqlDbType.Bit).Value = FeaturePanel.Visible;
                            if (FeaturePanel.Visible)
                            {
                                cmd.Parameters.Add("@daysFet", SqlDbType.NVarChar).Value = fetDays;
                            }
                            else
                            {
                                cmd.Parameters.Add("@daysFet", SqlDbType.NVarChar).Value = DBNull.Value;
                            }
                        }



                        if (MinTextBox.Text.Trim() != "")
                        {
                            cmd.Parameters.Add("@min", SqlDbType.Decimal).Value = decimal.Parse(MinTextBox.Text.Trim());
                        }
                        else
                        {
                            cmd.Parameters.Add("@min", SqlDbType.Decimal).Value = DBNull.Value;
                        }

                        if (MaxTextBox.Text.Trim() != "")
                        {
                            cmd.Parameters.Add("@max", SqlDbType.Decimal).Value = decimal.Parse(MaxTextBox.Text.Trim());
                        }
                        else
                        {
                            cmd.Parameters.Add("@max", SqlDbType.Decimal).Value = DBNull.Value;
                        }

                        if (isEditing && !isOwner)
                        {
                            if (ownerUpForGrabs)
                            {
                                if (OwnerCheckBox.Checked)
                                {
                                    hasEditChanged = true;
                                    cmd.Parameters.Add("@owner", SqlDbType.Int).Value = Session["User"].ToString();
                                    dat.Execute("INSERT INTO EventOwnerHistory (EventID, OwnerID, DateCreatedOwnership) " +
                                        "VALUES(" + eventID.Text + ", " + Session["User"].ToString() + ", '" + isn.ToString() + "')");
                                }
                                else
                                {
                                    if (ownerUpForGrabs)
                                    {
                                        cmd.Parameters.Add("@owner", SqlDbType.Int).Value = DBNull.Value;
                                    }
                                }
                            }
                            else
                            {

                            }


                            if (dsEvent.Tables[0].Rows[0]["Content"].ToString() != DescriptionTextBox.Content)
                            {
                                cmd.Parameters.Add("@content", SqlDbType.NVarChar).Value = DescriptionTextBox.Content;
                                hasEditChanged = true;
                            }
                            else
                            {
                                if (ownerUpForGrabs)
                                {
                                    cmd.Parameters.Add("@content", SqlDbType.NVarChar).Value = dsEvent.Tables[0].Rows[0]["Content"].ToString();
                                }
                                else
                                {
                                    cmd.Parameters.Add("@content", SqlDbType.NVarChar).Value = DBNull.Value;
                                }
                            }

                            if (dsEvent.Tables[0].Rows[0]["Header"].ToString() != EventNameTextBox.Text)
                            {
                                cmd.Parameters.Add("@header", SqlDbType.NVarChar).Value = EventNameTextBox.Text;
                                hasEditChanged = true;
                            }
                            else
                            {
                                if (ownerUpForGrabs)
                                {
                                    hasEditChanged = true;
                                    cmd.Parameters.Add("@header", SqlDbType.NVarChar).Value = dsEvent.Tables[0].Rows[0]["Header"].ToString();
                                }
                                else
                                {
                                    cmd.Parameters.Add("@header", SqlDbType.NVarChar).Value = DBNull.Value;
                                }
                            }

                            string shortDesc = DescriptionTextBox.Text;
                            if (shortDesc.Length > 150)
                                shortDesc = shortDesc.Substring(0, 150);

                            if (dsEvent.Tables[0].Rows[0]["ShortDescription"].ToString() != shortDesc)
                            {
                                cmd.Parameters.Add("@shortDescription", SqlDbType.NVarChar).Value = shortDesc;
                                hasEditChanged = true;
                            }
                            else
                            {
                                if (ownerUpForGrabs)
                                {
                                    cmd.Parameters.Add("@shortDescription", SqlDbType.NVarChar).Value =
                                        dsEvent.Tables[0].Rows[0]["ShortDescription"].ToString();
                                    hasEditChanged = true;
                                }
                                else
                                {
                                    cmd.Parameters.Add("@shortDescription", SqlDbType.NVarChar).Value = DBNull.Value;
                                }
                            }

                            //users that are not the owner are not allowed to delete songs or other media
                            //therefore, users can only change the state of 'hasSongs' to 'true', never to 'false'
                            if (ownerUpForGrabs && MusicCheckBox.Checked)
                            {
                                cmd.Parameters.Add("@songs", SqlDbType.Bit).Value = MusicCheckBox.Checked;
                                hasEditChanged = true;
                            }
                            else
                            {
                                cmd.Parameters.Add("@songs", SqlDbType.Bit).Value = dsEvent.Tables[0].Rows[0]["hasSongs"].ToString();
                            }

                            cmd.Parameters.Add("@sponsor", SqlDbType.NVarChar).Value = DBNull.Value;
                        }
                        else
                        {
                            string shortDesc = DescriptionTextBox.Text;
                            if (shortDesc.Length > 150)
                                shortDesc = shortDesc.Substring(0, 150);

                            hasEditChanged = true;
                            cmd.Parameters.Add("@content", SqlDbType.NVarChar).Value = DescriptionTextBox.Content;
                            cmd.Parameters.Add("@header", SqlDbType.NVarChar).Value = EventNameTextBox.Text;
                            cmd.Parameters.Add("@shortDescription", SqlDbType.NVarChar).Value = shortDesc;
                            cmd.Parameters.Add("@songs", SqlDbType.Bit).Value = MusicCheckBox.Checked;

                            if (!isEditing)
                            {
                                cmd.Parameters.Add("@rating", SqlDbType.Int).Value = 0;
                                cmd.Parameters.Add("@userName", SqlDbType.NVarChar).Value = Session["UserName"].ToString();
                                cmd.Parameters.Add("@eventGoers", SqlDbType.Int).Value = 0;
                                if (OwnerCheckBox.Checked)
                                {
                                    cmd.Parameters.Add("@owner", SqlDbType.Int).Value = Session["User"].ToString();
                                }
                                else
                                {
                                    cmd.Parameters.Add("@owner", SqlDbType.Int).Value = DBNull.Value;
                                }
                            }
                            else
                            {
                                if (isOwner)
                                {
                                    if (OwnerCheckBox.Checked)
                                        cmd.Parameters.Add("@owner", SqlDbType.NVarChar).Value = Session["User"].ToString();
                                    else
                                        cmd.Parameters.Add("@owner", SqlDbType.NVarChar).Value = DBNull.Value;
                                }
                            }

                            cmd.Parameters.Add("@sponsor", SqlDbType.NVarChar).Value = DBNull.Value;
                        }

                        #region Create/Assign Venue
                        string country = "";
                        string state1 = "";
                        string venue = "";
                        bool isNewVenue = false;
                        int venueID = 0;
                        if (LocaleRadioButtonList.SelectedValue == "0")
                        {
                            //Need to check if the user is creating a new venue.
                            //First If statement is if a user chose existing venue from dropdown
                            //The Else statement if for the new venue.

                            venueID = int.Parse(Session["NewVenue"].ToString());
                            DataSet dsVenue = dat.GetData("SELECT * FROM Venues WHERE ID=" + venueID.ToString());
                            venue = dsVenue.Tables[0].Rows[0]["Name"].ToString();
                            if (isEditing && !isOwner)
                            {
                                if (dsEvent.Tables[0].Rows[0]["Venue"].ToString() != venueID.ToString())
                                {
                                    hasEditChanged = true;
                                    cmd.Parameters.Add("@venue", SqlDbType.Int).Value = venueID;

                                    if (dsEvent.Tables[0].Rows[0]["Country"].ToString() != dsVenue.Tables[0].Rows[0]["Country"].ToString())
                                        cmd.Parameters.Add("@country", SqlDbType.Int).Value = dsVenue.Tables[0].Rows[0]["Country"].ToString();
                                    else
                                    {
                                        if (ownerUpForGrabs)
                                        {
                                            cmd.Parameters.Add("@country", SqlDbType.Int).Value = dsEvent.Tables[0].Rows[0]["Country"].ToString();
                                        }
                                        else
                                        {
                                            cmd.Parameters.Add("@country", SqlDbType.Int).Value = DBNull.Value;
                                        }
                                    }

                                    if (dsEvent.Tables[0].Rows[0]["State"].ToString() != dsVenue.Tables[0].Rows[0]["State"].ToString())
                                        cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = dsVenue.Tables[0].Rows[0]["State"].ToString();
                                    else
                                    {
                                        if (ownerUpForGrabs)
                                        {
                                            cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = dsEvent.Tables[0].Rows[0]["State"].ToString();
                                        }
                                        else
                                        {
                                            cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = DBNull.Value;
                                        }
                                    }
                                    if (dsEvent.Tables[0].Rows[0]["City"].ToString() != dsVenue.Tables[0].Rows[0]["City"].ToString())
                                        cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = dsVenue.Tables[0].Rows[0]["City"].ToString();
                                    else
                                    {
                                        if (ownerUpForGrabs)
                                        {
                                            cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = dsEvent.Tables[0].Rows[0]["City"].ToString();
                                        }
                                        else
                                        {
                                            cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = DBNull.Value;
                                        }

                                    }

                                    if (dsEvent.Tables[0].Rows[0]["Zip"].ToString() != dsVenue.Tables[0].Rows[0]["Zip"].ToString())
                                        cmd.Parameters.Add("@zip", SqlDbType.NVarChar).Value = dsVenue.Tables[0].Rows[0]["Zip"].ToString();
                                    else
                                    {
                                        if (ownerUpForGrabs)
                                        {
                                            cmd.Parameters.Add("@zip", SqlDbType.NVarChar).Value = dsEvent.Tables[0].Rows[0]["Zip"].ToString();
                                        }
                                        else
                                        {
                                            cmd.Parameters.Add("@zip", SqlDbType.NVarChar).Value = DBNull.Value;
                                        }
                                    }
                                }
                                else
                                {
                                    if (ownerUpForGrabs)
                                    {
                                        cmd.Parameters.Add("@venue", SqlDbType.Int).Value = dsEvent.Tables[0].Rows[0]["Venue"].ToString();
                                        cmd.Parameters.Add("@country", SqlDbType.Int).Value = dsEvent.Tables[0].Rows[0]["Country"].ToString();
                                        cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = dsEvent.Tables[0].Rows[0]["State"].ToString();
                                        cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = dsEvent.Tables[0].Rows[0]["City"].ToString();
                                        cmd.Parameters.Add("@zip", SqlDbType.NVarChar).Value = dsEvent.Tables[0].Rows[0]["Zip"].ToString();
                                    }
                                    else
                                    {
                                        cmd.Parameters.Add("@venue", SqlDbType.Int).Value = DBNull.Value;
                                        cmd.Parameters.Add("@country", SqlDbType.Int).Value = DBNull.Value;
                                        cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = DBNull.Value;
                                        cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = DBNull.Value;
                                        cmd.Parameters.Add("@zip", SqlDbType.NVarChar).Value = DBNull.Value;
                                    }
                                }
                            }
                            else
                            {
                                cmd.Parameters.Add("@venue", SqlDbType.Int).Value = venueID;
                                cmd.Parameters.Add("@country", SqlDbType.Int).Value = dsVenue.Tables[0].Rows[0]["Country"].ToString();
                                cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = dsVenue.Tables[0].Rows[0]["State"].ToString();
                                cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = dsVenue.Tables[0].Rows[0]["City"].ToString();
                                cmd.Parameters.Add("@zip", SqlDbType.NVarChar).Value = dsVenue.Tables[0].Rows[0]["Zip"].ToString();
                            }

                            country = dsVenue.Tables[0].Rows[0]["Country"].ToString();
                            state1 = dsVenue.Tables[0].Rows[0]["State"].ToString();
                        }
                        else
                        {
                            venue = dat.stripHTML(addressTextBox.Text.Trim());
                            string privState = "";
                            if (privateStateDropDown.Visible)
                                privState = privateStateDropDown.SelectedItem.Text;
                            else
                                privState = dat.stripHTML(privateStateTextBox.Text.Trim());

                            cmd.Parameters.Add("@venue", SqlDbType.NVarChar).Value = DBNull.Value;
                            cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = dat.stripHTML(addressTextBox.Text.Trim());
                            cmd.Parameters.Add("@country", SqlDbType.Int).Value = privateCountryDropDown.SelectedValue;
                            cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = privState;
                            cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = dat.stripHTML(cityTextBox.Text.Trim());
                            cmd.Parameters.Add("@zip", SqlDbType.NVarChar).Value = dat.stripHTML(ZipTextBox.Text.Trim());

                            country = privateCountryDropDown.SelectedValue;
                            state1 = privState;
                        }


                        


                        #endregion

                        if (isEditing)
                        {
                            cmd.ExecuteNonQuery();
                        }

                        if (!isEditing || isOwner)
                        {
                            cmd.ExecuteNonQuery();
                        }

                        bool songsChanged = false;
                        bool mediaChanged = false;
                        bool occuranceChanged = false;

                        cmd = new SqlCommand("SELECT @@IDENTITY AS ID", conn);
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd);
                        DataSet ds3 = new DataSet();
                        da2.Fill(ds3);

                        string revisionID = "";
                        if (isEditing && !isOwner && !ownerUpForGrabs)
                        {
                            revisionID = ds3.Tables[0].Rows[0]["ID"].ToString();
                        }
                        string ID = ds3.Tables[0].Rows[0]["ID"].ToString();

                        if (!isEditing)
                        {
                            if (OwnerCheckBox.Checked)
                            {
                                dat.Execute("INSERT INTO EventOwnerHistory (EventID, OwnerID, DateCreatedOwnership) " +
                                    "VALUES(" + ID + ", " + Session["User"].ToString() + ", '" + isn.ToString() + "')");
                            }
                        }

                        string firstStartDate = DateSelectionsListBox.Items[0].Text;

                        email += "<br/><br/><a href=\"http://HippoHappenings.com/" + dat.MakeNiceName(EventNameTextBox.Text) + "_" + ID +
                            "_Event\">" + EventNameTextBox.Text + "</a><br/><br/>" + venue + "<br/><br/>First Dates: " +
                            firstStartDate + "<br/><br/>" + DescriptionTextBox.Content;

                        textEmail = ". Name: " + EventNameTextBox.Text + ". Venue: " + venue + ". First Date: " +
                            firstStartDate +
                            ". Link: http://HippoHappenings.com/" + dat.MakeNiceName(EventNameTextBox.Text) + "_" + ID +
                            "_Event";

                        string temporaryID = "";
                        if (isEditing)
                        {
                            temporaryID = eventID.Text;
                        }
                        else
                        {

                            temporaryID = ID;
                        }

                        string categories =
                            CreateCategories(temporaryID, isOwner, isEditing, revisionID, ownerUpForGrabs);

                        if (isEditing)
                            ID = Request.QueryString["ID"].ToString();



                        //string temp = categories;

                        //if (categories != "")
                        //    temp += " OR ";
                        //temp += "UV.VenueID=" + venueID;

                        #region Take Care of Media

                        if (MusicCheckBox.Checked || hadSongs)
                        {
                            for (int i = 0; i < SongCheckList.Items.Count; i++)
                            {
                                if (isEditing)
                                {
                                    if (SongCheckList.Items[i].Enabled)
                                    {
                                        if (revisionID != "" && !isOwner && !ownerUpForGrabs)
                                        {
                                            cmd = new SqlCommand("INSERT INTO EventRevisions_Song_Mapping (RevisionID, EventID, SongName) " +
                                                "VALUES(" + revisionID + ",@eventID, @songName)", conn);
                                            cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = int.Parse(eventID.Text);
                                            cmd.Parameters.Add("@songName", SqlDbType.NVarChar).Value = SongCheckList.Items[i].Value.ToString();
                                            cmd.ExecuteNonQuery();
                                        }
                                        songsChanged = true;

                                        dat.Execute("UPDATE Events SET hasSongs='True' WHERE ID=" + eventID.Text);

                                        if (i == 0)
                                            dat.Execute("DELETE FROM Event_Song_Mapping WHERE EventID=" + eventID.Text);


                                        cmd = new SqlCommand("INSERT INTO Event_Song_Mapping (EventID, SongName, SongTitle) " +
                                            "VALUES(@eventID, @songName, @songTitle)", conn);
                                        cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = int.Parse(eventID.Text);
                                        cmd.Parameters.Add("@songName", SqlDbType.NVarChar).Value = SongCheckList.Items[i].Value.ToString();
                                        cmd.Parameters.Add("@songTitle", SqlDbType.NVarChar).Value = SongCheckList.Items[i].Text;
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    dat.Execute("UPDATE Events SET hasSongs='True' WHERE ID=" + ID);

                                    cmd = new SqlCommand("INSERT INTO Event_Song_Mapping (EventID, SongName, SongTitle) " +
                                        "VALUES(@eventID, @songName, @songTitle)", conn);
                                    cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = int.Parse(ID);
                                    cmd.Parameters.Add("@songName", SqlDbType.NVarChar).Value = SongCheckList.Items[i].Value.ToString();
                                    cmd.Parameters.Add("@songTitle", SqlDbType.NVarChar).Value = SongCheckList.Items[i].Text;
                                    cmd.ExecuteNonQuery();
                                }



                            }
                        }

                        //Media Categories: NONE: 0, Picture: 1, Video: 2, YouTubeVideo: 3, Slider: 4
                        bool isSlider = false;
                        if (PictureCheckList.Items.Count > 0)
                            isSlider = true;

                        string tempID = ID;
                        if (isEditing)
                        {
                            tempID = eventID.Text;
                        }

                        if (isSlider)
                        {

                            char[] delim2 = { '\\' };
                            string[] fileArray = System.IO.Directory.GetFiles(MapPath(".") + "\\UserFiles\\" + Session["EffectiveUserName"].ToString() + "\\Slider\\");

                            if (!System.IO.Directory.Exists(MapPath(".") + "\\UserFiles"))
                            {
                                System.IO.Directory.CreateDirectory(MapPath(".") + "\\UserFiles");
                                System.IO.Directory.CreateDirectory(MapPath(".") + "\\UserFiles\\Events\\");
                                System.IO.Directory.CreateDirectory(MapPath(".") + "\\UserFiles\\Events\\" + tempID);
                                System.IO.Directory.CreateDirectory(MapPath(".") + "\\UserFiles\\Events\\" + tempID + "\\Slider\\");
                            }
                            else
                            {
                                if (!System.IO.Directory.Exists(MapPath(".") + "\\UserFiles\\Events\\"))
                                {
                                    System.IO.Directory.CreateDirectory(MapPath(".") + "\\UserFiles\\Events\\");
                                    System.IO.Directory.CreateDirectory(MapPath(".") + "\\UserFiles\\Events\\" + tempID);
                                    System.IO.Directory.CreateDirectory(MapPath(".") + "\\UserFiles\\Events\\" + tempID + "\\Slider\\");
                                }
                                else
                                {
                                    if (!System.IO.Directory.Exists(MapPath(".") + "\\UserFiles\\Events\\" + tempID))
                                    {
                                        System.IO.Directory.CreateDirectory(MapPath(".") + "\\UserFiles\\Events\\" + tempID);
                                        System.IO.Directory.CreateDirectory(MapPath(".") + "\\UserFiles\\Events\\" + tempID + "\\Slider\\");
                                    }
                                    else
                                    {
                                        if (!System.IO.Directory.Exists(MapPath(".") + "\\UserFiles\\Events\\" + tempID + "\\Slider\\"))
                                        {
                                            System.IO.Directory.CreateDirectory(MapPath(".") + "\\UserFiles\\Events\\" + tempID + "\\Slider\\");
                                        }
                                    }
                                }
                            }

                            string YouTubeStr = "";
                            char[] delim3 = { '.' };
                            bool isPathAbsolute = false;
                            for (int i = 0; i < PictureCheckList.Items.Count; i++)
                            {
                                //int length = fileArray[i].Split(delim2).Length;
                                if (PictureCheckList.Items[i].Value == "ImgPathAbsolute")
                                    isPathAbsolute = true;
                                else
                                    isPathAbsolute = false;

                                if (isPathAbsolute)
                                {
                                    cmd = new SqlCommand("INSERT INTO Event_Slider_Mapping (EventID, PictureName, RealPictureName, ImgPathAbsolute) " +
                                                    "VALUES(@eventID, @picName, @realName, 'True')", conn);
                                    cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = tempID;
                                    cmd.Parameters.Add("@picName", SqlDbType.NVarChar).Value = PictureCheckList.Items[i].Text;
                                    cmd.Parameters.Add("@realName", SqlDbType.NVarChar).Value = PictureCheckList.Items[i].Text;
                                    cmd.ExecuteNonQuery();
                                }
                                else
                                {
                                    string[] tokens = PictureCheckList.Items[i].Value.ToString().Split(delim3);


                                    if (tokens.Length >= 2)
                                    {
                                        if (tokens[1].ToUpper() == "JPG" || tokens[1].ToUpper() == "JPEG" || tokens[1].ToUpper() == "GIF" || tokens[1].ToUpper() == "PNG")
                                        {
                                            if (!System.IO.File.Exists(MapPath(".") + "\\UserFiles\\Events\\" + tempID + "\\Slider\\" + PictureCheckList.Items[i].Value))
                                            {
                                                System.IO.File.Copy(MapPath(".") + "\\UserFiles\\" + Session["EffectiveUserName"].ToString() +
                                                                        "\\Slider\\" + PictureCheckList.Items[i].Value,
                                                                        MapPath(".") + "\\UserFiles\\Events\\" + tempID + "\\Slider\\" + PictureCheckList.Items[i].Value);
                                            }

                                            if (isEditing)
                                            {

                                                if (revisionID != "" && !isOwner && !ownerUpForGrabs)
                                                {
                                                    if (PictureCheckList.Items[i].Enabled)
                                                    {
                                                        cmd = new SqlCommand("INSERT INTO EventRevisions_Slider_Mapping (RevisionID, EventID, " +
                                                            "PictureName) VALUES(" + revisionID + ",@eventID, @picName)", conn);
                                                        cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = tempID;
                                                        cmd.Parameters.Add("@picName", SqlDbType.NVarChar).Value = PictureCheckList.Items[i].Value;
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }
                                                mediaChanged = true;

                                                if (i == 0)
                                                    dat.Execute("DELETE FROM Event_Slider_Mapping WHERE EventID=" + eventID.Text);

                                                dat.Execute("UPDATE Events SET mediaCategory=" + mediaCat + " WHERE ID=" + tempID);

                                                cmd = new SqlCommand("INSERT INTO Event_Slider_Mapping (EventID, PictureName, RealPictureName) " +
                                                    "VALUES(@eventID, @picName, @realName)", conn);
                                                cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = tempID;
                                                cmd.Parameters.Add("@picName", SqlDbType.NVarChar).Value = PictureCheckList.Items[i].Value;
                                                cmd.Parameters.Add("@realName", SqlDbType.NVarChar).Value = PictureCheckList.Items[i].Text;
                                                cmd.ExecuteNonQuery();

                                            }
                                            else
                                            {
                                                cmd = new SqlCommand("INSERT INTO Event_Slider_Mapping (EventID, PictureName, RealPicturename) " +
                                                    "VALUES(@eventID, @picName, @realName)", conn);
                                                cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = tempID;
                                                cmd.Parameters.Add("@picName", SqlDbType.NVarChar).Value = PictureCheckList.Items[i].Value;
                                                cmd.Parameters.Add("@realName", SqlDbType.NVarChar).Value = PictureCheckList.Items[i].Text;
                                                cmd.ExecuteNonQuery();
                                            }

                                        }
                                        //WE NO LONGER ALLOW Videos to be uploaded to the site itself. Only YouTube videos allowed.
                                        //else if (tokens[1].ToUpper() == "WMV")
                                        //{
                                        //    if (!System.IO.File.Exists(MapPath(".") + "\\UserFiles\\Events\\" + tempID + "\\Slider\\" + PictureCheckList.Items[i].Value))
                                        //    {
                                        //        System.IO.File.Copy(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() +
                                        //             "\\Slider\\" + PictureCheckList.Items[i].Value,
                                        //             MapPath(".") + "\\UserFiles\\Events\\" + tempID + "\\Slider\\" + PictureCheckList.Items[i].Value);
                                        //    }
                                        //    if (isEditing)
                                        //    {
                                        //        if (PictureCheckList.Items[i].Enabled)
                                        //        {
                                        //            cmd = new SqlCommand("INSERT INTO EventRevisions_Slider_Mapping (modifierID, EventID, PictureName) VALUES("+Session["User"].ToString()+",@eventID, @picName)", conn);
                                        //            cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = tempID;
                                        //            cmd.Parameters.Add("@picName", SqlDbType.NVarChar).Value = PictureCheckList.Items[i].Value;
                                        //            cmd.ExecuteNonQuery();

                                        //            mediaChanged = true;

                                        //            cmd = new SqlCommand("INSERT INTO Event_Slider_Mapping (EventID, PictureName) VALUES(@eventID, @picName)", conn);
                                        //            cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = tempID;
                                        //            cmd.Parameters.Add("@picName", SqlDbType.NVarChar).Value = PictureCheckList.Items[i].Value;
                                        //            cmd.ExecuteNonQuery();
                                        //        }
                                        //    }
                                        //    else
                                        //    {
                                        //        cmd = new SqlCommand("INSERT INTO Event_Slider_Mapping (EventID, PictureName) VALUES(@eventID, @picName)", conn);
                                        //        cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = tempID;
                                        //        cmd.Parameters.Add("@picName", SqlDbType.NVarChar).Value = PictureCheckList.Items[i].Value;
                                        //        cmd.ExecuteNonQuery();
                                        //    }

                                        //}
                                    }
                                    else
                                    {
                                        mediaChanged = true;
                                        YouTubeStr += PictureCheckList.Items[i].Value + ";";
                                    }
                                }
                            }

                            if (YouTubeStr != "")
                                if (isEditing)
                                {
                                    if (dsEvent.Tables[0].Rows[0]["YouTubeVideo"].ToString() != YouTubeStr && !isOwner && !ownerUpForGrabs)
                                    {
                                        dat.Execute("INSERT INTO EventRevisions_YouTube (EventID, YouTubeStr, RevisionID) " +
                                            "VALUES(" + ID + ", '" + YouTubeStr + "', " + revisionID + ")");
                                    }

                                    dat.Execute("UPDATE Events SET mediaCategory=" + mediaCat + ", YouTubeVideo='" + YouTubeStr + "' WHERE ID=" + tempID);
                                }
                                else
                                {
                                    dat.Execute("UPDATE Events SET mediaCategory=" + mediaCat + ", YouTubeVideo='" + YouTubeStr + "' WHERE ID=" + ID);
                                }

                        }

                        #endregion

                        #region Take Care of Event Occurance
                        DataSet dsEOccur = dat.GetData("SELECT * FROM Event_Occurance WHERE EventID=" + tempID);
                        DataView dvEOccur = new DataView(dsEOccur.Tables[0], "", "", DataViewRowState.CurrentRows);

                        //if (isEditing && (!isOwner || ownerUpForGrabs))
                        //{
                        //    dvEOccur.RowFilter = "DateTimeStart = '" + StartDateTimePicker.DbSelectedDate.ToString() + "' AND " +
                        //        "DateTimeEnd = '" + EndDateTimePicker.DbSelectedDate.ToString() + "'";

                        //    if (dvEOccur.Count == 0)
                        //    {
                        //        occuranceChanged = true;
                        //        hasEditChanged = true;
                        //        cmd = new SqlCommand("INSERT INTO EventRevisions_Occurance (EventID, DateTimeStart, DateTimeEnd, RevisionID, DATE) " +
                        //            "VALUES(@eventID, @dateStart, @dateEnd, " + temporaryID + ", '"+DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).ToString()+"')", conn);
                        //        cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = tempID;
                        //        cmd.Parameters.Add("@dateStart", SqlDbType.DateTime).Value = StartDateTimePicker.DbSelectedDate.ToString();
                        //        if (EndDateTimePicker.DbSelectedDate != null)
                        //            cmd.Parameters.Add("@dateEnd", SqlDbType.DateTime).Value = EndDateTimePicker.DbSelectedDate.ToString();
                        //        else
                        //            cmd.Parameters.Add("@dateEnd", SqlDbType.DateTime).Value = DBNull.Value;
                        //        cmd.ExecuteNonQuery();
                        //    }
                        //}
                        //else
                        //{
                        //    dat.Execute("DELETE FROM Event_Occurance WHERE EventID="+temporaryID);
                        //    cmd = new SqlCommand("INSERT INTO Event_Occurance (EventID, DateTimeStart, DateTimeEnd) VALUES(@eventID, @dateStart, @dateEnd)", conn);
                        //    cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = tempID;
                        //    cmd.Parameters.Add("@dateStart", SqlDbType.DateTime).Value = StartDateTimePicker.DbSelectedDate.ToString();
                        //    if (EndDateTimePicker.DbSelectedDate != null)
                        //        cmd.Parameters.Add("@dateEnd", SqlDbType.DateTime).Value = EndDateTimePicker.DbSelectedDate.ToString();
                        //    else
                        //        cmd.Parameters.Add("@dateEnd", SqlDbType.DateTime).Value = DBNull.Value;
                        //    cmd.ExecuteNonQuery();
                        //}


                        //DateTime a = DateTime.Parse(EndDateTimePicker.DbSelectedDate.ToString());
                        //TimeSpan b = a.Subtract(DateTime.Parse(StartDateTimePicker.DbSelectedDate.ToString()));

                        string[] delimStr = { " -- " };

                        dat.Execute("DELETE FROM Event_Occurance WHERE EventID=" + tempID);

                        for (int i = 0; i < DateSelectionsListBox.Items.Count; i++)
                        {
                            if (isEditing)
                            {

                                string[] tokensStr = DateSelectionsListBox.Items[i].Text.Split(delimStr,
                                        StringSplitOptions.RemoveEmptyEntries);


                                //if (dvEOccur.Count == 0)
                                //{
                                occuranceChanged = true;
                                hasEditChanged = true;

                                if (!ownerUpForGrabs && !isOwner)
                                {

                                    cmd = new SqlCommand("INSERT INTO EventRevisions_Occurance (EventID, DateTimeStart, DateTimeEnd, RevisionID, DATE)" +
                                        "VALUES(@eventID, @dateStart, @dateEnd, " + temporaryID + ", '" + isn.ToString() + "')", conn);
                                    cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = tempID;
                                    cmd.Parameters.Add("@dateStart", SqlDbType.DateTime).Value = tokensStr[0];
                                    cmd.Parameters.Add("@dateEnd", SqlDbType.DateTime).Value = tokensStr[1];
                                    cmd.ExecuteNonQuery();
                                }
                                else
                                {
                                    cmd = new SqlCommand("INSERT INTO Event_Occurance (EventID, DateTimeStart, DateTimeEnd) VALUES(@eventID, @dateStart, @dateEnd)", conn);
                                    cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = tempID;
                                    cmd.Parameters.Add("@dateStart", SqlDbType.DateTime).Value = tokensStr[0];
                                    cmd.Parameters.Add("@dateEnd", SqlDbType.DateTime).Value = tokensStr[1];
                                    cmd.ExecuteNonQuery();
                                }
                                //}
                            }
                            else
                            {
                                string[] tokensStr = DateSelectionsListBox.Items[i].Text.Split(delimStr, StringSplitOptions.RemoveEmptyEntries);

                                cmd = new SqlCommand("INSERT INTO Event_Occurance (EventID, DateTimeStart, DateTimeEnd) VALUES(@eventID, @dateStart, @dateEnd)", conn);
                                cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = tempID;
                                cmd.Parameters.Add("@dateStart", SqlDbType.DateTime).Value = tokensStr[0];
                                cmd.Parameters.Add("@dateEnd", SqlDbType.DateTime).Value = tokensStr[1];
                                cmd.ExecuteNonQuery();
                            }

                        }

                        #endregion

                        conn.Close();

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
                                    dat.Execute("INSERT INTO EventSearchTerms (EventID, SearchTerms, SearchDate) VALUES(" + ID +
                                        ", '" + terms.Replace("'", "''") + "', '" + item.Text + "')");
                            }
                        }
                        #endregion

                        try
                        {
                            if (chargeCard)
                            {
                                Encryption encrypt = new Encryption();

                                //Charge Card though Capture
                                country = dat.GetDataDV("SELECT country_2_code FROM Countries WHERE country_id=" + BillingCountry.SelectedValue)[0]["country_2_code"].ToString();
                                com.paypal.sdk.util.NVPCodec status = d.DoCaptureCode(transactionID, price.ToString(),
                                    "E" + temporaryID + isn.ToString(), "Capture Transaction for Featuring Event '" +
                                    dat.MakeNiceNameFull(EventNameTextBox.Text) + "'");
                                //message = status.ToString();
                                string successORFailure = status["ACK"];
                                switch (successORFailure.ToLower())
                                {
                                    case "failure":
                                        MessagePanel.Visible = true;
                                        YourMessagesLabel.Text = status["L_LONGMESSAGE0"];
                                        //MessagePanel.Visible = true;
                                        //foreach (string key in status.Keys)
                                        //{
                                        //    YourMessagesLabel.Text += "key: '" + key + "', value: '" + status[key] + "' <br/>";
                                        //}
                                        break;
                                    case "success":
                                        //MessagePanel.Visible = true;
                                        //foreach (string key in status.Keys)
                                        //{
                                        //    YourMessagesLabel.Text += "key: '" + key + "', value: '" + status[key] + "' <br/>";
                                        //}
                                        TakeCareOfPostEmail(isEditing, isOwner, isNewVenue, ownerUpForGrabs,
                                        temporaryID, venueID.ToString(), tempID, revisionID, ownerID);
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
                                //MessagePanel.Visible = true;
                                //YourMessagesLabel.Text = "no charge here";
                                TakeCareOfPostEmail(isEditing, isOwner, isNewVenue, ownerUpForGrabs,
                                    temporaryID, venueID.ToString(), tempID, revisionID, ownerID);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessagePanel.Visible = true;
                            YourMessagesLabel.Text = "problem: " + problem + ex.ToString();
                        }

                    }
                    else
                    {
                        MessagePanel.Visible = true;
                        YourMessagesLabel.Text = "" + message;
                    }
                }
                else
                {
                    MessagePanel.Visible = true;
                    YourMessagesLabel.Text = "You must agree to the terms and conditions.";
                }
            }
        }
        catch (Exception ex)
        {
            MessagePanel.Visible = true;
            YourMessagesLabel.Text = "problem: " + problem + ex.ToString() + ", command: ";
        }
    }

    protected void TakeCareOfPostEmail(bool isEditing, bool isOwner, bool isNewVenue, bool ownerUpForGrabs, 
        string ID, string venueID, string tempID, string revisionID, string ownerID)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn); DataSet dsUser = dat.GetData("SELECT Email, UserName FROM USERS WHERE User_ID=" +
                    Session["User"].ToString());
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
        conn.Open();
        string moreMessage = "";
        string emailBody = "";

        DataSet dsEvent = new DataSet();

        if (isEditing)
        {
            dsEvent = dat.GetData("SELECT * FROM Events WHERE ID=" + eventID.Text);
        }

        Encryption encrypt = new Encryption();
        if (isEditing && !isOwner)
        {
            if (isNewVenue)
                moreMessage = "You have created a new venue, to fill in the details go to the <a class=\"NavyLink12UD\" onclick=\"Search('enter-locale?ID=" + venueID.ToString() + "');\">venue's page.</a>";

            if (ownerUpForGrabs)
            {
                Session["Message"] =
                    "Your changes to this event have been posted successfully! Confirmation of this posting has been sent to your email account. " +
                    "<br/>";
            }
            else
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                SqlCommand cmd34 = new SqlCommand("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, To_UserID, Date, [Read], Mode, Live, SentLive) VALUES('" +
                     "EventID:" + Request.QueryString["ID"].ToString() + ",UserID:" + Session["User"].ToString() + ",RevisionID:" + revisionID + "',@content, "+dat.HIPPOHAPP_USERID+", " + ownerID + ", '" + isn.ToString() + "', 0, 4, 1, 1)", conn);
                cmd34.Parameters.Add("@content", SqlDbType.NVarChar).Value = "A change request has been submitted for an event you've created: " +
                    EventNameTextBox.Text;
                cmd34.ExecuteNonQuery();
                conn.Close();
                Session["Message"] = "Your changes to this event have been posted successfully! The user who has created this event will be notified and will need to approve these changes before they show on the site, however, any media and categories will be automatically approved.<br/>";
            }
            Session["Message"] += moreMessage + "<br/><br/>" + "-Go to <a class=\"NavyLink12UD\" onclick=\"Search('" + dat.MakeNiceName(EventNameTextBox.Text) + "_" + tempID +
                    "_Event');\">this event's</a> home page.<br/><br/> -<a class=\"NavyLink12UD\" onclick=\"Search('RateExperience.aspx?Type=ER&ID=" + tempID + "');\" >Rate </a>your user experience editing this event.";

            emailBody = "<div style=\"text-align: left;\"><br/>Dear " + dsUser.Tables[0].Rows[0]["UserName"].ToString() + ",<br/><br/> you have successfully posted the event \"" + EventNameTextBox.Text +
            "\". <br/><br/> You can find this event <a href=\"http://hippohappenings.com/" + dat.MakeNiceName(EventNameTextBox.Text) + "_" + tempID +
                    "_Event\">here</a>. " +
            "<br/><br/> While creating this event, you have created a new venue. To fill in the full details of this new venue please <a href=\"http://hippohappenings.com/enter-locale?ID=" + venueID.ToString() + "\">go to it's page.</a>" +
            "<br/></br> To rate your experience posting this event <a href=\"http://hippohappenings.com/RateExperience.aspx?Type=E&ID=" + tempID + "\">please include your feedback here.</a>" +
            "<br/><br/><br/>Have a Hippo Happening Day!<br/><br/></div>";
        }
        else
        {
            if (isNewVenue)
                moreMessage = "You have created a new venue, to fill in the details go to the <a class=\"NavyLink12UD\" onclick=\"Search('enter-locale?ID=" + venueID.ToString() + "');\">venue's page.</a>";
            Session["Message"] = "Your event has been posted successfully!<br/> Here are your next steps. An email with these choices will also be sent to your account. <br/><br/>";
            Session["Message"] += moreMessage + "<br/><br/>" + "-Go to <a class=\"NavyLink12UD\" onclick=\"Search('/" + dat.MakeNiceName(EventNameTextBox.Text) + "_" + tempID +
                    "_Event');\">this event's</a> home page.<br/><br/> -<a class=\"NavyLink12UD\" onclick=\"Search('RateExperience.aspx?Type=E&ID=" + tempID + "');\" >Rate </a>your user experience posting this event.";

            emailBody = "<br/><br/>Dear " + dsUser.Tables[0].Rows[0]["UserName"].ToString() + ", <br/><br/> you have successfully posted the event \"" + EventNameTextBox.Text +
            "\". <br/><br/> You can find this event <a href=\"http://hippohappenings.com/" + dat.MakeNiceName(EventNameTextBox.Text) + "_" + tempID +
                    "_Event\">here</a>. " +
            "<br/><br/> To rate your experience posting this event <a href=\"http://hippohappenings.com/RateExperience.aspx?Type=E&ID=" + tempID + "\">please include your feedback here.</a>" +
            "<br/><br/><br/>Have a Hippo Happening Day!<br/><br/>";
        }

        //MessageLiteral.Text = "<script type=\"text/javascript\">alert('" + message + "');</script>";
        if (!isEditing)
        {
            dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "You have successfully posted the event: " +
                EventNameTextBox.Text);
        }
        else
        {
            DataSet dsEventUser = dat.GetData("SELECT * FROM Users U WHERE UserName='" + dsEvent.Tables[0].Rows[0]["UserName"].ToString() + "'");
            emailBody = "<br/><br/>A change request has been submitted for an event you have posted: \"" + EventNameTextBox.Text +
                "\". <br/><br/> You can find this event <a href=\"http://hippohappenings.com/" + dat.MakeNiceName(EventNameTextBox.Text) + "_" +ID +
                    "_Event\">here</a>. " +
                "<br/><br/> Please log into Hippo Happenings and check your messages to view and approve these changes.</a>" +
                "<br/><br/><br/>Have a Hippo Happening Day!<br/><br/>";

            if (!ownerUpForGrabs && !isOwner)
            {
                dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                    dsEventUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "A change request has been submitted for an event you have posted: " +
                    EventNameTextBox.Text);
            }

            ////Send email to all users who have this event in their calendar and have their email preference set for event updates
            //if (isOwner || (ownerUpForGrabs && OwnerCheckBox.Checked))
            //{
            //    emailBody = "<br/><br/>Changes have been made to an event in your calendar: Event '\"" + EventNameTextBox.Text +
            //        "\"'. <br/><br/> To view these changes, please go to this event's <a class=\"NavyLink\"  href=\"http://hippohappenings.com/" + dat.MakeNiceName(EventNameTextBox.Text) + "_" + ID +
            //        "_Event\">page</a>. " +
            //        "<br/><br/><br/>Have a Hippo Happening Day!<br/><br/>";

            //    DataSet dsAllUsers = dat.GetData("SELECT * FROM User_Calendar UC, Users U, UserPreferences UP WHERE U.User_ID=UP.UserID AND UP.EmailPrefs LIKE '%C%' AND U.User_ID=UC.UserID AND UC.EventID="+ID);

            //    DataView dv = new DataView(dsAllUsers.Tables[0], "", "", DataViewRowState.CurrentRows);

            //    if (dv.Count > 0)
            //    {
            //        for(int i=0;i<dv.Count;i++)
            //        {
            //            dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
            //                System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
            //                dv[i]["Email"].ToString(), emailBody,
            //                "Event '" +
            //                EventNameTextBox.Text + "' has been modified");

            //            //Mode is 6: for a 'do not reply' message
            //            dat.Execute("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, " +
            //                "To_UserID, Date, [Read], Mode, Live, SentLive) VALUES('" + emailBody.Replace("'", "''") + "', '" + "Event ''" +
            //                EventNameTextBox.Text.ToString().Replace("'", "''") + "'' has been modified', "+
            //                dat.HIPPOHAPP_USERID+", " + dv[i]["UserID"].ToString() + ", GETDATE(), 0, 1, 1, 0)");

            //        }
            //    }
            //}
        }


        MessageRadWindow.NavigateUrl = "Message.aspx?message=" + encrypt.encrypt(Session["Message"].ToString() +
            "<br/><br/><div align=\"center\">" +
            "<div style=\"width: 50px;\" onclick=\"Search('home')\">" +
            "<div class=\"topDiv\" style=\"clear: both;\">" +
              "  <img style=\"float: left;\" src=\"NewImages/ButtonLeft.png\" height=\"27px\" /> " +
               " <div class=\"NavyLink12UD\" style=\"font-size: 12px; text-decoration: none; padding-top: 5px;padding-left: 6px; padding-right: 6px;height: 27px;float: left;background: url('NewImages/ButtonPixel.png'); background-repeat: repeat-x;\">" +
                   " OK " +
                "</div>" +
               " <img style=\"float: left;\" src=\"NewImages/ButtonRight.png\" height=\"27px\" /> " +
            "</div>" +
            "</div>" +
            "</div><br/>");
        MessageRadWindow.Visible = true;
        MessageRadWindow.VisibleOnPageLoad = true;
    }

    protected void ModifyIt(object sender, EventArgs e)
    {

    }

    protected string CreateCategories(string ID, bool isOwner, bool isUpdate, string revisionID, bool ownerUpForGrabs)
    {
        string categories = "";
        string message = "";
        
            Hashtable distinctHash = new Hashtable();
            Hashtable tagHash = new Hashtable();
            HttpCookie cookie = Request.Cookies["BrowserDate"];

            DateTime isn = DateTime.Now;

            if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
                isn = DateTime.Now;
            DateTime isNow = isn;
            Data dat = new Data(isn);
            if ((isUpdate && isOwner) || (isUpdate && ownerUpForGrabs))
            {
                dat.Execute("DELETE FROM Event_Category_Mapping WHERE EventID=" + ID);
            }

            DataSet dsCategories = dat.GetData("SELECT * FROM Event_Category_Mapping WHERE EventID=" + ID);
            DataView dvCat = new DataView(dsCategories.Tables[0], "", "", DataViewRowState.CurrentRows);



            categories += GetCategoriesFromTree(isUpdate, isOwner, ref CategoryTree, dvCat, ID, revisionID);
            categories += GetCategoriesFromTree(isUpdate, isOwner, ref RadTreeView2, dvCat, ID, revisionID);



        return categories;
    }

    protected string GetCategoriesFromTree(bool isUpdate, bool isOwner, ref Telerik.Web.UI.RadTreeView CategoryTree,
        DataView dvCat, string ID, string revisionID)
    {
        string categories = "";
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn); for (int i = 0; i < CategoryTree.Nodes.Count; i++)
        {

            categories = GetCategoriesFromNode(isUpdate, isOwner, CategoryTree.Nodes[i], dvCat, ID, revisionID);
            //Recurse if there is children
        }
        return categories;

    }

    protected string GetCategoriesFromNode(bool isUpdate, bool isOwner,
        Telerik.Web.UI.RadTreeNode TreeNode, DataView dvCat, string ID, string revisionID)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn); string ownerID = "";
        string categories = "";


        bool isOwnerUpForGrabs = dat.IsOwnerUpForGrabs(ID, ref ownerID, ref isOwner, false);
        if (TreeNode.Checked && TreeNode.Enabled)
        {
            dvCat.RowFilter = "CategoryID=" + TreeNode.Value;
            //distinctHash.Add(CategoriesCheckBoxes.Items[i], 21);
            //tagHash.Add(CategoriesCheckBoxes.Items[i], "22");

            if (isUpdate)
            {
                if (isOwner || isOwnerUpForGrabs)
                {
                    if (dvCat.Count == 0)
                    {
                        dat.Execute("INSERT INTO Event_Category_Mapping (CategoryID, EventID, tagSize) VALUES("
                            + TreeNode.Value + "," + ID + ", 22)");
                        if (categories != "")
                            categories += " OR ";
                        categories += " UC.CategoryID=" + TreeNode.Value;
                    }
                }
                else
                {
                    if (dvCat.Count == 0)
                    {
                        string command22 = "INSERT INTO EventCategoryRevisions (AddOrRemove, EventID, CatID, modifierID, RevisionID, DATE) " +
                            "VALUES(1, " + ID + ", " + TreeNode.Value + ", " + Session["User"].ToString() + ", " + revisionID + ", '" + isn + "')";
                        Session["command"] = command22;
                        dat.Execute(command22);
                    }
                    else
                    {
                        //This case is unnecessary since if the Node is checked and it exists for this event, we don't need to do anything
                        //dat.Execute("INSERT INTO VenueCategoryRevisions (AddOrRemove, VenueID, CatID, modifierID, RevisionID) " +
                        //    "VALUES(0, " + ID + ", " + CategoryTree.Nodes[i].Value + ", " + Session["User"].ToString() + ", " + revisionID + ")");
                    }

                }
            }
            else
            {
                dat.Execute("INSERT INTO Event_Category_Mapping (CategoryID, EventID, tagSize) VALUES("
                            + TreeNode.Value + "," + ID + ", 22)");
                if (categories != "")
                    categories += " OR ";
                categories += " UC.CategoryID=" + TreeNode.Value;
            }

        }
        else if (!TreeNode.Checked)
        {
            dvCat.RowFilter = "CategoryID=" + TreeNode.Value;

            if (isUpdate)
            {
                if (isOwner || isOwnerUpForGrabs)
                {
                    if (dvCat.Count == 0)
                    {
                    }
                    else
                    {
                        dat.Execute("DELETE FROM Event_Category_Mapping WHERE EventID=" + ID +
                            " AND CategoryID = " + TreeNode.Value);

                    }
                }
                else
                {
                    if (dvCat.Count == 0)
                    {
                    }
                    else
                    {
                        if (isOwnerUpForGrabs)
                        {
                            dat.Execute("DELETE FROM Event_Category_Mapping WHERE EventID=" + ID +
                            " AND CategoryID = " + TreeNode.Value);
                        }
                        else
                        {
                            dat.Execute("INSERT INTO EventCategoryRevisions (AddOrRemove, EventID, CatID, modifierID, RevisionID, DATE) " +
                                "VALUES(0, " + ID + ", " + TreeNode.Value + ", " + Session["User"].ToString() + ", " + revisionID + ", '" + isn.ToString() + "')");
                        }
                    }

                }
            }
            else
            {
            }
        }

        if (TreeNode.Nodes.Count > 0)
        {
            for (int j = 0; j < TreeNode.Nodes.Count; j++)
            {
                GetCategoriesFromNode(isUpdate, isOwner, TreeNode.Nodes[j], dvCat, ID, revisionID);
            }
        }

        
        
        return categories;
    }

    //protected void TabClick(object sender, EventArgs e)
    //{
    //    int nextIndex = EventTabStrip.SelectedIndex;
    //    short selectThisTab = 0;
    //    YourMessagesLabel.Text = "";
    //    MessagePanel.Visible = false;
    //    //ErrorLabel.Text = "nextindex: " + nextIndex.ToString();

    //    if (Session["EventPrevTab"] != null)
    //    {
    //        int selectedIndex = (int)Session["EventPrevTab"];
    //        //ErrorLabel.Text += "selectedindex: " + selectedIndex.ToString();
    //        Session["EventPrevTab"] = nextIndex;

    //        if (selectedIndex != nextIndex)
    //        {
    //            bool wasClean = OnwardsIT(false, selectedIndex);
    //            if (wasClean)
    //            {
    //                //ErrorLabel.Text += ";was here 1;";
    //                selectThisTab = short.Parse(nextIndex.ToString());

    //            }
    //            else
    //            {
    //                selectThisTab = short.Parse(selectedIndex.ToString());
    //                //ErrorLabel.Text += ";was here 2;";
    //            }

    //            if (MessagePanel.Visible)
    //            {
    //                selectThisTab = short.Parse(selectedIndex.ToString());
    //                //ErrorLabel.Text += ";was here 3;";
    //            }
    //        }
    //        else
    //        {
    //            selectThisTab = short.Parse(selectedIndex.ToString());
    //        }

    //        if (selectedIndex.ToString() == "5")
    //        {
    //            selectThisTab = short.Parse(nextIndex.ToString());
    //        }

    //        //ErrorLabel.Text += "selectthistab: " + selectThisTab.ToString();
    //    }
    //    else
    //    {
    //        Session["EventPrevTab"] = nextIndex;
    //        selectThisTab = short.Parse(nextIndex.ToString());
    //    }

    //    ChangeSelectedTab(0, selectThisTab);
    //}

    //protected void Onwards(object sender, EventArgs e)
    //{
    //    OnwardsIT(true, EventTabStrip.SelectedIndex);
    //}

    protected void Onwards(object sender, EventArgs e)
    {
        OnwardsIT();
    }

    //protected bool OnwardsIT(bool changeTab, int selectedIndex)
    //{
    //    try
    //    {
    //        YourMessagesLabel.Text = "";
    //        MessagePanel.Visible = false;
    //        Session["EventPrevTab"] = selectedIndex;

    //        HttpCookie cookie = Request.Cookies["BrowserDate"];

    //        DateTime isn = DateTime.Now;

    //        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
    //            isn = DateTime.Now;
    //        DateTime isNow = isn;
    //        Data dat = new Data(isn); bool goOn = false;

    //        if (Request.QueryString["ID"] != null)
    //        {
    //            //ImageButton6.PostBackUrl = "blog-event?edit=true&ID=" + Request.QueryString["ID"].ToString() + "#top";
    //            //ImageButton7.PostBackUrl = "blog-event?edit=true&ID=" + Request.QueryString["ID"].ToString() + "#top";
    //            //ImageButton5.PostBackUrl = "blog-event?edit=true&ID=" + Request.QueryString["ID"].ToString() + "#top";
    //            //MusicUploadButton.PostBackUrl = "blog-event?edit=true&ID=" + Request.QueryString["ID"].ToString() + "#top";
    //        }


    //        switch (selectedIndex)
    //        {
    //            case 0:

    //                bool eventName = false;
    //                bool startDate = false;
    //                bool endDate = true;
    //                bool location = false;
    //                bool venue = true;
    //                bool state = true;

    //                EventNameTextBox.Text = dat.stripHTML(EventNameTextBox.Text);

    //                if (EventNameTextBox.Text != "")
    //                    if (EventNameTextBox.Text.Trim().Length > 70)
    //                    {
    //                        eventName = false;
    //                        MessagePanel.Visible = true;
    //                        YourMessagesLabel.Text += "*The event name needs to be less than or equal to 70 characters.";
    //                        return false;
    //                    }
    //                    else
    //                    {
    //                        eventName = true;
    //                    }
    //                else
    //                {
    //                    eventName = false;
    //                    MessagePanel.Visible = true;
    //                    YourMessagesLabel.Text += "*The event name is required.";
    //                    return false;
    //                }



    //                location = true;
                    
    //                    if (Session["NewVenue"] == null)
    //                    {
    //                        if (TimeFrameDiv.InnerHtml.Trim() == "Select Locale >")
    //                        {
    //                            venue = false;
    //                            MessagePanel.Visible = true;
    //                            YourMessagesLabel.Text += "*Must include the Locale.";
    //                            return false;
    //                        }
    //                        else
    //                        {
    //                            Session["NewVenue"] = dat.GetDataDV("SELECT * FROM Venues WHERE Name='" +
    //                                TimeFrameDiv.InnerHtml.Replace("'", "''") + "'")[0]["ID"].ToString();
    //                            location = true;
    //                            state = true;
    //                            goOn = true;
    //                        }
    //                    }
    //                    else
    //                    {

    //                        location = true;
    //                        state = true;
    //                        goOn = true;
    //                    }

                    

    //                if (DateSelectionsListBox.Items.Count > 0)
    //                    startDate = true;
    //                else
    //                {
    //                    startDate = false;
    //                    MessagePanel.Visible = true;
    //                    YourMessagesLabel.Text += "*Must include the Date.";
    //                    return false;
    //                }

    //                //if (EndDateTimePicker.DbSelectedDate != null)
    //                //    endDate = true;
    //                //else
    //                //{
    //                //    endDate = false;
    //                //    MessagePanel.Visible = true;
    //                //    YourMessagesLabel.Text += "<br/><br/>*Must include the End Date.";
    //                //}

    //                //if (startDate && endDate)
    //                //{
    //                //    DateTime a = DateTime.Parse(StartDateTimePicker.DbSelectedDate.ToString());
    //                //    DateTime b = DateTime.Parse(EndDateTimePicker.DbSelectedDate.ToString());

    //                //    int c = DateTime.Compare(a, b);

    //                //    if (c > -1)
    //                //    {
    //                //        endDate = false;
    //                //        MessagePanel.Visible = true;
    //                //        YourMessagesLabel.Text += "<br/><br/>*The End date is earier than the Start date.";
    //                //    }
    //                //}
    //                if (eventName && startDate && endDate && location && venue && state && goOn)
    //                {
    //                    if(changeTab)
    //                        ChangeSelectedTab(0, 1);

    //                    FillLiteral();
    //                    return true;
    //                }
    //                else
    //                {
    //                    return false;
    //                }

    //                break;
    //            case 1:
    //                MinTextBox.Text = dat.stripHTML(MinTextBox.Text.Trim());
    //                MaxTextBox.Text = dat.stripHTML(MaxTextBox.Text.Trim());
    //                DescriptionTextBox.Content = dat.StripHTML_LeaveLinks(DescriptionTextBox.Content.Replace("<div>", "<br/>").Replace("</div>", ""));
    //                ShortDescriptionTextBox.Text = dat.StripHTML_LeaveLinksNoBr(ShortDescriptionTextBox.Text);
    //                decimal theDecimal = 0.00M;
    //                bool goMin = false;
    //                bool goMax = false;
    //                if (MinTextBox.Text.Trim() != "")
    //                {
    //                    if (decimal.TryParse(MinTextBox.Text, out theDecimal))
    //                    {
    //                        goMin = true;
    //                    }
    //                    else
    //                    {
    //                        goMin = false;
    //                    }
    //                }
    //                else
    //                {
    //                    goMin = true;
    //                }

    //                if (MaxTextBox.Text.Trim() != "")
    //                {
    //                    if (decimal.TryParse(MaxTextBox.Text, out theDecimal))
    //                    {
    //                        goMax = true;
    //                    }
    //                    else
    //                    {
    //                        goMax = false;
    //                    }
    //                }
    //                else
    //                {
    //                    goMax = true;
    //                }

    //                if (goMax && goMin)
    //                {
    //                    if (DescriptionTextBox.Text.Length >= 50)
    //                        if (ShortDescriptionTextBox.Text.Length <= 150 &&
    //                            ShortDescriptionTextBox.Text.Length != 0)
    //                        {
    //                            if (changeTab)
    //                                ChangeSelectedTab(1, 2);

    //                            FillLiteral();
    //                            return true;
    //                        }
    //                        else
    //                        {
    //                            MessagePanel.Visible = true;
    //                            YourMessagesLabel.Text += "Make sure that Short Description exists and is less than 150 characters.";
    //                            return false;
    //                        }
    //                    else
    //                    {
    //                        MessagePanel.Visible = true;
    //                        YourMessagesLabel.Text += "*Make sure you say what you want your viewers to hear about this event! The description must be at least 50 characters.";
    //                        return false;
    //                    }
    //                }
    //                else
    //                {
    //                    if (!goMin)
    //                    {
    //                        MessagePanel.Visible = true;
    //                        YourMessagesLabel.Text += "*The Min Price has to be a number.";
    //                        return false;
    //                    }
    //                    else
    //                    {
    //                        MessagePanel.Visible = true;
    //                        YourMessagesLabel.Text += "*The Max Price has to be a number.";
    //                        return false;
    //                    }
                        
    //                }
    //                break;
    //            case 2:
    //                if (Session["EventCategoriesSet"] == null && Request.QueryString["ID"] != null)
    //                {
    //                    SetCategories();
    //                    Session["EventCategoriesSet"] = "notnull";
    //                    //ImageButton6.PostBackUrl = "blog-event?edit=true&ID=" + Request.QueryString["ID"].ToString() + "#top";
    //                    //ImageButton7.PostBackUrl = "blog-event?edit=true&ID=" + Request.QueryString["ID"].ToString() + "#top";
    //                    //ImageButton5.PostBackUrl = "blog-event?edit=true&ID=" + Request.QueryString["ID"].ToString() + "#top";
    //                }
    //                if(changeTab)
    //                    ChangeSelectedTab(2, 3);

    //                FillLiteral();
    //                return true;
    //                break;
    //            case 3:

    //                if (CategorySelected())
    //                {
    //                    FillLiteral();
                        
    //                    if(changeTab)
    //                        ChangeSelectedTab(3, 4);
    //                    FindPrice();
    //                    return true;
    //                }
    //                else
    //                {
    //                    YourMessagesLabel.Text = "Must select at least one category.";
    //                    MessagePanel.Visible = true;
    //                    return false;
    //                }
    //                break;
    //            case 4:
    //                string message = "You have selected to feature the event, but not entered any specifics. If you do not want to feature the event any more, please click 'No, Thank You'.";
    //                if (FeaturePanel.Visible)
    //                {
    //                    if (FeatureDatesListBox.Items.Count == 0 && FeaturePanel.Visible )
    //                    {
    //                        goOn = false;
    //                        Session["Featured"] = false;
    //                    }
    //                    else
    //                    {
    //                        if (TotalLabel.Text.Trim() != "")
    //                        {
    //                            decimal price = 0.00M;
    //                            if (decimal.TryParse(TotalLabel.Text.Trim(), out price))
    //                            {
    //                                if (price != 0.00M)
    //                                {
    //                                    OwnerCheckBox.Checked = true;
    //                                    OwnerCheckBox.Enabled = false;
    //                                    goOn = true;
    //                                    Session["Featured"] = true;
    //                                }
    //                                else
    //                                {
    //                                    if (SearchTermsListBox.Items.Count > 0 && price == 0.00M)
    //                                    {
    //                                        goOn = false;
    //                                        Session["Featured"] = false;
    //                                        message = "You have entered search terms, but, have not included any dates.";
    //                                    }
    //                                    else
    //                                    {
    //                                        goOn = true;
    //                                        Session["Featured"] = false;
    //                                    }
    //                                }
    //                            }
    //                            else
    //                            {
    //                                goOn = true;
    //                                Session["Featured"] = false;
    //                            }
    //                        }
    //                        else
    //                        {
    //                            goOn = true;
    //                            Session["Featured"] = false;
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    OwnerCheckBox.Enabled = true;
    //                    goOn = true;
    //                    Session["Featured"] = false;
    //                }

    //                if (goOn)
    //                {
    //                    FillLiteral();
    //                    ChangeSelectedTab(4, 5);
    //                    return true;
    //                }
    //                else
    //                {
    //                    YourMessagesLabel.Text = message;
    //                    MessagePanel.Visible = true;
    //                    return false;
    //                }
    //                break;
    //            default: break;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        YourMessagesLabel.Text = ex.ToString();
    //        MessagePanel.Visible = true;
            
    //    }

    //    return false;
    //}

    protected bool OnwardsIT()
    {
        try
        {
            YourMessagesLabel.Text = "";
            MessagePanel.Visible = false;
            //Session["EventPrevTab"] = selectedIndex;

            HttpCookie cookie = Request.Cookies["BrowserDate"];

            DateTime isn = DateTime.Now;

            if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
                isn = DateTime.Now;
            DateTime isNow = isn;
            Data dat = new Data(isn); bool goOn = false;

            if (Request.QueryString["ID"] != null)
            {
                //ImageButton6.PostBackUrl = "blog-event?edit=true&ID=" + Request.QueryString["ID"].ToString() + "#top";
                //ImageButton7.PostBackUrl = "blog-event?edit=true&ID=" + Request.QueryString["ID"].ToString() + "#top";
                //ImageButton5.PostBackUrl = "blog-event?edit=true&ID=" + Request.QueryString["ID"].ToString() + "#top";
                //MusicUploadButton.PostBackUrl = "blog-event?edit=true&ID=" + Request.QueryString["ID"].ToString() + "#top";
            }


            
            #region Check First

            bool eventName = false;
            bool startDate = false;
            bool endDate = true;
            bool location = false;
            bool venue = true;
            bool state = true;

            EventNameTextBox.Text = dat.stripHTML(EventNameTextBox.Text);

            if (EventNameTextBox.Text != "")
            {
                eventName = true;
            }
            else
            {
                eventName = false;
                MessagePanel.Visible = true;
                YourMessagesLabel.Text += "*The event name is required.";
                return false;
            }



            location = true;

            if (Session["NewVenue"] == null && LocaleRadioButtonList.SelectedValue == "0")
            {
                if (TimeFrameDiv.InnerHtml.Trim() == "Select Locale >")
                {
                    venue = false;
                    MessagePanel.Visible = true;
                    YourMessagesLabel.Text += "*Must include the Locale or address.";
                    return false;
                }
                else
                {
                    Session["NewVenue"] = dat.GetDataDV("SELECT * FROM Venues WHERE Name='" +
                        TimeFrameDiv.InnerHtml.Replace("'", "''") + "'")[0]["ID"].ToString();
                    location = true;
                    state = true;
                    goOn = true;
                }
            }
            else
            {
                if (LocaleRadioButtonList.SelectedValue == "1")
                {
                    if (addressTextBox.Text.Trim() != "")
                    {
                        if (cityTextBox.Text.Trim() != "")
                        {
                            if (ZipTextBox.Text.Trim() != "")
                            {
                                if (privateStateDropDown.Visible)
                                {
                                    location = true;
                                    state = true;
                                    goOn = true;
                                }
                                else if (privateStateTextBox.Text.Trim() != "")
                                {
                                    location = true;
                                    state = true;
                                    goOn = true;
                                }
                                else
                                {
                                    location = false;
                                    goOn = false;
                                    MessagePanel.Visible = true;
                                    YourMessagesLabel.Text += "*Must include the state or a Locale.";
                                    return false;
                                }
                            }
                            else
                            {
                                location = false;
                                goOn = false;
                                MessagePanel.Visible = true;
                                YourMessagesLabel.Text += "*Must include the zip or a Locale.";
                                return false;
                            }
                        }
                        else
                        {
                            location = false;
                            goOn = false;
                            MessagePanel.Visible = true;
                            YourMessagesLabel.Text += "*Must include the city or a Locale.";
                            return false;
                        }
                    }
                    else
                    {
                        location = false;
                        goOn = false;
                        MessagePanel.Visible = true;
                        YourMessagesLabel.Text += "*Must include the address or a Locale.";
                        return false;
                    }
                }
            }



            if (DateSelectionsListBox.Items.Count > 0)
                startDate = true;
            else
            {
                startDate = false;
                MessagePanel.Visible = true;
                YourMessagesLabel.Text += "*Must include the Date.";
                return false;
            }




            #endregion
            
            if (eventName && startDate && endDate && location && venue && state && goOn)
            {
                
                #region Check Second
                MinTextBox.Text = dat.stripHTML(MinTextBox.Text.Trim());
                MaxTextBox.Text = dat.stripHTML(MaxTextBox.Text.Trim());
                DescriptionTextBox.Content = dat.StripHTML_LeaveLinks(DescriptionTextBox.Content.Replace("<div>", "<br/>").Replace("</div>", ""));
                //ShortDescriptionTextBox.Text = dat.StripHTML_LeaveLinksNoBr(ShortDescriptionTextBox.Text);
                decimal theDecimal = 0.00M;
                bool goMin = false;
                bool goMax = false;
                if (MinTextBox.Text.Trim() != "")
                {
                    if (decimal.TryParse(MinTextBox.Text, out theDecimal))
                    {
                        goMin = true;
                    }
                    else
                    {
                        goMin = false;
                    }
                }
                else
                {
                    goMin = true;
                }

                if (MaxTextBox.Text.Trim() != "")
                {
                    if (decimal.TryParse(MaxTextBox.Text, out theDecimal))
                    {
                        goMax = true;
                    }
                    else
                    {
                        goMax = false;
                    }
                }
                else
                {
                    goMax = true;
                }
                #endregion
                if (goMax && goMin)
                {
                    if (DescriptionTextBox.Text.Length >= 50)
                    {
                        


                        #region Check Three
                        if (Session["EventCategoriesSet"] == null && Request.QueryString["ID"] != null)
                        {
                            SetCategories();
                            Session["EventCategoriesSet"] = "notnull";
                            //ImageButton6.PostBackUrl = "blog-event?edit=true&ID=" + Request.QueryString["ID"].ToString() + "#top";
                            //ImageButton7.PostBackUrl = "blog-event?edit=true&ID=" + Request.QueryString["ID"].ToString() + "#top";
                            //ImageButton5.PostBackUrl = "blog-event?edit=true&ID=" + Request.QueryString["ID"].ToString() + "#top";
                        }
                        #endregion
                        
                        //Check Four
                        if (CategorySelected())
                        {
                            FindPrice();

                            

                            string message = "You have selected to feature the event, but not entered any specifics. If you do not want to feature the event any more, please click 'No, Thank You'.";
                            if (FeaturePanel.Visible)
                            {
                                if (FeatureDatesListBox.Items.Count == 0 && FeaturePanel.Visible)
                                {
                                    goOn = false;
                                    Session["Featured"] = false;
                                }
                                else
                                {
                                    if (TotalLabel.Text.Trim() != "")
                                    {
                                        decimal price = 0.00M;
                                        if (decimal.TryParse(TotalLabel.Text.Trim(), out price))
                                        {
                                            if (price != 0.00M)
                                            {
                                                OwnerCheckBox.Checked = true;
                                                OwnerCheckBox.Enabled = false;
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
                            else
                            {
                                OwnerCheckBox.Enabled = true;
                                goOn = true;
                                Session["Featured"] = false;
                            }
                            
                            if (goOn)
                            {
                                return true;
                            }
                            else
                            {
                                YourMessagesLabel.Text = message;
                                MessagePanel.Visible = true;
                                return false;
                            }
                        }
                        else
                        {
                            YourMessagesLabel.Text = "Must select at least one category.";
                            MessagePanel.Visible = true;
                            return false;
                        }
                    }

                    else
                    {
                        MessagePanel.Visible = true;
                        YourMessagesLabel.Text += "*Make sure you say what you want your viewers to hear about this event! The description must be at least 50 characters.";
                        return false;
                    }
                }
                else
                {
                    if (!goMin)
                    {
                        MessagePanel.Visible = true;
                        YourMessagesLabel.Text += "*The Min Price has to be a number.";
                        return false;
                    }
                    else
                    {
                        MessagePanel.Visible = true;
                        YourMessagesLabel.Text += "*The Max Price has to be a number.";
                        return false;
                    }

                }
            }

        }
        catch (Exception ex)
        {
            YourMessagesLabel.Text = ex.ToString();
            MessagePanel.Visible = true;
        }

        return false;
    }

    protected void UpdateState(object sender, EventArgs e)
    {
        BillingStateTextBox.Text = BillingStateDropDown.SelectedItem.Text;
        FetUpdatePanel.Update();
    }

    protected void FillLiteral()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn);
        EventPanel.Visible = true;
        ShowEventName.Text = EventNameTextBox.Text;

        ShowVenueName.Text = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + Session["NewVenue"].ToString())[0]["Name"].ToString();

        ShowDateAndTimeLabel.Text = "";

        for (int i = 0; i < DateSelectionsListBox.Items.Count; i++)
        {
            ShowDateAndTimeLabel.Text += DateSelectionsListBox.Items[i].Text + "<br/>";
        }

        ShowDescriptionBegining.Text = dat.BreakUpString(DescriptionTextBox.Content, 20);

        //if (DescriptionTextBox.Content.Length > 500)
        //{
        //    ShowDescriptionBegining.Text = DescriptionTextBox.Content.Substring(0, 500);
        //    int j = 500;
        //    if (DescriptionTextBox.Content[500] != ' ')
        //    {

        //        while (DescriptionTextBox.Content[j] != ' ')
        //        {
        //            ShowDescriptionBegining.Text += DescriptionTextBox.Content[j];
        //            j++;

        //            if (j >= DescriptionTextBox.Content.Length)
        //                break;
        //        }
        //    }
        //    ShowDescriptionBegining.Text = dat.BreakUpString(ShowDescriptionBegining.Text, 65);
        //    ShowRestOfDescription.Text = dat.BreakUpString(DescriptionTextBox.Content.Substring(j), 65);
        //}
        //else
        //{
        //    ShowDescriptionBegining.Text = dat.BreakUpString(DescriptionTextBox.Content, 60);
        //    ShowRestOfDescription.Text = "";
        //}

        if (MainAttractionCheck.Checked)
        {
            ShowVideoPictureLiteral.Text = "";
            if (PictureCheckList.Items.Count > 0)
            {
                Rotator1.Items.Clear();
                char[] delim = { '\\' };
                //string[] fileArray = System.IO.Directory.GetFiles(MapPath(".") + "\\UserFiles\\" +
                //    Session["UserName"].ToString() + "\\Slider\\");

                //string[] finalFileArray = new string[PictureCheckList.Items.Count];

                //for (int i = 0; i < PictureCheckList.Items.Count; i++)
                //{
                //    finalFileArray[i] = "http://" + Request.Url.Authority + "/HippoHappenings/UserFiles/" + 
                //        Session["UserName"].ToString() + "/Slider/" + PictureCheckList.Items[i].Value;
                //}
                char[] delim2 = { '.' };

                bool isEdit = false;

                if (Request.QueryString["edit"] != null)
                    isEdit = bool.Parse(Request.QueryString["edit"].ToString());

                for (int i = 0; i < PictureCheckList.Items.Count; i++)
                {
                    //if (PictureCheckList.Items[i].Enabled)
                    //{
                    Literal literal4 = new Literal();
                    string toUse = "";
                    string[] tokens = PictureCheckList.Items[i].Value.ToString().Split(delim2);
                    if (tokens.Length >= 2)
                    {
                        if (tokens[1].ToUpper() == "JPG" || tokens[1].ToUpper() == "JPEG" || tokens[1].ToUpper() == "GIF" || tokens[1].ToUpper() == "PNG")
                        {
                            System.Drawing.Image image;
                            if (isEdit)
                            {
                                if (System.IO.File.Exists(MapPath(".") + "\\UserFiles\\Events\\" + Request.QueryString["ID"].ToString() + "\\Slider\\" + PictureCheckList.Items[i].Value.ToString()))
                                {
                                    image = System.Drawing.Image.FromFile(MapPath(".") + "\\UserFiles\\Events\\" + Request.QueryString["ID"].ToString() + "\\Slider\\" + PictureCheckList.Items[i].Value.ToString());
                                    toUse = "/UserFiles/Events/" + Request.QueryString["ID"].ToString() + "/Slider/" + PictureCheckList.Items[i].Value.ToString();
                                }
                                else
                                {
                                    image = System.Drawing.Image.FromFile(MapPath(".") + "\\UserFiles\\" + Session["EffectiveUserName"].ToString() + "\\Slider\\" + PictureCheckList.Items[i].Value.ToString());
                                    toUse = "/UserFiles/" + Session["EffectiveUserName"].ToString() + "/Slider/" + PictureCheckList.Items[i].Value.ToString();
                                }
                            }
                            else
                            {
                                image = System.Drawing.Image.FromFile(MapPath(".") + "\\UserFiles\\" + Session["EffectiveUserName"].ToString() + "\\Slider\\" + PictureCheckList.Items[i].Value.ToString());
                                toUse = "/UserFiles/" + Session["EffectiveUserName"].ToString() + "/Slider/" + PictureCheckList.Items[i].Value.ToString();
                            }



                            //int width = 410;
                            //int height = 250;

                            int newHeight = image.Height;
                            int newIntWidth = image.Width;

                            ////if image height is less than resize height
                            //if (height >= image.Height)
                            //{
                            //    //leave the height as is
                            //    newHeight = image.Height;

                            //    if (width >= image.Width)
                            //    {
                            //        newIntWidth = image.Width;
                            //    }
                            //    else
                            //    {
                            //        newIntWidth = width;

                            //        double theDivider = double.Parse(image.Width.ToString()) / double.Parse(newIntWidth.ToString());
                            //        double newDoubleHeight = double.Parse(newHeight.ToString());
                            //        newDoubleHeight = double.Parse(height.ToString()) / theDivider;
                            //        newHeight = (int)newDoubleHeight;
                            //    }
                            //}
                            ////if image height is greater than resize height...resize it
                            //else
                            //{
                            //    //make height equal to the requested height.
                            //    newHeight = height;

                            //    //get the ratio of the new height/original height and apply that to the width
                            //    double theDivider = double.Parse(image.Height.ToString()) / double.Parse(newHeight.ToString());
                            //    double newDoubleWidth = double.Parse(newIntWidth.ToString());
                            //    newDoubleWidth = double.Parse(image.Width.ToString()) / theDivider;
                            //    newIntWidth = (int)newDoubleWidth;

                            //    //if the resized width is still to big
                            //    if (newIntWidth > width)
                            //    {
                            //        //make it equal to the requested width
                            //        newIntWidth = width;

                            //        //get the ratio of old/new width and apply it to the already resized height
                            //        theDivider = double.Parse(image.Width.ToString()) / double.Parse(newIntWidth.ToString());
                            //        double newDoubleHeight = double.Parse(newHeight.ToString());
                            //        newDoubleHeight = double.Parse(image.Height.ToString()) / theDivider;
                            //        newHeight = (int)newDoubleHeight;
                            //    }
                            //}


                            literal4.Text = "<div style=\"width: 412px; height: 250px;\"><img style=\" margin-left: " + ((410 - newIntWidth) / 2).ToString() + "px; margin-top: " + ((250 - newHeight) / 2).ToString() + "px;\" height=\"" + newHeight + "px\" width=\"" + newIntWidth + "px\" src=\""
                                + toUse + "\" /></div>";


                        }
                        else if (tokens[1].ToUpper() == "WMV")
                        {
                            literal4.Text = "<object><param  name=\"wmode\" value=\"opaque\" ></param><embed wmode=\"opaque\" height=\"250px\" width=\"410px\" src=\""
                                + "UserFiles/" + Session["EffectiveUserName"].ToString() + "/Slider/" + PictureCheckList.Items[i].Value.ToString() +
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
                    //}
                }

                if (Rotator1.Items.Count == 0)
                    RotatorPanel.Visible = false;
                else
                    RotatorPanel.Visible = true;

                if (Rotator1.Items.Count == 1)
                {
                    RotatorPanel.CssClass = "HiddeButtons";
                }
                else
                {
                    RotatorPanel.CssClass = "";
                }
            }
        }


    }

    //protected void Backwards(object sender, EventArgs e)
    //{
    //    int selectedIndex = EventTabStrip.SelectedIndex;
    //    if (Request.QueryString["ID"] != null)
    //    {
    //        //ImageButton6.PostBackUrl = "blog-event?edit=true&ID=" + Request.QueryString["ID"].ToString() + "#top";
    //        //ImageButton7.PostBackUrl = "blog-event?edit=true&ID=" + Request.QueryString["ID"].ToString() + "#top";
    //        //ImageButton5.PostBackUrl = "blog-event?edit=true&ID=" + Request.QueryString["ID"].ToString() + "#top";
    //    }

    //    switch (selectedIndex)
    //    {
    //        case 1:
    //            ChangeSelectedTab(1, 0);
    //            break;
    //        case 2:
    //            ChangeSelectedTab(2, 1);
    //            break;
    //        case 3:
    //            ChangeSelectedTab(3, 2);
    //            break;
    //        case 4:
    //            ChangeSelectedTab(4, 3);
    //            break;
    //        case 5:
    //            ChangeSelectedTab(5, 4);
    //            break;
    //        default: break;
    //    }
    //}
    
    protected void MusicUpload_Click(object sender, EventArgs e)
    {
        if (!MusicCheckBox.Checked)
            MusicCheckBox.Checked = true;
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn); if (MusicUpload.HasFile && SongCheckList.Items.Count < 3)
        {
            char[] delim = { '.' };
            string[] tokens = MusicUpload.FileName.Split(delim);
            string fileName = "rename" + isn.Ticks.ToString() + "." + tokens[1];
            string extension = fileName.Split(delim)[1];
            if (extension.ToUpper() == "MP3")
            {
                string root = MapPath(".") + "\\UserFiles\\" + Session["EffectiveUserName"].ToString() + "\\Songs\\";
                if (SongCheckList.Items.Count == 0)
                {
                    if (!System.IO.Directory.Exists(MapPath(".") + "\\UserFiles\\" + 
                        Session["EffectiveUserName"].ToString()))
                    {
                        System.IO.Directory.CreateDirectory(MapPath(".") + "\\UserFiles\\" + 
                            Session["EffectiveUserName"].ToString());
                    }

                    if (!System.IO.Directory.Exists(root))
                    {
                        System.IO.Directory.CreateDirectory(root);
                    }
                    
                }
                MusicUpload.SaveAs(root + fileName);
                SongCheckList.Items.Add(new ListItem(MusicUpload.FileName, fileName));
                DeleteSongButton.Visible = true;
            }
            else
            {
                MessagePanel.Visible = true;
                YourMessagesLabel.Text += "The music file has to be .mp3 file.";
            }
        }
    }
    
    //protected void SliderUpload_Click(object sender, EventArgs e)
    //{
    //    if (PictureUpload.HasFile && PictureCheckList.Items.Count < 20)
    //    {
    //        string root = MapPath(".") + "\\UserFiles\\" + Session["EffectiveUserName"].ToString() + "\\Slider\\";
    //        if (PictureCheckList.Items.Count == 0)
    //        {
    //            if (!System.IO.Directory.Exists(MapPath(".") + "\\UserFiles\\" + Session["EffectiveUserName"].ToString()))
    //            {
    //                System.IO.Directory.CreateDirectory(MapPath(".") + "\\UserFiles\\" + Session["EffectiveUserName"].ToString());
    //            }
    //            if (!System.IO.Directory.Exists(root))
    //            {
    //                System.IO.Directory.CreateDirectory(root);
    //            }
    //        }
    //        char[] delim = { '.' };
    //        string[] tokens = PictureUpload.FileName.Split(delim);
    //        string fileName = "rename" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Ticks.ToString() + "." + tokens[1];
    //        PictureUpload.SaveAs(root + fileName);
    //        PictureCheckList.Items.Add(new ListItem(PictureUpload.FileName,fileName));
    //        PictureNixItButton.Visible = true;
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
                    CreateDirectory();
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


                            if (!System.IO.Directory.Exists(MapPath(".").ToString() + "\\UserFiles\\" +
                            Session["EffectiveUserName"].ToString() + "\\Slider\\"))
                            {
                                if (!System.IO.Directory.Exists(MapPath(".").ToString() + "\\UserFiles\\" +
                            Session["EffectiveUserName"].ToString()))
                                {
                                    System.IO.Directory.CreateDirectory(MapPath(".").ToString() + "\\UserFiles\\" +
                            Session["EffectiveUserName"].ToString());
                                }

                                System.IO.Directory.CreateDirectory(MapPath(".").ToString() + "\\UserFiles\\" +
                            Session["EffectiveUserName"].ToString() + "\\Slider\\");
                            }

                            string typeS = "image/" + tokens[1].ToLower();

                            System.Drawing.Image img = System.Drawing.Image.FromStream(file.InputStream);

                            SaveThumbnail(img, true, MapPath(".").ToString() + "\\UserFiles\\" +
                                Session["EffectiveUserName"].ToString() + "\\Slider\\" + fileName, "image/" + tokens[1].ToLower());

                            //SaveJpeg(MapPath(".").ToString() + "/UserFiles/" +
                            //    Session["EffectiveUserName"].ToString() + "/Slider/" + fileName, img, 50, typeS);

                            //PictureUpload.SaveAs(MapPath(".").ToString() + "/UserFiles/" + 
                            //    Session["EffectiveUserName"].ToString() + "/Slider/" + fileName);
                            PictureNixItButton.Visible = true;
                        }
                        else
                        {
                            YourMessagesLabel.Text = "No go! Pictures can only be .gif, .jpg, .jpeg, or .png.";
                            MessagePanel.Visible = true;
                        }

                    }
                    else
                    {
                        YourMessagesLabel.Text = "Only up to 20 Pictures or Videos are allowed.";
                        MessagePanel.Visible = true;
                    }
                }
            }
            
        }
    }

    protected void CreateDirectory()
    {
        if (!System.IO.Directory.Exists(MapPath(".").ToString() + "\\UserFiles\\" + Session["EffectiveUserName"].ToString()))
        {
            System.IO.Directory.CreateDirectory(MapPath(".").ToString() + "\\UserFiles\\" + Session["EffectiveUserName"].ToString());
            System.IO.Directory.CreateDirectory(MapPath(".").ToString() + "\\UserFiles\\" + Session["EffectiveUserName"].ToString() + "\\Slider\\");
        }
        else
        {
            if (!System.IO.Directory.Exists(MapPath(".").ToString() + "\\UserFiles\\" + Session["EffectiveUserName"].ToString() + "\\Slider\\"))
            {
                System.IO.Directory.CreateDirectory(MapPath(".").ToString() + "\\UserFiles\\" + Session["EffectiveUserName"].ToString() + "\\Slider\\");
            }
        }
    }

    protected void VideoUpload_Click(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn);
        if (VideoUpload.HasFile)
        {
            if (PictureCheckList.Items.Count < 20)
            {
                if(PictureCheckList.Items.Count == 0)
                    UploadedVideosAndPics.Visible = true;
                char[] delim = { '.' };
                string[] tokens = VideoUpload.FileName.Split(delim);
                string fileName = "rename" + isn.Ticks.ToString() + "." + tokens[1];
                PictureCheckList.Items.Add(new ListItem(VideoUpload.FileName, fileName));
                VideoUpload.SaveAs(MapPath(".").ToString() + "/UserFiles/" + Session["EffectiveUserName"].ToString() + "/Slider/" + fileName);
                PictureNixItButton.Visible = true;
            }
        }
    }
    
    protected void YouTubeUpload_Click(object sender, EventArgs e)
    {
        if (YouTubeTextBox.Text != "")
        {
            YouTubeTextBox.Text = YouTubeTextBox.Text.Trim().Replace("http://www.youtube.com/watch?v=", "");
            if (PictureCheckList.Items.Count < 20)
            {
                if(PictureCheckList.Items.Count == 0)
                    UploadedVideosAndPics.Visible = true;
                PictureCheckList.Items.Add(new ListItem("YouTube ID: " + YouTubeTextBox.Text, YouTubeTextBox.Text));
                PictureNixItButton.Visible = true;
            }
        }
    }
    
    //protected void ChangeVenue(object sender, EventArgs e)
    //{
    //    HttpCookie cookie = Request.Cookies["BrowserDate"];
    //        Div1.Visible = true;
    //        LocationPanel.Visible = false;
    //        ExistingVenuePanel.Visible = true;
    //        VenueNameTextBox.Visible = false;
    //    VenueCountry.SelectedValue = "223";

    //    bool isTextBox = false;
    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //    DataSet ds = dat.GetData("SELECT * FROM State WHERE country_id=223");
    //    if (ds.Tables.Count > 0)
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            StateDropDownPanel.Visible = true;
    //            StateTextBoxPanel.Visible = false;
    //            StateDropDown.DataSource = ds;
    //            StateDropDown.DataTextField = "state_2_code";
    //            StateDropDown.DataValueField = "state_id";
    //            StateDropDown.DataBind();
    //        }
    //        else
    //            isTextBox = true;
    //    else
    //        isTextBox = true;

    //    if (isTextBox)
    //    {
    //        StateTextBoxPanel.Visible = true;
    //        StateDropDownPanel.Visible = false;
    //    }

    //}
    
    protected void ShowVideo(object sender, EventArgs e)
    {
        //if (VideoRadioList.SelectedValue == "0")
        //{
        //    UploadPanel.Visible = true;
        //    YouTubePanel.Visible = false;
        //}
        //else
        //{
        //    UploadPanel.Visible = false;
        //    YouTubePanel.Visible = true;
        //}
    }
    
    protected void ShowSliderOrVidPic(object sender, EventArgs e)
    {
        if (MainAttracionRadioList.SelectedValue == "0")
        {
            VideoPanel.Visible = true;
            YouTubePanel.Visible = true;
            PicturePanel.Visible = false;
        }
        else
        {
            VideoPanel.Visible = false;
            YouTubePanel.Visible = false;
            PicturePanel.Visible = true;
        }
    }
    
    //protected void ShowVideoOrPicture(object sender, EventArgs e)
    //{
    //    if (VideoRadioList.SelectedValue == "0")
    //    {
    //        PicturePanel.Visible = true;
    //        VideoPanel.Visible = false;
    //    }
    //    else
    //    {
    //        PicturePanel.Visible = false;
    //        VideoPanel.Visible = true;
    //    }
    //}
    
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

    protected void AddDate(object sender, EventArgs e)
    {
        if (StartDateTimePicker.DbSelectedDate != null && EndDateTimePicker.DbSelectedDate != null)
        {
            DateTime a = DateTime.Parse(StartDateTimePicker.DbSelectedDate.ToString());
            DateTime b = DateTime.Parse(EndDateTimePicker.DbSelectedDate.ToString());

            int c = DateTime.Compare(a, b);

            if (c > -1)
            {
                //MessagePanel.Visible = true;
                DateErrorLabel.Text = "<br/><br/>*The End date is earier than the Start date.";
            }
            else
            {
                DateTime startDate = DateTime.Parse(StartDateTimePicker.DbSelectedDate.ToString());
                DateTime endDate = DateTime.Parse(EndDateTimePicker.DbSelectedDate.ToString());

                DateSelectionsListBox.Items.Add(startDate.Month.ToString() + "/" + startDate.Day.ToString() +
                    "/" + startDate.Year.ToString() + " " + startDate.TimeOfDay.Hours.ToString() + ":" +
                    startDate.TimeOfDay.Minutes.ToString() + " -- " +
                    endDate.Month.ToString() + "/" + endDate.Day.ToString() +
                    "/" + endDate.Year.ToString() + " " + endDate.TimeOfDay.Hours.ToString() + ":" +
                    endDate.TimeOfDay.Minutes.ToString());
            }
        }
        else
        {
            //MessagePanel.Visible = true;
            DateErrorLabel.Text = "<br/><br/>*Need to include both the Start date and End date.";
        }
    }

    protected void EnableMusicPanel(object sender, EventArgs e)
    {
        if (MusicCheckBox.Checked)
        {
            MusicPanel.Enabled = true;
            MusicPanel.Visible = true;
        }
        else
        {
            MusicPanel.Enabled = false;
            MusicPanel.Visible = false;
        }
    }
    
    protected void NixIt(object sender, EventArgs e)
    {
        int songCount = SongCheckList.Items.Count;
        CheckBoxList tempList = new CheckBoxList();
        for (int i = 0; i < songCount; i++)
        {
            if (!SongCheckList.Items[i].Selected)
                tempList.Items.Add(SongCheckList.Items[i]);
        }
        SongCheckList.Items.Clear();
        for (int j = 0; j < tempList.Items.Count; j++)
        {
            SongCheckList.Items.Add(tempList.Items[j]);
        }
        if (SongCheckList.Items.Count == 0)
            DeleteSongButton.Visible = false;
    }
    
    protected void PictureNixIt(object sender, EventArgs e)
    {
        PictureCheckList.Items.Clear();
        UploadedVideosAndPics.Visible = false;

        PictureNixItButton.Visible = false;
    }
    
    protected void SliderNixIt(object sender, EventArgs e)
    {
        int songCount = PictureCheckList.Items.Count;
        CheckBoxList tempList = new CheckBoxList();
        for (int i = 0; i < songCount; i++)
        {
            if (!PictureCheckList.Items[i].Selected)
                tempList.Items.Add(PictureCheckList.Items[i]);
            else
            {
                if (System.IO.File.Exists(MapPath(".") + "/UserFiles/" + Session["UserName"].ToString() + "/Slider/" + PictureCheckList.Items[i].Value))
                {
                    System.IO.File.Delete(MapPath(".") + "/UserFiles/" + Session["UserName"].ToString() + "/Slider/" + PictureCheckList.Items[i].Value);
                }
            }
        }
        PictureCheckList.Items.Clear();
        for (int j = 0; j < tempList.Items.Count; j++)
        {
            PictureCheckList.Items.Add(tempList.Items[j]);
        }
        if (PictureCheckList.Items.Count == 0)
        {
            PictureNixItButton.Visible = false;
            UploadedVideosAndPics.Visible = false;
        }
    }
    
    protected void VideoNixIt(object sender, EventArgs e)
    {
        PictureCheckList.Items.Clear();

        PictureNixItButton.Visible = false;
        UploadedVideosAndPics.Visible = false;
    }
    
    protected void SuggestCategoryClick(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn); CategoriesTextBox.Text = dat.stripHTML(CategoriesTextBox.Text.Trim());

        if (CategoriesTextBox.Text.Trim() != "")
        {

            MessagePanel.Visible = true;

            YourMessagesLabel.Text = "Your category '" + CategoriesTextBox.Text + "' has been suggested. We'll send you an update when it has been approved.";

            CategoriesTextBox.Text = dat.StripHTML_LeaveLinks(CategoriesTextBox.Text);

            DataSet dsUser = dat.GetData("SELECT Email, UserName FROM USERS WHERE User_ID=" + Session["User"].ToString());

            dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["categoryemail"].ToString(),
                        System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                System.Configuration.ConfigurationManager.AppSettings["categoryemail"].ToString(), "Category has been suggested from 'blog-event'. The user ID who suggested " +
                "the category is userID: '" + Session["User"].ToString() + "'. The category suggestion is '" + CategoriesTextBox.Text + "'", "Hippo Happenings category suggestion");
            CategoriesTextBox.Text = "";
        }
        else
        {
            MessagePanel.Visible = true;

            YourMessagesLabel.Text = "Please type in the description and name of the category you want to suggest.";
        }
    }

    private void ChangeSelectedTab()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn);

        if (Session["NewVenue"] != null)
        {
            //Panel6.Visible = true;
            string venueID = Session["NewVenue"].ToString();
            DataView dvVenue = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + venueID);
            if (dvVenue[0]["Country"].ToString() == "223")
            {

                string majorCityID = GetMajorCity();
                MajorCity.Text = dat.GetDataDV("SELECT * FROM MajorCities WHERE ID=" + majorCityID)[0]["MajorCity"].ToString();

            }
            else
            {
                MajorCity.Text = dvVenue[0]["City"].ToString();
            }
            FillChart(new object(), new EventArgs());
            FindPrice();
        }
        else
        {
            //string majorCityID = GetMajorCity();
            MajorCity.Text = "Mobile";
            FillChart(new object(), new EventArgs());
            //FindPrice();
        }
        EverythingUpdatePanel.Update();
        FetUpdatePanel.Update();
        //Session["EventPrevTab"] = int.Parse(selectIndex.ToString());
        //EventTabStrip.Tabs[selectIndex].Enabled = true;
        //EventTabStrip.Tabs[selectIndex].Selected = true;
        //EventTabStrip.MultiPage.SelectedIndex = selectIndex;
        //EventTabStrip.SelectedTab.TabIndex = selectIndex;
        //EventTabStrip.SelectedIndex = selectIndex;
        //EventTabStrip.TabIndex = selectIndex;
        //EventTabStrip.Tabs[unselectIndex].Enabled = false;
    }

    //private void ChangeSelectedTab(int unselectIndex, short selectIndex)
    //{
    //    HttpCookie cookie = Request.Cookies["BrowserDate"];

    //    DateTime isn = DateTime.Now;

    //    if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
    //        isn = DateTime.Now;
    //    DateTime isNow = isn;
    //    Data dat = new Data(isn);
    //    string venueID = Session["NewVenue"].ToString();
    //    DataView dvVenue = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + venueID);
    //    if (dvVenue[0]["Country"].ToString() == "223")
    //    {

    //        string majorCityID = GetMajorCity();
    //        MajorCity.Text = dat.GetDataDV("SELECT * FROM MajorCities WHERE ID=" + majorCityID)[0]["MajorCity"].ToString();
            
    //    }
    //    else
    //    {
    //        MajorCity.Text = dvVenue[0]["City"].ToString();
    //    }
    //    FillChart(new object(), new EventArgs());

    //    Session["EventPrevTab"] = int.Parse(selectIndex.ToString());
    //    EventTabStrip.Tabs[selectIndex].Enabled = true;
    //    EventTabStrip.Tabs[selectIndex].Selected = true;
    //    EventTabStrip.MultiPage.SelectedIndex = selectIndex;
    //    EventTabStrip.SelectedTab.TabIndex = selectIndex;
    //    EventTabStrip.SelectedIndex = selectIndex;
    //    EventTabStrip.TabIndex = selectIndex;
    //    //EventTabStrip.Tabs[unselectIndex].Enabled = false;
    //}
    
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
    
    protected void GetVenues(object sender, EventArgs e)
    {
        GetThoseVenues();
    }

    protected void SetNewVenue(object sender, EventArgs e)
    {
        string parameter = Request.Form["__EVENTARGUMENT"];
        Session["NewVenue"] = parameter;
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        TimeFrameDiv.InnerHtml = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + parameter)[0]["Name"].ToString();

        ChangeSelectedTab();
    }

    protected void GetThoseVenues()
    {
        try
        {
            HttpCookie cookie = Request.Cookies["BrowserDate"];

            DateTime isn = DateTime.Now;

            if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
                isn = DateTime.Now;
            DateTime isNow = isn;
            Data dat = new Data(isn);
            TimeFrameDiv.InnerHtml = "Select Locale >";

            Session["NewVenue"] = null;
            Session.Remove("NewVenue");

            string state = "";
            if (VenueState.Visible)
                state = VenueState.SelectedItem.Text;
            else
                state = VenueStateTextBox.Text;

            SqlDbType[] types = { SqlDbType.NVarChar };
            object[] data = { state };
            DataSet ds = dat.GetDataWithParemeters("SELECT CASE WHEN SUBSTRING(Name, 1, 4) = 'The' THEN "+
                "SUBSTRING(Name, 5, LEN(Name)-4) ELSE Name END AS Name1, * FROM Venues WHERE Country=" +
                VenueCountry.SelectedValue + " AND State=@p0 ORDER BY Name1 ASC", types, data);
            

            Session["LocationVenues"] = ds;

            fillVenues(ds);

        }
        catch (Exception ex)
        {
            //MessagePanel.Visible = true;
            //YourMessagesLabel.Text += ex.ToString();
        }
    }

    protected void fillVenues(DataSet ds)
    {
        DataView dv = new DataView(ds.Tables[0], "", "", DataViewRowState.CurrentRows);
        Div1.Visible = true;
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
                litBeg.Text = "<div><div>";
                Literal litMid = new Literal();
                litMid.Text = "";

                char[] letters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
                char[] lettersLow = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

                int countLetters = 0;
                int max = 0;
                int columns = 0;
                int numInColumn = 0;
                int countIncolumn = 0;
                int toDistribute = 0;
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
                            displayL + "; float: left;\"><table>";
                        numInColumn = dv.Count / 7;
                        toDistribute = 0;
                        if (numInColumn * 7 < dv.Count)
                        {
                            toDistribute = dv.Count - (numInColumn * 7);
                        }
                        countIncolumn = 0;
                        for (int i = 0; i < dv.Count; i++)
                        {
                            string borderN = "border: solid 1px #dedbdb;";
                            //if (i != dv.Count - 1)
                            //    borderN = "border-right: solid 1px white;";
                            if (countIncolumn % 7 == 0)
                            {
                                litMid.Text += "<tr>";
                            }

                            litMid.Text += "<td valign=\"top\"><div class=\"Text12\" style=\"" + borderN + "padding: 5px; float: left; width: 100px;\">" + 
                                (countIncolumn + 1).ToString() + ") <a onclick=\"fillDrop('" +
                                    dv[i]["Name"].ToString().Replace("'", "&&") + "', " +
                                    dv[i]["ID"].ToString() + ");\" class=\"Blue12Link\">" +
                                    dv[i]["Name"].ToString() + "</a></div></td>";

                            countIncolumn++;

                            if ((countIncolumn) % 7 == 0)
                            {
                                litMid.Text += "</tr>";
                            }

                            //if (countIncolumn >= numInColumn)
                            //{
                            //    if (toDistribute == 0)
                            //    {
                            //        litMid.Text += "</td></tr>";
                            //        countIncolumn = 0;
                            //    }
                            //    else
                            //    {
                            //        toDistribute--;
                            //    }
                            //}
                        }
                        if (dv.Count % 7 != 0)
                            litMid.Text += "</tr>";
                        litMid.Text += "</table></div>";

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

                //MessagePanel.Visible = true;
                //YourMessagesLabel.Text += "No venues found. Create one my clicking on 'New Venue' radio button.";
            }
        else
        {
            Literal litMid = new Literal();
            litMid.Text = "No venues found for this location. Create one my clicking on 'New Venue' radio button.";

            Tip1.Controls.Add(litMid);

            //MessagePanel.Visible = true;
            //YourMessagesLabel.Text += "No venues found. Create one my clicking on 'New Venue' radio button.";
        }
    }

    //public Bitmap ResizeBitmap(Bitmap b, int nWidth, int nHeight)
    //{
    //    Bitmap result = new Bitmap(nWidth, nHeight);
    //    using (Graphics g = Graphics.FromImage((Image)result))
    //        g.DrawImage(b, 0, 0, nWidth, nHeight);
    //    return result;
    //}

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

        int newHeight = 0;
        int newIntWidth = 0;

        float newFloatHeight = 0.00F;
        float newFloatWidth = 0.00F;

        if (isRotator)
        {
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
        }
        else
        {
            newHeight = 100;
            newIntWidth = 100;
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

    protected void CheckedFeatured(object sender, EventArgs e)
    {
        ChangeSelectedTab();
        FeaturePanel.Visible = true;

        EnableDisablePayment();
    }

    protected void UnCheckedFeatured(object sender, EventArgs e)
    {
        Session["Featured"] = false;
        FeaturePanel.Visible = false;
        ClearFeatured();
        FillChart(new object(), new EventArgs());
        //OnwardsIT(true, EventTabStrip.SelectedIndex);
        //OnwardsIT();
        EnableDisablePayment();
    }

    protected void EnableDisablePayment()
    {
        string message = "You have selected to feature the event, but not entered any specifics. If you do not want to feature the event any more, please click 'No, Thank You'.";
        if (FeaturePanel.Visible)
        {
            if (FeatureDatesListBox.Items.Count == 0 && FeaturePanel.Visible)
            {
                Session["Featured"] = false;
            }
            else
            {
                if (TotalLabel.Text.Trim() != "")
                {
                    decimal price = 0.00M;
                    if (decimal.TryParse(TotalLabel.Text.Trim(), out price))
                    {
                        if (price != 0.00M)
                        {
                            OwnerCheckBox.Checked = true;
                            OwnerCheckBox.Enabled = false;
                            Session["Featured"] = true;
                        }
                        else
                        {
                            Session["Featured"] = false;
                        }
                    }
                    else
                    {
                        Session["Featured"] = false;
                    }
                }
                else
                {
                    Session["Featured"] = false;
                }
            }
        }
        else
        {
            OwnerCheckBox.Enabled = true;
            Session["Featured"] = false;
        }
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
        TermsErrorLabel.Text = "";
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
        string troubleTerms = "";
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
                    TermsErrorLabel.Text = "There are already too many featured events " +
                    "with the search term and date combination you are trying to add. " +
                    "The combination in question is: " + troubleTerms;
                }
            }
            else
            {
                TermsErrorLabel.Text = "Your event is already featured for this date.";
            }
        }
        else
        {
            if (!lassThan7Enabled)
                TermsErrorLabel.Text = "You can purchase 7 feature days at a time. You can always purchase more days by visiting the page again.";
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
                        TermsErrorLabel.Text = "There are already too many featured events " +
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

    protected void RemoveSearchTerm(object sender, EventArgs e)
    {
        if (SearchTermsListBox.SelectedItem != null)
        {
            SearchTermsListBox.Items.Remove(SearchTermsListBox.SelectedItem);
            FillChart(new object(), new EventArgs());
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
                dv = dat.GetDataDV("SELECT * FROM EventSearchTerms WHERE SearchDate = '" + itemDate.Text +
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
                dv = dat.GetDataDV("SELECT * FROM EventSearchTerms WHERE SearchDate = '" + itemDate.Text +
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
                    dv = dat.GetDataDV("SELECT * FROM EventSearchTerms WHERE SearchDate = '" + TermOrDateString +
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

    protected void FillChart(object sender, EventArgs e)
    {
        try
        {
            PricingLiteral.Text = "<td align='center'>0</td><td align='center'>$0</td><td align='center'>$0</td><td align='center'>$0</td><td align='center'>$0</td>";

            bool hasNewFeaturedItems = false;

            foreach (ListItem item in FeatureDatesListBox.Items)
            {
                if (item.Value != "Disabled")
                {
                    hasNewFeaturedItems = true;
                    break;
                }
            }



            if (hasNewFeaturedItems)
            {
                string venueID = Session["NewVenue"].ToString();

                HttpCookie cookie = Request.Cookies["BrowserDate"];

                DateTime isn = DateTime.Now;

                if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
                    isn = DateTime.Now;
                DateTime isNow = isn;
                Data dat = new Data(isn);
                DataView dvChart = dat.GetDataDV("SELECT * FROM StandardPricing");

                DataView dvVenue = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + venueID);

                int daysCount = 0;
                int indexToStart = 0;
                foreach (ListItem item in FeatureDatesListBox.Items)
                {
                    if (item.Value != "Disabled")
                    {
                        daysCount++;
                    }
                    else
                    {
                        indexToStart++;
                    }
                }

                DataView dvCountMembers;
                string zips = "";
                string majorCityID = GetMajorCity();
                if (dvVenue[0]["Country"].ToString() == "223")
                {
                    //Get Major City
                    zips = dat.GetDataDV("SELECT AllZips FROM MajorCities WHERE ID=" + majorCityID)[0]["AllZips"].ToString();

                    dvCountMembers = dat.GetDataDV("SELECT COUNT(*) AS Count FROM UserPreferences WHERE MajorCity = " +
                        majorCityID);

                }
                else
                {
                    dvCountMembers = dat.GetDataDV("SELECT COUNT(*) AS Count FROM UserPreferences WHERE CatCity = '" +
                        dvVenue[0]["City"].ToString() + "' AND CatCountry = " + dvVenue[0]["Country"].ToString() +
                        " AND CatState = '" + dvVenue[0]["State"].ToString() + "'");

                }

                int indexToLookAt = daysCount - 1;

                if (daysCount > 3)
                    indexToLookAt = 3;

                decimal memberCap = decimal.Round(decimal.Parse(dvChart[indexToLookAt]["PerMemberCap"].ToString()), 3, MidpointRounding.AwayFromZero);
                decimal priceForMembers = 0.00M;

                if (decimal.Parse(dvChart[indexToLookAt]["PerMemberPricing"].ToString()) *
                    decimal.Parse(dvCountMembers[0]["Count"].ToString()) > memberCap)
                {
                    priceForMembers = memberCap;
                }
                else
                {
                    priceForMembers = decimal.Round(decimal.Parse(dvChart[indexToLookAt]["PerMemberPricing"].ToString()) *
                        decimal.Parse(dvCountMembers[0]["Count"].ToString()), 3, MidpointRounding.AwayFromZero);
                }

                DataView dvCountEvents;
                decimal eventCountDay1 = 0.00M;
                DateTime dateDate;
                decimal eventCap = decimal.Round(decimal.Parse(dvChart[indexToLookAt]["PerEventCap"].ToString()), 3, MidpointRounding.AwayFromZero);
                decimal subtractionForEventsDay1 = 0.00M;

                decimal total = 0.00M;

                PricingLiteral.Text = "";


                zips = zips.Replace("CatZip", "Zip");
                foreach (ListItem item in FeatureDatesListBox.Items)
                {
                    if (item.Value != "Disabled")
                    {
                        dateDate = DateTime.Parse(item.Text);
                        if (dvVenue[0]["Country"].ToString() == "223")
                        {
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND (" + zips +
                                        ") AND DaysFeatured LIKE '%;" + dateDate.Month.ToString() + "/" +
                                    dateDate.Day.ToString() + "/" + dateDate.Year.ToString() + ";%'");
                        }
                        else
                        {
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND City = '" +
                                dvVenue[0]["City"].ToString() + "' AND Country = " + dvVenue[0]["Country"].ToString() +
                                " AND State = '" + dvVenue[0]["State"].ToString() + "' AND DaysFeatured LIKE '%;" + dateDate.Month.ToString() + "/" +
                                dateDate.Day.ToString() + "/" + dateDate.Year.ToString() + ";%'");
                        }

                        eventCountDay1 = int.Parse(dvCountEvents[0]["Count"].ToString());

                        if (decimal.Parse(dvChart[indexToLookAt]["PerEventPricing"].ToString()) *
                                decimal.Parse(dvCountEvents[0]["Count"].ToString()) > eventCap)
                        {
                            subtractionForEventsDay1 = eventCap;
                        }
                        else
                        {
                            subtractionForEventsDay1 = decimal.Round(decimal.Parse(dvChart[indexToLookAt]["PerEventPricing"].ToString()) *
                                decimal.Parse(dvCountEvents[0]["Count"].ToString()), 3, MidpointRounding.AwayFromZero);
                        }

                        PricingLiteral.Text += "<tr><td align=\"center\" >" + item.Text + "</td><td align=\"center\">" +
                            decimal.Round(decimal.Parse(dvChart[indexToLookAt]["StandardEventPricing"].ToString()), 2, MidpointRounding.AwayFromZero).ToString() +
                            "</td><td align=\"center\">$" + priceForMembers.ToString() + "</td><td align=\"center\">-$" + subtractionForEventsDay1.ToString() + "</td>" +
                            "<td align=\"center\">$" + decimal.Round((decimal.Parse(dvChart[indexToLookAt]["StandardEventPricing"].ToString()) +
                            priceForMembers - subtractionForEventsDay1), 3, MidpointRounding.AwayFromZero).ToString() + "</td></tr>";
                        total += decimal.Round((decimal.Parse(dvChart[indexToLookAt]["StandardEventPricing"].ToString()) +
                            priceForMembers - subtractionForEventsDay1), 3, MidpointRounding.AwayFromZero);
                    }
                }



                decimal searchTermTotal = 0.05M * SearchTermsListBox.Items.Count * daysCount;
                NumSearchTerms.Text = searchTermTotal.ToString();
                TotalLabel.Text = decimal.Round((total + searchTermTotal), 2, MidpointRounding.AwayFromZero).ToString();

                if (decimal.Round((total + searchTermTotal), 2, MidpointRounding.AwayFromZero) > 0.00M)
                    PaymentPanel.Visible = true;
                else
                    PaymentPanel.Visible = false;
            }
            else
            {
                PaymentPanel.Visible = false;
                TotalLabel.Text = "0.00";
            }

            EverythingUpdatePanel.Update();
            FetUpdatePanel.Update();
        }
        catch (Exception ex)
        {
            PriceErrorLabel.Text = ex.ToString();
        }
    }

    protected void FindPrice()
    {
        try
        {
            string state = "";
            string country = "";
            string city = "";
            string zip = "";

            HttpCookie cookie = Request.Cookies["BrowserDate"];

            DateTime isn = DateTime.Now;

            if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
                isn = DateTime.Now;
            DateTime isNow = isn;
            Data dat = new Data(isn);

            if (LocaleRadioButtonList.SelectedValue == "0")
            {
                string venueID = Session["NewVenue"].ToString();
                DataView dvVenue = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + venueID);
                country = dvVenue[0]["Country"].ToString();
                city = dvVenue[0]["City"].ToString();
                state = dvVenue[0]["State"].ToString();
            }
            else
            {
                country = privateCountryDropDown.SelectedValue;
                if (privateStateDropDown.Visible)
                    state = privateStateDropDown.SelectedItem.Text;
                else
                    state = dat.stripHTML(privateStateTextBox.Text.Trim());

                city = dat.stripHTML(cityTextBox.Text.Trim());
                zip = dat.stripHTML(ZipTextBox.Text.Trim());
            }


            DataView dvChart = dat.GetDataDV("SELECT * FROM StandardPricing");



            int daysCount = 0;
            int indexToStart = 0;

            DataView dvCountMembers;
            string zips = "";
            string majorCityID = GetMajorCity();
            if (country == "223")
            {
                //Get Major City
                zips = dat.GetDataDV("SELECT AllZips FROM MajorCities WHERE ID=" + majorCityID)[0]["AllZips"].ToString();

                dvCountMembers = dat.GetDataDV("SELECT COUNT(*) AS Count FROM UserPreferences WHERE MajorCity = " +
                    majorCityID);

            }
            else
            {
                dvCountMembers = dat.GetDataDV("SELECT COUNT(*) AS Count FROM UserPreferences WHERE CatCity = '" +
                    city + "' AND CatCountry = " + country +
                    " AND CatState = '" + state + "'");

            }

            int indexToLookAt = 0;

            decimal memberCap = decimal.Round(decimal.Parse(dvChart[indexToLookAt]["PerMemberCap"].ToString()), 3, MidpointRounding.AwayFromZero);
            decimal priceForMembers = 0.00M;

            if (decimal.Parse(dvChart[indexToLookAt]["PerMemberPricing"].ToString()) *
                decimal.Parse(dvCountMembers[0]["Count"].ToString()) > memberCap)
            {
                priceForMembers = memberCap;
            }
            else
            {
                priceForMembers = decimal.Round(decimal.Parse(dvChart[indexToLookAt]["PerMemberPricing"].ToString()) *
                    decimal.Parse(dvCountMembers[0]["Count"].ToString()), 3, MidpointRounding.AwayFromZero);
            }

            DataView dvCountEvents;
            decimal eventCountDay1 = 0.00M;
            DateTime dateDate;
            decimal eventCap = decimal.Round(decimal.Parse(dvChart[indexToLookAt]["PerEventCap"].ToString()), 3, MidpointRounding.AwayFromZero);
            decimal subtractionForEventsDay1 = 0.00M;

            decimal total = 0.00M;


            zips = zips.Replace("CatZip", "Zip");

            dateDate = DateTime.Now;
            if (country == "223")
            {
                dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND (" + zips +
                            ") AND DaysFeatured LIKE '%;" + dateDate.Month.ToString() + "/" +
                        dateDate.Day.ToString() + "/" + dateDate.Year.ToString() + ";%'");
            }
            else
            {
                dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND City = '" +
                    city + "' AND Country = " + country +
                    " AND State = '" + state + "' AND DaysFeatured LIKE '%;" + dateDate.Month.ToString() + "/" +
                    dateDate.Day.ToString() + "/" + dateDate.Year.ToString() + ";%'");
            }

            eventCountDay1 = int.Parse(dvCountEvents[0]["Count"].ToString());

            if (decimal.Parse(dvChart[indexToLookAt]["PerEventPricing"].ToString()) *
                    decimal.Parse(dvCountEvents[0]["Count"].ToString()) > eventCap)
            {
                subtractionForEventsDay1 = eventCap;
            }
            else
            {
                subtractionForEventsDay1 = decimal.Round(decimal.Parse(dvChart[indexToLookAt]["PerEventPricing"].ToString()) *
                    decimal.Parse(dvCountEvents[0]["Count"].ToString()), 3, MidpointRounding.AwayFromZero);
            }

            total += decimal.Round((decimal.Parse(dvChart[indexToLookAt]["StandardEventPricing"].ToString()) +
                priceForMembers - subtractionForEventsDay1), 3, MidpointRounding.AwayFromZero);

            string allTotal = decimal.Round(total, 2, MidpointRounding.AwayFromZero).ToString();

            PriceLiteral.Text = allTotal;
        }
        catch (Exception ex)
        {
            CLErrorLabel.Text = ex.ToString();
        }
    }

    protected string GetMajorCity()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn);

        string state = "";
        string country = "";
        string city = "";
        string zip = "";

        if (LocaleRadioButtonList.SelectedValue == "0")
        {
            string venueID = Session["NewVenue"].ToString();
            DataView dvVenue = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + venueID);
            country = dvVenue[0]["Country"].ToString();
            city = dvVenue[0]["City"].ToString();
            state = dvVenue[0]["State"].ToString();
            zip = dvVenue[0]["Zip"].ToString();
        }
        else
        {
            country = privateCountryDropDown.SelectedValue;
            if (privateStateDropDown.Visible)
                state = privateStateDropDown.SelectedItem.Text;
            else
                state = dat.stripHTML(privateStateTextBox.Text.Trim());

            city = dat.stripHTML(cityTextBox.Text.Trim());
        }
        

        int zipParam = 0;

        string zips = "";
        string MajorCityID = "";
        if (country == "223")
        {
            DataView dvAllZips;

            if (zip != "")
                dvAllZips = dat.GetDataDV("SELECT * FROM MajorZips MZ, MajorCities MC WHERE " +
                "MZ.MajorCityID=MC.ID AND MajorCityZip='" + zip + "'");
            else
            {
                DataView dvS = dat.GetDataDV("SELECT * FROM State WHERE state_2_code = '" + state + "'");
                string tmp = "";
                if (dvS.Count > 0)
                    tmp = dvS[0]["state_name"].ToString();
                else
                {
                    tmp = state;
                }
                dvAllZips = dat.GetDataDV("SELECT * FROM MajorZips MZ, MajorCities MC WHERE " +
                "MZ.MajorCityID=MC.ID AND MajorCity='" + city + "' AND State = '" + tmp + "'");

                if (dvAllZips.Count == 0)
                {
                    dvAllZips = dat.GetDataDV("SELECT * FROM MajorZips MZ, MajorCities MC WHERE " +
                "MZ.MajorCityID=MC.ID AND State = '" + tmp + "'");
                    if (dvAllZips.Count == 0)
                    {
                        dvAllZips = dat.GetDataDV("SELECT * FROM MajorZips MZ, MajorCities MC WHERE " +
                    "MZ.MajorCityID=MC.ID");

                        zip = dvAllZips[0]["MajorCityZip"].ToString();
                    }

                }
            }


            //If user's zip falls under a major city, just get all the zips that apply to that city
            if (dvAllZips.Count > 0)
            {
                MajorCityID = dvAllZips[0]["MajorCityID"].ToString();
            }
            //If zip doesn't fall under a major city, get the closest major city
            //Zip array will include all zips from that major city and all zips 
            //the same distance away as the current zip.
            else
            {
                DataView dvLatsLongs = dat.GetDataDV("SELECT Latitude, Longitude FROM ZipCodes WHERE Zip='" +
                        zip + "'");

                //If not found, find closest Latitude and Longitude
                zipParam = int.Parse(zip);
                if (dvLatsLongs.Count == 0)
                {
                    dvLatsLongs = null;
                    while (dvLatsLongs == null)
                    {
                        dvLatsLongs = dat.GetDataDV("SELECT Latitude, Longitude FROM ZipCodes WHERE Zip='" + zipParam++ + "'");
                        if (dvLatsLongs.Count > 0)
                        {

                        }
                        else
                        {
                            dvLatsLongs = null;
                        }
                    }
                }

                DataView dvMajors = dat.GetDataDV("SELECT dbo.GetDistance(" + dvLatsLongs[0]["Longitude"].ToString() +
                    ", " + dvLatsLongs[0]["Latitude"].ToString() + ", ZC.Longitude, ZC.Latitude) AS " +
                    "Distance, ZC.Zip, MC.MajorCity, MC.State, MZ.MajorCityID, ZC.Longitude, ZC.Latitude FROM MajorZips MZ, ZipCodes ZC, MajorCities MC " +
                    "WHERE MZ.MajorCityZip=ZC.Zip AND MZ.MajorCityID=MC.ID ORDER BY Distance ASC");

                //All zips in the closest major city
                MajorCityID = dvMajors[0]["MajorCityID"].ToString();
            }
        }
        else
        {
            MajorCityID = "";
        }
        return MajorCityID;
    }

    protected void ImportFromCraigslist(object sender, EventArgs e)
    {
        try
        {
            if (CraigslistURLTextBox.Text.Trim() != "")
            {
                Data dat = new Data(DateTime.Now);
                CraigslistURLTextBox.Text = dat.stripHTML(CraigslistURLTextBox.Text.Trim());
                if (dat.isItAURL(CraigslistURLTextBox.Text))
                {
                    Hashtable oneEvent = dat.grabEvent(CraigslistURLTextBox.Text);

                    if (oneEvent.ContainsKey("header"))
                    {
                        //fill the event's information
                        if (oneEvent.ContainsKey("startTime"))
                        {
                            DateTime startDate = (DateTime)oneEvent["startTime"];
                            DateTime endDate = (DateTime)oneEvent["endTime"];
                            DateSelectionsListBox.Items.Add(startDate.Month.ToString() + "/" + startDate.Day.ToString() +
                                "/" + startDate.Year.ToString() + " " + startDate.TimeOfDay.Hours.ToString() + ":" +
                                startDate.TimeOfDay.Minutes.ToString() + " -- " +
                                endDate.Month.ToString() + "/" + endDate.Day.ToString() +
                                "/" + endDate.Year.ToString() + " " + endDate.TimeOfDay.Hours.ToString() + ":" +
                                endDate.TimeOfDay.Minutes.ToString());
                        }

                        EventNameTextBox.Text = oneEvent["header"].ToString();
                        DescriptionTextBox.Content = oneEvent["description"].ToString();

                        //do images
                        string[] delim = { ";" };
                        string[] tokens;
                        if (oneEvent["images"].ToString().Trim() != "")
                        {
                            MainAttractionCheck.Checked = true;
                            MainAttractionPanel.Enabled = true;
                            MainAttractionPanel.Visible = true;
                            PictureNixItButton.Visible = true;
                            UploadedVideosAndPics.Visible = true;

                            tokens = oneEvent["images"].ToString().Trim().Split(delim, StringSplitOptions.RemoveEmptyEntries);

                            ListItem newListItem;
                            foreach (string token in tokens)
                            {
                                newListItem = new ListItem(token, "ImgPathAbsolute");
                                PictureCheckList.Items.Add(newListItem);
                            }
                        }

                        LocaleRadioButtonList.SelectedValue = "1";
                        SwitchLocationPanels(new object(), new EventArgs());

                        CLErrorLabel.Text = "Your information has been entered below. Include your address and categories so that people can find your event faster.";

                        //do categories
                        if (oneEvent["categories"].ToString().Trim() != "")
                        {
                            SetCLCategories(oneEvent["categories"].ToString().Trim());
                        }
                        FindPrice();
                    }
                    else
                    {
                        CLErrorLabel.Text = "Could not parse the information from the URL you provided. Please make sure you provided the full link of a craigslist event.";
                    }
                }
                else
                {
                    CLErrorLabel.Text = "The link you've provided is not in a URL format.";
                }
            }
            else
            {
                CLErrorLabel.Text = "Please include the URL.";
            }
        }
        catch (Exception ex)
        {
            CLErrorLabel.Text = "We could not find a craigslist event on the url of the web page you provided.";
        }
    }

    protected void SetCLCategories(string categories)
    {
        CategoryTree.DataBind();
        RadTreeView2.DataBind();

        try
        {
            if (categories.Length > 0)
            {
                List<Telerik.Web.UI.RadTreeNode> list = (List<Telerik.Web.UI.RadTreeNode>)CategoryTree.GetAllNodes();
                //List<Telerik.Web.UI.RadTreeNode> list2 = (List<Telerik.Web.UI.RadTreeNode>)RadTreeView1.GetAllNodes();
                List<Telerik.Web.UI.RadTreeNode> list3 = (List<Telerik.Web.UI.RadTreeNode>)RadTreeView2.GetAllNodes();
                //List<Telerik.Web.UI.RadTreeNode> list4 = (List<Telerik.Web.UI.RadTreeNode>)RadTreeView3.GetAllNodes();

                for (int i = 0; i < list.Count; i++)
                {
                    if (categories.Contains(";"+list[i].Value+";"))
                        list[i].Checked = true;
                }

                for (int i = 0; i < list3.Count; i++)
                {
                    if (categories.Contains(";" + list3[i].Value + ";"))
                        list3[i].Checked = true;
                }
            }
            Session["EventCategoriesSet"] = null;
            //Session["EventCategoriesSet"] = "notnull";
        }
        catch (Exception ex)
        {
            MessagePanel.Visible = true;
            YourMessagesLabel.Text = ex.ToString();
        }
    }

    protected void SwitchLocationPanels(object sender, EventArgs e)
    {
        if (LocaleRadioButtonList.SelectedValue == "1")
        {
            OneTimeVenuePanel.Visible = true;
            ExistingVenuePanel.Visible = false;
        }
        else
        {
            OneTimeVenuePanel.Visible = false;
            ExistingVenuePanel.Visible = true;
        }
    }

    protected void FillThePrice(object sender, EventArgs e)
    {
        FindPrice();
        EverythingUpdatePanel.Update();
    }
}

