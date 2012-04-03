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

public partial class Comment : System.Web.UI.UserControl
{
    public string COMMENT
    {
        get { return comment; }
        set { comment = value; }
    }
    public bool IS_LEFT
    {
        get { return isLeft; }
        set { isLeft = value; }
    }
    public string PROFILE_THUMB
    {
        get { return profileThumb; }
        set { profileThumb = value; }
    }
    public int COMMENTER_ID
    {
        get { return commenterID; }
        set { commenterID = value; }
    }
    public string DATE_LABEL
    {
        get { return dateLabel; }
        set { dateLabel = value; }
    }
    public string USER_LABEL
    {
        get { return userLabel; }
        set { userLabel = value; }
    }
    public string EVENT_OR_VENUE_ID
    {
        get { return eventOrvenueID; }
        set { eventOrvenueID = value; }
    }
    public bool IS_EVENT
    {
        get { return isEvent; }
        set { isEvent = value; }
    }
    private string comment;
    private bool isLeft;
    private string profileThumb;
    private int commenterID;
    private string dateLabel;
    private string userLabel;
    private string eventOrvenueID;
    private bool isEvent = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        try
        {
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            DateLabel.Text = dateLabel;
            UserLabel.Text = userLabel;
            UserLabel.NavigateUrl = "~/" + userLabel + "_Friend";
            ProfileImage.ImageUrl = profileThumb;
            ProfileImage.PostBackUrl = "~/" + userLabel + "_Friend";
            ProfileImage.AlternateText = "Member " + userLabel;

            if (profileThumb.Equals("~/NewImages/noAvatar_50x50_small.png"))
            {
                ProfileImage.Attributes.Add("onmouseover", "this.src='NewImages/noAvatar_50x50_smallhover.png'");
                ProfileImage.Attributes.Add("onmouseout", "this.src='NewImages/noAvatar_50x50_small.png'");
            }
            else
            {
                try
                {
                    System.Drawing.Image theimg = System.Drawing.Image.FromFile(Server.MapPath(profileThumb));

                    double width = double.Parse(theimg.Width.ToString());
                    double height = double.Parse(theimg.Height.ToString());

                    if (width > height)
                    {
                        if (width <= 50)
                        {

                        }
                        else
                        {
                            double dividor = double.Parse("50.00") / double.Parse(width.ToString());
                            width = double.Parse("50.00");
                            height = height * dividor;
                        }
                    }
                    else
                    {
                        if (width == height)
                        {
                            width = double.Parse("50.00");
                            height = double.Parse("50.00");
                        }
                        else
                        {
                            double dividor = double.Parse("50.00") / double.Parse(height.ToString());
                            height = double.Parse("50.00");
                            width = width * dividor;
                        }
                    }

                    ProfileImage.Width = int.Parse((Math.Round(decimal.Parse(width.ToString()))).ToString());
                    ProfileImage.Height = int.Parse((Math.Round(decimal.Parse(height.ToString()))).ToString());
                }
                catch (Exception ex)
                {
                    ProfileImage.Attributes.Add("onmouseover", "this.src='NewImages/noAvatar_50x50_smallhover.png'");
                    ProfileImage.Attributes.Add("onmouseout", "this.src='NewImages/noAvatar_50x50_small.png'");
                    ProfileImage.ImageUrl = "~/NewImages/noAvatar_50x50_small.png";
                }
            }
            CommentLabel.Text = comment;

            try
            {


                if (Session["User"] != null)
                {
                    DataSet ds = dat.GetData("SELECT CommunicationPrefs FROM UserPreferences WHERE UserID=" + commenterID);

                    if (ds.Tables.Count > 0)
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0]["CommunicationPrefs"].ToString() == "1")
                                if (Session["User"].ToString() != commenterID.ToString())
                                    CommentPanel.Visible = true;
                                else
                                    CommentPanel.Visible = false;
                            else
                                CommentPanel.Visible = false;
                        }
                        else
                            CommentPanel.Visible = false;
                    else
                        CommentPanel.Visible = false;
                }
                else
                {
                    CommentPanel.Visible = false;
                }
            }
            catch (Exception ex)
            {
            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString() + "path: " + Server.MapPath(profileThumb);
        }

        string type = "E";
        if (!isEvent)
            type = "V";
        if (Request.Url.Segments[Request.Url.Segments.Length - 1] == "Trip.aspx")
            type = "T";
        MessageRadWindow.NavigateUrl = "MessageAlert.aspx?T=Comment&ID=" + commenterID + "&EV=V&A=B&EVID=" + eventOrvenueID;

    }

    protected void Connect(object sender, EventArgs e)
    {
        
    }
}
