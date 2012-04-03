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
using System.Xml;

public partial class Controls_SongPlayer : System.Web.UI.UserControl
{
    public string SONG1
    {
        get { return song1; }
        set { song1 = value; }
    }

    public string SONG1_TITLE
    {
        get { return song1Title; }
        set { song1Title = value; }
    }

    public string SONG2
    {
        get { return song2; }
        set { song2 = value; }
    }

    public string SONG2_TITLE
    {
        get { return song2Title; }
        set { song2Title = value; }
    }

    public string SONG3
    {
        get { return song3; }
        set { song3 = value; }
    }

    public string SONG3_TITLE
    {
        get { return song3Title; }
        set { song3Title = value; }
    }

    public string USER_NAME
    {
        get { return UserName; }
        set { UserName = value; }
    }
    private string song1 = "";
    private string song1Title = "";
    private string song2 = "";
    private string song2Title = "";
    private string song3 = "";
    private string song3Title = "";
    private string UserName;
    protected void Page_Load(object sender, EventArgs e)
    {

        System.IO.FileInfo[] d = new System.IO.DirectoryInfo(Server.MapPath("Controls/PlayerXML/")).GetFiles();

        char[] delim = { '.' };
        

        foreach (System.IO.FileInfo file in d)
        {
            string [] fileTok = file.Name.Split(delim);
            if (fileTok[1].ToLower() == "xml")
                file.Delete();
        }

        XmlDocument xmldoc = new XmlDocument();
        xmldoc.Load(Server.MapPath("Controls/PlayList.xml"));
        xmldoc.GetElementsByTagName("xml")[0].RemoveAll();
        XmlElement elem = (XmlElement)xmldoc.GetElementsByTagName("xml")[0];
        if (song1 != "")
        {
            if (System.IO.File.Exists(Server.MapPath(".") + "\\UserFiles\\" + UserName + "\\Songs\\" + song1))
            {
                XmlElement track1 = xmldoc.CreateElement("track");
                XmlElement path1 = xmldoc.CreateElement("path");
                XmlElement title1 = xmldoc.CreateElement("title");

                path1.InnerText = "http://hippohappenings.com/UserFiles/" + UserName + "/Songs/" + song1;
                title1.InnerText = song1Title;

                track1.AppendChild(path1);
                track1.AppendChild(title1);

                elem.AppendChild(track1);
            }
        }
        if (song2 != "")
        {
            if (System.IO.File.Exists(Server.MapPath(".") + "\\UserFiles\\" + UserName + "\\Songs\\" + song2))
            {
                XmlElement track2 = xmldoc.CreateElement("track");
                XmlElement path2 = xmldoc.CreateElement("path");
                XmlElement title2 = xmldoc.CreateElement("title");

                path2.InnerText = "http://hippohappenings.com/UserFiles/" + UserName + "/Songs/" + song2;
                title2.InnerText = song2Title;

                track2.AppendChild(path2);
                track2.AppendChild(title2);

                elem.AppendChild(track2);
            }
        }
        if (song3 != "")
        {
            if (System.IO.File.Exists(Server.MapPath(".") + "\\UserFiles\\" + UserName + "\\Songs\\" + song3))
            {
                XmlElement track3 = xmldoc.CreateElement("track");
                XmlElement path3 = xmldoc.CreateElement("path");
                XmlElement title3 = xmldoc.CreateElement("title");

                path3.InnerText = "http://hippohappenings.com/UserFiles/" + UserName + "/Songs/" + song3;
                title3.InnerText = song3Title;

                track3.AppendChild(path3);
                track3.AppendChild(title3);

                elem.AppendChild(track3);
            }
        }

        HttpCookie cookie;


        string fName = "PlayList" + DateTime.Now.ToUniversalTime().ToString().Replace("/", "_").Replace(" ", "_").Replace(":", "_") + ".xml";

        cookie = new HttpCookie("PlayerXML");
        cookie.Value = fName;
        cookie.Expires = DateTime.Now.AddDays(1);
        Response.Cookies.Add(cookie);

        xmldoc.Save(Server.MapPath("Controls/PlayerXML/" + fName));
    }
}
