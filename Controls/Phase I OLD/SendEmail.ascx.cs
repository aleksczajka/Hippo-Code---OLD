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

public partial class Controls_SendEmail : System.Web.UI.UserControl
{
    public string RE_LABEL
    {
        get { return subject; }
        set { subject = value; }
    }
    public string THE_TEXT
    {
        get { return text; }
        set
        {
            text = value;
        }
    }
    public string MESSAGE
    {
        get { return message; }
        set { message = value; }
    }
    private string subject = "";
    private string text;
    private string message = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        
      
        //if (IsPostBack)
        //{
        //    MessageRadWindowManager.VisibleOnPageLoad = false;
        //}
    }

    protected void OpenMessage(object sender, EventArgs e)
    {
        Encryption encrypt = new Encryption();
        MessageRadWindow.NavigateUrl = "MessageAlert.aspx?T=Email";
        MessageRadWindow.Visible = true;

        MessageRadWindowManager.VisibleOnPageLoad = true;
    }
}
