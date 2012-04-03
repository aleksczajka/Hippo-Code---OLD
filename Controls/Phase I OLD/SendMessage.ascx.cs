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

public partial class Controls_SendMessage : System.Web.UI.UserControl
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
        string reocurr = "na";
        if (Session["ReOccurID"] != null)
            reocurr = Session["ReOccurID"].ToString();
        TheLiteral.Text = "<label class=\"AddWhiteLink\" onclick=\"OpenRadBefore_SendMessage('" + reocurr + "');\">" +
            text + "</label>";
        TheLiteral.Text += "<div style=\"display: none;\" id=\"idDiv2\">" + id.ToString() +
            "</div><div style=\"display: none;\" id=\"typeDiv2\">" + type + "</div>";
        Session["Type"] = "Message";
        if (subject != "")
            Session["Subject"] = subject;
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
            a = "e";
        }
        else
        {
            id = Request.QueryString["ID"].ToString();
            a = "v";
        }

        MessageRadWindow.NavigateUrl = "Controls/MessageAlert.aspx?T=Message&ID="+id+"&A="+type;
        MessageRadWindow.Visible = true;

        MessageRadWindowManager.VisibleOnPageLoad = true;
    }

   
}
