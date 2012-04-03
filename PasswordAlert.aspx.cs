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


public partial class PasswordAlert : System.Web.UI.Page
{
   protected void Page_Load(object sender, EventArgs e)
    {
        HtmlLink lk = new HtmlLink();
        HtmlHead head = (HtmlHead)Page.Header;
        lk.Attributes.Add("rel", "canonical");
        lk.Href = "http://hippohappenings.com/PasswordAlert.aspx";
        head.Controls.AddAt(0, lk);

        HtmlMeta hm = new HtmlMeta();
        hm.Name = "ROBOTS";
        hm.Content = "NOINDEX, FOLLOW";
        head.Controls.AddAt(0, hm);

        SendItButton.SERVER_CLICK += SendIt;

        Encryption decrypt = new Encryption();
        if (Request.QueryString["message"] != null)
            MessageLabel.Text = decrypt.decrypt(Request.QueryString["message"].ToString());

    }

   protected void SendIt(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        if (EmailTextBox.Text != "")
        {
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            SqlDbType [] types = {SqlDbType.NVarChar};
            EmailTextBox.Text = dat.stripHTML(EmailTextBox.Text.Trim());
            object[] parameters = { EmailTextBox.Text };
            DataSet ds = dat.GetDataWithParemeters("SELECT * FROM Users WHERE Email=@p0", types, parameters);
            
            bool isNot = false;
            if (ds.Tables.Count > 0)
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Encryption encrypt = new Encryption();
                    Random r = new Random(0);
                    string code = encrypt.encrypt(ds.Tables[0].Rows[0]["UserName"].ToString() + r.Next().ToString());
                    dat.Execute("UPDATE Users SET PasswordReset='" + code + "' WHERE User_ID="+ds.Tables[0].Rows[0]["User_ID"].ToString());
                    string body = "You have requested to re-set your password with Hippo Happenings. <br/>" +
                        "Please visit <a href=\"http://HippoHappenings.com/ResetPassword.aspx?CODE=" + code + "&UserName=" +
                        ds.Tables[0].Rows[0]["UserName"].ToString() + "\">http://HippoHappenings.com/ResetPassword.aspx?CODE=" + code + "&UserName=" +
                        ds.Tables[0].Rows[0]["UserName"].ToString() + "</a> to do so.";
                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                        ds.Tables[0].Rows[0]["Email"].ToString(), body, "Hippo Happenings Reset Password Request");
                    MessageLabel.Text = "An email with the instructions has been sent to your account.";
                }
                else
                    isNot = true;
            else
                isNot = true;

            if (isNot)
            {
                MessageLabel.Text = "There is no user associated with this email address. Please make sure you have typed it correctly.";
            }
        }
        else
        {
            MessageLabel.Text = "Please include the Email";
        }
        
    }


}
