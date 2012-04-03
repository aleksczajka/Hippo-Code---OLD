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

public partial class CompleteRegister : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlLink lk = new HtmlLink();
        HtmlHead head = (HtmlHead)Page.Header;
        lk.Attributes.Add("rel", "canonical");
        lk.Href = "http://hippohappenings.com/CompleteRegistration.aspx";
        head.Controls.AddAt(0, lk);

        if (Request.QueryString["ID"] != null)
        {
            Encryption decrypt = new Encryption();
            EmailLabel.Text = decrypt.decrypt(Request.QueryString["ID"].ToString());

            if (!IsPostBack)
            {
                CountryDropDown.SelectedValue = "223";
                ChangeTheState("223");
                Session["UserCountry"] = "223";
                if (Request.QueryString["message"] != null)
                    MessageLabel.Text = decrypt.decrypt(Request.QueryString["message"].ToString());
            }
        }
        else
        {
            Response.Redirect("Home.aspx");
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        //Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        //DataSet ds2 = dat.RetrieveAllAds(false);
        //Ads1.DATA_SET = ds2;
        //Ads1.MAIN_AD_DATA_SET = dat.RetrieveAllAds(true);
    }

    protected void ChangeCheckImage(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        ImageButton CheckImageButton = (ImageButton)dat.FindControlRecursive(this, "CheckImageButton");

        if (CheckImageButton.ImageUrl == "image/Check.png")
            CheckImageButton.ImageUrl = "image/CheckSelected.png";
        else
            CheckImageButton.ImageUrl = "image/Check.png";

    }

    protected void MakeItSo(object sender, EventArgs e)
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
            bool hasState = false;
            if (StateDropDown.Visible && StateDropDown.SelectedValue != "-1")
                hasState = true;

            if (!StateDropDown.Visible && StateTextBox.THE_TEXT.Trim() != "")
                hasState = true;

            bool hasCity = false;
            if (CityTextBox.THE_TEXT.Trim() != "")
                hasCity = true;

            bool hasHeadr = false;
            if (HowHeardTextBox.THE_TEXT.Trim() != "")
                hasHeadr = true;

            UserNameTextBox.THE_TEXT = UserNameTextBox.THE_TEXT.Trim().Replace(" ", "");
            //ReferringUserName.Text = ReferringUserName.Text.Trim().Replace(" ", "");

            //if (!dat.TrapKey(ReferringUserName.Text, 1))
            //{
            //    MessageLabel.Text = "The Referring User Name cannot contain any illegal characters.";
            //    return;
            //}
            //else
            //{
            //    DataView dv = dat.GetDataDV("SELECT * FROM Users WHERE UserName='" +
            //             ReferringUserName.Text + "'");
            //    if (dv.Count == 0)
            //    {
            //        MessageLabel.Text = "The Referring User Name you have provided does not exist in our system.";
            //        return;
            //    }
            //}

            MessageLabel.Text = "";
            if (hasCity && hasState && hasHeadr && UserNameTextBox.THE_TEXT != "" && PasswordTextBox.Text != "" && 
                ConfirmPasswordTextBox.Text != "" && ((FirstNameTextBox.THE_TEXT.Trim() != ""
                && LastNameTextBox.THE_TEXT.Trim() != "" && dat.TrapKey(FirstNameTextBox.THE_TEXT.Trim(), 1)
                && dat.TrapKey(LastNameTextBox.THE_TEXT.Trim(), 1))|| (FirstNameTextBox.THE_TEXT.Trim() == "" && 
                LastNameTextBox.THE_TEXT.Trim() == "")))
            {
                
                bool safeToGo = false;
                if (dat.TrapKey(UserNameTextBox.THE_TEXT, 1))
                {
                    DataSet ds = dat.GetData("SELECT * FROM Users WHERE UserName='" + 
                        UserNameTextBox.THE_TEXT + "'");

                    string message = "";
                    if (ds.Tables.Count > 0)
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            message = "*This User Name already exists in the database. Choose a different User Name.";
                            
                        }
                        else
                            safeToGo = true;
                    else
                        safeToGo = true;

                    ds = dat.GetData("SELECT * FROM Users WHERE Email='"+EmailLabel.Text+"'");

                    if (ds.Tables.Count > 0)
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            message += " This Email is already used by another user. You might have already completed this step. In that case, all you need to do is <a href=\"UserLogin.aspx\">Login</a>";
                            safeToGo = false;
                        }
                        

                    if (safeToGo)
                    {
                        if (dat.TrapKey(PasswordTextBox.Text, 1))
                        {
                            safeToGo = true;
                        }
                        else
                        {
                            MessageLabel.Text = "*The Password can contain only letters and numbers.";
                            safeToGo = false;
                        }

                        if (safeToGo)
                        {
                            if (PasswordTextBox.Text == ConfirmPasswordTextBox.Text)
                                safeToGo = true;
                            else
                            {
                                safeToGo = false;
                                MessageLabel.Text = "*The Confirm Password must match the Password.";
                            }

                            if (safeToGo)
                            {
                                //if (CheckImageButton.ImageUrl == "image/CheckSelected.png")
                                //    safeToGo = true;
                                //else
                                //{
                                //    safeToGo = false;
                                //    MessageLabel.Text = "*Read and agree to the Terms And Conditions in order to sign up.";
                                //}


                                if (safeToGo)
                                {
                                    Encryption encrypt = new Encryption();
                                    string namesStart = "";
                                    string namesEnd = "";
                                    if (FirstNameTextBox.THE_TEXT.Trim() != "")
                                    {
                                        namesStart = ", FirstName, LastName";
                                        namesEnd = ", '" +
                                        dat.stripHTML(FirstNameTextBox.THE_TEXT.Trim().Replace("'", "''")) + "', '" +
                                        dat.stripHTML(LastNameTextBox.THE_TEXT.Trim().Replace("'", "''")) + "'";
                                    }
                                    dat.Execute("INSERT INTO Users (HowHeard, UserName, Email, Password, IPs" + namesStart + ", DateCreated) VALUES('" +
                                        HowHeardTextBox.THE_TEXT.Trim().Replace("'", "").Replace("\"", "") + "','" +
                                        UserNameTextBox.THE_TEXT + "', '" + EmailLabel.Text + "', '" +
                                        encrypt.encrypt(PasswordTextBox.Text) + "', ';" + dat.GetIP() + ";'" + namesEnd + ", GETDATE())");
                                    DataSet dsID = dat.GetData("SELECT User_ID FROM Users WHERE UserName='" + UserNameTextBox.THE_TEXT + "'");
                                    string ID = dsID.Tables[0].Rows[0]["User_ID"].ToString();
                                    string message2 = "Welcome to Hippo Happenings! <br/> It is now your place for Events, "
                                    + "Venues, Classifieds and connecting with people in your area. <br/> To start, select your preferences"
                                    + " for filtering your content on the site by going to <a class=\"AddLink\" href=\"UserPreferences.aspx\">My Preferences</a>. ";
                                    dat.Execute("INSERT INTO UserPreferences (UserID, EventsAttended, CalendarPrivacyMode, CommunicationPrefs, CommentsPreferences, PollPreferences, CatCountry, EmailPrefs, RecommendationPrefs) VALUES (" + ID + ", 0, 1, 1, 1, 1, 223, '123456789C', 3)");
                                    dat.Execute("INSERT INTO UserMessages (MessageContent, MessageSubject, "
                                        + "From_UserID, To_UserID, Date, [Read], Mode, Live) VALUES('" + message2 +
                                        "', 'Hippo Happenings Welcome!', 6, " + ID + ", '" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).ToString() + "', 'False', 1, 'True')");

                                    ds = dat.GetData("SELECT U.Password, U.User_ID, UP.CatCountry, UP.CatState, UP.CatCity, U.UserName FROM Users U, UserPreferences UP WHERE U.User_ID=UP.UserID AND U.UserName='"+UserNameTextBox.THE_TEXT+"'");
                                    Session["UserName"] = UserNameTextBox.THE_TEXT.Trim();
                                    Session["User"] = ID;

                                    //insert all ad categories
                                    DataView dvCats = dat.GetDataDV("SELECT * FROM AdCategories WHERE isParent = 'True'");

                                    for (int i = 0; i < dvCats.Count; i++)
                                    {
                                        dat.Execute("INSERT INTO UserCategories (UserID, CategoryID) VALUES(" + ID + ", " + dvCats[i]["ID"].ToString() + ")");
                                    }

                                    string radius = "";
                                    if (RadiusPanel.Visible)
                                    {
                                        radius = ", Radius=" + RadiusDropDown.SelectedValue;
                                    }
                                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());

                                    if (conn.State != ConnectionState.Open)
                                        conn.Open();
                                    SqlCommand cmd = new SqlCommand("UPDATE UserPreferences SET  " +
                                        " CatCountry=@catCountry, CatState=@catState, CatCity=@catCity, CatZip=@catZip" + 
                                        radius +
                                        " WHERE UserID=@id ", conn);
                                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = ID;
                                    cmd.Parameters.Add("@catCity", SqlDbType.NVarChar).Value = CityTextBox.THE_TEXT;

                                    if (CatZipTextBox.THE_TEXT.Trim() != "")
                                        cmd.Parameters.Add("@catZip", SqlDbType.NVarChar).Value = 
                                            dat.GetAllZipsInRadius(RadiusDropDown.SelectedValue,
                                                    CatZipTextBox.THE_TEXT.Trim(), true);
                                    else
                                        cmd.Parameters.Add("@catZip", SqlDbType.NVarChar).Value = DBNull.Value;


                                    cmd.Parameters.Add("@catCountry", SqlDbType.Int).Value = CountryDropDown.SelectedValue;
                                    Session["UserCountry"] = CountryDropDown.SelectedValue;
                                    string state = "";
                                    if (StateDropDownPanel.Visible)
                                        state = StateDropDown.SelectedItem.Text;
                                    else
                                        state = StateTextBox.THE_TEXT;

                                    cmd.Parameters.Add("@catState", SqlDbType.NVarChar).Value = state;

                                    Session["UserState"] = state;
                                    Session["UserCity"] = CityTextBox;
                                    Session["UserCity"] = CatZipTextBox.THE_TEXT.Trim();

                                    cmd.ExecuteNonQuery();
                                    conn.Close();

                                    string groups = "User, Admin";
                                    //FormsAuthentication.RedirectFromLoginPage(txtUserName.Text, false);
                                    FormsAuthenticationTicket authTicket =
                                        new FormsAuthenticationTicket(1, ds.Tables[0].Rows[0]["User_ID"].ToString(),
                                                      DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")),
                                                      DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddMinutes(60),
                                                      false, groups);

                                    // Now encrypt the ticket.
                                    string encryptedTicket =
                                      FormsAuthentication.Encrypt(authTicket);
                                    // Create a cookie and add the encrypted ticket to the
                                    // cookie as data.
                                    HttpCookie authCookie =
                                                 new HttpCookie(FormsAuthentication.FormsCookieName,
                                                                encryptedTicket);

                                    // Add the cookie to the outgoing cookies collection.
                                    Response.Cookies.Add(authCookie);
                                    dat.WhatHappensOnUserLogin(ds);

                                    //Send out welcome email
                                    dat.SendWelcome();

                                    Response.Redirect("User.aspx");

                                }
                            }

                        }


                    }
                    else
                    {
                        MessageLabel.Text = message;
                    }
                }
                else
                    MessageLabel.Text = "User Name can only contain letter and numbers.";
            }
            else
                MessageLabel.Text = "All fields with * must be filled in. If you include First and Last name, these cannot have any special keys, only letters. If you include one of the names, you must include the other.";
        }
        catch (Exception ex)
        {
            MessageLabel.Text = ex.ToString();
        }
    }

    protected void ChangeState(object sender, EventArgs e)
    {
        ChangeTheState(CountryDropDown.SelectedValue);
    }

    protected void ChangeTheState(string country)
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
        DataSet ds = dat.GetData("SELECT * FROM State WHERE country_id=" + country);

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

                StateDropDown.Items.Insert(0, new ListItem("Select State...", "-1"));
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

        if (country == "223")
        {
            RadiusPanel.Visible = true;
        }
        else
        {
            RadiusPanel.Visible = false;
        }
    }
}
