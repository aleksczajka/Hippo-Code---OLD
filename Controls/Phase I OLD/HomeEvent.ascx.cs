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

public partial class Controls_HomeEvent : System.Web.UI.UserControl
{
    public string DAY
    {
        get { return day; }
        set { day = value; }
    }
    public string MONTH
    {
        get { return month; }
        set { month = value; }
    }
    public string DAY_NUMBER
    {
        get { return daynumber; }
        set { daynumber = value; }
    }
    public string EVENT_NAME
    {
        get { return eventname; }
        set { eventname = value; }
    }
    public string SUMMARY
    {
        get { return summary; }
        set { summary = value; }
    }
    public int EVENT_ID
    {
        get { return eventID; }
        set { eventID = value; }
    }
    private string day;
    private string month;
    private string daynumber;
    private string eventname;
    private string summary;
    private int eventID;
    protected void Page_Load(object sender, EventArgs e)
    {
        Data dat = new Data(DateTime.Now);
        DayLabel.Text = day;
        MonthLabel.Text = month;
        DayNumberLabel.Text = daynumber;
        EventNameLabel.Text = eventname;
        theTable.Attributes.Add("onclick", "window.location = '" + dat.MakeNiceName(eventname) + "_" + eventID + "_Event'");
        EventNameLabel.NavigateUrl = "~/" + dat.MakeNiceName(eventname) + "_" + eventID + "_Event";
        SummaryLabel.Text = summary;
        ReadMoreLink.NavigateUrl = "~/" + dat.MakeNiceName(eventname) + "_" + eventID + "_Event";
        //ReadMoreLink.NavigateUrl = "~/Event.aspx?EventID="+eventID;
    }
}
