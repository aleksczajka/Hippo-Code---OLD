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
using System.Globalization;
using Telerik.Web.UI;
using System.IO;

public partial class MasterPage : System.Web.UI.MasterPage
{
    public HtmlGenericControl BodyTag
    {
        get
        {
            return bodytag;
        }
        set
        {
            bodytag = value;
        }
    }
    public HtmlGenericControl FormTag
    {
        get
        {
            return form1;
        }
        set
        {
            form1 = value;
        }
    }
    public HtmlHead HEAD_TAG
    {
        get
        {
            return HeadTag;
        }
        set
        {
            HeadTag = value;
        }
    }
    public RadWindow RAD_WINDOW_3
    {
        get { return RadWindow3; }
        set { RadWindow3 = value; }
    }
    public ScriptManager theScriptManager
    {
        get
        {
            return ScriptManager1;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //2/23/2010%2020%3A54%3A38
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        DateTime isn = DateTime.Now;

        DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn);

        Data dat = new Data(isn);

        
        if ((Session["User"] == null && Session["UserName"] != null) ||
            (Session["UserName"] == null && Session["User"] != null))
        {
            dat.Execute("DELETE FROM Events_Seen_By_User WHERE userID=" + Session["User"].ToString());
            string save1 = "";
            if (Session["AnonymousUser"] != null)
                save1 = Session["AnonymousUser"].ToString();

            string save2 = "";
            if (Session["BigAnonymousUser"] != null)
                save2 = Session["BigAnonymousUser"].ToString();


            Session.Remove("User");
            Session.Remove("UserName");
            FormsAuthentication.SignOut();


            if (save1 != "")
                Session["AnonymousUser"] = save1;

            if (save2 != "")
                Session["BigAnonymousUser"] = save2;

            Response.Redirect("login");
        }

        try
        {
            HttpCookie cookieZone = Request.Cookies["TimeZone"];

            if (!IsPostBack)
            {
                //TextWriter tw = new StreamWriter(Server.MapPath("~/js/TotalJS.js"), false);
                //string str = "function timedout(){var today=new Date();var month=today.getMonth()+1;"+
                //    "var day=today.getDate();var year=today.getFullYear();var hours=today."+
                //    "getHours();var mins=today.getMinutes();var seconds=today.getSeconds();"+
                //    "var todayDate=month+'/'+day+'/'+year+' '+hours+':'+mins+':'+seconds;"+
                //    "setCookie(\"TimeZone\",today.getTimezoneOffset(),1);setCookie(\"BrowserDate\","+
                //        "todayDate,1)}function setCookie(c_name,value,expiredays){var exdate=new "+
                //        "Date();exdate.setDate(exdate.getDate()+expiredays);document.cookie="+
                //        "c_name+\"=\"+escape(value)+((expiredays==null)?\"\":\";expires=\"+exdate"+
                //        ".toGMTString())}function getCookie(c_name){if(document.cookie.length>0)"+
                //        "{c_start=document.cookie.indexOf(c_name+\"=\");if(c_start!=-1){c_start"+
                //        "=c_start+c_name.length+1;c_end=document.cookie.indexOf(\";\",c_start);"+
                //                        "if(c_end==-1)c_end=document.cookie.length;return unescape(document.cookie."+
                //                        "substring(c_start,c_end))}}return\"\"}"+
                //        "function setWidth(randNum)"+
                //        "{"+
                //           " var theWidthDiv = document.getElementById('widthDiv'+randNum);"+
                //         "   var theWrapDiv = document.getElementById('WrapDiv'+randNum);"+
                //         "   theWrapDiv.style.width = theWidthDiv.childNodes[3].offsetWidth + 14 + 'px';"+
                //         "   var somtinhere = 0;"+
                //        "}"+
                //        "function setWidthGreen(randNum)"+
                //        "{"+
                //         "   var theWidthDiv = document.getElementById('widthDiv'+randNum);"+
                //         "   var theWrapDiv = document.getElementById('WrapDiv'+randNum);"+
                //         "   theWrapDiv.style.width = theWidthDiv.childNodes[3].offsetWidth + 16 + 'px';"+
                //         "   var somtinhere = 0;"+
                //        "}"+
                //        "function setWait(event)"+
                //        "{"+
                //            "var thisGuy = document.getElementById(event.target.id);"+
                //            "if(thisGuy != null && thisGuy != undefined)"+
                //           "     thisGuy.style.cursor = 'wait';"+
                //            "return true;"+
                //        "}";
                //// write a line of text to the file
                //tw.WriteLine(str);

                //// close the stream
                //tw.Close();

                bool isPageNew = bool.Parse(ConfigurationSettings.AppSettings["isPageNew"].ToString());
                if (!isPageNew)
                    ConfigurationSettings.AppSettings["isPageNew"] = "true";
                if (Session["User"] == null)
                {
                    dat.Execute("INSERT INTO IP_User_Traffic (IP, PageName, DateAndTime) " +
                        "VALUES ('" + dat.GetIP() + "', '" + Request.Url.AbsolutePath.ToLower() +
                        "', GETDATE())");
                }
                else
                {
                    dat.Execute("INSERT INTO IP_User_Traffic (IP, UserID, PageName, DateAndTime) " +
                        "VALUES ('" + dat.GetIP() + "', " + Session["User"].ToString() +
                        ",'" + Request.Url.AbsolutePath.ToLower() + "', GETDATE())");
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
}
