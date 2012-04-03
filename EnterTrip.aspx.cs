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

public partial class EnterTrip : Telerik.Web.UI.RadAjaxPage
{
    private string UserName;
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

        MessagePanel.Visible = false;
        YourMessagesLabel.Text = "";

        HtmlLink lk = new HtmlLink();
        HtmlHead head = (HtmlHead)Page.Header;
        lk.Attributes.Add("rel", "canonical");
        lk.Href = "https://hippohappenings.com/enter-trip";
        head.Controls.AddAt(0, lk);

        HtmlMeta hm = new HtmlMeta();
        HtmlMeta kw = new HtmlMeta();

        kw.Name = "keywords";
        kw.Content = "Post local trips sights and adventures happening in your area.";

        head.Controls.AddAt(0, kw);

        hm.Name = "Description";
        hm.Content = "Post local trips sights and adventures happening in your area.";
        head.Controls.AddAt(0, hm);

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
        Data dat = new Data(isn); try
        {
            MessageRadWindowManager.VisibleOnPageLoad = false;
        }
        catch (Exception ex)
        {
            YourMessagesLabel.Text = ex.ToString();
            MessagePanel.Visible = true;
        }
        DataView dv = dat.GetDataDV("SELECT * FROM TermsAndConditions");
        Literal lit1 = new Literal();
        lit1.Text = dv[0]["Content"].ToString();
        TACTextBox.Controls.Add(lit1);

        DirectionsErrorLabel.Text = "";
        MonthsErrorLabel.Text = "";
        TimeErrorLabel.Text = "";
        //TermsErrorLabel.Text = "";

        #region Take Care of Buttons
        //DetailsOnwardsButton.SERVER_CLICK += Onwards;
        //ImageButton9.SERVER_CLICK += Backwards;
        //CategoryOnwardButton.SERVER_CLICK += Onwards;
        //ImageButton12.SERVER_CLICK += Backwards;
        PostItButton.SERVER_CLICK += PostIt;
        //ImageButton2.SERVER_CLICK += Backwards;
        //DescriptionOnwardsButton.SERVER_CLICK += Onwards;
        ImageButton7.SERVER_CLICK += YouTubeUpload_Click;
        PictureNixItButton.SERVER_CLICK += SliderNixIt;
        //ImageButton3.SERVER_CLICK += Backwards;
        //MediaOnwardsButton.SERVER_CLICK += Onwards;
        ImageButton11.SERVER_CLICK += SuggestCategoryClick;
        //BlueButton9.SERVER_CLICK += CheckedFeatured;
        //BlueButton10.SERVER_CLICK += UnCheckedFeatured;
        //BlueButton1.SERVER_CLICK += AddFeaturedDates;
        //BlueButton4.SERVER_CLICK += RemoveFeaturedDates;
        //BlueButton5.SERVER_CLICK += AddSearchTerm;
        //BlueButton6.SERVER_CLICK += RemoveSearchTerm;
        //BlueButton7.SERVER_CLICK += FillChart;
        //BlueButton2.SERVER_CLICK += Onwards;
        //BlueButton3.SERVER_CLICK += Backwards;
        BlueButton11.SERVER_CLICK += AddDestination;
        BlueButton12.SERVER_CLICK += RemoveDestination;
        BlueButton13.SERVER_CLICK += EditDestination;
        BlueButton14.SERVER_CLICK += ModifyDestination;
        BlueButton15.SERVER_CLICK += AddTime;
        BlueButton16.SERVER_CLICK += RemoveTime;
        BlueButton17.SERVER_CLICK += AddMonth;
        BlueButton18.SERVER_CLICK += RemoveMonth;
        BlueButton19.SERVER_CLICK += AddBring;
        BlueButton20.SERVER_CLICK += RemoveBring;
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

