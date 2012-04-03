using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;

public partial class Controls_Pager : System.Web.UI.UserControl
{
    public bool INCLUDE_TOP_PAGES
    {
        get { return includeTopPages; }
        set { includeTopPages = value; }
    }
    public bool INCLUDE_BOTTOM_PAGES
    {
        get { return includeBottomPages; }
        set { includeBottomPages = value; }
    }
    public int NUMBER_OF_PAGES
    {
        get { return numberOfPages; }
        set { numberOfPages = value; }
    }
    public int NUMBER_OF_ITEMS_PER_PAGE
    {
        get { return numberOfItemsPerPage; }
        set { numberOfItemsPerPage = value; }
    }
    public bool INCLUDE_SCROLL_BARS_VERTICAL
    {
        get { return includeScrollVertical; }
        set { includeScrollVertical = value; }
    }
    public string PANEL_CSSCLASS
    {
        get { return panelCSS; }
        set { panelCSS = value; }
    }
    public ArrayList DATA
    {
        get { return data; }
        set { data = value; }
    }
    public int WIDTH
    {
        get { return theWidth; }
        set { theWidth = value; }
    }
    public int SELECTED_PAGE
    {
        get { return selectedPage; }
        set { selectedPage = value; }
    }
    public int NUMBER_OF_VISIBLE_PAGES
    {
        get { return numOfVisiblePages; }
        set { numOfVisiblePages = value; }
    }
    public string PANEL_NAME
    {
        get { return panelName; }
        set { panelName = value; }
    }
    public string RUN_FUNCTION
    {
        get { return runFunction; }
        set { runFunction = value; }
    }
    private string runFunction = "";
    private string panelName = "";
    private ArrayList data;
    private bool includeTopPages = true;
    private bool includeBottomPages = true;
    private int numberOfPages = 0;
    private int numberOfItemsPerPage = 0;
    private bool includeScrollVertical = false;
    private string panelCSS = "";
    protected Panel EntirePanel;
    public Controls_Pager()
    {
        currentItemCountInPanel = 0;
        currentNumberOfPanels = 0;

    }
    private int currentItemCountInPanel;
    private int currentNumberOfPanels;
    private int numOfVisiblePages = 3;
    private int theWidth = 100;
    private int selectedPage = 2;
    public void Add(Control control, bool linkVisible, int totalControlCount)
    {
        int numerOfPanels = totalControlCount / numberOfItemsPerPage;
        if (totalControlCount > numberOfItemsPerPage * numerOfPanels)
            numerOfPanels++;
        bool enableFirstNext = false;
        if (numerOfPanels > 1)
            enableFirstNext = true;
        string enableFirstNextText = " disabled = 'false' style='display: inline; ' ";
        if (!enableFirstNext)
            enableFirstNextText = "";
        if (currentItemCountInPanel == 0)
        {
            Panel Panel1 = new Panel();

            Panel1.ID = "Panel1";

            string nameOfPanel = panelName;

            if (includeTopPages)
            {
                Literal prevBut = new Literal();
                prevBut.Text = "<a id=\"" + panelName + "prev" + numerOfPanels.ToString() + "1\" disabled=\"true\" class=\"AddPagerPreviousDisabled\" onclick=\"GoTo" + panelName + "(parseInt(selectedNum" + panelName + ") - 1, " +
                    numerOfPanels.ToString() + ", '" + panelName + "')\">Previous</a>";
                //LinkButton previous = new LinkButton();
                //previous.Text = "Previous ";
                //previous.ID = "PreviousLink";
                ////previous.OnClientClick = "this.style.cursor: wait;";
                //previous.Click += new EventHandler(clickPrevious);
                //previous.Attributes.Add("onclick", "this.style.cursor = 'wait';setTimeout(\"document.body.style.cursor = 'hand';\", 2000);");
                //previous.CssClass = "AddPagerPreviousDisabled";
                //previous.Style["cursor"] = "pointer";
                //previous.Enabled = false;

                Label lab = new Label();
                lab.Text = "|";
                lab.CssClass = "PagerPipe";

                Label lab2 = new Label();
                lab2.Text = "pages:&nbsp;&nbsp;";
                lab2.CssClass = "PagesPager";

                Panel topPagerPanel = new Panel();
                topPagerPanel.ID = "topPagerPanel";
                topPagerPanel.CssClass = "TopPagerPanel";
                topPagerPanel.Width = theWidth;

                Literal linkBut = new Literal();

                linkBut.Text = "<a style=\"display: inline\" id=\"" + panelName + "link" + numerOfPanels.ToString() + "1\" class=\"PageLinkSelected\" onclick=\"GoTo" + panelName + "(1, " +
                    numerOfPanels.ToString() + ", '" + panelName + "')\">1</a>";

                //LinkButton link1 = new LinkButton();
                //link1.ID = "link1";
                //link1.CssClass = "PageLinkSelected";
                //link1.CommandArgument = "1";
                //link1.Text = "1";
                //link1.Style["cursor"] = "pointer";
                ////link1.OnClientClick = "this.style.cursor: wait;";
                //link1.CausesValidation = false;
                //link1.Attributes.Add("onclick", "this.style.cursor = 'wait';setTimeout(\"document.body.style.cursor = 'hand';\", 2000);");
                //link1.Attributes.Add("onfocus", "this.style.cursor = 'default';");
                //link1.Click += new EventHandler(this.ChangePage);

                Literal elips = new Literal();
                elips.Text = "<div id=\"" + numerOfPanels +
                    "leftElipsis\" class=\"PageLink\" style=\"display: none;\">...</div>";


                this.Controls.Add(topPagerPanel);
                topPagerPanel.Controls.Add(lab2);
                topPagerPanel.Controls.Add(prevBut);
                topPagerPanel.Controls.Add(elips);
                topPagerPanel.Controls.Add(linkBut);
                topPagerPanel.Controls.Add(lab);

                Literal next = new Literal();
                string nextClass = "AddPagerNextLink";
                if (numerOfPanels == 1)
                    nextClass = "AddLinkNextDisabled";
                next.Text = "<a id=\"" + panelName + "next" + numerOfPanels.ToString() + "1\"  " + 
                    enableFirstNext + " class=\"" + nextClass + "\" onclick=\"GoTo" + panelName + 
                    "(parseInt(selectedNum" + panelName + ") + 1, " +
                    numerOfPanels.ToString() + ", '" + panelName + "')\">Next</a>";

                //LinkButton next = new LinkButton();
                //next.Text = " Next";
                ////next.OnClientClick = "this.style.cursor: wait;";
                //next.Attributes.Add("onclick", "this.style.cursor = 'wait';setTimeout(\"document.body.style.cursor = 'hand';\", 2000);");
                //next.ID = "NextLink";
                //next.Click += new EventHandler(clickNext);
                //next.CssClass = "AddLinkNextDisabled";
                //next.Enabled = false;
                //next.Style["cursor"] = "pointer";
                //next.Attributes.Add("onfocus", "this.style.cursor = 'default';");

                //   next.Attributes.Add("onafterupdate", "this.style.cursor = 'default';");
                //next.Attributes.Add("onchange", "this.style.cursor = 'default';");
                //next.Attributes.Add("oninit", "this.style.cursor = 'default';");
                //next.Attributes.Add("onload", "this.style.cursor = 'default';");
                //next.Attributes.Add("onlosecapture", "this.style.cursor = 'default';");
                //next.Attributes.Add("onprerender", "this.style.cursor = 'default';");
                //next.Attributes.Add("onpropertychange", "this.style.cursor = 'default';");
                //next.Attributes.Add("onreadystatechange", "this.style.cursor = 'default';");
                //next.Attributes.Add("onserverchange", "this.style.cursor = 'default';");
                //next.Attributes.Add("onunload", "this.style.cursor = 'default';");


                topPagerPanel.Controls.Add(next);

                EntirePanel = new Panel();
                if (includeScrollVertical)
                    EntirePanel.ScrollBars = ScrollBars.Vertical;
                EntirePanel.ID = "EntirePanel";


                this.Controls.Add(EntirePanel);
            }

            Panel1.Controls.Add(control);
            if (Panel1.Attributes["style"] == null)
                Panel1.Attributes.Add("style", "display: block;");
            else
                Panel1.Attributes["display"] = "display: block;";

            EntirePanel.Controls.Add(Panel1);

            if (panelCSS != "")
                EntirePanel.CssClass = panelCSS;
            currentItemCountInPanel++;
            currentNumberOfPanels++;


            if (includeBottomPages)
            {

                Literal prevBut = new Literal();
                prevBut.Text = "<a id=\"" + panelName + "prevBottom" + numerOfPanels.ToString() + "1\" disabled=\"true\" class=\"AddPagerPreviousDisabled\" onclick=\"GoTo" + panelName + "(parseInt(selectedNum" + panelName + ") - 1, " +
                    numerOfPanels.ToString() + ", '" + panelName + "')\">Previous</a>";
                //LinkButton previous = new LinkButton();
                //previous.Text = "Previous ";
                ////previous.OnClientClick = "this.style.cursor: wait;";
                //previous.Attributes.Add("onclick", "this.style.cursor = 'wait';setTimeout(\"document.body.style.cursor = 'default';\", 2000);");
                //previous.ID = "PreviousBottomLink";
                //previous.Click += new EventHandler(clickPrevious);
                //previous.CssClass = "AddPagerPreviousDisabled";
                //previous.Enabled = false;
                //previous.Style["cursor"] = "pointer";

                //this.Controls.Add(prevBut);

                Panel topPagerPanel = new Panel();
                topPagerPanel.ID = "bottomPagerPanel";
                topPagerPanel.CssClass = "BottomPagerPanel";
                topPagerPanel.Width = theWidth;

                Literal linkBut = new Literal();
                linkBut.Text = "<a style=\"display: inline;\" " + enableFirstNextText + " id=\"" + panelName + "linkBottom" + numerOfPanels.ToString() + "1\" class=\"PageLinkSelected\" onclick=\"GoTo" + panelName + "(1, " +
                    numerOfPanels.ToString() + ", '" + panelName + "')\">1</a>";

                //LinkButton link1 = new LinkButton();
                //link1.ID = "bottomlink1";
                //link1.CssClass = "PageLinkSelected";
                //link1.CommandArgument = "1";
                //link1.Text = "1";
                //link1.Attributes.Add("onclick", "this.style.cursor = 'wait';setTimeout(\"document.body.style.cursor = 'default';\", 2000);");
                //link1.CausesValidation = false;
                //link1.Click += new EventHandler(this.ChangePage);
                //link1.Style["cursor"] = "pointer";

                Label lab = new Label();
                lab.Text = "|";
                lab.CssClass = "PagerPipe";

                Label lab2 = new Label();
                lab2.Text = "pages:&nbsp;&nbsp;";
                lab2.CssClass = "PagesPager";

                Literal elips = new Literal();
                elips.Text = "<div id=\"" + numerOfPanels + "BleftElipsis\" class=\"PageLink\" style=\"display: none;\">...</div>";


                this.Controls.Add(topPagerPanel);
                topPagerPanel.Controls.Add(lab2);
                topPagerPanel.Controls.Add(elips);
                topPagerPanel.Controls.Add(prevBut);
                topPagerPanel.Controls.Add(linkBut);
                topPagerPanel.Controls.Add(lab);

                string nextBClass = "AddPagerNextLink";
                if (numerOfPanels == 1)
                    nextBClass = "AddLinkNextDisabled";

                Literal nextB = new Literal();
                nextB.Text = "<a id=\"" + panelName + "nextBottom" + numerOfPanels.ToString() + "1\" " + enableFirstNextText + " class=\"" +
                    nextBClass + "\" onclick=\"GoTo" + panelName + "(parseInt(selectedNum" + panelName + ") + 1, " +
                    numerOfPanels.ToString() + ", '" + panelName + "')\">Next</a>";

                //LinkButton next = new LinkButton();
                //next.Text = " Next";
                //next.ID = "NextBottomLink";
                //next.Attributes.Add("onclick", "this.style.cursor = 'wait';setTimeout(\"document.body.style.cursor = 'default';\", 2000);");
                //next.Click += new EventHandler(clickNext);
                //next.CssClass = "AddLinkNextDisabled";
                //next.Enabled = false;
                //next.Style["cursor"] = "pointer";

                topPagerPanel.Controls.Add(nextB);
            }

        }
        else
            if (currentItemCountInPanel <= numberOfItemsPerPage * currentNumberOfPanels)
            {
                string nameOfPanel;

                if (currentItemCountInPanel == numberOfItemsPerPage * currentNumberOfPanels)
                {
                    currentNumberOfPanels++;
                    Panel Panel2 = new Panel();
                    Panel2.ID = "Panel" + currentNumberOfPanels.ToString();
                    Panel2.Attributes.Add("style", "display: none");
                    nameOfPanel = Panel2.UniqueID.Replace(currentNumberOfPanels.ToString(), "");
                    if (includeTopPages)
                    {
                        Panel topPagerPanel = (Panel)FindControlRecursive(this, "topPagerPanel");
                        Literal linkBut2 = new Literal();

                        string disableA = "style=\"display: inline;\"";
                        if (currentNumberOfPanels > 3)
                            disableA = "style=\"display: none;\"";

                        linkBut2.Text = "<a id=\"" + panelName + "link" + numerOfPanels.ToString() + 
                            currentNumberOfPanels.ToString() +
                            "\" class=\"PageLink\" " + disableA + " onclick=\"GoTo" + panelName + "(" +
                            currentNumberOfPanels.ToString() + ", " + numerOfPanels.ToString() +
                            ", '" + panelName + "')\">" +
                            currentNumberOfPanels.ToString() + "</a>";

                        //LinkButton link = new LinkButton();
                        //link.ID = "link" + currentNumberOfPanels.ToString();
                        //link.CausesValidation = false;
                        //link.CssClass = "PageLink";
                        //link.Visible = linkVisible;
                        //link.Text = currentNumberOfPanels.ToString();
                        //link.CommandArgument = currentNumberOfPanels.ToString();
                        //link.Attributes.Add("onclick", "this.style.cursor = 'wait';setTimeout(\"document.body.style.cursor = 'default';\", 2000);");
                        //link.Click += new EventHandler(this.ChangePage);
                        //link.Style["cursor"] = "pointer";


                        //Label lab = new Label();
                        //lab.Text = ", ";
                        //lab.CssClass = "CommaText";

                        //topPagerPanel.Controls.Add(lab);
                        topPagerPanel.Controls.Add(linkBut2);

                        //LinkButton nextLink = (LinkButton)FindControlRecursive(this, "NextLink");
                        //if (nextLink != null)
                        //    topPagerPanel.Controls.Remove(nextLink);

                        //Literal next = new Literal();
                        //next.Text = "<a id=\"next" + numerOfPanels.ToString() + currentNumberOfPanels.ToString() +
                        //    "\" class=\"AddPagerNextLink\" onclick=\"GoTo(selectedNum + 1, " + numerOfPanels.ToString() +
                        //    ", '" + panelName + "')\">Next</a>";
                        ////LinkButton next = new LinkButton();
                        ////next.Text = " Next";
                        ////next.ID = "NextLink";
                        ////next.Attributes.Add("onclick", "this.style.cursor = 'wait';setTimeout(\"document.body.style.cursor = 'default';\", 2000);");
                        ////next.Click += new EventHandler(clickNext);
                        ////next.CssClass = "AddPagerNextLink";
                        ////next.Enabled = true;
                        ////next.Style["cursor"] = "pointer";

                        //topPagerPanel.Controls.Add(next);
                    }


                    Panel2.Controls.Add(control);
                    EntirePanel.Controls.Add(Panel2);

                    if (includeBottomPages)
                    {
                        Panel bottomPagerPanel = (Panel)FindControlRecursive(this, "bottomPagerPanel");

                        Literal linkBut = new Literal();
                        string disableA = "style=\"display: inline;\"";
                        if (currentNumberOfPanels > 3)
                            disableA = "style=\"display: none;\"";
                        linkBut.Text = "<a " + disableA + " id=\"" + panelName + "linkBottom" + numerOfPanels.ToString() + currentNumberOfPanels.ToString() +
                            "\" class=\"PageLink\" onclick=\"GoTo" + panelName + "(" +
                            currentNumberOfPanels.ToString() + ", " + numerOfPanels.ToString() +
                            ", '" + panelName + "')\">" +
                            currentNumberOfPanels.ToString() + "</a>";
                        //LinkButton link = new LinkButton();
                        //link.ID = "bottomlink" + currentNumberOfPanels.ToString();
                        //link.CausesValidation = false;
                        //link.CssClass = "PageLink";
                        //link.Visible = linkVisible;
                        //link.Text = currentNumberOfPanels.ToString();
                        //link.CommandArgument = currentNumberOfPanels.ToString();
                        //link.Attributes.Add("onclick", "this.style.cursor = 'wait';setTimeout(\"document.body.style.cursor = 'default';\", 2000);");
                        //link.Click += new EventHandler(this.ChangePage);
                        //link.Style["cursor"] = "pointer";

                        //Label lab = new Label();
                        //lab.Text = ", ";
                        //lab.CssClass = "CommaText";

                        //bottomPagerPanel.Controls.Add(lab);

                        bottomPagerPanel.Controls.Add(linkBut);

                        //LinkButton nextLink = (LinkButton)FindControlRecursive(this, "NextBottomLink");
                        //if (nextLink != null)
                        //    bottomPagerPanel.Controls.Remove(nextLink);

                        //LinkButton next = new LinkButton();
                        //next.Text = " Next";
                        //next.ID = "NextBottomLink";
                        //next.Click += new EventHandler(clickNext);
                        //next.CssClass = "AddPagerNextLink";
                        //next.Enabled = true;
                        //next.Attributes.Add("onclick", "this.style.cursor = 'wait';setTimeout(\"document.body.style.cursor = 'default';\", 2000);");
                        //next.Style["cursor"] = "pointer";

                        //bottomPagerPanel.Controls.Add(next);
                    }
                }
                else
                {
                    Panel Panel1 = (Panel)FindControlRecursive(this, "Panel" + currentNumberOfPanels.ToString());
                    Panel1.Controls.Add(control);
                }

                currentItemCountInPanel++;
            }

        numberOfPages = currentNumberOfPanels;

    }
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }

    public void BuildScript()
    {
        string addThis = "";
        if (runFunction != "")
            addThis = "var itbe = '" + runFunction + "' + currentPanel; if(eval(\"typeof \" + itbe + \" == 'function'\")) { window['" + runFunction + "' + currentPanel]();}";

        //HttpCookie cookie = Request.Cookies["BrowserDate"];
        //Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":").Replace("%28", "(").Replace("%29", ")")));


        //HtmlControl body = (HtmlControl)dat.FindControlRecursive(this.Page, "bodytag");

        Literal lit = new Literal();

        lit.Text = "\n" +
"var selectedNum" + panelName + " = 1;\n" +
"function GoTo" + panelName + "(currentPanel, numberOfPanels, namePanel)\n" +
"{\n" +
addThis + " \n" +
"   if(currentPanel <= numberOfPanels && currentPanel >= 1)\n" +
"  {\n" +
"     var i;\n" +
"    for(i = 1;i <= numberOfPanels;i++)\n" +
 "   {\n" +
  "      var aPanel = document.getElementById(namePanel+i.toString());\n" +
   "     var aLink = document.getElementById('" + panelName + "link'+numberOfPanels+i.toString());\n" +
    "    var bLink = document.getElementById('" + panelName + "linkBottom'+numberOfPanels+i.toString());\n" +
     "   if(aPanel != null && aPanel != undefined)\n" +
      "  {\n" +
       "     aPanel.style.display = 'none';\n" +
        "    aLink.className = \"PageLink\";\n" +
        "    if(i < currentPanel + 3 && i > currentPanel - 3) \n" +
        "    {\n" +
        "      aLink.style.display = 'inline'; \n" +
        "    }else\n" +
        "    {\n" +
        "      aLink.style.display = 'none'; \n" +
        "    }\n" +
       "     if(bLink != null && bLink != undefined)\n" +
       "    {\n" +
          "             bLink.className = \"PageLink\";\n" +
        "    if(i < currentPanel + 3 && i > currentPanel - 3) \n" +
        "    {\n" +
        "      bLink.style.display = 'inline'; \n" +
        "    }else\n" +
        "    {\n" +
        "      bLink.style.display = 'none'; \n" +
        "    }\n" +
       "    }\n" +

    "    }\n" +
"         }\n" +
"        var firstLink = document.getElementById('" + panelName + "link'+numberOfPanels+'1');\n" +
"        var lastLink = document.getElementById('" + panelName + "link'+numberOfPanels.toString()+numberOfPanels.toString());\n" +
"       var rightElipsis = document.getElementById('" + numberOfPages + "rightElipsis'); \n" +
"       var leftElipsis = document.getElementById('" + numberOfPages + "leftElipsis'); \n" +
"        var BfirstLink = document.getElementById('" + panelName + "linkBottom'+numberOfPanels+'1');\n" +
"        var BlastLink = document.getElementById('" + panelName + "linkBottom'+numberOfPanels.toString()+numberOfPanels.toString());\n" +
"       var BrightElipsis = document.getElementById('" + numberOfPages + "BrightElipsis'); \n" +
"       var BleftElipsis = document.getElementById('" + numberOfPages + "BleftElipsis'); \n" +
 "       if(firstLink.style.display == 'none')\n" +
"           {\n" +
"               if(leftElipsis != null && leftElipsis != undefined) leftElipsis.style.display = 'inline';\n" +
"           }else{\n" +
"              if(leftElipsis != null && leftElipsis != undefined) leftElipsis.style.display = 'none'; \n" +
"           }\n" +
 "       if(lastLink.style.display == 'none')\n" +
"           {\n" +
"               if(rightElipsis != null && rightElipsis != undefined) rightElipsis.style.display = 'inline';\n" +
"           }else{\n" +
"              if(rightElipsis != null && rightElipsis != undefined) rightElipsis.style.display = 'none'; \n" +
"           }\n" +
 "       if(BfirstLink.style.display == 'none')\n" +
"           {\n" +
"               if(BleftElipsis != null && BleftElipsis != undefined) BleftElipsis.style.display = 'inline';\n" +
"           }else{\n" +
"              if(BleftElipsis != null && BleftElipsis != undefined) BleftElipsis.style.display = 'none'; \n" +
"           }\n" +
 "       if(BlastLink.style.display == 'none')\n" +
"           {\n" +
"               if(BrightElipsis != null && BrightElipsis != undefined) BrightElipsis.style.display = 'inline';\n" +
"           }else{\n" +
"              if(BrightElipsis != null && BrightElipsis != undefined) BrightElipsis.style.display = 'none'; \n" +
"           }\n" +
"       var thePanel = document.getElementById(namePanel+currentPanel.toString());\n" +
"      var theLink = document.getElementById('" + panelName + "link'+numberOfPanels+currentPanel.toString());\n" +
"     var theBLink = document.getElementById('" + panelName + "linkBottom'+numberOfPanels+currentPanel.toString());\n" +
"            thePanel.style.display = 'block';\n" +
"           theLink.className = \"PageLinkSelected\";\n" +
"          if(theBLink != null && theBLink != undefined)\n" +
"             theBLink.className = \"PageLinkSelected\";\n" +
"        selectedNum" + panelName + " = currentPanel;\n" +
"       if(currentPanel == numberOfPanels)\n" +
"      {\n" +
"         var nextLink = document.getElementById('" + panelName + "next'+numberOfPanels+'1');\n" +
"        nextLink.className = \"AddLinkNextDisabled\";\n" +
 "       nextLink.disabled = true;\n" +
  "      \n" +
   "     var nextBLink = document.getElementById('" + panelName + "nextBottom'+numberOfPanels+'1');\n" +
    "    if(nextBLink != null && nextBLink != undefined)\n" +
     "   {\n" +
"                    nextBLink.className = \"AddLinkNextDisabled\";\n" +
"                   nextBLink.disabled = true;\n" +
"              }\n" +
"             \n" +
"            if(currentPanel == 1)\n" +
"                {\n" +
"                   var prevLink = document.getElementById('" + panelName + "prev'+numberOfPanels+'1');\n" +
"                  prevLink.className = \"AddPagerPreviousDisabled\";\n" +
"                 prevLink.disabled = true;" +
"                " +
"               var prevBLink = document.getElementById('" + panelName + "prevBottom'+numberOfPanels+'1');\n" +
"              if(prevBLink != null && prevBLink != undefined)\n" +
"             {\n" +
"                prevBLink.className = \"AddPagerPreviousDisabled\";\n" +
 "               prevBLink.disabled = true;\n" +
  "          }\n" +
   "     }else\n" +
    "    {\n" +
"                    var prevLink = document.getElementById('" + panelName + "prev'+numberOfPanels+'1');\n" +
"                   prevLink.className = \"AddPagerPreviousLink\";\n" +
"                  prevLink.disabled = false;\n" +
"                 \n" +
"                var prevBLink = document.getElementById('" + panelName + "prevBottom'+numberOfPanels+'1');\n" +
"               if(prevBLink != null && prevBLink != undefined)\n" +
"              {\n" +
"                 prevBLink.className = \"AddPagerPreviousLink\";\n" +
"                prevBLink.disabled = false;\n" +
 "           }\n" +
  "      }\n" +
   " }\n" +
    "else\n" +
"           {\n" +
"              if(currentPanel == 1)\n" +
"             {\n" +
"                var prevLink = document.getElementById('" + panelName + "prev'+numberOfPanels+'1');\n" +
"               prevLink.className = \"AddPagerPreviousDisabled\";\n" +
"              prevLink.disabled = true;\n" +
"             \n" +
"            var prevBLink = document.getElementById('" + panelName + "prevBottom'+numberOfPanels+'1');\n" +
 "           if(prevBLink != null && prevBLink != undefined)\n" +
  "          {\n" +
   "             prevBLink.className = \"AddPagerPreviousDisabled\";\n" +
    "            prevBLink.disabled = true;\n" +
     "       }\n" +
      "  }else\n" +
"                {\n" +
"                   var prevLink = document.getElementById('" + panelName + "prev'+numberOfPanels+'1');\n" +
"                  prevLink.className = \"AddPagerPreviousLink\";\n" +
"                 prevLink.disabled = false;\n" +
"                \n" +
"               var prevBLink = document.getElementById('" + panelName + "prevBottom'+numberOfPanels+'1');\n" +
"              if(prevBLink != null && prevBLink != undefined)\n" +
"             {\n" +
"                prevBLink.className = \"AddPagerPreviousLink\";\n" +
 "               prevBLink.disabled = false;\n" +
  "          }\n" +
   "     }\n" +
    "    \n" +
     "   var nextLink = document.getElementById('" + panelName + "next'+numberOfPanels+'1');\n" +
      "  nextLink.className = \"AddPagerNextLink\";\n" +
       " nextLink.disabled = false;\n" +
"          \n" +
"           var nextBLink = document.getElementById('" + panelName + "nextBottom'+numberOfPanels+'1');\n" +
"          if(nextBLink != null && nextBLink != undefined)\n" +
"         {\n" +
"            nextBLink.className = \"AddPagerNextLink\";\n" +
 "           nextBLink.disabled = false;\n" +
  "      }\n" +
   " }\n" +
" }\n" +
" }";
        lit.Text = "<script type=\"text/javascript\">" + lit.Text + "</script>";
        this.Page.Header.Controls.Add(lit);
        //TextWriter tw = new StreamWriter(Server.MapPath("~/js/TotalJS.js"), true);

        //// write a line of text to the file
        //tw.WriteLine(lit.Text);

        //// close the stream
        //tw.Close();

        //this.Controls.Add(lit);
    }

    public Control FindControlRecursive(Control Root, string Id)
    {
        if (Root.ID == Id)
            return Root;

        foreach (Control Ctl in Root.Controls)
        {
            Control FoundCtl = FindControlRecursive(Ctl, Id);

            if (FoundCtl != null)
                return FoundCtl;
        }
        return null;
    }
    protected void ChangePage(object sender, EventArgs e)
    {
        
        LinkButton button = (LinkButton)sender;
        
        int argument = int.Parse(button.CommandArgument);
        selectedPage = argument;
        button.CssClass = "PageLinkSelected";
        if (button.ID.Contains("bottom"))
        {
            LinkButton button2 = (LinkButton)FindControlRecursive(this, "link" + argument.ToString());
            button2.CssClass = "PageLinkSelected";
        }
        else
        {
            LinkButton button2 = (LinkButton)FindControlRecursive(this, "bottomlink" + argument.ToString());
            button2.CssClass = "PageLinkSelected";
        }

        changeThePage(argument);
    }

    protected void changeThePage(int argument)
    {
        HttpCookie theCookie = new HttpCookie("SelectedPage", argument.ToString());
        Response.Cookies.Add(theCookie);

        int panelCount = numberOfPages;

        selectedPage = argument;
        Session["PagerSelectedPage"] = selectedPage;
        bool needToHide = false;
        int lowRangeItem = numberOfPages - numOfVisiblePages;
        int highRangeItem = numberOfPages;
        if (numberOfPages > numOfVisiblePages + 3)
        {
            needToHide = true;

            while (!(argument <= highRangeItem && argument > lowRangeItem))
            {
                highRangeItem -= numOfVisiblePages;
                lowRangeItem -= numOfVisiblePages;
                if (lowRangeItem < 0)
                {
                    lowRangeItem = 0;
                    highRangeItem = numberOfPages;
                }
            }


        }

        for (int i = 1; i <= panelCount; i++)
        {
            LinkButton aButton = (LinkButton)FindControlRecursive(this, "link" + i.ToString());
            LinkButton bButton = (LinkButton)FindControlRecursive(this, "bottomlink" + i.ToString());
            aButton.CssClass = "PageLink";
            bButton.CssClass = "PageLink";
            aButton.Style["cursor"] = "pointer";
            bButton.Style["cursor"] = "pointer";

            if (needToHide)
            {
                if (i <= lowRangeItem || i > highRangeItem)
                {
                    aButton.Visible = false;
                    bButton.Visible = false;
                }
                else
                {
                    aButton.Visible = true;
                    bButton.Visible = true;
                }
            }
        }

        LinkButton aButton1 = (LinkButton)FindControlRecursive(this, "link" + argument.ToString());
        LinkButton bButton1 = (LinkButton)FindControlRecursive(this, "bottomlink" + argument.ToString());
        aButton1.CssClass = "PageLinkSelected";
        bButton1.CssClass = "PageLinkSelected";
        bButton1.Style["cursor"] = "pointer";
        aButton1.Style["cursor"] = "pointer";

        string thePanel = "Panel" + argument.ToString();

        for (int i = 1; i <= panelCount; i++)
        {
            Panel panel = (Panel)FindControlRecursive(this, "Panel" + i.ToString());
            panel.Visible = false;
        }

        if (argument == 1)
        {
            if (includeTopPages)
            {
                LinkButton previousLink = (LinkButton)FindControlRecursive(this, "PreviousLink");
                previousLink.CssClass = "AddPagerPreviousDisabled";
                previousLink.Enabled = false;

            }

            if (includeBottomPages)
            {
                LinkButton previousLink = (LinkButton)FindControlRecursive(this, "PreviousBottomLink");
                previousLink.CssClass = "AddPagerPreviousDisabled";
                previousLink.Enabled = false;
            }

            if (argument != numberOfPages)
            {
                if (includeTopPages)
                {
                    LinkButton nextLink = (LinkButton)FindControlRecursive(this, "NextLink");
                    nextLink.CssClass = "AddPagerNextLink";
                    nextLink.Style["cursor"] = "pointer";
                    nextLink.Enabled = true;
                }

                if (includeBottomPages)
                {
                    LinkButton nextLink = (LinkButton)FindControlRecursive(this, "NextBottomLink");
                    nextLink.CssClass = "AddPagerNextLink";
                    nextLink.Style["cursor"] = "pointer";
                    nextLink.Enabled = true;
                }
            }
        }
        if (argument == numberOfPages)
        {
            if (includeTopPages)
            {
                LinkButton previousLink = (LinkButton)FindControlRecursive(this, "NextLink");
                previousLink.CssClass = "AddLinkNextDisabled";
                previousLink.Enabled = false;

                if (argument != 1)
                {
                    LinkButton Link = (LinkButton)FindControlRecursive(this, "PreviousLink");
                    Link.CssClass = "AddPagerPreviousLink";
                    Link.Style["cursor"] = "pointer";
                    Link.Enabled = true;
                }
            }

            if (includeBottomPages)
            {
                LinkButton previousLink = (LinkButton)FindControlRecursive(this, "NextBottomLink");
                previousLink.CssClass = "AddLinkNextDisabled";
                previousLink.Enabled = false;

                if (argument != 1)
                {
                    LinkButton Link = (LinkButton)FindControlRecursive(this, "PreviousBottomLink");
                    Link.CssClass = "AddPagerPreviousLink";
                    Link.Style["cursor"] = "pointer";
                    Link.Enabled = true;
                }
            }
        }

        if (argument != 1 && argument != numberOfPages)
        {
            if (includeTopPages)
            {
                LinkButton previousLink = (LinkButton)FindControlRecursive(this, "PreviousLink");
                previousLink.CssClass = "AddPagerPreviousLink";
                previousLink.Style["cursor"] = "pointer";
                previousLink.Enabled = true;

                LinkButton nextLink = (LinkButton)FindControlRecursive(this, "NextLink");
                nextLink.CssClass = "AddPagerNextLink";
                nextLink.Style["cursor"] = "pointer";
                nextLink.Enabled = true;
            }

            if (includeBottomPages)
            {
                LinkButton previousLink = (LinkButton)FindControlRecursive(this, "PreviousBottomLink");
                previousLink.CssClass = "AddPagerPreviousLink";
                previousLink.Style["cursor"] = "pointer";
                previousLink.Enabled = true;

                LinkButton nextLink = (LinkButton)FindControlRecursive(this, "NextBottomLink");
                nextLink.CssClass = "AddPagerNextLink";
                nextLink.Style["cursor"] = "pointer";
                nextLink.Enabled = true;
            }
        }
        Panel theRealPanel = (Panel)FindControlRecursive(this, thePanel);
        theRealPanel.Visible = true;

    }

    public void DataBind2()
    {
        BuildScript();
        currentItemCountInPanel = 0;
        currentNumberOfPanels = 0;

        this.Controls.Clear();
        bool needToHide = false;
        int lowRangeItem = 0;
        
        int tempNumOfPages = data.Count / numberOfItemsPerPage;
        int highRangeItem = numOfVisiblePages;
        if (tempNumOfPages > numOfVisiblePages + 3)
            needToHide = true;

        if(tempNumOfPages * numberOfItemsPerPage < data.Count)
            tempNumOfPages++;

        bool isVisible = true;

        int i = 1;

        foreach (Control control in data)
        {
            //if (needToHide)
            //{
            //    if (i < lowRangeItem*numberOfItemsPerPage || i > highRangeItem*numberOfItemsPerPage)
            //    {
            //        isVisible = false;
            //    }
            //}
            i++;
            control.ID = i.ToString() + "_OnePagerContent";
            Add(control, isVisible, data.Count);
            isVisible = true;
        }
        Panel topPagerPanel = (Panel)FindControlRecursive(this, "topPagerPanel");
        Literal lit = new Literal();
        if (tempNumOfPages > 3)
        {
            lit.Text = "<div id=\"" + tempNumOfPages + 
                "rightElipsis\" class=\"PageLink\" style=\"display: inline;\">...</div>";
        }
        else
        {
            lit.Text = "<div id=\"" + tempNumOfPages +
                "rightElipsis\" class=\"PageLink\" style=\"display: none;\">...</div>";
        }
        if(topPagerPanel != null)
            topPagerPanel.Controls.Add(lit);
        if (includeBottomPages)
        {
            Panel bottomPagerPanel = (Panel)FindControlRecursive(this, "bottomPagerPanel");
            Literal lit2 = new Literal();
            if (tempNumOfPages > 4)
            {
                lit2.Text = "<div id=\"" + tempNumOfPages +
                    "BrightElipsis\" class=\"PageLink\" style=\"display: inline;\">...</div>";
            }
            else
            {
                lit2.Text = "<div id=\"" + tempNumOfPages +
                    "BrightElipsis\" class=\"PageLink\" style=\"display: none;\">...</div>";
            }
            if(bottomPagerPanel != null)
                bottomPagerPanel.Controls.Add(lit2);
        }

        try
        {
            if (Request.Cookies["SelectedPage"] != null)
                changeThePage(int.Parse(Response.Cookies["SelectedPage"].Value));
        }
        catch (Exception ex)
        {

        }
    }
    protected void clickNext(object sender, EventArgs e)
    {
        MovePage(1);
    }
    protected void clickPrevious(object sender, EventArgs e)
    {
        MovePage(-1);
    }
    protected void MovePage(int changearoo)
    {
        int selectedPage = 0;
        if (Session["PagerSelectedPage"] != null)
            selectedPage = int.Parse(Session["PagerSelectedPage"].ToString());
        else
            selectedPage = 0;
        //if (includeTopPages)
        //{
        //    Panel topPagerPanel = (Panel)FindControlRecursive(this, "topPagerPanel");
        //    for(int i=0;i<topPagerPanel.Controls.Count;i++)
        //    {
        //        try
        //        {
        //            LinkButton link = (LinkButton)topPagerPanel.Controls[i];
        //            if (link.CssClass == "PageLinkSelected")
        //            {
        //                selectedPage = int.Parse(link.ID.Replace("link", ""));

        //                break;
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    }
        //}
        //if (includeBottomPages)
        //{
        //    Panel bottomPagerPanel = (Panel)FindControlRecursive(this, "bottomPagerPanel");
        //    for (int i = 0; i < bottomPagerPanel.Controls.Count;i++ )
        //    {
        //        try
        //        {
        //            LinkButton link = (LinkButton)bottomPagerPanel.Controls[i];
        //            if (link.CssClass == "PageLinkSelected")
        //            {
        //                selectedPage = int.Parse(link.ID.Replace("bottomlink", ""));
        //                break;
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    }

        //}

        if (changearoo == 1)
        {
            if (changearoo != -1)
            {
                changeThePage(selectedPage + changearoo);
            }
            else
            {
                
            }
        }
        else
        {
            if (!(selectedPage == numberOfPages && changearoo == 1))
                changeThePage(selectedPage + changearoo);
            else
            {

            }
        }

        
    }
}
