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
public partial class AdMessage : System.Web.UI.Page
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
        //Ajax.Utility.RegisterTypeForAjax(typeof(AdMessage));
        if (!IsPostBack)
        {

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
                    Session["User"] = authTicket.Name;
                    DataSet dsFriends = dat.GetData("SELECT * FROM User_Friends UF, Users U WHERE UF.FriendID=U.User_ID AND UF.UserID=" + Session["User"].ToString());
                    bool getFriendsMessage = false;
                    if (dsFriends.Tables.Count > 0)
                        if (dsFriends.Tables[0].Rows.Count > 0)
                        {
                            string theImage = "";
                            string eventID = Request.QueryString["AID"].ToString();
                            for (int i = 0; i < dsFriends.Tables[0].Rows.Count; i++)
                            {
                                theImage += "<img style=\"padding: 5px;\" width=\"50px\" height=\"50px\" id=\"image" + i.ToString() + "\" ";
                                theImage += " alt=\"" + dsFriends.Tables[0].Rows[i]["UserName"].ToString() + "\" onclick=\"AddFriend('" +
                                    dsFriends.Tables[0].Rows[i]["UserName"].ToString() + "', '" + dsFriends.Tables[0].Rows[i]["User_ID"].ToString() + "');\" ";


                                if (System.IO.File.Exists(Server.MapPath(".") + "\\UserFiles\\" + dsFriends.Tables[0].Rows[i]["UserName"].ToString() + "\\Profile\\" + dsFriends.Tables[0].Rows[i]["ProfilePicture"].ToString()))
                                {
                                    theImage += " src=\"" + "UserFiles/" + dsFriends.Tables[0].Rows[i]["UserName"].ToString() + "/Profile/" + dsFriends.Tables[0].Rows[i]["ProfilePicture"].ToString() + "\" ";
                                }
                                else
                                    theImage += " src=\"" + "image/noAvatar_50x50_small.png\" ";

                                theImage += " />";
                            }

                            FriendLiteral.Text = theImage;

                            DataSet dsUser = dat.GetData("SELECT * FROM Users WHERE User_ID=" + Session["User"].ToString());
                            DataSet dsEvent = dat.GetData("SELECT * FROM Ads A, Ad_Calendar AC WHERE A.Ad_ID="+eventID+" AND A.Ad_ID=AC.AdID");
                            string msgBody = "<label>Ad: <a class=\"AddLink\" href=\"" + dat.MakeNiceName(dsEvent.Tables[0].Rows[0]["Header"].ToString()) + "_" + eventID + "_Ad\">" + dsEvent.Tables[0].Rows[0]["Header"].ToString() + "</a>" +
                                " \nDescription: " + dsEvent.Tables[0].Rows[0]["Description"].ToString() + "</label>";


                            Session["EventName"] = dsEvent.Tables[0].Rows[0]["Header"].ToString();

                            InvisiblesLiteral.Text = "<div style=\"display:none;\" id=\"userName\">" +
                                dsUser.Tables[0].Rows[0]["UserName"].ToString() +
                                "</div><div style=\"display: none;\" id=\"userID\">" + Session["User"].ToString() +
                                "</div><div style=\"display: none;\" id=\"eventName\">" +
                                dsEvent.Tables[0].Rows[0]["Header"].ToString() +
                                "</div><div style=\"display: none;\" id=\"msgBody\">" + msgBody + "</div>";
                        }
                        else
                            getFriendsMessage = true;
                    else
                        getFriendsMessage = true;

                    if (getFriendsMessage)
                    {
                        FriendLiteral.Text = "You do not have any friends listed. To add friends visit 'My Account'. To send information of this ad to an email account please visit this ad's page.";
                    }
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

    //[Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
    //public int SendIt(string msgText, string userName, string userID, string eventName, string[] idArray, string msgBody)
    //{
    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //    for (int i = 0; i < idArray.Length; i++)
    //    {
    //        SqlConnection conn = dat.GET_CONNECTED;
    //        SqlCommand cmd = new SqlCommand("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, To_UserID, Date, [Read], Mode)"
    //            + " VALUES(@content, @subject, @fromID, @toID, @date, 'False', 0)", conn);
    //        cmd.Parameters.Add("@content", SqlDbType.Text).Value = msgText + "\n\r" + msgBody;
    //        cmd.Parameters.Add("@subject", SqlDbType.NVarChar).Value = userName + " wants you to check out this ad:  " + eventName;
    //        cmd.Parameters.Add("@toID", SqlDbType.Int).Value = int.Parse(idArray[i]);
    //        cmd.Parameters.Add("@fromID", SqlDbType.Int).Value = int.Parse(userID);
    //        cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
    //        cmd.ExecuteNonQuery();
    //        conn.Close();
    //    }
    //    return 0;
    //}

    //[Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
    //public int RemoveID(string theDiv, string[] idArray)
    //{
    //    int numRemoved = -1;
    //    for (int i = 0; i < idArray.Length; i++)
    //    {
    //        if (idArray[i] == theDiv.Replace("div", ""))
    //        {
    //            numRemoved = i;
    //        }
    //    }

    //    return numRemoved;
    //}

    
   
}
