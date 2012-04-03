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

public partial class ManageParticipants : System.Web.UI.Page
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
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        ImageButton9.OnClientClick = "javascript:Search('GroupEvent.aspx?ID=" + Request.QueryString["ID"].ToString() + "&O=" + Request.QueryString["O"].ToString() + "');";

        DataView dvMembers = dat.GetDataDV("SELECT * FROM GroupEvent_Members GM, Users U WHERE " +
            "U.User_ID=GM.UserID AND GM.GroupEventID=" + Request.QueryString["ID"].ToString() + 
            " AND Accepted='True'");

        Label label;
        Literal lit;
        TextBox textB;
        TextBox textD;
        CheckBox checkS;

        lit = new Literal();
        lit.Text = "<table><tr><td><label>Member</label></td><td width=\"200px\" style=\"padding-left: 15px;\"><div style=\"float: left;padding-top: 5px;\"><label>Remove Member</label>" +
            "</div></td></tr>";
        MembersPanel.Controls.Add(lit);
        
        foreach (DataRowView row in dvMembers)
        {
            lit = new Literal();
            lit.Text = "<tr><td valign=\"top\">";
            MembersPanel.Controls.Add(lit);

            label = new Label();
            label.CssClass = "AddLink";
            label.Text = row["UserName"].ToString();
            label.ID = "member" + row["UserID"].ToString();
            MembersPanel.Controls.Add(label);

            lit = new Literal();
            lit.Text = "</td><td style=\"padding-left: 50px;\" valign=\"top\">";
            MembersPanel.Controls.Add(lit);

            checkS = new CheckBox();
            checkS.ID = "checkD" + row["UserID"].ToString();
            MembersPanel.Controls.Add(checkS);

            lit = new Literal();
            lit.Text = "</td></tr>";
            MembersPanel.Controls.Add(lit);
        }

        lit = new Literal();
        lit.Text = "</tr></table>";
        MembersPanel.Controls.Add(lit);
    }

    protected void SavePrefs(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        DataView dvMembers = dat.GetDataDV("SELECT * FROM GroupEvent_Members GM, Users U WHERE " +
    "U.User_ID=GM.UserID AND GM.GroupEventID=" + Request.QueryString["ID"].ToString() +" AND Accepted='True'");

        DataView dvGroup = dat.GetDataDV("SELECT * FROM Groups WHERE ID=" +
            Request.QueryString["ID"].ToString());

        TextBox textB;
        TextBox textD;
        CheckBox checkS;
        CheckBox checkD;

        foreach (DataRowView row in dvMembers)
        {
            checkD = (CheckBox)MembersPanel.FindControl("checkD" + row["UserID"].ToString());

            if (checkD.Checked)
            {
                dat.Execute("DELETE FROM GroupEvent_Members WHERE UserID=" + row["UserID"].ToString() +
                    " AND GroupEventID=" + Request.QueryString["ID"].ToString());
            }
        }

        RemovePanel.Visible = false;
        ThankYouPanel.Visible = true;
    }
}
