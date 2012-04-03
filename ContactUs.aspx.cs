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

public partial class ContactUs : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlLink lk = new HtmlLink();
        HtmlHead head = (HtmlHead)Page.Header;
        lk.Attributes.Add("rel", "canonical");
        lk.Href = "http://hippohappenings.com/contact-us";
        head.Controls.AddAt(0, lk);

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
        Data dat = new Data(isn);
        SendItButton.SERVER_CLICK += SendIt;
        SendItButton2.SERVER_CLICK += SendItNotLoggedIn;

        try
        {
            Session["RedirectTo"] = Request.Url.AbsoluteUri;

            if (Session["User"] != null)
            {
                LoggedInPanel.Visible = true;
                LogInPanel.Visible = false;
            }
            else
            {
                LoggedInPanel.Visible = false;
                LogInPanel.Visible = true;
            }
        }
        catch (Exception ex)
        {
            LoggedInPanel.Visible = false;
            LogInPanel.Visible = true;
        }

    }

    protected void SendIt(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn); SubjectTextBox.Text = dat.stripHTML(SubjectTextBox.Text);
        MessageTextBox.Text = dat.stripHTML(MessageTextBox.Text);
        if (SubjectTextBox.Text != "" && MessageTextBox.Text != "")
        {
            try
            {
                
                string command = "INSERT INTO MessagesForAdmin (UserID, Message, Subject, Type) VALUES(@p0, @p1, @p2, @p3)";
                SqlDbType[] types = { SqlDbType.Int, SqlDbType.NVarChar, SqlDbType.NVarChar, SqlDbType.Int };
                object[] data = { int.Parse(Session["User"].ToString()), MessageTextBox.Text, SubjectTextBox.Text, 1 };
                dat.ExecuteWithParemeters(command, types, data);

                DataSet dsUser = dat.GetData("SELECT * FROM Users WHERE User_ID="+Session["User"].ToString());

                dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(), 
                    System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString()
                    , "User ID: " + Session["User"].ToString() + ", UserName: " +
                    dsUser.Tables[0].Rows[0]["UserName"].ToString() + 
                    " has filled out a 'Contact Us' form. Here is their message: <br/><br/>" + 
                    MessageTextBox.Text, "Contact Us Form Submitted: "+SubjectTextBox.Text);

                dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                    dsUser.Tables[0].Rows[0]["Email"].ToString()
                    , "<br/><br/>Your contact request has been submitted to Hippo Happenings. We will reply to your "+
                    "request momentarily.", "Hippo Happenings Contact Request Submitted");
                
                Encryption encrypt = new Encryption();
                MessageRadWindow.NavigateUrl = "Message.aspx?message=" + encrypt.encrypt("<br/><br/>Thank you for your contact request. We will get back to you as soon as possible.<br/><br/><div align=\"center\">" +
    "<div style=\"width: 50px;\" onclick=\"Search('home')\">" +
    "<div class=\"topDiv\" style=\"clear: both;\">" +
      "  <img style=\"float: left;\" src=\"NewImages/ButtonLeft.png\" height=\"27px\" /> " +
       " <div class=\"NavyLink\" style=\"font-size: 12px; text-decoration: none; padding-top: 5px;padding-left: 6px; padding-right: 6px;height: 27px;float: left;background: url('NewImages/ButtonPixel.png'); background-repeat: repeat-x;\">" +
           " OK " +
        "</div>" +
       " <img style=\"float: left;\" src=\"NewImages/ButtonRight.png\" height=\"27px\" /> " +
    "</div>" +
    "</div>" +
    "</div>");
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
        string message = ConfirmEmail();
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn); TextBox1.Text = dat.stripHTML(TextBox1.Text);
        TextBox2.Text = dat.stripHTML(TextBox2.Text);
        if (message == "Success")
        {
            if (TextBox1.Text != "" && TextBox2.Text != "")
            {
                try
                {
                    
                    
                    string command = "INSERT INTO MessagesForAdmin (Message, Subject, Type, Email) VALUES(@p0, @p1, @p2, @p3)";
                    SqlDbType[] types = { SqlDbType.NChar, SqlDbType.NVarChar, SqlDbType.Int, SqlDbType.NVarChar };
                    object[] data = { TextBox2.Text, TextBox1.Text, 1, EmailTextBox.Text.Trim().ToLower() };
                    dat.ExecuteWithParemeters(command, types, data);



                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                        System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                        System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString()
                        , "Email: " + EmailTextBox.Text + ", has filled out a 'Contact Us' form. Here is their message: <br/><br/>" +
                        TextBox2.Text, "Contact Us Form Submitted: " + TextBox1.Text);

                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                        System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                        EmailTextBox.Text.Trim().ToLower()
                        , "<br/><br/>Your contact request has been submitted to Hippo Happenings. We will reply to your " +
                        "request momentarily.", "Hippo Happenings Contact Request Submitted");

                    Encryption encrypt = new Encryption();
                    MessageRadWindow.NavigateUrl = "Message.aspx?message=" + encrypt.encrypt("<br/><br/>Thank you for your contact request. We will get back to you as soon as possible.<br/><br/><div align=\"center\">" +
    "<div style=\"width: 50px;\" onclick=\"Search('home')\">" +
    "<div class=\"topDiv\" style=\"clear: both;\">" +
      "  <img style=\"float: left;\" src=\"NewImages/ButtonLeft.png\" height=\"27px\" /> " +
       " <div class=\"NavyLink\" style=\"font-size: 12px; text-decoration: none; padding-top: 5px;padding-left: 6px; padding-right: 6px;height: 27px;float: left;background: url('NewImages/ButtonPixel.png'); background-repeat: repeat-x;\">" +
           " OK " +
        "</div>" +
       " <img style=\"float: left;\" src=\"NewImages/ButtonRight.png\" height=\"27px\" /> " +
    "</div>" +
    "</div>" +
    "</div>");
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

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn); string message = "";
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
