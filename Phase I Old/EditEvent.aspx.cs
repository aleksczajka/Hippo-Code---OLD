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

public partial class EditEvent : Telerik.Web.UI.RadAjaxPage
{
    private string UserName;
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
        if (Session["User"] == null)
        {
            Response.Redirect("~/Home.aspx");



            
        }

        if (Request.QueryString["ID"] == null)
            Response.Redirect("Home.aspx");

        if (dat.HasEventPassed(Request.QueryString["ID"].ToString()))
        {
            DataView dvName = dat.GetDataDV("SELECT * FROM Events WHERE ID="+
                Request.QueryString["ID"].ToString());
            Response.Redirect(dat.MakeNiceName(dvName[0]["Header"].ToString()) + "_" +
                Request.QueryString["ID"].ToString() + "_Event");
        }
    }

    protected void GoToEventEdit(object sender, EventArgs e)
    {
        Response.Redirect("EventUpdate.aspx?ID=" + Request.QueryString["ID"].ToString());
    }

    protected void GoToLogin(object sender, EventArgs e)
    {
        Response.Redirect("BlogEvent.aspx?edit=true&ID="+Request.QueryString["ID"].ToString());
    }

}

