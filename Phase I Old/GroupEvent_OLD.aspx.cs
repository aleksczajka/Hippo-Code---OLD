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
using System.Diagnostics;
using Telerik.Web.UI;
using System.Data.SqlClient;

public partial class GroupEvent : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
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
        string command = "";
        try
        {
            DataView dvGroup = dat.GetDataDV("SELECT * FROM GroupEvents WHERE ID=" + Request.QueryString["ID"].ToString());
            DataView dvRealGroup = dat.GetDataDV("SELECT * FROM Groups WHERE ID=" + 
                dvGroup[0]["GroupID"].ToString());

            string groupID = dvRealGroup[0]["ID"].ToString();

            command = "SELECT * FROM GroupEvent_Occurance WHERE ID=" + Request.QueryString["O"].ToString();
            DataView dvEvent = dat.GetDataDV(command);

            if (dvEvent.Count == 0)
            {
                command = "SELECT * FROM GroupEvent_Occurance WHERE GroupEventID=" +
                    Request.QueryString["ID"].ToString();
                dvEvent = dat.GetDataDV(command);
                if (dvEvent.Count > 0)
                {
                    Response.Redirect(dat.MakeNiceName(dvGroup[0]["Name"].ToString()) + "_" + dvEvent[0]["ID"].ToString() + "_" + Request.QueryString["ID"].ToString() + "_GroupEvent");
                }
                else
                {
                    Response.Redirect(dat.MakeNiceName(dvRealGroup[0]["Name"].ToString()) +
                        "_" + dvRealGroup[0]["ID"].ToString() + "_Group");
                }
            }

            DateAndTimeLabel.Text = dvEvent[0]["DateTimeStart"].ToString() + " -- " + dvEvent[0]["DateTimeEnd"].ToString();

            GroupLabel.Text = dvRealGroup[0]["Header"].ToString();
            GroupLabel.NavigateUrl = dat.MakeNiceName(dvRealGroup[0]["Header"].ToString()) + "_" + dvRealGroup[0]["ID"].ToString() + "_Group";
            Page.Title = dvGroup[0]["Name"].ToString() + " | Hippo Group Event";

            if (!IsPostBack)
            {
                Session["RedirectTo"] = Request.Url.AbsoluteUri;
            }

            string country = dat.GetDataDV("SELECT * FROM Countries WHERE country_id=" + dvEvent[0]["Country"].ToString())[0]["country_name"].ToString();

            Literal liter = new Literal();
            liter.Text = "<link type=\"text/css\" href=\"Rads.css\" rel=\"stylesheet\" />";
            Page.Header.Controls.Add(liter);

            bool isMember = false;
            #region Take care of user types
            if (Session["User"] != null)
            {
                #region Take care of Share with friends and Share though email and Flag
                LoggedInPanel.Visible = true;

                ShareFriends.THE_TEXT = "Share this with a friend";
                ShareFriends.RE_LABEL = "Re: " + Session["UserName"].ToString() +
                    " would like to share the group event '\"" + dat.BreakUpString(dvGroup[0]["Name"].ToString(), 14) +
                    "\"' with you.";
                ShareFriends.TYPE = "ge";
                ShareFriends.ID = int.Parse(Request.QueryString["ID"].ToString());

                string descrip = dvGroup[0]["Content"].ToString();
                if (descrip.Length > 200)
                    descrip = descrip.Substring(0, 200) + "...";

                Session["messageEmail"] = "Group Event Name: <a href=\"http://hippohappenings.com/" +
                    dat.MakeNiceName(dat.BreakUpString(dvGroup[0]["Name"].ToString(), 14)) + "_" +
                    Request.QueryString["O"].ToString() + "_" +
                    Request.QueryString["ID"].ToString() + "_GroupEvent\">" +
                            dat.BreakUpString(dvGroup[0]["Name"].ToString(), 14) + "</a><br/><br/> " +
                            descrip;
                Session["FlagID"] = Request.QueryString["ID"].ToString();
                Session["FlagType"] = "GE";
                Session["ReOccurID"] = Request.QueryString["O"].ToString();

                DataView dvMember = dat.GetDataDV("SELECT * FROM Group_Members WHERE GroupID=" +
                    dvGroup[0]["GroupID"].ToString() + " AND MemberID=" + Session["User"].ToString());
                PostMessageID.Attributes.Add("onclick", "OpenMess('" + Request.QueryString["ID"].ToString() +
                    "', '" + Request.QueryString["O"].ToString() + "')");
                #endregion

                switch (dvGroup[0]["EventType"].ToString())
                {
                    case "1":
                        PrivateLabel.Text = "Public Event";
                        break;
                    case "2":
                        PrivateLabel.Text = "Private Event";
                        break;
                    case "3":
                        PrivateLabel.Text = "Exclusive Event";
                        break;
                    default: break;
                }

                DataView dvGoingMember = dat.GetDataDV("SELECT * FROM GroupEvent_Members WHERE GroupEventID=" +
                        Request.QueryString["ID"].ToString() + " AND Accepted='True' AND ReoccurrID=" + 
                        Request.QueryString["O"].ToString());
                dvGoingMember.RowFilter = "UserID=" + Session["User"].ToString();


                //if it's a public event
                if(dvGroup[0]["EventType"].ToString() == "1")
                    dvGoingMember = dat.GetDataDV("SELECT * FROM User_GroupEvent_Calendar WHERE GroupEventID=" +
                        Request.QueryString["ID"].ToString() + " AND ReoccurrID=" +
                        Request.QueryString["O"].ToString());

                dvGoingMember.RowFilter = "UserID=" + Session["User"].ToString();

                AreGoingPanel.Visible = false;
                RegistrationEndedPanel.Visible = false;
                GoingPanel.Visible = true;

                if (dvGoingMember.Count > 0)
                {
                    AreGoingPanel.Visible = true;
                    RegistrationEndedPanel.Visible = false;
                    GoingPanel.Visible = false;
                }
                else
                {
                    dvGoingMember.RowFilter = "";
                    if (dvGroup[0]["RegType"].ToString() == "2")
                    {
                        if (dvGroup[0]["RegNum"] != null)
                        {
                            if (dvGroup[0]["RegNum"].ToString().Trim() != "")
                            {
                                if (dvGoingMember.Count >= int.Parse(dvGroup[0]["RegNum"].ToString()))
                                {
                                    AreGoingPanel.Visible = false;
                                    RegistrationEndedPanel.Visible = true;
                                    GoingPanel.Visible = false;
                                }
                                else
                                {
                                    AreGoingPanel.Visible = false;
                                    RegistrationEndedPanel.Visible = false;
                                    GoingPanel.Visible = true;
                                }
                            } 
                            
                            if (dvGroup[0]["RegDeadline"] != null)
                            {
                                if (dvGroup[0]["RegDeadline"].ToString().Trim() != "")
                                {
                                    if (DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")) >
                                        DateTime.Parse(dvGroup[0]["RegDeadline"].ToString()))
                                    {
                                        AreGoingPanel.Visible = false;
                                        RegistrationEndedPanel.Visible = true;
                                        GoingPanel.Visible = false;
                                    }
                                }
                            }
                        }

                        if (dvGroup[0]["RegDeadline"] != null)
                        {
                            if (dvGroup[0]["RegDeadline"].ToString().Trim() != "")
                            {
                                if (DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")) >
                                    DateTime.Parse(dvGroup[0]["RegDeadline"].ToString()))
                                {
                                    AreGoingPanel.Visible = false;
                                    RegistrationEndedPanel.Visible = true;
                                    GoingPanel.Visible = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        DataView dvInvitedMember = dat.GetDataDV("SELECT * FROM GroupEvent_Members WHERE GroupEventID=" +
                        Request.QueryString["ID"].ToString() + " AND ReoccurrID=" +Request.QueryString["O"].ToString());

                        DataView dvMember2 = dat.GetDataDV("SELECT * FROM Group_Members WHERE GroupID=" +
                            groupID + " AND MemberID=" + Session["User"].ToString());

                        if (dvGroup[0]["EventType"].ToString() == "2")
                        {
                            if (dvMember2.Count > 0)
                            {
                                AreGoingPanel.Visible = false;
                                RegistrationEndedPanel.Visible = false;
                                GoingPanel.Visible = true;
                            }
                            else
                            {
                                AreGoingPanel.Visible = false;
                                RegistrationEndedPanel.Visible = false;
                                GoingPanel.Visible = false;
                            }
                        }
                        else if (dvGroup[0]["EventType"].ToString() == "3")
                        {
                            dvInvitedMember.RowFilter = "UserID=" + Session["User"].ToString();
                            if (dvInvitedMember.Count > 0)
                            {
                                AreGoingPanel.Visible = false;
                                RegistrationEndedPanel.Visible = false;
                                GoingPanel.Visible = true;
                            }
                            else
                            {
                                AreGoingPanel.Visible = false;
                                RegistrationEndedPanel.Visible = false;
                                GoingPanel.Visible = false;
                            }
                        }
                        else
                        {
                            AreGoingPanel.Visible = false;
                            RegistrationEndedPanel.Visible = false;
                            GoingPanel.Visible = true;
                        }
                        
                    }
                }
                
                if (dvMember.Count > 0)
                {
                    isMember = true;
                    if (bool.Parse(dvMember[0]["SharedHosting"].ToString()))
                    {
                        Session["isHost"] = true;
                        HostHeaderPanel.Visible = true;
                        MemberHeaderPanel.Visible = false;
                        //if is host, take care of host options
                        LinkButton1.Attributes.Add("onclick", "window.location='EnterGroupEvent.aspx?ID=" + Request.QueryString["ID"].ToString() + "&GroupID=" + dvGroup[0]["GroupID"].ToString() + "&O=" + Request.QueryString["O"].ToString() + "';");
                        LinkButton2.Attributes.Add("onclick", "window.location='EnterGroupEvent.aspx?copy=true&ID=" + Request.QueryString["ID"].ToString()  + "&GroupID=" + dvGroup[0]["GroupID"].ToString() + "';");
                        Label1.Attributes.Add("onclick", "OpenSendMess('" + Request.QueryString["ID"].ToString() + "')");
                        Label3.Attributes.Add("onclick", "OpenParticipants('" + Request.QueryString["ID"].ToString() +
                            "', '" + Request.QueryString["O"].ToString() + "')");
                        Label2.Attributes.Add("onclick", "OpenGroupEventDelete('" + Request.QueryString["ID"].ToString() +
                            "', '" + Request.QueryString["O"].ToString() + "')");
                    }
                    else
                    {
                        Session["isHost"] = false;
                        Session["isMember"] = true;
                        HostHeaderPanel.Visible = false;
                        MemberHeaderPanel.Visible = true;
                    }
                    PostMessagePanel.Visible = true;
                }
                else
                {
                    Session["isHost"] = false;
                    Session["isMember"] = false;
                    HostHeaderPanel.Visible = false;
                    MemberHeaderPanel.Visible = false;
                    PostMessagePanel.Visible = false;
                }
            }
            else
            {
                Session["isHost"] = false;
                Session["isMember"] = false;
                HostHeaderPanel.Visible = false;
                MemberHeaderPanel.Visible = false;
                PostMessagePanel.Visible = false;
            }
            #endregion

            #region Take care of map
            HttpCookie cookie2 = new HttpCookie("addressParameterName");
            cookie2.Value = dvGroup[0]["Name"].ToString();
            cookie2.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie2);
            bool isVenue = false;
            string address = "";

            if (dvEvent[0]["VenueID"] != null)
            {
                if (dvEvent[0]["VenueID"].ToString().Trim() != "")
                {
                    DataView dvVenue = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + dvEvent[0]["VenueID"].ToString());
                    NameOfPlaceLabel.Text = dvVenue[0]["Name"].ToString() + "<br />";
                    address = dat.GetAddress(dvVenue[0]["Address"].ToString(), dvVenue[0]["Country"].ToString() != "223");
                    isVenue = true;

                    Address1Label.Text = address;

                    Address2Label.Visible = false;

                    CityStateZipLabel.Text = dvVenue[0]["City"].ToString() + ", " +
                    dvVenue[0]["State"].ToString() + " " + dvVenue[0]["Zip"].ToString();
                }
                else
                    NameOfPlaceLabel.Visible = false;

            }
            else
                NameOfPlaceLabel.Visible = false;

            if (!isVenue)
            {
                if (dvEvent[0]["Country"].ToString() == "223")
                {
                    address = dvEvent[0]["StreetNumber"].ToString() + " " + dvEvent[0]["StreetName"].ToString() +
                        " " + dvEvent[0]["StreetDrop"].ToString();
                }
                else
                {
                    address = dvEvent[0]["Location"].ToString();
                }
            }

            cookie2 = new HttpCookie("addressParameter");
            cookie2.Value = address + " " + dvEvent[0]["City"].ToString() + " " + dvEvent[0]["State"].ToString() +
                " " + dvEvent[0]["Zip"].ToString() + " " + country;
            cookie2.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie2);


            Literal lit = new Literal();
            lit.Text = "<script src=\"http://maps.google.com/maps?file=api&amp;v=2&amp;sensor=false&amp;key=ABQIAAAAjjoxQtYNtdn3Tc17U5-jbBR2Kk_H7gXZZZniNQ8L14X1BLzkNhQjgZq1k-Pxm8FxVhUy3rfc6L9O4g\" type=\"text/javascript\"></script>";
            Page.Header.Controls.Add(lit);

            Master.BodyTag.Attributes.Add("onload", "initialize();");
            Master.BodyTag.Attributes.Add("onunload", "GUnload()");


            if (!isVenue)
            {
                if (dvEvent[0]["Country"].ToString() == "223")
                    Address1Label.Text = dvEvent[0]["StreetNumber"].ToString() + " " +
                        dvEvent[0]["StreetName"].ToString() + " " + dvEvent[0]["StreetDrop"].ToString();
                else
                    Address1Label.Text = dvEvent[0]["Location"].ToString();

                if (dvEvent[0]["AptName"].ToString().Trim() != "")
                {
                    Address2Label.Text = dvEvent[0]["AptName"].ToString() + " " +
                        dvEvent[0]["AptNo"].ToString() + "<br/>";
                }
                else
                    Address2Label.Visible = false;

                CityStateZipLabel.Text = dvEvent[0]["City"].ToString() + ", " +
                    dvEvent[0]["State"].ToString() + " " + dvEvent[0]["Zip"].ToString();
            }

            #endregion

            #region Members
            string members = "";
            string friendImg = "";
            string strFill = "";
            string title = "";
            string description = "";
            string mem = "";
            FillMembers();
            #endregion

            #region Stuff we need
            DataView dvStuff = dat.GetDataDV("SELECT * FROM GroupEvent_StuffNeeded WHERE GroupEventID=" + 
                Request.QueryString["ID"].ToString());
            if (dvStuff.Count > 0)
                StuffsPanel.Visible = true;
            CheckBox check;
            DataView dvUserID;
            bool userNotGrabbed = false;
            foreach (DataRowView row in dvStuff)
            {
                userNotGrabbed = false;
                if (row["UserID"] == null)
                {
                    userNotGrabbed = true;
                }
                else
                {
                    if (row["UserID"].ToString() == "")
                        userNotGrabbed = true;
                    else
                    {
                        if (isMember)
                        {
                            dvUserID = dat.GetDataDV("SELECT * FROM Users WHERE User_ID=" + row["UserID"].ToString());
                            string addthis = "";
                            if (row["UserID"].ToString() == Session["User"].ToString())
                            {
                                addthis = "<div style=\"float: left;\"><img style=\"cursor: pointer;\" name=\"remove the user\" src=\"image/X.png\" onclick=\"RemoveStuffUser('" + row["ID"].ToString() +
                                "');\" onmouseout=\"this.src = 'image/X.png';\" " +
                                "onmouseover=\"this.src = 'image/XSelected.png';\" /></div>";
                            }
                            lit = new Literal();
                            lit.ID = "lit" + row["ID"].ToString();
                            lit.Text = "<div class=\"topDiv\" width=\"400px\"><div style=\"float: left;\"><ul style=\"font-weight: bold;padding: 0; margin: 0; color: white;padding-left: 22px;\"><li><span style=\"color: white; font-size: 14px;\">" + row["StuffNeeded"].ToString() +
                                " -- " + dvUserID[0]["UserName"].ToString() + "</span></li></ul></div>" + addthis + "</div>";
                            StuffWeNeedPanel.Controls.Add(lit);
                        }
                    }
                }

                if (isMember)
                {
                    if (userNotGrabbed)
                    {
                        check = new CheckBox();
                        check.ID = "check" + row["ID"].ToString();
                        check.Text = row["StuffNeeded"].ToString();
                        check.AutoPostBack = true;
                        check.CheckedChanged += new EventHandler(StuffGrabbed);
                        check.ForeColor = System.Drawing.Color.White;
                        check.Font.Size = 14;

                        lit = new Literal();
                        lit.Text = "<div width=\"400px\">";

                        StuffWeNeedPanel.Controls.Add(lit);
                        StuffWeNeedPanel.Controls.Add(check);

                        lit = new Literal();
                        lit.Text = "</div>";
                        StuffWeNeedPanel.Controls.Add(lit);
                    }
                }
                else
                {

                    lit = new Literal();
                    lit.Text = "<div width=\"400px\"><ul style=\"font-weight: bold;padding: 0; margin: 0; color: white;padding-left: 22px;\"><li><span style=\"color: white; font-size: 14px;\">" + row["StuffNeeded"].ToString() +
                        "</span></li></ul></div>";
                    StuffWeNeedPanel.Controls.Add(lit);
                }
            }
            #endregion

            DataView dvUser = dat.GetDataDV("SELECT * FROM Users WHERE User_ID=" + dvGroup[0]["UserID"].ToString());

            HostLabel.Text = dvUser[0]["UserName"].ToString();
            HostLabel.NavigateUrl = HostLabel.Text + "_Friend";
            EventName.Text = dvGroup[0]["Name"].ToString();

            #region Event Message Board
            DataView dvMessages = dat.GetDataDV("SELECT *, GM.ID AS MessageID FROM GroupEventMessages GM, Users U WHERE GM.GroupEventID=" +
                Request.QueryString["ID"].ToString() + " AND GM.UserID=U.User_ID");
            DataView dvSticky = dat.GetDataDV("SELECT *, GM.ID AS MessageID FROM GroupEventMessages GM, Group_Members M, Users U WHERE GM.GroupEventID=" +
                Request.QueryString["ID"].ToString() + " AND GM.UserID=U.User_ID and M.GroupID=" +
                dvGroup[0]["GroupID"].ToString() + " AND M.MemberID=U.User_ID AND M.SharedHosting='True' AND GM.isSticky='True'");
            members = "";
            friendImg = "";
            strFill = "";
            description = "";

            string stickyID = "";

            if (dvSticky.Count > 0)
            {
                dvSticky.Sort = "DatePosted DESC";

                stickyID = dvSticky[0]["MessageID"].ToString();

                if (System.IO.File.Exists(Server.MapPath(".") + "\\UserFiles\\" + dvSticky[0]["UserName"].ToString() +
                            "\\Profile\\" + dvSticky[0]["ProfilePicture"].ToString()))
                {
                    friendImg = "UserFiles/" + dvSticky[0]["UserName"].ToString() + "/Profile/" + dvSticky[0]["ProfilePicture"].ToString();
                    strFill = "";
                }
                else
                {
                    friendImg = "image/noAvatar_50x50_small.png";
                    strFill = "onmouseover=\"this.src='NewImages/noAvatar_50x50_smallhover.png'\"" +
                        "onmouseout=\"this.src='image/noAvatar_50x50_small.png'\" ";
                }
                string imgLit = "";
                if (Session["User"] != null)
                {
                    if (Session["User"].ToString() == dvSticky[0]["User_ID"].ToString())
                    {
                        imgLit = "<div style=\"float: right;\"><img style=\"cursor: pointer;\" name=\"remove stickyness\" src=\"image/X.png\" onclick=\"OpenRemoveSticky('" + Request.QueryString["ID"].ToString() +
                            "', '" + stickyID + "', '"+Request.QueryString["O"].ToString()+"');\" onmouseout=\"this.src = 'image/X.png';\" " +
                            "onmouseover=\"this.src = 'image/XSelected.png';\" /></div>";
                    }
                }

                members = imgLit + "<div style=\"border-bottom: solid 1px #1b1b1b;float: left; clear: both; padding-left: 3px; padding-top: 5px;\"><a target='_blank' href=\"" + dvSticky[0]["UserName"].ToString() + "_Friend\"><img " + strFill +
                    " style=\" border: 0;float: left;padding-right: 7px; padding-bottom: 2px;\" " +
                                "src=\"" + friendImg + "\" width=\"50px\" height=\"50px\" /></a><a style=\"float: left;padding-bottom: 0px;\" href=\"" + dvSticky[0]["UserName"].ToString() + "_Friend\" class=\"AddWhiteLink\">" +
                                dvSticky[0]["UserName"].ToString() +
                                "</a><span class=\"MemberTitle\">&nbsp;-&nbsp;Posted On: " +
                                dvSticky[0]["DatePosted"].ToString().Trim() +
                                "</span> - <span style=\"color: #ff6b09; font-size: 30px;font: bold;\">!</span>" +
                                "<span class=\"MemberDescription\" style=\"color: white;\">&nbsp;&nbsp;" + dvSticky[0]["Content"].ToString() +
                                "</span><span style=\"font: bold;color: #ff6b09; font-size: 30px;\">!</span></div>";

                StickyMessageLiteral.Text = members;
            }

            members = "";

            if (stickyID != "")
            {
                dvMessages.RowFilter = "MessageID <> " + stickyID;
            }



            if (dvMessages.Count == 0)
            {
                lit = new Literal();
                lit.Text = "<div class=\"EventMemberDescription\">No messages have been posted for this group.</div>";
                MessagesPanel.Controls.Add(lit);
            }
            else
            {
                dvMessages.Sort = "DatePosted DESC";
                foreach (DataRowView row in dvMessages)
                {
                    if (System.IO.File.Exists(Server.MapPath(".") + "\\UserFiles\\" + row["UserName"].ToString() +
                                "\\Profile\\" + row["ProfilePicture"].ToString()))
                    {
                        friendImg = "UserFiles/" + row["UserName"].ToString() + "/Profile/" + row["ProfilePicture"].ToString();
                        strFill = "";
                    }
                    else
                    {
                        friendImg = "image/noAvatar_50x50_small.png";
                        strFill = "onmouseover=\"this.src='NewImages/noAvatar_50x50_smallhover.png'\"" +
                            "onmouseout=\"this.src='image/noAvatar_50x50_small.png'\" ";
                    }


                    description = "<span class=\"MemberDescription\" style=\"color: white;\">&nbsp;&nbsp;" + row["Content"].ToString().Trim() + "</span>";


                    members += "<div style=\"float: left; clear: both; padding-left: 3px; padding-top: 5px;\"><a target='_blank' href=\"" + row["UserName"].ToString() + "_Friend\"><img " + strFill +
                        " style=\" border: 0;float: left;padding-right: 7px; padding-bottom: 2px;\" " +
                                    "src=\"" + friendImg + "\" width=\"50px\" height=\"50px\" /></a><a style=\"float: left;padding-bottom: 0px;\" href=\"" + row["UserName"].ToString() + "_Friend\" class=\"AddWhiteLink\">" +
                                    row["UserName"].ToString() + "</a><span class=\"MemberTitle\">&nbsp;-&nbsp;Posted On: " + row["DatePosted"].ToString().Trim() + "</span> - " + description + "</div>";
                }

                lit = new Literal();
                lit.Text = members;
                MessagesPanel.Controls.Add(lit);
            }
            #endregion

            #region Take care of images and youtube
            char[] delim4 = { ';' };

            DataView dsSlider = dat.GetDataDV("SELECT * FROM GroupEvent_Slider_Mapping WHERE GroupEventID=" + Request.QueryString["ID"].ToString());
            if (dsSlider.Count > 0)
            {
                char[] delim = { '\\' };
                char[] delim3 = { '.' };


                for (int i = 0; i < dsSlider.Count; i++)
                {
                    string caption = "";
                    if (dsSlider[i]["Caption"] != null)
                    {
                        if (dsSlider[i]["Caption"].ToString().Trim() != "")
                        {
                            caption = "<div class=\"CaptionText\">" + dsSlider[i]["Caption"].ToString().Trim() + "</div>";
                        }
                    }
                    string[] tokens = dsSlider[i]["PictureName"].ToString().Split(delim3);

                    //dsSlider.RowFilter = "RealPictureName='" + tokens[0] + "." + tokens[1] + "'";
                    if (tokens.Length >= 2)
                    {
                        if (tokens[1].ToUpper() == "JPG" || tokens[1].ToUpper() == "JPEG" || tokens[1].ToUpper() == "GIF")
                        {
                            System.Drawing.Image image = System.Drawing.Image.FromFile(MapPath(".") +
                                "\\GroupEventFiles\\" + Request.QueryString["ID"].ToString() + "\\Slider\\" + dsSlider[i]["PictureName"].ToString());


                            int width = 400;
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
                            literal4.Text = "<div style=\"width: 400px; height: 250px;background-color: black;\">" +
                                "<img align=\"middle\" style=\"cursor: default; margin-left: " +
                                ((400 - newIntWidth) / 2).ToString() + "px; margin-top: " +
                                ((250 - newHeight) / 2).ToString() + "px;\" height=\"" + newHeight +
                                "px\" width=\"" + newIntWidth + "px\" height=\"" + height.ToString() + "px\" width=\"" + width.ToString() +
                                "px\" src=\"" + "GroupEventFiles/" + Request.QueryString["ID"].ToString() + "/Slider/" + dsSlider[i]["PictureName"].ToString() +
                                "\" /></div>" + caption;
                            Telerik.Web.UI.RadRotatorItem r4 = new Telerik.Web.UI.RadRotatorItem();
                            r4.Controls.Add(literal4);
                            Rotator1.Items.Add(r4);
                        }
                    }
                    else
                    {
                        Literal literal3 = new Literal();
                        literal3.Text = "<div class=\"topDiv\"><div style=\"float:left;background-color: black;\"><object width=\"400\" height=\"250\"><param  name=\"wmode\" value=\"opaque\" ></param><param name=\"movie\" value=\"http://www.youtube.com/v/" + dsSlider[i]["PictureName"].ToString() +
                            "\"></param><param name=\"allowFullScreen\" value=\"true\"></param><embed wmode=\"opaque\" src=\"http://www.youtube.com/v/" + dsSlider[i]["PictureName"].ToString() +
                            "\" type=\"application/x-shockwave-flash\" allowfullscreen=\"true\" width=\"400\" height=\"250\"></embed></object></div>" + caption + "</div>";
                        Telerik.Web.UI.RadRotatorItem r3 = new Telerik.Web.UI.RadRotatorItem();
                        r3.Controls.Add(literal3);
                        Rotator1.Items.Add(r3);
                    }
                }
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
            #endregion

            #region Take care of Groupings
            DataView dvGroupings = dat.GetDataDV("SELECT * FROM GroupEvent_Groupings WHERE GroupEventID=" + Request.QueryString["ID"].ToString());
            DataView dvGroupingMembers = dat.GetDataDV("SELECT * FROM GroupEvent_UserGroupings GG, Users U "+
                "WHERE GG.UserID=U.User_ID AND GG.GroupEventID=" + Request.QueryString["ID"].ToString());

            if (dvGroupings.Count > 0)
            {
                string headerStr = "";
                string usersStr = "";
                GroupingLabel.Text = "Groupings";
                Telerik.Web.UI.RadToolTip tip;
                foreach (DataRowView row in dvGroupings)
                {
                    lit = new Literal();
                    lit.Text = "<div style=\"float: left; padding-bottom: 10px;\">";
                    GroupingPanel.Controls.Add(lit);

                    Label lab = new Label();
                    lab.Text = row["GroupingName"].ToString();
                    lab.CssClass = "AddWhiteLink";
                    lab.ID = "tooltip" + row["ID"].ToString();
                    
                    GroupingPanel.Controls.Add(lab);

                    tip = new RadToolTip();
                    tip.Text = "<div class=\"EventDiv\"><label>" + row["GroupingDescription"].ToString() + "</label></div>";
                    tip.TargetControlID = lab.ClientID;
                    tip.IsClientID = true;
                    tip.ManualClose = true;
                    tip.Position = ToolTipPosition.MiddleRight;
                    tip.RelativeTo = ToolTipRelativeDisplay.Element;
                    tip.Skin = "Black";
                    tip.Width = 200;
                    tip.ShowEvent = ToolTipShowEvent.OnClick;
                    GroupingPanel.Controls.Add(tip);


                    lit = new Literal();
                    lit.Text = "<br/>";
                    GroupingPanel.Controls.Add(lit);

                    dvGroupingMembers.RowFilter = "GroupingID=" + row["ID"].ToString();

                    headerStr = "";
                    foreach (DataRowView row2 in dvGroupingMembers)
                    {
                        friendImg = "";
                        strFill = "";
                        if (System.IO.File.Exists(Server.MapPath(".") + "\\UserFiles\\" + row2["UserName"].ToString() +
                                    "\\Profile\\" + row2["ProfilePicture"].ToString()))
                        {
                            friendImg = "UserFiles/" + row2["UserName"].ToString() + "/Profile/" +
                                row2["ProfilePicture"].ToString();
                            strFill = "";
                        }
                        else
                        {
                            friendImg = "image/noAvatar_50x50_small.png";
                            strFill = "onmouseover=\"this.src='NewImages/noAvatar_50x50_smallhover.png'\"" +
                                "onmouseout=\"this.src='image/noAvatar_50x50_small.png'\" ";
                        }

                        headerStr += "<div style=\"float: left; clear: both; padding-left: 3px; padding-top: 5px;\"><a target='_blank' href=\"" + row2["UserName"].ToString() + "_Friend\"><img " + strFill +
                            " style=\" border: 0;float: left;padding-right: 7px; padding-bottom: 2px;\" " +
                                        "src=\"" + friendImg + "\" title=\"" + row2["UserName"].ToString() +
                                        "\" name=\"" + row2["UserName"].ToString() +
                                        "\" width=\"50px\" height=\"50px\" /></a></div><br/>";
                    }
                    headerStr += "</div>";

                    lit = new Literal();
                    lit.Text = headerStr;
                    GroupingPanel.Controls.Add(lit);
                }
            }
            else
            {
                MembersPanel.Width = 435;
            }
            #endregion

            //other details

            //Digg
            DiggLiteral.Text = "<a alt=\"Digg it!\" class=\"DiggThisButton DiggIcon\" id=\"dibbButt\"" +
                            "href='http://digg.com/submit?phase=2&url=" + "http://" +
                            Request.Url.Authority + "/" +
                            dat.MakeNiceName(dvGroup[0]["Name"].ToString()) +
                            "_" + Request.QueryString["ID"].ToString() + "_GroupEvent" +
                            "' target=\"_blank\">Digg</a>";

            if (dvGroup[0]["Agenda"] != null)
            {
                if (dvGroup[0]["Agenda"].ToString().Trim() != "")
                {
                    AgendaLiteral.Text = dvGroup[0]["Agenda"].ToString();
                    AgendaPanel.Visible = true;
                }
                else
                    AgendaPanel.Visible = false;
            }
            else
            {
                AgendaPanel.Visible = false;
            }

            TagCloud.THE_ID = int.Parse(Request.QueryString["ID"].ToString());

            GroupDescriptionLabel.Text = dvGroup[0]["Content"].ToString();

            HostLabel.Text = dvUser[0]["UserName"].ToString();
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString() + "<br/>" + command;
        }
    }

    protected void FillMembers()
    {
        Literal lit;
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        DataView dvGroup = dat.GetDataDV("SELECT * FROM GroupEvents WHERE ID=" + Request.QueryString["ID"].ToString());

        DataView dvMembers;
        if (dvGroup[0]["EventType"].ToString() == "1")
            dvMembers = dat.GetDataDV("SELECT * FROM User_GroupEvent_Calendar GM, Users U WHERE GM.GroupEventID=" +
                Request.QueryString["ID"].ToString() + " AND GM.UserID=U.User_ID AND ReoccurrID=" +
                Request.QueryString["O"].ToString());
        else
            dvMembers = dat.GetDataDV("SELECT * FROM GroupEvent_Members GM, Users U WHERE GM.GroupEventID=" +
                Request.QueryString["ID"].ToString() + " AND ReoccurrID=" + Request.QueryString["O"].ToString() +
                " AND GM.UserID=U.User_ID AND GM.Accepted='True'");

        DataView dvMemberM = dat.GetDataDV("SELECT * FROM Group_Members WHERE GroupID=" +
                dvGroup[0]["GroupID"].ToString());

        string members = "";
        string friendImg = "";
        string strFill = "";
        string title = "";
        string description = "";
        string mem = "";
        foreach (DataRowView row in dvMembers)
        {
            mem = "";
            dvMemberM.RowFilter = "MemberID=" + row["User_ID"].ToString();
            if (dvMemberM.Count == 0)
                mem = " - Non Group Member";
            else
                mem = " - Group Member";

            friendImg = "";
            strFill = "";
            if (System.IO.File.Exists(Server.MapPath(".") + "\\UserFiles\\" + row["UserName"].ToString() +
                        "\\Profile\\" + row["ProfilePicture"].ToString()))
            {
                friendImg = "UserFiles/" + row["UserName"].ToString() + "/Profile/" + row["ProfilePicture"].ToString();
                strFill = "";
            }
            else
            {
                friendImg = "image/noAvatar_50x50_small.png";
                strFill = "onmouseover=\"this.src='NewImages/noAvatar_50x50_smallhover.png'\"" +
                    "onmouseout=\"this.src='image/noAvatar_50x50_small.png'\" ";
            }

            bool getHost = false;

            members += "<div class=\"topDiv\" style=\"color: white; font-size: 12px;float: left; clear: both; padding-left: 3px; padding-top: 5px;\"><a target='_blank' href=\"" + row["UserName"].ToString() + "_Friend\"><img " + strFill +
                " style=\" border: 0;float: left;padding-right: 7px; padding-bottom: 2px;\" " +
                            "src=\"" + friendImg + "\" width=\"50px\" height=\"50px\" /></a><a style=\"float: left;\" class=\"MemberName\">" +
                            row["UserName"].ToString() + "</a><div style=\"float: left;\">" + mem + "</div></div>";
        }
        if (dvMembers.Count > 5)
        {
            lit = new Literal();
            lit.Text = members;
            MembersPanel.Controls.Add(lit);
            MembersPanel.Visible = true;
        }
        else
        {
            MembersLiteral.Text = members;
        }
    }

    protected bool HasRegistrationEnded()
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


        DataView dvGroup = dat.GetDataDV("SELECT * FROM GroupEvents WHERE ID=" + Request.QueryString["ID"].ToString());

        DataView dvGoingMember = dat.GetDataDV("SELECT * FROM GroupEvent_Members WHERE GroupEventID=" +
                        Request.QueryString["ID"].ToString() + " AND Accepted='True' AND ReoccurrID=" +
                        Request.QueryString["O"].ToString());
        if (dvGroup[0]["RegType"].ToString() == "2")
        {
            if (dvGroup[0]["RegNum"] != null)
            {
                if (dvGroup[0]["RegNum"].ToString().Trim() != "")
                {
                    if (dvGoingMember.Count >= int.Parse(dvGroup[0]["RegNum"].ToString()))
                    {
                        return true;
                    }
                    else
                    {

                    }
                }

                if (dvGroup[0]["RegDeadline"] != null)
                {
                    if (dvGroup[0]["RegDeadline"].ToString().Trim() != "")
                    {
                        if (DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")) >
                            DateTime.Parse(dvGroup[0]["RegDeadline"].ToString()))
                        {
                            return true;
                        }
                    }
                }
            }

            if (dvGroup[0]["RegDeadline"] != null)
            {
                if (dvGroup[0]["RegDeadline"].ToString().Trim() != "")
                {
                    if (DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")) >
                        DateTime.Parse(dvGroup[0]["RegDeadline"].ToString()))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    protected void CheckGoing(object sender, EventArgs e)
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
        
        DataView dvGroup = dat.GetDataDV("SELECT * FROM GroupEvents WHERE ID=" + Request.QueryString["ID"].ToString());

        if (!HasRegistrationEnded())
        {
            if (dvGroup[0]["EventType"].ToString() == "1")
            {
                DataView dvExists = dat.GetDataDV("SELECT * FROM User_GroupEvent_Calendar WHERE GroupEventID=" +
                    Request.QueryString["ID"].ToString() + " AND UserID=" + Session["User"].ToString() +
                    " AND ReoccurrID=" + Request.QueryString["O"].ToString());

                if (dvExists.Count == 0)
                {

                    dat.Execute("INSERT INTO User_GroupEvent_Calendar (GroupEventID, UserID, DateAdded, ReoccurrID) VALUES(" +
                    Request.QueryString["ID"].ToString() + ", " + Session["User"].ToString() + ", '" +
                    DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"))
                    + "', " +
                    Request.QueryString["O"].ToString() + ")");
                }
            }
            else
            {
                DataView dvIsInvited = dat.GetDataDV("SELECT * FROM GroupEvent_Members WHERE GroupEventID=" +
                    Request.QueryString["ID"].ToString() + " AND UserID=" + Session["User"].ToString() +
                    " AND ReoccurrID=" + Request.QueryString["O"].ToString());
                if (dvIsInvited.Count == 0)
                {
                    dat.Execute("INSERT INTO GroupEvent_Members (DateAdded,GroupEventID, UserID, Accepted, " +
                        "ReoccurrID) VALUES('" +
                    DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"))
                    + "', " +
                    Request.QueryString["ID"].ToString() + ", " + Session["User"].ToString() + ", 'True', " +
                    Request.QueryString["O"].ToString() + ")");
                }
                else
                {
                    dat.Execute("UPDATE GroupEvent_Members SET Accepted ='True' WHERE ReoccurrID=" +
                        Request.QueryString["O"].ToString() + " AND GroupEventID=" +
                        Request.QueryString["ID"].ToString() + " AND UserID=" + Session["User"].ToString());
                }
            }


            AreGoingPanel.Visible = true;
            RegistrationEndedPanel.Visible = false;
            GoingPanel.Visible = false;

            FillMembers();
        }
    }

    protected void StuffGrabbed(object sender, EventArgs e)
    {
        CheckBox check = (CheckBox)sender;

        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        string theID = check.ID.Replace("check", "");

        dat.Execute("UPDATE GroupEvent_StuffNeeded SET UserID=" + Session["User"].ToString() + " WHERE ID=" + theID);

        Literal lit = new Literal();

        string addthis = "";
        
            addthis = "<div style=\"float: left;\"><img style=\"cursor: pointer;\" name=\"remove "+
                "the user\" src=\"image/X.png\" onclick=\"RemoveStuffUser('" + theID +
            "');\" onmouseout=\"this.src = 'image/X.png';\" " +
            "onmouseover=\"this.src = 'image/XSelected.png';\" /></div>";

            lit.Text = "<div class=\"topDiv\" width=\"400px\"><div style=\"float: left;\"><ul style=\"font-weight: bold;padding: 0; margin: 0; " +
            "color: white;padding-left: 22px;\"><li><span style=\"color: white; font-size: 14px;\">" +
            check.Text +
                " -- " + Session["UserName"].ToString() + "</span></li></ul></div>" + addthis + "</div>";
        StuffWeNeedPanel.Controls.RemoveAt(StuffWeNeedPanel.Controls.IndexOf(check) - 1);
        StuffWeNeedPanel.Controls.RemoveAt(StuffWeNeedPanel.Controls.IndexOf(check) + 1);
        StuffWeNeedPanel.Controls.AddAt(StuffWeNeedPanel.Controls.IndexOf(check), lit);
        StuffWeNeedPanel.Controls.Remove(check);
    }

    protected void RemoveUserStuff(object sender, EventArgs e)
    {
        string parameter = Request.Params["__EVENTARGUMENT"];
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        dat.Execute("UPDATE GroupEvent_StuffNeeded SET UserID=NULL WHERE ID=" + parameter);

        Literal liter = (Literal)StuffWeNeedPanel.FindControl("lit" + parameter);

        DataView dv = dat.GetDataDV("SELECT * FROM GroupEvent_StuffNeeded WHERE ID=" + parameter);

        CheckBox check = new CheckBox();
        check.ID = "check" + parameter;
        check.Text = dv[0]["StuffNeeded"].ToString();
        check.AutoPostBack = true;
        check.CheckedChanged += new EventHandler(StuffGrabbed);
        check.ForeColor = System.Drawing.Color.White;
        check.Font.Size = 14;

        Literal lit = new Literal();
        lit.Text = "<div width=\"400px\">";

        int theIndex = StuffWeNeedPanel.Controls.IndexOf(liter);
        StuffWeNeedPanel.Controls.Remove(liter);
        StuffWeNeedPanel.Controls.AddAt(theIndex, lit);
        StuffWeNeedPanel.Controls.AddAt(theIndex, check);

        lit = new Literal();
        lit.Text = "</div>";
        StuffWeNeedPanel.Controls.AddAt(theIndex, lit);

    }
}
