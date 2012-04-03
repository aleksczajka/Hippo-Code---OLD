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

public partial class InviteFriendAlert : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlLink lk = new HtmlLink();
        HtmlHead head = (HtmlHead)Page.Header;
        lk.Attributes.Add("rel", "canonical");
        lk.Href = "http://hippohappenings.com/InviteFriendAlert.aspx";
        head.Controls.AddAt(0, lk);


        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }

        ASP.controls_bluebutton_ascx SendButton = (ASP.controls_bluebutton_ascx)SearchPanel.Items[0].Items[0].FindControl("SendButton");

        SendButton.SERVER_CLICK = SendIt;

        Label MessageLabel = (Label)SearchPanel.Items[0].Items[0].FindControl("MessageLabel");
        MessageLabel.Text = "";
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            try
            {
                if (Session["User"] != null)
                {
                    int count = 0;
                    if (Infolabel.Text != "")
                    {
                        count = int.Parse(Infolabel.Text);
                    }
                    else
                    {
                        count = 5;
                        Infolabel.Text = "5";
                    }
                    Panel FriendPanel = (Panel)SearchPanel.Items[0].Items[0].FindControl("FriendPanel");
                    FriendPanel.Controls.Clear();
                    TextBox text;
                    for (int i = 1; i <= count; i++)
                    {
                        text = new TextBox();
                        text.ID = "text" + i.ToString();
                        text.Width = 250;

                        FriendPanel.Controls.Add(text);
                    }
                }
                else
                {
                    Response.Redirect("login");
                }
            }
            catch (Exception ex)
            {
            }
    }

    protected void AddMore(object sender, EventArgs e)
    {
        Panel FriendPanel = (Panel)SearchPanel.Items[0].Items[0].FindControl("FriendPanel");
        int textsCount = int.Parse(Infolabel.Text);
        int nextCount = textsCount + 1;
        TextBox text = new TextBox();
        text.Width = 250;
        text.ID = "text"+nextCount.ToString();

        Infolabel.Text = (nextCount).ToString();

        FriendPanel.Controls.Add(text);
    }

    protected void SendIt(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        try
        {
            int boxes = int.Parse(Infolabel.Text);
            TextBox temp;
            string message = "";
            for (int i = 1; i <= boxes; i++)
            {
                temp = (TextBox)dat.FindControlRecursive(this, "text" + i);
                if (temp.Text != "")
                {
                    if (dat.ValidateEmail(temp.Text))
                    {
                        dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                        System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(), temp.Text,
                            "Greetings from HippoHappenings. <br/><br/> Your friend " + Session["UserName"].ToString()
                            + " would like you to join our team. <br/><br/>At HippoHappenings you can post "
                            + "happenings, share details and talk about favorite sightseeing trips, post classifieds, have your bulletins posted thoughout the site, have your very own "
                            + "happenings calendar, post locales and much more. <br/><br/> To sign up visit <a target=\"_blank\" "+
                            "href=\"http://HippoHappenings.com/Register.aspx\">HippoHappenings.com/Register.aspx</a>",
                            Session["UserName"].ToString() + " Would like you to join HippoHappenings!");
                    }
                    else
                        message += "Email in box " + i.ToString() + 
                            " was not valid and message was not sent. <br/>";
                }
            }

            Label MessageLabel = (Label)SearchPanel.Items[0].Items[0].FindControl("MessageLabel");

            if (message != "")
                MessageLabel.Text = message;
            else
                MessageLabel.Text = "Your friends have been invited!";
        }
        catch (Exception ex)
        {
            
        }
        
    }

}
