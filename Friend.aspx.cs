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
           
            ScriptLiteral.Text = "<div class=\"Friend21\" id=\"idDiv\">" + friendID + "</div>";
            if (Session["User"] != null)
            {
                MessagePanel.Visible = true;
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
                        CalendarLink.NavigateUrl = "my-calendar?ID=" + friendID;
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
                        CalendarLink.NavigateUrl = "my-calendar?ID=" + friendID;
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

                #region SEO
                this.Title = "HippoHappenings Member " + dsFriend.Tables[0].Rows[0]["UserName"].ToString();

                HtmlMeta hm = new HtmlMeta();
                HtmlMeta kw = new HtmlMeta();
                HtmlMeta lg = new HtmlMeta();
                HtmlHead head = (HtmlHead)Page.Header;

                hm.Name = "Description";
                hm.Content = "Find out more about the HippoHappenings Member " + dsFriend.Tables[0].Rows[0]["UserName"].ToString();
                head.Controls.AddAt(0, hm);

                kw.Name = "keywords";
                kw.Content = "HippoHappenings Member " + dsFriend.Tables[0].Rows[0]["UserName"].ToString();
                head.Controls.AddAt(0, kw);

                lg.Name = "language";
                lg.Content = "English";
                head.Controls.AddAt(0, lg);
                #endregion
            }
            else
            {
                MessagePanel.Visible = false;   
                DataSet dsPrefs = dat.GetData("SELECT * FROM UserPreferences WHERE UserID=" + friendID);
                CalendarPanel.Visible = false;
                EventsPanel.Visible = false;

               
                    if (dsPrefs.Tables[0].Rows[0]["CalendarPrivacyMode"].ToString() == "1")
                    {
                        CalendarPanel.Visible = true;
                        CalendarLink.Text = " View User's Calendar";
                        CalendarLink.NavigateUrl = "my-calendar?ID=" + friendID;
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
            Response.Redirect("home");
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
            Response.Redirect("~/home");
        USER_ID = int.Parse(Request.QueryString["ID"].ToString());
        Session["Friend"] = USER_ID;
        Data d = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet dsUser = d.GetData("SELECT * FROM Users WHERE User_ID=" + USER_ID);
        DataSet dsComments = d.GetData("SELECT * FROM User_Comments CU, Users U WHERE CU.CommenterID=U.User_ID AND CU.UserID=" + USER_ID.ToString());

        if (Session["User"] != null)
        {
            if (USER_ID == int.Parse(Session["User"].ToString()))
            {
                UserNameLabel.Text = "Your Profile";
                UserNameLabel2.Text = "Your";
            }
            else
            {
                UserNameLabel.Text = dsUser.Tables[0].Rows[0]["UserName"].ToString() + "'s Profile";
                UserNameLabel2.Text = dsUser.Tables[0].Rows[0]["UserName"].ToString() + "'s";
            }
        }
        else
        {
            UserNameLabel.Text = dsUser.Tables[0].Rows[0]["UserName"].ToString() + "'s Profile";
            UserNameLabel2.Text = dsUser.Tables[0].Rows[0]["UserName"].ToString() + "'s";
        }

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

        FriendImage.AlternateText = "HippoHappenings Member " + dsUser.Tables[0].Rows[0]["UserName"].ToString();

        EventsTitle.Text = dsUser.Tables[0].Rows[0]["UserName"].ToString() + "'s Events";

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

                CheckMayors();

                GetBadges();
            }

        DataSet ds = d.GetData("SELECT E.ID AS EID, V.ID AS VID, EO.DateTimeStart, EO.ID AS ReoccurrID, * FROM Events E, Venues V, Event_Occurance EO, "
            + "User_Calendar UC WHERE UC.EventID=E.ID AND E.ID=EO.EventID AND E.Venue=V.ID AND UC.UserID=" + USER_ID + " ORDER BY EO.DateTimeStart ");
        EventsCtrl.EVENTS_DS = ds;
        EventsCtrl.IS_CONNECT_TO = true;
        EventsCtrl.DataBind2();

    }

    protected void AddAsFriend(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
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

        DataSet dsTo = dat.GetData("SELECT * FROM Users U, UserPreferences UP WHERE UP.UserID=U.User_ID AND U.User_ID=" + Session["Friend"].ToString());
        if (dsTo.Tables[0].Rows[0]["EmailPrefs"].ToString().Contains("5"))
        {
            dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                dsTo.Tables[0].Rows[0]["Email"].ToString(), Session["UserName"].ToString() +
                " would like to extend a Hippo Happ friend invitation to you. You can find out " +
                "about this user <a target=\"_blank\" class=\"AddLink\" href=\"http://hippohappenings.com/" + Session["UserName"].ToString() + "_Friend"
                 + "\">here</a>. To accept this Hippo user as a friend, please log onto <a href=\"http://hippohappenings.com/my-account\">Hippo Happenings</a>.", "Hippo Happs Friend Request!");
        }
    }

    protected void GetBadges()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        DateTime isNow = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
        Data dat = new Data(isNow);

        string userID = Request.QueryString["ID"].ToString();

        DataView dvUser = dat.GetDataDV("SELECT * FROM Users WHERE User_ID=" + userID);
        DataView dv = dat.GetDataDV("SELECT * FROM Mayors WHERE UserID=" + userID);

        Literal lit;
        int num = 0;
        int remainder = 0;
        string wonText = "You received this badge because you have";
        string notWon = "";
        if (Session["User"] != null)
        {
            if (Session["User"].ToString() == userID)
            {
                wonText = "You received this badge because you have";
                notWon = "<div>You do not have any badges to display. To find out how to get badges " +
                                "visit our <a class=\"NavyLink12UD\" href=\"hippo-points\">Hippo Points Page</a>.</div>";
            }
            else
            {
                wonText = dvUser[0]["UserName"].ToString() + " has received this badge because they have";
                notWon = "<div>" + dvUser[0]["UserName"].ToString() + " does not have any badges to display. To find out how to get badges " +
                    "visit our <a class=\"NavyLink12UD\" href=\"hippo-points\">Hippo Points Page</a>.</div>";
            }
        }
        else
        {
            wonText = dvUser[0]["UserName"].ToString() + " has received this badge because they have";
            notWon = "<div>" + dvUser[0]["UserName"].ToString() + " does not have any badges to display. To find out how to get badges " +
                "visit our <a class=\"NavyLink12UD\" href=\"hippo-points\">Hippo Points Page</a>.</div>";
        }

        if (dv.Count == 0)
        {
            lit = new Literal();
            lit.Text = notWon;

            BadgesPanel.Controls.Add(lit);
        }
        else
        {
            lit = new Literal();

            if (dv.Count != 0)
            {
                lit.Text += "<div class=\"Friend17\"><div id=\"div1\" class=\"Friend18\"><b>The Hippo:</b> " +wonText +
                    " won the Hippo Boss award at least once." +
                    "</div><div onmouseout=\"var theDiv = document.getElementById('div1');theDiv.style.display " +
                    "= 'none';\" onmouseover=\"var theDiv = document.getElementById('div1');theDiv.style.display " +
                    "= 'block';\" class=\"Friend19\">" + dv.Count.ToString() + "</div></div>";
            }

            if (dv.Count >= 5)
            {
                num = dv.Count / 5;
                remainder = dv.Count - num * 5;
                lit.Text += "<div class=\"Friend17\"><div id=\"div5\" class=\"Friend18\"><b>The Bronze Hippo:</b> " +
                    wonText+" won the Hippo Boss award 5 or more times." +
                    "</div><div onmouseout=\"var theDiv = document.getElementById('div5');theDiv.style.display " +
                    "= 'none';\" onmouseover=\"var theDiv = document.getElementById('div5');theDiv.style.display " +
                    "= 'block';\" class=\"Friend22\">" + num.ToString() + "</div></div>";
            }

            if (dv.Count >= 10)
            {
                num = dv.Count / 10;
                remainder = dv.Count - num * 10;
                lit.Text += "<div class=\"Friend17\"><div id=\"div10\" class=\"Friend18\"><b>The Silver Hippo:</b> " +
                    wonText+" won the Hippo Boss award 10 or more times." +
                    "</div><div onmouseout=\"var theDiv = document.getElementById('div10');theDiv.style.display " +
                    "= 'none';\" onmouseover=\"var theDiv = document.getElementById('div10');theDiv.style.display " +
                    "= 'block';\" class=\"Friend23\">" + num.ToString() + "</div></div>";
            }

            if (dv.Count >= 20)
            {
                num = dv.Count / 20;
                remainder = dv.Count - num * 20;
                lit.Text += "<div class=\"Friend17\"><div id=\"div20\" class=\"Friend18\"><b>The Golden Hippo:</b> " +
                    wonText+" won the Hippo Boss award 20 or more times." +
                    "</div><div onmouseout=\"var theDiv = document.getElementById('div20');theDiv.style.display " +
                    "= 'none';\" onmouseover=\"var theDiv = document.getElementById('div20');theDiv.style.display " +
                    "= 'block';\" class=\"Friend24\">" + num.ToString() + "</div></div>";
            }

            dv.RowFilter = "IsGlobal = 'True'";

            if (dv.Count != 0)
            {
                lit.Text += "<div class=\"Friend17\"><div id=\"divG\" class=\"Friend18\"><b>The Global Hippo:</b> " +
                    wonText+" won the Hippo Boss award globally, not just in your location." +
                    "</div><div onmouseout=\"var theDiv = document.getElementById('divG');theDiv.style.display " +
                    "= 'none';\" onmouseover=\"var theDiv = document.getElementById('divG');theDiv.style.display " +
                    "= 'block';\" class=\"Friend25\">" + dv.Count.ToString() + "</div></div>";
            }

            BadgesPanel.Controls.Add(lit);
        }
    }

    protected void CheckMayors()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        DateTime isNow = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
        Data dat = new Data(isNow);
        string thisMonth = isNow.Month.ToString() + "/1/" + isNow.Year.ToString();
        string userID = Request.QueryString["ID"].ToString();
        DataView dvUser = dat.GetDataDV("SELECT * FROM Users WHERE User_ID=" + userID);
        DataView dv = dat.GetDataDV("SELECT * FROM UserPreferences WHERE UserID=" + userID);

        //Happenings
        DataView dvEventCount = dat.GetDataDV("SELECT COUNT(*) AS count  FROM Events WHERE UserName='" + dvUser[0]["UserName"].ToString() + "'");

        DataView dvEventCountLast = dat.GetDataDV("SELECT COUNT(*) AS count FROM Events WHERE PostedOn >= CONVERT(DATETIME,'" +
            thisMonth + "') AND UserName='" + dvUser[0]["UserName"].ToString() + "'");
        NumEventsLabel.Text = "<span class=\"TextNormal Friend20\">" + dvEventCount[0]["count"].ToString() +
            "</span> Happenings. <span class=\"TextNormal Friend20\">" + dvEventCountLast[0]["count"].ToString() +
            "</span> This Month.";

        //Trips
        dvEventCount = dat.GetDataDV("SELECT COUNT(*) AS count  FROM Trips WHERE UserName='" + dvUser[0]["UserName"].ToString() + "'");

        dvEventCountLast = dat.GetDataDV("SELECT COUNT(*) AS count  FROM Trips WHERE PostedOn >= CONVERT(DATETIME,'" +
            thisMonth + "') AND UserName='" + dvUser[0]["UserName"].ToString() + "'");
        NumTripsLabel.Text = "<span class=\"TextNormal Friend20\">" + dvEventCount[0]["count"].ToString() +
            "</span> Trips. <span class=\"TextNormal Friend20\">" + dvEventCountLast[0]["count"].ToString() +
            "</span> This Month.";

        //Locales
        dvEventCount = dat.GetDataDV("SELECT COUNT(*) AS count  FROM Venues WHERE CreatedByUser=" + userID);

        dvEventCountLast = dat.GetDataDV("SELECT COUNT(*) AS count  FROM Venues WHERE PostedOn >= CONVERT(DATETIME,'" +
            thisMonth + "') AND CreatedByUser=" + userID);
        NumLocalesLabel.Text = "<span class=\"TextNormal Friend20\">" + dvEventCount[0]["count"].ToString() +
            "</span> Locales. <span class=\"TextNormal Friend20\">" + dvEventCountLast[0]["count"].ToString() +
            "</span> This Month.";

        //Ads
        dvEventCount = dat.GetDataDV("SELECT COUNT(*) AS count  FROM Ads WHERE User_ID=" + userID);

        dvEventCountLast = dat.GetDataDV("SELECT COUNT(*) AS count  FROM Ads WHERE DateAdded >= CONVERT(DATETIME,'" +
            thisMonth + "') AND User_ID=" + userID);
        NumAdsLabel.Text = "<span class=\"TextNormal Friend20\">" + dvEventCount[0]["count"].ToString() +
            "</span> Bulletins. <span class=\"TextNormal Friend20\">" + dvEventCountLast[0]["count"].ToString() +
            "</span> This Month.";
    }
}
