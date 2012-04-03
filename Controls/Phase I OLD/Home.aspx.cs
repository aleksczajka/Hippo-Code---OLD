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
using System.Diagnostics;

public partial class Home : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Page.Trace.IsEnabled = true;
        //Page.Trace.TraceMode = TraceMode.SortByTime;
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.Date.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        bool fillUserData = false;
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":").Replace("%28", "(").Replace("%29", ")")));


        if (!IsPostBack)
        {
            try
            {
                HtmlMeta hm = new HtmlMeta();
                HtmlMeta kw = new HtmlMeta();

                HtmlHead head = (HtmlHead)Page.Header;
               
                hm.Name = "Description";
                hm.Content = "Find your local events, venues and classifieds all while " +
                         "ads from your peers, neighborhood, and community are displayed "+
                         "to you purely based on your interests." +
                         "Large corporations are not welcome!";
                head.Controls.AddAt(0, hm);


                HtmlMeta nMT = new HtmlMeta();
                nMT.Name = "google-site-verification";
                nMT.Content = "tw8rmOWW-DlZa-H4DZdGr201J5kC7NVLXUmk5oN8vFM";

                head.Controls.Add(nMT);

                HtmlLink lkl = new HtmlLink();
                lkl.Href = "ror.xml";
                lkl.Attributes.Add("rel", "alternate");
                lkl.Attributes.Add("type", "application/xml");
                lkl.Attributes.Add("title", "ROR");

                head.Controls.Add(lkl);


                kw.Name = "keywords";
                kw.Content = "events, ads, venues, post, search, find, local events, concerts, festivals, world, theatre, technology, " +
                    "family, peers, neighborhood, classifieds";

                //DataView dvCats = dat.GetDataDV("SELECT * FROM AdCategories");

                //for (int i = 0; i < dvCats.Count; i++)
                //{
                //    kw.Content += ", " + dvCats[i]["Name"].ToString();
                //}

                //dvCats = dat.GetDataDV("SELECT * FROM EventCategories");

                //for (int i = 0; i < dvCats.Count; i++)
                //{
                //    kw.Content += ", " + dvCats[i]["Name"].ToString();
                //}


                //dvCats = dat.GetDataDV("SELECT * FROM VenueCategories");

                //for (int i = 0; i < dvCats.Count; i++)
                //{
                //    kw.Content += ", " + dvCats[i]["Name"].ToString();
                //}

                head.Controls.AddAt(0, kw);



                //logInPanel.Visible = Session["User"] == null;
                //loggedInPanel.Visible = Session["User"] != null;

                //Fill the home mission
                //if (!IsPostBack)
                //{
                //    ChangeMission(MissionTimer, new EventArgs());
                //}

                string country = "";
                string state = "";
                string city = "";
                string countryID = "";
                string stateID = "";
                string cityID = "";
                try
                {
                    if (Session["User"] != null)
                    {
                        DataSet ds1 = dat.GetData("SELECT * FROM Users U, UserPreferences UP WHERE U.User_ID=" +
                            Session["User"].ToString() + " AND U.User_ID=UP.UserID ");

                        country = ds1.Tables[0].Rows[0]["CatCountry"].ToString();
                        countryID = country;

                        state = ds1.Tables[0].Rows[0]["CatState"].ToString();
                        city = ds1.Tables[0].Rows[0]["CatCity"].ToString();
                        stateID = state;
                        cityID = city;

                        //DataSet ds2 = dat.RetrieveAds(Session["User"].ToString(), false);
                        //DataSet dsMain = dat.RetrieveMainAds(Session["User"].ToString());   
                        //Ads1.DATA_SET = ds2;
                        //Ads1.MAIN_AD_DATA_SET = dsMain;

                        fillUserData = true;
                    }
                    else
                    {
                        DataSet ds1 = dat.GetData("SELECT * FROM Users U, UserPreferences UP WHERE " +
                                " U.User_ID=UP.UserID AND U.IPs LIKE '%" + dat.GetIP() + "%'");

                        bool getAnotherDs1 = false;
                        if (ds1.Tables.Count > 0)
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                country = ds1.Tables[0].Rows[0]["CatCountry"].ToString();
                                countryID = country;
                                state = ds1.Tables[0].Rows[0]["CatState"].ToString();
                                city = ds1.Tables[0].Rows[0]["CatCity"].ToString();
                                stateID = state;
                                cityID = city;
                            }
                            else
                            {
                                getAnotherDs1 = true;
                            }
                        else
                        {
                            getAnotherDs1 = true;
                        }

                        if (getAnotherDs1)
                        {
                            ds1 = dat.GetData("SELECT * FROM SearchIPs WHERE IP='" + dat.GetIP() + "'");

                            if (ds1.Tables.Count > 0)
                            {
                                if (ds1.Tables[0].Rows.Count > 0)
                                {
                                    country = ds1.Tables[0].Rows[0]["Country"].ToString();
                                    countryID = country;
                                    state = ds1.Tables[0].Rows[0]["State"].ToString();
                                    city = ds1.Tables[0].Rows[0]["City"].ToString();
                                    stateID = state;
                                    cityID = city;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }

                #region Take care of group activity

                if (country != "" && state != "")
                {
                    //Get Latest Group activity from Public groups
                    //GroupEvents
                    //    -posted on
                    //    -last edited

                    string thePlace = " AND G.Country=" + country + " AND G.State='" + state + "' ";
                    if (city != "")
                        thePlace += " AND G.City = '" + city.Replace("'", "''") + "'";

                    DataView dvTopGroupActivity = GetTheActivity(thePlace);

                    //Find the top 5 most recent
                    if (dvTopGroupActivity.Count > 0)
                    {
                        dvTopGroupActivity.Sort = "TheDate DESC";
                    }
                    int countIndex = 5;
                    if (dvTopGroupActivity.Count < 5)
                        countIndex = dvTopGroupActivity.Count;

                    string theType = "";
                    Literal theLit = new Literal();
                    GroupsPanel.Controls.Clear();
                    string userID = "";
                    string content = "";
                    DataView dvUser = new DataView();
                    DataView dv = new DataView();
                    DataView dvGroup = new DataView();
                    DataView dvG = new DataView();
                    if (dvTopGroupActivity.Count > 0)
                    {
                        for (int i = 0; i < countIndex; i++)
                        {
                            theType = dvTopGroupActivity[i]["Type"].ToString();
                            //<div style=\"width: 388px;background-repeat: repeat-y; background-image: url(images/ActCommentMiddle.png);\" class=\"topDiv\">
                            theLit.Text += "<div " +
                                "class=\"topDiv\" style=\"width: 378px;height: 143px;padding-left: 10px;padding-top: 10px; background-image: "+
                                "url(images/ActCommentTopPurple10.png); background-repeat: no-repeat; \"><div style=\"float: left;\"><img src=\"images/QuoteTop.png\" /></div><div style=\"float: left; width: 350px; padding-bottom: 5px;\">";

                            DateTime theDate = DateTime.Parse(dvTopGroupActivity[i]["TheDate"].ToString());
                            string postedText = "<div style=\"clear: both;\"><div style=\"float: left;\"><span " +
                                "style=\"font-size: 12px; font-style: italic; font-family: arial; color: #a5c13a;\">Posted " +
                                dat.GetMonth(theDate.Month.ToString()) + " " +
                                        theDate.Day.ToString() + ", " + theDate.Year.ToString() + "</span><br/>";
                            switch (theType)
                            {
                                case "GEP":
                                    dv = dat.GetDataDV("SELECT *, GEO.ID AS GEOID FROM GroupEvents GE, " +
                                        "GroupEvent_Occurance GEO WHERE GE.ID=GEO.GroupEventID AND GE.ID=" +
                                        dvTopGroupActivity[i]["TheID"].ToString());
                                    userID = dv[0]["UserID"].ToString();
                                    dvGroup = dat.GetDataDV("SELECT * FROM Groups WHERE ID=" + dv[0]["GroupID"].ToString());
                                    dvUser = dat.GetDataDV("SELECT * FROM UserPreferences UP, Users U " +
                                        "WHERE UP.UserID=U.User_ID AND UP.UserID=" + userID);

                                    theLit.Text += "<div style=\"float: left; padding-right: 10px; padding-left: " +
                                        "10px; padding-bottom: 5px; padding-top: 5px;\">" + getUserProfile(userID) + "</div>";

                                    content = dat.stripHTML(dv[0]["Content"].ToString());
                                    if (content.Length > 170)
                                        content = content.Substring(0, 170) + "...";

                                    theLit.Text += "<div style=\"padding-top: 5px;font-family: Arial; font-size: 12px; color: " +
                                        "#cccccc; font-style: italic;\">" + content + "&nbsp<a style=\"text-decoration: none;\" href=\"" +
                                        dat.MakeNiceName(dv[0]["Name"].ToString()) + "_" +
                                        dv[0]["GEOID"].ToString() + "_" + dvTopGroupActivity[i]["TheID"].ToString() +
                                        "_GroupEvent\"><span class=\"AddPurpleLink\">Read More</span></a></div>";
                                    theLit.Text += postedText;
                                    theLit.Text += "<span style=\"font-size: 12px; font-family: arial; color: #cccccc;\">posted by <a href=\"" + dvUser[0]["UserName"].ToString() +
                                        "_Friend\" class=\"AddLink\">" + dvUser[0]["UserName"].ToString() +
                                        "</a></span> <br/>";
                                    theLit.Text += "<a href=\"" + dat.MakeNiceName(dvGroup[0]["Header"].ToString()) +
                                        "_" + dvGroup[0]["ID"].ToString() + "_Group\" class=\"AddPurpleLink\">" +
                                        dvGroup[0]["Header"].ToString() + " Group</a></div><div style=\"float: right;\"><img src=\"images/QuoteBottom.png\" /></div></div>";
                                    break;
                                case "GEE":
                                    dv = dat.GetDataDV("SELECT *, GEO.ID AS GEOID FROM GroupEvents GE, " +
                                        "GroupEvent_Occurance GEO WHERE GE.ID=GEO.GroupEventID AND GE.ID=" +
                                        dvTopGroupActivity[i]["TheID"].ToString());
                                    userID = dv[0]["UserID"].ToString();
                                    dvGroup = dat.GetDataDV("SELECT * FROM Groups WHERE ID=" + dv[0]["GroupID"].ToString());
                                    dvUser = dat.GetDataDV("SELECT * FROM UserPreferences UP, Users U " +
                                        "WHERE UP.UserID=U.User_ID AND UP.UserID=" + userID);

                                    theLit.Text += "<div style=\"float: left; padding-right: 10px; padding-left: " +
                                        "10px; padding-bottom: 5px; padding-top: 5px;\">" + getUserProfile(userID) + "</div>";

                                    content = dat.stripHTML(dv[0]["Content"].ToString());
                                    if (content.Length > 170)
                                        content = content.Substring(0, 170) + "...";

                                    theLit.Text += "<div style=\"padding-top: 5px;font-family: Arial; font-size: 12px; color: " +
                                        "#cccccc; font-style: italic;\">" + content + "&nbsp<a style=\"text-decoration: none;\" href=\"" +
                                        dat.MakeNiceName(dv[0]["Name"].ToString()) + "_" +
                                        dv[0]["GEOID"].ToString() + "_" + dvTopGroupActivity[i]["TheID"].ToString() +
                                        "_GroupEvent\"><span class=\"AddPurpleLink\">Read More</span></a></div>";
                                    theLit.Text += postedText;
                                    theLit.Text += "<span style=\"font-size: 12px; font-family: arial; color: #cccccc;\">edited by <a href=\"" + dvUser[0]["UserName"].ToString() +
                                        "_Friend\" class=\"AddLink\">" + dvUser[0]["UserName"].ToString() +
                                        "</a></span> <br/>";
                                    theLit.Text += "<a href=\"" + dat.MakeNiceName(dvGroup[0]["Header"].ToString()) +
                                        "_" + dvGroup[0]["ID"].ToString() + "_Group\" class=\"AddPurpleLink\">" +
                                        dvGroup[0]["Header"].ToString() + " Group</a></div><div style=\"float: right;\"><img src=\"images/QuoteBottom.png\" /></div></div>";
                                    break;
                                case "GEM":
                                    dv = dat.GetDataDV("SELECT *, GEM.UserID AS TheUser, GEO.ID AS GEOID, GE.ID AS EID, GEM.Content AS TheContent FROM GroupEvents GE, " +
                                        "GroupEvent_Occurance GEO, GroupEventMessages GEM WHERE GEM.GroupEventID=GE.ID AND " +
                                        "GE.ID=GEO.GroupEventID AND GEM.ID=" + dvTopGroupActivity[i]["TheID"].ToString());
                                    userID = dv[0]["TheUser"].ToString();
                                    dvGroup = dat.GetDataDV("SELECT * FROM Groups WHERE ID=" + dv[0]["GroupID"].ToString());
                                    dvUser = dat.GetDataDV("SELECT * FROM UserPreferences UP, Users U " +
                                        "WHERE UP.UserID=U.User_ID AND UP.UserID=" + userID);

                                    theLit.Text += "<div style=\"float: left; padding-right: 10px; padding-left: " +
                                        "10px; padding-bottom: 5px; padding-top: 5px;\">" + getUserProfile(userID) + "</div>";

                                    content = dat.stripHTML(dv[0]["TheContent"].ToString());
                                    if (content.Length > 170)
                                        content = content.Substring(0, 170) + "...";

                                    theLit.Text += "<div style=\"padding-top: 5px;font-family: Arial; font-size: 12px; color: " +
                                        "#cccccc; font-style: italic;\">" + content + "&nbsp<a style=\"text-decoration: none;\" href=\"" +
                                        dat.MakeNiceName(dv[0]["Name"].ToString()) + "_" +
                                        dv[0]["GEOID"].ToString() + "_" + dv[0]["EID"].ToString() +
                                        "_GroupEvent\"><span class=\"AddPurpleLink\">Read More</span></a></div>";
                                    theLit.Text += postedText;
                                    theLit.Text += "<span style=\"font-size: 12px; font-family: arial; color: #cccccc;\">posted by <a href=\"" + dvUser[0]["UserName"].ToString() +
                                        "_Friend\" class=\"AddLink\">" + dvUser[0]["UserName"].ToString() +
                                        "</a></span> <br/>";
                                    theLit.Text += "<a href=\"" + dat.MakeNiceName(dvGroup[0]["Header"].ToString()) +
                                        "_" + dvGroup[0]["ID"].ToString() + "_Group\" class=\"AddPurpleLink\">" +
                                        dvGroup[0]["Header"].ToString() + " Group</a></div><div style=\"float: right;\"><img src=\"images/QuoteBottom.png\" /></div></div>";
                                    break;
                                case "GP":
                                    dvG = dat.GetDataDV("SELECT *  FROM Groups G WHERE ID=" +
                                        dvTopGroupActivity[i]["TheID"].ToString());
                                    userID = dvG[0]["Host"].ToString();
                                    dvUser = dat.GetDataDV("SELECT * FROM UserPreferences UP, Users U " +
                                        "WHERE UP.UserID=U.User_ID AND UP.UserID=" + userID);

                                    theLit.Text += "<div style=\"float: left; padding-right: 10px; padding-left: " +
                                        "10px; padding-bottom: 5px; padding-top: 5px;\">" + getUserProfile(userID) + "</div>";

                                    content = dat.stripHTML(dvG[0]["Content"].ToString());
                                    if (content.Length > 170)
                                        content = content.Substring(0, 170) + "...";

                                    theLit.Text += "<div style=\"padding-top: 5px;font-family: Arial; font-size: 12px; color: " +
                                        "#cccccc; font-style: italic;\">" + content + "&nbsp;<a style=\"text-decoration: none;\" href=\"" +
                                        dat.MakeNiceName(dvG[0]["Header"].ToString()) + "_" +
                                        dvTopGroupActivity[i]["TheID"].ToString() +
                                        "_Group\"><span class=\"AddPurpleLink\">Read More</span></a></div>";
                                    theLit.Text += postedText;
                                    theLit.Text += "<span style=\"font-size: 12px; font-family: arial; color: #cccccc;\">posted by <a href=\"" + dvUser[0]["UserName"].ToString() +
                                        "_Friend\" class=\"AddLink\">" + dvUser[0]["UserName"].ToString() +
                                        "</a></span> <br/>";
                                    theLit.Text += "<a href=\"" + dat.MakeNiceName(dvG[0]["Header"].ToString()) +
                                        "_" + dvG[0]["ID"].ToString() + "_Group\" class=\"AddPurpleLink\">" +
                                        dvG[0]["Header"].ToString() + " Group </a></div><div style=\"float: right;\"><img src=\"images/QuoteBottom.png\" /></div></div>";
                                    break;
                                case "GE":

                                    dvG = dat.GetDataDV("SELECT *  FROM Groups G WHERE ID=" +
                                        dvTopGroupActivity[i]["TheID"].ToString());
                                    userID = dvG[0]["Host"].ToString();
                                    dvUser = dat.GetDataDV("SELECT * FROM UserPreferences UP, Users U " +
                                        "WHERE UP.UserID=U.User_ID AND UP.UserID=" + userID);

                                    theLit.Text += "<div style=\"float: left; padding-right: 10px; padding-left: " +
                                        "10px; padding-bottom: 5px; padding-top: 5px;\">" + getUserProfile(userID) + "</div>";

                                    content = dat.stripHTML(dvG[0]["Content"].ToString());
                                    if (content.Length > 170)
                                        content = content.Substring(0, 170) + "...";

                                    theLit.Text += "<div style=\"padding-top: 5px;font-family: Arial; font-size: 12px; color: " +
                                        "#cccccc; font-style: italic;\">" + content + "&nbsp;<a style=\"text-decoration: none;\" href=\"" +
                                        dat.MakeNiceName(dvG[0]["Header"].ToString()) + "_" +
                                        dvTopGroupActivity[i]["TheID"].ToString() +
                                        "_Group\"><span class=\"AddPurpleLink\">Read More</span></a></div>";
                                    theLit.Text += postedText;
                                    theLit.Text += "<span style=\"font-size: 12px; font-family: arial; color: #cccccc;\">edited by <a href=\"" + dvUser[0]["UserName"].ToString() +
                                        "_Friend\" class=\"AddLink\">" + dvUser[0]["UserName"].ToString() +
                                        "</a></span> <br/>";
                                    theLit.Text += "<a href=\"" + dat.MakeNiceName(dvG[0]["Header"].ToString()) +
                                        "_" + dvG[0]["ID"].ToString() + "_Group\" class=\"AddPurpleLink\">" +
                                        dvG[0]["Header"].ToString() + " Group </a></div><div style=\"float: right;\"><img src=\"images/QuoteBottom.png\" /></div></div>";
                                    break;
                                case "GT":
                                    dvG = dat.GetDataDV("SELECT *, G.ID AS GID FROM Groups G, GroupThreads GT WHERE G.ID=GT.GroupID AND GT.ID=" +
                                        dvTopGroupActivity[i]["TheID"].ToString());
                                    userID = dvG[0]["StartedBy"].ToString();
                                    dvUser = dat.GetDataDV("SELECT * FROM UserPreferences UP, Users U " +
                                        "WHERE UP.UserID=U.User_ID AND UP.UserID=" + userID);

                                    theLit.Text += "<div style=\"float: left; padding-right: 10px; padding-left: " +
                                        "10px; padding-bottom: 5px; padding-top: 5px;\">" + getUserProfile(userID) + "</div>";

                                    content = dat.stripHTML("Thread added to the group: " + dvG[0]["ThreadName"].ToString());
                                    if (content.Length > 170)
                                        content = content.Substring(0, 170) + "...";

                                    theLit.Text += "<div style=\"padding-top: 5px;font-family: Arial; font-size: 12px; color: " +
                                        "#cccccc; font-style: italic;\">" + content + "&nbsp;<a style=\"text-decoration: none;\" href=\"" +
                                        dat.MakeNiceName(dvG[0]["Header"].ToString()) + "_" +
                                        dvG[0]["GID"].ToString() +
                                        "_Group\"><span class=\"AddPurpleLink\">Read More</span></a></div>";
                                    theLit.Text += postedText;
                                    theLit.Text += "<span style=\"font-size: 12px; font-family: arial; color: #cccccc;\">added by <a href=\"" + dvUser[0]["UserName"].ToString() +
                                        "_Friend\" class=\"AddLink\">" + dvUser[0]["UserName"].ToString() +
                                        "</a></span> <br/>";
                                    theLit.Text += "<a href=\"" + dat.MakeNiceName(dvG[0]["Header"].ToString()) +
                                        "_" + dvG[0]["GID"].ToString() + "_Group\" class=\"AddPurpleLink\">" +
                                        dvG[0]["Header"].ToString() + " Group </a></div><div style=\"float: right;\"><img src=\"images/QuoteBottom.png\" /></div></div>";
                                    break;
                                case "GTC":
                                    dvG = dat.GetDataDV("SELECT *, GTC.Content AS TheContent, G.ID AS GID FROM Groups G, " +
                                        "GroupThreads GT, GroupThreads_Comments GTC " +
                                        "WHERE GTC.ThreadID=GT.ID AND G.ID=GT.GroupID AND GTC.ID=" +
                                        dvTopGroupActivity[i]["TheID"].ToString());
                                    userID = dvG[0]["UserID"].ToString();
                                    dvUser = dat.GetDataDV("SELECT * FROM UserPreferences UP, Users U " +
                                        "WHERE UP.UserID=U.User_ID AND UP.UserID=" + userID);

                                    theLit.Text += "<div style=\"float: left; padding-right: 10px; padding-left: " +
                                        "10px; padding-bottom: 5px; padding-top: 5px;\">" + getUserProfile(userID) + "</div>";

                                    content = dat.stripHTML(dvG[0]["TheContent"].ToString());
                                    if (content.Length > 170)
                                        content = content.Substring(0, 170) + "...";

                                    theLit.Text += "<div style=\"padding-top: 5px;font-family: Arial; font-size: 12px; color: " +
                                        "#cccccc; font-style: italic;\">" + content + "&nbsp;<a style=\"text-decoration: none;\" href=\"" +
                                        dat.MakeNiceName(dvG[0]["Header"].ToString()) + "_" +
                                        dvG[0]["GID"].ToString() +
                                        "_Group\"><span class=\"AddPurpleLink\">Read More</span></a></div>";
                                    theLit.Text += postedText;
                                    theLit.Text += "<span style=\"font-size: 12px; font-family: arial; color: #cccccc;\">comment by <a href=\"" + dvUser[0]["UserName"].ToString() +
                                        "_Friend\" class=\"AddLink\">" + dvUser[0]["UserName"].ToString() +
                                        "</a></span> <br/>";
                                    theLit.Text += "<a href=\"" + dat.MakeNiceName(dvG[0]["Header"].ToString()) +
                                        "_" + dvG[0]["GID"].ToString() + "_Group\" class=\"AddPurpleLink\">" +
                                        dvG[0]["Header"].ToString() + " Group </a></div><div style=\"float: right;\"><img src=\"images/QuoteBottom.png\" /></div></div>";
                                    break;
                                case "GM":
                                    dvG = dat.GetDataDV("SELECT *, G.ID AS GID, GM.Content AS TheContent FROM GroupMessages GM, Groups G WHERE G.ID=GM.GroupID AND GM.ID=" +
                                        dvTopGroupActivity[i]["TheID"].ToString());
                                    userID = dvG[0]["UserID"].ToString();
                                    dvUser = dat.GetDataDV("SELECT * FROM UserPreferences UP, Users U " +
                                        "WHERE UP.UserID=U.User_ID AND UP.UserID=" + userID);

                                    theLit.Text += "<div style=\"float: left; padding-right: 10px; padding-left: " +
                                        "10px; padding-bottom: 5px; padding-top: 5px;\">" + getUserProfile(userID) + "</div>";

                                    content = dat.stripHTML(dvG[0]["TheContent"].ToString());
                                    if (content.Length > 170)
                                        content = content.Substring(0, 170) + "...";

                                    theLit.Text += "<div style=\"padding-top: 5px;font-family: Arial; font-size: 12px; color: " +
                                        "#cccccc; font-style: italic;\">" + content + "&nbsp;<a style=\"text-decoration: none;\" href=\"" +
                                        dat.MakeNiceName(dvG[0]["Header"].ToString()) + "_" +
                                        dvG[0]["GID"].ToString() +
                                        "_Group\"><span class=\"AddPurpleLink\">Read More</span></a></div>";
                                    theLit.Text += postedText;
                                    theLit.Text += "<span style=\"font-size: 12px; font-family: arial; color: #cccccc;\">message by <a href=\"" + dvUser[0]["UserName"].ToString() +
                                        "_Friend\" class=\"AddLink\">" + dvUser[0]["UserName"].ToString() +
                                        "</a></span> <br/>";
                                    theLit.Text += "<a href=\"" + dat.MakeNiceName(dvG[0]["Header"].ToString()) +
                                        "_" + dvG[0]["GID"].ToString() + "_Group\" class=\"AddPurpleLink\">" +
                                        dvG[0]["Header"].ToString() + " Group </a></div><div style=\"float: right;\"><img src=\"images/QuoteBottom.png\" /></div></div>";
                                    break;
                                default: break;
                            }

                            theLit.Text += "</div></div>";
                        }
                        GroupsPanel.Controls.Add(theLit);
                    }
                    else
                    {
                        Label lab = new Label();
                        lab.Text = "There are no groups in your area. To post some <a class=\"AddLink\" href=\"EnterGroup.aspx\">go here</a>";
                        GroupsLabel.Controls.Add(lab);
                    }
                }

                #endregion

                DataSet ds;

                if (!IsPostBack)
                {
                    LocationLabel.Text = "";
                    if (country != "")
                        country = " AND E.Country = " + country;

                    int c = 0;

                    if (state != "")
                    {

                        c++;
                    }

                    if (city != "")
                    {

                        c++;
                    }

                    SqlDbType[] types = new SqlDbType[c];
                    object[] data = new object[c];

                    if (state != "")
                    {
                        types[0] = SqlDbType.NVarChar;
                        data[0] = state;
                        state = " AND E.State=@p0 ";
                        if (city != "")
                        {
                            types[1] = SqlDbType.NVarChar;
                            data[1] = city;
                            city = " AND E.City=@p1 ";
                        }
                    }
                    else
                    {
                        if (city != "")
                        {
                            types[0] = SqlDbType.NVarChar;
                            data[0] = city;
                            city = " AND E.City=@p0 ";
                        }
                    }





                    ds = dat.GetDataWithParemeters("SELECT DISTINCT TOP 10 EO.DateTimeStart, E.Header, E.Content, EO.EventID FROM Events E, Event_Occurance EO WHERE E.LIVE='True' AND E.ID=EO.EventID " + country + state + city + " AND EO.DateTimeStart > '" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Date + "' ORDER BY EO.DateTimeStart", types, data);



                    Session["HomeEvents"] = ds;
                    LocationLabel.Text = "";
                    if (country == "" && state == "" && city == "")
                    {
                        LocationLabel.Text = " The World ";
                    }
                    else
                    {
                        if (city != "")
                        {
                            LocationLabel.Text += cityID;
                            if (state != "")
                                LocationLabel.Text += ", " + stateID;
                        }
                        else
                        {
                            if (state != "")
                                LocationLabel.Text += stateID;
                        }
                    }

                    if (LocationLabel.Text == "")
                    {
                        DataSet dsCountry = dat.GetData("SELECT * FROM Countries WHERE country_id=" + countryID);
                        LocationLabel.Text = dsCountry.Tables[0].Rows[0]["country_name"].ToString();
                    }
                }
                else
                {
                    EventPanel.Controls.Clear();
                    LocationLabel.Text = "";
                    ds = (DataSet)Session["HomeEvents"];

                    if (countryID == "" && stateID == "" && cityID == "")
                    {
                        LocationLabel.Text = " The World ";
                    }
                    else
                    {
                        if (cityID != "")
                        {
                            LocationLabel.Text += cityID;
                            if (state != "")
                                LocationLabel.Text += ", " + stateID;
                        }
                        else
                        {
                            if (state != "")
                                LocationLabel.Text += stateID;
                        }
                    }

                    //if (RadCalendar1.SelectedDate.ToShortDateString() != "1/1/0001")
                    //    LocationLabel.Text += " on " + RadCalendar1.SelectedDate.ToShortDateString();

                }

                ds = (DataSet)Session["HomeEvents"];

                if (ds != null)
                {

                    if (ds.Tables.Count > 0)
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                DateTime date = DateTime.Parse(ds.Tables[0].Rows[i]["DateTimeStart"].ToString());
                                ASP.controls_homeevent_ascx eventH = new ASP.controls_homeevent_ascx();
                                eventH.DAY = date.DayOfWeek.ToString().Substring(0, 3);
                                eventH.DAY_NUMBER = date.Day.ToString();
                                eventH.MONTH = dat.GetMonth(date.Month.ToString()).Substring(0, 3);
                                eventH.EVENT_NAME = ds.Tables[0].Rows[i]["Header"].ToString();
                                if (ds.Tables[0].Rows[i]["Content"].ToString().Length > 150)
                                    eventH.SUMMARY = dat.BreakUpString(ds.Tables[0].Rows[i]["Content"].ToString().Substring(0, 150).Replace("<br/>", " "), 67) + "...";
                                else
                                    eventH.SUMMARY = dat.BreakUpString(ds.Tables[0].Rows[i]["Content"].ToString().Replace("<br/>", " "), 67) + "...";
                                eventH.EVENT_ID = int.Parse(ds.Tables[0].Rows[i]["EventID"].ToString());
                                EventPanel.Controls.Add(eventH);
                            }

                        }
                        else
                        {

                            Label label = new Label();
                            label.CssClass = "EventBody";
                            label.Text = "No events are listed for this date. Sorry. But perhaps you could enter one... <a class=\"AddLink\" href=\"BlogEvent.aspx\">Enter Events</a>.";
                            EventPanel.Controls.Add(label);
                        }
                    else
                    {

                        Label label = new Label();
                        label.CssClass = "EventBody";
                        label.Text = "No events are listed for this date. Sorry. But perhaps you could enter one... <a class=\"AddLink\" href=\"BlogEvent.aspx\">Enter Events</a>.";
                        EventPanel.Controls.Add(label);
                    }
                }
                else
                {

                    Label label = new Label();
                    label.CssClass = "EventBody";
                    label.Text = "No events are listed for this date. Sorry. But perhaps you could enter one... <a class=\"AddLink\" href=\"BlogEvent.aspx\">Enter Events</a>.";
                    EventPanel.Controls.Add(label);
                }

                #region Original Home page code
                //if (ds.Tables.Count > 0)
                //{
                //    if (ds.Tables[0].Rows.Count > 0)
                //    {

                //        ASP.controls_footer_ascx thefooter = (ASP.controls_footer_ascx)dat.FindControlRecursive(this, "TheFooter");
                //        thefooter.EVENT_ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());

                //        string ID = ds.Tables[0].Rows[0]["ID"].ToString();
                //        DataSet dsDate = dat.GetData("SELECT * FROM Event_Occurance WHERE EventID=" + ID);
                //        DataSet dsVenue = dat.GetData("SELECT * FROM Venues WHERE ID=" + ds.Tables[0].Rows[0]["Venue"]);
                //        TagCloud.THE_ID = int.Parse(ID);


                //        DataSet dsComments = dat.GetData("SELECT C.BlogDate AS theDate, * FROM Comments C, Users U WHERE U.User_ID=C.UserID AND C.BlogID=" + ID +" ORDER BY C.BlogDate");
                //        TheComments.DATA_SET = dsComments;
                //        TheComments.DataBind2(true);

                //        if (bool.Parse(ds.Tables[0].Rows[0]["hasSongs"].ToString()))
                //        {
                //            DataSet dsSongs = dat.GetData("SELECT * FROM Event_Song_Mapping WHERE EventID=" + ID);
                //            ASP.controls_songplayer_ascx songs = new ASP.controls_songplayer_ascx();
                //            int songCount = dsSongs.Tables[0].Rows.Count;

                //            if (songCount > 2)
                //            {
                //                songs.SONG1 = dsSongs.Tables[0].Rows[0]["SongName"].ToString();
                //                songs.SONG2 = dsSongs.Tables[0].Rows[1]["SongName"].ToString();
                //                songs.SONG3 = dsSongs.Tables[0].Rows[2]["SongName"].ToString();
                //            }
                //            else if (songCount > 1)
                //            {
                //                songs.SONG1 = dsSongs.Tables[0].Rows[0]["SongName"].ToString();
                //                songs.SONG2 = dsSongs.Tables[0].Rows[1]["SongName"].ToString();
                //            }
                //            else
                //                songs.SONG1 = dsSongs.Tables[0].Rows[0]["SongName"].ToString();


                //            songs.USER_NAME = ds.Tables[0].Rows[0]["UserName"].ToString();

                //            SongPanel.Controls.Add(songs);
                //        }



                //        EventName.Text = ds.Tables[0].Rows[0]["Header"].ToString();
                //        Session["Subject"] = "Re: "+ds.Tables[0].Rows[0]["Header"].ToString();
                //        Session["CommentSubject"] = "Re: " + ds.Tables[0].Rows[0]["Header"].ToString();
                //        EventName.NavigateUrl = "~/Event.aspx?EventID=" + ID;
                //        Session["EventID"] = ID;
                //        VenueName.Text = dsVenue.Tables[0].Rows[0]["Name"].ToString();
                //        VenueName.NavigateUrl = "Venue.aspx?ID="+dsVenue.Tables[0].Rows[0]["ID"].ToString();
                //        DateTime date = (DateTime)dsDate.Tables[0].Rows[0]["DateTimeStart"];
                //        DateAndTimeLabel.Text = date.DayOfWeek.ToString() + ", " + GetMonth(date.Month.ToString()) + " " + date.Day + " " + date.Hour + ":" + date.Minute;
                //        string content = ds.Tables[0].Rows[0]["Content"].ToString();
                //        SendTxtID.MESSAGE = EventName.Text + " occurs at " + VenueName.Text + " on " + DateAndTimeLabel.Text;

                //        string href = Request.Url.AbsoluteUri;
                //        SendEmailID.MESSAGE = "EventName: <a class=\"AddLink\" href=\"" + href + 
                //            "\">" + EventName.Text + "</a> \n\r Venue: " + VenueName.Text + 
                //            " \n\r Date: " + DateAndTimeLabel.Text + " \n\r " + content;

                //        //if (fillUserData)
                //        //{
                //        //    DataSet ds2 = dat.GetData("SELECT EEL.ExcitmentLevel AS Level FROM User_Calendar UC, Event_ExcitmentLevel EEL WHERE UC.UserID="
                //        //        + Session["User"].ToString() + " AND UC.EventID = " + ID + " AND UC.ExcitmentID=EEL.ID ");

                //        //    bool addEvent = false;

                //        //    if (ds2.Tables.Count > 0)
                //        //        if (ds2.Tables[0].Rows.Count > 0)
                //        //        {
                //        //            Label label = new Label();
                //        //            label.CssClass = "AddLinkGoing";
                //        //            label.Text = "Guess What?!: you're going to this event and you are " + ds2.Tables[0].Rows[0]["Level"].ToString();
                //        //            CalendarPanel.Controls.Add(label);
                //        //        }
                //        //        else
                //        //            addEvent = true;
                //        //    else
                //        //        addEvent = true;

                //        //    if (addEvent)
                //        //    {
                //        //        ASP.controls_addtocalendar_ascx AddTo1 = new ASP.controls_addtocalendar_ascx();
                //        //        AddTo1.ID = "AddTo1";
                //        //        AddTo1.TEXT = "Add this event to calendar";
                //        //        AddTo1.EVENT_ID = int.Parse(ID);
                //        //        CalendarPanel.Controls.Add(AddTo1);
                //        //    }

                //        //}

                //        if (content.Length > 500)
                //        {
                //            ShowDescriptionBegining.Text = content.Substring(0, 500);
                //            int j = 500;
                //            if (content[500] != ' ')
                //            {

                //                while (content[j] != ' ')
                //                {
                //                    ShowDescriptionBegining.Text += content[j];
                //                    j++;
                //                }
                //            }
                //            ShowDescriptionBegining.Text = dat.BreakUpString(ShowDescriptionBegining.Text, 65);
                //            ShowRestOfDescription.Text = dat.BreakUpString(content.Substring(j), 65);
                //        }
                //        else
                //        {
                //            ShowDescriptionBegining.Text = dat.BreakUpString(content, 65);
                //            ShowRestOfDescription.Text = "";
                //        }


                //        //Media Categories: NONE: 0, Picture: 1, Video: 2, YouTubeVideo: 3, Slider: 4
                //        int mediaCategory = int.Parse(ds.Tables[0].Rows[0]["mediaCategory"].ToString());

                //        switch (mediaCategory)
                //        {
                //            case 0:
                //                break;
                //            case 1:
                //                ShowVideoPictureLiteral.Text = "<img style=\"float: left; padding-right: 10px; padding-top: 9px;\" height=\"250px\" width=\"440px\" src=\"UserFiles/" + ds.Tables[0].Rows[0]["Picture"].ToString() + "\" />";
                //                break;
                //            case 2:
                //                ShowVideoPictureLiteral.Text = "<div style=\"float:left; padding-top: 9px; padding-right: 10px;\"><embed  height=\"250px\" width=\"440px\" src=\"UserFiles/" + ds.Tables[0].Rows[0]["Video"].ToString() + "\" /></div>";
                //                break;
                //            case 3:
                //                ShowVideoPictureLiteral.Text = "<div style=\"float:left; padding-top: 9px; padding-right: 10px;\"><object width=\"440\" height=\"250\"><param name=\"movie\" value=\"http://www.youtube.com/v/" + ds.Tables[0].Rows[0]["YouTubeVideo"].ToString() + "\"></param><param name=\"allowFullScreen\" value=\"true\"></param><embed src=\"http://www.youtube.com/v/" + ds.Tables[0].Rows[0]["YouTubeVideo"].ToString() + "\" type=\"application/x-shockwave-flash\" allowfullscreen=\"true\" width=\"440\" height=\"250\"></embed></object></div>";
                //                break;
                //            case 4:
                //                ShowVideoPictureLiteral.Text = "";
                //                DataSet dsSlider = dat.GetData("SELECT * FROM Event_Slider_Mapping WHERE EventID=" + ID);
                //                if (dsSlider.Tables.Count > 0)
                //                    if (dsSlider.Tables[0].Rows.Count > 0)
                //                    {
                //                        char[] delim = { '\\' };
                //                        string[] fileArray = System.IO.Directory.GetFiles(MapPath(".") + "\\UserFiles\\" + Session["UserName"].ToString() + "\\Slider\\");

                //                        string[] finalFileArray = new string[fileArray.Length];

                //                        for (int i = 0; i < fileArray.Length; i++)
                //                        {
                //                            int length = fileArray[i].Split(delim).Length;
                //                            finalFileArray[i] = "http://" + Request.Url.Authority + "/HippoHappenings/UserFiles/" + Session["UserName"].ToString() + "/Slider/" + fileArray[i].Split(delim)[length - 1];
                //                        }
                //                        Rotator1.DataSource = finalFileArray;
                //                        Rotator1.DataBind();
                //                        RotatorPanel.Visible = true;
                //                    }
                //                break;
                //            default: break;
                //        }


                //        this.Title = EventName.Text;
                //    }
                //}
                #endregion
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.ToString();
            }
        }
    }

    protected DataView GetTheActivity(string thePlace)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.Date.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":").Replace("%28", "(").Replace("%29", ")")));


        DataView dvTopGroupActivity = new DataView();

        string notTheseGroups = "";
        string notTheseUsers = "";

        int count = 0;
        DataView dvGroupEventsPosts = new DataView();
        DataView dvGroupEventsEdits = new DataView();
        DataView dvGroupEventsMessages = new DataView();
        DataView dvGroupsPosts = new DataView();
        DataView dvGroupsEdits = new DataView();
        DataView dvGroupThreads = new DataView();
        DataView dvGroupThreadComments = new DataView();
        DataView dvGroupMessages = new DataView();

        int zeroCount = 0;
        DateTime dtm = new DateTime();

        Hashtable usedHash = new Hashtable();


        while (count < 5)
        {
            dvGroupEventsPosts = dat.GetDataDV("SELECT TOP 1 G.ID AS GID, GE.UserID, GE.ID AS TheID, GE.PostedOn AS TheDate, 'GEP' AS Type" +
                " FROM GroupEvents GE, Groups G " +
                "WHERE GE.PostedOn IS NOT NULL AND G.ID=GE.GroupID AND GE.EventType=1 AND G.isPrivate = 'False' " +
                thePlace + notTheseGroups + notTheseUsers + 
                " ORDER BY GE.PostedOn DESC");
            
            notTheseUsers = notTheseUsers.Replace("GE.UserID", "GE.LastEditBy");

            if (dvGroupEventsPosts.Count == 0)
            {
                zeroCount++;
            }
            else
            {
                if (!usedHash.Contains("EventPosts" + dvGroupEventsPosts[0]["TheID"].ToString()))
                {
                    dvTopGroupActivity = MergeDV(dvTopGroupActivity, dvGroupEventsPosts);

                    foreach (DataRowView row in dvGroupEventsPosts)
                    {
                        notTheseGroups += " AND G.ID <> " + row["GID"].ToString();
                        notTheseUsers += " AND GE.LastEditBy <> " + row["UserID"].ToString();
                    }
                    usedHash.Add("EventPosts" + dvGroupEventsPosts[0]["TheID"].ToString(), "");
                }
                else
                {
                    zeroCount++;
                }
            }

            dvGroupEventsEdits = dat.GetDataDV("SELECT TOP 1 G.ID AS GID, GE.LastEditBy, GE.ID AS TheID, GE.LastEdit AS TheDate, 'GEE' AS Type " +
                "FROM GroupEvents GE, Groups G " +
                "WHERE GE.LastEdit IS NOT NULL AND G.ID=GE.GroupID AND GE.EventType=1 AND G.isPrivate = 'False' " +
                thePlace + notTheseGroups + notTheseUsers + " ORDER BY GE.LastEdit DESC");

            notTheseUsers = notTheseUsers.Replace("GE.LastEditBy", "GEM.UserID");

            if (dvGroupEventsEdits.Count == 0)
            {
                zeroCount++;
            }
            else
            {
                if (!usedHash.Contains("EventEdits" + dvGroupEventsEdits[0]["TheID"].ToString()))
                {
                    dvTopGroupActivity = MergeDV(dvTopGroupActivity, dvGroupEventsEdits);

                    foreach (DataRowView row in dvGroupEventsEdits)
                    {
                        notTheseGroups += " AND G.ID <> " + row["GID"].ToString();
                        notTheseUsers += " AND GEM.UserID <> " + row["LastEditBy"].ToString();
                    }
                    usedHash.Add("EventEdits" + dvGroupEventsEdits[0]["TheID"].ToString(), "");
                }
                else
                {
                    zeroCount++;
                }
            }
            //Group Event messages
            //    -posted

            dvGroupEventsMessages = dat.GetDataDV("SELECT TOP 1 G.ID AS GID, GEM.UserID, GEM.ID AS TheID, GEM.DatePosted AS TheDate, 'GEM' " +
                "AS Type FROM GroupEventMessages " +
                "GEM, GroupEvents GE, Groups G WHERE G.ID=GE.GroupID AND GE.ID=GEM.GroupEventID AND " +
                "G.isPrivate='False' AND GE.EventType=1 " + thePlace + notTheseGroups +
                notTheseUsers + " ORDER BY GEM.DatePosted DESC");
            
            notTheseUsers = notTheseUsers.Replace("GEM.UserID", "G.Host");

            if (dvGroupEventsMessages.Count == 0)
            {
                zeroCount++;
            }
            else
            {
                if (!usedHash.Contains("EventMessages" + dvGroupEventsMessages[0]["TheID"].ToString()))
                {
                    dvTopGroupActivity = MergeDV(dvTopGroupActivity, dvGroupEventsMessages);

                    foreach (DataRowView row in dvGroupEventsMessages)
                    {
                        notTheseGroups += " AND G.ID <> " + row["GID"].ToString();
                        notTheseUsers += " AND G.Host <> " + row["UserID"].ToString();
                    }
                    usedHash.Add("EventMessages" + dvGroupEventsMessages[0]["TheID"].ToString(), "");
                }
                else
                {
                    zeroCount++;
                }
            }
            //Groups
            //    -posted on
            //    -last edited

            dvGroupsPosts = dat.GetDataDV("SELECT TOP 1 G.ID AS GID, G.Host, G.ID AS TheID, G.CreatedOn AS TheDate, 'GP' AS Type " +
                "FROM Groups G WHERE isPrivate = 'False' " +
                thePlace + notTheseGroups + notTheseUsers + " ORDER BY CreatedOn DESC");
            
            notTheseUsers = notTheseUsers.Replace("G.Host", "G.LastEditBy");

            if (dvGroupsPosts.Count == 0)
            {
                zeroCount++;
            }
            else
            {
                if (!usedHash.Contains("GroupPosts" + dvGroupsPosts[0]["TheID"].ToString()))
                {
                    dvTopGroupActivity = MergeDV(dvTopGroupActivity, dvGroupsPosts);

                    foreach (DataRowView row in dvGroupsPosts)
                    {
                        notTheseGroups += " AND G.ID <> " + row["GID"].ToString();
                        notTheseUsers += " AND G.LastEditBy <> " + row["Host"].ToString();
                    }
                    usedHash.Add("GroupPosts" + dvGroupsPosts[0]["TheID"].ToString(), "");
                }
                else
                {
                    zeroCount++;
                }
            }

            dvGroupsEdits = dat.GetDataDV("SELECT TOP 1 G.ID AS GID, G.LastEditBy, G.ID AS TheID, G.LastEditOn AS TheDate, 'GE' AS Type " +
                "FROM Groups G WHERE isPrivate = 'False'" +
                thePlace + notTheseGroups + notTheseUsers + " ORDER BY LastEditOn DESC");

            notTheseUsers = notTheseUsers.Replace("G.LastEditBy", "GT.StartedBy");

            if (dvGroupsEdits.Count == 0)
            {
                zeroCount++;
            }
            else
            {
                if (!usedHash.Contains("GroupEdits" + dvGroupsEdits[0]["TheID"].ToString()))
                {
                    dvTopGroupActivity = MergeDV(dvTopGroupActivity, dvGroupsEdits);

                    foreach (DataRowView row in dvGroupsEdits)
                    {
                        notTheseGroups += " AND G.ID <> " + row["GID"].ToString();
                        notTheseUsers += " AND GT.StartedBy <> " + row["LastEditBy"].ToString();
                    }
                    usedHash.Add("GroupEdits" + dvGroupsEdits[0]["TheID"].ToString(), "");
                }
                else
                {
                    zeroCount++;
                }
            }

            //Group Threads
            //    -posted

            dvGroupThreads = dat.GetDataDV("SELECT TOP 1 G.ID AS GID, GT.StartedBy, GT.ID AS TheID, GT.StartDate AS TheDate, 'GT' AS Type " +
                "FROM GroupThreads GT, Groups G WHERE " +
                "G.ID=GT.GroupID AND G.isPrivate = 'False' " + thePlace + notTheseGroups +
                notTheseUsers + " ORDER BY GT.StartDate DESC");
            
            notTheseUsers = notTheseUsers.Replace("GT.StartedBy", "GTC.UserID");

            if (dvGroupThreads.Count == 0)
            {
                zeroCount++;
            }
            else
            {
                if (!usedHash.Contains("Threads"+ dvGroupThreads[0]["TheID"].ToString()))
                {
                    dvTopGroupActivity = MergeDV(dvTopGroupActivity, dvGroupThreads);

                    foreach (DataRowView row in dvGroupThreads)
                    {
                        notTheseGroups += " AND G.ID <> " + row["GID"].ToString();
                        notTheseUsers += " AND GTC.UserID <> " + row["StartedBy"].ToString();
                    }
                    usedHash.Add("Threads" + dvGroupThreads[0]["TheID"].ToString(), "");
                }
                else
                {
                    zeroCount++;
                }
            }

            //Group Thread comments
            //    -posted

            dvGroupThreadComments = dat.GetDataDV("SELECT TOP 1 G.ID AS GID, GTC.UserID, GTC.ID AS TheID, GTC.PostedDate AS TheDate, " +
                "'GTC' AS Type FROM GroupThreads GT, GroupThreads_Comments GTC, Groups G " +
                " WHERE GT.GroupID=G.ID AND GT.ID=GTC.ThreadID AND G.isPrivate = 'False' " +
                thePlace + notTheseGroups + notTheseUsers + " ORDER BY GTC.PostedDate DESC");

            notTheseUsers = notTheseUsers.Replace("GTC.UserID", "GM.UserID");

            if (dvGroupThreadComments.Count == 0)
            {
                zeroCount++;
            }
            else
            {
                if (!usedHash.Contains("ThreadComments"+dvGroupThreadComments[0]["TheID"].ToString()))
                {
                    dvTopGroupActivity = MergeDV(dvTopGroupActivity, dvGroupThreadComments);

                    foreach (DataRowView row in dvGroupThreadComments)
                    {
                        notTheseGroups += " AND G.ID <> " + row["GID"].ToString();
                        notTheseUsers += " AND GM.UserID <> " + row["UserID"].ToString();
                    }

                    usedHash.Add("ThreadComments" + dvGroupThreadComments[0]["TheID"].ToString(), "");
                }
                else
                {
                    zeroCount++;
                }
            }

            //Group Messages
            //    -posted
            dvGroupMessages = dat.GetDataDV("SELECT TOP 1 GM.ID AS TheID, GM.UserID, GM.DatePosted AS TheDate, 'GM' AS Type " +
                "FROM GroupMessages GM, Groups G " +
                "WHERE G.ID=GM.GroupID AND G.isPrivate = 'False' " + thePlace + notTheseGroups +
                notTheseUsers + " ORDER BY GM.DatePosted DESC");

            notTheseUsers = notTheseUsers.Replace("GM.UserID", "GE.UserID");

            if (dvGroupMessages.Count == 0)
            {
                zeroCount++;
            }
            else
            {
                if (!usedHash.Contains("GroupMessages" + dvGroupMessages[0]["TheID"].ToString()))
                {
                    dvTopGroupActivity = MergeDV(dvTopGroupActivity, dvGroupMessages);

                    foreach (DataRowView row in dvGroupThreadComments)
                    {
                        notTheseGroups += " AND G.ID <> " + row["GID"].ToString();
                        notTheseUsers += " AND GE.UserID <> " + row["UserID"].ToString();
                    }
                    usedHash.Add("GroupMessages" + dvGroupMessages[0]["TheID"].ToString(), "");
                }
                else
                {
                    zeroCount++;
                }
            }

            if (zeroCount == 8)
                break;

            count += dvTopGroupActivity.Count;
            zeroCount = 0;
        }

        return dvTopGroupActivity;
    }

    protected string getUserProfile(string userID)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.Date.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", 
            " ").Replace("%3A", ":").Replace("%28", "(").Replace("%29", ")")));


        DataView dvUser = dat.GetDataDV("SELECT * FROM UserPreferences UP, Users U " +
            "WHERE UP.UserID=U.User_ID AND UP.UserID=" + userID);

        string profileThumb = dvUser[0]["ProfilePicture"].ToString();
        if (dvUser[0]["ProfilePicture"] == null)
            profileThumb = "image/noAvatar_50x50_small.png";
        if (dvUser[0]["ProfilePicture"].ToString().Trim() == "")
            profileThumb = "image/noAvatar_50x50_small.png";


        string hoverStr = "";

        double width = double.Parse("50.00");
        double height = double.Parse("50.00");

        if (profileThumb.ToString().Equals("image/noAvatar_50x50_small.png"))
        {
            hoverStr = " onmouseover = \"this.src='NewImages/noAvatar_50x50_smallhover.png'\" " +
                "onmouseout = \"this.src='image/noAvatar_50x50_small.png'\"";
        }
        else
        {
            if (System.IO.File.Exists(Server.MapPath("/UserFiles/" +
                dvUser[0]["UserName"].ToString() + "/Profile/" + profileThumb.ToString())))
            {
                System.Drawing.Image theimg = System.Drawing.Image.FromFile(Server.MapPath("/UserFiles/" +
                    dvUser[0]["UserName"].ToString() + "/Profile/" + profileThumb.ToString()));

                width = double.Parse(theimg.Width.ToString());
                height = double.Parse(theimg.Height.ToString());

                if (width > height)
                {
                    if (width <= 50)
                    {

                    }
                    else
                    {
                        double dividor = double.Parse("50.00") / double.Parse(width.ToString());
                        width = double.Parse("50.00");
                        height = height * dividor;
                    }
                }
                else
                {
                    if (width == height)
                    {
                        width = double.Parse("50.00");
                        height = double.Parse("50.00");
                    }
                    else
                    {
                        double dividor = double.Parse("50.00") / double.Parse(height.ToString());
                        height = double.Parse("50.00");
                        width = width * dividor;
                    }
                }

                profileThumb = "UserFiles/" + dvUser[0]["UserName"].ToString() + "/Profile/" + profileThumb;
            }
            else
            {
                profileThumb = "image/noAvatar_50x50_small.png";
                hoverStr = " onmouseover = \"this.src='NewImages/noAvatar_50x50_smallhover.png'\" " +
                "onmouseout = \"this.src='image/noAvatar_50x50_small.png'\"";
            }
        }

        string image = "<div width=\"52px\" align=\"center\" height=\"52px\" " +
            "bgcolor=\"#333333\" ><a  href=\"" + dvUser[0]["UserName"].ToString() + 
            "_Friend\"><img width=\"" +
            width.ToString() + "\" " + hoverStr + " height=\"" + height + "\" style=\"border: 0;\" src=\"" + 
            profileThumb.ToString() + "\" /></a></div>";
        return image;
    }

    protected DataView MergeDV(DataView dv1, DataView dv2)
    {
        DataTable dt = new DataTable();
        DataColumn dc = new DataColumn("TheDate");
        dt.Columns.Add(dc);
        dc = new DataColumn("Type");
        dt.Columns.Add(dc);
        dc = new DataColumn("TheID");
        dt.Columns.Add(dc);

        DataRow dtrow;

        foreach (DataRowView row in dv1)
        {
            dtrow = dt.NewRow();
            dtrow["TheDate"] = row["TheDate"];
            dtrow["Type"] = row["Type"];
            dtrow["TheID"] = row["TheID"];
            dt.Rows.Add(dtrow);
        }

        foreach (DataRowView row in dv2)
        {
            dtrow = dt.NewRow();
            dtrow["TheDate"] = row["TheDate"];
            dtrow["Type"] = row["Type"];
            dtrow["TheID"] = row["TheID"];
            dt.Rows.Add(dtrow);
        }

        return new DataView(dt, "", "", DataViewRowState.CurrentRows);
    }

    //protected void Search(object sender, EventArgs e)
    //{
    //    if (SearchTextBox.THE_TEXT != "")
    //    {
    //        char[] delim = { ' ' };
    //        string[] tokens;

    //        tokens = SearchTextBox.THE_TEXT.Split(delim);
    //        string temp = "";
    //        for (int i = 0; i < tokens.Length; i++)
    //        {
    //            temp += " E.Header LIKE @search" + i.ToString();

    //            if (i + 1 != tokens.Length)
    //                temp += " AND ";
    //        }
    //        string searchStr = "SELECT DISTINCT E.ID AS EID, V.ID AS VID, * FROM Events E, Venues V, Event_Occurance EO WHERE E.ID=EO.EventID AND E.Venue=V.ID AND " + temp;
    //        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
    //        if (conn.State != ConnectionState.Open)
    //            conn.Open();
    //        SqlCommand cmd = new SqlCommand(searchStr, conn);
    //        for (int i = 0; i < tokens.Length; i++)
    //        {
    //            cmd.Parameters.Add("@search" + i.ToString(), SqlDbType.NVarChar).Value = "%" + tokens[i] + "%";
    //        }

    //        DataSet ds = new DataSet();
    //        SqlDataAdapter da = new SqlDataAdapter(cmd);
    //        da.Fill(ds);
    //        conn.Close();


    //        Session["EventSearchDS"] = ds;
    //    }
    //    Response.Redirect("EventSearch.aspx");
    //}

    protected void Page_Init(object sender, EventArgs e)
    {

    }

    //protected void GoToSearch(object sender, EventArgs e)
    //{
    //    HttpCookie cookie = Request.Cookies["BrowserDate"];
    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //    string cookieName = FormsAuthentication.FormsCookieName;
    //    HttpCookie authCookie = Context.Request.Cookies[cookieName];

    //    string country = "";
    //    string state = "";
    //    string city = "";

    //    FormsAuthenticationTicket authTicket = null;

    //    string group = "";
    //    if (authCookie != null)
    //    {
    //        authTicket = FormsAuthentication.Decrypt(authCookie.Value);
    //        group = authTicket.UserData.ToString();
    //    }

    //    if (group.Contains("User"))
    //    {
    //        DataSet ds1 = dat.GetData("SELECT * FROM Users U, UserPreferences UP WHERE U.User_ID=" +
    //            authTicket.Name + " AND U.User_ID=UP.UserID ");

    //        if(ds1.Tables.Count > 0)
    //            if (ds1.Tables[0].Rows.Count > 0)
    //            {
    //                country = ds1.Tables[0].Rows[0]["CatCountry"].ToString();
    //                state = ds1.Tables[0].Rows[0]["CatState"].ToString();
    //                city = ds1.Tables[0].Rows[0]["CatCity"].ToString();
    //            }
    //    }
    //    else
    //    {
    //        DataSet ds1 = dat.GetData("SELECT * FROM SearchIPs WHERE IP='" + dat.GetIP() + "'");

    //        if(ds1.Tables.Count > 0)
    //            if (ds1.Tables[0].Rows.Count > 0)
    //            {
    //                country = ds1.Tables[0].Rows[0]["Country"].ToString();
    //                state = ds1.Tables[0].Rows[0]["State"].ToString();
    //                city = ds1.Tables[0].Rows[0]["City"].ToString();
    //            }

    //    }

    //    if (country != "")
    //        country = " AND E.Country = " + country;

    //    int c = 0;

    //    if (state != "")
    //    {

    //        c++;
    //    }

    //    if (city != "")
    //    {

    //        c++;
    //    }

    //    SqlDbType[] types = new SqlDbType[c];
    //    object[] data = new object[c];

    //    if (state != "")
    //    {
    //        types[0] = SqlDbType.NVarChar;
    //        data[0] = state;
    //        state = " AND E.State=@p0 ";
    //        if (city != "")
    //        {
    //            types[1] = SqlDbType.NVarChar;
    //            data[1] = city;
    //            city = " AND E.City=@p1 ";
    //        }
    //    }
    //    else
    //    {
    //        if (city != "")
    //        {
    //            types[0] = SqlDbType.NVarChar;
    //            data[0] = city;
    //            city = " AND E.City=@p0 ";
    //        }
    //    }





    //    DataSet ds = dat.GetDataWithParemeters("SELECT DISTINCT TOP 10 EO.DateTimeStart, E.Header, E.Content, EO.EventID FROM Events E, Event_Occurance EO WHERE E.ID=EO.EventID " + country + state + city + " AND CONVERT(NVARCHAR, MONTH(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR, DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR, YEAR(EO.DateTimeStart)) = '" + RadCalendar1.SelectedDate.ToShortDateString() + "'", types, data);

    //    EventPanel.Controls.Clear();
    //    if (ds.Tables.Count > 0)
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {

    //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //            {

    //                DateTime date = DateTime.Parse(ds.Tables[0].Rows[i]["DateTimeStart"].ToString());
    //                ASP.controls_homeevent_ascx eventH = new ASP.controls_homeevent_ascx();
    //                eventH.DAY = date.DayOfWeek.ToString().Substring(0, 3);
    //                eventH.DAY_NUMBER = date.Day.ToString();
    //                eventH.MONTH = dat.GetMonth(date.Month.ToString()).Substring(0, 3);
    //                eventH.EVENT_NAME = ds.Tables[0].Rows[i]["Header"].ToString();
    //                if (ds.Tables[0].Rows[i]["Content"].ToString().Length > 150)
    //                    eventH.SUMMARY = dat.BreakUpString(ds.Tables[0].Rows[i]["Content"].ToString().Substring(0, 150), 67) + "...";
    //                else
    //                    eventH.SUMMARY = dat.BreakUpString(ds.Tables[0].Rows[i]["Content"].ToString(), 67) + "...";
    //                eventH.EVENT_ID = int.Parse(ds.Tables[0].Rows[i]["EventID"].ToString());
    //                EventPanel.Controls.Add(eventH);

                    
    //            }
    //        }
    //        else
    //        {
    //            Label label = new Label();
    //            label.CssClass = "EventBody";
    //            label.Text = "No events are listed for this date. Sorry. But perhaps you could enter one... <a class=\"AddLink\" href=\"EnterEvent.aspx\">Enter Events</a>.";
    //            EventPanel.Controls.Add(label);
    //        }
    //    else
    //    {
    //        Label label = new Label();
    //        label.CssClass = "EventBody";
    //        label.Text = "No events are listed for this date. Sorry. But perhaps you could enter one... <a class=\"AddLink\" href=\"EnterEvent.aspx\">Enter Events</a>.";
    //        EventPanel.Controls.Add(label);
    //    }

    //    Session["HomeEvents"] = ds;
      
    //}


}