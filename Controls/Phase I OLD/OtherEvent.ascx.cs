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

public partial class Controls_OtherEvent : System.Web.UI.UserControl
{
    public string PRESENTED_BY
    {
        get { return presentedBy; }
        set { presentedBy = value; }
    }
    public string TITLE
    {
        get { return title; }
        set { title = value; }
    }
    public string SUMMARY
    {
        get { return summary; }
        set { summary = value; }
    }
    public string EVENT_ID
    {
        get { return eventID; }
        set { eventID = value; }
    }
    private string eventID;
    private string presentedBy;
    private string title;
    private string summary;
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        TitleLabel.Text = title;
        TitleLabel.NavigateUrl = "../" + dat.MakeNiceName(title) + "_" + eventID + "_Event";
        summary = summary.Replace("<br/>", " ");
        if (summary.Length > 70)
            summary = dat.BreakUpString(summary.Substring(0, 70), 35) + "...";
        OtherEventSummaryLabel.Text = summary;
        PresentedByLabel.Text = "presented on " + presentedBy;
    }
}
