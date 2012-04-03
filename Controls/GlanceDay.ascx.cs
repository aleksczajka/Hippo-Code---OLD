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

public partial class GlanceDay : System.Web.UI.UserControl
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
        DayOfWeek theWeekDay = DayOfWeek.Friday;
        switch (theDay)
        {
            case days.Sun:
                theWeekDay = DayOfWeek.Sunday;
                break;
            case days.Mon:
                theWeekDay = DayOfWeek.Monday;
                break;
            case days.Tues:
                theWeekDay = DayOfWeek.Tuesday;
                break;
            case days.Wed:
                theWeekDay = DayOfWeek.Wednesday;
                break;
            case days.Thurs:
                theWeekDay = DayOfWeek.Thursday;
                break;
            case days.Fri:
                theWeekDay = DayOfWeek.Friday;
                break;
            case days.Sat:
                theWeekDay = DayOfWeek.Saturday;
                break;
        }

        if (!IsPostBack)
        {
            string imageSrc = "";
            string eventStr = "";
            string thisDay = theDay.ToString();
           
            HttpCookie cookie = Request.Cookies["BrowserDate"];
            DateTime datNow = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
            Data dat = new Data(datNow);

            

            int thisDayCount = 0;
            DataView dvAll = dat.GetDataDV("SELECT DISTINCT EO.DateTimeStart AS TheDate, EO.EventID AS ID, E.Header FROM Events E, User_Calendar UC, Event_Occurance EO WHERE " +
                "UC.EventID=EO.EventID AND EO.EventID=E.ID AND UC.UserID=" + Session["User"].ToString());

            int subtraction = 0;

            switch (datNow.DayOfWeek)
            {
                case DayOfWeek.Friday:
                    subtraction = 5;
                    break;
                case DayOfWeek.Monday:
                    subtraction = 1;
                    break;
                case DayOfWeek.Saturday:
                    subtraction = 6;
                    break;
                case DayOfWeek.Sunday:
                    subtraction = 0;
                    break;
                case DayOfWeek.Thursday:
                    subtraction = 4;
                    break;
                case DayOfWeek.Tuesday:
                    subtraction = 2;
                    break;
                case DayOfWeek.Wednesday:
                    subtraction = 3;
                    break;
                default: break;
            }

            string toolTipText = "";

            if (dvAll.Count > 0)
            {
                for (int i = 0; i < dvAll.Count; i++)
                {
                    DateTime date2 = DateTime.Parse(dvAll[i]["TheDate"].ToString());

                    if (date2.DayOfWeek == theWeekDay)
                    {
                        if (dat.IsThisWeek(date2))
                        {
                            thisDayCount++;
                            toolTipText += "<div class=\"TextNormal\" style=\"clear: both;\"><a class=\"NavyLink12\" href=\"" + dat.MakeNiceName(dvAll[i]["Header"].ToString()) +
                                "_" + dvAll[i]["ID"].ToString() + "_Event\">" + dvAll[i]["Header"].ToString() + "</a> at " +
                                DateTime.Parse(dvAll[i]["TheDate"].ToString()).ToShortTimeString() + "</div>";
                        }
                    }
                }
            }

            numberOfEvents = thisDayCount;
            
            switch (numberOfEvents)
            {
                case 0:
                    imageSrc = "NewImages/GlanceCalendarOpen.png";
                    eventStr = "open";
                    break;
                default:
                    imageSrc = "NewImages/GlanceCalendarOpen.png";
                    if (numberOfEvents == 1)
                        eventStr = numberOfEvents + " event";
                    else
                        eventStr = numberOfEvents + " events";
                    break;
            }

            if (datNow.DayOfWeek.ToString().Substring(0, 3) == theDay.ToString() ||
                datNow.DayOfWeek.ToString().Substring(0, 4) == theDay.ToString() ||
                datNow.DayOfWeek.ToString().Substring(0, 5) == theDay.ToString())
            {
                thisDay = "Today";
                imageSrc = "NewImages/GlanceCalendarOrange.png";
            }

            string addStr = "";
            if (toolTipText != "")
                addStr = " style=\"text-decoration: underline;cursor: pointer;\"";
            ImageLiteral.Text = "<div class=\"TextNormal\" id=\"theDay" + theDay.ToString() +
                "\" style=\"font-size: 10px;float:left;margin-right:5px;" +
                "width: 53px; height: 58px; background-image:url('" +
                imageSrc + "'); font-weight: bold;\"><div style=\"padding-left:2px;\"> " +
                thisDay + "</div><br/><div align=\"center\" " + addStr + ">" + eventStr + "</div></div>";
            ImageLiteral.ID = "lit" + theDay.ToString();

            if (toolTipText != "")
            {
                Telerik.Web.UI.RadToolTip tip = new Telerik.Web.UI.RadToolTip();
                tip.Text = "<div style=\"padding: 10px;\">" + toolTipText + "</div>";
                tip.Width = 200;
                tip.TargetControlID = "theDay" + theDay.ToString();
                tip.IsClientID = true;
                tip.HideEvent = Telerik.Web.UI.ToolTipHideEvent.ManualClose;
                tip.Animation = Telerik.Web.UI.ToolTipAnimation.None;
                tip.Position = Telerik.Web.UI.ToolTipPosition.TopCenter;
                tip.ShowEvent = Telerik.Web.UI.ToolTipShowEvent.OnClick;
                tip.Skin = "Sunset";
                ToolTipPanel.Controls.Add(tip);
                tip.RelativeTo = Telerik.Web.UI.ToolTipRelativeDisplay.Element;
            }
        }
    }

}
