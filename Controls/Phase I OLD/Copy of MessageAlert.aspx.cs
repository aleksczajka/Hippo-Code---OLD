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

public partial class Coaches_MessageAlert : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (!IsPostBack)
        {
            string type = Session["Type"].ToString();
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Context.Request.Cookies[cookieName];
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
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
                    DataSet ds = dat.GetData("SELECT CommunicationPrefs FROM UserPreferences WHERE UserID=" + authTicket.Name);
                    if (ds.Tables[0].Rows[0]["CommunicationPrefs"].ToString() == "1")
                        Session["On"] = true;
                    else
                        Session["On"] = false;
                }
                else
                {
                    Session["On"] = false;
                }
            }
            catch (Exception ex)
            {
                Session["On"] = false;
            }
            MessageTextBox.Text = "";
            switch (type)
            {
                case "Connect":
                    MessagePanel.Visible = true;
                    SubjectLabel.Text = Session["Subject"].ToString();
                    SubjectLabelPanel.Visible = true;
                    CalendarPanel.Visible = false;
                    TextPanel.Visible = false;
                    VenuePanel.Visible = false;
                    break;
                case "Message":
                    MessagePanel.Visible = true;
                    if (Session["Subject"] != null)
                        if (Session["Subject"].ToString() != "")
                        {
                            SubjectLabelPanel.Visible = true;
                            SubjectLabel.Text = Session["Subject"].ToString();
                        }
                        else
                        {
                            SubjectTextBoxPanel.Visible = true;
                            ErrorLabel.Visible = true;
                            Session["Subject"] = "";
                        }
                    else
                    {
                        SubjectTextBoxPanel.Visible = true;
                        ErrorLabel.Visible = true;
                        Session["Subject"] = "";
                    }
                    CalendarPanel.Visible = false;
                    TextPanel.Visible = false;
                    VenuePanel.Visible = false;
                    break;
                case "Calendar":
                    CalendarPanel.Visible = true;
                    MessagePanel.Visible = false;
                    TextPanel.Visible = false;
                    VenuePanel.Visible = false;
                    ConnectCheckBox.Checked = false;
                    if (bool.Parse(Session["On"].ToString()))
                        ConnectCheckBox.Visible = true;
                    else
                        ConnectCheckBox.Visible = false;
                    break;
                case "Txt":
                    PhoneTextBox.Text = "";
                    TextPanel.Visible = true;
                    MessagePanel.Visible = false;
                    CalendarPanel.Visible = false;
                    VenuePanel.Visible = false;
                    TxtMessageTextBox.Text = Session["Message"].ToString();
                    break;
                case "Venue":
                    MessagePanel.Visible = false;
                    CalendarPanel.Visible = false;
                    TextPanel.Visible = false;
                    VenuePanel.Visible = true;
                    break;
                case "Email":
                    EmailSubjectLabel.Text = "Hippo Happenings Info Request";
                    EmailTextBox.Text = Session["EmailMessage"].ToString();
                    MessagePanel.Visible = false;
                    CalendarPanel.Visible = false;
                    TextPanel.Visible = false;
                    VenuePanel.Visible = false;
                    EmailPanel.Visible = true;
                    break;
                default: break;
            }
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        int i = 0;
    }

    protected void SendIt(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        string userID = "";
        string cookieName = FormsAuthentication.FormsCookieName;
        HttpCookie authCookie = Context.Request.Cookies[cookieName];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        FormsAuthenticationTicket authTicket = null;
        try
        {
            authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            string group = authTicket.UserData.ToString();

            if (group.Contains("User"))
            {
                userID = authTicket.Name;
            }
            else
            {
                Button calendarLink = (Button)dat.FindControlRecursive(this, "CalendarLink");
                calendarLink.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }

        string toUserID = Session["ViewedUser"].ToString();

        string subject = "";

        if (Session["Subject"].ToString() == "")
            subject = SubjectTextBox.Text;
        else
            subject = Session["Subject"].ToString();

        if (subject != "")
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());

            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, To_UserID, Date, [Read], Mode)"
                + " VALUES(@content, @subject, @fromID, @toID, @date, 'False', 0)", conn);
            cmd.Parameters.Add("@content", SqlDbType.Text).Value = MessageTextBox.Text;
            cmd.Parameters.Add("@subject", SqlDbType.NVarChar).Value = subject;
            cmd.Parameters.Add("@toID", SqlDbType.Int).Value = int.Parse(toUserID);
            cmd.Parameters.Add("@fromID", SqlDbType.Int).Value = int.Parse(userID);
            cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
            cmd.ExecuteNonQuery();
            conn.Close();
        }


    }

    protected void AddTo(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        string userID = "";
        string cookieName = FormsAuthentication.FormsCookieName;
        HttpCookie authCookie = Context.Request.Cookies[cookieName];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        FormsAuthenticationTicket authTicket = null;
        try
        {
            authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            string group = authTicket.UserData.ToString();

            if (group.Contains("User"))
            {
                userID = authTicket.Name;
            }
            else
            {
                Button calendarLink = (Button)dat.FindControlRecursive(this, "CalendarLink");
                calendarLink.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }

        

        dat.Execute("INSERT INTO User_Calendar (UserID, EventID, ExcitmentID, isConnect) VALUES(" + userID + ", " + 
            Session["EventID"].ToString() + ", " + EventExcitmentDropDown.SelectedValue + ", '"+
            ConnectCheckBox.Checked.ToString()+"')");

        Label label = (Label)dat.FindControlRecursive(this.Parent, "AddToLabel");
        LinkButton link = (LinkButton)dat.FindControlRecursive(this.Parent, "AddToLink");
        label.Text = "stufrf";
        label.Visible = true;
        link.Visible = false;
    }
    public delegate void EventDelegate(object from, int ID);
    public event EventDelegate myEvent;
    protected void AddToVenue(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        string userID = "";
        string cookieName = FormsAuthentication.FormsCookieName;
        HttpCookie authCookie = Context.Request.Cookies[cookieName];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        FormsAuthenticationTicket authTicket = null;
        try
        {
            authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            string group = authTicket.UserData.ToString();

            if (group.Contains("User"))
            {
                userID = authTicket.Name;
            }
            else
            {
                Button calendarLink = (Button)dat.FindControlRecursive(this, "CalendarLink");
                calendarLink.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }



        dat.Execute("INSERT INTO UserVenues (UserID, VenueID) VALUES(" + userID + ", " + Session["VenueID"].ToString()+")");


    }
    public void issueEvent(int id)
    {
        myEvent(this, id);
    }
    protected void SendMessage(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (PhoneTextBox.Text != "")
        {
            try
            {
                Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

                //dat.SendEmail("hippoHapp email address", "Hippo Happenings User", PhoneTextBox.Text + ProvidersDropDown.SelectedValue, TxtMessageTextBox.Text, "Hippo Happenings Info Request");

            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "The Phone Number you enter must be a number!";
            }

        }
        else
        {
            ErrorLabel.Text = "Include Phone Number!";
        }
    }
    protected void SendEmail(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        try
        {
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            //dat.SendEmail("Hippo happenings email address", "Hippo Happening", YourEmailTextBox.Text, EmailTextBox.Text, EmailSubjectLabel.Text);  
        }
        catch (Exception ex)
        {

        }
    }
}
