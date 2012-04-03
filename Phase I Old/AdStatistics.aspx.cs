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
using Telerik.Web.UI;
using Telerik.Charting;
using Telerik.Charting.Design;

public partial class AdStatistics : Telerik.Web.UI.RadAjaxPage
{
    private void Page_Load(object sender, EventArgs e)
    {
        

        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        if (Session["User"] == null)
        {
            Session["RedirectTo"] = "AdStatistics.aspx";
            if (Request.QueryString["Ad"] != null)
                Session["RedirectTo"] = "AdStatistics.aspx?Ad=" + Request.QueryString["Ad"].ToString();
            Response.Redirect("UserLogin.aspx");

        }
        else
        {
            if (Request.QueryString["Ad"] != null)
            {
                DataView dv = dat.GetDataDV("SELECT * FROM Ads WHERE Ad_ID=" + Request.QueryString["Ad"].ToString());
                Session["RedirectTo"] = "AdStatistics.aspx?Ad=" + Request.QueryString["Ad"].ToString();

                if(dv[0]["User_ID"].ToString() != Session["User"].ToString())
                    Response.Redirect("UserLogin.aspx");
            }
        }

        

        //Fill ad dropdown
        if (!IsPostBack)
        {
            DataView dvAds = dat.GetDataDV("SELECT * FROM Ads WHERE User_ID=" + Session["User"].ToString());

            AdDropDown.DataSource = dvAds;
            AdDropDown.DataTextField = "Header";
            AdDropDown.DataValueField = "Ad_ID";
            AdDropDown.DataBind();

            AdDropDown.Items.Insert(0, new ListItem("Select One...", "-1"));
        }
        else
        {
            ConsumerRadChart.Series.Clear();
            ConsumerRadChart.Clear();
            ConsumerRadChart.PlotArea.Chart.Series.Clear();
            ConsumerRadChart.PlotArea.XAxis.Clear();
            ConsumerRadChart.PlotArea.YAxis.Clear();
        }

        if (!IsPostBack)
        {
            if (Request.QueryString["Ad"] != null)
            {
                AdDropDown.Items.FindByValue(Request.QueryString["Ad"].Trim().ToString()).Selected = true;
                GetChart();
            }
        }

    }

    protected void GetAdChart(object sender, EventArgs e)
    {
        if (AdDropDown.SelectedValue != "-1")
            GetChart();
        else
        {
            ConsumerRadChart.Clear();
            ChartPanel.Visible = false;
        }
    }

    protected void GetChart()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        ConsumerRadChart.Series.Clear();
        ConsumerRadChart.Clear();
        ConsumerRadChart.PlotArea.Chart.Series.Clear();
        ConsumerRadChart.PlotArea.XAxis.Clear();
        ConsumerRadChart.PlotArea.YAxis.Clear();

        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        string adID = AdDropDown.SelectedItem.Value;
        DataView dvAd = dat.GetDataDV("SELECT * FROM Ads WHERE Ad_ID=" + adID);
        DataView dvAdCategories = dat.GetDataDV("SELECT DISTINCT  AC.Name, ACM.CategoryID FROM "+
            "Ad_Category_Mapping ACM, AdCategories AC WHERE AC.ID=ACM.CategoryID AND ACM.AdID=" + adID);
        TitleLabel.Text = "<span style=\"font-size: 30px;color: white;\">Statistics For Ad: <a href=\"" +
            "http://" + Request.Url.Authority + "/" +
                    dat.MakeNiceName(dat.BreakUpString(dvAd[0]["Header"].ToString(), 14)) +
                    "_" + adID + "_Ad\" style=\"color: #1fb6e7; text-decoration: none;\">" +
            dvAd[0]["Header"].ToString() + "</a></span><hr>";

        TitleLabel.Text += "<table cellspacing=\"20px\"><tr valign=\"top\"><td>";

        if (bool.Parse(dvAd[0]["BigAd"].ToString()))
            TitleLabel.Text += "<br/><b>Ad Type: </b><span style=\"color: #1fb6e7; font-weight: bold;\">Big Ad</span>";
        else
            TitleLabel.Text += "<br/><b>Ad Type: </b><span style=\"color: #1fb6e7; font-weight: bold;\">Normal Ad</span>";

