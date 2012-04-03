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

public partial class Header : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

        LoginButton.SERVER_CLICK += LogIn;

        if (Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower() == "event.aspx" ||
            Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower() == "eventsearch.aspx")
        {
            EventPanel.Visible = true;
            VenuePanel.Visible = false;
            AdPanel.Visible = false;
            TripPanel.Visible = false;
        }
        else if (Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower() == "venue.aspx" ||
            Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower() == "venuesearch.aspx" ||
            Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower() == "venuecalendar.aspx")
        {
            VenuePanel.Visible = true;
            EventPanel.Visible = false;
            AdPanel.Visible = false;
            TripPanel.Visible = false;
        }
        else if (Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower() == "ad.aspx" ||
  Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower() == "adsearch.aspx")
        {
            AdPanel.Visible = true;
            VenuePanel.Visible = false;
            EventPanel.Visible = false;
            TripPanel.Visible = false;
        }
        else if (Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower() == "trip.aspx" ||
  Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower() == "tripsearch.aspx")
        {
            AdPanel.Visible = false;
            VenuePanel.Visible = false;
            EventPanel.Visible = false;
            TripPanel.Visible = true;
        }
        else if (Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower() == "home.aspx" ||
     Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower() == "user.aspx" ||
        Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower() == "searchesandpages.aspx" ||
         Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower() == "usercalendar.aspx")
        {
            AboutPanel.Visible = true;
        }
