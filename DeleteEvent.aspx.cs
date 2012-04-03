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

        BlueButton2.SERVER_CLICK += DeleteEventAction;

        if (!IsPostBack)
        {
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            try
            {
                if (Session["User"] != null)
                {
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

}
