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

public partial class Controls_PollAnswers : System.Web.UI.UserControl
{
    public int POLL_ID
    {
        get { return pollID; }
        set { pollID = value; }
    }
    protected int pollID = -1;
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        string cookieName = FormsAuthentication.FormsCookieName;
        HttpCookie authCookie = Context.Request.Cookies[cookieName];

        FormsAuthenticationTicket authTicket = null;
        try
        {
            string group = "";
            if (authCookie != null)
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                group = authTicket.UserData.ToString();
            }

            if (group.Contains("Admin"))
            {
                if (group.Contains("User"))
                {
                    Session["User"] = authTicket.Name;
                }
            }
            else
            {
                ImageButton calendarLink = (ImageButton)dat.FindControlRecursive(this, "CalendarLink");
                calendarLink.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void DataBind2()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (pollID != -1)
        {
            int pollCount = 0;
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            DataSet pollsDS;
            if (Session["User"] != null)
            {
                string commentPrefs = dat.GetData("SELECT * FROM UserPreferences WHERE UserID=" + Session["User"].ToString()).Tables[0].Rows[0]["PollPreferences"].ToString();
                if (commentPrefs == "1")
                {
                    pollsDS = dat.GetData("SELECT * FROM PollAnswers WHERE PollID=" + pollID.ToString() + " AND UserID <> " + Session["User"].ToString());
                }
                else
                {
                    pollsDS = dat.GetData("SELECT  * FROM PollAnswers PA, Users U, User_Friends UF WHERE UF.UserID=" + Session["User"].ToString() + " AND UF.FriendID=U.User_ID AND U.User_ID=PA.UserID AND PA.PollID=" + pollID.ToString());
                }
            }
            else
            {
                pollsDS = dat.GetData("SELECT * FROM PollAnswers WHERE PollID=" + pollID.ToString() + " AND UserID <> " + Session["User"].ToString());
            }
            if (pollsDS.Tables.Count > 0)
            {
                pollCount = pollsDS.Tables[0].Rows.Count;
                Label title = new Label();
                title.CssClass = "EventHeader";
                title.Text = "Answers:";
                PollElementsPanel.Controls.Add(title);
            }


            ArrayList a = new ArrayList(pollCount);


            for (int i = 0; i < pollCount; i++)
            {
                if (pollsDS.Tables[0].Rows[i]["Answer"].ToString() == "" && pollsDS.Tables[0].Rows[i]["MediaCategory"].ToString() == "0")
                {

                }
                else
                {
                    ASP.controls_pollanswer_ascx pollElement = new ASP.controls_pollanswer_ascx();
                    pollElement.ANSWER_ID = int.Parse(pollsDS.Tables[0].Rows[i]["AnswerID"].ToString());
                    pollElement.POLL_ID = pollID;
                    pollElement.USER_ID = int.Parse(pollsDS.Tables[0].Rows[i]["UserID"].ToString());

                    a.Add(pollElement);
                }
                //pagerPanel.Add(searchElement);

            }
            ASP.controls_pager_ascx pagerPanel = new ASP.controls_pager_ascx();
            pagerPanel.NUMBER_OF_ITEMS_PER_PAGE = 10;
            pagerPanel.DATA = a;
            pagerPanel.DataBind2();
            PollElementsPanel.Controls.Add(pagerPanel);
        }
    }
    public void Clear()
    {
        PollElementsPanel.Controls.Clear();
    }
}
