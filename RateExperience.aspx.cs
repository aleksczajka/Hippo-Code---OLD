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

public partial class RateExperience : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlLink lk = new HtmlLink();
        HtmlHead head = (HtmlHead)Page.Header;
        lk.Attributes.Add("rel", "canonical");
        lk.Href = "http://hippohappenings.com/RateExperience.aspx";
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
        //string cookieName = FormsAuthentication.FormsCookieName;
        //HttpCookie authCookie = Context.Request.Cookies[cookieName];

        //FormsAuthenticationTicket authTicket = null;

        SubmitButton.SERVER_CLICK = SendIt;
        try
        {
            //string group = "";
            //if (authCookie != null)
            //{
            //    authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            //    group = authTicket.UserData.ToString();
            //}

            if (Request.QueryString["ID"] != null & Request.QueryString["Type"] != null)
            {
                //Session["User"] = authTicket.Name;
                //DataSet ds1 = dat.GetData("SELECT UserName FROM Users WHERE User_ID=" + authTicket.Name);
                //Session["UserName"] = ds1.Tables[0].Rows[0]["UserName"].ToString();
                LoggedInPanel.Visible = true;
                LogInPanel.Visible = false;

                //DataSet ds2 = dat.RetrieveAds(Session["User"].ToString(), false);
            }
            else
            {
                Response.Redirect("home");
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("home");
        }

        if (Request.QueryString["ID"] == null || Request.QueryString["Type"] == null)
        {
            Response.Redirect("home");
        }
    }

    protected void SendIt(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (MessageTextBox.Text != "")
        {
            try
            {
                Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

                //get user 
                string user = "";

                switch (Request.QueryString["Type"].ToString())
                {
                    case "E":
                        user = dat.GetData("SELECT * FROM Events E, Users U WHERE U.UserName=E.UserName " +
                     "AND E.ID=" + Request.QueryString["ID"].ToString()).Tables[0].Rows[0]["User_ID"].ToString();
                        break;
                    case "V":
                        string temp = "CreatedByUser";
                        if (bool.Parse(Request.QueryString["Edit"].ToString()))
                        {
                            temp = "EditedByUser";
                        }
                        user = dat.GetData("SELECT "+temp+" FROM Venues V WHERE " +
                            " ID=" + Request.QueryString["ID"].ToString()).Tables[0].Rows[0][temp].ToString();
                        break;
                    case "A":
                        user = dat.GetData("SELECT * FROM Ads WHERE Ad_ID=" + 
                            Request.QueryString["ID"].ToString()).Tables[0].Rows[0]["User_ID"].ToString();
                        break;
                    case "G":
                        user = dat.GetData("SELECT * FROM Groups WHERE ID=" +
                            Request.QueryString["ID"].ToString()).Tables[0].Rows[0]["Host"].ToString();
                        break;
                    case "GE":
                        user = dat.GetData("SELECT * FROM GroupEvents WHERE ID=" +
                            Request.QueryString["ID"].ToString()).Tables[0].Rows[0]["UserID"].ToString();
                        break;
                    default: break;
                }

                dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                    System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                    "user: " + user + ", postDetails: " + Request.QueryString["Type"].ToString() + "; ID=" +
                    Request.QueryString["ID"].ToString() + ", rating value: " +
                    RatingRadioList.SelectedValue + ", message: " + MessageTextBox.Text, "Hippo Experience Rating");

                string command = "INSERT INTO SiteExperienceRatings (UserID, PostDetails, ExperienceRating, ExperienceDetails) VALUES(@p0, @p1, @p2, @p3)";
                SqlDbType[] types = { SqlDbType.Int, SqlDbType.NVarChar, SqlDbType.Int, SqlDbType.NVarChar };
                object[] data = { int.Parse(user), 
                    "Type="+Request.QueryString["Type"].ToString()+"; ID="+Request.QueryString["ID"].ToString(), RatingRadioList.SelectedValue, MessageTextBox.Text };
                dat.ExecuteWithParemeters(command, types, data);
                //dat.SendEmail("Hippo happenings email address", "Hippo Happening", YourEmailTextBox.Text, EmailTextBox.Text + "<br/>"+Session["message"].ToString(), EmailSubjectLabel.Text);  
                Encryption encrypt = new Encryption();
                MessageRadWindow.NavigateUrl = "Message.aspx?message=" + encrypt.encrypt("Thank you for your "+
                    "feedback!<br/><br/><div align=\"center\">" +
                "<div style=\"width: 50px;\" onclick=\"Search('home')\">" +
                "<div class=\"topDiv\" style=\"clear: both;\">" +
                  "  <img style=\"float: left;\" src=\"NewImages/ButtonLeft.png\" height=\"27px\" /> " +
                   " <div class=\"NavyLink\" style=\"font-size: 12px; text-decoration: none; padding-top: 5px;padding-left: 6px; padding-right: 6px;height: 27px;float: left;background: url('NewImages/ButtonPixel.png'); background-repeat: repeat-x;\">" +
                       " OK " +
                    "</div>" +
                   " <img style=\"float: left;\" src=\"NewImages/ButtonRight.png\" height=\"27px\" /> " +
                "</div>" +
                "</div>" +
                "</div><br/>");
                MessageRadWindow.Visible = true;
                MessageRadWindowManager.VisibleOnPageLoad = true;
            }
            catch (Exception ex)
            {
                MessageRequired.Text = ex.ToString();
            }
        }
        else
        {
            if (MessageTextBox.Text == "")
                MessageRequired.Text = "*Please include a message";
        }
    }
}
