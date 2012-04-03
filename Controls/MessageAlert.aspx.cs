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

public partial class MessageAlert : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        //Ajax.Utility.RegisterTypeForAjax(typeof(Coaches_MessageAlert));

        SendItButton.SERVER_CLICK += SendIt;
        FlagButton.SERVER_CLICK += SendFlagEmail;
        BlueButton3.SERVER_CLICK += SendEmail;
        BlueButton5.SERVER_CLICK += AddVenue;
        BlueButton7.SERVER_CLICK += SendMessage;
        if (cookie != null)
        {
            if (!IsPostBack)
            {
                string type = Request.QueryString["T"].ToString();
                Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
                string phone = "";
                string providerExtension = "";
                try
                {

                    if (Session["User"] != null)
                    {
                        DataView dvUser = dat.GetDataDV("SELECT * FROM Users WHERE User_ID=" + Session["User"].ToString());
                        phone = dvUser[0]["PhoneNumber"].ToString();
                        providerExtension = dat.GetDataDV("SELECT Extension FROM PhoneProviders WHERE id=" +
                            dvUser[0]["PhoneProvider"].ToString())[0]["Extension"].ToString();

                        DataSet ds = dat.GetData("SELECT CommunicationPrefs FROM UserPreferences WHERE UserID="
                            + Session["User"].ToString());
                        if (ds.Tables[0].Rows[0]["CommunicationPrefs"].ToString() == "1")
                            Session["On"] = true;
                        else
                            Session["On"] = false;
                    }
                    else
                    {
                        TechLiteral.Text = "<script type=\"text/javascript\">Search('../login');</script>";
                        Session["On"] = false;
                    }
                }
                catch (Exception ex)
                {
                    Session["On"] = false;
                }
                MessageTextBox.Text = "";
                Encryption decrypt = new Encryption();
                switch (type)
                {
                    case "Connect":
                        MessagePanel.Visible = true;
                        if (Request.QueryString["Subject"] != null)
                            SubjectLabel.Text = Request.QueryString["Subject"].ToString();
                        else
                            SubjectLabel.Text = Session["Subject"].ToString();
                        SubjectLabelPanel.Visible = true;
                        CalendarPanel.Visible = false;
                        TextPanel.Visible = false;
                        VenuePanel.Visible = false;
                        break;
                    case "ConnectTrip":
                        DataSet dsAd = dat.GetData("SELECT * FROM Trips T, Users U WHERE T.UserName=U.UserName AND T.ID=" + Request.QueryString["ID"].ToString());
                        string subject = dsAd.Tables[0].Rows[0]["UserName"].ToString() + " would like to inquire into \"" + dsAd.Tables[0].Rows[0]["Header"].ToString() + "\"";
                        MessagePanel.Visible = true;
                        SubjectLabel.Text = subject;
                        SubjectLabelPanel.Visible = true;
                        CalendarPanel.Visible = false;
                        TextPanel.Visible = false;
                        VenuePanel.Visible = false;
                        break;
                    case "Message":
                        MessageTextBox.Text = "";
                        MessagePanel.Visible = true;
                        if (Session["Subject"] != null)
                            if (Session["Subject"].ToString() != "")
                            {

                                if (Request.QueryString["a"] != "")
                                {
                                    SubjectLabelPanel.Visible = true;
                                    SubjectLabel.Text = Session["Subject"].ToString();
                                    if (Request.QueryString["a"] == "ConnectTrip")
                                    {
                                        DataSet dsAd2 = dat.GetData("SELECT * FROM Trips T, Users U WHERE T.UserName=U.UserName AND T.ID=" + Request.QueryString["ID"].ToString());
                                        string subject2 = dsAd2.Tables[0].Rows[0]["UserName"].ToString() + " would like to inquire into \"" + dsAd2.Tables[0].Rows[0]["Header"].ToString() + "\"";
                                        SubjectLabel.Text = subject2;
                                    }
                                }
                                else
                                {
                                    SubjectTextBoxPanel.Visible = true;
                                    ErrorLabel.Visible = true;
                                    Session["Subject"] = "";
                                }
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
                        if (Request.QueryString["a"].ToString() == "a")
                            Session["ViewedUser"] = dat.GetData("SELECT User_ID FROM Ads WHERE Ad_ID=" + Request.QueryString["ID"].ToString()).Tables[0].Rows[0]["User_ID"].ToString();
                        else
                            Session["ViewedUser"] = Request.QueryString["ID"].ToString();
                        CalendarPanel.Visible = false;
                        TextPanel.Visible = false;
                        VenuePanel.Visible = false;


                        break;
                    case "Comment":
                        MessageTextBox.Text = "";
                        MessagePanel.Visible = true;
                        if (Session["CommentSubject"] != null)
                            if (Session["CommentSubject"].ToString() != "")
                            {
                                SubjectLabelPanel.Visible = true;
                                SubjectLabel.Text = Session["CommentSubject"].ToString();
                            }
                            else
                            {
                                SubjectTextBoxPanel.Visible = true;
                                ErrorLabel.Visible = true;
                                Session["CommentSubject"] = "";
                            }
                        else
                        {
                            SubjectTextBoxPanel.Visible = true;
                            ErrorLabel.Visible = true;
                            Session["CommentSubject"] = "";
                        }
                        Session["ViewedUser"] = Request.QueryString["ID"].ToString(); ;
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
                        string message = Session["messageText"].ToString();
                        if (phone != "0")
                            PhoneTextBox.Text = phone;
                        ProvidersDropDown.DataBind();
                        ProvidersDropDown.SelectedValue = providerExtension;
                        TextPanel.Visible = true;
                        MessagePanel.Visible = false;
                        CalendarPanel.Visible = false;
                        VenuePanel.Visible = false;
                        TxtMessageTextBox.Text = message;
                        break;
                    case "Venue":
                        MessagePanel.Visible = false;
                        CalendarPanel.Visible = false;
                        TextPanel.Visible = false;
                        VenuePanel.Visible = true;
                        TechLiteral.Text = "<div id=\"TechDiv\" style=\"display: none;\">" + Request.QueryString["ID"].ToString() + "</div>";
                        break;
                    case "Email":
                        if (Session["User"] != null)
                        {
                            //DataSet dsuser = dat.GetData("SELECT * FROM Users WHERE User_ID=" + Session["User"].ToString());
                            //YourEmailTextBox.Text = dsuser.Tables[0].Rows[0]["Email"].ToString();
                            //YourEmailTextBox.Enabled = false;
                            RequiredFieldValidatorEmail.IsValid = false;
                        }
                        else
                        {
                            RequiredFieldValidatorEmail.IsValid = false;
                        }

                        EmailSubjectLabel.Text = "Hippo Happenings Info";
                        MessagePanel.Visible = false;
                        CalendarPanel.Visible = false;
                        TextPanel.Visible = false;
                        VenuePanel.Visible = false;
                        EmailPanel.Visible = true;
                        break;
                    case "Flag":
                        FlagSubject.Text = "Hippo Happenings Flag!";
                        if (Session["User"] == null)
                        {
                            FlagTextPanel.Visible = true;
                            RequiredFieldValidator r = new RequiredFieldValidator();
                            r.ID = "RequiredFieldValidator3";
                            r.CssClass = "AddGreenLink";
                            r.ErrorMessage = "*Email is required";
                            r.ControlToValidate = "FlagEmailTextBox";
                            r.IsValid = false;
                            FlagTextPanel.Controls.Add(r);

                            RegularExpressionValidator t = new RegularExpressionValidator();
                            t.ID = "RegularExpressionValidator1";
                            t.CssClass = "AddGreenLink";
                            t.ErrorMessage = "*Email is not valid";
                            t.ValidationExpression = "^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$";
                            t.ControlToValidate = "FlagEmailTextBox";

                            FlagTextPanel.Controls.Add(t);

                        }

                        MessagePanel.Visible = false;
                        CalendarPanel.Visible = false;
                        TextPanel.Visible = false;
                        VenuePanel.Visible = false;
                        EmailPanel.Visible = false;
                        FlagPanel.Visible = true;
                        break;
                    default: break;
                }


            }
        }
        else
        {
            TechLiteral.Text = "<script type=\"text/javascript\">Search('../login');</script>";
        }
        if (!IsPostBack)
        {
            Session["VenueAdded"] = null;
        }

    }

    protected void Page_Init(object sender, EventArgs e)
    {
        int i = 0;
    }

    protected void SendIt(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        string userID = Session["User"].ToString();
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));


        string toUserID = "";
        DataSet dsAd = new DataSet();
        string subject = "";
        if (Session["Subject"] == null)
            subject = SubjectTextBox.Text;
        else if (Session["Subject"].ToString() == "")
            subject = SubjectTextBox.Text;
        else
            subject = Session["Subject"].ToString();
        string body = "";

        bool sendEmail = true;
        bool sendText = false;

        if (Request.QueryString["A"].ToString() == "Connect")
        {
            
             dsAd = dat.GetData("SELECT * FROM Ads WHERE Ad_ID=" + Request.QueryString["ID"].ToString());
            toUserID = dsAd.Tables[0].Rows[0]["User_ID"].ToString();
            body = 
                "<div><br/>A new email from "+Session["UserName"].ToString()+" arrived in your inbox on Hippo Happenings. <br/><br/> Subject: Regarding '"+dsAd.Tables[0].Rows[0]["Header"].ToString()+"'. <br/><br/> Message: " +
                MessageTextBox.Text + "<br/><br/> To view the email and reply, <a href=\"HippoHappenings.com/login\">" +
                "log into Hippo Happenings</a></div>";
        }
        else if (Request.QueryString["A"].ToString() == "ConnectTrip")
        {
            
             dsAd = dat.GetData("SELECT * FROM Trips T, Users U WHERE T.UserName=U.UserName AND T.ID=" + Request.QueryString["ID"].ToString());
            toUserID = dsAd.Tables[0].Rows[0]["User_ID"].ToString();
            subject = dsAd.Tables[0].Rows[0]["UserName"].ToString() + " would like to inquire into \"" + dsAd.Tables[0].Rows[0]["Header"].ToString() + "\"";
            body = 
                "<div><br/>A new email from "+Session["UserName"].ToString()+" arrived in your inbox on Hippo Happenings. <br/><br/> Subject: Regarding '"+dsAd.Tables[0].Rows[0]["Header"].ToString()+"'. <br/><br/> Message: " +
                MessageTextBox.Text + "<br/><br/> To view the email and reply, <a href=\"HippoHappenings.com/login\">" +
                "log into Hippo Happenings</a></div>";
        }
        else
        {
            toUserID = Request.QueryString["ID"].ToString();
            DataSet dsTo = dat.GetData("SELECT * FROM Users U, UserPreferences UP WHERE " +
                "UP.UserID=U.User_ID AND U.User_ID=" + toUserID);
            if (Request.QueryString["EV"] != null)
            {
                DataSet ds = new DataSet();
                //only send to email if users preferences are set to do so.
                    if (dsTo.Tables[0].Rows[0]["EmailPrefs"].ToString().Contains("7"))
                    {
                        sendEmail = true;
                    }
                    else
                    {
                        sendEmail = false;

                        DataView dvFirendsIndividual = dat.GetDataDV("SELECT * FROM Users U, UserPreferences UP, User_Friends UF WHERE " +
                            "UP.UserID=U.User_ID AND UP.EmailPrefs LIKE '%4%' AND U.User_ID=" + toUserID + " AND UF.UserID=" + toUserID + " AND UF.FriendID=" + Session["User"].ToString());
                        if (dvFirendsIndividual.Count > 0)
                            sendEmail = true;
                        else
                        {
                            dvFirendsIndividual = dat.GetDataDV("SELECT * FROM UserFriendPrefs UFP " +
                                "WHERE UFP.Preferences LIKE '%6%'AND UFP.UserID=" + toUserID + " AND UFP.FriendID=" + Session["User"].ToString());
                            if (dvFirendsIndividual.Count > 0)
                                sendEmail = true;
                        }
                    }
               
                if (Request.QueryString["EV"] == "V")
                {
                    body =
                    "<div><br/>A new email from " + Session["UserName"].ToString() + " arrived in your inbox on Hippo Happenings. <br/><br/> Subject: Regarding your comment on venue: " + subject + ". <br/><br/> Message: " +
                    MessageTextBox.Text + "<br/><br/> To view the email and reply, <a href=\"HippoHappenings.com/login\">" +
                    "log into Hippo Happenings</a></div>";
                }
                else
                {
                    body =
                    "<div><br/>A new email from " + Session["UserName"].ToString() + " arrived in your inbox on Hippo Happenings. <br/><br/> Subject: Regarding your comment on event: " + subject + ". <br/><br/> Message: " +
                    MessageTextBox.Text + "<br/><br/> To view the email and reply, <a href=\"HippoHappenings.com/login\">" +
                    "log into Hippo Happenings</a></div>";
                }
                
            }
            else
            {
                if (Request.QueryString["a"] == "z")
                {
                    //only send to email if users preferences are set to do so.
                    if (dsTo.Tables[0].Rows[0]["EmailPrefs"].ToString().Contains("7"))
                    {
                        sendEmail = true;
                    }
                    else
                    {
                        sendEmail = false;

                        DataView dvFirendsIndividual = dat.GetDataDV("SELECT * FROM UserFriendPrefs UFP " +
                            "WHERE UFP.Preferences LIKE '%6%'AND UFP.UserID=" + toUserID + " AND UFP.FriendID=" + Session["User"].ToString());
                        if (dvFirendsIndividual.Count > 0)
                            sendEmail = true;
                    }
                }
                else
                {
                    sendEmail = false;
                    sendText = false;
                    //only send to email if users preferences are set to do so.
                    DataView dvFirendsIndividual = dat.GetDataDV("SELECT * FROM UserFriendPrefs UFP " +
                            "WHERE UFP.Preferences LIKE '%8%'AND UFP.UserID=" + toUserID + " AND UFP.FriendID=" + Session["User"].ToString());
                    if (dvFirendsIndividual.Count > 0)
                        sendEmail = true;
                }
                body =
                    "<div><br/>A new email from " + Session["UserName"].ToString() + " arrived in your inbox on Hippo Happenings. <br/><br/> Subject: " + subject + ". <br/><br/> Message: " +
                    MessageTextBox.Text + "<br/><br/> To view the email and reply, <a href=\"HippoHappenings.com/login\">" +
                    "log into Hippo Happenings</a></div>";
            }
        }


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

            DataSet dsUser = dat.GetData("SELECT *, PP.Extension AS Ex1 FROM Users U, UserPreferences UP, PhoneProviders PP "+
                "WHERE U.User_ID=UP.UserID AND U.PhoneProvider=PP.ID AND U.User_ID=" + toUserID);
            DataSet dsUserEmail = dat.GetData("SELECT * FROM Users U, UserPreferences UP " +
                "WHERE U.User_ID=UP.UserID AND U.User_ID=" + toUserID);
            if (sendEmail)
            {
                dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                    dsUserEmail.Tables[0].Rows[0]["Email"].ToString(), body, "HippoHappenings Mail: " + subject);
            }

            EmailMessageLabel.Text = "Your message has been sent.";

            MessagePanel.Visible = false;
            EmailPanel.Visible = false;
            ThankYouPanel.Visible = true;
        }
        else
        {
            EmailMessageLabel.Text = "Subject is null";
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



        dat.Execute("INSERT INTO User_Calendar (UserID, EventID, ExcitmentID, isConnect, DateAdded) VALUES(" + userID + ", " +
            Session["EventID"].ToString() + ", " + EventExcitmentDropDown.SelectedValue + ", '" +
            ConnectCheckBox.Checked.ToString() + "', '" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).ToString() + "')");



        Label label = (Label)dat.FindControlRecursive(this.Parent, "AddToLabel");
        LinkButton link = (LinkButton)dat.FindControlRecursive(this.Parent, "AddToLink");
        label.Text = "stufrf";
        label.Visible = true;
        link.Visible = false;

        dat.SendFriendAddNotification(Session["User"].ToString(), ID);

    }
    public delegate void EventDelegate(object from, int ID);
    public event EventDelegate myEvent;

    //[Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
    //public int AddToVenue(string theID)
    //{
    //    string userID = "";
    //    string cookieName = FormsAuthentication.FormsCookieName;
    //    HttpCookie authCookie = Context.Request.Cookies[cookieName];
    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //    FormsAuthenticationTicket authTicket = null;
    //    try
    //    {
    //        authTicket = FormsAuthentication.Decrypt(authCookie.Value);
    //        string group = authTicket.UserData.ToString();

    //        if (group.Contains("User"))
    //        {
    //            userID = authTicket.Name;
    //            dat.Execute("INSERT INTO UserVenues (UserID, VenueID) VALUES(" + userID + ", " + theID + ")");
    //        }
    //        else
    //        {
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //    return 0;

    //}

    protected void AddVenue(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (Session["VenueAdded"] == null)
        {
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            dat.Execute("INSERT INTO UserVenues (UserID, VenueID, DateAdded) VALUES(" + Session["User"].ToString() +
                ", " + Request.QueryString["ID"].ToString() + ", '" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).ToString() + "')");

            Session["VenueAdded"] = "notnull";
            ScriptLiteral.Text = "<script type=\"text/javascript\" >Search('anything');</script>";
        }
        VenueMessageLabel.Text = "Venue has been added to your favorites";
    }

    public void issueEvent(int id)
    {
        myEvent(this, id);
    }
    protected void SendMessage(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        try
        {
            if (TxtMessageTextBox.Text.Trim().Length > 160)
            {
                PhoneMessage.Text = "Character count of your message must be less than 161.";
            }
            else
            {
                Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
                //DataSet dsuser = dat.GetData("SELECT * FROM Users WHERE User_ID=" + Session["User"].ToString());
                dat.SendText(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                    dat.MakeGoodPhone(PhoneTextBox.Text) + ProvidersDropDown.SelectedValue, TxtMessageTextBox.Text, "HippoHappenings Info");
                PhoneMessage.Text = "Your message has been sent.";
                TextPanel.Visible = false;
                ThankYouPanel.Visible = true;
            }
        }
        catch (Exception ex)
        {
            PhoneMessage.Text = ex.ToString();
            //PhoneMessage.Text = "Sending message failed! Make sure the number you entered is correct.";
        }

    }
    protected void SendEmail(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        try
        {
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            if (EmailTextBox.Text.Trim() != "")
            {
                if (Session["User"] != null)
                {
                    DataSet dsuser = dat.GetData("SELECT * FROM Users WHERE User_ID=" + Session["User"].ToString());

                    char[] delim = { ';' };
                    string[] tokens = YourEmailTextBox.Text.Trim().Split(delim);

                    bool goOn = true;
                    string problematic = "";
                    for (int i = 0; i < tokens.Length; i++)
                    {
                        if (tokens[i].Trim() != "")
                        {
                            if (!dat.ValidateEmail(tokens[i].Trim()))
                            {
                                goOn = false;
                                problematic = " First problematic email is '"+tokens[i].Trim()+"'";
                                break;
                            }
                        }
                    }

                    if (goOn)
                    {
                        for (int i = 0; i < tokens.Length; i++)
                        {
                            if (tokens[i].Trim() != "")
                            {
                                if (dat.ValidateEmail(tokens[i].Trim()))
                                {
                                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                                    tokens[i].Trim(), "<div>  " + Session["UserName"].ToString() + " has sent you the following information."
                                    + "<br/><br/>Personal Message: " + EmailTextBox.Text + "<br/><br/></div>Details: <br/>" +
                                    Session["messageEmail"].ToString(), EmailSubjectLabel.Text);
                                }
                            }
                        }
                        EmailSentLabel.Text = "Your message has been sent";
                        MessagePanel.Visible = false;
                        EmailPanel.Visible = false;
                        ThankYouPanel.Visible = true;
                    }
                    else
                    {
                        EmailSentLabel.Text = "One or more of the emails you provided is not valid. "+
                            "No emails were sent. "+problematic;
                    }
                }
                else
                {
                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                    YourEmailTextBox.Text, "<div>  " + Session["UserName"].ToString() + " has sent you the following information."+
                    "<br/><br/>Personal Message: " + EmailTextBox.Text +
                    "<br/><br/></div>Details: <br/>" +
                    Session["messageEmail"].ToString(), EmailSubjectLabel.Text);
                    EmailSentLabel.Text = "Your message has been sent";
                }
            }
            else
            {
                EmailSentLabel.Text = "Please include your message";
            }
            
        }
        catch (Exception ex)
        {
            EmailSentLabel.Text = ex.ToString();
        }
    }
    protected void SendFlagEmail(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        try
        {
            if (TextBox2.Text.Trim() != "")
            {
                string EType = Session["FlagType"].ToString();
                string id = Session["FlagID"].ToString();
                string body = "<br/> Regarding EType: " + EType + ", ID: " + id;
                Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
                if (FlagTextPanel.Visible)
                {
                    SqlDbType[] types = { SqlDbType.NVarChar, SqlDbType.NVarChar, SqlDbType.Int, SqlDbType.NVarChar };
                    object[] data = { TextBox2.Text, "Flag Item", 2, FlagEmailTextBox.Text };
                    dat.ExecuteWithParemeters("INSERT INTO MessagesForAdmin (Message, Subject, Type, Email) VALUES(@p0, @p1, @p2, @p3)", types, data);
                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                        System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                                        System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                         "Anonymous user has flagged an item. EType: " + EType + ", ID: " + id + " Here is their email:" +
                                        FlagEmailTextBox.Text + " Here is their message:  <br/><br/>" + TextBox2.Text, "Hippo Website Flag: An item has been flagged.");
                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                            System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                            FlagEmailTextBox.Text
            , "Hippo Happenings has received your request to flag an item. We will investigate and take appropriate action.", "Hippo Happenings Flag Request Received");


                }
                else
                {
                    SqlDbType[] types = { SqlDbType.NVarChar, SqlDbType.Int, SqlDbType.NVarChar, SqlDbType.Int };
                    object[] data = { TextBox2.Text, Session["User"].ToString(), "Flag Item", 2 };
                    dat.ExecuteWithParemeters("INSERT INTO MessagesForAdmin (Message, UserID, Subject, Type) VALUES(@p0, @p1, @p2, @p3)", types, data);


                    DataSet dsUser = dat.GetData("SELECT * FROM Users WHERE User_ID=" + Session["User"].ToString());

                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                        System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                                        System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString()
                        , "User ID: " + Session["User"].ToString() + ", UserName: " +
                        dsUser.Tables[0].Rows[0]["UserName"].ToString() +
                        " has flagged an item. EType: " + EType + ", ID: " + id + " Here is their message: <br/><br/>" +
                        TextBox2.Text, "Hippo Website Flag: An item has been flagged.");

                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                        System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                                        dsUser.Tables[0].Rows[0]["Email"].ToString()
                        , "Hippo Happenings has received your request to flag an item. " +
                        "We will investigate and get back to you.", "Hippo Happenings Flag Request Received");
                }

                Label2.Text = "Your message has been sent.";
                FlagPanel.Visible = false;
                ThankYouPanel.Visible = true;
            }
            else
            {
                Label2.Text = "Please include your message.";
            }
        }
        catch (Exception ex)
        {
            Label2.Text = ex.ToString();
        }
    }
    protected void ValidateTxtBox(object sender, ServerValidateEventArgs e)
    {
        if (TxtMessageTextBox.Text.Length > 160 || TxtMessageTextBox.Text.Length == 0)
            e.IsValid = false;
        else
            e.IsValid = true;
    } 
}
