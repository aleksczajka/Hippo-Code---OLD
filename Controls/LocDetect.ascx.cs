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

public partial class LocDetect : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["User"] == null)
            {
                if (!IsPostBack)
                {
                    HttpCookie cookie = Request.Cookies["BrowserDate"];
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);

                    DateTime isn = DateTime.Now;

                    DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn);

                    Data dat = new Data(isn);

                    if (Session["LocCity"] == null || Session["LocState"] == null || Session["LocCountry"] == null)
                    {
                        LocationLabel.Text = dat.IP2Location().Trim();
                    }
                    else
                    {
                        LocationLabel.Text = Session["LocCity"].ToString().Trim() + ", " +
                            Session["LocState"].ToString().Trim();
                    }
                }
            }
            else
            {
                NotLoggedIn.Visible = false;
            }
        }
        catch (Exception ex)
        {
            LocationLabel.Text = ex.ToString();
        }
    }
}
