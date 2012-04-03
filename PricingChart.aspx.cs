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

public partial class PricingChart : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HtmlMeta hm = new HtmlMeta();
            HtmlHead head = (HtmlHead)Page.Header;
            hm.Name = "ROBOTS";
            hm.Content = "INDEX, FOLLOW";
            head.Controls.AddAt(0, hm);

            HtmlLink lk = new HtmlLink();
            lk.Attributes.Add("rel", "canonical");
            lk.Href = "http://hippohappenings.com/PricingChart.aspx";
            head.Controls.AddAt(0, lk);

            HttpCookie cookie = Request.Cookies["BrowserDate"];
            if (cookie == null)
            {
                cookie = new HttpCookie("BrowserDate");
                cookie.Value = DateTime.Now.ToString();
                cookie.Expires = DateTime.Now.AddDays(22);
                Response.Cookies.Add(cookie);
            }
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            bool isEvent = false;
            bool isVenue = false;
            bool isAd = false;
            switch (Request.QueryString["P"])
            {
                case "E":
                    isEvent = true;
                    TitleLabel.Text = "Pricing for Featuring an Event";
                    DescriptLabel.Text = "*Prices for featuring an event are based on the number of members in the particular city, " +
                                        "number of existing featured events on the specific day (the more featured events, the less it costs to feature an event), number of selected search terms and number of chosen days to feature the event.";
                    AdjustmentLabel.Text = "Adjustment for Events (days total)";
                    break;
                case "V":
                    isVenue = true;
                    TitleLabel.Text = "Pricing for Featuring a Locale";
                    DescriptLabel.Text = "*Prices for featuring a locale are based on the number of members in the particular city, " +
                        "number of existing featured locales on the specific day (the more featured locales, the less it costs to " +
                        "feature a locale), number of selected search terms and number of chosen days to feature the locale.";
                    AdjustmentLabel.Text = "Adjustment for Locales (days total)";
                    break;
                case "A":
                    isAd = true;
                    TitleLabel.Text = "Pricing for Featuring a Bulletin";
                    DescriptLabel.Text = "*Prices for featuring a bulletin are based on the number of members in the particular city, " +
                        "number of existing featured bulletins on the specific day (the more featured bulletins, the less it costs to " +
                        "feature a bulletin), number of selected search terms and number of chosen days to feature the bulletin.";
                    AdjustmentLabel.Text = "Adjustment for Bulletins (days total)";
                    break;
                case "T":
                    isAd = true;
                    TitleLabel.Text = "Pricing for Featuring an Adventure";
                    DescriptLabel.Text = "*Prices for featuring an adventure are based on the number of members in the particular city, " +
                        "number of existing featured adventures on the specific day (the more featured adventures, the less it costs to " +
                        "feature an adventure), number of selected search terms and number of chosen days to feature the adventure.";
                    AdjustmentLabel.Text = "Adjustment for Adventures (days total)";
                    break;
                default: break;
            }



            PricesButton.SERVER_CLICK += FillChart;

            if (!IsPostBack)
            {
                CountryDropDown.DataBind();
                CountryDropDown.SelectedValue = "223";
                ChangeState(CountryDropDown, new EventArgs());
                ChangeCity(StateDropDown, new EventArgs());
                if (Session["User"] != null)
                {
                    DataView dvUser = dat.GetDataDV("SELECT * FROM Users U, UserPreferences UP WHERE " +
                        "UP.UserID=U.User_ID AND U.User_ID=" + Session["User"].ToString());
                    CountryDropDown.ClearSelection();
                    CountryDropDown.Items.FindByValue(dvUser[0]["CatCountry"].ToString()).Selected = true;
                    ChangeState(CountryDropDown, new EventArgs());

                    if (StateDropDownPanel.Visible)
                    {
                        StateDropDown.ClearSelection();
                        StateDropDown.Items.FindByText(dvUser[0]["CatState"].ToString()).Selected = true;
                    }
                    else
                    {
                        StateTextBox.Text = dvUser[0]["CatState"].ToString();
                    }
                    ChangeCity(StateDropDown, new EventArgs());
                    FillChart(new object(), new EventArgs());
                    PricesPanel.Visible = true;
                }
                else
                {
                    CountryDropDown.ClearSelection();
                    CountryDropDown.Items.FindByValue("223").Selected = true;
                    PricesPanel.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString() + CountryDropDown.SelectedValue.ToString();
        }
    }

    protected void FillChart(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        DateTime isNow = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
        Data dat = new Data(isNow);

        CityTextBox.Text = dat.stripHTML(CityTextBox.Text.Trim());

        bool isEvent = false;
        bool isVenue = false;
        bool isAd = false;
        bool isTrip = false;
        string city = "";
        
        switch (Request.QueryString["P"])
        {
            case "E":
                isEvent = true;
                break;
            case "V":
                isVenue = true;
                break;
            case "A":
                isAd = true;
                break;
            case "T":
                isTrip = true;
                break;
            default: break;
        }

        bool goOn = true;
        string state = "";
        if (!StateDropDownPanel.Visible && StateTextBox.Text.Trim() == "")
        {
            goOn = false;
        }
        else
        {
            if (!CityDropDownPanel.Visible && CityTextBox.Text.Trim() == "")
                goOn = false;
            else
            {
                if (StateDropDownPanel.Visible)
                    state = StateDropDown.SelectedItem.Text;
                else
                    state = StateTextBox.Text;

                if (CityDropDownPanel.Visible)
                {
                    LocationPricesLabel.Text = MajorCityDrop.SelectedItem.Text + ", " + state;
                    city = MajorCityDrop.SelectedItem.Text;
                }
                else
                {
                    LocationPricesLabel.Text = CityTextBox.Text + ", " + state;
                    city = CityTextBox.Text;
                }
            }
        }


        if (goOn)
        {

            DataView dvChart;

            switch (Request.QueryString["P"])
            {
                case "E":
                    dvChart = dat.GetDataDV("SELECT * FROM StandardPricing");
                    break;
                case "V":
                    dvChart = dat.GetDataDV("SELECT * FROM StandardVenuePricing");
                    break;
                case "A":
                    dvChart = dat.GetDataDV("SELECT * FROM StandardBulletinPricing");
                    break;
                case "T":
                    dvChart = dat.GetDataDV("SELECT * FROM StandardTripPricing");
                    break;
                default:
                    dvChart = dat.GetDataDV("SELECT * FROM StandardPricing");
                    break;
            }

                

            //1 Day, 1 Search Term
                NumberDays1Label.Text = "1";
                string standardPricing = "";
            decimal eventCountDay1 = 0.00M;
            decimal eventCountDay2 = 0.00M;
            decimal eventCountDay3 = 0.00M;
            decimal eventCountDay4 = 0.00M;
            decimal eventCountDay5 = 0.00M;
            decimal eventCountDay6 = 0.00M;
            decimal eventCountDay7 = 0.00M;

            decimal subtractionForEventsDay1 = 0.00M;
            decimal subtractionForEventsDay2 = 0.00M;
            decimal subtractionForEventsDay3 = 0.00M;
            decimal subtractionForEventsDay4 = 0.00M;
            decimal subtractionForEventsDay5 = 0.00M;
            decimal subtractionForEventsDay6 = 0.00M;
            decimal subtractionForEventsDay7 = 0.00M;

                switch (Request.QueryString["P"])
                {
                    case "E":
                        standardPricing = "StandardEventPricing";
                        StandardRate1Label.Text = "$" + decimal.Round(decimal.Parse(dvChart[0][standardPricing].ToString()), 2).ToString();
                        break;
                    case "V":
                        standardPricing = "StandardVenuePricing";
                        StandardRate1Label.Text = "$" + decimal.Round(decimal.Parse(dvChart[0][standardPricing].ToString()), 2).ToString();
                        break;
                    case "A":
                        standardPricing = "StandardBulletinPricing";
                        StandardRate1Label.Text = "$" + decimal.Round(decimal.Parse(dvChart[0][standardPricing].ToString()), 2).ToString();
                        break;
                    case "T":
                        standardPricing = "StandardTripPricing";
                        StandardRate1Label.Text = "$" + decimal.Round(decimal.Parse(dvChart[0][standardPricing].ToString()), 2).ToString();
                        break;
                    default: break;
                }

                DataView dvCountMembers;
                string zips = "";
                if (CountryDropDown.SelectedValue == "223")
                {
                    //DataView dvMajorZips = dat.GetDataDV("SELECT * FROM MajorCities MC, MajorZips MZ, State S WHERE " +
                    //    "MC.MajorCity='" + MajorCityDrop.SelectedItem.Text + "' AND MC.State=S.state_name AND S.state_2_code='" +
                    //    StateDropDown.SelectedItem.Text + "' AND MC.ID=MZ.MajorCityID");

                    
                    //DataView dv50RadiusZips;
                    //DataView dvLatLong;
                    //foreach (DataRowView row in dvMajorZips)
                    //{
                    //    if (zips != "")
                    //        zips += " OR ";
                    //    zips += " CatZip=" + row["MajorCityZip"].ToString();
                        
                    //    dvLatLong = dat.GetDataDV("SELECT * FROM ZipCodes WHERE Zip=" + row["MajorCityZip"].ToString());
                    //    if (dvLatLong.Count > 0)
                    //    {
                    //        dv50RadiusZips = dat.GetDataDV("SELECT * FROM ZipCodes WHERE dbo.GetDistance(" +
                    //            dvLatLong[0]["Longitude"].ToString() + ", " + dvLatLong[0]["Latitude"].ToString() + ", Longitude, Latitude) <= 5 ");

                    //        if (dv50RadiusZips.Count > 0)
                    //        {
                    //            foreach (DataRowView rowster in dv50RadiusZips)
                    //            {
                    //                zips += " OR CatZip=" + rowster["Zip"].ToString();
                    //            }
                    //        }
                    //    }
                    //}

                    zips = dat.GetDataDV("SELECT AllZips FROM MajorCities WHERE ID=" + MajorCityDrop.SelectedValue)[0]["AllZips"].ToString();

                    dvCountMembers = dat.GetDataDV("SELECT COUNT(*) AS Count FROM UserPreferences WHERE MajorCity = " +
                        MajorCityDrop.SelectedItem.Value);

                }
                else
                {
                    dvCountMembers = dat.GetDataDV("SELECT COUNT(*) AS Count FROM UserPreferences WHERE CatCity = '" +
                        city + "' AND CatCountry = " + CountryDropDown.SelectedValue + " AND CatState = '" + state + "'");

                }

                DataView dvCountEvents;
                string perCap = "";
                string perPricing = "";
                

                if (CountryDropDown.SelectedValue == "223")
                {
                    switch (Request.QueryString["P"])
                    {
                        case "E":
                            perPricing = "PerEventPricing";
                            perCap = "PerEventCap";
                            zips = zips.Replace("CatZip", "Zip");
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND (" + zips +
                                ") AND DaysFeatured LIKE '%;" + isNow.Month.ToString() + "/" +
                            isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";%'");
                            break;
                        case "V":
                            perCap = "PerVenueCap";
                            perPricing = "PerVenuePricing";
                            zips = zips.Replace("CatZip", "Zip");
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Venues WHERE Featured='True' AND (" + zips +
                                ") AND DaysFeatured LIKE '%;" + isNow.Month.ToString() + "/" +
                            isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";%'");
                            break;
                        case "A":
                            perCap = "PerBulletinCap";
                            perPricing = "PerBulletinPricing";
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Ads WHERE Featured='True' AND CatCity='"+
                                MajorCityDrop.SelectedItem.Text+"' AND DatesOfAd LIKE '%;" + isNow.Month.ToString() + "/" +
                            isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";%'");
                            break;
                        case "T":
                            perCap = "PerTripCap";
                            perPricing = "PerTripPricing";
                            zips = zips.Replace("CatZip", "TD.Zip");
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(DISTINCT TripID) AS Count FROM Trips T, "+
                                "TripDirections TD WHERE T.ID=TD.TripID AND T.Featured='True'  AND (" + zips +
                                ") AND T.DaysFeatured LIKE '%;" + isNow.Month.ToString() + "/" +
                               isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";%'");
                            break;
                        default:
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND (" + zips +
                                ") AND DaysFeatured LIKE '%;" + isNow.Month.ToString() + "/" +
                            isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";%'");
                            break;
                    }
                }
                else
                {
                    switch (Request.QueryString["P"])
                    {
                        case "E":
                            perPricing = "PerEventPricing";
                            perCap = "PerEventCap";
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND City = '" +
                            CityTextBox.Text + "' AND Country = " + CountryDropDown.SelectedValue +
                            " AND State = '" + state + "' AND DaysFeatured LIKE '%;" + isNow.Month.ToString() + "/" +
                            isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";%'");
                            
                            break;
                        case "V":
                            perCap = "PerVenueCap";
                            perPricing = "PerVenuePricing";
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Venues WHERE Featured='True' AND City = '" +
                               CityTextBox.Text + "' AND Country = " + CountryDropDown.SelectedValue +
                               " AND State = '" + state + "' AND DaysFeatured LIKE '%;" + isNow.Month.ToString() + "/" +
                               isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";%'");
                            break;
                        case "A":
                            perCap = "PerBulletinCap";
                            perPricing = "PerBulletinPricing";
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Ads WHERE Featured='True' AND CatCity = '" +
                               CityTextBox.Text + "' AND CatCountry = " + CountryDropDown.SelectedValue +
                               " AND CatState = '" + state + "' AND DatesOfAd LIKE '%;" + isNow.Month.ToString() + "/" +
                               isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";%'");
                            break;
                        case "T":
                            perCap = "PerTripCap";
                            perPricing = "PerTripPricing";
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(DISTINCT TripID) AS Count FROM Trips T, TripDirections TD WHERE T.ID=TD.TripID AND T.Featured='True' AND TD.City = '" +
                                CityTextBox.Text + "' AND TD.Country = " + CountryDropDown.SelectedValue +
                                " AND TD.State = '" + state + "' AND T.DaysFeatured LIKE '%;" + isNow.Month.ToString() + "/" +
                               isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";%'");
                            break;
                        default:
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND City = '" +
                            CityTextBox.Text + "' AND Country = " + CountryDropDown.SelectedValue +
                            " AND State = '" + state + "' AND DaysFeatured LIKE '%;" + isNow.Month.ToString() + "/" +
                            isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";%'");
                            break;
                    }
                }

                eventCountDay1 = decimal.Parse(dvCountEvents[0]["Count"].ToString());

                SearchTerms1Label.Text = "$0.05";

                decimal memberCap = decimal.Round(decimal.Parse(dvChart[0]["PerMemberCap"].ToString()), 2);
                decimal eventCap = 0.00M;

                eventCap = decimal.Round(decimal.Parse(dvChart[0][perCap].ToString()), 2);

                decimal priceForMembers = 0.00M;
                decimal subtractionForEvents = 0.00M;

                if (decimal.Parse(dvChart[0]["PerMemberPricing"].ToString()) * decimal.Parse(dvCountMembers[0]["Count"].ToString()) > memberCap)
                {
                    priceForMembers = memberCap;
                }
                else
                {
                    priceForMembers = decimal.Round(decimal.Parse(dvChart[0]["PerMemberPricing"].ToString()) * 
                        decimal.Parse(dvCountMembers[0]["Count"].ToString()), 3);
                }

                if (decimal.Parse(dvChart[0][perPricing].ToString()) * decimal.Parse(dvCountEvents[0]["Count"].ToString()) > eventCap)
                {
                    subtractionForEvents = eventCap;
                }
                else
                {
                    subtractionForEvents = decimal.Round(decimal.Parse(dvChart[0][perPricing].ToString()) * 
                        decimal.Parse(dvCountEvents[0]["Count"].ToString()), 3);
                }
                
                MembersAdjustment1Label.Text = "$" + priceForMembers.ToString() + " (" + decimal.Round(decimal.Parse(dvChart[0]["PerMemberPricing"].ToString()), 3).ToString() + "/user)";
                EventsAdjustmentLabel.Text = "-$" + decimal.Round(subtractionForEvents, 2, MidpointRounding.AwayFromZero).ToString();
                //Price1Label.Text = "$" + decimal.Round((decimal.Parse(dvChart[0][standardPricing].ToString()) +
                //    priceForMembers - subtractionForEvents + 0.05M), 2).ToString();

                PriceTotal1Label.Text = "$" + decimal.Round((decimal.Parse(dvChart[0][standardPricing].ToString()) +
                    priceForMembers - subtractionForEvents + 0.05M), 2).ToString();

            //2 Day, 1 Search Term
                isNow = isNow.AddDays(1.00);
                NumberDays2Label.Text = "2";
                StandardRate2Label.Text = "$" + decimal.Round(decimal.Parse(dvChart[1][standardPricing].ToString()), 2).ToString();

                if (CountryDropDown.SelectedValue == "223")
                {
                    switch (Request.QueryString["P"])
                    {
                        case "E":
                            perPricing = "PerEventPricing";
                            perCap = "PerEventCap";
                            zips = zips.Replace("CatZip", "Zip");
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND (" + zips +
                                ") AND DaysFeatured LIKE '%;" + isNow.Month.ToString() + "/" +
                            isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";%'");
                            break;
                        case "V":
                            perCap = "PerVenueCap";
                            perPricing = "PerVenuePricing";
                            zips = zips.Replace("CatZip", "Zip");
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Venues WHERE Featured='True' AND (" + zips +
                                ") AND DaysFeatured LIKE '%;" + isNow.Month.ToString() + "/" +
                            isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";%'");
                            break;
                        case "A":
                            perCap = "PerBulletinCap";
                            perPricing = "PerBulletinPricing";
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Ads WHERE Featured='True' AND CatCity='" +
                                MajorCityDrop.SelectedItem.Text + "' AND DatesOfAd LIKE '%;" + isNow.Month.ToString() + "/" +
                            isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";%'");
                            break;
                        case "T":
                            perCap = "PerTripCap";
                            perPricing = "PerTripPricing";
                            zips = zips.Replace("CatZip", "TD.Zip");
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(DISTINCT TripID) AS Count FROM Trips T, " +
                                "TripDirections TD WHERE T.ID=TD.TripID AND T.Featured='True'  AND (" + zips +
                                ") AND T.DaysFeatured LIKE '%;" + isNow.Month.ToString() + "/" +
                               isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";%'");
                            break;
                        default:
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND (" + zips +
                                ") AND DaysFeatured LIKE '%;" + isNow.Month.ToString() + "/" +
                            isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";%'");
                            break;
                    }
                }
                else
                {
                    switch (Request.QueryString["P"])
                    {
                        case "E":
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND City = '" +
                            CityTextBox.Text + "' AND Country = " + CountryDropDown.SelectedValue +
                            " AND State = '" + state + "' AND DaysFeatured LIKE '%;" + isNow.Month.ToString() + "/" +
                            isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";%'");
                            break;
                        case "V":
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Venues WHERE Featured='True' AND City = '" +
                               CityTextBox.Text + "' AND Country = " + CountryDropDown.SelectedValue +
                               " AND State = '" + state + "' AND DaysFeatured LIKE '%;" + isNow.Month.ToString() + "/" +
                               isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";%'");
                            break;
                        case "A":
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Ads WHERE Featured='True' AND CatCity = '" +
                               CityTextBox.Text + "' AND CatCountry = " + CountryDropDown.SelectedValue +
                               " AND CatState = '" + state + "' AND DatesOfAd LIKE '%;" + isNow.Month.ToString() + "/" +
                               isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";%'");
                            break;
                        case "T":
                            perCap = "PerTripCap";
                            perPricing = "PerTripPricing";
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(DISTINCT TripID) AS Count FROM Trips T, TripDirections TD WHERE T.ID=TD.TripID AND T.Featured='True' AND TD.City = '" +
                                CityTextBox.Text + "' AND TD.Country = " + CountryDropDown.SelectedValue +
                                " AND TD.State = '" + state + "' AND T.DaysFeatured LIKE '%;" + isNow.Month.ToString() + "/" +
                               isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";%'");
                            break;
                        default:
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND City = '" +
                            CityTextBox.Text + "' AND Country = " + CountryDropDown.SelectedValue +
                            " AND State = '" + state + "' AND DaysFeatured LIKE '%;" + isNow.Month.ToString() + "/" +
                            isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";%'");
                            break;
                    }
                }

            eventCountDay2 = decimal.Parse(dvCountEvents[0]["Count"].ToString());
                SearchTerms2Label.Text = "$0.10";

                memberCap = decimal.Round(decimal.Parse(dvChart[1]["PerMemberCap"].ToString()), 2);
                eventCap = decimal.Round(decimal.Parse(dvChart[1][perCap].ToString()), 2);
                priceForMembers = 0.00M;
                subtractionForEvents = 0.00M;

                if (decimal.Parse(dvChart[1]["PerMemberPricing"].ToString()) * decimal.Parse(dvCountMembers[0]["Count"].ToString()) > memberCap)
                {
                    priceForMembers = memberCap;
                }
                else
                {
                    priceForMembers = decimal.Round(decimal.Parse(dvChart[1]["PerMemberPricing"].ToString()) * 
                        decimal.Parse(dvCountMembers[0]["Count"].ToString()), 3);
                }

                if (decimal.Parse(dvChart[1][perPricing].ToString()) * eventCountDay1 > eventCap)
                {
                    subtractionForEvents = eventCap;
                }
                else
                {
                    subtractionForEvents = decimal.Round(decimal.Parse(dvChart[1][perPricing].ToString()) * 
                        eventCountDay1, 3);
                }

                //Second Day
                if (decimal.Parse(dvChart[1][perPricing].ToString()) * eventCountDay2 > eventCap)
                {
                    subtractionForEventsDay2 = eventCap;
                }
                else
                {
                    subtractionForEventsDay2 = decimal.Round(decimal.Parse(dvChart[1][perPricing].ToString()) * 
                        eventCountDay2, 3);
                }
                MembersAdjustment2Label.Text = "$" + priceForMembers.ToString() + " (" + decimal.Round(decimal.Parse(dvChart[1]["PerMemberPricing"].ToString()), 3).ToString() + "/user)";
                EventsAdjustment2Label.Text = "-$" + decimal.Round((subtractionForEvents + subtractionForEventsDay2), 2, MidpointRounding.AwayFromZero).ToString();
                //Price2Label.Text = "$" + decimal.Round((decimal.Parse(dvChart[1][standardPricing].ToString()) +
                //    priceForMembers - subtractionForEvents + 0.05M), 2).ToString();

            //Instead of counting both days the same, just add the previous to the second
                PriceTotal2Label.Text = "$" + decimal.Round((2.00M *decimal.Parse(dvChart[1][standardPricing].ToString()) +
                    (2.00M * priceForMembers) - (subtractionForEvents + subtractionForEventsDay2) + 0.10M), 2).ToString();

                //PriceTotal2Label.Text = "$" + (decimal.Round((decimal.Parse(dvChart[1][standardPricing].ToString()) +
                //        (priceForMembers) - (subtractionForEvents) + 0.05M), 2) + decimal.Parse(PriceTotal1Label.Text.Replace("$", ""))).ToString();

                PricesPanel.Visible = true;

            //3 Day, 1 Search Term
                isNow = isNow.AddDays(1.00);
                NumberDays3Label.Text = "3";
                StandardRate3Label.Text = "$" + decimal.Round(decimal.Parse(dvChart[2][standardPricing].ToString()), 2).ToString();

                if (CountryDropDown.SelectedValue == "223")
                {
                    switch (Request.QueryString["P"])
                    {
                        case "E":
                            perPricing = "PerEventPricing";
                            perCap = "PerEventCap";
                            zips = zips.Replace("CatZip", "Zip");
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND (" + zips +
                                ") AND DaysFeatured LIKE '%;" + isNow.Month.ToString() + "/" +
                            isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";%'");
                            break;
                        case "V":
                            perCap = "PerVenueCap";
                            perPricing = "PerVenuePricing";
                            zips = zips.Replace("CatZip", "Zip");
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Venues WHERE Featured='True' AND (" + zips +
                                ") AND DaysFeatured LIKE '%;" + isNow.Month.ToString() + "/" +
                            isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";%'");
                            break;
                        case "A":
                            perCap = "PerBulletinCap";
                            perPricing = "PerBulletinPricing";
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Ads WHERE Featured='True' AND CatCity='" +
                                MajorCityDrop.SelectedItem.Text + "' AND DatesOfAd LIKE '%;" + isNow.Month.ToString() + "/" +
                            isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";%'");
                            break;
                        case "T":
                            perCap = "PerTripCap";
                            perPricing = "PerTripPricing";
                            zips = zips.Replace("CatZip", "TD.Zip");
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(DISTINCT TripID) AS Count FROM Trips T, " +
                                "TripDirections TD WHERE T.ID=TD.TripID AND T.Featured='True'  AND (" + zips +
                                ") AND T.DaysFeatured LIKE '%;" + isNow.Month.ToString() + "/" +
                               isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";%'");
                            break;
                        default:
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND (" + zips +
                                ") AND DaysFeatured LIKE '%;" + isNow.Month.ToString() + "/" +
                            isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";%'");
                            break;
                    }
                }
                else
                {
                    switch (Request.QueryString["P"])
                    {
                        case "E":
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND City = '" +
                            CityTextBox.Text + "' AND Country = " + CountryDropDown.SelectedValue +
                            " AND State = '" + state + "' AND DaysFeatured LIKE '%;" + isNow.Month.ToString() + "/" +
                            isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";%'");
                            break;
                        case "V":
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Venues WHERE Featured='True' AND City = '" +
                               CityTextBox.Text + "' AND Country = " + CountryDropDown.SelectedValue +
                               " AND State = '" + state + "' AND DaysFeatured LIKE '%;" + isNow.Month.ToString() + "/" +
                               isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";%'");
                            break;
                        case "A":
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Ads WHERE Featured='True' AND CatCity = '" +
                               CityTextBox.Text + "' AND CatCountry = " + CountryDropDown.SelectedValue +
                               " AND CatState = '" + state + "' AND DatesOfAd LIKE '%;" + isNow.Month.ToString() + "/" +
                               isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";%'");
                            break;
                        case "T":
                            perCap = "PerTripCap";
                            perPricing = "PerTripPricing";
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(DISTINCT TripID) AS Count FROM Trips T, TripDirections TD WHERE T.ID=TD.TripID AND T.Featured='True' AND TD.City = '" +
                                CityTextBox.Text + "' AND TD.Country = " + CountryDropDown.SelectedValue +
                                " AND TD.State = '" + state + "' AND T.DaysFeatured LIKE '%;" + isNow.Month.ToString() + "/" +
                               isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";%'");
                            break;
                        default:
                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND City = '" +
                            CityTextBox.Text + "' AND Country = " + CountryDropDown.SelectedValue +
                            " AND State = '" + state + "' AND DaysFeatured LIKE '%;" + isNow.Month.ToString() + "/" +
                            isNow.Day.ToString() + "/" + isNow.Year.ToString() + ";%'");
                            break;
                    }
                }

                eventCountDay3 = decimal.Parse(dvCountEvents[0]["Count"].ToString());

                SearchTerms3Label.Text = "$0.15";

                memberCap = decimal.Round(decimal.Parse(dvChart[2]["PerMemberCap"].ToString()), 2);
                eventCap = decimal.Round(decimal.Parse(dvChart[2][perCap].ToString()), 2);
                priceForMembers = 0.00M;
                subtractionForEvents = 0.00M;

                if (decimal.Parse(dvChart[2]["PerMemberPricing"].ToString()) * decimal.Parse(dvCountMembers[0]["Count"].ToString()) > memberCap)
                {
                    priceForMembers = memberCap;
                }
                else
                {
                    priceForMembers = decimal.Round(decimal.Parse(dvChart[2]["PerMemberPricing"].ToString()) *
                        decimal.Parse(dvCountMembers[0]["Count"].ToString()), 3);
                }

            //Day 1
                if (decimal.Parse(dvChart[2][perPricing].ToString()) * eventCountDay1 > eventCap)
                {
                    subtractionForEvents = eventCap;
                }
                else
                {
                    subtractionForEvents = decimal.Round(decimal.Parse(dvChart[2][perPricing].ToString()) *
                       eventCountDay1, 3);
                }

            //Day 2
                if (decimal.Parse(dvChart[2][perPricing].ToString()) * eventCountDay2 > eventCap)
                {
                    subtractionForEventsDay2 = eventCap;
                }
                else
                {
                    subtractionForEventsDay2 = decimal.Round(decimal.Parse(dvChart[2][perPricing].ToString()) *
                        eventCountDay2, 3);
                }

            //Day 3
                if (decimal.Parse(dvChart[2][perPricing].ToString()) * eventCountDay3 > eventCap)
                {
                    subtractionForEventsDay3 = eventCap;
                }
                else
                {
                    subtractionForEventsDay3 = decimal.Round(decimal.Parse(dvChart[2][perPricing].ToString()) *
                        eventCountDay3, 3);
                }

                MembersAdjustment3Label.Text = "$" + priceForMembers.ToString() + " (" + decimal.Round(decimal.Parse(dvChart[2]["PerMemberPricing"].ToString()), 3).ToString() + "/user)";
                EventsAdjustment3Label.Text = "-$" + decimal.Round(subtractionForEvents + subtractionForEventsDay2 + 
                    subtractionForEventsDay3, 2, MidpointRounding.AwayFromZero).ToString();
                //Price3Label.Text = "$" + decimal.Round((decimal.Parse(dvChart[2][standardPricing].ToString()) +
                //    priceForMembers - subtractionForEvents + 0.05M), 2).ToString();

            //Instead of counting both days the same, just add the previous to the second
                PriceTotal3Label.Text = "$" + decimal.Round((3.00M * decimal.Parse(dvChart[2][standardPricing].ToString()) +
                    (3.00M * priceForMembers) - (subtractionForEvents + subtractionForEventsDay2 + subtractionForEventsDay3) + 
                    3.00M * 0.05M), 2).ToString();

                //PriceTotal3Label.Text = "$" + (decimal.Round((decimal.Parse(dvChart[2][standardPricing].ToString()) +
                //    (priceForMembers) - (subtractionForEvents) + 0.05M), 2) + decimal.Parse(PriceTotal2Label.Text.Replace("$", ""))).ToString();



            //7 Day, 1 Search Term
                NumberDays4Label.Text = "7";
                StandardRate4Label.Text = "$" + decimal.Round(decimal.Parse(dvChart[3][standardPricing].ToString()), 2).ToString();
                DataView dvCountEvents4;
                DataView dvCountEvents5;
                DataView dvCountEvents6;

                if (CountryDropDown.SelectedValue == "223")
                {
                    switch (Request.QueryString["P"])
                    {
                        case "E":
                            perPricing = "PerEventPricing";
                            perCap = "PerEventCap";
                            zips = zips.Replace("CatZip", "Zip");

                            dvCountEvents4 = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND (" + zips +
                                ") AND DaysFeatured LIKE '%;" + isNow.AddDays(1).Month.ToString() + "/" +
                            isNow.AddDays(1).Day.ToString() + "/" + isNow.AddDays(1).Year.ToString() + ";%'");

                            dvCountEvents5 = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND (" + zips +
                                ") AND DaysFeatured LIKE '%;" + isNow.AddDays(2).Month.ToString() + "/" +
                            isNow.AddDays(2).Day.ToString() + "/" + isNow.AddDays(2).Year.ToString() + ";%'");

                            dvCountEvents6 = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND (" + zips +
                                ") AND DaysFeatured LIKE '%;" + isNow.AddDays(3).Month.ToString() + "/" +
                            isNow.AddDays(3).Day.ToString() + "/" + isNow.AddDays(3).Year.ToString() + ";%'");

                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND (" + zips +
                                ") AND DaysFeatured LIKE '%;" + isNow.AddDays(4).Month.ToString() + "/" +
                            isNow.AddDays(4).Day.ToString() + "/" + isNow.AddDays(4).Year.ToString() + ";%'");

                            break;
                        case "V":
                            perCap = "PerVenueCap";
                            perPricing = "PerVenuePricing";
                            zips = zips.Replace("CatZip", "Zip");

                            dvCountEvents4 = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Venues WHERE Featured='True' AND (" + zips +
                                ") AND DaysFeatured LIKE '%;" + isNow.AddDays(1).Month.ToString() + "/" +
                            isNow.AddDays(1).Day.ToString() + "/" + isNow.AddDays(1).Year.ToString() + ";%'");

                            dvCountEvents5 = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Venues WHERE Featured='True' AND (" + zips +
                                ") AND DaysFeatured LIKE '%;" + isNow.AddDays(2).Month.ToString() + "/" +
                            isNow.AddDays(2).Day.ToString() + "/" + isNow.AddDays(2).Year.ToString() + ";%'");

                            dvCountEvents6 = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Venues WHERE Featured='True' AND (" + zips +
                                ") AND DaysFeatured LIKE '%;" + isNow.AddDays(3).Month.ToString() + "/" +
                            isNow.AddDays(3).Day.ToString() + "/" + isNow.AddDays(3).Year.ToString() + ";%'");

                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Venues WHERE Featured='True' AND (" + zips +
                                ") AND DaysFeatured LIKE '%;" + isNow.AddDays(4).Month.ToString() + "/" +
                            isNow.AddDays(4).Day.ToString() + "/" + isNow.AddDays(4).Year.ToString() + ";%'");

                            break;
                        case "A":
                            perCap = "PerBulletinCap";
                            perPricing = "PerBulletinPricing";

                            dvCountEvents4 = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Ads WHERE Featured='True' AND CatCity='" +
                                MajorCityDrop.SelectedItem.Text + "' AND DatesOfAd LIKE '%;" + isNow.AddDays(1).Month.ToString() + "/" +
                            isNow.AddDays(1).Day.ToString() + "/" + isNow.AddDays(1).Year.ToString() + ";%'");

                            dvCountEvents5 = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Ads WHERE Featured='True' AND CatCity='" +
                                MajorCityDrop.SelectedItem.Text + "' AND DatesOfAd LIKE '%;" + isNow.AddDays(2).Month.ToString() + "/" +
                            isNow.AddDays(2).Day.ToString() + "/" + isNow.AddDays(2).Year.ToString() + ";%'");

                            dvCountEvents6 = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Ads WHERE Featured='True' AND CatCity='" +
                                MajorCityDrop.SelectedItem.Text + "' AND DatesOfAd LIKE '%;" + isNow.AddDays(3).Month.ToString() + "/" +
                            isNow.AddDays(3).Day.ToString() + "/" + isNow.AddDays(3).Year.ToString() + ";%'");

                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Ads WHERE Featured='True' AND CatCity='" +
                                MajorCityDrop.SelectedItem.Text + "' AND DatesOfAd LIKE '%;" + isNow.AddDays(4).Month.ToString() + "/" +
                            isNow.AddDays(4).Day.ToString() + "/" + isNow.AddDays(4).Year.ToString() + ";%'");
                            break;
                        case "T":
                            perCap = "PerTripCap";
                            perPricing = "PerTripPricing";
                            zips = zips.Replace("CatZip", "TD.Zip");

                            dvCountEvents4 = dat.GetDataDV("SELECT COUNT(DISTINCT TripID) AS Count FROM Trips T, " +
                                "TripDirections TD WHERE T.ID=TD.TripID AND T.Featured='True'  AND (" + zips +
                                ") AND T.DaysFeatured LIKE '%;" + isNow.AddDays(1).Month.ToString() + "/" +
                               isNow.AddDays(1).Day.ToString() + "/" + isNow.AddDays(1).Year.ToString() + ";%'");

                            dvCountEvents5 = dat.GetDataDV("SELECT COUNT(DISTINCT TripID) AS Count FROM Trips T, " +
                                "TripDirections TD WHERE T.ID=TD.TripID AND T.Featured='True'  AND (" + zips +
                                ") AND T.DaysFeatured LIKE '%;" + isNow.AddDays(2).Month.ToString() + "/" +
                               isNow.AddDays(2).Day.ToString() + "/" + isNow.AddDays(2).Year.ToString() + ";%'");

                            dvCountEvents6 = dat.GetDataDV("SELECT COUNT(DISTINCT TripID) AS Count FROM Trips T, " +
                                "TripDirections TD WHERE T.ID=TD.TripID AND T.Featured='True'  AND (" + zips +
                                ") AND T.DaysFeatured LIKE '%;" + isNow.AddDays(3).Month.ToString() + "/" +
                               isNow.AddDays(3).Day.ToString() + "/" + isNow.AddDays(3).Year.ToString() + ";%'");

                            dvCountEvents = dat.GetDataDV("SELECT COUNT(DISTINCT TripID) AS Count FROM Trips T, " +
                                "TripDirections TD WHERE T.ID=TD.TripID AND T.Featured='True'  AND (" + zips +
                                ") AND T.DaysFeatured LIKE '%;" + isNow.AddDays(4).Month.ToString() + "/" +
                               isNow.AddDays(4).Day.ToString() + "/" + isNow.AddDays(4).Year.ToString() + ";%'");

                            break;
                        default:
                            dvCountEvents4 = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND (" + zips +
                                ") AND DaysFeatured LIKE '%;" + isNow.AddDays(1).Month.ToString() + "/" +
                            isNow.AddDays(1).Day.ToString() + "/" + isNow.AddDays(1).Year.ToString() + ";%'");

                            dvCountEvents5 = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND (" + zips +
                                ") AND DaysFeatured LIKE '%;" + isNow.AddDays(2).Month.ToString() + "/" +
                            isNow.AddDays(2).Day.ToString() + "/" + isNow.AddDays(2).Year.ToString() + ";%'");

                            dvCountEvents6 = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND (" + zips +
                                ") AND DaysFeatured LIKE '%;" + isNow.AddDays(3).Month.ToString() + "/" +
                            isNow.AddDays(3).Day.ToString() + "/" + isNow.AddDays(3).Year.ToString() + ";%'");

                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND (" + zips +
                                ") AND DaysFeatured LIKE '%;" + isNow.AddDays(4).Month.ToString() + "/" +
                            isNow.AddDays(4).Day.ToString() + "/" + isNow.AddDays(4).Year.ToString() + ";%'");
                            break;
                    }
                }
                else
                {
                    switch (Request.QueryString["P"])
                    {
                        case "E":
                            dvCountEvents4 = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND City = '" +
                            CityTextBox.Text + "' AND Country = " + CountryDropDown.SelectedValue +
                            " AND State = '" + state + "' AND DaysFeatured LIKE '%;" + isNow.AddDays(1).Month.ToString() + "/" +
                            isNow.AddDays(1).Day.ToString() + "/" + isNow.AddDays(1).Year.ToString() + ";%'");

                            dvCountEvents5 = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND City = '" +
                            CityTextBox.Text + "' AND Country = " + CountryDropDown.SelectedValue +
                            " AND State = '" + state + "' AND DaysFeatured LIKE '%;" + isNow.AddDays(2).Month.ToString() + "/" +
                            isNow.AddDays(2).Day.ToString() + "/" + isNow.AddDays(2).Year.ToString() + ";%'");

                            dvCountEvents6 = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND City = '" +
                            CityTextBox.Text + "' AND Country = " + CountryDropDown.SelectedValue +
                            " AND State = '" + state + "' AND DaysFeatured LIKE '%;" + isNow.AddDays(3).Month.ToString() + "/" +
                            isNow.AddDays(3).Day.ToString() + "/" + isNow.AddDays(3).Year.ToString() + ";%'");

                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND City = '" +
                            CityTextBox.Text + "' AND Country = " + CountryDropDown.SelectedValue +
                            " AND State = '" + state + "' AND DaysFeatured LIKE '%;" + isNow.AddDays(4).Month.ToString() + "/" +
                            isNow.AddDays(4).Day.ToString() + "/" + isNow.AddDays(4).Year.ToString() + ";%'");
                            break;
                        case "V":
                            dvCountEvents4 = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Venues WHERE Featured='True' AND City = '" +
                               CityTextBox.Text + "' AND Country = " + CountryDropDown.SelectedValue +
                               " AND State = '" + state + "' AND DaysFeatured LIKE '%;" + isNow.AddDays(1).Month.ToString() + "/" +
                               isNow.AddDays(1).Day.ToString() + "/" + isNow.AddDays(1).Year.ToString() + ";%'");

                            dvCountEvents5 = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Venues WHERE Featured='True' AND City = '" +
                               CityTextBox.Text + "' AND Country = " + CountryDropDown.SelectedValue +
                               " AND State = '" + state + "' AND DaysFeatured LIKE '%;" + isNow.AddDays(2).Month.ToString() + "/" +
                               isNow.AddDays(2).Day.ToString() + "/" + isNow.AddDays(2).Year.ToString() + ";%'");

                            dvCountEvents6 = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Venues WHERE Featured='True' AND City = '" +
                               CityTextBox.Text + "' AND Country = " + CountryDropDown.SelectedValue +
                               " AND State = '" + state + "' AND DaysFeatured LIKE '%;" + isNow.AddDays(3).Month.ToString() + "/" +
                               isNow.AddDays(3).Day.ToString() + "/" + isNow.AddDays(3).Year.ToString() + ";%'");

                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Venues WHERE Featured='True' AND City = '" +
                               CityTextBox.Text + "' AND Country = " + CountryDropDown.SelectedValue +
                               " AND State = '" + state + "' AND DaysFeatured LIKE '%;" + isNow.AddDays(4).Month.ToString() + "/" +
                               isNow.AddDays(4).Day.ToString() + "/" + isNow.AddDays(4).Year.ToString() + ";%'");
                            break;
                        case "A":
                            dvCountEvents4 = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Ads WHERE Featured='True' AND CatCity = '" +
                               CityTextBox.Text + "' AND CatCountry = " + CountryDropDown.SelectedValue +
                               " AND CatState = '" + state + "' AND DatesOfAd LIKE '%;" + isNow.AddDays(1).Month.ToString() + "/" +
                               isNow.AddDays(1).Day.ToString() + "/" + isNow.AddDays(1).Year.ToString() + ";%'");

                            dvCountEvents5 = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Ads WHERE Featured='True' AND CatCity = '" +
                               CityTextBox.Text + "' AND CatCountry = " + CountryDropDown.SelectedValue +
                               " AND CatState = '" + state + "' AND DatesOfAd LIKE '%;" + isNow.AddDays(2).Month.ToString() + "/" +
                               isNow.AddDays(2).Day.ToString() + "/" + isNow.AddDays(2).Year.ToString() + ";%'");

                            dvCountEvents6 = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Ads WHERE Featured='True' AND CatCity = '" +
                               CityTextBox.Text + "' AND CatCountry = " + CountryDropDown.SelectedValue +
                               " AND CatState = '" + state + "' AND DatesOfAd LIKE '%;" + isNow.AddDays(3).Month.ToString() + "/" +
                               isNow.AddDays(3).Day.ToString() + "/" + isNow.AddDays(3).Year.ToString() + ";%'");

                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Ads WHERE Featured='True' AND CatCity = '" +
                               CityTextBox.Text + "' AND CatCountry = " + CountryDropDown.SelectedValue +
                               " AND CatState = '" + state + "' AND DatesOfAd LIKE '%;" + isNow.AddDays(4).Month.ToString() + "/" +
                               isNow.AddDays(4).Day.ToString() + "/" + isNow.AddDays(4).Year.ToString() + ";%'");
                            break;
                        case "T":
                            perCap = "PerTripCap";
                            perPricing = "PerTripPricing";

                            dvCountEvents4 = dat.GetDataDV("SELECT COUNT(DISTINCT TripID) AS Count FROM Trips T, TripDirections TD WHERE T.ID=TD.TripID AND T.Featured='True' AND TD.City = '" +
                                CityTextBox.Text + "' AND TD.Country = " + CountryDropDown.SelectedValue +
                                " AND TD.State = '" + state + "' AND T.DaysFeatured LIKE '%;" + isNow.AddDays(1).Month.ToString() + "/" +
                               isNow.AddDays(1).Day.ToString() + "/" + isNow.AddDays(1).Year.ToString() + ";%'");

                            dvCountEvents5 = dat.GetDataDV("SELECT COUNT(DISTINCT TripID) AS Count FROM Trips T, TripDirections TD WHERE T.ID=TD.TripID AND T.Featured='True' AND TD.City = '" +
                                CityTextBox.Text + "' AND TD.Country = " + CountryDropDown.SelectedValue +
                                " AND TD.State = '" + state + "' AND T.DaysFeatured LIKE '%;" + isNow.AddDays(2).Month.ToString() + "/" +
                               isNow.AddDays(2).Day.ToString() + "/" + isNow.AddDays(2).Year.ToString() + ";%'");

                            dvCountEvents6 = dat.GetDataDV("SELECT COUNT(DISTINCT TripID) AS Count FROM Trips T, TripDirections TD WHERE T.ID=TD.TripID AND T.Featured='True' AND TD.City = '" +
                                CityTextBox.Text + "' AND TD.Country = " + CountryDropDown.SelectedValue +
                                " AND TD.State = '" + state + "' AND T.DaysFeatured LIKE '%;" + isNow.AddDays(3).Month.ToString() + "/" +
                               isNow.AddDays(3).Day.ToString() + "/" + isNow.AddDays(3).Year.ToString() + ";%'");

                            dvCountEvents = dat.GetDataDV("SELECT COUNT(DISTINCT TripID) AS Count FROM Trips T, TripDirections TD WHERE T.ID=TD.TripID AND T.Featured='True' AND TD.City = '" +
                                CityTextBox.Text + "' AND TD.Country = " + CountryDropDown.SelectedValue +
                                " AND TD.State = '" + state + "' AND T.DaysFeatured LIKE '%;" + isNow.AddDays(4).Month.ToString() + "/" +
                               isNow.AddDays(4).Day.ToString() + "/" + isNow.AddDays(4).Year.ToString() + ";%'");
                            break;
                        default:
                            dvCountEvents4 = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND City = '" +
                            CityTextBox.Text + "' AND Country = " + CountryDropDown.SelectedValue +
                            " AND State = '" + state + "' AND DaysFeatured LIKE '%;" + isNow.AddDays(1).Month.ToString() + "/" +
                            isNow.AddDays(1).Day.ToString() + "/" + isNow.AddDays(1).Year.ToString() + ";%'");

                            dvCountEvents5 = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND City = '" +
                            CityTextBox.Text + "' AND Country = " + CountryDropDown.SelectedValue +
                            " AND State = '" + state + "' AND DaysFeatured LIKE '%;" + isNow.AddDays(2).Month.ToString() + "/" +
                            isNow.AddDays(2).Day.ToString() + "/" + isNow.AddDays(2).Year.ToString() + ";%'");

                            dvCountEvents6 = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND City = '" +
                            CityTextBox.Text + "' AND Country = " + CountryDropDown.SelectedValue +
                            " AND State = '" + state + "' AND DaysFeatured LIKE '%;" + isNow.AddDays(3).Month.ToString() + "/" +
                            isNow.AddDays(3).Day.ToString() + "/" + isNow.AddDays(3).Year.ToString() + ";%'");

                            dvCountEvents = dat.GetDataDV("SELECT COUNT(*) AS Count FROM Events WHERE Featured='True' AND City = '" +
                            CityTextBox.Text + "' AND Country = " + CountryDropDown.SelectedValue +
                            " AND State = '" + state + "' AND DaysFeatured LIKE '%;" + isNow.AddDays(4).Month.ToString() + "/" +
                            isNow.AddDays(4).Day.ToString() + "/" + isNow.AddDays(4).Year.ToString() + ";%'");
                            break;
                    }
                }

                eventCountDay4 = decimal.Parse(dvCountEvents4[0]["Count"].ToString());
                eventCountDay5 = decimal.Parse(dvCountEvents5[0]["Count"].ToString());
                eventCountDay6 = decimal.Parse(dvCountEvents6[0]["Count"].ToString());
                eventCountDay7 = decimal.Parse(dvCountEvents[0]["Count"].ToString());

                SearchTerms4Label.Text = "$0.35";

                memberCap = decimal.Round(decimal.Parse(dvChart[3]["PerMemberCap"].ToString()), 2);
                eventCap = decimal.Round(decimal.Parse(dvChart[3][perCap].ToString()), 2);
                priceForMembers = 0.00M;
                subtractionForEvents = 0.00M;
            decimal subtractionForEvents4 = 0.00M;
            decimal subtractionForEvents5 = 0.00M;
            decimal subtractionForEvents6 = 0.00M;

                if (decimal.Parse(dvChart[3]["PerMemberPricing"].ToString()) * decimal.Parse(dvCountMembers[0]["Count"].ToString()) > memberCap)
                {
                    priceForMembers = memberCap;
                }
                else
                {
                    priceForMembers = decimal.Round(decimal.Parse(dvChart[3]["PerMemberPricing"].ToString()) *
                        decimal.Parse(dvCountMembers[0]["Count"].ToString()), 3);
                }

            //Day1
                if (decimal.Parse(dvChart[3][perPricing].ToString()) * eventCountDay1 > eventCap)
                {
                    subtractionForEventsDay1 = eventCap;
                }
                else
                {
                    subtractionForEventsDay1 = decimal.Round(decimal.Parse(dvChart[3][perPricing].ToString()) *
                        eventCountDay1, 3);
                }

            //Day 2
                if (decimal.Parse(dvChart[3][perPricing].ToString()) * eventCountDay2 > eventCap)
                {
                    subtractionForEventsDay2 = eventCap;
                }
                else
                {
                    subtractionForEventsDay2 = decimal.Round(decimal.Parse(dvChart[3][perPricing].ToString()) *
                        eventCountDay2, 3);
                }

            //Day 3
                if (decimal.Parse(dvChart[3][perPricing].ToString()) * eventCountDay3 > eventCap)
                {
                    subtractionForEventsDay3 = eventCap;
                }
                else
                {
                    subtractionForEventsDay3 = decimal.Round(decimal.Parse(dvChart[3][perPricing].ToString()) *
                        eventCountDay3, 3);
                }

            //Day 4
                if (decimal.Parse(dvChart[3][perPricing].ToString()) * eventCountDay4 > eventCap)
                {
                    subtractionForEventsDay4 = eventCap;
                }
                else
                {
                    subtractionForEventsDay4 = decimal.Round(decimal.Parse(dvChart[3][perPricing].ToString()) *
                        eventCountDay4, 3);
                }

            //Day 5
                if (decimal.Parse(dvChart[3][perPricing].ToString()) * eventCountDay5 > eventCap)
                {
                    subtractionForEventsDay5 = eventCap;
                }
                else
                {
                    subtractionForEventsDay5 = decimal.Round(decimal.Parse(dvChart[3][perPricing].ToString()) *
                        eventCountDay5, 3);
                }

            //Day 6
                if (decimal.Parse(dvChart[3][perPricing].ToString()) * eventCountDay6 > eventCap)
                {
                    subtractionForEventsDay6 = eventCap;
                }
                else
                {
                    subtractionForEventsDay6 = decimal.Round(decimal.Parse(dvChart[3][perPricing].ToString()) *
                        eventCountDay6, 3);
                }

           //Day 7 
                if (decimal.Parse(dvChart[3][perPricing].ToString()) * eventCountDay7 > eventCap)
                {
                    subtractionForEventsDay7 = eventCap;
                }
                else
                {
                    subtractionForEventsDay7 = decimal.Round(decimal.Parse(dvChart[3][perPricing].ToString()) *
                        eventCountDay7, 3);
                }

                MembersAdjustment4Label.Text = "$" + priceForMembers.ToString() + " (" + decimal.Round(decimal.Parse(dvChart[3]["PerMemberPricing"].ToString()), 3).ToString() + "/user)";
                EventsAdjustment4Label.Text = "-$" + decimal.Round(subtractionForEventsDay1 + subtractionForEventsDay2 +
                    subtractionForEventsDay3 +
                    subtractionForEventsDay4 + subtractionForEventsDay5 +
                    subtractionForEventsDay6 + subtractionForEventsDay7, 2, MidpointRounding.AwayFromZero).ToString();

                //Price4Label.Text = "$" + decimal.Round((decimal.Parse(dvChart[3][standardPricing].ToString()) +
                //    priceForMembers - subtractionForEvents + 0.05M), 2).ToString();

                PriceTotal4Label.Text = "$" + decimal.Round((7.00M * decimal.Parse(dvChart[3][standardPricing].ToString()) +
                    7.00M * (priceForMembers) - (subtractionForEventsDay1 + subtractionForEventsDay2 + subtractionForEventsDay3 +
                    subtractionForEventsDay4 + subtractionForEventsDay5 +
                    subtractionForEventsDay6 + subtractionForEventsDay7) + 7.00M * 0.05M), 2).ToString();


                PricesPanel.Visible = true;
        }
        else
        {
            PricesPanel.Visible = false;
            ErrorLabel.Text = "Please fill in the city, state and country.";
        }
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
            CityDropDownPanel.Visible = true;
            StateDropDown.AutoPostBack = true;
            CityTextBoxPanel.Visible = false;
            ChangeCity(StateDropDown, new EventArgs());
        }
        else
        {
            CityDropDownPanel.Visible = false;
            CityTextBoxPanel.Visible = true;
            StateDropDown.AutoPostBack = false;
        }

        FillChart(new object(), new EventArgs());
    }

    protected void ChangeCity(object sender, EventArgs e)
    {
        try
        {
            if (CountryDropDown.SelectedValue == "223")
            {
                HttpCookie cookie = Request.Cookies["BrowserDate"];
                Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
                DataSet ds = dat.GetData("SELECT *, MC.ID AS MCID FROM MajorCities MC, State S WHERE MC.State=S.state_name " +
                    "AND S.state_2_code='" + StateDropDown.SelectedItem.Text + "'");

                CityTextBoxPanel.Visible = false;
                CityDropDownPanel.Visible = true;
                MajorCityDrop.DataSource = ds;
                MajorCityDrop.DataTextField = "MajorCity";
                MajorCityDrop.DataValueField = "MCID";
                MajorCityDrop.DataBind();
            }
            else
            {
                CityDropDownPanel.Visible = false;
                CityTextBox.Visible = true;
            }

            FillChart(new object(), new EventArgs());
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString();
        }
    }
}
