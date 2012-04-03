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

public partial class ResetPassword : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SignInButton.SERVER_CLICK += MakeItSo;
        HtmlLink lk = new HtmlLink();
        HtmlHead head = (HtmlHead)Page.Header;
        lk.Attributes.Add("rel", "canonical");
        lk.Href = "http://hippohappenings.com/ResetPassword.aspx";
        head.Controls.AddAt(0, lk);

        HtmlMeta hm = new HtmlMeta();
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
        //Ads1.DATA_SET = dat.RetrieveAllAds(false);
        //Ads1.MAIN_AD_DATA_SET = dat.RetrieveAllAds(true);
        if (Request.QueryString["CODE"] == null || Request.QueryString["UserName"] == null)
        {
            Response.Redirect("home");
        }
        else
        {
            SqlDbType[] types = { SqlDbType.NVarChar };
            object[] parameters = { Request.QueryString["UserName"].ToString() };
            DataSet ds = dat.GetDataWithParemeters("SELECT * FROM Users WHERE UserName=@p0", types, parameters);

            bool isNot = false;
            if (ds.Tables.Count > 0)
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string code = ds.Tables[0].Rows[0]["PasswordReset"].ToString();
                    if (code != Request.QueryString["CODE"].ToString())
                        Response.Redirect("home");
                    else
                    {
                        UserLabel.Text = Request.QueryString["UserName"].ToString();
                    }
                }
                else
                    isNot = true;
            else
                isNot = true;

        }
    }

    protected void MakeItSo(object sender, System.EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        Encryption encrypt = new Encryption();
        if (PasswordTextBox.Text.Trim() != "" && ConfirmPasswordTextBox.Text.Trim() != "")
        {
            if (PasswordTextBox.Text.Trim() == ConfirmPasswordTextBox.Text.Trim())
            {
                dat.Execute("UPDATE Users SET PasswordReset='', PASSWORD='"+encrypt.encrypt(PasswordTextBox.Text.Trim())+"' WHERE UserName='"+Request.QueryString["UserName"].ToString()+"'");
                ErrorLabel.Text = "Your password has been changed.";
            }
            else
            {
                ErrorLabel.Text = "Password and confirm password must match.";
            }
        }
        else
        {
            ErrorLabel.Text = "Include both password and confirm password.";
        }

    }


}
