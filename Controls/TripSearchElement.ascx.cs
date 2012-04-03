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

public partial class TripSearchElement : System.Web.UI.UserControl
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
    public bool IS_FEATURED
    {
        get { return isFeatured; }
        set { isFeatured = value; }
    }
    public string PRICE_MEANS_TIMEFRAME
    {
        get { return priceMeansTimeFrame; }
        set { priceMeansTimeFrame = value; }
    }
    private string priceMeansTimeFrame = "";
    private bool isFeatured = false;
    private int numofallresults = 0;
    private string toolTipID = "";
    private string firstDate = "";
    private string calendarLetter = "";
    private string imageLiteral = "";
    private string searchNumber = "";
    private string address = "";
    protected bool windowT = false;
    private bool isConnectTo = false;
    private string tags = "";
    private string searchLabel = "";
    private string numLabel = "";
    private string color = "White";
    private int eventID;
    private int width = 423;
    private string datelabel = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn); 

        if (IsPostBack)
        {
            MessageRadWindowManager.VisibleOnPageLoad = false;
        }

        string background = "";
        if (isFeatured)
        {
            background = "background-color: #e9f1f4;";
        }
        else
        {
            background = "border-bottom: solid 1px #dedbdb;";
        }

        if (searchNumber != "")
        {
            string markerNumber = dat.GetImageNum(searchNumber);
            ImageLiteral.Text = imageLiteral + "<div style=\"float: left;padding-right: 5px;height: 50px;\"><img style=\"cursor: " +
                "pointer;\" onclick=\"myclick(" + markerNumber + ");\" id='image" + searchNumber + "' src=" +
                "\"http://www.google.com/mapfiles/marker" + searchNumber + ".png\" /></div>";
            BeginingLiteral.Text = "<div style=\"margin-top: 3px;height: 56px;margin-bottom: 3px; width: " + width.ToString() +
                "px; padding: 3px;" + background + "\">";
        }
        else
        {
            BeginingLiteral.Text = "<div class=\"topDiv\" style=\"margin-top: 3px;margin-bottom: 3px;" + background +
                " width: " + width.ToString() + "px;padding: 3px;\">";
        }

        if (windowT)
        {
            SearchLabel.Attributes.Add("onclick", "CloseWindow('" + "../" +
                dat.MakeNiceName(searchLabel) + "_" + eventID + "_Trip" + "');");
        }
        else
        {
            SearchLabel.NavigateUrl = "../" + dat.MakeNiceName(searchLabel) + "_" + eventID + "_Trip";
        }

        if (searchLabel.Length > 36)
        {
            searchLabel = searchLabel.Substring(0, 33) + "...";
        }

        SearchLabel.Text = searchLabel;

        PriceRangeLabel.Text = priceMeansTimeFrame;

        DataView dvEvent = dat.GetDataDV("SELECT * FROM Trips WHERE ID=" + eventID);

        DataView dvEventsSlider = dat.GetDataDV("SELECT * FROM Trips E, Trip_Slider_Mapping ESM WHERE E.ID=ESM.TripID AND E.ID=" + eventID);
        if (dvEventsSlider.Count > 0)
        {
            if (System.IO.File.Exists(MapPath("../") + "\\Trips\\" +
                eventID + "\\Slider\\" + dvEventsSlider[0]["PictureName"].ToString()))
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(MapPath("../") + "\\Trips\\" +
                    eventID + "\\Slider\\" + dvEventsSlider[0]["PictureName"].ToString());

                SaveThumbnail(img, MapPath("../") + "\\Temp\\Temp_" + dvEventsSlider[0]["PictureName"].ToString());
                EventImageLiteral.Text = "<img style='float: left; padding-right: 5px;' src='http://hippohappenings.com/Temp/Temp_" +
                    dvEventsSlider[0]["PictureName"].ToString() + "' />";
            }
        }
        else
        {
            EventImageLiteral.Text = "";
        }

        string shortDesc = "";

        if (dvEvent[0]["ShortDescription"].ToString().Trim() != "")
        {
            shortDesc = dat.stripHTML(dvEvent[0]["ShortDescription"].ToString()).Trim();
            if (shortDesc.Length > 130)
            {
                shortDesc = shortDesc.Substring(0, 127) + "...";
            }
        }


        if (shortDesc.Trim() == "")
        {
            shortDesc = dat.stripHTML(dvEvent[0]["Content"].ToString()).Trim();
            if (shortDesc.Length > 130)
            {
                shortDesc = shortDesc.Substring(0, 127) + "...";
            }
        }

        ShortDescriptionLabel.Text = dat.stripHTML(shortDesc);

        

        if (isConnectTo)
        {
            ConnectImageButton.Visible = true;
        }
    }

    private void SaveThumbnail(System.Drawing.Image image, string path)
    {
        int width = 63;
        int height = 54;

        int newHeight = 0;
        int newIntWidth = 0;

        float newFloatHeight = 0.00F;
        float newFloatWidth = 0.00F;

        //if image height is less than resize height
        if (height >= image.Height)
        {
            //leave the height as is
            newHeight = image.Height;

            if (width >= image.Width)
            {
                newIntWidth = image.Width;
                newFloatWidth = image.Width;
            }
            else
            {
                newIntWidth = width;

                double theDivider = double.Parse(image.Width.ToString()) / double.Parse(newIntWidth.ToString());
                double newDoubleHeight = double.Parse(newHeight.ToString());
                newDoubleHeight = double.Parse(height.ToString()) / theDivider;
                newHeight = (int)newDoubleHeight;
                newFloatHeight = (float)newDoubleHeight;
            }
        }
        //if image height is greater than resize height...resize it
        else
        {
            //make height equal to the requested height.
            newHeight = height;
            newFloatHeight = height;
            //get the ratio of the new height/original height and apply that to the width
            double theDivider = double.Parse(image.Height.ToString()) / double.Parse(newHeight.ToString());
            double newDoubleWidth = double.Parse(newIntWidth.ToString());
            newDoubleWidth = double.Parse(image.Width.ToString()) / theDivider;
            newIntWidth = (int)newDoubleWidth;
            newFloatWidth = (float)newDoubleWidth;
            //if the resized width is still to big
            if (newIntWidth > width)
            {
                //make it equal to the requested width
                newIntWidth = width;

                //get the ratio of old/new width and apply it to the already resized height
                theDivider = double.Parse(image.Width.ToString()) / double.Parse(newIntWidth.ToString());
                double newDoubleHeight = double.Parse(newHeight.ToString());
                newDoubleHeight = double.Parse(image.Height.ToString()) / theDivider;
                newHeight = (int)newDoubleHeight;
                newFloatHeight = (float)newDoubleHeight;
            }
        }


        System.Drawing.Bitmap bmpResized = new System.Drawing.Bitmap(image, newIntWidth, newHeight);

        bmpResized.Save(path);
        //thumbnail.Save(path);
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
