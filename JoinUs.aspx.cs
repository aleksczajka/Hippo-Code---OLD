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

public partial class JoinUs : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        HtmlMeta hm = new HtmlMeta();
        HtmlHead head = (HtmlHead)Page.Header;
        hm.Name = "ROBOTS";
        hm.Content = "NOINDEX, FOLLOW";
        head.Controls.AddAt(0, hm);

        ImageButton9.OnClientClick = "javascript:Search('Group.aspx?ID=" +
            Request.QueryString["ID"].ToString() + "');";

        DataView dvGroup = dat.GetDataDV("SELECT * FROM Groups WHERE AutomaticAccept='True' "+
            "AND ID=" + Request.QueryString["ID"]);

        if (Session["User"] == null)
        {
            RemovePanel.Visible = false;
            NotSignedInPanel.Visible = true;
            AutomaticPanel.Visible = false;
        }
        else
        {
            
            if (dvGroup.Count == 0)
            {

                NotSignedInPanel.Visible = false;
                RemovePanel.Visible = true;
                AutomaticPanel.Visible = false;
            }
            else
            {
                ThankYouLabel.Text = "You have joined the group.";
                AutomaticPanel.Visible = true;
                NotSignedInPanel.Visible = false;
                RemovePanel.Visible = false;
            }
        }

        
    }

    protected void AutomaticJoin(object sender, EventArgs e)
    {
        try
        {
            HttpCookie cookie = Request.Cookies["BrowserDate"];
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            string shared = "False";
            string title = "";
            string prefs = "124";
            DataView dvGroup = dat.GetDataDV("SELECT * FROM Groups WHERE AutomaticHost='True' " +
                "AND ID=" + Request.QueryString["ID"]);

            if (dvGroup.Count > 0)
            {
                shared = "True";
                title = "Shared Host";
                prefs = "12345";
            }

            dat.Execute("INSERT INTO Group_Members (GroupID, MemberID, Title, SharedHosting, Accepted, Prefs) " +
                "VALUES(" + Request.QueryString["ID"] + ", " + Session["User"].ToString() +
                ", '" + title + "', '" + shared + "', 'True', " + prefs + ")");

            AutomaticPanel.Visible = false;
            ThankYouPanel.Visible = true;
        }
        catch (Exception ex)
        {
            Label1.Text = ex.ToString();
        }
    }

    protected void PostThread(object sender, EventArgs e)
    {
        string command = "";
        try
        {
            if (SubjectTextBox.Text.Trim() != "")
            {
                string GroupID = Request.QueryString["ID"].ToString();
                HttpCookie cookie = Request.Cookies["BrowserDate"];
                Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

                SqlConnection conn = dat.GET_CONNECTED;
                SqlCommand cmd;

                DataView dv = dat.GetDataDV("SELECT * FROM Groups WHERE ID=" + GroupID);

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd = new SqlCommand("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, To_UserID, " +
                    "[Date], [Read], [Mode], [Live]) VALUES(@content, @subject, @from, @to, @date, 'False', 8, 'True')", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@content", SqlDbType.NVarChar).Value = GroupID + " " + SubjectTextBox.Text.Trim();
                cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
                cmd.Parameters.Add("@from", SqlDbType.Int).Value = Session["User"].ToString();
                cmd.Parameters.Add("@to", SqlDbType.Int).Value = dv[0]["Host"].ToString();
                cmd.Parameters.Add("@subject", SqlDbType.NVarChar).Value = "Request to join group '" + dv[0]["Header"].ToString().Replace("'", "''") + "'";
                cmd.ExecuteNonQuery();

                DataView dvUsers = dat.GetDataDV("SELECT * FROM Group_Members GM, Users U WHERE GM.MemberID=" +
                    "U.User_ID AND U.User_ID=" + dv[0]["Host"].ToString() + " AND GM.GroupID=" +
                    GroupID + " AND Prefs LIKE '%3%'");

                if (dvUsers.Count > 0)
                {
                    string email = "Someone requested to join the group '" +
                    dv[0]["Header"].ToString() + "'. To view the request please log into <a href=\"http://hippohappenings.com/my-account\">your account</a>.";
                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(), 
                    dvUsers[0]["Email"].ToString(),
                    email, "A request to join your group");
                }

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

    protected void GoSign(object sender, EventArgs e)
    {
        Response.Redirect("login");
    }
}
