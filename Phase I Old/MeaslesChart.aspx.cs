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

public partial class MeaslesChart : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlMeta hm = new HtmlMeta();
        HtmlHead head = (HtmlHead)Page.Header;
        hm.Name = "ROBOTS";
        hm.Content = "INDEX, FOLLOW";
        head.Controls.AddAt(0, hm);

        HtmlLink lk = new HtmlLink();
        lk.Attributes.Add("rel", "canonical");
        lk.Href = "http://hippohappenings.com/MeaslesChart.aspx";
        head.Controls.AddAt(0, lk);

    }
}
