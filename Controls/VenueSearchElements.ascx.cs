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

            DateTime isn = DateTime.Now;

            if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
                isn = DateTime.Now;
            DateTime isNow = isn;
            Data dat = new Data(isn); 
            
            string featureDate = isNow.Month.ToString() + "/" + isNow.Day.ToString() + "/" + isNow.Year.ToString();
            
            int searchTo = venueCount;

            if (dat.SearchCutOffNumber < venueCount)
            {
                searchTo = dat.SearchCutOffNumber;
            }
            int mapCount = 0;

            DataView dv = new DataView(venueDS.Tables[0], "", "", DataViewRowState.CurrentRows);

            ASP.controls_pager_ascx pagerPanel = new ASP.controls_pager_ascx();


            pagerPanel.NUMBER_OF_ITEMS_PER_PAGE = num_of_pages;
            if (Request.Url.AbsolutePath.ToLower() == "/venuesearch.aspx")
            {
                pagerPanel.PANEL_NAME = "ctl00_ContentPlaceHolder1_VenueSearchElements_ctl00_Panel";
                pagerPanel.RUN_FUNCTION = "initialize";
                dv.Sort = "colOrder ASC";
            }
            else if (Request.Url.AbsolutePath.ToLower() == "/venuecategorysearch.aspx")
            {
                pagerPanel.PANEL_NAME = "ctl00_ContentPlaceHolder1_SearchElements_ctl00_Panel";
            }
            

            int countGotten = 0;
            foreach (DataRowView row in dv)
            {
                if (countGotten < searchTo)
                {
                    ASP.controls_venuesearchelement_ascx searchElement = new ASP.controls_venuesearchelement_ascx();
                    searchElement.Venue_ID = int.Parse(row["VID"].ToString());
                    searchElement.SEARCH_LABEL = dat.stripHTML(row["Name"].ToString());
                    searchElement.CITY = row["City"].ToString();
                    searchElement.STATE = row["State"].ToString();
                    searchElement.NUM_OF_ALL_RESULTS = searchTo;
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
                    if (doMap)
                    {
                        searchElement.SEARCH_MAP_NUM = row["SearchNum"].ToString();
                    }

                    a.Add(searchElement);
                }
                else
                {
                    break;
                }
            }
            

            
            
            
            SearchElementsPanel.Controls.Add(pagerPanel);
            pagerPanel.PANEL_NAME = pagerPanel.ClientID + "_Panel";
            pagerPanel.DATA = a;
            pagerPanel.WIDTH = 420;
            pagerPanel.DataBind2();
        }
    }
    public void Clear()
    {
        SearchElementsPanel.Controls.Clear();
    }
}
