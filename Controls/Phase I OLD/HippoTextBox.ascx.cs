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

public partial class Controls_HippoTextBox : System.Web.UI.UserControl
{
    public string CSS_CLASS
    {
        get { return cssClass; }
        set { cssClass = value; }
    }
    public string LITERAL_CSS_CLASS
    {
        get { return literalCssClass; }
        set { literalCssClass = value; }
    }
    public int TEXTBOX_WIDTH
    {
        get { return textBoxWidth; }
        set { textBoxWidth = value; }
    }
    public string THE_TEXT
    {
        get { return TheTextBox.Text; }
        set { TheTextBox.Text = value; }
    }
    public bool IS_MULTILINE
    {
        get { return isMultiline; }
        set { isMultiline = value; }
    }
    public bool IS_WRAP
    {
        get { return isWrap; }
        set { isWrap = value; }
    }
    public string ON_CLIENT_KEYPRESS
    {
        set { TheTextBox.Attributes.Add("onKeyUp", value); }
    }
    private string cssClass;
    private string literalCssClass;
    private int textBoxWidth = 0;
    private bool isMultiline = false;
    private bool isWrap = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TheTextBox.CssClass = cssClass;
            if (textBoxWidth != 0)
                TheTextBox.Width = textBoxWidth;
            TheLiteral.Text = "<div class=\"" + literalCssClass + "\" > ";

            if (isMultiline)
                TheTextBox.TextMode = TextBoxMode.MultiLine;

            TheTextBox.Wrap = isWrap;
        }
    }
}
