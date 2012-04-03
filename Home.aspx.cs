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
using System.IO;
using System.Xml;
using System.Net;
using Telerik.Web.UI;

public partial class Home : Telerik.Web.UI.RadAjaxPage
{
    protected enum TimeFrame {Beginning, Today, Tomorrow, ThisWeek, ThisWeekend, ThisMonth, NextDays, HighestPrice};

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

        DateTime isn = DateTime.Now;

        

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn);
        #region Take Care of Buttons
        SmallButton1.SERVER_CLICK += Suggest;
        TodayButton.SERVER_CLICK += SelectToday;
        TomorrowButton.SERVER_CLICK += SelectTomorrow;
        ThisWeekButton.SERVER_CLICK += SelectThisWeek;
        ThisWeekendButton.SERVER_CLICK += SelectThisWeekend;
        ThisMonthButton.SERVER_CLICK += SelectThisMonth;
        TodayButton.BUTTON_TEXT = "<img alt=\"Events today\" class=\"PostButton\" src=\"NewImages/TodayText.png\"/>";
        TomorrowButton.BUTTON_TEXT = "<img alt=\"Events tomorrow\" class=\"PostButton\" src=\"NewImages/TomorrowText.png\"/>";
        ThisWeekButton.BUTTON_TEXT = "<img alt=\"Events this week\" class=\"PostButton\" src=\"NewImages/ThisWeekText.png\"/>";
        ThisWeekendButton.BUTTON_TEXT = "<img alt=\"Events this weekend\" class=\"PostButton\" src=\"NewImages/ThisWeekendText.png\"/>";
        ThisMonthButton.BUTTON_TEXT = "<img alt=\"Events this month\" class=\"PostButton\" src=\"NewImages/ThisMonth.png\"/>";
        SearchButton.BUTTON_TEXT += "<img style=\"border: 0; padding-top: 3px;\" src=\"NewImages/search.png\"/>";
        #endregion

        

        if (!IsPostBack)
        {
            

            try
            {
                HtmlControl body = (HtmlControl)dat.FindControlRecursive(this.Page, "bodytag");

                body.Attributes["onload"] += "StartRotator();";

                //Literal lit = new Literal();
                //lit.Text = "<script type=\"text/javascript\">StopRotator();StartRotator();</script>";
                //HtmlGenericControl body = (HtmlGenericControl)dat.FindControlRecursive(this.Page, "bodytag");
                //body.Controls.Add(lit);

                #region SEO
                HtmlMeta hm = new HtmlMeta();
                HtmlMeta kw = new HtmlMeta();
                HtmlMeta lg = new HtmlMeta();
                HtmlLink cn = new HtmlLink();
                HtmlHead head = (HtmlHead)Page.Header;
               
                hm.Name = "Description";
                hm.Content = "Find and post for free all local events locales trips adventures and bulletins in your city. ";
                head.Controls.AddAt(0, hm);

                kw.Name = "keywords";
                kw.Content = "event happening trip adventure locale venue city";
                head.Controls.AddAt(0, kw);

                lg.Name = "Language";
                lg.Content = "English";
                head.Controls.AddAt(0, lg);

                cn.Attributes.Add("rel", "canonical");
                cn.Href = "http://hippohappenings.com";
                head.Controls.AddAt(0, cn);

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
                #endregion

                DataSet ds = GetEvents(TimeFrame.Beginning);
                
                PostEvents(ds);

                TimeFrameLabel.Text = "<span class='HomeTitle'>F</span>eatured <span class='HomeTitle'>E</span>vents <span class='HomeTitle'>N</span>ear <span class='HomeTitle'>Y</span>ou";

                //DoMayors();

                //if (Session["User"] != null)
                //    if (Session["User"].ToString() == "80" || Session["User"].ToString() == "307")
                //        DoCraigslist();

            }
            catch (Exception ex)
            {
                ErrorLabel.Text += ex.ToString();
            }
        }
    }

    protected void DoCraigslist()
    {
        CLPanel.Visible = true;
        Data dat = new Data(DateTime.Now);
        ArrayList clEvents = dat.grabEvents();

        Label lab;
        RadRotatorItem item;
        foreach (string token in clEvents)
        {
            lab = new Label();
            lab.Text = token;

            item = new RadRotatorItem();
            item.Controls.Add(lab);

            RadRotator3.Items.Add(item);
        }
    }

    protected void GetAllZipsInProximity(out string zips)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn);

        string zip = GetZip();

        int zipParam = 0;

        zips = "";

        
        if (Session["LocCountry"].ToString() == "223")
        {
            DataView dvAllZips = dat.GetDataDV("SELECT * FROM MajorZips MZ, MajorCities MC WHERE " +
                "MZ.MajorCityID=MC.ID AND MajorCityZip='" + zip + "'");

            //If user's zip falls under a major city, just get all the zips that apply to that city
            if (dvAllZips.Count > 0)
            {
                DataView dvAllZipsInCity = dat.GetDataDV("SELECT * FROM MajorZips MZ, MajorCities MC "+
                    "WHERE MZ.MajorCityID=MC.ID AND MC.MajorCity='" + dvAllZips[0]["MajorCity"].ToString() + 
                    "' AND MC.State='" + dvAllZips[0]["State"].ToString() + "'");
                foreach (DataRowView row in dvAllZipsInCity)
                {
                    zips += row["MajorCityZip"].ToString() + ";";
                }
            }
            //If zip doesn't fall under a major city, get the closest major city
            //Zip array will include all zips from that major city and all zips 
            //the same distance away as the current zip.
            else
            {
                DataView dvLatsLongs = dat.GetDataDV("SELECT Latitude, Longitude FROM ZipCodes WHERE Zip='" +
                        zip + "'");

                //If not found, find closest Latitude and Longitude
                zipParam = int.Parse(zip);
                if (dvLatsLongs.Count == 0)
                {
                    dvLatsLongs = null;
                    while (dvLatsLongs == null)
                    {
                        dvLatsLongs = dat.GetDataDV("SELECT Latitude, Longitude FROM ZipCodes WHERE Zip='" + zipParam++ + "'");
                        if (dvLatsLongs.Count > 0)
                        {

                        }
                        else
                        {
                            dvLatsLongs = null;
                        }
                    }
                }

                DataView dvMajors = dat.GetDataDV("SELECT dbo.GetDistance(" + dvLatsLongs[0]["Longitude"].ToString() +
                    ", " + dvLatsLongs[0]["Latitude"].ToString() + ", ZC.Longitude, ZC.Latitude) AS " +
                    "Distance, ZC.Zip, MC.MajorCity, MC.State, MZ.MajorCityID, ZC.Longitude, ZC.Latitude FROM MajorZips MZ, ZipCodes ZC, MajorCities MC " +
                    "WHERE MZ.MajorCityZip=ZC.Zip AND MZ.MajorCityID=MC.ID ORDER BY Distance ASC");

                //All zips in the closest major city
                DataView dvAllZipsInCity = dat.GetDataDV("SELECT * FROM MajorZips MZ, MajorCities MC " +
                    "WHERE MZ.MajorCityID=MC.ID AND MC.MajorCity='" + dvMajors[0]["MajorCity"].ToString() +
                    "' AND MC.State='" + dvMajors[0]["State"].ToString() + "'");
                foreach (DataRowView row in dvAllZipsInCity)
                {
                    zips += row["MajorCityZip"].ToString() + ";";
                }

                //All zips the same distance away from major city as the current zip
                DataView dvAllZipsSameDistance = dat.GetDataDV("SELECT dbo.GetDistance(" + dvMajors[0]["Longitude"].ToString() +
                    ", " + dvMajors[0]["Latitude"].ToString() + ", ZC.Longitude, ZC.Latitude) AS " +
                    "Distance, ZC.Zip FROM ZipCodes ZC " +
                    "WHERE dbo.GetDistance(" + dvMajors[0]["Longitude"].ToString() +
                    ", " + dvMajors[0]["Latitude"].ToString() + ", ZC.Longitude, ZC.Latitude) <= 100");

                foreach (DataRowView row in dvAllZipsSameDistance)
                {
                    zips += row["Zip"].ToString() + ";";
                }
            }
        }
        
    }

    protected string GetZip()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn);
        string zip = "";
        if (Session["User"] != null)
        {
            DataView dvUser = dat.GetDataDV("SELECT * FROM UserPreferences WHERE UserID=" + Session["User"].ToString());
            zip = dvUser[0]["CatZip"].ToString();
        }
        else
        {
            if (Session["LocCountry"].ToString() == "223")
            {
                DataView Zips = dat.GetDataDV("SELECT Zip, State, Longitude, Latitude, dbo.GetDistance(" +
                    Session["LocLong"].ToString() + ", " + Session["LocLat"].ToString() +
                    ", Longitude, Latitude) AS Expr1 FROM ZipCodes ORDER BY Expr1");

                zip = Zips[0]["Zip"].ToString();
            }
        }

        return zip;
    }

    protected void GetMajorLocation(out string zip, out string city, out string state, out string country)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn);
        zip = GetZip();

        int zipParam = 0;

        if (Session["LocCountry"].ToString() == "223")
        {
            DataView dvAllZips = dat.GetDataDV("SELECT * FROM MajorZips MZ, MajorCities MC, State S WHERE " +
                "MZ.MajorCityID=MC.ID AND S.state_name=MC.State AND MajorCityZip='" + zip + "'");

            if (dvAllZips.Count > 0)
            {
                city = dvAllZips[0]["MajorCity"].ToString();
                state = dvAllZips[0]["state_2_code"].ToString();
                country = "223";
            }
            else
            {
                DataView dvLatsLongs = dat.GetDataDV("SELECT Latitude, Longitude FROM ZipCodes WHERE Zip='" +
                        zip + "'");

                //If not found, find closest Latitude and Longitude
                zipParam = int.Parse(zip);
                if (dvLatsLongs.Count == 0)
                {
                    dvLatsLongs = null;
                    while (dvLatsLongs == null)
                    {
                        dvLatsLongs = dat.GetDataDV("SELECT Latitude, Longitude FROM ZipCodes WHERE Zip='" + zipParam++ + "'");
                        if (dvLatsLongs.Count > 0)
                        {

                        }
                        else
                        {
                            dvLatsLongs = null;
                        }
                    }
                }

                DataView dvMajors = dat.GetDataDV("SELECT dbo.GetDistance(" + dvLatsLongs[0]["Longitude"].ToString() +
                    ", " + dvLatsLongs[0]["Latitude"].ToString() + ", ZC.Longitude, ZC.Latitude) AS " +
                    "Distance, ZC.Zip, MC.MajorCity, S.state_2_code, MZ.MajorCityID FROM State S, MajorZips MZ, ZipCodes ZC, MajorCities MC " +
                    "WHERE S.state_name=MC.State AND MZ.MajorCityZip=ZC.Zip AND MZ.MajorCityID=MC.ID ORDER BY Distance ASC");

                city = dvMajors[0]["MajorCity"].ToString();
                state = dvMajors[0]["state_2_code"].ToString();
                country = "223";
            }
        }
        else
        {
            city = Session["LocCity"].ToString();
            state = Session["LocState"].ToString();
            country = Session["LocCountry"].ToString();
        }
    }

    //protected void DoMayors()
    //{
    //    HttpCookie cookie = Request.Cookies["BrowserDate"];
    //    if (cookie == null)
    //    {
    //        cookie = new HttpCookie("BrowserDate");
    //        cookie.Value = DateTime.Now.Date.ToString();
    //        cookie.Expires = DateTime.Now.AddDays(22);
    //        Response.Cookies.Add(cookie);
    //    }
    //    bool fillUserData = false;

    //    DateTime isn = DateTime.Now;

    //    if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
    //        isn = DateTime.Now;
    //    DateTime isNow = isn;
    //    Data dat = new Data(isn);
    //    bool mayorsNotStartedYet = false;

        
    //    //Find the proper major city

    //    string zip = "";
    //    string city = "";
    //    string state = "";
    //    string country = "";

    //    GetMajorLocation(out zip, out city, out state, out country);

    //    DataView dvMayors = dat.GetDataDV("SELECT * FROM Mayors M, MayorDetails MD, Users U WHERE " +
    //        "U.User_ID=M.UserID AND MD.MayorsID=M.ID AND M.City = '" +
    //        city + "' AND M.State = '" + state + "' AND M.Country=" + country +
    //        " AND M.MayorDate = '" + isNow.Month.ToString() + "/1/" + isNow.Year.ToString() + "'");

    //    MayorsLiteral.Text = "<div class=\"topDiv Home34\" align=\"center\">";

    //    string w = "0";
    //    string h = "0";

    //    DataView dvControl = dat.GetDataDV("SELECT * FROM MayorsControl");

    //    string link = "";

    //    if (bool.Parse(dvControl[0]["MayorsOn"].ToString()))
    //    {

    //        if (mayorsNotStartedYet)
    //        {
    //            MayorsLiteral.Text += "<div class=\"Home35\">This is the first month the Hippo is starting the Hippo Boss awards. " +
    //                "All winners get to be featured on our home page, choose thier photo, text and link " +
    //                "to any page they desire. You can even use this feature to promote your business, service or cause. <a class=\"NavyLink Home23\" href=\"hippo-points\">Find out more about hippo bosses and hippo points</a>.</div>";
    //        }
    //        else
    //        {
    //            if (dvMayors.Count == 0)
    //            {
    //                MayorsLiteral.Text += "<div class=\"Text12 Home36\">There are no people enrolled in the Hippo Boss award in your location. Bummer, because, you could be taking up this spot right now and promoting your cause, service, business or just you... all for being the most active user on the Hippo in your location. <br/><a href=\"hippo-points\" class=\"NavyLink Home23\">learn more about hippo bosses and hippo points</a>.</div>";
    //            }
    //            else
    //            {
    //                if (dvMayors[0]["UserChosenLink"].ToString().Trim() != "")
    //                {
    //                    string srt2 = dvMayors[0]["UserChosenLink"].ToString();
    //                    if (srt2.Substring(0, 7).ToLower() != "http://" && srt2.Substring(0, 8).ToLower() != "https://")
    //                        srt2 = "http://" + srt2;

    //                    link = "<div class=\"Home37\"><a target=\"_blank\" class=\"NavyLink12\" href=\"" + srt2 +
    //                        "\"><b>Go to " + dvMayors[0]["UserName"].ToString() + "'s website</b></a>" +
    //                           "</div>";
    //                }

    //                MayorsLiteral.Text += "<div align=\"left\" class=\"Home36\">" +
    //                           "<div class=\"FooterBottom\"><div align=\"center\" class=\"Home38\">" + GetMayorImage(dvMayors[0]["UserName"].ToString(),
    //                           dvMayors[0]["UserChosenPicture"].ToString()) + "</div><span class=\"Home39\"><a class=\"NavyLink12\"  href=\"" + dvMayors[0]["UserName"].ToString() + "_Friend\"><b>" + dvMayors[0]["UserName"].ToString() + "</b></a> is the local winner of " +
    //                           "this month's Hippo Boss award. Here's more about " + dvMayors[0]["UserName"].ToString() + ":</span><br/> " +
    //                           dvMayors[0]["UserChosenText"].ToString() +
    //                               "</div>" + link +
    //                           "</div>";
    //            }


    //            dvMayors = dat.GetDataDV("SELECT * FROM Mayors M, MayorDetails MD, Users U WHERE M.UserID=U.User_ID AND M.ID=MD.MayorsID " +
    //                "AND M.IsGlobal = 'True' AND M.MayorDate = '" +
    //                isNow.Month.ToString() + "/1/" + isNow.Year.ToString() + "'");

    //            if (dvMayors.Count == 0)
    //            {
    //                MayorsLiteral.Text += "<div class=\"Text12\" class=\"Home36\">There are no people enrolled in the Hippo Boss award in your location. Bummer, because, you could be taking up this spot right now and promoting your cause, service, business or just you... all for being the most active user on the Hippo in your location. <br/><a class=\"NavyLink Home23\" href=\"hippo-points\">learn more about hippo bosses and hippo points</a>.</div>";
    //            }
    //            else
    //            {
    //                if (dvMayors[0]["UserChosenLink"].ToString().Trim() != "")
    //                {
    //                    string srt = dvMayors[0]["UserChosenLink"].ToString();
    //                    if (srt.Substring(0, 7).ToLower() != "http://" && srt.Substring(0, 8).ToLower() != "https://")
    //                        srt = "http://" + srt;
    //                    link = "<div class=\"Home37\"><a target=\"_blank\" class=\"NavyLink12\" href=\"" + srt +
    //                        "\"><b>Go to " + dvMayors[0]["UserName"].ToString() + "'s website</b></a>" +
    //                           "</div>";
    //                }
    //                MayorsLiteral.Text += "<div align=\"left\" class=\"Home36\">" +
    //                           "<div class=\"FooterBottom\"><div align=\"center\" class=\"Home40\">" + GetMayorImage(dvMayors[0]["UserName"].ToString(),
    //                           dvMayors[0]["UserChosenPicture"].ToString()) + "</div><span class=\"Home39\"><a class=\"NavyLink12\" href=\"" + dvMayors[0]["UserName"].ToString() + 
    //                           "_Friend\"><b>" + dvMayors[0]["UserName"].ToString() + "</b></a> is the global winner of " +
    //                           "this month's Hippo Boss award. Here's more about " + dvMayors[0]["UserName"].ToString() + ":</span><br/> " +
    //                           dvMayors[0]["UserChosenText"].ToString() +
    //                               "</div>" + link +
    //                               "</div>";
    //            }
    //        }
    //    }
    //    else
    //    {
    //        MayorsLiteral.Text += "<div class=\"Text12 Home41\">The Hippo Bosses are currently being calculated for this month. " +
    //                "Find out more about <a href=\"hippo-points\" class=\"NavyLink Home23\">Hippo Points and Hippo Bosses</a>.</div>";
    //    }

    //    MayorsLiteral.Text += "</div>";
    //}

    protected void SelectToday(object sender, EventArgs e)
    {
        DataSet ds = GetEvents(TimeFrame.Today);
        PostEvents(ds);
    }

    protected void SelectTomorrow(object sender, EventArgs e)
    {
        DataSet ds = GetEvents(TimeFrame.Tomorrow);
        PostEvents(ds);
    }

    protected void SelectThisWeek(object sender, EventArgs e)
    {
        DataSet ds = GetEvents(TimeFrame.ThisWeek);
        PostEvents(ds);
    }

    protected void SelectThisWeekend(object sender, EventArgs e)
    {
        DataSet ds = GetEvents(TimeFrame.ThisWeekend);
        PostEvents(ds);
    }

    protected void SelectThisMonth(object sender, EventArgs e)
    {
        DataSet ds = GetEvents(TimeFrame.ThisMonth);
        PostEvents(ds);
    }

    protected void SelectNextDays(object sender, EventArgs e)
    {
        if (!(NextDaysInput.Value == null || NextDaysInput.Value == ""))
        {
            double tryDouble = 0.00;

            if (double.TryParse(NextDaysInput.Value, out tryDouble))
            {
                DataSet ds = GetEvents(TimeFrame.NextDays);
                PostEvents(ds);
            }
        }
    }

    protected void SelectHighestPrice(object sender, EventArgs e)
    {

        DataSet ds = GetEvents(TimeFrame.HighestPrice);
        PostEvents(ds);
    }

    protected void PostEvents(DataSet ds)
    {
        try
        {
            HttpCookie cookie = Request.Cookies["BrowserDate"];
            if (cookie == null)
            {
                cookie = new HttpCookie("BrowserDate");
                cookie.Value = DateTime.Now.Date.ToString();
                cookie.Expires = DateTime.Now.AddDays(22);
                Response.Cookies.Add(cookie);
            }
            bool fillUserData = false;

            DateTime isn = DateTime.Now;

            if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
                isn = DateTime.Now;
            DateTime isNow = isn;
            Data dat = new Data(isn);
            Rotator1.Items.Clear();
            DataView dv = new DataView(ds.Tables[0], "", "", DataViewRowState.CurrentRows);

            string content = "";
            bool isPathAbsolute = false;
            string imagePath = "";
            if (dv.Count > 0)
            {
                foreach (DataRowView row in dv)
                {
                    Telerik.Web.UI.RadRotatorItem item = new Telerik.Web.UI.RadRotatorItem();
                    Literal lit = new Literal();
                    if (row["Type"].ToString() == "E")
                    {
                        DataView dsSlider = dat.GetDataDV("SELECT * FROM Event_Slider_Mapping WHERE EventID=" +
                            row["EventID"].ToString());

                        string imageStr = "";

                        bool doImage = false;
                        if (dsSlider.Count > 0)
                        {
                            doImage = true;
                            isPathAbsolute = bool.Parse(dsSlider[0]["ImgPathAbsolute"].ToString());
                            if (isPathAbsolute)
                            {
                                imagePath = dsSlider[0]["PictureName"].ToString();
                            }
                            else
                            {
                                imagePath = "UserFiles/Events/" + row["EventID"].ToString() + "/Slider/" + dsSlider[0]["PictureName"].ToString();

                                if (!System.IO.File.Exists(MapPath(".")+imagePath))
                                {
                                    doImage = true;   
                                }
                            }
                        }

                        if (doImage)
                        {
                            if (isPathAbsolute)
                            {
                                imagePath = dsSlider[0]["PictureName"].ToString();
                            }
                            else
                            {
                                imagePath = "UserFiles/Events/" + row["EventID"].ToString() + "/Slider/" + dsSlider[0]["PictureName"].ToString();
                            }
                            imageStr = "<div align=\"center\" style=\"vertical-align: middle;position: absolute; height: 240px; width: 240px;\"><table width='240px' cellpadding='0' cellspacing='0' height='238px' bgcolor='black'><tr><td valign='center'><a width='240px' href=\"" + dat.MakeNiceName(row["Header"].ToString()) +
                                "_" + row["EventID"].ToString() + "_Event\"" +
                                "\"><img  alt=\"" + dat.MakeNiceName(row["Header"].ToString()).Replace("-", " ") + "\" src=\""
                                + imagePath + "\" /></a></td></tr></table><div style=\"font-weight: bold;color: white;position: absolute;bottom: 0;width: 240px; height: 50px; z-index: 1000;\"><table cellpadding='0' cellspacing='0' height='50px'><tr><td valign='center'><div align='center'>" +
                                row["Header"].ToString() + "</div></td></tr></table></div>" +
                                "<div style=\"vertical-align: middle;color: white;position: absolute; bottom: 0;width: 240px; " +
                                "background-color: black; opacity: .7; filter: alpha(opacity=70);height: 50px;\"></div>" +
                                "</div>";
                            lit.Text = imageStr;
                        }
                        else
                        {

                            DataView dvTime = dat.GetDataDV("SELECT * FROM Event_Occurance WHERE EventID=" +
                                row["EventID"].ToString() + " ORDER BY DateTimeStart DESC");
                            DateTime itEvent = new DateTime();
                            itEvent = DateTime.Parse(dvTime[0]["DateTimeStart"].ToString());
                            string timeStr = GetTimeStr(itEvent);

                            lit.Text = "<div class=\"Home42\"><div class=\"Home43\"><div align=\"center\" class=\"FooterBottom\">" +
                                "<h2 class=\"Event\"><a class=\"HomeEventTitle\" href=\"" + dat.MakeNiceName(row["Header"].ToString()) +
                                "_" + row["EventID"].ToString() + "_Event\">";

                            string headerStr = row["Header"].ToString();

                            if (headerStr.Length > 49)
                            {
                                headerStr = headerStr.Substring(0, 46) + "...";
                            }

                            lit.Text += dat.BreakUpString(headerStr, 20) + "</a></h2></div><div class=\"FooterBottom\">" +
                                imageStr + "<div class=\"Text12 Home44\">";

                            int contentLimit = 105;
                            if (imageStr == "")
                                contentLimit = 281;
                            content = dat.stripHTML(row["Content"].ToString());
                            if (content.Length > contentLimit)
                            {
                                content = content.Substring(0, contentLimit) + "...";
                            }

                            lit.Text += timeStr + "<br/>" +
                                dat.BreakUpString(content.Replace("<br/>", " ").Replace("<br>",
                                " ").Replace("<br />", " ").Replace("<BR>", " ").Replace("<BR />",
                                " ").Replace("<BR/>", " "), 13);

                            lit.Text += "</div><div class=\"ReadMoreHome\"><a class=\"ReadMoreHome\" href=\"" + dat.MakeNiceName(row["Header"].ToString()) +
                                "_" + row["EventID"].ToString() + "_Event\">Read More</a></div></div></div></div>";
                        }

                        item.Controls.Add(lit);
                        RadRotator1.Items.Add(item);
                    }
                    else if (row["Type"].ToString() == "T")
                    {
                        DataView dsSlider = dat.GetDataDV("SELECT * FROM Trip_Slider_Mapping WHERE TripID=" +
                            row["EventID"].ToString());

                        string imageStr = "";
                        bool isImage = false;
                        if (dsSlider.Count > 0)
                        {
                            isImage = true;
                            if (!System.IO.File.Exists(MapPath(".") + "/Trips/" + row["EventID"].ToString() + "/Slider/" + dsSlider[0]["PictureName"].ToString()))
                            {
                                isImage = false;
                            }
                        }

                        if (isImage)
                        {
                            imageStr = "<div align=\"center\" style=\"vertical-align: middle; width: 240px; height: 240px;\"><table width='240px' cellpadding='0' cellspacing='0' height='238px' bgcolor='black'><tr><td valign='center'><a width='240px' href=\"" + dat.MakeNiceName(row["Header"].ToString()) +
                                "_" + row["EventID"].ToString() + "_Trip\"" +
                                "\"><img  alt=\"" + dat.MakeNiceName(row["Header"].ToString()).Replace("-", " ") + "\" src=\""
                                + "Trips/" + row["EventID"].ToString() + "/Slider/" + dsSlider[0]["PictureName"].ToString() + "\" /></a></td></tr></table>" +
                                "<div style=\"font-weight: bold;color: white;position: absolute;bottom: 0;width: 240px; height: 50px; z-index: 1000;\"><table cellpadding='0' cellspacing='0' height='50px'><tr><td valign='center'><div align='center'>" +
                                row["Header"].ToString() + "</div></td></tr></table></div>" +
                                "<div style=\"vertical-align: middle;color: white;position: absolute; bottom: 0;width: 240px; " +
                                "background-color: black; opacity: .7; filter: alpha(opacity=70);height: 50px;\"></div>" +
                                "</div>";
                            lit.Text = imageStr;
                        }
                        else
                        {


                            lit.Text = "<div class=\"Home42\"><div class=\"Home43\"><div align=\"center\" class=\"FooterBottom\">" +
                                "<h2 class=\"Event\"><a class=\"HomeEventTitle\" href=\"" + dat.MakeNiceName(row["Header"].ToString()) +
                                "_" + row["EventID"].ToString() + "_Trip\">";

                            string headerStr = row["Header"].ToString();

                            if (headerStr.Length > 49)
                            {
                                headerStr = headerStr.Substring(0, 46) + "...";
                            }

                            lit.Text += dat.BreakUpString(headerStr, 20) + "</a></h2></div><div class=\"FooterBottom\">" +
                                imageStr + "<div  class=\"Home44\" class=\"Text12\">";

                            string timeStr = "";
                            DateTime itEvent = new DateTime();
                            itEvent = DateTime.Parse(row["StartTime"].ToString());
                            timeStr = "<span class='HomeTime'>" + GetHourStr(itEvent) + "</span>";
                            itEvent = DateTime.Parse(row["EndTime"].ToString());
                            timeStr += "<span class='HomeTime'> to " + GetHourStr(itEvent) + "</span><span class=\"MoreTimes\"> + more times</span>";

                            int contentLimit = 105;
                            if (imageStr == "")
                                contentLimit = 281;
                            content = dat.stripHTML(row["Content"].ToString());
                            if (content.Length > contentLimit)
                            {
                                content = content.Substring(0, contentLimit) + "...";
                            }

                            lit.Text += timeStr + "<br/>" +
                                dat.BreakUpString(content.Replace("<br/>", " ").Replace("<br>",
                                " ").Replace("<br />", " ").Replace("<BR>", " ").Replace("<BR />",
                                " ").Replace("<BR/>", " "), 13);

                            lit.Text += "</div><div  class=\"ReadMoreHome\"><a class=\"ReadMoreHome\" href=\"" + dat.MakeNiceName(row["Header"].ToString()) +
                                "_" + row["EventID"].ToString() + "_Trip\">Read More</a></div></div></div></div>";
                        }
                        item.Controls.Add(lit);
                        RadRotator2.Items.Add(item);
                    }
                    else
                    {
                        DataView dsSlider = dat.GetDataDV("SELECT * FROM Venue_Slider_Mapping WHERE VenueID=" +
                                row["EventID"].ToString());

                        string imageStr = "";
                        bool isImage = false;
                        if (dsSlider.Count > 0)
                        {
                            isImage = true;
                            if (!System.IO.File.Exists(MapPath(".") + "VenueFiles/" + row["EventID"].ToString() + "/Slider/" + dsSlider[0]["PictureName"].ToString()))
                            {
                                isImage = false;
                            }
                        }
                        if (isImage)
                        {
                            imageStr = "<div align=\"center\" style=\"vertical-align: middle; width: 240px; height: 240px;\"><table width='240px' cellpadding='0' cellspacing='0' height='238px' bgcolor='black'><tr><td valign='center'><a width='240px' href=\"" + dat.MakeNiceName(row["Header"].ToString()) +
                                "_" + row["EventID"].ToString() + "_Venue\"" +
                                "\"><img  alt=\"" + dat.MakeNiceName(row["Header"].ToString()).Replace("-", " ") + "\" src=\""
                                + "VenueFiles/" + row["EventID"].ToString() + "/Slider/" + dsSlider[0]["PictureName"].ToString() + "\" /></a></td></tr></table>"+
                                "<div style=\"font-weight: bold;color: white;position: absolute;bottom: 0;width: 240px; height: 50px; z-index: 1000;\"><table cellpadding='0' cellspacing='0' height='50px'><tr><td valign='center'><div align='center'>" +
                                row["Header"].ToString() + "</div></td></tr></table></div>" +
                                "<div style=\"vertical-align: middle;color: white;position: absolute; bottom: 0;width: 240px; " +
                                "background-color: black; opacity: .7; filter: alpha(opacity=70);height: 50px;\"></div>" +
                                "</div>";
                            lit.Text = imageStr;
                        }
                        else
                        {
                            string timeStr = "";
                            DateTime itEvent = new DateTime();
                            itEvent = DateTime.Parse(row["StartTime"].ToString());
                            timeStr = "<span class='HomeTime'>" + GetHourStr(itEvent) + "</span>";
                            itEvent = DateTime.Parse(row["EndTime"].ToString());
                            timeStr += "<span class='HomeTime'> to " + GetHourStr(itEvent) + "</span><span class=\"MoreTimes\"> + more times</span>";

                            lit.Text = "<div  class=\"Home42\"><div class=\"Home43\"><div align=\"center\" class=\"FooterBottom\">" +
                                "<h2 class=\"Event\"><a class=\"HomeEventTitle\" href=\"" + dat.MakeNiceName(row["Header"].ToString()) +
                                "_" + row["EventID"].ToString() + "_Venue\">";

                            string headerStr = row["Header"].ToString();

                            if (headerStr.Length > 49)
                            {
                                headerStr = headerStr.Substring(0, 46) + "...";
                            }

                            lit.Text += dat.BreakUpString(headerStr, 20) + "</a></h2></div><div class=\"FooterBottom\">" +
                                imageStr + "<div  class=\"Home44\" class=\"Text12\">";

                            int contentLimit = 105;
                            if (imageStr == "")
                                contentLimit = 281;
                            content = dat.stripHTML(row["Content"].ToString());
                            if (content.Length > contentLimit)
                            {
                                content = content.Substring(0, contentLimit) + "...";
                            }

                            lit.Text += timeStr + "<br/>" +
                                dat.BreakUpString(content.Replace("<br/>", " ").Replace("<br>",
                                " ").Replace("<br />", " ").Replace("<BR>", " ").Replace("<BR />",
                                " ").Replace("<BR/>", " "), 13);

                            lit.Text += "</div><div  class=\"ReadMoreHome\"><a class=\"ReadMoreHome\" href=\"" + dat.MakeNiceName(row["Header"].ToString()) +
                                "_" + row["EventID"].ToString() + "_Venue\">Read More</a></div></div></div></div>";

                        }
                        item.Controls.Add(lit);
                        Rotator1.Items.Add(item);

                    }
                    
                }

                if (RadRotator2.Controls.Count == 0)
                {
                    Literal lit = new Literal();
                    lit.Text = "<div class=\"Home42\"><div class=\"Home43\"><div align=\"center\" class=\"FooterBottom\">";

                    string headerStr = "<div style=\"padding-top: 55px;\">There are currently no adventures "+
                        "posted in your location. But you can add them! <a class=\"NavyLinkSmall\" "+
                        "href=\"enter-trip\"><br/>+Add Adventure</a></div>";

                    lit.Text += dat.BreakUpString(headerStr, 20) + "</div><div class=\"FooterBottom\"><div "+
                        "class=\"Home44\" class=\"Text12\">";

                    lit.Text += "</div></div></div></div>";

                    RadRotatorItem item = new RadRotatorItem();
                    item.Controls.Add(lit);
                    RadRotator2.Items.Add(item);
                }

                if (RadRotator1.Controls.Count == 0)
                {
                    Literal lit = new Literal();
                    lit.Text = "<div class=\"Home42\"><div class=\"Home43\"><div align=\"center\" class=\"FooterBottom\">";

                    string headerStr = "<div style=\"padding-top: 55px;\">There are currently no events " +
                        "posted in your location. But you can add them! <a class=\"NavyLinkSmall\" "+
                        "href=\"blog-event\"><br/>+Add Event</a></div>";

                    lit.Text += dat.BreakUpString(headerStr, 20) + "</div><div class=\"FooterBottom\"><div " +
                        "class=\"Home44\" class=\"Text12\">";

                    lit.Text += "</div></div></div></div>";

                    RadRotatorItem item = new RadRotatorItem();
                    item.Controls.Add(lit);
                    RadRotator1.Items.Add(item);
                }

                if (Rotator1.Controls.Count == 0)
                {
                    Literal lit = new Literal();
                    lit.Text = "<div class=\"Home42\"><div class=\"Home43\"><div align=\"center\" class=\"FooterBottom\">";

                    string headerStr = "<div style=\"padding-top: 55px;\">There are currently no locales " +
                        "posted in your location. But you can add them! <a class=\"NavyLinkSmall\" "+
                        "href=\"enter-locale\"><br/>+Add Locale</a></div>";

                    lit.Text += dat.BreakUpString(headerStr, 20) + "</div><div class=\"FooterBottom\"><div " +
                        "class=\"Home44\" class=\"Text12\">";

                    lit.Text += "</div></div></div></div>";

                    RadRotatorItem item = new RadRotatorItem();
                    item.Controls.Add(lit);
                    Rotator1.Items.Add(item);
                }

                //if (ds.Tables[0].Rows.Count < 4)
                //{
                //    if (ds.Tables[0].Rows.Count == 1)
                //    {

                //        Literal lit = new Literal();
                //        lit.Text = "<div class=\"Text12 Home45\">" +
                //            "<div  class=\"AdEmptyInner\">There aren't many events posted " +
                //            "in your location for this time frame. Be the first to post an event and have it featured on our home page. " +
                //            "<br/><a class=\"NavyLinkSmall\" href=\"blog-event\"><br/>+Add Event</a><a class=\"NavyLinkSmall\" href=\"enter-locale\"><br/>+Add Locale</a>" +
                //            "<a class=\"NavyLinkSmall\" href=\"enter-trip\"><br/>+Add Adventure</a></div></div>";
                //        Telerik.Web.UI.RadRotatorItem item = new Telerik.Web.UI.RadRotatorItem();
                //        item.Controls.Add(lit);

                //        Rotator1.Items.Add(item);
                //        RadRotator1.Items.Add(item);
                //        RadRotator2.Items.Add(item);

                //        lit = new Literal();
                //        lit.Text = "<div class=\"Text12 Home46\">" +
                //            "</div>";
                //        item = new Telerik.Web.UI.RadRotatorItem();
                //        item.Controls.Add(lit);

                //        Rotator1.Items.Add(item);
                //        RadRotator1.Items.Add(item);
                //        RadRotator2.Items.Add(item);

                //        lit = new Literal();
                //        lit.Text = "<div class=\"Text12 Home47\">" +
                //            "</div>";
                //        item = new Telerik.Web.UI.RadRotatorItem();
                //        item.Controls.Add(lit);

                //        Rotator1.Items.Add(item);
                //        RadRotator1.Items.Add(item);
                //        RadRotator2.Items.Add(item);
                //    }
                //    else if (ds.Tables[0].Rows.Count == 2)
                //    {
                //        Literal lit = new Literal();
                //        lit.Text = "<div class=\"Text12 Home45\">" +
                //            "<div  class=\"AdEmptyInner\">There aren't many events posted in your location for this time frame. Be the first to post an event and have " +
                //            "it featured on our home page. " +
                //            "<br/><a class=\"NavyLinkSmall\" href=\"blog-event\"><br/>+Add Event</a>" +
                //            "<a class=\"NavyLinkSmall\" href=\"enter-locale\"><br/>+Add Locale</a>" +
                //            "<a class=\"NavyLinkSmall\" href=\"enter-trip\"><br/>+Add Adventure</a></div></div>";
                //        Telerik.Web.UI.RadRotatorItem item = new Telerik.Web.UI.RadRotatorItem();
                //        item.Controls.Add(lit);

                //        Rotator1.Items.Add(item);
                //        RadRotator1.Items.Add(item);
                //        RadRotator2.Items.Add(item);

                //        lit = new Literal();
                //        lit.Text = "<div class=\"Home47\">" +
                //            "</div>";
                //        item = new Telerik.Web.UI.RadRotatorItem();
                //        item.Controls.Add(lit);

                //        Rotator1.Items.Add(item);
                //        RadRotator1.Items.Add(item);
                //        RadRotator2.Items.Add(item);
                //    }
                //    else if (ds.Tables[0].Rows.Count == 3)
                //    {
                //        Literal lit = new Literal();
                //        lit.Text = "<div class=\"Text12 Home48\">" +
                //            "<div  class=\"AdEmptyInner\">There aren't many events posted in your location for this time frame. Be the first to post an event and have it featured on our home page. " +
                //            "<br/><a class=\"NavyLinkSmall\" href=\"blog-event\"><br/>+Add Event</a>" +
                //            "<a class=\"NavyLinkSmall\" href=\"enter-locale\"><br/>+Add Locale</a>" +
                //            "<a class=\"NavyLinkSmall\" href=\"enter-trip\"><br/>+Add Adventure</a></div></div>";
                //        Telerik.Web.UI.RadRotatorItem item = new Telerik.Web.UI.RadRotatorItem();
                //        item.Controls.Add(lit);

                //        Rotator1.Items.Add(item);
                //        RadRotator1.Items.Add(item);
                //        RadRotator2.Items.Add(item);
                //    }
                //}
            }
            else
            {
                
                Literal lit = new Literal();
                lit.Text = "<div class=\"Text12 Home45\">" +
                    "<div  class=\"AdEmptyInner\">There are no posted events in your location for this time frame. Be the first to post an event and have it featured on our home page. " +
                            "<br/><a class=\"NavyLinkSmall\" href=\"blog-event\"><br/>+Add Event</a>" +
                            "<a class=\"NavyLinkSmall\" href=\"enter-locale\"><br/>+Add Locale</a>" +
                            "<a class=\"NavyLinkSmall\" href=\"enter-trip\"><br/>+Add Adventure</a></div></div>";
                Telerik.Web.UI.RadRotatorItem item = new Telerik.Web.UI.RadRotatorItem();
                item.Controls.Add(lit);

                Rotator1.Items.Add(item);
                RadRotator1.Items.Add(item);
                RadRotator2.Items.Add(item);

                lit = new Literal();
                lit.Text = "<div  class=\"Home49\">" +
                    "</div>";
                item = new Telerik.Web.UI.RadRotatorItem();
                item.Controls.Add(lit);

                Rotator1.Items.Add(item);
                RadRotator1.Items.Add(item);
                RadRotator2.Items.Add(item);

                lit = new Literal();
                lit.Text = "<div  class=\"Home49\">" +
                    "</div>";
                item = new Telerik.Web.UI.RadRotatorItem();
                item.Controls.Add(lit);

                Rotator1.Items.Add(item);
                RadRotator1.Items.Add(item);
                RadRotator2.Items.Add(item);

                lit = new Literal();
                lit.Text = "<div class=\"Home50\">" +
                    "</div>";
                item = new Telerik.Web.UI.RadRotatorItem();
                item.Controls.Add(lit);

                Rotator1.Items.Add(item);
                RadRotator1.Items.Add(item);
                RadRotator2.Items.Add(item);
            }
        }
        catch (Exception ex)
        {
            TimeFrameLabel.Text += ex.ToString();
        }
    }

    protected string GetTimeStr(DateTime itEvent)
    {
        string timeStr = "";
        Data dat = new Data(DateTime.Now);
        timeStr += GetHourStr(itEvent);
        timeStr = "<span class='HomeTime'>" + dat.GetDay(itEvent.Day, true) + " " +
            itEvent.Month.ToString() + "/" + itEvent.Day.ToString() + " at " + timeStr + " </span>";
        return timeStr;
    }

    protected string GetHourStr(DateTime itEvent)
    {
        string pOrA = "a";
        string timeStr = itEvent.Hour.ToString();
        if (itEvent.Hour >= 12)
        {
            pOrA = "p";
            if (itEvent.Hour != 12)
                timeStr = (itEvent.Hour - 12).ToString();
        }
        if (itEvent.Hour == 0)
            timeStr = "12";
        if (itEvent.Minute > 0)
            timeStr += ":" + itEvent.Minute.ToString();
        timeStr += pOrA;
        return timeStr;
    }

    protected DataSet GetEvents(TimeFrame theEnum)
    {
        string message = "";
        try
        {
            DateTime StartDate = new DateTime();
            DateTime EndDate = new DateTime();

            HttpCookie cookie = Request.Cookies["BrowserDate"];
            if (cookie == null)
            {
                cookie = new HttpCookie("BrowserDate");
                cookie.Value = DateTime.Now.Date.ToString();
                cookie.Expires = DateTime.Now.AddDays(22);
                Response.Cookies.Add(cookie);
            }

            int totalCount = 20;

            bool fillUserData = false;

            string timeframe = "";

            DateTime isn = DateTime.Now;

            if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
                isn = DateTime.Now;
            DateTime itBeNow = isn;
            Data dat = new Data(isn);
            #region Get Events

            int subtraction = 0;
            string startDate = "";
            string endDate = "";

            string highestPrice = "";
            string highestText = "";
            if (theEnum == TimeFrame.HighestPrice)
            {
                highestText = " and <span class='HomeTitle'>H</span>ighest <span class='HomeTitle'>P</span>rice of $" + HighestPriceInput.Value.Replace("$", "");
                highestPrice = " AND (E.MinPrice <= " + HighestPriceInput.Value + " OR E.MinPrice is Null) ";
                theEnum = (TimeFrame)Session["HomeTimeFrame"];
            }

            #region TimeFrame
            switch (theEnum)
            {
                case TimeFrame.Beginning:
                    timeframe = " AND EO.DateTimeStart > '" + isn.Date + "' ";
                    TimeFrameLabel.Text = "<span class='HomeTitle'>F</span>eatured <span class='HomeTitle'>E</span>vents "+ highestText;
                    Session["HomeTimeFrame"] = TimeFrame.Beginning;
                    break;
                case TimeFrame.Today:
                    timeframe = " AND CONVERT(NVARCHAR,Month(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR, DAY(EO.DateTimeStart)) + " +
                        "'/' + CONVERT(NVARCHAR, YEAR(EO.DateTimeStart)) = '" + itBeNow.Month.ToString() + "/" +
                        itBeNow.Day.ToString() + "/" + itBeNow.Year.ToString() + "'  AND EO.DateTimeStart > '" + isn.Date + "' ";
                    TimeFrameLabel.Text = "<span class='HomeTitle'>F</span>eatured <span class='HomeTitle'>E</span>vents <span class='HomeTitle'>T</span>oday" + highestText;
                    Session["HomeTimeFrame"] = TimeFrame.Today;
                    break;
                case TimeFrame.Tomorrow:
                    itBeNow = itBeNow.AddDays(1.00);
                    timeframe = " AND CONVERT(NVARCHAR,Month(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR, DAY(EO.DateTimeStart)) + " +
                        "'/' + CONVERT(NVARCHAR, YEAR(EO.DateTimeStart)) = '" + itBeNow.Month.ToString() + "/" +
                        itBeNow.Day.ToString() + "/" + itBeNow.Year.ToString() + "' AND EO.DateTimeStart > '" + isn.Date + "' ";
                    TimeFrameLabel.Text = "<span class='HomeTitle'>F</span>eatured <span class='HomeTitle'>E</span>vents <span class='HomeTitle'>T</span>omorrow" + highestText;
                    Session["HomeTimeFrame"] = TimeFrame.Tomorrow;
                    break;
                case TimeFrame.ThisWeek:
                    switch (itBeNow.DayOfWeek)
                    {
                        case DayOfWeek.Friday:
                            subtraction = 5;
                            break;
                        case DayOfWeek.Monday:
                            subtraction = 1;
                            break;
                        case DayOfWeek.Saturday:
                            subtraction = 6;
                            break;
                        case DayOfWeek.Sunday:
                            subtraction = 0;
                            break;
                        case DayOfWeek.Thursday:
                            subtraction = 4;
                            break;
                        case DayOfWeek.Tuesday:
                            subtraction = 2;
                            break;
                        case DayOfWeek.Wednesday:
                            subtraction = 3;
                            break;
                        default: break;
                    }
                    startDate = itBeNow.Subtract(TimeSpan.FromDays(subtraction)).ToShortDateString();
                    endDate = itBeNow.AddDays(7.00 - subtraction).ToShortDateString();
                    timeframe = " AND (CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) " +
                        " >= CONVERT(DATETIME, '" + startDate + "') AND CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + " +
                        "CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) <= CONVERT(DATETIME, '" + endDate + "')OR " +
                        "(CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + " +
                        "CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) < " +
                        "CONVERT(DATETIME, '" + startDate + "') AND CONVERT(NVARCHAR,MONTH(EO.DateTimeEnd)) + '/' + " +
                        "CONVERT(NVARCHAR,DAY(EO.DateTimeEnd)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeEnd)) > CONVERT(DATETIME, '" + startDate + "'))) AND EO.DateTimeStart > '" + isn.Date + "' ";
                    TimeFrameLabel.Text = "<span class='HomeTitle'>F</span>eatured <span class='HomeTitle'>E</span>vents <span class='HomeTitle'>T</span>his <span class='HomeTitle'>W</span>eek" + highestText;
                    Session["HomeTimeFrame"] = TimeFrame.ThisWeek;
                    break;
                case TimeFrame.ThisWeekend:
                    switch (itBeNow.DayOfWeek)
                    {
                        case DayOfWeek.Friday:
                            subtraction = 0;
                            break;
                        case DayOfWeek.Monday:
                            subtraction = -4;
                            break;
                        case DayOfWeek.Saturday:
                            subtraction = 1;
                            break;
                        case DayOfWeek.Sunday:
                            subtraction = 2;
                            break;
                        case DayOfWeek.Thursday:
                            subtraction = -1;
                            break;
                        case DayOfWeek.Tuesday:
                            subtraction = -3;
                            break;
                        case DayOfWeek.Wednesday:
                            subtraction = -2;
                            break;
                        default: break;
                    }
                    startDate = itBeNow.Subtract(TimeSpan.FromDays(subtraction)).ToShortDateString();
                    endDate = itBeNow.AddDays(2 - subtraction).ToShortDateString();
                    timeframe = " AND (CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) " +
                        "+ '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) " +
                        " >= CONVERT(DATETIME, '" + startDate + "') AND CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + " +
                        "'/' + CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) " +
                        "<= CONVERT(DATETIME, '" + endDate + "') OR " +
                        "(CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + " +
                        "CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) < " +
                        "CONVERT(DATETIME, '" + startDate + "') AND CONVERT(NVARCHAR,MONTH(EO.DateTimeEnd)) + '/' + " +
                        "CONVERT(NVARCHAR,DAY(EO.DateTimeEnd)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeEnd)) > CONVERT(DATETIME, '" + startDate + "'))) AND EO.DateTimeStart > '" + isn.Date + "' ";
                    TimeFrameLabel.Text = "<span class='HomeTitle'>F</span>eatured <span class='HomeTitle'>E</span>vents <span class='HomeTitle'>T</span>his <span class='HomeTitle'>W</span>eekend" + highestText;
                    Session["HomeTimeFrame"] = TimeFrame.ThisWeekend;
                    break;
                case TimeFrame.ThisMonth:
                    timeframe = " AND (MONTH(EO.DateTimeStart) = '" + itBeNow.Month +
                        "' OR (EO.DateTimeStart < CONVERT(DATETIME,'" + itBeNow.Month +
                        "/"+DateTime.DaysInMonth(itBeNow.Year, itBeNow.Month).ToString()+"/" + 
                        itBeNow.Year + " 23:59:59') AND EO.DateTimeEnd > CONVERT(DATETIME,'" +
                        itBeNow.Month + "/1/" + itBeNow.Year + " 00:00:00'))) AND EO.DateTimeStart > '" + 
                        isn.Date + "' ";
                    TimeFrameLabel.Text = "<span class='HomeTitle'>F</span>eatured <span class='HomeTitle'>E</span>vents <span class='HomeTitle'>T</span>his <span class='HomeTitle'>M</span>onth" + highestText;
                    Session["HomeTimeFrame"] = TimeFrame.ThisMonth;
                    break;
                case TimeFrame.NextDays:
                    StartDate = itBeNow;
                    decimal theDub = 0.00M;
                    if (decimal.TryParse(NextDaysInput.Value, out theDub))
                    {

                        EndDate = itBeNow.AddDays(int.Parse(NextDaysInput.Value));

                        timeframe = " AND (CONVERT(NVARCHAR,MONTH(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,DAY(EO.DateTimeStart)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeStart)) " +
                                                " <= CONVERT(DATETIME, '" + EndDate.Month.ToString() + "/" + EndDate.Day.ToString() + "/" + EndDate.Year.ToString() + " 23:59:59') " +
                                                " AND CONVERT(NVARCHAR,MONTH(EO.DateTimeEnd)) + '/' + CONVERT(NVARCHAR,DAY(EO.DateTimeEnd)) + '/' + CONVERT(NVARCHAR,YEAR(EO.DateTimeEnd)) >= " +
                                                " CONVERT(DATETIME, '" + StartDate.Month.ToString() + "/" + StartDate.Day.ToString() + "/" + StartDate.Year.ToString() + " 00:00:00')) AND EO.DateTimeStart > '" +
                                                isn.Date + "' ";
                        TimeFrameLabel.Text = "<span class='HomeTitle'>F</span>eatured <span class='HomeTitle'>E</span>vents <span class='HomeTitle'>N</span>ext " + NextDaysInput.Value + " <span class='HomeTitle'>D</span>ays" + highestText;
                        Session["HomeTimeFrame"] = TimeFrame.NextDays;
                    }
                    else
                    {
                        TimeFrameLabel.Text = "<span class='HomeTitle'>F</span>eatured <span class='HomeTitle'>E</span>vents "+highestText;
                        timeframe = " AND EO.DateTimeStart > '" + itBeNow.Date.ToString() + "' ";
                    }
                    break;
                default: break;
            }
            #endregion

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
                    if (Session["LocCountry"] == null || Session["LocState"] == null || Session["LocCity"] == null)
                    {
                        DataSet ds1 = dat.GetData("SELECT * FROM Users U, UserPreferences UP WHERE U.User_ID=" +
                            Session["User"].ToString() + " AND U.User_ID=UP.UserID ");

                        country = ds1.Tables[0].Rows[0]["CatCountry"].ToString();
                        countryID = country;

                        state = ds1.Tables[0].Rows[0]["CatState"].ToString();
                        city = ds1.Tables[0].Rows[0]["CatCity"].ToString();
                        stateID = state;
                        cityID = city;
                    }
                    else
                    {
                        country = Session["LocCountry"].ToString().Trim();
                        state = Session["LocState"].ToString().Trim();

                        city = Session["LocCity"].ToString();
                        stateID = state;
                        cityID = city;
                    }

                    fillUserData = true;
                }
                else
                {
                    if (Session["LocCountry"] == null || Session["LocState"] == null || Session["LocCity"] == null)
                    {
                        dat.IP2Location();
                    }

                    country = Session["LocCountry"].ToString().Trim();
                    state = Session["LocState"].ToString().Trim();

                    city = Session["LocCity"].ToString();
                    stateID = state;
                    cityID = city;


                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text += ex.ToString();
            }

            

            DataSet ds;

            string realCountry = country;
            string tripCountry = "";
            if (country != "")
            {
                country = " AND E.Country = " + realCountry;
                tripCountry = " AND TDir.Country = " + realCountry;
            }
            
            string zips = "";

            if (realCountry == "223")
            {
                GetAllZipsInProximity(out zips);

                char[] delim = { ';' };
                string[] zipArray = zips.Split(delim, StringSplitOptions.RemoveEmptyEntries);

                zips = "";

                foreach (string token in zipArray)
                {
                    if (zips == "")
                        zips = " AND (";
                    else
                        zips += " OR ";
                    zips += " Zip = " + token;
                }

                zips += ")";
                
                //TimeFrameLabel.Text = "SELECT DISTINCT TOP " + totalCount.ToString() + " " +
                //"E.Header, EO.DateTimeStart AS StartTime, EO.DateTimeEnd AS EndTime, E.Featured, E.DaysFeatured, E.Content, E.PostedOn, EO.EventID, 'E' AS Type FROM Events E, Event_Occurance EO WHERE " +
                //"E.LIVE='True' AND E.ID=EO.EventID " + country + zips + timeframe + highestPrice +
                //"  ORDER BY E.Featured, E.PostedOn ";
                ds = dat.GetData("SELECT DISTINCT TOP " + totalCount.ToString() + " " +
                "E.Header, EO.DateTimeStart AS StartTime, EO.DateTimeEnd AS EndTime, E.Featured, E.DaysFeatured, E.Content, E.PostedOn, EO.EventID, 'E' AS Type FROM Events E, Event_Occurance EO WHERE " +
                "E.LIVE='True' AND E.ID=EO.EventID " + country + zips + timeframe + highestPrice +
                "  ORDER BY E.Featured, E.PostedOn ");
                
            }
            else
            {
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

                ds = dat.GetDataWithParemeters("SELECT DISTINCT TOP " + totalCount.ToString() + " " +
                "E.Header, E.Featured, EO.DateTimeStart AS StartTime, EO.DateTimeEnd AS EndTime, E.DaysFeatured, E.PostedOn, E.Content, EO.EventID, 'E' AS Type FROM Events E, Event_Occurance EO WHERE " +
                "E.LIVE='True' AND E.ID=EO.EventID " + country + state + city + timeframe + highestPrice +
                " ORDER BY E.Featured, E.PostedOn, EO.DateTimeStart", types, data);
            }

            //totalCount = totalCount - ds.Tables[0].Rows.Count;

            #endregion

            if (totalCount > 0)
            {
                #region Get Trips
                string theDate = "";
                subtraction = 0;
                startDate = "";
                endDate = "";



                string dayToday = itBeNow.Day.ToString();
                string monthTody = itBeNow.Month.ToString();
                string yearToday = itBeNow.Year.ToString();

                string dateToday = monthTody + "/" + dayToday + "/" + yearToday;

                string dayOfWeek = dat.getDayOfWeek(itBeNow.DayOfWeek);

                string timeNow = itBeNow.TimeOfDay.ToString();

                string dayStart = "";
                string dayEnd = "";
                string startMonth = "";
                string endMonth = "";
                string startYear = "";
                string endYear = "";

                string dateStart = "";
                string dateEnd = "";

                string startDayOfWeek = "";
                string endDayOfWeek = "";
                string resultsStr = "";
                string date = "";

                string highestP = "";

                if (theEnum == TimeFrame.HighestPrice)
                {
                    highestP = " AND ((E.MaxPrice <= " + HighestPriceInput.Value.Replace("$", "") +
                        ") OR (E.MaxPrice >= " + HighestPriceInput.Value.Replace("$", "") + " AND E.MinPrice <= " +
                        HighestPriceInput.Value.Replace("$", "") + ")" +
                        "OR (E.MaxPrice IS NULL AND E.MinPrice IS NULL)) ";
                }

                switch (theEnum)
                {
                    case TimeFrame.Beginning:
                        StartDate = itBeNow;
                        EndDate = itBeNow;

                        date = " AND (CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthStart)+'/'+CONVERT(NVARCHAR,TM.DayStart)" +
                            "+'/'+'" + EndDate.Year.ToString() +
                            "') <= CONVERT(DATETIME, '" + EndDate.Month.ToString() + "/" + EndDate.Day.ToString() + "/" + EndDate.Year.ToString() + " 00:00:00') "+
                            " AND CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthEnd)+'/'+CONVERT(NVARCHAR,TM.DayEnd)" +
                            "+'/'+'" + EndDate.Year.ToString() +
                            "') >= CONVERT(DATETIME, '" + EndDate.Month.ToString() + "/" + EndDate.Day.ToString() + "/" + EndDate.Year.ToString() + " 00:00:00') ) ";
                        break;
                    case TimeFrame.Today:
                        resultsStr += " Today ";
                        date = " AND (CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthStart)+'/'+CONVERT(NVARCHAR,TM.DayStart)+'/'+'" + yearToday +
                            "') <= CONVERT(DATETIME, '" + dateToday + "') AND CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthEnd) +" +
                            "'/'+CONVERT(NVARCHAR,TM.DayEnd)+'/'+'" + yearToday +
                            "') >= CONVERT(DATETIME, '" + dateToday + "') AND TDS.Days LIKE '%" + dayOfWeek +
                            "%' AND CONVERT(DATETIME, TDS.StartTime) <= CONVERT(DATETIME, '" + timeNow +
                            "') AND CONVERT(DATETIME, TDS.EndTime) >= CONVERT(DATETIME, '" + timeNow + "') )";
                        break;
                    case TimeFrame.Tomorrow:
                        itBeNow = itBeNow.AddDays(1);
                        dayToday = itBeNow.Day.ToString();
                        monthTody = itBeNow.Month.ToString();
                        yearToday = itBeNow.Year.ToString();

                        dateToday = monthTody + "/" + dayToday + "/" + yearToday;

                        dayOfWeek = dat.getDayOfWeek(itBeNow.DayOfWeek);

                        timeNow = itBeNow.TimeOfDay.ToString();

                        resultsStr += " Tomorrow ";
                        date = " AND (CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthStart)+'/'+CONVERT(NVARCHAR,TM.DayStart)+'/'+'" + yearToday +
                            "') <= CONVERT(DATETIME, '" + dateToday + "') AND CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthEnd) +" +
                            "'/'+CONVERT(NVARCHAR,TM.DayEnd)+'/'+'" + yearToday +
                            "') >= CONVERT(DATETIME, '" + dateToday + "')) AND TDS.Days LIKE '%" + dayOfWeek +
                            "%'";
                        break;
                    case TimeFrame.ThisWeekend:
                        resultsStr += " This Weekend ";
                        switch (DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).DayOfWeek)
                        {
                            case DayOfWeek.Friday:
                                subtraction = 0;
                                break;
                            case DayOfWeek.Monday:
                                subtraction = -4;
                                break;
                            case DayOfWeek.Saturday:
                                subtraction = 1;
                                break;
                            case DayOfWeek.Sunday:
                                subtraction = 2;
                                break;
                            case DayOfWeek.Thursday:
                                subtraction = -1;
                                break;
                            case DayOfWeek.Tuesday:
                                subtraction = -3;
                                break;
                            case DayOfWeek.Wednesday:
                                subtraction = -2;
                                break;
                            default: break;
                        }

                        StartDate = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Subtract(TimeSpan.FromDays(subtraction));
                        EndDate = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(2 - subtraction);

                        dayStart = StartDate.Day.ToString();
                        dayEnd = EndDate.Day.ToString();
                        startMonth = StartDate.Month.ToString();
                        endMonth = EndDate.Month.ToString();
                        startYear = StartDate.Year.ToString();
                        endYear = EndDate.Year.ToString();

                        dateStart = startMonth + "/" + dayStart + "/" + startYear;
                        dateEnd = endMonth + "/" + dayEnd + "/" + endYear;

                        startDayOfWeek = dat.getDayOfWeek(StartDate.DayOfWeek);
                        endDayOfWeek = dat.getDayOfWeek(EndDate.DayOfWeek);

                        date = " AND (CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthStart)+'/'+CONVERT(NVARCHAR,TM.DayStart)+'/'+'" + startYear +
                            "') <= CONVERT(DATETIME, '" + dateStart + "') AND CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthEnd) +" +
                            "'/'+CONVERT(NVARCHAR,TM.DayEnd)+'/'+'" + endYear +
                            "') >= CONVERT(DATETIME, '" + dateEnd + "')) AND (TDS.Days LIKE '%5%' OR TDS.Days LIKE '%6%' OR TDS.Days LIKE '%7%')";
                        break;
                    case TimeFrame.ThisWeek:
                        resultsStr += " This Week ";
                        switch (DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).DayOfWeek)
                        {
                            case DayOfWeek.Friday:
                                subtraction = 5;
                                break;
                            case DayOfWeek.Monday:
                                subtraction = 1;
                                break;
                            case DayOfWeek.Saturday:
                                subtraction = 6;
                                break;
                            case DayOfWeek.Sunday:
                                subtraction = 0;
                                break;
                            case DayOfWeek.Thursday:
                                subtraction = 4;
                                break;
                            case DayOfWeek.Tuesday:
                                subtraction = 2;
                                break;
                            case DayOfWeek.Wednesday:
                                subtraction = 3;
                                break;
                            default: break;
                        }
                        StartDate = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Subtract(TimeSpan.FromDays(subtraction));
                        EndDate = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(7.00 - subtraction);

                        dayStart = StartDate.Day.ToString();
                        dayEnd = EndDate.Day.ToString();
                        startMonth = StartDate.Month.ToString();
                        endMonth = EndDate.Month.ToString();
                        startYear = StartDate.Year.ToString();
                        endYear = EndDate.Year.ToString();

                        dateStart = startMonth + "/" + dayStart + "/" + startYear;
                        dateEnd = endMonth + "/" + dayEnd + "/" + endYear;

                        startDayOfWeek = dat.getDayOfWeek(StartDate.DayOfWeek);
                        endDayOfWeek = dat.getDayOfWeek(EndDate.DayOfWeek);

                        date = " AND (CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthStart)+'/'+CONVERT(NVARCHAR,TM.DayStart)+'/'+'" + startYear +
                            "') <= CONVERT(DATETIME, '" + dateStart + "') AND CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthEnd) +" +
                            "'/'+CONVERT(NVARCHAR,TM.DayEnd)+'/'+'" + endYear +
                            "') >= CONVERT(DATETIME, '" + dateEnd + "')) AND (TDS.Days LIKE '%1%' OR TDS.Days LIKE '%2%' OR TDS.Days LIKE '%3%' OR TDS.Days LIKE '%4%')";
                        break;
                    case TimeFrame.ThisMonth:
                        resultsStr += " This Month ";

                        dateToday = monthTody + "/" + DateTime.DaysInMonth(int.Parse(yearToday), int.Parse(monthTody)).ToString() + "/" + yearToday;
                        date = " AND (CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthStart)+'/'+CONVERT(NVARCHAR,TM.DayStart)+'/'+'" + yearToday +
                            "') <= CONVERT(DATETIME, '" + dateToday + "') AND CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthEnd) +" +
                            "'/'+CONVERT(NVARCHAR,TM.DayEnd)+'/" + yearToday +
                            "') >= CONVERT(DATETIME, '" + monthTody + "/1/" + yearToday + "'))";
                        break;
                    case TimeFrame.NextDays:


                        decimal theDub = 0.00M;
                        if (decimal.TryParse(NextDaysInput.Value, out theDub))
                        {
                            resultsStr += " Next " + NextDaysInput.Value + " Days ";
                            StartDate = itBeNow;
                            EndDate = itBeNow.AddDays(int.Parse(NextDaysInput.Value));

                            dayStart = StartDate.Day.ToString();
                            dayEnd = EndDate.Day.ToString();
                            startMonth = StartDate.Month.ToString();
                            endMonth = EndDate.Month.ToString();
                            startYear = StartDate.Year.ToString();
                            endYear = EndDate.Year.ToString();

                            dateStart = startMonth + "/" + dayStart + "/" + startYear;
                            dateEnd = endMonth + "/" + dayEnd + "/" + endYear;

                            startDayOfWeek = dat.getDayOfWeek(StartDate.DayOfWeek);
                            endDayOfWeek = dat.getDayOfWeek(EndDate.DayOfWeek);

                            date = " AND (CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthStart)+'/'+CONVERT(NVARCHAR,TM.DayStart)" +
                                "+'/'+'" + endYear +
                                "') <= CONVERT(DATETIME, '" + EndDate.Month.ToString() + "/" + EndDate.Day.ToString() + "/" + EndDate.Year.ToString() + " 23:59:59') AND CONVERT(DATETIME,CONVERT" +
                                "(NVARCHAR,TM.MonthEnd) +" +
                                "'/'+CONVERT(NVARCHAR,TM.DayEnd)+'/'+'" + startYear +
                                "') >= CONVERT(DATETIME, '" + StartDate.Month.ToString() + "/" + StartDate.Day.ToString() + "/" + StartDate.Year.ToString() + " 00:00:00'))";

                        }
                        else
                        {
                            StartDate = itBeNow;
                            EndDate = itBeNow;

                            date = " AND (CONVERT(DATETIME,CONVERT(NVARCHAR,TM.MonthStart)+'/'+CONVERT(NVARCHAR,TM.DayStart)" +
                                "+'/'+'" + EndDate.Year.ToString() +
                                "') >= CONVERT(DATETIME, '" + EndDate.Month.ToString() + "/" + EndDate.Day.ToString() + "/" + EndDate.Year.ToString() + " 00:00:00')) ";
                        }
                        break;
                    default: break;
                }

                string searchStr = "SELECT DISTINCT TOP " + totalCount.ToString() + " E.ID AS EventID,  E.DaysFeatured, E.PostedOn, '$'+CONVERT(NVARCHAR,E.MinPrice)+' - $'+" +
                    "CONVERT(NVARCHAR, E.MaxPrice)" +
                    "AS PriceRange, E.Featured, E.MinPrice AS Price, E.Content, E.Means, dbo.GetDuration(E.Duration, 0) AS " +
                    "TimeFrame, dbo.GetDuration(E.Duration, 1) AS Duration, E.Header, " +
                    " E.Featured, E.DaysFeatured, 'T' AS Type, TDS.StartTime, TDS.EndTime FROM TripDirections TDir, Trips E, TripMonths TM, TripDays TDS " +
                    "WHERE TDir.TripID=E.ID AND TM.TripID=TDS.TripID AND TM.TripID=E.ID AND E.Live='True' " +
                    tripCountry + zips + date + highestP + "  ORDER BY E.Featured, E.PostedOn";
                //TimeFrameLabel.Text += searchStr;
                DataSet dsTrips = dat.GetData(searchStr);
                //totalCount = totalCount - dsTrips.Tables[0].Rows.Count;
                #endregion

                if (ds.Tables[0].Rows.Count > 0)
                    ds = MergeEvents(ds, dsTrips);
                else
                    ds = dsTrips;

                if (totalCount > 0)
                {
                    #region Get Locale Happenings
                    date = "";
                    dayOfWeek = dat.getDayOfWeek(itBeNow.DayOfWeek);
                    int dayOfWeekInt = int.Parse(dayOfWeek);
                    switch (theEnum)
                    {
                        case TimeFrame.Today:
                            date = " AND Days LIKE '%" + dayOfWeek + "%'";
                            break;
                        case TimeFrame.Tomorrow:
                            dayOfWeekInt++;
                            if (dayOfWeekInt > 7)
                                dayOfWeekInt = 1;
                            date = " AND Days LIKE '%" + dayOfWeekInt.ToString() + "%'";
                            break;
                        case TimeFrame.ThisWeekend:
                            date = " AND (Days LIKE '%5%' OR Days LIKE '%6%' OR Days LIKE '%7%') ";
                            break;
                        case TimeFrame.ThisWeek:
                            date = "";
                            break;
                        case TimeFrame.ThisMonth:
                            date = "";
                            break;
                        case TimeFrame.Beginning:
                            date = "";
                            break;
                        case TimeFrame.NextDays:
                            decimal theDub = 0.00M;
                            if (decimal.TryParse(NextDaysInput.Value, out theDub))
                            {
                                if (int.Parse(NextDaysInput.Value) >= 7)
                                {
                                    date = "";
                                }
                                else
                                {
                                    date += " AND (Days LIKE '%" + dayOfWeekInt.ToString() + "%' ";
                                    for (int i = 0; i < int.Parse(NextDaysInput.Value); i++)
                                    {
                                        date += " OR Days LIKE '%" + (++dayOfWeekInt).ToString() + "%' ";
                                    }
                                    date += " ) ";
                                }
                            }
                            else
                            {
                                date = " AND Days LIKE '%" + dayOfWeek + "%'";
                            }
                            break;
                        default: break;
                    }

                    DataSet dsVenues = dat.GetData("SELECT TOP " + totalCount.ToString() + "  V.Featured,  V.DaysFeatured, V.PostedOn, VE.EventName + ' at '+V.Name AS Header, V.ID AS EventID, " +
                        "V.Content, 'H' AS Type, HourStart AS StartTime, HourEnd AS EndTime FROM Venues V, VenueEvents VE WHERE VE.VenueID=V.ID " +
                        zips + date + " ORDER BY V.Featured, V.PostedOn");
                    //TimeFrameLabel.Text += "SELECT V.Featured,  V.DaysFeatured, V.PostedOn, VE.EventName + ' at '+V.Name AS Header, V.ID AS EventID, " +
                    //    "V.Content, 'V' AS Type FROM Venues V, VenueEvents VE WHERE VE.VenueID=V.ID " +
                    //    zips + date + " ORDER BY V.Featured, V.PostedOn";
                    //totalCount = totalCount - dsVenues.Tables[0].Rows.Count;
                    #endregion
                    if (ds.Tables[0].Rows.Count > 0)
                        ds = MergeEvents(ds, dsVenues);
                    else
                        ds = dsVenues;
                    
                    if (totalCount > 0)
                    {
                        #region Get Locales
                        date = "";
                        dayOfWeek = dat.getDayOfWeek(itBeNow.DayOfWeek);
                        dayOfWeekInt = int.Parse(dayOfWeek);
                        switch (theEnum)
                        {
                            case TimeFrame.Today:
                                date = " AND Days LIKE '%" + dayOfWeek + "%'";
                                break;
                            case TimeFrame.Tomorrow:
                                dayOfWeekInt++;
                                if (dayOfWeekInt > 7)
                                    dayOfWeekInt = 1;
                                date = " AND Days LIKE '%" + dayOfWeekInt.ToString() + "%'";
                                break;
                            case TimeFrame.ThisWeekend:
                                date = " AND (Days LIKE '%5%' OR Days LIKE '%6%' OR Days LIKE '%7%') ";
                                break;
                            case TimeFrame.ThisWeek:
                                date = "";
                                break;
                            case TimeFrame.ThisMonth:
                                date = "";
                                break;
                            case TimeFrame.Beginning:
                                date = "";
                                break;
                            case TimeFrame.NextDays:
                                decimal theDub = 0.00M;
                                if (decimal.TryParse(NextDaysInput.Value, out theDub))
                                {
                                    resultsStr += " Next " + NextDaysInput.Value + " Days ";

                                    if (int.Parse(NextDaysInput.Value) >= 7)
                                    {
                                        date = "";
                                    }
                                    else
                                    {
                                        date += " AND (Days LIKE '%" + dayOfWeekInt.ToString() + "%' ";
                                        for (int i = 0; i < int.Parse(NextDaysInput.Value); i++)
                                        {
                                            date += " OR Days LIKE '%" + (++dayOfWeekInt).ToString() + "%' ";
                                        }
                                        date += " ) ";
                                    }
                                }
                                else
                                {
                                    date = " AND Days LIKE '%" + dayOfWeek + "%'";
                                }
                                break;
                            default: break;
                        }

                        DataSet dsLocales = dat.GetData("SELECT DISTINCT TOP " + totalCount.ToString() + 
                            "  V.Featured, HourStart AS StartTime, HourEnd AS EndTime, V.DaysFeatured, V.PostedOn, 'Check Out ' + V.Name AS Header, V.ID AS EventID, " +
                            "V.Content, 'V' AS Type FROM Venues V, VenueHours VE WHERE VE.VenueID=V.ID " +
                            zips + date + " ORDER BY V.Featured, V.PostedOn");
                        //totalCount = totalCount - dsLocales.Tables[0].Rows.Count;
                        #endregion
                        if (ds.Tables[0].Rows.Count > 0)
                            ds = MergeEvents(ds, dsLocales);
                        else
                            ds = dsLocales;
                    }
                }
            }
            return ds;
        }
        catch (Exception ex)
        {
            TimeFrameLabel.Text += ex.ToString() + "<br/>" + message;
        }

        return new DataSet();
    }

    protected DataSet MergeEvents(DataSet ds, DataSet ds2)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime itBeNow = isn;
        Data dat = new Data(isn);

        DataRow dr;
        DataView dv = new DataView(ds2.Tables[0], "", "", DataViewRowState.CurrentRows);
        DataView dv1 = new DataView(ds.Tables[0], "", "", DataViewRowState.CurrentRows);

        //Used for sifting same venues who are close
        string prevID = "";
        DataSet dsNEW = new DataSet();
        dsNEW.Tables.Add(new DataTable());
        DataColumn dc = new DataColumn("Header");
        dsNEW.Tables[0].Columns.Add(dc);
        dc = new DataColumn("Content");
        dsNEW.Tables[0].Columns.Add(dc);
        dc = new DataColumn("EventID");
        dsNEW.Tables[0].Columns.Add(dc);
        dc = new DataColumn("Featured");
        dsNEW.Tables[0].Columns.Add(dc);
        dc = new DataColumn("DaysFeatured");
        dsNEW.Tables[0].Columns.Add(dc);
        dc = new DataColumn("Type");
        dsNEW.Tables[0].Columns.Add(dc);
        dc = new DataColumn("StartTime");
        dsNEW.Tables[0].Columns.Add(dc);
        dc = new DataColumn("EndTime");
        dsNEW.Tables[0].Columns.Add(dc);

        //Used for inserting featured items first
        DataSet dsFeatured = new DataSet();
        dsFeatured.Tables.Add(new DataTable());
        dc = new DataColumn("Header");
        dsFeatured.Tables[0].Columns.Add(dc);
        dc = new DataColumn("Content");
        dsFeatured.Tables[0].Columns.Add(dc);
        dc = new DataColumn("EventID");
        dsFeatured.Tables[0].Columns.Add(dc);
        dc = new DataColumn("Featured");
        dsFeatured.Tables[0].Columns.Add(dc);
        dc = new DataColumn("DaysFeatured");
        dsFeatured.Tables[0].Columns.Add(dc);
        dc = new DataColumn("Type");
        dsFeatured.Tables[0].Columns.Add(dc);
        dc = new DataColumn("StartTime");
        dsFeatured.Tables[0].Columns.Add(dc);
        dc = new DataColumn("EndTime");
        dsFeatured.Tables[0].Columns.Add(dc);

        dv.RowFilter = "Featured='True' AND DaysFeatured LIKE '%" + itBeNow.Date.ToShortDateString() + "%' ";
        dv1.RowFilter = "Featured='True' AND DaysFeatured LIKE '%" + itBeNow.Date.ToShortDateString() + "%' ";

        DataView dvToUse = dv;
        DataView dvOther = dv1;
        int theMax = dv.Count;
        if (theMax < dv1.Count)
        {
            theMax = dv1.Count;
            dvToUse = dv1;
            dvOther = dv;
        }

        //Add all featured items to dsFeatured 
        //and also mix them together

        Hashtable hashTab = new Hashtable();

        string prevFeaturedID = "";
        DataRow dr2;
        for (int i = 0; i < theMax; i++)
        {
            dr = dsFeatured.Tables[0].NewRow();
            if (!hashTab.ContainsKey(dvToUse[i]["Type"].ToString() + dvToUse[i]["EventID"].ToString()))
            {
                
                dr["Header"] = dvToUse[i]["Header"];
                dr["Content"] = dvToUse[i]["Content"];
                dr["EventID"] = dvToUse[i]["EventID"];
                dr["Featured"] = dvToUse[i]["Featured"];
                dr["DaysFeatured"] = dvToUse[i]["DaysFeatured"];
                dr["Type"] = dvToUse[i]["Type"];
                dr["StartTime"] = dvToUse[i]["StartTime"];
                dr["EndTime"] = dvToUse[i]["EndTime"];
            }

            if (dvOther.Count >= i + 1)
            {
                if (!hashTab.ContainsKey(dvOther[i]["Type"].ToString() + dvOther[i]["EventID"].ToString()))
                {
                    dr2 = dsFeatured.Tables[0].NewRow();
                    dr2["Header"] = dvOther[i]["Header"];
                    dr2["Content"] = dvOther[i]["Content"];
                    dr2["EventID"] = dvOther[i]["EventID"];
                    dr2["Featured"] = dvOther[i]["Featured"];
                    dr2["DaysFeatured"] = dvOther[i]["DaysFeatured"];
                    dr2["Type"] = dvOther[i]["Type"];
                    dr2["StartTime"] = dvOther[i]["StartTime"];
                    dr2["EndTime"] = dvOther[i]["EndTime"];

                    if (prevFeaturedID == "")
                    {
                        if (!hashTab.ContainsKey(dvToUse[i]["Type"].ToString() + dvToUse[i]["EventID"].ToString()))
                        {
                            dsFeatured.Tables[0].Rows.Add(dr);
                            hashTab.Add(dvToUse[i]["Type"].ToString() + dvToUse[i]["EventID"].ToString(), "1");
                        }
                        dsFeatured.Tables[0].Rows.Add(dr2);
                        hashTab.Add(dvOther[i]["Type"].ToString() + dvOther[i]["EventID"].ToString(), "1");
                        prevFeaturedID = dvOther[i]["EventID"].ToString();
                    }
                    else
                    {
                        if (dvToUse[i]["EventID"].ToString() == prevFeaturedID)
                        {
                            dsFeatured.Tables[0].Rows.Add(dr2);
                            hashTab.Add(dvOther[i]["Type"].ToString() + dvOther[i]["EventID"].ToString(), "1");
                            if (!hashTab.ContainsKey(dvToUse[i]["Type"].ToString() + dvToUse[i]["EventID"].ToString()))
                            {
                                dsFeatured.Tables[0].Rows.Add(dr);
                                hashTab.Add(dvToUse[i]["Type"].ToString() + dvToUse[i]["EventID"].ToString(), "1");
                            }
                        }
                        else
                        {
                            if (!hashTab.ContainsKey(dvToUse[i]["Type"].ToString() + dvToUse[i]["EventID"].ToString()))
                            {
                                dsFeatured.Tables[0].Rows.Add(dr);
                                hashTab.Add(dvToUse[i]["Type"].ToString() + dvToUse[i]["EventID"].ToString(), "1");
                            }
                            dsFeatured.Tables[0].Rows.Add(dr2);
                            hashTab.Add(dvOther[i]["Type"].ToString() + dvOther[i]["EventID"].ToString(), "1");
                        }
                        prevFeaturedID = dvOther[i]["EventID"].ToString();
                    }
                }
                else
                {
                    if (!hashTab.ContainsKey(dvToUse[i]["Type"].ToString() + dvToUse[i]["EventID"].ToString()))
                    {
                        dsFeatured.Tables[0].Rows.Add(dr);
                        hashTab.Add(dvToUse[i]["Type"].ToString() + dvToUse[i]["EventID"].ToString(), "1");
                    }
                }
            }
            else
            {
                if (!hashTab.ContainsKey(dvToUse[i]["Type"].ToString() + dvToUse[i]["EventID"].ToString()))
                {
                    dsFeatured.Tables[0].Rows.Add(dr);
                    hashTab.Add(dvToUse[i]["Type"].ToString() + dvToUse[i]["EventID"].ToString(), "1");
                }
            }
        }


        //Do the opposite, mix and combine the non-featured items
        DataSet dsNonFeatured = new DataSet();
        dsNonFeatured.Tables.Add(new DataTable());
        dc = new DataColumn("Header");
        dsNonFeatured.Tables[0].Columns.Add(dc);
        dc = new DataColumn("Content");
        dsNonFeatured.Tables[0].Columns.Add(dc);
        dc = new DataColumn("EventID");
        dsNonFeatured.Tables[0].Columns.Add(dc);
        dc = new DataColumn("Featured");
        dsNonFeatured.Tables[0].Columns.Add(dc);
        dc = new DataColumn("DaysFeatured");
        dsNonFeatured.Tables[0].Columns.Add(dc);
        dc = new DataColumn("Type");
        dsNonFeatured.Tables[0].Columns.Add(dc);
        dc = new DataColumn("StartTime");
        dsNonFeatured.Tables[0].Columns.Add(dc);
        dc = new DataColumn("EndTime");
        dsNonFeatured.Tables[0].Columns.Add(dc);

        dv.RowFilter = "Featured='False' OR (Featured='True' AND DaysFeatured NOT LIKE '%" + 
            itBeNow.Date.ToShortDateString() + "%') ";
        dv1.RowFilter = "Featured='False' OR (Featured='True' AND DaysFeatured NOT LIKE '%" +
            itBeNow.Date.ToShortDateString() + "%') ";

        dvToUse = dv;
        dvOther = dv1;
        theMax = dv.Count;
        if (theMax < dv1.Count)
        {
            theMax = dv1.Count;
            dvToUse = dv1;
            dvOther = dv;
        }

        //Add all non-featured items to dsNonFeatured 
        //and also mix them together
        for (int i = 0; i < theMax; i++)
        {
            if (!hashTab.ContainsKey(dvToUse[i]["Type"].ToString() + dvToUse[i]["EventID"].ToString()))
            {
                dr = dsNonFeatured.Tables[0].NewRow();
                dr["Header"] = dvToUse[i]["Header"];
                dr["Content"] = dvToUse[i]["Content"];
                dr["EventID"] = dvToUse[i]["EventID"];
                dr["Featured"] = dvToUse[i]["Featured"];
                dr["DaysFeatured"] = dvToUse[i]["DaysFeatured"];
                dr["Type"] = dvToUse[i]["Type"];
                dr["StartTime"] = dvToUse[i]["StartTime"];
                dr["EndTime"] = dvToUse[i]["EndTime"];
                dsNonFeatured.Tables[0].Rows.Add(dr);
                hashTab.Add(dvToUse[i]["Type"].ToString() + dvToUse[i]["EventID"].ToString(), "1");
            }
            if (dvOther.Count >= i + 1)
            {
                if (!hashTab.ContainsKey(dvOther[i]["Type"].ToString() + dvOther[i]["EventID"].ToString()))
                {
                    dr = dsNonFeatured.Tables[0].NewRow();
                    dr["Header"] = dvOther[i]["Header"];
                    dr["Content"] = dvOther[i]["Content"];
                    dr["EventID"] = dvOther[i]["EventID"];
                    dr["Featured"] = dvOther[i]["Featured"];
                    dr["DaysFeatured"] = dvOther[i]["DaysFeatured"];
                    dr["Type"] = dvOther[i]["Type"];
                    dr["StartTime"] = dvOther[i]["StartTime"];
                    dr["EndTime"] = dvOther[i]["EndTime"];
                    dsNonFeatured.Tables[0].Rows.Add(dr);
                    hashTab.Add(dvOther[i]["Type"].ToString() + dvOther[i]["EventID"].ToString(), "1");
                }
            }
        }

        //Combine featured with non featured making sure the 
        // non featured items are not next to each other
        // if they're from the same venue
        dv = new DataView(dsNonFeatured.Tables[0], "", "", DataViewRowState.CurrentRows);

        foreach(DataRowView row in dv)
        {
            //Prevent content from same venue being next to 
            //each other unless it is featured
            if (prevID != "" && prevID == row["EventID"].ToString() && !bool.Parse(row["Featured"].ToString()))
            {
                //if (!hashTab.ContainsKey(row["Type"].ToString() + row["EventID"].ToString()))
                //{
                    prevID = row["EventID"].ToString();
                    dr = dsNEW.Tables[0].NewRow();
                    dr["Header"] = row["Header"];
                    dr["Content"] = row["Content"];
                    dr["EventID"] = row["EventID"];
                    dr["Featured"] = row["Featured"];
                    dr["DaysFeatured"] = row["DaysFeatured"];
                    dr["Type"] = row["Type"];
                    dr["StartTime"] = row["StartTime"];
                    dr["EndTime"] = row["EndTime"];
                    dsNEW.Tables[0].Rows.Add(dr);
                //    hashTab.Add(row["Type"].ToString() + row["EventID"].ToString(), "1");
                //}
            }
            else
            {
                //if (!hashTab.ContainsKey(row["Type"].ToString() + row["EventID"].ToString()))
                //{
                    prevID = row["EventID"].ToString();
                    dr = dsFeatured.Tables[0].NewRow();
                    dr["Header"] = row["Header"];
                    dr["Content"] = row["Content"];
                    dr["EventID"] = row["EventID"];
                    dr["Featured"] = row["Featured"];
                    dr["DaysFeatured"] = row["DaysFeatured"];
                    dr["Type"] = row["Type"];
                    dr["StartTime"] = row["StartTime"];
                    dr["EndTime"] = row["EndTime"];
                    dsFeatured.Tables[0].Rows.Add(dr);
                //    hashTab.Add(row["Type"].ToString() + row["EventID"].ToString(), "1");
                //}
            }
        }

        if (dsNEW.Tables[0].Rows.Count > 0)
        {
            if (dsNEW.Tables[0].Rows.Count == 1)
            {
                //if (!hashTab.ContainsKey(dsNEW.Tables[0].Rows[0]["Type"].ToString() + dsNEW.Tables[0].Rows[0]["EventID"].ToString()))
                //{
                    dr = dsFeatured.Tables[0].NewRow();
                    dr["Header"] = dsNEW.Tables[0].Rows[0]["Header"];
                    dr["Content"] = dsNEW.Tables[0].Rows[0]["Content"];
                    dr["EventID"] = dsNEW.Tables[0].Rows[0]["EventID"];
                    dr["Featured"] = dsNEW.Tables[0].Rows[0]["Featured"];
                    dr["DaysFeatured"] = dsNEW.Tables[0].Rows[0]["DaysFeatured"];
                    dr["Type"] = dsNEW.Tables[0].Rows[0]["Type"];
                    dr["StartTime"] = dsNEW.Tables[0].Rows[0]["StartTime"];
                    dr["EndTime"] = dsNEW.Tables[0].Rows[0]["EndTime"];
                    dsFeatured.Tables[0].Rows.Add(dr);
                //    hashTab.Add(dsNEW.Tables[0].Rows[0]["Type"].ToString() + dsNEW.Tables[0].Rows[0]["EventID"].ToString(), "1");
                //}
            }
            else
            {
                dsFeatured = MergeEvents(dsFeatured, dsNEW);
            }
        }

        return dsFeatured;
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

        DateTime isn = DateTime.Now;

        if (!DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn))
            isn = DateTime.Now;
        DateTime isNow = isn;
        Data dat = new Data(isn);

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
            "_Friend\"><img alt=\"HippoHappenings Member " + dvUser[0]["UserName"].ToString() + "\" width=\"" +
            width.ToString() + "\" " + hoverStr + " height=\"" + height + "\" class=\"imgBorder\" src=\"" + 
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
    //    Response.Redirect("event-search");
    //}

    protected void Page_Init(object sender, EventArgs e)
    {

    }

    //protected void Page_Unload(object sender, EventArgs e)
    //{
    //    Master.BodyTag.Attributes.Remove("onload");
    //}

    protected string GetImageStr(string eventID, string pictureName, string link, string header)
    {
        System.Drawing.Image image = System.Drawing.Image.FromFile(MapPath(".") +
            "\\UserFiles\\Events\\" + eventID + "\\Slider\\" + pictureName);

        int width = 194;
        int height = 201;

        string padding = "padding-top: 3px;padding-bottom: 3px;";

        if (image.Height > image.Width)
        {
            width = 105;
            height = 120;
            padding = "padding-right: 3px;padding-top: 3px;";
        }

        int newHeight = 0;
        int newIntWidth = 0;

        //if image height is less than resize height
        if (height >= image.Height)
        {
            //leave the height as is
            newHeight = image.Height;

            if (width >= image.Width)
            {
                newIntWidth = image.Width;
            }
            else
            {
                newIntWidth = width;

                double theDivider = double.Parse(image.Width.ToString()) / double.Parse(newIntWidth.ToString());
                double newDoubleHeight = double.Parse(newHeight.ToString());
                newDoubleHeight = double.Parse(height.ToString()) / theDivider;
                newHeight = (int)newDoubleHeight;
            }
        }
        //if image height is greater than resize height...resize it
        else
        {
            //make height equal to the requested height.
            newHeight = height;

            //get the ratio of the new height/original height and apply that to the width
            double theDivider = double.Parse(image.Height.ToString()) / double.Parse(newHeight.ToString());
            double newDoubleWidth = double.Parse(newIntWidth.ToString());
            newDoubleWidth = double.Parse(image.Width.ToString()) / theDivider;
            newIntWidth = (int)newDoubleWidth;

            //if the resized width is still to big
            if (newIntWidth > width)
            {
                //make it equal to the requested width
                newIntWidth = width;

                //get the ratio of old/new width and apply it to the already resized height
                theDivider = double.Parse(image.Width.ToString()) / double.Parse(newIntWidth.ToString());
                double newDoubleHeight = double.Parse(newHeight.ToString());
                newDoubleHeight = double.Parse(image.Height.ToString()) / theDivider;
                newHeight = (int)newDoubleHeight;
            }
        }

        string strReturn = "";

        string strWidth = "width: " + width.ToString() + "px;margin-bottom: 3px; border: solid 1px #dedbdb;" + padding + "float: left; height: " + newHeight.ToString() + "px;";

        if (image.Height > image.Width)
        {
            strWidth = "border: 1px solid #DEDBDB;float: left;height: 120px; " +
                "margin-bottom: 3px; " +
                "margin-left: -5px; " +
                "margin-right: 5px; " +
                "padding: 3px; border: solid 1px #dedbdb;" + padding + " height: " + newHeight.ToString() + "px;";
        }

        strReturn = "<div align=\"center\" style=\"" + strWidth + "\"><a href=\"" + link + "\"><img alt=\"" + header.Replace("-", " ") + "\"  class=\"imgBorder\" height=\"" + newHeight + "px\" width=\"" + newIntWidth + "px\" src=\""
            + "UserFiles/Events/" + eventID + "/Slider/" + pictureName + "\" /></a></div>";

        return strReturn;
    }

    //protected string GetImageStr(string eventID, string pictureName, string link, string header)
    //{
    //    System.Drawing.Image image = System.Drawing.Image.FromFile(MapPath(".") +
    //        "\\UserFiles\\Events\\" + eventID + "\\Slider\\" + pictureName);

    //    int width = 194;
    //    int height = 201;

    //    int newHeight = 0;
    //    int newIntWidth = 0;

    //    bool isWidthSmaller = image.Width < width;
    //    bool isHeightSmaller = image.Height < height;

    //    if (isWidthSmaller || isHeightSmaller)
    //    {
    //        if (isWidthSmaller)
    //        {
    //            if (!isHeightSmaller)
    //            {
    //                newIntWidth = width;


    //            }
    //        }

    //        //if image height is less than resize height
    //        if (image.Width > image.Height)
    //        {
    //            //set the width
    //            newIntWidth = width;


    //            double theDivider = double.Parse(newIntWidth.ToString()) / double.Parse(image.Width.ToString());
    //            double newDoubleHeight = double.Parse(image.Height.ToString());
    //            newDoubleHeight = double.Parse(height.ToString()) / theDivider;
    //            newHeight = (int)newDoubleHeight;

    //        }
    //        //if image height is greater than resize height...resize it
    //        else
    //        {
    //            //make height equal to the requested height.
    //            newHeight = height;

    //            //get the ratio of the new height/original height and apply that to the width
    //            double theDivider = double.Parse(image.Height.ToString()) / double.Parse(newHeight.ToString());
    //            double newDoubleWidth = double.Parse(newIntWidth.ToString());
    //            newDoubleWidth = double.Parse(image.Width.ToString()) / theDivider;
    //            newIntWidth = (int)newDoubleWidth;

    //            //if the resized width is still to big
    //            if (newIntWidth > width)
    //            {
    //                //make it equal to the requested width
    //                newIntWidth = width;

    //                //get the ratio of old/new width and apply it to the already resized height
    //                theDivider = double.Parse(image.Width.ToString()) / double.Parse(newIntWidth.ToString());
    //                double newDoubleHeight = double.Parse(newHeight.ToString());
    //                newDoubleHeight = double.Parse(image.Height.ToString()) / theDivider;
    //                newHeight = (int)newDoubleHeight;
    //            }
    //        }
    //    }

 

    //    string strReturn = "";

    //    string strWidth = "width: " + width.ToString() + "px;margin-bottom: 3px; border: solid 1px #dedbdb;" + padding + "float: left; height: " + newHeight.ToString() + "px;";

    //    if (image.Height > image.Width)
    //    {
    //        strWidth = "border: 1px solid #DEDBDB;float: left;height: 120px; " +
    //            "margin-bottom: 3px; " +
    //            "margin-left: -5px; " +
    //            "margin-right: 5px; " +
    //            "padding: 3px; border: solid 1px #dedbdb;" + padding + " height: " + newHeight.ToString() + "px;";
    //    }

    //    strReturn = "<div align=\"center\" style=\"" + strWidth + "\"><a href=\"" + link + "\"><img alt=\"" + header.Replace("-", " ") + "\"  class=\"imgBorder\" height=\"" + newHeight + "px\" width=\"" + newIntWidth + "px\" src=\""
    //        + "UserFiles/Events/" + eventID + "/Slider/" + pictureName + "\" /></a></div>";

    //    return strReturn;
    //}

    protected string GetImageStrTrip(string eventID, string pictureName, string link, string header)
    {
        System.Drawing.Image image = System.Drawing.Image.FromFile(MapPath(".") +
            "\\Trips\\" + eventID + "\\Slider\\" + pictureName);

        int width = 175;
        int height = 70;

        string padding = "padding-top: 5px;padding-bottom: 5px;";

        if (image.Height > image.Width)
        {
            width = 105;
            height = 120;
            padding = "padding-right: 5px;padding-top: 5px;";
        }

        int newHeight = 0;
        int newIntWidth = 0;

        //if image height is less than resize height
        if (height >= image.Height)
        {
            //leave the height as is
            newHeight = image.Height;

            if (width >= image.Width)
            {
                newIntWidth = image.Width;
            }
            else
            {
                newIntWidth = width;

                double theDivider = double.Parse(image.Width.ToString()) / double.Parse(newIntWidth.ToString());
                double newDoubleHeight = double.Parse(newHeight.ToString());
                newDoubleHeight = double.Parse(height.ToString()) / theDivider;
                newHeight = (int)newDoubleHeight;
            }
        }
        //if image height is greater than resize height...resize it
        else
        {
            //make height equal to the requested height.
            newHeight = height;

            //get the ratio of the new height/original height and apply that to the width
            double theDivider = double.Parse(image.Height.ToString()) / double.Parse(newHeight.ToString());
            double newDoubleWidth = double.Parse(newIntWidth.ToString());
            newDoubleWidth = double.Parse(image.Width.ToString()) / theDivider;
            newIntWidth = (int)newDoubleWidth;

            //if the resized width is still to big
            if (newIntWidth > width)
            {
                //make it equal to the requested width
                newIntWidth = width;

                //get the ratio of old/new width and apply it to the already resized height
                theDivider = double.Parse(image.Width.ToString()) / double.Parse(newIntWidth.ToString());
                double newDoubleHeight = double.Parse(newHeight.ToString());
                newDoubleHeight = double.Parse(image.Height.ToString()) / theDivider;
                newHeight = (int)newDoubleHeight;
            }
        }

        string strReturn = "";

        string strWidth = "width: " + width.ToString() + "px;margin-bottom: 3px; border: solid 1px #dedbdb;" + padding + "float: left; height: " + newHeight.ToString() + "px;";

        if (image.Height > image.Width)
        {
            strWidth = "border: 1px solid #DEDBDB;float: left;height: 120px; " +
                "margin-bottom: 3px; " +
                "margin-left: -5px; " +
                "margin-right: 5px; " +
                "padding: 3px;border: solid 1px #dedbdb;" + padding + " height: " + newHeight.ToString() + "px;";
        }

        strReturn = "<div align=\"center\" style=\"" + strWidth + "\"><a href=\"" + link + "\"><img alt=\"" + header.Replace("-", " ") + "\"  class=\"imgBorder\" height=\"" + newHeight + "px\" width=\"" + newIntWidth + "px\" src=\""
            + "Trips/" + eventID + "/Slider/" + pictureName + "\" /></a></div>";

        return strReturn;
    }

    protected string GetImageStrVenue(string eventID, string pictureName, string link, string header)
    {
        System.Drawing.Image image = System.Drawing.Image.FromFile(MapPath(".") +
            "\\VenueFiles\\" + eventID + "\\Slider\\" + pictureName);

        int width = 175;
        int height = 70;

        string padding = "padding-top: 5px;padding-bottom: 5px;";

        if (image.Height > image.Width)
        {
            width = 105;
            height = 120;
            padding = "padding-right: 5px;padding-top: 5px;";
        }

        int newHeight = 0;
        int newIntWidth = 0;

        //if image height is less than resize height
        if (height >= image.Height)
        {
            //leave the height as is
            newHeight = image.Height;

            if (width >= image.Width)
            {
                newIntWidth = image.Width;
            }
            else
            {
                newIntWidth = width;

                double theDivider = double.Parse(image.Width.ToString()) / double.Parse(newIntWidth.ToString());
                double newDoubleHeight = double.Parse(newHeight.ToString());
                newDoubleHeight = double.Parse(height.ToString()) / theDivider;
                newHeight = (int)newDoubleHeight;
            }
        }
        //if image height is greater than resize height...resize it
        else
        {
            //make height equal to the requested height.
            newHeight = height;

            //get the ratio of the new height/original height and apply that to the width
            double theDivider = double.Parse(image.Height.ToString()) / double.Parse(newHeight.ToString());
            double newDoubleWidth = double.Parse(newIntWidth.ToString());
            newDoubleWidth = double.Parse(image.Width.ToString()) / theDivider;
            newIntWidth = (int)newDoubleWidth;

            //if the resized width is still to big
            if (newIntWidth > width)
            {
                //make it equal to the requested width
                newIntWidth = width;

                //get the ratio of old/new width and apply it to the already resized height
                theDivider = double.Parse(image.Width.ToString()) / double.Parse(newIntWidth.ToString());
                double newDoubleHeight = double.Parse(newHeight.ToString());
                newDoubleHeight = double.Parse(image.Height.ToString()) / theDivider;
                newHeight = (int)newDoubleHeight;
            }
        }

        string strReturn = "";

        string strWidth = "width: " + width.ToString() + "px;margin-bottom: 3px; border: solid 1px #dedbdb;" + padding + "float: left; height: " + newHeight.ToString() + "px;";

        if (image.Height > image.Width)
        {
            strWidth = "border: 1px solid #DEDBDB;float: left;height: 120px; " +
                "margin-bottom: 3px; " +
                "margin-left: -5px; " +
                "margin-right: 5px; " +
                "padding: 3px;border: solid 1px #dedbdb;" + padding + " height: " + newHeight.ToString() + "px;";
        }

        strReturn = "<div align=\"center\" style=\"" + strWidth + "\"><a href=\"" + link + "\"><img  alt=\"" + header.Replace("-", " ") + "\"  class=\"imgBorder\" height=\"" + newHeight + "px\" width=\"" + newIntWidth + "px\" src=\""
            + "VenueFiles/" + eventID + "/Slider/" + pictureName + "\" /></a></div>";

        return strReturn;
    }

    protected string GetMayorImage(string UserName, string pictureName)
    {
        System.Drawing.Image image = System.Drawing.Image.FromFile(MapPath(".") +
            "\\UserFiles\\" + UserName + "\\" + pictureName);

        int width = 200;
        int height = 130;


        int newHeight = 0;
        int newIntWidth = 0;

        //if image height is less than resize height
        if (height >= image.Height)
        {
            //leave the height as is
            newHeight = image.Height;

            if (width >= image.Width)
            {
                newIntWidth = image.Width;
            }
            else
            {
                newIntWidth = width;

                double theDivider = double.Parse(image.Width.ToString()) / double.Parse(newIntWidth.ToString());
                double newDoubleHeight = double.Parse(newHeight.ToString());
                newDoubleHeight = double.Parse(height.ToString()) / theDivider;
                newHeight = (int)newDoubleHeight;
            }
        }
        //if image height is greater than resize height...resize it
        else
        {
            //make height equal to the requested height.
            newHeight = height;

            //get the ratio of the new height/original height and apply that to the width
            double theDivider = double.Parse(image.Height.ToString()) / double.Parse(newHeight.ToString());
            double newDoubleWidth = double.Parse(newIntWidth.ToString());
            newDoubleWidth = double.Parse(image.Width.ToString()) / theDivider;
            newIntWidth = (int)newDoubleWidth;

            //if the resized width is still to big
            if (newIntWidth > width)
            {
                //make it equal to the requested width
                newIntWidth = width;

                //get the ratio of old/new width and apply it to the already resized height
                theDivider = double.Parse(image.Width.ToString()) / double.Parse(newIntWidth.ToString());
                double newDoubleHeight = double.Parse(newHeight.ToString());
                newDoubleHeight = double.Parse(image.Height.ToString()) / theDivider;
                newHeight = (int)newDoubleHeight;
            }
        }

        string strReturn = "";

        string strWidth = "width=\"" + width.ToString() + "px;\"";

        if (image.Height > image.Width)
        {
            strWidth = "width=\"" + newIntWidth.ToString() + "px;\"";
        }

        strReturn = "<div><table align=\"center\" cellpadding=\"0\" cellspacing=\"0\" width=\"200px\" " +
            "height=\"130px\"><tr><td valign=\"center\"><div class=\"Home51\" align=\"center\"><img alt=\"Hippo Boss " + UserName + "\"  class=\"imgBorder\" height=\"" + newHeight +
            "px\" width=\"" + newIntWidth + "px\" src=\""
            + "UserFiles/" + UserName + "/" + pictureName + "\" /></div></td></tr></table></div>";

        return strReturn;
    }

    protected void GoToSearch(object sender, EventArgs e)
    {
        Data dat = new Data(DateTime.Now);
        string str = dat.MakeNiceNameFull(dat.stripHTML(KeywordsTextBox.Text)).Replace("_", "+");
        Response.Redirect("event-search?title=" + str);
    }

    protected void Suggest(object sender, EventArgs e)
    {
        if (VenueSuggestTextBox.Text.Trim() != "")
        {
            Data dat = new Data(DateTime.Now);
            VenueSuggestTextBox.Text = dat.stripHTML(VenueSuggestTextBox.Text.Trim());

            dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString()
                , "Gotten a venue suggestions from: IP: " + dat.GetIP() + ", Message: " + VenueSuggestTextBox.Text + ", Email: " + EmailTextBox.Text,
                "Venue Suggestion Submitted");

            SuggestErrorLabel.Text = "Your suggestion has been made! Thanks!";
        }
    }
}