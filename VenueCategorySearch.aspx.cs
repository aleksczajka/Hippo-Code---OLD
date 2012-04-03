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

public partial class VenueCategorySearch : Telerik.Web.UI.RadAjaxPage
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
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet ds;

        HtmlHead head = (HtmlHead)Page.Header;

        if (Request.QueryString["ID"] != null)
        {
            string catID = Request.QueryString["ID"].ToString();

            string location = "";

            //Get the location
            if (Session["User"] != null)
            {
                DataView dvPrefs = dat.GetDataDV("SELECT * FROM UserPreferences WHERE UserID=" + Session["User"].ToString());
                location = " AND V.State='" + dvPrefs[0]["CatState"].ToString() + "' AND V.City='" + dvPrefs[0]["CatCity"].ToString() + "'";
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
                location = " AND V.State='" + state + "' AND V.City='" + city + "' AND V.Country=" + country;
            }

            ds = dat.GetData("SELECT V.Name, V.ID AS VID, V.City, V.State, C.ID, C.Name AS " +
                "CategoryName, VC.tagSize, V.Featured, V.DaysFeatured FROM Venue_Category VC, VenueCategories C, Venues V " +
                "WHERE V.ID=VC.VENUE_ID AND VC.CATEGORY_ID=C.ID AND VC.CATEGORY_ID=" + catID + location);
           
            SearchElements.VENUE_DS = ds;
            SearchElements.DataBind2();

            NumResultsLabel.Text = "(" + ds.Tables[0].Rows.Count + " Records Found)";

            DataView dvName = dat.GetDataDV("SELECT * FROM VenueCategories WHERE ID=" + catID);

            string theLink = "http://" + Request.Url.Authority + "/" +
            dat.MakeNiceName(dvName[0]["Name"].ToString()) + "_Venue_Category";

            HtmlLink lk = new HtmlLink();
            lk.Href = theLink;
            lk.Attributes.Add("rel", "bookmark");
            head.Controls.AddAt(0, lk);

            HtmlMeta hm = new HtmlMeta();
            HtmlMeta kw = new HtmlMeta();
            HtmlMeta dc = new HtmlMeta();

            kw.Name = "keywords";
            kw.Content = dvName[0]["Name"].ToString() + " Venue Category";

            head.Controls.AddAt(0, kw);

            dc.Name = "description";
            dc.Content = "Results for venue category: " + dvName[0]["Name"].ToString();

            head.Controls.AddAt(0, dc);

            SearchResultsTitleLabel.Text = "<a href=\"" + theLink + "\" style=\"text-decoration: none; color: white;\" ><h1>Venues in '" +
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
