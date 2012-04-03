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

public partial class ProhibitedAds : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlHead head = (HtmlHead)Page.Header;
        HtmlLink lk = new HtmlLink();
        lk.Href = "http://" + Request.Url.Authority + "/ProhibitedAds.aspx";
        lk.Attributes.Add("rel", "bookmark");
        head.Controls.AddAt(0, lk);
    }
}
