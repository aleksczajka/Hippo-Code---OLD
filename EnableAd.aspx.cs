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

public partial class EnableAd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlMeta hm = new HtmlMeta();
        HtmlHead head = (HtmlHead)Page.Header;
        hm.Name = "ROBOTS";
        hm.Content = "NOINDEX, FOLLOW";
        head.Controls.AddAt(0, hm);

        BlueButton3.SERVER_CLICK += DeleteEventAction;
    }

    protected void Page_Init(object sender, EventArgs e)
    {
    }

    protected void SendIt(object sender, EventArgs e)
    {
        
        
    }

    protected void DeleteEventAction(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        dat.Execute("UPDATE Ads SET LIVE='True', ReNewed='True' WHERE AD_ID=" + Request.QueryString["ID"].ToString());
        RemovePanel.Visible = false;
        ThankYouPanel.Visible = true;
        
    }

}
