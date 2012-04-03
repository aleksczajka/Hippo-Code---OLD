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

public partial class SelectFriendMembers : System.Web.UI.Page
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
        //Ajax.Utility.RegisterTypeForAjax(typeof(Communicate));
        if (!IsPostBack)
        {
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            try
            {
                if (Session["User"] != null)
                {

                    DataSet dsUser = dat.GetData("SELECT * FROM Users WHERE User_ID=" + Session["User"].ToString());
                    DataSet dsUsers = dat.GetData("SELECT DISTINCT U.UserName, U.User_ID FROM Users U, User_Friends UF "
                        + " WHERE U.User_ID=UF.FriendID AND UF.UserID=" + Session["User"].ToString());

                    UsersListBox.DataSource = dsUsers;
                    UsersListBox.DataTextField = "UserName";
                    UsersListBox.DataValueField = "User_ID";
                    UsersListBox.DataBind();
                }
            }
            catch (Exception ex)
            {
                MessageLabel.Text = ex.ToString();
            }  
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        int i = 0;
    }

    protected void SendIt(object sender, EventArgs e)
    {
        try
        {
            //Hashtable table = (Hashtable)Session["TheHash"];

            //foreach (string key in table.Keys)
            //{
            //    Session["SelectedMembers"] += key + ";";
            //}


            //ThankYouPanel.Visible = true;
            //MessagePanel.Visible = false;
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString();
        }
    }

    protected void SelectThemFriends(object sender, EventArgs e)
    {
        try
        {
           
            Hashtable hash = new Hashtable();
            foreach (ListItem item in UsersListBox.Items)
            {
                if (item.Selected)
                {
                    hash.Add(item.Value, "");
                }
            }
            Session["TheHash"] = hash;

            Session["SelectedMembers"] = "";
            foreach (string key in hash.Keys)
            {
                Session["SelectedMembers"] += key + ";";
            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString();
        }
    }
}
