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

public partial class Controls_BigAd : System.Web.UI.UserControl
{
    public bool IS_WINDOW
    {
        get { return windowT; }
        set { windowT = value; }
    }
    protected bool windowT = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.Cache.SetCacheability(HttpCacheability.NoCache);

        try
        {
            if (!IsPostBack)
            {
                HttpCookie cookie = Request.Cookies["BrowserDate"];
                Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
                DateTime timeNow = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));

                DataView dvUserAds;
                int lastSeen = 0;
                if (Request.Url.AbsolutePath.ToLower() == "/group.aspx")
                {
                    //If we are in the group, all we need to do is get
                    //ads that are in this location and in the groups categories
                    //plus ads that are only in the location and have displayToAll = 'True'
                    string commandStr = "";

                    dvUserAds = dat.GetGroupAds(Request.QueryString["ID"].ToString(), ref commandStr, ref lastSeen);
                }
                else
                {
                    dvUserAds = dat.GetAdSet(1, true);
                }

                DataBind2(dvUserAds, lastSeen);
            }
        }
        catch (Exception ex)
        {
            Errorlabel.Text = ex.ToString() + "<br/><br/>" + Request.Url.AbsolutePath.ToLower();
        }
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        try
        {
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            int currentAdIndex = 0;
            DataView dvAds = new DataView();
            if (Request.Url.AbsolutePath.ToLower() == "/group.aspx")
            {
                string commandStr = "";
                int lastSeen = 0;
                dvAds = dat.GetGroupAds(Request.QueryString["ID"].ToString(), ref commandStr, ref lastSeen);
                
                DataView dv = dat.GetDataDV("SELECT * FROM BigAdStatistics_Anonymous WHERE isGroup='True' AND SessionNum = '" +
                    Session["BigAnonymousGroupSession"].ToString() + "'");
                if (int.Parse(dv[0]["LastSeenIndex"].ToString()) == dat.NUMB_TOTAL_BIG_ADS - 1)
                    currentAdIndex = 0;
                else
                    currentAdIndex = int.Parse(dv[0]["LastSeenIndex"].ToString()) + 1;
            }
            else
            {
                if (Session["ADBigDV"] == null || Session["ADBigCount"] == null)
                    dat.GetAdSet(1, true);

                dvAds = (DataView)Session["ADBigDV"];
                currentAdIndex = int.Parse(Session["ADBigCount"].ToString());

                if (currentAdIndex < dvAds.Count - 1)
                {
                    currentAdIndex++;
                }
                else
                {
                    currentAdIndex = 0;
                }
            }
            

            

            //Draw the next ad
            DrawAd(currentAdIndex, dvAds);
            Session["ADBigCount"] = currentAdIndex;
        }
        catch (Exception ex)
        {
            Errorlabel.Text = ex.ToString();
        }
    }

    protected void DrawAd(int nextAdIndex, DataView dvAds)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        DataView dvAd = dat.GetDataDV("SELECT * FROM Ads WHERE Ad_ID = " + dvAds[nextAdIndex]["Ad_ID"].ToString());
        DataView dvUser = dat.GetDataDV("SELECT * FROM Users WHERE User_ID=" + dvAd[0]["User_ID"].ToString());
        string templateID = dvAd[0]["Template"].ToString();

        string w = "0";
        string h = "0";


        if (templateID == "1")
        {
            Template1Panel.Visible = true;
            Template2Panel.Visible = false;
            CustomerImage.AlternateText = dvAds[nextAdIndex]["Header"].ToString();
            if (dvAds[nextAdIndex]["FeaturedPicture"].ToString() == "" || dvAds[nextAdIndex]["FeaturedPicture"] == null)
            {
                CustomerImage.Visible = false;
                TitleLabel.Text = dat.BreakUpString(dvAds[nextAdIndex]["Header"].ToString(), 21);
            }
            else
            {
                CustomerImage.Visible = true;
                GetAdSize(out w, out h, dvUser[0]["UserName"].ToString() + "\\" +
            dvAds[nextAdIndex]["FeaturedPicture"].ToString(), templateID);
                CustomerImage.ImageUrl = "../UserFiles/" + dvUser[0]["UserName"].ToString() +
                    "/" + dvAds[nextAdIndex]["FeaturedPicture"].ToString();
                CustomerImage.Width = int.Parse(w);
                CustomerImage.Height = int.Parse(h);
                TitleLabel.Text = dat.BreakUpString(dvAds[nextAdIndex]["Header"].ToString(), 10);
            }

            TitleLabel.Text = dvAds[nextAdIndex]["Header"].ToString();

            if (windowT)
            {
                TitleLabel.Attributes.Add("onclick", "CloseWindow('" + "../" + dat.MakeNiceName(dvAds[nextAdIndex]["Header"].ToString()) +
                                "_" + dvAds[nextAdIndex]["Ad_ID"].ToString() + "_Ad" + "');");
                ReadMoreLink.Attributes.Add("onclick", "CloseWindow('" + "../" + dat.MakeNiceName(dvAds[nextAdIndex]["Header"].ToString()) +
                                "_" + dvAds[nextAdIndex]["Ad_ID"].ToString() + "_Ad" + "');");
                A1.HRef = "javascript:CloseWindow('../" + dat.MakeNiceName(dvAds[nextAdIndex]["Header"].ToString()) +
                    "_" + dvAds[nextAdIndex]["Ad_ID"].ToString() + "_Ad');";
            }
            else
            {
                TitleLabel.NavigateUrl = "../" + dat.MakeNiceName(dvAds[nextAdIndex]["Header"].ToString()) +
                                "_" + dvAds[nextAdIndex]["Ad_ID"].ToString() + "_Ad";
                ReadMoreLink.NavigateUrl = "../" + dat.MakeNiceName(dvAds[nextAdIndex]["Header"].ToString()) +
                    "_" + dvAds[nextAdIndex]["Ad_ID"].ToString() + "_Ad";
                A1.HRef = "../" + dat.MakeNiceName(dvAds[nextAdIndex]["Header"].ToString()) +
                    "_" + dvAds[nextAdIndex]["Ad_ID"].ToString() + "_Ad";
            }

            BodyLabel.Text = dat.BreakUpString(dvAds[nextAdIndex]["FeaturedSummary"].ToString(), 21);
        }
        else
        {
            Template1Panel.Visible = false;
            Template2Panel.Visible = true;
            Image1.AlternateText = dvAds[nextAdIndex]["Header"].ToString();
            if (dvAds[nextAdIndex]["FeaturedPicture"].ToString() == "" || dvAds[nextAdIndex]["FeaturedPicture"] == null)
            {
                Image1.Visible = false;
            }
            else
            {
                GetAdSize(out w, out h, dvUser[0]["UserName"].ToString() + "\\" +
            dvAds[nextAdIndex]["FeaturedPicture"].ToString(), templateID);
                Image1.ImageUrl = "../UserFiles/" + dvUser[0]["UserName"].ToString() +
                    "/" + dvAds[nextAdIndex]["FeaturedPicture"].ToString();
                Image1.Width = int.Parse(w);
                Image1.Height = int.Parse(h);
            }

            if (windowT)
            {
                Image1.Attributes.Add("onclick", "CloseWindow('" + "../" + dat.MakeNiceName(dvAds[nextAdIndex]["Header"].ToString()) +
                                "_" + dvAds[nextAdIndex]["Ad_ID"].ToString() + "_Ad" + "');");
            }
            else
            {
                Image1.Attributes.Add("onclick", "window.location = '" + "../" + dat.MakeNiceName(dvAds[nextAdIndex]["Header"].ToString()) +
                                "_" + dvAds[nextAdIndex]["Ad_ID"].ToString() + "_Ad" + "'");
            }
        }

        if (Request.Url.AbsolutePath.ToLower() == "/group.aspx")
        {
            dat.CountGroupAds(int.Parse(dvAds[nextAdIndex]["Ad_ID"].ToString()));
        }
        else
        {
            if (Session["User"] != null)
            {
                dat.CountAd(true, dvAds);
            }
            else
            {
                dat.CountAdAnonymous(true, dvAds);
            }
        }
    }

    protected void GetAdSize(out string w, out string h, string picture, string templateID)
    {
        System.Drawing.Image image =
            System.Drawing.Image.FromFile(MapPath("../") + "\\UserFiles\\" + picture);

        int height = 100;
        int width = 100;

        if (templateID == "1")
        {
            height = 190;
            width = 212;
        }
        else
        {
            height = 190;
            width = 415;
        }

        int newHeight = 0;
        int newIntWidth = 0;

        float newFloatHeight = 0.00F;
        float newFloatWidth = 0.00F;

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

            float frak = newIntWidth / image.Width;

            newFloatHeight = image.Height * frak;
            newHeight = (int)newFloatHeight;
        }

        w = newIntWidth.ToString();
        h = newHeight.ToString();

        if (w == "0")
        {
            if (templateID == "1")
                w = "212";
            else
                w = "415";
        }
        if (h == "0")
            w = "190";
    }

    public void DataBind2(DataView dvAds, int lastSeen)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        Session["ADBigCount"] = 0;
        Session["ADBigDV"] = dvAds;
        DrawAd(lastSeen, dvAds);
        
    }
}
