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

public partial class Friend : Telerik.Web.UI.RadAjaxPage
{
    public int USER_ID;
    protected void Page_Load(object sender, EventArgs e)
    {    
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        Session["ViewedUser"] = Request.QueryString["ID"].ToString();
        Session["Subject"] = null;

        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        
        try
        {
            string friendID = Request.QueryString["ID"].ToString();
            string group = "";
           
            ScriptLiteral.Text = "<div style=\"display: none;\" id=\"idDiv\">" + friendID + "</div>";
            if (Session["User"] != null)
            {
                

                DataSet ds = dat.GetData("SELECT * FROM User_Friends UF, Users U WHERE UF.UserID="+Session["User"].ToString()+
                    " AND UF.FriendID="+friendID + " AND UF.UserID=U.User_ID");

                DataSet dsFriend = dat.GetData("SELECT * FROM Users U WHERE U.User_ID="+friendID);

                DataSet dsPrefs = dat.GetData("SELECT * FROM UserPreferences WHERE UserID="+friendID);

                bool isFriend = false;
                if(ds.Tables.Count > 0)
                    if(ds.Tables[0].Rows.Count > 0)
                        isFriend = true;

                if (isFriend)
                {
                    if (int.Parse(dsPrefs.Tables[0].Rows[0]["CalendarPrivacyMode"].ToString()) <= 2)
                    {
                        CalendarPanel.Visible = true;
                        CalendarLink.Text = " View " + dsFriend.Tables[0].Rows[0]["UserName"].ToString() + " Calendar";
                        CalendarLink.NavigateUrl = "UserCalendar.aspx?ID=" + friendID;
                        EventsPanel.Visible = true;
                    }
                    else
                    {
                        CalendarPanel.Visible = false;
                        EventsPanel.Visible = false;
                        PrivacyLabel.Text = "The rest of this user's profile is private.";
                    }
                }
                else
                {
                    if (dsPrefs.Tables[0].Rows[0]["CalendarPrivacyMode"].ToString() == "1")
                    {
                        CalendarPanel.Visible = true;
                        CalendarLink.Text = " View User's Calendar";
                        CalendarLink.NavigateUrl = "UserCalendar.aspx?ID=" + friendID;
                        EventsPanel.Visible = true;
                    }
                    else
                    {
                        if (dsPrefs.Tables[0].Rows[0]["CalendarPrivacyMode"].ToString() == "2")
                            PrivacyLabel.Text = "The rest of this user's profile is only viewable to friends.";
                        else
                            PrivacyLabel.Text = "The rest of this user's profile is private.";
                        CalendarPanel.Visible = false;
                        EventsPanel.Visible = false;
                    }

                    



                }
            }
            else
            {
                DataSet dsPrefs = dat.GetData("SELECT * FROM UserPreferences WHERE UserID=" + friendID);
                CalendarPanel.Visible = false;
                EventsPanel.Visible = false;

               
                    if (dsPrefs.Tables[0].Rows[0]["CalendarPrivacyMode"].ToString() == "1")
                    {
                        CalendarPanel.Visible = true;
                        CalendarLink.Text = " View User's Calendar";
                        CalendarLink.NavigateUrl = "UserCalendar.aspx?ID=" + friendID;
                        EventsPanel.Visible = true;
                    }
                    else
                    {
                        if (dsPrefs.Tables[0].Rows[0]["CalendarPrivacyMode"].ToString() == "2")
                            PrivacyLabel.Text = "The rest of this user's profile is only viewable to friends.";
                        else
                            PrivacyLabel.Text = "The rest of this user's profile is private.";

                        CalendarPanel.Visible = false;
                        EventsPanel.Visible = false;
                    }
                
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("Home.aspx");
        }

        LoadInfo();
        
        
    }

    protected void OpenMessage(object sender, EventArgs e)
    {
        Session["Type"] = "Message";
        //MessageLiteral.Text = "<script type=\"text/javascript\">alert('" + message + "');</script>";
        string id = "";
        string a = "";
        Session["Subject"] = "Just a message";
        id = Request.QueryString["ID"].ToString();
           


        MessageRadWindow.NavigateUrl = "MessageAlert.aspx?T=Message&ID=" + id;
        MessageRadWindow.Visible = true;

        MessageRadWindowManager.VisibleOnPageLoad = true;
    }

    protected void Page_Init(object sender, EventArgs e)
    {
    }

    protected void LoadInfo()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        if (Request.QueryString["ID"] == null)
            Response.Redirect("~/Home.aspx");
        USER_ID = int.Parse(Request.QueryString["ID"].ToString());
        Session["Friend"] = USER_ID;
        Data d = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet dsUser = d.GetData("SELECT * FROM Users WHERE User_ID=" + USER_ID);
        DataSet dsComments = d.GetData("SELECT * FROM User_Comments CU, Users U WHERE CU.CommenterID=U.User_ID AND CU.UserID=" + USER_ID.ToString());
        UserNameLabel.Text = dsUser.Tables[0].Rows[0]["UserName"].ToString();

        if (dsUser.Tables[0].Rows[0]["ProfilePicture"].ToString() != null)
            if (System.IO.File.Exists(Server.MapPath(".") + "/UserFiles/" + dsUser.Tables[0].Rows[0]["UserName"].ToString() +
                "/Profile/" + dsUser.Tables[0].Rows[0]["ProfilePicture"].ToString()))
            {
                System.Drawing.Image theimg = System.Drawing.Image.FromFile(Server.MapPath(".") + "/UserFiles/" + dsUser.Tables[0].Rows[0]["UserName"].ToString() +
                "/Profile/" + dsUser.Tables[0].Rows[0]["ProfilePicture"].ToString());

                double width = double.Parse(theimg.Width.ToString());
                double height = double.Parse(theimg.Height.ToString());

                if (width > height)
                {
                    if (width <= 150)
                    {

                    }
                    else
                    {
                        double dividor = double.Parse("150.00") / double.Parse(width.ToString());
                        width = double.Parse("150.00");
                        height = height * dividor;
                    }
                }
                else
                {
                    if (width == height)
                    {
                        width = double.Parse("150.00");
                        height = double.Parse("150.00");
                    }
                    else
                    {
                        double dividor = double.Parse("150.00") / double.Parse(height.ToString());
                        height = double.Parse("150.00");
                        width = width * dividor;
                    }
                }

                FriendImage.Width = int.Parse((Math.Round(decimal.Parse(width.ToString()))).ToString());
                FriendImage.Height = int.Parse((Math.Round(decimal.Parse(height.ToString()))).ToString());
                FriendImage.ImageUrl = "UserFiles/" + dsUser.Tables[0].Rows[0]["UserName"].ToString() +
                    "/Profile/" + dsUser.Tables[0].Rows[0]["ProfilePicture"].ToString();

            }
            else
                FriendImage.ImageUrl = "NewImages/NoAvatar150.jpg";
        else
            FriendImage.ImageUrl = "NewImages/NoAvatar150.jpg";

        EventsTitle.Text = dsUser.Tables[0].Rows[0]["UserName"].ToString() + "'s Events";

        //if (dsUser.Tables.Count > 0)
        //    if (dsUser.Tables[0].Rows.Count > 0)
        //    {
        //        if (dsUser.Tables[0].Rows[0]["Email"] != null)
        //            if (dsUser.Tables[0].Rows[0]["Email"].ToString() != "")
        //                EmailTextBox.Text = dsUser.Tables[0].Rows[0]["Email"].ToString();
        //    }

        if (Session["User"] != null)
        {
            DataSet dsFriend = d.GetData("SELECT * FROM User_Friends WHERE UserID=" + Session["User"].ToString() + " AND FriendID=" + USER_ID);
            AddAsFriendPanel.Visible = true;
            if (dsFriend.Tables.Count > 0)
                if (dsFriend.Tables[0].Rows.Count > 0)
                    AddAsFriendPanel.Visible = false;

            if (Session["User"].ToString() == USER_ID.ToString())
                AddAsFriendPanel.Visible = false;

            if (AddAsFriendPanel.Visible)
            {
                //make sure user doen't click 'add as friend' twice:
                DataSet dsSent = dat.GetData("SELECT * FROM UserMessages WHERE To_UserID=" +
                        Request.QueryString["ID"].ToString() + " AND From_UserID=" +
                        Session["User"].ToString() + " AND Mode=2");
                bool isSent = false;

                if (dsSent.Tables.Count > 0)
                    if (dsSent.Tables[0].Rows.Count > 0)
                        isSent = true;

                if (isSent)
                {
                    AddAsFriendPanel.Visible = false;
                    AddedFriendLabel.Text = "Your friend request has been sent!";
                }
                else
                {
                    AddAsFriendPanel.Visible = true;
                }
            }
        }
        else
            AddAsFriendPanel.Visible = false;

        

        DataSet dsUserPrefs = d.GetData("SELECT * FROM UserPreferences WHERE UserID=" + USER_ID);

        if (dsUserPrefs.Tables.Count > 0)
            if (dsUserPrefs.Tables[0].Rows.Count > 0)
            {
                //AgeTextBox.Text = dsUserPrefs.Tables[0].Rows[0]["Age"].ToString();
                SexTextBox.Text = dsUserPrefs.Tables[0].Rows[0]["Sex"].ToString();

                LocationTextBox.Text = dsUserPrefs.Tables[0].Rows[0]["Location"].ToString();
                string eventsPosted = "0";
                DataSet dsEvents = d.GetData("SELECT COUNT(*) AS COUNTs FROM Events WHERE UserName='" + dsUser.Tables[0].Rows[0]["UserName"].ToString() + "'");
                if (dsEvents.Tables.Count > 0)
                    if (dsEvents.Tables[0].Rows.Count > 0)
                        eventsPosted = dsEvents.Tables[0].Rows[0]["COUNTs"].ToString();
                EventsLabel.Text = eventsPosted;
                dsEvents = d.GetData("SELECT COUNT(*) AS COUNTs FROM User_Calendar WHERE UserID=" + USER_ID);
                eventsPosted = "0";
                if (dsEvents.Tables.Count > 0)
                    if (dsEvents.Tables[0].Rows.Count > 0)
                        eventsPosted = dsEvents.Tables[0].Rows[0]["COUNTs"].ToString();

                AttendedLabel.Text = eventsPosted;
            }

        DataSet ds = d.GetData("SELECT E.ID AS EID, V.ID AS VID, EO.DateTimeStart, EO.ID AS ReoccurrID, 'False' AS isGroup, CONVERT(NVARCHAR, E.ID)+'E' AS HashID, * FROM Events E, Venues V, Event_Occurance EO, "
            + "User_Calendar UC WHERE UC.EventID=E.ID AND E.ID=EO.EventID AND E.Venue=V.ID AND UC.UserID=" + USER_ID + " ORDER BY EO.DateTimeStart ");

        EventsCtrl.EVENTS_DS = ds;
        EventsCtrl.IS_CONNECT_TO = true;
        EventsCtrl.DataBind2();

    }

