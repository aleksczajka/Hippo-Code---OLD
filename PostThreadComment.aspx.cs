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

public partial class PostThreadComment : System.Web.UI.Page
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
        string command = "";
        try
        {
            if (SubjectTextBox.Text.Trim() != "")
            {
                string GroupID = Request.QueryString["ID"].ToString();
                string ThreadID = Request.QueryString["TID"].ToString();
                HttpCookie cookie = Request.Cookies["BrowserDate"];
                Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

                SqlConnection conn = dat.GET_CONNECTED;
                SqlCommand cmd;

                cmd = new SqlCommand("INSERT INTO GroupThreads_Comments (ThreadID, UserID, PostedDate, [Content], Image, YouTube)" +
                    " VALUES(@tid, @userID, GETDATE(), @content, @image, @tube)", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@content", SqlDbType.NVarChar).Value = SubjectTextBox.Text.Trim();
                cmd.Parameters.Add("@userID", SqlDbType.Int).Value = Session["User"].ToString();
                cmd.Parameters.Add("@tid", SqlDbType.Int).Value = Request.QueryString["TID"].ToString();

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

                SendThreadNotifications(ThreadID);

                RemovePanel.Visible = false;
                ThankYouPanel.Visible = true;

            }
            else
            {
                ErrorLabel.Text = "Please include the comment.";
            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString() + "<br/><br/>" + command;
        }
    }

    protected void SendThreadNotifications(string threadID)
    {
        string theID = Request.QueryString["ID"].ToString();

        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));


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

        dvUsers = dat.GetDataDV("SELECT * FROM Group_Members GM, Users U WHERE GM.MemberID=" +
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
}
