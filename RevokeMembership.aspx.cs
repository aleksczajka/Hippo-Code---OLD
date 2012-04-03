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

public partial class RevokeMembership : System.Web.UI.Page
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


        DataView dvGroup = dat.GetDataDV("SELECT * FROM Groups WHERE ID="+Request.QueryString["ID"].ToString());
        if (dvGroup[0]["Host"].ToString() == Session["User"].ToString())
        {
            TheLabel.Text = "Since you are the primary host of the group, you must first designate another host for the group. Go to your group's home page and click on 'Edit Members' Prefs'.";
            Button3.Visible = false;
        }

        

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

        dat.Execute("DELETE FROM Group_Members WHERE GroupID=" + Request.QueryString["ID"].ToString() +
            " AND MemberID=" + Session["User"].ToString());

        RemovePanel.Visible = false;
        ThankYouPanel.Visible = true;
    }
}
