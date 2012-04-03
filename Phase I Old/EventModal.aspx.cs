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

public partial class EventModal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlLink lk = new HtmlLink();
        HtmlHead head = (HtmlHead)Page.Header;
        lk.Attributes.Add("rel", "canonical");
        lk.Href = "http://hippohappenings.com/EventModal.aspx";
        head.Controls.AddAt(0, lk);

        HtmlMeta hm = new HtmlMeta();
        hm.Name = "ROBOTS";
        hm.Content = "NOINDEX, NOFOLLOW";
        head.Controls.AddAt(0, hm);

        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        string ID = Request.QueryString["eventID"].ToString();
        DataSet ds = dat.GetData("SELECT * FROM Events WHERE ID=" + ID);
        string content = ds.Tables[0].Rows[0]["Content"].ToString();


        //Media Categories: NONE: 0, Picture: 1, Video: 2, YouTubeVideo: 3, Slider: 4
        int mediaCategory = int.Parse(ds.Tables[0].Rows[0]["mediaCategory"].ToString());
        string youtube = ds.Tables[0].Rows[0]["YouTubeVideo"].ToString();
        switch (mediaCategory)
        {
            case 0:
                break;
            case 1:
                char[] delim4 = { ';' };
                string[] youtokens = youtube.Split(delim4);
                if (youtube != "")
                {
                    for (int i = 0; i < youtokens.Length; i++)
                    {
                        if (youtokens[i].Trim() != "")
                        {
                            Literal literal3 = new Literal();
                            literal3.Text = "<div style=\"float:left;\"><object width=\"410\" height=\"250\"><param name=\"movie\" value=\"http://www.youtube.com/v/" + youtokens[i] +
                                "\"></param><param name=\"allowFullScreen\" value=\"true\"></param><embed src=\"http://www.youtube.com/v/" +
                                youtokens[i] + "\" type=\"application/x-shockwave-flash\" allowfullscreen=\"true\" width=\"410\" height=\"250\"></embed></object></div>";
                            Telerik.Web.UI.RadRotatorItem r3 = new Telerik.Web.UI.RadRotatorItem();
                            r3.Controls.Add(literal3);
                            Rotator1.Items.Add(r3);
                        }
                    }
                }
                DataSet dsSlider = dat.GetData("SELECT * FROM Event_Slider_Mapping WHERE EventID=" + ID);
                if (dsSlider.Tables.Count > 0)
                    if (dsSlider.Tables[0].Rows.Count > 0)
                    {
                        char[] delim = { '\\' };
                        char[] delim3 = { '.' };
                        string[] fileArray = System.IO.Directory.GetFiles(MapPath(".") + "\\UserFiles\\Events\\" + ID + "\\Slider");

                        string[] finalFileArray = new string[fileArray.Length];

                        for (int i = 0; i < fileArray.Length; i++)
                        {
                            int length = fileArray[i].Split(delim).Length;
                            finalFileArray[i] = "http://" + Request.Url.Authority + "/HippoHappenings/UserFiles/Events/" +
                                ID + "/Slider/" + fileArray[i].Split(delim)[length - 1];
                            string[] tokens = fileArray[i].Split(delim)[length - 1].Split(delim3);


                            if (tokens.Length >= 2)
                            {
                                if (tokens[1].ToUpper() == "JPG" || tokens[1].ToUpper() == "JPEG" || tokens[1].ToUpper() == "GIF")
                                {
                                    Literal literal4 = new Literal();
                                    literal4.Text = "<img style=\"cursor: pointer;\" onclick=\"OpenEventModal(" + i.ToString() + ", " + ID + ");\" height=\"250px\" width=\"410px\" src=\""
                                        + "UserFiles/Events/" + ID + "/Slider/" + fileArray[i].Split(delim)[length - 1].ToString() + "\" />";
                                    Telerik.Web.UI.RadRotatorItem r4 = new Telerik.Web.UI.RadRotatorItem();
                                    r4.Controls.Add(literal4);
                                    Rotator1.Items.Add(r4);
                                }
                                else if (tokens[1].ToUpper() == "WMV")
                                {
                                    Literal literal4 = new Literal();
                                    literal4.Text = "<div style=\"float:left;\"><embed  height=\"250px\" width=\"410px\" src=\""
                                        + "UserFiles/Events/" + ID + "/Slider/" + fileArray[i].Split(delim)[length - 1].ToString() + "\" /></div>";
                                    Telerik.Web.UI.RadRotatorItem r4 = new Telerik.Web.UI.RadRotatorItem();
                                    r4.Controls.Add(literal4);
                                    Rotator1.Items.Add(r4);
                                }

                            }
                        }
                    }
                break;
            default: break;
        }
    }
}
