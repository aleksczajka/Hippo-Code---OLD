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

public partial class Controls_SendTxtToCell : System.Web.UI.UserControl
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
        
        //Ajax.Utility.RegisterTypeForAjax(typeof(Controls_SendTxtToCell));
        //if (IsPostBack)
        //{
        //    MessageRadWindowManager.VisibleOnPageLoad = false;
        //}
    }
    

    //protected void OpenMessage(object sender, EventArgs e)
    //{
    //    Encryption encrypt = new Encryption();
    //    MessageRadWindow.NavigateUrl = "MessageAlert.aspx?T=Txt&message=" + encrypt.encrypt(message);
    //    MessageRadWindow.Visible = true;

    //    MessageRadWindowManager.VisibleOnPageLoad = true;
    //}

    //[Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
    //public string encryption()
    //{
    //    Encryption encrypt = new Encryption();
    //    return encrypt.encrypt(message);
    //}
}
