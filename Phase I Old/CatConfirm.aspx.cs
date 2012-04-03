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


public partial class CatConfirm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlMeta hm = new HtmlMeta();
        HtmlHead head = (HtmlHead)Page.Header;
        hm.Name = "ROBOTS";
        hm.Content = "NOINDEX, FOLLOW";
        head.Controls.AddAt(0, hm);

        if (!IsPostBack)
        {
            CountryDropDown.SelectedValue = "223";
            ChangeTheState("223");
            Session["UserCountry"] = "223";
            Encryption decrypt = new Encryption();
            if (Request.QueryString["message"] != null)
                MessageLabel.Text = decrypt.decrypt(Request.QueryString["message"].ToString());
        }

        //VariablesLiteral.Text = "<a style=\"cursor: pointer; font-size: 20px;\" class=\"AddLink\" onclick=\"Search('User.aspx?ID=" + Request.QueryString["ID"].ToString() + "');\">Ok</a>";


    }
    protected void ChangeState(object sender, EventArgs e)
    {
        ChangeTheState(CountryDropDown.SelectedValue);
    }

    protected void ChangeTheState(string country)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet ds = dat.GetData("SELECT * FROM State WHERE country_id=" + country);

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

                StateDropDown.Items.Insert(0, new ListItem("Select State...", "-1"));
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

        if (country == "223")
        {
            RadiusPanel.Visible = true;
        }
        else
        {
            RadiusPanel.Visible = false;
        }
    }

    protected void Save(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data d = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        bool hasState = false;
        if (StateDropDown.Visible && StateDropDown.SelectedValue != "-1")
            hasState = true;

        if (!StateDropDown.Visible && StateTextBox.THE_TEXT.Trim() != "")
            hasState = true;

        bool hasCity = false;
        if (CityTextBox.THE_TEXT.Trim() != "")
            hasCity = true;


        if (hasState && hasCity)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
            conn.Open();
            string ID = Request.QueryString["ID"].Trim();

            string radius = "";
            if (RadiusPanel.Visible)
            {
                radius = ", Radius=" + RadiusDropDown.SelectedValue;
            }

            SqlCommand cmd = new SqlCommand("UPDATE UserPreferences SET  " +
                " CatCountry=@catCountry, CatState=@catState, CatCity=@catCity, CatZip=@catZip" + radius +
                " WHERE UserID=@id ", conn);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = ID;
            cmd.Parameters.Add("@catCity", SqlDbType.NVarChar).Value = CityTextBox.THE_TEXT;

            if (CatZipTextBox.THE_TEXT.Trim() != "")
                cmd.Parameters.Add("@catZip", SqlDbType.NVarChar).Value = d.GetAllZipsInRadius(RadiusDropDown.SelectedValue,
                            CatZipTextBox.THE_TEXT.Trim(), true);
            else
                cmd.Parameters.Add("@catZip", SqlDbType.NVarChar).Value = DBNull.Value;


            cmd.Parameters.Add("@catCountry", SqlDbType.Int).Value = CountryDropDown.SelectedValue;
            Session["UserCountry"] = CountryDropDown.SelectedValue;
            string state = "";
            if (StateDropDownPanel.Visible)
                state = StateDropDown.SelectedItem.Text;
            else
                state = StateTextBox.THE_TEXT;

            cmd.Parameters.Add("@catState", SqlDbType.NVarChar).Value = state;

            Session["UserState"] = state;
            Session["UserCity"] = CityTextBox;
            Session["UserCity"] = CatZipTextBox.THE_TEXT.Trim();

            cmd.ExecuteNonQuery();
            conn.Close();

            LocationPanel.Visible = false;
            DonePanel.Visible = true;
        }
        else
        {
            MessageLabel.Text = "Please Include City and State.";
        }

    }

}
