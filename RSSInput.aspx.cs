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
using System.Net;

public partial class RSSInput: Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Data dat = new Data(DateTime.Now);

        DataSet ds = dat.GetData("SELECT ID, Name FROM Venues WHERE State = 'NY' ORDER BY Name");

        VenuesDropDown.DataSource = ds;
        VenuesDropDown.DataTextField = "Name";
        VenuesDropDown.DataValueField = "ID";
        VenuesDropDown.DataBind();
    }

    protected void GetRSSFeed(object sender, EventArgs e)
    {
        WebClient myClient = new WebClient();
        XmlDocument xmlDoc = new XmlDocument();
        string xmlString = myClient.DownloadString(RSSTextBox.Text).Replace("\r",
            "").Replace("\n", "");
        xmlDoc.LoadXml(xmlString);

        xmlDoc.Save(MapPath(".") + "/output.xml");

        RSSDisplayTextBox.Text = xmlDoc.OuterXml;
    }

    protected void Upload(object sender, EventArgs e)
    {
        Data dat = new Data(DateTime.Now);

        dat.Execute("INSERT INTO EventRSSFeeds (RSSLink, VenueID, ElementsPath, " +
            "LinkPath, TitlePath, DescriptionPath, DatePath, TimePath, DateTimeTogether, " +
            "FixDate, ImgInContent, Live) VALUES('" + RSSTextBox.Text + "', " + VenuesDropDown.SelectedValue + ", '" +
            ElementsPathTextBox.Text + "', '" + LinkPathTextBox.Text + "', '" +
            TitlePathTextBox.Text + "', '" + DescriptionPathTextBox.Text + "', '" +
            DatePathTextBox.Text + "', '" + TimePathTextBox.Text + "', '" +
            DateTimeTogetherDropDown.Text + "', '" + FixDateDropDown.Text + "', '" +
            ImgInContentDropDown.Text + "', 'True')");
    }

    protected void UploadRSSFeed(object sender, EventArgs e)
    {
        Data dat = new Data(DateTime.Now);

        DataView dvPages = dat.GetDataDV("SELECT * FROM EventRSSFeeds WHERE LIVE='True'");

        WebClient myClient = new WebClient();
        XmlDocument xmlDoc;
        HtmlAgilityPack.HtmlDocument htmlDocImages;

        string title = "";
        string content = "";
        string date = "";
        string time = "";
        string images = "";
        string categories = "";
        string link = "";

        try
        {
            foreach (DataRowView row in dvPages)
            {
                xmlDoc = new XmlDocument();
                string xmlString = myClient.DownloadString(row["RSSLink"].ToString()).Replace("\r",
                    "").Replace("\n", "");
                xmlDoc.LoadXml(xmlString);

                XmlNodeList events = xmlDoc.SelectNodes(row["ElementsPath"].ToString());

                foreach (XmlNode Event in events)
                {
                    images = "";

                    title = Event.SelectSingleNode(row["TitlePath"].ToString()).InnerText;
                    content = Event.SelectSingleNode(row["DescriptionPath"].ToString()).InnerText;
                    date = Event.SelectSingleNode(row["DatePath"].ToString()).InnerText;
                    time = Event.SelectSingleNode(row["TimePath"].ToString()).InnerText;
                    link = Event.SelectSingleNode(row["LinkPath"].ToString()).InnerText;

                    title = MakeNiceNameFull(title.Replace("\t\n", ", ").Replace("\t", ", ").Replace("\n", ", ")).Replace("-", " ");

                    if (!bool.Parse(row["DateTimeTogether"].ToString()))
                    {
                        date = date + " " + time;
                    }

                    if (bool.Parse(row["FixDate"].ToString()))
                    {
                        date = FixDate(date, true, row, false, "");
                    }

                    if (bool.Parse(row["ImgInContent"].ToString()))
                    {
                        htmlDocImages = new HtmlAgilityPack.HtmlDocument();
                        htmlDocImages.LoadHtml("<html><body>" + content.Replace("&lt", "<").Replace("&gt", ">") + "</body></html>");

                        HtmlAgilityPack.HtmlNodeCollection imageCollection = htmlDocImages.DocumentNode.SelectNodes("//img");

                        if (imageCollection != null)
                            foreach (HtmlAgilityPack.HtmlNode Image in imageCollection)
                            {
                                images += Image.Attributes["src"].Value + "^";
                            }
                    }

                    //Find out Event's Categories
                    categories = GetCategories(content);

                    if (categories == "")
                    {
                        categories = ";16;";
                    }

                    //Strip content of HTML, but, not before we get any potential images
                    content = stripHTML(content.Replace("&lt", "<").Replace("&gt", ">"));

                    CheckAndInsert(title, content, date, images, row["VenueID"].ToString(), categories, link);
                }
            }
        }
        catch (Exception ex)
        {
            //ErrorLabel.Text = ex.ToString() + "<br/><br/>" + title + ", " + content + ", " +
            //    date + ", " + time + ", " + images + ", " + categories + ", " + link;
            ErrorLabel.Text = date;
        }
    }

    protected void CheckAndInsert(string header, string description, string date, string images, string VenueID, string categories, string URL)
    {
        Data dat = new Data(DateTime.Now);

        //Check whether event exists in database
        DateTime dStart = DateTime.Parse(date);
        DataView dvEvent = dat.GetDataDV("SELECT * FROM Events E, Event_Occurance EO WHERE E.Venue=" + VenueID +
            " AND E.Header='" + header + "' AND E.ID=EO.EventID AND EO.DateTimeStart = CONVERT(DATETIME, '" + dStart.ToString() + "')");
        //Insert if does not exist
        if (dvEvent.Count == 0)
        {
            DataView dvVenue = dat.GetDataDV("SELECT country, city, state, zip, address FROM Venues WHERE ID=" + VenueID);
            string country = dvVenue[0]["country"].ToString();
            string city = dvVenue[0]["city"].ToString();
            string state = dvVenue[0]["state"].ToString();
            string zip = dvVenue[0]["zip"].ToString();
            string address = dvVenue[0]["address"].ToString();

            string mediaCat = "0";
            if (images != "")
                mediaCat = "1";

            if (description.Length > 500)
                description = description.Substring(0, 500) + "... " + URL;

            dvEvent = dat.GetDataDV("INSERT INTO Events (PostedOn, LastEditOn, UserName, Venue, Header, [Content], StarRating, hasSongs, mediaCategory, " +
                "city, country, state, zip, address) VALUES ('" + DateTime.Now.ToString() + "', '" + DateTime.Now.ToString() + "', 'aleksczajka', " +
                VenueID + ", '" + header +
                "', '" + description.Replace("'", "''").Replace("\\", "\\\\") + "', 0, 'False', " + mediaCat + ", '" + city + "', " +
                country + ", '" + state + "', '" + zip + "', '" + address + "')  SELECT @@IDENTITY AS 'Identity'");

            string theID = dvEvent[0]["Identity"].ToString();

            //Insert date and time
            dat.Execute("INSERT INTO Event_Occurance (EventID, DateTimeStart, DateTimeEnd) VALUES (" + theID + ", '" +
                dStart.ToString() + "', '" + dStart.AddHours(2.00).ToString() + "')");

            //Insert categories
            string[] delim = { ";" };
            string[] tokens = categories.Split(delim, StringSplitOptions.RemoveEmptyEntries);
            foreach (string token in tokens)
            {
                dat.Execute("INSERT INTO Event_Category_Mapping (CategoryID, EventID, tagSize) VALUES (" + token + ", '" +
                    theID + "', 22)");
            }

            //Insert Images
            string[] imgDelim = { "^" };
            if (images != "")
            {
                string[] imgTokens = images.Split(imgDelim, StringSplitOptions.RemoveEmptyEntries);
                foreach (string token in imgTokens)
                {
                    dat.Execute("INSERT INTO Event_Slider_Mapping (EventID, PictureName, RealPictureName, " +
                        "ImgPathAbsolute) VALUES(" + theID + ", '" + token + "', '" + token + "', 'True')");
                }
            }
        }
    }

    protected string GetCategories(string description)
    {
        Data dat = new Data(DateTime.Now);
        //parse description for categories
        DataView dvCats = dat.GetDataDV("SELECT * FROM EventCategories");
        string categories = "";
        char[] delim = { '/' };
        char[] delimSpace = { ' ' };
        string[] tokens;
        description = stripHTML(description);
        foreach (DataRowView row in dvCats)
        {
            if (description.Contains(row["Name"].ToString()))
                categories += ";" + row["ID"].ToString() + ";";
            else if (row["Name"].ToString()[row["Name"].ToString().Length - 1].ToString().ToLower() == "s")
            {
                if (description.Contains(row["Name"].ToString().Substring(0, row["Name"].ToString().Length - 1)))
                    categories += ";" + row["ID"].ToString() + ";";
            }
            else if (row["Name"].ToString().ToLower().Contains("/"))
            {
                tokens = row["Name"].ToString().ToLower().Split(delim);

                if (description.Contains(tokens[0]))
                    categories += ";" + row["ID"].ToString() + ";";
                else if (description.Contains(tokens[1]))
                    categories += ";" + row["ID"].ToString() + ";";
            }
            else if (row["Name"].ToString().Trim().Contains(" "))
            {
                tokens = row["Name"].ToString().Trim().ToLower().Split(delimSpace);

                if (description.Contains(tokens[0]) && tokens[0] != "event")
                    categories += ";" + row["ID"].ToString() + ";";
                else if (description.Contains(tokens[1]) && tokens[1] != "event")
                    categories += ";" + row["ID"].ToString() + ";";
            }
        }

        return categories;
    }

    protected string MakeNiceNameFull(string str)
    {
        str = stripHTML(str).Replace((char)10, '-').Replace((char)13, '-').Replace("\r", "-").Replace("\t", "-").Replace("\n", "-").Replace("+", "-").Replace("'", "-").Replace("&", "-").Replace(";",
                "-").Replace(":", "-").Replace(",", "-").Replace("\"", "-").Replace("\\",
                "-").Replace("%", "-").Replace("(", "-").Replace(")", "-").Replace(" ",
                "-").Replace("-", "-").Replace("/", "-").Replace("!", "-").Replace("@",
                "-").Replace("#", "-").Replace("$", "-").Replace("^", "-").Replace("*",
                "-").Replace("=", "-").Replace("[", "-").Replace("]", "-").Replace("{",
                "-").Replace("}", "-").Replace("|", "-").Replace("?", "-").Replace("<",
                "-").Replace(">", "-").Replace(".", "-").Replace("~", "-").Replace("`",
                "-").Replace("“", "").Replace("”", "").Replace(".", "").Replace("…", "").Replace("!",
                "-").Replace("-----", "-").Replace("----", "-").Replace("---",
                "-").Replace("--", "-");
        if (str.Length > 0)
            if (str.Substring(str.Length - 1, 1) == "-")
                str = str.Substring(0, str.Length - 1);
        return str;
    }

    protected string ReplaceMonths(string date)
    {
        date = date.ToLower();

        date = date.Replace("january", "~").Replace(" jan ", " ~ ").Replace("february", "~~").Replace(" feb ",
            " ~~ ").Replace("march", "~~~").Replace(" mar ", " ~~~ ").Replace("april", "~~~~").Replace(" apr ",
            " ~~~~ ").Replace("may", "~~~~~").Replace("june", "~~~~~~").Replace(" jun ", " ~~~~~~ ").Replace("july",
            "~~~~~~~").Replace(" jul ", " ~~~~~~~ ").Replace("august", "~~~~~~~~").Replace(" aug ",
            " ~~~~~~~~ ").Replace("september", "~~~~~~~~~").Replace(" sept ", " ~~~~~~~~~ ").Replace("october",
            "~~~~~~~~~~").Replace(" oct ", " ~~~~~~~~~~ ").Replace("november", "~~~~~~~~~~~").Replace(" nov ",
            " ~~~~~~~~~~~ ").Replace("december", "~~~~~~~~~~~~").Replace(" dec ", " ~~~~~~~~~~~~ ");

        return date;
    }

    protected string PutBackMonths(string date)
    {
        date = date.ToLower();

        date = date.Replace("~~~~~~~~~~~~", " december ").Replace("~~~~~~~~~~~", " november ").Replace("~~~~~~~~~~",
            " october ").Replace("~~~~~~~~~", " september ").Replace("~~~~~~~~", " august ").Replace("~~~~~~~",
            " july ").Replace("~~~~~~", " june ").Replace("~~~~~", " may ").Replace("~~~~", " april ").Replace("~~~",
            " march ").Replace("~~", " febuary ").Replace("~", " january ");

        return date;
    }

    protected string FixDate(string date, bool parse, DataRowView row, bool gotTime, string gottenTime)
    {
        char[] delimer = { ' ' };
        string[] prelimToks = date.Split(delimer);
        int outputInt = 0;

        date = "";

        foreach (string token in prelimToks)
        {
            if (token.ToLower().Contains("pm"))
            {
                if (int.TryParse(token.ToLower().Replace("pm", ""), out outputInt))
                {
                    date += outputInt.ToString() + " || ";
                }
                else
                {
                    date += token + " ";
                }
            }
            else if (token.ToLower().Contains("p"))
            {
                if (int.TryParse(token.ToLower().Replace("p", ""), out outputInt))
                {
                    date += outputInt.ToString() + " ||| ";
                }
                else
                {
                    date += token + " ";
                }
            }
            else if (token.ToLower().Contains("am"))
            {
                if (int.TryParse(token.ToLower().Replace("am", ""), out outputInt))
                {
                    date += outputInt.ToString() + " |||| ";
                }
                else
                {
                    date += token + " ";
                }
            }
            else if (token.ToLower().Contains("a"))
            {
                if (int.TryParse(token.ToLower().Replace("a", ""), out outputInt))
                {
                    date += outputInt.ToString() + " |||||| ";
                }
                else
                {
                    date += token + " ";
                }
            }
            else
            {
                date += token + " ";
            }
        }

        date = ReplaceMonths(date);
        date = date.ToLower().Replace("monday", "").Replace("tuesday", "").Replace("wednesday", "").Replace("thursday",
            "").Replace("friday", "").Replace("saturday", "").Replace("sunday", "").Replace("sat",
            "").Replace("mon", "").Replace("tues", "").Replace("tue",
            "").Replace("wed", "").Replace("thr", "").Replace("thurs", "").Replace("fri", "").Replace("sun",
            "").Replace("A", "").Replace("a",
            "").Replace("B", "").Replace("b", "").Replace("C",
            "").Replace("c", "").Replace("D", "").Replace("d", "").Replace("E", "").Replace("e",
            "").Replace("F", "").Replace("f", "").Replace("G", "").Replace("g", "").Replace("H",
            "").Replace("h", "").Replace("I", "").Replace("i", "").Replace("J", "").Replace("j",
            "").Replace("K", "").Replace("k", "").Replace("L", "").Replace("l", "").Replace("M",
            "").Replace("m", "").Replace("N", "").Replace("n", "").Replace("O", "").Replace("o",
            "").Replace("P", "").Replace("p", "").Replace("Q", "").Replace("q", "").Replace("R",
            "").Replace("r", "").Replace("S", "").Replace("s", "").Replace("T", "").Replace("t",
            "").Replace("U", "").Replace("u", "").Replace("V", "").Replace("v", "").Replace("W",
            "").Replace("w", "").Replace("X", "").Replace("x", "").Replace("Y", "").Replace("y",
            "").Replace("Z", "").Replace("z", "").Replace("'", "").Replace(",", "").Trim();

        date = PutBackMonths(date);
        date = date.Replace("|||||", " a").Replace("||||", " am").Replace("|||", " p").Replace("||", " pm");

        if (parse)
        {

            string yearNow = DateTime.Now.Year.ToString();
            string yearLast = (DateTime.Now.Year - 1).ToString();
            string yearNext = (DateTime.Now.Year + 1).ToString();

            string[] delim = { " " };
            string[] tokens = date.Split(delim, StringSplitOptions.RemoveEmptyEntries);

            char[] delim2 = { '/' };
            string[] tokens2;

            char[] delim3 = { '.' };
            string[] tokens3;

            char[] delim4 = { ':' };
            string[] tokens4;

            string year = "";
            string time = "";
            string month = "";
            string day = "";

            int tryParseTime = 0;

            bool intTimeGotten = false;
            bool letterTimeGotten = false;
            bool isMonthGotten = false;
            bool isDayGotten = false;

            month = TextMonth(date);

            if (month != "")
                isMonthGotten = true;

            if (gotTime)
            {
                intTimeGotten = true;
                letterTimeGotten = true;
                time = gottenTime;
            }

            foreach (string token in tokens)
            {
                if (token.Contains("/") && !isMonthGotten)
                {
                    tokens2 = token.Split(delim2);
                    month = tokens2[0];
                    day = tokens2[1];
                    isMonthGotten = true;

                    if (tokens2.Length > 2)
                        year = tokens2[2];
                }
                else if (token.Contains(".") && !isMonthGotten)
                {
                    tokens3 = token.Split(delim3);
                    month = tokens3[0];
                    day = tokens3[1];
                    isMonthGotten = true;
                }
                else if (!isMonthGotten && int.TryParse(Strip(token), out tryParseTime))
                {
                    month = token;
                    isMonthGotten = true;
                }
                else if (!isDayGotten && int.TryParse(Strip(token), out tryParseTime))
                {
                    day = token;
                    isDayGotten = true;
                }
                else if (token == yearNow)
                {
                    year = yearNow;
                }
                else if (token == yearLast)
                {
                    year = yearLast;
                }
                else if (token == yearNext)
                {
                    year = yearNext;
                }
                else if (token.Contains("'"))
                {
                    if (yearLast.Contains(token.Replace("'", "")))
                    {
                        year = yearLast;
                    }
                    else if (yearNow.Contains(token.Replace("'", "")))
                    {
                        year = yearNow;
                    }
                    else if (yearNext.Contains(token.Replace("'", "")))
                    {
                        year = yearNext;
                    }
                }
                else if (!intTimeGotten && int.TryParse(Strip(token), out tryParseTime))
                {
                    time += token;
                    intTimeGotten = true;
                }
                else if (!intTimeGotten && token.Contains(":"))
                {
                    tokens4 = token.Split(delim4);
                    if (int.TryParse(tokens4[0], out tryParseTime) && int.TryParse(tokens4[1], out tryParseTime))
                    {
                        time = tokens4[0] + ":" + tokens4[1];
                        intTimeGotten = true;
                    }
                }
                else if (!letterTimeGotten)
                {
                    if (intTimeGotten)
                    {
                        if (token.ToLower() == "p")
                        {
                            time += " pm";
                            letterTimeGotten = true;
                        }
                        else if (token.ToLower() == "pm")
                        {
                            time += " pm";
                            letterTimeGotten = true;
                        }
                        else if (token.ToLower() == "p")
                        {
                            time += " am";
                            letterTimeGotten = true;
                        }
                        else if (token.ToLower() == "am")
                        {
                            time += " am";
                            letterTimeGotten = true;
                        }
                    }
                }
            }

            if (year == "")
                year = yearNow;

            if (!intTimeGotten)
                time = "12" + time;

            //add default time frame
            if (!time.Contains("pm") && !time.Contains("am") && !time.Contains("p") && !time.Contains("a"))
            {
                time += " pm";
            }

            date = month + "/" + day + " " + year + " " + time;
        }

        return date;
    }

    protected string stripHTML(string body)
    {
        string toRet = System.Text.RegularExpressions.Regex.Replace(body, @"<(.|\n)*?>", string.Empty);
        toRet = toRet.Replace("&amp;", " ").Replace("nbsp;", " ");
        return toRet;
    }

    protected string TextMonth(string date)
    {

        if (date.ToLower().Contains("january"))
        {
            return "1";

        }
        else if (date.ToLower().Contains("jan"))
        {
            return "1";

        }
        else if (date.ToLower().Contains("february"))
        {
            return "2";

        }
        else if (date.ToLower().Contains("feb"))
        {
            return "2";

        }
        else if (date.ToLower().Contains("march"))
        {
            return "3";

        }
        else if (date.ToLower().Contains("mar"))
        {
            return "3";

        }
        else if (date.ToLower().Contains("april"))
        {
            return "4";

        }
        else if (date.ToLower().Contains("apr"))
        {
            return "4";

        }
        else if (date.ToLower().Contains("may"))
        {
            return "5";

        }
        else if (date.ToLower().Contains("june"))
        {
            return "6";

        }
        else if (date.ToLower().Contains("jun"))
        {
            return "6";

        }
        else if (date.ToLower().Contains("july"))
        {
            return "7";

        }
        else if (date.ToLower().Contains("jul"))
        {
            return "7";

        }
        else if (date.ToLower().Contains("august"))
        {
            return "8";

        }
        else if (date.ToLower().Contains("aug"))
        {
            return "8";

        }
        else if (date.ToLower().Contains("september"))
        {
            return "9";

        }
        else if (date.ToLower().Contains("sept"))
        {
            return "9";

        }
        else if (date.ToLower().Contains("sep"))
        {
            return "9";

        }
        else if (date.ToLower().Contains("october"))
        {
            return "10";

        }
        else if (date.ToLower().Contains("oct"))
        {
            return "10";

        }
        else if (date.ToLower().Contains("november"))
        {
            return "11";

        }
        else if (date.ToLower().Contains("nov"))
        {
            return "11";

        }
        else if (date.ToLower().Contains("december"))
        {
            return "12";

        }
        else if (date.ToLower().Contains("dec"))
        {
            return "12";
        }
        else
            return "";

    }

    protected string Strip(string str)
    {
        return str.Replace(",", "");
    }
}
