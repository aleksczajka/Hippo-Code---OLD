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

public partial class Controls_Poll : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        string cookieName = FormsAuthentication.FormsCookieName;
        HttpCookie authCookie = Context.Request.Cookies[cookieName];

        FormsAuthenticationTicket authTicket = null;
        try
        {
            string group = "";
            if (authCookie != null)
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                group = authTicket.UserData.ToString();
            }

            if (group.Contains("Admin"))
            {
                
                if (group.Contains("User"))
                {
                    
                    UserPanel.Visible = true;
                    NonUserPanel.Visible = false;
                    GetActivePoll();
                    Session["User"] = authTicket.Name;
                }
                else
                {
                    NonUserPanel.Visible = true;
                    UserPanel.Visible = false;
                    AnsweredPanel.Visible = false;
                    GetNonUserPoll();
                }
            }
            else
            {
            }
        }
        catch (Exception ex)
        {
        }

        
    }

    protected void GetActivePoll()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data d = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet ds = new DataSet();
        ds = d.GetData("SELECT * FROM Polls WHERE ActivePoll='True'");
        DataSet dsUser = new DataSet();
        dsUser = d.GetData("SELECT * FROM PollAnswers WHERE UserID="+Session["User"].ToString() + " AND PollID="+ds.Tables[0].Rows[0]["ID"].ToString());

        bool showAnswers = false;

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                PollLiteral.Text = "<a href=\"AnswerPoll.aspx\" class=\"PollQuestion\">" + ds.Tables[0].Rows[0]["Question"].ToString() + "</a>";

                if (dsUser.Tables.Count > 0)
                    if (dsUser.Tables[0].Rows.Count > 0)
                    {
                        UserPanel.Visible = false;
                        AnsweredPanel.Visible = true;

                        DataSet dsAns = d.GetData("SELECT COUNT(*) AS Count FROM PollAnswers WHERE AnswerID=1 AND PollID=" + ds.Tables[0].Rows[0]["ID"].ToString());
                        int oneCount = int.Parse(dsAns.Tables[0].Rows[0]["Count"].ToString());
                        dsAns = d.GetData("SELECT COUNT(*) AS Count FROM PollAnswers WHERE AnswerID=2 AND PollID=" + ds.Tables[0].Rows[0]["ID"].ToString());
                        int twoCount = int.Parse(dsAns.Tables[0].Rows[0]["Count"].ToString());

                        int threeCount = 0;

                        if (ds.Tables[0].Rows[0]["Ans3"] != null)
                            if (ds.Tables[0].Rows[0]["Ans3"].ToString() != "")
                            {
                                dsAns = d.GetData("SELECT COUNT(*) AS Count FROM PollAnswers WHERE AnswerID=3 AND PollID=" + ds.Tables[0].Rows[0]["ID"].ToString());
                                threeCount = int.Parse(dsAns.Tables[0].Rows[0]["Count"].ToString());
                                Ans3NumLabel.Text = ((int)(((Decimal)threeCount / ((Decimal)oneCount + (Decimal)twoCount + (Decimal)threeCount)) * (Decimal)100)).ToString() + "% ";
                                Ans3Label.Text = ds.Tables[0].Rows[0]["Ans3"].ToString();
                            }

                        Ans1NumLabel.Text = ((int)(((Decimal)oneCount / ((Decimal)oneCount + (Decimal)twoCount + (Decimal)threeCount)) * (Decimal)100)).ToString() + "% ";
                        Ans2NumLabel.Text = ((int)(((Decimal)twoCount / ((Decimal)oneCount + (Decimal)twoCount + (Decimal)threeCount)) * (Decimal)100)).ToString() + "% ";

                        Ans1Label.Text = ds.Tables[0].Rows[0]["Ans1"].ToString();
                        Ans2Label.Text = ds.Tables[0].Rows[0]["Ans2"].ToString();
                        

                    }
                    else
                        showAnswers = true;
                else
                    showAnswers = true;

                if (showAnswers)
                {
                    AnsweredPanel.Visible = false;
                    UserPanel.Visible = true;

                    AnswersLiteral.Text = "<label>A. "+ds.Tables[0].Rows[0]["Ans1"].ToString()+"</label><br/>";
                    AnswersLiteral.Text += "<label>B. " + ds.Tables[0].Rows[0]["Ans2"].ToString() + "</label><br/>";


                    if (ds.Tables[0].Rows[0]["Ans3"] != null)
                        if (ds.Tables[0].Rows[0]["Ans3"].ToString() != "")
                        {
                            AnswersLiteral.Text += "<label>C. " + ds.Tables[0].Rows[0]["Ans3"].ToString() + "</label><br/>";
                        }
                }
            }
        }
    }

    protected void GetNonUserPoll()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data d = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet ds = new DataSet();
        ds = d.GetData("SELECT * FROM Polls WHERE ActivePoll='True'");

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                PollLiteral.Text = "<label class=\"PollQuestion\">"+ds.Tables[0].Rows[0]["Question"].ToString()+"</label>";
                
            }
        }
    }

    protected void GoTo(object sender, EventArgs e)
    {
        Response.Redirect("AnswerPoll.aspx");
    }
}
