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

public partial class AddToCalendar : System.Web.UI.UserControl
{
    SqlConnection conn;
    public string TEXT
    {
        get { return text; }
        set { text = value; }
    }
    public int EVENT_ID
    {
        get { return EventID; }
        set
        {
            EventID = value;
            
        }
    }
    private string text;
    private int EventID;
    protected void Page_Load(object sender, EventArgs e)
    {

        DataBind2();
    }

    public void DataBind2()
    {
        HttpCookie cookie = Context.Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        try
        {
            if (Session["User"] != null)
            {

                DataSet ds1 = dat.GetData("SELECT UserName FROM Users WHERE User_ID=" + Session["User"].ToString());
                DataSet ds = dat.GetData("SELECT EEL.ExcitmentLevel AS Level FROM User_Calendar UC, Event_ExcitmentLevel EEL WHERE UC.UserID="
            + Session["User"].ToString() + " AND UC.EventID = " + Session["EventID"].ToString() + " AND UC.ExcitmentID=EEL.ID ");

                if (ds.Tables.Count > 0)
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        AddToLabel.Text = "You're Going!";
                        AddToLabel.CssClass = "Green12LinkNF";
                        AddToLabel.Visible = true;
                        AddLiteral.Text = "";
                    }
                    else
                    {
                        AddLiteral.Text = "<a class=\"Green12LinkNFUD\" onclick=\"OpenRad_Add();\">Add to calendar</a>";
                    }
                else
                {
                    AddLiteral.Text = "<a class=\"Green12LinkNFUD\" onclick=\"OpenRad_Add();\">Add to calendar</a>";
                }
            }
            else
            {
                LoggedInPanel.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }

        
    }

    protected void ShowMessage(object sender, EventArgs e)
    {
        Session["Type"] = "Calendar";
        //MessageLiteral.Text = "<script type=\"text/javascript\">alert('" + message + "');</script>";
        string theID = Session["EventID"].ToString();
        MessageRadWindow.NavigateUrl = "Controls/AddEventAlert.aspx?ID=" + theID;
        MessageRadWindow.Visible = true;

        MessageRadWindowManager.VisibleOnPageLoad = true;
    }

}
