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
using System.Net;
using System.IO;

public partial class SearchElement : System.Web.UI.UserControl
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
    public bool IS_FEATURED
    {
        get { return isFeatured; }
        set { isFeatured = value; }
    }

    public bool IS_VENUE
    {
        get { return isVenue; }
        set { isVenue = value; }
    }
    private bool isVenue = false;
    private bool isFeatured = false;
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
    private string color = "White";
    private int eventID;
    private int width = 423;
    private string datelabel = "";
    private int venueID;
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn);

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
            ImageLiteral.Text = imageLiteral + "<div class=\"topDiv\" style=\"float: left;padding-right: 5px;height: 50px;\"><img style=\"cursor: " +
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

        bool imgAbsolute = false;
        VenueLabel.Text = venueLabel;

        if (!isVenue)
        {
            DataView dvEvent = dat.GetDataDV("SELECT * FROM Events WHERE ID=" + eventID);
            
            DataView dvEventsSlider = dat.GetDataDV("SELECT * FROM Events E, Event_Slider_Mapping ESM WHERE E.ID=ESM.EventID AND E.ID=" + eventID);
            if (dvEventsSlider.Count > 0)
            {
                if(bool.Parse(dvEventsSlider[0]["ImgPathAbsolute"].ToString()))
                    imgAbsolute = true;

                if (imgAbsolute)
                {
                    string name = dat.MakeNiceNameFull(dvEventsSlider[0]["PictureName"].ToString().Replace("/", "_").Replace(":", "_").Replace(".", "_"));

                    int i = 0;
                    for (i = name.Length - 1; i >= 0; i--)
                    {
                        if (name[i] == '_')
                        {
                            break;
                        }
                    }

                    Random rand = new Random(DateTime.Now.Millisecond);

                    name = dat.MakeNiceNameFive(dvEventsSlider[0]["PictureName"].ToString()) + "_" + eventID + "." + name.Substring(i + 1, name.Length - i - 1);


                    //if (name.Length > 230)
                    //    name = name.Substring(name.Length - 50, 50);

                    //if (!System.IO.File.Exists(MapPath("../") + "\\Temp\\Temp_" + name))
                    //{
                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(dvEventsSlider[0]["PictureName"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        Stream receiveStream = response.GetResponseStream();
                        // read the stream
                        System.Drawing.Image img = System.Drawing.Image.FromStream(receiveStream);
                        receiveStream.Close();
                        response.Close();

                        SaveThumbnail(img, MapPath("../") + "\\Temp\\Temp_" + name);
                        //}
                        EventImageLiteral.Text = "<img style='float: left; padding-right: 5px;' src='http://hippohappenings.com/Temp/Temp_" +
                            name + "' />";
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else
                {
                    if (!System.IO.File.Exists(MapPath("../") + "\\Temp\\Temp_" + dvEventsSlider[0]["PictureName"].ToString()))
                    {
                        if (System.IO.File.Exists(MapPath("../") + "\\UserFiles\\" +
                            dvEvent[0]["UserName"].ToString() + "\\Slider\\" + dvEventsSlider[0]["PictureName"].ToString()))
                        {
                            System.Drawing.Image img = System.Drawing.Image.FromFile(MapPath("../") + "\\UserFiles\\" +
                                dvEvent[0]["UserName"].ToString() + "\\Slider\\" + dvEventsSlider[0]["PictureName"].ToString());

                            SaveThumbnail(img, MapPath("../") + "\\Temp\\Temp_" + dvEventsSlider[0]["PictureName"].ToString());
                            EventImageLiteral.Text = "<img style='float: left; padding-right: 5px;' src='http://hippohappenings.com/Temp/Temp_" +
                        dvEventsSlider[0]["PictureName"].ToString() + "' />";
                        }
                    }

                    
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

            if (VenueLabel.Text.Length > 41)
            {
                VenueLabel.Text = VenueLabel.Text.Substring(0, 38) + "...";
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

            if (windowT)
            {

                SearchLabel.Attributes.Add("onclick", "CloseWindow('" + "../" +
                    dat.MakeNiceName(searchLabel) + "_" + eventID + "_Event" + "');");

                VenueLabel.Attributes.Add("onclick", "CloseWindow('" + "../" +
                    dat.MakeNiceName(venueLabel) + "_" + venueID + "_Venue" + "');");
            }
            else
            {

                SearchLabel.NavigateUrl = "../" + dat.MakeNiceName(searchLabel) + "_" + eventID + "_Event";

                VenueLabel.NavigateUrl = "../" + dat.MakeNiceName(venueLabel) +
                    "_" + venueID + "_Venue";
            }

            //if (datelabel.Trim() != "")
            //{
            //    char[] delim = { '/' };
            //    char[] delimer = { ' ' };
            //    string[] toks = datelabel.Split(delimer, StringSplitOptions.RemoveEmptyEntries);
            //    string[] tokens = toks[0].Split(delim, StringSplitOptions.RemoveEmptyEntries);

            //    if (tokens.Length > 1)
            //        DateLabel.Text = tokens[0] + "/" + tokens[1];

            //    if (toks.Length > 1)
            //        DateLabel.Text += " " + toks[1];

            //    if (datelabel.Contains("+ more dates"))
            //    {
            //        DateLabel.Text += " +";
            //    }
            //}

            DateLabel.Text = datelabel;

            if (isConnectTo && Session["User"] != null)
            {
                ConnectImageButton.Visible = true;
                ConnectImageButton.Attributes.Add("onclick", "OpenConnect('" + eventID + "', 'You have a Connect request for Event: " + searchLabel.Replace("'", "\\'") + "')");
            }
        }
        else
        {
            DataView dvEvent = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + venueID);

            DataView dvEventsSlider = dat.GetDataDV("SELECT * FROM Venues E, Venue_Slider_Mapping ESM "+
                "WHERE E.ID=ESM.VenueID AND E.ID=" + venueID);

            if (dvEventsSlider.Count > 0)
            {
                if (!System.IO.File.Exists(MapPath("../") + "\\Temp\\Temp_" + venueID + "_" +
                    dvEventsSlider[0]["PictureName"].ToString()))
                {
                    if (System.IO.File.Exists(MapPath("../") + "\\VenueFiles\\" +
                        venueID + "\\Slider\\" + dvEventsSlider[0]["PictureName"].ToString()))
                    {
                        System.Drawing.Image img = System.Drawing.Image.FromFile(MapPath("../") + "\\VenueFiles\\" +
                            venueID + "\\Slider\\" + dvEventsSlider[0]["PictureName"].ToString());

                        SaveThumbnail(img, MapPath("../") + "\\Temp\\Temp_" + venueID + "_" + dvEventsSlider[0]["PictureName"].ToString());
                        EventImageLiteral.Text = "<img style='float: left; padding-right: 5px;' src='http://hippohappenings.com/Temp/Temp_" + venueID + "_" +
                        dvEventsSlider[0]["PictureName"].ToString() + "' />";
                    }
                }

                
            }
            else
            {
                EventImageLiteral.Text = "";
            }

            string shortDesc = "";

            if (dvEvent[0]["Content"].ToString().Trim() != "")
            {
                shortDesc = dat.stripHTML(dvEvent[0]["Content"].ToString()).Trim();
                if (shortDesc.Length > 130)
                {
                    shortDesc = shortDesc.Substring(0, 127) + "...";
                }
            }

            if (VenueLabel.Text.Length > 45)
            {
                VenueLabel.Text = VenueLabel.Text.Substring(0, 42) + "...";
            }

            ShortDescriptionLabel.Text = dat.stripHTML(shortDesc);

            if (windowT)
            {

                SearchLabel.Attributes.Add("onclick", "CloseWindow('" + "../" +
                    dat.MakeNiceName(searchLabel) + "_" + venueID + "_Venue" + "');");

                VenueLabel.Attributes.Add("onclick", "CloseWindow('" + "../" +
                    dat.MakeNiceName(venueLabel) + "_" + venueID + "_Venue" + "');");
            }
            else
            {

                SearchLabel.NavigateUrl = "../" + dat.MakeNiceName(searchLabel) + "_" + venueID + "_Venue";

                VenueLabel.NavigateUrl = "../" + dat.MakeNiceName(venueLabel) +
                    "_" + venueID + "_Venue";
            }
        }

        if (searchLabel.Length > 36)
        {
            searchLabel = searchLabel.Substring(0, 33) + "...";
        }

        SearchLabel.Text = searchLabel;
    }

    private void SaveThumbnail(System.Drawing.Image image, string path)
    {
        try
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
        catch (Exception ex)
        {

        }
    }
}
