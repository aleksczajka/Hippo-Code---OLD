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

public partial class CompleteRegistration : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlLink lk = new HtmlLink();
        HtmlHead head = (HtmlHead)Page.Header;
        lk.Attributes.Add("rel", "canonical");
        lk.Href = "http://hippohappenings.com/complete-registration";
        head.Controls.AddAt(0, lk);

        MakeItSoButton.SERVER_CLICK += MakeItSo;
        MessageLabel.Text = "";
        if (Request.QueryString["ID"] != null)
        {
            Encryption decrypt = new Encryption();
            EmailLabel.Text = decrypt.decrypt(Request.QueryString["ID"].ToString());

            if (!IsPostBack)
            {
                CountryDropDown.DataBind();
                CountryDropDown.Items.FindByValue("223").Selected = true;
                ChangeTheState("223");
                ChangeCity(StateDropDown, new EventArgs());
                Session["UserCountry"] = "223";
                if (Request.QueryString["message"] != null)
                    MessageLabel.Text = decrypt.decrypt(Request.QueryString["message"].ToString());
            }
        }
        else
        {
            Response.Redirect("home");
        }
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

            if (!StateDropDown.Visible && StateTextBox.Text.Trim() != "")
                hasState = true;

            bool hasCity = false;
            if (CityTextBox.Text.Trim() != "")
                hasCity = true;

            bool hasHeadr = false;
            if (HowHeardTextBox.Text.Trim() != "")
                hasHeadr = true;

            bool hasZip = false;
            if (CatZipTextBox.Text.Trim() != "")
                hasZip = true;

            UserNameTextBox.Text = UserNameTextBox.Text.Trim().Replace(" ", "");


            MessageLabel.Text = "";
            if (hasZip && hasCity && hasState && hasHeadr && UserNameTextBox.Text != "" && PasswordTextBox.Text != "" && 
                ConfirmPasswordTextBox.Text != "" && ((FirstNameTextBox.Text.Trim() != ""
                && LastNameTextBox.Text.Trim() != "" && dat.TrapKey(FirstNameTextBox.Text.Trim(), 1)
                && dat.TrapKey(LastNameTextBox.Text.Trim(), 1))|| (FirstNameTextBox.Text.Trim() == "" && 
                LastNameTextBox.Text.Trim() == "")))
            {
                
                bool safeToGo = false;
                if (dat.TrapKey(UserNameTextBox.Text, 1))
                {
                    DataSet ds = dat.GetData("SELECT * FROM Users WHERE UserName='" + 
                        UserNameTextBox.Text + "'");

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
                            message += " This Email is already used by another user. You might have already completed this step. In that case, all you need to do is <a href=\"login\">Login</a>";
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
                                if (safeToGo)
                                {
                                    Encryption encrypt = new Encryption();
                                    string namesStart = "";
                                    string namesEnd = "";
                                    if (FirstNameTextBox.Text.Trim() != "")
                                    {
                                        namesStart = ", FirstName, LastName";
                                        namesEnd = ", '" +
                                        dat.stripHTML(FirstNameTextBox.Text.Trim().Replace("'", "''")) + "', '" +
                                        dat.stripHTML(LastNameTextBox.Text.Trim().Replace("'", "''")) + "'";
                                    }
                                    dat.Execute("INSERT INTO Users (HowHeard, UserName, Email, Password, IPs" + namesStart + ", DateCreated) VALUES( '" +
                                        HowHeardTextBox.Text.Trim().Replace("'", "").Replace("\"", "") + "','" +
                                        UserNameTextBox.Text + "', '" + EmailLabel.Text + "', '" +
                                        encrypt.encrypt(PasswordTextBox.Text) + "', ';" + dat.GetIP() + ";'" + namesEnd + ", GETDATE())");
                                    DataSet dsID = dat.GetData("SELECT User_ID FROM Users WHERE UserName='" + UserNameTextBox.Text + "'");
                                    string ID = dsID.Tables[0].Rows[0]["User_ID"].ToString();
                                    string message2 = "Welcome to Hippo Happenings! <br/> We strive to be your favorite place for Happenings, "
                                    + "Trips & Sights, Locales, Bulletins and connecting with people in your area. <br/>So, let's "+
                                    "do it. Explore our site by posting events in your area, add them to your calendar and "+
                                    "share with your friends at <a href=\"http://hippohappenings.com/blog-event\">Post an Event</a>. " +
                                    "Spread word about your favorite locales by entering them into the Hippo at <a href=\"http://hippohappen"+
                                    "ings.com/enter-locale\">Post a Locale</a>. And, share neighborhood and local trip's you've been "+
                                    "dying to tell people about at <a href=\"http://hippohappenings.com/enter-trip\">Post a Trip</a>. <br/>"+
                                    "Own a business? Need more people at your garage sale? Call out to your community via our bulletins at <a href=\"http://hippohappenings.com/Pos"+
                                    "tAnAd.aspx\">Post a Bulletin</a><br/><br/>";
                                    dat.Execute("INSERT INTO UserPreferences (Mayors, UserID, EventsAttended, CalendarPrivacyMode, CommunicationPrefs, CommentsPreferences, PollPreferences, CatCountry, EmailPrefs, RecommendationPrefs, MajorCity) VALUES ('" +
                                        BossCheckBox.Checked.ToString() + "'," + ID + ", 0, 1, 1, 1, 1, 223, '123456789C', 3, " + MajorCityDropDown.SelectedItem.Value + ")");
                                    dat.Execute("INSERT INTO UserMessages (MessageContent, MessageSubject, "
                                        + "From_UserID, To_UserID, Date, [Read], Mode, Live) VALUES('" + message2.Replace("'", "''") +
                                        "', 'Hippo Happenings Welcome!', 6, " + ID + ", '" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).ToString() + "', 'False', 1, 'True')");

                                    ds = dat.GetData("SELECT U.Password, U.User_ID, UP.CatCountry, UP.CatState, UP.CatCity, U.UserName FROM Users U, UserPreferences UP WHERE U.User_ID=UP.UserID AND U.UserName='"+UserNameTextBox.Text+"'");
                                    Session["UserName"] = UserNameTextBox.Text.Trim();
                                    Session["User"] = ID;

                                    //insert all ad categories
                                    DataView dvCats = dat.GetDataDV("SELECT * FROM AdCategories WHERE isParent = 'True'");

                                    for (int i = 0; i < dvCats.Count; i++)
                                    {
                                        dat.Execute("INSERT INTO UserCategories (UserID, CategoryID) VALUES(" + ID + ", " + dvCats[i]["ID"].ToString() + ")");
                                    }

                                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());

                                    if (conn.State != ConnectionState.Open)
                                        conn.Open();
                                    SqlCommand cmd = new SqlCommand("UPDATE UserPreferences SET  " +
                                        " CatCountry=@catCountry, MajorCity=@major, CatState=@catState, CatCity=@catCity, CatZip=@catZip WHERE UserID=" + ID, conn);
                                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = ID;
                                    cmd.Parameters.Add("@catCity", SqlDbType.NVarChar).Value = CityTextBox.Text;

                                    cmd.Parameters.Add("@catZip", SqlDbType.NVarChar).Value = CatZipTextBox.Text.Trim();

                                    if (CountryDropDown.SelectedValue == "223")
                                    {
                                        cmd.Parameters.Add("@major", SqlDbType.Int).Value = MajorCityDropDown.SelectedValue;
                                    }
                                    else
                                    {
                                        cmd.Parameters.Add("@major", SqlDbType.Int).Value = DBNull.Value;
                                    }

                                    cmd.Parameters.Add("@catCountry", SqlDbType.Int).Value = CountryDropDown.SelectedValue;
                                    Session["UserCountry"] = CountryDropDown.SelectedValue;
                                    string state = "";
                                    if (StateDropDownPanel.Visible)
                                        state = StateDropDown.SelectedItem.Text;
                                    else
                                        state = StateTextBox.Text;

                                    cmd.Parameters.Add("@catState", SqlDbType.NVarChar).Value = state;

                                    Session["UserState"] = state;
                                    Session["UserCity"] = CityTextBox;
                                    Session["UserCity"] = CatZipTextBox.Text.Trim();

                                    cmd.ExecuteNonQuery();
                                    conn.Close();

                                    dat.WhatHappensOnUserLogin(ds);

                                    //Send out welcome email
                                    dat.SendWelcome();

                                    Response.Redirect("my-account");

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
            MajorCityPanel.Visible = true;
            ChangeCity(StateDropDown, new EventArgs());
            StateDropDown.AutoPostBack = true;
            ZipValidator.Visible = true;
        }
        else
        {
            MajorCityPanel.Visible = false;
            StateDropDown.AutoPostBack = false;
            ZipValidator.Visible = false;
        }
    }

    protected void ChangeCity(object sender, EventArgs e)
    {
        try
        {
            if (CountryDropDown.SelectedValue == "223")
            {
                HttpCookie cookie = Request.Cookies["BrowserDate"];
                Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
                DataSet ds = dat.GetData("SELECT *, MC.ID AS MCID FROM MajorCities MC, State S WHERE MC.State=S.state_name " +
                    "AND S.state_2_code='" + StateDropDown.SelectedItem.Text + "'");

                MajorCityPanel.Visible = true;
                MajorCityDropDown.DataSource = ds;
                MajorCityDropDown.DataTextField = "MajorCity";
                MajorCityDropDown.DataValueField = "MCID";
                MajorCityDropDown.DataBind();
            }
            else
            {
                MajorCityPanel.Visible = false;
            }
        }
        catch (Exception ex)
        {
            MessageLabel.Text = ex.ToString();
        }
    }
}
