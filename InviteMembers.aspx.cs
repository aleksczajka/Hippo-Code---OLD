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

public partial class InviteMembers : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlMeta hm = new HtmlMeta();
        HtmlHead head = (HtmlHead)Page.Header;
        hm.Name = "ROBOTS";
        hm.Content = "NOINDEX, FOLLOW";
        head.Controls.AddAt(0, hm);

        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        if (!IsPostBack)
        {
            MembererrorLabel.Text = "";
            MemberDescriptionTextBox.Attributes.Add("onkeyup", "CountCharsDMem(event)");
            MemberTitleTextBox.Attributes.Add("onkeyup", "CountCharsTMem(event)");
        }
    }

    protected void SendIt(object sender, EventArgs e)
    {
        
        
    }

    protected void FillMemberInvites(object sender, EventArgs e)
    {
        try
        {
            char[] delim = { ';' };
            char[] delim2 = { '-' };
            if (Session["SelectedMembers"] != null)
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

                Hashtable hash = new Hashtable();

                foreach (ListItem item in MembersListBox.Items)
                {
                    string[] toks = item.Text.Split(delim2);
                    hash.Add(toks[0].Trim(), "");
                }

                string[] tokens = Session["SelectedMembers"].ToString().Split(delim);
                DataView dvv;
                
                foreach (string token in tokens)
                {
                    
                    if (token.Trim() != "")
                    {
                        dvv = dat.GetDataDV("SELECT * FROM Users WHERE User_ID=" + token);
                        if (!hash.Contains(dvv[0]["UserName"].ToString()))
                        {
                            MembersListBox.Items.Add(new ListItem(dvv[0]["UserName"].ToString(), dvv[0]["User_ID"].ToString()));
                            hash.Add(dvv[0]["UserName"].ToString(), "");
                        }
                    }
                }
                Session["SelectedMembers"] = null;
                Session.Remove("SelectedMembers");
            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString();
        }
    }

    protected void RemoveMember(object sender, EventArgs e)
    {
        if (MembersListBox.SelectedIndex != -1)
        {
            ListItem item = MembersListBox.SelectedItem;
            char[] delim = { '-' };
            string[] tokens = item.Value.Split(delim);

            if (tokens[0].Trim() != Session["UserName"].ToString())
                MembersListBox.Items.Remove(item);
        }
    }

    protected void AssignTitle(object sender, EventArgs e)
    {
        try
        {
            if (MembersListBox.SelectedIndex != -1)
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

                char[] delim ={ '-' };
                string[] tokens = MembersListBox.SelectedItem.Text.Split(delim);

                string userName = tokens[0];
                string sharedStr = "";
                if (SharedHostingCheckBox.Checked)
                {
                    sharedStr = " - shared hosting";
                }
                string description = "";
                bool goOn = true;
                if (MemberDescriptionTextBox.Text.Trim() != "")
                {
                    if (MemberDescriptionTextBox.Text.Trim().Length <= 150)
                    {
                        string[] valueTokens = MembersListBox.SelectedItem.Value.Split(delim);
                            MembersListBox.SelectedItem.Value = valueTokens[0].Trim() + "-" + MemberDescriptionTextBox.Text;

                        description = " - description added";
                    }
                    else
                        goOn = false;
                }
                if (goOn)
                {
                    string title = "";
                    if (MemberTitleTextBox.Text.Trim() != "")
                    {
                        if (MemberTitleTextBox.Text.Trim().Length <= 15)
                        {
                            title = " - " + MemberTitleTextBox.Text.Trim();
                        }
                        else
                        {
                            goOn = false;
                        }
                    }
                    if (goOn)
                        MembersListBox.SelectedItem.Text = userName + title + description + sharedStr;
                    else
                    {
                        MembererrorLabel.Text = "Member's title must be less than 15 characters.";
                    }
                }
                else
                {
                    MembererrorLabel.Text = "Member's description must be less than 150 characters.";
                }
            }
            else
            {
                MembererrorLabel.Text = "Choose somebody.";
            }
        }
        catch (Exception ex)
        {
            MembererrorLabel.Text = ex.ToString();
        }
    }

    protected void SelectTitle(object sender, EventArgs e)
    {
        try
        {
            if (MembersListBox.SelectedValue != null)
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

                char[] delim = { '-' };
                if (MembersListBox.SelectedItem.Text.Trim() != "")
                {
                    string[] tokens = MembersListBox.SelectedItem.Text.Split(delim);
                    if (tokens.Length > 1)
                    {
                        if (tokens[1].Trim() != "description added" && tokens[1].Trim() != "shared hosting")
                        {
                            MemberTitleTextBox.Text = tokens[1].Trim();
                        }
                        else if (tokens[1].Trim() == "shared hosting")
                        {
                            SharedHostingCheckBox.Checked = true;
                        }
                        string[] valueTokens = MembersListBox.SelectedItem.Value.Split(delim);

                        if (valueTokens.Length > 1)
                            MemberDescriptionTextBox.Text = valueTokens[1].Trim();
                        else
                            MemberDescriptionTextBox.Text = "";

                        if (tokens.Length > 2)
                        {
                            if (tokens[2].Trim() == "shared hosting")
                                SharedHostingCheckBox.Checked = true;
                            else
                            {
                                if (tokens.Length > 3)
                                {
                                    if (tokens[3].Trim() == "shared hosting")
                                        SharedHostingCheckBox.Checked = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        MemberDescriptionTextBox.Text = "";
                        MemberTitleTextBox.Text = "";
                        SharedHostingCheckBox.Checked = false;
                    }
                }
                else
                {
                    MemberDescriptionTextBox.Text = "";
                    MemberTitleTextBox.Text = "";
                    SharedHostingCheckBox.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
            ErrorLabelUpdate.Text = ex.ToString();
        }
    }

    protected void CreateMembers(object sender, EventArgs e)
    {
        string command = "";
        try
        {
            string theID = Request.QueryString["ID"].ToString();
            
            char[] delim = { '-' };
            HttpCookie cookie = Request.Cookies["BrowserDate"];
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            DataView dvGroup = dat.GetDataDV("SELECT * FROM Groups WHERE ID=" + theID);
            string name = dvGroup[0]["Header"].ToString();

            string emailBody = Session["UserName"].ToString() + " is inviting you to the group <a href=\"http://hippohappenings.com/" +
                dat.MakeNiceName(name) +
                "_" + theID + "_Group\">" + name +
                   "</a>." +
                   " To accept the membership <a href=\"http://hippohappenings.com/my-account\">log in to HippoHappenings</a>, view your messages in 'My Account' and " +
                   "click 'Accept' in the message corresponding to this email. Good luck in your membership! <br/><br/>HippoHappenings";
            string emailSubject = "You have a group invite for '" + name + "'";
            string messageBody =  theID + " " + Session["UserName"].ToString() + " has created a new group <a href=\"" +
                dat.MakeNiceName(name) +
                "_" + theID + "_Group\">" + name +
                   "</a> and is inviting you to it. Click " +
                "the 'Accept' button to the right if you want to become part of this group. ";
            foreach (ListItem item in MembersListBox.Items)
            {
                string[] tokens = item.Text.Split(delim);
                DataView dvUser = dat.GetDataDV("SELECT * FROM Users WHERE UserName='" + tokens[0].Trim() + "'");
                string ID = dvUser[0]["User_ID"].ToString();
                
                string title = "";
                string titleUpdate = "";
                string titleBeg = "";
                string descrip = "";
                string descripUpdate = "";
                string descripBeg = "";
                string shared = "";
                string sharedUpdate = "";
                string sharedBeg = "";
                string updateString = "";
                if (tokens.Length > 1)
                {
                    if (tokens[1].Trim() != "description added" && tokens[1].Trim() != "shared hosting")
                    {
                        title = ", '" + tokens[1].Trim().Replace("'", "''") + "'";
                        titleUpdate = " Title = '" + tokens[1].Trim().Replace("'", "''") + "'";
                        titleBeg = ",Title";
                    }
                    else if (tokens[1].Trim() == "shared hosting")
                    {
                        sharedUpdate = " SharedHosting = 'True' ";
                        shared = ", 'True'";
                        sharedBeg = ", SharedHosting";
                    }
                }

                if (ID == Session["User"].ToString())
                {
                    sharedUpdate = " SharedHosting = 'True' ";
                    shared = ", 'True'";
                    sharedBeg = ", SharedHosting";
                }

                if (item.Value.Trim() != "")
                {
                    string[] valueTokens = item.Value.Split(delim);
                    if (valueTokens.Length > 1)
                    {
                        descripUpdate = " Description = '" + item.Value.Trim().Replace("'", "''") + "'";
                        descrip = ", '" + valueTokens[1].Trim().Replace("'", "''") + "'";
                        descripBeg = ", Description";
                    }
                }
                if (tokens.Length > 2)
                {
                    if (tokens[2].Trim() == "shared hosting")
                    {
                        sharedUpdate = " SharedHosting = 'True' ";
                        shared = ", 'True'";
                        sharedBeg = ", SharedHosting";
                    }
                    else
                    {
                        if (tokens.Length > 3)
                        {
                            if (tokens[3].Trim() == "shared hosting")
                            {
                                sharedUpdate = " SharedHosting = 'True' ";
                                shared = ", 'True'";
                                sharedBeg = ", SharedHosting";
                            }
                        }
                    }
                }

                //take care of update string
                if (titleUpdate.Trim() != "")
                {
                    updateString = titleUpdate;
                }

                if (descripUpdate.Trim() != "")
                {
                    if (updateString.Trim() != "")
                    {
                        updateString += ", " + descripUpdate;
                    }
                    else
                    {
                        updateString = descripUpdate;
                    }
                }

                if (sharedUpdate.Trim() != "")
                {
                    if (updateString.Trim() != "")
                    {
                        updateString += ", " + sharedUpdate;
                    }
                    else
                    {
                        updateString = sharedUpdate;
                    }
                }

                //execute update/insert member
                DataView dvMember = dat.GetDataDV("SELECT * FROM Group_Members WHERE MemberID=" + ID + " AND GroupID=" + Request.QueryString["ID"].ToString());
                if (dvMember.Count == 0)
                {
                    command = "INSERT INTO Group_Members (GroupID, MemberID " + titleBeg + descripBeg + sharedBeg + ") VALUES(" + theID + ", " + ID + title + descrip + shared + ")";
                }
                else
                {
                    if (updateString != "")
                        command = "UPDATE Group_Members SET " + updateString + " WHERE MemberID=" + ID + " AND GroupID=" + Request.QueryString["ID"].ToString();
                }
                //ErrorLabel.Text = command;
                if (command.Trim() != "")
                {
                    dat.Execute(command);
                    dat.Execute("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, To_UserID, " +
                    "[Date], [Read], [Mode], [Live]) VALUES('" + messageBody.Replace("'", "''") + "', '" + emailSubject.Replace("'", "''") + "', " +
                    dat.HIPPOHAPP_USERID.ToString() + ", " + ID + ", '" +
                    DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).ToString() + "', 'False', 7, 'True')");
                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                            System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                                            dvUser[0]["Email"].ToString(), emailBody, emailSubject);
                }
            }

            RemovePanel.Visible = false;
            ThankYouPanel.Visible = true;
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString() + "<br/><br/>" + command;
        }
    }
}
