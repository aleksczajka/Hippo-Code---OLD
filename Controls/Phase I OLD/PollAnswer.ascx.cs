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

public partial class Controls_PollAnswer : System.Web.UI.UserControl
{
    public int POLL_ID
    {
        get { return PollID; }
        set { PollID = value; }
    }
    public int USER_ID
    {
        get { return UserID; }
        set { UserID = value; }
    }
    public int ANSWER_ID
    {
        get { return AnswerID; }
        set { AnswerID = value; }
    }
    private int PollID;
    private int UserID;
    private string Answer;
    private int AnswerID;
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet ds = dat.GetData("SELECT U.UserName, U.User_ID,  U.ProfilePicture ,P.Ans"+AnswerID.ToString()+" As ShortAnswer, PA.Answer, PA.Picture, PA.Video, PA.YouTubeVideo, PA.MediaCategory, PA.Date FROM Polls P, PollAnswers PA, Users U WHERE U.User_ID="+UserID.ToString()+" AND U.User_ID=PA.UserID AND P.ID=PA.PollID AND P.ID="+PollID.ToString());

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                UserNameLabel.NavigateUrl = "~/Friend.aspx?ID=" + ds.Tables[0].Rows[0]["User_ID"].ToString();
                UserNameLabel.Text = ds.Tables[0].Rows[0]["UserName"].ToString();
                AnswerLabel.Text = ds.Tables[0].Rows[0]["ShortAnswer"].ToString();
                DetailedAnswerLabel.Text = ds.Tables[0].Rows[0]["Answer"].ToString();
                UserDateLabel.Text = ds.Tables[0].Rows[0]["Date"].ToString();
                int mediaCategory = int.Parse(ds.Tables[0].Rows[0]["MediaCategory"].ToString());
                if (System.IO.File.Exists(Server.MapPath(".") + "/UserFiles/" +
                    ds.Tables[0].Rows[0]["UserName"].ToString() + "/Profile/" +
                    ds.Tables[0].Rows[0]["ProfilePicture"].ToString()))
                {
                    UserImage.ImageUrl = "~/UserFiles/" +
                    ds.Tables[0].Rows[0]["UserName"].ToString() + "/Profile/" +
                    ds.Tables[0].Rows[0]["ProfilePicture"].ToString();
                }
                else
                {
                    UserImage.ImageUrl = "~/image/noAvatar_50x50_small.png";
                }
                switch (mediaCategory)
                {
                    case 0:
                        break;
                    case 1:
                        MediaPanel.Visible = true;
                        MediaLiteral.Text = "<img style=\"float: left;\" height=\"245px\" width=\"435px\" src=\"UserFiles/" + ds.Tables[0].Rows[0]["UserName"].ToString() +"/Poll/"+ ds.Tables[0].Rows[0]["Picture"].ToString() + "\" />";
                        break;
                    case 2:
                        MediaPanel.Visible = true;
                        MediaLiteral.Text = "<div style=\"float:left; \"><embed  height=\"245px\" width=\"435px\" src=\"UserFiles/" + ds.Tables[0].Rows[0]["UserName"].ToString() + "/Poll/" + ds.Tables[0].Rows[0]["Video"].ToString() + "\" /></div>";
                        break;
                    case 3:
                        MediaPanel.Visible = true;
                        MediaLiteral.Text = "<div style=\"float:left;\"><object width=\"435\" height=\"245\"><param name=\"movie\" value=\"http://www.youtube.com/v/" + ds.Tables[0].Rows[0]["YouTubeVideo"].ToString() + "\"></param><param name=\"allowFullScreen\" value=\"true\"></param><embed src=\"http://www.youtube.com/v/" + ds.Tables[0].Rows[0]["YouTubeVideo"].ToString() + "\" type=\"application/x-shockwave-flash\" allowfullscreen=\"true\" width=\"435\" height=\"245\"></embed></object></div>";
                        break;
                    default: break;
                }

            }
        }
    }
}
