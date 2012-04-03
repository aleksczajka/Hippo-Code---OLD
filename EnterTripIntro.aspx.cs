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

public partial class EnterTripIntro : Telerik.Web.UI.RadAjaxPage
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


        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn);        
        try
        {
            HtmlHead head = (HtmlHead)Page.Header;
            HtmlLink lk = new HtmlLink();
            lk.Href = "http://" + Request.Url.Authority + "/enter-trip";
            lk.Attributes.Add("rel", "bookmark");
            head.Controls.AddAt(0, lk);

            EventTitle.Text = "<a  style=\"text-decoration: none;\"  href=\"http://" + Request.Url.Authority + "/enter-trip\" ><h1>Entering an Adventure</h1></a>";


            Session["RedirectTo"] = "enter-trip";
            if (Session["User"] != null)
            {
                WelcomeLabel.Text = "On Hippo Happenings, the users have all the power to post events, local trips, bulletins and locales alike. " +
                    "Having an account with us also allows you to do many other things. You're able to add events to your calendar, communicate with others going to events, add your friends, filter content based on your preferences, " +
                    " feature your bulletins thoughout the site and much more. <br/><br/>So let's go <a class=\"NavyLink12\" style=\"font-size: 16px;\" href=\"enter-trip\">Post a Trip!</a>";
            }
            else
            {
                WelcomeLabel.Text = "<h1 class=\"SideColumn\">You Need To Be <a class=\"NavyLink12\" href=\"login\">Logged In</a> to Post A Trip.</h1><br/>On Hippo Happenings, the users have all the power to post events, local trips, bulletins and locales alike. In order to do so, and for us to maintain clean and manageable content,  "
                    + " we require that you <a class=\"NavyLink12\" href=\"Register.aspx\">create an account</a> with us. <a class=\"NavyLink12\" href=\"login\">Log in</a> if you have an account already. " +
                    "Having an account with us will allow you to do many other things as well. You'll will be able to add events to your calendar, communicate with others going to events, add your friends, filter content based on your preferences, " +
                    " feature your bulletins thoughout the site and much more. <br/><br/>So let's go <a class=\"NavyLink12\" style=\"font-size: 16px;\" href=\"Register.aspx\">Register!</a>";
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("~/login");
        }
    }

}
