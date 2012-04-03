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

public partial class VenueSearchElement : System.Web.UI.UserControl
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
    public string SEARCH_MAP_NUM
    {
        get { return searchNumber; }
        set { searchNumber = value; }
    }

    public string IMAGE_LITERAL
    {
        get { return imageLiteral; }
        set { imageLiteral = value; }
    }

    public int NUM_OF_ALL_RESULTS
    {
        get { return numofallresults; }
        set { numofallresults = value; }
    }
    private int numofallresults = 0;
    private string searchNumber = "";
    protected bool windowT = false;
    private string tags = "";
    private string searchLabel = "";
    private string imageLiteral = "";
    private string numLabel = "";
    private string color = "#333333; border: solid 1px #cccccc;";
    private int venueID;
    private int width = 410;
    private string city = "";
    private string state = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        if (searchNumber != "")
        {
            string markerNumber = dat.GetImageNum(searchNumber);  //(int.Parse(dat.GetImageNum(searchNumber)) + numofallresults).ToString();
            ImageLiteral.Text = imageLiteral + "<div style=\"float: left;\"><img style=\"cursor: pointer;\" onclick=\"myclick(" + markerNumber + ");\" src=" +
                "\"http://www.google.com/mapfiles/marker" + searchNumber + ".png\" /></div>";
            //SearchLiteral.Text += "<script type=\"text/javascript\">var letter = String.fromCharCode(\"A\".charCodeAt(0) + " +
            //    searchNumber + "); var letteredIcon = new GIcon(baseIcon); document.getElementById('image" + searchNumber +
            //    "').src = \"http://www.google.com/mapfiles/marker\" + letter + \".png\";</script>";
            BeginingLiteral.Text = "<div style=\";background-color:" + color + "; min-height: 33px; width: " + width.ToString() +
                "px; padding: 3px; margin-bottom: 3px;\">";
        }
        else
        {
            BeginingLiteral.Text = "<div style=\"background-color:" + color + "; width: " + width.ToString() +
                "px; padding: 3px; margin-bottom: 3px;\">";
        }

        SearchLabel.Text = searchLabel;

        //search = true is disabled here
        
        if (windowT)
        {
            SearchLabel.Attributes.Add("onclick", "CloseWindow('" + "../" + dat.MakeNiceName(searchLabel) + "_" + venueID + "_Venue" + "');");
        }
        else
        {
            SearchLabel.NavigateUrl = "../" + dat.MakeNiceName(searchLabel) + "_" + venueID + "_Venue";
        }

        string begining = "<div style=\"color: #b1a812; font-family: Arial; font-size: 11px;\">";
        string end = "</div>";
        TagsLiteral.Text = begining + city + ", " + state + end;

        dat.TAG_TYPE = Data.tagType.VENUE;
        
        string tags = dat.getTags(dat.GetData("SELECT DISTINCT C.ID, C.Name AS CategoryName, VC.tagSize FROM Venue_Category VC, VenueCategories C WHERE VC.CATEGORY_ID=C.ID AND VC.VENUE_ID=" + venueID), false, windowT);
        TagsLiteral.Text += "<span style=\"color: #b1a812; line-height: 10px; font-size: 11px;\" >" + tags + "</span>";

    }


}
