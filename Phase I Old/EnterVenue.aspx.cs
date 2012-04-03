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
        HtmlLink lk = new HtmlLink();
        HtmlHead head = (HtmlHead)Page.Header;
        lk.Attributes.Add("rel", "canonical");
        lk.Href = "http://hippohappenings.com/EnterVenue.aspx";
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
        string cookieName = FormsAuthentication.FormsCookieName;
        HttpCookie authCookie = Context.Request.Cookies[cookieName];

        FormsAuthenticationTicket authTicket = null;

        ImageButton5.PostBackUrl = Request.Url.AbsoluteUri;

        Button button = (Button)dat.FindControlRecursive(this, "Button2");
        button.CssClass = "NavBarImageAddVenueSelected";

        DataView dv = dat.GetDataDV("SELECT * FROM TermsAndConditions");
        Literal lit1 = new Literal();
        lit1.Text = dv[0]["Content"].ToString();
        TACTextBox.Controls.Add(lit1);

        if (!IsPostBack)
        {
            
            Session.Remove("CategoriesSet");
            Session["CategoriesSet"] = null;

            CountryDropDown.SelectedValue = "223";
            DataSet dsCountries = dat.GetData("SELECT * FROM State WHERE country_id=223");
           
            StateDropDown.DataSource = dsCountries;
            StateDropDown.DataTextField = "state_2_code";
            StateDropDown.DataValueField = "state_id";
            StateDropDown.DataBind();
            StateDropDownPanel.Visible = true;
            StateTextBoxPanel.Visible = false;
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
                WelcomeLabel.Text = "On Hippo Happenings, the users have all the power to post event, ads and venues alike. In order to do so, and for us to maintain clean and manageable content,  "
                    + " we require that you <a class=\"AddLink\" href=\"Register.aspx\">create an account</a> with us. <a class=\"AddLink\" href=\"UserLogin.aspx\">Log in</a> if you have an account already. " +
                    "Having an account with us will allow you to do many other things as well. You'll will be able to add events to your calendar, communicate with others going to events, add your friends, filter content based on your preferences, " +
                    " feature your ads thoughout the site and much more. <br/><br/>So let's go <a class=\"AddLink\" style=\"font-size: 16px;\" href=\"Register.aspx\">Register!</a>";

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
    
    protected void Page_Init(object sender, EventArgs e)
    {
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

        bool ownerUpForGrabs = false;
        bool isOwner = EnableOwnerPanel(ref ownerUpForGrabs);

        if (isOwner || ownerUpForGrabs)
        {
            PictureNixItButton.Visible = true;
        }

        if (dsVenue.Tables.Count > 0)
            if (dsVenue.Tables[0].Rows.Count > 0)
            {
                TitleLabel.Text = "You are submitting changes for venue '" + dsVenue.Tables[0].Rows[0]["Name"].ToString() +
                    "'";
                VenueNameTextBox.THE_TEXT = dsVenue.Tables[0].Rows[0]["Name"].ToString();
                PhoneTextBox.Text = dsVenue.Tables[0].Rows[0]["Phone"].ToString();
                EmailTextBox.Text = dsVenue.Tables[0].Rows[0]["Email"].ToString();
                WebSiteTextBox.Text = dsVenue.Tables[0].Rows[0]["Web"].ToString();
                DescriptionTextBox.Content = dsVenue.Tables[0].Rows[0]["Content"].ToString();
                ZipTextBox.Text = dsVenue.Tables[0].Rows[0]["Zip"].ToString();
                CityTextBox.Text = dsVenue.Tables[0].Rows[0]["City"].ToString();
                DataSet dsCountries = dat.GetData("SELECT * FROM State WHERE country_id=" + dsVenue.Tables[0].Rows[0]["Country"].ToString());
                CountryDropDown.SelectedValue = dsVenue.Tables[0].Rows[0]["Country"].ToString();
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
                    StateTextBox.THE_TEXT = dsVenue.Tables[0].Rows[0]["State"].ToString();
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
    }

    protected void SetCategories()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet dsCategories = dat.GetData("SELECT * FROM Venue_Category WHERE VENUE_ID=" +
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

                        node = (Telerik.Web.UI.RadTreeNode)RadTreeView1.FindNodeByValue(dsCategories.Tables[0].Rows[i]["Category_ID"].ToString());
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
                            else
                            {
                                node = (Telerik.Web.UI.RadTreeNode)RadTreeView3.FindNodeByValue(dsCategories.Tables[0].Rows[i]["Category_ID"].ToString());
                                if (node != null)
                                {
                                    node.Checked = true;
                                    //node.Enabled = false;
                                }

                            }

                        }
                    }
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
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (AgreeCheckBox.Checked)
        {
            
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
            conn.Open();

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

            if (Request.QueryString["ID"] != null)
            {
                dsVenue = dat.GetData("SELECT * FROM Venues WHERE ID=" + Request.QueryString["ID"].ToString());
                isUpdate = true;
                ownerUpForGrabs = dat.IsOwnerUpForGrabs(Request.QueryString["ID"].ToString(), ref ownerID, ref isOwner, true);
            }

            string state = "";
                if (StateDropDownPanel.Visible)
                    state = StateDropDown.SelectedItem.Text;
                else
                    state = StateTextBox.THE_TEXT;

            //We already do this in 'Onwards' method
            //SqlCommand cmd = new SqlCommand("SELECT * FROM Venues WHERE Name=@name AND City=@city AND State=@state ", conn);
            //cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = VenueNameTextBox.THE_TEXT;
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
            //        YourMessagesLabel.Text += "<br/><br/>A venue under this name already exists in this City and State. To edit the details of this particular venue please contact Hippo Happenings " + "<a class=\"AddGreenLink\" href=\"ContactUs.aspx\">here</a>. Otherwise, please modify the name slightly.";
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
                        command = "UPDATE Venues SET Name=@name, Owner=@owner, City=@city, Edit='False', Email=@email, Phone=@phone, State=@state, Country=@country, Zip=@zip, Address=@address, "
                        + " EditedByUser=@user, Content=@content, Web=@web, mediaCategory=" + mediaCat + ", LastEditOn=@dateE WHERE ID=" + Request.QueryString["ID"].ToString();
                    else
                    {
                        command = "INSERT INTO VenueRevisions (Web, modifierID, VenueID, [Content], " +
                         "City, State, Country, Zip, Name, Email, Phone, Address, DATE)"
                         + " VALUES(@web, "+Session["User"].ToString()+"," + Request.QueryString["ID"].ToString() + 
                         ", @content, @city, @state, @country, @zip, "+
                         "@name, @email, @phone, @address, '"+DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"))+"')";
                        
                    }
                }
                else
                    command = "INSERT INTO Venues (Web, Owner, City, State, Country, Zip, Edit, Name, Email, Phone, Address, CreatedByUser,Content, mediaCategory, Rating, PostedOn) "
                     + "VALUES(@web, @owner, @city, @state, @country, @zip, 'False', @name, @email, @phone, @address, @user, @content, " + mediaCat + ", 0, @dateE)";

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
                cmd.Parameters.Add("@dateE", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
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

                    if (dsVenue.Tables[0].Rows[0]["Name"].ToString() != VenueNameTextBox.THE_TEXT.Trim())
                    {
                        cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = VenueNameTextBox.THE_TEXT.Trim();
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
                        if(EmailTextBox.Text.Trim() != "")
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
                        if(OwnerCheckBox.Checked)
                            cmd.Parameters.Add("@owner", SqlDbType.Int).Value = Session["User"].ToString();
                        else
                            cmd.Parameters.Add("@owner", SqlDbType.Int).Value = DBNull.Value;
                    }
                    else
                    {
                        
                    }
                    
                    cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = VenueNameTextBox.THE_TEXT.Trim();

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
                    System.IO.Directory.CreateDirectory(MapPath(".") + "\\VenueFiles\\" + ID+"\\");
                    System.IO.Directory.CreateDirectory(MapPath(".") + "\\VenueFiles\\" + ID + "\\Slider\\");
                }
                else
                {
                    if (!System.IO.Directory.Exists(MapPath(".") + "\\VenueFiles\\" + ID))
                    {
                        System.IO.Directory.CreateDirectory(MapPath(".") + "\\VenueFiles\\" + ID+"\\");
                        System.IO.Directory.CreateDirectory(MapPath(".") + "\\VenueFiles\\" + ID+"\\Slider\\");
                    }
                    else
                    {
                        if (!!System.IO.Directory.Exists(MapPath(".")+"\\VenueFiles\\" + ID+"\\Slider\\"))
                            System.IO.Directory.CreateDirectory(MapPath(".")+"\\VenueFiles\\" + ID+"\\Slider\\");
                    }
                }

                string YouTubeStr = "";
                char[] delim3 = { '.'};
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

                                    



                                    cmd = new SqlCommand("INSERT INTO Venue_Slider_Mapping (VenueID, PictureName, RealPictureName) "+
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

                string emailBody = "<br/><br/>Dear " + dsUser.Tables[0].Rows[0]["UserName"].ToString() + ", <br/><br/> you have successfully posted the venue \"" + VenueNameTextBox.THE_TEXT +
                   "\". <br/><br/> You can find this venue <a href=\"http://hippohappenings.com/" + dat.MakeNiceName(VenueNameTextBox.THE_TEXT) + "_" + ID + "_Venue\">here</a>. " +
                   "<br/><br/> To rate your experience posting this venue <a href=\"http://hippohappenings.com/RateExperience.aspx?Edit=" + isUpdate.ToString() + "&Type=V&ID=" + ID + "\">please include your feedback here.</a>" +
                   "<br/><br/><br/>Have a Hippo Happening Day!<br/><br/>";
                    
                    //MessageLiteral.Text = "<script type=\"text/javascript\">alert('" + message + "');</script>";

                     if (isUpdate && !isOwner)
                     {
                         if (!ownerUpForGrabs)
                         {
                             DataSet dsEventUser = dat.GetData("SELECT * FROM Users U WHERE User_ID=" + ownerID);
                             emailBody = "<br/><br/>A change request has been submitted for a venue you are the owner of on HippoHappenings: \"" + VenueNameTextBox.THE_TEXT.Trim() +
                                 "\". <br/><br/> You can find this venue <a href=\"http://hippohappenings.com/" + dat.MakeNiceName(VenueNameTextBox.THE_TEXT) + "_" + ID + "_Venue\">here</a>. " +
                                 "<br/><br/> Please log into Hippo Happenings and check your messages to view and approve these changes.</a>" +
                                 "<br/><br/><br/>Have a Hippo Happening Day!<br/><br/>";

                             //conn.Open();

                             SqlCommand cmd34 = new SqlCommand("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, To_UserID, Date, [Read], Mode, Live, SentLive) VALUES('" +
                                 "VenueID:" + Request.QueryString["ID"].ToString() + ",UserID:" + Session["User"].ToString() + ",RevisionID:" + revisionID + "',@content, "+dat.HIPPOHAPP_USERID.ToString()+", " + ownerID + ", '" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).ToString() + "', 0, 5, 1, 1)", conn);
                             cmd34.Parameters.Add("@content", SqlDbType.NVarChar).Value = "A change request has been submitted for a venue you've created: " +
                                 VenueNameTextBox.THE_TEXT;
                             cmd34.ExecuteNonQuery();
                             conn.Close();
                             if (!Request.Url.AbsoluteUri.Contains("localhost"))
                             {
                                 dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                 System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                                 dsEventUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "A change request has been submitted for a venue you own on HippoHappenings: " +
                                 VenueNameTextBox.THE_TEXT);
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
                             //    VenueNameTextBox.THE_TEXT);
                         }
                         else
                         {
                             if (ownerUpForGrabs)
                             {
                                 emailBody = "<br/><br/>You have successfully submitted updates for venue: \"" + VenueNameTextBox.THE_TEXT.Trim() +
                                 "\". <br/><br/> You can find this venue <a href=\"http://hippohappenings.com/" + dat.MakeNiceName(VenueNameTextBox.THE_TEXT) + "_" + ID + "_Venue\">here</a>. " +
                                 "<br/><br/> To rate your experience posting this venue <a href=\"http://hippohappenings.com/RateExperience.aspx?Edit=" + 
                                 isUpdate.ToString() + "&Type=V&ID=" + ID + "\">please include your feedback here.</a><br/><br/>"+
                                 "Have a Hippo Happening Day!<br/><br/>";
                             }
                             else
                             {
                                 emailBody = "<br/><br/>You have successfully submitted updates for venue: \"" + VenueNameTextBox.THE_TEXT.Trim() +
                                 "\". <br/><br/> You can find this venue <a href=\"http://hippohappenings.com/" + dat.MakeNiceName(VenueNameTextBox.THE_TEXT) + "_" + ID + "_Venue\">here</a>. " +
                                 "<br/><br/> The owner of the venue will need to approve/reject your change suggestions. If you do not hear back " +
                                 "from the venue's owner within 7 days, you will be allowed to take over ownership of this venue and automatically submit changes. That is, if no one else beats you to it! " +
                                 "If you have chosen to take over ownership, a button will be available for you on the venue's page. If you have not, you will need to edit the venue's details again." +
                                 "<br/><br/><br/>Have a Hippo Happening Day!<br/><br/>";
                             }

                             if (!Request.Url.AbsoluteUri.Contains("localhost"))
                             {
                                 dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                     System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                                     dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "You have submitted updates for venue: " +
                                     VenueNameTextBox.THE_TEXT);
                             } 
                         }
                     }
                     else
                     {
                         if (!Request.Url.AbsoluteUri.Contains("localhost"))
                         {
                             dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                 System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                                 dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "You have posted the venue: " +
                                 VenueNameTextBox.THE_TEXT);
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
                                string OwnerHistoryID = dat.GetData("SELECT * FROM VenueOwnerHistory WHERE VenueID="+
                                    Request.QueryString["ID"].ToString()+" AND OwnerID="+Session["User"].ToString()+
                                    " ORDER BY DateCreatedOwnership DESC").Tables[0].Rows[0]["ID"].ToString();

                                dat.Execute("UPDATE VenueOwnerHistory SET DateLostOwnership='"+DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).ToString()+
                                    "', GaveUpOwnership='True' WHERE ID="+OwnerHistoryID);
                            }
                        }
                        else
                        {
                            if (OwnerCheckBox.Checked)
                            {
                                dat.Execute("INSERT INTO VenueOwnerHistory (VenueID, OwnerID, DateCreatedOwnership) "+
                                    "VALUES("+Request.QueryString["ID"].ToString()+", "+Session["User"].ToString()+
                                    ", '"+DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).ToString()+"')");
                            }
                            
                        }
                    }
                }
                else
                {
                    dat.Execute("INSERT INTO VenueOwnerHistory (VenueID, OwnerID, DateCreatedOwnership) VALUES("+ID+", "+Session["User"].ToString()+", '"+DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).ToString()+"')");
                }


                //pop up the message to the user
                Encryption encrypt = new Encryption();

                if (isOwner || !isUpdate)
                {
                    Session["Message"] = "Your venue has been posted successfully! An email with this info will also be sent to your account.<br/>"
                        + "Check out <a class=\"AddLink\" onclick=\"Search('" + dat.MakeNiceName(VenueNameTextBox.THE_TEXT) + "_" + ID + "_Venue');\">this venues's</a> home page.<br/><br/><br/> -<a class=\"AddLink\" onclick=\"Search('RateExperience.aspx?Edit=" + isUpdate.ToString() + "&Type=V&ID=" + ID + "');\" >Rate </a>your user experience posting this venue.<br/>";
                    //MessageLiteral.Text = "<script type=\"text/javascript\">alert('" + message + "');</script>";
                
                }
                else
                {
                    if (ownerUpForGrabs)
                    {
                        Session["Message"] = "You have successfully submitted updates for venue: \"" + VenueNameTextBox.THE_TEXT.Trim() +
                        "\".<br/><br/>Check out <a class=\"AddLink\" onclick=\"Search('" + dat.MakeNiceName(VenueNameTextBox.THE_TEXT) + "_" + ID + 
                        "_Venue');\">this venues's</a> home page.<br/><br/> <a class=\"AddLink\" onclick=\"Search('RateExperience.aspx?Edit=" + 
                        isUpdate.ToString() + "&Type=V&ID=" + ID + "');\" >Rate </a>your user experience editing this venue.<br/>";

                    }
                    else
                    {
                        Session["Message"] = "You have successfully submitted updates for this venue." +
                                 "<br/><br/> The owner of the venue will need to <b>approve/reject</b> your change suggestions. If you do not hear back " +
                                 "from the venue's owner within <b>7 days</b>, you will be allowed to <b>take over ownership</b> of this venue and automatically submit changes. That is, if no one else beats you to it! " +
                                 "If you have chosen to take over ownership, a button will be available for you on the venue's page to do so. If you have not, you will need to edit the venue's details again.<br/><br/>"
                            + "Check out <a class=\"AddLink\" onclick=\"Search('" + dat.MakeNiceName(VenueNameTextBox.THE_TEXT) + "_" + ID + "_Venue');\">this venues's</a> home page.<br/><br/> <a class=\"AddLink\" onclick=\"Search('RateExperience.aspx?Edit=" + isUpdate.ToString() + "&Type=V&ID=" + ID + "');\" >Rate </a>your user experience editing this venue.<br/>";

                        MessageRadWindow.Width = 530;
                        MessageRadWindow.Height = int.Parse(MessageRadWindow.Height.Value.ToString()) + 20;
                    }


                    
                        
                    
                } 

                MessageRadWindow.NavigateUrl = "Message.aspx?message=" + encrypt.encrypt(Session["Message"].ToString() + "<br/><img onclick=\"Search('Home.aspx');\" onmouseover=\"this.src='image/DoneSonButtonSelected.png'\" onmouseout=\"this.src='image/DoneSonButton.png'\" src=\"image/DoneSonButton.png\"/>");
                MessageRadWindow.Visible = true;
                MessageRadWindowManager.VisibleOnPageLoad = true;
            }
        }
        else
        {
            MessagePanel.Visible = true;
            YourMessagesLabel.Text += "<br/><br/>You must agree to the terms and conditions.";
        }
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
            GetCategoriesFromTree(isUpdate, isOwner, ref RadTreeView1, dvCat, ID, revisionID);
            GetCategoriesFromTree(isUpdate, isOwner, ref RadTreeView2, dvCat, ID, revisionID);
            GetCategoriesFromTree(isUpdate, isOwner, ref RadTreeView3, dvCat, ID, revisionID);


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

            if (selectedIndex.ToString() == "4")
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
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        
      Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
      SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
      conn.Open();

        switch (selectedIndex)
        {
            case 0:
                #region Case 0

                bool goOn = false;

                bool isInternational = false;
                string locationStr = "";

                VenueNameTextBox.THE_TEXT = dat.stripHTML(VenueNameTextBox.THE_TEXT);
                CityTextBox.Text = dat.stripHTML(CityTextBox.Text);
                ZipTextBox.Text = dat.stripHTML(ZipTextBox.Text);
                if (CountryDropDown.SelectedValue == "223")
                {
                    isInternational = true;
                    string apt = "";
                    if(AptNumberTextBox.Text.Trim() != "")
                        apt = AptDropDown.SelectedItem.Text + " " + AptNumberTextBox.Text.Trim().ToLower();
                    locationStr = StreetNumberTextBox.Text.Trim().ToLower() + ";" + StreetNameTextBox.Text.Trim().ToLower()
                    + ";" + StreetDropDown.SelectedItem.Text + ";" + apt;
                    goOn = VenueNameTextBox.THE_TEXT.Trim() != "" && StreetNumberTextBox.Text.Trim() != ""
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
                    goOn = VenueNameTextBox.THE_TEXT.Trim() != "" && LocationTextBox.Text.Trim() != "" &&
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
                            MessagePanel.Visible = true;
                            YourMessagesLabel.Text += "<br/><br/>*Email address is not in the appropriate format.";
                            goOn = false;
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
                                MessagePanel.Visible = true;
                                YourMessagesLabel.Text += "<br/><br/>*Zip must be a 5 digit code.";
                                goOn = false;
                            }
                        }
                        else
                        {
                            MessagePanel.Visible = true;
                            YourMessagesLabel.Text += "<br/><br/>*Zip must be a 5 digit code.";
                            goOn = false;
                        }
                    }

                    if (StateDropDown.Visible)
                    {
                        if (StateDropDown.SelectedIndex == -1)
                        {
                            MessagePanel.Visible = true;
                            YourMessagesLabel.Text += "<br/><br/>*Must include state.";
                            goOn = false;
                        }

                    }
                    else
                    {
                        if (StateTextBox.THE_TEXT.Trim() == "")
                        {
                            MessagePanel.Visible = true;
                            YourMessagesLabel.Text += "<br/><br/>*Must include state.";
                            goOn = false;
                        }
                    }


                    if (!isUpdate && goOn)
                    {
                        string state = "";
                        if (StateDropDownPanel.Visible)
                            state = StateDropDown.SelectedItem.Text;
                        else
                            state = StateTextBox.THE_TEXT;

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
                                MessagePanel.Visible = true;
                                YourMessagesLabel.Text += "<br/><br/>A venue already exists under this address. " +
                                    "Please take a look at it " +
                                    "<a target=\"_blank\" class=\"AddGreenLink\" href=\"" + dat.MakeNiceName(VenueNameTextBox.THE_TEXT) + "_" +
                                    ds.Tables[0].Rows[0]["ID"].ToString() + "_Venue\">here.</a> You can submit edits to the existing venue by licking on the 'Edit Venue' link at the top of the venue page.";
                                goOn = false;
                            }
                    }





                    if (goOn)
                    {
                        if(changeTab)
                            ChangeSelectedTab(0, 1);

                        FillLiteral();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    MessagePanel.Visible = true;
                    YourMessagesLabel.Text += "<br/><br/>*Please fill in all fields.";
                }
                return false;
                #endregion
                break;
            case 1:
                DescriptionTextBox.Content = dat.StripHTML_LeaveLinks(DescriptionTextBox.Content.Replace("<div>", "<br/>").Replace("</div>", ""));
                if (DescriptionTextBox.Text.Length >= 50)
                {
                    if(changeTab)
                        ChangeSelectedTab(1, 2);

                    FillLiteral();
                    return true;
                }
                else
                {
                    MessagePanel.Visible = true;
                    YourMessagesLabel.Text += "<br/><br/>*Make sure you say what you want your viewers to hear about this venue! The description must be at least 50 characters.";
                    return false;
                }
                break;
            case 2:
                if(Session["CategoriesSet"] == null && Request.QueryString["ID"] != null)
                    SetCategories();

                if (changeTab)
                {
                    ChangeSelectedTab(2, 3);
                    
                }
                FillLiteral();
                return true;
                break;
            case 3:
                if (CategorySelected())
                {
                    FillLiteral();

                    bool isownerupforgrabs = false;
                    EnableOwnerPanel(ref isownerupforgrabs);

                    if(changeTab)
                        ChangeSelectedTab(3, 4);

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

        return false;
    }

    protected void FillLiteral()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        EventPanel.Visible = true;
        ShowEventName.Text = VenueNameTextBox.THE_TEXT;

        //if (DescriptionTextBox.Content.Length > 500)
        //{
        ShowDescriptionBegining.Text = dat.BreakUpString(DescriptionTextBox.Content, 60);
        //int j = 500;
        //if (DescriptionTextBox.Content[500] != ' ')
        //{

        //    while (DescriptionTextBox.Content[j] != ' ')
        //    {
        //        ShowDescriptionBegining.Text += DescriptionTextBox.Content[j];
        //        j++;

        //        if (j >= DescriptionTextBox.Content.Length)
        //            break;
        //    }
        //}
        //ShowDescriptionBegining.Text = dat.BreakUpString(ShowDescriptionBegining.Text, 65);
        //ShowRestOfDescription.Text = dat.BreakUpString(DescriptionTextBox.Content.Substring(j), 65);
        //}
        //else
        //{
        //    ShowDescriptionBegining.Text = dat.BreakUpString(DescriptionTextBox.Content, 65);
        //    ShowRestOfDescription.Text = "";
        //}

        if (MainAttractionCheck.Checked)
        {
            ShowVideoPictureLiteral.Text = "";
            if (PictureCheckList.Items.Count > 0)
            {
                Rotator1.Items.Clear();
                char[] delim = { '\\' };



                string[] finalFileArray = new string[PictureCheckList.Items.Count];

                //for (int i = 0; i < PictureCheckList.Items.Count; i++)
                //{
                //    finalFileArray[i] = "http://" + Request.Url.Authority + "/HippoHappenings/UserFiles/" + 
                //        Session["UserName"].ToString() + "/Slider/" + PictureCheckList.Items[i].Value;
                //}
                char[] delim2 = { '.' };
                bool isEdit = false;

                if (Request.QueryString["ID"] != null)
                    isEdit = true;
                for (int i = 0; i < PictureCheckList.Items.Count; i++)
                {
                    Literal literal4 = new Literal();
                    string toUse = "";
                    string[] tokens = PictureCheckList.Items[i].Value.ToString().Split(delim2);
                    //if (PictureCheckList.Items[i].Enabled)
                    //{
                    RotatorPanel.Visible = true;
                    if (tokens.Length >= 2)
                    {
                        if (tokens[1].ToUpper() == "JPG" || tokens[1].ToUpper() == "JPEG" || tokens[1].ToUpper() == "GIF" || tokens[1].ToUpper() == "PNG")
                        {
                            System.Drawing.Image image;
                            if (isEdit)
                            {
                                if (System.IO.File.Exists(MapPath(".") + "\\VenueFiles\\" + Request.QueryString["ID"].ToString() + "\\Slider\\" + PictureCheckList.Items[i].Value.ToString()))
                                {
                                    image = System.Drawing.Image.FromFile(MapPath(".") + "\\VenueFiles\\" + Request.QueryString["ID"].ToString() + "\\Slider\\" + PictureCheckList.Items[i].Value.ToString());
                                    toUse = "/VenueFiles/" + Request.QueryString["ID"].ToString() + "/Slider/" + PictureCheckList.Items[i].Value.ToString();
                                }
                                else
                                {
                                    image = System.Drawing.Image.FromFile(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\Slider\\" + PictureCheckList.Items[i].Value.ToString());
                                    toUse = "/UserFiles/" + Session["UserName"].ToString() + "/Slider/" + PictureCheckList.Items[i].Value.ToString();
                                }
                            }
                            else
                            {
                                image = System.Drawing.Image.FromFile(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\Slider\\" + PictureCheckList.Items[i].Value.ToString());
                                toUse = "/UserFiles/" + Session["UserName"].ToString() + "/Slider/" + PictureCheckList.Items[i].Value.ToString();
                            }



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
                                + toUse +
                                "\" /></div>";
                        }
                        else if (tokens[1].ToUpper() == "WMV")
                        {
                            literal4.Text = "<embed  height=\"250px\" width=\"410px\" src=\""
                                + "UserFiles/" + Session["UserName"].ToString() + "/Slider/" + PictureCheckList.Items[i].Value.ToString() +
                                "\" />";
                        }
                    }
                    else
                    {
                        literal4.Text = "<div style=\"float:left;\"><object width=\"410\" height=\"250\"><param  name=\"wmode\" value=\"opaque\" ></param><param name=\"movie\" value=\"http://www.youtube.com/v/" + PictureCheckList.Items[i].Value.ToString() + "\"></param><param name=\"allowFullScreen\" value=\"true\"></param><embed wmode=\"opaque\" src=\"http://www.youtube.com/v/" + PictureCheckList.Items[i].Value.ToString() + "\" type=\"application/x-shockwave-flash\" allowfullscreen=\"true\" width=\"410\" height=\"250\"></embed></object></div>";
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
                    RotatorPanel.CssClass = "HiddeButtons";
                else
                    RotatorPanel.CssClass = "";

            }

        }
    }

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
            default: break;
        }
    }
    
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
                            "/Slider/" + fileName, "image/" + tokens[1].ToLower());

                        //PictureUpload.SaveAs(MapPath(".").ToString() + "/UserFiles/" + Session["UserName"].ToString() +
                        //    "/Slider/" + fileName);
                        PictureNixItButton.Visible = true;
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
    
    protected void VideoUpload_Click(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (VideoUpload.HasFile)
        {
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
            PictureNixItButton.Visible = false;
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

            YourMessagesLabel.Text = "<br/><br/>Your category '" + CategoriesTextBox.Text +
                "' has been suggested. We'll send you an update when it has been approved.";

            CategoriesTextBox.Text = dat.StripHTML_LeaveLinks(CategoriesTextBox.Text);


            DataSet dsUser = dat.GetData("SELECT EMAIL, UserName FROM USERS WHERE User_ID=" +
                Session["User"].ToString());

            dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["categoryemail"].ToString(),
                        System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                System.Configuration.ConfigurationManager.AppSettings["categoryemail"].ToString(),
                "Category has been suggested from 'EnterVenue.aspx'. The user ID who suggested " +
                "the category is userID: '" + Session["User"].ToString() + "'. The category suggestion is '" + CategoriesTextBox.Text + "'", "Hippo Happenings category suggestion");
            CategoriesTextBox.Text = "";
        }
        else
        {
            MessagePanel.Visible = true;
            YourMessagesLabel.Text = "<br/><br/>Please enter a category to suggest.";
        }
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
}
