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


public partial class Message : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlMeta hm = new HtmlMeta();
        HtmlHead head = (HtmlHead)Page.Header;
        hm.Name = "ROBOTS";
        hm.Content = "NOINDEX, FOLLOW";
        head.Controls.AddAt(0, hm);

        HtmlLink lk = new HtmlLink();
        lk.Attributes.Add("rel", "canonical");
        lk.Href = "http://hippohappenings.com/Message.aspx";
        head.Controls.AddAt(0, lk);

        Encryption decrypt = new Encryption();
        Label MessageLabel = new Label();
        
        if (Session["Message"] != null || Request.QueryString["message"] != null)
        {
            string message = "";
            if (Request.QueryString["message"] != null)
                message = "<label>" + decrypt.decrypt(Request.QueryString["message"].ToString()) + "</label>";
            else
                message = "<label>" + Session["Message"].ToString() + "</label>";

            //if (Session["str"] != null)
            //    message += "<div style=\"color: red;\">" + Session["str"].ToString() + "</div>";

            if (message.Contains("<savedadsearch>"))
            {
                string[] delim = { "<savedadsearch>ID:" };
                string[] tokens = message.Split(delim, StringSplitOptions.None);
                string ID2 = "";
                int i = 0;
                while (tokens[1][i] != '<')
                {
                    ID2 += tokens[1][i];
                    i++;
                }

                MessageLabel.Text = tokens[0]+"<label>Your search is now live. You will receive "+
                    "periodic emails with new featured ads matching your criteria. Click the "+
                    "button to disable this feature. You can also modify it in the Searches page "+
                    "later on.<br/><table><tr><td>";
                MessageLabel.ID = "Label1";
                Label newLabel = new Label();
                newLabel.Text = "</td></tr></table>" + tokens[1].Replace(ID2 +
                    "</savedadsearch>", "") + "</label>";


                ASP.controls_bluebutton_ascx button = new ASP.controls_bluebutton_ascx();
                
                MessagePanel.Controls.Add(MessageLabel);
                MessagePanel.Controls.Add(button);
                MessagePanel.Controls.Add(newLabel);

                button.BUTTON_TEXT = "Disable Live Search";
                button.SERVER_CLICK += button_ServerClick;
                button.Attributes.Add("value", ID2);
            }
            else
            {
                MessageLabel.Text = message;
                MessagePanel.Controls.Add(MessageLabel);
            }
        }
    }

    protected void button_ServerClick(object sender, EventArgs e)
    {
        Encryption decrypt = new Encryption();
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }

        string ID2 = "";

        string message = "";
            if (Request.QueryString["message"] != null)
                message = "<label>" + decrypt.decrypt(Request.QueryString["message"].ToString()) + "</label>";
            else
                message = "<label>" + Session["Message"].ToString() + "</label>";

            //if (Session["str"] != null)
            //    message += "<div style=\"color: red;\">" + Session["str"].ToString() + "</div>";

            if (message.Contains("<savedadsearch>"))
            {
                string[] delim = { "<savedadsearch>ID:" };
                string[] tokens = message.Split(delim, StringSplitOptions.None);

                int i = 0;
                while (tokens[1][i] != '<')
                {
                    ID2 += tokens[1][i];
                    i++;
                }
            }
        
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        dat.Execute("UPDATE SavedAdSearches SET Live='False' WHERE ID=" + ID2);

        Label label = new Label();
        label.Text = "Your email feature for your saved search is now disabled. <table><tr><td align=\"right\"><div style=\"padding-top: 20px;\" align=\"center\">" +
                    "<div style=\"width: 50px;\" onclick=\"Search()\">" +
                    "<div class=\"topDiv\" style=\"clear: both;\">" +
                      "  <img style=\"float: left;\" src=\"NewImages/ButtonLeft.png\" height=\"27px\" /> " +
                       " <div class=\"NavyLink\" style=\"font-size: 12px; text-decoration: none; padding-top: 5px;padding-left: 6px; padding-right: 6px;height: 27px;float: left;background: url('NewImages/ButtonPixel.png'); background-repeat: repeat-x;\">" +
                           " OK " +
                        "</div>" +
                       " <img style=\"float: left;\" src=\"NewImages/ButtonRight.png\" height=\"27px\" /> " +
                    "</div>" +
                    "</div>" +
                    "</div></td><td align=\"left\"><div style=\"padding-top: 20px;\" align=\"center\">" +
                    "<div style=\"width: 150px;\" onclick=\"Search('SearchesAndPages.aspx')\">" +
                    "<div class=\"topDiv\" style=\"clear: both;\">" +
                      "  <img style=\"float: left;\" src=\"NewImages/ButtonLeft.png\" height=\"27px\" /> " +
                       " <div class=\"NavyLink\" style=\"font-size: 12px; text-decoration: none; padding-top: 5px;padding-left: 6px; padding-right: 6px;height: 27px;float: left;background: url('NewImages/ButtonPixel.png'); background-repeat: repeat-x;\">" +
                           " Go To Your Searches " +
                        "</div>" +
                       " <img style=\"float: left;\" src=\"NewImages/ButtonRight.png\" height=\"27px\" /> " +
                    "</div>" +
                    "</div>" +
                    "</div></td></tr></table>";

        MessagePanel.Controls.Clear();
        MessagePanel.Controls.Add(label);
    }
}
