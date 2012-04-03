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

public partial class MessageAlert : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        

        HttpCookie cookie = Request.Cookies["BrowserDate"];

        bool isCl = false;
        if (Request.QueryString["cl"] != null)
            if (bool.Parse(Request.QueryString["cl"]))
            {
                isCl = true;
            }

        string command = "";

        try
        {
            if (cookie != null)
            {
                HtmlMeta hm = new HtmlMeta();
                HtmlHead head = (HtmlHead)Page.Header;
                hm.Name = "ROBOTS";
                hm.Content = "NOINDEX, FOLLOW";
                head.Controls.AddAt(0, hm);

                HtmlLink lk = new HtmlLink();
                lk.Attributes.Add("rel", "canonical");
                lk.Href = "http://hippohappenings.com/MessageAlert.aspx";
                head.Controls.AddAt(0, lk);
                MessageLabel2.Text = "";

                SendItButton.SERVER_CLICK += ServerSendMessage;

                if (!IsPostBack)
                {
                    Session["ButtonClicked"] = null;
                    Session["CheckUsers"] = null;
                    Session["AllFriends"] = null;

                    Session.Remove("ButtonClicked");
                    Session.Remove("CheckUsers");
                    Session.Remove("AllFriends");
                }

                Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

                if (Session["User"] != null)
                {
                    TechLiteral.Text = "";
                    DataSet dsFriends = dat.GetData("SELECT * FROM User_Friends UF, Users U WHERE UF.FriendID=U.User_ID AND UF.UserID=" + Session["User"].ToString());
                    bool getFriendsMessage = false;
                    if (dsFriends.Tables.Count > 0)
                        if (dsFriends.Tables[0].Rows.Count > 0)
                        {
                            Session["AllFriends"] = "";
                            ImageButton theImage;
                            string eventID = "";
                            string type = "";

                            if (isCl)
                            {
                                type = "cl";
                                eventID = Request.QueryString["EID"];
                            }
                            else
                            {
                                if (Request.QueryString["ID"] == null)
                                {
                                    if (Request.QueryString["EID"] == null)
                                    {
                                        type = "a";
                                        eventID = Request.QueryString["AID"];
                                    }
                                    else
                                    {
                                        type = "e";
                                        eventID = Request.QueryString["EID"];
                                    }
                                }
                                else
                                {
                                    type = Request.QueryString["A"].ToString();
                                    eventID = Request.QueryString["ID"].ToString();
                                }
                            }

                            for (int i = 0; i < dsFriends.Tables[0].Rows.Count; i++)
                            {
                                theImage = new ImageButton();
                                theImage.Width = 50;
                                theImage.Height = 50;
                                theImage.Style.Add("padding", "5px");
                                theImage.AlternateText = dsFriends.Tables[0].Rows[i]["UserName"].ToString();
                                theImage.ToolTip = dsFriends.Tables[0].Rows[i]["UserName"].ToString();
                                theImage.Click += new ImageClickEventHandler(AddFriendPanel);
                                theImage.CommandArgument = dsFriends.Tables[0].Rows[i]["UserName"].ToString();

                                Session["AllFriends"] += dsFriends.Tables[0].Rows[i]["UserName"].ToString() + ";";

                                if (System.IO.File.Exists(Server.MapPath(".") + "\\UserFiles\\" + dsFriends.Tables[0].Rows[i]["UserName"].ToString() + "\\Profile\\" + dsFriends.Tables[0].Rows[i]["ProfilePicture"].ToString()))
                                {
                                    theImage.ImageUrl += "UserFiles/" + dsFriends.Tables[0].Rows[i]["UserName"].ToString() + "/Profile/" + dsFriends.Tables[0].Rows[i]["ProfilePicture"].ToString();
                                }
                                else
                                {
                                    theImage.ImageUrl = "~/NewImages/noAvatar_50x50_small.png";
                                    theImage.Attributes.Add("onmouseover", "this.src='NewImages/noAvatar_50x50_smallhover.png'");
                                    theImage.Attributes.Add("onmouseout", "this.src='NewImages/noAvatar_50x50_small.png'");
                                }

                                FriendsPanel.Controls.Add(theImage);
                            }

                            DataSet dsUser = dat.GetData("SELECT * FROM Users WHERE User_ID=" + Session["User"].ToString());

                            DataSet dsEvent = new DataSet();

                            if (type == "a")
                            {
                                dsEvent = dat.GetData("SELECT * FROM Ads WHERE Ad_ID=" + eventID);
                                Session["MessageBody"] = "<label>Ad: <a class=\"AddLink\" href=\"http://hippohappenings.com/" + dat.MakeNiceName(dsEvent.Tables[0].Rows[0]["Header"].ToString())
                                    + "_" + eventID + "_Ad\">" + dsEvent.Tables[0].Rows[0]["Header"].ToString() + "</a>" +

                                " \nDescription: " + dsEvent.Tables[0].Rows[0]["Description"].ToString() + "</label>";


                                Session["EventName"] = dsEvent.Tables[0].Rows[0]["Header"].ToString();
                            }
                            else if (type == "cl")
                            {
                                string desc = Session["ClDescription"].ToString();
                                if (desc.Length > 100)
                                    desc = desc.Substring(0, 100) + "...";
                                Session["MessageBody"] = "<label>Event: <a class=\"AddLink\" href=\"http://hippohappenings.com/" +
                                    dat.MakeNiceName(Session["ClHeader"].ToString()) + "_CLHH" + eventID.Replace("/", "_") + "_ClEvent\">" +
                                    Session["ClHeader"].ToString() + "</a>" +
                                " \nDescription: " + desc + "</label>";

                                Session["EventName"] = Session["ClHeader"].ToString();
                            }
                            else if (type == "e")
                            {
                                dsEvent = dat.GetData("SELECT * FROM Events E, Event_Occurance EO, " +
                                    "Venues V WHERE E.ID=EO.EventID AND E.Venue=V.ID AND E.ID=" +
                                    eventID);
                                Session["MessageBody"] = "<label>Event: <a class=\"AddLink\" href=\"http://hippohappenings.com/" +
                                    dat.MakeNiceName(dsEvent.Tables[0].Rows[0]["Header"].ToString()) + "_" + eventID + "_Event\">" +
                                    dsEvent.Tables[0].Rows[0]["Header"].ToString() + "</a>" +
                                " \nVenue: " + dsEvent.Tables[0].Rows[0]["Name"].ToString() +
                                " \nDate: " + dsEvent.Tables[0].Rows[0]["DateTimeStart"].ToString() +
                                " \nDescription: " + dsEvent.Tables[0].Rows[0]["Content"].ToString() + "</label>";

                                Session["EventName"] = dsEvent.Tables[0].Rows[0]["Header"].ToString();
                            }
                            else if (type == "ge")
                            {
                                string reocurr = Request.QueryString["O"].ToString();
                                command = "SELECT * FROM GroupEvents GE, GroupEvent_Occurance GO WHERE GE.ID=" +
                                    eventID + " AND GE.ID=GO.GroupEventID";
                                dsEvent = dat.GetData(command);
                                Session["MessageBody"] = "<label>Group Event: <a class=\"AddLink\" " +
                                    "href=\"http://hippohappenings.com/" +
                                    dat.MakeNiceName(dsEvent.Tables[0].Rows[0]["Name"].ToString()) + "_" + reocurr +
                                    "_" +
                                    eventID + "_GroupEvent\">" +
                                    dsEvent.Tables[0].Rows[0]["Name"].ToString() + "</a>" +
                                " \nDate: " + dsEvent.Tables[0].Rows[0]["DateTimeStart"].ToString() +
                                " \nDescription: " + dsEvent.Tables[0].Rows[0]["Content"].ToString() + "</label>";

                                Session["EventName"] = dsEvent.Tables[0].Rows[0]["Name"].ToString();
                            }
                            else if (type == "g")
                            {
                                command = "SELECT * FROM Groups WHERE ID=" + eventID;
                                dsEvent = dat.GetData(command);
                                Session["MessageBody"] = "<label>Group: <a class=\"AddLink\" href=\"http://hippohappenings.com/" +
                                    dat.MakeNiceName(dsEvent.Tables[0].Rows[0]["Header"].ToString()) + "_" +
                                    eventID + "_Group\">" +
                                    dsEvent.Tables[0].Rows[0]["Header"].ToString() + "</a>" +
                                " \nDescription: " + dsEvent.Tables[0].Rows[0]["Content"].ToString() + "</label>";


                                Session["EventName"] = dsEvent.Tables[0].Rows[0]["Header"].ToString();
                            }
                            else if (type == "t")
                            {
                                command = "SELECT * FROM Trips WHERE ID=" + eventID;
                                dsEvent = dat.GetData(command);
                                Session["MessageBody"] = "<label>Trip: <a class=\"AddLink\" href=\"http://hippohappenings.com/" +
                                    dat.MakeNiceName(dsEvent.Tables[0].Rows[0]["Header"].ToString()) + "_" +
                                    eventID + "_Trip\">" +
                                    dsEvent.Tables[0].Rows[0]["Header"].ToString() + "</a>" +
                                " \nDescription: " + dsEvent.Tables[0].Rows[0]["Content"].ToString() + "</label>";


                                Session["EventName"] = dsEvent.Tables[0].Rows[0]["Header"].ToString();
                            }
                            else
                            {
                                dsEvent = dat.GetData("SELECT * FROM Venues WHERE ID=" + eventID);
                                Session["MessageBody"] = "<label>Venue: <a class=\"AddLink\" href=\"http://hippohappenings.com/" +
                                    dat.MakeNiceName(dsEvent.Tables[0].Rows[0]["Name"].ToString()) + "_" +
                                    eventID + "_Venue\">" + dsEvent.Tables[0].Rows[0]["Name"].ToString() + "</a>" +
                                " \nDescription: " + dsEvent.Tables[0].Rows[0]["Content"].ToString() + "</label>";

                                Session["EventName"] = dsEvent.Tables[0].Rows[0]["Name"].ToString();
                            }
                        }
                        else
                            getFriendsMessage = true;
                    else
                        getFriendsMessage = true;

                    if (getFriendsMessage)
                    {
                        Literal FriendLiteral = new Literal();
                        FriendLiteral.Text = "You do not have any friends listed. To add friends visit 'My Account'. To send information of this event to an email account please visit this event's page.";
                        FriendsPanel.Controls.Add(FriendLiteral);
                    }

                    CreateCheckUser();
                }
                else
                {
                    TechLiteral.Text = "<script type=\"text/javascript\">Search('login');</script>";
                }
            }
            else
            {
                TechLiteral.Text = "<script type=\"text/javascript\">Search('login');</script>";
            }

            
        }
        catch (Exception ex)
        {
            MessageLabel.Text += ex.ToString() + "<br/>" + command;
        }
    }

    protected void ServerSendMessage(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        try
        {
            bool isCl = false;
            if (Request.QueryString["cl"] != null)
                if (bool.Parse(Request.QueryString["cl"]))
                {
                    isCl = true;
                }

            bool sendEmail = false;
            bool sendText = false;
            string message = "";
            if (Session["ButtonClicked"] == null)
            {
                Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

                if (Session["CheckUsers"] != null)
                {
                    Session["ButtonClicked"] = "notnull";
                    char[] delim = { ';' };
                    string[] tokens = Session["CheckUsers"].ToString().Split(delim);
                    string temp = "";

                    string eventID = "";
                    string temp2 = "";
                    string temp3 = "";

                    if (Request.QueryString["A"] != null)
                    {
                        if (Request.QueryString["A"].ToString() == "v")
                        {
                            temp2 = "the venue";
                            temp3 = "a venue";
                        }
                        else if (Request.QueryString["A"].ToString() == "e" || isCl)
                        {
                            temp2 = "the event";
                            temp3 = "an event";
                        }
                        else if (Request.QueryString["A"].ToString() == "a")
                        {
                            temp2 = "the ad";
                            temp3 = "an ad";
                        }
                        else if (Request.QueryString["A"].ToString() == "ge")
                        {
                            temp2 = "the group event";
                            temp3 = "a group event";
                        }
                        else if (Request.QueryString["A"].ToString() == "g")
                        {
                            temp2 = "the group";
                            temp3 = "a group";
                        }
                        else if (Request.QueryString["A"].ToString() == "t")
                        {
                            temp2 = "the adventure";
                            temp3 = "an adventure";
                        }
                    }
                    else
                    {
                        temp2 = "the ad";
                        temp3 = "an ad";
                    }

                    MessageLabel.Text = "got here";
                    for (int i = 0; i < tokens.Length; i++)
                    {
                        if (tokens[i].Trim() != "")
                        {
                            if (!temp.Contains(tokens[i] + ";"))
                            {
                                DataSet dsUser = dat.GetData("SELECT * FROM Users U, UserPreferences UP WHERE UP.UserID=U.User_ID AND U.UserName='" + tokens[i] + "'");

                                SqlConnection conn = dat.GET_CONNECTED;
                                SqlCommand cmd = new SqlCommand("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, " +
                                    "To_UserID, Date, [Read], Mode, Live)"
                                    + " VALUES(@content, @subject, @fromID, @toID, @date, 'False', 0, 1)", conn);
                                cmd.Parameters.Add("@content", SqlDbType.Text).Value = MessageInput.Text + "<br/><br/>" +
                                    Session["MessageBody"].ToString();
                                cmd.Parameters.Add("@subject", SqlDbType.NVarChar).Value = Session["UserName"].ToString() + " wants you to check out " +
                                    temp2 + " " + Session["EventName"].ToString();
                                cmd.Parameters.Add("@toID", SqlDbType.Int).Value = int.Parse(dsUser.Tables[0].Rows[0]["User_ID"].ToString());
                                cmd.Parameters.Add("@fromID", SqlDbType.Int).Value = int.Parse(Session["User"].ToString());
                                cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
                                cmd.ExecuteNonQuery();
                                conn.Close();

                                try
                                {
                                    if (dsUser.Tables[0].Rows[0]["EmailPrefs"].ToString().Contains("9"))
                                    {
                                        sendEmail = true;
                                    }
                                    else
                                    {
                                        //check if prefs are set per individual
                                        DataView dvFirendsIndividual = dat.GetDataDV("SELECT * FROM UserFriendPrefs UFP " +
                                            "WHERE UFP.Preferences LIKE '%8%'AND UFP.UserID=" + dsUser.Tables[0].Rows[0]["User_ID"].ToString() + " AND UFP.FriendID=" + Session["User"].ToString());
                                        if (dvFirendsIndividual.Count > 0)
                                            sendEmail = true;
                                    }

                                    DataView dvF = dat.GetDataDV("SELECT * FROM Users WHERE User_ID=" + Session["User"].ToString());
                                    if (sendEmail)
                                    {
                                        dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                            System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                                            dsUser.Tables[0].Rows[0]["Email"].ToString(),
                                            "<div><br/>A new email arrived at your inbox on Hippo Happenings. <br/><br/> "+dvF[0]["UserName"].ToString() + " shared " + temp3 + " with you." +
                                              "<br/><br/>Message: " +
                                              MessageInput.Text + "<br/><br/>" + Session["MessageBody"].ToString() +
                                              "<br/><br/> To view the message, <a href=\"HippoHappenings.com/login\">" +
                                            "log into Hippo Happenings</a></div>", dvF[0]["UserName"].ToString() + " shared " + temp3 + " with you.");

                                    }

                                    //if (sendText)
                                    //{
                                    //    DataView dvUser = dat.GetDataDV("SELECT *, PP.Extension AS Ex1 FROM Users U, UserPreferences UP, PhoneProviders PP " +
                                    //        "WHERE U.User_ID=UP.UserID AND U.PhoneProvider=PP.ID AND U.User_ID=" + dsUser.Tables[0].Rows[0]["User_ID"].ToString());
                                    //    if (dvUser[0]["PhoneNumber"].ToString().Trim() != "")
                                    //    {
                                    //        string txtmessage = MessageInput.Text;
                                    //        if (txtmessage.Length > 118)
                                    //            txtmessage = MessageInput.Text.Substring(0, 118);

                                    //        txtmessage += " Login to HippoHappenings.com to read more.";
                                            
                                    //        dat.SendText(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                    //            System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                                    //            dat.MakeGoodPhone(dvUser[0]["PhoneNumber"].ToString()) +
                                    //            dvUser[0]["Ex1"].ToString(),
                                    //           txtmessage, dvF[0]["UserName"].ToString() + " shared " + temp3 + " with you.");
                                    //    }
                                    //}
                                }
                                catch (Exception ex)
                                {
                                    MessageLabel.Text += ex.ToString();
                                }


                                temp += tokens[i] + ";";
                            }
                        }
                    }



                    if (message != "")
                    {
                        MessageLabel2.Text = "The following users have emails that are not in a correct format. You might want to notify your friends as many of their settings will not work correctly without a correct email address. A message about your event was still sent to their Hippo Mail box: ";
                    }
                    else
                    {
                        MessageLabel2.Text = "Your messages have been sent.";
                        ThankYouPanel.Visible = true;
                        MessagePanel.Visible = false;

                        Session["ButtonClicked"] = null;
                        Session["CheckUsers"] = null;
                        Session["AllFriends"] = null;

                        Session.Remove("ButtonClicked");
                        Session.Remove("CheckUsers");
                        Session.Remove("AllFriends");
                    }
                }
                else
                {
                    MessageLabel2.Text = "Please choose your friends to contact.";
                }
            }
            else
            {

                if (Session["CheckUsers"] != null)
                {
                    if (message != "")
                    {
                        MessageLabel2.Text = "The following users have emails that are not in a correct format. You might want to notify your friends as many of their settings will not work correctly without a correct email address. A message about your event was still sent to their Hippo Mail box: ";
                    }
                    else
                    {
                        MessageLabel2.Text = "Your messages have been sent.";
                        ThankYouPanel.Visible = true;
                        MessagePanel.Visible = false;

                        Session["ButtonClicked"] = null;
                        Session["CheckUsers"] = null;
                        Session["AllFriends"] = null;

                        Session.Remove("ButtonClicked");
                        Session.Remove("CheckUsers");
                        Session.Remove("AllFriends");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageLabel.Text = ex.ToString();
        }
    }

    protected void CreateCheckUser()
    {
        try
        {
            
            if (Session["CheckUsers"] != null)
            {
                
                char[] delim = { ';' };
                string[] tokens = Session["CheckUsers"].ToString().Split(delim);
                ChecksPanel.Controls.Clear();
                for (int i = 0; i < tokens.Length; i++)
                {
                    if (tokens[i].Trim() != "")
                    {
                        Label label = new Label();
                        label.Text = tokens[i];
                        label.CssClass = "NumLabel";
                        label.ID = "label" + tokens[i];
                        label.Width = 120;
                        label.Height = 30;
                        label.Style.Add("vertical-align", "middle");
                        label.Style.Add("padding-left", "5px");

                        if (!ChecksPanel.Controls.Contains(label))
                        {
                            ImageButton img2 = new ImageButton();
                            img2.ImageUrl = "image/DeleteCircle.png";
                            img2.CssClass = "FloatLeft";
                            img2.ID = "img" + tokens[i];
                            img2.CommandArgument = tokens[i];
                            img2.Click += new ImageClickEventHandler(RemoveUserCheck);
                            img2.AlternateText = "Remove Friend";
                            img2.ToolTip = "Remove Friend";

                            ChecksPanel.Controls.Add(img2);
                            ChecksPanel.Controls.Add(label);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageLabel.Text += ex.ToString();
        }
    }

    protected void SelectAllFriends(object sender, EventArgs e)
    {
        Session["CheckUsers"] = Session["AllFriends"];
        CreateCheckUser();
    }

    protected void AddFriendPanel(object sender, EventArgs e)
    {
        ImageButton img = (ImageButton)sender;


        AddOneFriend(img.CommandArgument);
    }

    protected void AddOneFriend(string UserName)
    {
        bool goOn = false;

        if (Session["CheckUsers"] != null)
        {
            if (!Session["CheckUsers"].ToString().Contains(UserName + ";"))
                goOn = true;
        }
        else
        {
            goOn = true;
        }

        if (goOn)
        {
            Session["CheckUsers"] += UserName + ";";

            Label label = new Label();
            label.Text = UserName;
            label.CssClass = "NumLabel";
            label.ID = "label" + UserName;
            label.Width = 120;
            label.Height = 30;
            label.Style.Add("vertical-align", "middle");
            label.Style.Add("padding-left", "5px");

            

            ImageButton img2 = new ImageButton();
            img2.ImageUrl = "image/DeleteCircle.png";
            img2.ID = "img" + UserName;
            img2.CommandArgument = UserName;
            img2.Click += new ImageClickEventHandler(RemoveUserCheck);
            img2.AlternateText = "Remove Friend";
            img2.ToolTip = "Remove Friend";

            ChecksPanel.Controls.Add(img2);
            ChecksPanel.Controls.Add(label);

        }
    }

    protected void RemoveUserCheck(object sender, EventArgs e)
    {

        ImageButton img = (ImageButton)sender;
        Session["CheckUsers"] = Session["CheckUsers"].ToString().Replace(img.CommandArgument + ";", "");



        Label label = (Label)ChecksPanel.FindControl("label" + img.CommandArgument);

        ChecksPanel.Controls.Remove(img);
        ChecksPanel.Controls.Remove(label);

        if (Session["CheckUsers"].ToString().Trim() == "")
        {
            Session["CheckUsers"] = null;
            Session.Remove("CheckUsers");
        }
    }

    public int SendIt(string msgText, string userName, string userID, string eventName, 
        string[] idArray, string msgBody)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet dsUser;
        for (int i = 0; i < idArray.Length; i++)
        {
            SqlConnection conn = dat.GET_CONNECTED;
            SqlCommand cmd = new SqlCommand("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, To_UserID, Date, [Read], Mode)"
                + " VALUES(@content, @subject, @fromID, @toID, @date, 'False', 0)", conn);
            cmd.Parameters.Add("@content", SqlDbType.Text).Value = msgText + "<br/><br/>"+msgBody;
            cmd.Parameters.Add("@subject", SqlDbType.NVarChar).Value = userName + " wants you to check out event " + eventName;
            cmd.Parameters.Add("@toID", SqlDbType.Int).Value = int.Parse(idArray[i]);
            cmd.Parameters.Add("@fromID", SqlDbType.Int).Value = int.Parse(userID);
            cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
            cmd.ExecuteNonQuery();
            conn.Close();

            dsUser = dat.GetData("SELECT * FROM Users WHERE User_ID=" + idArray[i]);
            dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                dsUser.Tables[0].Rows[0]["Email"].ToString(),
                "<div><br/>A new email arrived at your inbox on Hippo Happenings. <br/><br/> Email Contents:<br/><br/>Subject: " +
                  userName + " wants you to check out event " + eventName + "<br/><br/>Message Body: " +
                  msgText + "<br/><br/>" + msgBody +
                  "<br/><br/> To view the message, <a href=\"HippoHappenings.com/login\">" +
                "log into Hippo Happenings</a></div>", "You have a new email at Hippo Happenings!");

            MessageLabel2.Text = "Your message has been sent.";
            ThankYouPanel.Visible = true;
            MessagePanel.Visible = false;
        }
        return 0;
    }
}
