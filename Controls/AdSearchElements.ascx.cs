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
        if (adDS != null)
        {
            int adCount = 0;
            if (adDS.Tables.Count > 0)
                adCount = adDS.Tables[0].Rows.Count;
            

            ArrayList a = new ArrayList(adCount);
            HttpCookie cookie = Request.Cookies["BrowserDate"];

            DateTime isn = DateTime.Now;

            if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
                isn = DateTime.Now;
            DateTime isNow = isn;
            Data dat = new Data(isn);
            string featureDate = isNow.Month.ToString() + "/" + isNow.Day.ToString() + "/" + isNow.Year.ToString();
            int searchTo = adCount;

            if (dat.SearchCutOffNumber < adCount)
            {
                searchTo = dat.SearchCutOffNumber;
            }

            DataView dvAd;
            DataView dvAds = new DataView(adDS.Tables[0], "", "", DataViewRowState.CurrentRows);

            ASP.controls_pager_ascx pagerPanel = new ASP.controls_pager_ascx();

            if (Request.Url.AbsolutePath.ToLower() == "/adsearch.aspx")
            {
                pagerPanel.PANEL_NAME = "AdSearchElements_ctl00_Panel";
                dvAds.Sort = "colOrder ASC";
            }
            else
            {
                pagerPanel.PANEL_NAME = "ctl00_ContentPlaceHolder1_SearchElements_ctl00_Panel";
            }

            foreach(DataRowView ad in dvAds)
            {
                ASP.controls_adsearchelement_ascx searchElement = new ASP.controls_adsearchelement_ascx();
                searchElement.AD_ID = int.Parse(ad["VID"].ToString());
                searchElement.SEARCH_LABEL = ad["Header"].ToString();
                searchElement.IS_WINDOW = windowT;

                dvAd = dat.GetDataDV("SELECT * FROM Ads WHERE Ad_ID=" + ad["VID"].ToString());

                if (bool.Parse(dvAd[0]["Featured"].ToString()))
                {
                    if (dvAd[0]["DatesOfAd"].ToString().Contains(";" + featureDate + ";"))
                        searchElement.IS_FEATURED = true;
                    else
                        searchElement.IS_FEATURED = false;
                }
                else
                {
                    searchElement.IS_FEATURED = false;
                }
                
                a.Add(searchElement);

                //pagerPanel.Add(searchElement);
                
            }
            
            SearchElementsPanel.Controls.Add(pagerPanel);
            pagerPanel.NUMBER_OF_ITEMS_PER_PAGE = 10;
            pagerPanel.DATA = a;
            pagerPanel.WIDTH = 420;
            pagerPanel.PANEL_NAME = pagerPanel.ClientID + "_Panel";

            pagerPanel.DataBind2();
        }
    }
    public void Clear()
    {
        SearchElementsPanel.Controls.Clear();
    }
}
