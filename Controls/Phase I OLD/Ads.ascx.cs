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

public partial class Controls_Ads : System.Web.UI.UserControl
{

    public DataSet DATA_SET
    {
        get { return dsAds; }
        set { dsAds = value; }
    }
    public DataSet MAIN_AD_DATA_SET
    {
        get { return dsMainAds; }
        set { dsMainAds = value; }
    }
    private DataSet dsAds;
    private DataSet dsMainAds;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);

        //if (Session["command"] != null)
        //    ErrorLit.Text = Session["command"].ToString();

        try
        {
            HttpCookie cookie = Request.Cookies["BrowserDate"];
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            //if (!IsPostBack)
            //{
            //    DataView ds1 = dat.GetAdSet(1);
            //    DataView ds2 = dat.GetAdSet(2);
            //    DataView ds3 = dat.GetAdSet(3);
            //    DataView ds4 = dat.GetAdSet(4);



            //    Ad1.DataBind2(ds1);
            //    Ad2.DataBind2(ds2);
            //    Ad3.DataBind2(ds3);
            //    Ad4.DataBind2(ds4);
            //}
        }
        catch (Exception ex)
        {
            ErrorLit.Text = ex.ToString();
        }
    }

    //public void LoadIt()
    //{
    //    Ajax.Utility.RegisterTypeForAjax(typeof(Controls_Ads));
    //    DoMainAd();
    //    //if (Session["AdsLoaded"] == null)
    //    //{
    //    //Get the ID of the last ad seen. The ads will start after this index
    //    string startID = "";
    //    bool doFast = false;
    //    DataSet ds1 = new DataSet();
    //    DataSet ds2 = new DataSet();
    //    DataSet ds3 = new DataSet();
    //    DataSet ds4 = new DataSet();

    //    if (Session["User"] != null)
    //    {
    //        if (Session[Session["User"].ToString() + "_lastseen"] != null)
    //        {
    //            startID = Session[Session["User"].ToString() + "_lastseen"].ToString();
    //        }

    //        if (Session["Ad1_" + Session["User"].ToString()] != null)
    //        {
    //            doFast = true;
    //            ds1 = (DataSet)Session["Ad1_" + Session["User"].ToString()];
    //            ds2 = (DataSet)Session["Ad2_" + Session["User"].ToString()];
    //            ds3 = (DataSet)Session["Ad3_" + Session["User"].ToString()];
    //            ds4 = (DataSet)Session["Ad4_" + Session["User"].ToString()];
    //        }
    //    }
    //    else if (Session["GenericUser_lastseen"] != null)
    //    {
    //        startID = Session["GenericUser_lastseen"].ToString();

    //        if (Session["Ad1_Generic"] != null)
    //        {
    //            doFast = true;
    //            ds1 = (DataSet)Session["Ad1_Generic"];
    //            ds2 = (DataSet)Session["Ad2_Generic"];
    //            ds3 = (DataSet)Session["Ad3_Generic"];
    //            ds4 = (DataSet)Session["Ad4_Generic"];
    //        }
    //    }

    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

    //    if (doFast)
    //    {

    //        //get the index for the ad that was last seen + 1;
    //        int startIndex = 0;

    //        DataView dv = new DataView(dsAds.Tables[0], "", "", DataViewRowState.CurrentRows);

    //        if (startID.Trim() != "45" && startID != "")
    //        {
    //            dv.RowFilter = "Ad_ID=" + startID;
    //            startIndex = int.Parse(dv[0]["Row"].ToString()) + 1;
    //            if (startIndex >= dat.numberOfAllAdsInDay)
    //            {
    //                startIndex = 0;
    //            }
    //        }

    //        for (int i = 0; i < startIndex; i = i + 4)
    //        {
    //            DataRow dr1 = ds1.Tables[0].Rows[i];
    //            ds1.Tables[0].Rows.RemoveAt(i);
    //            ds1.Tables[0].Rows.Add(dr1);

    //            DataRow dr2 = ds2.Tables[0].Rows[i];
    //            ds2.Tables[0].Rows.RemoveAt(i);
    //            ds2.Tables[0].Rows.Add(dr2);

    //            DataRow dr3 = ds3.Tables[0].Rows[i];
    //            ds3.Tables[0].Rows.RemoveAt(i);
    //            ds3.Tables[0].Rows.Add(dr3);

    //            DataRow dr4 = ds4.Tables[0].Rows[i];
    //            ds4.Tables[0].Rows.RemoveAt(i);
    //            ds4.Tables[0].Rows.Add(dr4);
    //        }






    //        dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //        //ds = dat.GetData("SELECT * FROM Ads WHERE Featured='True'");






    //        if (Session["User"] != null)
    //        {
    //            Session["Ad1_" + Session["User"].ToString()] = ds1;
    //            //if(Session["Ad1_Count" + Session["User"].ToString()] == null)
    //            Session["Ad1_Count" + Session["User"].ToString()] = "0";

    //            Session["Ad2_" + Session["User"].ToString()] = ds2;
    //            //if(Session["Ad2_Count" + Session["User"].ToString()] == null)
    //            Session["Ad2_Count" + Session["User"].ToString()] = "0";

    //            Session["Ad3_" + Session["User"].ToString()] = ds3;
    //            //if(Session["Ad3_Count" + Session["User"].ToString()] == null)
    //            Session["Ad3_Count" + Session["User"].ToString()] = "0";

    //            Session["Ad4_" + Session["User"].ToString()] = ds4;
    //            //if(Session["Ad4_Count" + Session["User"].ToString()] == null)
    //            Session["Ad4_Count" + Session["User"].ToString()] = "0";
    //        }
    //        else
    //        {
    //            Session["Ad1_Generic"] = ds1;
    //            //if(Session["Ad1_Count_Generic"] == null)
    //            Session["Ad1_Count_Generic"] = "0";

    //            Session["Ad2_Generic"] = ds2;
    //            //if (Session["Ad2_Count_Generic"] == null)
    //            Session["Ad2_Count_Generic"] = "0";

    //            Session["Ad3_Generic"] = ds3;
    //            //if (Session["Ad3_Count_Generic"] == null)
    //            Session["Ad3_Count_Generic"] = "0";

    //            Session["Ad4_Generic"] = ds4;
    //            //if (Session["Ad4_Count_Generic"] == null)
    //            Session["Ad4_Count_Generic"] = "0";
    //        }

    //        //if user has bandwidth for more ads, get the ads that have 'DisplayToAll' set
    //        DataSet dsHippAd = dat.RetrieveAdminAd();

    //        if (ds1.Tables[0].Rows.Count < dat.numberOfAdsInADay)
    //        {
    //            int numCount = dat.numberOfAdsInADay - ds1.Tables[0].Rows.Count;
    //            for (int i = 0; i < numCount; i++)
    //            {
    //                DataRow row = ds1.Tables[0].NewRow();
    //                row["UserName"] = dsHippAd.Tables[0].Rows[0]["UserName"];
    //                row["Ad_ID"] = dsHippAd.Tables[0].Rows[0]["Ad_ID"].ToString();
    //                row["FeaturedSummary"] = dsHippAd.Tables[0].Rows[0]["FeaturedSummary"].ToString();
    //                row["Description"] = dsHippAd.Tables[0].Rows[0]["Description"].ToString();
    //                row["Header"] = dsHippAd.Tables[0].Rows[0]["Header"].ToString();
    //                row["FeaturedPicture"] = dsHippAd.Tables[0].Rows[0]["FeaturedPicture"].ToString();
    //                row["FeaturedPicture2"] = dsHippAd.Tables[0].Rows[0]["FeaturedPicture2"].ToString();
    //                row["Row"] = dsHippAd.Tables[0].Rows[0]["Row"].ToString();
    //                ds1.Tables[0].Rows.Add(row);
    //            }
    //        }
    //        if (ds2.Tables[0].Rows.Count < dat.numberOfAdsInADay)
    //        {
    //            int numCount = dat.numberOfAdsInADay - ds2.Tables[0].Rows.Count;
    //            for (int i = 0; i < numCount; i++)
    //            {
    //                DataRow row = ds2.Tables[0].NewRow();
    //                row["UserName"] = dsHippAd.Tables[0].Rows[0]["UserName"];
    //                row["Ad_ID"] = dsHippAd.Tables[0].Rows[0]["Ad_ID"].ToString();
    //                row["FeaturedSummary"] = dsHippAd.Tables[0].Rows[0]["FeaturedSummary"].ToString();
    //                row["Description"] = dsHippAd.Tables[0].Rows[0]["Description"].ToString();
    //                row["Header"] = dsHippAd.Tables[0].Rows[0]["Header"].ToString();
    //                row["FeaturedPicture"] = dsHippAd.Tables[0].Rows[0]["FeaturedPicture"].ToString();
    //                row["FeaturedPicture2"] = dsHippAd.Tables[0].Rows[0]["FeaturedPicture2"].ToString();
    //                row["Row"] = dsHippAd.Tables[0].Rows[0]["Row"].ToString();
    //                ds2.Tables[0].Rows.Add(row);
    //            }
    //        }

    //        if (ds3.Tables[0].Rows.Count < dat.numberOfAdsInADay)
    //        {
    //            int numCount = dat.numberOfAdsInADay - ds3.Tables[0].Rows.Count;
    //            for (int i = 0; i < numCount; i++)
    //            {
    //                DataRow row = ds3.Tables[0].NewRow();
    //                row["UserName"] = dsHippAd.Tables[0].Rows[0]["UserName"];
    //                row["Ad_ID"] = dsHippAd.Tables[0].Rows[0]["Ad_ID"].ToString();
    //                row["FeaturedSummary"] = dsHippAd.Tables[0].Rows[0]["FeaturedSummary"].ToString();
    //                row["Description"] = dsHippAd.Tables[0].Rows[0]["Description"].ToString();
    //                row["Header"] = dsHippAd.Tables[0].Rows[0]["Header"].ToString();
    //                row["FeaturedPicture"] = dsHippAd.Tables[0].Rows[0]["FeaturedPicture"].ToString();
    //                row["FeaturedPicture2"] = dsHippAd.Tables[0].Rows[0]["FeaturedPicture2"].ToString();
    //                row["Row"] = dsHippAd.Tables[0].Rows[0]["Row"].ToString();
    //                ds3.Tables[0].Rows.Add(row);
    //            }
    //        }

    //        if (ds4.Tables[0].Rows.Count < dat.numberOfAdsInADay)
    //        {
    //            int numCount = dat.numberOfAdsInADay - ds4.Tables[0].Rows.Count;
    //            for (int i = 0; i < numCount; i++)
    //            {
    //                DataRow row = ds4.Tables[0].NewRow();
    //                row["UserName"] = dsHippAd.Tables[0].Rows[0]["UserName"];
    //                row["Ad_ID"] = dsHippAd.Tables[0].Rows[0]["Ad_ID"].ToString();
    //                row["FeaturedSummary"] = dsHippAd.Tables[0].Rows[0]["FeaturedSummary"].ToString();
    //                row["Description"] = dsHippAd.Tables[0].Rows[0]["Description"].ToString();
    //                row["Header"] = dsHippAd.Tables[0].Rows[0]["Header"].ToString();
    //                row["FeaturedPicture"] = dsHippAd.Tables[0].Rows[0]["FeaturedPicture"].ToString();
    //                row["FeaturedPicture2"] = dsHippAd.Tables[0].Rows[0]["FeaturedPicture2"].ToString();
    //                row["Row"] = dsHippAd.Tables[0].Rows[0]["Row"].ToString();
    //                ds4.Tables[0].Rows.Add(row);
    //            }
    //        }

    //        //this is taken care of by setting startIndex = 0 if startIndex is greater than dat.numberofadsinaday 
    //        //DataView dv1 = new DataView(ds1.Tables[0], " Row < " + dat.numberOfAdsInADay, "", DataViewRowState.CurrentRows);
    //        //DataView dv2 = new DataView(ds2.Tables[0], " Row < " + dat.numberOfAdsInADay, "", DataViewRowState.CurrentRows);
    //        //DataView dv3 = new DataView(ds3.Tables[0], " Row < " + dat.numberOfAdsInADay, "", DataViewRowState.CurrentRows);
    //        //DataView dv4 = new DataView(ds4.Tables[0], " Row < " + dat.numberOfAdsInADay, "", DataViewRowState.CurrentRows);


    //        //Write out the schema that this DataSet created,    
    //        //use the WriteXmlSchema method of the DataSet class   
    //        //ds1.WriteXmlSchema(Server.MapPath("\\UserFiles\\categories1.xsd"));


    //        // To write out the contents of the DataSet as XML,    
    //        //use a file name to call the WriteXml method of the DataSet class   
    //        ds1.WriteXml(Server.MapPath(".") + "\\UserFiles\\categoies1.xml", XmlWriteMode.IgnoreSchema);
    //        ds2.WriteXml(Server.MapPath(".") + "\\UserFiles\\categoies2.xml", XmlWriteMode.IgnoreSchema);
    //        ds3.WriteXml(Server.MapPath(".") + "\\UserFiles\\categoies3.xml", XmlWriteMode.IgnoreSchema);
    //        ds4.WriteXml(Server.MapPath(".") + "\\UserFiles\\categoies4.xml", XmlWriteMode.IgnoreSchema);

    //        //Ad1.DataBind2();
    //        //Ad2.DataBind2();
    //        //Ad3.DataBind2();
    //        //Ad4.DataBind2();
    //        Session["AdsLoaded"] = "true";
    //    }
    //    else
    //    {
    //        GetAds();
    //    }
    //}

    //protected void DoMainAd()
    //{
    //    string startID = "";
    //        bool doFast = false;
    //        DataSet dsMain = new DataSet();
    //        dsMain.Tables.Add();
    //        //get the index for the ad that was last seen + 1;
    //        int startIndex = 0;

    //        if (Session["User"] != null)
    //        {
    //            if (Session[Session["User"].ToString() + "_Main_lastseen"] != null)
    //            {
    //                startID = Session[Session["User"].ToString() + "_Main_lastseen"].ToString();
    //            }

    //            if (Session["AdMain_" + Session["User"].ToString()] != null)
    //            {
    //                doFast = true;
    //                dsMain = (DataSet)Session["AdMain_" + Session["User"].ToString()];
    //            }
    //        }
    //        else if (Session["AdMain_Count_Generic"] != null)
    //        {
    //            startIndex = int.Parse(Session["AdMain_Count_Generic"].ToString());
    //        }
    //        //else if (Session["GenericUser_Main_lastseen"] != null)
    //        //{
    //        //    startID = Session["GenericUser_Main_lastseen"].ToString();

    //        //    if (Session["AdMain_Generic"] != null)
    //        //    {
    //        //        doFast = true;
    //        //        dsMain = (DataSet)Session["AdMain_Generic"];
    //        //    }
    //        //}

    //        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

    //        if (doFast)
    //        {
                
                

    //            //DataView dv = new DataView(dsMainAds.Tables[0], "", "", DataViewRowState.CurrentRows);

    //            //if (startID.Trim() != "45" && startID != "")
    //            //{
    //            //    dv.RowFilter = "Ad_ID=" + startID;
    //            //    startIndex = int.Parse(dv[0]["Row"].ToString()) + 1;
    //            //    if (startIndex >= dat.numberOfMainAdsInADay)
    //            //        startIndex = 0;

    //            //}

    //            for (int i = 0; i < startIndex; i++)
    //            {
    //                DataRow dr1 = dsMain.Tables[0].Rows[i];
    //                dsMain.Tables[0].Rows.RemoveAt(i);
    //                dsMain.Tables[0].Rows.Add(dr1);
    //            }






    //            dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //            //ds = dat.GetData("SELECT * FROM Ads WHERE Featured='True'");

                
                



    //            if (Session["User"] != null)
    //            {
    //                Session["AdMain_" + Session["User"].ToString()] = dsMain;
    //                //if(Session["Ad1_Count" + Session["User"].ToString()] == null)
    //                Session["AdMain_Count" + Session["User"].ToString()] = "0";
    //            }
    //            else
    //            {
    //                Session["AdMain_Generic"] = dsMain;
    //                //if(Session["Ad1_Count_Generic"] == null)
    //                Session["AdMain_Count_Generic"] = "0";
    //            }

    //            //if user has bandwidth for more ads, get the ads that have 'DisplayToAll' set
    //            DataSet dsHippAd = dat.RetrieveAdminAd();

                

    //            if (dsMain.Tables[0].Rows.Count < dat.numberOfMainAdsInADay)
    //            {
    //                int numCount = dat.numberOfMainAdsInADay - dsMain.Tables[0].Rows.Count;
    //                for (int i = 0; i < numCount; i++)
    //                {
    //                    DataRow row = dsMain.Tables[0].NewRow();
    //                    row["UserName"] = dsHippAd.Tables[0].Rows[0]["UserName"];
    //                    row["Ad_ID"] = dsHippAd.Tables[0].Rows[0]["Ad_ID"].ToString();
    //                    row["FeaturedSummary"] = dsHippAd.Tables[0].Rows[0]["FeaturedSummary"].ToString();
    //                    row["Description"] = dsHippAd.Tables[0].Rows[0]["Description"].ToString();
    //                    row["Header"] = dsHippAd.Tables[0].Rows[0]["Header"].ToString();
    //                    row["FeaturedPicture"] = dsHippAd.Tables[0].Rows[0]["FeaturedPicture"].ToString();
    //                    row["FeaturedPicture2"] = dsHippAd.Tables[0].Rows[0]["FeaturedPicture2"].ToString();
    //                    row["Row"] = dsHippAd.Tables[0].Rows[0]["Row"].ToString();
    //                    dsMain.Tables[0].Rows.Add(row);
    //                }
    //            }


    //            dsMain.WriteXml(Server.MapPath(".") + "\\UserFiles\\categoiesMain.xml", XmlWriteMode.IgnoreSchema);

    //            XmlDataSource xmlAds = new XmlDataSource();
    //            xmlAds.DataFile = Server.MapPath(".") + "\\UserFiles\\categoiesMain.xml";
    //            xmlAds.DataBind();

    //            RadRotator1.DataSource = xmlAds;
    //            RadRotator1.DataBind();



    //            Session["MainAdLoaded"] = "true";
    //        }
    //        else
    //        {
    //            GetMainAds();
    //        }


    //}

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    //if (Session["AdsLoaded"] == null)
    //    //{
    //    //Get the ID of the last ad seen. The ads will start after this index
    //    string startID = "";

    //    if (Session["User"] != null)
    //    {
    //        if (Session[Session["User"].ToString() + "_lastseen"] != null)
    //        {
    //            startID = Session[Session["User"].ToString() + "_lastseen"].ToString();
    //        }
    //    }
    //    else if (Session["GenericUser_lastseen"] != null)
    //    {
    //        startID = Session["GenericUser_lastseen"].ToString();
    //    }

    //    GetAds();
    //}

    //protected void GetAds()
    //{
    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

    //    //if (Session["Ad1_" + Session["User"].ToString()] == null)
    //    //{
    //    //DataSet ds = dat.GetData("SELECT * FROM Ads WHERE Ad_ID=3");

    //    string startID = "";
    //    if (Session["User"] != null)
    //    {
    //        if (Session[Session["User"].ToString() + "_lastseen"] != null)
    //        {
    //            startID = Session[Session["User"].ToString() + "_lastseen"].ToString();
    //        }
    //    }
    //    else if (Session["GenericUser_lastseen"] != null)
    //    {
    //        startID = Session["GenericUser_lastseen"].ToString();
    //    }

    //    if (dsAds != null)
    //    {
    //        DataSet ds1 = dsAds.Clone();
    //        ds1.Tables[0].Rows.Clear();
    //        DataSet ds2 = dsAds.Clone();
    //        ds2.Tables[0].Rows.Clear();
    //        DataSet ds3 = dsAds.Clone();
    //        ds3.Tables[0].Rows.Clear();
    //        DataSet ds4 = dsAds.Clone();
    //        ds4.Tables[0].Rows.Clear();
    //        if (dsAds.Tables.Count > 0)
    //        {
    //            if (dsAds.Tables[0].Rows.Count > 0)
    //            {
    //                //get the index for the ad that was last seen + 1;
    //                int startIndex = 0;

    //                DataView dv = new DataView(dsAds.Tables[0], "", "", DataViewRowState.CurrentRows);




    //                if (startID.Trim() != "45" && startID != "")
    //                {
    //                    dv.RowFilter = "Ad_ID=" + startID;
    //                    startIndex = int.Parse(dv[0]["Row"].ToString()) + 1;
    //                    if (startIndex >= dat.numberOfAllAdsInDay)
    //                    {
    //                        startIndex = 0;
    //                    }
    //                }
    //                for (int i = startIndex; i < dsAds.Tables[0].Rows.Count; i = i + 4)
    //                {
    //                    DataRow row = ds1.Tables[0].NewRow();
    //                    row["UserName"] = dsAds.Tables[0].Rows[i]["UserName"];
    //                    row["Ad_ID"] = dsAds.Tables[0].Rows[i]["Ad_ID"].ToString();
    //                    row["FeaturedSummary"] = dat.BreakUpString(dsAds.Tables[0].Rows[i]["FeaturedSummary"].ToString(), 30);
    //                    row["Description"] = dsAds.Tables[0].Rows[i]["Description"].ToString();
    //                    row["Header"] = dsAds.Tables[0].Rows[i]["Header"].ToString();
    //                    row["FeaturedPicture"] = dsAds.Tables[0].Rows[i]["FeaturedPicture"].ToString();
    //                    row["FeaturedPicture2"] = dsAds.Tables[0].Rows[i]["FeaturedPicture2"].ToString();
    //                    row["Row"] = dsAds.Tables[0].Rows[i]["Row"].ToString();
    //                    ds1.Tables[0].Rows.Add(row);

    //                    if (i + 1 <= dsAds.Tables[0].Rows.Count - 1)
    //                    {
    //                        DataRow row1 = ds2.Tables[0].NewRow();
    //                        row1["UserName"] = dsAds.Tables[0].Rows[i + 1]["UserName"];
    //                        row1["Ad_ID"] = dsAds.Tables[0].Rows[i + 1]["Ad_ID"].ToString();
    //                        row1["FeaturedSummary"] = dat.BreakUpString(dsAds.Tables[0].Rows[i + 1]["FeaturedSummary"].ToString(), 30);
    //                        row1["Description"] = dsAds.Tables[0].Rows[i + 1]["Description"].ToString();
    //                        row1["Header"] = dsAds.Tables[0].Rows[i + 1]["Header"].ToString();
    //                        row1["FeaturedPicture"] = dsAds.Tables[0].Rows[i + 1]["FeaturedPicture"].ToString();
    //                        row1["FeaturedPicture2"] = dsAds.Tables[0].Rows[i + 1]["FeaturedPicture2"].ToString();
    //                        row1["Row"] = dsAds.Tables[0].Rows[i + 1]["Row"].ToString();
    //                        ds2.Tables[0].Rows.Add(row1);
    //                    }
    //                    if (i + 2 <= dsAds.Tables[0].Rows.Count - 1)
    //                    {
    //                        DataRow row2 = ds3.Tables[0].NewRow();
    //                        row2["UserName"] = dsAds.Tables[0].Rows[i + 2]["UserName"];
    //                        row2["Ad_ID"] = dsAds.Tables[0].Rows[i + 2]["Ad_ID"].ToString();
    //                        row2["FeaturedSummary"] = dat.BreakUpString(dsAds.Tables[0].Rows[i + 2]["FeaturedSummary"].ToString(), 30);
    //                        row2["Description"] = dsAds.Tables[0].Rows[i + 2]["Description"].ToString();
    //                        row2["Header"] = dsAds.Tables[0].Rows[i + 2]["Header"].ToString();
    //                        row2["FeaturedPicture"] = dsAds.Tables[0].Rows[i + 2]["FeaturedPicture"].ToString();
    //                        row2["FeaturedPicture2"] = dsAds.Tables[0].Rows[i + 2]["FeaturedPicture2"].ToString();
    //                        row2["Row"] = dsAds.Tables[0].Rows[i + 2]["Row"].ToString();
    //                        ds3.Tables[0].Rows.Add(row2);
    //                    }
    //                    if (i + 3 <= dsAds.Tables[0].Rows.Count - 1)
    //                    {
    //                        DataRow row3 = ds4.Tables[0].NewRow();
    //                        row3["UserName"] = dsAds.Tables[0].Rows[i + 3]["UserName"];
    //                        row3["Ad_ID"] = dsAds.Tables[0].Rows[i + 3]["Ad_ID"].ToString();
    //                        row3["FeaturedSummary"] = dat.BreakUpString(dsAds.Tables[0].Rows[i + 3]["FeaturedSummary"].ToString(), 30);
    //                        row3["Description"] = dsAds.Tables[0].Rows[i + 3]["Description"].ToString();
    //                        row3["Header"] = dsAds.Tables[0].Rows[i + 3]["Header"].ToString();
    //                        row3["FeaturedPicture"] = dsAds.Tables[0].Rows[i + 3]["FeaturedPicture"].ToString();
    //                        row3["FeaturedPicture2"] = dsAds.Tables[0].Rows[i + 3]["FeaturedPicture2"].ToString();
    //                        row3["Row"] = dsAds.Tables[0].Rows[i + 3]["Row"].ToString();
    //                        ds4.Tables[0].Rows.Add(row3);
    //                    }
    //                }


    //                if (startIndex != 0)
    //                {
    //                    int numberLeftToAd = startIndex;

    //                    for (int i = 0; i < numberLeftToAd; i = i + 4)
    //                    {
    //                        DataRow row = ds1.Tables[0].NewRow();
    //                        row["UserName"] = dsAds.Tables[0].Rows[i]["UserName"];
    //                        row["Ad_ID"] = dsAds.Tables[0].Rows[i]["Ad_ID"].ToString();
    //                        row["FeaturedSummary"] = dat.BreakUpString(dsAds.Tables[0].Rows[i]["FeaturedSummary"].ToString(), 30);
    //                        row["Description"] = dsAds.Tables[0].Rows[i]["Description"].ToString();
    //                        row["Header"] = dsAds.Tables[0].Rows[i]["Header"].ToString();
    //                        row["FeaturedPicture"] = dsAds.Tables[0].Rows[i]["FeaturedPicture"].ToString();
    //                        row["FeaturedPicture2"] = dsAds.Tables[0].Rows[i]["FeaturedPicture2"].ToString();
    //                        row["Row"] = dsAds.Tables[0].Rows[i]["Row"].ToString();
    //                        ds1.Tables[0].Rows.Add(row);

    //                        if (i + 1 <= dsAds.Tables[0].Rows.Count - 1 && i + 1 < numberLeftToAd)
    //                        {
    //                            DataRow row1 = ds2.Tables[0].NewRow();
    //                            row1["UserName"] = dsAds.Tables[0].Rows[i + 1]["UserName"];
    //                            row1["Ad_ID"] = dsAds.Tables[0].Rows[i + 1]["Ad_ID"].ToString();
    //                            row1["FeaturedSummary"] = dat.BreakUpString(dsAds.Tables[0].Rows[i + 1]["FeaturedSummary"].ToString(), 30);
    //                            row1["Description"] = dsAds.Tables[0].Rows[i + 1]["Description"].ToString();
    //                            row1["Header"] = dsAds.Tables[0].Rows[i + 1]["Header"].ToString();
    //                            row1["FeaturedPicture"] = dsAds.Tables[0].Rows[i + 1]["FeaturedPicture"].ToString();
    //                            row1["FeaturedPicture2"] = dsAds.Tables[0].Rows[i + 1]["FeaturedPicture2"].ToString();
    //                            row1["Row"] = dsAds.Tables[0].Rows[i + 1]["Row"].ToString();
    //                            ds2.Tables[0].Rows.Add(row1);
    //                        }
    //                        if (i + 2 <= dsAds.Tables[0].Rows.Count - 1 && i + 2 < numberLeftToAd)
    //                        {
    //                            DataRow row2 = ds3.Tables[0].NewRow();
    //                            row2["UserName"] = dsAds.Tables[0].Rows[i + 2]["UserName"];
    //                            row2["Ad_ID"] = dsAds.Tables[0].Rows[i + 2]["Ad_ID"].ToString();
    //                            row2["FeaturedSummary"] = dat.BreakUpString(dsAds.Tables[0].Rows[i + 2]["FeaturedSummary"].ToString(), 30);
    //                            row2["Description"] = dsAds.Tables[0].Rows[i + 2]["Description"].ToString();
    //                            row2["Header"] = dsAds.Tables[0].Rows[i + 2]["Header"].ToString();
    //                            row2["FeaturedPicture"] = dsAds.Tables[0].Rows[i + 2]["FeaturedPicture"].ToString();
    //                            row2["FeaturedPicture2"] = dsAds.Tables[0].Rows[i + 2]["FeaturedPicture2"].ToString();
    //                            row2["Row"] = dsAds.Tables[0].Rows[i + 2]["Row"].ToString();
    //                            ds3.Tables[0].Rows.Add(row2);
    //                        }
    //                        if (i + 3 <= dsAds.Tables[0].Rows.Count - 1 && i + 3 < numberLeftToAd)
    //                        {
    //                            DataRow row3 = ds4.Tables[0].NewRow();
    //                            row3["UserName"] = dsAds.Tables[0].Rows[i + 3]["UserName"];
    //                            row3["Ad_ID"] = dsAds.Tables[0].Rows[i + 3]["Ad_ID"].ToString();
    //                            row3["FeaturedSummary"] = dat.BreakUpString(dsAds.Tables[0].Rows[i + 3]["FeaturedSummary"].ToString(), 30);
    //                            row3["Description"] = dsAds.Tables[0].Rows[i + 3]["Description"].ToString();
    //                            row3["Header"] = dsAds.Tables[0].Rows[i + 3]["Header"].ToString();
    //                            row3["FeaturedPicture"] = dsAds.Tables[0].Rows[i + 3]["FeaturedPicture"].ToString();
    //                            row3["FeaturedPicture2"] = dsAds.Tables[0].Rows[i + 3]["FeaturedPicture2"].ToString();
    //                            row3["Row"] = dsAds.Tables[0].Rows[i + 3]["Row"].ToString();
    //                            ds4.Tables[0].Rows.Add(row3);
    //                        }
    //                    }
    //                }


    //                dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //                //ds = dat.GetData("SELECT * FROM Ads WHERE Featured='True'");

    //                //MainAdLiteral.Text = "<a href=\"Ad.aspx?AdID=" + dsAds.Tables[0].Rows[0]["Ad_ID"].ToString() + "\"> <img style=\"float: left;padding-left:8px; padding-top: 8px; padding-right: 6px; padding-bottom: 3px; border: 0; \" height=\"190px\" width=\"212px\" src=\"UserFiles/" + dsAds.Tables[0].Rows[0]["UserName"].ToString() + "/" + dsAds.Tables[0].Rows[0]["FeaturedPicture2"].ToString() + "\" /></a>";
    //                //ReadMoreLink.NavigateUrl = "../Ad.aspx?AdID=" + dsAds.Tables[0].Rows[0]["Ad_ID"].ToString();
    //                //string finalTitle = dsAds.Tables[0].Rows[0]["Header"].ToString();
    //                //if (finalTitle.Length > 50)
    //                //    finalTitle = finalTitle.Substring(0, 50) + "...";
    //                //MainAdTitleLabel.Text = finalTitle;
    //                //MainAdTitleLabel.NavigateUrl = "../Ad.aspx?AdID=" + dsAds.Tables[0].Rows[0]["Ad_ID"].ToString();
    //                //string finalBody = dsAds.Tables[0].Rows[0]["FeaturedSummary"].ToString();
    //                //if (finalBody.Length > 200)
    //                //    finalBody = finalBody.Substring(0, 200) + "...";
    //                //string temp = dat.BreakUpString(finalBody, 20);
    //                //int count = 0;
    //                //string temp2 = "";

    //                ////Don't allow more than 7 lines.
    //                //if (temp.Contains("<br/>"))
    //                //{
    //                //    bool falze = false;
    //                //    while (!falze)
    //                //    {
    //                //        int p = temp.IndexOf("<br/>");
    //                //        if (p != -1)
    //                //        {
    //                //            if (count < 6)
    //                //            {
    //                //                temp2 += temp.Substring(0, p + 5);
    //                //                temp = temp.Substring(p + 5);
    //                //                count++;
    //                //            }
    //                //            else
    //                //                falze = true;
    //                //        }
    //                //        else
    //                //            falze = true;
    //                //    }
    //                //}
    //                //else
    //                //    temp2 = temp;

    //                //MainAdBodyLabel.Text = temp2;
    //            }
    //        }
    //        //}
    //        //else
    //        //{


    //        //}

    //        if (Session["User"] != null)
    //        {
    //            Session["Ad1_" + Session["User"].ToString()] = ds1;
    //            //if(Session["Ad1_Count" + Session["User"].ToString()] == null)
    //            Session["Ad1_Count" + Session["User"].ToString()] = "0";

    //            Session["Ad2_" + Session["User"].ToString()] = ds2;
    //            //if(Session["Ad2_Count" + Session["User"].ToString()] == null)
    //            Session["Ad2_Count" + Session["User"].ToString()] = "0";

    //            Session["Ad3_" + Session["User"].ToString()] = ds3;
    //            //if(Session["Ad3_Count" + Session["User"].ToString()] == null)
    //            Session["Ad3_Count" + Session["User"].ToString()] = "0";

    //            Session["Ad4_" + Session["User"].ToString()] = ds4;
    //            //if(Session["Ad4_Count" + Session["User"].ToString()] == null)
    //            Session["Ad4_Count" + Session["User"].ToString()] = "0";
    //        }
    //        else
    //        {
    //            Session["Ad1_Generic"] = ds1;
    //            //if(Session["Ad1_Count_Generic"] == null)
    //            Session["Ad1_Count_Generic"] = "0";

    //            Session["Ad2_Generic"] = ds2;
    //            //if (Session["Ad2_Count_Generic"] == null)
    //            Session["Ad2_Count_Generic"] = "0";

    //            Session["Ad3_Generic"] = ds3;
    //            //if (Session["Ad3_Count_Generic"] == null)
    //            Session["Ad3_Count_Generic"] = "0";

    //            Session["Ad4_Generic"] = ds4;
    //            //if (Session["Ad4_Count_Generic"] == null)
    //            Session["Ad4_Count_Generic"] = "0";
    //        }

    //        //if user has bandwidth for more ads, get the ads that have 'DisplayToAll' set
    //        DataSet dsHippAd = dat.RetrieveAdminAd();

    //        if (ds1.Tables[0].Rows.Count < dat.numberOfAdsInADay)
    //        {
    //            int numCount = dat.numberOfAdsInADay - ds1.Tables[0].Rows.Count;
    //            for (int i = 0; i < numCount; i++)
    //            {
    //                DataRow row = ds1.Tables[0].NewRow();
    //                row["UserName"] = dsHippAd.Tables[0].Rows[0]["UserName"];
    //                row["Ad_ID"] = dsHippAd.Tables[0].Rows[0]["Ad_ID"].ToString();
    //                row["FeaturedSummary"] = dsHippAd.Tables[0].Rows[0]["FeaturedSummary"].ToString();
    //                row["Description"] = dsHippAd.Tables[0].Rows[0]["Description"].ToString();
    //                row["Header"] = dsHippAd.Tables[0].Rows[0]["Header"].ToString();
    //                row["FeaturedPicture"] = dsHippAd.Tables[0].Rows[0]["FeaturedPicture"].ToString();
    //                row["FeaturedPicture2"] = dsHippAd.Tables[0].Rows[0]["FeaturedPicture2"].ToString();
    //                row["Row"] = dsHippAd.Tables[0].Rows[0]["Row"].ToString();
    //                ds1.Tables[0].Rows.Add(row);
    //            }
    //        }
    //        if (ds2.Tables[0].Rows.Count < dat.numberOfAdsInADay)
    //        {
    //            int numCount = dat.numberOfAdsInADay - ds2.Tables[0].Rows.Count;
    //            for (int i = 0; i < numCount; i++)
    //            {
    //                DataRow row = ds2.Tables[0].NewRow();
    //                row["UserName"] = dsHippAd.Tables[0].Rows[0]["UserName"];
    //                row["Ad_ID"] = dsHippAd.Tables[0].Rows[0]["Ad_ID"].ToString();
    //                row["FeaturedSummary"] = dsHippAd.Tables[0].Rows[0]["FeaturedSummary"].ToString();
    //                row["Description"] = dsHippAd.Tables[0].Rows[0]["Description"].ToString();
    //                row["Header"] = dsHippAd.Tables[0].Rows[0]["Header"].ToString();
    //                row["FeaturedPicture"] = dsHippAd.Tables[0].Rows[0]["FeaturedPicture"].ToString();
    //                row["FeaturedPicture2"] = dsHippAd.Tables[0].Rows[0]["FeaturedPicture2"].ToString();
    //                row["Row"] = dsHippAd.Tables[0].Rows[0]["Row"].ToString();
    //                ds2.Tables[0].Rows.Add(row);
    //            }
    //        }

    //        if (ds3.Tables[0].Rows.Count < dat.numberOfAdsInADay)
    //        {
    //            int numCount = dat.numberOfAdsInADay - ds3.Tables[0].Rows.Count;
    //            for (int i = 0; i < numCount; i++)
    //            {
    //                DataRow row = ds3.Tables[0].NewRow();
    //                row["UserName"] = dsHippAd.Tables[0].Rows[0]["UserName"];
    //                row["Ad_ID"] = dsHippAd.Tables[0].Rows[0]["Ad_ID"].ToString();
    //                row["FeaturedSummary"] = dsHippAd.Tables[0].Rows[0]["FeaturedSummary"].ToString();
    //                row["Description"] = dsHippAd.Tables[0].Rows[0]["Description"].ToString();
    //                row["Header"] = dsHippAd.Tables[0].Rows[0]["Header"].ToString();
    //                row["FeaturedPicture"] = dsHippAd.Tables[0].Rows[0]["FeaturedPicture"].ToString();
    //                row["FeaturedPicture2"] = dsHippAd.Tables[0].Rows[0]["FeaturedPicture2"].ToString();
    //                row["Row"] = dsHippAd.Tables[0].Rows[0]["Row"].ToString();
    //                ds3.Tables[0].Rows.Add(row);
    //            }
    //        }

    //        if (ds4.Tables[0].Rows.Count < dat.numberOfAdsInADay)
    //        {
    //            int numCount = dat.numberOfAdsInADay - ds4.Tables[0].Rows.Count;
    //            for (int i = 0; i < numCount; i++)
    //            {
    //                DataRow row = ds4.Tables[0].NewRow();
    //                row["UserName"] = dsHippAd.Tables[0].Rows[0]["UserName"];
    //                row["Ad_ID"] = dsHippAd.Tables[0].Rows[0]["Ad_ID"].ToString();
    //                row["FeaturedSummary"] = dsHippAd.Tables[0].Rows[0]["FeaturedSummary"].ToString();
    //                row["Description"] = dsHippAd.Tables[0].Rows[0]["Description"].ToString();
    //                row["Header"] = dsHippAd.Tables[0].Rows[0]["Header"].ToString();
    //                row["FeaturedPicture"] = dsHippAd.Tables[0].Rows[0]["FeaturedPicture"].ToString();
    //                row["FeaturedPicture2"] = dsHippAd.Tables[0].Rows[0]["FeaturedPicture2"].ToString();
    //                row["Row"] = dsHippAd.Tables[0].Rows[0]["Row"].ToString();
    //                ds4.Tables[0].Rows.Add(row);
    //            }
    //        }

    //        //DataView dv1 = new DataView(ds1.Tables[0], " Row < " + dat.numberOfAdsInADay, "", DataViewRowState.CurrentRows);
    //        //DataView dv2 = new DataView(ds2.Tables[0], " Row < " + dat.numberOfAdsInADay, "", DataViewRowState.CurrentRows);
    //        //DataView dv3 = new DataView(ds3.Tables[0], " Row < " + dat.numberOfAdsInADay, "", DataViewRowState.CurrentRows);
    //        //DataView dv4 = new DataView(ds4.Tables[0], " Row < " + dat.numberOfAdsInADay, "", DataViewRowState.CurrentRows);

    //        //Write out the schema that this DataSet created,    
    //        //use the WriteXmlSchema method of the DataSet class   
    //        //ds1.WriteXmlSchema(Server.MapPath("\\UserFiles\\categories1.xsd"));


    //        // To write out the contents of the DataSet as XML,    
    //        //use a file name to call the WriteXml method of the DataSet class   
    //        ds1.WriteXml(Server.MapPath(".") + "\\UserFiles\\categoies1.xml", XmlWriteMode.IgnoreSchema);
    //        ds2.WriteXml(Server.MapPath(".") + "\\UserFiles\\categoies2.xml", XmlWriteMode.IgnoreSchema);
    //        ds3.WriteXml(Server.MapPath(".") + "\\UserFiles\\categoies3.xml", XmlWriteMode.IgnoreSchema);
    //        ds4.WriteXml(Server.MapPath(".") + "\\UserFiles\\categoies4.xml", XmlWriteMode.IgnoreSchema);

    //        //Ad1.DataBind2();
    //        //Ad2.DataBind2();
    //        //Ad3.DataBind2();
    //        //Ad4.DataBind2();
    //        Session["AdsLoaded"] = "true";
    //        //}
    //        //else
    //        //{
    //        //    if (Session["User"] != null)
    //        //    {
    //        //        Ad1.DS_ADS = (DataSet)Session["Ad1_" + Session["User"].ToString()];

    //        //        Ad2.DS_ADS = (DataSet)Session["Ad2_" + Session["User"].ToString()];

    //        //        Ad3.DS_ADS = (DataSet)Session["Ad3_" + Session["User"].ToString()];

    //        //        Ad4.DS_ADS = (DataSet)Session["Ad4_" + Session["User"].ToString()];
    //        //    }
    //        //    else
    //        //    {
    //        //        Ad1.DS_ADS = (DataSet)Session["Ad1_Generic"];

    //        //        Ad2.DS_ADS = (DataSet)Session["Ad2_Generic"];

    //        //        Ad3.DS_ADS = (DataSet)Session["Ad3_Generic"];

    //        //        Ad4.DS_ADS = (DataSet)Session["Ad4_Generic"];

    //        //        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //        //        DataSet ad1ds = (DataSet)Session["Ad1_Generic"];
    //        //        //ds = dat.GetData("SELECT * FROM Ads WHERE Featured='True'");

    //        //        MainAdLiteral.Text = "<a href=\"Ad.aspx?AdID=" + ad1ds.Tables[0].Rows[0]["Ad_ID"].ToString() + "\"> <img style=\"float: left;padding-left:8px; padding-top: 8px; padding-right: 6px; padding-bottom: 3px; border: 0; \" height=\"190px\" width=\"212px\" src=\"UserFiles/" + dsAds.Tables[0].Rows[0]["UserName"].ToString() + "/" + dsAds.Tables[0].Rows[0]["FeaturedPicture2"].ToString() + "\" /></a>";
    //        //        ReadMoreLink.NavigateUrl = "../Ad.aspx?AdID=" + ad1ds.Tables[0].Rows[0]["Ad_ID"].ToString();
    //        //        string finalTitle = ad1ds.Tables[0].Rows[0]["Header"].ToString();
    //        //        if (finalTitle.Length > 50)
    //        //            finalTitle = finalTitle.Substring(0, 50) + "...";
    //        //        MainAdTitleLabel.Text = finalTitle;
    //        //        MainAdTitleLabel.NavigateUrl = "../Ad.aspx?AdID=" + ad1ds.Tables[0].Rows[0]["Ad_ID"].ToString();
    //        //        string finalBody = ad1ds.Tables[0].Rows[0]["FeaturedSummary"].ToString();
    //        //        if (finalBody.Length > 200)
    //        //            finalBody = finalBody.Substring(0, 200) + "...";
    //        //        string temp = dat.BreakUpString(finalBody, 20);
    //        //        int count = 0;
    //        //        string temp2 = "";

    //        //        //Don't allow more than 7 lines.
    //        //        if (temp.Contains("<br/>"))
    //        //        {
    //        //            bool falze = false;
    //        //            while (!falze)
    //        //            {
    //        //                int p = temp.IndexOf("<br/>");
    //        //                if (p != -1)
    //        //                {
    //        //                    if (count < 6)
    //        //                    {
    //        //                        temp2 += temp.Substring(0, p + 5);
    //        //                        temp = temp.Substring(p + 5);
    //        //                        count++;
    //        //                    }
    //        //                    else
    //        //                        falze = true;
    //        //                }
    //        //                else
    //        //                    falze = true;
    //        //            }
    //        //        }
    //        //        else
    //        //            temp2 = temp;

    //        //        MainAdBodyLabel.Text = temp2;
    //        //    }
    //        //}
    //    }
    //}

    //protected void GetMainAds()
    //{
    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

    //    string startID = "";
    //    int startIndex = 0;
    //    if (Session["User"] != null)
    //    {
    //        if (Session[Session["User"].ToString() + "_Main_lastseen"] != null)
    //        {
    //            startID = Session[Session["User"].ToString() + "_Main_lastseen"].ToString();
    //        }
    //    }
    //    else if (Session["AdMain_Count_Generic"] != null)
    //    {
    //        startIndex = int.Parse(Session["AdMain_Count_Generic"].ToString());
    //    }
    //    //else if (Session["GenericUser_Main_lastseen"] != null)
    //    //{
    //    //    startID = Session["GenericUser_Main_lastseen"].ToString();
    //    //}

    //    DataSet dsMain = new DataSet();
    //    if (dsMainAds != null)
    //    {
    //        if (dsMainAds.Tables.Count > 0)
    //        {
    //            if (dsMainAds.Tables[0].Rows.Count > 0)
    //            {
    //                //get the index for the ad that was last seen + 1;
                    
    //                dsMain = dsMainAds.Clone();
    //                dsMain.Tables[0].Rows.Clear();

    //                DataView dv = new DataView(dsMainAds.Tables[0], "", "", DataViewRowState.CurrentRows);

    //                if (startIndex >= dsMainAds.Tables[0].Rows.Count)
    //                    startIndex = 0;
    //                //if (startID.Trim() != "45" && startID != "")
    //                //{
    //                //    dv.RowFilter = "Ad_ID=" + startID;
    //                //    startIndex = int.Parse(dv[0]["Row"].ToString()) + 1;
    //                //    if (startIndex >= dat.numberOfMainAdsInADay)
    //                //        startIndex = 0;

    //                //}
    //                for (int i = startIndex; i < dsMainAds.Tables[0].Rows.Count; i++)
    //                {
    //                    DataRow row = dsMain.Tables[0].NewRow();
    //                    row["UserName"] = dsMainAds.Tables[0].Rows[i]["UserName"];
    //                    row["Ad_ID"] = dsMainAds.Tables[0].Rows[i]["Ad_ID"].ToString();
    //                    row["FeaturedSummary"] = dat.BreakUpString(dsMainAds.Tables[0].Rows[i]["FeaturedSummary"].ToString(), 30);
    //                    row["Description"] = dsMainAds.Tables[0].Rows[i]["Description"].ToString();
    //                    row["Header"] = dsMainAds.Tables[0].Rows[i]["Header"].ToString();
    //                    row["FeaturedPicture"] = dsMainAds.Tables[0].Rows[i]["FeaturedPicture"].ToString();
    //                    row["FeaturedPicture2"] = dsMainAds.Tables[0].Rows[i]["FeaturedPicture2"].ToString();
    //                    row["Row"] = dsMainAds.Tables[0].Rows[i]["Row"].ToString();
    //                    dsMain.Tables[0].Rows.Add(row);

    //                }


    //                if (startIndex != 0)
    //                {
    //                    int numberLeftToAd = startIndex;

    //                    for (int i = 0; i < numberLeftToAd; i++)
    //                    {
    //                        DataRow row = dsMain.Tables[0].NewRow();
    //                        row["UserName"] = dsMainAds.Tables[0].Rows[i]["UserName"];
    //                        row["Ad_ID"] = dsMainAds.Tables[0].Rows[i]["Ad_ID"].ToString();
    //                        row["FeaturedSummary"] = dat.BreakUpString(dsMainAds.Tables[0].Rows[i]["FeaturedSummary"].ToString(), 30);
    //                        row["Description"] = dsMainAds.Tables[0].Rows[i]["Description"].ToString();
    //                        row["Header"] = dsMainAds.Tables[0].Rows[i]["Header"].ToString();
    //                        row["FeaturedPicture"] = dsMainAds.Tables[0].Rows[i]["FeaturedPicture"].ToString();
    //                        row["FeaturedPicture2"] = dsMainAds.Tables[0].Rows[i]["FeaturedPicture2"].ToString();
    //                        row["Row"] = dsMainAds.Tables[0].Rows[i]["Row"].ToString();
    //                        dsMain.Tables[0].Rows.Add(row);

    //                    }
    //                }


    //                dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //            }
    //        }

    //        if (Session["User"] != null)
    //        {
    //            Session["AdMain_" + Session["User"].ToString()] = dsMain;
    //            //if(Session["Ad1_Count" + Session["User"].ToString()] == null)
    //            Session["AdMain_Count" + Session["User"].ToString()] = "0";
    //        }
    //        else
    //        {
    //            Session["AdMain_Generic"] = dsMain;
    //            //if(Session["Ad1_Count_Generic"] == null)
    //            Session["AdMain_Count_Generic"] = "0";
    //        }

    //        //if user has bandwidth for more ads, get the ads that have 'DisplayToAll' set
    //        DataSet dsHippAd = dat.RetrieveAdminAd();

    //        if (dsMain.Tables.Count > 0)
    //        {
    //            if (dsMain.Tables[0].Rows.Count < dat.numberOfMainAdsInADay)
    //            {
    //                int numCount = dat.numberOfMainAdsInADay - dsMain.Tables[0].Rows.Count;
    //                for (int i = 0; i < numCount; i++)
    //                {
    //                    DataRow row = dsMain.Tables[0].NewRow();
    //                    row["UserName"] = dsHippAd.Tables[0].Rows[0]["UserName"];
    //                    row["Ad_ID"] = dsHippAd.Tables[0].Rows[0]["Ad_ID"].ToString();
    //                    row["FeaturedSummary"] = dsHippAd.Tables[0].Rows[0]["FeaturedSummary"].ToString();
    //                    row["Description"] = dsHippAd.Tables[0].Rows[0]["Description"].ToString();
    //                    row["Header"] = dsHippAd.Tables[0].Rows[0]["Header"].ToString();
    //                    row["FeaturedPicture"] = dsHippAd.Tables[0].Rows[0]["FeaturedPicture"].ToString();
    //                    row["FeaturedPicture2"] = dsHippAd.Tables[0].Rows[0]["FeaturedPicture2"].ToString();
    //                    row["Row"] = dsHippAd.Tables[0].Rows[0]["Row"].ToString();
    //                    dsMain.Tables[0].Rows.Add(row);
    //                }
    //            }
    //        }
    //        else
    //        {
    //            dsMain = dsHippAd.Clone();
    //            dsMain.Tables[0].Rows.Clear();
    //            for (int i = 0; i < dat.numberOfMainAdsInADay; i++)
    //            {
    //                DataRow row = dsMain.Tables[0].NewRow();
    //                row["UserName"] = dsHippAd.Tables[0].Rows[0]["UserName"];
    //                row["Ad_ID"] = dsHippAd.Tables[0].Rows[0]["Ad_ID"].ToString();
    //                row["FeaturedSummary"] = dsHippAd.Tables[0].Rows[0]["FeaturedSummary"].ToString();
    //                row["Description"] = dsHippAd.Tables[0].Rows[0]["Description"].ToString();
    //                row["Header"] = dsHippAd.Tables[0].Rows[0]["Header"].ToString();
    //                row["FeaturedPicture"] = dsHippAd.Tables[0].Rows[0]["FeaturedPicture"].ToString();
    //                row["FeaturedPicture2"] = dsHippAd.Tables[0].Rows[0]["FeaturedPicture2"].ToString();
    //                row["Row"] = dsHippAd.Tables[0].Rows[0]["Row"].ToString();
    //                dsMain.Tables[0].Rows.Add(row);
    //            }
    //        }

    //        //DataView dvMain = new DataView(dsMain.Tables[0], " Row < " + dat.numberOfMainAdsInADay, 
    //        //    "", DataViewRowState.CurrentRows);
    //        //RadRotator1.DataSource = dvMain;
    //        dsMain.WriteXml(Server.MapPath(".") + "\\UserFiles\\categoiesMain.xml", XmlWriteMode.IgnoreSchema);

    //        XmlDataSource xmlAds = new XmlDataSource();
    //        xmlAds.DataFile = Server.MapPath(".") + "\\UserFiles\\categoiesMain.xml";
    //        xmlAds.DataBind();

    //        RadRotator1.DataSource = xmlAds;
    //        RadRotator1.DataBind();


    //        Session["MainAdLoaded"] = "true";
    //    }

    //}

    //[Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
    //public int SendData(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")))
    //{

        
    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //    return dat.PerformMainAdCount(false);
      
    //}
}