        int totalViews = int.Parse(dvAd[0]["NumViews"].ToString());
        int currentViews = int.Parse(dvAd[0]["NumCurrentViews"].ToString());

        DataView dvAdStatistics = dat.GetDataDV("SELECT CASE WHEN UserID IS NULL THEN IP "+
            "ELSE CONVERT(NVARCHAR,UserID) END AS theID, Reason, LocationOnly, Date, WasEmail "+
            "FROM AdStatistics WHERE AdID=" + adID + " ORDER BY [Date] DESC");
        DataView dvAdStatisticsCovert = dat.GetDataDV("SELECT CASE WHEN LocationOnly = 'True' THEN 'Location Only' " +
            "WHEN WasEmail = 'True' THEN 'Email View' WHEN Reason = 'NonAssigned' THEN 'Non-Assigned User' " +
            "ELSE dbo.GetCategories(Reason) END AS 'Reason', CASE WHEN LocationOnly = 'True' THEN 'Location Only' " +
            "WHEN WasEmail = 'True' THEN 'Email View' WHEN Reason = 'NonAssigned' THEN 'Non-Assigned User' " +
            "ELSE dbo.GetCategories(Reason) END AS 'Reason For View [Location Only, " +
            "Non-Assigned User or list of Categories]', Date " +
            "FROM AdStatistics WHERE AdID=" + adID + " GROUP BY [Date], UserID, IP, LocationOnly, WasEmail, Reason ORDER BY [Date] DESC");

        
        string lastDayDay = "";
        if (dvAdStatistics.Count > 0)
            lastDayDay = DateTime.Parse(dvAdStatistics[0]["Date"].ToString()).Date.ToShortDateString();

        dvAdStatistics.RowFilter = "WasEmail = 1";
        int wasEmailCount = dvAdStatistics.Count;
        int notEmailCount = currentViews - wasEmailCount;
        dvAdStatistics.RowFilter = "";
        if (currentViews < totalViews)
        {
            TitleLabel.Text += "<br/><b>Status: </b><span style=\"color: #1fb6e7; font-weight: bold;\">Ad is still running</span>";
            TitleLabel.Text += "<br/><b>Number of Views Needed: </b><span style=\"color: #1fb6e7; font-weight: bold;\">" + totalViews.ToString() + "</span>";
            TitleLabel.Text += "</td><td>";
            TitleLabel.Text += "<br/><b>Number of Current Views: </b><span style=\"color: #1fb6e7; font-weight: bold;\">" + currentViews.ToString() + "</span>";
            TitleLabel.Text += "<br/><b>Views by Email: </b><span style=\"color: #1fb6e7; font-weight: bold;\">" + wasEmailCount.ToString() + "</span>";
            TitleLabel.Text += "<br/><b>Views on Site: </b><span style=\"color: #1fb6e7; font-weight: bold;\">" + notEmailCount.ToString() + "</span>";
            TitleLabel.Text += "</td><td>";
            TitleLabel.Text += "<br/><b>Display To All: </b><span style=\"color: #1fb6e7; font-weight: bold;\">" + dvAd[0]["DisplayToAll"].ToString() + "</span>";
            TitleLabel.Text += "<br/><b>Display To Non-Users: </b><span style=\"color: #1fb6e7; font-weight: bold;\">" + dvAd[0]["NonUsersAllowed"].ToString() + "</span>";
            TitleLabel.Text += "</td></tr></table>";
        }
        else
        {
            TitleLabel.Text += "<br/><b>Ad has finished running on:</b> <span style=\"color: #1fb6e7; font-weight: bold;\">" + lastDayDay + "</span>";
            TitleLabel.Text += "</td><td>";
            TitleLabel.Text += "<br/><b>Views by Email: </b><span style=\"color: #1fb6e7; font-weight: bold;\">" + wasEmailCount.ToString() + "</span>";
            TitleLabel.Text += "<br/><b>Views on Site: </b><span style=\"color: #1fb6e7; font-weight: bold;\">" + notEmailCount.ToString() + "</span>";
            TitleLabel.Text += "</td><td>";
            TitleLabel.Text += "<br/><b>Display To All: </b><span style=\"color: #1fb6e7; font-weight: bold;\">" + dvAd[0]["DisplayToAll"].ToString() + "</span>";
            TitleLabel.Text += "<br/><b>Display To Non-Users: </b><span style=\"color: #1fb6e7; font-weight: bold;\">" + dvAd[0]["NonUsersAllowed"].ToString() + "</span>";
            TitleLabel.Text += "</td></tr></table>";
        }

