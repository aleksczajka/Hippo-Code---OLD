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

public partial class UserPreferences : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlMeta hm = new HtmlMeta();
        HtmlHead head = (HtmlHead)Page.Header;
        hm.Name = "ROBOTS";
        hm.Content = "NOINDEX, FOLLOW";
        head.Controls.AddAt(0, hm);

        

        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        MessageRadWindowManager.VisibleOnPageLoad = false;
        if (!IsPostBack)
        {
            
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
                    Session["User"] = authTicket.Name;
                    Session["UserName"] = dat.GetData("SELECT UserName FROM USERS WHERE User_ID=" + Session["User"].ToString()).Tables[0].Rows[0]["UserName"].ToString();
                }
                else
                {
                    Button calendarLink = (Button)dat.FindControlRecursive(this, "CalendarLink");
                    calendarLink.Visible = false;
                    Response.Redirect("~/UserLogin.aspx");
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("~/UserLogin.aspx");
            }

            string USER_ID = Session["User"].ToString();

            //DataSet dsCat = dat.GetData("SELECT * FROM Categories");
            //CategoriesCheckBoxes.DataSource = dsCat;
            //CategoriesCheckBoxes.DataTextField = "CategoryName";
            //CategoriesCheckBoxes.DataValueField = "ID";
            //CategoriesCheckBoxes.DataBind();

            CategoryTree.DataBind();
            RadTreeView1.DataBind();
            RadTreeView2.DataBind();
            RadTreeView3.DataBind();

            DataSet dsCategories = dat.GetData("SELECT * FROM UserCategories WHERE UserID=" + USER_ID);
            if (dsCategories.Tables.Count > 0)
                for (int i = 0; i < dsCategories.Tables[0].Rows.Count; i++)
                {
                    Telerik.Web.UI.RadTreeNode node = (Telerik.Web.UI.RadTreeNode)CategoryTree.Nodes.FindNodeByValue(dsCategories.Tables[0].Rows[i]["CategoryID"].ToString());
                    if (node != null)
                    {
                        node.Checked = true;
                        //node.Enabled = false;
                    }
                    else
                    {

                        node = (Telerik.Web.UI.RadTreeNode)RadTreeView1.Nodes.FindNodeByValue(dsCategories.Tables[0].Rows[i]["CategoryID"].ToString());
                        if (node != null)
                        {
                            node.Checked = true;
                            //node.Enabled = false;
                        }
                        else
                        {

                            node = (Telerik.Web.UI.RadTreeNode)RadTreeView2.Nodes.FindNodeByValue(dsCategories.Tables[0].Rows[i]["CategoryID"].ToString());
                            if (node != null)
                            {
                                node.Checked = true;
                                //node.Enabled = false;
                            }
                            else
                            {
                                node = (Telerik.Web.UI.RadTreeNode)RadTreeView3.Nodes.FindNodeByValue(dsCategories.Tables[0].Rows[i]["CategoryID"].ToString());
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

           
           

            DataSet dsProvider = dat.GetData("SELECT * FROM PhoneProviders");
            ProviderDropDown.DataSource = dsProvider;
            ProviderDropDown.DataTextField = "Provider";
            ProviderDropDown.DataValueField = "ID";
            ProviderDropDown.DataBind();


            Data d = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            DataSet ds = d.GetData("SELECT * FROM Events E, Venues V, Event_Occurance EO, User_Calendar UC WHERE UC.EventID=E.ID AND E.ID=EO.EventID AND E.Venue=V.ID AND UC.UserID=" + USER_ID);
            DataSet dsUser = d.GetData("SELECT * FROM Users WHERE User_ID=" + USER_ID);
            DataSet dsUserPrefs = d.GetData("SELECT * FROM UserPreferences WHERE UserID=" + USER_ID);

            if (dsUserPrefs.Tables.Count > 0)
                if (dsUserPrefs.Tables[0].Rows.Count > 0)
                {
                    AgeTextBox.Text = dsUserPrefs.Tables[0].Rows[0]["Age"].ToString();
                    SexTextBox.Text = dsUserPrefs.Tables[0].Rows[0]["Sex"].ToString();

                    LocationTextBox.Text = dsUserPrefs.Tables[0].Rows[0]["Location"].ToString();

                    string eventsPosted = "0";
                    DataSet dsEvents = dat.GetData("SELECT COUNT(*) AS COUNT1 FROM Events WHERE UserName='" + Session["UserName"].ToString() + "'");
                    if (dsEvents.Tables.Count > 0)
                        if (dsEvents.Tables[0].Rows.Count > 0)
                            eventsPosted = dsEvents.Tables[0].Rows[0]["COUNT1"].ToString();
                    EventsLabel.Text = eventsPosted;
                    AttendedLabel.Text = dsUserPrefs.Tables[0].Rows[0]["EventsAttended"].ToString();

                    if (dsUserPrefs.Tables[0].Rows[0]["CalendarPrivacyMode"].ToString() != null)
                    {
                        PublicPrivateCheckList.SelectedValue = dsUserPrefs.Tables[0].Rows[0]["CalendarPrivacyMode"].ToString();
                    }

                    if (dsUserPrefs.Tables[0].Rows[0]["PollPreferences"].ToString() != null)
                    {
                        PollRadioList.SelectedValue = dsUserPrefs.Tables[0].Rows[0]["PollPreferences"].ToString();
                    }

                    if (dsUserPrefs.Tables[0].Rows[0]["CommentsPreferences"].ToString() != null)
                    {
                        CommentsRadioList.SelectedValue = dsUserPrefs.Tables[0].Rows[0]["CommentsPreferences"].ToString();
                    }

                    if (dsUserPrefs.Tables[0].Rows[0]["CategoriesOnOff"].ToString() != null)
                    {
                        if(bool.Parse(dsUserPrefs.Tables[0].Rows[0]["CategoriesOnOff"].ToString()))
                            CategoriesOnOffRadioList.SelectedValue = "1";
                        else
                            CategoriesOnOffRadioList.SelectedValue = "2";
                    }

                    if (dsUserPrefs.Tables[0].Rows[0]["CommunicationPrefs"].ToString() != null)
                    {
                        CommunicationPrefsRadioList.SelectedValue = dsUserPrefs.Tables[0].Rows[0]["CommunicationPrefs"].ToString();
                    }


                    if (dsUserPrefs.Tables[0].Rows[0]["Country"].ToString() != null)
                    {
                        if (dsUserPrefs.Tables[0].Rows[0]["Country"].ToString().Trim() != "")
                        {
                            if (dsUserPrefs.Tables[0].Rows[0]["Address"].ToString() != null)
                            {
                                AddressTextBox.THE_TEXT = dsUserPrefs.Tables[0].Rows[0]["Address"].ToString();
                            }

                            if (dsUserPrefs.Tables[0].Rows[0]["City"].ToString() != null)
                            {
                                BillCityTextBox.THE_TEXT = dsUserPrefs.Tables[0].Rows[0]["City"].ToString();
                            }

                            if (dsUserPrefs.Tables[0].Rows[0]["ZIP"].ToString() != null)
                            {
                                ZipTextBox.THE_TEXT = dsUserPrefs.Tables[0].Rows[0]["ZIP"].ToString();
                            }

                            BillCountryDropDown.SelectedValue = dsUserPrefs.Tables[0].Rows[0]["Country"].ToString();

                            DataSet dsStates = dat.GetData("SELECT * FROM State WHERE country_id=" + dsUserPrefs.Tables[0].Rows[0]["Country"].ToString());

                            bool isText = false;
                            if (dsStates.Tables.Count > 0)
                                if (dsStates.Tables[0].Rows.Count > 0)
                                {
                                    BillStateDropDown.DataSource = dsStates;
                                    BillStateDropDown.DataTextField = "state_2_code";
                                    BillStateDropDown.DataValueField = "state_id";
                                    BillStateDropDown.DataBind();
                                    BillStateDropDown.Items.Insert(0, new ListItem("Select State..", "-1"));

                                    if (dsUserPrefs.Tables[0].Rows[0]["State"].ToString() != null)
                                    {
                                        ListItem a = BillStateDropDown.Items.FindByText(dsUserPrefs.Tables[0].Rows[0]["State"].ToString());
                                        if (a != null)
                                            BillStateDropDown.SelectedValue = a.Value;
                                    }

                                    BillStateDropPanel.Visible = true;
                                    BillStateTextPanel.Visible = false;
                                }
                                else
                                {
                                    isText = true;
                                }
                            else
                                isText = true;

                            if (isText)
                            {
                                if (dsUserPrefs.Tables[0].Rows[0]["State"].ToString() != null)
                                {
                                    BillStateTextBox.THE_TEXT = dsUserPrefs.Tables[0].Rows[0]["State"].ToString();
                                }
                                BillStateTextPanel.Visible = true;
                                BillStateDropPanel.Visible = false;
                            }
                        }
                    }
                    if (dsUserPrefs.Tables[0].Rows[0]["CatCountry"].ToString() != null)
                    {
                        if (dsUserPrefs.Tables[0].Rows[0]["CatCountry"].ToString().Trim() != "")
                        {
                            if (dsUserPrefs.Tables[0].Rows[0]["CatCity"].ToString() != null)
                            {
                                CityTextBox.THE_TEXT = dsUserPrefs.Tables[0].Rows[0]["CatCity"].ToString();
                            }


                            CountryDropDown.SelectedValue = dsUserPrefs.Tables[0].Rows[0]["CatCountry"].ToString();

                            DataSet dsStates = dat.GetData("SELECT * FROM State WHERE country_id=" + dsUserPrefs.Tables[0].Rows[0]["CatCountry"].ToString());

                            bool isText = false;
                            if (dsStates.Tables.Count > 0)
                                if (dsStates.Tables[0].Rows.Count > 0)
                                {
                                    StateDropDown.DataSource = dsStates;
                                    StateDropDown.DataTextField = "state_2_code";
                                    StateDropDown.DataValueField = "state_id";
                                    StateDropDown.DataBind();

                                    if (dsUserPrefs.Tables[0].Rows[0]["CatState"] != null)
                                    {
                                        StateDropDown.SelectedValue = StateDropDown.Items.FindByText(dsUserPrefs.Tables[0].Rows[0]["CatState"].ToString()).Value;
                                    }

                                    StateDropDownPanel.Visible = true;
                                    StateTextBoxPanel.Visible = false;
                                }
                                else
                                {
                                    isText = true;
                                }
                            else
                                isText = true;

                            if (isText)
                            {
                                if (dsUserPrefs.Tables[0].Rows[0]["CatState"].ToString() != null)
                                {
                                    StateTextBox.THE_TEXT = dsUserPrefs.Tables[0].Rows[0]["CatState"].ToString();
                                }
                                StateTextBoxPanel.Visible = true;
                                StateDropDownPanel.Visible = false;
                            }
                        }
                    }
                    if (dsUserPrefs.Tables[0].Rows[0]["TextingPrefs"].ToString() != null)
                    {
                        if (dsUserPrefs.Tables[0].Rows[0]["TextingPrefs"].ToString().Contains("1"))
                            TextingCheckBoxList.Items[0].Selected = true;
                        if (dsUserPrefs.Tables[0].Rows[0]["TextingPrefs"].ToString().Contains("2"))
                            TextingCheckBoxList.Items[1].Selected = true;
                        if (dsUserPrefs.Tables[0].Rows[0]["TextingPrefs"].ToString().Contains("3"))
                            TextingCheckBoxList.Items[2].Selected = true;
                    }

                    if (dsUserPrefs.Tables[0].Rows[0]["EmailPrefs"].ToString() != null)
                    {
                        if (dsUserPrefs.Tables[0].Rows[0]["EmailPrefs"].ToString().Contains("1"))
                            EmailCheckList.Items[0].Selected = true;
                        if (dsUserPrefs.Tables[0].Rows[0]["EmailPrefs"].ToString().Contains("2"))
                            EmailCheckList.Items[1].Selected = true;
                        if (dsUserPrefs.Tables[0].Rows[0]["EmailPrefs"].ToString().Contains("3"))
                            EmailCheckList.Items[2].Selected = true;
                    }
                }

            DataSet dsComments = d.GetData("SELECT * FROM User_Comments CU, Users U WHERE CU.CommenterID=U.User_ID AND CU.UserID=" + USER_ID.ToString());
            UserNameLabel.Text = dsUser.Tables[0].Rows[0]["UserName"].ToString();

            if (dsUser.Tables[0].Rows[0]["Email"].ToString() != null)
            {
                EmailTextBox.Text = dsUser.Tables[0].Rows[0]["Email"].ToString();
            }

            if (dsUser.Tables[0].Rows[0]["PhoneNumber"].ToString() != null)
            {
                PhoneTextBox.THE_TEXT = dsUser.Tables[0].Rows[0]["PhoneNumber"].ToString();
            }

            if (dsUser.Tables[0].Rows[0]["PhoneProvider"].ToString() != null)
            {
                ProviderDropDown.SelectedValue = dsUser.Tables[0].Rows[0]["PhoneProvider"].ToString();
            }

            if (dsUser.Tables[0].Rows[0]["ProfilePicture"].ToString() != null)
            {
                if (System.IO.File.Exists(Server.MapPath(".") + "/UserFiles/" + dsUser.Tables[0].Rows[0]["UserName"].ToString() + "/Profile/" + dsUser.Tables[0].Rows[0]["ProfilePicture"].ToString()))
                {
                    FriendImage.ImageUrl = Server.MapPath(".") + "/UserFiles/" + dsUser.Tables[0].Rows[0]["UserName"].ToString() + "/Profile/" + dsUser.Tables[0].Rows[0]["ProfilePicture"].ToString();
                    Session["ProfilePicture"] = dsUser.Tables[0].Rows[0]["ProfilePicture"].ToString();
                }
                else
                {
                    FriendImage.ImageUrl = "NewImages/NoAvatar150.jpg";
                }
            }
            else
                FriendImage.ImageUrl = "NewImages/NoAvatar150.jpg";
        }
        else
        {
            if (Session["User"] == null)
            {
                Session.Abandon();
                Response.Redirect("~/UserLogin.aspx");
                
            }
        }

        DataSet dsVenues = dat.GetData("SELECT * FROM UserVenues UV, Venues V WHERE V.ID=UV.VenueID AND UV.UserID=" + Session["User"].ToString());

        VenuesRadPanel.Items[0].Text = "<div style=\"border-bottom: dotted 1px black; background-color: #1b1b1b; "+
            "\"><label class=\"PreferencesTitle\">Favorite Venues</label><span " +
            "style=\"font-style: italic; font-family: Arial; font-size: 14px; color: #666666;\">"+
            " (Click to drop down the list. Un-check to remove a venue from the favorites list.)</span></div>";

        //"<div style=\"border-bottom: dotted 1px black; " +
        //    "padding-top: 10px;\"><label class=\"PreferencesTitle\">Favorite Venues</label><span " +
        //    "style=\"font-style: italic; font-family: Arial; font-size: 14px; color: #666666;\">" +
        //    "(You can add these from the <a href=\"VenueSearch.aspx\" class=\"AddLink\">Venues Page</a>. " +
        //    "Un-check to remove a venue from the favorites list.)</span></div>";


        CheckBoxList VenueCheckBoxes = new CheckBoxList();
        VenueCheckBoxes.Width = 600;
        VenueCheckBoxes.CssClass = "VenueCheckBoxes";
        VenueCheckBoxes.ID = "VenueCheckBoxes";
        VenueCheckBoxes.RepeatColumns = 4;
        VenueCheckBoxes.RepeatDirection = RepeatDirection.Horizontal;

        VenueCheckBoxes.DataSource = dsVenues;
        VenueCheckBoxes.DataTextField = "NAME";
        VenueCheckBoxes.DataValueField = "ID";
        VenueCheckBoxes.DataBind();

        for (int i = 0; i < VenueCheckBoxes.Items.Count; i++)
        {
            VenueCheckBoxes.Items[i].Selected = true;
        }

        if (VenueCheckBoxes.Items.Count == 0)
        {
            Label label = new Label();
            label.CssClass = "Paddings20 VenueCheckBoxes";
            label.Text = "You have no venues specifieds as your favorite. To add venues as your favorites search for them on the <a href=\"VenueSearch.aspx\" class=\"AddLink\">Venues Page</a>";
            VenuesRadPanel.Items[0].Items[0].Controls.Add(label);
        }
        else
        {
            VenuesRadPanel.Items[0].Items[0].Controls.Add(VenueCheckBoxes);
        }

        VenuesRadPanel.CollapseAllItems();
        
    }

    protected void UploadPhoto(object sender, EventArgs e)
    {
        if (PictureUpload.HasFile)
        {
            PictureUpload.SaveAs(MapPath(".").ToString() + "/UserFiles/" + UserNameLabel.Text + "/Profile/" + PictureUpload.FileName);
            FriendImage.ImageUrl = "UserFiles/" + UserNameLabel.Text + "/Profile/" + PictureUpload.FileName;
            Session["ProfilePicture"] = PictureUpload.FileName;
            

        }
    }

    protected void Save(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        try
        {
            Data d = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            if (d.ValidateEmail(EmailTextBox.Text))
            {
                string USER_ID = Session["User"].ToString();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Users SET ProfilePicture=@pic, Email=@email, PhoneNumber=@phone " +
                    ", PhoneProvider=@provider WHERE User_ID=@id ", conn);
                if (Session["ProfilePicture"] != null)
                    cmd.Parameters.Add("@pic", SqlDbType.NVarChar).Value = Session["ProfilePicture"].ToString();
                else
                    cmd.Parameters.Add("@pic", SqlDbType.NVarChar).Value = DBNull.Value;
                cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = EmailTextBox.Text;
                cmd.Parameters.Add("@phone", SqlDbType.Real).Value = d.RemoveNoneNumbers(PhoneTextBox.THE_TEXT);
                cmd.Parameters.Add("@provider", SqlDbType.Int).Value = ProviderDropDown.SelectedValue;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = USER_ID;
                cmd.ExecuteNonQuery();

                string prefs = "0";

                if (TextingCheckBoxList.Items[0].Selected)
                    prefs += "1";
                if (TextingCheckBoxList.Items[1].Selected)
                    prefs += "2";
                if (TextingCheckBoxList.Items[2].Selected)
                    prefs += "3";

                string emailPrefs = "0";

                if (EmailCheckList.Items[0].Selected)
                    emailPrefs += "1";
                if (EmailCheckList.Items[1].Selected)
                    emailPrefs += "2";
                if (EmailCheckList.Items[2].Selected)
                    emailPrefs += "3";

                string calendarPrefs = "";
                if (PublicPrivateCheckList.SelectedValue != null)
                    if (PublicPrivateCheckList.SelectedValue != "")
                        calendarPrefs = PublicPrivateCheckList.SelectedValue;

                string commPrefs = "";
                if (CommunicationPrefsRadioList.SelectedValue != null)
                    if (CommunicationPrefsRadioList.SelectedValue != "")
                        commPrefs = CommunicationPrefsRadioList.SelectedValue;

                string commentsPrefs = "";
                if (CommentsRadioList.SelectedValue != null)
                    if (CommentsRadioList.SelectedValue != "")
                        commentsPrefs = CommentsRadioList.SelectedValue;

                string pollPrefs = "";
                if (PollRadioList.SelectedValue != null)
                    if (PollRadioList.SelectedValue != "")
                        pollPrefs = PollRadioList.SelectedValue;

                string onoff = "";
                if (CategoriesOnOffRadioList.SelectedValue != null)
                    if (CategoriesOnOffRadioList.SelectedValue != "")
                        onoff = CategoriesOnOffRadioList.SelectedValue;

                cmd = new SqlCommand("UPDATE UserPreferences SET Age=@age, Sex=@sex, Location=@location, CalendarPrivacyMode=@calendarmode " +
                    ", CommunicationPrefs=@commPrefs, TextingPrefs=@textprefs, EmailPrefs=@email, Address=@address, City=@city, " +
                    " CommentsPreferences=@comments, PollPreferences=@poll, CategoriesOnOff=@onoff, State=@state, ZIP=@zip, Country=@country, " +
                    " CatCountry=@catCountry, CatState=@catState, CatCity=@catCity WHERE UserID=@id ", conn);
                cmd.Parameters.Add("@age", SqlDbType.NVarChar).Value = AgeTextBox.Text;
                cmd.Parameters.Add("@sex", SqlDbType.NVarChar).Value = SexTextBox.Text;
                cmd.Parameters.Add("@location", SqlDbType.NVarChar).Value = LocationTextBox.Text;
                if (onoff != "")
                    cmd.Parameters.Add("@onoff", SqlDbType.Int).Value = onoff;
                else
                    cmd.Parameters.Add("@onoff", SqlDbType.Int).Value = DBNull.Value;
                if (calendarPrefs != "")
                    cmd.Parameters.Add("@calendarmode", SqlDbType.Int).Value = calendarPrefs;
                else
                    cmd.Parameters.Add("@calendarmode", SqlDbType.Int).Value = DBNull.Value;
                if (commPrefs != "")
                    cmd.Parameters.Add("@commPrefs", SqlDbType.Int).Value = commPrefs;
                else
                    cmd.Parameters.Add("@commPrefs", SqlDbType.Int).Value = DBNull.Value;
                if (commentsPrefs != "")
                    cmd.Parameters.Add("@comments", SqlDbType.Int).Value = commentsPrefs;
                else
                    cmd.Parameters.Add("@comments", SqlDbType.Int).Value = DBNull.Value;
                if (pollPrefs != "")
                    cmd.Parameters.Add("@poll", SqlDbType.Int).Value = pollPrefs;
                else
                    cmd.Parameters.Add("@poll", SqlDbType.Int).Value = DBNull.Value;
                cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = AddressTextBox.THE_TEXT;
                cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = BillCityTextBox.THE_TEXT;
                if (BillCountryDropDown.SelectedValue != "-1")
                {
                    cmd.Parameters.Add("@country", SqlDbType.Int).Value = BillCountryDropDown.SelectedValue;

                    string state = "";
                    if (BillStateDropPanel.Visible)
                        state = BillStateDropDown.SelectedItem.Text;
                    else
                        state = BillStateTextBox.THE_TEXT;

                    cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = state;
                }
                else
                {
                    cmd.Parameters.Add("@country", SqlDbType.Int).Value = DBNull.Value;
                    cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = DBNull.Value;
                }


                if (CountryDropDown.SelectedValue != "-1")
                {
                    cmd.Parameters.Add("@catCountry", SqlDbType.Int).Value = CountryDropDown.SelectedValue;
                    Session["UserCountry"] = CountryDropDown.SelectedValue;

                    string state = "";
                    if (StateDropDownPanel.Visible)
                        state = StateDropDown.SelectedItem.Text;
                    else
                        state = StateTextBox.THE_TEXT;

                    if (state != "")
                    {
                        cmd.Parameters.Add("@catState", SqlDbType.NVarChar).Value = state;

                    }
                    else
                        cmd.Parameters.Add("@catState", SqlDbType.NVarChar).Value = DBNull.Value;
                    Session["UserState"] = state;
                    if (CityTextBox.THE_TEXT != "")
                    {
                        cmd.Parameters.Add("@catCity", SqlDbType.NVarChar).Value = CityTextBox.THE_TEXT;

                    }
                    else
                        cmd.Parameters.Add("@catCity", SqlDbType.NVarChar).Value = DBNull.Value;
                    Session["UserCity"] = CityTextBox.THE_TEXT;
                }
                else
                {
                    cmd.Parameters.Add("@catCountry", SqlDbType.Int).Value = DBNull.Value;
                    cmd.Parameters.Add("@catState", SqlDbType.NVarChar).Value = DBNull.Value;
                    cmd.Parameters.Add("@catCity", SqlDbType.NVarChar).Value = DBNull.Value;
                }



                cmd.Parameters.Add("@zip", SqlDbType.NVarChar).Value = ZipTextBox.THE_TEXT;
                cmd.Parameters.Add("@textprefs", SqlDbType.Int).Value = prefs;
                cmd.Parameters.Add("@email", SqlDbType.Int).Value = emailPrefs;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = USER_ID;
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("DELETE FROM UserCategories WHERE UserID=@user", conn);
                cmd.Parameters.Add("@user", SqlDbType.Int).Value = USER_ID;
                cmd.ExecuteNonQuery();

                for (int i = 0; i < CategoryTree.Nodes.Count; i++)
                {
                    if (CategoryTree.Nodes[i].Checked)
                    {
                        cmd = new SqlCommand("INSERT INTO UserCategories (UserID, CategoryID) VALUES(@user, @cat)", conn);
                        cmd.Parameters.Add("@user", SqlDbType.Int).Value = USER_ID;
                        cmd.Parameters.Add("@cat", SqlDbType.Int).Value = CategoryTree.Nodes[i].Value;
                        cmd.ExecuteNonQuery();
                    }
                    for (int n = 0; n < CategoryTree.Nodes[i].Nodes.Count; n++)
                    {
                        if (CategoryTree.Nodes[i].Nodes[n].Checked)
                        {
                            cmd = new SqlCommand("INSERT INTO UserCategories (UserID, CategoryID) VALUES(@user, @cat)", conn);
                            cmd.Parameters.Add("@user", SqlDbType.Int).Value = USER_ID;
                            cmd.Parameters.Add("@cat", SqlDbType.Int).Value = CategoryTree.Nodes[i].Nodes[n].Value;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                for (int i = 0; i < RadTreeView1.Nodes.Count; i++)
                {
                    if (RadTreeView1.Nodes[i].Checked)
                    {
                        cmd = new SqlCommand("INSERT INTO UserCategories (UserID, CategoryID) VALUES(@user, @cat)", conn);
                        cmd.Parameters.Add("@user", SqlDbType.Int).Value = USER_ID;
                        cmd.Parameters.Add("@cat", SqlDbType.Int).Value = RadTreeView1.Nodes[i].Value;
                        cmd.ExecuteNonQuery();
                    }
                    for (int n = 0; n < RadTreeView1.Nodes[i].Nodes.Count; n++)
                    {
                        if (RadTreeView1.Nodes[i].Nodes[n].Checked)
                        {
                            cmd = new SqlCommand("INSERT INTO UserCategories (UserID, CategoryID) VALUES(@user, @cat)", conn);
                            cmd.Parameters.Add("@user", SqlDbType.Int).Value = USER_ID;
                            cmd.Parameters.Add("@cat", SqlDbType.Int).Value = RadTreeView1.Nodes[i].Nodes[n].Value;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                for (int i = 0; i < RadTreeView2.Nodes.Count; i++)
                {
                    if (RadTreeView2.Nodes[i].Checked)
                    {
                        cmd = new SqlCommand("INSERT INTO UserCategories (UserID, CategoryID) VALUES(@user, @cat)", conn);
                        cmd.Parameters.Add("@user", SqlDbType.Int).Value = USER_ID;
                        cmd.Parameters.Add("@cat", SqlDbType.Int).Value = RadTreeView2.Nodes[i].Value;
                        cmd.ExecuteNonQuery();
                    }
                    for (int n = 0; n < RadTreeView2.Nodes[i].Nodes.Count; n++)
                    {
                        if (RadTreeView2.Nodes[i].Nodes[n].Checked)
                        {
                            cmd = new SqlCommand("INSERT INTO UserCategories (UserID, CategoryID) VALUES(@user, @cat)", conn);
                            cmd.Parameters.Add("@user", SqlDbType.Int).Value = USER_ID;
                            cmd.Parameters.Add("@cat", SqlDbType.Int).Value = RadTreeView2.Nodes[i].Nodes[n].Value;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                for (int i = 0; i < RadTreeView3.Nodes.Count; i++)
                {
                    if (RadTreeView3.Nodes[i].Checked)
                    {
                        cmd = new SqlCommand("INSERT INTO UserCategories (UserID, CategoryID) VALUES(@user, @cat)", conn);
                        cmd.Parameters.Add("@user", SqlDbType.Int).Value = USER_ID;
                        cmd.Parameters.Add("@cat", SqlDbType.Int).Value = RadTreeView3.Nodes[i].Value;
                        cmd.ExecuteNonQuery();
                    }
                    for (int n = 0; n < RadTreeView3.Nodes[i].Nodes.Count; n++)
                    {
                        if (RadTreeView3.Nodes[i].Nodes[n].Checked)
                        {
                            cmd = new SqlCommand("INSERT INTO UserCategories (UserID, CategoryID) VALUES(@user, @cat)", conn);
                            cmd.Parameters.Add("@user", SqlDbType.Int).Value = USER_ID;
                            cmd.Parameters.Add("@cat", SqlDbType.Int).Value = RadTreeView3.Nodes[i].Nodes[n].Value;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                cmd = new SqlCommand("DELETE FROM UserVenues WHERE UserID=@user", conn);
                cmd.Parameters.Add("@user", SqlDbType.Int).Value = USER_ID;
                cmd.ExecuteNonQuery();

                CheckBoxList VenueCheckBoxes = (CheckBoxList)VenuesRadPanel.Items[0].Items[0].FindControl("VenueCheckBoxes");

                if (VenueCheckBoxes != null)
                {
                    for (int i = 0; i < VenueCheckBoxes.Items.Count; i++)
                    {
                        if (VenueCheckBoxes.Items[i].Selected)
                        {
                            cmd = new SqlCommand("INSERT INTO UserVenues (UserID, VenueID) VALUES(@user, @cat)", conn);
                            cmd.Parameters.Add("@user", SqlDbType.Int).Value = USER_ID;
                            cmd.Parameters.Add("@cat", SqlDbType.Int).Value = VenueCheckBoxes.Items[i].Value;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                conn.Close();
                Encryption encrypt = new Encryption();
                Session["Message"] = "Your profile has been updated!";

                //MessageLiteral.Text = "<script type=\"text/javascript\">alert('" + message + "');</script>";

                MessageRadWindow.Title = "Profile Updated";
                MessageRadWindow.NavigateUrl = "Message.aspx?message=" + encrypt.encrypt(Session["Message"].ToString() + "<br/><img onclick=\"Search('Home.aspx');\" onmouseover=\"this.src='image/DoneSonButtonSelected.png'\" onmouseout=\"this.src='image/DoneSonButton.png'\" src=\"image/DoneSonButton.png\"/>");
                MessageRadWindow.Visible = true;
                MessageRadWindowManager.VisibleOnPageLoad = true;
            }
            else
            {
                Encryption encrypt = new Encryption();
                Session["Message"] = "Email is invalid";

                //MessageLiteral.Text = "<script type=\"text/javascript\">alert('" + message + "');</script>";

                MessageRadWindow.Title = "Invalid Email";
                MessageRadWindow.NavigateUrl = "Message.aspx?message=" + encrypt.encrypt(Session["Message"].ToString() + "<br/><img onclick=\"Search('Home.aspx');\" onmouseover=\"this.src='image/DoneSonButtonSelected.png'\" onmouseout=\"this.src='image/DoneSonButton.png'\" src=\"image/DoneSonButton.png\"/>");
                MessageRadWindow.Visible = true;
                MessageRadWindowManager.VisibleOnPageLoad = true;
            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString();
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
    }

    protected void ChangeBillState(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet ds = dat.GetData("SELECT * FROM State WHERE country_id=" + BillCountryDropDown.SelectedValue);

        bool isTextBox = false;
        if (ds.Tables.Count > 0)
            if (ds.Tables[0].Rows.Count > 0)
            {
                BillStateDropPanel.Visible = true;
                BillStateTextPanel.Visible = false;
                BillStateDropDown.DataSource = ds;
                BillStateDropDown.DataTextField = "state_2_code";
                BillStateDropDown.DataValueField = "state_id";
                BillStateDropDown.DataBind();
            }
            else
                isTextBox = true;
        else
            isTextBox = true;

        if (isTextBox)
        {
            BillStateTextPanel.Visible = true;
            BillStateDropPanel.Visible = false;
        }
    }
}
