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

public partial class PrizesToWin : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        string cookieName = FormsAuthentication.FormsCookieName;
        HttpCookie authCookie = Context.Request.Cookies[cookieName];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        FormsAuthenticationTicket authTicket = null;
        try
        {
            string group = "";
            if (authCookie != null)
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                group = authTicket.UserData.ToString();
            }

            if (group.Contains("User"))
            {
                Session["User"] = authTicket.Name;
                DataSet ds1 = dat.GetData("SELECT UserName FROM Users WHERE User_ID=" + authTicket.Name);
                Session["UserName"] = ds1.Tables[0].Rows[0]["UserName"].ToString();

                DataSet ds2 = dat.RetrieveAds(Session["User"].ToString(), false);
            }
            else
            {
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
        string cookieName = FormsAuthentication.FormsCookieName;
        HttpCookie authCookie = Context.Request.Cookies[cookieName];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        FormsAuthenticationTicket authTicket = null;
        try
        {
            string group = "";
            if (authCookie != null)
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                group = authTicket.UserData.ToString();
            }

            if (group.Contains("User"))
            {
                //DataSet ds2 = dat.RetrieveAds(Session["User"].ToString(), false);
                //Ads1.DATA_SET = ds2;
                //Ads1.MAIN_AD_DATA_SET = dat.RetrieveMainAds(Session["User"].ToString());
            }
            else
            {
                //Ads1.DATA_SET = dat.RetrieveAllAds(false);
                //Ads1.MAIN_AD_DATA_SET = dat.RetrieveAllAds(true);
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("~/UserLogin.aspx");
        }
    }

    protected void GoPost(object sender, EventArgs e)
    {
        Response.Redirect("~/BlogEvent.aspx");
    }
}
