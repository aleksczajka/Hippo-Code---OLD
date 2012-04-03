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
using System.Diagnostics;

public partial class HippoPoints : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.Date.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        DateTime isNow = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":").Replace("%28", "(").Replace("%29", ")"));
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":").Replace("%28", "(").Replace("%29", ")")));

        HtmlMeta hm = new HtmlMeta();
        HtmlMeta kw = new HtmlMeta();
        HtmlMeta lg = new HtmlMeta();
        HtmlHead head = (HtmlHead)Page.Header;

        hm.Name = "Description";
        hm.Content = "Hippo Boss and Hippo Points are your tool on HippoHappenings for "+
            "promoting yourself online. It's our gift to you for having posted lots of content on our site.";
        head.Controls.AddAt(0, hm);

        kw.Name = "keywords";
        kw.Content = "Hippo Boss and Points promote your cause";
        head.Controls.AddAt(0, kw);

        lg.Name = "language";
        lg.Content = "English";
        head.Controls.AddAt(0, lg);
    }
}