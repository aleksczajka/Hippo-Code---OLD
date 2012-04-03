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

public partial class SendMessageToParticipants : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlMeta hm = new HtmlMeta();
        HtmlHead head = (HtmlHead)Page.Header;
        hm.Name = "ROBOTS";
        hm.Content = "NOINDEX, FOLLOW";
        head.Controls.AddAt(0, hm);

        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        ImageButton9.OnClientClick = "javascript:Search();";

    }

    protected void SavePrefs(object sender, EventArgs e)
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

        DataView dvMembers;



        if (TheCheckList.Items[0].Selected && TheCheckList.Items[1].Selected)
        {
            if (Request.QueryString["type"].ToString() == "e")
                dvMembers = dat.GetDataDV("SELECT * FROM GroupEvent_Members GM, Users U WHERE " +
                "U.User_ID=GM.UserID AND GM.GroupEventID=" + Request.QueryString["ID"].ToString());
            else
                dvMembers = dat.GetDataDV("SELECT * FROM Group_Members GM, Users U WHERE " +
                "U.User_ID=GM.MemberID AND GM.GroupID=" + Request.QueryString["ID"].ToString());
        }
        else
        {
            if (TheCheckList.Items[0].Selected)
            {
                if (Request.QueryString["type"].ToString() == "e")
                    dvMembers = dat.GetDataDV("SELECT * FROM GroupEvent_Members GM, Users U WHERE " +
                    "U.User_ID=GM.UserID AND GM.GroupEventID=" + Request.QueryString["ID"].ToString() + " AND Accepted <> 'True'");
                else
                    dvMembers = dat.GetDataDV("SELECT * FROM Group_Members GM, Users U WHERE " +
                    "U.User_ID=GM.MemberID AND GM.GroupID=" + Request.QueryString["ID"].ToString() + " AND Accepted <> 'True'");
            }
            else
            {
                if (Request.QueryString["type"].ToString() == "e")
                    dvMembers = dat.GetDataDV("SELECT * FROM GroupEvent_Members GM, Users U WHERE " +
                    "U.User_ID=GM.UserID AND GM.GroupEventID=" + Request.QueryString["ID"].ToString() + " AND Accepted='True'");
                else
                    dvMembers = dat.GetDataDV("SELECT * FROM Group_Members GM, Users U WHERE " +
                    "U.User_ID=GM.MemberID AND GM.GroupID=" + Request.QueryString["ID"].ToString() + " AND Accepted='True'");
            }
        }
            
            

        DataView dvGroup;
        if (Request.QueryString["type"].ToString() == "e")
            dvGroup = dat.GetDataDV("SELECT * FROM GroupEvents WHERE ID=" +
            Request.QueryString["ID"].ToString());
        else
            dvGroup = dat.GetDataDV("SELECT * FROM Groups WHERE ID=" +
            Request.QueryString["ID"].ToString());

        TextBox textB;
        TextBox textD;
        CheckBox checkS;
        CheckBox checkD;
        MessageTextBox.Text = dat.StripHTML_LeaveLinks(MessageTextBox.Text.Trim());
        string message = "";
        if (Request.QueryString["type"].ToString() == "e")
            message = "You have a message from Group Event " +
                 dvGroup[0]["Name"].ToString();
        else
            message = "You have a message from Group " +
             dvGroup[0]["Header"].ToString();

        string userid = "";
        foreach (DataRowView row in dvMembers)
        {
            if (Request.QueryString["type"].ToString() == "e")
                userid = row["UserID"].ToString();
            else
                userid = row["MemberID"].ToString();
            dat.Execute("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, To_UserID, " +
            "[Date], [Read], [Mode], [Live]) VALUES('" + MessageTextBox.Text.Replace("'", "''") + "', '" +
            message + "', " +
            Session["User"].ToString() + ", " + userid + ", '" +
            DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")) +
            "', 'False', 0, 'True')");

            dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
            System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(), 
            row["Email"].ToString(),
            MessageTextBox.Text.Replace("'", "''"), message);
        }

        RemovePanel.Visible = false;
        ThankYouPanel.Visible = true;
    }
}
