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

public partial class Ads : System.Web.UI.UserControl
{
    public int THE_WIDTH
    {
        get { return theWidth; }
        set { theWidth = value; isDouble = true; }
    }
    private int theWidth = 848;
    private bool isDouble = false;
    protected void Page_Load(object sender, EventArgs e)
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
        if (Session["LocCountry"] == null || Session["LocState"] == null || Session["LocCity"] == null)
            dat.IP2Location();
        DoAds();

        HtmlControl body = (HtmlControl)dat.FindControlRecursive(this.Page, "bodytag");

        //body.Attributes["onload"] = "\r\nif\r\n(\r\ntypeof(window.myFunction) == 'function')\r\n{\r\nStartRotator2();}" + body.Attributes["onload"];

        body.Attributes["onload"] = "StartRotator2();" + body.Attributes["onload"];
    }

    public Control FindControlRecursive(Control Root, string Id)
    {
        if (Root.ID == Id)
            return Root;

        foreach (Control Ctl in Root.Controls)
        {
            Control FoundCtl = FindControlRecursive(Ctl, Id);

            if (FoundCtl != null)
                return FoundCtl;
        }
        return null;
    }
    protected void DoAds()
    {
        string message = "";
        try
        {
            
            Rotator2.Width = theWidth;
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
            string timeFrame = " AND DatesOfAd LIKE '%;" +
                isNow.Month.ToString() + "/" + isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";%' ";

            string country = Session["LocCountry"].ToString().Trim();
            string state = Session["LocState"].ToString().Trim();
            string zip = "";
            string city = "";
            GetMajorLocation(out zip, out city, out state, out country);

            if (Session["SeenNum"] == null)
                Session["SeenNum"] = 0;
            else
            {
                Session["SeenNum"] = int.Parse(Session["SeenNum"].ToString()) + 1;
            }

            DataView dvAds = dat.GetDataDV("SELECT DISTINCT *, ROW_NUMBER() OVER(ORDER BY DateAdded ASC) AS Row FROM Ads A, Users U WHERE " +
                "A.User_ID=U.User_ID AND A.Featured='True' AND A.CatCountry = " +
                country + " AND A.CatState = '" + state +
                "' AND A.CatCity = '" + city +
                "'" + timeFrame +" ORDER BY DateAdded ASC");
            
            Telerik.Web.UI.RadRotatorItem item;
            Literal lit;
            string w = "0";
            string h = "0";
            Rotator2.Items.Clear();

            int numAds = 4;
            if (isDouble)
                numAds = 2;

            int startIndex = int.Parse(Session["SeenNum"].ToString()) * numAds;

            if (startIndex > dvAds.Count - 1)
            {
                Session["SeenNum"] = 0;
                startIndex = 0;
            }

            int cutOff = 20;
            int count = 0;
            int indexToUse = startIndex;

            if (dvAds.Count == 0)
            {
                UpdatePanel1.Visible = false;
            }
            else
            {
                UpdatePanel1.Visible = true;
                for (int i = 0; i < dvAds.Count; i++)
                {
                    if (count < cutOff)
                    {
                        count++;
                        lit = new Literal();
                        lit.Text = "";

                        indexToUse = i + startIndex;
                        if (indexToUse > dvAds.Count - 1)
                        {
                            indexToUse = 0;
                            startIndex = -i;
                        }

                        if (dvAds[indexToUse]["Template"].ToString() == "1" || dvAds[indexToUse]["Template"].ToString() == "")
                        {
                            lit.Text = "<div align=\"center\" class=\"AdTemplate1\"><div align=\"center\" class=\"AdTemplate1Wrapper\">";

                            if (dvAds[indexToUse]["FeaturedPicture"] != null)
                            {
                                if (dvAds[indexToUse]["FeaturedPicture"].ToString().Trim() != "")
                                {
                                    GetAdSize(ref w, ref h, dvAds[indexToUse]["UserName"].ToString() + "\\" +
                                        dvAds[indexToUse]["FeaturedPicture"].ToString(), "1");

                                    lit.Text += "<div class=\"AdTemplate1Inner\"><table width=\"100px\" height=\"100px\" " +
                                        "cellpadding=\"0\" cellspacing=\"0\"><tbody align=\"center\"><tr><td valign=\"middle\"> " +
                                        "<img alt=\"" + dat.MakeNiceNameFull(dvAds[indexToUse]["Header"].ToString()).Replace("-", " ") + "\" onclick=\"window.location = '../" + dat.MakeNiceName(dvAds[indexToUse]["Header"].ToString()) +
                                            "_" + dvAds[indexToUse]["Ad_ID"].ToString() + "_Ad'\" class=\"AdImage\" width=\"" + w +
                                            "px\" Height=\"" + h + "px\" src=\"UserFiles/" + dvAds[indexToUse]["UserName"].ToString() +
                                            "/" + dvAds[indexToUse]["FeaturedPicture"].ToString() + "\" /> " +
                                        "</td></tr></tbody></table></div>";
                                }
                            }

                            string headerStr = dat.BreakUpString(dvAds[indexToUse]["Header"].ToString(), 10);

                            lit.Text += "<div><h1 class=\"Text14\"><a class=\"AdsTitle\" href=\"../" + dat.MakeNiceName(dvAds[indexToUse]["Header"].ToString()) +
                                            "_" + dvAds[indexToUse]["Ad_ID"].ToString() + "_Ad\">" + headerStr +
                                            "</a></h1></div></div>" +
                                "<div align=\"center\" class=\"AdTemplate1InnerInner\"> " +
                                "<span class=\"Text12\">" +
                                dat.BreakUpString(dvAds[indexToUse]["FeaturedSummary"].ToString(), 21) + "</span>" +

                                "</div>" +
                                "<div><a class=\"ReadMoreHome\" href=\"../" +
                                dat.MakeNiceName(dvAds[indexToUse]["Header"].ToString()) +
                                            "_" + dvAds[indexToUse]["Ad_ID"].ToString() + "_Ad\">Read More</a>" +
                                "</div></div>";
                        }
                        else if (dvAds[indexToUse]["Template"].ToString() == "2")
                        {
                            lit.Text = "<div align=\"center\" class=\"AdTemplate1\"><div class=\"AdTemplate2Wrapper\">";

                            if (dvAds[indexToUse]["FeaturedPicture"] != null)
                            {
                                if (dvAds[indexToUse]["FeaturedPicture"].ToString().Trim() != "")
                                {
                                    GetAdSize(ref w, ref h, dvAds[indexToUse]["UserName"].ToString() + "\\" +
                                        dvAds[indexToUse]["FeaturedPicture"].ToString(), "2");
                                    lit.Text += "<div class=\"AdTemplate1Inner\"><table width=\"198px\" height=\"140px\" " +
                                        "cellpadding=\"0\" cellspacing=\"0\"><tbody align=\"center\"><tr><td valign=\"middle\"> " +
                                        "<img alt=\"" + dat.MakeNiceNameFull(dvAds[indexToUse]["Header"].ToString()).Replace("-", " ") + "\" onclick=\"window.location = '../" + dat.MakeNiceName(dvAds[indexToUse]["Header"].ToString()) +
                                            "_" + dvAds[indexToUse]["Ad_ID"].ToString() + "_Ad'\" class=\"AdImage\" width=\"" + w +
                                            "px\" Height=\"" + h + "px\" src=\"UserFiles/" + dvAds[indexToUse]["UserName"].ToString() +
                                            "/" + dvAds[indexToUse]["FeaturedPicture"].ToString() + "\" runat=\"server\" /> " +
                                        "</td></tr></tbody></table></div>";
                                }
                            }

                            string headerStr = dat.BreakUpString(dvAds[indexToUse]["Header"].ToString(), 21);

                            lit.Text += "</div><div class=\"AdTemplate2Inner\"><h1 class=\"Text14\"><a class=\"AdsTitle\" href=\"../" + dat.MakeNiceName(dvAds[indexToUse]["Header"].ToString()) +
                                            "_" + dvAds[indexToUse]["Ad_ID"].ToString() + "_Ad\" class=\"Text14\">" + headerStr +
                                            "</a></h1></div>" +
                                "<div align=\"center\" class=\"AdTemplate2InnerInner\"> " +
                                "<span class=\"Text12\">" +
                                dat.BreakUpString(dvAds[indexToUse]["FeaturedSummary"].ToString(), 21) + "</span>" +
                                "</div>" +
                                "<div><a class=\"ReadMoreHome\" href=\"../" +
                                dat.MakeNiceName(dvAds[indexToUse]["Header"].ToString()) +
                                            "_" + dvAds[indexToUse]["Ad_ID"].ToString() + "_Ad\">Read More</a>" +
                                "</div></div>";
                        }
                        else if (dvAds[indexToUse]["Template"].ToString() == "3")
                        {
                            lit.Text = "<div class=\"AdTemplate1\"><div>";



                            if (dvAds[indexToUse]["FeaturedPicture"] != null)
                            {
                                if (dvAds[indexToUse]["FeaturedPicture"].ToString().Trim() != "")
                                {
                                    GetAdSize(ref w, ref h, dvAds[indexToUse]["UserName"].ToString() + "\\" +
                                        dvAds[indexToUse]["FeaturedPicture"].ToString(), "3");

                                    lit.Text += "<div center=\"float\" class=\"FloatLeft\"><table width=\"198px\" height=\"262px\" " +
                                        "cellpadding=\"0\" cellspacing=\"0\"><tbody align=\"center\"><tr><td valign=\"middle\"> " +
                                        "<img alt=\"" + dat.MakeNiceNameFull(dvAds[indexToUse]["Header"].ToString()).Replace("-", " ") + "\" onclick=\"window.location = '../" + dat.MakeNiceName(dvAds[indexToUse]["Header"].ToString()) +
                                            "_" + dvAds[indexToUse]["Ad_ID"].ToString() + "_Ad'\" class=\"AdImage\" width=\"" + w +
                                            "px\" Height=\"" + h + "px\" src=\"UserFiles/" + dvAds[indexToUse]["UserName"].ToString() +
                                            "/" + dvAds[indexToUse]["FeaturedPicture"].ToString() + "\" runat=\"server\" /> " +
                                        "</td></tr></tbody></table></div>";
                                }
                            }

                            lit.Text += "</div>";
                        }


                        item = new Telerik.Web.UI.RadRotatorItem();
                        item.Controls.Add(lit);
                        Rotator2.Items.Add(item);
                    }
                }

                string border = "";

                if (dvAds.Count < 4)
                {

                    if (dvAds.Count == 0)
                    {
                        lit = new Literal();
                        lit.Text = "<div class=\"Text12 AdEmpty\">" +
                            "<div class=\"AdEmptyInner\">There are no bulletins posted " +
                            "in your location today. Be the first to feature a bulletin in your location and have it viewed by visitors throughout the site. " +
                            "<br/><a class=\"NavyLinkSmall\" href=\"post-bulletin\"><br/>+Add Bulletin</a></div></div>";

                        item = new Telerik.Web.UI.RadRotatorItem();
                        item.Controls.Add(lit);

                        Rotator2.Items.Add(item);

                        if (!isDouble)
                        {
                            lit = new Literal();
                            lit.Text = "<div class=\"Text12 AdEmptyInnerInner\">" +
                                "</div>";
                            item = new Telerik.Web.UI.RadRotatorItem();
                            item.Controls.Add(lit);

                            Rotator2.Items.Add(item);

                            lit = new Literal();
                            lit.Text = "<div class=\"Text12 AdEmptyInnerInner\">" +
                                "</div>";
                            item = new Telerik.Web.UI.RadRotatorItem();
                            item.Controls.Add(lit);

                            Rotator2.Items.Add(item);
                        }

                        lit = new Literal();
                        lit.Text = "<div class=\"Text12 AdEmptyInnerInnerInner\">" +
                            "</div>";
                        item = new Telerik.Web.UI.RadRotatorItem();
                        item.Controls.Add(lit);

                        Rotator2.Items.Add(item);
                    }
                    else if (dvAds.Count == 1)
                    {
                        if (isDouble)
                            border = "AdEmptyTop";
                        else
                            border = "AdEmptyTop2";
                        lit = new Literal();
                        lit.Text = "<div class=\"Text12 " + border + "\">" +
                            "<div class=\"AdEmptyInner\">There aren't many bulletins posted " +
                            "in your location today. Be the first to feature a bulletin in your location and have it viewed by visitors throughout the site. " +
                            "<br/><a class=\"NavyLinkSmall\" href=\"post-bulletin\"><br/>+Add Bulletin</a></div></div>";

                        item = new Telerik.Web.UI.RadRotatorItem();
                        item.Controls.Add(lit);

                        Rotator2.Items.Add(item);

                        if (!isDouble)
                        {
                            lit = new Literal();
                            lit.Text = "<div class=\"Text12 AdEmptyInnerInner\">" +
                                "</div>";
                            item = new Telerik.Web.UI.RadRotatorItem();
                            item.Controls.Add(lit);

                            Rotator2.Items.Add(item);

                            lit = new Literal();
                            lit.Text = "<div class=\"Text12 AdEmptyInnerInnerInner\">" +
                                "</div>";
                            item = new Telerik.Web.UI.RadRotatorItem();
                            item.Controls.Add(lit);

                            Rotator2.Items.Add(item);
                        }
                    }
                    else if (dvAds.Count == 2)
                    {
                        if (!isDouble)
                        {
                            lit = new Literal();
                            lit.Text = "<div class=\"Text12 AdEmpty\">" +
                                "<div class=\"AdEmptyInner\">There aren't many bulletins posted " +
                            "in your location today. Be the first to feature a bulletin in your location and have it viewed by visitors throughout the site. " +
                            "<br/><a class=\"NavyLinkSmall\" href=\"post-bulletin\"><br/>+Add Bulletin</a></div></div>";
                            item = new Telerik.Web.UI.RadRotatorItem();
                            item.Controls.Add(lit);

                            Rotator2.Items.Add(item);

                            lit = new Literal();
                            lit.Text = "<div class=\"AdEmptyInnerInnerInner\">" +
                                "</div>";
                            item = new Telerik.Web.UI.RadRotatorItem();
                            item.Controls.Add(lit);

                            Rotator2.Items.Add(item);
                        }
                    }
                    else if (dvAds.Count == 3)
                    {
                        if (!isDouble)
                        {
                            lit = new Literal();
                            lit.Text = "<div class=\"Text12 AdTopInner\">" +
                                "<div class=\"AdEmptyInner\">There aren't many bulletins posted " +
                            "in your location today. Be the first to feature a bulletin in your location and have it viewed by visitors throughout the site. " +
                            "<br/><a class=\"NavyLinkSmall\" href=\"post-bulletin\"><br/>+Add Bulletin</a></div></div>";
                            item = new Telerik.Web.UI.RadRotatorItem();
                            item.Controls.Add(lit);

                            Rotator2.Items.Add(item);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            
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

    protected void GetAdSize(ref string w, ref string h, string picture, string templateID)
    {
        try
        {
            string thePath = MapPath("../") + "/UserFiles/" + picture;
            if (MapPath(".").Substring(MapPath(".").Length - 1, 1) == "\\")
            {
                thePath = MapPath("../") + "/UserFiles/" + picture;
            }
            System.Drawing.Image image =
                System.Drawing.Image.FromFile(thePath);

            int height = 100;
            int width = 100;
            if (templateID == "2")
            {
                height = 140;
                width = 190;
            }
            else if (templateID == "3")
            {
                height = 250;
                width = 198;
            }

            int newHeight = 0;
            int newIntWidth = 0;

            float newFloatHeight = 0.00F;
            float newFloatWidth = 0.00F;

            if (image.Height > height || image.Width > width)
            {


                if (image.Height >= image.Width)
                {
                    //leave the height as is
                    newHeight = height;

                    float frak = float.Parse(newHeight.ToString()) / float.Parse(image.Height.ToString());

                    newFloatWidth = image.Width * frak;
                    newIntWidth = (int)newFloatWidth;
                }
                //if image height is greater than resize height...resize it
                else
                {
                    newIntWidth = width;

                    float frak = float.Parse(newIntWidth.ToString()) / float.Parse(image.Width.ToString());

                    newFloatHeight = image.Height * frak;
                    newHeight = (int)newFloatHeight;
                }

                w = newIntWidth.ToString();
                h = newHeight.ToString();
            }
            else
            {
                w = image.Width.ToString();
                h = image.Height.ToString();
            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString();
        }
    }
}
