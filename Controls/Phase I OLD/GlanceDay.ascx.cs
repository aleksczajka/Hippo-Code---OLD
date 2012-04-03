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

public partial class Controls_GlanceDay : System.Web.UI.UserControl
{
    public enum days{Sun, Mon, Tues, Wed, Thurs, Fri, Sat};
    public days THE_DAY
    {
        get { return theDay; }
        set { theDay = value; }
    }
    public int NUM_OF_EVENTS
    {
        get { return numberOfEvents; }
        set { numberOfEvents = value; }
    }
    public DateTime DATE
    {
        get { return date; }
        set { date = value; }
    }
    public string USER_ID
    {
        get { return UserID; }
        set { UserID = value; }
    }
    private string day = "";
    private days theDay;
    private int numberOfEvents = 0;
    private DateTime date;
    private string UserID;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string imageSrc = "";
            string eventStr = "";
            string thisDay = theDay.ToString();
            switch (numberOfEvents)
            {
                case 0:
                    imageSrc = "image/GlanceCalendarOpen.png";
                    eventStr = "open";
                    break;
                default:
                    imageSrc = "image/GlanceCalendarBlue.png";
                    if (numberOfEvents == 1)
                        eventStr = numberOfEvents + " event";
                    else
                        eventStr = numberOfEvents + " events";
                    break;
            }
            HttpCookie cookie = Request.Cookies["BrowserDate"];
            if (DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).DayOfWeek.ToString().Substring(0, 3) == theDay.ToString() || DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).DayOfWeek.ToString().Substring(0, 4) == theDay.ToString() || DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).DayOfWeek.ToString().Substring(0, 5) == theDay.ToString())
            {
                thisDay = "Today";
                imageSrc = "image/GlanceCalendarOrange.png";
            }
            string href = "UserCalendar.aspx?ID=" + UserID + "&Date=" + date.ToShortDateString().Replace('/', '.');
            ImageLiteral.Text = "<a style=\"text-decoration: none; cursor: pointer;\" href=\"" +
                href + "\"><div style=\"float:left;color:White;font-family:Arial;font-size:10px;margin-right:5px;width: 53px; height: 57px; background-image:url('" +
                imageSrc + "'); font-weight: bold;\"><div style=\"padding-left:2px;\"> " +
                thisDay + "</div><br/><div align=\"center\">" + eventStr + "</div></div></a>";
        }
    }
}
