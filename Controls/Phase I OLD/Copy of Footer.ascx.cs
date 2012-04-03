

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

public partial class Controls_Footer : System.Web.UI.UserControl
{
    public int CUT_OFF
    {
        get { return cutOff; }
        set { cutOff = value; }
    }
    public int EVENT_ID
    {
        get { return eventID; }
        set { eventID = value; }
    }
    public string PAGE_TYPE
    {
        get { return pageType; }
        set { pageType = value; }
    }
    private int cutOff;
    private int eventID;
    private string pageType;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            switch (pageType)
            {
                case "Main":

                    GetMainEvents();

                    break;
                default: break;
            }
        }
    }
    private void GetMainEvents()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data d = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        int rowCount = 0;

        try
        {

            if (Session["User"] != null)
            {
                EventsPanel.Controls.Clear();

                DataSet ds = d.GetEventsInLocation(true);

                if (ds.Tables.Count > 0)
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        rowCount = ds.Tables[0].Rows.Count;
                        if (rowCount > cutOff)
                            rowCount = cutOff;
                        for (int i = 0; i < rowCount; i++)
                        {
                            ASP.controls_otherevent_ascx anEvent = new ASP.controls_otherevent_ascx();
                            anEvent.EVENT_ID = ds.Tables[0].Rows[i]["ID"].ToString();
                            anEvent.TITLE = ds.Tables[0].Rows[i]["Header"].ToString();
                            anEvent.SUMMARY = ds.Tables[0].Rows[i]["ShortDescription"].ToString();
                            anEvent.PRESENTED_BY = d.GetDataDV("SELECT DateTimeStart FROM Event_Occurance WHERE EventID=" + 
                                ds.Tables[0].Rows[i]["ID"].ToString())[0]["DateTimeStart"].ToString();
                            EventsPanel.Controls.Add(anEvent);
                            //goto NotMuchElse;

                            d.Execute("INSERT INTO Events_Seen_By_User (eventID, userID, Date, SessionID) " +
                                " VALUES(" + ds.Tables[0].Rows[i]["ID"].ToString() + ", " + Session["User"].ToString() + ", " +
                                " '" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).ToString() +
                                "', '" + Session["UserSession" + Session["User"].ToString()].ToString() + "')");
                        }

                    }
            }
            else
            {
                DataView dvLocation = d.GetDataDV("SELECT * FROM Users WHERE IPs LIKE '&;" + d.GetIP() + ";%'");

                if(dvLocation.Count == 0)
                    dvLocation =  d.GetDataDV("SELECT * FROM SearchIPs WHERE IP = '" + d.GetIP() + "'");
                DataSet ds = new DataSet();
                bool getUS = false;
                if (Session["GenericEventSession"] == null)
                {
                    Random rand = new Random(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).ToUniversalTime().Millisecond);
                    Session["GenericEventSession"] = rand.Next();
                }


                if (dvLocation.Count > 0)
                {
                    string country = "";
                    string state = "";
                    string city = "";
                    if (dvLocation[0]["Country"].ToString() != "")
                        country = " AND E.Country = " + dvLocation[0]["Country"].ToString();
                    if (dvLocation[0]["State"].ToString() != "")
                        state = " AND E.State = @p0 ";
                    if (dvLocation[0]["City"].ToString() != "")
                        city = " AND E.City = @p1 ";

                    DataSet dsSeenAds = d.GetData("SELECT * FROM Events_Seen_Generic WHERE IP='" +
                        d.GetIP() + "' AND SessionID='" +
                        Session["GenericEventSession"].ToString() + "'");
                    string notTheseAds = "";

                    if (dsSeenAds.Tables.Count > 0)
                        if (dsSeenAds.Tables[0].Rows.Count > 0)
                        {
                            for (int j = 0; j < dsSeenAds.Tables[0].Rows.Count; j++)
                            {
                                if (notTheseAds != "")
                                    notTheseAds += " AND ";
                                notTheseAds += " E.ID <> " + dsSeenAds.Tables[0].Rows[j]["eventID"].ToString();


                            }
                        }
                    if (notTheseAds != "")
                        notTheseAds = " AND ( " + notTheseAds + " ) ";



                    bool couldGetMore = false;

                    if (state != "")
                    {
                        if (city != "")
                        {
                            SqlDbType[] types = { SqlDbType.NVarChar, SqlDbType.NVarChar };
                            object[] data = { dvLocation[0]["State"].ToString(), dvLocation[0]["City"].ToString() };
                            ds = d.GetDataWithParemeters("SELECT  DISTINCT EO.EventID, E.Header, E.Content, E.SponsorPresenter, EO.DateTimeStart FROM Events E, Event_Occurance EO WHERE EO.DateTimeStart >= GETDATE() AND  E.ID=EO.EventID " + notTheseAds + country + state + city, types, data);
                        }
                        else
                        {
                            SqlDbType[] types = { SqlDbType.NVarChar };
                            object[] data = { dvLocation[0]["State"].ToString() };
                            ds = d.GetDataWithParemeters("SELECT  DISTINCT EO.EventID, E.Header, E.Content, E.SponsorPresenter, EO.DateTimeStart FROM Events E, Event_Occurance EO WHERE EO.DateTimeStart >= GETDATE() AND  E.ID=EO.EventID " + notTheseAds + country + state, types, data);
                        }
                    }
                    else
                        ds = d.GetData("SELECT  DISTINCT EO.EventID, E.Header, E.Content, E.SponsorPresenter, EO.DateTimeStart FROM Events E, Event_Occurance EO WHERE EO.DateTimeStart >= GETDATE() AND  E.ID=EO.EventID " + notTheseAds + country);


                    EventsPanel.Controls.Clear();
                    if (ds.Tables.Count > 0)
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                        }
                        else
                        {
                            couldGetMore = true;
                        }
                    else
                    {
                        couldGetMore = true;
                    }

                    if (couldGetMore)
                    {
                        Random rand = new Random(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).ToUniversalTime().Millisecond);
                        d.Execute("DELETE FROM Events_Seen_Generic WHERE SessionID='" +
                            Session["GenericEventSession"].ToString() + "' AND IP='" + d.GetIP() + "'");

                        Session["GenericEventSession"] = rand.Next();
                        if (state != "")
                        {
                            if (city != "")
                            {
                                SqlDbType[] types = { SqlDbType.NVarChar, SqlDbType.NVarChar };
                                object[] data = { dvLocation[0]["State"].ToString(), dvLocation[0]["City"].ToString() };
                                ds = d.GetDataWithParemeters("SELECT  DISTINCT EO.EventID, E.Header, E.Content, E.SponsorPresenter, EO.DateTimeStart FROM Events E, Event_Occurance EO WHERE EO.DateTimeStart >= GETDATE() AND  E.ID=EO.EventID " + country + state + city, types, data);
                            }
                            else
                            {
                                SqlDbType[] types = { SqlDbType.NVarChar };
                                object[] data = { dvLocation[0]["State"].ToString() };
                                ds = d.GetDataWithParemeters("SELECT  DISTINCT EO.EventID, E.Header, E.Content, E.SponsorPresenter, EO.DateTimeStart FROM Events E, Event_Occurance EO WHERE EO.DateTimeStart >= GETDATE() AND  E.ID=EO.EventID " + country + state, types, data);
                            }
                        }
                        else
                            ds = d.GetData("SELECT  DISTINCT EO.EventID, E.Header, E.Content, E.SponsorPresenter, EO.DateTimeStart FROM Events E, Event_Occurance EO WHERE EO.DateTimeStart >= GETDATE() AND  E.ID=EO.EventID " + country);

                        if (ds.Tables.Count > 0)
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                            }
                            else
                                getUS = true;
                        else
                            getUS = true;
                    }
                }
                else
                {
                    getUS = true;
                }

                if (getUS)
                {
                    ds = d.GetData("SELECT DISTINCT EO.EventID, E.Header, E.Content, E.SponsorPresenter, EO.DateTimeStart FROM Events E, Event_Occurance EO  WHERE EO.DateTimeStart >= GETDATE() AND  E.ID=EO.EventID AND E.Country=223");
                }

                if (ds.Tables.Count > 0)
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        rowCount = ds.Tables[0].Rows.Count;
                        if (rowCount > cutOff)
                            rowCount = cutOff;
                        for (int i = 0; i < rowCount; i++)
                        {
                            ASP.controls_otherevent_ascx anEvent = new ASP.controls_otherevent_ascx();
                            anEvent.EVENT_ID = ds.Tables[0].Rows[i]["EventID"].ToString();
                            anEvent.TITLE = ds.Tables[0].Rows[i]["Header"].ToString();
                            anEvent.SUMMARY = ds.Tables[0].Rows[i]["Content"].ToString();
                            anEvent.PRESENTED_BY = ds.Tables[0].Rows[i]["DateTimeStart"].ToString();
                            EventsPanel.Controls.Add(anEvent);
                            //goto NotMuchElse;

                            d.Execute("INSERT INTO Events_Seen_Generic (eventID, IP, Date, SessionID) " +
                                                                " VALUES(" + ds.Tables[0].Rows[i]["EventID"].ToString() + ", '" + d.GetIP() + "', " +
                                                                " '" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Date.ToString() + "', '" + Session["GenericEventSession"].ToString() + "')");
                        }

                    }

            }
        }
        catch (Exception ex)
        {

        }
    }
}
