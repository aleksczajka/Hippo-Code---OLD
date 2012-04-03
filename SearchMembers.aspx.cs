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

public partial class SearchMembers : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            if (Session["SearchDS"] != null)
            {
                DataSet ds = (DataSet)Session["SearchDS"];
                if(ds.Tables.Count > 0)
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        FillFriends(ds);
                    }
            }
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
    }

    //[Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
    //public string SendIt(string friendID)
    //{
    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

    //    try
    //    {
    //        SqlConnection conn = dat.GET_CONNECTED;
    //        SqlCommand cmd = new SqlCommand("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, To_UserID, Date, [Read], Mode)"
    //            + " VALUES(@content, @subject, @fromID, @toID, @date, 'False', 2)", conn);
    //        cmd.Parameters.Add("@content", SqlDbType.Text).Value = Session["UserName"].ToString() +" would like to extend a Hippo Happ friend invitation to you. You can find out about this user <a target=\"_blank\" class=\"AddLink\" href=\"Friend.aspx?ID="+Session["User"].ToString()+"\">here</a>. To accept this Hippo user as a friend, click on the link below.";
    //        cmd.Parameters.Add("@subject", SqlDbType.NVarChar).Value = "Hippo Happs Friend Request!";
    //        cmd.Parameters.Add("@toID", SqlDbType.Int).Value = friendID;
    //        cmd.Parameters.Add("@fromID", SqlDbType.Int).Value = Session["User"].ToString();
    //        cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
    //        cmd.ExecuteNonQuery();
    //        conn.Close();

            

    //        return "stuff success";
    //    }
    //    catch (Exception ex)
    //    {
            
    //        return ex.ToString();
    //    }
        
    //}

    //protected void SendIt(object sender, EventArgs e)
    //{
    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

    //    try
    //    {
    //        SqlConnection conn = dat.GET_CONNECTED;
    //        SqlCommand cmd = new SqlCommand("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, To_UserID, Date, [Read], Mode)"
    //            + " VALUES(@content, @subject, @fromID, @toID, @date, 'False', 2)", conn);
    //        cmd.Parameters.Add("@content", SqlDbType.Text).Value = Session["UserName"].ToString() + 
    //            " would like to extend a Hippo Happ friend invitation to you. You can find out about this user <a target=\"_blank\" class=\"AddLink\" href=\"Friend.aspx?ID=" + Session["User"].ToString() + "\">here</a>. To accept this Hippo user as a friend, click on the link below.";
    //        cmd.Parameters.Add("@subject", SqlDbType.NVarChar).Value = "Hippo Happs Friend Request!";
    //        cmd.Parameters.Add("@toID", SqlDbType.Int).Value = friendID;
    //        cmd.Parameters.Add("@fromID", SqlDbType.Int).Value = Session["User"].ToString();
    //        cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
    //        cmd.ExecuteNonQuery();
    //        conn.Close();

    //    }
    //    catch (Exception ex)
    //    {

 
    //    }

    //}

    protected void Search(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        char[] delim = { ' ' };
        string[] tokens = SearchTextBox.Text.Split(delim);

        string query = "SELECT DISTINCT UserName, ProfilePicture, FirstName, LastName, User_ID FROM USERS WHERE User_ID <> " + 
            Session["User"].ToString() + " AND ";
        SqlDbType[] types = new SqlDbType[tokens.Length];
        for (int i = 0; i < tokens.Length; i++)
        {
            types[i] = SqlDbType.NVarChar;
            if (i != 0)
                query += " OR ";
            query += " ( FirstName LIKE '%'+@p" + i.ToString() + "+'%' OR LastName LIKE '%'+@p" +
                i.ToString() + "+'%' OR UserName LIKE '%'+@p" + i.ToString() + "+'%' OR Email LIKE '%'+@p" + i.ToString() + "+'%' ) ";
        }

        DataSet ds = dat.GetDataWithParemeters(query, types, tokens);
        Session["SearchDS"] = ds;
        FillFriends(ds);
    }

    private void FillFriends(DataSet ds)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        try
        {
            FriendsPanel.Visible = true;
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            bool noFriends = true;

            FriendsPanel.Controls.Clear();
            if (ds.Tables.Count > 0)
                if (ds.Tables[0].Rows.Count > 0)
                {


                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        noFriends = false;
                        Literal lit = new Literal();
                        lit.Text = "<div style=\"border: solid 1px #cccccc; margin: 3px;\"><table><tr><td><a target=\"_blank\" href=\"/" +
                            ds.Tables[0].Rows[i]["UserName"].ToString() +
                            "_Friend\"><img style=\"border: 0;\" alt=\"" +
                            ds.Tables[0].Rows[i]["UserName"].ToString()
                            + "\" name=\"" + ds.Tables[0].Rows[i]["UserName"].ToString() +
                            "\" class=\"FriendPic\" width=\"50px\" height=\"50px\" src=\"";

                        if (System.IO.File.Exists(Server.MapPath(".") + "\\UserFiles\\" + ds.Tables[0].Rows[i]["UserName"].ToString() + "\\Profile\\" + ds.Tables[0].Rows[i]["ProfilePicture"].ToString()))
                        {
                            lit.Text += "UserFiles/" + ds.Tables[0].Rows[i]["UserName"].ToString() + "/Profile/" + ds.Tables[0].Rows[i]["ProfilePicture"].ToString() + "\"";
                        }
                        else
                        {
                            lit.Text += "image/noAvatar_50x50_small.png\" onmouseout=\"this.src='image/noAvatar_50x50_small.png'\" onmouseover=\"this.src='NewImages/noAvatar_50x50_smallhover.png'\" ";

                        }

                        string realName = "";

                        if (ds.Tables[0].Rows[i]["LastName"] != null && ds.Tables[0].Rows[i]["FirstName"] != null&&
                            ds.Tables[0].Rows[i]["LastName"].ToString().Trim() != "" && 
                            ds.Tables[0].Rows[i]["FirstName"].ToString().Trim() != "")
                            realName = ", "+ds.Tables[0].Rows[i]["FirstName"].ToString() + " " + 
                                ds.Tables[0].Rows[i]["LastName"].ToString();

                        lit.Text += " /></td><td><label>" + ds.Tables[0].Rows[i]["UserName"].ToString() + realName + "</label></td>";

                      
                        bool isFriend = false;
                        if (isFriend)
                        {
                            lit.Text += "<td> | <div style=\"display: inline;\" class=\"AddGreenLink\" id=\"div" +
                                ds.Tables[0].Rows[i]["User_ID"].ToString() + "\">This user is your friend!</div></td></tr></table></div>";
                            FriendsPanel.Controls.Add(lit);
                        }
                        else
                        {
                            bool isSent = false;
                           
                            if (isSent)
                            {
                                lit.Text += "<td> | <div style=\"display: inline;\" class=\"AddGreenLink\" id=\"div" +
                                ds.Tables[0].Rows[i]["User_ID"].ToString() + "\">Friend request sent.</div></td></tr></table></div>";
                                FriendsPanel.Controls.Add(lit);
                            }
                            else
                            {
                                lit.Text += "<td> | ";
                                FriendsPanel.Controls.Add(lit);

                                LinkButton butt = new LinkButton();
                                butt.ID = "link" + ds.Tables[0].Rows[i]["User_ID"].ToString();
                                butt.Text = "Invite";
                                butt.CssClass = "AddLink";
                                butt.Click += new EventHandler(AddFriend_Click);
                                butt.OnClientClick = "getScroll();";
                                butt.CommandArgument = ds.Tables[0].Rows[i]["User_ID"].ToString();

                                FriendsPanel.Controls.Add(butt);

                                Literal lit2 = new Literal();
                                lit2.Text = "</td></tr></table></div>";


                                FriendsPanel.Controls.Add(lit2);
                            }
                        }

                    }
                }
            
            if (noFriends)
            {
                Literal lit = new Literal();
                lit.Text = "<label>No friends found.</label>";
                FriendsPanel.Controls.Add(lit);
            }
        }
        catch (Exception ex)
        {
            Literal lit = new Literal();
            lit.Text = ex.ToString();
            FriendsPanel.Controls.Add(lit);
        }
    }

    protected void AddFriend_Click(object sender, EventArgs e)
    {
        LinkButton butt = (LinkButton)sender;
        Session["SelectedMembers"] += butt.CommandArgument + ";";

        int theIndex = FriendsPanel.Controls.IndexOf(butt);
        FriendsPanel.Controls.Remove(butt);


        Literal tex = new Literal();
        tex.Text = "<div class=\"AddGreenLink\"  style=\"display: inline;\">Member Invited</div>";

        FriendsPanel.Controls.AddAt(theIndex, tex);
    }
}