//        else if (Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower() == "contactus.aspx" ||
//          Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower() == "feedback.aspx" ||
//          Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower() == "userlogin.aspx" ||
//         Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower() == "termsandconditions.aspx" ||
//         Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower() == "about.aspx" ||
//    Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower() == "privacypolicy.aspx" ||
//    Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower() == "prohibitedads.aspx" ||
//    Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower() == "sitemap.aspx" ||
// Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower() == "entertripintro.aspx" ||
// Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower() == "enterevent.aspx" ||
// Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower() == "add.aspx" ||
//  Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower() == "feature.aspx" ||
// Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower() == "completeregistration.aspx" ||
// Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower() == "entervenueintro.aspx" ||
// Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower() == "hippopointstou.aspx" ||
// Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower() == "rateexperience.aspx" ||
//Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower() == "register.aspx" ||
//Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower() == "venuecalendar.aspx")
//        {
//            AdPanel.Visible = false;
//            VenuePanel.Visible = false;
//            EventPanel.Visible = false;
//            TripPanel.Visible = false;
//            AboutPanel.Visible = false;
//            //HippoPanel.Visible = true;
//        }

        if (!IsPostBack)
        {
            HttpCookie cookie = Request.Cookies["BrowserDate"];
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            DateTime isn = DateTime.Now;

            DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn);

            Data dat = new Data(isn);
            try
            {
                if (Session["User"] != null)
                {
                    LoginPanel.Visible = true;
                    LoginButtonPanel.Visible = false;
                }
                else
                {
                    LoginPanel.Visible = false;
                    LoginButtonPanel.Visible = true;
                }

            }
            catch (Exception ex)
            {
            }
        }
    }

    protected void GoTo(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        string arg = ((LinkButton)sender).CommandArgument.ToString();
        Data d = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        switch (arg)
        {

            case "H":
                Response.Redirect("~/home");
                break;
            case "E":
                Response.Redirect("~/event-search");
                break;
            case "B":
                Response.Redirect("enter-event");
                break;
            case "C":
                Response.Redirect("my-calendar");
                break;
            case "V":
                Response.Redirect("venue-search");
                break;
            case "A":
                Response.Redirect("ad-search");
                break;
            case "M":
                Response.Redirect("my-account?ID=" + Session["User"].ToString());
                break;
            case "P":
                Response.Redirect("PrizesToWin.aspx");
                break;
            case "L":
                Response.Redirect("login");
                break;
            case "R":
                Response.Redirect("register");
                break;
            case "O":
                Session.Remove("User");
                Session.Remove("UserName");
                Session.Abandon();
                Response.Redirect("home");
                break;
            case "Ab":
                Response.Redirect("about");
                break;
            case "Fb":
                Response.Redirect("feedback");
                break;
            case "Vo":
                Response.Redirect("Vote.aspx");
                break;
            case "AddAd":

                Response.Redirect("post-bulletin");
                break;
            case "AddVenue":
                if(Session["User"] == null)
                    Response.Redirect("Enter-Venue-Intro");
                else
                    Response.Redirect("enter-locale");
                break;
            case "AddEvent":
                if(Session["User"] == null)
                    Response.Redirect("enter-event");
                else
                    Response.Redirect("blog-event");
                break;
            case "AddGroup":
                Response.Redirect("EnterGroup.aspx");
                break;
            default: break;
        }
    }

    protected void ImageGoTo(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        string arg = ((ImageButton)sender).CommandArgument.ToString();
        Data d = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        switch (arg)
        {
            case "A1":
                Response.Redirect("~/about");
                break;
            case "A2":
                Response.Redirect("~/feedback");
                break;
            case "A3":
                Response.Redirect("~/Vote.aspx");
                break;
            case "M":
                Response.Redirect("my-account?ID=" + Session["User"].ToString());
                break;
            case "P":
                Response.Redirect("PrizesToWin.aspx");
                break;
            case "L":
                Response.Redirect("login");
                break;
            case "R":
                Response.Redirect("Register");
                break;
            case "W":
                Response.Redirect("PostAndWin.aspx");
                break;
            case "O":
                d.Execute("DELETE FROM Events_Seen_By_User WHERE userID=" + Session["User"].ToString());
                string save1 = "";
                if (Session["AnonymousUser"] != null)
                    save1 = Session["AnonymousUser"].ToString();

                string save2 = "";
                if (Session["BigAnonymousUser"] != null)
                    save2 = Session["BigAnonymousUser"].ToString();


                Session.Remove("User");
                Session.Remove("UserName");


                if (save1 != "")
                    Session["AnonymousUser"] = save1;

                if (save2 != "")
                    Session["BigAnonymousUser"] = save2;

                Response.Redirect("home");
                break;
            default: break;
        }
    }

    protected void LogIn(object sender, System.EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        try
        {
            Encryption encrypt = new Encryption();

            DateTime isn = DateTime.Now;

            if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
                isn = DateTime.Now;
            DateTime isNow = isn;
            Data dat = new Data(isn); if (Page.IsValid)
            {
                if (DBConnection(LoginTextBox.Text.Trim(), encrypt.encrypt(PswTextBox.Text.Trim())))
                {
                    string groups = "";
                    SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
                    myConn.Open();
                    SqlCommand myCmd = new SqlCommand("SELECT U.Password, U.User_ID, UP.CatCountry, UP.CatState, UP.CatCity, U.UserName FROM Users U, UserPreferences UP WHERE U.User_ID=UP.UserID AND U.UserName=@UserName", myConn);
                    myCmd.CommandType = CommandType.Text;
                    myCmd.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = LoginTextBox.Text.Trim();
                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(myCmd);
                    da.Fill(ds);
                    myConn.Close();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dat.WhatHappensOnUserLogin(ds);

                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {

                        StatusLabel.Text = "Invalid Login, please try again!";
                    }
                }
                else
                {
                    StatusLabel.Text = "Invalid Login, please try again!";
                }
            }
        }
        catch (Exception ex)
        {
            StatusLabel.Text = ex.ToString();
        }

    }
    private bool DBConnection(string txtUser, string txtPass)
    {
        SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
        SqlCommand myCmd = new SqlCommand("SELECT Password, UserName, User_ID FROM Users WHERE UserName=@UserName", myConn);
        myCmd.CommandType = CommandType.Text;
        myCmd.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = txtUser;

        try
        {

            myConn.Open();
            SqlDataAdapter da = new SqlDataAdapter(myCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Password"].ToString() != txtPass)
                    {
                        StatusLabel.Text = "Invalid Login, please try again!";
                        return false;
                    }
                    else
                    {
                        myConn.Close();
                        Session["User"] = ds.Tables[0].Rows[0]["User_ID"].ToString();
                        Session["UserName"] = ds.Tables[0].Rows[0]["UserName"].ToString();
                        return true;
                    }
                }
                else
                {
                    StatusLabel.Text = "Invalid Login, please try again!";
                    return false;
                }
            }
            else
            {
                StatusLabel.Text = "Invalid Login, please try again!";
                return false;
            }



        }
        catch (Exception ex)
        {
            StatusLabel.Text = ex + "Error Connecting to the database";
            return false;
        }

    }
}
