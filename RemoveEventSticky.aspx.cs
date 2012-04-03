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

public partial class RemoveEventSticky : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlMeta hm = new HtmlMeta();
        HtmlHead head = (HtmlHead)Page.Header;
        hm.Name = "ROBOTS";
        hm.Content = "NOINDEX, FOLLOW";
        head.Controls.AddAt(0, hm);
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        DataView dvGroupEvent = dat.GetDataDV("SELECT * FROM GroupEvents WHERE ID=" + 
            Request.QueryString["ID"].ToString());

        ImageButton9.OnClientClick = "javascript:Search('" +
            dat.MakeNiceName(dvGroupEvent[0]["Name"].ToString()) + "_" + Request.QueryString["O"].ToString() + "_" +
            Request.QueryString["ID"].ToString() + "_GroupEvent');";


    }

    protected void PostThread(object sender, EventArgs e)
    {
        string command = "";
        try
        {
            string GroupID = Request.QueryString["RID"].ToString();
            HttpCookie cookie = Request.Cookies["BrowserDate"];
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            SqlConnection conn = dat.GET_CONNECTED;
            SqlCommand cmd;
            command = "UPDATE GroupEventMessages SET isSticky='False' WHERE ID=" + GroupID;
            cmd = new SqlCommand(command, conn);
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
