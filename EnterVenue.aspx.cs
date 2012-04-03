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

public partial class EnterVenue : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["RedirectTo"] = Request.Url.AbsoluteUri;
        if (Session["User"] == null)
            Response.Redirect("~/login");

        //if (!Request.IsSecureConnection)
        //{
        //    string absoluteUri = Request.Url.AbsoluteUri;
        //    Response.Redirect(absoluteUri.Replace("http://", "https://"), true);
        //}

        HtmlLink lk = new HtmlLink();
        HtmlHead head = (HtmlHead)Page.Header;
        lk.Attributes.Add("rel", "canonical");
        lk.Href = "https://hippohappenings.com/enter-locale";
        head.Controls.AddAt(0, lk);

        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        #region Take care of buttons
        //DetailsOnwardsButton.SERVER_CLICK += Onwards;
        //ImageButton2.SERVER_CLICK += Onwards;
        //DescriptionOnwardsButton.SERVER_CLICK += Backwards;
        //ImageButton3.SERVER_CLICK += Onwards;
        //MediaOnwardsButton.SERVER_CLICK += Backwards;
        //ImageButton9.SERVER_CLICK += Onwards;
        //CategoryOnwardButton.SERVER_CLICK += Backwards;
        //ImageButton12.SERVER_CLICK += Backwards;
        PostItButton.SERVER_CLICK += PostIt;
        ImageButton6.SERVER_CLICK += VideoUpload_Click;
        ImageButton7.SERVER_CLICK += YouTubeUpload_Click;
        PictureNixItButton.SERVER_CLICK += SliderNixIt;
        buttonSubmit.PostBackUrl = Request.Url.AbsoluteUri;
        ImageButton11.SERVER_CLICK += SuggestCategoryClick;
        AddHoursButton.SERVER_CLICK += AddHours;
        RemoveHoursButton.SERVER_CLICK += RemoveHours;
        AddEventButton.SERVER_CLICK += AddEvent;
        RemoveEventButton.SERVER_CLICK += RemoveEvent;
        //BlueButton7.SERVER_CLICK += FillChart;
        //BlueButton9.SERVER_CLICK += CheckedFeatured;
        //BlueButton10.SERVER_CLICK += UnCheckedFeatured;
        //BlueButton1.SERVER_CLICK += AddFeaturedDates;
        //BlueButton4.SERVER_CLICK += RemoveFeaturedDates;
        //BlueButton5.SERVER_CLICK += AddSearchTerm;
        //BlueButton6.SERVER_CLICK += RemoveSearchTerm;
        //BlueButton19.SERVER_CLICK += Onwards;
        //BlueButton20.SERVER_CLICK += Backwards;
        //BlueButton2.SERVER_CLICK += Onwards;
        //BlueButton3.SERVER_CLICK += Backwards;
        #endregion

        //#region Take Care of Feature Features
        ////Limit feature calendar to 30 days
        //FeaturedDatePicker.MinDate = DateTime.Now;
        //FeaturedDatePicker.MaxDate = DateTime.Now.AddDays(30);

        //foreach (ListItem item2 in FeatureDatesListBox.Items)
        //{
        //    if (item2.Value == "Disabled")
        //        item2.Attributes.Add("class", "DisabledListItem");
        //}
        //#endregion

        DataView dv = dat.GetDataDV("SELECT * FROM TermsAndConditions");
        Literal lit1 = new Literal();
        lit1.Text = dv[0]["Content"].ToString();
        TACTextBox.Controls.Add(lit1);

        if (!IsPostBack)
        {
            //Session.Remove("Featured");
            //Session["Featured"] = null;

            Session["CategoriesSet"] = null;
            Session.Remove("CategoriesSet");
            

            CountryDropDown.SelectedValue = "223";
            DataSet dsCountries = dat.GetData("SELECT * FROM State WHERE country_id=223");
           
            StateDropDown.DataSource = dsCountries;
            StateDropDown.DataTextField = "state_2_code";
            StateDropDown.DataValueField = "state_id";
            StateDropDown.DataBind();
            StateDropDownPanel.Visible = true;
            StateTextBoxPanel.Visible = false;

            ////Take Care of Billing Locaion
            //dsCountries = dat.GetData("SELECT * FROM Countries");
            //BillingCountry.DataSource = dsCountries;
            //BillingCountry.DataTextField = "country_name";
            //BillingCountry.DataValueField = "country_id";
            //BillingCountry.DataBind();

            //BillingCountry.SelectedValue = "223";
            //BillingStateTextBox.Text = "AL";
            //CardTypeDropDown.Items.Add(new ListItem("Visa", "Visa"));
            //CardTypeDropDown.Items.Add(new ListItem("MasterCard", "MasterCard"));
            //CardTypeDropDown.Items.Add(new ListItem("Discover", "Discover"));
            //CardTypeDropDown.Items.Add(new ListItem("American Express", "Amex"));
            //ChangeStateAction(BillingCountry, new EventArgs());
        }

        try
        {
            Session["RedirectTo"] = Request.Url.AbsoluteUri;


            if (Session["User"] != null)
            {

                //ASP.controls_ads_ascx Ads1 = new ASP.controls_ads_ascx();
                //Ads1.DATA_SET = dat.RetrieveAds(Session["User"].ToString(), false);
                //Ads1.MAIN_AD_DATA_SET = dat.RetrieveMainAds(Session["User"].ToString());
                //Ads1.Controls.Add(Ads1);
                BigEventPanel.Visible = true;
                LoggedOutPanel.Visible = false;
                LoadControls();

                if (!IsPostBack)
                {
                    if (Request.QueryString["ID"] != null)
                    {

                        fillVenue();

                    }
                }


            }
            else
            {
                WelcomeLabel.Text = "<h1 class=\"SideColumn\">You Need To Be <a class=\"NavyLink12\" href=\"login\">Logged In</a> to Post A Locale.</h1><br/>On Hippo Happenings, the users have all the power to post event, bulleins, trips and locales alike. In order to do so, and for us to maintain clean and manageable content,  "
                    + " we require that you <a class=\"AddLink\" href=\"Register.aspx\">create an account</a> with us. <a class=\"AddLink\" href=\"login\">Log in</a> if you have an account already. " +
                    "Having an account with us will allow you to do many other things as well. You will be able to add events to your calendar, communicate with others going to events, add your friends, filter content based on your preferences, " +
                    " feature your bulletins thoughout the site and much more. <br/><br/>So let's do it <a class=\"AddLink\" style=\"font-size: 16px;\" href=\"Register.aspx\">Register!</a>";

                BigEventPanel.Visible = false;
                LoggedOutPanel.Visible = true;

            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString();
            LoggedOutPanel.Visible = true;
            BigEventPanel.Visible = false;
        }

        
        
    }

    protected void LoadControls()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        YourMessagesLabel.Text = "";
        MessagePanel.Visible = false;

        if (!IsPostBack)
        {


            //DataSet ds = dat.GetData("SELECT * FROM Categories");
            //CategoriesCheckBoxes.DataSource = ds;
            //CategoriesCheckBoxes.DataTextField = "CategoryName";
            //CategoriesCheckBoxes.DataValueField = "ID";
            //CategoriesCheckBoxes.DataBind();



            //ShowVideoPictureLiteral.Text = "";

        }
    }

    protected bool EnableOwnerPanel(ref bool ownerUpForGrabs)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        bool isOwner = false;
        ownerUpForGrabs = false;
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        if (Request.QueryString["ID"] != null)
        {
            DataSet dsVenue = dat.GetData("SELECT * FROM Venues WHERE ID=" + Request.QueryString["ID"].ToString());
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

    protected void fillVenue()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        //If there is no owner, the venue is up for grabs.
        DataSet dsVenue = dat.GetData("SELECT * FROM Venues WHERE ID=" + Request.QueryString["ID"].ToString());
        DataView dv = new DataView(dsVenue.Tables[0], "", "", DataViewRowState.CurrentRows);

        bool ownerUpForGrabs = false;
        bool isOwner = EnableOwnerPanel(ref ownerUpForGrabs);

        if (isOwner || ownerUpForGrabs)
        {
            PictureNixItButton.Visible = true;
        }

        if (dsVenue.Tables.Count > 0)
            if (dsVenue.Tables[0].Rows.Count > 0)
            {
                TitleLabel.Text = "You are submitting changes for locale '" + dsVenue.Tables[0].Rows[0]["Name"].ToString() +
                    "'";
                VenueNameTextBox.Text = dsVenue.Tables[0].Rows[0]["Name"].ToString();
                PhoneTextBox.Text = dsVenue.Tables[0].Rows[0]["Phone"].ToString();
                EmailTextBox.Text = dsVenue.Tables[0].Rows[0]["Email"].ToString();
                WebSiteTextBox.Text = dsVenue.Tables[0].Rows[0]["Web"].ToString();
                DescriptionTextBox.Content = dsVenue.Tables[0].Rows[0]["Content"].ToString();
                ZipTextBox.Text = dsVenue.Tables[0].Rows[0]["Zip"].ToString();
                CityTextBox.Text = dsVenue.Tables[0].Rows[0]["City"].ToString();
                DataSet dsCountries = dat.GetData("SELECT * FROM State WHERE country_id=" + dsVenue.Tables[0].Rows[0]["Country"].ToString());
                CountryDropDown.DataBind();
                CountryDropDown.Items.FindByValue(dsVenue.Tables[0].Rows[0]["Country"].ToString()).Selected = true;
                bool showStateText = false;
                if (dsCountries.Tables.Count > 0)
                    if (dsCountries.Tables[0].Rows.Count > 0)
                    {
                        StateDropDown.DataSource = dsCountries;
                        StateDropDown.DataTextField = "state_2_code";
                        StateDropDown.DataValueField = "state_id";
                        StateDropDown.DataBind();
                        StateDropDown.SelectedValue = StateDropDown.Items.FindByText(dsVenue.Tables[0].Rows[0]["State"].ToString()).Value;
                        StateDropDownPanel.Visible = true;
                        StateTextBoxPanel.Visible = false;
                    }
                    else
                        showStateText = true;
                else
                    showStateText = true;

                if (showStateText)
                {
                    StateTextBoxPanel.Visible = true;
                    StateDropDownPanel.Visible = false;
                    StateTextBox.Text = dsVenue.Tables[0].Rows[0]["State"].ToString();
                }
                char[] delimB = { ';' };
                if (dsVenue.Tables[0].Rows[0]["Country"].ToString() == "223")
                {
                    USPanel.Visible = true;
                    InternationalPanel.Visible = false;
                    string str = dsVenue.Tables[0].Rows[0]["Address"].ToString();
                    
                    string[] atokens = str.Split(delimB);

                    StreetNumberTextBox.Text = atokens[0];
                    try
                    {
                        string temp = atokens[1][0].ToString().ToUpper() + atokens[1].Substring(1, atokens[1].Length - 1);
                        StreetNameTextBox.Text = temp;
                        StreetDropDown.Items.FindByText(atokens[2]).Selected = true;
                        AptNumberTextBox.Text = atokens[3];
                        if (atokens[3].Contains("Suite"))
                        {
                            AptNumberTextBox.Text = AptNumberTextBox.Text.Replace("Suite ", "");
                            AptDropDown.Items.FindByValue("1").Selected = true;
                        }
                        else
                        {
                            AptNumberTextBox.Text = AptNumberTextBox.Text.Replace("Apt ", "");
                            AptDropDown.Items.FindByValue("2").Selected = true;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                        
                    
                }
                else
                {
                    string[] atokensI = dsVenue.Tables[0].Rows[0]["Address"].ToString().Split(delimB);
                    LocationTextBox.Text = atokensI[0];
                    if (atokensI.Length > 1)
                    {
                        if (atokensI[1].Trim() != "")
                        {
                            AptNumberTextBox.Text = atokensI[1];
                            if (atokensI[1].Contains("Suite"))
                            {
                                AptNumberTextBox.Text = AptNumberTextBox.Text.Replace("Suite ", "");
                                AptDropDown.Items.FindByValue("1").Selected = true;
                            }
                            else
                            {
                                AptNumberTextBox.Text = AptNumberTextBox.Text.Replace("Apt ", "");
                                AptDropDown.Items.FindByValue("2").Selected = true;
                            }
                        }
                    }
                    USPanel.Visible = false;
                    InternationalPanel.Visible = true;
                }


                //string mediaCategory = dsVenue.Tables[0].Rows[0]["mediaCategory"].ToString();
                string youtube = dsVenue.Tables[0].Rows[0]["YouTubeVideo"].ToString();
                char[] delim = { '\\' };
                char[] delim4 = { ';' };
                string[] youtokens = youtube.Split(delim4);
                if (youtokens.Length > 0)
                {
                    MainAttractionCheck.Checked = true;
                    MainAttractionPanel.Enabled = true;
                    MainAttractionPanel.Visible = true;

                    for (int i = 0; i < youtokens.Length; i++)
                    {
                        if (youtokens[i].Trim() != "")
                        {
                            ListItem newListItem = new ListItem("You Tube ID: " + youtokens[i], youtokens[i]);
                            if (isOwner)
                            {
                                newListItem.Enabled = true;
                            }
                            else
                            {
                                newListItem.Enabled = false;
                            }
                            PictureCheckList.Items.Add(newListItem);
                        }
                    }
                }
                if (System.IO.Directory.Exists(MapPath(".") + "\\VenueFiles\\" + Request.QueryString["ID"].ToString() +
                        "\\Slider\\"))
                {
                    string[] fileArray = System.IO.Directory.GetFiles(MapPath(".") + "\\VenueFiles\\" + Request.QueryString["ID"].ToString() +
                        "\\Slider\\");
                    if (fileArray.Length > 0)
                    {
                        MainAttractionCheck.Checked = true;
                        MainAttractionPanel.Enabled = true;
                        MainAttractionPanel.Visible = true;

                        UploadedVideosAndPics.Visible = true;

                        for (int i = 0; i < fileArray.Length; i++)
                        {
                            string[] fileTokens = fileArray[i].Split(delim);
                            string nameFile = fileTokens[fileTokens.Length - 1];

                            DataView dvV = dat.GetDataDV("SELECT * FROM Venue_Slider_Mapping WHERE PictureName='" + nameFile + "' AND VenueID=" + Request.QueryString["ID"].ToString());
                            if (dvV.Count > 0)
                            {
                                ListItem newListItem = new ListItem(dvV[0]["RealPictureName"].ToString(), nameFile);
                                if (isOwner)
                                {
                                    newListItem.Enabled = true;
                                }
                                else
                                {
                                    newListItem.Enabled = false;
                                }
                                PictureCheckList.Items.Add(newListItem);
                            }
                        }
                    }
                }
               
            }
        
        FillHoursAndEvents(Request.QueryString["ID"].ToString());

        //FeatureDatesListBox.Items.Clear();
        //if (bool.Parse(dv[0]["Featured"].ToString()))
        //{
        //    char[] delim = { ';' };
        //    string[] tokens = dv[0]["DaysFeatured"].ToString().Split(delim, StringSplitOptions.RemoveEmptyEntries);
        //    ListItem item;
        //    foreach (string token in tokens)
        //    {
        //        item = new ListItem(token);
        //        item.Attributes.Add("class", "DisabledListItem");
        //        item.Value = "Disabled";
        //        FeatureDatesListBox.Items.Add(item);
        //    }
        //}



        Session["CategoriesSet"] = null;
        //if (Request.QueryString["Feature"] != null)
        //{
        //    if (bool.Parse(Request.QueryString["Feature"]))
        //    {
        //        ChangeSelectedTab(0, 1);
        //        ChangeSelectedTab(1, 2);
        //        ChangeSelectedTab(2, 3);
        //        ChangeSelectedTab(3, 4);
        //        ChangeSelectedTab(4, 5);
        //        //FeaturePanel.Visible = true;
        //        //FindPrice();
        //    }
        //}
        SetCategories();
    }

    protected void SetCategories()
    {
        CategoryTree.DataBind();
        RadTreeView2.DataBind();
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataView dvCategories = dat.GetDataDV("SELECT * FROM Venue_Category WHERE VENUE_ID=" +
            Request.QueryString["ID"].ToString());

        
        if (dvCategories.Count > 0)
        {
            List<Telerik.Web.UI.RadTreeNode> list = (List<Telerik.Web.UI.RadTreeNode>)CategoryTree.GetAllNodes();
            //List<Telerik.Web.UI.RadTreeNode> list2 = (List<Telerik.Web.UI.RadTreeNode>)RadTreeView1.GetAllNodes();
            List<Telerik.Web.UI.RadTreeNode> list3 = (List<Telerik.Web.UI.RadTreeNode>)RadTreeView2.GetAllNodes();
            //List<Telerik.Web.UI.RadTreeNode> list4 = (List<Telerik.Web.UI.RadTreeNode>)RadTreeView3.GetAllNodes();

            for (int i = 0; i < list.Count; i++)
            {
                dvCategories.RowFilter = "Category_ID=" + list[i].Value;

                if (dvCategories.Count > 0)
                    list[i].Checked = true;

            }

            for (int i = 0; i < list3.Count; i++)
            {
                dvCategories.RowFilter = "Category_ID=" + list3[i].Value;

                if (dvCategories.Count > 0)
                    list3[i].Checked = true;

            }
        }

        Session["CategoriesSet"] = "notnull";

    }

    protected void GetCategoriesFromTree(bool isUpdate, bool isOwner, 
        ref Telerik.Web.UI.RadTreeView CategoryTree, DataView dvCat, string ID, string revisionID)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        string message = "";
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        for (int i = 0; i < CategoryTree.Nodes.Count; i++)
        {
            
                GetCategoriesFromNode(isUpdate, isOwner, CategoryTree.Nodes[i], dvCat, ID, revisionID);
                //Recurse if there is children
        }
    }

    protected void GetCategoriesFromNode(bool isUpdate, bool isOwner,
        Telerik.Web.UI.RadTreeNode TreeNode, DataView dvCat, string ID, string revisionID)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        string ownerID = "";
        bool isOwnerUpForGrabs = dat.IsOwnerUpForGrabs(ID, ref ownerID, ref isOwner, true);
        if (TreeNode.Checked && TreeNode.Enabled)
        {
            dvCat.RowFilter = "CATEGORY_ID=" + TreeNode.Value;
                //distinctHash.Add(CategoriesCheckBoxes.Items[i], 21);
                //tagHash.Add(CategoriesCheckBoxes.Items[i], "22");

                if (isUpdate)
                {
                    if (isOwner || isOwnerUpForGrabs)
                    {
                        if (dvCat.Count == 0)
                        {
                            dat.Execute("INSERT INTO Venue_Category (VENUE_ID, CATEGORY_ID, tagSize) VALUES("
                                + ID + ", " + TreeNode.Value + ", 22)");
                        }
                    }
                    else
                    {
                        if (dvCat.Count == 0)
                        {
                            dat.Execute("INSERT INTO VenueCategoryRevisions (AddOrRemove, VenueID, CatID, modifierID, RevisionID, DATE) " +
                                "VALUES(1, " + ID + ", " + TreeNode.Value + ", " + Session["User"].ToString() + ", " + revisionID + ", '" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")) + "')");
                        }
                        else
                        {
                            //dat.Execute("INSERT INTO VenueCategoryRevisions (AddOrRemove, VenueID, CatID, modifierID, RevisionID) " +
                            //    "VALUES(0, " + ID + ", " + CategoryTree.Nodes[i].Value + ", " + Session["User"].ToString() + ", " + revisionID + ")");
                        }

                    }
                }
                else
                {
                    dat.Execute("INSERT INTO Venue_Category (VENUE_ID, CATEGORY_ID, tagSize) VALUES("
                            + ID + ", " + TreeNode.Value + ", 22)");
                }

            }
            else if (!TreeNode.Checked)
            {
                dvCat.RowFilter = "CATEGORY_ID=" + TreeNode.Value;

                if (isUpdate)
                {
                    if (isOwner || isOwnerUpForGrabs)
                    {
                        if (dvCat.Count == 0)
                        {
                        }
                        else
                        {
                            dat.Execute("DELETE FROM Venue_Category WHERE VENUE_ID=" + ID +
                                " AND CATEGORY_ID = " + TreeNode.Value);

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
                                dat.Execute("DELETE FROM Venue_Category WHERE VENUE_ID=" + ID +
                                " AND CATEGORY_ID = " + TreeNode.Value);
                            }
                            else
                            {
                                dat.Execute("INSERT INTO VenueCategoryRevisions (AddOrRemove, VenueID, CatID, modifierID, RevisionID, DATE) " +
                                    "VALUES(0, " + ID + ", " + TreeNode.Value + ", " + Session["User"].ToString() + ", " + revisionID + ", '" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")) + "')");

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

    }
   
    protected void PostIt(object sender, EventArgs e)
    {
        
        if (AgreeCheckBox.Checked)
        {
            string validateMessage = ValidatePage();
            if (validateMessage == "success")
            {
                bool chargeCard = false;
                string message = "";
                HttpCookie cookie = Request.Cookies["BrowserDate"];

                DateTime isn = DateTime.Now;

                if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
                    isn = DateTime.Now;
                DateTime isNow = isn;
                Data dat = new Data(isn); string email = "";

                //AuthorizePayPal d = new AuthorizePayPal();

                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
                conn.Open();

                //Add case for if Paypal is filled in...
                //Authorize Credit Card
                decimal price = 0.00M;
                bool goOn = true;
                //string transactionID = "";
                //if (TotalLabel.Text.Trim() != "")
                //{
                //    if (decimal.TryParse(TotalLabel.Text.Trim(), out price))
                //    {
                //        if (price != 0.00M)
                //        {
                //            if (FirstNameTextBox.Text.Trim() == "" || LastNameTextBoxtBox.Text.Trim() == "" ||
                //               BillingStreetAddressTextBox.Text.Trim() == "" || BillingCityTextBox.Text.Trim() == "" ||
                //               BillingZipCodeTextBox.Text.Trim() == "" || BillingStateTextBox.Text.Trim() == "" ||
                //               CardNumberTextBox.Text.Trim() == "" || CSVTextBox.Text.Trim() == "")
                //            {
                //                goOn = false;
                //                Session["Featured"] = false;
                //                message = "Please fill in all of the billing information.";
                //            }
                //            else
                //            {
                //                goOn = false;
                //                Session["Featured"] = false;
                //                string country = dat.GetDataDV("SELECT country_2_code FROM Countries WHERE country_id=" + BillingCountry.SelectedValue)[0]["country_2_code"].ToString();
                //                com.paypal.sdk.util.NVPCodec status = d.DoPayment("Authorization", TotalLabel.Text, CardTypeDropDown.SelectedValue, CardNumberTextBox.Text.Trim(),
                //                    ExpirationMonth.SelectedItem.Text, ExpirationYear.SelectedItem.Text, CSVTextBox.Text.Trim(), FirstNameTextBox.Text.Trim(), LastNameTextBoxtBox.Text.Trim(),
                //                    BillingStreetAddressTextBox.Text.Trim(), BillingCityTextBox.Text, BillingStateTextBox.Text, country, BillingZipCodeTextBox.Text.Trim(), dat.GetIP());
                //                message = status.ToString();
                //                string successORFailure = status["ACK"];

                //                switch (successORFailure.ToLower())
                //                {
                //                    case "failure":
                //                        goOn = false;
                //                        Session["Featured"] = false;
                //                        message = status["L_LONGMESSAGE0"];
                //                        break;
                //                    case "successwithwarning":
                //                        goOn = false;
                //                        Session["Featured"] = false;
                //                        message = status["L_SHORTMESSAGE0"];
                //                        if (message == "Transaction approved but with invalid CSC format.")
                //                            message = "Your CVC/CSV format for this card is not valid.";
                //                        break;
                //                    case "success":
                //                        chargeCard = true;
                //                        transactionID = status["TRANSACTIONID"];
                //                        Session["TransID"] = transactionID;
                //                        goOn = true;
                //                        Session["Featured"] = true;
                //                        break;
                //                    default:
                //                        goOn = false;
                //                        Session["Featured"] = false;
                //                        message = "There was an internal problem. Please contact support at: help@hippohappenings.com. Please include as much detail as possible about what you are trying to do.";
                //                        foreach (string key in status.Keys)
                //                        {
                //                            message += "key: " + key.ToString() + ", value: " + status[key].ToString() + "<br/>";
                //                        }
                //                        break;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            goOn = true;
                //            Session["Featured"] = false;
                //        }
                //    }
                //    else
                //    {
                //        goOn = true;
                //        Session["Featured"] = false;
                //    }
                //}
                //else
                //{
                //    goOn = true;
                //    Session["Featured"] = false;
                //}

                if (goOn)
                {

                    bool mediaChanged = false;
                    bool contentChanged = false;

                    string mediaCat = "0";
                    if (PictureCheckList.Items.Count > 0)
                        mediaCat = "1";

                    bool isUpdate = false;
                    bool isOwner = false;

                    string ownerID = "";
                    DataSet dsVenue = new DataSet();
                    bool ownerUpForGrabs = false;

                    DataView dvVenue = new DataView();
                    bool wasFeatured = false;
                    if (Request.QueryString["ID"] != null)
                    {
                        dsVenue = dat.GetData("SELECT * FROM Venues WHERE ID=" + Request.QueryString["ID"].ToString());
                        dvVenue = new DataView(dsVenue.Tables[0], "", "", DataViewRowState.CurrentRows);

                        //wasFeatured = bool.Parse(dvVenue[0]["Featured"].ToString());
                        isUpdate = true;
                        ownerUpForGrabs = dat.IsOwnerUpForGrabs(Request.QueryString["ID"].ToString(), ref ownerID, ref isOwner, true);
                    }

                    string state = "";
                    if (StateDropDownPanel.Visible)
                        state = StateDropDown.SelectedItem.Text;
                    else
                        state = StateTextBox.Text;

                    //We already do this in 'Onwards' method
                    //SqlCommand cmd = new SqlCommand("SELECT * FROM Venues WHERE Name=@name AND City=@city AND State=@state ", conn);
                    //cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = VenueNameTextBox.Text;
                    //cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = CityTextBox.Text;
                    //cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = state;
                    //DataSet ds = new DataSet();
                    //SqlDataAdapter da = new SqlDataAdapter(cmd);
                    //da.Fill(ds);

                    //bool cont = false;

                    //if (ds.Tables.Count > 0)
                    //    if (ds.Tables[0].Rows.Count > 0 && !isUpdate)
                    //    {
                    //        MessagePanel.Visible = true;
                    //        YourMessagesLabel.Text += "<br/><br/>A venue under this name already exists in this City and State. To edit the details of this particular venue please contact Hippo Happenings " + "<a class=\"AddGreenLink\" href=\"contact-us\">here</a>. Otherwise, please modify the name slightly.";
                    //    }
                    //    else
                    //        cont = true;
                    //else
                    //    cont = true;
                    bool cont = true;
                    if (cont)
                    {
                        string command = "";

                        if (isUpdate)
                        {
                            if (isOwner || ownerUpForGrabs)
                                command = "UPDATE Venues SET DaysFeatured=@fetDays, Featured=@fet, Name=@name, Owner=@owner, City=@city, Edit='False', Email=@email, Phone=@phone, State=@state, Country=@country, Zip=@zip, Address=@address, "
                                + " EditedByUser=@user, Content=@content, Web=@web, mediaCategory=" + mediaCat + ", LastEditOn=@dateE WHERE ID=" + Request.QueryString["ID"].ToString();
                            else
                            {
                                command = "INSERT INTO VenueRevisions (Web, modifierID, VenueID, [Content], " +
                                 "City, State, Country, Zip, Name, Email, Phone, Address, DATE)"
                                 + " VALUES(@web, " + Session["User"].ToString() + "," + Request.QueryString["ID"].ToString() +
                                 ", @content, @city, @state, @country, @zip, " +
                                 "@name, @email, @phone, @address, '" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")) + "')";

                            }
                        }
                        else
                            command = "INSERT INTO Venues (DaysFeatured, Featured, Web, Owner, City, State, Country, Zip, Edit, Name, Email, Phone, Address, CreatedByUser,Content, mediaCategory, Rating, PostedOn) "
                             + "VALUES(@fetDays, @fet, @web, @owner, @city, @state, @country, @zip, 'False', @name, @email, @phone, @address, @user, @content, " + mediaCat + ", 0, @dateE)";

                        string locationStr = "";
                        string apt = "";
                        if (AptNumberTextBox.Text.Trim() != "")
                            apt = AptDropDown.SelectedItem.Text + " " + AptNumberTextBox.Text.Trim().ToLower();
                        if (CountryDropDown.SelectedValue == "223")
                        {
                            locationStr = StreetNumberTextBox.Text.Trim().ToLower() + ";" + StreetNameTextBox.Text.Trim().ToLower()
                                + ";" + StreetDropDown.SelectedItem.Text + ";" + apt;
                        }
                        else
                        {
                            locationStr = LocationTextBox.Text.Trim().ToLower() + ";" + apt;
                        }


                        SqlCommand cmd = new SqlCommand(command, conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("@dateE", SqlDbType.DateTime).Value = DateTime.Now;


                        string fetDays = "";
                        //foreach (ListItem item in FeatureDatesListBox.Items)
                        //{
                        //    fetDays += ";" + item.Text + ";";
                        //}

                        if (isUpdate && (isOwner || ownerUpForGrabs) || !isUpdate)
                        {
                            //if (wasFeatured)
                            //{
                            //    cmd.Parameters.Add("@fet", SqlDbType.Bit).Value = true;
                            //    if (FeaturePanel.Visible)
                            //    {
                            //        cmd.Parameters.Add("@fetDays", SqlDbType.NVarChar).Value = fetDays;
                            //    }
                            //    else
                            //        cmd.Parameters.Add("@fetDays", SqlDbType.NVarChar).Value = dvVenue[0]["DaysFeatured"].ToString();
                            //}
                            //else
                            //{
                            cmd.Parameters.Add("@fet", SqlDbType.Bit).Value = false;
                            //FeaturePanel.Visible;
                            //if (FeaturePanel.Visible)
                            //{
                            //    cmd.Parameters.Add("@fetDays", SqlDbType.NVarChar).Value = fetDays;
                            //}
                            //else
                            //{
                            cmd.Parameters.Add("@fetDays", SqlDbType.NVarChar).Value = DBNull.Value;
                            //}
                            //}
                        }

                        if (isUpdate && !isOwner)
                        {
                            if (ownerUpForGrabs)
                            {
                                if (OwnerCheckBox.Checked)
                                    cmd.Parameters.Add("@owner", SqlDbType.Int).Value = Session["User"].ToString();
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

                            cmd.Parameters.Add("@user", SqlDbType.Int).Value = Session["User"].ToString();

                            if (dsVenue.Tables[0].Rows[0]["Name"].ToString() != VenueNameTextBox.Text.Trim())
                            {
                                cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = VenueNameTextBox.Text.Trim();
                                contentChanged = true;
                            }
                            else
                            {
                                if (ownerUpForGrabs)
                                {
                                    cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = dsVenue.Tables[0].Rows[0]["Name"].ToString();
                                }
                                else
                                {
                                    cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = DBNull.Value;
                                }
                            }

                            if (dsVenue.Tables[0].Rows[0]["Email"].ToString() != EmailTextBox.Text.Trim())
                            {
                                if (EmailTextBox.Text.Trim() != "")
                                    cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = EmailTextBox.Text.Trim();
                                else
                                    cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = DBNull.Value;

                                contentChanged = true;
                            }
                            else
                            {
                                if (ownerUpForGrabs)
                                {
                                    cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = dsVenue.Tables[0].Rows[0]["Email"].ToString();
                                }
                                else
                                {
                                    cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = DBNull.Value;
                                }
                            }

                            if (dsVenue.Tables[0].Rows[0]["Phone"].ToString() != PhoneTextBox.Text.Trim())
                            {
                                if (PhoneTextBox.Text.Trim() != "")
                                    cmd.Parameters.Add("@phone", SqlDbType.NVarChar).Value = PhoneTextBox.Text.Trim();
                                else
                                    cmd.Parameters.Add("@phone", SqlDbType.NVarChar).Value = DBNull.Value;

                                contentChanged = true;
                            }
                            else
                            {
                                if (ownerUpForGrabs)
                                {
                                    cmd.Parameters.Add("@phone", SqlDbType.NVarChar).Value = dsVenue.Tables[0].Rows[0]["Phone"].ToString();
                                }
                                else
                                {
                                    cmd.Parameters.Add("@phone", SqlDbType.NVarChar).Value = DBNull.Value;
                                }
                            }

                            if (dsVenue.Tables[0].Rows[0]["Web"].ToString() != WebSiteTextBox.Text.Trim())
                            {
                                if (WebSiteTextBox.Text.Trim() != "")
                                    cmd.Parameters.Add("@web", SqlDbType.NVarChar).Value = WebSiteTextBox.Text.Trim();
                                else
                                    cmd.Parameters.Add("@web", SqlDbType.NVarChar).Value = DBNull.Value;

                                contentChanged = true;
                            }
                            else
                            {
                                if (ownerUpForGrabs)
                                {
                                    cmd.Parameters.Add("@web", SqlDbType.NVarChar).Value =
                                        dsVenue.Tables[0].Rows[0]["Web"].ToString();
                                }
                                else
                                {
                                    cmd.Parameters.Add("@web", SqlDbType.NVarChar).Value = DBNull.Value;
                                }
                            }

                            if (dsVenue.Tables[0].Rows[0]["Address"].ToString() != locationStr)
                            {
                                cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = locationStr;
                                contentChanged = true;
                            }
                            else
                            {
                                if (ownerUpForGrabs)
                                {
                                    cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = dsVenue.Tables[0].Rows[0]["Address"].ToString();
                                }
                                else
                                {
                                    cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = DBNull.Value;
                                }
                            }

                            if (dsVenue.Tables[0].Rows[0]["Content"].ToString() != DescriptionTextBox.Content.Trim())
                            {
                                cmd.Parameters.Add("@content", SqlDbType.NVarChar).Value = DescriptionTextBox.Content;
                                contentChanged = true;
                            }
                            else
                            {
                                if (ownerUpForGrabs)
                                {
                                    cmd.Parameters.Add("@content", SqlDbType.NVarChar).Value = dsVenue.Tables[0].Rows[0]["Content"].ToString();
                                }
                                else
                                {
                                    cmd.Parameters.Add("@content", SqlDbType.NVarChar).Value = DBNull.Value;
                                }
                            }

                            if (dsVenue.Tables[0].Rows[0]["Country"].ToString() != CountryDropDown.SelectedValue)
                            {

                                cmd.Parameters.Add("@country", SqlDbType.Int).Value = int.Parse(CountryDropDown.SelectedValue);
                                contentChanged = true;
                            }
                            else
                            {
                                if (ownerUpForGrabs)
                                {
                                    cmd.Parameters.Add("@country", SqlDbType.Int).Value = dsVenue.Tables[0].Rows[0]["Country"].ToString();

                                }
                                else
                                {
                                    cmd.Parameters.Add("@country", SqlDbType.Int).Value = DBNull.Value;
                                }
                            }

                            if (dsVenue.Tables[0].Rows[0]["Zip"].ToString() != ZipTextBox.Text.Trim())
                            {

                                cmd.Parameters.Add("@zip", SqlDbType.NVarChar).Value = ZipTextBox.Text.Trim();
                                contentChanged = true;
                            }
                            else
                            {
                                if (ownerUpForGrabs)
                                {
                                    cmd.Parameters.Add("@zip", SqlDbType.NVarChar).Value = dsVenue.Tables[0].Rows[0]["Zip"].ToString();
                                }
                                else
                                {
                                    cmd.Parameters.Add("@zip", SqlDbType.NVarChar).Value = DBNull.Value;
                                }
                            }

                            if (dsVenue.Tables[0].Rows[0]["City"].ToString() != CityTextBox.Text.Trim())
                            {

                                cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = CityTextBox.Text.Trim();
                                contentChanged = true;
                            }
                            else
                            {
                                if (ownerUpForGrabs)
                                {
                                    cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = dsVenue.Tables[0].Rows[0]["City"].ToString();
                                }
                                else
                                {
                                    cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = DBNull.Value;
                                }
                            }

                            if (dsVenue.Tables[0].Rows[0]["State"].ToString() != state)
                            {
                                cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = state;
                                contentChanged = true;
                            }
                            else
                            {
                                if (ownerUpForGrabs)
                                {
                                    cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = dsVenue.Tables[0].Rows[0]["State"].ToString();
                                }
                                else
                                {
                                    cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = DBNull.Value;
                                }
                            }
                        }
                        else
                        {
                            if (OwnerPanel.Visible)
                            {
                                if (OwnerCheckBox.Checked)
                                    cmd.Parameters.Add("@owner", SqlDbType.Int).Value = Session["User"].ToString();
                                else
                                    cmd.Parameters.Add("@owner", SqlDbType.Int).Value = DBNull.Value;
                            }
                            else
                            {

                            }

                            cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = VenueNameTextBox.Text.Trim();

                            if (EmailTextBox.Text != "")
                                cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = EmailTextBox.Text.Trim();
                            else
                                cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = DBNull.Value;

                            if (PhoneTextBox.Text != "")
                                cmd.Parameters.Add("@phone", SqlDbType.NVarChar).Value = PhoneTextBox.Text.Trim();
                            else
                                cmd.Parameters.Add("@phone", SqlDbType.NVarChar).Value = DBNull.Value;

                            if (WebSiteTextBox.Text != "")
                                cmd.Parameters.Add("@web", SqlDbType.NVarChar).Value = WebSiteTextBox.Text.Trim();
                            else
                                cmd.Parameters.Add("@web", SqlDbType.NVarChar).Value = DBNull.Value;

                            cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = locationStr;
                            cmd.Parameters.Add("@user", SqlDbType.Int).Value = int.Parse(Session["User"].ToString());
                            cmd.Parameters.Add("@content", SqlDbType.NVarChar).Value = DescriptionTextBox.Content.Trim();
                            cmd.Parameters.Add("@country", SqlDbType.Int).Value = int.Parse(CountryDropDown.SelectedValue);
                            cmd.Parameters.Add("@zip", SqlDbType.NVarChar).Value = ZipTextBox.Text.Trim();
                            cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = CityTextBox.Text.Trim();


                            cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = state;


                        }
                        cmd.ExecuteNonQuery();



                        string ID = "";
                        string revisionID = "1";
                        if (isUpdate)
                        {
                            if (!isOwner)
                            {
                                ID = Request.QueryString["ID"].ToString();
                                cmd = new SqlCommand("SELECT @@IDENTITY AS IDS", conn);
                                SqlDataAdapter da2 = new SqlDataAdapter(cmd);
                                DataSet ds3 = new DataSet();
                                da2.Fill(ds3);
                                revisionID = ds3.Tables[0].Rows[0]["IDS"].ToString();
                            }
                            else
                            {
                                ID = Request.QueryString["ID"].ToString();
                            }
                        }
                        else
                        {

                            cmd = new SqlCommand("SELECT @@IDENTITY AS IDS", conn);
                            SqlDataAdapter da2 = new SqlDataAdapter(cmd);
                            DataSet ds3 = new DataSet();
                            da2.Fill(ds3);
                            ID = ds3.Tables[0].Rows[0]["IDS"].ToString();
                        }



                        bool isSlider = false;
                        if (PictureCheckList.Items.Count > 0)
                            isSlider = true;
                        if (isSlider)
                        {

                            char[] delim2 = { '\\' };
                            //string[] fileArray = System.IO.Directory.GetFiles(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\Slider\\");

                            if (!System.IO.Directory.Exists(MapPath(".") + "\\VenueFiles"))
                            {
                                System.IO.Directory.CreateDirectory(MapPath(".") + "\\VenueFiles\\");
                                System.IO.Directory.CreateDirectory(MapPath(".") + "\\VenueFiles\\" + ID + "\\");
                                System.IO.Directory.CreateDirectory(MapPath(".") + "\\VenueFiles\\" + ID + "\\Slider\\");
                            }
                            else
                            {
                                if (!System.IO.Directory.Exists(MapPath(".") + "\\VenueFiles\\" + ID))
                                {
                                    System.IO.Directory.CreateDirectory(MapPath(".") + "\\VenueFiles\\" + ID + "\\");
                                    System.IO.Directory.CreateDirectory(MapPath(".") + "\\VenueFiles\\" + ID + "\\Slider\\");
                                }
                                else
                                {
                                    if (!!System.IO.Directory.Exists(MapPath(".") + "\\VenueFiles\\" + ID + "\\Slider\\"))
                                        System.IO.Directory.CreateDirectory(MapPath(".") + "\\VenueFiles\\" + ID + "\\Slider\\");
                                }
                            }

                            string YouTubeStr = "";
                            char[] delim3 = { '.' };
                            dat.Execute("DELETE FROM Venue_Slider_Mapping WHERE VenueID=" + ID.ToString());
                            for (int i = 0; i < PictureCheckList.Items.Count; i++)
                            {
                                //int length = fileArray[i].Split(delim2).Length;
                                string[] tokens = PictureCheckList.Items[i].Value.ToString().Split(delim3);


                                if (tokens.Length >= 2)
                                {
                                    //if (PictureCheckList.Items[i].Enabled)
                                    //{
                                    if (tokens[1].ToUpper() == "JPG" || tokens[1].ToUpper() == "JPEG" || tokens[1].ToUpper() == "GIF" || tokens[1].ToUpper() == "PNG")
                                    {
                                        if (!System.IO.File.Exists(MapPath(".") + "\\VenueFiles\\" + ID.ToString() + "\\Slider\\" + PictureCheckList.Items[i].Value))
                                        {
                                            System.IO.File.Copy(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\Slider\\" +
                                                                PictureCheckList.Items[i].Value,
                                                                    MapPath(".") + "\\VenueFiles\\" + ID.ToString() + "\\Slider\\" + PictureCheckList.Items[i].Value);
                                        }





                                        cmd = new SqlCommand("INSERT INTO Venue_Slider_Mapping (VenueID, PictureName, RealPictureName) " +
                                            "VALUES (@eventID, @picName, @realName)", conn);
                                        cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = ID;
                                        cmd.Parameters.Add("@picName", SqlDbType.NVarChar).Value = PictureCheckList.Items[i].Value;
                                        cmd.Parameters.Add("@realName", SqlDbType.NVarChar).Value = PictureCheckList.Items[i].Text;
                                        cmd.ExecuteNonQuery();

                                    }
                                    else if (tokens[1].ToUpper() == "WMV")
                                    {
                                        if (!System.IO.File.Exists(MapPath(".") + "\\VenueFiles\\" + ID.ToString() + "\\Slider\\" + PictureCheckList.Items[i].Value))
                                        {
                                            System.IO.File.Copy(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\Slider\\" +
                                                                PictureCheckList.Items[i].Value,
                                                                    MapPath(".") + "\\VenueFiles\\" + ID.ToString() + "\\Slider\\" + PictureCheckList.Items[i].Value);
                                        }


                                        cmd = new SqlCommand("INSERT INTO Venue_Slider_Mapping (VenueID, PictureName) VALUES (@eventID, @picName)", conn);
                                        cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = ID;
                                        cmd.Parameters.Add("@picName", SqlDbType.NVarChar).Value = PictureCheckList.Items[i].Value;
                                        cmd.ExecuteNonQuery();


                                    }
                                    //}
                                }
                                else
                                {
                                    YouTubeStr += PictureCheckList.Items[i].Value + ";";
                                }


                            }


                            dat.Execute("UPDATE Venues SET YouTubeVideo='" + YouTubeStr + "' WHERE ID=" + ID);


                        }


                        //if (ownerUpForGrabs || isOwner)
                        CreateHoursEventsAndTerms(ID);

                        CreateCategories(ID, isOwner, isUpdate, revisionID, ownerUpForGrabs);

                        //if (CategoriesCheckBoxes.Items.Count > 0)
                        //{
                        //    int catCount = CategoriesCheckBoxes.Items.Count;

                        //    for (int i = 0; i < catCount; i++)
                        //    {
                        //        cmd = new SqlCommand("INSERT INTO Event_Category_Mapping (EventID, CategoryID) VALUES (@eventID, @catID)", conn);
                        //        cmd.Parameters.Add("@catID", SqlDbType.Int).Value = int.Parse(CategoriesCheckBoxes.Items[i].Value);
                        //        cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = ID;
                        //        cmd.ExecuteNonQuery();
                        //    }
                        //}


                        //Send the informational email to the user
                        DataSet dsUser = dat.GetData("SELECT Email, UserName FROM USERS WHERE User_ID=" +
                            Session["User"].ToString());

                        string emailBody = "<br/><br/>Dear " + dsUser.Tables[0].Rows[0]["UserName"].ToString() + ", <br/><br/> you have successfully posted the locale \"" + VenueNameTextBox.Text +
                           "\". <br/><br/> You can find this locale <a href=\"http://hippohappenings.com/" + dat.MakeNiceName(VenueNameTextBox.Text) + "_" + ID + "_Venue\">here</a>. " +
                           "<br/><br/> To rate your experience posting this locale <a href=\"http://hippohappenings.com/RateExperience.aspx?Edit=" + isUpdate.ToString() + "&Type=V&ID=" + ID + "\">please include your feedback here.</a>" +
                           "<br/><br/><br/>Have a Hippo Happening Day!<br/><br/>";

                        //MessageLiteral.Text = "<script type=\"text/javascript\">alert('" + message + "');</script>";

                        if (isUpdate && !isOwner)
                        {
                            if (!ownerUpForGrabs)
                            {
                                DataSet dsEventUser = dat.GetData("SELECT * FROM Users U WHERE User_ID=" + ownerID);
                                emailBody = "<br/><br/>A change request has been submitted for a locale you are the owner of on HippoHappenings: \"" + VenueNameTextBox.Text.Trim() +
                                    "\". <br/><br/> You can find this locale <a href=\"http://hippohappenings.com/" + dat.MakeNiceName(VenueNameTextBox.Text) + "_" + ID + "_Venue\">here</a>. " +
                                    "<br/><br/> Please log into Hippo Happenings and check your messages to view and approve these changes.</a>" +
                                    "<br/><br/><br/>Have a Hippo Happening Day!<br/><br/>";

                                //conn.Open();

                                SqlCommand cmd34 = new SqlCommand("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, To_UserID, Date, [Read], Mode, Live, SentLive) VALUES('" +
                                    "VenueID:" + Request.QueryString["ID"].ToString() + ",UserID:" + Session["User"].ToString() + ",RevisionID:" + revisionID + "',@content, " + dat.HIPPOHAPP_USERID.ToString() + ", " + ownerID + ", '" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).ToString() + "', 0, 5, 1, 1)", conn);
                                cmd34.Parameters.Add("@content", SqlDbType.NVarChar).Value = "A change request has been submitted for a locale you've created: " +
                                    VenueNameTextBox.Text;
                                cmd34.ExecuteNonQuery();
                                conn.Close();
                                if (!Request.Url.AbsoluteUri.Contains("localhost"))
                                {
                                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                                    dsEventUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "A change request has been submitted for a locale you own on HippoHappenings: " +
                                    VenueNameTextBox.Text);
                                }

                            }
                        }

                        if (isUpdate)
                        {
                            if (isOwner)
                            {
                                //dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                //    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                                //    dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "You have updated venue: " +
                                //    VenueNameTextBox.Text);
                            }
                            else
                            {
                                if (ownerUpForGrabs)
                                {
                                    emailBody = "<br/><br/>You have successfully submitted updates for locale: \"" + VenueNameTextBox.Text.Trim() +
                                    "\". <br/><br/> You can find this locale <a href=\"http://hippohappenings.com/" + dat.MakeNiceName(VenueNameTextBox.Text) + "_" + ID + "_Venue\">here</a>. " +
                                    "<br/><br/> To rate your experience posting this locale <a href=\"http://hippohappenings.com/RateExperience.aspx?Edit=" +
                                    isUpdate.ToString() + "&Type=V&ID=" + ID + "\">please include your feedback here.</a><br/><br/>" +
                                    "Have a Hippo Happening Day!<br/><br/>";
                                }
                                else
                                {
                                    emailBody = "<br/><br/>You have successfully submitted updates for locale: \"" + VenueNameTextBox.Text.Trim() +
                                    "\". <br/><br/> You can find this locale <a href=\"http://hippohappenings.com/" + dat.MakeNiceName(VenueNameTextBox.Text) + "_" + ID + "_Venue\">here</a>. " +
                                    "<br/><br/> The owner of the locale will need to approve/reject your change suggestions. If you do not hear back " +
                                    "from the locale's owner within 7 days, you will be allowed to take over ownership of this locale and automatically submit changes. That is, if no one else beats you to it! " +
                                    "If you have chosen to take over ownership, a button will be available for you on the locale's page. If you have not, you will need to edit the locale's details again." +
                                    "<br/><br/><br/>Have a Hippo Happening Day!<br/><br/>";
                                }

                                if (!Request.Url.AbsoluteUri.Contains("localhost"))
                                {
                                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                        System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                                        dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "You have submitted updates for locale: " +
                                        VenueNameTextBox.Text);
                                }
                            }
                        }
                        else
                        {
                            if (!Request.Url.AbsoluteUri.Contains("localhost"))
                            {
                                dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                                    dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "You have posted the locale: " +
                                    VenueNameTextBox.Text);
                            }
                        }


                        conn.Close();

                        //Update ownership history if neccessary
                        if (isUpdate)
                        {
                            if (OwnerPanel.Visible)
                            {
                                if (isOwner)
                                {
                                    if (!OwnerCheckBox.Checked)
                                    {
                                        string OwnerHistoryID = dat.GetData("SELECT * FROM VenueOwnerHistory WHERE VenueID=" +
                                            Request.QueryString["ID"].ToString() + " AND OwnerID=" + Session["User"].ToString() +
                                            " ORDER BY DateCreatedOwnership DESC").Tables[0].Rows[0]["ID"].ToString();

                                        dat.Execute("UPDATE VenueOwnerHistory SET DateLostOwnership='" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).ToString() +
                                            "', GaveUpOwnership='True' WHERE ID=" + OwnerHistoryID);
                                    }
                                }
                                else
                                {
                                    if (OwnerCheckBox.Checked)
                                    {
                                        dat.Execute("INSERT INTO VenueOwnerHistory (VenueID, OwnerID, DateCreatedOwnership) " +
                                            "VALUES(" + Request.QueryString["ID"].ToString() + ", " + Session["User"].ToString() +
                                            ", '" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).ToString() + "')");
                                    }

                                }
                            }
                        }
                        else
                        {
                            dat.Execute("INSERT INTO VenueOwnerHistory (VenueID, OwnerID, DateCreatedOwnership) VALUES(" + ID + ", " + Session["User"].ToString() + ", '" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).ToString() + "')");
                        }


                        try
                        {
                            bool showMessage = true;
                            //if (chargeCard)
                            //{
                            //    Encryption encrypt = new Encryption();

                            //    //Charge Card
                            //    string country = dat.GetDataDV("SELECT country_2_code FROM Countries WHERE country_id=" + BillingCountry.SelectedValue)[0]["country_2_code"].ToString();
                            //    com.paypal.sdk.util.NVPCodec status = d.DoCaptureCode(transactionID, price.ToString(),
                            //        "V" + ID + isn.ToString(), "Capture Transaction for Featuring Locale '" +
                            //        dat.MakeNiceNameFull(VenueNameTextBox.Text) + "'");
                            //    //message = status.ToString();
                            //    string successORFailure = status["ACK"];
                            //    switch (successORFailure.ToLower())
                            //    {
                            //        case "failure":
                            //            MessagePanel.Visible = true;
                            //            YourMessagesLabel.Text += status["L_LONGMESSAGE0"];
                            //            //MessagePanel.Visible = true;
                            //            //foreach (string key in status.Keys)
                            //            //{
                            //            //    YourMessagesLabel.Text += "key: '" + key + "', value: '" + status[key] + "' <br/>";
                            //            //}
                            //            break;
                            //        case "success":
                            //            //MessagePanel.Visible = true;
                            //            //foreach (string key in status.Keys)
                            //            //{
                            //            //    YourMessagesLabel.Text += "key: '" + key + "', value: '" + status[key] + "' <br/>";
                            //            //}
                            //            showMessage = true;
                            //            break;
                            //        default:
                            //            MessagePanel.Visible = true;
                            //            foreach (string key in status.Keys)
                            //            {
                            //                YourMessagesLabel.Text += "key: '" + key + "', value: '" + status[key] + "' <br/>";
                            //            }
                            //            break;
                            //    }
                            //}
                            //else
                            //{
                            //    showMessage = true;
                            //}

                            if (showMessage)
                            {
                                //pop up the message to the user
                                Encryption encrypt = new Encryption();

                                if (isOwner || !isUpdate)
                                {
                                    Session["Message"] = "Your locale has been posted successfully! An email with this info will also be sent to your account.<br/><br/>"
                                        + "Check out <a class=\"AddLink\" onclick=\"Search('" + dat.MakeNiceName(VenueNameTextBox.Text) + "_" + ID + "_Venue');\">this locale's</a> home page.<br/><br/> <a class=\"AddLink\" onclick=\"Search('RateExperience.aspx?Edit=" + isUpdate.ToString() + "&Type=V&ID=" + ID + "');\" >Rate </a>your user experience posting this locale.<br/>";
                                    //MessageLiteral.Text = "<script type=\"text/javascript\">alert('" + message + "');</script>";

                                }
                                else
                                {
                                    if (ownerUpForGrabs)
                                    {
                                        Session["Message"] = "You have successfully submitted updates for locale: \"" + VenueNameTextBox.Text.Trim() +
                                        "\".<br/><br/>Check out <a class=\"AddLink\" onclick=\"Search('" + dat.MakeNiceName(VenueNameTextBox.Text) + "_" + ID +
                                        "_Venue');\">this locale's</a> home page.<br/> <a class=\"AddLink\" onclick=\"Search('RateExperience.aspx?Edit=" +
                                        isUpdate.ToString() + "&Type=V&ID=" + ID + "');\" >Rate </a>your user experience editing this locale.<br/>";

                                    }
                                    else
                                    {
                                        Session["Message"] = "You have successfully submitted updates for this locale." +
                                                 "<br/><br/> The owner of the locale will need to <b>approve/reject</b> your change suggestions. If you do not hear back " +
                                                 "from the locale's owner within <b>7 days</b>, you will be allowed to <b>take over ownership</b> of this locale and automatically submit changes. That is, if no one else beats you to it! " +
                                                 "If you have chosen to take over ownership, a button will be available for you on the locale's page to do so. If you have not, you will need to edit the locale's details again.<br/><br/>"
                                            + "Check out <a class=\"AddLink\" onclick=\"Search('" + dat.MakeNiceName(VenueNameTextBox.Text) + "_" + ID + "_Venue');\">this locales's</a> home page.<br/><br/> <a class=\"AddLink\" onclick=\"Search('RateExperience.aspx?Edit=" + isUpdate.ToString() + "&Type=V&ID=" + ID + "');\" >Rate </a>your user experience editing this locale.<br/>";

                                        MessageRadWindow.Width = 530;
                                        MessageRadWindow.Height = int.Parse(MessageRadWindow.Height.Value.ToString()) + 20;
                                    }





                                }

                                MessageRadWindow.NavigateUrl = "Message.aspx?message=" + encrypt.encrypt(Session["Message"].ToString() + "<br/><br/><div align=\"center\">" +
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
                                MessageRadWindow.VisibleOnPageLoad = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessagePanel.Visible = true;
                            YourMessagesLabel.Text += "<br/><br/>" + ex.ToString();
                        }


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
                YourMessagesLabel.Text += validateMessage;
            }
        }
        else
        {
            MessagePanel.Visible = true;
            YourMessagesLabel.Text += "You must agree to the terms and conditions.";
        }
    }

    protected void FillHoursAndEvents(string ID)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        DataView dvHours = dat.GetDataDV("SELECT * FROM VenueHours WHERE VenueID=" + ID);
        ListItem item;
        foreach (DataRowView row in dvHours)
        {
            item = new ListItem(dat.GetHours(row["Days"].ToString()).Replace("<br/>", "") + " -- " + row["HourStart"].ToString() + 
                " -- " + row["HourEnd"].ToString(),
                row["Days"].ToString() + "---" + row["HourStart"].ToString() +
                "---" + row["HourEnd"].ToString());
            HoursListBox.Items.Add(item);
        }

        DataView dvEvents = dat.GetDataDV("SELECT * FROM VenueEvents WHERE VenueID=" + ID);
        foreach (DataRowView row in dvEvents)
        {
            item = new ListItem(row["EventName"].ToString() + " -- " + dat.GetHours(row["Days"].ToString()).Replace("<br/>", "") + 
                " -- " + row["HourStart"].ToString() + " -- " + row["HourEnd"].ToString(),
                row["EventName"].ToString() + "---" + row["Days"].ToString() + "---" + row["HourStart"].ToString() +
                "---" + row["HourEnd"].ToString());
            RegularEventsListbox.Items.Add(item);
        }
    }

    protected void CreateHoursEventsAndTerms(string ID)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        dat.Execute("DELETE FROM VenueHours WHERE VenueID=" + ID);
        string[] delim = { "---" };
        string[] tokens;
        foreach (ListItem item in HoursListBox.Items)
        {
            tokens = item.Value.Split(delim, StringSplitOptions.RemoveEmptyEntries);
            dat.Execute("INSERT INTO VenueHours (VenueID, Days, HourStart, HourEnd) VALUES(" + ID +
                ", '" + tokens[0] + "', '" + tokens[1] + "', '" + tokens[2] + "')");
        }

        dat.Execute("DELETE FROM VenueEvents WHERE VenueID=" + ID);

        foreach (ListItem item in RegularEventsListbox.Items)
        {
            tokens = item.Value.Split(delim, StringSplitOptions.RemoveEmptyEntries);
            dat.Execute("INSERT INTO VenueEvents (VenueID, EventName, Days, HourStart, HourEnd) VALUES(" + ID +
                ", '" + tokens[0].Replace("'", "''") + "', '" + tokens[1] + "', '" + tokens[2] + "', '" + tokens[3] + "')");
        }

        //if (FeaturePanel.Visible)
        //{
        //    string terms = "";
        //    foreach (ListItem item in SearchTermsListBox.Items)
        //    {
        //        terms += ";" + item.Text + ";";
        //    }
        //    foreach (ListItem item in FeatureDatesListBox.Items)
        //    {
        //        if (item.Value != "Disabled")
        //            dat.Execute("INSERT INTO VenueSearchTerms (VenueID, SearchTerms, SearchDate) VALUES(" + ID +
        //                ", '" + terms.Replace("'", "''") + "', '" + item.Text + "')");
        //    }
        //}
    }

    protected void CreateCategories(string ID, bool isOwner, bool isUpdate, string revisionID, bool ownerUpForGrabs)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        string message = "";
        try
        {
            Hashtable distinctHash = new Hashtable();
            Hashtable tagHash = new Hashtable();
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            if ((isUpdate && isOwner) || (isUpdate && ownerUpForGrabs))
            {
                dat.Execute("DELETE FROM Venue_Category WHERE VENUE_ID=" + ID);
            }

            DataSet dsCategories = dat.GetData("SELECT * FROM Venue_Category WHERE VENUE_ID=" + ID);
            DataView dvCat = new DataView(dsCategories.Tables[0], "", "", DataViewRowState.CurrentRows);



            GetCategoriesFromTree(isUpdate, isOwner,ref CategoryTree, dvCat, ID, revisionID);
            GetCategoriesFromTree(isUpdate, isOwner, ref RadTreeView2, dvCat, ID, revisionID);


        }
        catch (Exception ex)
        {
            ErrorLabel.Text = message;
        }
        //char[] delim = { ' ' };
        //string[] descriptionTokens = DescriptionTextBox.Content.Trim().Split(delim);


        ////Create categories from description text

        //for (int i = 0; i < descriptionTokens.Length; i++)
        //{
        //    if (!dat.isCommonWord(descriptionTokens[i]))
        //    {
        //        if (distinctHash.Contains(descriptionTokens[i]))
        //        {
        //            distinctHash[descriptionTokens[i]] = int.Parse(distinctHash[descriptionTokens[i]].ToString()) + 1;
        //            if (int.Parse(distinctHash[descriptionTokens[i]].ToString()) + 1 > 20)
        //                tagHash[descriptionTokens[i]] = "22";
        //            else if (int.Parse(distinctHash[descriptionTokens[i]].ToString()) + 1 > 15)
        //                tagHash[descriptionTokens[i]] = "20";
        //            else if (int.Parse(distinctHash[descriptionTokens[i]].ToString()) + 1 > 10)
        //                tagHash[descriptionTokens[i]] = "16";
        //            else if (int.Parse(distinctHash[descriptionTokens[i]].ToString()) + 1 > 6)
        //                tagHash[descriptionTokens[i]] = "14";
        //            else if (int.Parse(distinctHash[descriptionTokens[i]].ToString()) + 1 > 3)
        //                tagHash[descriptionTokens[i]] = "12";

        //            continue;
        //        }
        //        else
        //        {
        //            distinctHash.Add(descriptionTokens[i], 1);
        //        }
        //    }
        //}


        //int tagCount = tagHash.Count;

        //string[] keyArray = new string[tagCount];
        //tagHash.Keys.CopyTo(keyArray, 0);
        //bool newCat = false;
        //for (int i = 0; i < tagCount; i++)
        //{
        //    cmd = new SqlCommand("SELECT * FROM Categories WHERE CategoryName=@catName");
        //    cmd.Parameters.Add("@catName", SqlDbType.NVarChar).Value = keyArray.GetValue(i);
        //    DataSet dsCat = new DataSet();
        //    da = new SqlDataAdapter();
        //    da.Fill(dsCat);

        //    if (dsCat.Tables.Count > 0)
        //        if (dsCat.Tables[0].Rows.Count > 0)
        //        {
        //            dat.Execute("INSERT INTO Venue_Category (VENUE_ID, CATEGORY_ID, tagSize) VALUES("
        //                + dsCat.Tables[0].Rows[0]["ID"].ToString() + ", " + ID + ", " + tagHash[keyArray.GetValue(i)] + ")");
        //        }
        //        else
        //            newCat = true;
        //    else
        //        newCat = true;

        //    if (newCat)
        //    {
        //        cmd = new SqlCommand("INSERT INTO Categories (CategoryName) VALUES(@catName)", conn);
        //        cmd.Parameters.Add("@catName", SqlDbType.NVarChar).Value = keyArray.GetValue(i);
        //        cmd.ExecuteNonQuery();

        //        cmd = new SqlCommand("SELECT @@IDENTITY AS ID", conn);
        //        SqlDataAdapter da3 = new SqlDataAdapter(cmd);
        //        DataSet ds4 = new DataSet();
        //        da3.Fill(ds4);

        //        dat.Execute("INSERT INTO Event_Category_Mapping (CategoryID, EventID, tagSize) VALUES("
        //            + ds4.Tables[0].Rows[0]["ID"].ToString() + ", " + ID + ", " + tagHash[keyArray.GetValue(i)] + ")");
        //    }
        //}



     
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
    
    //protected void TabClick(object sender, EventArgs e)
    //{
    //    int nextIndex = EventTabStrip.SelectedIndex;
    //    short selectThisTab = 0;
    //    YourMessagesLabel.Text = "";
    //    MessagePanel.Visible = false;
    //    //ErrorLabel.Text = "nextindex: " + nextIndex.ToString();

    //    if (Session["VenuePrevTab"] != null)
    //    {
    //        int selectedIndex = (int)Session["VenuePrevTab"];
    //        //ErrorLabel.Text += "selectedindex: " + selectedIndex.ToString();
    //        Session["VenuePrevTab"] = nextIndex;

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

    //        if (selectedIndex.ToString() == "6")
    //        {
    //            selectThisTab = short.Parse(nextIndex.ToString());
    //        }

    //        //ErrorLabel.Text += "selectthistab: " + selectThisTab.ToString();
    //    }
    //    else
    //    {
    //        Session["VenuePrevTab"] = nextIndex;
    //        selectThisTab = short.Parse(nextIndex.ToString());
    //    }

    //    ChangeSelectedTab(0, selectThisTab);
    //}

    //protected void Onwards(object sender, EventArgs e)
    //{
    //    OnwardsIT(true, EventTabStrip.SelectedIndex);
    //}

    protected string ValidatePage()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        string validateMessage = "";

        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());

        bool goOn = true;

        bool isInternational = false;
        string locationStr = "";

        VenueNameTextBox.Text = dat.stripHTML(VenueNameTextBox.Text);
        CityTextBox.Text = dat.stripHTML(CityTextBox.Text);
        ZipTextBox.Text = dat.stripHTML(ZipTextBox.Text);
        if (CountryDropDown.SelectedValue == "223")
        {
            isInternational = true;
            string apt = "";
            if (AptNumberTextBox.Text.Trim() != "")
                apt = AptDropDown.SelectedItem.Text + " " + AptNumberTextBox.Text.Trim().ToLower();
            locationStr = StreetNumberTextBox.Text.Trim().ToLower() + ";" + StreetNameTextBox.Text.Trim().ToLower()
            + ";" + StreetDropDown.SelectedItem.Text + ";" + apt;
            goOn = VenueNameTextBox.Text.Trim() != "" && StreetNumberTextBox.Text.Trim() != ""
            && StreetNameTextBox.Text.Trim() != "" && StreetDropDown.SelectedItem.Text != "Select One..."
            && CityTextBox.Text.Trim() != ""
            && ZipTextBox.Text.Trim() != "";
        }
        else
        {
            string apt = "";
            if (AptNumberTextBox.Text.Trim() != "")
                apt = AptDropDown.SelectedItem.Text + " " + AptNumberTextBox.Text.Trim().ToLower();
            locationStr = LocationTextBox.Text.Trim().ToLower() + ";" + apt;
            goOn = VenueNameTextBox.Text.Trim() != "" && LocationTextBox.Text.Trim() != "" &&
            CityTextBox.Text.Trim() != "" && ZipTextBox.Text.Trim() != "";
        }


        if (goOn)
        {
            bool isUpdate = false;
            if (Request.QueryString["ID"] != null)
            {
                isUpdate = true;
            }

            if (EmailTextBox.Text.Trim() != "")
            {
                if (!dat.ValidateEmail(EmailTextBox.Text))
                {

                    validateMessage += "*Email address is not in the appropriate format.";

                }
            }

            if (CountryDropDown.SelectedValue == "223")
            {
                int zip = 0;
                if (int.TryParse(ZipTextBox.Text.Trim(), out zip))
                {
                    if (ZipTextBox.Text.Trim().Length == 5)
                    {

                    }
                    else
                    {
                        validateMessage += "*Zip must be a 5 digit code.";
                        goOn = false;
                    }
                }
                else
                {
                    validateMessage += "*Zip must be a 5 digit code.";
                    goOn = false;
                }
            }

            if (StateDropDown.Visible)
            {
                if (StateDropDown.SelectedIndex == -1)
                {
                    validateMessage += "*Must include state.";
                    goOn = false;
                }

            }
            else
            {
                if (StateTextBox.Text.Trim() == "")
                {
                    validateMessage += "*Must include state.";
                    goOn = false;
                }
            }


            if (!isUpdate && goOn)
            {
                string state = "";
                if (StateDropDownPanel.Visible)
                    state = StateDropDown.SelectedItem.Text;
                else
                    state = StateTextBox.Text;

                SqlCommand cmd = new SqlCommand("SELECT * FROM Venues WHERE Address=@address AND City=@city AND State=@state", conn);
                cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = locationStr;
                cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = CityTextBox.Text.Trim().ToLower();
                cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = state;
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        validateMessage += "A locale already exists under this address. " +
                            "Please take a look at it " +
                            "<a target=\"_blank\" class=\"AddGreenLink\" href=\"" + dat.MakeNiceName(VenueNameTextBox.Text) + "_" +
                            ds.Tables[0].Rows[0]["ID"].ToString() + "_Venue\">here.</a> You can submit edits to the existing locale by licking on the 'Edit Locale' link at the top of the locale page.";
                        goOn = false;
                    }
            }
        }
        else
        {
            validateMessage += "*Please fill in all fields.";
            goOn = false;
        }

        if (goOn)
        {
            DescriptionTextBox.Content = dat.StripHTML_LeaveLinks(DescriptionTextBox.Content.Replace("<div>", "<br/>").Replace("</div>", ""));

            if (CategorySelected())
            {
                validateMessage = "success";
            }
            else
            {
                validateMessage = "Must include at least one category.";
            }
        }

        return validateMessage;
    }

    //protected bool OnwardsIT(bool changeTab, int selectedIndex)
    //{
    //    HttpCookie cookie = Request.Cookies["BrowserDate"];
        
    //  Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //  SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
    //  conn.Open();
    //    string message = "";
    //    switch (selectedIndex)
    //    {
    //        case 0:
    //            #region Case 0

    //            bool goOn = false;

    //            bool isInternational = false;
    //            string locationStr = "";

    //            VenueNameTextBox.Text = dat.stripHTML(VenueNameTextBox.Text);
    //            CityTextBox.Text = dat.stripHTML(CityTextBox.Text);
    //            ZipTextBox.Text = dat.stripHTML(ZipTextBox.Text);
    //            if (CountryDropDown.SelectedValue == "223")
    //            {
    //                isInternational = true;
    //                string apt = "";
    //                if(AptNumberTextBox.Text.Trim() != "")
    //                    apt = AptDropDown.SelectedItem.Text + " " + AptNumberTextBox.Text.Trim().ToLower();
    //                locationStr = StreetNumberTextBox.Text.Trim().ToLower() + ";" + StreetNameTextBox.Text.Trim().ToLower()
    //                + ";" + StreetDropDown.SelectedItem.Text + ";" + apt;
    //                goOn = VenueNameTextBox.Text.Trim() != "" && StreetNumberTextBox.Text.Trim() != ""
    //                && StreetNameTextBox.Text.Trim() != "" && StreetDropDown.SelectedItem.Text != "Select One..."
    //                && CityTextBox.Text.Trim() != ""
    //                && ZipTextBox.Text.Trim() != "";
    //            }
    //            else
    //            {
    //                string apt = "";
    //                if (AptNumberTextBox.Text.Trim() != "")
    //                    apt = AptDropDown.SelectedItem.Text + " " + AptNumberTextBox.Text.Trim().ToLower();
    //                locationStr = LocationTextBox.Text.Trim().ToLower() + ";" + apt;
    //                goOn = VenueNameTextBox.Text.Trim() != "" && LocationTextBox.Text.Trim() != "" &&
    //                CityTextBox.Text.Trim() != "" && ZipTextBox.Text.Trim() != "";
    //            }


    //            if (goOn)
    //            {
    //                bool isUpdate = false;
    //                if (Request.QueryString["ID"] != null)
    //                {
    //                    isUpdate = true;
    //                }

    //                if (EmailTextBox.Text.Trim() != "")
    //                {
    //                    if (!dat.ValidateEmail(EmailTextBox.Text))
    //                    {
    //                        MessagePanel.Visible = true;
    //                        YourMessagesLabel.Text += "*Email address is not in the appropriate format.";
    //                        goOn = false;
    //                    }
    //                }

    //                if (CountryDropDown.SelectedValue == "223")
    //                {
    //                    int zip = 0;
    //                    if (int.TryParse(ZipTextBox.Text.Trim(), out zip))
    //                    {
    //                        if (ZipTextBox.Text.Trim().Length == 5)
    //                        {

    //                        }
    //                        else
    //                        {
    //                            MessagePanel.Visible = true;
    //                            YourMessagesLabel.Text += "*Zip must be a 5 digit code.";
    //                            goOn = false;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        MessagePanel.Visible = true;
    //                        YourMessagesLabel.Text += "*Zip must be a 5 digit code.";
    //                        goOn = false;
    //                    }
    //                }

    //                if (StateDropDown.Visible)
    //                {
    //                    if (StateDropDown.SelectedIndex == -1)
    //                    {
    //                        MessagePanel.Visible = true;
    //                        YourMessagesLabel.Text += "*Must include state.";
    //                        goOn = false;
    //                    }

    //                }
    //                else
    //                {
    //                    if (StateTextBox.Text.Trim() == "")
    //                    {
    //                        MessagePanel.Visible = true;
    //                        YourMessagesLabel.Text += "*Must include state.";
    //                        goOn = false;
    //                    }
    //                }


    //                if (!isUpdate && goOn)
    //                {
    //                    string state = "";
    //                    if (StateDropDownPanel.Visible)
    //                        state = StateDropDown.SelectedItem.Text;
    //                    else
    //                        state = StateTextBox.Text;

    //                    SqlCommand cmd = new SqlCommand("SELECT * FROM Venues WHERE Address=@address AND City=@city AND State=@state", conn);
    //                    cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = locationStr;
    //                    cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = CityTextBox.Text.Trim().ToLower();
    //                    cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = state;
    //                    DataSet ds = new DataSet();
    //                    SqlDataAdapter da = new SqlDataAdapter(cmd);
    //                    da.Fill(ds);

    //                    if (ds.Tables.Count > 0)
    //                        if (ds.Tables[0].Rows.Count > 0)
    //                        {
    //                            MessagePanel.Visible = true;
    //                            YourMessagesLabel.Text += "A locale already exists under this address. " +
    //                                "Please take a look at it " +
    //                                "<a target=\"_blank\" class=\"AddGreenLink\" href=\"" + dat.MakeNiceName(VenueNameTextBox.Text) + "_" +
    //                                ds.Tables[0].Rows[0]["ID"].ToString() + "_Venue\">here.</a> You can submit edits to the existing locale by licking on the 'Edit Locale' link at the top of the locale page.";
    //                            goOn = false;
    //                        }
    //                }





    //                if (goOn)
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
    //            }
    //            else
    //            {
    //                MessagePanel.Visible = true;
    //                YourMessagesLabel.Text += "*Please fill in all fields.";
    //            }
    //            return false;
    //            #endregion
    //            break;
    //        case 1:
    //            DescriptionTextBox.Content = dat.StripHTML_LeaveLinks(DescriptionTextBox.Content.Replace("<div>", "<br/>").Replace("</div>", ""));
    //            if (DescriptionTextBox.Text.Length >= 50)
    //            {
    //                if(changeTab)
    //                    ChangeSelectedTab(1, 2);

    //                FillLiteral();
    //                return true;
    //            }
    //            else
    //            {
    //                MessagePanel.Visible = true;
    //                YourMessagesLabel.Text += "*Make sure you say what you want your viewers to hear about this locale! The description must be at least 50 characters.";
    //                return false;
    //            }
    //            break;
    //        case 2:
    //            if(Session["CategoriesSet"] == null && Request.QueryString["ID"] != null)
    //                SetCategories();

    //            if (changeTab)
    //            {
    //                ChangeSelectedTab(2, 3);
                    
    //            }
    //            FillLiteral();
    //            return true;
    //            break;
    //        case 3:
    //            if (CategorySelected())
    //            {
    //                FillLiteral();

    //                bool isownerupforgrabs = false;
    //                EnableOwnerPanel(ref isownerupforgrabs);

    //                if(changeTab)
    //                    ChangeSelectedTab(3, 4);

    //                return true;
    //            }
    //            else
    //            {
    //                MessagePanel.Visible = true;
    //                YourMessagesLabel.Text = "Must include at least one category.";
    //                return false;
    //            }
    //            break;
    //        case 4:
    //            FillLiteral();
    //            if (changeTab)
    //            {
    //                ChangeSelectedTab(4, 5);
    //            }
    //            FindPrice();
    //            return true;
    //            break;
    //        case 5:
    //            if (FeaturePanel.Visible)
    //            {
    //                if (FeatureDatesListBox.Items.Count == 0 && FeaturePanel.Visible)
    //                {
    //                    goOn = false;
    //                    Session["Featured"] = false;
    //                }
    //                else
    //                {
    //                    if (TotalLabel.Text.Trim() != "")
    //                    {
    //                        decimal price = 0.00M;
    //                        if (decimal.TryParse(TotalLabel.Text.Trim(), out price))
    //                        {
    //                            if (price != 0.00M)
    //                            {
    //                                OwnerCheckBox.Checked = true;
    //                                OwnerCheckBox.Enabled = false;
    //                                goOn = true;
    //                                Session["Featured"] = true;
    //                            }
    //                            else
    //                            {
    //                                if (SearchTermsListBox.Items.Count > 0 && price == 0.00M)
    //                                {
    //                                    goOn = false;
    //                                    Session["Featured"] = false;
    //                                    message = "You have entered search terms, but, have not included any dates.";
    //                                }
    //                                else
    //                                {
    //                                    goOn = true;
    //                                    Session["Featured"] = false;
    //                                }
    //                            }
    //                        }
    //                        else
    //                        {
    //                            goOn = true;
    //                            Session["Featured"] = false;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        goOn = true;
    //                        Session["Featured"] = false;
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                goOn = true;
    //                Session["Featured"] = false;
    //            }
    //            if (goOn)
    //            {

    //                FillLiteral();
    //                if(changeTab)
    //                    ChangeSelectedTab(5, 6);

                    
    //                return true;
    //            }
    //            else
    //            {
    //                if (message != "")
    //                    YourMessagesLabel.Text = message;   
    //                else    
    //                    YourMessagesLabel.Text = "You have selected to feature the locale, but not entered any specifics. If you do not want to feature the locale any more, please click 'No, Thank You'.";

    //                MessagePanel.Visible = true;
    //                return true;
    //            }
    //            break;
    //        default: break;
    //    }

    //    return false;
    //}

    //protected void FillLiteral()
    //{
    //    HttpCookie cookie = Request.Cookies["BrowserDate"];

    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //    EventPanel.Visible = true;
    //    ShowEventName.Text = VenueNameTextBox.Text;

    //    //if (DescriptionTextBox.Content.Length > 500)
    //    //{
    //    ShowDescriptionBegining.Text = dat.BreakUpString(DescriptionTextBox.Content, 60);
    //    //int j = 500;
    //    //if (DescriptionTextBox.Content[500] != ' ')
    //    //{

    //    //    while (DescriptionTextBox.Content[j] != ' ')
    //    //    {
    //    //        ShowDescriptionBegining.Text += DescriptionTextBox.Content[j];
    //    //        j++;

    //    //        if (j >= DescriptionTextBox.Content.Length)
    //    //            break;
    //    //    }
    //    //}
    //    //ShowDescriptionBegining.Text = dat.BreakUpString(ShowDescriptionBegining.Text, 65);
    //    //ShowRestOfDescription.Text = dat.BreakUpString(DescriptionTextBox.Content.Substring(j), 65);
    //    //}
    //    //else
    //    //{
    //    //    ShowDescriptionBegining.Text = dat.BreakUpString(DescriptionTextBox.Content, 65);
    //    //    ShowRestOfDescription.Text = "";
    //    //}

    //    if (MainAttractionCheck.Checked)
    //    {
    //        if (PictureCheckList.Items.Count > 0)
    //        {
    //            Rotator1.Items.Clear();
    //            char[] delim = { '\\' };



    //            string[] finalFileArray = new string[PictureCheckList.Items.Count];

    //            //for (int i = 0; i < PictureCheckList.Items.Count; i++)
    //            //{
    //            //    finalFileArray[i] = "http://" + Request.Url.Authority + "/HippoHappenings/UserFiles/" + 
    //            //        Session["UserName"].ToString() + "/Slider/" + PictureCheckList.Items[i].Value;
    //            //}
    //            char[] delim2 = { '.' };
    //            bool isEdit = false;

    //            if (Request.QueryString["ID"] != null)
    //                isEdit = true;
    //            for (int i = 0; i < PictureCheckList.Items.Count; i++)
    //            {
    //                Literal literal4 = new Literal();
    //                string toUse = "";
    //                string[] tokens = PictureCheckList.Items[i].Value.ToString().Split(delim2);
    //                //if (PictureCheckList.Items[i].Enabled)
    //                //{
    //                RotatorPanel.Visible = true;
    //                if (tokens.Length >= 2)
    //                {
    //                    if (tokens[1].ToUpper() == "JPG" || tokens[1].ToUpper() == "JPEG" || tokens[1].ToUpper() == "GIF" || tokens[1].ToUpper() == "PNG")
    //                    {
    //                        System.Drawing.Image image;
    //                        if (isEdit)
    //                        {
    //                            if (System.IO.File.Exists(MapPath(".") + "\\VenueFiles\\" + Request.QueryString["ID"].ToString() + "\\Slider\\" + PictureCheckList.Items[i].Value.ToString()))
    //                            {
    //                                image = System.Drawing.Image.FromFile(MapPath(".") + "\\VenueFiles\\" + Request.QueryString["ID"].ToString() + "\\Slider\\" + PictureCheckList.Items[i].Value.ToString());
    //                                toUse = "/VenueFiles/" + Request.QueryString["ID"].ToString() + "/Slider/" + PictureCheckList.Items[i].Value.ToString();
    //                            }
    //                            else
    //                            {
    //                                image = System.Drawing.Image.FromFile(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\Slider\\" + PictureCheckList.Items[i].Value.ToString());
    //                                toUse = "/UserFiles/" + Session["UserName"].ToString() + "/Slider/" + PictureCheckList.Items[i].Value.ToString();
    //                            }
    //                        }
    //                        else
    //                        {
    //                            image = System.Drawing.Image.FromFile(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\Slider\\" + PictureCheckList.Items[i].Value.ToString());
    //                            toUse = "/UserFiles/" + Session["UserName"].ToString() + "/Slider/" + PictureCheckList.Items[i].Value.ToString();
    //                        }


    //                        int width = 410;
    //                        int height = 250;

    //                        int newHeight = image.Height;
    //                        int newIntWidth = image.Width;

    //                        literal4.Text = "<div style=\"width: 412px; height: 250px;\"><img style=\" margin-left: " + ((410 - newIntWidth) / 2).ToString() + "px; margin-top: " + ((250 - newHeight) / 2).ToString() + "px;\" height=\"" + newHeight + "px\" width=\"" + newIntWidth + "px\" src=\""
    //                            + toUse + "\" /></div>";
    //                    }
    //                    else if (tokens[1].ToUpper() == "WMV")
    //                    {
    //                        literal4.Text = "<embed  height=\"250px\" width=\"410px\" src=\""
    //                            + "UserFiles/" + Session["UserName"].ToString() + "/Slider/" + PictureCheckList.Items[i].Value.ToString() +
    //                            "\" />";
    //                    }
    //                }
    //                else
    //                {
    //                    literal4.Text = "<div style=\"float:left;\"><object width=\"410\" height=\"250\"><param  name=\"wmode\" value=\"opaque\" ></param><param name=\"movie\" value=\"http://www.youtube.com/v/" + PictureCheckList.Items[i].Value.ToString() + "\"></param><param name=\"allowFullScreen\" value=\"true\"></param><embed wmode=\"opaque\" src=\"http://www.youtube.com/v/" + PictureCheckList.Items[i].Value.ToString() + "\" type=\"application/x-shockwave-flash\" allowfullscreen=\"true\" width=\"410\" height=\"250\"></embed></object></div>";
    //                }

    //                Telerik.Web.UI.RadRotatorItem r4 = new Telerik.Web.UI.RadRotatorItem();
    //                r4.Controls.Add(literal4);
    //                Rotator1.Items.Add(r4);
    //                //}
    //            }
    //            if (Rotator1.Items.Count == 0)
    //                RotatorPanel.Visible = false;
    //            else
    //                RotatorPanel.Visible = true;

    //            if (Rotator1.Items.Count == 1)
    //                RotatorPanel.CssClass = "HiddeButtons";
    //            else
    //                RotatorPanel.CssClass = "";

    //        }

    //    }
    //}

    //protected void Backwards(object sender, EventArgs e)
    //{
    //    YourMessagesLabel.Text = "";
    //    MessagePanel.Visible = false;
    //    int selectedIndex = EventTabStrip.SelectedIndex;

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
    //        case 6:
    //            ChangeSelectedTab(6, 5);
    //            break;
    //        case 7:
    //            ChangeSelectedTab(7, 6);
    //            break;
    //        default: break;
    //    }
    //}
    
    //protected void SliderUpload_Click(object sender, EventArgs e)
    //{
    //    if (SliderFileUpload.HasFile && SliderCheckList.Items.Count < 20)
    //    {
    //        string root = MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\Slider\\";
    //        if (SliderCheckList.Items.Count == 0)
    //        {
    //            if (!System.IO.Directory.Exists(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString()))
    //            {
    //                System.IO.Directory.CreateDirectory(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString());
    //            }
    //            if (!System.IO.Directory.Exists(root))
    //            {
    //                System.IO.Directory.CreateDirectory(root);
    //            }
    //        }
    //        char[] delim = { '.' };
    //        string[] tokens = SliderFileUpload.FileName.Split(delim);
    //        string fileName = "rename" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Ticks.ToString()+"."+tokens[1];
    //        SliderFileUpload.SaveAs(root + fileName);
    //        SliderCheckList.Items.Add(new ListItem(SliderFileUpload.FileName, fileName));
    //        SliderDeleteButton.Visible = true;
    //    }
    //}
    
    protected void PictureUpload_Click(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        DateTime isNow;

        string message = "";

        if (PictureUpload.UploadedFiles.Count > 0)
        {
            UploadedVideosAndPics.Visible = true;
            foreach (Telerik.Web.UI.UploadedFile file in PictureUpload.UploadedFiles)
            {
                if (PictureCheckList.Items.Count < 20)
                {
                    isNow = DateTime.Now;
                    if (PictureCheckList.Items.Count == 0)
                        UploadedVideosAndPics.Visible = true;
                    char[] delim = { '.' };
                    string[] tokens = file.FileName.Split(delim);


                    if (tokens.Length > 1)
                    {
                        if (tokens[1].ToUpper() == "JPEG" || tokens[1].ToUpper() == "JPG" || tokens[1].ToUpper() == "GIF" || tokens[1].ToUpper() == "PNG")
                        {
                            try
                            {
                                string fileName = "rename" + isNow.Ticks.ToString() + "." + tokens[1];
                                PictureCheckList.Items.Add(new ListItem(file.FileName, fileName));
                                if (!System.IO.Directory.Exists(MapPath(".").ToString() + "/UserFiles/" +
                                    Session["UserName"].ToString() + "/Slider/"))
                                {
                                    if (!System.IO.Directory.Exists(MapPath(".").ToString() + "/UserFiles/" +
                                        Session["UserName"].ToString()))
                                    {
                                        System.IO.Directory.CreateDirectory(MapPath(".").ToString() + "/UserFiles/" +
                                            Session["UserName"].ToString());
                                    }

                                    System.IO.Directory.CreateDirectory(MapPath(".").ToString() + "/UserFiles/" +
                                        Session["UserName"].ToString() + "/Slider/");
                                }

                                System.Drawing.Image img = System.Drawing.Image.FromStream(file.InputStream);

                                message = Server.MapPath("/UserFiles/" + Session["UserName"].ToString() +
                                    "/Slider/") + fileName;

                                SaveThumbnail(img, true, Server.MapPath("/UserFiles/" + Session["UserName"].ToString() +
                                    "/Slider/") + fileName, "image/" + tokens[1].ToLower());

                                //PictureUpload.SaveAs(MapPath(".").ToString() + "/UserFiles/" + Session["UserName"].ToString() +
                                //    "/Slider/" + fileName);
                                PictureNixItButton.Visible = true;
                            }
                            catch (Exception ex)
                            {
                                ErrorLabel.Text = ex.ToString() + message;
                            }
                        }
                        else
                        {
                            YourMessagesLabel.Text = "No go! Pictures can only be .gif, .jpg, .jpeg, or .png.";
                            MessagePanel.Visible = true;
                        }
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
    
    protected void VideoUpload_Click(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (VideoUpload.HasFile)
        {
            UploadedVideosAndPics.Visible = true;
            if (PictureCheckList.Items.Count < 20)
            {
                char[] delim = { '.' };
                string[] tokens = VideoUpload.FileName.Split(delim);
                string fileName = "rename" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Ticks.ToString() + "." + tokens[1];
                PictureCheckList.Items.Add(new ListItem(VideoUpload.FileName, fileName));
                VideoUpload.SaveAs(MapPath(".").ToString() + "/UserFiles/" + Session["UserName"].ToString() + "/Slider/" + fileName);
                PictureNixItButton.Visible = true;
            }
        }

    }
    
    protected void YouTubeUpload_Click(object sender, EventArgs e)
    {

        if (YouTubeTextBox.Text.Trim() != "")
        {
            UploadedVideosAndPics.Visible = true;
            YouTubeTextBox.Text = YouTubeTextBox.Text.Trim().Replace("http://www.youtube.com/watch?v=", "");
            if (PictureCheckList.Items.Count < 20)
            {
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
    
    //protected void CheckVideoOrPicture(object sender, EventArgs e)
    //{
    //    if (VideoOrPictureCheck.Checked)
    //    {
    //        PictureVideoPanel.Visible = true;
    //    }
    //    else
    //    {
    //        PictureVideoPanel.Visible = false;
    //    }
    //}
    //protected void CheckSlider(object sender, EventArgs e)
    //{
    //    if (SliderCheck.Checked)
    //        SliderPanel.Visible = true;
    //    else
    //        SliderPanel.Visible = false;
    //}
    
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

    protected void ChangeState(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
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
            USPanel.Visible = true;
            InternationalPanel.Visible = false;
        }
        else
        {
            USPanel.Visible = false;
            InternationalPanel.Visible = true;
        }

    }
    
    protected void PictureNixIt(object sender, EventArgs e)
    {
        PictureCheckList.Items.Clear();

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

    protected void ShowSliderOrVidPic(object sender, EventArgs e)
    {
        if (MainAttracionRadioList.SelectedValue == "0")
        {
            VideoPanel.Visible = true;
            PicturePanel.Visible = false;
        }
        else
        {
            VideoPanel.Visible = false;
            PicturePanel.Visible = true;
        }
    }

    protected void VideoNixIt(object sender, EventArgs e)
    {
        //VideoCheckList.Items.Clear();

        //VideoNixItButton.Visible = false;
    }

    protected void SuggestCategoryClick(object sender, EventArgs e)
    {
        if (CategoriesTextBox.Text.Trim() != "")
        {
            HttpCookie cookie = Request.Cookies["BrowserDate"];
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            MessagePanel.Visible = true;
            CategoriesTextBox.Text = dat.stripHTML(CategoriesTextBox.Text.Trim());

            YourMessagesLabel.Text = "Your category '" + CategoriesTextBox.Text +
                "' has been suggested. We'll send you an update when it has been approved.";

            CategoriesTextBox.Text = dat.StripHTML_LeaveLinks(CategoriesTextBox.Text);


            DataSet dsUser = dat.GetData("SELECT EMAIL, UserName FROM USERS WHERE User_ID=" +
                Session["User"].ToString());

            dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["categoryemail"].ToString(),
                        System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                System.Configuration.ConfigurationManager.AppSettings["categoryemail"].ToString(),
                "Category has been suggested from 'enter-locale'. The user ID who suggested " +
                "the category is userID: '" + Session["User"].ToString() + "'. The category suggestion is '" + CategoriesTextBox.Text + "'", "Hippo Happenings category suggestion");
            CategoriesTextBox.Text = "";
        }
        else
        {
            MessagePanel.Visible = true;
            YourMessagesLabel.Text = "Please enter a category to suggest.";
        }
    }
    
    //private void ChangeSelectedTab(int unselectIndex, short selectIndex)
    //{
    //    HttpCookie cookie = Request.Cookies["BrowserDate"];
    //    DateTime isNow = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
    //    Data dat = new Data(isNow);

    //    if (CountryDropDown.SelectedValue == "223")
    //    {
    //        string majorCityID = GetMajorCity();
    //        MajorCity.Text = dat.GetDataDV("SELECT * FROM MajorCities WHERE ID=" + majorCityID)[0]["MajorCity"].ToString();
    //    }
    //    else
    //    {
    //        MajorCity.Text = CityTextBox.Text;
    //    }
    //    FillChart(new object(), new EventArgs());
    //    Session["VenuePrevTab"] = int.Parse(selectIndex.ToString());
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

    protected void AddHours(object sender, EventArgs e)
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

        string days = "";
        bool gotSelected = false;
        foreach (ListItem item2 in DaysListBox.Items)
        {
            if (item2.Selected)
            {
                gotSelected = true;
                days += item2.Value + ";";
            }
        }

        if (gotSelected && StartTimePicker.SelectedDate != null && EndTimePicker.SelectedDate != null)
        {
            //if (StartTimePicker.SelectedDate.Value < EndTimePicker.SelectedDate.Value)
            //{
                string starTime = StartTimePicker.SelectedDate.Value.TimeOfDay.Hours + ":" + StartTimePicker.SelectedDate.Value.TimeOfDay.Minutes.ToString();
                if (starTime.Substring(starTime.Length - 2, 2) == ":0")
                    starTime = starTime.Replace(":0", ":00");

                string endTime = EndTimePicker.SelectedDate.Value.TimeOfDay.Hours + ":" + EndTimePicker.SelectedDate.Value.TimeOfDay.Minutes.ToString();
                if (endTime.Substring(endTime.Length - 2, 2) == ":0")
                    endTime = endTime.Replace(":0", ":00");

                ListItem item = new ListItem(dat.GetHours(days).Replace("<br/>", "") + " -- " +
                    starTime + " -- " + endTime, days + "---" + starTime + "---" + endTime);
                if (!HoursListBox.Items.Contains(item))
                {
                    HoursListBox.Items.Add(item);
                }
            //}
            //else
            //{
            //    HoursErrorLabel.Text = "The start time must be less than the end time.";
            //}
        }
        else
        {
            HoursErrorLabel.Text = "Make sure you have chosen days, start time and end time.";
        }
    }

    protected void RemoveHours(object sender, EventArgs e)
    {
        if (HoursListBox.SelectedItem != null)
        {
            HoursListBox.Items.Remove(HoursListBox.SelectedItem);
        }
    }

    protected void AddEvent(object sender, EventArgs e)
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

        string days = "";
        bool gotSelected = false;
        foreach (ListItem item2 in RegularDaysListBox.Items)
        {
            if (item2.Selected)
            {
                gotSelected = true;
                days += item2.Value + ";";
            }
        }

        RegularEventNameTextBox.Text = dat.stripHTML(RegularEventNameTextBox.Text.Trim());

        if (gotSelected && RadTimePicker1.SelectedDate != null && RadTimePicker2.SelectedDate != null)
        {
            //if (RadTimePicker1.SelectedDate.Value < RadTimePicker2.SelectedDate.Value)
            //{
                string starTime = RadTimePicker1.SelectedDate.Value.TimeOfDay.Hours + ":" + RadTimePicker1.SelectedDate.Value.TimeOfDay.Minutes.ToString();
                if (starTime.Substring(starTime.Length - 2, 2) == ":0")
                    starTime = starTime.Replace(":0", ":00");

                string endTime = RadTimePicker2.SelectedDate.Value.TimeOfDay.Hours + ":" + RadTimePicker2.SelectedDate.Value.TimeOfDay.Minutes.ToString();
                if (endTime.Substring(endTime.Length - 2, 2) == ":0")
                    endTime = endTime.Replace(":0", ":00");

                ListItem item = new ListItem(RegularEventNameTextBox.Text + " -- " +
                    dat.GetHours(days).Replace("<br/>", "") + " -- " + starTime + " -- " + endTime,
                     RegularEventNameTextBox.Text + "---" + days + "---" + starTime + "---" + endTime);
                if (!RegularEventsListbox.Items.Contains(item))
                    RegularEventsListbox.Items.Add(item);
            //}
            //else
            //{
            //    EventsErrorLabel.Text = "The start time must be less than the end time.";
            //}
        }
        else
        {
            EventsErrorLabel.Text = "Make sure you have chosen days, start time and end time.";
        }
    }

    protected void RemoveEvent(object sender, EventArgs e)
    {
        if (RegularEventsListbox.SelectedItem != null)
        {
            RegularEventsListbox.Items.Remove(RegularEventsListbox.SelectedItem);
        }
    }

    //protected void CheckedFeatured(object sender, EventArgs e)
    //{
    //    FeaturePanel.Visible = true;
    //}

    //protected void UnCheckedFeatured(object sender, EventArgs e)
    //{
    //    Session["Featured"] = false;
    //    FeaturePanel.Visible = false;
    //    ClearFeatured();
    //    FillChart(new object(), new EventArgs());
    //    OnwardsIT(true, EventTabStrip.SelectedIndex);
    //}

    //protected void ClearFeatured()
    //{
    //    ListItemCollection col = new ListItemCollection();

    //    foreach (ListItem item in FeatureDatesListBox.Items)
    //    {
    //        col.Add(item);
    //    }

    //    int a = col.Count;
    //    for (int i = 0; i < a; i++)
    //    {
    //        if (col[i].Value != "Disabled")
    //        {
    //            FeatureDatesListBox.Items.Remove(col[i]);
    //        }
    //    }

    //    col = new ListItemCollection();

    //    foreach (ListItem item in SearchTermsListBox.Items)
    //    {
    //        col.Add(item);
    //    }

    //    a = col.Count;
    //    for (int i = 0; i < a; i++)
    //    {
    //        SearchTermsListBox.Items.Remove(col[i]);
    //    }
    //}

    //protected void AddFeaturedDates(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        bool lassThan7Enabled = true;
    //        int count = 0;
    //        foreach (ListItem item2 in FeatureDatesListBox.Items)
    //        {
    //            if (item2.Value != "Disabled")
    //            {
    //                count++;
    //                if (count == 7)
    //                {
    //                    lassThan7Enabled = false;
    //                    break;
    //                }
    //            }
    //        }
    //        string troubleTerms = "";
    //        if (FeaturedDatePicker.SelectedDate != null && lassThan7Enabled)
    //        {
    //            if (!FeatureDatesListBox.Items.Contains(new ListItem(FeaturedDatePicker.SelectedDate.Value.ToShortDateString()))
    //                && !FeatureDatesListBox.Items.Contains(new ListItem(FeaturedDatePicker.SelectedDate.Value.ToShortDateString(), "Disabled")))
    //            {
    //                if (!CheckFor4Bulletins(false, FeaturedDatePicker.SelectedDate.Value.ToShortDateString(), ref troubleTerms))
    //                {
    //                    ListItem item = new ListItem(FeaturedDatePicker.SelectedDate.Value.ToShortDateString());
    //                    FeatureDatesListBox.Items.Add(item);
    //                    FillChart(new object(), new EventArgs());
    //                }
    //                else
    //                {
    //                    TermsErrorLabel.Text = "There are already too many featured locales " +
    //                    "with the search term and date combination you are trying to add. " +
    //                    "The combination in question is: " + troubleTerms;
    //                }
    //            }
    //            else
    //            {
    //                TermsErrorLabel.Text = "Your locale is already featured for this date.";
    //            }
    //        }
    //        else
    //        {
    //            if (!lassThan7Enabled)
    //                TermsErrorLabel.Text = "You can purchase 7 feature days at a time. You can always purchase more days by visiting the page again.";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ErrorLabel.Text = ex.ToString();
    //    }
    //}

    //protected void RemoveFeaturedDates(object sender, EventArgs e)
    //{
    //    if (FeatureDatesListBox.SelectedItem != null)
    //    {

    //        if (FeatureDatesListBox.SelectedItem.Value != "Disabled")
    //        {
    //            FeatureDatesListBox.Items.Remove(FeatureDatesListBox.SelectedItem);
    //            FillChart(new object(), new EventArgs());
    //        }
    //    }
    //}

    //protected void AddSearchTerm(object sender, EventArgs e)
    //{
    //    TermsErrorLabel.Text = "";
    //    HttpCookie cookie = Request.Cookies["BrowserDate"];
    //    if (cookie == null)
    //    {
    //        cookie = new HttpCookie("BrowserDate");
    //        cookie.Value = DateTime.Now.ToString();
    //        cookie.Expires = DateTime.Now.AddDays(22);
    //        Response.Cookies.Add(cookie);
    //    }
    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

    //    TermsBox.Text = dat.stripHTML(TermsBox.Text);
    //    string troubleTerms = "";
    //    if (!TermsBox.Text.Contains(" "))
    //    {

    //        if (TermsBox.Text.Trim() != "")
    //        {
    //            if (!SearchTermsListBox.Items.Contains(new ListItem(TermsBox.Text.Trim())))
    //            {
    //                if (!CheckFor4Bulletins(true, TermsBox.Text.Trim(), ref troubleTerms))
    //                {
    //                    SearchTermsListBox.Items.Add(new ListItem(TermsBox.Text.Trim()));
    //                    FillChart(new object(), new EventArgs());
    //                }
    //                else
    //                {
    //                    TermsErrorLabel.Text = "There are already too many featured locales " +
    //                        "with the search term and date combination you are trying to add. " +
    //                        "The combination in question is: " + troubleTerms;
    //                }
    //            }
    //            else
    //            {
    //                TermsErrorLabel.Text = "You can only add the same search term once.";
    //            }
    //        }
    //        else
    //        {
    //            TermsErrorLabel.Text = "Please include a search term.";
    //        }
    //    }
    //    else
    //    {
    //        TermsErrorLabel.Text = "Search terms cannot contain spaces.";
    //    }
    //}

    //protected void RemoveSearchTerm(object sender, EventArgs e)
    //{
    //    if (SearchTermsListBox.SelectedItem != null)
    //    {
    //        SearchTermsListBox.Items.Remove(SearchTermsListBox.SelectedItem);
    //        FillChart(new object(), new EventArgs());
    //    }
    //}

    //protected bool CheckFor4Bulletins(bool isTerm, string TermOrDateString, ref string troubleDateOrTerm)
    //{
    //    HttpCookie cookie = Request.Cookies["BrowserDate"];
    //    DateTime isNow = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
    //    Data dat = new Data(isNow);

    //    DataView dv;
    //    bool termExists = false;
    //    foreach (ListItem itemDate in FeatureDatesListBox.Items)
    //    {
    //        //Check if the current term we're trying to bring in would exceed 4 with any date
    //        if (isTerm)
    //        {
    //            dv = dat.GetDataDV("SELECT * FROM VenueSearchTerms WHERE SearchDate = '" + itemDate.Text +
    //                "' AND SearchTerms LIKE '%;" + TermOrDateString + ";%'");

    //            if (dv.Count > 3)
    //            {
    //                troubleDateOrTerm = " search term '" + TermOrDateString + "' and date '" + itemDate.Text + "' ";
    //                termExists = true;
    //                break;
    //            }
    //        }

    //        foreach (ListItem itemTerm in SearchTermsListBox.Items)
    //        {
    //            dv = dat.GetDataDV("SELECT * FROM VenueSearchTerms WHERE SearchDate = '" + itemDate.Text +
    //                "' AND SearchTerms LIKE '%;" + itemTerm.Text + ";%'");

    //            if (dv.Count > 3)
    //            {
    //                troubleDateOrTerm = " search term '" + itemTerm.Text + "' and date '" + itemDate.Text + "' ";
    //                termExists = true;
    //                break;
    //            }

    //            //Check if the current date we're trying to bring in would exceed 4 with any term
    //            if (!isTerm)
    //            {
    //                dv = dat.GetDataDV("SELECT * FROM VenueSearchTerms WHERE SearchDate = '" + TermOrDateString +
    //                "' AND SearchTerms LIKE '%;" + itemTerm.Text + ";%'");

    //                if (dv.Count > 4)
    //                {
    //                    troubleDateOrTerm = " search term '" + itemTerm.Text + "' and date '" + TermOrDateString + "' ";
    //                    termExists = true;
    //                    break;
    //                }
    //            }
    //        }

    //        if (termExists)
    //            break;
    //    }

    //    return termExists;
    //}

    //protected void FillChart(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        PricingLiteral.Text = "<td align='center'>0</td><td align='center'>$0</td><td align='center'>$0</td><td align='center'>$0</td><td align='center'>$0</td>";

    //        bool hasNewFeaturedItems = false;

    //        foreach (ListItem item in FeatureDatesListBox.Items)
    //        {
    //            if (item.Value != "Disabled")
    //            {
    //                hasNewFeaturedItems = true;
    //                break;
    //            }
    //        }
    //        if (hasNewFeaturedItems)
    //        {

    //            HttpCookie cookie = Request.Cookies["BrowserDate"];
    //            DateTime isNow = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
    //            Data dat = new Data(isNow);

    //            DataView dvChart = dat.GetDataDV("SELECT * FROM StandardVenuePricing");

    //            int daysCount = 0;
    //            foreach (ListItem item in FeatureDatesListBox.Items)
    //            {
    //                if (item.Value != "Disabled")
    //                {
    //                    daysCount++;
    //                }
    //            }

    //            string state = "";
    //            if (StateDropDownPanel.Visible)
    //                state = StateDropDown.SelectedItem.Text;
    //            else
    //                state = StateTextBox.Text.Trim();

    //            string zips = "";
    //            string majorCityID = GetMajorCity();
    //            DataView dvCountMembers;
    //            if (CountryDropDown.SelectedValue == "223")
    //            {
    //                //Get Major City
    //                zips = dat.GetDataDV("SELECT AllZips FROM MajorCities WHERE ID=" + majorCityID)[0]["AllZips"].ToString();

    //                dvCountMembers = dat.GetDataDV("SELECT COUNT(*) AS Count FROM UserPreferences WHERE MajorCity = " +
    //                    majorCityID);

    //            }
    //            else
    //            {
    //                dvCountMembers = dat.GetDataDV("SELECT COUNT(*) AS Count FROM UserPreferences WHERE CatCity = '" +
    //                    CityTextBox.Text + "' AND CatCountry = " + CountryDropDown.SelectedValue +
    //                    " AND CatState = '" + state + "'");

    //            }

    //            int indexToLookAt = daysCount - 1;

    //            if (daysCount > 3)
    //                indexToLookAt = 3;

    //            decimal memberCap = decimal.Round(decimal.Parse(dvChart[indexToLookAt]["PerMemberCap"].ToString()), 3, MidpointRounding.AwayFromZero);
    //            decimal priceForMembers = 0.00M;

    //            if (decimal.Parse(dvChart[indexToLookAt]["PerMemberPricing"].ToString()) * decimal.Parse(dvCountMembers[0]["Count"].ToString()) > memberCap)
    //            {
    //                priceForMembers = memberCap;
    //            }
    //            else
    //            {
    //                priceForMembers = decimal.Round(decimal.Parse(dvChart[indexToLookAt]["PerMemberPricing"].ToString()) *
    //                    decimal.Parse(dvCountMembers[0]["Count"].ToString()), 3, MidpointRounding.AwayFromZero);
    //            }

    //            DataView dvCountEvents;
    //            DateTime dateDate;
    //            decimal eventCap = decimal.Round(decimal.Parse(dvChart[indexToLookAt]["PerVenueCap"].ToString()), 3, MidpointRounding.AwayFromZero);
    //            decimal subtractionForEvents = 0.00M;
    //            decimal total = 0.00M;

    //            PricingLiteral.Text = "";
    //            zips = zips.Replace("CatZip", "Zip");
    //            foreach (ListItem item in FeatureDatesListBox.Items)
    //            {
    //                if (item.Value != "Disabled")
    //                {
    //                    dateDate = DateTime.Parse(item.Text);

    //                    if (CountryDropDown.SelectedValue == "223")
    //                    {
    //                        dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Venues WHERE Featured='True' AND (" + zips +
    //                                    ") AND DaysFeatured LIKE '%;" + dateDate.Month.ToString() + "/" +
    //                    dateDate.Day.ToString() + "/" + dateDate.Year.ToString() + ";%'");

    //                    }
    //                    else
    //                    {
    //                        dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Venues WHERE Featured='True' AND City = '" +
    //                    CityTextBox.Text + "' AND Country = " + CountryDropDown.SelectedValue +
    //                    " AND State = '" + state + "' AND DaysFeatured LIKE '%;" + dateDate.Month.ToString() + "/" +
    //                    dateDate.Day.ToString() + "/" + dateDate.Year.ToString() + ";%'");
    //                    }


    //                    if (decimal.Parse(dvChart[indexToLookAt]["PerVenuePricing"].ToString()) *
    //                        decimal.Parse(dvCountEvents[0]["Count"].ToString()) > eventCap)
    //                    {
    //                        subtractionForEvents = eventCap;
    //                    }
    //                    else
    //                    {
    //                        subtractionForEvents = decimal.Round(decimal.Parse(dvChart[indexToLookAt]["PerVenuePricing"].ToString()) *
    //                            decimal.Parse(dvCountEvents[0]["Count"].ToString()), 3, MidpointRounding.AwayFromZero);
    //                    }

    //                    PricingLiteral.Text += "<tr><td align=\"center\" >" + item.Text + "</td><td align=\"center\">" +
    //                        decimal.Round(decimal.Parse(dvChart[indexToLookAt]["StandardVenuePricing"].ToString()), 3, MidpointRounding.AwayFromZero).ToString() +
    //                        "</td><td align=\"center\">$" + priceForMembers.ToString() + "</td><td align=\"center\">-$" + subtractionForEvents.ToString() + "</td>" +
    //                        "<td align=\"center\">$" + decimal.Round((decimal.Parse(dvChart[indexToLookAt]["StandardVenuePricing"].ToString()) +
    //                        priceForMembers - subtractionForEvents), 3, MidpointRounding.AwayFromZero).ToString() + "</td></tr>";
    //                    total += decimal.Round((decimal.Parse(dvChart[indexToLookAt]["StandardVenuePricing"].ToString()) +
    //                        priceForMembers - subtractionForEvents), 3, MidpointRounding.AwayFromZero);
    //                }
    //            }

    //            decimal searchTermTotal = 0.05M * SearchTermsListBox.Items.Count * daysCount;
    //            NumSearchTerms.Text = searchTermTotal.ToString();
    //            TotalLabel.Text = decimal.Round((total + searchTermTotal), 2, MidpointRounding.AwayFromZero).ToString();

    //            if (decimal.Round((total + searchTermTotal), 2, MidpointRounding.AwayFromZero) > 0.00M)
    //                PaymentPanel.Visible = true;
    //            else
    //                PaymentPanel.Visible = false;
    //        }
    //        else
    //        {
    //            PaymentPanel.Visible = false;
    //            TotalLabel.Text = "0.00";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        FeatureErrorLabel.Text += ex.ToString();
    //    }
    //}

    //protected string GetMajorCity()
    //{
    //    HttpCookie cookie = Request.Cookies["BrowserDate"];
    //    DateTime isNow = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":").Replace("%28", "(").Replace("%29", ")"));
    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":").Replace("%28", "(").Replace("%29", ")")));


    //    string zip = ZipTextBox.Text;

    //    int zipParam = 0;

    //    string zips = "";
    //    string MajorCityID = "";
    //    if (CountryDropDown.SelectedValue == "223")
    //    {
    //        DataView dvAllZips = dat.GetDataDV("SELECT * FROM MajorZips MZ, MajorCities MC WHERE " +
    //            "MZ.MajorCityID=MC.ID AND MajorCityZip='" + zip + "'");

    //        //If user's zip falls under a major city, just get all the zips that apply to that city
    //        if (dvAllZips.Count > 0)
    //        {
    //            MajorCityID = dvAllZips[0]["MajorCityID"].ToString();
    //        }
    //        //If zip doesn't fall under a major city, get the closest major city
    //        //Zip array will include all zips from that major city and all zips 
    //        //the same distance away as the current zip.
    //        else
    //        {
    //            DataView dvLatsLongs = dat.GetDataDV("SELECT Latitude, Longitude FROM ZipCodes WHERE Zip='" +
    //                    zip + "'");

    //            //If not found, find closest Latitude and Longitude
    //            zipParam = int.Parse(zip);
    //            if (dvLatsLongs.Count == 0)
    //            {
    //                dvLatsLongs = null;
    //                while (dvLatsLongs == null)
    //                {
    //                    dvLatsLongs = dat.GetDataDV("SELECT Latitude, Longitude FROM ZipCodes WHERE Zip='" + zipParam++ + "'");
    //                    if (dvLatsLongs.Count > 0)
    //                    {

    //                    }
    //                    else
    //                    {
    //                        dvLatsLongs = null;
    //                    }
    //                }
    //            }

    //            DataView dvMajors = dat.GetDataDV("SELECT dbo.GetDistance(" + dvLatsLongs[0]["Longitude"].ToString() +
    //                ", " + dvLatsLongs[0]["Latitude"].ToString() + ", ZC.Longitude, ZC.Latitude) AS " +
    //                "Distance, ZC.Zip, MC.MajorCity, MC.State, MZ.MajorCityID, ZC.Longitude, ZC.Latitude FROM MajorZips MZ, ZipCodes ZC, MajorCities MC " +
    //                "WHERE MZ.MajorCityZip=ZC.Zip AND MZ.MajorCityID=MC.ID ORDER BY Distance ASC");

    //            //All zips in the closest major city
    //            MajorCityID = dvMajors[0]["MajorCityID"].ToString();
    //        }
    //    }
    //    else
    //    {
    //        MajorCityID = "";
    //    }
    //    return MajorCityID;
    //}

    //protected void ChangeStateAction(object sender, EventArgs e)
    //{
    //    HttpCookie cookie = Request.Cookies["BrowserDate"];

    //    DateTime isn = DateTime.Now;

    //    if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
    //        isn = DateTime.Now;
    //    DateTime isNow = isn;
    //    Data dat = new Data(isn);
    //    DataSet ds = dat.GetData("SELECT * FROM State WHERE country_id=" + BillingCountry.SelectedValue);

    //    if (BillingCountry.SelectedValue == "223")
    //    {
    //        CardTypeDropDown.Items.Clear();
    //        CardTypeDropDown.Items.Add(new ListItem("Visa", "Visa"));
    //        CardTypeDropDown.Items.Add(new ListItem("MasterCard", "MasterCard"));
    //        CardTypeDropDown.Items.Add(new ListItem("Discover", "Discover"));
    //        CardTypeDropDown.Items.Add(new ListItem("American Express", "Amex"));
    //    }
    //    else if (BillingCountry.SelectedValue == "222")
    //    {
    //        CardTypeDropDown.Items.Clear();
    //        CardTypeDropDown.Items.Add(new ListItem("Visa, including Visa Electron and Visa Debit", "Visa"));
    //        CardTypeDropDown.Items.Add(new ListItem("MasterCard", "MasterCard"));
    //        CardTypeDropDown.Items.Add(new ListItem("Maestro, including Switch", "Maestro"));
    //        CardTypeDropDown.Items.Add(new ListItem("Solo", "Solo"));
    //    }
    //    else
    //    {
    //        CardTypeDropDown.Items.Clear();
    //        CardTypeDropDown.Items.Add(new ListItem("Visa", "Visa"));
    //        CardTypeDropDown.Items.Add(new ListItem("MasterCard", "MasterCard"));
    //    }

    //    bool isTextBox = false;
    //    if (ds.Tables.Count > 0)
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            BillingStateDropDown.Visible = true;
    //            BillingStateTextBox.Visible = false;
    //            BillingStateDropDown.DataSource = ds;
    //            BillingStateDropDown.DataTextField = "state_2_code";
    //            BillingStateDropDown.DataValueField = "state_id";
    //            BillingStateDropDown.DataBind();
    //        }
    //        else
    //            isTextBox = true;
    //    else
    //        isTextBox = true;

    //    if (isTextBox)
    //    {
    //        BillingStateTextBox.Visible = true;
    //        BillingStateDropDown.Visible = false;
    //    }
    //}

    //protected void UpdateState(object sender, EventArgs e)
    //{
    //    BillingStateTextBox.Text = BillingStateDropDown.SelectedItem.Text;
    //    FetUpdatePanel.Update();
    //}

    //protected void FindPrice()
    //{
    //    try
    //    {

    //        HttpCookie cookie = Request.Cookies["BrowserDate"];
    //        DateTime isNow = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
    //        Data dat = new Data(isNow);

    //        DataView dvChart = dat.GetDataDV("SELECT * FROM StandardVenuePricing");

    //        int daysCount = 0;

    //        string state = "";
    //        if (StateDropDownPanel.Visible)
    //            state = StateDropDown.SelectedItem.Text;
    //        else
    //            state = StateTextBox.Text.Trim();

    //        string zips = "";
    //        string majorCityID = GetMajorCity();
    //        DataView dvCountMembers;
    //        if (CountryDropDown.SelectedValue == "223")
    //        {
    //            //Get Major City
    //            zips = dat.GetDataDV("SELECT AllZips FROM MajorCities WHERE ID=" + majorCityID)[0]["AllZips"].ToString();

    //            dvCountMembers = dat.GetDataDV("SELECT COUNT(*) AS Count FROM UserPreferences WHERE MajorCity = " +
    //                majorCityID);

    //        }
    //        else
    //        {
    //            dvCountMembers = dat.GetDataDV("SELECT COUNT(*) AS Count FROM UserPreferences WHERE CatCity = '" +
    //                CityTextBox.Text + "' AND CatCountry = " + CountryDropDown.SelectedValue +
    //                " AND CatState = '" + state + "'");

    //        }

    //        int indexToLookAt = 0;

    //        decimal memberCap = decimal.Round(decimal.Parse(dvChart[indexToLookAt]["PerMemberCap"].ToString()), 3, MidpointRounding.AwayFromZero);
    //        decimal priceForMembers = 0.00M;

    //        if (decimal.Parse(dvChart[indexToLookAt]["PerMemberPricing"].ToString()) * decimal.Parse(dvCountMembers[0]["Count"].ToString()) > memberCap)
    //        {
    //            priceForMembers = memberCap;
    //        }
    //        else
    //        {
    //            priceForMembers = decimal.Round(decimal.Parse(dvChart[indexToLookAt]["PerMemberPricing"].ToString()) *
    //                decimal.Parse(dvCountMembers[0]["Count"].ToString()), 3, MidpointRounding.AwayFromZero);
    //        }

    //        DataView dvCountEvents;
    //        DateTime dateDate;
    //        decimal eventCap = decimal.Round(decimal.Parse(dvChart[indexToLookAt]["PerVenueCap"].ToString()), 3, MidpointRounding.AwayFromZero);
    //        decimal subtractionForEvents = 0.00M;
    //        decimal total = 0.00M;

    //        zips = zips.Replace("CatZip", "Zip");

    //        dateDate = DateTime.Now;

    //        if (CountryDropDown.SelectedValue == "223")
    //        {
    //            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Venues WHERE Featured='True' AND (" + zips +
    //                        ") AND DaysFeatured LIKE '%;" + dateDate.Month.ToString() + "/" +
    //        dateDate.Day.ToString() + "/" + dateDate.Year.ToString() + ";%'");

    //        }
    //        else
    //        {
    //            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Venues WHERE Featured='True' AND City = '" +
    //        CityTextBox.Text + "' AND Country = " + CountryDropDown.SelectedValue +
    //        " AND State = '" + state + "' AND DaysFeatured LIKE '%;" + dateDate.Month.ToString() + "/" +
    //        dateDate.Day.ToString() + "/" + dateDate.Year.ToString() + ";%'");
    //        }


    //        if (decimal.Parse(dvChart[indexToLookAt]["PerVenuePricing"].ToString()) *
    //            decimal.Parse(dvCountEvents[0]["Count"].ToString()) > eventCap)
    //        {
    //            subtractionForEvents = eventCap;
    //        }
    //        else
    //        {
    //            subtractionForEvents = decimal.Round(decimal.Parse(dvChart[indexToLookAt]["PerVenuePricing"].ToString()) *
    //                decimal.Parse(dvCountEvents[0]["Count"].ToString()), 3, MidpointRounding.AwayFromZero);
    //        }

    //        total += decimal.Round((decimal.Parse(dvChart[indexToLookAt]["StandardVenuePricing"].ToString()) +
    //            priceForMembers - subtractionForEvents), 3, MidpointRounding.AwayFromZero);

    //        string allTotal = decimal.Round(total, 2, MidpointRounding.AwayFromZero).ToString();

    //        PriceLiteral.Text = allTotal;
    //    }
    //    catch (Exception ex)
    //    {
    //        FeatureErrorLabel.Text += ex.ToString();
    //    }
    //}
}
