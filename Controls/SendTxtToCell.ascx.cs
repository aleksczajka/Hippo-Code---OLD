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

public partial class SendTxtToCell : System.Web.UI.UserControl
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
    


}