        //Construct the chart
        //for each entry in AdStatistics table for this particular ad
            //get the number of different categories it was in
                //One of the categories will be the location
            //increment each category if exists in AdStatistics entry
            //each category will have it's own series


        ArrayList listOfCategories = new ArrayList(dvAdCategories.Count);
        Hashtable hashOfCategories = new Hashtable();

        char[] delim = { ';' };
        string[] tokens;

        //Get the hash of the count of all the categories of the reason why the ad was seen.
        Literal ListofUsersLiteral = new Literal();
        //ListofUsersLiteral.Text = "<table cellpadding=\"10px\" style=\"font-size: 12px;border: solid 1px #1fb6e7; background-color: #1b1b1b;\"><tr><td>" +
        //    "<span style=\"font-size: 14px; font-weight: bold;\"></span>"+
        //    "</td><td><span style=\"font-size: 14px; font-weight: bold;\">";
        
        //LinkButton linkButt = new LinkButton();
        //linkButt.Text = "Reason For View [Location Only, Non-Assigned User or list of Categories]";
        //linkButt.Click += new EventHandler(SortReason);
        //PanelofUsers.Controls.Add(linkButt);

        //ListofUsersLiteral = new Literal();
        //ListofUsersLiteral.Text = "</span>" +
        //    "</td><td><span style=\"font-size: 14px; font-weight: bold;\">";
        //PanelofUsers.Controls.Add(ListofUsersLiteral);

        //linkButt = new LinkButton();
        //linkButt.Text = "Date";
        //linkButt.Click += new EventHandler(SortDate);
        //PanelofUsers.Controls.Add(linkButt);

        //ListofUsersLiteral = new Literal();
        //ListofUsersLiteral.Text = "</span></td></tr>";
        //PanelofUsers.Controls.Add(ListofUsersLiteral);

        for (int i = 0; i < dvAdStatisticsCovert.Count; i++)
        {
            ListofUsersLiteral.Text += "<tr><td>" + (i + 1).ToString() + "</td><td>";
            //ListofUsersLiteral.Text += dvAdStatisticsCovert[i]["Reason"].ToString();
            if (bool.Parse(dvAdStatistics[i]["LocationOnly"].ToString()))
            {
                ListofUsersLiteral.Text += "Location Only";
                if (hashOfCategories.ContainsKey("-1"))
                {
                    hashOfCategories["-1"] = int.Parse(hashOfCategories["-1"].ToString()) + 1;
                }
                else
                {
                    hashOfCategories["-1"] = 1;
                }
            }
            else
            {
                if (dvAdStatistics[i]["Reason"].ToString().Trim() != "NonAssigned")
                {
                    tokens = dvAdStatistics[i]["Reason"].ToString().Split(delim);

                    for (int j = 0; j < tokens.Length; j++)
                    {
                        if (tokens[j].Trim() != "")
                        {
                            dvAdCategories.RowFilter = "CategoryID = " + tokens[j];
                            if (dvAdCategories.Count > 0)
                                ListofUsersLiteral.Text += dvAdCategories[0]["Name"].ToString() + ", ";
                            if (hashOfCategories.ContainsKey(tokens[j].Trim()))
                            {
                                hashOfCategories[tokens[j].Trim()] =
                                    int.Parse(hashOfCategories[tokens[j].Trim()].ToString()) + 1;
                            }
                            else
                            {
                                hashOfCategories.Add(tokens[j].Trim(), 1);
                            }
                        }
                    }
                    if (ListofUsersLiteral.Text.Substring(ListofUsersLiteral.Text.Length - 2, 2) == ", ")
                        ListofUsersLiteral.Text = ListofUsersLiteral.Text.Substring(0, ListofUsersLiteral.Text.Length - 2);
                }
                else
                {
                    ListofUsersLiteral.Text += "Non-Assigned User";
                    if (hashOfCategories.ContainsKey("-2"))
                    {
                        hashOfCategories["-2"] = int.Parse(hashOfCategories["-2"].ToString()) + 1;
                    }
                    else
                    {
                        hashOfCategories["-2"] = 1;
                    }
                }
            }
            ListofUsersLiteral.Text += "</td><td>" +
                DateTime.Parse(dvAdStatistics[i]["Date"].ToString()).ToShortDateString() + "</td>"+
                "<td>" + dvAdStatistics[i]["WasEmail"].ToString() + "</td>" +
                "</tr>";
        }

