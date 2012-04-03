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

public partial class AdSearchElement : System.Web.UI.UserControl
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
    public int AD_ID
    {
        get { return adID; }
        set { adID = value; }
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
    public bool IS_FEATURED
    {
        get { return isFeatured; }
        set { isFeatured = value; }
    }
    protected bool isFeatured = false;
    protected bool windowT = false;
    private string tags = "";
    private string searchLabel = "";

    private string numLabel = "";
    private string color = "White";
    private int adID;
    private int width = 418;
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn);
        string background = "";
        if (isFeatured)
        {
            background = "background-color: #e9f1f4;";
        }
        else
        {
            background = "border-bottom: solid 1px #dedbdb;";
        }

        BeginingLiteral.Text = "<div style=\"" + background + " width: " + width.ToString() +
            "px; padding: 3px; margin-bottom: 3px; height: 56px;\">";
        
        //search = true is disabled here
        
        if (windowT)
        {
            SearchLabel.Attributes.Add("onclick", "CloseWindow('" + "../" + dat.MakeNiceName(searchLabel) + "_" + adID + "_Ad" + "');");
        }
        else
        {
            SearchLabel.NavigateUrl = "../" + dat.MakeNiceName(searchLabel) + "_" + adID + "_Ad";
        }

        if (searchLabel.Length > 36)
        {
            searchLabel = searchLabel.Substring(0, 33) + "...";
        }
        SearchLabel.Text = searchLabel;

        DataView dvEvent = dat.GetDataDV("SELECT * FROM Ads WHERE Ad_ID=" + adID);

        string shortDesc = "";

        if (dvEvent[0]["Description"].ToString().Trim() != "")
        {
            shortDesc = dat.stripHTML(dvEvent[0]["Description"].ToString()).Trim();
            if (shortDesc.Length > 123)
            {
                shortDesc = shortDesc.Substring(0, 120) + "...";
            }
        }

        ShortDescriptionLabel.Text = dat.stripHTML(shortDesc);


        DataView dvEventsSlider = dat.GetDataDV("SELECT * FROM Ads E, Users U, Ad_Slider_Mapping ESM " +
                    "WHERE U.User_ID=E.User_ID AND E.Ad_ID=ESM.AdID AND E.Ad_ID=" + adID);
        if (dvEventsSlider.Count > 0)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(MapPath("../") + "\\UserFiles\\" +
                dvEventsSlider[0]["UserName"].ToString() + "\\AdSlider\\" +
                adID + "\\" + dvEventsSlider[0]["PictureName"].ToString());

            SaveThumbnail(img, MapPath("../") + "\\Temp\\Temp_" + dvEventsSlider[0]["PictureName"].ToString());
            EventImageLiteral.Text = "<img style='float: left; padding-right: 5px;' src='http://hippohappenings.com/Temp/Temp_" +
                dvEventsSlider[0]["PictureName"].ToString() + "' />";
        }
        else
        {
            EventImageLiteral.Text = "";
        }

        dat.TAG_TYPE = Data.tagType.AD;

        int tagLength = 0;

        if (ShortDescriptionLabel.Text.Length > 64)
        {
            tagLength = 57;
        }
        else if (ShortDescriptionLabel.Text.Length == 0)
        {
            tagLength = 183;
        }
        else
        {
            tagLength = 122;
        }

        string tags = dat.getTagsRestricted(dat.GetData("SELECT DISTINCT C.ID, C.Name AS " +
            "CategoryName, VC.tagSize FROM Ad_Category_MAPPING VC, AdCategories " +
            "C WHERE VC.CategoryID=C.ID AND VC.AdID=" + adID), false, windowT, tagLength);


        TagsLiteral.Text = tags;
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
}
