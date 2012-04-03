using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Controls_HippoRating : System.Web.UI.UserControl
{
    public string VENUE_ID
    {
        get { return vIDfromUserPage; }
        set { vIDfromUserPage = value; }
    }
    protected string vIDfromUserPage = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        try
        {
            FloatingLiteral.Text = "<div class=\"EventDiv\" style=\"padding-top: 3px;\">";
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            bool showRating = false;



            if (Session["User"] != null)
            {
                showRating = true;
            }
            else
            {


            }



            if (!IsPostBack)
            {
                string urlToken = Request.Url.Segments[Request.Url.Segments.Length - 1];

                SqlCommand cmd = new SqlCommand();

                switch (urlToken)
                {
                    case "Venue.aspx":
                        TitleLiteral.Text = "<span style=\"padding-top: 3px;\"><label>Rate this venue:  </label></span>";
                        string ID1 = Request.QueryString["ID"].ToString();
                        DataSet dsRating1 = dat.GetData("SELECT SUM(Rating) AS Rating FROM VenueRatings WHERE VenueID=" + ID1);
                        DataSet dsUsers1 = dat.GetData("SELECT UserID FROM VenueRatings WHERE VenueID=" + ID1);
                        DoLogic(dsRating1, dsUsers1, showRating, "V", ID1, "float:left;");
                        break;
                    case "Event.aspx":
                        TitleLiteral.Text = "<span style=\"padding-top: 3px;\"><label>Rate this event:  </label></span>";
                        string ID = Request.QueryString["EventID"].ToString();
                        DataSet dsRating = dat.GetData("SELECT SUM(Rating) AS Rating FROM EventRatings WHERE EventID=" + ID);
                        DataSet dsUsers = dat.GetData("SELECT UserID FROM EventRatings WHERE EventID=" + ID);
                        DoLogic(dsRating, dsUsers, showRating, "E", ID, "float:left;");
                        break;
                    case "Home.aspx":
                        TitleLiteral.Text = "<span style=\"padding-top: 3px;\"><label>Rate this event:  </label></span>";
                        DataSet ds2 = dat.GetData("SELECT * FROM Events WHERE Featured='True'");
                        string ID2 = ds2.Tables[0].Rows[0]["ID"].ToString();
                        DataSet dsRating2 = dat.GetData("SELECT SUM(Rating) AS Rating FROM EventRatings WHERE EventID=" + ID2);
                        DataSet dsUsers2 = dat.GetData("SELECT UserID FROM EventRatings WHERE EventID=" + ID2);
                        DoLogic(dsRating2, dsUsers2, showRating, "H", ID2, "float:left;");
                        break;
                    case "User.aspx":
                        TitleLiteral.Text = "";
                        DataSet dsRating12 = dat.GetData("SELECT SUM(Rating) AS Rating FROM VenueRatings WHERE VenueID=" + vIDfromUserPage);
                        DataSet dsUsers12 = dat.GetData("SELECT UserID FROM VenueRatings WHERE VenueID=" + vIDfromUserPage);
                        DoLogic(dsRating12, dsUsers12, false, "V", vIDfromUserPage, "float:right;");
                        FloatingLiteral.Text = "<div class=\"EventDiv\" style=\"float: right;padding-top: 3px;\">";
                        break;
                    default: break;
                }
            }
        }
        catch (SystemException ex)
        {
            Errorlabel.Text = ex.ToString();
        }
        catch (Exception ex)
        {
            Errorlabel.Text = ex.ToString();
        }
    }

    protected void ServerSetRating(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        try
        {
            string urlToken = Request.Url.Segments[Request.Url.Segments.Length - 1];
            HtmlButton theButton = (HtmlButton)sender;

            string theValue = theButton.Attributes["value"].ToString();
            SqlCommand cmd = new SqlCommand();
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            switch (urlToken)
            {
                case "Venue.aspx":
                    string ID1 = Request.QueryString["ID"].ToString();

                    dat.Execute("INSERT INTO VenueRatings (UserID, VenueID, Rating) VALUES(" + 
                        Session["User"].ToString() +
                        ", " + ID1 + ", " + theValue + ")");

                    DataSet dsRating1 = dat.GetData("SELECT SUM(Rating) AS Rating FROM VenueRatings WHERE VenueID=" + ID1);
                    DataSet dsUsers1 = dat.GetData("SELECT UserID FROM VenueRatings WHERE VenueID=" + ID1);
                    DoLogic(dsRating1, dsUsers1, true, "V", ID1, "float:right;");
                    break;
                case "Event.aspx":
                    string ID = Request.QueryString["EventID"].ToString();

                    dat.Execute("INSERT INTO EventRatings (UserID, EventID, Rating) VALUES(" + 
                        Session["User"].ToString() +
                        ", " + ID + ", " + theValue + ")");

                    DataSet dsRating = dat.GetData("SELECT SUM(Rating) AS Rating FROM EventRatings WHERE EventID=" + ID);
                    DataSet dsUsers = dat.GetData("SELECT UserID FROM EventRatings WHERE EventID=" + ID);
                    DoLogic(dsRating, dsUsers, true, "E", ID, "float:right;");
                    break;
                case "Home.aspx":
                    DataSet ds2 = dat.GetData("SELECT * FROM Events WHERE Featured='True'");

                    string ID2 = ds2.Tables[0].Rows[0]["ID"].ToString();

                    dat.Execute("INSERT INTO EventRatings (UserID, EventID, Rating) VALUES(" + Session["User"].ToString() +
                        ", " + ID2 + ", " + theValue + ")");

                    DataSet dsRating2 = dat.GetData("SELECT SUM(Rating) AS Rating FROM EventRatings WHERE EventID=" + ID2);
                    DataSet dsUsers2 = dat.GetData("SELECT UserID FROM EventRatings WHERE EventID=" + ID2);
                    DoLogic(dsRating2, dsUsers2, true, "H", ID2, "float:right;");
                    break;
                default: break;
            }

        }
        catch (Exception ex)
        {
            Errorlabel.Text = ex.ToString();
        }
    }

    protected void DoLogic(DataSet dsRating, DataSet dsUsers, bool showRating, string type, string ID, string floatLR)
    {
        try
        {
            bool rate = false;
            if (dsRating.Tables.Count > 0)
                if (dsRating.Tables[0].Rows.Count > 0)
                {
                    if (dsRating.Tables[0].Rows[0]["Rating"].ToString().Trim() != "")
                    {
                        int numRating = int.Parse(dsRating.Tables[0].Rows[0]["Rating"].ToString());
                        string numPeople = dsUsers.Tables[0].Rows.Count.ToString();

                        SetRating(numRating / int.Parse(numPeople), floatLR);

                        if (showRating)
                        {
                            DataView dv = new DataView(dsUsers.Tables[0], "UserID=" + 
                                Session["User"].ToString(), "", DataViewRowState.CurrentRows);
                            if (dv.Count > 0)
                            {
                                RatingPanel.Visible = true;
                                RatePanel.Visible = false;
                            }
                            else
                            {
                                RatingPanel.Visible = false;
                                RatePanel.Visible = true;
                            }
                        }
                        else
                            RatingPanel.Visible = true;
                    }
                    else
                    {
                        rate = true;
                        SetRating(0, floatLR);
                    }
                }
                else
                    rate = true;
            else
                rate = true;

            if (showRating)
            {
                if (rate)
                {
                    RatingPanel.Visible = false;
                    RatePanel.Visible = true;
                }
            }
            else
            {
                RatingPanel.Visible = true;
            }
        }
        catch (Exception ex)
        {
            Errorlabel.Text = ex.ToString();
        }
    }

    protected void SetRating(int rating, string floatLR)
    {
        try
        {
            RatingLiteral.Text = "";
            //RatingLiteral.Text = "<label style=\""+floatLR+" padding-right: 5px;\"></label>";
            for (int i = 1; i <= 10; i++)
            {
                if (i <= rating)
                {
                    if ((i % 2) == 0)
                        RatingLiteral.Text += "<img src=\"image/FullFullRightHalf.png\" id=\"img" + (i + 1).ToString() + "\" />";
                    else
                        RatingLiteral.Text += "<img src=\"image/FullFullHalf.png\" id=\"img" + (i + 1).ToString() + "\" />";
                }
                else
                {
                    if ((i % 2) == 0)
                        RatingLiteral.Text += "<img src=\"image/EmptyEmptyRightHalf.png\" id=\"img" + (10 - i).ToString() + "\" />";
                    else
                        RatingLiteral.Text += "<img src=\"image/EmptyEmptyHalf.png\" id=\"img" + (10 - i).ToString() + "\" />";
                }
            }
        }
        catch (Exception ex)
        {
            Errorlabel.Text = ex.ToString();
        }
    }

}
