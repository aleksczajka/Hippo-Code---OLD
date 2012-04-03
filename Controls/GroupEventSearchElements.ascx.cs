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

public partial class GroupEventSearchElements : System.Web.UI.UserControl
{
    public DataSet GROUP_DS
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


            for (int i = 0; i < searchTo; i++)
            {
                ASP.controls_groupeventsearchelement_ascx searchElement = new ASP.controls_groupeventsearchelement_ascx();
                searchElement.Venue_ID = int.Parse(venueDS.Tables[0].Rows[i]["GroupEvent_ID"].ToString());
                searchElement.SEARCH_LABEL = venueDS.Tables[0].Rows[i]["Name"].ToString();
                searchElement.CITY = venueDS.Tables[0].Rows[i]["City"].ToString();
                searchElement.STATE = venueDS.Tables[0].Rows[i]["State"].ToString();
                searchElement.IS_WINDOW = windowT;
                if (i % 2 != 0)
                    searchElement.COLOR = "#1b1b1b";

                a.Add(searchElement);

                //pagerPanel.Add(searchElement);
                
            }
            ASP.controls_pager_ascx pagerPanel = new ASP.controls_pager_ascx();
            pagerPanel.NUMBER_OF_ITEMS_PER_PAGE = 10;
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
