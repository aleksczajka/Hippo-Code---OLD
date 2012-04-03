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

public partial class DeleteEvent : System.Web.UI.Page
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
        //Ajax.Utility.RegisterTypeForAjax(typeof(Delete));
        if (!IsPostBack)
        {
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

                    userLiteral.Text = "<div style=\"display: none;\" id=\"eventID\">" +
                        Request.QueryString["ID"].ToString() +
                        "</div><div style=\"display: none;\" id=\"userID\">" +
                        Session["User"].ToString() + "</div>";
                  
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
            }  
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        int i = 0;
    }

    protected void SendIt(object sender, EventArgs e)
    {
        
        
    }

    protected void DeleteEventAction(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        if (Request.QueryString["O"] != null)
        {
            if (Request.QueryString["U"] != null)
            {
                dat.Execute("DELETE FROM User_GroupEvent_Calendar WHERE UserID=" + Session["User"].ToString() +
               " AND GroupEventID=" + Request.QueryString["ID"].ToString() + " AND ReoccurrID=" +
               Request.QueryString["O"].ToString());
            }
            else
            {
                dat.Execute("DELETE FROM GroupEvent_Members WHERE UserID=" + Session["User"].ToString() +
               " AND GroupEventID=" + Request.QueryString["ID"].ToString() + " AND ReoccurrID=" +
               Request.QueryString["O"].ToString());
            }
        }
        else
        {
            dat.Execute("DELETE FROM User_Calendar WHERE UserID=" + Session["User"].ToString() +
           " AND EventID=" + Request.QueryString["ID"].ToString());
        }

       
        RemovePanel.Visible = false;
        ThankYouPanel.Visible = true;
        
    }

    //[Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
    //public int DeleteAd1(string theID, string userID)
    //{
    //    try
    //    {
    //        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //        dat.Execute("DELETE FROM Ad_Calendar WHERE AdID=" + theID);
    //        dat.Execute("DELETE FROM Ads WHERE Ad_ID=" + theID);
    //        dat.Execute("DELETE FROM Ad_Category_Mapping WHERE AdID=" + theID);
    //        dat.Execute("DELETE FROM Ad_Slider_Mapping WHERE AdID=" + theID);
    //        //RadScheduler1.Appointments; 
    //        return int.Parse(theID); 
    //    }
    //    catch (Exception ex)
    //    {
    //        return 0;
    //    }
    //}
}
