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

public partial class TripSearchElements : System.Web.UI.UserControl
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

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
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
                pagerPanel.ID = "PagerPanel";
                if (Request.Url.AbsolutePath.ToLower() == "/tripsearch.aspx")
                {
                    dv.Sort = "colOrder ASC";
                    pagerPanel.RUN_FUNCTION = "initialize";
                    pagerPanel.PANEL_NAME = "ctl00_ContentPlaceHolder1_EventSearchElements_PagerPanel_Panel";
                }
                else if (Request.Url.AbsolutePath.ToLower() == "/tripcategorysearch.aspx")
                {
                    pagerPanel.PANEL_NAME = "ctl00_ContentPlaceHolder1_SearchElements_PagerPanel_Panel";
                }
                int mapCount = 0;
                string means = "";

                foreach(DataRowView row in dv)
                {
                    if (countGotten < searchTo)
                    {
                        try
                        {
                            ASP.controls_tripsearchelement_ascx searchElement = new ASP.controls_tripsearchelement_ascx();


                            if (bool.Parse(row["Featured"].ToString()))
                            {
                                if (row["DaysFeatured"].ToString().Contains(";" + featureDate + ";"))
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

                            means = "";
                            if (row["Means"].ToString().Contains(";1"))
                            {
                                means += "Car, ";
                            }

                            if (row["Means"].ToString().Contains(";2"))
                            {
                                means += "Walking, ";
                            }

                            if (row["Means"].ToString().Contains(";3"))
                            {
                                means += "Hiking, ";
                            }

                            if (row["Means"].ToString().Contains(";4"))
                            {
                                means += "Biking, ";
                            }

                            if (row["Means"].ToString().Contains(";5"))
                            {
                                means += "Flying, ";
                            }

                            if (row["PriceRange"].ToString().Trim() != "")
                                means = row["PriceRange"].ToString() + ", " + means;

                            searchElement.PRICE_MEANS_TIMEFRAME = means + row["TimeFrame"].ToString();

                            string toolTipID = "";

                            searchElement.EVENT_ID = int.Parse(row["EID"].ToString());
                            searchElement.IS_WINDOW = false;



                            if (doMap)
                            {
                                searchElement.SEARCH_MAP_NUM = row["SearchNum"].ToString();
                            }


                            toolTipID = "False" + row["EID"].ToString();



                            searchElement.TOOL_TIP_ID = toolTipID;

                            if (isConnectTo)
                            {
                                searchElement.IS_CONNECT_TO = bool.Parse(row["isConnect"].ToString());
                                searchElement.WIDTH = 417;
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
