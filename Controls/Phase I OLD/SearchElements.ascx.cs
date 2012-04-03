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

public partial class SearchElements : System.Web.UI.UserControl
{
    public DataSet EVENTS_DS
    {
        get { return eventsDS; }
        set { eventsDS = value; }
    }
    public bool IS_CONNECT_TO
    {
        get { return isConnectTo; }
        set { isConnectTo = value; }
    }
    public int SELECTED_PAGE
    {
        get { return selectedpage; }
        set { selectedpage = value; }
    }
    public string SORT_STR
    {
        get { return sortString; }
        set { sortString = value; }
    }
    public bool IS_WINDOW
    {
        get { return windowT; }
        set { windowT = value; }
    }
    public bool DO_MAP
    {
        get { return doMap; }
        set { doMap = value; }
    }
    public string[] MAP_STRINGS
    {
        get { return mapStrings; }
        set { mapStrings = value; }
    }
    public int NUM_OF_PAGES
    {
        get { return num_of_pages; }
        set { num_of_pages = value; }
    }
    public bool DO_CALENDAR
    {
        get { return doCalendar; }
        set { doCalendar = value; }
    }
    public int COUNT_UNIQUE_VENUES
    {
        get { return countOfUniqueVenues; }
        set { countOfUniqueVenues = value; }
    }
    public int CUT_OFF
    {
        get { return theCutOff; }
        set { theCutOff = value; }
    }
    private int theCutOff = 200;
    private int countOfUniqueVenues = 0;
    private bool doCalendar = false;
    private int num_of_pages = 10;
    private string [] mapStrings;
    private bool doMap = false;
    protected bool windowT = false;
    protected string sortString = "";
    protected DataSet eventsDS;
    bool isConnectTo = false;
    protected int selectedpage;

    protected void Page_Load(object sender, EventArgs e)
    {
         //ASP.controls_pager_ascx pager = (ASP.controls_pager_ascx)SearchElementsPanel.FindControl("PagerPanel");
         //   if(pager != null)
         //   selectedpage = pager.SELECTED_PAGE;
        
    }
    
    public int DataBind2()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        Hashtable hash = new Hashtable();
                        int countInHash = 0;

