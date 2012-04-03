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

public partial class Blog_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Data dat = new Data(DateTime.Now);

        Page.Title = "Hippo Blog - The Portal To What's Happenings<";

        DataView dvFeatured = dat.GetDataDV("SELECT * FROM HippoBlogContent WHERE Featured = 'True' ");

        if (dvFeatured.Count > 0)
        {
            FeaturedLiteral.Text = "<table>" +
                "<tr>" +
                   " <td class=\"ContentLeft\" valign=\"top\">" +
                   "     <img src=\"../NewImages/Cloud.png\" />" +
                   " </td>" +
                   " <td valign=\"top\">" +
                    "     <h1 class=\"FeaturedTitle\">" + dvFeatured[0]["Title"].ToString() + "</h1>" +
                    
                   " </td>" +
               " </tr>" +
                "<tr>" +
                 "   <td class=\"ContentLeft\" align=\"center\">" +
                 "       2pm <br />" +
                 "       Wed <br />" +
                 "       Mar. <br />" +
                 "       22nd <br />" +
                  "  </td>" +
                  "  <td class=\"FeaturedContent\">" + "<img src=\"" + dvFeatured[0]["HomeImage"].ToString() + "\" />" + dvFeatured[0]["HomePageSummary"].ToString() +
                  "  </td>" +
                "</tr>" +
                "<tr>" +
                 "   <td>" +
                 "   </td>" +
                  "  <td>" +
                  "      Continue Reading <a class=\"FeaturedLink\" href=\"" +
                  dat.MakeNiceNameFive(dvFeatured[0]["Title"].ToString()) + "_" +
                  dvFeatured[0]["ID"].ToString() + "_Blog\">" +
                  dvFeatured[0]["Title"].ToString() + "</a>" +
                  "  </td>" +
               " </tr>" +
           " </table>";
        }

        DataView dv = dat.GetDataDV("SELECT * FROM HippoBlogContent WHERE Featured = 'False' ORDER BY PostDate DESC");

        foreach (DataRowView row in dv)
        {
            RestOfArticlesLiteral.Text += "<table class=\"OneArticle\">" +
                "<tr>" +
                   " <td class=\"ContentLeft\" valign=\"top\">" +
                   "     <img src=\"../NewImages/Cloud.png\" />" +
                   " </td>" +
                   " <td valign=\"top\">" +
                    "     <h1 class=\"Title\">" + row["Title"].ToString() + "</h1>" +
                    
                   " </td>" +
               " </tr>" +
                "<tr>" +
                 "   <td class=\"ContentLeft\" align=\"center\">" +
                 "       2pm <br />" +
                 "       Wed <br />" +
                 "       Mar. <br />" +
                 "       22nd <br />" +
                  "  </td>" +
                  "  <td class=\"Content\">" + "<img src=\"" + row["HomeImage"].ToString() + "\" />" + row["HomePageSummary"].ToString() +
                  "  </td>" +
                "</tr>" +
                "<tr>" +
                 "   <td>" +
                 "   </td>" +
                  "  <td>" +
                  "      Continue Reading <a class=\"Title\" href=\"" +
                  dat.MakeNiceNameFive(row["Title"].ToString()) + "_" +
                  row["ID"].ToString() + "_Blog\">" +
                  row["Title"].ToString() + "</a>" +
                  "  </td>" +
               " </tr>" +
           " </table>";
        }
    }
}