        if (!IsPostBack)
        {
            CountryDropDown.DataBind();
            CountryDropDown.ClearSelection();
            CountryDropDown.Items.FindByValue("223").Selected = true;
            ChangeState(new object(), new EventArgs());

            ////Take Care of Billing Locaion
            //DataSet dsCountries = dat.GetData("SELECT * FROM Countries");
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

            //Session.Remove("Featured");
            //Session["Featured"] = null;

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
                        }
                    }

                    UserName = Session["UserName"].ToString();
                    UserNameLabel.Text = UserName;
                }
                else
                {
                    Session.Abandon();
                    FormsAuthentication.SignOut();
                    Response.Redirect("~/enter-trip-intro");
                }
            }
            catch (Exception ex)
            {

                MessagePanel.Visible = true;
                YourMessagesLabel.Text = ex.ToString();
            }
            
        }

        if (Session["User"] == null)
        {
            Response.Redirect("home");
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
            Data dat = new Data(isn); DataView dvEvent = dat.GetDataDV("SELECT * FROM Trips T WHERE T.ID=" + ID);
            EventNameTextBox.Text = dvEvent[0]["Header"].ToString();

            HowDressTextBox.Text = dvEvent[0]["HowDress"].ToString();
            WhatObtainTextBox.Text = dvEvent[0]["WhatObtain"].ToString();

            if (isedit)
            {
                nameLabel.Text = "<h1>You are submitting changes for Adventure: " + dvEvent[0]["Header"].ToString() + "</h1>";
            }
            Session["EffectiveUserName"] = dvEvent[0]["UserName"].ToString();

            DataView dvDirections = dat.GetDataDV("SELECT * FROM TripDirections WHERE TripID=" + ID);
            ListItem item;
            string walking = "";
            foreach (DataRowView row in dvDirections)
            {
                walking = "1";
                if (row["Walking"].ToString() == "False")
                    walking = "0";
                item = new ListItem("Destination " + (DirectionsListBox.Items.Count + 1).ToString(),
                        row["Country"].ToString() + ";" + row["State"].ToString() + ";" +
                        row["City"].ToString() + ";" +
                        row["Zip"].ToString() + ";" + walking + ";" + row["Directions"].ToString() +
                        "-" + row["Address"].ToString());

                DirectionsListBox.Items.Add(item);
            }

            dvDirections = dat.GetDataDV("SELECT * FROM TripMonths WHERE TripID=" + ID);
            foreach (DataRowView row in dvDirections)
            {
                item = new ListItem(dat.GetMonth(row["MonthStart"].ToString()) + ", " + row["DayStart"].ToString() +
                " - " + dat.GetMonth(row["MonthEnd"].ToString()) + "," + row["DayEnd"].ToString(),
                row["MonthStart"].ToString() + ", " + row["DayStart"].ToString() +
                " - " + row["MonthEnd"].ToString() + "," + row["DayEnd"].ToString());
                MonthsListBox.Items.Add(item);
            }

            dvDirections = dat.GetDataDV("SELECT * FROM TripDays WHERE TripID=" + ID);
            string days = "";
            foreach (DataRowView row in dvDirections)
            {
                item = new ListItem(dat.GetHours(row["Days"].ToString()).Replace("<br/>", "") +
                    " - " + row["StartTime"].ToString() +
                    " - " + row["EndTime"].ToString(), row["Days"].ToString() +
                    " - " + row["StartTime"].ToString() +
                    " - " + row["EndTime"].ToString());
                TimeListBox.Items.Add(item);
            }

            dvDirections = dat.GetDataDV("SELECT * FROM Trips_WhatToBring WHERE TripID=" + ID);
            foreach (DataRowView row in dvDirections)
            {
                item = new ListItem((BringListBox.Items.Count + 1).ToString() + ". " +
                    row["WhatToBring"].ToString(), row["WhatToBring"].ToString());
                BringListBox.Items.Add(item);
            }

            foreach (ListItem itemz in MeansCheckList.Items)
            {
                if (dvEvent[0]["Means"].ToString().Contains(itemz.Value))
                    itemz.Selected = true;
            }

            char[] delimer = { '-' };
            char[] delmer = { ':' };

            string[] toksTimes = dvEvent[0]["Duration"].ToString().Split(delimer);
            string[] toksMin = toksTimes[0].Split(delmer);
            string[] toksMax = toksTimes[1].Split(delmer);

            MinHoursTextBox.Text = toksMin[0];
            MinMinsTextBox.Text = toksMin[1];
            MaxHoursTextBox.Text = toksMax[0];
            MaxMinsTextBox.Text = toksMax[1];

            if (dvEvent[0]["MinPrice"] != null)
            {
                if (dvEvent[0]["MinPrice"].ToString() != "")
                {
                    MinTextBox.Text = dvEvent[0]["MinPrice"].ToString();
                }
            }

            if (dvEvent[0]["MaxPrice"] != null)
            {
                if (dvEvent[0]["MaxPrice"].ToString() != "")
                {
                    MaxTextBox.Text = dvEvent[0]["MaxPrice"].ToString();
                }
            }

            bool isEdit = false;
            DateTime startDate;
            DateTime endDate;
            if (Request.QueryString["edit"] != null)
                isEdit = bool.Parse(Request.QueryString["edit"].ToString());


            DescriptionTextBox.Content = dvEvent[0]["Content"].ToString();
            ShortDescriptionTextBox.Text = dvEvent[0]["ShortDescription"].ToString();

            string mediaCategory = dvEvent[0]["mediaCategory"].ToString();
            string youtube = dvEvent[0]["YouTubeVideo"].ToString();

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

                        for (int i = 0; i < youtokens.Length; i++)
                        {
                            if (youtokens[i].Trim() != "")
                            {
                                ListItem newListItem = new ListItem("You Tube ID: " + youtokens[i], youtokens[i]);
                                PictureCheckList.Items.Add(newListItem);
                            }
                        }
                    }
                    string[] fileArray = System.IO.Directory.GetFiles(MapPath(".") + "\\Trips\\" + ID +
                        "\\Slider\\");
                    if (fileArray.Length > 0)
                        PictureNixItButton.Visible = true;

                    if (fileArray.Length > 0)
                    {
                        UploadedVideosAndPics.Visible = true;
                        for (int i = 0; i < fileArray.Length; i++)
                        {
                            if (i == 0)
                                UploadedVideosAndPics.Visible = true;
                            string[] fileTokens = fileArray[i].Split(delim);
                            string nameFile = fileTokens[fileTokens.Length - 1];
                            DataView dvE = dat.GetDataDV("SELECT * FROM Trip_Slider_Mapping WHERE PictureName='" + nameFile +
                                "' AND TripID=" + ID);
                            if (dvE.Count != 0)
                            {
                                ListItem newListItem = new ListItem(dvE[0]["RealPictureName"].ToString(), nameFile);
                                PictureCheckList.Items.Add(newListItem);
                            }
                        }
                    }
                    break;
                default: break;
            }

            //FeatureDatesListBox.Items.Clear();
            //if (bool.Parse(dvEvent[0]["Featured"].ToString()))
            //{
            //    char[] delim = { ';' };
            //    string[] tokens = dvEvent[0]["DaysFeatured"].ToString().Split(delim, StringSplitOptions.RemoveEmptyEntries);
            //    foreach (string token in tokens)
            //    {
            //        item = new ListItem(token);
            //        item.Attributes.Add("class", "DisabledListItem");
            //        item.Value = "Disabled";
            //        FeatureDatesListBox.Items.Add(item);
            //    }
            //}

            //if (Request.QueryString["Feature"] != null)
            //{
            //    if (bool.Parse(Request.QueryString["Feature"]))
            //    {
            //        ChangeSelectedTab(0, 1);
            //        ChangeSelectedTab(1, 2);
            //        ChangeSelectedTab(2, 3);
            //        ChangeSelectedTab(3, 4);
            //        FeaturePanel.Visible = true;
            //        FindPrice();
            //    }
            //}

            SetCategories();
            Session["EventCategoriesSet"] = null;
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
        Data dat = new Data(isn); DataView dvCategories = dat.GetDataDV("SELECT * FROM Trip_Category WHERE TripID=" +
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

        Session["EventCategoriesSet"] = "notnull";
    }
    
    protected void PostIt(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        string problem = "";
        try
        {
            if (AgreeCheckBox.Checked)
            {
                string validateMessage = ValidatePage();
                if (validateMessage == "success")
                {
                    //AuthorizePayPal d = new AuthorizePayPal();
                    bool chargeCard = false;
                    string transactionID = "";
                    string message = "";
                    DateTime isn = DateTime.Now;

                    if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
                        isn = DateTime.Now;
                    DateTime isNow = isn;
                    Data dat = new Data(isn);

                    //Add case for if Paypal is filled in...
                    //Authorize Credit Card
                    //decimal price = 0.00M;
                    bool goOn = true;
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
                    //            if (SearchTermsListBox.Items.Count > 0 && price == 0.00M)
                    //            {
                    //                goOn = false;
                    //                Session["Featured"] = false;
                    //                message = "You have entered search terms, but, have not included any dates.";
                    //            }
                    //            else
                    //            {
                    //                goOn = true;
                    //                Session["Featured"] = false;
                    //            }
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

                        string email = "";
                        string textEmail = "";
                        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
                        conn.Open();

                        string mediaCat = "0";
                        if (PictureCheckList.Items.Count > 0)
                            mediaCat = "1";
                        bool wasFeatured = false;
                        bool isEditing = false;
                        if (isEdit.Text != "")
                        {
                            isEditing = bool.Parse(isEdit.Text);
                        }

                        DataView dvEvent = new DataView();
                        string theCat = "NULL";
                        if (isEditing)
                        {
                            dvEvent = dat.GetDataDV("SELECT * FROM Trips WHERE ID=" + eventID.Text);
                            wasFeatured = bool.Parse(dvEvent[0]["Featured"].ToString());
                            if (dvEvent[0]["MediaCategory"].ToString() != mediaCat)
                            {
                                theCat = mediaCat;
                            }
                        }
                        string command = "";
                        if (isEditing)
                        {
                            command = "UPDATE Trips SET Means = @means, DaysFeatured=@daysFet, Featured=@fet, MinPrice=@min, MaxPrice=@max, " +
                                "[Content]=@content, Header=@header, mediaCategory=" + mediaCat + ", " +
                                "ShortDescription=@shortDescription, WhatObtain=@whatObtain, HowDress=@howDress, " +
                                "LastEditOn=@dateP, Duration=@dur WHERE ID=" + Request.QueryString["ID"].ToString();
                        }
                        else
                        {
                            command = "INSERT INTO Trips (Means, Duration, WhatObtain, HowDress, DaysFeatured,Featured,MinPrice, MaxPrice, [Content], " +
                                 "Header, mediaCategory, UserName, "
                             + "ShortDescription, StarRating, PostedOn)"
                                 + " VALUES(@means, @dur, @whatObtain, @howDress, @daysFet, @fet, @min, @max, @content, @header, "
                                  + mediaCat + ", @userName, @shortDescription"
                             + ", 0, @dateP)";
                        }

                        SqlCommand cmd = new SqlCommand(command, conn);
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.Add("@whatObtain", SqlDbType.NVarChar).Value = WhatObtainTextBox.Text;
                        cmd.Parameters.Add("@howDress", SqlDbType.NVarChar).Value = HowDressTextBox.Text;

                        cmd.Parameters.Add("@dur", SqlDbType.NVarChar).Value = MinHoursTextBox.Text.Trim() + ":" +
                            MinMinsTextBox.Text.Trim() + " - " + MaxHoursTextBox.Text.Trim() + ":" + MaxMinsTextBox.Text.Trim();

                        string means = "";
                        foreach (ListItem check in MeansCheckList.Items)
                        {
                            if (check.Selected)
                                means += ";" + check.Value;
                        }

                        //string fetDays = "";
                        //foreach (ListItem item in FeatureDatesListBox.Items)
                        //{
                        //    fetDays += ";" + item.Text + ";";
                        //}

                        cmd.Parameters.Add("@dateP", SqlDbType.DateTime).Value = DateTime.Now;

                        cmd.Parameters.Add("@means", SqlDbType.NVarChar).Value = means;

                        //if (wasFeatured)
                        //{
                        //    cmd.Parameters.Add("@fet", SqlDbType.Bit).Value = true;

                        //    if (FeaturePanel.Visible)
                        //    {
                        //        cmd.Parameters.Add("@daysFet", SqlDbType.NVarChar).Value = fetDays;
                        //    }
                        //    else
                        //        cmd.Parameters.Add("@daysFet", SqlDbType.NVarChar).Value = dvEvent[0]["DaysFeatured"].ToString();

                        //}
                        //else
                        //{
                            cmd.Parameters.Add("@fet", SqlDbType.Bit).Value = false;
                            //if (FeaturePanel.Visible)
                            //{
                            //    cmd.Parameters.Add("@daysFet", SqlDbType.NVarChar).Value = DBNull.Value;
                            //}
                            //else
                            //{
                                cmd.Parameters.Add("@daysFet", SqlDbType.NVarChar).Value = DBNull.Value;
                            //}
                        //}

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

                        if (isEditing)
                        {
                            cmd.Parameters.Add("@content", SqlDbType.NVarChar).Value = DescriptionTextBox.Content;
                            cmd.Parameters.Add("@header", SqlDbType.NVarChar).Value = EventNameTextBox.Text;
                            cmd.Parameters.Add("@shortDescription", SqlDbType.NVarChar).Value = ShortDescriptionTextBox.Text;
                            cmd.Parameters.Add("@userName", SqlDbType.Int).Value = Session["User"].ToString();
                        }
                        else
                        {
                            cmd.Parameters.Add("@content", SqlDbType.NVarChar).Value = DescriptionTextBox.Content;
                            cmd.Parameters.Add("@header", SqlDbType.NVarChar).Value = EventNameTextBox.Text;
                            cmd.Parameters.Add("@shortDescription", SqlDbType.NVarChar).Value = ShortDescriptionTextBox.Text;

                            if (!isEditing)
                            {
                                cmd.Parameters.Add("@rating", SqlDbType.Int).Value = 0;
                                cmd.Parameters.Add("@userName", SqlDbType.NVarChar).Value = Session["UserName"].ToString();
                            }
                        }


                        if (isEditing)
                        {
                            cmd.ExecuteNonQuery();
                        }

                        if (!isEditing)
                        {
                            cmd.ExecuteNonQuery();
                        }

                        bool mediaChanged = false;

                        cmd = new SqlCommand("SELECT @@IDENTITY AS ID", conn);
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd);
                        DataSet ds3 = new DataSet();
                        da2.Fill(ds3);

                        string ID = ds3.Tables[0].Rows[0]["ID"].ToString();

                        if (isEditing)
                        {
                            ID = Request.QueryString["ID"].ToString();
                            dat.Execute("DELETE FROM TripDays WHERE TripID=" + ID);
                            dat.Execute("DELETE FROM TripMonths WHERE TripID=" + ID);
                            dat.Execute("DELETE FROM Trips_WhatToBring WHERE TripID=" + ID);
                            dat.Execute("DELETE FROM TripDirections WHERE TripID=" + ID);
                        }


                        #region Take Care of Trip Days
                        char[] delter = { '-' };
                        string[] tokns;
                        foreach (ListItem intem in TimeListBox.Items)
                        {
                            tokns = intem.Value.Split(delter, StringSplitOptions.RemoveEmptyEntries);
                            dat.Execute("INSERT INTO TripDays (TripID, Days, StartTime, EndTime)" +
                                "VALUES(" + ID + ", '" + tokns[0].Trim() + "', '" + tokns[1].Trim() + "', '" + tokns[2].Trim() + "')");
                        }
                        #endregion

                        #region Take Care of Trip Months
                        char[] delterim = { ',' };
                        string[] toks2;
                        string[] toks3;
                        foreach (ListItem intem in MonthsListBox.Items)
                        {
                            tokns = intem.Value.Split(delter);
                            toks2 = tokns[0].Split(delterim);
                            toks3 = tokns[1].Split(delterim);
                            dat.Execute("INSERT INTO TripMonths (TripID, MonthStart, DayStart, MonthEnd, DayEnd)" +
                                "VALUES(" + ID + ", '" + toks2[0] + "', '" + toks2[1] + "', '" +
                                toks3[0] + "', '" + toks3[1] + "')");
                        }
                        #endregion

                        #region Take Care of Trip What To Bring
                        foreach (ListItem intem in BringListBox.Items)
                        {
                            dat.Execute("INSERT INTO Trips_WhatToBring (TripID, WhatToBring)" +
                                "VALUES(" + ID + ", '" + intem.Value.Replace("'", "''") + "')");
                        }
                        #endregion

                        #region Take Care of Trip Directions
                        char[] delim = { ';' };
                        string[] locTokens;
                        string[] addressToks;
                        foreach (ListItem intem in DirectionsListBox.Items)
                        {
                            tokns = intem.Value.Split(delter);
                            locTokens = tokns[0].Split(delim);
                            addressToks = tokns[1].Split(delim);
                            dat.Execute("INSERT INTO TripDirections (TripID, Directions, Address, City, " +
                                "State, Country, Zip, Walking)" +
                                "VALUES(" + ID + ", '" + locTokens[5].Replace("'", "''") + "', '" + tokns[1] +
                                "', '" + locTokens[2] + "', '" + locTokens[1] + "', " + locTokens[0] + ", '" + locTokens[3] +
                                "', " + locTokens[4] + " )");
                        }
                        #endregion


                        string temporaryID = eventID.Text;

                        string categories =
                            CreateCategories(ID, isEditing);

                        //#region Take care of search terms
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
                        //            dat.Execute("INSERT INTO TripSearchTerms (TripID, SearchTerms, SearchDate) VALUES(" + ID +
                        //                ", '" + terms.Replace("'", "''") + "', '" + item.Text + "')");
                        //    }
                        //}
                        //#endregion

                        string temp = categories;

                        #region Take Care of Media

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
                            string[] fileArray = System.IO.Directory.GetFiles(MapPath(".") +
                                "\\UserFiles\\" + Session["EffectiveUserName"].ToString() + "\\Slider\\");

                            if (!System.IO.Directory.Exists(MapPath(".") + "\\Trips\\" + ID))
                            {
                                System.IO.Directory.CreateDirectory(MapPath(".") + "\\Trips\\" + ID + "\\");
                                System.IO.Directory.CreateDirectory(MapPath(".") + "\\Trips\\" + ID + "\\Slider\\");
                            }
                            else
                            {
                                if (!System.IO.Directory.Exists(MapPath(".") + "\\Trips\\" + ID + "\\Slider\\"))
                                {
                                    System.IO.Directory.CreateDirectory(MapPath(".") + "\\Trips\\" + ID + "\\Slider\\");
                                }
                            }

                            string YouTubeStr = "";
                            char[] delim3 = { '.' };
                            for (int i = 0; i < PictureCheckList.Items.Count; i++)
                            {
                                //int length = fileArray[i].Split(delim2).Length;
                                string[] tokens = PictureCheckList.Items[i].Value.ToString().Split(delim3);


                                if (tokens.Length >= 2)
                                {
                                    if (tokens[1].ToUpper() == "JPG" || tokens[1].ToUpper() == "JPEG" || tokens[1].ToUpper() == "GIF" || tokens[1].ToUpper() == "PNG")
                                    {
                                        if (!System.IO.File.Exists(MapPath(".") + "\\Trips\\" + tempID + "\\Slider\\" + PictureCheckList.Items[i].Value))
                                        {
                                            System.IO.File.Copy(MapPath(".") + "\\UserFiles\\" + Session["EffectiveUserName"].ToString() +
                                                                    "\\Slider\\" + PictureCheckList.Items[i].Value,
                                                                    MapPath(".") + "\\Trips\\" + tempID + "\\Slider\\" + PictureCheckList.Items[i].Value);
                                        }

                                        if (isEditing)
                                        {
                                            mediaChanged = true;

                                            if (i == 0)
                                                dat.Execute("DELETE FROM Trip_Slider_Mapping WHERE TripID=" + eventID.Text);
                                        }
                                        cmd = new SqlCommand("INSERT INTO Trip_Slider_Mapping (TripID, PictureName, RealPicturename) " +
                                            "VALUES(@eventID, @picName, @realName)", conn);
                                        cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = tempID;
                                        cmd.Parameters.Add("@picName", SqlDbType.NVarChar).Value = PictureCheckList.Items[i].Value;
                                        cmd.Parameters.Add("@realName", SqlDbType.NVarChar).Value = PictureCheckList.Items[i].Text;
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    mediaChanged = true;
                                    YouTubeStr += PictureCheckList.Items[i].Value + ";";
                                }

                            }

                            if (YouTubeStr != "")
                                dat.Execute("UPDATE Trips SET mediaCategory=" + mediaCat + ", YouTubeVideo='" + YouTubeStr + "' WHERE ID=" + ID);

                        }

                        #endregion

                        conn.Close();

                        Encryption encrypt = new Encryption();
                        string moreMessage = "";

                        string emailBody = "";

                        try
                        {
                            //if (chargeCard)
                            //{
                            //    //Charge Card
                            //    string country = dat.GetDataDV("SELECT country_2_code FROM Countries WHERE country_id=" + BillingCountry.SelectedValue)[0]["country_2_code"].ToString();
                            //    com.paypal.sdk.util.NVPCodec status = d.DoCaptureCode(transactionID, price.ToString(),
                            //        "T" + ID + isn.ToString(), "Capture Transaction for Featuring Trip '" +
                            //        dat.MakeNiceNameFull(EventNameTextBox.Text) + "'");
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
                            //            TakeCareOfPostEmail(isEditing, ID);
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
                                //MessagePanel.Visible = true;
                                //YourMessagesLabel.Text = "no charge here";
                                TakeCareOfPostEmail(isEditing, ID);
                            //}
                        }
                        catch (Exception ex)
                        {
                            MessagePanel.Visible = true;
                            YourMessagesLabel.Text += "problem: " + problem + ex.ToString();
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
        catch (Exception ex)
        {
            MessagePanel.Visible = true;
            YourMessagesLabel.Text = "problem: " + problem + ex.ToString();
        }
    }

    protected void TakeCareOfPostEmail(bool isEditing, string tripID)
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

        Encryption encrypt = new Encryption();
        Session["Message"] = "Your adventure has been posted successfully!<br/> Here are your next " +
            "steps. An email with these choices will also be sent to your account. <br/><br/>";
        Session["Message"] += moreMessage + "<br/><br/>" + "-Go to <a class=\"NavyLink\" onclick=\"Search('/" + dat.MakeNiceName(EventNameTextBox.Text) + "_" + tripID +
                "_Trip');\" style=\"text-decoration: underline;\">this adventure's</a> home page.<br/><br/> "+
                "-<a class=\"NavyLink\" style=\"text-decoration: underline;\" onclick=\"Search('RateExperience.aspx?Type=T&ID=" + tripID + "');\" >Rate </a>your user experience posting this adventure.";

        emailBody = "<br/><br/>Dear " + dsUser.Tables[0].Rows[0]["UserName"].ToString() + ", <br/><br/> you have successfully posted the adventure \"" + EventNameTextBox.Text +
        "\". <br/><br/> You can find this adventure <a href=\"http://hippohappenings.com/" + dat.MakeNiceName(EventNameTextBox.Text) + "_" + tripID +
                "_Trip\">here</a>. " +
        "<br/><br/> To rate your experience posting this adventure <a href=\"http://hippohappenings.com/RateExperience.aspx?Type=T&ID=" + tripID + "\">please include your feedback here.</a>" +
        "<br/><br/><br/>Have a HippoHappening Day!<br/><br/>";


        dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
            System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
            dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "You have successfully posted the adventure: " +
            EventNameTextBox.Text);

        Session["message"] = Session["Message"].ToString() +
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
            "</div><br/>";

        MessageRadWindow.NavigateUrl = "Message.aspx";
        MessageRadWindow.Visible = true;
        MessageRadWindow.VisibleOnPageLoad = true;
    }

    protected void ModifyIt(object sender, EventArgs e)
    {

    }

    protected string CreateCategories(string tripID, bool isUpdate)
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
        dat.Execute("DELETE FROM Trip_Category WHERE TripID=" + tripID);

        DataSet dsCategories = dat.GetData("SELECT * FROM Trip_Category WHERE TripID=" + tripID);
        DataView dvCat = new DataView(dsCategories.Tables[0], "", "", DataViewRowState.CurrentRows);

        categories += GetCategoriesFromTree(isUpdate, ref CategoryTree, dvCat, tripID);
        categories += GetCategoriesFromTree(isUpdate, ref RadTreeView2, dvCat, tripID);

        return categories;
    }

    protected string GetCategoriesFromTree(bool isUpdate, ref Telerik.Web.UI.RadTreeView CategoryTree,
        DataView dvCat, string tripID)
    {
        string categories = "";
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn); for (int i = 0; i < CategoryTree.Nodes.Count; i++)
        {
            categories = GetCategoriesFromNode(isUpdate, CategoryTree.Nodes[i], dvCat, tripID);
        }
        return categories;
    }

    protected string GetCategoriesFromNode(bool isUpdate,
        Telerik.Web.UI.RadTreeNode TreeNode, DataView dvCat, string tripID)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn); string categories = "";

        if (TreeNode.Checked && TreeNode.Enabled)
        {
            dvCat.RowFilter = "CategoryID=" + TreeNode.Value;
            dat.Execute("INSERT INTO Trip_Category (CategoryID, TripID, tagSize) VALUES("
                + TreeNode.Value + "," + tripID + ", 22)");
            if (categories != "")
                categories += " OR ";
            categories += " UC.CategoryID=" + TreeNode.Value;
        }

        if (TreeNode.Nodes.Count > 0)
        {
            for (int j = 0; j < TreeNode.Nodes.Count; j++)
            {
                GetCategoriesFromNode(isUpdate, TreeNode.Nodes[j], dvCat, tripID);
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

    //        if (FeaturePanel.Visible)
    //        {
    //            FillChart(new object(), new EventArgs());
    //        }

    //        switch (selectedIndex)
    //        {
    //            case 0:
    //                bool eventName = false;
    //                goOn = true;
    //                EventNameTextBox.Text = dat.stripHTML(EventNameTextBox.Text);

    //                if (EventNameTextBox.Text != "")
    //                    if (EventNameTextBox.Text.Trim().Length > 70)
    //                    {
    //                        eventName = false;
    //                        MessagePanel.Visible = true;
    //                        YourMessagesLabel.Text += "The adventure title needs to be less than or equal to 70 characters. ";
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
    //                    YourMessagesLabel.Text += "The adventure title is required. ";
    //                    return false;
    //                }

    //                if (DirectionsListBox.Items.Count == 0)
    //                {
    //                    goOn = false;
    //                    MessagePanel.Visible = true;
    //                    YourMessagesLabel.Text += "Please include at least one location/directions. ";
    //                    return false;
    //                }

    //                bool gotChecked = false;
    //                foreach (ListItem item in MeansCheckList.Items)
    //                {
    //                    if (item.Selected)
    //                    {
    //                        gotChecked = true;
    //                        break;
    //                    }
    //                }

    //                if (!gotChecked)
    //                {
    //                    goOn = false;
    //                    MessagePanel.Visible = true;
    //                    YourMessagesLabel.Text += "Please include at least one means of getting around on this adventure. ";
    //                    return false;
    //                }
    //                int tryMinHours = 0;
    //                int tryMinMins = 0;
    //                int tryMaxHours = 0;
    //                int tryMaxMins = 0;
    //                if (MinHoursTextBox.Text.Trim() == "" || MinMinsTextBox.Text.Trim() == "" ||
    //                    MaxHoursTextBox.Text.Trim() == "" || MaxMinsTextBox.Text.Trim() == "")
    //                {
    //                    goOn = false;
    //                    MessagePanel.Visible = true;
    //                    YourMessagesLabel.Text += "Please include all adventure duration times. ";
    //                    return false;
    //                }
    //                else
    //                {
    //                    if (int.TryParse(MinHoursTextBox.Text.Trim(), out tryMinHours) && int.TryParse(MinMinsTextBox.Text.Trim(), out tryMinMins)
    //                        && int.TryParse(MaxHoursTextBox.Text.Trim(), out tryMaxHours) && int.TryParse(MaxMinsTextBox.Text.Trim(), out tryMaxMins))
    //                    {
    //                        if ((decimal.Parse(tryMinHours.ToString()) + (decimal.Parse(tryMinMins.ToString()) / 60.00M)) >
    //                            (decimal.Parse(tryMaxHours.ToString()) + (decimal.Parse(tryMaxMins.ToString()) / 60.00M)))
    //                        {
    //                            goOn = false;
    //                            MessagePanel.Visible = true;
    //                            YourMessagesLabel.Text += "The Minimum amount of time needs to be less than or equal to the Maximum amount of time. ";
    //                            return false;
    //                        }
    //                        else
    //                        {
    //                            if (tryMinMins > 59 || tryMaxMins > 59)
    //                            {
    //                                goOn = false;
    //                                MessagePanel.Visible = true;
    //                                YourMessagesLabel.Text += "Please make sure that your Minimum and Maximum duration minutes are less than an hour. ";
    //                                return false;
    //                            }
    //                        }
    //                    }
    //                    else
    //                    {
    //                        goOn = false;
    //                        MessagePanel.Visible = true;
    //                        YourMessagesLabel.Text += "All adventure duration times must be integers. ";
    //                        return false;
    //                    }
    //                }

    //                if (TimeListBox.Items.Count == 0)
    //                {
    //                    goOn = false;
    //                    MessagePanel.Visible = true;
    //                    YourMessagesLabel.Text += "Please include at least one entry for days and times someone could go on this adventure. ";
    //                    return false;
    //                }

    //                if (MonthsListBox.Items.Count == 0)
    //                {
    //                    goOn = false;
    //                    MessagePanel.Visible = true;
    //                    YourMessagesLabel.Text += "Please include at least one month time frame when this trip could be taken. ";
    //                    return false;
    //                }

    //                if (eventName && goOn)
    //                {
    //                    if(changeTab)
    //                        ChangeSelectedTab(0, 1);

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
    //                WhatObtainTextBox.Text = dat.StripHTML_LeaveLinksNoBr(WhatObtainTextBox.Text.Trim());
    //                HowDressTextBox.Text = dat.StripHTML_LeaveLinksNoBr(HowDressTextBox.Text.Trim());
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
    //                            if (BringListBox.Items.Count == 0)
    //                            {
    //                                MessagePanel.Visible = true;
    //                                YourMessagesLabel.Text += "Please include at list one item to bring on this adventure.";
    //                                return false;
    //                            }
    //                            else
    //                            {
    //                                if (WhatObtainTextBox.Text == "")
    //                                {
    //                                    MessagePanel.Visible = true;
    //                                    YourMessagesLabel.Text += "Please a little bit about what one should hope to obtain from this adventure.";
    //                                    return false;
    //                                }
    //                                else
    //                                {
    //                                    if (HowDressTextBox.Text == "")
    //                                    {
    //                                        MessagePanel.Visible = true;
    //                                        YourMessagesLabel.Text += "Please a little bit about how one should dress for this adventure.";
    //                                        return false;
    //                                    }
    //                                    else
    //                                    {
    //                                        if (changeTab)
    //                                            ChangeSelectedTab(1, 2);

    //                                        return true;
    //                                    }
    //                                }
    //                            }
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
    //                }
    //                if(changeTab)
    //                    ChangeSelectedTab(2, 3);

    //                return true;
    //                break;
    //            case 3:

    //                if (CategorySelected())
    //                {                        
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
    //                string message = "";
    //                if (FeaturePanel.Visible)
    //                {
    //                    if (FeatureDatesListBox.Items.Count == 0 && FeaturePanel.Visible)
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
    //                    goOn = true;
    //                    Session["Featured"] = false;
    //                }
                        
                  
    //                if (goOn)
    //                {
    //                    ChangeSelectedTab(4, 5);
    //                    return true;
    //                }
    //                else
    //                {
    //                    if (message != "")
    //                        YourMessagesLabel.Text = message; 
    //                    else
    //                        YourMessagesLabel.Text = "You have selected to feature the adventure, but not entered any " +
    //                            "specifics. If you do not want to feature the adventure any more, please click 'No, Thank You'.";

                              
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

    protected string ValidatePage()
    {
        string validateMessage = "";
        try
        {
            HttpCookie cookie = Request.Cookies["BrowserDate"];

            DateTime isn = DateTime.Now;

            if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
                isn = DateTime.Now;
            DateTime isNow = isn;
            Data dat = new Data(isn); bool goOn = false;

            bool eventName = false;
            goOn = true;
            EventNameTextBox.Text = dat.stripHTML(EventNameTextBox.Text);

            if (EventNameTextBox.Text != "")
            {
            }
            else
            {
                goOn = false;
                validateMessage += "The adventure title is required. ";
            }

            if (DirectionsListBox.Items.Count == 0)
            {
                goOn = false;
                validateMessage += "Please include at least one location/directions. ";
            }

            bool gotChecked = false;
            foreach (ListItem item in MeansCheckList.Items)
            {
                if (item.Selected)
                {
                    gotChecked = true;
                    break;
                }
            }

            if (!gotChecked)
            {
                goOn = false;
                validateMessage += "Please include at least one means of getting around on this adventure. ";
            }
            int tryMinHours = 0;
            int tryMinMins = 0;
            int tryMaxHours = 0;
            int tryMaxMins = 0;
            if (MinHoursTextBox.Text.Trim() == "" || MinMinsTextBox.Text.Trim() == "" ||
                MaxHoursTextBox.Text.Trim() == "" || MaxMinsTextBox.Text.Trim() == "")
            {
                goOn = false;
                validateMessage += "Please include all adventure duration times. ";
            }
            else
            {
                if (int.TryParse(MinHoursTextBox.Text.Trim(), out tryMinHours) && int.TryParse(MinMinsTextBox.Text.Trim(), out tryMinMins)
                    && int.TryParse(MaxHoursTextBox.Text.Trim(), out tryMaxHours) && int.TryParse(MaxMinsTextBox.Text.Trim(), out tryMaxMins))
                {
                    if ((decimal.Parse(tryMinHours.ToString()) + (decimal.Parse(tryMinMins.ToString()) / 60.00M)) >
                        (decimal.Parse(tryMaxHours.ToString()) + (decimal.Parse(tryMaxMins.ToString()) / 60.00M)))
                    {
                        goOn = false;
                        validateMessage += "The Minimum amount of time needs to be less than or equal to the Maximum amount of time. ";
                    }
                    else
                    {
                        if (tryMinMins > 59 || tryMaxMins > 59)
                        {
                            goOn = false;
                            validateMessage += "Please make sure that your Minimum and Maximum duration minutes are less than an hour. ";
                        }
                    }
                }
                else
                {
                    goOn = false;
                    validateMessage += "All adventure duration times must be integers. ";
                }
            }

            if (TimeListBox.Items.Count == 0)
            {
                goOn = false;
                validateMessage += "Please include at least one entry for days and times someone could go on this adventure. ";
            }

            if (MonthsListBox.Items.Count == 0)
            {
                goOn = false;
                validateMessage += "Please include at least one month time frame when this trip could be taken. ";
            }

            if (goOn)
            {
                MinTextBox.Text = dat.stripHTML(MinTextBox.Text.Trim());
                MaxTextBox.Text = dat.stripHTML(MaxTextBox.Text.Trim());
                DescriptionTextBox.Content = dat.StripHTML_LeaveLinks(DescriptionTextBox.Content.Replace("<div>", "<br/>").Replace("</div>", ""));
                ShortDescriptionTextBox.Text = dat.StripHTML_LeaveLinksNoBr(ShortDescriptionTextBox.Text);
                WhatObtainTextBox.Text = dat.StripHTML_LeaveLinksNoBr(WhatObtainTextBox.Text.Trim());
                HowDressTextBox.Text = dat.StripHTML_LeaveLinksNoBr(HowDressTextBox.Text.Trim());
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

                if (goMax && goMin)
                {
                    if (DescriptionTextBox.Text.Length >= 50)
                        if (ShortDescriptionTextBox.Text.Length <= 150 &&
                            ShortDescriptionTextBox.Text.Length != 0)
                        {
                            if (BringListBox.Items.Count == 0)
                            {
                                validateMessage += "Please include at list one item to bring on this adventure.";
                            }
                            else
                            {
                                if (WhatObtainTextBox.Text == "")
                                {
                                    validateMessage += "Please a little bit about what one should hope to obtain from this adventure.";
                                }
                                else
                                {
                                    if (HowDressTextBox.Text == "")
                                    {
                                        validateMessage += "Please a little bit about how one should dress for this adventure.";
                                    }
                                }
                            }
                        }
                        else
                        {
                            validateMessage += "Make sure that Short Description exists and is less than 150 characters.";
                        }
                    else
                    {
                        validateMessage += "*Make sure you say what you want your viewers to hear about this event! The description must be at least 50 characters.";
                    }
                }
                else
                {
                    if (!goMin)
                    {
                        validateMessage += "*The Min Price has to be a number.";
                    }
                    else
                    {
                        validateMessage += "*The Max Price has to be a number.";
                    }
                    goOn = false;

                }

                if(!(goMin && goMax))
                    goOn = false;

                if (!CategorySelected())
                {
                    goOn = false;
                    validateMessage += "Must select at least one category.";

                }
            }

            if (goOn)
                validateMessage = "success";
        }
        catch (Exception ex)
        {
            validateMessage += ex.ToString();
        }

        return validateMessage;
    }

    //protected void Backwards(object sender, EventArgs e)
    //{
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
    //        default: break;
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
                System.Configuration.ConfigurationManager.AppSettings["categoryemail"].ToString(), "Category has been suggested from 'enter-trip'. The user ID who suggested " +
                "the category is userID: '" + Session["User"].ToString() + "'. The category suggestion is '" + CategoriesTextBox.Text + "'", "Hippo Happenings category suggestion");
            CategoriesTextBox.Text = "";
        }
        else
        {
            MessagePanel.Visible = true;

            YourMessagesLabel.Text = "Please type in the description and name of the category you want to suggest.";
        }
    }
    
    //private void ChangeSelectedTab(int unselectIndex, short selectIndex)
    //{
    //    HttpCookie cookie = Request.Cookies["BrowserDate"];

    //    DateTime isn = DateTime.Now;

    //    if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
    //        isn = DateTime.Now;
    //    DateTime isNow = isn;
    //    Data dat = new Data(isn);
    //    char[] delim = { ';' };
    //    char[] delter = { '-' };
    //    string[] locTokens;
    //    string[] addressToks;
    //    ListItem intem = DirectionsListBox.Items[0];

    //    string [] tokns = intem.Value.Split(delter);
    //    locTokens = tokns[0].Split(delim);

    //    string country = locTokens[0];
    //    string zip = locTokens[3];
    //    string city = locTokens[2];
    //    string majorCityID = GetMajorCity();
    //    if (country == "223")
    //    {
    //        MajorCity.Text = dat.GetDataDV("SELECT * FROM MajorCities WHERE ID=" + majorCityID)[0]["MajorCity"].ToString();
    //    }
    //    else
    //    {
    //        MajorCity.Text = city;
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
    //                    TermsErrorLabel.Text = "There are already too many featured adventures " +
    //                    "with the search term and date combination you are trying to add. " +
    //                    "The combination in question is: " + troubleTerms;
    //                }
    //            }
    //            else
    //            {
    //                TermsErrorLabel.Text = "Your adventure is already featured for this date.";
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
    //        TermsErrorLabel.Text = ex.ToString();
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

    //    DateTime isn = DateTime.Now;

    //    if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
    //        isn = DateTime.Now;
    //    DateTime isNow = isn;
    //    Data dat = new Data(isn);
    //    TermsBox.Text = dat.stripHTML(TermsBox.Text);
    //    string troubleTerms = "";
    //    if (TermsBox.Text.Trim() != "")
    //    {
    //        if (!TermsBox.Text.Contains(" "))
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
    //                    TermsErrorLabel.Text = "There are already too many featured adventures " +
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
    //            TermsErrorLabel.Text = "Search terms cannot contain spaces.";
    //        }
    //    }
    //    else
    //    {
    //        TermsErrorLabel.Text = "Please include a search term.";
    //    }
    //}

    //protected bool CheckFor4Bulletins(bool isTerm, string TermOrDateString, ref string troubleDateOrTerm)
    //{
    //    HttpCookie cookie = Request.Cookies["BrowserDate"];

    //    DateTime isn = DateTime.Now;

    //    if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
    //        isn = DateTime.Now;
    //    DateTime isNow = isn;
    //    Data dat = new Data(isn);
    //    DataView dv;
    //    bool termExists = false;
    //    foreach (ListItem itemDate in FeatureDatesListBox.Items)
    //    {
    //        //Check if the current term we're trying to bring in would exceed 4 with any date
    //        if (isTerm)
    //        {
    //            dv = dat.GetDataDV("SELECT * FROM TripSearchTerms WHERE SearchDate = '" + itemDate.Text +
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
    //            dv = dat.GetDataDV("SELECT * FROM TripSearchTerms WHERE SearchDate = '" + itemDate.Text +
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
    //                dv = dat.GetDataDV("SELECT * FROM TripSearchTerms WHERE SearchDate = '" + TermOrDateString +
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

    //protected void RemoveSearchTerm(object sender, EventArgs e)
    //{
    //    if (SearchTermsListBox.SelectedItem != null)
    //    {
    //        SearchTermsListBox.Items.Remove(SearchTermsListBox.SelectedItem);
    //        FillChart(new object(), new EventArgs());
    //    }
    //}

    //protected void FillChart(object sender, EventArgs e)
    //{
    //    PricingLiteral.Text = "<td align='center'>0</td><td align='center'>$0</td><td align='center'>$0</td><td align='center'>$0</td><td align='center'>$0</td>";

    //    bool hasNewFeaturedItems = false;

    //    foreach (ListItem item in FeatureDatesListBox.Items)
    //    {
    //        if (item.Value != "Disabled")
    //        {
    //            hasNewFeaturedItems = true;
    //            break;
    //        }
    //    }

    //    if (hasNewFeaturedItems)
    //    {

    //        HttpCookie cookie = Request.Cookies["BrowserDate"];

    //        DateTime isn = DateTime.Now;

    //        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
    //            isn = DateTime.Now;
    //        DateTime isNow = isn;
    //        Data dat = new Data(isn);
    //        DataView dvChart = dat.GetDataDV("SELECT * FROM StandardTripPricing");

    //        int daysCount = 0;
    //        foreach (ListItem item in FeatureDatesListBox.Items)
    //        {
    //            if (item.Value != "Disabled")
    //            {
    //                daysCount++;
    //            }
    //        }

    //        char[] delim = { ';' };
    //        char[] delter = { '-' };

    //        string[] tokns = DirectionsListBox.Items[0].Value.Split(delter);
    //        string[] locTokens = tokns[0].Split(delim);

    //        string city = locTokens[2];
    //        string state = locTokens[1];
    //        string country = locTokens[0];
    //        string zips = "";
    //        string majorCityID = GetMajorCity();
    //        DataView dvCountMembers;
    //        if (country == "223")
    //        {
    //            //Get Major City
    //            zips = dat.GetDataDV("SELECT AllZips FROM MajorCities WHERE ID=" + majorCityID)[0]["AllZips"].ToString();

    //            dvCountMembers = dat.GetDataDV("SELECT COUNT(*) AS Count FROM UserPreferences WHERE MajorCity = " +
    //                majorCityID);

    //        }
    //        else
    //        {
    //            dvCountMembers = dat.GetDataDV("SELECT COUNT(*) AS Count FROM UserPreferences WHERE CatCity = '" +
    //                city + "' AND CatCountry = " + country +
    //                " AND CatState = '" + state + "'");

    //        }


    //        int indexToLookAt = daysCount - 1;

    //        if (daysCount > 3)
    //            indexToLookAt = 3;

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
    //        decimal eventCap = decimal.Round(decimal.Parse(dvChart[indexToLookAt]["PerTripCap"].ToString()), 2);
    //        decimal subtractionForEvents = 0.00M;
    //        decimal total = 0.00M;

    //        PricingLiteral.Text = "";
    //        zips = zips.Replace("CatZip", "TD.Zip");
    //        foreach (ListItem item in FeatureDatesListBox.Items)
    //        {
    //            if (item.Value != "Disabled")
    //            {
    //                dateDate = DateTime.Parse(item.Text);
    //                if (country == "223")
    //                {
    //                    dvCountEvents = dat.GetDataDV("SELECT DISTINCT TripID FROM Trips T, TripDirections TD WHERE T.ID=TD.TripID AND T.Featured='True'  AND (" + zips +
    //                                ") AND T.DaysFeatured LIKE '%;" + dateDate.Month.ToString() + "/" +
    //                    dateDate.Day.ToString() + "/" + dateDate.Year.ToString() + ";%'");
    //                }
    //                else
    //                {
    //                    dvCountEvents = dat.GetDataDV("SELECT DISTINCT TripID FROM Trips T, TripDirections TD WHERE T.ID=TD.TripID AND T.Featured='True' AND TD.City = '" +
    //                    city + "' AND TD.Country = " + country +
    //                    " AND TD.State = '" + state + "' AND T.DaysFeatured LIKE '%;" + dateDate.Month.ToString() + "/" +
    //                    dateDate.Day.ToString() + "/" + dateDate.Year.ToString() + ";%'");
    //                }

    //                if (decimal.Parse(dvChart[indexToLookAt]["PerTripPricing"].ToString()) *
    //                    decimal.Parse(dvCountEvents.Count.ToString()) > eventCap)
    //                {
    //                    subtractionForEvents = eventCap;
    //                }
    //                else
    //                {
    //                    subtractionForEvents = decimal.Round(decimal.Parse(dvChart[indexToLookAt]["PerTripPricing"].ToString()) *
    //                        decimal.Parse(dvCountEvents.Count.ToString()), 3, MidpointRounding.AwayFromZero);
    //                }

    //                PricingLiteral.Text += "<tr><td align=\"center\" >" + item.Text + "</td><td align=\"center\">" +
    //                    decimal.Round(decimal.Parse(dvChart[indexToLookAt]["StandardTripPricing"].ToString()), 3, MidpointRounding.AwayFromZero).ToString() +
    //                    "</td><td align=\"center\">$" + priceForMembers.ToString() + "</td><td align=\"center\">-$" + subtractionForEvents.ToString() + "</td>" +
    //                    "<td align=\"center\">$" + decimal.Round((decimal.Parse(dvChart[indexToLookAt]["StandardTripPricing"].ToString()) +
    //                    priceForMembers - subtractionForEvents), 3, MidpointRounding.AwayFromZero).ToString() + "</td></tr>";


    //                total += decimal.Round((decimal.Parse(dvChart[indexToLookAt]["StandardTripPricing"].ToString()) +
    //                    priceForMembers - subtractionForEvents), 3, MidpointRounding.AwayFromZero);
    //            }
    //        }

    //        decimal searchTermTotal = 0.05M * SearchTermsListBox.Items.Count * daysCount;
    //        NumSearchTerms.Text = searchTermTotal.ToString();
    //        TotalLabel.Text = decimal.Round((total + searchTermTotal), 2, MidpointRounding.AwayFromZero).ToString();

    //        if (decimal.Round((total + searchTermTotal), 2, MidpointRounding.AwayFromZero) > 0.00M)
    //            PaymentPanel.Visible = true;
    //        else
    //            PaymentPanel.Visible = false;
    //    }
    //    else
    //    {
    //        PaymentPanel.Visible = false;
    //        TotalLabel.Text = "0.00";
    //    }
    //}

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
            USPanel.Visible = true;
            InternationalPanel.Visible = false;
        }
        else
        {
            USPanel.Visible = false;
            InternationalPanel.Visible = true;
        }
    }

    protected void AddDestination(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn);
        DirectionTextBox.Text = dat.stripHTML(DirectionTextBox.Text.Trim()).Replace(";", "");
        if (DirectionTextBox.Text.Trim() != "")
        {
            string country = CountryDropDown.SelectedValue;
            string state = "";
            string zip = "";

            if (ZipTextBox.Text.Trim() != "")
            {
                int zipInt = 0;
                if ((int.TryParse(ZipTextBox.Text.Trim(), out zipInt) && country == "223") || country != "223")
                {
                    if (StateDropDownPanel.Visible)
                    {
                        state = StateDropDown.SelectedItem.Text;
                    }
                    else
                    {
                        StateTextBox.Text = dat.stripHTML(StateTextBox.Text.Trim());
                        if (StateTextBox.Text != "")
                        {
                            state = StateTextBox.Text;
                        }
                        else
                        {
                            DirectionsErrorLabel.Text = "Please include the state.";
                        }
                    }

                    if (state != "")
                    {
                        VenueCityTextBox.Text = dat.stripHTML(VenueCityTextBox.Text.Trim());
                        if (VenueCityTextBox.Text.Trim() != "")
                        {
                            if ((country == "223" && StreetNumberTextBox.Text.Trim() != ""
                                && StreetNameTextBox.Text.Trim() != "" && StreetDropDown.Text != "Select One...") ||
                                (country != "223" && LocationTextBox.Text.Trim() != ""))
                            {
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

                                ListItem item = new ListItem("Destination " + (DirectionsListBox.Items.Count + 1).ToString(),
                                    country + ";" + state + ";" + VenueCityTextBox.Text.Trim() + ";" +
                                    ZipTextBox.Text.Trim() + ";" + WalkingDropDown.SelectedValue + ";" + DirectionTextBox.Text + "-" + locationStr);

                                string valToCheck = country + ";" + state + ";" + VenueCityTextBox.Text.Trim() + ";" +
                                    ZipTextBox.Text.Trim() + ";" + locationStr;

                                bool hasValue = false;
                                char[] delim = { ';' };
                                char[] delimer = { '-' };
                                string[] tokens;
                                string[] toks;
                                string theDescript = "";
                                foreach (ListItem it in DirectionsListBox.Items)
                                {
                                    toks = it.Value.Split(delimer, StringSplitOptions.RemoveEmptyEntries);
                                    tokens = it.Value.Split(delim, StringSplitOptions.RemoveEmptyEntries);
                                    if (tokens[0] + ";" + tokens[1] + ";" + tokens[2] + ";" + tokens[3] +
                                        ";" + toks[1] == valToCheck)
                                    {
                                        hasValue = true;
                                        theDescript = it.Text;
                                        break;
                                    }
                                }

                                if (!hasValue)
                                    DirectionsListBox.Items.Add(item);
                                else
                                {
                                    DirectionsErrorLabel.Text = "This same address is already associated with " + theDescript + ". ";
                                }
                            }
                            else
                            {
                                DirectionsErrorLabel.Text = "Please include all required field for the address.";
                            }
                        }
                        else
                        {
                            DirectionsErrorLabel.Text = "Please include the city.";
                        }
                    }
                }
                else
                {
                    DirectionsErrorLabel.Text = "Zip must be a 5 digit number.";
                }
            }
            else
            {
                DirectionsErrorLabel.Text = "Please include the zip code.";
            }
            
        }
        else
        {
            DirectionsErrorLabel.Text = "Please include the directions for this step.";
        }
    }

    protected void RemoveDestination(object sender, EventArgs e)
    {
        if (DirectionsListBox.SelectedItem != null)
        {
            DirectionsListBox.Items.Remove(DirectionsListBox.SelectedItem);
        }
    }

    protected void EditDestination(object sender, EventArgs e)
    {
        try
        {
            if (DirectionsListBox.SelectedItem != null)
            {
                char[] delim = { ';' };
                char[] delter = { '-' };
                string[] toks = DirectionsListBox.SelectedItem.Value.Split(delter);
                string[] tokens = toks[0].Split(delim, StringSplitOptions.RemoveEmptyEntries);
                string[] addressToks = toks[1].Split(delim, StringSplitOptions.RemoveEmptyEntries);
                EditLabel.Text = DirectionsListBox.SelectedItem.Text;

                DirectionTextBox.Text = tokens[5];
                WalkingDropDown.ClearSelection();
                WalkingDropDown.Items.FindByValue(tokens[4]).Selected = true;
                CountryDropDown.ClearSelection();
                CountryDropDown.Items.FindByValue(tokens[0]).Selected = true;
                ChangeState(new object(), new EventArgs());
                if (StateDropDownPanel.Visible)
                {
                    StateDropDown.ClearSelection();
                    StateDropDown.Items.FindByText(tokens[1]).Selected = true;
                }
                else
                {
                    StateTextBox.Text = tokens[1];
                }
                VenueCityTextBox.Text = tokens[2];
                ZipTextBox.Text = tokens[3];

                char[] delimeter = { ' ' };
                string[] aptToks;
                if (tokens[0] == "223")
                {
                    StreetNumberTextBox.Text = addressToks[0];
                    StreetNameTextBox.Text = addressToks[1];
                    StreetDropDown.ClearSelection();
                    StreetDropDown.Items.FindByText(addressToks[2]).Selected = true;
                    if (addressToks.Length > 3)
                    {
                        aptToks = addressToks[3].Split(delimeter);
                        AptDropDown.ClearSelection();
                        AptDropDown.Items.FindByText(aptToks[0]).Selected = true;
                        AptNumberTextBox.Text = aptToks[1];
                    }
                }
                else
                {
                    LocationTextBox.Text = addressToks[0];
                    if (addressToks.Length > 1)
                    {
                        aptToks = addressToks[1].Split(delimeter);
                        AptDropDown.ClearSelection();
                        AptDropDown.Items.FindByText(aptToks[0]).Selected = true;
                        AptNumberTextBox.Text = aptToks[1];
                    }
                }
            }
        }
        catch (Exception ex)
        {
            DirectionsErrorLabel.Text = ex.ToString() + "<br/>" + DirectionsListBox.SelectedItem.Value;
        }
    }

    protected void ModifyDestination(object sender, EventArgs e)
    {
        try
        {
            HttpCookie cookie = Request.Cookies["BrowserDate"];

            DateTime isn = DateTime.Now;

            if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
                isn = DateTime.Now;
            DateTime isNow = isn;
            Data dat = new Data(isn);
            if (EditLabel.Text != "")
            {
                DirectionTextBox.Text = dat.stripHTML(DirectionTextBox.Text.Trim()).Replace(";", "");
                if (DirectionTextBox.Text.Trim() != "")
                {
                    string country = CountryDropDown.SelectedValue;
                    string state = "";
                    string zip = "";

                    if (ZipTextBox.Text.Trim() != "")
                    {
                        if (StateDropDownPanel.Visible)
                        {
                            state = StateDropDown.SelectedItem.Text;
                        }
                        else
                        {
                            StateTextBox.Text = dat.stripHTML(StateTextBox.Text.Trim());
                            if (StateTextBox.Text != "")
                            {
                                state = StateTextBox.Text;
                            }
                            else
                            {
                                DirectionsErrorLabel.Text = "Please include the state.";
                            }
                        }

                        if (state != "")
                        {
                            VenueCityTextBox.Text = dat.stripHTML(VenueCityTextBox.Text.Trim());
                            if (VenueCityTextBox.Text.Trim() != "")
                            {
                                if ((country == "223" && StreetNumberTextBox.Text.Trim() != ""
                                    && StreetNameTextBox.Text.Trim() != "" && StreetDropDown.Text != "Select One...") ||
                                    (country != "223" && LocationTextBox.Text.Trim() != ""))
                                {
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

                                    DirectionsListBox.Items.FindByText(EditLabel.Text).Value = country + ";" +
                                        state + ";" + VenueCityTextBox.Text.Trim() + ";" + ZipTextBox.Text.Trim() + ";" +
                                        WalkingDropDown.SelectedValue + ";" + DirectionTextBox.Text + "-" + locationStr;
                                }
                                else
                                {
                                    DirectionsErrorLabel.Text = "Please include all required field for the address.";
                                }
                            }
                            else
                            {
                                DirectionsErrorLabel.Text = "Please include the city.";
                            }
                        }
                    }
                    else
                    {
                        DirectionsErrorLabel.Text = "Please include the zip code.";
                    }

                }
                else
                {
                    DirectionsErrorLabel.Text = "Please include the directions for this step.";
                }
            }
        }
        catch (Exception ex)
        {
            DirectionsErrorLabel.Text = ex.ToString();
        }
    }

    protected void AddTime(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn);
        if (StartTimePicker.SelectedDate != null && EndTimePicker.SelectedDate != null)
        {
            //if (StartTimePicker.SelectedDate.Value < EndTimePicker.SelectedDate.Value)
            //{
                if (DaysOfWeekListBox.SelectedItem != null)
                {
                    string days = "";
                    foreach (ListItem it in DaysOfWeekListBox.Items)
                    {
                        if (it.Selected)
                            days += it.Value + ";";
                    }

                    ListItem item = new ListItem(dat.GetHours(days).Replace("<br/>", "") + " - " + StartTimePicker.SelectedDate.Value.ToShortTimeString() +
                        " - " + EndTimePicker.SelectedDate.Value.ToShortTimeString(), days + " - " + StartTimePicker.SelectedDate.Value.ToShortTimeString() +
                        " - " + EndTimePicker.SelectedDate.Value.ToShortTimeString());
                    if (!TimeListBox.Items.Contains(item))
                        TimeListBox.Items.Add(item);
                }
                else
                {
                    TimeErrorLabel.Text = "Please select all days of the week this time corresponds to.";
                }
            //}
            //else
            //{
            //    TimeErrorLabel.Text = "Start time must be less than the end time.";
            //}
        }
        else
        {
            TimeErrorLabel.Text = "Please enter both the start time and the end time.";
        }
    }

    protected void RemoveTime(object sender, EventArgs e)
    {
        if (TimeListBox.SelectedItem != null)
            TimeListBox.Items.Remove(TimeListBox.SelectedItem);
    }

    protected void AddMonth(object sender, EventArgs e)
    {
        if (MonthDropDown.SelectedValue != null && StartDayDropDown.SelectedValue != null &&
            EndMonthDropDown.SelectedValue != null && EndDayDropDown.SelectedValue != null)
        {
            DateTime dtStart = DateTime.Parse(MonthDropDown.SelectedValue.ToString() + "/" + StartDayDropDown.SelectedValue + "/2011");
            DateTime dtEnd = DateTime.Parse(EndMonthDropDown.SelectedValue.ToString() + "/" + EndDayDropDown.SelectedValue + "/2011");

            if (dtStart < dtEnd)
            {
                ListItem item = new ListItem(MonthDropDown.SelectedItem.Text + ", " + StartDayDropDown.SelectedItem +
                    " - " + EndMonthDropDown.SelectedItem.Text + ", " + EndDayDropDown.SelectedItem.Text,
                    MonthDropDown.SelectedItem.Value + ", " + StartDayDropDown.SelectedItem +
                    " - " + EndMonthDropDown.SelectedItem.Value + ", " + EndDayDropDown.SelectedItem.Text);
                if (!MonthsListBox.Items.Contains(item))
                    MonthsListBox.Items.Add(item);
            }
            else
            {
                MonthsErrorLabel.Text = "Starting time frame must be less than the ending time frame.";
            }
        }
        else
        {
            MonthsErrorLabel.Text = "Select all the fields: start month, start day, end month and end day.";
        }

    }

    protected void RemoveMonth(object sender, EventArgs e)
    {
        if (MonthsListBox.SelectedItem != null)
            MonthsListBox.Items.Remove(MonthsListBox.SelectedItem);
    }

    protected void AddBring(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn);
        BringTextBox.Text = dat.stripHTML(BringTextBox.Text.Trim());

        if (BringTextBox.Text.Trim() != "")
        {
            ListItem item = new ListItem((BringListBox.Items.Count + 1).ToString() + ". " + BringTextBox.Text,
                BringTextBox.Text.Trim());
            if (!BringListBox.Items.Contains(item))
                BringListBox.Items.Add(item);
        }
    }

    protected void RemoveBring(object sender, EventArgs e)
    {
        if (BringListBox.SelectedItem != null)
            BringListBox.Items.Remove(BringListBox.SelectedItem);
    }

    protected string GetMajorCity()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn);
        char[] delim = {';'};
        char[] delter = { '-' };
        string[] locTokens;
        string[] addressToks;
        ListItem intem = DirectionsListBox.Items[0];
        
        string [] tokns = intem.Value.Split(delter);
        locTokens = tokns[0].Split(delim);
                  
        string country = locTokens[0];
        string zip = locTokens[3];

        int zipParam = 0;

        string zips = "";
        string MajorCityID = "";
        if (country == "223")
        {
            DataView dvAllZips = dat.GetDataDV("SELECT * FROM MajorZips MZ, MajorCities MC WHERE " +
                "MZ.MajorCityID=MC.ID AND MajorCityZip='" + zip + "'");

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
    //    Data dat = new Data(DateTime.Now);
    //    DataView dvChart = dat.GetDataDV("SELECT * FROM StandardTripPricing");

    //    char[] delim = { ';' };
    //    char[] delter = { '-' };

    //    string[] tokns = DirectionsListBox.Items[0].Value.Split(delter);
    //    string[] locTokens = tokns[0].Split(delim);

    //    string city = locTokens[2];
    //    string state = locTokens[1];
    //    string country = locTokens[0];
    //    string zips = "";
    //    string majorCityID = GetMajorCity();
    //    DataView dvCountMembers;
    //    if (country == "223")
    //    {
    //        //Get Major City
    //        zips = dat.GetDataDV("SELECT AllZips FROM MajorCities WHERE ID=" + majorCityID)[0]["AllZips"].ToString();

    //        dvCountMembers = dat.GetDataDV("SELECT COUNT(*) AS Count FROM UserPreferences WHERE MajorCity = " +
    //            majorCityID);
    //    }
    //    else
    //    {
    //        dvCountMembers = dat.GetDataDV("SELECT COUNT(*) AS Count FROM UserPreferences WHERE CatCity = '" +
    //            city + "' AND CatCountry = " + country +
    //            " AND CatState = '" + state + "'");
    //    }

    //    int indexToLookAt = 0;

    //    decimal memberCap = decimal.Round(decimal.Parse(dvChart[indexToLookAt]["PerMemberCap"].ToString()), 3, MidpointRounding.AwayFromZero);
    //    decimal priceForMembers = 0.00M;

    //    if (decimal.Parse(dvChart[indexToLookAt]["PerMemberPricing"].ToString()) * decimal.Parse(dvCountMembers[0]["Count"].ToString()) > memberCap)
    //    {
    //        priceForMembers = memberCap;
    //    }
    //    else
    //    {
    //        priceForMembers = decimal.Round(decimal.Parse(dvChart[indexToLookAt]["PerMemberPricing"].ToString()) *
    //            decimal.Parse(dvCountMembers[0]["Count"].ToString()), 3, MidpointRounding.AwayFromZero);
    //    }

    //    DataView dvCountEvents;
    //    DateTime dateDate;
    //    decimal eventCap = decimal.Round(decimal.Parse(dvChart[indexToLookAt]["PerTripCap"].ToString()), 2);
    //    decimal subtractionForEvents = 0.00M;
    //    decimal total = 0.00M;

    //    zips = zips.Replace("CatZip", "TD.Zip");

    //    dateDate = DateTime.Now;
    //    if (country == "223")
    //    {
    //        dvCountEvents = dat.GetDataDV("SELECT DISTINCT TripID FROM Trips T, TripDirections TD WHERE T.ID=TD.TripID AND T.Featured='True'  AND (" + zips +
    //                    ") AND T.DaysFeatured LIKE '%;" + dateDate.Month.ToString() + "/" +
    //        dateDate.Day.ToString() + "/" + dateDate.Year.ToString() + ";%'");
    //    }
    //    else
    //    {
    //        dvCountEvents = dat.GetDataDV("SELECT DISTINCT TripID FROM Trips T, TripDirections TD WHERE T.ID=TD.TripID AND T.Featured='True' AND TD.City = '" +
    //        city + "' AND TD.Country = " + country +
    //        " AND TD.State = '" + state + "' AND T.DaysFeatured LIKE '%;" + dateDate.Month.ToString() + "/" +
    //        dateDate.Day.ToString() + "/" + dateDate.Year.ToString() + ";%'");
    //    }

    //    if (decimal.Parse(dvChart[indexToLookAt]["PerTripPricing"].ToString()) *
    //        decimal.Parse(dvCountEvents.Count.ToString()) > eventCap)
    //    {
    //        subtractionForEvents = eventCap;
    //    }
    //    else
    //    {
    //        subtractionForEvents = decimal.Round(decimal.Parse(dvChart[indexToLookAt]["PerTripPricing"].ToString()) *
    //            decimal.Parse(dvCountEvents.Count.ToString()), 3, MidpointRounding.AwayFromZero);
    //    }

    //    total += decimal.Round((decimal.Parse(dvChart[indexToLookAt]["StandardTripPricing"].ToString()) +
    //        priceForMembers - subtractionForEvents), 3, MidpointRounding.AwayFromZero);

    //    string allTotal = decimal.Round(total, 2, MidpointRounding.AwayFromZero).ToString();
    //    PriceLiteral.Text = allTotal;
    //}
}

