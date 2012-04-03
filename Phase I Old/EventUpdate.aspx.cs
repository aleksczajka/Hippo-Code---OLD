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

public partial class EventUpdate : Telerik.Web.UI.RadAjaxPage
{
    private string UserName;
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlLink lk = new HtmlLink();
        HtmlHead head = (HtmlHead)Page.Header;
        lk.Attributes.Add("rel", "canonical");
        lk.Href = "http://hippohappenings.com/EventUpdate.aspx";
        head.Controls.AddAt(0, lk);

        HtmlMeta hm = new HtmlMeta();
        hm.Name = "ROBOTS";
        hm.Content = "NOINDEX, NOFOLLOW";
        head.Controls.AddAt(0, hm);

        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        Response.Redirect("~/Home.aspx");
        if (Session["User"] == null)
        {
            Response.Redirect("~/Home.aspx");
        }
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet dsEvent = dat.GetData("SELECT * FROM Events WHERE ID="+Request.QueryString["ID"].ToString());

        nameLabel.Text = "Enter an update for the event ' " + dsEvent.Tables[0].Rows[0]["Header"].ToString() +" '";


    }

    protected void SubmitIt(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        if (UpdateText.Text.Trim() != "")
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO EventUpdates (modifierID, EventID, eventChange) "+
                "VALUES("+Session["User"].ToString()+", "+Request.QueryString["ID"].ToString()+", @p3)", conn);
            cmd.Parameters.Add("@p3", SqlDbType.NVarChar).Value = UpdateText.Text;
            cmd.ExecuteNonQuery();
            conn.Close();

            string ID = Request.QueryString["ID"].ToString();
            Encryption encrypt = new Encryption();
            DataSet dsEvent = dat.GetData("SELECT * FROM Events WHERE ID=" + ID);
            //send email to users who have this event in their calendar and selected their preferences
            string emailBody = "<br/><br/>Updates have been made to an event in your calendar: Event '\"" + dsEvent.Tables[0].Rows[0]["Header"].ToString() +
                    "\"'. <br/><br/> To view these updates, please go to this event's <a href=\"http://hippohappenings.com/" + dat.MakeNiceName(dsEvent.Tables[0].Rows[0]["Header"].ToString())+
                    "_" + ID + "_Event\">page</a> and click on the updates drop down. " +
                    "<br/><br/><br/>Have a Hippo Happening Day!<br/><br/> <a href=\"http://HippoHappenings.com\">Happening Hippo</a>";

            DataSet dsAllUsers = dat.GetData("SELECT * FROM User_Calendar UC, Users U, UserPreferences UP WHERE U.User_ID=UP.UserID AND UP.EmailPrefs LIKE '%C%' AND U.User_ID=UC.UserID AND UC.EventID=" + ID);

            DataView dv = new DataView(dsAllUsers.Tables[0], "", "", DataViewRowState.CurrentRows);

            

            if (dv.Count > 0)
            {
                for (int i = 0; i < dv.Count; i++)
                {
                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                        System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                        dv[i]["Email"].ToString(), emailBody,
                        "Event '" + dsEvent.Tables[0].Rows[0]["Header"].ToString() + "' has been modified");
                }
            }

            Session["Message"] = "Your update has been posted successfully<br/><br/> Here are your next steps. <br/><br/>";
            Session["Message"] += "<br/><br/>" + "-Go to <a class=\"AddLink\" onclick=\"Search('" + 
                dat.MakeNiceName(dsEvent.Tables[0].Rows[0]["Header"].ToString()) + "_" + ID + "_Event');\">this event's</a> home page.<br/><br/> -<a class=\"AddLink\" onclick=\"Search('RateExperience.aspx?Type=E&ID=" + ID + "');\" >Rate </a>your user experience posting this event.";
            MessageRadWindow.NavigateUrl = "Message.aspx?message=" + encrypt.encrypt(Session["Message"].ToString() +
                "<br/><br/><img onclick=\"Search('Home.aspx');\" onmouseover=\"this.src='image/DoneSonButtonSelected.png'\" onmouseout=\"this.src='image/DoneSonButton.png'\" src=\"image/DoneSonButton.png\"/><br/>");
            MessageRadWindow.Visible = true;
            MessageRadWindowManager.VisibleOnPageLoad = true;

        }
        else
        {
            ErrorLabel.Text = "Please enter an Update.";
        }
    }        
}