        dvAdCategories.RowFilter = "";
        //ListofUsersLiteral.Text += "</table>";

        PanelofUsers.Controls.Add(ListofUsersLiteral);

        System.Drawing.Color level4Colora = System.Drawing.Color.FromArgb(51, 51, 51);
        System.Drawing.Color level4Colorb = System.Drawing.Color.FromArgb(134, 175, 200);
        System.Drawing.Color level3Colora = System.Drawing.Color.FromArgb(33, 65, 11);
        System.Drawing.Color level3Colorb = System.Drawing.Color.FromArgb(115, 148, 77);
        System.Drawing.Color level2Colora = System.Drawing.Color.FromArgb(213, 79, 2);
        System.Drawing.Color level2Colorb = System.Drawing.Color.FromArgb(244, 189, 67);
        System.Drawing.Color level1Colora = System.Drawing.Color.FromArgb(192, 140, 8);
        System.Drawing.Color level1Colorb = System.Drawing.Color.FromArgb(227, 201, 70);


        ConsumerRadChart.Chart.Series.Clear();

        ConsumerRadChart.Chart.ChartTitle.TextBlock.Text = "Ad Statistics for '" + dvAd[0]["Header"].ToString() + "'. Date: " + lastDayDay.Trim();
        ConsumerRadChart.Chart.ChartTitle.TextBlock.Appearance.AutoTextWrap = Telerik.Charting.Styles.AutoTextWrap.True;
        
        //Put the UserCount into the DV so that we can easily bind it to the chart
        DataTable dt = dvAdCategories.ToTable();
        DataColumn dc = new DataColumn("UserCount");
        DataColumn dc2 = new DataColumn("XCount");
        dt.Columns.Add(dc);
        dt.Columns.Add(dc2);
        DataRow row;
        int colcount = 0;
        for (int i = 0; i < dvAdCategories.Count; i++)
        {
            if (hashOfCategories.ContainsKey(dvAdCategories[i]["CategoryID"].ToString()))
            {
                dt.Rows[i]["UserCount"] = int.Parse(hashOfCategories[dvAdCategories[i]["CategoryID"].ToString()].ToString());
                dt.Rows[i]["XCount"] = colcount;
                colcount++;
            }
        }
        
        if (hashOfCategories.ContainsKey("-1"))
        {
            row = dt.NewRow();
            row["UserCount"] = hashOfCategories["-1"].ToString();
            row["Name"] = "Location Only";
            row["CategoryID"] = "-1";
            row["XCount"] = colcount;
            dt.Rows.Add(row);
        }

        if (hashOfCategories.ContainsKey("-2"))
        {
            row = dt.NewRow();
            row["UserCount"] = hashOfCategories["-2"].ToString();
            row["Name"] = "Non-Assigned User";
            row["CategoryID"] = "-2";
            row["XCount"] = colcount + 1;
            dt.Rows.Add(row);
        }

        dvAdCategories = new DataView(dt, "", "", DataViewRowState.CurrentRows);

        dvAdCategories.RowFilter = "Isnull(UserCount,'Null Column') <> 'Null Column'";

        ChartSeries salesSeries1;
        int itemsCount = 0;
        ConsumerRadChart.DataGroupColumn = "Name";
        ConsumerRadChart.DataSource = dvAdCategories;

