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

public partial class EditMemberPrefs : System.Web.UI.Page
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

        ImageButton9.OnClientClick = "javascript:Search('Group.aspx?ID=" + Request.QueryString["ID"].ToString() + "');";

        DataView dvMembers = dat.GetDataDV("SELECT * FROM Group_Members GM, Users U WHERE " +
            "U.User_ID=GM.MemberID AND GM.GroupID=" + Request.QueryString["ID"].ToString());

        DataView dvGroup = dat.GetDataDV("SELECT * FROM Groups WHERE ID=" + Request.QueryString["ID"].ToString());

        string host = dvGroup[0]["Host"].ToString();

        bool isHost = false;
        if (Session["User"].ToString() == host)
            isHost = true;

        Label label;
        Literal lit;
        TextBox textB;
        TextBox textD;
        CheckBox checkS;
        RadioButton radButt;

        lit = new Literal();
        lit.Text = "<table><tr><td><label>Member</label></td><td><label>Title</label>" +
            "</td><td><label>Description</label></td><td><label>Shared Hosting</label>" +
            "</td>";
        if(isHost)
            lit.Text += "<td align=\"center\"><label>Make Primary Host</label></td>";

        lit.Text += "<td width=\"200px\" style=\"padding-left: 15px;\"><div style=\"float: left;padding-top: 5px;\"><label>Remove Member</label>" +
            "</div><div style=\"float: left; padding-left: 5px; \" >";
        MembersPanel.Controls.Add(lit);
        
        Image HelpImage = new Image();
        HelpImage.CssClass = "HelpImage";
        HelpImage.ID = "HelpImage2";
        HelpImage.ImageUrl = "~/image/helpIcon.png";
        MembersPanel.Controls.Add(HelpImage);

        Telerik.Web.UI.RadToolTip RadToolTip2 = new Telerik.Web.UI.RadToolTip();
        RadToolTip2.Skin = "Black";
        RadToolTip2.Width = 200;
        RadToolTip2.Height = 200;
        RadToolTip2.ManualClose = true;
        RadToolTip2.ShowEvent = Telerik.Web.UI.ToolTipShowEvent.OnClick;
        RadToolTip2.Position = Telerik.Web.UI.ToolTipPosition.MiddleRight;
        RadToolTip2.RelativeTo = Telerik.Web.UI.ToolTipRelativeDisplay.Element;
        RadToolTip2.TargetControlID = HelpImage.ClientID;
        
        lit = new Literal();
        lit.Text = "<label>To remove a member, you must first disable their 'Shared Hosting' "+
        "if they have it. However, you cannot ever remove the member that has posted the group.</label>";
        RadToolTip2.Controls.Add(lit);
        MembersPanel.Controls.Add(RadToolTip2);

        lit = new Literal();
        lit.Text = "</div></td></tr>";
        MembersPanel.Controls.Add(lit);

        foreach (DataRowView row in dvMembers)
        {
            lit = new Literal();
            lit.Text = "<tr><td valign=\"top\">";
            MembersPanel.Controls.Add(lit);

            label = new Label();
            label.CssClass = "AddLink";
            label.Text = row["UserName"].ToString();
            label.ID = "member" + row["MemberID"].ToString();
            MembersPanel.Controls.Add(label);

            lit = new Literal();
            lit.Text = "</td><td valign=\"top\">";
            MembersPanel.Controls.Add(lit);

            textB = new TextBox();
            textB.ID = "text" + row["MemberID"].ToString();
            textB.Width = 150;
            textB.Text = row["Title"].ToString();
            MembersPanel.Controls.Add(textB);

            lit = new Literal();
            lit.Text = "</td><td valign=\"top\">";
            MembersPanel.Controls.Add(lit);

            textD = new TextBox();
            textD.ID = "textD" + row["MemberID"].ToString();
            textD.TextMode = TextBoxMode.MultiLine;
            textD.Width = 170;
            textD.Height = 100;
            textD.Text = row["Description"].ToString();
            MembersPanel.Controls.Add(textD);

            lit = new Literal();
            lit.Text = "</td><td align=\"center\" valign=\"top\">";
            MembersPanel.Controls.Add(lit);

            checkS = new CheckBox();
            checkS.ID = "checkS" + row["MemberID"].ToString();
            checkS.Checked = bool.Parse(row["SharedHosting"].ToString());
            MembersPanel.Controls.Add(checkS);

            lit = new Literal();
            lit.Text = "</td><td align=\"center\" valign=\"top\" width=\"50px\">";
            MembersPanel.Controls.Add(lit);

            if (isHost)
            {
                radButt = new RadioButton();
                radButt.Checked = false;
                radButt.AutoPostBack = true;
                if (!IsPostBack)
                {
                    if (row["MemberID"].ToString() == host)
                        radButt.Checked = true;
                }
                radButt.ID = "radButt" + row["MemberID"].ToString();
                radButt.CheckedChanged += new EventHandler(radButt_CheckedChanged);
                MembersPanel.Controls.Add(radButt);

                lit = new Literal();
                lit.Text = "</td><td align=\"center\" valign=\"top\" style=\"padding-left: 15px;\">";
                MembersPanel.Controls.Add(lit);
            }

            checkS = new CheckBox();
            checkS.ID = "checkD" + row["MemberID"].ToString();
            MembersPanel.Controls.Add(checkS);

            lit = new Literal();
            lit.Text = "</td></tr>";
            MembersPanel.Controls.Add(lit);
        }

        lit = new Literal();
        lit.Text = "</tr></table>";
        MembersPanel.Controls.Add(lit);
    }

    protected void radButt_CheckedChanged(object sender, EventArgs e)
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

        DataView dvMembers = dat.GetDataDV("SELECT * FROM Group_Members GM, Users U WHERE " +
            "U.User_ID=GM.MemberID AND GM.GroupID=" + Request.QueryString["ID"].ToString());
        RadioButton rad;
        foreach (DataRowView row in dvMembers)
        {
            rad = (RadioButton)MembersPanel.FindControl("radButt" + row["MemberID"].ToString());
            rad.Checked = false;
        }
        RadioButton radButt = (RadioButton)sender;
        radButt.Checked = true;
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

        DataView dvMembers = dat.GetDataDV("SELECT * FROM Group_Members GM, Users U WHERE " +
    "U.User_ID=GM.MemberID AND GM.GroupID=" + Request.QueryString["ID"].ToString());

        DataView dvGroup = dat.GetDataDV("SELECT * FROM Groups WHERE ID=" +
            Request.QueryString["ID"].ToString());

        TextBox textB;
        TextBox textD;
        CheckBox checkS;
        CheckBox checkD;
        RadioButton radButt;

        foreach (DataRowView row in dvMembers)
        {

            textB = (TextBox)MembersPanel.FindControl("text" + row["MemberID"].ToString());
            textD = (TextBox)MembersPanel.FindControl("textD" + row["MemberID"].ToString());
            checkS = (CheckBox)MembersPanel.FindControl("checkS" + row["MemberID"].ToString());
            checkD = (CheckBox)MembersPanel.FindControl("checkD" + row["MemberID"].ToString());

            radButt = (RadioButton)MembersPanel.FindControl("radButt" + row["MemberID"].ToString());

           

            if (checkD.Checked && !bool.Parse(row["SharedHosting"].ToString()) && row["MemberID"].ToString() != dvGroup[0]["Host"].ToString())
            {
                dat.Execute("DELETE FROM Group_Members WHERE MemberID=" + row["MemberID"].ToString() +
                    " AND GroupID=" + Request.QueryString["ID"].ToString());
            }
            else
            {
                dat.Execute("UPDATE Group_Members SET Title='" + textB.Text.Trim().Replace("'", "''") +
                    "', Description='" + textD.Text.Trim().Replace("'", "''") +
                    "', SharedHosting='" + checkS.Checked.ToString() +
                    "' WHERE MemberID=" + row["MemberID"].ToString() + " AND GroupID=" +
                    Request.QueryString["ID"].ToString());

                if (radButt.Checked)
                {
                    dat.Execute("UPDATE Groups SET Host=" + row["MemberID"].ToString()+
                        " WHERE ID=" + Request.QueryString["ID"].ToString() );
                }
            }
        }

        RemovePanel.Visible = false;
        ThankYouPanel.Visible = true;
    }
}