    protected void AddAsFriend(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());

        conn.Open();
        SqlCommand cmd = new SqlCommand("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, To_UserID, Date, [Read], Mode)"
            + " VALUES(@content, @subject, @fromID, @toID, @date, 'False', 2)", conn);
        cmd.Parameters.Add("@content", SqlDbType.Text).Value = "Good Day from Hippo Happenings!, <br/><br/> We wanted to let you know that the user '" + Session["UserName"].ToString() + "' would like " +
            "to add you to their list of friends. To accept this request select the link below. <br/><br/> Have a Happening Day! <br/><br/> ";
        cmd.Parameters.Add("@subject", SqlDbType.NVarChar).Value = "You Have a Hippo Friend Request!";
        cmd.Parameters.Add("@toID", SqlDbType.Int).Value = int.Parse(Session["Friend"].ToString());
        cmd.Parameters.Add("@fromID", SqlDbType.Int).Value = int.Parse(Session["User"].ToString());
        cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
        cmd.ExecuteNonQuery();
        conn.Close();
        AddAsFriendPanel.Visible = false;
        AddedFriendLabel.Text = "Your friend request has been sent!";
    }

    //protected void ShowConnet(object sender, EventArgs e)
    //{
    //    Session["Type"] = "Connect";
    //    MessageRadWindow.NavigateUrl = "MessageAlert.aspx";
    //    MessageRadWindow.Visible = true;
    //    MessageRadWindowManager.VisibleOnPageLoad = true;
    //}
}
