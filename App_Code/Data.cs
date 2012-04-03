 using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Collections;
using System.Net.Mail;
using System.Net;
using System.Xml;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Telerik.Web.UI;
using System.IO;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Summary description for Data
/// </summary>
public class Data : Telerik.Web.UI.RadAjaxPage
{
    SqlConnection conn;
    Hashtable commonWordHash;
    public int numberOfAdsInADay = 7;
    public int numberOfMainAdsInADay = 10;
    public int numberOfAllAdsInDay = 28;
    public enum tagType { AD, EVENT, VENUE, GROUP, GROUP_EVENT, TRIP};
    public tagType TAG_TYPE
    {
        get { return tag_type; }
        set { tag_type = value; }
    }
    public SqlConnection GET_CONNECTED
    {
        get
        {
            if (conn.State != ConnectionState.Open) 
                conn.Open();
            return conn;
        }
    }
    private tagType tag_type;
    public int SearchCutOffNumber = 200;
    public int LOST_AND_FOUND_CATEGORY
    {
        get { return lostAndFoundCategory; }
        set { lostAndFoundCategory = value; }
    }
    private int lostAndFoundCategory = 20;  //if this number is changed, will also have to change it
                                             //in my-account AdCategorie preferences SqlDataSources 
                                             //for CategoryTree and RadTreeView2
    public int HIPPO_AD_CATEGORY
    {
        get { return hippoAdCategory; }
        set { hippoAdCategory = value; }
    }

    public int HIPPO_HALF_AVAILABLE
    {
        get { return hippoHalfAvailableCategory; }
        set { hippoHalfAvailableCategory = value; }
    }

    public int ANONYMOUS_AD_ID
    {
        get { return inconspicuousAdID; }
        set { inconspicuousAdID = value; }
    }

    private int hippoAdCategory = 45;
    private int hippoHalfAvailableCategory = 69; //this ad is shown when more than half of the space is available
                                                //and means that users ads will be shown multiple times for the same cost as
                                                //one ad
    private int inconspicuousAdID = 20;

    public int NUMBER_OF_TOTAL_ADS
    {
        get { return numberTotalAds; }
        set { numberTotalAds = value; }
    }
    private int numberTotalAds = 120;
    public int NUMB_TOTAL_BIG_ADS
    {
        get { return numberTotalBigAds; }
        set { numberTotalBigAds = value; }
    }
    private int numberTotalBigAds = 30;

    public int HIPPOHAPP_USERID
    {
        get { return hippoHappeningsUserID; }
        set { hippoHappeningsUserID = value; }
    }

    private int hippoHappeningsUserID = 42;

    //Used to get User for files that can't access session like iCalHandler.ashx
    public string TheUser
    {
        get { return Session["User"].ToString(); }
    }

    //Time frame in which to get the recommended events
    private DateTime timeNow = DateTime.Now;
    private string timeFrame = "";

    public string TIME_FRAME
    {
        get { return timeFrame; }
        set { timeFrame = value; }
    }

	public Data(DateTime itTime)
	{
        timeNow = itTime;
        timeFrame = " AND EO.DateTimeStart >  CONVERT(DATETIME,'" + timeNow.ToString() + 
            "') AND EO.DateTimeStart < CONVERT(DATETIME,'" +
        timeNow.AddMonths(6).ToString() + "')";
        conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
        commonWordHash = new Hashtable();
    }

    public enum Measurement
    {
        Miles,
        Kilometers
    }

    public string StripHTML_LeaveLinks(string filthyString)
    {
        filthyString = filthyString.Replace("<br/>", "[br/]");
        filthyString = filthyString.Replace("<br", "[br");
        
        string cleanString = Regex.Replace(filthyString, "<(?!\\/?a(?=>|\\s.*>))\\/?.*?>", string.Empty).Replace("<a", "<a class=\"AddLink\" target=\"_blank\"");

        return cleanString.Replace("[br/]", "<br/>").Replace("[br", "<br");
    }

    public string StripHTML_LeaveLinksNoBr(string filthyString)
    {

        string cleanString = Regex.Replace(filthyString, "<(?!\\/?a(?=>|\\s.*>))\\/?.*?>", string.Empty).Replace("<a", "<a class=\"AddLink\" target=\"_blank\"");

        return cleanString;
    }

    //**************************AD Retrieval Methods*******************************

    public string GetAllZipsInRadius(string radius, string zipText, bool notLikes)
    {

        //do only if United States and not for international zip codes
        string zip = "";
        string nonExistantZip = "";
        if (zipText.Trim() != "")
        {
            if (radius != null)
            {
                if (radius.Trim() != "")
                {
                    //Get all zips within the specified radius
                    int zipParam = 0;
                    if (int.TryParse(zipText.Trim(), out zipParam))
                    {
                        DataSet dsLatsLongs = GetData("SELECT Latitude, Longitude FROM ZipCodes WHERE Zip='" +
                            zipParam + "'");

                        //some zip codes don't exist in the database, find the closest one
                        bool findClosest = false;
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
                            zip = " AND A.CatZip LIKE '%;" + zipParam.ToString() + ";%' ";
                            if (notLikes)
                                zip = ";" + zipParam + ";";
                            nonExistantZip = " OR A.CatZip LIKE '%;" + zipParam.ToString() + ";%'";
                            if (notLikes)
                                nonExistantZip = zipParam.ToString() + ";";
                            while (dsLatsLongs == null)
                            {
                                dsLatsLongs = GetData("SELECT Latitude, Longitude FROM ZipCodes WHERE Zip='" + zipParam++ + "'");
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

                        //get all the zip codes within the specified radius
                        DataSet dsZips = GetData("SELECT Zip FROM ZipCodes WHERE dbo.GetDistance(" + dsLatsLongs.Tables[0].Rows[0]["Longitude"].ToString() +
                            ", " + dsLatsLongs.Tables[0].Rows[0]["Latitude"].ToString() + ", Longitude, Latitude) < " + radius);

                        if (dsZips.Tables.Count > 0)
                        {
                            if (dsZips.Tables[0].Rows.Count > 0)
                            {
                                zip = " AND (A.CatZip LIKE '%;" + zipParam.ToString() + ";%' " + nonExistantZip;
                                if (notLikes)
                                    zip = ";" + zipParam.ToString() + ";" + nonExistantZip + ";";
                                for (int i = 0; i < dsZips.Tables[0].Rows.Count; i++)
                                {
                                    if (notLikes)
                                        zip += ";" + dsZips.Tables[0].Rows[i]["Zip"].ToString() + ";";
                                    else
                                        zip += " OR A.CatZip LIKE '%;" + dsZips.Tables[0].Rows[i]["Zip"].ToString() + ";%' ";
                                }
                                zip += ") ";
                            }
                            else
                            {
                                if (notLikes)
                                    zip = ";" + zipText.Trim() + ";";
                                else
                                    zip = " AND A.CatZip LIKE '%;" + zipText.Trim() + ";%'";
                            }
                        }
                        else
                        {
                            if (notLikes)
                                zip = ";" + zipText.Trim() + ";";
                            else
                                zip = " AND A.CatZip LIKE '%;" + zipText.Trim() + ";%'";
                        }


                    }
                    else
                    {
                        if (notLikes)
                            zip = ";" + zipText.Trim() + ";";
                        else
                            zip = " AND A.CatZip LIKE '%;" + zipText.Trim() + ";%' ";
                    }
                }
                else
                {
                    if (notLikes)
                        zip = ";" + zipText.Trim() + ";";
                    else
                        zip = " AND A.CatZip LIKE '%;" + zipText.Trim() + ";%' ";
                }
            }
            else
            {
                if (notLikes)
                    zip = ";" + zipText.Trim() + ";";
                else
                    zip = " AND A.CatZip LIKE '%;" + zipText.Trim() + ";%' ";
            }
        }
        else
        {
            zip = null;
        }

        return zip;
    }

    public int GetNewAnonymousNum(bool isBig)
    {
        string randomNumber = "";
        if (isBig)
        {
            randomNumber = GetDataDV("SELECT * FROM BigAdStatistics_Anonymous ORDER BY ID ASC")[0]["SessionNum"].ToString();
            randomNumber = (int.Parse(randomNumber) + 1).ToString();
            DataSet dsGet = GetData("SELECT * FROM BigAdStatistics_Anonymous WHERE SessionNum='" + randomNumber + "'");
            DataView dvGet = new DataView(dsGet.Tables[0], "", "", DataViewRowState.CurrentRows);

            //If the new session number is in the database, it is not new.
            //continue on until find new number.
            while (dvGet.Count != 0)
            {
                randomNumber = (int.Parse(randomNumber) + 1).ToString();
                dsGet = GetData("SELECT * FROM BigAdStatistics_Anonymous WHERE SessionNum='" + randomNumber + "'");
                dvGet = new DataView(dsGet.Tables[0], "", "", DataViewRowState.CurrentRows);
            }
            Session["BigAnonymousUser"] = randomNumber;
        }
        else
        {
            randomNumber = GetDataDV("SELECT * FROM AdStatistics_Anonymous ORDER BY ID ASC")[0]["SessionNum"].ToString();
            randomNumber = (int.Parse(randomNumber) + 1).ToString();
            DataSet dsGet = GetData("SELECT * FROM AdStatistics_Anonymous WHERE SessionNum='" + randomNumber + "'");
            DataView dvGet = new DataView(dsGet.Tables[0], "", "", DataViewRowState.CurrentRows);

            //If the new session number is in the database, it is not new.
            //continue on until find new number.
            while (dvGet.Count != 0)
            {
                randomNumber = (int.Parse(randomNumber) + 1).ToString();
                dsGet = GetData("SELECT * FROM AdStatistics_Anonymous WHERE SessionNum='" + randomNumber + "'");
                dvGet = new DataView(dsGet.Tables[0], "", "", DataViewRowState.CurrentRows);
            }
            Session["AnonymousUser"] = randomNumber;
        }
        return int.Parse(randomNumber);
    }

    /// <summary>
    ///Get ads when a user is not logged in
    ///This methods tries to determine the location based on the IP address.
    ///However, when an IP address is not found in the database, ads are selected from the most recently
    ///added ads to the database in the entire world.
    /// </summary>
    /// <param name="randomNum"></param>
    /// <returns></returns>
    public DataView GetAnonymousAds(int randomNum, bool isBig)
    {
        //Find the Country, City and State where the user is
        //b.	When user is not logged in, can’t charge for the ads.
        //    i.	Look up IP address in user’s table
        //    1.	If one found, look up ads on city and state
        //    2.	If multiple found, get the majority city and state
        //    ii.	If not found look in searches table, get the city and state
        //    iii.	If not found at all, get a way to find the city and state based on just IP address.
        string IP = GetIP();

        //Look up IP address in user's table
        DataSet dsIP = GetData("SELECT * FROM Users U, UserPreferences UP WHERE UP.UserID=U.User_ID AND U.IPs LIKE '%;" + IP + ";%'");
        DataView dvIP = new DataView(dsIP.Tables[0], "", "", DataViewRowState.CurrentRows);

        //If one or multiple found, look up ads on city and state of the first record
        if (dvIP.Count > 0)
        {
            return RetrieveAnonymousAds(dvIP[0]["CatCity"].ToString(), 
                dvIP[0]["CatState"].ToString(), dvIP[0]["CatCountry"].ToString(), isBig, "");
        }
        //If not found look in searches table, get the city and state
        else
        {
            //DataSet dsSearch = GetData("SELECT * FROM SearchIPs WHERE IP='" + IP + "'");
            //DataView dvSearch = new DataView(dsSearch.Tables[0], "", "", DataViewRowState.CurrentRows);
            //if (dvSearch.Count > 0)
            //{
            //    return RetrieveAnonymousAds(dvSearch[0]["City"].ToString(), dvSearch[0]["State"].ToString(), 
            //        dvSearch[0]["Country"].ToString(), isBig, "");
            //}
            //else
            //{
            //    //iii.	THIS CANNOT BE DONE: If not found at all, get a way to find the city and state based on just IP address. 
                //1.	PLAN 2: Pick a category that would be inconspicuous to the state 
                //a.	and get two ads from each state if they exist
                //b.	however many spaces still need to be filled, get the latest ads.

                //DataSet dsA = GetData("SELECT DISTINCT TOP 120 * FROM Ads A, Ad_Category_Mapping ACM WHERE A.Ad_ID=ACM.AdID AND A.NumCurrentViews < " +
                //    "A.NumViews AND A.Featured=1 AND A.NonUsersAllowed = 'True' AND A.BigAd='" + isBig.ToString() + "' AND ACM.CategoryID=" + inconspicuousAdID.ToString() +
                //    " ORDER BY A.DateAdded DESC");

                DataSet dsA = GetData("SELECT DISTINCT TOP 120 * FROM Ads A WHERE A.NumCurrentViews < " +
                    "A.NumViews AND A.LIVE=1 AND A.Featured=1 AND A.NonUsersAllowed = 'True' AND A.BigAd='" +
                    isBig.ToString() + "' ORDER BY A.DateAdded DESC");

                //DataSet dsFin = MergeAdSetsFull(dsA, dsA2, false);

                //DataView dvA = FillAdSet(dsFin, isBig);

                DataView dvA = new DataView(dsA.Tables[0], "", "", DataViewRowState.CurrentRows);

                return dvA;
            //}
        }
    }

    public DataView GetAnonymousAds(bool isBig)
    {
        DataSet ds;
        if (isBig)
        {
            ds = GetData("SELECT * FROM BigAdStatistics_Anonymous WHERE SessionNum = '" +
            Session["BigAnonymousUser"].ToString() + "'");
        }
        else
        {
            ds = GetData("SELECT * FROM AdStatistics_Anonymous WHERE SessionNum = '" +
                Session["AnonymousUser"].ToString() + "'");
        }

        return GetDVFromDS(GetAdsBasedOnString(ds.Tables[0].Rows[0]["AdsList"].ToString()));
    }

    public DataView RetrieveAnonymousAds(string city, string state, 
        string country, bool isBig, string excludeThese)
    {
        DataSet ds1 = GetData("SELECT DISTINCT TOP 120 * FROM Ads A, Ad_Category_Mapping ACM " +
            "WHERE A.Ad_ID=ACM.AdID AND A.NumCurrentViews < " +
                    "A.NumViews AND A.BigAd='" + isBig.ToString() + "' AND A.LIVE=1 AND A.CatCity='" +
            city + "' AND A.CatState='" + state + "'  AND A.CatCountry=" + country +
            " AND A.Featured=1 AND A.DisplayToAll = 'True' AND ACM.CategoryID=" + inconspicuousAdID.ToString() +
                    excludeThese + " ORDER BY A.DateAdded DESC");

        DataSet ds2 = GetData("SELECT DISTINCT TOP 120 * FROM Ads A " +
            "WHERE A.NumCurrentViews < " +
                    "A.NumViews AND A.BigAd='" + isBig.ToString() + "' AND A.LIVE=1 AND A.CatCity='" +
            city + "' AND A.DisplayToAll = 'True' AND A.CatState='" + state + "'  AND A.CatCountry=" + country +
            " AND A.Featured=1 " + excludeThese + " ORDER BY A.DateAdded DESC");

        DataSet ds3 = MergeAdSetsFull(ds1, ds2, false);

        return FillAdSet(ds3, isBig);
    }

    public DataView SortByLastSeen(DataView dvAds, bool isBig)
    {
        DataSet dsLastSeen = GetData("SELECT LastSeenIndex FROM UserAds WHERE UserID=" +
            Session["User"].ToString() + " AND BigAd='" + isBig.ToString() + "' AND CONVERT(NVARCHAR,Month([Date])) + '/' + CONVERT(NVARCHAR, DAY([Date])) + " +
        "'/' + CONVERT(NVARCHAR, YEAR([Date])) = '" + timeNow.Date + "'");
        DataView dvLastSeen = new DataView(dsLastSeen.Tables[0], "", "", DataViewRowState.CurrentRows);

        if (dvAds.Count > 0)
        {
            if (dvLastSeen.Count > 0)
            {
                string lastSeenAd = dvLastSeen[0]["LastSeenIndex"].ToString();
                //Reorder the ads
                ArrayList a = new ArrayList(dvAds.Count);

                //Get Index of LastSeenAd
                int startIndex = int.Parse(lastSeenAd);

                int otherHalfCount = 0;

                for (int i = 0; i < dvAds.Count; i++)
                {
                    if ((i + startIndex + 1) >= dvAds.Count)
                    {
                        a.Insert(i, dvAds[otherHalfCount++]["Ad_ID"].ToString());
                    }
                    else
                    {
                        a.Insert(i, dvAds[i + startIndex + 1]["Ad_ID"].ToString());
                    }
                    
                }

                DataSet dsAds = GetAdsBasedOnArray(a);
                DataView dvFinal = new DataView(dsAds.Tables[0], "", "", DataViewRowState.CurrentRows);
                return dvFinal;
            }
            else
            {
                return dvAds;
            }
        }
        else
        {
            return dvAds;
        }

    }

    //Call from Ads.aspx
    //Must include UserID, cannot get it from Session["User"] since user's ads can also be grabbed
    //based on the user's preferences if they are not logged in. The IP address could be used to 
    //match up with the user's profile and look up the ads based on the UserID assigned to that IP.
    public DataView RetrieveUsersAds(bool isBig, string UserID)
    {
        //Perform the algorithm only if there is no entry 
        //for the user normal/big in the UserAds table for today's date.
        bool userHasAdSet = false;

        DataSet dsUser = GetData("SELECT * FROM UserAds WHERE UserID=" +
            UserID + " AND CONVERT(NVARCHAR,Month([Date])) + '/' + CONVERT(NVARCHAR, DAY([Date])) + " +
            "'/' + CONVERT(NVARCHAR, YEAR([Date])) ='" + timeNow.Date.ToShortDateString().Replace("12:00:00 AM", "").Trim() +
                    "' AND BigAd='" + isBig.ToString() + "'");

        DataView dvUser = new DataView(dsUser.Tables[0], "", "", DataViewRowState.CurrentRows);

        bool isPresent = false;
        bool isFull = true;
        if (dvUser.Count > 0)
        {
            if (dvUser[0]["AdSet"].ToString().Trim() != "")
            {
                isPresent = true;
                isFull = bool.Parse(dvUser[0]["isFull"].ToString());

                if(isFull)
                    userHasAdSet = true;
            }
                
        }
        char[] delim = { ';' };
        if (userHasAdSet)
        {
            
            Array a = dvUser[0]["AdSet"].ToString().Split(delim);
            DataSet dsA = GetAdsBasedOnArray(a);
            DataView dvA = new DataView(dsA.Tables[0], "", "", DataViewRowState.CurrentRows);
            return dvA;
        }
        //If the ads are not in the database, perform the algorithm
        else
        {
            //If we made it here because the ad array is not full
            string seenAds = "";
            int countSeen = 0;
            Hashtable tempHash = new Hashtable();
            if (!isFull)
            {
                seenAds = dvUser[0]["SeenAdSet"].ToString();
                if (seenAds.Trim() != "")
                {
                    string[] adTokens = seenAds.Split(delim);
                    foreach (string str in adTokens)
                    {
                        if (str.Trim() == "")
                        {
                            if (!tempHash.Contains(str))
                            {
                                countSeen++;
                                tempHash.Add(str, "");
                            }
                        }
                    }
                }
            }
            //iii.	Get all user’s chosen ad categories (including lost and 
            //found category: make a global variable in the Data class for the 
            //    ID of the lost and found category since it’s bound to change) 
            //in user’s category location

            int count1 = 0;
            int count2 = 0;
            int count3 = 0;
            int count4 = 0;
            DataSet adsOnCategoryNLocation1 = GetUserAds(isBig, false, UserID, true);

            DataSet adsOnCategoryNLocation2 = GetUserAds(isBig, true, UserID, true);

            DataSet adsFinal;

            bool isLessThanTotal = false;

            int adNumberToGet = numberTotalAds;

            if (isBig)
                adNumberToGet = numberTotalBigAds;

            if (adsOnCategoryNLocation1.Tables[0].Rows.Count < adNumberToGet - countSeen)
            {
                adsFinal = MergeAdSets(adsOnCategoryNLocation1, adsOnCategoryNLocation2, false);
            }
            else
            {
                adsFinal = adsOnCategoryNLocation1;
            }

            if (adsFinal.Tables[0].Rows.Count < adNumberToGet - countSeen)
                isLessThanTotal = true;


            //if (count1 < adNumberToGet-countSeen)
            //{
            //    adsOnCategoryNLocation2 = GetAdsOnCategoryAndLocation(isBig, true, true, false, 
            //        ref count2, true);

            //    if (count1 + count2 < adNumberToGet - countSeen)
            //    {
            //        adsOnCategoryNLocation3 = GetAdsOnCategoryAndLocation(isBig, false, true, true,
            //            ref count3, false);

            //        if (count1 + count2 + count3 < adNumberToGet - countSeen)
            //        {
            //            adsOnCategoryNLocation4 = GetAdsOnCategoryAndLocation(isBig, false, true, false, 
            //                ref count4, true);

            //            //if still less than numberTotalAds, FillAdSet will take care of it
            //             adsFinal = MergeAdSets(MergeAdSets(MergeAdSets(adsOnCategoryNLocation1,
            //                adsOnCategoryNLocation2, true), adsOnCategoryNLocation3, true), adsOnCategoryNLocation4, true);

            //             if (count1 + count2 + count3 + count4 < adNumberToGet - countSeen)
            //             {
            //                 isLessThanTotal = true;
            //             }
                        
            //        }
            //        else
            //        {
            //            adsFinal = MergeAdSets(MergeAdSets(adsOnCategoryNLocation1, 
            //                adsOnCategoryNLocation2, true), adsOnCategoryNLocation3, true);
            //        }
            //    }
            //    else
            //    {
            //        //Merge the two sets
            //        adsFinal = MergeAdSets(adsOnCategoryNLocation1, adsOnCategoryNLocation2, true);
            //    }
            //}
            //else
            //{
            //    adsFinal = adsOnCategoryNLocation1;
            //}

            //Save the DataSet
            
            string lastSeenAd = "";
            DataView dvAdsFinal;

            DataSet dsAdsSure;

            if (adsFinal.Tables[0].Rows.Count == 0)
            {
                dsAdsSure = new DataSet();
                dsAdsSure.Tables.Add();
            }
            else
                dsAdsSure = MakeSureDSIsUnique(adsFinal);

            if (countSeen != 0)
            {
                dvAdsFinal = FillAdSet(MergeAdSets(dsAdsSure, GetAdsBasedOnString(seenAds), false), isBig);
            }
            else
            {
                dvAdsFinal = FillAdSet(dsAdsSure, isBig);
            }
            
            string adSet = GetAdStringBasedOnDS(dvAdsFinal, ref lastSeenAd);

            bool isFullNow = true;
            if (isLessThanTotal)
                isFullNow = false;


            string lastIndexPrimo = (numberTotalAds - 1).ToString();
            if (isBig)
                lastIndexPrimo = (numberTotalBigAds - 1).ToString();

            if (isPresent)
            {
                Execute("UPDATE UserAds SET AdSet='" + adSet + "', LastSeenIndex='"+lastIndexPrimo+"', isFull='" + isFullNow.ToString() +
                    "' WHERE UserID=" + UserID + " AND CONVERT(NVARCHAR,Month([Date])) + '/' + CONVERT(NVARCHAR, DAY([Date])) + " +
        "'/' + CONVERT(NVARCHAR, YEAR([Date])) ='" + timeNow.Date.ToShortDateString().Replace("12:00:00 AM", "").Trim() +
                    "' AND BigAd='" + isBig.ToString() + "'");
            }
            else
            {
                Execute("INSERT INTO UserAds (AdSet, LastSeenIndex, isFull, BigAd, [Date], UserID) " +
                    " VALUES('" + adSet + "', " + lastIndexPrimo + ", '" + isFullNow.ToString() + "', '" + isBig.ToString() + "', '" +
                    timeNow.Date.ToShortDateString() + "', " + UserID + ")");
            }
            
            //return SortByLastSeen(adsFinal, isBig);
            return dvAdsFinal;
        }

    }

    public DataView RetrieveAnonymousUsersAds(bool isBig, string UserID)
    {
        //Perform the algorithm only if there is no entry 
        //for the user normal/big in the UserAds table for today's date.
        bool userHasAdSet = false;

        bool isPresent = false;
        bool isFull = true;
        //If we made it here because the ad array is not full
        string seenAds = "";
        int countSeen = 0;
        //iii.	Get all user’s chosen ad categories (including lost and 
        //found category: make a global variable in the Data class for the 
        //    ID of the lost and found category since it’s bound to change) 
        //in user’s category location

        int count1 = 0;
        int count2 = 0;
        int count3 = 0;
        int count4 = 0;
        DataSet adsOnCategoryNLocation1 = GetUserAds(isBig, false, UserID, true);

        DataSet adsOnCategoryNLocation2 = GetUserAds(isBig, true, UserID, true);

        DataSet adsFinal;

        bool isLessThanTotal = false;

        int adNumberToGet = numberTotalAds;

        if (isBig)
            adNumberToGet = numberTotalBigAds;

        if (adsOnCategoryNLocation1.Tables[0].Rows.Count < adNumberToGet - countSeen)
        {
            adsFinal = MergeAdSets(adsOnCategoryNLocation1, adsOnCategoryNLocation2, false);
        }
        else
        {
            adsFinal = adsOnCategoryNLocation1;
        }

        if (adsFinal.Tables[0].Rows.Count < adNumberToGet - countSeen)
            isLessThanTotal = true;

        string lastSeenAd = "";
        DataView dvAdsFinal;

        DataSet dsAdsSure;

        if (adsFinal.Tables[0].Rows.Count == 0)
        {
            dsAdsSure = new DataSet();
            dsAdsSure.Tables.Add();
        }
        else
            dsAdsSure = MakeSureDSIsUnique(adsFinal);

        if (countSeen != 0)
        {
            dvAdsFinal = FillAdSet(MergeAdSets(dsAdsSure, GetAdsBasedOnString(seenAds), false), isBig);
        }
        else
        {
            dvAdsFinal = FillAdSet(dsAdsSure, isBig);
        }

        string adSet = GetAdStringBasedOnDS(dvAdsFinal, ref lastSeenAd);

        bool isFullNow = true;
        if (isLessThanTotal)
            isFullNow = false;


        string lastIndexPrimo = (numberTotalAds - 1).ToString();
        if (isBig)
            lastIndexPrimo = (numberTotalBigAds - 1).ToString();

        //return SortByLastSeen(adsFinal, isBig);
        return dvAdsFinal;
    }

    public DataView RetrieveAnonymousUsersAds_Multiple(bool isBig, string theIP)
    {
        //We are here because the user is not logged in and there 
        //are multiple users associated with this IP address
        //The algorithm for this case is:
        //i. get all ads for every one of these users where displaytoall is arbitrary
        //ii. only save the ones that fall under each one of the users
        //iii. get all ads in this city, state and country that have displaytoall = 'True'
        //iv. take the union of the previous two sets

        DataView dvUserIP = GetDataDV("SELECT * FROM Users WHERE IPs LIKE '%;" + theIP + ";%'");

        //i. Get intersection of all of these user's ads
        DataSet dsOneUser;
        DataSet dsUsersIntersection = new DataSet();
        foreach (DataRowView row in dvUserIP)
        {
            dsOneUser = GetUserAds(isBig, false, row["User_ID"].ToString(), false);
            if (dsUsersIntersection.Tables.Count == 0)
            {
                dsUsersIntersection = dsOneUser;
            }
            else
            {
                dsUsersIntersection = IntersectDS1withDS2(dsOneUser, dsUsersIntersection);
            }
        }

        //iii. get all ads in the location that have displaytoall = 'True'
        int locationcount = 0;
        DataSet adsLocation = GetAdsOnCategoryAndLocation(isBig, true, true, false,
            ref locationcount, true, dvUserIP[0]["User_ID"].ToString(), dvUserIP, false);

        //iv. take the union of the previous two sets

        DataSet adsFinal;

        bool isLessThanTotal = false;

        int adNumberToGet = numberTotalAds;

        if (isBig)
            adNumberToGet = numberTotalBigAds;


        adsFinal = dsUsersIntersection;
        //If there's too little ads, merge the two sets
        if (adsFinal.Tables[0].Rows.Count < adNumberToGet)
        {
            adsFinal = MergeAdSets(dsUsersIntersection, adsLocation, false);
        }

        //if there is now too many ads, remove the rest
        if (adsFinal.Tables[0].Rows.Count > adNumberToGet)
        {
            int numToRemove = adsFinal.Tables[0].Rows.Count - adNumberToGet;
            for (int i = 0; i < numToRemove; i++)
            {
                adsFinal.Tables[0].Rows.Remove(adsFinal.Tables[0].Rows[adNumberToGet]);
            }
        }

        DataView dvAdsFinal;

        DataSet dsAdsSure;

        if (adsFinal.Tables[0].Rows.Count == 0)
        {
            dsAdsSure = new DataSet();
            dsAdsSure.Tables.Add();
        }
        else
            dsAdsSure = MakeSureDSIsUnique(adsFinal);


        dvAdsFinal = FillAdSet(dsAdsSure, isBig);


        return dvAdsFinal;
    }

    public DataSet GetUserAds(bool isBig, bool onDisplayToNonCategories, string UserID, bool notTodays)
    {
        DateTime date = DateTime.Today;
        DataSet dsPrefs = GetData("SELECT * FROM UserPreferences WHERE UserID=" + UserID);

        string compareState = "";
        string compareCity = "";
        string compareCountry = " AND A.CatCountry=" + dsPrefs.Tables[0].Rows[0]["CatCountry"].ToString();
        SqlDbType[] types;
        object[] data;

        string zips = "";

        if (dsPrefs.Tables[0].Rows[0]["CatState"].ToString() != null)
        {
            compareState = " AND A.CatState = @p0 ";

            if (dsPrefs.Tables[0].Rows[0]["CatCity"].ToString() != null)
            {
                compareCity = " AND A.CatCity = @p1 ";
                types = new SqlDbType[2];
                data = new object[2];
                types[0] = SqlDbType.NVarChar;
                types[1] = SqlDbType.NVarChar;
                data[0] = dsPrefs.Tables[0].Rows[0]["CatState"].ToString();
                data[1] = dsPrefs.Tables[0].Rows[0]["CatCity"].ToString();

            }
            else
            {
                types = new SqlDbType[1];
                data = new object[1];
                types[0] = SqlDbType.NVarChar;
                data[0] = dsPrefs.Tables[0].Rows[0]["CatState"].ToString();


            }
        }
        else
        {
            types = new SqlDbType[0];
            data = new object[0];
        }


        if (dsPrefs.Tables[0].Rows[0]["CatZip"].ToString() != null)
        {
            if (dsPrefs.Tables[0].Rows[0]["CatZip"].ToString().Trim() != "")
            {
                char[] delim = { ';' };
                string[] tokens = dsPrefs.Tables[0].Rows[0]["CatZip"].ToString().Split(delim);
                zips = "";

                for (int i = 0; i < tokens.Length; i++)
                {
                    if (tokens[i].Trim() != "")
                    {
                        if(zips.Trim() != "")
                            zips += " OR ";
                        zips += " A.CatZip LIKE '%;" + tokens[i].Trim() + ";%' ";
                    }
                }
            }
        }

        if (zips.Trim() != "")
            zips = " AND (( " + zips + ") OR A.CatZip is NULL) ";


        string command = "";

        //Select ad only in users category location
        //that is featured
        //that has not been posted by the user viewing this ad
        //that has been seen less numbers than it was paid for
        //that is live
        //that is big/normal based on the isBig boolean.

        //Get all seen ads to exclude in the command
        string excludeTodays = "";
        if (notTodays)
            excludeTodays = " AND CONVERT(NVARCHAR, MONTH([DATE])) +'/' + CONVERT(NVARCHAR, DAY([DATE])) +" +
            "'/' + CONVERT(NVARCHAR, YEAR([DATE])) <> '" + timeNow.Date.ToShortDateString().Replace("12:00:00 AM", "").Trim() + "'";

        DataSet dsExclude = GetData("SELECT AdID AS Ad_ID FROM AdStatistics WHERE UserID=" +
            UserID + " " + excludeTodays);
        DataView dvExclude = new DataView(dsExclude.Tables[0], "", "", DataViewRowState.CurrentRows);

        string adsExclude = "";

        for (int j = 0; j < dvExclude.Count; j++)
        {
            if (j == 0)
            {
                adsExclude += " AND (";
            }

            adsExclude += " A.Ad_ID <> " + dvExclude[j]["Ad_ID"].ToString();

            if (j != dvExclude.Count - 1)
                adsExclude += " AND ";
            else
                adsExclude += " ) ";

        }

        string includeInAll = " AND A.LIVE=1 AND A.Featured=1 AND A.User_ID <> " + 
            hippoHappeningsUserID.ToString() + " AND A.User_ID <> " + UserID + 
            " AND A.NumCurrentViews < A.NumViews AND A.BigAd='" +
                isBig.ToString() + "' " + compareCountry + compareState + compareCity + zips + 
                " ORDER BY DateAdded ASC";





            string displayToAll = "";
            if (onDisplayToNonCategories)
                displayToAll = " OR  A.DisplayToAll='True' ";

            string strOfCategories = "";

            DataView dvCats = GetDataDV("SELECT * FROM UserCategories WHERE UserID=" + UserID);

            for (int i = 0; i < dvCats.Count; i++)
            {
                if (strOfCategories != "")
                {
                    strOfCategories += " OR ";
                }

                strOfCategories += " ACM.CategoryID=" + dvCats[i]["CategoryID"].ToString();
                DataView dvCats2 = GetDataDV("SELECT * FROM AdCategories WHERE ParentID=" + dvCats[i]["CategoryID"].ToString());
                for (int j = 0; j < dvCats2.Count; j++)
                {
                    if (strOfCategories != "")
                    {
                        strOfCategories += " OR ";
                    }
                    strOfCategories += " ACM.CategoryID=" + dvCats2[j]["ID"].ToString();

                    DataView dvCats3 = GetDataDV("SELECT * FROM AdCategories WHERE ParentID=" + dvCats2[j]["ID"].ToString());

                    for (int n = 0; n < dvCats3.Count; n++)
                    {
                        if (strOfCategories != "")
                        {
                            strOfCategories += " OR ";
                        }
                        strOfCategories += " ACM.CategoryID=" + dvCats3[n]["ID"].ToString();
                    }
                }

            }



            if (strOfCategories.Trim() != "")
            {
                strOfCategories = "(ACM.CategoryID=" + lostAndFoundCategory.ToString() + " OR " + strOfCategories + ")";
            }
            else
            {
                strOfCategories += " ACM.CategoryID=" + lostAndFoundCategory.ToString();
            }

            command = "SELECT * FROM Ads A, Ad_Category_Mapping ACM, Ad_Calendar AC WHERE " +
                "AC.AdID=A.Ad_ID AND AC.DateTimeStart <=  CONVERT(DATETIME,'" + timeNow.ToString() + "') AND " +
                " ACM.AdID=A.Ad_ID AND (" + strOfCategories + displayToAll + ")" + adsExclude + includeInAll;
            
            //command = "SELECT * FROM Ads A, UserCategories UC, Ad_Category_Mapping ACM, Ad_Calendar AC WHERE " +
            //    "AC.AdID=A.Ad_ID AND AC.DateTimeStart <=  CONVERT(DATETIME,'" + timeNow.ToString() + "') " + displayToAll +
            //    " ACM.AdID=A.Ad_ID AND " +
            //    " ((ACM.ParentID=UC.CategoryID AND UC.UserID=" + UserID + ") OR (ACM.CategoryID=UC.CategoryID AND UC.UserID=" +
            //        UserID + ") OR ACM.CategoryID=" +
            //    lostAndFoundCategory.ToString() + ") " + adsExclude + includeInAll;

            Session["command"] = command;
        DataSet returnDS = GetDataWithParemeters(command, types, data);
        DataView returnDV = new DataView(returnDS.Tables[0], "", "", DataViewRowState.CurrentRows);

        //count = returnDV.Count;

        return returnDS;
    }

    public DataSet MakeSureDSIsUnique(DataSet adsFinal)
    {
        DataView dv = new DataView(adsFinal.Tables[0], "", "", DataViewRowState.CurrentRows);

        string ads = "";

        Hashtable adsHash = new Hashtable();

        for (int i = 0; i < dv.Count; i++)
        {
            if (!adsHash.Contains(dv[i]["Ad_ID"].ToString()))
            {
                ads += dv[i]["Ad_ID"].ToString() + ";";
                adsHash.Add(dv[i]["Ad_ID"].ToString(), "1");
            }
        }

        return GetAdsBasedOnString(ads);
    }

    public DataView RetrieveAllUserAds(bool isBig, string UserID, string city, 
        string state, string country, bool onlyOnLocation, DateTime startDate)
    {
        //Get only user's category ads

        DataSet adsOnCategoryNLocation1 = GetCategoryAds(UserID, isBig, city, state, country, onlyOnLocation, startDate);

        return new DataView(adsOnCategoryNLocation1.Tables[0], "", "", DataViewRowState.CurrentRows);
    }

    //Call whenever need to increment ad count 
    public void CountAd(bool isBig, DataView dvNotUsed)
    {
        //Get the next 4 adIDs to update judging by the last seen ad

        DataView dv = GetDataDV("SELECT AdSet, LastSeenIndex FROM UserAds WHERE UserID=" +
            Session["User"].ToString() + " AND CONVERT(NVARCHAR,Month([Date])) + '/' + " +
            "CONVERT(NVARCHAR, DAY([Date])) + " +
            "'/' + CONVERT(NVARCHAR, YEAR([Date])) ='" + timeNow.Date.ToShortDateString().Replace("12:00:00 AM", "").Trim() +
                "' AND BigAd='" + isBig.ToString() + "'");
        
        //Sometimes we could be transitioning into the next day but still have the ads of
        //previous day here
        if (dv.Count == 0)
        {
            dv = GetDataDV("SELECT AdSet, LastSeenIndex FROM UserAds WHERE UserID=" +
            Session["User"].ToString() + " AND CONVERT(NVARCHAR,Month([Date])) + '/' + " +
            "CONVERT(NVARCHAR, DAY([Date])) + " +
            "'/' + CONVERT(NVARCHAR, YEAR([Date])) ='" +
                timeNow.AddDays(double.Parse("-1")).Date.ToShortDateString().Replace("12:00:00 AM", "").Trim() +
                "' AND BigAd='" + isBig.ToString() + "'");
        }

        if (dv.Count != 0)
        {

            string AdSet = "";
            string AdID1 = "";
            string AdID2 = "";
            string AdID3 = "";
            string AdID4 = "";

            AdSet = dv[0]["AdSet"].ToString();
            int lastSeenIndex = int.Parse(dv[0]["LastSeenIndex"].ToString());

            char[] delim = { ';' };
            string[] tokens = AdSet.Split(delim);

            int a1 = 0;
            int a2 = 0;
            int a3 = 0;
            int a4 = 0;

            if (isBig)
            {
                if (lastSeenIndex == numberTotalBigAds - 1)
                    a1 = 0;
                else
                    a1 = lastSeenIndex + 1;
            }
            else
            {
                int adNumber = numberTotalAds;


                int a = adNumber - lastSeenIndex - 1;
                if (a < 4)
                {
                    if (a == 0)
                    {
                        a1 = 0;
                        a2 = 1;
                        a3 = 2;
                        a4 = 3;
                    }
                    else if (a == 1)
                    {
                        a1 = lastSeenIndex + 1;
                        a2 = 0;
                        a3 = 1;
                        a4 = 2;
                    }
                    else if (a == 2)
                    {
                        a1 = lastSeenIndex + 1;
                        a2 = lastSeenIndex + 2;
                        a3 = 0;
                        a4 = 1;
                    }
                    else if (a == 3)
                    {
                        a1 = lastSeenIndex + 1;
                        a2 = lastSeenIndex + 2;
                        a3 = lastSeenIndex + 3;
                        a4 = 0;
                    }
                }
                else
                {

                    a1 = lastSeenIndex + 1;
                    a2 = lastSeenIndex + 2;
                    a3 = lastSeenIndex + 3;
                    a4 = lastSeenIndex + 4;

                }
            }

            AdID1 = tokens[a1];

            if (!isBig)
            {
                AdID2 = tokens[a2];
                AdID3 = tokens[a3];
                AdID4 = tokens[a4];
            }


            CountAd(int.Parse(AdID1), a1, isBig);

            if (!isBig)
            {
                CountAd(int.Parse(AdID2), a2, isBig);
                CountAd(int.Parse(AdID3), a3, isBig);
                CountAd(int.Parse(AdID4), a4, isBig);
            }
        }
    }

    public void CountAdAnonymous(bool isBig, DataView dvNotUsed)
    {
        //Get the next 4 adIDs to update judging by the last seen ad

        string tableToUse = "BigAdStatistics_Anonymous";
        if (!isBig)
            tableToUse = "AdStatistics_Anonymous";

        string userSessionToUse = "AnonymousUser";
        if (isBig)
            userSessionToUse = "BigAnonymousUser";


        DataView dv = GetDataDV("SELECT AdsList, LastSeenIndex FROM " + tableToUse + " " +
        "WHERE SessionNum='" + Session[userSessionToUse].ToString() +
        "' AND CONVERT(NVARCHAR,Month([Date])) + '/' + CONVERT(NVARCHAR, DAY([Date])) + " +
        "'/' + CONVERT(NVARCHAR, YEAR([Date])) = '" + timeNow.Date.ToShortDateString().Replace("12:00:00 AM", "").Trim() +
        "' ");

        //Sometimes we could be transitioning into the next day but still have the ads of
        //previous day here
        if (dv.Count == 0)
        {
            dv = GetDataDV("SELECT AdsList, LastSeenIndex FROM " + tableToUse + " " +
                "WHERE SessionNum='" + Session[userSessionToUse].ToString() +
                "' AND CONVERT(NVARCHAR,Month([Date])) + '/' + CONVERT(NVARCHAR, DAY([Date])) + " +
        "'/' + CONVERT(NVARCHAR, YEAR([Date]))='" + 
        timeNow.AddDays(double.Parse("-1")).Date.ToShortDateString().Replace("12:00:00 AM", "").Trim() +
                "' ");
        }

        if (dv.Count != 0)
        {
            string AdSet = "";
            string AdID1 = "";
            string AdID2 = "";
            string AdID3 = "";
            string AdID4 = "";

            AdSet = dv[0]["AdsList"].ToString();
            int lastSeenIndex = int.Parse(dv[0]["LastSeenIndex"].ToString());

            char[] delim = { ';' };
            string[] tokens = AdSet.Split(delim);

            int a1 = 0;
            int a2 = 0;
            int a3 = 0;
            int a4 = 0;

            if (isBig)
            {
                if (lastSeenIndex == numberTotalBigAds - 1)
                    a1 = 0;
                else
                    a1 = lastSeenIndex + 1;
            }
            else
            {
                int adNumber = numberTotalAds;


                int a = adNumber - lastSeenIndex - 1;
                if (a < 4)
                {
                    if (a == 0)
                    {
                        a1 = 0;
                        a2 = 1;
                        a3 = 2;
                        a4 = 3;
                    }
                    else if (a == 1)
                    {
                        a1 = lastSeenIndex + 1;
                        a2 = 0;
                        a3 = 1;
                        a4 = 2;
                    }
                    else if (a == 2)
                    {
                        a1 = lastSeenIndex + 1;
                        a2 = lastSeenIndex + 2;
                        a3 = 0;
                        a4 = 1;
                    }
                    else if (a == 3)
                    {
                        a1 = lastSeenIndex + 1;
                        a2 = lastSeenIndex + 2;
                        a3 = lastSeenIndex + 3;
                        a4 = 0;
                    }
                }
                else
                {

                    a1 = lastSeenIndex + 1;
                    a2 = lastSeenIndex + 2;
                    a3 = lastSeenIndex + 3;
                    a4 = lastSeenIndex + 4;

                }
            }

            AdID1 = tokens[a1];
            Session["testmessage"] = AdSet;
            if (isBig)
            {
                CountAdAnonymous(int.Parse(AdID1), a1, isBig);
            }
            else
            {
                if (!isBig)
                {
                    AdID2 = tokens[a2];
                    AdID3 = tokens[a3];
                    AdID4 = tokens[a4];
                }


                CountAdAnonymous(int.Parse(AdID1), a1, isBig);

                if (!isBig)
                {
                    CountAdAnonymous(int.Parse(AdID2), a2, isBig);
                    CountAdAnonymous(int.Parse(AdID3), a3, isBig);
                    CountAdAnonymous(int.Parse(AdID4), a4, isBig);
                }
            }
        }

    }

    public void CountAd(int AdID, int index, bool isBig)
    {
        
            //Determine whether the ad has been seen
            //If not, 
            //update the last see ad as this ad 
            //and update AdStatistics
            //Also update Ads table for the number
            // of users who've seen the ad
            //If so, 
            //update the last see ad as this ad 


            //Determine whether ad has been seen
            DataSet dsAdStatistic = GetData("SELECT * FROM AdStatistics WHERE UserID=" +
                Session["User"].ToString() + " AND AdID=" + AdID.ToString());
            DataView dvAdStatistic = new DataView(dsAdStatistic.Tables[0], "", "", 
                DataViewRowState.CurrentRows);

            bool updateAdStatistics = false;

            if (dvAdStatistic.Count == 0)
            {
                updateAdStatistics = true;
            }

            //update the last see ad as this ad 
            //If the user is seeing ads, it means that a record has been created
            //for the UserID and date. don't need to insert new.
            Execute("UPDATE UserAds SET LastSeenIndex=" + index.ToString() + " WHERE UserID=" +
                Session["User"].ToString() + " AND BigAd='" + isBig.ToString() + "' AND CONVERT(NVARCHAR,Month([Date])) + " +
            "'/' + CONVERT(NVARCHAR, DAY([Date])) + " +
        "'/' + CONVERT(NVARCHAR, YEAR([Date])) ='" + timeNow.Date.ToShortDateString().Replace("12:00:00 AM", "").Trim() +
        "'");

            //Update AdStatistics
            //The Reason column is composed of categoryIDs dilimeted by a semi colon.
            if (updateAdStatistics)
            {
                //Get the data for Reason column and LocationOnly column to AdStatistics table
                DataSet dsCategories = GetData("SELECT * FROM Ad_Category_Mapping ACM, UserCategories UC " +
                    "WHERE ACM.AdID=" + AdID.ToString() + " AND ACM.CategoryID=UC.CategoryID AND UC.UserID=" +
                    Session["User"].ToString());

                //Make sure not counting hippo ads
                if (AdID != hippoAdCategory && AdID != hippoHalfAvailableCategory)
                {
                    Execute("UPDATE UserAds SET SeenAdSet = CASE WHEN (SeenAdSet IS NULL) THEN  ';" + 
                        AdID.ToString() + "' ELSE SeenAdSet + ';" + AdID.ToString() + "' END WHERE UserID=" +
                        Session["User"].ToString() + " AND BigAd='" + isBig.ToString() + "' AND CONVERT(NVARCHAR,Month([Date])) + " +
            "'/' + CONVERT(NVARCHAR, DAY([Date])) + " +
        "'/' + CONVERT(NVARCHAR, YEAR([Date])) ='" + timeNow.Date.ToShortDateString().Replace("12:00:00 AM", "").Trim() +
        "'");

                }
                DataView dvCategories = new DataView(dsCategories.Tables[0], "", "", DataViewRowState.CurrentRows);

                string categories = "";
                for (int i = 0; i < dvCategories.Count; i++)
                {
                    categories += dvCategories[i]["CategoryID"].ToString() + ";";
                }

                bool locationOnly = false;
                if (categories == "")
                {
                    locationOnly = true;
                }

                Execute("INSERT INTO AdStatistics (UserID, Date, AdID, Reason, LocationOnly) " +
                    "VALUES(" + Session["User"].ToString() + ", '" + timeNow.Date.ToString() + "', " +
                    AdID + ", '" + categories + "', '" + locationOnly.ToString() + "')");

                //Get number of users to see this ad and increment by one.
                //DataSet dsAd = GetData("SELECT * FROM Ads WHERE Ad_ID=" + AdID);
                //DataView dvAd = new DataView(dsAd.Tables[0], "", "", DataViewRowState.CurrentRows);
                //int numCurrentViews = int.Parse(dvAd[0]["NumCurrentViews"].ToString());

                //Make sure not to increment hippo ads
                if (AdID != hippoAdCategory && AdID != hippoHalfAvailableCategory)
                {
                    Execute("UPDATE Ads SET NumCurrentViews= NumCurrentViews + 1 WHERE Ad_ID=" + AdID.ToString());
                }

                DataView dvUpdatedAd = GetDataDV("SELECT * FROM Ads WHERE Ad_ID="+AdID.ToString());

                int sentAdCount = int.Parse(dvUpdatedAd[0]["NumCurrentViews"].ToString());
                int totalAdCount = int.Parse(dvUpdatedAd[0]["NumViews"].ToString());

                //If we have already reached the total ad count that this user has paid for, notify the user.
                if (sentAdCount >= totalAdCount)
                {
                    SendAdFinishedEmail(AdID.ToString());
                }
            }
        
    }

    public void CountAdAnonymous(int AdID, int index, bool isBig)
    {
        //Determine whether ad has been seen
        //-if there is no IP address associated, count it under that IP
        //-if there is an IP address, count it as that UserID… along with an anonymous session and IP

        string userSessionToUse = "AnonymousUser";
        if (isBig)
            userSessionToUse = "BigAnonymousUser";

        string wheres2 = " SessionNum='" + Session[userSessionToUse].ToString() + 
            "' AND IP='" + GetIP() + "'";

        string tableToUse = "BigAdStatistics_Anonymous";
        if (!isBig)
            tableToUse = "AdStatistics_Anonymous";

        
        DataView dvAdStatistic = GetDataDV("SELECT * FROM AdStatistics WHERE IP = '" +
            GetIP() + "' AND AdID=" + AdID.ToString());

        DataView dvAdStatisticUser = GetDataDV("SELECT * FROM Users U, AdStatistics A " +
            "WHERE U.IPs LIKE '%;" + GetIP() + ";%' AND A.UserID=U.User_ID AND A.IP='" + 
            GetIP() + "' AND A.AdID=" + AdID.ToString());

        bool updateAdStatistics = false;

        if (dvAdStatistic.Count == 0 && dvAdStatisticUser.Count == 0)
        {
            updateAdStatistics = true;
        }


        //update the last see ad as this ad 
        //If the user is seeing ads, it means that a record has been created
        //for the UserID and date. don't need to insert new.
        Execute("UPDATE " + tableToUse + " SET LastSeenIndex=" + index.ToString() +
            " WHERE UserID is NULL AND IP = '" +
            GetIP() + "' AND SessionNum='" + Session[userSessionToUse].ToString() +
            "'  AND CONVERT(NVARCHAR,Month([Date])) + " +
            "'/' + CONVERT(NVARCHAR, DAY([Date])) + " +
        "'/' + CONVERT(NVARCHAR, YEAR([Date])) ='" + timeNow.Date.ToShortDateString().Replace("12:00:00 AM", "").Trim() +
        "'");

        //Update AdStatistics
        //The Reason column is composed of categoryIDs dilimeted by a semi colon.
        if (updateAdStatistics)
        {

            //Make sure not counting hippo ads
            if (AdID != hippoAdCategory && AdID != hippoHalfAvailableCategory)
            {
                Execute("UPDATE " + tableToUse + " SET AdsSeen = CASE WHEN (AdsSeen IS NULL) THEN  ';" +
                    AdID.ToString() + "' ELSE AdsSeen + ';" + AdID.ToString() + "' END WHERE UserID is NULL AND IP = '" +
                    GetIP() + "' AND SessionNum='" + Session[userSessionToUse].ToString() +
                    "' AND " +
                    " CONVERT(NVARCHAR,Month([Date])) + " +
                    "'/' + CONVERT(NVARCHAR, DAY([Date])) + " +
                    "'/' + CONVERT(NVARCHAR, YEAR([Date])) ='" + timeNow.Date.ToShortDateString().Replace("12:00:00 AM", "").Trim() +
                    "'");
            }

            bool locationOnly = true;

            DataView dvUser = GetDataDV("SELECT * FROM Users WHERE IPs LIKE '%;" + GetIP() + ";%'");

            string thereason = "";
            string isLocation = "True";
            if (dvUser.Count == 0)
            {
                thereason = "NonAssigned";
                isLocation = "False";
            }

            Execute("INSERT INTO AdStatistics (Date, Reason, AdID, LocationOnly, IP, AnnonymousSessionNum) " +
                "VALUES('" + timeNow.Date.ToString() + "', '" + thereason + "'," +
                AdID + ", '" + isLocation + "', '" + GetIP() +
                "', '" + Session[userSessionToUse].ToString() + "')");


            //Get number of users to see this ad and increment by one.
            //DataSet dsAd = GetData("SELECT * FROM Ads WHERE Ad_ID=" + AdID);
            //DataView dvAd = new DataView(dsAd.Tables[0], "", "", DataViewRowState.CurrentRows);
            //int numCurrentViews = int.Parse(dvAd[0]["NumCurrentViews"].ToString());

            //Make sure not to increment hippo ads
            if (AdID != hippoAdCategory && AdID != hippoHalfAvailableCategory)
            {
                Execute("UPDATE Ads SET NumCurrentViews = NumCurrentViews + 1 WHERE Ad_ID=" + AdID.ToString());
            }

            DataView dvUpdatedAd = GetDataDV("SELECT * FROM Ads WHERE Ad_ID=" + AdID.ToString());

            int sentAdCount = int.Parse(dvUpdatedAd[0]["NumCurrentViews"].ToString());
            int totalAdCount = int.Parse(dvUpdatedAd[0]["NumViews"].ToString());

            //If we have already reached the total ad count that this user has paid for, notify the user.
            if (sentAdCount >= totalAdCount)
            {
                SendAdFinishedEmail(AdID.ToString());
            }
        }

    }

    public void CountGroupAds(int AdID)
    {
        //Determine whether ad has been seen
        //-if there is no IP address associated, count it under that IP
        //-if there is an IP address, count it as that UserID… along with an anonymous session and IP

        string tableToUse = "BigAdStatistics_Anonymous";


        DataView dvAdStatistic = new DataView();

        if (Session["User"] != null)
        {
            dvAdStatistic = GetDataDV("SELECT * FROM AdStatistics WHERE UserID = " +
            Session["User"].ToString() + " AND AdID=" + AdID.ToString());
        }
        else
        {
            dvAdStatistic = GetDataDV("SELECT * FROM AdStatistics WHERE UserID is NULL AND IP = '" +
            GetIP() + "' AND AdID=" + AdID.ToString());
        }

        bool updateAdStatistics = false;

        if (dvAdStatistic.Count == 0)
        {
            updateAdStatistics = true;
        }

        //Update AdStatistics
        //The Reason column is composed of categoryIDs dilimeted by a semi colon.
        if (updateAdStatistics)
        {
            //Get number of users to see this ad and increment by one.
            //DataSet dsAd = GetData("SELECT * FROM Ads WHERE Ad_ID=" + AdID);
            //DataView dvAd = new DataView(dsAd.Tables[0], "", "", DataViewRowState.CurrentRows);
            //int numCurrentViews = int.Parse(dvAd[0]["NumCurrentViews"].ToString());

            //Make sure not to increment hippo ads
            if (AdID != hippoAdCategory && AdID != hippoHalfAvailableCategory)
            {
                if (Session["User"] != null)
                {
                    Execute("INSERT INTO AdStatistics (Date, Reason, AdID, LocationOnly, UserID, isGroup) " +
                    "VALUES('" + timeNow.Date.ToString() + "', ''," +
                    AdID.ToString() + ", 'True', " + Session["User"].ToString() + ", 'True')");
                }
                else
                {
                    Execute("INSERT INTO AdStatistics (Date, Reason, AdID, LocationOnly, IP, isGroup) " +
                    "VALUES('" + timeNow.Date.ToString() + "', ''," +
                    AdID.ToString() + ", 'True', '" + GetIP() + "', 'True')");
                }
                Execute("UPDATE Ads SET NumCurrentViews = NumCurrentViews + 1 WHERE Ad_ID=" + AdID.ToString());
            }

            DataView dvUpdatedAd = GetDataDV("SELECT * FROM Ads WHERE Ad_ID=" + AdID.ToString());

            int sentAdCount = int.Parse(dvUpdatedAd[0]["NumCurrentViews"].ToString());
            int totalAdCount = int.Parse(dvUpdatedAd[0]["NumViews"].ToString());

            //If we have already reached the total ad count that this user has paid for, notify the user.
            if (sentAdCount >= totalAdCount)
            {
                SendAdFinishedEmail(AdID.ToString());
            }
        }

    }

    public DataSet GetAdsOnCategoryAndLocation(bool isBig, bool onCity, bool onState, 
        bool onCategories, ref int count, bool onDisplayToNonCategories, 
        string theUser, DataView allUsers, bool notTodays)
    {
        DateTime date = DateTime.Today;
        DataSet dsPrefs = GetData("SELECT * FROM UserPreferences WHERE UserID=" + theUser);

        string compareState = "";
        string compareCity = "";
        string compareCountry = " AND A.CatCountry=" + dsPrefs.Tables[0].Rows[0]["CatCountry"].ToString();
        SqlDbType[] types;
        object[] data;
        if (dsPrefs.Tables[0].Rows[0]["CatState"].ToString() != null && onState)
        {
            compareState = " AND A.CatState = @p0 ";

            if (dsPrefs.Tables[0].Rows[0]["CatCity"].ToString() != null && onCity)
            {
                compareCity = " AND A.CatCity = @p1 ";
                types = new SqlDbType[2];
                data = new object[2];
                types[0] = SqlDbType.NVarChar;
                types[1] = SqlDbType.NVarChar;
                data[0] = dsPrefs.Tables[0].Rows[0]["CatState"].ToString();
                data[1] = dsPrefs.Tables[0].Rows[0]["CatCity"].ToString();
            }
            else
            {
                types = new SqlDbType[1];
                data = new object[1];
                types[0] = SqlDbType.NVarChar;
                data[0] = dsPrefs.Tables[0].Rows[0]["CatState"].ToString();
            }
        }
        else
        {
            types = new SqlDbType[0];
            data = new object[0];
        }


        string command = "";

        //Select ad only in users category location
        //that is featured
        //that has not been posted by the user viewing this ad
        //that has been seen less numbers than it was paid for
        //that is live
        //that is big/normal based on the isBig boolean.

        //Get all seen ads to exclude in the command
        string excludeTodays = "";
        if (notTodays)
            excludeTodays = "AND CONVERT(NVARCHAR, MONTH([DATE])) +'/' + CONVERT(NVARCHAR, DAY([DATE])) +" +
            "'/' + CONVERT(NVARCHAR, YEAR([DATE])) <> '" + timeNow.Date.ToShortDateString().Replace("12:00:00 AM", "").Trim() + "'";
        DataSet dsExclude = GetData("SELECT AdID AS Ad_ID FROM AdStatistics WHERE UserID=" +
            theUser + " "+excludeTodays);
        DataView dvExclude = new DataView(dsExclude.Tables[0], "", "", DataViewRowState.CurrentRows);

        string adsExclude = "";

        for (int j = 0; j < dvExclude.Count; j++)
        {
            if (j == 0)
            {
                adsExclude += " AND (";
            }

            adsExclude += " A.Ad_ID <> "+dvExclude[j]["Ad_ID"].ToString();

            if (j != dvExclude.Count - 1)
                adsExclude += " AND ";
            else
                adsExclude += " ) ";

        }

        string includeInAll = "";
        if (allUsers != null)
        {
            includeInAll = " AND A.LIVE=1 AND A.Featured=1 AND A.User_ID <> " +
                 hippoHappeningsUserID.ToString() + "  ";
            foreach (DataRowView row in allUsers)
            {
                includeInAll += " AND A.User_ID <> " + row["User_ID"].ToString();
            }
            includeInAll += " AND A.NumCurrentViews < A.NumViews AND A.BigAd='" +
                     isBig.ToString() + "' " + compareCountry + compareState + compareCity +
                     " ORDER BY DateAdded ASC";
        }
        else
        {
            includeInAll = " AND A.LIVE=1 AND A.Featured=1 AND A.User_ID <> " +
                 hippoHappeningsUserID.ToString() + " AND A.User_ID <> " + theUser +
                 " AND A.NumCurrentViews < A.NumViews AND A.BigAd='" +
                     isBig.ToString() + "' " + compareCountry + compareState + compareCity +
                     " ORDER BY DateAdded ASC";
        }

        if (onCategories)
        {
            string displayToAll = " AND ";
            if (onDisplayToNonCategories)
                displayToAll = " AND  A.DisplayToAll='True' AND ";

            string strOfCategories = "";

            DataView dvCats = GetDataDV("SELECT * FROM UserCategories WHERE UserID=" + theUser);

            for (int i = 0; i < dvCats.Count; i++)
            {
                if (strOfCategories != "")
                {
                    strOfCategories += " OR ";
                }

                strOfCategories += " ACM.CategoryID=" + dvCats[i]["CategoryID"].ToString();
                DataView dvCats2 = GetDataDV("SELECT * FROM AdCategories WHERE ParentID=" + 
                    dvCats[i]["CategoryID"].ToString());
                for (int j = 0; j < dvCats2.Count; j++)
                {
                    if (strOfCategories != "")
                    {
                        strOfCategories += " OR ";
                    }
                    strOfCategories += " ACM.CategoryID=" + dvCats2[j]["ID"].ToString();

                    DataView dvCats3 = GetDataDV("SELECT * FROM AdCategories WHERE ParentID=" + 
                        dvCats2[j]["ID"].ToString());

                    for (int n = 0; n < dvCats3.Count; n++)
                    {
                        if (strOfCategories != "")
                        {
                            strOfCategories += " OR ";
                        }
                        strOfCategories += " ACM.CategoryID=" + dvCats3[n]["ID"].ToString();
                    }
                }

            }



            if (strOfCategories.Trim() != "")
            {
                strOfCategories = "(ACM.CategoryID=" + lostAndFoundCategory.ToString() + " OR " + 
                    strOfCategories + ")";
            }
            else
            {
                strOfCategories += " ACM.CategoryID=" + lostAndFoundCategory.ToString();
            }

            command = "SELECT * FROM Ads A, Ad_Category_Mapping ACM, Ad_Calendar AC WHERE " +
                "AC.AdID=A.Ad_ID AND AC.DateTimeStart <=  CONVERT(DATETIME,'" + timeNow.ToString() + 
                "') " + displayToAll +
                " ACM.AdID=A.Ad_ID AND " + strOfCategories + adsExclude + includeInAll;
            
            //command = "SELECT * FROM Ads A, UserCategories UC, Ad_Category_Mapping ACM, Ad_Calendar AC WHERE " +
            //    "AC.AdID=A.Ad_ID AND AC.DateTimeStart <=  CONVERT(DATETIME,'" + timeNow.ToString() + "') " + displayToAll +
            //    " ACM.AdID=A.Ad_ID AND " +
            //    " ((ACM.ParentID=UC.CategoryID AND UC.UserID=" + Session["User"].ToString() + ") OR (ACM.CategoryID=UC.CategoryID AND UC.UserID=" +
            //        Session["User"].ToString() + ") OR ACM.CategoryID=" +
            //    lostAndFoundCategory.ToString() + ") " + adsExclude + includeInAll;
        }
        else
        {

            string displayToAll = "";
            if (onDisplayToNonCategories)
                displayToAll = " AND  A.DisplayToAll='True' ";
            command = "SELECT * FROM Ads A, Ad_Calendar AC WHERE AC.AdID=A.Ad_ID AND AC.DateTimeStart "+
                "<=  CONVERT(DATETIME,'"+timeNow.ToString()+"') " + displayToAll + adsExclude + includeInAll;
            
        }
       
        DataSet returnDS = GetDataWithParemeters(command, types, data);
        DataView returnDV = new DataView(returnDS.Tables[0], "", "", DataViewRowState.CurrentRows);

        count = returnDV.Count;

        return returnDS;
    }

    public DataSet GetCategoryAds(string UserID, bool isBig, string city, string state,
        string country, bool onlyOnLocation, DateTime startDate)
    {
        DateTime date = DateTime.Today;

        string compareState = "";
        string compareCity = "";
        string compareCountry = " AND A.CatCountry=" + country;
        SqlDbType[] types;
        object[] data;

        compareCity = " AND A.CatCity = @p1 ";
        types = new SqlDbType[2];
        data = new object[2];
        types[0] = SqlDbType.NVarChar;
        types[1] = SqlDbType.NVarChar;
        data[0] = state;
        data[1] = city;

        //if onlyOnLocation, we need to grab both category ad and location ads
        //this is because, any ads that are posted before this one (based on date, and if the same date, 
        //based on who first posted) will fill in the location ad space of a user first.
        //however, we still count the category ads first before any location ads
        //if not onlyOnLocation we only get category ads

        string command = "";

        string command2 = "";

        //Select ad only in users category location
        //that is featured
        //that has not been posted by the user viewing this ad
        //that has been seen less numbers than it was paid for
        //that is live
        //that is big/normal based on the isBig boolean.

        //Get all seen ads to exclude in the command
        DataSet dsExclude = GetData("SELECT AdID AS Ad_ID FROM AdStatistics WHERE UserID=" + UserID +
            " AND CONVERT(NVARCHAR, MONTH([DATE])) +'/' + " +
            "CONVERT(NVARCHAR, DAY([DATE])) +" +
            "'/' + CONVERT(NVARCHAR, YEAR([DATE])) = < '" + timeNow.Date.ToShortDateString().Replace("12:00:00 AM", "").Trim() + "'");
        DataView dvExclude = new DataView(dsExclude.Tables[0], "", "", DataViewRowState.CurrentRows);

        string adsExclude = "";

        if (dvExclude.Count > 0)
        {
            adsExclude = " AND (";

            for (int i = 0; i < dvExclude.Count; i++)
            {
                adsExclude += " A.Ad_ID <> " + dvExclude[i]["Ad_ID"].ToString();

                if (i < dvExclude.Count - 1)
                    adsExclude += " AND ";
                else
                    adsExclude += " ) ";
            }
        }

        string includeInAll = " AND Ad.AdID=A.Ad_ID AND Ad.DateTimeStart <= CONVERT(DATETIME,'" +
            startDate.ToShortDateString() + "') AND A.LIVE=1 AND A.Featured=1 AND A.User_ID <> " +
            UserID + " AND A.User_ID <> " + hippoHappeningsUserID.ToString() + " AND A.NumCurrentViews < A.NumViews AND A.BigAd='" +
                isBig.ToString() + "' " + compareCountry + compareState + compareCity + " ORDER BY DateAdded ASC";

        string displayToAll = "";


        command = "SELECT * FROM Ads A, Ad_Calendar Ad, UserCategories UC, Ad_Category_Mapping ACM WHERE " + displayToAll + " ACM.AdID=A.Ad_ID AND " +
            " ((ACM.CategoryID=UC.CategoryID AND UC.UserID=" + UserID + ") OR ACM.CategoryID=" +
            lostAndFoundCategory.ToString() + ") " + adsExclude + includeInAll;

        DataSet returnDS = GetDataWithParemeters(command, types, data);
        DataView returnDV = new DataView(returnDS.Tables[0], "", "", DataViewRowState.CurrentRows);

        //for onlyOnLocation flag
        if (onlyOnLocation)
        {
            //get all ads in the user's location that are not in the user's categories

            string adsExcludeCategories = "";

            if (returnDV.Count > 0)
            {
                adsExcludeCategories = " AND (";

                for (int i = 0; i < returnDV.Count; i++)
                {
                    adsExcludeCategories += " A.Ad_ID <> " + returnDV[i]["Ad_ID"].ToString();

                    if (i < returnDV.Count - 1)
                        adsExcludeCategories += " AND ";
                    else
                        adsExcludeCategories += " ) ";
                }
            }

            displayToAll = " A.DisplayToAll = 'True' ";


            command2 = "SELECT * FROM Ads A, Ad_Calendar Ad WHERE " + displayToAll + adsExclude + includeInAll;

            returnDS = GetDataWithParemeters(command2, types, data);
            returnDV = new DataView(returnDS.Tables[0], "", "", DataViewRowState.CurrentRows);
        }

        return returnDS;
    }

    public DataView FillAdSet(DataSet dsAds, bool isBig)
    {
        //1.	If less than or equal to 52, add the ads 
        //in a second time. If less than 35, add the ads three times, 
        //if less than 26 four times, less than 21 five times, less than 
        //17 six times, less than 15 seven times, less than 13 eight times, 
        //    less than 11 nine times, less than 10 ten times, less than 9 eleven 
        //        times, less than 8 thirteen times, less than 7 fifteen times, 
        //            less than 6 seventeen times, less than 5 twenty one times, 
        //less than 4 twenty six times, 3 thirty times.
        //2.	Fill the rest with Hippo Happenings ad stating that there’s a discount, 
        //meaning you will see your ad more frequently for the same price.

        DataView dvAds = new DataView(dsAds.Tables[0], "", "", DataViewRowState.CurrentRows);

        //If ad count is as desired, return the original dataset
        int adNumberToGet = numberTotalAds;

        if (isBig)
            adNumberToGet = numberTotalBigAds;

        if (dvAds.Count == adNumberToGet)
        {
            return dvAds;
        }
        //If ad count is bigger than desired, trim the dataset
        else if (dvAds.Count > adNumberToGet)
        {
            for (int i = adNumberToGet; i < dvAds.Count - adNumberToGet; i++)
            {
                dvAds.Delete(i);
            }
            return dvAds;
        }
        //If ad set is zero, fill all with hippo happenings ads
        else if (dvAds.Count == 0)
        {
            ArrayList a = new ArrayList(adNumberToGet);
            for (int i = 0; i < adNumberToGet; i++)
            {
                if (i % 2 == 0)
                {
                    a.Insert(i, hippoHalfAvailableCategory);
                }
                else
                {
                    a.Insert(i, hippoAdCategory);
                }
            }

            DataSet ab = GetAdsBasedOnArray(a);
            DataView dvAB = new DataView(ab.Tables[0], "", "", DataViewRowState.CurrentRows);
            return dvAB;
        }
        //If ad count is less than desired, add hippo ads and multiply the ads whever probable
        else
        {
            ArrayList a = new ArrayList(adNumberToGet);

            //Fill with ads that state half of the space is available so your ad will be seen 
            //more than once for the same price.
            bool fillWithHalf = false;

            int toDivideBy = 4;

            if (isBig)
                toDivideBy = 2;

            int temp = adNumberToGet / toDivideBy;

            //subtract one of the ads for every four/two since these will be filled with hippo ads.
            int temp2 = adNumberToGet - temp;

            int temp3 = temp2 / 2;

            if (dvAds.Count <= temp3)
            {
                fillWithHalf = true;
            }

            if (fillWithHalf)
            {
                int repeatNum = temp2 / dvAds.Count;

                int repeatNumCount = 1;

                int threeOrOne = 3;

                if (isBig)
                    threeOrOne = 1;

                int countAds = 0;

                for (int j = 0; j < adNumberToGet; j++)
                {
                    //when there is less than or equal to a half minus hippo ads
                    //fill the hippo add every fourt ad, 
                    //repeat the ads the number of times all of them can repeat in full
                    //when reach the point where they can't all repeat, fill the rest with hippo ads
                    if (j % toDivideBy == threeOrOne)
                    {
                        a.Insert(j,hippoHalfAvailableCategory);
                    }
                    else
                    {
                        if (repeatNumCount <= repeatNum)
                        {
                            a.Insert(j, dvAds[countAds]["Ad_ID"]);
                            countAds++;
                            if (countAds == dvAds.Count)
                            {
                                repeatNumCount++;
                                countAds = 0;
                            }
                        }
                        else
                        {
                            a.Insert(j, hippoHalfAvailableCategory);
                        }
                    }
                }
            }
            else
            {
                for (int j = 0; j < adNumberToGet; j++)
                {
                    //when there is more than half - hippo ads, fill ads from the beginning
                    //Hippo ads will go in the back
                    if (j < dvAds.Count)
                    {
                        a.Insert(j, dvAds[j]["Ad_ID"]);
                    }
                    else
                    {
                        a.Insert(j, hippoAdCategory);
                    }
                }
            }
            DataSet ab = GetAdsBasedOnArray(a);
            DataView dvAB = new DataView(ab.Tables[0], "", "", DataViewRowState.CurrentRows);
            return dvAB;
        }
    }
    
    public string GetAdStringBasedOnDS(DataView dv, ref string lastSeen)
    {
        //dv.Sort = "DateAdded ASC";
        string returnStr = "";
        for (int i = 0; i < dv.Count; i++)
        {
            //if (i == 0)
            //    lastSeen = dv[i]["Ad_ID"].ToString();
            returnStr += dv[i]["Ad_ID"].ToString() + ";";
        }

        if (lastSeen == "")
            lastSeen = "0";

        return returnStr;
    }

    public DataSet GetAdsBasedOnArray(ArrayList a)
    {
        DataSet ds;
        int initialIndex = 0;
        if (a[0].ToString().Trim() == "")
            initialIndex = 1;
        ds = GetData("SELECT * FROM Ads A WHERE A.Ad_ID=" + a[initialIndex]);
        DataView dv;
        DataRow dr;
        for (int i = (initialIndex + 1); i < a.Count; i++)
        {
            if (a[i].ToString().Trim() != "")
            {
                dv = GetDataDV("SELECT * FROM Ads A WHERE A.Ad_ID=" + a[i]);
                dr = ds.Tables[0].NewRow();
                dr["Ad_ID"] = dv[0]["Ad_ID"].ToString();
                dr["User_ID"] = dv[0]["User_ID"].ToString();
                dr["FeaturedSummary"] = dv[0]["FeaturedSummary"].ToString();
                dr["Description"] = dv[0]["Description"].ToString();
                dr["Header"] = dv[0]["Header"].ToString();
                dr["Featured"] = dv[0]["Featured"].ToString();
                dr["FeaturedPicture"] = dv[0]["FeaturedPicture"].ToString();
                dr["CatCountry"] = dv[0]["CatCountry"].ToString();
                dr["CatState"] = dv[0]["CatState"].ToString();
                dr["CatCity"] = dv[0]["CatCity"].ToString();
                dr["LIVE"] = dv[0]["LIVE"].ToString();
                dr["BigAd"] = dv[0]["BigAd"].ToString();
                ds.Tables[0].Rows.Add(dr);
            }
        }
        return ds;
    }

    public DataSet GetAdsBasedOnString(string ads)
    {
        string command = "";

        char[] delim = { ';' };
        string[] tokens = ads.Split(delim);
        
        

        if (ads.Trim() == "")
        {
            DataSet dsNew = new DataSet();
            dsNew.Tables.Add();

            DataView dvNew = FillAdSet(dsNew, false);

            DataSet dsReturn = new DataSet();
            dsNew.Tables.Add(dvNew.ToTable());

            return dsReturn;
        }

        return GetAdsBasedOnArray(tokens);
    }

    public DataSet GetAdsBasedOnArray(Array a)
    {
        DataSet ds;
        int initialIndex = 0;
        if (a.GetValue(0).ToString().Trim() == "")
            initialIndex = 1;
        
            ds = GetData("SELECT * FROM Ads A WHERE A.Ad_ID=" + a.GetValue(initialIndex));
            DataView dv;
            DataRow dr;
            for (int i = (initialIndex + 1); i < a.Length; i++)
            {
                if (a.GetValue(i).ToString().Trim() != "")
                {
                    dv = GetDataDV("SELECT * FROM Ads A WHERE A.Ad_ID=" + a.GetValue(i));
                    dr = ds.Tables[0].NewRow();
                    dr["Ad_ID"] = dv[0]["Ad_ID"].ToString();
                    dr["User_ID"] = dv[0]["User_ID"].ToString();
                    dr["FeaturedSummary"] = dv[0]["FeaturedSummary"].ToString();
                    dr["Description"] = dv[0]["Description"].ToString();
                    dr["Header"] = dv[0]["Header"].ToString();
                    dr["Featured"] = dv[0]["Featured"].ToString();
                    dr["FeaturedPicture"] = dv[0]["FeaturedPicture"].ToString();
                    dr["CatCountry"] = dv[0]["CatCountry"].ToString();
                    dr["CatState"] = dv[0]["CatState"].ToString();
                    dr["CatCity"] = dv[0]["CatCity"].ToString();
                    dr["LIVE"] = dv[0]["LIVE"].ToString();
                    dr["BigAd"] = dv[0]["BigAd"].ToString();
                    ds.Tables[0].Rows.Add(dr);
                }
            }
       
        return ds;
    }

    public DataSet IntersectDS1withDS2(DataSet dsAds1, DataSet dsAds2)
    {
        Hashtable hash = new Hashtable();

        int temp = 0;

        int countInDS = 0;


        //Create the new DS
        DataSet ds = new DataSet();

        DataColumn col = new DataColumn();
        col.ColumnName = "Ad_ID";

        DataColumn col2 = new DataColumn();
        col2.ColumnName = "Header";

        DataColumn col4 = new DataColumn();
        col4.ColumnName = "DateAdded";

        DataColumn col5 = new DataColumn();
        col5.ColumnName = "FeaturedSummary";

        DataColumn col6 = new DataColumn();
        col6.ColumnName = "FeaturedPicture";

        ds.Tables.Add("table");
        ds.Tables["table"].Columns.Add(col);
        ds.Tables["table"].Columns.Add(col2);
        ds.Tables["table"].Columns.Add(col4);
        ds.Tables["table"].Columns.Add(col5);
        ds.Tables["table"].Columns.Add(col6);

        DataRow row;

        int i = 0;


        for (i = 0; i < dsAds1.Tables[0].Rows.Count; i++)
        {
            if (!hash.ContainsKey(dsAds1.Tables[0].Rows[i]["Ad_ID"].ToString()))
            {
                hash.Add(dsAds1.Tables[0].Rows[i]["Ad_ID"].ToString(), "1");
            }
        }

        for (i = 0; i < dsAds2.Tables[0].Rows.Count; i++)
        {
            if (hash.ContainsKey(dsAds2.Tables[0].Rows[i]["Ad_ID"].ToString()))
            {
                row = ds.Tables["table"].NewRow();
                row["Ad_ID"] = dsAds2.Tables[0].Rows[i]["Ad_ID"].ToString();
                row["Header"] = dsAds2.Tables[0].Rows[i]["Header"].ToString();
                row["FeaturedSummary"] = dsAds2.Tables[0].Rows[i]["FeaturedSummary"].ToString();
                row["FeaturedPicture"] = dsAds2.Tables[0].Rows[i]["FeaturedPicture"].ToString();
                row["DateAdded"] = dsAds2.Tables[0].Rows[i]["DateAdded"].ToString();

                ds.Tables["table"].Rows.Add(row);
            }
        }

        return ds;
    }

    public DataSet MergeAdSets(DataSet dsAds1, DataSet dsAds2, bool includeDuplicates)
    {
        Hashtable hash = new Hashtable();

        int temp = 0;

        int countInDS = 0;


        //Create the new DS
        DataSet ds = new DataSet();

        DataColumn col = new DataColumn();
        col.ColumnName = "Ad_ID";

        DataColumn col2 = new DataColumn();
        col2.ColumnName = "Header";

        DataColumn col4 = new DataColumn();
        col4.ColumnName = "DateAdded";

        DataColumn col5 = new DataColumn();
        col5.ColumnName = "FeaturedSummary";

        DataColumn col6 = new DataColumn();
        col6.ColumnName = "FeaturedPicture";

        ds.Tables.Add("table");
        ds.Tables["table"].Columns.Add(col);
        ds.Tables["table"].Columns.Add(col2);
        ds.Tables["table"].Columns.Add(col4);
        ds.Tables["table"].Columns.Add(col5);
        ds.Tables["table"].Columns.Add(col6);

        DataRow row;

        int i = 0;


        for (i = 0; i < dsAds1.Tables[0].Rows.Count; i++)
        {

            if (!hash.ContainsKey(dsAds1.Tables[0].Rows[i]["Ad_ID"].ToString()) || includeDuplicates)
            {
                if (!hash.ContainsKey(dsAds1.Tables[0].Rows[i]["Ad_ID"].ToString()))
                {
                    hash.Add(dsAds1.Tables[0].Rows[i]["Ad_ID"].ToString(), "1");
                }
                row = ds.Tables["table"].NewRow();
                row["Ad_ID"] = dsAds1.Tables[0].Rows[i]["Ad_ID"].ToString();
                row["Header"] = dsAds1.Tables[0].Rows[i]["Header"].ToString();
                row["FeaturedSummary"] = dsAds1.Tables[0].Rows[i]["FeaturedSummary"].ToString();
                row["FeaturedPicture"] = dsAds1.Tables[0].Rows[i]["FeaturedPicture"].ToString();
                row["DateAdded"] = dsAds1.Tables[0].Rows[i]["DateAdded"].ToString();

                ds.Tables["table"].Rows.Add(row);
            }

        }

        for (i = 0; i < dsAds2.Tables[0].Rows.Count; i++)
        {

            if (!hash.ContainsKey(dsAds2.Tables[0].Rows[i]["Ad_ID"].ToString()) || includeDuplicates)
            {
                if (!hash.ContainsKey(dsAds2.Tables[0].Rows[i]["Ad_ID"].ToString()))
                {
                    hash.Add(dsAds2.Tables[0].Rows[i]["Ad_ID"].ToString(), "1");
                }

                row = ds.Tables["table"].NewRow();
                row["Ad_ID"] = dsAds2.Tables[0].Rows[i]["Ad_ID"].ToString();
                row["Header"] = dsAds2.Tables[0].Rows[i]["Header"].ToString();
                row["FeaturedSummary"] = dsAds2.Tables[0].Rows[i]["FeaturedSummary"].ToString();
                row["FeaturedPicture"] = dsAds2.Tables[0].Rows[i]["FeaturedPicture"].ToString();
                row["DateAdded"] = dsAds2.Tables[0].Rows[i]["DateAdded"].ToString();

                ds.Tables["table"].Rows.Add(row);
            }

        }

        return ds;
    }

    protected DataSet MergeAdSetsFull(DataSet dsAds1, DataSet dsAds2, bool includeDuplicates)
    {
        

        Hashtable hash = new Hashtable();

        int temp = 0;

        int countInDS = 0;


        //Create the new DS
        DataSet ds = new DataSet();
        

        DataColumn col = new DataColumn();
        col.ColumnName = "Ad_ID";

        DataColumn col2 = new DataColumn();
        col2.ColumnName = "Header";

        DataColumn col4 = new DataColumn();
        col4.ColumnName = "DateAdded";

        DataColumn col5 = new DataColumn();
        col5.ColumnName = "FeaturedSummary";

        DataColumn col6 = new DataColumn();
        col6.ColumnName = "FeaturedPicture";

        DataColumn col7 = new DataColumn();
        col7.ColumnName = "User_ID";

        DataColumn col8 = new DataColumn();
        col8.ColumnName = "Description";

        DataColumn col9 = new DataColumn();
        col9.ColumnName = "Featured";

        DataColumn col10 = new DataColumn();
        col10.ColumnName = "CatCountry";

        DataColumn col11 = new DataColumn();
        col11.ColumnName = "CatState";

        DataColumn col12 = new DataColumn();
        col12.ColumnName = "CatCity";

        DataColumn col13 = new DataColumn();
        col13.ColumnName = "LIVE";

        DataColumn col14 = new DataColumn();
        col14.ColumnName = "BigAd";

        ds.Tables.Add("table");
        ds.Tables["table"].Columns.Add(col);
        ds.Tables["table"].Columns.Add(col2);
        ds.Tables["table"].Columns.Add(col4);
        ds.Tables["table"].Columns.Add(col5);
        ds.Tables["table"].Columns.Add(col6);
        ds.Tables["table"].Columns.Add(col7);
        ds.Tables["table"].Columns.Add(col8);
        ds.Tables["table"].Columns.Add(col9);
        ds.Tables["table"].Columns.Add(col10);
        ds.Tables["table"].Columns.Add(col11);
        ds.Tables["table"].Columns.Add(col12);
        ds.Tables["table"].Columns.Add(col13);
        ds.Tables["table"].Columns.Add(col14);
        DataRow row;

        int i = 0;


        for (i = 0; i < dsAds1.Tables[0].Rows.Count; i++)
        {

            if (!hash.ContainsKey(dsAds1.Tables[0].Rows[i]["Ad_ID"].ToString()) || includeDuplicates)
            {
                if (!hash.ContainsKey(dsAds1.Tables[0].Rows[i]["Ad_ID"].ToString()))
                {
                    hash.Add(dsAds1.Tables[0].Rows[i]["Ad_ID"].ToString(), "1");
                }
                row = ds.Tables["table"].NewRow();
                row["Ad_ID"] = dsAds1.Tables[0].Rows[i]["Ad_ID"].ToString();
                row["Header"] = dsAds1.Tables[0].Rows[i]["Header"].ToString();
                row["FeaturedSummary"] = dsAds1.Tables[0].Rows[i]["FeaturedSummary"].ToString();
                row["FeaturedPicture"] = dsAds1.Tables[0].Rows[i]["FeaturedPicture"].ToString();
                row["DateAdded"] = dsAds1.Tables[0].Rows[i]["DateAdded"].ToString();

                row["User_ID"] = dsAds1.Tables[0].Rows[i]["User_ID"].ToString();
                row["Description"] = dsAds1.Tables[0].Rows[i]["Description"].ToString();
                row["Featured"] = dsAds1.Tables[0].Rows[i]["Featured"].ToString();
                row["CatCountry"] = dsAds1.Tables[0].Rows[i]["CatCountry"].ToString();

                row["CatState"] = dsAds1.Tables[0].Rows[i]["CatState"].ToString();
                row["CatCity"] = dsAds1.Tables[0].Rows[i]["CatCity"].ToString();
                row["LIVE"] = dsAds1.Tables[0].Rows[i]["LIVE"].ToString();
                row["BigAd"] = dsAds1.Tables[0].Rows[i]["BigAd"].ToString();

                ds.Tables["table"].Rows.Add(row);
            }

        }

        for (i = 0; i < dsAds2.Tables[0].Rows.Count; i++)
        {

            if (!hash.ContainsKey(dsAds2.Tables[0].Rows[i]["Ad_ID"].ToString()) || includeDuplicates)
            {
                if (!hash.ContainsKey(dsAds2.Tables[0].Rows[i]["Ad_ID"].ToString()))
                {
                    hash.Add(dsAds2.Tables[0].Rows[i]["Ad_ID"].ToString(), "1");
                }

                row = ds.Tables["table"].NewRow();
                row["Ad_ID"] = dsAds2.Tables[0].Rows[i]["Ad_ID"].ToString();
                row["Header"] = dsAds2.Tables[0].Rows[i]["Header"].ToString();
                row["FeaturedSummary"] = dsAds2.Tables[0].Rows[i]["FeaturedSummary"].ToString();
                row["FeaturedPicture"] = dsAds2.Tables[0].Rows[i]["FeaturedPicture"].ToString();
                row["DateAdded"] = dsAds2.Tables[0].Rows[i]["DateAdded"].ToString();

                row["User_ID"] = dsAds2.Tables[0].Rows[i]["User_ID"].ToString();
                row["Description"] = dsAds2.Tables[0].Rows[i]["Description"].ToString();
                row["Featured"] = dsAds2.Tables[0].Rows[i]["Featured"].ToString();
                row["CatCountry"] = dsAds2.Tables[0].Rows[i]["CatCountry"].ToString();

                row["CatState"] = dsAds2.Tables[0].Rows[i]["CatState"].ToString();
                row["CatCity"] = dsAds2.Tables[0].Rows[i]["CatCity"].ToString();
                row["LIVE"] = dsAds2.Tables[0].Rows[i]["LIVE"].ToString();
                row["BigAd"] = dsAds2.Tables[0].Rows[i]["BigAd"].ToString();

                ds.Tables["table"].Rows.Add(row);
            }

        }

        return ds;
    }

    ///summary
    //    1.	Search for users in ad location
    //a.	Search for user in those categories
    //b.	For each one of those users, determine whether they have all their add space filled in
    //i.	Get all the ads that fall under them, take into account the date that the ads are 
    //      specified to start at and determine whether the person has any more space to see more ads.
    //ii.	Count each such persons and display to user on the add summary page before checkout.
    public void CalculateUsersAdCapacity(string City, string State, string Country, 
        string categories, DateTime startShowing, ref int firstDayUsersCount, ref int secDayUsersCount,
        ref int thirdDayUsersCount, ref int fourthDayUsersCount, ref bool isNoUsers, bool isBig, 
        bool onlyInLocation)
    {
        //Get all users in location
        DateTime date = DateTime.Today;
        string command = "SELECT DISTINCT UP.UserID FROM UserPreferences UP ";

        if (categories.Trim() != "")
        {
            command += ", UserCategories UC WHERE UP.CategoriesOnOff='True' " +
            "AND UC.UserID=UP.UserID AND";
        }
        else
        {
            command += " WHERE "; 
        }


        string command2 = "SELECT DISTINCT UP.UserID FROM UserPreferences UP WHERE UP.CategoriesOnOff='True' " +
            " AND ";
        string compareState = "";
        string compareCity = "";
        string compareCountry = " UP.CatCountry=" + Country;
        SqlDbType[] types;
        object[] data;
        if (State != null)
        {
            compareState = " AND UP.CatState = @p0 ";

            if (City != null)
            {
                compareCity = " AND UP.CatCity = @p1 ";
                types = new SqlDbType[2];
                data = new object[2];
                types[0] = SqlDbType.NVarChar;
                types[1] = SqlDbType.NVarChar;
                data[0] = State;
                data[1] = City;
            }
            else
            {
                types = new SqlDbType[1];
                data = new object[1];
                types[0] = SqlDbType.NVarChar;
                data[0] = State;
            }
        }
        else
        {
            types = new SqlDbType[0];
            data = new object[0];
        }

        command2 += " ( (" + compareCountry + compareState + compareCity + " ) OR (" + compareCountry + compareState + ") ) ";
        command += compareCountry + compareState + compareCity;

         //From those users, get all users under those categories
        if (categories.Trim() != "")
        {
            if (categories[categories.Length - 1] == ';')
            {
                categories = categories.Remove(categories.Length - 1);
            }

            char[] delim = { ';' };
            string[] tokens = categories.Split(delim);

            bool firstGotten1 = false;
            bool firstGotten2 = false;
            for (int i = 0; i < tokens.Length; i++)
            {
                if (tokens[i].Trim() != "")
                {
                    if (!firstGotten1)
                    {
                        command += " AND (";
                        firstGotten1 = true;
                    }

                    if (!firstGotten2)
                    {
                        //command2 += " AND (";
                        firstGotten2 = true;
                    }

                    command += " UC.CategoryID=" + tokens[i];
                    //command2 += " UC.CategoryID <> " + tokens[i];
                    if (i != tokens.Length - 1)
                    {
                        command += " OR ";
                        //command2 += " AND ";
                    }
                    else
                    {
                        command += " ) ";
                        //command2 += " ) ";
                    }
                }
                else
                {
                    if (i == tokens.Length - 1)
                    {
                        command += " ) ";
                        //command2 += " ) ";
                    }

                }
            }
        }
        

        DataSet dsUsersLocationAndCats = GetDataWithParemeters(command, types, data);
        DataView dvUsers = new DataView(dsUsersLocationAndCats.Tables[0], "", "", DataViewRowState.CurrentRows);

        string excludeFromLocationList = "";

        for (int i = 0; i < dvUsers.Count; i++)
        {
            excludeFromLocationList += " UP.UserID <> " + dvUsers[i]["UserID"].ToString();

                excludeFromLocationList += " AND ";
        }

        if (excludeFromLocationList != "")
        {
            excludeFromLocationList += " UP.UserID <> " + Session["User"].ToString();
            excludeFromLocationList = " AND ( " + excludeFromLocationList + ") ";
        }

        DataSet dsUsersLocationNotCats = GetDataWithParemeters(command2 + excludeFromLocationList, types, data);
        DataView dvUsersNotCats = new DataView(dsUsersLocationNotCats.Tables[0], "", "", DataViewRowState.CurrentRows);

        //Will separately query users only in location and not categories from usrs in the location
        //and in those categories. These need to be shown separately to the user.

        if (onlyInLocation)
        {
            if (dvUsersNotCats.Count > 0)
            {
                isNoUsers = false;
                IncrementAdCount(startShowing, ref firstDayUsersCount, ref secDayUsersCount, 
                    ref thirdDayUsersCount,ref fourthDayUsersCount, dvUsersNotCats, isBig, City, State, Country, onlyInLocation);

            }
            else
            {
                isNoUsers = true;
            }
        }
        else
        {
            if (dvUsers.Count > 0)
            {
                isNoUsers = false;

                //For each one of those users
                IncrementAdCount(startShowing, ref firstDayUsersCount, ref secDayUsersCount, ref thirdDayUsersCount,
                    ref fourthDayUsersCount, dvUsers, isBig, City, State, Country, onlyInLocation);
            }
            else
            {
                //If there are no users under these then return isNoUsers=true
                isNoUsers = true;
            }
        }
    }

    public void IncrementAdCount(DateTime startShowing, ref int firstDayUsersCount, 
        ref int secDayUsersCount, ref int thirdDayUsersCount, ref int fourthDayUsersCount, 
        DataView dvUsers, bool isBig, string city, string state, string country, bool OnlyOnLocation)
    {
        for (int i = 0; i < dvUsers.Count; i++)
        {
            //1. Get all their ads sorted by date
            DataView dvAds1 = RetrieveAllUserAds(isBig, dvUsers[i]["UserID"].ToString(), city, state, 
                country, OnlyOnLocation, startShowing);

            DataView dvAds2 = RetrieveAllUserAds(isBig, dvUsers[i]["UserID"].ToString(), city, state,
                country, OnlyOnLocation, startShowing.Date.AddDays(double.Parse("1.00")));
            DataView dvAds3 = RetrieveAllUserAds(isBig, dvUsers[i]["UserID"].ToString(), city, state,
                country, OnlyOnLocation, startShowing.Date.AddDays(double.Parse("2.00")));
            DataView dvAds4 = RetrieveAllUserAds(isBig, dvUsers[i]["UserID"].ToString(), city, state,
                country, OnlyOnLocation, startShowing.Date.AddDays(double.Parse("3.00")));

            bool isDayOneLessThanToday = false;
            bool isDayTwoLessThanToday = false;
            bool isDayThreeLessThanToday = false;
            bool isDayFourLessThanToday = false;

            if (startShowing < timeNow)
                isDayOneLessThanToday = true;
            if (startShowing.AddDays(double.Parse("1.00")).Date < timeNow.Date)
                isDayTwoLessThanToday = true;
            if (startShowing.AddDays(double.Parse("2.00")).Date < timeNow.Date)
                isDayThreeLessThanToday = true;
            if (startShowing.AddDays(double.Parse("3.00")).Date < timeNow.Date)
                isDayFourLessThanToday = true;

            int numDaysBeforeStardDate = GetDayDifference(startShowing, timeNow);
            int adNumberToGet = numberTotalAds;
            if (isBig)
                adNumberToGet = numberTotalBigAds;


            int countOnDay1 = 0;
            int countOnDay2 = 0;
            int countOnDay3 = 0;
            int countOnDay4 = 0;










            int a = 0;





            if (isDayFourLessThanToday)
            {

            }
            else if (isDayThreeLessThanToday)
            {
                int totalAdCount = dvAds1.Count + dvAds2.Count + dvAds3.Count + dvAds4.Count;
                

                a = totalAdCount / adNumberToGet;

                if (a >= 1)
                {

                }
                else
                {
                    fourthDayUsersCount++;
                }
            }
            else if (isDayTwoLessThanToday)
            {
                //first take care of day three
                int totalAdCount = dvAds1.Count + dvAds2.Count + dvAds3.Count;

                a = totalAdCount / adNumberToGet;

                if (a >= 2)
                {

                }
                else
                {
                    if (a != 1)
                        thirdDayUsersCount++;

                    //take care for the fourth day
                    totalAdCount = dvAds1.Count + dvAds2.Count + dvAds3.Count + dvAds4.Count - (a*adNumberToGet);

                    a = totalAdCount / adNumberToGet;

                    if (a >= 1)
                    {

                    }
                    else
                    {
                        fourthDayUsersCount++;
                    }
                }
            }
            else if (isDayOneLessThanToday)
            {
                //first, take care of day two
                int totalAdCount = dvAds1.Count + dvAds2.Count;

                a = totalAdCount / adNumberToGet;

                if (a >= 3)
                {

                }
                else
                {
                    if (a < 1)
                        secDayUsersCount++;

                    //take care of the third day
                    totalAdCount = dvAds1.Count + dvAds2.Count + dvAds3.Count - (a*adNumberToGet);
                    a = totalAdCount / adNumberToGet;

                    
                }
            }

            if (dvAds1.Count > 0)
            {
                //a. For each day up to the startShowing date, 
                //subtract numberTotalAds from the user's total ad count.

                

                

                

                //b. If the user's ad count comes up to be less than the numberTotalAds
                //count this user for this day.
                //c. If the user's ad count comes up to be more than or equal to numberTotalAds
                //don't count the user
                //2. If the user count for the startShowing date is more than 0
                //this day will be shown to the user posting the ad, along with the next 4 days.
                //3. If the count is not more than 0
                //check the next day and the next 4 days after the next day, but,
                //don't count startShowing date. Will have to tell the user at this point
                //that in their selected categories and location combination, there
                //is no users that can view the ads on the startShowing date. But, we'll show
                //the user what the first days are available and how may users can view them. 
                //Tell the user that this does not include anonymous users who don't log in
                //and therefore it is still possible for users to see this ad. However, advise
                //the user to perhaps choose different categories.

                
                    if (a <= numDaysBeforeStardDate)
                    {
                        if (isDayFourLessThanToday)
                        {

                        }
                        else if (isDayThreeLessThanToday)
                        {
                            fourthDayUsersCount++;
                        }
                        else if (isDayTwoLessThanToday)
                        {
                            thirdDayUsersCount++;
                            fourthDayUsersCount++;
                        }
                        else if (isDayOneLessThanToday)
                        {
                            secDayUsersCount++;
                            thirdDayUsersCount++;
                            fourthDayUsersCount++;
                        }
                        else
                        {
                            firstDayUsersCount++;
                            secDayUsersCount++;
                            thirdDayUsersCount++;
                            fourthDayUsersCount++;
                        }
                    }
                    else if (a <= numDaysBeforeStardDate + 1)
                    {
                        if (isDayFourLessThanToday)
                        {
                            //increment nothing because none of the days fall after today or today
                        }
                        else if (isDayThreeLessThanToday)
                        {
                            //increment nothing because one day of the user is taken up by ads 
                            //and only one days is adequate.
                        }
                        else if (isDayTwoLessThanToday)
                        {
                            fourthDayUsersCount++;
                        }
                        else if (isDayOneLessThanToday)
                        {
                            thirdDayUsersCount++;
                            fourthDayUsersCount++;
                        }
                        else
                        {
                            secDayUsersCount++;
                            thirdDayUsersCount++;
                            fourthDayUsersCount++;
                        }
                    }
                    else if (a <= numDaysBeforeStardDate + 2)
                    {
                        if (isDayFourLessThanToday)
                        {

                        } else if (isDayThreeLessThanToday)
                        {

                        }
                        else if (isDayTwoLessThanToday)
                        {

                        }
                        else if (isDayOneLessThanToday)
                        {
                            fourthDayUsersCount++;
                        }
                        else
                        {
                            thirdDayUsersCount++;
                            fourthDayUsersCount++;
                        }
                    }
                    else if (a <= numDaysBeforeStardDate + 3)
                    {
                        if (isDayFourLessThanToday)
                        {

                        }
                        else if (isDayThreeLessThanToday)
                        {

                        }
                        else if (isDayTwoLessThanToday)
                        {

                        }
                        else if (isDayOneLessThanToday)
                        {

                        }
                        else
                        {
                            fourthDayUsersCount++;
                        }
                    }

                

            }
            else
            {
                if (isDayFourLessThanToday)
                {

                }
                else if (isDayThreeLessThanToday)
                {
                    fourthDayUsersCount++;
                }
                else if (isDayTwoLessThanToday)
                {
                    thirdDayUsersCount++;
                    fourthDayUsersCount++;
                }
                else if (isDayOneLessThanToday)
                {
                    secDayUsersCount++;
                    thirdDayUsersCount++;
                    fourthDayUsersCount++;
                }
                else
                {
                    firstDayUsersCount++;
                    secDayUsersCount++;
                    thirdDayUsersCount++;
                    fourthDayUsersCount++;
                }
            }
        }
    }

    /// <summary>
    ///Get all saved ad searches that are live
    ///From these get all ad searches with the same city, state, and country
    ///From these get all ad searches with the samve categories
    ///From these get all ad searches that are one away from being a list ready to send to the user
    ///return this count in firstDayUsersCount variable.
    /// </summary>
    /// <param name="City"></param>
    /// <param name="State"></param>
    /// <param name="Country"></param>
    /// <param name="categories"></param>
    /// <param name="startShowing"></param>
    /// <param name="firstDayUsersCount"></param>
    /// <param name="isNoUsers"></param>
    /// <param name="isBig"></param>
    public DataView CalculateUsersEmailAdCapacity(ref string cmd, string header, string description, 
        string City, string Zip,
        string State, string Country, string categories, ref int firstDayUsersCount, ref bool isNoUsers, bool isAll)
    {
        try
        {
            //Get all saved ad searches that are live and in selected categories with selected keywords
            string command = "";
            cmd = "got here yo";
            if (categories.Trim() != "")
            {
                if (categories[categories.Length - 1] == ';')
                {
                    categories = categories.Remove(categories.Length - 1);
                }

                char[] delim = { ';' };
                string[] tokens = categories.Split(delim);

                for (int i = 0; i < tokens.Length; i++)
                {
                    if (tokens[i].Trim() != "")
                    {
                        if (command == "")
                        {
                            command += " AND (";
                        }

                        command += " SAC.CategoryID=" + tokens[i];
                        if (i != tokens.Length - 1)
                        {
                            command += " OR ";
                        }
                        else
                        {
                            command += " ) ";
                        }
                    }
                    else
                    {
                        if (i == tokens.Length - 1)
                        {
                            command += " ) ";
                        }
                    }
                }
            }
            string findAll = "";
            if (!isAll)
                findAll = " AND SA.CountInEmailList = SA.NumAdsInEmail-1 ";
            else
                findAll = " AND SA.CountInEmailList < SA.NumAdsInEmail-1 ";

            //From these get all ad searches that are one away from being a list ready to send to the user
            //return this count in firstDayUsersCount variable.
            string subcommand = "";
            string subcommand2 = "";
            //if (categories.Trim() != "")
                subcommand = ", SavedAdSearches_Categories SAC WHERE SAC.SearchID=SA.ID " + command + " AND ";
            //else
                subcommand2 = " WHERE SA.ID NOT IN (SELECT SAC.SearchID FROM SavedAdSearches_Categories SAC)  AND ";


            //From these get all ad searches with the same city, state, and country

            //get the zip string
            string filter = "";
            if (Zip != null)
            {
                Zip = Zip.Replace("UP.", "");

                filter = " AND ((State = '" + State + "' AND Country=" +
                Country + " AND City='" + City + "' " + Zip + ") OR (Country="+Country+" AND (State is NULL OR State='') AND (City is NULL OR City = '') "+Zip+
                ") OR (State = '" + State + "' AND Country=" +
                Country + " AND (City is NULL OR City = '') " + Zip + ") OR " +
                " (State = '"+State+"' AND Country = "+Country+" AND City = '"+City+"' AND (Zip is NULL OR Zip = '')) OR (State = '" + State + "' AND Country=" +
                Country + " AND City='" + City + "' AND (Zip is NULL OR Zip = '')) OR (State = '" +
                State + "' AND Country=" +
                Country + " AND (City is NULL OR City = '') AND (Zip is NULL OR Zip = '') ) OR (" +
                "Country=" + Country + " AND (State is NULL OR State = '') AND  (City is NULL OR City = '') " +
                Zip + "))";
            }
            else
            {
                filter = " AND ((State = '" + State + "' AND Country=" +
                Country + " AND City='" + City + "' AND (Zip is NULL OR Zip = '')) OR (State = '" +
                State + "' AND Country=" +
                Country + " AND (City is NULL OR City = '') AND (Zip is NULL OR Zip = '') ))";
            }

            string str = "SELECT DISTINCT SA.ID, SA.UserID, SA.Keywords, SA.City, SA.State, " +
        "SA.Country, SA.Zip FROM SavedAdSearches SA " + subcommand +
        " SA.Live='True' AND SA.UserID <> " + Session["User"].ToString() + findAll + filter;
            string str2 = "SELECT DISTINCT SA.ID, SA.UserID, SA.Keywords, SA.City, SA.State, " +
        "SA.Country, SA.Zip FROM SavedAdSearches SA " + subcommand2 +
        " SA.Live='True' AND SA.UserID <> " + Session["User"].ToString() + findAll + filter;
            cmd = str;
            DataSet dsLiveSearches = GetData(str);
            DataView dvLiveSearches = new DataView(dsLiveSearches.Tables[0], "", "", DataViewRowState.CurrentRows);
            DataView dvLiveSearches2 = GetDataDV(str2);
            Session["command"] = str + str2;
            //From this list, get the ad searches witch have keywords that fit this ads header or description
            char[] delim2 = { ' ' };

            DataView dvReturn = new DataView();
            DataTable table = new DataTable("table");
            DataColumn column = new DataColumn("ID");
            table.Columns.Add(column);
            DataColumn column2 = new DataColumn("UserID");
            table.Columns.Add(column2);

            for (int i = 0; i < dvLiveSearches.Count; i++)
            {
                if (dvLiveSearches[i]["Keywords"].ToString().Trim() != "")
                {
                    bool hasKeyword = false;
                    string[] tokens2 = dvLiveSearches[i]["Keywords"].ToString().Split(delim2);
                    for (int j = 0; j < tokens2.Length; j++)
                    {
                        if (tokens2[j].Trim() != "")
                        {
                            if (description.Contains(tokens2[j].Trim()))
                            {
                                hasKeyword = true;
                                break;
                            }
                            else if (header.Contains(tokens2[j].Trim()))
                            {
                                hasKeyword = true;
                                break;
                            }
                        }
                    }
                    if (hasKeyword)
                    {
                        DataRow row = table.NewRow();
                        row[0] = dvLiveSearches[i]["ID"].ToString().Trim();
                        row[1] = dvLiveSearches[i]["UserID"].ToString().Trim();
                        table.Rows.Add(row);
                    }
                }
                else
                {
                    DataRow row = table.NewRow();
                    row[0] = dvLiveSearches[i]["ID"].ToString().Trim();
                    row[1] = dvLiveSearches[i]["UserID"].ToString().Trim();
                    table.Rows.Add(row);
                }
            }

            for (int i = 0; i < dvLiveSearches2.Count; i++)
            {
                if (dvLiveSearches2[i]["Keywords"].ToString().Trim() != "")
                {
                    bool hasKeyword = false;
                    string[] tokens2 = dvLiveSearches2[i]["Keywords"].ToString().Split(delim2);
                    for (int j = 0; j < tokens2.Length; j++)
                    {
                        if (tokens2[j].Trim() != "")
                        {
                            if (description.Contains(tokens2[j].Trim()))
                            {
                                hasKeyword = true;
                                break;
                            }
                            else if (header.Contains(tokens2[j].Trim()))
                            {
                                hasKeyword = true;
                                break;
                            }
                        }
                    }
                    if (hasKeyword)
                    {
                        DataRow row = table.NewRow();
                        row[0] = dvLiveSearches2[i]["ID"].ToString().Trim();
                        row[1] = dvLiveSearches2[i]["UserID"].ToString().Trim();
                        table.Rows.Add(row);
                    }
                }
                else
                {
                    DataRow row = table.NewRow();
                    row[0] = dvLiveSearches2[i]["ID"].ToString().Trim();
                    row[1] = dvLiveSearches2[i]["UserID"].ToString().Trim();
                    table.Rows.Add(row);
                }
            }

            dvReturn.Table = table;
            firstDayUsersCount = dvLiveSearches.Count + dvLiveSearches2.Count;
            return dvReturn;
        }
        catch (Exception ex)
        {
            cmd = ex.ToString();
        }

        return new DataView();
    }

    public void returnCategoryIDString(Telerik.Web.UI.RadTreeView CategoryTree, ref string categories)
    {
        List<Telerik.Web.UI.RadTreeNode> list = (List<Telerik.Web.UI.RadTreeNode>)CategoryTree.GetAllNodes();

        foreach (Telerik.Web.UI.RadTreeNode node in list)
        {
            if (node.Checked)
            {
                categories += node.Value + ";";

                if (node.ParentNode != null)
                {
                    categories += node.ParentNode.Value + ";";

                    if (node.ParentNode.ParentNode != null)
                        categories += node.ParentNode.ParentNode.Value + ";";
                }
            }
        }
    }

    /// <summary>
    /// //Methods updates user's record with a new IP address if there is one.
    /// </summary>
    public void UpdateUserSession()
    {
        //Update the generic login entry with user's information.
        //if (Session["GenericLoginID"] != null)
        //{
        //Execute("UPDATE GenericLogins SET isUser = 'True', UserID = " + Session["User"].ToString() +
        //    " WHERE ID=" + Session["GenericLoginID"].ToString());

        //Update the user's information with the IP address from the generic login.
        DataSet dsUserIPs = GetData("SELECT * FROM Users WHERE User_ID=" + Session["User"].ToString());
        string IPs = dsUserIPs.Tables[0].Rows[0]["IPs"].ToString();

        if (IPs == "")
        {
            IPs = ";" + GetIP() + ";";
        }
        else
        {
            char[] delim = { ';' };
            string[] tokens = IPs.Split(delim);
            bool insert = true;
            for (int i = 0; i < tokens.Length; i++)
            {
                if (tokens[i] == GetIP())
                {
                    insert = false;
                    break;
                }
            }
            if (insert)
            {
                IPs += GetIP() + ";";
            }

        }

        Execute("UPDATE Users SET IPs='" + IPs + "' WHERE User_ID=" + Session["User"].ToString());

        //********July 30th: starting to count anonymous ads******************
        //-if the user logs in: assign all ads in the AdStatistics table with that anonymous session that UserID. 
        //-same thing with if they create a new account

        

        //if (Session["AnonymousUser"] != null)
        //{
        //    //Get the data for Reason column and LocationOnly column to AdStatistics table
        //    DataView dvAllSeenAds = GetDataDV("SELECT * FROM AdStatistics WHERE IP='" + GetIP() + "' AND UserID is NULL");

        //    foreach (DataRowView row in dvAllSeenAds)
        //    {
        //        DataSet dsCategories = GetData("SELECT * FROM Ad_Category_Mapping ACM, UserCategories UC " +
        //            "WHERE ACM.AdID=" + row["AdID"].ToString() + " AND ACM.CategoryID=UC.CategoryID AND UC.UserID=" +
        //            Session["User"].ToString());

        //        DataView dvCategories = new DataView(dsCategories.Tables[0], "", "", DataViewRowState.CurrentRows);

        //        string categories = "";
        //        for (int i = 0; i < dvCategories.Count; i++)
        //        {
        //            categories += dvCategories[i]["CategoryID"].ToString() + ";";
        //        }

        //        bool locationOnly = false;
        //        if (categories == "")
        //        {
        //            locationOnly = true;
        //        }

        //        Execute("UPDATE AdStatistics SET Reason='" + categories + "', LocationOnly='" +
        //            locationOnly.ToString() + "', UserID=" + Session["User"].ToString() +
        //            " WHERE ID=" + row["ID"].ToString());
        //    }
        //}

        ////if (Session["BigAnonymousUser"] != null)
        ////{
        ////    //Get the data for Reason column and LocationOnly column to AdStatistics table
        ////    DataView dvAllSeenAds = GetDataDV("SELECT * FROM AdStatistics WHERE AnnonymousSessionNum='" + 
        ////        Session["BigAnonymousUser"].ToString() + "'");

        ////    foreach (DataRowView row in dvAllSeenAds)
        ////    {
        ////        DataSet dsCategories = GetData("SELECT * FROM Ad_Category_Mapping ACM, UserCategories UC " +
        ////            "WHERE ACM.AdID=" + row["AdID"].ToString() + " AND ACM.CategoryID=UC.CategoryID AND UC.UserID=" +
        ////            Session["User"].ToString());

        ////        DataView dvCategories = new DataView(dsCategories.Tables[0], "", "", DataViewRowState.CurrentRows);

        ////        string categories = "";
        ////        for (int i = 0; i < dvCategories.Count; i++)
        ////        {
        ////            categories += dvCategories[i]["CategoryID"].ToString() + ";";
        ////        }

        ////        bool locationOnly = false;
        ////        if (categories == "")
        ////        {
        ////            locationOnly = true;
        ////        }

        ////        Execute("UPDATE AdStatistics SET Reason='" + categories + "', LocationOnly='" +
        ////            locationOnly.ToString() + "', UserID=" + Session["User"].ToString() +
        ////            " WHERE AnnonymousSessionNum='" + Session["BigAnonymousUser"].ToString() +
        ////            "' AND AdID=" + row["AdID"].ToString());
        ////    }
        ////}

        //GetAdSet(1, false);
        //GetAdSet(1, true);

        //DataView dv = GetDataDV("SELECT * FROM AdStatistics AD, Ads A WHERE A.Ad_ID=AD.AdID AND AD.UserID='" +
        //    Session["User"].ToString() + "' AND CONVERT(NVARCHAR, MONTH(AD.[DATE])) +'/' + " +
        //    "CONVERT(NVARCHAR, DAY(AD.[DATE])) +" +
        //    "'/' + CONVERT(NVARCHAR, YEAR(AD.[DATE])) = '" +
        //    timeNow.Date.ToShortDateString().Replace("12:00:00 AM", "").Trim() + "'");

        ////make sure the adIDs are unique
        //Hashtable tempHash = new Hashtable();

        //foreach (DataRowView row in dv)
        //{
        //    if (!tempHash.Contains(row["AdID"].ToString().Trim()))
        //    {
        //        tempHash.Add(row["AdID"].ToString().Trim(), "");
        //        if (int.Parse(row["AdID"].ToString()) != hippoAdCategory && int.Parse(row["AdID"].ToString()) != hippoHalfAvailableCategory)
        //        {

        //            Execute("UPDATE UserAds SET SeenAdSet = CASE WHEN (SeenAdSet IS NULL) THEN  ';" +
        //                 row["AdID"].ToString() + "' ELSE SeenAdSet + ';" +
        //                 row["AdID"].ToString() + "' END WHERE UserID=" +
        //                 Session["User"].ToString() + " AND BigAd='" + row["BigAd"].ToString() +
        //                 "' AND CONVERT(NVARCHAR,Month([Date])) + " +
        //                 "'/' + CONVERT(NVARCHAR, DAY([Date])) + " +
        //                 "'/' + CONVERT(NVARCHAR, YEAR([Date])) ='" + timeNow.Date.ToShortDateString().Replace("12:00:00 AM","").Trim() +
        //                 "'");
        //        }
        //    }
        //}

        ////**************************

        ////THIS NO LONGER HAPPENS. GENERIC ADS ARE FREE.
        //////Update the information for the ads that the user has already seen while being generic.
        ////DataSet ds = GetData("SELECT * FROM Ads_Seen_Generic WHERE SessionID='" + Session["GenericSession"].ToString() + "'");
        ////if (ds.Tables.Count > 0)
        ////{
        ////    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
        ////    {
        ////        string date = ds.Tables[0].Rows[j]["Date"].ToString();
        ////        Execute("INSERT INTO Ads_Seen_By_User (Ad_ID, User_ID, Date) VALUES(" +
        ////            ds.Tables[0].Rows[j]["Ad_ID"].ToString() + ", " + Session["User"].ToString() + ", '" + date + "')");
        ////        Execute("UPDATE Ads_Seen_Generic SET SessionTransfered='True', UserID=" +
        ////            Session["User"].ToString() + " WHERE ID=" + ds.Tables[0].Rows[j]["ID"].ToString());
        ////    }
        ////}

        //////Update the information for the MAIN ads that the user has already seen while being generic.
        ////ds = GetData("SELECT * FROM Ads_Seen_Generic WHERE SessionID='" + Session["GenericSessionBig"].ToString() + "'");
        ////if (ds.Tables.Count > 0)
        ////{
        ////    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
        ////    {
        ////        string date = ds.Tables[0].Rows[j]["Date"].ToString();
        ////        Execute("INSERT INTO Ads_Seen_By_User (Ad_ID, User_ID, Date) VALUES(" +
        ////            ds.Tables[0].Rows[j]["Ad_ID"].ToString() + ", " + Session["User"].ToString() + ", '" + date + "')");
        ////        Execute("UPDATE Ads_Seen_Generic SET SessionTransfered='True', UserID=" +
        ////            Session["User"].ToString() + " WHERE ID=" + ds.Tables[0].Rows[j]["ID"].ToString());
        ////    }
        ////}

    }

    public void SendAdFinishedEmail(string adID)
    {
        DataSet ds = GetData("SELECT * FROM Users U, Ads A WHERE A.User_ID=U.User_ID AND A.Ad_ID=" + adID);
        DataView dv = new DataView(ds.Tables[0], "", "", DataViewRowState.CurrentRows);
        string toEmail = dv[0]["Email"].ToString();

        string email = "Your ad '" + dv[0]["Header"].ToString() +
            "' has finished posting. You can find the statistics of how your ad has been viewed <a class=\"AddLink\" href=\"http://hippohappenings.com/AdStatistics.aspx?Ad=" + adID +
            "\">here</a>.<br/><br/><br/>Have a Hippo Happening Day!<br/><br/> " +
            "<a class=\"AddLink\" href=\"http://HippoHappenings.com\">HippoHappenings</a>";

        Execute("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, To_UserID, " +
            "[Date], [Read], [Mode], [Live]) VALUES('" + email.Replace("'", "''") + "', 'Your ad ''" +
            dv[0]["Header"].ToString().Replace("'", "''") + "'' has finished posting!', " +
            hippoHappeningsUserID.ToString() + ", " + dv[0]["User_ID"] + ", '" +
            timeNow.ToString() + "', 'False', 1, 'True')");

        //Update the Ad to Live = false
        Execute("UPDATE Ads SET LIVE='False' WHERE Ad_ID=" + adID);


        SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
            System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(), toEmail,
            email, "Your Ad '" + dv[0]["Header"].ToString() + "' has finished posting.");
    }

    public DataView GetAdSet(int setNumber, bool isBig)
    {
        DataView dvUserAds = new DataView();
        if (Session["User"] != null)
        {
            dvUserAds = RetrieveUsersAds(isBig, Session["User"].ToString());


            dvUserAds = SortByLastSeen(dvUserAds, isBig);
        }
        else
        {
            //Create a new unique anonymous session number if one does not exist.
            string userSessionToUse = "AnonymousUser";
            if (isBig)
                userSessionToUse = "BigAnonymousUser";

            string firstIndex = (numberTotalAds - 1).ToString();
            if(isBig)
                firstIndex = (numberTotalBigAds - 1).ToString();

            if (Session[userSessionToUse] != null)
            {
                DataSet ds;
                if (isBig)
                {
                    ds = GetData("SELECT * FROM BigAdStatistics_Anonymous WHERE SessionNum = '" +
                    Session["BigAnonymousUser"].ToString() + "'");
                }
                else
                {
                    ds = GetData("SELECT * FROM AdStatistics_Anonymous WHERE SessionNum = '" +
                        Session["AnonymousUser"].ToString() + "'");
                }
                bool clearSession = false;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dvUserAds = GetDVFromDS(GetAdsBasedOnString(ds.Tables[0].Rows[0]["AdsList"].ToString()));
                    }
                    else
                    {
                        clearSession = true;
                    }
                }
                else
                {
                    clearSession = true;
                }

                if (clearSession)
                {
                    Session.Remove(userSessionToUse);
                }
            }

            if (Session[userSessionToUse] == null)
            {
                int randomNum;

                //Create the record for this anonymous session in the database.
                //IP address, City and State, AdsList, Date, Ads seen, Session # 

                //1. look for their IP address to associate with a userID.
                string theIP = GetIP();
                string tableToUse = "BigAdStatistics_Anonymous";
                if (!isBig)
                    tableToUse = "AdStatistics_Anonymous";
                DataView dvUserIP = GetDataDV("SELECT * FROM Users WHERE IPs LIKE '%;" + theIP + ";%'");
                //2. If not found, search the UserAds table to see if there are any 
                //      anonymous records for that IP address.
                if (dvUserIP.Count == 0)
                {
                    //UPDATE 11/20/2010: CANNOT ASSUME THAT JUST BECAUSE IP SAW AD THEY ARE IN
                    //DataView dvAUserAds = GetDataDV("SELECT * FROM AdStatistics WHERE IP = '" +
                    //    theIP + "'");
                    //THE SAME LOCATION AS THE AD
                    //a. If none found - get any random ads for this user to see 
                    //if (dvAUserAds.Count == 0)
                    //{
                        if(conn.State != ConnectionState.Open)
                            conn.Open();
                        SqlCommand cmd = new SqlCommand("INSERT INTO " + tableToUse + " (IP,Date, LastSeenIndex) VALUES('" +
                            theIP + "', '" + timeNow.ToString() + "', " + firstIndex + ")", conn);
                        cmd.ExecuteNonQuery();
                        
                        cmd = new SqlCommand("SELECT @@IDENTITY AS AID", conn);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        randomNum = int.Parse(ds.Tables[0].Rows[0]["AID"].ToString());

                        Session[userSessionToUse] = randomNum;

                        Execute("UPDATE " + tableToUse + " SET SessionNum='" + randomNum.ToString() + "' WHERE ID=" + randomNum.ToString());                        
                        
                        dvUserAds = GetAnonymousAds(randomNum, isBig);
                        string lastSeen = "";
                        string ads = GetAdStringBasedOnDS(dvUserAds, ref lastSeen);
                        Execute("UPDATE " + tableToUse + " SET AdsList='" + ads +
                            "' WHERE SessionNum='" + randomNum.ToString() + "'");
                    //}
                    //else
                    //{
                    //    //b. If found - 
                    //    //get an address of one ad 
                    //    DataView dvAd = GetDataDV("SELECT * FROM Ads WHERE Ad_ID=" +
                    //        dvAUserAds[0]["AdID"].ToString());

                    //    string catCountry = dvAd[0]["CatCountry"].ToString();
                    //    string catState = dvAd[0]["CatState"].ToString();
                    //    string catCity = dvAd[0]["CatCity"].ToString();
                    //    //get a list of all seen ads by this IP
                    //    DataView dvSeenAds = GetDataDV("SELECT * FROM AdStatistics WHERE IP='" + theIP +
                    //        "' AND  CONVERT(NVARCHAR, MONTH([DATE])) +'/' + CONVERT(NVARCHAR, DAY([DATE])) +" +
                    //        "'/' + CONVERT(NVARCHAR, YEAR([DATE])) <> '" + timeNow.Date.ToShortDateString().Replace("12:00:00 AM","").Trim() + "'");
                    //    //get all ads in that address that are not the ones seen
                    //    string notThese = "";
                    //    foreach (DataRowView row in dvSeenAds)
                    //    {
                    //        notThese += " AND A.Ad_ID <> " + row["AdID"].ToString();
                    //    }
                    //    dvUserAds = RetrieveAnonymousAds(catCity, catState, catCountry, isBig, notThese);

                    //    if (conn.State != ConnectionState.Open)
                    //        conn.Open();
                    //    SqlCommand cmd = new SqlCommand("INSERT INTO " + tableToUse + " (IP,Date, LastSeenIndex) VALUES('" +
                    //        theIP + "', '" + timeNow.ToString() + "', " + firstIndex + ")", conn);
                    //    cmd.ExecuteNonQuery();

                    //    cmd = new SqlCommand("SELECT @@IDENTITY AS AID", conn);
                    //    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    //    DataSet ds = new DataSet();
                    //    da.Fill(ds);

                    //    randomNum = int.Parse(ds.Tables[0].Rows[0]["AID"].ToString());

                    //    Session[userSessionToUse] = randomNum;

                    //    Execute("UPDATE " + tableToUse + " SET SessionNum='" + randomNum.ToString() + "' WHERE ID=" + randomNum.ToString());                        

                    //    string lastSeen = "";
                    //    string ads = GetAdStringBasedOnDS(dvUserAds, ref lastSeen);
                    //    Execute("UPDATE " + tableToUse + " SET AdsList='" + ads +
                    //        "' WHERE SessionNum='" + randomNum.ToString() + "'");
                    //}
                }
                //3. If found 
                else
                {
                    //i. If there are multiple records exclude all ads everyone has seen
                    //ii. If only one record...assign that one
                    string userID = dvUserIP[0]["User_ID"].ToString();
                    //a. get the ads for this user.
                    if (dvUserIP.Count > 1)
                    {
                        dvUserAds = RetrieveAnonymousUsersAds_Multiple(isBig, theIP);
                    }
                    else
                    {
                        dvUserAds = RetrieveAnonymousUsersAds(isBig, userID);
                    }

                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO " + tableToUse +
                        "(IP,Date, LastSeenIndex) VALUES('" +
                            theIP + "', '" + timeNow.ToString() + "', " + firstIndex + ")", conn);
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("SELECT @@IDENTITY AS AID", conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    randomNum = int.Parse(ds.Tables[0].Rows[0]["AID"].ToString());

                    Session[userSessionToUse] = randomNum;

                    Execute("UPDATE " + tableToUse + " SET SessionNum='" + randomNum.ToString() + 
                        "' WHERE ID=" + randomNum.ToString());                        

                    string lastSeen = "";
                    string ads = GetAdStringBasedOnDS(dvUserAds, ref lastSeen);
                    Execute("UPDATE " + tableToUse + " SET AdsList='" + ads +
                        "' WHERE SessionNum='" + randomNum.ToString() + "'");
                }
            }
        }

        if (isBig)
        {
            Session["ADBigDV"] = dvUserAds;
            Session["ADBigCount"] = 0;
            return dvUserAds;
        }
        else
        {
            DataView ds1 = SplitDataViewBy4(0, dvUserAds);
            DataView ds2 = SplitDataViewBy4(1, dvUserAds);
            DataView ds3 = SplitDataViewBy4(2, dvUserAds);
            DataView ds4 = SplitDataViewBy4(3, dvUserAds);

            Session["AD1DV"] = ds1;
            Session["AD2DV"] = ds2;
            Session["AD3DV"] = ds3;
            Session["AD4DV"] = ds4;
            Session["AD1Count"] = 0;
            Session["AD2Count"] = 0;
            Session["AD3Count"] = 0;
            Session["AD4Count"] = 0;
            //ds1.WriteXml(Server.MapPath(".") + "\\UserFiles\\categoies1.xml", XmlWriteMode.IgnoreSchema);
            //ds2.WriteXml(Server.MapPath(".") + "\\UserFiles\\categoies2.xml", XmlWriteMode.IgnoreSchema);
            //ds3.WriteXml(Server.MapPath(".") + "\\UserFiles\\categoies3.xml", XmlWriteMode.IgnoreSchema);
            //ds4.WriteXml(Server.MapPath(".") + "\\UserFiles\\categoies4.xml", XmlWriteMode.IgnoreSchema);

            switch (setNumber)
            {
                case 1:
                    return ds1;
                    break;
                case 2:
                    return ds2;
                    break;
                case 3:
                    return ds3;
                    break;
                case 4:
                    return ds4;
                    break;
                default: return ds1; break;
            }
        }
    }

    public DataView GetGroupAds(string GroupID, ref string commandStr, ref int lastSeen)
    {
        DataView finalData = new DataView();
        bool getAll = false;
        if (Session["User"] != null)
        {
            DataView dv = GetDataDV("SELECT * FROM UserAds WHERE UserID=" + Session["User"].ToString()+
                " AND CONVERT(NVARCHAR,Month([Date])) + '/' + CONVERT(NVARCHAR, DAY([Date])) + " +
                "'/' + CONVERT(NVARCHAR, YEAR([Date])) = '" + timeNow.Date + 
                "' AND isGroup = 'True'");
            if (dv.Count == 0)
                getAll = true;
            else
            {
                finalData = new DataView(GetAdsBasedOnString(dv[0]["AdSet"].ToString()).Tables[0], "", "", DataViewRowState.CurrentRows);
                lastSeen = int.Parse(dv[0]["LastSeenIndex"].ToString());
            }
        }
        else
        {
            if (Session["BigAnonymousGroupSession"] != null)
            {
                DataView dv = GetDataDV("SELECT * FROM BigAdStatistics_Anonymous WHERE isGroup='True' AND SessionNum = '" +
                    Session["BigAnonymousGroupSession"].ToString() + "'");
                finalData = new DataView(GetAdsBasedOnString(dv[0]["AdsList"].ToString()).Tables[0], "", "", DataViewRowState.CurrentRows);
                lastSeen = int.Parse(dv[0]["LastSeenIndex"].ToString());
            }
            else
                getAll = true;
        }
       
        if(getAll)
        {
            lastSeen = 0;
            DataView dvGroup = GetDataDV("SELECT * FROM Groups WHERE ID=" + GroupID);
            string country = dvGroup[0]["Country"].ToString();
            string state = dvGroup[0]["State"].ToString();
            string zip = dvGroup[0]["Zip"].ToString();

            string compareState = "";
            string compareCity = "";
            string compareCountry = " AND A.CatCountry=" + dvGroup[0]["Country"].ToString();
            SqlDbType[] types;
            object[] data;

            string zips = "";

            if (dvGroup[0]["State"].ToString() != null)
            {
                compareState = " AND A.CatState = @p0 ";

                if (dvGroup[0]["City"].ToString() != null)
                {
                    compareCity = " AND A.CatCity = @p1 ";
                    types = new SqlDbType[2];
                    data = new object[2];
                    types[0] = SqlDbType.NVarChar;
                    types[1] = SqlDbType.NVarChar;
                    data[0] = dvGroup[0]["State"].ToString();
                    data[1] = dvGroup[0]["City"].ToString();

                }
                else
                {
                    types = new SqlDbType[1];
                    data = new object[1];
                    types[0] = SqlDbType.NVarChar;
                    data[0] = dvGroup[0]["State"].ToString();


                }
            }
            else
            {
                types = new SqlDbType[0];
                data = new object[0];
            }


            if (dvGroup[0]["Zip"].ToString() != null)
            {
                if (dvGroup[0]["Zip"].ToString().Trim() != "")
                {
                    char[] delim = { ';' };
                    string[] tokens = dvGroup[0]["Zip"].ToString().Split(delim);
                    zips = "";

                    for (int i = 0; i < tokens.Length; i++)
                    {
                        if (tokens[i].Trim() != "")
                        {
                            if (zips.Trim() != "")
                                zips += " OR ";
                            zips += " A.CatZip LIKE '%;" + tokens[i].Trim() + ";%' ";
                        }
                    }
                }
            }

            if (zips.Trim() != "")
                zips = " AND (( " + zips + ") OR A.CatZip is NULL) ";


            //Select ad only in users category location
            //that is featured
            //that has not been posted by the user viewing this ad
            //that has been seen less numbers than it was paid for
            //that is live
            //that is big/normal based on the isBig boolean.

            //Get all seen ads to exclude in the command
            string excludeTodays = "";
            bool notTodays = true;
            if (notTodays)
                excludeTodays = " AND CONVERT(NVARCHAR, MONTH([DATE])) +'/' + CONVERT(NVARCHAR, DAY([DATE])) +" +
                "'/' + CONVERT(NVARCHAR, YEAR([DATE])) <> '" + timeNow.Date.ToShortDateString().Replace("12:00:00 AM","").Trim() + "'";

            DataSet dsExclude = new DataSet();
            dsExclude.Tables.Add();
            string excludeUsers = "";
            //If user is logged in, exclude the ads already seen previous to today
            if (Session["User"] != null)
            {
                dsExclude = GetData("SELECT AdID AS Ad_ID FROM AdStatistics WHERE UserID=" +
                     Session["User"].ToString() + " " + excludeTodays);
                excludeUsers = " AND User_ID <> " + Session["User"].ToString();
            }
            //If no one is logged in, exclude the ads already seen by IP address previous to today
            else
            {
                GetData("SELECT AdID AS Ad_ID FROM AdStatistics WHERE IP='" + GetIP() +
                    "' " + excludeTodays);
                DataView dvAllUsers = GetDataDV("SELECT * FROM Users WHERE IPs LIKE '%;" + GetIP() + ";%'");
                foreach (DataRowView row in dvAllUsers)
                {
                    excludeUsers += " AND User_ID <> " + row["User_ID"].ToString();
                }
            }

            DataView dvExclude = new DataView(dsExclude.Tables[0], "", "", DataViewRowState.CurrentRows);

            string adsExclude = "";

            for (int j = 0; j < dvExclude.Count; j++)
            {
                if (j == 0)
                {
                    adsExclude += " AND (";
                }

                adsExclude += " A.Ad_ID <> " + dvExclude[j]["Ad_ID"].ToString();

                if (j != dvExclude.Count - 1)
                    adsExclude += " AND ";
                else
                    adsExclude += " ) ";

            }

            string includeInAll = " AND A.LIVE=1 AND A.Featured=1 AND A.User_ID <> " +
                hippoHappeningsUserID.ToString() + " " + excludeUsers +
                " AND A.NumCurrentViews < A.NumViews AND A.BigAd='True' " +
                compareCountry + compareState + compareCity + zips +
                    " ORDER BY DateAdded ASC";

            //Get ads on categories

            DataSet dsCategoryAds = GetGroupCategoryAds(GroupID, includeInAll, adsExclude, types, data, ref commandStr);
            DataSet dsLocationAds = GetGroupLocationAds(GroupID, includeInAll, adsExclude, types, data);
            finalData = FillAdSet(MergeAdSets(dsCategoryAds, dsLocationAds, false), true);

            string command = "";
            string temp = "";
            string ads = GetAdStringBasedOnDS(finalData, ref temp);

            if (Session["User"] != null)
            {
                command = "INSERT INTO UserAds (UserID,Date, LastSeenIndex, isGroup, AdSet) VALUES(" +
                    Session["User"].ToString() + ", '" + timeNow.ToString() + "', 0, 'True', '" + ads + "')";
            }
            else
            {
                command = "INSERT INTO BigAdStatistics_Anonymous (IP,Date, LastSeenIndex, isGroup) VALUES('" +
                GetIP() + "', '" + timeNow.ToString() + "', 0, 'True')";

                if (conn.State != ConnectionState.Open)
                    conn.Open();
                SqlCommand cmd = new SqlCommand(command, conn);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("SELECT @@IDENTITY AS AID", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                string randomNum = ds.Tables[0].Rows[0]["AID"].ToString();

                Session["BigAnonymousGroupSession"] = randomNum;


                Execute("UPDATE BigAdStatistics_Anonymous SET AdsList='" + ads +
                    "', SessionNum='" + randomNum + "' WHERE isGroup='True' AND ID=" + randomNum);
            }
        }

        return finalData;
    }

    public DataSet GetGroupCategoryAds(string GroupID, string includeInAll, 
        string adsExclude, SqlDbType[] types, object[] data, ref string commandStr)
    {
        string strOfCategories = "";

        DataView dvCats = GetDataDV("SELECT AdCategory_ID FROM Group_Category GC, Group_Ad_Categories_Mapping GACM " +
            "WHERE GC.Category_ID=GACM.GroupCategory_ID AND GC.Group_ID=" + GroupID);

        for (int i = 0; i < dvCats.Count; i++)
        {
            if (strOfCategories != "")
            {
                strOfCategories += " OR ";
            }

            strOfCategories += " ACM.CategoryID=" + dvCats[i]["AdCategory_ID"].ToString();
            DataView dvCats2 = GetDataDV("SELECT * FROM AdCategories " +
                "WHERE ParentID=" + dvCats[i]["AdCategory_ID"].ToString());
            for (int j = 0; j < dvCats2.Count; j++)
            {
                if (strOfCategories != "")
                {
                    strOfCategories += " OR ";
                }
                strOfCategories += " ACM.CategoryID=" + dvCats2[j]["ID"].ToString();

                DataView dvCats3 = GetDataDV("SELECT * FROM AdCategories WHERE ParentID=" +
                    dvCats2[j]["ID"].ToString());

                for (int n = 0; n < dvCats3.Count; n++)
                {
                    if (strOfCategories != "")
                    {
                        strOfCategories += " OR ";
                    }
                    strOfCategories += " ACM.CategoryID=" + dvCats3[n]["ID"].ToString();
                }
            }

        }

        if (strOfCategories.Trim() != "")
        {
            strOfCategories = "(ACM.CategoryID=" + lostAndFoundCategory.ToString() + " OR " + strOfCategories + ")";
        }
        else
        {
            strOfCategories += " ACM.CategoryID=" + lostAndFoundCategory.ToString();
        }

        string command = "SELECT * FROM Ads A, Ad_Category_Mapping ACM, Ad_Calendar AC WHERE " +
            "AC.AdID=A.Ad_ID AND AC.DateTimeStart <=  CONVERT(DATETIME,'" + timeNow.ToString() + "') AND " +
            " ACM.AdID=A.Ad_ID AND (" + strOfCategories + ")" + adsExclude + includeInAll;
        commandStr = command;
        DataSet returnDS = GetDataWithParemeters(command, types, data);

        return returnDS;
    }

    public DataSet GetGroupLocationAds(string GroupID, string includeInAll,
        string adsExclude, SqlDbType[] types, object[] data)
    {
        string command = "SELECT * FROM Ads A, Ad_Category_Mapping ACM, Ad_Calendar AC WHERE " +
            "AC.AdID=A.Ad_ID AND AC.DateTimeStart <=  CONVERT(DATETIME,'" + timeNow.ToString() + "') AND " +
            " ACM.AdID=A.Ad_ID AND A.DisplayToAll='True' " + adsExclude + includeInAll;

        DataSet returnDS = GetDataWithParemeters(command, types, data);

        return returnDS;
    }

    //**************************END AD Retrieval Methods***************************

    public DataSet GetDSFromDV(DataView dvA)
    {
        DataSet dsT = new DataSet();
        dsT.Tables.Add(dvA.Table);
        dvA.ToTable();
        return dsT;
    }

    public DataView GetDVFromDS(DataSet ds)
    {
        return new DataView(ds.Tables[0], "", "", DataViewRowState.CurrentRows);
    }

    public DataSet SplitDataset(int startIndex, int endIndex, DataSet dsStart)
    {
        for (int i = 0; i <= startIndex; i++)
        {
            dsStart.Tables[0].Rows.RemoveAt(i);
        }

        for (int i = endIndex; i <= endIndex; i++)
        {
            dsStart.Tables[0].Rows.RemoveAt(i);
        }

        return dsStart;
    }

    public DataSet SplitDatasetBy4(int startIndex, DataSet dsUserAds)
    {
        DataView dv = new DataView(dsUserAds.Tables[0], "", "", DataViewRowState.CurrentRows);

        int i = 0;
        while (i < dv.Count)
        {
            if (i % 4 == startIndex)
            {

            }
            else
            {
                dsUserAds.Tables[0].Rows.RemoveAt(i);
            }
            i++;
        }

        return dsUserAds;
    }

    public DataView SplitDataViewBy4(int startIndex, DataView dv)
    {
        DataSet ds = GetData("SELECT TOP 1 * FROM Ads A");
        DataRow dr;
        ds.Tables[0].Rows.Clear();
        for (int i = 0; i < dv.Count; i++)
        {
            if (i % 4 == startIndex)
            {
                dr = ds.Tables[0].NewRow();
                dr["Ad_ID"] = dv[i]["Ad_ID"].ToString();
                dr["User_ID"] = dv[i]["User_ID"].ToString();
                dr["FeaturedSummary"] = dv[i]["FeaturedSummary"].ToString();
                dr["Description"] = dv[i]["Description"].ToString();
                dr["Header"] = dv[i]["Header"].ToString();
                dr["Featured"] = dv[i]["Featured"].ToString();
                dr["FeaturedPicture"] = dv[i]["FeaturedPicture"].ToString();
                dr["CatCountry"] = dv[i]["CatCountry"].ToString();
                dr["CatState"] = dv[i]["CatState"].ToString();
                dr["CatCity"] = dv[i]["CatCity"].ToString();
                dr["LIVE"] = dv[i]["LIVE"].ToString();
                dr["BigAd"] = dv[i]["BigAd"].ToString();
                ds.Tables[0].Rows.Add(dr);
            }
        }

        return new DataView(ds.Tables[0], "", "", DataViewRowState.CurrentRows);
    }

    public static double ToRadians(double d)
    {
        return (d / 180) * Math.PI;
    }

    //public double Distance(string fromLongitude, string fromLatitude, string toLongitude, string toLatitude, Measurement m)
    //{
    //    double dLon = fromLongitude - toLongitude;
    //    double dLat = fromLatitude - toLatitude;

    //    double a = Math.Pow(Math.Sin(dLat / 2.0), 2) +
    //            Math.Cos(ToRadians(double.Parse(toLatitude))) * 
    //            Math.Cos(ToRadians(double.Parse(fromLatitude))) *
    //            Math.Pow(Math.Sin(dLon / 2.0), 2.0);

    //    double c = 2 * Math.Asin(Math.Min(1.0, Math.Sqrt(a)));
    //    double d = (m == Measurement.Miles ? 3956 : 6367) * c;

    //    return d;
    //}

    public void WhatHappensOnUserLogin(DataSet ds)
    {
        Session["User"] = ds.Tables[0].Rows[0]["User_ID"].ToString();
        Session["UserName"] = ds.Tables[0].Rows[0]["UserName"].ToString();
        Session["AD1Count"] = null;
        Session["AD1DV"] = null;
        Session["AD2Count"] = null;
        Session["AD2DV"] = null;
        Session["AD3Count"] = null;
        Session["AD3DV"] = null;
        Session["AD4Count"] = null;
        Session["AD4DV"] = null;

        Execute("INSERT INTO UserSignOns (UserID, SignOnDate) VALUES(" +
            Session["User"].ToString() + ", GETDATE())");

        UpdateUserSession();
        //Set Users new session
        Random rad = new Random(timeNow.ToUniversalTime().Millisecond);
        Session["UserSession" + Session["User"].ToString()] = rad.Next();
        //Remove session 'Generic User' to tell the ads.aspx that the user has signed in.
        Session.Remove("GenericUser");
        Session["UserCountry"] = ds.Tables[0].Rows[0]["CatCountry"].ToString();
        Session["UserState"] = ds.Tables[0].Rows[0]["CatState"].ToString();
        Session["UserCity"] = ds.Tables[0].Rows[0]["CatCity"].ToString();

        Session["LocCountry"] = ds.Tables[0].Rows[0]["CatCountry"].ToString();
        Session["LocState"] = ds.Tables[0].Rows[0]["CatState"].ToString();
        Session["LocCity"] = ds.Tables[0].Rows[0]["CatCity"].ToString();
    }

    public void SendWelcome()
    {
        DataView dvUser = GetDataDV("SELECT * FROM Users WHERE User_ID=" + Session["User"].ToString());
        string Name = dvUser[0]["UserName"].ToString();
        string Body = "<span style=\"font-size: 12px;\">We're so happy to have you with us at <a style=\"text-decoration: " +
                        "underline; color: #09718F; \" href=\"http://HippoHappenings.com\">HippoHappenings.com</a>. " +
            "There's so many things possible on our site "+
            "and we don't want you to miss a thing. " +
            "Take a look below at all the Hippo capabilities and read more on our <a style=\"text-decoration: " +
                        "underline; color: #09718F;\" href=\"http://HippoHappenings.com/about\">about page</a>.<br/><br/>Here are a couple of suggestions to help you start exploring the Hippo: " +
            "<ol><li>Find <a style=\"text-decoration: " +
                        "underline; color: #09718F;\" href=\"http://HippoHappenings.com/event-search\">events</a> and <a style=\"text-decoration: " +
                        "underline; color: #09718F;\" href=\"http://HippoHappenings.com/trip-search\">adventures</a> in your city and add your favorites to your calendar, and </li><li>Share <a style=\"text-decoration: " +
                        "underline; color: #09718F;\" href=\"http://HippoHappenings.com/blog-event\">events" +
            "</a> and <a style=\"text-decoration: " +
                        "underline; color: #09718F;\" href=\"http://HippoHappenings.com/enter-trip\">adventures" +
            "</a> that you know about and help promote them or bring more participants to them.</li></ol> To get more out of the Hippo, " +
            "we suggest <a style=\"text-decoration: " +
                        "underline; color: #09718F;\" href=\"http://HippoHappenings.com/my-account\">inviting friends</a> to share your experience. They will be able to share events, adventures, locales and bulletins with you and <a style=\"text-decoration: " +
                        "underline; color: #09718F;;\" href=\"http://HippoHappenings.com/about\">much more</a>.<br/><br/>We hope this starts you off " +
            "on a great adventure on the Hippo ride. However, if you are having trouble, we're here to help. <a style=\"text-decoration: " +
                        "underline; color: #09718F;\" href=\"http://HippoHappenings.com/contact-us\">Contact Us</a>: you can always find the link on the bottom of the screen. " +
            "<br/><br/>And, don't forget to connect with us on <a style=\"text-decoration: " +
                        "underline; color: #09718F;\" href=\"http://twitter.com/HippoHappenings\">Twitter</a> and <a style=\"text-decoration: " +
                        "underline; color: #09718F;\" href=\"http://www.facebook.com/#!/pages/HippoHappenings/141790032527996\">" +
                        "Facebook</a>.</span><br/><br/>";
        string email = "<style type=\"text/css\">a.AddLink{font-family: Arial;color: #09718F;font-size: 12px;" +
            "text-decoration: underline;padding-bottom: 11px;}div.im " +
    "{color: #dcdada !important;}</style><div align=\"center\" style=\"width: 700px; " +
    "\">" +
    "<div align=\"center\" style=\"width: 500px;\">" +
    "<a href=\"http://hippohappenings.com\"><img style=\"border: 0;\" src=\"http://HippoHappenings.com/image/EmailHeader2.png\"/></a> <br/>" +
    "<div style=\"clear: both;\">" +
        "<div style=\"font-family: Arial;" +
            "font-size: 12px; line-height: 20px; " +
            "color: #535252;" +
            "font-weight: normal;" +
            "text-decoration: none;\" align=\"left\"><div align=\"center\" style=\"" +
                        "font-size: 25px;" +
                        "padding: 10px;padding-top: 20px; padding-bottom: 20px;\">Welcome "+Name+"</div>" +
                    Body + "<br/><div style=\"float: right;padding-right: " +
                    "35px;\" align=\"left\">Welcome and Have a Great Stay<br/>Aleksandra Czajka, CEO<br/>HippoHappenings Team<br/><br/></div></div>"+
        "</div>" +
        "<div style=\"clear: both;border: solid 1px #535252; background-color: #dedbdb; color: #535252; font-size: 11px;padding: " +
                    "10px; text-align: center;\">You are receiving this email because you signed up for HippoHappenings membership. "+
                    "To remove your account from our records write to <a style=\"color: #09718F;\" href=\"javascript:mailto:removeAccount@HippoHappenings.com\">removeAccount@HippoHappenings.com</a>. <br/>Copyright © " + DateTime.Now.Year.ToString() + " <a style=\"color: #09718F\" href=\"http://HippoHappenings.com\">HippoHappenings</a>, " +
        "LLC. All rights reserved. <br/>  <a style=\"color: #09718F;\" href=\"mailto:HippoHappenings@HippoHappenings.com\">Contact Us</a></div>" +
    "</div></div>";
        SendEmailNoFrills(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
             System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
             dvUser[0]["Email"].ToString(), email, "Welcome to HippoHappenings");
    }

    public string GetMonth(string month)
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

    public string GetDay2(int day, bool isShort)
    {
        if (isShort)
        {
            switch (day)
            {
                case 1:
                    return "Mon";
                    break;
                case 2:
                    return "Tues";
                    break;
                case 3:
                    return "Wed";
                    break;
                case 4:
                    return "Thur";
                    break;
                case 5:
                    return "Fri";
                    break;
                case 6:
                    return "Sat";
                    break;
                case 7:
                    return "Sun";
                    break;
                default:
                    return "Mon";
                    break;

            }
        }
        else
        {
            switch (day)
            {
                case 1:
                    return "Monday";
                    break;
                case 2:
                    return "Tuesday";
                    break;
                case 3:
                    return "Wednesday";
                    break;
                case 4:
                    return "Thursday";
                    break;
                case 5:
                    return "Friday";
                    break;
                case 6:
                    return "Saturday";
                    break;
                case 7:
                    return "Sunday";
                    break;
                default:
                    return "Monday";
                    break;

            }
        }
    }

    public string GetDay(int day, bool isShort)
    {
        if (isShort)
        {
            switch (day)
            {
                case 2:
                    return "Mon";
                    break;
                case 3:
                    return "Tues";
                    break;
                case 4:
                    return "Wed";
                    break;
                case 5:
                    return "Thur";
                    break;
                case 6:
                    return "Fri";
                    break;
                case 7:
                    return "Sat";
                    break;
                case 1:
                    return "Sun";
                    break;
                default:
                    return "Mon";
                    break;

            }
        }
        else
        {
            switch (day)
            {
                case 2:
                    return "Monday";
                    break;
                case 3:
                    return "Tuesday";
                    break;
                case 4:
                    return "Wednesday";
                    break;
                case 5:
                    return "Thursday";
                    break;
                case 6:
                    return "Friday";
                    break;
                case 7:
                    return "Saturday";
                    break;
                case 1:
                    return "Sunday";
                    break;
                default:
                    return "Mon";
                    break;

            }
        }
    }

    public string GetTime(string time)
    {
        TimeSpan timeTime = TimeSpan.Parse(time);

        string timeResult = timeTime.Hours.ToString();
        if (timeTime.Minutes != 0)
            timeResult += ":" + timeTime.Minutes.ToString();

        if (timeTime.Hours < 12)
            timeResult += " am";
        else
            timeResult += " pm";

        return timeResult;
    }

    public string SendEventNotification(string categoriesAndVenues, string venues, 
        string categories, string email, string textEmail, string country, string state, string subject)
    {
        string problem = "";
        try
        {
            string optOut = "<div style=\"font-size: 10px; color: #cccccc;padding: 20px; background-color: black; border: solid 1px #cccccc;margin-top: 40px;\">This notification is being sent to you "+
                "because you have the recommendation option for an event in a favorite category or favorite venue checked off. "+
                "To un-check this option, please visit <a href=\"http://HippoHappenings.com/my-account\">My Account</a> --> My Prefs --> Event Recommendation Preferences.</div>";

            SqlDbType[] types = { SqlDbType.NVarChar };
            object[] data = { state };
            subject = "A favorite event has been posted";
            if (categories != "")
                categories += " AND ";
            DataSet dsUsersVenues = GetDataWithParemeters("SELECT DISTINCT U.User_ID, U.UserName, U.Email FROM " +
                " UserVenues UV, Users U, UserPreferences UP" +
                " WHERE U.User_ID=UP.UserID AND UP.EmailPrefs LIKE '%3%' AND UV.UserID=U.User_ID AND " +
                venues, types, data);

            DataSet dsUsersCategories = GetDataWithParemeters("SELECT DISTINCT U.User_ID, U.UserName, U.Email FROM UserCategories UC, UserVenues UV, Users U, UserPreferences UP" +
                " WHERE U.User_ID=UP.UserID AND UP.EmailPrefs LIKE '%1%' AND " + categories + " UP.CatCountry = " + country +
                " AND UP.CatState=@p0 AND UC.UserID=U.User_ID ", types, data);
            
            if (dsUsersVenues.Tables.Count > 0)
                if (dsUsersVenues.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsUsersVenues.Tables[0].Rows.Count; i++)
                    {
                        SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                            System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                            dsUsersVenues.Tables[0].Rows[i]["Email"].ToString(),
                            email + optOut, "Favorite Venue: event has been posted");
                    }
                }

            if (dsUsersCategories.Tables.Count > 0)
                if (dsUsersCategories.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsUsersCategories.Tables[0].Rows.Count; i++)
                    {
                        SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                            System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                            dsUsersCategories.Tables[0].Rows[i]["Email"].ToString(),
                            email.Replace("%20", " ") + optOut, "Favorite Category: event has been posted");
                    }
                }

            string str = "SELECT DISTINCT U.User_ID, U.UserName, U.PhoneNumber, PP.Extension FROM " +
                "UserVenues UV, Users U, UserPreferences UP, PhoneProviders PP " +
                " WHERE U.PhoneProvider=PP.ID AND U.User_ID=UP.UserID AND UP.TextingPrefs LIKE '%3%' AND UV.UserID=U.User_ID AND " + venues;
            dsUsersVenues = GetDataWithParemeters(str, types, data);
            Session["str"] = str;
                dsUsersCategories = GetDataWithParemeters("SELECT DISTINCT U.User_ID, U.UserName, U.PhoneNumber, "+
                    "PP.Extension FROM UserCategories UC, Users U, UserPreferences UP, PhoneProviders PP " +
                " WHERE U.PhoneProvider=PP.ID AND U.User_ID=UP.UserID AND UP.TextingPrefs LIKE '%1%' AND " + categories + " UP.CatCountry = " + 
                country + " AND UP.CatState=@p0 AND UC.UserID=U.User_ID ", types, data);

                if (dsUsersVenues.Tables.Count > 0)
                    if (dsUsersVenues.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsUsersVenues.Tables[0].Rows.Count; i++)
                    {
                        if (dsUsersVenues.Tables[0].Rows[i]["PhoneNumber"].ToString().Trim() != "")
                        {
                            try
                            {
                                problem = MakeGoodPhone(dsUsersVenues.Tables[0].Rows[i]["PhoneNumber"].ToString()) + 
                                    dsUsersVenues.Tables[0].Rows[i]["Extension"].ToString();
                                SendText(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                                    MakeGoodPhone(dsUsersVenues.Tables[0].Rows[i]["PhoneNumber"].ToString()) +
                                    dsUsersVenues.Tables[0].Rows[i]["Extension"].ToString(),
                                    textEmail, "Favorite Venue Event Added");
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }

            if (dsUsersCategories.Tables.Count > 0)
                if (dsUsersCategories.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsUsersCategories.Tables[0].Rows.Count; i++)
                    {
                        if (dsUsersCategories.Tables[0].Rows[i]["PhoneNumber"].ToString().Trim() != "")
                        {
                            try
                            {
                                problem = MakeGoodPhone(dsUsersCategories.Tables[0].Rows[i]["PhoneNumber"].ToString()) +
                                    dsUsersCategories.Tables[0].Rows[i]["Extension"].ToString();
                                SendText(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                                    MakeGoodPhone(dsUsersCategories.Tables[0].Rows[i]["PhoneNumber"].ToString()) +
                                    dsUsersCategories.Tables[0].Rows[i]["Extension"].ToString(),
                                    textEmail, "Favorite Category Event Added");
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
            problem = "Success";
        }
        catch (Exception ex)
        {
            problem += ex.ToString();
        }

        return problem;
    }

    public void SendFriendPostedEventNotification(string userID, string eventID)
    {
        string optOut = "<div style=\"font-size: 10px; color: #cccccc;padding: 20px; background-color: black; border: solid 1px #cccccc;margin-top: 40px;\">This notification is being sent to you " +
    "because you have chosen to receive emails when any of your friends post an event. " +
    "To un-check this option, please visit <a href=\"http://HippoHappenings.com/my-account\">My Account</a> --> My Prefs --> Email Preferences.</div>";

        DataView dvFirendsIndividual = GetDataDV("SELECT * FROM UserFriendPrefs UFP, UserPreferences UP, Users U WHERE " +
    "UFP.Preferences LIKE '%2%'AND UP.UserID=UFP.UserID AND U.User_ID=UP.UserID AND UFP.FriendID=" + userID);

        string username = GetDataDV("SELECT * FROM Users WHERE User_ID=" + userID)[0]["UserName"].ToString();
        
        DataSet dsEvent = GetData("SELECT * FROM Events E, Venues V, Event_Occurance EO WHERE EO.EventID=E.ID AND E.Venue=V.ID AND E.ID=" + eventID);
        string name = dsEvent.Tables[0].Rows[0]["Header"].ToString();
        string email = "<a href=\"http://HippoHappenings.com/" + MakeNiceName(dsEvent.Tables[0].Rows[0]["Header"].ToString()) + "_" + eventID + "_Event\">" +
                    dsEvent.Tables[0].Rows[0]["Header"].ToString() +
                    "</a><br/><br/><a href=\"http://HippoHappenings.com/" + MakeNiceName(dsEvent.Tables[0].Rows[0]["Name"].ToString()) + "_" + dsEvent.Tables[0].Rows[0]["Venue"].ToString() + "_Venue\"> " +
                    dsEvent.Tables[0].Rows[0]["Name"].ToString() + "</a><br/><br/>" +
                    dsEvent.Tables[0].Rows[0]["DateTimeStart"].ToString() + "<br/><br/>" +
                    dsEvent.Tables[0].Rows[0]["Content"].ToString() + optOut;

        if (dvFirendsIndividual.Count > 0)
        {
            for (int i = 0; i < dvFirendsIndividual.Count; i++)
            {
                SendEmail("HippoHappenings@HippoHappenings.com", "HippoHappenings",
                    dvFirendsIndividual[i]["Email"].ToString(),
                    email, username + " has posted the event " + name + ".");
            }
        }

        dvFirendsIndividual = GetDataDV("SELECT *, PP.Extension AS Ex1 FROM UserFriendPrefs UFP, UserPreferences UP, PhoneProviders PP, Users U WHERE " +
            "UFP.Preferences LIKE '%3%'AND UP.UserID=UFP.UserID AND U.User_ID=UP.UserID AND PP.ID=U.PhoneProvider AND UFP.FriendID=" + userID);

        for (int i = 0; i < dvFirendsIndividual.Count; i++)
        {
            if (dvFirendsIndividual[i]["PhoneNumber"].ToString().Trim() != "")
            {

                try
                {
                    SendText(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                        System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                        MakeGoodPhone(dvFirendsIndividual[i]["PhoneNumber"].ToString()) +
                        dvFirendsIndividual[i]["Ex1"].ToString(),
                        username + " has posted the event '" + name +
                        "'.", "");
                }
                catch (Exception ex)
                {

                }
            }
        }

    }

    public void SendFriendPostedAdNotification(string userID, string eventID)
    {
        string optOut = "<div style=\"font-size: 10px; color: #cccccc;padding: 20px; background-color: black; border: solid 1px #cccccc;margin-top: 40px;\">This notification is being sent to you " +
            "because you have chosen to receive emails when any of your friends post an ad. " +
            "To un-check this option, please visit <a href=\"http://HippoHappenings.com/my-account\">My Account</a> --> My Prefs.</div>";


        DataView dvFirendsIndividual = GetDataDV("SELECT * FROM UserFriendPrefs UFP, UserPreferences UP, Users U WHERE " +
            "UFP.Preferences LIKE '%4%'AND UP.UserID=UFP.UserID AND U.User_ID=UP.UserID AND UFP.FriendID=" + userID);

        string username = GetDataDV("SELECT * FROM Users WHERE User_ID=" + userID)[0]["UserName"].ToString();

        DataSet dsEvent = GetData("SELECT * FROM Ads WHERE Ad_ID=" + eventID);
        string name = dsEvent.Tables[0].Rows[0]["Header"].ToString();
        string email = "<a href=\"http://HippoHappenings.com/" +
            MakeNiceName(dsEvent.Tables[0].Rows[0]["Header"].ToString()) + "_" + eventID + "_Ad\">" +
                    dsEvent.Tables[0].Rows[0]["Header"].ToString() +
                    "</a><br/><br/>" +
                    dsEvent.Tables[0].Rows[0]["Description"].ToString() + optOut;

        if (dvFirendsIndividual.Count > 0)
        {
            for (int i = 0; i < dvFirendsIndividual.Count; i++)
            {
                SendEmail("HippoHappenings@HippoHappenings.com", "HippoHappenings",
                    dvFirendsIndividual[i]["Email"].ToString(),
                    email, "Your friend "+username + " has posted the ad '" + name + "'.");
            }
        }

        dvFirendsIndividual = GetDataDV("SELECT *, PP.Extension AS Ex1 FROM UserFriendPrefs UFP, UserPreferences UP, Users U, PhoneProviders PP WHERE " +
            "UFP.Preferences LIKE '%5%'AND UP.UserID=UFP.UserID AND U.User_ID=UP.UserID AND PP.ID=U.PhoneProvider AND UFP.FriendID=" + userID);

        for (int i = 0; i < dvFirendsIndividual.Count; i++)
        {
            if (dvFirendsIndividual[i]["PhoneNumber"].ToString().Trim() != "")
            {

                try
                {
                    SendText(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                        System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                        MakeGoodPhone(dvFirendsIndividual[i]["PhoneNumber"].ToString()) +
                        dvFirendsIndividual[i]["Ex1"].ToString(),
                        username + " has posted the ad '" + name +
                        "'.", "");
                }
                catch (Exception ex)
                {

                }
            }
        }

    }

    public void SendFriendAddNotification(string userID, string eventID)
    {
        DataSet dsFriends = GetData("SELECT * FROM User_Friends UF, UserPreferences UP, Users U WHERE U.User_ID=UF.FriendID AND UF.UserID=" + userID +
            " AND UP.UserID=UF.FriendID AND UP.EmailPrefs LIKE '%2%'");


        DataSet dsEvent = GetData("SELECT * FROM Events E, Venues V, Event_Occurance EO WHERE EO.EventID=E.ID AND E.Venue=V.ID AND E.ID="+eventID);
        string name = dsEvent.Tables[0].Rows[0]["Header"].ToString();
        string username = GetDataDV("SELECT * FROM Users WHERE User_ID=" + userID)[0]["UserName"].ToString();
        string notTheseFriends = "";

        string email = "<a href=\"http://HippoHappenings.com/" + MakeNiceName(dsEvent.Tables[0].Rows[0]["Header"].ToString()) + "_" + eventID + "_Event\">" +
                    dsEvent.Tables[0].Rows[0]["Header"].ToString() +
                    "</a><br/><br/><a href=\"http://HippoHappenings.com/" + MakeNiceName(dsEvent.Tables[0].Rows[0]["Name"].ToString()) + "_" + dsEvent.Tables[0].Rows[0]["Venue"].ToString() + "_Venue\"> " +
                    dsEvent.Tables[0].Rows[0]["Name"].ToString() + "</a><br/><br/>" +
                    dsEvent.Tables[0].Rows[0]["DateTimeStart"].ToString() + "<br/><br/>" +
                    dsEvent.Tables[0].Rows[0]["Content"].ToString();
                
        if(dsFriends.Tables.Count > 0)
            if (dsFriends.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsFriends.Tables[0].Rows.Count; i++)
                {
                    notTheseFriends = " AND FriendID <> " + dsFriends.Tables[0].Rows[i]["User_ID"].ToString();
                    SendEmail("HippoHappenings@HippoHappenings.com", "HippoHappenings", dsFriends.Tables[0].Rows[i]["Email"].ToString(),
                        email, username + " has added "+name+" to their calendar. ");
                }
            }

        DataView dvFirendsIndividual = GetDataDV("SELECT * FROM UserFriendPrefs UFP, UserPreferences UP, Users U WHERE " +
            "UFP.Preferences LIKE '%0%'AND UP.UserID=UFP.UserID AND U.User_ID=UP.UserID AND UFP.FriendID=" + userID + notTheseFriends);
        if (dvFirendsIndividual.Count > 0)
        {
            for (int i = 0; i < dvFirendsIndividual.Count; i++)
            {
                SendEmail("HippoHappenings@HippoHappenings.com", "HippoHappenings",
                    dvFirendsIndividual[i]["Email"].ToString(),
                    email, username + " has added " + name + " to their calendar. ");
            }
        }


        dsFriends = GetData("SELECT DISTINCT U.User_ID, U.UserName, U.PhoneNumber, PP.Extension FROM User_Friends UF, UserPreferences UP, Users U, PhoneProviders PP WHERE U.PhoneProvider=PP.ID AND U.User_ID=UF.FriendID AND UF.UserID=" + userID +
            " AND UP.UserID=UF.FriendID AND UP.TextingPrefs LIKE '%2%'");
        notTheseFriends = "";
        if (dsFriends.Tables.Count > 0)
            if (dsFriends.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsFriends.Tables[0].Rows.Count; i++)
                {
                    if (dsFriends.Tables[0].Rows[i]["PhoneNumber"].ToString().Trim() != "")
                    {
                       
                        try
                        {
                            notTheseFriends = " AND FriendID <> " + dsFriends.Tables[0].Rows[i]["User_ID"].ToString();
                            SendText(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                                MakeGoodPhone(dsFriends.Tables[0].Rows[i]["PhoneNumber"].ToString()) + 
                                dsFriends.Tables[0].Rows[i]["Extension"].ToString(),
                                username+ " has added event '"+name + 
                                "' to their calendar. ", "");
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }

        dvFirendsIndividual = GetDataDV("SELECT *, PP.Extension AS Ex1 FROM UserFriendPrefs UFP, UserPreferences UP, Users U, PhoneProviders PP WHERE " +
            "UFP.Preferences LIKE '%1%' AND PP.ID=U.PhoneProvider AND UP.UserID=UFP.UserID AND U.User_ID = UP.UserID AND UFP.FriendID=" +
            userID + notTheseFriends);

        for (int i = 0; i < dvFirendsIndividual.Count; i++)
        {
            if (dvFirendsIndividual[i]["PhoneNumber"].ToString().Trim() != "")
            {

                try
                {
                    SendText(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                        System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                        MakeGoodPhone(dvFirendsIndividual[i]["PhoneNumber"].ToString()) +
                        dvFirendsIndividual[i]["Ex1"].ToString(),
                        username + " has added event '"+ name +
                        "' to their calendar. ", "");
                }
                catch (Exception ex)
                {

                }
            }
        }
    }

    public bool IsThisWeek(DateTime date)
    {
        DayOfWeek today = timeNow.DayOfWeek;
        DayOfWeek dayFromDate = date.DayOfWeek;

        if (timeNow.Date == date.Date)
            return true;

        DateTime minDate = new DateTime();
        DateTime maxDate = new DateTime();

        switch (timeNow.DayOfWeek)
        {
            case DayOfWeek.Sunday:
                minDate = timeNow;
                maxDate = timeNow.AddDays(6);
                break;
            case DayOfWeek.Monday:
                minDate = timeNow.Subtract(new TimeSpan(1, 0, 0, 0, 0));
                maxDate = timeNow.AddDays(5);
                break;
            case DayOfWeek.Tuesday:
                minDate = timeNow.Subtract(new TimeSpan(2, 0, 0, 0, 0));
                maxDate = timeNow.AddDays(4);
                break;
            case DayOfWeek.Wednesday:
                minDate = timeNow.Subtract(new TimeSpan(3, 0, 0, 0, 0));
                maxDate = timeNow.AddDays(3);
                break;
            case DayOfWeek.Thursday:
                minDate = timeNow.Subtract(new TimeSpan(4, 0, 0, 0, 0));
                maxDate = timeNow.AddDays(2);
                break;
            case DayOfWeek.Friday:
                minDate = timeNow.Subtract(new TimeSpan(5, 0, 0, 0, 0));
                maxDate = timeNow.AddDays(1);
                break;
            case DayOfWeek.Saturday:
                minDate = timeNow.Subtract(new TimeSpan(6, 0, 0, 0, 0));
                maxDate = timeNow;
                break;
        }

        DateTime dateToCheck = DateTime.Parse(date.Month + "/" + date.Day + "/" + date.Year);
        DateTime dateMin = DateTime.Parse(minDate.Month + "/" + minDate.Day + "/" + minDate.Year);
        DateTime dateMax = DateTime.Parse(maxDate.Month + "/" + maxDate.Day + "/" + maxDate.Year);
        if (dateToCheck >= dateMin && dateToCheck <= dateMax)
            return true;
        else
            return false;
        
    }

    public DataSet GetData(string command)
    {
        try
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            SqlCommand cmd = new SqlCommand(command, conn);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            conn.Close();
            return ds;
        }
        finally
        {
            conn.Close();
        }
    }

    public DataView GetDataDV(string command)
    {
        DataSet ds = GetData(command);
        DataView dv = new DataView(ds.Tables[0], "", "", DataViewRowState.CurrentRows);

        return dv;
    }

    public DataSet GetDataWithParemeters(string command, SqlDbType[] types, object[] data)
    {
        try
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            SqlCommand cmd = new SqlCommand(command, conn);

            for (int i = 0; i < types.Length; i++)
            {
                cmd.Parameters.Add("@p" + i, types[i]).Value = data[i];
            }
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            conn.Close();

            return ds;
        }
        finally
        {
            conn.Close();
        }
    }

    public void Execute(string command)
    {
        try
        {
            if (conn.State != ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand(command, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        finally
        {
            conn.Close();
        }
    }

    public void ExecuteWithParemeters(string command, SqlDbType [] types, object [] data)
    {
        try
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand(command, conn);
            for (int i = 0; i < types.Length; i++)
            {
                cmd.Parameters.Add("@p" + i, types[i]).Value = data[i];
            }
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        finally
        {
            conn.Close();
        }
    }

    public bool TrapKey(string text, int keyType)
    {
        switch (keyType)
        {

            case 1:
                //case for a string
                if (text.Contains("'") || text.Contains(".") || text.Contains(";") || text.Contains(":") || text.Contains("<")
                    || text.Contains(">") || text.Contains("~") || text.Contains("`") || text.Contains("!") || text.Contains("@")
                    || text.Contains("#") || text.Contains("$") || text.Contains("%") || text.Contains("^") || text.Contains("&")
                    || text.Contains("*") || text.Contains("(") || text.Contains(")") || text.Contains("-") || text.Contains("_")
                    || text.Contains("+") || text.Contains("=") || text.Contains("[") || text.Contains("]") || text.Contains("{")
                    || text.Contains("}") || text.Contains("|") || text.Contains("\"") || text.Contains("?") || text.Contains("/"))
                    return false;
                else
                    return true;
                break;
            case 2:
                //case for an email (allows '.', '_' and '@')
                if (text.Contains("'") || text.Contains(";") || text.Contains(":") || text.Contains("<")
                    || text.Contains(">") || text.Contains("~") || text.Contains("`") || text.Contains("!")
                    || text.Contains("#") || text.Contains("$") || text.Contains("%") || text.Contains("^") || text.Contains("&")
                    || text.Contains("*") || text.Contains("(") || text.Contains(")")
                    || text.Contains("+") || text.Contains("=") || text.Contains("[") || text.Contains("]") || text.Contains("{")
                    || text.Contains("}") || text.Contains("|") || text.Contains("\"") || text.Contains("?") || text.Contains("/"))
                    return false;
                else
                    return true;
                break;
            default: return false; break;


        }
    }

    // validate email address
    public bool ValidateEmail(string email)
    {
        if (TrapKey(email, 2))
        {
            if (email.Contains("@"))
            {
                char[] delim = { '@' };
                string[] emailTokens = email.Split(delim);

                if (emailTokens.Length != 2)
                    return false;
                else if (emailTokens[0].Length == 0 || emailTokens[1].Length == 0)
                    return false;

                char[] delimDot = { '.' };
                string[] emailDomainTokens = emailTokens[1].Split(delimDot);

                if (emailDomainTokens.Length != 2)
                    return false;
                else if (emailDomainTokens[0].Length == 0 || emailDomainTokens[1].Length == 0)
                    return false;
                else
                    return true;
            }
            else
                return false;
        }
        else
            return false;
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

    private void buildCommonHash()
    {
        commonWordHash.Add("A", "1");
        commonWordHash.Add("TOO", "1");
        commonWordHash.Add("TO", "1");
        commonWordHash.Add("FROM", "1");
        commonWordHash.Add("WHAT", "1");
        commonWordHash.Add("WHERE", "1");
        commonWordHash.Add("WHEN", "1");
        commonWordHash.Add("THEN", "1");
        commonWordHash.Add("THERE", "1");
        commonWordHash.Add("THAT", "1");
        commonWordHash.Add("THEM", "1");
        commonWordHash.Add("YOU", "1");
        commonWordHash.Add("ON", "1");
        commonWordHash.Add("UNDER", "1");
        commonWordHash.Add("OVER", "1");
        commonWordHash.Add("IN", "1");
        commonWordHash.Add("AROUND", "1");
        commonWordHash.Add("ME", "1");
        commonWordHash.Add("SHE", "1");
        commonWordHash.Add("AT", "1");
        commonWordHash.Add("AND", "1");
        commonWordHash.Add("SURE", "1");
        commonWordHash.Add("YOUR", "1");
        commonWordHash.Add("HIS", "1");
        commonWordHash.Add("HERS", "1");
        commonWordHash.Add("BE", "1");
        commonWordHash.Add("THIS", "1");
        commonWordHash.Add("THE", "1");
        commonWordHash.Add("ABOUT", "1");
        commonWordHash.Add("AN", "1");
        commonWordHash.Add("IS", "1");
        commonWordHash.Add("ARE", "1");
        commonWordHash.Add("WERE", "1");
        commonWordHash.Add("WITH", "1");
        commonWordHash.Add("YES", "1");
        commonWordHash.Add("NOT", "1");
        commonWordHash.Add("NO", "1");
        commonWordHash.Add("AS", "1");
        commonWordHash.Add("ITS", "1");
        commonWordHash.Add("IT'S", "1");
        commonWordHash.Add("YOUR'S", "1");
        commonWordHash.Add("IM", "1");
        commonWordHash.Add("I'M", "1");
        commonWordHash.Add("OF", "1");
        commonWordHash.Add("&", "1");
        commonWordHash.Add("OUR", "1");
        commonWordHash.Add("THEIR", "1");
        commonWordHash.Add("IT", "1");
        commonWordHash.Add("WAS", "1");
        commonWordHash.Add("FOR", "1");
        commonWordHash.Add("OTHER", "1");
        commonWordHash.Add("USE", "1");
        commonWordHash.Add("USED", "1");
        commonWordHash.Add("THOSE", "1");
        commonWordHash.Add("END", "1");
        commonWordHash.Add("START", "1");

        commonWordHash.Add("WROTE", "1");
        commonWordHash.Add("HAD", "1");
        commonWordHash.Add("HER", "1");
        commonWordHash.Add("HE", "1");
        commonWordHash.Add("HIM", "1");

        commonWordHash.Add("SUCH", "1");
        commonWordHash.Add("SO", "1");
        commonWordHash.Add("SOON", "1");
        commonWordHash.Add("NEVER", "1");
        commonWordHash.Add("WE", "1");
        commonWordHash.Add("OUT", "1");

        commonWordHash.Add("AFTER", "1");
        commonWordHash.Add("BEFORE", "1");
        commonWordHash.Add("MIDDLE", "1");
        commonWordHash.Add("LIST", "1");
        commonWordHash.Add("UP", "1");
        commonWordHash.Add("DOWN", "1");
        commonWordHash.Add("BACK", "1");

        commonWordHash.Add("FRONT", "1");
        commonWordHash.Add("BY", "1");
        commonWordHash.Add("HOW", "1");
        commonWordHash.Add("WHY", "1");
        commonWordHash.Add("MANY", "1");
        commonWordHash.Add("LITTLE", "1");
        commonWordHash.Add("SLOW", "1");
        commonWordHash.Add("FAST", "1");

        commonWordHash.Add("WILL", "1");
        commonWordHash.Add("MAX", "1");
        commonWordHash.Add("MIN", "1");

    }

    public bool isCommonWord(string word)
    {
        if(commonWordHash.Count == 0)
            buildCommonHash();
        if (commonWordHash.Contains(word.ToUpper()))
            return true;
        else
        {
            try
            {
                int.Parse(word);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

    public string getTags(DataSet ds, bool isCloud, bool isWindow)
    {
        string tags = "";

        string link = "";
        string category = "";
        string size = "";
        string clas = "";
        if (isCloud)
        {
            clas = "Link";
            if (tag_type == tagType.GROUP_EVENT)
            {
                clas = "GreenLink";
            }
        }
        else
            clas = "TagLink";

        string whichH = "1";

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            link = ds.Tables[0].Rows[i]["ID"].ToString();
            category = ds.Tables[0].Rows[i]["CategoryName"].ToString();

            if (isCloud)
            {
                size = ds.Tables[0].Rows[i]["tagSize"].ToString();
                if (size == "22")
                    whichH = "1";
                else
                    whichH = "2";
            }
            else
            {
                whichH = "2";
                size = "11";
            }

            

            switch (tag_type)
            {
                case tagType.AD:
                    if (isWindow)
                    {
                        tags += "<h" + whichH + " class=\"Tags\" ><a class=\"" + clas + "\" href=\"javascript:CloseWindow('" + MakeNiceName(category) + "_Ad_Category')\" style=\"font-size: " + size + "px;\" > " + category + "</a></h" + whichH + ">";
                    }
                    else
                    {
                        tags += "<h" + whichH + " class=\"Tags\" ><a class=\"" + clas + "\" href=\"" + MakeNiceName(category) + "_Ad_Category\" style=\"font-size: " + size + "px;\" > " + category + "</a></h" + whichH + ">";
                    }
                    break;
                case tagType.EVENT:
                    if (isWindow)
                    {
                        tags += "<h" + whichH + " class=\"Tags\" ><a class=\"" + clas + "\" href=\"javascript:CloseWindow('" + MakeNiceName(category) + "_Event_Category')\" style=\"font-size: " + size + "px;\" > " + category + "</a></h" + whichH + ">";
                    }
                    else
                    {
                        tags += "<h" + whichH + " class=\"Tags\" ><a class=\"" + clas + "\" href=\"" + MakeNiceName(category) + "_Event_Category\" style=\"font-size: " + size + "px;\" > " + category + "</a></h" + whichH + ">";
                    }
                    break;
                case tagType.VENUE:
                    if (isWindow)
                    {
                        tags += "<h" + whichH + " class=\"Tags\" ><a class=\"" + clas + "\" href=\"javascript:CloseWindow('" + MakeNiceName(category) + "_Venue_Category')\" style=\"font-size: " + size + "px;\" > " + category + "</a></h" + whichH + ">";
                    }
                    else
                    {
                        tags += "<h" + whichH + " class=\"Tags\" ><a class=\"" + clas + "\" href=\"" + MakeNiceName(category) + "_Venue_Category\" style=\"font-size: " + size + "px;\" > " + category + "</a></h" + whichH + ">";
                    }
                    break;
                case tagType.TRIP:
                    if (isWindow)
                    {
                        tags += "<h" + whichH + " class=\"Tags\" ><a class=\"" + clas + "\" href=\"javascript:CloseWindow('" + MakeNiceName(category) + "_Trip_Category')\" style=\"font-size: " + size + "px;\" > " + category + "</a></h" + whichH + ">";
                    }
                    else
                    {
                        tags += "<h" + whichH + " class=\"Tags\" ><a class=\"" + clas + "\" href=\"" + MakeNiceName(category) + "_Trip_Category\" style=\"font-size: " + size + "px;\" > " + category + "</a></h" + whichH + ">";
                    }
                    break;
                case tagType.GROUP:
                    if (isWindow)
                    {
                        tags += "<h" + whichH + "><a class=\"" + clas + "\" href=\"javascript:CloseWindow('" +
                            MakeNiceName(category) + "_" + link + "_Group_Category')\" style=\"font-size: " +
                            size + "px;\" > " + category + "</a></h" + whichH + ">";
                    }
                    else
                    {
                        if (isCloud)
                        {
                            tags += "<div style=\"float: left;\"><div style=\"height: 29px; background-position: left; background-repeat: no-repeat; background-image: url(images/CatRight.png); " +
                                "padding-left: 3px;\"><div style=\"height: 29px; background-repeat: no-repeat; background-position: right; " +
                                "background-image: url(images/CatLeft.png); " +
                                "padding-right: 4px;\"><div style=\"height: 29px; background-repeat: repeat-x; " +
                                "background-image: url(images/CatMiddle.png);\"><a class=\"" + clas + "\" href=\"" + MakeNiceName(category) + "_" + link +
                                "_Group_Category\" style=\"font-size: " + size + 
                                "px; line-height: 28px !important;\" > " + category + "</a></div></div></div></div>";
                        }
                        else
                        {
                            tags += "<h" + whichH + "><a class=\"" + clas + "\" href=\"" + MakeNiceName(category) + "_" + link +
                                "_Group_Category\" style=\"font-size: " + size + "px; \" > " + category + "</a></h" + whichH + ">";
                        }
                    }
                    break;
                case tagType.GROUP_EVENT:
                    
                    if (isWindow)
                    {
                        tags += "<a class=\"" + clas + "\" href=\"javascript:CloseWindow('" +
                            MakeNiceName(category) + "_" + link + "_GroupEvent_Category')\" style=\"font-size: " + size + "px;\" > " + category + "</a>";
                    }
                    else
                    {
                        if (isCloud)
                        {
                            tags += "<div style=\"float: left;\"><div style=\"height: 29px; background-position: left; background-repeat: no-repeat; background-image: url(images/CatRight.png); " +
                                "padding-left: 3px;\"><div style=\"height: 29px; background-repeat: no-repeat; background-position: right; " +
                                "background-image: url(images/CatLeft.png); " +
                                "padding-right: 4px;\"><div style=\"height: 29px; background-repeat: repeat-x; " +
                                "background-image: url(images/CatMiddle.png);\"><a class=\"" + clas + "\" href=\"" + 
                                MakeNiceName(category) +
                                "_" + link + "_GroupEvent_Category\" style=\"font-size: " + size + "px; line-height: 28px !important;\" > " +
                                category + "</a></div></div></div></div>";
                        }
                        else
                        {
                            tags += "<a class=\"" + clas + "\" href=\"" + MakeNiceName(category) +
                                "_" + link + "_GroupEvent_Category\" style=\"font-size: " + size + "px;\" > " + category + "</a>";
                        }
                    }
                    break;
                default: break;
            }
        }

        return tags;
    }

    public string getTagsRestricted(DataSet ds, bool isCloud, bool isWindow, int tagLength)
    {
        string tags = "";

        string link = "";
        string category = "";
        string size = "";
        string clas = "";
        if (isCloud)
        {
            clas = "Link";
            if (tag_type == tagType.GROUP_EVENT)
            {
                clas = "GreenLink";
            }
        }
        else
            clas = "TagLink";

        int actualTagLength = 0;
        bool isTerminated = false;
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            if (!isTerminated)
            {
                link = ds.Tables[0].Rows[i]["ID"].ToString();
                category = ds.Tables[0].Rows[i]["CategoryName"].ToString();
                if (isCloud)
                    size = ds.Tables[0].Rows[i]["tagSize"].ToString();
                else
                    size = "11";

                switch (tag_type)
                {
                    case tagType.AD:
                        if (isWindow)
                        {
                            tags += "<a class=\"" + clas + "\" href=\"javascript:CloseWindow('" + MakeNiceName(category) + "_Ad_Category')\" style=\"font-size: " + size + "px;\" > " + category + "</a>";
                        }
                        else
                        {
                            actualTagLength += category.Length + 1;
                            if (actualTagLength >= tagLength)
                            {
                                int leftOver = tagLength - (actualTagLength - category.Length - 1);
                                if (leftOver >= 3)
                                    tags += "<span class=\"" + clas + "\" style=\"font-size: " + size + "px;\" >...</span>";
                                else if (leftOver >= 2)
                                    tags += "<span class=\"" + clas + "\" style=\"font-size: " + size + "px;\" >..</span>";
                                isTerminated = true;
                                break;
                            }
                            else
                            {
                                tags += "<a class=\"" + clas + "\" href=\"" + MakeNiceName(category) + "_Ad_Category\" style=\"font-size: " + size + "px;\" > " + category + "</a>";
                            }
                        }
                        break;
                    case tagType.EVENT:
                        if (isWindow)
                        {
                            tags += "<a class=\"" + clas + "\" href=\"javascript:CloseWindow('" + MakeNiceName(category) + "_Event_Category')\" style=\"font-size: " + size + "px;\" > " + category + "</a>";
                        }
                        else
                        {
                            tags += "<a class=\"" + clas + "\" href=\"" + MakeNiceName(category) + "_Event_Category\" style=\"font-size: " + size + "px;\" > " + category + "</a>";
                        }
                        break;
                    case tagType.VENUE:
                        if (isWindow)
                        {
                            tags += "<a class=\"" + clas + "\" href=\"javascript:CloseWindow('" + MakeNiceName(category) + "_Venue_Category')\" style=\"font-size: " + size + "px;\" > " + category + "</a>";
                        }
                        else
                        {
                            actualTagLength += category.Length + 1;
                            if (actualTagLength >= tagLength)
                            {
                                int leftOver = tagLength - (actualTagLength - category.Length - 1);
                                if (leftOver >= 3)
                                    tags += "<span class=\"" + clas + "\" style=\"font-size: " + size + "px;\" >...</span>";
                                else if (leftOver >= 2)
                                    tags += "<span class=\"" + clas + "\" style=\"font-size: " + size + "px;\" >..</span>";
                                isTerminated = true;
                                break;
                            }
                            else
                            {
                                tags += "<a class=\"" + clas + "\" href=\"" + MakeNiceName(category) + "_Venue_Category\" style=\"font-size: " + size + "px;\" > " + category + "</a>";
                            }
                        }
                        break;
                    case tagType.TRIP:
                        if (isWindow)
                        {
                            tags += "<a class=\"" + clas + "\" href=\"javascript:CloseWindow('" + MakeNiceName(category) + "_Trip_Category')\" style=\"font-size: " + size + "px;\" > " + category + "</a>";
                        }
                        else
                        {
                            tags += "<a class=\"" + clas + "\" href=\"" + MakeNiceName(category) + "_Trip_Category\" style=\"font-size: " + size + "px;\" > " + category + "</a>";
                        }
                        break;
                    case tagType.GROUP:
                        if (isWindow)
                        {
                            tags += "<a class=\"" + clas + "\" href=\"javascript:CloseWindow('" +
                                MakeNiceName(category) + "_" + link + "_Group_Category')\" style=\"font-size: " +
                                size + "px;\" > " + category + "</a>";
                        }
                        else
                        {
                            if (isCloud)
                            {
                                tags += "<div style=\"float: left;\"><div style=\"height: 29px; background-position: left; background-repeat: no-repeat; background-image: url(images/CatRight.png); " +
                                    "padding-left: 3px;\"><div style=\"height: 29px; background-repeat: no-repeat; background-position: right; " +
                                    "background-image: url(images/CatLeft.png); " +
                                    "padding-right: 4px;\"><div style=\"height: 29px; background-repeat: repeat-x; " +
                                    "background-image: url(images/CatMiddle.png);\"><a class=\"" + clas + "\" href=\"" + MakeNiceName(category) + "_" + link +
                                    "_Group_Category\" style=\"font-size: " + size +
                                    "px; line-height: 28px !important;\" > " + category + "</a></div></div></div></div>";
                            }
                            else
                            {
                                tags += "<a class=\"" + clas + "\" href=\"" + MakeNiceName(category) + "_" + link +
                                    "_Group_Category\" style=\"font-size: " + size + "px; \" > " + category + "</a>";
                            }
                        }
                        break;
                    case tagType.GROUP_EVENT:

                        if (isWindow)
                        {
                            tags += "<a class=\"" + clas + "\" href=\"javascript:CloseWindow('" +
                                MakeNiceName(category) + "_" + link + "_GroupEvent_Category')\" style=\"font-size: " + size + "px;\" > " + category + "</a>";
                        }
                        else
                        {
                            if (isCloud)
                            {
                                tags += "<div style=\"float: left;\"><div style=\"height: 29px; background-position: left; background-repeat: no-repeat; background-image: url(images/CatRight.png); " +
                                    "padding-left: 3px;\"><div style=\"height: 29px; background-repeat: no-repeat; background-position: right; " +
                                    "background-image: url(images/CatLeft.png); " +
                                    "padding-right: 4px;\"><div style=\"height: 29px; background-repeat: repeat-x; " +
                                    "background-image: url(images/CatMiddle.png);\"><a class=\"" + clas + "\" href=\"" +
                                    MakeNiceName(category) +
                                    "_" + link + "_GroupEvent_Category\" style=\"font-size: " + size + "px; line-height: 28px !important;\" > " +
                                    category + "</a></div></div></div></div>";
                            }
                            else
                            {
                                tags += "<a class=\"" + clas + "\" href=\"" + MakeNiceName(category) +
                                    "_" + link + "_GroupEvent_Category\" style=\"font-size: " + size + "px;\" > " + category + "</a>";
                            }
                        }
                        break;
                    default: break;
                }
            }
            else
            {
                break;
            }
        }

        return tags;
    }

    public Int64 RemoveNoneNumbers(string str)
    {
        string temp = "";
        int tempInt = 0;
        for (int i = 0; i < str.Length; i++)
        {
            try
            {
                tempInt = int.Parse(str[i].ToString());
                temp += str[i];
            }
            catch (Exception ex)
            {

            }
        }

        if (temp != "")
            return Int64.Parse(temp);
        else
            return 0;
    }

    public void SendEmailNoFrills(string From, string FromName, string To, string Body, string Subject)
    {

        string SERVER = System.Configuration.ConfigurationManager.AppSettings["smtpserver"].ToString();
        MailMessage oMail = new MailMessage();

        MailAddress from = new MailAddress(From, FromName);
        MailAddress to = new MailAddress(To);
        MailMessage msg = new MailMessage(from, to);

        msg.Subject = Subject;
        msg.IsBodyHtml = true;
        msg.Body = Body;

        System.Net.Mail.SmtpClient smtp = new SmtpClient(SERVER);
        //smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings.Get("smtpuser"), ConfigurationManager.AppSettings.Get("smtppass"));
        //smtp.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;  
        bool passed = false;
        try
        {
            smtp.Send(msg);
            passed = true;
        }
        catch (SmtpFailedRecipientException ex)
        {
            bool tryNextEmail = false;
            string nextEmail = "";
            if (FromName == System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString())
            {
                tryNextEmail = true;
                nextEmail = "hippo1@hippohappenings.com";
            }
            else if (From.ToLower() == "hippo1@hippohappenings.com" ||
                From.ToLower() == "hippo2@hippohappenings.com" ||
                From.ToLower() == "hippo3@hippohappenings.com" ||
                From.ToLower() == "hippo4@hippohappenings.com" ||
                From.ToLower() == "hippo5@hippohappenings.com" ||
                From.ToLower() == "hippo6@hippohappenings.com" ||
                From.ToLower() == "hippo7@hippohappenings.com" ||
                From.ToLower() == "hippo8@hippohappenings.com" ||
                From.ToLower() == "hippo9@hippohappenings.com")
            {
                string temp = From.ToLower().Replace("@hippohappenings.com", "").Replace("hippo", "");
                nextEmail = "hippo" + (int.Parse(temp) + 1).ToString() + "@hippohappenings.com";
            }

            if (tryNextEmail)
            {
                SendEmailNoFrills(nextEmail, FromName, To, Body, Subject);
            }
        }

        oMail = null; // free up resources
    }

    public void SendEmail(string From, string FromName, string To, string Body, string Subject)
    {
         //String strSmtpServer = ConfigurationManager.AppSettings.Get("smtpserver");
            //String strSmtpUser = ConfigurationManager.AppSettings.Get("smtpuser");
            //String strSmtpPass = ConfigurationManager.AppSettings.Get("smtppass");
            //System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(strSmtpServer);
            //smtp.Credentials = new System.Net.NetworkCredential(strSmtpUser, strSmtpPass);

            //MailAddress from = new MailAddress(From, FromName);
            //MailAddress to = new MailAddress(To, "name");
            //MailMessage msg = new MailMessage(from, to);

            //msg.Subject = Subject;
            //msg.IsBodyHtml = true;
            //msg.Body = Body;
            //smtp.Send(msg);

            string SERVER = System.Configuration.ConfigurationManager.AppSettings["smtpserver"].ToString();
            MailMessage oMail = new MailMessage();

            MailAddress from = new MailAddress(From, FromName);
            MailAddress to = new MailAddress(To);
            MailMessage msg = new MailMessage(from, to);

            Body = Body.Replace("<a", "<a style=\"cursor: pointer;font-family: Arial;"+
            "color: #09718F;font-size: 12px;text-decoration: underline;\" ");

            msg.Subject = Subject;
            msg.IsBodyHtml = true;
            msg.Body = "<style type=\"text/css\">div.im {color: #535252 !important;} .im{color: White !important;}</style><div align=\"center\" style=\"width: 700px;\"><div align=\"center\" style=\"width: 500px;\">" +
                "<a href=\"http://hippohappenings.com\"><img style=\"border: 0;\" src=\"http://HippoHappenings.com/image/EmailHeader2.png\"/></a> "+
                "<br/><div align=\"left\" style=\"font-family: Arial;" +
                "font-size: 12px;" +
                "color: #535252; padding: 10px;\">" + Body +
                "</div><br/><div style=\"border-top: solid 1px #dedbdb; height: 80px; color: #535252; padding-top: " +
                "20px; text-align: center;\">Copyright © "+DateTime.Now.Year.ToString()+
                " <a style=\"color: #09718F;\" href=\"http://HippoHappenings.com\">HippoHappenings</a>, " +
                "LLC. All rights reserved. <br/>  <a style=\"color: #09718F;\" href=\"mailto:HippoHappenings@HippoHappenings.com\">Contact Us</a></div></div></div>";

            System.Net.Mail.SmtpClient smtp = new SmtpClient(SERVER);
            //smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings.Get("smtpuser"), ConfigurationManager.AppSettings.Get("smtppass"));
            //smtp.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;  
            bool passed = false;
            //while (!passed)
            //{
            try
            {
                smtp.Send(msg);
                passed = true;
            }
            catch (SmtpFailedRecipientException ex)
            {
                bool tryNextEmail = false;
                string nextEmail = "";
                if (FromName == System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString())
                {
                    tryNextEmail = true;
                    nextEmail = "hippo1@hippohappenings.com";
                }
                else if (From.ToLower() == "hippo1@hippohappenings.com" ||
                    From.ToLower() == "hippo2@hippohappenings.com" ||
                    From.ToLower() == "hippo3@hippohappenings.com" ||
                    From.ToLower() == "hippo4@hippohappenings.com" ||
                    From.ToLower() == "hippo5@hippohappenings.com" ||
                    From.ToLower() == "hippo6@hippohappenings.com" ||
                    From.ToLower() == "hippo7@hippohappenings.com" ||
                    From.ToLower() == "hippo8@hippohappenings.com" ||
                    From.ToLower() == "hippo9@hippohappenings.com")
                {
                    string temp = From.ToLower().Replace("@hippohappenings.com", "").Replace("hippo", "");
                    nextEmail = "hippo" + (int.Parse(temp) + 1).ToString() + "@hippohappenings.com";
                }

                if (tryNextEmail)
                {
                    SendEmail(nextEmail, FromName, To, Body, Subject);
                }
            }
            catch (Exception ex)
            {

            }
            //}

            oMail = null; // free up resources
           
        
    }

    public void SendText(string From, string FromName, string To, string Body, string Subject)
    {
        //String strSmtpServer = ConfigurationManager.AppSettings.Get("smtpserver");
        //String strSmtpUser = ConfigurationManager.AppSettings.Get("smtpuser");
        //String strSmtpPass = ConfigurationManager.AppSettings.Get("smtppass");
        //System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(strSmtpServer);
        //smtp.Credentials = new System.Net.NetworkCredential(strSmtpUser, strSmtpPass);

        //MailAddress from = new MailAddress(From, FromName);
        //MailAddress to = new MailAddress(To, "name");
        //MailMessage msg = new MailMessage(from, to);

        //msg.Subject = Subject;
        //msg.IsBodyHtml = true;
        //msg.Body = Body;
        //smtp.Send(msg);

        string SERVER = System.Configuration.ConfigurationManager.AppSettings["smtpserver"].ToString();
        MailMessage oMail = new MailMessage();

        MailAddress from = new MailAddress(From, FromName);
        MailAddress to = new MailAddress(To);
        MailMessage msg = new MailMessage(from, to);

        msg.Subject = Subject;
        msg.IsBodyHtml = false;
        msg.Subject = Subject;
        msg.Body = Body;
        System.Net.Mail.SmtpClient smtp = new SmtpClient(SERVER);

        bool passed = false;
        //while (!passed)
        //{
            try
            {
                smtp.Send(msg);
                passed = true;
            }
            catch (SmtpFailedRecipientException ex)
            {
                bool tryNextEmail = false;
                string nextEmail = "";
                if (FromName == System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString())
                {
                    tryNextEmail = true;
                    nextEmail = "hippo1@hippohappenings.com";
                }
                else if (From.ToLower() == "hippo1@hippohappenings.com" ||
                    From.ToLower() == "hippo2@hippohappenings.com" ||
                    From.ToLower() == "hippo3@hippohappenings.com" ||
                    From.ToLower() == "hippo4@hippohappenings.com" ||
                    From.ToLower() == "hippo5@hippohappenings.com" ||
                    From.ToLower() == "hippo6@hippohappenings.com" ||
                    From.ToLower() == "hippo7@hippohappenings.com" ||
                    From.ToLower() == "hippo8@hippohappenings.com" ||
                    From.ToLower() == "hippo9@hippohappenings.com")
                {
                    string temp = From.ToLower().Replace("@hippohappenings.com", "").Replace("hippo", "");
                    nextEmail = "hippo" + (int.Parse(temp) + 1).ToString() + "@hippohappenings.com";
                }

                if (tryNextEmail)
                {
                    SendText(nextEmail, FromName, To, Body, Subject);
                }
            }
        //}
        oMail = null; // free up resources

    }

    public string stripHTML(string body)
    {
        string toRet = System.Text.RegularExpressions.Regex.Replace(body, @"<(.|\n)*?>", string.Empty);
        toRet = toRet.Replace("&amp;", " ").Replace("nbsp;", " ");
        return toRet;        
    }

    public string BreakUpString(string content, int numLetters)
    {
        int count = content.Length / numLetters;

        string temp = "";
        int end = 0;
        int total = 0;
        bool doIt = true;
        content = content.Replace("&nbsp;", " ").Replace("<br />", " [br] ").Replace("<br/>", 
            " [br] ").Replace("<br>", " [br] ").Replace("</br>", " [br] ").Replace("</ br>", 
            " [br] ").Replace("<BR />", " [br] ").Replace("<BR/>", 
            " [br] ").Replace("<BR>", " [br] ").Replace("</BR>", " [br] ").Replace("</ BR>", " [br] ");
        string restOfString = content;
        
        char[] delim = { ' ', '\r', '\t', '\n' };
        string[] tokens = content.Split(delim, StringSplitOptions.RemoveEmptyEntries);

        string tempStr = "";
        string anothertemp = "";
        string greaterThan = "";
        string lessThan = "";
        bool doElement = false;
        string workOnThis = "";
        string front = "";
        string back = "";
        if (count > 0)
        {
            foreach (string token in tokens)
            {
                workOnThis = token.Trim();
                doElement = false;
                front = "";
                back = "";
                if (workOnThis.Trim() != "")
                {
                    //if (workOnThis[0] == '<')
                    //{
                    //    doElement = true;
                    //    greaterThan = ">";
                    //    lessThan = "<";
                    //}
                    //else if (workOnThis.Length > 3)
                    //{
                    //    if (workOnThis.Substring(0, 3) == "&lt")
                    //    {
                    //        doElement = true;
                    //        greaterThan = "&gt";
                    //        lessThan = "&lt";
                    //    }
                    //}

                    //if (doElement)
                    //{
                    //    anothertemp = workOnThis.Substring(0, workOnThis.IndexOf(greaterThan));
                    //    front = anothertemp;
                    //    anothertemp = workOnThis.Replace(anothertemp, "");
                    //    workOnThis = "yo"+anothertemp.Substring(0, anothertemp.IndexOf(lessThan));
                    //    back = anothertemp.Substring(anothertemp.IndexOf(lessThan), anothertemp.Length - anothertemp.IndexOf(lessThan));
                    //}


                    if (workOnThis.Length > 3)
                    {
                        if (workOnThis.Substring(0, 4).ToLower() == "href")
                        {
                            doElement = true;
                        }
                        else if (workOnThis.Length > 4)
                        {
                            if (workOnThis.Substring(0, 5).ToLower() == "class")
                            {
                                doElement = true;
                            }
                            else if (workOnThis.Substring(0, 5).ToLower() == "style")
                            {
                                doElement = true;
                            }
                            else if (workOnThis.Length > 12)
                            {
                                if (workOnThis.Substring(0, 13).ToLower() == "outline-style")
                                {
                                    doElement = true;
                                }
                                else if (workOnThis.Length > 14)
                                {
                                    if (workOnThis.Substring(0, 15).ToLower() == "text-decoration")
                                    {
                                        doElement = true;
                                    }
                                }
                            }
                        }
                    }
                    
                    if (workOnThis.Length > numLetters && !doElement)
                    {
                        tempStr = workOnThis;
                        temp += front;
                        while (tempStr.Length > numLetters)
                        {
                            temp += " " + tempStr.Substring(0, numLetters);
                            tempStr = tempStr.Substring(numLetters);
                        }
                        temp += " " + tempStr + back;
                    }
                    else
                    {
                        temp += " " + workOnThis;
                    }
                }
            }

            temp = temp.Replace("[br]", "<br/>");
            return temp;
        }
        else
        {
            return content.Replace("[br]", "<br/>");
        }
    }

    //public string BreakUpString(string content, int numLetters)
    //{
    //    int count = content.Length / numLetters;

    //    string temp = "";
    //    int end = 0;
    //    int total = 0;
    //    if (count > 0)
    //    {
    //        for (int i = 0; i < count; i++)
    //        {
    //            end = numLetters;
    //            if (end >= content.Length)
    //                end = content.Length;
    //            if (!content.Substring(i * numLetters, end).Contains(" ") && !content.Substring(i * numLetters, end).Contains("\t")
    //                && !content.Substring(i * numLetters, end).Contains("\n") && !content.Substring(i * numLetters, end).Contains("\r"))
    //            {
    //                temp += content.Substring(i * numLetters, end) + "<br/>";
    //            }
    //            else
    //            {
    //                temp += content.Substring(i * numLetters, end);
    //            }
    //            total = end;
    //        }
    //        temp += content.Substring(numLetters*count,content.Length - numLetters*count);
            
    //        //if there is still tokens that are greater than numLetters, break them up
    //        char[] delim = { ' ' };
    //        string[] tokens = temp.Split(delim);

    //        temp = "";

    //        for (int i = 0; i < tokens.Length; i++)
    //        {
    //            if (tokens[i].Length > numLetters)
    //            {
    //                temp += tokens[i].Substring(0, numLetters) + "<br/>" + 
    //                    tokens[i].Substring(numLetters, tokens[i].Length - numLetters) + " ";
    //            }
    //            else
    //            {
    //                temp += tokens[i] + " ";
    //            }
    //        }
            
    //        return temp;
    //    }
    //    else
    //    {
    //        return content;
    //    }
    //}

    public string GetIP()
    {
        string ipaddress = "";

        //Works as well: string s = HttpContext.Current.Request.UserHostAddress;

        ipaddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        if (ipaddress == "" || ipaddress == null)

            ipaddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
       
        return ipaddress;
    }

    public void IP2Location2()
    {
        string service = "http://api.quova.com/";
        string version = "v1/";
        string method = "ipinfo/";
        string ipAddress = GetIP();
        string apikey = "100.gsb2snckhagxtk68nskq";
        string secret = "x4dF8wSE";
        string sig = MD5GenerateHash(apikey + secret + (Int32)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);
        string fullURL = service + version + method + ipAddress + "?apikey=" + apikey + "&sig=" + sig + "&format=xml";

        try
        {
            // Create the web request
            HttpWebRequest request = WebRequest.Create(fullURL) as HttpWebRequest;
            XmlDocument doc = new XmlDocument();
            // Get response
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            // Get the response stream
            StreamReader reader = new StreamReader(response.GetResponseStream());

            // Write response to the console
            string al = reader.ReadToEnd();
            doc.LoadXml(al);
            StreamWriter sw = new StreamWriter(Server.MapPath("~/Location231232.txt"), false);
            sw.Write(al);
            sw.Close();
        }
        catch (Exception ex)
        {
            StreamWriter sw = new StreamWriter(Server.MapPath("~/Location231232.txt"), false);
            sw.Write(ex.ToString());
            sw.Close();
        }
    }

    private static string MD5GenerateHash(string strInput)
    {
        // Create a new instance of the MD5CryptoServiceProvider object.
        MD5 md5Hasher = MD5.Create();

        // Convert the input string to a byte array and compute the hash.
        byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(strInput));

        // Create a new Stringbuilder to collect the bytes and create a string.
        StringBuilder sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data and format each one as a hexadecimal string.
        for (int nIndex = 0; nIndex < data.Length; ++nIndex)
        {
            sBuilder.Append(data[nIndex].ToString("x2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();
    }

    public string IP2Location()
    {
        string xmlString = "";
        XmlDocument doc = new XmlDocument();
        try
        {
            
            WebClient myclient = new WebClient();

            //Check whether this IP was already mapped before and the user manualy changed the location
            //or if the IP is already associated with a user in the database
            DataView dv = GetDataDV("SELECT UP.CatCity AS City, UP.CatState AS State, UP.CatCountry AS Country, UP.CatZip FROM Users U, UserPreferences UP WHERE U.User_ID=UP.UserID AND U.IPs LIKE '%;" + GetIP() + ";%'");
                
            bool doIt = false;

            if (dv.Count > 0)
            {
                doIt = true;

                DataView dvLoc = GetDataDV("SELECT * FROM ZipCodes WHERE Zip='" + dv[0]["CatZip"].ToString() + "'");

                while (dvLoc.Count == 0)
                {
                    dvLoc = GetDataDV("SELECT * FROM ZipCodes WHERE Zip='" + (int.Parse(dv[0]["CatZip"].ToString()) + 1).ToString() + "'");
                }

                if (dvLoc.Count > 0)
                {
                    Session["LocLat"] = dvLoc[0]["Latitude"].ToString();

                    Session["LocLong"] = dvLoc[0]["Longitude"].ToString();
                }
            }
            else
            {
                dv = GetDataDV("SELECT * FROM IpToLocation WHERE IP='" + GetIP() + "'");
                if (dv.Count > 0)
                {
                    if (dv[0]["Country"].ToString() == "223")
                    {
                        DataView dvLoc = GetDataDV("SELECT * FROM ZipCodes WHERE State='" + dv[0]["State"].ToString() + "'");

                        Session["LocLat"] = dvLoc[0]["Latitude"].ToString();

                        Session["LocLong"] = dvLoc[0]["Longitude"].ToString();
                        
                        doIt = true;
                    }
                }
            }

            if (doIt)
            {
                Session["LocCity"] = dv[0]["City"].ToString();

                Session["LocCountry"] = dv[0]["Country"].ToString();

                Session["LocState"] = dv[0]["State"].ToString();

                return Session["LocCity"] + ", " + Session["LocState"].ToString();
            }
            else
            {

                string str = myclient.DownloadString("http://geomaplookup.net/index.php?kml=true&ip=" +
                    GetIP());
                //System.IO.StreamReader fs = new System.IO.StreamReader("http://geomaplookup.net/index.php?kml=true&ip=" +
                //    dat.GetIP(), System.Text.ASCIIEncoding.ASCII);


                //xmlString = str;
                //StreamWriter sw = new StreamWriter(Server.MapPath("~/Location.txt"), false);
                //sw.Write(str);
                //sw.Close();
                ////doc.Load(fs);

                //FileStream stream = new FileStream(Server.MapPath("~/Location.txt"), FileMode.Open);
                //TextWriter tw = new StreamWriter("http://geomaplookup.net/index.php?kml=true&ip=" + GetIP());

                //File file = File.Open(, FileMode.Open);
                //System.IO.FileInfo f = new FileInfo("");
                //f.sa
                //FileStream strLocal = new FileStream("http://geomaplookup.net/index.php?kml=true&ip=" + GetIP(), 
                //    FileMode.Create);
                //strLocal.sav
                //StreamReader sr = new StreamReader(strLocal.b
                //xmlString = sr.ReadToEnd();
                doc.LoadXml(str);
                //stream.Close();
                string city = "unknown";
                try
                {
                    string location = doc.ChildNodes[1].FirstChild.ChildNodes[2].ChildNodes[3].ChildNodes[0].InnerText.ToString();
                    string temp = location.Substring(location.IndexOf("<strong>Latitude:</strong>")).Replace("<strong>Latitude:</strong>", "");
                    Session["LocLat"] = temp.Substring(0, temp.IndexOf("<"));

                    temp = location.Substring(location.IndexOf("<strong>Longitude:</strong>")).Replace("<strong>Longitude:</strong>", "");
                    Session["LocLong"] = temp.Substring(0, temp.IndexOf("<"));
                    temp = location.Substring(location.IndexOf("<strong>Country:</strong>")).Replace("<strong>Country:</strong>", "");

                    string country = temp.Substring(0, temp.IndexOf("<"));
                    temp = location.Substring(location.IndexOf("<strong>City:</strong>")).Replace("<strong>City:</strong>", "");

                    city = temp.Substring(0, temp.IndexOf("<"));


                    DataView dvCountry = GetDataDV("SELECT * FROM Countries WHERE country_name = '" +
                        country.ToLower().Trim() + "'");
                    Session["LocCountry"] = dvCountry[0]["country_id"].ToString();


                    Session["LocCity"] = city;

                    if (dvCountry[0]["country_id"].ToString() == "223")
                    {
                        DataView Zips = GetDataDV("SELECT Zip, State, Longitude, Latitude, dbo.GetDistance(" +
                            Session["LocLong"].ToString() + ", " + Session["LocLat"].ToString() +
                            ", Longitude, Latitude) AS Expr1 FROM ZipCodes ORDER BY Expr1");
                        xmlString = "SELECT Zip, State, Longitude, Latitude, dbo.GetDistance(" +
                            Session["LocLong"].ToString() + ", " + Session["LocLat"].ToString() +
                            ", Longitude, Latitude) AS Expr1 FROM ZipCodes ORDER BY Expr1";
                        if (Zips.Count > 0)
                            Session["LocState"] = Zips[0]["State"].ToString();
                    }
                    else
                    {
                        Session["LocState"] = "State";
                    }



                    //Session["LocCountry"] = "223";



                    //return city + ", " + Session["LocState"].ToString();
                    return city + ", " + Session["LocState"].ToString();
                }
                catch (Exception ex)
                {
                    //return ex.ToString() + "<br/>xml str: " + xmlString;
                    return city + ", " + Session["LocState"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            Session["LocCity"] = "Portland";
            Session["LocState"] = "OR";
            Session["LocCountry"] = "223";
            DataView dvLatLong = GetDataDV("SELECT * FROM ZipCodes WHERE Zip='97205'");
            Session["LocLat"] = dvLatLong[0]["Latitude"].ToString();
            Session["LocLong"] = dvLatLong[0]["Longitude"].ToString();
            return "Portland, OR";
        }
    }

    public string GetSeenEvents(string UserID)
    {
        string notTheseAds = "";
        try
        {
            if (Session["User"] != null)
            {
                DataSet dsSeenAds = GetData("SELECT * FROM Events_Seen_By_User WHERE UserID=" +
                    UserID + " AND SessionID='" +
                    Session["UserSession" + Session["User"].ToString()].ToString() + "'");


                if (dsSeenAds.Tables.Count > 0)
                    if (dsSeenAds.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < dsSeenAds.Tables[0].Rows.Count; j++)
                        {
                            if (notTheseAds != "")
                                notTheseAds += " AND ";
                            notTheseAds += " E.ID <> " + dsSeenAds.Tables[0].Rows[j]["eventID"].ToString();


                        }
                    }
                if (notTheseAds != "")
                    notTheseAds = " AND ( " + notTheseAds + " ) ";
            }
        }
        catch (Exception ex)
        {

        }
        return notTheseAds;
    }

    public string MakeSQLSave(string str)
    {
        return str.Replace("'", "''");
    }

    public DataSet GetEventsInLocation(bool showSeen)
    {

        string returnData = "";

        string country = "";
        string state = "";
        string city = "";

        if (Session["UserCountry"] != null)
            country = " AND E.Country = " + Session["UserCountry"].ToString();
        if (Session["UserState"] != null)
            if (Session["UserState"].ToString() != "")
                state = " AND E.State = '" + MakeSQLSave(Session["UserState"].ToString()) + "' ";
        if (Session["UserCity"] != null)
            if (Session["UserCity"].ToString() != "")
                city = " AND E.City = '" + MakeSQLSave(Session["UserCity"].ToString()) + "' ";

        //Get Events seen by user
        string notTheseAds = GetSeenEvents(Session["User"].ToString());

        DataSet ds = new DataSet();

        //Get Events in state and city
        bool couldGetMore = false;
        bool getUS = false;
        if (state != "")
        {
            if (city != "")
            {
                returnData = "SELECT DISTINCT E.ID, E.Header, E.Content, " +
                    "E.SponsorPresenter FROM Events E, Event_Occurance EO " +
                    "WHERE E.LIVE='True' AND E.ID=EO.EventID " + notTheseAds +
                    country + state + city + timeFrame;
                ds = GetData(returnData);
            }
            else
            {
                returnData = "SELECT  DISTINCT E.ID, E.Header, E.Content, " +
                    "E.SponsorPresenter FROM Events E, Event_Occurance EO WHERE " +
                    " E.LIVE='True' AND E.ID=EO.EventID " + notTheseAds + country + state + timeFrame;
                ds = GetData(returnData);
            }
        }
        else
        {
            returnData = "SELECT  DISTINCT E.ID, E.Header, E.Content, E.SponsorPresenter " +
                "FROM Events E, Event_Occurance EO WHERE  E.LIVE='True' AND  E.ID=EO.EventID " +
                notTheseAds + country + timeFrame;
            ds = GetData(returnData);
        }

        if (ds.Tables.Count > 0)
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds;
            }
            else
            {
                couldGetMore = true;
            }
        else
        {
            couldGetMore = true;
        }

        if (showSeen)
        {
            //if no events re-show the seen events

            //delete user's seen events from db and re-show them
            Random rand = new Random(timeNow.ToUniversalTime().Millisecond);
            Execute("DELETE FROM Events_Seen_By_User WHERE SessionID='" + Session["UserSession" +
                Session["User"].ToString()].ToString() + "' AND userID=" + Session["User"].ToString());
            Session["UserSession" + Session["User"].ToString()] = rand.Next();

            if(notTheseAds.Trim() != "")
                returnData = returnData.Replace(notTheseAds, " ");
            //if (state != "")
            //{
            //    if (city != "")
            //    {
            //        returnData = "SELECT  DISTINCT EO.EventID, E.Header, E.Content, E.SponsorPresenter  " +
            //            "FROM Events E, Event_Occurance EO WHERE " +
            //        "  E.ID=EO.EventID " + country + state + city + timeFrame;
            //        ds = GetData(returnData);
            //    }
            //    else
            //    {
            //        returnData = "SELECT  DISTINCT EO.EventID, E.Header, E.Content, " +
            //        "E.SponsorPresenter FROM Events E, Event_Occurance EO WHERE " +
            //        "  E.ID=EO.EventID " + country + state + timeFrame;
            //        ds = GetData(returnData);
            //    }
            //}
            //else
            //{
            //    returnData = "SELECT  DISTINCT EO.EventID, E.Header, E.Content, E.SponsorPresenter " +
            //        "FROM Events E, Event_Occurance EO WHERE " +
            //        " E.ID=EO.EventID " + country + timeFrame;
            //    ds = GetData("SELECT  DISTINCT EO.EventID, E.Header, E.Content, E.SponsorPresenter " +
            //        "FROM Events E, Event_Occurance EO WHERE " +
            //        " E.ID=EO.EventID " + country + timeFrame);

            //}

            ds = GetData(returnData);
            if (ds.Tables.Count > 0)
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds;
                }
                else
                    getUS = true;
            else
                getUS = true;


            if (getUS)
            {
                return GetData("SELECT DISTINCT E.ID, E.Header, E.Content, E.SponsorPresenter " +
                    "FROM Events E, Event_Occurance EO WHERE  E.LIVE='True' AND EO.EventID=E.ID AND  E.Country=223 " + timeFrame);
            }
        }


        return new DataSet();
    }

    public string GetString(string command, string stringToGet)
    {
        string returnStr = "";
        DataSet ds = GetData(command);
        if (ds.Tables.Count > 0)
            if (ds.Tables[0].Rows.Count > 0)
                if(ds.Tables[0].Rows[0][stringToGet].ToString().Trim() == "")
                    return null;
                else
                    return ds.Tables[0].Rows[0][stringToGet].ToString();
            else
                return null;
        else
            return null;
    }

    public DataSet GetEventsOnVenues()
    {
        DateTime date = DateTime.Today;
        string command = "SELECT DISTINCT TOP 20 E.Header, EO.DateTimeStart, E.Content, " +
            "E.ID AS EventID, V.Name As VenueName, V.ID AS VID FROM UserVenues UV, " +
            "Events E, Event_Occurance EO, Venues V WHERE  E.LIVE='True' AND UV.UserID=" + Session["User"].ToString() +
            " AND UV.VenueID=V.ID AND E.Venue=UV.VenueID AND " +
            "E.ID=EO.EventID " + timeFrame + " ORDER BY EO.DateTimeStart ASC ";
        DataSet dsVenues = GetData(command);
        return dsVenues;
    }

    public DataSet GetEventsOnCategoryAndLocation()
    {
        DateTime date = DateTime.Today;
        DataSet dsPrefs = GetData("SELECT * FROM UserPreferences WHERE UserID=" + Session["User"].ToString());

        string recomPrefs = dsPrefs.Tables[0].Rows[0]["RecommendationPrefs"].ToString();

        string compareState = "";
        string compareCity = "";
        string compareCountry = " AND E.Country=" + dsPrefs.Tables[0].Rows[0]["CatCountry"].ToString();
        SqlDbType[] types;
        object[] data;

        string zips = "";

        if (dsPrefs.Tables[0].Rows[0]["CatZip"].ToString() != null)
        {
            if (dsPrefs.Tables[0].Rows[0]["CatZip"].ToString().Trim() != "")
            {
                char[] delim = { ';' };
                string[] tokens = dsPrefs.Tables[0].Rows[0]["CatZip"].ToString().Split(delim);
                zips = "";

                for (int i = 0; i < tokens.Length; i++)
                {
                    if (tokens[i].Trim() != "")
                    {
                        if (zips.Trim() != "")
                            zips += " OR ";
                        zips += " E.Zip = '" + tokens[i].Trim() + "' ";
                    }
                }
            }
        }

        if (zips.Trim() != "")
            zips = " AND ( (" + zips + ") OR (E.Zip is NULL OR E.Zip = '')) ";



        if (dsPrefs.Tables[0].Rows[0]["CatState"].ToString() != null)
        {
            compareState = " AND E.State = @p0 ";

            if (dsPrefs.Tables[0].Rows[0]["CatCity"].ToString() != null)
            {
                compareCity = " AND E.City = @p1 ";
                types = new SqlDbType[2];
                data = new object[2];
                types[0] = SqlDbType.NVarChar;
                types[1] = SqlDbType.NVarChar;
                data[0] = dsPrefs.Tables[0].Rows[0]["CatState"].ToString();
                data[1] = dsPrefs.Tables[0].Rows[0]["CatCity"].ToString();
            }
            else
            {
                types = new SqlDbType[1];
                data = new object[1];
                types[0] = SqlDbType.NVarChar;
                data[0] = dsPrefs.Tables[0].Rows[0]["CatState"].ToString();
            }
        }
        else
        {
            types = new SqlDbType[0];
            data = new object[0];
        }


        string command = "SELECT DISTINCT E.Header, EO.DateTimeStart, E.Content, E.ID AS EventID, V.Name As VenueName, V.ID AS VID  " +
                "FROM            EventCategories AS C INNER JOIN " +
                       "  UserEventCategories AS UC ON C.ID = UC.CategoryID INNER JOIN " +
                        " Event_Occurance AS EO INNER JOIN " +
                        " Events AS E ON EO.EventID = E.ID INNER JOIN " +
                        " Event_Category_Mapping AS ECM ON E.ID = ECM.EventID ON UC.CategoryID = ECM.CategoryID, Venues V WHERE  E.LIVE='True' AND E.Venue=V.ID AND " +
                "   (UC.UserID = " + Session["User"].ToString() + ") " +zips + compareCountry + compareState + compareCity+timeFrame;
        return GetDataWithParemeters(command, types, data);
    }

    public DataSet GetEventsOnCategory()
    {
        DateTime date = DateTime.Today;
        
        string command = "SELECT DISTINCT E.Header, EO.DateTimeStart, E.Content, E.ID AS EventID, "+
            "V.Name As VenueName, V.ID AS VID  " +
                "FROM            Categories AS C INNER JOIN " +
                       "  UserEventCategories AS UC ON C.ID = UC.CategoryID INNER JOIN " +
                        " Event_Occurance AS EO INNER JOIN " +
                        " Events AS E ON EO.EventID = E.ID INNER JOIN " +
                        " Event_Category_Mapping AS ECM ON E.ID = ECM.EventID ON UC.CategoryID "+
                        "= ECM.CategoryID, Venues V WHERE  E.LIVE='True' AND E.Venue=V.ID AND " +
                "   (UC.UserID = " + Session["User"].ToString() + ") " + timeFrame;
        return GetData(command);
    }

    public DataSet GetSimilarEvents()
    {
        //Get all events in user's calendar
        //Get all people who went to these events, excluding this user
        //Get all events in these people's calendars including duplicates
        //Traverse through this list and put the eventIDs in a hash, each 
        //time hit an event that's in the hash increment the hash value
        //Final Similar event array is of the events that count 5 or hire 
        //in the has values
        //Return the string concatenated by 'And E.ID=' of all these eventIDs 
        //that are similar

        DataSet dsUser = GetData("SELECT DISTINCT EventID FROM User_Calendar WHERE UserID=" + 
            Session["User"].ToString());

        DataSet ds = new DataSet();       

        DataColumn col = new DataColumn();
        col.ColumnName = "EventID";

        DataColumn col2 = new DataColumn();
        col2.ColumnName = "Header";

        DataColumn col3 = new DataColumn();
        col3.ColumnName = "VID";

        DataColumn col4 = new DataColumn();
        col4.ColumnName = "VenueName";

        DataColumn col5 = new DataColumn();
        col5.ColumnName = "Content";

        ds.Tables.Add("table");
        ds.Tables["table"].Columns.Add(col);
        ds.Tables["table"].Columns.Add(col2);
        ds.Tables["table"].Columns.Add(col3);
        ds.Tables["table"].Columns.Add(col4);
        ds.Tables["table"].Columns.Add(col5);

        Hashtable hash = new Hashtable();

        DataSet dsTemp;
        DataSet dsTemp2;
        string users = "";

        if (dsUser.Tables.Count > 0)
        {
            if (dsUser.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsUser.Tables[0].Rows.Count; i++)
                {
                    //select all users who went to this event
                    dsTemp = GetData("SELECT DISTINCT UserID FROM User_Calendar WHERE EventID=" +
                        dsUser.Tables[0].Rows[i]["EventID"].ToString() + " AND UserID <> " + Session["User"].ToString());

                    if (dsTemp.Tables.Count > 0)
                        if (dsTemp.Tables[0].Rows.Count > 0)
                        {
                            for (int j = 0; j < dsTemp.Tables[0].Rows.Count; j++)
                            {
                                //select all events of each of these users
                                dsTemp2 = GetData("SELECT UC.EventID FROM User_Calendar UC, Event_Occurance EO "+
                                    " WHERE EO.EventID=UC.EventID AND UC.EventID <> " + dsUser.Tables[0].Rows[i]["EventID"].ToString() + " AND UC.UserID=" + dsTemp.Tables[0].Rows[j]["UserID"].ToString() + timeFrame);

                                if(dsTemp2.Tables.Count > 0)
                                    if (dsTemp2.Tables[0].Rows.Count > 0)
                                    {
                                        for (int n = 0; n < dsTemp2.Tables[0].Rows.Count; n++)
                                        {
                                            
                                                if (!hash.Contains(dsTemp2.Tables[0].Rows[n]["EventID"].ToString()))
                                                    hash.Add(dsTemp2.Tables[0].Rows[n]["EventID"].ToString(), "1");
                                                else
                                                {
                                                    hash[dsTemp2.Tables[0].Rows[n]["EventID"].ToString()] =
                                                        int.Parse(hash[dsTemp2.Tables[0].Rows[n]["EventID"].ToString()].ToString()) + 1;
                                                }
                                            
                                        }
                                    }
                            }
                        }
                }

                
            }
        }

        //Get the info for the events

        DataRow row;
        foreach (string key in hash.Keys)
        {
            //select only similar events, i.e. if people who went to this event numbered greater than 4
            if (int.Parse(hash[key].ToString()) > 4)
            {
                dsTemp = GetData("SELECT * FROM Events E, Venues V WHERE  E.LIVE='True' AND E.ID=" + key + " AND E.Venue=V.ID");

                if (dsTemp.Tables[0].Rows.Count > 0)
                {

                    row = ds.Tables[0].NewRow();
                    row["EventID"] = key;
                    row["Header"] = dsTemp.Tables[0].Rows[0]["Header"].ToString();
                    row["VID"] = dsTemp.Tables[0].Rows[0]["Venue"].ToString();
                    row["VenueName"] = dsTemp.Tables[0].Rows[0]["Name"].ToString();
                    row["Content"] = dsTemp.Tables[0].Rows[0]["Content"].ToString();
                    ds.Tables[0].Rows.Add(row);
                }
            }
        }

        return ds;
    }

    public void GetRecommendationIcons(string eventID, ref Panel panel)
    {
        DataSet dsAll = RetrieveRecommendedEvents(100, true);

        DataView dv = new DataView(dsAll.Tables[0], "EventID=" + eventID, "", DataViewRowState.CurrentRows);

        if (dv.Count > 0)
        {
            Literal litTop = new Literal();
            litTop.Text = "<div style=\"display: block;height: 20px;\">";
            Panel toadd = new Panel();
            Label catLabel = new Label();
            Image image = new Image();
            Literal imageCatLiteral = new Literal();
            Image imageS = new Image();
            if (bool.Parse(dv[0]["isVenue"].ToString()))
            {
                //insert venue icon
                image.ImageUrl = "~/image/VenueIcon.png";
                image.AlternateText = "Favorite Venue Recommendation";
                image.ToolTip = "Favorite Venue Recommendation";
                image.Style.Add("float", "left");
                image.Style.Add("margin-right", "5px");
                image.Width = 25;
                image.Height = 25;
            }
            else
            {
                image.Visible = false;
            }
            if (dv[0]["isCategory"].ToString().ToLower() != "false")
            {
                //insert venue icon
                imageCatLiteral.Text = "<div class=\"Text12\" title=\"Number of Favorite Categories Recommendation\" " +
                    "style=\"font-size: 12px;cursor: default;float: left;background-repeat: no-repeat;width: 25px; " +
                    "height: 25px;background-image: url(image/CategoryIcon.png);padding-left: 9px;padding-top: 3px;" +
                    "margin-left: 3px; margin-right: 3px; font-weight: bold;\" >" +
                    dv[0]["isCategory"].ToString().ToLower() + "</div>";

            }
            else
            {
                imageCatLiteral.Visible = false;
            }
            if (bool.Parse(dv[0]["isSimilar"].ToString()))
            {
                //insert venue icon
                imageS.ImageUrl = "~/image/SimilarIcon.png";
                imageS.AlternateText = "Similar Event Recommendation";
                imageS.ToolTip = "Similar Event Recommendation";
                imageS.Style.Add("float", "left");
                imageS.Style.Add("margin-right", "5px");
                imageS.Width = 20;
                imageS.Height = 20;
            }
            else
            {
                imageS.Visible = false;
            }

            Literal litBot = new Literal();
            litBot.Text = "</div>";

            panel.Controls.Add(litTop);
            panel.Controls.Add(image);
            panel.Controls.Add(imageCatLiteral);
            panel.Controls.Add(imageS);
            panel.Controls.Add(catLabel);
            panel.Controls.Add(litBot);
        }
    }

    public void InsertOneEvent(DataSet dsAll, int i, ref ArrayList a, bool isAll)
    {

        //Insert the icons
        if (dsAll.Tables[0].Rows.Count > i)
        {
            DataSet dsUCal = GetData("SELECT * FROM User_Calendar WHERE EventID=" +
        dsAll.Tables[0].Rows[i]["EventID"].ToString() + " AND UserID=" + Session["User"].ToString());

            //only add if not in user's calendar
            if (dsUCal.Tables[0].Rows.Count == 0)
            {
                Literal litTop = new Literal();
                litTop.Text = "<div style=\"display: block;padding-bottom: 5px;padding-top: 10px;height: 20px;\">";
                Panel toadd = new Panel();
                Label catLabel = new Label();
                Image image = new Image();
                Literal imageCatLiteral = new Literal();
                Image imageS = new Image();
                if (bool.Parse(dsAll.Tables[0].Rows[i]["isVenue"].ToString()))
                {
                    //insert venue icon
                    image.ImageUrl = "~/image/VenueIcon.png";
                    image.AlternateText = "Favorite Venue Recommendation";
                    image.ToolTip = "Favorite Venue Recommendation";
                    image.Style.Add("float", "left");
                    image.Style.Add("margin-right", "5px");
                    image.Width = 25;
                    image.Height = 25;
                }
                else
                {
                    image.Visible = false;
                }
                if (dsAll.Tables[0].Rows[i]["isCategory"].ToString().ToLower() != "false")
                {
                    //insert venue icon
                    imageCatLiteral.Text = "<div title=\"Number of Favorite Categories Recommendation\" style=\"font-size: 12px;cursor: default;float: left;background-repeat: no-repeat;width: 25px; height: 25px;background-image: url(image/CategoryIcon.png);padding-left: 9px;padding-top: 2px;margin-left: 3px; margin-right: 3px; font-family: Arial; font-weight: bold;\" >" + dsAll.Tables[0].Rows[i]["isCategory"].ToString().ToLower() + "</div>";

                }
                else
                {
                    imageCatLiteral.Visible = false;
                }
                if (bool.Parse(dsAll.Tables[0].Rows[i]["isSimilar"].ToString()))
                {
                    //insert venue icon
                    imageS.ImageUrl = "~/image/SimilarIcon.png";
                    imageS.AlternateText = "Similar Event Recommendation";
                    imageS.ToolTip = "Similar Event Recommendation";
                    imageS.Style.Add("float", "left");
                    imageS.Style.Add("margin-right", "5px");
                    imageS.Width = 20;
                    imageS.Height = 20;
                }
                else
                {
                    imageS.Visible = false;
                }

                //Add the rest of the info
                Literal label1 = new Literal();
                if (isAll)
                {
                    label1.Text = "<div style=\"float: left; width: 187px; vertical-align: center;\"><a style=\"float: left\" class=\"AddLink\" onclick=\"Search('" +MakeNiceName(dsAll.Tables[0].Rows[i]["Header"].ToString())+"_"+
                        dsAll.Tables[0].Rows[i]["EventID"].ToString() + "_Event');\">" +
                        dsAll.Tables[0].Rows[i]["Header"].ToString() + "</a></div></div>";
                }
                else
                {
                    label1.Text = "<div style=\"float: left; width: 227px; vertical-align: center;\"><a style=\"float: left\" class=\"AddLink\" href=\"" + MakeNiceName(dsAll.Tables[0].Rows[i]["Header"].ToString()) + "_" +
                        dsAll.Tables[0].Rows[i]["EventID"].ToString() + "_Event\">" +
                        dsAll.Tables[0].Rows[i]["Header"].ToString() + "</a></div></div>";
                }
                Label label3 = new Label();
                label3.Style.Add("float", "left");
                if (isAll)
                {
                    label3.Text = "<div style=\"padding-left: 5px; float: left; clear: both;\">at <b><a class=\"AddGreenLink\" onclick=\"Search('" +
                        MakeNiceName(dsAll.Tables[0].Rows[i]["VenueName"].ToString())+"_" +
                        dsAll.Tables[0].Rows[i]["VID"].ToString() + "_Venue');\">" +
                        dsAll.Tables[0].Rows[i]["VenueName"].ToString() + "</a></b></div><br/>";
                }
                else
                {
                    label3.Text = "<div style=\"padding-left: 5px; float: left;\">at <b><a "+
                        "class=\"AddGreenLink\" href=\"" +
                        MakeNiceName(dsAll.Tables[0].Rows[i]["VenueName"].ToString())+"_" +
                        dsAll.Tables[0].Rows[i]["VID"].ToString() + "_Venue\">" +
                        dsAll.Tables[0].Rows[i]["VenueName"].ToString() + "</a></b></div><br/>";
                }
                label3.CssClass = "EventBody";

                Label label2 = new Label();
                label2.CssClass = "EventBody";

                int lenghtNum = 250;
                if (isAll)
                    lenghtNum = 150;

                if (dsAll.Tables[0].Rows[i]["Content"].ToString().Length > lenghtNum)
                    label2.Text = "<div style=\"display: block;clear: both; padding-left: 5px;\">" + 
                         RemoveBreaks(dsAll.Tables[0].Rows[i]["Content"].ToString().Substring(0, lenghtNum)) + "...</div>";
                else
                    label2.Text = "<div style=\"display: block;clear: both; padding-left: 5px;\">" + 
                        RemoveBreaks(dsAll.Tables[0].Rows[i]["Content"].ToString()) + "</div>";
                label2.Text += "<br/>";
                //RecommendedEvents.Controls.Add(label1);
                //RecommendedEvents.Controls.Add(label3);
                //RecommendedEvents.Controls.Add(label2);

                Literal litBegin = new Literal();
                string width = "260";
                if (isAll)
                    width = "220";
                string height = "";
                if (isAll)
                    height = "height: 200px;overflow:hidden;";
                litBegin.Text = "<div style=\"float: left; padding-right: 5px; ; width: " + width + "px;" + height + "\">";

                toadd.Controls.Add(litBegin);
                toadd.Controls.Add(litTop);
                toadd.Controls.Add(image);
                toadd.Controls.Add(imageCatLiteral);
                toadd.Controls.Add(imageS);
                toadd.Controls.Add(catLabel);
                toadd.Controls.Add(label1);
                toadd.Controls.Add(label3);
                toadd.Controls.Add(label2);

                Literal litEnd = new Literal();
                litEnd.Text = "</div>";

                toadd.Controls.Add(litEnd);

                a.Add(toadd);
            }
        }
    }

    public string RemoveBreaks(string replaceStr)
    {
        return replaceStr.Replace("<br>", "").Replace("<br/>",
            "").Replace("<BR/>", "").Replace("<BR>", "").Replace("<BR/>", "").Replace("<br />", "").Replace("<BR />", ""); ;
    }

    //public void InsertOneEvent(DataSet dsAll, int i, ref ArrayList a)
    //{
        
    //        //Insert the icons
    //    if (dsAll.Tables[0].Rows.Count > i)
    //    {
    //        DataSet dsUCal = GetData("SELECT * FROM User_Calendar WHERE EventID=" +
    //    dsAll.Tables[0].Rows[i]["EventID"].ToString() + " AND UserID=" + Session["User"].ToString());

    //        //only add if not in user's calendar
    //        if (dsUCal.Tables[0].Rows.Count == 0)
    //        {
    //            Literal litTop = new Literal();
    //            litTop.Text = "<div style=\"display: block;padding-bottom: 5px;\">";
    //            Panel toadd = new Panel();
    //            Label catLabel = new Label();
    //            Image image = new Image();
    //            Literal imageCatLiteral = new Literal();
    //            Image imageS = new Image();
    //            if (bool.Parse(dsAll.Tables[0].Rows[i]["isVenue"].ToString()))
    //            {
    //                //insert venue icon
    //                image.ImageUrl = "~/image/VenueIcon.png";
    //                image.AlternateText = "Favorite Venue Recommendation";
    //                image.ToolTip = "Favorite Venue Recommendation";
    //                image.Style.Add("float", "left");
    //                image.Style.Add("margin-right", "5px");
    //                image.Width = 20;
    //                image.Height = 20;
    //            }
    //            if (dsAll.Tables[0].Rows[i]["isCategory"].ToString().ToLower() != "false")
    //            {
    //                //insert venue icon
    //                imageCatLiteral.Text = "<div style=\"float: left;background-repeat: no-repeat;width: 20px; height: 20px;background-image: url(image/CategoryIcon.png);padding-left: 6px;margin-left: 3px; margin-right: 3px;\" class=\"AddWhiteLink\">" + dsAll.Tables[0].Rows[i]["isCategory"].ToString().ToLower() + "</div>";

    //            }
    //            if (bool.Parse(dsAll.Tables[0].Rows[i]["isSimilar"].ToString()))
    //            {
    //                //insert venue icon
    //                imageS.ImageUrl = "~/image/SimilarIcon.png";
    //                imageS.AlternateText = "Similar Event Recommendation";
    //                imageS.ToolTip = "Similar Event Recommendation";
    //                imageS.Style.Add("float", "left");
    //                imageS.Style.Add("margin-right", "5px");
    //                imageS.Width = 20;
    //                imageS.Height = 20;
    //            }


    //            //Add the rest of the info
    //            Label label1 = new Label();
    //            label1.Text = "<a class=\"AddLink\"  href=\"Event.aspx?EventID=" +
    //                dsAll.Tables[0].Rows[i]["EventID"].ToString() + "\">" +
    //                dsAll.Tables[0].Rows[i]["Header"].ToString() + "</a><br/>";

    //            Label label3 = new Label();
    //            label3.Text = "<span style=\"padding-left: 5px;\">at <b><a class=\"EventBody\" href=\"Venue.aspx?ID=" +
    //                dsAll.Tables[0].Rows[i]["VID"].ToString() + "\">" +
    //                dsAll.Tables[0].Rows[i]["VenueName"].ToString() + "</a></b></span><br/>";
    //            label3.CssClass = "EventBody";

    //            Label label2 = new Label();
    //            label2.CssClass = "EventBody";
    //            if (dsAll.Tables[0].Rows[i]["Content"].ToString().Length > 250)
    //                label2.Text = "<div style=\"display: block; padding-left: 5px;\">" + dsAll.Tables[0].Rows[i]["Content"].ToString().Substring(0, 250) + "...";
    //            else
    //                label2.Text = dsAll.Tables[0].Rows[i]["Content"].ToString();
    //            label2.Text += "</div><br/>";
    //            //RecommendedEvents.Controls.Add(label1);
    //            //RecommendedEvents.Controls.Add(label3);
    //            //RecommendedEvents.Controls.Add(label2);


    //            Literal litBegin = new Literal();
    //            litBegin.Text = "<div style=\"float: left; padding-right: 5px; width: 260px;\">";

    //            toadd.Controls.Add(litBegin);
    //            toadd.Controls.Add(litTop);
    //            toadd.Controls.Add(image);
    //            toadd.Controls.Add(imageCatLiteral);
    //            toadd.Controls.Add(imageS);
    //            toadd.Controls.Add(catLabel);
    //            toadd.Controls.Add(label1);
    //            toadd.Controls.Add(label3);
    //            toadd.Controls.Add(label2);

    //            Literal litEnd = new Literal();
    //            litEnd.Text = "</div>";

    //            toadd.Controls.Add(litEnd);

    //            a.Add(toadd);
    //        }
    //    }
    //}

    public DataSet MergeSets(DataSet dsVenues, DataSet dsCategory, DataSet dsSimilar,
        bool doVenues, bool doCategories, bool doSimilar, int topCount, bool all)
    {
        Hashtable hash = new Hashtable();

        Hashtable venueHash = new Hashtable();
        Hashtable catHash = new Hashtable();
        Hashtable simHash = new Hashtable();

        int temp = 0;

        int countInDS = 0;


        //Create the new DS
        DataSet ds = new DataSet();

        DataColumn col = new DataColumn();
        col.ColumnName = "EventID";

        DataColumn col2 = new DataColumn();
        col2.ColumnName = "Header";

        DataColumn col3 = new DataColumn();
        col3.ColumnName = "VID";

        DataColumn col4 = new DataColumn();
        col4.ColumnName = "VenueName";

        DataColumn col5 = new DataColumn();
        col5.ColumnName = "Content";

        DataColumn col6 = new DataColumn();
        col6.ColumnName = "isVenue";

        DataColumn col7 = new DataColumn();
        col7.ColumnName = "isCategory";

        DataColumn col8 = new DataColumn();
        col8.ColumnName = "isSimilar";

        ds.Tables.Add("table");
        ds.Tables["table"].Columns.Add(col);
        ds.Tables["table"].Columns.Add(col2);
        ds.Tables["table"].Columns.Add(col3);
        ds.Tables["table"].Columns.Add(col4);
        ds.Tables["table"].Columns.Add(col5);
        ds.Tables["table"].Columns.Add(col6);
        ds.Tables["table"].Columns.Add(col7);
        ds.Tables["table"].Columns.Add(col8);

        DataRow row;

        int i = 0;

        if (doCategories)
        {
            for (i = 0; i < dsCategory.Tables[0].Rows.Count; i++)
            {
                if (all || (countInDS < topCount && !all))
                {
                    if (!hash.ContainsKey(dsCategory.Tables[0].Rows[i]["EventID"].ToString()))
                    {
                        countInDS++;
                        hash.Add(dsCategory.Tables[0].Rows[i]["EventID"].ToString(), "1");

                        row = ds.Tables["table"].NewRow();
                        row["EventID"] = dsCategory.Tables[0].Rows[i]["EventID"].ToString();
                        row["Header"] = dsCategory.Tables[0].Rows[i]["Header"].ToString();
                        row["VID"] = dsCategory.Tables[0].Rows[i]["VID"].ToString();
                        row["VenueName"] = dsCategory.Tables[0].Rows[i]["VenueName"].ToString();
                        row["Content"] = dsCategory.Tables[0].Rows[i]["Content"].ToString();

                        ds.Tables["table"].Rows.Add(row);
                    }
                }

                if (!catHash.ContainsKey(dsCategory.Tables[0].Rows[i]["EventID"].ToString()))
                {
                    catHash.Add(dsCategory.Tables[0].Rows[i]["EventID"].ToString(), "1");
                }
            }
        }

        if (all || (countInDS < topCount && !all))
        {
            if (doVenues)
            {
                for (i = 0; i < dsVenues.Tables[0].Rows.Count; i++)
                {
                    if (all || (countInDS < topCount && !all))
                    {
                        if (!hash.ContainsKey(dsVenues.Tables[0].Rows[i]["EventID"].ToString()))
                        {
                            countInDS++;
                            hash.Add(dsVenues.Tables[0].Rows[i]["EventID"].ToString(), "1");

                            row = ds.Tables["table"].NewRow();
                            row["EventID"] = dsVenues.Tables[0].Rows[i]["EventID"].ToString();
                            row["Header"] = dsVenues.Tables[0].Rows[i]["Header"].ToString();
                            row["VID"] = dsVenues.Tables[0].Rows[i]["VID"].ToString();
                            row["VenueName"] = dsVenues.Tables[0].Rows[i]["VenueName"].ToString();
                            row["Content"] = dsVenues.Tables[0].Rows[i]["Content"].ToString();

                            ds.Tables["table"].Rows.Add(row);
                        }
                    }

                    if (!venueHash.ContainsKey(dsVenues.Tables[0].Rows[i]["EventID"].ToString()))
                    {
                        venueHash.Add(dsVenues.Tables[0].Rows[i]["EventID"].ToString(), "1");
                    }
                }

                if (all || (countInDS < topCount && !all))
                {
                    if (doSimilar)
                    {
                        for (i = 0; i < dsSimilar.Tables[0].Rows.Count; i++)
                        {
                            if (all || (countInDS < topCount && !all))
                            {
                                if (!hash.ContainsKey(dsSimilar.Tables[0].Rows[i]["EventID"].ToString()))
                                {
                                    countInDS++;
                                    hash.Add(dsSimilar.Tables[0].Rows[i]["EventID"].ToString(), "1");

                                    row = ds.Tables["table"].NewRow();
                                    row["EventID"] = dsSimilar.Tables[0].Rows[i]["EventID"].ToString();
                                    row["Header"] = dsSimilar.Tables[0].Rows[i]["Header"].ToString();
                                    row["VID"] = dsSimilar.Tables[0].Rows[i]["VID"].ToString();
                                    row["VenueName"] = dsSimilar.Tables[0].Rows[i]["VenueName"].ToString();
                                    row["Content"] = dsSimilar.Tables[0].Rows[i]["Content"].ToString();

                                    ds.Tables["table"].Rows.Add(row);
                                }
                            }

                            if (!simHash.ContainsKey(dsSimilar.Tables[0].Rows[i]["EventID"].ToString()))
                            {
                                simHash.Add(dsSimilar.Tables[0].Rows[i]["EventID"].ToString(), "1");
                            }
                        }
                    }
                }
            }
            else
            {
                if (doSimilar)
                {
                    if (all || (countInDS < topCount && !all))
                    {

                        for (i = 0; i < dsSimilar.Tables[0].Rows.Count; i++)
                        {
                            if (all || (countInDS < topCount && !all))
                            {
                                if (!hash.ContainsKey(dsSimilar.Tables[0].Rows[i]["EventID"].ToString()))
                                {
                                    countInDS++;
                                    hash.Add(dsSimilar.Tables[0].Rows[i]["EventID"].ToString(), "1");

                                    row = ds.Tables["table"].NewRow();
                                    row["EventID"] = dsSimilar.Tables[0].Rows[i]["EventID"].ToString();
                                    row["Header"] = dsSimilar.Tables[0].Rows[i]["Header"].ToString();
                                    row["VID"] = dsSimilar.Tables[0].Rows[i]["VID"].ToString();
                                    row["VenueName"] = dsSimilar.Tables[0].Rows[i]["VenueName"].ToString();
                                    row["Content"] = dsSimilar.Tables[0].Rows[i]["Content"].ToString();

                                    ds.Tables["table"].Rows.Add(row);
                                }
                            }

                            if (!simHash.ContainsKey(dsSimilar.Tables[0].Rows[i]["EventID"].ToString()))
                            {
                                simHash.Add(dsSimilar.Tables[0].Rows[i]["EventID"].ToString(), "1");
                            }
                        }
                    }
                }
            }
        }

        if (ds.Tables["table"].Rows.Count > 0)
        {
            for (i = 0; i < ds.Tables["table"].Rows.Count; i++)
            {
                if (venueHash.ContainsKey(ds.Tables["table"].Rows[i]["EventID"].ToString()))
                    ds.Tables["table"].Rows[i]["isVenue"] = "true";
                else
                    ds.Tables["table"].Rows[i]["isVenue"] = "false";

                if (catHash.ContainsKey(ds.Tables["table"].Rows[i]["EventID"].ToString()))
                    ds.Tables["table"].Rows[i]["isCategory"] =
                        catHash[ds.Tables["table"].Rows[i]["EventID"].ToString()].ToString();
                else
                {
                    DataSet dsCats = GetData("SELECT DISTINCT ECM.CategoryID FROM UserEventCategories UEC, Event_Category_Mapping ECM WHERE " +
                        "UEC.UserID=" + Session["User"].ToString() + " AND ECM.CategoryID=UEC.CategoryID AND ECM.EventID=" + ds.Tables["table"].Rows[i]["EventID"].ToString());

                    if (dsCats.Tables[0].Rows.Count > 0)
                    {
                        ds.Tables["table"].Rows[i]["isCategory"] = dsCats.Tables[0].Rows.Count;
                    }
                    else
                    {
                        ds.Tables["table"].Rows[i]["isCategory"] = "false";
                    }

                }

                if (simHash.ContainsKey(ds.Tables["table"].Rows[i]["EventID"].ToString()))
                    ds.Tables["table"].Rows[i]["isSimilar"] = "true";
                else
                    ds.Tables["table"].Rows[i]["isSimilar"] = "false";
            }
        }

        return ds;
    }

    public DataSet RetrieveRecommendedEvents(int topCount, bool all)
    {
        //Retrieve user recommendation preferences

        DataSet dsPrefs = GetData("SELECT * FROM UserPreferences WHERE UserID=" + 
            Session["User"].ToString());

        string recomPrefs = dsPrefs.Tables[0].Rows[0]["RecommendationPrefs"].ToString();

        DataSet dsCategory = GetEventsOnCategoryAndLocation();

        DataSet dsVenues = GetEventsOnVenues();


        DataSet dsSimilar = GetSimilarEvents();

        bool doCategories = false;
        bool doVenues = false;
        bool doSimilar = false;

        if (dsCategory.Tables.Count > 0)
            if (dsCategory.Tables[0].Rows.Count > 0)
            {
                
                if(recomPrefs.Contains("2")){
                    doCategories = true;
                }
            }

        if (dsVenues.Tables.Count > 0)
            if (dsVenues.Tables[0].Rows.Count > 0)
            {
                if (recomPrefs.Contains("1"))
                {
                    doVenues = true;
                }
            }

        if(dsSimilar.Tables.Count > 0)
            if(dsSimilar.Tables[0].Rows.Count > 0)
                if (recomPrefs.Contains("3"))
                {
                    doSimilar = true;
                }

        return MergeSets(dsVenues, dsCategory, dsSimilar, 
            doVenues, doCategories, doSimilar, topCount, all);
        
    }

    public DataSet RetrieveMainAds(string userID)
    {
        return RetrieveAds(userID, true);
    }

    public DataSet RetrieveAds(string userID, bool isBig)
    {
        bool getDS = true;
        bool getDSBig = true;
        int countInFirstTry = 0;
        string userAdSession = "UserAds_" + userID;

        int numAds = numberOfAllAdsInDay;


        if (Session["UserAds_" + userID] != null)
        {
            DataSet theDS = (DataSet)Session["UserAds_" + userID];

                    getDS = false;
           
        }

        if (Session["UserAds_Big_" + userID] != null)
        {
            DataSet theDS = (DataSet)Session["UserAds_Big_" + userID];

            getDSBig = false;
            numAds = numberOfMainAdsInADay;
        }

        string adBig = " A.BigAd = 'False' AND ";
        if (isBig)
        {
            adBig = " A.BigAd = 'True' AND ";
            userAdSession = "UserAds_Big_" + userID;
        }

        if ((getDS && !isBig) || (getDSBig && isBig))
        {
            //Retrieve the ads the users has already seen in the past (excluding today)
            string notTheseAds = "";
            if (Session["UserSeenAdsNotToday"] == null)
            {
                DataSet dsSeenAds = GetData("SELECT * FROM AdStatistics WHERE UserID=" +
                    userID + " AND CONVERT(NVARCHAR, MONTH([DATE])) +'/' + CONVERT(NVARCHAR, DAY([DATE])) +" +
                "'/' + CONVERT(NVARCHAR, YEAR([DATE])) <> '" + timeNow.ToShortDateString().Replace("12:00:00 AM", "").Trim() + "'");
                if (dsSeenAds.Tables.Count > 0)
                    if (dsSeenAds.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < dsSeenAds.Tables[0].Rows.Count; j++)
                        {
                            DateTime date = DateTime.Parse(dsSeenAds.Tables[0].Rows[j]["Date"].ToString());
                            DateTime dateEnd = date.AddDays(1.00);

                            if (timeNow > dateEnd)
                            {
                                if (notTheseAds != "")
                                    notTheseAds += " AND ";
                                notTheseAds += " A.Ad_ID <> " + dsSeenAds.Tables[0].Rows[j]["Ad_ID"].ToString();

                            }
                        }
                    }
                if (notTheseAds != "")
                    notTheseAds = " AND ( " + notTheseAds + " ) ";

                Session["UserSeenAdsNotToday"] = notTheseAds;
            }
            else
            {
                notTheseAds = Session["UserSeenAdsNotToday"].ToString();
            }
            

            //Check if Categories Preferences are 'On' and filter on user's categories
            DataSet dsCatPrefs = GetData("SELECT CategoriesOnOff, CatCountry, CatState, CatCity FROM UserPreferences WHERE UserID=" + userID);
            bool filterOnCat = false;
            DataSet dsAdsInCategories = new DataSet();

            string catCountry = "";
            string catState = "";
            string catCity = "";
            int i = 0;
            SqlDbType one = new SqlDbType();
            object obj1 = new object();
            SqlDbType two = new SqlDbType();
            object obj2 = new object();
            if (dsCatPrefs.Tables[0].Rows[0]["CatCountry"] != null)
                if (dsCatPrefs.Tables[0].Rows[0]["CatCountry"].ToString() != "")
                    catCountry = " AND A.CatCountry = " + dsCatPrefs.Tables[0].Rows[0]["CatCountry"].ToString();

            if (dsCatPrefs.Tables[0].Rows[0]["CatState"] != null)
                if (dsCatPrefs.Tables[0].Rows[0]["CatState"].ToString() != "")
                {
                    catState = " AND A.CatState=@p0 ";
                    i++;
                    one = SqlDbType.NVarChar;
                    obj1 = dsCatPrefs.Tables[0].Rows[0]["CatState"].ToString();
                }

            if (dsCatPrefs.Tables[0].Rows[0]["CatCity"] != null)
                if (dsCatPrefs.Tables[0].Rows[0]["CatCity"].ToString() != "")
                {
                    catCity = " AND A.CatCity=@p1 ";
                    i++;
                    two = SqlDbType.NVarChar;
                    obj2 = dsCatPrefs.Tables[0].Rows[0]["CatCity"].ToString();

                }

            SqlDbType[] types = new SqlDbType[i];
            object[] data = new object[i];
            if (i > 0)
            {
                types[0] = one;
                data[0] = obj1;
            }
            if (i > 1)
            {
                types[1] = two;
                data[1] = obj2;
            }
            bool tryAgain = false;
            DataSet dsAdsInLocation = new DataSet();
            if (bool.Parse(dsCatPrefs.Tables[0].Rows[0]["CategoriesOnOff"].ToString()))
            {
                dsAdsInCategories = GetData("SELECT DISTINCT U.UserName, A.Ad_ID, A.FeaturedSummary, " +
                    "A.Description, A.Header, A.NumCurrentViews, A.FeaturedPicture As FeaturedPicture2, ('../UserFiles/' + U.UserName + '/' + A.FeaturedPicture) As FeaturedPicture FROM UserCategories AS UC CROSS JOIN "+
                        " Ad_Calendar AS AC INNER JOIN " +
                        " Ads AS A ON AC.AdID = A.Ad_ID INNER JOIN " +
                        " Ad_Category_Mapping AS ACM ON A.Ad_ID = ACM.AdID INNER JOIN " +
                        " Users AS U ON A.User_ID = U.User_ID " +
" WHERE    " + adBig + "    (AC.DateTimeStart <= CONVERT(DATETIME,'"+timeNow.ToString()+"')) AND (A.NumCurrentViews < A.NumViews) AND (UC.UserID = "+userID+") AND (UC.CategoryID = ACM.CategoryID) AND  " +
             "            (A.Featured = 'True') AND (U.User_ID <> "+userID+") OR " +
              "           (AC.DateTimeStart <=  CONVERT(DATETIME,'"+timeNow.ToString()+"')) AND (A.NumCurrentViews < A.NumViews) AND (A.Featured = 'True') AND  " +
               "          (" + adBig + " ACM.CategoryID = 198 AND U.User_ID <> " + userID + ") " + notTheseAds + 
" ORDER BY A.NumCurrentViews");

                dsAdsInLocation = GetDataWithParemeters("SELECT DISTINCT U.UserName, A.Ad_ID, A.FeaturedSummary, " +
                    "A.Description, A.Header, A.NumCurrentViews, A.FeaturedPicture As FeaturedPicture2, ('../UserFiles/' + U.UserName +" +
                    "'/' + A.FeaturedPicture) As FeaturedPicture FROM Ads A, Users U, Ad_Calendar AC WHERE   " + adBig + " AC.AdID=A.Ad_ID AND AC.DateTimeStart <=  CONVERT(DATETIME,'"+timeNow.ToString()+"') AND A.NumCurrentViews < A.NumViews AND A.Featured='True' AND U.User_ID != " + userID + " AND U.User_ID = A.User_ID " +
                    catCountry + catCity + catState +notTheseAds + " ORDER BY A.NumCurrentViews ASC", types, data);


                if (dsAdsInLocation.Tables.Count > 0)
                    if (dsAdsInLocation.Tables[0].Rows.Count > 0)
                    {
                        if (dsAdsInCategories.Tables.Count > 0)
                            if (dsAdsInCategories.Tables[0].Rows.Count > 0)
                            {
                                DataView dv = new DataView(dsAdsInCategories.Tables[0], "", "", DataViewRowState.CurrentRows);
                                for (int j = 0; j < dsAdsInLocation.Tables[0].Rows.Count; j++)
                                {
                                    //Check first if the ad already exists in the categories dataset.
                                    dv.RowFilter = "Ad_ID="+dsAdsInLocation.Tables[0].Rows[j]["Ad_ID"].ToString();

                                    if (dv.Count == 0)
                                    {
                                        DataRow row = dsAdsInCategories.Tables[0].NewRow();
                                        row["UserName"] = dsAdsInLocation.Tables[0].Rows[j]["UserName"].ToString();
                                        row["Ad_ID"] = dsAdsInLocation.Tables[0].Rows[j]["Ad_ID"].ToString();
                                        row["FeaturedSummary"] = dsAdsInLocation.Tables[0].Rows[j]["FeaturedSummary"].ToString();
                                        row["Description"] = dsAdsInLocation.Tables[0].Rows[j]["Description"].ToString();
                                        row["Header"] = dsAdsInLocation.Tables[0].Rows[j]["Header"].ToString();
                                        row["FeaturedPicture"] = dsAdsInLocation.Tables[0].Rows[j]["FeaturedPicture"].ToString();
                                        row["FeaturedPicture2"] = dsAdsInLocation.Tables[0].Rows[j]["FeaturedPicture2"].ToString();
                                        dsAdsInCategories.Tables[0].Rows.Add(row);
                                    }
                                }
                            }
                            else
                                dsAdsInCategories = dsAdsInLocation;
                        else
                            dsAdsInCategories = dsAdsInLocation;
                    }

                if (dsAdsInCategories.Tables.Count == 0)
                    tryAgain = true;
                else if (dsAdsInCategories.Tables[0].Rows.Count < numAds)
                {
                    tryAgain = true;
                    countInFirstTry = dsAdsInCategories.Tables[0].Rows.Count;

                    for (int n = 0; n < dsAdsInCategories.Tables[0].Rows.Count; n++)
                    {
                        notTheseAds += " AND A.Ad_ID <> " + dsAdsInCategories.Tables[0].Rows[n]["Ad_ID"].ToString();
                    }
                }
                


            }
            else
            {
                tryAgain = true;
            }

            if (tryAgain)
            {
                DataSet dsTemp;
                int countInTemp = 0;
                dsTemp = GetDataWithParemeters("SELECT U.UserName, A.NumCurrentViews, A.Ad_ID, A.FeaturedSummary, A.Description, A.Header, A.FeaturedPicture As FeaturedPicture2, ('../UserFiles/' + U.UserName + '/' + A.FeaturedPicture) As FeaturedPicture FROM " +
                    " Ads A, Users U, Ad_Calendar AC WHERE   " + adBig + "  AC.AdID=A.Ad_ID AND AC.DateTimeStart <=  CONVERT(DATETIME,'"+timeNow.ToString()+"') AND A.NumCurrentViews < A.NumViews AND  A.Featured='True' AND A.User_ID=U.User_ID  AND U.User_ID != " + userID +
                    catCountry + catCity + catState +notTheseAds + " ORDER BY A.NumCurrentViews ASC", types, data);

                tryAgain = false;
                if (dsTemp.Tables.Count == 0)
                    tryAgain = true;
                else if (dsTemp.Tables[0].Rows.Count  < (numAds - countInFirstTry)){
                    tryAgain = true;
                    countInTemp = dsTemp.Tables[0].Rows.Count;

                    for (int n = 0; n < dsTemp.Tables[0].Rows.Count; n++)
                    {
                        notTheseAds += " AND A.Ad_ID <> " + dsTemp.Tables[0].Rows[n]["Ad_ID"].ToString();
                    }
                }

                if (tryAgain)
                {
                    DataSet dsTemp2;
                    if (i != 0)
                    {
                        i--;
                        SqlDbType[] types2 = new SqlDbType[i];
                        object[] data2 = new object[i];

                        if (i > 0)
                        {
                            types2[0] = SqlDbType.NVarChar;
                            data2[0] = obj1;
                        }
                        else
                        {
                            catState = "";
                        }


                        dsTemp2 = GetDataWithParemeters("SELECT U.UserName, A.NumCurrentViews, A.Ad_ID, A.FeaturedSummary, A.Description, A.Header, A.FeaturedPicture As FeaturedPicture2, ('../UserFiles/' + U.UserName + '/' + A.FeaturedPicture) As FeaturedPicture FROM " +
                        " Ads A, Users U, Ad_Calendar AC WHERE   " + adBig + "   AC.AdID=A.Ad_ID AND AC.DateTimeStart <=  CONVERT(DATETIME,'"+timeNow.ToString()+"') AND A.NumCurrentViews < A.NumViews AND  A.Featured='True' AND A.User_ID=U.User_ID  AND U.User_ID != " + userID +
                        catCountry + catState + notTheseAds + "  ORDER BY A.NumCurrentViews ASC", types2, data2);
                    }
                    else
                    {
                        dsTemp2 = GetData("SELECT U.UserName, A.Ad_ID, A.NumCurrentViews, A.FeaturedSummary, A.Description, A.Header, A.FeaturedPicture As FeaturedPicture2, ('../UserFiles/' + U.UserName + '/' + A.FeaturedPicture) As FeaturedPicture FROM " +
                        " Ads A, Users U, Ad_Calendar AC WHERE  " + adBig + "   AC.AdID=A.Ad_ID AND AC.DateTimeStart <=  CONVERT(DATETIME,'"+timeNow.ToString()+"') AND A.NumCurrentViews < A.NumViews AND  A.Featured='True' AND A.User_ID=U.User_ID  AND U.User_ID != " + userID + notTheseAds + "  ORDER BY A.NumCurrentViews ASC");
                   
                    }

                    int countInTempNTemp2 = countInTemp;
                    tryAgain = false;
                    if (dsTemp2.Tables.Count == 0)
                        tryAgain = true;
                    else if (dsTemp2.Tables[0].Rows.Count < numAds - countInFirstTry - countInTemp)
                    {
                        tryAgain = true;
                        countInTempNTemp2 = countInTemp + dsTemp2.Tables[0].Rows.Count;

                        for (int n = 0; n < dsTemp2.Tables[0].Rows.Count; n++)
                        {
                            notTheseAds += " AND A.Ad_ID <> " + dsTemp2.Tables[0].Rows[n]["Ad_ID"].ToString();
                        }
                    }

                    //Transfer ads from Temp2 to Temp
                    if (countInTemp == 0)
                    {
                        dsTemp = dsTemp2;
                    }
                    else
                    {
                        int count = numAds - countInFirstTry - countInTemp;
                        if (dsTemp2.Tables[0].Rows.Count < count)
                            count = dsTemp2.Tables[0].Rows.Count;
                        for (int n = 0; n < count; n++)
                        {
                            DataRow row = dsTemp.Tables[0].NewRow();
                            row["UserName"] = dsTemp2.Tables[0].Rows[n]["UserName"].ToString();
                            row["Ad_ID"] = dsTemp2.Tables[0].Rows[n]["Ad_ID"].ToString();
                            row["FeaturedSummary"] = dsTemp2.Tables[0].Rows[n]["FeaturedSummary"].ToString();
                            row["Description"] = dsTemp2.Tables[0].Rows[n]["Description"].ToString();
                            row["Header"] = dsTemp2.Tables[0].Rows[n]["Header"].ToString();
                            row["FeaturedPicture"] = dsTemp2.Tables[0].Rows[n]["FeaturedPicture"].ToString();
                            row["FeaturedPicture2"] = dsTemp2.Tables[0].Rows[n]["FeaturedPicture2"].ToString();
                            dsTemp.Tables[0].Rows.Add(row);
                        }
                    }
                    
                    int countInTemp1Temp2Temp3 = countInTempNTemp2;
                    if (tryAgain)
                    {
                        DataSet dsTemp3;
                        dsTemp3 = GetData("SELECT U.UserName, A.Ad_ID, A.NumCurrentViews, A.FeaturedSummary, A.Description, A.Header, A.FeaturedPicture As FeaturedPicture2, ('../UserFiles/' + U.UserName + '/' + A.FeaturedPicture) As FeaturedPicture FROM " +
                        " Ads A, Users U, Ad_Calendar AC WHERE  " + adBig + "   AC.AdID=A.Ad_ID AND AC.DateTimeStart <=  CONVERT(DATETIME,'"+timeNow.ToString()+"') AND A.NumCurrentViews < A.NumViews AND A.Featured='True' AND A.User_ID=U.User_ID  AND U.User_ID != " + userID + catCountry + notTheseAds + "  ORDER BY A.NumCurrentViews ASC");

                        if(dsTemp3.Tables.Count != 0)
                            if (dsTemp3.Tables[0].Rows.Count != 0)
                            {
                                int count2 = numAds - countInFirstTry - countInTempNTemp2;
                                
                                if (dsTemp3.Tables[0].Rows.Count < count2)
                                    count2 = dsTemp3.Tables[0].Rows.Count;
                                countInTemp1Temp2Temp3 = countInTempNTemp2 + count2;
                                for (int n = 0; n < count2; n++)
                                {
                                    DataRow row = dsTemp.Tables[0].NewRow();
                                    row["UserName"] = dsTemp3.Tables[0].Rows[n]["UserName"].ToString();
                                    row["Ad_ID"] = dsTemp3.Tables[0].Rows[n]["Ad_ID"].ToString();
                                    row["FeaturedSummary"] = dsTemp3.Tables[0].Rows[n]["FeaturedSummary"].ToString();
                                    row["Description"] = dsTemp3.Tables[0].Rows[n]["Description"].ToString();
                                    row["Header"] = dsTemp3.Tables[0].Rows[n]["Header"].ToString();
                                    row["FeaturedPicture"] = dsTemp3.Tables[0].Rows[n]["FeaturedPicture"].ToString();
                                    row["FeaturedPicture2"] = dsTemp3.Tables[0].Rows[n]["FeaturedPicture2"].ToString();
                                    dsTemp.Tables[0].Rows.Add(row);

                                   
                                    notTheseAds += " AND A.Ad_ID <> " + dsTemp3.Tables[0].Rows[n]["Ad_ID"].ToString();
                                    
                                }
                            }

                        //tryAgain = false;
                        //if (dsTemp3.Tables.Count == 0)
                        //    tryAgain = true;
                        //else if (dsTemp3.Tables[0].Rows.Count == 0)
                        //    tryAgain = true;

                        //Don't allow ads from another country.
                        //if (tryAgain)
                        //{
                        //    dsTemp3 = GetData("SELECT U.UserName,  A.Ad_ID, A.FeaturedSummary, A.Description, A.Header, A.FeaturedPicture As FeaturedPicture2, ('../UserFiles/' + U.UserName + '/' + A.FeaturedPicture) As FeaturedPicture FROM " +
                        //" Ads A, Users U, Ad_Calendar AC WHERE AC.AdID=A.Ad_ID AND AC.DateTimeStart <=  CONVERT(DATETIME,'"+timeNow.ToString()+"') AND A.NumCurrentViews < A.NumViews AND A.Featured='True' AND A.User_ID=U.User_ID  AND U.User_ID != " + userID + notTheseAds);
                        //}

                    }

                    if (dsAdsInCategories.Tables.Count == 0)
                        dsAdsInCategories = dsTemp;
                    else if (dsAdsInCategories.Tables[0].Rows.Count == 0)
                        dsAdsInCategories = dsTemp;
                    else
                    {
                        for (int j = 0; j < dsTemp.Tables[0].Rows.Count; j++)
                        {
                            DataRow row = dsAdsInCategories.Tables[0].NewRow();
                            row["UserName"] = dsTemp.Tables[0].Rows[j]["UserName"].ToString();
                            row["Ad_ID"] = dsTemp.Tables[0].Rows[j]["Ad_ID"].ToString();
                            row["FeaturedSummary"] = dsTemp.Tables[0].Rows[j]["FeaturedSummary"].ToString();
                            row["Description"] = dsTemp.Tables[0].Rows[j]["Description"].ToString();
                            row["Header"] = dsTemp.Tables[0].Rows[j]["Header"].ToString();
                            row["FeaturedPicture"] = dsTemp.Tables[0].Rows[j]["FeaturedPicture"].ToString();
                            row["FeaturedPicture2"] = dsTemp.Tables[0].Rows[j]["FeaturedPicture2"].ToString();
                            dsAdsInCategories.Tables[0].Rows.Add(row);
                        }
                    }
                }



            }

            if (dsAdsInCategories.Tables[0].Rows.Count < numAds)
            {
                DataSet dsOthers = GetDataWithParemeters("SELECT U.UserName, A.NumCurrentViews, A.Ad_ID, A.FeaturedSummary, " +
                    "A.Description, A.Header, A.FeaturedPicture As FeaturedPicture2, ('../UserFiles/' + U.UserName + " +
                    "'/' + A.FeaturedPicture) As FeaturedPicture FROM Ads A, Users U, Ad_Calendar AC WHERE  " + adBig + "   AC.AdID=A.Ad_ID AND AC.DateTimeStart <=  CONVERT(DATETIME,'"+timeNow.ToString()+"') AND  " +
            "A.NumCurrentViews < A.NumViews AND U.User_ID = A.User_ID AND U.User_ID != " + userID + " AND A.Featured = 'True' AND A.DisplayToAll = 'True'" +
                    catCountry + catCity + catState + notTheseAds + "  ORDER BY A.NumCurrentViews ASC", types, data);
                if (dsOthers.Tables.Count != 0)
                {
                    if (dsOthers.Tables[0].Rows.Count != 0)
                    {
                        DataView dv = new DataView(dsAdsInCategories.Tables[0], "", "", DataViewRowState.CurrentRows);
                        for (int n = 0; n < dsOthers.Tables[0].Rows.Count; n++)
                        {
                            dv.RowFilter = "Ad_ID="+dsOthers.Tables[0].Rows[n]["Ad_ID"].ToString();
                            if (dv.Count == 0)
                            {
                                DataRow row = dsAdsInCategories.Tables[0].NewRow();
                                row["UserName"] = dsOthers.Tables[0].Rows[n]["UserName"].ToString();
                                row["Ad_ID"] = dsOthers.Tables[0].Rows[n]["Ad_ID"].ToString();
                                row["FeaturedSummary"] = dsOthers.Tables[0].Rows[n]["FeaturedSummary"].ToString();
                                row["Description"] = dsOthers.Tables[0].Rows[n]["Description"].ToString();
                                row["Header"] = dsOthers.Tables[0].Rows[n]["Header"].ToString();
                                row["FeaturedPicture"] = dsOthers.Tables[0].Rows[n]["FeaturedPicture"].ToString();
                                row["FeaturedPicture2"] = dsOthers.Tables[0].Rows[n]["FeaturedPicture2"].ToString();
                                dsAdsInCategories.Tables[0].Rows.Add(row);
                            }
                        }
                    }
                }
            }
            //Don't remove the ads here. They will be needed later when user has seen other ads already.
            //else if (dsAdsInCategories.Tables[0].Rows.Count > numAds)
            //{
            //    int numToRemove = dsAdsInCategories.Tables[0].Rows.Count - numAds;

            //    for (int n = 0; n < numToRemove; n++)
            //    {
            //        dsAdsInCategories.Tables[0].Rows.RemoveAt(dsAdsInCategories.Tables[0].Rows.Count - 1);
            //    }

            //}

            //Add the row number into the table for easy counting of the ads
            dsAdsInCategories.Tables[0].Columns.Add("Row");
            for (int c = 0; c < dsAdsInCategories.Tables[0].Rows.Count; c++)
            {

                dsAdsInCategories.Tables[0].Rows[c]["Row"] = c.ToString();
            }
            Session[userAdSession] = dsAdsInCategories;
        }

        DataSet dsReturned = (DataSet)Session[userAdSession];
        if (dsReturned == null)
        {
            dsReturned = GetGlobalAds(isBig, userID);
            dsReturned.Tables[0].Columns.Add("Row");
            for (int c = 0; c < dsReturned.Tables[0].Rows.Count; c++)
            {
                dsReturned.Tables[0].Rows[c]["Row"] = c.ToString();
            }
        }
        else if (dsReturned.Tables.Count == 0)
        {
            dsReturned = GetGlobalAds(isBig, userID);
            dsReturned.Tables[0].Columns.Add("Row");
            for (int c = 0; c < dsReturned.Tables[0].Rows.Count; c++)
            {
                dsReturned.Tables[0].Rows[c]["Row"] = c.ToString();
            }
        }
        else if (dsReturned.Tables[0].Rows.Count == 0)
        {
            dsReturned = GetGlobalAds(isBig, userID);
            dsReturned.Tables[0].Columns.Add("Row");
            for (int c = 0; c < dsReturned.Tables[0].Rows.Count; c++)
            {
                dsReturned.Tables[0].Rows[c]["Row"] = c.ToString();
            }
        }


        return dsReturned;
    }

    public DataSet GetGlobalAds(bool isBig, string userID)
    {
        string adBig = " A.BigAd = 'False' AND ";
        if (isBig)
        {
            adBig = " A.BigAd = 'True' AND ";
        }

        DataSet dsOthers = GetData("SELECT U.UserName, A.NumCurrentViews, A.Ad_ID, A.FeaturedSummary, " +
            "A.Description, A.Header, A.FeaturedPicture As FeaturedPicture2, ('../UserFiles/' + U.UserName + " +
            "'/' + A.FeaturedPicture) As FeaturedPicture FROM Ads A, Users U, Ad_Calendar AC WHERE  " + adBig + 
            "   AC.AdID=A.Ad_ID AND AC.DateTimeStart <=  CONVERT(DATETIME,'"+timeNow.ToString()+"') AND  " +
            "A.NumCurrentViews < A.NumViews AND U.User_ID = A.User_ID AND U.User_ID != " + userID + 
            " AND A.Featured = 'True' AND A.DisplayToAll = 'True' ORDER BY A.NumCurrentViews ASC");

        return dsOthers;

    }

    public DataSet RetrieveAllAds(bool isBig)
    {
        string adBig = " A.BigAd = 'False' AND ";

        string sessionName = "GenericSession";
        string userSessionName = "GenericUser";

        int numAds = numberOfAllAdsInDay;

        if (isBig)
        {
            adBig = " A.BigAd = 'True' AND ";
            sessionName = "GenericSessionBig";
            userSessionName = "GenericUserBig";
            numAds = numberOfMainAdsInADay;
        }

        bool getMore = false;

        if (Session[sessionName] != null && Session[userSessionName] != null)
        {
            DataSet dsName = (DataSet)Session[userSessionName];
            if (dsName.Tables.Count == 0)
                getMore = true;
            else if (dsName.Tables[0].Rows.Count == 0)
                getMore = true; 
        }

       if (Session[sessionName] == null ||  Session[userSessionName] != null || getMore)
        {
            //set a random session variable to identify this session by. it will be used later to transfer the
            //ads seen under the appropriate users if the user logs in.
            Random rand = new Random(timeNow.ToUniversalTime().Millisecond);
            Session[sessionName] = rand.Next().ToString() + timeNow.ToUniversalTime();

            string IP = GetIP();

            string today = timeNow.ToString();
            Execute("INSERT INTO GenericLogins (IP, LoginDate, isUser, UserID, SessionID) VALUES('"+IP+
                "', '"+today+"', 0, NULL, '"+Session[sessionName].ToString()+"')");

            DataSet dsGenericID = GetData("SELECT ID FROM GenericLogins WHERE IP='"+IP+"' AND LoginDate='"+
                today+"' AND SessionID='"+Session[sessionName].ToString()+"'");

            Session["GenericLoginID"] = dsGenericID.Tables[0].Rows[0]["ID"].ToString();
            Session["GenericIP"] = IP;

            //Get the IP address's location preferences
            string city = "";
            string state = "";
            string country = "";
            DataSet dsUsers = GetData("SELECT * FROM Users U, UserPreferences UP WHERE U.User_ID=UP.UserID AND U.IPs LIKE '%"+IP+"%'");
            bool getIP = true;
            if (dsUsers.Tables.Count > 0)
                if (dsUsers.Tables[0].Rows.Count > 0)
                {
                    getIP = false;
                    city = dsUsers.Tables[0].Rows[0]["CatCity"].ToString();
                    state = dsUsers.Tables[0].Rows[0]["CatState"].ToString();
                    country = dsUsers.Tables[0].Rows[0]["CatCountry"].ToString();
                }


            //Get location of IP address if IP not found in Users table
            //if (getIP)
            //{
            //    DataTable location = GetLocation("192.168.0.199");
            //    city = location.Rows[0]["City"].ToString();
            //    state = location.Rows[0]["RegionName"].ToString();
            //    country = location.Rows[0]["CountryCode"].ToString();
                
            //    //get the state ID
            //    DataSet dsState = GetData("SELECT * FROM State WHERE state_name = '" + state + "'");

            //    if (dsState.Tables.Count > 0)
            //        if (dsState.Tables[0].Rows.Count > 0)
            //            state = dsState.Tables[0].Rows[0]["state_2_code"].ToString();
            //        else
            //            state = "OR";
            //    else
            //        state = "OR";

            //    //get the country ID
            //    DataSet dsCountry = GetData("SELECT * FROM Countries WHERE country_2_code = '"+country+"'");

            //    if (dsCountry.Tables.Count > 0)
            //        if (dsCountry.Tables[0].Rows.Count > 0)
            //            country = dsCountry.Tables[0].Rows[0]["country_id"].ToString();
            //        else
            //            country = "223";
            //    else
            //        country = "223";
            //}

            //put the location information into a query
            int i = 0;
            SqlDbType one = new SqlDbType();
            object obj1 = new object();
            SqlDbType two = new SqlDbType();
            object obj2 = new object();
            string catCountry = "";
            string catCity = "";
            string catState = "";
            if (country != "")
                    catCountry = " AND A.CatCountry = " + country;

            if (state != "")
                {
                    catState = " AND A.CatState=@p0 ";
                    i++;
                    one = SqlDbType.NVarChar;
                    obj1 = state;
                }

            if (city != "")
                {
                    catCity = " AND A.CatCity=@p1 ";
                    i++;
                    two = SqlDbType.NVarChar;
                    obj2 = city;

                }

            SqlDbType[] types = new SqlDbType[i];
            object[] data = new object[i];
            if (i > 0)
            {
                types[0] = one;
                data[0] = obj1;
            }
            if (i > 1)
            {
                types[1] = two;
                data[1] = obj2;
            }

            DataSet ds = GetDataWithParemeters("SELECT A.Header, A.Description, A.NumCurrentViews, A.FeaturedSummary, " +
            "A.Ad_ID, U.UserName, A.FeaturedPicture As FeaturedPicture2, " +
            "('../UserFiles/' + U.UserName + '/' + A.FeaturedPicture) As FeaturedPicture " +
            "FROM Ads A, Users U, Ad_Calendar AC WHERE   " + adBig + "  AC.AdID=A.Ad_ID AND A.Featured='True' " +
            "AND A.User_ID=U.User_ID AND AC.DateTimeStart <=  CONVERT(DATETIME,'"+timeNow.ToString()+"') AND  " +
            "A.NumCurrentViews < A.NumViews AND A.DisplayToAll='True' "+catCountry + catState+" ORDER BY A.NumCurrentViews ASC", types, data);

            

            getMore = false;


            if (ds.Tables.Count == 0)
                getMore = true;
            else if (ds.Tables[0].Rows.Count < numAds)
                getMore = true;

            if (getMore)
            {
                DataSet ds1 = GetData("SELECT * FROM SearchIPs WHERE IP='" + GetIP() + "'");

                if(ds1.Tables.Count > 0)
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        country = ds1.Tables[0].Rows[0]["Country"].ToString();
                        state = ds1.Tables[0].Rows[0]["State"].ToString();
                        city = ds1.Tables[0].Rows[0]["City"].ToString();
                    }
                
                string countryID = country;
                
                string stateID = state;
                string cityID = city;

                i = 0;
                if (country != "")
                    catCountry = " AND A.CatCountry = " + country;

                if (state != "")
                {
                    catState = " AND A.CatState=@p0 ";
                    i++;
                    one = SqlDbType.NVarChar;
                    obj1 = state;
                }

                if (city != "")
                {
                    catCity = " AND A.CatCity=@p1 ";
                    i++;
                    two = SqlDbType.NVarChar;
                    obj2 = city;

                }

                types = new SqlDbType[i];
                data = new object[i];
                if (i > 0)
                {
                    types[0] = one;
                    data[0] = obj1;
                }
                if (i > 1)
                {
                    types[1] = two;
                    data[1] = obj2;
                }

                DataSet ds2 = GetDataWithParemeters("SELECT A.Header, A.Description, A.NumCurrentViews, A.FeaturedSummary, " +
                "A.Ad_ID, U.UserName, A.FeaturedPicture As FeaturedPicture2, " +
                "('../UserFiles/' + U.UserName + '/' + A.FeaturedPicture) As FeaturedPicture " +
                "FROM Ads A, Users U, Ad_Calendar AC WHERE   " + adBig + "  AC.AdID=A.Ad_ID AND A.Featured='True' " +
                "AND A.User_ID=U.User_ID AND AC.DateTimeStart <=  CONVERT(DATETIME,'"+timeNow.ToString()+"') AND  " +
                "A.NumCurrentViews < A.NumViews AND A.DisplayToAll='True' " + catCountry + catState + " ORDER BY A.NumCurrentViews ASC", types, data);

               

                if(ds2.Tables.Count > 0)
                    if (ds2.Tables[0].Rows.Count > 0)
                    {
                        DataView dv = new DataView(ds.Tables[0], "", "", DataViewRowState.CurrentRows);
                        for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
                        {
                            //Check first if the ad already exists in the categories dataset.
                            dv.RowFilter = "Ad_ID=" + ds2.Tables[0].Rows[j]["Ad_ID"].ToString();

                            if (dv.Count == 0)
                            {
                                DataRow row = ds.Tables[0].NewRow();
                                row["UserName"] = ds2.Tables[0].Rows[j]["UserName"].ToString();
                                row["Ad_ID"] = ds2.Tables[0].Rows[j]["Ad_ID"].ToString();
                                row["FeaturedSummary"] = ds2.Tables[0].Rows[j]["FeaturedSummary"].ToString();
                                row["Description"] = ds2.Tables[0].Rows[j]["Description"].ToString();
                                row["Header"] = ds2.Tables[0].Rows[j]["Header"].ToString();
                                row["FeaturedPicture"] = ds2.Tables[0].Rows[j]["FeaturedPicture"].ToString();
                                row["FeaturedPicture2"] = ds2.Tables[0].Rows[j]["FeaturedPicture2"].ToString();
                                ds.Tables[0].Rows.Add(row);
                            }
                        }
                    }
            }

                ds.Tables[0].Columns.Add("Row");
                for (int c = 0; c < ds.Tables[0].Rows.Count; c++)
                {
                    ds.Tables[0].Rows[c]["Row"] = c.ToString();
                }
            
            Session[userSessionName] = ds;
        }

        DataSet dsReturned = (DataSet)Session[userSessionName];
        if (dsReturned == null)
        {
            dsReturned = GetGlobalAds(isBig, hippoHappeningsUserID.ToString());
            dsReturned.Tables[0].Columns.Add("Row");
            for (int c = 0; c < dsReturned.Tables[0].Rows.Count; c++)
            {
                dsReturned.Tables[0].Rows[c]["Row"] = c.ToString();
            }
        }
        else if (dsReturned.Tables.Count == 0)
        {
            dsReturned = GetGlobalAds(isBig, hippoHappeningsUserID.ToString());
            dsReturned.Tables[0].Columns.Add("Row");
            for (int c = 0; c < dsReturned.Tables[0].Rows.Count; c++)
            {
                dsReturned.Tables[0].Rows[c]["Row"] = c.ToString();
            }
        }
        else if (dsReturned.Tables[0].Rows.Count == 0)
        {
            dsReturned = GetGlobalAds(isBig, hippoHappeningsUserID.ToString());
            dsReturned.Tables[0].Columns.Add("Row");
            for (int c = 0; c < dsReturned.Tables[0].Rows.Count; c++)
            {
                dsReturned.Tables[0].Rows[c]["Row"] = c.ToString();
            }
        }

        return dsReturned;
        
    }

    public DataSet RetrieveAdminAd()
    {
        return GetData("SELECT A.Header, '-1' As Row, A.Description, A.FeaturedSummary, " +
            "A.Ad_ID, U.UserName, A.FeaturedPicture As FeaturedPicture2, " +
            "('../UserFiles/' + U.UserName + '/' + A.FeaturedPicture) As FeaturedPicture " +
            "FROM Ads A, Users U WHERE A.Ad_ID=45 " +
            "AND A.User_ID=U.User_ID");
    }

    public int PerformAdCount(string adNumber, bool setSession)
    {
        
        DataSet ds = new DataSet();
        int index = 0;
        string id = "";
        bool increment = false;
        if (Session["User"] != null)
        {
            ds = (DataSet)Session["Ad" + adNumber + "_" + Session["User"].ToString()];
            index = int.Parse(Session["Ad" + adNumber + "_Count" + Session["User"].ToString()].ToString());
            if (index < numberOfAdsInADay)
            {
                id = ds.Tables[0].Rows[index]["Ad_ID"].ToString();
                Session["Ad" + adNumber + "_Count" + Session["User"].ToString()] = index + 1;
                increment = true;
            }
        }
        else
        {
            ds = (DataSet)Session["Ad" + adNumber + "_Generic"];
            index = int.Parse(Session["Ad" + adNumber + "_Count_Generic"].ToString());
            if (index < numberOfAdsInADay)
            {
                id = ds.Tables[0].Rows[index]["Ad_ID"].ToString();
                Session["Ad" + adNumber + "_Count_Generic"] = index + 1;
                increment = true;
            }
        }


        if (setSession)
        {
            if (Session["User"] != null)
                Session[Session["User"].ToString() + "_lastseen"] = id;
            else
                Session["GenericUser_lastseen"] = id;
        }
        if (id.Trim() != "45" && increment)
        {


            if (Session["User"] != null)
            {
                DataSet dsHasUserSeen = GetData("SELECT * FROM Ads_Seen_By_User WHERE Ad_ID=" + id + " AND User_ID=" + Session["User"].ToString());
                bool incrementIt = false;
                if (dsHasUserSeen.Tables.Count == 0)
                    incrementIt = true;
                else if (dsHasUserSeen.Tables[0].Rows.Count == 0)
                    incrementIt = true;

                if (incrementIt)
                {
                    Execute("UPDATE Ads SET NumCurrentViews = NumCurrentViews + 1 WHERE Ad_ID=" + id);
                    Execute("INSERT INTO Ads_Seen_By_User (Ad_ID, User_ID, Date) VALUES(" + id + ", " + Session["User"].ToString() + ", '" + timeNow.ToString() + "')");
                }
            }
            else
            {
                DataSet dsHasUserSeen = GetData("SELECT * FROM Ads_Seen_Generic WHERE Ad_ID=" + id +" AND IP='"+GetIP().Trim()+"'");
                bool incrementIt = false;
                if (dsHasUserSeen.Tables.Count == 0)
                    incrementIt = true;
                else if (dsHasUserSeen.Tables[0].Rows.Count == 0)
                    incrementIt = true;

                if (incrementIt)
                {
                    Execute("UPDATE Ads SET NumCurrentViews = NumCurrentViews + 1 WHERE Ad_ID=" + id);
                    Execute("INSERT INTO Ads_Seen_Generic (Date, IP, SessionTransfered, Ad_ID, SessionID) VALUES('"+timeNow.ToString()+"', '"+GetIP().Trim()+"', 'False', "+id+", '"+Session["GenericSession"].ToString()+"')");
                }
            }
        }
        return 0;
    }

    public int PerformMainAdCount(bool setSession)
    {

        DataSet ds = new DataSet();
        int index = 0;
        string id = "";
        bool increment = false;
        if (Session["User"] != null)
        {
            ds = (DataSet)Session["AdMain_" + Session["User"].ToString()];
            index = int.Parse(Session["AdMain_Count" + Session["User"].ToString()].ToString());
            if (index < numberOfAdsInADay)
            {
                id = ds.Tables[0].Rows[index]["Ad_ID"].ToString();
                Session["AdMain_Count" + Session["User"].ToString()] = index + 1;
                increment = true;
            }
        }
        else
        {
            ds = (DataSet)Session["AdMain_Generic"];
            index = int.Parse(Session["AdMain_Count_Generic"].ToString());
            if (index < numberOfMainAdsInADay)
            {
                id = ds.Tables[0].Rows[index]["Ad_ID"].ToString();
                Session["AdMain_Count_Generic"] = index + 1;
                increment = true;
            }
        }


        if (setSession)
        {
            if (Session["User"] != null)
                Session[Session["User"].ToString() + "_Main_lastseen"] = id;
            else
                Session["GenericUser_Main_lastseen"] = id;
        }
        if (id.Trim() != "45" && increment)
        {


            if (Session["User"] != null)
            {
                DataSet dsHasUserSeen = GetData("SELECT * FROM Ads_Seen_By_User WHERE Ad_ID=" + id + " AND User_ID=" + Session["User"].ToString());
                bool incrementIt = false;
                if (dsHasUserSeen.Tables.Count == 0)
                    incrementIt = true;
                else if (dsHasUserSeen.Tables[0].Rows.Count == 0)
                    incrementIt = true;

                if (incrementIt)
                {
                    Execute("UPDATE Ads SET NumCurrentViews = NumCurrentViews + 1 WHERE Ad_ID=" + id);
                    Execute("INSERT INTO Ads_Seen_By_User (Ad_ID, User_ID, Date) VALUES(" + id + ", " + Session["User"].ToString() + ", '" + timeNow.ToString() + "')");
                }
            }
            else
            {
                DataSet dsHasUserSeen = GetData("SELECT * FROM Ads_Seen_Generic WHERE Ad_ID=" + id + " AND IP='" + GetIP().Trim() + "'");
                bool incrementIt = false;
                if (dsHasUserSeen.Tables.Count == 0)
                    incrementIt = true;
                else if (dsHasUserSeen.Tables[0].Rows.Count == 0)
                    incrementIt = true;

                if (incrementIt)
                {
                    Execute("UPDATE Ads SET NumCurrentViews = NumCurrentViews + 1 WHERE Ad_ID=" + id);
                    Execute("INSERT INTO Ads_Seen_Generic (Date, IP, SessionTransfered, Ad_ID, SessionID) VALUES('" + timeNow.ToString() + "', '" + GetIP().Trim() + "', 'False', " + id + ", '" + Session["GenericSession"].ToString() + "')");
                }
            }
        }
        return 0;
    }

    public void SetLocationForIP(string country, string city, string state)
    {
        string ip = GetIP();
        DataSet dsIP = GetData("SELECT * FROM SearchIPs WHERE IP='" + ip + "'");
        bool isInsert = false;
        SqlDbType[] types = new SqlDbType[2];
        object[] data = new object[2];
        types[0] = SqlDbType.NVarChar;
        types[1] = SqlDbType.NVarChar;
        data[0] = state;
        data[1] = city;

        if (dsIP.Tables.Count > 0)
            if (dsIP.Tables[0].Rows.Count > 0)
            {
                 ExecuteWithParemeters("UPDATE SearchIPs SET Country=" + country +
                    ", State=@p0, City=@p1 WHERE ID=" + dsIP.Tables[0].Rows[0]["ID"].ToString(), types, data);
            }
            else
                isInsert = true;
        else
            isInsert = true;

        if (isInsert)
        {
            ExecuteWithParemeters("INSERT INTO SearchIPs  (IP, Country, State, City) VALUES ('"+ip+"', "+country+
                ", @p0, @p1)", types, data);
        }
    }

    //Code from: http://www.aspsnippets.com/post/2009/04/12/Find-Visitors-Geographic-Location-using-IP-Address-in-ASPNet.aspx
    public DataTable GetLocation(string ipaddress)
    {

        //Create a WebRequest 
        WebRequest rssReq =

            WebRequest.Create("http://freegeoip.appspot.com/xml/"

                + ipaddress);



        //Create a Proxy 

        WebProxy px =

           new WebProxy("http://freegeoip.appspot.com/xml/"

                + ipaddress, true);



        //Assign the proxy to the WebRequest 

        rssReq.Proxy = px;



        //Set the timeout in Seconds for the WebRequest 

        rssReq.Timeout = 2000;

        try
        {

            //Get the WebResponse 

            WebResponse rep = rssReq.GetResponse();



            //Read the Response in a XMLTextReader 

            XmlTextReader xtr = new XmlTextReader(rep.GetResponseStream());



            //Create a new DataSet 

            DataSet ds = new DataSet();



            //Read the Response into the DataSet 

            ds.ReadXml(xtr);

            return ds.Tables[0];

        }

        catch
        {

            return null;

        }
    }

    public bool IsOwnerUpForGrabs(string VenueID, ref string ownerID, ref bool isOwner, bool isVenue)
    {
        bool ownerUpForGrabs = false;
        isOwner = false;
        string command = "SELECT * FROM Events WHERE ID=";
        if (isVenue)
        {
            command = "SELECT * FROM Venues WHERE ID=";
        }

            DataSet dsVenue = GetData(command + VenueID);

            if (dsVenue.Tables[0].Rows[0]["Owner"] != null)
            {
                if (dsVenue.Tables[0].Rows[0]["Owner"].ToString().Trim() != "")
                {
                    if (dsVenue.Tables[0].Rows[0]["Owner"].ToString() == Session["User"].ToString())
                        isOwner = true;

                    ownerID = dsVenue.Tables[0].Rows[0]["Owner"].ToString();
                }
                else
                {
                    ownerUpForGrabs = true;
                }
            }
            else
            {
                ownerUpForGrabs = true;
            }

    
        

        return ownerUpForGrabs;
    }

    public bool HasEventPassed(string EventID)
    {
        bool hasEventPassed = false;

        DataSet dsEvent = GetData("SELECT * FROM Event_Occurance WHERE EventID="+EventID +
            " ORDER BY DateTimeEnd DESC");
        if (DateTime.Parse(dsEvent.Tables[0].Rows[0]["DateTimeEnd"].ToString()) < timeNow)
        {
            hasEventPassed = true;
        }

        return hasEventPassed;
    }

    public string MakeGoodPhone(string badPhone)
    {
        string goodPhone = "";
        int tryer;
        for (int i = 0; i < badPhone.Length; i++)
        {
            try
            {
                tryer = int.Parse(badPhone[i].ToString());
                goodPhone += tryer.ToString();
            }
            catch (Exception ex)
            {

            }
        }
        return goodPhone;
    }

    public bool IsOwnerDelinquent(string contentID, bool isLocal, string VenueOrEvent)
    {
        //Function is used for both venues and events
        string ContentTable = "Venues";
        string HistoryTable = "VenueOwnerHistory";
        string ContentStrID = "VenueID";
        string RevisionsTable = "VenueRevisions";
        string CategoryRevTable = "VenueCategoryRevisions";
        string HeaderField = "Name";
        string nullsString = "OR  NOT(Email is null) OR   NOT(Phone is null) OR   NOT(Address is null) OR   NOT(State is null) OR   " +
            "NOT(Zip is null) OR   NOT(Country is null) OR   "+
            "NOT(City is null) OR   NOT(Content is null)";
        bool isVenue = true;
        int daysToForfit = -7;
        if (VenueOrEvent.ToUpper() == "E")
        {
            daysToForfit = -4;
            isVenue = false;
            ContentTable = "Events";
            HistoryTable = "EventOwnerHistory";
            ContentStrID = "EventID";
            RevisionsTable = "EventRevisions";
            CategoryRevTable = "EventCategoryRevisions";
            HeaderField = "Header";
            nullsString = "OR  NOT(Venue is null) OR   NOT(Address is null) OR   NOT(State is null) OR   NOT(Zip is null) OR   NOT(Country is null) OR   NOT(ShortDescription is null) OR   NOT(City is null) OR   NOT(Content is null)";
        }



        //Query whether current owner was delinquent on approve/reject changes
        //1. if the Owner column in venues table is null, then we know that the ownership is open
        //2. if it is not null, check if the current owner is delinquent on any messages and if so
        //      set the Owner column to null.

        bool isOwnerNull = false;
        DataSet dsVenue = GetData("SELECT U.Email AS TheEmail, * FROM "+ContentTable+" V, Users U WHERE V.ID="+contentID + " AND V.Owner=U.User_ID");

        if (dsVenue.Tables.Count > 0)
        {
            if (dsVenue.Tables[0].Rows.Count > 0)
            {
                if (dsVenue.Tables[0].Rows[0]["Owner"] != null)
                {
                    if (dsVenue.Tables[0].Rows[0]["Owner"].ToString().Trim() == "")
                        isOwnerNull = true;
                }
                else
                {
                    isOwnerNull = true;
                }
            }
            else
            {
                isOwnerNull = true;
            }
        }
        else
        {
            isOwnerNull = true;
        }

        if (isOwnerNull)
        {
            return true;
        }
        else
        {
            string ownerID = dsVenue.Tables[0].Rows[0]["Owner"].ToString();
            DataSet dsOwnerDate = GetData("SELECT * FROM "+HistoryTable+" WHERE "+ContentStrID+"="+contentID+
                " AND OwnerID="+dsVenue.Tables[0].Rows[0]["Owner"].ToString() + " ORDER BY DateCreatedOwnership DESC");
            //get the latest date that the current owner became the owner.
            //only count the messages from this date.
            string dateClaimedOwnership = dsOwnerDate.Tables[0].Rows[0]["DateCreatedOwnership"].ToString();

            DataSet dsApproved = GetData("SELECT * FROM "+RevisionsTable+" WHERE Approved is null AND "+
                ContentStrID+"=" +
                contentID + " AND DATE < CONVERT(DATETIME,'" + timeNow.AddDays(daysToForfit) +
                "') AND DATE > CONVERT(DATETIME,'" + dateClaimedOwnership + "') AND (NOT("+HeaderField+
                " is null) "+nullsString+"  )");

            bool checkCategories = false;

            bool hasDelinquent = false;

            if (dsApproved.Tables.Count > 0)
                if (dsApproved.Tables[0].Rows.Count > 0)
                    hasDelinquent = true;
                else
                    checkCategories = true;
            else
                checkCategories = true;

            if (checkCategories)
            {
                dsApproved = GetData("SELECT * FROM "+CategoryRevTable+" WHERE Approved is null AND "+
                    ContentStrID+"=" +
                contentID + " AND DATE < CONVERT(DATETIME,'" + timeNow.AddDays(daysToForfit) + 
                "')  AND DATE > CONVERT(DATETIME,'" + dateClaimedOwnership + "')");

                if (dsApproved.Tables.Count > 0)
                    if (dsApproved.Tables[0].Rows.Count > 0)
                        hasDelinquent = true;
            }

            //For the Events we still have to check the EventRevisions_Occurances table
            if (!hasDelinquent && !isVenue)
            {
                dsApproved = GetData("SELECT * FROM EventRevisions_Occurance WHERE Approved is null AND " + 
                    ContentStrID + "=" +
                contentID + " AND DATE < CONVERT(DATETIME,'" + timeNow.AddDays(daysToForfit) + 
                "')  AND DATE > CONVERT(DATETIME,'" + dateClaimedOwnership + "')");

                if (dsApproved.Tables.Count > 0)
                    if (dsApproved.Tables[0].Rows.Count > 0)
                        hasDelinquent = true;
            }

            //if the current owner is delinquent, set Owner column in venues to null.
            //Second time we are in this method, we just need to check whether the Owner column is 'null'.
            //Send email to the previous owner telling them that their ownership has been automatically waived.
            if (hasDelinquent)
            {
                Execute("UPDATE "+ContentTable+" SET Owner=null WHERE ID="+contentID);

                if (!isLocal)
                {
                    if (isVenue)
                    {
                        string emailBody = "Hello from HippoHappenings <br/><br/> We would like to inform you that your <b>ownership for the venue '" + dsVenue.Tables[0].Rows[0]["Name"].ToString() +
                            "' has been automatically taken away</b>. This happened because you have been delinquent on responding to users' request for changes on this venue's page. <br/>" +
                            "You can <b>re-claim your ownership</b> by going to this <a class=\"AddLink\" href=\"http://HippoHappenings.com/"+MakeNiceName(dsVenue.Tables[0].Rows[0]["Name"].ToString())+"_" + contentID + "_Venue\">venue's page</a> and clicking on the 'Venue ownership open' button. You must hury before another user takes over the ownership of this venue. However, if you do not respond to users requests within 7 days, your ownership will be taken away once again.<br/><br/>" +
                            "<b>If you no longer wish to be the owner of this venue, you need not do anything.</b> <br/><br/>Have a Hippo Happening day!";

                        Execute("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, To_UserID, " +
                            "[Date], [Read], [Mode], [Live]) VALUES('" + 
                            emailBody.Replace("'", "''") + "', 'Your Ownership for venue " + 
                            dsVenue.Tables[0].Rows[0]["Name"].ToString() + " has been taken away.', "+
                            hippoHappeningsUserID.ToString()+", " + 
                            ownerID + ", '" +
                            timeNow.ToString() + "', 'False', 1, 'True')");

                        SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                         System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                         dsVenue.Tables[0].Rows[0]["TheEmail"].ToString(), emailBody, "Your Ownership for venue '" + dsVenue.Tables[0].Rows[0]["Name"].ToString() + "' has been taken away.");
                    }
                    else
                    {
                        string emailBody = "Hello from HippoHappenings <br/><br/> We would like to inform you that your <b>ownership for the event '" + dsVenue.Tables[0].Rows[0][HeaderField].ToString() +
                            "' has been automatically taken away</b>. This happened because you have been delinquent on responding to users' request for changes on this event's page. <br/>" +
                            "You can <b>re-claim your ownership</b> by going to this <a class=\"AddLink\" href=\"http://HippoHappenings.com/"+MakeNiceName(dsVenue.Tables[0].Rows[0][HeaderField].ToString())+"_" + contentID + "_Event\">event's page</a> and clicking on the 'Event ownership open' button.  You must hurry before another user takes over the ownership of this venue. However, if you do not respond to users requests within 4 days, your ownership will be taken away once again.<br/><br/>" +
                            "<b>If you no longer wish to be the owner of this event, you need not do anything.</b> <br/><br/>Have a Hippo Happening day!";

                        Execute("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, To_UserID, " +
                            "[Date], [Read], [Mode], [Live]) VALUES('" + emailBody.Replace("'", "''") + "', 'Your Ownership for event " +
                            dsVenue.Tables[0].Rows[0]["Header"].ToString() + " has been taken away.', " + hippoHappeningsUserID.ToString() +
                            ", " + ownerID + ", '" +
                            timeNow.ToString() + "', 'False', 1, 'True')");

                        SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                         System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                         dsVenue.Tables[0].Rows[0]["TheEmail"].ToString(), emailBody, "Your Ownership for event '" + dsVenue.Tables[0].Rows[0]["Header"].ToString() + "' has been taken away.");
                    }
                }
            }


            return hasDelinquent;

        }
    }

    public bool isNotSeen(DataSet dsEvent2, int columnNumber)
    {
        if (dsEvent2.Tables[0].Rows[0]["Approved"] != null)
        {
            if (dsEvent2.Tables[0].Rows[0]["Approved"].ToString().Trim() != "")
            {
                string approved = dsEvent2.Tables[0].Rows[0]["Approved"].ToString();
                char[] delim = { ';' };
                string[] tokens = approved.Trim().Split(delim);

                if (tokens.Length < columnNumber+1)
                    return true;
                else
                {
                    if (tokens[columnNumber].Trim().ToLower() == "null" || tokens[columnNumber].Trim().ToLower() == "")
                        return true;
                    else
                        return false;
                }
            }
            else
            {
                return true;
            }

        }
        else
        {
            return true;
        }
    }

    public bool isApproved(DataSet dsEvent2, int columnNumber)
    {
        if (dsEvent2.Tables[0].Rows[0]["Approved"] != null)
        {
            if (dsEvent2.Tables[0].Rows[0]["Approved"].ToString().Trim() != "")
            {
                string approved = dsEvent2.Tables[0].Rows[0]["Approved"].ToString();
                char[] delim = { ';' };
                string[] tokens = approved.Trim().Split(delim);

                if (tokens.Length < columnNumber+1)
                    return false;
                else
                {
                    if (tokens[columnNumber].Trim().ToLower() == "null" || 
                        tokens[columnNumber].Trim().ToLower() == "")
                        return false;
                    else
                    {
                        if (tokens[columnNumber].Trim().ToLower() == "false")
                            return false;
                        else
                            return true;
                    }
                }
            }
            else
            {
                return false;
            }

        }
        else
        {
            return false;
        }
    }

    public void ApproveRejectChange(string table, string rowID, int columnNumber, bool approved)
    {
        DataSet dsVenue = GetData("SELECT * FROM "+table+" WHERE ID=" + rowID);
        string approvedStr = "";
        bool getFull = false;
        char[] delim = { ';' };
        if (dsVenue.Tables[0].Rows[0]["Approved"] != null)
        {
            if (dsVenue.Tables[0].Rows[0]["Approved"].ToString().Trim() != "")
            {
                string[] tokens = dsVenue.Tables[0].Rows[0]["Approved"].ToString().Trim().Split(delim);

                bool gotten = false;
                int leftOff = 0;
                for (int i = 0; i < tokens.Length; i++)
                {
                    if (tokens[i].Trim() != "")
                    {
                        if (i != columnNumber)
                        {
                            approvedStr += tokens[i] + ";";
                            leftOff = i;
                        }
                        else
                        {
                            approvedStr += approved.ToString() + ";";
                            gotten = true;
                        }
                    }
                }

                if (!gotten)
                {
                    for (int j = 1; j < columnNumber - leftOff; j++)
                    {
                        approvedStr += "null;";
                    }
                    approvedStr += approved.ToString() + ";";
                }
            }
            else
            {
                getFull = true;
            }
        }
        else
        {
            getFull = true;
        }

        if (getFull)
        {
            int j = 0;
            while (j < columnNumber)
            {
                approvedStr += "null;";
                j++;
            }
            approvedStr += approved.ToString()+";";
        }

        Execute("UPDATE "+table+" SET Approved='" + approvedStr + "' WHERE ID=" + rowID);
    }

    public string GetAddress(string address, bool isInternational)
    {
        char[] delim42 = { ';' };
        string[] tokens42 = address.Split(delim42, StringSplitOptions.RemoveEmptyEntries);

        if (!isInternational)
        {
            string temp = tokens42[1][0].ToString().ToUpper();
            temp += tokens42[1].Trim().Substring(1, tokens42[1].Length - 1);

            string theReturn = tokens42[0].Trim() + " " + temp + " ";

            for (int i = 2; i < tokens42.Length; i++)
            {
                theReturn += tokens42[i].Trim() + " ";
            }
            return theReturn;
        }
        else
        {
            if (tokens42.Length == 1)
                return tokens42[0].Trim();
            else
            {
                if (tokens42[1].Trim() != "")
                {
                    return tokens42[0].Trim() + " " + tokens42[1].Trim();
                }
                else
                {
                    return tokens42[0].Trim();
                }
            }
        }

        return address;
    }

    /// <summary>
    /// Returns number of days between the two dates. Dates have to be in ajecent years.
    /// </summary>
    /// <param name="biggerDate">The date that is later.</param>
    /// <param name="lesserDate">The date that is earlier.</param>
    /// <returns>Number of days between the dates. Negative number if lesserDate is greater than bigger Date</returns>
    public int GetDayDifference(DateTime biggerDate, DateTime lesserDate)
    {
        if (biggerDate.Year == lesserDate.Year)
        {
            return biggerDate.DayOfYear - lesserDate.DayOfYear;
        }
        else
        {
            int numDaysInLesser = 365;
            if (DateTime.IsLeapYear(lesserDate.Year))
                numDaysInLesser = 366;

            int numDaysInBigger = 365;
            if (DateTime.IsLeapYear(biggerDate.Year))
                numDaysInBigger = 366;

            return (numDaysInLesser - lesserDate.DayOfYear) + numDaysInBigger - biggerDate.DayOfYear;
        }
    }

    public string MakeNiceName(string str)
    {
        str = MakeNiceNameFull(str);
        if (str.Length > 0)
        {
            char [] delim = {'-'};
            string[] urlTokens = str.Split(delim, StringSplitOptions.RemoveEmptyEntries);
            int i = 0;
            str = "";
            foreach (string token in urlTokens)
            {
                if (i < 6)
                {
                    if (str != "")
                        str += "-";
                    str += token;
                    i++;
                }
                else
                {
                    break;
                }
            }
        }
        return str;
    }

    public string MakeNiceNameFive(string str)
    {
        str = MakeNiceNameFull(str);
        if (str.Length > 0)
        {
            char[] delim = { '-' };
            string[] urlTokens = str.Split(delim, StringSplitOptions.RemoveEmptyEntries);
            int i = 0;
            str = "";
            foreach (string token in urlTokens)
            {
                if (i < 5)
                {
                    if (str != "")
                        str += "-";
                    str += token;
                    i++;
                }
                else
                {
                    break;
                }
            }
        }
        return str;
    }

    public string MakeNiceNameFull(string str)
    {
        str = stripHTML(str).Replace((char)10, '-').Replace((char)13, '-').Replace("\r", "-").Replace("\t", "-").Replace("\n", "-").Replace("+", "-").Replace("'", "-").Replace("&", "-").Replace(";",
                "-").Replace(":", "-").Replace(",", "-").Replace("\"", "-").Replace("\\",
                "-").Replace("%", "-").Replace("(", "-").Replace(")", "-").Replace(" ",
                "-").Replace("-", "-").Replace("/", "-").Replace("!", "-").Replace("@",
                "-").Replace("#", "-").Replace("$", "-").Replace("^", "-").Replace("*",
                "-").Replace("=", "-").Replace("[", "-").Replace("]", "-").Replace("{",
                "-").Replace("}", "-").Replace("|", "-").Replace("?", "-").Replace("<",
                "-").Replace(">", "-").Replace(".", "-").Replace("~", "-").Replace("`",
                "-").Replace("“", "").Replace("”", "").Replace(".", "").Replace("…", "").Replace("!",
                "-").Replace("-----", "-").Replace("----", "-").Replace("---",
                "-").Replace("--", "-");
        if (str.Length > 0)
            if (str.Substring(str.Length - 1, 1) == "-")
                str = str.Substring(0, str.Length - 1);
        return str;
    }

    public string CleanExcelString(string str)
    {
        return str.Replace((char)10, '_').Replace((char)13, '_').Replace("\r",
            "_").Replace("\t", "_").Replace("\n", "_");
    }

    public string Reverse(string str)
    {
        string tmp = "";
        for (int i = 0; i < str.Length; i++)
        {
            tmp += str[str.Length - i - 1].ToString();
        }
        return tmp;
    }

    public string GetImage(string searchNumber)
    {
        switch (searchNumber)
        {
            case "0":
                return "A";
                break;
            case "1":
                return "B";
                break;
            case "2":
                return "C";
                break;
            case "3":
                return "D";
                break;
            case "4":
                return "E";
                break;
            case "5":
                return "F";
                break;
            case "6":
                return "G";
                break;
            case "7":
                return "H";
                break;
            case "8":
                return "I";
                break;
            case "9":
                return "J";
                break;
            case "10":
                return "K";
                break;
            case "11":
                return "L";
                break;
            case "12":
                return "M";
                break;
            case "13":
                return "N";
                break;
            case "14":
                return "O";
                break;
            case "15":
                return "P";
                break;
            case "16":
                return "Q";
                break;
            case "17":
                return "R";
                break;
            case "18":
                return "S";
                break;
            case "19":
                return "T";
                break;
            case "20":
                return "U";
                break;
            case "21":
                return "V";
                break;
            case "22":
                return "W";
                break;
            case "23":
                return "X";
                break;
            case "24":
                return "Y";
                break;
            case "25":
                return "Z";
                break;
            default:
                return "";
                break;

        }
    }

    public string GetImageNum(string searchLetter)
    {
        switch (searchLetter)
        {
            case "A":
                return "0";
                break;
            case "B":
                return "1";
                break;
            case "C":
                return "2";
                break;
            case "D":
                return "3";
                break;
            case "E":
                return "4";
                break;
            case "F":
                return "5";
                break;
            case "G":
                return "6";
                break;
            case "H":
                return "7";
                break;
            case "I":
                return "8";
                break;
            case "J":
                return "9";
                break;
            case "K":
                return "10";
                break;
            case "L":
                return "11";
                break;
            case "M":
                return "12";
                break;
            case "N":
                return "13";
                break;
            case "O":
                return "14";
                break;
            case "P":
                return "15";
                break;
            case "Q":
                return "16";
                break;
            case "R":
                return "17";
                break;
            case "S":
                return "18";
                break;
            case "T":
                return "19";
                break;
            case "U":
                return "20";
                break;
            case "V":
                return "21";
                break;
            case "W":
                return "22";
                break;
            case "X":
                return "23";
                break;
            case "Y":
                return "24";
                break;
            case "Z":
                return "25";
                break;
            default:
                return "";
                break;

        }
    }

    public string GetCalendarLetter(int searchNumber)
    {
        int numOfDivided = searchNumber / 26;
        int remainder = searchNumber - numOfDivided * 26;

        string theLetter = "";

        string firstSwitch = remainder.ToString();
        //if (remainder == 0)
        //{
        //    firstSwitch = "25";
        //}
        //else
        //{
        //    firstSwitch = (remainder - 1).ToString();
        //}

        switch (firstSwitch)
        {
            case "0":
                theLetter = "A";
                break;
            case "1":
                theLetter = "B";
                break;
            case "2":
                theLetter = "C";
                break;
            case "3":
                theLetter = "D";
                break;
            case "4":
                theLetter = "E";
                break;
            case "5":
                theLetter = "F";
                break;
            case "6":
                theLetter = "G";
                break;
            case "7":
                theLetter = "H";
                break;
            case "8":
                theLetter = "I";
                break;
            case "9":
                theLetter = "J";
                break;
            case "10":
                theLetter = "K";
                break;
            case "11":
                theLetter = "L";
                break;
            case "12":
                theLetter = "M";
                break;
            case "13":
                theLetter = "N";
                break;
            case "14":
                theLetter = "O";
                break;
            case "15":
                theLetter = "P";
                break;
            case "16":
                theLetter = "Q";
                break;
            case "17":
                theLetter = "R";
                break;
            case "18":
                theLetter = "S";
                break;
            case "19":
                theLetter = "T";
                break;
            case "20":
                theLetter = "U";
                break;
            case "21":
                theLetter = "V";
                break;
            case "22":
                theLetter = "W";
                break;
            case "23":
                theLetter = "X";
                break;
            case "24":
                theLetter = "Y";
                break;
            case "25":
                theLetter = "Z";
                break;
            default:
                break;

        }

        if (numOfDivided == 0)
            return theLetter;
        else
            return theLetter + numOfDivided.ToString();
    }

    public string GetHours(string theDays)
    {
        char[] delim = { ';' };
        string[] tokens;
        int firstToken = 0;
        int secondToken = 0;
        int tokenCount = 0;
        string hoursText = "";
        string hoursTexts = "";
        bool isDash = false;
        bool allow = false;

        if (theDays == "1;2;3;4;5;6;7;")
        {
            return "<br/>Mon-Sun<br/>";
        }
        else
        {
            tokens = theDays.ToString().Split(delim, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 1)
            {
                return "<br/>" + GetDay2(int.Parse(tokens[0]), true) + "<br/>";
            }
            else
            {
                hoursTexts = "";
                allow = false;
                firstToken = int.Parse(tokens[0]);
                secondToken = int.Parse(tokens[1]);
                tokenCount = 2;

                hoursText = GetDay2(firstToken, true) + ", " + GetDay2(secondToken, true);

                if (tokens.Length > 2)
                {
                    while (secondToken != int.Parse(tokens[tokens.Length - 1]) || allow)
                    {
                        hoursText = GetDay2(firstToken, true);
                        isDash = false;
                        while (firstToken == secondToken - 1 && tokenCount < tokens.Length)
                        {
                            firstToken = secondToken;
                            secondToken = int.Parse(tokens[tokenCount++]);
                            if (!isDash)
                            {
                                hoursText += " - ";
                                isDash = true;
                            }
                        }

                        if (tokenCount == tokens.Length && firstToken == secondToken - 1)
                            firstToken = secondToken;

                        if (!isDash)
                        {
                            hoursText += ", ";
                        }
                        else
                        {
                            hoursText += GetDay2(firstToken, true);
                        }

                        hoursTexts += hoursText;
                        if (secondToken != int.Parse(tokens[tokens.Length - 1]))
                        {
                            if (isDash)
                                hoursTexts += ", ";
                            firstToken = secondToken;
                            secondToken = int.Parse(tokens[tokenCount++]);
                            if (secondToken == int.Parse(tokens[tokens.Length - 1]))
                            {
                                allow = true;
                                tokenCount--;
                            }
                        }
                        else if (secondToken == int.Parse(tokens[tokens.Length - 1]) &&
                            firstToken != secondToken - 1 && firstToken != secondToken)
                        {
                            if (isDash)
                                hoursTexts += ", ";
                            firstToken = secondToken;
                            allow = true;
                        }
                        else
                            allow = false;
                    }
                }
                if (hoursTexts == "")
                    hoursTexts = hoursText;

                if (hoursTexts.Trim()[hoursTexts.Trim().Length - 1] == ',')
                    hoursTexts = hoursTexts.Trim().Substring(0, hoursTexts.Trim().Length - 1);
                allow = false;
                return "<br/>" + hoursTexts + "<br/>";
            }
        }
    }

    public string GetMonths(string theDays)
    {
        char[] delim = { ';' };
        string[] tokens;
        int firstToken = 0;
        int secondToken = 0;
        int tokenCount = 0;
        string hoursText = "";
        string hoursTexts = "";
        bool isDash = false;
        bool allow = false;

        if (theDays == "1;2;3;4;5;6;7;8;9;10;11;12;")
        {
            return "Jan-Dec";
        }
        else
        {
            tokens = theDays.ToString().Split(delim, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 1)
            {
                return GetMonth(tokens[0]).Substring(0, 3);
            }
            else
            {
                hoursTexts = "";
                allow = false;
                firstToken = int.Parse(tokens[0]);
                secondToken = int.Parse(tokens[1]);
                tokenCount = 2;

                hoursText = GetMonth(firstToken.ToString()).Substring(0, 3) + ", " + GetMonth(secondToken.ToString()).Substring(0, 3);

                if (tokens.Length > 2)
                {
                    while (secondToken != int.Parse(tokens[tokens.Length - 1]) || allow)
                    {
                        hoursText = GetMonth(firstToken.ToString()).Substring(0, 3);
                        isDash = false;
                        while (firstToken == secondToken - 1 && tokenCount < tokens.Length)
                        {
                            firstToken = secondToken;
                            secondToken = int.Parse(tokens[tokenCount++]);
                            if (!isDash)
                            {
                                hoursText += " - ";
                                isDash = true;
                            }
                        }

                        if (tokenCount == tokens.Length && firstToken == secondToken - 1)
                            firstToken = secondToken;

                        if (!isDash)
                        {
                            hoursText += ", ";
                        }
                        else
                        {
                            hoursText += GetMonth(firstToken.ToString()).Substring(0, 3);
                        }

                        hoursTexts += hoursText;
                        if (secondToken != int.Parse(tokens[tokens.Length - 1]))
                        {
                            if (isDash)
                                hoursTexts += ", ";
                            firstToken = secondToken;
                            secondToken = int.Parse(tokens[tokenCount++]);
                            if (secondToken == int.Parse(tokens[tokens.Length - 1]))
                            {
                                allow = true;
                                tokenCount--;
                            }
                        }
                        else if (secondToken == int.Parse(tokens[tokens.Length - 1]) &&
                            firstToken != secondToken - 1 && firstToken != secondToken)
                        {
                            if (isDash)
                                hoursTexts += ", ";
                            firstToken = secondToken;
                            allow = true;
                        }
                        else
                            allow = false;
                    }
                }
                if (hoursTexts == "")
                    hoursTexts = hoursText;

                if (hoursTexts.Trim()[hoursTexts.Trim().Length - 1] == ',')
                    hoursTexts = hoursTexts.Trim().Substring(0, hoursTexts.Trim().Length - 1);
                allow = false;
                return hoursTexts;
            }
        }
    }

    public string GetDuration(string str)
    {
        char[] delim = { '-' };
        string[] tokens = str.Split(delim);

        char[] delim2 = { ':' };

        string start = tokens[0];
        string end = tokens[1];

        int startHours = int.Parse(start.Split(delim2)[0]);
        int endHours = int.Parse(end.Split(delim2)[0]);

        int numStartDays = startHours / 24;
        int numStartHours = startHours - (numStartDays * 24);

        int numEndDays = endHours / 24;
        int numEndHours = endHours - (numEndDays * 24);

        string returnStr = "";

        if (numStartDays > 0)
        {
            if (numStartDays == 1)
                returnStr = numStartDays + " day ";
            else
                returnStr = numStartDays + " days ";
        }

        if (numStartHours != 0)
        {
            if (numStartHours == 1)
                returnStr += numStartHours + " hr";
            else
                returnStr += numStartHours + " hr";
        }

        returnStr += " - ";

        if (numEndDays > 0)
        {
            if (numEndDays == 1)
                returnStr += numEndDays + " day ";
            else
                returnStr += numEndDays + " days ";
        }

        if (numEndHours != 0)
        {
            if (numEndHours == 1)
                returnStr += numEndHours + " hr";
            else
                returnStr += numEndHours + " hr";
        }

        return returnStr;
    }

    public string getDayOfWeek(DayOfWeek dayOfWeek)
    {
        switch (dayOfWeek)
        {
            case DayOfWeek.Monday:
                return "1";
                break;
            case DayOfWeek.Tuesday:
                return "2";
                break;
            case DayOfWeek.Wednesday:
                return "3";
                break;
            case DayOfWeek.Thursday:
                return "4";
                break;
            case DayOfWeek.Friday:
                return "5";
                break;
            case DayOfWeek.Saturday:
                return "6";
                break;
            case DayOfWeek.Sunday:
                return "7";
                break;
            default: return "1";
        }
    }

    public ArrayList grabEvents()
    {
        string URL = "http://newyork.craigslist.org/eve/";

        WebClient myClient = new WebClient();
        string webPageString = myClient.DownloadString(URL).Replace("\r", "").Replace("\n", "");

        //string result = CleanUpResult(webPageString.Replace("\n", " ").Replace("\t", " "));
        //XmlDocument xmlDoc = BuildXML(result);
        //xmlDoc.Save("output.xml");

        HtmlAgilityPack.HtmlDocument a = new HtmlAgilityPack.HtmlDocument();
        a.LoadHtml(webPageString);

        string date = "";
        string link = "";
        string header = "";
        bool hasImg = false;
        HtmlAgilityPack.HtmlDocument ad;

        //HtmlAgilityPack.HtmlNode n = new HtmlAgilityPack.HtmlNode();
        //n.Descendants();
        //n.ChildNodes;

        //n.SelectNodes("//p");

        IEnumerable<HtmlAgilityPack.HtmlNode> pNodes = a.DocumentNode.ChildNodes[1].ChildNodes[1].ChildNodes[5].SelectNodes("//p");

        //IEnumerable<HtmlAgilityPack.HtmlNode> pNodes = nodes;
        IEnumerable<HtmlAgilityPack.HtmlNode> subNodes;

        char[] delim = { '/' };
        string[] tokens;
        string url = "";
        string temp = "";
        ArrayList events = new ArrayList();

        foreach (HtmlAgilityPack.HtmlNode node in pNodes)
        {
            url = "";
            hasImg = false;
            if (node.ChildNodes.Count > 4)
                hasImg = true;

            if (hasImg)
            {
                date = node.ChildNodes[0].InnerText;
                link = node.ChildNodes[1].Attributes[0].Value;
                header = node.ChildNodes[1].InnerText;

                ad = new HtmlAgilityPack.HtmlDocument();
                webPageString = myClient.DownloadString(link).Replace("\r", "").Replace("\n", "");
                ad.LoadHtml(webPageString);
                subNodes = ad.DocumentNode.SelectNodes("//img");

                tokens = link.Split(delim);
                foreach (string token in tokens)
                {
                    if (token.Trim() != "" && !token.ToLower().Contains(".org")
                        && token.ToLower() != "http:" && token.ToLower() != "https:")
                    {
                        if (url != "")
                            url += "_";
                        url += token.Replace(".html", "");
                    }
                }

                foreach (HtmlAgilityPack.HtmlNode subNode in subNodes)
                {
                    temp = "<div style=\"width: 200px; height: 200px;overflow: hidden;" +
                    "position: relative;border: 2px solid #585757;\">" +
                    "<div align=\"center\" style=\"vertical-align: middle;\"><table cellpadding='0' " +
                        "cellspacing='0' height='192px' bgcolor='black'><tr><td valign='center'><a " +
                        "width='201px' href=\"http://HippoHappenings.com/" + MakeNiceName(header) +
                            "_CLHH_" + url + "_ClEvent" +
                            "\">" + subNode.OuterHtml.Replace("<img", "<img style=\"position: absolute; " +
                            "top: 0; left: 0;\" ") + "</a></td></tr></table>" +
                            "<div style=\"vertical-align: middle;color: white;position: absolute; bottom: " +
                            "0;width: 200px; " +
                            "background-color: black; opacity: .7; filter: alpha(opacity=70);height: 50px;\">" +
                            "<table cellpadding='0' cellspacing='0' height='50px'><tr><td valign='center'><div align='center'>" +
                            header + "</div></td></tr></table></div>" +
                            "</div></div>";
                    events.Add(temp);
                    break;
                }
            }
        }

        return events;
    }

    public string RemoveImages(string str)
    {
        string img = "<img";
        char ch = 'd';
        int index = str.IndexOf(img);

        string temp = str.Substring(0, index);

        while (index != -1)
        {
            str = str.Remove(index, str.Substring(index).IndexOf(">") + 1);

            index = str.IndexOf(img);
        }

        return str;
    }

    public Hashtable grabEvent(string URL)
    {
        WebClient myClient = new WebClient();
        string webPageString = myClient.DownloadString(URL).Replace("\r", "").Replace("\n", "");
        HtmlAgilityPack.HtmlDocument ad = new HtmlAgilityPack.HtmlDocument();
        ad.LoadHtml(webPageString);

        Hashtable oneEvent = new Hashtable();

        string date = "";
        string header = "";

        IEnumerable<HtmlAgilityPack.HtmlNode> headerNodes = ad.DocumentNode.SelectNodes("//h2");

        foreach (HtmlAgilityPack.HtmlNode headerNode in headerNodes)
        {
            header = headerNode.InnerText;
            break;
        }

        
        DateTime DateTimeStart = new DateTime();
        DateTime DateTimeEnd = new DateTime();
        bool isEnd = false;
        GetStartAndEndDate(out DateTimeStart, out DateTimeEnd, ref header, out isEnd);

        oneEvent.Add("header", header);
        oneEvent.Add("startTime", DateTimeStart);
        oneEvent.Add("endTime", DateTimeEnd);


        IEnumerable<HtmlAgilityPack.HtmlNode> pNodes = ad.DocumentNode.SelectNodes("//div[@id='userbody']");

        string description = "";

        foreach (HtmlAgilityPack.HtmlNode node in pNodes)
        {
            description += node.OuterHtml;
            break;
        }

        oneEvent.Add("description", RemoveImages(description));


        IEnumerable<HtmlAgilityPack.HtmlNode> subNodes = ad.DocumentNode.SelectNodes("//img");
        string images = "";
        if (subNodes != null)
        {
            foreach (HtmlAgilityPack.HtmlNode subNode in subNodes)
            {
                if (subNode.Name.ToLower() == "img")
                    images += subNode.Attributes["src"].Value + ";";
            }
        }
        oneEvent.Add("images", images);

        //parse description for categories
        DataView dvCats = GetDataDV("SELECT * FROM EventCategories");
        string categories = "";
        char[] delim = { '/' };
        char[] delimSpace = { ' ' };
        string[] tokens;
        foreach (DataRowView row in dvCats)
        {
            if (description.Contains(row["Name"].ToString()))
                categories += ";" + row["ID"].ToString() + ";";
            else if (row["Name"].ToString()[row["Name"].ToString().Length - 1].ToString().ToLower() == "s")
            {
                if (description.Contains(row["Name"].ToString().Substring(0, row["Name"].ToString().Length - 1)))
                    categories += ";" + row["ID"].ToString() + ";";
            }
            else if (row["Name"].ToString().ToLower().Contains("/"))
            {
                tokens = row["Name"].ToString().ToLower().Split(delim);

                if (description.Contains(tokens[0]))
                    categories += ";" + row["ID"].ToString() + ";";
                else if (description.Contains(tokens[1]))
                    categories += ";" + row["ID"].ToString() + ";";
            }
            else if (row["Name"].ToString().Trim().Contains(" "))
            {
                tokens = row["Name"].ToString().Trim().ToLower().Split(delimSpace);

                if (description.Contains(tokens[0]) && tokens[0] != "event")
                    categories += ";" + row["ID"].ToString() + ";";
                else if (description.Contains(tokens[1]) && tokens[1] != "event")
                    categories += ";" + row["ID"].ToString() + ";";
            }
        }

        oneEvent.Add("categories", categories);

        return oneEvent;
    }

    public void GetStartAndEndDate(out DateTime DateTimeStart, out DateTime DateTimeEnd, ref string header, out bool isEnd)
    {
        string dateIt = "";
        int index = 0;
        char chr = header[index];
        while (chr != ':')
        {
            dateIt += chr;
            index++;
            chr = header[index];
        }
        header = header.Replace(dateIt + ": ", "");
        char[] delim = { '-' };
        string[] tokens = dateIt.Split(delim);
        DateTimeStart = DateTime.Parse(tokens[0]);
        DateTimeEnd = new DateTime();
        isEnd = false;
        if (tokens.Length > 1)
        {
            isEnd = true;
            DateTimeEnd = DateTime.Parse(tokens[1]);
        }
    }

    public bool isItAURL(string supposedURL)
    {
        return Regex.IsMatch(supposedURL, "^(?#Protocol)(?:(?:ht|f)tp(?:s?)\\:\\/\\/|~/|/)?(?#Username:Password)(?:\\w+:\\w+@)?(?#Subdomains)(?:(?:[-\\w]+\\.)+(?#TopLevel Domains)(?:com|org|net|gov|mil|biz|info|mobi|name|aero|jobs|museum|travel|[a-z]{2}))(?#Port)(?::[\\d]{1,5})?(?#Directories)(?:(?:(?:/(?:[-\\w~!$+|.,=]|%[a-f\\d]{2})+)+|/)+|\\?|#)?(?#Query)(?:(?:\\?(?:[-\\w~!$+|.,*:]|%[a-f\\d{2}])+=(?:[-\\w~!$+|.,*:=]|%[a-f\\d]{2})*)(?:&(?:[-\\w~!$+|.,*:]|%[a-f\\d{2}])+=(?:[-\\w~!$+|.,*:=]|%[a-f\\d]{2})*)*)*(?#Anchor)(?:#(?:[-\\w~!$+|.,*:=]|%[a-f\\d]{2})*)?$");
    }
}
