using InitReset.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InitReset
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
                return;

            FileInfo iniFileinfo = new FileInfo(args[0]);
            if (!iniFileinfo.Exists)
            {
                return;
            }            

            DbAgent dbAgent = new DbAgent($"Data Source={iniFileinfo.FullName};Version=3;UseUTF16Encoding=True;");
            Regex regex = new Regex("https\\:\\/\\/manatoki[0-9]*\\.net");

            using (DataTable tbData = dbAgent.GetDataTable("SELECT * FROM Data;"))
            {
                foreach (DataRow row in tbData.Rows)
                {
                    string sJsonData = row["value"].ToString();

                   
                    ModelNameItem item = JsonSerializer.Deserialize<ModelNameItem>(sJsonData);

                    item.Anime = false;
                    item.Artist = null;
                    item.Dir = "";
                    item.Done = false;
                    item.FileSize = 0;
                    item.LabelColor = null;
                    item.Lazy = true;
                    item.Music = false;
                    item.PBar[0] = 0;
                    item.PBar[1] = 0;
                    item.PLazy = 0;
                    item.Title = $"대기중... {item.GalNum}";

                    string sNewJsonData = JsonSerializer.Serialize<ModelNameItem>(item);

                    QuerySet set = new QuerySet("UPDATE Data SET value = @value WHERE option = @option;");
                    set.AddParamters("@option", row["option"]);
                    set.AddParamters("@value", sNewJsonData);
                    dbAgent.ExecuteQuery(set);

                    set = new QuerySet("UPDATE Names SET value = '[]' WHERE option = @option;");
                    set.AddParamters("@option", row["option"]);
                    dbAgent.ExecuteQuery(set);
                }
            }
            Console.WriteLine("Complete!");
        }
    }
}
