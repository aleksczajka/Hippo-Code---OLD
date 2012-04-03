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
    public void Add(Control control, bool linkVisible)
    {
        if (currentItemCountInPanel == 0)
        {
            if (includeTopPages)
            {
                LinkButton previous = new LinkButton();
                previous.Text = "Previous ";
                previous.ID = "PreviousLink";
                //previous.OnClientClick = "this.style.cursor: wait;";
                previous.Click += new EventHandler(clickPrevious);
                previous.Attributes.Add("onclick", "this.style.cursor = 'wait';setTimeout(\"document.body.style.cursor = 'hand';\", 2000);");
                previous.CssClass = "AddPagerPreviousDisabled";
                previous.Style["cursor"] = "pointer";
                previous.Enabled = false;

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

                LinkButton link1 = new LinkButton();
                link1.ID = "link1";
                link1.CssClass = "PageLinkSelected";
                link1.CommandArgument = "1";
                link1.Text = "1";
                link1.Style["cursor"] = "pointer";
                //link1.OnClientClick = "this.style.cursor: wait;";
                link1.CausesValidation = false;
                link1.Attributes.Add("onclick", "this.style.cursor = 'wait';setTimeout(\"document.body.style.cursor = 'hand';\", 2000);");
                link1.Attributes.Add("onfocus", "this.style.cursor = 'default';");
                link1.Click += new EventHandler(this.ChangePage);


                this.Controls.Add(topPagerPanel);
                topPagerPanel.Controls.Add(lab2);
                topPagerPanel.Controls.Add(previous);
                topPagerPanel.Controls.Add(link1);
                topPagerPanel.Controls.Add(lab);
                

                LinkButton next = new LinkButton();
                next.Text = " Next";
                //next.OnClientClick = "this.style.cursor: wait;";
                next.Attributes.Add("onclick", "this.style.cursor = 'wait';setTimeout(\"document.body.style.cursor = 'hand';\", 2000);");
                next.ID = "NextLink";
                next.Click += new EventHandler(clickNext);
                next.CssClass = "AddLinkNextDisabled";
                next.Enabled = false;
                next.Style["cursor"] = "pointer";
                next.Attributes.Add("onfocus", "this.style.cursor = 'default';");
                          
                    next.Attributes.Add("onafterupdate", "this.style.cursor = 'default';");
                 next.Attributes.Add("onchange", "this.style.cursor = 'default';");
                 next.Attributes.Add("oninit", "this.style.cursor = 'default';");
                 next.Attributes.Add("onload", "this.style.cursor = 'default';");
                 next.Attributes.Add("onlosecapture", "this.style.cursor = 'default';");
                 next.Attributes.Add("onprerender", "this.style.cursor = 'default';");
                 next.Attributes.Add("onpropertychange", "this.style.cursor = 'default';");
                 next.Attributes.Add("onreadystatechange", "this.style.cursor = 'default';");
                 next.Attributes.Add("onserverchange", "this.style.cursor = 'default';");
                 next.Attributes.Add("onunload", "this.style.cursor = 'default';");


                topPagerPanel.Controls.Add(next);

                EntirePanel = new Panel();
                if (includeScrollVertical)
                    EntirePanel.ScrollBars = ScrollBars.Vertical;
                EntirePanel.ID = "EntirePanel";


                this.Controls.Add(EntirePanel);
            }

            Panel Panel1 = new Panel();
            
            Panel1.ID = "Panel1";
            Panel1.Controls.Add(control);
            EntirePanel.Controls.Add(Panel1);
            if (panelCSS != "")
                EntirePanel.CssClass = panelCSS;
            currentItemCountInPanel++;
            currentNumberOfPanels++;

            
            if (includeBottomPages)
            {
                LinkButton previous = new LinkButton();
                previous.Text = "Previous ";
                //previous.OnClientClick = "this.style.cursor: wait;";
                previous.Attributes.Add("onclick", "this.style.cursor = 'wait';setTimeout(\"document.body.style.cursor = 'default';\", 2000);");
                previous.ID = "PreviousBottomLink";
                previous.Click += new EventHandler(clickPrevious);
                previous.CssClass = "AddPagerPreviousDisabled";
                previous.Enabled = false;
                previous.Style["cursor"] = "pointer";

                this.Controls.Add(previous);

                Panel topPagerPanel = new Panel();
                topPagerPanel.ID = "bottomPagerPanel";
                topPagerPanel.CssClass = "BottomPagerPanel";
                topPagerPanel.Width = theWidth;

                LinkButton link1 = new LinkButton();
                link1.ID = "bottomlink1";
                link1.CssClass = "PageLinkSelected";
                link1.CommandArgument = "1";
                link1.Text = "1";
                link1.Attributes.Add("onclick", "this.style.cursor = 'wait';setTimeout(\"document.body.style.cursor = 'default';\", 2000);");
                link1.CausesValidation = false;
                link1.Click += new EventHandler(this.ChangePage);
                link1.Style["cursor"] = "pointer";

                Label lab = new Label();
                lab.Text = "|";
                lab.CssClass = "PagerPipe";

                Label lab2 = new Label();
                lab2.Text = "pages:&nbsp;&nbsp;";
                lab2.CssClass = "PagesPager";

                this.Controls.Add(topPagerPanel);
                topPagerPanel.Controls.Add(lab2);
                topPagerPanel.Controls.Add(previous);
                topPagerPanel.Controls.Add(link1);
                topPagerPanel.Controls.Add(lab);

                LinkButton next = new LinkButton();
                next.Text = " Next";
                next.ID = "NextBottomLink";
                next.Attributes.Add("onclick", "this.style.cursor = 'wait';setTimeout(\"document.body.style.cursor = 'default';\", 2000);");
                next.Click += new EventHandler(clickNext);
                next.CssClass = "AddLinkNextDisabled";
                next.Enabled = false;
                next.Style["cursor"] = "pointer";

                topPagerPanel.Controls.Add(next);
            }

        }else
            if (currentItemCountInPanel <= numberOfItemsPerPage*currentNumberOfPanels)
            {



                if (currentItemCountInPanel == numberOfItemsPerPage * currentNumberOfPanels)
                {
                    currentNumberOfPanels++;
                    Panel Panel2 = new Panel();
                    Panel2.ID = "Panel" + currentNumberOfPanels.ToString();
                    Panel2.Visible = false;
                    EntirePanel.Controls.Add(Panel2);

                    if (includeTopPages)
                    {
                        Panel topPagerPanel = (Panel)FindControlRecursive(this, "topPagerPanel");
                        LinkButton link = new LinkButton();
                        link.ID = "link" + currentNumberOfPanels.ToString();
                        link.CausesValidation = false;
                        link.CssClass = "PageLink";
                        link.Visible = linkVisible;
                        link.Text = currentNumberOfPanels.ToString();
                        link.CommandArgument = currentNumberOfPanels.ToString();
                        link.Attributes.Add("onclick", "this.style.cursor = 'wait';setTimeout(\"document.body.style.cursor = 'default';\", 2000);");
                        link.Click += new EventHandler(this.ChangePage);
                        link.Style["cursor"] = "pointer";
                        

                        Label lab = new Label();
                        lab.Text = ", ";
                        lab.CssClass = "CommaText";

                        topPagerPanel.Controls.Add(lab);
                        topPagerPanel.Controls.Add(link);

                        LinkButton nextLink = (LinkButton)FindControlRecursive(this, "NextLink");
                        if (nextLink != null)
                            topPagerPanel.Controls.Remove(nextLink);

                        LinkButton next = new LinkButton();
                        next.Text = " Next";
                        next.ID = "NextLink";
                        next.Attributes.Add("onclick", "this.style.cursor = 'wait';setTimeout(\"document.body.style.cursor = 'default';\", 2000);");
                        next.Click += new EventHandler(clickNext);
                        next.CssClass = "AddPagerNextLink";
                        next.Enabled = true;
                        next.Style["cursor"] = "pointer";

                        topPagerPanel.Controls.Add(next);
                    }

                    Panel Panel1 = (Panel)FindControlRecursive(this, "Panel" + currentNumberOfPanels.ToString());

                    Panel1.Controls.Add(control);

                    if (includeBottomPages)
                    {
                        Panel bottomPagerPanel = (Panel)FindControlRecursive(this, "bottomPagerPanel");
                        LinkButton link = new LinkButton();
                        link.ID = "bottomlink" + currentNumberOfPanels.ToString();
                        link.CausesValidation = false;
                        link.CssClass = "PageLink";
                        link.Visible = linkVisible;
                        link.Text = currentNumberOfPanels.ToString();
                        link.CommandArgument = currentNumberOfPanels.ToString();
                        link.Attributes.Add("onclick", "this.style.cursor = 'wait';setTimeout(\"document.body.style.cursor = 'default';\", 2000);");
                        link.Click += new EventHandler(this.ChangePage);
                        link.Style["cursor"] = "pointer";

                        Label lab = new Label();
                        lab.Text = ", ";
                        lab.CssClass = "CommaText";

                        bottomPagerPanel.Controls.Add(lab);

                        bottomPagerPanel.Controls.Add(link);

                        LinkButton nextLink = (LinkButton)FindControlRecursive(this, "NextBottomLink");
                        if (nextLink != null)
                            bottomPagerPanel.Controls.Remove(nextLink);

                        LinkButton next = new LinkButton();
                        next.Text = " Next";
                        next.ID = "NextBottomLink";
                        next.Click += new EventHandler(clickNext);
                        next.CssClass = "AddPagerNextLink";
                        next.Enabled = true;
                        next.Attributes.Add("onclick", "this.style.cursor = 'wait';setTimeout(\"document.body.style.cursor = 'default';\", 2000);");
                        next.Style["cursor"] = "pointer";

                        bottomPagerPanel.Controls.Add(next);
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

    protected void Page_Unload(object sender, EventArgs e)
    {
       
    }

    protected void changeThePage(int argument)
    {
        HttpCookie theCookie = new HttpCookie("SelectedPage", argument.ToString());
        Response.Cookies.Add(theCookie);

        int panelCount = numberOfPages;

        selectedPage = argument;

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
        this.Controls.Clear();
        bool needToHide = false;
        int lowRangeItem = 0;
        
        int tempNumOfPages = data.Count / numberOfItemsPerPage;
        int highRangeItem = numOfVisiblePages;
        if (tempNumOfPages > numOfVisiblePages + 3)
            needToHide = true;

        bool isVisible = true;

        int i = 1;

        foreach (Control control in data)
        {
            if (needToHide)
            {
                if (i < lowRangeItem*numberOfItemsPerPage || i > highRangeItem*numberOfItemsPerPage)
                {
                    isVisible = false;
                }
            }
            i++;
            control.ID = i.ToString() + "_OnePagerContent";
            Add(control, isVisible);
            isVisible = true;
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
        if (includeTopPages)
        {
            Panel topPagerPanel = (Panel)FindControlRecursive(this, "topPagerPanel");
            for(int i=0;i<topPagerPanel.Controls.Count;i++)
            {
                try
                {
                    LinkButton link = (LinkButton)topPagerPanel.Controls[i];
                    if (link.CssClass == "PageLinkSelected")
                    {
                        selectedPage = int.Parse(link.ID.Replace("link", ""));

                        break;
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
        if (includeBottomPages)
        {
            Panel bottomPagerPanel = (Panel)FindControlRecursive(this, "bottomPagerPanel");
            for (int i = 0; i < bottomPagerPanel.Controls.Count;i++ )
            {
                try
                {
                    LinkButton link = (LinkButton)bottomPagerPanel.Controls[i];
                    if (link.CssClass == "PageLinkSelected")
                    {
                        selectedPage = int.Parse(link.ID.Replace("bottomlink", ""));
                        break;
                    }
                }
                catch (Exception ex)
                {

                }
            }

        }

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
