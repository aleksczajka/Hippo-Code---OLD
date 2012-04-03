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

public partial class Controls_TagCloud : System.Web.UI.UserControl
{
    public enum tagType { AD, EVENT, VENUE, GROUP, GROUP_EVENT };
    public int THE_ID
    {
        get { return theID; }
        set { theID = value; }
    }
    public tagType TAG_TYPE
    {
        get { return tag_type; }
        set { tag_type = value; }
    }

    private int theID;
    private tagType tag_type;
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        string tags = "";
        Data d = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet ds = new DataSet();
        TopLiteral.Text = "<div style=\" width: 300px;\">";
        switch (tag_type)
        {
            case tagType.AD:
                ds = d.GetData("SELECT DISTINCT C.ID, ACM.ID AS AID, C.Name AS CategoryName, ACM.tagSize FROM Ad_Category_Mapping ACM, AdCategories C WHERE ACM.CategoryID=C.ID AND ACM.AdID=" + theID +" ORDER BY ACM.ID");
                d.TAG_TYPE = Data.tagType.AD;
                break;
            case tagType.EVENT:
                ds = d.GetData("SELECT DISTINCT C.ID, ECM.ID AS EID, C.Name AS CategoryName, ECM.tagSize FROM Event_Category_Mapping ECM, EventCategories C WHERE ECM.CategoryID=C.ID AND ECM.EventID=" + theID + " ORDER BY ECM.ID ");
                d.TAG_TYPE = Data.tagType.EVENT;
                break;
            case tagType.VENUE:
                ds = d.GetData("SELECT DISTINCT C.ID, C.Name AS CategoryName, VC.tagSize FROM Venue_Category VC, VenueCategories C WHERE VC.CATEGORY_ID=C.ID AND VC.VENUE_ID=" + theID);
                d.TAG_TYPE = Data.tagType.VENUE;
                break;
            case tagType.GROUP:
                ds = d.GetData("SELECT DISTINCT C.ID, C.GroupName AS CategoryName, '22' AS tagSize FROM Group_Category VC, GroupCategories C WHERE VC.CATEGORY_ID=C.ID AND VC.Group_ID=" + theID);
                d.TAG_TYPE = Data.tagType.GROUP;
                HeaderPanel.Visible = false;
                TopLiteral.Text = "<div style=\" width: 400px;\">";
                break;
            case tagType.GROUP_EVENT:
                ds = d.GetData("SELECT DISTINCT C.ID, C.GroupName AS CategoryName, '22' AS tagSize " +
                    "FROM GroupEvent_Category VC, GroupCategories C WHERE VC.CATEGORY_ID=C.ID AND " +
                "VC.GroupEvent_ID=" + theID);
                d.TAG_TYPE = Data.tagType.GROUP_EVENT;
                HeaderPanel.Visible = false;
                TopLiteral.Text = "<div style=\" width: 390px;\">";
                break;
            default: break;
        }

        if (ds.Tables.Count > 0)
            if (ds.Tables[0].Rows.Count > 0)
            {
                TagsLiteral.Text = d.getTags(ds, true, false);
            }
            else
                HeaderPanel.Visible = false;
        else
            HeaderPanel.Visible = false;
    }
}
