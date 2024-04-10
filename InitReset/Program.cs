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
            DbAgent dbAgent = new DbAgent($"Data Source=hitomi_downloader_GUI.ini;Version=3;UseUTF16Encoding=True;");
            Regex regex = new Regex("https\\:\\/\\/manatoki[0-9]*\\.net");

            using (DataTable tbData = dbAgent.GetDataTable("SELECT * FROM Data;"))
            {
                foreach (DataRow row in tbData.Rows)
                {
                    string sJsonData = row["value"].ToString();
                   
                    ModelNameItem item = JsonSerializer.Deserialize<ModelNameItem>(sJsonData);

                    QuerySet set = new QuerySet("SELECT * FROM Names WHERE option = @option;");
                    set.AddParamters("@option", item.Uid); 
                    using(DataTable tbName = dbAgent.GetDataTable(set))
                    {
                        if(tbName != null && tbName.Rows.Count > 0)
                        {
                            DataRow rowName = tbName.Rows[0];
                            string sNames = rowName["value"].ToString();
                            string [] aryNames = JsonSerializer.Deserialize<string[]>(sNames);
                            foreach(string sName in aryNames)
                            {
                                Console.WriteLine(sName);
                            }

                        }
                    }
                }
            }
            Console.WriteLine("Complete!");
        }
    }
}
