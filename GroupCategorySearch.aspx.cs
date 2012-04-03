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

public partial class GroupCategorySearch : Telerik.Web.UI.RadAjaxPage
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
                location = " AND G.State='" + dvPrefs[0]["CatState"].ToString() + "' AND G.City='" + 
                    dvPrefs[0]["CatCity"].ToString() + "'";
            }
            else
            {
                DataView dvIP = dat.GetDataDV("SELECT * FROM Users U, UserPreferences UP WHERE UP.UserID=U.User_ID AND U.IPs LIKE '%;" + dat.GetIP() + ";%'");

                //If one or multiple found, look up ads on city and state of the first record
                if (dvIP.Count > 0)
                {
                    location = " AND G.State='" + dvIP[0]["CatState"].ToString() +
                        "' AND G.City='" + dvIP[0]["CatCity"].ToString() + "'";
                }
                //If not found look in searches table, get the city and state
                else
                {
                    DataView dvSearch = dat.GetDataDV("SELECT * FROM SearchIPs WHERE IP='" + dat.GetIP() + "'");
                    location = " AND G.State='" + dvSearch[0]["State"].ToString() +
                        "' AND G.City='" + dvSearch[0]["City"].ToString() + "'";
                }
            }

            ds = dat.GetData("SELECT * FROM Groups G, Group_Category GC, GroupCategories GCC WHERE GCC.ID=" +
                catID + " AND GC.Category_ID=GCC.ID AND G.ID=GC.Group_ID " + location);

            SearchElements.GROUP_DS = ds;
            SearchElements.DataBind2();

            NumResultsLabel.Text = "(" + ds.Tables[0].Rows.Count + " Records Found)";

            string theLink = "http://" + Request.Url.Authority + "/" +
            dat.MakeNiceName(ds.Tables[0].Rows[0]["GroupName"].ToString()) + "_" + catID + "_Group_Category";

            HtmlLink lk = new HtmlLink();
            lk.Href = theLink;
            lk.Attributes.Add("rel", "bookmark");
            head.Controls.AddAt(0, lk);

            HtmlMeta hm = new HtmlMeta();
            HtmlMeta kw = new HtmlMeta();
            HtmlMeta dc = new HtmlMeta();

            kw.Name = "keywords";
            kw.Content = ds.Tables[0].Rows[0]["GroupName"].ToString() + " Group Category";

            head.Controls.AddAt(0, kw);

            dc.Name = "description";
            dc.Content = "Results for group category: " + ds.Tables[0].Rows[0]["GroupName"].ToString();

            head.Controls.AddAt(0, dc);

            SearchResultsTitleLabel.Text = "<a href=\"" + theLink + "\" style=\"text-decoration: none; color: white;\" >Groups in '" +
                ds.Tables[0].Rows[0]["GroupName"].ToString() + "' Category</a>";
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
