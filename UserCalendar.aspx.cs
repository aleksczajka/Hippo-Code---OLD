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
using System.Collections.Generic;
using Telerik.Web.UI;

public partial class UserCalendar : Telerik.Web.UI.RadAjaxPage
{
    private void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        Session["AdCount"] = 0;
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        #region Take care of bulletins right arrow
        HtmlHead head = (HtmlHead)Page.Header;

        Literal lit = new Literal();
        lit.Text = "<style type=\"text/css\"> " +
            ".radr_Black .radr_button.radr_buttonRight " +
            "{" +
            "    position: relative;" +
            "    right: -405px !important;" +
            "    top: 40% !important;" +
            "} " +
            ".radr_Black .radr_button.radr_buttonLeft { " +
            "background-position: 0 -60px; " +
            "left: -19px !important; " +
            "margin-top: -10px; " +
            "top: 50%; " +
            "}</style>";
        head.Controls.Add(lit);
        #endregion

        string IDtoGet = "";
        if (!IsPostBack)
        {
            try
            {
                if (Session["User"] != null)
                {
                    IDtoGet = Session["User"].ToString();
                    if (Request.QueryString["ID"] != null)
                    {
                        string friendID = Request.QueryString["ID"].ToString();
                        if (Session["User"].ToString() != friendID)
                        {
                            IDtoGet = friendID;
                            DataSet ds = dat.GetData("SELECT * FROM User_Friends UF, Users U "
                            + "WHERE UF.UserID=" + Session["User"].ToString() +
                            " AND UF.FriendID=" + friendID + " AND UF.UserID=U.User_ID ");

                            DataSet dsPrefs = dat.GetData("SELECT * FROM UserPreferences UP, Users U WHERE UP.UserID=U.User_ID AND UP.UserID=" + friendID);

                            if (ds.Tables.Count > 0)
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    if (int.Parse(dsPrefs.Tables[0].Rows[0]["CalendarPrivacyMode"].ToString()) <= 2)
                                    {
                                        CalendarLabel.Text = "<h1>"+
                                            dsPrefs.Tables[0].Rows[0]["UserName"].ToString() + "'s Calendar</h1>";
                                        AdsCalendarPanel.Visible = false;
                                        TextPanel.Visible = false;
                                    }
                                    else
                                    {
                                        Response.Redirect("~/login");
                                    }
                                }
                                else
                                {
                                    if (dsPrefs.Tables[0].Rows[0]["CalendarPrivacyMode"].ToString() == "1")
                                    {
                                        CalendarLabel.Text = "<h1>" + 
                                            dsPrefs.Tables[0].Rows[0]["UserName"].ToString() + "'s Calendar</h1>";
                                        AdsCalendarPanel.Visible = false;
                                        TextPanel.Visible = false;
                                    }
                                    else
                                    {
                                        Response.Redirect("~/login");
                                    }
                                }
                            else
                            {
                                if (dsPrefs.Tables[0].Rows[0]["CalendarPrivacyMode"].ToString() == "1")
                                {
                                    CalendarLabel.Text = "<h1>"+
                                        dsPrefs.Tables[0].Rows[0]["UserName"].ToString() + "'s Calendar</h1>";
                                    AdsCalendarPanel.Visible = false;
                                    TextPanel.Visible = false;
                                }
                                else
                                {
                                    Response.Redirect("~/login");
                                }
                            }

                        }
                        else
                        {
                        }
                    }
                    else
                    {
                        iCalHyperLink.Visible = true;
                    }
                }
                else
                {
                    Response.Redirect("~/login");
                }
            }
            catch (Exception ex)
            {
                CalendarLabel.Text = ex.ToString();
                //Response.Redirect("~/login");
            }


