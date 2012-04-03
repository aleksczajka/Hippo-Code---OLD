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

public partial class SearchResults : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //RadTabStrip1.SelectedIndex = 0;
        //RadMultiPage1.SelectedIndex = 0;
        RadScheduler1.SelectedView = SchedulerViewType.MonthView;
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        //Ajax.Utility.RegisterTypeForAjax(typeof(EventCommunicate));
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        Literal liter = new Literal();
        liter.Text = "<link type=\"text/css\" href=\"Rads_Search.css\" rel=\"stylesheet\" />";
        Page.Header.Controls.Add(liter);


        if (!IsPostBack)
        {
            if (Session["SearchResults"] != null)
            {
                ResultsLabel.Text = Session["SearchResults"].ToString();
            }

            HtmlLink lk = new HtmlLink();
            HtmlHead head = (HtmlHead)Page.Header;
            lk.Attributes.Add("rel", "canonical");
            lk.Href = "http://hippohappenings.com/SearchResults.aspx";
            head.Controls.AddAt(0, lk);
            if (Session["Searching"] != null)
            {
                if (Session["Searching"].ToString() == "Events")
                {
                    if (Session["sortString"] != null)
                        SortDropDown.SelectedValue = Session["sortString"].ToString();
                }
                else
                {
                    SortDropDown.Visible = false;
                }
            }
        }
        try
        {
            ////*********************Map TEST**************************
            //DataSet dsEvents = dat.GetData("SELECT TOP 20 Venue, '0' AS SearchNum, 'US' AS Country, Header, EventGoersCount,  1 AS VID, CONVERT(DATETIME, '9/1/2010') AS DateTimeStart, ID AS HashID, ID AS EID, 'False' " +
            //    "AS isGroup, Header AS Name FROM Events WHERE State='OR' AND City='Portland' ORDER BY Venue");
            //DataView dvEvents = new DataView(dsEvents.Tables[0], "", "", DataViewRowState.CurrentRows);
            //Session["SearchDS"] = dsEvents;
            //Session["Searching"] = "Events";


            ////////////****************************END Map TEST*****************************
            if (Session["SearchDS"] != null)
            {
                DataSet ds = new DataSet();
                ds = (DataSet)Session["SearchDS"];
                //DataRow[] theRows;
                //DataSet ds2 = ds.Copy();
                //if (ds.Tables[0].Rows.Count > 26)
                //{
                //    ds2.Tables[0].Rows.Clear();
                //    for (int i = 0; i < 26; i++)
                //    {
                //        ds2.Tables[0].ImportRow(ds.Tables[0].Rows[i]);
                //    }
                //}
                //ds = ds2;
                NumsLabel.Text = ds.Tables[0].Rows.Count + " Results Found";
                switch (Session["Searching"].ToString())
                {
                    case "Events":
                        TopLiteral.Text += "<script type=\"text/javascript\">selectFirstTab();</script>";
                        DataColumn dc = new DataColumn("SearchNum");
                        if (!ds.Tables[0].Columns.Contains("SearchNum"))
                            ds.Tables[0].Columns.Add(dc);

                        dc = new DataColumn("CalendarNum");
                        if (!ds.Tables[0].Columns.Contains("CalendarNum"))
                            ds.Tables[0].Columns.Add(dc);
                                                
                        DataView dv = new DataView(ds.Tables[0], "", "", DataViewRowState.CurrentRows);
                        if (Session["sortString"] != null)
                            dv.Sort = Session["sortString"].ToString();
                        int numofpages = 10;

                        
                            int countOfUniqueVenues = 0;

                        string[] mapStrings = GetEventLiteral(dv, dat, numofpages);

                        if (!IsPostBack)
                        {
                            //Need to copy the table 
                            DataTable dtCopy = ds.Tables[0].Copy();
                            DataView dvNonReference = new DataView(dtCopy, "", "", DataViewRowState.CurrentRows);
                            DoCalendar(dv);
                            //countOfUniqueVenues = GetAllEventLiteral(dvNonReference, dat);
                            //Session["CountUniqueVenues"] = countOfUniqueVenues;

                            //BottomScriptLiteral.Text += "<script type=\"text/javascript\">initializeAll();</script>";
                        }
                        else
                        {
                            if (Session["CountUniqueVenues"] != null)
                            {
                                countOfUniqueVenues = int.Parse(Session["CountUniqueVenues"].ToString());
                            }
                        }

                        
                        
                        DataSet dsNew = new DataSet();
                        dsNew.Tables.Add(dv.ToTable());

                        ds = dsNew;

                        EventSearchElements.COUNT_UNIQUE_VENUES = countOfUniqueVenues;
                        EventSearchElements.CUT_OFF = 200;
                        EventSearchElements.Visible = true;
                        EventSearchElements.EVENTS_DS = ds;
                        EventSearchElements.IS_WINDOW = true;
                        EventSearchElements.DO_MAP = true;
                        EventSearchElements.DO_CALENDAR = true;
                        EventSearchElements.MAP_STRINGS = mapStrings;
                        EventSearchElements.NUM_OF_PAGES = numofpages;
                        if (Session["sortString"] != null)
                            EventSearchElements.SORT_STR = Session["sortString"].ToString();
                        NumsLabel.Text = EventSearchElements.DataBind2() + " Results Found";
                        break;
                    case "Venues":
                        RadTabStrip1.Tabs[0].Text = "Map";
                        RadTabStrip1.Tabs[1].Visible = false;
                        DataColumn dc2 = new DataColumn("SearchNum");
                        if (!ds.Tables[0].Columns.Contains("SearchNum"))
                            ds.Tables[0].Columns.Add(dc2);
                        DataView dv2 = new DataView(ds.Tables[0], "", "", DataViewRowState.CurrentRows);

                        int numofpages2 = 10;

                        if (!IsPostBack)
                        {
                            //Need to copy the table 
                            DataTable dtCopy = ds.Tables[0].Copy();
                            DataView dvNonReference = new DataView(dtCopy, "", "", DataViewRowState.CurrentRows);
                            //GetAllVenuesLiteral(dvNonReference, dat);
                            //BottomScriptLiteral.Text += "<script type=\"text/javascript\">initializeAll();</script>";
                        }

                        string[] mapStrings2 = GetVenuesLiteral(dv2, dat, numofpages2);
                        
                        DataSet dsNew2 = new DataSet();
                        dsNew2.Tables.Add(dv2.ToTable());

                        ds = dsNew2;
                        VenueSearchElements.DO_MAP = true;
                        VenueSearchElements.Visible = true;
                        VenueSearchElements.VENUE_DS = ds;
                        VenueSearchElements.MAP_STRINGS = mapStrings2;
                        VenueSearchElements.NUM_OF_PAGES = numofpages2;
                        VenueSearchElements.IS_WINDOW = true;
                        VenueSearchElements.DataBind2();
                        break;
                    case "Ads":
                        RadTabStrip1.Visible = false;
                        RadMultiPage1.Visible = false;
                        AdSearchElements.Visible = true;
                        AdSearchElements.AD_DS = ds;
                        AdSearchElements.IS_WINDOW = true;
                        AdSearchElements.DataBind2();
                        break;
                    case "Groups":
                        RadTabStrip1.Visible = false;
                        RadMultiPage1.Visible = false;
                        GroupSearchElements.Visible = true;
                        GroupSearchElements.GROUP_DS = ds;
                        GroupSearchElements.IS_WINDOW = true;
                        GroupSearchElements.DataBind2();
                        break;
                    default: break;
                }
            }
            else
            {
                NumsLabel.Text = "0 Results Found";
                SortDropDown.Visible = false;
                switch (Session["Searching"].ToString())
                {
                    case "Events":
                        RadTabStrip1.Visible = false;
                        RadMultiPage1.Visible = false;
                        HelpUsLiteral.Text = "<label>There were no events found matching your "+
                            "search criteria. But you can help us by posting events at "+
                            "<a class=\"AddLink\" onclick=\"CloseWindow('BlogEvent.aspx')\">Blog an Event</a></label>";
                        break;
                    case "Venues":
                        RadTabStrip1.Visible = false;
                        RadMultiPage1.Visible = false;
                        HelpUsLiteral.Text = "<label>There were no venues found matching your " +
                                                    "search criteria. But you can help us by entering more venues at " +
                                                    "<a class=\"AddLink\" onclick=\"CloseWindow('EnterVenue.aspx')\">Submit a Venue</a></label>"; 
                        break;
                    case "Ads":
                        RadTabStrip1.Visible = false;
                        RadMultiPage1.Visible = false;
                        HelpUsLiteral.Text = "<label>There were no classifieds found matching your " +
                                                    "search criteria. But you can help us by posting classifieds at " +
                                                    "<a class=\"AddLink\" onclick=\"CloseWindow('PostAnAd.aspx')\">Post a Classified</a></label>"; 
                        break;
                    case "Groups":
                        RadTabStrip1.Visible = false;
                        RadMultiPage1.Visible = false;
                        HelpUsLiteral.Text = "<label>There were no public groups found matching your " +
                                                    "search criteria. But you can help us by starting your own groups at " +
                                                    "<a class=\"AddLink\" onclick=\"CloseWindow('EnterGroup.aspx')\">Make a Group</a></label>"; 
                        break;
                    default: break;
                }
                
                
            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString();
        }
    }

    protected void DoCalendar(DataView dv)
    {
        //Fix calendar timezone
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        DateTime dateNow = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
        Data dat = new Data(dateNow);
        
        RadScheduler1.TimeZoneOffset = -(DateTime.UtcNow.Subtract(dateNow));

        //dv.RowFilter = "CalendarNum <> ''";

        RadScheduler1.DataSource = dv;
        RadScheduler1.DataBind();
        RadScheduler1.SelectedDate = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
        //RadScheduler1.SelectedDate = DateTime.Parse("8/9/2010 23:13:00");
        MonthLabel.Text = dat.GetMonth(RadScheduler1.SelectedDate.Month.ToString());
    }

    protected void RadScheduler1_AppointmentCreated(object sender, AppointmentCreatedEventArgs e)
    {
        try
        {
            HttpCookie cookie = Request.Cookies["BrowserDate"];
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));



            char[] delim = { ';' };
            string[] calendarKeys = e.Appointment.ID.ToString().Split(delim);
            bool isGroup = bool.Parse(calendarKeys[0]);
            string EID = calendarKeys[1];
            string reoccurrID = calendarKeys[2];
            bool isVenue = false;
            DataView dvE = new DataView();
            DataView dvV = new DataView();

            DataView dvCountry = new DataView();

            string theDate = "";
            string eventLink = "";
            string venueStr = "";
            string address = "";
            string country = "";

            string eventColumnTitle = "";

            if (isGroup)
            {
                eventColumnTitle = "Name";
                country = dat.GetDataDV("SELECT * FROM Countries C, GroupEvent_Occurance GEO WHERE GEO.ID=" +
                        reoccurrID + " AND C.country_id=GEO.Country ")[0]["country_2_code"].ToString();

                dvE = dat.GetDataDV("SELECT * FROM GroupEvents E, GroupEvent_Occurance EO WHERE EO.ID=" + reoccurrID +
                    " and EO.GroupEventID=E.ID AND E.ID=" + EID);
                
                if (dvE[0]["VenueID"] != null)
                {
                    if (dvE[0]["VenueID"].ToString() != "")
                    {
                        isVenue = true;
                        dvV = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + dvE[0]["VenueID"].ToString());
                        venueStr = "<span style=\"color: #98cb2a; " +
                                            "font-weight: bold;text-decoration: underline; " +
                                            " cursor: pointer;\" onclick=\"CloseWindow(\'" + "../" +
                                            dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                                            dvV[0]["ID"].ToString() + "_Venue" + "\');\">" +
                                            dvV[0]["Name"].ToString().Replace("'",
                                            " ").Replace("(", " ").Replace(")", " ") + "</span><br/>";
                    }

                }
                eventLink = dat.MakeNiceName(dvE[0]["Name"].ToString()) + "_" + reoccurrID + "_" +
                                        dvE[0]["ID"].ToString() + "_GroupEvent";

                if (!isVenue)
                {
                    if (dvE[0]["Country"].ToString() == "223")
                    {
                        address = dvE[0]["StreetNumber"].ToString() + " " + dvE[0]["StreetName"].ToString() +
                            " " + dvE[0]["StreetDrop"].ToString();
                    }
                    else
                    {
                        address = dvE[0]["Location"].ToString();
                    }
                }
                else
                {
                    address = dat.GetAddress(dvV[0]["Address"].ToString(),
                                dvV[0]["Country"].ToString() != "223").Trim();
                }

                address = address.Trim() + " " + dvE[0]["City"].ToString() + " " + dvE[0]["State"].ToString() +
                    " " + dvE[0]["Zip"].ToString() + " " + country;
            }
            else
            {
                eventColumnTitle = "Header";
                dvE = dat.GetDataDV("SELECT * FROM Events E, Event_Occurance EO WHERE EO.ID=" + reoccurrID +
                    " and EO.EventID=E.ID AND E.ID=" + EID);
                dvV = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + dvE[0]["Venue"].ToString());

                dvCountry = dat.GetDataDV("SELECT * FROM countries WHERE country_id=" + dvE[0]["Country"].ToString());
                country = dvCountry[0]["country_2_code"].ToString();
                if (country.ToLower() == "us")
                {
                    try
                    {
                        address = dat.GetAddress(dvV[0]["Address"].ToString(), false);
                    }
                    catch (Exception ex1)
                    {
                        address = "";
                    }
                }
                else
                {
                    address = dat.GetAddress(dvV[0]["Address"].ToString(), true);
                }
                address = address.Trim().Replace("'", "''").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["State"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        country.Replace("'", " ").Replace("(", " ").Replace(")", " ");
                eventLink = dat.MakeNiceName(dvE[0]["Header"].ToString()) + "_" +
                                        dvE[0]["ID"].ToString() + "_Event";
                venueStr = "<span style=\"color: #98cb2a; " +
                                        "font-weight: bold;text-decoration: underline; " +
                                        " cursor: pointer;\" onclick=\"CloseWindow(\'" + "../" +
                                        dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                                        dvV[0]["ID"].ToString() + "_Venue" + "\');\">" +
                                        dvV[0]["Name"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ") + "</span><br/>";
            }

            theDate = dvE[0]["DateTimeStart"].ToString();
            RadToolTip newToolTip = new RadToolTip();

            newToolTip.Text = "<div class='AddLink' style=\"width: 200px;color: #cccccc; font-weight: normal;\">" +
                "<div style=\"color: #1fb6e7; font-weight: bold; text-decoration: " +
                "underline; float: left; cursor: pointer;\" onclick=\"CloseWindow('" + "../" +
                                        eventLink + "');\">" +
                                        dvE[0][eventColumnTitle].ToString().Replace("'", " ").Replace("(",
                                        " ").Replace(")", " ") + "</div><br/>on "+theDate+"<br/>at " + venueStr + address + "</div>";

            newToolTip.TargetControlID = e.Appointment.ClientID;
            newToolTip.IsClientID = true;
            newToolTip.HideEvent = ToolTipHideEvent.ManualClose;
            newToolTip.Animation = ToolTipAnimation.None;
            newToolTip.Position = ToolTipPosition.MiddleRight;
            newToolTip.ShowEvent = ToolTipShowEvent.OnClick;
            newToolTip.Skin = "Black";
            newToolTip.ID = isGroup.ToString() + EID + reoccurrID;
            ToolTipPanel.Controls.Add(newToolTip);
            if (Session["SelectToolTip"] != null)
            {
                if (Session["SelectToolTip"].ToString() == isGroup.ToString() + EID + reoccurrID)
                {
                    newToolTip.Show();
                    Session["SelectToolTip"] = null;
                }
            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString();
        }
    }

    protected string [] GetEventLiteral(DataView dvEvents, Data dat, int numOfRecordsPerPage)
    {
        int thecount = dvEvents.Count;
        int numberInArray = thecount / numOfRecordsPerPage;

        if (thecount % numOfRecordsPerPage != 0)
            numberInArray++;
        string oneStringRecord = "";

        int normalRecordCount = 0;
        int countInArray = 0;
        string[] theArray = new string[numberInArray];
        try
        {
            Hashtable normalEventHash = new Hashtable();

            int i = 0;
            string funcCount = "";

            string theLiteral = "<script type=\"text/javascript\">function initialize"+
                (countInArray+1).ToString()+"(){ "+
               " if(map != null && map != undefined) " +
               " { var address;";
            string address = "";
            string venue = "";
            bool isFirstElement = true;
            string country = "US";

            


            Hashtable venueMapping = new Hashtable();
            Hashtable venuesNumMapping = new Hashtable();
            DataView dvRecords = new DataView();
            DataTable dt = new DataTable();
            DataColumn dc2 = new DataColumn("Address");
            dt.Columns.Add(dc2);
            dc2 = new DataColumn("Letter");
            dt.Columns.Add(dc2);
            int lastUsedCount = 0;
            foreach (DataRowView row in dvEvents)
            {
                if (normalRecordCount % numOfRecordsPerPage == 0)
                {
                    if (normalRecordCount != 0 && normalRecordCount != lastUsedCount)
                    {
                        lastUsedCount = normalRecordCount;
                        //ErrorLabel.Text += "on script insert normal rec count: " + normalRecordCount.ToString();
                        dvRecords = new DataView(dt, "", "Letter ASC", DataViewRowState.CurrentRows);
                        theLiteral = "<script type=\"text/javascript\">function initialize" +
                            (countInArray + 1).ToString() + "(){if(map != null && map != undefined) " +
                           " { var address;";
                        oneStringRecord = theLiteral;
                        int rowCount = 0;
                        foreach (DataRowView rowster in dvRecords)
                        {
                            if (rowCount == 0)
                            {
                                oneStringRecord += venueMapping[rowster["Address"].ToString()].ToString() +
                                "', true);".Replace("   ",
                                " ").Replace("  ", " ");

                                if (rowCount == dvRecords.Count - 1)
                                {
                                    oneStringRecord += " }}</script>".Replace("   ",
                                                                " ").Replace("  ", " ");
                                }
                            }
                            else if (rowCount == dvRecords.Count - 1)
                            {
                                oneStringRecord += venueMapping[rowster["Address"].ToString()].ToString() +
                                                            "', false); }}</script>".Replace("   ",
                                                            " ").Replace("  ", " ");
                            }
                            else
                            {
                                oneStringRecord += venueMapping[rowster["Address"].ToString()].ToString() +
                                                            "', false);".Replace("   ",
                                                            " ").Replace("  ", " ");
                            }
                            rowCount++;
                        }
                        TopLiteral.Text += oneStringRecord;
                        theArray[countInArray] = oneStringRecord;
                        dt.Rows.Clear();
                        countInArray++;
                        venueMapping.Clear();
                        venuesNumMapping.Clear();
                    }

                    venue = "";
                    isFirstElement = true;
                    i = 0;
                }

                if (!bool.Parse(row["isGroup"].ToString()))
                {
                    if (!normalEventHash.Contains(row["EID"].ToString()))
                    {
                        normalEventHash.Add(row["EID"].ToString(), normalRecordCount.ToString());
                        row["CalendarNum"] = dat.GetCalendarLetter(normalRecordCount);

                        address = "";
                        DataView dvE = dat.GetDataDV("SELECT * FROM Events WHERE ID=" + row["EID"].ToString());
                        DataView dvV = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + dvE[0]["Venue"].ToString());

                        DataView dvCountry = dat.GetDataDV("SELECT * FROM countries WHERE country_id=" + dvE[0]["Country"].ToString());
                        country = dvCountry[0]["country_2_code"].ToString();

                        if (country.ToLower() == "us")
                        {
                            try
                            {
                                address = dat.GetAddress(dvV[0]["Address"].ToString(), false);
                            }
                            catch (Exception ex1)
                            {
                                address = "";
                            }
                        }
                        else
                        {
                            address = dat.GetAddress(dvV[0]["Address"].ToString(), true);
                        }

                        if (dvE[0]["Country"].ToString().ToLower() == "uk")
                        {
                            //VenueName.Text + "@&" + 
                            if (venueMapping.ContainsKey(address))
                            {
                                row["SearchNum"] = venuesNumMapping[address].ToString();
                                venueMapping[address] += ",<br/><div style=\\\"color: #1fb6e7; font-weight: bold; text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                        dat.MakeNiceName(dvE[0]["Header"].ToString()) + "_" +
                                        dvE[0]["ID"].ToString() + "_Event" + "\\');\\\">" +
                                        dat.CleanExcelString(dvE[0]["Header"].ToString().Replace("'", 
                                        " ").Replace("(", " ").Replace(")", " ")) + "</div>";
                            }
                            else
                            {
                                if (!isFirstElement)
                                {
                                    i++;
                                    venuesNumMapping.Add(address, dat.GetImage(i.ToString()));
                                    venueMapping.Add(address, "address =  '" + address.Trim().Replace("'", "''").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                                        dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                                        country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; " +
                                        "showAddressUS('<span style=\\\"color: #98cb2a; font-weight: bold;text-decoration: underline; " +
                                        " cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                        dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                                        dvV[0]["ID"].ToString() + "_Venue" + "\\');\\\">" +
                                        dvV[0]["Name"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ") + "</span>', 0, address, 0, address, 1, " + i.ToString() + ", '" +
                                        "<div style=\\\"color: #1fb6e7; font-weight: bold; text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                        dat.MakeNiceName(dvE[0]["Header"].ToString()) + "_" +
                                        dvE[0]["ID"].ToString() + "_Event" + "\\');\\\">" +
                                        dat.CleanExcelString(dvE[0]["Header"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ")) + "</div>");

                                    row["SearchNum"] = dat.GetImage(i.ToString());
                                    DataRow r = dt.NewRow();
                                    r["Address"] = address;
                                    r["Letter"] = dat.GetImage(i.ToString());
                                    dt.Rows.Add(r);
                                }
                                else
                                {
                                    DataRow r = dt.NewRow();
                                    r["Address"] = address;
                                    r["Letter"] = dat.GetImage(i.ToString());
                                    dt.Rows.Add(r);
                                    row["SearchNum"] = dat.GetImage(i.ToString());
                                    venuesNumMapping.Add(address, dat.GetImage(i.ToString()));
                                    venueMapping.Add(address, "address =  '" + address.Trim().Replace("'", "''").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                                        dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                                        country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; showAddressUS('<span style=\\\"color: #98cb2a; font-weight: bold;text-decoration: underline; " +
                                        " cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                        dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                                        dvV[0]["ID"].ToString() + "_Venue" + "\\');\\\">" +
                                        dvV[0]["Name"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ") + "</span>',0, address, 0, address, 1, " + i.ToString() + ", '" +
                                        "<div style=\\\"color: #1fb6e7; font-weight: bold; text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                        dat.MakeNiceName(dvE[0]["Header"].ToString()) + "_" +
                                        dvE[0]["ID"].ToString() + "_Event" + "\\');\\\">" +
                                        dat.CleanExcelString(dvE[0]["Header"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ")) + "</div>");
                                }
                            }

                        }
                        else
                        {
                            //VenueName.Text + "@&" + 
                            if (venueMapping.ContainsKey(address))
                            {
                                row["SearchNum"] = venuesNumMapping[address].ToString();
                                venueMapping[address] += ",<br/><div style=\\\"color: #1fb6e7; font-weight: bold; text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                        dat.MakeNiceName(dvE[0]["Header"].ToString()) + "_" +
                                        dvE[0]["ID"].ToString() + "_Event" + "\\');\\\">" +
                                        dat.CleanExcelString(dvE[0]["Header"].ToString().Replace("'", 
                                        " ").Replace("(", " ").Replace(")", " ")) + "</div>";
                            }
                            else
                            {
                                if (!isFirstElement)
                                {
                                    i++;
                                    venuesNumMapping.Add(address, dat.GetImage(i.ToString()));
                                    venueMapping.Add(address, "address =  '" + address.Trim().Replace("'", "''").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["State"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; showAddressUS('<span style=\\\"color: #98cb2a; font-weight: bold;text-decoration: underline; " +
                                        " cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                        dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                                        dvV[0]["ID"].ToString() + "_Venue" + "\\');\\\">" +
                                        dvV[0]["Name"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ") + "</span>',0, address, 0, address, 1, " + i.ToString() + ", '" +
                                        "<div style=\\\"color: #1fb6e7; font-weight: bold; text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                        dat.MakeNiceName(dvE[0]["Header"].ToString()) + "_" +
                                        dvE[0]["ID"].ToString() + "_Event" + "\\');\\\">" +
                                       dat.CleanExcelString(dvE[0]["Header"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ")) + "</div>");
                                    DataRow r = dt.NewRow();
                                    r["Address"] = address;
                                    r["Letter"] = dat.GetImage(i.ToString());
                                    dt.Rows.Add(r);
                                    row["SearchNum"] = dat.GetImage(i.ToString());
                                }
                                else
                                {
                                    row["SearchNum"] = dat.GetImage(i.ToString());
                                    DataRow r = dt.NewRow();
                                    r["Address"] = address;
                                    r["Letter"] = dat.GetImage(i.ToString());
                                    dt.Rows.Add(r);
                                    venuesNumMapping.Add(address, dat.GetImage(i.ToString()));
                                    venueMapping.Add(address, "address =  '" + address.Trim().Replace("'", "''").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["State"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; showAddressUS('<span style=\\\"color: #98cb2a; font-weight: bold;text-decoration: underline; " +
                                        "cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                        dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                                        dvV[0]["ID"].ToString() + "_Venue" + "\\');\\\">" +
                                        dvV[0]["Name"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ") + "</span>',0, address, 0, address, 1, " + i.ToString() + ", '" +
                                        "<div style=\\\"color: #1fb6e7; font-weight: bold; text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                        dat.MakeNiceName(dvE[0]["Header"].ToString()) + "_" +
                                        dvE[0]["ID"].ToString() + "_Event" + "\\');\\\">" +
                                        dat.CleanExcelString(dvE[0]["Header"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ")) + "</div>");
                                }
                            }

                        }
                        isFirstElement = false;
                        normalRecordCount++;
                        venue = address;
                    }
                    else
                    {
                        row["CalendarNum"] = dat.GetCalendarLetter(int.Parse(normalEventHash[row["EID"].ToString()].ToString()));
                    }
                }
                else
                {
                    bool isVenue = false;
                    address = "";
                    row["CalendarNum"] = dat.GetCalendarLetter(normalRecordCount);


                    DataView dvE = dat.GetDataDV("SELECT * FROM GroupEvents GE, GroupEvent_Occurance GEO WHERE GE.ID=" +
                        row["EID"].ToString() + " AND GEO.GroupEventID=GE.ID AND GEO.ID=" + row["ReoccurrID"].ToString());
                    country = dat.GetDataDV("SELECT * FROM Countries C, GroupEvent_Occurance GEO WHERE GEO.ID=" +
                        row["ReoccurrID"].ToString() +
                        " AND C.country_id=GEO.Country ")[0]["country_2_code"].ToString();

                    DataView dvV = new DataView();

                    if (dvE[0]["VenueID"] != null)
                    {
                        if (dvE[0]["VenueID"].ToString().Trim() != "")
                        {
                            dvV = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + dvE[0]["VenueID"].ToString());
                            address = dat.GetAddress(dvV[0]["Address"].ToString(),
                                dvV[0]["Country"].ToString() != "223").Trim();
                            isVenue = true;
                        }
                    }

                    if (!isVenue)
                    {
                        if (dvE[0]["Country"].ToString() == "223")
                        {
                            address = dvE[0]["StreetNumber"].ToString() + " " + dvE[0]["StreetName"].ToString() +
                                " " + dvE[0]["StreetDrop"].ToString();
                        }
                        else
                        {
                            address = dvE[0]["Location"].ToString();
                        }
                    }

                    address = address.Trim() + " " + dvE[0]["City"].ToString() + " " + dvE[0]["State"].ToString() +
                        " " + dvE[0]["Zip"].ToString() + " " + country;




                    if (venueMapping.ContainsKey(address))
                    {
                        row["SearchNum"] = venuesNumMapping[address];
                        venueMapping[address] += ",<br/><div style=\\\"color: #1fb6e7; font-weight: bold; text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                dat.MakeNiceName(dvE[0]["Name"].ToString()) + "_" + row["ReoccurrID"].ToString() + "_" +
                                dvE[0]["ID"].ToString() + "_GroupEvent" + "\\');\\\">" +
                                dat.CleanExcelString(dvE[0]["Name"].ToString().Replace("'", 
                                " ").Replace("(", " ").Replace(")", " ")) + "</div>";
                    }
                    else
                    {
                        if (!isFirstElement)
                        {

                            i++;
                            if (isVenue)
                                venueMapping.Add(address, "address =  '" + address.Trim().Replace("'", "''").Replace("(",
                                    " ").Replace(")", " ") + "';showAddressUS('<span style=\\\"color: #98cb2a; font-weight: bold;text-decoration: underline; " +
                                        "cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                        dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                                        dvV[0]["ID"].ToString() + "_Venue" + "\\');\\\">" +
                                        dvV[0]["Name"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ") + "</span>',0, address, 0, address, 1, " + i.ToString() + ", '" +
                                "<div style=\\\"color: #1fb6e7; font-weight: bold; text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                dat.MakeNiceName(dvE[0]["Name"].ToString()) + "_" + row["ReoccurrID"].ToString() + "_" +
                                dvE[0]["ID"].ToString() + "_GroupEvent" + "\\');\\\">" +
                                dat.CleanExcelString(dvE[0]["Name"].ToString().Replace("'", 
                                " ").Replace("(", " ").Replace(")", " ")) + "</div>");
                            else
                                venueMapping.Add(address, "address =  '" + address.Trim().Replace("'", "''").Replace("(",
                                " ").Replace(")", " ") + "';showAddressUS('', 0, address, 0, address, 1, " + i.ToString() + ", '" +
                                "<div style=\\\"color: #1fb6e7; font-weight: bold; text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                dat.MakeNiceName(dvE[0]["Name"].ToString()) + "_" + row["ReoccurrID"].ToString() + "_" +
                                dvE[0]["ID"].ToString() + "_GroupEvent" + "\\');\\\">" +
                                dat.CleanExcelString(dvE[0]["Name"].ToString().Replace("'", 
                                " ").Replace("(", " ").Replace(")", " ")) + "</div>");
                            row["SearchNum"] = dat.GetImage(i.ToString());
                            venuesNumMapping.Add(address, dat.GetImage(i.ToString()));
                            row["SearchNum"] = dat.GetImage(i.ToString());
                            DataRow r = dt.NewRow();
                            r["Address"] = address;
                            r["Letter"] = dat.GetImage(i.ToString());
                            dt.Rows.Add(r);
                        }
                        else
                        {
                            row["SearchNum"] = dat.GetImage(i.ToString());

                            if (isVenue)
                                venueMapping.Add(address, "address =  '" + address.Trim().Replace("'", "''").Replace("(",
                                    " ").Replace(")", " ") + "'; showAddressUS('<span style=\\\"color: #98cb2a; font-weight: bold;text-decoration: underline; " +
                                        "cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                        dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                                        dvV[0]["ID"].ToString() + "_Venue" + "\\');\\\">" +
                                        dvV[0]["Name"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ") + "</span>',0, address, 0, address, 1, " + i.ToString() + ", '" +
                                "<div style=\\\"color: #1fb6e7; font-weight: bold; text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                dat.MakeNiceName(dvE[0]["Name"].ToString()) + "_" + row["ReoccurrID"].ToString() + "_" +
                                dvE[0]["ID"].ToString() + "_GroupEvent" + "\\');\\\">" +
                                dat.CleanExcelString(dvE[0]["Name"].ToString().Replace("'", 
                                " ").Replace("(", " ").Replace(")", " ")) + "</div>");
                            else
                                venueMapping.Add(address, "address =  '" + address.Trim().Replace("'", "''").Replace("(",
                                " ").Replace(")", " ") + "';showAddressUS('', 0, address, 0, address, 1, " + i.ToString() + ", '" +
                                "<div style=\\\"color: #1fb6e7; font-weight: bold; text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                dat.MakeNiceName(dvE[0]["Name"].ToString()) + "_" + row["ReoccurrID"].ToString() + "_" +
                                dvE[0]["ID"].ToString() + "_GroupEvent" + "\\');\\\">" +
                                dat.CleanExcelString(dvE[0]["Name"].ToString().Replace("'", 
                                " ").Replace("(", " ").Replace(")", " ")) + "</div>");

                            venuesNumMapping.Add(address, dat.GetImage(i.ToString()));
                            row["SearchNum"] = dat.GetImage(i.ToString());
                            DataRow r = dt.NewRow();
                            r["Address"] = address;
                            r["Letter"] = dat.GetImage(i.ToString());
                            dt.Rows.Add(r);
                        }
                    }

                    isFirstElement = false;
                    normalRecordCount++;
                    venue = address;
                }
            }
            //if (thecount % numOfRecordsPerPage != 0)
            //{
                dvRecords = new DataView(dt, "", "Letter ASC", DataViewRowState.CurrentRows);
                theLiteral = "<script type=\"text/javascript\">function initialize" +
                    (countInArray + 1).ToString() + "(){if(map != null && map != undefined) " +
                   " { var address;";    
            oneStringRecord = theLiteral;
                int theCount = 0;
                foreach (DataRowView rowster in dvRecords)
                {
                    if (theCount == 0)
                    {
                        oneStringRecord += venueMapping[rowster["Address"].ToString()].ToString() +
                        "', true);".Replace("   ",
                        " ").Replace("  ", " ");

                        if (theCount == dvRecords.Count - 1)
                        {
                            oneStringRecord += " }}</script>".Replace("   ",
                                                        " ").Replace("  ", " ");
                        }
                    }
                    else if (theCount == dvRecords.Count - 1)
                    {
                        oneStringRecord += venueMapping[rowster["Address"].ToString()].ToString() +
                                                    "', false); }}</script>".Replace("   ",
                                                    " ").Replace("  ", " ");
                    }
                    else
                    {
                        oneStringRecord += venueMapping[rowster["Address"].ToString()].ToString() +
                                                    "', false);".Replace("   ",
                                                    " ").Replace("  ", " ");
                    }
                    theCount++;
                }
                TopLiteral.Text += oneStringRecord;
                theArray[countInArray] = oneStringRecord;
            //}

            
        }
        catch (Exception ex)
        {
            ErrorLabel.Text += ex.ToString() + "; countInArray: " + countInArray +
                "; theArray.Length: " + theArray.Length + "; eventsCount: " + thecount.ToString() + "; normalRecordCount: " + normalRecordCount.ToString();
        }

        return theArray;
    }

    protected int GetAllEventLiteral(DataView dv, Data dat)
    {
        Hashtable normalEventHash = new Hashtable();

        DataView dvEvents = dv;

        int i = 0;
        string theLiteral = "<script type=\"text/javascript\">function initializeAll(){var address;";
        int thecount = dvEvents.Count;

        string oneStringRecord = "";

        int normalRecordCount = 0;
        int countOfAllUniqueVenues = 0;
        try
        {
            string address = "";
            string venue = "";
            bool isFirstElement = true;
            string country = "US";




            Hashtable venueMapping = new Hashtable();
            Hashtable venuesNumMapping = new Hashtable();
            DataView dvRecords = new DataView();
            DataTable dt = new DataTable();
            DataColumn dc2 = new DataColumn("Address");
            dt.Columns.Add(dc2);
            dc2 = new DataColumn("Letter");
            dt.Columns.Add(dc2);
            foreach (DataRowView row in dvEvents)
            {

                if (!bool.Parse(row["isGroup"].ToString()))
                {
                    if (!normalEventHash.Contains(row["EID"].ToString()))
                    {
                        normalEventHash.Add(row["EID"].ToString(), normalRecordCount.ToString());

                        address = "";
                        DataView dvE = dat.GetDataDV("SELECT * FROM Events WHERE ID=" + row["EID"].ToString());
                        DataView dvV = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + dvE[0]["Venue"].ToString());

                        DataView dvCountry = dat.GetDataDV("SELECT * FROM countries WHERE country_id=" + dvE[0]["Country"].ToString());
                        country = dvCountry[0]["country_2_code"].ToString();

                        if (country.ToLower() == "us")
                        {
                            try
                            {
                                address = dat.GetAddress(dvV[0]["Address"].ToString(), false);
                            }
                            catch (Exception ex1)
                            {
                                address = "";
                            }
                        }
                        else
                        {
                            address = dat.GetAddress(dvV[0]["Address"].ToString(), true);
                        }

                        if (dvE[0]["Country"].ToString().ToLower() == "uk")
                        {
                            //VenueName.Text + "@&" + 
                            if (venueMapping.ContainsKey(address))
                            {
                                venueMapping[address] += ",<br/><div style=\\\"color: #1fb6e7; font-weight: bold; text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                        dat.MakeNiceName(dvE[0]["Header"].ToString()) + "_" +
                                        dvE[0]["ID"].ToString() + "_Event" + "\\');\\\">" +
                                        dat.CleanExcelString(dvE[0]["Header"].ToString().Replace("'", 
                                        " ").Replace("(", " ").Replace(")", " ")) + "</div>";
                            }
                            else
                            {
                                countOfAllUniqueVenues++;
                                if (!isFirstElement)
                                {
                                    i++;
                                    venuesNumMapping.Add(address, dat.GetImage(i.ToString()));
                                    venueMapping.Add(address, "address =  '" + address.Trim().Replace("'", "''").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                                        dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                                        country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; " +
                                        "showAddressUS('<span style=\\\"color: #98cb2a; font-weight: bold;text-decoration: underline; " +
                                        " cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                        dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                                        dvV[0]["ID"].ToString() + "_Venue" + "\\');\\\">" +
                                        dvV[0]["Name"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ") + "</span>', 1, address, 0, address, 1, " + i.ToString() + ", '" +
                                        "<div style=\\\"color: #1fb6e7; font-weight: bold; text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                        dat.MakeNiceName(dvE[0]["Header"].ToString()) + "_" +
                                        dvE[0]["ID"].ToString() + "_Event" + "\\');\\\">" +
                                        dat.CleanExcelString(dvE[0]["Header"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ")) + "</div>");

                                    DataRow r = dt.NewRow();
                                    r["Address"] = address;
                                    r["Letter"] = dat.GetImage(i.ToString());
                                    dt.Rows.Add(r);
                                }
                                else
                                {
                                    DataRow r = dt.NewRow();
                                    r["Address"] = address;
                                    r["Letter"] = dat.GetImage(i.ToString());
                                    dt.Rows.Add(r);
                                    venuesNumMapping.Add(address, dat.GetImage(i.ToString()));
                                    venueMapping.Add(address, "address =  '" + address.Trim().Replace("'", "''").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                                        dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                                        country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; showAddressUS('<span style=\\\"color: #98cb2a; font-weight: bold;text-decoration: underline; " +
                                        " cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                        dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                                        dvV[0]["ID"].ToString() + "_Venue" + "\\');\\\">" +
                                        dvV[0]["Name"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ") + "</span>',1, address, 0, address, 1, " + i.ToString() + ", '" +
                                        "<div style=\\\"color: #1fb6e7; font-weight: bold; text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                        dat.MakeNiceName(dvE[0]["Header"].ToString()) + "_" +
                                        dvE[0]["ID"].ToString() + "_Event" + "\\');\\\">" +
                                        dat.CleanExcelString(dvE[0]["Header"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ")) + "</div>");
                                }
                            }

                        }
                        else
                        {
                            //VenueName.Text + "@&" + 
                            if (venueMapping.ContainsKey(address))
                            {
                                venueMapping[address] += ",<br/><div style=\\\"color: #1fb6e7; font-weight: bold; text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                        dat.MakeNiceName(dvE[0]["Header"].ToString()) + "_" +
                                        dvE[0]["ID"].ToString() + "_Event" + "\\');\\\">" +
                                        dat.CleanExcelString(dvE[0]["Header"].ToString().Replace("'", 
                                        " ").Replace("(", " ").Replace(")", " ")) + "</div>";
                            }
                            else
                            {
                                countOfAllUniqueVenues++;
                                if (!isFirstElement)
                                {
                                    i++;
                                    venuesNumMapping.Add(address, dat.GetImage(i.ToString()));
                                    venueMapping.Add(address, "address =  '" + address.Trim().Replace("'", "''").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["State"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; showAddressUS('<span style=\\\"color: #98cb2a; font-weight: bold;text-decoration: underline; " +
                                        " cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                        dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                                        dvV[0]["ID"].ToString() + "_Venue" + "\\');\\\">" +
                                        dvV[0]["Name"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ") + "</span>',1, address, 0, address, 1, " + i.ToString() + ", '" +
                                        "<div style=\\\"color: #1fb6e7; font-weight: bold; text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                        dat.MakeNiceName(dvE[0]["Header"].ToString()) + "_" +
                                        dvE[0]["ID"].ToString() + "_Event" + "\\');\\\">" +
                                        dat.CleanExcelString(dvE[0]["Header"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ")) + "</div>");
                                    DataRow r = dt.NewRow();
                                    r["Address"] = address;
                                    r["Letter"] = dat.GetImage(i.ToString());
                                    dt.Rows.Add(r);
                                }
                                else
                                {
                                    DataRow r = dt.NewRow();
                                    r["Address"] = address;
                                    r["Letter"] = dat.GetImage(i.ToString());
                                    dt.Rows.Add(r);
                                    venuesNumMapping.Add(address, dat.GetImage(i.ToString()));
                                    venueMapping.Add(address, "address =  '" + address.Trim().Replace("'", "''").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["State"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                                        country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; showAddressUS('<span style=\\\"color: #98cb2a; font-weight: bold;text-decoration: underline; " +
                                        "cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                        dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                                        dvV[0]["ID"].ToString() + "_Venue" + "\\');\\\">" +
                                        dvV[0]["Name"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ") + "</span>',1, address, 0, address, 1, " + i.ToString() + ", '" +
                                        "<div style=\\\"color: #1fb6e7; font-weight: bold; text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                        dat.MakeNiceName(dvE[0]["Header"].ToString()) + "_" +
                                        dvE[0]["ID"].ToString() + "_Event" + "\\');\\\">" +
                                        dat.CleanExcelString(dvE[0]["Header"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ")) + "</div>");
                                }
                            }

                        }
                        isFirstElement = false;
                        normalRecordCount++;
                        venue = address;
                    }

                }
                else
                {
                    countOfAllUniqueVenues++;
                    bool isVenue = false;
                    address = "";


                    DataView dvE = dat.GetDataDV("SELECT * FROM GroupEvents GE, GroupEvent_Occurance GEO WHERE GE.ID=" +
                        row["EID"].ToString() + " AND GEO.GroupEventID=GE.ID AND GEO.ID=" + row["ReoccurrID"].ToString());
                    country = dat.GetDataDV("SELECT * FROM Countries C, GroupEvent_Occurance GEO WHERE GEO.ID=" +
                        row["ReoccurrID"].ToString() +
                        " AND C.country_id=GEO.Country ")[0]["country_2_code"].ToString();

                    DataView dvV = new DataView();

                    if (dvE[0]["VenueID"] != null)
                    {
                        if (dvE[0]["VenueID"].ToString().Trim() != "")
                        {
                            dvV = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + dvE[0]["VenueID"].ToString());
                            address = dat.GetAddress(dvV[0]["Address"].ToString(),
                                dvV[0]["Country"].ToString() != "223").Trim();
                            isVenue = true;
                        }
                    }

                    if (!isVenue)
                    {
                        if (dvE[0]["Country"].ToString() == "223")
                        {
                            address = dvE[0]["StreetNumber"].ToString() + " " + dvE[0]["StreetName"].ToString() +
                                " " + dvE[0]["StreetDrop"].ToString();
                        }
                        else
                        {
                            address = dvE[0]["Location"].ToString();
                        }
                    }

                    address = address.Trim() + " " + dvE[0]["City"].ToString() + " " + dvE[0]["State"].ToString() +
                        " " + dvE[0]["Zip"].ToString() + " " + country;




                    if (venueMapping.ContainsKey(address))
                    {
                        venueMapping[address] += ",<br/><div style=\\\"color: #1fb6e7; font-weight: bold; text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                dat.MakeNiceName(dvE[0]["Name"].ToString()) + "_" + row["ReoccurrID"].ToString() + "_" +
                                dvE[0]["ID"].ToString() + "_GroupEvent" + "\\');\\\">" +
                                dat.CleanExcelString(dvE[0]["Name"].ToString().Replace("'", 
                                " ").Replace("(", " ").Replace(")", " ")) + "</div>";
                    }
                    else
                    {
                        if (!isFirstElement)
                        {

                            i++;
                            if (isVenue)
                                venueMapping.Add(address, "address =  '" + address.Trim().Replace("'", "''").Replace("(",
                                    " ").Replace(")", " ") + "';showAddressUS('<span style=\\\"color: #98cb2a; font-weight: bold;text-decoration: underline; " +
                                        "cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                        dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                                        dvV[0]["ID"].ToString() + "_Venue" + "\\');\\\">" +
                                        dvV[0]["Name"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ") + "</span>',1, address, 0, address, 1, " + i.ToString() + ", '" +
                                "<div style=\\\"color: #1fb6e7; font-weight: bold; text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                dat.MakeNiceName(dvE[0]["Name"].ToString()) + "_" + row["ReoccurrID"].ToString() + "_" +
                                dvE[0]["ID"].ToString() + "_GroupEvent" + "\\');\\\">" +
                                dat.CleanExcelString(dvE[0]["Name"].ToString().Replace("'", 
                                " ").Replace("(", " ").Replace(")", " ")) + "</div>");
                            else
                                venueMapping.Add(address, "address =  '" + address.Trim().Replace("'", "''").Replace("(",
                                " ").Replace(")", " ") + "';showAddressUS('', 1, address, 0, address, 1, " + i.ToString() + ", '" +
                                "<div style=\\\"color: #1fb6e7; font-weight: bold; text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                dat.MakeNiceName(dvE[0]["Name"].ToString()) + "_" + row["ReoccurrID"].ToString() + "_" +
                                dvE[0]["ID"].ToString() + "_GroupEvent" + "\\');\\\">" +
                                dat.CleanExcelString(dvE[0]["Name"].ToString().Replace("'", 
                                " ").Replace("(", " ").Replace(")", " ")) + "</div>");
                            venuesNumMapping.Add(address, dat.GetImage(i.ToString()));
                            DataRow r = dt.NewRow();
                            r["Address"] = address;
                            r["Letter"] = dat.GetImage(i.ToString());
                            dt.Rows.Add(r);
                        }
                        else
                        {

                            if (isVenue)
                                venueMapping.Add(address, "address =  '" + address.Trim().Replace("'", "''").Replace("(",
                                    " ").Replace(")", " ") + "'; showAddressUS('<span style=\\\"color: #98cb2a; font-weight: bold;text-decoration: underline; " +
                                        "cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                        dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                                        dvV[0]["ID"].ToString() + "_Venue" + "\\');\\\">" +
                                        dvV[0]["Name"].ToString().Replace("'",
                                        " ").Replace("(", " ").Replace(")", " ") + "</span>',1, address, 0, address, 1, " + i.ToString() + ", '" +
                                "<div style=\\\"color: #1fb6e7; font-weight: bold; text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                dat.MakeNiceName(dvE[0]["Name"].ToString()) + "_" + row["ReoccurrID"].ToString() + "_" +
                                dvE[0]["ID"].ToString() + "_GroupEvent" + "\\');\\\">" +
                                dat.CleanExcelString(dvE[0]["Name"].ToString().Replace("'", 
                                " ").Replace("(", " ").Replace(")", " ")) + "</div>");
                            else
                                venueMapping.Add(address, "address =  '" + address.Trim().Replace("'", "''").Replace("(",
                                " ").Replace(")", " ") + "';showAddressUS('', 1, address, 0, address, 1, " + i.ToString() + ", '" +
                                "<div style=\\\"color: #1fb6e7; font-weight: bold; text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                                dat.MakeNiceName(dvE[0]["Name"].ToString()) + "_" + row["ReoccurrID"].ToString() + "_" +
                                dvE[0]["ID"].ToString() + "_GroupEvent" + "\\');\\\">" +
                                dat.CleanExcelString(dvE[0]["Name"].ToString().Replace("'", 
                                " ").Replace("(", " ").Replace(")", " ")) + "</div>");

                            venuesNumMapping.Add(address, dat.GetImage(i.ToString()));
                            DataRow r = dt.NewRow();
                            r["Address"] = address;
                            r["Letter"] = dat.GetImage(i.ToString());
                            dt.Rows.Add(r);
                        }
                    }

                    isFirstElement = false;
                    normalRecordCount++;
                    venue = address;
                }
            }

                dvRecords = new DataView(dt, "", "Letter ASC", DataViewRowState.CurrentRows);
                oneStringRecord = theLiteral;
                int theCount = 0;
                foreach (DataRowView rowster in dvRecords)
                {
                    if (theCount == 0)
                    {
                        oneStringRecord += venueMapping[rowster["Address"].ToString()].ToString() +
                        "', false);".Replace("   ",
                        " ").Replace("  ", " ");

                        if (theCount == dvRecords.Count - 1)
                        {
                            oneStringRecord += " }}</script>".Replace("   ",
                                                        " ").Replace("  ", " ");
                        }
                    }
                    else if (theCount == dvRecords.Count - 1)
                    {
                        oneStringRecord += venueMapping[rowster["Address"].ToString()].ToString() +
                                                    "', false); }}</script>".Replace("   ",
                                                    " ").Replace("  ", " ");
                    }
                    else
                    {
                        oneStringRecord += venueMapping[rowster["Address"].ToString()].ToString() +
                                                    "', false);".Replace("   ",
                                                    " ").Replace("  ", " ");
                    }
                    theCount++;
                }

        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString();
        }

        ScriptLiteral.Text = oneStringRecord;
        return countOfAllUniqueVenues;
    }

    protected void GetAllVenuesLiteral(DataView dvEvents, Data dat)
    {
        string theLiteral = "<script type=\"text/javascript\">function initializeAll(){" +

           " if(map != null && map != undefined) " +
           " { " +
           "     var address;";
        string address = "";
        string country = "US";

        foreach (DataRowView row in dvEvents)
        {
            address = "";
            DataView dvV = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + row["VID"].ToString());

            DataView dvCountry = dat.GetDataDV("SELECT * FROM countries WHERE country_id=" +
                dvV[0]["Country"].ToString());
            country = dvCountry[0]["country_2_code"].ToString();

            if (country.ToLower() == "us")
            {
                try
                {
                    address = dat.GetAddress(dvV[0]["Address"].ToString(), false);
                }
                catch (Exception ex1)
                {
                    address = "";
                }
            }
            else
            {
                address = dat.GetAddress(dvV[0]["Address"].ToString(), true);
            }

            if (dvV[0]["Country"].ToString().ToLower() == "uk")
            {
                theLiteral += "address =  '" + address.Replace("'", "''").Replace("(", " ").Replace(")", " ") +
                    dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                    dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                    country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; " +
                    "showAddressUS('',1, address, 0, address, 1, 0, '" +
                "<div style=\\\"color: #98cb2a;font-weight: bold;text-decoration: underline; float: left; cursor: pointer;\\\" " +
                "onclick=\\\"CloseWindow(\\'" + "../" +
                dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                dvV[0]["ID"].ToString() + "_Venue" + "\\');\\\">" +
                dvV[0]["Name"].ToString().Replace("'", " ").Replace("(",
                " ").Replace(")", " ") + "</div>', false);";
            }
            else
            {
                theLiteral += "address =  '" + address +
                    dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                    dvV[0]["State"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                    dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                    country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; " +
                    "showAddressUS('',1, address, 0, address, 1, 0, '" +
                    "<div style=\\\"color: #98cb2a;font-weight: bold;text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                    dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                    dvV[0]["ID"].ToString() + "_Venue" + "\\');\\\">" +
                    dvV[0]["Name"].ToString().Replace("'", " ").Replace("(",
                    " ").Replace(")", " ") + "</div>', false);";
            }
        }

        ScriptLiteral.Text += theLiteral + "}}</script>";
    }

    //protected void GetEventLiteral(ref DataView dvEvents, Data dat)
    //{

    //    Hashtable normalEventHash = new Hashtable();

    //    int i = 0;
    //    string theLiteral = "<script type=\"text/javascript\">function initialize(){if (GBrowserIsCompatible()){ " +
    //       " map = new GMap2(document.getElementById(\"map_canvas\")); " +
    //       " map.addControl(new GMapTypeControl()); " +

    //       " if(map != null && map != undefined) " +
    //       " { " +
    //       "     map.setUIToDefault(); " +
    //       "    geocoder = new GClientGeocoder(); var address;";
    //    string address = "";
    //    string venue = "";
    //    bool isVenueSame = false;
    //    bool gotEntry = false;
    //    string nameStr = "";
    //    bool isFirstElement = true;
    //    string country = "US";
    //    dvEvents.Sort = " VID ASC";
    //    foreach (DataRowView row in dvEvents)
    //    {
    //        if (!bool.Parse(row["isGroup"].ToString()))
    //        {
    //            if (!normalEventHash.Contains(row["EID"].ToString()))
    //            {
    //                normalEventHash.Add(row["EID"].ToString(), "");
    //                gotEntry = false;
    //                address = "";
    //                DataView dvE = dat.GetDataDV("SELECT * FROM Events WHERE ID=" + row["EID"].ToString());
    //                DataView dvV = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + dvE[0]["Venue"].ToString());

    //                if (venue == dvE[0]["Venue"].ToString())
    //                {
    //                    isVenueSame = true;
    //                }
    //                else
    //                {
    //                    isVenueSame = false;
    //                }

    //                venue = dvE[0]["Venue"].ToString();

    //                DataView dvCountry = dat.GetDataDV("SELECT * FROM countries WHERE country_id=" + dvE[0]["Country"].ToString());
    //                country = dvCountry[0]["country_2_code"].ToString();

    //                if (country.ToLower() == "us")
    //                {
    //                    try
    //                    {
    //                        address = dat.GetAddress(dvV[0]["Address"].ToString(), false);
    //                    }
    //                    catch (Exception ex1)
    //                    {
    //                        address = "";
    //                    }
    //                }
    //                else
    //                {
    //                    address = dat.GetAddress(dvV[0]["Address"].ToString(), true);
    //                }


    //                if (dvE[0]["Country"].ToString().ToLower() == "uk")
    //                {
    //                    //VenueName.Text + "@&" + 
    //                    if (isVenueSame)
    //                    {
    //                        row["SearchNum"] = dat.GetImage(i.ToString());
    //                        nameStr += ",<br/><div style=\\\"text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
    //                                dat.MakeNiceName(dvE[0]["Header"].ToString()) + "_" +
    //                                dvE[0]["ID"].ToString() + "_Event" + "\\');\\\">" +
    //                                dvE[0]["Header"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + "</div>";
    //                    }
    //                    else
    //                    {
    //                        if (!isFirstElement)
    //                        {
    //                            theLiteral += nameStr + "', false); ";
    //                            theLiteral += "address =  '" + dvV[0]["Name"].ToString().Replace("'", " ").Replace("(",
    //                                " ").Replace(")", " ") + " " + address.Replace("'", "''").Replace("(", " ").Replace(")", " ") +
    //                                dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
    //                                dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
    //                                country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; ";

    //                            nameStr = "";
    //                            i++;
    //                            nameStr = " showAddressUS(address, 0, address, 1, " + i.ToString() + ", '" +
    //                                "<div style=\\\"text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
    //                                dat.MakeNiceName(dvE[0]["Header"].ToString()) + "_" +
    //                                dvE[0]["ID"].ToString() + "_Event" + "\\');\\\">" +
    //                                dvE[0]["Header"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + "</div>";
    //                            row["SearchNum"] = dat.GetImage(i.ToString());
    //                        }
    //                        else
    //                        {
    //                            row["SearchNum"] = dat.GetImage(i.ToString());
    //                            theLiteral += "address =  '" + dvV[0]["Name"].ToString().Replace("'", " ").Replace("(",
    //                                " ").Replace(")", " ") +
    //                                " " + address.Replace("'", "''").Replace("(", " ").Replace(")", " ") +
    //                                dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
    //                                dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
    //                                country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; ";

    //                            nameStr = " showAddressUS(address, 0, address, 1, " + i.ToString() + ", '" +
    //                                "<div style=\\\"text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
    //                                dat.MakeNiceName(dvE[0]["Header"].ToString()) + "_" +
    //                                dvE[0]["ID"].ToString() + "_Event" + "\\');\\\">" +
    //                                dvE[0]["Header"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + "</div>";
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    if (isVenueSame)
    //                    {
    //                        row["SearchNum"] = dat.GetImage(i.ToString());
    //                        nameStr += ",<br/><div style=\\\"text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
    //                                dat.MakeNiceName(dvE[0]["Header"].ToString()) + "_" +
    //                                dvE[0]["ID"].ToString() + "_Event" + "\\');\\\">" +
    //                                dvE[0]["Header"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + "</div>";
    //                    }
    //                    else
    //                    {
    //                        if (!isFirstElement)
    //                        {
    //                            theLiteral += nameStr + "', false); ";
    //                            theLiteral += "address =  '" + address.Trim() +" " +
    //                                dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
    //                                dvV[0]["State"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
    //                                dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
    //                                country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; ";

    //                            nameStr = "";
    //                            i++;
    //                            nameStr = " showAddressUS(address, 0, address, 1, " + i.ToString() + ", '" +
    //                                "<div style=\\\"text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
    //                                dat.MakeNiceName(dvE[0]["Header"].ToString()) + "_" +
    //                                dvE[0]["ID"].ToString() + "_Event" + "\\');\\\">" +
    //                                dvE[0]["Header"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + "</div>";
    //                            row["SearchNum"] = dat.GetImage(i.ToString());
    //                        }
    //                        else
    //                        {
    //                            row["SearchNum"] = dat.GetImage(i.ToString());
    //                            theLiteral += "address =  '" + address.Trim() +" "+
    //                                dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
    //                                dvV[0]["State"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
    //                                dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
    //                                country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; ";

    //                            nameStr = " showAddressUS(address, 0, address, 1, " + i.ToString() + ", '" +
    //                                "<div style=\\\"text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
    //                                dat.MakeNiceName(dvE[0]["Header"].ToString()) + "_" +
    //                                dvE[0]["ID"].ToString() + "_Event" + "\\');\\\">" +
    //                                dvE[0]["Header"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + "</div>";
    //                        }
    //                    }
    //                }
    //                isFirstElement = false;

    //            }
    //        }
    //        else
    //        {
    //            bool isVenue = false;
    //            address = "";

    //            DataView dvE = dat.GetDataDV("SELECT * FROM GroupEvents GE, GroupEvent_Occurance GEO WHERE GE.ID=" +
    //                row["EID"].ToString() + " AND GEO.GroupEventID=GE.ID AND GEO.ID=" + row["ReoccurrID"].ToString());
    //            country = dat.GetDataDV("SELECT * FROM Countries C, GroupEvent_Occurance GEO WHERE GEO.ID=" +
    //                row["ReoccurrID"].ToString() +
    //                " AND C.country_id=GEO.Country ")[0]["country_2_code"].ToString();

    //            DataView dvV = new DataView();

    //            if (dvE[0]["VenueID"] != null)
    //            {
    //                if (dvE[0]["VenueID"].ToString().Trim() != "")
    //                {
    //                    dvV = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + dvE[0]["VenueID"].ToString());
    //                    address = dat.GetAddress(dvV[0]["Address"].ToString(),
    //                        dvV[0]["Country"].ToString() != "223");
    //                    isVenue = true;
    //                }

    //            }

    //            if (!isVenue)
    //            {
    //                if (dvE[0]["Country"].ToString() == "223")
    //                {
    //                    address = dvE[0]["StreetNumber"].ToString() + " " + dvE[0]["StreetName"].ToString() +
    //                        " " + dvE[0]["StreetDrop"].ToString();
    //                }
    //                else
    //                {
    //                    address = dvE[0]["Location"].ToString();
    //                }
    //            }

    //            address += " " + dvE[0]["City"].ToString() + " " + dvE[0]["State"].ToString() +
    //                " " + dvE[0]["Zip"].ToString() + " " + country;

    //            if (venue == address)
    //            {
    //                isVenueSame = true;
    //            }
    //            else
    //            {
    //                isVenueSame = false;
    //            }

    //            venue = address;

    //            if (isVenueSame)
    //            {
    //                row["SearchNum"] = dat.GetImage(i.ToString());
    //                nameStr += ",<br/><div style=\\\"text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
    //                        dat.MakeNiceName(dvE[0]["Name"].ToString()) + "_" + row["ReoccurrID"].ToString() + "_" +
    //                        dvE[0]["ID"].ToString() + "_GroupEvent" + "\\');\\\">" +
    //                        dvE[0]["Name"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + "</div>";
    //            }
    //            else
    //            {
    //                if (!isFirstElement)
    //                {
    //                    theLiteral += nameStr + "', false); ";
    //                    if (isVenue)
    //                        theLiteral += "address =  '" + dvV[0]["Name"].ToString().Replace("'", " ").Replace("(",
    //                            " ").Replace(")", " ") + " " + address.Replace("'", "''").Replace("(",
    //                            " ").Replace(")", " ") + "'; ";
    //                    else
    //                        theLiteral += "address =  '" + address.Replace("'", "''").Replace("(",
    //                        " ").Replace(")", " ") + "'; ";

    //                    nameStr = "";
    //                    i++;
    //                    nameStr = " showAddressUS(address, 0, address, 1, " + i.ToString() + ", '" +
    //                        "<div style=\\\"text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
    //                        dat.MakeNiceName(dvE[0]["Name"].ToString()) + "_" + row["ReoccurrID"].ToString() + "_" +
    //                        dvE[0]["ID"].ToString() + "_GroupEvent" + "\\');\\\">" +
    //                        dvE[0]["Name"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + "</div>";
    //                    row["SearchNum"] = dat.GetImage(i.ToString());
    //                }
    //                else
    //                {
    //                    row["SearchNum"] = dat.GetImage(i.ToString());
    //                    if (isVenue)
    //                        theLiteral += "address =  '" + dvV[0]["Name"].ToString().Replace("'", " ").Replace("(",
    //                            " ").Replace(")", " ") + " " + address.Replace("'", "''").Replace("(",
    //                            " ").Replace(")", " ") + "'; ";
    //                    else
    //                        theLiteral += "address =  '" + address.Replace("'", "''").Replace("(",
    //                        " ").Replace(")", " ") + "'; ";

    //                    nameStr = " showAddressUS(address, 0, address, 1, " + i.ToString() + ", '" +
    //                        "<div style=\\\"text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
    //                        dat.MakeNiceName(dvE[0]["Name"].ToString()) + "_" + row["ReoccurrID"].ToString() + "_" +
    //                        dvE[0]["ID"].ToString() + "_GroupEvent" + "\\');\\\">" +
    //                        dvE[0]["Name"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + "</div>";
    //                }
    //            }
    //            isFirstElement = false;

    //        }
    //    }
    //    theLiteral += nameStr + "', true)";
    //    theLiteral = theLiteral.Replace("   ", " ");
    //    theLiteral = theLiteral.Replace("  ", " ");
    //    ScriptLiteral.Text = theLiteral + "}}}</script>";
    //}

    protected string[] GetVenuesLiteral(DataView dvEvents, Data dat, int numOfRecordsPerPage)
    {
        int i = 0;
        
        string address = "";
        string lastAddress = "";
        string nameStr = "";
        string country = "US";
        dvEvents.Sort = " Address DESC ";
        bool isAddressSame = false;
        bool isFirstElement = true;

        int thecount = dvEvents.Count;
        int numberInArray = thecount / numOfRecordsPerPage;

        if (thecount % numOfRecordsPerPage != 0)
            numberInArray++;
        string oneStringRecord = "";

        int normalRecordCount = 0;
        int countInArray = 0;
        string theLiteral = "<script type=\"text/javascript\">function initialize" +
                (countInArray + 1).ToString() + "(){"+
               " if(map != null && map != undefined) " +
               " { " +
               "     map.setUIToDefault(); " +
               "    geocoder = new GClientGeocoder(); var address;";
        string[] theArray = new string[numberInArray];
        int lastUsedCount = 0;
        Hashtable venueMapping = new Hashtable();
        Hashtable venuesNumMapping = new Hashtable();
        DataView dvRecords = new DataView();
        DataTable dt = new DataTable();
        DataColumn dc2 = new DataColumn("Address");
        dt.Columns.Add(dc2);
        dc2 = new DataColumn("Letter");
        dt.Columns.Add(dc2);

        string addToLit = "";
        string addToLitBottom = "";
        foreach (DataRowView row in dvEvents)
        {
            if (normalRecordCount % numOfRecordsPerPage == 0)
            {
                if (normalRecordCount != 0 && normalRecordCount != lastUsedCount)
                {
                    lastUsedCount = normalRecordCount;
                    //ErrorLabel.Text += "on script insert normal rec count: " + normalRecordCount.ToString();
                    dvRecords = new DataView(dt, "", "Letter ASC", DataViewRowState.CurrentRows);
                    //if (countInArray == 0)
                    //{
                    //    addToLit = "if (GBrowserIsCompatible()){ " +
                    //       " map = new GMap2(document.getElementById(\"map_canvas\")); " +
                    //       " map.addControl(new GMapTypeControl()); " +

                    //       " if(map != null && map != undefined) " +
                    //       " { " +
                    //       "     map.setUIToDefault(); " +
                    //       "    geocoder = new GClientGeocoder(); ";
                    //    addToLitBottom = "}}}";
                    //}
                    //else
                    //{
                    //    addToLit = " if(map != null && map != undefined) " +
                    //        " {";
                    //    addToLitBottom = "}}";
                    //}
                    addToLit = " if(map != null && map != undefined) " +
                            " {";
                    addToLitBottom = "}}";
                    theLiteral = "<script type=\"text/javascript\">function initialize" +
                (countInArray + 1).ToString() + "(){" + addToLit + "var address;";
                    oneStringRecord = theLiteral;
                    int rowCount = 0;
                    foreach (DataRowView rowster in dvRecords)
                    {
                        if (rowCount == 0)
                        {
                            oneStringRecord += venueMapping[rowster["Address"].ToString()].ToString() +
                            "', true);".Replace("   ",
                            " ").Replace("  ", " ");
                            if (rowCount == dvRecords.Count - 1)
                            {
                                oneStringRecord += addToLitBottom+"</script>".Replace("   ",
                                                            " ").Replace("  ", " ");
                            }
                        }
                        else if (rowCount == dvRecords.Count - 1)
                        {
                            oneStringRecord += venueMapping[rowster["Address"].ToString()].ToString() +
                                                        "', false); " + addToLitBottom + "</script>".Replace("   ",
                                                        " ").Replace("  ", " ");
                        }
                        else
                        {
                            oneStringRecord += venueMapping[rowster["Address"].ToString()].ToString() +
                                                        "', false);".Replace("   ",
                                                        " ").Replace("  ", " ");
                        }
                        rowCount++;
                    }
                    TopLiteral.Text += oneStringRecord;

                    theArray[countInArray] = oneStringRecord;
                    dt.Rows.Clear();
                    countInArray++;
                    venueMapping.Clear();
                    venuesNumMapping.Clear();
                }

                lastAddress = "";
                isFirstElement = true;
                i = 0;
            }
            isAddressSame = false;
            address = "";
            DataView dvV = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + row["VID"].ToString());

            DataView dvCountry = dat.GetDataDV("SELECT * FROM countries WHERE country_id=" +
                dvV[0]["Country"].ToString());
            country = dvCountry[0]["country_2_code"].ToString();

            if (country.ToLower() == "us")
            {
                try
                {
                    address = dat.GetAddress(dvV[0]["Address"].ToString(), false);
                }
                catch (Exception ex1)
                {
                    address = "";
                }
            }
            else
            {
                address = dat.GetAddress(dvV[0]["Address"].ToString(), true);
            }


            if (lastAddress == address && !isFirstElement)
                isAddressSame = true;
            else
                isAddressSame = false;

            if (dvV[0]["Country"].ToString().ToLower() == "uk")
            {
                if (venueMapping.ContainsKey(address))
                {
                    row["SearchNum"] = venuesNumMapping[address].ToString();
                    venueMapping[address] += ",<br/><div style=\\\"color: #98cb2a;font-weight: bold;text-decoration: underline; float: left; cursor: pointer;\\\" " +
                        "onclick=\\\"CloseWindow(\\'" + "../" +
                        dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                        dvV[0]["ID"].ToString() + "_Venue" + "\\');\\\">" +
                        dvV[0]["Name"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + "</div>";
                }
                else
                {
                    if (!isFirstElement)
                    {
                        i++;
                        venuesNumMapping.Add(address, dat.GetImage(i.ToString()));
                        venueMapping.Add(address, "address =  '" + address.Replace("'", "''").Replace("(", " ").Replace(")", " ") +
                            dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                            dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                            country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; "+
                            "showAddressUS('',0, address, 0, address, 1, " + i.ToString() + ", '" +
                        "<div style=\\\"color: #98cb2a;font-weight: bold;text-decoration: underline; float: left; cursor: pointer;\\\" " +
                        "onclick=\\\"CloseWindow(\\'" + "../" +
                        dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                        dvV[0]["ID"].ToString() + "_Venue" + "\\');\\\">" +
                        dvV[0]["Name"].ToString().Replace("'", " ").Replace("(", 
                        " ").Replace(")", " ") + "</div> ");
                        row["SearchNum"] = dat.GetImage(i.ToString());
                        DataRow r = dt.NewRow();
                        r["Address"] = address;
                        r["Letter"] = dat.GetImage(i.ToString());
                        dt.Rows.Add(r);
                    }
                    else
                    {
                        DataRow r = dt.NewRow();
                        r["Address"] = address;
                        r["Letter"] = dat.GetImage(i.ToString());
                        dt.Rows.Add(r);
                        row["SearchNum"] = dat.GetImage(i.ToString());
                        venuesNumMapping.Add(address, dat.GetImage(i.ToString()));
                        venueMapping.Add(address, "address =  '" + address.Replace("'", "''").Replace("(", " ").Replace(")", " ") +
                            dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                            dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + ", " +
                            country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; "+
                            " showAddressUS('',0, address, 0, address, 1, " + i.ToString() + ", '" +
                        "<div style=\\\"color: #98cb2a;font-weight: bold;text-decoration: underline; float: left; cursor: pointer;\\\" " +
                        "onclick=\\\"CloseWindow(\\'" + "../" +
                        dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                        dvV[0]["ID"].ToString() + "_Venue" + "\\');\\\">" +
                        dvV[0]["Name"].ToString().Replace("'", " ").Replace("(", 
                        " ").Replace(")", " ") + "</div> ");
                    }                        
                }
            }
            else
            {
                if (venueMapping.ContainsKey(address))
                {
                    row["SearchNum"] = venuesNumMapping[address].ToString();
                    venueMapping[address] += ",<br/><div style=\\\"color: #98cb2a;font-weight: bold;text-decoration: underline; float: left; cursor: pointer;\\\" " +
                        "onclick=\\\"CloseWindow(\\'" + "../" +
                        dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                        dvV[0]["ID"].ToString() + "_Venue" + "\\');\\\">" +
                        dvV[0]["Name"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + "</div>";
                }
                else
                {
                    if (!isFirstElement)
                    {
                        i++;
                        venuesNumMapping.Add(address, dat.GetImage(i.ToString()));
                        venueMapping.Add(address, "address =  '" + address +
                            dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                            dvV[0]["State"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                            dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                            country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; "+
                            "showAddressUS('',0, address, 0, address, 1, " + i.ToString() + ", '" +
                            "<div style=\\\"color: #98cb2a;font-weight: bold;text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                            dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                            dvV[0]["ID"].ToString() + "_Venue" + "\\');\\\">" +
                            dvV[0]["Name"].ToString().Replace("'", " ").Replace("(", 
                            " ").Replace(")", " ") + "</div> ");
                        DataRow r = dt.NewRow();
                        r["Address"] = address;
                        r["Letter"] = dat.GetImage(i.ToString());
                        dt.Rows.Add(r);
                        row["SearchNum"] = dat.GetImage(i.ToString());
                    }
                    else
                    {
                        row["SearchNum"] = dat.GetImage(i.ToString());
                        DataRow r = dt.NewRow();
                        r["Address"] = address;
                        r["Letter"] = dat.GetImage(i.ToString());
                        dt.Rows.Add(r);
                        venuesNumMapping.Add(address, dat.GetImage(i.ToString()));
                        venueMapping.Add(address, "address =  '" + address +
                            dvV[0]["City"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                            dvV[0]["State"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                            dvV[0]["Zip"].ToString().Replace("'", " ").Replace("(", " ").Replace(")", " ") + " " +
                            country.Replace("'", " ").Replace("(", " ").Replace(")", " ") + "'; " +
                            "showAddressUS('',0, address, 0, address, 1, " + i.ToString() + ", '" +
                            "<div style=\\\"color: #98cb2a;font-weight: bold;text-decoration: underline; float: left; cursor: pointer;\\\" onclick=\\\"CloseWindow(\\'" + "../" +
                            dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                            dvV[0]["ID"].ToString() + "_Venue" + "\\');\\\">" +
                            dvV[0]["Name"].ToString().Replace("'", " ").Replace("(",
                            " ").Replace(")", " ") + "</div> ");
                    }
                }

            }
            lastAddress = address;
            isFirstElement = false;
            normalRecordCount++;
        }
        //if (thecount % numOfRecordsPerPage != 0)
        //{
            dvRecords = new DataView(dt, "", "Letter ASC", DataViewRowState.CurrentRows);
            //if (countInArray == 0)
            //{
            //    addToLit = "if (GBrowserIsCompatible()){ " +
            //       " map = new GMap2(document.getElementById(\"map_canvas\")); " +
            //       " map.addControl(new GMapTypeControl()); " +

            //       " if(map != null && map != undefined) " +
            //       " { " +
            //       "     map.setUIToDefault(); " +
            //       "    geocoder = new GClientGeocoder(); ";
            //    addToLitBottom = "}}}";
            //}
            //else
            //{
            //    addToLit = " if(map != null && map != undefined) " +
            //        " {";
            //    addToLitBottom = "}}";
            //}
            addToLit = " if(map != null && map != undefined) " +
                        " {";
            addToLitBottom = "}}";
            theLiteral = "<script type=\"text/javascript\">function initialize" +
        (countInArray + 1).ToString() + "(){" + addToLit + "var address;";
            oneStringRecord = theLiteral;
            int theCount = 0;
            foreach (DataRowView rowster in dvRecords)
            {
                if (theCount == 0)
                {
                    oneStringRecord += venueMapping[rowster["Address"].ToString()].ToString() +
                    "', true);".Replace("   ",
                    " ").Replace("  ", " ");
                    if (theCount == dvRecords.Count - 1)
                    {
                        oneStringRecord += addToLitBottom+"</script>".Replace("   ",
                                                    " ").Replace("  ", " ");
                    }
                }
                else if (theCount == dvRecords.Count - 1)
                {
                    oneStringRecord += venueMapping[rowster["Address"].ToString()].ToString() +
                                                "', false); " + addToLitBottom + "</script>".Replace("   ",
                                                " ").Replace("  ", " ");
                }
                else
                {
                    oneStringRecord += venueMapping[rowster["Address"].ToString()].ToString() +
                                                "', false);".Replace("   ",
                                                " ").Replace("  ", " ");
                }
                theCount++;
            }
            TopLiteral.Text += oneStringRecord;

            theArray[countInArray] = oneStringRecord;
        //}
        return theArray;
    }

    protected void SortResults(object sender, EventArgs e)
    {
        try
        {
            if (Session["SearchDS"] != null)
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
                DataColumn dc = new DataColumn("SearchNum");
                DataSet ds;
                ds = (DataSet)Session["SearchDS"];
                if (!ds.Tables[0].Columns.Contains("SearchNum"))
                    ds.Tables[0].Columns.Add(dc);
                DataView dv = new DataView(ds.Tables[0], "", "", DataViewRowState.CurrentRows);

                if (SortDropDown.SelectedValue != "-1")
                {
                    Session["sortString"] = SortDropDown.SelectedValue;
                    //Session["sortString"] = SortDropDown.SelectedValue;
                    //DataSet ds;

                    //ds = (DataSet)Session["SearchDS"];
                    //EventSearchElements.EVENTS_DS = ds;
                    //EventSearchElements.IS_WINDOW = true;
                    //EventSearchElements.SORT_STR = Session["sortString"].ToString();
                    //EventSearchElements.DataBind2();
                    dv.Sort = SortDropDown.SelectedValue;
                }
                else
                {
                    Session["sortString"] = null;
                    dv.Sort = "";
                }

                int numofpages = 10;
                string[] mapStrings = GetEventLiteral(dv, dat, numofpages);

                DataSet dsNew = new DataSet();
                dsNew.Tables.Add(dv.ToTable());

                ds = dsNew;

                DoCalendar(dv);

                EventSearchElements.Visible = true;
                EventSearchElements.EVENTS_DS = ds;
                EventSearchElements.IS_WINDOW = true;
                EventSearchElements.DO_MAP = true;
                EventSearchElements.MAP_STRINGS = mapStrings;
                EventSearchElements.NUM_OF_PAGES = numofpages;
                if (Session["sortString"] != null)
                    EventSearchElements.SORT_STR = Session["sortString"].ToString();
                EventSearchElements.DataBind2();
            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString();
        }
    }

    protected void AdvanceMonth(object sender, EventArgs e)
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

        RadScheduler1.SelectedDate = RadScheduler1.SelectedDate.AddMonths(1);
        MonthLabel.Text = dat.GetMonth(RadScheduler1.SelectedDate.Month.ToString()) + " " + RadScheduler1.SelectedDate.Year.ToString();
    }

    protected void DevanceMonth(object sender, EventArgs e)
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

        RadScheduler1.SelectedDate = RadScheduler1.SelectedDate.AddMonths(-1);
        MonthLabel.Text = dat.GetMonth(RadScheduler1.SelectedDate.Month.ToString()) + " " + RadScheduler1.SelectedDate.Year.ToString();
    }

    protected void ProgressCalendar(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        string parameter = Request.Params["__EVENTARGUMENT"];
        char[] delim = { ';' };
        string[] parmtrs = parameter.Split(delim);
        Session["SelectToolTip"] = parmtrs[1];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        
        RadScheduler1.SelectedDate = DateTime.Parse(parmtrs[0]);
        MonthLabel.Text = dat.GetMonth(RadScheduler1.SelectedDate.Month.ToString()) + " " + RadScheduler1.SelectedDate.Year.ToString();
    }
}
