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

public partial class Register : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        MakeItSoButton.SERVER_CLICK += MakeItSo;
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
        Data dat = new Data(isn); ImageButton CheckImageButton = (ImageButton)dat.FindControlRecursive(this, "CheckImageButton");

        if (CheckImageButton.ImageUrl == "image/Check.png")
            CheckImageButton.ImageUrl = "image/CheckSelected.png";
        else
            CheckImageButton.ImageUrl = "image/Check.png";

    }

    protected void MakeItSo(object sender, EventArgs e)
    {
        MessageLabel.Text = "";
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn);
        EmailTextBox.Text = dat.stripHTML(EmailTextBox.Text.Trim());
        ConfirmEmailTextBox.Text = dat.stripHTML(ConfirmEmailTextBox.Text.Trim()); 

        if (EmailTextBox.Text != "" && ConfirmEmailTextBox.Text != "")
        {
            bool safeToGo = true;
           
                if (dat.ValidateEmail(EmailTextBox.Text))
                {
                    if (safeToGo)
                    {
                        if (dat.TrapKey(EmailTextBox.Text, 2))
                        {
                            if (EmailTextBox.Text == ConfirmEmailTextBox.Text)
                                safeToGo = true;
                            else
                            {
                                MessageLabel.Text = "*Comfirmation Email must match the Email text box.";
                                safeToGo = false;
                            }
                        }
                        else
                        {
                            MessageLabel.Text = "*The Email must only contain allowed characters.";
                            safeToGo = false;
                        }

                        if (safeToGo)
                        {
                            DataSet dsEmail = dat.GetData("SELECT * FROM Users WHERE Email='"+EmailTextBox.Text+"'");

                            if (dsEmail.Tables.Count > 0)
                                if (dsEmail.Tables[0].Rows.Count > 0)
                                    safeToGo = false;

                            if (safeToGo)
                            {

                                if (TermsCheckBox.Checked)
                                {
                                    Encryption encrypt = new Encryption();

                                    dat.SendEmailNoFrills(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                        System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(), EmailTextBox.Text,
                                        "Hello From Hippo Happenings! <br/><br/> You have requested to create a Hippo Happenings account. We'd like to say, " +
                                        " GREAT CHOICE! <br/><br/> Please visit <a href=\"http://HippoHappenings.com/complete-registration?ID=" +
                                        encrypt.encrypt(EmailTextBox.Text) + "\">this link</a> to complete the creation of your account. ", "Hippo Happenings User Account Request");

                                    MessageRadWindow.NavigateUrl = "Message.aspx?message=" +
                                        encrypt.encrypt("An email has been sent to your address. "+
                                        "Please make sure to check your <b>JUNK MAIL</b> and your " +
                                        " inbox for further instruction to complete " +
                                        "your registration." +
                                        "<br/><br/><div align=\"center\">" +
                                        "<div style=\"width: 50px;\" onclick=\"Search('home')\">" +
                                        "<div class=\"topDiv\" style=\"clear: both;\">" +
                                          "  <img style=\"float: left;\" src=\"NewImages/ButtonLeft.png\" height=\"27px\" /> " +
                                           " <div class=\"NavyLink\" style=\"font-size: 12px; text-decoration: none; padding-top: 5px;padding-left: 6px; padding-right: 6px;height: 27px;float: left;background: url('NewImages/ButtonPixel.png'); background-repeat: repeat-x;\">" +
                                               " Ok " +
                                            "</div>" +
                                           " <img style=\"float: left;\" src=\"NewImages/ButtonRight.png\" height=\"27px\" /> " +
                                        "</div>" +
                                        "</div>" +
                                        "</div><br/>");
                                    MessageRadWindow.Visible = true;
                                    MessageRadWindowManager.VisibleOnPageLoad = true;
                                    MessageLabel.Text = "";
                                }
                                else
                                {
                                    MessageLabel.Text = "*You must agree to the terms and contidions.";
                                }
                            }
                            else
                            {
                                MessageLabel.Text = "An account for this email address already exists. If you believe this is your account "+
                                    "and you have forgot your password, go to <a href=\"login\">login page</a> and click on the 'forgot password' link.";
                            }
                        }
                    }
                }
                else
                    MessageLabel.Text = "Email is not valid";
   
        }
        else
            MessageLabel.Text = "*All fields must be filled in.";
    }

    //protected void MakeItSo(object sender, EventArgs e)
    //{
    //    MessageLabel.Text = "";
    //    if (UserNameTextBox.THE_TEXT != "" && EmailTextBox.THE_TEXT != "" && ConfirmEmailTextBox.THE_TEXT != ""
    //        && PasswordTextBox.THE_TEXT != "" && ConfirmPasswordTextBox.THE_TEXT != "")
    //    {
    //        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //        bool safeToGo = false;
    //        if (dat.TrapKey(UserNameTextBox.THE_TEXT, 1))
    //        {
    //            if (dat.ValidateEmail(EmailTextBox.THE_TEXT))
    //            {
    //                DataSet ds = dat.GetData("SELECT * FROM Users WHERE UserName='" + UserNameTextBox.THE_TEXT + "'");
    //                if (ds.Tables.Count > 0)
    //                    if (ds.Tables[0].Rows.Count > 0)
    //                        MessageLabel.Text = "*This User Name already exists in the database. Choose a different User Name.";
    //                    else
    //                        safeToGo = true;
    //                else
    //                    safeToGo = true;

    //                if (safeToGo)
    //                {
    //                    if (dat.TrapKey(EmailTextBox.THE_TEXT, 2))
    //                    {
    //                        if (EmailTextBox.THE_TEXT == ConfirmEmailTextBox.THE_TEXT)
    //                            safeToGo = true;
    //                        else
    //                        {
    //                            MessageLabel.Text = "*Comfirmation Email must match the Email text box.";
    //                            safeToGo = false;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        MessageLabel.Text = "*The Email must only contain allowed characters.";
    //                        safeToGo = false;
    //                    }

    //                    if (safeToGo)
    //                    {
    //                        if (dat.TrapKey(PasswordTextBox.THE_TEXT, 1))
    //                        {
    //                            safeToGo = true;
    //                        }
    //                        else
    //                        {
    //                            MessageLabel.Text = "*The Password can contain only letters and numbers.";
    //                            safeToGo = false;
    //                        }

    //                        if (safeToGo)
    //                        {
    //                            if (PasswordTextBox.THE_TEXT == ConfirmPasswordTextBox.THE_TEXT)
    //                                safeToGo = true;
    //                            else
    //                            {
    //                                safeToGo = false;
    //                                MessageLabel.Text = "*The Confirm Password must match the Password.";
    //                            }

    //                            if (safeToGo)
    //                            {
    //                                if (CheckImageButton.ImageUrl == "image/CheckSelected.png")
    //                                    safeToGo = true;
    //                                else
    //                                {
    //                                    safeToGo = false;
    //                                    MessageLabel.Text = "*Read and agree to the Terms And Conditions in order to sign up.";
    //                                }


    //                                if (safeToGo)
    //                                {
    //                                    Encryption encrypt = new Encryption();
    //                                    dat.Execute("INSERT INTO Users (UserName, Email, Password, IPs) VALUES('" + UserNameTextBox.THE_TEXT + "', '" + EmailTextBox.THE_TEXT + "', '"+encrypt.encrypt(PasswordTextBox.THE_TEXT)+"', '"+dat.GetIP()+";')");
    //                                    DataSet dsID = dat.GetData("SELECT User_ID FROM Users WHERE UserName='" + UserNameTextBox.THE_TEXT + "'");
    //                                    string ID = dsID.Tables[0].Rows[0]["User_ID"].ToString();
    //                                    string message = "Welcome to Hippo Happenings! <br/> It is now your place for Events, "
    //                                    + "Venues, Classifieds and connecting with people in your area. <br/> To start, select your preferences"
    //                                    +" for filtering your content on the site by going to <a class=\"AddLink\" href=\"UserPreferences.aspx\">My Preferences</a>. ";
    //                                    dat.Execute("INSERT INTO UserPreferences (UserID, EventsAttended, CalendarPrivacyMode, CommunicationPrefs, CommentsPreferences, PollPreferences, CatCountry) VALUES ("+ID+", 0, 3, 1, 1, 1, 223)");
    //                                    dat.Execute("INSERT INTO UserMessages (MessageContent, MessageSubject, "
    //                                        + "From_UserID, To_UserID, Date, [Read], Mode, Live) VALUES('" + message + 
    //                                        "', 'Hippo Happenings Welcome!', 6, "+ID+", '"+DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).ToString()+"', 'False', 1, 'True')");

                                        

    //                                    MessageRadWindow.NavigateUrl = "CatConfirm.aspx?ID="+ID;
    //                                    MessageRadWindow.Visible = true;
    //                                    MessageRadWindowManager.VisibleOnPageLoad = true;

    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //            else
    //                MessageLabel.Text = "Email is not valid";
    //        }
    //        else
    //            MessageLabel.Text = "User Name can only contain letter and numbers.";
    //    }
    //    else
    //        MessageLabel.Text = "*All fields must be filled in.";
    //}

    

}
