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

public partial class GreenButton : System.Web.UI.UserControl, System.Web.UI.WebControls.IButtonControl
{
    public string BUTTON_TEXT
    {
        get { return buttonTextuals; }
        set { buttonTextuals = value; }
    }

    public bool ENABLED
    {
        get { return ButtonText.Enabled; }
        set { ButtonText.Enabled = value; }
    }

    public string CLIENT_CLICK
    {
        get { return ClientClickStr; }
        set { ClientClickStr = value; }
    }

    public EventHandler SERVER_CLICK
    {
        get { return new EventHandler(Page_Load); }
        set { ButtonText.Click += value; gotServerEvents = true; }
    }

    bool IButtonControl.CausesValidation
    {
        get { return theValidation; }
        set { theValidation = value; }
    }

    string IButtonControl.CommandArgument
    {
        get { return theCommandArgument; }
        set { theCommandArgument = value; }
    }

    string IButtonControl.CommandName
    {
        get { return theCommandName; }
        set { theCommandName = value; }
    }

    string IButtonControl.PostBackUrl
    {
        get { return thePostbackUrl; }
        set { thePostbackUrl = value; }
    }

    string IButtonControl.Text
    {
        get { return theText; }
        set { theText = value; }
    }

    public string ValidationGroup
    {
        get { return theValidGroup; }
        set { theValidGroup = value; }
    }

    public string WIDTH
    {
        get { return width; }
        set { width = value; }
    }

    protected string width = "";
    protected string theValidGroup = "";
    protected string theText = "";
    protected string thePostbackUrl = "";
    protected string theCommandName = "";
    protected string theCommandArgument = "";
    protected bool theValidation = false;
    protected string buttonTextuals = "";
    protected string ClientClickStr = "";
    bool gotServerEvents = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        //Take care of setting a size for the button
        string randNext = ButtonText.ClientID;
        WrapperLiteral.Text = "<div align=\"center\" id=\"WrapDiv" + randNext + "\">";
        ScriptLiteral.Text = "<script type=\"text/javascript\">setWidthGreen('" + randNext + "');</script>";
        WidthLiteral.Text = "<div class=\"topDiv aboutLink\" id=\"widthDiv" + randNext + "\">";

        //HtmlForm frm = (HtmlForm)Page.Form;

        //HtmlGenericControl body = (HtmlGenericControl)frm.Parent;

        //if (body.Attributes["onload"] != null)
        //{
        //    body.Attributes["onload"] += "setWidth('" + randNext + "');";
        //}
        //else
        //{
        //    body.Attributes.Add("onload", "setWidth('" + randNext + "');");
        //}

        string theWidth = "";
        if (width != "")
            theWidth = " style=\"width: " + width + "\" ";

        BegLiteral.Text = "<div " + theWidth + ">";

        if (ClientClickStr != "")
        {
            BegLiteral.Text = "<div " + theWidth + " onclick=\"" + ClientClickStr + "\">";
        }
        ButtonText.Text = "<h1 class=\"BlueButton\">" + buttonTextuals + "</h1>";

        if (!gotServerEvents)
        {
            OnlyTextLiteral.Text = "<div class=\"NavyLink\">" + ButtonText.Text + "</div>";
            ButtonText.Visible = false;
        }
    }

    public void SetClientClick()
    {
        BegLiteral.Text = "<div onclick=\"" + ClientClickStr + "\">";
    }

    public GreenButton()
    {
    }

    public event EventHandler Click
    {
        add { ButtonText.Click += new EventHandler(value); }
        remove { ButtonText.Click -= new EventHandler(value); }
    }
    event System.Web.UI.WebControls.CommandEventHandler IButtonControl.Command
    {
        add { }
        remove { }
    }
    public override string ClientID
    {
        get
        {
            return ButtonText.ClientID;
        }
    }
}
