using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace InitChanger
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int nSiteNo = 326;
            bool bIsSuccess = false;
            DbAgent dbAgent = new DbAgent("Data Source=hitomi_downloader_GUI.ini;Version=3;UseUTF16Encoding=True;");
            Regex regex = new Regex("https\\:\\/\\/manatoki(?<no>[0-9]*)\\.net");

            using (DataTable tbNames = dbAgent.GetDataTable("SELECT * FROM Data;"))
            {
                foreach (DataRow row in tbNames.Rows)
                {
                    string sJsonData = row["value"].ToString();
                    Match match = regex.Match(sJsonData);
                    if(match.Success)
                    {
                        nSiteNo = Convert.ToInt32(match.Groups["no"].Value);
                        break;
                    }                    
                }
            }


          
            while (!bIsSuccess)
            {                
                try
                {
                    string sUrl = $"https://manatoki{nSiteNo}.net/";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sUrl);
                    request.Method = "GET";
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3";
                    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    request.Headers.Add("Accept-Language", "ko-KR,ko;q=0.8,en-US;q=0.6,en;q=0.4");
                    request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");

                    Console.WriteLine($"Connecting to {nSiteNo} ...");
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        if(response.StatusCode == HttpStatusCode.OK
                            && sUrl.Contains(response.ResponseUri.Host))
                        {
                            bIsSuccess = true;
                            Console.WriteLine(nSiteNo);
                        }
                        else
                            nSiteNo++;
                    }
                }
                catch(Exception ex)
                {
                    nSiteNo++;
                    Console.WriteLine(ex.Message);
                }
            }

            Console.WriteLine($"Found Site : https://manatoki{nSiteNo}.net/");


            

            using (DataTable tbData = dbAgent.GetDataTable("SELECT * FROM Data;"))
            {
                foreach(DataRow row in tbData.Rows)
                {
                    string sJsonData = row["value"].ToString();
                    string sReplaceJsonData = regex.Replace(sJsonData, $"https://manatoki{nSiteNo}.net");
                    Console.WriteLine(row["option"]);
                    QuerySet set = new QuerySet("UPDATE Data SET value = @value WHERE option = @option;");
                    set.AddParamters("@value", sReplaceJsonData);
                    set.AddParamters("@option", row["option"]);

                    dbAgent.ExecuteQuery(set);
                }
            }

            using(DataTable tbNames = dbAgent.GetDataTable("SELECT * FROM Names;"))
            {
                foreach (DataRow row in tbNames.Rows)
                {
                    string sJsonData = row["value"].ToString();
                    string sReplaceJsonData = regex.Replace(sJsonData, $"https://manatoki{nSiteNo}.net");
                    Console.WriteLine(row["option"]);                    
                    QuerySet set = new QuerySet("UPDATE Names SET value = @value WHERE option = @option;");
                    set.AddParamters("@value", sReplaceJsonData);
                    set.AddParamters("@option", row["option"]);

                    dbAgent.ExecuteQuery(set);
                }
            }

            Console.WriteLine("Complete!");
        }
    }
}
