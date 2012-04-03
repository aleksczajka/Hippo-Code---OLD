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

public partial class Communicate : System.Web.UI.Page
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
        //Ajax.Utility.RegisterTypeForAjax(typeof(Communicate));
        if (!IsPostBack)
        {
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            try
            {
                string group = "";
                

                string eventID = Request.QueryString["ID"].ToString();
                if (Session["User"] != null)
                {
                    //DataSet dsFriends = dat.GetData("SELECT * FROM User_Friends UF, Users U, User_Calendar UC WHERE UC.UserID=UF.FreindID AND UC.EventID= "+eventID+" AND UF.FriendID=U.User_ID AND UF.UserID=" + Session["User"].ToString());
                    //bool getFriendsMessage = false;
                    //if (dsFriends.Tables.Count > 0)
                    //    if (dsFriends.Tables[0].Rows.Count > 0)
                    //    {
                    //        string theImage = "";
                            
                    //        for (int i = 0; i < dsFriends.Tables[0].Rows.Count; i++)
                    //        {
                    //            theImage += "<img style=\"padding: 5px;\" width=\"50px\" height=\"50px\" id=\"image"+i.ToString()+"\" ";
                    //            theImage += " alt=\"" + dsFriends.Tables[0].Rows[i]["UserName"].ToString() + "\" onclick=\"AddFriend('"+
                    //                dsFriends.Tables[0].Rows[i]["UserName"].ToString()+"', '"+dsFriends.Tables[0].Rows[i]["User_ID"].ToString()+"');\" ";
                                
                                
                    //            if (System.IO.File.Exists(Server.MapPath(".") + "\\UserFiles\\" + dsFriends.Tables[0].Rows[i]["UserName"].ToString() + "\\Profile\\" + dsFriends.Tables[0].Rows[i]["ProfilePicture"].ToString()))
                    //            {
                    //                theImage += " src=\"" + "UserFiles/" + dsFriends.Tables[0].Rows[i]["UserName"].ToString() + "/Profile/" + dsFriends.Tables[0].Rows[i]["ProfilePicture"].ToString() + "\" ";
                    //            }
                    //            else
                    //                theImage += " src=\""+ "image/noAvatar_50x50_small.png\" ";

                    //            theImage += " />";
                    //        }



                            DataSet dsUser = dat.GetData("SELECT * FROM Users WHERE User_ID="+Session["User"].ToString());
                            DataSet dsEvent = dat.GetData("SELECT * FROM Events E, Event_Occurance EO, Venues V WHERE E.ID=EO.EventID AND E.Venue=V.ID AND E.ID="+eventID);
                            string msgBody = "<label>Event: <a class=\"AddLink\" href=\""+dat.MakeNiceName(dsEvent.Tables[0].Rows[0]["Header"].ToString())+"_"+
                                eventID+"_Event\">" + dsEvent.Tables[0].Rows[0]["Header"].ToString() +"</a>"+
                                " \nVenue: " + dsEvent.Tables[0].Rows[0]["Name"].ToString() +
                                " \nDate: " + dsEvent.Tables[0].Rows[0]["DateTimeStart"].ToString() +
                                " \nDescription: " + dsEvent.Tables[0].Rows[0]["Content"].ToString()+"</label>";

                           
                            Session["EventName"] = dsEvent.Tables[0].Rows[0]["Header"].ToString();

                            InvisiblesLiteral.Text = "<div style=\"display:none;\" id=\"userName\">" +
                                dsUser.Tables[0].Rows[0]["UserName"].ToString() +
                                "</div><div style=\"display: none;\" id=\"userID\">" + Session["User"].ToString() +
                                "</div><div style=\"display: none;\" id=\"eventName\">" +
                                dsEvent.Tables[0].Rows[0]["Header"].ToString() +
                                "</div><div style=\"display: none;\" id=\"msgBody\">"+msgBody+"</div>";


                            DataSet dsUsers = dat.GetData("SELECT DISTINCT U.UserName, U.User_ID FROM Users U, User_Calendar UC, UserPreferences UP, User_Friends UF "
                                +" WHERE U.User_ID <> "+Session["User"].ToString()+" AND UP.UserID=U.User_ID AND UC.EventID="
                                + eventID + " AND UC.UserID=U.User_ID AND ((UP.CommunicationPrefs = 2 AND UC.isConnect='True') "+
                                "OR (UP.CommunicationPrefs = 1) OR (UP.CommunicationPrefs = 3 AND UF.UserID=U.User_ID AND UF.FriendID = " + Session["User"].ToString() + "))");

                            UsersListBox.DataSource = dsUsers;
                            UsersListBox.DataTextField = "UserName";
                            UsersListBox.DataValueField = "User_ID";
                            UsersListBox.DataBind();

                    //    }
                    //    else
                    //        getFriendsMessage = true;
                    //else
                    //    getFriendsMessage = true;

                  
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
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
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        SqlDbType [] types = {SqlDbType.NVarChar, SqlDbType.NVarChar, SqlDbType.Int, SqlDbType.Int, SqlDbType.DateTime, SqlDbType.Bit, SqlDbType.Int};
       
       bool isUserSelected = false;
        for (int i = 0; i < UsersListBox.Items.Count; i++)
        {
            if (UsersListBox.Items[i].Selected)
            {
                isUserSelected = true;
                object[] data = {TextInput.InnerText, "You have a communication request for event '"+
                    Session["EventName"].ToString()+"'", Session["User"].ToString(), UsersListBox.Items[i].Value, DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")), false, 3};
                dat.ExecuteWithParemeters("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, To_UserID, Date, [Read], Mode) " +
                    " VALUES(@p0,@p1,@p2,@p3,@p4,@p5,@p6)", types, data);

                DataSet dsFriend = dat.GetData("SELECT * FROM Users U, UserPreferences UP WHERE UP.UserID=U.User_ID AND U.User_ID=" + UsersListBox.Items[i].Value);

                string subject = "You have a communication request for event '" +
                    Session["EventName"].ToString() + "' from " + dsFriend.Tables[0].Rows[0]["UserName"].ToString();
                string body = "<div style=\"color: #cccccc;\"><br/>A new email arrived at your inbox on Hippo Happenings from " + Session["UserName"].ToString() + ". <br/><br/> Subject: " + subject + ". <br/><br/> Message: " +
                TextInput.InnerText + "<br/><br/> To view the email and reply, <a href=\"http://HippoHappenings.com/User.aspx\">" +
                "log into Hippo Happenings</a></div>";

                //only send to email if users preferences are set to do so.
                if (dsFriend.Tables[0].Rows[0]["EmailPrefs"].ToString().Contains("6"))
                {
                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                        System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                        dsFriend.Tables[0].Rows[0]["Email"].ToString(), body, "HippoHappenings Mail: " + subject);
                }
            }
        }

        if (!isUserSelected)
        {
            CommunicateLabel.Text = "You have not selected any participants.";
        }
        else
        {
            ThankYouPanel.Visible = true;
            MessagePanel.Visible = false;
        }
        
    }


}
