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

public partial class Blog_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Data dat = new Data(DateTime.Now);

        if(Request.QueryString["ID"] == null)
            Response.Redirect("../home");

        string theID = Request.QueryString["ID"].ToString();

        DataView dv = dat.GetDataDV("SELECT * FROM HippoBlogContent WHERE ID=" + theID);

        TitleLiteral.Text = dv[0]["Title"].ToString();
        TagLineLiteral.Text = dv[0]["TagLine"].ToString();
        MainContentLiteral.Text = dv[0]["MainContent"].ToString();

        MainImage.Visible = false;
        if (dv[0]["MainImage"].ToString() != "")
        {
            MainImage.ImageUrl = dv[0]["MainImage"].ToString();
            MainImage.Visible = true;
        }

        Page.Title = dat.MakeNiceName(dv[0]["Title"].ToString());
    }
}
