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

public partial class EnterGroup : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
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

        MembererrorLabel.Text = "";
        MemberDescriptionTextBox.Attributes.Add("onkeyup", "CountCharsDMem(event)");
        MemberTitleTextBox.Attributes.Add("onkeyup", "CountCharsTMem(event)");
        CaptionTextBox.Attributes.Add("onkeyup", "CountCharsCaption(event)");
        if (!IsPostBack)
        {
            DescriptionTextBox.Attributes.Add("onkeyup", "CountCharsEditor(editor)");

            if (Request.QueryString["ID"] != null)
                MembersListBox.Items.Add(new ListItem(Session["UserName"].ToString(), ""));
            Session.Remove("CategoriesSet");
            Session["CategoriesSet"] = null;

            CountryDropDown.SelectedValue = "223";
            DataSet dsCountries = dat.GetData("SELECT * FROM State WHERE country_id=223");

            StateDropDown.DataSource = dsCountries;
            StateDropDown.DataTextField = "state_2_code";
            StateDropDown.DataValueField = "state_id";
            StateDropDown.DataBind();
            StateDropDown.Items.Insert(0, new ListItem("Select State..", "-1"));
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
                        DataView dvg = dat.GetDataDV("SELECT * FROM Group_Members WHERE MemberID=" +
                    Session["User"].ToString() + " AND SharedHosting='True' AND GroupID=" +
                    Request.QueryString["id"].ToString());

                        if (dvg.Count == 0)
                            Response.Redirect("Home.aspx");
                        fillVenue();

                    }
                }


            }
            else
            {
                WelcomeLabel.Text = "On Hippo Happenings, the users have all the power to post event, ads, groups and venues alike. In order to do so, and for us to maintain clean and manageable content,  "
                    + " we require that you <a class=\"AddLink\" href=\"Register.aspx\">create an account</a> with us. <a class=\"AddLink\" href=\"UserLogin.aspx\">Log in</a> if you have an account already. " +
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
        DataSet dsCategories = dat.GetData("SELECT * FROM Group_Category WHERE Group_ID=" +
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

        //If there is no owner, the venue is up for grabs.
        DataSet dsVenue = dat.GetData("SELECT * FROM Groups WHERE ID=" + Request.QueryString["ID"].ToString());

        

        if (dsVenue.Tables.Count > 0)
            if (dsVenue.Tables[0].Rows.Count > 0)
            {
                TitleLabel.Text = "You are editing for group '" + dsVenue.Tables[0].Rows[0]["Header"].ToString() +
                    "'";
                PrivateCheckBox.Checked = bool.Parse(dsVenue.Tables[0].Rows[0]["isPrivate"].ToString());

                if (dsVenue.Tables[0].Rows[0]["MainPicture"] != null)
                {
                    if (dsVenue.Tables[0].Rows[0]["MainPicture"].ToString() != "")
                    {
                        MainImageCheck.Visible = true;
                        MainImageCheck.Items.Add(new ListItem(dsVenue.Tables[0].Rows[0]["MainPictureName"].ToString(),
                            dsVenue.Tables[0].Rows[0]["MainPicture"].ToString()));
                        ImageButton1.Visible = true;
                    }
                }
                HostInstructions.Content = dsVenue.Tables[0].Rows[0]["HostInstructions"].ToString();
                VenueNameTextBox.THE_TEXT = dsVenue.Tables[0].Rows[0]["Header"].ToString();
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
                        StateDropDown.Items.Insert(0, new ListItem("Select State..", "-1"));
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
                char[] delim = { '\\' };
                char[] delim4 = { ';' };
                char[] delim5 = { '.' };
                
                DataView dvSlider = dat.GetDataDV("SELECT * FROM Group_Slider_Mapping WHERE GroupID=" + Request.QueryString["ID"].ToString());
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
                        dat.Execute("INSERT INTO Group_Category (Group_ID, CATEGORY_ID) VALUES("
                            + ID + ", " + TreeNode.Value + ")");
                    }
            }
            else
            {
                dat.Execute("INSERT INTO Group_Category (Group_ID, CATEGORY_ID) VALUES("
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
                    dat.Execute("DELETE FROM Group_Category WHERE Group_ID=" + ID +
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


            bool isUpdate = false;

            DataSet dsVenue = new DataSet();

            if (Request.QueryString["ID"] != null)
            {
                dsVenue = dat.GetData("SELECT * FROM Groups WHERE ID=" + Request.QueryString["ID"].ToString());
                isUpdate = true;
            }

            string state = "";
            if (StateDropDownPanel.Visible)
                state = StateDropDown.SelectedItem.Text;
            else
                state = StateTextBox.THE_TEXT;

            bool cont = true;
            if (cont)
            {
                string command = "";

                if (isUpdate)
                {
                        command = "UPDATE Groups SET HostInstructions=@instr, MainPicture=@mainPic, MainPictureName=@mainPicName, Header=@name, isPrivate='"+PrivateCheckBox.Checked.ToString()+"', City=@city, Email=@email, Phone=@phone, State=@state, Country=@country, Zip=@zip, Address=@address, "
                        + " Content=@content, Web=@web, LastEditOn=@dateE WHERE ID=" + Request.QueryString["ID"].ToString();
                }
                else
                    command = "INSERT INTO Groups (HostInstructions, MainPicture, MainPictureName, isPrivate, Web, Host, City, State, Country, Zip, Header, Email, Phone, Address, Content, CreatedOn) "
                     + "VALUES(@instr, @mainPic, @mainPicName, '" + PrivateCheckBox.Checked.ToString() + "', @web, @host, @city, @state, @country, @zip, @name, @email, @phone, @address, @content, @dateE)";

                string locationStr = "";
                string apt = "";
                if (AptNumberTextBox.Text.Trim() != "")
                    apt = AptDropDown.SelectedItem.Text + " " + AptNumberTextBox.Text.Trim().ToLower();
                if (CountryDropDown.SelectedValue == "223")
                {
                    if (StreetNameTextBox.Text.Trim() != "" && StreetNameTextBox.Text.Trim() != ""
                                    && StreetDropDown.SelectedItem.Text.Trim() != "Select One...")
                    {

                        locationStr = StreetNumberTextBox.Text.Trim().ToLower() + ";" + StreetNameTextBox.Text.Trim().ToLower()
                            + ";" + StreetDropDown.SelectedItem.Text + ";" + apt;
                    }
                }
                else
                {
                    if (LocationTextBox.Text.Trim() != "")
                    {
                        locationStr = LocationTextBox.Text.Trim().ToLower() + ";" + apt;
                    }
                }


                SqlCommand cmd = new SqlCommand(command, conn);
                cmd.CommandType = CommandType.Text;

                if (HostInstructions.Text.Trim() != "")
                {
                    cmd.Parameters.Add("@instr", SqlDbType.NVarChar).Value = HostInstructions.Content;
                }
                else
                {
                    cmd.Parameters.Add("@instr", SqlDbType.NVarChar).Value = DBNull.Value;
                }

                if (MainImageCheck.Items.Count != 0)
                {
                    cmd.Parameters.Add("@mainPic", SqlDbType.NVarChar).Value = MainImageCheck.Items[0].Value;
                    cmd.Parameters.Add("@mainPicName", SqlDbType.NVarChar).Value = MainImageCheck.Items[0].Text;
                }
                else
                {
                    cmd.Parameters.Add("@mainPic", SqlDbType.NVarChar).Value = DBNull.Value;
                    cmd.Parameters.Add("@mainPicName", SqlDbType.NVarChar).Value = DBNull.Value;
                }

                cmd.Parameters.Add("@dateE", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
                
                cmd.Parameters.Add("@host", SqlDbType.Int).Value = Session["User"].ToString();


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
                    cmd.Parameters.Add("@content", SqlDbType.NVarChar).Value = DescriptionTextBox.Content.Trim();
                    cmd.Parameters.Add("@country", SqlDbType.Int).Value = int.Parse(CountryDropDown.SelectedValue);
                    cmd.Parameters.Add("@zip", SqlDbType.NVarChar).Value = ZipTextBox.Text.Trim();
                    cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = CityTextBox.Text.Trim();


                    cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = state;

                cmd.ExecuteNonQuery();


                string ID = "";
                if (isUpdate)
                {
                        ID = Request.QueryString["ID"].ToString();

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
                    string[] fileArray = System.IO.Directory.GetFiles(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\Slider\\");

                    if (!System.IO.Directory.Exists(MapPath(".") + "\\GroupFiles"))
                    {
                        System.IO.Directory.CreateDirectory(MapPath(".") + "\\GroupFiles\\");
                        System.IO.Directory.CreateDirectory(MapPath(".") + "\\GroupFiles\\" + ID + "\\");
                        System.IO.Directory.CreateDirectory(MapPath(".") + "\\GroupFiles\\" + ID + "\\Slider\\");
                    }
                    else
                    {
                        if (!System.IO.Directory.Exists(MapPath(".") + "\\GroupFiles\\" + ID))
                        {
                            System.IO.Directory.CreateDirectory(MapPath(".") + "\\GroupFiles\\" + ID + "\\");
                            System.IO.Directory.CreateDirectory(MapPath(".") + "\\GroupFiles\\" + ID + "\\Slider\\");
                        }
                        else
                        {
                            if (!!System.IO.Directory.Exists(MapPath(".") + "\\GroupFiles\\" + ID + "\\Slider\\"))
                                System.IO.Directory.CreateDirectory(MapPath(".") + "\\GroupFiles\\" + ID + "\\Slider\\");
                        }
                    }

                    string YouTubeStr = "";
                    char[] delim3 = { '.' };
                    char[] delim4 = { '-' };
                    string realName = "";
                    string origName = "";
                    string caption = "";
                    dat.Execute("DELETE FROM Group_Slider_Mapping WHERE GroupID=" + ID.ToString());
                    for (int i = 0; i < PictureCheckList.Items.Count; i++)
                    {
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
                            if (tokens[1].ToUpper() == "JPG" || tokens[1].ToUpper() == "JPEG" || tokens[1].ToUpper() == "GIF")
                            {
                                if (!System.IO.File.Exists(MapPath(".") + "\\GroupFiles\\" + ID.ToString() + "\\Slider\\" + realName))
                                {
                                    System.IO.File.Copy(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\Slider\\" +
                                                        realName, MapPath(".") + "\\GroupFiles\\" + ID.ToString() + "\\Slider\\" + realName);
                                }

                                cmd = new SqlCommand("INSERT INTO Group_Slider_Mapping (GroupID, PictureName, RealPictureName, Caption) " +
                                    "VALUES (@eventID, @picName, @realName, @cap)", conn);
                                cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = ID;
                                cmd.Parameters.Add("@picName", SqlDbType.NVarChar).Value = realName;
                                cmd.Parameters.Add("@realName", SqlDbType.NVarChar).Value = origName;
                                cmd.Parameters.Add("@cap", SqlDbType.NVarChar).Value = caption;
                                cmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            cmd = new SqlCommand("INSERT INTO Group_Slider_Mapping (isYouTube,GroupID, PictureName, RealPictureName, Caption) " +
                                    "VALUES ('True',@eventID, @picName, @realName, @cap)", conn);
                            cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = ID;
                            cmd.Parameters.Add("@picName", SqlDbType.NVarChar).Value = realName.Replace("YouTube ID: ", "");
                            cmd.Parameters.Add("@realName", SqlDbType.NVarChar).Value = realName.Replace("YouTube ID: ", "");
                            cmd.Parameters.Add("@cap", SqlDbType.NVarChar).Value = caption;
                            cmd.ExecuteNonQuery();
                        }
                    }

                }

                CreateMembers(ID, isUpdate);
                CreateCategories(ID, isUpdate);

                //Send the informational email to the user
                DataSet dsUser = dat.GetData("SELECT Email, UserName FROM USERS WHERE User_ID=" +
                    Session["User"].ToString());

                string emailBody = "<br/><br/>Dear " + dsUser.Tables[0].Rows[0]["UserName"].ToString() + ", <br/><br/> you have successfully posted the group \"" + VenueNameTextBox.THE_TEXT +
                   "\". <br/><br/> You can find this group <a href=\"http://hippohappenings.com/" + dat.MakeNiceName(VenueNameTextBox.THE_TEXT) + "_" + ID + "_Group\">here</a>. " +
                   "<br/><br/> To rate your experience posting this group <a href=\"http://hippohappenings.com/RateExperience.aspx?Edit=" + isUpdate.ToString() + "&Type=G&ID=" + ID + "\">please include your feedback here.</a>" +
                   "<br/><br/><br/>Have a Hippo Happening Day!<br/><br/>";

                if (isUpdate)
                {
                   
                        if (!Request.Url.AbsoluteUri.Contains("localhost"))
                        {
                            dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                                dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "You have updated group: " +
                                VenueNameTextBox.THE_TEXT);
                        }
                }
                else
                {
                    if (!Request.Url.AbsoluteUri.Contains("localhost"))
                    {
                        dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                            System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                            dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "You have posted the group: " +
                            VenueNameTextBox.THE_TEXT);
                    }
                }


                conn.Close();

                

                //pop up the message to the user
                Encryption encrypt = new Encryption();

                if (!isUpdate)
                {
                    Session["Message"] = "Your group has been posted successfully! An email with this info will also be sent to your account.<br/>"
                        + "<br/>Check out <a class=\"AddLink\" onclick=\"Search('" + dat.MakeNiceName(VenueNameTextBox.THE_TEXT) + "_" + ID + "_Group');\">this group's</a> home page.<br/><br/><br/> -<a class=\"AddLink\" onclick=\"Search('RateExperience.aspx?Edit=" + isUpdate.ToString() + "&Type=G&ID=" + ID + "');\" >Rate </a>your user experience posting this group.<br/>";
                }
                else
                {
                    Session["Message"] = "You have successfully updated group: \"" + VenueNameTextBox.THE_TEXT.Trim() +
                    "\".<br/><br/>Check out <a class=\"AddLink\" onclick=\"Search('" + dat.MakeNiceName(VenueNameTextBox.THE_TEXT) + "_" + ID +
                    "_Group');\">this group's</a> home page.<br/><br/> <a class=\"AddLink\" onclick=\"Search('RateExperience.aspx?Edit=" +
                    isUpdate.ToString() + "&Type=G&ID=" + ID + "');\" >Rate </a>your user experience editing this group.<br/>";
                }

                MessageRadWindow.NavigateUrl = "Message.aspx?message=" + encrypt.encrypt(Session["Message"].ToString() + "<br/><img onclick=\"Search('Home.aspx');\" onmouseover=\"this.src='image/DoneSonButtonSelected.png'\" onmouseout=\"this.src='image/DoneSonButton.png'\" src=\"image/DoneSonButton.png\"/>");
                MessageRadWindow.Visible = true;
                MessageRadWindow.VisibleOnPageLoad = true;
            }
        }
        else
        {
            MessagePanel.Visible = true;
            YourMessagesLabel.Text += "<br/><br/>You must agree to the terms and conditions.";
        }
    }

    protected void CreateMembers(string theID, bool isUpdate)
    {
        string command = "";
        try
        {
            char[] delim = { '-' };
            HttpCookie cookie = Request.Cookies["BrowserDate"];
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            string emailBody = Session["UserName"].ToString() + " has created a new group <a href=\"http://hippohappenings.com/" +
                dat.MakeNiceName(VenueNameTextBox.THE_TEXT.Trim()) +
                "_" + theID + "_Group\">" + VenueNameTextBox.THE_TEXT.Trim() +
                   "</a> and is inviting you to it." +
                   " To accept the membership <a href=\"http://hippohappenings.com/User.aspx\">log in to HippoHappenings</a>, view your messages in 'My Account' and " +
                   "click 'Accept' in the message corresponding to this email. Good luck in your membership! <br/><br/>HippoHappenings";
            string emailSubject = "You have a group invite for '" + VenueNameTextBox.THE_TEXT + "'";
            string messageBody = theID + " " + Session["UserName"].ToString() + " has created a new group <a href=\"" +
                dat.MakeNiceName(VenueNameTextBox.THE_TEXT.Trim()) +
                "_" + theID + "_Group\">" + VenueNameTextBox.THE_TEXT.Trim() +
                   "</a> and is inviting you to it. Click " +
                "the 'Accept' button to the right if you want to become part of this group. ";

            bool hasHost = false;
            foreach (ListItem item in MembersListBox.Items)
            {
                string[] tokens = item.Text.Split(delim);
                DataView dvUser = dat.GetDataDV("SELECT * FROM Users WHERE UserName='" + tokens[0].Trim() + "'");
                string ID = dvUser[0]["User_ID"].ToString();
                string title = "";
                string titleUpdate = "";
                string titleBeg = "";
                string descrip = "";
                string descripUpdate = "";
                string descripBeg = "";
                string shared = "";
                string sharedUpdate = "";
                string sharedBeg = "";
                string updateString = "";
                if (tokens.Length > 1)
                {
                    if (tokens[1].Trim() != "description added" && tokens[1].Trim() != "shared hosting")
                    {
                        title = ", '" + tokens[1].Trim().Replace("'", "''") + "'";
                        titleUpdate = " Title = '" + tokens[1].Trim().Replace("'", "''") + "'";
                        titleBeg = ",Title";
                    }
                    else if (tokens[1].Trim() == "shared hosting")
                    {
                        sharedUpdate = " SharedHosting = 'True' ";
                        shared = ", 'True'";
                        sharedBeg = ", SharedHosting";
                    }
                }

                if (ID == Session["User"].ToString())
                {
                    hasHost = true;
                    sharedUpdate = " SharedHosting = 'True' ";
                    shared = ", 'True'";
                    sharedBeg = ", SharedHosting";
                }

                if (item.Value.Trim() != "")
                {
                    descripUpdate = " Description = '" + item.Value.Trim().Replace("'", "''") + "'";
                    descrip = ", '" + item.Value.Trim().Replace("'", "''") + "'";
                    descripBeg = ", Description";
                }
                if (tokens.Length > 2)
                {
                    if (tokens[2].Trim() == "shared hosting")
                    {
                        sharedUpdate = " SharedHosting = 'True' ";
                        shared = ", 'True'";
                        sharedBeg = ", SharedHosting";
                    }
                    else
                    {
                        if (tokens.Length > 3)
                        {
                            if (tokens[3].Trim() == "shared hosting")
                            {
                                sharedUpdate = " SharedHosting = 'True' ";
                                shared = ", 'True'";
                                sharedBeg = ", SharedHosting";
                            }
                        }
                    }
                }

                //take care of update string
                if (titleUpdate.Trim() != "")
                {
                    updateString = titleUpdate;
                }

                if (descripUpdate.Trim() != "")
                {
                    if (updateString.Trim() != "")
                    {
                        updateString += ", " + descripUpdate;
                    }
                    else
                    {
                        updateString = descripUpdate;
                    }
                }

                if (sharedUpdate.Trim() != "")
                {
                    if (updateString.Trim() != "")
                    {
                        updateString += ", " + sharedUpdate;
                    }
                    else
                    {
                        updateString = sharedUpdate;
                    }
                }

                //execute update/insert member
                DataView dvMember = new DataView();
                if (Request.QueryString["ID"] != null)
                {
                    dvMember = dat.GetDataDV("SELECT * FROM Group_Members WHERE MemberID=" +
                     ID + " AND GroupID=" + Request.QueryString["ID"].ToString());
                }

                if (dvMember.Count == 0)
                {
                    command = "INSERT INTO Group_Members (GroupID, MemberID " + titleBeg + descripBeg + sharedBeg + ") VALUES(" + theID + ", " + ID + title + descrip + shared + ")";
                }
                else
                {
                    if (updateString != "")
                        command = "UPDATE Group_Members SET " + updateString + " WHERE MemberID=" + ID + " AND GroupID=" + Request.QueryString["ID"].ToString();
                }
                dat.Execute(command);

                if (ID != Session["User"].ToString())
                {
                    dat.Execute("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, To_UserID, " +
                    "[Date], [Read], [Mode], [Live]) VALUES('" + messageBody.Replace("'", "''") + "', '" + emailSubject.Replace("'", "''") + "', " +
                    dat.HIPPOHAPP_USERID.ToString() + ", " + ID + ", '" +
                    DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).ToString() + "', 'False', 7, 'True')");
                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                            System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                                            dvUser[0]["Email"].ToString(), emailBody, emailSubject);
                }
            }

            if (!isUpdate && !hasHost)
            {
                dat.Execute("INSERT INTO Group_Members (GroupID, MemberID, SharedHosting, Accepted) VALUES(" +
                    theID + ", " + Session["User"].ToString() + ", 'True', 'True')");
            }
        }
        catch (Exception ex)
        {
            MessagePanel.Visible = true;
            YourMessagesLabel.Text = ex.ToString()+"<br/>command: "+command;
        }
    }

    protected void AddCaption(object sender, EventArgs e)
    {
        char[] delim = { '-' };
        if (CaptionTextBox.Text.Trim().Length <= 200)
        {
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
        else
        {
            YourMessagesLabel.Text = "Caption must be less than 200 characters.";
            MessagePanel.Visible = true;
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

    protected void CreateCategories(string ID, bool isUpdate)
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
                dat.Execute("DELETE FROM Group_Category WHERE Group_ID=" + ID);
            }

            DataSet dsCategories = dat.GetData("SELECT * FROM Group_Category WHERE Group_ID=" + ID);
            DataView dvCat = new DataView(dsCategories.Tables[0], "", "", DataViewRowState.CurrentRows);

            GetCategoriesFromTree(isUpdate, ref CategoryTree, dvCat, ID);
            GetCategoriesFromTree(isUpdate, ref RadTreeView2, dvCat, ID);
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

            switch (selectedIndex)
            {
                case 0:
                    #region Case 0

                    bool goOn = false;

                    bool isInternational = false;
                    string locationStr = "";

                    VenueNameTextBox.THE_TEXT = dat.stripHTML(VenueNameTextBox.THE_TEXT);
                    DescriptionTextBox.Content = dat.StripHTML_LeaveLinks(DescriptionTextBox.Content.Replace("<div>", "<br/>").Replace("</div>", ""));

                    if (VenueNameTextBox.THE_TEXT.Trim() != "" && DescriptionTextBox.Text.Trim() != "")
                    {
                        bool isUpdate = false;
                        if (Request.QueryString["ID"] != null)
                        {
                            isUpdate = true;
                        }

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
                                YourMessagesLabel.Text += "<br/><br/>*Group Name be under 70 characters.";
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
                    return false;
                    #endregion
                    break;
                case 1:

                    bool goon = true;
                    HostInstructions.Content = dat.StripHTML_LeaveLinks(HostInstructions.Content.Replace("<div>", "<br/>").Replace("</div>", ""));
                    if (HostInstructions.Text.Length > 300)
                    {
                        message = "Host's instructions must be less than 300 characters.";
                        goon = false;
                    }

                    if (goon)
                    {
                        CityTextBox.Text = dat.stripHTML(CityTextBox.Text);
                        ZipTextBox.Text = dat.stripHTML(ZipTextBox.Text);
                        LocationTextBox.Text = dat.stripHTML(LocationTextBox.Text.Trim());
                        PhoneTextBox.Text = dat.stripHTML(PhoneTextBox.Text.Trim());
                        EmailTextBox.Text = dat.stripHTML(EmailTextBox.Text.Trim());
                        WebSiteTextBox.Text = dat.stripHTML(WebSiteTextBox.Text.Trim());
                        string state = "";
                        if (StateTextBox.Visible)
                            state = dat.stripHTML(StateTextBox.THE_TEXT.Trim());
                        else
                        {
                            if (StateDropDown.SelectedValue != "-1")
                                state = StateDropDown.SelectedItem.Text;
                        }

                        if (state == "" || CityTextBox.Text.Trim() == "")
                        {
                            goon = false;
                            message = "Group Head-Quarter location must be included.";
                        }

                        if (goon)
                        {
                            if (StreetNumberTextBox.Text.Trim() != "" || StreetNameTextBox.Text.Trim() != "")
                            {
                                if (StreetNameTextBox.Text.Trim() == "" || StreetNameTextBox.Text.Trim() == ""
                                    || StreetDropDown.SelectedItem.Text.Trim() == "Select One...")
                                {
                                    goon = false;
                                    message = "Specific address is not required, but, if you are including one, all fields must be entered.";
                                }

                            }

                            if (goon)
                            {
                                if (changeTab)
                                    ChangeSelectedTab(1, 2);

                                return true;
                            }
                        }
                    }

                    MessagePanel.Visible = true;
                    YourMessagesLabel.Text += "<br/><br/>*" + message;

                    return false;
                    break;
                case 2:
                    if (changeTab)
                    {
                        ChangeSelectedTab(2, 3);

                    }
                    return true;
                    break;
                case 3:
                    if (Session["CategoriesSet"] == null && Request.QueryString["ID"] != null)
                        SetCategories();
                    if (changeTab)
                        ChangeSelectedTab(3, 4);
                    return true;
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

    protected void MainImageNixIt(object sender, EventArgs e)
    {
        MainImageCheck.Items.Clear();
        ImageButton1.Visible = false;
    }

    protected void MainPicUpload(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (MainImageUpload.HasFile)
        {
            char[] delim = { '.' };
            string[] tokens = MainImageUpload.FileName.Split(delim);

            if (tokens.Length > 1)
            {
                MainImageCheck.Items.Clear();
                if (tokens[1].ToUpper() == "JPEG" || tokens[1].ToUpper() == "JPG" ||
                    tokens[1].ToUpper() == "GIF" || tokens[1].ToUpper() == "PNG")
                {
                    string fileName = "rename" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Ticks.ToString() + "." + tokens[1];
                    MainImageCheck.Items.Add(new ListItem(MainImageUpload.FileName, fileName));
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

                    System.Drawing.Image img = System.Drawing.Image.FromStream(MainImageUpload.PostedFile.InputStream);

                    SaveThumbnail(img, true, MapPath(".").ToString() + "/UserFiles/" + Session["UserName"].ToString() +
                        "/Slider/" + fileName, "image/" + tokens[1].ToLower(), 200, 200);

                    //PictureUpload.SaveAs(MapPath(".").ToString() + "/UserFiles/" + Session["UserName"].ToString() +
                    //    "/Slider/" + fileName);
                    ImageButton1.Visible = true;
                }
                else
                {
                    YourMessagesLabel.Text = "No go! Pictures can only be .gif, .jpg, or .jpeg.";
                    MessagePanel.Visible = true;
                }
            }
            else
            {
                YourMessagesLabel.Text = "No go! Pictures can only be .gif, .jpg, or .jpeg.";
                MessagePanel.Visible = true;
            }

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
                    if (tokens[1].ToUpper() == "JPEG" || tokens[1].ToUpper() == "JPG" ||
                        tokens[1].ToUpper() == "GIF" || tokens[1].ToUpper() == "PNG")
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
                        YourMessagesLabel.Text = "No go! Pictures can only ge .gif, .jpg, or .jpeg.";
                        MessagePanel.Visible = true;
                    }
                }
                else
                {
                    YourMessagesLabel.Text = "No go! Pictures can only ge .gif, .jpg, or .jpeg.";
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
            "Category has been suggested from 'EnterVenue.aspx'. The user ID who suggested " +
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
        try
        {
            char[] delim = { ';' };
            char[] delim2 = { '-' };
            if (Session["SelectedMembers"] != null)
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

                Hashtable hash = new Hashtable();

                foreach (ListItem item in MembersListBox.Items)
                {
                    string[] toks = item.Text.Split(delim2);
                    hash.Add(toks[0].Trim(), "");
                }

                string[] tokens = Session["SelectedMembers"].ToString().Split(delim);
                DataView dvv;
                foreach (string token in tokens)
                {
                    
                    if (token.Trim() != "")
                    {
                        dvv = dat.GetDataDV("SELECT * FROM Users WHERE User_ID=" + token);
                        if (!hash.Contains(dvv[0]["UserName"].ToString()))
                        {
                            MembersListBox.Items.Add(new ListItem(dvv[0]["UserName"].ToString(), ""));
                            hash.Add(dvv[0]["UserName"].ToString(), "");
                        }
                    }
                }
                Session["SelectedMembers"] = null;
                Session.Remove("SelectedMembers");
            }
        }
        catch (Exception ex)
        {
            MessagePanel.Visible = true;
            YourMessagesLabel.Text = ex.ToString();
        }
    }

    protected void RemoveMember(object sender, EventArgs e)
    {
        if (MembersListBox.SelectedIndex != -1)
        {
            ListItem item = MembersListBox.SelectedItem;
            char[] delim = { '-' };
            string[] tokens = item.Value.Split(delim);

            if(tokens[0].Trim() != Session["UserName"].ToString())
                MembersListBox.Items.Remove(item);
        }
    }

    protected void AssignTitle(object sender, EventArgs e)
    {
        try
        {
            if (MembersListBox.SelectedIndex != -1)
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

                char[] delim ={ '-' };
                string [] tokens = MembersListBox.SelectedItem.Text.Split(delim);

                string userName = tokens[0];
                string sharedStr = "";
                if (SharedHostingCheckBox.Checked)
                {
                    sharedStr = " - shared hosting";
                }
                string description = "";
                bool goOn = true;
                if (MemberDescriptionTextBox.Text.Trim() != "")
                {
                    if (MemberDescriptionTextBox.Text.Trim().Length <= 150)
                    {
                        MembersListBox.SelectedItem.Value = MemberDescriptionTextBox.Text;
                        description = " - description added";
                    }
                    else
                        goOn = false;
                }
                if (goOn)
                {
                    string title = "";
                    if (MemberTitleTextBox.Text.Trim() != "")
                    {
                        if (MemberTitleTextBox.Text.Trim().Length <= 15)
                        {
                            title = " - " + MemberTitleTextBox.Text.Trim();
                        }
                        else
                        {
                            goOn = false;
                        }
                    }
                    if (goOn)
                        MembersListBox.SelectedItem.Text = userName + title + description + sharedStr;
                    else
                    {
                        MembererrorLabel.Text = "Member's title must be less than 15 characters.";
                    }
                }
                else
                {
                    MembererrorLabel.Text = "Member's description must be less than 150 characters.";
                }
            }
            else
            {
                MembererrorLabel.Text = "Choose somebody.";
            }
        }
        catch (Exception ex)
        {
            MembererrorLabel.Text = ex.ToString();
        }
    }

    protected void SelectTitle(object sender, EventArgs e)
    {
        if (MembersListBox.SelectedIndex != -1)
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

            char[] delim = { '-' };
            string[] tokens = MembersListBox.SelectedItem.Text.Split(delim);

            if (tokens.Length > 1)
            {
                if (tokens[1].Trim() != "description added" && tokens[1].Trim() != "shared hosting")
                {
                    MemberTitleTextBox.Text = tokens[1].Trim();
                }
                else if (tokens[1].Trim() == "shared hosting")
                {
                    SharedHostingCheckBox.Checked = true;
                }

                MemberDescriptionTextBox.Text = MembersListBox.SelectedItem.Value;

                if (tokens.Length > 2)
                {
                    if (tokens[2].Trim() == "shared hosting")
                        SharedHostingCheckBox.Checked = true;
                    else
                    {
                        if (tokens.Length > 3)
                        {
                            if (tokens[3].Trim() == "shared hosting")
                                SharedHostingCheckBox.Checked = true;
                        }
                    }
                }
            }
        }
    }

    protected void ChangeColorPanel(object sender, EventArgs e)
    {
        if (ColorSchemeRadioList.SelectedValue == "1")
        {
            ColorPanel.Visible = true;
        }
        else
        {
            ColorPanel.Visible = false;
        }
    }
}
