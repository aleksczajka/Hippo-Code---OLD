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

public partial class Controls_Comments : System.Web.UI.UserControl
{
    public int CUT_OFF
    {
        get { return cutOff; }
        set { cutOff = value; }
    }
    private int cutOff = 0;
    public int THE_ID
    {
        get { return theID; }
        set { theID = value; }
    }
    public DataSet DATA_SET
    {
        get { return ds; }
        set { ds = value; }
    }
    private int theID = -1;
    private DataSet ds;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["LastPostedComment"] = null;
            Session.Remove("LastPostedComment");
        }
    }

    public void DataBind2(bool isEvent)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        
        try
        {
           
            if (Session["User"] != null)
            {
                DataSet ds1 = dat.GetData("SELECT UserName FROM Users WHERE User_ID=" + Session["User"].ToString());
                Session["UserName"] = ds1.Tables[0].Rows[0]["UserName"].ToString();
                GotSayPanel.Visible = true;
            }
            else
            {
                RegisterPanel.Visible = true;
                GotSayPanel.Visible = false;
            }


            if (ds != null)
            {
                DataView dv = new DataView(ds.Tables[0], "", "", DataViewRowState.CurrentRows);
                dv.Sort = "theDate DESC";
                if (ds.Tables.Count > 0)
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        CommentsPanel.Controls.Clear();
                        CommentsTitlePanel.Visible = true;
                        int rowCount = ds.Tables[0].Rows.Count;
                        if (rowCount > cutOff && cutOff != 0)
                            rowCount = cutOff;
                        for (int i = 0; i < rowCount; i++)
                        {
                            ASP.controls_comment_ascx comment = new ASP.controls_comment_ascx();
                            comment.ID = "Comment" + i.ToString();
                            if (dv[i]["ProfilePicture"] != DBNull.Value)
                                comment.PROFILE_THUMB = "~/UserFiles/" + dv[i]["UserName"].ToString() + "/Profile/" + dv[i]["ProfilePicture"].ToString();
                            else
                                comment.PROFILE_THUMB = "~/image/noAvatar_50x50_small.png";

                            if ((i % 2) == 0)
                                comment.IS_LEFT = true;
                            else
                                comment.IS_LEFT = false;
                            comment.COMMENTER_ID = int.Parse(dv[i]["User_ID"].ToString());
                            comment.USER_LABEL = dv[i]["UserName"].ToString();
                            comment.DATE_LABEL = dv[i]["theDate"].ToString();
                            comment.IS_EVENT = isEvent;
                            string theComment = dv[i]["Comment"].ToString();
                            //if (theComment.Length > 300)
                            //    theComment = theComment.Substring(0, 300) + "...";
                            comment.COMMENT = theComment;
                            CommentsPanel.Controls.Add(comment);
                        }

                        //if (Request.Url.Segments[Request.Url.Segments.Length - 1] == "Home.aspx")
                        //{
                        //    if (ds.Tables[0].Rows.Count > rowCount)
                        //    {
                        //        ToSeePanel.Visible = true;
                        //        DataSet dsHome = dat.GetData("SELECT * FROM Events WHERE Featured='True'");
                        //        string ID = dsHome.Tables[0].Rows[0]["ID"].ToString();
                        //        PageLink.NavigateUrl = "~/Event.aspx?EventID=" + ID;
                        //    }
                        //}
                    }
                    else
                        CommentsTitlePanel.Visible = false;
                else
                    CommentsTitlePanel.Visible = false;

            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void PostIt(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        bool goOn = false;
        if (Session["LastPostedComment"] == null)
            goOn = true;
        else if (DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")) > ((DateTime)Session["LastPostedComment"]).AddSeconds(double.Parse("2.00")))
            goOn = true;

        if (goOn)
        {
            Session["LastPostedComment"] = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Context.Request.Cookies[cookieName];

            FormsAuthenticationTicket authTicket = null;
            try
            {
                string group = "";
                if (authCookie != null)
                {
                    authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    group = authTicket.UserData.ToString();
                }

                if (group.Contains("User"))
                {
                    Session["User"] = authTicket.Name;
                    DataSet ds1 = dat.GetData("SELECT UserName FROM Users WHERE User_ID=" + authTicket.Name);
                    Session["UserName"] = ds1.Tables[0].Rows[0]["UserName"].ToString();
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
            }

            if (CommentTextBox.Text.Trim() != "")
            {


                string urlToken = Request.Url.Segments[Request.Url.Segments.Length - 1];
                DataSet dsComments;
                SqlCommand cmd = new SqlCommand();
                string commentPrefs = "";
                CommentTextBox.Text = dat.StripHTML_LeaveLinks(CommentTextBox.Text);
                switch (urlToken)
                {


                    case "Venue.aspx":
                        cmd.CommandText = "INSERT INTO Venue_Comments (Comment, UserID, VenueID, CommentDate) "
                        + "VALUES(@comment, @userID, @venueID, @date)";
                        cmd.Connection = dat.GET_CONNECTED;
                        cmd.Parameters.Add("@comment", SqlDbType.NVarChar).Value = CommentTextBox.Text;
                        cmd.Parameters.Add("@userID", SqlDbType.Int).Value = int.Parse(Session["User"].ToString());
                        cmd.Parameters.Add("@venueID", SqlDbType.Int).Value = int.Parse(Request.QueryString["ID"].ToString());
                        cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
                        cmd.ExecuteNonQuery();
                        CommentTextBox.Text = "";

                        commentPrefs = dat.GetData("SELECT * FROM UserPreferences WHERE UserID=" + Session["User"].ToString()).Tables[0].Rows[0]["CommentsPreferences"].ToString();
                        if (commentPrefs == "1")
                        {
                            dsComments = dat.GetData("SELECT VC.CommentDate AS theDate, * FROM Venue_Comments VC, Users U WHERE VC.UserID=U.User_ID AND VC.VenueID=" + Request.QueryString["ID"].ToString() + " ORDER BY VC.CommentDate ");
                        }
                        else
                        {
                            dsComments = dat.GetData("SELECT DISTINCT U.ProfilePicture, U.User_ID, U.UserName, VC.Comment, VC.CommentDate AS theDate FROM Venue_Comments VC, Users U, User_Friends UF WHERE ((UF.UserID=" + Session["User"].ToString() + " AND UF.FriendID=U.User_ID AND U.User_ID=VC.UserID) OR (U.User_ID=" +
                                                Session["User"].ToString() + " AND U.User_ID=VC.UserID)) AND VC.VenueID=" + Request.QueryString["ID"].ToString() + " ORDER BY VC.CommentDate");
                        } this.DATA_SET = dsComments;
                        DataBind2(false);
                        break;
                    case "Event.aspx":
                        cmd.CommandText = "INSERT INTO Comments (BlogID, Comment, UserID, BlogDate) VALUES"
                        + "(@blogID, @comment, @userID, @date)";
                        cmd.Connection = dat.GET_CONNECTED;
                        int eventID = int.Parse(Request.QueryString["EventID"].ToString());
                        cmd.Parameters.Add("@blogID", SqlDbType.Int).Value = eventID;
                        cmd.Parameters.Add("@comment", SqlDbType.NVarChar).Value = CommentTextBox.Text;
                        cmd.Parameters.Add("@userID", SqlDbType.Int).Value = int.Parse(Session["User"].ToString());
                        cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
                        cmd.ExecuteNonQuery();

                        commentPrefs = dat.GetData("SELECT * FROM UserPreferences WHERE UserID=" + Session["User"].ToString()).Tables[0].Rows[0]["CommentsPreferences"].ToString();
                        if (commentPrefs == "1")
                        {
                            dsComments = dat.GetData("SELECT C.BlogDate AS theDate, * FROM Comments C, Users U WHERE U.User_ID=C.UserID AND C.BlogID=" + Request.QueryString["EventID"].ToString() + " ORDER BY C.BlogDate");
                        }
                        else
                        {
                            dsComments = dat.GetData("SELECT DISTINCT U.ProfilePicture, U.User_ID, U.UserName, C.Comment, C.BlogDate AS theDate FROM Comments C, Users U, User_Friends UF WHERE ((UF.UserID=" + Session["User"].ToString() + " AND UF.FriendID=U.User_ID AND U.User_ID=C.UserID) OR (U.User_ID=" +
                                Session["User"].ToString() + " AND U.User_ID=C.UserID)) AND C.BlogID=" + Request.QueryString["EventID"].ToString() + " ORDER BY C.BlogDate");
                        } CommentTextBox.Text = "";
                        this.DATA_SET = dsComments;
                        DataBind2(true);
                        break;
                    case "Home.aspx":
                        DataSet ds = dat.GetData("SELECT * FROM Events WHERE Featured='True'");
                        string ID2 = ds.Tables[0].Rows[0]["EventID"].ToString();
                        cmd.CommandText = "INSERT INTO Comments (BlogID, Comment, UserID, BlogDate) VALUES"
                        + "(@blogID, @comment, @userID, @date)";
                        cmd.Connection = dat.GET_CONNECTED;
                        cmd.Parameters.Add("@blogID", SqlDbType.Int).Value = int.Parse(ID2);
                        cmd.Parameters.Add("@comment", SqlDbType.NVarChar).Value = CommentTextBox.Text;
                        cmd.Parameters.Add("@userID", SqlDbType.Int).Value = int.Parse(Session["User"].ToString());
                        cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
                        cmd.ExecuteNonQuery();

                        commentPrefs = dat.GetData("SELECT * FROM UserPreferences WHERE UserID=" + Session["User"].ToString()).Tables[0].Rows[0]["CommentsPreferences"].ToString();
                        if (commentPrefs == "1")
                        {
                            dsComments = dat.GetData("SELECT C.BlogDate AS theDate, * FROM Comments C, Users U WHERE U.User_ID=C.UserID AND C.BlogID=" + Request.QueryString["EventID"].ToString() + " ORDER BY C.BlogDate");
                        }
                        else
                        {
                            dsComments = dat.GetData("SELECT DISTINCT U.ProfilePicture, U.User_ID, U.UserName, C.Comment, C.BlogDate AS theDate FROM Comments C, Users U, User_Friends UF WHERE ((UF.UserID=" + Session["User"].ToString() + " AND UF.FriendID=U.User_ID AND U.User_ID=C.UserID) OR (U.User_ID=" +
                                Session["User"].ToString() + " AND U.User_ID=C.UserID)) AND C.BlogID=" + Request.QueryString["EventID"].ToString() + " ORDER BY C.BlogDate");
                        } CommentTextBox.Text = "";
                        this.DATA_SET = dsComments;
                        DataBind2(true);
                        break;
                    default: break;
                }


            }
        }
    }
}
