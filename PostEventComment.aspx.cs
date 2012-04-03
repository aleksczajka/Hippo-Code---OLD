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

public partial class PostEventComment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HtmlMeta hm = new HtmlMeta();
            HtmlHead head = (HtmlHead)Page.Header;
            hm.Name = "ROBOTS";
            hm.Content = "NOINDEX, FOLLOW";
            head.Controls.AddAt(0, hm);
            HttpCookie cookie = Request.Cookies["BrowserDate"];
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            DataView dvGroupEvent = dat.GetDataDV("SELECT * FROM GroupEvents WHERE ID=" + Request.QueryString["ID"].ToString());
            ImageButton9.OnClientClick = "javascript:Search('" +
                dat.MakeNiceName(dvGroupEvent[0]["Name"].ToString()) + "_" + Request.QueryString["O"].ToString() + "_" +
                Request.QueryString["ID"].ToString() + "_GroupEvent');";


            string groupID = Request.QueryString["ID"].ToString();
            string command = "SELECT * FROM Group_Members WHERE GroupID=" +
               dvGroupEvent[0]["GroupID"].ToString() + " AND MemberID=" + Session["User"].ToString();
            DataView dvMembers = dat.GetDataDV(command);
            if (bool.Parse(dvMembers[0]["SharedHosting"].ToString()))
            {
                HostPanel.Visible = true;
            }
            else
                HostPanel.Visible = false;
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString();
        }
    }

    protected void PostThread(object sender, EventArgs e)
    {
        string command = "";
        try
        {
            if (SubjectTextBox.Text.Trim() != "")
            {
                string theID = Request.QueryString["ID"].ToString();

                HttpCookie cookie = Request.Cookies["BrowserDate"];
                Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

                SqlConnection conn = dat.GET_CONNECTED;
                SqlCommand cmd;

                cmd = new SqlCommand("INSERT INTO GroupEventMessages ([Content], UserID, GroupEventID, isSticky, DatePosted)" +
                    " VALUES(@content, @userID, @groupID, @sticky, GETDATE())", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@content", SqlDbType.NVarChar).Value = SubjectTextBox.Text.Trim();
                cmd.Parameters.Add("@userID", SqlDbType.Int).Value = Session["User"].ToString();
                cmd.Parameters.Add("@groupID", SqlDbType.Int).Value = Request.QueryString["ID"].ToString();
                cmd.Parameters.Add("@sticky", SqlDbType.Bit).Value = StickyCheckBox.Checked;
                cmd.ExecuteNonQuery();

                RemovePanel.Visible = false;
                ThankYouPanel.Visible = true;

            }
            else
            {
                ErrorLabel.Text = "Please include the message.";
            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString() + "<br/><br/>" + command;
        }
    }
}
