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

public partial class AdSearchElements : System.Web.UI.UserControl
{
    public DataSet AD_DS
    {
        get { return adDS; }
        set { adDS = value; }
    }
    public bool IS_WINDOW
    {
        get { return windowT; }
        set { windowT = value; }
    }
    protected bool windowT = false;
    protected DataSet adDS;
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    public void DataBind2()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (adDS != null)
        {
            int adCount = 0;
            if (adDS.Tables.Count > 0)
                adCount = adDS.Tables[0].Rows.Count;
            

            ArrayList a = new ArrayList(adCount);

            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            int searchTo = adCount;

            if (dat.SearchCutOffNumber < adCount)
            {
                searchTo = dat.SearchCutOffNumber;
            }


            for (int i = 0; i < searchTo; i++)
            {
                ASP.controls_adsearchelement_ascx searchElement = new ASP.controls_adsearchelement_ascx();
                searchElement.AD_ID = int.Parse(adDS.Tables[0].Rows[i]["Ad_ID"].ToString());
                searchElement.SEARCH_LABEL = adDS.Tables[0].Rows[i]["Header"].ToString();
                searchElement.IS_WINDOW = windowT;
                if (i % 2 != 0)
                    searchElement.COLOR = "#1b1b1b";

                
                a.Add(searchElement);

                //pagerPanel.Add(searchElement);
                
            }
            ASP.controls_pager_test_ascx pagerPanel = new ASP.controls_pager_test_ascx();
            pagerPanel.NUMBER_OF_ITEMS_PER_PAGE = 10;
            pagerPanel.DATA = a;
            pagerPanel.WIDTH = 420;
            pagerPanel.PANEL_NAME = "AdSearchElements_ctl00_Panel";
            pagerPanel.DataBind2();
            SearchElementsPanel.Controls.Add(pagerPanel);
        }
    }
    public void Clear()
    {
        SearchElementsPanel.Controls.Clear();
    }
}
