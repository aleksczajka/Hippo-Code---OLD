// JScript File
//Code from: http://michaelsync.net/2006/06/14/javascript-working-with-database
//DECIDED NOT TO USE THIS CODE SINCE NOT ALL BROWSERS SUPPORT SQL DATABASE ACCESS FROM JAVASCRIPT
var conn_str = "Data Source=HippoHappenings.db.3781715.hostedresource.com; Initial Catalog=HippoHappenings; UID=HippoHappenings; PWD=DILU4ireEMF;";

//function getAdoDb(strAdoType)
//{
//    if (window.ActiveXObject)
//    {
//        return new ActiveXObject(strAdoType);
//    }
//    else
//    {
//        return new ActiveXObject(strAdoType);
//    }
//}

function IncrementView()
{
    try
    {
        
        var userID = document.getElementById("userIDDiv").innerText;
        
        //Run the algorithm only if user is logged in
        if(userID != "No User")
        {
            //Database Connection
            var conn = Server.CreateObject("ADODB.Connection");
            conn.open(conn_str, "", "");

            //Recordset
            //var rs = new ActiveXObject("ADODB.Recordset");
            
            //Get the next 4 adIDs to update
            strQuery = "SELECT AdSet, LastSeenAd FROM UserAds WHERE UserID="+userID;
            var rs = oConn.Execute(strSQL);
            
            var lastSeenAd = "";
            var AdSet = "";
            var AdID1 = "";
            var AdID2 = "";
            var AdID3 = "";
            var AdID4 = "";
            if(!rs.bof)
            {
                rs.MoveFirst();
                if(!rs.eof) 
                {
                    AdSet = rs.fields(0).value;
                    lastSeenAd = rs.fields(1).value;
                    
                    var tokens = AdSet.split(";");
                    var a1 = 0;
                    var a2 = 0;
                    var a3 = 0;
                    var a4 = 0;
                    for(i = 0; i < tokens.length; i++)
                    {
                        if(tokens[i] == lastSeenAd)
                        {
                            var a = tokens.length - (i+1);
                            
                            if(a < 4)
                            {
                                if(a == 0)
                                {
                                     a1 = 0; 
                                     a2 = 1;
                                     a3 = 2;
                                     a4 = 3;  
                                }else if(a == 1)
                                {
                                     a1 = i+1; 
                                     a2 = 0;
                                     a3 = 1;
                                     a4 = 2;
                                }else if(a == 2)
                                {
                                     a1 = i+1; 
                                     a2 = i+2;
                                     a3 = 0;
                                     a4 = 1;
                                }else if(a == 3)
                                {
                                     a1 = i+1; 
                                     a2 = i+2;
                                     a3 = i+3;
                                     a4 = 0;
                                }
                            }
                            else
                            {
                                 a1 = i + 1; 
                                 a2 = i + 2;
                                 a3 = i + 3;
                                 a4 = i + 4;  
                            }
                            
                            AdID1 = tokens[a1];
                            AdID2 = tokens[a2];
                            AdID3 = tokens[a3];
                            AdID4 = tokens[a4];
                        }
                    }
                }
            }
            
            
            ExecuteAdAlgorithm(userID, AdID1);
        }
    }
    catch(ex)
    {
        alert(ex.message);
    }
}

function ExecuteAdAlgorithm(userID, AdID)
{
        //Database Connection
        var conn = getAdoDb("ADODB.Connection");
        conn.open(conn_str, "", "");

        //Recordset
        var rs = getAdoDb("ADODB.Recordset");
        //determine whether the ad has been seen
        
        
        strQuery = "SELECT * FROM AdStatistics WHERE UserID=" +
            userID + " AND AdID=" + AdID;
        rs.open(strQuery, conn, adOpenDynamic, adLockOptimistic);

        var updateStatistics = true;

        if(!rs.bof)
        {
            updateStatistics = false;
        }

        //update the last see ad as this ad 
        strQuery = "UPDATE UserAds SET LastSeenAd=" + AdID + " WHERE UserID=" + userID +
            " AND BigAd='False'";
        rs.open(strQuery, conn, adOpenDynamic, adLockOptimistic);
        
        //Update AdStatistics
        //The Reason column is composed of categoryIDs dilimeted by a semi colon.
        if (updateStatistics)
        {
            //Get the data for Reason column and LocationOnly column to AdStatistics table
            strQuery = "SELECT CategoryID FROM Ad_Category_Mapping ACM, UserCategories UC " +
                "WHERE ACM.AdID=" + AdID + " AND ACM.CategoryID=UC.CategoryID AND UC.User_ID=" +
                userID;
            rs.open(strQuery, conn, adOpenDynamic, adLockOptimistic);
            
            var strCategories = "";
            
            if(!rs.bof)
            {
                rs.MoveFirst();
                while(!rs.eof) 
                {
                    strCategories += rs.fields(0).value + ";";
                    rs.MoveNext();
                }
            }

            var locationOnly = "True";
            if (strCategories == "")
            {
                locationOnly = "False";
            }
            
            strQuery = "INSERT INTO AdStatistics (UserID, Date, AdID, Reason, LocationOnly) " +
                "VALUES(" + userID + ", '" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Date.ToShortDateString() + "', " +
                AdID + ", '" + strCategories + "', '" + locationOnly + "')";
            rs.open(strQuery, conn, adOpenDynamic, adLockOptimistic);
            
            //Increment number of user to see this ad by one.
            strQuery = "UPDATE Ads SET NumCurrentViews= NumCurrentViews + 1 WHERE Ad_ID=" + AdID;
        }
}