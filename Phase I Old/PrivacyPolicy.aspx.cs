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

public partial class PrivacyPolicy : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlMeta hm = new HtmlMeta();
        HtmlHead head = (HtmlHead)Page.Header;
        hm.Name = "ROBOTS";
        hm.Content = "NOINDEX, FOLLOW";
        head.Controls.AddAt(0, hm);
    }
}
