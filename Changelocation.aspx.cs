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
using System.Collections.Generic;
using System.Data.OleDb;

public partial class Changelocation : Telerik.Web.UI.RadAjaxPage
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
        DateTime isn = DateTime.Now;

        DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn);

        Data dat = new Data(isn);
        SelectButton.SERVER_CLICK += SaveLocation;

        if (!IsPostBack)
        {
            CountryDropDown.SelectedValue = "223";

            DataSet ds3 = dat.GetData("SELECT * FROM State WHERE country_id=" + CountryDropDown.SelectedValue);

            StateDropDownPanel.Visible = true;
            StateTextBoxPanel.Visible = false;
            StateDropDown.DataSource = ds3;
            StateDropDown.DataTextField = "state_2_code";
            StateDropDown.DataValueField = "state_id";
            StateDropDown.DataBind();

            StateDropDown.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("State", "-1"));

            StateDropDownPanel.Visible = true;
            StateTextBoxPanel.Visible = false;
        }
    }

    protected void ChangeState(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        DateTime isn = DateTime.Now;

        DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn);

        Data dat = new Data(isn); DataSet ds = dat.GetData("SELECT * FROM State WHERE country_id=" + CountryDropDown.SelectedValue);

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

                StateDropDown.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("State", "-1"));

                
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

    }

    protected void SaveLocation(object sender, EventArgs e)
    {
        try
        {
            HttpCookie cookie = Request.Cookies["BrowserDate"];
            DateTime isn = DateTime.Now;

            DateTime.TryParse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"), out isn);

            Data dat = new Data(isn);
            if ((StateTextBoxPanel.Visible && StateTextBox.Text.Trim() != "") ||
                (!StateTextBoxPanel.Visible && StateDropDown.SelectedItem.Text != "State"))
            {
                if (CityTextBox.Text.Trim() != "")
                {
                    CityTextBox.Text = dat.stripHTML(CityTextBox.Text.Trim());
                    Session["LocCountry"] = CountryDropDown.SelectedItem.Value.Trim();
                    string state = "";
                    if (StateTextBoxPanel.Visible)
                    {
                        Session["LocState"] = StateTextBox.Text.Trim();
                        state = StateTextBox.Text.Trim();
                    }
                    else
                    {
                        Session["LocState"] = StateDropDown.SelectedItem.Text.Trim();
                        state = StateDropDown.SelectedItem.Text.Trim();
                    }

                    Session["LocCity"] = CityTextBox.Text.Trim();

                    if (CountryDropDown.SelectedValue == "223")
                    {
                        DataView dvCity = dat.GetDataDV("SELECT * FROM MajorZips MZ, MajorCities MC, State S WHERE " +
                        "MZ.MajorCityID=MC.ID AND S.state_2_code='" + state +
                        "' AND S.state_name=State AND MajorCity='" + CityTextBox.Text.Trim() + "'");

                        //ErrorLabel.Text = "SELECT * FROM MajorZips MZ, MajorCities MC, State S WHERE " +
                        //"MZ.MajorCityID=MC.ID AND S.state_2_code='" + state +
                        //"' AND S.state_name=State AND MajorCity='" + CityTextBox.Text.Trim() + "'";

                        string zip = "";

                        if (dvCity.Count == 0)
                        {
                            dvCity = dat.GetDataDV("SELECT * FROM MajorZips MZ, MajorCities MC, State S WHERE " +
                                "MZ.MajorCityID=MC.ID AND S.state_2_code='" + state +
                                "' AND S.state_name=State");
                            zip = dvCity[0]["MajorCityZip"].ToString();
                        }
                        else
                        {
                            zip = dvCity[0]["MajorCityZip"].ToString();
                        }

                        DataView dvZip = dat.GetDataDV("SELECT * FROM ZipCodes WHERE Zip=" + zip);
                        int count = 1;
                        while (dvZip.Count == 0)
                        {
                            if (dvCity.Count >= count + 1)
                            {
                                zip = dvCity[count]["MajorCityZip"].ToString();
                                count++;
                                dvZip = dat.GetDataDV("SELECT * FROM ZipCodes WHERE Zip=" + zip);
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (dvZip.Count != 0)
                        {
                            Session["LocLat"] = dvZip[0]["Latitude"].ToString();
                            Session["LocLong"] = dvZip[0]["Longitude"].ToString();
                        }
                    }

                    DataView dvCheck = dat.GetDataDV("SELECT * FROM IpToLocation WHERE IP='" + dat.GetIP() + "'");

                    if (dvCheck.Count > 0)
                    {
                        dat.Execute("UPDATE IpToLocation SET State = '" + Session["LocState"].ToString() +
                            "', City = '" + Session["LocCity"].ToString() + "', Country = '" + Session["LocCountry"].ToString() + "' WHERE IP='" + dat.GetIP() + "'");
                    }
                    else
                    {
                        dat.Execute("INSERT INTO IpToLocation (State, City, Country, IP) VALUES ('" + Session["LocState"].ToString() +
                            "', '" + Session["LocCity"].ToString() + "', '" + Session["LocCountry"].ToString() + "','" + dat.GetIP() + "')");
                    }

                    BottomScript.Text = "<script type=\"text/javascript\">Search('anything');</script>";
                }
                else
                {
                    ErrorLabel.Text = "<br />Please Include the City.<br /><br />";
                }
            }
            else
            {
                ErrorLabel.Text = "<br />Please Include the State.<br /><br />";
            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text += ex.ToString();
        }
    }
}

