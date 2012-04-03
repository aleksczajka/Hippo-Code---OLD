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
    protected bool windowT = false;
    private string tags = "";
    private string searchLabel = "";

    private string numLabel = "";
    private string color = "#333333; border: solid 1px #cccccc;";
    private int adID;
    private int width = 418;
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        BeginingLiteral.Text = "<div style=\"background-color:" + 
            color + "; width: " + width.ToString() + "px; padding: 3px; margin-bottom: 3px;\">";
        SearchLabel.Text = searchLabel;
        //search = true is disabled here
        
        if (windowT)
        {
            SearchLabel.Attributes.Add("onclick", "CloseWindow('" + "../" + dat.MakeNiceName(searchLabel) + "_" + adID + "_Ad" + "');");
        }
        else
        {
            SearchLabel.NavigateUrl = "../" + dat.MakeNiceName(searchLabel) + "_" + adID + "_Ad";
        }
        
        string begining = "<a style=\"color: #b1a812; font-family: Arial; font-size: 11px;\">";
        string end = "</a>";


        dat.TAG_TYPE = Data.tagType.AD;
        string tags = dat.getTags(dat.GetData("SELECT C.ID, C.Name AS CategoryName, ACM.tagSize FROM Ad_Category_Mapping ACM, AdCategories C WHERE ACM.CategoryID=C.ID AND ACM.AdID=" + adID), false, windowT);

        TagsLiteral.Text = "<span style=\"color: #b1a812; line-height: 10px; font-size: 11px;\" >" + tags + "</span>";

    }

}
