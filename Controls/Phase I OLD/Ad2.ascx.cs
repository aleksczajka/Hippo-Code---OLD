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
using Telerik.Web.UI;

public partial class Ad2 : System.Web.UI.UserControl
{
    public string TITLE
    {
        get { return title; }
        set { title = value; }
    }
    public string BODY
    {
        get { return body; }
        set { body = value; }
    }
    public string PICTURE
    {
        get { return picture; }
        set { picture = value; }
    }
    public string AD_ID
    {
        get { return adID; }
        set { adID = value; }
    }
    public DataSet DS_ADS
    {
        get { return dsAds; }
        set { dsAds = value; }
    }
    private string title;
    private string body;
    private string picture;
    private string adID;
    private DataSet dsAds;
    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        try
        {
            if (!IsPostBack)
            {
                if (Session["User"] != null)
                {
                    if (Session["AD2Count"] == null || Session["AD2DV"] == null)
                        dat.GetAdSet(2, false);
                    else
                    {
                        if (Application[Session["User"].ToString() + "_Normal"] != null)
                        {
                            if (Application[Session["User"].ToString() + "_Normal"].ToString() == "set")
                            {
                                dat.GetAdSet(2, false);
                                Application[Session["User"].ToString() + "_Normal"] = null;
                            }
                        }
                    }
                }
                else
                {
                    if (Session["AD2Count"] == null || Session["AD2DV"] == null)
                        dat.GetAdSet(2, false);
                }

                DataBind2((DataView)Session["AD2DV"]);
            }
        }
        catch (Exception ex)
        {
            BodyLabel.Text = ex.ToString();
        }

    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        try
        {
            HttpCookie cookie = Request.Cookies["BrowserDate"];
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            if (Session["AD2Count"] == null || Session["AD2DV"] == null)
                dat.GetAdSet(2, false);

            DataView dvAds = (DataView)Session["AD2DV"];
            int currentAdIndex = int.Parse(Session["AD2Count"].ToString());

            if (currentAdIndex < dvAds.Count - 1)
            {
                currentAdIndex++;
            }
            else
            {
                currentAdIndex = 0;
            }

            //Draw the next ad
            DrawAd(currentAdIndex, dvAds);
            Session["AD2Count"] = currentAdIndex;
            //Only count the ad when user is logged in. 
            //If user not logged in, can't charge for the ads.

            //WE DONT NEED TO COUNT THE AD HERE, AD1 COUNTS ALL THE ADS FOR US

            //if (Session["User"] != null)
            //{
            //    dat.CountAd(false);
            //}
        }
        catch (Exception ex)
        {
            BodyLabel.Text = ex.ToString();
        }
    }

    protected void DrawAd(int nextAdIndex, DataView dvAds)
    {
        try
        {
            HttpCookie cookie = Request.Cookies["BrowserDate"];
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            DataView dvUser = dat.GetDataDV("SELECT * FROM Users WHERE User_ID=" + dvAds[nextAdIndex]["User_ID"].ToString());
            string templateID = dat.GetDataDV("SELECT * FROM Ads WHERE Ad_ID=" + dvAds[nextAdIndex]["Ad_ID"].ToString())[0]["Template"].ToString();

            if (dvAds[nextAdIndex]["FeaturedPicture"] != null)
            {
                if (dvAds[nextAdIndex]["FeaturedPicture"].ToString().Trim() != "")
                {
                    DrawTemplateAd(templateID, dvUser, dvAds, nextAdIndex);
                }
            }
            if (dvAds[nextAdIndex]["FeaturedPicture"].ToString() == "" || dvAds[nextAdIndex]["FeaturedPicture"] == null)
            {
                CustomerImage.Visible = false;
                ImagePanel.Visible = false;
                Template1Panel.Visible = true;
                Template2Panel.Visible = false;
                Template3Panel.Visible = false;
                TitleLabel.Text = dat.BreakUpString(dvAds[nextAdIndex]["Header"].ToString(), 21);
                TitleLabel.NavigateUrl = "../" + dat.MakeNiceName(dvAds[nextAdIndex]["Header"].ToString()) +
                "_" + dvAds[nextAdIndex]["Ad_ID"].ToString() + "_Ad";
                BodyLabel.Text = dat.BreakUpString(dvAds[nextAdIndex]["FeaturedSummary"].ToString(), 21);
                ReadMoreLink.NavigateUrl = "../" + dat.MakeNiceName(dvAds[nextAdIndex]["Header"].ToString()) +
                    "_" + dvAds[nextAdIndex]["Ad_ID"].ToString() + "_Ad";
            }
        }
        catch (Exception ex)
        {
            BodyLabel.Text = ex.ToString();
        }
    }
    
    protected void DrawTemplateAd(string AdTemplate, DataView dvUser, DataView dvAds, int nextAdIndex)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        string w = "0";
        string h = "0";

        BodyLabel.Text = "template ID=" + AdTemplate;

        GetAdSize(ref w, ref h, dvUser[0]["UserName"].ToString() + "\\" +
            dvAds[nextAdIndex]["FeaturedPicture"].ToString(), AdTemplate);

        switch (AdTemplate)
        {
            case "1":
                Template1Panel.Visible = true;
                Template2Panel.Visible = false;
                Template3Panel.Visible = false;

                CustomerImage.Width = int.Parse(w);
                CustomerImage.Height = int.Parse(h);

                if (CustomerImage.Width == 0)
                    CustomerImage.Width = 100;
                if (CustomerImage.Height == 0)
                    CustomerImage.Height = 100;

                CustomerImage.AlternateText = dvAds[nextAdIndex]["Header"].ToString();
                CustomerImage.Attributes.Add("onclick", "window.location = '../" +
                    dat.MakeNiceName(dvAds[nextAdIndex]["Header"].ToString()) +
                    "_" + dvAds[nextAdIndex]["Ad_ID"].ToString() + "_Ad'");
                ImagePanel.Visible = true;
                CustomerImage.Visible = true;
                CustomerImage.ImageUrl = "../UserFiles/" + dvUser[0]["UserName"].ToString() +
                    "/" + dvAds[nextAdIndex]["FeaturedPicture"].ToString();
                TitleLabel.Text = dat.BreakUpString(dvAds[nextAdIndex]["Header"].ToString(), 10);
                TitleLabel.NavigateUrl = "../" + dat.MakeNiceName(dvAds[nextAdIndex]["Header"].ToString()) +
                "_" + dvAds[nextAdIndex]["Ad_ID"].ToString() + "_Ad";
                BodyLabel.Text = dat.BreakUpString(dvAds[nextAdIndex]["FeaturedSummary"].ToString(), 21);
                ReadMoreLink.NavigateUrl = "../" + dat.MakeNiceName(dvAds[nextAdIndex]["Header"].ToString()) +
                    "_" + dvAds[nextAdIndex]["Ad_ID"].ToString() + "_Ad";

                break;
            case "2":
                Template1Panel.Visible = false;
                Template2Panel.Visible = true;
                Template3Panel.Visible = false;
                Image1.Width = int.Parse(w);
                Image1.Height = int.Parse(h);

                if (Image1.Width == 0)
                    Image1.Width = 200;
                if (Image1.Height == 0)
                    Image1.Height = 140;

                Image1.AlternateText = dvAds[nextAdIndex]["Header"].ToString();

                Image1.Visible = true;
                Image1.ImageUrl = "../UserFiles/" + dvUser[0]["UserName"].ToString() +
                    "/" + dvAds[nextAdIndex]["FeaturedPicture"].ToString();

                Image1.Attributes.Add("onclick", "window.location = '../" +
                    dat.MakeNiceName(dvAds[nextAdIndex]["Header"].ToString()) +
                    "_" + dvAds[nextAdIndex]["Ad_ID"].ToString() + "_Ad'");

                HyperLink1.Text = dat.BreakUpString(dvAds[nextAdIndex]["Header"].ToString(), 10);

                HyperLink1.Text = dat.BreakUpString(dvAds[nextAdIndex]["Header"].ToString(), 21);
                HyperLink1.NavigateUrl = "../" + dat.MakeNiceName(dvAds[nextAdIndex]["Header"].ToString()) +
                    "_" + dvAds[nextAdIndex]["Ad_ID"].ToString() + "_Ad";
                Label1.Text = dat.BreakUpString(dvAds[nextAdIndex]["FeaturedSummary"].ToString(), 21);
                HyperLink2.NavigateUrl = "../" + dat.MakeNiceName(dvAds[nextAdIndex]["Header"].ToString()) +
                    "_" + dvAds[nextAdIndex]["Ad_ID"].ToString() + "_Ad";
                break;
            case "3":
                Template1Panel.Visible = false;
                Template2Panel.Visible = false;
                Template3Panel.Visible = true;

                Image2.Width = int.Parse(w);
                Image2.Height = int.Parse(h);

                if (Image2.Width == 0)
                    Image2.Width = 200;
                if (Image2.Height == 0)
                    Image2.Height = 250;

                Image2.AlternateText = dvAds[nextAdIndex]["Header"].ToString();
                Image2.Attributes.Add("onclick", "window.location = '../" +
                    dat.MakeNiceName(dvAds[nextAdIndex]["Header"].ToString()) +
                    "_" + dvAds[nextAdIndex]["Ad_ID"].ToString() + "_Ad'");
                Image2.Visible = true;
                Image2.ImageUrl = "../UserFiles/" + dvUser[0]["UserName"].ToString() +
                    "/" + dvAds[nextAdIndex]["FeaturedPicture"].ToString();
                break;
            default: break;

        }


    }

    protected void GetAdSize(ref string w, ref string h, string picture, string templateID)
    {

        string thePath = MapPath("../") + "\\UserFiles\\" + picture;
        if (MapPath("../").Substring(MapPath("../").Length - 1, 1) == "\\")
        {
            thePath = MapPath("../") + "UserFiles\\" + picture;
        }
        System.Drawing.Image image =
            System.Drawing.Image.FromFile(thePath);

        int height = 100;
        int width = 100;
        if (templateID == "2")
        {
            height = 140;
            width = 200;
        }
        else if (templateID == "3")
        {
            height = 250;
            width = 200;
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
    
    public void DataBind2(DataView dvAds)
    {
        try
        {
            HttpCookie cookie = Request.Cookies["BrowserDate"];
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            if (Session["AD2Count"] == null)
                Session["AD2Count"] = 0;
            Session["AD2DV"] = dvAds;
            DrawAd((int)Session["AD2Count"], dvAds);
        }
        catch (Exception ex)
        {
            BodyLabel.Text = ex.ToString();
        }
        //AD1 CODE DOES THIS FOR US
        //if (Session["User"] != null)
        //{
        //    dat.CountAd(false);
        //}


        //Ajax.Utility.RegisterTypeForAjax(typeof(Ad2));
       

    

        //XmlDataSource xmlAds = new XmlDataSource();
        //xmlAds.DataFile = Server.MapPath(".") + "\\UserFiles\\categoies2.xml";
        //xmlAds.DataBind();

        //RadRotator1.DataSource = xmlAds;
        //RadRotator1.DataBind();
    }

    //[Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
    //public int SendData(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")))
    //{
    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //    return dat.PerformAdCount("2", false);
    //}
}
