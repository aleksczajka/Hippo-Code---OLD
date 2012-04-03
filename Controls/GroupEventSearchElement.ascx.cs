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

public partial class GroupEventSearchElement : System.Web.UI.UserControl
{
    public string SEARCH_LABEL
    {
        get { return SearchLabel.Text; }
        set { searchLabel = value; }
    }
    public string TAGS
    {
        get { return tags; }
        set { tags = value; }
    }

    public string COLOR
    {
        get { return color; }
        set { color = value; }
    }
    public int Venue_ID
    {
        get { return venueID; }
        set { venueID = value; }
    }
    public int WIDTH
    {
        get { return width; }
        set { width = value; }
    }
    public string CITY
    {
        get { return city; }
        set { city = value; }
    }
    public string STATE
    {
        get { return state; }
        set { state = value; }
    }
    public bool IS_WINDOW
    {
        get { return windowT; }
        set { windowT = value; }
    }
    protected bool windowT = false;
    private string tags = "";
    private string searchLabel = "";

    private string numLabel = "";
    private string color = "#333333; border: solid 1px #cccccc;";
    private int venueID;
    private int width = 410;
    private string city = "";
    private string state = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        BeginingLiteral.Text = "<div style=\"background-color:" + color + "; width: " + width.ToString() + 
            "px; height: 78px; padding: 3px; margin-bottom: 3px;\">";
        SearchLabel.Text = searchLabel;

        //search = true is disabled here
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataView dv = dat.GetDataDV("SELECT GEO.ID FROM GroupEvents GE, GroupEvent_Occurance GEO WHERE GE.ID=GEO.GroupEventID AND GE.ID=" + venueID);
        if (windowT)
        {
            SearchLabel.Attributes.Add("onclick", "CloseWindow('" + "../" + dat.MakeNiceName(searchLabel) +
                "_" + dv[0]["ID"].ToString() + "_" + venueID + "_GroupEvent" + "');");
        }
        else
        {
            SearchLabel.NavigateUrl = "../" + dat.MakeNiceName(searchLabel) + "_" + dv[0]["ID"].ToString() + "_" + venueID + "_GroupEvent";
        }

        string begining = "<div style=\"color: #b1a812; font-family: Arial; font-size: 11px;\">";
        string end = "</div>";
        TagsLiteral.Text = begining + city + ", " + state + end;

        dat.TAG_TYPE = Data.tagType.GROUP_EVENT;
        
        DataSet ds = dat.GetData("SELECT DISTINCT C.ID, C.GroupName AS CategoryName, '22px' AS tagSize  FROM GroupEvent_Category VC, GroupCategories C WHERE C.ID=VC.CATEGORY_ID AND VC.GROUPEVENT_ID=" + venueID);
        string tags = dat.getTags(ds, false, windowT);
        TagsLiteral.Text += "<span style=\"color: #b1a812; line-height: 10px; font-size: 11px;\" >" + tags + "</span>";

    }


}
