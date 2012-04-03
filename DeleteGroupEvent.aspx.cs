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

public partial class DeleteGroupEvent : System.Web.UI.Page
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
        //Ajax.Utility.RegisterTypeForAjax(typeof(Delete));
        if (!IsPostBack)
        {
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Context.Request.Cookies[cookieName];
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            DataView dvEvent = dat.GetDataDV("SELECT * FROM GroupEvents WHERE ID=" + 
                Request.QueryString["ID"].ToString());
            string groupID = dvEvent[0]["GroupID"].ToString();
            DataView dvGroup = dat.GetDataDV("SELECT * FROM Groups WHERE ID=" + groupID);
            ImageButton9.OnClientClick = "Search('" + dat.MakeNiceName(dvGroup[0]["Header"].ToString()) + "_" + groupID + "_Group');";

            TextLabel.Text = "Are you sure you want to delete the event '" + dvEvent[0]["Name"].ToString() +
                "' from the group '" + dvGroup[0]["Header"].ToString() + "'";
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        int i = 0;
    }

    protected void SendIt(object sender, EventArgs e)
    {
        
        
    }

    protected void DeleteEventAction(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        if (JustInstanceCheck.Checked)
        {
            dat.Execute("DELETE FROM GroupEvent_Occurance WHERE ID=" + Request.QueryString["O"].ToString());
            DataView dv = dat.GetDataDV("SELECT * FROM GroupEvent_Occurance WHERE GroupEventID=" + 
                Request.QueryString["ID"].ToString());
            if(dv.Count == 0)
                dat.Execute("UPDATE GroupEvents SET LIVE='False' WHERE ID=" + 
                    Request.QueryString["ID"].ToString());
        }
        else
        {
            dat.Execute("UPDATE GroupEvents SET LIVE='False' WHERE ID=" + Request.QueryString["ID"].ToString());
        }


       
        RemovePanel.Visible = false;
        ThankYouPanel.Visible = true;
        
    }
}
