using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.IO;
using System.Data.Sql;
using System.Data.SqlClient;


    public class RewriteHttpModule : IHttpModule
    {

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new System.EventHandler(this.context_BeginRequest);
        }

        public void Dispose()
        {

        }

        public static string stripString(string strIn)
        {
            strIn = strIn.Replace(" ", "+");
            strIn = Regex.Replace(strIn, @"[^\w\.\+/]", "");
            return strIn;
        }

        private void context_BeginRequest(object sender, EventArgs e)
        {
            HttpRequest request = ((HttpApplication)(sender)).Request;
            HttpContext context = ((HttpApplication)(sender)).Context;
            HttpResponse response = ((HttpApplication)(sender)).Response;

            string applicationPath = request.ApplicationPath;
            if ((applicationPath == "/"))
            {
                applicationPath = String.Empty;
            }
            string requestPath = request.Url.AbsolutePath.Substring(applicationPath.Length);
            


            DateTime isn = DateTime.Now;


            Data dat = new Data(isn);

            if (requestPath.ToLower() == "/blog/home")
            {
                context.RewritePath(applicationPath + "/Blog/home.aspx");
            }
            else if (requestPath.ToLower() == "/home")
            {
                context.RewritePath(applicationPath + "/home.aspx");
            }
            else if (requestPath.ToLower() == "/event-search")
            {
                context.RewritePath(applicationPath + "/eventsearch.aspx");
            }
            else if (requestPath.ToLower() == "/venue-search")
            {
                context.RewritePath(applicationPath + "/venuesearch.aspx");
            }
            else if (requestPath.ToLower() == "/ad-search")
            {
                context.RewritePath(applicationPath + "/adsearch.aspx");
            }
            else if (requestPath.ToLower() == "/trip-search")
            {
                context.RewritePath(applicationPath + "/tripsearch.aspx");
            }
            else if (requestPath.ToLower() == "/about")
            {
                context.RewritePath(applicationPath + "/about.aspx");
            }
            else if (requestPath.ToLower() == "/add")
            {
                context.RewritePath(applicationPath + "/add.aspx");
            }
            else if (requestPath.ToLower() == "/blog-event")
            {
                context.RewritePath(applicationPath + "/blogevent.aspx");
            }
            else if (requestPath.ToLower() == "/complete-registration")
            {
                context.RewritePath(applicationPath + "/completeregistration.aspx");
            }
            else if (requestPath.ToLower() == "/contact-us")
            {
                context.RewritePath(applicationPath + "/contactus.aspx");
            }
            else if (requestPath.ToLower() == "/edit-event")
            {
                context.RewritePath(applicationPath + "/editevent.aspx");
            }
            else if (requestPath.ToLower() == "/enter-event")
            {
                context.RewritePath(applicationPath + "/enterevent.aspx");
            }
            else if (requestPath.ToLower() == "/enter-trip-intro")
            {
                context.RewritePath(applicationPath + "/entertripintro.aspx");
            }
            else if (requestPath.ToLower() == "/enter-trip")
            {
                context.RewritePath(applicationPath + "/entertrip.aspx");
            }
            else if (requestPath.ToLower() == "/enter-locale")
            {
                context.RewritePath(applicationPath + "/entervenue.aspx");
            }
            else if (requestPath.ToLower() == "/feedback")
            {
                context.RewritePath(applicationPath + "/feedback.aspx");
            }
            else if (requestPath.ToLower() == "/feature")
            {
                context.RewritePath(applicationPath + "/feature.aspx");
            }
            else if (requestPath.ToLower() == "/hippo-points")
            {
                context.RewritePath(applicationPath + "/hippo-points.aspx");
            }
            else if (requestPath.ToLower() == "/hippo-points-tou")
            {
                context.RewritePath(applicationPath + "/hippopointstou.aspx");
            }
            else if (requestPath.ToLower() == "/post-bulletin")
            {
                context.RewritePath(applicationPath + "/postanad.aspx");
            }
            else if (requestPath.ToLower() == "/privacy-policy")
            {
                context.RewritePath(applicationPath + "/privacypolicy.aspx");
            }
            else if (requestPath.ToLower() == "/prohibited-ads")
            {
                context.RewritePath(applicationPath + "/prohibitedAds.aspx");
            }
            else if (requestPath.ToLower() == "/rate-experience")
            {
                context.RewritePath(applicationPath + "/rateexperience.aspx");
            }
            else if (requestPath.ToLower() == "/register")
            {
                context.RewritePath(applicationPath + "/register.aspx");
            }
            else if (requestPath.ToLower() == "/my-pages")
            {
                context.RewritePath(applicationPath + "/searchesandpages.aspx");
            }
            else if (requestPath.ToLower() == "/site-map")
            {
                context.RewritePath(applicationPath + "/sitemap.aspx");
            }
            else if (requestPath.ToLower() == "/terms-and-conditions")
            {
                context.RewritePath(applicationPath + "/termsandconditions.aspx");
            }
            else if (requestPath.ToLower() == "/my-account")
            {
                context.RewritePath(applicationPath + "/user.aspx");
            }
            else if (requestPath.ToLower() == "/my-calendar")
            {
                context.RewritePath(applicationPath + "/usercalendar.aspx");
            }
            else if (requestPath.ToLower() == "/login")
            {
                context.RewritePath(applicationPath + "/userlogin.aspx");
            }
            else if (requestPath.ToLower() == "/locale-calendar")
            {
                context.RewritePath(applicationPath + "/venuecalendar.aspx");
            }
            else
            {
                requestPath = stripString(requestPath.Replace("-", "+"));
                requestPath = dat.MakeNiceName(requestPath);

                string theInt = "";
                int theIntOut = 0;

                bool isPageNew = bool.Parse(ConfigurationSettings.AppSettings["isPageNew"].ToString());

                string addThis = "";
                if (isPageNew)
                    addThis = "";
                string origRequestPath = requestPath;
                requestPath = dat.Reverse(requestPath);
                if (requestPath.Length > 5)
                {
                    if (requestPath.Substring(0, 5) == "golB_")
                    {
                        int start = 5;

                        string theI = requestPath.Substring(start, 1);
                        while (int.TryParse(theI, out theIntOut))
                        {
                            theInt = theIntOut.ToString() + theInt;
                            theI = requestPath.Substring(++start, 1);
                        }

                        context.RewritePath(applicationPath + "/Blog/Article.aspx?ID=" + theInt);
                    }
                    else if (requestPath.Substring(0, 5) == "pirT_")
                    {
                        int start = 5;

                        string theI = requestPath.Substring(start, 1);
                        while (int.TryParse(theI, out theIntOut))
                        {
                            theInt = theIntOut.ToString() + theInt;
                            theI = requestPath.Substring(++start, 1);
                        }

                        context.RewritePath(applicationPath + "/Trip.aspx?ID=" + theInt);
                    }
                    else if (requestPath.Substring(0, 6) == "puorG_")
                    {
                        int start = 6;

                        string theI = requestPath.Substring(start, 1);
                        while (int.TryParse(theI, out theIntOut))
                        {
                            theInt = theIntOut.ToString() + theInt;
                            theI = requestPath.Substring(++start, 1);
                        }

                        context.RewritePath(applicationPath + "/Group.aspx?ID=" + theInt);
                    }
                    else if (requestPath.Substring(0, 6) == "tnevE_")
                    {
                        int start = 6;

                        string theI = requestPath.Substring(start, 1);
                        while (int.TryParse(theI, out theIntOut))
                        {
                            theInt = theIntOut.ToString() + theInt;
                            theI = requestPath.Substring(++start, 1);
                        }

                        context.RewritePath(applicationPath + "/Event" + addThis + ".aspx?EventID=" + theInt);
                    }
                    else if (requestPath.Substring(0, 3) == "dA_")
                    {
                        int start = 3;

                        string theI = requestPath.Substring(start, 1);
                        while (int.TryParse(theI, out theIntOut))
                        {
                            theInt = theIntOut.ToString() + theInt;
                            theI = requestPath.Substring(++start, 1);
                        }

                        context.RewritePath(applicationPath + "/Ad" + addThis + ".aspx?AdID=" + theInt);
                    }
                    else if (requestPath.Substring(0, 6) == "euneV_")
                    {
                        int start = 6;

                        string theI = requestPath.Substring(start, 1);
                        while (int.TryParse(theI, out theIntOut))
                        {
                            theInt = theIntOut.ToString() + theInt;
                            theI = requestPath.Substring(++start, 1);
                        }

                        context.RewritePath(applicationPath + "/Venue" + addThis + ".aspx?ID=" + theInt);
                    }
                    else if (requestPath.Length >= 7)
                    {
                        if (requestPath.Substring(0, 7).ToLower() == "dneirf_")
                        {
                            DataView dvName = dat.GetDataDV("SELECT * FROM Users WHERE UserName='" + dat.Reverse(requestPath.Substring(7, requestPath.Length - 7)) + "'");
                            context.RewritePath(applicationPath + "/Friend" + addThis + ".aspx?ID=" + dvName[0]["User_ID"].ToString());

                        }
                        else if (requestPath.Length >= 8)
                        {
                            if (requestPath.Substring(0, 8) == "tnevElC_")
                            {
                                string[] delimetr = { "CLHH" };
                                string[] tks = origRequestPath.Split(delimetr, StringSplitOptions.RemoveEmptyEntries);
                                string tmp = tks[1].Replace("_ClEvent","");
                                context.RewritePath(applicationPath + "/Event" + addThis + ".aspx?cl=true&EventID=" + tmp);
                            }
                            else if (requestPath.Length >= 9)
                            {
                                if (requestPath.Substring(0, 9).ToLower() == "radnelac_")
                                {
                                    int start = 9;

                                    string theI = requestPath.Substring(start, 1);
                                    while (int.TryParse(theI, out theIntOut))
                                    {
                                        theInt = theIntOut.ToString() + theInt;
                                        theI = requestPath.Substring(++start, 1);
                                    }

                                    context.RewritePath(applicationPath + "/VenueCalendar" + addThis + ".aspx?ID=" + theInt);
                                }
                                else if (requestPath.Length >= 11)
                                {
                                    if (requestPath.Substring(0, 11).ToLower() == "tnevepuorg_")
                                    {
                                        int start = 11;
                                        string theOnt = "";
                                        string theI = requestPath.Substring(start, 1);
                                        while (int.TryParse(theI, out theIntOut))
                                        {
                                            theInt = theIntOut.ToString() + theInt;
                                            theI = requestPath.Substring(++start, 1);
                                        }
                                        start = 11 + theInt.ToString().Length + 1;
                                        string theO = requestPath.Substring(start, 1);
                                        while (int.TryParse(theO, out theIntOut))
                                        {
                                            theOnt = theIntOut.ToString() + theOnt;
                                            theO = requestPath.Substring(++start, 1);
                                        }

                                        context.RewritePath(applicationPath + "/GroupEvent.aspx?ID=" + theInt + "&O=" + theOnt);
                                    }
                                    else if (requestPath.Length >= 12)
                                    {
                                        if (requestPath.Substring(0, 12) == "yrogetaC_dA_")
                                        {
                                            string name = requestPath.Substring(12, requestPath.Length - 12).Replace("-", " ");
                                            name = dat.Reverse(name);
                                            DataView dvName = dat.GetDataDV("SELECT * FROM AdCategories WHERE  REPLACE(REPLACE(Name, '/', ' '), '-', ' ') = '" + name + "'");
                                            context.RewritePath(applicationPath + "/AdCategorySearch" + addThis + ".aspx?ID=" + dvName[0]["ID"].ToString());

                                        }
                                        else if (requestPath.Length >= 14)
                                        {
                                            if (requestPath.Substring(0, 14) == "yrogetaC_pirT_")
                                            {
                                                string name = requestPath.Substring(14, requestPath.Length - 14).Replace("-", " ");
                                                name = dat.Reverse(name);
                                                DataView dvName = dat.GetDataDV("SELECT * FROM TripCategories WHERE REPLACE(REPLACE(Name, '/', ' '), '-', ' ') = '" + name + "'");
                                                context.RewritePath(applicationPath + "/TripCategorySearch.aspx?ID=" + dvName[0]["ID"].ToString());
                                            }
                                            else if (requestPath.Length >= 15)
                                            {
                                                if (requestPath.Substring(0, 15) == "yrogetaC_euneV_")
                                                {
                                                    string name = requestPath.Substring(15, requestPath.Length - 15).Replace("-", " ");
                                                    name = dat.Reverse(name);

                                                    DataView dvName = dat.GetDataDV("SELECT * FROM VenueCategories WHERE REPLACE(REPLACE(Name, '/', ' '), '-', ' ') = '" + name + "'");
                                                    context.RewritePath(applicationPath + "/VenueCategorySearch" + addThis + ".aspx?ID=" + dvName[0]["ID"].ToString());
                                                }
                                                else if (requestPath.Substring(0, 15) == "yrogetaC_tnevE_")
                                                {
                                                    string name = dat.Reverse(requestPath.Substring(15, requestPath.Length - 15).Replace("-", " "));
                                                    DataView dvName = dat.GetDataDV("SELECT * FROM EventCategories WHERE  REPLACE(REPLACE(Name, '/', ' '), '-', ' ') = '" + name + "'");
                                                    context.RewritePath(applicationPath + "/EventCategorySearch" + addThis + ".aspx?ID=" + dvName[0]["ID"].ToString());
                                                }
                                                else if (requestPath.Substring(0, 15) == "yrogetaC_puorG_")
                                                {
                                                    int start = 15;

                                                    string theI = requestPath.Substring(start, 1);
                                                    while (int.TryParse(theI, out theIntOut))
                                                    {
                                                        theInt = theIntOut.ToString() + theInt;
                                                        theI = requestPath.Substring(++start, 1);
                                                    }
                                                    context.RewritePath(applicationPath + "/GroupCategorySearch.aspx?ID=" + theInt);

                                                }
                                                else if (requestPath.Length >= 20)
                                                {
                                                    if (requestPath.Substring(0, 20) == "yrogetaC_tnevEpuorG_")
                                                    {
                                                        int start = 20;

                                                        string theI = requestPath.Substring(start, 1);
                                                        while (int.TryParse(theI, out theIntOut))
                                                        {
                                                            theInt = theIntOut.ToString() + theInt;
                                                            theI = requestPath.Substring(++start, 1);
                                                        }
                                                        context.RewritePath(applicationPath + "/GroupEventCategorySearch.aspx?ID=" + theInt);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }
    }

