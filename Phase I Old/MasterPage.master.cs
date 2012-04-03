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
using System.Globalization;
using Telerik.Web.UI;

public partial class MasterPage : System.Web.UI.MasterPage
{
    public HtmlGenericControl BodyTag
    {
        get
        {
            return bodytag;
        }
        set
        {
            bodytag = value;
        }
    }
    public HtmlForm FormTag
    {
        get
        {
            return form1;
        }
        set
        {
            form1 = value;
        }
    }
    public HtmlHead HEAD_TAG
    {
        get
        {
            return HeadTag;
        }
        set
        {
            HeadTag = value;
        }
    }
    public RadWindow RAD_WINDOW_3
    {
        get { return RadWindow3; }
        set { RadWindow3 = value; }
    }
    public ScriptManager theScriptManager
    {
        get
        {
            return ScriptManager1;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //2/23/2010%2020%3A54%3A38
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        bool isPageNew = bool.Parse(ConfigurationSettings.AppSettings["isPageNew"].ToString());
        if (isPageNew)
            ConfigurationSettings.AppSettings["isPageNew"] = "false";

        if ((Session["User"] == null && Session["UserName"] != null) ||
            (Session["UserName"] == null && Session["User"] != null))
        {
            dat.Execute("DELETE FROM Events_Seen_By_User WHERE userID=" + Session["User"].ToString());
            string save1 = "";
            if (Session["AnonymousUser"] != null)
                save1 = Session["AnonymousUser"].ToString();

            string save2 = "";
            if (Session["BigAnonymousUser"] != null)
                save2 = Session["BigAnonymousUser"].ToString();


            Session.Remove("User");
            Session.Remove("UserName");
            FormsAuthentication.SignOut();


            if (save1 != "")
                Session["AnonymousUser"] = save1;

            if (save2 != "")
                Session["BigAnonymousUser"] = save2;

            Response.Redirect("UserLogin.aspx");
        }

        //if (!IsPostBack)
        //{
            try
            {
                HttpCookie cookieZone = Request.Cookies["TimeZone"];

                if (!IsPostBack)
                {
                    if (Session["User"] == null)
                    {
                        dat.Execute("INSERT INTO IP_User_Traffic (IP, PageName, DateAndTime) " +
                            "VALUES ('" + dat.GetIP() + "', '" + Request.Url.AbsolutePath.ToLower() +
                            "', GETDATE())");
                    }
                    else
                    {
                        dat.Execute("INSERT INTO IP_User_Traffic (IP, UserID, PageName, DateAndTime) " +
                            "VALUES ('" + dat.GetIP() + "', " + Session["User"].ToString() +
                            ",'" + Request.Url.AbsolutePath.ToLower() + "', GETDATE())");
                    }
                }

                if (Session["Searching"] != null)
                {
                    RadWindow3.Title = "Your " + Session["Searching"].ToString().Replace("s", "") + " Search Results";
                }

                if (Session["SearchDS"] != null)
                {
                    if (Session["Searching"].ToString() == "Events" && 
                        Request.Url.AbsolutePath.ToLower() == "/eventsearch.aspx" ||
                        Session["Searching"].ToString() == "Venues" &&
                        Request.Url.AbsolutePath.ToLower() == "/venuesearch.aspx")
                    {
                        RadWindow3.Width = 910;
                        RadWindow3.Height = 550;
                        RadWindow3.Top = 100;
                        RadWindow3.Left = 178;
                    }
                    else
                    {
                        RadWindow3.Top = 10;
                        RadWindow3.Left = 765;
                        RadWindow3.Width = 480;
                        RadWindow3.Height = 620;
                        RadWindow3.VisibleOnPageLoad = true;
                    }

                    RadWindow3.VisibleOnPageLoad = true;
                }
                else
                {
                    RadWindow3.VisibleOnPageLoad = false;
                }

                

            }
            catch (Exception ex)
            {
               
            }
        //}
    }

    protected void setsession(object sender, EventArgs e)
    {
        Session["SearchDS"] = null;
        Session.Remove("SearchDS");

        RadWindow3.VisibleOnPageLoad = false;

    }
}