        int maxYCount = 0;
        ConsumerRadChart.PlotArea.XAxis.LayoutMode = Telerik.Charting.Styles.ChartAxisLayoutMode.Between;
        ConsumerRadChart.PlotArea.XAxis.AutoShrink = true;
        ConsumerRadChart.SeriesOrientation = Telerik.Charting.ChartSeriesOrientation.Vertical;
        ConsumerRadChart.PlotArea.XAxis.AxisLabel.Appearance.RotationAngle = 45;
        for (int i = 0; i < dvAdCategories.Count; i++)
        {
            if (hashOfCategories.ContainsKey(dvAdCategories[i]["CategoryID"].ToString()))
            {
                
                //TitleLabel.Text += "<br/><br/>"+i.ToString()+": "+dvAdCategories[i]["Name"].ToString()+", userCount" +
                //dvAdCategories[i]["UserCount"].ToString() + ", XCount: "+dvAdCategories[i]["XCount"].ToString() +
                //", catID: " + dvAdCategories[i]["CategoryID"].ToString();
                
                salesSeries1 = new ChartSeries(dvAdCategories[i]["Name"].ToString(), ChartSeriesType.Bar);
                //salesSeries1.Appearance.LabelAppearance.Visible = false;
                for (int j = 0; j < itemsCount; j++)
                {
                    ChartSeriesItem chartSI = new ChartSeriesItem(true);
                    chartSI.Visible = false;
                    salesSeries1.Items.Add(chartSI);
                }
                salesSeries1.DataYColumn = "UserCount";
                salesSeries1.DataXColumn = "XCount";
                salesSeries1.DataLabelsColumn = "UserCount";
                ConsumerRadChart.AddChartSeries(salesSeries1);
                salesSeries1.Items.Add(new ChartSeriesItem(double.Parse(dvAdCategories[i]["UserCount"].ToString()),
                    dvAdCategories[i]["UserCount"].ToString()));
                salesSeries1.PlotArea.XAxis.AxisLabel.Appearance.RotationAngle = 45;
                ConsumerRadChart.PlotArea.XAxis.AddItem(dvAdCategories[i]["Name"].ToString());
                ConsumerRadChart.PlotArea.XAxis.Items[itemsCount].TextBlock.Text = dvAdCategories[i]["Name"].ToString();
                ConsumerRadChart.PlotArea.XAxis.Items[itemsCount].Appearance.RotationAngle = 45;
                ConsumerRadChart.PlotArea.XAxis.Items[itemsCount].Appearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.Bottom;

                ConsumerRadChart.PlotArea.XAxis.Items[itemsCount].TextBlock.Appearance.Position.Y = float.Parse("-30.00");
                //ConsumerRadChart.PlotArea.XAxis.Items[itemsCount].TextBlock.Appearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.Bottom;
                ConsumerRadChart.PlotArea.XAxis.Items[itemsCount].TextBlock.Appearance.Position.Auto = true;


                
                itemsCount++;

                if (int.Parse(dvAdCategories[i]["UserCount"].ToString()) > maxYCount)
                    maxYCount = int.Parse(dvAdCategories[i]["UserCount"].ToString());
            }
        }

        

        //if (hashOfCategories.ContainsKey("Location Only"))
        //{
        //    ConsumerRadChart.PlotArea.XAxis.AddItem("Location Only");
        //    ConsumerRadChart.PlotArea.XAxis.Items[ConsumerRadChart.PlotArea.XAxis.Items.Count - 1].TextBlock.Text = "Location Only";
        //    salesSeries1 = new ChartSeries("Location Only", ChartSeriesType.Bar);
        //    salesSeries1.Items.Clear();
        //    ChartSeriesItem item = new ChartSeriesItem(int.Parse(hashOfCategories["Location Only"].ToString()),
        //        "Location Only");
        //    item.YValue = itemsCount;
        //    salesSeries1.AddItem(item);
        //    ConsumerRadChart.AddChartSeries(salesSeries1);
        //    ConsumerRadChart.DataBind();
        //    if (int.Parse(hashOfCategories["Location Only"].ToString()) > maxYCount)
        //        maxYCount = int.Parse(hashOfCategories["Location Only"].ToString());
        //}
        //else
        //{
        //    //ChartSeriesItem item = new ChartSeriesItem(0, "Location Only");
        //    //item.XValue = ConsumerRadChart.PlotArea.XAxis.Items.Count;

        //    //salesSeries1.Items.Add(item);
        //}

        ConsumerRadChart.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Categories";
        ConsumerRadChart.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Number of Views";
        ConsumerRadChart.Chart.PlotArea.XAxis.AxisLabel.TextBlock.Text = "Categories";
        ConsumerRadChart.Chart.PlotArea.YAxis.AxisLabel.TextBlock.Text = "Number of Views";
        ConsumerRadChart.Chart.PlotArea.XAxis.AxisLabel.Appearance.Dimensions.Height = 200;
        ConsumerRadChart.Chart.PlotArea.YAxis.LabelStep = 1;
        ConsumerRadChart.Chart.PlotArea.XAxis.LabelStep = 1;
        ConsumerRadChart.Chart.PlotArea.YAxis.AutoScale = false;
        ConsumerRadChart.Chart.PlotArea.YAxis.AxisMode = ChartYAxisMode.Normal;
        ConsumerRadChart.Chart.PlotArea.YAxis.MaxValue = maxYCount + 2;
        ConsumerRadChart.Chart.PlotArea.YAxis.Step = 1;
        ConsumerRadChart.Chart.PlotArea.Appearance.Dimensions.Margins.Right = 200;
        ConsumerRadChart.Chart.PlotArea.XAxis.AutoScale = false;

