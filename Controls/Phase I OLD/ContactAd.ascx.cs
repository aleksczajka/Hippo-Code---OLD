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

public partial class Controls_ContactAd : System.Web.UI.UserControl
{
    public string RE_LABEL
    {
        get { return subject; }
        set { subject = value; }
    }
    public string THE_TEXT
    {
        get { return text; }
        set
        {
            text = value;
        }
    }
    public string TYPE
    {
        get { return type; }
        set { type = value; }
    }
    public int ID
    {
        get { return id; }
        set { id = value; }
    }
    private string subject = "";
    private string text;
    private string type;
    private int id;
    protected void Page_Load(object sender, EventArgs e)
    {
        //Ajax.Utility.RegisterTypeForAjax(typeof(Controls_ContactAd));
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        if (Request.QueryString["AdID"] != null)
        {
            if (bool.Parse(dat.GetDataDV("SELECT * FROM Ads WHERE Ad_ID=" +
                Request.QueryString["AdID"].ToString())[0]["LIVE"].ToString()))
                TheLiteral.Text = "<label class=\"AddWhiteLink\" onclick=\"OpenRadBefore_ContactAd();\">" + text + "</label>";
            else
            {
                TheLiteral.Text = "<label class=\"AddGreenLink\">this ad has been discontinued</label>";
                ImagePanel.Visible = false;
            }
        }
        else if (Request.QueryString["EventID"] != null)
        {
            TheLiteral.Text = "<label class=\"AddWhiteLink\" onclick=\"OpenRadBefore_ContactAd();\">" + text + "</label>";

        }
        else
        {
            TheLiteral.Text = "<label class=\"AddWhiteLink\" onclick=\"OpenRadBefore_ContactAd();\">" + text + "</label>";

        }

        TheLiteral.Text += "<div style=\"display: none;\" id=\"idDiv\">"+id.ToString()+"</div><div style=\"display: none;\" id=\"typeDiv\">"+type+"</div>";
        Session["Type"] = "Message";
        if (subject != "")
            Session["Subject"] = subject;
        
        //if (IsPostBack)
        //{
        //    MessageRadWindowManager.VisibleOnPageLoad = false;
        //}
    }

    protected void OpenMessage(object sender, EventArgs e)
    {
        Session["Type"] = "Message";
        //MessageLiteral.Text = "<script type=\"text/javascript\">alert('" + message + "');</script>";
        string id = "";
        string a = "";
        if (Request.QueryString["AdID"] != null)
        {
            id = Request.QueryString["AdID"].ToString();
            a = "a";
        }
        else if (Request.QueryString["EventID"] != null)
        {
            id = Request.QueryString["EventID"].ToString();
            a = "f";
        }
        else
        {
            id = Request.QueryString["ID"].ToString();
            a = "f";
        }

        MessageRadWindow.NavigateUrl = "Controls/MessageAlert.aspx?T=Message&ID="+id+"&A="+type;
        MessageRadWindow.Visible = true;

        MessageRadWindowManager.VisibleOnPageLoad = true;
    }

   
}
