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


public partial class HippoPointsEdit : System.Web.UI.Page
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

        HtmlMeta hm = new HtmlMeta();
        HtmlHead head = (HtmlHead)Page.Header;
        hm.Name = "ROBOTS";
        hm.Content = "NOINDEX, FOLLOW";
        head.Controls.AddAt(0, hm);

        HtmlLink lk = new HtmlLink();
        lk.Attributes.Add("rel", "canonical");
        lk.Href = "http://hippohappenings.com/HippoPointsEdit.aspx";
        head.Controls.AddAt(0, lk);

        UploadButton.SERVER_CLICK += UploadPick;
        PictureNixItButton.SERVER_CLICK += NixIt;
        BlueButton2.SERVER_CLICK += Save;

        ErrorLabel.Text = "";

        Literal lit = new Literal();
        lit.Text = dat.GetDataDV("SELECT * FROM HippoPointsConditions")[0]["Content"].ToString();

        TACTextBox.Controls.Add(lit);

        if (!IsPostBack)
        {
            DataView dvUser = dat.GetDataDV("SELECT * FROM UserPreferences UP WHERE UP.UserID=" + Session["User"].ToString());

            if(dvUser[0]["MayorText"].ToString().Trim() != "")
                DescriptionTextBox.Text = dvUser[0]["MayorText"].ToString();

            Checkbox1.Checked = bool.Parse(dvUser[0]["Mayors"].ToString());

            if (dvUser[0]["PictureName"].ToString().Trim() != "")
            {
                TheImage.ImageUrl = dvUser[0]["PictureName"].ToString();
                PictureNixItButton.Visible = true;
            }

            if (dvUser[0]["MayorLink"].ToString().Trim() != "")
            {
                LinkTextBox.Text = dvUser[0]["MayorLink"].ToString();
            }
        }
    }

    protected void UploadPick(object sender, EventArgs e)
    {
        DateTime isNow = DateTime.Now;

        if (FileUpload.HasFile)
        {
            char[] delim = { '.' };
            string[] tokens = FileUpload.FileName.Split(delim);

            if (tokens.Length >= 2)
            {
                if (tokens[1].ToUpper() == "JPG" || tokens[1].ToUpper() == "JPEG" || 
                    tokens[1].ToUpper() == "GIF" || tokens[1].ToUpper() == "PNG")
                {
                    string fileName = "rename" + isNow.Ticks.ToString() + "." + tokens[1];

                    System.Drawing.Image img = System.Drawing.Image.FromStream(FileUpload.PostedFile.InputStream);

                    if(!System.IO.Directory.Exists(MapPath(".").ToString() + "\\UserFiles\\" +
                        Session["UserName"].ToString()))
                    {
                        System.IO.Directory.CreateDirectory(MapPath(".").ToString() + "\\UserFiles\\" +
                        Session["UserName"].ToString());
                    }

                    SaveThumbnail(img, MapPath(".").ToString() + "\\UserFiles\\" +
                        Session["UserName"].ToString() + "\\" + fileName);

                    TheImage.ImageUrl = "~/UserFiles/" + Session["UserName"].ToString() + "/" + fileName;
                    PictureNixItButton.Visible = true;
                }
                else
                {
                    ErrorLabel.Text = "The image file must be .jpg, .jpeg, .gif or .png.";
                }
            }
        }
    }

    private void NixIt(object sender, EventArgs e)
    {
        TheImage.ImageUrl = "";
        PictureNixItButton.Visible = false;
    }

    private void SaveThumbnail(System.Drawing.Image image, string path)
    {
        int width = 200;
        int height = 130;

        int newHeight = 0;
        int newIntWidth = 0;

        float newFloatHeight = 0.00F;
        float newFloatWidth = 0.00F;

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

        System.Drawing.Bitmap bmpResized = new System.Drawing.Bitmap(image, newIntWidth, newHeight);


        bmpResized.Save(path);
        //thumbnail.Save(path);
    }

    private void Save(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        if (Checkbox1.Checked)
        {
            if (DescriptionTextBox.Text.Trim() != "")
            {
                if (TheImage.ImageUrl != "")
                {
                    DescriptionTextBox.Text = dat.stripHTML(DescriptionTextBox.Text.Trim());

                    if (DescriptionTextBox.Text.Length <= 430)
                    {
                        LinkTextBox.Text = dat.stripHTML(LinkTextBox.Text.Trim());

                        dat.Execute("UPDATE UserPreferences SET MayorLink='" + LinkTextBox.Text.Trim() +
                            "', Mayors='True', MayorText='" + DescriptionTextBox.Text.Replace("'", "''") +
                            "', PictureName='" + TheImage.ImageUrl + "' WHERE UserID=" +
                            Session["User"].ToString());

                        ErrorLabel.Text = "Your information has been saved.";
                    }
                    else
                    {
                        ErrorLabel.Text = "Your paragraph is " + (DescriptionTextBox.Text.Length - 430).ToString() + " characters over the limit.";
                    }
                }
                else
                {
                    ErrorLabel.Text = "Please include an image.";
                }
            }
            else
            {
                ErrorLabel.Text = "Please include a paragraph.";
            }
        }
        else
        {
            dat.Execute("UPDATE UserPreferences SET Mayors='False' WHERE UserID=" + Session["User"].ToString());

            ErrorLabel.Text = "Your information has been saved.";
        }
    }
}
