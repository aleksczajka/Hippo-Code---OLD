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

public partial class CreateVenue : System.Web.UI.Page
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

        if (!IsPostBack)
        {
            Session["NewVenue"] = null;
            HtmlLink lk = new HtmlLink();
            HtmlHead head = (HtmlHead)Page.Header;
            lk.Attributes.Add("rel", "canonical");
            lk.Href = "http://hippohappenings.com/CreateVenue.aspx";
            head.Controls.AddAt(0, lk);
            CountryDropDown.ClearSelection();
            CountryDropDown.DataBind();
            CountryDropDown.Items.FindByValue("223").Selected = true;
            ChangeState(CountryDropDown, new EventArgs());
            try
            {
                if (Session["User"] != null)
                {
                }
            }
            catch (Exception ex)
            {
            }  
        }
    }

    protected void CreateNewVenue(object sender, EventArgs e)
    {
        //User created a new venue. Must save it
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        StateTextBox.Text = dat.stripHTML(StateTextBox.Text.Trim());
        VenueCityTextBox.Text = dat.stripHTML(VenueCityTextBox.Text.Trim());
        StreetNameTextBox.Text = dat.stripHTML(StreetNameTextBox.Text.Trim());
        StreetNumberTextBox.Text = dat.stripHTML(StreetNumberTextBox.Text.Trim());
        VenueNameTextBox.THE_TEXT = dat.stripHTML(VenueNameTextBox.THE_TEXT.Trim());
        ZipTextBox.Text = dat.stripHTML(ZipTextBox.Text.Trim());
        LocationTextBox.Text = dat.stripHTML(LocationTextBox.Text.Trim());
        string state = "";
        if (StateDropDownPanel.Visible)
            state = StateDropDown.SelectedItem.Text;
        else
            state = StateTextBox.Text;
        if (state != "" && ZipTextBox.Text.Trim() != "" && VenueCityTextBox.Text.Trim() != ""
            && VenueNameTextBox.THE_TEXT.Trim() != "" && StreetNumberTextBox.Text.Trim() != ""
            && StreetNameTextBox.Text.Trim() != "" && StreetDropDown.SelectedItem.Text != "Select One...")
        {
            SqlCommand cmd2;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
            conn.Open();

            cmd2 = new SqlCommand("INSERT INTO Venues (Name, Address, Country, State, City, Zip, " +
                "MediaCategory, CreatedByUser, Edit, Rating, PostedOn) VALUES " +
                "(@vName, @address, @country, @state, @city, @zip, 0, @user, 'True', 0, GETDATE())", conn);
            cmd2.Parameters.Add("@vName", SqlDbType.NVarChar).Value = VenueNameTextBox.THE_TEXT;
            cmd2.Parameters.Add("@user", SqlDbType.Int).Value = int.Parse(Session["User"].ToString());
            cmd2.Parameters.Add("@country", SqlDbType.Int).Value = int.Parse(CountryDropDown.SelectedValue);
            cmd2.Parameters.Add("@city", SqlDbType.NVarChar).Value = VenueCityTextBox.Text;
            cmd2.Parameters.Add("@zip", SqlDbType.NVarChar).Value = ZipTextBox.Text;


            string locationStr = "";
            string apt = "";
            if (AptNumberTextBox.Text.Trim() != "")
                apt = AptDropDown.SelectedItem.Text + " " + AptNumberTextBox.Text.Trim().ToLower();
            if (CountryDropDown.SelectedValue == "223")
            {
                locationStr = StreetNumberTextBox.Text.Trim().ToLower() + ";" + StreetNameTextBox.Text.Trim().ToLower()
                + ";" + StreetDropDown.SelectedItem.Text + ";" + apt;

            }
            else
            {
                locationStr = LocationTextBox.Text.Trim().ToLower() + ";" + apt;
            }

            cmd2.Parameters.Add("@state", SqlDbType.NVarChar).Value = state;
            cmd2.Parameters.Add("@address", SqlDbType.NVarChar).Value = locationStr;
            cmd2.ExecuteNonQuery();

            cmd2 = new SqlCommand("SELECT @@IDENTITY AS IDA", conn);

            SqlDataAdapter da1 = new SqlDataAdapter(cmd2);
            DataSet ds2 = new DataSet();
            da1.Fill(ds2);

            Session["NewVenue"] = ds2.Tables[0].Rows[0]["IDA"].ToString();

            conn.Close();

            LocationPanel.Visible = false;
            ThankYouPanel.Visible = true;
            //ThankYouLabel.Text = Session["NewVenue"].ToString();
        }
        else
        {
            ErrorLabel.Text = "Must include all requuired fields.";
        }

        //venueID = int.Parse(ds2.Tables[0].Rows[0]["ID"].ToString());

        //Fill the venues tooltip and make the venue just created selected in the venue box
    }

    protected void ChangeState(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet ds = dat.GetData("SELECT * FROM State WHERE country_id=" + CountryDropDown.SelectedValue);

        bool isTextBox = false;
        if (ds.Tables.Count > 0)
            if (ds.Tables[0].Rows.Count > 0)
            {
                StateDropDownPanel.Visible = true;
                StateTextBoxPanel.Visible = false;
                StateDropDown.DataSource = ds;
                StateDropDown.DataTextField = "state_2_code";
                StateDropDown.DataValueField = "state_id";
                StateDropDown.DataBind();
            }
            else
                isTextBox = true;
        else
            isTextBox = true;

        if (isTextBox)
        {
            StateTextBoxPanel.Visible = true;
            StateDropDownPanel.Visible = false;
        }

        if (CountryDropDown.SelectedValue == "223")
        {
            USPanel.Visible = true;
            InternationalPanel.Visible = false;
        }
        else
        {
            USPanel.Visible = false;
            InternationalPanel.Visible = true;
        }
    }

}
