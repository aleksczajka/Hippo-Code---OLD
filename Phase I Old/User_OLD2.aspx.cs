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
using System.Drawing.Imaging;


public partial class User : Telerik.Web.UI.RadAjaxPage
{
    public string UserName;
    
    public int UserID;

    System.Drawing.Color greyText = System.Drawing.Color.FromArgb(102, 102, 102);
    System.Drawing.Color greyBack = System.Drawing.Color.FromArgb(51, 51, 51);
    System.Drawing.Color greyDark = System.Drawing.Color.FromArgb(27, 27, 27);
    System.Drawing.Color greyBorder = System.Drawing.Color.FromArgb(54, 54, 54);

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
        DoAll();

        //Request.Browser.Browser = "Netscape";
        
        if (!IsPostBack)
        {
            //Categories have to be set from Page_Load not Page_Init for some reason. Otherwise it doesn't work.
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            //Set Ad categories
            Telerik.Web.UI.RadTreeView CategoryTree = (Telerik.Web.UI.RadTreeView)AdCategoryRadPanel.Items[0].Items[0].FindControl("CategoryTree");
            Telerik.Web.UI.RadTreeView RadTreeView2 = (Telerik.Web.UI.RadTreeView)AdCategoryRadPanel.Items[0].Items[0].FindControl("RadTreeView2");
            CategoryTree.DataBind();
            //RadTreeView1.DataBind();
            RadTreeView2.DataBind();
            //RadTreeView3.DataBind();

            DataSet dsCategories = dat.GetData("SELECT * FROM UserCategories UC, AdCategories AC WHERE UC.UserID=" +
                Session["User"].ToString() + " AND UC.CategoryID=AC.ID");
            FillCategories(dsCategories, ref CategoryTree);
            FillCategories(dsCategories, ref RadTreeView2);


            //Set Event categories
            Telerik.Web.UI.RadTreeView RadTreeView1 =
                (Telerik.Web.UI.RadTreeView)EventPanelBar.Items[0].Items[0].FindControl("RadTreeView1");
            Telerik.Web.UI.RadTreeView RadTreeView3 =
                (Telerik.Web.UI.RadTreeView)EventPanelBar.Items[0].Items[0].FindControl("RadTreeView3");
            RadTreeView1.DataBind();
            RadTreeView3.DataBind();

            dsCategories = dat.GetData("SELECT * FROM UserEventCategories UEC, EventCategories EC WHERE UEC.UserID=" +
                Session["User"].ToString() + " AND UEC.CategoryID=EC.ID");
            FillCategories(dsCategories, ref RadTreeView1);
            FillCategories(dsCategories, ref RadTreeView3);

            //Image1.Attributes.Add("onclick", "goEventNext()");
            //Master.BodyTag.Attributes.Add("onload", "startUpScripts()");

            //Master.theScriptManager.Scripts.Add(new ScriptReference("Telerik.Web.UI.Common.Core.js", "Telerik.Web.UI"));
        }
    }

    protected void FillGroups()
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

        DataView dvUsersGroups = dat.GetDataDV("SELECT * FROM Group_Members GM, Groups G WHERE "+
            "GM.GroupID=G.ID AND GM.Accepted='True' AND GM.MemberID=" + Session["User"].ToString() +" ORDER BY GM.ID");

        Literal lit;
        CheckBox check;
        string sharedHost = "";

    
        if (dvUsersGroups.Count > 0)
        {
            //Check for New Group Messages Since last Sign on
            GetGroupMessages();

            foreach (DataRowView row in dvUsersGroups)
            {
                lit = new Literal();

                sharedHost = "";
                if (bool.Parse(row["SharedHosting"].ToString()))
                    sharedHost = "<label> | Shared Host</label>";
                if (row["Host"].ToString() == Session["User"].ToString())
                    sharedHost = "<label> | Primary Host</label>";

                if (sharedHost == "")
                    sharedHost = "<label> | Member</label>";

                lit.Text = "<div style=\"padding-top: 10px;\"><a href=\"" + dat.MakeNiceName(row["Header"].ToString()) +
                    "_" + row["GroupID"].ToString() + "_Group\" class=\"AddGreenLink\">" + row["Header"].ToString() +
                    "</a><label> | </label><a class=\"AddLink\" onclick=\"OpenRevoke('" + row["GroupID"].ToString() +
                    "')\">Revoke group membership</a>" + sharedHost + "</div><div style=\"padding-left: 20px;\">";
                GroupsPanel.Controls.Add(lit);

                check = new CheckBox();
                check.Text = "Receive emails when new group thread is posted.";
                check.ID = "thread" + row["GroupID"].ToString();
                if (row["Prefs"] != null)
                {
                    if (row["Prefs"].ToString().Trim() != "")
                    {
                        if (row["Prefs"].ToString().Trim().Contains("1"))
                            check.Checked = true;
                    }
                }
                GroupsPanel.Controls.Add(check);

                lit = new Literal();
                lit.Text = "<br/>";
                GroupsPanel.Controls.Add(lit);

                check = new CheckBox();
                check.Text = "Receive emails if new events have been posted in the group.";
                check.ID = "newevent" + row["GroupID"].ToString();
                GroupsPanel.Controls.Add(check);

                if (row["Prefs"] != null)
                {
                    if (row["Prefs"].ToString().Trim() != "")
                    {

                        if (row["Prefs"].ToString().Trim().Contains("2"))
                            check.Checked = true;
                    }
                }

                if (row["Host"].ToString() == Session["User"].ToString())
                {
                    lit = new Literal();
                    lit.Text = "<br/>";
                    GroupsPanel.Controls.Add(lit);

                    check = new CheckBox();
                    check.Text = "Receive emails when users request to be part of the group.";
                    check.ID = "joinButton" + row["GroupID"].ToString();
                    GroupsPanel.Controls.Add(check);

                    if (row["Prefs"] != null)
                    {
                        if (row["Prefs"].ToString().Trim() != "")
                        {

                            if (row["Prefs"].ToString().Trim().Contains("3"))
                                check.Checked = true;
                        }
                    }
                }

                lit = new Literal();
                lit.Text = "<br/>";
                GroupsPanel.Controls.Add(lit);

                check = new CheckBox();
                check.Text = "Receive emails if new messages have been posted on the group board.";
                check.ID = "board" + row["GroupID"].ToString();
                GroupsPanel.Controls.Add(check);

                if (row["Prefs"] != null)
                {
                    if (row["Prefs"].ToString().Trim() != "")
                    {

                        if (row["Prefs"].ToString().Trim().Contains("4"))
                            check.Checked = true;
                    }
                }

                lit = new Literal();
                lit.Text = "<br/>";
                GroupsPanel.Controls.Add(lit);

                check = new CheckBox();
                check.Text = "Receive emails if when new comment has been posted on a thread.";
                check.ID = "comment" + row["GroupID"].ToString();
                GroupsPanel.Controls.Add(check);

                if (row["Prefs"] != null)
                {
                    if (row["Prefs"].ToString().Trim() != "")
                    {

                        if (row["Prefs"].ToString().Trim().Contains("5"))
                            check.Checked = true;
                    }
                }

                lit = new Literal();
                lit.Text = "</div>";

                GroupsPanel.Controls.Add(lit);
            }

            lit = new Literal();
            lit.Text = "<div style=\"float: right;\">";
            GroupsPanel.Controls.Add(lit);
            
            HtmlButton img = new HtmlButton();
            img.ID = "groupButton" + Session["User"].ToString();
            img.Style.Value = "cursor: pointer;margin-top: 20px; margin-left: 50px;padding-bottom: 4px;height: 30px; width: 112px;background-color: transparent; " +
            "color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat: " +
            "no-repeat; border: 0;";
            img.ServerClick += new EventHandler(SaveGroupsPrefs);
            img.Attributes.Add("onmouseover", "this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';");
            img.Attributes.Add("onmouseout", "this.style.backgroundImage = 'url(image/PostButtonNoPost.png)';");
            img.Attributes.Add("onclick", "this.innerHTML = 'Working...';this.disabled=true;this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';");
            img.InnerHtml = "Save";

            GroupsPanel.Controls.Add(img);

            lit = new Literal();
            lit.Text = "</div>";
            GroupsPanel.Controls.Add(lit);
        }
        else
        {
            lit = new Literal();
            lit.Text = "<label>You are not a member of any groups.</label>";
            GroupsPanel.Controls.Add(lit);
        }
    }

    protected void GetGroupMessages()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }

        DateTime dtToday = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));

        Data dat = new Data(dtToday);

        DateTime dtm = dtToday.AddDays(double.Parse("-10.00"));

            //User's group/group event messages
        DataView dvUsersGroupMessages = dat.GetDataDV("SELECT G.ID, GMS.Content, G.Header, GMS.DatePosted AS TheDate FROM Group_Members GM, Groups G, " +
            "GroupMessages GMS WHERE GM.GroupID=G.ID AND GM.Accepted='True' AND GM.MemberID=" +
            Session["User"].ToString() + " AND GMS.UserID <> " + Session["User"].ToString() +
            " AND GMS.GroupID=G.ID AND " +
            "GMS.DatePosted > CONVERT(NVARCHAR, '" + dtm.Month.ToString() + "/" +
            dtm.Day.ToString() + "/" + dtm.Year.ToString() + "') ORDER BY GMS.DatePosted DESC");

            DataView dvGroupEventMessages = dat.GetDataDV("SELECT GEM.ID AS GEMID, GEM.Content, E.Name, GEM.DatePosted AS TheDate, E.ID FROM GroupEvent_Members UC, GroupEvents E, " +
            " GroupEventMessages GEM WHERE GEM.GroupEventID=E.ID AND " +
            "UC.GroupEventID=E.ID AND GEM.UserID <> " + Session["User"].ToString() + " AND UC.Accepted = " +
            "'True' AND UC.UserID=" + Session["User"].ToString() + " AND GEM.DatePosted > CONVERT(NVARCHAR, '" + dtm.Month.ToString() + "/" +
                dtm.Day.ToString() + "/" + dtm.Year.ToString() + "') ORDER BY GEM.DatePosted DESC");

            DataView dvGroupEventMessagesNonMember = dat.GetDataDV("SELECT GEM.ID AS GEMID, GEM.Content, E.Name, "+
                "GEM.DatePosted AS TheDate, E.ID  FROM User_GroupEvent_Calendar UC, GroupEvents E, " +
                " GroupEventMessages GEM WHERE GEM.GroupEventID=E.ID " +
                "AND UC.GroupEventID=E.ID AND GEM.UserID <> " + Session["User"].ToString() + " AND UC.UserID=" + Session["User"].ToString() + " AND GEM.DatePosted > CONVERT(NVARCHAR, '" + dtm.Month.ToString() + "/" +
                dtm.Day.ToString() + "/" + dtm.Year.ToString() + "') ORDER BY GEM.DatePosted DESC");

            DataView dvGroupThreads = dat.GetDataDV("SELECT GT.ID AS TheID, " +
                "GT.StartDate AS TheDate, GT.ThreadName " +
                    "FROM GroupThreads GT, Groups G, Group_Members GM WHERE GM.MemberID= " +Session["User"].ToString()+
                    " AND GM.GroupID=G.ID AND G.ID=GT.GroupID AND GT.StartedBy <> " + Session["User"].ToString() +
                    " AND G.isPrivate = 'False' ORDER BY GT.StartDate DESC");

            DataView dvGroupThreadComments = dat.GetDataDV("SELECT GTC.UserID, GTC.ID AS TheID, GTC.PostedDate AS " +
                "TheDate, GTC.Content FROM GroupThreads GT, GroupThreads_Comments GTC, Groups G, Group_Members GM " +
                    " WHERE GM.MemberID= " + Session["User"].ToString() +
                    " AND GM.GroupID=G.ID AND GT.GroupID=G.ID AND GT.ID=GTC.ThreadID AND GTC.UserID <> " + Session["User"].ToString() +
                    " AND G.isPrivate = 'False' " +
                    "ORDER BY GTC.PostedDate DESC");

            Literal lit = new Literal();

            int allCount = dvGroupEventMessages.Count + dvUsersGroupMessages.Count +
                dvGroupEventMessagesNonMember.Count + dvGroupThreads.Count + dvGroupThreadComments.Count;

            string content = "";
            int count1 = 0;
            int count2 = 0;
            int count3 = 0;
            int count4 = 0;
            int count5 = 0;
            DateTime dt1 = new DateTime();
            DateTime dt2 = new DateTime();
            DateTime dt3 = new DateTime();
            DateTime dt4 = new DateTime();
            DateTime dt5 = new DateTime();
            bool use1 = false;
            bool use2 = false;
            bool use3 = false;
            bool use4 = false;
            bool use5 = false;
            string idToUse = "";
            if (allCount > 0)
            {
                YourGroupsLabel.Text = "Your Lastest Group Messages";
                GroupUpdatesPanel.Visible = true;

                Hashtable hash = new Hashtable();

                for (int i = 0; i < allCount; i++)
                {
                    if (dvUsersGroupMessages.Count - 1 >= count1)
                    {
                        dt1 = DateTime.Parse(dvUsersGroupMessages[count1]["TheDate"].ToString());
                        use1 = true;
                    }
                    else
                        use1 = false;

                    if (dvGroupEventMessages.Count - 1 >= count2)
                    {
                        dt2 = DateTime.Parse(dvGroupEventMessages[count2]["TheDate"].ToString());
                        use2 = true;
                    }
                    else
                        use2 = false;

                    if (dvGroupEventMessagesNonMember.Count - 1 >= count3)
                    {
                        dt3 = DateTime.Parse(dvGroupEventMessagesNonMember[count3]["TheDate"].ToString());
                        use3 = true;
                    }
                    else
                        use3 = false;

                    if (dvGroupThreads.Count - 1 >= count4)
                    {
                        dt4 = DateTime.Parse(dvGroupThreads[count4]["TheDate"].ToString());
                        use4 = true;
                    }
                    else
                    {
                        use4 = false;
                    }

                    if (dvGroupThreadComments.Count - 1 >= count5)
                    {
                        dt5 = DateTime.Parse(dvGroupThreadComments[count5]["TheDate"].ToString());
                        use5 = true;
                    }
                    else
                    {
                        use5 = false;
                    }

                    idToUse = GetIDToUse(use1, use2, use3, use4, use5, dt1, dt2, dt3, dt4, dt5);

                    switch (idToUse)
                    {
                        case "1":
                            content = dvUsersGroupMessages[count1]["Content"].ToString();
                            if (content.Length > 100)
                                content = content.Substring(0, 100) + "...";
                            content = "<a class=\"AddGreenLink\" href=\"" +
                                dat.MakeNiceName(dvUsersGroupMessages[count1]["Header"].ToString()) +
                                "_" + dvUsersGroupMessages[count1]["ID"].ToString() + "_Group\">" +
                                dvUsersGroupMessages[count1]["Header"].ToString() + " Group</a>&nbsp;|&nbsp;<span style=\"color: White;\">On "+
                                dvUsersGroupMessages[count1]["TheDate"].ToString() + "</span>&nbsp;|&nbsp;" + content;
                            content += "&nbsp; <a class=\"AddLink\" href=\"" +
                                dat.MakeNiceName(dvUsersGroupMessages[count1]["Header"].ToString()) +
                                "_" + dvUsersGroupMessages[count1]["ID"].ToString() + "_Group\">Read More</a>";
                            lit.Text += "<div class=\"EventDiv2\"><label>" + content + "</label></div>";
                            count1++;
                            break;
                        case "2":
                            if (!hash.Contains(dvGroupEventMessages[count2]["GEMID"].ToString()+"2"))
                            {
                                DataView dvE = dat.GetDataDV("SELECT GEO.ID, G.ID AS GID, GE.Name, G.Header FROM GroupEvents GE, Groups G, GroupEvent_Occurance GEO WHERE " +
                                    " GE.ID=GEO.GroupEventID AND G.ID=GE.GroupID AND GE.ID=" + dvGroupEventMessages[count2]["ID"].ToString());
                                content = dvGroupEventMessages[count2]["Content"].ToString();
                                if (content.Length > 100)
                                    content = content.Substring(0, 100) + "...";
                                content = "<a class=\"AddGreenLink\" href=\"" +
                                    dat.MakeNiceName(dvE[0]["Header"].ToString()) +
                                    "_" + dvE[0]["GID"].ToString() + "_Group\">" +
                                    dvE[0]["Header"].ToString() + "</a>&nbsp;|&nbsp;<span style=\"color: White;\">On " +
                                dvGroupEventMessages[count2]["TheDate"].ToString() + "</span>&nbsp;|&nbsp;" + content;
                                content += "&nbsp; <a class=\"AddLink\" href=\"" +
                                    dat.MakeNiceName(dvGroupEventMessages[count2]["Name"].ToString()) + "_" +
                                    dvE[0]["ID"].ToString() +
                                    "_" + dvGroupEventMessages[count2]["ID"].ToString() + "_GroupEvent\">Read More</a>";
                                lit.Text += "<div class=\"EventDiv2\"><label>" + content + "</label></div>";
                                hash.Add(dvGroupEventMessages[count2]["GEMID"].ToString()+"2", "");
                            }
                            count2++;
                            break;
                        case "3":
                            if (!hash.Contains(dvGroupEventMessagesNonMember[count3]["GEMID"].ToString() + "3"))
                            {
                                DataView dvE2 = dat.GetDataDV("SELECT GEO.ID, G.ID AS GID, G.Header, GE.Name FROM Groups G, GroupEvents GE, GroupEvent_Occurance GEO WHERE " +
                                    " GE.ID=GEO.GroupEventID AND G.ID=GE.GroupID AND GE.ID=" + dvGroupEventMessagesNonMember[count3]["ID"].ToString());
                                content = dvGroupEventMessagesNonMember[count3]["Content"].ToString();
                                if (content.Length > 100)
                                    content = content.Substring(0, 100) + "...";
                                content = "<a class=\"AddGreenLink\" href=\"" +
                                    dat.MakeNiceName(dvE2[0]["Header"].ToString()) +
                                    "_" + dvE2[0]["GID"].ToString() + "_Group\">" +
                                    dvE2[0]["Header"].ToString() + " Group</a>&nbsp;|&nbsp;<span style=\"color: White;\">On " +
                                dvGroupEventMessagesNonMember[count3]["TheDate"].ToString() + "</span>&nbsp;|&nbsp;" + content;
                                content += "&nbsp; <a class=\"AddLink\" href=\"" +
                                    dat.MakeNiceName(dvGroupEventMessagesNonMember[count3]["Name"].ToString()) + "_" +
                                    dvE2[0]["ID"].ToString() +
                                    "_" + dvGroupEventMessagesNonMember[count3]["ID"].ToString() + "_GroupEvent\">Read More</a>";
                                lit.Text += "<div class=\"EventDiv2\"><label>" + content + "</label></div>";
                                hash.Add(dvGroupEventMessagesNonMember[count3]["GEMID"].ToString() + "3", "");
                            }
                            count3++;
                            break;
                        case "4":
                            if (!hash.Contains(dvGroupThreads[count4]["TheID"].ToString() + "4"))
                            {
                                DataView dvE4 = dat.GetDataDV("SELECT G.ID AS GID, G.Header FROM Groups G, GroupThreads GT WHERE " +
                                    " GT.GroupID=G.ID AND GT.ID=" + dvGroupThreads[count4]["TheID"].ToString());
                                content = dvGroupThreads[count4]["ThreadName"].ToString();
                                if (content.Length > 100)
                                    content = content.Substring(0, 100) + "...";
                                content = "<a class=\"AddGreenLink\" href=\"" +
                                    dat.MakeNiceName(dvE4[0]["Header"].ToString()) +
                                    "_" + dvE4[0]["GID"].ToString() + "_Group\">" +
                                    dvE4[0]["Header"].ToString() + " Group</a>&nbsp;|&nbsp;<span style=\"color: White;\">On " +
                                dvGroupThreads[count4]["TheDate"].ToString() + "</span>&nbsp;|&nbsp;" + content;
                                content += "&nbsp; <a class=\"AddLink\" href=\"" +
                                    dat.MakeNiceName(dvE4[0]["Header"].ToString()) + "_" +
                                    dvE4[0]["GID"].ToString() + "_Group\">Read More</a>";
                                lit.Text += "<div class=\"EventDiv2\"><label>" + content + "</label></div>";
                                hash.Add(dvGroupThreads[count4]["TheID"].ToString() + "4", "");
                            }
                            count4++;
                            break;
                        case "5":
                            if (!hash.Contains(dvGroupThreadComments[count5]["TheID"].ToString() + "5"))
                            {
                                DataView dvE5 = dat.GetDataDV("SELECT G.ID AS GID, G.Header FROM Groups G, " +
                                    "GroupThreads_Comments GTC, GroupThreads GT WHERE GT.GroupID=G.ID AND GT.ID=GTC.ThreadID " +
                                    "AND GTC.ID=" + dvGroupThreadComments[count5]["TheID"].ToString());
                                content = dvGroupThreadComments[count5]["Content"].ToString();
                                if (content.Length > 100)
                                    content = content.Substring(0, 100) + "...";
                                content = "<a class=\"AddGreenLink\" href=\"" +
                                    dat.MakeNiceName(dvE5[0]["Header"].ToString()) +
                                    "_" + dvE5[0]["GID"].ToString() + "_Group\">" +
                                    dvE5[0]["Header"].ToString() + " Group</a>&nbsp;|&nbsp;<span style=\"color: White;\">On " +
                                dvGroupThreadComments[count5]["TheDate"].ToString() + "</span>&nbsp;|&nbsp;" + content;
                                content += "&nbsp; <a class=\"AddLink\" href=\"" +
                                    dat.MakeNiceName(dvE5[0]["Header"].ToString()) + "_" +
                                    dvE5[0]["GID"].ToString() + "_Group\">Read More</a>";
                                lit.Text += "<div class=\"EventDiv2\"><label>" + content + "</label></div>";
                                hash.Add(dvGroupThreadComments[count5]["TheID"].ToString() + "5", "");
                            }
                            count5++;
                            break;
                        default: break;
                    }
                }
            }
            else
            {
                YourGroupsLabel.Text = "";
            }
            GroupUpdatesPanel.Controls.Add(lit);
        
    }

    protected string GetIDToUse(bool use1, bool use2, bool use3, bool use4, bool use5, DateTime dt1,
        DateTime dt2, DateTime dt3, DateTime dt4, DateTime dt5)
    {
        string idToUse = "";
        if (use1)
        {
            if (use2)
            {
                if (use3)
                {
                    if (use4)
                    {
                        if (use5)
                        {
                            if (dt1 > dt2)
                            {
                                if (dt1 > dt3)
                                {
                                    if (dt1 > dt4)
                                    {
                                        if (dt1 > dt5)
                                            idToUse = "1";
                                        else
                                            idToUse = "5";
                                    }
                                    else
                                    {
                                        if (dt4 > dt5)
                                            idToUse = "4";
                                        else
                                            idToUse = "5";
                                    }
                                }
                                else
                                {
                                    if (dt3 > dt4)
                                    {
                                        if (dt3 > dt5)
                                            idToUse = "3";
                                        else
                                            idToUse = "5";
                                    }
                                    else
                                    {
                                        if (dt4 > dt5)
                                            idToUse = "4";
                                        else
                                            idToUse = "5";
                                    }
                                }
                            }
                            else
                            {
                                if (dt2 > dt3)
                                {
                                    if (dt2 > dt4)
                                    {
                                        if (dt2 > dt5)
                                            idToUse = "2";
                                        else
                                            idToUse = "5";
                                    }
                                    else
                                    {
                                        if (dt4 > dt5)
                                            idToUse = "4";
                                        else
                                            idToUse = "5";
                                    }
                                }
                                else
                                {
                                    if (dt3 > dt4)
                                    {
                                        if (dt3 > dt5)
                                            idToUse = "3";
                                        else
                                            idToUse = "5";
                                    }
                                    else
                                    {
                                        if (dt4 > dt5)
                                            idToUse = "4";
                                        else
                                            idToUse = "5";
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (dt1 > dt2)
                            {
                                if (dt1 > dt3)
                                {
                                    if (dt1 > dt4)
                                    {
                                        idToUse = "1";
                                    }
                                    else
                                    {
                                        idToUse = "4";
                                    }
                                }
                                else
                                {
                                    if (dt3 > dt4)
                                    {
                                        idToUse = "3";
                                    }
                                    else
                                    {
                                        idToUse = "4";
                                    }
                                }
                            }
                            else
                            {
                                if (dt2 > dt3)
                                {
                                    if (dt2 > dt4)
                                    {
                                        idToUse = "2";
                                    }
                                    else
                                    {
                                        idToUse = "4";
                                    }
                                }
                                else
                                {
                                    if (dt3 > dt4)
                                    {
                                        idToUse = "3";
                                    }
                                    else
                                    {
                                        idToUse = "4";
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (use5)
                        {
                            if (dt1 > dt2)
                            {
                                if (dt1 > dt3)
                                {
                                    if (dt1 > dt5)
                                        idToUse = "1";
                                    else
                                        idToUse = "5";
                                }
                                else
                                {
                                    if (dt3 > dt5)
                                        idToUse = "3";
                                    else
                                        idToUse = "5";
                                }
                            }
                            else
                            {
                                if (dt2 > dt3)
                                {
                                    if (dt2 > dt5)
                                        idToUse = "2";
                                    else
                                        idToUse = "5";
                                }
                                else
                                {
                                    if (dt3 > dt5)
                                        idToUse = "3";
                                    else
                                        idToUse = "5";
                                }
                            }
                        }
                        else
                        {
                            if (dt1 > dt2)
                            {
                                if (dt1 > dt3)
                                {
                                    idToUse = "1";
                                }
                                else
                                {
                                    idToUse = "3";
                                }
                            }
                            else
                            {
                                if (dt2 > dt3)
                                {
                                    idToUse = "2";
                                }
                                else
                                {
                                    idToUse = "3";
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (dt1 > dt2)
                    {
                        if (use4)
                        {
                            if (use5)
                            {
                                if (dt1 > dt4)
                                {
                                    if (dt1 > dt5)
                                        idToUse = "1";
                                    else
                                        idToUse = "5";
                                }
                                else
                                {
                                    if (dt4 > dt5)
                                        idToUse = "4";
                                    else
                                        idToUse = "5";
                                }
                            }
                            else
                            {
                                if (dt1 > dt4)
                                {
                                    idToUse = "1";
                                }
                                else
                                {
                                    idToUse = "4";
                                }
                            }
                        }
                        else
                        {
                            if (use5)
                            {
                                if (dt1 > dt5)
                                    idToUse = "1";
                                else
                                    idToUse = "5";
                            }
                            else
                            {
                                idToUse = "1";
                            }
                        }
                    }
                    else
                    {
                        if (use4)
                        {
                            if (use5)
                            {
                                if (dt2 > dt4)
                                {
                                    if (dt2 > dt5)
                                        idToUse = "2";
                                    else
                                        idToUse = "5";
                                }
                                else
                                {
                                    if (dt4 > dt5)
                                        idToUse = "4";
                                    else
                                        idToUse = "5";
                                }
                            }
                            else
                            {
                                if (dt2 > dt4)
                                {
                                    idToUse = "4";
                                }
                                else
                                {
                                    idToUse = "2";
                                }
                            }
                        }
                        else
                        {
                            if (use5)
                            {
                                if (dt2 > dt5)
                                    idToUse = "2";
                                else
                                    idToUse = "5";
                            }
                            else
                                idToUse = "2";
                        }
                    }
                }
            }
            else
            {
                if (use4)
                {
                    if (use5)
                    {
                        if (dt1 > dt4)
                        {
                            if (dt1 > dt5)
                                idToUse = "1";
                            else
                                idToUse = "5";
                        }
                        else
                        {
                            if (dt4 > dt5)
                                idToUse = "4";
                            else
                                idToUse = "5";
                        }
                    }
                    else
                    {
                        if (dt1 > dt4)
                        {
                            idToUse = "1";
                        }
                        else
                        {
                            idToUse = "4";
                        }
                    }
                }
                else
                {
                    if (use5)
                    {
                        if (dt1 > dt5)
                            idToUse = "1";
                        else
                            idToUse = "5";
                    }
                    else
                        idToUse = "1";
                }
            }
        }
        else
        {
            if (use2)
            {
                if (use3)
                {
                    if (use4)
                    {
                        if (use5)
                        {
                            if (dt2 > dt3)
                            {
                                if (dt2 > dt4)
                                {
                                    if (dt2 > dt5)
                                        idToUse = "2";
                                    else
                                        idToUse = "5";
                                }
                                else
                                {
                                    if (dt4 > dt5)
                                        idToUse = "4";
                                    else
                                        idToUse = "5";
                                }
                            }
                            else
                            {
                                if (dt3 > dt4)
                                {
                                    if (dt3 > dt5)
                                        idToUse = "3";
                                    else
                                        idToUse = "5";
                                }
                                else
                                {
                                    if (dt4 > dt5)
                                        idToUse = "4";
                                    else
                                        idToUse = "5";
                                }
                            }
                        }
                        else
                        {
                            if (dt2 > dt3)
                            {
                                if (dt2 > dt4)
                                {
                                    idToUse = "2";
                                }
                                else
                                {
                                    idToUse = "4";
                                }
                            }
                            else
                            {
                                if (dt3 > dt4)
                                {
                                    idToUse = "3";
                                }
                                else
                                {
                                    idToUse = "4";
                                }
                            }
                        }
                    }
                    else
                    {
                        if (use5)
                        {
                            if (dt2 > dt3)
                            {
                                if (dt2 > dt5)
                                    idToUse = "2";
                                else
                                    idToUse = "5";
                            }
                            else
                            {
                                if (dt3 > dt5)
                                    idToUse = "3";
                                else
                                    idToUse = "5";
                            }
                        }
                        else
                        {
                            if (dt2 > dt3)
                            {
                                idToUse = "2";
                            }
                            else
                                idToUse = "3";
                        }
                    }
                }
                else
                {
                    if (use4)
                    {
                        if (use5)
                        {
                            if (dt2 > dt4)
                            {
                                if (dt2 > dt5)
                                    idToUse = "2";
                                else
                                    idToUse = "5";
                            }
                            else
                            {
                                if (dt4 > dt5)
                                    idToUse = "4";
                                else
                                    idToUse = "5";
                            }
                        }
                        else
                        {
                            if (dt2 > dt4)
                                idToUse = "2";
                            else
                                idToUse = "4";
                        }
                    }
                    else
                    {
                        if (use5)
                        {
                            if (dt2 > dt5)
                                idToUse = "2";
                            else
                                idToUse = "5";
                        }
                        else
                            idToUse = "2";
                    }
                }
            }
            else
            {
                if (use3)
                {
                    if (use4)
                    {
                        if (use5)
                        {
                            if (dt3 > dt4)
                            {
                                if (dt3 > dt5)
                                    idToUse = "3";
                                else
                                    idToUse = "5";
                            }
                            else
                            {
                                if (dt4 > dt5)
                                    idToUse = "4";
                                else
                                    idToUse = "5";
                            }
                        }
                        else
                        {
                            if (dt3 > dt4)
                                idToUse = "3";
                            else
                                idToUse = "4";
                        }
                    }
                    else
                    {
                        if (use5)
                        {
                            if (dt3 > dt5)
                                idToUse = "3";
                            else
                                idToUse = "5";
                        }
                        else
                            idToUse = "3";
                    }
                }
                else
                {
                    if (use4)
                    {
                        if (use5)
                        {
                            if (dt4 > dt5)
                                idToUse = "4";
                            else
                                idToUse = "5";
                        }
                        else
                            idToUse = "4";
                    }
                    else
                    {
                        idToUse = "5";
                    }
                }
            }
        }

        return idToUse;
    }

    protected void AfterGroupDelete(object sender, EventArgs e)
    {
        FillGroups();
    }

    protected void SaveGroupsPrefs(object sender, EventArgs e)
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

        DataView dvUsersGroups = dat.GetDataDV("SELECT * FROM Group_Members GM, Groups G " +
            "WHERE GM.GroupID=G.ID AND GM.Accepted='True' AND GM.MemberID=" + Session["User"].ToString());

        Literal lit;
        CheckBox check1;
        CheckBox check2;
        CheckBox check3;
        CheckBox check4;
        CheckBox check5;
        string prefs = "";

        foreach (DataRowView row in dvUsersGroups)
        {
            prefs = "";
            check1 = (CheckBox)GroupsPanel.FindControl("thread" + row["GroupID"].ToString());
            check2 = (CheckBox)GroupsPanel.FindControl("newevent" + row["GroupID"].ToString());
            check3 = (CheckBox)GroupsPanel.FindControl("joinButton" + row["GroupID"].ToString());
            check4 = (CheckBox)GroupsPanel.FindControl("board" + row["GroupID"].ToString());
            check5 = (CheckBox)GroupsPanel.FindControl("comment" + row["GroupID"].ToString());

            if (check1.Checked)
                prefs += "1";
            if (check2.Checked)
                prefs += "2";
            if (check3 != null)
                if (check3.Checked)
                    prefs += "3";
            if (check4.Checked)
                prefs += "4";
            if (check5.Checked)
                prefs += "5";

            dat.Execute("UPDATE Group_Members SET Prefs = '" + prefs + "' WHERE GroupID=" +
                row["GroupID"].ToString() + " AND MemberID=" + Session["User"].ToString());
        }
    }

    protected void GoToSearches(object sender, EventArgs e)
    {
        Response.Redirect("SearchesAndPages.aspx");
    }

    protected void GoToAdStatistics(object sender, EventArgs e)
    {
        Response.Redirect("AdStatistics.aspx");
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        string USER_ID = "";
        try
        {
            //Ajax.Utility.RegisterTypeForAjax(typeof(User));
            

            //if (!IsPostBack)
            //{
            //    FriendsButton.Attributes.Add("onmouseover", "this.src='image/MyFriendsHover.png'");
            //    FriendsButton.Attributes.Add("onmouseout", "this.src='image/MyFriends.png'");


            //}
            //else
            //{
            //    FriendsButton.Attributes.Remove("onmouseover");
            //    FriendsButton.Attributes.Remove("onmouseout");
            //    MessagesButton.Attributes.Remove("onmouseover");
            //    MessagesButton.Attributes.Remove("onmouseout");
            //}


            //FOR USER PREFERENCES
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            MessageRadWindowManager.VisibleOnPageLoad = false;

            
            if (!IsPostBack)
            {
                try
                {
                    

                    if (Session["User"] != null)
                    {
                    }
                    else
                    {
                        Button calendarLink = (Button)dat.FindControlRecursive(this, "CalendarLink");
                        calendarLink.Visible = false;
                        Response.Redirect("~/UserLogin.aspx");
                    }
                }
                catch (Exception ex)
                {
                    ErrorLabel.Text = ex.ToString();
                   Response.Redirect("~/UserLogin.aspx");
                }

                USER_ID = Session["User"].ToString();

                //DataSet dsCat = dat.GetData("SELECT * FROM Categories");
                //CategoriesCheckBoxes.DataSource = dsCat;
                //CategoriesCheckBoxes.DataTextField = "CategoryName";
                //CategoriesCheckBoxes.DataValueField = "ID";
                //CategoriesCheckBoxes.DataBind();

                



                DataSet dsProvider = dat.GetData("SELECT * FROM PhoneProviders");
                ProviderDropDown.DataSource = dsProvider;
                ProviderDropDown.DataTextField = "Provider";
                ProviderDropDown.DataValueField = "ID";
                ProviderDropDown.DataBind();


                Data d = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
                DataSet ds = d.GetData("SELECT * FROM Events E, Venues V, Event_Occurance EO, User_Calendar UC WHERE UC.EventID=E.ID AND E.ID=EO.EventID AND E.Venue=V.ID AND UC.UserID=" + USER_ID);
                DataSet dsUser = d.GetData("SELECT * FROM Users WHERE User_ID=" + USER_ID);
                DataSet dsUserPrefs = d.GetData("SELECT * FROM UserPreferences WHERE UserID=" + USER_ID);

                WeeklyCheckBox.Checked = bool.Parse(dsUser.Tables[0].Rows[0]["Weekly"].ToString());

                if (dsUser.Tables[0].Rows[0]["FirstName"] != null)
                    FirstNameTextBox.THE_TEXT = dsUser.Tables[0].Rows[0]["FirstName"].ToString();

                if (dsUser.Tables[0].Rows[0]["LastName"] != null)
                    LastNameTextBox.THE_TEXT = dsUser.Tables[0].Rows[0]["LastName"].ToString();

                if (dsUserPrefs.Tables.Count > 0)
                    if (dsUserPrefs.Tables[0].Rows.Count > 0)
                    {
                        //AgeTextBox.Text = dsUserPrefs.Tables[0].Rows[0]["Age"].ToString();
                        SexTextBox.Text = dsUserPrefs.Tables[0].Rows[0]["Sex"].ToString();

                        LocationTextBox.Text = dsUserPrefs.Tables[0].Rows[0]["Location"].ToString();

                        string eventsPosted = "0";
                        DataSet dsEvents = dat.GetData("SELECT COUNT(*) AS COUNT1 FROM Events WHERE UserName='" + Session["UserName"].ToString() + "'");
                        if (dsEvents.Tables.Count > 0)
                            if (dsEvents.Tables[0].Rows.Count > 0)
                                eventsPosted = dsEvents.Tables[0].Rows[0]["COUNT1"].ToString();
                        EventsLabel.Text = eventsPosted;
                        AttendedLabel.Text = dsUserPrefs.Tables[0].Rows[0]["EventsAttended"].ToString();

                        if (dsUserPrefs.Tables[0].Rows[0]["CalendarPrivacyMode"].ToString() != null)
                        {
                            PublicPrivateCheckList.SelectedValue = dsUserPrefs.Tables[0].Rows[0]["CalendarPrivacyMode"].ToString();
                        }

                        //if (dsUserPrefs.Tables[0].Rows[0]["PollPreferences"].ToString() != null)
                        //{
                        //    PollRadioList.SelectedValue = dsUserPrefs.Tables[0].Rows[0]["PollPreferences"].ToString();
                        //}

                        if (dsUserPrefs.Tables[0].Rows[0]["CommentsPreferences"].ToString() != null)
                        {
                            CommentsRadioList.SelectedValue = dsUserPrefs.Tables[0].Rows[0]["CommentsPreferences"].ToString();
                        }
                        RadioButtonList CategoriesOnOffRadioList = (RadioButtonList)AdCategoryRadPanel.Items[0].Items[0].FindControl("CategoriesOnOffRadioList");
                        if (dsUserPrefs.Tables[0].Rows[0]["CategoriesOnOff"].ToString() != null)
                        {
                            if (bool.Parse(dsUserPrefs.Tables[0].Rows[0]["CategoriesOnOff"].ToString()))
                                CategoriesOnOffRadioList.SelectedValue = "1";
                            else
                                CategoriesOnOffRadioList.SelectedValue = "2";
                        }

                       

                        if(dsUserPrefs.Tables[0].Rows[0]["RecommendationPrefs"].ToString() != null){
                            string recom = dsUserPrefs.Tables[0].Rows[0]["RecommendationPrefs"].ToString();
                            if (recom.Contains("1"))
                                RecommendationsCheckList.Items[0].Selected = true;
                            if (recom.Contains("2"))
                                RecommendationsCheckList.Items[1].Selected = true;
                            if (recom.Contains("3"))
                                RecommendationsCheckList.Items[2].Selected = true;
                        }

                        if (dsUserPrefs.Tables[0].Rows[0]["CommunicationPrefs"].ToString() != null)
                        {
                            CommunicationPrefsRadioList.SelectedValue =
                                dsUserPrefs.Tables[0].Rows[0]["CommunicationPrefs"].ToString();
                        }


                        if (dsUserPrefs.Tables[0].Rows[0]["Country"].ToString() != null)
                        {
                            if (dsUserPrefs.Tables[0].Rows[0]["Country"].ToString().Trim() != "")
                            {
                                if (dsUserPrefs.Tables[0].Rows[0]["Address"].ToString() != null)
                                {
                                    AddressTextBox.THE_TEXT = dsUserPrefs.Tables[0].Rows[0]["Address"].ToString();
                                }

                                if (dsUserPrefs.Tables[0].Rows[0]["City"].ToString() != null)
                                {
                                    BillCityTextBox.THE_TEXT = dsUserPrefs.Tables[0].Rows[0]["City"].ToString();
                                }

                                if (dsUserPrefs.Tables[0].Rows[0]["ZIP"].ToString() != null)
                                {
                                    ZipTextBox.THE_TEXT = dsUserPrefs.Tables[0].Rows[0]["ZIP"].ToString();
                                }

                                BillCountryDropDown.SelectedValue = dsUserPrefs.Tables[0].Rows[0]["Country"].ToString();

                                DataSet dsStates = dat.GetData("SELECT * FROM State WHERE country_id=" + dsUserPrefs.Tables[0].Rows[0]["Country"].ToString());

                                bool isText = false;
                                if (dsStates.Tables.Count > 0)
                                    if (dsStates.Tables[0].Rows.Count > 0)
                                    {
                                        BillStateDropDown.DataSource = dsStates;
                                        BillStateDropDown.DataTextField = "state_2_code";
                                        BillStateDropDown.DataValueField = "state_id";
                                        BillStateDropDown.DataBind();
                                        BillStateDropDown.Items.Insert(0, new ListItem("Select State..", "-1"));

                                        if (dsUserPrefs.Tables[0].Rows[0]["State"].ToString() != null)
                                        {
                                            ListItem a = BillStateDropDown.Items.FindByText(dsUserPrefs.Tables[0].Rows[0]["State"].ToString());
                                            if (a != null)
                                                BillStateDropDown.SelectedValue = a.Value;
                                        }

                                        BillStateDropPanel.Visible = true;
                                        BillStateTextPanel.Visible = false;
                                    }
                                    else
                                    {
                                        isText = true;
                                    }
                                else
                                    isText = true;

                                if (isText)
                                {
                                    if (dsUserPrefs.Tables[0].Rows[0]["State"].ToString() != null)
                                    {
                                        BillStateTextBox.THE_TEXT = dsUserPrefs.Tables[0].Rows[0]["State"].ToString();
                                    }
                                    BillStateTextPanel.Visible = true;
                                    BillStateDropPanel.Visible = false;
                                }
                            }
                        }
                        if (dsUserPrefs.Tables[0].Rows[0]["CatCountry"].ToString() != null)
                        {
                            if (dsUserPrefs.Tables[0].Rows[0]["CatCountry"].ToString().Trim() != "")
                            {
                                if (dsUserPrefs.Tables[0].Rows[0]["CatCity"].ToString() != null)
                                {
                                    CityTextBox.THE_TEXT = dsUserPrefs.Tables[0].Rows[0]["CatCity"].ToString();
                                }

                                if (dsUserPrefs.Tables[0].Rows[0]["CatZip"].ToString() != null)
                                {
                                    if (dsUserPrefs.Tables[0].Rows[0]["CatZip"].ToString().Trim() != "")
                                    {
                                        char[] delim = { ';' };
                                        string[] tokens = dsUserPrefs.Tables[0].Rows[0]["CatZip"].ToString().Split(delim);
                                        
                                        if(tokens.Length > 1)
                                            CatZipTextBox.THE_TEXT = tokens[1].Trim();
                                    }
                                }

                                if (dsUserPrefs.Tables[0].Rows[0]["Radius"].ToString() != null)
                                {
                                    RadiusDropDown.SelectedValue = 
                                        dsUserPrefs.Tables[0].Rows[0]["Radius"].ToString();
                                }

                                CountryDropDown.SelectedValue = dsUserPrefs.Tables[0].Rows[0]["CatCountry"].ToString();

                                DataSet dsStates = dat.GetData("SELECT * FROM State WHERE country_id=" + dsUserPrefs.Tables[0].Rows[0]["CatCountry"].ToString());

                                bool isText = false;
                                if (dsStates.Tables.Count > 0)
                                    if (dsStates.Tables[0].Rows.Count > 0)
                                    {
                                        StateDropDown.DataSource = dsStates;
                                        StateDropDown.DataTextField = "state_2_code";
                                        StateDropDown.DataValueField = "state_id";
                                        StateDropDown.DataBind();

                                        if (dsUserPrefs.Tables[0].Rows[0]["CatState"] != null)
                                        {
                                            StateDropDown.SelectedValue = StateDropDown.Items.FindByText(dsUserPrefs.Tables[0].Rows[0]["CatState"].ToString()).Value;
                                        }

                                        StateDropDownPanel.Visible = true;
                                        StateTextBoxPanel.Visible = false;
                                    }
                                    else
                                    {
                                        isText = true;
                                    }
                                else
                                    isText = true;

                                if (isText)
                                {
                                    if (dsUserPrefs.Tables[0].Rows[0]["CatState"].ToString() != null)
                                    {
                                        StateTextBox.THE_TEXT = dsUserPrefs.Tables[0].Rows[0]["CatState"].ToString();
                                    }
                                    StateTextBoxPanel.Visible = true;
                                    StateDropDownPanel.Visible = false;
                                }
                            }
                        }
                        if (dsUserPrefs.Tables[0].Rows[0]["TextingPrefs"].ToString() != null)
                        {
                            if (dsUserPrefs.Tables[0].Rows[0]["TextingPrefs"].ToString().Contains("1"))
                                TextingCheckBoxList.Items[0].Selected = true;
                            if (dsUserPrefs.Tables[0].Rows[0]["TextingPrefs"].ToString().Contains("2"))
                                TextingCheckBoxList.Items[1].Selected = true;
                            if (dsUserPrefs.Tables[0].Rows[0]["TextingPrefs"].ToString().Contains("3"))
                                TextingCheckBoxList.Items[2].Selected = true;
                        }

                        if (dsUserPrefs.Tables[0].Rows[0]["EmailPrefs"].ToString() != null)
                        {
                            if (dsUserPrefs.Tables[0].Rows[0]["EmailPrefs"].ToString().Contains("1"))
                                EmailCheckList.Items[0].Selected = true;
                            if (dsUserPrefs.Tables[0].Rows[0]["EmailPrefs"].ToString().Contains("2"))
                                EmailCheckList.Items[1].Selected = true;
                            if (dsUserPrefs.Tables[0].Rows[0]["EmailPrefs"].ToString().Contains("3"))
                                EmailCheckList.Items[2].Selected = true;

                            if (dsUserPrefs.Tables[0].Rows[0]["EmailPrefs"].ToString().Contains(EmailUserCheckList1.Items[0].Value))
                                EmailUserCheckList1.Items[0].Selected = true;
                            if (dsUserPrefs.Tables[0].Rows[0]["EmailPrefs"].ToString().Contains(EmailUserCheckList1.Items[1].Value))
                                EmailUserCheckList1.Items[1].Selected = true;
                            if (dsUserPrefs.Tables[0].Rows[0]["EmailPrefs"].ToString().Contains(EmailUserCheckList1.Items[2].Value))
                                EmailUserCheckList1.Items[2].Selected = true;

                            if (dsUserPrefs.Tables[0].Rows[0]["EmailPrefs"].ToString().Contains(EmailUserCheckList2.Items[0].Value))
                                EmailUserCheckList2.Items[0].Selected = true;
                            if (dsUserPrefs.Tables[0].Rows[0]["EmailPrefs"].ToString().Contains(EmailUserCheckList2.Items[1].Value))
                                EmailUserCheckList2.Items[1].Selected = true;
                            if (dsUserPrefs.Tables[0].Rows[0]["EmailPrefs"].ToString().Contains(EmailUserCheckList2.Items[2].Value))
                                EmailUserCheckList2.Items[2].Selected = true;
                        

                            if (dsUserPrefs.Tables[0].Rows[0]["EmailPrefs"].ToString().Contains("C"))
                                EmailCheckList3.Items[0].Selected = true;
                        }
                    }

                DataSet dsComments = d.GetData("SELECT * FROM User_Comments CU, Users U WHERE CU.CommenterID=U.User_ID AND CU.UserID=" + USER_ID.ToString());
                //Label UserNameLabel = (Label)Tab3.FindControl("UserNameLabel");

                //UserNameLabel.Text = dsUser.Tables[0].Rows[0]["UserName"].ToString();

                if (dsUser.Tables[0].Rows[0]["Email"].ToString() != null)
                {
                    EmailTextBox.Text = dsUser.Tables[0].Rows[0]["Email"].ToString();
                }

                if (dsUser.Tables[0].Rows[0]["PhoneNumber"].ToString() != null)
                {
                    PhoneTextBox.THE_TEXT = dsUser.Tables[0].Rows[0]["PhoneNumber"].ToString();
                }

                if (dsUser.Tables[0].Rows[0]["PhoneProvider"].ToString() != null)
                {
                    ProviderDropDown.SelectedValue = dsUser.Tables[0].Rows[0]["PhoneProvider"].ToString();
                }

                Image FriendImage = (Image)Tab3.FindControl("FriendImage");

                if (dsUser.Tables[0].Rows[0]["ProfilePicture"].ToString() != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(".") + "/UserFiles/" + dsUser.Tables[0].Rows[0]["UserName"].ToString() + "/Profile/" + dsUser.Tables[0].Rows[0]["ProfilePicture"].ToString()))
                    {
                        System.Drawing.Image theimg = System.Drawing.Image.FromFile(Server.MapPath(".") + "/UserFiles/" + dsUser.Tables[0].Rows[0]["UserName"].ToString() +
                "/Profile/" + dsUser.Tables[0].Rows[0]["ProfilePicture"].ToString());

                        double width = double.Parse(theimg.Width.ToString());
                        double height = double.Parse(theimg.Height.ToString());

                        if (width > height)
                        {
                            if (width <= 150)
                            {

                            }
                            else
                            {
                                double dividor = double.Parse("150.00") / double.Parse(width.ToString());
                                width = double.Parse("150.00");
                                height = height * dividor;
                            }
                        }
                        else
                        {
                            if (width == height)
                            {
                                width = double.Parse("150.00");
                                height = double.Parse("150.00");
                            }
                            else
                            {
                                double dividor = double.Parse("150.00") / double.Parse(height.ToString());
                                height = double.Parse("150.00");
                                width = width * dividor;
                            }
                        }

                        FriendImage.Width = int.Parse((Math.Round(decimal.Parse(width.ToString()))).ToString());
                        FriendImage.Height = int.Parse((Math.Round(decimal.Parse(height.ToString()))).ToString());

                        FriendImage.ImageUrl = "~/UserFiles/" + dsUser.Tables[0].Rows[0]["UserName"].ToString() + "/Profile/" + dsUser.Tables[0].Rows[0]["ProfilePicture"].ToString();
                        Session["ProfilePicture"] = dsUser.Tables[0].Rows[0]["ProfilePicture"].ToString();
                    }
                    else
                    {
                        FriendImage.ImageUrl = "~/NewImages/NoAvatar150.jpg";
                    }
                }
                else
                    FriendImage.ImageUrl = "~/NewImages/NoAvatar150.jpg";
            }
            else
            {
                if (Session["User"] == null)
                {
                    ErrorLabel.Text = "something happen";
                    //Session.Abandon();
                    Response.Redirect("~/UserLogin.aspx");

                }
            }

            

            DataSet dsVenues = dat.GetData("SELECT * FROM UserVenues UV, Venues V WHERE V.ID=UV.VenueID AND UV.UserID=" + Session["User"].ToString());

            VenuesRadPanel.Items[0].Text = "<div style=\" background-color: #1b1b1b; cursor: pointer;" +
                "\"><label class=\"PreferencesTitle\" style=\"cursor: pointer !important;\">Favorite Venues</label><span " +
                "style=\"font-style: italic; font-family: Arial; font-size: 14px; color: #cccccc;\">" +
                " (Click to drop down the list. Un-check to remove a venue from the favorites list.)</span></div>";

            //"<div style=\"border-bottom: dotted 1px black; " +
            //    "padding-top: 10px;\"><label class=\"PreferencesTitle\">Favorite Venues</label><span " +
            //    "style=\"font-style: italic; font-family: Arial; font-size: 14px; color: #666666;\">" +
            //    "(You can add these from the <a href=\"VenueSearch.aspx\" class=\"AddLink\">Venues Page</a>. " +
            //    "Un-check to remove a venue from the favorites list.)</span></div>";


            CheckBoxList VenueCheckBoxes = new CheckBoxList();
            VenueCheckBoxes.Width = 560;
            VenueCheckBoxes.CssClass = "VenueCheckBoxes";
            VenueCheckBoxes.ID = "VenueCheckBoxes";
            VenueCheckBoxes.RepeatColumns = 4;
            VenueCheckBoxes.RepeatDirection = RepeatDirection.Horizontal;

            VenueCheckBoxes.DataSource = dsVenues;
            VenueCheckBoxes.DataTextField = "NAME";
            VenueCheckBoxes.DataValueField = "ID";
            VenueCheckBoxes.DataBind();

            for (int i = 0; i < VenueCheckBoxes.Items.Count; i++)
            {
                VenueCheckBoxes.Items[i].Selected = true;
            }

            if (VenueCheckBoxes.Items.Count == 0)
            {
                Label label = new Label();
                label.CssClass = "VenueCheckBoxes";
                label.Text = "You have no venues specified as your favorite. To add venues as your favorites search for them on the <a href=\"VenueSearch.aspx\" class=\"AddLink\">Venues Page</a>";
                VenuesRadPanel.Items[0].Items[0].Controls.Add(label);
            }
            else
            {
                VenuesRadPanel.Items[0].Items[0].Controls.Add(VenueCheckBoxes);
            }

            VenuesRadPanel.CollapseAllItems();
        }
        catch (Exception ex)
        {
            UserErrorLabel.Text = ex.ToString();
        }

        if (Request.QueryString["p"] != null)
        {
            RadTabStrip2.Tabs[2].Selected = true;
            RadTabStrip2.Tabs[2].CssClass = "MyTabsPreferencesSelected";
            RadTabStrip2.Tabs[0].CssClass = "MyTabsMessages";
            RadTabStrip2.Tabs[1].CssClass = "MyTabsFriends";
            TheMultipage.PageViews[2].Selected = true;
        }

        if (Request.QueryString["G"] != null)
        {
            RadTabStrip2.Tabs[3].Selected = true;
            RadTabStrip2.Tabs[3].CssClass = "MyTabsGroupsSelected";
            RadTabStrip2.Tabs[0].CssClass = "MyTabsMessages";
            RadTabStrip2.Tabs[1].CssClass = "MyTabsFriends";
            RadTabStrip2.Tabs[2].CssClass = "MyTabsPreferences";
            TheMultipage.PageViews[3].Selected = true;
        }
    }

    protected void FillCategories(DataSet dsContent, ref Telerik.Web.UI.RadTreeView treeView)
    {
        if (treeView.Nodes.Count > 0)
        {
            if (dsContent.Tables.Count > 0)
                for (int i = 0; i < dsContent.Tables[0].Rows.Count; i++)
                {
                    Telerik.Web.UI.RadTreeNode node = (Telerik.Web.UI.RadTreeNode)treeView.FindNodeByValue(dsContent.Tables[0].Rows[i]["CategoryID"].ToString());
                    
                    if(node != null)
                        node.Checked = true;
                }
        }
    }

    protected void DoAll()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (Session["User"] != null)
        {
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Context.Request.Cookies[cookieName];

            FormsAuthenticationTicket authTicket = null;
            try
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                string group = authTicket.UserData.ToString();

                if (group.Contains("User"))
                {
                    Session["User"] = authTicket.Name;
                    Session["UserName"] = dat.GetData("SELECT UserName FROM USERS WHERE User_ID=" + Session["User"].ToString()).Tables[0].Rows[0]["UserName"].ToString();
                }
                else
                {
                    Button calendarLink = (Button)dat.FindControlRecursive(this, "CalendarLink");
                    calendarLink.Visible = false;
                    Response.Redirect("~/UserLogin.aspx");
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("~/UserLogin.aspx");
            }
            
            Session["UserName"] = dat.GetData("SELECT * FROM Users WHERE User_ID=" +
                Session["User"].ToString()).Tables[0].Rows[0]["UserName"].ToString();
            UserLabel.Text = Session["UserName"].ToString();
            ClearMessage();
            CalendarLink.NavigateUrl = "UserCalendar.aspx?ID=" + Session["User"].ToString();
            if (IsPostBack)
            {
                if (ViewState["FriendDS"] != null)
                    FillSearchPanel((DataSet)ViewState["FriendDS"]);
            }
            FillVenues();
            FillRecommendedEvents();

            
                LoadControlsNotAJAX();
            

            LoadFriends();

            LoadGlanceCalendar();
            FillGroups();
            //GetFriendEvents();

            

        }
        else
        {
            ErrorLabel.Text = "somtin happen";
            Response.Redirect("UserLogin.aspx");
        }
    }

    protected void LoadGlanceCalendar()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        int sun = 0;
        int mon = 0;
        int tues = 0;
        int wed = 0;
        int thurs = 0;
        int fri = 0;
        int sat = 0;
        DataView dv = dat.GetDataDV("SELECT DISTINCT EO.DateTimeStart AS TheDate, EO.EventID AS ID FROM User_Calendar UC, Event_Occurance EO WHERE " +
            "UC.EventID=EO.EventID AND UC.UserID="+Session["User"].ToString());

        DataView dvGroupEventMessages = dat.GetDataDV("SELECT DISTINCT GEO.DateTimeStart AS TheDate, E.ID FROM GroupEvent_Members UC, "+
            "GroupEvents E, " +
        " GroupEvent_Occurance GEO WHERE GEO.GroupEventID=E.ID AND UC.GroupEventID=E.ID AND UC.Accepted = " +
        "'True' AND UC.UserID=" + Session["User"].ToString());

        DataView dvGroupEventMessagesNonMember = dat.GetDataDV("SELECT DISTINCT GEO.DateTimeStart AS TheDate, E.ID FROM "+
            "User_GroupEvent_Calendar UC, GroupEvents E, GroupEvent_Occurance GEO WHERE  UC.GroupEventID=E.ID "+
            " AND UC.UserID=" + Session["User"].ToString() + " AND GEO.GroupEventID=E.ID ");

        DataView dvAll = MergeDVTwoCol(dv, MergeDVTwoCol(dvGroupEventMessages, dvGroupEventMessagesNonMember));

        int subtraction = 0;

        switch (DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).DayOfWeek)
        {
            case DayOfWeek.Friday:
                subtraction = 5;
                break;
            case DayOfWeek.Monday:
                subtraction = 1;
                break;
            case DayOfWeek.Saturday:
                subtraction = 6;
                break;
            case DayOfWeek.Sunday:
                subtraction = 0;
                break;
            case DayOfWeek.Thursday:
                subtraction = 4;
                break;
            case DayOfWeek.Tuesday:
                subtraction = 2;
                break;
            case DayOfWeek.Wednesday:
                subtraction = 3;
                break;
            default: break;
        }

        if (dvAll.Count > 0)
        {
            for (int i = 0; i < dvAll.Count; i++)
            {
                DateTime date = DateTime.Parse(dvAll[i]["TheDate"].ToString());


                switch (date.DayOfWeek)
                {
                    case DayOfWeek.Friday:
                        if (dat.IsThisWeek(date))
                            fri++;
                        GlanceDay6.DATE = date;
                        break;
                    case DayOfWeek.Monday:
                        if (dat.IsThisWeek(date))
                            mon++;
                        GlanceDay2.DATE = date;
                        break;
                    case DayOfWeek.Saturday:
                        if (dat.IsThisWeek(date))
                            sat++;
                        GlanceDay7.DATE = date;
                        break;
                    case DayOfWeek.Sunday:
                        if (dat.IsThisWeek(date))
                            sun++;
                        GlanceDay1.DATE = date;
                        break;
                    case DayOfWeek.Thursday:
                        if (dat.IsThisWeek(date))
                            thurs++;
                        GlanceDay5.DATE = date;
                        break;
                    case DayOfWeek.Tuesday:
                        if (dat.IsThisWeek(date))
                            tues++;
                        GlanceDay3.DATE = date;
                        break;
                    case DayOfWeek.Wednesday:
                        if (dat.IsThisWeek(date))
                            wed++;
                        GlanceDay4.DATE = date;
                        break;
                    default: break;
                }


            }
        }

        if (sun == 0)
            GlanceDay1.DATE = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Subtract(TimeSpan.FromDays(subtraction));
        GlanceDay1.NUM_OF_EVENTS = sun;

        if (mon == 0)
            GlanceDay2.DATE = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Subtract(TimeSpan.FromDays(subtraction-1));
        GlanceDay2.NUM_OF_EVENTS = mon;

        if (tues == 0)
            GlanceDay3.DATE = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Subtract(TimeSpan.FromDays(subtraction-2));
        GlanceDay3.NUM_OF_EVENTS = tues;

        if (wed == 0)
            GlanceDay4.DATE = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Subtract(TimeSpan.FromDays(subtraction-3));
        GlanceDay4.NUM_OF_EVENTS = wed;

        if (thurs == 0)
            GlanceDay5.DATE = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Subtract(TimeSpan.FromDays(subtraction-4));
        GlanceDay5.NUM_OF_EVENTS = thurs;

        if (fri == 0)
            GlanceDay6.DATE = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Subtract(TimeSpan.FromDays(subtraction-5));
        GlanceDay6.NUM_OF_EVENTS = fri;

        if (sat == 0)
            GlanceDay7.DATE = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Subtract(TimeSpan.FromDays(subtraction-6));
        GlanceDay7.NUM_OF_EVENTS = sat;

        GlanceDay1.USER_ID = Session["User"].ToString();
        GlanceDay2.USER_ID = Session["User"].ToString();
        GlanceDay3.USER_ID = Session["User"].ToString();
        GlanceDay4.USER_ID = Session["User"].ToString();
        GlanceDay5.USER_ID = Session["User"].ToString();
        GlanceDay6.USER_ID = Session["User"].ToString();
        GlanceDay7.USER_ID = Session["User"].ToString();

    }
    
    //protected void LoadControls_OLD()
    //{
    //    Telerik.Web.UI.RadPanelBar MessagePanelBar = new Telerik.Web.UI.RadPanelBar();
    //    MessagePanelBar.CausesValidation = false;
    //    MessagePanelBar.Width = 560;
    //    MessagePanelBar.ExpandMode = Telerik.Web.UI.PanelBarExpandMode.MultipleExpandedItems;
    //    MessagePanelBar.EnableEmbeddedSkins = false;
    //    MessagePanelBar.ID = "MessagePanelBar1";
    //    MessagePanelBar.ItemClick += new Telerik.Web.UI.RadPanelBarEventHandler(MarkAsRead);

    //    ClearMessage();
    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //    DataSet ds = dat.GetData("SELECT * FROM UserMessages WHERE Live='True' AND To_UserID="+
    //        Session["User"].ToString()+" ORDER BY Date DESC");
        
       
    //    System.Drawing.Color greyText = System.Drawing.Color.FromArgb(102, 102, 102);
    //    System.Drawing.Color greyBack = System.Drawing.Color.FromArgb(51, 51, 51);

    //    ASP.controls_pager_ascx pagerPanel = new ASP.controls_pager_ascx();
    //    pagerPanel.NUMBER_OF_ITEMS_PER_PAGE = 1;
    //    ArrayList a = new ArrayList(ds.Tables[0].Rows.Count);

    //    int unreadCount = 0;
    //    int count = 0;
    //    int times = 1;
    //    if (ds.Tables.Count > 0)
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //            {
                    
    //                if (count == 10*times)
    //                {
    //                    a.Add(MessagePanelBar);
    //                    times++;
    //                    MessagePanelBar = new Telerik.Web.UI.RadPanelBar();
    //                    MessagePanelBar.CausesValidation = false;
    //                    MessagePanelBar.Width = 560;
    //                    MessagePanelBar.ExpandMode = Telerik.Web.UI.PanelBarExpandMode.MultipleExpandedItems;
    //                    MessagePanelBar.EnableEmbeddedSkins = false;
    //                    MessagePanelBar.ID = "MessagePanelBar"+times.ToString();
    //                    MessagePanelBar.ItemClick += new Telerik.Web.UI.RadPanelBarEventHandler(MarkAsRead);
                        
    //                }
    //                count++;
    //                DataSet dsUsers = dat.GetData("SELECT * FROM Users WHERE User_ID=" + ds.Tables[0].Rows[i]["From_UserID"].ToString());

    //                if (!bool.Parse(ds.Tables[0].Rows[i]["Read"].ToString()))
    //                    unreadCount++;


    //                Telerik.Web.UI.RadPanelItem messageItem = new Telerik.Web.UI.RadPanelItem();
    //                messageItem.Expanded = false;
    //                messageItem.Height = 45;
    //                messageItem.BorderColor = greyBack;
    //                messageItem.BorderWidth = 1;
    //                messageItem.Attributes.Add("CommandArgument", ds.Tables[0].Rows[i]["ID"].ToString());
    //                messageItem.Attributes.Add("Read", ds.Tables[0].Rows[i]["Read"].ToString());
    //                messageItem.BorderStyle = BorderStyle.Solid;
    //                messageItem.BackColor = greyBack;

    //                string theUser = dsUsers.Tables[0].Rows[0]["UserName"].ToString();
    //                if (ds.Tables[0].Rows[i]["Mode"].ToString() == "1")
    //                {
    //                    theUser = "HippoHappenings";
    //                }

    //                string boldOrNot = "font-size: 12px;";

    //                if (!bool.Parse(ds.Tables[0].Rows[i]["Read"].ToString()))
    //                    boldOrNot = "font-weight:bold;font-size: 14px;";

    //                messageItem.Text = "<table  width=\"560px\" cellpadding=\"0\" cellspacing=\"0\"><tr><td><span style=\"padding-left:5px;font-family:Arial;color: #cccccc; " +
    //                    boldOrNot + "\">From: </span><span style=\"" + boldOrNot + "color: #1fb6e7; font-family: Arial;\">" + theUser
    //                    + "</span></td><td><span style=\"padding-top:10px;font-family:Arial;" +
    //                    boldOrNot + "color: #cccccc;float:right;padding-right:5px;\">" +
    //                    ds.Tables[0].Rows[i]["Date"].ToString() + "</span></td></tr><tr><td colspan=\"3\"><span style=\"font-family:Arial;" + boldOrNot + "color: #cccccc;padding-left: 5px;\">Subject: "
    //                    + ds.Tables[0].Rows[i]["MessageSubject"].ToString() +
    //                    "</span></td></tr></table>";


    //                messageItem.ForeColor = greyText;
    //                ASP.controls_usermessage_ascx message = new ASP.controls_usermessage_ascx();
    //                message.myEvent += new ASP.controls_usermessage_ascx.EventDelegate(this_OnProgress);

    //                message.ID = "message" + i.ToString();
    //                message.MESSAGE_TEXT = ds.Tables[0].Rows[i]["MessageContent"].ToString();
    //                message.SUBJECT_TEXT = ds.Tables[0].Rows[i]["MessageSubject"].ToString();
    //                message.TO_ID = int.Parse(ds.Tables[0].Rows[i]["To_UserID"].ToString());
    //                message.FROM_ID = int.Parse(ds.Tables[0].Rows[i]["From_UserID"].ToString());
    //                message.DATE = ds.Tables[0].Rows[i]["Date"].ToString();
    //                message.MESSAGE_ID = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
    //                message.CONTROL_ID = i;
    //                if (ds.Tables[0].Rows[i]["Mode"].ToString() == "1")
    //                    message.MODE = Controls_UserMessage.Mode.HippoRequest;
    //                else if (ds.Tables[0].Rows[i]["Mode"].ToString() == "2")
    //                    message.MODE = Controls_UserMessage.Mode.HippoReply;
    //                Telerik.Web.UI.RadPanelItem subItem = new Telerik.Web.UI.RadPanelItem();
    //                subItem.Controls.Add(message);


    //                messageItem.Items.Add(subItem);


    //                MessagePanelBar.Items.Add(messageItem);
                    
    //                //message.DATE = ds.Tables[0].Rows[i]["Date"].ToString();


    //                //MessagePanel.Controls.Add(message);
    //            }

    //            if (ds.Tables[0].Rows.Count % 10 != 0 || ds.Tables[0].Rows.Count == 10)
    //            {
    //                 a.Add(MessagePanelBar);
    //            }

    //        }

    //    pagerPanel.DATA = a;
    //    pagerPanel.DataBind2();

    //    Label label = new Label();

    //    string temp = "messages";
    //    if (ds.Tables[0].Rows.Count == 1)
    //        temp = "message";
    //    label.Text = "<span style=\"font-family: Arial; font-size: 20px; color: White;\">My Messages</span>"
    //        + "<span style=\"font-family: Arial; font-size: 12px; color: #cccccc; padding-left: 5px;\">(" + unreadCount.ToString()
    //        + " new " + temp + ")</span>";

    //    MessagesPanel.Controls.Clear();
    //    MessagesPanel.Controls.Add(label);
    //    MessagesPanel.Controls.Add(pagerPanel);

    //}

    protected int AddMessages(DataSet ds, ref ArrayList a, bool areSent)
    {
        //Mode 4,5: venue,event changes request
        //Mode 2: Friend request
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        string message = "";
        try
        {
            int itemCount = 0;
            int times = 1;
            int unreadCount = 0;

            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            Telerik.Web.UI.RadPanelBar bar = new Telerik.Web.UI.RadPanelBar();
            bar.BorderColor = greyBorder;
            bar.BorderWidth = 3;
            bar.ExpandAnimation.Type = Telerik.Web.UI.AnimationType.Linear;
            bar.ExpandAnimation.Duration = 50;
            bar.AllowCollapseAllItems = true;
            bar.ExpandMode = Telerik.Web.UI.PanelBarExpandMode.SingleExpandedItem;
            bar.Width = 570;


            int replyMessagesCount = 0;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                message = ds.Tables[0].Rows[i]["ID"].ToString();
                if (itemCount == 20 * times)
                {
                    if (!areSent)
                    {
                        bar.ItemClick += new Telerik.Web.UI.RadPanelBarEventHandler(ServerMarkRead);
                    }
                    a.Add(bar);
                    bar = new Telerik.Web.UI.RadPanelBar();

                    bar.BorderColor = greyBorder;
                    bar.BorderWidth = 3;
                    bar.ExpandAnimation.Type = Telerik.Web.UI.AnimationType.Linear;
                    bar.ExpandAnimation.Duration = 50;
                    bar.AllowCollapseAllItems = true;
                    bar.ExpandMode = Telerik.Web.UI.PanelBarExpandMode.SingleExpandedItem;
                    bar.Width = 570;

                    times++;
                }
                itemCount++;
                Telerik.Web.UI.RadPanelItem item = new Telerik.Web.UI.RadPanelItem();

                item.BackColor = greyDark;
                item.CssClass = "OneMessage";
                item.SelectedCssClass = "OneMessageSelected";


                #region Mark If Read
                if (!areSent)
                {
                    if (!bool.Parse(ds.Tables[0].Rows[i]["Read"].ToString()))
                    {
                        item.Text = "<div id=\"divID" + i.ToString() + "\" style=\"font-weight: bold; color: White;\"><div style=\"float: left;\">From: <span class=\"AddLinkNotBold\">" +
                            ds.Tables[0].Rows[i]["UserName"].ToString() +
                            "</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Subject:&nbsp;&nbsp;</div><div style=\"" +
                        "width: 200px; text-wrap: true; float: left;\">" + ds.Tables[0].Rows[i]["MessageSubject"].ToString() +
                            "</div><div style=\"float: right; margin-right: 8px;\">" +
                                ds.Tables[0].Rows[i]["Date"].ToString() + "</div></div>";
                        item.Value = ds.Tables[0].Rows[i]["ID"].ToString();
                    }
                    else
                    {
                        item.Text = "<div style=\"float: left; color: #cccccc;\">From: <span class=\"AddLinkNotBold\">" +
                            ds.Tables[0].Rows[i]["UserName"].ToString() +
                            "</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Subject:&nbsp;&nbsp;</div><div style=\"" +
                        "width: 200px; color: #cccccc; text-wrap: true; float: left;\">" + ds.Tables[0].Rows[i]["MessageSubject"].ToString() +
                            "</div><div style=\"float: right; margin-right: 8px; color: #cccccc;\">" +
                                ds.Tables[0].Rows[i]["Date"].ToString() + "</div>";
                        item.Value = ds.Tables[0].Rows[i]["ID"].ToString();
                    }
                }
                else
                {
                    item.Text = "<div style=\"float: left; color: #cccccc;\">To: <span class=\"AddLinkNotBold\">" + ds.Tables[0].Rows[i]["UserName"].ToString() +
                        "</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Subject:&nbsp;&nbsp;</div><div style=\"width: 200px;  color: #cccccc;text-wrap: true; float: left;\">" + ds.Tables[0].Rows[i]["MessageSubject"].ToString() +
                        "</div><div style=\"float: right; margin-right: 8px; color: #cccccc;\">" + ds.Tables[0].Rows[i]["Date"].ToString() + "</div>";
                    item.Value = ds.Tables[0].Rows[i]["ID"].ToString();
                }

                #endregion

                #region Create Delete Button
                item.Expanded = false;


                Telerik.Web.UI.RadPanelItem item2 = new Telerik.Web.UI.RadPanelItem();
                item2.BackColor = greyDark;
                item2.CssClass = "OneMessageContent";

                Literal wrapLit = new Literal();
                wrapLit.Text = "<div class=\"topDiv\" style=\"min-height: 200px; overflow:hidden;\">" +
                    "<div style=\"width: 100%;\"><div align=\"right\" style=\" padding-bottom: 10px;padding-right: 22px; display: block;\">";
                item2.Controls.Add(wrapLit);

                ImageButton xIt = new ImageButton();

                xIt.ID = ds.Tables[0].Rows[i]["From_UserID"].ToString() + "X" + i.ToString();
                xIt.Width = 16;
                xIt.Height = 16;
                xIt.ImageUrl = "~/image/X.png";
                xIt.AlternateText = "Delete Message";
                xIt.ToolTip = "Delete Message";
                xIt.CommandArgument = ds.Tables[0].Rows[i]["ID"].ToString();
                xIt.Click += new ImageClickEventHandler(ServerDeleteMessage);
                //xIt.Attributes.Add("onserverclick", "ServerDeleteMessage");
                xIt.Attributes.Add("onmouseover", "this.src = 'image/XSelected.png';");
                xIt.Attributes.Add("onmouseout", "this.src = 'image/X.png';");
                xIt.OnClientClick = "return confirm('Do you want to delete this message?');";

                item2.Controls.Add(xIt);

                #endregion

                #region Construct Message Content

                wrapLit = new Literal();
                wrapLit.Text = "</div></div><div style=\"width: 100%; display: block;\"><div style=\"float: left;\"> ";

                item2.Controls.Add(wrapLit);

                Label theMessage = new Label();
                theMessage.BackColor = greyDark;
                theMessage.CssClass = "OneMessageContent";
                theMessage.Width = 300;
                theMessage.Text = ds.Tables[0].Rows[i]["MessageContent"].ToString();

                string groupID = "";

                DataSet dsEvent = new DataSet();
                DataSet dsSentUser = new DataSet();
                if (ds.Tables[0].Rows[i]["Mode"].ToString() == "4")
                {
                   string abc = ds.Tables[0].Rows[i]["MessageContent"].ToString();
                    string [] delimeter = {",UserID:"};
                    string[] thetokens = abc.Replace("EventID:", "").Split(delimeter, StringSplitOptions.None);

                    string[] delimeter2 = { ",RevisionID:" };
                    string[] thetokens2 = thetokens[1].Split(delimeter2, StringSplitOptions.None);

                    dsSentUser = dat.GetData("SELECT * FROM Users WHERE User_ID="+thetokens2[0]);
                    dsEvent = dat.GetData("SELECT * FROM Events WHERE ID="+thetokens[0]);

                    theMessage.Text = "Hello from HippoHappenings,<br/><br/> The user <a href=\"" + dat.MakeNiceName(dsSentUser.Tables[0].Rows[0]["UserName"].ToString()) + "_Friend\" class=\"AddGreenLink\">" +
                        dsSentUser.Tables[0].Rows[0]["UserName"].ToString() + "</a> has requested to make a " +
                        "change to the event '" + dsEvent.Tables[0].Rows[0]["Header"].ToString() +
                        "'.<br/>Click <a class=\"AddLink\" href=\"" + dat.MakeNiceName(dsEvent.Tables[0].Rows[0]["Header"].ToString()) +
                        "_" + thetokens[0] + "_Event\">here</a> to view this event. <br/> <br/> We must fully stress that if you do not either accept or reject ALL the requested chanes " +
                        "within <span style=\"color: #ff7704; font-weight: bold;\">4 days</span>, your ownership of this event will be waived and taken over by someone else willing to be the moderator for this event." +
                        "<br/>For each one of the changes which you accept, please select 'Accept Changes' on the right. If no changes are listed on the right, this means " +
                        "the user chose to only add media (songs/videos/pictues) or add new categories which have been automatically added to the event.";
                }
                else if (ds.Tables[0].Rows[i]["Mode"].ToString() == "5")
                {
                    //VenueID:90,UserID:40,RevisionID:90
                    string abc = ds.Tables[0].Rows[i]["MessageContent"].ToString();
                    string[] delimeter = { ",UserID:" };
                    string[] thetokens = abc.Replace("VenueID:", "").Split(delimeter, StringSplitOptions.None);

                    string[] delimeter2 = { ",RevisionID:" };
                    string[] thetokens2 = thetokens[1].Split(delimeter2, StringSplitOptions.None);

                    dsSentUser = dat.GetData("SELECT * FROM Users WHERE User_ID=" + thetokens2[0]);
                    string fromUserName = dsSentUser.Tables[0].Rows[0]["UserName"].ToString();
                    dsEvent = dat.GetData("SELECT * FROM Venues WHERE ID=" + thetokens[0]);
                    string eventName = dsEvent.Tables[0].Rows[0]["Name"].ToString();
                    theMessage.Text = "Hello from HippoHappenings,<br/><br/> The user <a href=\"" + dat.MakeNiceName(fromUserName) + "_Friend\" class=\"AddGreenLink\">" +
                        fromUserName + "</a> has requested to make a " +
                        "change to the venue '" + eventName +
                        "'.<br/>Click <a class=\"AddLink\" href=\"" + dat.MakeNiceName(eventName) + "_" + thetokens[0] + "_Venue\">here</a> to view this venue.<br/> <br/> We must fully stress that if you do not either accept or reject ALL the requested chanes " +
                        "within <span style=\"color: #ff7704; font-weight: bold;\">7 days</span>, your ownership of this event will be waived and taken over by someone else willing to be the moderator for this event." +
                        "<br/>For each one of the changes, please select 'Accept' or 'Reject'. If no changes are listed on the right, this means " +
                        "the user chose to only add media (videos/pictues) which have been automatically added to the venue.";

                }
                else if (ds.Tables[0].Rows[i]["Mode"].ToString() == "7" ||
               ds.Tables[0].Rows[i]["Mode"].ToString() == "9")
                {
                    string usethis = ds.Tables[0].Rows[i]["MessageContent"].ToString();
                    char ab = usethis[0];
                    groupID = ab.ToString();
                    int next = 1;
                    ab = usethis[next];
                    groupID += ab.ToString();
                    while(ab != ' ')
                    {
                        ab = usethis[++next];
                        groupID += ab.ToString();
                    }
                    theMessage.Text = usethis.Substring(next);
                }
                else if (ds.Tables[0].Rows[i]["Mode"].ToString() == "8")
                {
                    string usethis = ds.Tables[0].Rows[i]["MessageContent"].ToString();
                    char ab = usethis[0];
                    groupID = ab.ToString();
                    int next = 1;
                    ab = usethis[next];
                    groupID += ab.ToString();
                    while (ab != ' ')
                    {
                        ab = usethis[++next];
                        groupID += ab.ToString();
                    }
                    DataView dvU = dat.GetDataDV("SELECT * FROM Users WHERE User_ID=" + ds.Tables[0].Rows[i]["From_UserID"].ToString());
                    DataView dvG = dat.GetDataDV("SELECT * FROM Groups WHERE ID=" + groupID);
                    theMessage.Text = "The user <a class=\"AddLink\" href=\"" +
                        dvU[0]["UserName"].ToString() + "_Friend\">" + dvU[0]["UserName"].ToString() + "</a> has requested to join the group '" +
                        dvG[0]["Header"].ToString() + "'. Here is their message: <br/>" + usethis.Substring(next);
                }


                if (ds.Tables[0].Rows[i]["From_UserID"].ToString() == dat.HIPPOHAPP_USERID.ToString() && theMessage.Text.Contains("My Preferences"))
                {
                    theMessage.Text = theMessage.Text.Replace("<a class=\"AddLink\" href=\"UserPreferences.aspx\">My Preferences</a>.", "");
                    Literal theLit = new Literal();
                    theLit.Text = "<div class=\"OneMessageContent\">" + theMessage.Text +
                        "<div style=\"cursor: pointer;\" onclick=\"SelectPreferences();\" class=\"AddLink\">My Preferences</div></div>";
                    item2.Controls.Add(theLit);
                }
                else
                {
                    item2.Controls.Add(theMessage);
                }


                wrapLit = new Literal();
                wrapLit.Text = "</div>";

                item2.Controls.Add(wrapLit);

                if (!bool.Parse(ds.Tables[0].Rows[i]["Read"].ToString()))
                    unreadCount++;

                if (ds.Tables[0].Rows[i]["Mode"].ToString() == "2")
                {
                    DataSet ds3 = dat.GetData("SELECT * FROM User_Friends WHERE UserID=" + Session["User"].ToString() +
                    " AND FriendID=" + ds.Tables[0].Rows[i]["From_UserID"].ToString());

                    bool hasFriend = false;

                    if (ds3.Tables.Count > 0)
                        if (ds3.Tables[0].Rows.Count > 0)
                            hasFriend = true;
                        else
                            hasFriend = false;
                    else
                        hasFriend = false;

                    if (!areSent)
                    {
                        if (!hasFriend)
                        {

                            HtmlButton img = new HtmlButton();

                            img.ID = ds.Tables[0].Rows[i]["From_UserID"].ToString() + "accept" + i.ToString();
                            img.Style.Value = "margin-top: 20px; margin-left: 50px;padding-bottom: 4px;height: 30px; width: 112px;background-color: transparent; " +
                            "color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat: " +
                            "no-repeat; border: 0;";
                            img.ServerClick += new EventHandler(ServerAcceptFriend);
                            img.Attributes.Add("onmouseover", "this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';");
                            img.Attributes.Add("onmouseout", "this.style.backgroundImage = 'url(image/PostButtonNoPost.png)';");
                            img.Attributes.Add("onclick", "this.innerHTML = 'Working...';this.disabled=true;this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';");

                            img.InnerText = "Accept Friend";


                            item2.Controls.Add(img);
                        }
                        else
                        {
                            Literal lit = new Literal();
                            lit.Text = "<div style=\"float: right; width: 220px;height: 30px; margin: 5px;\" class=\"AddGreenLink\">You have accepted this gal/guy as a friend! Good luck, you two!</div>";
                            item2.Controls.Add(lit);
                        }
                    }
                    else
                    {
                        if (!hasFriend)
                        {
                            Literal lit = new Literal();
                            lit.Text = "<div style=\"float: right; width: 220px;height: 30px; margin: 5px;\" class=\"AddGreenLink\">You are still waiting for a response from this user!</div>";
                            item2.Controls.Add(lit);
                        }
                        else
                        {
                            Literal lit = new Literal();
                            lit.Text = "<div style=\"float: right; width: 220px;height: 30px; margin: 5px;\" class=\"AddGreenLink\">Your friend has already accepted your invitation!</div>";
                            item2.Controls.Add(lit);
                        }
                    }
                }
                else if (ds.Tables[0].Rows[i]["Mode"].ToString() == "4" || 
                    ds.Tables[0].Rows[i]["Mode"].ToString() == "5")
                {
                    string abc = ds.Tables[0].Rows[i]["MessageContent"].ToString();
                    string [] delimeter = {",UserID:"};

                    string temp = "VenueID:";
                    if (ds.Tables[0].Rows[i]["Mode"].ToString() == "4")
                        temp = "EventID:";
                    string [] thetokens = abc.Replace(temp, "").Split(delimeter, StringSplitOptions.None);

                    string[] delimeter2 = { ",RevisionID:" };
                    string[] thetokens2 = thetokens[1].Split(delimeter2, StringSplitOptions.None);

                    string temp2 = "";
                    if (thetokens2[1].Trim() != "")
                    {
                        temp = "VenueRevisions";
                        if (ds.Tables[0].Rows[i]["Mode"].ToString() == "4")
                            temp = "EventRevisions";

                        DataSet dsChanges = dat.GetData("SELECT * FROM " + temp + " WHERE ID=" + thetokens2[1]);
                        
                        if (dsChanges.Tables[0].Rows.Count > 0)
                        {
                            temp = "Venues";
                            if (ds.Tables[0].Rows[i]["Mode"].ToString() == "4")
                                temp = "Events";

                            temp2 = "VenueID";
                            if (ds.Tables[0].Rows[i]["Mode"].ToString() == "4")
                                temp2 = "EventID";

                            DataSet dsEvent2 = dat.GetData("SELECT * FROM " + temp + " WHERE ID=" + dsChanges.Tables[0].Rows[0][temp2].ToString());

                            Literal theLit = new Literal();
                            theLit.Text = "<table style=\"margin-right: 10px;margin-bottom: 20px;color: #cccccc;border: solid 1px #1fb6e7;\"><tr><td>";
                            item2.Controls.Add(theLit);

                            int count = 1;
                            string tempstr = "<div class=\"topDiv\"><hr color=\"#1fb6e7\" size=\"1\" width=\"100%\"/></div></td></tr><tr><td>";

                            int tempInt = 9;
                            if (ds.Tables[0].Rows[i]["Mode"].ToString() == "4")
                                tempInt = 12;

                            for (int n = 0; n < tempInt; n++)
                            {
                                InsertRevision(ref item2, dsChanges, n, tempstr, ref count, ds, i);
                            }
                            if (ds.Tables[0].Rows[i]["Mode"].ToString() == "5")
                            {
                                CategoryChanges(ref item2, thetokens2, ref count, tempstr, true);
                            }

                            if (ds.Tables[0].Rows[i]["Mode"].ToString() == "4")
                            {
                                EventOccuranceChanges(ref item2, ref count, thetokens2, tempstr);
                                CategoryChanges(ref item2, thetokens2, ref count, tempstr, false);
                            }
                            theLit = new Literal();
                            theLit.Text = "</td></tr></table>";
                            item2.Controls.Add(theLit);
                        }
                    }
                }
                else if (ds.Tables[0].Rows[i]["Mode"].ToString() == "7")
                {
                    Literal lit = new Literal();
                    lit.Text = "<div style=\"width: 220px; float: right;\" >";
                    item2.Controls.Add(lit);
                    message = "SELECT * FROM Group_Members WHERE MemberID=" +
                        Session["User"].ToString() + " AND GroupID=" + groupID;
                    DataView dvMember = dat.GetDataDV(message);
                    if (dvMember.Count > 0)
                    {
                        if (bool.Parse(dvMember[0]["Accepted"].ToString()))
                        {
                            lit = new Literal();
                            lit.Text = "<label class=\"AddGreenLink\">You have accepted this membership.</label>";
                            item2.Controls.Add(lit);
                        }
                        else
                        {
                            HtmlButton img = new HtmlButton();
                            img.ID = "acceptMembership" + groupID;
                            img.Style.Value = "cursor: pointer;padding-bottom: 4px;height: 30px; width: 112px;background-color: transparent; " +
                            "color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat: " +
                            "no-repeat; border: 0;";
                            img.ServerClick += new EventHandler(AcceptMembership);
                            img.Attributes.Add("onmouseover", "this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';");
                            img.Attributes.Add("onmouseout", "this.style.backgroundImage = 'url(image/PostButtonNoPost.png)';");
                            img.Attributes.Add("onclick", "this.innerHTML = 'Working...';this.disabled=true;this.style.backgroundImage='url(image/PostButtonNoPostHover.png)';");

                            img.InnerText = "Accept";

                            item2.Controls.Add(img);
                        }
                        
                    }
                    else
                    {
                        lit = new Literal();
                        lit.Text = "<label class=\"AddGreenLink\">The invitation no longer stands.</label>";
                        item2.Controls.Add(lit);
                    }
                    lit = new Literal();
                    lit.Text = "</div>";
                    item2.Controls.Add(lit);

                }
                else if (ds.Tables[0].Rows[i]["Mode"].ToString() == "8")
                {
                    Literal lit = new Literal();
                    lit.Text = "<div style=\"width: 220px; float: right;\" >";
                    item2.Controls.Add(lit);

                    DataView dvMember = dat.GetDataDV("SELECT * FROM Group_Members WHERE MemberID=" + ds.Tables[0].Rows[i]["From_UserID"].ToString() + 
                        " AND GroupID=" + groupID);


                    bool getIt = false;
                    if (dvMember.Count > 0)
                    {
                        if (bool.Parse(dvMember[0]["Accepted"].ToString()))
                        {
                            lit = new Literal();
                            lit.Text = "<label class=\"AddGreenLink\">You have approved this membership.</label>";
                            item2.Controls.Add(lit);
                        }
                        else
                        {
                            getIt = true;
                        }
                    }
                    else
                        getIt = true;

                    if(getIt)
                    {
                        HtmlButton img = new HtmlButton();
                        img.ID = ds.Tables[0].Rows[i]["From_UserID"].ToString() + "acceptMembership" + groupID;
                        img.Style.Value = "cursor: pointer;padding-bottom: 4px;height: 30px; width: 112px;background-color: transparent; " +
                        "color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat: " +
                        "no-repeat; border: 0;";
                        img.ServerClick += new EventHandler(ApproveMembership);
                        img.Attributes.Add("onmouseover", "this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';");
                        img.Attributes.Add("onmouseout", "this.style.backgroundImage = 'url(image/PostButtonNoPost.png)';");
                        img.Attributes.Add("onclick", "this.innerHTML = 'Working...';this.disabled=true;this.style.backgroundImage='url(image/PostButtonNoPostHover.png)';");

                        img.InnerText = "Approve";

                        item2.Controls.Add(img);
                    }
                    lit = new Literal();
                    lit.Text = "</div>";

                    item2.Controls.Add(lit);

                }
                else if (ds.Tables[0].Rows[i]["Mode"].ToString() == "9")
                {
                    Literal lit = new Literal();
                    lit.Text = "<div style=\"width: 220px; float: right;\" >";
                    item2.Controls.Add(lit);

                    DataView dvReoccurr = dat.GetDataDV("SELECT * FROM GroupEvent_Occurance WHERE GroupEventID=" +
                        groupID);
                    bool getIt = false;
                    string reoccurrID = "";
                    bool noExit = false;
                    bool regEnded = false;
                    if (dvReoccurr.Count == 0)
                    {
                        getIt = true;
                        noExit = true;
                    }
                    else
                    {
                        reoccurrID = dvReoccurr[0]["ID"].ToString();

                        DataView dvMember = dat.GetDataDV("SELECT * FROM GroupEvent_Members WHERE UserID=" +
                            Session["User"].ToString() +
                            " AND GroupEventID=" + groupID + " AND ReoccurrID=" + reoccurrID);
                        if (dvMember.Count > 0)
                        {
                            if (dvMember[0]["Accepted"] != null)
                            {
                                if (dvMember[0]["Accepted"].ToString() != "")
                                {
                                    if (bool.Parse(dvMember[0]["Accepted"].ToString()))
                                    {
                                        lit = new Literal();
                                        lit.Text = "<label class=\"AddGreenLink\">You have accepted this invitation.</label>";
                                        item2.Controls.Add(lit);
                                    }
                                    else
                                    {
                                        getIt = true;
                                    }
                                }
                                else
                                    getIt = true;
                            }
                            else
                                getIt = true;
                        }
                        else
                            getIt = true;

                        regEnded = HasRegistrationEnded(groupID, reoccurrID);
                    }
                
                    if(getIt)
                    {
                        if (regEnded)
                        {
                            lit = new Literal();
                            lit.Text = "<label class=\"AddGreenLink\">Registration for this events has ended.</label>";
                            item2.Controls.Add(lit);
                        }
                        else if (noExit)
                        {
                            lit = new Literal();
                            lit.Text = "<label class=\"AddGreenLink\">Event no longer exists.</label>";
                            item2.Controls.Add(lit);
                        }
                        else
                        {
                            HtmlButton img = new HtmlButton();
                            img.ID = groupID + "acceptInvitation" + reoccurrID;
                            img.Style.Value = "cursor: pointer;padding-bottom: 4px;height: 30px; width: 112px;background-color: transparent; " +
                            "color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat: " +
                            "no-repeat; border: 0;";
                            img.ServerClick += new EventHandler(AcceptInvitation);
                            img.Attributes.Add("onmouseover", "this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';");
                            img.Attributes.Add("onmouseout", "this.style.backgroundImage = 'url(image/PostButtonNoPost.png)';");
                            img.Attributes.Add("onclick", "this.innerHTML = 'Working...';this.disabled=true;this.style.backgroundImage='url(image/PostButtonNoPostHover.png)';");

                            img.InnerText = "Accept";

                            item2.Controls.Add(img);
                        }
                    }
                    lit = new Literal();
                    lit.Text = "</div>";

                    item2.Controls.Add(lit);

                }
                else
                {
                    if (!areSent)
                    {

                        if (ds.Tables[0].Rows[i]["From_UserID"].ToString() == dat.HIPPOHAPP_USERID.ToString())
                        {

                        }
                        else
                        {
                            //Insert ability to reply to message
                            Literal lit = new Literal();
                            lit.Text = "<div style=\"width: 220px; float: right;\" >";

                            item2.Controls.Add(lit);

                            TextBox textbox = new TextBox();
                            textbox.ID = ds.Tables[0].Rows[i]["From_UserID"].ToString() + "textbox" + ds.Tables[0].Rows[i]["ID"].ToString();
                            textbox.Width = 200;
                            textbox.Height = 100;
                            textbox.TextMode = TextBoxMode.MultiLine;

                            item2.Controls.Add(textbox);

                            lit = new Literal();
                            lit.Text = "<br/><br/><br/>";

                            item2.Controls.Add(lit);

                            HtmlButton img = new HtmlButton();
                            img.ID = ds.Tables[0].Rows[i]["From_UserID"].ToString() + "reply" + ds.Tables[0].Rows[i]["ID"].ToString();
                            img.Style.Value = "cursor: pointer;padding-bottom: 4px;height: 30px; width: 112px;background-color: transparent; " +
                            "color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat: " +
                            "no-repeat; border: 0;";
                            img.ServerClick += new EventHandler(ServerReply);
                            img.Attributes.Add("onmouseover", "this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';");
                            img.Attributes.Add("onmouseout", "this.style.backgroundImage = 'url(image/PostButtonNoPost.png)';");
                            img.Attributes.Add("onclick", "this.innerHTML = 'Working...';this.disabled=true;this.style.backgroundImage='url(image/PostButtonNoPostHover.png)';");

                            img.InnerText = "Reply";

                            item2.Controls.Add(img);

                            lit = new Literal();
                            lit.Text = "</div>";

                            item2.Controls.Add(lit);

                            replyMessagesCount++;
                        }
                    }
                }

                wrapLit = new Literal();

                wrapLit.Text = "</div></div>";
                item2.Controls.Add(wrapLit);

                item.Items.Add(item2);
                bar.Items.Add(item);
                #endregion
            }

            if (ds.Tables[0].Rows.Count % 20 != 0 || ds.Tables[0].Rows.Count == 20)
            {
                if (!areSent)
                {
                    bar.ItemClick += new Telerik.Web.UI.RadPanelBarEventHandler(ServerMarkRead);
                }
                a.Add(bar);
            }

            return unreadCount;
        }
        catch (Exception ex)
        {
            UserErrorLabel.Text = ex.ToString() + "<br/>" + message;
            return 0;
        }
    }

    protected bool HasRegistrationEnded(string eventID, string reoccurrID)
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


        DataView dvGroup = dat.GetDataDV("SELECT * FROM GroupEvents WHERE ID=" + eventID);

        DataView dvGoingMember = dat.GetDataDV("SELECT * FROM GroupEvent_Members WHERE GroupEventID=" +
                        eventID + " AND Accepted='True' AND ReoccurrID=" +
                        reoccurrID);
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

    protected void InsertRevision(ref Telerik.Web.UI.RadPanelItem item2, DataSet dsChanges, int n, 
        string tempstr, ref int count, DataSet ds, int i)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        if (dsChanges.Tables[0].Rows[0][n + 3].ToString().Trim() != "")
        {

            Label theLab = new Label();
            if (count != 1)
                theLab.Text = tempstr;

            string content = "";





            string str = "venue";
            if (ds.Tables[0].Rows[i]["Mode"].ToString() == "4")
                str = "event";

            bool isVenue = false;
            if (str == "venue")
                isVenue = true;

            string colName = dsChanges.Tables[0].Columns[n + 3].ColumnName.ToLower();

            if (!isVenue && (colName == "zip" || colName == "state" || colName == "city" || colName == "country"))
            {

            }
            else
            {
                if (dsChanges.Tables[0].Columns[n + 3].ColumnName.ToLower() == "address")
                {
                    DataView dvV = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" +
                        dsChanges.Tables[0].Rows[0]["VenueID"].ToString());
                    if (dvV[0]["Country"].ToString() == "223")
                        content = dat.GetAddress(dsChanges.Tables[0].Rows[0][n + 3].ToString(), false);
                    else
                        content = dat.GetAddress(dsChanges.Tables[0].Rows[0][n + 3].ToString(), true);
                }
                else if (dsChanges.Tables[0].Columns[n + 3].ColumnName.ToLower() == "venue")
                {
                    DataView dvV = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + dsChanges.Tables[0].Rows[0][n + 3].ToString());
                    content = "<a class=\"AddLink\" target=\"_blank\" href=\"" +
                        dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
                        dsChanges.Tables[0].Rows[0][n + 3].ToString() +
                        "_Venue\">" + dvV[0]["Name"].ToString() + "</a>";
                }
                else
                {
                    content = dsChanges.Tables[0].Rows[0][n + 3].ToString();
                }



                theLab.Text += count + ". " + dsChanges.Tables[0].Columns[n + 3].ColumnName + " Change Request To: <br/><br/>" +
                    dat.BreakUpString(content, 30) + "<br/>";
                count++;
                item2.Controls.Add(theLab);

                bool notSeen = dat.isNotSeen(dsChanges, n);
                bool isApproved = dat.isApproved(dsChanges, n);

                bool changeHasBeenMade = true;

                if (notSeen && changeHasBeenMade)
                {
                    HtmlButton img = new HtmlButton();
                    img.Attributes.Add("commArg", str);
                    img.Attributes.Add("commandargument", dsChanges.Tables[0].Rows[0]["ID"].ToString() +
                        "accept" + (n + 3).ToString());
                    img.ID = dsChanges.Tables[0].Rows[0]["ID"].ToString() + "accept" + (n + 3).ToString();
                    img.Style.Value = "cursor: pointer;float: right;font-size: 11px;font-weight: bold;margin-top: 20px; padding-bottom: 4px;height: 30px; width: 112px;background-color: transparent; " +
                    "color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat: " +
                    "no-repeat; border: 0;";
                    img.ServerClick += new EventHandler(ServerAcceptChange);
                    img.Attributes.Add("onmouseover", "this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';");
                    img.Attributes.Add("onmouseout", "this.style.backgroundImage = 'url(image/PostButtonNoPost.png)';");
                    img.Attributes.Add("onclick", "this.innerHTML = 'Working...';this.disabled=true;this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';");

                    img.InnerText = "Accept";

                    item2.Controls.Add(img);

                    img = new HtmlButton();
                    img.Attributes.Add("commArg", str);
                    img.Attributes.Add("commandargument", dsChanges.Tables[0].Rows[0]["ID"].ToString() + "reject" +
                        (n + 3).ToString());
                    img.ID = dsChanges.Tables[0].Rows[0]["ID"].ToString() + "reject" + (n + 3).ToString();
                    img.Style.Value = "cursor: pointer;float: right;font-size: 11px;font-weight: bold;margin-top: 20px; padding-bottom: 4px;height: 30px; width: 112px;background-color: transparent; " +
                    "color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat: " +
                    "no-repeat; border: 0;";
                    img.ServerClick += new EventHandler(ServerRejectChange);
                    img.Attributes.Add("onmouseover", "this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';");
                    img.Attributes.Add("onmouseout", "this.style.backgroundImage = 'url(image/PostButtonNoPost.png)';");
                    img.Attributes.Add("onclick", "this.innerHTML = 'Working...';this.disabled=true;this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';");

                    img.InnerText = "Reject";

                    item2.Controls.Add(img);
                }
                else
                {

                    theLab = new Label();
                    Label lab2 = new Label();
                    if (isApproved)
                    {

                        theLab.Text = "<br/><br/><span  class=\"AddGreenLink FloatRight\">You have accepted this change.</span>";

                    }
                    else
                    {
                        theLab.Text = "<br/><br/><span  class=\"AddGreenLink FloatRight\">You have rejected this change.</span>";
                    }

                    item2.Controls.Add(lab2);
                    item2.Controls.Add(theLab);
                }
                Literal theLit = new Literal();
                theLit.Text = "</td></tr><tr><td>";

                item2.Controls.Add(theLit);

            }
        }
    }

    protected void EventOccuranceChanges(ref Telerik.Web.UI.RadPanelItem item2, ref int count, 
        string[] thetokens2, string tempstr)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet dsChanges = dat.GetData("SELECT * FROM EventRevisions_Occurance WHERE RevisionID=" + thetokens2[1]);

        if (dsChanges.Tables[0].Rows.Count > 0)
        {
            Label theLab = new Label();
            theLab.Text = tempstr + count.ToString() + ". The following re-occurance dates have been added <br/><br/>";
            count++;
            item2.Controls.Add(theLab);

            theLab = new Label();
            for (int n = 0; n < dsChanges.Tables[0].Rows.Count; n++)
            {

                theLab.Text += dsChanges.Tables[0].Rows[n]["DateTimeStart"].ToString() + "<br/><div class='topDiv'>";
            }

            item2.Controls.Add(theLab);

            if (dsChanges.Tables[0].Rows[0]["Approved"].ToString().Trim() == "")
            {
                HtmlButton img = new HtmlButton();
                img.Attributes.Add("commArg", "event");
                img.Attributes.Add("commandargument", thetokens2[1] + "occurance");
                img.ID = dsChanges.Tables[0].Rows[0]["ID"].ToString() + "occurance";
                img.Style.Value = "float: right;font-size: 11px;font-weight: bold;margin-top: 20px; padding-bottom: 4px;height: 30px; width: 112px;background-color: transparent; " +
                "color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat: " +
                "no-repeat; border: 0;";
                img.ServerClick += new EventHandler(ServerAcceptChange);
                img.Attributes.Add("onmouseover", "this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';");
                img.Attributes.Add("onmouseout", "this.style.backgroundImage = 'url(image/PostButtonNoPost.png)';");
                img.Attributes.Add("onclick", "this.innerHTML = 'Working...';this.disabled=true;this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';");

                img.InnerText = "Accept";

                item2.Controls.Add(img);

                img = new HtmlButton();
                img.Attributes.Add("commArg", "event");
                img.Attributes.Add("commandargument", thetokens2[1] + "occurance");
                img.ID = dsChanges.Tables[0].Rows[0]["ID"].ToString() + "Rejectoccurance";
                img.Style.Value = "float: right;font-size: 11px;font-weight: bold;margin-top: 20px; padding-bottom: 4px;height: 30px; width: 112px;background-color: transparent; " +
                "color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat: " +
                "no-repeat; border: 0;";
                img.ServerClick += new EventHandler(ServerRejectChange);
                img.Attributes.Add("onmouseover", "this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';");
                img.Attributes.Add("onmouseout", "this.style.backgroundImage = 'url(image/PostButtonNoPost.png)';");
                img.Attributes.Add("onclick", "this.innerHTML = 'Working...';this.disabled=true;this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';");

                img.InnerText = "Reject";

                item2.Controls.Add(img);
            }
            else
            {
                theLab = new Label();
                if (bool.Parse(dsChanges.Tables[0].Rows[0]["Approved"].ToString()))
                {
                    theLab.Text = "<br/><br/><span  class=\"AddGreenLink FloatRight\">You have accepted this change.</span>";
                }
                else
                {
                    theLab.Text = "<br/><br/><span  class=\"AddGreenLink FloatRight\">You have rejected this change.</span>";
                }
                item2.Controls.Add(theLab);
            }
            theLab = new Label();
            theLab.Text = "</div>";
            item2.Controls.Add(theLab);

        }
    }

    protected void CategoryChanges(ref Telerik.Web.UI.RadPanelItem item2, string[] thetokens2, 
        ref int count, string tempstr, bool isVenue)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        string categoryRevisions = "EventCategoryRevisions";
        string categories = "EventCategories";
        string nameID = "EventID";
        string EventOVenue = "event";
        if (isVenue)
        {
            categoryRevisions = "VenueCategoryRevisions";
            categories = "VenueCategories";
            nameID = "VenueID";
            EventOVenue = "venue";
        }

        DataSet dsChanges = dat.GetData("SELECT * FROM "+categoryRevisions+" VCR, "+categories+" VC " +
                                    "WHERE VCR.CatID=VC.ID AND VCR.RevisionID=" + thetokens2[1]);
   
        if (dsChanges.Tables.Count > 0)
        {
            if (dsChanges.Tables[0].Rows.Count > 0)
            {
                Literal theLab = new Literal();
                theLab.Text = tempstr + count.ToString() + ". The following category changes have been suggested <br/><br/>";
                count++;
                item2.Controls.Add(theLab);
                string tempS = "Add ";
                HtmlButton theButt;
                Literal lab;
                Literal lit;
                HtmlButton rejectButt;

                for (int h = 0; h < dsChanges.Tables[0].Rows.Count; h++)
                {
                    lit = new Literal();
                    lit.Text = "<div style=\"width: 240px;\">";
                    item2.Controls.Add(lit);
                    theButt = new HtmlButton();
                    rejectButt = new HtmlButton();
                    rejectButt.Style.Value = "cursor: pointer;font-size: 11px;font-weight: bold;margin-top: 20px; padding-bottom: 4px;height: 30px; width: 112px;background-color: transparent; " +
                "color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat: " +
                "no-repeat; border: 0;";
                    theButt.Style.Value = "cursor: pointer;font-size: 11px;font-weight: bold;margin-top: 20px; padding-bottom: 4px;height: 30px; width: 112px;background-color: transparent; " +
                "color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat: " +
                "no-repeat; border: 0;";
                    theButt.Attributes.Add("commandargument", dsChanges.Tables[0].Rows[h][nameID].ToString() + "category" + EventOVenue + "category" + dsChanges.Tables[0].Rows[h]["CatID"].ToString() + "category" + dsChanges.Tables[0].Rows[h]["ID"].ToString());
                    rejectButt.Attributes.Add("commandargument", dsChanges.Tables[0].Rows[h][nameID].ToString() + "category" + EventOVenue + "category" + dsChanges.Tables[0].Rows[h]["CatID"].ToString() + "category" + dsChanges.Tables[0].Rows[h]["ID"].ToString());

                    theButt.Attributes.Add("onmouseover", "this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';");
                    theButt.Attributes.Add("onmouseout", "this.style.backgroundImage = 'url(image/PostButtonNoPost.png)';");
                    theButt.Attributes.Add("onclick", "this.innerHTML = 'Working...';this.disabled=true;this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';");

                    rejectButt.Attributes.Add("onmouseover", "this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';");
                    rejectButt.Attributes.Add("onmouseout", "this.style.backgroundImage = 'url(image/PostButtonNoPost.png)';");
                    rejectButt.Attributes.Add("onclick", "this.innerHTML = 'Working...';this.disabled=true;this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';");

                    theLab = new Literal();
                    Label lab2 = new Label();
                    lit = new Literal();
                    if (!bool.Parse(dsChanges.Tables[0].Rows[h]["AddOrRemove"].ToString()))
                    {
                        if (dsChanges.Tables[0].Rows[h]["Approved"].ToString().Trim() != "")
                        {
                            if (dsChanges.Tables[0].Rows[h]["Approved"].ToString().ToLower() == "true")
                            {
                                lab = new Literal();


                                lab.Text = "<label class=\"AddGreenLink\">&nbsp;&nbsp;&nbsp;&nbsp;Removed</label></div>";
                                theLab.Text = "<div align=\"right\" style=\"margin-left: 10px;padding-bottom: 4px;\">" + tempS + dsChanges.Tables[0].Rows[h]["Name"].ToString();
                                item2.Controls.Add(theLab);
                                item2.Controls.Add(lab2);
                                
                                item2.Controls.Add(lab);
                                lit.Text = "<br/><br/>";
                                item2.Controls.Add(lit);
                            }
                            else
                            {
                                lab = new Literal();


                                lab.Text = "<label class=\"AddGreenLink\">&nbsp;&nbsp;&nbsp;&nbsp;Rejected</label></div>";
                                theLab.Text = "<div align=\"right\" style=\"margin-left: 10px;padding-bottom: 4px;\">" + tempS + dsChanges.Tables[0].Rows[h]["Name"].ToString();
                                item2.Controls.Add(theLab);
                                item2.Controls.Add(lab2);
                                item2.Controls.Add(lab);
                                lit.Text = "<br/><br/>";

                                
                                item2.Controls.Add(lit);
                            }
                        }
                        else
                        {
                            tempS = "Remove ";
                            theLab.Text ="<div style=\"padding-bottom: 4px;\">"+ tempS + dsChanges.Tables[0].Rows[h]["Name"].ToString()+"<br/>";
                            item2.Controls.Add(theLab);
                            theButt.InnerHtml = tempS;
                            rejectButt.InnerHtml = "Reject";
                            rejectButt.ServerClick += new EventHandler(RejectCategory);
                            theButt.ServerClick += new EventHandler(RemoveCategory);
                            item2.Controls.Add(theButt);
                            item2.Controls.Add(rejectButt);
                            lit.Text = "</div><br/>";

                            item2.Controls.Add(lit);
                        }

                    }
                    else
                    {
                        if (dsChanges.Tables[0].Rows[h]["Approved"].ToString().Trim() != "")
                        {
                            if (dsChanges.Tables[0].Rows[h]["Approved"].ToString().ToLower() == "true")
                            {
                                lab = new Literal();
                                lab.Text = "<label class=\"AddGreenLink\">&nbsp;&nbsp;&nbsp;&nbsp;Added</label></div>";

                                theLab.Text = "<div align=\"right\" style=\"margin-left: 10px;padding-bottom: 4px;\">" +
                                    tempS + dsChanges.Tables[0].Rows[h]["Name"].ToString();
                                item2.Controls.Add(theLab);
                                item2.Controls.Add(lab);
                                lit.Text = "<br/><br/>";

                                item2.Controls.Add(lab2);
                                item2.Controls.Add(lit);
                            }
                            else
                            {
                                lab = new Literal();
                                lab.Text = "<label class=\"AddGreenLink\">&nbsp;&nbsp;&nbsp;&nbsp;Rejected</label></div>";

                                theLab.Text = "<div align=\"right\" style=\"margin-left: 10px;padding-bottom: 4px;\">" +
                                    tempS + dsChanges.Tables[0].Rows[h]["Name"].ToString();
                                item2.Controls.Add(theLab);
                                item2.Controls.Add(lab2);
                                item2.Controls.Add(lab);
                                lit.Text = "<br/><br/>";

                                
                                item2.Controls.Add(lit);
                            }
                        }
                        else
                        {
                            tempS = "Add ";
                            theButt.ServerClick += new EventHandler(AddCategory);
                            rejectButt.ServerClick += new EventHandler(RejectCategory);
                            theLab.Text = "<div style=\"padding-bottom: 4px;\">" + tempS + 
                                dsChanges.Tables[0].Rows[h]["Name"].ToString()+"<br/>";
                            item2.Controls.Add(theLab);
                            theButt.InnerHtml = tempS;
                            rejectButt.InnerHtml = "Reject";
                            item2.Controls.Add(theButt);
                            item2.Controls.Add(rejectButt);
                            lit.Text = "</div><br/><br/>";
                            item2.Controls.Add(lit);
                        }
                    }


                    lit = new Literal();
                    lit.Text = "</div>";
                    item2.Controls.Add(lit);


                }
            }
        }
    }

    protected void LoadControlsNotAJAX()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        try
        {
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            DataSet ds = dat.GetData("SELECT TOP 100 UM.MessageSubject, UM.ID, UM.From_UserID, UM.MessageContent, UM.[Read], UM.Date, U.UserName, UM.Mode FROM UserMessages UM, Users U WHERE UM.Live='True' AND UM.To_UserID=" +
                Session["User"].ToString() + " AND UM.From_UserID=U.User_ID AND UM.LIVE=1 ORDER BY Date DESC");

            ASP.controls_pager_ascx pagerPanel = new ASP.controls_pager_ascx();
            pagerPanel.NUMBER_OF_ITEMS_PER_PAGE = 1;
            pagerPanel.WIDTH = 576;
            pagerPanel.NUMBER_OF_VISIBLE_PAGES = 4;
            ArrayList a = new ArrayList(ds.Tables[0].Rows.Count);

            if (ds.Tables[0].Rows.Count == 0)
            {
                Literal lit = new Literal();
                lit.Text = "<div style=\"border: solid 3px #1b1b1b;" +
               "float: left; width: 570px;\"><div style=\"width: 560px; display: block; float: left; margin" +
               "-left: 5px;\"><table width=\"100%\"><tr><td><span style=\"font-family: Arial; font-size: 12px;" +
               "color: #cccccc; padding-left: 5px;\">(0 messages)</span></td></tr></table></div>" +
               "<div style=\"width: 560px; display: block; float: left; margin-left: 5px; padding-bottom: 5px;\">" +
               "</div></div>";
                MessagesPanel.Controls.Add(lit);
            }
            else
            {
                int unreadCount = AddMessages(ds, ref a, false);

                pagerPanel.DATA = a;
                pagerPanel.DataBind2();

                Label label = new Label();

                string temp = "messages";
                if (ds.Tables[0].Rows.Count == 1)
                    temp = "message";

                if (unreadCount > 0)
                {
                    RadTabStrip3.Tabs[0].Text = "Inbox <span style=\"font-size: 12px; vertical-align: middle; font-weight: normal; padding-bottom: 3px;\">(" + unreadCount.ToString() + " New)</span>";
                    //label.Text = "<span style=\"font-family: Arial; font-size: 20px; color: White;\">My Messages</span>"
                    //    + "<span style=\"font-family: Arial; font-size: 12px; color: #cccccc; padding-left: 5px;\">(" + unreadCount.ToString()
                    //    + " new " + temp + ")</span>";
                    RadTabStrip3.Tabs[0].Value = unreadCount.ToString();
                }
                MessagesPanel.Controls.Clear();
                //MessagesPanel.Controls.Add(label);
                MessagesPanel.Controls.Add(pagerPanel);
            }





            ds = dat.GetData("SELECT TOP 100 UM.MessageContent, UM.ID, UM.To_UserID AS From_UserID, UM.MessageSubject, UM.[Read], UM.Mode, UM.Date, U.UserName FROM UserMessages UM, Users U WHERE UM.Live='True' AND UM.From_UserID=" +
                Session["User"].ToString() + " AND U.User_ID=UM.To_UserID AND UM.LIVE=1 ORDER BY Date DESC");

            pagerPanel = new ASP.controls_pager_ascx();
            pagerPanel.NUMBER_OF_ITEMS_PER_PAGE = 1;
            pagerPanel.WIDTH = 576;
            a = new ArrayList(ds.Tables[0].Rows.Count);

            if (ds.Tables[0].Rows.Count == 0)
            {
                Literal lit = new Literal();
                lit.Text = "<div style=\"border: solid 3px #1b1b1b;" +
               "float: left; width: 570px;\"><div style=\"width: 560px; display: block; float: left; margin" +
               "-left: 5px;\"><table width=\"100%\"><tr><td><span style=\"font-family: Arial; font-size: 12px;" +
               "color: #cccccc; padding-left: 5px;\">(0 sent messages)</span></td></tr></table></div>" +
               "<div style=\"width: 560px; display: block; float: left; margin-left: 5px; padding-bottom: 5px;\">" +
               "</div></div>";
                UsedMessagesPanel.Controls.Add(lit);
            }
            else
            {

                AddMessages(ds, ref a, true);

                pagerPanel.DATA = a;
                pagerPanel.DataBind2();

                UsedMessagesPanel.Controls.Clear();
                UsedMessagesPanel.Controls.Add(pagerPanel);
            }
        }
        catch (Exception ex)
        {
            UserErrorLabel.Text = ex.ToString();
        }
    }

    protected void LoadControls()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        TheDiv.InnerHtml = "";

        

        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet ds = dat.GetData("SELECT UM.MessageSubject, UM.MessageContent, U.UserName FROM UserMessages UM, Users U WHERE UM.Live='True' AND UM.To_UserID=" +
            Session["User"].ToString() + " AND UM.From_UserID=U.User_ID ORDER BY Date DESC");


        System.Drawing.Color greyText = System.Drawing.Color.FromArgb(102, 102, 102);
        System.Drawing.Color greyBack = System.Drawing.Color.FromArgb(51, 51, 51);

        ASP.controls_pager_ascx pagerPanel = new ASP.controls_pager_ascx();
        pagerPanel.NUMBER_OF_ITEMS_PER_PAGE = 1;
        ArrayList a = new ArrayList(ds.Tables[0].Rows.Count);

        int unreadCount = 0;
        int count = 0;
        int times = 1;

        Panel ItemsPanel = new Panel();
        ItemsPanel.ID = "PanelM1";
        if (ds.Tables.Count > 0)
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    if (count == 10 * times)
                    {
                        a.Add(ItemsPanel);
                        times++;
                        ItemsPanel = new Panel();
                        ItemsPanel.ID = "PanelM" + times.ToString();
                    }
                    count++;
                    DataSet dsUsers = dat.GetData("SELECT * FROM Users WHERE User_ID=" + 
                        ds.Tables[0].Rows[i]["From_UserID"].ToString());

                    if (!bool.Parse(ds.Tables[0].Rows[i]["Read"].ToString()))
                        unreadCount++;

                    string boldOrNot = "font-size: 12px;";

                    if (!bool.Parse(ds.Tables[0].Rows[i]["Read"].ToString()))
                        boldOrNot = "font-weight:bold;font-size: 14px;";

                    HtmlGenericControl thediv = new HtmlGenericControl();
                    thediv.Attributes.Add("class", "MailHeader");
                    thediv.Attributes.Add("style", boldOrNot);
                    thediv.Attributes.Add("onclick", "OpenEmail('" + i.ToString() + "');");
                    //thediv.Attributes.Add("onserverclick", "MarkAsRead2");
                    thediv.ID = "header" + i.ToString();
                    


                    string thedivsText = "";
                    //thedivsText = "<div class=\"MailHeader\" style=\""+boldOrNot+"\" "+
                    //    "onclick=\"OpenEmail('"+i.ToString()+"');\" id=\"header"+i.ToString()+"\">";

                    //thedivsText += "<div style=\"display: none;\" id=\"readDiv"+i.ToString()+"\">"+
                    //    ds.Tables[0].Rows[i]["Read"].ToString()+"</div>";
                    //thedivsText += "<div style=\"display: none;\" id=\"idDiv"+i.ToString()+"\">"+
                    //    ds.Tables[0].Rows[i]["ID"].ToString()+"</div>";
                    //thedivsText += "<div style=\"display: none;\" id=\"userDiv" + i.ToString() + "\">" + 
                    //    Session["User"].ToString() + "</div>";
                    //thedivsText += "<div style=\"display: none;\" id=\"fromDiv" + i.ToString() + "\">" + 
                    //    ds.Tables[0].Rows[i]["From_UserID"].ToString() + "</div>";

                    string tempor2 = "";
                    string theUser = dsUsers.Tables[0].Rows[0]["UserName"].ToString();
                    if (ds.Tables[0].Rows[i]["Mode"].ToString() == "1")
                    {
                        theUser = "HippoHappenings";
                        tempor2 = " style=\"display:none;\" ";
                    }

                    string acceptHTML = "";
                    string tempor = "";
                    
                    if (ds.Tables[0].Rows[i]["Mode"].ToString() == "2")
                    {
                        tempor2 = " style=\"display:none;\" ";
                        tempor = "<a class=\"AddGreenLink\" href=\"javascript:AcceptFriend('" + i.ToString() + "');\">Accept Friend Request</a>"; 
                        DataSet isFirend = dat.GetData("SELECT * FROM User_Friends WHERE UserID="+Session["User"].ToString()+" AND FriendID="+ds.Tables[0].Rows[i]["From_UserID"].ToString());
                        if (isFirend.Tables.Count > 0)
                            if (isFirend.Tables[0].Rows.Count > 0)
                                tempor = "<span class=\"AddGreenLink\">You are already friends with "+theUser + "</span>";
                        acceptHTML = "<div class=\"AddGreenLink\" id=\"accept"+i.ToString()+"\">"+tempor+"</div><br/>";
                    }

                    thedivsText += "<table width=\"560px\" cellpadding=\"0\" cellspacing=\"0\"><tr><td><span style=\"padding-left:5px;font-family:Arial;color: #cccccc; "
                        + "\">From: </span><span style=\"color: #1fb6e7; font-family: Arial;\">" + theUser
                        + "</span></td><td ><span style=\"padding-top:10px;font-family:Arial;" +
                        "color: #cccccc;float:right;padding-right:5px;\">" +
                        ds.Tables[0].Rows[i]["Date"].ToString() + "</span></td></tr><tr><td colspan=\"3\"><span style=\"font-family:Arial;color: #cccccc;padding-left: 5px;\">Subject: <span id=\"subject" + i.ToString() + "\">"
                        + ds.Tables[0].Rows[i]["MessageSubject"].ToString() + 
                        "</span></span></td></tr></table>";

                    thedivsText += "</div>";
                    thediv.InnerHtml = thedivsText;

                    HtmlGenericControl messageDiv = new HtmlGenericControl();
                    messageDiv.Attributes.Add("style", "display: none;");
                    messageDiv.Attributes.Add("class", "MailContent EventBody");
                    messageDiv.ID = "contentDiv";

                    string messageDivsText = "";


                    //thedivsText += "<div style=\"display: none;\" class=\"MailContent EventBody\" id=\"contentDiv"+
                    //    i.ToString()+"\">";
                    messageDivsText += "<table><tr><td width=\"285px\" valign=\"top\"><label>" + 
                        ds.Tables[0].Rows[i]["MessageContent"].ToString()+" <br/><br/><br/> " +acceptHTML + "</label></td>";
                    messageDivsText += "<td valign=\"top\"><table ><tr><td width=\"285px\" align=\"right\">" +
                        "<img alt=\"Delete Message\" name=\"Delete Message\" style=\"cursor: pointer;\" "+
                        "onclick=\"DeleteEmail('" + i.ToString() + "');\" src=\"image/X.png\" "+
                        "onmouseover=\"this.src='image/XSelected.png'\" onmouseout=\"this.src='image/X.png'\" />"+
                        "</td></tr><tr><td " + tempor2 + "><textarea cols=\"30\" rows=\"10\" id=\"textDiv" + 
                        i.ToString() + "\"></textarea></td></tr>";
                    messageDivsText += "<tr><td " + tempor2 + "><img onclick=\"ReplyMessage('" + i.ToString() + 
                        "');\" src=\"image/ReplyButton.png\" onmouseout=\"this.src='image/ReplyButton.png'\""+
                        "onmouseover=\"this.src='image/ReplyButtonSelected.png'\" /></td></tr>";
                    messageDivsText += "<tr><td><div class=\"AddLink\" id=\"message" + i.ToString() + "\"></div></td></tr>";
                    messageDivsText += "</table></td></tr></table>";
                    //thedivsText += "</div>";

                    messageDiv.InnerHtml = messageDivsText;

                    ItemsPanel.Controls.Add(thediv);


                    //message.DATE = ds.Tables[0].Rows[i]["Date"].ToString();


                    //MessagePanel.Controls.Add(message);
                }

                if (ds.Tables[0].Rows.Count % 10 != 0 || ds.Tables[0].Rows.Count == 10)
                {
                    a.Add(ItemsPanel);
                }

            }

        if (a.Count > 0)
        {
            pagerPanel.DATA = a;
            pagerPanel.DataBind2();
        }
        Label label = new Label();

        string temp = "messages";
        if (ds.Tables[0].Rows.Count == 1)
            temp = "message";
        label.Text = "<table width=\"570px\"><tr><td><span style=\"font-family: Arial; font-size: 20px; color: White;\">My Messages</span>"
            + "<span style=\"font-family: Arial; font-size: 12px; color: #cccccc; padding-left: 5px;\">(<span id=\"messagesCount\">" + unreadCount.ToString()
            + "</span> new " + temp + ")</span></td><td align=\"right\"><div class=\"AddGreenLink\" id=\"globalMessage\"></div></td></tr></table>";

        Panel MessagesPanel = (Panel)Tab1.FindControl("MessagesPanel");

        MessagesPanel.Controls.Clear();
        MessagesPanel.Controls.Add(label);
        MessagesPanel.Controls.Add(pagerPanel);
    }

    protected DataView MergeDVThreeCol(DataView dv1, DataView dv2)
    {
        //even if dv1 is empty, we still want to use 
        //the method to convert the date column to datetime
        DataView dv;
        DataTable dt = new DataTable();
        DataColumn dc = new DataColumn();
        dc.ColumnName = "UserName";
        dc.DataType = typeof(String);
        dt.Columns.Add(dc);
        
        dc = new DataColumn();
        dc.ColumnName = "THE_DATE";
        dc.DataType = typeof(DateTime);
        dt.Columns.Add(dc);

        dc = new DataColumn();
        dc.ColumnName = "HEADER";
        dc.DataType = typeof(String);
        dt.Columns.Add(dc);

        dc = new DataColumn();
        dc.ColumnName = "ProfilePicture";
        dc.DataType = typeof(String);
        dt.Columns.Add(dc);

        DataRow row;
        if (dv1.Count != 0)
        {
            for (int i = 0; i < dv1.Count; i++)
            {
                row = dt.NewRow();
                row["UserName"] = dv1[i]["UserName"];
                row["THE_DATE"] = DateTime.Parse(dv1[i]["THE_DATE"].ToString());
                row["HEADER"] = dv1[i]["HEADER"];
                row["ProfilePicture"] = dv1[i]["ProfilePicture"];
                dt.Rows.Add(row);
            }
        }
       
        if(dv2.Count != 0)
        {
            for (int i = 0; i < dv2.Count; i++)
            {
                row = dt.NewRow();
                row["UserName"] = dv2[i]["UserName"];
                row["THE_DATE"] = DateTime.Parse(dv2[i]["THE_DATE"].ToString());
                row["HEADER"] = dv2[i]["HEADER"];
                row["ProfilePicture"] = dv2[i]["ProfilePicture"];
                dt.Rows.Add(row);
            }
        }

        DataView newDV = new DataView(dt, "", "", DataViewRowState.CurrentRows);
        newDV.Sort = "THE_DATE DESC";

        return newDV;
    }

    protected DataView MergeDVTwoCol(DataView dv1, DataView dv2)
    {
        //even if dv1 is empty, we still want to use 
        //the method to convert the date column to datetime
        DataView dv;
        DataTable dt = new DataTable();
        DataColumn dc = new DataColumn();

        dc.ColumnName = "TheDate";
        dc.DataType = typeof(DateTime);
        dt.Columns.Add(dc);

        dc = new DataColumn();
        dc.ColumnName = "ID";
        dt.Columns.Add(dc);

        DataRow row;
        if (dv1.Count != 0)
        {
            for (int i = 0; i < dv1.Count; i++)
            {
                row = dt.NewRow();
                row["TheDate"] = dv1[i]["TheDate"];
                row["ID"] = dv1[i]["ID"];
                dt.Rows.Add(row);
            }
        }

        if (dv2.Count != 0)
        {
            for (int i = 0; i < dv2.Count; i++)
            {
                row = dt.NewRow();
                row["TheDate"] = dv2[i]["TheDate"];
                row["ID"] = dv2[i]["ID"];
                dt.Rows.Add(row);
            }
        }

        DataView newDV = new DataView(dt, "", "", DataViewRowState.CurrentRows);

        return newDV;
    }


    protected void LoadFriends()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        ClearMessage();
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet dsFriends = dat.GetData("SELECT * FROM User_Friends UF, Users U WHERE UF.FriendID=U.User_ID AND UF.UserID=" + Session["User"].ToString());
        Panel FriendPanel = (Panel)Tab1.FindControl("FriendPanel");

        

        //Search for what your friends did in the past 30 days.
        Panel WhatMyFriendsDidPanel = (Panel)Tab2.FindControl("WhatMyFriendsDidPanel");
        DataView dvFriends = new DataView(dsFriends.Tables[0], "", "", DataViewRowState.CurrentRows);

        if (dvFriends.Count > 0)
        {
            //Added Events to calendar
            DataView dvEvents = dat.GetDataDV("SELECT U.UserName,  U.ProfilePicture, UC.DateAdded AS THE_DATE, 'Added the event <a class=AddLink target=_blank href=' + dbo.MAKENICENAME(E.Header) + '_' + CONVERT(NVARCHAR,E.ID)+'_Event>'+ CASE WHEN LEN(E.Header) > 20 THEN SUBSTRING(E.Header, 0, 20) + '...' ELSE E.Header END +'</a>' AS HEADER " +
                "FROM User_Friends UF, Users U, User_Calendar UC, Events E WHERE UF.UserID=" +
                Session["User"].ToString() + " AND U.User_ID=UF.FriendID AND E.ID=UC.EventID " +
                "AND UF.FriendID=UC.UserID AND UC.DateAdded > CONVERT(DATETIME, '" +
                DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A",
                ":")).AddDays(double.Parse("-10")).Date.ToString() + "') ORDER BY THE_DATE DESC");

            //Added Group Events to Calendar
            DataView dvGroupEvents = dat.GetDataDV("SELECT U.UserName, U.ProfilePicture, UGEC.DateAdded AS THE_DATE, " +
                "'Added the group event <a target=_blank class=AddLink href='+ " +
                "dbo.MAKENICENAME(GE.Name)+'_'+CONVERT(NVARCHAR, GEO.ID)+'_'+CONVERT(NVARCHAR,GE.ID)+'_GroupEvent>'+ " +
                "CASE WHEN LEN(GE.Name) " +
                "> 20 THEN SUBSTRING(GE.Name, 0, 20) + '...' ELSE GE.Name END +'</a>' AS HEADER " +
                " FROM User_GroupEvent_Calendar UGEC, GroupEvents GE, GroupEvent_Occurance GEO, User_Friends UF, Users U " +
                "WHERE GE.EventType=1 AND GEO.GroupEventID=GE.ID AND UGEC.GroupEventID=GE.ID AND GE.ID=UGEC.GroupEventID " +
                " AND UF.UserID=" + Session["User"].ToString() + " AND U.User_ID=UF.FriendID AND " +
                "UF.FriendID=UGEC.UserID AND UGEC.DateAdded > CONVERT(DATETIME, '" +
                DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A",
                ":")).AddDays(double.Parse("-10")).Date.ToString() + "')  ORDER BY THE_DATE DESC");

            dvEvents = MergeDVThreeCol(dvEvents, dvGroupEvents);

            //Added Group Events to Calendar
            DataView dvGroupEvents2 = dat.GetDataDV("SELECT U.UserName, U.ProfilePicture, GEM.DateAdded AS THE_DATE, " +
                "'Added the group event <a target=_blank class=AddLink href='+ " +
                "dbo.MAKENICENAME(GE.Name)+'_'+CONVERT(NVARCHAR, GEO.ID)+'_'+CONVERT(NVARCHAR,GE.ID)+'_GroupEvent>'+ " +
                "CASE WHEN LEN(GE.Name) " +
                "> 20 THEN SUBSTRING(GE.Name, 0, 20) + '...' ELSE GE.Name END +'</a>' AS HEADER " +
                " FROM GroupEvent_Members GEM, GroupEvents GE, GroupEvent_Occurance GEO, User_Friends UF, Users U " +
                "WHERE GE.EventType=1 AND GEO.GroupEventID=GE.ID AND GEM.GroupEventID=GE.ID AND GE.ID=GEM.GroupEventID " +
                " AND UF.UserID=" + Session["User"].ToString() + " AND U.User_ID=UF.FriendID AND " +
                "UF.FriendID=GEM.UserID AND GEM.DateAdded > CONVERT(DATETIME, '" +
                DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A",
                ":")).AddDays(double.Parse("-10")).Date.ToString() + "')  ORDER BY THE_DATE DESC");

            dvEvents = MergeDVThreeCol(dvEvents, dvGroupEvents2);

            //Added Favorite Venues
            DataView dvVenues = dat.GetDataDV("SELECT U.UserName, UV.DateAdded AS THE_DATE, 'Added a favorite venue <a class=AddLink target=_blank href='+dbo.MAKENICENAME(V.Name)+'_'+CONVERT(NVARCHAR,V.ID)+'_Venue>'+ CASE WHEN LEN(V.Name) > 20 THEN SUBSTRING(V.Name, 0, 20) + '...' ELSE V.Name END + +'</a>' AS HEADER " +
                "FROM User_Friends UF, Users U, UserVenues UV, Venues V WHERE UF.UserID=" +
                Session["User"].ToString() + " AND V.ID=UV.VenueID AND U.User_ID=UF.FriendID AND UF.FriendID=UV.UserID AND UV.DateAdded > CONVERT(DATETIME, '" +
                DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(double.Parse("-10")).Date.ToString() + "')  ORDER BY THE_DATE DESC");

            //Added Event Comments
            DataView dvComments = dat.GetDataDV("SELECT U.UserName, U.ProfilePicture,  C.BlogDate AS THE_DATE, 'Posted a comment:  " +
                "<a class=AddGreenLink target=_blank href=' + dbo.MAKENICENAME(E.Header) + '_' + CONVERT(NVARCHAR,E.ID)+'_Event>'+CASE WHEN LEN(C.Comment) <= 27 THEN C.Comment ELSE SUBSTRING(C.Comment, 0, 27) + '...' END +'</a>' AS HEADER " +
                "FROM User_Friends UF, Users U, Comments C, Events E WHERE UF.UserID=" +
                Session["User"].ToString() + " AND U.User_ID=UF.FriendID AND C.BlogID=E.ID "+
                "AND UF.FriendID=C.UserID AND C.BlogDate > CONVERT(DATETIME, '" +
                DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A",
                ":")).AddDays(double.Parse("-10")).Date.ToString() + "')  ORDER BY THE_DATE DESC");

            //Added Venue Comments
            DataView dvCommentsVenue = dat.GetDataDV("SELECT U.UserName, U.ProfilePicture,  VC.CommentDate AS THE_DATE, 'Posted a comment: " +
                "<a class=AddGreenLink target=_blank href='+dbo.MAKENICENAME(V.Name)+'_'+CONVERT(NVARCHAR,V.ID)+'_Venue>'+CASE WHEN LEN(VC.Comment) <= 27 THEN VC.Comment ELSE SUBSTRING(VC.Comment, 0, 27) + '...' END +'</a>' AS HEADER " +
                "FROM User_Friends UF, Users U, Venue_Comments VC, Venues V WHERE UF.UserID=" +
                Session["User"].ToString() + " AND U.User_ID=UF.FriendID AND V.ID=VC.VenueID " +
                "AND UF.FriendID=VC.UserID AND VC.CommentDate > CONVERT(DATETIME, '" +
                DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A",
                ":")).AddDays(double.Parse("-10")).Date.ToString() + "')  ORDER BY THE_DATE DESC");

            //Added Group Thread Comments
            DataView dvGroupThreadComments = dat.GetDataDV("SELECT U.UserName, U.ProfilePicture, GTC.PostedDate AS THE_DATE, " +
                "'Posted a comment on group <a target=_blank class=AddGreenLink href='+ " +
                "dbo.MAKENICENAME(G.Header)+'_'+CONVERT(NVARCHAR,G.ID)+'_Group>'+ " +
                "CASE WHEN LEN(G.Header) " +
                "> 20 THEN SUBSTRING(G.Header, 0, 20) + '...' ELSE G.Header END +'</a>' AS HEADER " +
                " FROM  GroupThreads_Comments GTC, Groups G, GroupThreads GT, User_Friends UF, Users U " +
                "WHERE G.isPrivate='False' AND GT.ID=GTC.ThreadID AND GT.GroupID=G.ID AND UF.UserID=" + Session["User"].ToString() + " AND U.User_ID=UF.FriendID AND " +
                "UF.FriendID=GTC.UserID AND GTC.PostedDate > CONVERT(DATETIME, '" +
                DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A",
                ":")).AddDays(double.Parse("-10")).Date.ToString() + "')  ORDER BY THE_DATE DESC");

            dvComments = MergeDVThreeCol(dvComments, dvGroupThreadComments);

            //Posted Events
            DataView dvPostedEvents = dat.GetDataDV("SELECT U.UserName, U.ProfilePicture, E.PostedOn AS THE_DATE, "+
                "'Posted the event <a class=AddOrangeLink target=_blank href=' + dbo.MAKENICENAME(E.Header) + '_' + CONVERT(NVARCHAR,E.ID)+'_Event>'+ CASE WHEN LEN(E.Header) > 20 THEN SUBSTRING(E.Header, 0, 20) + '...' ELSE E.Header END +'</a>'AS HEADER " +
                "FROM User_Friends UF, Users U, Events E WHERE UF.UserID=" +
                Session["User"].ToString() + " AND U.User_ID=UF.FriendID AND U.UserName=E.UserName "+
            "AND E.PostedOn > CONVERT(DATETIME, '" +
                DateTime.Parse(cookie.Value.ToString().Replace("%20",
                " ").Replace("%3A", ":")).AddDays(double.Parse("-10")).Date.ToString() + "')  ORDER BY THE_DATE DESC");

            //Posted Venues
            DataView dvPostedVenues = dat.GetDataDV("SELECT U.UserName,  U.ProfilePicture, V.PostedOn AS THE_DATE, "+
                "'Posted the venue <a class=AddOrangeLink target=_blank href='+ dbo.MAKENICENAME(V.Name)+'_'+CONVERT(NVARCHAR,V.ID)+'_Venue>'+ CASE WHEN LEN(V.Name) > 20 THEN SUBSTRING(V.Name, 0, 20) + '...' ELSE V.Name END +'</a>' AS HEADER " +
                "FROM User_Friends UF, Users U, Venues V WHERE UF.UserID=" +
                Session["User"].ToString() + " AND U.User_ID=UF.FriendID AND UF.FriendID=V.CreatedByUser AND V.PostedOn > CONVERT(DATETIME, '" +
                DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(double.Parse("-10")).Date.ToString() + "')  ORDER BY THE_DATE DESC");

            //Posted Ads
            DataView dvPostedAds = dat.GetDataDV("SELECT U.UserName,  U.ProfilePicture, CONVERT(DATETIME,A.DateAdded) AS THE_DATE, "+
                "'Posted the ad <a target=_blank class=AddOrangeLink href='+ dbo.MAKENICENAME(A.Header)+'_'+CONVERT(NVARCHAR,A.Ad_ID)+'_Ad>'+ CASE WHEN LEN(A.Header) > 20 THEN SUBSTRING(A.Header, 0, 20) + '...' ELSE A.Header END +'</a>' AS HEADER " +
                "FROM User_Friends UF, Users U, Ads A WHERE UF.UserID=" +
                Session["User"].ToString() + " AND U.User_ID=UF.FriendID AND UF.FriendID=A.User_ID AND A.DateAdded > CONVERT(DATETIME, '" +
                DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(double.Parse("-10")).Date.ToString() + "')  ORDER BY THE_DATE DESC");

            //Posted Group Events
            DataView dvGroupEventsPosts = dat.GetDataDV("SELECT U.UserName, U.ProfilePicture, GE.LastEdit AS THE_DATE, " +
                "'Posted/Edited the group event <a target=_blank class=AddOrangeLink href='+ " +
                "dbo.MAKENICENAME(GE.Name)+'_'+CONVERT(NVARCHAR,GEO.ID)+'_'+CONVERT(NVARCHAR,GE.ID)+'_GroupEvent>'+ " +
                "CASE WHEN LEN(GE.Name) " +
                "> 20 THEN SUBSTRING(GE.Name, 0, 20) + '...' ELSE GE.Name END +'</a>' AS HEADER " +
                ",G.ID AS GID, GE.ID AS TheID, " +
                " 'GEP' AS Type, GEO.ID AS GEOID " +
                " FROM GroupEvent_Occurance GEO, GroupEvents GE, Groups G, User_Friends UF, Users U " +
                "WHERE GEO.GroupEventID=GE.ID AND GE.LastEdit IS NOT NULL AND G.ID=GE.GroupID AND GE.EventType=1 " +
                " AND UF.UserID=" + Session["User"].ToString() + " AND U.User_ID=UF.FriendID AND " +
                "UF.FriendID=GE.LastEditBy AND GE.LastEdit > CONVERT(DATETIME, '" +
                DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A",
                ":")).AddDays(double.Parse("-10")).Date.ToString() + "')  ORDER BY THE_DATE DESC");

            //Posted Groups
            DataView dvGroupPosts = dat.GetDataDV("SELECT U.UserName, U.ProfilePicture, G.LastEditOn AS THE_DATE, " +
                "'Posted/Edited the group <a target=_blank class=AddOrangeLink href='+ " +
                "dbo.MAKENICENAME(G.Header)+'_'+CONVERT(NVARCHAR,G.ID)+'_Group>'+ " +
                "CASE WHEN LEN(G.Header) " +
                "> 20 THEN SUBSTRING(G.Header, 0, 20) + '...' ELSE G.Header END +'</a>' AS HEADER "+
                " FROM Groups G, User_Friends UF, Users U " +
                "WHERE G.LastEditOn IS NOT NULL " +
                " AND G.isPrivate='False' AND UF.UserID=" + Session["User"].ToString() + " AND U.User_ID=UF.FriendID AND " +
                "UF.FriendID=G.LastEditBy AND G.LastEditOn > CONVERT(DATETIME, '" +
                DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A",
                ":")).AddDays(double.Parse("-10")).Date.ToString() + "')  ORDER BY THE_DATE DESC");

            DataView dv = MergeDVThreeCol(MergeDVThreeCol(MergeDVThreeCol(MergeDVThreeCol(dvPostedEvents,
                dvPostedVenues), dvPostedAds), dvGroupEventsPosts), dvGroupPosts);

            string friendImg = "";
            string strFill = "";

            Telerik.Web.UI.RadRotatorItem rItem = new Telerik.Web.UI.RadRotatorItem();
            Literal hiddenPostingLiteral = new Literal();

            if (dv.Count != 0)
            {
                PostingLiteral.Text = "<ul style=\"margin-bottom: 0px;font-family: Arial; font-size: 12px; color: #ff6b09;padding-left:0; \">";
                int count = 0;

                dv.Sort = "THE_DATE DESC";

                for (int i = 0; i < dv.Count;i++ )
                {
                    string rowHeader = dv[i]["HEADER"].ToString();

                    if (System.IO.File.Exists(Server.MapPath(".") + "\\UserFiles\\" + dv[i]["UserName"].ToString() +
                        "\\Profile\\" + dv[i]["ProfilePicture"].ToString()))
                    {
                        friendImg = "UserFiles/" + dv[i]["UserName"].ToString() + "/Profile/" + dv[i]["ProfilePicture"].ToString();
                        strFill = "";
                    }
                    else
                    {
                        friendImg = "image/noAvatar_50x50_small.png";
                        strFill = "onmouseover=\"this.src='NewImages/noAvatar_50x50_smallhover.png'\"" +
                            "onmouseout=\"this.src='image/noAvatar_50x50_small.png'\" ";
                    }
                    if (count < 3)
                    {
                        PostingLiteral.Text += "<li style=\"background-color: #393939;\">";
                        PostingLiteral.Text += "<div style=\"line-height: 12px;color: #cccccc; min-height: 54px;\">" +
                            "<a href=\"" + dv[i]["UserName"].ToString() + "_Friend\"><img " + strFill + " style=\" border: 0;float: left;padding-right: 7px; padding-bottom: 2px;\" " +
                            "src=\"" + friendImg + "\" width=\"50px\" height=\"50px\" /></a>";
                        PostingLiteral.Text += "<span style=\"color: #ff6b09; font-weight: bold;\"><a class=\"AddOrangeLink\" href=\"" + dv[i]["UserName"].ToString() + "_Friend\">" +
                            dv[i]["UserName"].ToString() + "</a></span> " + rowHeader + " on " + DateTime.Parse(dv[i]["THE_DATE"].ToString()).ToShortDateString() + "</div>";
                        PostingLiteral.Text += "</li>";
                        if (count == dv.Count - 1 || count == 2)
                            PostingLiteral.Text += "</ul>";
                    }
                    else if (count == 3)
                    {
                        PostingLiteral.Text += "<div style=\"cursor: pointer;margin-top: -10px;float: right;font-family: Arial; font-size: 12px; color: #ff6b09;padding-left:0; font-weight: bold;\" onclick=\"OpenPostingDiv();\">more..</div>";
                    }

                    hiddenPostingLiteral.Text += "<div style=\"height: 53px;line-height: 12px;font-family: Arial; font-size: 12px; color: #cccccc;padding-left:0;float: left; padding-right: 5px;width: 200px;\">";

                    hiddenPostingLiteral.Text += "<div style=\"color: #cccccc; padding-left: 5px; padding-top: 2px;\">" +
                        "<a href=\"" + dv[i]["UserName"].ToString() + "_Friend\"><img " + strFill + " style=\" border: 0;float: left;padding-right: 7px;\" " +
                        "src=\"" + friendImg + "\" width=\"50px\" height=\"50px\" /></a>";
                    hiddenPostingLiteral.Text += "<span style=\"color: #ff6b09; font-weight: bold;\"><a class=\"AddOrangeLink\" href=\"" + dv[i]["UserName"].ToString() + "_Friend\">" +
                        dv[i]["UserName"].ToString() + "</a></span> " + rowHeader + " on " + DateTime.Parse(dv[i]["THE_DATE"].ToString()).ToShortDateString() + "</div></div>";
                    count++;

                    if (count % 9 == 0)
                    {
                        rItem.Height = 159;
                        rItem.Controls.Add(hiddenPostingLiteral);
                        PostingRotator.Items.Add(rItem);
                        rItem = new Telerik.Web.UI.RadRotatorItem();
                        hiddenPostingLiteral = new Literal();
                    }
                }
                if (dv.Count % 9 != 0)
                {
                    rItem.Height = 159;
                    rItem.Controls.Add(hiddenPostingLiteral);
                    PostingRotator.Items.Add(rItem);
                }
                
                //PostingLiteral.Text = "<div onmouseover=\"OpenPostingDiv()\">" + PostingLiteral.Text + "</div>";

                
            }
            else
            {
                PostingLiteral.Text = "<div  style=\"padding-top: 10px;font-family: Arial; font-size: 12px; color: #ff6b09;padding-left:0; \">Your friends haven't had any Posting Action in 10 days.</div>";
            }

            if (dvEvents.Count != 0)
            {
                rItem = new Telerik.Web.UI.RadRotatorItem();
                hiddenPostingLiteral = new Literal();

                EventGoingLiteral.Text = "<ul style=\"margin-bottom: 0px;font-family: Arial; font-size: 12px; color: #1fb6e7;padding-left:0; \">";
                int count = 0;

                dvEvents.Sort = "THE_DATE DESC";

                foreach (DataRowView row in dvEvents)
                {
                    string rowHeader = row["HEADER"].ToString();

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
                    if (count < 3)
                    {
                        EventGoingLiteral.Text += "<li style=\"background-color: #393939;\">";
                        EventGoingLiteral.Text += "<div style=\"line-height: 12px;color: #cccccc; min-height: 54px;\">" +
                            "<a href=\"" + row["UserName"].ToString() + "_Friend\"><img " + strFill + " style=\" border: 0;float: left;padding-right: 7px; padding-bottom: 2px;\" " +
                            "src=\"" + friendImg + "\" width=\"50px\" height=\"50px\" /></a>";
                        EventGoingLiteral.Text += "<span style=\"color: #1fb6e7; font-weight: bold;\"><a class=\"AddLink\" href=\"" + row["UserName"].ToString() + "_Friend\">" +
                            row["UserName"].ToString() + "</a></span> " + rowHeader + " on " + DateTime.Parse(row["THE_DATE"].ToString()).ToShortDateString() + "</div>";
                        EventGoingLiteral.Text += "</li>";

                        if (count == dvEvents.Count - 1 || count == 2)
                            EventGoingLiteral.Text += "</ul>";
                    }
                    else if (count == 3)
                    {
                        EventGoingLiteral.Text += "<div style=\"cursor: pointer;margin-top: -10px;float: right;font-family: Arial; font-size: 12px; color: #1fb6e7;padding-left:0; font-weight: bold;\" onclick=\"OpenEventDiv();\">more..</div>";
                    }

                    hiddenPostingLiteral.Text += "<div style=\"height: 53px;line-height: 12px;font-family: Arial; font-size: 12px; color: #cccccc;padding-left:0;float: left; padding-right: 5px;width: 200px;\">";

                    hiddenPostingLiteral.Text += "<div style=\"color: #cccccc; padding-left: 5px; padding-top: 2px;\">" +
                        "<a href=\"" + row["UserName"].ToString() + "_Friend\"><img " + strFill + " style=\" border: 0;float: left;padding-right: 7px;\" " +
                        "src=\"" + friendImg + "\" width=\"50px\" height=\"50px\" /></a>";
                    hiddenPostingLiteral.Text += "<span style=\"color: #1fb6e7; font-weight: bold;\"><a class=\"AddLink\" href=\"" + row["UserName"].ToString() + "_Friend\">" +
                        row["UserName"].ToString() + "</a></span> " + rowHeader + " on " + DateTime.Parse(row["THE_DATE"].ToString()).ToShortDateString() + "</div></div>";
                    count++;

                    if (count % 9 == 0)
                    {
                        rItem.Height = 159;
                        rItem.Controls.Add(hiddenPostingLiteral);
                        EventRotator.Items.Add(rItem);
                        rItem = new Telerik.Web.UI.RadRotatorItem();
                        hiddenPostingLiteral = new Literal();
                    }
                }
                if (dvEvents.Count % 9 != 0)
                {
                    rItem.Height = 159;
                    rItem.Controls.Add(hiddenPostingLiteral);
                    EventRotator.Items.Add(rItem);
                }
                EventGoingLiteral.Text += "</ul>";
            }
            else
            {
                EventGoingLiteral.Text = "<div  style=\"padding-top: 10px;font-family: Arial; font-size: 12px; color: " +
                    "#1fb6e7;padding-left:0; \">Your friends haven't had any Event Going Action in 10 days.</div>";
            }

            DataView dvCommentsFinal = MergeDVThreeCol(dvComments, dvCommentsVenue);

            if (dvCommentsFinal.Count != 0)
            {
                rItem = new Telerik.Web.UI.RadRotatorItem();
                hiddenPostingLiteral = new Literal();

                CommentingLiteral.Text = "<ul style=\"margin-bottom: 0px;font-family: Arial; font-size: 12px; color: #568301;padding-left:0; \">";
                int count = 0;
                dvCommentsFinal.Sort = "THE_DATE DESC";
                foreach (DataRowView row in dvCommentsFinal)
                {
                    string rowHeader = row["HEADER"].ToString();

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
                    if (count < 3)
                    {
                        CommentingLiteral.Text += "<li style=\"background-color: #393939;\">";
                        CommentingLiteral.Text += "<div style=\"line-height: 12px;color: #cccccc; min-height: 54px;\">" +
                            "<a href=\"" + row["UserName"].ToString() + "_Friend\"><img " + strFill + " style=\" border: 0;float: left;padding-right: 7px; padding-bottom: 2px;\" " +
                            "src=\"" + friendImg + "\" width=\"50px\" height=\"50px\" /></a>";
                        CommentingLiteral.Text += "<span style=\"color: #628e02; font-weight: bold;\"></span> " + rowHeader + " on " + DateTime.Parse(row["THE_DATE"].ToString()).ToShortDateString() + "</div>";
                        CommentingLiteral.Text += "</li>";

                        if (count == dvCommentsFinal.Count - 1 || count == 2)
                            CommentingLiteral.Text += "</ul>";
                    }
                    else if (count == 3)
                    {
                        CommentingLiteral.Text += "<div style=\"cursor: pointer;margin-top: -10px;float: right;font-family: Arial; font-size: 12px; color: #628e02;padding-left:0; font-weight: bold;\" onclick=\"OpenCommentDiv();\">more..</div>";
                    }

                    hiddenPostingLiteral.Text += "<div style=\"height: 53px;line-height: 12px;font-family: Arial; font-size: 12px; color: #cccccc;padding-left:0;float: left; padding-right: 5px;width: 200px;\">";

                    hiddenPostingLiteral.Text += "<div style=\"color: #cccccc; padding-left: 5px; padding-top: 2px;\">" +
                        "<a href=\"" + row["UserName"].ToString() + "_Friend\"><img " + strFill + " style=\" border: 0;float: left;padding-right: 7px;\" " +
                        "src=\"" + friendImg + "\" width=\"50px\" height=\"50px\" /></a>";
                    hiddenPostingLiteral.Text += "<span style=\"color: #628e02; font-weight: bold;\"><a class=\"AddGreenLink\" href=\"" + row["UserName"].ToString() + "_Friend\">" +
                        row["UserName"].ToString() + "</a></span> " + rowHeader + " on " + DateTime.Parse(row["THE_DATE"].ToString()).ToShortDateString() + "</div></div>";
                    count++;

                    if (count % 9 == 0)
                    {
                        rItem.Height = 159;
                        rItem.Controls.Add(hiddenPostingLiteral);
                        CommentRotator.Items.Add(rItem);
                        rItem = new Telerik.Web.UI.RadRotatorItem();
                        hiddenPostingLiteral = new Literal();
                    }
                }
                if (dvCommentsFinal.Count % 9 != 0)
                {
                    rItem.Height = 159;
                    rItem.Controls.Add(hiddenPostingLiteral);
                    CommentRotator.Items.Add(rItem);
                }


                CommentingLiteral.Text += "</ul>";
            }
            else
            {
                CommentingLiteral.Text = "<div  style=\"padding-top: 10px;font-family: Arial; font-size: 12px; color: " +
                    "#A5C13A;padding-left:0; \">Your friends haven't had any Commenting Action in 10 days.</div>";
            }

        //    DataView dvFinal = MergeDVThreeCol(MergeDVThreeCol(MergeDVThreeCol(MergeDVThreeCol(MergeDVThreeCol(MergeDVThreeCol(dvEvents,
        //        dvVenues), dvComments), dvPostedEvents), dvPostedVenues), dvPostedAds), dvCommentsVenue);

        //    dvFinal.Sort = "THE_DATE DESC";
        //    Label lab;
        //    if (dvFinal.Count != 0)
        //    {
        //        Literal lit = new Literal();
        //        lit.Text = "<ul style=\"color: #1fb6e7;margin: 0;margin-bottom:5px;margin-top:5px;padding-left:20px;\" color=#1fb6e7>";
        //        WhatMyFriendsDidPanel.Controls.Add(lit);
        //        for (int i = 0; i < dvFinal.Count; i++)
        //        {
        //            lab = new Label();
        //            lab.Text = "<li color=#1fb6e7><span class=\"AspLabel\"><span class=\"AddWhiteLink\">" + dvFinal[i]["UserName"].ToString() + "</span> " + dvFinal[i]["HEADER"].ToString() +
        //                " on " + dvFinal[i]["THE_DATE"].ToString() + "</span></li>";
        //            WhatMyFriendsDidPanel.Controls.Add(lab);
        //        }
        //        lit = new Literal();
        //        lit.Text = "</ul>";
        //        WhatMyFriendsDidPanel.Controls.Add(lit);
        //    }
        //    else
        //    {
        //        WhatMyFriendsDidPanel.Visible = false;
        //    }
        }
        else
        {
            PostingLiteral.Text = "<div  style=\"padding-top: 10px;font-family: Arial; font-size: 12px; color: #ff6b09;padding-left:0; \">Your friends haven't had any Posting Action in 10 days.</div>";
            CommentingLiteral.Text = "<div  style=\"padding-top: 10px;font-family: Arial; font-size: 12px; color: " +
    "#568301;padding-left:0; \">Your friends haven't had any Commenting Action in 10 days.</div>";
            EventGoingLiteral.Text = "<div  style=\"padding-top: 10px;font-family: Arial; font-size: 12px; color: " +
                "#1fb6e7;padding-left:0; \">Your friends haven't had any Event Going Action in 10 days.</div>";
            WhatMyFriendsDidPanel.Visible = false;
            MyFriendsPanel.Visible = false;
        }

        DataView dvUser = dat.GetDataDV("SELECT * FROM UserPreferences WHERE UserID=" + Session["User"].ToString());
        string emailPrefs = dvUser[0]["EmailPrefs"].ToString();
        dvUser = dat.GetDataDV("SELECT * FROM UserFriendPrefs WHERE UserID=" + Session["User"].ToString());
        string emailFriendPrefs = "";
        int friendcount = 0; 
        if (dsFriends.Tables.Count > 0)
            if (dsFriends.Tables[0].Rows.Count > 0)
            {
                friendcount = dsFriends.Tables[0].Rows.Count;
                for (int i = 0; i < dsFriends.Tables[0].Rows.Count; i++)
                {
                    dvUser.RowFilter = "FriendID = " + dsFriends.Tables[0].Rows[i]["FriendID"].ToString();
                    if (dvUser.Count > 0)
                        emailFriendPrefs = dvUser[0]["Preferences"].ToString();
                    else
                        emailFriendPrefs = "";

                    Literal lit = new Literal();
                    lit.Text = "<div style=\"float: left; padding: 8px;\"><table align=\"center\" valign=\"middle\" cellpadding=\"0\" cellspacing=\"0\"  bgcolor=\"#666666\" width=\"52\" height=\"52\"><tr><td align=\"center\">";
                    ImageButton profilePicture = new ImageButton();
                    
                    profilePicture.AlternateText = dsFriends.Tables[0].Rows[i]["UserName"].ToString();
                    profilePicture.ToolTip = dsFriends.Tables[0].Rows[i]["UserName"].ToString();
                    profilePicture.Height = 50;
                    profilePicture.Width = 50;
                    profilePicture.ID = "pic" + i.ToString();
                    profilePicture.AlternateText = dsFriends.Tables[0].Rows[i]["UserName"].ToString();
                    profilePicture.CommandArgument = dsFriends.Tables[0].Rows[i]["FriendID"].ToString();
                    if (System.IO.File.Exists(Server.MapPath(".") + "\\UserFiles\\" + dsFriends.Tables[0].Rows[i]["UserName"].ToString() + "\\Profile\\" + dsFriends.Tables[0].Rows[i]["ProfilePicture"].ToString()))
                    {
                        profilePicture.ImageUrl = "~/UserFiles/" + dsFriends.Tables[0].Rows[i]["UserName"].ToString() + "/Profile/" + dsFriends.Tables[0].Rows[i]["ProfilePicture"].ToString();
                        System.Drawing.Image theimg = System.Drawing.Image.FromFile(Server.MapPath(".") + "/UserFiles/" + dsFriends.Tables[0].Rows[i]["UserName"].ToString() +
                     "/Profile/" + dsFriends.Tables[0].Rows[i]["ProfilePicture"].ToString());

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

                        profilePicture.Width = int.Parse((Math.Round(decimal.Parse(width.ToString()))).ToString());
                        profilePicture.Height = int.Parse((Math.Round(decimal.Parse(height.ToString()))).ToString());
                    
                    }
                    else
                    {
                        profilePicture.ImageUrl = "~/image/noAvatar_50x50_small.png";
                        profilePicture.Attributes.Add("onmouseover", "this.src='NewImages/noAvatar_50x50_smallhover.png'");
                        profilePicture.Attributes.Add("onmouseout", "this.src='image/noAvatar_50x50_small.png'");
                    }

                    profilePicture.Click += new ImageClickEventHandler(ViewFriend);

                    MyFriendsPanel.Controls.Add(lit);
                    MyFriendsPanel.Controls.Add(profilePicture);
                    lit = new Literal();
                    lit.Text = "</td></tr></table><div align=\"center\">";
                    MyFriendsPanel.Controls.Add(lit);

                    HyperLink link = new HyperLink();
                    link.Text = "edit prefs";
                    link.CssClass = "PrefsLink";
                    link.ID = "editPrefs" + dsFriends.Tables[0].Rows[i]["UserName"].ToString();

                    Telerik.Web.UI.RadToolTip tip = new Telerik.Web.UI.RadToolTip();
                    tip.TargetControlID = "editPrefs" + dsFriends.Tables[0].Rows[i]["UserName"].ToString();
                    tip.ShowEvent = Telerik.Web.UI.ToolTipShowEvent.OnClick;
                    tip.Position = Telerik.Web.UI.ToolTipPosition.MiddleRight;
                    tip.RelativeTo = Telerik.Web.UI.ToolTipRelativeDisplay.Element;
                    tip.ManualClose = true;
                    tip.Skin = "Vista";

                    UpdatePanel upP = new UpdatePanel();
                    upP.UpdateMode = UpdatePanelUpdateMode.Conditional;

                    Literal tipLit = new Literal();
                    tipLit.Text = "<div align=\"center\" style=\"width: 206px !important; height: 254px !important;\">";
                    tipLit.Text += "<table width=\"100%\" cellspacing=\"0\" align=\"center\" style=\"font-family: Arial; font-size: 11px; color: #1fb6e7;\">"+
                        "<tr><td align=\"center\" style=\"padding-bottom: 17px;padding-top: 20px;\"><span style=\"color: #cccccc; " +
                        "font-size: 12px; font-weight: bold;\">Update Friend's Prefs</span></td></tr>";
                    tipLit.Text += "<tr><td align=\"center\">adds an event to calendar</td></tr>";
                    tipLit.Text += "<tr><td align=\"center\">";

                    upP.ContentTemplateContainer.Controls.Add(tipLit);
                    ImageButton imgB;
                    if (emailPrefs.Contains("2"))
                    {
                        tipLit = new Literal();
                        tipLit.Text = "<span style=\"color: #ff6b09;\">This preference is on for everyone.  If you want to set it just for this user, turn it off for everyone in 'Email Prefs'.</span></td></tr>";

                        upP.ContentTemplateContainer.Controls.Add(tipLit);
                    }
                    else
                    {
                        tipLit = new Literal();
                        tipLit.Text = "<div style=\"width: 118px;\" class=\"topDiv\"><div style=\"float: left;padding-right: 8px;\">";

                        upP.ContentTemplateContainer.Controls.Add(tipLit);

                        imgB = new ImageButton();
                        imgB.ID = "email" + dsFriends.Tables[0].Rows[i]["FriendID"].ToString();
                        if(emailFriendPrefs.Contains("0"))
                            imgB.ImageUrl = "image/CheckSelected.png";
                        else
                            imgB.ImageUrl = "image/Check.png";
                        imgB.CommandArgument = "0";
                        imgB.Click += new ImageClickEventHandler(ChangeCheckImage);

                        upP.ContentTemplateContainer.Controls.Add(imgB);

                        tipLit = new Literal();
                        tipLit.Text = "</div><div style=\"color: #cccccc; float: left;padding-right: 28px;\">email</div><div style=\"padding-right: 8px;float: left;\">";

                        upP.ContentTemplateContainer.Controls.Add(tipLit);

                        imgB = new ImageButton();
                        imgB.ID = "text" + dsFriends.Tables[0].Rows[i]["FriendID"].ToString();
                        if (emailFriendPrefs.Contains("1"))
                            imgB.ImageUrl = "image/CheckSelected.png";
                        else
                            imgB.ImageUrl = "image/Check.png";
                        imgB.CommandArgument = "1";
                        imgB.Click += new ImageClickEventHandler(ChangeCheckImage);

                        upP.ContentTemplateContainer.Controls.Add(imgB);

                        tipLit = new Literal();
                        tipLit.Text = "</div><div style=\"color: #cccccc; float: left\">text</div></div></td></tr>";

                        upP.ContentTemplateContainer.Controls.Add(tipLit);
                    }

                    //posts an event
                    tipLit = new Literal();
                    tipLit.Text = "<tr><td align=\"center\">posts an event</td></tr>";
                    tipLit.Text += "<tr><td align=\"center\"><div style=\"width: 118px;\" class=\"topDiv\"><div style=\"float: left;padding-right: 8px;\">";

                    upP.ContentTemplateContainer.Controls.Add(tipLit);

                    imgB = new ImageButton();
                    imgB.ID = "email2" + dsFriends.Tables[0].Rows[i]["FriendID"].ToString();
                    if (emailFriendPrefs.Contains("2"))
                        imgB.ImageUrl = "image/CheckSelected.png";
                    else
                        imgB.ImageUrl = "image/Check.png";
                    imgB.CommandArgument = "2";
                    imgB.Click += new ImageClickEventHandler(ChangeCheckImage);

                    upP.ContentTemplateContainer.Controls.Add(imgB);

                    tipLit = new Literal();
                    tipLit.Text = "</div><div style=\"color: #cccccc; float: left;padding-right: 28px;\">email</div><div style=\"padding-right: 8px;float: left;\">";

                    upP.ContentTemplateContainer.Controls.Add(tipLit);

                    imgB = new ImageButton();
                    imgB.ID = "text2" + dsFriends.Tables[0].Rows[i]["FriendID"].ToString();
                    if (emailFriendPrefs.Contains("3"))
                        imgB.ImageUrl = "image/CheckSelected.png";
                    else
                        imgB.ImageUrl = "image/Check.png";
                    imgB.CommandArgument = "3";
                    imgB.Click += new ImageClickEventHandler(ChangeCheckImage);

                    upP.ContentTemplateContainer.Controls.Add(imgB);

                    tipLit = new Literal();
                    tipLit.Text = "</div><div style=\"color: #cccccc; float: left\">text</div></td></tr>";

                    upP.ContentTemplateContainer.Controls.Add(tipLit);


                    //posts an ad
                    tipLit = new Literal();
                    tipLit.Text = "<tr><td align=\"center\">posts an ad</td></tr>";
                    tipLit.Text += "<tr><td align=\"center\"><div style=\"width: 118px;\" class=\"topDiv\"><div style=\"float: left;padding-right: 8px;\">";

                    upP.ContentTemplateContainer.Controls.Add(tipLit);

                    imgB = new ImageButton();
                    imgB.ID = "email3" + dsFriends.Tables[0].Rows[i]["FriendID"].ToString();
                    if (emailFriendPrefs.Contains("4"))
                        imgB.ImageUrl = "image/CheckSelected.png";
                    else
                        imgB.ImageUrl = "image/Check.png";
                    imgB.CommandArgument = "4";
                    imgB.Click += new ImageClickEventHandler(ChangeCheckImage);

                    upP.ContentTemplateContainer.Controls.Add(imgB);

                    tipLit = new Literal();
                    tipLit.Text = "</div><div style=\"color: #cccccc; float: left;padding-right: 28px;\">email</div><div style=\"padding-right: 8px;float: left;\">";

                    upP.ContentTemplateContainer.Controls.Add(tipLit);

                    imgB = new ImageButton();
                    imgB.ID = "text3" + dsFriends.Tables[0].Rows[i]["FriendID"].ToString();
                    if (emailFriendPrefs.Contains("5"))
                        imgB.ImageUrl = "image/CheckSelected.png";
                    else
                        imgB.ImageUrl = "image/Check.png";
                    imgB.CommandArgument = "5";
                    imgB.Click += new ImageClickEventHandler(ChangeCheckImage);

                    upP.ContentTemplateContainer.Controls.Add(imgB);

                    tipLit = new Literal();
                    tipLit.Text = "</div><div style=\"color: #cccccc; float: left\">text</div></td></tr>";

                    upP.ContentTemplateContainer.Controls.Add(tipLit);


                    //sends a Hippo Mail to you
                    tipLit = new Literal();
                    tipLit.Text = "<tr><td align=\"center\">sends a Hippo Mail to you</td></tr>";
                    tipLit.Text += "<tr><td align=\"center\">";

                    upP.ContentTemplateContainer.Controls.Add(tipLit);

                    if (emailPrefs.Contains("7"))
                    {
                        tipLit = new Literal();
                        tipLit.Text = "<span style=\"color: #ff6b09;\">This preference is on for everyone.  If you want to set it just for this user, turn it off for everyone in 'Email Prefs'.</span></td></tr>";

                        upP.ContentTemplateContainer.Controls.Add(tipLit);
                    }
                    else
                    {
                        tipLit = new Literal();
                        tipLit.Text = "<div style=\"width: 118px;\" class=\"topDiv\"><div style=\"float: left;padding-right: 8px;\">";
                        upP.ContentTemplateContainer.Controls.Add(tipLit);

                        imgB = new ImageButton();
                        imgB.ID = "email4" + dsFriends.Tables[0].Rows[i]["FriendID"].ToString();
                        if (emailFriendPrefs.Contains("6"))
                            imgB.ImageUrl = "image/CheckSelected.png";
                        else
                            imgB.ImageUrl = "image/Check.png";
                        imgB.CommandArgument = "6";
                        imgB.Click += new ImageClickEventHandler(ChangeCheckImage);

                        upP.ContentTemplateContainer.Controls.Add(imgB);

                        tipLit = new Literal();
                        tipLit.Text = "</div><div style=\"color: #cccccc; float: left;padding-right: 28px;\">email</div><div style=\"padding-right: 8px;float: left;\">";

                        upP.ContentTemplateContainer.Controls.Add(tipLit);

                        imgB = new ImageButton();
                        imgB.ID = "text4" + dsFriends.Tables[0].Rows[i]["FriendID"].ToString();
                        if (emailFriendPrefs.Contains("7"))
                            imgB.ImageUrl = "image/CheckSelected.png";
                        else
                            imgB.ImageUrl = "image/Check.png";
                        imgB.CommandArgument = "7";
                        imgB.Click += new ImageClickEventHandler(ChangeCheckImage);

                        upP.ContentTemplateContainer.Controls.Add(imgB);

                        tipLit = new Literal();
                        tipLit.Text = "</div><div style=\"color: #cccccc; float: left\">text</div></td></tr>";

                        upP.ContentTemplateContainer.Controls.Add(tipLit);
                    }

                    //shares event/venue/ad with you
                    tipLit = new Literal();
                    tipLit.Text = "<tr><td align=\"center\">shares event/venue/ad with you</td></tr>";
                    tipLit.Text += "<tr><td align=\"center\">";

                    upP.ContentTemplateContainer.Controls.Add(tipLit);

                    if (emailPrefs.Contains("9"))
                    {
                        tipLit = new Literal();
                        tipLit.Text = "<span style=\"color: #ff6b09;\">This preference is on for everyone.  If you want to set it just for this user, turn it off for everyone in 'Email Prefs'.</span></td></tr>";

                        upP.ContentTemplateContainer.Controls.Add(tipLit);
                    }
                    else
                    {
                        tipLit = new Literal();
                        tipLit.Text = "<div style=\"width: 118px;\" class=\"topDiv\"><div style=\"float: left;padding-right: 8px;\">";
                        upP.ContentTemplateContainer.Controls.Add(tipLit);

                        imgB = new ImageButton();
                        imgB.ID = "email5" + dsFriends.Tables[0].Rows[i]["FriendID"].ToString();
                        if (emailFriendPrefs.Contains("8"))
                            imgB.ImageUrl = "image/CheckSelected.png";
                        else
                            imgB.ImageUrl = "image/Check.png";
                        imgB.CommandArgument = "8";
                        imgB.Click += new ImageClickEventHandler(ChangeCheckImage);

                        upP.ContentTemplateContainer.Controls.Add(imgB);

                        tipLit = new Literal();
                        tipLit.Text = "</div><div style=\"color: #cccccc; float: left;padding-right: 28px;\">email</div><div style=\"padding-right: 8px;float: left;\">";

                        upP.ContentTemplateContainer.Controls.Add(tipLit);

                        imgB = new ImageButton();
                        imgB.ID = "text5" + dsFriends.Tables[0].Rows[i]["FriendID"].ToString();
                        if (emailFriendPrefs.Contains("9"))
                            imgB.ImageUrl = "image/CheckSelected.png";
                        else
                            imgB.ImageUrl = "image/Check.png";
                        imgB.CommandArgument = "9";
                        imgB.Click += new ImageClickEventHandler(ChangeCheckImage);

                        upP.ContentTemplateContainer.Controls.Add(imgB);

                        tipLit = new Literal();
                        tipLit.Text = "</div><div style=\"color: #cccccc; float: left\">text</div></td></tr>";

                        upP.ContentTemplateContainer.Controls.Add(tipLit);

                    }
                    tipLit = new Literal();
                    tipLit.Text = "</table><div style=\"color: red;float: left;padding-top: 10px;\">";
                    upP.ContentTemplateContainer.Controls.Add(tipLit);

                    Label labl = new Label();
                    labl.ID = "ErrorLabel" + dsFriends.Tables[0].Rows[i]["FriendID"].ToString();

                    upP.ContentTemplateContainer.Controls.Add(labl);

                    tipLit = new Literal();
                    tipLit.Text = "</div><div style=\"float: right;padding-top: 10px;\">";

                    upP.ContentTemplateContainer.Controls.Add(tipLit);

                    Button btn = new Button();
                    btn.Text = "Save";
                    btn.CssClass="SearchButtonSmall";
                    btn.Attributes.Add("onmouseout", "this.style.backgroundImage='url(image/SmallButton.png)'");
                    btn.Attributes.Add("onmouseover","this.style.backgroundImage='url(image/SmallButtonSelected.png)'");
                    btn.Attributes.Add("onclientclick", "this.value = 'Working...';");
                    btn.CommandArgument = dsFriends.Tables[0].Rows[i]["FriendID"].ToString();
                    btn.OnClientClick = "javascript:CloseToolTip();";
                    btn.Click += new EventHandler(PrefsSave);

                    upP.ContentTemplateContainer.Controls.Add(btn);

                    tipLit = new Literal();
                    tipLit.Text = "</div></div>";

                    upP.ContentTemplateContainer.Controls.Add(tipLit);

                    tip.Controls.Add(upP);

                    MyFriendsPanel.Controls.Add(link);
                    MyFriendsPanel.Controls.Add(tip);

                    lit = new Literal();
                    lit.Text = "</div></div>";
                    MyFriendsPanel.Controls.Add(lit); 
                }
            }

        NumFriendsLabel.Text = friendcount.ToString();
        LinkButton friendLink = new LinkButton();
        friendLink.Text = "Add Friends";
    }

    protected void PrefsSave(object sender, EventArgs e)
    {
        
            HttpCookie cookie = Request.Cookies["BrowserDate"];
            string message = "";
            if (cookie == null)
            {
                cookie = new HttpCookie("BrowserDate");
                cookie.Value = DateTime.Now.ToString();
                cookie.Expires = DateTime.Now.AddDays(22);
                Response.Cookies.Add(cookie);
            }
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            string prefs = "";

            Button saveB = (Button)sender;
            string friend = saveB.CommandArgument;

            Label label = (Label)dat.FindControlRecursive(this, "ErrorLabel" + friend);

            try
            {
                ImageButton imbE = (ImageButton)dat.FindControlRecursive(this, "email" + friend);
                ImageButton imbT = (ImageButton)dat.FindControlRecursive(this, "text" + friend);

                if (imbE != null && imbT != null)
                {
                    if (imbE.ImageUrl == "image/CheckSelected.png")
                        prefs += "0";

                    if (imbT.ImageUrl == "image/CheckSelected.png")
                        prefs += "1";
                }

                imbE = (ImageButton)dat.FindControlRecursive(this, "email2" + friend);
                imbT = (ImageButton)dat.FindControlRecursive(this, "text2" + friend);

                if (imbE != null && imbT != null)
                {
                    if (imbE.ImageUrl == "image/CheckSelected.png")
                        prefs += "2";

                    if (imbT.ImageUrl == "image/CheckSelected.png")
                        prefs += "3";
                }

                imbE = (ImageButton)dat.FindControlRecursive(this, "email3" + friend);
                imbT = (ImageButton)dat.FindControlRecursive(this, "text3" + friend);

                if (imbE != null && imbT != null)
                {
                    if (imbE.ImageUrl == "image/CheckSelected.png")
                        prefs += "4";

                    if (imbT.ImageUrl == "image/CheckSelected.png")
                        prefs += "5";
                }

                imbE = (ImageButton)dat.FindControlRecursive(this, "email4" + friend);
                imbT = (ImageButton)dat.FindControlRecursive(this, "text4" + friend);

                if (imbE != null && imbT != null)
                {
                    if (imbE.ImageUrl == "image/CheckSelected.png")
                        prefs += "6";

                    if (imbT.ImageUrl == "image/CheckSelected.png")
                        prefs += "7";
                }

                imbE = (ImageButton)dat.FindControlRecursive(this, "email5" + friend);
                imbT = (ImageButton)dat.FindControlRecursive(this, "text5" + friend);

                if (imbE != null && imbT != null)
                {
                    if (imbE.ImageUrl == "image/CheckSelected.png")
                        prefs += "8";

                    if (imbT.ImageUrl == "image/CheckSelected.png")
                        prefs += "9";
                }

                DataView dvF = dat.GetDataDV("SELECT * FROM UserFriendPrefs WHERE FriendID=" + friend);
               
                if (dvF.Count > 0)
                {
                    dat.Execute("UPDATE UserFriendPrefs SET Preferences = '" + prefs + "' WHERE UserID=" +
                        Session["User"].ToString() + " AND FriendID=" + friend);
                }
                else
                {
                    dat.Execute("INSERT INTO UserFriendPrefs (Preferences, UserID, FriendID) VALUES('" + prefs + "', " +
                        Session["User"].ToString() + ", " + friend + ")");
                }
            }
            catch (Exception ex)
            {
                label.Text += message+"<br/><br/>"+ex.ToString();
            }

    }

    protected void ChangeCheckImage(object sender, EventArgs e)
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
        ImageButton CheckImageButton = (ImageButton)sender;

        if (CheckImageButton.ImageUrl == "image/Check.png")
            CheckImageButton.ImageUrl = "image/CheckSelected.png";
        else
            CheckImageButton.ImageUrl = "image/Check.png";
    }

    //protected void MarkAsRead_OLD(object sender, Telerik.Web.UI.RadPanelBarEventArgs e)
    //{
    //    if (e.Item.Attributes["CommandArgument"] != null)
    //    {
    //        bool read = bool.Parse(e.Item.Attributes["Read"]);

    //        if (!read)
    //        {
    //            Telerik.Web.UI.RadPanelItem rItem = (Telerik.Web.UI.RadPanelItem)e.Item;
    //            int key = int.Parse(e.Item.Attributes["CommandArgument"]);
    //            rItem.Text = rItem.Text.Replace("font-weight:bold;", "");
    //            rItem.Text = rItem.Text.Replace("font-weight: bold;", "");
    //            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

    //            dat.Execute("UPDATE UserMessages SET [Read] = 'True' WHERE ID=" + key);

    //            //LoadControls();
    //        }
    //    }
    //}

    //[Ajax.AjaxMethod]
    //public static string MarkAsRead(string messageID)
    //{
    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));       
    //    dat.Execute("UPDATE UserMessages SET [Read] = 'True' WHERE ID=" + messageID);

    //    return "strindsfdffg";
    //}

    protected void MarkAsRead2(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        HtmlGenericControl theSender = (HtmlGenericControl)sender;
        string messageID = theSender.ID.Replace("header", "");
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        dat.Execute("UPDATE UserMessages SET [Read] = 'True' WHERE ID=" + messageID);

    }

    protected void OpenSearchFriends(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        Panel SearchFriendPanel = (Panel)dat.FindControlRecursive(this, "SearchFriendPanel");
        SearchFriendPanel.Visible = true;
    }
    
    protected void CancelFriendSearch(object sender, EventArgs e)
    {
        CloseSearchPanel();
    }
    
    protected void CloseSearchPanel()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        Panel SearchFriendPanel = (Panel)dat.FindControlRecursive(this, "SearchFriendPanel");
        SearchFriendPanel.Visible = false;
    }
    
    protected void FriendSearch(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        ClearMessage();
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        ASP.controls_hippotextbox_ascx FriendSearchTextBox = (ASP.controls_hippotextbox_ascx)dat.FindControlRecursive(this,"FriendSearchTextBox");
        
        Label FriendMessageLabel = (Label)dat.FindControlRecursive(this, "FriendMessageLabel");


        if (FriendSearchTextBox.THE_TEXT != "" && dat.TrapKey(FriendSearchTextBox.THE_TEXT, 1))
        {
            DataSet ds = dat.GetData("SELECT * FROM Users WHERE UserName LIKE '%" + FriendSearchTextBox.THE_TEXT + "%'");

            if (ds.Tables.Count > 0)
                if (ds.Tables[0].Rows.Count > 0)
                {
                    FillSearchPanel(ds);
                    ViewState["FriendDS"] = ds;
                }
                else
                    FriendMessageLabel.Text = "0 Results Found.";
            else
                FriendMessageLabel.Text = "0 Results Found.";
        }
        else
            FriendMessageLabel.Text = "Include a valid User Name in the text field.";
    }
    
    protected void FillSearchPanel(DataSet ds)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        Panel SearchResultsPanel = (Panel)dat.FindControlRecursive(this, "SearchResultsPanel");
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            Image image = new Image();
            image.Width = 50;
            image.Height = 50;

            if (System.IO.File.Exists(Server.MapPath(".") + "\\UserFiles\\" + ds.Tables[0].Rows[i]["UserName"].ToString() + "\\Profile\\" + ds.Tables[0].Rows[i]["ProfilePicture"].ToString()))
            {
                image.ImageUrl = "~/UserFiles/" + ds.Tables[0].Rows[i]["UserName"].ToString() + "/Profile/" + ds.Tables[0].Rows[i]["ProfilePicture"].ToString();
            }
            else
                image.ImageUrl = "~/image/noAvatar_50x50_small.png";

            Label label = new Label();
            label.Text = ds.Tables[0].Rows[i]["UserName"].ToString();

            LinkButton link = new LinkButton();
            link.Text = "Add Friend";
            link.CssClass = "AddLink";
            link.CausesValidation = false;
            link.ID = "link" + i.ToString();
            link.CommandArgument = ds.Tables[0].Rows[i]["User_ID"].ToString();
            link.Click += new EventHandler(this.AddFriend);


            SearchResultsPanel.Controls.Add(image);
            SearchResultsPanel.Controls.Add(label);
            SearchResultsPanel.Controls.Add(link);
        }
    }
    
    protected void FillVenues()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet ds = dat.GetData("SELECT *, UV.VenueID AS VID  FROM UserVenues UV, Venues V "+
            "WHERE V.ID=UV.VenueID AND UV.UserID="+
            Session["User"].ToString() + " ORDER BY V.Name");

        bool noneExist = false;

        if (ds.Tables.Count > 0)
            if (ds.Tables[0].Rows.Count > 0)
            {
                ASP.controls_pager_ascx pagerPanel = new ASP.controls_pager_ascx();
                pagerPanel.NUMBER_OF_ITEMS_PER_PAGE = 5;
                pagerPanel.PANEL_CSSCLASS = "FavoritesPanel";
                pagerPanel.WIDTH = 260;
                ArrayList a = new ArrayList(ds.Tables[0].Rows.Count * 2);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Literal lit = new Literal();
                    lit.Text = "<div class=\"topDiv\"><div class=\"topDiv\" style=\"padding-top: 20px;\">";

                    Literal lit2 = new Literal();
                    lit2.Text = "</div>";

                    ASP.controls_hipporating_ascx rating = new ASP.controls_hipporating_ascx();
                    rating.VENUE_ID = ds.Tables[0].Rows[i]["ID"].ToString();

                    Label label = new Label();
                    label.Text = "<div style=\"float:left; padding-top: 4px;\"><a class=\"AddLink\" href=\"" + dat.MakeNiceName(ds.Tables[0].Rows[i]["Name"].ToString()) +'_'+ ds.Tables[0].Rows[i]["ID"].ToString() + "_Venue\">" + ds.Tables[0].Rows[i]["Name"].ToString() + "</a></div>";

                    DataSet dsEvents = dat.GetData("SELECT * FROM Events E, Event_Occurance EO WHERE EO.EventID=E.ID AND "+
                        "DAY(EO.DateTimeStart) = DAY(GETDATE()) AND MONTH(EO.DateTimeStart) = MONTH(GETDATE()) AND YEAR"+
                        "(EO.DateTimeStart) = YEAR(GETDATE()) "
                      +   " AND E.Venue=" + ds.Tables[0].Rows[i]["VID"].ToString());
                    int count = 0;

                    if (dsEvents.Tables.Count > 0)
                        if (dsEvents.Tables[0].Rows.Count > 0)
                            count = dsEvents.Tables[0].Rows.Count;

                    Label label2 = new Label();
                    label2.Text = "<div style=\"display: block; padding-top: 3px;\">"+count + " event[s] going on today </div></div>";

                    Panel allP = new Panel();
                    allP.Controls.Add(lit);
                    allP.Controls.Add(label);
                    allP.Controls.Add(rating);
                    allP.Controls.Add(lit2);
                    allP.Controls.Add(label2);
                    
                    a.Add(allP);
                }

                pagerPanel.DATA = a;
                pagerPanel.DataBind2();
                Literal newLit = new Literal();
                newLit.Text = "<div class=\"topDiv\">";
                FavoriteVenues.Controls.Add(newLit);
                FavoriteVenues.Controls.Add(pagerPanel);
                newLit = new Literal();
                newLit.Text = "</div>";
                FavoriteVenues.Controls.Add(newLit);
            }
            else
                noneExist = true;
        else
            noneExist = true;

        if (noneExist)
        {
            Label label = new Label();
            label.Text = "You have not added any venues to your favorite's list. To search for some awesome venues and make them your favorite visit the <a class=\"AddGreenLink\" href=\"VenueSearch.aspx\">venue's page.</a>";
            FavoriteVenues.Controls.Add(label);
        }
    }
    
    protected void FillRecommendedEvents()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        //only show top 10
        int topCount = 10;

        //Put the sets together
        DataSet dsAll = dat.RetrieveRecommendedEvents(topCount, true);

        bool noneMessage = false;

        if (dsAll.Tables.Count > 0)
            if (dsAll.Tables[0].Rows.Count > 0)
            {
                //insert link to all recommended events
                Session["RecomDS"] = dsAll;
                Literal link = new Literal();
                link.Text = "<h4 style=\"margin-bottom: 5px;\">Top 10 Recommended Events</h4><div><a class=\"AddLink\" onclick=\"OpenRadRecom();\">See All</a></div>";

                RecommendedEvents.Controls.Add(link);

                Hashtable hash = new Hashtable();

                ASP.controls_pager_ascx pagerPanel = new ASP.controls_pager_ascx();
                pagerPanel.NUMBER_OF_ITEMS_PER_PAGE = 3;
                pagerPanel.PANEL_CSSCLASS = "FavoritesPanel";
                pagerPanel.WIDTH = 270;
                ArrayList a = new ArrayList(topCount);

                for (int i = 0; i < topCount; i++)
                {
                    dat.InsertOneEvent(dsAll, i, ref a, false);
                }

                pagerPanel.DATA = a;
                pagerPanel.DataBind2();
                RecommendedEvents.Controls.Add(pagerPanel);
            }
            else
            {
                noneMessage = true;
            }
        else
            noneMessage = true;

        if (noneMessage)
        {
            Label lab = new Label();
            lab.Text = "There are no recommended events here this month. There are a few reasons for this. "+
                "There simply is no events that fit your recommendation criteria, or you have selected not to "+
                "recommend any events. To modify your preferences please visit the 'My Preferences' tab on this page.";

            RecommendedEvents.Controls.Add(lab);
        }

    }

    //protected void GetFriendEvents()
    //{
    //    DateTime date = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //    string command = "SELECT DISTINCT U.UserName, UC.EventID, EO.DateTimeStart, E.Header " +
    //        "FROM Users AS U INNER JOIN " +
    //                     "User_Friends AS UF ON U.User_ID = UF.FriendID INNER JOIN " +
    //                     "User_Calendar AS UC ON UF.FriendID = UC.UserID INNER JOIN " +
    //                     "Event_Occurance AS EO ON UC.EventID = EO.EventID INNER JOIN " +
    //                     "Events AS E ON EO.EventID = E.ID " +
    //                     "WHERE (UF.UserID = "+Session["User"].ToString()+") AND MONTH(EO.DateTimeStart) = "+DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Month.ToString();

    //    DataSet dsFriends = dat.GetData(command);
    //    int count = 0;
    //    if (dsFriends.Tables.Count > 0)
    //        if (dsFriends.Tables[0].Rows.Count > 0)
    //            if (dsFriends.Tables[0].Rows.Count > count)
    //            {
    //                count = dsFriends.Tables[0].Rows.Count;
                    
    //            }

    //    if (count == 0)
    //        FriendMessagesPanel.Visible = false;
    //    for (int i = 0; i < count; i++)
    //    {
    //        string title = dsFriends.Tables[0].Rows[i]["Header"].ToString();

    //        if (title.Length > 30)
    //            title = title.Substring(0, 30) + "..";
    //        FriendMessagesLiteral.Text += "<div class=\"EventBody FriendMessage\" style=\"width: 400px;\"> " + dsFriends.Tables[0].Rows[i]["UserName"].ToString() +
    //            " has added <a class=\"AddLink\"  href=\"Event.aspx?EventID=" +
    //            dsFriends.Tables[0].Rows[i]["EventID"].ToString() + "\">" +
    //            dsFriends.Tables[0].Rows[i]["Header"].ToString() + "</a> to their calendar<br/></div>";
    //    }
    //}

    protected void ServerDeleteMessage(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        UserErrorLabel.Text = "hello";
        try
        {
            ImageButton theImg = (ImageButton)sender;
            string messageID = theImg.CommandArgument;

            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            DataSet dsMess = dat.GetData("SELECT * FROM UserMessages WHERE ID="+messageID);
            if (dsMess.Tables[0].Rows[0]["From_UserID"] == Session["User"])
            {
                dat.Execute("UPDATE UserMessages SET SentLive=0 WHERE ID=" + messageID);
            }
            else
            {
                dat.Execute("UPDATE UserMessages SET Live=0 WHERE ID=" + messageID);
            }

            //Telerik.Web.UI.RadPanelBar bar = (Telerik.Web.UI.RadPanelBar)theImg.Parent.Parent.Parent;

            //Telerik.Web.UI.RadPanelItem item = (Telerik.Web.UI.RadPanelItem)theImg.Parent.Parent;
            //item.Visible = false;

            Response.Redirect("User.aspx");

            //LoadControlsNotAJAX();

            //RadPageView3 : UserErrorLabel.Text = theImg.Parent.Parent.Parent.Parent.Parent.Parent.Parent.ID.ToString();
        }
        catch (Exception ex)
        {
            UserErrorLabel.Text = ex.ToString();
        }
        
    }

    protected void ServerMarkRead(object sender, Telerik.Web.UI.RadPanelBarEventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        try
        {
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            DataSet dsRead = dat.GetData("SELECT * FROM UserMessages WHERE ID=" + e.Item.Value);

            if (!bool.Parse(dsRead.Tables[0].Rows[0]["Read"].ToString()))
            {

                dat.Execute("UPDATE UserMessages SET [Read]='True' WHERE ID=" + e.Item.Value);

                e.Item.Text = e.Item.Text.Replace("bold", "normal");
                e.Item.Text = e.Item.Text.Replace("White", "#cccccc");

                int unreadCount = int.Parse(RadTabStrip3.Tabs[0].Value) - 1;

                RadTabStrip3.Tabs[0].Value = unreadCount.ToString();

                if (unreadCount == 0)
                {
                    RadTabStrip3.Tabs[0].Text = "Inbox";
                }
                else
                {

                    RadTabStrip3.Tabs[0].Text = "Inbox <span style=\"font-size: 12px; vertical-align: middle; font-weight: normal; padding-bottom: 3px;\">(" + unreadCount.ToString() + " New)</span>";

                }
            }
        }
        catch (Exception ex)
        {
            UserErrorLabel.Text = ex.ToString();
        }
    }

    protected void ServerReply(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        bool goOn = false;
        if (Session["TimeReplySubmitted"] == null)
            goOn = true;
        else
        {
            if (DateTime.Now < ((DateTime)Session["TimeReplySubmitted"]).AddSeconds(2))
            {
                goOn = false;
            }
            else
            {
                goOn = true;
            }
        }

        if (goOn)
        {
            try
            {
                Session["TimeReplySubmitted"] = DateTime.Now;
                Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

                HtmlButton link = (HtmlButton)sender;
                string[] delim = { "reply" };

                string[] tokens = link.ID.Split(delim, StringSplitOptions.None);

                TextBox textbox = (TextBox)link.Parent.FindControl(tokens[0] + "textbox" + tokens[1]);

                int To_ID = int.Parse(tokens[0]);

                DataSet ds = dat.GetData("SELECT * FROM UserMessages WHERE ID=" + tokens[1]);


                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());

                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO UserMessages (MessageContent, MessageSubject, " +
                    "From_UserID, To_UserID, Date, [Read], Mode)"
                    + " VALUES(@content, @subject, @fromID, @toID, @date, 'False', 0)", conn);
                cmd.Parameters.Add("@content", SqlDbType.Text).Value = textbox.Text;
                cmd.Parameters.Add("@subject", SqlDbType.NVarChar).Value = "Re: " + ds.Tables[0].Rows[0]["MessageSubject"].ToString();
                cmd.Parameters.Add("@toID", SqlDbType.Int).Value = To_ID;
                cmd.Parameters.Add("@fromID", SqlDbType.Int).Value = Session["User"].ToString();
                cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
                cmd.ExecuteNonQuery();

                DataSet dsUser = dat.GetData("SELECT * FROM Users WHERE User_ID=" + Session["User"].ToString());
                DataSet dsTo = dat.GetData("SELECT * FROM Users U, UserPreferences UP WHERE UP.UserID=U.User_ID AND U.User_ID=" + To_ID);

                //only send to email if users preferences are set to do so.
                if (dsTo.Tables[0].Rows[0]["EmailPrefs"].ToString().Contains("4"))
                {
                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"],
                        System.Configuration.ConfigurationManager.AppSettings["emailName"],
                        dsTo.Tables[0].Rows[0]["Email"].ToString(), textbox.Text, "Re: " + ds.Tables[0].Rows[0]["MessageSubject"].ToString());
                }
                conn.Close();


                Literal lit = new Literal();
                lit.Text = "<div align=\"left\" style=\"height: 30px; width: 220px; margin: 5px; float: right;\" class=\"AddGreenLink\">Your reply has been sent!</div>";

                link.Parent.Controls.AddAt(9, lit);
            }
            catch (Exception ex)
            {
                UserErrorLabel.Text = ex.ToString();
            }
        }
        else
        {
            HtmlButton link = (HtmlButton)sender;
            Literal lit = new Literal();
            lit.Text = "<div align=\"left\" style=\"height: 30px; width: 220px; margin: 5px; float: right;\" class=\"AddGreenLink\">Your reply has been sent!</div>";

            link.Parent.Controls.AddAt(9, lit);
        }
    }

    protected void ServerAcceptFriend(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        try
        {
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            HtmlButton link = (HtmlButton)sender;
            string[] delim = { "accept" };

            int To_ID = int.Parse(link.ID.Split(delim, StringSplitOptions.None)[0]);

            DataSet ds = dat.GetData("SELECT * FROM User_Friends WHERE UserID=" + Session["User"].ToString() +
                " AND FriendID=" + To_ID);

            bool hasFriend = false;

            if (ds.Tables.Count > 0)
                if (ds.Tables[0].Rows.Count > 0)
                    hasFriend = true;
                else
                    hasFriend = false;
            else
                hasFriend = false;

            if (!hasFriend)
            {

                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());

                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO UserMessages (MessageContent, MessageSubject, " +
                    "From_UserID, To_UserID, Date, [Read], Mode)"
                    + " VALUES(@content, @subject, "+dat.HIPPOHAPP_USERID.ToString()+", @toID, @date, 'False', 0)", conn);
                cmd.Parameters.Add("@content", SqlDbType.Text).Value = "Congratulations!, <br/><br/> " +
                    "We wanted to let you know that " + Session["UserName"].ToString()
                    + " has accepted your friend request. Good luck in your journey!<br/><br/> Have a " +
                    "Happening Day! <br/><br/> ";
                cmd.Parameters.Add("@subject", SqlDbType.NVarChar).Value = "Friend Request Approved from " + Session["UserName"].ToString();
                cmd.Parameters.Add("@toID", SqlDbType.Int).Value = To_ID;
                cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
                cmd.ExecuteNonQuery();

                DataSet dsUser = dat.GetData("SELECT * FROM Users WHERE User_ID=" + Session["User"].ToString());
                DataSet dsTo = dat.GetData("SELECT * FROM Users U, UserPreferences UP WHERE UP.UserID=U.User_ID AND U.User_ID=" + To_ID);

                //only send to email if users preferences are set to do so.
                if (dsTo.Tables[0].Rows[0]["EmailPrefs"].ToString().Contains("8"))
                {
                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"],
                    System.Configuration.ConfigurationManager.AppSettings["emailName"],
                        dsTo.Tables[0].Rows[0]["Email"].ToString(), "Congratulations!, <br/><br/> " +
                        "We wanted to let you know that " + Session["UserName"].ToString()
                        + " has accepted your friend request. Good luck in your journey!<br/><br/> Have a " +
                        "Happening Day! <br/><br/> ", "Friend Request Approved from " + Session["UserName"].ToString());
                }
                dat.Execute("INSERT INTO User_Friends (UserID, FriendID) VALUES(" + Session["User"].ToString()
                    + ", " + To_ID + ")");
                dat.Execute("INSERT INTO User_Friends (UserID, FriendID) VALUES(" + To_ID
                    + ", " + Session["User"].ToString() + ")");

                conn.Close();

            }

            Literal lit = new Literal();
            lit.Text = "<div style=\"float: right; width: 220px;height: 30px; margin: 5px;\" " +
                "class=\"AddGreenLink\">You have accepted this gal/guy as a friend! Good luck, you two!</div>";

            link.Parent.Controls.Add(lit);

            link.Parent.Controls.Remove(link);
        }
        catch (Exception ex)
        {
            UserErrorLabel.Text = ex.ToString();
        }
    }

    protected void ServerAcceptChange(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        try
        {
            HtmlButton thebutton = (HtmlButton)sender;

            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            string theRow = "";
            bool isVenue = false;

            if (thebutton.Attributes["commArg"].ToString().ToLower() == "venue")
                isVenue = true;

            if (isVenue)
            {
                if (thebutton.Attributes["commandargument"].Contains("category"))
                {


                }
                else
                {

                    string[] delim = { "accept" };
                    string[] tokens = thebutton.Attributes["commandargument"].Split(delim, StringSplitOptions.None);

                    string rowID = tokens[0];
                    string columnNumber = tokens[1];
                    theRow = rowID;
                    DataSet dsEventID = dat.GetData("SELECT * FROM VenueRevisions WHERE ID=" + rowID);

                    string eventID = dsEventID.Tables[0].Rows[0]["VenueID"].ToString();

                    string columnName = dsEventID.Tables[0].Columns[int.Parse(columnNumber)].ColumnName;

                    string temp = dsEventID.Tables[0].Rows[0][columnName].ToString();

                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE Venues SET " + columnName + "=@p1 WHERE ID=@eventID", conn);
                    cmd.Parameters.Add("@p1", SqlDbType.NVarChar).Value = dsEventID.Tables[0].Rows[0][columnName].ToString();
                    cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = eventID;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    dat.ApproveRejectChange("VenueRevisions", rowID, int.Parse(columnNumber)-3, true);


                    string venueName = dat.GetData("SELECT * FROM Venues V, VenueRevisions VCR WHERE V.ID=VCR.VenueID AND VCR.ID=" + rowID).Tables[0].Rows[0]["Name"].ToString();


                    //string categoryName = dat.GetData("SELECT * FROM VenueRevisions VCR, VenueCategories VC " +
                    //    "WHERE VC.CategoryID=VCR.CategoryID AND VCR.ID=" + rowID).Tables[0].Rows[0]["CategoryName"].ToString();

                    DataSet dsRevision = dat.GetData("SELECT * FROM VenueRevisions VCR, Users U WHERE U.User_ID=VCR.modifierID AND VCR.ID=" + rowID);

                    DataSet dsUser = dat.GetData("SELECT * FROM Users U WHERE User_ID=" + dsRevision.Tables[0].Rows[0]["modifierID"].ToString());
                    string emailBody = "The revised venue " + columnName + " has been apporved by the author of the venue. <br/><br/> " +
                        "To view these changes, please visit <a href=\"http://HippoHappenings.com/"+dat.MakeNiceName(venueName) +"_"+ dsRevision.Tables[0].Rows[0]["VenueID"].ToString() + "_Venue\">" + venueName + "</a>";

                    if (!Request.IsLocal)
                    {
                        dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                        System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                        dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "One of your changes has been approved for venue: '" +
                        venueName + "'");
                    }
                    Literal lab = new Literal();

                    lab.Text = "<div class=\"AddGreenLink\" style=\"float: right;margin-top: 20px; padding-bottom: 4px;height: 30px;\">You have accepted this change.</div>";


                    Label lab2 = new Label();
                    HtmlButton but2 = new HtmlButton();
                    but2 = (HtmlButton)thebutton.Parent.Controls[thebutton.Parent.Controls.IndexOf(thebutton) + 1];

                    but2.Parent.Controls.AddAt(but2.Parent.Controls.IndexOf(but2), lab2);
                    but2.Parent.Controls.Remove(but2);
                    //Put the label in place of the button
                    thebutton.Parent.Controls.AddAt(thebutton.Parent.Controls.IndexOf(thebutton), lab);
                    //Then remove the button
                    thebutton.Parent.Controls.Remove(thebutton);
                }
            }
            else
            {


                if (thebutton.Attributes["commandargument"].Contains("occurance"))
                {
                    //MessagesLabel.Text = "got here ";
                    string rowID = thebutton.Attributes["commandargument"].Replace("occurance", "");
                    theRow = rowID;
                    //MessagesLabel.Text = rowID;
                    dat.Execute("INSERT INTO Event_Occurance (EventID, DateTimeStart, DateTimeEnd) " +
        "SELECT EventID, DateTimeStart, DateTimeEnd FROM EventRevisions_Occurance WHERE RevisionID=" + rowID);
                    //MessagesLabel.Text = "flew here";
                    dat.Execute("UPDATE EventRevisions_Occurance SET Approved='True' WHERE RevisionID=" + rowID);

                    DataSet dsRevision = dat.GetData("SELECT E.Header AS H1, E.ID AS TID, *  FROM EventRevisions ER, Events E WHERE E.ID=ER.EventID AND ER.ID=" + rowID);

                    DataSet dsUser = dat.GetData("SELECT * FROM Users U WHERE User_ID=" + dsRevision.Tables[0].Rows[0]["modifierID"].ToString());
                    string emailBody = "The revised event re-occurance dates have been apporved by the author of the event. <br/><br/> " +
                        "To view these changes, please visit <a href=\"http://HippoHappenings.com/"+dat.MakeNiceName(dsRevision.Tables[0].Rows[0]["H1"].ToString())+"_" + dsRevision.Tables[0].Rows[0]["TID"].ToString() + "_Event\">" + dsRevision.Tables[0].Rows[0]["H1"].ToString() + "</a>";

                    if (!Request.IsLocal)
                    {
                        dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                        System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                        dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "One of your changes has been approved for event: '" +
                        dsRevision.Tables[0].Rows[0]["H1"].ToString() + "'");

                        //Send email to all users who have this event in their calendar and have their email preference set for event updates

                        emailBody = "<br/><br/>Changes have been made to an event in your calendar: Event '\"" + dsRevision.Tables[0].Rows[0]["H1"].ToString() +
                            "\"'. <br/><br/> To view these changes, please go to this event's <a class=\"AddLink\"  href=\"http://hippohappenings.com/" + dat.MakeNiceName(dsRevision.Tables[0].Rows[0]["H1"].ToString()) + "_" + dsRevision.Tables[0].Rows[0]["TID"].ToString() + "_Event\">page</a>. " +
                            "<br/><br/><br/>Have a Hippo Happening Day!<br/><br/> <a class=\"AddLink\"  href=\"http://HippoHappenings.com\">Happening Hippo</a>";

                        DataSet dsAllUsers = dat.GetData("SELECT * FROM User_Calendar UC, Users U, UserPreferences UP WHERE U.User_ID=UP.UserID AND UP.EmailPrefs LIKE '%C%' AND U.User_ID=UC.UserID AND UC.EventID=" + dsRevision.Tables[0].Rows[0]["TID"].ToString());

                        DataView dv = new DataView(dsAllUsers.Tables[0], "", "", DataViewRowState.CurrentRows);

                        if (dv.Count > 0)
                        {
                            for (int i = 0; i < dv.Count; i++)
                            {
                                dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                                    dv[i]["Email"].ToString(), emailBody,
                                    "Event '" +
                                    dsRevision.Tables[0].Rows[0]["H1"].ToString() + "' has been modified");

                                dat.Execute("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, " +
                                    "To_UserID, Date, [Read], Mode, Live, SentLive) VALUES('" + emailBody.Replace("'", "''") + "', '" + "Event ''" +
                                    dsRevision.Tables[0].Rows[0]["H1"].ToString().Replace("'", "''") + "'' has been modified', "+dat.HIPPOHAPP_USERID.ToString()+", " + dv[i]["UserID"].ToString() + ", GETDATE(), 0, 1, 1, 0)");
                            }
                        }

                    }

                    Literal lab = new Literal();
                    lab.Text = "<div class=\"AddGreenLink\" style=\"float: right;margin-top: 20px; padding-bottom: 4px;height: 30px;\">You have accepted this change.</div>";

                    Label lab2 = new Label();
                    HtmlButton but2 = new HtmlButton();
                    but2 = (HtmlButton)thebutton.Parent.Controls[thebutton.Parent.Controls.IndexOf(thebutton) + 1];

                    but2.Parent.Controls.AddAt(but2.Parent.Controls.IndexOf(but2), lab2);
                    but2.Parent.Controls.Remove(but2);
                    //Put the label in place of the button
                    thebutton.Parent.Controls.AddAt(thebutton.Parent.Controls.IndexOf(thebutton), lab);
                    //Then remove the button
                    thebutton.Parent.Controls.Remove(thebutton);
                }
                else
                {

                    string[] delim = { "accept" };
                    string[] tokens = thebutton.Attributes["commandargument"].Split(delim, StringSplitOptions.None);

                    string rowID = tokens[0];
                    string columnNumber = tokens[1];
                    theRow = rowID;
                    DataSet dsEventID = dat.GetData("SELECT * FROM EventRevisions WHERE ID=" + rowID);

                    string eventID = dsEventID.Tables[0].Rows[0]["EventID"].ToString();

                    string columnName = dsEventID.Tables[0].Columns[int.Parse(columnNumber)].ColumnName;

                    string temp = dsEventID.Tables[0].Rows[0][columnName].ToString();

                    dat.ApproveRejectChange("EventRevisions", rowID, int.Parse(columnNumber)-3, true);


                    //if user is accepting a venue change, also accept zip, city, state and country
                    if (columnName.ToLower() == "venue")
                    {
                        dat.ApproveRejectChange("EventRevisions", rowID, 3, true);
                        dat.ApproveRejectChange("EventRevisions", rowID, 4, true);
                        dat.ApproveRejectChange("EventRevisions", rowID, 5, true);
                        dat.ApproveRejectChange("EventRevisions", rowID, 6, true);
                        dat.ApproveRejectChange("EventRevisions", rowID, 10, true);
                    }



                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE Events SET " + columnName + "=@p1 WHERE ID=@eventID", conn);
                    cmd.Parameters.Add("@p1", SqlDbType.NVarChar).Value = dsEventID.Tables[0].Rows[0][columnName].ToString();
                    cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = eventID;
                    cmd.ExecuteNonQuery();
                    conn.Close();


                    DataSet dsRevision = dat.GetData("SELECT E.Header AS H1, E.ID AS TID, * FROM EventRevisions ER, Events E WHERE E.ID=ER.EventID AND ER.ID=" + rowID);

                    DataSet dsUser = dat.GetData("SELECT * FROM Users U WHERE User_ID=" + dsRevision.Tables[0].Rows[0]["modifierID"].ToString());
                    string emailBody = "The revised event " + columnName + " has been apporved by the author of the event. <br/><br/> " +
                        "To view these changes, please visit <a href=\"http://HippoHappenings.com/" + dat.MakeNiceName(dsRevision.Tables[0].Rows[0]["H1"].ToString()) + "_" + dsRevision.Tables[0].Rows[0]["TID"].ToString() + "_Event\">" + dsRevision.Tables[0].Rows[0]["H1"].ToString() + "</a>";

                    if (!Request.IsLocal)
                    {

                        dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                        System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                        dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "One of your changes has been approved for event: '" +
                        dsRevision.Tables[0].Rows[0]["H1"].ToString() + "'");

                        //Send email to all users who have this event in their calendar and have their email preference set for event updates

                        emailBody = "<br/><br/>Changes have been made to an event in your calendar: Event '\"" + dsRevision.Tables[0].Rows[0]["H1"].ToString() +
                            "\"'. <br/><br/> To view these changes, please go to this event's <a class=\"AddLink\" href=\"http://hippohappenings.com/" + dat.MakeNiceName(dsRevision.Tables[0].Rows[0]["H1"].ToString()) + "_" + dsRevision.Tables[0].Rows[0]["TID"].ToString() + "_Event\">page</a>. " +
                            "<br/><br/><br/>Have a Hippo Happening Day!<br/><br/> <a class=\"AddLink\" href=\"http://HippoHappenings.com\">HippoHappenings</a>";

                        DataSet dsAllUsers = dat.GetData("SELECT * FROM User_Calendar UC, Users U, UserPreferences UP " +
                            "WHERE U.User_ID=UP.UserID  AND U.User_ID=UC.UserID AND UC.EventID=" +
                            dsRevision.Tables[0].Rows[0]["TID"].ToString());

                        DataView dv = new DataView(dsAllUsers.Tables[0], "", "", DataViewRowState.CurrentRows);

                        if (dv.Count > 0)
                        {
                            for (int i = 0; i < dv.Count; i++)
                            {
                                if (dv[i]["EmailPrefs"].ToString().Contains("C"))
                                {
                                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                        System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                                        dv[i]["Email"].ToString(), emailBody,
                                        "Event '" +
                                        dsRevision.Tables[0].Rows[0]["H1"].ToString() + "' has been modified");
                                }
                                

                                dat.Execute("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, " +
                                    "To_UserID, Date, [Read], Mode, Live, SentLive) VALUES('" + emailBody.Replace("'", "''") + "', '" + "Event ''" +
                                    dsRevision.Tables[0].Rows[0]["H1"].ToString().Replace("'", "''") + "'' has been modified', " + dat.HIPPOHAPP_USERID +
                                    ", " + dv[i]["UserID"].ToString() + ", GETDATE(), 0, 1, 1, 0)");
                            }
                        }
                    }
                    Literal lab = new Literal();
                    lab.Text = "<div class=\"AddGreenLink\" style=\"float: right;margin-top: 20px; padding-bottom: 4px;height: 30px;\">You have accepted this change.</div>";
                    


                    Label lab2 = new Label();
                    HtmlButton but2 = new HtmlButton();
                    but2 = (HtmlButton)thebutton.Parent.Controls[thebutton.Parent.Controls.IndexOf(thebutton) + 1];

                    but2.Parent.Controls.AddAt(but2.Parent.Controls.IndexOf(but2), lab2);
                    but2.Parent.Controls.Remove(but2);
                    //Put the label in place of the button
                    thebutton.Parent.Controls.AddAt(thebutton.Parent.Controls.IndexOf(thebutton), lab);
                    //Then remove the button
                    thebutton.Parent.Controls.Remove(thebutton);
                }


            }
        }
        catch (Exception ex)
        {
            MessagesLabel.Text = ex.ToString();
        }
    }

    protected void ServerRejectChange(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        try
        {
            HtmlButton thebutton = (HtmlButton)sender;

            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            string theRow = "";
            bool isVenue = false;

            if (thebutton.Attributes["commArg"].ToString().ToLower() == "venue")
                isVenue = true;

            string rowID = "";

            if (isVenue)
            {
                if (thebutton.Attributes["commandargument"].Contains("category"))
                {
                    //THIS PART NOW TAKEN CARE OF BY REJECTCATEGORY METHOD
                    //rowID = thebutton.Attributes["commandargument"].Replace("category", "");
                    //theRow = rowID;

                    //string venueName = dat.GetData("SELECT * FROM Venues V, VenueCategoryRevisions VCR WHERE V.ID=VCR.VenueID AND VCR.ID=" + rowID).Tables[0].Rows[0]["Name"].ToString();

                    //string categoryName = dat.GetData("SELECT * FROM VenueCategoryRevisions VCR, VenueCategories VC " +
                    //    "WHERE VC.CategoryID=VCR.CategoryID AND VCR.ID=" + rowID).Tables[0].Rows[0]["CategoryName"].ToString();
                    //dat.Execute("UPDATE VenueCategoryRevisions SET Approved='False' WHERE ID=" + rowID);

                    //DataSet dsRevision = dat.GetData("SELECT * FROM VenueCategoryRevisions VCR, Users U WHERE U.User_ID=VCR.modifierID AND VCR.ID=" + rowID);


                    //DataSet dsUser = dat.GetData("SELECT * FROM Users U WHERE User_ID=" + dsRevision.Tables[0].Rows[0]["modifierID"].ToString());
                    //string emailBody = "The category suggestion '" + categoryName + "' has been rejected for the venue '" + venueName +
                    //    "' by the venue's author. <br/><br/> " +
                    //    "To view the venue, please visit <a href=\"http://HippoHappenings.com/Venue.aspx?ID=" +
                    //    dsRevision.Tables[0].Rows[0]["VenueID"].ToString() + "\">" + venueName + "</a>";
                    //dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                    //System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                    //dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "One of your changes has been rejected for venue: '" +
                    //venueName + "'");

                    //Literal lab = new Literal();

                    //lab.Text = "<div class=\"AddGreenLink\" style=\"float: right;margin-top: 20px; padding-bottom: 4px;height: 30px;\">You have rejected this change.</div>";
                    ////Remove the 'Accept' button first
                    //thebutton.Page.Controls.RemoveAt(thebutton.Parent.Controls.IndexOf(thebutton) - 1);
                    ////Put the label in place of the 'Reject' button
                    //thebutton.Parent.Controls.AddAt(thebutton.Parent.Controls.IndexOf(thebutton), lab);
                    ////Then remove the 'Reject' button
                    //thebutton.Parent.Controls.Remove(thebutton);
                }
                else
                {

                    string[] delim = { "reject" };
                    string[] tokens = thebutton.Attributes["commandargument"].Split(delim, StringSplitOptions.None);
                    rowID = tokens[0];
                    int columnNumber = int.Parse(tokens[1]);
                    dat.ApproveRejectChange("VenueRevisions", rowID, columnNumber-3, false);

                    theRow = rowID;
                    DataSet dsEventID = dat.GetData("SELECT * FROM VenueRevisions WHERE ID=" + rowID);

                    string eventID = dsEventID.Tables[0].Rows[0]["VenueID"].ToString();

                    string columnName = dsEventID.Tables[0].Columns[columnNumber].ColumnName;

                    string temp = dsEventID.Tables[0].Rows[0][columnName].ToString();

                    Literal lab = new Literal();

                    lab.Text = "<div class=\"AddGreenLink\" style=\"float: right;margin-top: 20px; padding-bottom: 4px;height: 30px;\">You have rejected this change.</div>";
                    string venueName = dat.GetData("SELECT * FROM Venues V, VenueRevisions VCR WHERE V.ID=VCR.VenueID AND VCR.ID=" +
                        rowID).Tables[0].Rows[0]["Name"].ToString();

                    //string categoryName = dat.GetData("SELECT * FROM VenueRevisions VCR, VenueCategories VC " +
                    //    "WHERE VC.CategoryID=VCR.CategoryID AND VCR.ID=" + rowID).Tables[0].Rows[0]["CategoryName"].ToString();

                    DataSet dsRevision = dat.GetData("SELECT * FROM VenueRevisions VCR, Users U WHERE U.User_ID=VCR.modifierID AND VCR.ID=" +
                        rowID);

                    DataSet dsUser = dat.GetData("SELECT * FROM Users U WHERE User_ID=" + dsRevision.Tables[0].Rows[0]["modifierID"].ToString());
                    string emailBody = "The submitted change for " + columnName + " has been rejected by the author of the venue. <br/><br/> " +
                        "To view these changes, please visit <a href=\"http://HippoHappenings.com/"+dat.MakeNiceName(venueName)+"_" + dsRevision.Tables[0].Rows[0]["VenueID"].ToString() + "_Venue\">" + venueName + "</a>";


                    if (!Request.IsLocal)
                    {
                        dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                        System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                        dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "One of your changes has been rejected for venue: '" +
                        venueName + "'");
                    }

                    Label lab2 = new Label();
                    HtmlButton but2 = new HtmlButton();
                    but2 = (HtmlButton)thebutton.Parent.Controls[thebutton.Parent.Controls.IndexOf(thebutton) - 1];

                    but2.Parent.Controls.AddAt(but2.Parent.Controls.IndexOf(but2), lab2);
                    but2.Parent.Controls.Remove(but2);
                    //Put the label in place of the button
                    thebutton.Parent.Controls.AddAt(thebutton.Parent.Controls.IndexOf(thebutton), lab);
                    //Then remove the button
                    thebutton.Parent.Controls.Remove(thebutton);
                }
            }
            else
            {
                if (thebutton.Attributes["commandargument"].Contains("occurance"))
                {
                    rowID = thebutton.Attributes["commandargument"].Replace("occurance", "");
                    theRow = rowID;

                    dat.Execute("UPDATE EventRevisions_Occurance SET Approved='False' WHERE RevisionID=" + rowID);

                    DataSet dsRevision = dat.GetData("SELECT E.Header AS H1, E.ID AS TID, *  FROM EventRevisions ER, Events E WHERE E.ID=ER.EventID AND ER.ID=" + rowID);

                    DataSet dsUser = dat.GetData("SELECT * FROM Users U WHERE User_ID=" + dsRevision.Tables[0].Rows[0]["modifierID"].ToString());
                    string emailBody = "The revised event re-occurance dates have been rejected by the author of the event. <br/><br/> " +
                        "We appologize for any inconvenience. <br/><br/>To view the event, please visit <a href=\"http://HippoHappenings.com/"+dat.MakeNiceName(dsRevision.Tables[0].Rows[0]["H1"].ToString())+"_" +
                        dsRevision.Tables[0].Rows[0]["TID"].ToString() + "_Event\">" + dsRevision.Tables[0].Rows[0]["H1"].ToString() + "</a>";
                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                    dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "One of your changes has been rejected for event: '" +
                    dsRevision.Tables[0].Rows[0]["H1"].ToString() + "'");

                    Literal lab = new Literal();

                    lab.Text = "<div class=\"AddGreenLink\" style=\"float: right;margin-top: 20px; padding-bottom: 4px;height: 30px;\">You have rejected this change.</div>";
                    


                    Label lab2 = new Label();
                    HtmlButton but2 = new HtmlButton();
                    but2 = (HtmlButton)thebutton.Parent.Controls[thebutton.Parent.Controls.IndexOf(thebutton) + 1];

                    but2.Parent.Controls.AddAt(but2.Parent.Controls.IndexOf(but2), lab2);
                    but2.Parent.Controls.Remove(but2);
                    //Put the label in place of the button
                    thebutton.Parent.Controls.AddAt(thebutton.Parent.Controls.IndexOf(thebutton), lab);
                    //Then remove the button
                    thebutton.Parent.Controls.Remove(thebutton);
                }
                else
                {

                    string[] delim = { "reject" };
                    string[] tokens = thebutton.Attributes["commandargument"].Split(delim, StringSplitOptions.None);

                    rowID = tokens[0];
                    int columnNumber = int.Parse(tokens[1]);
                    theRow = rowID;
                    DataSet dsEventID = dat.GetData("SELECT * FROM EventRevisions WHERE ID=" + rowID);

                    dat.ApproveRejectChange("EventRevisions", rowID, columnNumber-3, false);

                    string eventID = dsEventID.Tables[0].Rows[0]["EventID"].ToString();

                    string columnName = dsEventID.Tables[0].Columns[columnNumber].ColumnName;

                    string temp = dsEventID.Tables[0].Rows[0][columnName].ToString();

                    if (columnName.ToLower() == "venue")
                    {
                        dat.ApproveRejectChange("EventRevisions", rowID, 3, false);
                        dat.ApproveRejectChange("EventRevisions", rowID, 4, false);
                        dat.ApproveRejectChange("EventRevisions", rowID, 5, false);
                        dat.ApproveRejectChange("EventRevisions", rowID, 6, false);
                        dat.ApproveRejectChange("EventRevisions", rowID, 10, false);
                    }

                    DataSet dsRevision = dat.GetData("SELECT E.Header AS H1, E.ID AS TID, * FROM EventRevisions ER, Events E WHERE E.ID=ER.EventID AND ER.ID=" + rowID);

                    DataSet dsUser = dat.GetData("SELECT * FROM Users U WHERE User_ID=" + dsRevision.Tables[0].Rows[0]["modifierID"].ToString());
                    string emailBody = "The revised event " + columnName + " has been rejected by the author of the event. <br/><br/> " +
                        "We appologize for any inconvenience. To view the event, please visit <a href=\"http://HippoHappenings.com/" +dat.MakeNiceName(dsRevision.Tables[0].Rows[0]["H1"].ToString())+"_"+
                        dsRevision.Tables[0].Rows[0]["TID"].ToString() + "_Event\">" + dsRevision.Tables[0].Rows[0]["H1"].ToString() + "</a>";
                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                    dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "One of your changes has been rejected for event: '" +
                    dsRevision.Tables[0].Rows[0]["H1"].ToString() + "'");

                    Literal lab = new Literal();

                    lab.Text = "<div class=\"AddGreenLink\" style=\"float: right;margin-top: 20px; padding-bottom: 4px;height: 30px;\">You have rejected this change.</div>";
                    

                    Label lab2 = new Label();
                    HtmlButton but2 = new HtmlButton();
                    but2 = (HtmlButton)thebutton.Parent.Controls[thebutton.Parent.Controls.IndexOf(thebutton) - 1];

                    but2.Parent.Controls.AddAt(but2.Parent.Controls.IndexOf(but2), lab2);
                    but2.Parent.Controls.Remove(but2);
                    //Put the label in place of the button
                    thebutton.Parent.Controls.AddAt(thebutton.Parent.Controls.IndexOf(thebutton), lab);
                    //Then remove the button
                    thebutton.Parent.Controls.Remove(thebutton);
                }


            }
        }
        catch (Exception ex)
        {
            MessagesLabel.Text = ex.ToString();
        }
        
    }

    protected void AddCategory(object sender, EventArgs e)
    {
        //MessagesLabel.Text += "got here";
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        try
        {
            HtmlButton theButt = (HtmlButton)sender;

            string[] delim = { "category" };
            string[] tokens = theButt.Attributes["commandargument"].Split(delim, StringSplitOptions.None);
            string CatID = tokens[2];
            string venueOrEvent = tokens[1];
            string contentID = tokens[0];
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            //MessagesLabel.Text += "tok 1: " + tokens[0] + ", tok2: " + tokens[1] + ", tok3: " + tokens[2];

            Literal lab = new Literal();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
            conn.Open();
            SqlCommand cmd;
            if (venueOrEvent == "venue")
            {
                cmd = new SqlCommand("INSERT INTO Venue_Category (VENUE_ID, CATEGORY_ID) VALUES(@vID, @cID)", conn);
                cmd.Parameters.Add("@vID", SqlDbType.Int).Value = contentID;
                cmd.Parameters.Add("@cID", SqlDbType.Int).Value = CatID;
                cmd.ExecuteNonQuery();

                dat.Execute("UPDATE VenueCategoryRevisions SET Approved='True' WHERE ID=" + tokens[3]);

                #region Send Email
                //send email
                string rowID = tokens[3];

                string venueName = dat.GetData("SELECT * FROM Venues V, VenueCategoryRevisions VCR WHERE V.ID=VCR.VenueID AND VCR.ID=" + rowID).Tables[0].Rows[0]["Name"].ToString();

                string categoryName = dat.GetData("SELECT * FROM VenueCategoryRevisions VCR, VenueCategories VC " +
                    "WHERE VC.ID=VCR.CatID AND VCR.ID=" + rowID).Tables[0].Rows[0]["Name"].ToString();

                DataSet dsRevision = dat.GetData("SELECT * FROM VenueCategoryRevisions VCR, Users U WHERE U.User_ID=VCR.modifierID AND VCR.ID=" + rowID);



                DataSet dsUser = dat.GetData("SELECT * FROM Users U WHERE User_ID=" + dsRevision.Tables[0].Rows[0]["modifierID"].ToString());
                string emailBody = "The addition of category '" + categoryName + "' has been approved for the venue '" + venueName +
                    "' by the venue's author. <br/><br/> " +
                    "To view these changes, please visit <a href=\"http://HippoHappenings.com/"+dat.MakeNiceName(venueName)+"_" +
                    dsRevision.Tables[0].Rows[0]["VenueID"].ToString() + "_Venue\">" + venueName + "</a>";

                if (!Request.IsLocal)
                {
                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                    dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "One of your changes has been approved for venue: '" +
                    venueName + "'");
                }
                #endregion
            }
            else
            {
                cmd = new SqlCommand("INSERT INTO Event_Category_Mapping (EventID, CategoryID) VALUES(@vID, @cID)",
                    conn);
                cmd.Parameters.Add("@vID", SqlDbType.Int).Value = contentID;
                cmd.Parameters.Add("@cID", SqlDbType.Int).Value = CatID;
                cmd.ExecuteNonQuery();

                dat.Execute("UPDATE EventCategoryRevisions SET Approved='True' WHERE ID=" + tokens[3]);

                #region Send Email
                //send email
                string rowID = tokens[3];

                string eventName = dat.GetData("SELECT * FROM Events V, EventCategoryRevisions VCR WHERE V.ID=VCR.EventID AND VCR.ID=" + rowID).Tables[0].Rows[0]["Header"].ToString();

                string categoryName = dat.GetData("SELECT * FROM EventCategoryRevisions VCR, EventCategories VC " +
                    "WHERE VC.ID=VCR.CatID AND VCR.ID=" + rowID).Tables[0].Rows[0]["Name"].ToString();

                DataSet dsRevision = dat.GetData("SELECT * FROM EventCategoryRevisions VCR, Users U WHERE U.User_ID=VCR.modifierID AND VCR.ID=" + rowID);



                DataSet dsUser = dat.GetData("SELECT * FROM Users U WHERE User_ID=" + dsRevision.Tables[0].Rows[0]["modifierID"].ToString());
                string emailBody = "The addition of category '" + categoryName + "' has been approved for the event '" + eventName +
                    "' by the event's author. <br/><br/> " +
                    "To view these changes, please visit <a href=\"http://HippoHappenings.com/" +dat.MakeNiceName(eventName)+"_"+
                    dsRevision.Tables[0].Rows[0]["EventID"].ToString() + "_Event\">" + eventName + "</a>";

                if (!Request.IsLocal)
                {
                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                    dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "One of your changes has been approved for event: '" +
                    eventName + "'");
                }
                #endregion
            }




            conn.Close();
            lab.Text = "<label class=\"AddGreenLink\">&nbsp;&nbsp;&nbsp;&nbsp;Added</label>";
            Label lab2 = new Label();
            Literal theLit =
                (Literal)theButt.Parent.Controls[theButt.Parent.Controls.IndexOf(theButt) - 1];
            theLit.Text = theLit.Text.Replace("<br/>", "");
            theLit.Text = theLit.Text.Replace("<div style=\"padding-bottom: 4px;\">", 
                "<div align=\"right\" style=\"padding-bottom: 4px;\">");

            HtmlButton but2 = new HtmlButton();
            but2 = (HtmlButton)theButt.Parent.Controls[theButt.Parent.Controls.IndexOf(theButt) + 1];

            but2.Parent.Controls.AddAt(but2.Parent.Controls.IndexOf(but2), lab2);
            but2.Parent.Controls.Remove(but2);
            //Put the label in place of the button
            theButt.Parent.Controls.AddAt(theButt.Parent.Controls.IndexOf(theButt), lab);
            //Then remove the button
            theButt.Parent.Controls.Remove(theButt);
        }
        catch (Exception ex)
        {
            MessagesLabel.Text = ex.ToString();
        }

    }

    protected void RemoveCategory(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        try
        {
            HtmlButton theButt = (HtmlButton)sender;

            string[] delim = { "category" };
            string[] tokens = theButt.Attributes["commandargument"].Split(delim, StringSplitOptions.None);
            string CatID = tokens[2];
            string venueOrEvent = tokens[1];
            string contentID = tokens[0];
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            Literal lab = new Literal();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
            conn.Open();
            SqlCommand cmd;
            if (venueOrEvent == "venue")
            {
                cmd = new SqlCommand("DELETE FROM Venue_Category WHERE Venue_ID=@vID AND Category_ID=@cID", conn);
                cmd.Parameters.Add("@vID", SqlDbType.Int).Value = contentID;
                cmd.Parameters.Add("@cID", SqlDbType.Int).Value = CatID;
                cmd.ExecuteNonQuery();

                dat.Execute("UPDATE VenueCategoryRevisions SET Approved='True' WHERE ID=" + tokens[3]);
                #region Send Email
                //send email
                string rowID = tokens[3];

                string venueName = dat.GetData("SELECT * FROM Venues V, VenueCategoryRevisions VCR WHERE V.ID=VCR.VenueID AND VCR.ID=" + rowID).Tables[0].Rows[0]["Name"].ToString();

                string categoryName = dat.GetData("SELECT * FROM VenueCategoryRevisions VCR, VenueCategories VC " +
                    "WHERE VC.ID=VCR.CatID AND VCR.ID=" + rowID).Tables[0].Rows[0]["Name"].ToString();

                DataSet dsRevision = dat.GetData("SELECT * FROM VenueCategoryRevisions VCR, Users U WHERE U.User_ID=VCR.modifierID AND VCR.ID=" + rowID);



                DataSet dsUser = dat.GetData("SELECT * FROM Users U WHERE User_ID=" + dsRevision.Tables[0].Rows[0]["modifierID"].ToString());
                string emailBody = "The removal of category '" + categoryName + "' has been approved for the venue '" + venueName +
                    "' by the venue's author. <br/><br/> " +
                    "To view these changes, please visit <a href=\"http://HippoHappenings.com/"+dat.MakeNiceName(venueName) + "_"+
                    dsRevision.Tables[0].Rows[0]["VenueID"].ToString() + "_Venue\">" + venueName + "</a>";

                if (!Request.IsLocal)
                {
                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                    dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "One of your changes has been approved for venue: '" +
                    venueName + "'");
                }
                #endregion
            }
            else
            {
                cmd = new SqlCommand("DELETE FROM Event_Category_Mapping WHERE EventID=@vID AND CategoryID=@cID", conn);
                cmd.Parameters.Add("@vID", SqlDbType.Int).Value = contentID;
                cmd.Parameters.Add("@cID", SqlDbType.Int).Value = CatID;
                cmd.ExecuteNonQuery();

                dat.Execute("UPDATE EventCategoryRevisions SET Approved='True' WHERE ID=" + tokens[3]);

                #region Send Email
                //send email
                string rowID = tokens[3];

                string eventName = dat.GetData("SELECT * FROM Events V, EventCategoryRevisions VCR WHERE V.ID=VCR.EventID AND VCR.ID=" + rowID).Tables[0].Rows[0]["Header"].ToString();

                string categoryName = dat.GetData("SELECT * FROM EventCategoryRevisions VCR, EventCategories VC " +
                    "WHERE VC.ID=VCR.CatID AND VCR.ID=" + rowID).Tables[0].Rows[0]["Name"].ToString();

                DataSet dsRevision = dat.GetData("SELECT * FROM EventCategoryRevisions VCR, Users U WHERE U.User_ID=VCR.modifierID AND VCR.ID=" + rowID);



                DataSet dsUser = dat.GetData("SELECT * FROM Users U WHERE User_ID=" + dsRevision.Tables[0].Rows[0]["modifierID"].ToString());
                string emailBody = "The removal of category '" + categoryName + "' has been approved for the event '" + eventName +
                    "' by the event's author. <br/><br/> " +
                    "To view these changes, please visit <a href=\"http://HippoHappenings.com/" +dat.MakeNiceName(eventName)+"_"+
                    dsRevision.Tables[0].Rows[0]["EventID"].ToString() + "_Event\">" + eventName + "</a>";

                if (!Request.IsLocal)
                {
                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                    dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "One of your changes has been approved for event: '" +
                    eventName + "'");
                }
                #endregion
            }
            conn.Close();
            lab.Text = "<label class=\"AddGreenLink\">&nbsp;&nbsp;&nbsp;&nbsp;Removed</label>";


            Literal theLit =
                (Literal)theButt.Parent.Controls[theButt.Parent.Controls.IndexOf(theButt) - 1];
            theLit.Text = theLit.Text.Replace("<br/>", "");
            theLit.Text = theLit.Text.Replace("<div style=\"padding-bottom: 4px;\">", "<div align=\"right\" style=\"padding-bottom: 4px;\">");
            Label lab2 = new Label();


            HtmlButton but2 = new HtmlButton();
            but2 = (HtmlButton)theButt.Parent.Controls[theButt.Parent.Controls.IndexOf(theButt) + 1];

            but2.Parent.Controls.AddAt(but2.Parent.Controls.IndexOf(but2), lab2);
            but2.Parent.Controls.Remove(but2);
            //Put the label in place of the button
            theButt.Parent.Controls.AddAt(theButt.Parent.Controls.IndexOf(theButt), lab);
            //Then remove the button
            theButt.Parent.Controls.Remove(theButt);
        }
        catch (Exception ex)
        {
            MessagesLabel.Text = ex.ToString();
        }
    }

    protected void RejectCategory(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        try
        {
            HtmlButton theButt = (HtmlButton)sender;

            string[] delim = { "category" };
            string[] tokens = theButt.Attributes["commandargument"].Split(delim, StringSplitOptions.None);
            string CatID = tokens[2];
            string venueOrEvent = tokens[1];
            string contentID = tokens[0];
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            Literal lab = new Literal();
            if (venueOrEvent == "venue")
            {

                dat.Execute("UPDATE VenueCategoryRevisions SET Approved='False' WHERE ID=" + tokens[3]);
                #region Send Email
                //send email
                string rowID = tokens[3];

                string venueName = dat.GetData("SELECT * FROM Venues V, VenueCategoryRevisions VCR WHERE V.ID=VCR.VenueID AND VCR.ID=" + rowID).Tables[0].Rows[0]["Name"].ToString();

                string categoryName = dat.GetData("SELECT * FROM VenueCategoryRevisions VCR, VenueCategories VC " +
                    "WHERE VC.ID=VCR.CatID AND VCR.ID=" + rowID).Tables[0].Rows[0]["Name"].ToString();

                DataSet dsRevision = dat.GetData("SELECT * FROM VenueCategoryRevisions VCR, Users U WHERE U.User_ID=VCR.modifierID AND VCR.ID=" + rowID);



                DataSet dsUser = dat.GetData("SELECT * FROM Users U WHERE User_ID=" + dsRevision.Tables[0].Rows[0]["modifierID"].ToString());
                string emailBody = "The removal of category '" + categoryName + "' has been rejected for the venue '" + venueName +
                    "' by the venue's author. <br/><br/> " +
                    "To view these changes, please visit <a href=\"http://HippoHappenings.com/" +dat.MakeNiceName(venueName)+"_"+
                    dsRevision.Tables[0].Rows[0]["VenueID"].ToString() + "_Venue\">" + venueName + "</a>";

                if (!Request.IsLocal)
                {
                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                    dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "One of your changes has been rejected for venue: '" +
                    venueName + "'");
                }
                #endregion
            }
            else
            {
                dat.Execute("UPDATE EventCategoryRevisions SET Approved='False' WHERE ID=" + tokens[3]);

                #region Send Email
                //send email
                string rowID = tokens[3];

                string eventName = dat.GetData("SELECT * FROM Events V, EventCategoryRevisions "+
                    "VCR WHERE V.ID=VCR.EventID AND VCR.ID=" + rowID).Tables[0].Rows[0]["Header"].ToString();

                string categoryName = dat.GetData("SELECT * FROM EventCategoryRevisions VCR, EventCategories VC " +
                    "WHERE VC.ID=VCR.CatID AND VCR.ID=" + rowID).Tables[0].Rows[0]["Name"].ToString();

                DataSet dsRevision = dat.GetData("SELECT * FROM EventCategoryRevisions VCR, Users U WHERE U.User_ID=VCR.modifierID AND VCR.ID=" + rowID);



                DataSet dsUser = dat.GetData("SELECT * FROM Users U WHERE User_ID=" + dsRevision.Tables[0].Rows[0]["modifierID"].ToString());
                string emailBody = "The removal of category '" + categoryName + "' has been rejected for the event '" + eventName +
                    "' by the event's author. <br/><br/> " +
                    "To view these changes, please visit <a href=\"http://HippoHappenings.com/" + dat.MakeNiceName(eventName) + "_" +
                    dsRevision.Tables[0].Rows[0]["EventID"].ToString() + "_Event\">" + eventName + "</a>";

                if (!Request.IsLocal)
                {
                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                    dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "One of your changes has been rejected for event: '" +
                    eventName + "'");
                }
                #endregion
            }
            lab.Text = "<label class=\"AddGreenLink\">&nbsp;&nbsp;&nbsp;&nbsp;Rejected</label>";

            Literal theLit =
        (Literal)theButt.Parent.Controls[theButt.Parent.Controls.IndexOf(theButt) - 2];
            theLit.Text = theLit.Text.Replace("<br/>", "");
            theLit.Text = theLit.Text.Replace("<div style=\"padding-bottom: 4px;\">", "<div align=\"right\" style=\"padding-bottom: 4px;\">");


            Label lab2 = new Label();


            HtmlButton but2 = new HtmlButton();
            but2 = (HtmlButton)theButt.Parent.Controls[theButt.Parent.Controls.IndexOf(theButt) - 1];

            but2.Parent.Controls.AddAt(but2.Parent.Controls.IndexOf(but2), lab2);
            but2.Parent.Controls.Remove(but2);
            //Put the label in place of the button
            theButt.Parent.Controls.AddAt(theButt.Parent.Controls.IndexOf(theButt), lab);
            //Then remove the button
            theButt.Parent.Controls.Remove(theButt);
        }
        catch (Exception ex)
        {
            MessagesLabel.Text = ex.ToString();
        }
    }

    protected void AddFriend(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        LinkButton link = (LinkButton)sender;

        int To_ID = int.Parse(link.CommandArgument);

        DataSet ds = dat.GetData("SELECT * FROM User_Friends WHERE UserID="+Session["User"].ToString() + 
            " AND FriendID="+To_ID);

        bool hasFriend = false;

        if (ds.Tables.Count > 0)
            if (ds.Tables[0].Rows.Count > 0)
                hasFriend = true;
            else
                hasFriend = false;
        else
            hasFriend = false;

        if (!hasFriend)
        {

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());

            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, To_UserID, Date, [Read], Mode)"
                + " VALUES(@content, @subject, @fromID, @toID, @date, 'False', 2)", conn);
            cmd.Parameters.Add("@content", SqlDbType.Text).Value = "Good Day from Hippo Happenings!, <br/><br/> We wanted to let you know that the user '" + Session["UserName"].ToString() + "' would like " +
                "to add you to their list of friends. To accept this request select the link below. <br/><br/> Have a Happening Day! <br/><br/> ";
            cmd.Parameters.Add("@subject", SqlDbType.NVarChar).Value = "You Have a Hippo Friend Request!";
            cmd.Parameters.Add("@toID", SqlDbType.Int).Value = To_ID;
            cmd.Parameters.Add("@fromID", SqlDbType.Int).Value = Session["User"].ToString();
            cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
            cmd.ExecuteNonQuery();
            conn.Close();

            MessageLabel.Text = "Your friend request has been sent!";


        }
        else
            MessageLabel.Text = "The user you selected is already your friend!";



        Panel SearchResultsPanel = (Panel)dat.FindControlRecursive(this, "SearchResultsPanel");
        SearchResultsPanel.Controls.Clear();

        CloseSearchPanel();

    }
    
    protected void ViewFriend(object sender, EventArgs e)
    {
                HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        ClearMessage();
        ImageButton button = (ImageButton)sender;
        string arg = button.CommandArgument;
        Session["FriendToView"] = arg;
        DataView dvF = dat.GetDataDV("SELECT * FROM USERS WHERE USER_ID=" + arg);
        Response.Redirect(dvF[0]["UserName"].ToString() + "_Friend");
    }
    
    private void this_OnProgress(object sender, int ID)
    {
        if (Session["Archived"] != null)
            if (bool.Parse(Session["Archived"].ToString()))
            {
                LoadControls();
                DoAll();
            }

        Session["Archived"] = "False";

    }
    
    protected void GoTo(object sender, EventArgs e)
    {
        ImageButton b = (ImageButton)sender;

        string command = b.CommandArgument;

        switch (command)
        {
            case "P":
                Response.Redirect("UserPreferences.aspx?ID="+Session["User"].ToString());
                break;
            case "M":
                //MessagesPanel.Visible = true;
                //FriendPanel.Visible = false;
                //MessagesButton.ImageUrl = "image/MyMessagesHover.png";
                //FriendsButton.ImageUrl = "image/MyFriends.png";
                //MessagesButton.Attributes.Remove("onmouseover");
                //MessagesButton.Attributes.Remove("onmouseout");
                //FriendsButton.Attributes.Add("onmouseover", "this.src='image/MyFriendsHover.png'");
                //FriendsButton.Attributes.Add("onmouseout", "this.src='image/MyFriends.png'");
                break;
            case "F":
                //FriendPanel.Visible = true;
                //MessagesPanel.Visible = false;
                //MessagesButton.ImageUrl = "image/MyMessages.png";
                //MessagesButton.Attributes.Add("onmouseover", "this.src='image/MyMessagesHover.png'");
                //MessagesButton.Attributes.Add("onmouseout", "this.src='image/MyMessages.png'");
                //FriendsButton.ImageUrl = "image/MyFriendsHover.png";
                //FriendsButton.Attributes.Remove("onmouseover");
                //FriendsButton.Attributes.Remove("onmouseout");
                break;
            default: break;
        }
    }
   
    protected void ClearMessage()
    {
        MessageLabel.Text = "";
    }

    //[Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
    //public int Reply(string message, string subject, string fromID, string userID, string messageID, string i)
    //{
    //    try
    //    {
    //        SqlConnection conn;
    //        conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
    //        conn.Open();

    //        SqlCommand cmd = new SqlCommand("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, To_UserID, Date, [Read], Mode) " +
    //            " VALUES(@content, @subject, @from, @to, @date, 'false', 0)", conn);
    //        cmd.Parameters.Add("@content", SqlDbType.NVarChar).Value = message;
    //        cmd.Parameters.Add("@subject", SqlDbType.NVarChar).Value = "Re: " + subject;
    //        cmd.Parameters.Add("@from", SqlDbType.Int).Value = userID;
    //        cmd.Parameters.Add("@to", SqlDbType.Int).Value = fromID;
    //        cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
    //        cmd.ExecuteNonQuery();
    //        conn.Close();

            
    //    }
    //    catch (Exception ex)
    //    {
            
    //    }

    //    return int.Parse(i);
    //}

    //[Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
    //public int AcceptFriend(string userID, string friendID, string i)
    //{
    //    try
    //    {
    //        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

    //        SqlConnection conn;
    //        conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
    //        conn.Open();
    //        string username = dat.GetData("SELECT UserName FROM USERS WHERE User_ID=" + userID).Tables[0].Rows[0]["UserName"].ToString();
    //        //Add the friend.
    //        SqlCommand cmd = new SqlCommand("INSERT INTO User_Friends (FriendID, UserID) VALUES(@friend, @user)", conn);
    //        cmd.Parameters.Add("@friend", SqlDbType.Int).Value = userID;
    //        cmd.Parameters.Add("@user", SqlDbType.Int).Value = friendID;
    //        cmd.ExecuteNonQuery();
    //        cmd = new SqlCommand("INSERT INTO User_Friends (FriendID, UserID) VALUES(@friend, @user)", conn);
    //        cmd.Parameters.Add("@friend", SqlDbType.Int).Value = friendID;
    //        cmd.Parameters.Add("@user", SqlDbType.Int).Value = userID;
    //        cmd.ExecuteNonQuery();

    //        //Send message notifying the requestor that the friend has accepted
    //        cmd = new SqlCommand("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, To_UserID, Date, [Read], Mode) " +
    //            " VALUES(@content, @subject, @from, @to, @date, 'false', 1)", conn);
    //        cmd.Parameters.Add("@content", SqlDbType.NVarChar).Value = "Good Day from Hippo Happenings! <br/><br/> Congratulations! The friend request submited for user '" + username + "' has been approved by this user. You are now friends! ";
    //        cmd.Parameters.Add("@subject", SqlDbType.NVarChar).Value = "Friend Request Approved!";
    //        cmd.Parameters.Add("@from", SqlDbType.Int).Value = 6;
    //        cmd.Parameters.Add("@to", SqlDbType.Int).Value = friendID;
    //        cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
    //        cmd.ExecuteNonQuery();

    //        conn.Close();

    //    }
    //    catch (Exception ex)
    //    {

    //    }

    //    return int.Parse(i);

    //}

    //[Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
    //public int ArchiveMessage(string ID)
    //{
    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //    dat.Execute("UPDATE UserMessages SET Live='False' WHERE ID="+ID);

    //   return 0;
    //}

    protected void UploadPhoto(object sender, EventArgs e)
    {

        if (PictureUpload.HasFile)
        {
            if (!System.IO.Directory.Exists(MapPath(".").ToString() + "/UserFiles/" + Session["UserName"].ToString() +
                "/Profile/"))
            {
                System.IO.Directory.CreateDirectory(MapPath(".").ToString() + "/UserFiles/" + Session["UserName"].ToString() +
                "/Profile/");
            }

            char[] delim = { '.' };
            string[] tokens = PictureUpload.FileName.Split(delim);

            System.Drawing.Image img = System.Drawing.Image.FromStream(PictureUpload.PostedFile.InputStream);

            SaveThumbnail(img, false, MapPath(".").ToString() + "/UserFiles/" + Session["UserName"].ToString() +
                "/Profile/" + PictureUpload.FileName, "image/" + tokens[1].ToLower());

            //PictureUpload.SaveAs(MapPath(".").ToString() + "/UserFiles/" + Session["UserName"].ToString() +
            //    "/Profile/" + PictureUpload.FileName);
            System.Drawing.Image theimg = System.Drawing.Image.FromFile(Server.MapPath(".") + "/UserFiles/" + Session["UserName"].ToString() +
                "/Profile/" + PictureUpload.FileName);
            FriendImage.ImageUrl = "UserFiles/" + Session["UserName"].ToString() + "/Profile/" +
                PictureUpload.FileName;
            FriendImage.Width = theimg.Width;
            FriendImage.Height = theimg.Height;
            Session["ProfilePicture"] = PictureUpload.FileName;
        }
    }

    protected void Save(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        try
        {
            Data d = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            if (d.ValidateEmail(EmailTextBox.Text))
            {
                //This is a flag to check whether the ads on the site need to be reset. 
                //They will need to be reset if the user changed the location or ad categories
                bool resetAds = false;

                string USER_ID = Session["User"].ToString();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
                conn.Open();
                SqlCommand cmd;
               

                string nameStart = "";
                string nameEnd = "";
                if (LastNameTextBox.THE_TEXT.Trim() != "")
                    nameStart = ", LastName='" + LastNameTextBox.THE_TEXT.Trim().Replace("'", "''") + "' ";

                if(FirstNameTextBox.THE_TEXT.Trim() != "")
                    nameEnd =  ", FirstName='" + FirstNameTextBox.THE_TEXT.Trim().Replace("'", "''") + "' ";

                cmd = new SqlCommand("UPDATE Users SET Weekly='" + WeeklyCheckBox.Checked.ToString() + "', ProfilePicture=@pic, Email=@email, PhoneNumber=@phone " +
                    ", PhoneProvider=@provider " + nameStart + nameEnd + "WHERE User_ID=@id ", conn);
                if (Session["ProfilePicture"] != null)
                    cmd.Parameters.Add("@pic", SqlDbType.NVarChar).Value = Session["ProfilePicture"].ToString();
                else
                    cmd.Parameters.Add("@pic", SqlDbType.NVarChar).Value = DBNull.Value;
                cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = EmailTextBox.Text;
                cmd.Parameters.Add("@phone", SqlDbType.NVarChar).Value = d.RemoveNoneNumbers(PhoneTextBox.THE_TEXT);
                cmd.Parameters.Add("@provider", SqlDbType.Int).Value = ProviderDropDown.SelectedValue;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = USER_ID;
                cmd.ExecuteNonQuery();

                string prefs = "0";

                if (TextingCheckBoxList.Items[0].Selected)
                    prefs += "1";
                if (TextingCheckBoxList.Items[1].Selected)
                    prefs += "2";
                if (TextingCheckBoxList.Items[2].Selected)
                    prefs += "3";

                string emailPrefs = "0";

                if (EmailCheckList.Items[0].Selected)
                    emailPrefs += "1";
                if (EmailCheckList.Items[1].Selected)
                    emailPrefs += "2";
                if (EmailCheckList.Items[2].Selected)
                    emailPrefs += "3";

                if (EmailCheckList3.Items[0].Selected)
                    emailPrefs += "C";

                if (EmailUserCheckList1.Items[0].Selected)
                    emailPrefs += EmailUserCheckList1.Items[0].Value;
                if (EmailUserCheckList1.Items[1].Selected)
                    emailPrefs += EmailUserCheckList1.Items[1].Value;
                if (EmailUserCheckList1.Items[2].Selected)
                    emailPrefs += EmailUserCheckList1.Items[2].Value;

                if (EmailUserCheckList2.Items[0].Selected)
                    emailPrefs += EmailUserCheckList2.Items[0].Value;
                if (EmailUserCheckList2.Items[1].Selected)
                    emailPrefs += EmailUserCheckList2.Items[1].Value;
                if (EmailUserCheckList2.Items[2].Selected)
                    emailPrefs += EmailUserCheckList2.Items[2].Value;

                string calendarPrefs = "";
                if (PublicPrivateCheckList.SelectedValue != null)
                    if (PublicPrivateCheckList.SelectedValue != "")
                        calendarPrefs = PublicPrivateCheckList.SelectedValue;

                string commPrefs = "";
                if (CommunicationPrefsRadioList.SelectedValue != null)
                    if (CommunicationPrefsRadioList.SelectedValue != "")
                        commPrefs = CommunicationPrefsRadioList.SelectedValue;

                string commentsPrefs = "";
                if (CommentsRadioList.SelectedValue != null)
                    if (CommentsRadioList.SelectedValue != "")
                        commentsPrefs = CommentsRadioList.SelectedValue;

                string pollPrefs = "";
                //if (PollRadioList.SelectedValue != null)
                //    if (PollRadioList.SelectedValue != "")
                //        pollPrefs = PollRadioList.SelectedValue;

                string onoff = "";
                RadioButtonList CategoriesOnOffRadioList = (RadioButtonList)AdCategoryRadPanel.Items[0].Items[0].FindControl("CategoriesOnOffRadioList");
                Telerik.Web.UI.RadTreeView CategoryTree = (Telerik.Web.UI.RadTreeView)AdCategoryRadPanel.Items[0].Items[0].FindControl("CategoryTree");
                Telerik.Web.UI.RadTreeView RadTreeView2 = (Telerik.Web.UI.RadTreeView)AdCategoryRadPanel.Items[0].Items[0].FindControl("RadTreeView2");

                if (CategoriesOnOffRadioList.SelectedValue != null)
                    if (CategoriesOnOffRadioList.SelectedValue != "")
                        onoff = CategoriesOnOffRadioList.SelectedValue;

                
                string recommendPrefs = "";
                if (RecommendationsCheckList.Items[0].Selected)
                    recommendPrefs += "1";
                if (RecommendationsCheckList.Items[1].Selected)
                    recommendPrefs += "2";
                if (RecommendationsCheckList.Items[2].Selected)
                    recommendPrefs += "3";

                DataView usersPrevPrefs = d.GetDataDV("SELECT * FROM UserPreferences WHERE UserID="+Session["User"].ToString());

                string radius = "";
                if (RadiusPanel.Visible)
                {
                    radius = ", Radius="+RadiusDropDown.SelectedValue;
                }

                cmd = new SqlCommand("UPDATE UserPreferences SET Sex=@sex, Location=@location, CalendarPrivacyMode=@calendarmode " +
                    ", CommunicationPrefs=@commPrefs, TextingPrefs=@textprefs, EmailPrefs=@email, Address=@address, City=@city, " +
                    " CommentsPreferences=@comments, PollPreferences=@poll, CategoriesOnOff=@onoff, State=@state, ZIP=@zip, Country=@country, " +
                    " CatCountry=@catCountry" + radius + ", CatState=@catState, CatCity=@catCity, RecommendationPrefs=@rPrefs, CatZip=@catZip WHERE UserID=@id ", conn);
                //cmd.Parameters.Add("@age", SqlDbType.NVarChar).Value = AgeTextBox.Text;
                cmd.Parameters.Add("@sex", SqlDbType.NVarChar).Value = SexTextBox.Text;
                cmd.Parameters.Add("@location", SqlDbType.NVarChar).Value = LocationTextBox.Text;
                if (recommendPrefs != "")
                    cmd.Parameters.Add("@rPrefs", SqlDbType.Int).Value = recommendPrefs;
                else
                    cmd.Parameters.Add("@rPrefs", SqlDbType.Int).Value = DBNull.Value;
                if (onoff != "")
                    cmd.Parameters.Add("@onoff", SqlDbType.Int).Value = onoff;
                else
                    cmd.Parameters.Add("@onoff", SqlDbType.Int).Value = DBNull.Value;
                if (calendarPrefs != "")
                    cmd.Parameters.Add("@calendarmode", SqlDbType.Int).Value = calendarPrefs;
                else
                    cmd.Parameters.Add("@calendarmode", SqlDbType.Int).Value = DBNull.Value;
                if (commPrefs != "")
                    cmd.Parameters.Add("@commPrefs", SqlDbType.Int).Value = commPrefs;
                else
                    cmd.Parameters.Add("@commPrefs", SqlDbType.Int).Value = DBNull.Value;
                if (commentsPrefs != "")
                    cmd.Parameters.Add("@comments", SqlDbType.Int).Value = commentsPrefs;
                else
                    cmd.Parameters.Add("@comments", SqlDbType.Int).Value = DBNull.Value;
                if (pollPrefs != "")
                    cmd.Parameters.Add("@poll", SqlDbType.Int).Value = pollPrefs;
                else
                    cmd.Parameters.Add("@poll", SqlDbType.Int).Value = DBNull.Value;
                cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = AddressTextBox.THE_TEXT;
                cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = BillCityTextBox.THE_TEXT;
                
                
                if (BillCountryDropDown.SelectedValue != "-1")
                {
                    cmd.Parameters.Add("@country", SqlDbType.Int).Value = BillCountryDropDown.SelectedValue;

                    string state = "";
                    if (BillStateDropPanel.Visible)
                        state = BillStateDropDown.SelectedItem.Text;
                    else
                        state = BillStateTextBox.THE_TEXT;

                    cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = state;
                }
                else
                {
                    cmd.Parameters.Add("@country", SqlDbType.Int).Value = DBNull.Value;
                    cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = DBNull.Value;
                }

                

                if (CountryDropDown.SelectedValue != "-1")
                {
                    cmd.Parameters.Add("@catCountry", SqlDbType.Int).Value = CountryDropDown.SelectedValue;
                    Session["UserCountry"] = CountryDropDown.SelectedValue;

                    string state = "";
                    if (StateDropDownPanel.Visible)
                        state = StateDropDown.SelectedItem.Text;
                    else
                        state = StateTextBox.THE_TEXT;

                    if (state != "")
                    {
                        cmd.Parameters.Add("@catState", SqlDbType.NVarChar).Value = state;

                    }
                    else
                        cmd.Parameters.Add("@catState", SqlDbType.NVarChar).Value = DBNull.Value;
                    Session["UserState"] = state;
                    if (CityTextBox.THE_TEXT != "")
                    {
                        cmd.Parameters.Add("@catCity", SqlDbType.NVarChar).Value = CityTextBox.THE_TEXT.Trim();

                    }
                    else
                        cmd.Parameters.Add("@catCity", SqlDbType.NVarChar).Value = DBNull.Value;
                    Session["UserCity"] = CityTextBox.THE_TEXT.Trim();

                    if (CatZipTextBox.THE_TEXT.Trim() != "")
                    {
                        cmd.Parameters.Add("@catZip", SqlDbType.NVarChar).Value = d.GetAllZipsInRadius(RadiusDropDown.SelectedValue,
                            CatZipTextBox.THE_TEXT.Trim(), true);

                    }
                    else
                        cmd.Parameters.Add("@catZip", SqlDbType.NVarChar).Value = DBNull.Value;
                    Session["UserZip"] = CatZipTextBox.THE_TEXT.Trim();
                    Session["UserRadius"] = RadiusDropDown.SelectedValue;

                    if (CityTextBox.THE_TEXT.Trim() != usersPrevPrefs[0]["CatCity"].ToString() ||
                    state != usersPrevPrefs[0]["CatState"].ToString() ||
                    CountryDropDown.SelectedValue != usersPrevPrefs[0]["CatCountry"].ToString() ||
                    CatZipTextBox.THE_TEXT.Trim() != usersPrevPrefs[0]["CatZip"].ToString() ||
                    RadiusDropDown.SelectedValue != usersPrevPrefs[0]["Radius"].ToString())
                    {
                        resetAds = true;
                    }

                    
                }
                else
                {
                    cmd.Parameters.Add("@catCountry", SqlDbType.Int).Value = DBNull.Value;
                    cmd.Parameters.Add("@catState", SqlDbType.NVarChar).Value = DBNull.Value;
                    cmd.Parameters.Add("@catCity", SqlDbType.NVarChar).Value = DBNull.Value;
                    cmd.Parameters.Add("@catZip", SqlDbType.NVarChar).Value = DBNull.Value;
                }



                cmd.Parameters.Add("@zip", SqlDbType.NVarChar).Value = ZipTextBox.THE_TEXT;
                cmd.Parameters.Add("@textprefs", SqlDbType.Int).Value = prefs;
                cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = emailPrefs;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = USER_ID;
                cmd.ExecuteNonQuery();



                CreateCategories(ref resetAds);



                cmd = new SqlCommand("DELETE FROM UserVenues WHERE UserID=@user", conn);
                cmd.Parameters.Add("@user", SqlDbType.Int).Value = USER_ID;
                cmd.ExecuteNonQuery();

                CheckBoxList VenueCheckBoxes = (CheckBoxList)VenuesRadPanel.Items[0].Items[0].FindControl("VenueCheckBoxes");

                if (VenueCheckBoxes != null)
                {
                    for (int i = 0; i < VenueCheckBoxes.Items.Count; i++)
                    {
                        if (VenueCheckBoxes.Items[i].Selected)
                        {
                            cmd = new SqlCommand("INSERT INTO UserVenues (UserID, VenueID) VALUES(@user, @cat)", conn);
                            cmd.Parameters.Add("@user", SqlDbType.Int).Value = USER_ID;
                            cmd.Parameters.Add("@cat", SqlDbType.Int).Value = VenueCheckBoxes.Items[i].Value;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }


                //If reset is set, reset the user ads
                if (resetAds)
                {
                    //DataView dvprevads = d.GetDataDV("SELECT * FROM UserAds WHERE UserID=" + Session["User"].ToString() +
                    //    " AND [DATE]= '" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Date.ToShortDateString() + "'");

                    //if (dvprevads.Count != 0)
                    //{
                        d.Execute("DELETE FROM UserAds WHERE UserID=" + Session["User"].ToString() + 
                            " AND BigAd='True' AND [Date] = '" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Date.ToString() + "'");
                    //}
                }

                conn.Close();
                Encryption encrypt = new Encryption();
                Session["Message"] = "Your profile has been updated!";

                //MessageLiteral.Text = "<script type=\"text/javascript\">alert('" + message + "');</script>";

                //MessageRadWindow.NavigateUrl = "Message.aspx?message=" + 
                //    encrypt.encrypt(Session["Message"].ToString() + 
                //    "<br/><br/><br/><img onclick=\"Search('User.aspx');\" onmouseover=\"this.src='image/DoneSonButtonSelected.png'\" onmouseout=\"this.src='image/DoneSonButton.png'\" src=\"image/DoneSonButton.png\"/>");
                //MessageRadWindow.Visible = true;
                //MessageRadWindowManager.VisibleOnPageLoad = true;

               
            }
            else
            {
                ErrorLabel.Text = "Email is invalid";
                Encryption encrypt = new Encryption();
                Session["Message"] = "Email is invalid";

                //MessageLiteral.Text = "<script type=\"text/javascript\">alert('" + message + "');</script>";

                //MessageRadWindow.Title = "Invalid Email";
                //MessageRadWindow.NavigateUrl = "Message.aspx?message=" + encrypt.encrypt(Session["Message"].ToString() + "<br/><img onclick=\"Search('Home.aspx');\" onmouseover=\"this.src='image/DoneSonButtonSelected.png'\" onmouseout=\"this.src='image/DoneSonButton.png'\" src=\"image/DoneSonButton.png\"/>");
                //MessageRadWindow.Visible = true;
                //MessageRadWindowManager.VisibleOnPageLoad = true;
            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString();
        }

        //RecomUpdate.Update();
    }

    protected void CreateCategories(ref bool resetAds)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        string categories = "";
        string message = "";
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        Telerik.Web.UI.RadTreeView CategoryTree = (Telerik.Web.UI.RadTreeView)AdCategoryRadPanel.Items[0].Items[0].FindControl("CategoryTree");
        Telerik.Web.UI.RadTreeView RadTreeView2 = (Telerik.Web.UI.RadTreeView)AdCategoryRadPanel.Items[0].Items[0].FindControl("RadTreeView2");
        Telerik.Web.UI.RadTreeView RadTreeView1 = (Telerik.Web.UI.RadTreeView)EventPanelBar.Items[0].Items[0].FindControl("RadTreeView1");
        Telerik.Web.UI.RadTreeView RadTreeView3 = (Telerik.Web.UI.RadTreeView)EventPanelBar.Items[0].Items[0].FindControl("RadTreeView3");

        //First delete user's Ad Categories and Event categories
        dat.Execute("DELETE FROM UserEventCategories WHERE UserID=" + Session["User"].ToString());
        dat.Execute("DELETE FROM UserCategories WHERE UserID=" + Session["User"].ToString());

        //Second add the categories back into the user's account
        GetCategoriesFromTree(ref CategoryTree, true, ref resetAds);
        GetCategoriesFromTree(ref RadTreeView1, false, ref resetAds);
        GetCategoriesFromTree(ref RadTreeView2, true, ref resetAds);
        GetCategoriesFromTree(ref RadTreeView3, false, ref resetAds);
    }

    protected void GetCategoriesFromTree(ref Telerik.Web.UI.RadTreeView CategoryTree, bool isAd, ref bool resetAds)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        string categories = "";
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        List<Telerik.Web.UI.RadTreeNode> list = (List<Telerik.Web.UI.RadTreeNode>)CategoryTree.GetAllNodes();

        foreach (Telerik.Web.UI.RadTreeNode node in list)
        {
            DataView dv = dat.GetDataDV("SELECT * FROM UserCategories WHERE CategoryID=" + node.Value +
                " AND UserID=" + Session["User"].ToString());
            if (node.Checked)
            {
                if (isAd)
                {
                    dat.Execute("INSERT INTO UserCategories (CategoryID, UserID) VALUES("
                                + node.Value + "," + Session["User"].ToString() + ")");

                    if (dv.Count == 0)
                        resetAds = true;
                }
                else
                {
                    dat.Execute("INSERT INTO UserEventCategories (CategoryID, UserID) VALUES("
                                + node.Value + "," + Session["User"].ToString() + ")");

                }
            }
            else
            {
                if (isAd)
                {
                    if (dv.Count != 0)
                        resetAds = true;
                }
            }
        }
    }

    protected void ChangeState(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet ds = dat.GetData("SELECT * FROM State WHERE country_id=" + CountryDropDown.SelectedValue);

        bool isTextBox = false;
        if (ds.Tables.Count > 0)
            if (ds.Tables[0].Rows.Count > 0)
            {
                StateDropDownPanel.Visible = true;
                StateTextBoxPanel.Visible = false;
                StateDropDown.DataSource = ds;
                StateDropDown.DataTextField = "state_2_code";
                StateDropDown.DataValueField = "state_id";
                StateDropDown.DataBind();
            }
            else
                isTextBox = true;
        else
            isTextBox = true;

        if (isTextBox)
        {
            StateTextBoxPanel.Visible = true;
            StateDropDownPanel.Visible = false;
        }

        if (CountryDropDown.SelectedValue != "223")
        {
            RadiusPanel.Visible = false;
        }
        else
        {
            RadiusPanel.Visible = true;
        }
    }

    protected void ChangeBillState(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet ds = dat.GetData("SELECT * FROM State WHERE country_id=" + BillCountryDropDown.SelectedValue);

        bool isTextBox = false;
        if (ds.Tables.Count > 0)
            if (ds.Tables[0].Rows.Count > 0)
            {
                BillStateDropPanel.Visible = true;
                BillStateTextPanel.Visible = false;
                BillStateDropDown.DataSource = ds;
                BillStateDropDown.DataTextField = "state_2_code";
                BillStateDropDown.DataValueField = "state_id";
                BillStateDropDown.DataBind();
            }
            else
                isTextBox = true;
        else
            isTextBox = true;

        if (isTextBox)
        {
            BillStateTextPanel.Visible = true;
            BillStateDropPanel.Visible = false;
        }
    }

        private static ImageCodecInfo GetEncoderInfo(string mimeType)
    {
        // Get image codecs for all image formats 
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

        // Find the correct image codec
        ImageCodecInfo a = codecs[0];
        for (int i = 0; i < codecs.Length; i++)
        {
            if (codecs[i].MimeType == mimeType)
            {
                return codecs[i];

            }
            if (codecs[i].MimeType == "jpeg")
                a = codecs[i];
        }
        return a;
    }

    private void SaveThumbnail(System.Drawing.Image theimage, bool isRotator, string path, string typeS)
    {
        double width = double.Parse(theimage.Width.ToString());
        double height = double.Parse(theimage.Height.ToString());

        if (width > height)
        {
            if (width <= 150)
            {

            }
            else
            {
                double dividor = double.Parse("150.00") / double.Parse(width.ToString());
                width = double.Parse("150.00");
                height = height * dividor;
            }
        }
        else
        {
            if (width == height)
            {
                width = double.Parse("150.00");
                height = double.Parse("150.00");
            }
            else
            {
                double dividor = double.Parse("150.00") / double.Parse(height.ToString());
                height = double.Parse("150.00");
                width = width * dividor;
            }
        }

        int w = int.Parse((Math.Round(decimal.Parse(width.ToString()))).ToString());
        int h = int.Parse((Math.Round(decimal.Parse(height.ToString()))).ToString());

        System.Drawing.Bitmap bmpResized = new System.Drawing.Bitmap(theimage, w, h);


        bmpResized.Save(path);
    }

    public static void SaveJpeg(string path, System.Drawing.Image img, int quality, string typeS)
    {
        if (quality < 0 || quality > 100)
            throw new ArgumentOutOfRangeException("quality must be between 0 and 100.");


        // Encoder parameter for image quality 
        EncoderParameter qualityParam =
            new EncoderParameter(Encoder.Quality, quality);
        // Jpeg image codec 
        ImageCodecInfo jpegCodec = GetEncoderInfo(typeS);

        EncoderParameters encoderParams = new EncoderParameters(1);
        encoderParams.Param[0] = qualityParam;

        img.Save(path, jpegCodec, encoderParams);
    }

    private bool EmptyCallBack()
    {
        return false;
    }

    protected void AcceptMembership(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        HtmlButton link = (HtmlButton)sender;
        string groupID = link.ID.Replace("acceptMembership", "");

        dat.Execute("UPDATE Group_Members SET Accepted='True' WHERE GroupID=" + groupID + 
            " AND MemberID=" + Session["User"].ToString());
    }

    protected void ApproveMembership(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        HtmlButton link = (HtmlButton)sender;
        string [] delim = {"acceptMembership"};
        string [] tokens = link.ID.Split(delim, StringSplitOptions.RemoveEmptyEntries);
        string groupID = tokens[1];
        string userID = tokens[0];

        DataView dvExists = dat.GetDataDV("SELECT * FROM Group_Members WHERE MemberID=" + UserID + " AND GroupID=" + groupID);

        if (dvExists.Count == 0)
            dat.Execute("INSERT INTO Group_Members (Accepted, MemberID, GroupID) VALUES('True', " + userID +
                ", " + groupID + ")");
        else
            dat.Execute("UPDATE Group_Members SET Accepted='True' WHERE MemberID=" + userID +
                " AND GroupID = " + groupID);

        DataView dvU = dat.GetDataDV("SELECT * FROM Users WHERE User_ID=" + userID);
        DataView dvG = dat.GetDataDV("SELECT * FROM Groups WHERE ID=" + groupID);

        string content = "Your membership for group '"+dvG[0]["Header"].ToString()+"' has been approved. <br/><br/>"+
            "You can find this group <a target=\"_blank\" href=\"http://hippohappenings.com/Group.aspx?ID="+
            groupID+"\">here</a>. We also recommend that you set your email settings for this group "+
            "<a href=\"http://hippohappenings.com/User.aspx?G=true#groups\">here</a>.<br/><br/> Have fun!";

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());

        conn.Open();
        SqlCommand cmd = new SqlCommand("INSERT INTO UserMessages (MessageContent, MessageSubject, " +
            "From_UserID, To_UserID, Date, [Read], Mode)"
            + " VALUES(@content, @subject, @fromID, @toID, @date, 'False', 0)", conn);
        cmd.Parameters.Add("@content", SqlDbType.Text).Value = content;
        cmd.Parameters.Add("@subject", SqlDbType.NVarChar).Value = "Your Request for Membership has been Approved.";
        cmd.Parameters.Add("@toID", SqlDbType.Int).Value = userID;
        cmd.Parameters.Add("@fromID", SqlDbType.Int).Value = Session["User"].ToString();
        cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
        cmd.ExecuteNonQuery();

        dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"],
            System.Configuration.ConfigurationManager.AppSettings["emailName"],
            dvU[0]["Email"].ToString(), content , "Your Request for Membership has been Approved.");

    }

    protected void AcceptInvitation(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        HtmlButton link = (HtmlButton)sender;
        string[] delim = { "acceptInvitation" };
        string[] tokens = link.ID.Split(delim, StringSplitOptions.RemoveEmptyEntries);
        string groupID = tokens[0];
        string reoccurrID = tokens[1];

        if (!HasRegistrationEnded(groupID, reoccurrID))
        {

            dat.Execute("UPDATE GroupEvent_Members SET Accepted='True' WHERE GroupEventID=" + groupID +
                " AND UserID=" + Session["User"].ToString() + " AND ReoccurrID=" + reoccurrID);
        }
    }
}