            RadScheduler1.SelectedDate = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
            RadScheduler2.SelectedDate = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
            if (Request.QueryString["Date"] != null)
            {
                DateTime date = DateTime.Parse(Request.QueryString["Date"].ToString());
                RadScheduler1.SelectedView = SchedulerViewType.DayView;
                RadScheduler1.SelectedDate = date;
            }
        }
        else
        {
            IDtoGet = Session["UserToUse"].ToString();
        }

        Session["UserToUse"] = IDtoGet;

        RadScheduler1.DataSource = GetUserEvents(IDtoGet);
        RadScheduler2.DataSource = dat.GetData("SELECT A.Header, AC.AdID, AC.DateTimeStart AS Start, DATEADD(day,30,AC.DateTimeStart) AS [End] FROM Ads A, Ad_Calendar AC WHERE A.Ad_ID=AC.AdID AND A.User_ID=" + IDtoGet);
        RadScheduler1.DataBind();
        RadScheduler2.DataBind();
        userLiteral.Text = "<div style=\"display: none;\" id=\"userID\">" + Session["UserToUse"].ToString() + "</div>";

    }

    protected DataView GetUserEvents(string IDtoGet)
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

        DataView dvNormal = dat.GetDataDV("SELECT EO.DateTimeStart AS Start, EO.DateTimeEnd AS [End], " +
            "SUBSTRING(E.Header, 0, 16) + ' ...' AS ShortHeader, E.Header, CONVERT(NVARCHAR,E.ID)" +
            " + ';'+CONVERT(NVARCHAR,EO.ID)+';1' AS ID, E.Address FROM User_Calendar UC, Events E, " +
            "Event_Occurance EO WHERE EO.EventID=E.ID AND UC.EventID=E.ID AND UC.UserID=" + IDtoGet);

        DataView dvGroup = dat.GetDataDV("SELECT EO.DateTimeStart AS Start, EO.DateTimeEnd AS [End], " +
            "SUBSTRING(E.Name, 0, 16) + ' ...' AS ShortHeader, E.Name AS Header, CONVERT(NVARCHAR,E.ID)" +
            " + ';'+CONVERT(NVARCHAR,EO.ID)+';2' AS ID, EO.City + ' ' +EO.State + ' '+EO.Zip AS Address, 'True' AS isGroup FROM GroupEvent_Members UC, GroupEvents E, " +
            "GroupEvent_Occurance EO WHERE EO.ID=UC.ReoccurrID AND UC.GroupEventID=E.ID AND UC.Accepted = "+
            "'True' AND UC.UserID=" + IDtoGet);

        DataView dvGroupNonMember = dat.GetDataDV("SELECT EO.DateTimeStart AS Start, EO.DateTimeEnd AS [End], " +
            "SUBSTRING(E.Name, 0, 16) + ' ...' AS ShortHeader, E.Name AS Header, CONVERT(NVARCHAR,E.ID)" +
            " + ';'+CONVERT(NVARCHAR,EO.ID)+';3' AS ID, EO.City + ' ' +EO.State + ' '+EO.Zip AS Address, 'True' AS isGroup FROM User_GroupEvent_Calendar UC, GroupEvents E, " +
            "GroupEvent_Occurance EO WHERE EO.ID=UC.ReoccurrID AND UC.GroupEventID=E.ID AND UC.UserID=" + IDtoGet);

        return MergeDV(dvNormal, MergeDV(dvGroup, dvGroupNonMember));
    }

    protected DataView MergeDV(DataView firstDV, DataView secondDV)
    {
        DataTable dt = firstDV.ToTable();
        DataRow oneRow;
        foreach (DataRowView row in secondDV)
        {
            oneRow = dt.NewRow();
            oneRow["Start"] = row["Start"];
            oneRow["End"] = row["End"];
            oneRow["ShortHeader"] = row["ShortHeader"];
            oneRow["Header"] = row["Header"];
            oneRow["ID"] = row["ID"];
            oneRow["Address"] = row["Address"];
            dt.Rows.Add(oneRow);
        }
        return new DataView(dt, "", "", DataViewRowState.CurrentRows);
    }

    protected void Page_Init(object sender, EventArgs e)
    {

        
    }

    protected void RadScheduler1_AppointmentCreated(object sender, AppointmentCreatedEventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        char[] delim = { ';' };
        string[] tokens = e.Appointment.ID.ToString().Split(delim);

        string aptmntID = tokens[0];
        string reOcurrID = tokens[1];
        string switchOn = tokens[2];

        DataView dv = new DataView();

        switch (switchOn)
        {
            case "1":
                dv = dat.GetDataDV("SELECT EO.DateTimeStart AS Start, EO.DateTimeEnd AS " +
                "[End], E.Header, E.ID, E.Address, V.Name FROM Venues V, User_Calendar UC, Events E, " +
                "Event_Occurance EO WHERE E.Venue=V.ID AND EO.EventID=E.ID AND UC.EventID=E.ID AND UC.UserID=" +
                Session["UserToUse"].ToString() + " AND E.ID=" + aptmntID + " AND EO.ID=" + reOcurrID);
                break;
            case "2":
                dv = dat.GetDataDV("SELECT EO.DateTimeStart AS Start, EO.DateTimeEnd AS " +
                "[End], E.Name, E.ID, EO.City + ' ' +EO.State + ' '+EO.Zip AS Address, E.Content FROM GroupEvent_Members UC, GroupEvents E, " +
                "GroupEvent_Occurance EO WHERE EO.GroupEventID=E.ID AND UC.GroupEventID=E.ID AND UC.UserID=" +
                Session["UserToUse"].ToString() + " AND E.ID=" + aptmntID + " AND EO.ID=" + reOcurrID);
                break;
            case "3":
                dv = dat.GetDataDV("SELECT EO.DateTimeStart AS Start, EO.DateTimeEnd AS " +
                "[End], E.Name, E.ID, EO.City + ' ' +EO.State + ' '+EO.Zip AS Address, E.Content FROM User_GroupEvent_Calendar UC, GroupEvents E, " +
                "GroupEvent_Occurance EO WHERE EO.GroupEventID=E.ID AND UC.GroupEventID=E.ID AND UC.UserID=" +
                Session["UserToUse"].ToString() + " AND E.ID=" + aptmntID + " AND EO.ID=" + reOcurrID);
                break;
            default: break;
        }


        if (dv.Count > 0)
        {
            e.Appointment.ToolTip = "";
            RadToolTip newToolTip = new RadToolTip();

            string addThis = "";
            string content = "";
            if (Session["UserToUse"].ToString() == Session["User"].ToString() && switchOn == "1")
            {
                addThis = "<a href=\"javascript:OpenRad('div" + aptmntID + "', '" +
                aptmntID + "', '" + switchOn + "', 'na');\"><img title=\"Send info to friends\" alt=\"Send info to friends\" style=\"border: none;padding-right: 5px;\" " +
                "src=\"NewImages/Envelope.png\" /></a><a href=\"javascript:OpenCommunication('div" + aptmntID + "', '" +
                aptmntID + "');\"><img style=\"border: none;padding-right: 5px;\" title=\"Set communication on/off for this event.\" alt=\"Set communication on/off for this event.\" src=\"NewImages/EventComment.png\" /></a>" +
                "<a href=\"javascript:OpenRadDelete('div" + aptmntID + "', '" +
                aptmntID + "', '" + switchOn + "', 'na');\"><img style=\"border: none;padding-right: 5px;\" title=\"Delete event from your calendar\" alt=\"Delete event from your calendar\" " +
                "src=\"NewImages/DeleteCircle.png\" /></a><a href=\"javascript:OpenAlarm('div" + aptmntID + "', '" +
                aptmntID + "', '" + reOcurrID + "');\"><img style=\"border: none;\" title=\"Set Reminder for this event\" alt=\"Set Reminder for this event\" " +
                "src=\"NewImages/Alarm.png\" /></a>";
            }

            if (Session["UserToUse"].ToString() == Session["User"].ToString() && switchOn != "1")
            {
                addThis = "<a href=\"javascript:OpenRad('divG" + aptmntID + "', '" +
                aptmntID + "', '" + switchOn + "', '" + reOcurrID + "');\"><img title=\"Send info to friends\" " +
                "alt=\"Send info to friends\" style=\"border: none;padding-right: 5px;\" " +
                "src=\"NewImages/Envelope.png\" /></a>" +
                "<a href=\"javascript:OpenRadDelete('divG" + aptmntID + "', '" +
                aptmntID + "', '" + switchOn + "', '" + reOcurrID + "');\"><img style=\"border: none;\" title=\"Delete " +
                "event from your calendar\" alt=\"Delete event from your calendar\" " +
                "src=\"NewImages/DeleteCircle.png\" /></a>";
            }

            if (switchOn == "1")
            {
                newToolTip.Text = "<div style=\"padding: 10px;display: block; width: 200px;\" id=\"div" + aptmntID + 
                    "\"><a href=\"" + dat.MakeNiceName(dv[0]["Header"].ToString()) + "_" +
                    aptmntID + "_Event\" class=\"AddLink\">" +
                    dat.BreakUpString(dv[0]["Header"].ToString(), 20) + "</a>" +
                    "<br/><br/>" + dv[0]["Name"].ToString() +
                    "<br/>" + e.Appointment.Start.ToShortTimeString() +
                    "<br/>" + dv[0]["Address"].ToString() +
                    "<br/><br/><br/>" + addThis + "</div>";
            }

            newToolTip.TargetControlID = e.Appointment.ClientID;
            newToolTip.IsClientID = true;
            newToolTip.HideEvent = ToolTipHideEvent.ManualClose;
            newToolTip.Animation = ToolTipAnimation.None;
            newToolTip.Position = ToolTipPosition.MiddleRight;
            newToolTip.ShowEvent = ToolTipShowEvent.OnClick;
            newToolTip.Skin = "Sunset";
            ToolTipPanel.Controls.Add(newToolTip);
        }
    }

    //protected void UpdatePanel2_Load(object sender, EventArgs e)
    //{
    //    HttpCookie cookie = Request.Cookies["BrowserDate"];
    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

    //    RadScheduler2.DataSource = dat.GetData("SELECT A.Header, AC.AdID, AC.DateTimeStart AS Start, AC.DateTimeEnd AS [End] FROM Ads A, Ad_Calendar AC WHERE A.Ad_ID=AC.AdID AND A.User_ID=" + Session["User"].ToString());
    //    RadScheduler2.DataBind();
    //    UpdatePanel2.Update();
    //}


    protected void RadScheduler2_AppointmentCreated(object sender, AppointmentCreatedEventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet ds = dat.GetData("SELECT A.Header, AC.AdID, AC.DateTimeStart AS Start, A.Featured, AC.DateTimeEnd AS [End] FROM Ads A, Ad_Calendar AC WHERE A.Ad_ID=AC.AdID AND A.User_ID=" + Session["UserToUse"].ToString());
        DataView dv = new DataView(ds.Tables[0], "AdID=" + e.Appointment.ID, "", DataViewRowState.CurrentRows);

        if (dv.Count > 0)
        {
            e.Appointment.ToolTip = "";
            RadToolTip newToolTip = new RadToolTip();
            newToolTip.Text = "<div style=\"padding: 10px;display: block; width: 200px;\" id=\"div" + e.Appointment.ID + "\"><a href=\"" + dat.MakeNiceName(e.Appointment.Subject) + "_" +
                e.Appointment.ID.ToString() + "_Ad\" class=\"AddLink\">" +
                e.Appointment.Subject + "</a>" +
                "<br/><br/>Start: " + dv[0]["Start"].ToString() +
                "<br/>End: " + dv[0]["End"].ToString() +
                "<br/><br/><br/><a href=\"javascript:OpenAdMsg('" +
                e.Appointment.ID + "');\"><img title=\"Send info to friends\" alt=\"Send info to friends\" style=\"padding-right: 5px;border: none;\" " +
                "src=\"NewImages/Envelope.png\" /></a><a href=\"javascript:OpenRadDeleteAd('div" + e.Appointment.ID + "', '" +
                e.Appointment.ID + "');\"\"><img style=\"border: none;\" title=\"Turn ad off.\" alt=\"Turn ad off.\" src=\"NewImages/DeleteCircle.png\" /></a></div>";

            if (bool.Parse(dv[0]["Featured"].ToString()))
            {
                e.Appointment.BorderColor = System.Drawing.Color.DarkOrange;
                e.Appointment.BorderStyle = BorderStyle.Solid;
            }

            newToolTip.TargetControlID = e.Appointment.ClientID;
            newToolTip.IsClientID = true;
            newToolTip.HideEvent = ToolTipHideEvent.ManualClose;
            newToolTip.Animation = ToolTipAnimation.None;
            newToolTip.Position = ToolTipPosition.MiddleRight;
            newToolTip.ShowEvent = ToolTipShowEvent.OnClick;
            newToolTip.Skin = "Sunset";
            ToolTipPanel2.Controls.Add(newToolTip);
        }
    }

    //[Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
    //public int ShowMessage()
    //{
    //    //Session["Type"] = "Calendar";
    //    //MessageLiteral.Text = "<script type=\"text/javascript\">alert('" + message + "');</script>";
    //    RadWindowManager RadWindowManager = new RadWindowManager();
    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //    RadWindow RadWindow1 = new RadWindow();
    //    RadWindow1.NavigateUrl = "http://google.com";
    //    RadWindow1.Visible = true;

    //    RadWindowManager.Windows.Add(RadWindow1);
    //    RadWindowManager.VisibleOnPageLoad = true;
    //    this.Controls.Add(RadWindowManager);
    //    return 0;
    //}

    //protected override void OnPreRender(EventArgs e)
    //{
    //    base.OnPreRender(e);
    //}

    //protected void RadAjaxManager1_AjaxSettingCreating(object sender, Telerik.Web.UI.AjaxSettingCreatingEventArgs e)
    //{
    //    e.UpdatePanel.UpdateMode = UpdatePanelUpdateMode.Always;
    //}

    //protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    //{
    //    //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
    //    //{
    //    //    string propertyID = DataBinder.Eval(e.Item.DataItem, "PropertyID").ToString();
          
    //    //}
    //}

    //protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    //{
    //    if (e.CommandName == "DisplayPropertySchedule")
    //    {
           
    //    }
    //}

    //protected void RadCalendar1_SelectionChanged(object sender, Telerik.Web.UI.Calendar.SelectedDatesEventArgs e)
    //{
       
    //}

    //protected void RadScheduler1_AppointmentCreated(object sender, Telerik.Web.UI.AppointmentCreatedEventArgs e)
    //{
    //    Resource prop = e.Appointment.Resources.GetResourceByType("Property");
    //    e.Appointment.CssClass = "ID" + prop.Key.ToString();
    //}

    //protected void ShowAll_Click(object sender, EventArgs e)
    //{
       
    //}

    //protected void gotoAd(object sender, EventArgs e)
    //{
    //    Response.Redirect("Ad.aspx?ID=1");
    //}

    //[Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
    //public int DeleteUserEvent(string theID, string userID)
    //{
    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //    dat.Execute("DELETE FROM User_Calendar WHERE UserID=" + userID + " AND EventID=" + theID);
    //    //RadScheduler1.Appointments; 
    //    return int.Parse(theID);
    //}

    //[Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
    public int DeleteAd(string theID, string userID)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        dat.Execute("DELETE FROM Ad_Calendar WHERE AdID=" + theID);
        dat.Execute("DELETE FROM Ads WHERE Ad_ID=" + theID);
        dat.Execute("DELETE FROM Ad_Category_Mapping WHERE AdID=" + theID);
        dat.Execute("DELETE FROM Ad_Slider_Mapping WHERE AdID=" + theID);
        //RadScheduler1.Appointments; 
        return int.Parse(theID);
    }
}
