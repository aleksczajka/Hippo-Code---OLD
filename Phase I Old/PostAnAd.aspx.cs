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
    
    private int UserID = 1;
    
    protected void Page_Load(object sender, EventArgs e)
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
            StartDateTimePicker.MinDate = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Subtract(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).TimeOfDay);
            Session["RedirectTo"] = Request.Url.AbsoluteUri;
            if (!IsPostBack)
            {
                Session["UserAvailableDate"] = null;
                Session.Remove("UserAvailableDate");
                Session["RedirectTo"] = "PostAnAd.aspx";
                Session["categorySession"] = null;


            }

           

            if (Session["UserAvailableDate"] != null)
            {
                bool isBig = false;
                if (AdPlacementList.SelectedValue == "0.04")
                    isBig = true;
                //GetAvailableUsers((DateTime)Session["UserAvailableDate"], isBig);
            }

            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            DataView dv = dat.GetDataDV("SELECT * FROM TermsAndConditions");
            Literal lit1 = new Literal();
            lit1.Text = "<div style=\"padding-bottom: 20px;\">"+dv[0]["Content"].ToString()+"</div>";
            TACTextBox.Controls.Add(lit1);

            if (Session["User"] != null)
            {
                LoggedInPanel.Visible = true;
            }
            else
            {
                WelcomeLabel.Text = "HippoHappenings's delivers content for users by users. " +
                "You have all the power to post events, advertisements and venues. " +
                "Users are responsible for all posted content, so mind " +
                "what you post. Everyone has the power to flag content deemed expressly " +
                "offensive, illegal or corporate in nature. In order to make this possible, " +
                "and for us to maintain clean and manageable content, we require that you " +
                "<a class=\"AddLink\" href=\"Register.aspx\">create an account</a> with us. If you already have an account, <a class=\"AddLink\" href=\"UserLogin.aspx\">login</a> and post your ad!";
                //WelcomeLabel.Text = "On Hippo Happenings, the users have all the power to post event, ads and venues alike. In order to do so, and for us to maintain clean and manageable content,  "
                //    + " we require that you <a class=\"AddLink\" href=\"Register.aspx\">create an account</a> with us. <a class=\"AddLink\" href=\"UserLogin.aspx\">Log in</a> if you have an account already. " +
                //    "Having an account with us will allow you to do many other things as well. You'll will be able to add events to your calendar, communicate with others going to events, add your friends, filter content based on your preferences, " +
                //    " feature your ads throughout the site and much more. <br/><br/>So let's go <a class=\"AddLink\" style=\"font-size: 16px;\" href=\"Register.aspx\">Register!</a>";
            }


            try
            {
                if (!IsPostBack)
                {
                    SummaryTextBox.Attributes.Add("onkeyup", "CountChars()");
                    HtmlLink lk = new HtmlLink();
                    HtmlHead head = (HtmlHead)Page.Header;
                    lk.Attributes.Add("rel", "canonical");
                    lk.Href = "http://hippohappenings.com/PostAnAd.aspx";
                    head.Controls.AddAt(0, lk);

                    HtmlMeta kw = new HtmlMeta();
                    kw.Name = "keywords";
                    kw.Content = "post local ad in your neighborhood for your community interest groups";

                    head.Controls.AddAt(0, kw);

                    HtmlMeta hm = new HtmlMeta();
                    hm.Name = "description";
                    hm.Content = "Post local ads in your neighborhood which will be posted in your community and interest groups";

                    head.Controls.AddAt(0, hm);

                    lk = new HtmlLink();
                    lk.Href = "http://" + Request.Url.Authority + "/PostAnAd.aspx";
                    lk.Attributes.Add("rel", "bookmark");
                    head.Controls.AddAt(0, lk);

                    DataSet dsCountry = dat.GetData("SELECT * FROM Countries");
                    CountryDropDown.DataSource = dsCountry;
                    CountryDropDown.DataTextField = "country_name";
                    CountryDropDown.DataValueField = "country_id";
                    CountryDropDown.DataBind();

                    CountryDropDown.SelectedValue = "223";



                    DataSet ds = dat.GetData("SELECT * FROM State WHERE country_id=223");

                    StateDropDownPanel.Visible = true;
                    StateTextBoxPanel.Visible = false;
                    StateDropDown.DataSource = ds;
                    StateDropDown.DataTextField = "state_2_code";
                    StateDropDown.DataValueField = "state_id";
                    StateDropDown.DataBind();
                }

                MusicUploadButton.PostBackUrl = Request.Url.AbsoluteUri;
                ImageButton1.PostBackUrl = Request.Url.AbsoluteUri;
                ImageButton10.PostBackUrl = Request.Url.AbsoluteUri;
                ImageButton5.PostBackUrl = Request.Url.AbsoluteUri;



                if (Session["User"] != null)
                {

                    Session["UserName"] = dat.GetData("SELECT UserName FROM Users WHERE User_ID=" + Session["User"].ToString()).Tables[0].Rows[0]["UserName"].ToString();
                    //Session["EffectiveUserName"] = Session["UserName"].ToString();
                    LoggedInPanel.Visible = true;
                    LogInPanel.Visible = false;
                    if (Request.QueryString["edit"] != null)
                    {
                        if (bool.Parse(Request.QueryString["edit"].ToString()))
                        {
                            string IDi = Request.QueryString["ID"].ToString();
                            if (Request.Cookies["editAd" + IDi] != null)
                            {
                                if (bool.Parse(Request.Cookies["editAd" + IDi].Value))
                                {
                                    DataSet dsAd = dat.GetData("SELECT * FROM Ads WHERE Ad_ID=" + Request.QueryString["ID"].ToString());

                                    if (Session["User"].ToString() == dsAd.Tables[0].Rows[0]["User_ID"].ToString())
                                    {

                                        isEdit.Text = "True";
                                        adID.Text = IDi;
                                        if (!IsPostBack)
                                        {
                                            fillAd(IDi, true);
                                        }

                                        IntroPanel.Visible = false;
                                        TabsPanel.Visible = true;
                                    }
                                    else
                                    {
                                        Response.Redirect("Home.aspx");
                                    }
                                }
                                else
                                {
                                    Response.Redirect("Home.aspx");
                                }
                            }
                            else
                            {
                                Response.Redirect("Home.aspx");
                            }


                        }
                        else
                        {
                            Response.Redirect("Home.aspx");
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
                                    StartDateTimePicker.DbSelectedDate = null;
                                    DaysDropDown.ClearSelection();
                                }

                                IntroPanel.Visible = false;
                                TabsPanel.Visible = true;
                            }
                            else
                            {
                                Response.Redirect("Home.aspx");
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
                //Session.Abandon();
                //FormsAuthentication.SignOut();
                //Response.Redirect("~/UserLogin.aspx");
            }

            //YourMessagesLabel.Text = "";
            //MessagePanel.Visible = false;

            if (!IsPostBack)
            {
                Session.Remove("AdCategoriesSet");
                Session["AdCategoriesSet"] = null;
                //DataSet ds = dat.GetData("SELECT * FROM Categories");


                //CategoriesCheckBoxes.DataSource = ds;
                //CategoriesCheckBoxes.DataTextField = "CategoryName";
                //CategoriesCheckBoxes.DataValueField = "ID";
                //CategoriesCheckBoxes.DataBind();

                ShowVideoPictureLiteral.Text = "";


                //Button adsLink = (Button)dat.FindControlRecursive(this, "AdsLink");
                //adsLink.CssClass = "NavBarImageAdSelected";

                DataView dvAdControl = dat.GetDataDV("SELECT * FROM AdControl");
                if (bool.Parse(dvAdControl[0]["areAdsFree"].ToString()))
                {
                    FreeLabel.Text = "Choose how many users you want to see your ad. " +
                        "<span style='color: #ff770d;'>[As the ads are now free, your limit on the number of users you can choose is " +
                        dvAdControl[0]["userLimit"].ToString() + ".] </span><br/>Your price will be calulated based on the number of " +
                "users you want your ad to be displayed to. Your ad will stop running " +
                "when that particular number of users have seen your ad. The price for " +
                "each user is $0.01, since you have chosen the 'Normal Ad Space'. " +
                "For example, if you choose 1000 users, your cost will be $10.";
                    FreeValidator.MaximumValue = dvAdControl[0]["userLimit"].ToString();
                    FreeFeaturedAdsPanel.Visible = true;
                    //FreeFeaturedAdPanel2.Visible = false;
                    FreeFeaturedAdPanel3.Visible = true;
                    FreeFeaturedAdPanel4.Visible = true;
                    AdPlacementList.Items[0].Text = "Normal Ad  <span style='color: #ff770d;'>[usually $0.01/user, today it is free. Limit of users is " + dvAdControl[0]["userLimit"].ToString() + "]</span>";
                    AdPlacementList.Items[1].Text = "Big Ad  <span style='color: #ff770d;'>[usually $0.04/user, today it is free. Limit of users is " + dvAdControl[0]["bigUserLimit"].ToString() + "]</span>";
                    FreeValidator.Visible = true;
                }
                else
                {
                    FreeValidator.Visible = false;
                    FreeLabel.Text = "Choose how many users you want to see your ad.<br/>Your price will be calulated based on the number of " +
                "users you want your ad to be displayed to. Your ad will stop running " +
                "when that particular number of users have seen your ad. The price for " +
                "each user is $0.01, since you have chosen the 'Normal Ad Space'. " +
                "For example, if you choose 1000 users, your cost will be $10. <br/>We recommend you start with a small number (ex: 100) and monitor whether your ad has been viewed in your Ad Statistics page found from your My Account page. If you find your ad views are quickly runnying out, you can then purchase more views.";
                }
            }
        }
        catch (Exception ex)
        {
            YourMessagesLabel.Text = ex.ToString();
            MessagePanel.Visible = true;
        }

        
    }
    
    protected void Page_Init(object sender, EventArgs e)
    {
        
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
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            DataSet dsEvent = dat.GetData("SELECT * FROM Ads WHERE Ad_ID=" + ID);
            DataSet dsCalendar = dat.GetData("SELECT * FROM Ad_Calendar WHERE AdID=" + ID);
            AdNameTextBox.THE_TEXT = dsEvent.Tables[0].Rows[0]["Header"].ToString();
            nameLabel.Text = "You are submitting changes for Ad: " + dsEvent.Tables[0].Rows[0]["Header"].ToString(); ;
            nameLabel.Visible = false;
            //Session["EffectiveUserName"] = Session["UserName"].ToString();

            DataSet dsCountry = dat.GetData("SELECT * FROM State WHERE country_id=" + dsEvent.Tables[0].Rows[0]["CatCountry"].ToString());

            isFeatured.Text = dsEvent.Tables[0].Rows[0]["Featured"].ToString();


            if (DateTime.Parse(dsCalendar.Tables[0].Rows[0]["DateTimeStart"].ToString()) < StartDateTimePicker.MinDate)
                StartDateTimePicker.DbSelectedDate = StartDateTimePicker.MinDate;
            else
                StartDateTimePicker.DbSelectedDate = DateTime.Parse(dsCalendar.Tables[0].Rows[0]["DateTimeStart"].ToString());


            AllLocationCheckBox.Checked = bool.Parse(dsEvent.Tables[0].Rows[0]["NonUsersAllowed"].ToString());
            

            TimeSpan abc = DateTime.Parse(dsCalendar.Tables[0].Rows[0]["DateTimeEnd"].ToString()).Subtract(DateTime.Parse(dsCalendar.Tables[0].Rows[0]["DateTimeStart"].ToString()));

            DaysDropDown.SelectedValue = abc.Days.ToString();

            CountryDropDown.SelectedValue = dsEvent.Tables[0].Rows[0]["CatCountry"].ToString();


            bool isTextBox = false;

            if (dsEvent.Tables[0].Rows[0]["CatState"] != null)
            {

                DataSet dsState = dat.GetData("SELECT * FROM State WHERE country_id=" + dsEvent.Tables[0].Rows[0]["CatCountry"].ToString());

                if (dsState.Tables.Count > 0)
                    if (dsState.Tables[0].Rows.Count > 0)
                    {
                        StateDropDownPanel.Visible = true;
                        StateTextBoxPanel.Visible = false;
                        StateDropDown.DataSource = dsState;
                        StateDropDown.DataTextField = "state_2_code";
                        StateDropDown.DataValueField = "state_id";
                        StateDropDown.DataBind();

                        StateDropDown.Items.FindByText(dsEvent.Tables[0].Rows[0]["CatState"].ToString()).Selected = true;
                    }
                    else
                        isTextBox = true;
                else
                    isTextBox = true;

                if (isTextBox)
                {
                    StateTextBoxPanel.Visible = true;
                    StateDropDownPanel.Visible = false;
                    StateTextBox.THE_TEXT = dsEvent.Tables[0].Rows[0]["CatState"].ToString();

                }
            }

            if (dsEvent.Tables[0].Rows[0]["CatCity"] != null)
            {
                CityTextBox.Text = dsEvent.Tables[0].Rows[0]["CatCity"].ToString();
            }

            if (dsEvent.Tables[0].Rows[0]["CatZip"] != null)
            {
                if (dsEvent.Tables[0].Rows[0]["CatZip"].ToString().Trim() != "")
                {
                    char[] delim2 = { ';' };
                    string[] tokens2 = dsEvent.Tables[0].Rows[0]["CatZip"].ToString().Split(delim2);
                    ZipTextBox.Text = tokens2[1];
                }
            }

            if (dsEvent.Tables[0].Rows[0]["Radius"] != null)
            {
                RadiusDropDown.SelectedValue = dsEvent.Tables[0].Rows[0]["Radius"].ToString();
            }

            DescriptionTextBox.Content = dsEvent.Tables[0].Rows[0]["Description"].ToString();

            

            if (bool.Parse(dsEvent.Tables[0].Rows[0]["DisplayToAll"].ToString()))
                DisplayCheckList.SelectedValue = "1";
            else
                DisplayCheckList.SelectedValue = "2";

            if (bool.Parse(dsEvent.Tables[0].Rows[0]["Featured"].ToString()))
            {
                
                if(!iscopy)
                    DateCannotEditLabel.Text = "You cannot edit the date for a featured ad.<br/>";
                
                if(isedit && !iscopy)
                    StartDateTimePicker.Enabled = false;
                SummaryTextBox.InnerHtml = dsEvent.Tables[0].Rows[0]["FeaturedSummary"].ToString();

                SelectFeatured();

                AdMediaPanel.Enabled = true;
                AdMediaPanel.Visible = true;
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
            else
            {
                SelectFree();
            }

            //InfoPanelFeaturedLabel.Visible = false;
            //InfoPanelFreeLabel.Visible = false;

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
                    YouTubePanel.Enabled = true;
                    PicturePanel.Enabled = true;
                    PictureCheckList.Enabled = true;
                    PictureNixItButton.Visible = true;
                    break;
                default: break;
            }

            DataSet dsMusic = dat.GetData("SELECT * FROM Ad_Song_Mapping WHERE AdID=" + dsEvent.Tables[0].Rows[0]["Ad_ID"].ToString());
            SongCheckList.Items.Clear();
            if (dsMusic.Tables.Count > 0)
                if (dsMusic.Tables[0].Rows.Count > 0)
                {
                    MusicPanel.Enabled = true;
                    for (int i = 0; i < dsMusic.Tables[0].Rows.Count; i++)
                    {
                        SongCheckList.Enabled = true;
                        DeleteSongButton.Visible = true;
                        MusicCheckBox.Checked = true;
                        ListItem newItem = new ListItem(dsMusic.Tables[0].Rows[i]["SongTitle"].ToString(),
                            dsMusic.Tables[0].Rows[i]["SongName"].ToString());

                        SongCheckList.Items.Add(newItem);
                    }
                }

            DataSet dsCategories = dat.GetData("SELECT * FROM Ad_Category_Mapping WHERE AdID=" + ID);

            if (dsCategories.Tables.Count > 0)
                if (dsCategories.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsCategories.Tables[0].Rows.Count; i++)
                    {
                        Telerik.Web.UI.RadTreeNode node =
                            (Telerik.Web.UI.RadTreeNode)CategoryTree.Nodes.FindNodeByValue(dsCategories.Tables[0].Rows[i]["CategoryID"].ToString());
                        if (node != null)
                        {
                            node.Checked = true;
                        }
                        else
                        {

                            node = (Telerik.Web.UI.RadTreeNode)RadTreeView1.Nodes.FindNodeByValue(dsCategories.Tables[0].Rows[i]["CategoryID"].ToString());
                            if (node != null)
                            {
                                node.Checked = true;
                            }
                            else
                            {

                                node = (Telerik.Web.UI.RadTreeNode)RadTreeView2.Nodes.FindNodeByValue(dsCategories.Tables[0].Rows[i]["CategoryID"].ToString());
                                if (node != null)
                                {
                                    node.Checked = true;
                                }
                                else
                                {
                                    node = (Telerik.Web.UI.RadTreeNode)RadTreeView3.Nodes.FindNodeByValue(dsCategories.Tables[0].Rows[i]["CategoryID"].ToString());
                                    if (node != null)
                                    {
                                        node.Checked = true;
                                    }

                                }

                            }
                        }
                        //CategoriesCheckBoxes.Items.FindByValue(dsCategories.Tables[0].Rows[i]["CategoryID"].ToString()).Selected = true;
                        //CategoriesCheckBoxes.Items.FindByValue(dsCategories.Tables[0].Rows[i]["CategoryID"].ToString()).Enabled = false;
                    }
                }


            if (!iscopy)
            {
                PaidForPanel.Visible = true;
                AdPlacementList.Enabled = false;
                NotEditLabel.Text = "You are not allowed to edit this part.";
                NumberOfPaidUsers.Text = dsEvent.Tables[0].Rows[0]["NumViews"].ToString() + "<br/>";
            }
            DataView dvAdControl = dat.GetDataDV("SELECT * FROM AdControl");

            if (bool.Parse(dsEvent.Tables[0].Rows[0]["Featured"].ToString()))
            {
                if (bool.Parse(dsEvent.Tables[0].Rows[0]["BigAd"].ToString()))
                {
                    BigPanel.Visible = true;
                    SmallPanel.Visible = false;

                    RadioButtonList1.SelectedValue = dsEvent.Tables[0].Rows[0]["Template"].ToString();

                    AdPlacementList.SelectedValue = "0.04";

                    RadToolTip2.Text = "<label>Your price will be calulated based on the number of " +
                    "users you want your ad to be displayed to. Your ad will stop running " +
                    "when that particular number of users have seen your ad. The price for " +
                    "each user is $0.04, since you have chosen the 'Big Ad Space'. " +
                    "For example, if you choose 1000 users, your cost will be $40.</label>";
                    if (bool.Parse(dvAdControl[0]["areAdsFree"].ToString()))
                    {
                        FreeLabel.Text = "Choose how many users you want to see your ad. " +
                            " <span style='color: #ff770d;'>[As the ads are now free, your limit on the " +
                            "number of users you can choose is " +
                            dvAdControl[0]["bigUserLimit"].ToString() + ".]</span>Your price will be calulated based on the number of " +
                    "users you want your ad to be displayed to. Your ad will stop running " +
                    "when that particular number of users have seen your ad. The price for " +
                    "each user is $0.04, since you have chosen the 'Big Ad Space'. " +
                    "For example, if you choose 1000 users, your cost will be $40.";
                        FreeValidator.MaximumValue = dvAdControl[0]["bigUserLimit"].ToString();
                        FreeValidator.ErrorMessage = "User count can only be between 0 and " +
                            dvAdControl[0]["bigUserLimit"].ToString() + ".";
                    }
                    else
                    {
                        CalcPrice();
                        //GetAvailableUsers((DateTime)Session["SelectedStartDate"], true);
                    }

                }
                else
                {
                    BigPanel.Visible = false;
                    SmallPanel.Visible = true;

                    TemplateRadioList.SelectedValue = dsEvent.Tables[0].Rows[0]["Template"].ToString();


                    AdPlacementList.SelectedValue = "0.01";
                    RadToolTip2.Text = "<label>Your price will be calulated based on the number of " +
                        "users you want your ad to be displayed to. Your ad will stop running " +
                        "when that particular number of users have seen your ad. The price for " +
                        "each user is $0.01, since you have chosen a 'Normal Ad Space'. " +
                        "For example, if you choose 1000 users, your cost will be $10.</label>";

                    if (bool.Parse(dvAdControl[0]["areAdsFree"].ToString()))
                    {
                        FreeLabel.Text = "Choose how many users you want to see your ad. " +
                            " <span style='color: #ff770d;'>[As the ads are now free, your limit on the number of users you can choose is " +
                            dvAdControl[0]["userLimit"].ToString() + ".]</span> <br/>Your price will be calulated based on the number of " +
                        "users you want your ad to be displayed to. Your ad will stop running " +
                        "when that particular number of users have seen your ad. The price for " +
                        "each user is $0.01, since you have chosen a 'Normal Ad Space'. " +
                        "For example, if you choose 1000 users, your cost will be $10.";
                        Image6.Visible = true;
                        FreeValidator.MaximumValue = dvAdControl[0]["userLimit"].ToString();
                        FreeValidator.ErrorMessage = "User count can only be between 0 and " +
                            dvAdControl[0]["userLimit"].ToString() + ".";
                    }
                    else
                    {
                        Image6.Visible = false;
                        CalcPrice();
                        //GetAvailableUsers((DateTime)Session["SelectedStartDate"], false);

                    }
                }
                ChangeFeaturedText(RadioButtonList1, new EventArgs());
            }
            if (bool.Parse(dvAdControl[0]["areAdsFree"].ToString()))
            {
                if (!iscopy)
                {
                    UserNumberPanel.Enabled = false;
                    UserNumberLabel.Text = "<label>Since featured ads are now free. Allowed user count is restricted.</label>";
                }
                FreeValidator.MaximumValue = dvAdControl[0]["userLimit"].ToString();
                FreeValidator.ErrorMessage = "User count can only be between 0 and " +
                    dvAdControl[0]["userLimit"].ToString() + ".";
            }
            else
            {
                UserNumberPanel.Enabled = true;
                UserNumberLabel.Text = "";
            }
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

            if (selectedIndex.ToString() == "6")
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
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        try
        {
            YourMessagesLabel.Text = "";
            MessagePanel.Visible = false;
            
            Session["PrevTab"] = selectedIndex;
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            switch (selectedIndex)
            {
                case 0:
                    #region First Tab
                    DescriptionTextBox.Content = dat.StripHTML_LeaveLinks(DescriptionTextBox.Content.Replace("<div>", "<br/>").Replace("</div>", ""));
                    AdNameTextBox.THE_TEXT = dat.stripHTML(AdNameTextBox.THE_TEXT);
                    if (AdNameTextBox.THE_TEXT.Trim().Length != 0)
                    {
                        if (DescriptionTextBox.Text.Length <= 1000)
                        {
                            if (StartDateTimePicker.DbSelectedDate != null)
                            {
                                if ((DateTime)StartDateTimePicker.DbSelectedDate < DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Date)
                                {
                                    YourMessagesLabel.Text += "<br/><br/>*The start date cannot be before today.";
                                    MessagePanel.Visible = true;
                                    AdTabStrip.SelectedIndex = 0;
                                    AdPostPages.PageViews[0].Selected = true;
                                    return false;
                                }
                                else
                                {
                                    if(changeTab)
                                        ChangeSelectedTab(0, 1);
                                    MessagePanel.Visible = false;
                                    FillLiteral();
                                    return true;
                                }

                            }
                            else
                            {
                                YourMessagesLabel.Text += "<br/><br/>*All date fields are required.";
                                MessagePanel.Visible = true;
                                AdTabStrip.SelectedIndex = 0;
                                AdPostPages.PageViews[0].Selected = true;
                                return false;
                            }
                        }
                        else
                        {
                            YourMessagesLabel.Text += "<br/><br/>*The description must be less than or equal to 1000 characters.";
                            MessagePanel.Visible = true;
                            AdTabStrip.SelectedIndex = 0;
                            AdPostPages.PageViews[0].Selected = true;
                            return false;
                        }
                    }
                    else
                    {
                        MessagePanel.Visible = true;
                        YourMessagesLabel.Text += "<br/><br/>*Must include a Headline.";
                        if (DescriptionTextBox.Content.Length < 100)
                            YourMessagesLabel.Text += "<br/><br/>*Make sure you say what you want your viewers to hear about your advertisment! The description must be at least 100 characters.";
                        AdTabStrip.SelectedIndex = 0;
                        AdPostPages.PageViews[0].Selected = true;
                        return false;
                    }

                    return false;
                    #endregion
                    break;
                case 1:
                    if (bool.Parse(isFeatured.Text))
                    {
                        FeaturedAdMediaPanel.Visible = true;
                    }
                    else
                    {
                        FeaturedAdMediaPanel.Visible = false;
                    }
                    if(changeTab)
                        ChangeSelectedTab(1, 2);
                    MessagePanel.Visible = false;
                    FillLiteral();
                    return true;
                    break;
                case 2:
                    #region Third Tab
                    bool goOn = false;
                    if (bool.Parse(isFeatured.Text))
                    {
                        SummaryTextBox.InnerHtml = dat.StripHTML_LeaveLinksNoBr(SummaryTextBox.InnerHtml);
                    }
                        bool isEditing = false;
                        if (isEdit.Text.Trim() != "")
                            if (bool.Parse(isEdit.Text))
                                isEditing = true;
                        if (isEditing && Session["categorySession"] == null)
                        {
                            Session["categorySession"] = true;
                            DataView dsCategories = dat.GetDataDV("SELECT * FROM Ad_Category_Mapping WHERE AdID=" +adID.Text);

                            List<Telerik.Web.UI.RadTreeNode> list = (List<Telerik.Web.UI.RadTreeNode>)CategoryTree.GetAllNodes();
                            List<Telerik.Web.UI.RadTreeNode> list2 = (List<Telerik.Web.UI.RadTreeNode>)RadTreeView1.GetAllNodes();
                            List<Telerik.Web.UI.RadTreeNode> list3 = (List<Telerik.Web.UI.RadTreeNode>)RadTreeView2.GetAllNodes();
                            List<Telerik.Web.UI.RadTreeNode> list4 = (List<Telerik.Web.UI.RadTreeNode>)RadTreeView3.GetAllNodes();

                            if (dsCategories.Count > 0)
                            {
                                for (int i = 0; i < list.Count; i++)
                                {
                                    dsCategories.RowFilter = "CategoryID=" + list[i].Value;

                                    if (dsCategories.Count > 0)
                                        list[i].Checked = true;

                                }

                                for (int i = 0; i < list2.Count; i++)
                                {
                                    dsCategories.RowFilter = "CategoryID=" + list2[i].Value;

                                    if (dsCategories.Count > 0)
                                        list2[i].Checked = true;

                                }

                                for (int i = 0; i < list4.Count; i++)
                                {
                                    dsCategories.RowFilter = "CategoryID=" + list4[i].Value;

                                    if (dsCategories.Count > 0)
                                        list4[i].Checked = true;

                                }

                                for (int i = 0; i < list3.Count; i++)
                                {
                                    dsCategories.RowFilter = "CategoryID=" + list3[i].Value;

                                    if (dsCategories.Count > 0)
                                        list3[i].Checked = true;

                                }

                                
                            }
                        }
                        if (bool.Parse(isFeatured.Text))
                        {
                            if (FeaturedTextPanel.Visible)
                            {
                                if (AdPlacementList.SelectedValue == "0.01")
                                {
                                    if (TemplateRadioList.SelectedValue == "1")
                                    {
                                        if (SummaryTextBox.InnerHtml.Trim().Length > 250 || SummaryTextBox.InnerHtml.Trim().Length == 0)
                                        {
                                            YourMessagesLabel.Text += "<br/><br/>*Must include the summary for a featured ad. It must be less than 250 characters.";
                                            MessagePanel.Visible = true;
                                            AdTabStrip.SelectedIndex = 2;
                                            AdPostPages.PageViews[2].Selected = true;
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
                                            YourMessagesLabel.Text += "<br/><br/>*Must include the summary for a featured ad. It must be less than 100 characters.";
                                            MessagePanel.Visible = true;
                                            AdTabStrip.SelectedIndex = 2;
                                            AdPostPages.PageViews[2].Selected = true;
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
                                    if (SummaryTextBox.InnerHtml.Trim().Length > 250 || SummaryTextBox.InnerHtml.Trim().Length == 0)
                                    {
                                        YourMessagesLabel.Text += "<br/><br/>*Must include the summary for a featured ad. It must be less than 250 characters.";
                                        MessagePanel.Visible = true;
                                        AdTabStrip.SelectedIndex = 2;
                                        AdPostPages.PageViews[2].Selected = true;
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
                                goOn = true;
                            }

                            if (goOn)
                            {
                                if (CityTextBox.Text.Trim() != "")
                                {
                                    if (dat.TrapKey(CityTextBox.Text, 1))
                                    {
                                        if (BannerAdCheckBox.Checked && AdPictureCheckList.Items.Count == 0)
                                        {
                                            YourMessagesLabel.Text += "<br/><br/>*Please include the banner image or un-check the 'Include a Banner Image' check box.";
                                            MessagePanel.Visible = true;
                                            AdTabStrip.SelectedIndex = 2;
                                            AdPostPages.PageViews[2].Selected = true;
                                            return false;
                                        }
                                        else
                                        {
                                            goOn = true;
                                        }
                                    }
                                    else
                                    {
                                        YourMessagesLabel.Text += "<br/><br/>*City can only contain letters. No special characters allowed.";
                                        MessagePanel.Visible = true;
                                        AdTabStrip.SelectedIndex = 2;
                                        AdPostPages.PageViews[2].Selected = true;
                                        return false;
                                    }
                                }
                                else
                                {
                                    YourMessagesLabel.Text += "<br/><br/>*Must include the city.";
                                    MessagePanel.Visible = true;
                                    AdTabStrip.SelectedIndex = 2;
                                    AdPostPages.PageViews[2].Selected = true;
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            if (CityTextBox.Text.Trim() != "")
                            {
                                goOn = true;
                            }
                            else
                            {
                                YourMessagesLabel.Text += "<br/><br/>*Must include the city.";
                                MessagePanel.Visible = true;
                                AdTabStrip.SelectedIndex = 2;
                                AdPostPages.PageViews[2].Selected = true;
                                return false;
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
                                ChangeSelectedTab(2, 3);

                            FillLiteral();
                            ChangePrice(AdPlacementList, new EventArgs());
                            return true;
                        }
                        return false;
                    #endregion
                    break;
                case 3:
                    #region Fourth Tab
                    
                    if (CategorySelected())
                    {
                        if (bool.Parse(isFeatured.Text))
                        {
                            //CategoriesLiteral.Text = "";
                            //Get the location string
                            //string location = " AND UP.CatCountry=" + CountryDropDown.SelectedValue;

                            Session["UserAvailableDate"] = (DateTime)StartDateTimePicker.DbSelectedDate;
                            Session["SelectedStartDate"] = ((DateTime)StartDateTimePicker.DbSelectedDate).ToShortDateString();

                            bool isBig = false;
                            if (AdPlacementList.SelectedValue == "0.04")
                                isBig = true;

                            //GetAvailableUsers((DateTime)StartDateTimePicker.DbSelectedDate, isBig);

                            //Get users available today to email
                            int firstDayCountEmail = 0;
                            DataView dvUsersIDs = GetSavedSearchesUsers(ref firstDayCountEmail, false);



                            string notTheseUsers = " UP.UserID <> " + Session["User"].ToString();

                            if (dvUsersIDs.Count > 0)
                                notTheseUsers += " AND ";

                            for (int i = 0; i < dvUsersIDs.Count; i++)
                            {
                                notTheseUsers += " UP.UserID <> " + dvUsersIDs[i]["UserID"].ToString();
                                if (i != dvUsersIDs.Count - 1)
                                {
                                    notTheseUsers += " AND ";
                                }
                            }
                            string notUsers = notTheseUsers;
                            if (notTheseUsers.Trim() != "")
                                notTheseUsers = " AND (" + notTheseUsers + ")";

                            if (firstDayCountEmail > 0)
                            {
                                EmailUsersLabel.Text = firstDayCountEmail.ToString();
                                EmailUsersPanel.Visible = true;
                            }
                            else
                            {
                                EmailUsersPanel.Visible = false;
                            }

                            //MessagePanel.Visible = true;
                            //YourMessagesLabel.Text = Session["command"].ToString();

                            //Get all users that fall under the category and location
                            string catString = GetCheckedCategoriesString();

                            string state = StateTextBox.THE_TEXT.ToLower().Trim();
                            if (StateDropDown.Visible)
                            {
                                state = StateDropDown.SelectedItem.Text;
                            }

                            string zip = "";
                            
                            if(ZipTextBox.Text.Trim() != "")
                                zip = GetAllZipsInRadius(true, true);

                            if (zip == null)
                                zip = "";
                            zip = zip.Replace("AND", "");

                            if (zip.Trim() != "")
                                zip = "AND ((" + zip + ") OR (CatZip is NULL OR CatZip = ''))";
                            string str = "SELECT DISTINCT UP.UserID FROM UserPreferences UP, " +
                                "UserCategories UC WHERE UP.CatCountry=" + CountryDropDown.SelectedValue +
                                " AND UP.CatCity='" +
                                CityTextBox.Text.ToLower().Trim() +
                                "' AND UP.CatState='" + state +
                                "' "+zip+"  AND " +
                                "UC.UserID=UP.UserID AND (" + catString + ") " + notTheseUsers;

                            //MessagePanel.Visible = true;
                            //YourMessagesLabel.Text = str;

                            DataView dvUsers = dat.GetDataDV(str);
                            NumTotalUsers.Text = dvUsers.Count.ToString();

                            //Now get all users who might see your ad if 
                            //they have no other ads to see
                            //because they are in the same location
                            //but not in your categories.

                            if (notUsers.Trim() != "" && dvUsers.Count != 0)
                                notUsers += " AND ";
                            for (int i = 0; i < dvUsers.Count; i++)
                            {
                                notUsers += " UP.UserID <> " + dvUsers[i]["UserID"].ToString();
                                if (i != dvUsers.Count - 1)
                                {
                                    notUsers += " AND ";
                                }
                            }

                            if (notUsers.Trim() != "")
                                notUsers = " AND (" + notUsers + ")";

                            str = "SELECT DISTINCT UP.UserID FROM UserPreferences UP WHERE UP.CatCountry=" +
                                CountryDropDown.SelectedValue +
                                " AND UP.CatCity='" +
                                CityTextBox.Text.ToLower().Trim() +
                                "' AND UP.CatState='" + state +
                                "' " + zip + notUsers;

                            

                            DataView dvLocationUsers = dat.GetDataDV(str);
                            LocationUserLabel.Text = dvLocationUsers.Count.ToString();

                            if (changeTab)
                                ChangeSelectedTab(3, 4);

                            FillLiteral();
                            return true;
                        }
                        else
                        {
                            FillLiteral();

                            if(changeTab)
                                ChangeSelectedTab(3, 6);

                            return true;
                        }
                    }
                    else
                    {
                        MessagePanel.Visible = true;
                        YourMessagesLabel.Text = "Must include at least one category.";
                        AdTabStrip.SelectedIndex = 3;
                        AdPostPages.PageViews[3].Selected = true;

                        return false;
                    }

                    return false;
                    #endregion
                    break;
                case 4:
                    #region Fifth Tab
                    YourMessagesLabel.Text = "";
                    MessagePanel.Visible = false;
                    bool isEdit2 = false;

                    if (Request.QueryString["edit"] != null)
                    {
                        isEdit2 = bool.Parse(Request.QueryString["edit"].ToString());
                    }
                    if (UsersTextBox.Text != "")
                    {

                        int users = 0;
                        try
                        {
                            users = int.Parse(UsersTextBox.Text);
                            if (users != 0 || isEdit2)
                            {
                                if (TotalUsers.Text == "0" & DisplayCheckList.SelectedValue == "2")
                                {
                                    MessagePanel.Visible = true;
                                    YourMessagesLabel.Text += "<br/><br/>*Since the category and location grouping you have chosen returned ZERO users, you must select to 'Display to non-categories'. Otherwise, no one will see your ad.";
                                    AdTabStrip.SelectedIndex = 4;
                                    AdPostPages.PageViews[4].Selected = true;
                                    return false;
                                }
                                else
                                {
                                    MessagePanel.Visible = false;

                                    if(changeTab)
                                        ChangeSelectedTab(4, 5);

                                    FillLiteral();
                                    return true;
                                }
                            }
                            else
                            {
                                MessagePanel.Visible = true;
                                YourMessagesLabel.Text += "<br/><br/>*The number of users you post your ad to must be greater than 0.";
                                AdTabStrip.SelectedIndex = 4;
                                AdPostPages.PageViews[4].Selected = true;
                                return false;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessagePanel.Visible = true;
                            YourMessagesLabel.Text += "<br/><br/>*The number of users must be a whole number";
                            YourMessagesLabel.Text += ex.ToString();
                            AdTabStrip.SelectedIndex = 4;
                            AdPostPages.PageViews[4].Selected = true;
                            return false;
                        }
                    }
                    else
                    {
                        MessagePanel.Visible = true;
                        YourMessagesLabel.Text += "<br/><br/>*Must include the number of users.";
                        AdTabStrip.SelectedIndex = 4;
                        AdPostPages.PageViews[4].Selected = true;
                        return false;
                    }

                    return false;
                    #endregion
                    break;
                case 5:
                    MessagePanel.Visible = false;
                    FillLiteral();

                    if(changeTab)
                        ChangeSelectedTab(5, 6);
                    return true;
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

    protected string GetAllZipsInRadius(bool likes, bool isCat)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        //do only if United States and not for international zip codes
        string zip = "";
        string nonExistantZip = "";

        string cat = "";
        if (isCat)
            cat = "Cat";

        if (ZipTextBox.Text.Trim() != "")
        {
            if (RadiusPanel.Visible)
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
                        zip = " AND UP." + cat + "Zip = '" + zipParam.ToString() + "' ";
                        if (likes)
                            zip = " AND UP." + cat + "Zip LIKE '%;" + zipParam.ToString() + ";%' ";
                        nonExistantZip = " OR UP." + cat + "Zip = '" + zipParam.ToString() + "'";
                        if (likes)
                            nonExistantZip = " OR UP." + cat + "Zip LIKE '%;" + zipParam.ToString() + ";%' ";
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
                            zip = " AND (UP." + cat + "Zip = '" + zipParam.ToString() + "' " + nonExistantZip;
                            if (likes)
                                zip = " AND (UP." + cat + "Zip LIKE '%;" + zipParam.ToString() + ";%' " + nonExistantZip;
                            for (int i = 0; i < dsZips.Tables[0].Rows.Count; i++)
                            {
                                if (likes)
                                    zip += " OR UP." + cat + "Zip LIKE '%;" + dsZips.Tables[0].Rows[i]["Zip"].ToString() + ";%' ";
                                else
                                    zip += " OR UP." + cat + "Zip = '" + dsZips.Tables[0].Rows[i]["Zip"].ToString() + "' ";
                            }
                            zip += ") ";
                        }
                        else
                        {
                            if (likes)
                                zip = " AND UP." + cat + "Zip LIKE '%;" + ZipTextBox.Text.Trim() + ";%' ";
                            else
                                zip = " AND UP." + cat + "Zip='" + ZipTextBox.Text.Trim() + "'";
                        }
                    }
                    else
                    {
                        if (likes)
                            zip = " AND UP." + cat + "Zip LIKE '%;" + ZipTextBox.Text.Trim() + ";%' ";
                        else
                            zip = " AND UP." + cat + "Zip='" + ZipTextBox.Text.Trim() + "'";
                    }


                }
                else
                {
                    if (likes)
                        zip = " AND UP." + cat + "Zip LIKE '%;" + ZipTextBox.Text.Trim() + ";%' ";
                    else
                        zip = " AND UP." + cat + "Zip = '" + ZipTextBox.Text.Trim() + "' ";
                }
            }
            else
            {
                if (likes)
                    zip = " AND UP." + cat + "Zip LIKE '%;" + ZipTextBox.Text.Trim() + ";%' ";
                else
                    zip = " AND UP." + cat + "Zip = '" + ZipTextBox.Text.Trim() + "' ";
            }
        }
        else
        {
            zip = null;
        }

        return zip;
    }

    protected bool CategorySelected()
    {
        return OneCategorySelected(ref CategoryTree) || OneCategorySelected(ref RadTreeView1)
        || OneCategorySelected(ref RadTreeView2) || OneCategorySelected(ref RadTreeView3);
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
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        string state = "";
        if (StateDropDownPanel.Visible)
            state = StateDropDown.SelectedItem.Text;
        else
            state = StateTextBox.THE_TEXT;

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

        string zip = GetAllZipsInRadius(true, false);

        
        dat.returnCategoryIDString(CategoryTree, ref categories);
        dat.returnCategoryIDString(RadTreeView1, ref categories);
        dat.returnCategoryIDString(RadTreeView2, ref categories);
        dat.returnCategoryIDString(RadTreeView3, ref categories);
        string command = "";
         

        DataView dv = dat.CalculateUsersEmailAdCapacity(ref command, AdNameTextBox.THE_TEXT.ToLower(),
            DescriptionTextBox.Content.ToLower(), city, zip, state, CountryDropDown.SelectedItem.Value,
            categories, ref firstDayCountEmail, ref isNoUsers, isAll);

        Session["command"] = command;
        return dv;
    }

    //protected void GetAvailableUsers(DateTime startDate, bool isBig)
    //{
    //    CategoryDaysPanel.Controls.Clear();


    //    UpdatePanel Up = new UpdatePanel();
    //    Up.ChildrenAsTriggers = false;
    //    Up.ID = "Up";
    //    Up.UpdateMode = UpdatePanelUpdateMode.Conditional;

    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //    string txtLocation = CountryDropDown.SelectedItem.Text;
    //    string state = "";
    //    if (StateDropDownPanel.Visible)
    //        state = StateDropDown.SelectedItem.Text;
    //    else
    //        state = StateTextBox.THE_TEXT;

    //    int firstDayCount = 0;
    //    int secDayCount = 0;
    //    int thirdDayCount = 0;
    //    int fourthDayCount = 0;

    //    int firstDayCountLoc = 0;
    //    int secDayCountLoc = 0;
    //    int thirdDayCountLoc = 0;
    //    int fourthDayCountLoc = 0;


    //    TotalUsers.Text = firstDayCount.ToString();

    //    string city = "";
    //    if (CityTextBox.Text.Trim() != "")
    //    {
    //        city = CityTextBox.Text.Trim();
    //    }
    //    else
    //    {
    //        city = null;
    //    }

    //    bool isNoUsers = false;

    //    string categories = "";

    //    dat.returnCategoryIDString(CategoryTree, ref categories);
    //    dat.returnCategoryIDString(RadTreeView1, ref categories);
    //    dat.returnCategoryIDString(RadTreeView2, ref categories);
    //    dat.returnCategoryIDString(RadTreeView3, ref categories);

    //    dat.CalculateUsersAdCapacity(city, state, CountryDropDown.SelectedItem.Value, categories, startDate,
    //        ref firstDayCount, ref secDayCount, ref thirdDayCount, ref fourthDayCount, ref isNoUsers, isBig, false);
    //    dat.CalculateUsersAdCapacity(city, state, CountryDropDown.SelectedItem.Value, categories, startDate,
    //        ref firstDayCountLoc, ref secDayCountLoc, ref thirdDayCountLoc, ref fourthDayCountLoc, ref isNoUsers, 
    //        isBig, true);
        

    //    System.Drawing.Color green = System.Drawing.Color.FromArgb(98, 142, 2);

    //    string startDateReal = "";
    //    if (Session["SelectedStartDate"] == null)
    //        startDateReal = ((DateTime)StartDateTimePicker.DbSelectedDate).ToShortDateString();
    //    else
    //        startDateReal = Session["SelectedStartDate"].ToString();

    //    Literal lab = new Literal();
    //    lab.ID = "dateLiteral";
    //    lab.Text = "<div style=\"clear: both; float: right;\"><label>Selected Start Date: </label><span "+
    //        "class=\"AddGreenLinkBig\">" + startDateReal + "</span></div><div style=\"clear: both;\">";
        



    //    Panel topPanel = new Panel();
    //    topPanel.Width = 590;

    //    LinkButton link1 = new LinkButton();

    //    if (startDate <= DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Date)
    //    {
    //        link1.Enabled = false;
    //        link1.CssClass = "AddLinkNoPaddingDisabled";
    //    }
    //    else
    //    {
    //        link1.Enabled = true;
    //        link1.CssClass = "AddLinkNoPadding";
    //    }
    //    link1.Text = "Previous 4 Days";
        
    //    link1.Style.Add("float", "left");
    //    link1.Click += new EventHandler(prev4Click);
    //    link1.ID = "prevLink";

    //    LinkButton link = new LinkButton();
    //    link.Text = "Next 4 Days";
    //    link.ID = "nextLink";
    //    link.CssClass = "AddLinkNoPadding";
    //    link.Style.Add("float", "right");
    //    link.Click += new EventHandler(next4Click);

    //    Literal lit2 = new Literal();
    //    lit2.Text = "</div>";

    //    topPanel.Controls.Add(lab);
    //    topPanel.Controls.Add(link1);
    //    topPanel.Controls.Add(link);
    //    topPanel.Controls.Add(lit2);

        

    //    Literal lit = new Literal();
    //    lit.Text = "<br/>";
       
    //    Up.ContentTemplateContainer.Controls.Add(topPanel);
    //    Up.ContentTemplateContainer.Controls.Add(lit);
    //    //Draw the panel
    //    Panel panelDay1 = new Panel();
    //    Label label1 = new Label();

        

    //    label1.Text = (startDate).Date.ToShortDateString() + "<br/>";
    //    Label label12 = new Label();
    //    label1.CssClass = "NumLabel";
    //    label12.CssClass = "NumLabel";

    //    if (startDate < DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Date)
    //    {
    //        panelDay1.Enabled = false;
    //        label12.Text = "0 Users in Categories<br/>" +
    //            "0 Users in Location <br/>";
    //    }
    //    else
    //    {

    //        label12.Text = (firstDayCount).ToString() + " Users in Categories<br/>" +
    //            firstDayCountLoc.ToString() + " Users in Location <br/>";

    //    }
    //    Button button1 = new Button();
    //    button1.Text = "Select date";
    //    button1.ID = "selectDate1";
    //    button1.Click += new EventHandler(SelectDate);
    //    button1.CommandArgument = (startDate).Date.ToShortDateString();
    //    panelDay1.Height = 100;
    //    panelDay1.BorderColor = green;
    //    panelDay1.BorderStyle = BorderStyle.Solid;
    //    panelDay1.BorderWidth = 2;
    //    panelDay1.Style.Add("float", "left");
    //    panelDay1.Style.Add("padding", "5px");
    //    panelDay1.Style.Add("margin", "5px");
    //    panelDay1.Controls.Add(label1);
    //    panelDay1.Controls.Add(label12);
    //    panelDay1.Controls.Add(button1);
    //    Up.ContentTemplateContainer.Controls.Add(panelDay1);

    //    Panel panelDay2 = new Panel();

        

    //    Label label2 = new Label();
    //    label2.Text = (startDate).AddDays(double.Parse("1.00")).ToShortDateString() + "<br/>";
    //    Label label22 = new Label();

    //    if ((startDate).AddDays(double.Parse("1.00")) < DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Date)
    //    {
    //        panelDay2.Enabled = false;
    //        label22.Text = "0 Users in Categories<br/>" +
    //            "0 Users in Location <br/>";
    //    }
    //    else
    //    {

    //        label22.Text = secDayCount.ToString() + " Users in Categories<br/>" +
    //            secDayCountLoc.ToString() + " Users in Location <br/>";
    //    }

    //    label2.CssClass = "NumLabel";
    //    label22.CssClass = "NumLabel";

    //    Button button2 = new Button();
    //    button2.Text = "Select date";
    //    button2.ID = "selectDate2";
    //    button2.Click += new EventHandler(SelectDate);
    //    button2.CommandArgument = (startDate).AddDays(double.Parse("1.00")).ToShortDateString();
    //    panelDay2.Height = 100;
    //    panelDay2.BorderColor = green;
    //    panelDay2.BorderStyle = BorderStyle.Solid;
    //    panelDay2.BorderWidth = 2;
    //    panelDay2.Style.Add("float", "left");
    //    panelDay2.Style.Add("padding", "5px");
    //    panelDay2.Style.Add("margin", "5px");
    //    panelDay2.Controls.Add(label2);
    //    panelDay2.Controls.Add(label22);
    //    panelDay2.Controls.Add(button2);
    //    Up.ContentTemplateContainer.Controls.Add(panelDay2);

    //    Panel panelDay3 = new Panel();
        

    //    Label label3 = new Label();
    //    label3.Text = (startDate).AddDays(double.Parse("2.00")).ToShortDateString() + "<br/>";
    //    Label label32 = new Label();

    //    if ((startDate).AddDays(double.Parse("2.00")) < DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Date)
    //    {
    //        panelDay3.Enabled = false;
    //        label32.Text = "0 Users in Categories<br/>" +
    //           "0 Users in Location <br/>";
    //    }
    //    else
    //    {

    //        label32.Text = secDayCount.ToString() + " Users in Categories<br/>" +
    //            thirdDayCountLoc.ToString() + " Users in Location <br/>";

    //    }

    //    label3.CssClass = "NumLabel";
    //    label32.CssClass = "NumLabel";

    //    Button button3 = new Button();
    //    button3.Text = "Select date";
    //    button3.ID = "selectDate3";
    //    button3.Click += new EventHandler(SelectDate);
    //    button3.CommandArgument = (startDate).AddDays(double.Parse("2.00")).ToShortDateString();
    //    panelDay3.Height = 100;
    //    panelDay3.BorderColor = green;
    //    panelDay3.BorderStyle = BorderStyle.Solid;
    //    panelDay3.BorderWidth = 2;
    //    panelDay3.Style.Add("float", "left");
    //    panelDay3.Style.Add("padding", "5px");
    //    panelDay3.Style.Add("margin", "5px");
    //    panelDay3.Controls.Add(label3);
    //    panelDay3.Controls.Add(label32);
    //    panelDay3.Controls.Add(button3);
    //    Up.ContentTemplateContainer.Controls.Add(panelDay3);

    //    Panel panelDay4 = new Panel();

        

    //    Label label4 = new Label();
    //    label4.Text = (startDate).AddDays(double.Parse("3.00")).ToShortDateString() + "<br/>";
    //    Label label42 = new Label();

    //    if ((startDate).AddDays(double.Parse("3.00")) < DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Date)
    //    {
    //        panelDay4.Enabled = false;
    //        label42.Text = "0 Users in Categories<br/>" +
    //            "0 Users in Location <br/>";
    //    }
    //    else
    //    {

    //        label42.Text = secDayCount.ToString() + " Users in Categories<br/>" +
    //            fourthDayCountLoc.ToString() + " Users in Location <br/>";
    //    }

    //    label4.CssClass = "NumLabel";
    //    label42.CssClass = "NumLabel";

    //    Button button4 = new Button();
    //    button4.Text = "Select date";
    //    button4.ID = "selectDate4";
    //    button4.Click += new EventHandler(SelectDate);
    //    button4.CommandArgument = (startDate).AddDays(double.Parse("3.00")).ToShortDateString();
    //    panelDay4.Height = 100;
    //    panelDay4.BorderColor = green;
    //    panelDay4.BorderStyle = BorderStyle.Solid;
    //    panelDay4.BorderWidth = 2;
    //    panelDay4.Style.Add("float", "left");
    //    panelDay4.Style.Add("padding", "5px");
    //    panelDay4.Style.Add("margin", "5px");
    //    panelDay4.Controls.Add(label4);
    //    panelDay4.Controls.Add(label42);
    //    panelDay4.Controls.Add(button4);
    //    Up.ContentTemplateContainer.Controls.Add(panelDay4);


    //    AsyncPostBackTrigger apt = new AsyncPostBackTrigger();
    //    apt.ControlID = link1.ClientID;
    //    apt.EventName = "Click";
    //    Up.Triggers.Add(apt);

    //    AsyncPostBackTrigger apt1 = new AsyncPostBackTrigger();
    //    apt1.ControlID = button1.ClientID;
    //    apt1.EventName = "Click";
    //    Up.Triggers.Add(apt1);

    //    AsyncPostBackTrigger apt2 = new AsyncPostBackTrigger();
    //    apt2.ControlID = link.ClientID;
    //    apt2.EventName = "Click";
    //    Up.Triggers.Add(apt2);

    //    AsyncPostBackTrigger apt3 = new AsyncPostBackTrigger();
    //    apt3.ControlID = button2.ClientID;
    //    apt3.EventName = "Click";
    //    Up.Triggers.Add(apt3);

    //    AsyncPostBackTrigger apt4 = new AsyncPostBackTrigger();
    //    apt4.ControlID = button3.ClientID;
    //    apt4.EventName = "Click";
    //    Up.Triggers.Add(apt4);

    //    AsyncPostBackTrigger apt5 = new AsyncPostBackTrigger();
    //    apt5.ControlID = button4.ClientID;
    //    apt5.EventName = "Click";
    //    Up.Triggers.Add(apt5);

    //    AsyncPostBackTrigger apt6 = new AsyncPostBackTrigger();
    //    apt6.ControlID = AdPlacementList.UniqueID;
    //    apt6.EventName = "SelectedIndexChanged";
    //    Up.Triggers.Add(apt6);

    //    Up.Update();

    //    CategoryDaysPanel.Controls.Add(Up);
    //}

    //protected void SelectDate(object sender, EventArgs e)
    //{
    //    Button b = (Button)sender;
    //    string startDateReal = b.CommandArgument;
        
    //    Session["SelectedStartDate"] = startDateReal;

    //    Literal lab = (Literal)CategoryDaysPanel.FindControl("dateLiteral");
    //    lab.Text = "<div style=\"clear: both; float: right;\"><label>Selected Start Date: </label><span " +
    //        "class=\"AddGreenLinkBig\">" + startDateReal + "</span></div><div style=\"clear: both;\">";

    //    UpdatePanel up = (UpdatePanel)CategoryDaysPanel.FindControl("Up");
    //    up.Update();
    //}

    //protected void next4Click(object sender, EventArgs e)
    //{
    //    //CategoryDaysPanel.Controls.Clear();
    //    Session["UserAvailableDate"] = ((DateTime)Session["UserAvailableDate"]).AddDays(4);

    //    bool isBig = false;
    //    if (AdPlacementList.SelectedValue == "0.04")
    //        isBig = true;

    //    GetAvailableUsers((DateTime)Session["UserAvailableDate"], isBig);

        
    //}

    //protected void prev4Click(object sender, EventArgs e)
    //{
    //    CategoryDaysPanel.Controls.Clear();
    //    Session["UserAvailableDate"] = ((DateTime)Session["UserAvailableDate"]).AddDays(-4);

    //    bool isBig = false;
    //    if (AdPlacementList.SelectedValue == "0.04")
    //        isBig = true;

    //    GetAvailableUsers((DateTime)Session["UserAvailableDate"], isBig);
    //}

    private void FillLiteral()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));


        if (bool.Parse(isFeatured.Text))
        {
            FeaturedWeWouldPanel.Visible = true;
            bool isBig = false;
            string templateID = "";

            if (AdPlacementList.SelectedValue == "0.04")
            {
                isBig = true;
                templateID = RadioButtonList1.SelectedValue;
            }
            else
            {
                templateID = TemplateRadioList.SelectedValue;
            }

            FeaturedPreviewPanel.Visible = true;
            int integer = 2;
            string username = dat.GetDataDV("select * from Users where user_id=" + Session["User"].ToString())[0]["UserName"].ToString();
            string email = "";
            if (AdPictureCheckList.Items.Count == 0 || !AdMediaPanel.Visible)
                email = GetEmailString(AdNameTextBox.THE_TEXT, null,
                 null, SummaryTextBox.InnerHtml, username, isBig, ref integer, templateID);
            else
                email = GetEmailString(AdNameTextBox.THE_TEXT, AdPictureCheckList.Items[0].Value,
                 null, SummaryTextBox.InnerHtml, username, isBig, ref integer, templateID);

            if (!isBig)
                email += "</tr>";

            email = "<table>" + email + "</table>";
            FeaturedPreviewLiteral.Text = email;
        }
        else
        {
            FeaturedWeWouldPanel.Visible = false;
        }


        EventPanel.Visible = true;
        ShowHeaderName.Text = AdNameTextBox.THE_TEXT;
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

        

        //for (int i = 0; i < PictureCheckList.Items.Count; i++)
        //{
        //    finalFileArray[i] = "http://" + Request.Url.Authority + "/HippoHappenings/UserFiles/" + 
        //        Session["UserName"].ToString() + "/Slider/" + PictureCheckList.Items[i].Value;
        //}
        char[] delim2 = { '.' };
        for (int i = 0; i < PictureCheckList.Items.Count; i++)
        {
            Literal literal4 = new Literal();
            string[] tokens = PictureCheckList.Items[i].Value.ToString().Split(delim2);
            if (tokens.Length >= 2)
            {
                if (tokens[1].ToUpper() == "JPG" || tokens[1].ToUpper() == "JPEG" || tokens[1].ToUpper() == "GIF" || tokens[1].ToUpper() == "PNG")
                {
                    System.Drawing.Image image = System.Drawing.Image.FromFile(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\AdSlider\\" + PictureCheckList.Items[i].Value.ToString());


                    int width = 410;
                    int height = 250;

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


                    literal4.Text = "<div style=\"width: 410px; height: 250px;background-color: black;\"><img style=\"cursor: pointer; margin-left: " + ((410 - newIntWidth) / 2).ToString() + "px; margin-top: " + ((250 - newHeight) / 2).ToString() + "px;\" height=\"" + newHeight + "px\" width=\"" + newIntWidth + "px\" src=\""
                        + "UserFiles/" + Session["UserName"].ToString() + "/AdSlider/" + PictureCheckList.Items[i].Value.ToString() + "\" /></div>";


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
                literal4.Text = "<div style=\"float:left;\"><object width=\"410\" height=\"250\"><param  name=\"wmode\" value=\"opaque\" ></param><param name=\"movie\" value=\"http://www.youtube.com/v/" + PictureCheckList.Items[i].Value.ToString() + "\"></param><param name=\"allowFullScreen\" value=\"true\"></param><embed wmode=\"opaque\" src=\"http://www.youtube.com/v/" + PictureCheckList.Items[i].Value.ToString() + "\" type=\"application/x-shockwave-flash\" allowfullscreen=\"true\" width=\"410\" height=\"250\"></embed></object></div>";
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

        //if (MainAttractionCheck.Checked)
        //{
        //    if (MainAttractionRadioList.SelectedValue == "0")
        //    {
        //        Rotator1.Items.Clear();
        //        RotatorPanel.Visible = false;
        //        if (VideoPictureRadioList.SelectedValue == "0")
        //        {
        //            if (PictureCheckList.Items.Count != 0)
        //                ShowVideoPictureLiteral.Text = "<img style=\"float: left; padding-right: 10px; padding-top: 9px;\" height=\"250px\" width=\"440px\" src=\"UserFiles/" + PictureCheckList.Items[0].Value + "\" />";
        //        }
        //        else
        //        {
        //            if (VideoRadioList.SelectedValue == "0")
        //            {
        //                //ShowVideoPictureLiteral.Text = "<div style=\"float:left; padding-top: 9px; padding-right: 10px;\"><embed  height=\"250px\" width=\"440px\" src=\"UserFiles/" + VideoCheckList.Items[0].Text + "\" /></div>";
        //                ShowVideoPictureLiteral.Text = "<div style=\"float:left; padding-top: 9px; padding-right: 10px;\"><object width=\"440\" height=\"250\"><param name=\"movie\" value=\"UserFiles/" + VideoCheckList.Items[0].Text + "\"></param><param name=\"allowFullScreen\" value=\"true\"></param><embed src=\"UserFiles/" + VideoCheckList.Items[0].Text + "\" type=\"application/x-shockwave-flash\" allowfullscreen=\"true\" width=\"440\" height=\"250\"></embed></object></div>";
        //            }
        //            else
        //                ShowVideoPictureLiteral.Text = "<div style=\"float:left; padding-top: 9px; padding-right: 10px;\"><object width=\"440\" height=\"250\"><param name=\"movie\" value=\"http://www.youtube.com/v/" + YouTubeTextBox.Text + "\"></param><param name=\"allowFullScreen\" value=\"true\"></param><embed src=\"http://www.youtube.com/v/" + YouTubeTextBox.Text + "\" type=\"application/x-shockwave-flash\" allowfullscreen=\"true\" width=\"440\" height=\"250\"></embed></object></div>";
        //        }
        //    }
        //    else
        //    {
        //        ShowVideoPictureLiteral.Text = "";
        //        if (SliderCheckList.Items.Count > 0)
        //        {
        //            char[] delim = { '\\' };
        //            string[] fileArray = System.IO.Directory.GetFiles(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\AdSlider\\");

        //            string[] finalFileArray = new string[fileArray.Length];

        //            for (int i = 0; i < SliderCheckList.Items.Count; i++)
        //            {
        //                finalFileArray[i] = "http://" + Request.Url.Authority + "/HippoHappenings/UserFiles/" +
        //                     Session["UserName"].ToString() + "/AdSlider/" + SliderCheckList.Items[i].Value;
        //            }
                    
        //            Rotator1.DataSource = finalFileArray;
        //            Rotator1.DataBind();
        //            RotatorPanel.Visible = true;
        //        }
        //    }
        //}
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
                if (bool.Parse(isFeatured.Text))
                    ChangeSelectedTab(6, 5);
                else
                    ChangeSelectedTab(6, 3);
                break;
            default: break;
        }
    }
    
    protected void SelectFreePanel(object sender, EventArgs e)
    {
        SelectFree();
    }

    protected void SelectFree()
    {
        FeaturedStartDatePanel.Visible = false;
        NormalStartDatePanel.Visible = true;
        isFeatured.Text = "false";
        //AdTabStrip.Tabs[2].Visible = false;
        AdTabStrip.Tabs[4].Visible = false;
        AdTabStrip.Tabs[5].Visible = false;

        TabPostingDetails.Visible = false;
        TabFive.Visible = false;
        
        EndDatePanel.Visible = true;
        DaysExplanationPanel.Visible = false;
        TabsPanel.Visible = true;
        IntroPanel.Visible = false;

        AdTabStrip.Tabs[6].ImageUrl = "images/PostingOrangeFull5.gif";
        AdTabStrip.Tabs[6].HoveredImageUrl = "images/PostingOrangeFull5.gif";
        AdTabStrip.Tabs[6].DisabledImageUrl = "images/PostingOrangeEmpty5.gif";
        AdTabStrip.Tabs[6].SelectedImageUrl = "images/PostingOrangeFull5.gif";
        AdTabStrip.Tabs[6].PageViewID = "TabSix";

        AdTabStrip.Tabs[2].ImageUrl = "images/LocationOrangeFull.gif";
        AdTabStrip.Tabs[2].HoveredImageUrl = "images/LocationOrangeFull.gif";
        AdTabStrip.Tabs[2].DisabledImageUrl = "images/LocationOrangeEmpty.gif";
        AdTabStrip.Tabs[2].SelectedImageUrl = "images/LocationOrangeFull.gif";

        //InfoPanelFreeLabel.Visible = true;

        //InfoPanelFeaturedLabel.Visible = false;
    }

    protected void SelectFeaturedPanel(object sender, EventArgs e)
    {
        SelectFeatured();

    }

    protected void SelectFeatured()
    {
        FeaturedStartDatePanel.Visible = true;
        NormalStartDatePanel.Visible = false;
        isFeatured.Text = "true";
        AdTabStrip.Tabs[2].Visible = true;
        AdTabStrip.Tabs[4].Visible = true;
        AdTabStrip.Tabs[5].Visible = true;
        EndDatePanel.Visible = false;
        DaysExplanationPanel.Visible = true;
        IntroPanel.Visible = false;
        TabsPanel.Visible = true;
        CatDisplayPanel.Visible = true;
        //InfoPanelFeaturedLabel.Visible = true;

        //InfoPanelFreeLabel.Visible = false;
    }
    
    protected void ClearForm()
    {

    }
    
    protected void setCategories()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet dsCategories = dat.GetData("SELECT * FROM Ad_Category_Mapping WHERE AdID=" +
            Request.QueryString["ID"].ToString());

        if (dsCategories.Tables.Count > 0)
            if (dsCategories.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsCategories.Tables[0].Rows.Count; i++)
                {
                    Telerik.Web.UI.RadTreeNode node = (Telerik.Web.UI.RadTreeNode)CategoryTree.FindNodeByValue(dsCategories.Tables[0].Rows[i]["CategoryID"].ToString());
                    if (node != null)
                    {
                        node.Checked = true;

                        //node.Enabled = false;
                    }
                    else
                    {

                        node = (Telerik.Web.UI.RadTreeNode)RadTreeView1.FindNodeByValue(dsCategories.Tables[0].Rows[i]["CategoryID"].ToString());
                        if (node != null)
                        {
                            node.Checked = true;
                            //node.Enabled = false;
                        }
                        else
                        {

                            node = (Telerik.Web.UI.RadTreeNode)RadTreeView2.FindNodeByValue(dsCategories.Tables[0].Rows[i]["CategoryID"].ToString());
                            if (node != null)
                            {
                                node.Checked = true;
                                //node.Enabled = false;
                            }
                            else
                            {
                                node = (Telerik.Web.UI.RadTreeNode)RadTreeView3.FindNodeByValue(dsCategories.Tables[0].Rows[i]["CategoryID"].ToString());
                                if (node != null)
                                {
                                    node.Checked = true;
                                    //node.Enabled = false;
                                }

                            }

                        }
                    }
                    //CategoriesCheckBoxes.Items.FindByValue(dsCategories.Tables[0].Rows[i]["CategoryID"].ToString()).Selected = true;
                    //CategoriesCheckBoxes.Items.FindByValue(dsCategories.Tables[0].Rows[i]["CategoryID"].ToString()).Enabled = false;
                }
            }

        Session["AdCategoriesSet"] = "notnull";
    }
    
    protected void EnableMainAttractionPanel(object sender, EventArgs e)
    {
        if (MainAttractionCheck.Checked)
            MainAttractionPanel.Enabled = true;
        else
            MainAttractionPanel.Enabled = false;
    }
    
    protected void EnableAdMediaPanel(object sender, EventArgs e)
    {
        if (BannerAdCheckBox.Checked)
        {
            AdMediaPanel.Enabled = true;
            AdMediaPanel.Visible = true;

            TemplateRadioList.SelectedValue = "1";
            RadioButtonList1.SelectedValue = "1";

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
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (PictureUpload.HasFile)
        {
            if (PictureCheckList.Items.Count < 20)
            {
                char[] delim = { '.' };
                string[] tokens = PictureUpload.FileName.Split(delim);

                if (tokens.Length >= 2)
                {
                    if (tokens[1].ToUpper() == "JPG" || tokens[1].ToUpper() == "JPEG" || tokens[1].ToUpper() == "GIF" || tokens[1].ToUpper() == "PNG")
                    {

                        string fileName = "rename" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Ticks.ToString() + "." + tokens[1];
                        PictureCheckList.Items.Add(new ListItem(PictureUpload.FileName, fileName));

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

                        System.Drawing.Image img = System.Drawing.Image.FromStream(PictureUpload.PostedFile.InputStream);

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
    
    protected void AdPictureUpload_Click(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (AdPictureUpload.HasFile)
        {
            AdPictureCheckList.Items.Clear();
            char[] delim = { '.' };
            string[] tokens = AdPictureUpload.FileName.Split(delim);
            string fileName = "rename" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Ticks.ToString() + "." + tokens[1];
            AdPictureCheckList.Items.Add( new ListItem(AdPictureUpload.FileName, fileName));
            if (!System.IO.Directory.Exists(MapPath(".").ToString() + "/UserFiles/" +
                Session["UserName"].ToString()))
                System.IO.Directory.CreateDirectory(MapPath(".").ToString() + "/UserFiles/" +
                Session["UserName"].ToString());


            System.Drawing.Image img = System.Drawing.Image.FromStream(AdPictureUpload.PostedFile.InputStream);

            SaveThumbnail(img, false, MapPath(".").ToString() + "/UserFiles/" +
                Session["UserName"].ToString() + "/" + fileName, "image/" + tokens[1].ToLower());


            //AdPictureUpload.SaveAs(MapPath(".").ToString() + "/UserFiles/" +
            //    Session["UserName"].ToString() + "/" + fileName);
            AdNixItButton.Visible = true;
        }
    }

    //protected void VideoUpload_Click(object sender, EventArgs e)
    //{
    //    if (VideoUpload.HasFile)
    //    {
    //        VideoCheckList.Items.Clear();
    //        char[] delim = { '.' };
    //        string[] tokens = VideoUpload.FileName.Split(delim);
    //        string fileName = "rename" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Ticks.ToString() + "." + tokens[1];
    //        VideoCheckList.Items.Add(fileName);
    //        VideoUpload.SaveAs(MapPath(".").ToString() + "/UserFiles/" +
    //            Session["UserName"].ToString() + "/" + fileName);
    //        VideoNixItButton.Visible = true;
    //    }
    //}

    protected void YouTubeUpload_Click(object sender, EventArgs e)
    {
        if (YouTubeTextBox.Text != "")
        {
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
    
    protected void ShowVideoOrPicture(object sender, EventArgs e)
    {
        if (VideoPictureRadioList.SelectedValue == "0")
        {
            PicturePanel.Visible = true;
            VideoPanel.Visible = false;
        }
        else
        {
            PicturePanel.Visible = false;
            VideoPanel.Visible = true;
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

    protected void EnableMusicPanel(object sender, EventArgs e)
    {
        if (MusicCheckBox.Checked)
            MusicPanel.Enabled = true;
        else
            MusicPanel.Enabled = false;
    }

    protected void MusicUpload_Click(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (!MusicCheckBox.Checked)
            MusicCheckBox.Checked = true;

        if (MusicUpload.HasFile && SongCheckList.Items.Count < 3)
        {
            char[] delim = { '.' };
            string[] tokens = MusicUpload.FileName.Split(delim);
            string fileName = "rename" + DateTime.Parse(cookie.Value.ToString().Replace("%20",
                " ").Replace("%3A", ":")).Ticks.ToString() + "." + tokens[1];
            string extension = fileName.Split(delim)[1];
            if (extension.ToUpper() == "MP3")
            {
                string root = MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\Songs\\";
                if (SongCheckList.Items.Count == 0)
                {
                    if (!System.IO.Directory.Exists(MapPath(".") + "\\UserFiles\\" +
                        Session["UserName"].ToString()))
                    {
                        System.IO.Directory.CreateDirectory(MapPath(".") + "\\UserFiles\\" +
                            Session["UserName"].ToString());
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
                YourMessagesLabel.Text += "<br/><br/>The music file has to be .mp3 file.";
            }
        }
    }
    //protected void PictureNixIt(object sender, EventArgs e)
    //{
    //    PictureCheckList.Items.Clear();

    //    PictureNixItButton.Visible = false;
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
            PictureNixItButton.Visible = false;
    }
    
    protected void VideoNixIt(object sender, EventArgs e)
    {
        VideoCheckList.Items.Clear();

        VideoNixItButton.Visible = false;
    }
    
    protected void PostIt(object sender, EventArgs e)
    {
        object appSeshCountry;
        object appSeshState;
        object appSeshCity;
        object appSeshZip;
        object appSeshRadius;
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        string state = "";
        if (AgreeCheckBox.Checked)
        {
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
            conn.Open();

            bool isEdit = false;

            if (Request.QueryString["edit"] != null)
            {
                isEdit = bool.Parse(Request.QueryString["edit"].ToString());
            }

            string mediaCat = "0";
            if (PictureCheckList.Items.Count > 0)
                mediaCat = "1";

            string command = "";

            DataSet dsEvent;
            int numViews = 0;
            if (isEdit)
            {
                dsEvent = dat.GetData("SELECT * FROM Ads WHERE Ad_ID="+adID.Text);
                numViews = int.Parse(dsEvent.Tables[0].Rows[0]["NumViews"].ToString());

                string rad = "";

                if (RadiusPanel.Visible)
                    rad = ", Radius=@radius ";

                DataView dvRenu = dat.GetDataDV("SELECT * FROM Ads WHERE Ad_ID=" + adID.Text);
                string renutext = "";
                if (bool.Parse(dvRenu[0]["ReNewed"].ToString()))
                {
                    renutext = ", NumCurrentViews=0 ";
                }

                command = "UPDATE Ads SET NonUsersAllowed=@nonUsers, Template=@template, hasSongs=@songs, User_ID=@userID, FeaturedSummary=@featuredSummary , " +
                    "Description=@description, Header=@header, CountShown=@countShown,  mediaCategory=" + mediaCat + ", " +
                    "FeaturedPicture=@featuredPicture, FeaturedPictureName=@featuredPictureName, CatCountry=@catCountry, CatState=@catState, CatCity=@catCity, " +
                    "CatZip=@catZip" + rad + ", DisplayToAll=@displayAll, NumViews=@num " +
                    renutext + " WHERE Ad_ID=" + adID.Text;
            }
            else
            {
                string rad = "";
                string radEnd = "";
                if (RadiusPanel.Visible)
                {
                    rad = ", Radius ";
                    radEnd = ", @radius ";
                }

                command = "INSERT INTO Ads (NonUsersAllowed, Template, hasSongs, User_ID, FeaturedSummary ,Description, Header, " +
                    "CountShown, Featured, mediaCategory, FeaturedPicture, FeaturedPictureName, CatCountry, CatState, CatCity, "+
                    "CatZip" + rad + ", DisplayToAll, NumViews, NumCurrentViews, BigAd, DateAdded) "
                    + " VALUES(@nonUsers, @template, @songs, @userID, @featuredSummary, @description, @header, @countShown, "+
                    "@featured, " + mediaCat + ", @featuredPicture, @featuredPictureName, @catCountry, @catState, @catCity, @catZip" + radEnd + ", @displayAll, @num, 0, @big, '" + 
                    DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).ToString() + "')";
            }


            SqlCommand cmd = new SqlCommand(command, conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@nonUsers", SqlDbType.Bit).Value = AllLocationCheckBox.Checked;
            cmd.Parameters.Add("@num", SqlDbType.Int).Value = int.Parse(UsersTextBox.Text) + numViews;
            cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = DescriptionTextBox.Content;
            cmd.Parameters.Add("@userID", SqlDbType.Int).Value = int.Parse(Session["User"].ToString());
            cmd.Parameters.Add("@featured", SqlDbType.Bit).Value = bool.Parse(isFeatured.Text);
            if (bool.Parse(isFeatured.Text))
            {
                if (BannerAdCheckBox.Checked)
                {
                    if (AdPlacementList.SelectedValue == "0.01")
                        cmd.Parameters.Add("@template", SqlDbType.Int).Value = TemplateRadioList.SelectedValue;
                    else
                        cmd.Parameters.Add("@template", SqlDbType.Int).Value = RadioButtonList1.SelectedValue;
                }
                else
                {
                    cmd.Parameters.Add("@template", SqlDbType.Int).Value = DBNull.Value;
                }
            }
            else
            {
                cmd.Parameters.Add("@template", SqlDbType.Int).Value = DBNull.Value;
            }
            cmd.Parameters.Add("@songs", SqlDbType.Bit).Value = MusicCheckBox.Checked;

            if (bool.Parse(isFeatured.Text))
                cmd.Parameters.Add("@featuredSummary", SqlDbType.NVarChar).Value = SummaryTextBox.InnerHtml;
            else
                cmd.Parameters.Add("@featuredSummary", SqlDbType.NVarChar).Value = DBNull.Value;
            
            cmd.Parameters.Add("@header", SqlDbType.NVarChar).Value = AdNameTextBox.THE_TEXT;
            cmd.Parameters.Add("@countShown", SqlDbType.Int).Value = 0;

            if (AdPlacementList.SelectedValue == "0.01")
                cmd.Parameters.Add("@big", SqlDbType.Bit).Value = false;
            else
                cmd.Parameters.Add("@big", SqlDbType.Bit).Value = true;

            if (bool.Parse(isFeatured.Text))
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
                cmd.Parameters.Add("@featuredPicture", SqlDbType.NVarChar).Value = DBNull.Value;
                cmd.Parameters.Add("@featuredPictureName", SqlDbType.NVarChar).Value = DBNull.Value;
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
                    state = StateTextBox.THE_TEXT;

                appSeshState = state;

                if (state != "")
                    cmd.Parameters.Add("@catState", SqlDbType.NVarChar).Value = state;
                else
                    cmd.Parameters.Add("@catState", SqlDbType.NVarChar).Value = DBNull.Value;

                if (CityTextBox.Text.Trim() != "")
                {
                    appSeshCity = CityTextBox.Text.Trim();
                    cmd.Parameters.Add("@catCity", SqlDbType.NVarChar).Value = CityTextBox.Text.Trim();
                }
                else
                    cmd.Parameters.Add("@catCity", SqlDbType.NVarChar).Value = DBNull.Value;

                if (ZipTextBox.Text.Trim() != "")
                {
                    appSeshZip = ZipTextBox.Text.Trim();
                    appSeshRadius = RadiusDropDown.SelectedValue;
                    cmd.Parameters.Add("@catZip", SqlDbType.NVarChar).Value =
                        dat.GetAllZipsInRadius(RadiusDropDown.SelectedValue, ZipTextBox.Text.Trim(), true);
                }
                else
                    cmd.Parameters.Add("@catZip", SqlDbType.NVarChar).Value = DBNull.Value;

                if(CountryDropDown.SelectedValue == "223")
                    cmd.Parameters.Add("@radius", SqlDbType.Int).Value = RadiusDropDown.SelectedValue;
            }
            else
            {
                cmd.Parameters.Add("@catCountry", SqlDbType.Int).Value = DBNull.Value;
                cmd.Parameters.Add("@catState", SqlDbType.NVarChar).Value = DBNull.Value;
                cmd.Parameters.Add("@catCity", SqlDbType.NVarChar).Value = DBNull.Value;
                cmd.Parameters.Add("@catZip", SqlDbType.NVarChar).Value = DBNull.Value;
                cmd.Parameters.Add("@radius", SqlDbType.Int).Value = DBNull.Value;
            }
            cmd.Parameters.Add("@displayAll", SqlDbType.Bit).Value = DisplayCheckList.Items[0].Selected;
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
            {
                dat.Execute("DELETE FROM Ad_Calendar WHERE AdID="+adID.Text);

                if (bool.Parse(isFeatured.Text))
                {
                    cmd = new SqlCommand("INSERT INTO Ad_Calendar (AdID, DateTimeStart, DateTimeEnd) VALUES(@id, @start, @end)", conn);
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = adID.Text;
                    cmd.Parameters.Add("@start", SqlDbType.DateTime).Value = Session["SelectedStartDate"].ToString();
                    cmd.Parameters.Add("@end", SqlDbType.DateTime).Value = DateTime.Parse(Session["SelectedStartDate"].ToString()).AddDays(double.Parse(DaysDropDown.SelectedValue));
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd = new SqlCommand("INSERT INTO Ad_Calendar (AdID, DateTimeStart, DateTimeEnd) VALUES(@id, @start, @end)", conn);
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = adID.Text;
                    cmd.Parameters.Add("@start", SqlDbType.DateTime).Value = StartDateTimePicker.DbSelectedDate;
                    cmd.Parameters.Add("@end", SqlDbType.DateTime).Value = DateTime.Parse(StartDateTimePicker.DbSelectedDate.ToString()).AddDays(double.Parse(DaysDropDown.SelectedValue));
                    cmd.ExecuteNonQuery();
                }
            }
            else
            {
                cmd = new SqlCommand("INSERT INTO Ad_Calendar (AdID, DateTimeStart, DateTimeEnd) VALUES(@id, @start, @end)", conn);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = ID;
                if (bool.Parse(isFeatured.Text))
                {
                    cmd.Parameters.Add("@start", SqlDbType.DateTime).Value = Session["SelectedStartDate"].ToString();
                    cmd.Parameters.Add("@end", SqlDbType.DateTime).Value = DateTime.Parse(Session["SelectedStartDate"].ToString()).AddDays(double.Parse(DaysDropDown.SelectedValue));
                }
                else
                {
                    cmd.Parameters.Add("@start", SqlDbType.DateTime).Value = DateTime.Parse(StartDateTimePicker.DbSelectedDate.ToString());
                    cmd.Parameters.Add("@end", SqlDbType.DateTime).Value = DateTime.Parse(StartDateTimePicker.DbSelectedDate.ToString()).AddDays(double.Parse(DaysDropDown.SelectedValue));
                }
                    
                    cmd.ExecuteNonQuery();
            }


            

            string theID = ID;
            if (isEdit)
            {
                theID = adID.Text;
            }
            if (MusicCheckBox.Checked)
            {
                
                if (isEdit)
                {
                    
                    dat.Execute("DELETE FROM Ad_Song_Mapping WHERE AdID="+theID);
                }
                
                for (int i = 0; i < SongCheckList.Items.Count; i++)
                {
                        cmd = new SqlCommand("INSERT INTO Ad_Song_Mapping (AdID, SongName, SongTitle) "+
                            "VALUES(@eventID, @songName, @songTitle)", conn);
                        cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = int.Parse(theID);
                        cmd.Parameters.Add("@songName", SqlDbType.NVarChar).Value = SongCheckList.Items[i].Value.ToString();
                        cmd.Parameters.Add("@songTitle", SqlDbType.NVarChar).Value = SongCheckList.Items[i].Text;
                        cmd.ExecuteNonQuery();

                        dat.Execute("UPDATE Ads SET hasSongs=1 WHERE Ad_ID="+theID);
                }
            }

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
                dat.Execute("DELETE FROM Ad_Category_Mapping WHERE AdID="+theID);
            }

            CreateCategories(theID);


            string views = dat.GetDataDV("SELECT NumViews - NumCurrentViews AS Views FROM Ads WHERE Ad_ID=" + 
                theID)[0]["Views"].ToString();

            if (bool.Parse(isFeatured.Text) && int.Parse(views) != 0)
                EmailSavedSearches(theID, views);
            

            //if (CategoriesCheckBoxes.Items.Count > 0)
            //{
            //    int catCount = CategoriesCheckBoxes.Items.Count;

            //    for (int i = 0; i < catCount; i++)
            //    {
            //        cmd = new SqlCommand("INSERT INTO Ad_Category_Mapping (AdID, CategoryID) VALUES (@adID, @catID)", conn);
            //        cmd.Parameters.Add("@adID", SqlDbType.Int).Value = ID;
            //        cmd.Parameters.Add("@catID", SqlDbType.Int).Value = int.Parse(CategoriesCheckBoxes.Items[i].Value);
            //        cmd.ExecuteNonQuery();
            //    }
            //}

            string adFeatured = "";
            string adFeaturedEmail = "";
            if (bool.Parse(isFeatured.Text))
            {
                adFeatured += "<br/><br/>Since you selected to have your ad Featured, you can view the progress of your ad on the <a class=\"AddLink\" onclick=\"Search('AdStatistics.aspx?Ad="+theID+"');\">Ad Statistics</a> page, which is also linkable from your user page.";
                adFeaturedEmail += "<br/><br/>Since you selected to have your ad Featured, you can view the progress of your ad on the <a href=\"http://hippohappenings.com/AdStatistics.aspx?Ad="+theID+"\">Ad Statistics</a> page, which is also linkable from your user page.";
            }
            DataSet dsUser = dat.GetData("SELECT Email, UserName FROM USERS WHERE User_ID=" +
                    Session["User"].ToString());
            string emailBody = "<br/><br/>Dear " + dsUser.Tables[0].Rows[0]["UserName"].ToString() + ", <br/><br/> you have successfully posted the ad \"" + AdNameTextBox.THE_TEXT +
"\". <br/><br/> You can find this ad <a href=\"http://hippohappenings.com/" + dat.MakeNiceName(AdNameTextBox.THE_TEXT)+"_" + theID + "_Ad\">here</a>. " + adFeaturedEmail +
"<br/><br/> To rate your experience posting this ad <a href=\"http://hippohappenings.com/RateExperience.aspx?Type=A&ID=" + theID + "\">please include your feedback here.</a>" +
"<br/><br/><br/>Have a Hippo Happening Day!<br/><br/>";

            if (isEdit)
            {
                emailBody = "<br/><br/>Dear " + dsUser.Tables[0].Rows[0]["UserName"].ToString() + ", <br/><br/> you have successfully edited the ad \"" + AdNameTextBox.THE_TEXT +
"\". <br/><br/> You can find this ad <a href=\"http://hippohappenings.com/" + dat.MakeNiceName(AdNameTextBox.THE_TEXT) + "_" + theID + "_Ad\">here</a>. " + adFeaturedEmail +
"<br/><br/> To rate your experience editing this ad <a href=\"http://hippohappenings.com/RateExperience.aspx?Type=A&ID=" + theID + "\">please include your feedback here.</a>" +
"<br/><br/><br/>Have a Hippo Happening Day!<br/><br/>";
            }

            if (!Request.IsLocal)
            {
                dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                            System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                            dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "You have successfully posted the ad: " +
                            AdNameTextBox.THE_TEXT);
            }
            conn.Close();

            dat.SendFriendPostedAdNotification(Session["User"].ToString(), theID);

            Session["Message"] = "Your ad has been posted successfully!<br/> Here are your next steps: <br/>";

            if (isEdit)
            {
                Session["Message"] = "Your ad has been edited successfully!<br/> Here are your next steps: <br/>";
            }

            //Refresh all users' ads list in this state
            CheckAllUsers(AdPlacementList.SelectedValue != "0.01", theID, state);

            //Clear cache so that the PlayerList.xml can be grabbed by the browser again.
            ClearCache();

            Session["Message"] += "<br/>" + "Go to <a class=\"AddLink\" onclick=\"Search('" + dat.MakeNiceName(AdNameTextBox.THE_TEXT) + "_" + theID + "_Ad');\">your ad's</a> home page." + adFeatured + "<br/><br/> -<a class=\"AddLink\" onclick=\"Search('RateExperience.aspx?Type=A&ID=" + theID + "');\" >Rate </a>your user experience posting this venue.<br/>";
            //MessageLiteral.Text = "<script type=\"text/javascript\">alert('" + message + "');</script>";
            Encryption encrypt = new Encryption();
            MessageRadWindow.NavigateUrl = "Message.aspx?message=" + encrypt.encrypt(Session["Message"].ToString() + 
                "<br/><img onclick=\"Search('Home.aspx');\" onmouseover=\"this.src='image/DoneSonButtonSelected.png'\" onmouseout=\"this.src='image/DoneSonButton.png'\" src=\"image/DoneSonButton.png\"/>");
            MessageRadWindow.Visible = true;
            MessageRadWindowManager.VisibleOnPageLoad = true;

            Session["categorySession"] = null;
        }
        else
        {
            MessagePanel.Visible = true;
            YourMessagesLabel.Text += "<br/><br/>You must agree to the terms and conditions.";
        }
    }

    private delegate void CheckUsersDelegate(bool isBig, string AdID);

    protected void CheckAllUsers(bool isBig, string AdID, string state)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        DataView dvUsers = dat.GetDataDV("SELECT * FROM Users U, UserPreferences UP WHERE UP.UserID=U.User_ID AND UP.CatState='" + state + "'");
        DataView Ads;
        foreach (DataRowView row in dvUsers)
        {
            if (isBig)
            {
                Application.Add(row["User_ID"].ToString() + "_Big", "set");
            }
            else
            {
                Application.Add(row["User_ID"].ToString() + "_Normal", "set");
            }
        }
    }

    /// <summary>
    /// a.	Get all users who have ad searches that are Live and match the criteria
    ///i.	If the ad count ads up to the number in NumAdsInEmail
    ///1.	    Construct the email with those ads
    ///a.	        Big ads are bigger and on the top.
    ///2.	    Send the email
    ///a.	        When email is sent, update the viewed count for the ad in the Ads table. 
    ///b.	        Update the AdStatistics table with the ad and the manner the ad was seen: which will be Email not site view.
    ///3.	        Clear the list of ads in EmailAdList column
    ///ii.	If the ad count is less than NumAdsInEmail, 
    ///1.	    add this ad ID to the EmailAdList column.
    ///2.	    Increment the CountInEmailList column.
    /// </summary>
    /// <param name="adID"></param>
    protected void EmailSavedSearches(string adID, string views)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        int firstCount = 0;
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        /// a.	Get all users who have ad searches that are Live and match the criteria

        ///i.	If the ad count ads up to the number in NumAdsInEmail
        ///1.	    Construct the email with those ads
        ///a.	        Big ads are bigger and on the top.
        ///2.	    Send the email
        ///a.	        When email is sent, update the viewed count for the ad in the Ads table. 
        ///b.	        Update the AdStatistics table with the ad and the manner the ad was seen: which will be Email not site view.
        ///3.	        Clear the list of ads in EmailAdList column
        ///
        int sentAdCount = 0;

        int totalAdCount = int.Parse(views);


        Hashtable notTable = new Hashtable();
        //Send searches that are ready to go

       
            DataView dvSearches = GetSavedSearchesUsers(ref firstCount, false);

            if (firstCount > 0)
            {
                for (int j = 0; j < dvSearches.Count; j++)
                {
                    //Check if user has see the ad (the ad could be edited)
                    DataView dvAdStats = dat.GetDataDV("SELECT * FROM AdStatistics WHERE AdID=" + 
                        adID + " AND UserID=" + dvSearches[j]["UserID"].ToString());
                    if (dvAdStats.Count == 0)
                    {

                        string toEmail = "";
                        ///a.	        When email is sent, update the viewed count for the ad in the Ads table. 
                        ///b.	        Update the AdStatistics table with the ad and the manner the ad was seen: which will be Email not site view.
                        ///3.	        Clear the list of ads in EmailAdList column
                        string email = ConstructSearchesEmail(dvSearches[j]["ID"].ToString(), adID, ref toEmail,
                            ref sentAdCount, totalAdCount);

                        if (email != "")
                        {
                            notTable.Add(dvSearches[j]["ID"].ToString(), "1");
                            email = "<table><tr><td>" + email + 
                                "</td></tr></table><br/><br/><br/>Have a Hippo Happening Day!<br/><br/>";
                            if (!Request.IsLocal)
                            {
                                dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(), toEmail,
                                    email, "Your Saved Search Ads");
                            }
                            if (sentAdCount >= totalAdCount)
                            {
                                break;
                            }
                        }
                    }
                }

                //If we have already reached the total ad count that this user has paid for, notify the user.
                if (sentAdCount >= totalAdCount)
                {
                    if (!Request.IsLocal)
                        dat.SendAdFinishedEmail(adID);
                }
            }
            ///ii.	If the ad count is less than NumAdsInEmail, 
            ///1.	    add this ad ID to the EmailAdList column.
            ///2.	    Increment the CountInEmailList column.
            else
            {

            }
        

        
        //For the searches that are not ready to go yet, increment their set with this new ad
        DataView dvNotReadySearches = GetSavedSearchesUsers(ref firstCount, true);
        for (int i = 0; i < dvNotReadySearches.Count; i++)
        {
            if (Session["User"].ToString() != dvNotReadySearches[i]["UserID"].ToString())
            {
                //use the notTable hash to determine whether this search has be already updated in this post
                if (!notTable.Contains(dvNotReadySearches[i]["ID"].ToString()))
                {
                    dat.Execute("UPDATE SavedAdSearches SET CountInEmailList = CASE WHEN (CountInEmailList IS NULL) THEN 1 " +
                        "ELSE CountInEmailList + 1 END, EmailAdList = CASE WHEN (EmailAdList IS NULL) THEN ';" + adID +
                        "' ELSE EmailAdList + ';'+ '" +
                        adID + "' END WHERE ID=" + dvNotReadySearches[i]["ID"].ToString());
                }
            }
        }
    }

    protected string ConstructSearchesEmail(string searchID, string adID, ref string toEmail, ref int sentAdCount,
        int totalAdCount)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        int normalAdCount = 0;

        DataSet dsAdsOne = dat.GetData("SELECT * FROM Ads A, Users U WHERE A.User_ID=U.User_ID AND A.Ad_ID=" + 
            adID + " ORDER BY A.BigAd DESC");
        DataView dvAdsOne = new DataView(dsAdsOne.Tables[0], "", "", DataViewRowState.CurrentRows);

        DataSet ds = dat.GetData("SELECT * FROM SavedAdSearches WHERE ID=" + searchID);

        
        DataView dv = new DataView(ds.Tables[0], "", "", DataViewRowState.CurrentRows);

        if (dv[0]["UserID"].ToString() != Session["User"].ToString())
        {
            char[] delim = { ';' };
            string[] tokens = dv[0]["EmailAdList"].ToString().Split(delim);

            

            string ads = "";
            for (int i = 0; i < tokens.Length; i++)
            {
                if (tokens[i].Trim() != "")
                {
                    if (ads != "")
                        ads += " OR ";
                    else
                        ads = "( ";
                    ads += " A.Ad_ID=" + tokens[i].Trim();
                }
            }

            if (ads != "")
            {
                ads = " AND " + ads + " ) ";
            }

            DataSet dsAds = dat.GetData("SELECT * FROM Ads A, Users U WHERE A.User_ID=U.User_ID " + ads + 
                " ORDER BY A.BigAd DESC");
            DataView dvAds = new DataView(dsAds.Tables[0], "", "", DataViewRowState.CurrentRows);


            toEmail = dat.GetData("SELECT * FROM Users WHERE User_ID=" + dv[0]["UserID"].ToString()).Tables[0].Rows[0]["Email"].ToString();

            string email = "";

            email = "Below you will find ads pertaining to one of your saved ad searches. <br/><br/><table cellspacing=\"5\">";

            string categories = "";

            DataSet dsCats = dat.GetData("SELECT * FROM SavedAdSearches_Categories WHERE SearchID=" + searchID);
            DataView dvCats = new DataView(dsCats.Tables[0], "", "", DataViewRowState.CurrentRows);

            for (int n = 0; n < dvCats.Count; n++)
            {
                categories += dvCats[n]["CategoryID"].ToString() + ";";
            }
            DataView dvAdStatistics;
            if (bool.Parse(dvAdsOne[0]["BigAd"].ToString()))
            {
                email += GetEmailString(dvAdsOne[0]["Header"].ToString(), dvAdsOne[0]["FeaturedPicture"].ToString(),
                    dvAdsOne[0]["Ad_ID"].ToString(), dvAdsOne[0]["FeaturedSummary"].ToString(),
                    dvAdsOne[0]["UserName"].ToString(), bool.Parse(dvAdsOne[0]["BigAd"].ToString()),
                    ref normalAdCount, dvAdsOne[0]["Template"].ToString());

                ///a.	        When email is sent, update the viewed count for the ad in the Ads table. 
                ///BUT ONLY IF THE USER HAS NOT SEEN THIS AD PREVIOUSLY
                ///
                dvAdStatistics =
                    dat.GetDataDV("SELECT * FROM AdStatistics WHERE UserID=" + dv[0]["UserID"].ToString() + 
                    " AND AdID=" + dvAdsOne[0]["Ad_ID"].ToString());
                if (dvAdStatistics.Count == 0)
                {
                    dat.Execute("UPDATE Ads SET NumCurrentViews = NumCurrentViews + 1 WHERE Ad_ID=" + dvAdsOne[0]["Ad_ID"].ToString());
                    ///b.	        Update the AdStatistics table with the ad and the manner the ad was seen: 
                    /////which will be Email not site view.
                    //[UserID],[Reason],[LocationOnly],[Date],[AdID]

                    string locOnly = "'False'";
                    if (categories.Trim() == "")
                        locOnly = "'True'";
                    dat.Execute("INSERT INTO AdStatistics (UserID, Reason, LocationOnly, Date, AdID, WasEmail) VALUES(" + dv[0]["UserID"].ToString() +
                        ", '" + categories + "', " + locOnly + ", '" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).ToString() + "', " + dvAdsOne[0]["Ad_ID"].ToString() + ", 'True')");

                    //update the ad count for the algorithm
                    sentAdCount++;

                }
            }

            if (ads.Trim() != "")
            {
                for (int j = 0; j < dvAds.Count; j++)
                {
                    email += GetEmailString(dvAds[j]["Header"].ToString(), dvAds[j]["FeaturedPicture"].ToString(),
                        dvAds[j]["Ad_ID"].ToString(), dvAds[j]["FeaturedSummary"].ToString(),
                        dvAds[j]["UserName"].ToString(), bool.Parse(dvAds[j]["BigAd"].ToString()),
                        ref normalAdCount, dvAdsOne[0]["Template"].ToString());

                    dvAdStatistics =
                    dat.GetDataDV("SELECT * FROM AdStatistics WHERE UserID=" + dv[0]["UserID"].ToString() +
                    " AND AdID=" + dvAds[j]["Ad_ID"].ToString());
                    if (dvAdStatistics.Count == 0)
                    {
                        ///a.	        When email is sent, update the viewed count for the ad in the Ads table. 
                        dat.Execute("UPDATE Ads SET NumCurrentViews = NumCurrentViews + 1 WHERE Ad_ID=" + dvAds[j]["Ad_ID"].ToString());

                        string locOnly = "'False'";
                        if (categories.Trim() == "")
                            locOnly = "'True'";

                        ///b.	        Update the AdStatistics table with the ad and the manner the ad was seen: 
                        /////which will be Email not site view.
                        //[UserID],[Reason],[LocationOnly],[Date],[AdID]
                        dat.Execute("INSERT INTO AdStatistics (UserID, Reason, LocationOnly, Date, AdID, WasEmail) VALUES(" + dv[0]["UserID"].ToString() +
                            ", '" + categories + "', " + locOnly + ", '" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).ToString() + "', " +
                            dvAds[j]["Ad_ID"].ToString() + ", 'True')");
                    }
                }
            }

            if (!bool.Parse(dvAdsOne[0]["BigAd"].ToString()))
            {
                email += GetEmailString(dvAdsOne[0]["Header"].ToString(), dvAdsOne[0]["FeaturedPicture"].ToString(),
                    dvAdsOne[0]["Ad_ID"].ToString(), dvAdsOne[0]["FeaturedSummary"].ToString(),
                    dvAdsOne[0]["UserName"].ToString(), bool.Parse(dvAdsOne[0]["BigAd"].ToString()),
                    ref normalAdCount, dvAdsOne[0]["Template"].ToString());

                //Only when this ad is not found in adstatistics of the user do we increment the count
                dvAdStatistics =
                    dat.GetDataDV("SELECT * FROM AdStatistics WHERE UserID=" + dv[0]["UserID"].ToString() +
                    " AND AdID=" + dvAdsOne[0]["Ad_ID"].ToString());

                if (dvAdStatistics.Count == 0)
                {
                    ///a.	        When email is sent, update the viewed count for the ad in the Ads table. 
                    dat.Execute("UPDATE Ads SET NumCurrentViews = NumCurrentViews + 1 WHERE Ad_ID=" + dvAdsOne[0]["Ad_ID"].ToString());


                    string locOnly = "'False'";
                    if (categories.Trim() == "")
                        locOnly = "'True'";
                    ///b.	        Update the AdStatistics table with the ad and the manner the ad was seen: 
                    /////which will be Email not site view.
                    //[UserID],[Reason],[LocationOnly],[Date],[AdID]
                    dat.Execute("INSERT INTO AdStatistics (UserID, Reason, LocationOnly, Date, AdID, WasEmail) VALUES(" + dv[0]["UserID"].ToString() +
                        ", '" + categories + "', " + locOnly + ", '" +
                        DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).ToString() +
                        "', " + dvAdsOne[0]["Ad_ID"].ToString() + ", 'True')");

                    //update the ad count for the algorithm
                    sentAdCount++;
                }
            }



            email += "</table>";

            ///3.	        Clear the list of ads in EmailAdList column

            dat.Execute("UPDATE SavedAdSearches SET EmailAdList='', CountInEmailList=0 WHERE ID=" + searchID);

            return email;
        }
        else
        {
            return "";
        }
    }

    protected string GetEmailString(string header, string picture, string ID, string description, 
        string userName, bool isBig, ref int normalAdCount, string templateID)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        string adtitle = "style=\"color: White;font-weight: bold;font-family: Arial;font-size: 14px;margin-top: 10px;text-decoration: none;\"";
        string adbody = "style=\"color: #cccccc;font-family: Arial;font-size: 12px;line-height: 20px;\"";
        string readmorelink = "style=\"font-family: Arial; font-weight: bold;color: #1fb6e7; font-size: 12px;text-decoration: none;\"";
        string email = "";

        string w = "";
        string h = "";

        if (isBig)
        {
            email += "<tr><td colspan=\"2\"><div style=\"float: left;width: 419px;margin: 10px;  height: 206px; background-color: #686868;\">" +
            "<div class=\"topDiv\"  style=\"padding-top: 5px; clear: both; padding-left: 5px; background-color: #686868;\">";
            if (picture == null)
            {
                email += "</div>" +

                 "<div style=\"float: left;padding-top: 5px; padding-left: 3px;\">";
            }
            else
            {
                if (templateID == "1")
                {
                    email += "<div style=\"float: left;\"><table width='212px' height='190px' " +
                        " cellpadding=\"0\" cellspacing=\"0\"><tbody align=\"center\"><tr>" +
                        "<td valign=\"middle\">";

                    GetAdSize(out w, out h, picture, templateID, isBig);

                    if (ID == null)
                    {
                        email += "<a " + adtitle + "><img width='" + w + "' height='" + h + "' style=\"border: 0;\" alt=\"" +
                            header + "\" " + "name=\"" + header + "\" src=\"./UserFiles/" + userName + "/" +
                        picture + "\" /></a></td></tr></tbody></table></div>" + "" +
                         "<div style=\"text-align: center; padding-top: 5px; padding-right: 3px; padding-left: 3px; float: left;\">";
                    }
                    else
                    {
                        email += "<a href=\"http://HippoHappenings.com/" + dat.MakeNiceName(header) + "_" + ID + "_Ad" +
                      "\" " + adtitle + ">" + dat.BreakUpString(header, 10) + "><img width='" + w + "' height='" + h + "'  style=\"border: 0;\" alt=\"" +
                            header + "\" " + "name=\"" + header + "\" src=\"http://hippohappenings.com/UserFiles/" + userName + "/" +
                        picture + "\" /></a></td></tr></tbody></table></div>" + "" +
                         "<div style=\"text-align: center; padding-top: 5px; padding-left: 3px; float: left; padding-right: 3px;\">";
                    }

                }
                else
                {
                    GetAdSize(out w, out h, picture, templateID, isBig);
                    if (ID == null)
                    {
                        email += "<div style=\"float: left;\"><table width='" + w + "' height='" + h + "' " +
                            " cellpadding=\"0\" cellspacing=\"0\"><tbody align=\"center\"><tr>" +
                            "<td valign=\"middle\"><img width='" + w + "' height='" + h + "'  alt=\"" +
                            header + "\" " + "name=\"" + header + "\" src=\"./UserFiles/" + userName + "/" +
                        picture + "\" /></td></tr></tbody></table></div>";
                    }
                    else
                    {
                        email += "<div style=\"float: left;\"><table width='" + w + "' height='" + h + "' " +
                            "bgcolor=\"black\" cellpadding=\"0\" cellspacing=\"0\"><tbody align=\"center\"><tr>" +
                            "<td valign=\"middle\"><a href=\"http://HippoHappenings.com/" + dat.MakeNiceName(header) + "_" + ID + "_Ad" +
                      "\" " + adtitle + "><img style=\"border: 0;\" width='" + w + "' height='" + h + "'  alt=\"" +
                            header + "\" " + "name=\"" + header + "\" src=\"http://hippohappenings.com/UserFiles/" + userName + "/" +
                        picture + "\" /></a></td></tr></tbody></table></div>";
                    }
                }
            }

            if (templateID == "1")
            {
                if (ID == null)
                {
                    email += "<a " + adtitle + ">" + dat.BreakUpString(header, 21) + "</a><br/>";
                }
                else
                {
                    email += "<a href=\"http://HippoHappenings.com/" + dat.MakeNiceName(header) + "_" + ID + "_Ad" +
                      "\" " + adtitle + ">" + dat.BreakUpString(header, 10) + "</a><br/>";
                }

                email += "<label " + adbody + ">" + description + "</label>";

                if (ID == null)
                {
                    email += "<div style=\"float: right; padding-right: 8px; height: 50px; width: 80px; padding-top: 10px;\">" +
                                 "<div style=\"float: left;padding-top: 5px; clear: none;\"><img src=\"http://HippoHappenings.com/image/ReadMoreArrow.png\" /></div>" +
                                 "<div style=\"float: right;clear: none;\"><a " + readmorelink + ">Read More</a>";
                }
                else
                {
                    email += "<div style=\"float: right; padding-right: 8px; height: 50px; width: 80px; padding-top: 10px;\">" +
                                 "<div style=\"float: left;padding-top: 5px; clear: none;\"><img src=\"http://HippoHappenings.com/image/ReadMoreArrow.png\" /></div>" +
                                 "<div style=\"float: right;clear: none;\"><a " + readmorelink + " href=\"http://HippoHappenings.com/" + dat.MakeNiceName(header) + "_" + ID + "_Ad\">Read More</a>";

                }

                email += "</div></div>" +
                 "</div>" +
                 "</div>" +
              "</div></td></tr>";
            }
        }
        else
        {
            if (normalAdCount % 2 == 0)
            {
                email += "<tr><td>";
            }
            else
            {
                email += "<td>";
            }
            email += "<div style=\"float: left;margin: 10px; width: 214px; height: 268px; "+
                "\">" +
        "<div style=\"float: left;width: 214px; height: 268px; background-color: Transparent; background: url('http://hippohappenings.com/image/AdBackgroundWithShadow.png'); background-repeat: repeat-x;\">" +
            "<div class=\"topDiv\" style=\";padding-top: 10px; clear: both; padding-left: 10px;\">";

            if (picture == null)
            {
                email += "";
            }
            else
            {
                
                GetAdSize(out w, out h, picture, templateID, isBig);

                if (templateID == "1")
                {
                    email += "<div style=\"float: left;\"><table width=\"100px\" height=\"100px\" " +
                        "  cellpadding=\"0\" cellspacing=\"0\"><tbody align=\"center\">" +
                        "<tr><td valign=\"middle\">";
                }
                else if (templateID == "2")
                {
                    email += "<div style=\"float: left;\"><table width=\"200px\" height=\"140px\" " +
                        "  cellpadding=\"0\" cellspacing=\"0\"><tbody align=\"center\">" +
                        "<tr><td valign=\"middle\">";
                }
                else
                {
                    email += "<div style=\"float: left;\"><table width=\"200px\" height=\"200px\" " +
                        "  cellpadding=\"0\" cellspacing=\"0\"><tbody align=\"center\">"+
                        "<tr><td valign=\"middle\">";
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
                if (templateID == "1" || templateID == "2")
                {
                    email += "<div style=\"text-align: center; padding-left: 3px; padding-right: 3px;\"><a " + adtitle + ">" +
                        dat.BreakUpString(header, 10) + "</a><br/>" +
                    "</div>" +
                "</div>" +
                "<div align=\"left\" style=\"clear: both; padding-left: 12px; padding-right: 8px;\">" +
                "<label " + adbody + ">" + description + "</label>" +
                "</div>" +
                "<div style=\"float: right; padding-right: 8px; height: 50px; width: 80px; padding-top: 10px;\">" +
                    "<div style=\"float: left;padding-top: 5px; clear: none;\"><img src=\"http://HippoHappenings.com/image/ReadMoreArrow.png\" /></div>" +
                    "<div style=\"float: right;clear: none;\"><a " + readmorelink + " >Read More</a>" +
                    "</div></div></div></div>";
                }
            }
            else
            {
                if (templateID == "1" || templateID == "2")
                {
                    email += "<div style=\"float: left;\"><a href=\"http://HippoHappenings.com/" + dat.MakeNiceName(header) +
                        "_" + ID + "_Ad\" " + adtitle + ">" + dat.BreakUpString(header, 10) + "</a><br/>" +
                    "</div>" +
                "</div>" +
                "<div align=\"left\" style=\"clear: both; padding-left: 12px; padding-right: 8px;\">" +
                "<label " + adbody + ">" + description + "</label>" +
                "</div>" +
                "<div style=\"float: right; padding-right: 8px; height: 50px; width: 80px; padding-top: 10px;\">" +
                    "<div style=\"float: left;padding-top: 5px; clear: none;\"><img src=\"http://HippoHappenings.com/image/ReadMoreArrow.png\" /></div>" +
                    "<div style=\"float: right;clear: none;\"><a " + readmorelink + " href=\"http://HippoHappenings.com/" + dat.MakeNiceName(header) + "_" + ID + "_Ad\">Read More</a>" +
                    "</div></div></div></div>";
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
        }
            return email;
    }

    protected void GetAdSize(out string w, out string h, string picture, string templateID, bool isBig)
    {
        System.Drawing.Image image =
            System.Drawing.Image.FromFile(MapPath(".")+"\\UserFiles\\" +
            Session["UserName"].ToString() + "\\" + picture);

        int height = 100;
        int width = 100;
        if (isBig)
        {
            if (templateID == "1")
            {
                height = 190;
                width = 212;
            }
            else
            {
                height = 190;
                width = 415;
            }
        }
        else
        {
            if (templateID == "2")
            {
                height = 140;
                width = 200;
            }
            else if (templateID == "3")
            {
                height = 250;
                width = 200;
            }
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
        //#region Create Categories

        //Hashtable distinctHash = new Hashtable();
        //Hashtable tagHash = new Hashtable();
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        GetCheckedCategories(ref CategoryTree, ID);
        GetCheckedCategories(ref RadTreeView1, ID);
        GetCheckedCategories(ref RadTreeView2, ID);
        GetCheckedCategories(ref RadTreeView3, ID);

    }

    protected void GetCheckedCategories(ref Telerik.Web.UI.RadTreeView CategoryTree, string ID)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        List<Telerik.Web.UI.RadTreeNode> list = (List<Telerik.Web.UI.RadTreeNode>)CategoryTree.GetAllNodes();

        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        string tagsize = "22";

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

            //foreach (Telerik.Web.UI.RadTreeNode node1 in node.Nodes)
            //{
            //    if (node1.Checked)
            //    {
                    

            //        dat.Execute("INSERT INTO Ad_Category_Mapping (CategoryID, AdID, tagSize) VALUES("
            //                    + node1.Value + ", " + ID + ", 16)");
            //    }
            //}
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

        list = (List<Telerik.Web.UI.RadTreeNode>)RadTreeView1.GetAllNodes();

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

        list = (List<Telerik.Web.UI.RadTreeNode>)RadTreeView3.GetAllNodes();

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

        return categories.Remove(categories.Length - 4, 4);
    }

    private void ChangeSelectedTab(int unselectIndex, short selectIndex)
    {
        Session["PrevTab"] = int.Parse(selectIndex.ToString());
        AdTabStrip.Tabs[selectIndex].Enabled = true;
        AdTabStrip.Tabs[selectIndex].Selected = true;
        AdTabStrip.MultiPage.SelectedIndex = selectIndex;
        AdTabStrip.SelectedTab.TabIndex = selectIndex;
        AdTabStrip.SelectedIndex = selectIndex;
        AdTabStrip.TabIndex = selectIndex;
        //AdTabStrip.Tabs[unselectIndex].Enabled = false;
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
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            MessagePanel.Visible = true;
            CategoriesTextBox.Text = dat.stripHTML(CategoriesTextBox.Text.Trim());
            YourMessagesLabel.Text = "<br/><br/>Your category '" + CategoriesTextBox.Text +
                "' has been suggested. We'll send you an update when it has been approved.";


            CategoriesTextBox.Text = dat.StripHTML_LeaveLinks(CategoriesTextBox.Text);

            DataSet dsUser = dat.GetData("SELECT EMAIL, UserName FROM USERS WHERE User_ID=" +
                Session["User"].ToString());

            dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["categoryemail"],
                    System.Configuration.ConfigurationManager.AppSettings["emailName"],
                System.Configuration.ConfigurationManager.AppSettings["categoryemail"].ToString(),
                "Category has been suggested from 'PostAnAd.aspx'. The user ID who suggested " +
                "the category is userID: '" + Session["User"].ToString() + "'. The category suggestion is '" +
                CategoriesTextBox.Text + "'", "Hippo Happenings category suggestion");

            CategoriesTextBox.Text = "";
        }
        else
        {
            YourMessagesLabel.Text = "<br/><br/>Please include the category.";
            MessagePanel.Visible = true;
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

        if (CountryDropDown.SelectedValue != "223")
        {
            RadiusPanel.Visible = false;
        }
        else
        {
            RadiusPanel.Visible = true;
        }
    }
    
    protected void CalculatePrice(object sender, EventArgs e)
    {
        CalcPrice();
    }

    protected void CalcPrice()
    {
        YourMessagesLabel.Text = "";
        MessagePanel.Visible = false;
        int users = 0;

        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }

        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        DataView dvAdControl = dat.GetDataDV("SELECT * FROM AdControl");
        if (bool.Parse(dvAdControl[0]["areAdsFree"].ToString()))
        {
            YourPriceLabel.Text = "$0.00!!!";
        }
        else
        {
            try
            {
                users = int.Parse(UsersTextBox.Text);

                decimal usersDec = decimal.Parse(users.ToString());

                decimal price = usersDec * decimal.Parse(AdPlacementList.SelectedValue);

                YourPriceLabel.Text = "$" + price;
            }
            catch (Exception ex)
            {
                UsersTextBox.Text = "";
                YourMessagesLabel.Text = "<br/><br/>The number of users must be a whole number.";
                MessagePanel.Visible = true;
            }
        }
    }

    protected void ChangePrice(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        try
        {
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            DataView dvAdControl = dat.GetDataDV("SELECT * FROM AdControl");
           
            if (AdPlacementList.SelectedValue == "0.01")
            {
                RadToolTip2.Text = "<label>Your price will be calulated based on the number of " +
                "users you want your ad to be displayed to. Your ad will stop running " +
                "when that particular number of users have seen your ad. The price for " +
                "each user is $0.01, since you have chosen a 'Normal Ad Space'. " +
                "For example, if you choose 1000 users, your cost will be $10.</label>";

                if (bool.Parse(dvAdControl[0]["areAdsFree"].ToString()))
                {
                    FreeLabel.Text = "Choose hoose how many users you want to see your ad. " +
                        " <span style='color: #ff770d;'>[As the ads are now free, your limit on the number of users you can choose is " +
                        dvAdControl[0]["userLimit"].ToString() + ".]</span><br/>Your price will be calulated based on the number of " +
                "users you want your ad to be displayed to. Your ad will stop running " +
                "when that particular number of users have seen your ad. The price for " +
                "each user is $0.01, since you have chosen a 'Normal Ad Space'. " +
                "For example, if you choose 1000 users, your cost will be $10.";
                    FreeValidator.MaximumValue = dvAdControl[0]["userLimit"].ToString();
                    FreeValidator.ErrorMessage = "User count can only be between 0 and " +
                        dvAdControl[0]["userLimit"].ToString() + ".";
                }
                else
                {
                    CalcPrice();
                    //GetAvailableUsers((DateTime)Session["SelectedStartDate"], false);

                }
            }
            else
            {
                RadToolTip2.Text = "<label>Your price will be calulated based on the number of " +
                "users you want your ad to be displayed to. Your ad will stop running " +
                "when that particular number of users have seen your ad. The price for " +
                "each user is $0.04, since you have chosen the 'Big Ad Space'. " +
                "For example, if you choose 1000 users, your cost will be $40.</label>";
                if (bool.Parse(dvAdControl[0]["areAdsFree"].ToString()))
                {
                    FreeLabel.Text = "Choose how many users you want to see your ad. " +
                        " <span style='color: #ff770d;'>[As the ads are now free, your limit on the number of users you can choose is " +
                        dvAdControl[0]["bigUserLimit"].ToString() + ".]</span><br/>Your price will be calulated based on the number of " +
                "users you want your ad to be displayed to. Your ad will stop running " +
                "when that particular number of users have seen your ad. The price for " +
                "each user is $0.04, since you have chosen the 'Big Ad Space'. " +
                "For example, if you choose 1000 users, your cost will be $40.";
                    FreeValidator.MaximumValue = dvAdControl[0]["bigUserLimit"].ToString();
                    FreeValidator.ErrorMessage = "User count can only be between 0 and " + 
                        dvAdControl[0]["bigUserLimit"].ToString() + ".";
                }
                else
                {
                    CalcPrice();
                    //GetAvailableUsers((DateTime)Session["SelectedStartDate"], true);
                }
            }
        }
        catch (Exception ex)
        {
            MessagePanel.Visible = true;
            YourMessagesLabel.Text = ex.ToString();
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
            width = 415;
            height = 190;
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

    //protected void OpenMeasles(object sender, EventArgs e)
    //{
    //    MessageRadWindowManager.Visible = true;
    //    RadWindow1.Visible = true;
        
    //}

    protected void InsertBreak(object sender, EventArgs e)
    {
        DescriptionTextBox.Content += "<br/>";
    }

    protected void ChangeFeaturedText(object sender, EventArgs e)
    {
        if (AdPlacementList.SelectedValue == "0.01")
        {
            SmallPanel.Visible = true;
            BigPanel.Visible = false;

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
        else
        {
            BigPanel.Visible = true;
            SmallPanel.Visible = false;

            if (RadioButtonList1.SelectedValue == "1")
            {
                FeaturedTextLiteral.Text = "max 250 characters &nbsp;&nbsp;&nbsp;";
                SummaryTextBox.Attributes.Add("onkeyup", "CountChars()");
                FeaturedTextPanel.Visible = true;
                FeaturedTextNotAllowedPanel.Visible = false;
            }
            else
            {
                FeaturedTextPanel.Visible = false;
                FeaturedTextNotAllowedPanel.Visible = true;
            }
        }
    }
}