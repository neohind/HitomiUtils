using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace InitReset
{
    public class QuerySet
    {
        private List<SQLiteParameter> m_aryParameters = new List<SQLiteParameter>();

        public string Query 
        { 
            get; 
            set; 
        }

        public SQLiteParameter[] Parameters
        {
            get
            {
                return m_aryParameters.ToArray();
            }
        }

        public QuerySet(string sQuery)
        {
            Query = sQuery;
        }

        public void AddParamters(string sName, object value)
        {
            m_aryParameters.Add(new SQLiteParameter(sName, value));
        }
    }
}
