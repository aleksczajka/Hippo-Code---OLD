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
        if (!IsPostBack)
        {
            HttpCookie cookie = Request.Cookies["BrowserDate"];
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            try
            {

                //Put the ads into session
                //if (!IsPostBack)
                //{
                //    if (Session["User"] != null)
                //    {
                //        if (Session["AD1Count"] == null || Session["AD1DV"] == null)
                //        {
                //            dat.GetAdSet(1, false);
                //            dat.GetAdSet(1, true);
                //        }
                //        else
                //        {
                //            if (Application[Session["User"].ToString() + "_Normal"] != null)
                //            {
                //                if (Application[Session["User"].ToString() + "_Normal"].ToString() == "set")
                //                {
                //                    dat.GetAdSet(1, false);
                //                    dat.GetAdSet(1, true);
                //                    Application[Session["User"].ToString() + "_Normal"] = null;
                //                }
                //            }
                //        }
                //    }
                //    else
                //    {
                //        if (Session["AnonymousUser"] == null)
                //            dat.GetAdSet(1, false);

                //        if (Session["BigAnonymousUser"] == null)
                //            dat.GetAdSet(1, true);

                //    }
                //}


                if (Session["User"] != null)
                {
                    LoginPanel.Visible = true;
                    LoginButtonPanel.Visible = false;
                    CalendarPanel.Visible = true;
                    //int numOfEventsMax = int.Parse(System.Configuration.ConfigurationManager.AppSettings["NumberOfEventsTilPrize"].ToString());
                    //DataSet ds =  dat.GetData("SELECT NumPrizes FROM Users_Prizes WHERE UserID="+Session["User"].ToString());
                    //int numPrizes = 0;

                    //if (ds.Tables.Count > 0)
                    //    if (ds.Tables[0].Rows.Count > 0)
                    //        numPrizes = int.Parse(ds.Tables[0].Rows[0]["NumPrizes"].ToString());

                    //int numOfEvents = int.Parse(dat.GetData("SELECT COUNT(UserName) AS COUNT1 FROM Events WHERE UserName='"+Session["UserName"].ToString()+"'").Tables[0].Rows[0]["COUNT1"].ToString());

                    //int numLeft = 0;
                    //int temp = numOfEvents;
                    //if (numOfEvents > (numPrizes+1) * numOfEventsMax)
                    //    numLeft = numOfEventsMax - numOfEvents;
                    //else
                    //{
                    //    while (temp > numOfEventsMax * (numPrizes+1))
                    //    {
                    //        temp = -numOfEventsMax;
                    //    }

                    //    numLeft = numOfEventsMax - temp;
                    //}

                    //Session["UserName"] = dat.GetData("SELECT UserName FROM USERS WHERE User_ID="+Session["User"].ToString()).Tables[0].Rows[0]["UserName"].ToString();
                    //PostsNumberLabel.Text = numLeft.ToString();
                }
                else
                {
                    LoginPanel.Visible = false;
                    LoginButtonPanel.Visible = true;
                    CalendarPanel.Visible = false;
                    //Label label = new Label();
                    //label.Text = "Win stuff for posting events. Go to: ";
                    //NumberPanel.Controls.Clear();
                    //NumberPanel.Controls.Add(label);
                }

            }
            catch (Exception ex)
            {
            }
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (Request.Url.AbsolutePath == "/EnterEvent.aspx" || Request.Url.AbsolutePath == "/BlogEvent.aspx")
            Button3.CssClass = "NavBarImageAddEventSelected";
        else if (Request.Url.AbsolutePath == "/EnterVenue.aspx" || Request.Url.AbsolutePath == "/EnterVenueIntro.aspx")
            Button2.CssClass = "NavBarImageAddVenueSelected";
        else if (Request.Url.AbsolutePath == "/PostAnAd.aspx")
            Button1.CssClass = "NavBarImageAddAdSelected";
        else if (Request.Url.AbsolutePath == "/EnterGroup.aspx")
            Button4.CssClass = "NavBarImageAddGroupSelected";
    }

    protected void GoTo(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        string arg = ((Button)sender).CommandArgument.ToString();
        Data d = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        switch (arg)
        {

            case "H":
                Response.Redirect("~/Home.aspx");
                break;
            case "E":
                Response.Redirect("~/EventSearch.aspx");
                break;
            case "B":
                Response.Redirect("EnterEvent.aspx");
                break;
            case "C":
                Response.Redirect("UserCalendar.aspx");
                break;
            case "V":
                Response.Redirect("VenueSearch.aspx");
                break;
            case "A":
                Response.Redirect("AdSearch.aspx");
                break;
            case "G":
                SearchTextBox.Text = d.stripHTML(SearchTextBox.Text);
                Session["EventSearchString"] = "SELECT DISTINCT E.ID AS EID, * FROM Events E, Venues V, Event_Occurance EO WHERE E.ID=EO.EventID AND E.Venue=V.ID AND E.Header LIKE '%" + SearchTextBox.Text + "%'";
                Response.Redirect("EventSearch.aspx");
                break;
            case "M":
                Response.Redirect("User.aspx?ID="+Session["User"].ToString());
                break;
            case "P":
                Response.Redirect("PrizesToWin.aspx");
                break;
            case "L":
                Response.Redirect("UserLogin.aspx");
                break;
            case "R":
                Response.Redirect("Register.aspx");
                break;
            case "O":
                //Session.Abandon();
                d.Execute("DELETE FROM Events_Seen_By_User WHERE userID=" + Session["User"].ToString());
                FormsAuthentication.SignOut();
                Response.Redirect("Home.aspx");
                break;
            case "Ab":
                Response.Redirect("About.aspx");
                break;
            case "Fb":
                Response.Redirect("Feedback.aspx");
                break;
            case "Vo":
                Response.Redirect("Vote.aspx");
                break;
            case "AddAd":
                
                Response.Redirect("PostAnAd.aspx");
                break;
            case "AddVenue":
                if(Session["User"] == null)
                Response.Redirect("EnterVenueIntro.aspx");
                else
                Response.Redirect("EnterVenue.aspx");
                break;
            case "AddEvent":
                if(Session["User"] == null)
                Response.Redirect("EnterEvent.aspx");
                else
                Response.Redirect("BlogEvent.aspx");
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
                Response.Redirect("~/About.aspx");
                break;
            case "A2":
                Response.Redirect("~/Feedback.aspx");
                break;
            case "A3":
                Response.Redirect("~/Vote.aspx");
                break;
            case "G":
                SearchTextBox.Text = d.stripHTML(SearchTextBox.Text);
                if (SearchTextBox.Text != "")
                {
                    char[] delim = { ' ' };
                    string[] tokens;

                    tokens = SearchTextBox.Text.Split(delim);
                    string temp = "";
                    for (int i = 0; i < tokens.Length; i++)
                    {
                        temp += " E.Header LIKE @search" + i.ToString();

                        if (i + 1 != tokens.Length)
                            temp += " AND ";
                    }
                    string searchStr = "SELECT DISTINCT E.ID AS EID, V.ID AS VID, * FROM Events E, Venues V, Event_Occurance EO WHERE E.ID=EO.EventID AND E.Venue=V.ID AND "+temp;
                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    SqlCommand cmd = new SqlCommand();

                    //Check if signed in, get address
                    string country = "";
                    string state = "";
                    string stateParam = "";
                    string city = "";
                    string cityParam = "";

                    DataSet dsAddress = new DataSet();
                    if (Session["User"] != null)
                    {
                        dsAddress = d.GetData("SELECT * FROM UserPreferences WHERE UserID=" + 
                            Session["User"].ToString());

                        //Get search on address
                        if (dsAddress.Tables.Count > 0)
                            if (dsAddress.Tables[0].Rows.Count > 0)
                            {
                                country = " AND E.Country=" + dsAddress.Tables[0].Rows[0]["CatCountry"].ToString();

                                if (dsAddress.Tables[0].Rows[0]["CatState"].ToString() != "")
                                {
                                    state = " AND E.State=@state ";
                                    stateParam = dsAddress.Tables[0].Rows[0]["CatState"].ToString();

                                    if (dsAddress.Tables[0].Rows[0]["CatCity"].ToString() != "")
                                    {
                                        city = " AND E.City=@city ";
                                        cityParam = dsAddress.Tables[0].Rows[0]["CatCity"].ToString();
                                    }
                                }
                            }
                    }
                    else
                    {
                        //Check if IP in User table, get address
                        DataSet dsCheck = d.GetData("SELECT * FROM Users WHERE IPs LIKE '%" + d.GetIP() + "%'");

                        bool getSearchIP = false;
                        if (dsCheck.Tables.Count > 0)
                        {
                            if (dsCheck.Tables[0].Rows.Count > 0)
                            {
                                dsAddress = d.GetData("SELECT * FROM UserPreferences WHERE UserID=" +
                            dsCheck.Tables[0].Rows[0]["User_ID"].ToString());

                                //Get search on address
                                if (dsAddress.Tables.Count > 0)
                                    if (dsAddress.Tables[0].Rows.Count > 0)
                                    {
                                        country = " AND E.Country=" + dsAddress.Tables[0].Rows[0]["CatCountry"].ToString();

                                        if (dsAddress.Tables[0].Rows[0]["CatState"].ToString() != "")
                                        {
                                            state = " AND E.State=@state ";
                                            stateParam = dsAddress.Tables[0].Rows[0]["CatState"].ToString();

                                            if (dsAddress.Tables[0].Rows[0]["CatCity"].ToString() != "")
                                            {
                                                city = " AND E.City=@city ";
                                                cityParam = dsAddress.Tables[0].Rows[0]["CatCity"].ToString();
                                            }
                                        }
                                    }
                            }
                            else
                            {
                                getSearchIP = true;
                            }
                        }
                        else
                            getSearchIP = true;
                        

                        if (getSearchIP)
                        {
                            //Get search on address
                            dsAddress = d.GetData("SELECT * FROM SearchIPs WHERE IP='" + d.GetIP()+"'");
                            if (dsAddress.Tables.Count > 0)
                                if (dsAddress.Tables[0].Rows.Count > 0)
                                {
                                    country = " AND E.Country=" + dsAddress.Tables[0].Rows[0]["Country"].ToString();

                                    if (dsAddress.Tables[0].Rows[0]["State"].ToString() != "")
                                    {
                                        state = " AND E.State=@state ";
                                        stateParam = dsAddress.Tables[0].Rows[0]["State"].ToString();

                                        if (dsAddress.Tables[0].Rows[0]["City"].ToString() != "")
                                        {
                                            city = " AND E.City=@city ";
                                            cityParam = dsAddress.Tables[0].Rows[0]["City"].ToString();
                                        }
                                    }
                                }
                        }
                    }

                    if (country != "")
                    {
                        searchStr += country;

                        if (state != "")
                        {
                            searchStr += state;
                            cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = stateParam;

                            if (city != "")
                            {
                                searchStr += city;
                                cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = city;
                            }
                        }

                    }



                    Session["searchstring"] = searchStr;

                    cmd.Connection = conn;
                    cmd.CommandText = searchStr;
                    for (int i = 0; i < tokens.Length; i++)
                    {
                        cmd.Parameters.Add("@search" + i.ToString(), SqlDbType.NVarChar).Value = "%" + tokens[i] + "%";
                    }
                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    conn.Close();


                    Session["EventSearchDS"] = ds;
                }
                Response.Redirect("EventSearch.aspx");
                break;
            case "M":
                Response.Redirect("User.aspx?ID=" + Session["User"].ToString());
                break;
            case "P":
                Response.Redirect("PrizesToWin.aspx");
                break;
            case "L":
                Response.Redirect("UserLogin.aspx");
                break;
            case "R":
                Response.Redirect("Register.aspx");
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
                FormsAuthentication.SignOut();


                if (save1 != "")
                    Session["AnonymousUser"] = save1;

                if (save2 != "")
                    Session["BigAnonymousUser"] = save2;

                Response.Redirect("Home.aspx");
                break;
            default: break;
        }
    }

    
}
