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

public partial class EnterGroupEvent : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DateErrorLabel.Text = "";
        GroupingErrorLabel.Text = "";
        HtmlLink lk = new HtmlLink();
        HtmlHead head = (HtmlHead)Page.Header;
        lk.Attributes.Add("rel", "canonical");
        lk.Href = "http://hippohappenings.com/EnterGroup.aspx";
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

        DataView dv = dat.GetDataDV("SELECT * FROM TermsAndConditions");
        Literal lit1 = new Literal();
        lit1.Text = dv[0]["Content"].ToString();
        TACTextBox.Controls.Add(lit1);

        Literal liter = new Literal();
        liter.Text = "<link type=\"text/css\" href=\"Rads.css\" rel=\"stylesheet\" />";
        Page.Header.Controls.Add(liter);



        if (!IsPostBack)
        {
            ColorTd1.BgColor = "#568301";
            ColorTd2.BgColor = "#1fb6e7";

            Session["NewVenue"] = null;
            Session.Remove("NewVenue");

            DataSet dsCountries = dat.GetData("SELECT * FROM Countries");
            VenueCountry.DataSource = dsCountries;
            VenueCountry.DataTextField = "country_name";
            VenueCountry.DataValueField = "country_id";
            VenueCountry.DataBind();

            VenueCountry.SelectedValue = "223";
            ChangeVenueState("223");
            ExistingVenuePanel.Visible = true;

            DescriptionTextBox.Attributes.Add("onkeyup", "CountCharsEditor(editor)");

            Session.Remove("CategoriesSet");
            Session["CategoriesSet"] = null;

            CountryDropDown.SelectedValue = "223";
            dsCountries = dat.GetData("SELECT * FROM State WHERE country_id=223");

            StateDropDown.DataSource = dsCountries;
            StateDropDown.DataTextField = "state_2_code";
            StateDropDown.DataValueField = "state_id";
            StateDropDown.DataBind();
            StateDropDown.Items.Insert(0, new ListItem("Select State..", "-1"));
            StateDropDownPanel.Visible = true;
            StateTextBoxPanel.Visible = false;
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

        try
        {
            Session["RedirectTo"] = Request.Url.AbsoluteUri;


            if (Session["User"] != null)
            {

                DataView dvg = dat.GetDataDV("SELECT * FROM Group_Members WHERE MemberID=" +
                    Session["User"].ToString() + " AND SharedHosting='True' AND GroupID=" +
                    Request.QueryString["groupid"].ToString());

                if (dvg.Count == 0)
                    Response.Redirect("home");

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
                WelcomeLabel.Text = "On Hippo Happenings, the users have all the power to post event, ads, groups and venues alike. In order to do so, and for us to maintain clean and manageable content,  "
                    + " we require that you <a class=\"AddLink\" href=\"Register.aspx\">create an account</a> with us. <a class=\"AddLink\" href=\"login\">Log in</a> if you have an account already. " +
                    "Having an account with us will allow you to do many other things as well. You'll will be able to add events to your calendar, communicate with others going to events, add your friends, filter content based on your preferences, " +
                    " feature your ads thoughout the site, join and create groups and group events and much more. <br/><br/>So let's go <a class=\"AddLink\" style=\"font-size: 16px;\" href=\"Register.aspx\">Register!</a>";

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

    protected void SetCategories()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet dsCategories = dat.GetData("SELECT * FROM GroupEvent_Category WHERE GroupEvent_ID=" +
            Request.QueryString["ID"].ToString());

        if (dsCategories.Tables.Count > 0)
            if (dsCategories.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < dsCategories.Tables[0].Rows.Count; i++)
                {
                    Telerik.Web.UI.RadTreeNode node = (Telerik.Web.UI.RadTreeNode)CategoryTree.FindNodeByValue(dsCategories.Tables[0].Rows[i]["Category_ID"].ToString());
                    if (node != null)
                    {
                        node.Checked = true;
                        //node.Enabled = false;
                    }
                    else
                    {

                        node = (Telerik.Web.UI.RadTreeNode)RadTreeView2.FindNodeByValue(dsCategories.Tables[0].Rows[i]["Category_ID"].ToString());
                        if (node != null)
                        {
                            node.Checked = true;
                            //node.Enabled = false;
                        }
                    }
                }
            }

        Session["CategoriesSet"] = "notnull";

    }

    protected void fillVenue()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        bool isCopy = false;
        if (Request.QueryString["ID"] != null)
        {
            if (Request.QueryString["copy"] != null)
            {
                if (!bool.Parse(Request.QueryString["copy"].ToString()))
                {
                }
                else
                {
                    isCopy = true;
                }
            }
        }
        //If there is no owner, the venue is up for grabs.
        DataSet dsVenue = dat.GetData("SELECT * FROM GroupEvents WHERE ID=" + Request.QueryString["ID"].ToString());
        DataView dvVenue = new DataView(dsVenue.Tables[0], "", "", DataViewRowState.CurrentRows);
        if (dsVenue.Tables.Count > 0)
            if (dsVenue.Tables[0].Rows.Count > 0)
            {
                #region First Tab
                TitleLabel.Text = "You are editing the group event '" + dsVenue.Tables[0].Rows[0]["Name"].ToString() +
                    "'";
                DescriptionTextBox.Content = dsVenue.Tables[0].Rows[0]["Content"].ToString();
                VenueNameTextBox.THE_TEXT = dsVenue.Tables[0].Rows[0]["Name"].ToString();

                DataView dvOccur = dat.GetDataDV("SELECT * FROM GroupEvent_Occurance WHERE GroupEventID=" + 
                    Request.QueryString["ID"].ToString());

                bool isVenue = false;
                string venueID = "";
                string venueName = "";
                DataView dvVenName;
                if (isCopy)
                {


                    if (dvOccur[0]["VenueID"] != null)
                    {
                        if (dvOccur[0]["VenueID"].ToString().Trim() != "")
                        {
                            isVenue = true;
                        }
                    }

                    if (isVenue)
                    {
                        PrivateCheckList.SelectedValue = "2";
                        PrivatePanel.Visible = false;
                        PublicPanel.Visible = true;

                        venueID = dvOccur[0]["VenueID"].ToString();
                        dvVenName = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + venueID);
                        VenueCountry.SelectedValue = dvVenName[0]["Country"].ToString();
                        if (VenueState.Visible)
                            VenueState.Items.FindByText(dvVenName[0]["State"].ToString()).Selected = true;
                        else
                            VenueStateTextBox.Text = dvVenName[0]["State"].ToString();

                        GetVenues(GoButton, new EventArgs());

                        venueName = dvVenName[0]["Name"].ToString();
                        TimeFrameDiv.InnerHtml = venueName;
                        Session["NewVenue"] = venueID;


                    }
                    else
                    {
                        PrivateCheckList.SelectedValue = "1";
                        PrivatePanel.Visible = true;
                        PublicPanel.Visible = false;
                        CountryDropDown.DataBind();
                        CountryDropDown.SelectedValue = dvOccur[0]["Country"].ToString().Trim();
                        //CountryDropDown.Items.FindByValue(dvOccur[0]["Country"].ToString().Trim()).Selected = true;
                        ChangeState(CountryDropDown, new EventArgs());
                        if (StateDropDown.Visible)
                            StateDropDown.SelectedValue = dvOccur[0]["State"].ToString();
                        else
                            StateTextBox.THE_TEXT = dvOccur[0]["State"].ToString();

                        CityTextBox.Text = dvOccur[0]["City"].ToString();
                        ZipTextBox.Text = dvOccur[0]["Zip"].ToString();
                        if (CountryDropDown.SelectedValue == "223")
                        {
                            if (dvOccur[0]["StreetNumber"].ToString().Trim() != "")
                            {
                                StreetNumberTextBox.Text = dvOccur[0]["StreetNumber"].ToString().Trim();
                            }

                            if (dvOccur[0]["StreetName"].ToString().Trim() != "")
                            {
                                StreetNameTextBox.Text = dvOccur[0]["StreetName"].ToString().Trim();
                            }

                            if (dvOccur[0]["AptName"].ToString().Trim() != "")
                            {
                                AptDropDown.Items.FindByText(dvOccur[0]["AptName"].ToString().Trim()).Selected = true;
                                AptNumberTextBox.Text = dvOccur[0]["AptNo"].ToString().Trim();
                            }

                            if (dvOccur[0]["StreetDrop"].ToString().Trim() != "")
                            {
                                StreetDropDown.Items.FindByText(dvOccur[0]["StreetDrop"].ToString()).Selected = true;
                            }
                        }
                        else
                        {
                            CityTextBox.Text = dvOccur[0]["City"].ToString();
                            ZipTextBox.Text = dvOccur[0]["Zip"].ToString();
                            LocationTextBox.Text = dvOccur[0]["Location"].ToString();
                            if (dvOccur[0]["AptName"].ToString().Trim() != "")
                            {
                                AptDropDown.Items.FindByText(dvOccur[0]["AptName"].ToString().Trim()).Selected = true;
                                AptNumberTextBox.Text = dvOccur[0]["AptNo"].ToString().Trim();
                            }
                        }
                    }
                }
                else
                {
                    foreach (DataRowView row in dvOccur)
                    {
                        if (row["VenueID"] != null)
                            if (row["VenueID"].ToString().Trim() != "")
                                isVenue = true;

                        if (!isVenue)
                        {
                            if (row["Country"].ToString() == "223")
                                venueID = row["City"].ToString() + ";" + row["State"].ToString() + ";" + row["Zip"].ToString() +
                                    ";" + row["StreetNumber"].ToString() + "%" + row["StreetName"].ToString() + "%" + row["StreetDrop"].ToString() +
                                    ";" + row["AptName"].ToString() +
                                    " " + row["AptNo"].ToString() + ";" + row["Country"].ToString();
                            else
                                venueID = row["City"].ToString() + ";" + row["State"].ToString() + ";" + row["Zip"].ToString() +
                                    ";" + row["Location"].ToString() +
                                    ";" + row["AptName"].ToString() +
                                    " " + row["AptNo"].ToString() + ";" + row["Country"].ToString();
                            venueName = "Private Location";
                        }
                        else
                        {
                            venueID = row["VenueID"].ToString();
                            dvVenName = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + venueID);
                            venueName = dvVenName[0]["Name"].ToString();
                        }

                        DateSelectionsListBox.Items.Add(new ListItem(row["DateTimeStart"].ToString() + " -- " +
                            row["DateTimeEnd"].ToString() + " -- " + venueName, venueID));
                    }
                }

                #endregion

                #region Second Tab
                //string mediaCategory = dsVenue.Tables[0].Rows[0]["mediaCategory"].ToString();
                char[] delim6 = { '\\' };
                char[] delim4 = { ';' };
                char[] delim5 = { '.' };

                DataView dvSlider = dat.GetDataDV("SELECT * FROM GroupEvent_Slider_Mapping WHERE GroupEventID=" + Request.QueryString["ID"].ToString());
                if (dvSlider.Count > 0)
                {
                    MainAttractionCheck.Checked = true;
                    MainAttractionPanel.Visible = true;
                    PicsNVideosPanel.Visible = true;
                    for (int i = 0; i < dvSlider.Count; i++)
                    {

                        if (!bool.Parse(dvSlider[i]["isYouTube"].ToString()))
                        {
                            string firstName = "";
                            string secondName = "";
                            bool assignName = false;
                            if (dvSlider[i]["Caption"] != null)
                            {
                                if (dvSlider[i]["Caption"].ToString().Trim() != "")
                                {
                                    firstName = dvSlider[i]["RealPictureName"].ToString() + " - Caption Added";
                                    secondName = dvSlider[i]["PictureName"].ToString() + " - " + dvSlider[i]["Caption"].ToString();
                                }
                                else
                                    assignName = true;
                            }
                            else
                                assignName = true;

                            if (assignName)
                            {
                                firstName = dvSlider[i]["RealPictureName"].ToString();
                                secondName = dvSlider[i]["PictureName"].ToString();
                            }

                            ListItem newListItem = new ListItem(firstName, secondName);
                            PictureCheckList.Items.Add(newListItem);

                        }
                        else
                        {
                            ListItem newListItem = new ListItem("You Tube ID: " + dvSlider[i]["RealPictureName"].ToString(), dvSlider[i]["RealPictureName"].ToString());
                            if (dvSlider[i]["Caption"] != null)
                            {
                                if (dvSlider[i]["Caption"].ToString().Trim() != "")
                                {
                                    newListItem = new ListItem("You Tube ID: " + dvSlider[i]["RealPictureName"].ToString() +
                                        " - Caption Added", dvSlider[i]["RealPictureName"].ToString() + " - " +
                                        dvSlider[i]["Caption"].ToString().Trim());
                                }
                            }

                            PictureCheckList.Items.Add(newListItem);
                        }
                    }

                }

                if (dsVenue.Tables[0].Rows[0]["ColorA"].ToString() == "568301"
                    && dsVenue.Tables[0].Rows[0]["ColorB"].ToString() == "1fb6e7")
                {
                    ColorSchemeRadioList.SelectedValue = "0";
                    ColorPanel.Visible = false;
                    ColorTd1.BgColor = "#568301";
                    ColorTd2.BgColor = "#1fb6e7";
                }
                else
                {
                    ColorSchemeRadioList.SelectedValue = "1";
                    ColorPanel.Visible = true;
                    ColorTd1.BgColor = "#" + dsVenue.Tables[0].Rows[0]["ColorA"].ToString();
                    ColorTd2.BgColor = "#" + dsVenue.Tables[0].Rows[0]["ColorB"].ToString();
                }

                if (dsVenue.Tables[0].Rows[0]["TextA"].ToString() == "ffffff"
                    && dsVenue.Tables[0].Rows[0]["TextB"].ToString() == "ffffff")
                {
                    ColorTextRadioList.SelectedValue = "0";
                    ColorPanel.Visible = false;
                }
                else
                {
                    ColorTextRadioList.SelectedValue = "1";
                    ColorPanel.Visible = true;
                    Text1Label.ForeColor =
                        System.Drawing.ColorTranslator.FromHtml("#" + dsVenue.Tables[0].Rows[0]["TextA"].ToString());
                    Text2Label.ForeColor =
                        System.Drawing.ColorTranslator.FromHtml("#" + dsVenue.Tables[0].Rows[0]["TextB"].ToString());
                }

                #endregion

                #region Third Tab
                if (dvVenue[0]["Agenda"] != null)
                {
                    if (dvVenue[0]["Agenda"].ToString().Trim() != "")
                    {
                        AgendaCheckBox.Checked = true;
                        ShowAgendaPanel(AgendaCheckBox, new EventArgs());
                        AgendaLiteral.Text = dvVenue[0]["Agenda"].ToString();
                    }
                }

                DataView dvStuff = dat.GetDataDV("SELECT * FROM GroupEvent_StuffNeeded WHERE GroupEventID=" +
                    Request.QueryString["ID"].ToString() + " ORDER BY OrderID");
                if (dvStuff.Count > 0)
                {
                    StuffCheckBox.Checked = true;
                    ShowStuffPanel(StuffCheckBox, new EventArgs());
                }
                StuffCheckableCheckBox.Checked = bool.Parse(dvVenue[0]["StuffNeededCheckable"].ToString());
                foreach (DataRowView row in dvStuff)
                {
                    StuffNeededListBox.Items.Add(new ListItem(row["StuffNeeded"].ToString(), ""));
                }
                #endregion

                #region Fourth Tab
                ParticipantsList.SelectedValue = dvVenue[0]["EventType"].ToString();
                if (ParticipantsList.SelectedValue == "3")
                {
                    DataView dvM = dat.GetDataDV("SELECT * FROM GroupEvent_Members GM, Users U " +
                        "WHERE U.User_ID=GM.UserID AND GM.GroupEventID=" + Request.QueryString["ID"].ToString());

                    foreach (DataRowView row in dvM)
                    {
                        GroupMembersSelectedListBox.Items.Add(new ListItem(row["UserName"].ToString(), row["User_ID"].ToString()));
                    }
                }
                ChangeUpParticipants(ParticipantsList, new EventArgs());
                RagistrationRadioList.SelectedValue = dvVenue[0]["RegType"].ToString();
                ChangeRegist(RagistrationRadioList, new EventArgs());
                if (dvVenue[0]["RegType"].ToString() == "2")
                {
                    if (dvVenue[0]["RegNum"] != null)
                        NumRegTextBox.Text = dvVenue[0]["RegNum"].ToString();

                    if (dvVenue[0]["RegDeadline"] != null)
                        if (dvVenue[0]["RegDeadline"].ToString().Trim() != "")
                            DeadlineDatePicker.SelectedDate = DateTime.Parse(dvVenue[0]["RegDeadline"].ToString());
                }
                
                //Grouppings
                if (ParticipantsList.SelectedValue == "2" || ParticipantsList.SelectedValue == "3")
                {
                    DataView dvGroupings = dat.GetDataDV("SELECT * FROM GroupEvent_Groupings WHERE GroupEventID=" + Request.QueryString["ID"].ToString());
                    DataView dvGroupingMembers = dat.GetDataDV("SELECT * FROM GroupEvent_UserGroupings GG, Users U WHERE GG.UserID=U.User_ID AND GG.GroupEventID=" + Request.QueryString["ID"].ToString());
                    if (dvGroupings.Count > 0)
                    {
                        GroupingsCheckBox.Checked = true;
                        OpenGroupings(GroupingsCheckBox, new EventArgs());
                        foreach (DataRowView row in dvGroupings)
                        {
                            GroupingsTreeVie.Nodes.Add(new Telerik.Web.UI.RadTreeNode(row["GroupingName"].ToString(), row["GroupingDescription"].ToString()));
                            dvGroupingMembers.RowFilter = "GroupingID=" + row["ID"].ToString();
                            foreach (DataRowView row2 in dvGroupingMembers)
                            {
                                GroupingParticipantsListBox.Items.Remove(new ListItem(row2["UserName"].ToString(), row2["User_ID"].ToString()));
                                GroupingsTreeVie.Nodes[GroupingsTreeVie.Nodes.Count - 1].Nodes.Add(new Telerik.Web.UI.RadTreeNode(row2["UserName"].ToString(), row2["User_ID"].ToString()));
                            }
                        }
                    }
                }
                #endregion

            }
    }

    protected void GetCategoriesFromTree(bool isUpdate,
        ref Telerik.Web.UI.RadTreeView CategoryTree, DataView dvCat, string ID)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        string message = "";
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        for (int i = 0; i < CategoryTree.Nodes.Count; i++)
        {

            GetCategoriesFromNode(isUpdate, CategoryTree.Nodes[i], dvCat, ID);
            //Recurse if there is children
        }
    }

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

    protected void AddDate(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        bool hasLocation = false;
        string venueName = "";
        string venueID = "";
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
                CityTextBox.Text = dat.stripHTML(CityTextBox.Text);
                ZipTextBox.Text = dat.stripHTML(ZipTextBox.Text);
                StreetNameTextBox.Text = dat.stripHTML(StreetNameTextBox.Text.Trim());
                StreetNumberTextBox.Text = dat.stripHTML(StreetNumberTextBox.Text.Trim());
                LocationTextBox.Text = dat.stripHTML(LocationTextBox.Text.Trim());
                string apt = "";
                if (PrivatePanel.Visible)
                {
                    string state = "";
                    if (StateDropDownPanel.Visible)
                        if (StateDropDown.SelectedItem.Text != "Select State..")
                        {
                            state = StateDropDown.SelectedItem.Text;
                            hasLocation = true;
                        }
                        else
                            hasLocation = false;
                    else
                    {
                        state = dat.stripHTML(StateTextBox.THE_TEXT.Trim());
                        hasLocation = true;
                    }

                    if (hasLocation)
                    {
                        hasLocation = false;
                        if (AptNumberTextBox.Text.Trim() != "")
                            apt = AptDropDown.SelectedItem.Text + " " + AptNumberTextBox.Text.Trim().ToLower();

                        if (CountryDropDown.SelectedValue == "223")
                        {
                            if (state != "" && ZipTextBox.Text.Trim() != "" && CityTextBox.Text.Trim() != ""
                                && StreetNumberTextBox.Text.Trim() != "" && StreetNameTextBox.Text.Trim() != ""
                                && StreetDropDown.Text.Trim() != "Select One...")
                            {
                                hasLocation = true;

                                venueID = CityTextBox.Text.Trim() + ";" + state + ";" + ZipTextBox.Text.Trim() + ";" +
                                     StreetNumberTextBox.Text.Trim() + "%" +
                                    StreetNameTextBox.Text.Trim() + "%" + StreetDropDown.SelectedItem.Text + ";" + apt +
                                    ";" + CountryDropDown.SelectedValue;
                                venueName = "Private Location";
                            }
                        }
                        else
                        {
                            if (state != "" && ZipTextBox.Text.Trim() != "" && CityTextBox.Text.Trim() != ""
                                && LocationTextBox.Text.Trim() != "")
                            {
                                hasLocation = true;
                                venueID = CityTextBox.Text.Trim() + ";" + state + ";" + ZipTextBox.Text.Trim() + ";" +
                                     LocationTextBox.Text.Trim() + ";" + apt + ";" + CountryDropDown.SelectedValue;
                                venueName = "Private Location";
                            }
                        }
                    }
                }
                else
                {
                    if (TimeFrameDiv.InnerHtml.Trim() != "Select Venue >")
                    {
                        hasLocation = true;
                        venueID = Session["NewVenue"].ToString();
                        venueName = TimeFrameDiv.InnerHtml.Trim();
                    }
                }

                if (hasLocation)
                {
                    DateSelectionsListBox.Items.Add(new ListItem(StartDateTimePicker.DbSelectedDate.ToString() + " -- " +
                        EndDateTimePicker.DbSelectedDate.ToString() + " -- " + venueName, venueID));
                }
                else
                {
                    DateErrorLabel.Text = "<br/><br/>*Need to include the location.";
                }
            }
        }
        else
        {
            //MessagePanel.Visible = true;
            DateErrorLabel.Text = "<br/><br/>*Need to include both the Start date and End date.";
        }
    }

    protected void GetCategoriesFromNode(bool isUpdate,
        Telerik.Web.UI.RadTreeNode TreeNode, DataView dvCat, string ID)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        if (TreeNode.Checked && TreeNode.Enabled)
        {
            dvCat.RowFilter = "CATEGORY_ID=" + TreeNode.Value;
            //distinctHash.Add(CategoriesCheckBoxes.Items[i], 21);
            //tagHash.Add(CategoriesCheckBoxes.Items[i], "22");

            if (isUpdate)
            {
                if (dvCat.Count == 0)
                {
                    dat.Execute("INSERT INTO GroupEvent_Category (GroupEvent_ID, CATEGORY_ID) VALUES("
                        + ID + ", " + TreeNode.Value + ")");
                }
            }
            else
            {
                dat.Execute("INSERT INTO GroupEvent_Category (GroupEvent_ID, CATEGORY_ID) VALUES("
                        + ID + ", " + TreeNode.Value + ")");
            }

        }
        else if (!TreeNode.Checked)
        {
            dvCat.RowFilter = "CATEGORY_ID=" + TreeNode.Value;

            if (isUpdate)
            {
                if (dvCat.Count == 0)
                {
                }
                else
                {
                    dat.Execute("DELETE FROM GroupEvent_Category WHERE GroupEvent_ID=" + ID +
                        " AND CATEGORY_ID = " + TreeNode.Value);
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
                GetCategoriesFromNode(isUpdate, TreeNode.Nodes[j], dvCat, ID);
            }
        }

    }

    protected void PostIt(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (AgreeCheckBox.Checked)
        {

            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
            conn.Open();
            string command = "";

            try
            {
                bool isUpdate = false;
                bool isCopy = false;
                DataSet dsVenue = new DataSet();

                if (Request.QueryString["ID"] != null)
                {
                    if (Request.QueryString["copy"] != null)
                    {
                        if (!bool.Parse(Request.QueryString["copy"].ToString()))
                        {
                            dsVenue = dat.GetData("SELECT * FROM GroupEvents WHERE ID=" + Request.QueryString["ID"].ToString());
                            isUpdate = true;
                        }
                        else
                        {
                            isCopy = true;
                        }
                    }
                    else
                    {
                        dsVenue = dat.GetData("SELECT * FROM GroupEvents WHERE ID=" + Request.QueryString["ID"].ToString());
                        isUpdate = true;
                    }
                }

                bool cont = true;
                if (cont)
                {


                    if (isUpdate)
                    {
                        command = "UPDATE GroupEvents SET StuffNeededCheckable=@checkable, ColorA=@cA, ColorB=@cB, TextA=@tA, TextB=@tB, LastEdit=GETDATE(), LastEditBy=" + Session["User"].ToString() +
                            ", Name=@name, [Content]=@content, Agenda=@agenda, " +
                            " EventType=@eType, RegType=@rType, RegNum=@rNum, RegDeadline=@rDeadline WHERE ID=" + Request.QueryString["ID"].ToString();
                    }
                    else
                        command = "INSERT INTO GroupEvents (StuffNeededCheckable, ColorA, ColorB, TextA, TextB, PostedOn, LastEdit, LastEditBy, UserID, GroupID, Name, [Content],  " +
                            " Agenda, EventType, RegType, RegNum, RegDeadline) VALUES(@checkable, @cA, @cB, @tA, @tB, GETDATE(), GETDATE()," +
                            Session["User"].ToString() + ", " + Session["User"].ToString() + ", " +
                            Request.QueryString["GroupID"].ToString() +
                            ", @name, @content, @agenda, @eType, " +
                            " @rType, @rNum, @rDeadline)";

                    SqlCommand cmd = new SqlCommand(command, conn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@checkable", SqlDbType.Bit).Value = StuffCheckableCheckBox.Checked;

                    if (ColorSchemeRadioList.SelectedValue == "0")
                    {
                        cmd.Parameters.Add("@cA", SqlDbType.NVarChar).Value = "568301";
                        cmd.Parameters.Add("@cB", SqlDbType.NVarChar).Value = "1fb6e7";
                    }
                    else
                    {
                        cmd.Parameters.Add("@cA", SqlDbType.NVarChar).Value = ColorTd1.BgColor.Replace("#", "");
                        cmd.Parameters.Add("@cB", SqlDbType.NVarChar).Value = ColorTd2.BgColor.Replace("#", "");
                    }

                    if (ColorTextRadioList.SelectedValue == "0")
                    {
                        cmd.Parameters.Add("@tA", SqlDbType.NVarChar).Value = "ffffff";
                        cmd.Parameters.Add("@tB", SqlDbType.NVarChar).Value = "ffffff";
                    }
                    else
                    {
                        cmd.Parameters.Add("@tA", SqlDbType.NVarChar).Value =
                            System.Drawing.ColorTranslator.ToHtml(Text1Label.ForeColor).Replace("#", "");
                        cmd.Parameters.Add("@tB", SqlDbType.NVarChar).Value =
                            System.Drawing.ColorTranslator.ToHtml(Text2Label.ForeColor).Replace("#", "");
                    }

                    cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = VenueNameTextBox.THE_TEXT.Trim();
                    cmd.Parameters.Add("@content", SqlDbType.NVarChar).Value = DescriptionTextBox.Content.Trim();
                    if (AgendaCheckBox.Checked)
                        cmd.Parameters.Add("@agenda", SqlDbType.NVarChar).Value = AgendaLiteral.Text;
                    else
                        cmd.Parameters.Add("@agenda", SqlDbType.NVarChar).Value = DBNull.Value;
                    cmd.Parameters.Add("@eType", SqlDbType.Int).Value = ParticipantsList.SelectedValue;
                    cmd.Parameters.Add("@rType", SqlDbType.Int).Value = RagistrationRadioList.SelectedValue;
                    if (RagistrationRadioList.SelectedValue == "1")
                    {
                        cmd.Parameters.Add("@rNum", SqlDbType.Int).Value = DBNull.Value;
                        cmd.Parameters.Add("@rDeadline", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        if (NumRegTextBox.Text.Trim() != "")
                            cmd.Parameters.Add("@rNum", SqlDbType.Int).Value = NumRegTextBox.Text.Trim();
                        else
                            cmd.Parameters.Add("@rNum", SqlDbType.Int).Value = DBNull.Value;
                        if (DeadlineDatePicker.SelectedDate == null)
                            cmd.Parameters.Add("@rDeadline", SqlDbType.DateTime).Value = DBNull.Value;
                        else
                            cmd.Parameters.Add("@rDeadline", SqlDbType.DateTime).Value = DeadlineDatePicker.SelectedDate.Value;
                    }

                    cmd.ExecuteNonQuery();

                    string IDofFirstOccurance = "";
                    string eventID = "";
                    if (isUpdate)
                    {
                        eventID = Request.QueryString["ID"].ToString();
                        IDofFirstOccurance = Request.QueryString["O"].ToString();
                    }
                    else
                    {
                        cmd = new SqlCommand("SELECT @@IDENTITY AS IDS", conn);
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd);
                        DataSet ds3 = new DataSet();
                        da2.Fill(ds3);
                        eventID = ds3.Tables[0].Rows[0]["IDS"].ToString();
                    }

                    #region Take care of Venue and Occurance

                    string state = "";
                    string zip = "";
                    string city = "";
                    string country = "";
                    string address = "";
                    string apt = "";
                    string aptNo = "";
                    string aptName = "";
                    string location = "";
                    string streetNo = "";
                    string streetName = "";
                    string streetDrop = "";
                    DataView dvRealVenue;
                    char[] delim = { ';' };
                    string[] delimStr = { "--" };
                    string[] tokens3;
                    string[] tokens2;
                    int venueID = 0;
                    bool isVenue = false;
                    string startDate = "";
                    string endDate = "";
                    
                    

                    foreach (ListItem item in DateSelectionsListBox.Items)
                    {
                        isVenue = false;
                        tokens3 = item.Text.Split(delimStr, StringSplitOptions.RemoveEmptyEntries);
                        startDate = tokens3[0].Trim();
                        endDate = tokens3[1].Trim();
                        tokens2 = item.Value.Split(delim);
                        if (tokens3[2].Trim() != "Private Location")
                        {
                            isVenue = true;
                            venueID = int.Parse(item.Value);
                        }

                        if (!isVenue)
                        {
                            state = tokens2[1];
                            city = tokens2[0];
                            zip = tokens2[2];
                            location = tokens2[3];
                            apt = tokens2[4];
                            country = tokens2[5];

                            if(apt.Trim() != ""){
                                char [] tempdelimer = {' '};
                                string [] apttoks = apt.Split(tempdelimer);
                                aptName = apttoks[0];
                                aptNo = apttoks[1];
                            }


                            if (country == "223")
                            {
                                char[] tempDelim = { '%' };
                                string[] streettoks = location.Split(tempDelim);
                                streetNo = streettoks[0];
                                streetName = streettoks[1];
                                streetDrop = streettoks[2];
                            }
                        }
                        else
                        {
                            dvRealVenue = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + item.Value);
                            state = dvRealVenue[0]["State"].ToString();
                            zip = dvRealVenue[0]["Zip"].ToString();
                            city = dvRealVenue[0]["City"].ToString();
                            country = dvRealVenue[0]["Country"].ToString();
                        }

                        if (isUpdate)
                        {
                            DataView dvSelect = dat.GetDataDV("SELECT * FROM GroupEvent_Occurance WHERE GroupEventID=" +
                                eventID + " AND DateTimeStart = CONVERT(DATETIME,'" + startDate + "')");
                            if (dvSelect.Count == 0)
                            {
                                command = "INSERT INTO GroupEvent_Occurance (GroupEventID, DateTimeStart, DateTimeEnd, " +
                                    "State, City, Country, Zip, StreetNumber, StreetName, StreetDrop, AptName, AptNo, Location, VenueID) VALUES(" + eventID + ", @start, @end, @state, " +
                                    "@city, @country, @zip, @sNo, @sName, @sDrop, @aName, @aNo, @location, @venue)";
                            }
                            else
                            {
                                command = "UPDATE GroupEvent_Occurance SET DateTimeEnd=@end, State=@state, " +
                                    "City=@city, Country=@country, Zip=@zip, StreetNumber=@sNo, StreetName=@sName, " +
                                    "StreetDrop=@sDrop, AptName=@aName, AptNo=@aNo, Location=@location, VenueID=@venue " +
                                    " WHERE ID=" + dvSelect[0]["ID"].ToString();
                            }
                        }
                        else
                        {
                            command = "INSERT INTO GroupEvent_Occurance (GroupEventID, DateTimeStart, DateTimeEnd, " +
                                "State, City, Country, Zip, StreetNumber, StreetName, StreetDrop, AptName, AptNo, Location, VenueID) VALUES(" + eventID + ", @start, @end, @state, " +
                                "@city, @country, @zip, @sNo, @sName, @sDrop, @aName, @aNo, @location, @venue)";
                        }

                        cmd = new SqlCommand(command, conn);
                        command += "venueID: '" + venueID.ToString() + "', country: " + country + ", address: " + address;
                        if (!isVenue)
                            cmd.Parameters.Add("@venue", SqlDbType.Int).Value = DBNull.Value;
                        else
                            cmd.Parameters.Add("@venue", SqlDbType.Int).Value = venueID;
                        cmd.Parameters.Add("@start", SqlDbType.DateTime).Value = startDate;
                        cmd.Parameters.Add("@end", SqlDbType.DateTime).Value = endDate;
                        cmd.Parameters.Add("@zip", SqlDbType.NVarChar).Value = zip;
                        cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = city;
                        cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = state;
                        if (isVenue)
                        {
                            cmd.Parameters.Add("@sNo", SqlDbType.NVarChar).Value = DBNull.Value;
                            cmd.Parameters.Add("@sName", SqlDbType.NVarChar).Value = DBNull.Value;
                            cmd.Parameters.Add("@sDrop", SqlDbType.NVarChar).Value = DBNull.Value;
                            cmd.Parameters.Add("@aName", SqlDbType.NVarChar).Value = DBNull.Value;
                            cmd.Parameters.Add("@aNo", SqlDbType.NVarChar).Value = DBNull.Value;
                            cmd.Parameters.Add("@location", SqlDbType.NVarChar).Value = DBNull.Value;
                        }
                        else
                        {
                            if (country == "223")
                            {
                                cmd.Parameters.Add("@sNo", SqlDbType.NVarChar).Value = streetNo;
                                cmd.Parameters.Add("@sName", SqlDbType.NVarChar).Value = streetName;
                                cmd.Parameters.Add("@sDrop", SqlDbType.NVarChar).Value = streetDrop;
                                cmd.Parameters.Add("@aName", SqlDbType.NVarChar).Value = aptName;
                                cmd.Parameters.Add("@aNo", SqlDbType.NVarChar).Value = aptNo;
                                cmd.Parameters.Add("@location", SqlDbType.NVarChar).Value = DBNull.Value;
                            }
                            else
                            {
                                cmd.Parameters.Add("@sNo", SqlDbType.NVarChar).Value = DBNull.Value;
                                cmd.Parameters.Add("@sName", SqlDbType.NVarChar).Value = DBNull.Value;
                                cmd.Parameters.Add("@sDrop", SqlDbType.NVarChar).Value = DBNull.Value;
                                cmd.Parameters.Add("@location", SqlDbType.NVarChar).Value = location;
                                cmd.Parameters.Add("@aName", SqlDbType.NVarChar).Value = aptName;
                                cmd.Parameters.Add("@aNo", SqlDbType.NVarChar).Value = aptNo;
                            }
                        }
                        cmd.Parameters.Add("@country", SqlDbType.Int).Value = country;
                       
                        cmd.ExecuteNonQuery();

                        if (IDofFirstOccurance == "")
                        {
                            cmd = new SqlCommand("SELECT @@IDENTITY AS AAS", conn);
                            DataSet dsTemp = new DataSet();
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(dsTemp);
                            IDofFirstOccurance = dsTemp.Tables[0].Rows[0]["AAS"].ToString();
                        }
                    }
                    #endregion

                    #region Take care of slider
                    bool isSlider = false;
                    if (PictureCheckList.Items.Count > 0)
                        isSlider = true;
                    if (isSlider)
                    {

                        char[] delim2 = { '\\' };
                        string[] fileArray = System.IO.Directory.GetFiles(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\Slider\\");

                        if (!System.IO.Directory.Exists(MapPath(".") + "\\GroupEventFiles"))
                        {
                            System.IO.Directory.CreateDirectory(MapPath(".") + "\\GroupEventFiles\\");
                            System.IO.Directory.CreateDirectory(MapPath(".") + "\\GroupEventFiles\\" + eventID + "\\");
                            System.IO.Directory.CreateDirectory(MapPath(".") + "\\GroupEventFiles\\" + eventID + "\\Slider\\");
                        }
                        else
                        {
                            if (!System.IO.Directory.Exists(MapPath(".") + "\\GroupEventFiles\\" + eventID))
                            {
                                System.IO.Directory.CreateDirectory(MapPath(".") + "\\GroupEventFiles\\" + eventID + "\\");
                                System.IO.Directory.CreateDirectory(MapPath(".") + "\\GroupEventFiles\\" + eventID + "\\Slider\\");
                            }
                            else
                            {
                                if (!!System.IO.Directory.Exists(MapPath(".") + "\\GroupEventFiles\\" + eventID + "\\Slider\\"))
                                    System.IO.Directory.CreateDirectory(MapPath(".") + "\\GroupEventFiles\\" + eventID + "\\Slider\\");
                            }
                        }

                        char[] delim3 = { '.' };
                        char[] delim4 = { '-' };
                        string realName = "";
                        string origName = "";
                        string caption = "";
                        dat.Execute("DELETE FROM GroupEvent_Slider_Mapping WHERE GroupEventID=" + eventID.ToString());
                        for (int i = 0; i < PictureCheckList.Items.Count; i++)
                        {
                            caption = "";
                            string[] tokensN = PictureCheckList.Items[i].Value.Split(delim4);
                            if (tokensN.Length > 1)
                            {
                                realName = tokensN[0].Trim();
                                caption = PictureCheckList.Items[i].Value.Replace(tokensN[0].Trim() + " - ", "");
                            }
                            else
                            {
                                realName = tokensN[0].Trim();
                            }

                            tokensN = PictureCheckList.Items[i].Text.Split(delim4);
                            origName = tokensN[0].Trim();

                            string[] tokens = realName.Split(delim3);

                            if (tokens.Length >= 2)
                            {
                                if (tokens[1].ToUpper() == "JPG" || tokens[1].ToUpper() == "JPEG" || tokens[1].ToUpper() == "GIF" || tokens[1].ToUpper() == "PNG")
                                {
                                    if (!System.IO.File.Exists(MapPath(".") + "\\GroupEventFiles\\" + eventID.ToString() + "\\Slider\\" + realName))
                                    {
                                        System.IO.File.Copy(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\Slider\\" +
                                                            realName, MapPath(".") + "\\GroupEventFiles\\" + eventID.ToString() + "\\Slider\\" + realName);
                                    }

                                    cmd = new SqlCommand("INSERT INTO GroupEvent_Slider_Mapping (GroupEventID, PictureName, RealPictureName, Caption) " +
                                        "VALUES (@eventID, @picName, @realName, @cap)", conn);
                                    cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = eventID;
                                    cmd.Parameters.Add("@picName", SqlDbType.NVarChar).Value = realName;
                                    cmd.Parameters.Add("@realName", SqlDbType.NVarChar).Value = origName;
                                    cmd.Parameters.Add("@cap", SqlDbType.NVarChar).Value = caption;
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                cmd = new SqlCommand("INSERT INTO GroupEvent_Slider_Mapping (isYouTube,GroupEventID, PictureName, RealPictureName, Caption) " +
                                        "VALUES ('True',@eventID, @picName, @realName, @cap)", conn);
                                cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = eventID;
                                cmd.Parameters.Add("@picName", SqlDbType.NVarChar).Value = realName.Replace("YouTube ID: ", "");
                                cmd.Parameters.Add("@realName", SqlDbType.NVarChar).Value = realName.Replace("YouTube ID: ", "");
                                cmd.Parameters.Add("@cap", SqlDbType.NVarChar).Value = caption;
                                cmd.ExecuteNonQuery();
                            }
                        }

                    }
                    #endregion

                    #region Take care of 'Stuff we Need'
                    int order = 0;
                    DataView dvStuff;
                    bool doInsert = false;
                    string keepthese = "";
                    if (StuffCheckBox.Checked)
                    {
                        foreach (ListItem item in StuffNeededListBox.Items)
                        {
                            if (isUpdate)
                            {
                                dvStuff = dat.GetDataDV("SELECT * FROM GroupEvent_StuffNeeded WHERE GroupEventID=" +
                                    eventID + " AND StuffNeeded='" + item.Text.Replace("'", "''") + "'");

                                if (dvStuff.Count == 0)
                                {
                                    doInsert = true;
                                }
                                else
                                {
                                    cmd = new SqlCommand("UPDATE GroupEvent_StuffNeeded SET OrderID=@order WHERE GroupEventID=" +
                                    eventID + " AND StuffNeeded='" + item.Text.Replace("'", "''") + "'", conn);
                                    cmd.Parameters.Add("@order", SqlDbType.Int).Value = order++;
                                    cmd.ExecuteNonQuery();


                                    keepthese += " AND ID <> " + dvStuff[0]["ID"].ToString();
                                }
                            }
                            else
                            {
                                doInsert = true;
                            }

                            if (doInsert)
                            {
                                cmd = new SqlCommand("INSERT INTO GroupEvent_StuffNeeded (GroupEventID, StuffNeeded, " +
                                    "UserID, OrderID) VALUES(" + eventID + ", @stuff, @UserID, @order)", conn);
                                cmd.Parameters.Add("@stuff", SqlDbType.NVarChar).Value = item.Text;
                                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = DBNull.Value;
                                cmd.Parameters.Add("@order", SqlDbType.Int).Value = order++;
                                cmd.ExecuteNonQuery();

                                cmd = new SqlCommand("SELECT @@IDENTITY AS AID", conn);
                                DataSet dsN = new DataSet();
                                SqlDataAdapter d1 = new SqlDataAdapter(cmd);
                                d1.Fill(dsN);

                                keepthese += " AND ID <> " + dsN.Tables[0].Rows[0]["AID"].ToString();
                            }
                        }
                    }
                    dat.Execute("DELETE FROM GroupEvent_StuffNeeded WHERE GroupEventID=" + eventID + keepthese);

                    #endregion

                    CreateMembers(eventID, isUpdate, IDofFirstOccurance);
                    CreateGroupings(eventID, isUpdate);
                    CreateCategories(eventID, isUpdate);

                    //Send the informational email to the user
                    DataSet dsUser = dat.GetData("SELECT Email, UserName FROM USERS WHERE User_ID=" +
                        Session["User"].ToString());

                    string emailBody = "<br/><br/>Dear " + dsUser.Tables[0].Rows[0]["UserName"].ToString() + ", <br/><br/> you have successfully posted the group event \"" + VenueNameTextBox.THE_TEXT +
                       "\". <br/><br/> You can find this group event <a href=\"http://hippohappenings.com/" + 
                       dat.MakeNiceName(VenueNameTextBox.THE_TEXT) + "_" + IDofFirstOccurance + "_" +
                       eventID + "_GroupEvent\">here</a>. " +
                       "<br/><br/> To rate your experience posting this group event <a href=\"http://hippohappenings.com/RateExperience.aspx?Edit=" + isUpdate.ToString() + "&Type=GE&ID=" + eventID +
                       "\">please include your feedback here.</a>" +
                       "<br/><br/><br/>Have a Hippo Happening Day!<br/><br/>";

                    try
                    {
                        if (isUpdate)
                        {

                            if (!Request.Url.AbsoluteUri.Contains("localhost"))
                            {
                                dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                                    dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "You have updated group event: " +
                                    VenueNameTextBox.THE_TEXT);
                            }
                        }
                        else
                        {
                            if (!Request.Url.AbsoluteUri.Contains("localhost"))
                            {
                                dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                                    dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "You have posted the event: " +
                                    VenueNameTextBox.THE_TEXT);

                                SendGroupEventNotifications(eventID, IDofFirstOccurance);
                            }
                        }

                    }
                    catch (Exception ex)
                    {

                    }

                    conn.Close();



                    //pop up the message to the user
                    Encryption encrypt = new Encryption();

                    if (!isUpdate)
                    {
                        Session["Message"] = "Your group event has been posted successfully! An email with this info will also be sent to your account.<br/>"
                            + "<br/>Check out <a class=\"AddLink\" onclick=\"Search('" + 
                            dat.MakeNiceName(VenueNameTextBox.THE_TEXT) + "_" +
                            IDofFirstOccurance + "_" + eventID + "_GroupEvent');\">this group event's</a> home page.<br/><br/><br/> " +
                            "-<a class=\"AddLink\" onclick=\"Search('RateExperience.aspx?Edit=" + isUpdate.ToString() + "&Type=GE&ID=" +
                            eventID + "');\" >Rate </a>your user experience posting this group event.<br/>";
                    }
                    else
                    {
                        Session["Message"] = "You have successfully updated group event: \"" + VenueNameTextBox.THE_TEXT.Trim() +
                        "\".<br/><br/>Check out <a class=\"AddLink\" onclick=\"Search('" + 
                        dat.MakeNiceName(VenueNameTextBox.THE_TEXT) + "_" + Request.QueryString["O"].ToString() + "_" + eventID +
                        "_GroupEvent');\">this group event's</a> home page.<br/><br/> <a class=\"AddLink\" onclick=\"Search('RateExperience.aspx?Edit=" +
                        isUpdate.ToString() + "&Type=GE&ID=" + eventID + "');\" >Rate </a>your user experience editing this group event.<br/>";
                    }

                    MessageRadWindow.NavigateUrl = "Message.aspx?message=" + encrypt.encrypt(Session["Message"].ToString() + "<br/><img onclick=\"Search('home');\" onmouseover=\"this.src='image/DoneSonButtonSelected.png'\" onmouseout=\"this.src='image/DoneSonButton.png'\" src=\"image/DoneSonButton.png\"/>");
                    MessageRadWindow.Visible = true;
                    MessageRadWindow.VisibleOnPageLoad = true;
                }
            }
            catch (Exception ex)
            {
                MessagePanel.Visible = true;
                YourMessagesLabel.Text += ex.ToString() + command;
            }
        }
        else
        {
            MessagePanel.Visible = true;
            YourMessagesLabel.Text += "<br/><br/>You must agree to the terms and conditions.";
        }
    }

    protected void SendGroupEventNotifications(string eventID, string IDofFirstOccurance)
    {
        string theID = Request.QueryString["GroupID"].ToString();

        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        DataView dvUsers = dat.GetDataDV("SELECT * FROM Group_Members GM, Users U WHERE GM.MemberID=" +
            "U.User_ID AND GM.GroupID=" + theID + " AND Prefs LIKE '%2%'");
        DataView dvGroup = dat.GetDataDV("SELECT * FROM Groups WHERE ID=" + theID);
        DataView dvThread = dat.GetDataDV("SELECT * FROM GroupEvents WHERE ID=" + eventID);
        string email = "A new event '" + dvThread[0]["Name"].ToString() +
            "' has been posted for the group '" +
            dvGroup[0]["Header"].ToString() + "'. <a href=\"http://hippohappenings.com/" +
            dat.MakeNiceName(dvThread[0]["Name"].ToString()) + "_" + IDofFirstOccurance + "_" + eventID +
            "_GroupEvent\">Check it out.</a>";
        foreach (DataRowView row in dvUsers)
        {
            dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
            System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(), row["Email"].ToString(),
            email, "A new group event has been posted");
        }
    }

    protected void CreateMembers(string theID, bool isUpdate, string IDofFirstOccurance)
    {
        if (ParticipantsList.SelectedValue != "1")
        {
            string command = "";
            try
            {
                char[] delim = { '-' };
                HttpCookie cookie = Request.Cookies["BrowserDate"];
                Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

                string emailBody = Session["UserName"].ToString() + " has created a new group event <a href=\"http://hippohappenings.com/" +
                   dat.MakeNiceName(VenueNameTextBox.THE_TEXT.Trim()) +
                   "_" + IDofFirstOccurance + "_" + theID + "_GroupEvent\">" + VenueNameTextBox.THE_TEXT.Trim() +
                      "</a> and is inviting you to it." +
                      " To accept this invitation <a href=\"http://hippohappenings.com/my-account\">log in to HippoHappenings</a>, view your messages in 'My Account' and " +
                      "click 'Accept' in the message corresponding to this email. Have good fun at the event! <br/><br/>HippoHappenings";
                string emailSubject = "You have a group event invite for '" + VenueNameTextBox.THE_TEXT + "'";
                string messageBody = theID + " " + Session["UserName"].ToString() + " has created a new group event <a class=\"AddLink\" href=\"" +
                    dat.MakeNiceName(VenueNameTextBox.THE_TEXT.Trim()) +
                    "_" + IDofFirstOccurance + "_" + theID + "_GroupEvent\">" + VenueNameTextBox.THE_TEXT.Trim() +
                       "</a> and is inviting you to it. Click " +
                    "the 'Accept' button to the right if you accept this invitation. ";

                if (isUpdate)
                {
                    emailBody = Session["UserName"].ToString() + " has updated the group event <a href=\"http://hippohappenings.com/" +
                   dat.MakeNiceName(VenueNameTextBox.THE_TEXT.Trim()) +
                   "_" + IDofFirstOccurance + "_" + theID + "_GroupEvent\">" + VenueNameTextBox.THE_TEXT.Trim() +
                      "</a> and is inviting you to it." +
                      " To accept this invitation <a href=\"http://hippohappenings.com/my-account\">log in to HippoHappenings</a>, view your messages in 'My Account' and " +
                      "click 'Accept' in the message corresponding to this email. Have good fun at the event! <br/><br/>HippoHappenings";
                    emailSubject = "You have a group event invite for '" + VenueNameTextBox.THE_TEXT + "'";
                    messageBody = theID + " " + Session["UserName"].ToString() + " has updated the group event <a class=\"AddLink\" href=\"" +
                         dat.MakeNiceName(VenueNameTextBox.THE_TEXT.Trim()) +
                         "_" + IDofFirstOccurance + "_" + theID + "_GroupEvent\">" + VenueNameTextBox.THE_TEXT.Trim() +
                            "</a> and is inviting you to it. Click " +
                         "the 'Accept' button to the right if you accept this invitation. ";
                }

                DataView dvMems;

                dvMems = dat.GetDataDV("SELECT * FROM Group_Members WHERE GroupID=" +
                    Request.QueryString["GroupID"].ToString());


                DataView dvUserGroupCalendar = dat.GetDataDV("SELECT * FROM User_GroupEvent_Calendar WHERE GroupEventID=" + theID);
                bool doIt = false;
                if (dvUserGroupCalendar.Count > 0)
                {
                    if (ParticipantsList.SelectedValue == "2" || ParticipantsList.SelectedValue == "3")
                    {
                        foreach (DataRowView row in dvUserGroupCalendar)
                        {
                            doIt = false;
                            if (ParticipantsList.SelectedValue == "2")
                            {
                                dvMems.RowFilter = "MemberID=" + row["UserID"].ToString();
                                if (dvMems.Count > 0)
                                    doIt = true;
                            }
                            else
                            {
                                if (GroupMembersSelectedListBox.Items.Contains(new ListItem(dat.GetDataDV("SELECT * FROM "+
                                    "Users WHERE User_ID="+row["UserID"].ToString())[0]["UserName"].ToString(), 
                                    row["UserID"].ToString())))
                                    doIt = true;
                            }

                            if (doIt)
                            {
                                if (dat.GetDataDV("SELECT * FROM GroupEvent_Members WHERE UserID=" +
                                    row["UserID"].ToString() + " AND GroupEventID=" + theID + " AND ReoccurrID=" +
                                    row["ReoccurrID"].ToString()).Count == 0)
                                {
                                    dat.Execute("INSERT INTO GroupEvent_Members (UserID, GroupEventID, Accepted, ReoccurrID, DateAdded) " +
                                        "VALUES(" + row["UserID"].ToString() + ", " + theID + ", 'True', " + row["ReoccurrID"].ToString() +
                                        ", CONVERT(DATETIME, '" + row["DateAdded"].ToString() + "'))");
                                }
                            }
                        }
                    }
                }

                
                DataView dvPrevMembers = dat.GetDataDV("SELECT * FROM GroupEvent_Members WHERE GroupEventID=" + theID);

                if (dvPrevMembers.Count > 0 && ParticipantsList.SelectedValue == "1")
                {
                    foreach (DataRowView row in dvUserGroupCalendar)
                    {
                        if (dat.GetDataDV("SELECT * FROM User_GroupEvent_Calendar WHERE UserID=" +
                            row["UserID"].ToString() + " AND GroupEventID=" + theID + " AND ReoccurrID=" +
                            row["ReoccurrID"].ToString()).Count == 0)
                        {
                            dat.Execute("INSERT INTO User_GroupEvent_Calendar (UserID, GroupEventID, ReoccurrID, DateAdded) " +
                                "VALUES(" + row["UserID"].ToString() + ", " + theID + ", " + row["ReoccurrID"].ToString() +
                                ", CONVERT(DATETIME, '" + row["DateAdded"].ToString() + "'))");
                        }
                    }
                }
                
                DataView dvAllOccurances = dat.GetDataDV("SELECT * FROM GroupEvent_Occurance WHERE GroupEventID=" + theID);




                string keepthese = "";
                if (ParticipantsList.SelectedValue == "3")
                {
                    foreach (ListItem item in GroupMembersSelectedListBox.Items)
                    {
                        //execute update/insert member
                        foreach (DataRowView row in dvAllOccurances)
                        {
                            if (isUpdate)
                            {
                                dvPrevMembers.RowFilter = "UserID=" + item.Value + " AND ReoccurrID=" + row["ID"].ToString();
                                if (dvPrevMembers.Count == 0)
                                {

                                    command = "INSERT INTO GroupEvent_Members (GroupEventID, UserID, ReoccurrID) VALUES(" + theID +
                                        ", " + item.Value + ", " + row["ID"].ToString() + ")";
                                    dat.Execute(command);
                                    keepthese += " AND NOT(UserID = " + item.Value + " AND ReoccurrID=" + row["ID"].ToString() + ") ";


                                    DataView dvUser = dat.GetDataDV("SELECT * FROM Users WHERE User_ID=" + item.Value);
                                    if (item.Value != Session["User"].ToString())
                                    {
                                        dat.Execute("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, To_UserID, " +
                                        "[Date], [Read], [Mode], [Live]) VALUES('" + messageBody.Replace("'", "''") + "', '" + emailSubject.Replace("'", "''") + "', " +
                                        dat.HIPPOHAPP_USERID.ToString() + ", " + item.Value + ", '" +
                                        DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).ToString() + "', 'False', 9, 'True')");
                                        dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                                                System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                                                                dvUser[0]["Email"].ToString(), emailBody, emailSubject);
                                    }
                                }
                                else
                                {
                                    keepthese += " AND NOT(UserID = " + item.Value + " AND ReoccurrID=" + row["ID"].ToString() + ")";
                                }
                            }
                            else
                            {
                                command = "INSERT INTO GroupEvent_Members (GroupEventID, UserID, ReoccurrID) VALUES(" + theID +
                                    ", " + item.Value + ", " + row["ID"].ToString() + ")";
                                dat.Execute(command);
                                DataView dvUser = dat.GetDataDV("SELECT * FROM Users WHERE User_ID=" + item.Value);
                                if (item.Value != Session["User"].ToString())
                                {
                                    dat.Execute("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, To_UserID, " +
                                    "[Date], [Read], [Mode], [Live]) VALUES('" + messageBody.Replace("'", "''") + "', '" + emailSubject.Replace("'", "''") + "', " +
                                    dat.HIPPOHAPP_USERID.ToString() + ", " + item.Value + ", '" +
                                    DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).ToString() + "', 'False', 9, 'True')");
                                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                                            System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                                                            dvUser[0]["Email"].ToString(), emailBody, emailSubject);
                                }
                            }
                        }
                    }

                    if (isUpdate)
                    {
                        dat.Execute("DELETE FROM GroupEvent_Members WHERE GroupEventID=" + theID + keepthese);
                    }
                }
                else if (ParticipantsList.SelectedValue == "2")
                {
                    foreach (DataRowView item in dvMems)
                    {
                        //execute update/insert member
                        foreach (DataRowView row in dvAllOccurances)
                        {
                            if (isUpdate)
                            {
                                dvPrevMembers.RowFilter = "UserID=" + item["MemberID"] + " AND ReoccurrID=" + 
                                    row["ID"].ToString();
                                if (dvPrevMembers.Count == 0)
                                {

                                    command = "INSERT INTO GroupEvent_Members (GroupEventID, UserID, ReoccurrID) VALUES(" + theID +
                                        ", " + item["MemberID"] + ", " + row["ID"].ToString() + ")";
                                    dat.Execute(command);
                                    keepthese += " AND NOT(UserID = " + item["MemberID"] + " AND ReoccurrID=" + 
                                        row["ID"].ToString() + ") ";

                                    DataView dvUser = dat.GetDataDV("SELECT * FROM Users WHERE User_ID=" + item["MemberID"]);
                                    if (item["MemberID"] != Session["User"].ToString())
                                    {
                                        dat.Execute("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, To_UserID, " +
                                        "[Date], [Read], [Mode], [Live]) VALUES('" + messageBody.Replace("'", "''") + "', '" + emailSubject.Replace("'", "''") + "', " +
                                        dat.HIPPOHAPP_USERID.ToString() + ", " + item["MemberID"] + ", '" +
                                        DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).ToString() + "', 'False', 9, 'True')");
                                        dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                                                System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                                                                dvUser[0]["Email"].ToString(), emailBody, emailSubject);
                                    }

                                }
                                else
                                {
                                    keepthese += " AND NOT(UserID = " + item["MemberID"] +
                                        " AND ReoccurrID=" + row["ID"].ToString() + ")";
                                }
                            }
                            else
                            {
                                command = "INSERT INTO GroupEvent_Members (GroupEventID, UserID, ReoccurrID) VALUES(" + 
                                    theID + ", " + item["MemberID"] + ", " + row["ID"].ToString() + ")"; 
                                dat.Execute(command);
                                DataView dvUser = dat.GetDataDV("SELECT * FROM Users WHERE User_ID=" + item["MemberID"]);
                                if (item["MemberID"] != Session["User"].ToString())
                                {
                                    dat.Execute("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, To_UserID, " +
                                    "[Date], [Read], [Mode], [Live]) VALUES('" + messageBody.Replace("'", "''") + "', '" + emailSubject.Replace("'", "''") + "', " +
                                    dat.HIPPOHAPP_USERID.ToString() + ", " + item["MemberID"] + ", '" +
                                    DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).ToString() + "', 'False', 9, 'True')");
                                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                                            System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                                                            dvUser[0]["Email"].ToString(), emailBody, emailSubject);
                                }
                            }
                        }
                    }

                    if (isUpdate)
                    {
                        dat.Execute("DELETE FROM GroupEvent_Members WHERE GroupEventID=" + theID + keepthese);
                    }
                }


            }
            catch (Exception ex)
            {
                MessagePanel.Visible = true;
                YourMessagesLabel.Text = ex.ToString() + "<br/>command: " + command;
            }
        }
    }

    protected void CreateGroupings(string theID, bool isUpdate)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
        conn.Open();
        SqlCommand cmd;
        DataSet dsGroupingID;
        SqlDataAdapter da;
        string groupingID = "";

        dat.Execute("DELETE FROM GroupEvent_Groupings WHERE GroupEventID=" + theID);
        dat.Execute("DELETE FROM GroupEvent_UserGroupings WHERE GroupEventID=" + theID);

        if (ParticipantsList.SelectedValue == "2" || ParticipantsList.SelectedValue == "3")
        {
            if (GroupingsTreeVie.Nodes.Count > 0)
            {
                
                
                foreach (Telerik.Web.UI.RadTreeNode node in GroupingsTreeVie.Nodes)
                {
                    cmd = new SqlCommand("INSERT INTO GroupEvent_Groupings (GroupingName, GroupingDescription, GroupEventID) " +
                        "VALUES('" + node.Text.Replace("'", "''") + "', '" + node.Value.Replace("'", "''") + "', " + theID + ")", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("SELECT @@IDENTITY AS SID", conn);
                    dsGroupingID = new DataSet();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dsGroupingID);

                    groupingID = dsGroupingID.Tables[0].Rows[0]["SID"].ToString();
                    foreach (Telerik.Web.UI.RadTreeNode noddet in node.Nodes)
                    {
                        dat.Execute("INSERT INTO GroupEvent_UserGroupings (GroupingID, UserID, GroupEventID) " +
                            "VALUES(" + groupingID + ", " + noddet.Value + ", " + theID + ")");
                    }
                }
            }
        }
    }

    protected void AddCaption(object sender, EventArgs e)
    {
        char[] delim = { '-' };
        foreach (ListItem item in PictureCheckList.Items)
        {
            if (item.Selected)
            {

                string[] tokens = item.Text.Split(delim);
                if (tokens.Length > 1)
                {
                    item.Text = tokens[0].Trim() + " - Caption Added";
                }
                else
                {
                    item.Text += " - Caption Added";
                }

                tokens = item.Value.Split(delim);

                if (tokens.Length > 1)
                {
                    item.Value = tokens[0].Trim() + " - " + CaptionTextBox.Text.Trim();
                }
                else
                {
                    item.Value += " - " + CaptionTextBox.Text.Trim();
                }
                break;

            }
        }
    }

    protected void EditCaption(object sender, EventArgs e)
    {
        char[] delim = { '-' };
        foreach (ListItem item in PictureCheckList.Items)
        {
            if (item.Selected)
            {
                string[] tokens = item.Value.Split(delim);

                if (tokens.Length > 1)
                {
                    CaptionTextBox.Text = item.Value.Replace(tokens[0].Trim() + " - ", "");
                }
                break;
            }
        }
    }

    protected void CreateCategories(string eventID, bool isUpdate)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        string message = "";
        try
        {
            Hashtable distinctHash = new Hashtable();
            Hashtable tagHash = new Hashtable();
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            if (isUpdate)
            {
                dat.Execute("DELETE FROM GroupEvent_Category WHERE GroupEvent_ID=" + eventID);
            }

            DataSet dsCategories = dat.GetData("SELECT * FROM GroupEvent_Category WHERE GroupEvent_ID=" + eventID);
            DataView dvCat = new DataView(dsCategories.Tables[0], "", "", DataViewRowState.CurrentRows);

            GetCategoriesFromTree(isUpdate, ref CategoryTree, dvCat, eventID);
            GetCategoriesFromTree(isUpdate, ref RadTreeView2, dvCat, eventID);
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = message;
        }
    }

    protected bool CategorySelected()
    {
        return OneCategorySelected(ref CategoryTree)
        || OneCategorySelected(ref RadTreeView2);
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

    protected void TabClick(object sender, EventArgs e)
    {
        int nextIndex = EventTabStrip.SelectedIndex;
        short selectThisTab = 0;
        YourMessagesLabel.Text = "";
        MessagePanel.Visible = false;
        //ErrorLabel.Text = "nextindex: " + nextIndex.ToString();

        if (Session["VenuePrevTab"] != null)
        {
            int selectedIndex = (int)Session["VenuePrevTab"];
            //ErrorLabel.Text += "selectedindex: " + selectedIndex.ToString();
            Session["VenuePrevTab"] = nextIndex;

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

            if (selectedIndex.ToString() == "5")
            {
                selectThisTab = short.Parse(nextIndex.ToString());
            }

            //ErrorLabel.Text += "selectthistab: " + selectThisTab.ToString();
        }
        else
        {
            Session["VenuePrevTab"] = nextIndex;
            selectThisTab = short.Parse(nextIndex.ToString());
        }

        ChangeSelectedTab(0, selectThisTab);
    }

    protected void Onwards(object sender, EventArgs e)
    {
        OnwardsIT(true, EventTabStrip.SelectedIndex);
    }

    protected bool OnwardsIT(bool changeTab, int selectedIndex)
    {
        try
        {
            HttpCookie cookie = Request.Cookies["BrowserDate"];

            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
            conn.Open();
            string message = "";
            bool goOn = false;
            switch (selectedIndex)
            {
                case 0:
                    #region Case 0

                    bool hasLocation = false;
                    if (DateSelectionsListBox.Items.Count > 0)
                        hasLocation = true;

                    DescriptionTextBox.Content = dat.StripHTML_LeaveLinks(DescriptionTextBox.Content.Replace("<div>", 
                        "<br/>").Replace("</div>", ""));
                    
                    if (hasLocation)
                    {
                        VenueNameTextBox.THE_TEXT = dat.stripHTML(VenueNameTextBox.THE_TEXT);
                        DescriptionTextBox.Content = dat.StripHTML_LeaveLinks(DescriptionTextBox.Content.Replace("<div>",
                            "<br/>").Replace("</div>", ""));

                        if (VenueNameTextBox.THE_TEXT.Trim() != "" && DescriptionTextBox.Text.Trim() != "")
                        {
                            if (DescriptionTextBox.Text.Length <= 420)
                            {
                                if (VenueNameTextBox.THE_TEXT.Trim().Length <= 70)
                                {
                                    if (changeTab)
                                        ChangeSelectedTab(0, 1);

                                    return true;
                                }
                                else
                                {
                                    MessagePanel.Visible = true;
                                    YourMessagesLabel.Text += "<br/><br/>*Group Event Name be under 70 characters.";
                                }
                            }
                            else
                            {
                                MessagePanel.Visible = true;
                                YourMessagesLabel.Text += "<br/><br/>*Description must be under 420 characters.";
                            }


                        }
                        else
                        {
                            MessagePanel.Visible = true;
                            YourMessagesLabel.Text += "<br/><br/>*Please fill in all required fields.";
                        }
                    }
                    else
                    {
                        MessagePanel.Visible = true;
                        YourMessagesLabel.Text += "<br/><br/>*Please specify date and location.";

                    }
                    return false;
                    #endregion
                    break;
                case 1:
                    if (changeTab)
                    {
                        ChangeSelectedTab(1, 2);
                    }
                    return true;
                    break;
                case 2:
                    if (changeTab)
                    {
                        ChangeSelectedTab(2, 3);

                    }
                    return true;
                    break;
                case 3:
                    goOn = false;
                    if (ParticipantsList.SelectedIndex != -1)
                    {
                        if (ParticipantsList.SelectedValue == "3" && GroupMembersSelectedListBox.Items.Count == 0)
                            message = "Please select the members for your Exclusive Event type.";
                        else
                        {
                            if (RagistrationRadioList.SelectedIndex != -1)
                            {
                                if (RagistrationRadioList.SelectedValue == "2" && NumRegTextBox.Text.Trim() == ""
                                    && DeadlineDatePicker.SelectedDate == null)
                                {
                                    message = "Please select either the limit of users or deadline for the Limited Registration Type.";
                                }
                                else
                                    goOn = true;
                            }
                            else
                                message = "Please select the Registration Type.";
                        }
                    }
                    else
                    {
                        message = "Please select the Event type.";
                    }

                    if (goOn)
                    {
                        if (Session["CategoriesSet"] == null && Request.QueryString["ID"] != null)
                            SetCategories();
                        if (changeTab)
                            ChangeSelectedTab(3, 4);
                        return true;
                    }
                    else
                    {
                        YourMessagesLabel.Text = message;
                        MessagePanel.Visible = true;
                        return false;
                    }
                    break;
                case 4:

                    if (CategorySelected())
                    {
                        if (changeTab)
                            ChangeSelectedTab(4, 5);

                        return true;
                    }
                    else
                    {
                        MessagePanel.Visible = true;
                        YourMessagesLabel.Text = "Must include at least one category.";
                        return false;
                    }
                    break;
                default: break;
            }

        }
        catch (Exception ex)
        {
            MessagePanel.Visible = true;
            YourMessagesLabel.Text = ex.ToString();
        }
        return false;

    }

    protected void FillLiteral() { }

    protected void Backwards(object sender, EventArgs e)
    {
        YourMessagesLabel.Text = "";
        MessagePanel.Visible = false;
        int selectedIndex = EventTabStrip.SelectedIndex;

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
                break;
            case 4:
                ChangeSelectedTab(4, 3);
                break;
            case 5:
                ChangeSelectedTab(5, 4);
                break;
            default: break;
        }
    }

    protected void PictureUpload_Click(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (PictureUpload.HasFile)
        {
            if (PictureCheckList.Items.Count < 20)
            {

                char[] delim = { '.' };
                string[] tokens = PictureUpload.FileName.Split(delim);

                if (tokens.Length > 1)
                {
                    if (tokens[1].ToUpper() == "JPEG" || tokens[1].ToUpper() == "JPG" || tokens[1].ToUpper() == "GIF" || tokens[1].ToUpper() == "PNG")
                    {
                        string fileName = "rename" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Ticks.ToString() + "." + tokens[1];
                        PictureCheckList.Items.Add(new ListItem(PictureUpload.FileName, fileName));
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

                        System.Drawing.Image img = System.Drawing.Image.FromStream(PictureUpload.PostedFile.InputStream);

                        SaveThumbnail(img, true, MapPath(".").ToString() + "/UserFiles/" + Session["UserName"].ToString() +
                            "/Slider/" + fileName, "image/" + tokens[1].ToLower(), 410, 250);

                        //PictureUpload.SaveAs(MapPath(".").ToString() + "/UserFiles/" + Session["UserName"].ToString() +
                        //    "/Slider/" + fileName);
                        PicsNVideosPanel.Visible = true;
                    }
                    else
                    {
                        YourMessagesLabel.Text = "No go! Pictures can only ge .gif, .jpg, .jpeg, or .png.";
                        MessagePanel.Visible = true;
                    }
                }
                else
                {
                    YourMessagesLabel.Text = "No go! Pictures can only ge .gif, .jpg, .jpeg, or .png.";
                    MessagePanel.Visible = true;
                }
            }
        }
    }

    protected void VideoUpload_Click(object sender, EventArgs e) { }

    protected void YouTubeUpload_Click(object sender, EventArgs e)
    {

        if (YouTubeTextBox.Text != "")
        {
            YouTubeTextBox.Text = YouTubeTextBox.Text.Trim().Replace("http://www.youtube.com/watch?v=", "");
            if (PictureCheckList.Items.Count < 20)
            {
                PictureCheckList.Items.Add(new ListItem("YouTube ID: " + YouTubeTextBox.Text, YouTubeTextBox.Text));
                PicsNVideosPanel.Visible = true;
            }
        }

    }

    protected void EnableMainAttractionPanel(object sender, EventArgs e)
    {
        if (MainAttractionCheck.Checked)
            MainAttractionPanel.Visible = true;
        else
            MainAttractionPanel.Visible = false;
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
                StateDropDown.Items.Insert(0, new ListItem("Select State..", "-1"));
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

    //protected void PictureNixIt(object sender, EventArgs e)
    //{
    //    PictureCheckList.Items.Clear();

    //    PictureNixItButton.Visible = false;
    //}

    //protected void SliderNixIt(object sender, EventArgs e)
    //{
    //    int songCount = PictureCheckList.Items.Count;
    //    CheckBoxList tempList = new CheckBoxList();
    //    for (int i = 0; i < songCount; i++)
    //    {
    //        if (!PictureCheckList.Items[i].Selected)
    //            tempList.Items.Add(PictureCheckList.Items[i]);
    //        else
    //        {
    //            if (System.IO.File.Exists(MapPath(".") + "/UserFiles/" + Session["UserName"].ToString() + "/Slider/" + PictureCheckList.Items[i].Value))
    //            {
    //                System.IO.File.Delete(MapPath(".") + "/UserFiles/" + Session["UserName"].ToString() + "/Slider/" + PictureCheckList.Items[i].Value);
    //            }
    //        }
    //    }
    //    PictureCheckList.Items.Clear();
    //    for (int j = 0; j < tempList.Items.Count; j++)
    //    {
    //        PictureCheckList.Items.Add(tempList.Items[j]);
    //    }
    //    if (PictureCheckList.Items.Count == 0)
    //        PictureNixItButton.Visible = false;
    //}

    //protected void ShowSliderOrVidPic(object sender, EventArgs e)
    //{
    //    if (MainAttracionRadioList.SelectedValue == "0")
    //    {
    //        VideoPanel.Visible = true;
    //        PicturePanel.Visible = false;
    //    }
    //    else
    //    {
    //        VideoPanel.Visible = false;
    //        PicturePanel.Visible = true;
    //    }
    //}

    //protected void VideoNixIt(object sender, EventArgs e)
    //{
    //    //VideoCheckList.Items.Clear();

    //    //VideoNixItButton.Visible = false;
    //}

    protected void SuggestCategoryClick(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        MessagePanel.Visible = true;
        CategoriesTextBox.Text = dat.stripHTML(CategoriesTextBox.Text.Trim());

        YourMessagesLabel.Text += "<br/><br/>Your category '" + CategoriesTextBox.Text +
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
            PicsNVideosPanel.Visible = false;
    }

    private void ChangeSelectedTab(int unselectIndex, short selectIndex)
    {
        Session["VenuePrevTab"] = int.Parse(selectIndex.ToString());
        EventTabStrip.Tabs[selectIndex].Enabled = true;
        EventTabStrip.Tabs[selectIndex].Selected = true;
        EventTabStrip.MultiPage.SelectedIndex = selectIndex;
        EventTabStrip.SelectedTab.TabIndex = selectIndex;
        EventTabStrip.SelectedIndex = selectIndex;
        EventTabStrip.TabIndex = selectIndex;
        //EventTabStrip.Tabs[unselectIndex].Enabled = false;
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

    private void SaveThumbnail(System.Drawing.Image image, bool isRotator, string path, string typeS, int width, int height)
    {

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

    protected void FillMemberInvites(object sender, EventArgs e)
    {

    }

    protected void RemoveMember(object sender, EventArgs e)
    {

    }

    protected void AssignTitle(object sender, EventArgs e)
    {

    }

    protected void SelectTitle(object sender, EventArgs e)
    {

    }

    protected void ChangePrivate(object sender, EventArgs e)
    {
        if (PrivatePanel.Visible)
        {
            PrivatePanel.Visible = false;
            PublicPanel.Visible = true;
        }
        else
        {
            PrivatePanel.Visible = true;
            PublicPanel.Visible = false;
        }
    }

    protected void GetNewVenue(object sender, EventArgs e)
    {
        try
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
                    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

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
                }
                catch (Exception ex)
                {
                    YourMessagesLabel.Text = ex.ToString();
                    MessagePanel.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
    
    protected void ChangeVenueStateAction(object sender, EventArgs e)
    {
        ChangeVenueState(VenueCountry.SelectedValue);
    }
    
    protected void GetVenues(object sender, EventArgs e)
    {

        GetThoseVenues();
    }

    protected void SetNewVenue(object sender, EventArgs e)
    {
        try
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
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString();
        }
    }

    protected void GetThoseVenues()
    {
        try
        {
            HttpCookie cookie = Request.Cookies["BrowserDate"];
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            TimeFrameDiv.InnerHtml = "Select Venue >";

            Session["NewVenue"] = null;
            Session.Remove("NewVenue");

            string state = "";
            if (VenueState.Visible)
                state = VenueState.SelectedItem.Text;
            else
                state = VenueStateTextBox.Text;

            SqlDbType[] types = { SqlDbType.NVarChar };
            object[] data = { state };
            DataSet ds = dat.GetDataWithParemeters("SELECT CASE WHEN SUBSTRING(Name, 1, 4) = 'The' THEN " +
                "SUBSTRING(Name, 5, LEN(Name)-4) ELSE Name END AS Name1, * FROM Venues WHERE Country=" +
                VenueCountry.SelectedValue + " AND State=@p0 ORDER BY Name1 ASC", types, data);


            Session["LocationVenues"] = ds;

            fillVenues(ds);

        }
        catch (Exception ex)
        {
            MessagePanel.Visible = true;
            YourMessagesLabel.Text += ex.ToString();
        }
    }

    protected void ChangeVenueState(string countryID)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet ds = dat.GetData("SELECT * FROM State WHERE country_id=" + countryID);


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
                litBeg.Text = "<div style=\"max-width: 800px;\"><div>";
                Literal litMid = new Literal();
                litMid.Text = "";

                char[] letters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
                char[] lettersLow = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

                int countLetters = 0;
                int max = 0;

                for (int j = 0; j < letters.Length; j++)
                {
                    dv.RowFilter = "substring(Name1,1,1) = '" + lettersLow[j] + "' OR substring(Name1,1,1) = '" + letters[j] + "' ";


                    if (dv.Count > 0)
                    {
                        countLetters++;

                        string classL = "";
                        if (countLetters == 1)
                            classL = "AddGreenLink";
                        else
                            classL = "AddWhiteLink";
                        litBeg.Text += "<div style=\"float: left; padding: 5px;\" id='titleDiv" + letters[j] +
                            "' class=\"" + classL + "\"><a onclick=\"SelectLetterDiv('" +
                            letters[j] + "');\">" + letters[j] + "</a></div>";
                        string displayL = "";
                        if (countLetters == 1)
                            displayL = "block";
                        else
                            displayL = "none";
                        litMid.Text += "<div id='contentDiv" + letters[j] + "' style=\"display: " +
                            displayL + "; float: left;\">";
                        for (int i = 0; i < dv.Count; i++)
                        {
                            string borderN = "";
                            if (i != dv.Count - 1)
                                borderN = "border-right: solid 1px white;";
                            litMid.Text += "<div style=\"" + borderN + "padding: 5px; float: left; width: 100px;\"><a onclick=\"fillDrop('" +
                                dv[i]["Name"].ToString().Replace("'", "&&") + "', " +
                                dv[i]["ID"].ToString() + ");\" class=\"AddLink\">" +
                                dv[i]["Name"].ToString() + "</a></div>";
                        }
                        litMid.Text += "</div>";

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

                MessagePanel.Visible = true;
                YourMessagesLabel.Text += "<br/><br/>No venues found. Create one my clicking on 'New Venue' radio button.";
            }
        else
        {
            Literal litMid = new Literal();
            litMid.Text = "No venues found for this location. Create one my clicking on 'New Venue' radio button.";

            Tip1.Controls.Add(litMid);

            MessagePanel.Visible = true;
            YourMessagesLabel.Text += "<br/><br/>No venues found. Create one my clicking on 'New Venue' radio button.";
        }
    }

    protected void AddAgendaItem(object sender, EventArgs e)
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
        AgendaItemTextBox.Text = dat.stripHTML(AgendaItemTextBox.Text.Trim());
        AgendaDescriptionTextBox.Text = dat.stripHTML(AgendaDescriptionTextBox.Text.Trim());
        if (AgendaItemTextBox.Text.Trim() != "")
        {
            AgendaLiteral.Text += "<div style=\"padding: 0px; padding-bottom: 3px;\" class=\"AddLink\">" +
                dat.BreakUpString(AgendaItemTextBox.Text.Trim(), 44) + "</div>";
            if (AgendaDescriptionTextBox.Text.Trim() != "")
            {
                AgendaLiteral.Text += "<div style=\"padding: 0px; padding-left: 20px; padding-bottom: 3px; color: #cccccc; font-family: arial; font-size: 11px;\">" +
                dat.BreakUpString(AgendaDescriptionTextBox.Text.Trim(), 44) + "</div>";
            }
        }
        else
        {
            AgendaErrorLabel.Text = "Must include the item title.";
        }
    }

    protected void AddStuffNeed(object sender, EventArgs e)
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
        OneStuffNeededTextBox.Text = dat.stripHTML(OneStuffNeededTextBox.Text.Trim());
        if (OneStuffNeededTextBox.Text.Trim() != "")
        {
            StuffNeededListBox.Items.Add(new ListItem(OneStuffNeededTextBox.Text.Trim()));
        }
    }

    protected void ShowAgendaPanel(object sender, EventArgs e)
    {
        if (AgendaCheckBox.Checked)
            AgendaPanel.Visible = true;
        else
            AgendaPanel.Visible = false;
    }

    protected void ShowStuffPanel(object sender, EventArgs e)
    {
        if (StuffCheckBox.Checked)
            StuffPanel.Visible = true;
        else
            StuffPanel.Visible = false;
    }

    protected void RemoveOneAgenda(object sender, EventArgs e)
    {
        if (AgendaLiteral.Text.Trim() != "")
        {
            int i = AgendaLiteral.Text.Length - 1;
            char a = AgendaLiteral.Text[i];
            string agendaOne = AgendaLiteral.Text.Remove(AgendaLiteral.Text.Length - 1);
            int toGoCount = 0;
            char d = ' ';
            while (d == ' ')
            {
                a = AgendaLiteral.Text[--i];
                agendaOne = agendaOne.Remove(agendaOne.Length - 1);
                if (a == '<')
                {
                    toGoCount++;
                    if (toGoCount == 2)
                        d = 'i';
                }
            }

            AgendaLiteral.Text = agendaOne;
        }
    }

    protected void RemoveStuffNeed(object sender, EventArgs e)
    {
        if (StuffNeededListBox.SelectedIndex != -1)
        {
            StuffNeededListBox.Items.Remove(StuffNeededListBox.SelectedItem);
        }
    }

    protected void ChangeUpParticipants(object sender, EventArgs e)
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

        try
        {
            GroupingsTreeVie.Nodes.Clear();
            if (ParticipantsList.SelectedValue == "1")
            {
                PublicEventPanel.Visible = true;
                PrivateEventPanel.Visible = false;
                ExclusiveEventPanel.Visible = false;
                RegistrationPanel.Visible = true;
                TheGroupingsPanel.Visible = false;
            }
            else if (ParticipantsList.SelectedValue == "2")
            {
                PublicEventPanel.Visible = false;
                PrivateEventPanel.Visible = true;
                ExclusiveEventPanel.Visible = false;
                RegistrationPanel.Visible = true;
                TheGroupingsPanel.Visible = true;

                GroupingParticipantsListBox.Items.Clear();

                DataView dvGroupMems = dat.GetDataDV("SELECT * FROM Users U, Group_Members GM WHERE GM.GroupID=" +
                    Request.QueryString["GroupID"].ToString() + " AND U.User_ID=GM.MemberID");

                foreach (DataRowView row in dvGroupMems)
                {
                    GroupingParticipantsListBox.Items.Add(new ListItem(row["UserName"].ToString(), row["User_ID"].ToString()));
                }
            }
            else if (ParticipantsList.SelectedValue == "3")
            {
                PublicEventPanel.Visible = false;
                PrivateEventPanel.Visible = false;
                ExclusiveEventPanel.Visible = true;
                RegistrationPanel.Visible = true;
                TheGroupingsPanel.Visible = true;

                DataView dvMembers = dat.GetDataDV("SELECT * FROM Group_Members GM, Users U WHERE " +
                    "GM.MemberID=U.User_ID AND GM.GroupID=" + Request.QueryString["GroupID"].ToString());
                GroupMembersListBox.DataSource = dvMembers;
                GroupMembersListBox.DataTextField = "UserName";
                GroupMembersListBox.DataValueField = "User_ID";
                GroupMembersListBox.DataBind();

                GroupingParticipantsListBox.Items.Clear();

                foreach (ListItem item in GroupMembersSelectedListBox.Items)
                {
                    GroupingParticipantsListBox.Items.Add(item);
                }
            }
        }
        catch (Exception ex)
        {
            ParticipantsErrorLabel.Text = ex.ToString();
        }
    }

    protected void ChangeRegist(object sender, EventArgs e)
    {
        if (RagistrationRadioList.SelectedValue == "1")
            LimitedRegistrationPanel.Visible = false;
        else
            LimitedRegistrationPanel.Visible = true;
    }

    protected void AddParticipant(object sender, EventArgs e)
    {
        if (GroupMembersListBox.SelectedIndex != -1)
        {
            if (!GroupMembersSelectedListBox.Items.Contains(new ListItem(GroupMembersListBox.SelectedItem.Text,
                GroupMembersListBox.SelectedItem.Value)))
            {
                GroupMembersSelectedListBox.Items.Add(new ListItem(GroupMembersListBox.SelectedItem.Text,
                    GroupMembersListBox.SelectedItem.Value));

                GroupingParticipantsListBox.Items.Add(new ListItem(GroupMembersListBox.SelectedItem.Text,
                    GroupMembersListBox.SelectedItem.Value));
            }
        }
    }

    protected void RemoveParticipant(object sender, EventArgs e)
    {
        if (GroupMembersSelectedListBox.SelectedIndex != -1)
        {
            GroupingParticipantsListBox.Items.Remove(GroupMembersSelectedListBox.SelectedItem);

            bool goOn = true;

            foreach (Telerik.Web.UI.RadTreeNode node in GroupingsTreeVie.Nodes)
            {
                if (!goOn)
                    break;
                if (node.Nodes.Count > 0)
                {
                    foreach (Telerik.Web.UI.RadTreeNode n1 in node.Nodes)
                    {
                        if (n1.Text == GroupMembersSelectedListBox.SelectedItem.Text)
                        {
                            node.Nodes.Remove(n1);
                            goOn = false;
                            break;
                        }
                    }
                }
            }

            GroupMembersSelectedListBox.Items.Remove(GroupMembersSelectedListBox.SelectedItem);
        }
    }

    protected void OpenGroupings(object sender, EventArgs e)
    {
        if (GroupingsCheckBox.Checked)
            GroupingPanel.Visible = true;
        else
            GroupingPanel.Visible = false;
    }

    protected void AddGrouping(object sender, EventArgs e)
    {
        if (GroupingNameTextBox.Text.Trim() != "")
        {
            if (!GroupingsTreeVie.Nodes.Contains(new Telerik.Web.UI.RadTreeNode(GroupingNameTextBox.Text.Trim(),
                GroupingDescriptionTextBox.Text.Trim())))
            {
                GroupingsTreeVie.Nodes.Add(new Telerik.Web.UI.RadTreeNode(GroupingNameTextBox.Text.Trim(),
                    GroupingDescriptionTextBox.Text.Trim()));
            }
        }
    }

    protected void RemoveGrouping(object sender, EventArgs e)
    {
        if (GroupingsTreeVie.SelectedNode != null)
        {
            if (GroupingsTreeVie.SelectedNode.Level == 0)
            {
                if (GroupingsTreeVie.SelectedNode.Nodes.Count > 0)
                {
                    foreach (Telerik.Web.UI.RadTreeNode node in GroupingsTreeVie.SelectedNode.Nodes)
                    {
                        GroupingParticipantsListBox.Items.Add(new ListItem(node.Text, node.Value));
                    }
                }
                GroupingsTreeVie.Nodes.Remove(GroupingsTreeVie.SelectedNode);
            }
        }
    }

    protected void AssignParticipantGrouping(object sender, EventArgs e)
    {
        if (GroupingParticipantsListBox.SelectedIndex != -1)
        {
            if (GroupingsTreeVie.SelectedNode != null)
            {
                if (GroupingsTreeVie.SelectedNode.Level == 0)
                {
                    GroupingsTreeVie.SelectedNode.Nodes.Add(new Telerik.Web.UI.RadTreeNode(
                        GroupingParticipantsListBox.SelectedItem.Text, GroupingParticipantsListBox.SelectedItem.Value));
                    GroupingParticipantsListBox.Items.Remove(GroupingParticipantsListBox.SelectedItem);
                    GroupingsTreeVie.SelectedNode.Expanded = true;
                }
                else
                {
                    GroupingErrorLabel.Text = "Please select a Grouping.";
                }
            }
            else
            {
                GroupingErrorLabel.Text = "Please select a Grouping.";
            }
        }
        else
        {
            GroupingErrorLabel.Text = "Please select a Participant.";
        }
    }

    protected void RemoveParticipantGrouping(object sender, EventArgs e)
    {
        if (GroupingsTreeVie.SelectedNode != null)
        {
            if (GroupingsTreeVie.SelectedNode.Level == 1)
            {
                GroupingParticipantsListBox.Items.Add(new ListItem(GroupingsTreeVie.SelectedNode.Text, GroupingsTreeVie.SelectedNode.Value));
                GroupingsTreeVie.SelectedNode.ParentNode.Nodes.Remove(GroupingsTreeVie.SelectedNode);
            }
            else
            {
                GroupingErrorLabel.Text = "Please select a Participant to remove.";
            }
        }
        else
        {
            GroupingErrorLabel.Text = "Please select a Participant to remove.";
        }
    }

    protected void ChangeColorPanel(object sender, EventArgs e)
    {
        ColorBackRadioButton.Checked = true;
        ColorTextRadioButton.Checked = false;
        if (ColorSchemeRadioList.SelectedValue == "1")
        {
            ColorPanel.Visible = true;
        }
        else
        {
            ColorTd1.BgColor = "#568301";
            ColorTd2.BgColor = "#1fb6e7";
            ColorPanel.Visible = false;
        }
    }

    protected void ChangeTextColorPanel(object sender, EventArgs e)
    {
        ColorBackRadioButton.Checked = false;
        ColorTextRadioButton.Checked = true;
        if (ColorTextRadioList.SelectedValue == "1")
        {
            ColorPanel.Visible = true;
        }
        else
        {
            ColorPanel.Visible = false;
        }
    }

    protected void ChangeColor(object sender, EventArgs e)
    {
        if (ColorBackRadioButton.Checked)
        {
            if (Color1CheckBox.Checked)
            {
                ColorTd1.BgColor = System.Drawing.ColorTranslator.ToHtml(RadColorPicker1.SelectedColor);
            }

            if (Color2CheckBox.Checked)
            {
                ColorTd2.BgColor = System.Drawing.ColorTranslator.ToHtml(RadColorPicker1.SelectedColor);
            }
        }

        if (ColorTextRadioButton.Checked)
        {
            if (Color1CheckBox.Checked)
            {
                Text1Label.ForeColor = RadColorPicker1.SelectedColor;
            }

            if (Color2CheckBox.Checked)
            {
                Text2Label.ForeColor = RadColorPicker1.SelectedColor;
            }
        }

    }

    protected void SelectColorBackNText(object sender, EventArgs e)
    {
        RadioButton radB = (RadioButton)sender;
        if (radB == ColorTextRadioButton)
        {
            ColorBackRadioButton.Checked = false;
            ColorTextRadioList.SelectedValue = "1";
            ColorPanel.Visible = true;
        }
        else
        {
            ColorTextRadioButton.Checked = false;
            ColorSchemeRadioList.SelectedValue = "1";
            ColorPanel.Visible = true;
        }
    }
}
