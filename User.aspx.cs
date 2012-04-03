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
using System.Drawing.Imaging;
using System.IO;


public partial class User : Telerik.Web.UI.RadAjaxPage
{
    public string UserName;
    
    public int UserID;

    System.Drawing.Color greyText = System.Drawing.Color.FromArgb(102, 102, 102);
    System.Drawing.Color greyBack = System.Drawing.Color.FromArgb(51, 51, 51);
    System.Drawing.Color greyDark = System.Drawing.Color.FromArgb(27, 27, 27);
    System.Drawing.Color greyBorder = System.Drawing.Color.FromArgb(54, 54, 54);

    //protected void SendWelcome(object sender, EventArgs e)
    //{
    //            HttpCookie cookie = Request.Cookies["BrowserDate"];
    //    if (cookie == null)
    //    {
    //        cookie = new HttpCookie("BrowserDate");
    //        cookie.Value = DateTime.Now.ToString();
    //        cookie.Expires = DateTime.Now.AddDays(22);
    //        Response.Cookies.Add(cookie);
    //    }

    //        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //        dat.SendWelcome();
    //}

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

        TopSaveButton.SERVER_CLICK += Save;
        UploadButton.SERVER_CLICK += UploadPhoto;
        BottomSaveButton.SERVER_CLICK += Save;

        #region Take care of bulletins right arrow
        HtmlHead head = (HtmlHead)Page.Header;

        Literal lit = new Literal();
        lit.Text = "<style type=\"text/css\"> " +
            ".radr_Black .radr_button.radr_buttonRight " +
            "{" +
            "    position: relative;" +
            "    right: -405px !important;" +
            "    top: 40% !important;" +
            "} " +
            ".radr_Black .radr_button.radr_buttonLeft { "+
            "background-position: 0 -60px; "+
            "left: -19px !important; "+
            "margin-top: -10px; "+
            "top: 50%; "+
            "}</style>";
        head.Controls.Add(lit);
        #endregion

        DoAll();

        //Request.Browser.Browser = "Netscape";
        
