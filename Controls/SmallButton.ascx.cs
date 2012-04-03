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

public partial class SmallButton : System.Web.UI.UserControl, System.Web.UI.WebControls.IButtonControl
{
    public string BUTTON_TEXT
    {
        get { return buttonTextuals; }
        set { buttonTextuals = value; }
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
    protected string theCommandName = "";
    protected string theCommandArgument = "";
    protected bool theValidation = false;
    protected string theValidGroup = "";
    public string ValidationGroup
    {
        get { return theValidGroup; }
        set { theValidGroup = value; }
    }
    string IButtonControl.PostBackUrl
    {
        get { return thePostbackUrl; }
        set { thePostbackUrl = value; }
    }
    protected string thePostbackUrl = "";
    string IButtonControl.Text
    {
        get { return buttonTextuals; }
        set { buttonTextuals = value; }
    }
    public string CLIENT_CLICK
    {
        get { return ClientClickStr; }
        set { ClientClickStr = value; }
    }

    public EventHandler SERVER_CLICK
    {
        get { return new EventHandler(Page_Load); }
        set { ButtonText.Click += value; isClick = true; }
    }

    public string ADD_STYLE
    {
        get { return addStyle; }
        set { addStyle = value; }
    }

    public string CLIENT_LINK_CLICK
    {
        get { return clientLinkClick; }
        set { clientLinkClick = value; }
    }

    protected string clientLinkClick = "";
    protected string addStyle = "";
    protected string buttonTextuals = "";
    protected string ClientClickStr = "";
    protected bool isClick = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        //clear the literal
        BegLiteral.Text = "";

        BegLiteral.Text += "<div>";

        if (ClientClickStr != "" && !isClick)
        {
            if (ClientClickStr != "")
            {
                JustTextLiteral.Text = "<div onclick=\"" + ClientClickStr + "\">";
            }

            JustTextLiteral.Text = "<div class=\"NavyLink12\" onclick=\"" + clientLinkClick + " return true;\">";
            JustTextLiteral.Text += buttonTextuals + "</div>";
        }
        else
        {
            BegLiteral.Text = "<div>";
            if (clientLinkClick != "")
                ButtonText.OnClientClick = clientLinkClick +"; return true;";

            ButtonText.Text = buttonTextuals;
        }

        
        StyleLit.Text = "<div style=\"text-decoration: none; padding-top: " +
            "4px;padding-left: 6px; padding-right: 6px;height: 24px;float: left;background: " +
            "url('NewImages/SmallButtonPixel.png'); background-repeat: repeat-x;" + addStyle + "\">";

        
    }

    public void SetClientClick()
    {
        BegLiteral.Text = "<div onclick=\"" + ClientClickStr + "\">";
    }
    public SmallButton()
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
