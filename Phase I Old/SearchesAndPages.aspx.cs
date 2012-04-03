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
using Telerik.Web.UI;

public partial class SearchesAndPages : Telerik.Web.UI.RadAjaxPage
{
    private void Page_Load(object sender, EventArgs e)
    {
        Session["RedirectTo"] = Request.Url.AbsoluteUri;
        if (Session["User"] == null)
            Response.Redirect("UserLogin.aspx");
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        Session["RedirectTo"] = Request.Url.AbsoluteUri;
        if (Session["User"] == null)
            Response.Redirect("UserLogin.aspx");
        GetSavedSearchAds();
        
            GetAds();
            GetEvents();
            GetVenues();
        
    }

    protected void GetSavedSearchAds()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        AdSearchesPanel.Controls.Clear();
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet dsSearches = dat.GetData("SELECT * FROM SavedAdSearches AS SAS, Countries C WHERE SAS.Country=C.country_id AND SAS.UserID="+Session["User"].ToString()+" ORDER BY SAS.ID");
        DataView dvSearches = new DataView(dsSearches.Tables[0], "", "", DataViewRowState.CurrentRows);
        bool isAlt = false;
        if (dvSearches.Count > 0)
        {
            
            Literal beginningTable2 = new Literal();
            beginningTable2.Text = "<table style=\"margin-top: 20px; margin-bottom: 50px;border-style: "+
                "solid; border-collapse: collapse; border-color: #121010; border-width: 2px; width: 800px;\">"+
                "<tr><th width=\"350px\"><label>Search Specifications</label></th><th "+
                "valign=\"middle\"><table><tr><td><label>Active Search </label></td><td>";
            
            Literal beginningTable = new Literal();
            beginningTable.Text = "</td></tr></table></th><th><table><tr><td><label>Ad Number to Send </label></td><td>";
            Literal b4 = new Literal();
            b4.Text = "</td></tr></table></th><th><label>Remove</label></th></tr>";
            string criteria = "";
            bool hasSearches = false;
            Image img = new Image();
            img.CssClass = "HelpImage";
            img.ImageUrl = "~/image/helpIcon.png";
            img.ID = "QuestionMark1";

            RadToolTip tip = new RadToolTip();
            tip.Text = "<label style=\"color: #cccccc;\"> An active search will " +
                "periodically send you emails when new featrued ads are posted on Hippo Happenings " +
                "that fit your search criteria.</label>";
            tip.Skin = "Black";
            tip.Width = 200;
            tip.ManualClose = true;
            tip.ShowEvent = ToolTipShowEvent.OnClick;
            tip.Position = ToolTipPosition.MiddleRight;
            tip.RelativeTo = ToolTipRelativeDisplay.Element;
            tip.TargetControlID = "QuestionMark1";

            Image img2 = new Image();
            img2.CssClass = "HelpImage";
            img2.ImageUrl = "~/image/helpIcon.png";
            img2.ID = "QuestionMark2";

            RadToolTip tip2 = new RadToolTip();
            tip2.Text = "<label style=\"color: #cccccc;\"> When your search is active"+
                ", this number tells us how many featured ads to gather before sending out the email. For example, if you select \"20\", "+
                "we will wait until there are 20 featured ads matching your search criteria before sending you an email with the ads."+
               " [You could potentially be waiting a long time for 20 ads to gather in our system.] "+
                "If you select \"1\", we will send you an email every time someone posts a featured ad that matches your search criteria.</label>";
            tip2.Skin = "Black";
            tip2.Width = 200;
            tip2.ManualClose = true;
            tip2.ShowEvent = ToolTipShowEvent.OnClick;
            tip2.Position = ToolTipPosition.MiddleRight;
            tip2.RelativeTo = ToolTipRelativeDisplay.Element;
            tip2.TargetControlID = "QuestionMark2";

            AdSearchesPanel.Controls.Add(beginningTable2);
            AdSearchesPanel.Controls.Add(img);
            AdSearchesPanel.Controls.Add(tip);
            AdSearchesPanel.Controls.Add(beginningTable);
            AdSearchesPanel.Controls.Add(img2);
            AdSearchesPanel.Controls.Add(tip2);
            AdSearchesPanel.Controls.Add(b4);
            for (int i = 0; i < dvSearches.Count; i++)
            {
                beginningTable = new Literal();
                criteria = "";
                if(dvSearches[i]["Keywords"] != null)
                    if (dvSearches[i]["Keywords"].ToString().Trim() != "")
                    {
                        criteria += "Keywords: " + dvSearches[i]["Keywords"].ToString().Trim();
                    }
                if (dvSearches[i]["State"] != null)
                    if (dvSearches[i]["State"].ToString().Trim() != "")
                    {
                        criteria += " | State: " + dvSearches[i]["State"].ToString().Trim();
                    }
                if (dvSearches[i]["Country"] != null)
                    if (dvSearches[i]["Country"].ToString().Trim() != "")
                    {
                        criteria += " | Country: " + dvSearches[i]["country_name"].ToString().Trim();
                    }
                if (dvSearches[i]["City"] != null)
                    if (dvSearches[i]["City"].ToString().Trim() != "")
                    {
                        criteria += " | City: " + dvSearches[i]["City"].ToString().Trim();
                    }

                

                if (dvSearches[i]["Zip"] != null)
                    if (dvSearches[i]["Zip"].ToString().Trim() != "")
                    {
                        char[] delim = { ';' };
                        string[] tokens = dvSearches[i]["Zip"].ToString().Trim().Split(delim);

                        criteria += " | Zip: " + tokens[1].Trim();
                    }

                if (dvSearches[i]["Radius"] != null)
                    if (dvSearches[i]["Radius"].ToString().Trim() != "")
                    {
                        criteria += " | Radius: " + dvSearches[i]["Radius"].ToString().Trim();
                    }

                DataSet dsCats = dat.GetData("SELECT * FROM SavedAdSearches_Categories SSC, AdCategories AC WHERE SSC.SearchID="+
                    dvSearches[i]["ID"].ToString()+" AND SSC.CategoryID=AC.ID");
                DataView dvCates = new DataView(dsCats.Tables[0], "", "", DataViewRowState.CurrentRows);

                if (dvCates.Count > 0)
                {
                    if(criteria != "")
                        criteria += "<br/>";
                    criteria += "categories: ";
                    for (int j = 0; j < dvCates.Count; j++)
                    {
                        criteria += dvCates[j]["Name"].ToString();

                        if (j + 1 != dvCates.Count)
                        {
                            criteria += ", ";
                        }
                    }
                }
                

                if (criteria != "")
                {
                    string classA = "isAlt";
                    string classB = "AddGreyLinkAlt";
                    if (!isAlt)
                    {
                        classA = "notAlt";
                        classB = "AddGreyLink";
                        isAlt = true;
                    }
                    else
                    {
                        isAlt = false;
                    }

                    beginningTable.Text += "<tr class=\"" + classA + "\"><td><a class=\"" + classB + 
                        "\" href=\"AdSearch.aspx?search=" + dvSearches[i]["ID"].ToString() + "\">" + 
                        criteria + "</a></td><td align=\"center\">";
                    AdSearchesPanel.Controls.Add(beginningTable);
                    hasSearches = true;
                    bool live = false;
                    if (dvSearches[i]["Live"] != null)
                        if (dvSearches[i]["Live"].ToString().Trim() != "")
                        {
                            live = bool.Parse(dvSearches[i]["Live"].ToString().Trim().ToString());
                            CheckBox check = new CheckBox();
                            check.Checked = live;
                            check.AutoPostBack = true;
                            check.Attributes.Add("value", dvSearches[i]["ID"].ToString());
                            check.CheckedChanged += new EventHandler(check_CheckedChanged);
                            AdSearchesPanel.Controls.Add(check);
                        }

                    beginningTable = new Literal();
                    beginningTable.Text = "</td><td align=\"center\">";
                    AdSearchesPanel.Controls.Add(beginningTable);

                    Literal b2 = new Literal();
                    b2.Text = "</td><td align=\"center\">";

                    if (live)
                    {
                        string numAds = dvSearches[i]["NumAdsInEmail"].ToString().Trim().ToString();
                        DropDownList drop = new DropDownList();
                        drop.Width = 50;
                        drop.ID = "drop" + dvSearches[i]["ID"].ToString();
                        drop.Items.Add(new ListItem("1", "1;" + dvSearches[i]["ID"].ToString()));
                        drop.Items.Add(new ListItem("5", "5;" + dvSearches[i]["ID"].ToString()));
                        drop.Items.Add(new ListItem("10", "10;" + dvSearches[i]["ID"].ToString()));
                        drop.Items.Add(new ListItem("20", "20;" + dvSearches[i]["ID"].ToString()));
                        drop.SelectedValue = numAds + ";" + dvSearches[i]["ID"].ToString();
                        drop.AutoPostBack = true;
                        drop.SelectedIndexChanged += new EventHandler(drop_SelectedIndexChanged);
                        

                        AdSearchesPanel.Controls.Add(drop);
                    }
                    else
                    {
                        Literal b3 = new Literal();
                        b3.Text = "---";
                        b3.ID = "lit" + dvSearches[i]["ID"].ToString();
                        AdSearchesPanel.Controls.Add(b3);
                    }

                    AdSearchesPanel.Controls.Add(b2);

                    LinkButton link = new LinkButton();
                    link.Text = "Remove";
                    link.CssClass = "AddGreyLink";
                    link.CommandArgument = dvSearches[i]["ID"].ToString();
                    link.Click += new EventHandler(removeAdSearch);

                    AdSearchesPanel.Controls.Add(link);


                    beginningTable = new Literal();
                    beginningTable.Text = "</td></tr>";

                    AdSearchesPanel.Controls.Add(beginningTable);
                }

                
            }

            beginningTable = new Literal();

            if (!hasSearches)
            {
                
                beginningTable.Text += "<tr><td colspan=\"3\"><label>You have no saved ad searches. </label></td></tr></table>";
            }
            else
            {
                beginningTable.Text += "</table>";
            }

            AdSearchesPanel.Controls.Add(beginningTable);
        }
        else
        {
            Literal lit = new Literal();
            lit.Text = "<table style=\"margin-top: 20px; margin-bottom: 50px;border-style: solid; border-collapse: collapse; border-color: #121010; border-width: 2px; width: 800px;\">";
            lit.Text += "<tr><td colspan=\"2\"><label>You have no saved ad searches. </label></td></tr></table>";

            AdSearchesPanel.Controls.Add(lit);
        }
    }

    protected void drop_SelectedIndexChanged(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        DropDownList drop = (DropDownList)sender; 
        char[] delim = { ';' };
        string[] tokens = drop.SelectedValue.Split(delim);

        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        dat.Execute("UPDATE SavedAdSearches SET NumAdsInEmail=" + tokens[0] + " WHERE ID=" + tokens[1]);

        Response.Redirect("SearchesAndPages.aspx");
    }

    protected void removeAdSearch(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        LinkButton link = (LinkButton)sender;
        string ID = link.CommandArgument;

        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        dat.Execute("DELETE FROM SavedAdSearches WHERE ID="+ID);
        dat.Execute("DELETE FROM SavedAdSearches_Categories WHERE SearchID=" + ID);

        Response.Redirect("SearchesAndPages.aspx"); //GetSavedSearchAds();
    }

    protected void check_CheckedChanged(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        CheckBox check = (CheckBox)sender;
        string ID = check.Attributes["value"].ToString();
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        

        

        if (check.Checked)
        {
            dat.Execute("UPDATE SavedAdSearches SET Live='True', NumAdsInEmail=1 WHERE ID=" + ID);
            Literal lit = (Literal)AdSearchesPanel.FindControl("lit" + ID);
            DropDownList drop = new DropDownList();
            drop.Width = 50;
            drop.ID = "drop" + ID;
            drop.Items.Add(new ListItem("1 (email sent for every ad created)", "1;" + ID));
            drop.Items.Add(new ListItem("5 (email sent for every 5 ads created)", "5;" + ID));
            drop.Items.Add(new ListItem("10 (every 10 ads)", "10;" + ID));
            drop.Items.Add(new ListItem("20 (every 20: this could potentially be a long waiting period)", "20;" + ID));
            drop.AutoPostBack = true;
            drop.SelectedIndexChanged += new EventHandler(drop_SelectedIndexChanged);

            AdSearchesPanel.Controls.AddAt(AdSearchesPanel.Controls.IndexOf(lit), drop);
            AdSearchesPanel.Controls.Remove(lit);
        }
        else
        {
            dat.Execute("UPDATE SavedAdSearches SET Live='False' WHERE ID=" + ID);
            DropDownList drop = (DropDownList)AdSearchesPanel.FindControl("drop" + ID);
            Literal b3 = new Literal();
            b3.Text = "---";
            AdSearchesPanel.Controls.AddAt(AdSearchesPanel.Controls.IndexOf(drop), b3);
            AdSearchesPanel.Controls.Remove(drop);
        }

        Response.Redirect("SearchesAndPages.aspx");
    }

    protected void ReloadTheAds(object sender, EventArgs e)
    {
        GetAds();
    }

    protected void ReloadTheEvents(object sender, EventArgs e)
    {
        GetEvents();
    }

    protected void ReloadTheVenes(object sender, EventArgs e)
    {
        GetVenues();
    }

    protected void GetAds()
    {
        AdPanel.Controls.Clear();
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet dsAds = dat.GetData("SELECT * FROM Ads WHERE User_ID=" + Session["User"].ToString() + " ORDER BY Header");
        DataView dvAdds = new DataView(dsAds.Tables[0], "", "", DataViewRowState.CurrentRows);

        Panel panel;


        ASP.controls_pager_ascx pagerPanel = new ASP.controls_pager_ascx();


        if (dvAdds.Count > 0)
        {
            pagerPanel.NUMBER_OF_ITEMS_PER_PAGE = 10;
            pagerPanel.WIDTH = 576;
            pagerPanel.NUMBER_OF_VISIBLE_PAGES = 4;
            ArrayList a = new ArrayList(dvAdds.Count);
            int pagecount = 1;
            for (int i = 0; i < dvAdds.Count; i++)
            {
                string featured = "False";
                if (bool.Parse(dvAdds[i]["Featured"].ToString()))
                    featured = "True";
                Literal label = new Literal();

                label.Text = "<tr><td><a class=\"AddGreyLinkAlt\" style=\"padding-left: 10px;\"  " +
                    "href=\"" + dat.MakeNiceName(dvAdds[i]["Header"].ToString()) + "_" +
                    dvAdds[i]["Ad_ID"].ToString() + "_Ad\">" + dvAdds[i]["Header"].ToString() +
                    "</a></td><td><label class=\"AddGreenLink\">" + featured + "</label></td>";

                string disableStr = "";
                if (bool.Parse(dvAdds[i]["LIVE"].ToString()))
                {
                    disableStr = "<td><a class=\"PageButtons\" onclick=\"OpenRadDeleteAd(" + dvAdds[i]["Ad_ID"].ToString() +
                        ")\">Disable</a></td>";
                }
                else
                {
                    disableStr = "<td><a class=\"PageButtons\" onclick=\"OpenRadEnableAd(" + dvAdds[i]["Ad_ID"].ToString() +
                        ")\">Enable</a></td>";
                }

                if (featured == "True")
                {
                    label.Text += "<td><a class=\"AddGreyLinkAlt\" href=\"AdStatistics.aspx?Ad=" +
                        dvAdds[i]["Ad_ID"].ToString() + "\">View Statistics</a></td><td width=\"50px\"><a " +
                        "class=\"PageButtons\" href=\"PostAnAd.aspx?copy=true&ID=" +
                        dvAdds[i]["Ad_ID"].ToString() + "\">Copy</a></td>"+disableStr+"</tr>";
                }
                else
                {
                    label.Text += "<td></td><td width=\"50px\"><a " +
                        "class=\"PageButtons\" href=\"PostAnAd.aspx?copy=true&ID=" +
                        dvAdds[i]["Ad_ID"].ToString() + "\">Copy</a></td>"+disableStr+"</tr>";
                }

                //panel = new Panel();

                //Literal lNew = new Literal();
                if ((i % pagerPanel.NUMBER_OF_ITEMS_PER_PAGE) == 0)
                {
                    label.Text = "<table width=\"576px\"><tbody style=\"opacity: .7; filter: alpha(opacity = 70);background-color: #603E6A;\"><tr><td width=\"400px\"><label>Ad Name</label></td><td><label>Featured?</label></td><td></td><td></td><td></td></tr>"
                    + label.Text;
                    //panel.Controls.Add(lNew);
                }
                //Literal lOld = new Literal();
                //panel.Controls.Add(label);
                if ((i + 1) == 10 * pagecount || i == dvAdds.Count - 1)
                {
                    label.Text = label.Text + "</tbody></table>";
                    pagecount++;
                    //panel.Controls.Add(lOld);
                }
                a.Add(label);
            }
            pagerPanel.DATA = a;
            pagerPanel.DataBind2();

            AdPanel.Controls.Add(pagerPanel);
        }
        else
        {
            pagerPanel.NUMBER_OF_ITEMS_PER_PAGE = 1;
            pagerPanel.WIDTH = 576;
            pagerPanel.NUMBER_OF_VISIBLE_PAGES = 1;
            ArrayList a = new ArrayList(1);

            Literal lit = new Literal();
            lit.Text = "<label>You are not an owner of any ads. To post ads go to <a href=\"PostAnAd.aspx\" class=\"AddLink\">Post An Ad page</a>.</label>";
            a.Add(lit);
            pagerPanel.DATA = a;
            pagerPanel.DataBind2();

            AdPanel.Controls.Add(pagerPanel);
        }


    }

    protected void GetEvents()
    {
        EventsPanel.Controls.Clear();
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet dsAds = dat.GetData("SELECT * FROM Events WHERE Owner=" + Session["User"].ToString() + " ORDER BY Header");
        DataView dvAdds = new DataView(dsAds.Tables[0], "", "", DataViewRowState.CurrentRows);

        Panel panel;

        ASP.controls_pager_ascx pagerPanel = new ASP.controls_pager_ascx();
        if (dvAdds.Count > 0)
        {
            
            pagerPanel.NUMBER_OF_ITEMS_PER_PAGE = 10;
            pagerPanel.WIDTH = 576;
            pagerPanel.NUMBER_OF_VISIBLE_PAGES = 4;
            ArrayList a = new ArrayList(dvAdds.Count);
            int pagecount = 1;
            for (int i = 0; i < dvAdds.Count; i++)
            {
                Literal label = new Literal();
                //label.CssClass = "AddLink";
                string disableStr = "";
                if (bool.Parse(dvAdds[i]["LIVE"].ToString()))
                {
                    disableStr = "<td width=\"50px\"><a class=\"PageButtons\" onclick=\"OpenRadDeleteEvent(" + dvAdds[i]["ID"].ToString() +
                        ")\">Disable</a></td>";
                }
                else
                {
                    disableStr = "<td width=\"50px\"><a class=\"PageButtons\" onclick=\"OpenRadEnableEvent(" + dvAdds[i]["ID"].ToString() +
                        ")\">Enable</a></td>";
                }
                if (i % 10 == 0)
                    label.Text = "<table width=\"576px\"><tbody  style=\"background-color: #628E02;opacity: .8; filter:alpha(opacity = 80);\">";
                label.Text += "<tr><td><a class=\"AddGreyLinkAlt\" style=\"padding-left: 10px;\"  href=\"" +
                    dat.MakeNiceName(dvAdds[i]["Header"].ToString()) + "_" + dvAdds[i]["ID"].ToString() + "_Event\">" +
                    dvAdds[i]["Header"].ToString() + "</a></td><td width=\"50px\"><a style=\"font-family: Arial; font-size: 14px;text-decoration: none; color: #cccccc; font-weight: bold;\" href=\"BlogEvent.aspx?copy=true&ID=" +
                    dvAdds[i]["ID"].ToString() + "\">Copy</a></td>"+disableStr+"</tr>";

                if ((i + 1) == 10 * pagecount || i == dvAdds.Count - 1)
                {
                    label.Text += "</tbody></table>";
                    pagecount++;
                }
                //panel = new Panel();
                //panel.Controls.Add(label);
                a.Add(label);
            }
            pagerPanel.DATA = a;
            pagerPanel.DataBind2();


            EventsPanel.Controls.Add(pagerPanel);
        }
        else
        {
            pagerPanel.NUMBER_OF_ITEMS_PER_PAGE = 1;
            pagerPanel.WIDTH = 576;
            pagerPanel.NUMBER_OF_VISIBLE_PAGES = 1;
            ArrayList a = new ArrayList(1);

            Literal lit = new Literal();
            lit.Text = "<label>You are not an owner of any events. To post events go to <a href=\"EnterEvent.aspx\" class=\"AddLink\">Post An Event page</a>.</label>";
            a.Add(lit);
            pagerPanel.DATA = a;
            pagerPanel.DataBind2();

            EventsPanel.Controls.Add(pagerPanel);
        }
    }

    protected void GetVenues()
    {
        VenuesPanel.Controls.Clear();
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet dsAds = dat.GetData("SELECT * FROM Venues WHERE Owner=" + Session["User"].ToString() + " ORDER BY Name");
        DataView dvAdds = new DataView(dsAds.Tables[0], "", "", DataViewRowState.CurrentRows);

        ASP.controls_pager_ascx pagerPanel = new ASP.controls_pager_ascx();
        if (dvAdds.Count > 0)
        {
            
            pagerPanel.NUMBER_OF_ITEMS_PER_PAGE = 10;
            pagerPanel.WIDTH = 576;
            pagerPanel.NUMBER_OF_VISIBLE_PAGES = 4;
            ArrayList a = new ArrayList(dvAdds.Count);
            int pagecount = 1;
            for (int i = 0; i < dvAdds.Count; i++)
            {
                Literal label = new Literal();
                //label.CssClass = "AddLink";

                if (i % 10 == 0)
                    label.Text = "<table width=\"576px\"><tbody  style=\"opacity: .8; filter: alpha(opacity = 80);background-color: #603E6A;\">";
                label.Text += "<tr><td><a class=\"AddGreyLinkAlt\" style=\"padding-left: 10px;\" href=\"" + 
                    dat.MakeNiceName(dvAdds[i]["Name"].ToString()) + "_" + dvAdds[i]["ID"].ToString() + 
                    "_Venue\">" +
                    dvAdds[i]["Name"].ToString() + "</a></td></tr>";
                if ((i + 1) == 10 * pagecount || i == dvAdds.Count - 1)
                {
                    label.Text += "</tbody></table>";
                    pagecount++;
                }
                //panel = new Panel();
                //panel.Controls.Add(label);
                a.Add(label);
            }
            pagerPanel.DATA = a;
            pagerPanel.DataBind2();

            VenuesPanel.Controls.Add(pagerPanel);
        }
        else
        {
            pagerPanel.NUMBER_OF_ITEMS_PER_PAGE = 1;
            pagerPanel.WIDTH = 576;
            pagerPanel.NUMBER_OF_VISIBLE_PAGES = 1;
            ArrayList a = new ArrayList(1);

            Literal lit = new Literal();
            lit.Text = "<label>You are not an owner of any venues. To post venues go to <a href=\"EnterVenueIntro.aspx\" class=\"AddLink\">Post A Venue page</a>.</label>";
            a.Add(lit);
            pagerPanel.DATA = a;
            pagerPanel.DataBind2();

            VenuesPanel.Controls.Add(pagerPanel);
        }
    }
}
