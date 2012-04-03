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
    }
    
    public int DataBind2()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;
        
        DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn);
        DateTime isNow = isn;
        Data dat = new Data(isn);
        Hashtable hash = new Hashtable();
        int countInHash = 0;

        string featureDate = isNow.Month.ToString() +"/"+isNow.Day.ToString()+"/"+isNow.Year.ToString();

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

                DataView dvEvent;

                int countGotten = 0;
                int i = 0;


                ASP.controls_pager_ascx pagerPanel = new ASP.controls_pager_ascx();
                
                pagerPanel.NUMBER_OF_ITEMS_PER_PAGE = num_of_pages;
                
                if (Request.Url.AbsolutePath.ToLower() == "/eventsearch.aspx")
                {
                    pagerPanel.ID = "PagerPanel";
                    pagerPanel.RUN_FUNCTION = "initialize";
                    dv.Sort = "colOrder ASC";
                }
                else if (Request.Url.AbsolutePath.ToLower() == "/eventcategorysearch.aspx")
                {
                    pagerPanel.PANEL_NAME = "ctl00_ContentPlaceHolder1_SearchElements_PagerPanel_Panel";
                }
                else  if (Request.Url.AbsolutePath.ToLower() == "/friend.aspx")
                {
                    pagerPanel.ID = "PagerPanel";
                    pagerPanel.PANEL_NAME = "ctl00_ContentPlaceHolder1_EventsCtrl_PagerPanel_Panel";
                }
                int mapCount = 0;

                bool isEvent = true;
                DateTime dateOfEvent;
                foreach(DataRowView row in dv)
                {
                    if (countGotten < searchTo)
                    {
                        try
                        {
                            ASP.controls_searchelement_ascx searchElement = new ASP.controls_searchelement_ascx();

                            if (dv.Table.Columns.Contains("Type"))
                            {
                                if (row["Type"].ToString() == "V")
                                    isEvent = false;
                                else
                                    isEvent = true;
                            }
                            else
                            {
                                isEvent = true;
                            }

                            if (isEvent)
                            {
                                dvEvent = dat.GetDataDV("SELECT *, EO.ID AS ReoccurrID FROM Events E, Event_Occurance EO WHERE E.ID=EO.EventID AND E.ID=" + row["EID"].ToString());

                                if (bool.Parse(dvEvent[0]["Featured"].ToString()))
                                {
                                    if (dvEvent[0]["DaysFeatured"].ToString().Contains(";" + featureDate + ";"))
                                        searchElement.IS_FEATURED = true;
                                    else
                                        searchElement.IS_FEATURED = false;
                                }
                                else
                                {
                                    searchElement.IS_FEATURED = false;
                                }


                                searchElement.NUM_OF_ALL_RESULTS = countOfUniqueVenues;
                                searchElement.SEARCH_LABEL = row["Header"].ToString();

                                string toolTipID = "";

                                searchElement.VENUE_LABEL = row["Name"].ToString();
                                searchElement.EVENT_ID = int.Parse(row["EID"].ToString());
                                searchElement.IS_WINDOW = false;



                                if (doMap)
                                {
                                    searchElement.SEARCH_MAP_NUM = row["SearchNum"].ToString();
                                }

                                if (isEvent)
                                {
                                    dateOfEvent = DateTime.Parse(row["DateTimeStart"].ToString());
                                    searchElement.DATE_LABEL = dateOfEvent.DayOfWeek.ToString().Substring(0, 3) +
                                        " " + row["DateTimeStart"].ToString().Replace(":00",
                                        "").Trim().Replace(" PM", "p").Replace(" AM", "a").Replace("/" + dateOfEvent.Year.ToString(), "");
                                    toolTipID = "False" + row["EID"].ToString() + row["ReoccurrID"].ToString();
                                }


                                if (dv.Count > 1)
                                    searchElement.DATE_LABEL += "+";

                                if (row["VID"].ToString() != "NONE")
                                    searchElement.VENUE_ID = int.Parse(row["VID"].ToString());


                                searchElement.TOOL_TIP_ID = toolTipID;

                                if (isConnectTo)
                                {
                                    searchElement.IS_CONNECT_TO = bool.Parse(row["isConnect"].ToString());
                                    searchElement.WIDTH = 417;
                                }
                            }
                            else
                            {
                                dvEvent = dat.GetDataDV("SELECT * FROM Venues E WHERE E.ID=" + row["VID"].ToString());

                                if (bool.Parse(dvEvent[0]["Featured"].ToString()))
                                {
                                    if (dvEvent[0]["DaysFeatured"].ToString().Contains(";" + featureDate + ";"))
                                        searchElement.IS_FEATURED = true;
                                    else
                                        searchElement.IS_FEATURED = false;
                                }
                                else
                                {
                                    searchElement.IS_FEATURED = false;
                                }


                                searchElement.NUM_OF_ALL_RESULTS = countOfUniqueVenues;
                                searchElement.SEARCH_LABEL = row["Header"].ToString();

                                string toolTipID = "";

                                searchElement.DATE_LABEL = "";
                                searchElement.VENUE_LABEL = row["Name"].ToString();
                                searchElement.VENUE_ID = int.Parse(row["VID"].ToString());
                                searchElement.IS_WINDOW = false;
                                searchElement.IS_VENUE = true;

                                if (doMap)
                                {
                                    searchElement.SEARCH_MAP_NUM = row["SearchNum"].ToString();
                                }

                                toolTipID = "False" + row["VID"].ToString();
                                searchElement.TOOL_TIP_ID = toolTipID;
                            }
                            //if (somecount % 2 != 0)
                            searchElement.COLOR = "#ebe7e7";

                            a.Add(searchElement);

                            //pagerPanel.Add(searchElement);
                            countGotten++;
                        }
                        catch (Exception ex)
                        {
                            Label lab = new Label();
                            lab.Text = ex.ToString();
                            a.Add(lab);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                
                
                SearchElementsPanel.Controls.Add(pagerPanel);
                pagerPanel.PANEL_NAME = pagerPanel.ClientID + "_Panel";
                pagerPanel.DATA = a;
                pagerPanel.WIDTH = 429;
                
                pagerPanel.DataBind2();
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
