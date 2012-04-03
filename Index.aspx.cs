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

public partial class Index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string [] fileArray = System.IO.Directory.GetFiles(MapPath(".") + "\\SellFiles");

        int newIntWidth = 0;
        int newHeight = 0;

        for (int i = 0; i < fileArray.Length; i++)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(MapPath(".") + "\\UserFiles\\Events\\" + 
                ID + "\\Slider\\" + fileArray[i]);

            GetSize(ref newIntWidth, ref newHeight, image);

            Literal literal4 = new Literal();
            literal4.Text = "<div style=\"width: 410px; height: 250px;background-color: black;\"><img style=\" margin-left: " + ((410 - newIntWidth) / 2).ToString() + "px; margin-top: " + ((250 - newHeight) / 2).ToString() + "px;\" height=\"" + newHeight + "px\" width=\"" + newIntWidth + "px\" src=\""
                + "UserFiles/Events/" + ID + "/Slider/" + fileArray[i] + "\" /></div>";
            Telerik.Web.UI.RadRotatorItem r4 = new Telerik.Web.UI.RadRotatorItem();
            r4.Controls.Add(literal4);

            Rotator1.Items.Add(r4);
        }
    }

    protected void GetSize(ref int newIntWidth, ref int newHeight, System.Drawing.Image image)
    {
        int width = 410;
        int height = 250;

        newHeight = 0;
        newIntWidth = 0;

        //if image height is less than resize height
        if (height >= image.Height)
        {
            //leave the height as is
            newHeight = image.Height;

            if (width >= image.Width)
            {
                newIntWidth = image.Width;
            }
            else
            {
                newIntWidth = width;

                double theDivider = double.Parse(image.Width.ToString()) / double.Parse(newIntWidth.ToString());
                double newDoubleHeight = double.Parse(newHeight.ToString());
                newDoubleHeight = double.Parse(height.ToString()) / theDivider;
                newHeight = (int)newDoubleHeight;
            }
        }
        //if image height is greater than resize height...resize it
        else
        {
            //make height equal to the requested height.
            newHeight = height;

            //get the ratio of the new height/original height and apply that to the width
            double theDivider = double.Parse(image.Height.ToString()) / double.Parse(newHeight.ToString());
            double newDoubleWidth = double.Parse(newIntWidth.ToString());
            newDoubleWidth = double.Parse(image.Width.ToString()) / theDivider;
            newIntWidth = (int)newDoubleWidth;

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
            }
        }
    }
}
