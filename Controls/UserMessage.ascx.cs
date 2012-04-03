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

public partial class Controls_UserMessage : System.Web.UI.UserControl
{
    public enum Mode { Private, HippoRequest, HippoReply };
    public string MESSAGE_TEXT
    {
        get { return messageText; }
        set { messageText = value; }
    }
    public string SUBJECT_TEXT
    {
        get { return subjectText; }
        set { subjectText = value; }
    }
    public string USER_NAME
    {
        get { return username; }
        set { username = value; }
    }
    public string DATE
    {
        get { return date; }
        set { date = value; }
    }
    public int FROM_ID
    {
        get { return From_ID; }
        set { From_ID = value; }
    }
    public int TO_ID
    {
        get { return To_ID; }
        set { To_ID = value; }
    }
    public int MESSAGE_ID
    {
        get { return message_ID; }
        set { message_ID = value; }
    }
    public int NUM_IN_RAD_PANEL
    {
        get { return numInRadPanel; }
        set { numInRadPanel = value; }
    }
    public int CONTROL_ID
    {
        get { return controlID; }
        set { controlID = value; }
    }
    public Mode MODE
    {
        get { return messsageMode; }
        set { messsageMode = value; }
    }
    protected string messageText;
    protected string subjectText;
    protected string username;
    protected string date;
    protected int From_ID;
    protected int To_ID;
    protected int message_ID;
    protected int numInRadPanel;
    protected int controlID;
    protected Mode messsageMode;
    protected void Page_Init(object sender, EventArgs e)
    {
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
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
                Session["UserName"] = dat.GetData("SELECT UserName FROM USERS WHERE User_ID=" + Session["User"].ToString()).Tables[0].Rows[0]["UserName"].ToString();
            }
            else
            {
            }
        }
        catch (Exception ex)
        {
        }

        Label messageLabel = (Label)dat.FindControlRecursive(this, "MessageLabel");

        if (messsageMode == Mode.Private)
            messageLabel.Text = messageText;
        else if (messsageMode == Mode.HippoRequest)
        {
            Label label = new Label();
            label.Text = messageText;

            LinkButton link = new LinkButton();
            link.Text = "Accept This Friend";
            link.CssClass = "AddLink";
            link.CausesValidation = false;
            link.Click += new EventHandler(this.AcceptFriendReply);

            MessagePanel.Controls.Add(label);
            MessagePanel.Controls.Add(link);

            SearchBottomButton.Visible = false;
            MessageTextBox.Visible = false;
        }
        else if (messsageMode == Mode.HippoReply)
        {
            Label label = new Label();
            label.Text = messageText;

            MessagePanel.Controls.Add(label);

            SearchBottomButton.Visible = false;
            MessageTextBox.Visible = false;
        }

    }
    public delegate void EventDelegate(object from, int ID);
    public event EventDelegate myEvent;
    protected void Reply(object sender, EventArgs args)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (MessageTextBox.Text != "")
        {
            SqlConnection conn;
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, To_UserID, Date, [Read]) " +
                " VALUES(@content, @subject, @from, @to, @date, 'false')", conn);
            cmd.Parameters.Add("@content", SqlDbType.NVarChar).Value = MessageTextBox.Text;
            cmd.Parameters.Add("@subject", SqlDbType.NVarChar).Value = "Re: " + subjectText;
            cmd.Parameters.Add("@from", SqlDbType.Int).Value = To_ID;
            cmd.Parameters.Add("@to", SqlDbType.Int).Value = From_ID;
            cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
            cmd.ExecuteNonQuery();

            cmd = new SqlCommand("UPDATE UserMessages SET [Read]='True' WHERE ID = "+message_ID, conn);
            cmd.ExecuteNonQuery();

            MessageLabel2.Text = "Your message has been sent.";


            conn.Close();
            this.issueEvent(controlID);
        }
    }
    protected void AcceptFriendReply(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
            SqlConnection conn;
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
            conn.Open();

        //Add the friend.
            SqlCommand cmd = new SqlCommand("INSERT INTO User_Friends (FriendID, UserID) VALUES(@friend, @user)", conn);
            cmd.Parameters.Add("@friend", SqlDbType.Int).Value = To_ID;
            cmd.Parameters.Add("@user", SqlDbType.Int).Value = From_ID;
            cmd.ExecuteNonQuery();
        //Send message notifying the requestor that the friend has accepted
            cmd = new SqlCommand("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, To_UserID, Date, [Read], Mode) " +
                " VALUES(@content, @subject, @from, @to, @date, 'false', 2)", conn);
            cmd.Parameters.Add("@content", SqlDbType.NVarChar).Value = "Good Day from Hippo Happenings! <br/><br/> Congratulations! The friend request submited for user '" + username + "' has been approved by this user. You are now friends! ";
            cmd.Parameters.Add("@subject", SqlDbType.NVarChar).Value = "Friend Request Approved!";
            cmd.Parameters.Add("@from", SqlDbType.Int).Value = 6;
            cmd.Parameters.Add("@to", SqlDbType.Int).Value = From_ID;
            cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
            cmd.ExecuteNonQuery();

            cmd = new SqlCommand("UPDATE UserMessages SET [Read]='True' WHERE ID = " + message_ID, conn);
            cmd.ExecuteNonQuery();

            conn.Close();
            this.issueEvent(controlID);
        
    }

    //[Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
    //public int AcceptTheFriend(int friendID, int userID)
    //{
    //    try
    //    {
    //        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
           

    //        SqlConnection conn;
    //        conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
    //        conn.Open();
    //        string username = dat.GetData("SELECT UserName FROM USERS WHERE User_ID="+userID).Tables[0].Rows[0]["UserName"].ToString();
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
    //            " VALUES(@content, @subject, @from, @to, @date, 'false', 2)", conn);
    //        cmd.Parameters.Add("@content", SqlDbType.NVarChar).Value = "Good Day from Hippo Happenings! <br/><br/> Congratulations! The friend request submited for user '" + username + "' has been approved by this user. You are now friends! ";
    //        cmd.Parameters.Add("@subject", SqlDbType.NVarChar).Value = "Friend Request Approved!";
    //        cmd.Parameters.Add("@from", SqlDbType.Int).Value = 6;
    //        cmd.Parameters.Add("@to", SqlDbType.Int).Value = friendID;
    //        cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
    //        cmd.ExecuteNonQuery();

    //        cmd = new SqlCommand("UPDATE UserMessages SET [Read]='True' WHERE ID = " + message_ID, conn);
    //        cmd.ExecuteNonQuery();

    //        conn.Close();
            
    //    }
    //    catch (Exception ex)
    //    {

    //    }

    //    return 0;
    //}
    
    public void issueEvent(int id)
    {
        myEvent(this, id);
    }



    protected void ArchiveMessage(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Session["Archived"] = "True";
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        dat.Execute("UPDATE UserMessages SET Live='False' WHERE ID=" + message_ID);
        this.issueEvent(controlID);
    }
}
