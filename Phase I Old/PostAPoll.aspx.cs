
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

public partial class PostAPoll : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
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

            if (group.Contains("Admin"))
            {
                if (group.Contains("User"))
                {
                    Session["User"] = authTicket.Name;
                    PollPanel.Visible = true;
                    LoginPanel.Visible = false;
                    //Ads1.DATA_SET = dat.RetrieveAds(authTicket.Name, false);
                    //Ads1.MAIN_AD_DATA_SET = dat.RetrieveMainAds(Session["User"].ToString());
                    
                }
                else
                {
                    PollPanel.Visible = false;
                    LoginPanel.Visible = true;
                    //Ads1.DATA_SET = dat.RetrieveAllAds(false);
                    //Ads1.MAIN_AD_DATA_SET = dat.RetrieveAllAds(true);
                }
            }
            else
            {
                ImageButton calendarLink = (ImageButton)dat.FindControlRecursive(this, "CalendarLink");
                calendarLink.Visible = false;
                Response.Redirect("~/UserLogin.aspx");
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("~/UserLogin.aspx");
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

            if (group.Contains("Admin"))
            {
                if (group.Contains("User"))
                {
                    //Ads1.DATA_SET = dat.RetrieveAds(authTicket.Name, false);
                    //Ads1.MAIN_AD_DATA_SET = dat.RetrieveMainAds(Session["User"].ToString());

                }
                else
                {
                    //Ads1.DATA_SET = dat.RetrieveAllAds(false);
                    //Ads1.MAIN_AD_DATA_SET = dat.RetrieveAllAds(true);
                }
            }
            else
            {
                Response.Redirect("~/UserLogin.aspx");
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("~/UserLogin.aspx");
        }
    }

    protected void CreatePoll(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        if (QuestionTextBox.Text != "")
        {
            if (Answer1TextBox.THE_TEXT != "")
            {
                if (Answer2TextBox.THE_TEXT != "")
                {
                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Polls (Question, Ans1, Ans2, Ans3, ActivePoll, UserID) "
                        +" VALUES(@question, @ans1, @ans2, @ans2, @active, @uID)", conn);
                    cmd.Parameters.Add("@question", SqlDbType.NVarChar).Value = QuestionTextBox.Text;
                    cmd.Parameters.Add("@ans1", SqlDbType.NVarChar).Value = Answer1TextBox.THE_TEXT;
                    cmd.Parameters.Add("@ans2", SqlDbType.NVarChar).Value = Answer2TextBox.THE_TEXT;
                    if (Answer3TextBox.THE_TEXT != "")
                        cmd.Parameters.Add("@ans3", SqlDbType.NVarChar).Value = Answer3TextBox.THE_TEXT;
                    else
                        cmd.Parameters.Add("@ans3", SqlDbType.NVarChar).Value = DBNull.Value;
                    cmd.Parameters.Add("@active", SqlDbType.Bit).Value = false;
                    cmd.Parameters.Add("@uID", SqlDbType.Int).Value = int.Parse(Session["User"].ToString());
                    cmd.ExecuteNonQuery();

                    QuestionTextBox.Text = "";
                    Answer1TextBox.THE_TEXT = "";
                    Answer2TextBox.THE_TEXT = "";
                    Answer3TextBox.THE_TEXT = "";

                    MessageLabel.Text = "Your Poll has been submited for review. ";
                }
                else
                    MessageLabel.Text = "Include the second answer.";
            }
            else
                MessageLabel.Text = "Include the first answer.";
        }
        else
        {
            MessageLabel.Text = "Include the question.";
        }
    }
}
