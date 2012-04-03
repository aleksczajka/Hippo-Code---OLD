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

public partial class RemoveSticky : System.Web.UI.Page
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


    }

    protected void PostThread(object sender, EventArgs e)
    {
        string command = "";
        try
        {
            string GroupID = Request.QueryString["ID"].ToString();
            HttpCookie cookie = Request.Cookies["BrowserDate"];
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            SqlConnection conn = dat.GET_CONNECTED;
            SqlCommand cmd;

            cmd = new SqlCommand("UPDATE GroupMessages SET isSticky='False' WHERE GroupID=" + GroupID, conn);
            cmd.ExecuteNonQuery();

            RemovePanel.Visible = false;
            ThankYouPanel.Visible = true;
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString() + "<br/><br/>" + command;
        }
    }
}
