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

public partial class Vote : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        //Button button = (Button)dat.FindControlRecursive(this, "Button2");
        //button.CssClass = "NavBarImageVoteSelected";

        bool disableAll = false;

        Session["RedirectTo"] = Request.Url.AbsoluteUri;

        if (Session["User"] == null)
        {
            LogInPanel.Visible = true;
            disableAll = true;
        }
        else
        {
            LogInPanel.Visible = false;
            DataView dvVoted = dat.GetDataDV("SELECT * FROM FunctionalityVote WHERE UserID=" + Session["User"].ToString());

            if (dvVoted.Count > 0)
                disableAll = true;
        }

        DataView dvFunc = dat.GetDataDV("SELECT * FROM NewFunctionality ORDER BY SortOrder");

        

        Literal begLit = new Literal();
        begLit.Text = "<table cellspacing='10px'>";

        FuncPanel.Controls.Add(begLit);

        for (int i = 0; i < dvFunc.Count; i++)
        {
            Literal midLit = new Literal();
            midLit.Text = "<tr><td><label><span class=\"AddLink\">" + (i + 1).ToString() +
                "</span></label></td><td><label>" + dvFunc[i]["Content"].ToString() + "</td><td>";

            FuncPanel.Controls.Add(midLit);

            Button midButt = new Button();
            midButt.Text = "Vote for Me!";
            midButt.CommandArgument = dvFunc[i]["ID"].ToString();
            if (disableAll)
                midButt.Enabled = false;
            else
                midButt.Click += new EventHandler(VoteOnIt);

            FuncPanel.Controls.Add(midButt);

            Literal botLit = new Literal();
            botLit.Text = "</label></td></tr>";

            FuncPanel.Controls.Add(botLit);
        }

        Literal endLit = new Literal();
        endLit.Text = "</table>";
        FuncPanel.Controls.Add(endLit);
    }

    protected void VoteOnIt(object sender, EventArgs e)
    {
        Button button = (Button)sender;

        string theID = button.CommandArgument;

        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        dat.Execute("INSERT INTO FunctionalityVote (UserID, FunctionalityID, DateVoted) VALUES(" + Session["User"].ToString() + ", " + theID + ", GETDATE())");

        Encryption encrypt = new Encryption();
        MessageRadWindow.NavigateUrl = "Message.aspx?message=" +
            encrypt.encrypt("<div style=\"height: 200px; vertical-align: middle;\">"+
            "Thank You!<br/><br/>Your vote has been recorded.<br/><br/><button onclick=\"Search('Home.aspx');\""+
            "name=\"Ok\" title=\"Ok\">Ok</button></div>");
        MessageRadWindow.Visible = true;
        MessageRadWindowManager.VisibleOnPageLoad = true;
    }


}
