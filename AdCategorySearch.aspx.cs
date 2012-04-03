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

public partial class AdCategorySearch : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        DateTime today = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
        Data dat = new Data(today);
        DataSet ds;

        

        if (Request.QueryString["ID"] != null)
        {
            string catID = Request.QueryString["ID"].ToString();
            string location = "";

            //Get the location
            if (Session["User"] != null)
            {
                DataView dvPrefs = dat.GetDataDV("SELECT * FROM UserPreferences WHERE UserID=" + Session["User"].ToString());
                location = " AND A.CatState='" + dvPrefs[0]["CatState"].ToString() + "' AND A.CatCity='" + dvPrefs[0]["CatCity"].ToString() + "' AND A.CatCountry=" + dvPrefs[0]["CatCountry"].ToString();
            }
            else
            {
                if (Session["LocCountry"] == null || Session["LocState"] == null || Session["LocCity"] == null)
                {
                    dat.IP2Location();
                }

                string country = Session["LocCountry"].ToString().Trim();
                string state = Session["LocState"].ToString().Trim();

                string city = Session["LocCity"].ToString();
                location = " AND A.CatState='" + state + "' AND A.CatCity='" + city + "' AND A.CatCountry=" + country;
            }
            
            string sqlToday = "CONVERT(DATETIME, '" + today.Month.ToString() + "/" +
                today.Day.ToString() + "/" + today.Year.ToString() + "')";
            string timeline = " AND (A.Featured='False' AND AC.DateTimeEnd >= " +
                sqlToday + ")";

            ds = dat.GetData("SELECT ACM.AdID AS VID, A.Featured, C.ID, A.Header, C.Name AS CategoryName, " +
                "ACM.tagSize FROM Ad_Category_Mapping ACM, AdCategories C, Ads A, Ad_Calendar AC WHERE " +
                "ACM.AdID=A.Ad_ID AND AC.AdID=A.Ad_ID AND ACM.CategoryID=C.ID AND A.LIVE = 'True' AND ACM.CategoryID=" +
                catID + location + timeline);
            SearchElements.AD_DS = ds;
            SearchElements.DataBind2();

            NumResultsLabel.Text = "(" + ds.Tables[0].Rows.Count + " Records Found)";

            HtmlHead head = (HtmlHead)Page.Header;

            string theLink = "";

            if (ds.Tables[0].Rows.Count > 0)
            {
                theLink = "http://" + Request.Url.Authority + "/" +
                dat.MakeNiceName(ds.Tables[0].Rows[0]["CategoryName"].ToString()) + "_Ad_Category";
            }
            else
            {
                theLink = "http://" + Request.Url.Authority + "/AdCategorySearch.aspx?ID=" + catID;
            }


            HtmlLink lk = new HtmlLink();
            lk.Href = theLink;
            lk.Attributes.Add("rel", "bookmark");
            head.Controls.AddAt(0, lk);

            HtmlMeta hm = new HtmlMeta();
            HtmlMeta kw = new HtmlMeta();
            HtmlMeta dc = new HtmlMeta();

            kw.Name = "keywords";

            if (ds.Tables[0].Rows.Count > 0)
            {
                kw.Content = ds.Tables[0].Rows[0]["CategoryName"].ToString() + " Ad Category";
            }
            else
            {
                kw.Content = "No Ad Category";
            }

            head.Controls.AddAt(0, kw);

            dc.Name = "description";
            if (ds.Tables[0].Rows.Count > 0)
            {
                dc.Content = "Results for ad category: " + ds.Tables[0].Rows[0]["CategoryName"].ToString();
            }
            else
            {
                dc.Content = "Results for ad category: None";
            }

            head.Controls.AddAt(0, dc);

            if(ds.Tables.Count > 0)
                if (ds.Tables[0].Rows.Count > 0)
                {
                    SearchResultsTitleLabel.Text = "<a href=\"" + theLink + "\" style=\"text-decoration: none; color: white;\" ><h1>Ads in '" +
                        ds.Tables[0].Rows[0]["CategoryName"].ToString() + "' Category</h1></a>";
                }
                else
                {
                    SearchResultsTitleLabel.Text = "No Ads Found";
                }
            else
            {
                SearchResultsTitleLabel.Text = "No Ads Found";
            }
        }else
        {
            Response.Redirect("home");
        }
    }
}