        //ConsumerRadChart.Chart.PlotArea.XAxis.MaxValue = dvAdCategories.Count + 2;
        //ConsumerRadChart.Chart.PlotArea.XAxis.Step = 1;
        //ConsumerRadChart.DataBind();

        ChartPanel.Visible = true;
    }

    protected void SortReason(object sender, EventArgs e)
    {

        string adID = AdDropDown.SelectedItem.Value;

        HttpCookie cookie = Request.Cookies["BrowserDate"];

        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        if (Session["SortReason"] == null)
            Session["SortReason"] = " DESC";

        if (Session["SortReason"] == " DESC")
            Session["SortReason"] = " ASC";
        else
            Session["SortReason"] = " DESC";

        DataView dvAdStatisticsCovert = dat.GetDataDV("SELECT WasEmail, CASE WHEN LocationOnly = 'True' THEN 'Location Only' " +
            "WHEN WasEmail = 'True' THEN 'Email View' WHEN Reason = 'NonAssigned' THEN 'Non-Assigned User' " +
            "ELSE dbo.GetCategories(Reason) END AS 'Reason', CASE WHEN LocationOnly = 'True' THEN 'Location Only' " +
            "WHEN WasEmail = 'True' THEN 'Email View' WHEN Reason = 'NonAssigned' THEN 'Non-Assigned User' " +
            "ELSE dbo.GetCategories(Reason) END AS 'Reason For View [Location Only, " +
            "Non-Assigned User or list of Categories]', [Date] " +
            "FROM AdStatistics WHERE AdID=" + adID );

        dvAdStatisticsCovert.Sort = "Reason" + Session["SortReason"].ToString();

        if (Session["SortReason"].ToString() == " ASC")
        {
            DateLinkButton.Text = "Date";
            ReasonLinkButton.Text = "Reason For View &#9650;";
            EmailLinkButton.Text = "Email View";
        }
        else
        {
            DateLinkButton.Text = "Date";
            ReasonLinkButton.Text = "Reason For View &#9660;";
            EmailLinkButton.Text = "Email View";
        }

        Literal ListofUsersLiteral = new Literal();
        PanelofUsers.Controls.Clear();
        for (int i = 0; i < dvAdStatisticsCovert.Count; i++)
        {
            ListofUsersLiteral.Text += "<tr><td>" + (i + 1).ToString() + "</td><td>";
            ListofUsersLiteral.Text += dvAdStatisticsCovert[i]["Reason"].ToString();

            ListofUsersLiteral.Text += "</td><td>" +
                DateTime.Parse(dvAdStatisticsCovert[i]["Date"].ToString()).ToShortDateString() + "</td>" +
                "<td>" + dvAdStatisticsCovert[i]["WasEmail"].ToString() + "</td>" +
                "</tr>";
        }


        PanelofUsers.Controls.Add(ListofUsersLiteral);
    }

    protected void SortDate(object sender, EventArgs e)
    {
        try
        {

            string adID = AdDropDown.SelectedItem.Value;

            HttpCookie cookie = Request.Cookies["BrowserDate"];

            if (Session["SortDate"] == null)
                Session["SortDate"] = " DESC";

            if (Session["SortDate"] == " DESC")
                Session["SortDate"] = " ASC";
            else
                Session["SortDate"] = " DESC";

            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            DataView dvAdStatisticsCovert = dat.GetDataDV("SELECT WasEmail, CASE WHEN LocationOnly = 'True' THEN 'Location Only' " +
                "WHEN WasEmail = 'True' THEN 'Email View' WHEN Reason = 'NonAssigned' THEN 'Non-Assigned User' " +
                "ELSE dbo.GetCategories(Reason) END AS 'Reason', CASE WHEN LocationOnly = 'True' THEN 'Location Only' " +
                "WHEN WasEmail = 'True' THEN 'Email View' WHEN Reason = 'NonAssigned' THEN 'Non-Assigned User' " +
                "ELSE dbo.GetCategories(Reason) END AS 'Reason For View [Location Only, " +
                "Non-Assigned User or list of Categories]', [Date] " +
                "FROM AdStatistics WHERE AdID=" + adID);

            dvAdStatisticsCovert.Sort = "Date" + Session["SortDate"].ToString();

            if (Session["SortDate"].ToString() == " ASC")
            {
                DateLinkButton.Text = "Date &#9650;";
                ReasonLinkButton.Text = "Reason For View";
                EmailLinkButton.Text = "Email View";
            }
            else
            {
                DateLinkButton.Text = "Date &#9660;";
                ReasonLinkButton.Text = "Reason For View";
                EmailLinkButton.Text = "Email View";
            }

            Literal ListofUsersLiteral = new Literal();
            PanelofUsers.Controls.Clear();
            for (int i = 0; i < dvAdStatisticsCovert.Count; i++)
            {
                ListofUsersLiteral.Text += "<tr><td>" + (i + 1).ToString() + "</td><td>";
                ListofUsersLiteral.Text += dvAdStatisticsCovert[i]["Reason"].ToString();

                ListofUsersLiteral.Text += "</td><td>" +
                    DateTime.Parse(dvAdStatisticsCovert[i]["Date"].ToString()).ToShortDateString() + "</td>" +
                    "<td>" + dvAdStatisticsCovert[i]["WasEmail"].ToString() + "</td>" +
                    "</tr>";
            }


            PanelofUsers.Controls.Add(ListofUsersLiteral);
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString();
        }
    }

    protected void SortEmail(object sender, EventArgs e)
    {

        string adID = AdDropDown.SelectedItem.Value;

        HttpCookie cookie = Request.Cookies["BrowserDate"];

        if (Session["SortEmail"] == null)
            Session["SortEmail"] = " DESC";

        if (Session["SortEmail"] == " DESC")
            Session["SortEmail"] = " ASC";
        else
            Session["SortEmail"] = " DESC";

        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        DataView dvAdStatisticsCovert = dat.GetDataDV("SELECT WasEmail, CASE WHEN LocationOnly = 'True' THEN 'Location Only' " +
            "WHEN WasEmail = 'True' THEN 'Email View' WHEN Reason = 'NonAssigned' THEN 'Non-Assigned User' " +
            "ELSE dbo.GetCategories(Reason) END AS 'Reason', CASE WHEN LocationOnly = 'True' THEN 'Location Only' " +
            "WHEN WasEmail = 'True' THEN 'Email View' WHEN Reason = 'NonAssigned' THEN 'Non-Assigned User' " +
            "ELSE dbo.GetCategories(Reason) END AS 'Reason For View [Location Only, " +
            "Non-Assigned User or list of Categories]', [Date] " +
            "FROM AdStatistics WHERE AdID=" + adID);

        dvAdStatisticsCovert.Sort = "WasEmail" + Session["SortEmail"].ToString();

        if (Session["SortEmail"].ToString() == " ASC")
        {
            DateLinkButton.Text = "Date";
            ReasonLinkButton.Text = "Reason For View";
            EmailLinkButton.Text = "Email View &#9650;";
        }
        else
        {
            DateLinkButton.Text = "Date";
            ReasonLinkButton.Text = "Reason For View";
            EmailLinkButton.Text = "Email View &#9660;";
        }

        Literal ListofUsersLiteral = new Literal();
        PanelofUsers.Controls.Clear();
        for (int i = 0; i < dvAdStatisticsCovert.Count; i++)
        {
            ListofUsersLiteral.Text += "<tr><td>" + (i + 1).ToString() + "</td><td>";
            ListofUsersLiteral.Text += dvAdStatisticsCovert[i]["Reason"].ToString();

            ListofUsersLiteral.Text += "</td><td>" +
                DateTime.Parse(dvAdStatisticsCovert[i]["Date"].ToString()).ToShortDateString() + "</td>" +
                "<td>" + dvAdStatisticsCovert[i]["WasEmail"].ToString() + "</td>" +
                "</tr>";
        }


        PanelofUsers.Controls.Add(ListofUsersLiteral);
    }
}
