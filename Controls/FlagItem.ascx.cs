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
using System.Net.Mail;

public partial class FlagItem : System.Web.UI.UserControl
{
    public int USER_ID
    {
        get { return User_ID; }
        set { User_ID = value; }
    }
    public string MESSAGE
    {
        get { return message; }
        set { message = value; }
    }
    private int User_ID;
    private string message = "";
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    
    protected void OpenMessage(object sender, EventArgs e)
    {
        string id = "";
        string eType = "";
        if (Request.QueryString["ID"] != null)
        {
            id = Request.QueryString["ID"].ToString();
            eType = "V";
        }
        else if (Request.QueryString["AdID"] != null)
        {
            id = Request.QueryString["AdID"].ToString();
            eType = "A";
        }
        else if (Request.QueryString["EventID"] != null)
        {
            id = Request.QueryString["EventID"].ToString();
            eType = "E";
        }

        Encryption encrypt = new Encryption();
        MessageRadWindow.NavigateUrl = "MessageAlert.aspx?T=Flag&EType="+eType+"&message=" + encrypt.encrypt(message)+"&ID="+id;
        MessageRadWindow.Visible = true;

        MessageRadWindowManager.VisibleOnPageLoad = true;
    }

    //[Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
    //public string encryption()
    //{
    //    return "Controls/MessageAlert.aspx?T=Flag";
    //}
}
