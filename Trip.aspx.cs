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

public partial class Trip : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string eventID = Request.QueryString["ID"].ToString();
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        try
        {
            Session["RedirectTo"] = Request.Url.AbsoluteUri;

            bool fillUserData = false;

            try
            {
                if (Session["User"] != null)
                {
                    LoggedInPanel.Visible = true;
                    LoggedOutPanel.Visible = false;

                    fillUserData = true;

                   
                    
                }
                else
                {
                    LoggedOutPanel.Visible = true;
                    LoggedInPanel.Visible = false;
                }

                GetOtherEvents();
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.ToString();
            }


            if (Request.QueryString["ID"] == null)
                Response.Redirect("~/home");

            string ID = eventID;
            Session["TripID"] = ID;
            DataView dv = dat.GetDataDV("SELECT * FROM Trips WHERE ID=" + ID);

            Session["FlagID"] = ID;
            Session["FlagType"] = "T";

            #region SEO
            //Create keyword and description meta tags and title
            topTopLiteral.Text = "<a class=\"NavyLink12UD\" href=\"#" + dat.MakeNiceNameFull(dv[0]["Header"].ToString()) + "\">" +
            dat.MakeNiceNameFull(dv[0]["Header"].ToString()).Replace("-", " ") + " From The Top</a>";

            HtmlMeta hm = new HtmlMeta();
            HtmlMeta kw = new HtmlMeta();
            HtmlMeta lg = new HtmlMeta();
            HtmlLink cn = new HtmlLink();
            HtmlHead head = (HtmlHead)Page.Header;

            cn.Attributes.Add("rel", "canonical");
            cn.Href = "http://" + Request.Url.Authority + "/" +
                dat.MakeNiceName(dv[0]["Header"].ToString()) +
                "_" + eventID + "_Trip";
            head.Controls.AddAt(0, cn);

            kw.Name = "keywords";
            hm.Name = "Description";
            lg.Name = "language";
            lg.Content = "English";
            head.Controls.AddAt(0, lg);

            char[] delimeter = { ' ' };
            string[] keywords = dat.MakeNiceNameFull(dv[0]["Header"].ToString()).Replace("-", " ").Split(delimeter, StringSplitOptions.RemoveEmptyEntries);
            int count2 = 0;
            foreach (string token in keywords)
            {
                if (count2 < 16)
                {
                    if (kw.Content != "")
                        kw.Content += " ";
                    kw.Content += token;

                    count2++;
                }
            }
            head.Controls.AddAt(0, kw);

            hm.Content = dat.MakeNiceNameFull(dat.stripHTML(dv[0]["Content"].ToString()).Replace("   ", " ").Replace("  ", " ")).Replace("-", " ");
            if (hm.Content.Length > 200)
                hm.Content = hm.Content.Substring(0, 197) + "...";

            head.Controls.AddAt(0, hm);

            this.Title = kw.Content;

            HtmlLink lk = new HtmlLink();
            lk.Href = "http://" + Request.Url.Authority + "/" +
                dat.MakeNiceName(dat.BreakUpString(dv[0]["Header"].ToString(), 14)) +
                "_" + ID.ToString() + "_Trip";
            lk.Attributes.Add("rel", "bookmark");
            head.Controls.AddAt(0, lk);
            #endregion

            fbLiteral.Text = "<fb:like href=\"" + Request.Url.AbsoluteUri + "\" send=\"true\" layout=\"button_count\" width=\"100\" show_faces=\"true\" font=\"\"></fb:like>";

            Literal lit = new Literal();
            lit.Text = "<script src=\"http://maps.google.com/maps?file=api&amp;v=2&amp;sensor=false&amp;key=ABQIAAAAjjoxQtYNtdn3Tc17U5-jbBR2Kk_H7gXZZZniNQ8L14X1BLzkNhQjgZq1k-Pxm8FxVhUy3rfc6L9O4g\" type=\"text/javascript\"></script>";
            head.Controls.Add(lit);


            if (bool.Parse(dv[0]["Live"].ToString()))
            {
                if (Session["User"] != null)
                {
                    if (dv[0]["UserName"].ToString() == Session["UserName"].ToString())
                        EditLink.Visible = true;
                    else
                        EditLink.Visible = false;
                }
                else
                {
                    EditLink.Visible = false;
                }

                ASP.controls_contactad_ascx contact = new ASP.controls_contactad_ascx();
                contact.THE_TEXT = "Contact Adventure Poster";
                contact.RE_LABEL = "Re: " + dat.BreakUpString(dv[0]["Header"].ToString(), 14);
                contact.TYPE = "ConnectTrip";
                contact.ID = int.Parse(eventID);

                ContactPanel.Controls.Add(contact);

                PricePanel.Visible = false;
                if (dv[0]["MaxPrice"] != null)
                {
                    if (dv[0]["MaxPrice"].ToString() != "")
                    {
                        MinPrice.Text = dv[0]["MinPrice"].ToString().Replace(".00", "");
                        MaxPrice.Text = dv[0]["MaxPrice"].ToString().Replace(".00", "");
                        PricePanel.Visible = true;
                    }
                }

                DurationLabel.Text = "<b>Duration:</b> " + dat.GetDuration(dv[0]["Duration"].ToString());

                char[] del = { ';' };

                DataView daysToGo = dat.GetDataDV("SELECT * FROM TripDays WHERE TripID=" + eventID);

                foreach (DataRowView row in daysToGo)
                {
                    DaysLabel.Text += dat.GetHours(row["Days"].ToString()) + " " +
                        row["StartTime"].ToString() + " - " + row["EndTime"].ToString() + "<br/>";
                }


                DataView monthsToGo = dat.GetDataDV("SELECT * FROM TripMonths WHERE TripID=" + eventID);

                foreach (DataRowView row in monthsToGo)
                {
                    MonthsLabel.Text += dat.GetMonths(row["MonthStart"].ToString()) + ", " +
                        row["DayStart"].ToString() + " - " + dat.GetMonths(row["MonthEnd"].ToString()) +
                        ", " + row["DayEnd"].ToString() + "<br/>";
                }

                ObtainLabel.Text = dv[0]["WhatObtain"].ToString();

                DressLabel.Text = dv[0]["HowDress"].ToString();

                foreach (ListItem item in MeansCheckList.Items)
                {
                    if (dv[0]["Means"].ToString().Contains(item.Value))
                    {
                        item.Selected = true;
                    }
                }

                foreach (ListItem item in MeansCheckList2.Items)
                {
                    if (dv[0]["Means"].ToString().Contains(item.Value))
                    {
                        item.Selected = true;
                    }
                }

                foreach (ListItem item in MeansCheckList3.Items)
                {
                    if (dv[0]["Means"].ToString().Contains(item.Value))
                    {
                        item.Selected = true;
                    }
                }

                DataView dvBring = dat.GetDataDV("SELECT * FROM Trips_WhatToBring WHERE TripID=" + eventID);

                int count = 1;
                foreach (DataRowView row in dvBring)
                {
                    BringLabel.Text += count.ToString() + ". " + row["WhatToBring"].ToString() + "<br/>";
                    count++;
                }

                dvBring = dat.GetDataDV("SELECT * FROM TripDirections WHERE TripID=" + eventID);

                count = 1;
                string dirFunc = "function PlotMapDirections(){var address; var address2; \r\n";
                string prevAddrs = "";
                string thisAddress = "";
                int mapCount = 0;
                string walking = "";
                if (dvBring.Count == 1)
                {
                    Directions.Text += count.ToString() + ". " + dvBring[0]["Directions"].ToString();
                    MapLiteral.Text = "<div id=\"map_canvas\" class=\"TripMapCanvas\"></div>";
                    thisAddress = dat.GetAddress(dvBring[0]["Address"].ToString(),
                        dvBring[0]["Country"].ToString() != "223") + " " + dvBring[0]["City"].ToString() + " " + dvBring[0]["State"].ToString() +
                        " " + dvBring[0]["Zip"].ToString() + " " + dvBring[0]["Country"].ToString();

                    dirFunc += "map = new GMap2(document.getElementById(\"map_canvas\")); map.setUIToDefault(); " +
                                "address = '" +
                                thisAddress.Replace("'", "''") + "'; createMarker(address, '" + dv[0]["Header"].ToString().Replace("'", "''") +
                                "','<div class=\"MapHeader\"><h2>" + dv[0]["Header"].ToString().Replace("'", "''") + ":</h2> <br/>" + dvBring[0]["Directions"].ToString().Replace("'", "''") + "</div>');}";
                }
                else
                {
                    string firstAddress = "";
                    foreach (DataRowView row in dvBring)
                    {
                        Directions.Text += count.ToString() + ". " + row["Directions"].ToString() + "<br/>";
                        count++;
                        thisAddress = dat.GetAddress(row["Address"].ToString(),
                                row["Country"].ToString() != "223") + " " + row["City"].ToString() + " " + row["State"].ToString() +
                                " " + row["Zip"].ToString() + " " + row["Country"].ToString();
                        if (prevAddrs != "")
                        {
                            if ((bool)row["Walking"])
                                walking = "{travelMode:G_TRAVEL_MODE_WALKING}";
                            else
                                walking = "{travelMode:G_TRAVEL_MODE_DRIVING}";
                            dirFunc += "map = new GMap2(document.getElementById(\"map_canvas" +
                                mapCount.ToString() + "\")); \r\n map.setUIToDefault();\r\n directionsPanel = document.getElementById(\"my_textual_div" +
                                mapCount.ToString() + "\"); \r\n directions = " +
                                "new GDirections(map, directionsPanel);directions.load(\"from: " + prevAddrs.Replace("'", "''") +
                                " to: " + thisAddress.Replace("'", "''") + "\", " + walking + ");\r\n ";
                            GoogleDirectionsLiteral.Text += "<div id=\"my_textual_div" + mapCount.ToString() + "\"></div>";
                            MapLiteral.Text += "<div class=\"MapLiteral2\" id=\"map_canvas" + mapCount.ToString() + "\"></div>";
                            mapCount++;
                        }
                        else
                        {
                            firstAddress = thisAddress;
                        }
                        prevAddrs = thisAddress;
                    }
                    MapLiteral.Text += "<h1 class=\"SideColumn\">Map: <a onclick=\"createMarker('" + firstAddress.Replace("'", "''") + "', 'Beginning', '<h2>Beginning:</h2> <br/>" +
                        dvBring[0]["Directions"].ToString().Replace("'", "''") + "');\" class=\"NavyLinkUD\">Beginning</a> " +
                        "and <a class=\"NavyLinkUD\" onclick=\"createMarker('" + prevAddrs.Replace("'", "''") + "', 'Destination', '<h2>Destination:</h2> <br/>" +
                        dvBring[dvBring.Count - 1]["Directions"].ToString() + "');\">Destination</a></h1><div class=\"topDiv MapLiteral3\" id=\"map_canvas\" " +
                        "></div>";
                    dirFunc += "\r\n map = new GMap(document.getElementById(\"map_canvas\"));" +
                        "map.addControl(new GSmallMapControl());map.setCenter(new GLatLng(0,0), 0);\r\n " +
                        "createMarker('" + firstAddress.Replace("'", "''") + "', \"Beginning\", '<div class=\"MapHeader\"><h2>Beginning:</h2> <br/>" +
                        dvBring[0]["Directions"].ToString().Replace("'", "''") + "</div>');\r\n";
                    dirFunc += "}\r\n";
                }

                DirectionsLiteral.Text = "<script type=\"text/javascript\">" + dirFunc + "</script>";

                Master.BodyTag.Attributes.Add("onload", "initialize();");
                Master.BodyTag.Attributes.Add("onunload", "GUnload()");

                EventName.Text = "<a id=\"" + dat.MakeNiceNameFull(dv[0]["Header"].ToString()) + "\" class=\"aboutLink\" href=\"http://" + Request.Url.Authority + 
                    "/" +
                    dat.MakeNiceName(dv[0]["Header"].ToString()) +
                    "_" + ID.ToString() + "_Trip\"><h1>" +
                    dat.BreakUpString(dv[0]["Header"].ToString(), 50) + "</h1></a>";
                Session["Subject"] = "Re: " + dv[0]["Header"].ToString();
                Session["CommentSubject"] = "Re: " + dv[0]["Header"].ToString();
                string UserName = dv[0]["UserName"].ToString();

                TagCloud.THE_ID = int.Parse(ID);

                string content = dv[0]["Content"].ToString();
                string niceName = dat.MakeNiceName(dv[0]["Header"].ToString());


                //ScriptLiteral.Text = "<script type=\"text/javascript\">ReturnURL('" + niceName.Replace("_", " ") + " http://HippoHappenings.com/" + Request.QueryString["ID"].ToString() + "_Trip');</script>";

                if (Session["User"] != null)
                {
                    DataSet dsComments;
                    string commentPrefs = dat.GetData("SELECT * FROM UserPreferences WHERE UserID=" + Session["User"].ToString()).Tables[0].Rows[0]["CommentsPreferences"].ToString();
                    if (commentPrefs == "1")
                    {
                        dsComments = dat.GetData("SELECT C.BlogDate AS theDate, * FROM TripComments C, Users U WHERE U.User_ID=C.UserID AND C.BlogID=" + ID + " ORDER BY C.BlogDate");
                    }
                    else
                    {
                        dsComments = dat.GetData("SELECT DISTINCT U.ProfilePicture, U.User_ID, U.UserName, C.Comment, C.BlogDate AS theDate FROM TripComments C, Users U, User_Friends UF WHERE ((UF.UserID=" + Session["User"].ToString() + " AND UF.FriendID=U.User_ID AND U.User_ID=C.UserID) OR (U.User_ID=" +
                            Session["User"].ToString() + " AND U.User_ID=C.UserID)) AND C.BlogID=" + ID + " ORDER BY C.BlogDate");
                    }
                    TheComments.DATA_SET = dsComments;
                    TheComments.DataBind2(true);
                }
                else
                {
                    DataSet dsComments = dat.GetData("SELECT C.BlogDate AS theDate, * FROM TripComments C, Users U WHERE U.User_ID=C.UserID AND C.BlogID=" + ID + " ORDER BY C.BlogDate");
                    TheComments.DATA_SET = dsComments;
                    TheComments.DataBind2(true);
                }

                ShowDescriptionBegining.Text = dat.BreakUpString(content, 20);


                Session["messageText"] = dat.BreakUpString(dv[0]["Header"].ToString(), 14);
                Session["messageEmail"] = "Adventure Name: <a href=\"http://hippohappenings.com/" + dat.MakeNiceName(dat.BreakUpString(dv[0]["Header"].ToString(), 14)) + "_" + ID + "_Trip\">" +
                    dat.BreakUpString(dv[0]["Header"].ToString(), 14) + "</a> <br/><br/> "+ ShowDescriptionBegining.Text;

                //Media Categories: NONE: 0, Picture: 1, Video: 2, YouTubeVideo: 3, Slider: 4
                Rotator1.Items.Clear();
                int mediaCategory = int.Parse(dv[0]["mediaCategory"].ToString());
                string youtube = dv[0]["YouTubeVideo"].ToString();
                switch (mediaCategory)
                {
                    case 0:
                        break;
                    case 1:
                        char[] delim4 = { ';' };
                        string[] youtokens = youtube.Split(delim4);
                        if (youtube != "")
                        {
                            for (int i = 0; i < youtokens.Length; i++)
                            {
                                if (youtokens[i].Trim() != "")
                                {
                                    Literal literal3 = new Literal();
                                    //literal3.Text = "<object width=\"400\" height=\"250\"><param  name=\"wmode\" value=\"opaque\" ><param name=\"movie\" value=\"http://www.youtube.com/cp/vjVQa1PpcFOFUjhw1qTHaE09Z1e9QYKk9y1JrWf5VAc=\"></param><embed wmode=\"opaque\" src=\"http://www.youtube.com/cp/vjVQa1PpcFOFUjhw1qTHaE09Z1e9QYKk9y1JrWf5VAc=\" type=\"application/x-shockwave-flash\" width=\"400\" height=\"250\"></embed></object>";
                                    literal3.Text = "<div class=\"YouTubeWrapper\"><object class=\"toHidde\" width=\"412\" " +
                                        "height=\"250\"><param name=\"movie\" value=\"http://www.youtube.com/v/" + youtokens[i] +
                                        "\"/><param  name=\"wmode2\" value=\"transparent\" /><param  name=\"wmode\" " +
                                        "value=\"opaque\" /><param name=\"allowFullScreen\" value=\"true\"/><embed " +
                                        "src=\"http://www.youtube.com/v/" +
                                        youtokens[i] + "\" wmode=\"opaque\" wmode2=\"transparent\" type=\"application/x-shockwave" +
                                        "-flash\" allowfullscreen=\"true\" width=\"412\" height=\"250\"/></object></div>";
                                    Telerik.Web.UI.RadRotatorItem r3 = new Telerik.Web.UI.RadRotatorItem();
                                    r3.Controls.Add(literal3);
                                    Rotator1.Items.Add(r3);
                                }
                            }
                        }
                        DataView dsSlider = dat.GetDataDV("SELECT * FROM Trip_Slider_Mapping WHERE TripID=" + ID);
                        if (dsSlider.Count > 0)
                        {
                            try
                            {
                                char[] delim = { '\\' };
                                char[] delim3 = { '.' };
                                string[] fileArray = System.IO.Directory.GetFiles(MapPath(".") + "\\Trips\\" + ID + "\\Slider");

                                string[] finalFileArray = new string[fileArray.Length];

                                for (int i = 0; i < dsSlider.Count; i++)
                                {

                                    string[] tokens = dsSlider[i]["PictureName"].ToString().Split(delim3);

                                    //dsSlider.RowFilter = "PictureName='" + tokens[0] + "." + tokens[1] + "'";
                                    if (tokens.Length >= 2 && dsSlider.Count > 0)
                                    {
                                        if (tokens[1].ToUpper() == "JPG" || tokens[1].ToUpper() == "JPEG" || tokens[1].ToUpper() == "GIF" || tokens[1].ToUpper() == "PNG")
                                        {
                                            System.Drawing.Image image = System.Drawing.Image.FromFile(MapPath(".") + "\\Trips\\" +
                                                ID + "\\Slider\\" + dsSlider[i]["PictureName"].ToString());


                                            int width = 410;
                                            int height = 250;

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

                                            Literal literal4 = new Literal();
                                            string[] nameTokens = dsSlider[i]["RealPictureName"].ToString().Split(delim3);
                                            string realName = dat.MakeNiceName(nameTokens[0]).Replace("_", " ");
                                            literal4.Text = "<div class=\"RotatorImage\"><img  alt=\"" + realName +
                                                "\" style=\" margin-left: " + ((410 - newIntWidth) / 2).ToString() + "px; margin-top: " + ((250 - newHeight) / 2).ToString() + "px;\" height=\"" + newHeight + "px\" width=\"" + newIntWidth + "px\" src=\""
                                                + "Trips/" + ID + "/Slider/" + dsSlider[i]["PictureName"].ToString() + "\" /></div>";
                                            Telerik.Web.UI.RadRotatorItem r4 = new Telerik.Web.UI.RadRotatorItem();
                                            r4.Controls.Add(literal4);

                                            Rotator1.Items.Add(r4);
                                        }
                                    }

                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        break;
                    default: break;
                }

                if (Rotator1.Items.Count == 0)
                    RotatorPanel.Visible = false;
                else
                {
                    RotatorPanel.Visible = true;
                    if (Rotator1.Items.Count == 1)
                    {
                        RotatorPanel.CssClass = "HiddeButtons";
                    }
                }


                if (fillUserData)
                {
                    ASP.controls_sendmessage_ascx SendMessage1 = new ASP.controls_sendmessage_ascx();
                    SendMessage1.THE_TEXT = "Share on Hippo";
                    SendMessage1.RE_LABEL = "Re: " + Session["UserName"].ToString() +
                        " would like to share \"" + dat.BreakUpString(dv[0]["Header"].ToString(), 14) + "\" with you.";
                    SendMessage1.TYPE = "t";
                    SendMessage1.ID = int.Parse(ID);

                    CalendarSharePanel.Controls.Add(SendMessage1);

                    Session["Subject"] = "Re: " + Session["UserName"].ToString() +
                        " would like to share \"" + dat.BreakUpString(dv[0]["Header"].ToString(), 14) + "\" with you.";
                }
            }
            else
            {

                EventName.Text = "This adventure has been disabled";
            }

        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString();
            //Response.Redirect("~/home");
        }

    }

    private string GetMonth(string month)
    {
        switch (month)
        {
            case "1":
                return "January";
                break;
            case "2":
                return "Febuary";
                break;
            case "3":
                return "March";
                break;
            case "4":
                return "April";
                break;
            case "5":
                return "May";
                break;
            case "6":
                return "June";
                break;
            case "7":
                return "July";
                break;
            case "8":
                return "August";
                break;
            case "9":
                return "September";
                break;
            case "10":
                return "October";
                break;
            case "11":
                return "November";
                break;
            case "12":
                return "December";
                break;
            default:
                return "January";
                break;

        }
    }

    protected void GoToEdit(object sender, EventArgs args)
    {
        string eventID = Request.QueryString["ID"].ToString();
        HttpCookie cookie2 = Request.Cookies["BrowserDate"];
        HttpCookie cookie = new HttpCookie("editTrip" + eventID);
        cookie.Value = "True";
        cookie.Expires = DateTime.Parse(cookie2.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(1);
        Response.Cookies.Add(cookie);

        Response.Redirect("enter-trip?edit=true&ID="+eventID);

    }

    protected void GetOtherEvents()
    {
        string eventID = Request.QueryString["ID"];
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        DateTime isNow = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        DataView dvEvent = dat.GetDataDV("SELECT * FROM Trips WHERE ID = " + eventID);

        DataView dvTrip = dat.GetDataDV("SELECT TOP 1 * FROM Trips T, TripDirections TD WHERE " +
            "T.ID=TD.TripID AND T.ID=" + eventID);

        string country = dvTrip[0]["Country"].ToString();
        string state = dvTrip[0]["State"].ToString();
        string city = dvTrip[0]["City"].ToString();

        DataView dvEventCategories = dat.GetDataDV("SELECT * FROM Trips E, Trip_Category ECM WHERE E.ID=ECM.TripID AND E.ID=" + eventID);

        string similarQuery = "SELECT DISTINCT CASE WHEN E.DaysFeatured IS NULL THEN 'False' WHEN E.DaysFeatured LIKE '%" +
            isNow.Date.ToShortDateString() + "%' THEN 'True' ELSE 'False' END AS Featured, E.PostedOn, E.Content, E.Header, E.ID FROM Trips E, TripDirections TD, Trip_Category ECM WHERE " +
            " E.ID=ECM.TripID AND TD.TripID=E.ID AND TD.Country=" + country +
            " AND TD.State='" + state + "' AND TD.City='" + city + "' AND E.ID <> " + eventID;

        if (dvEventCategories.Count != 0)
        {
            similarQuery += " AND (";
            bool gotfirst = false;
            foreach (DataRowView row in dvEventCategories)
            {
                if (gotfirst)
                {
                    similarQuery += " OR ";
                }
                similarQuery += " ECM.CategoryID=" + row["CategoryID"].ToString();
                gotfirst = true;
            }
            similarQuery += " ) ";
        }
        DataView dvSimilar = dat.GetDataDV(similarQuery + " ORDER BY Featured DESC, PostedOn ASC");

        string andNot = "";

        Literal lit = new Literal();

        DataSet dsLatsLongs = dat.GetData("SELECT Latitude, Longitude FROM ZipCodes WHERE Zip='" +
                                        dvTrip[0]["Zip"].ToString() + "'");

        //some zip codes don't exist in the database, find the closest one
        bool findClosest = false;
        int zipParam = int.Parse(dvTrip[0]["Zip"].ToString());
        if (dsLatsLongs.Tables.Count > 0)
        {
            if (dsLatsLongs.Tables[0].Rows.Count > 0)
            {

            }
            else
            {
                findClosest = true;
            }
        }
        else
        {
            findClosest = true;
        }

        if (findClosest)
        {
            dsLatsLongs = null;

            while (dsLatsLongs == null)
            {
                if (zipParam > 99999)
                    zipParam = 10000;
                dsLatsLongs = dat.GetData("SELECT Latitude, Longitude FROM ZipCodes WHERE Zip='" + zipParam++ + "'");
                if (dsLatsLongs.Tables.Count > 0)
                {
                    if (dsLatsLongs.Tables[0].Rows.Count > 0)
                    {

                    }
                    else
                    {
                        dsLatsLongs = null;
                    }
                }
                else
                {
                    dsLatsLongs = null;
                }
            }
        }

        DataSet dsZips = dat.GetData("SELECT Zip FROM ZipCodes WHERE dbo.GetDistance(" + dsLatsLongs.Tables[0].Rows[0]["Longitude"].ToString() +
                ", " + dsLatsLongs.Tables[0].Rows[0]["Latitude"].ToString() + ", Longitude, Latitude) < 5");
        //some zip codes don't exist in the database, find the closest one
        string zip = "";
        string nonExistantZip = " OR V.Zip = '" + dvTrip[0]["Zip"].ToString() + "'";


        if (dsZips.Tables.Count > 0)
        {
            if (dsZips.Tables[0].Rows.Count > 0)
            {
                zip = " AND (V.Zip = '" + dvTrip[0]["Zip"].ToString() + "' " + nonExistantZip;
                for (int i = 0; i < dsZips.Tables[0].Rows.Count; i++)
                {
                    zip += " OR V.Zip = '" + dsZips.Tables[0].Rows[i]["Zip"].ToString() + "' ";
                }
                zip += ") ";
            }
            else
            {
                zip = " AND V.Zip='" + dvTrip[0]["Zip"].ToString() + "'";
            }
        }
        else
        {
            zip = " AND V.Zip='" + dvTrip[0]["Zip"].ToString() + "'";
        }

        int similarCount = 0;
        foreach (DataRowView row in dvSimilar)
        {
            andNot += " AND V.ID <> " + row["ID"].ToString();
            similarCount++;
            if (similarCount == 6)
                break;
        }

        string thisDayQuery = "SELECT DISTINCT CASE WHEN T.DaysFeatured IS NULL THEN 'False' WHEN T.DaysFeatured LIKE '%" +
            isNow.Date.ToShortDateString() + "%' THEN 'True' ELSE 'False' END AS Featured, T.PostedOn, T.ID, T.Header, T.Content FROM Trips T, TripDirections V, " +
            "Trip_Category VCM WHERE " +
            " T.ID=V.TripID AND V.TripID=VCM.TripID AND V.Country=" + dvTrip[0]["Country"].ToString() +
            " AND T.ID <> " + eventID + zip + andNot + " ORDER BY Featured DESC, PostedOn ASC";

        DataView dvDay = dat.GetDataDV(thisDayQuery);

        if (dvDay.Count > 0)
        {
            lit = new Literal();
            lit.Text = "<a id=\"Nearby-Adventures\" class=\"aboutLink\"><h1 class=\"SideColumn\">Nearby Adventures</h1><div class=\"Text12 SimilarSide\"></a>";

            OtherEventsPanel.Controls.Add(lit);

            if (dvSimilar.Count >= 3)
            {
                DrawEvents(dvDay, 3);
            }
            else
            {
                DrawEvents(dvDay, 6 - dvSimilar.Count);
            }

            lit = new Literal();
            lit.Text = "</div>";

            OtherEventsPanel.Controls.Add(lit);
        }

        if (dvSimilar.Count > 0)
        {
            lit = new Literal();
            lit.Text = "<a id=\"Similar-Adventures\" class=\"aboutLink\"><h1 class=\"SideColumn\">Similar Adventures</h1><div class=\"Text12 SimilarSide\"></a>";

            OtherEventsPanel.Controls.Add(lit);

            if (dvDay.Count >= 3)
            {
                DrawEvents(dvSimilar, 3);
            }
            else
            {
                DrawEvents(dvSimilar, 6 - dvDay.Count);
            }

            lit = new Literal();
            lit.Text = "</div>";

            OtherEventsPanel.Controls.Add(lit);
        }
    }

    protected void DrawEvents(DataView dvEvents, int cutOff)
    {
        string eventID = Request.QueryString["ID"];
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        DateTime isNow = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        Literal lit = new Literal();

        int count = 0;
        DataView dvEvent;
        string contentSub = "";
        foreach (DataRowView row in dvEvents)
        {
            if (count > cutOff - 1)
            {
                break;
            }
            else
            {
                contentSub = dat.stripHTML(row["Content"].ToString().Replace("<br>",
                    "").Replace("</br>", "").Replace("<br/>", "").Replace("<BR>", "").Replace("<BR/>",
                    "").Replace("<br />", "").Replace("<BR />", ""));
                if (dat.stripHTML(row["Content"].ToString().Replace("<br>",
                    "").Replace("</br>", "").Replace("<br/>", "").Replace("<BR>", "").Replace("<BR/>",
                    "").Replace("<br />", "").Replace("<BR />", "")).Length > 100)
                    contentSub = dat.stripHTML(row["Content"].ToString().Replace("<br>",
                        "").Replace("</br>", "").Replace("<br/>", "").Replace("<BR>", "").Replace("<BR/>",
                        "").Replace("<br />", "").Replace("<BR />", "")).Substring(0, 100);
                dvEvent = dat.GetDataDV("SELECT * FROM Trips T WHERE T.ID=" + row["ID"].ToString());
                lit.Text += "<div class=\"SimilarSide\">";
                lit.Text += "<a class=\"Green12LinkNF\" href=\"" + dat.MakeNiceName(row["Header"].ToString()) +
                    "_" + row["ID"].ToString() + "_Trip\">" + row["Header"].ToString() +
                    "</a> " + dat.BreakUpString(contentSub, 30) +
                    "... <a class=\"Blue12Link\" href=\"" + dat.MakeNiceName(row["Header"].ToString()) +
                    "_" + row["ID"].ToString() + "_Trip\">Read More</a>";
                lit.Text += "</div>";
                count++;
            }
        }

        OtherEventsPanel.Controls.Add(lit);
    }
  
}
