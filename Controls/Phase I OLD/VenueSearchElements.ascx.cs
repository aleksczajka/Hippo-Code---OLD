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

public partial class VenueSearchElements : System.Web.UI.UserControl
{
    public DataSet VENUE_DS
    {
        get { return venueDS; }
        set { venueDS = value; }
    }
    protected DataSet venueDS;
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
    private int num_of_pages = 10;
    private string[] mapStrings;
    private bool doMap = false;
    protected bool windowT = false;
    protected string sortString = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    public void DataBind2()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (venueDS != null)
        {
            int venueCount = 0;
            if (venueDS.Tables.Count > 0)
                venueCount = venueDS.Tables[0].Rows.Count;
            

            ArrayList a = new ArrayList(venueCount);

            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            int searchTo = venueCount;

            if (dat.SearchCutOffNumber < venueCount)
            {
                searchTo = dat.SearchCutOffNumber;
            }
            int mapCount = 0;

            for (int i = 0; i < searchTo; i++)
            {
                ASP.controls_venuesearchelement_ascx searchElement = new ASP.controls_venuesearchelement_ascx();
                searchElement.Venue_ID = int.Parse(venueDS.Tables[0].Rows[i]["VID"].ToString());
                searchElement.SEARCH_LABEL = venueDS.Tables[0].Rows[i]["Name"].ToString();
                searchElement.CITY = venueDS.Tables[0].Rows[i]["City"].ToString();
                searchElement.STATE = venueDS.Tables[0].Rows[i]["State"].ToString();
                searchElement.IS_WINDOW = windowT;
                searchElement.NUM_OF_ALL_RESULTS = searchTo;
                if (doMap)
                {
                    searchElement.SEARCH_MAP_NUM = venueDS.Tables[0].Rows[i]["SearchNum"].ToString();
                }
                if (i % 2 != 0)
                    searchElement.COLOR = "#1b1b1b";

                //if (i % num_of_pages == 0)
                //{
                //    //if (mapCount < mapStrings.Length)
                //    //{
                //    searchElement.IMAGE_LITERAL += mapStrings[mapCount];
                //    mapCount++;
                //    //}
                //}

                a.Add(searchElement);

                //pagerPanel.Add(searchElement);
                
            }
            ASP.controls_pager_test_ascx pagerPanel = new ASP.controls_pager_test_ascx();

            
            pagerPanel.NUMBER_OF_ITEMS_PER_PAGE = num_of_pages;
            if (Request.Url.AbsolutePath.ToLower() == "/searchresults.aspx")
            {
                pagerPanel.PANEL_NAME = "VenueSearchElements_ctl00_Panel";
                pagerPanel.RUN_FUNCTION = "initialize";
            }
            else if (Request.Url.AbsolutePath.ToLower() == "/venuecategorysearch.aspx")
            {
                pagerPanel.PANEL_NAME = "ctl00_ContentPlaceHolder1_SearchElements_ctl00_Panel";
            }

            pagerPanel.DATA = a;
            pagerPanel.WIDTH = 420;
            pagerPanel.DataBind2();
            
            
            SearchElementsPanel.Controls.Add(pagerPanel);
        }
    }
    public void Clear()
    {
        SearchElementsPanel.Controls.Clear();
    }
}
