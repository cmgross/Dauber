using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace MFP
{
    public class Scrape
    {
        public static bool IsPublic(string clientUserName)
        {
            var document = DownloadDiary(clientUserName);
            HtmlNode primaryDiv = document.DocumentNode.Descendants("div").FirstOrDefault(d => d.Attributes.Contains("class") && d.Attributes["class"].Value == "block-1");
            if (primaryDiv == null) return true;
            return !primaryDiv.InnerHtml.Contains("This user is not allowing others to view his or her diary");
        }

        public static Macros GetMacros(string clientUserName, string date)
        {
            var document = DownloadDiary(clientUserName, date);
            HtmlNode footer = document.DocumentNode.Descendants("tfoot").FirstOrDefault();
            if (footer == null) return new Macros();
            //<tfoot>
            //  <tr>
            //    <td class="first">TOTAL:</td>
            //    <td>2,708</td> Calories [0]
            //    <td>273g</td> Carbs [1]
            //    <td>89g</td> Fat [2]
            //    <td>225g</td> Protein [3]
            //    <td>838mg</td> Cholest
            //    <td>1,848mg</td> Sodium 
            //    <td>50g</td> Sugars
            //    <td class="last">43g</td> Fiber
            //</tr>
            //</tfoot>

            HtmlNode totalsRow = footer.Descendants("tr").First();
            HtmlNodeCollection cells = totalsRow.SelectNodes(".//td[not(@class='first') and not(@class='last')]");

            var macros = new Macros
            {
                Calories = cells[0].InnerText.Replace(",", string.Empty),
                Carbs = cells[1].InnerText.Replace("g", string.Empty),
                Fat = cells[2].InnerText.Replace("g", string.Empty),
                Protein = cells[3].InnerText.Replace("g", string.Empty)
            };
            return macros;
        }

        private static HtmlDocument DownloadDiary(string clientUserName, string date = null)
        {
            if (date == null) date = DateTime.Today.ToString("yyyy-MM-dd");
            string url = "http://www.myfitnesspal.com/reports/printable_diary/" + clientUserName + "?from=" + date + "&to=" + date;
            string results;

            using (var client = new WebClient())
            {
                client.Headers["User-Agent"] = "Mozilla/5.0 (Windows; U; MSIE 9.0; Windows NT 9.0; en-US)";
                results = client.DownloadString(url);
            }
            var document = new HtmlDocument();
            document.LoadHtml(results);
            return document;
        }
    }
}
