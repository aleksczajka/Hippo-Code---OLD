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

public partial class EnterEvent : Telerik.Web.UI.RadAjaxPage
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

        

        string cookieName = FormsAuthentication.FormsCookieName;
        HttpCookie authCookie = Context.Request.Cookies[cookieName];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        
        FormsAuthenticationTicket authTicket = null;
        try
        {
            HtmlHead head = (HtmlHead)Page.Header;
            HtmlLink lk = new HtmlLink();
            lk.Href = "http://" + Request.Url.Authority + "/EnterEvent.aspx";
            lk.Attributes.Add("rel", "bookmark");
            head.Controls.AddAt(0, lk);

            EventTitle.Text = "<a  style=\"text-decoration: none; color: white;\" href=\"http://" + Request.Url.Authority + "/EnterEvent.aspx\" >Event Blogging</a>";

            string group = "";
            if (authCookie != null)
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                group = authTicket.UserData.ToString();
            }
            Session["RedirectTo"] = "EnterEvent.aspx";
            if (group.Contains("User"))
            {
                Session["User"] = authTicket.Name;
                WelcomeLabel.Text = "On Hippo Happenings, the users have all the power to post events, ads and venues alike. " +
                    "Having an account with us also allows you to do many other things. You're able to add events to your calendar, communicate with others going to events, add your friends, filter content based on your preferences, " +
                    " feature your ads thoughout the site and much more. <br/><br/>So let's go <a class=\"AddLink\" style=\"font-size: 16px;\" href=\"BlogEvent.aspx\">Blog an Event!</a>";
            }
            else
            {
                WelcomeLabel.Text = "On Hippo Happenings, the users have all the power to post events, ads and venues alike. In order to do so, and for us to maintain clean and manageable content,  "
                    + " we require that you <a class=\"AddLink\" href=\"Register.aspx\">create an account</a> with us. <a class=\"AddLink\" href=\"UserLogin.aspx\">Log in</a> if you have an account already. " +
                    "Having an account with us will allow you to do many other things as well. You'll will be able to add events to your calendar, communicate with others going to events, add your friends, filter content based on your preferences, " +
                    " feature your ads thoughout the site and much more. <br/><br/>So let's go <a class=\"AddLink\" style=\"font-size: 16px;\" href=\"Register.aspx\">Register!</a>";
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
                //Ads1.DATA_SET = dat.RetrieveAds(Session["User"].ToString(), false);
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
}
