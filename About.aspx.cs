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

public partial class About: Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HtmlMeta ds = new HtmlMeta();
            HtmlMeta kw = new HtmlMeta();
            HtmlMeta lg = new HtmlMeta();

            HtmlHead head = (HtmlHead)Page.Header;

            kw.Name = "keywords";
            kw.Content = "About HippoHappenings";
            head.Controls.AddAt(0, kw);

            ds.Name = "description";
            ds.Content = "HippoHappenings provides events and unconventional local happenings";
            head.Controls.AddAt(0, ds);

            lg.Name = "language";
            lg.Content = "English";
            head.Controls.AddAt(0, lg);

            ChangeMission(MissionTimer, new EventArgs());
        }
    }

    protected void ChangeMission(object sender, EventArgs e)
    {
        string strMission = "";
        if (Session["TheMission"] == null)
        {
            Session["TheMission"] = "1";
        }
        strMission = Session["TheMission"].ToString();
        MissionLiteral.Text = "<div class=\"Text12\" style=\"width: 300px; padding-top: 20px; padding-left: 23px;\">";

        switch (strMission)
        {
            case "1":
                MissionLiteral.Text += "<div style=\"font-size: 16px;\" align=\"center\">Let's Get Closer Together</div>" +
                    "<div> <br/>Help the Hippo reach its mission of bringing us closer together by providing content "+
                    "that helps us experience more of what our cities and neighborhoods are about. <br/></div>" +
                    "<div align=\"center\" style=\"padding-top: 5px;\"><img alt=\"Experience happenings in your city and neighborhood\" src=\"images/CloserTogether.png\" /></div>";
                break;
            case "2":
                MissionLiteral.Text += "<div style=\"font-size: 16px;\" align=\"center\">The Hippo Roams Free </div>" +
                    "<div style=\"padding-top: 5px;\">Throwing away all " +
                    "obstacles standing in the way of providing happening adventures, "+
                    "we made posting of event, locales, adventures and bulletins free! All " +
                    "<br/></div><div align=\"center\" style=\"padding-top: 5px;\"><img alt=\"Free event, locale, adventure and bulletin posts\" src=\"images/FreeTheHippo_o.png\" /></div>";
                break;
            case "3":
                MissionLiteral.Text += "<div style=\"font-size: 16px;\" align=\"center\">Take Back our Ad Space! </div>" +
                    "<div class=\"topDiv\"><div style=\"float: left; width: " +
                    "140px;padding-top: 5px; padding-left: 5px;\"><br/>Bulletins on HippoHappenings are nothing like the ads on other sites. They are devoted to you. "+
                    "We want you to use this space to promote yourself, share local news, and absolutely anything neighborhood related. Take back this "+
                    "space from big corporatins and use it to your own means." +
                    "<br/></div><div align=\"center\" style=\"float: left: width: 150px;padding-top: 5px;\">" +
                    "<img alt=\"Ad space devoted to users\" src=\"images/AdSpace.png\" /></div></div>";
                break;
            case "4":
                MissionLiteral.Text += "<div style=\"font-size: 16px;\" align=\"center\">Call Out to The Community! </div>" +
                    "<div class=\"topDiv\"><div style=\"float: left; width: 140px;padding-top: 5px; padding-left: 5px;\">" +
                    "<br/>Use event, locale, adventure and bulletin postings in your city to share what is happening, "+
                    "find a happening event or tell your city about anything local related." +
                    "<br/></div><div align=\"center\" style=\"float: left: width: 150px;padding-top: 5px; margin-right: -3px;\"><img alt=\"Reach your city and neighbors\" src=\"images/CallOut.png\" /></div></div>";
                break;
            case "5":
                MissionLiteral.Text += "<div style=\"font-size: 16px;\" align=\"center\">Grow With Us! </div>" +
                    "<div class=\"topDiv\"><div style=\"float: left; width: 140px;padding-top: 5px; padding-left: 5px;\"><br/>Building a site to benefit the people of our cities "+
                    "would hardly have any value without the people's input. We need you to make us better! Provide us your <a id=\"provide-feedback\" class=\"NavyLink12UD\" href=\"feedback\">feedback and your comments</a>." +
                   "<br/></div><div align=\"center\" style=\"float: " +
                   "left: width: 153px;padding-top: 5px;\">" +
                   "<img src=\"images/GrowWithUs.png\" alt=\"provide feedback\" /></div></div>";
                break;
            default: break;
        }

        MissionLiteral.Text += "</div>";
        int theMission = int.Parse(strMission);
        if (theMission == 5)
            theMission = 1;
        else
            theMission++;

        Session["TheMission"] = theMission.ToString();
    }
}
