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

public partial class Controls_SearchElement : System.Web.UI.UserControl
{
    public string SEARCH_LABEL
    {
        get { return SearchLabel.Text; }
        set { searchLabel = value; }
    }
    public string DATE_LABEL
    {
        get { return datelabel; }
        set { datelabel = value; }
    }
    public string VENUE_LABEL
    {
        get { return VenueLabel.Text; }
        set { venueLabel = value; }
    }
    public string TAGS
    {
        get { return tags; }
        set { tags = value; }
    }
    public string NUM_LABEL
    {
        get { return NumLabel.Text; }
        set { numLabel = value; }
    }
    public string COLOR
    {
        get { return color; }
        set { color = value; }
    }
    public bool IS_CONNECT_TO
    {
        get { return isConnectTo; }
        set { isConnectTo = value; }
    }
    public int EVENT_ID
    {
        get { return eventID; }
        set { eventID = value; }
    }
    public int VENUE_ID
    {
        get { return venueID; }
        set { venueID = value; }
    }
    public int WIDTH
    {
        get { return width; }
        set { width = value; }
    }
    public bool IS_WINDOW
    {
        get { return windowT; }
        set { windowT = value; }
    }
    public bool IS_GROUP
    {
        get { return isGroup; }
        set { isGroup = value; }
    }
    public string REOCCURR_ID
    {
        get { return reoccurrID; }
        set { reoccurrID = value; }
    }
    public string ADDRESS
    {
        get { return address; }
        set { address = value; }
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
    public string CALENDAR_LETTER
    {
        get { return calendarLetter; }
        set { calendarLetter = value; }
    }
    public string FIRST_DATE
    {
        get { return firstDate; }
        set { firstDate = value; }
    }
    public string TOOL_TIP_ID
    {
        get { return toolTipID; }
        set { toolTipID = value; }
    }
    public int NUM_OF_ALL_RESULTS
    {
        get { return numofallresults; }
        set { numofallresults = value; }
    }
    private int numofallresults = 0;
    private string toolTipID = "";
    private string firstDate = "";
    private string calendarLetter = "";
    private string imageLiteral = "";
    private string searchNumber = "";
    private string address = "";
    private string reoccurrID = "";
    private bool isGroup = false;
    protected bool windowT = false;
    private bool isConnectTo = false;
    private string tags = "";
    private string searchLabel = "";
    private string venueLabel = "";
    private string numLabel = "";
    private string color = "#333333; border: solid 1px #cccccc;";
    private int eventID;
    private int width = 410;
    private string datelabel = "";
    private int venueID;
    private string borderColor = "#1b1b1b";
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        if (IsPostBack)
        {
            MessageRadWindowManager.VisibleOnPageLoad = false;
        }

        //take care of mapping to the search map

        if (searchNumber != "")
        {
            string markerNumber = dat.GetImageNum(searchNumber); // (int.Parse(dat.GetImageNum(searchNumber)) + numofallresults).ToString();
            ImageLiteral.Text = imageLiteral + "<div style=\"float: left;\"><img style=\"cursor: pointer;\" onclick=\"selectFirstTab();myclick(" + markerNumber + ");\" id='image" + searchNumber + "' src=" +
                "\"http://www.google.com/mapfiles/marker" + searchNumber + ".png\" /></div>";
            //SearchLiteral.Text += "<script type=\"text/javascript\">var letter = String.fromCharCode(\"A\".charCodeAt(0) + " +
            //    searchNumber + "); var letteredIcon = new GIcon(baseIcon); document.getElementById('image" + searchNumber +
            //    "').src = \"http://www.google.com/mapfiles/marker\" + letter + \".png\";</script>";
            BeginingLiteral.Text = "<div style=\"margin-bottom: 3px;background-color:" +
                color + "; width: " + width.ToString() + "px; min-height: 33px; padding: 3px;\">";
        }
        else
        {
            BeginingLiteral.Text = "<div style=\"margin-bottom: 3px;background-color:" + color + "; width: " + width.ToString() + "px;padding: 3px;\">";
        }

        if (calendarLetter != "")
        {
            ImageLiteral.Text += "<div onclick=\"selectCalendarDate('"+firstDate+"', '"+toolTipID+"');\" align=\"center\" style=\"cursor: pointer;margin-right: 3px;margin-left: 5px; margin-top: 6px;font-weight: bold;width: 20px; height: 14px;color: #1b1b1b;font-size: " +
                "10px; padding-top: 6px;float: left; background-image: url(images/CalendarIcon.png);" +
                "\">" + calendarLetter + "</div>";
        }

        if (isGroup)
        {
            ImageLiteral.Text += "<div style=\"float: left; height: 30px; margin-top: -4px\"><img height=\"40px\" title=\"Group Event\" " +
                "name=\"Group Event\" src=\"images/GroupsIcon2.png\" /></div>";
        }

        //search = true is disabled here
        SearchLabel.Text = searchLabel;
        if (venueLabel.Trim() != "")
            VenueLabel.Text = venueLabel;
        else
        {
            VenueRealLabel.Text = address;
            VenueRealLabel.Style.Add("color", "#cccccc");
            VenueRealLabel.Style.Add("font-weight", "normal");
            VenueRealLabel.Style.Add("pointer", "default");
            VenueRealLabel.Style.Add("font-family", "Arial");
            VenueRealLabel.Style.Add("font-size", "12px");
        }

        if (windowT)
        {
            if (isGroup)
                SearchLabel.Attributes.Add("onclick", "CloseWindow('" + "../" +
                    dat.MakeNiceName(searchLabel) + "_" + reoccurrID + "_" + eventID + "_GroupEvent" + "');");
            else
                SearchLabel.Attributes.Add("onclick", "CloseWindow('" + "../" +
                    dat.MakeNiceName(searchLabel) + "_" + eventID + "_Event" + "');");
            
            if (venueLabel.Trim() != "")
                VenueLabel.Attributes.Add("onclick", "CloseWindow('" + "../" +
                    dat.MakeNiceName(venueLabel) + "_" + venueID + "_Venue" + "');");
        }
        else
        {
            if (isGroup)
                SearchLabel.NavigateUrl = "../" + dat.MakeNiceName(searchLabel) + "_" +
                    reoccurrID + "_" + eventID + "_GroupEvent";
            else
                SearchLabel.NavigateUrl = "../" + dat.MakeNiceName(searchLabel) + "_" + eventID + "_Event";

            if (venueLabel.Trim() != "")
                VenueLabel.NavigateUrl = "../" + dat.MakeNiceName(venueLabel) +
                    "_" + venueID + "_Venue";
        }
        //if(numLabel != "")
        //    NumLabel.Text = numLabel;
        DateLabel.Text = datelabel;
        string begining = "<a style=\"color: #b1a812; font-family: Arial; font-size: 11px;\">";
        string end = "</a>";

        string tags = "";
        if (isGroup)
        {
            dat.TAG_TYPE = Data.tagType.GROUP_EVENT;
            DataSet ds1 = dat.GetData("SELECT DISTINCT C.ID, C.GroupName AS CategoryName, '22px' AS tagSize  FROM GroupEvent_Category VC, GroupCategories C WHERE C.ID=VC.CATEGORY_ID AND VC.GROUPEVENT_ID=" + eventID);
            tags = dat.getTags(ds1, false, windowT);
        }
        else
        {
            dat.TAG_TYPE = Data.tagType.EVENT;
            tags = dat.getTags(dat.GetData("SELECT C.ID, C.Name AS CategoryName, ECM.tagSize FROM Event_Category_Mapping ECM, EventCategories C WHERE ECM.CategoryID=C.ID AND ECM.EventID=" + eventID), false, windowT);
            DataSet ds = dat.GetData("SELECT ShortDescription FROM Events WHERE ID=" + eventID);
            //ShortDescriptionLabel.Text = dat.BreakUpString(ds.Tables[0].Rows[0]["ShortDescription"].ToString(), 300);
        }


        //if (Session["User"] != null && !isGroup)
        //{
        //    dat.GetRecommendationIcons(eventID.ToString(), ref RecomPanel);

        //}

        TagsLiteral.Text = "<span style=\"color: #b1a812; line-height: 10px; font-size: 11px;\" >" + tags + "</span>";

        if (isConnectTo)
        {
            ConnectImageButton.Visible = true;
        }


    }



    protected void Connect(object sender, EventArgs e)
    {
        
        Session["Type"] = "Connect";
        Session["Subject"] = "You have a Connect request for Event: " + searchLabel;
        //MessageLiteral.Text = "<script type=\"text/javascript\">alert('" + message + "');</script>";

        MessageRadWindow.NavigateUrl = "MessageAlert.aspx?T=Connect&A=friend&ID="+Request.QueryString["ID"].ToString();
        MessageRadWindow.Visible = true;

        MessageRadWindowManager.VisibleOnPageLoad = true;
    }

}
