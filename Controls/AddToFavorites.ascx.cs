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

public partial class AddToFavorites : System.Web.UI.UserControl
{
    SqlConnection conn;

    public string TEXT
    {
        get { return text; }
        set { text = value; }
    }
    public int VENUE_ID
    {
        get { return VenueID; }
        set
        {
            VenueID = value;
        }
    }
    private string text;
    private int VenueID;
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Session["Type"] = "Venue";
        Session["VenueID"] = VenueID;
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        if (IsPostBack)
        {
            MessageRadWindowManager.VisibleOnPageLoad = false;
        }

        if (Session["User"] != null)
        {

            DataSet ds = dat.GetData("SELECT * FROM UserVenues WHERE UserID=" + 
                Session["User"].ToString() + " AND VenueID=" + VenueID.ToString());

            if (ds.Tables.Count > 0)
                if (ds.Tables[0].Rows.Count > 0)
                {
                    AddToLabel.Text = "Your Favorite";
                    AddToLabel.Visible = true;
                }
                else
                {
                    AddToLink.Text = "Add to favorites";
                    AddToLink.Visible = true;
                }
            else
            {
                AddToLink.Text = "Add to favorites";
                AddToLink.Visible = true;
            }
        }
    }

    protected void AddToLink_Click(object sender, int id)
    {
        AddToLink.Text = "Hello";
    }

    protected void ShowMessage(object sender, EventArgs e)
    {
        Session["Type"] = "Venue";
        //MessageLiteral.Text = "<script type=\"text/javascript\">alert('" + message + "');</script>";

        MessageRadWindow.NavigateUrl = "MessageAlert.aspx?T=Venue&ID="+Session["VenueID"].ToString();
        MessageRadWindow.Visible = true;

        MessageRadWindowManager.VisibleOnPageLoad = true;
    }

}
