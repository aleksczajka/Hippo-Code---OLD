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
                DataView dvIP = dat.GetDataDV("SELECT * FROM Users U, UserPreferences UP WHERE UP.UserID=U.User_ID AND U.IPs LIKE '%;" + dat.GetIP() + ";%'");

                //If one or multiple found, look up ads on city and state of the first record
                if (dvIP.Count > 0)
                {
                    location = " AND V.State='" + dvIP[0]["CatState"].ToString() +
                        "' AND V.City='" + dvIP[0]["CatCity"].ToString() + "'";
                }
                //If not found look in searches table, get the city and state
                else
                {
                    DataView dvSearch = dat.GetDataDV("SELECT * FROM SearchIPs WHERE IP='" + dat.GetIP() + "'");
                    location = " AND V.State='" + dvSearch[0]["State"].ToString() +
                        "' AND V.City='" + dvSearch[0]["City"].ToString() + "'";
                }
            }

            //get only events after today, since showing all events might be too much
            string timeline = " AND EO.DateTimeStart >= CONVERT(DATETIME, '" + today.Month.ToString() + "/" + today.Day.ToString() + "/" + today.Year.ToString() + "')";


            string command = "SELECT E.ID AS EID, EO.ID AS ReoccurrID, EO.DateTimeStart, 'False' AS isGroup, CONVERT(NVARCHAR, E.ID)+'E' AS HashID, C.Name AS CategoryName, V.ID AS VID, * " +
                "FROM Event_Occurance EO, Event_Category_Mapping ECM, Events E, Venues V, EventCategories C " +
                "WHERE EO.EventID=E.ID AND C.ID=ECM.CategoryID AND E.Venue=V.ID AND ECM.EventID=E.ID AND " +
                "ECM.CategoryID=" + catID + location + timeline;
            
            ds = dat.GetData(command);
            SearchElements.EVENTS_DS = ds;
            SearchElements.DataBind2();

            NumResultsLabel.Text = "(" + ds.Tables[0].Rows.Count + " Records Found)";

            DataView dvName = dat.GetDataDV("SELECT * FROM EventCategories WHERE ID=" + catID);

            HtmlHead head = (HtmlHead)Page.Header;

            string theLink = "http://" + Request.Url.Authority + "/" +
               dvName[0]["Name"].ToString() + "_Event_Category";

            HtmlLink lk = new HtmlLink();
            lk.Href = theLink;
            lk.Attributes.Add("rel", "bookmark");
            head.Controls.AddAt(0, lk);

            HtmlMeta hm = new HtmlMeta();
            HtmlMeta kw = new HtmlMeta();
            HtmlMeta dc = new HtmlMeta();

            kw.Name = "keywords";
            kw.Content = dvName[0]["Name"].ToString() + " event Category";

            head.Controls.AddAt(0, kw);

            dc.Name = "description";
            dc.Content = "Results for event category: " + dvName[0]["Name"].ToString();

            head.Controls.AddAt(0, dc);


            SearchResultsTitleLabel.Text = "<a href=\"" + theLink +
                "\" style=\"text-decoration: none; color: white;\" >Events in '" +
                dvName[0]["Name"].ToString() + "' Category</a>";


            
        }else
        {
            Response.Redirect("Home.aspx");
        }


        
    }

    protected void Page_Init(object sender, EventArgs e)
    {

    }
}
