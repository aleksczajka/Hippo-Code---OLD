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
        //Ajax.Utility.RegisterTypeForAjax(typeof(AddEventAlert));
        if (!IsPostBack)
        {
            //string type = Session["Type"].ToString();
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Context.Request.Cookies[cookieName];
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            FormsAuthenticationTicket authTicket = null;
            try
            {
                string group = "";
                if (authCookie != null)
                {
                    authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    group = authTicket.UserData.ToString();
                }

                if (group.Contains("User"))
                {
                    DataSet ds = dat.GetData("SELECT CommunicationPrefs FROM UserPreferences WHERE UserID=" + authTicket.Name);
                    if (ds.Tables[0].Rows[0]["CommunicationPrefs"].ToString() == "1")
                        Session["On"] = true;
                    else
                        Session["On"] = false;
                }
                else
                {
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
            string eventID = "";
            if (Request.QueryString["ID"] != null)
                eventID = Request.QueryString["ID"].ToString();
            else
                eventID = Session["EventID"].ToString();
                    TechLiteral.Text = "<div id=\"TechDiv\" style=\"display: none;\">"+eventID+"</div>";
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
        string cookieName = FormsAuthentication.FormsCookieName;
        HttpCookie authCookie = Context.Request.Cookies[cookieName];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        FormsAuthenticationTicket authTicket = null;

        bool isGroup = false;
        if(Request.QueryString["T"] != null)
            if (Request.QueryString["T"].ToString() == "G")
            {
                isGroup = true;
            }

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
                if (!isGroup)
                {
                    dat.Execute("INSERT INTO User_Calendar (UserID, EventID, ExcitmentID, isConnect, DateAdded) VALUES(" +
                        userID + ", " +
                        eventID + ", 1, 'True', '" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).ToString() + "')");

                    //Get users to notify of friend's adding of event.
                    dat.SendFriendAddNotification(userID, eventID);

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
                else
                {
                    DataView dvGroup = dat.GetDataDV("SELECT * FROM GroupEvents WHERE ID=" + 
                        Request.QueryString["ID"].ToString());

                    if (dvGroup[0]["EventType"].ToString() == "1")
                        dat.Execute("INSERT INTO User_GroupEvent_Calendar (GroupEventID, UserID, DateAdded, ReoccurrID) VALUES(" +
                        Request.QueryString["ID"].ToString() + ", " + Session["User"].ToString() + ", '" +
                        DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"))
                        + "', " +
                        Request.QueryString["O"].ToString() + ")");
                    else
                    {
                        DataView dvIsInvited = dat.GetDataDV("SELECT * FROM GroupEvent_Members WHERE GroupEventID=" +
                            Request.QueryString["ID"].ToString() + " AND UserID=" + Session["User"].ToString() +
                            " AND ReoccurrID=" + Request.QueryString["O"].ToString());
                        if (dvIsInvited.Count == 0)
                        {
                            dat.Execute("INSERT INTO GroupEvent_Members (GroupEventID, UserID, Accepted, ReoccurrID) VALUES(" +
                            Request.QueryString["ID"].ToString() + ", " + Session["User"].ToString() + ", 'True', " +
                            Request.QueryString["O"].ToString() + ")");
                        }
                        else
                        {
                            dat.Execute("UPDATE GroupEvent_Members SET Accepted ='True' WHERE ReoccurrID=" +
                                Request.QueryString["O"].ToString() + " AND GroupEventID=" +
                                Request.QueryString["ID"].ToString() + " AND UserID=" + Session["User"].ToString());
                        }
                    }

                    ContentPanel.Visible = false;
                    ThankYouPanel.Visible = true;
                }
            }
            else
            {
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
