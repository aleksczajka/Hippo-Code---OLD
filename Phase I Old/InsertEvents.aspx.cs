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
using System.Collections.Generic;
using System.Data.OleDb;

public partial class InsertEvents : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlMeta hm = new HtmlMeta();
        HtmlHead head = (HtmlHead)Page.Header;
        hm.Name = "ROBOTS";
        hm.Content = "NOINDEX, FOLLOW";
        head.Controls.AddAt(0, hm);
        //BuyAtAPI.BuyAtAPIClient client = new BuyAtAPI.BuyAtAPIClient(System.Configuration.ConfigurationManager.AppSettings["BuyAtAPIKey"].ToString());
        //BuyAtAPI.Feed feed = client.GetFeedInfo(300);
        //int[] feedIDs = {300};
        //int[] exld = {0, 1};
        //BuyAtAPI.ProductResultSet products = client.SearchProducts("", 1, 10, 
        //    feedIDs, null, null, feedIDs, 1, 2, false, "1", BuyAtAPI.BuyAtAPIClient.ProductField.productName, 
        //    BuyAtAPI.BuyAtAPIClient.SortOrder.asc);

        //StatusPanelPanel.Visible = true;
        //foreach (BuyAtAPI.Product prod in products.Products)
        //{
        //    StatusPanel.Text = "brandname: " + prod.BrandName + ", description: " + prod.Description + ", " + prod.Level1CategoryName;
        //}
       
    }

    protected void ClickItVenues(object sender, EventArgs e)
    {
        DoIt(false);
    }

    protected void DoIt(bool isVenue)
    {
        try
        {
            string fileName;
            if (MainFileUpload.HasFile)
            {
                fileName = MainFileUpload.FileName;
                MainFileUpload.SaveAs(Server.MapPath("images/" + fileName));

                char[] sep2 = { '.' };
                string[] ext = fileName.Split(sep2);
                string extension = ext[1];

                if (extension == "xls" || extension == "xlsx")
                {
                    OleDbConnection dbConnection =
                      new OleDbConnection
                        (@"Provider=Microsoft.Jet.OLEDB.4.0;"
                         + @"Data Source=" + Server.MapPath("images/" + fileName) + ";"
                         + @"Extended Properties=""Excel 8.0;HDR=Yes;IMEX=1;""");
                    dbConnection.Open();
                    OleDbDataAdapter dbAdapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", dbConnection);
                    OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", dbConnection);

                    DataSet myDataSet = new DataSet();

                    myCommand.Fill(myDataSet);

                    

                    try
                    {
                        DataView dv = new DataView(myDataSet.Tables[0], "", "", DataViewRowState.CurrentRows);

                        foreach (DataRowView row in dv)
                        {
                            if (isVenue)
                            {
                                PostItVenue(row);
                            }
                            else
                            {
                                PostItEventsByHand(row);
                            }

                            StatusPanelPanel.Visible = true;
                            StatusPanel.Text += "got one row";
                        }
                    }
                    catch (Exception ex)
                    {
                        StatusPanelPanel.Visible = true;
                        StatusPanel.Text = ex.ToString();
                    }


                    dbConnection.Close();
                }
                else
                {
                    StatusPanelPanel.Visible = true;
                    StatusPanel.Text = "The File must have .xls extension.";
                }

            }
            else
            {
                StatusPanelPanel.Visible = true;
                StatusPanel.Text = "Must Enter File.";
            }
        }
        catch (Exception ex)
        {
            StatusPanelPanel.Visible = true;
            
                StatusPanel.Text = ex.ToString();
            
        }
    }

    protected void PostItEventsByHand(DataRowView row)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        string problem = "";
        try
        {
            if (row["Content"].ToString().Trim() != "" && row["Header"].ToString().Trim() != "")
            {
                StatusPanel.Text += "got inside";
                Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
                string email = "";
                string textEmail = "";
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
                conn.Open();

                //this means there is no pictures or videos




                DataSet dsEvent = new DataSet();
                string theCat = "NULL";

                string command = "";

                command = "INSERT INTO Events (Owner, [Content], " +
                     "Header, Venue, EventGoersCount, SponsorPresenter, hasSongs, mediaCategory, UserName, "
                 + "ShortDescription, Country, State, Zip, City, StarRating, PostedOn)"
                     + " VALUES(@owner, @content, @header, @venue, "
                     + " 0, @sponsor, 0, 0, @userName, @shortDescription"
                 + ", @country, @state, @zip, @city, 0, @dateP)";


                SqlCommand cmd = new SqlCommand(command, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@dateP", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
                cmd.Parameters.Add("@content", SqlDbType.NVarChar).Value = row["Content"].ToString();
                cmd.Parameters.Add("@header", SqlDbType.NVarChar).Value = dat.CleanExcelString(row["Header"].ToString());
                cmd.Parameters.Add("@shortDescription", SqlDbType.NVarChar).Value = row["ShortDescription"].ToString();
                cmd.Parameters.Add("@userName", SqlDbType.NVarChar).Value = row["UserName"].ToString();
                cmd.Parameters.Add("@owner", SqlDbType.Int).Value = DBNull.Value;
                cmd.Parameters.Add("@sponsor", SqlDbType.NVarChar).Value = DBNull.Value;




                #region Create/Assign Venue
                string venue = "";
                bool isNewVenue = false;
                int venueID = 0;
                string country = "";
                string state1 = "";


                //User created a new venue. Must save it.
                string city = "";
                string zip = "";
                string state = "";
                bool goNext = true;
                if (row["VenueID"] != null)
                {
                    if (row["VenueID"].ToString().Trim() != "")
                    {
                        venueID = int.Parse(row["VenueID"].ToString());
                        isNewVenue = false;
                        goNext = false;
                        DataView dvA = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + row["VenueID"].ToString());
                        country = dvA[0]["Country"].ToString();
                        city = dvA[0]["City"].ToString();
                        zip = dvA[0]["Zip"].ToString();
                        state = dvA[0]["State"].ToString();
                    }
                }



                SqlCommand cmd2;
                //SqlCommand cmd2 = new SqlCommand("SELECT * FROM Venues WHERE Name=@name", conn);
                //cmd2.Parameters.Add("@name", SqlDbType.NVarChar).Value = venue;


                //SqlDataAdapter da = new SqlDataAdapter(cmd2);
                //DataSet ds = new DataSet();
                //da.Fill(ds);

                //bool goNext = false;

                //if (ds.Tables.Count > 0)
                //    if (ds.Tables[0].Rows.Count > 0)
                //        cmd.Parameters.Add("@venue", SqlDbType.Int).Value = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                //    else
                //        goNext = true;
                //else
                //    goNext = true;

                if (goNext)
                {
                    cmd2 = new SqlCommand("INSERT INTO Venues (Content, Name, Address, Country, State, City, Zip, " +
                        "MediaCategory, CreatedByUser, Edit, Rating) VALUES (@content, @vName, @address, @country, @state, @city, @zip, 0, @user, 'True', 0)", conn);
                    cmd2.Parameters.Add("@vName", SqlDbType.NVarChar).Value = row["VenueName"].ToString();
                    cmd2.Parameters.Add("@user", SqlDbType.Int).Value = row["UserID"].ToString();
                    cmd2.Parameters.Add("@country", SqlDbType.Int).Value = row["Country"].ToString();
                    cmd2.Parameters.Add("@city", SqlDbType.NVarChar).Value = row["City"].ToString();
                    cmd2.Parameters.Add("@zip", SqlDbType.NVarChar).Value = row["Zip"].ToString();
                    cmd2.Parameters.Add("@content", SqlDbType.NVarChar).Value = row["VenueContent"].ToString();
                    state = row["State"].ToString();

                    string locationStr = "";
                    string apt = "";
                    if (row["AptNumber"].ToString().Trim() != "")
                        apt = row["SuiteApt"].ToString() + " " + row["AptNumber"].ToString();

                    locationStr = row["StreetNumber"].ToString() + ";" + row["StreetName"].ToString().Trim().ToLower()
                        + ";" + row["StreetTitle"].ToString() + ";" + apt;


                    cmd2.Parameters.Add("@state", SqlDbType.NVarChar).Value = state;
                    cmd2.Parameters.Add("@address", SqlDbType.NVarChar).Value = locationStr;
                    cmd2.ExecuteNonQuery();

                    cmd2 = new SqlCommand("SELECT @@IDENTITY AS AID", conn);


                    SqlDataAdapter da1 = new SqlDataAdapter(cmd2);
                    DataSet ds2 = new DataSet();
                    da1.Fill(ds2);

                    venueID = int.Parse(ds2.Tables[0].Rows[0]["AID"].ToString());


                    country = row["Country"].ToString();
                    city = row["City"].ToString();
                    zip = row["Zip"].ToString();
                }

                cmd.Parameters.Add("@venue", SqlDbType.Int).Value = venueID;

                cmd.Parameters.Add("@country", SqlDbType.Int).Value = country;
                cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = state;
                cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = city;
                cmd.Parameters.Add("@zip", SqlDbType.NVarChar).Value = zip;


                #endregion





                cmd.ExecuteNonQuery();


                cmd = new SqlCommand("SELECT @@IDENTITY AS ID", conn);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd);
                DataSet ds3 = new DataSet();
                da2.Fill(ds3);


                string ID = ds3.Tables[0].Rows[0]["ID"].ToString();




                string temporaryID = ID;

                CreateCategories(temporaryID, row["Categories"].ToString());

                StatusPanel.Text += "inserted event and veue;";

                #region Take Care of Event Occurance

                StatusPanel.Text += ";startime:" + row["StartDateTime"].ToString();

                cmd = new SqlCommand("INSERT INTO Event_Occurance (EventID, DateTimeStart, DateTimeEnd) VALUES(@eventID, @dateStart, @dateEnd)", conn);
                cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = temporaryID;
                cmd.Parameters.Add("@dateStart", SqlDbType.DateTime).Value = row["StartDateTime"].ToString();
                cmd.Parameters.Add("@dateEnd", SqlDbType.DateTime).Value = row["EndDateTime"].ToString();
                cmd.ExecuteNonQuery();


                #endregion

                conn.Close();


            }

        }
        catch (Exception ex)
        {
            StatusPanelPanel.Visible = true;

            StatusPanel.Text = ex.ToString();
        }
    }

    protected void PostItEventsByTix(DataRowView row)
    {
        string message = "";
        bool cont = true;
        if (row["VenueID"].ToString().Trim() == "")
            cont = false;

        if (cont)
        {
            HttpCookie cookie = Request.Cookies["BrowserDate"];
            string problem = "";
            try
            {
                StatusPanel.Text += "got inside";
                Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

                SqlCommand cmd;
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
                conn.Open();
                DataView dvVenue = dat.GetDataDV("SELECT * FROM Venues WHERE TixMasterCode=" + row["VenueID"].ToString());

                if (dvVenue.Count > 0)
                {

                    DataView dvEvent = dat.GetDataDV("SELECT * FROM Events WHERE Header= '" +
                        row["product_name"].ToString().Replace("'", "''") + "' AND Venue=" + dvVenue[0]["ID"].ToString());

                    string temporaryID;

                    if (dvEvent.Count == 0)
                    {
                        string email = "";
                        string textEmail = "";

                        //this means there is no pictures or videos
                        DataSet dsEvent = new DataSet();
                        string theCat = "NULL";

                        string command = "";

                        command = "INSERT INTO Events (Owner, [Content], " +
                             "Header, Venue, EventGoersCount, SponsorPresenter, hasSongs, mediaCategory, UserName, "
                         + "ShortDescription, Country, State, Zip, City, StarRating, PostedOn, BuyAtTix)"
                             + " VALUES(@owner, @content, @header, @venue, "
                             + " 0, @sponsor, 0, 0, @userName, @shortDescription"
                         + ", @country, @state, @zip, @city, 0, @dateP, '" + row["buyat_short_deeplink_url"].ToString() + "')";


                        cmd = new SqlCommand(command, conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("@dateP", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
                        cmd.Parameters.Add("@content", SqlDbType.NVarChar).Value = row["product_name"].ToString() + " <br/> This events was grabbed from a feed by TicketMaster. As this feed does not provide an end time, all such events are reported as having the end time 1hour after start time. Please check out TicketMaster for the actual end time!";
                        cmd.Parameters.Add("@header", SqlDbType.NVarChar).Value = row["product_name"].ToString();
                        cmd.Parameters.Add("@shortDescription", SqlDbType.NVarChar).Value = row["product_name"].ToString();
                        cmd.Parameters.Add("@userName", SqlDbType.NVarChar).Value = "HippoHappenings";
                        cmd.Parameters.Add("@owner", SqlDbType.Int).Value = DBNull.Value;
                        cmd.Parameters.Add("@sponsor", SqlDbType.NVarChar).Value = DBNull.Value;

                        cmd.Parameters.Add("@venue", SqlDbType.Int).Value = int.Parse(dvVenue[0]["ID"].ToString());

                        cmd.Parameters.Add("@country", SqlDbType.Int).Value = dvVenue[0]["Country"].ToString();
                        cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = dvVenue[0]["State"].ToString();
                        cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = dvVenue[0]["City"].ToString();
                        cmd.Parameters.Add("@zip", SqlDbType.NVarChar).Value = dvVenue[0]["Zip"].ToString();







                        cmd.ExecuteNonQuery();


                        cmd = new SqlCommand("SELECT @@IDENTITY AS ID", conn);
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd);
                        DataSet ds3 = new DataSet();
                        da2.Fill(ds3);


                        temporaryID = ds3.Tables[0].Rows[0]["ID"].ToString();

                        CreateCategories(temporaryID, row["level1"].ToString());
                    }
                    else
                    {
                        temporaryID = dvEvent[0]["ID"].ToString();
                    }

                    StatusPanel.Text += "inserted event and veue;";

                    string dtime = row["EventDate"].ToString().Replace(" 12:00:00 AM", "") + " " +
                        row["EventTime"].ToString().Replace("12/30/1899", "");

                    message += dtime;
                    DateTime dt = DateTime.Parse(dtime);

                    StatusPanel.Text += ";startime:" + dt.ToString();

                    cmd = new SqlCommand("INSERT INTO Event_Occurance (EventID, DateTimeStart, DateTimeEnd) VALUES(@eventID, @dateStart, @dateEnd)", conn);
                    cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = temporaryID;
                    cmd.Parameters.Add("@dateStart", SqlDbType.DateTime).Value = dt;
                    cmd.Parameters.Add("@dateEnd", SqlDbType.DateTime).Value = dt.AddHours(1);
                    cmd.ExecuteNonQuery();


                    conn.Close();


                }

            }
            catch (Exception ex)
            {
                StatusPanelPanel.Visible = true;

                StatusPanel.Text += ex.ToString() + ", dt: " + message;
            }
        }
    }

    protected void PostItVenue(DataRowView row)
    {
        bool cont = true;
        if (row["product_name"].ToString().Contains("duplicate do not use") || row["State"].ToString().Trim() == ""
             || row["Street"].ToString().Trim() == "")
            cont = false;

        

        if (cont)
        {
            HttpCookie cookie = Request.Cookies["BrowserDate"];
            string problem = "";
            try
            {
                StatusPanel.Text += "got inside";
                Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
                string email = "";
                string textEmail = "";
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
                conn.Open();

                //this means there is no pictures or videos

                DataView dvStreets = dat.GetDataDV("SELECT * FROM Streets");


                DataSet dsEvent = new DataSet();
                string theCat = "NULL";

                string command = "";

                #region Create/Assign Venue
                string venue = "";
                bool isNewVenue = false;
                int venueID = 0;
                string country = "";
                string state1 = "";


                //User created a new venue. Must save it.

                venue = row["product_name"].ToString();
                isNewVenue = true;
                bool goNext = true;
                SqlCommand cmd2;


                if (goNext)
                {
                    cmd2 = new SqlCommand("INSERT INTO Venues (Content, Name, Address, Country, State, City, Zip, " +
                        "MediaCategory, CreatedByUser, Edit, Rating, TixMasterCode, PostedOn, LastEditOn) VALUES (@content, @vName, @address, @country, "+
                        "@state, @city, @zip, 0, @user, 'True', 0, @thecode, GETDATE(), GETDATE())", conn);
                    cmd2.Parameters.Add("@vName", SqlDbType.NVarChar).Value = venue;
                    cmd2.Parameters.Add("@user", SqlDbType.Int).Value = 42;
                    cmd2.Parameters.Add("@country", SqlDbType.Int).Value = 223;
                    cmd2.Parameters.Add("@city", SqlDbType.NVarChar).Value = row["City"].ToString();
                    cmd2.Parameters.Add("@zip", SqlDbType.NVarChar).Value = row["ZipCode"].ToString();
                    cmd2.Parameters.Add("@content", SqlDbType.NVarChar).Value = row["buyat_short_deeplink_url"].ToString();
                    cmd2.Parameters.Add("@thecode", SqlDbType.NVarChar).Value = row["product_code"].ToString();

                    string state = row["State"].ToString();

                    //Get Address parameter
                    string street = row["State"].ToString();

                    string locationStr = GetLocation(row["Street"].ToString(), dvStreets);
                    


                    cmd2.Parameters.Add("@state", SqlDbType.NVarChar).Value = state;
                    cmd2.Parameters.Add("@address", SqlDbType.NVarChar).Value = locationStr;
                    cmd2.ExecuteNonQuery();


                }



                #endregion



                StatusPanel.Text += "inserted event and veue;";



                conn.Close();




            }
            catch (Exception ex)
            {
                StatusPanelPanel.Visible = true;

                StatusPanel.Text = ex.ToString();
            }
        }
    }

    protected string GetLocation(string loc, DataView Streets)
    {
        string apt = "";

        char[] delim = { ' ' };
        string[] tokens = loc.Split(delim);

        int theNumber = 0;

        string streetNumber = "";
        string streetName = "";
        string streetTitle = "";
        string aptNumber = "";


        for (int i = 0; i < tokens.Length; i++)
        {
            if (int.TryParse(tokens[i], out theNumber))
            {
                if (streetNumber == "")
                    streetNumber = theNumber.ToString();
                else
                {
                    if (loc.ToLower().Contains("apt") || loc.ToLower().Contains("suite") || loc.ToLower().Contains("#"))
                        aptNumber = theNumber.ToString();
                    else
                        streetName += " " + theNumber.ToString();
                }
            }
            else
            {
                Streets.RowFilter = "Name Like '%" + tokens[i].Replace("'", "''") + "%'";

                if (Streets.Count == 0)
                    streetName += " " + tokens[i];
            }
        }

        for (int i = 0; i < Streets.Count; i++)
        {
            if (loc.ToLower().Contains(Streets[i]["Name"].ToString().ToLower()))
            {
                streetTitle = Streets[i]["Name"].ToString();
            }
        }



        if (aptNumber != "")
            if (loc.ToLower().Contains("apt") || loc.ToLower().Contains("#"))
                aptNumber = "Apt " + aptNumber;
            else
                aptNumber = "Suite " + aptNumber;

        string locationStr = streetNumber + ";" + streetName.ToLower()
            + ";" + streetTitle + ";" + aptNumber;

        return locationStr;
    }

    protected void CreateCategories(string ID, string Categories)
    {
        char[] delim = { ';' };
        string[] tokens = Categories.Split(delim);

        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        string catID = "";
        string size = "22";
        if (Categories.Trim() != "")
        {
            for (int i = 0; i < tokens.Length; i++)
            {
                DataView dvC = dat.GetDataDV("SELECT * FROM EventCategories WHERE Name = '" +
                    tokens[i] + "'");

                if (dvC.Count > 0)
                {
                    catID = dvC[0]["ID"].ToString();

                    if (dvC[0]["ParentID"] != null && dvC[0]["ParentID"].ToString() != "")
                        size = "16";
                    else
                        size = "22";

                    if (catID != null)
                    {
                        if (tokens[i].Trim() != "")
                        {
                            dat.Execute("INSERT INTO Event_Category_Mapping (CategoryID, EventID, tagSize) VALUES("
                                        + catID + "," + ID + ", " + size + ")");
                        }
                    }
                }
            }
        }
    }

    protected void CloseSummary(object sender, EventArgs e)
    {
        StatusPanelPanel.Visible = false;
        StatusPanel.Text = "";
    }

}

