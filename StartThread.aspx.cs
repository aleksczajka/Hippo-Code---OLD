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

public partial class StartThread : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlMeta hm = new HtmlMeta();
        HtmlHead head = (HtmlHead)Page.Header;
        hm.Name = "ROBOTS";
        hm.Content = "NOINDEX, FOLLOW";
        head.Controls.AddAt(0, hm);

        ImageButton9.OnClientClick = "javascript:Search('Group.aspx?ID=" +
            Request.QueryString["ID"].ToString() + "');";
    }

    protected void PostThread(object sender, EventArgs e)
    {
        string command = "beginning";
        try
        {
            if (SubjectTextBox.Text.Trim() != "")
            {
                string theID = Request.QueryString["ID"].ToString();

                HttpCookie cookie = Request.Cookies["BrowserDate"];
                if (cookie == null)
                {
                    cookie = new HttpCookie("BrowserDate");
                    cookie.Value = DateTime.Now.ToString();
                    cookie.Expires = DateTime.Now.AddDays(22);
                    Response.Cookies.Add(cookie);
                }

                Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

                SqlConnection conn = dat.GET_CONNECTED;
                SqlCommand cmd = new SqlCommand("SELECT * FROM GroupThreads WHERE ThreadName = @name", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = SubjectTextBox.Text.Trim();
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                DataView dv = new DataView(ds.Tables[0], "", "", DataViewRowState.CurrentRows);

                command = "middle";

                if (dv.Count == 0)
                {

                    cmd = new SqlCommand("INSERT INTO GroupThreads (ThreadName, StartDate, StartedBy, GroupID)" +
                        " VALUES(@name, @date, @by, @groupID)", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = SubjectTextBox.Text.Trim();
                    cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
                    cmd.Parameters.Add("@by", SqlDbType.Int).Value = Session["User"].ToString();
                    cmd.Parameters.Add("@groupID", SqlDbType.Int).Value = Request.QueryString["ID"].ToString();

                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("SELECT @@IDENTITY AS SID", conn);
                    cmd.CommandType = CommandType.Text;
                    da = new SqlDataAdapter(cmd);
                    ds = new DataSet();
                    da.Fill(ds);

                    DataView dvN = new DataView(ds.Tables[0], "", "", DataViewRowState.CurrentRows);
                    string threadID = dvN[0]["SID"].ToString();

                    if (CommentTextBox.Text.Trim() != "")
                    {
                        cmd = new SqlCommand("INSERT INTO GroupThreads_Comments (ThreadID, UserID, PostedDate, Content, Image, YouTube)" +
                            " VALUES(@threadID, @user, @date, @content, @image, @tube)", conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("@threadID", SqlDbType.Int).Value = threadID;
                        cmd.Parameters.Add("@user", SqlDbType.Int).Value = Session["User"].ToString();
                        cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
                        cmd.Parameters.Add("@content", SqlDbType.NVarChar).Value = CommentTextBox.Text.Trim();

                        if (PictureCheckList.Items.Count > 0)
                        {
                            if (VidPicRadioList.SelectedValue == "1")
                            {
                                cmd.Parameters.Add("@image", SqlDbType.NVarChar).Value = PictureCheckList.Items[0].Value;
                                cmd.Parameters.Add("@tube", SqlDbType.NVarChar).Value = DBNull.Value;
                            }
                            else
                            {
                                cmd.Parameters.Add("@image", SqlDbType.NVarChar).Value = DBNull.Value;
                                cmd.Parameters.Add("@tube", SqlDbType.NVarChar).Value = PictureCheckList.Items[0].Value;
                            }                           
                        }
                        else
                        {
                            cmd.Parameters.Add("@image", SqlDbType.NVarChar).Value = DBNull.Value;
                            cmd.Parameters.Add("@tube", SqlDbType.NVarChar).Value = DBNull.Value;
                        }
                        cmd.ExecuteNonQuery();
                    }
                    command = "got here";
                    SendThreadNotifications(threadID);
                    
                    //Mark whether comment/thread is read by each participant
                    DataView dvUsers = dat.GetDataDV("SELECT * FROM Group_Members GM, Users U WHERE GM.MemberID=" +
                        "U.User_ID AND GM.GroupID=" + theID);
                    DataView dvThreadRead = dat.GetDataDV("SELECT * FROM ThreadRead WHERE ThreadID=" + threadID);
                    foreach (DataRowView row in dvUsers)
                    {
                        dvThreadRead.RowFilter = "UserID = " + row["MemberID"].ToString();
                        if (dvThreadRead.Count > 0)
                        {
                            dat.Execute("UPDATE ThreadRead SET [Read] = 'False' WHERE ThreadID=" + threadID +
                                " AND UserID=" + row["MemberID"].ToString());
                        }
                        else
                        {
                            dat.Execute("INSERT INTO ThreadRead ([Read], UserID, ThreadID) VALUES('False', " +
                                row["MemberID"].ToString() + ", " + threadID + ")");
                        }
                    }

                    if (CommentTextBox.Text.Trim() != "")
                    {
                        SendThreadCommentNotifications(threadID);

                    }
                    command = "got here 2";
                    RemovePanel.Visible = false;
                    ThankYouPanel.Visible = true;
                }
                else
                {
                    ErrorLabel.Text = "A thread with this title aready exists for this group.";
                }
            }
            else
            {
                ErrorLabel.Text = "Please include the subject.";
            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString() + "<br/><br/>" + command;
        }
    }

    protected void YouTubeUpload_Click(object sender, EventArgs e)
    {

        if (YouTubeTextBox.Text != "")
        {
            YouTubeTextBox.Text = YouTubeTextBox.Text.Trim().Replace("http://www.youtube.com/watch?v=", "");
            if (PictureCheckList.Items.Count < 20)
            {
                PictureCheckList.Items.Add(new ListItem("YouTube ID: " + YouTubeTextBox.Text, YouTubeTextBox.Text));
                PicsNVideosPanel.Visible = true;
            }
        }

    }

    protected void ShowVidPic(object sender, EventArgs e)
    {
        PictureCheckList.Items.Clear();
        if (VidPicRadioList.SelectedValue == "1")
        {
            YouTubePanel.Visible = false;
            PicturePanel.Visible = true;
        }
        else
        {
            YouTubePanel.Visible = true;
            PicturePanel.Visible = false;
        }
    }

    protected void PictureUpload_Click(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (PictureUpload.HasFile)
        {
            if (PictureCheckList.Items.Count < 20)
            {

                char[] delim = { '.' };
                string[] tokens = PictureUpload.FileName.Split(delim);

                if (tokens.Length > 1)
                {
                    if (tokens[1].ToUpper() == "JPEG" || tokens[1].ToUpper() == "JPG" ||
                        tokens[1].ToUpper() == "GIF" || tokens[1].ToUpper() == "PNG")
                    {
                        string fileName = "rename" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Ticks.ToString() + "." + tokens[1];
                        PictureCheckList.Items.Add(new ListItem(PictureUpload.FileName, fileName));
                        if (!System.IO.Directory.Exists(MapPath(".").ToString() + "/GroupFiles/" + 
                            Request.QueryString["ID"] + "/Slider/"))
                        {
                            if (!System.IO.Directory.Exists(MapPath(".").ToString() + "/GroupFiles/" +
                            Request.QueryString["ID"]))
                            {
                                System.IO.Directory.CreateDirectory(MapPath(".").ToString() + "/GroupFiles/" +
                            Request.QueryString["ID"]);
                            }

                            System.IO.Directory.CreateDirectory(MapPath(".").ToString() + "/GroupFiles/" +
                            Request.QueryString["ID"] + "/Slider/");
                        }

                        System.Drawing.Image img = System.Drawing.Image.FromStream(PictureUpload.PostedFile.InputStream);

                        SaveThumbnail(img, true, MapPath(".").ToString() + "/GroupFiles/" +
                            Request.QueryString["ID"] +
                            "/Slider/" + fileName, "image/" + tokens[1].ToLower(), 410, 250);

                        //PictureUpload.SaveAs(MapPath(".").ToString() + "/UserFiles/" + Session["UserName"].ToString() +
                        //    "/Slider/" + fileName);
                        PicsNVideosPanel.Visible = true;
                    }
                    else
                    {
                        MessageLabel.Text = "No go! Pictures can only be .gif, .jpg, .jpeg, or .png.";
                    }
                }
                else
                {
                    MessageLabel.Text = "No go! Pictures can only be .gif, .jpg, .jpeg, or .png.";
                }
            }
        }
    }

    private void SaveThumbnail(System.Drawing.Image image, bool isRotator, 
        string path, string typeS, int width, int height)
    {

        int newHeight = 0;
        int newIntWidth = 0;

        float newFloatHeight = 0.00F;
        float newFloatWidth = 0.00F;

        if (isRotator)
        {
            //if image height is less than resize height
            if (height >= image.Height)
            {
                //leave the height as is
                newHeight = image.Height;

                if (width >= image.Width)
                {
                    newIntWidth = image.Width;
                    newFloatWidth = image.Width;
                }
                else
                {
                    newIntWidth = width;

                    double theDivider = double.Parse(image.Width.ToString()) / double.Parse(newIntWidth.ToString());
                    double newDoubleHeight = double.Parse(newHeight.ToString());
                    newDoubleHeight = double.Parse(height.ToString()) / theDivider;
                    newHeight = (int)newDoubleHeight;
                    newFloatHeight = (float)newDoubleHeight;
                }
            }
            //if image height is greater than resize height...resize it
            else
            {
                //make height equal to the requested height.
                newHeight = height;
                newFloatHeight = height;
                //get the ratio of the new height/original height and apply that to the width
                double theDivider = double.Parse(image.Height.ToString()) / double.Parse(newHeight.ToString());
                double newDoubleWidth = double.Parse(newIntWidth.ToString());
                newDoubleWidth = double.Parse(image.Width.ToString()) / theDivider;
                newIntWidth = (int)newDoubleWidth;
                newFloatWidth = (float)newDoubleWidth;
                //if the resized width is still to big
                if (newIntWidth > width)
                {
                    //make it equal to the requested width
                    newIntWidth = width;

                    //get the ratio of old/new width and apply it to the already resized height
                    theDivider = double.Parse(image.Width.ToString()) / double.Parse(newIntWidth.ToString());
                    double newDoubleHeight = double.Parse(newHeight.ToString());
                    newDoubleHeight = double.Parse(image.Height.ToString()) / theDivider;
                    newHeight = (int)newDoubleHeight;
                    newFloatHeight = (float)newDoubleHeight;
                }
            }
        }
        else
        {
            newHeight = 100;
            newIntWidth = 100;
        }

        //if (quality < 0 || quality > 100)
        //    throw new ArgumentOutOfRangeException("quality must be between 0 and 100.");


        //// Encoder parameter for image quality 
        //EncoderParameter qualityParam =
        //    new EncoderParameter(Encoder.Quality, 100);
        //// Jpeg image codec 
        //ImageCodecInfo jpegCodec = GetEncoderInfo(typeS);

        //EncoderParameters encoderParams = new EncoderParameters(1);
        //encoderParams.Param[0] = qualityParam;

        System.Drawing.Bitmap bmpResized = new System.Drawing.Bitmap(image, newIntWidth, newHeight);


        //System.Drawing.Image thumbnail = image.GetThumbnailImage(newIntWidth, newHeight,
        //    new System.Drawing.Image.GetThumbnailImageAbort(EmptyCallBack), IntPtr.Zero);
        //SaveJpeg(path, thumbnail, 10, typeS);



        bmpResized.Save(path);
        //thumbnail.Save(path);
    }

    protected void SliderNixIt(object sender, EventArgs e)
    {
        int songCount = PictureCheckList.Items.Count;
        CheckBoxList tempList = new CheckBoxList();
        for (int i = 0; i < songCount; i++)
        {
            if (!PictureCheckList.Items[i].Selected)
                tempList.Items.Add(PictureCheckList.Items[i]);
            else
            {
                if (System.IO.File.Exists(MapPath(".") + "/GroupFiles/" +
                            Request.QueryString["ID"] + "/Slider/" + PictureCheckList.Items[i].Value))
                {
                    System.IO.File.Delete(MapPath(".") + "/GroupFiles/" +
                            Request.QueryString["ID"] + "/Slider/" + PictureCheckList.Items[i].Value);
                }
            }
        }
        PictureCheckList.Items.Clear();
        for (int j = 0; j < tempList.Items.Count; j++)
        {
            PictureCheckList.Items.Add(tempList.Items[j]);
        }
        if (PictureCheckList.Items.Count == 0)
            PicsNVideosPanel.Visible = false;
    }

    protected void SendThreadCommentNotifications(string threadID)
    {
        string theID = Request.QueryString["ID"].ToString();

        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        DataView dvUsers = dat.GetDataDV("SELECT * FROM Group_Members GM, Users U WHERE GM.MemberID=" +
            "U.User_ID AND GM.GroupID=" + theID + " AND Prefs LIKE '%5%'");
        DataView dvGroup = dat.GetDataDV("SELECT * FROM Groups WHERE ID=" + Request.QueryString["ID"].ToString());
        DataView dvThread = dat.GetDataDV("SELECT * FROM GroupThreads WHERE ID=" + threadID);
        string email = "A new comment has been posted on the discussion thread '" + dvThread[0]["ThreadName"].ToString() +
            "' for the group '" +
            dvGroup[0]["Header"].ToString() + "'. <a href=\"http://hippohappenings.com/" +
            dat.MakeNiceName(dvGroup[0]["Header"].ToString()) + "_" + dvGroup[0]["ID"].ToString() +
            "_Group\">Check it out.</a>";
        foreach (DataRowView row in dvUsers)
        {
            dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
            System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(), row["Email"].ToString(),
            email, "A new thread comment has been posted");
        }
    }

    protected void SendThreadNotifications(string threadID)
    {
        string theID = Request.QueryString["ID"].ToString();

        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        DataView dvUsers = dat.GetDataDV("SELECT * FROM Group_Members GM, Users U WHERE GM.MemberID=" +
            "U.User_ID AND GM.GroupID=" + theID + " AND Prefs LIKE '%1%'");
        DataView dvGroup = dat.GetDataDV("SELECT * FROM Groups WHERE ID=" + Request.QueryString["ID"].ToString());
        DataView dvThread = dat.GetDataDV("SELECT * FROM GroupThreads WHERE ID=" + threadID);
        string email = "A new discussion thread '" + dvThread[0]["ThreadName"].ToString() +
            "' has been posted for the group '" +
            dvGroup[0]["Header"].ToString() + "'. <a href=\"http://hippohappenings.com/" +
            dat.MakeNiceName(dvGroup[0]["Header"].ToString()) + "_" + dvGroup[0]["ID"].ToString() +
            "_Group\">Check it out.</a>";
        foreach (DataRowView row in dvUsers)
        {
            dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
            System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(), row["Email"].ToString(),
            email, "A new thread has been posted");
        }
    }
}
