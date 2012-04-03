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

public partial class PostMemberComment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlMeta hm = new HtmlMeta();
        HtmlHead head = (HtmlHead)Page.Header;
        hm.Name = "ROBOTS";
        hm.Content = "NOINDEX, FOLLOW";
        head.Controls.AddAt(0, hm);

        ImageButton9.OnClientClick = "javascript:Search('Group.aspx?ID=" +
            Request.QueryString["ID"].ToString() + "');";

        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        string groupID = Request.QueryString["ID"].ToString();

        DataView dvMembers = dat.GetDataDV("SELECT * FROM Group_Members WHERE GroupID=" +
           groupID + " AND MemberID=" + Session["User"].ToString());

        if (bool.Parse(dvMembers[0]["SharedHosting"].ToString()))
        {
            HostPanel.Visible = true;
        }
        else
            HostPanel.Visible = false;
    }

    protected void PostThread(object sender, EventArgs e)
    {
        string command = "";
        try
        {
            if (SubjectTextBox.Text.Trim() != "")
            {
                string theID = Request.QueryString["ID"].ToString();

                HttpCookie cookie = Request.Cookies["BrowserDate"];
                Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

                SqlConnection conn = dat.GET_CONNECTED;
                SqlCommand cmd;

                cmd = new SqlCommand("INSERT INTO GroupMessages ([Content], UserID, GroupID, isSticky, DatePosted)" +
                    " VALUES(@content, @userID, @groupID, @sticky, GETDATE())", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@content", SqlDbType.NVarChar).Value = SubjectTextBox.Text.Trim();
                cmd.Parameters.Add("@userID", SqlDbType.Int).Value = Session["User"].ToString();
                cmd.Parameters.Add("@groupID", SqlDbType.Int).Value = Request.QueryString["ID"].ToString();
                cmd.Parameters.Add("@sticky", SqlDbType.Bit).Value = StickyCheckBox.Checked;
                cmd.ExecuteNonQuery();

                SendCommentNotifications(theID);

                RemovePanel.Visible = false;
                ThankYouPanel.Visible = true;

            }
            else
            {
                ErrorLabel.Text = "Please include the message.";
            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString() + "<br/><br/>" + command;
        }
    }

    protected void SendCommentNotifications(string threadID)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        DataView dvUsers = dat.GetDataDV("SELECT * FROM Group_Members GM, Users U WHERE GM.MemberID=" +
            "U.User_ID AND GM.GroupID=" + threadID + " AND Prefs LIKE '%4%'");
        DataView dvGroup = dat.GetDataDV("SELECT * FROM Groups WHERE ID=" + threadID);
        string email = "A new message has been posted on the message board for group '" +
            dvGroup[0]["Header"].ToString() + "'. <a href=\"http://hippohappenings.com/" +
            dat.MakeNiceName(dvGroup[0]["Header"].ToString()) + "_" + threadID +
            "_Group\">Check it out.</a>";
        foreach (DataRowView row in dvUsers)
        {
            if (Session["User"].ToString() != row["User_ID"].ToString())
            {
                dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(), row["Email"].ToString(),
                email, "A new group message has been posted");
            }
        }
    }
}
