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

public partial class TripCategorySearch : Telerik.Web.UI.RadAjaxPage
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
                location = " AND TD.State='" + dvPrefs[0]["CatState"].ToString() + "' AND TD.City='" +
                    dvPrefs[0]["CatCity"].ToString() + "' AND TD.Country=" + dvPrefs[0]["CatCountry"].ToString();
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
                location = " AND TD.State='" + state + "' AND TD.City='" + city + "' AND TD.Country=" + country;
            }

            string command = "SELECT  DISTINCT E.ID AS EID, '$'+CONVERT(NVARCHAR,E.MinPrice)+' - $'+CONVERT(NVARCHAR, E.MaxPrice)" +
                            "AS PriceRange, E.MinPrice AS Price, E.Means, dbo.GetDuration(E.Duration, 0) AS " +
                            "TimeFrame, dbo.GetDuration(E.Duration, 1) AS Duration, E.Header, " +
                            " E.Featured, E.DaysFeatured " +
                "FROM Trip_Category ECM, Trips E, TripDirections TD, TripCategories C " +
                "WHERE C.ID=ECM.CategoryID AND TD.TripID=E.ID AND ECM.TripID=E.ID AND " +
                "ECM.CategoryID=" + catID + location;


            ds = dat.GetData(command);
            SearchElements.EVENTS_DS = ds;
            SearchElements.DataBind2();

            NumResultsLabel.Text = "(" + ds.Tables[0].Rows.Count + " Records Found)";

            DataView dvName = dat.GetDataDV("SELECT * FROM TripCategories WHERE ID=" + catID);

            HtmlHead head = (HtmlHead)Page.Header;

            string theLink = "http://" + Request.Url.Authority + "/" +
               dat.MakeNiceName(dvName[0]["Name"].ToString()) + "_Trip_Category";

            HtmlLink lk = new HtmlLink();
            lk.Href = theLink;
            lk.Attributes.Add("rel", "bookmark");
            head.Controls.AddAt(0, lk);

            HtmlMeta hm = new HtmlMeta();
            HtmlMeta kw = new HtmlMeta();
            HtmlMeta dc = new HtmlMeta();

            kw.Name = "keywords";
            kw.Content = dvName[0]["Name"].ToString() + " trip Category";

            head.Controls.AddAt(0, kw);

            dc.Name = "description";
            dc.Content = "Results for trip category: " + dvName[0]["Name"].ToString();

            head.Controls.AddAt(0, dc);


            SearchResultsTitleLabel.Text = "<a href=\"" + theLink +
                "\" style=\"text-decoration: none;\" ><h1>Trips in '" +
                dvName[0]["Name"].ToString() + "' Category</h1></a>";

        }
        else
        {
            Response.Redirect("home");
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {

    }
}
