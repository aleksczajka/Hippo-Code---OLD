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

public partial class BlueButton : System.Web.UI.UserControl, System.Web.UI.WebControls.IButtonControl
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
        set { theHandler = value; gotServerEvents = true; }
    }

    public string COMMAND_ARGS
    {
        get { return commandArgs; }
        set { commandArgs = value; }
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

    public string CLIENT_LINK_CLICK
    {
        get { return clientLinkClick; }
        set { clientLinkClick = value; }
    }

    public string LINK_ID
    {
        get { return linkID; }
        set { linkID = value; }
    }

    protected string linkID = "";
    protected string clientLinkClick = "";
    protected string width = "";
    protected EventHandler theHandler;
    protected string commandArgs = "";
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
        WrapperLiteral.Text = "<div align=\"center\" id=\"WrapDiv" + randNext+ "\">";
        if (width == "")
            ScriptLiteral.Text = "<script type=\"text/javascript\">setWidth('" + randNext + "');</script>";
        WidthLiteral.Text = "<div class=\"topDiv FloatLeft\" id=\"widthDiv" + randNext + "\">";

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

        if (commandArgs != "")
        {
            ButtonText.CommandArgument = commandArgs;
        }
        string theWidth = "";
        if (width != "")
            theWidth = " style=\"width: " + width + "\" ";

        BegLiteral.Text = "<div " + theWidth + ">";

        if (ClientClickStr != "")
        {
            BegLiteral.Text = "<div " + theWidth + " onclick=\"" + ClientClickStr + "\">";
        }
        ButtonText.Text = "<h1 class=\"BlueButton\">" + buttonTextuals + "</h1>";

        if (clientLinkClick != "")
            ButtonText.OnClientClick = clientLinkClick +"; return true;";

        if (!gotServerEvents)
        {
            OnlyTextLiteral.Text = "<div class=\"NavyLink\">" + ButtonText.Text + "</div>";
            ButtonText.Visible = false;
        }
        else
        {
            ButtonText.Click += theHandler;
        }

        linkID = ButtonText.UniqueID;
    }

    public void SetClientClick()
    {
        BegLiteral.Text = "<div onclick=\"" + ClientClickStr + "\">";
    }

    public void SetAttribute(string theAttribute, string theAttributeValue)
    {
        if (ButtonText.Attributes[theAttribute] == null)
            ButtonText.Attributes[theAttribute] = theAttributeValue;
        else
            ButtonText.Attributes.Add(theAttribute, theAttributeValue);
    }

    public BlueButton()
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
