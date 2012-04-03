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

public partial class UserLogin : Telerik.Web.UI.RadAjaxPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int i = 0;

        HtmlHead head = (HtmlHead)Page.Header;


        HtmlMeta kw = new HtmlMeta();
        kw.Name = "keywords";
        kw.Content = "create user account search view events, venues ads neighborhood community interest groups";

        head.Controls.AddAt(0, kw);

        HtmlMeta hm = new HtmlMeta();
        hm.Name = "description";
        hm.Content = "create a user account to search and view events, venues and ads in your neighborhood from your community interest groups";

        head.Controls.AddAt(0, hm);
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        
            //Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            //Ads1.DATA_SET = dat.RetrieveAllAds(false);
            //Ads1.MAIN_AD_DATA_SET = dat.RetrieveAllAds(true);
       
    }

    protected void MakeItSo(object sender, System.EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["BrowserDate"];
        if (cookie == null)
        {
            cookie = new HttpCookie("BrowserDate");
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.AddDays(22);
            Response.Cookies.Add(cookie);
        }
        try
        {
            Encryption encrypt = new Encryption();
            Data dat = new Data(DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")));
            if (Page.IsValid)
            {
                if (DBConnection(UserNameTextBox.Text.Trim(), encrypt.encrypt(PasswordTextBox.Text.Trim())))
                {
                    string groups = "";
                    SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
                    myConn.Open();
                    SqlCommand myCmd = new SqlCommand("SELECT U.Password, U.User_ID, UP.CatCountry, UP.CatState, UP.CatCity, U.UserName FROM Users U, UserPreferences UP WHERE U.User_ID=UP.UserID AND U.UserName=@UserName", myConn);
                    myCmd.CommandType = CommandType.Text;
                    myCmd.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = UserNameTextBox.Text.Trim();
                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(myCmd);
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        groups = "User, Admin";
                        //FormsAuthentication.RedirectFromLoginPage(txtUserName.Text, false);
                        FormsAuthenticationTicket authTicket =
                            new FormsAuthenticationTicket(1, ds.Tables[0].Rows[0]["User_ID"].ToString(),
                                          DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")),
                                          DateTime.Parse(cookie.Value.ToString().Replace("%20", " ").Replace("%3A", ":")).AddMinutes(60),
                                          false, groups);

                        // Now encrypt the ticket.
                        string encryptedTicket =
                          FormsAuthentication.Encrypt(authTicket);
                        // Create a cookie and add the encrypted ticket to the
                        // cookie as data.
                        HttpCookie authCookie =
                                     new HttpCookie(FormsAuthentication.FormsCookieName,
                                                    encryptedTicket);

                        // Add the cookie to the outgoing cookies collection.
                        Response.Cookies.Add(authCookie);
                        dat.WhatHappensOnUserLogin(ds);

                        string redirectTo = "User.aspx";
                        if (Session["RedirectTo"] != null)
                            redirectTo = Session["RedirectTo"].ToString();

                        Response.Redirect(redirectTo, false);
                    }
                    else
                    {

                        StatusLabel.Text = "Invalid Login, please try again!";
                    }
                }
                else
                {
                    StatusLabel.Text = "Invalid Login, please try again!";
                }
            }
        }
        catch (Exception ex)
        {
            StatusLabel.Text = ex.ToString();
        }

    }
    private bool DBConnection(string txtUser, string txtPass)
    {
        SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
        SqlCommand myCmd = new SqlCommand("SELECT Password, UserName, User_ID FROM Users WHERE UserName=@UserName", myConn);
        myCmd.CommandType = CommandType.Text;
        myCmd.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = txtUser;

        try
        {

            myConn.Open();
            SqlDataAdapter da = new SqlDataAdapter(myCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Password"].ToString() != txtPass)
                    {
                        StatusLabel.Text = "Invalid Login, please try again!";
                        return false;
                    }
                    else
                    {
                        myConn.Close();
                        Session["User"] = ds.Tables[0].Rows[0]["User_ID"].ToString();
                        Session["UserName"] = ds.Tables[0].Rows[0]["UserName"].ToString();
                        return true;
                    }
                }
                else
                {
                    StatusLabel.Text = "Invalid Login, please try again!";
                    return false;
                }
            }
            else
            {
                StatusLabel.Text = "Invalid Login, please try again!";
                return false;
            }



        }
        catch (Exception ex)
        {
            StatusLabel.Text = ex + "Error Connecting to the database";
            return false;
        }

    }

}