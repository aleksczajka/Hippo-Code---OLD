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

public partial class EventCategorySearch : Telerik.Web.UI.RadAjaxPage
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

        HtmlHead head = (HtmlHead)Page.Header;

        if (Request.QueryString["ID"] != null)
        {
            string catID = Request.QueryString["ID"].ToString();

            string location = "";

            //Get the location
            if (Session["User"] != null)
            {
                DataView dvPrefs = dat.GetDataDV("SELECT * FROM UserPreferences WHERE UserID=" + Session["User"].ToString());
                location = " AND V.State='" + dvPrefs[0]["CatState"].ToString() + "' AND V.City='" + dvPrefs[0]["CatCity"].ToString() + "' AND V.Country = " + dvPrefs[0]["CatCountry"].ToString();
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

            //get only events after today, since showing all events might be too much
            string timeline = " AND EO.DateTimeStart >= CONVERT(DATETIME, '" + today.Month.ToString() + "/" + today.Day.ToString() + "/" + today.Year.ToString() + "')";
            
            string command = "SELECT DISTINCT E.ID  " +
                "FROM Event_Occurance EO, Event_Category_Mapping ECM, Events E, Venues V, EventCategories C " +
                "WHERE EO.EventID=E.ID AND C.ID=ECM.CategoryID  AND ECM.EventID=E.ID AND " +
                "ECM.CategoryID=" + catID + location + timeline;

            DataView dvE = dat.GetDataDV(command);

            ds = new DataSet();
            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn("EID");
            dt.Columns.Add(dc);
            dc = new DataColumn("ReoccurrID");
            dt.Columns.Add(dc);
            dc = new DataColumn("DateTimeStart");
            dt.Columns.Add(dc);
            dc = new DataColumn("VID");
            dt.Columns.Add(dc);
            dc = new DataColumn("Header");
            dt.Columns.Add(dc);
            dc = new DataColumn("Name");
            dt.Columns.Add(dc);

            DataView dv;
            DataRow dr;
            foreach (DataRowView row in dvE)
            {
                dv = dat.GetDataDV("SELECT E.ID AS EID, EO.ID AS ReoccurrID, EO.DateTimeStart, E.Header, "+
                    "V.ID AS VID, V.Name " +
                "FROM Event_Occurance EO, Events E, Venues V " +
                "WHERE EO.EventID=E.ID AND E.Venue=V.ID AND E.ID=" + row["ID"].ToString());

                dr = dt.NewRow();
                dr["EID"] = dv[0]["EID"];
                dr["ReoccurrID"] = dv[0]["ReoccurrID"];
                dr["DateTimeStart"] = dv[0]["DateTimeStart"];
                dr["VID"] = dv[0]["VID"];
                dr["Header"] = dv[0]["Header"];
                dr["Name"] = dv[0]["Name"];
                dt.Rows.Add(dr);
            }

            ds.Tables.Add(dt);
            SearchElements.EVENTS_DS = ds;
            SearchElements.DataBind2();

            NumResultsLabel.Text = "(" + ds.Tables[0].Rows.Count + " Records Found)";

            DataView dvName = dat.GetDataDV("SELECT * FROM EventCategories WHERE ID=" + catID);

            string theLink = "http://" + Request.Url.Authority + "/" +
               dat.MakeNiceName(dvName[0]["Name"].ToString()) + "_Event_Category";

            HtmlLink lk = new HtmlLink();
            lk.Href = theLink;
            lk.Attributes.Add("rel", "bookmark");
            head.Controls.AddAt(0, lk);

            HtmlMeta hm = new HtmlMeta();
            HtmlMeta kw = new HtmlMeta();
            HtmlMeta dc2 = new HtmlMeta();

            kw.Name = "keywords";
            kw.Content = dvName[0]["Name"].ToString() + " event Category";

            head.Controls.AddAt(0, kw);

            dc2.Name = "description";
            dc2.Content = "Results for event category: " + dvName[0]["Name"].ToString();

            head.Controls.AddAt(0, dc2);


            SearchResultsTitleLabel.Text = "<a href=\"" + theLink +
                "\" style=\"text-decoration: none; \" ><h1>Events in '" +
                dvName[0]["Name"].ToString() + "' Category</h1></a>";


            
        }else
        {
            Response.Redirect("home");
        }


        
    }
}