        if (!IsPostBack)
        {
            Session["MessagesToDelete"] = "";

            //Categories have to be set from Page_Load not Page_Init for some reason. Otherwise it doesn't work.
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            //CategoryTree.DataBind();
            ////RadTreeView1.DataBind();
            //RadTreeView2.DataBind();
            //RadTreeView3.DataBind();

            DataSet dsCategories = dat.GetData("SELECT * FROM UserCategories UC, AdCategories AC WHERE UC.UserID=" +
                Session["User"].ToString() + " AND UC.CategoryID=AC.ID");
            //FillCategories(dsCategories, ref CategoryTree);
            //FillCategories(dsCategories, ref RadTreeView2);


            //Set Event categories
            RadTreeView1.DataBind();
            RadTreeView3.DataBind();

            dsCategories = dat.GetData("SELECT * FROM UserEventCategories UEC, EventCategories EC WHERE UEC.UserID=" +
                Session["User"].ToString() + " AND UEC.CategoryID=EC.ID");
            FillCategories(dsCategories, ref RadTreeView1);
            FillCategories(dsCategories, ref RadTreeView3);

            //Image1.Attributes.Add("onclick", "goEventNext()");
            //Master.BodyTag.Attributes.Add("onload", "startUpScripts()");

            //Master.theScriptManager.Scripts.Add(new ScriptReference("Telerik.Web.UI.Common.Core.js", "Telerik.Web.UI"));
        }
    }

    protected string GetUserImage(string UserID)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        string friendImg = "";
        string strFill = "";

        DataView dvUser = dat.GetDataDV("SELECT * FROM Users WHERE User_ID=" + UserID);

        if (System.IO.File.Exists(Server.MapPath(".") + "\\UserFiles\\" + dvUser[0]["UserName"].ToString() +
            "\\Profile\\" + dvUser[0]["ProfilePicture"].ToString()))
        {
            friendImg = "UserFiles/" + dvUser[0]["UserName"].ToString() + "/Profile/" + 
                dvUser[0]["ProfilePicture"].ToString();
            strFill = "";
        }
        else
        {
            friendImg = "NewImages/noAvatar_50x50_small.png";
            strFill = "onmouseover=\"this.src='NewImages/noAvatar_50x50_smallhover.png'\"" +
                "onmouseout=\"this.src='NewImages/noAvatar_50x50_small.png'\" ";
        }
        return "<a href=\"" + dvUser[0]["UserName"].ToString() + "_Friend\"><img " + 
            strFill + " style=\" border: 0;float: left;padding-right: 7px; padding-bottom: 2px;\" " +
                "src=\"" + friendImg + "\" width=\"50px\" height=\"50px\" /></a>";
    }

    protected string GetIDToUse(bool use1, bool use2, bool use3, bool use4, bool use5, DateTime dt1,
        DateTime dt2, DateTime dt3, DateTime dt4, DateTime dt5)
    {
        string idToUse = "";
        if (use1)
        {
            if (use2)
            {
                if (use3)
                {
                    if (use4)
                    {
                        if (use5)
                        {
                            if (dt1 > dt2)
                            {
                                if (dt1 > dt3)
                                {
                                    if (dt1 > dt4)
                                    {
                                        if (dt1 > dt5)
                                            idToUse = "1";
                                        else
                                            idToUse = "5";
                                    }
                                    else
                                    {
                                        if (dt4 > dt5)
                                            idToUse = "4";
                                        else
                                            idToUse = "5";
                                    }
                                }
                                else
                                {
                                    if (dt3 > dt4)
                                    {
                                        if (dt3 > dt5)
                                            idToUse = "3";
                                        else
                                            idToUse = "5";
                                    }
                                    else
                                    {
                                        if (dt4 > dt5)
                                            idToUse = "4";
                                        else
                                            idToUse = "5";
                                    }
                                }
                            }
                            else
                            {
                                if (dt2 > dt3)
                                {
                                    if (dt2 > dt4)
                                    {
                                        if (dt2 > dt5)
                                            idToUse = "2";
                                        else
                                            idToUse = "5";
                                    }
                                    else
                                    {
                                        if (dt4 > dt5)
                                            idToUse = "4";
                                        else
                                            idToUse = "5";
                                    }
                                }
                                else
                                {
                                    if (dt3 > dt4)
                                    {
                                        if (dt3 > dt5)
                                            idToUse = "3";
                                        else
                                            idToUse = "5";
                                    }
                                    else
                                    {
                                        if (dt4 > dt5)
                                            idToUse = "4";
                                        else
                                            idToUse = "5";
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (dt1 > dt2)
                            {
                                if (dt1 > dt3)
                                {
                                    if (dt1 > dt4)
                                    {
                                        idToUse = "1";
                                    }
                                    else
                                    {
                                        idToUse = "4";
                                    }
                                }
                                else
                                {
                                    if (dt3 > dt4)
                                    {
                                        idToUse = "3";
                                    }
                                    else
                                    {
                                        idToUse = "4";
                                    }
                                }
                            }
                            else
                            {
                                if (dt2 > dt3)
                                {
                                    if (dt2 > dt4)
                                    {
                                        idToUse = "2";
                                    }
                                    else
                                    {
                                        idToUse = "4";
                                    }
                                }
                                else
                                {
                                    if (dt3 > dt4)
                                    {
                                        idToUse = "3";
                                    }
                                    else
                                    {
                                        idToUse = "4";
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (use5)
                        {
                            if (dt1 > dt2)
                            {
                                if (dt1 > dt3)
                                {
                                    if (dt1 > dt5)
                                        idToUse = "1";
                                    else
                                        idToUse = "5";
                                }
                                else
                                {
                                    if (dt3 > dt5)
                                        idToUse = "3";
                                    else
                                        idToUse = "5";
                                }
                            }
                            else
                            {
                                if (dt2 > dt3)
                                {
                                    if (dt2 > dt5)
                                        idToUse = "2";
                                    else
                                        idToUse = "5";
                                }
                                else
                                {
                                    if (dt3 > dt5)
                                        idToUse = "3";
                                    else
                                        idToUse = "5";
                                }
                            }
                        }
                        else
                        {
                            if (dt1 > dt2)
                            {
                                if (dt1 > dt3)
                                {
                                    idToUse = "1";
                                }
                                else
                                {
                                    idToUse = "3";
                                }
                            }
                            else
                            {
                                if (dt2 > dt3)
                                {
                                    idToUse = "2";
                                }
                                else
                                {
                                    idToUse = "3";
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (dt1 > dt2)
                    {
                        if (use4)
                        {
                            if (use5)
                            {
                                if (dt1 > dt4)
                                {
                                    if (dt1 > dt5)
                                        idToUse = "1";
                                    else
                                        idToUse = "5";
                                }
                                else
                                {
                                    if (dt4 > dt5)
                                        idToUse = "4";
                                    else
                                        idToUse = "5";
                                }
                            }
                            else
                            {
                                if (dt1 > dt4)
                                {
                                    idToUse = "1";
                                }
                                else
                                {
                                    idToUse = "4";
                                }
                            }
                        }
                        else
                        {
                            if (use5)
                            {
                                if (dt1 > dt5)
                                    idToUse = "1";
                                else
                                    idToUse = "5";
                            }
                            else
                            {
                                idToUse = "1";
                            }
                        }
                    }
                    else
                    {
                        if (use4)
                        {
                            if (use5)
                            {
                                if (dt2 > dt4)
                                {
                                    if (dt2 > dt5)
                                        idToUse = "2";
                                    else
                                        idToUse = "5";
                                }
                                else
                                {
                                    if (dt4 > dt5)
                                        idToUse = "4";
                                    else
                                        idToUse = "5";
                                }
                            }
                            else
                            {
                                if (dt2 > dt4)
                                {
                                    idToUse = "4";
                                }
                                else
                                {
                                    idToUse = "2";
                                }
                            }
                        }
                        else
                        {
                            if (use5)
                            {
                                if (dt2 > dt5)
                                    idToUse = "2";
                                else
                                    idToUse = "5";
                            }
                            else
                                idToUse = "2";
                        }
                    }
                }
            }
            else
            {
                if (use4)
                {
                    if (use5)
                    {
                        if (dt1 > dt4)
                        {
                            if (dt1 > dt5)
                                idToUse = "1";
                            else
                                idToUse = "5";
                        }
                        else
                        {
                            if (dt4 > dt5)
                                idToUse = "4";
                            else
                                idToUse = "5";
                        }
                    }
                    else
                    {
                        if (dt1 > dt4)
                        {
                            idToUse = "1";
                        }
                        else
                        {
                            idToUse = "4";
                        }
                    }
                }
                else
                {
                    if (use5)
                    {
                        if (dt1 > dt5)
                            idToUse = "1";
                        else
                            idToUse = "5";
                    }
                    else
                        idToUse = "1";
                }
            }
        }
        else
        {
            if (use2)
            {
                if (use3)
                {
                    if (use4)
                    {
                        if (use5)
                        {
                            if (dt2 > dt3)
                            {
                                if (dt2 > dt4)
                                {
                                    if (dt2 > dt5)
                                        idToUse = "2";
                                    else
                                        idToUse = "5";
                                }
                                else
                                {
                                    if (dt4 > dt5)
                                        idToUse = "4";
                                    else
                                        idToUse = "5";
                                }
                            }
                            else
                            {
                                if (dt3 > dt4)
                                {
                                    if (dt3 > dt5)
                                        idToUse = "3";
                                    else
                                        idToUse = "5";
                                }
                                else
                                {
                                    if (dt4 > dt5)
                                        idToUse = "4";
                                    else
                                        idToUse = "5";
                                }
                            }
                        }
                        else
                        {
                            if (dt2 > dt3)
                            {
                                if (dt2 > dt4)
                                {
                                    idToUse = "2";
                                }
                                else
                                {
                                    idToUse = "4";
                                }
                            }
                            else
                            {
                                if (dt3 > dt4)
                                {
                                    idToUse = "3";
                                }
                                else
                                {
                                    idToUse = "4";
                                }
                            }
                        }
                    }
                    else
                    {
                        if (use5)
                        {
                            if (dt2 > dt3)
                            {
                                if (dt2 > dt5)
                                    idToUse = "2";
                                else
                                    idToUse = "5";
                            }
                            else
                            {
                                if (dt3 > dt5)
                                    idToUse = "3";
                                else
                                    idToUse = "5";
                            }
                        }
                        else
                        {
                            if (dt2 > dt3)
                            {
                                idToUse = "2";
                            }
                            else
                                idToUse = "3";
                        }
                    }
                }
                else
                {
                    if (use4)
                    {
                        if (use5)
                        {
                            if (dt2 > dt4)
                            {
                                if (dt2 > dt5)
                                    idToUse = "2";
                                else
                                    idToUse = "5";
                            }
                            else
                            {
                                if (dt4 > dt5)
                                    idToUse = "4";
                                else
                                    idToUse = "5";
                            }
                        }
                        else
                        {
                            if (dt2 > dt4)
                                idToUse = "2";
                            else
                                idToUse = "4";
                        }
                    }
                    else
                    {
                        if (use5)
                        {
                            if (dt2 > dt5)
                                idToUse = "2";
                            else
                                idToUse = "5";
                        }
                        else
                            idToUse = "2";
                    }
                }
            }
            else
            {
                if (use3)
                {
                    if (use4)
                    {
                        if (use5)
                        {
                            if (dt3 > dt4)
                            {
                                if (dt3 > dt5)
                                    idToUse = "3";
                                else
                                    idToUse = "5";
                            }
                            else
                            {
                                if (dt4 > dt5)
                                    idToUse = "4";
                                else
                                    idToUse = "5";
                            }
                        }
                        else
                        {
                            if (dt3 > dt4)
                                idToUse = "3";
                            else
                                idToUse = "4";
                        }
                    }
                    else
                    {
                        if (use5)
                        {
                            if (dt3 > dt5)
                                idToUse = "3";
                            else
                                idToUse = "5";
                        }
                        else
                            idToUse = "3";
                    }
                }
                else
                {
                    if (use4)
                    {
                        if (use5)
                        {
                            if (dt4 > dt5)
                                idToUse = "4";
                            else
                                idToUse = "5";
                        }
                        else
                            idToUse = "4";
                    }
                    else
                    {
                        idToUse = "5";
                    }
                }
            }
        }

        return idToUse;
    }

    protected void GoToSearches(object sender, EventArgs e)
    {
        Response.Redirect("SearchesAndPages.aspx");
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        string USER_ID = "";
        try
        {
            //Ajax.Utility.RegisterTypeForAjax(typeof(User));
            

            //if (!IsPostBack)
            //{
            //    FriendsButton.Attributes.Add("onmouseover", "this.src='image/MyFriendsHover.png'");
            //    FriendsButton.Attributes.Add("onmouseout", "this.src='image/MyFriends.png'");


            //}
            //else
            //{
            //    FriendsButton.Attributes.Remove("onmouseover");
            //    FriendsButton.Attributes.Remove("onmouseout");
            //    MessagesButton.Attributes.Remove("onmouseover");
            //    MessagesButton.Attributes.Remove("onmouseout");
            //}


            //FOR USER PREFERENCES
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            MessageRadWindowManager.VisibleOnPageLoad = false;

            
            if (!IsPostBack)
            {
                try
                {
                    

                    if (Session["User"] != null)
                    {
                    }
                    else
                    {
                        Button calendarLink = (Button)dat.FindControlRecursive(this, "CalendarLink");
                        calendarLink.Visible = false;
                        Response.Redirect("~/login");
                    }
                }
                catch (Exception ex)
                {
                    ErrorLabel.Text = ex.ToString();
                   Response.Redirect("~/login");
                }

                USER_ID = Session["User"].ToString();

                //DataSet dsCat = dat.GetData("SELECT * FROM Categories");
                //CategoriesCheckBoxes.DataSource = dsCat;
                //CategoriesCheckBoxes.DataTextField = "CategoryName";
                //CategoriesCheckBoxes.DataValueField = "ID";
                //CategoriesCheckBoxes.DataBind();

                



                DataSet dsProvider = dat.GetData("SELECT * FROM PhoneProviders");
                ProviderDropDown.DataSource = dsProvider;
                ProviderDropDown.DataTextField = "Provider";
                ProviderDropDown.DataValueField = "ID";
                ProviderDropDown.DataBind();


                Data d = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
                DataSet ds = d.GetData("SELECT * FROM Events E, Venues V, Event_Occurance EO, User_Calendar UC WHERE UC.EventID=E.ID AND E.ID=EO.EventID AND E.Venue=V.ID AND UC.UserID=" + USER_ID);
                DataSet dsUser = d.GetData("SELECT * FROM Users WHERE User_ID=" + USER_ID);
                DataSet dsUserPrefs = d.GetData("SELECT * FROM UserPreferences WHERE UserID=" + USER_ID);

                WeeklyCheckBox.Checked = bool.Parse(dsUser.Tables[0].Rows[0]["Weekly"].ToString());

                if (dsUser.Tables[0].Rows[0]["FirstName"] != null)
                    FirstNameTextBox.Text = dsUser.Tables[0].Rows[0]["FirstName"].ToString();

                if (dsUser.Tables[0].Rows[0]["LastName"] != null)
                    LastNameTextBox.Text = dsUser.Tables[0].Rows[0]["LastName"].ToString();

                if (dsUserPrefs.Tables.Count > 0)
                    if (dsUserPrefs.Tables[0].Rows.Count > 0)
                    {
                        //AgeTextBox.Text = dsUserPrefs.Tables[0].Rows[0]["Age"].ToString();
                        SexTextBox.Text = dsUserPrefs.Tables[0].Rows[0]["Sex"].ToString();

                        LocationTextBox.Text = dsUserPrefs.Tables[0].Rows[0]["Location"].ToString();

                        if (dsUserPrefs.Tables[0].Rows[0]["CalendarPrivacyMode"].ToString() != null)
                        {
                            PublicPrivateCheckList.SelectedValue = dsUserPrefs.Tables[0].Rows[0]["CalendarPrivacyMode"].ToString();
                        }

                        //if (dsUserPrefs.Tables[0].Rows[0]["PollPreferences"].ToString() != null)
                        //{
                        //    PollRadioList.SelectedValue = dsUserPrefs.Tables[0].Rows[0]["PollPreferences"].ToString();
                        //}

                        if (dsUserPrefs.Tables[0].Rows[0]["CommentsPreferences"].ToString() != null)
                        {
                            CommentsRadioList.SelectedValue = dsUserPrefs.Tables[0].Rows[0]["CommentsPreferences"].ToString();
                        }
                        //if (dsUserPrefs.Tables[0].Rows[0]["CategoriesOnOff"].ToString() != null)
                        //{
                        //    if (bool.Parse(dsUserPrefs.Tables[0].Rows[0]["CategoriesOnOff"].ToString()))
                        //        CategoriesOnOffRadioList.SelectedValue = "1";
                        //    else
                        //        CategoriesOnOffRadioList.SelectedValue = "2";
                        //}



                        if (dsUserPrefs.Tables[0].Rows[0]["RecommendationPrefs"].ToString() != null)
                        {
                            string recom = dsUserPrefs.Tables[0].Rows[0]["RecommendationPrefs"].ToString();
                            if (recom.Contains("1"))
                                RecommendationsCheckList.Items[0].Selected = true;
                            if (recom.Contains("2"))
                                RecommendationsCheckList.Items[1].Selected = true;
                            if (recom.Contains("3"))
                                RecommendationsCheckList.Items[2].Selected = true;
                        }

                        if (dsUserPrefs.Tables[0].Rows[0]["CommunicationPrefs"].ToString() != null)
                        {
                            CommunicationPrefsRadioList.SelectedValue =
                                dsUserPrefs.Tables[0].Rows[0]["CommunicationPrefs"].ToString();
                        }


                        //if (dsUserPrefs.Tables[0].Rows[0]["Country"].ToString() != null)
                        //{
                        //    if (dsUserPrefs.Tables[0].Rows[0]["Country"].ToString().Trim() != "")
                        //    {
                        //        if (dsUserPrefs.Tables[0].Rows[0]["Address"].ToString() != null)
                        //        {
                        //            AddressTextBox.Text = dsUserPrefs.Tables[0].Rows[0]["Address"].ToString();
                        //        }

                        //        if (dsUserPrefs.Tables[0].Rows[0]["City"].ToString() != null)
                        //        {
                        //            BillCityTextBox.Text = dsUserPrefs.Tables[0].Rows[0]["City"].ToString();
                        //        }

                        //        if (dsUserPrefs.Tables[0].Rows[0]["ZIP"].ToString() != null)
                        //        {
                        //            ZipTextBox.Text = dsUserPrefs.Tables[0].Rows[0]["ZIP"].ToString();
                        //        }
                        //        BillCountryDropDown.DataBind();
                        //        BillCountryDropDown.ClearSelection();
                        //        BillCountryDropDown.SelectedValue = dsUserPrefs.Tables[0].Rows[0]["Country"].ToString();

                        //        DataSet dsStates = dat.GetData("SELECT * FROM State WHERE country_id=" + dsUserPrefs.Tables[0].Rows[0]["Country"].ToString());

                        //        bool isText = false;
                        //        if (dsStates.Tables.Count > 0)
                        //            if (dsStates.Tables[0].Rows.Count > 0)
                        //            {
                        //                BillStateDropDown.DataSource = dsStates;
                        //                BillStateDropDown.DataTextField = "state_2_code";
                        //                BillStateDropDown.DataValueField = "state_id";
                        //                BillStateDropDown.DataBind();
                        //                BillStateDropDown.Items.Insert(0, new ListItem("Select State..", "-1"));

                        //                if (dsUserPrefs.Tables[0].Rows[0]["State"].ToString() != null)
                        //                {
                        //                    ListItem a = BillStateDropDown.Items.FindByText(dsUserPrefs.Tables[0].Rows[0]["State"].ToString());
                        //                    if (a != null)
                        //                        BillStateDropDown.SelectedValue = a.Value;
                        //                }

                        //                BillStateDropPanel.Visible = true;
                        //                BillStateTextPanel.Visible = false;
                        //            }
                        //            else
                        //            {
                        //                isText = true;
                        //            }
                        //        else
                        //            isText = true;

                        //        if (isText)
                        //        {
                        //            if (dsUserPrefs.Tables[0].Rows[0]["State"].ToString() != null)
                        //            {
                        //                BillStateTextBox.Text = dsUserPrefs.Tables[0].Rows[0]["State"].ToString();
                        //            }
                        //            BillStateTextPanel.Visible = true;
                        //            BillStateDropPanel.Visible = false;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        BillCountryDropDown.DataBind();
                        //        BillCountryDropDown.SelectedValue = "223";
                        //    }
                        //}
                        //else
                        //{
                        //    BillCountryDropDown.DataBind();
                        //    BillCountryDropDown.SelectedValue = "223";
                        //}

                        if (dsUserPrefs.Tables[0].Rows[0]["CatCountry"].ToString() != null)
                        {
                            if (dsUserPrefs.Tables[0].Rows[0]["CatCountry"].ToString().Trim() != "")
                            {
                                if (dsUserPrefs.Tables[0].Rows[0]["CatCity"].ToString() != null)
                                {
                                    CityTextBox.Text = dsUserPrefs.Tables[0].Rows[0]["CatCity"].ToString();
                                }

                                CatZipTextBox.Text = dsUserPrefs.Tables[0].Rows[0]["CatZip"].ToString();

                                CountryDropDown.DataBind();
                                CountryDropDown.SelectedValue = dsUserPrefs.Tables[0].Rows[0]["CatCountry"].ToString();
                                ChangeState(CountryDropDown, new EventArgs());

                                DataSet dsStates = dat.GetData("SELECT * FROM State WHERE country_id=" + dsUserPrefs.Tables[0].Rows[0]["CatCountry"].ToString());

                                bool isText = false;
                                if (dsStates.Tables.Count > 0)
                                    if (dsStates.Tables[0].Rows.Count > 0)
                                    {
                                        StateDropDown.DataSource = dsStates;
                                        StateDropDown.DataTextField = "state_2_code";
                                        StateDropDown.DataValueField = "state_id";
                                        StateDropDown.DataBind();

                                        if (dsUserPrefs.Tables[0].Rows[0]["CatState"] != null)
                                        {
                                            StateDropDown.SelectedValue = StateDropDown.Items.FindByText(dsUserPrefs.Tables[0].Rows[0]["CatState"].ToString()).Value;
                                        }

                                        StateDropDownPanel.Visible = true;
                                        StateTextBoxPanel.Visible = false;
                                        RequiredFieldValidator2.Visible = false;
                                    }
                                    else
                                    {
                                        isText = true;
                                    }
                                else
                                    isText = true;

                                if (isText)
                                {
                                    RequiredFieldValidator2.Visible = true;
                                    if (dsUserPrefs.Tables[0].Rows[0]["CatState"].ToString() != null)
                                    {
                                        StateTextBox.Text = dsUserPrefs.Tables[0].Rows[0]["CatState"].ToString();
                                    }
                                    StateTextBoxPanel.Visible = true;
                                    StateDropDownPanel.Visible = false;
                                }
                                ChangeCity(new object(), new EventArgs());
                                if (CountryDropDown.SelectedValue == "223")
                                    MajorCityDrop.Items.FindByValue(dsUserPrefs.Tables[0].Rows[0]["MajorCity"].ToString()).Selected = true;
                            }
                        }
                        if (dsUserPrefs.Tables[0].Rows[0]["TextingPrefs"].ToString() != null)
                        {
                            if (dsUserPrefs.Tables[0].Rows[0]["TextingPrefs"].ToString().Contains("1"))
                                TextingCheckBoxList.Items[0].Selected = true;
                            //if (dsUserPrefs.Tables[0].Rows[0]["TextingPrefs"].ToString().Contains("2"))
                            //    TextingCheckBoxList.Items[1].Selected = true;
                            if (dsUserPrefs.Tables[0].Rows[0]["TextingPrefs"].ToString().Contains("3"))
                                TextingCheckBoxList.Items[2].Selected = true;
                        }

                        if (dsUserPrefs.Tables[0].Rows[0]["EmailPrefs"].ToString() != null)
                        {
                            if (dsUserPrefs.Tables[0].Rows[0]["EmailPrefs"].ToString().Contains("1"))
                                EmailCheckList.Items[0].Selected = true;
                            //if (dsUserPrefs.Tables[0].Rows[0]["EmailPrefs"].ToString().Contains("2"))
                            //    EmailCheckList.Items[1].Selected = true;

                            if (dsUserPrefs.Tables[0].Rows[0]["EmailPrefs"].ToString().Contains(EmailUserCheckList1.Items[0].Value))
                                EmailUserCheckList1.Items[0].Selected = true;
                            if (dsUserPrefs.Tables[0].Rows[0]["EmailPrefs"].ToString().Contains(EmailUserCheckList1.Items[1].Value))
                                EmailUserCheckList1.Items[1].Selected = true;
                            if (dsUserPrefs.Tables[0].Rows[0]["EmailPrefs"].ToString().Contains(EmailUserCheckList1.Items[2].Value))
                                EmailUserCheckList1.Items[2].Selected = true;
                            if (dsUserPrefs.Tables[0].Rows[0]["EmailPrefs"].ToString().Contains(EmailUserCheckList1.Items[3].Value))
                                EmailUserCheckList1.Items[3].Selected = true;

                            if (dsUserPrefs.Tables[0].Rows[0]["EmailPrefs"].ToString().Contains(EmailUserCheckList2.Items[0].Value))
                                EmailUserCheckList2.Items[0].Selected = true;
                            if (dsUserPrefs.Tables[0].Rows[0]["EmailPrefs"].ToString().Contains(EmailUserCheckList2.Items[1].Value))
                                EmailUserCheckList2.Items[1].Selected = true;
                            //if (dsUserPrefs.Tables[0].Rows[0]["EmailPrefs"].ToString().Contains(EmailUserCheckList2.Items[2].Value))
                            //    EmailUserCheckList2.Items[2].Selected = true;
                        }
                    }

                DataSet dsComments = d.GetData("SELECT * FROM User_Comments CU, Users U WHERE CU.CommenterID=U.User_ID AND CU.UserID=" + USER_ID.ToString());
                //Label UserNameLabel = (Label)Tab3.FindControl("UserNameLabel");

                //UserNameLabel.Text = dsUser.Tables[0].Rows[0]["UserName"].ToString();

                if (dsUser.Tables[0].Rows[0]["Email"].ToString() != null)
                {
                    EmailTextBox.Text = dsUser.Tables[0].Rows[0]["Email"].ToString();
                }

                if (dsUser.Tables[0].Rows[0]["PhoneNumber"].ToString() != null)
                {
                    PhoneTextBox.Text = dsUser.Tables[0].Rows[0]["PhoneNumber"].ToString();
                }

                if (dsUser.Tables[0].Rows[0]["PhoneProvider"].ToString() != null)
                {
                    ProviderDropDown.SelectedValue = dsUser.Tables[0].Rows[0]["PhoneProvider"].ToString();
                }

                Image FriendImage = (Image)Tab3.FindControl("FriendImage");

                if (dsUser.Tables[0].Rows[0]["ProfilePicture"].ToString() != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(".") + "/UserFiles/" + dsUser.Tables[0].Rows[0]["UserName"].ToString() + "/Profile/" + dsUser.Tables[0].Rows[0]["ProfilePicture"].ToString()))
                    {
                        System.Drawing.Image theimg = System.Drawing.Image.FromFile(Server.MapPath(".") + "/UserFiles/" + dsUser.Tables[0].Rows[0]["UserName"].ToString() +
                "/Profile/" + dsUser.Tables[0].Rows[0]["ProfilePicture"].ToString());

                        double width = double.Parse(theimg.Width.ToString());
                        double height = double.Parse(theimg.Height.ToString());

                        if (width > height)
                        {
                            if (width <= 150)
                            {

                            }
                            else
                            {
                                double dividor = double.Parse("150.00") / double.Parse(width.ToString());
                                width = double.Parse("150.00");
                                height = height * dividor;
                            }
                        }
                        else
                        {
                            if (width == height)
                            {
                                width = double.Parse("150.00");
                                height = double.Parse("150.00");
                            }
                            else
                            {
                                double dividor = double.Parse("150.00") / double.Parse(height.ToString());
                                height = double.Parse("150.00");
                                width = width * dividor;
                            }
                        }

                        FriendImage.Width = int.Parse((Math.Round(decimal.Parse(width.ToString()))).ToString());
                        FriendImage.Height = int.Parse((Math.Round(decimal.Parse(height.ToString()))).ToString());

                        FriendImage.ImageUrl = "~/UserFiles/" + dsUser.Tables[0].Rows[0]["UserName"].ToString() + "/Profile/" + dsUser.Tables[0].Rows[0]["ProfilePicture"].ToString();
                        Session["ProfilePicture"] = dsUser.Tables[0].Rows[0]["ProfilePicture"].ToString();
                    }
                    else
                    {
                        FriendImage.ImageUrl = "~/NewImages/NoAvatar150.jpg";
                    }
                }
                else
                    FriendImage.ImageUrl = "~/NewImages/NoAvatar150.jpg";
            }
            else
            {
                if (Session["User"] == null)
                {
                    ErrorLabel.Text = "something happen";
                    //Session.Abandon();
                    Response.Redirect("~/login");

                }
            }

            

            DataSet dsVenues = dat.GetData("SELECT * FROM UserVenues UV, Venues V WHERE V.ID=UV.VenueID AND UV.UserID=" + Session["User"].ToString());


            CheckBoxList VenueCheckBoxes = new CheckBoxList();
            VenueCheckBoxes.Width = 530;
            VenueCheckBoxes.CssClass = "VenueCheckBoxes";
            VenueCheckBoxes.ID = "VenueCheckBoxes";
            VenueCheckBoxes.RepeatColumns = 4;
            VenueCheckBoxes.RepeatDirection = RepeatDirection.Horizontal;

            VenueCheckBoxes.DataSource = dsVenues;
            VenueCheckBoxes.DataTextField = "NAME";
            VenueCheckBoxes.DataValueField = "ID";
            VenueCheckBoxes.DataBind();

            Literal lit = new Literal();
            lit.Text = "<div class=\"Pad2\">";
            VenuesChecksPanel.Controls.Add(lit);
            for (int i = 0; i < VenueCheckBoxes.Items.Count; i++)
            {
                VenueCheckBoxes.Items[i].Selected = true;
            }

            if (VenueCheckBoxes.Items.Count == 0)
            {
                Label label = new Label();
                label.CssClass = "VenueCheckBoxes";
                label.Text = "You have no venues specified as your favorite. To add venues as your favorites search for them on the <a href=\"venue-search\" class=\"NavyLink12\">Venues Page</a>";
                VenuesChecksPanel.Controls.Add(label);
            }
            else
            {
                VenuesChecksPanel.Controls.Add(VenueCheckBoxes);
            }
            lit = new Literal();
            lit.Text = "</div>";

            VenuesChecksPanel.Controls.Add(lit);
        }
        catch (Exception ex)
        {
            UserErrorLabel.Text = ex.ToString();
        }

        if (Request.QueryString["p"] != null)
        {
            RadTabStrip2.Tabs[2].Selected = true;
            RadTabStrip2.Tabs[2].CssClass = "MyTabsPreferencesSelected";
            RadTabStrip2.Tabs[0].CssClass = "MyTabsMessages";
            RadTabStrip2.Tabs[1].CssClass = "MyTabsFriends";
            TheMultipage.PageViews[2].Selected = true;
        }

        if (Request.QueryString["G"] != null)
        {
            RadTabStrip2.Tabs[3].Selected = true;
            RadTabStrip2.Tabs[3].CssClass = "MyTabsGroupsSelected";
            RadTabStrip2.Tabs[0].CssClass = "MyTabsMessages";
            RadTabStrip2.Tabs[1].CssClass = "MyTabsFriends";
            RadTabStrip2.Tabs[2].CssClass = "MyTabsPreferences";
            TheMultipage.PageViews[3].Selected = true;
        }
    }

    protected void FillCategories(DataSet dsContent, ref Telerik.Web.UI.RadTreeView treeView)
    {
        if (treeView.Nodes.Count > 0)
        {
            if (dsContent.Tables.Count > 0)
                for (int i = 0; i < dsContent.Tables[0].Rows.Count; i++)
                {
                    Telerik.Web.UI.RadTreeNode node = (Telerik.Web.UI.RadTreeNode)treeView.FindNodeByValue(dsContent.Tables[0].Rows[i]["CategoryID"].ToString());
                    
                    if(node != null)
                        node.Checked = true;
                }
        }
    }

    protected void DoAll()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (Session["User"] != null)
        {
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            try
            {
                if (Session["User"] == null)
                {
                    Response.Redirect("~/login");
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("~/login");
            }
            
            Session["UserName"] = dat.GetData("SELECT * FROM Users WHERE User_ID=" +
                Session["User"].ToString()).Tables[0].Rows[0]["UserName"].ToString();
            UserLabel.Text = Session["UserName"].ToString();
            ClearMessage();
            //CalendarLink.NavigateUrl = "my-calendar?ID=" + Session["User"].ToString();
            if (IsPostBack)
            {
                if (ViewState["FriendDS"] != null)
                    FillSearchPanel((DataSet)ViewState["FriendDS"]);
            }
            //CheckMayors();
            //GetBadges();
            FillVenues();
            FillRecommendedEvents();
            LoadControlsNotAJAX();
            LoadFriends();
        }
        else
        {
            ErrorLabel.Text = "somtin happen";
            Response.Redirect("login");
        }
    }

    //protected void CheckMayors()
    //{
    //    HttpCookie cookie = Request.Cookies["BrowserDate"];
    //    DateTime isNow = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
    //    Data dat = new Data(isNow);
    //    string thisMonth = isNow.Month.ToString() + "/1/" + isNow.Year.ToString();
        
    //    DataView dv = dat.GetDataDV("SELECT * FROM UserPreferences WHERE UserID=" + Session["User"].ToString());

    //    //Happenings
    //    DataView dvEventCount = dat.GetDataDV("SELECT COUNT(*) AS count  FROM Events WHERE UserName='" + Session["UserName"].ToString() + "'");

    //    DataView dvEventCountLast = dat.GetDataDV("SELECT COUNT(*) AS count FROM Events WHERE PostedOn >= CONVERT(DATETIME,'" +
    //        thisMonth + "') AND UserName='" + Session["UserName"].ToString() + "'");
    //    NumEventsLabel.Text = "<table cellpadding=\"0\"><tr><td width=\"89px\"><span class=\"TextNormal\" style='font-weight: bold;'>" + dvEventCount[0]["count"].ToString() +
    //        "</span> Happenings</td><td width=\"85px\"> <span class=\"TextNormal\" style='font-weight: bold;'>" + dvEventCountLast[0]["count"].ToString() +
    //        "</span> This Month</td></tr>";

    //    //Trips
    //    dvEventCount = dat.GetDataDV("SELECT COUNT(*) AS count  FROM Trips WHERE UserName='" + Session["UserName"].ToString() + "'");

    //    dvEventCountLast = dat.GetDataDV("SELECT COUNT(*) AS count  FROM Trips WHERE PostedOn >= CONVERT(DATETIME,'" +
    //        thisMonth + "') AND UserName='" + Session["UserName"].ToString() + "'");
    //    NumEventsLabel.Text += "<tr><td><span class=\"TextNormal\" style='font-weight: bold;'>" + dvEventCount[0]["count"].ToString() +
    //        "</span> Trips</td><td> <span class=\"TextNormal\" style='font-weight: bold;'>" + dvEventCountLast[0]["count"].ToString() +
    //        "</span> This Month</td></tr>";

    //    //Locales
    //    dvEventCount = dat.GetDataDV("SELECT COUNT(*) AS count  FROM Venues WHERE CreatedByUser=" + Session["User"].ToString());

    //    dvEventCountLast = dat.GetDataDV("SELECT COUNT(*) AS count  FROM Venues WHERE PostedOn >= CONVERT(DATETIME,'" +
    //        thisMonth + "') AND CreatedByUser=" + Session["User"].ToString());
    //    NumEventsLabel.Text += "<tr><td><span class=\"TextNormal\" style='font-weight: bold;'>" + dvEventCount[0]["count"].ToString() + 
    //        "</span> Locales</td><td><span class=\"TextNormal\" style='font-weight: bold;'>" + dvEventCountLast[0]["count"].ToString() +
    //        "</span> This Month</td></tr>";

    //    //Ads
    //    dvEventCount = dat.GetDataDV("SELECT COUNT(*) AS count  FROM Ads WHERE User_ID=" + Session["User"].ToString());

    //    dvEventCountLast = dat.GetDataDV("SELECT COUNT(*) AS count  FROM Ads WHERE DateAdded >= CONVERT(DATETIME,'" +
    //        thisMonth + "') AND User_ID=" + Session["User"].ToString());
    //    NumEventsLabel.Text += "<tr><td><span class=\"TextNormal\" style='font-weight: bold;'>" + dvEventCount[0]["count"].ToString() +
    //        "</span> Bulletins</td><td> <span class=\"TextNormal\" style='font-weight: bold;'>" + dvEventCountLast[0]["count"].ToString() +
    //        "</span> This Month</td></tr></table>";

    //    DataView dvOn = dat.GetDataDV("SELECT * FROM MayorsControl");
        
    //    if (bool.Parse(dv[0]["Mayors"].ToString()))
    //    {
    //        EnteredPanel.Visible = true;
    //        if (bool.Parse(dvOn[0]["MayorsOn"].ToString()))
    //        {
    //            EnteredEditLabel.Visible = true;
    //            EnteredNotEditLabel.Visible = false;
    //        }
    //        else
    //        {
    //            EnteredEditLabel.Visible = false;
    //            EnteredNotEditLabel.Visible = true;
    //        }
    //        NotEnteredPanelOn.Visible = false;
    //        NotEnteredPanelOff.Visible = false;
    //        FinalRatingLabel.Text = "<span class=\"Asterisk\" style='font-weight: bold;'>*</span>Final Rating for this month is on <span class=\"TextNormal\" style='font-weight: bold;'>" + 
    //            dat.GetMonth(isNow.Month.ToString()) +
    //            " " + DateTime.DaysInMonth(isNow.Year, isNow.Month).ToString() + "</span>";

    //        //Determine user's zip


    //        if (dvEventCountLast.Count > 0)
    //        {
    //            string rank = (GetUserRank()).ToString();
    //            if (rank == "0")
    //            {
    //                PlaceLabel.Text = "<span class=\"Text12\"><span class='Asterisk'></span>You do not yet have a "+
    //                    "placing among Hippo Boss contestants since you haven't posted any content this month.";
    //            }
    //            else
    //            {
    //                PlaceLabel.Text = "<span class=\"Text12\">#" +
    //                     rank +" Place for this Month's Hippo Boss.";
    //            }
    //        }
    //        else
    //        {
    //            PlaceLabel.Text = "<span class=\"Text12\">No</span> Place for this Month's Hippo Boss as you have not posted any events, trips, locales or bulletins this month.";
    //        }
    //    }
    //    else
    //    {
    //        EnteredPanel.Visible = false;
    //        if (bool.Parse(dvOn[0]["MayorsOn"].ToString()))
    //        {
    //            NotEnteredPanelOn.Visible = true;
    //            NotEnteredPanelOff.Visible = false;
    //        }
    //        else
    //        {
    //            NotEnteredPanelOn.Visible = false;
    //            NotEnteredPanelOff.Visible = true;
    //        }
    //    }
    //}

    protected int GetUserRank()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        DateTime isNow = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
        Data dat = new Data(isNow);

        DataView dvUser = dat.GetDataDV("SELECT * FROM UserPreferences WHERE UserID=" + Session["User"].ToString());

        string majorCity = dvUser[0]["MajorCity"].ToString();

        string thisMonth = DateTime.Now.Month.ToString() + "/1/" + DateTime.Now.Year.ToString();
        DataView dvAllCities = dat.GetDataDV("SELECT CatZip, UserID FROM UserPreferences " +
            "WHERE Mayors='True' AND MajorCity=" + majorCity);
        DataView dvAllZips = dat.GetDataDV("SELECT MC.MajorCity, MZ.MajorCityID, MZ.MajorCityZip, S.state_2_code AS State FROM " +
            "State S, MajorZips MZ, MajorCities MC WHERE S.state_name=MC.State AND MZ.MajorCityID=MC.ID");

        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        DataColumn dc = new DataColumn("MajorCity");
        dt.Columns.Add(dc);
        dc = new DataColumn("UserID");
        dt.Columns.Add(dc);
        dc = new DataColumn("MajorCityName");
        dt.Columns.Add(dc);
        dc = new DataColumn("MajorState");
        dt.Columns.Add(dc);
        DataRow dr;

        dc = new DataColumn("MajorCityID");
        dt2.Columns.Add(dc);
        dc = new DataColumn("MajorCityName");
        dt2.Columns.Add(dc);
        dc = new DataColumn("MajorState");
        dt2.Columns.Add(dc);
        dc = new DataColumn("MajorCountry");
        dt2.Columns.Add(dc);
        DataRow dr2;

        Hashtable cityHash = new Hashtable();

        DataView dvMajors;
        DataView dvLatsLongs;
        int zipParam = 0;

        bool insertD2 = false;


        string command = "";

        try
        {
            command = "DECLARE @minMax TABLE (Id int,data NVARCHAR(100), city NVARCHAR(MAX))BEGIN INSERT INTO @minMax " +
                "(Id, data, city) select count(user_id) AS count, user_id, majorcity from ads a, userpreferences u where  " +
                "u.userid=a.user_id and u.majorcity='" + majorCity + "' and " +
                "live='True' and dateadded >= convert(datetime, '" + thisMonth + "') group by " +
                "user_id, majorcity UNION select count(createdbyuser) AS count, createdbyuser, majorcity as " +
                "user_id from venues v, userpreferences u where u.majorcity='" + majorCity + "' and u.userid=v.createdbyuser " +
                "and live='True' and postedon >= convert(datetime, '" + thisMonth + "') " +
                "group by createdbyuser, majorcity UNION select count(u.user_id) AS count, u.user_id, majorcity " +
                "from events e, " +
                "users u, userpreferences up where up.majorcity='" + majorCity + "' and up.userid=u.user_id and live='True' " +
                "and u.username=e.username and e.postedon >= " +
                "convert(datetime, '" + thisMonth + "') group by u.user_id, majorcity UNION select count(u.user_id) " +
                "AS count, u.user_id, majorcity from trips e, users u, userpreferences up where up.majorcity='" + majorCity + "' and " +
                "u.user_id=up.userid and live='True' " +
                "and u.username=e.username and e.postedon >= convert(datetime, '" + thisMonth + "') " +
                "group by u.user_id, majorcity END select ROW_NUMBER() OVER (order by sum(id) desc) AS Row, " +
                "sum(id) as sum, data, city from @minMax group by data, city order by sum desc ";
            //ErrorLabel.Text = command;
            DataView dvWinners = dat.GetDataDV(command);

            #region Old Code
            //ArrayList sameRankUsers = new ArrayList();
            //string firstRank = "";
            //string allUsers = "";
            //string allUsersCreatedBy = "";
            //string winUser = "";
            //string filterUsers = "";

            //DataTable dtWin = new DataTable();
            //dc = new DataColumn("UserID");
            //dtWin.Columns.Add(dc);
            //dc = new DataColumn("WinRank");
            //dtWin.Columns.Add(dc);
            //dc = new DataColumn("MajorCity");
            //dtWin.Columns.Add(dc);
            //dc = new DataColumn("MajorCityName");
            //dtWin.Columns.Add(dc);
            //dc = new DataColumn("MajorState");
            //dtWin.Columns.Add(dc);
            //dc = new DataColumn("MajorCountry");
            //dtWin.Columns.Add(dc);
            //dc = new DataColumn("PostCount");
            //dtWin.Columns.Add(dc);
            //dc = new DataColumn("GlobalWinner");
            //dtWin.Columns.Add(dc);

            //if (dvUser[0]["CatCountry"].ToString() == "223")
            //{
            //    //For each user, get their major city
            //    foreach (DataRowView row in dvAllCities)
            //    {
            //        dvAllZips.RowFilter = "MajorCityZip = '" + row["CatZip"].ToString() + "'";

            //        dr = dt.NewRow();
            //        dr2 = dt2.NewRow();
            //        dr["UserID"] = row["UserID"];

            //        if (dvAllZips.Count > 0)
            //        {
            //            if (!cityHash.Contains(dvAllZips[0]["MajorCityID"].ToString()))
            //            {
            //                dr2["MajorCityID"] = dvAllZips[0]["MajorCityID"];
            //                dr2["MajorCityName"] = dvAllZips[0]["MajorCity"];
            //                dr2["MajorState"] = dvAllZips[0]["State"];
            //                dr2["MajorCountry"] = "USA";
            //                cityHash.Add(dvAllZips[0]["MajorCityID"].ToString(), "1");
            //                insertD2 = true;
            //            }
            //            else
            //            {
            //                insertD2 = false;
            //            }

            //            dr["MajorCity"] = dvAllZips[0]["MajorCityID"];
            //            dr["MajorCityName"] = dvAllZips[0]["MajorCity"];
            //            dr["MajorState"] = dvAllZips[0]["State"];
            //        }
            //        else
            //        {
            //            dvLatsLongs = dat.GetDataDV("SELECT Latitude, Longitude FROM ZipCodes WHERE Zip='" +
            //                    row["CatZip"].ToString() + "'");

            //            //If not found, find closest Latitude and Longitude
            //            zipParam = int.Parse(row["CatZip"].ToString());
            //            if (dvLatsLongs.Count == 0)
            //            {
            //                dvLatsLongs = null;
            //                while (dvLatsLongs == null)
            //                {
            //                    dvLatsLongs = dat.GetDataDV("SELECT Latitude, Longitude FROM ZipCodes WHERE Zip='" + zipParam++ + "'");
            //                    if (dvLatsLongs.Count > 0)
            //                    {

            //                    }
            //                    else
            //                    {
            //                        dvLatsLongs = null;
            //                    }
            //                }
            //            }

            //            dvMajors = dat.GetDataDV("SELECT GetDistance(" + dvLatsLongs[0]["Longitude"].ToString() +
            //                ", " + dvLatsLongs[0]["Latitude"].ToString() + ", ZC.Longitude, ZC.Latitude) AS " +
            //                "Distance, ZC.Zip, MC.MajorCity, MZ.MajorCityID, S.state_2_code AS State FROM " +
            //                "State S, MajorZips MZ, ZipCodes ZC, MajorCities MC " +
            //                "WHERE S.state_name=MC.State AND MZ.MajorCityZip=ZC.Zip AND MZ.MajorCityID=MC.ID ORDER BY Distance ASC");
            //            dr["MajorCity"] = dvMajors[0]["MajorCityID"];
            //            dr["MajorCityName"] = dvMajors[0]["MajorCity"];
            //            dr["MajorState"] = dvAllZips[0]["State"];

            //            if (!cityHash.Contains(dvAllZips[0]["MajorCityID"].ToString()))
            //            {
            //                dr2["MajorCityID"] = dvMajors[0]["MajorCityID"];
            //                dr2["MajorCityName"] = dvMajors[0]["MajorCity"];
            //                dr2["MajorState"] = dvMajors[0]["State"];
            //                dr2["MajorCountry"] = "USA";
            //                cityHash.Add(dvAllZips[0]["MajorCityID"].ToString(), "1");
            //                insertD2 = true;
            //            }
            //            else
            //            {
            //                insertD2 = false;
            //            }
            //        }

            //        if (insertD2)
            //        {
            //            dt2.Rows.Add(dr2);
            //        }

            //        dt.Rows.Add(dr);
            //    }


            //    //For each of the cities in the list of users, calculate the winner
            //    DataView dvCitiesAndUsers = new DataView(dt, "", "", DataViewRowState.CurrentRows);
            //    DataView dvCities = new DataView(dt2, "", "", DataViewRowState.CurrentRows);



            //    dvCities.Sort = "MajorCityID ASC";

            //    filterUsers = "";




            //    foreach (DataRowView row in dvCities)
            //    {
            //        filterUsers = "";
            //        firstRank = "";
            //        sameRankUsers = new ArrayList();
            //        dvCitiesAndUsers.RowFilter = "MajorCity=" + row["MajorCityID"].ToString();

            //        if (dvCitiesAndUsers.Count > 0)
            //        {
            //            foreach (DataRowView rowster in dvCitiesAndUsers)
            //            {
            //                if (filterUsers != "")
            //                    filterUsers += " OR ";
            //                filterUsers += " data=" + rowster["UserID"].ToString();
            //            }

            //            dvWinners.RowFilter = filterUsers;
            //            dvWinners.Sort = "Row ASC";

            //            //Resolve ties by checking the mediacategory fields
            //            if (dvWinners.Count > 0)
            //            {
            //            //    foreach (DataRowView rowTrow in dvWinners)
            //            //    {
            //            //        if (firstRank == "")
            //            //        {
            //            //            sameRankUsers.Add(rowTrow["data"].ToString());
            //            //            firstRank = rowTrow["Sum"].ToString();
            //            //            allUsers = " AND (u.user_id = " + rowTrow["data"].ToString();
            //            //            allUsersCreatedBy = " AND (createdbyuser = " + rowTrow["data"].ToString();
            //            //        }
            //            //        else
            //            //        {
            //            //            if (rowTrow["Sum"].ToString() == firstRank)
            //            //            {
            //            //                sameRankUsers.Add(rowTrow["data"].ToString());
            //            //                allUsers += " OR u.user_id = " + rowTrow["data"].ToString();
            //            //                allUsersCreatedBy += " OR createdbyuser = " + rowTrow["data"].ToString();
            //            //            }
            //            //            else
            //            //            {
            //            //                allUsers += " ) ";
            //            //                allUsersCreatedBy += " ) ";
            //            //                break;
            //            //            }
            //            //        }
            //            //    }
            //            //    if (allUsers != "")
            //            //        allUsers += " ) ";

            //            //    if (allUsersCreatedBy != "")
            //            //        allUsersCreatedBy += " ) ";
            //            //    if (sameRankUsers.Count > 1)
            //            //    {
            //            //        command = "DECLARE @minMax TABLE " +
            //            //            "(" +
            //            //                "Id int," +
            //            //                "data NVARCHAR(100)" +
            //            //            ")" +
            //            //            "BEGIN " +
            //            //              "INSERT INTO @minMax (Id, data) " +
            //            //                "select  count(user_id) AS count, user_id from ads u where live='True' " +
            //            //                "and dateadded >= convert(datetime, '" + thisMonth +
            //            //                "') and mediacategory > 0 " + allUsers + " group by user_id " +
            //            //            "UNION " +
            //            //            "select  count(createdbyuser) AS count, createdbyuser as user_id from venues " +
            //            //            "where live='True' and postedon >= convert(datetime, '" + thisMonth +
            //            //            "') and mediacategory > 0 " + allUsersCreatedBy + " group by createdbyuser " +
            //            //            "UNION " +
            //            //            "select  count(u.user_id) AS count, u.user_id from events e, users u where " +
            //            //            "live='True' and u.username=e.username and e.postedon >= convert(datetime, '" + thisMonth +
            //            //            "') and mediacategory > 0 " + allUsers + " group by u.user_id " +
            //            //            "UNION " +
            //            //            "select  count(u.user_id) AS count, u.user_id from trips e, users u where " +
            //            //            "live='True' and u.username=e.username and e.postedon >= convert(datetime, '" + thisMonth +
            //            //            "') and mediacategory > 0 " + allUsers + " group by u.user_id" +
            //            //                " END " +
            //            //            "select ROW_NUMBER() OVER (order by sum(id) desc) AS Row, sum(id) as sum, " +
            //            //            "data from @minMax group by data order by sum desc";

            //            //        DataView dvWinNew = dat.GetDataDV(command);

            //            //        if (dvWinNew.Count == 0)
            //            //        {
            //            //            winUser = dvWinners[0]["data"].ToString();
            //            //        }
            //            //        else
            //            //        {
            //            //            winUser = dvWinNew[0]["data"].ToString();
            //            //        }
            //            //    }
            //            //    else
            //            //    {
            //            //        winUser = dvWinners[0]["data"].ToString();
            //            //    }

            //                //dvWinners.RowFilter = "data = " + winUser;

            //                dr = dtWin.NewRow();
            //                dr["UserID"] = dvWinners[0]["data"];
            //                dr["MajorCity"] = row["MajorCityID"].ToString();
            //                dr["MajorCityName"] = row["MajorCityName"].ToString();
            //                dr["MajorState"] = row["MajorState"].ToString();
            //                dr["MajorCountry"] = "223";
            //                dr["WinRank"] = dvWinners[0]["Row"];
            //                dr["PostCount"] = dvWinners[0]["sum"];
            //                dr["GlobalWinner"] = false;
            //                dtWin.Rows.Add(dr);
            //            }
            //        }
            //    }

            //    #region String For SQL testing
            //    //DECLARE @minMax TABLE (
            //    //            Id int,
            //    //            data NVARCHAR(100)
            //    //        )
            //    //        BEGIN 
            //    //          INSERT INTO @minMax (Id, data) 
            //    //            select  count(user_id) AS count, user_id from ads where dateadded >= convert(datetime, '3/1/2011') group by user_id 
            //    //        UNION 
            //    //        select  count(createdbyuser) AS count, createdbyuser as user_id from venues where postedon >= convert(datetime, '3/1/2011') group by createdbyuser 
            //    //        UNION 
            //    //        select  count(u.user_id) AS count, u.user_id from events e, users u where u.username=e.username and e.postedon >= convert(datetime, '3/1/2011') group by u.user_id 
            //    //        UNION 
            //    //        select  count(u.user_id) AS count, u.user_id from trips e, users u where u.username=e.username and e.postedon >= convert(datetime, '3/1/2011') group by u.user_id
            //    //             END 
            //    //        select ROW_NUMBER() OVER (order by sum(id) desc) AS Row, sum(id) as sum, data from @minMax group by data order by sum desc
            //    #endregion
            //}
            //else
            //{
            //    //Get the winners not in US
            //    DataView dvAllNonUSUsers = dat.GetDataDV("SELECT CatCity, CatState, CatCountry, UserID " +
            //        "FROM UserPreferences WHERE CatCountry <> 223 AND Mayors='True' GROUP BY CatCountry, CatState, CatCity, UserID");

            //    DataView dvAllNonUSCities = dat.GetDataDV("SELECT CatCity, CatState, CatCountry " +
            //        "FROM UserPreferences WHERE CatCountry <> 223 AND Mayors='True' GROUP BY CatCountry, CatState, CatCity");

            //    foreach (DataRowView row in dvAllNonUSCities)
            //    {
            //        filterUsers = "";
            //        firstRank = "";
            //        sameRankUsers = new ArrayList();
            //        dvAllNonUSUsers.RowFilter = "CatCity='" + row["CatCity"].ToString() +
            //            "' AND CatState='" + row["CatState"].ToString() + "' AND CatCountry=" + row["CatCountry"].ToString();

            //        if (dvAllNonUSUsers.Count > 0)
            //        {
            //            foreach (DataRowView rowster in dvAllNonUSUsers)
            //            {
            //                if (filterUsers != "")
            //                    filterUsers += " OR ";
            //                filterUsers += " data=" + rowster["UserID"].ToString();
            //            }

            //            dvWinners.RowFilter = filterUsers;
            //            dvWinners.Sort = "Row ASC";

            //            if (dvWinners.Count > 0)
            //            {

            //            //    foreach (DataRowView rowTrow in dvWinners)
            //            //    {
            //            //        if (firstRank == "")
            //            //        {
            //            //            sameRankUsers.Add(rowTrow["data"].ToString());
            //            //            firstRank = rowTrow["Sum"].ToString();
            //            //            allUsers = " AND (u.user_id = " + rowTrow["data"].ToString();
            //            //            allUsersCreatedBy = " AND (createdbyuser = " + rowTrow["data"].ToString();
            //            //        }
            //            //        else
            //            //        {
            //            //            if (rowTrow["Sum"].ToString() == firstRank)
            //            //            {
            //            //                sameRankUsers.Add(rowTrow["data"].ToString());
            //            //                allUsers += " OR u.user_id = " + rowTrow["data"].ToString();
            //            //                allUsersCreatedBy += " OR createdbyuser = " + rowTrow["data"].ToString();
            //            //            }
            //            //            else
            //            //            {
            //            //                allUsers += " ) ";
            //            //                allUsersCreatedBy += " ) ";
            //            //                break;
            //            //            }
            //            //        }
            //            //    }

            //            //    if (allUsers != "")
            //            //        allUsers += ") ";

            //            //    if (allUsersCreatedBy != "")
            //            //        allUsersCreatedBy += ") ";

            //            //    if (sameRankUsers.Count > 1)
            //            //    {
            //            //        command = "DECLARE @minMax TABLE " +
            //            //            "(" +
            //            //                "Id int," +
            //            //                "data NVARCHAR(100)" +
            //            //            ")" +
            //            //            "BEGIN " +
            //            //              "INSERT INTO @minMax (Id, data) " +
            //            //                "select  count(user_id) AS count, user_id from ads u where live='True' " +
            //            //                "and dateadded >= convert(datetime, '" + thisMonth +
            //            //                "') and mediacategory > 0 " + allUsers + " group by user_id " +
            //            //            "UNION " +
            //            //            "select  count(createdbyuser) AS count, createdbyuser as user_id from venues " +
            //            //            "where live='True' and postedon >= convert(datetime, '" + thisMonth +
            //            //            "') and mediacategory > 0 " + allUsersCreatedBy + " group by createdbyuser " +
            //            //            "UNION " +
            //            //            "select  count(u.user_id) AS count, u.user_id from events e, users u where " +
            //            //            "live='True' and u.username=e.username and e.postedon >= convert(datetime, '" + thisMonth +
            //            //            "') and mediacategory > 0 " + allUsers + " group by u.user_id " +
            //            //            "UNION " +
            //            //            "select  count(u.user_id) AS count, u.user_id from trips e, users u where " +
            //            //            "live='True' and u.username=e.username and e.postedon >= convert(datetime, '" + thisMonth +
            //            //            "') and mediacategory > 0 " + allUsers + " group by u.user_id" +
            //            //                " END " +
            //            //            "select ROW_NUMBER() OVER (order by sum(id) desc) AS Row, sum(id) as sum, " +
            //            //            "data from @minMax group by data order by sum desc";

            //            //        DataView dvWinNew = dat.GetDataDV(command);

            //            //        if (dvWinNew.Count == 0)
            //            //        {
            //            //            winUser = dvWinners[0]["data"].ToString();
            //            //        }
            //            //        else
            //            //        {
            //            //            winUser = dvWinNew[0]["data"].ToString();
            //            //        }
            //            //    }
            //            //    else
            //            //    {
            //            //        winUser = dvWinners[0]["data"].ToString();
            //            //    }

            //                //dvWinners.RowFilter = "data = " + winUser;

            //                dr = dtWin.NewRow();
            //                dr["UserID"] = dvWinners[0]["data"];
            //                dr["MajorCity"] = row["CatCity"].ToString() + ", " + row["CatState"].ToString() +
            //                    ", " + row["CatCountry"].ToString();
            //                dr["MajorCityName"] = row["CatCity"].ToString();
            //                dr["MajorState"] = row["CatState"].ToString();
            //                dr["MajorCountry"] = row["CatCountry"].ToString();
            //                dr["WinRank"] = dvWinners[0]["Row"];
            //                dr["PostCount"] = dvWinners[0]["sum"];
            //                dr["GlobalWinner"] = false;
            //                dtWin.Rows.Add(dr);
            //            }
            //        }
            //    }
            //}

            //DataView dvAllWinners = new DataView(dtWin, "", "", DataViewRowState.CurrentRows);
            #endregion

            Label lab;

            //dvWinners.Sort = "WinRank ASC";
            dvWinners.RowFilter = "data=" + Session["User"].ToString();
            if (dvWinners.Count == 0)
                return 0;
            else
                return int.Parse(dvWinners[0]["Row"].ToString());
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString() + "<br/><br/>" + command;
            return 0;
        }
    }

    //protected void GetBadges()
    //{
    //    HttpCookie cookie = Request.Cookies["BrowserDate"];
    //    DateTime isNow = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
    //    Data dat = new Data(isNow);

    //    DataView dv = dat.GetDataDV("SELECT * FROM Mayors WHERE UserID=" + Session["User"].ToString());

    //    Literal lit;
    //    int num = 0;
    //    int remainder = 0;
    //    if (dv.Count == 0)
    //    {
    //        lit = new Literal();
    //        lit.Text = "<div>You do not have any badges to display. To find out how to get badges " +
    //            "visit our <a class=\"NavyLink12\" href=\"hippo-points\">Hippo Points Page</a>.</div>";

    //        BadgesPanel.Controls.Add(lit);
    //    }
    //    else
    //    {
    //        lit = new Literal();

    //        if (dv.Count != 0)
    //        {
    //            lit.Text += "<div style=\"position: relative;float: left;\"><div id=\"div1\" style=\"z-index: 10000;left: 55px; top: -50px;" +
    //                "padding: 10px;position: absolute;display: " +
    //                "none;background-color: white; width: 150px; border: solid 3px #09718F;\"><b>The Hippo:</b> " +
    //                "You received this badge because you have won the Hippo Boss award at least once." +
    //                "</div><div onmouseout=\"var theDiv = document.getElementById('div1');theDiv.style.display " +
    //                "= 'none';\" onmouseover=\"var theDiv = document.getElementById('div1');theDiv.style.display " +
    //                "= 'block';\" style=\"margin-right: 10px;margin-bottom: 10px;width: 72px; height: 50px; float: " +
    //                "left;background-image: url('NewImages/HippoBadge.png');\">" + dv.Count.ToString() + "</div></div>";
    //        }

    //        if (dv.Count >= 5)
    //        {
    //            num = dv.Count / 5;
    //            remainder = dv.Count - num * 5;
    //            lit.Text += "<div style=\"position: relative;float: left;\"><div id=\"div5\" style=\"z-index: 10000;left: 55px; top: -50px;" +
    //                "padding: 10px;position: absolute;display: " +
    //                "none;background-color: white; width: 150px; border: solid 3px #09718F;\"><b>The Bronze Hippo:</b> " +
    //                "You received this badge because you have won the Hippo Boss award 5 or more times." +
    //                "</div><div onmouseout=\"var theDiv = document.getElementById('div5');theDiv.style.display " +
    //                "= 'none';\" onmouseover=\"var theDiv = document.getElementById('div5');theDiv.style.display " +
    //                "= 'block';\" style=\"margin-right: 10px;margin-bottom: 10px;width: 72px; height: 50px; float: " +
    //                "left;background-image: url('NewImages/HippoBronzeBadge.png');\">" + num.ToString() + "</div></div>";
    //        }

    //        if (dv.Count >= 10)
    //        {
    //            num = dv.Count / 10;
    //            remainder = dv.Count - num * 10;
    //            lit.Text += "<div style=\"position: relative;float: left;\"><div id=\"div10\" style=\"z-index: 10000;left: 55px; top: -50px;" +
    //                "padding: 10px;position: absolute;display: " +
    //                "none;background-color: white; width: 150px; border: solid 3px #09718F;\"><b>The Silver Hippo:</b> " +
    //                "You received this badge because you have won the Hippo Boss award 10 or more times." +
    //                "</div><div onmouseout=\"var theDiv = document.getElementById('div10');theDiv.style.display " +
    //                "= 'none';\" onmouseover=\"var theDiv = document.getElementById('div10');theDiv.style.display " +
    //                "= 'block';\" style=\"margin-right: 10px;margin-bottom: 10px;width: 72px; height: 50px; float: " +
    //                "left;background-image: url('NewImages/HippoSilverBadge.png');\">" + num.ToString() + "</div></div>";
    //        }

    //        if (dv.Count >= 20)
    //        {
    //            num = dv.Count / 20;
    //            remainder = dv.Count - num * 20;
    //            lit.Text += "<div style=\"position: relative;float: left;\"><div id=\"div20\" style=\"z-index: 10000;left: 55px; top: -50px;" +
    //            "padding: 10px;position: absolute;display: "+
    //                "none;background-color: white; width: 150px; border: solid 3px #09718F;\"><b>The Golden Hippo:</b> " +
    //                "You received this badge because you have won the Hippo Boss award 20 or more times."+
    //                "</div><div onmouseout=\"var theDiv = document.getElementById('div20');theDiv.style.display "+
    //                "= 'none';\" onmouseover=\"var theDiv = document.getElementById('div20');theDiv.style.display "+
    //                "= 'block';\" style=\"margin-right: 10px;margin-bottom: 10px;width: 72px; height: 50px; float: "+
    //                "left;background-image: url('NewImages/HippoGoldBadge.png');\">" + num.ToString() + "</div></div>";
    //        }
            
    //        dv.RowFilter = "IsGlobal = 'True'";

    //        if (dv.Count != 0)
    //        {
    //            lit.Text += "<div style=\"position: relative;float: left;\"><div id=\"divG\" style=\"z-index: 10000;left: 55px; top: -50px;" +
    //                "padding: 10px;position: absolute;display: " +
    //                "none;background-color: white; width: 150px; border: solid 3px #09718F;\"><b>The Global Hippo:</b> " +
    //                "You received this badge because you have won the Hippo Boss award globally, not just in your location." +
    //                "</div><div onmouseout=\"var theDiv = document.getElementById('divG');theDiv.style.display " +
    //                "= 'none';\" onmouseover=\"var theDiv = document.getElementById('divG');theDiv.style.display " +
    //                "= 'block';\" style=\"margin-right: 10px;margin-bottom: 10px;width: 72px; height: 50px; float: " +
    //                "left;background-image: url('NewImages/HippoGlobalBadge.png');\">" + dv.Count.ToString() + "</div></div>";
    //        }

    //        BadgesPanel.Controls.Add(lit);
    //    }
    //}

    protected int AddMessages(DataSet ds, ref ArrayList a, bool areSent)
    {
        //Mode 4,5: venue,event changes request
        //Mode 2: Friend request
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        string message = "";
        try
        {
            int itemCount = 0;
            int times = 1;
            int unreadCount = 0;

            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            Panel barAllPanel = new Panel();
            barAllPanel.ID = "barAllPanel";
            barAllPanel.Width = 532;
            Telerik.Web.UI.RadPanelBar bar = new Telerik.Web.UI.RadPanelBar();
            bar.CssClass = "MessagesRadPanel";
            bar.ExpandAnimation.Type = Telerik.Web.UI.AnimationType.Linear;
            bar.ExpandAnimation.Duration = 50;
            bar.AllowCollapseAllItems = true;
            bar.ExpandMode = Telerik.Web.UI.PanelBarExpandMode.SingleExpandedItem;
            bar.Width = 532;

            Panel checksPanel = new Panel();

            int replyMessagesCount = 0;
            Literal barLit;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                message = ds.Tables[0].Rows[i]["ID"].ToString();
                if (itemCount == 20 * times)
                {
                    barLit = new Literal();
                    barLit.Text = "<div>";
                    barAllPanel.Controls.AddAt(0, barLit);
                    barLit = new Literal();
                    barLit.Text = "</div>";
                    barAllPanel.Controls.Add(barLit);

                    a.Add(barAllPanel);
                    barAllPanel = new Panel();
                    barAllPanel.Width = 532;

                    times++;
                }
                itemCount++;
                bar = new Telerik.Web.UI.RadPanelBar();
                bar.CollapseAnimation.Duration = 50;
                bar.CollapseAnimation.Type = Telerik.Web.UI.AnimationType.None;
                bar.ExpandAnimation.Duration = 50;
                bar.ExpandAnimation.Type = Telerik.Web.UI.AnimationType.None;
                bar.ID = "B" + ds.Tables[0].Rows[i]["ID"].ToString();
                bar.CssClass = "MessagesRadPanel";
                bar.AllowCollapseAllItems = true;
                bar.ExpandMode = Telerik.Web.UI.PanelBarExpandMode.SingleExpandedItem;
                bar.Width = 510;
                if (!areSent)
                {
                    bar.ItemClick += new Telerik.Web.UI.RadPanelBarEventHandler(ServerMarkRead);
                }
                Telerik.Web.UI.RadPanelItem item = new Telerik.Web.UI.RadPanelItem();

                item.BackColor = System.Drawing.Color.White;
                item.CssClass = "OneMessage";
                item.SelectedCssClass = "OneMessageSelected";
                

                CheckBox check = new CheckBox();
                if (!areSent)
                    check.ID = "X" + ds.Tables[0].Rows[i]["ID"].ToString();
                else
                    check.ID = "XS" + ds.Tables[0].Rows[i]["ID"].ToString();

                check.AutoPostBack = true;
                //check.CheckedChanged += new EventHandler(check_CheckedChanged);

                Literal lit32 = new Literal();
                #region Mark If Read
                if (!areSent)
                {
                    if (!bool.Parse(ds.Tables[0].Rows[i]["Read"].ToString()))
                    {
                        bar.Attributes.Add("onclick", "MakeUnBold('divID" + i.ToString() + "')");
                        item.Text = "<div id=\"divID" + i.ToString() + "\" style=\"font-weight: bold; " +
                        "background-color: white;\"><div style=\"float: left;\">From: <span class=\"NavyLink12\">" +
                            ds.Tables[0].Rows[i]["UserName"].ToString() +
                            "</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Subject:&nbsp;&nbsp;</div><div style=\"" +
                        "width: 180px; text-wrap: true; float: left;\">" + ds.Tables[0].Rows[i]["MessageSubject"].ToString() +
                            "</div><div style=\"float: right; margin-right: 8px;\">" +
                                DateTime.Parse(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString() + " " + DateTime.Parse(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortTimeString() + "</div></div>";
                        item.Value = ds.Tables[0].Rows[i]["ID"].ToString();
                    }
                    else
                    {
                        item.Text = "<div style=\"float: left;background-color: white;\">" +
                            "From: <span class=\"NavyLink12\">" +
                            ds.Tables[0].Rows[i]["UserName"].ToString() +
                            "</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Subject:&nbsp;&nbsp;</div><div style=\"" +
                        "width: 180px; text-wrap: true; float: left;\">" + ds.Tables[0].Rows[i]["MessageSubject"].ToString() +
                            "</div><div style=\"float: right; margin-right: 8px;\">" +
                                DateTime.Parse(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString() + " " + DateTime.Parse(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortTimeString() + "</div>";
                        item.Value = ds.Tables[0].Rows[i]["ID"].ToString();
                    }
                }
                else
                {
                    item.Text = "<div style=\"float: left;background-color: white;\">" +
                    "To: <span class=\"NavyLink12\">" + ds.Tables[0].Rows[i]["UserName"].ToString() +
                        "</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Subject:&nbsp;&nbsp;</div><div style=\"width: 180px;text-wrap: true; float: left;\">" + ds.Tables[0].Rows[i]["MessageSubject"].ToString() +
                        "</div><div style=\"float: right; margin-right: 8px;\">" + DateTime.Parse(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString() + " " + DateTime.Parse(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortTimeString() + "</div>";
                    item.Value = ds.Tables[0].Rows[i]["ID"].ToString();
                }

                #endregion

                #region Create Delete Button
                item.Expanded = false;


                Telerik.Web.UI.RadPanelItem item2 = new Telerik.Web.UI.RadPanelItem();
                item2.BackColor = System.Drawing.Color.White;
                item2.CssClass = "OneMessageContent";

                Panel panel = new Panel();
                panel.ID = "RadPanel" + ds.Tables[0].Rows[i]["ID"].ToString();

                Literal wrapLit = new Literal();
                wrapLit.Text = "<div class=\"topDiv\" style=\"background-color: white;min-height: 180px; overflow:hidden;\">" +
                    "<div style=\"width: 100%;\"><div align=\"right\" style=\" padding-bottom: 10px;padding-right: 22px; display: block;\">";
                panel.Controls.Add(wrapLit);

                #endregion

                #region Construct Message Content

                wrapLit = new Literal();
                wrapLit.Text = "</div></div><div class=\"topDiv\" style=\"width: 100%; display: block;\"><div style=\"float: left;\"> ";

                panel.Controls.Add(wrapLit);

                Label theMessage = new Label();
                theMessage.BackColor = System.Drawing.Color.White;
                theMessage.CssClass = "OneMessageContent";
                theMessage.Width = 280;
                theMessage.Text = ds.Tables[0].Rows[i]["MessageContent"].ToString();

                string groupID = "";

                DataSet dsEvent = new DataSet();
                DataSet dsSentUser = new DataSet();
                //if (ds.Tables[0].Rows[i]["Mode"].ToString() == "4")
                //{
                //   string abc = ds.Tables[0].Rows[i]["MessageContent"].ToString();
                //    string [] delimeter = {",UserID:"};
                //    string[] thetokens = abc.Replace("EventID:", "").Split(delimeter, StringSplitOptions.None);

                //    string[] delimeter2 = { ",RevisionID:" };
                //    string[] thetokens2 = thetokens[1].Split(delimeter2, StringSplitOptions.None);

                //    dsSentUser = dat.GetData("SELECT * FROM Users WHERE User_ID="+thetokens2[0]);
                //    dsEvent = dat.GetData("SELECT * FROM Events WHERE ID="+thetokens[0]);

                //    theMessage.Text = "Hello from HippoHappenings,<br/><br/> The user <a href=\"" + dat.MakeNiceName(dsSentUser.Tables[0].Rows[0]["UserName"].ToString()) + "_Friend\" class=\"Green12LinkNF\">" +
                //        dsSentUser.Tables[0].Rows[0]["UserName"].ToString() + "</a> has requested to make a " +
                //        "change to the event '" + dsEvent.Tables[0].Rows[0]["Header"].ToString() +
                //        "'.<br/>Click <a class=\"NavyLink12\" href=\"" + dat.MakeNiceName(dsEvent.Tables[0].Rows[0]["Header"].ToString()) +
                //        "_" + thetokens[0] + "_Event\">here</a> to view this event. <br/> <br/> We must fully stress that if you do not either accept or reject ALL the requested chanes " +
                //        "within <span class=\"NavyLink\" style=\"font-weight: bold;\">4 days</span>, your ownership of this event will be waived and taken over by someone else willing to be the moderator for this event." +
                //        "<br/>For each one of the changes which you accept, please select 'Accept Changes' on the right. If no changes are listed on the right, this means " +
                //        "the user chose to only add media (songs/videos/pictues) or add new categories which have been automatically added to the event.";
                //}
                //else if (ds.Tables[0].Rows[i]["Mode"].ToString() == "5")
                //{
                //    //VenueID:90,UserID:40,RevisionID:90
                //    string abc = ds.Tables[0].Rows[i]["MessageContent"].ToString();
                //    string[] delimeter = { ",UserID:" };
                //    string[] thetokens = abc.Replace("VenueID:", "").Split(delimeter, StringSplitOptions.None);

                //    string[] delimeter2 = { ",RevisionID:" };
                //    string[] thetokens2 = thetokens[1].Split(delimeter2, StringSplitOptions.None);

                //    dsSentUser = dat.GetData("SELECT * FROM Users WHERE User_ID=" + thetokens2[0]);
                //    string fromUserName = dsSentUser.Tables[0].Rows[0]["UserName"].ToString();
                //    dsEvent = dat.GetData("SELECT * FROM Venues WHERE ID=" + thetokens[0]);
                //    string eventName = dsEvent.Tables[0].Rows[0]["Name"].ToString();
                //    theMessage.Text = "Hello from HippoHappenings,<br/><br/> The user <a href=\"" + dat.MakeNiceName(fromUserName) + "_Friend\" class=\"Green12LinkNF\">" +
                //        fromUserName + "</a> has requested to make a " +
                //        "change to the venue '" + eventName +
                //        "'.<br/>Click <a class=\"NavyLink12\" href=\"" + dat.MakeNiceName(eventName) + "_" + thetokens[0] + "_Venue\">here</a> to view this venue.<br/> <br/> We must fully stress that if you do not either accept or reject ALL the requested chanes " +
                //        "within <span class=\"NavyLink\" style=\"font-weight: bold;\">7 days</span>, your ownership of this event will be waived and taken over by someone else willing to be the moderator for this event." +
                //        "<br/>For each one of the changes, please select 'Accept' or 'Reject'. If no changes are listed on the right, this means " +
                //        "the user chose to only add media (videos/pictues) which have been automatically added to the venue.";

                //}


                if (ds.Tables[0].Rows[i]["From_UserID"].ToString() == dat.HIPPOHAPP_USERID.ToString() && theMessage.Text.Contains("My Preferences"))
                {
                    theMessage.Text = theMessage.Text.Replace("<a class=\"NavyLink12\" href=\"UserPreferences.aspx\">My Preferences</a>.", "");
                    Literal theLit = new Literal();
                    theLit.Text = "<div class=\"OneMessageContent\">" + theMessage.Text +
                        "<div style=\"cursor: pointer;\" onclick=\"SelectPreferences();\" class=\"NavyLink12\">My Preferences</div></div>";
                    panel.Controls.Add(theLit);
                }
                else
                {
                    panel.Controls.Add(theMessage);
                }


                wrapLit = new Literal();
                wrapLit.Text = "</div>";

                panel.Controls.Add(wrapLit);

                if (!bool.Parse(ds.Tables[0].Rows[i]["Read"].ToString()))
                    unreadCount++;

                if (ds.Tables[0].Rows[i]["Mode"].ToString() == "2")
                {
                    DataSet ds3 = dat.GetData("SELECT * FROM User_Friends WHERE UserID=" + Session["User"].ToString() +
                    " AND FriendID=" + ds.Tables[0].Rows[i]["From_UserID"].ToString());

                    bool hasFriend = false;

                    if (ds3.Tables.Count > 0)
                        if (ds3.Tables[0].Rows.Count > 0)
                            hasFriend = true;
                        else
                            hasFriend = false;
                    else
                        hasFriend = false;

                    if (!areSent)
                    {
                        if (!hasFriend)
                        {
                            lit32 = new Literal();
                            lit32.Text = "<div style=\"float: left;\">";
                            panel.Controls.Add(lit32);

                            Panel wrapPanel = new Panel();
                            wrapPanel.ID = "acceptWrap" + ds.Tables[0].Rows[i]["ID"].ToString();

                            ASP.controls_bluebutton_ascx img = new ASP.controls_bluebutton_ascx();
                            img.BUTTON_TEXT = "Accept Friend";
                            img.COMMAND_ARGS = ds.Tables[0].Rows[i]["ID"].ToString() + "accept" + ds.Tables[0].Rows[i]["From_UserID"].ToString();
                            img.WIDTH = "100px";
                            wrapPanel.Controls.Add(img);
                            panel.Controls.Add(wrapPanel);
                            img.CLIENT_LINK_CLICK = "setWait(event);";
                            img.SERVER_CLICK += ServerAcceptFriend;

                            lit32 = new Literal();
                            lit32.Text = "</div>";
                            panel.Controls.Add(lit32);
                        }
                        else
                        {
                            Literal lit = new Literal();
                            lit.Text = "<div style=\"float: right; width: 220px;height: 30px; margin: 5px;\" class=\"Green12LinkNF\">You have accepted this gal/guy as a friend! Good luck, you two!</div>";
                            panel.Controls.Add(lit);
                        }
                    }
                    else
                    {
                        if (!hasFriend)
                        {
                            Literal lit = new Literal();
                            lit.Text = "<div style=\"float: right; width: 220px;height: 30px; margin: 5px;\" class=\"Green12LinkNF\">You are still waiting for a response from this user!</div>";
                            panel.Controls.Add(lit);
                        }
                        else
                        {
                            Literal lit = new Literal();
                            lit.Text = "<div style=\"float: right; width: 220px;height: 30px; margin: 5px;\" class=\"Green12LinkNF\">Your friend has already accepted your invitation!</div>";
                            panel.Controls.Add(lit);
                        }
                    }
                }
                //else if (ds.Tables[0].Rows[i]["Mode"].ToString() == "4" || 
                //    ds.Tables[0].Rows[i]["Mode"].ToString() == "5")
                //{
                //    string abc = ds.Tables[0].Rows[i]["MessageContent"].ToString();
                //    string [] delimeter = {",UserID:"};

                //    string temp = "VenueID:";
                //    if (ds.Tables[0].Rows[i]["Mode"].ToString() == "4")
                //        temp = "EventID:";
                //    string [] thetokens = abc.Replace(temp, "").Split(delimeter, StringSplitOptions.None);

                //    string[] delimeter2 = { ",RevisionID:" };
                //    string[] thetokens2 = thetokens[1].Split(delimeter2, StringSplitOptions.None);

                //    string temp2 = "";
                //    if (thetokens2[1].Trim() != "")
                //    {
                //        temp = "VenueRevisions";
                //        if (ds.Tables[0].Rows[i]["Mode"].ToString() == "4")
                //            temp = "EventRevisions";

                //        DataSet dsChanges = dat.GetData("SELECT * FROM " + temp + " WHERE ID=" + thetokens2[1]);
                        
                //        if (dsChanges.Tables[0].Rows.Count > 0)
                //        {
                //            temp = "Venues";
                //            if (ds.Tables[0].Rows[i]["Mode"].ToString() == "4")
                //                temp = "Events";

                //            temp2 = "VenueID";
                //            if (ds.Tables[0].Rows[i]["Mode"].ToString() == "4")
                //                temp2 = "EventID";

                //            DataSet dsEvent2 = dat.GetData("SELECT * FROM " + temp + " WHERE ID=" + dsChanges.Tables[0].Rows[0][temp2].ToString());

                //            Literal theLit = new Literal();
                //            theLit.Text = "<table style=\"margin-right: 10px;margin-bottom: 20px;border: solid 1px #dedbdb;\"><tr><td>";
                //            panel.Controls.Add(theLit);

                //            int count = 1;
                //            string tempstr = "<div class=\"topDiv\"><hr color=\"#dedbdb\" size=\"1\" width=\"100%\"/></div></td></tr><tr><td>";

                //            int tempInt = 9;
                //            if (ds.Tables[0].Rows[i]["Mode"].ToString() == "4")
                //                tempInt = 12;

                //            for (int n = 0; n < tempInt; n++)
                //            {
                //                InsertRevision(ref item2, dsChanges, n, tempstr, ref count, ds, i);
                //            }
                //            if (ds.Tables[0].Rows[i]["Mode"].ToString() == "5")
                //            {
                //                CategoryChanges(ref item2, thetokens2, ref count, tempstr, true);
                //            }

                //            if (ds.Tables[0].Rows[i]["Mode"].ToString() == "4")
                //            {
                //                EventOccuranceChanges(ref item2, ref count, thetokens2, tempstr);
                //                CategoryChanges(ref item2, thetokens2, ref count, tempstr, false);
                //            }
                //            theLit = new Literal();
                //            theLit.Text = "</td></tr></table>";
                //            panel.Controls.Add(theLit);
                //        }
                //    }
                //}
                else
                {
                    if (!areSent)
                    {

                        if (ds.Tables[0].Rows[i]["From_UserID"].ToString() == dat.HIPPOHAPP_USERID.ToString())
                        {

                        }
                        else
                        {
                            //Insert ability to reply to message
                            Literal lit = new Literal();
                            lit.Text = "<div style=\"width: 220px; float: right;\" >";

                            panel.Controls.Add(lit);

                            TextBox textbox = new TextBox();
                            textbox.ID = ds.Tables[0].Rows[i]["From_UserID"].ToString() + "textbox" + ds.Tables[0].Rows[i]["ID"].ToString();
                            textbox.Width = 200;
                            textbox.Height = 100;
                            textbox.TextMode = TextBoxMode.MultiLine;

                            panel.Controls.Add(textbox);

                            lit = new Literal();
                            lit.Text = "<div style=\"float: left;\">";

                            panel.Controls.Add(lit);

                            ASP.controls_bluebutton_ascx img = new ASP.controls_bluebutton_ascx();
                            
                            img.BUTTON_TEXT = "Reply";
                            img.WIDTH = "57px";
                            img.COMMAND_ARGS = ds.Tables[0].Rows[i]["From_UserID"].ToString() + "reply" + ds.Tables[0].Rows[i]["ID"].ToString();
                            panel.Controls.Add(img);
                            img.CLIENT_LINK_CLICK = "setWait(event);";

                            img.SERVER_CLICK += ServerReply;

                            lit = new Literal();
                            lit.Text = "</div></div>";

                            panel.Controls.Add(lit);

                            replyMessagesCount++;
                        }
                    }
                }

                wrapLit = new Literal();

                wrapLit.Text = "</div></div>";
                panel.Controls.Add(wrapLit);

                UpdatePanel theUpdate = new UpdatePanel();
                theUpdate.UpdateMode = UpdatePanelUpdateMode.Conditional;
                theUpdate.ContentTemplateContainer.Controls.Add(panel);

                item2.Controls.Add(theUpdate);
                item.Items.Add(item2);
                bar.Items.Add(item);

                barLit = new Literal();
                barLit.Text = "<div class=\"topDiv\" style=\"clear: both;border-left: solid 1px #dedbdb;\"><div style=\"float: left;border-top: solid 1px #dedbdb;\">";
                barAllPanel.Controls.Add(barLit);
                barAllPanel.Controls.Add(check);
                barLit = new Literal();
                barLit.Text = "</div><div style=\"float: left;\">";
                barAllPanel.Controls.Add(barLit);
                barAllPanel.Controls.Add(bar);
                barLit = new Literal();
                barLit.Text = "</div></div>";
                barAllPanel.Controls.Add(barLit);
                #endregion
            }

            if (ds.Tables[0].Rows.Count % 20 != 0 || ds.Tables[0].Rows.Count == 20)
            {
                if (!areSent)
                {
                    bar.ItemClick += new Telerik.Web.UI.RadPanelBarEventHandler(ServerMarkRead);
                }
                barLit = new Literal();
                barLit.Text = "<div>";
                barAllPanel.Controls.AddAt(0, barLit);
                barLit = new Literal();
                barLit.Text = "</div>";
                barAllPanel.Controls.Add(barLit);
                a.Add(barAllPanel);
            }

            return unreadCount;
        }
        catch (Exception ex)
        {
            UserErrorLabel.Text = ex.ToString() + "<br/>" + message;
            return 0;
        }
    }

    protected void check_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox check = (CheckBox)sender;

        string idToAdd = check.ID.Replace("X", "");

        if (Session["MessagesToDelete"] == null)
            Session["MessagesToDelete"] = "";

        string messagesToDelete = Session["MessagesToDelete"].ToString();

        if (check.Checked)
        {
            if (!messagesToDelete.Contains(";" + idToAdd + ";"))
            {
                messagesToDelete += ";" + idToAdd + ";";
            }
        }
        else
        {
            if (messagesToDelete.Contains(";" + idToAdd + ";"))
            {
                messagesToDelete = messagesToDelete.Replace(";" + idToAdd + ";", "");
            }
        }
        Session["MessagesToDelete"] = messagesToDelete;
        if (Session["User"].ToString() == "80")
        {
            AleksLabel.Text = messagesToDelete + " got here";
        }
    }

    protected void DeleteMessages(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        
        try
        {
            CheckBox check;
            DataSet ds = dat.GetData("SELECT DISTINCT TOP 100 UM.From_UserID, UM.To_UserID, UM.ID FROM UserMessages UM, Users U WHERE (UM.To_UserID=" +
            Session["User"].ToString() + " AND UM.Live=1) ORDER BY UM.From_UserID, UM.To_UserID, UM.ID DESC");

            DataView dv = new DataView(ds.Tables[0], "", "", DataViewRowState.CurrentRows);

            foreach (DataRowView row in dv)
            {
                check = (CheckBox)dat.FindControlRecursive(this, "X" + row["ID"].ToString());

                if (check != null)
                {
                    if (check.Checked)
                    {
                        dat.Execute("UPDATE UserMessages SET Live=0, SentLive=0 WHERE ID=" + row["ID"].ToString());
                    }
                }
            }

            ds = dat.GetData("SELECT DISTINCT TOP 100 UM.From_UserID, UM.To_UserID, UM.ID FROM UserMessages UM, Users U WHERE (UM.From_UserID=" +
            Session["User"].ToString() + " AND UM.SentLive=1) ORDER BY UM.From_UserID, UM.To_UserID, UM.ID DESC");
            dv = new DataView(ds.Tables[0], "", "", DataViewRowState.CurrentRows);

            foreach (DataRowView row in dv)
            {
                check = (CheckBox)dat.FindControlRecursive(this, "XS" + row["ID"].ToString());

                if (check != null)
                {
                    if (check.Checked)
                    {
                        dat.Execute("UPDATE UserMessages SET Live=0, SentLive=0 WHERE ID=" + row["ID"].ToString());
                    }
                }
            }

            MessagesPanel.Controls.Clear();
            
            UsedMessagesPanel.Controls.Clear();
            LoadControlsNotAJAX();

            StreamWriter tw = new StreamWriter(Server.MapPath("~/js/TotalJS.js"), true);
            tw.WriteLine(" ");
            // close the stream
            tw.Close();


            UpdatePanel2.Update();
            
            //Response.Redirect("my-account");
        }
        catch (Exception ex)
        {
            UserErrorLabel.Text = ex.ToString();
        }
    }

    protected bool HasRegistrationEnded(string eventID, string reoccurrID)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));


        DataView dvGroup = dat.GetDataDV("SELECT * FROM GroupEvents WHERE ID=" + eventID);

        DataView dvGoingMember = dat.GetDataDV("SELECT * FROM GroupEvent_Members WHERE GroupEventID=" +
                        eventID + " AND Accepted='True' AND ReoccurrID=" +
                        reoccurrID);
        if (dvGroup[0]["RegType"].ToString() == "2")
        {
            if (dvGroup[0]["RegNum"] != null)
            {
                if (dvGroup[0]["RegNum"].ToString().Trim() != "")
                {
                    if (dvGoingMember.Count >= int.Parse(dvGroup[0]["RegNum"].ToString()))
                    {
                        return true;
                    }
                    else
                    {

                    }
                }

                if (dvGroup[0]["RegDeadline"] != null)
                {
                    if (dvGroup[0]["RegDeadline"].ToString().Trim() != "")
                    {
                        if (DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")) >
                            DateTime.Parse(dvGroup[0]["RegDeadline"].ToString()))
                        {
                            return true;
                        }
                    }
                }
            }

            if (dvGroup[0]["RegDeadline"] != null)
            {
                if (dvGroup[0]["RegDeadline"].ToString().Trim() != "")
                {
                    if (DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")) >
                        DateTime.Parse(dvGroup[0]["RegDeadline"].ToString()))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    //protected void InsertRevision(ref Telerik.Web.UI.RadPanelItem item2, DataSet dsChanges, int n, 
    //    string tempstr, ref int count, DataSet ds, int i)
    //{
    //    HttpCookie cookie = Request.Cookies["BrowserDate"];
    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //    if (dsChanges.Tables[0].Rows[0][n + 3].ToString().Trim() != "")
    //    {

    //        Label theLab = new Label();
    //        if (count != 1)
    //            theLab.Text = tempstr;

    //        string content = "";





    //        string str = "venue";
    //        if (ds.Tables[0].Rows[i]["Mode"].ToString() == "4")
    //            str = "event";

    //        bool isVenue = false;
    //        if (str == "venue")
    //            isVenue = true;

    //        string colName = dsChanges.Tables[0].Columns[n + 3].ColumnName.ToLower();

    //        if (!isVenue && (colName == "zip" || colName == "state" || colName == "city" || colName == "country"))
    //        {

    //        }
    //        else
    //        {
    //            if (dsChanges.Tables[0].Columns[n + 3].ColumnName.ToLower() == "address")
    //            {
    //                DataView dvV = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" +
    //                    dsChanges.Tables[0].Rows[0]["VenueID"].ToString());
    //                if (dvV[0]["Country"].ToString() == "223")
    //                    content = dat.GetAddress(dsChanges.Tables[0].Rows[0][n + 3].ToString(), false);
    //                else
    //                    content = dat.GetAddress(dsChanges.Tables[0].Rows[0][n + 3].ToString(), true);
    //            }
    //            else if (dsChanges.Tables[0].Columns[n + 3].ColumnName.ToLower() == "venue")
    //            {
    //                DataView dvV = dat.GetDataDV("SELECT * FROM Venues WHERE ID=" + dsChanges.Tables[0].Rows[0][n + 3].ToString());
    //                content = "<a class=\"NavyLink12\" target=\"_blank\" href=\"" +
    //                    dat.MakeNiceName(dvV[0]["Name"].ToString()) + "_" +
    //                    dsChanges.Tables[0].Rows[0][n + 3].ToString() +
    //                    "_Venue\">" + dvV[0]["Name"].ToString() + "</a>";
    //            }
    //            else
    //            {
    //                content = dsChanges.Tables[0].Rows[0][n + 3].ToString();
    //            }



    //            theLab.Text += count + ". " + dsChanges.Tables[0].Columns[n + 3].ColumnName + " Change Request To: <br/><br/>" +
    //                dat.BreakUpString(content, 30) + "<br/>";
    //            count++;
    //            item2.Controls.Add(theLab);

    //            bool notSeen = dat.isNotSeen(dsChanges, n);
    //            bool isApproved = dat.isApproved(dsChanges, n);

    //            bool changeHasBeenMade = true;

    //            if (notSeen && changeHasBeenMade)
    //            {
    //                ASP.controls_bluebutton_ascx img = new ASP.controls_bluebutton_ascx();
    //                img.SetAttribute("commArg", str);
    //                img.SetAttribute("commandargument", dsChanges.Tables[0].Rows[0]["ID"].ToString() +
    //                    "accept" + (n + 3).ToString());
    //                img.ID = dsChanges.Tables[0].Rows[0]["ID"].ToString() + "accept" + (n + 3).ToString();
    //                img.SERVER_CLICK += ServerAcceptChange;
    //                img.BUTTON_TEXT = "Accept";
    //                img.WIDTH = "50px";
    //                item2.Controls.Add(img);

    //                img = new ASP.controls_bluebutton_ascx();
    //                img.SetAttribute("commArg", str);
    //                img.SetAttribute("commandargument", dsChanges.Tables[0].Rows[0]["ID"].ToString() + "reject" +
    //                    (n + 3).ToString());
    //                img.ID = dsChanges.Tables[0].Rows[0]["ID"].ToString() + "reject" + (n + 3).ToString();
    //                img.SERVER_CLICK += ServerRejectChange;

    //                img.BUTTON_TEXT = "Reject";

    //                item2.Controls.Add(img);
    //            }
    //            else
    //            {

    //                theLab = new Label();
    //                Label lab2 = new Label();
    //                if (isApproved)
    //                {

    //                    theLab.Text = "<br/><br/><span  class=\"Green12LinkNF FloatRight\">You have accepted this change.</span>";

    //                }
    //                else
    //                {
    //                    theLab.Text = "<br/><br/><span  class=\"Green12LinkNF FloatRight\">You have rejected this change.</span>";
    //                }

    //                item2.Controls.Add(lab2);
    //                item2.Controls.Add(theLab);
    //            }
    //            Literal theLit = new Literal();
    //            theLit.Text = "</td></tr><tr><td>";

    //            item2.Controls.Add(theLit);

    //        }
    //    }
    //}

    protected void EventOccuranceChanges(ref Telerik.Web.UI.RadPanelItem item2, ref int count, 
        string[] thetokens2, string tempstr)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet dsChanges = dat.GetData("SELECT * FROM EventRevisions_Occurance WHERE RevisionID=" + thetokens2[1]);

        if (dsChanges.Tables[0].Rows.Count > 0)
        {
            Label theLab = new Label();
            theLab.Text = tempstr + count.ToString() + ". The following re-occurance dates have been added <br/><br/>";
            count++;
            item2.Controls.Add(theLab);

            theLab = new Label();
            for (int n = 0; n < dsChanges.Tables[0].Rows.Count; n++)
            {

                theLab.Text += dsChanges.Tables[0].Rows[n]["DateTimeStart"].ToString() + "<br/><div class='topDiv'>";
            }

            item2.Controls.Add(theLab);

            if (dsChanges.Tables[0].Rows[0]["Approved"].ToString().Trim() == "")
            {
                HtmlButton img = new HtmlButton();
                img.Attributes.Add("commArg", "event");
                img.Attributes.Add("commandargument", thetokens2[1] + "occurance");
                img.ID = dsChanges.Tables[0].Rows[0]["ID"].ToString() + "occurance";
                img.Style.Value = "float: right;font-size: 11px;font-weight: bold;margin-top: 20px; padding-bottom: 4px;height: 30px; width: 112px;background-color: transparent; " +
                "color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat: " +
                "no-repeat; border: 0;";
                img.ServerClick += new EventHandler(ServerAcceptChange);
                img.Attributes.Add("onmouseover", "this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';");
                img.Attributes.Add("onmouseout", "this.style.backgroundImage = 'url(image/PostButtonNoPost.png)';");
                img.Attributes.Add("onclick", "this.innerHTML = 'Working...';this.disabled=true;this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';");

                img.InnerText = "Accept";

                item2.Controls.Add(img);

                img = new HtmlButton();
                img.Attributes.Add("commArg", "event");
                img.Attributes.Add("commandargument", thetokens2[1] + "occurance");
                img.ID = dsChanges.Tables[0].Rows[0]["ID"].ToString() + "Rejectoccurance";
                img.Style.Value = "float: right;font-size: 11px;font-weight: bold;margin-top: 20px; padding-bottom: 4px;height: 30px; width: 112px;background-color: transparent; " +
                "color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat: " +
                "no-repeat; border: 0;";
                img.ServerClick += new EventHandler(ServerRejectChange);
                img.Attributes.Add("onmouseover", "this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';");
                img.Attributes.Add("onmouseout", "this.style.backgroundImage = 'url(image/PostButtonNoPost.png)';");
                img.Attributes.Add("onclick", "this.innerHTML = 'Working...';this.disabled=true;this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';");

                img.InnerText = "Reject";

                item2.Controls.Add(img);
            }
            else
            {
                theLab = new Label();
                if (bool.Parse(dsChanges.Tables[0].Rows[0]["Approved"].ToString()))
                {
                    theLab.Text = "<br/><br/><span  class=\"Green12LinkNF FloatRight\">You have accepted this change.</span>";
                }
                else
                {
                    theLab.Text = "<br/><br/><span  class=\"Green12LinkNF FloatRight\">You have rejected this change.</span>";
                }
                item2.Controls.Add(theLab);
            }
            theLab = new Label();
            theLab.Text = "</div>";
            item2.Controls.Add(theLab);

        }
    }

    protected void CategoryChanges(ref Telerik.Web.UI.RadPanelItem item2, string[] thetokens2, 
        ref int count, string tempstr, bool isVenue)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        string categoryRevisions = "EventCategoryRevisions";
        string categories = "EventCategories";
        string nameID = "EventID";
        string EventOVenue = "event";
        if (isVenue)
        {
            categoryRevisions = "VenueCategoryRevisions";
            categories = "VenueCategories";
            nameID = "VenueID";
            EventOVenue = "venue";
        }

        DataSet dsChanges = dat.GetData("SELECT * FROM "+categoryRevisions+" VCR, "+categories+" VC " +
                                    "WHERE VCR.CatID=VC.ID AND VCR.RevisionID=" + thetokens2[1]);
   
        if (dsChanges.Tables.Count > 0)
        {
            if (dsChanges.Tables[0].Rows.Count > 0)
            {
                Literal theLab = new Literal();
                theLab.Text = tempstr + count.ToString() + ". The following category changes have been suggested <br/><br/>";
                count++;
                item2.Controls.Add(theLab);
                string tempS = "Add ";
                HtmlButton theButt;
                Literal lab;
                Literal lit;
                HtmlButton rejectButt;

                for (int h = 0; h < dsChanges.Tables[0].Rows.Count; h++)
                {
                    lit = new Literal();
                    lit.Text = "<div style=\"width: 240px;\">";
                    item2.Controls.Add(lit);
                    theButt = new HtmlButton();
                    rejectButt = new HtmlButton();
                    rejectButt.Style.Value = "cursor: pointer;font-size: 11px;font-weight: bold;margin-top: 20px; padding-bottom: 4px;height: 30px; width: 112px;background-color: transparent; " +
                "color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat: " +
                "no-repeat; border: 0;";
                    theButt.Style.Value = "cursor: pointer;font-size: 11px;font-weight: bold;margin-top: 20px; padding-bottom: 4px;height: 30px; width: 112px;background-color: transparent; " +
                "color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat: " +
                "no-repeat; border: 0;";
                    theButt.Attributes.Add("commandargument", dsChanges.Tables[0].Rows[h][nameID].ToString() + "category" + EventOVenue + "category" + dsChanges.Tables[0].Rows[h]["CatID"].ToString() + "category" + dsChanges.Tables[0].Rows[h]["ID"].ToString());
                    rejectButt.Attributes.Add("commandargument", dsChanges.Tables[0].Rows[h][nameID].ToString() + "category" + EventOVenue + "category" + dsChanges.Tables[0].Rows[h]["CatID"].ToString() + "category" + dsChanges.Tables[0].Rows[h]["ID"].ToString());

                    theButt.Attributes.Add("onmouseover", "this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';");
                    theButt.Attributes.Add("onmouseout", "this.style.backgroundImage = 'url(image/PostButtonNoPost.png)';");
                    theButt.Attributes.Add("onclick", "this.innerHTML = 'Working...';this.disabled=true;this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';");

                    rejectButt.Attributes.Add("onmouseover", "this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';");
                    rejectButt.Attributes.Add("onmouseout", "this.style.backgroundImage = 'url(image/PostButtonNoPost.png)';");
                    rejectButt.Attributes.Add("onclick", "this.innerHTML = 'Working...';this.disabled=true;this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';");

                    theLab = new Literal();
                    Label lab2 = new Label();
                    lit = new Literal();
                    if (!bool.Parse(dsChanges.Tables[0].Rows[h]["AddOrRemove"].ToString()))
                    {
                        if (dsChanges.Tables[0].Rows[h]["Approved"].ToString().Trim() != "")
                        {
                            if (dsChanges.Tables[0].Rows[h]["Approved"].ToString().ToLower() == "true")
                            {
                                lab = new Literal();


                                lab.Text = "<label class=\"Green12LinkNF\">&nbsp;&nbsp;&nbsp;&nbsp;Removed</label></div>";
                                theLab.Text = "<div align=\"right\" style=\"margin-left: 10px;padding-bottom: 4px;\">" + tempS + dsChanges.Tables[0].Rows[h]["Name"].ToString();
                                item2.Controls.Add(theLab);
                                item2.Controls.Add(lab2);
                                
                                item2.Controls.Add(lab);
                                lit.Text = "<br/><br/>";
                                item2.Controls.Add(lit);
                            }
                            else
                            {
                                lab = new Literal();


                                lab.Text = "<label class=\"Green12LinkNF\">&nbsp;&nbsp;&nbsp;&nbsp;Rejected</label></div>";
                                theLab.Text = "<div align=\"right\" style=\"margin-left: 10px;padding-bottom: 4px;\">" + tempS + dsChanges.Tables[0].Rows[h]["Name"].ToString();
                                item2.Controls.Add(theLab);
                                item2.Controls.Add(lab2);
                                item2.Controls.Add(lab);
                                lit.Text = "<br/><br/>";

                                
                                item2.Controls.Add(lit);
                            }
                        }
                        else
                        {
                            tempS = "Remove ";
                            theLab.Text ="<div style=\"padding-bottom: 4px;\">"+ tempS + dsChanges.Tables[0].Rows[h]["Name"].ToString()+"<br/>";
                            item2.Controls.Add(theLab);
                            theButt.InnerHtml = tempS;
                            rejectButt.InnerHtml = "Reject";
                            rejectButt.ServerClick += new EventHandler(RejectCategory);
                            theButt.ServerClick += new EventHandler(RemoveCategory);
                            item2.Controls.Add(theButt);
                            item2.Controls.Add(rejectButt);
                            lit.Text = "</div><br/>";

                            item2.Controls.Add(lit);
                        }

                    }
                    else
                    {
                        if (dsChanges.Tables[0].Rows[h]["Approved"].ToString().Trim() != "")
                        {
                            if (dsChanges.Tables[0].Rows[h]["Approved"].ToString().ToLower() == "true")
                            {
                                lab = new Literal();
                                lab.Text = "<label class=\"Green12LinkNF\">&nbsp;&nbsp;&nbsp;&nbsp;Added</label></div>";

                                theLab.Text = "<div align=\"right\" style=\"margin-left: 10px;padding-bottom: 4px;\">" +
                                    tempS + dsChanges.Tables[0].Rows[h]["Name"].ToString();
                                item2.Controls.Add(theLab);
                                item2.Controls.Add(lab);
                                lit.Text = "<br/><br/>";

                                item2.Controls.Add(lab2);
                                item2.Controls.Add(lit);
                            }
                            else
                            {
                                lab = new Literal();
                                lab.Text = "<label class=\"Green12LinkNF\">&nbsp;&nbsp;&nbsp;&nbsp;Rejected</label></div>";

                                theLab.Text = "<div align=\"right\" style=\"margin-left: 10px;padding-bottom: 4px;\">" +
                                    tempS + dsChanges.Tables[0].Rows[h]["Name"].ToString();
                                item2.Controls.Add(theLab);
                                item2.Controls.Add(lab2);
                                item2.Controls.Add(lab);
                                lit.Text = "<br/><br/>";

                                
                                item2.Controls.Add(lit);
                            }
                        }
                        else
                        {
                            tempS = "Add ";
                            theButt.ServerClick += new EventHandler(AddCategory);
                            rejectButt.ServerClick += new EventHandler(RejectCategory);
                            theLab.Text = "<div style=\"padding-bottom: 4px;\">" + tempS + 
                                dsChanges.Tables[0].Rows[h]["Name"].ToString()+"<br/>";
                            item2.Controls.Add(theLab);
                            theButt.InnerHtml = tempS;
                            rejectButt.InnerHtml = "Reject";
                            item2.Controls.Add(theButt);
                            item2.Controls.Add(rejectButt);
                            lit.Text = "</div><br/><br/>";
                            item2.Controls.Add(lit);
                        }
                    }


                    lit = new Literal();
                    lit.Text = "</div>";
                    item2.Controls.Add(lit);


                }
            }
        }
    }

    protected void LoadControlsNotAJAX()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        try
        {
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            DataSet ds = dat.GetData("SELECT TOP 100 UM.MessageSubject, UM.ID, UM.From_UserID, UM.MessageContent, UM.[Read], UM.Date, U.UserName, UM.Mode FROM UserMessages UM, Users U WHERE UM.Live='True' AND UM.To_UserID=" +
                Session["User"].ToString() + " AND UM.From_UserID=U.User_ID AND UM.LIVE=1 ORDER BY Date DESC");

            ASP.controls_pager_ascx pagerPanel = new ASP.controls_pager_ascx();
            
            pagerPanel.PANEL_NAME = "ctl00_ContentPlaceHolder1_ctl156_Panel";
            pagerPanel.NUMBER_OF_ITEMS_PER_PAGE = 1;
            pagerPanel.WIDTH = 530;
            pagerPanel.NUMBER_OF_VISIBLE_PAGES = 4;
            ArrayList a = new ArrayList(ds.Tables[0].Rows.Count);

            if (ds.Tables[0].Rows.Count == 0)
            {
                Literal lit = new Literal();
                lit.Text = "<div style=\"border: solid 3px #dedbdb;" +
               "float: left; width: 530px;\"><div style=\"width: 530px; display: block; float: left; margin" +
               "-left: 5px;\"><table width=\"100%\"><tr><td><span class=\"TextNormal\" "+
               "style=\"padding-left: 5px;\">(0 messages)</span></td></tr></table></div>" +
               "<div style=\"width: 530px; display: block; float: left; margin-left: 5px; padding-bottom: 5px;\">" +
               "</div></div>";
                MessagesPanel.Controls.Add(lit);
            }
            else
            {
                int unreadCount = AddMessages(ds, ref a, false);

                

                Label label = new Label();

                string temp = "messages";
                if (ds.Tables[0].Rows.Count == 1)
                    temp = "message";

                if (unreadCount > 0)
                {
                    RadTabStrip3.Tabs[0].Text = "<div style=\"float: left;\">Inbox </div>" +
                        "<div style=\"float: left;font-size: 12px; vertical-align: " +
                        "middle; font-weight: normal; padding-bottom: 3px;\" id=\"InboxDiv\">" +
                        "<div style=\"float: left;\">&nbsp;(</div><div style=\"float: left;\" id=\"InboxInnerDiv\">" +
                        unreadCount.ToString() + " </div><div style=\"float: left;\">&nbsp;New)</div></div>";
                    
                    //label.Text = "<span style=\"font-family: Arial; font-size: 20px; color: White;\">My Messages</span>"
                    //    + "<span style=\"font-family: Arial; font-size: 12px;   padding-left: 5px;\">(" + unreadCount.ToString()
                    //    + " new " + temp + ")</span>";
                    RadTabStrip3.Tabs[0].Value = unreadCount.ToString();
                }
                MessagesPanel.Controls.Clear();
                //MessagesPanel.Controls.Add(label);
                MessagesPanel.Controls.Add(pagerPanel);
                pagerPanel.PANEL_NAME = pagerPanel.ClientID + "_Panel";

                pagerPanel.DATA = a;
                pagerPanel.DataBind2();
            }





            ds = dat.GetData("SELECT TOP 100 UM.MessageContent, UM.ID, UM.To_UserID AS From_UserID, UM.MessageSubject, UM.[Read], UM.Mode, UM.Date, U.UserName FROM UserMessages UM, Users U WHERE UM.From_UserID=" +
                Session["User"].ToString() + " AND U.User_ID=UM.To_UserID AND UM.SentLive=1 ORDER BY Date DESC");

            pagerPanel = new ASP.controls_pager_ascx();
            pagerPanel.PANEL_NAME = "ctl00_ContentPlaceHolder1_ctl157_Panel";
            pagerPanel.NUMBER_OF_ITEMS_PER_PAGE = 1;
            pagerPanel.WIDTH = 530;
            a = new ArrayList(ds.Tables[0].Rows.Count);

            if (ds.Tables[0].Rows.Count == 0)
            {
                Literal lit = new Literal();
                lit.Text = "<div style=\"border: solid 3px #dedbdb;" +
               "float: left; width: 530px;\"><div style=\"width: 530px; display: block; float: left; margin" +
               "-left: 5px;\"><table width=\"100%\"><tr><td><span class=\"TextNormal\" style=\" "+
               "padding-left: 5px;\">(0 sent messages)</span></td></tr></table></div>" +
               "<div style=\"width: 530px; display: block; float: left; margin-left: 5px; padding-bottom: 5px;\">" +
               "</div></div>";
                UsedMessagesPanel.Controls.Add(lit);
            }
            else
            {

                AddMessages(ds, ref a, true);

                UsedMessagesPanel.Controls.Clear();
                UsedMessagesPanel.Controls.Add(pagerPanel);

                pagerPanel.PANEL_NAME = pagerPanel.ClientID + "_Panel";

                pagerPanel.DATA = a;
                pagerPanel.DataBind2();
            }
        }
        catch (Exception ex)
        {
            UserErrorLabel.Text = ex.ToString();
        }
    }

    protected void LoadControls()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        TheDiv.InnerHtml = "";

        

        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet ds = dat.GetData("SELECT UM.MessageSubject, UM.MessageContent, U.UserName FROM UserMessages UM, Users U WHERE UM.Live='True' AND UM.To_UserID=" +
            Session["User"].ToString() + " AND UM.From_UserID=U.User_ID ORDER BY Date DESC");


        System.Drawing.Color greyText = System.Drawing.Color.FromArgb(102, 102, 102);
        System.Drawing.Color greyBack = System.Drawing.Color.FromArgb(51, 51, 51);

        ASP.controls_pager_ascx pagerPanel = new ASP.controls_pager_ascx();
        pagerPanel.PANEL_NAME = "ctl00_ContentPlaceHolder1_ctl14_Panel";
        pagerPanel.NUMBER_OF_ITEMS_PER_PAGE = 1;
        ArrayList a = new ArrayList(ds.Tables[0].Rows.Count);

        int unreadCount = 0;
        int count = 0;
        int times = 1;

        Panel ItemsPanel = new Panel();
        ItemsPanel.ID = "PanelM1";
        if (ds.Tables.Count > 0)
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    if (count == 10 * times)
                    {
                        a.Add(ItemsPanel);
                        times++;
                        ItemsPanel = new Panel();
                        ItemsPanel.ID = "PanelM" + times.ToString();
                    }
                    count++;
                    DataSet dsUsers = dat.GetData("SELECT * FROM Users WHERE User_ID=" + 
                        ds.Tables[0].Rows[i]["From_UserID"].ToString());

                    if (!bool.Parse(ds.Tables[0].Rows[i]["Read"].ToString()))
                        unreadCount++;

                    string boldOrNot = "font-size: 12px;";

                    if (!bool.Parse(ds.Tables[0].Rows[i]["Read"].ToString()))
                        boldOrNot = "font-weight:bold;font-size: 14px;";

                    HtmlGenericControl thediv = new HtmlGenericControl();
                    thediv.Attributes.Add("class", "MailHeader");
                    thediv.Attributes.Add("style", boldOrNot);
                    thediv.Attributes.Add("onclick", "OpenEmail('" + i.ToString() + "');");
                    //thediv.Attributes.Add("onserverclick", "MarkAsRead2");
                    thediv.ID = "header" + i.ToString();
                    


                    string thedivsText = "";
                    //thedivsText = "<div class=\"MailHeader\" style=\""+boldOrNot+"\" "+
                    //    "onclick=\"OpenEmail('"+i.ToString()+"');\" id=\"header"+i.ToString()+"\">";

                    //thedivsText += "<div style=\"display: none;\" id=\"readDiv"+i.ToString()+"\">"+
                    //    ds.Tables[0].Rows[i]["Read"].ToString()+"</div>";
                    //thedivsText += "<div style=\"display: none;\" id=\"idDiv"+i.ToString()+"\">"+
                    //    ds.Tables[0].Rows[i]["ID"].ToString()+"</div>";
                    //thedivsText += "<div style=\"display: none;\" id=\"userDiv" + i.ToString() + "\">" + 
                    //    Session["User"].ToString() + "</div>";
                    //thedivsText += "<div style=\"display: none;\" id=\"fromDiv" + i.ToString() + "\">" + 
                    //    ds.Tables[0].Rows[i]["From_UserID"].ToString() + "</div>";

                    string tempor2 = "";
                    string theUser = dsUsers.Tables[0].Rows[0]["UserName"].ToString();
                    if (ds.Tables[0].Rows[i]["Mode"].ToString() == "1")
                    {
                        theUser = "HippoHappenings";
                        tempor2 = " style=\"display:none;\" ";
                    }

                    string acceptHTML = "";
                    string tempor = "";
                    
                    if (ds.Tables[0].Rows[i]["Mode"].ToString() == "2")
                    {
                        tempor2 = " style=\"display:none;\" ";
                        tempor = "<a class=\"Green12LinkNF\" href=\"javascript:AcceptFriend('" + i.ToString() + "');\">Accept Friend Request</a>"; 
                        DataSet isFirend = dat.GetData("SELECT * FROM User_Friends WHERE UserID="+Session["User"].ToString()+" AND FriendID="+ds.Tables[0].Rows[i]["From_UserID"].ToString());
                        if (isFirend.Tables.Count > 0)
                            if (isFirend.Tables[0].Rows.Count > 0)
                                tempor = "<span class=\"Green12LinkNF\">You are already friends with "+theUser + "</span>";
                        acceptHTML = "<div class=\"Green12LinkNF\" id=\"accept"+i.ToString()+"\">"+tempor+"</div><br/>";
                    }

                    thedivsText += "<table width=\"530px\" cellpadding=\"0\" cellspacing=\"0\"><tr><td><span style=\"padding-left:5px;font-family:Arial;  "
                        + "\">From: </span><span style=\"color: #1fb6e7; font-family: Arial;\">" + theUser
                        + "</span></td><td ><span style=\"padding-top:10px;font-family:Arial;" +
                        " float:right;padding-right:5px;\">" +
                        ds.Tables[0].Rows[i]["Date"].ToString() + "</span></td></tr><tr><td colspan=\"3\"><span style=\"font-family:Arial; padding-left: 5px;\">Subject: <span id=\"subject" + i.ToString() + "\">"
                        + ds.Tables[0].Rows[i]["MessageSubject"].ToString() + 
                        "</span></span></td></tr></table>";

                    thedivsText += "</div>";
                    thediv.InnerHtml = thedivsText;

                    HtmlGenericControl messageDiv = new HtmlGenericControl();
                    messageDiv.Attributes.Add("style", "display: none;");
                    messageDiv.Attributes.Add("class", "MailContent EventBody");
                    messageDiv.ID = "contentDiv";

                    string messageDivsText = "";


                    //thedivsText += "<div style=\"display: none;\" class=\"MailContent EventBody\" id=\"contentDiv"+
                    //    i.ToString()+"\">";
                    messageDivsText += "<table><tr><td width=\"285px\" valign=\"top\"><label>" + 
                        ds.Tables[0].Rows[i]["MessageContent"].ToString()+" <br/><br/><br/> " +acceptHTML + "</label></td>";
                    messageDivsText += "<td valign=\"top\"><table ><tr><td width=\"285px\" align=\"right\">" +
                        "<img alt=\"Delete Message\" name=\"Delete Message\" style=\"cursor: pointer;\" "+
                        "onclick=\"DeleteEmail('" + i.ToString() + "');\" src=\"image/X.png\" "+
                        "onmouseover=\"this.src='image/XSelected.png'\" onmouseout=\"this.src='image/X.png'\" />"+
                        "</td></tr><tr><td " + tempor2 + "><textarea cols=\"30\" rows=\"10\" id=\"textDiv" + 
                        i.ToString() + "\"></textarea></td></tr>";
                    messageDivsText += "<tr><td " + tempor2 + "><img onclick=\"ReplyMessage('" + i.ToString() + 
                        "');\" src=\"image/ReplyButton.png\" onmouseout=\"this.src='image/ReplyButton.png'\""+
                        "onmouseover=\"this.src='image/ReplyButtonSelected.png'\" /></td></tr>";
                    messageDivsText += "<tr><td><div class=\"NavyLink12\" id=\"message" + i.ToString() + "\"></div></td></tr>";
                    messageDivsText += "</table></td></tr></table>";
                    //thedivsText += "</div>";

                    messageDiv.InnerHtml = messageDivsText;

                    ItemsPanel.Controls.Add(thediv);


                    //message.DATE = ds.Tables[0].Rows[i]["Date"].ToString();


                    //MessagePanel.Controls.Add(message);
                }

                if (ds.Tables[0].Rows.Count % 10 != 0 || ds.Tables[0].Rows.Count == 10)
                {
                    a.Add(ItemsPanel);
                }

            }

        
        Label label = new Label();

        string temp = "messages";
        if (ds.Tables[0].Rows.Count == 1)
            temp = "message";
        label.Text = "<table width=\"570px\"><tr><td><span style=\"font-family: Arial; font-size: 20px; color: White;\">My Messages</span>"
            + "<span style=\"font-family: Arial; font-size: 12px;   padding-left: 5px;\">(<span id=\"messagesCount\">" + unreadCount.ToString()
            + "</span> new " + temp + ")</span></td><td align=\"right\"><div class=\"Green12LinkNF\" id=\"globalMessage\"></div></td></tr></table>";

        Panel MessagesPanel = (Panel)Tab1.FindControl("MessagesPanel");

        MessagesPanel.Controls.Clear();
        MessagesPanel.Controls.Add(label);
        MessagesPanel.Controls.Add(pagerPanel);

        if (a.Count > 0)
        {
            pagerPanel.DATA = a;
            pagerPanel.DataBind2();
        }
    }

    protected DataView MergeDVThreeCol(DataView dv1, DataView dv2)
    {
        //even if dv1 is empty, we still want to use 
        //the method to convert the date column to datetime
        DataView dv;
        DataTable dt = new DataTable();
        DataColumn dc = new DataColumn();
        dc.ColumnName = "UserName";
        dc.DataType = typeof(String);
        dt.Columns.Add(dc);
        
        dc = new DataColumn();
        dc.ColumnName = "THE_DATE";
        dc.DataType = typeof(DateTime);
        dt.Columns.Add(dc);

        dc = new DataColumn();
        dc.ColumnName = "HEADER";
        dc.DataType = typeof(String);
        dt.Columns.Add(dc);

        dc = new DataColumn();
        dc.ColumnName = "ProfilePicture";
        dc.DataType = typeof(String);
        dt.Columns.Add(dc);

        DataRow row;
        if (dv1.Count != 0)
        {
            for (int i = 0; i < dv1.Count; i++)
            {
                row = dt.NewRow();
                row["UserName"] = dv1[i]["UserName"];
                row["THE_DATE"] = DateTime.Parse(dv1[i]["THE_DATE"].ToString());
                row["HEADER"] = dv1[i]["HEADER"];
                row["ProfilePicture"] = dv1[i]["ProfilePicture"];
                dt.Rows.Add(row);
            }
        }
       
        if(dv2.Count != 0)
        {
            for (int i = 0; i < dv2.Count; i++)
            {
                row = dt.NewRow();
                row["UserName"] = dv2[i]["UserName"];
                row["THE_DATE"] = DateTime.Parse(dv2[i]["THE_DATE"].ToString());
                row["HEADER"] = dv2[i]["HEADER"];
                row["ProfilePicture"] = dv2[i]["ProfilePicture"];
                dt.Rows.Add(row);
            }
        }

        DataView newDV = new DataView(dt, "", "", DataViewRowState.CurrentRows);
        newDV.Sort = "THE_DATE DESC";

        return newDV;
    }

    protected DataView MergeDVTwoCol(DataView dv1, DataView dv2)
    {
        //even if dv1 is empty, we still want to use 
        //the method to convert the date column to datetime
        DataView dv;
        DataTable dt = new DataTable();
        DataColumn dc = new DataColumn();

        dc.ColumnName = "TheDate";
        dc.DataType = typeof(DateTime);
        dt.Columns.Add(dc);

        dc = new DataColumn();
        dc.ColumnName = "ID";
        dt.Columns.Add(dc);

        DataRow row;
        if (dv1.Count != 0)
        {
            for (int i = 0; i < dv1.Count; i++)
            {
                row = dt.NewRow();
                row["TheDate"] = dv1[i]["TheDate"];
                row["ID"] = dv1[i]["ID"];
                dt.Rows.Add(row);
            }
        }

        if (dv2.Count != 0)
        {
            for (int i = 0; i < dv2.Count; i++)
            {
                row = dt.NewRow();
                row["TheDate"] = dv2[i]["TheDate"];
                row["ID"] = dv2[i]["ID"];
                dt.Rows.Add(row);
            }
        }

        DataView newDV = new DataView(dt, "", "", DataViewRowState.CurrentRows);

        return newDV;
    }

    protected void LoadFriends()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        ClearMessage();
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DateTime isNow = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
        DataSet dsFriends = dat.GetData("SELECT * FROM User_Friends UF, Users U WHERE UF.FriendID=U.User_ID AND UF.UserID=" + Session["User"].ToString());
        Panel FriendPanel = (Panel)Tab1.FindControl("FriendPanel");

        //Search for what your friends did in the past 30 days.
        string thisMonth = isNow.AddDays(-30).Month.ToString() + "/" + isNow.AddDays(-30).Day.ToString() + "/" +
            isNow.AddDays(-30).Year.ToString();
        Panel WhatMyFriendsDidPanel = (Panel)Tab2.FindControl("WhatMyFriendsDidPanel");
        DataView dvFriends = new DataView(dsFriends.Tables[0], "", "", DataViewRowState.CurrentRows);

        if (dvFriends.Count > 0)
        {
            //Added Events to calendar
            DataView dvEvents = dat.GetDataDV("SELECT TOP 10 U.UserName,  U.ProfilePicture, UC.DateAdded AS THE_DATE, 'Added the event <a class=NavyLink12 target=_blank href=' + dbo.MAKENICENAME(E.Header) + '_' + CONVERT(NVARCHAR,E.ID)+'_Event>'+  E.Header +'</a>' AS HEADER " +
                "FROM User_Friends UF, Users U, User_Calendar UC, Events E WHERE UF.UserID=" +
                Session["User"].ToString() + " AND U.User_ID=UF.FriendID AND E.ID=UC.EventID " +
                "AND UF.FriendID=UC.UserID AND UC.DateAdded > CONVERT(DATETIME, '" + thisMonth + "') ORDER BY THE_DATE DESC");
                //AND UC.DateAdded > CONVERT(DATETIME, '" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A",":")).AddDays(double.Parse("-10")).Date.ToString() + "')
            //Added Favorite Venues
            DataView dvVenues = dat.GetDataDV("SELECT TOP 10  U.UserName, U.ProfilePicture, UV.DateAdded AS THE_DATE, 'Added a favorite venue <a class=NavyLink12 target=_blank href='+dbo.MAKENICENAME(V.Name)+'_'+CONVERT(NVARCHAR,V.ID)+'_Venue>'+ V.Name +'</a>' AS HEADER " +
                "FROM User_Friends UF, Users U, UserVenues UV, Venues V WHERE UF.UserID=" +
                Session["User"].ToString() + " AND V.ID=UV.VenueID AND UV.DateAdded > CONVERT(DATETIME, '" + thisMonth + "') AND U.User_ID=UF.FriendID AND UF.FriendID=UV.UserID ORDER BY THE_DATE DESC");

            //AND UV.DateAdded > CONVERT(DATETIME, '" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(double.Parse("-10")).Date.ToString() + "') 

            //Added Event Comments
            DataView dvComments = dat.GetDataDV("SELECT TOP 10 U.UserName, U.ProfilePicture,  C.BlogDate AS THE_DATE, 'Posted a comment:  " +
                "<a class=Green12LinkNF target=_blank href=' + dbo.MAKENICENAME(E.Header) + '_' + CONVERT(NVARCHAR,E.ID)+'_Event>'+ C.Comment +'</a>' AS HEADER " +
                "FROM User_Friends UF, Users U, Comments C, Events E WHERE UF.UserID=" +
                Session["User"].ToString() + " AND C.BlogDate > CONVERT(DATETIME, '" + thisMonth + "') AND U.User_ID=UF.FriendID AND C.BlogID=E.ID " +
                "AND UF.FriendID=C.UserID  ORDER BY THE_DATE DESC");

            //AND C.BlogDate > CONVERT(DATETIME, '" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A",":")).AddDays(double.Parse("-10")).Date.ToString() + "')

            //Added Venue Comments
            DataView dvCommentsVenue = dat.GetDataDV("SELECT TOP 10  U.UserName, U.ProfilePicture,  VC.CommentDate AS THE_DATE, 'Posted a comment: " +
                "<a class=Green12LinkNF target=_blank href='+dbo.MAKENICENAME(V.Name)+'_'+CONVERT(NVARCHAR,V.ID)+'_Venue>'+ VC.Comment +'</a>' AS HEADER " +
                "FROM User_Friends UF, Users U, Venue_Comments VC, Venues V WHERE UF.UserID=" +
                Session["User"].ToString() + " AND VC.CommentDate > CONVERT(DATETIME, '" + thisMonth + "') AND U.User_ID=UF.FriendID AND V.ID=VC.VenueID " +
                "AND UF.FriendID=VC.UserID ORDER BY THE_DATE DESC");

             //AND VC.CommentDate > CONVERT(DATETIME, '" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A",":")).AddDays(double.Parse("-10")).Date.ToString() + "') 

            //Posted Events
            DataView dvPostedEvents = dat.GetDataDV("SELECT TOP 10  U.UserName, U.ProfilePicture, E.PostedOn AS THE_DATE, " +
                "'Posted the event <a class=AddOrangeLink target=_blank href=' + dbo.MAKENICENAME(E.Header) + '_' + CONVERT(NVARCHAR,E.ID)+'_Event>'+ E.Header +'</a>'AS HEADER " +
                "FROM User_Friends UF, Users U, Events E WHERE UF.UserID=" +
                Session["User"].ToString() + " AND E.PostedOn > CONVERT(DATETIME, '" + thisMonth + "') AND U.User_ID=UF.FriendID AND U.UserName=E.UserName  ORDER BY THE_DATE DESC");

            //"+"AND E.PostedOn > CONVERT(DATETIME, '" +" ").Replace("%3A", ":")).AddDays(double.Parse("-10")).Date.ToString() + "') 

            //Posted Venues
            DataView dvPostedVenues = dat.GetDataDV("SELECT TOP 10  U.UserName,  U.ProfilePicture, V.PostedOn AS THE_DATE, " +
                "'Posted the venue <a class=AddOrangeLink target=_blank href='+ dbo.MAKENICENAME(V.Name)+'_'+CONVERT(NVARCHAR,V.ID)+'_Venue>'+ V.Name +'</a>' AS HEADER " +
                "FROM User_Friends UF, Users U, Venues V WHERE UF.UserID=" +
                Session["User"].ToString() + " AND V.PostedOn > CONVERT(DATETIME, '" + thisMonth + "') AND U.User_ID=UF.FriendID AND UF.FriendID=V.CreatedByUser ORDER BY THE_DATE DESC");

            // AND V.PostedOn > CONVERT(DATETIME, '" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(double.Parse("-10")).Date.ToString() + "') 

            //Posted Ads
            DataView dvPostedAds = dat.GetDataDV("SELECT TOP 10  U.UserName,  U.ProfilePicture, CONVERT(DATETIME,A.DateAdded) AS THE_DATE, " +
                "'Posted the ad <a target=_blank class=AddOrangeLink href='+ dbo.MAKENICENAME(A.Header)+'_'+CONVERT(NVARCHAR,A.Ad_ID)+'_Ad>'+ A.Header +'</a>' AS HEADER " +
                "FROM User_Friends UF, Users U, Ads A WHERE UF.UserID=" +
                Session["User"].ToString() + " AND CONVERT(DATETIME,A.DateAdded) > CONVERT(DATETIME, '" + thisMonth + "') AND U.User_ID=UF.FriendID AND UF.FriendID=A.User_ID  ORDER BY THE_DATE DESC");

            //AND A.DateAdded > CONVERT(DATETIME, '" + DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddDays(double.Parse("-10")).Date.ToString() + "') 

            DataView dv = MergeDVThreeCol(MergeDVThreeCol(MergeDVThreeCol(MergeDVThreeCol(MergeDVThreeCol(MergeDVThreeCol(dvPostedEvents,
                dvPostedVenues), dvPostedAds), dvCommentsVenue), dvComments), dvVenues), dvEvents);

            string friendImg = "";
            string strFill = "";

            Telerik.Web.UI.RadRotatorItem rItem = new Telerik.Web.UI.RadRotatorItem();
            Literal hiddenPostingLiteral = new Literal();

            dv.Sort = "THE_DATE DESC";

            if (dv.Count != 0)
            {
                int count = 0;

                dv.Sort = "THE_DATE DESC";

                for (int i = 0; i < dv.Count;i++ )
                {
                    string rowHeader = dv[i]["HEADER"].ToString();

                    if (System.IO.File.Exists(Server.MapPath(".") + "\\UserFiles\\" + dv[i]["UserName"].ToString() +
                        "\\Profile\\" + dv[i]["ProfilePicture"].ToString()))
                    {
                        friendImg = "UserFiles/" + dv[i]["UserName"].ToString() + "/Profile/" + dv[i]["ProfilePicture"].ToString();
                        strFill = "";
                    }
                    else
                    {
                        friendImg = "NewImages/noAvatar_50x50_small.png";
                        strFill = "onmouseover=\"this.src='NewImages/noAvatar_50x50_smallhover.png'\"" +
                            "onmouseout=\"this.src='NewImages/noAvatar_50x50_small.png'\" ";
                    }

                        PostingLiteral.Text += "<div align=\"left\" style=\"margin-right: 10px;float: left;width: 230px; line-height: 12px; min-height: 54px;padding: 10px; margin-bottom: 20px;\">" +
                            "<a href=\"" + dv[i]["UserName"].ToString() + "_Friend\"><img " + strFill + " style=\"border: solid 1px #dedbdb;background-color: #dedbdb; float: left;margin-right: 7px; margin-bottom: 2px;\" " +
                            "src=\"" + friendImg + "\" width=\"50px\" height=\"50px\" /></a>";
                        PostingLiteral.Text += "<span class=\"NavyLink\" style=\" font-weight: bold;\"><a class=\"NavyLink12\" href=\"" + dv[i]["UserName"].ToString() + "_Friend\">" +
                            dv[i]["UserName"].ToString() + "</a></span> " + rowHeader + " on " + DateTime.Parse(dv[i]["THE_DATE"].ToString()).ToShortDateString() + "</div>";
                    count++;
                }
            }
            else
            {
                //PostingLiteral.Text += "<div  style=\"padding-top: 10px;padding-left:0; padding-right: 10px;\">Your friends haven't been up to anything. Tell them that they could win the Hippo Points badge for the month and promote themselves on our home page just by posting content on the Hippo! <a class=\"NavyLink\" href=\"hippo-points\">Read more about Hippo Points.</a></div>";
                PostingLiteral.Text += "<div  style=\"padding-top: 10px;padding-left:0; padding-right: 10px;\">Hey, your friends haven't been up to anything lately. Sad face!</div>";

            }

            //dvEvents = MergeDVThreeCol(dvEvents, dvVenues);

            //if (dvEvents.Count != 0)
            //{
            //    rItem = new Telerik.Web.UI.RadRotatorItem();
            //    hiddenPostingLiteral = new Literal();

            //    int count = 0;

            //    dvEvents.Sort = "THE_DATE DESC";

            //    foreach (DataRowView row in dvEvents)
            //    {
            //        string rowHeader = row["HEADER"].ToString();

            //        if (System.IO.File.Exists(Server.MapPath(".") + "\\UserFiles\\" + row["UserName"].ToString() +
            //            "\\Profile\\" + row["ProfilePicture"].ToString()))
            //        {
            //            friendImg = "UserFiles/" + row["UserName"].ToString() + "/Profile/" + row["ProfilePicture"].ToString();
            //            strFill = "";
            //        }
            //        else
            //        {
            //            friendImg = "NewImages/noAvatar_50x50_small.png";
            //            strFill = "onmouseover=\"this.src='NewImages/noAvatar_50x50_smallhover.png'\"" +
            //                "onmouseout=\"this.src='NewImages/noAvatar_50x50_small.png'\" ";
            //        }

            //            PostingLiteral.Text += "<div style=\"line-height: 12px; min-height: 54px;\">" +
            //                "<a href=\"" + row["UserName"].ToString() + "_Friend\"><img " + strFill + " style=\"background-color: #dedbdb;border: 0;float: left;margin-right: 7px; margin-bottom: 2px;\" " +
            //                "src=\"" + friendImg + "\" width=\"50px\" height=\"50px\" /></a>";
            //            PostingLiteral.Text += "<span style=\" font-weight: bold;\"><a class=\"NavyLink12\" href=\"" + row["UserName"].ToString() + "_Friend\">" +
            //                row["UserName"].ToString() + "</a></span> " + rowHeader + " on " + DateTime.Parse(row["THE_DATE"].ToString()).ToShortDateString() + "</div>";
            //    }
            //}
            //else
            //{
            //    PostingLiteral.Text += "<div  style=\"padding-top: 10px;padding-left:0; \">Your friends haven't had any Event Going Action in 10 days.</div>";
            //}

            //DataView dvCommentsFinal = MergeDVThreeCol(dvComments, dvCommentsVenue);

            //if (dvCommentsFinal.Count != 0)
            //{
            //    int count = 0;
            //    dvCommentsFinal.Sort = "THE_DATE DESC";
            //    foreach (DataRowView row in dvCommentsFinal)
            //    {
            //        string rowHeader = row["HEADER"].ToString();

            //        if (System.IO.File.Exists(Server.MapPath(".") + "\\UserFiles\\" + row["UserName"].ToString() +
            //            "\\Profile\\" + row["ProfilePicture"].ToString()))
            //        {
            //            friendImg = "UserFiles/" + row["UserName"].ToString() + "/Profile/" + row["ProfilePicture"].ToString();
            //            strFill = "";
            //        }
            //        else
            //        {
            //            friendImg = "NewImages/noAvatar_50x50_small.png";
            //            strFill = "onmouseover=\"this.src='NewImages/noAvatar_50x50_smallhover.png'\"" +
            //                "onmouseout=\"this.src='NewImages/noAvatar_50x50_small.png'\" ";
            //        }

            //            PostingLiteral.Text += "<div style=\"line-height: 12px;  min-height: 54px;\">" +
            //                "<a href=\"" + row["UserName"].ToString() + "_Friend\"><img " + strFill + " style=\"background-color: #dedbdb;border: 0;float: left;margin-right: 7px; margin-bottom: 2px;\" " +
            //                "src=\"" + friendImg + "\" width=\"50px\" height=\"50px\" /></a>";
            //            PostingLiteral.Text += "<span style=\"color: #628e02; font-weight: bold;\"></span> " + rowHeader + " on " + DateTime.Parse(row["THE_DATE"].ToString()).ToShortDateString() + "</div>";
            //    }
            //}
            //else
            //{
            //    PostingLiteral.Text += "<div  style=\"padding-top: 10px;padding-left:0; \">Your friends haven't had any Commenting Action in 10 days.</div>";
            //}
        }
        else
        {
            PostingLiteral.Text += "<div  style=\"padding-top: 10px;padding-left:0; padding-right: 10px;\">Your friends haven't been up to anything. Tell them that they could win the Hippo Points badge for the month and promote themselves on our home page just by posting content on the Hippo! <a class=\"NavyLink\" href=\"hippo-points\">Read more about Hippo Points.</a></div>";
        }

        DataView dvUser = dat.GetDataDV("SELECT * FROM UserPreferences WHERE UserID=" + Session["User"].ToString());
        string emailPrefs = dvUser[0]["EmailPrefs"].ToString();
        dvUser = dat.GetDataDV("SELECT * FROM UserFriendPrefs WHERE UserID=" + Session["User"].ToString());
        string emailFriendPrefs = "";
        int friendcount = 0; 
        if (dsFriends.Tables.Count > 0)
            if (dsFriends.Tables[0].Rows.Count > 0)
            {
                friendcount = dsFriends.Tables[0].Rows.Count;
                for (int i = 0; i < dsFriends.Tables[0].Rows.Count; i++)
                {
                    dvUser.RowFilter = "FriendID = " + dsFriends.Tables[0].Rows[i]["FriendID"].ToString();
                    if (dvUser.Count > 0)
                        emailFriendPrefs = dvUser[0]["Preferences"].ToString();
                    else
                        emailFriendPrefs = "";

                    Literal lit = new Literal();
                    lit.Text = "<div style=\"float: left; padding: 8px;\"><table align=\"center\" valign=\"middle\" cellpadding=\"0\" cellspacing=\"0\"  bgcolor=\"#dedbdb\" width=\"52\" style=\"border: solid 1px #DEDBDB\" height=\"52\"><tr><td align=\"center\">";
                    ImageButton profilePicture = new ImageButton();
                    
                    profilePicture.AlternateText = dsFriends.Tables[0].Rows[i]["UserName"].ToString();
                    profilePicture.ToolTip = dsFriends.Tables[0].Rows[i]["UserName"].ToString();
                    profilePicture.Height = 50;
                    profilePicture.Width = 50;
                    profilePicture.ID = "pic" + i.ToString();
                    profilePicture.AlternateText = dsFriends.Tables[0].Rows[i]["UserName"].ToString();
                    profilePicture.CommandArgument = dsFriends.Tables[0].Rows[i]["FriendID"].ToString();
                    if (System.IO.File.Exists(Server.MapPath(".") + "\\UserFiles\\" + dsFriends.Tables[0].Rows[i]["UserName"].ToString() + "\\Profile\\" + dsFriends.Tables[0].Rows[i]["ProfilePicture"].ToString()))
                    {
                        profilePicture.ImageUrl = "~/UserFiles/" + dsFriends.Tables[0].Rows[i]["UserName"].ToString() + "/Profile/" + dsFriends.Tables[0].Rows[i]["ProfilePicture"].ToString();
                        System.Drawing.Image theimg = System.Drawing.Image.FromFile(Server.MapPath(".") + "/UserFiles/" + dsFriends.Tables[0].Rows[i]["UserName"].ToString() +
                     "/Profile/" + dsFriends.Tables[0].Rows[i]["ProfilePicture"].ToString());

                        double width = double.Parse(theimg.Width.ToString());
                        double height = double.Parse(theimg.Height.ToString());

                        if (width > height)
                        {
                            if (width <= 50)
                            {

                            }
                            else
                            {
                                double dividor = double.Parse("50.00") / double.Parse(width.ToString());
                                width = double.Parse("50.00");
                                height = height * dividor;
                            }
                        }
                        else
                        {
                            if (width == height)
                            {
                                width = double.Parse("50.00");
                                height = double.Parse("50.00");
                            }
                            else
                            {
                                double dividor = double.Parse("50.00") / double.Parse(height.ToString());
                                height = double.Parse("50.00");
                                width = width * dividor;
                            }
                        }

                        profilePicture.Width = int.Parse((Math.Round(decimal.Parse(width.ToString()))).ToString());
                        profilePicture.Height = int.Parse((Math.Round(decimal.Parse(height.ToString()))).ToString());
                    
                    }
                    else
                    {
                        profilePicture.ImageUrl = "~/NewImages/noAvatar_50x50_small.png";
                        profilePicture.Attributes.Add("onmouseover", "this.src='NewImages/noAvatar_50x50_smallhover.png'");
                        profilePicture.Attributes.Add("onmouseout", "this.src='NewImages/noAvatar_50x50_small.png'");
                    }
                    profilePicture.PostBackUrl = "~/" + dsFriends.Tables[0].Rows[i]["UserName"].ToString() + "_Friend";
                    //profilePicture.Click += new ImageClickEventHandler(ViewFriend);

                    MyFriendsPanel.Controls.Add(lit);
                    MyFriendsPanel.Controls.Add(profilePicture);
                    lit = new Literal();
                    lit.Text = "</td></tr></table><div align=\"center\">";
                    MyFriendsPanel.Controls.Add(lit);

                    HyperLink link = new HyperLink();
                    link.Text = "edit prefs";
                    link.CssClass = "PrefsLink";
                    link.ID = "editPrefs" + dsFriends.Tables[0].Rows[i]["UserName"].ToString();

                    Telerik.Web.UI.RadToolTip tip = new Telerik.Web.UI.RadToolTip();
                    tip.TargetControlID = "editPrefs" + dsFriends.Tables[0].Rows[i]["UserName"].ToString();
                    tip.ShowEvent = Telerik.Web.UI.ToolTipShowEvent.OnClick;
                    tip.Position = Telerik.Web.UI.ToolTipPosition.MiddleRight;
                    tip.RelativeTo = Telerik.Web.UI.ToolTipRelativeDisplay.Element;
                    tip.ManualClose = true;
                    tip.Attributes.Add("style", "z-index: 1000;");
                    tip.Skin = "Sunset";
                    tip.ID = "Tooltip" + dsFriends.Tables[0].Rows[i]["FriendID"].ToString();

                    UpdatePanel upP = new UpdatePanel();
                    upP.ID = "UpdatePanel" + dsFriends.Tables[0].Rows[i]["UserName"].ToString();
                    upP.UpdateMode = UpdatePanelUpdateMode.Conditional;

                    Literal tipLit = new Literal();
                    tipLit.Text = "<div align=\"center\" style=\"width: 300px !important; height: 254px !important;\">";
                    tipLit.Text += "<table width=\"100%\" cellspacing=\"0\" align=\"center\" style=\"font-family: "+
                        "Arial; font-size: 12px;   padding: 10px;\">";
                    tipLit.Text += "<tr><td  style=\"padding-bottom: 5px;\"> <span style=\"  " +
                        "font-size: 11px; font-weight: bold;\">Friend Options:</span></td></tr>";
                    tipLit.Text += "<tr><td align=\"center\" style=\"padding-bottom: 10px;\">" +
                        "<a class=\"NavyLink12\" onclick=\"OpenRemove('" +
                        dsFriends.Tables[0].Rows[i]["FriendID"].ToString() +
                        "')\">Remove Friend</a></td></tr>";
                    tipLit.Text += "<tr><td><span style=\"  " +
                        "font-size: 11px; font-weight: bold;\">Notify me when friend:</span></td></tr>";
                    upP.ContentTemplateContainer.Controls.Add(tipLit);
                    //tipLit.Text += "<tr><td align=\"center\" style=\"padding-top: 5px;\"><h2>adds an event to calendar</h2></td></tr>";
                    //tipLit.Text += "<tr><td align=\"center\">";

                    //upP.ContentTemplateContainer.Controls.Add(tipLit);
                    ImageButton imgB;
                    CheckBox check;
                    //if (emailPrefs.Contains("6"))
                    //{
                    //    tipLit = new Literal();
                    //    tipLit.Text = "<span style=\"color: #33a923;\">This preference is turned on for everyone.  If you want to set it just for this user, turn it off for everyone in 'Email Prefs'.</span></td></tr>";

                    //    upP.ContentTemplateContainer.Controls.Add(tipLit);
                    //}
                    //else
                    //{
                    //    tipLit = new Literal();
                    //    tipLit.Text = "<div style=\"width: 118px;\" class=\"topDiv\"><div>";

                    //    upP.ContentTemplateContainer.Controls.Add(tipLit);

                    //    check = new CheckBox();
                    //    check.ID = "email" + dsFriends.Tables[0].Rows[i]["FriendID"].ToString();
                    //    check.Text = "email";
                    //    if (emailFriendPrefs.Contains("0"))
                    //        check.Checked = true;

                    //    upP.ContentTemplateContainer.Controls.Add(check);

                    //    tipLit = new Literal();
                    //    tipLit.Text = "</div></div></td></tr>";
                    //    //    "<div style=\"float: left;\">";

                    //    //upP.ContentTemplateContainer.Controls.Add(tipLit);

                    //    //check = new CheckBox();
                    //    //check.ID = "text" + dsFriends.Tables[0].Rows[i]["FriendID"].ToString();
                    //    //check.Text = "text";
                    //    //if (emailFriendPrefs.Contains("1"))
                    //    //    check.Checked = true;

                    //    //upP.ContentTemplateContainer.Controls.Add(check);

                    //    //tipLit = new Literal();
                    //    //tipLit.Text = "</div>"
                            

                    //    upP.ContentTemplateContainer.Controls.Add(tipLit);
                    //}

                    //posts an event
                    tipLit = new Literal();
                    tipLit.Text = "<tr><td align=\"center\" style=\"padding-top: 5px;\"><h2>posts event/locale/adventure</h2></td></tr>";
                    if (emailPrefs.Contains("3"))
                    {
                        tipLit.Text += "<tr><td align=\"center\"><span style=\"color: #33a923;\">This preference is turned on for everyone.  If you want to set it just for this user, turn it off for everyone in 'Email Prefs'.</span></td></tr>";

                        upP.ContentTemplateContainer.Controls.Add(tipLit);
                    }
                    else
                    {
                        tipLit.Text += "<tr><td align=\"center\"><div style=\"width: 125px;\" class=\"topDiv\"><div>";

                        upP.ContentTemplateContainer.Controls.Add(tipLit);

                        check = new CheckBox();
                        check.ID = "email2" + dsFriends.Tables[0].Rows[i]["FriendID"].ToString();
                        check.Text = "email";
                        if (emailFriendPrefs.Contains("2"))
                            check.Checked = true;

                        upP.ContentTemplateContainer.Controls.Add(check);

                        tipLit = new Literal();
                        tipLit.Text = "</div></td></tr>";
                            
                        //    "<div style=\"padding-right: 8px;float: left;\">";

                        //upP.ContentTemplateContainer.Controls.Add(tipLit);

                        //check = new CheckBox();
                        //check.ID = "text2" + dsFriends.Tables[0].Rows[i]["FriendID"].ToString();
                        //check.Text = "text";
                        //if (emailFriendPrefs.Contains("3"))
                        //    check.Checked = true;

                        //upP.ContentTemplateContainer.Controls.Add(check);

                        //tipLit = new Literal();
                        //tipLit.Text = "</div>"
                            
                        upP.ContentTemplateContainer.Controls.Add(tipLit);
                    }

                    ////posts an ad
                    //tipLit = new Literal();
                    //tipLit.Text = "<tr><td align=\"center\" style=\"padding-top: 5px;\"><h2>posts an ad</h2></td></tr>";
                    //tipLit.Text += "<tr><td align=\"center\"><div style=\"width: 125px;\" class=\"topDiv\"><div style=\"float: left;padding-right: 8px;\">";

                    //upP.ContentTemplateContainer.Controls.Add(tipLit);

                    //check = new CheckBox();
                    //check.ID = "email3" + dsFriends.Tables[0].Rows[i]["FriendID"].ToString();
                    //check.Text = "email";
                    //if (emailFriendPrefs.Contains("4"))
                    //    check.Checked = true;

                    //upP.ContentTemplateContainer.Controls.Add(check);

                    //tipLit = new Literal();
                    //tipLit.Text = "</div><div style=\"padding-right: 8px;float: left;\">";

                    //upP.ContentTemplateContainer.Controls.Add(tipLit);

                    //check = new CheckBox();
                    //check.ID = "text3" + dsFriends.Tables[0].Rows[i]["FriendID"].ToString();
                    //check.Text = "text";
                    //if (emailFriendPrefs.Contains("5"))
                    //    check.Checked = true;

                    //upP.ContentTemplateContainer.Controls.Add(check);

                    //tipLit = new Literal();
                    //tipLit.Text = "</div></td></tr>";

                    //upP.ContentTemplateContainer.Controls.Add(tipLit);


                    //sends a Hippo Mail to you
                    tipLit = new Literal();
                    tipLit.Text = "<tr><td align=\"center\" style=\"padding-top: 5px;\"><h2>sends a Hippo Mail to you</h2></td></tr>";
                    tipLit.Text += "<tr><td align=\"center\">";

                    upP.ContentTemplateContainer.Controls.Add(tipLit);

                    if (emailPrefs.Contains("4"))
                    {
                        tipLit = new Literal();
                        tipLit.Text = "<span style=\"color: #33a923;\">This preference is on for everyone.  If you want to set it just for this user, turn it off for everyone in 'Email Prefs'.</span></td></tr>";

                        upP.ContentTemplateContainer.Controls.Add(tipLit);
                    }
                    else
                    {
                        tipLit = new Literal();
                        tipLit.Text = "<div style=\"width: 125px;\" class=\"topDiv\"><div>";
                        upP.ContentTemplateContainer.Controls.Add(tipLit);

                        check = new CheckBox();
                        check.ID = "email4" + dsFriends.Tables[0].Rows[i]["FriendID"].ToString();
                        check.Text = "email";
                        if (emailFriendPrefs.Contains("6"))
                            check.Checked = true;

                        upP.ContentTemplateContainer.Controls.Add(check);

                        tipLit = new Literal();
                        tipLit.Text = "</div></td></tr>";
                            
                        //    "<div style=\"padding-right: 8px;float: left;\">";

                        //upP.ContentTemplateContainer.Controls.Add(tipLit);

                        //check = new CheckBox();
                        //check.ID = "text4" + dsFriends.Tables[0].Rows[i]["FriendID"].ToString();
                        //check.Text = "text";
                        //if (emailFriendPrefs.Contains("7"))
                        //    check.Checked = true;

                        //upP.ContentTemplateContainer.Controls.Add(check);

                        //tipLit = new Literal();
                        //tipLit.Text = "</div>"
                            
                        upP.ContentTemplateContainer.Controls.Add(tipLit);
                    }

                    //shares event/venue/ad with you
                    tipLit = new Literal();
                    tipLit.Text = "<tr><td align=\"center\" style=\"padding-top: 5px;\"><h2>shares event/locale/adventure with you</h2></td></tr>";
                    tipLit.Text += "<tr><td align=\"center\">";

                    upP.ContentTemplateContainer.Controls.Add(tipLit);

                    if (emailPrefs.Contains("9"))
                    {
                        tipLit = new Literal();
                        tipLit.Text = "<span style=\"color: #33a923;\">This preference is turned on for everyone.  If you want to set it just for this user, turn it off for everyone in 'Email Prefs'.</span></td></tr>";

                        upP.ContentTemplateContainer.Controls.Add(tipLit);
                    }
                    else
                    {
                        tipLit = new Literal();
                        tipLit.Text = "<div style=\"width: 125px;\" class=\"topDiv\"><div>";
                        upP.ContentTemplateContainer.Controls.Add(tipLit);

                        check = new CheckBox();
                        check.ID = "email5" + dsFriends.Tables[0].Rows[i]["FriendID"].ToString();
                        check.Text = "email";
                        if (emailFriendPrefs.Contains("8"))
                            check.Checked = true;

                        upP.ContentTemplateContainer.Controls.Add(check);

                        tipLit = new Literal();
                        tipLit.Text = "</div></td></tr>";
                            
                        //"<div style=\"padding-right: 8px;float: left;\">";

                        //upP.ContentTemplateContainer.Controls.Add(tipLit);

                        //check = new CheckBox();
                        //check.Text = "text";
                        //check.ID = "text5" + dsFriends.Tables[0].Rows[i]["FriendID"].ToString();
                        //if (emailFriendPrefs.Contains("9"))
                        //    check.Checked = true;

                        //upP.ContentTemplateContainer.Controls.Add(check);

                        //tipLit = new Literal();
                        //tipLit.Text = "</div>"

                        upP.ContentTemplateContainer.Controls.Add(tipLit);

                    }
                    tipLit = new Literal();
                    tipLit.Text = "</table><div style=\"color: red;float: left;padding-top: 10px;\">";
                    upP.ContentTemplateContainer.Controls.Add(tipLit);

                    Label labl = new Label();
                    labl.ID = "ErrorLabel" + dsFriends.Tables[0].Rows[i]["FriendID"].ToString();

                    upP.ContentTemplateContainer.Controls.Add(labl);

                    tipLit = new Literal();
                    tipLit.Text = "</div><div style=\"float: right;padding-top: 10px;\">";

                    upP.ContentTemplateContainer.Controls.Add(tipLit);

                    ASP.controls_bluebutton_ascx btn = new ASP.controls_bluebutton_ascx();
                    btn.BUTTON_TEXT = "Save";
                    btn.COMMAND_ARGS = dsFriends.Tables[0].Rows[i]["FriendID"].ToString();
                    btn.CLIENT_CLICK = "javascript:CloseToolTip();";
                    btn.SERVER_CLICK += PrefsSave;
                    btn.ID = "Save" + dsFriends.Tables[0].Rows[i]["FriendID"].ToString();
                    btn.WIDTH = "55px";

                    upP.ContentTemplateContainer.Controls.Add(btn);

                    tipLit = new Literal();
                    tipLit.Text = "</div></div>";

                    upP.ContentTemplateContainer.Controls.Add(tipLit);

                    tip.Controls.Add(upP);

                    MyFriendsPanel.Controls.Add(link);
                    MyFriendsPanel.Controls.Add(tip);

                    lit = new Literal();
                    lit.Text = "</div></div>";
                    MyFriendsPanel.Controls.Add(lit); 
                }
            }

        NumFriendsLabel.Text = friendcount.ToString();
        LinkButton friendLink = new LinkButton();
        friendLink.Text = "Add Friends";
    }

    protected void PrefsSave(object sender, EventArgs e)
    {
        
            HttpCookie cookie = Request.Cookies["BrowserDate"];
            string message = "";
            if (cookie == null)
            {
                cookie = new HttpCookie("BrowserDate");
                cookie.Value = DateTime.Now.ToString();
                cookie.Expires = DateTime.Now.AddDays(22);
                Response.Cookies.Add(cookie);
            }
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            string prefs = "";

            LinkButton saveB = (LinkButton)sender;
            string friend = saveB.CommandArgument;

            Label label = (Label)dat.FindControlRecursive(this, "ErrorLabel" + friend);

            try
            {
                CheckBox imbE = (CheckBox)dat.FindControlRecursive(this, "email" + friend);

                if (imbE != null)
                {
                    if (imbE.Checked)
                        prefs += "0";

                }

                imbE = (CheckBox)dat.FindControlRecursive(this, "email2" + friend);

                if (imbE != null)
                {
                    if (imbE.Checked)
                        prefs += "2";
                }

                imbE = (CheckBox)dat.FindControlRecursive(this, "email3" + friend);

                if (imbE != null)
                {
                    if (imbE.Checked)
                        prefs += "4";
                }

                imbE = (CheckBox)dat.FindControlRecursive(this, "email4" + friend);

                if (imbE != null)
                {
                    if (imbE.Checked)
                        prefs += "6";
                }

                imbE = (CheckBox)dat.FindControlRecursive(this, "email5" + friend);

                if (imbE != null)
                {
                    if (imbE.Checked)
                        prefs += "8";
                }

                DataView dvF = dat.GetDataDV("SELECT * FROM UserFriendPrefs WHERE FriendID=" + friend);
               
                if (dvF.Count > 0)
                {
                    dat.Execute("UPDATE UserFriendPrefs SET Preferences = '" + prefs + "' WHERE UserID=" +
                        Session["User"].ToString() + " AND FriendID=" + friend);
                }
                else
                {
                    dat.Execute("INSERT INTO UserFriendPrefs (Preferences, UserID, FriendID) VALUES('" + prefs + "', " +
                        Session["User"].ToString() + ", " + friend + ")");
                }
            }
            catch (Exception ex)
            {
                label.Text += message+"<br/><br/>"+ex.ToString();
            }

    }

    protected void ChangeCheckImage(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        ImageButton CheckImageButton = (ImageButton)sender;

        if (CheckImageButton.ImageUrl == "image/Check.png")
            CheckImageButton.ImageUrl = "image/CheckSelected.png";
        else
            CheckImageButton.ImageUrl = "image/Check.png";
    }

    //protected void MarkAsRead_OLD(object sender, Telerik.Web.UI.RadPanelBarEventArgs e)
    //{
    //    if (e.Item.Attributes["CommandArgument"] != null)
    //    {
    //        bool read = bool.Parse(e.Item.Attributes["Read"]);

    //        if (!read)
    //        {
    //            Telerik.Web.UI.RadPanelItem rItem = (Telerik.Web.UI.RadPanelItem)e.Item;
    //            int key = int.Parse(e.Item.Attributes["CommandArgument"]);
    //            rItem.Text = rItem.Text.Replace("font-weight:bold;", "");
    //            rItem.Text = rItem.Text.Replace("font-weight: bold;", "");
    //            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

    //            dat.Execute("UPDATE UserMessages SET [Read] = 'True' WHERE ID=" + key);

    //            //LoadControls();
    //        }
    //    }
    //}

    //[Ajax.AjaxMethod]
    //public static string MarkAsRead(string messageID)
    //{
    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));       
    //    dat.Execute("UPDATE UserMessages SET [Read] = 'True' WHERE ID=" + messageID);

    //    return "strindsfdffg";
    //}

    protected void MarkAsRead2(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        HtmlGenericControl theSender = (HtmlGenericControl)sender;
        string messageID = theSender.ID.Replace("header", "");
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        dat.Execute("UPDATE UserMessages SET [Read] = 'True' WHERE ID=" + messageID);

    }

    protected void OpenSearchFriends(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        Panel SearchFriendPanel = (Panel)dat.FindControlRecursive(this, "SearchFriendPanel");
        SearchFriendPanel.Visible = true;
    }
    
    protected void CancelFriendSearch(object sender, EventArgs e)
    {
        CloseSearchPanel();
    }
    
    protected void CloseSearchPanel()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        Panel SearchFriendPanel = (Panel)dat.FindControlRecursive(this, "SearchFriendPanel");
        SearchFriendPanel.Visible = false;
    }
    
    protected void FriendSearch(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        ClearMessage();
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        TextBox FriendSearchTextBox = (TextBox)dat.FindControlRecursive(this, "FriendSearchTextBox");
        
        Label FriendMessageLabel = (Label)dat.FindControlRecursive(this, "FriendMessageLabel");


        if (FriendSearchTextBox.Text != "" && dat.TrapKey(FriendSearchTextBox.Text, 1))
        {
            DataSet ds = dat.GetData("SELECT * FROM Users WHERE UserName LIKE '%" + FriendSearchTextBox.Text + "%'");

            if (ds.Tables.Count > 0)
                if (ds.Tables[0].Rows.Count > 0)
                {
                    FillSearchPanel(ds);
                    ViewState["FriendDS"] = ds;
                }
                else
                    FriendMessageLabel.Text = "0 Results Found.";
            else
                FriendMessageLabel.Text = "0 Results Found.";
        }
        else
            FriendMessageLabel.Text = "Include a valid User Name in the text field.";
    }
    
    protected void FillSearchPanel(DataSet ds)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        Panel SearchResultsPanel = (Panel)dat.FindControlRecursive(this, "SearchResultsPanel");
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            Image image = new Image();
            image.Width = 50;
            image.Height = 50;

            if (System.IO.File.Exists(Server.MapPath(".") + "\\UserFiles\\" + ds.Tables[0].Rows[i]["UserName"].ToString() + "\\Profile\\" + ds.Tables[0].Rows[i]["ProfilePicture"].ToString()))
            {
                image.ImageUrl = "~/UserFiles/" + ds.Tables[0].Rows[i]["UserName"].ToString() + "/Profile/" + ds.Tables[0].Rows[i]["ProfilePicture"].ToString();
            }
            else
                image.ImageUrl = "~/NewImages/noAvatar_50x50_small.png";

            Label label = new Label();
            label.Text = ds.Tables[0].Rows[i]["UserName"].ToString();

            LinkButton link = new LinkButton();
            link.Text = "Add Friend";
            link.CssClass = "NavyLink12";
            link.CausesValidation = false;
            link.ID = "link" + i.ToString();
            link.CommandArgument = ds.Tables[0].Rows[i]["User_ID"].ToString();
            link.Click += new EventHandler(this.AddFriend);


            SearchResultsPanel.Controls.Add(image);
            SearchResultsPanel.Controls.Add(label);
            SearchResultsPanel.Controls.Add(link);
        }
    }
    
    protected void FillVenues()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        DataSet ds = dat.GetData("SELECT *, UV.VenueID AS VID  FROM UserVenues UV, Venues V "+
            "WHERE V.ID=UV.VenueID AND UV.UserID="+
            Session["User"].ToString() + " ORDER BY V.Name");

        bool noneExist = false;

        if (ds.Tables.Count > 0)
            if (ds.Tables[0].Rows.Count > 0)
            {
                ASP.controls_pager_ascx pagerPanel = new ASP.controls_pager_ascx();
                pagerPanel.NUMBER_OF_ITEMS_PER_PAGE = 5;
                pagerPanel.PANEL_CSSCLASS = "FavoritesPanel";
                pagerPanel.WIDTH = 260;
                ArrayList a = new ArrayList(ds.Tables[0].Rows.Count * 2);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Literal lit = new Literal();
                    lit.Text = "<div class=\"topDiv\"><div class=\"topDiv\" style=\"padding-top: 20px;\">";

                    Literal lit2 = new Literal();
                    lit2.Text = "</div>";

                    ASP.controls_hipporating_ascx rating = new ASP.controls_hipporating_ascx();
                    rating.VENUE_ID = ds.Tables[0].Rows[i]["ID"].ToString();

                    Label label = new Label();
                    label.Text = "<div style=\"float:left; padding-top: 4px;\"><a class=\"NavyLink12\" href=\"" + 
                        dat.MakeNiceName(ds.Tables[0].Rows[i]["Name"].ToString()) +'_'+ ds.Tables[0].Rows[i]["ID"].ToString() + "_Venue\">" + ds.Tables[0].Rows[i]["Name"].ToString() + "</a></div>";

                    DataSet dsEvents = dat.GetData("SELECT * FROM Events E, Event_Occurance EO WHERE EO.EventID=E.ID AND "+
                        "DAY(EO.DateTimeStart) = DAY(GETDATE()) AND MONTH(EO.DateTimeStart) = MONTH(GETDATE()) AND YEAR"+
                        "(EO.DateTimeStart) = YEAR(GETDATE()) "
                      +   " AND E.Venue=" + ds.Tables[0].Rows[i]["VID"].ToString());
                    int count = 0;

                    if (dsEvents.Tables.Count > 0)
                        if (dsEvents.Tables[0].Rows.Count > 0)
                            count = dsEvents.Tables[0].Rows.Count;

                    string theColor = "";
                    if (count > 0)
                        theColor = "color: #ff6b09;";


                    Label label2 = new Label();
                    label2.Text = "<div style=\"display: block; padding-top: 3px;" + theColor +
                        "\">" + count + " event[s] going on today </div></div>";

                    Panel allP = new Panel();
                    allP.Controls.Add(lit);
                    allP.Controls.Add(label);
                    allP.Controls.Add(rating);
                    allP.Controls.Add(lit2);
                    allP.Controls.Add(label2);
                    
                    a.Add(allP);
                }

                
                Literal newLit = new Literal();
                newLit.Text = "<div class=\"topDiv\">";
                FavoriteVenues.Controls.Add(newLit);
                FavoriteVenues.Controls.Add(pagerPanel);

                pagerPanel.PANEL_NAME = pagerPanel.ClientID + "_Panel";
                pagerPanel.DATA = a;
                pagerPanel.DataBind2();

                newLit = new Literal();
                newLit.Text = "</div>";
                FavoriteVenues.Controls.Add(newLit);
            }
            else
                noneExist = true;
        else
            noneExist = true;

        if (noneExist)
        {
            Label label = new Label();
            label.Text = "You have not added any locales to your favorite's list. To search for some awesome locales and make them your favorite visit the <a class=\"Green12LinkNF\" href=\"venue-search\">locale's page.</a>";
            FavoriteVenues.Controls.Add(label);
        }
    }
    
    protected void FillRecommendedEvents()
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        //only show top 10
        int topCount = 10;

        //Put the sets together
        DataSet dsAll = dat.RetrieveRecommendedEvents(topCount, true);

        bool noneMessage = false;

        if (dsAll.Tables.Count > 0)
            if (dsAll.Tables[0].Rows.Count > 0)
            {
                //insert link to all recommended events
                Session["RecomDS"] = dsAll;
                Literal link = new Literal();
                link.Text = "<h4 style=\"margin-bottom: 5px;\">Top 10 Recommended Events</h4><div><a class=\"NavyLink12\" onclick=\"OpenRadRecom();\">See All</a></div>";

                RecommendedEvents.Controls.Add(link);

                Hashtable hash = new Hashtable();

                ASP.controls_pager_ascx pagerPanel = new ASP.controls_pager_ascx();
                pagerPanel.PANEL_NAME = "ctl00_ContentPlaceHolder1_ctl13_Panel";
                pagerPanel.NUMBER_OF_ITEMS_PER_PAGE = 3;
                pagerPanel.PANEL_CSSCLASS = "FavoritesPanel";
                pagerPanel.WIDTH = 260;
                ArrayList a = new ArrayList(topCount);

                for (int i = 0; i < topCount; i++)
                {
                    dat.InsertOneEvent(dsAll, i, ref a, false);
                }

                
                RecommendedEvents.Controls.Add(pagerPanel);
                pagerPanel.DATA = a;
                pagerPanel.DataBind2();
            }
            else
            {
                noneMessage = true;
            }
        else
            noneMessage = true;

        if (noneMessage)
        {
            Label lab = new Label();
            lab.Text = "There are no recommended events here this month. There are a few reasons for this. "+
                "There simply is no events that fit your recommendation criteria, or you have selected not to "+
                "recommend any events. To modify your preferences please visit the 'My Preferences' tab on this page.";

            RecommendedEvents.Controls.Add(lab);
        }

    }

    //protected void GetFriendEvents()
    //{
    //    DateTime date = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //    string command = "SELECT DISTINCT U.UserName, UC.EventID, EO.DateTimeStart, E.Header " +
    //        "FROM Users AS U INNER JOIN " +
    //                     "User_Friends AS UF ON U.User_ID = UF.FriendID INNER JOIN " +
    //                     "User_Calendar AS UC ON UF.FriendID = UC.UserID INNER JOIN " +
    //                     "Event_Occurance AS EO ON UC.EventID = EO.EventID INNER JOIN " +
    //                     "Events AS E ON EO.EventID = E.ID " +
    //                     "WHERE (UF.UserID = "+Session["User"].ToString()+") AND MONTH(EO.DateTimeStart) = "+DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).Month.ToString();

    //    DataSet dsFriends = dat.GetData(command);
    //    int count = 0;
    //    if (dsFriends.Tables.Count > 0)
    //        if (dsFriends.Tables[0].Rows.Count > 0)
    //            if (dsFriends.Tables[0].Rows.Count > count)
    //            {
    //                count = dsFriends.Tables[0].Rows.Count;
                    
    //            }

    //    if (count == 0)
    //        FriendMessagesPanel.Visible = false;
    //    for (int i = 0; i < count; i++)
    //    {
    //        string title = dsFriends.Tables[0].Rows[i]["Header"].ToString();

    //        if (title.Length > 30)
    //            title = title.Substring(0, 30) + "..";
    //        FriendMessagesLiteral.Text += "<div class=\"EventBody FriendMessage\" style=\"width: 400px;\"> " + dsFriends.Tables[0].Rows[i]["UserName"].ToString() +
    //            " has added <a class=\"NavyLink12\"  href=\"Event.aspx?EventID=" +
    //            dsFriends.Tables[0].Rows[i]["EventID"].ToString() + "\">" +
    //            dsFriends.Tables[0].Rows[i]["Header"].ToString() + "</a> to their calendar<br/></div>";
    //    }
    //}

    protected void ServerDeleteMessage(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        UserErrorLabel.Text = "hello";
        try
        {
            ImageButton theImg = (ImageButton)sender;
            string messageID = theImg.CommandArgument;

            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            DataSet dsMess = dat.GetData("SELECT * FROM UserMessages WHERE ID="+messageID);
            if (dsMess.Tables[0].Rows[0]["From_UserID"] == Session["User"])
            {
                dat.Execute("UPDATE UserMessages SET SentLive=0 WHERE ID=" + messageID);
            }
            else
            {
                dat.Execute("UPDATE UserMessages SET Live=0 WHERE ID=" + messageID);
            }

            //Telerik.Web.UI.RadPanelBar bar = (Telerik.Web.UI.RadPanelBar)theImg.Parent.Parent.Parent;

            //Telerik.Web.UI.RadPanelItem item = (Telerik.Web.UI.RadPanelItem)theImg.Parent.Parent;
            //item.Visible = false;

            Response.Redirect("my-account");

            //LoadControlsNotAJAX();

            //RadPageView3 : UserErrorLabel.Text = theImg.Parent.Parent.Parent.Parent.Parent.Parent.Parent.ID.ToString();
        }
        catch (Exception ex)
        {
            UserErrorLabel.Text = ex.ToString();
        }
        
    }

    protected void ServerMarkRead(object sender, Telerik.Web.UI.RadPanelBarEventArgs e)
    {
        //e.Item.Text = e.Item.Text.Replace("bold", "normal");
        //e.Item.Text = e.Item.Text.Replace("White", "#cccccc");
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        try
        {
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            DataSet dsRead = dat.GetData("SELECT * FROM UserMessages WHERE ID=" + e.Item.Value);

            if (!bool.Parse(dsRead.Tables[0].Rows[0]["Read"].ToString()))
            {

                dat.Execute("UPDATE UserMessages SET [Read]='True' WHERE ID=" + e.Item.Value);

                e.Item.Text = e.Item.Text.Replace("bold", "normal");
                e.Item.Text = e.Item.Text.Replace("White", "#cccccc");

                int unreadCount = int.Parse(RadTabStrip3.Tabs[0].Value) - 1;

                RadTabStrip3.Tabs[0].Value = unreadCount.ToString();

                if (unreadCount == 0)
                {
                    RadTabStrip3.Tabs[0].Text = "Inbox";
                }
                else
                {

                    RadTabStrip3.Tabs[0].Text = "Inbox <span style=\"font-size: 12px; vertical-align: middle; font-weight: normal; padding-bottom: 3px;\">(" + unreadCount.ToString() + " New)</span>";

                }
            }
        }
        catch (Exception ex)
        {
            UserErrorLabel.Text = ex.ToString();
        }
    }

    protected void ServerReply(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
        bool goOn = false;

        if (Session["NotFinished"] == null)
        {
            Session["NotFinished"] = true;
            if (Session["TimeReplySubmitted"] == null)
                goOn = true;
            else
            {
                if (DateTime.Now < ((DateTime)Session["TimeReplySubmitted"]).AddSeconds(2))
                {
                    goOn = false;
                }
                else
                {
                    goOn = true;
                }
            }

            if (goOn)
            {
                try
                {
                    Session["TimeReplySubmitted"] = DateTime.Now;

                    LinkButton link = (LinkButton)sender;
                    string[] delim = { "reply" };

                    string[] tokens = link.CommandArgument.Split(delim, StringSplitOptions.None);

                    TextBox textbox = (TextBox)dat.FindControlRecursive(this, tokens[0] + "textbox" + tokens[1]);

                    Panel panel = (Panel)dat.FindControlRecursive(this, "RadPanel" + tokens[1]);

                    int To_ID = int.Parse(tokens[0]);

                    DataSet ds = dat.GetData("SELECT * FROM UserMessages WHERE ID=" + tokens[1]);

                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());

                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO UserMessages (MessageContent, MessageSubject, " +
                        "From_UserID, To_UserID, Date, [Read], Mode)"
                        + " VALUES(@content, @subject, @fromID, @toID, @date, 'False', 0)", conn);
                    cmd.Parameters.Add("@content", SqlDbType.Text).Value = textbox.Text;
                    cmd.Parameters.Add("@subject", SqlDbType.NVarChar).Value = "Re: " + ds.Tables[0].Rows[0]["MessageSubject"].ToString();
                    cmd.Parameters.Add("@toID", SqlDbType.Int).Value = To_ID;
                    cmd.Parameters.Add("@fromID", SqlDbType.Int).Value = Session["User"].ToString();
                    cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
                    cmd.ExecuteNonQuery();

                    DataSet dsUser = dat.GetData("SELECT * FROM Users WHERE User_ID=" + Session["User"].ToString());
                    DataSet dsTo = dat.GetData("SELECT * FROM Users U, UserPreferences UP WHERE UP.UserID=U.User_ID AND U.User_ID=" + To_ID);

                    //only send to email if users preferences are set to do so.
                    if (dsTo.Tables[0].Rows[0]["EmailPrefs"].ToString().Contains("4"))
                    {
                        dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"],
                            System.Configuration.ConfigurationManager.AppSettings["emailName"],
                            dsTo.Tables[0].Rows[0]["Email"].ToString(), textbox.Text, "Re: " + ds.Tables[0].Rows[0]["MessageSubject"].ToString());
                    }
                    conn.Close();


                    Literal lit = new Literal();
                    lit.Text = "<div align=\"left\" style=\"width: 220px; margin: 5px; float: right;\" class=\"Green12LinkNF\">Your reply has been sent!</div>";

                    panel.Controls.AddAt(7, lit);

                    Session["NotFinished"] = null;
                    Session.Remove("NotFinished");
                }
                catch (Exception ex)
                {
                    MessagesErrorLabel.Text += " " + ex.ToString();

                    Session["NotFinished"] = null;
                    Session.Remove("NotFinished");
                }
            }
            else
            {
                LinkButton link = (LinkButton)sender;
                string[] delim = { "reply" };

                string[] tokens = link.CommandArgument.Split(delim, StringSplitOptions.None);

                Panel panel = (Panel)dat.FindControlRecursive(this, "RadPanel" + tokens[1]);

                Literal lit = new Literal();
                lit.Text = "<div align=\"left\" style=\"height: 30px; width: 220px; margin: 5px; float: right;\" class=\"Green12LinkNF\">Your reply has been sent!</div>";

                panel.Controls.AddAt(7, lit);

                Session["NotFinished"] = null;
                Session.Remove("NotFinished");
            }
        }
    }

    protected void ServerAcceptFriend(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];

        try
        {
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            LinkButton link = (LinkButton)sender;
            string[] delim = { "accept" };

            int To_ID = int.Parse(link.CommandArgument.Split(delim, StringSplitOptions.None)[1]);

            Panel panel = (Panel)dat.FindControlRecursive(this, "RadPanel" + link.CommandArgument.Split(delim, StringSplitOptions.None)[0]);
            Panel wrapPanel = (Panel)dat.FindControlRecursive(this, "acceptWrap" + link.CommandArgument.Split(delim, StringSplitOptions.None)[0]);

            DataSet ds = dat.GetData("SELECT * FROM User_Friends WHERE UserID=" + Session["User"].ToString() +
                " AND FriendID=" + To_ID);

            bool hasFriend = false;

            if (ds.Tables.Count > 0)
                if (ds.Tables[0].Rows.Count > 0)
                    hasFriend = true;
                else
                    hasFriend = false;
            else
                hasFriend = false;

            if (!hasFriend)
            {

                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());

                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO UserMessages (MessageContent, MessageSubject, " +
                    "From_UserID, To_UserID, Date, [Read], Mode)"
                    + " VALUES(@content, @subject, "+dat.HIPPOHAPP_USERID.ToString()+", @toID, @date, 'False', 0)", conn);
                cmd.Parameters.Add("@content", SqlDbType.Text).Value = "Congratulations!, <br/><br/> " +
                    "We wanted to let you know that " + Session["UserName"].ToString()
                    + " has accepted your friend request. Good luck in your journey!<br/><br/> Have a " +
                    "Happening Day! <br/><br/> ";
                cmd.Parameters.Add("@subject", SqlDbType.NVarChar).Value = "Friend Request Approved from " + Session["UserName"].ToString();
                cmd.Parameters.Add("@toID", SqlDbType.Int).Value = To_ID;
                cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
                cmd.ExecuteNonQuery();

                DataSet dsUser = dat.GetData("SELECT * FROM Users WHERE User_ID=" + Session["User"].ToString());
                DataSet dsTo = dat.GetData("SELECT * FROM Users U, UserPreferences UP WHERE UP.UserID=U.User_ID AND U.User_ID=" + To_ID);

                //only send to email if users preferences are set to do so.
                if (dsTo.Tables[0].Rows[0]["EmailPrefs"].ToString().Contains("8"))
                {
                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"],
                    System.Configuration.ConfigurationManager.AppSettings["emailName"],
                        dsTo.Tables[0].Rows[0]["Email"].ToString(), "Congratulations!, <br/><br/> " +
                        "We wanted to let you know that " + Session["UserName"].ToString()
                        + " has accepted your friend request. Good luck in your journey!<br/><br/> Have a " +
                        "Happening Day! <br/><br/> ", "Friend Request Approved from " + Session["UserName"].ToString());
                }
                dat.Execute("INSERT INTO User_Friends (UserID, FriendID) VALUES(" + Session["User"].ToString()
                    + ", " + To_ID + ")");
                dat.Execute("INSERT INTO User_Friends (UserID, FriendID) VALUES(" + To_ID
                    + ", " + Session["User"].ToString() + ")");

                conn.Close();

            }

            Literal lit = new Literal();
            lit.Text = "<div style=\"float: right; width: 220px;height: 30px; margin: 5px;\" " +
                "class=\"Green12LinkNF\">You have accepted this gal/guy as a friend! Good luck, you two!</div>";

            panel.Controls.AddAt(7, lit);

            wrapPanel.Visible = false;
        }
        catch (Exception ex)
        {
            AleksLabel.Text = ex.ToString();
            UpdatePanel2.Update();
        }
    }

    protected void ServerAcceptChange(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        try
        {
            LinkButton thebutton = (LinkButton)sender;

            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            string theRow = "";
            bool isVenue = false;

            if (thebutton.Attributes["commArg"].ToString().ToLower() == "venue")
                isVenue = true;

            if (isVenue)
            {
                if (thebutton.Attributes["commandargument"].Contains("category"))
                {


                }
                else
                {

                    string[] delim = { "accept" };
                    string[] tokens = thebutton.Attributes["commandargument"].Split(delim, StringSplitOptions.None);

                    string rowID = tokens[0];
                    string columnNumber = tokens[1];
                    theRow = rowID;
                    DataSet dsEventID = dat.GetData("SELECT * FROM VenueRevisions WHERE ID=" + rowID);

                    string eventID = dsEventID.Tables[0].Rows[0]["VenueID"].ToString();

                    string columnName = dsEventID.Tables[0].Columns[int.Parse(columnNumber)].ColumnName;

                    string temp = dsEventID.Tables[0].Rows[0][columnName].ToString();

                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE Venues SET " + columnName + "=@p1 WHERE ID=@eventID", conn);
                    cmd.Parameters.Add("@p1", SqlDbType.NVarChar).Value = dsEventID.Tables[0].Rows[0][columnName].ToString();
                    cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = eventID;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    dat.ApproveRejectChange("VenueRevisions", rowID, int.Parse(columnNumber)-3, true);


                    string venueName = dat.GetData("SELECT * FROM Venues V, VenueRevisions VCR WHERE V.ID=VCR.VenueID AND VCR.ID=" + rowID).Tables[0].Rows[0]["Name"].ToString();


                    //string categoryName = dat.GetData("SELECT * FROM VenueRevisions VCR, VenueCategories VC " +
                    //    "WHERE VC.CategoryID=VCR.CategoryID AND VCR.ID=" + rowID).Tables[0].Rows[0]["CategoryName"].ToString();

                    DataSet dsRevision = dat.GetData("SELECT * FROM VenueRevisions VCR, Users U WHERE U.User_ID=VCR.modifierID AND VCR.ID=" + rowID);

                    DataSet dsUser = dat.GetData("SELECT * FROM Users U WHERE User_ID=" + dsRevision.Tables[0].Rows[0]["modifierID"].ToString());
                    string emailBody = "The revised venue " + columnName + " has been apporved by the author of the venue. <br/><br/> " +
                        "To view these changes, please visit <a href=\"http://HippoHappenings.com/"+dat.MakeNiceName(venueName) +"_"+ dsRevision.Tables[0].Rows[0]["VenueID"].ToString() + "_Venue\">" + venueName + "</a>";

                    if (!Request.IsLocal)
                    {
                        dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                        System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                        dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "One of your changes has been approved for venue: '" +
                        venueName + "'");
                    }
                    Literal lab = new Literal();

                    lab.Text = "<div class=\"Green12LinkNF\" style=\"float: right;margin-top: 20px; padding-bottom: 4px;height: 30px;\">You have accepted this change.</div>";


                    Label lab2 = new Label();
                    HtmlButton but2 = new HtmlButton();
                    but2 = (HtmlButton)thebutton.Parent.Controls[thebutton.Parent.Controls.IndexOf(thebutton) + 1];

                    but2.Parent.Controls.AddAt(but2.Parent.Controls.IndexOf(but2), lab2);
                    but2.Parent.Controls.Remove(but2);
                    //Put the label in place of the button
                    thebutton.Parent.Controls.AddAt(thebutton.Parent.Controls.IndexOf(thebutton), lab);
                    //Then remove the button
                    thebutton.Parent.Controls.Remove(thebutton);
                }
            }
            else
            {


                if (thebutton.Attributes["commandargument"].Contains("occurance"))
                {
                    //MessagesLabel.Text = "got here ";
                    string rowID = thebutton.Attributes["commandargument"].Replace("occurance", "");
                    theRow = rowID;
                    //MessagesLabel.Text = rowID;
                    dat.Execute("INSERT INTO Event_Occurance (EventID, DateTimeStart, DateTimeEnd) " +
        "SELECT EventID, DateTimeStart, DateTimeEnd FROM EventRevisions_Occurance WHERE RevisionID=" + rowID);
                    //MessagesLabel.Text = "flew here";
                    dat.Execute("UPDATE EventRevisions_Occurance SET Approved='True' WHERE RevisionID=" + rowID);

                    DataSet dsRevision = dat.GetData("SELECT E.Header AS H1, E.ID AS TID, *  FROM EventRevisions ER, Events E WHERE E.ID=ER.EventID AND ER.ID=" + rowID);

                    DataSet dsUser = dat.GetData("SELECT * FROM Users U WHERE User_ID=" + dsRevision.Tables[0].Rows[0]["modifierID"].ToString());
                    string emailBody = "The revised event re-occurance dates have been apporved by the author of the event. <br/><br/> " +
                        "To view these changes, please visit <a href=\"http://HippoHappenings.com/"+dat.MakeNiceName(dsRevision.Tables[0].Rows[0]["H1"].ToString())+"_" + dsRevision.Tables[0].Rows[0]["TID"].ToString() + "_Event\">" + dsRevision.Tables[0].Rows[0]["H1"].ToString() + "</a>";

                    if (!Request.IsLocal)
                    {
                        dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                        System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                        dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "One of your changes has been approved for event: '" +
                        dsRevision.Tables[0].Rows[0]["H1"].ToString() + "'");

                        //Send email to all users who have this event in their calendar and have their email preference set for event updates

                        emailBody = "<br/><br/>Changes have been made to an event in your calendar: Event '\"" + dsRevision.Tables[0].Rows[0]["H1"].ToString() +
                            "\"'. <br/><br/> To view these changes, please go to this event's <a class=\"NavyLink12\"  href=\"http://hippohappenings.com/" + dat.MakeNiceName(dsRevision.Tables[0].Rows[0]["H1"].ToString()) + "_" + dsRevision.Tables[0].Rows[0]["TID"].ToString() + "_Event\">page</a>. " +
                            "<br/><br/><br/>Have a Hippo Happening Day!<br/><br/> <a class=\"NavyLink12\"  href=\"http://HippoHappenings.com\">Happening Hippo</a>";

                        DataSet dsAllUsers = dat.GetData("SELECT * FROM User_Calendar UC, Users U, UserPreferences UP WHERE U.User_ID=UP.UserID AND UP.EmailPrefs LIKE '%C%' AND U.User_ID=UC.UserID AND UC.EventID=" + dsRevision.Tables[0].Rows[0]["TID"].ToString());

                        DataView dv = new DataView(dsAllUsers.Tables[0], "", "", DataViewRowState.CurrentRows);

                        if (dv.Count > 0)
                        {
                            for (int i = 0; i < dv.Count; i++)
                            {
                                dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                                    dv[i]["Email"].ToString(), emailBody,
                                    "Event '" +
                                    dsRevision.Tables[0].Rows[0]["H1"].ToString() + "' has been modified");

                                dat.Execute("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, " +
                                    "To_UserID, Date, [Read], Mode, Live, SentLive) VALUES('" + emailBody.Replace("'", "''") + "', '" + "Event ''" +
                                    dsRevision.Tables[0].Rows[0]["H1"].ToString().Replace("'", "''") + "'' has been modified', "+dat.HIPPOHAPP_USERID.ToString()+", " + dv[i]["UserID"].ToString() + ", GETDATE(), 0, 1, 1, 0)");
                            }
                        }

                    }

                    Literal lab = new Literal();
                    lab.Text = "<div class=\"Green12LinkNF\" style=\"float: right;margin-top: 20px; padding-bottom: 4px;height: 30px;\">You have accepted this change.</div>";

                    Label lab2 = new Label();
                    HtmlButton but2 = new HtmlButton();
                    but2 = (HtmlButton)thebutton.Parent.Controls[thebutton.Parent.Controls.IndexOf(thebutton) + 1];

                    but2.Parent.Controls.AddAt(but2.Parent.Controls.IndexOf(but2), lab2);
                    but2.Parent.Controls.Remove(but2);
                    //Put the label in place of the button
                    thebutton.Parent.Controls.AddAt(thebutton.Parent.Controls.IndexOf(thebutton), lab);
                    //Then remove the button
                    thebutton.Parent.Controls.Remove(thebutton);
                }
                else
                {

                    string[] delim = { "accept" };
                    string[] tokens = thebutton.Attributes["commandargument"].Split(delim, StringSplitOptions.None);

                    string rowID = tokens[0];
                    string columnNumber = tokens[1];
                    theRow = rowID;
                    DataSet dsEventID = dat.GetData("SELECT * FROM EventRevisions WHERE ID=" + rowID);

                    string eventID = dsEventID.Tables[0].Rows[0]["EventID"].ToString();

                    string columnName = dsEventID.Tables[0].Columns[int.Parse(columnNumber)].ColumnName;

                    string temp = dsEventID.Tables[0].Rows[0][columnName].ToString();

                    dat.ApproveRejectChange("EventRevisions", rowID, int.Parse(columnNumber)-3, true);


                    //if user is accepting a venue change, also accept zip, city, state and country
                    if (columnName.ToLower() == "venue")
                    {
                        dat.ApproveRejectChange("EventRevisions", rowID, 3, true);
                        dat.ApproveRejectChange("EventRevisions", rowID, 4, true);
                        dat.ApproveRejectChange("EventRevisions", rowID, 5, true);
                        dat.ApproveRejectChange("EventRevisions", rowID, 6, true);
                        dat.ApproveRejectChange("EventRevisions", rowID, 10, true);
                    }



                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE Events SET " + columnName + "=@p1 WHERE ID=@eventID", conn);
                    cmd.Parameters.Add("@p1", SqlDbType.NVarChar).Value = dsEventID.Tables[0].Rows[0][columnName].ToString();
                    cmd.Parameters.Add("@eventID", SqlDbType.Int).Value = eventID;
                    cmd.ExecuteNonQuery();
                    conn.Close();


                    DataSet dsRevision = dat.GetData("SELECT E.Header AS H1, E.ID AS TID, * FROM EventRevisions ER, Events E WHERE E.ID=ER.EventID AND ER.ID=" + rowID);

                    DataSet dsUser = dat.GetData("SELECT * FROM Users U WHERE User_ID=" + dsRevision.Tables[0].Rows[0]["modifierID"].ToString());
                    string emailBody = "The revised event " + columnName + " has been apporved by the author of the event. <br/><br/> " +
                        "To view these changes, please visit <a href=\"http://HippoHappenings.com/" + dat.MakeNiceName(dsRevision.Tables[0].Rows[0]["H1"].ToString()) + "_" + dsRevision.Tables[0].Rows[0]["TID"].ToString() + "_Event\">" + dsRevision.Tables[0].Rows[0]["H1"].ToString() + "</a>";

                    if (!Request.IsLocal)
                    {

                        dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                        System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                        dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "One of your changes has been approved for event: '" +
                        dsRevision.Tables[0].Rows[0]["H1"].ToString() + "'");

                        //Send email to all users who have this event in their calendar and have their email preference set for event updates

                        emailBody = "<br/><br/>Changes have been made to an event in your calendar: Event '\"" + dsRevision.Tables[0].Rows[0]["H1"].ToString() +
                            "\"'. <br/><br/> To view these changes, please go to this event's <a class=\"NavyLink12\" href=\"http://hippohappenings.com/" + dat.MakeNiceName(dsRevision.Tables[0].Rows[0]["H1"].ToString()) + "_" + dsRevision.Tables[0].Rows[0]["TID"].ToString() + "_Event\">page</a>. " +
                            "<br/><br/><br/>Have a Hippo Happening Day!<br/><br/> <a class=\"NavyLink12\" href=\"http://HippoHappenings.com\">HippoHappenings</a>";

                        DataSet dsAllUsers = dat.GetData("SELECT * FROM User_Calendar UC, Users U, UserPreferences UP " +
                            "WHERE U.User_ID=UP.UserID  AND U.User_ID=UC.UserID AND UC.EventID=" +
                            dsRevision.Tables[0].Rows[0]["TID"].ToString());

                        DataView dv = new DataView(dsAllUsers.Tables[0], "", "", DataViewRowState.CurrentRows);

                        if (dv.Count > 0)
                        {
                            for (int i = 0; i < dv.Count; i++)
                            {
                                if (dv[i]["EmailPrefs"].ToString().Contains("C"))
                                {
                                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                                        System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                                        dv[i]["Email"].ToString(), emailBody,
                                        "Event '" +
                                        dsRevision.Tables[0].Rows[0]["H1"].ToString() + "' has been modified");
                                }
                                

                                dat.Execute("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, " +
                                    "To_UserID, Date, [Read], Mode, Live, SentLive) VALUES('" + emailBody.Replace("'", "''") + "', '" + "Event ''" +
                                    dsRevision.Tables[0].Rows[0]["H1"].ToString().Replace("'", "''") + "'' has been modified', " + dat.HIPPOHAPP_USERID +
                                    ", " + dv[i]["UserID"].ToString() + ", GETDATE(), 0, 1, 1, 0)");
                            }
                        }
                    }
                    Literal lab = new Literal();
                    lab.Text = "<div class=\"Green12LinkNF\" style=\"float: right;margin-top: 20px; padding-bottom: 4px;height: 30px;\">You have accepted this change.</div>";
                    


                    Label lab2 = new Label();
                    HtmlButton but2 = new HtmlButton();
                    but2 = (HtmlButton)thebutton.Parent.Controls[thebutton.Parent.Controls.IndexOf(thebutton) + 1];

                    but2.Parent.Controls.AddAt(but2.Parent.Controls.IndexOf(but2), lab2);
                    but2.Parent.Controls.Remove(but2);
                    //Put the label in place of the button
                    thebutton.Parent.Controls.AddAt(thebutton.Parent.Controls.IndexOf(thebutton), lab);
                    //Then remove the button
                    thebutton.Parent.Controls.Remove(thebutton);
                }


            }
        }
        catch (Exception ex)
        {
            MessagesLabel.Text = ex.ToString();
        }
    }

    protected void ServerRejectChange(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        try
        {
            HtmlButton thebutton = (HtmlButton)sender;

            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            string theRow = "";
            bool isVenue = false;

            if (thebutton.Attributes["commArg"].ToString().ToLower() == "venue")
                isVenue = true;

            string rowID = "";

            if (isVenue)
            {
                if (thebutton.Attributes["commandargument"].Contains("category"))
                {
                    //THIS PART NOW TAKEN CARE OF BY REJECTCATEGORY METHOD
                    //rowID = thebutton.Attributes["commandargument"].Replace("category", "");
                    //theRow = rowID;

                    //string venueName = dat.GetData("SELECT * FROM Venues V, VenueCategoryRevisions VCR WHERE V.ID=VCR.VenueID AND VCR.ID=" + rowID).Tables[0].Rows[0]["Name"].ToString();

                    //string categoryName = dat.GetData("SELECT * FROM VenueCategoryRevisions VCR, VenueCategories VC " +
                    //    "WHERE VC.CategoryID=VCR.CategoryID AND VCR.ID=" + rowID).Tables[0].Rows[0]["CategoryName"].ToString();
                    //dat.Execute("UPDATE VenueCategoryRevisions SET Approved='False' WHERE ID=" + rowID);

                    //DataSet dsRevision = dat.GetData("SELECT * FROM VenueCategoryRevisions VCR, Users U WHERE U.User_ID=VCR.modifierID AND VCR.ID=" + rowID);


                    //DataSet dsUser = dat.GetData("SELECT * FROM Users U WHERE User_ID=" + dsRevision.Tables[0].Rows[0]["modifierID"].ToString());
                    //string emailBody = "The category suggestion '" + categoryName + "' has been rejected for the venue '" + venueName +
                    //    "' by the venue's author. <br/><br/> " +
                    //    "To view the venue, please visit <a href=\"http://HippoHappenings.com/Venue.aspx?ID=" +
                    //    dsRevision.Tables[0].Rows[0]["VenueID"].ToString() + "\">" + venueName + "</a>";
                    //dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                    //System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                    //dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "One of your changes has been rejected for venue: '" +
                    //venueName + "'");

                    //Literal lab = new Literal();

                    //lab.Text = "<div class=\"AddGreenLink\" style=\"float: right;margin-top: 20px; padding-bottom: 4px;height: 30px;\">You have rejected this change.</div>";
                    ////Remove the 'Accept' button first
                    //thebutton.Page.Controls.RemoveAt(thebutton.Parent.Controls.IndexOf(thebutton) - 1);
                    ////Put the label in place of the 'Reject' button
                    //thebutton.Parent.Controls.AddAt(thebutton.Parent.Controls.IndexOf(thebutton), lab);
                    ////Then remove the 'Reject' button
                    //thebutton.Parent.Controls.Remove(thebutton);
                }
                else
                {

                    string[] delim = { "reject" };
                    string[] tokens = thebutton.Attributes["commandargument"].Split(delim, StringSplitOptions.None);
                    rowID = tokens[0];
                    int columnNumber = int.Parse(tokens[1]);
                    dat.ApproveRejectChange("VenueRevisions", rowID, columnNumber-3, false);

                    theRow = rowID;
                    DataSet dsEventID = dat.GetData("SELECT * FROM VenueRevisions WHERE ID=" + rowID);

                    string eventID = dsEventID.Tables[0].Rows[0]["VenueID"].ToString();

                    string columnName = dsEventID.Tables[0].Columns[columnNumber].ColumnName;

                    string temp = dsEventID.Tables[0].Rows[0][columnName].ToString();

                    Literal lab = new Literal();

                    lab.Text = "<div class=\"Green12LinkNF\" style=\"float: right;margin-top: 20px; padding-bottom: 4px;height: 30px;\">You have rejected this change.</div>";
                    string venueName = dat.GetData("SELECT * FROM Venues V, VenueRevisions VCR WHERE V.ID=VCR.VenueID AND VCR.ID=" +
                        rowID).Tables[0].Rows[0]["Name"].ToString();

                    //string categoryName = dat.GetData("SELECT * FROM VenueRevisions VCR, VenueCategories VC " +
                    //    "WHERE VC.CategoryID=VCR.CategoryID AND VCR.ID=" + rowID).Tables[0].Rows[0]["CategoryName"].ToString();

                    DataSet dsRevision = dat.GetData("SELECT * FROM VenueRevisions VCR, Users U WHERE U.User_ID=VCR.modifierID AND VCR.ID=" +
                        rowID);

                    DataSet dsUser = dat.GetData("SELECT * FROM Users U WHERE User_ID=" + dsRevision.Tables[0].Rows[0]["modifierID"].ToString());
                    string emailBody = "The submitted change for " + columnName + " has been rejected by the author of the venue. <br/><br/> " +
                        "To view these changes, please visit <a href=\"http://HippoHappenings.com/"+dat.MakeNiceName(venueName)+"_" + dsRevision.Tables[0].Rows[0]["VenueID"].ToString() + "_Venue\">" + venueName + "</a>";


                    if (!Request.IsLocal)
                    {
                        dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                        System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                        dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "One of your changes has been rejected for venue: '" +
                        venueName + "'");
                    }

                    Label lab2 = new Label();
                    HtmlButton but2 = new HtmlButton();
                    but2 = (HtmlButton)thebutton.Parent.Controls[thebutton.Parent.Controls.IndexOf(thebutton) - 1];

                    but2.Parent.Controls.AddAt(but2.Parent.Controls.IndexOf(but2), lab2);
                    but2.Parent.Controls.Remove(but2);
                    //Put the label in place of the button
                    thebutton.Parent.Controls.AddAt(thebutton.Parent.Controls.IndexOf(thebutton), lab);
                    //Then remove the button
                    thebutton.Parent.Controls.Remove(thebutton);
                }
            }
            else
            {
                if (thebutton.Attributes["commandargument"].Contains("occurance"))
                {
                    rowID = thebutton.Attributes["commandargument"].Replace("occurance", "");
                    theRow = rowID;

                    dat.Execute("UPDATE EventRevisions_Occurance SET Approved='False' WHERE RevisionID=" + rowID);

                    DataSet dsRevision = dat.GetData("SELECT E.Header AS H1, E.ID AS TID, *  FROM EventRevisions ER, Events E WHERE E.ID=ER.EventID AND ER.ID=" + rowID);

                    DataSet dsUser = dat.GetData("SELECT * FROM Users U WHERE User_ID=" + dsRevision.Tables[0].Rows[0]["modifierID"].ToString());
                    string emailBody = "The revised event re-occurance dates have been rejected by the author of the event. <br/><br/> " +
                        "We appologize for any inconvenience. <br/><br/>To view the event, please visit <a href=\"http://HippoHappenings.com/"+dat.MakeNiceName(dsRevision.Tables[0].Rows[0]["H1"].ToString())+"_" +
                        dsRevision.Tables[0].Rows[0]["TID"].ToString() + "_Event\">" + dsRevision.Tables[0].Rows[0]["H1"].ToString() + "</a>";
                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                    dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "One of your changes has been rejected for event: '" +
                    dsRevision.Tables[0].Rows[0]["H1"].ToString() + "'");

                    Literal lab = new Literal();

                    lab.Text = "<div class=\"Green12LinkNF\" style=\"float: right;margin-top: 20px; padding-bottom: 4px;height: 30px;\">You have rejected this change.</div>";
                    


                    Label lab2 = new Label();
                    HtmlButton but2 = new HtmlButton();
                    but2 = (HtmlButton)thebutton.Parent.Controls[thebutton.Parent.Controls.IndexOf(thebutton) + 1];

                    but2.Parent.Controls.AddAt(but2.Parent.Controls.IndexOf(but2), lab2);
                    but2.Parent.Controls.Remove(but2);
                    //Put the label in place of the button
                    thebutton.Parent.Controls.AddAt(thebutton.Parent.Controls.IndexOf(thebutton), lab);
                    //Then remove the button
                    thebutton.Parent.Controls.Remove(thebutton);
                }
                else
                {

                    string[] delim = { "reject" };
                    string[] tokens = thebutton.Attributes["commandargument"].Split(delim, StringSplitOptions.None);

                    rowID = tokens[0];
                    int columnNumber = int.Parse(tokens[1]);
                    theRow = rowID;
                    DataSet dsEventID = dat.GetData("SELECT * FROM EventRevisions WHERE ID=" + rowID);

                    dat.ApproveRejectChange("EventRevisions", rowID, columnNumber-3, false);

                    string eventID = dsEventID.Tables[0].Rows[0]["EventID"].ToString();

                    string columnName = dsEventID.Tables[0].Columns[columnNumber].ColumnName;

                    string temp = dsEventID.Tables[0].Rows[0][columnName].ToString();

                    if (columnName.ToLower() == "venue")
                    {
                        dat.ApproveRejectChange("EventRevisions", rowID, 3, false);
                        dat.ApproveRejectChange("EventRevisions", rowID, 4, false);
                        dat.ApproveRejectChange("EventRevisions", rowID, 5, false);
                        dat.ApproveRejectChange("EventRevisions", rowID, 6, false);
                        dat.ApproveRejectChange("EventRevisions", rowID, 10, false);
                    }

                    DataSet dsRevision = dat.GetData("SELECT E.Header AS H1, E.ID AS TID, * FROM EventRevisions ER, Events E WHERE E.ID=ER.EventID AND ER.ID=" + rowID);

                    DataSet dsUser = dat.GetData("SELECT * FROM Users U WHERE User_ID=" + dsRevision.Tables[0].Rows[0]["modifierID"].ToString());
                    string emailBody = "The revised event " + columnName + " has been rejected by the author of the event. <br/><br/> " +
                        "We appologize for any inconvenience. To view the event, please visit <a href=\"http://HippoHappenings.com/" +dat.MakeNiceName(dsRevision.Tables[0].Rows[0]["H1"].ToString())+"_"+
                        dsRevision.Tables[0].Rows[0]["TID"].ToString() + "_Event\">" + dsRevision.Tables[0].Rows[0]["H1"].ToString() + "</a>";
                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                    dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "One of your changes has been rejected for event: '" +
                    dsRevision.Tables[0].Rows[0]["H1"].ToString() + "'");

                    Literal lab = new Literal();

                    lab.Text = "<div class=\"Green12LinkNF\" style=\"float: right;margin-top: 20px; padding-bottom: 4px;height: 30px;\">You have rejected this change.</div>";
                    

                    Label lab2 = new Label();
                    HtmlButton but2 = new HtmlButton();
                    but2 = (HtmlButton)thebutton.Parent.Controls[thebutton.Parent.Controls.IndexOf(thebutton) - 1];

                    but2.Parent.Controls.AddAt(but2.Parent.Controls.IndexOf(but2), lab2);
                    but2.Parent.Controls.Remove(but2);
                    //Put the label in place of the button
                    thebutton.Parent.Controls.AddAt(thebutton.Parent.Controls.IndexOf(thebutton), lab);
                    //Then remove the button
                    thebutton.Parent.Controls.Remove(thebutton);
                }


            }
        }
        catch (Exception ex)
        {
            MessagesLabel.Text = ex.ToString();
        }
        
    }

    protected void AddCategory(object sender, EventArgs e)
    {
        //MessagesLabel.Text += "got here";
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        try
        {
            HtmlButton theButt = (HtmlButton)sender;

            string[] delim = { "category" };
            string[] tokens = theButt.Attributes["commandargument"].Split(delim, StringSplitOptions.None);
            string CatID = tokens[2];
            string venueOrEvent = tokens[1];
            string contentID = tokens[0];
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            //MessagesLabel.Text += "tok 1: " + tokens[0] + ", tok2: " + tokens[1] + ", tok3: " + tokens[2];

            Literal lab = new Literal();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
            conn.Open();
            SqlCommand cmd;
            if (venueOrEvent == "venue")
            {
                cmd = new SqlCommand("INSERT INTO Venue_Category (VENUE_ID, CATEGORY_ID) VALUES(@vID, @cID)", conn);
                cmd.Parameters.Add("@vID", SqlDbType.Int).Value = contentID;
                cmd.Parameters.Add("@cID", SqlDbType.Int).Value = CatID;
                cmd.ExecuteNonQuery();

                dat.Execute("UPDATE VenueCategoryRevisions SET Approved='True' WHERE ID=" + tokens[3]);

                #region Send Email
                //send email
                string rowID = tokens[3];

                string venueName = dat.GetData("SELECT * FROM Venues V, VenueCategoryRevisions VCR WHERE V.ID=VCR.VenueID AND VCR.ID=" + rowID).Tables[0].Rows[0]["Name"].ToString();

                string categoryName = dat.GetData("SELECT * FROM VenueCategoryRevisions VCR, VenueCategories VC " +
                    "WHERE VC.ID=VCR.CatID AND VCR.ID=" + rowID).Tables[0].Rows[0]["Name"].ToString();

                DataSet dsRevision = dat.GetData("SELECT * FROM VenueCategoryRevisions VCR, Users U WHERE U.User_ID=VCR.modifierID AND VCR.ID=" + rowID);



                DataSet dsUser = dat.GetData("SELECT * FROM Users U WHERE User_ID=" + dsRevision.Tables[0].Rows[0]["modifierID"].ToString());
                string emailBody = "The addition of category '" + categoryName + "' has been approved for the venue '" + venueName +
                    "' by the venue's author. <br/><br/> " +
                    "To view these changes, please visit <a href=\"http://HippoHappenings.com/"+dat.MakeNiceName(venueName)+"_" +
                    dsRevision.Tables[0].Rows[0]["VenueID"].ToString() + "_Venue\">" + venueName + "</a>";

                if (!Request.IsLocal)
                {
                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                    dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "One of your changes has been approved for venue: '" +
                    venueName + "'");
                }
                #endregion
            }
            else
            {
                cmd = new SqlCommand("INSERT INTO Event_Category_Mapping (EventID, CategoryID) VALUES(@vID, @cID)",
                    conn);
                cmd.Parameters.Add("@vID", SqlDbType.Int).Value = contentID;
                cmd.Parameters.Add("@cID", SqlDbType.Int).Value = CatID;
                cmd.ExecuteNonQuery();

                dat.Execute("UPDATE EventCategoryRevisions SET Approved='True' WHERE ID=" + tokens[3]);

                #region Send Email
                //send email
                string rowID = tokens[3];

                string eventName = dat.GetData("SELECT * FROM Events V, EventCategoryRevisions VCR WHERE V.ID=VCR.EventID AND VCR.ID=" + rowID).Tables[0].Rows[0]["Header"].ToString();

                string categoryName = dat.GetData("SELECT * FROM EventCategoryRevisions VCR, EventCategories VC " +
                    "WHERE VC.ID=VCR.CatID AND VCR.ID=" + rowID).Tables[0].Rows[0]["Name"].ToString();

                DataSet dsRevision = dat.GetData("SELECT * FROM EventCategoryRevisions VCR, Users U WHERE U.User_ID=VCR.modifierID AND VCR.ID=" + rowID);



                DataSet dsUser = dat.GetData("SELECT * FROM Users U WHERE User_ID=" + dsRevision.Tables[0].Rows[0]["modifierID"].ToString());
                string emailBody = "The addition of category '" + categoryName + "' has been approved for the event '" + eventName +
                    "' by the event's author. <br/><br/> " +
                    "To view these changes, please visit <a href=\"http://HippoHappenings.com/" +dat.MakeNiceName(eventName)+"_"+
                    dsRevision.Tables[0].Rows[0]["EventID"].ToString() + "_Event\">" + eventName + "</a>";

                if (!Request.IsLocal)
                {
                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                    dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "One of your changes has been approved for event: '" +
                    eventName + "'");
                }
                #endregion
            }




            conn.Close();
            lab.Text = "<label class=\"Green12LinkNF\">&nbsp;&nbsp;&nbsp;&nbsp;Added</label>";
            Label lab2 = new Label();
            Literal theLit =
                (Literal)theButt.Parent.Controls[theButt.Parent.Controls.IndexOf(theButt) - 1];
            theLit.Text = theLit.Text.Replace("<br/>", "");
            theLit.Text = theLit.Text.Replace("<div style=\"padding-bottom: 4px;\">", 
                "<div align=\"right\" style=\"padding-bottom: 4px;\">");

            HtmlButton but2 = new HtmlButton();
            but2 = (HtmlButton)theButt.Parent.Controls[theButt.Parent.Controls.IndexOf(theButt) + 1];

            but2.Parent.Controls.AddAt(but2.Parent.Controls.IndexOf(but2), lab2);
            but2.Parent.Controls.Remove(but2);
            //Put the label in place of the button
            theButt.Parent.Controls.AddAt(theButt.Parent.Controls.IndexOf(theButt), lab);
            //Then remove the button
            theButt.Parent.Controls.Remove(theButt);
        }
        catch (Exception ex)
        {
            MessagesLabel.Text = ex.ToString();
        }

    }

    protected void RemoveCategory(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        try
        {
            HtmlButton theButt = (HtmlButton)sender;

            string[] delim = { "category" };
            string[] tokens = theButt.Attributes["commandargument"].Split(delim, StringSplitOptions.None);
            string CatID = tokens[2];
            string venueOrEvent = tokens[1];
            string contentID = tokens[0];
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            Literal lab = new Literal();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
            conn.Open();
            SqlCommand cmd;
            if (venueOrEvent == "venue")
            {
                cmd = new SqlCommand("DELETE FROM Venue_Category WHERE Venue_ID=@vID AND Category_ID=@cID", conn);
                cmd.Parameters.Add("@vID", SqlDbType.Int).Value = contentID;
                cmd.Parameters.Add("@cID", SqlDbType.Int).Value = CatID;
                cmd.ExecuteNonQuery();

                dat.Execute("UPDATE VenueCategoryRevisions SET Approved='True' WHERE ID=" + tokens[3]);
                #region Send Email
                //send email
                string rowID = tokens[3];

                string venueName = dat.GetData("SELECT * FROM Venues V, VenueCategoryRevisions VCR WHERE V.ID=VCR.VenueID AND VCR.ID=" + rowID).Tables[0].Rows[0]["Name"].ToString();

                string categoryName = dat.GetData("SELECT * FROM VenueCategoryRevisions VCR, VenueCategories VC " +
                    "WHERE VC.ID=VCR.CatID AND VCR.ID=" + rowID).Tables[0].Rows[0]["Name"].ToString();

                DataSet dsRevision = dat.GetData("SELECT * FROM VenueCategoryRevisions VCR, Users U WHERE U.User_ID=VCR.modifierID AND VCR.ID=" + rowID);



                DataSet dsUser = dat.GetData("SELECT * FROM Users U WHERE User_ID=" + dsRevision.Tables[0].Rows[0]["modifierID"].ToString());
                string emailBody = "The removal of category '" + categoryName + "' has been approved for the venue '" + venueName +
                    "' by the venue's author. <br/><br/> " +
                    "To view these changes, please visit <a href=\"http://HippoHappenings.com/"+dat.MakeNiceName(venueName) + "_"+
                    dsRevision.Tables[0].Rows[0]["VenueID"].ToString() + "_Venue\">" + venueName + "</a>";

                if (!Request.IsLocal)
                {
                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                    dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "One of your changes has been approved for venue: '" +
                    venueName + "'");
                }
                #endregion
            }
            else
            {
                cmd = new SqlCommand("DELETE FROM Event_Category_Mapping WHERE EventID=@vID AND CategoryID=@cID", conn);
                cmd.Parameters.Add("@vID", SqlDbType.Int).Value = contentID;
                cmd.Parameters.Add("@cID", SqlDbType.Int).Value = CatID;
                cmd.ExecuteNonQuery();

                dat.Execute("UPDATE EventCategoryRevisions SET Approved='True' WHERE ID=" + tokens[3]);

                #region Send Email
                //send email
                string rowID = tokens[3];

                string eventName = dat.GetData("SELECT * FROM Events V, EventCategoryRevisions VCR WHERE V.ID=VCR.EventID AND VCR.ID=" + rowID).Tables[0].Rows[0]["Header"].ToString();

                string categoryName = dat.GetData("SELECT * FROM EventCategoryRevisions VCR, EventCategories VC " +
                    "WHERE VC.ID=VCR.CatID AND VCR.ID=" + rowID).Tables[0].Rows[0]["Name"].ToString();

                DataSet dsRevision = dat.GetData("SELECT * FROM EventCategoryRevisions VCR, Users U WHERE U.User_ID=VCR.modifierID AND VCR.ID=" + rowID);



                DataSet dsUser = dat.GetData("SELECT * FROM Users U WHERE User_ID=" + dsRevision.Tables[0].Rows[0]["modifierID"].ToString());
                string emailBody = "The removal of category '" + categoryName + "' has been approved for the event '" + eventName +
                    "' by the event's author. <br/><br/> " +
                    "To view these changes, please visit <a href=\"http://HippoHappenings.com/" +dat.MakeNiceName(eventName)+"_"+
                    dsRevision.Tables[0].Rows[0]["EventID"].ToString() + "_Event\">" + eventName + "</a>";

                if (!Request.IsLocal)
                {
                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                    dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "One of your changes has been approved for event: '" +
                    eventName + "'");
                }
                #endregion
            }
            conn.Close();
            lab.Text = "<label class=\"Green12LinkNF\">&nbsp;&nbsp;&nbsp;&nbsp;Removed</label>";


            Literal theLit =
                (Literal)theButt.Parent.Controls[theButt.Parent.Controls.IndexOf(theButt) - 1];
            theLit.Text = theLit.Text.Replace("<br/>", "");
            theLit.Text = theLit.Text.Replace("<div style=\"padding-bottom: 4px;\">", "<div align=\"right\" style=\"padding-bottom: 4px;\">");
            Label lab2 = new Label();


            HtmlButton but2 = new HtmlButton();
            but2 = (HtmlButton)theButt.Parent.Controls[theButt.Parent.Controls.IndexOf(theButt) + 1];

            but2.Parent.Controls.AddAt(but2.Parent.Controls.IndexOf(but2), lab2);
            but2.Parent.Controls.Remove(but2);
            //Put the label in place of the button
            theButt.Parent.Controls.AddAt(theButt.Parent.Controls.IndexOf(theButt), lab);
            //Then remove the button
            theButt.Parent.Controls.Remove(theButt);
        }
        catch (Exception ex)
        {
            MessagesLabel.Text = ex.ToString();
        }
    }

    protected void RejectCategory(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        try
        {
            HtmlButton theButt = (HtmlButton)sender;

            string[] delim = { "category" };
            string[] tokens = theButt.Attributes["commandargument"].Split(delim, StringSplitOptions.None);
            string CatID = tokens[2];
            string venueOrEvent = tokens[1];
            string contentID = tokens[0];
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            Literal lab = new Literal();
            if (venueOrEvent == "venue")
            {

                dat.Execute("UPDATE VenueCategoryRevisions SET Approved='False' WHERE ID=" + tokens[3]);
                #region Send Email
                //send email
                string rowID = tokens[3];

                string venueName = dat.GetData("SELECT * FROM Venues V, VenueCategoryRevisions VCR WHERE V.ID=VCR.VenueID AND VCR.ID=" + rowID).Tables[0].Rows[0]["Name"].ToString();

                string categoryName = dat.GetData("SELECT * FROM VenueCategoryRevisions VCR, VenueCategories VC " +
                    "WHERE VC.ID=VCR.CatID AND VCR.ID=" + rowID).Tables[0].Rows[0]["Name"].ToString();

                DataSet dsRevision = dat.GetData("SELECT * FROM VenueCategoryRevisions VCR, Users U WHERE U.User_ID=VCR.modifierID AND VCR.ID=" + rowID);



                DataSet dsUser = dat.GetData("SELECT * FROM Users U WHERE User_ID=" + dsRevision.Tables[0].Rows[0]["modifierID"].ToString());
                string emailBody = "The removal of category '" + categoryName + "' has been rejected for the venue '" + venueName +
                    "' by the venue's author. <br/><br/> " +
                    "To view these changes, please visit <a href=\"http://HippoHappenings.com/" +dat.MakeNiceName(venueName)+"_"+
                    dsRevision.Tables[0].Rows[0]["VenueID"].ToString() + "_Venue\">" + venueName + "</a>";

                if (!Request.IsLocal)
                {
                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                    dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "One of your changes has been rejected for venue: '" +
                    venueName + "'");
                }
                #endregion
            }
            else
            {
                dat.Execute("UPDATE EventCategoryRevisions SET Approved='False' WHERE ID=" + tokens[3]);

                #region Send Email
                //send email
                string rowID = tokens[3];

                string eventName = dat.GetData("SELECT * FROM Events V, EventCategoryRevisions "+
                    "VCR WHERE V.ID=VCR.EventID AND VCR.ID=" + rowID).Tables[0].Rows[0]["Header"].ToString();

                string categoryName = dat.GetData("SELECT * FROM EventCategoryRevisions VCR, EventCategories VC " +
                    "WHERE VC.ID=VCR.CatID AND VCR.ID=" + rowID).Tables[0].Rows[0]["Name"].ToString();

                DataSet dsRevision = dat.GetData("SELECT * FROM EventCategoryRevisions VCR, Users U WHERE U.User_ID=VCR.modifierID AND VCR.ID=" + rowID);



                DataSet dsUser = dat.GetData("SELECT * FROM Users U WHERE User_ID=" + dsRevision.Tables[0].Rows[0]["modifierID"].ToString());
                string emailBody = "The removal of category '" + categoryName + "' has been rejected for the event '" + eventName +
                    "' by the event's author. <br/><br/> " +
                    "To view these changes, please visit <a href=\"http://HippoHappenings.com/" + dat.MakeNiceName(eventName) + "_" +
                    dsRevision.Tables[0].Rows[0]["EventID"].ToString() + "_Event\">" + eventName + "</a>";

                if (!Request.IsLocal)
                {
                    dat.SendEmail(System.Configuration.ConfigurationManager.AppSettings["emailemail"].ToString(),
                    System.Configuration.ConfigurationManager.AppSettings["emailName"].ToString(),
                    dsUser.Tables[0].Rows[0]["Email"].ToString(), emailBody, "One of your changes has been rejected for event: '" +
                    eventName + "'");
                }
                #endregion
            }
            lab.Text = "<label class=\"Green12LinkNF\">&nbsp;&nbsp;&nbsp;&nbsp;Rejected</label>";

            Literal theLit =
        (Literal)theButt.Parent.Controls[theButt.Parent.Controls.IndexOf(theButt) - 2];
            theLit.Text = theLit.Text.Replace("<br/>", "");
            theLit.Text = theLit.Text.Replace("<div style=\"padding-bottom: 4px;\">", "<div align=\"right\" style=\"padding-bottom: 4px;\">");


            Label lab2 = new Label();


            HtmlButton but2 = new HtmlButton();
            but2 = (HtmlButton)theButt.Parent.Controls[theButt.Parent.Controls.IndexOf(theButt) - 1];

            but2.Parent.Controls.AddAt(but2.Parent.Controls.IndexOf(but2), lab2);
            but2.Parent.Controls.Remove(but2);
            //Put the label in place of the button
            theButt.Parent.Controls.AddAt(theButt.Parent.Controls.IndexOf(theButt), lab);
            //Then remove the button
            theButt.Parent.Controls.Remove(theButt);
        }
        catch (Exception ex)
        {
            MessagesLabel.Text = ex.ToString();
        }
    }

    protected void AddFriend(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        LinkButton link = (LinkButton)sender;

        int To_ID = int.Parse(link.CommandArgument);

        DataSet ds = dat.GetData("SELECT * FROM User_Friends WHERE UserID="+Session["User"].ToString() + 
            " AND FriendID="+To_ID);

        bool hasFriend = false;

        if (ds.Tables.Count > 0)
            if (ds.Tables[0].Rows.Count > 0)
                hasFriend = true;
            else
                hasFriend = false;
        else
            hasFriend = false;

        if (!hasFriend)
        {

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());

            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, To_UserID, Date, [Read], Mode)"
                + " VALUES(@content, @subject, @fromID, @toID, @date, 'False', 2)", conn);
            cmd.Parameters.Add("@content", SqlDbType.Text).Value = "Good Day from Hippo Happenings!, <br/><br/> We wanted to let you know that the user '" + Session["UserName"].ToString() + "' would like " +
                "to add you to their list of friends. To accept this request select the link below. <br/><br/> Have a Happening Day! <br/><br/> ";
            cmd.Parameters.Add("@subject", SqlDbType.NVarChar).Value = "You Have a Hippo Friend Request!";
            cmd.Parameters.Add("@toID", SqlDbType.Int).Value = To_ID;
            cmd.Parameters.Add("@fromID", SqlDbType.Int).Value = Session["User"].ToString();
            cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
            cmd.ExecuteNonQuery();
            conn.Close();

            MessageLabel.Text = "Your friend request has been sent!";


        }
        else
            MessageLabel.Text = "The user you selected is already your friend!";



        Panel SearchResultsPanel = (Panel)dat.FindControlRecursive(this, "SearchResultsPanel");
        SearchResultsPanel.Controls.Clear();

        CloseSearchPanel();

    }
    
    protected void ViewFriend(object sender, EventArgs e)
    {
                HttpCookie cookie = Request.Cookies["BrowserDate"];
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        ClearMessage();
        ImageButton button = (ImageButton)sender;
        string arg = button.CommandArgument;
        Session["FriendToView"] = arg;
        DataView dvF = dat.GetDataDV("SELECT * FROM USERS WHERE USER_ID=" + arg);
        Response.Redirect(dvF[0]["UserName"].ToString() + "_Friend");
    }
    
    private void this_OnProgress(object sender, int ID)
    {
        if (Session["Archived"] != null)
            if (bool.Parse(Session["Archived"].ToString()))
            {
                LoadControls();
                DoAll();
            }

        Session["Archived"] = "False";

    }
    
    protected void GoTo(object sender, EventArgs e)
    {
        ImageButton b = (ImageButton)sender;

        string command = b.CommandArgument;

        switch (command)
        {
            case "P":
                Response.Redirect("UserPreferences.aspx?ID="+Session["User"].ToString());
                break;
            case "M":
                //MessagesPanel.Visible = true;
                //FriendPanel.Visible = false;
                //MessagesButton.ImageUrl = "image/MyMessagesHover.png";
                //FriendsButton.ImageUrl = "image/MyFriends.png";
                //MessagesButton.Attributes.Remove("onmouseover");
                //MessagesButton.Attributes.Remove("onmouseout");
                //FriendsButton.Attributes.Add("onmouseover", "this.src='image/MyFriendsHover.png'");
                //FriendsButton.Attributes.Add("onmouseout", "this.src='image/MyFriends.png'");
                break;
            case "F":
                //FriendPanel.Visible = true;
                //MessagesPanel.Visible = false;
                //MessagesButton.ImageUrl = "image/MyMessages.png";
                //MessagesButton.Attributes.Add("onmouseover", "this.src='image/MyMessagesHover.png'");
                //MessagesButton.Attributes.Add("onmouseout", "this.src='image/MyMessages.png'");
                //FriendsButton.ImageUrl = "image/MyFriendsHover.png";
                //FriendsButton.Attributes.Remove("onmouseover");
                //FriendsButton.Attributes.Remove("onmouseout");
                break;
            default: break;
        }
    }
   
    protected void ClearMessage()
    {
        MessageLabel.Text = "";
    }

    //[Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
    //public int Reply(string message, string subject, string fromID, string userID, string messageID, string i)
    //{
    //    try
    //    {
    //        SqlConnection conn;
    //        conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
    //        conn.Open();

    //        SqlCommand cmd = new SqlCommand("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, To_UserID, Date, [Read], Mode) " +
    //            " VALUES(@content, @subject, @from, @to, @date, 'false', 0)", conn);
    //        cmd.Parameters.Add("@content", SqlDbType.NVarChar).Value = message;
    //        cmd.Parameters.Add("@subject", SqlDbType.NVarChar).Value = "Re: " + subject;
    //        cmd.Parameters.Add("@from", SqlDbType.Int).Value = userID;
    //        cmd.Parameters.Add("@to", SqlDbType.Int).Value = fromID;
    //        cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
    //        cmd.ExecuteNonQuery();
    //        conn.Close();

            
    //    }
    //    catch (Exception ex)
    //    {
            
    //    }

    //    return int.Parse(i);
    //}

    //[Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
    //public int AcceptFriend(string userID, string friendID, string i)
    //{
    //    try
    //    {
    //        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

    //        SqlConnection conn;
    //        conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
    //        conn.Open();
    //        string username = dat.GetData("SELECT UserName FROM USERS WHERE User_ID=" + userID).Tables[0].Rows[0]["UserName"].ToString();
    //        //Add the friend.
    //        SqlCommand cmd = new SqlCommand("INSERT INTO User_Friends (FriendID, UserID) VALUES(@friend, @user)", conn);
    //        cmd.Parameters.Add("@friend", SqlDbType.Int).Value = userID;
    //        cmd.Parameters.Add("@user", SqlDbType.Int).Value = friendID;
    //        cmd.ExecuteNonQuery();
    //        cmd = new SqlCommand("INSERT INTO User_Friends (FriendID, UserID) VALUES(@friend, @user)", conn);
    //        cmd.Parameters.Add("@friend", SqlDbType.Int).Value = friendID;
    //        cmd.Parameters.Add("@user", SqlDbType.Int).Value = userID;
    //        cmd.ExecuteNonQuery();

    //        //Send message notifying the requestor that the friend has accepted
    //        cmd = new SqlCommand("INSERT INTO UserMessages (MessageContent, MessageSubject, From_UserID, To_UserID, Date, [Read], Mode) " +
    //            " VALUES(@content, @subject, @from, @to, @date, 'false', 1)", conn);
    //        cmd.Parameters.Add("@content", SqlDbType.NVarChar).Value = "Good Day from Hippo Happenings! <br/><br/> Congratulations! The friend request submited for user '" + username + "' has been approved by this user. You are now friends! ";
    //        cmd.Parameters.Add("@subject", SqlDbType.NVarChar).Value = "Friend Request Approved!";
    //        cmd.Parameters.Add("@from", SqlDbType.Int).Value = 6;
    //        cmd.Parameters.Add("@to", SqlDbType.Int).Value = friendID;
    //        cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":"));
    //        cmd.ExecuteNonQuery();

    //        conn.Close();

    //    }
    //    catch (Exception ex)
    //    {

    //    }

    //    return int.Parse(i);

    //}

    //[Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
    //public int ArchiveMessage(string ID)
    //{
    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //    dat.Execute("UPDATE UserMessages SET Live='False' WHERE ID="+ID);

    //   return 0;
    //}

    protected void UploadPhoto(object sender, EventArgs e)
    {

        if (PictureUpload.HasFile)
        {
            HttpCookie cookie = Request.Cookies["BrowserDate"];

            Data d = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            if (!System.IO.Directory.Exists(MapPath(".").ToString() + "/UserFiles/" + Session["UserName"].ToString()))
            {
                System.IO.Directory.CreateDirectory(MapPath(".").ToString() + "/UserFiles/" + Session["UserName"].ToString());
            }

            if (!System.IO.Directory.Exists(MapPath(".").ToString() + "/UserFiles/" + Session["UserName"].ToString() +
                "/Profile/"))
            {
                System.IO.Directory.CreateDirectory(MapPath(".").ToString() + "/UserFiles/" + Session["UserName"].ToString() +
                "/Profile/");
            }

            char[] delim = { '.' };
            string[] tokens = PictureUpload.FileName.Split(delim);

            System.Drawing.Image img = System.Drawing.Image.FromStream(PictureUpload.PostedFile.InputStream);

            SaveThumbnail(img, false, MapPath(".").ToString() + "/UserFiles/" + Session["UserName"].ToString() +
                "/Profile/" + PictureUpload.FileName, "image/" + tokens[1].ToLower());

            //PictureUpload.SaveAs(MapPath(".").ToString() + "/UserFiles/" + Session["UserName"].ToString() +
            //    "/Profile/" + PictureUpload.FileName);
            System.Drawing.Image theimg = System.Drawing.Image.FromFile(Server.MapPath(".") + "/UserFiles/" + Session["UserName"].ToString() +
                "/Profile/" + PictureUpload.FileName);
            FriendImage.ImageUrl = "UserFiles/" + Session["UserName"].ToString() + "/Profile/" +
                PictureUpload.FileName;
            FriendImage.Width = theimg.Width;
            FriendImage.Height = theimg.Height;
            Session["ProfilePicture"] = PictureUpload.FileName;

            d.Execute("UPDATE Users SET ProfilePicture= '" + PictureUpload.FileName + "' WHERE User_ID=" + Session["User"].ToString());
        }
    }

    protected void Save(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        try
        {
            Data d = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

            if (d.ValidateEmail(EmailTextBox.Text))
            {
                //This is a flag to check whether the ads on the site need to be reset. 
                //They will need to be reset if the user changed the location or ad categories
                bool resetAds = false;

                string USER_ID = Session["User"].ToString();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
                conn.Open();
                SqlCommand cmd;
               

                string nameStart = "";
                string nameEnd = "";
                if (LastNameTextBox.Text.Trim() != "")
                    nameStart = ", LastName='" + LastNameTextBox.Text.Trim().Replace("'", "''") + "' ";

                if(FirstNameTextBox.Text.Trim() != "")
                    nameEnd =  ", FirstName='" + FirstNameTextBox.Text.Trim().Replace("'", "''") + "' ";

                cmd = new SqlCommand("UPDATE Users SET Weekly='" + WeeklyCheckBox.Checked.ToString() + "', Email=@email, PhoneNumber=@phone " +
                    ", PhoneProvider=@provider " + nameStart + nameEnd + "WHERE User_ID=@id ", conn);
                
                cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = EmailTextBox.Text;
                cmd.Parameters.Add("@phone", SqlDbType.NVarChar).Value = d.RemoveNoneNumbers(PhoneTextBox.Text);
                cmd.Parameters.Add("@provider", SqlDbType.Int).Value = ProviderDropDown.SelectedValue;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = USER_ID;
                cmd.ExecuteNonQuery();

                string prefs = "0";

                if (TextingCheckBoxList.Items[0].Selected)
                    prefs += "1";
                //if (TextingCheckBoxList.Items[1].Selected)
                //    prefs += "2";

                string emailPrefs = "0";

                if (EmailCheckList.Items[0].Selected)
                    emailPrefs += "1";
                //if (EmailCheckList.Items[1].Selected)
                //    emailPrefs += "2";

                if (EmailUserCheckList1.Items[0].Selected)
                    emailPrefs += EmailUserCheckList1.Items[0].Value;
                if (EmailUserCheckList1.Items[1].Selected)
                    emailPrefs += EmailUserCheckList1.Items[1].Value;
                if (EmailUserCheckList1.Items[2].Selected)
                    emailPrefs += EmailUserCheckList1.Items[2].Value;
                if (EmailUserCheckList1.Items[3].Selected)
                    emailPrefs += EmailUserCheckList1.Items[3].Value;

                if (EmailUserCheckList2.Items[0].Selected)
                    emailPrefs += EmailUserCheckList2.Items[0].Value;
                if (EmailUserCheckList2.Items[1].Selected)
                    emailPrefs += EmailUserCheckList2.Items[1].Value;
                //if (EmailUserCheckList2.Items[2].Selected)
                //    emailPrefs += EmailUserCheckList2.Items[2].Value;

                string calendarPrefs = "";
                if (PublicPrivateCheckList.SelectedValue != null)
                    if (PublicPrivateCheckList.SelectedValue != "")
                        calendarPrefs = PublicPrivateCheckList.SelectedValue;

                string commPrefs = "";
                if (CommunicationPrefsRadioList.SelectedValue != null)
                    if (CommunicationPrefsRadioList.SelectedValue != "")
                        commPrefs = CommunicationPrefsRadioList.SelectedValue;

                string commentsPrefs = "";
                if (CommentsRadioList.SelectedValue != null)
                    if (CommentsRadioList.SelectedValue != "")
                        commentsPrefs = CommentsRadioList.SelectedValue;

                string pollPrefs = "";
                //if (PollRadioList.SelectedValue != null)
                //    if (PollRadioList.SelectedValue != "")
                //        pollPrefs = PollRadioList.SelectedValue;

                string onoff = "";

                //if (CategoriesOnOffRadioList.SelectedValue != null)
                //    if (CategoriesOnOffRadioList.SelectedValue != "")
                //        onoff = CategoriesOnOffRadioList.SelectedValue;

                
                string recommendPrefs = "";
                if (RecommendationsCheckList.Items[0].Selected)
                    recommendPrefs += "1";
                if (RecommendationsCheckList.Items[1].Selected)
                    recommendPrefs += "2";
                if (RecommendationsCheckList.Items[2].Selected)
                    recommendPrefs += "3";

                DataView usersPrevPrefs = d.GetDataDV("SELECT * FROM UserPreferences WHERE UserID="+Session["User"].ToString());

                cmd = new SqlCommand("UPDATE UserPreferences SET MajorCity=@major, Sex=@sex, Location=@location, CalendarPrivacyMode=@calendarmode " +
                    ", CommunicationPrefs=@commPrefs, TextingPrefs=@textprefs, EmailPrefs=@email, " +
                    " CommentsPreferences=@comments, PollPreferences=@poll, " +
                    " CatCountry=@catCountry, CatState=@catState, CatCity=@catCity, RecommendationPrefs=@rPrefs, CatZip=@catZip WHERE UserID=@id ", conn);
                //cmd.Parameters.Add("@age", SqlDbType.NVarChar).Value = AgeTextBox.Text;
                cmd.Parameters.Add("@sex", SqlDbType.NVarChar).Value = SexTextBox.Text;
                cmd.Parameters.Add("@location", SqlDbType.NVarChar).Value = LocationTextBox.Text;
                if (recommendPrefs != "")
                    cmd.Parameters.Add("@rPrefs", SqlDbType.Int).Value = recommendPrefs;
                else
                    cmd.Parameters.Add("@rPrefs", SqlDbType.Int).Value = DBNull.Value;

                if (CountryDropDown.SelectedValue == "223")
                    cmd.Parameters.Add("@major", SqlDbType.NVarChar).Value = MajorCityDrop.SelectedValue;
                else
                    cmd.Parameters.Add("@major", SqlDbType.NVarChar).Value = DBNull.Value;

                //if (onoff != "")
                //    cmd.Parameters.Add("@onoff", SqlDbType.Int).Value = onoff;
                //else
                //    cmd.Parameters.Add("@onoff", SqlDbType.Int).Value = DBNull.Value;
                if (calendarPrefs != "")
                    cmd.Parameters.Add("@calendarmode", SqlDbType.Int).Value = calendarPrefs;
                else
                    cmd.Parameters.Add("@calendarmode", SqlDbType.Int).Value = DBNull.Value;
                if (commPrefs != "")
                    cmd.Parameters.Add("@commPrefs", SqlDbType.Int).Value = commPrefs;
                else
                    cmd.Parameters.Add("@commPrefs", SqlDbType.Int).Value = DBNull.Value;
                if (commentsPrefs != "")
                    cmd.Parameters.Add("@comments", SqlDbType.Int).Value = commentsPrefs;
                else
                    cmd.Parameters.Add("@comments", SqlDbType.Int).Value = DBNull.Value;
                if (pollPrefs != "")
                    cmd.Parameters.Add("@poll", SqlDbType.Int).Value = pollPrefs;
                else
                    cmd.Parameters.Add("@poll", SqlDbType.Int).Value = DBNull.Value;
                //cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = "";
                //cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = "";
                
                
                //if (BillCountryDropDown.SelectedValue != "-1")
                //{
                //    cmd.Parameters.Add("@country", SqlDbType.Int).Value = BillCountryDropDown.SelectedValue;

                //    string state = "";
                //    if (BillStateDropPanel.Visible)
                //        state = BillStateDropDown.SelectedItem.Text;
                //    else
                //        state = BillStateTextBox.Text;

                //    cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = state;
                //}
                //else
                //{
                //    cmd.Parameters.Add("@country", SqlDbType.Int).Value = DBNull.Value;
                //    cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = DBNull.Value;
                //}

                if (CountryDropDown.SelectedValue != "-1")
                {
                    cmd.Parameters.Add("@catCountry", SqlDbType.Int).Value = CountryDropDown.SelectedValue;
                    Session["UserCountry"] = CountryDropDown.SelectedValue;

                    string state = "";
                    if (StateDropDownPanel.Visible)
                        state = StateDropDown.SelectedItem.Text;
                    else
                        state = StateTextBox.Text;

                    if (state != "")
                    {
                        cmd.Parameters.Add("@catState", SqlDbType.NVarChar).Value = state;

                    }
                    else
                        cmd.Parameters.Add("@catState", SqlDbType.NVarChar).Value = DBNull.Value;
                    Session["UserState"] = state;
                    if (CityTextBox.Text != "")
                    {
                        cmd.Parameters.Add("@catCity", SqlDbType.NVarChar).Value = CityTextBox.Text.Trim();

                    }
                    else
                        cmd.Parameters.Add("@catCity", SqlDbType.NVarChar).Value = DBNull.Value;
                    Session["UserCity"] = CityTextBox.Text.Trim();

                    cmd.Parameters.Add("@catZip", SqlDbType.NVarChar).Value = CatZipTextBox.Text.Trim();

                    Session["UserZip"] = CatZipTextBox.Text.Trim();
                }
                else
                {
                    cmd.Parameters.Add("@catCountry", SqlDbType.Int).Value = DBNull.Value;
                    cmd.Parameters.Add("@catState", SqlDbType.NVarChar).Value = DBNull.Value;
                    cmd.Parameters.Add("@catCity", SqlDbType.NVarChar).Value = DBNull.Value;
                    cmd.Parameters.Add("@catZip", SqlDbType.NVarChar).Value = DBNull.Value;
                }

                //cmd.Parameters.Add("@zip", SqlDbType.NVarChar).Value = ZipTextBox.Text;
                cmd.Parameters.Add("@textprefs", SqlDbType.Int).Value = prefs;
                cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = emailPrefs;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = USER_ID;
                cmd.ExecuteNonQuery();



                CreateCategories(ref resetAds);



                cmd = new SqlCommand("DELETE FROM UserVenues WHERE UserID=@user", conn);
                cmd.Parameters.Add("@user", SqlDbType.Int).Value = USER_ID;
                cmd.ExecuteNonQuery();

                CheckBoxList VenueCheckBoxes = (CheckBoxList)VenuesChecksPanel.FindControl("VenueCheckBoxes");

                if (VenueCheckBoxes != null)
                {
                    for (int i = 0; i < VenueCheckBoxes.Items.Count; i++)
                    {
                        if (VenueCheckBoxes.Items[i].Selected)
                        {
                            cmd = new SqlCommand("INSERT INTO UserVenues (UserID, VenueID) VALUES(@user, @cat)", conn);
                            cmd.Parameters.Add("@user", SqlDbType.Int).Value = USER_ID;
                            cmd.Parameters.Add("@cat", SqlDbType.Int).Value = VenueCheckBoxes.Items[i].Value;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                conn.Close();
                Encryption encrypt = new Encryption();
                ErrorLabel.Text = "Your profile has been updated!";
                BottomErrorLabel.Text = "Your profile has been updated!";
                Session["Message"] = "Your profile has been updated!";               
            }
            else
            {
                ErrorLabel.Text = "Email is invalid";
                BottomErrorLabel.Text = "Email is invalid";
                Encryption encrypt = new Encryption();
                Session["Message"] = "Email is invalid";
            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString();
        }
    }

    protected void CreateCategories(ref bool resetAds)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        string categories = "";
        string message = "";
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));


        //First delete user's Ad Categories and Event categories
        dat.Execute("DELETE FROM UserEventCategories WHERE UserID=" + Session["User"].ToString());
        dat.Execute("DELETE FROM UserCategories WHERE UserID=" + Session["User"].ToString());

        //Second add the categories back into the user's account
        //GetCategoriesFromTree(ref CategoryTree, true, ref resetAds);
        GetCategoriesFromTree(ref RadTreeView1, false, ref resetAds);
        //GetCategoriesFromTree(ref RadTreeView2, true, ref resetAds);
        GetCategoriesFromTree(ref RadTreeView3, false, ref resetAds);
    }

    protected void GetCategoriesFromTree(ref Telerik.Web.UI.RadTreeView CategoryTree, bool isAd, ref bool resetAds)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        string categories = "";
        Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));

        List<Telerik.Web.UI.RadTreeNode> list = (List<Telerik.Web.UI.RadTreeNode>)CategoryTree.GetAllNodes();

        foreach (Telerik.Web.UI.RadTreeNode node in list)
        {
            DataView dv = dat.GetDataDV("SELECT * FROM UserCategories WHERE CategoryID=" + node.Value +
                " AND UserID=" + Session["User"].ToString());
            if (node.Checked)
            {
                if (isAd)
                {
                    dat.Execute("INSERT INTO UserCategories (CategoryID, UserID) VALUES("
                                + node.Value + "," + Session["User"].ToString() + ")");

                    if (dv.Count == 0)
                        resetAds = true;
                }
                else
                {
                    dat.Execute("INSERT INTO UserEventCategories (CategoryID, UserID) VALUES("
                                + node.Value + "," + Session["User"].ToString() + ")");

                }
            }
            else
            {
                if (isAd)
                {
                    if (dv.Count != 0)
                        resetAds = true;
                }
            }
        }
    }

    //protected void ChangeBillState(object sender, EventArgs e)
    //{
    //    HttpCookie cookie = Request.Cookies["BrowserDate"];
    //    Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
    //    DataSet ds = dat.GetData("SELECT * FROM State WHERE country_id=" + BillCountryDropDown.SelectedValue);

    //    bool isTextBox = false;
    //    if (ds.Tables.Count > 0)
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            BillStateDropPanel.Visible = true;
    //            BillStateTextPanel.Visible = false;
    //            BillStateDropDown.DataSource = ds;
    //            BillStateDropDown.DataTextField = "state_2_code";
    //            BillStateDropDown.DataValueField = "state_id";
    //            BillStateDropDown.DataBind();
    //        }
    //        else
    //            isTextBox = true;
    //    else
    //        isTextBox = true;

    //    if (isTextBox)
    //    {
    //        BillStateTextPanel.Visible = true;
    //        BillStateDropPanel.Visible = false;
    //    }
    //}

        private static ImageCodecInfo GetEncoderInfo(string mimeType)
    {
        // Get image codecs for all image formats 
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

        // Find the correct image codec
        ImageCodecInfo a = codecs[0];
        for (int i = 0; i < codecs.Length; i++)
        {
            if (codecs[i].MimeType == mimeType)
            {
                return codecs[i];

            }
            if (codecs[i].MimeType == "jpeg")
                a = codecs[i];
        }
        return a;
    }

    private void SaveThumbnail(System.Drawing.Image theimage, bool isRotator, string path, string typeS)
    {
        double width = double.Parse(theimage.Width.ToString());
        double height = double.Parse(theimage.Height.ToString());

        if (width > height)
        {
            if (width <= 150)
            {

            }
            else
            {
                double dividor = double.Parse("150.00") / double.Parse(width.ToString());
                width = double.Parse("150.00");
                height = height * dividor;
            }
        }
        else
        {
            if (width == height)
            {
                width = double.Parse("150.00");
                height = double.Parse("150.00");
            }
            else
            {
                double dividor = double.Parse("150.00") / double.Parse(height.ToString());
                height = double.Parse("150.00");
                width = width * dividor;
            }
        }

        int w = int.Parse((Math.Round(decimal.Parse(width.ToString()))).ToString());
        int h = int.Parse((Math.Round(decimal.Parse(height.ToString()))).ToString());

        System.Drawing.Bitmap bmpResized = new System.Drawing.Bitmap(theimage, w, h);


        bmpResized.Save(path);
    }

    public static void SaveJpeg(string path, System.Drawing.Image img, int quality, string typeS)
    {
        if (quality < 0 || quality > 100)
            throw new ArgumentOutOfRangeException("quality must be between 0 and 100.");


        // Encoder parameter for image quality 
        EncoderParameter qualityParam =
            new EncoderParameter(Encoder.Quality, quality);
        // Jpeg image codec 
        ImageCodecInfo jpegCodec = GetEncoderInfo(typeS);

        EncoderParameters encoderParams = new EncoderParameters(1);
        encoderParams.Param[0] = qualityParam;

        img.Save(path, jpegCodec, encoderParams);
    }

    private bool EmptyCallBack()
    {
        return false;
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

                RequiredFieldValidator2.Visible = false;

            }
            else
                isTextBox = true;
        else
            isTextBox = true;

        if (isTextBox)
        {
            RequiredFieldValidator2.Visible = true;

            StateTextBoxPanel.Visible = true;
            StateDropDownPanel.Visible = false;
        }

        if (CountryDropDown.SelectedValue == "223")
        {
            CityDropDownPanel.Visible = true;
            ChangeCity(StateDropDown, new EventArgs());
            StateDropDown.AutoPostBack = true;
            ZipValidator.Visible = true;
        }
        else
        {
            CityDropDownPanel.Visible = false;
            StateDropDown.AutoPostBack = false;
            ZipValidator.Visible = false;
        }
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

                CityDropDownPanel.Visible = true;
                MajorCityDrop.DataSource = ds;
                MajorCityDrop.DataTextField = "MajorCity";
                MajorCityDrop.DataValueField = "MCID";
                MajorCityDrop.DataBind();
            }
            else
            {
                CityDropDownPanel.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.ToString();
        }
    }
}
