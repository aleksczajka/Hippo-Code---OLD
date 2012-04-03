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

public partial class ThreadAdmin : System.Web.UI.Page
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

        switch (Request.QueryString["M"].ToString())
        {
            case "A":
                ActivePanel.Visible = true;
                InactivatePanel.Visible = false;
                DeletePanel.Visible = false;
                ShowPanel.Visible = false;
                HidePanel.Visible = false;
                Button3.Text = "Activate";
                break;
            case "I":
                ActivePanel.Visible = false;
                InactivatePanel.Visible = true;
                DeletePanel.Visible = false;
                ShowPanel.Visible = false;
                HidePanel.Visible = false;
                Button3.Text = "Inactivate";
                break;
            case "H":
                ActivePanel.Visible = false;
                InactivatePanel.Visible = false;
                DeletePanel.Visible = false;
                ShowPanel.Visible = false;
                HidePanel.Visible = true;
                Button3.Text = "Hide";
                break;
            case "S":
                ActivePanel.Visible = false;
                InactivatePanel.Visible = false;
                DeletePanel.Visible = false;
                ShowPanel.Visible = true;
                HidePanel.Visible = false;
                Button3.Text = "Show";
                break;
            case "D":
                ActivePanel.Visible = false;
                InactivatePanel.Visible = false;
                DeletePanel.Visible = true;
                ShowPanel.Visible = false;
                HidePanel.Visible = false;
                Button3.Text = "Delete";
                break;
            default: break;
        }

    }

    protected void InactivateThread(object sender, EventArgs e)
    {
        string command = "";
        try
        {
            string ThreadID = Request.QueryString["ID"].ToString();
            HttpCookie cookie = Request.Cookies["BrowserDate"];
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            dat.Execute("UPDATE GroupThreads SET Active='False' WHERE ID=" + ThreadID);

            ThankYouLabel.Text = "Your thread has been inactivated.";

            InactivatePanel.Visible = false;
            ThankYouPanel.Visible = true;
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString() + "<br/><br/>" + command;
        }
    }

    protected void ActivateThread(object sender, EventArgs e)
    {
        string command = "";
        try
        {
            string ThreadID = Request.QueryString["ID"].ToString();
            HttpCookie cookie = Request.Cookies["BrowserDate"];
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            dat.Execute("UPDATE GroupThreads SET Active='True' WHERE ID=" + ThreadID);

            ThankYouLabel.Text = "Your thread has been activated.";

            ActivePanel.Visible = false;
            ThankYouPanel.Visible = true;
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString() + "<br/><br/>" + command;
        }
    }

    protected void HideThread(object sender, EventArgs e)
    {
        string command = "";
        try
        {
            string ThreadID = Request.QueryString["ID"].ToString();
            HttpCookie cookie = Request.Cookies["BrowserDate"];
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            dat.Execute("UPDATE GroupThreads SET Hidden='True' WHERE ID=" + ThreadID);

            ThankYouLabel.Text = "Your thread has been hidden.";

            HidePanel.Visible = false;
            ThankYouPanel.Visible = true;
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString() + "<br/><br/>" + command;
        }
    }

    protected void ShowThread(object sender, EventArgs e)
    {
        string command = "";
        try
        {
            string ThreadID = Request.QueryString["ID"].ToString();
            HttpCookie cookie = Request.Cookies["BrowserDate"];
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            dat.Execute("UPDATE GroupThreads SET Hidden='False' WHERE ID=" + ThreadID);

            ThankYouLabel.Text = "Your thread has been shown.";

            ShowPanel.Visible = false;
            ThankYouPanel.Visible = true;
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString() + "<br/><br/>" + command;
        }
    }

    protected void DeleteThread(object sender, EventArgs e)
    {
        string command = "";
        try
        {
            string ThreadID = Request.QueryString["ID"].ToString();
            HttpCookie cookie = Request.Cookies["BrowserDate"];
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            dat.Execute("DELETE FROM GroupThreads WHERE ID=" + ThreadID);

            dat.Execute("DELETE FROM GroupThreads_Comments WHERE ThreadID=" + ThreadID);

            ThankYouLabel.Text = "Your thread has been deleted.";

            DeletePanel.Visible = false;
            ThankYouPanel.Visible = true;
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString() + "<br/><br/>" + command;
        }
    }
}
