

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

                if (Session["UserSessionEvents"] == null)
                {
                    DataSet dsT = d.GetEventsInLocation(true);
                    Session["UserSessionEvents"] = dsT;
                }


                DataSet ds = (DataSet)Session["UserSessionEvents"];

                if (Session["UserSessionLastSeenEvent"] == null)
                    Session["UserSessionLastSeenEvent"] = "-1";

                if (ds.Tables.Count > 0)
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        rowCount = ds.Tables[0].Rows.Count;
                        if (rowCount > cutOff)
                            rowCount = cutOff;

                        int startIndex = 0;
                        if (Session["UserSessionLastSeenEvent"].ToString() != "-1")
                            startIndex = int.Parse(Session["UserSessionLastSeenEvent"].ToString());


                        int endIndex = startIndex + cutOff;

                        Session["UserSessionLastSeenEvent"] = endIndex - 1;


                        if (endIndex > ds.Tables[0].Rows.Count)
                        {
                            endIndex = ds.Tables[0].Rows.Count;
                            Session["UserSessionLastSeenEvent"] = "-1";
                        }

                        for (int i = startIndex; i < endIndex; i++)
                        {
                            ASP.controls_otherevent_ascx anEvent = new ASP.controls_otherevent_ascx();
                            anEvent.EVENT_ID = ds.Tables[0].Rows[i]["ID"].ToString();
                            anEvent.TITLE = ds.Tables[0].Rows[i]["Header"].ToString();
                            anEvent.SUMMARY = ds.Tables[0].Rows[i]["Content"].ToString();
                            anEvent.PRESENTED_BY = d.GetDataDV("SELECT DateTimeStart FROM Event_Occurance WHERE EventID=" +
                                ds.Tables[0].Rows[i]["ID"].ToString())[0]["DateTimeStart"].ToString();
                            EventsPanel.Controls.Add(anEvent);
                        }
                    }
            }
            else
            {

                if (Session["GenericEventSession"] == null)
                {
                    DataView dvLocation = d.GetDataDV("SELECT * FROM Users WHERE IPs LIKE '&;" + d.GetIP() + ";%'");

                    if (dvLocation.Count == 0)
                        dvLocation = d.GetDataDV("SELECT * FROM SearchIPs WHERE IP = '" + d.GetIP() + "'");
                    DataSet ds = new DataSet();
                    bool getUS = false;


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

                        bool couldGetMore = false;

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


                        EventsPanel.Controls.Clear();
                        if (ds.Tables.Count > 0)
                            if (ds.Tables[0].Rows.Count > 0)
                            {

                            }
                            else
                            {
                                getUS = true;
                            }
                        else
                        {
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


                    Session["GenericEventSession"] = ds;
                    Session["GenericLastSeenEvent"] = "-1";
                }

                if (Session["GenericLastSeenEvent"] == null)
                    Session["GenericLastSeenEvent"] = "-1";

                DataSet dsTotal = (DataSet)Session["GenericEventSession"];

                if (dsTotal.Tables.Count > 0)
                    if (dsTotal.Tables[0].Rows.Count > 0)
                    {
                        rowCount = dsTotal.Tables[0].Rows.Count;
                        if (rowCount > cutOff)
                            rowCount = cutOff;

                        int startIndex = 0;
                        if (Session["GenericLastSeenEvent"].ToString() != "-1")
                            startIndex = int.Parse(Session["GenericLastSeenEvent"].ToString());


                        int endIndex = startIndex + cutOff;

                        Session["GenericLastSeenEvent"] = endIndex - 1;


                        if (endIndex > dsTotal.Tables[0].Rows.Count)
                        {
                            endIndex = dsTotal.Tables[0].Rows.Count;
                            Session["GenericLastSeenEvent"] = "-1";
                        }

                        for (int i = startIndex; i < endIndex; i++)
                        {
                            ASP.controls_otherevent_ascx anEvent = new ASP.controls_otherevent_ascx();
                            anEvent.EVENT_ID = dsTotal.Tables[0].Rows[i]["EventID"].ToString();
                            anEvent.TITLE = dsTotal.Tables[0].Rows[i]["Header"].ToString();
                            anEvent.SUMMARY = dsTotal.Tables[0].Rows[i]["Content"].ToString();
                            anEvent.PRESENTED_BY = dsTotal.Tables[0].Rows[i]["DateTimeStart"].ToString();
                            EventsPanel.Controls.Add(anEvent);
                        }
                    }

            }
        }
        catch (Exception ex)
        {
            Label l = new Label();
            l.Text = ex.ToString();
            EventsPanel.Controls.Add(l);
        }
    }
}
