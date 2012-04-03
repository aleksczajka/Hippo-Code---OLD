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

public partial class Group : Telerik.Web.UI.RadAjaxPage
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
        DateTime dateNow = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
        Data dat = new Data(dateNow);
        Literal lit = new Literal();
        lit.Text = "<script src=\"http://maps.google.com/maps?file=api&amp;v=2&amp;sensor=false&amp;key=ABQIAAAAjjoxQtYNtdn3Tc17U5-jbBR2Kk_H7gXZZZniNQ8L14X1BLzkNhQjgZq1k-Pxm8FxVhUy3rfc6L9O4g\" type=\"text/javascript\"></script>";
        Master.HEAD_TAG.Controls.Add(lit);
        try
        {
            DataView dvGroup = dat.GetDataDV("SELECT * FROM Groups WHERE ID=" + Request.QueryString["ID"].ToString());
            Page.Title = dvGroup[0]["Header"].ToString() + " | Hippo Group";

            #region Take Care of Style
            ColorAa_1.Style.Add("background-color", "#" + dvGroup[0]["ColorA"].ToString());
            ColorAb_1.Style.Add("background-color", "#" + dvGroup[0]["ColorA"].ToString());
            ColorAa_1.Style.Add("color", "#" + dvGroup[0]["TextA"].ToString());

            ColorAa_2.Style.Add("background-color", "#" + dvGroup[0]["ColorA"].ToString());
            ColorAb_2.Style.Add("background-color", "#" + dvGroup[0]["ColorA"].ToString());
            ColorAa_2.Style.Add("color", "#" + dvGroup[0]["TextA"].ToString());

            ColorAa_3.Style.Add("background-color", "#" + dvGroup[0]["ColorA"].ToString());
            ColorAb_3.Style.Add("background-color", "#" + dvGroup[0]["ColorA"].ToString());
            ColorAa_3.Style.Add("color", "#" + dvGroup[0]["TextA"].ToString());

            ColorBa_1.Style.Add("background-color", "#" + dvGroup[0]["ColorB"].ToString());
            ColorBb_1.Style.Add("background-color", "#" + dvGroup[0]["ColorB"].ToString());
            ColorBa_1.Style.Add("color", "#" + dvGroup[0]["TextB"].ToString());

            ColorBa_2.Style.Add("background-color", "#" + dvGroup[0]["ColorB"].ToString());
            ColorBb_2.Style.Add("background-color", "#" + dvGroup[0]["ColorB"].ToString());
            ColorBa_2.Style.Add("color", "#" + dvGroup[0]["TextB"].ToString());

            RadScheduler1.BorderColor = System.Drawing.ColorTranslator.FromHtml("#" + dvGroup[0]["ColorB"].ToString());
            ScriptLiteral.Text = "<style type=\"text/css\">.RadScheduler .rsTodayCell {border: solid 1px #" +
                dvGroup[0]["ColorA"].ToString() + " !important;}</style>";
            #endregion

            if (!IsPostBack)
            {
                Session["RedirectTo"] = Request.Url.AbsoluteUri;
            }

            string country = dat.GetDataDV("SELECT * FROM Countries WHERE country_id=" + dvGroup[0]["Country"].ToString())[0]["country_name"].ToString();

            Literal liter = new Literal();
            liter.Text = "<link type=\"text/css\" href=\"Rads_Groups.css\" rel=\"stylesheet\" />"+
                "<style type=\"text/css\">.RadTabStrip_Telerik{border-bottom:1px solid #666666;}</style>";
            Page.Header.Controls.AddAt(0, liter);

            bool isMember = false;

            #region Take care of user types
            if (Session["User"] != null)
            {
                //Take care of Share with friends and Share though email and Flag
                LoggedInPanel.Visible = true;

                ShareFriends.THE_TEXT = "Share this with a friend";
                ShareFriends.RE_LABEL = "Re: " + Session["UserName"].ToString() +
                    " would like to share the group '\"" + dat.BreakUpString(dvGroup[0]["Header"].ToString(), 14) +
                    "\"' with you.";
                ShareFriends.TYPE = "g";
                ShareFriends.ID = int.Parse(Request.QueryString["ID"].ToString());

                string descrip = dvGroup[0]["Content"].ToString();
                if(descrip.Length > 200)
                    descrip = descrip.Substring(0, 200) + "...";

                Session["messageEmail"] = "Group Name: <a href=\"http://hippohappenings.com/" +
                    dat.MakeNiceName(dat.BreakUpString(dvGroup[0]["Header"].ToString(), 14)) + "_" +
                    Request.QueryString["ID"].ToString() + "_Group\">" +
                            dat.BreakUpString(dvGroup[0]["Header"].ToString(), 14) + "</a>  <br/><br/> " +
                            descrip;
                Session["FlagID"] = Request.QueryString["ID"].ToString();
                Session["FlagType"] = "G";

                if (bool.Parse(dvGroup[0]["isPrivate"].ToString()))
                    PrivateLabel.Text = "Private Group";
                else
                    PrivateLabel.Text = "Public Group";

                DataView dvMember = dat.GetDataDV("SELECT * FROM Group_Members WHERE GroupID=" +
                    Request.QueryString["ID"].ToString() + " AND Accepted='True' AND MemberID=" + Session["User"].ToString());
                PostMessageID.Attributes.Add("onclick", "OpenMess('" + Request.QueryString["ID"].ToString() + "')");
                if (dvMember.Count > 0)
                {
                    if (bool.Parse(dvMember[0]["SharedHosting"].ToString()))
                    {
                        isMember = true;
                        Session["isHost"] = true;
                        HostHeaderPanel.Visible = true;
                        AddEventButton2.Visible = true;
                        MemberHeaderPanel.Visible = false;
                        //if is host, take care of host options
                        LinkButton1.Attributes.Add("onclick", "OpenInvite('" + Request.QueryString["ID"].ToString() + "')");
                        LinkButton2.Attributes.Add("onclick", "OpenPrefs('" + Request.QueryString["ID"].ToString() + "')");
                        LinkButton3.PostBackUrl = "EnterGroup.aspx?ID=" + Request.QueryString["ID"].ToString();
                        LinkButton4.PostBackUrl = "EnterGroupEvent.aspx?GroupID=" + Request.QueryString["ID"].ToString();
                        AddEventButton2.PostBackUrl = "EnterGroupEvent.aspx?GroupID=" + Request.QueryString["ID"].ToString();
                        LinkButton8.Attributes.Add("onclick", "OpenThread('" + Request.QueryString["ID"].ToString() + "')");
                        Label1.Attributes.Add("onclick", "OpenSendMess('" + Request.QueryString["ID"].ToString() + "')");
                    }
                    else
                    {
                        isMember = true;
                        Session["isHost"] = false;
                        Session["isMember"] = true;
                        HostHeaderPanel.Visible = false;
                        AddEventButton2.Visible = false;
                        MemberHeaderPanel.Visible = true;
                        Label2.Attributes.Add("onclick", "OpenThread('" + Request.QueryString["ID"].ToString() + "')");
                    }
                    PostMessagePanel.Visible = true;
                    Button2.Visible = false;
                }
                else
                {
                    Session["isHost"] = false;
                    Session["isMember"] = false;
                    HostHeaderPanel.Visible = false;
                    AddEventButton2.Visible = false;
                    MemberHeaderPanel.Visible = false;
                    PostMessagePanel.Visible = false;
                    Button2.Attributes.Add("onclick", "OpenJoin('" + Request.QueryString["ID"].ToString() + "')");
                }
            }
            else
            {
                Session["isHost"] = false;
                Session["isMember"] = false;
                HostHeaderPanel.Visible = false;
                AddEventButton2.Visible = false;
                MemberHeaderPanel.Visible = false;
                PostMessagePanel.Visible = false;
                Button2.Attributes.Add("onclick", "OpenJoin('" + Request.QueryString["ID"].ToString() + "')");
            }
            #endregion

            #region Contact Info
            CityStateZipLabel.Text = dvGroup[0]["City"].ToString() + ", " + dvGroup[0]["State"].ToString() + " " + dvGroup[0]["Zip"].ToString();

            if (dvGroup[0]["Phone"].ToString().Trim() != "")
            {
                PhoneLabel.Text = "Phone: " + dvGroup[0]["Phone"].ToString() + "<br/>";
                GroupContactInfoLabel.Visible = true;
            }

            if (dvGroup[0]["Email"].ToString().Trim() != "")
            {
                EmailLabel.Text = "Email: " + dvGroup[0]["Email"].ToString() + "<br/>";
                GroupContactInfoLabel.Visible = true;
            }

            if (dvGroup[0]["Web"].ToString().Trim() != "")
            {
                WebLabel.Text = "Web: " + dvGroup[0]["Web"].ToString();
                GroupContactInfoLabel.Visible = true;
            }

            if (dvGroup[0]["HostInstructions"].ToString() != null)
            {
                if (dvGroup[0]["HostInstructions"].ToString().Trim() != "")
                {
                    HostTitleInstructionsLabel.Visible = true;
                    HostInstructionsLabel.Text = dvGroup[0]["HostInstructions"].ToString().Trim();
                }
            }
            #endregion

            #region Members
            DataView dvMainHost = dat.GetDataDV("SELECT * FROM Groups G, Group_Members GM, Users U "+
                "WHERE GM.MemberID=U.User_ID AND GM.GroupID=G.ID AND G.ID=" +
                Request.QueryString["ID"].ToString() + " AND G.Host=U.User_ID");
            DataView dvHosts = dat.GetDataDV("SELECT * FROM Group_Members GM, Users U WHERE GM.GroupID=" +
                Request.QueryString["ID"].ToString() + " AND GM.MemberID=U.User_ID AND GM.Accepted='True' " +
                "AND GM.SharedHosting='True' AND GM.MemberID <> " + dvMainHost[0]["User_ID"].ToString());
            DataView dvMembers = dat.GetDataDV("SELECT * FROM Group_Members GM, Users U WHERE GM.GroupID=" +
                Request.QueryString["ID"].ToString() + " AND GM.MemberID=U.User_ID AND GM.Accepted='True' "+
                "AND GM.SharedHosting='False' ORDER BY U.UserName");

            string members = "";
            string friendImg = "";
            string strFill = "";
            string title = "";
            string description = "";

            foreach (DataRowView row in dvMainHost)
            {
                GetMemberString(ref members, row, dvGroup);
            }

            foreach (DataRowView row in dvHosts)
            {
                GetMemberString(ref members, row, dvGroup);
            }

            foreach (DataRowView row in dvMembers)
            {
                GetMemberString(ref members, row, dvGroup);
            }

            if (dvMembers.Count + dvMainHost.Count + dvHosts.Count > 6)
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

            #endregion

            DataView dvUser = dat.GetDataDV("SELECT * FROM Users WHERE User_ID=" + dvGroup[0]["Host"].ToString());
            EventName.Text = dvGroup[0]["Header"].ToString();

            #region Group Message Board
            DataView dvMessages = dat.GetDataDV("SELECT *, GM.ID AS MessageID FROM GroupMessages GM, Users U WHERE GM.GroupID=" +
                Request.QueryString["ID"].ToString() + " AND GM.UserID=U.User_ID");
            DataView dvSticky = dat.GetDataDV("SELECT *, GM.ID AS MessageID FROM GroupMessages GM, Group_Members M, Users U WHERE GM.GroupID=" +
                Request.QueryString["ID"].ToString() + " AND GM.UserID=U.User_ID and M.GroupID=" +
                Request.QueryString["ID"].ToString() + " AND M.MemberID=U.User_ID AND M.SharedHosting='True' AND GM.isSticky='True'");
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
                            "');\" onmouseout=\"this.src = 'image/X.png';\" " +
                            "onmouseover=\"this.src = 'image/XSelected.png';\" /></div>";
                    }
                }

                members = "<div style=\"border-bottom: solid 1px #1b1b1b;float: left; clear: both; padding-left: 3px; padding-top: 5px;\">" + imgLit + 
                    "<a target='_blank' href=\"" + dvSticky[0]["UserName"].ToString() + "_Friend\"><img " + strFill +
                    " style=\" border: 0;float: left;padding-right: 7px; padding-bottom: 2px;\" " +
                                "src=\"" + friendImg + "\" width=\"50px\" height=\"50px\" /></a>"+
                                "<div style=\"float: left;\"><div style=\"height: 19px; background-position: left; background-repeat: no-repeat; background-image: url(images/UserNameLeft.png); "+
                                "padding-left: 4px;\"><div style=\"height: 19px; background-repeat: no-repeat; background-position: right; "+
                                "background-image: url(images/UserNameRight.png); "+
                                "padding-right: 4px;\"><div style=\"height: 19px; background-repeat: repeat-x; " +
                                "background-image: url(images/UserNameMiddle.png);\"><a style=\"float: left;\" href=\"" + 
                                dvSticky[0]["UserName"].ToString() + "_Friend\" " +
                                "class=\"MemberName\">" +
                                dvSticky[0]["UserName"].ToString() +
                                "</a></div></div></div></div><span class=\"MemberTitle\" style=\"color: #" + dvGroup[0]["TextB"].ToString() + " !important;\">&nbsp;-&nbsp;Posted On: " +
                                dvSticky[0]["DatePosted"].ToString().Trim() +
                                "</span> - <span style=\"color: #ff6b09; font-size: 30px;font: bold;\">!</span>" +
                                "<span class=\"MemberDescription\"  style=\"color: #" + dvGroup[0]["TextB"].ToString() + " !important;\">&nbsp;&nbsp;" + dvSticky[0]["Content"].ToString() +
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
                lit.Text = "<div class=\"MemberDescription\" style=\"color: #" + dvGroup[0]["TextB"].ToString() + " !important;\">No messages have been posted for this group.</div>";
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


                    description = "<span class=\"MemberDescription\" style=\"color: #" + dvGroup[0]["TextB"].ToString() + " !important;\">&nbsp;&nbsp;" + row["Content"].ToString().Trim() + "</span>";


                    members += "<div style=\"float: left; clear: both; padding-left: 3px; padding-top: 5px;\"><a target='_blank' href=\"" + row["UserName"].ToString() + "_Friend\"><img " + strFill +
                        " style=\" border: 0;float: left;padding-right: 7px; padding-bottom: 2px;\" " +
                                    "src=\"" + friendImg + "\" width=\"50px\" height=\"50px\" /></a>"+
                                    "<div style=\"float: left;\"><div style=\"height: 19px; background-position: left; background-repeat: no-repeat; background-image: url(images/UserNameLeft.png); " +
                                "padding-left: 4px;\"><div style=\"height: 19px; background-repeat: no-repeat; background-position: right; " +
                                "background-image: url(images/UserNameRight.png); " +
                                "padding-right: 4px;\"><div style=\"height: 19px; background-repeat: repeat-x; " +
                                "background-image: url(images/UserNameMiddle.png);\"><a style=\"float: left;\" href=\"" + row["UserName"].ToString() + "_Friend\" class=\"MemberName\">" +
                                    row["UserName"].ToString() + "</a></div></div></div></div><span class=\"MemberTitle\" style=\"color: #" + dvGroup[0]["TextB"].ToString() + 
                                    " !important;\">&nbsp;-&nbsp;Posted On: " + row["DatePosted"].ToString().Trim() + "</span> - " + description + "</div>";
                }

                lit = new Literal();
                lit.Text = members;
                MessagesPanel.Controls.Add(lit);
            }
            #endregion

            #region Take care of images and youtube
            char[] delim4 = { ';' };

            DataView dsSlider = dat.GetDataDV("SELECT * FROM Group_Slider_Mapping WHERE GroupID=" + Request.QueryString["ID"].ToString());
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
                        if (tokens[1].ToUpper() == "JPG" || tokens[1].ToUpper() == "JPEG" || tokens[1].ToUpper() == "GIF" || tokens[1].ToUpper() == "PNG")
                        {
                            System.Drawing.Image image = System.Drawing.Image.FromFile(MapPath(".") +
                                "\\GroupFiles\\" + Request.QueryString["ID"].ToString() + "\\Slider\\" + dsSlider[i]["PictureName"].ToString());


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
                                "<img align=\"middle\" style=\"cursor: pointer; margin-left: " +
                                ((400 - newIntWidth) / 2).ToString() + "px; margin-top: " +
                                ((250 - newHeight) / 2).ToString() + "px;\" height=\"" + newHeight +
                                "px\" width=\"" + newIntWidth + "px\" height=\"" + height.ToString() + "px\" width=\"" + width.ToString() +
                                "px\" src=\"" + "GroupFiles/" + Request.QueryString["ID"].ToString() + "/Slider/" + dsSlider[i]["PictureName"].ToString() +
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

            #region Take care of events
            //RadScheduler1.DataSource = dat.GetData("SELECT GEO.DateTimeStart, SUBSTRING(GE.Name, 0, 8) + ' ...' AS Name , GEO.DateTimeEnd, CONVERT(NVARCHAR,GE.ID) + ';'+CONVERT(NVARCHAR,GEO.ID) AS ID FROM GroupEvent_Occurance GEO, GroupEvents GE WHERE GEO.GroupEventID=GE.ID AND GE.GroupID=" + Request.QueryString["ID"].ToString());
            if (!IsPostBack)
            {
                if (isMember)
                {
                    RadScheduler1.DataSource = dat.GetData("SELECT CONVERT(NVARCHAR,MONTH(GEO.DateTimeStart)) + '/' + " +
                        "CONVERT(NVARCHAR,DAY(GEO.DateTimeStart)) + '/' + " +
                        "CONVERT(NVARCHAR,YEAR(GEO.DateTimeStart)) + ' 12:00:00' AS DateStart, " +
                        "CONVERT(NVARCHAR,COUNT(CONVERT(NVARCHAR,MONTH(GEO.DateTimeStart)) + '/' + " +
                        "CONVERT(NVARCHAR,DAY(GEO.DateTimeStart)) + '/' +" +
                        "CONVERT(NVARCHAR,YEAR(GEO.DateTimeStart)))) + CASE WHEN COUNT(CONVERT(NVARCHAR,MONTH(GEO.DateTimeStart)) + '/' + " +
                        "CONVERT(NVARCHAR,DAY(GEO.DateTimeStart)) + '/' +" +
                        "CONVERT(NVARCHAR,YEAR(GEO.DateTimeStart))) > 1 THEN ' events' ELSE ' event' END AS Name FROM " +
                        "GroupEvent_Occurance GEO, GroupEvents GE WHERE GE.LIVE='True' AND GEO.GroupEventID=GE.ID " +
                        "AND GE.GroupID=" + Request.QueryString["ID"].ToString() +
                        " GROUP BY CONVERT(NVARCHAR,MONTH(GEO.DateTimeStart)) + '/' + " +
                        "CONVERT(NVARCHAR,DAY(GEO.DateTimeStart)) + '/' +" +
                        "CONVERT(NVARCHAR,YEAR(GEO.DateTimeStart))");
                }
                else
                {
                    RadScheduler1.DataSource = dat.GetData("SELECT CONVERT(NVARCHAR,MONTH(GEO.DateTimeStart)) + '/' + " +
                        "CONVERT(NVARCHAR,DAY(GEO.DateTimeStart)) + '/' + " +
                        "CONVERT(NVARCHAR,YEAR(GEO.DateTimeStart)) + ' 12:00:00' AS DateStart, " +
                        "CONVERT(NVARCHAR,COUNT(CONVERT(NVARCHAR,MONTH(GEO.DateTimeStart)) + '/' + " +
                        "CONVERT(NVARCHAR,DAY(GEO.DateTimeStart)) + '/' +" +
                        "CONVERT(NVARCHAR,YEAR(GEO.DateTimeStart)))) + CASE WHEN COUNT(CONVERT(NVARCHAR,MONTH(GEO.DateTimeStart)) + '/' + " +
                        "CONVERT(NVARCHAR,DAY(GEO.DateTimeStart)) + '/' +" +
                        "CONVERT(NVARCHAR,YEAR(GEO.DateTimeStart))) > 1 THEN ' events' ELSE ' event' END AS Name FROM " +
                        "GroupEvent_Occurance GEO, GroupEvents GE WHERE GE.LIVE='True' AND GE.EventType=1 AND GEO.GroupEventID=GE.ID " +
                        "AND GE.GroupID=" + Request.QueryString["ID"].ToString() +
                        " GROUP BY CONVERT(NVARCHAR,MONTH(GEO.DateTimeStart)) + '/' + " +
                        "CONVERT(NVARCHAR,DAY(GEO.DateTimeStart)) + '/' +" +
                        "CONVERT(NVARCHAR,YEAR(GEO.DateTimeStart))");
                }
                RadScheduler1.DataBind();
                RadScheduler1.SelectedDate = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
                //RadScheduler1.SelectedDate = DateTime.Parse("8/9/2010 23:13:00");
                MonthLabel.Text = dat.GetMonth(RadScheduler1.SelectedDate.Month.ToString());
                //MonthLabel.Text += " "+RadScheduler1.SelectedDate.ToString();
            }            
            #endregion

            //Do this before unread threads are marked as read
            if (Session["User"] != null)
            {
                DataView ThreadsRead = dat.GetDataDV("SELECT * FROM ThreadRead TH, GroupThreads GT WHERE TH.UserID=" +
                    Session["User"].ToString() + " AND TH.[Read] = 'False' AND GT.GroupID=" + Request.QueryString["ID"].ToString() +
                    " AND GT.ID=TH.ThreadID ");

                if (ThreadsRead.Count > 0)
                    RadTabStrip1.Tabs[1].Text = "<div id=\"BiggerNewDiv\" style=\"float: left;display: block;font-weight: bold;color: #ff6b09;font-size: 12px;" +
                                "\"><div style=\"float: left;\">(</div><div style=\"float: left\"  id=\"NewThreadDiv\">" +
                                ThreadsRead.Count.ToString() +
                                "</div>&nbsp;unread)</div>&nbsp;&nbsp;<span onclick='HideSocial()'>Stuff We Talk About</span>";
            }

            #region Take care of threads
            SortDropDown.Items[1].Attributes.Add("onclick", "ShowLoading();");
            SortDropDown.Items[2].Attributes.Add("onclick", "ShowLoading();");
            SortDropDown.Items[3].Attributes.Add("onclick", "ShowLoading();");
            SortThreads(SortDropDown, new EventArgs());
            #endregion

            //other details

            //TagCloud.THE_ID = int.Parse(Request.QueryString["ID"].ToString());
            GroupDescriptionLabel.Text = dvGroup[0]["Content"].ToString();

            //Fix calendar timezone
            RadScheduler1.TimeZoneOffset = -(DateTime.UtcNow.Subtract(dateNow));

            HostLabel.Text = dvUser[0]["UserName"].ToString();
            HostLabel.NavigateUrl = HostLabel.Text + "_Friend";

            #region Take care of map
            HttpCookie cookie2 = new HttpCookie("addressParameterName");
            cookie2.Value = dvGroup[0]["Header"].ToString();
            cookie2.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie2);

            string address = "";
            if (dvGroup[0]["Address"] != null)
            {
                if (dvGroup[0]["Address"].ToString() != "")
                {
                    address = dat.GetAddress(dvGroup[0]["Address"].ToString(), dvGroup[0]["Country"].ToString() != "223");

                }
            }

            cookie2 = new HttpCookie("addressParameter");
            cookie2.Value = address + " " + dvGroup[0]["City"].ToString() + " " + dvGroup[0]["State"].ToString() +
                " " + dvGroup[0]["Zip"].ToString() + " " + country;
            cookie2.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie2);


            Literal lit2 = new Literal();
            lit2.Text = "<script src=\"http://maps.google.com/maps?file=api&amp;v=2&amp;sensor=false&amp;key=ABQIAAAAjjoxQtYNtdn3Tc17U5-jbBR2Kk_H7gXZZZniNQ8L14X1BLzkNhQjgZq1k-Pxm8FxVhUy3rfc6L9O4g\" type=\"text/javascript\"></script>";
            Page.Header.Controls.Add(lit2);

            Master.BodyTag.Attributes.Add("onload", "initialize();");
            Master.BodyTag.Attributes.Add("onunload", "GUnload()");
            #endregion

            #region Take care of main image
            bool doVertical = false;
            if (dvGroup[0]["MainPicture"] != null)
            {
                if (dvGroup[0]["MainPicture"].ToString().Trim() != "")
                {
                    ImageLiteral.Text = "<div style=\"width: 200px;\"><div";
                    GroupMainImage.ImageUrl = "GroupFiles/" + dvGroup[0]["ID"].ToString() + "/Slider/" + dvGroup[0]["MainPicture"].ToString().Trim();
                    System.Drawing.Image img = System.Drawing.Image.FromFile(Server.MapPath("GroupFiles/" + dvGroup[0]["ID"].ToString() + "/Slider/" + dvGroup[0]["MainPicture"].ToString().Trim()));

                    if (img.Height < 200)
                    {
                        string toDivide = ((200 - img.Height) / 2).ToString();
                        ImageLiteral.Text += " style=\" padding-top: 10px; " + "\" ";

                        if (img.Width < 200)
                        {
                            string toDivide2 = ((200 - img.Width) / 2).ToString();
                            ImageLiteral.Text += " padding-left: " + toDivide2 + "px;" + "\" ";
                        }
                    }
                    else if (img.Width < 200)
                    {
                        string toDivide2 = ((200 - img.Width) / 2).ToString();
                        ImageLiteral.Text += " style=\" padding-left: " + toDivide2 + "px;" + "\" ";
                    }



                    ImageLiteral.Text += ">";
                    ImageLiteralBottom.Text = "</div></div>";

                    //Digg
                    DiggLiteral.Text = "<table style=\"padding-top: 5px;\">" +
                                   " <tr> " +
                                     "  <td valign=\"bottom\" style=\"padding-right: 0px;\">" +
                                       "     <a name=\"fb_share\" type=\"button\" " +
                                       "href=\"http://www.facebook.com/sharer.php\">Share</a>" +
                                       "<script src=\"http://static.ak.fbcdn.net/connect.php/js/FB.Share\"" +
                                       " type=\"text/javascript\"></script>" +
                                       " </td> " +
                                       " <td valign=\"bottom\" style=\"padding-right: 0px;\"> " +
                                        "    <a style=\"border: 0; padding: 0; margin: 0;\" " +
                                        "id=\"TweeterA\" title=\"Click to send this page to Twitter!\" " +
                                        "target=\"_blank\" rel=\"nofollow\"><img style=\"border: 0; " +
                                        "padding: 0; margin: 0;\" " +
                                        "src=\"http://twitter-badges.s3.amazonaws.com/twitter-a.png\" " +
                                        "alt=\"Share on Twitter\"/></a> " +
                                        "</td> " +
                                        "<td valign=\"bottom\">" +
                                         "   <a href=\"javascript:void(window.open('http://www.myspace.com/Modules/PostTo/Pages/?u='+encodeURIComponent(document.location.toString()),'ptm','height=450,width=440').focus())\"> " +
                                        "        <img style=\"position: relative; top: 2px\" " +
                                        "src=\"http://cms.myspacecdn.com/cms/ShareOnMySpace/small.png\" " +
                                        "border=\"0\" alt=\"Share on MySpace\" /> " +
                                         "   </a> " +
                                        "</td> " +
                                    "</tr> " +
                                   " <tr> " +
                                      "  <td valign=\"bottom\" colspan=\"3\" align=\"center\"> " +
                                        "    <table cellpadding=\"5px\"> " +
                                         "       <tr> " +
                                          "          <td><a alt=\"Digg it!\" class=\"DiggThisButton DiggIcon\" id=\"dibbButt\"" +
                                    "href='http://digg.com/submit?phase=2&url=" + "http://" +
                                    Request.Url.Authority + "/" +
                                    dat.MakeNiceName(dvGroup[0]["Header"].ToString()) +
                                    "_" + Request.QueryString["ID"].ToString() + "_Group" +
                                    "' target=\"_blank\">Digg</a>" +
                                                    "</td>" +
                                                    "<td>" +
                           "<a href=\"http://delicious.com/save\" onclick=\"window.open('http://delicious.com/save?v=5&noui&jump=close&url='+encodeURIComponent(location.href)+'&title='+encodeURIComponent(document.title), 'delicious','toolbar=no,width=550,height=550'); return false;\"> " +
                           "<img border=\"0\" src=\"http://static.delicious.com/img/delicious.small.gif\" height=\"10\" width=\"10\" alt=\"Delicious\" /> " +
                                                  "  </a> " +

                                                   " </td> " +
                                                    "<td> " +
                                                     "<script src=\"http://www.stumbleupon.com/hostedbadge.php?s=4\"></script>" +
                                                    "</td>" +
                                                "</tr>" +
                                            "</table>" +
                                        "</td>" +
                                        "<td></td>" +
                                   " </tr>" +
                               " </table>";

                    SocialsVertical.Visible = false;
                    SocialsHorizontal.Visible = true;

                }
                else
                {
                    doVertical = true;
                    GroupMainImage.Visible = false;
                }
            }
            else
            {
                doVertical = true;
                GroupMainImage.Visible = false;
            }

            if (doVertical)
            {
                //Digg
                DiggLiteralVertical.Text = "<table style=\"padding-top: 50px; padding-left: 70px;\">" +
                                        "<tr> " +
                                            "<td valign=\"bottom\" style=\"padding-right: 0px;\">" +
                                             "   <a name=\"fb_share\" type=\"button\" href=\"http://www.facebook.com/sharer.php\">Share</a><script src=\"http://static.ak.fbcdn.net/connect.php/js/FB.Share\" type=\"text/javascript\"></script>" +
                                            "</td>" +
                                        "</tr>" +
                                       " <tr>" +
                                         "   <td valign=\"bottom\" style=\"padding-right: 0px;\">" +
                                         "       <a style=\"border: 0; padding: 0; margin: 0;\" id=\"A1\" title=\"Click to send this page to Twitter!\" target=\"_blank\" rel=\"nofollow\"><img style=\"border: 0; padding: 0; margin: 0;\" src=\"http://twitter-badges.s3.amazonaws.com/twitter-a.png\" alt=\"Share on Twitter\"/></a>" +
                                         "   </td>" +
                                        "</tr>" +
                                        "<tr>" +
                                         "   <td valign=\"bottom\">" +
                                         "       <a href=\"javascript:void(window.open('http://www.myspace.com/Modules/PostTo/Pages/?u='+encodeURIComponent(document.location.toString()),'ptm','height=450,width=440').focus())\">" +
                                          "          <img src=\"http://cms.myspacecdn.com/cms/ShareOnMySpace/small.png\" border=\"0\" alt=\"Share on MySpace\" />" +
                                           "     </a>" +
                                           " </td>" +
                                       " </tr>" +
                                       " <tr>" +
                                         "   <td valign=\"bottom\"> <a alt=\"Digg it!\" class=\"DiggThisButton DiggIcon\" id=\"dibbButt\"" +
                                "href='http://digg.com/submit?phase=2&url=" + "http://" +
                                Request.Url.Authority + "/" +
                                dat.MakeNiceName(dvGroup[0]["Header"].ToString()) +
                                "_" + Request.QueryString["ID"].ToString() + "_Group" +
                                "' target=\"_blank\">Digg</a> " +
                                              "  <a href=\"http://delicious.com/save\" onclick=\"window.open('http://delicious.com/save?v=5&noui&jump=close&url='+encodeURIComponent(location.href)+'&title='+encodeURIComponent(document.title), 'delicious','toolbar=no,width=550,height=550'); return false;\">" +
                                                  "  <img border=\"0\" src=\"http://static.delicious.com/img/delicious.small.gif\" height=\"10\" width=\"10\" alt=\"Delicious\" />" +
                                                "</a>" +
                                               "  <script src=\"http://www.stumbleupon.com/hostedbadge.php?s=4\"></script>" +
                                            " </td>" +
                                        " </tr>" +

                                   "  </table>";
                SocialsHorizontal.Visible = false;
                SocialsVertical.Visible = true;
            }
            #endregion
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString();
        }
    }

    protected void GetMemberString(ref string members, DataRowView row, DataView dvGroup)
    {
        string friendImg = "";
        string strFill = "";
        string title = "";
        string description = "";
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
        if (row["Title"] != null)
        {
            if (row["Title"].ToString().Trim() != "")
            {
                title = "<span class=\"MemberTitle\"  style=\"color: #" + dvGroup[0]["TextA"].ToString() + " !important;\">&nbsp;-&nbsp;" + 
                    row["Title"].ToString().Trim() + "</span>";
            }
            else
            {
                getHost = true;
            }
        }
        else
        {
            getHost = true;
        }

        if (getHost && row["MemberID"].ToString() == dvGroup[0]["Host"].ToString())
        {
            title = "<span class=\"MemberTitle\" style=\"color: #" + dvGroup[0]["TextA"].ToString() + " !important;\">&nbsp;-&nbsp;Host</span>";
        }

        if (row["Description"] != null)
        {
            if (row["Description"].ToString().Trim() != "")
            {
                description = "<span class=\"MemberDescription\" style=\"color: #" + dvGroup[0]["TextA"].ToString() + " !important;\">&nbsp;-&nbsp;" + row["Description"].ToString().Trim() + "</span>";
            }
        }

        members += "<div style=\"float: left; clear: both; padding-left: 3px; padding-top: 5px;\"><a target='_blank' href=\"" + row["UserName"].ToString() + "_Friend\"><img " + strFill +
            " style=\" border: 0;float: left;padding-right: 7px; padding-bottom: 2px;\" " +
                        "src=\"" + friendImg + "\" width=\"50px\" height=\"50px\" /></a>"+
                        "<div style=\"float: left;\"><div style=\"height: 19px; background-position: left; background-repeat: no-repeat; background-image: url(images/UserNameLeft.png); " +
                                "padding-left: 4px;\"><div style=\"height: 19px; background-repeat: no-repeat; background-position: right; " +
                                "background-image: url(images/UserNameRight.png); " +
                                "padding-right: 4px;\"><div style=\"height: 19px; background-repeat: repeat-x; " +
                                "background-image: url(images/UserNameMiddle.png);\"><a style=\"float: left;\" href=\"" + row["UserName"].ToString() + "_Friend\" class=\"MemberName\">" +
                        row["UserName"].ToString() + "</a></div></div></div></div>" + title + description + "</div>";
    }

    protected void JoinGroup(object sender, EventArgs e)
    {
    }

    protected void RadScheduler1_AppointmentCreated(object sender, AppointmentCreatedEventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        DateTime timeEvent = DateTime.Parse(e.Appointment.ID.ToString());

        string dateStart = timeEvent.Month.ToString() + "/" + timeEvent.Day.ToString() + "/" + timeEvent.Year.ToString();

        DataView dv = dat.GetDataDV("SELECT *, GEO.ID AS GEOID FROM GroupEvents GE, GroupEvent_Occurance " +
            "GEO WHERE GE.ID=GEO.GroupEventID AND CONVERT(NVARCHAR,MONTH(GEO.DateTimeStart)) + '/' + " +
            "CONVERT(NVARCHAR,DAY(GEO.DateTimeStart)) + '/' +" +
            "CONVERT(NVARCHAR,YEAR(GEO.DateTimeStart)) = '" + dateStart + "' AND GE.GroupID=" + Request.QueryString["ID"].ToString());
        
        RadToolTip newToolTip = new RadToolTip();
        DateTime dt = new DateTime();
        string replaceIt = "";
        string content = "";
        newToolTip.Text = "<div style=\"display: block; width: 500px; color: #cccccc;\">";
        foreach (DataRowView row in dv)
        {
            dt = DateTime.Parse(row["DateTimeStart"].ToString());
            content = row["Content"].ToString();

            if (content.Length > 400)
            {
                content = content.Substring(0, 400) + "... <a href=\"" + dat.MakeNiceName(row["Name"].ToString()) +
                    "_" + row["GEOID"].ToString() + "_" +
                 row["GroupEventID"].ToString() + "_GroupEvent\" class=\"AddUnderLink\">Read More</a>";
            }

            replaceIt = dt.Month.ToString() + "/" + dt.Day.ToString() + "/" + dt.Year.ToString();
            newToolTip.Text += "<a href=\"" + dat.MakeNiceName(row["Name"].ToString()) + "_" + 
                row["GEOID"].ToString() + "_" +
                 row["GroupEventID"].ToString() + "_GroupEvent\" class=\"AddUnderLink\" >" +
                dat.BreakUpString(row["Name"].ToString(), 50) + "</a>" +
                "<br/>" + row["DateTimeStart"].ToString().Replace(replaceIt, "") + " - " +
                row["DateTimeEnd"].ToString().Replace(replaceIt, "") + "<br/>" + content + "<br/><br/>";
        }
        newToolTip.Text += "</div>";
        newToolTip.TargetControlID = e.Appointment.ClientID;
        newToolTip.IsClientID = true;
        newToolTip.HideEvent = ToolTipHideEvent.ManualClose;
        newToolTip.Animation = ToolTipAnimation.None;
        newToolTip.Position = ToolTipPosition.MiddleLeft;
        newToolTip.ShowEvent = ToolTipShowEvent.OnClick;
        newToolTip.Skin = "Black";
        newToolTip.Width = 520;
        ToolTipPanel.Controls.Add(newToolTip);
    }

    protected void OpenThread(object sender, EventArgs e)
    {
        try
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

            LinkButton link;
            Literal lit;
            Literal litBottom;

            DataView dvGroup = dat.GetDataDV("SELECT * FROM Groups WHERE ID=" + Request.QueryString["ID"].ToString());

            string colorB = "#" + dvGroup[0]["ColorB"].ToString();
            string textA = "#" + dvGroup[0]["TextA"].ToString();

            string notRead = "<div style=\"padding-left: 30px;";

            if (Session["User"] != null)
            {
                DataView dvRead = dat.GetDataDV("SELECT * FROM ThreadRead WHERE UserID=" + Session["User"].ToString() +
                    " AND ThreadID=" + Request.QueryString["ID"].ToString());
                if (dvRead.Count > 0)
                {
                    if (!bool.Parse(dvRead[0]["Read"].ToString()))
                    {
                        notRead = "<img style=\"float: left; padding-left: 8px;\" src=\"images/NewComment.png\" /><div style=\"padding-left: 5px;";
                    }
                }
            }

            foreach (Control ctrl in ThreadsPanel.Controls)
            {
                try
                {
                    link = (LinkButton)ctrl;
                    link.CssClass = "Thread";

                    notRead = "<div style=\"padding-left: 30px;";

                    if (Session["User"] != null)
                    {
                        DataView dvRead = dat.GetDataDV("SELECT * FROM ThreadRead WHERE UserID=" + Session["User"].ToString() +
                            " AND ThreadID=" + link.ID.Replace("link", ""));
                        if (dvRead.Count > 0)
                        {
                            if (!bool.Parse(dvRead[0]["Read"].ToString()))
                            {
                                notRead = "<img style=\"float: left; padding-left: 8px;\" src=\"images/NewComment.png\" /><div style=\"padding-left: 5px;";
                            }
                        }
                    }

                    

                    lit = (Literal)ThreadsPanel.Controls[ThreadsPanel.Controls.IndexOf(link) - 1];
                    litBottom = (Literal)ThreadsPanel.Controls[ThreadsPanel.Controls.IndexOf(link) + 1];
                    lit.Text = "<div style=\"margin-top: 10px;width: 284px;background-color: "+colorB+";float: left; clear: both;background-image: url(images/ThreadMiddle.png); " +
                        "background-repeat: repeat-y;\"><div style=\"background-color: " + colorB + ";height: 10px;background-image: url(images/ThreadTop.png);\"></div>"+notRead+"color: " + textA + ";padding-right: 15px; padding-bottom: 3px;\">";
                    litBottom.Text = "</div></div><div style=\"float: left;height: 15px;width: 284px;background-color: " + colorB + ";background-image: url(images/ThreadBottom.png)\"></div>";

                }
                catch (Exception ex)
                {

                }
            }

            notRead = "<div style=\"padding-left: 30px;";
            link = (LinkButton)sender;
            link.CssClass = "ThreadSelected";
            if (Session["User"] != null)
            {
                DataView dvRead = dat.GetDataDV("SELECT * FROM ThreadRead WHERE UserID=" + Session["User"].ToString() +
                    " AND ThreadID=" + link.ID.Replace("link", ""));
                if (dvRead.Count > 0)
                {
                    if (!bool.Parse(dvRead[0]["Read"].ToString()))
                    {
                        notRead = "<img style=\"float: left; padding-left: 8px;\" src=\"images/NewComment.png\" /><div style=\"padding-left: 5px;";
                    }
                }
            }

            lit = (Literal)ThreadsPanel.Controls[ThreadsPanel.Controls.IndexOf(link) - 1];
            litBottom = (Literal)ThreadsPanel.Controls[ThreadsPanel.Controls.IndexOf(link) + 1];
            lit.Text = "<div style=\"margin-top: 10px;width: 284px;background-color: " + colorB + ";float: left; clear: both;background-image: url(images/ThreadMiddleSelected.png); " +
                    "background-repeat: repeat-y;\"><div style=\"background-color: " + colorB + ";height: 9px;background-image: url(images/ThreadTopSelected.png);\"></div>" +
                    notRead + "color: " + textA + ";padding-right: 15px; padding-bottom: 2px;\">";
            litBottom.Text = "</div></div><div style=\"float: left;height: 14px;width: 284px;background-color: " + colorB + ";background-image: url(images/ThreadBottomSelected.png)\"></div>";


            DataView dvComms = dat.GetDataDV("SELECT *, GTC.ID AS CID FROM GroupThreads_Comments GTC, " +
                        "Users U WHERE U.User_ID=GTC.UserID AND GTC.ThreadID=" + link.ID.Replace("link", "") +
                        " ORDER BY GTC.PostedDate DESC");
            bool isLeft = false;
            FillThreadComments(link.ID.Replace("link", ""));

            if (Session["User"] != null)
            {
                dat.Execute("UPDATE ThreadRead SET [Read] = 'True' WHERE UserID=" + Session["User"].ToString() +
                    " AND ThreadID=" + link.ID.Replace("link", ""));

            
                DataView ThreadsRead = dat.GetDataDV("SELECT * FROM ThreadRead TH, GroupThreads GT WHERE TH.UserID=" +
                    Session["User"].ToString() + " AND TH.[Read] = 'False' AND GT.GroupID=" + Request.QueryString["ID"].ToString() +
                    " AND GT.ID=TH.ThreadID ");

                if (ThreadsRead.Count > 0)
                    RadTabStrip1.Tabs[1].Text = "<div id=\"BiggerNewDiv\"  style=\"float: left;display: block;font-weight: bold;color: #ff6b09;font-size: 12px;" +
                                "\"><div style=\"float: left;\">(</div><div style=\"float: left\" id=\"NewThreadDiv\">" +
                                ThreadsRead.Count.ToString() +
                                "</div>&nbsp;unread)</div>&nbsp;&nbsp;<span onclick='HideSocial()'>Stuff We Talk About</span>";
            }
        }
        catch (Exception ex)
        {
            ThreadErrorLabel.Text = ex.ToString();
        }
    }

    protected void CreatePostComment(string ThreadID)
    {
        Literal lit1 = new Literal();
        lit1.Text = "<div class=\"topDiv\" style=\"padding-bottom: 5px;\"><div style=\"float: " +
                    "right; clear: both;\">";
        CommentsPanel.Controls.Add(lit1);

        LinkButton l = new LinkButton();
        l.Text = "post a comment";
        l.CssClass = "AddLink";
        l.Click += new EventHandler(OpenComment);
        l.ID = "open" + ThreadID;

        CommentsPanel.Controls.Add(l);

        lit1 = new Literal();
        lit1.Text = "</div></div>";
        CommentsPanel.Controls.Add(lit1);

        Panel panel = new Panel();
        panel.ID = "commentPanel" + ThreadID;
        lit1 = new Literal();
        lit1.Text = "<div class=\"topDiv\" style=\"padding-bottom: 5px;\"><div style=\"float: " +
                    "right; clear: both;\">";
        panel.Controls.Add(lit1);

        TextBox commentText = new TextBox();
        commentText.ID = "commentText" + ThreadID;
        commentText.TextMode = TextBoxMode.MultiLine;
        commentText.Width = 200;
        commentText.Height = 100;
        panel.Controls.Add(commentText);

        lit1 = new Literal();
        lit1.Text = "<br/>";
        panel.Controls.Add(lit1);

        Button b = new Button();
        b.Click += new EventHandler(PostComment);
        b.ID = "postComment" + ThreadID;
        b.Text = "Post";
        b.CssClass = "SearchButton";
        b.Attributes.Add("onmouseout", "this.style.backgroundImage='url(image/PostButtonNoPost.png)'");
        b.Attributes.Add("onmouseover", "this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'");
        panel.Controls.Add(b);

        Button c = new Button();
        c.Click += new EventHandler(CancelComment);
        c.ID = "cancelComment" + ThreadID;
        c.Text = "Cancel";
        c.CssClass = "SearchButton";
        c.Attributes.Add("onmouseout", "this.style.backgroundImage='url(image/PostButtonNoPost.png)'");
        c.Attributes.Add("onmouseover", "this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'");
        panel.Controls.Add(c);

        lit1 = new Literal();
        lit1.Text = "</div></div>";
        panel.Controls.Add(lit1);
        panel.Visible = false;

        CommentsPanel.Controls.Add(panel);
    }

    protected Panel GetThreadComment(object profileThumb,
        string userLabel, string postedOn, string comment, 
        bool isLeft, string commentID)
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

        DataView dvGroup = dat.GetDataDV("SELECT * FROM Groups WHERE ID=" + Request.QueryString["ID"].ToString());
        string sndcolor = dvGroup[0]["ColorB"].ToString();
        string fstcolor = dvGroup[0]["ColorA"].ToString();

        Panel thePanel = new Panel();
        Literal lit = new Literal();
        DataView dvComms = dat.GetDataDV("SELECT * FROM GroupThreads_Comments WHERE ID=" + commentID);

        string imageOrVid = "";
        if (dvComms[0]["Image"] != null)
        {
            if (dvComms[0]["Image"].ToString() != "")
            {
                imageOrVid = "<div style=\"float: right; padding: 10px;margin: 3px; margin-right: 0px; border: solid 1px #" + fstcolor + ";" +
                    "background-color: #1b1b1b;\">" +
                    "<img style=\"border: solid 1px #" + sndcolor + ";\" align=\"middle\"  src=\"GroupFiles/" +
                    Request.QueryString["ID"].ToString() + "/Slider/" +
                    dvComms[0]["Image"].ToString() + "\" /></div>";
            }
        }

        if (dvComms[0]["YouTube"] != null)
        {
            if (dvComms[0]["YouTube"].ToString() != "")
            {
                imageOrVid = "<div style=\"float:right; padding: 10px;margin: 3px; margin-right: 0px; border: solid 1px #" + fstcolor + ";" +
                    "background-color: #1b1b1b;\"><div style=\"border: solid 1px #" + sndcolor + ";\" ><object width=\"400\" height=\"250\">" +
                    "<param  name=\"wmode\" value=\"opaque\" ></param><param name=\"movie\" " +
                    "value=\"http://www.youtube.com/v/" + dvComms[0]["YouTube"].ToString() +
                    "\"></param><param name=\"allowFullScreen\" value=\"true\"></param>" +
                    "<embed wmode=\"opaque\" src=\"http://www.youtube.com/v/" + dvComms[0]["YouTube"].ToString() +
                    "\" type=\"application/x-shockwave-flash\" " +
                    "allowfullscreen=\"true\" width=\"400\" height=\"250\"></embed></object></div></div>";
            }
        }

        lit.Text = "<div style=\"padding-bottom: 10px; width: 530px;\">" +
            "<div class=\"commentTop\" style=\"background-image: url('images/BigCommentTop.png'); " +
            "background-repeat: no-repeat;height: 5px;\">" +
            "&nbsp;" +
            "</div>" +
            "<div id=\"topDiv\" style=\"font-size: 11px; font-family: Arial; color: White; line-height: " +
            "20px ;padding-left: 10px; " +
            "padding-right: 10px ; padding-bottom: 0px; background-color: #666666; " +
            "background-repeat: repeat-y;\">" +
            "<div style=\"float: left; padding-top: 10px; margin-top: -5px;width: 72px;\">" +
            "<table align=\"center\" onmouseout=\"this.style.backgroundColor = '#666666';\" " +
            "onmouseover=\"this.style.backgroundColor = '#a5c13a';\" valign=\"middle\" cellpadding=\"0\" " +
            "cellspacing=\"1\"  bgcolor=\"#666666\">" +
            "<tr>" +
            "<td align=\"center\" valign=\"middle\">" +
            "<table width=\"52px\" align=\"center\" height=\"52px\" bgcolor=\"#333333\" " +
            "cellpadding=\"0\" cellspacing=\"0\">" +
            "<tr>" +
            "<td align=\"center\">";

        thePanel.Controls.Add(lit);

        ImageButton ProfileImage = new ImageButton();
        ProfileImage.ID = "comment" + commentID;
        if (profileThumb == null)
            profileThumb = "~/image/noAvatar_50x50_small.png";
        if(profileThumb.ToString().Trim() == "")
            profileThumb = "~/image/noAvatar_50x50_small.png";
        ProfileImage.PostBackUrl = "~/" + userLabel + "_Friend";

        if (profileThumb.ToString().Equals("~/image/noAvatar_50x50_small.png"))
        {
            ProfileImage.Attributes.Add("onmouseover", "this.src='NewImages/noAvatar_50x50_smallhover.png'");
            ProfileImage.Attributes.Add("onmouseout", "this.src='image/noAvatar_50x50_small.png'");
            ProfileImage.ImageUrl = "~/image/noAvatar_50x50_small.png";
        }
        else
        {
            ProfileImage.ImageUrl = "~/UserFiles/" + userLabel + "/Profile/" + profileThumb.ToString();

            System.Drawing.Image theimg = System.Drawing.Image.FromFile(Server.MapPath("/UserFiles/" +
                userLabel + "/Profile/" + profileThumb.ToString()));

            double width = double.Parse(theimg.Width.ToString());
            double height = double.Parse(theimg.Height.ToString());

            if (width > height)
            {
                if (width <= 50)
                {

                }
                else
                {
                    double dividor = double.Parse("50.00") / double.Parse(width.ToString());
                    width = double.Parse("50.00");
                    height = height * dividor;
                }
            }
            else
            {
                if (width == height)
                {
                    width = double.Parse("50.00");
                    height = double.Parse("50.00");
                }
                else
                {
                    double dividor = double.Parse("50.00") / double.Parse(height.ToString());
                    height = double.Parse("50.00");
                    width = width * dividor;
                }
            }

            ProfileImage.Width = int.Parse((Math.Round(decimal.Parse(width.ToString()))).ToString());
            ProfileImage.Height = int.Parse((Math.Round(decimal.Parse(height.ToString()))).ToString());
        }
        thePanel.Controls.Add(ProfileImage);
        lit = new Literal();
        lit.Text = "</td></tr></table></td></tr></table></div>" +
            "Posted on <span class=\"CommentLabel\">" + postedOn + "</span><br />" +
            "Posted by <span class=\"CommentLabel\">" + userLabel + "</span><br />" + imageOrVid +
            comment + "</div><div>";

        if (isLeft)
            lit.Text += "<img src=\"images/BigCommentBottomLeft.png\" />";
        else
            lit.Text += "<img src=\"images/BigCommentBottom.png\" />";

        lit.Text += "</div></div>";
        thePanel.Controls.Add(lit);

        return thePanel;
    }

    protected void PostComment(object sender, EventArgs e)
    {
        Button b = (Button)sender;
        TextBox SubjectTextBox = (TextBox)CommentsPanel.FindControl("commentText" + b.ID.Replace("cancelComment", ""));
        if (SubjectTextBox.Text.Trim() != "")
        {
            string ThreadID = b.ID.Replace("cancelComment", "");
            HttpCookie cookie = Request.Cookies["BrowserDate"];
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            SqlConnection conn = dat.GET_CONNECTED;
            SqlCommand cmd;

            cmd = new SqlCommand("INSERT INTO GroupThreads_Comments (ThreadID, UserID, PostedDate, [Content])" +
                " VALUES(@tid, @userID, @DatePosted, @content)", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@content", SqlDbType.NVarChar).Value = SubjectTextBox.Text.Trim();
            cmd.Parameters.Add("@DatePosted", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
            cmd.Parameters.Add("@userID", SqlDbType.Int).Value = Session["User"].ToString();
            cmd.Parameters.Add("@tid", SqlDbType.Int).Value = ThreadID;
            cmd.ExecuteNonQuery();

            FillThreadComments(ThreadID);

        }
        else
        {
        }
    }

    protected void FillThreadComments(string ThreadID)
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

        DataView dvComms = dat.GetDataDV("SELECT *, GTC.ID AS CID FROM GroupThreads_Comments GTC, " +
                       "Users U WHERE U.User_ID=GTC.UserID AND GTC.ThreadID=" + ThreadID +
                       " ORDER BY GTC.PostedDate DESC");
        string addIt = "";

        if (Session["User"] != null)
        {
            DataView dvThr = dat.GetDataDV("SELECT * FROM GroupThreads WHERE ID=" + ThreadID);

            if (bool.Parse(Session["isHost"].ToString()) || bool.Parse(Session["isMember"].ToString()))
            {
                if (bool.Parse(Session["isHost"].ToString()) && dvThr[0]["StartedBy"].ToString() == Session["User"].ToString())
                {
                    if (bool.Parse(dvThr[0]["Active"].ToString()))
                    {
                        addIt = "<div style=\"float: " +
                        "right;padding-left: 10px;\"><a class=\"AddLink\" onclick=\"OpenThreadAdmin('" + ThreadID +
                        "', 'I');\">make thread inactive</a></div>";
                    }
                    else
                    {
                        addIt = "<div style=\"float: " +
                           "right;padding-left: 10px;\"><a class=\"AddLink\" onclick=\"OpenThreadAdmin('" + ThreadID +
                           "', 'A');\">make thread active</a></div>";
                    }
                    if (bool.Parse(dvThr[0]["Hidden"].ToString()))
                    {
                        addIt += "<div style=\"float: " +
                        "right;padding-left: 10px;\"><a class=\"AddLink\" onclick=\"OpenThreadAdmin('" + ThreadID +
                        "', 'S');\">show thread</a></div>";
                    }
                    else
                    {
                        addIt += "<div style=\"float: " +
                        "right;padding-left: 10px;\"><a class=\"AddLink\" onclick=\"OpenThreadAdmin('" + ThreadID +
                        "', 'H');\">hide thread</a></div>";
                    }
                    addIt += "<div style=\"padding-left: 10px;float: " +
                    "right;\"><a class=\"AddOrangeLink\" onclick=\"OpenThreadAdmin('" + ThreadID +
                    "', 'D');\">delete thread</a></div>";
                }
                else
                {
                    if (!bool.Parse(dvThr[0]["Active"].ToString()))
                    {
                        addIt = "<div style=\"float: " +
                               "right;padding-left: 10px;\"><a class=\"AddOrangeLink\">this thread is inactive</a></div>";
                    }
                }

            }

            if (bool.Parse(dvThr[0]["Active"].ToString())&& (bool.Parse(Session["isHost"].ToString()) || bool.Parse(Session["isMember"].ToString())))
            {
                addIt += "<div style=\"float: " +
                "right;padding-left: 10px;\"><a class=\"AddLink\" onclick=\"OpenComment('" +
                Request.QueryString["ID"].ToString() + "', '" + ThreadID +
                "');\">post a comment</a></div>";
            }

            Literal lit1 = new Literal();
            lit1.Text = "<div class=\"topDiv\" style=\"padding-bottom: 5px; height: 25px;clear: both;\">" + addIt + "</div>";
            CommentDoButton.CommandArgument = ThreadID;
            CommentsPanel.Controls.Add(lit1);
            //FillThreadComments(ThreadID);
            //CreatePostComment(ThreadID);
        }
        bool isLeft = true;
        foreach (DataRowView comment in dvComms)
        {
            CommentsPanel.Controls.Add(GetThreadComment(comment["ProfilePicture"],
                comment["UserName"].ToString(), comment["PostedDate"].ToString(),
                comment["Content"].ToString(), isLeft, comment["CID"].ToString()));
            isLeft = !isLeft;
        }
    }

    protected void OpenComment(object sender, EventArgs e)
    {
        LinkButton link = (LinkButton)sender;
        Panel panel = (Panel)CommentsPanel.FindControl("commentPanel" + link.ID.Replace("open", ""));
        panel.Visible = true;
        FillThreadComments(link.ID.Replace("cancelComment", ""));

    }

    protected void CancelComment(object sender, EventArgs e)
    {
        Button link = (Button)sender;
        Panel panel = (Panel)CommentsPanel.FindControl("commentPanel" + link.ID.Replace("cancelComment", ""));
        panel.Visible = false;
        FillThreadComments(link.ID.Replace("cancelComment", ""));
    }

    protected void FillC(object sender, EventArgs e)
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

        string threadID = ((LinkButton)sender).CommandArgument.ToString();
        LinkButton link = (LinkButton)ThreadsPanel.FindControl("link" + threadID);
        link.Text = link.Text.Replace("<div style=\"float: left;\"><table><tr><td valign=\"top\">+ </td>", "<div style=\"float: left;\"><table><tr><td style=\"padding-left: 10px;\" valign=\"top\">- </td>");
        link.CssClass = "ThreadSelected";

        DataView dvGroup = dat.GetDataDV("SELECT * FROM Groups WHERE ID=" + Request.QueryString["ID"].ToString());

        link.ForeColor = System.Drawing.ColorTranslator.FromHtml("#" + dvGroup[0]["TextA"].ToString());

        FillThreadComments(threadID);
    }

    protected void AdvanceMonth(object sender, EventArgs e)
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

        RadScheduler1.SelectedDate = RadScheduler1.SelectedDate.AddMonths(1);
        MonthLabel.Text = dat.GetMonth(RadScheduler1.SelectedDate.Month.ToString()) + " " + RadScheduler1.SelectedDate.Year.ToString();
    }

    protected void DevanceMonth(object sender, EventArgs e)
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

        RadScheduler1.SelectedDate = RadScheduler1.SelectedDate.AddMonths(-1);
        MonthLabel.Text = dat.GetMonth(RadScheduler1.SelectedDate.Month.ToString()) + " " + RadScheduler1.SelectedDate.Year.ToString();
    }

    protected void SortThreads(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }


        bool isHost = false;
        if (Session["isHost"] != null)
            if (bool.Parse(Session["isHost"].ToString()))
                isHost = true;

        string addthis = "";
        if (!isHost)
            addthis = " AND Hidden='False'";

        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataView dvThreads = dat.GetDataDV("SELECT * FROM GroupThreads WHERE GroupID=" +
                    Request.QueryString["ID"].ToString() + addthis + " ORDER BY StartDate DESC");
        DataView dvT2;
        DataView dvAll = new DataView();
        DataView dvGroup = dat.GetDataDV("SELECT * FROM Groups WHERE ID=" + Request.QueryString["ID"].ToString());
        

        if (SortDropDown.SelectedValue != "-1")
        {
            string orderBy = "";
            switch (SortDropDown.SelectedValue)
            {
                case "1":
                    orderBy = " GT.ThreadName ";
                    dvThreads = dat.GetDataDV("SELECT DISTINCT GT.ID, GT.ThreadName, GT.StartDate FROM GroupThreads GT WHERE GT.GroupID=" +
                            Request.QueryString["ID"].ToString() + addthis+" ORDER BY " + orderBy + " ASC");
                    dvAll = dvThreads;
                    break;
                case "2":
                    orderBy = " GT.StartDate ";
                    dvThreads = dat.GetDataDV("SELECT DISTINCT GT.ID, GT.ThreadName, GT.StartDate FROM GroupThreads GT WHERE GT.GroupID=" +
                            Request.QueryString["ID"].ToString() + addthis+" ORDER BY " + orderBy + " ASC");
                    dvAll = dvThreads;
                    break;
                case "3":
                    dvAll = new DataView();
                    dvThreads = dat.GetDataDV("SELECT * FROM GroupThreads WHERE GroupID=" + 
                        Request.QueryString["ID"].ToString() + addthis);
                    string notThis = "";

                    dvAll = dat.GetDataDV("SELECT MAX(GTC.PostedDate) AS MaxDate, GT.ID, GT.ThreadName, GT.StartDate FROM GroupThreads GT, GroupThreads_Comments GTC " +
                        "WHERE GT.ID=GTC.ThreadID " + addthis + " AND GT.GroupID=" +
                        Request.QueryString["ID"].ToString() +
                        " GROUP BY GT.ID, GT.ThreadName, GT.StartDate ORDER BY MaxDate DESC ");

                    dvThreads = dat.GetDataDV("SELECT * FROM GroupThreads GT WHERE GT.GroupID=" +
                        Request.QueryString["ID"].ToString() + addthis + " AND NOT EXISTS (SELECT * FROM " +
                        "GroupThreads_Comments GTC WHERE GT.ID=GTC.ThreadID)");
                    
                    if(dvThreads.Count > 0)
                        dvAll = MergeDV(dvAll, dvThreads);
                    
                    break;
                default: break;
            }
        }
        else
        {
            dvAll = dvThreads;
        }

        LinkButton link;
        Literal lit;
        Literal litBottom;
        bool foundFirst = false;
        string classS = "Thread";
        bool isLeft = true;
        ThreadsPanel.Controls.Clear();
        CommentsPanel.Controls.Clear();

        DataView dvGroup2 = dat.GetDataDV("SELECT * FROM Groups WHERE ID=" + Request.QueryString["ID"].ToString());

        string colorB = "#" + dvGroup2[0]["ColorB"].ToString();
        string textA = "#" + dvGroup2[0]["TextA"].ToString();

        string notRead = "<div style=\"padding-left: 30px;";

        

        if (dvAll.Count > 0)
        {
            foreach (DataRowView row in dvAll)
            {
                notRead = "<div style=\"padding-left: 30px;";
                link = new LinkButton();
                if (Session["User"] != null)
                {
                    DataView dvRead = dat.GetDataDV("SELECT * FROM ThreadRead WHERE UserID=" + Session["User"].ToString() +
                        " AND ThreadID=" + row["ID"].ToString());
                    if (dvRead.Count > 0)
                    {
                        if (!bool.Parse(dvRead[0]["Read"].ToString()))
                        {
                            notRead = "<img style=\"float: left; padding-left: 8px;\" src=\"images/NewComment.png\" /><div style=\"padding-left: 5px;";
                            link.Attributes.Add("onclick", "DecrementNewComment();ShowLoading();");
                        }
                        else
                        {
                            link.Attributes.Add("onclick", "ShowLoading();");
                        }
                    }
                    else
                    {
                        link.Attributes.Add("onclick", "ShowLoading();");
                    }
                }

                
                

                lit = new Literal();
                litBottom = new Literal();
                if (!foundFirst && !IsPostBack)
                {
                    lit.Text = "<div style=\"margin-top: 10px;width: 284px;background-color: "+colorB+";float: left; clear: both;background-image: url(images/ThreadMiddleSelected.png); " +
                        "background-repeat: repeat-y;\"><div style=\"background-color: "+colorB+";height: 9px;background-image: url(images/ThreadTopSelected.png);\"></div>"+
                        notRead + " padding-right: 15px; padding-bottom: 2px;color: " + textA + ";\">";
                    litBottom.Text = "</div></div><div style=\"float: left;height: 14px;width: 284px;background-color: " + colorB + ";background-image: url(images/ThreadBottomSelected.png)\"></div>";
                    classS = "ThreadSelected";
                    //link.Text = "<div style=\"float: left; width: 300px;\"><table><tr><td style=\"padding-left: 10px;\" valign=\"top\">- </td>";

                    foundFirst = true;
                    //show first thread's comments
                    //Literal lit1 = new Literal();
                    //lit1.Text = "<div class=\"topDiv\" style=\"padding-bottom: 5px;\"><div style=\"float: " +
                    //    "right; clear: both;\"><a class=\"AddLink\" onclick=\"OpenComment('" +
                    //    Request.QueryString["ID"].ToString() + "', '" + row["ID"].ToString() +
                    //    "');\">post a comment</a></div></div>";
                    //CommentsPanel.Controls.Add(lit1);
                    FillThreadComments(row["ID"].ToString());
                    //if (Session["User"] != null)
                    //{
                    //    dat.Execute("UPDATE ThreadRead SET [Read] = 'True' WHERE UserID=" + Session["User"].ToString() +
                    //        " AND ThreadID=" + row["ID"].ToString());
                    //}

                    if (Session["User"] != null)
                    {
                        DataView ThreadsRead = dat.GetDataDV("SELECT * FROM ThreadRead TH, GroupThreads GT WHERE TH.UserID=" +
                            Session["User"].ToString() + " AND TH.[Read] = 'False' AND GT.GroupID=" + Request.QueryString["ID"].ToString() +
                            " AND GT.ID=TH.ThreadID ");

                        if (ThreadsRead.Count > 0)
                            RadTabStrip1.Tabs[1].Text = "<div id=\"BiggerNewDiv\"  style=\"float: left;display: block;font-weight: bold;color: #ff6b09;font-size: 12px;" +
                                "\"><div style=\"float: left;\">(</div><div style=\"float: left\"  id=\"NewThreadDiv\">" + 
                                ThreadsRead.Count.ToString() + 
                                "</div>&nbsp;unread)</div>&nbsp;&nbsp;<span onclick='HideSocial()'>Stuff We Talk About</span>";
                    }
                }
                else
                {
                    lit.Text = "<div style=\"margin-top: 10px;width: 284px;background-color: " + colorB + ";float: left; clear: both;background-image: url(images/ThreadMiddle.png); " +
                        "background-repeat: repeat-y;\"><div style=\"background-color: " + colorB + ";height: 10px;background-image: url(images/ThreadTop.png);\"></div>" + notRead +
                        " padding-right: 15px; padding-bottom: 3px;color: " + textA + ";\">";
                    litBottom.Text = "</div></div><div style=\"float: left;height: 15px;width: 284px;background-color: " + colorB + ";background-image: url(images/ThreadBottom.png)\"></div>";
                    classS = "Thread";
                    //link.Text = "<div style=\"float: left; width: 300px;\"><table><tr><td valign=\"top\">+ </td>";
                }

                ThreadsPanel.Controls.Add(lit);

                link.CssClass = classS;
                link.ForeColor = System.Drawing.ColorTranslator.FromHtml("#" + dvGroup[0]["TextA"].ToString());

                //link.Text += "<td valign=\"top\">" +
                //    row["ThreadName"].ToString() + "</td></tr></table></div>";
                link.Text += row["ThreadName"].ToString() + " (Created On: " + DateTime.Parse(row["StartDate"].ToString()).ToShortDateString() + ")";
                link.Click += new EventHandler(OpenThread);
                link.ID = "link" + row["ID"].ToString();

                ThreadsPanel.Controls.Add(link);

                ThreadsPanel.Controls.Add(litBottom);
            }
        }
        else
        {
            SortPanel.Visible = false;
            lit = new Literal();
            lit.Text = "<div class=\"MemberDescription\">No discussion threads have been posted for this group.</div>";
            ThreadsPanel.Controls.Add(lit);
        }

    }

    protected DataView MergeDV(DataView dvThreads, DataView dvT2)
    {
        DataTable dt = dvThreads.Table.Copy();
        DataRow newRow;
        foreach (DataRowView row in dvT2)
        {
            newRow = dt.NewRow();
            newRow["ThreadName"] = row["ThreadName"].ToString();
            newRow["ID"] = row["ID"].ToString();
            newRow["StartDate"] = row["StartDate"].ToString();
            dt.Rows.Add(newRow);
        }

        return new DataView(dt, "", "", DataViewRowState.CurrentRows);
    }
}
