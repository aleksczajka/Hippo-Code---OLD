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

public partial class Feedback : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlLink lk = new HtmlLink();
        HtmlHead head = (HtmlHead)Page.Header;
        lk.Attributes.Add("rel", "canonical");
        lk.Href = "http://hippohappenings.com/Feedback.aspx";
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

        //Button button = (Button)dat.FindControlRecursive(this, "Button1");
        //button.CssClass = "NavBarImageFeedbackSelected";

        FormsAuthenticationTicket authTicket = null;
        try
        {
            Session["RedirectTo"] = Request.Url.AbsoluteUri;
            string group = "";
            if (authCookie != null)
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                group = authTicket.UserData.ToString();
            }

            if (group.Contains("User"))
            {
                Session["User"] = authTicket.Name;
                DataSet ds1 = dat.GetData("SELECT UserName FROM Users WHERE User_ID=" + authTicket.Name);
                Session["UserName"] = ds1.Tables[0].Rows[0]["UserName"].ToString();
                LoggedInPanel.Visible = true;
                LogInPanel.Visible = false;
                DataSet ds2 = dat.RetrieveAds(Session["User"].ToString(), false);
            }
            else
            {
                LoggedInPanel.Visible = false;
                LogInPanel.Visible = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
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
                //DataSet ds2 = dat.RetrieveAds(Session["User"].ToString(), false);
                //Ads.DATA_SET = ds2;
                //Ads.MAIN_AD_DATA_SET = dat.RetrieveMainAds(Session["User"].ToString());
            }
            else
            {
                //Ads.DATA_SET = dat.RetrieveAllAds(false);
                //Ads.MAIN_AD_DATA_SET = dat.RetrieveAllAds(true);
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void SendIt(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (SubjectTextBox.Text != "" && MessageTextBox.Text != "")
        {
            try
            {
                Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
                SubjectTextBox.Text = dat.stripHTML(SubjectTextBox.Text);
                MessageTextBox.Text = dat.stripHTML(MessageTextBox.Text);

                string command = "INSERT INTO MessagesForAdmin (UserID, Message, Subject, Type) VALUES(@p0, @p1, @p2, @p3)";
                SqlDbType[] types = { SqlDbType.Int, SqlDbType.NVarChar, SqlDbType.NVarChar, SqlDbType.Int };
                object[] data = { int.Parse(Session["User"].ToString()), MessageTextBox.Text, SubjectTextBox.Text, 3};
                dat.ExecuteWithParemeters(command, types, data);

                DataSet dsUser = dat.GetData("SELECT * FROM Users WHERE User_ID=" + Session["User"].ToString());

                dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                    System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                    "Feedback from site. User logged in. UserID: " + Session["User"].ToString() +
                    "<br/><br/>User's Subject: " + SubjectTextBox.Text + "<br/><br/>User's Message: " + MessageTextBox.Text,
                    "Feedback from HippoHappenings[Logged in]: " + SubjectTextBox.Text); 
                Encryption encrypt = new Encryption();
                MessageRadWindow.NavigateUrl = "Message.aspx?message=" +
                    encrypt.encrypt("<div style=\"height: 200px; vertical-align: middle;\">Your message has been sent<br/><br/><br/><br/><button onclick=\"Search('Home.aspx');\" name=\"Ok\" title=\"Ok\">Ok</button></div>");
                MessageRadWindow.Visible = true;
                MessageRadWindowManager.VisibleOnPageLoad = true;
            }
            catch (Exception ex)
            {

            }
        }
        else
        {
            if (SubjectTextBox.Text == "")
                SubjectRequired.Text = "*Please include the subject";
            if (MessageTextBox.Text == "")
                MessageRequired.Text = "*Please include a message";
        }
    }

    protected void SendItNotLoggedIn(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        string message = ConfirmEmail();
        if (message == "Success")
        {
            if (TextBox1.Text != "" && TextBox2.Text != "")
            {
                try
                {
                    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
                    TextBox1.Text = dat.stripHTML(TextBox1.Text);
                    TextBox2.Text = dat.stripHTML(TextBox2.Text);
                    string command = "INSERT INTO MessagesForAdmin (Message, Subject, Type, Email) VALUES(@p0, @p1, @p2, @p3)";
                    SqlDbType[] types = { SqlDbType.NChar, SqlDbType.NVarChar, SqlDbType.Int, SqlDbType.NVarChar };
                    object[] data = { TextBox2.Text, TextBox1.Text, 3, EmailTextBox.Text.Trim().ToLower() };
                    dat.ExecuteWithParemeters(command, types, data);
                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                                    System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                    "Feedback from site. User not logged in. Email: " + EmailTextBox.Text.Trim().ToLower() +
                                    "<br/><br/>User's Subject: " + TextBox1.Text + "<br/><br/>User's Message: " + TextBox2.Text,
                                    "Feedback from HippoHappenings[Not logged in]: " + TextBox1.Text);
                    Encryption encrypt = new Encryption();
                    MessageRadWindow.NavigateUrl = "Message.aspx?message=" +
                        encrypt.encrypt("<div style=\"height: 200px; vertical-align: middle;\"><br/>Your message has been sent. <br/><br/>Thank you for your feedback!<br/><br/><br/><button onclick=\"Search('Home.aspx');\" name=\"Ok\" title=\"Ok\">Ok</button></div>");
                    MessageRadWindow.Visible = true;
                    MessageRadWindowManager.VisibleOnPageLoad = true;
                }
                catch (Exception ex)
                {
                    Label2.Text = ex.ToString();
                }
            }
            else
            {
                if (TextBox1.Text.Trim() == "")
                    Label1.Text = "*Please include the subject";
                if (TextBox2.Text.Trim() == "")
                    Label2.Text = "*Please include a message";
            }
        }
        else
        {
            Label2.Text = message;
        }
    }

    protected string ConfirmEmail()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        string message = "";
        if (EmailTextBox.Text.Trim() == "")
        {
            message = "Please include a valid email.";
        }
        else if (ConfirmTextBox.Text.Trim() == "")
        {
            message = "Please include a confirmation email.";
        }
        else if (EmailTextBox.Text.Trim().ToLower() != ConfirmTextBox.Text.Trim().ToLower())
        {
            message = "Email and Confirm Email fields must match.";
        }
        else if (!dat.ValidateEmail(EmailTextBox.Text.Trim()))
        {
            message = "Email is not valid.";
        }
        else
        {
            message = "Success";
        }

        return message;
    }
}