        if (eventsDS != null)
        {
            try
            {
                Clear();
                int eventCount = 0;
                if (eventsDS.Tables.Count > 0)
                    eventCount = eventsDS.Tables[0].Rows.Count;


                ArrayList a = new ArrayList(eventCount);

                int searchTo = eventCount;

                if (dat.SearchCutOffNumber < eventCount)
                {
                    searchTo = dat.SearchCutOffNumber;
                }

                DataView dv = new DataView(eventsDS.Tables[0], "", "", DataViewRowState.CurrentRows);
                if (Session["sortString"] != null)
                {
                    if (Session["sortString"] != "")
                    {
                        dv.Sort = Session["sortString"].ToString();
                        sortString = Session["sortString"].ToString();
                    }
                }
                int countGotten = 0;
                int i = 0;


                Hashtable actualHash = new Hashtable();
                while (countInHash < searchTo && i < dv.Count)
                {
                    if (!hash.Contains(dv[i]["HashID"].ToString()))
                    {
                        hash.Add(dv[i]["HashID"].ToString(), "");
                        actualHash.Add(countInHash, dv[i]["HashID"].ToString());
                        countInHash++;
                    }
                    i++;
                }

                int somecount = 0;

                ASP.controls_pager_test_ascx pagerPanel = new ASP.controls_pager_test_ascx();
                
                pagerPanel.NUMBER_OF_ITEMS_PER_PAGE = num_of_pages;
                pagerPanel.ID = "PagerPanel";
                if (Request.Url.AbsolutePath.ToLower() == "/searchresults.aspx")
                {
                    pagerPanel.RUN_FUNCTION = "initialize";
                    pagerPanel.PANEL_NAME = "EventSearchElements_PagerPanel_Panel";
                }
                else if (Request.Url.AbsolutePath.ToLower() == "/eventcategorysearch.aspx")
                {
                    pagerPanel.PANEL_NAME = "ctl00_ContentPlaceHolder1_SearchElements_PagerPanel_Panel";
                }
                else  if (Request.Url.AbsolutePath.ToLower() == "/Friend.aspx")
                {
                    pagerPanel.PANEL_NAME = "ctl00_ContentPlaceHolder1_EventsCtrl_PagerPanel_Panel";
                }
                int mapCount = 0;

                if (countInHash > theCutOff)
                    countInHash = theCutOff;

                for (int j = 0; j < countInHash;j++ )
                {
                    try
                    {
                        string str = actualHash[j].ToString();
                        dv.RowFilter = "HashID = '" + str + "'";
                        ASP.controls_searchelement_ascx searchElement = new ASP.controls_searchelement_ascx();

                        //if (j % num_of_pages == 0)
                        //{
                        //    if (mapStrings != null)
                        //    {
                        //        searchElement.IMAGE_LITERAL += mapStrings[mapCount];
                        //        mapCount++;
                        //    }
                        //}

                        if (doCalendar)
                        {
                            searchElement.CALENDAR_LETTER = dv[0]["CalendarNum"].ToString();
                        }
                        searchElement.NUM_OF_ALL_RESULTS = countOfUniqueVenues;
                        searchElement.SEARCH_LABEL = dv[0]["Header"].ToString();

                        string toolTipID = "";
                        if (bool.Parse(dv[0]["isGroup"].ToString()))
                        {
                            searchElement.EVENT_ID = int.Parse(dv[0]["EID"].ToString());
                            searchElement.IS_GROUP = true;
                            searchElement.IS_WINDOW = windowT;

                            toolTipID = "True" + dv[0]["EID"].ToString() + dv[0]["ReoccurrID"].ToString();

                            if (doMap)
                            {
                                searchElement.SEARCH_MAP_NUM = dv[0]["SearchNum"].ToString();
                            }
                            
                            foreach (DataRowView row in dv)
                            {
                                searchElement.DATE_LABEL += row["DateTimeStart"].ToString() +
                                    "<span style='font-family: Arial; font-size: 14px; font-weight: bold; color: #1fb6e7;'>, </span>";
                            }

                            DataView dvEventOccurance = dat.GetDataDV("SELECT * FROM GroupEvent_Occurance WHERE GroupEventID=" +
                                dv[0]["EID"].ToString() + " AND ID=" + dv[0]["ReoccurrID"].ToString());
                            DataView dvGroupEvent = dat.GetDataDV("SELECT * FROM GroupEvents WHERE ID=" + dv[0]["EID"].ToString());
                            bool getAddress = false;
                            if (dvEventOccurance[0]["VenueID"] != null)
                            {
                                if (dvEventOccurance[0]["VenueID"].ToString() != "")
                                {
                                    DataView dvVenue = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + dvEventOccurance[0]["VenueID"].ToString());
                                    searchElement.VENUE_LABEL = dvVenue[0]["Name"].ToString();
                                    searchElement.VENUE_ID = int.Parse(dvVenue[0]["ID"].ToString());
                                }
                                else
                                    getAddress = true;
                            }
                            else
                                getAddress = true;

                            if (getAddress)
                            {
                                bool isInter = false;
                                if (dvEventOccurance[0]["Country"].ToString() != "223")
                                    isInter = true;
                                string address = "";
                                if (dvEventOccurance[0]["Country"].ToString() == "223")
                                {
                                    address = dvEventOccurance[0]["StreetNumber"].ToString() + " " + dvEventOccurance[0]["StreetName"].ToString() +
                                        " " + dvEventOccurance[0]["StreetDrop"].ToString();
                                }
                                else
                                {
                                    address = dvEventOccurance[0]["Location"].ToString();
                                }
                                searchElement.ADDRESS = address;
                            }
                        }
                        else
                        {
                            searchElement.VENUE_LABEL = dv[0]["Name"].ToString();
                            searchElement.NUM_LABEL = ", " + dv[0]["EventGoersCount"].ToString() +
                                " Peeps attending";
                            searchElement.EVENT_ID = int.Parse(dv[0]["EID"].ToString());
                            searchElement.IS_WINDOW = windowT;

                            toolTipID = "False" + dv[0]["EID"].ToString() + dv[0]["ReoccurrID"].ToString();

                            if (doMap)
                            {
                                searchElement.SEARCH_MAP_NUM = dv[0]["SearchNum"].ToString();
                            }

                            foreach (DataRowView row in dv)
                            {
                                searchElement.DATE_LABEL += row["DateTimeStart"].ToString() +
                                    "<span style='font-family: Arial; font-size: 14px; font-weight: bold; color: #1fb6e7;'>, </span>";
                            }

                            searchElement.VENUE_ID = int.Parse(dv[0]["VID"].ToString());
                        }

                        searchElement.TOOL_TIP_ID = toolTipID;

                        if (isConnectTo)
                        {
                            searchElement.IS_CONNECT_TO = bool.Parse(dv[0]["isConnect"].ToString());
                            searchElement.WIDTH = 417;
                        }

                        if (somecount % 2 != 0)
                            searchElement.COLOR = "#1b1b1b";

                        somecount++;
                        a.Add(searchElement);

                        //pagerPanel.Add(searchElement);
                    }
                    catch (Exception ex)
                    {
                        Label lab = new Label();
                        lab.Text = ex.ToString();
                        a.Add(lab);
                    }

                }
                
                pagerPanel.DATA = a;
                pagerPanel.WIDTH = 420;
                pagerPanel.DataBind2();
                SearchElementsPanel.Controls.Add(pagerPanel);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.ToString();
            }
        }
        return countInHash;
    }
    public void Clear()
    {
        SearchElementsPanel.Controls.Clear();
    }
}
