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
using Telerik.Web.UI;

public partial class VenueCalendar : Telerik.Web.UI.RadAjaxPage
{
    private void Page_Load(object sender, EventArgs e)
    {
        HtmlLink lk = new HtmlLink();
        HtmlHead head = (HtmlHead)Page.Header;
        lk.Attributes.Add("rel", "canonical");
        lk.Href = "http://hippohappenings.com/locale-calendar";
        head.Controls.AddAt(0, lk);

        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        DateTime dateNow = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));

        if (!IsPostBack)
        {
            try
            {
                if (Session["User"] != null)
                {
                    IntroLabel.Text = "Click on an event for information, send the information to friends or add the event to your calendar. ";
                }
                else
                {
                    IntroLabel.Text = "Click on an event for information. And, <b><a style=\"text-decoration: none;  color: #145769; font-size: 14px; font-family: Arial; text-decoration: underline;\" href=\"login\">Log in</a></b> or <b><a style=\"text-decoration: none; color: #145769; font-size: 14px; font-family: Arial; text-decoration: underline;\"  href=\"Register.aspx\">Register</a></b> to be able to do much more!";
                }
                RadScheduler1.SelectedDate = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
            }
            catch (Exception ex)
            {
                Response.Redirect("home");
            }
        }

        string id = Request.QueryString["ID"].ToString();
        RadScheduler1.DataSource =  GetVenueEvents();
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet ds = dat.GetData("SELECT Name FROM Venues WHERE ID=" + id);
        CalendarHeading.Text = "<a class=\"NavyLink\" href=\"" + dat.MakeNiceName(ds.Tables[0].Rows[0]["Name"].ToString()) + "_" + id + "_Venue\">" + ds.Tables[0].Rows[0]["Name"].ToString() + "</a> Events Calendar ";
    }

    protected DataView GetVenueEvents()
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
        string id = Request.QueryString["ID"].ToString();

        DataView dvNormal = dat.GetDataDV("SELECT EO.DateTimeStart AS Start, EO.DateTimeEnd AS " +
            "[End], E.Header, CONVERT(NVARCHAR,E.ID)+';E;'+CONVERT(NVARCHAR,EO.ID) AS ID, V.Name "+
            "FROM Event_Occurance EO, Events E, Venues " +
            "V WHERE EO.EventID=E.ID AND E.Venue=V.ID AND V.ID=" + id);

        return dvNormal;
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
            oneRow["Header"] = row["Header"];
            oneRow["ID"] = row["ID"];
            oneRow["Name"] = row["Name"];
            dt.Rows.Add(oneRow);
        }
        return new DataView(dt, "", "", DataViewRowState.CurrentRows);
    }

    protected void Page_Init(object sender, EventArgs e)
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
        Session["Type"] = "Calendar";
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
    }

    protected void RadAjaxManager1_AjaxSettingCreating(object sender, Telerik.Web.UI.AjaxSettingCreatingEventArgs e)
    {
        e.UpdatePanel.UpdateMode = UpdatePanelUpdateMode.Always;
    }

    protected void RadScheduler1_AppointmentCreated(object sender, AppointmentCreatedEventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        string venueID = Request.QueryString["ID"].ToString();

        char[] delim = { ';' };
        string[] tokens = e.Appointment.ID.ToString().Split(delim);

        string aptmntID = tokens[0];
        string reOcurrID = tokens[2];
        string switchOn = tokens[1];

        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DateTime dateNow = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));

        DataSet ds = dat.GetData("SELECT DATEADD(minute,450,EO.DateTimeStart)  AS Start, DATEADD(minute,450,EO.DateTimeEnd)  AS [End], E.Header, " +
            "E.ID, E.Address, V.Name FROM Venues V, Events E, Event_Occurance EO WHERE E.Venue=V.ID AND " +
            "EO.EventID=E.ID AND V.ID=" + venueID);
        DataView dv = new DataView(ds.Tables[0], "ID=" + aptmntID, "", DataViewRowState.CurrentRows);


        e.Appointment.ToolTip = "";
        RadToolTip newToolTip = new RadToolTip();
        string address = "";

        DateTime date = e.Appointment.Start;

        newToolTip.Text = "<div width=\"250px\" id=\"divE" + aptmntID + "\"><a href=\"" +
            dat.MakeNiceName(e.Appointment.Subject) + "_" +
                    aptmntID + "_Event\" class=\"NavyLink12\">" +
                    dat.BreakUpString(e.Appointment.Subject, 20) + "</a>" +
                    "<br/>" + date.ToShortTimeString();

        if (Session["User"] != null)
        {
            DataView dvUser = dat.GetDataDV("SELECT * FROM User_Calendar WHERE UserID=" +
                Session["User"].ToString() + " AND EventID=" + aptmntID);

            newToolTip.Text += "<br/><a href=\"javascript:OpenRad2('divE" + aptmntID + "','" +
                aptmntID + "', 'E', " + reOcurrID + ");\"><img title=\"Send info to friends\" alt=\"Send info to " +
                "friends\" style=\"border: none;\" " +
                "src=\"image/Envelope.png\" /></a>";
            string addText = "";
            if (dvUser.Count > 0)
            {
                e.Appointment.BackColor = System.Drawing.Color.Yellow;
                newToolTip.Text = "<span style=\"color: orange;\">Event is in your calendar</span>" +
                    newToolTip.Text + "</div>";
            }
            else
            {

                newToolTip.Text += "<a style=\"text-decoration: none;\" " +
                    "href=\"javascript:AddToCalendar('divE" + aptmntID + "','" +
                aptmntID + "', 'E');\"><img style=\"padding-bottom: 4px; border: 0;\" " +
                "title=\"Add to Calendar\" alt=\"Add to Calendar\" " +
                "src=\"image/CalendarIcon.png\" /></a></div>";
            }
        }
        else
            newToolTip.Text += "</div>";

        newToolTip.TargetControlID = e.Appointment.ClientID;
        newToolTip.IsClientID = true;
        newToolTip.ManualClose = true;
        newToolTip.HideEvent = ToolTipHideEvent.ManualClose;
        newToolTip.Animation = ToolTipAnimation.None;
        newToolTip.AutoCloseDelay = 100000000;
        newToolTip.HideDelay = 1000000;
        newToolTip.ShowEvent = ToolTipShowEvent.OnClick;
        newToolTip.Position = ToolTipPosition.MiddleRight;
        newToolTip.Width = 250;
        newToolTip.Skin = "Sunset";
        ToolTipPanel.Controls.Add(newToolTip);
    }

    protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
    }

    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
    }

    protected void RadCalendar1_SelectionChanged(object sender, Telerik.Web.UI.Calendar.SelectedDatesEventArgs e)
    {
       
    }

    protected void ShowAll_Click(object sender, EventArgs e)
    {
       
    }

    protected void gotoAd(object sender, EventArgs e)
    {
        Response.Redirect("Ad.aspx?ID=1");
    }

   
}
