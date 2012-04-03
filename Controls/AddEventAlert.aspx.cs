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

public partial class AddEventAlert : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        YesButton.SERVER_CLICK += Server_AddTo;
        Button2.SERVER_CLICK += SetPrefs;
        Button1.SERVER_CLICK += UnSetPrefs;

        if (!IsPostBack)
        {
            try
            {
                if (Session["User"] != null)
                {
                    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

                    DataSet ds = dat.GetData("SELECT CommunicationPrefs FROM UserPreferences WHERE UserID=" + Session["User"].ToString());
                    if (ds.Tables[0].Rows[0]["CommunicationPrefs"].ToString() == "1")
                        Session["On"] = true;
                    else
                        Session["On"] = false;

                    string eventID = "";
                    if (Request.QueryString["ID"] != null)
                        eventID = Request.QueryString["ID"].ToString();
                    else
                        eventID = Session["EventID"].ToString();
                    TechLiteral.Text = "<div id=\"TechDiv\" style=\"display: none;\">" + eventID + "</div>";
                }
                else
                {
                    TechLiteral.Text = "<script type=\"text/javascript\">Search('../login');</script>";

                    Session["On"] = false;
                }
            }
            catch (Exception ex)
            {
                Session["On"] = false;
            }
        
      
              
                    //ConnectCheckBox.Checked = false;
                    //if (bool.Parse(Session["On"].ToString()))
                    //    ConnectCheckBox.Visible = true;
                    //else
                     //   ConnectCheckBox.Visible = false;
            
        }
    }

    //[Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
    //public int AddTo(string theID)
    //{
    //    string userID = "";
    //    string cookieName = FormsAuthentication.FormsCookieName;
    //    HttpCookie authCookie = Context.Request.Cookies[cookieName];
    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //    FormsAuthenticationTicket authTicket = null;
    //    try
    //    {
    //        authTicket = FormsAuthentication.Decrypt(authCookie.Value);
    //        string group = authTicket.UserData.ToString();

    //        if (group.Contains("User"))
    //        {
    //            userID = authTicket.Name;
    //            dat.Execute("INSERT INTO User_Calendar (UserID, EventID, ExcitmentID, isConnect) VALUES(" + userID + ", " +
    //                theID + ", 1, 'True')");

    //            //Get users to notify of friend's adding of event.
    //            dat.SendFriendAddNotification(userID, theID);
    //        }
    //        else
    //        {
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }

        

        

    //    //Label label = (Label)dat.FindControlRecursive(this.Parent, "AddToLabel");
    //    //LinkButton link = (LinkButton)dat.FindControlRecursive(this.Parent, "AddToLink");
    //    //label.Text = "stufrf";
    //    //label.Visible = true;
    //    //link.Visible = false;

    //    return 0;
    //}

    protected void Server_AddTo(object sender, EventArgs e)
    {
        string userID = "";
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));


        string eventID = "";
        if (Request.QueryString["ID"] != null)
            eventID = Request.QueryString["ID"].ToString();
        else
            eventID = Session["EventID"].ToString();

        try
        {
            if (Session["User"] != null)
            {
                userID = Session["User"].ToString();

                dat.Execute("INSERT INTO User_Calendar (UserID, EventID, ExcitmentID, isConnect, DateAdded) VALUES(" +
                    userID + ", " +
                    eventID + ", 1, 'True', '" + DateTime.Now.ToString() + "')");

                //Get users to notify of friend's adding of event.

                DataView dvU = dat.GetDataDV("SELECT * FROM UserPreferences WHERE UserID=" +
                    Session["User"].ToString() + " AND CommunicationPrefs='2'");

                ContentPanel.Visible = false;
                if (dvU.Count > 0)
                {
                    SetPreferences.Visible = true;
                }
                else
                {

                    ThankYouPanel.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptLiteral.Text = ex.ToString();
            ErrorLabel.Text = ex.ToString();
        }
    }

    protected void SetPrefs(object sender, EventArgs e)
    {

        string eventID = "";
        if (Request.QueryString["ID"] != null)
            eventID = Request.QueryString["ID"].ToString();
        else
            eventID = Session["EventID"].ToString();
                HttpCookie cookie = Request.Cookies["BrowserDate"];
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            dat.Execute("UPDATE User_Calendar SET isConnect=1 WHERE UserID=" + Session["User"].ToString() +
                " AND EventID=" + eventID);

            ContentPanel.Visible = false;
            SetPreferences.Visible = false;
            ThankYouPanel.Visible = false;
            ThankYouPanel2.Visible = true;
    }

    protected void UnSetPrefs(object sender, EventArgs e)
    {
        string eventID = "";
        if (Request.QueryString["ID"] != null)
            eventID = Request.QueryString["ID"].ToString();
        else
            eventID = Session["EventID"].ToString();
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        dat.Execute("UPDATE User_Calendar SET isConnect=0 WHERE UserID=" + Session["User"].ToString() +
            " AND EventID=" + eventID);

        ContentPanel.Visible = false;
        SetPreferences.Visible = false;
        ThankYouPanel.Visible = false;
        ThankYouPanel2.Visible = true;
    }
}
