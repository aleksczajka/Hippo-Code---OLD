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

public partial class AlarmMessage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlLink lk = new HtmlLink();
        HtmlHead head = (HtmlHead)Page.Header;
        lk.Attributes.Add("rel", "canonical");
        lk.Href = "http://hippohappenings.com/AlarmMessage.aspx";
        head.Controls.AddAt(0, lk);

        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        //Ajax.Utility.RegisterTypeForAjax(typeof(EventCommunicate));
        if (!IsPostBack)
        {
            try
            {
                if (Session["User"] != null)
                    LoadCustomAlert();
            }
            catch (Exception ex)
            {
            }  
        }
    }

    protected void LoadCustomAlert()
    {
        try
        {
            HttpCookie cookie = Request.Cookies["BrowserDate"];
           
            Data d = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            DataView dvAlerts = d.GetDataDV("SELECT * FROM UserEventAlerts WHERE UserID=" +
                       Session["User"].ToString() + " AND EventID=" + Request.QueryString["EID"].ToString());

            if (dvAlerts.Count > 0)
            {
                if (bool.Parse(dvAlerts[0]["ON"].ToString()))
                {
                    AlertRadioList.Items[0].Selected = true;
                    AlertRadioList.Items[1].Selected = false;
                }
                else
                {
                    AlertRadioList.Items[0].Selected = false;
                    AlertRadioList.Items[1].Selected = true;
                }

                if (dvAlerts[0]["MHDWNumb"] != null)
                {
                    if (dvAlerts[0]["MHDWNumb"].ToString().Trim() != "")
                    {
                        TimeCheck.Checked = true;
                        TimeTextBox.Text = dvAlerts[0]["MHDWNumb"].ToString();
                        TimeDropDown.SelectedValue = dvAlerts[0]["MHDW"].ToString();
                    }
                }
                else
                {
                    TimeCheck.Checked = false;
                    TimeTextBox.Text = "";
                    TimeDropDown.ClearSelection();
                }

                if (dvAlerts[0]["RepeatMHDWNumb"] != null)
                {
                    if (dvAlerts[0]["RepeatMHDWNumb"].ToString().Trim() != "")
                    {
                        RepeatCheck.Checked = true;
                        RepeatTextBox.Text = dvAlerts[0]["RepeatMHDWNumb"].ToString();
                        RepeatDropDown.SelectedValue = dvAlerts[0]["RepeatMHDW"].ToString();
                    }
                }
                else
                {
                    RepeatCheck.Checked = false;
                    RepeatTextBox.Text = "";
                    RepeatDropDown.ClearSelection();
                }

                EndedCheck.Checked = bool.Parse(dvAlerts[0]["AlertWhenEnded"].ToString());

                EmailCheck.Checked = bool.Parse(dvAlerts[0]["isEmail"].ToString());

                TextCheck.Checked = bool.Parse(dvAlerts[0]["isText"].ToString());

                DateTime nowTime = DateTime.Now;
                if (nowTime.Second > 30)
                    nowTime = nowTime.AddSeconds(60 - nowTime.Second);
                else
                    nowTime = nowTime.AddSeconds(-nowTime.Second);

            }
            else
            {
                AlertRadioList.Items[0].Selected = false;
                AlertRadioList.Items[1].Selected = false;

                TimeCheck.Checked = false;
                TimeTextBox.Text = "";
                TimeDropDown.ClearSelection();

                RepeatCheck.Checked = false;
                RepeatTextBox.Text = "";
                RepeatDropDown.ClearSelection();


                EndedCheck.Checked = false;

                EmailCheck.Checked = false;

                TextCheck.Checked = false;
            }
        }
        catch (Exception ex)
        {
            MessageLabel.Text = ex.ToString();
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        int i = 0;
    }

    protected void SaveSettingsDB(object sender, EventArgs e)
    {
        string message = "";
        try
        {
            HttpCookie cookie = Request.Cookies["BrowserDate"];

            Data d = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            //save custom alert preferences
            DataView dvAlerts = d.GetDataDV("SELECT * FROM UserEventAlerts WHERE UserID=" +
                       Session["User"].ToString() + " AND ReoccurID=" +
                       Request.QueryString["RID"].ToString() + " AND EventID=" +
                       Request.QueryString["EID"].ToString());
            bool execute = false;

            if (!TextCheck.Checked && !EmailCheck.Checked)
            {
                message = "Please check 'Email', 'Text', or both for this alert.";
                execute = false;
            }
            else
            {
                if (dvAlerts.Count > 0)
                {
                    if (TimePicker.DbSelectedDate != null)
                    {
                        cmd = new SqlCommand("UPDATE UserEventAlerts SET MHDW=@mhdw, MHDWNumb=@mhdwNum, " +
                           "RepeatMHDW=@repeatMHDW, LastRan=@lastRun, RepeatMHDWNumb=@repeatMHDWNum, AlertWhenEnded=@ended, " +
                           "isEmail=@email, isText=@text, [On]=@on, ServerTimeDifference=@time WHERE UserID=" +
                           Session["User"].ToString() + " AND isAllEvents = 'False' AND EventID=" + Request.QueryString["EID"].ToString(), conn);
                        execute = true;
                    }
                    else
                    {
                        message = "The time in your location is required.";
                        execute = false;
                    }
                }
                else if (TimeCheck.Checked || RepeatCheck.Checked || EndedCheck.Checked)
                {
                    if (TimePicker.DbSelectedDate != null)
                    {
                        execute = true;
                        cmd = new SqlCommand("INSERT INTO UserEventAlerts (ReoccurID, EventID, UserID, MHDW, MHDWNumb, " +
                            "RepeatMHDW, RepeatMHDWNumb, AlertWhenEnded, " +
                            "isEmail, isText, [On], isAllEvents,ServerTimeDifference) VALUES (" +
                            Request.QueryString["RID"].ToString() + ", " +
                            Request.QueryString["EID"].ToString() + ", " +
                            Session["User"].ToString() + ",@mhdw, @mhdwNum, @repeatMHDW, " +
                            "@repeatMHDWNum, @ended, @email, @text, @on, 'False', @time)", conn);
                    }
                    else
                    {
                        message = "The time in your location is required.";
                        execute = false;
                    }
                }
                else
                {
                    message = "You must enter alert criteria.";
                    execute = false;
                }
            }

            if (execute)
            {
                if (TimeCheck.Checked && TimeTextBox.Text.Trim() != "")
                {
                    cmd.Parameters.Add("@mhdw", SqlDbType.Int).Value = TimeDropDown.SelectedValue;
                    cmd.Parameters.Add("@mhdwNum", SqlDbType.Int).Value = int.Parse(TimeTextBox.Text.Trim());
                }
                else
                {
                    cmd.Parameters.Add("@mhdw", SqlDbType.Int).Value = DBNull.Value;
                    cmd.Parameters.Add("@mhdwNum", SqlDbType.Int).Value = DBNull.Value;
                }

                if (RepeatCheck.Checked && RepeatTextBox.Text.Trim() != "")
                {
                    cmd.Parameters.Add("@repeatMHDW", SqlDbType.Int).Value = RepeatDropDown.SelectedValue;
                    cmd.Parameters.Add("@repeatMHDWNum", SqlDbType.Int).Value = int.Parse(RepeatTextBox.Text.Trim());
                }
                else
                {
                    cmd.Parameters.Add("@repeatMHDW", SqlDbType.Int).Value = DBNull.Value;
                    cmd.Parameters.Add("@repeatMHDWNum", SqlDbType.Int).Value = DBNull.Value;
                }

                cmd.Parameters.Add("@lastRun", SqlDbType.DateTime).Value = DBNull.Value;
                cmd.Parameters.Add("@ended", SqlDbType.Bit).Value = EndedCheck.Checked;
                cmd.Parameters.Add("@email", SqlDbType.Bit).Value = EmailCheck.Checked;
                cmd.Parameters.Add("@text", SqlDbType.Bit).Value = TextCheck.Checked;
                cmd.Parameters.Add("@on", SqlDbType.Bit).Value = AlertRadioList.Items[0].Selected;

                message += TimePicker.DbSelectedDate.ToString();

                DateTime timNow = DateTime.Parse(TimePicker.DbSelectedDate.ToString());
                DateTime nowTime = DateTime.Now;
                if (nowTime.Second > 30)
                    nowTime = nowTime.AddSeconds(60 - nowTime.Second);
                else
                    nowTime = nowTime.AddSeconds(-nowTime.Second);

                TimeSpan timeDiff = nowTime - timNow;

                cmd.Parameters.Add("@time", SqlDbType.Int).Value = timeDiff.TotalSeconds;

                cmd.ExecuteNonQuery();

                MessageLabel.Text = "Your alert has been saved";
            }
            else
            {
                MessageLabel.Text = message;
            }
        }
        catch (Exception ex)
        {
            MessageLabel.Text = message + "<br/>" + ex.ToString();
        }
    }

    
   
}
