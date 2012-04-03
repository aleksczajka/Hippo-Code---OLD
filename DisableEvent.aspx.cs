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

public partial class DisableEvent : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlMeta hm = new HtmlMeta();
        HtmlHead head = (HtmlHead)Page.Header;
        hm.Name = "ROBOTS";
        hm.Content = "NOINDEX, FOLLOW";
        head.Controls.AddAt(0, hm);

        BlueButton2.SERVER_CLICK += DeleteEventAction;
    }

    protected void DeleteEventAction(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        dat.Execute("UPDATE Events SET LIVE='False' WHERE ID=" + Request.QueryString["ID"].ToString());
        RemovePanel.Visible = false;
        ThankYouPanel.Visible = true;
        
    }

    //[Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
    //public int DeleteAd1(string theID, string userID)
    //{
    //    try
    //    {
    //        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //        dat.Execute("DELETE FROM Ad_Calendar WHERE AdID=" + theID);
    //        dat.Execute("DELETE FROM Ads WHERE Ad_ID=" + theID);
    //        dat.Execute("DELETE FROM Ad_Category_Mapping WHERE AdID=" + theID);
    //        dat.Execute("DELETE FROM Ad_Slider_Mapping WHERE AdID=" + theID);
    //        //RadScheduler1.Appointments; 
    //        return int.Parse(theID); 
    //    }
    //    catch (Exception ex)
    //    {
    //        return 0;
    //    }
    //}
}
