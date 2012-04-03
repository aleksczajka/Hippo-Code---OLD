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

public partial class AnswerPoll : Telerik.Web.UI.RadAjaxPage
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
                    Session["User"] = authTicket.Name;
                    Session["UserName"] = dat.GetData("SELECT UserName FROM USERS WHERE User_ID="+authTicket.Name).Tables[0].Rows[0]["UserName"].ToString();
                    if(!IsPostBack)
                        SetPoll();
                }
                else
                {
                    if(!IsPostBack)
                        SetNotAnsweredPoll();
                }
            }
            else
            {
                ImageButton calendarLink = (ImageButton)dat.FindControlRecursive(this, "CalendarLink");
                calendarLink.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }

    }

    protected void SetNotAnsweredPoll()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet ds = dat.GetData("SELECT * FROM Polls WHERE ActivePoll='True'");

        YourAnswerPanel.Visible = false;

        int pollID = -1;
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                PollQuestionLabel.Text = ds.Tables[0].Rows[0]["Question"].ToString();

                pollID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                DataSet dsUser = new DataSet();
                dsUser = dat.GetData("SELECT * FROM PollAnswers P, Users U WHERE P.UserID=U.User_ID AND P.UserID=" + Session["User"].ToString());

                    PollPanel.Visible = true;

                    AnswerRadioList.Items.Add(new ListItem(ds.Tables[0].Rows[0]["Ans1"].ToString(), "1"));
                    AnswerRadioList.Items.Add(new ListItem(ds.Tables[0].Rows[0]["Ans2"].ToString(), "2"));
                    if (ds.Tables[0].Rows[0]["Ans3"] != null)
                        if (ds.Tables[0].Rows[0]["Ans3"].ToString() != "")
                            AnswerRadioList.Items.Add(new ListItem(ds.Tables[0].Rows[0]["Ans3"].ToString(), "3"));

                    if (pollID != -1)
                    {
                        PollAnswers.POLL_ID = pollID;
                        PollAnswers.DataBind2();
                    }
                
            }
        }
    }

    protected void SetPoll()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet ds = dat.GetData("SELECT * FROM Polls WHERE ActivePoll='True'");

        YourAnswerPanel.Visible = false;

        int pollID = -1;
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                PollQuestionLabel.Text = ds.Tables[0].Rows[0]["Question"].ToString();

                pollID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                Session["PollID"] = pollID.ToString();
                DataSet dsUser = new DataSet();
                dsUser = dat.GetData("SELECT * FROM PollAnswers P, Users U, Polls Po WHERE Po.ID=P.PollID AND P.UserID=U.User_ID AND U.User_ID=" + Session["User"].ToString() + " AND P.PollID="+pollID);

                DataSet dsTotal = new DataSet();
                dsTotal = dat.GetData("SELECT * FROM PollAnswers WHERE PollID="+pollID);

                bool notAnswered = false;

                if (dsUser.Tables.Count > 0)
                    if (dsUser.Tables[0].Rows.Count > 0)
                    {
                        PollPanel.Visible = false;
                        YourAnswerPanel.Visible = true;
                        AnswerLabel.Text = dsUser.Tables[0].Rows[0]["Answer"].ToString();
                        ShortAnswerLabel.Text = dsUser.Tables[0].Rows[0]["Ans" + dsUser.Tables[0].Rows[0]["AnswerID"].ToString()].ToString();
                        DateLabel.Text = dsUser.Tables[0].Rows[0]["Date"].ToString();
                        AnsweredPanel.Visible = true;
                        TotalUsersLabel.Text = dsTotal.Tables[0].Rows.Count.ToString();
                        DataSet dsAns = dat.GetData("SELECT COUNT(*) AS Count FROM PollAnswers WHERE AnswerID=1 AND PollID=" + ds.Tables[0].Rows[0]["ID"].ToString());
                        int oneCount = int.Parse(dsAns.Tables[0].Rows[0]["Count"].ToString());
                        dsAns = dat.GetData("SELECT COUNT(*) AS Count FROM PollAnswers WHERE AnswerID=2 AND PollID=" + ds.Tables[0].Rows[0]["ID"].ToString());
                        int twoCount = int.Parse(dsAns.Tables[0].Rows[0]["Count"].ToString());

                        int threeCount = 0;

                        if (ds.Tables[0].Rows[0]["Ans3"] != null)
                            if (ds.Tables[0].Rows[0]["Ans3"].ToString() != "")
                            {
                                dsAns = dat.GetData("SELECT COUNT(*) AS Count FROM PollAnswers WHERE AnswerID=3 AND PollID=" + ds.Tables[0].Rows[0]["ID"].ToString());
                                threeCount = int.Parse(dsAns.Tables[0].Rows[0]["Count"].ToString());
                                Ans3NumLabel.Text = ((int)(((Decimal)threeCount / ((Decimal)oneCount + (Decimal)twoCount + (Decimal)threeCount)) * (Decimal)100)).ToString() + "% ";
                                Ans3Label.Text = ds.Tables[0].Rows[0]["Ans3"].ToString();
                            }

                        Ans1NumLabel.Text = ((int)(((Decimal)oneCount / ((Decimal)oneCount + (Decimal)twoCount + (Decimal)threeCount)) * (Decimal)100)).ToString() + "% ";
                        Ans2NumLabel.Text = ((int)(((Decimal)twoCount / ((Decimal)oneCount + (Decimal)twoCount + (Decimal)threeCount)) * (Decimal)100)).ToString() + "% ";

                        Ans1Label.Text = ds.Tables[0].Rows[0]["Ans1"].ToString();
                        Ans2Label.Text = ds.Tables[0].Rows[0]["Ans2"].ToString();

                        int mediaCategory = int.Parse(dsUser.Tables[0].Rows[0]["MediaCategory"].ToString());

                        switch (mediaCategory)
                        {
                            case 0:
                                break;
                            case 1:
                                MediaLiteral.Text = "<img style=\"float: left;\" height=\"245px\" width=\"435px\" src=\"UserFiles/" + dsUser.Tables[0].Rows[0]["UserName"].ToString() + "/Poll/" + dsUser.Tables[0].Rows[0]["Picture"].ToString() + "\" />";
                                break;
                            case 2:
                                MediaLiteral.Text = "<div style=\"float:left; \"><embed  height=\"245px\" width=\"435px\" src=\"UserFiles/" + dsUser.Tables[0].Rows[0]["UserName"].ToString() + "/Poll/" + dsUser.Tables[0].Rows[0]["Video"].ToString() + "\" /></div>";
                                break;
                            case 3:
                                MediaLiteral.Text = "<div style=\"float:left;\"><object width=\"435\" height=\"245\"><param name=\"movie\" value=\"http://www.youtube.com/v/" + dsUser.Tables[0].Rows[0]["YouTubeVideo"].ToString() + "\"></param><param name=\"allowFullScreen\" value=\"true\"></param><embed src=\"http://www.youtube.com/v/" + dsUser.Tables[0].Rows[0]["YouTubeVideo"].ToString() + "\" type=\"application/x-shockwave-flash\" allowfullscreen=\"true\" width=\"435\" height=\"245\"></embed></object></div>";
                                break;
                            default: break;
                        }

                        if (pollID != -1)
                        {
                            PollAnswers.POLL_ID = pollID;
                            PollAnswers.DataBind2();
                        }
                    }
                    else
                        notAnswered = true;
                else
                    notAnswered = true;

                if (notAnswered)
                {
                    PollPanel.Visible = true;
                    
                    AnswerRadioList.Items.Add(new ListItem(ds.Tables[0].Rows[0]["Ans1"].ToString(), "1"));
                    AnswerRadioList.Items.Add(new ListItem(ds.Tables[0].Rows[0]["Ans2"].ToString(), "2"));
                    if (ds.Tables[0].Rows[0]["Ans3"] != null)
                        if (ds.Tables[0].Rows[0]["Ans3"].ToString() != "")
                            AnswerRadioList.Items.Add(new ListItem(ds.Tables[0].Rows[0]["Ans3"].ToString(), "3"));

                }
            }
        }
    }

    protected void ChangePanels(object sender, EventArgs e)
    {
        switch(MediaRadioList.SelectedValue)
        {
            case "0":
                PicturePanel.Visible = true;
                VideoPanel.Visible = false;
                YouTubePanel.Visible = false;
                break;
            case "1":
                PicturePanel.Visible = false;
                VideoPanel.Visible = true;
                YouTubePanel.Visible = false;
                break;
            case "2":
                PicturePanel.Visible = false;
                VideoPanel.Visible = false;
                YouTubePanel.Visible = true;
                break;
            default: break;
        }
    }

    protected void PictureUpload(object sender, EventArgs e)
    {
        if(PictureFileUpload.HasFile)
        {
            char[] delim = { '.' };
            string[] tokens = PictureFileUpload.FileName.Split(delim);
            if (tokens.Length > 1)
            {
                if (tokens[1].ToUpper() == "JPG" || tokens[1].ToUpper() == "JPEG" || tokens[1].ToUpper() == "GIF" || tokens[1].ToUpper() == "PNG")
                {
                    StatusLabel.Text = "Picture: '" + PictureFileUpload.FileName + "' Uploaded";
                    PictureFileUpload.SaveAs(MapPath(".").ToString() + "/UserFiles/" + Session["UserName"].ToString() + "/Poll/" + PictureFileUpload.FileName);
                    Session["PictureName"] = PictureFileUpload.FileName;
                }
                else
                {
                    StatusLabel.Text = "Picture not uploaded. File must have format jpg, jpeg, gif, or png.";
                }
            }
            else
            {
                StatusLabel.Text = "Picture not uploaded. File must have format jpg, jpeg, gif, or png.";
            }
        }
    }

    protected void VideoUpload(object sender, EventArgs e)
    {
        if (VideoFileUpload.HasFile)
        {
            char[] delim = { '.' };
            string[] tokens = VideoFileUpload.FileName.Split(delim);
            if (tokens[1].ToUpper() == "AVI" || tokens[1].ToUpper() == "WMV")
            {
                StatusLabel.Text = "Video: '" + VideoFileUpload.FileName + "' Uploaded";
                VideoFileUpload.SaveAs(MapPath(".").ToString() + "/UserFiles/" + Session["UserName"].ToString() + "/Poll/" + VideoFileUpload.FileName);
                Session["VideoName"] = VideoFileUpload.FileName;
            }
            else
            {
                StatusLabel.Text = "Video not uploaded. File must be either avi or wmv.";
            }
        }
    }

    protected void YouTubeUpload(object sender, EventArgs e)
    {
        if (YouTubeTextBox.Text != "")
        {
            StatusLabel.Text = "Video: '" + YouTubeTextBox.Text + "' Uploaded";
            Session["YouTubeName"] = YouTubeTextBox.Text;
        }
    }

    protected void PostAnswer(object sender, EventArgs e)
    {
        if (AnswerRadioList.SelectedValue != null)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO PollAnswers (UserID, PollID, AnswerID, Answer, MediaCategory, Picture, Video, YouTubeVideo, Date) "+
                " VALUES(@userID, @pollID, @answerID, @answer, @cat, @pic, @vid, @tube, GETDATE())", conn);
            cmd.Parameters.Add("@userID", SqlDbType.Int).Value = int.Parse(Session["User"].ToString());
            cmd.Parameters.Add("@pollID", SqlDbType.Int).Value = int.Parse(Session["PollID"].ToString());
            cmd.Parameters.Add("@answerID", SqlDbType.Int).Value = int.Parse(AnswerRadioList.SelectedValue);
            cmd.Parameters.Add("@answer", SqlDbType.NVarChar).Value = AnswerTextBox.Text;

            if (MediaRadioList.SelectedValue != null)
            {
                if (MediaRadioList.SelectedValue != "")
                {
                    cmd.Parameters.Add("@cat", SqlDbType.Int).Value = int.Parse(MediaRadioList.SelectedValue) + 1;

                    switch (MediaRadioList.SelectedValue)
                    {
                        case "0":
                            cmd.Parameters.Add("@pic", SqlDbType.NVarChar).Value = Session["PictureName"].ToString();
                            cmd.Parameters.Add("@vid", SqlDbType.NVarChar).Value = DBNull.Value;
                            cmd.Parameters.Add("@tube", SqlDbType.NVarChar).Value = DBNull.Value;
                            break;
                        case "1":
                            cmd.Parameters.Add("@pic", SqlDbType.NVarChar).Value = DBNull.Value;
                            cmd.Parameters.Add("@vid", SqlDbType.NVarChar).Value = Session["VideoName"].ToString();
                            cmd.Parameters.Add("@tube", SqlDbType.NVarChar).Value = DBNull.Value;
                            break;
                        case "2":
                            cmd.Parameters.Add("@pic", SqlDbType.NVarChar).Value = DBNull.Value;
                            cmd.Parameters.Add("@vid", SqlDbType.NVarChar).Value = DBNull.Value;
                            cmd.Parameters.Add("@tube", SqlDbType.NVarChar).Value = Session["YouTubeName"].ToString();
                            break;
                        default: break;
                    }
                }
                else
                {
                    cmd.Parameters.Add("@cat", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@pic", SqlDbType.NVarChar).Value = DBNull.Value;
                    cmd.Parameters.Add("@vid", SqlDbType.NVarChar).Value = DBNull.Value;
                    cmd.Parameters.Add("@tube", SqlDbType.NVarChar).Value = DBNull.Value;
                }
            }
            else
            {
                cmd.Parameters.Add("@cat", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@pic", SqlDbType.NVarChar).Value = DBNull.Value;
                cmd.Parameters.Add("@vid", SqlDbType.NVarChar).Value = DBNull.Value;
                cmd.Parameters.Add("@tube", SqlDbType.NVarChar).Value = DBNull.Value;
            }

            cmd.ExecuteNonQuery();


            conn.Close();

            SetPoll();
        }
    }
}
