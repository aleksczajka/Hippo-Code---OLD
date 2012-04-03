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
using System.Data.SqlClient;
using Telerik.Web.UI;

public partial class SearchResults : System.Web.UI.Page
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
        //Ajax.Utility.RegisterTypeForAjax(typeof(EventCommunicate));
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        if (!IsPostBack)
        {
            if (Session["SearchResults"] != null)
            {
                ResultsLabel.Text = Session["SearchResults"].ToString();
            }

            HtmlLink lk = new HtmlLink();
            HtmlHead head = (HtmlHead)Page.Header;
            lk.Attributes.Add("rel", "canonical");
            lk.Href = "http://hippohappenings.com/SearchResults.aspx";
            head.Controls.AddAt(0, lk);
            if (Session["Searching"] != null)
            {
                if (Session["Searching"] == "Events")
                {
                    if (Session["sortString"] != null)
                        SortDropDown.SelectedValue = Session["sortString"].ToString();
                }
                else
                {
                    SortDropDown.Visible = false;
                }
            }
        }
        try
        {
            if (Session["SearchDS"] != null)
            {
                DataSet ds;
                ds = (DataSet)Session["SearchDS"];
                NumsLabel.Text = ds.Tables[0].Rows.Count + " Results Found";
                switch (Session["Searching"].ToString())
                {
                    case "Events":
                        EventSearchElements.Visible = true;
                        EventSearchElements.EVENTS_DS = ds;
                        EventSearchElements.IS_WINDOW = true;
                        if (Session["sortString"] != null)
                            EventSearchElements.SORT_STR = Session["sortString"].ToString();
                        EventSearchElements.DataBind2();
                        break;
                    case "Venues":
                        VenueSearchElements.Visible = true;
                        VenueSearchElements.VENUE_DS = ds;
                        VenueSearchElements.IS_WINDOW = true;
                        VenueSearchElements.DataBind2();
                        break;
                    case "Ads":
                        AdSearchElements.Visible = true;
                        AdSearchElements.AD_DS = ds;
                        AdSearchElements.IS_WINDOW = true;
                        AdSearchElements.DataBind2();
                        break;
                    case "Groups":
                        GroupSearchElements.Visible = true;
                        GroupSearchElements.GROUP_DS = ds;
                        GroupSearchElements.IS_WINDOW = true;
                        GroupSearchElements.DataBind2();
                        break;
                    default: break;
                }
            }
            else
            {
                NumsLabel.Text = "0 Results Found";
                SortDropDown.Visible = false;
                switch (Session["Searching"].ToString())
                {
                    case "Events":
                        HelpUsLiteral.Text = "<label>There were no events found matching your "+
                            "search criteria. But you can help us by posting events at "+
                            "<a class=\"AddLink\" onclick=\"CloseWindow('BlogEvent.aspx')\">Blog an Event</a></label>";
                        break;
                    case "Venues":
                        HelpUsLiteral.Text = "<label>There were no venues found matching your " +
                                                    "search criteria. But you can help us by entering more venues at " +
                                                    "<a class=\"AddLink\" onclick=\"CloseWindow('EnterVenue.aspx')\">Submit a Venue</a></label>"; 
                        break;
                    case "Ads":
                        HelpUsLiteral.Text = "<label>There were no classifieds found matching your " +
                                                    "search criteria. But you can help us by posting classifieds at " +
                                                    "<a class=\"AddLink\" onclick=\"CloseWindow('PostAnAd.aspx')\">Post a Classified</a></label>"; 
                        break;
                    case "Groups":
                        HelpUsLiteral.Text = "<label>There were no public groups found matching your " +
                                                    "search criteria. But you can help us by starting your own groups at " +
                                                    "<a class=\"AddLink\" onclick=\"CloseWindow('EnterGroup.aspx')\">Make a Group</a></label>"; 
                        break;
                    default: break;
                }
                
                
            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString();
        }
    }

    protected void SortResults(object sender, EventArgs e)
    {
        try
        {
            if (Session["SearchDS"] != null)
            {
                
                if (SortDropDown.SelectedValue != "-1")
                {
                    Session["sortString"] = SortDropDown.SelectedValue;

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

                    ds = (DataSet)Session["SearchDS"];
                    EventSearchElements.EVENTS_DS = ds;
                    EventSearchElements.IS_WINDOW = true;
                    EventSearchElements.SORT_STR = Session["sortString"].ToString();
                    EventSearchElements.DataBind2();
                }
            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString();
        }
    }
}
