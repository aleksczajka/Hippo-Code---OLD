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


public partial class Recommendations : System.Web.UI.Page
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
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet dsAll = dat.RetrieveRecommendedEvents(100, true);

        FillEvents(dsAll);

    }

    protected void ChangeDS(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        DataSet dsAll = new DataSet();
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
                
        switch (TimeFrameDropDown.SelectedValue)
        {
            case "0":
                dat.TIME_FRAME = " AND EO.DateTimeStart > GETDATE() AND EO.DateTimeStart < CONVERT(DATETIME,'" + 
                    DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddMonths(6)+"')";
                break;
            case "6":
                dat.TIME_FRAME = " AND EO.DateTimeStart > GETDATE() ";
                break;
            case "1":
            #region This Week
                string day = "";
                switch(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        day = "0";
                        break;
                    case DayOfWeek.Monday:
                        day = "6";
                        break;
                    case DayOfWeek.Tuesday:
                        day = "5";
                        break;
                    case DayOfWeek.Wednesday:
                        day = "4";
                        break;
                    case DayOfWeek.Thursday:
                        day = "3";
                        break;
                    case DayOfWeek.Friday:
                        day = "2";
                        break;
                    case DayOfWeek.Saturday:
                        day = "1";
                        break;
                    default: day = "0"; break;
                }
                dat.TIME_FRAME = " AND EO.DateTimeStart > GETDATE() AND EO.DateTimeStart <= CONVERT(DATETIME,'"+DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(double.Parse(day))+"')";
                #endregion
                break;
            case "2":
            #region This Weekend
                DateTime dayThurs = new DateTime();
                DateTime dayFri = new DateTime();
                DateTime daySat = new DateTime();
                DateTime daySun = new DateTime();
                string temp = "";
                switch(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        daySun = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(0.00);
                        temp = " (EO.DateTimeStart = CONVERT(DATETIME, '"+daySun.Date+"'))";
                        break;
                    case DayOfWeek.Monday:
                        dayThurs = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(3.00);
                        dayFri = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(4.00);
                        daySat = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(5.00);
                        daySun = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(6.00);
                        temp = " (EO.DateTimeStart = CONVERT(DATETIME, '"+dayThurs.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+dayFri.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+daySat.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+daySun.Date+"'))";
                        break;
                    case DayOfWeek.Tuesday:
                        dayThurs = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(2.00);
                        dayFri = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(3.00);
                        daySat = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(4.00);
                        daySun = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(5.00);
                        temp = " (EO.DateTimeStart = CONVERT(DATETIME, '"+dayThurs.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+dayFri.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+daySat.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+daySun.Date+"'))";
                        break;
                    case DayOfWeek.Wednesday:
                        dayThurs = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(1.00);
                        dayFri = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(2.00);
                        daySat = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(3.00);
                        daySun = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(4.00);
                        temp = " (EO.DateTimeStart = CONVERT(DATETIME, '"+dayThurs.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+dayFri.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+daySat.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+daySun.Date+"'))";
                        break;
                    case DayOfWeek.Thursday:
                        dayThurs = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(0.00);
                        dayFri = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(1.00);
                        daySat = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(2.00);
                        daySun = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(3.00);
                        temp = " (EO.DateTimeStart = CONVERT(DATETIME, '"+dayThurs.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+dayFri.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+daySat.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+daySun.Date+"'))";
                        break;
                    case DayOfWeek.Friday:
                        dayFri = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(0.00);
                        daySat = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(1.00);
                        daySun = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(2.00);
                        temp = " (EO.DateTimeStart = CONVERT(DATETIME, '"+dayFri.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+daySat.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+daySun.Date+"'))";
                        break;
                    case DayOfWeek.Saturday:
                        daySat = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(1.00);
                        daySun = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(2.00);
                        temp = " (EO.DateTimeStart = CONVERT(DATETIME, '"+daySat.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+daySun.Date+"'))";
                        break;
                    default: day = "0"; break;
                }
                dat.TIME_FRAME = " AND EO.DateTimeStart > GETDATE() AND "+temp;
#endregion
                break;
            case "3":
            #region Next Weekend
                DateTime dayThurs1 = new DateTime();
                DateTime dayFri1 = new DateTime();
                DateTime daySat1 = new DateTime();
                DateTime daySun1 = new DateTime();
                string temp1 = "";
                switch(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        dayThurs1 = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(4.00);
                        dayFri1 = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(5.00);
                        daySat1 = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(6.00);
                        daySun1 = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(7.00);
                        temp1 = " (EO.DateTimeStart = CONVERT(DATETIME, '"+dayThurs1.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+dayFri1.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+daySat1.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+daySun1.Date+"'))";
                        break;
                    case DayOfWeek.Monday:
                        dayThurs1 = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(10.00);
                        dayFri1 = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(11.00);
                        daySat1 = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(12.00);
                        daySun1 = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(13.00);
                        temp1 = " (EO.DateTimeStart = CONVERT(DATETIME, '"+dayThurs1.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+dayFri1.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+daySat1.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+daySun1.Date+"'))";
                        break;
                    case DayOfWeek.Tuesday:
                        dayThurs1 = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(9.00);
                        dayFri1 = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(10.00);
                        daySat1 = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(11.00);
                        daySun1 = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(12.00);
                        temp1 = " (EO.DateTimeStart = CONVERT(DATETIME, '"+dayThurs1.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+dayFri1.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+daySat1.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+daySun1.Date+"'))";
                        break;
                    case DayOfWeek.Wednesday:
                        dayThurs1 = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(8.00);
                        dayFri1 = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(9.00);
                        daySat1 = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(10.00);
                        daySun1 = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(11.00);
                        temp1 = " (EO.DateTimeStart = CONVERT(DATETIME, '"+dayThurs1.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+dayFri1.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+daySat1.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+daySun1.Date+"'))";
                        break;
                    case DayOfWeek.Thursday:
                        dayThurs1 = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(7.00);
                        dayFri1 = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(8.00);
                        daySat1 = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(9.00);
                        daySun1 = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(10.00);
                        temp1 = " (EO.DateTimeStart = CONVERT(DATETIME, '"+dayThurs1.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+dayFri1.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+daySat1.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+daySun1.Date+"'))";
                        break;
                    case DayOfWeek.Friday:
                        dayThurs1 = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(6.00);
                        dayFri = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(7.00);
                        daySat = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(8.00);
                        daySun = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(9.00);
                        temp1 = " (EO.DateTimeStart = CONVERT(DATETIME, '"+dayThurs1.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+dayFri1.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+daySat1.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+daySun1.Date+"'))";
                        break;
                    case DayOfWeek.Saturday:
                        dayThurs1 = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(5.00);
                        dayFri = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(6.00);
                        daySat = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(7.00);
                        daySun = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(8.00);
                        temp1 = " (EO.DateTimeStart = CONVERT(DATETIME, '"+dayThurs1.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+dayFri1.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+daySat1.Date+
                            "') OR EO.DateTimeStart = CONVERT(DATETIME, '"+daySun1.Date+"'))";
                        break;
                    default: day = "0"; break;
                }
                dat.TIME_FRAME = " AND EO.DateTimeStart > GETDATE() AND "+temp1;
#endregion
                break;
            case "4":
            #region Next Week
                string endDay = "";
                string startDay = "";
                switch(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        startDay = "7";
                        endDay = "13";
                        break;
                    case DayOfWeek.Monday:
                        startDay = "6";
                        endDay = "12";
                        break;
                    case DayOfWeek.Tuesday:
                        startDay = "5";
                        endDay = "11";
                        break;
                    case DayOfWeek.Wednesday:
                        startDay = "4";
                        endDay = "10";
                        break;
                    case DayOfWeek.Thursday:
                        startDay = "3";
                        endDay = "9";
                        break;
                    case DayOfWeek.Friday:
                        startDay = "2";
                        endDay = "8";
                        break;
                    case DayOfWeek.Saturday:
                        startDay = "1";
                        endDay = "7";
                        break;
                    default: break;
                }
                dat.TIME_FRAME = " AND EO.DateTimeStart > GETDATE() AND EO.DateTimeStart <= "+
                    "CONVERT(DATETIME,'"+DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(double.Parse(endDay))+"') "+
                    "AND EO.DateTimeStart >= CONVERT(DATETIME,'"+DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(double.Parse(startDay))+"')";
                #endregion
                break;
            case "5":
            #region Next Two Weeks
                string endDay2 = "";
                string startDay2 = "";
                switch(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        startDay2 = "14";
                        endDay2 = "19";
                        break;
                    case DayOfWeek.Monday:
                        startDay2 = "13";
                        endDay2 = "18";
                        break;
                    case DayOfWeek.Tuesday:
                        startDay2 = "12";
                        endDay2 = "17";
                        break;
                    case DayOfWeek.Wednesday:
                        startDay2 = "11";
                        endDay2 = "16";
                        break;
                    case DayOfWeek.Thursday:
                        startDay2 = "10";
                        endDay2 = "15";
                        break;
                    case DayOfWeek.Friday:
                        startDay2 = "9";
                        endDay2 = "14";
                        break;
                    case DayOfWeek.Saturday:
                        startDay2 = "8";
                        endDay2 = "13";
                        break;
                    default: break;
                }
                dat.TIME_FRAME = " AND EO.DateTimeStart > GETDATE() AND EO.DateTimeStart <= "+
                    "CONVERT(DATETIME,'"+DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(double.Parse(endDay2))+"') "+
                    "AND EO.DateTimeStart >= CONVERT(DATETIME,'"+DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(double.Parse(startDay2))+"')";
                #endregion
                break;
            case "7":
                dat.TIME_FRAME = " AND EO.DateTimeStart > GETDATE() AND MONTH(EO.DateTimeStart) = " + 
                    DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Month + " AND YEAR(EO.DateTimeStart) = "+DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Year;
                break;
            case "8":
                dat.TIME_FRAME = " AND EO.DateTimeStart > GETDATE() AND ((MONTH(EO.DateTimeStart) = " + 
                    DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Month + " AND YEAR(EO.DateTimeStart) = "+DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Year +") OR ("+
                    "MONTH(EO.DateTimeStart) = "+DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddMonths(1).Month + " AND YEAR(EO.DateTimeStart) = "+
                    DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddMonths(1).Year + "))";
                break;
            default: break;
        }

        dsAll = dat.RetrieveRecommendedEvents(100, true);
        FillEvents(dsAll);
    }

    protected void FillEvents(DataSet dsAll)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        int count = dsAll.Tables[0].Rows.Count;
        Encryption decrypt = new Encryption();
        //if (Request.QueryString["message"] != null)
        //    MessageLabel.Text = decrypt.decrypt(Request.QueryString["message"].ToString());
        //else if (Session["Message"] != null)
        //{
        //    MessageLabel.Text = Session["Message"].ToString();
        //}


        if (count > 0)
        {
            ASP.controls_pager_test_ascx pagerPanel = new ASP.controls_pager_test_ascx();
            pagerPanel.NUMBER_OF_ITEMS_PER_PAGE = 6;
            pagerPanel.PANEL_NAME = "ctl02_Panel";
            pagerPanel.PANEL_CSSCLASS = "FavoritesPanel";
            pagerPanel.WIDTH = 650;
            ArrayList a = new ArrayList(count * 4);

            for (int i = 0; i < count; i++)
            {
                dat.InsertOneEvent(dsAll, i, ref a, true);
            }

            pagerPanel.DATA = a;
            pagerPanel.DataBind2();
            RecommendedEvents.Controls.Clear();
            RecommendedEvents.Controls.Add(pagerPanel);
        }
        else
        {

            Label lab = new Label();
            lab.CssClass = "EventBody";
            lab.Text = "<br/>No Recommended events in this time frame.";

            RecommendedEvents.Controls.Clear();
            RecommendedEvents.Controls.Add(lab);
        }
    }

}
