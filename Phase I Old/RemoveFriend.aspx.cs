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

public partial class RemoveFriend : System.Web.UI.Page
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


        DataView dvGroup = dat.GetDataDV("SELECT * FROM Users WHERE User_ID=" + Request.QueryString["ID"].ToString());

        TheLabel.Text = " Are you sure you want to remove your friend '" + dvGroup[0]["UserName"].ToString() + "'?";
        

    }

    protected void RemoveMember(object sender, EventArgs e)
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

        dat.Execute("DELETE FROM User_Friends WHERE UserID=" + Session["User"].ToString() +
            " AND FriendID =" + Request.QueryString["ID"].ToString());

        dat.Execute("DELETE FROM User_Friends WHERE UserID=" + Request.QueryString["ID"].ToString() +
            " AND FriendID =" + Session["User"].ToString());

        dat.Execute("DELETE FROM UserFriendPrefs WHERE UserID=" + Session["User"].ToString() +
            " AND FriendID =" + Request.QueryString["ID"].ToString());

        dat.Execute("DELETE FROM UserFriendPrefs WHERE UserID=" + Request.QueryString["ID"].ToString() +
            " AND FriendID =" + Session["User"].ToString());

        RemovePanel.Visible = false;
        ThankYouPanel.Visible = true;
    }
}
