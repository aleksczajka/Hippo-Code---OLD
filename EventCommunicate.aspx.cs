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

public partial class EventCommunicate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlLink lk = new HtmlLink();
        HtmlHead head = (HtmlHead)Page.Header;
        lk.Attributes.Add("rel", "canonical");
        lk.Href = "http://hippohappenings.com/EventCommunicate.aspx";
        head.Controls.AddAt(0, lk);

        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }

        YesButton.SERVER_CLICK += SaveSettingsDB;

        if (!IsPostBack)
        {
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            try
            {
                if (Session["User"] != null)
                {
                    DoUser();
                }
            }
            catch (Exception ex)
            {
            }  
        }
    }

    protected void DoUser()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        DataSet ds = dat.GetData("SELECT * FROM User_Calendar WHERE UserID=" + Session["User"].ToString() + 
            " AND EventID=" + Request.QueryString["EID"].ToString());

        bool isConnect = bool.Parse(ds.Tables[0].Rows[0]["isConnect"].ToString());

        EventLiteral.Text = "<div style=\"display: none;\" id=\"userID\">" + Session["User"].ToString()
            + "</div><div id=\"eventID\" style=\"display: none;\">" + Request.QueryString["EID"].ToString() + 
            "</div>";
        if (isConnect)
        {
            SettingLabel.Text = "<label class=\"AddLink\">ON</label><label>. This means people can contact"+
                "you about this event.</label>";
            ChangeToLabel.Text = "Would you like to turn it <b>OFF</b>?";

            EventLiteral.Text += "<div style=\"display: none;\" id=\"userSetting\">false</div>";
        }
        else
        {
            SettingLabel.Text = "<label class=\"AddLink\">OFF</label><label>. This means people cannot"+
                "contact you about this event.</label>";
            ChangeToLabel.Text = "Would you like to turn it <b>ON</b>?";
            EventLiteral.Text += "<label style=\"display: none;\" id=\"userSetting\">true</label>";
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        int i = 0;
    }

    protected void SaveSettingsDB(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        dat.Execute("UPDATE User_Calendar SET isConnect = ~(isConnect) WHERE UserID=" + Session["User"].ToString() +
            " AND EventID=" + Request.QueryString["EID"].ToString());

        DoUser();
    }

    
   
}
