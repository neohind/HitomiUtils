using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitChanger
{
    public class DbAgent
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        private DbAgent()
        { }

        public DbAgent(string sConnectionString)
        {
            this.ConnectionString = sConnectionString;
        }

        /// <summary>
        /// 연결 문자열을 설정하거나 돌려준다.
        /// </summary>
        public string ConnectionString
        {
            get;
            protected set;
        }

        public DataTable GetDataTable(QuerySet querySet)
        {
            return GetDataTable(querySet.Query, CommandType.Text, querySet.Parameters);
        }

        /// <summary>
        /// 문자열 쿼리를 실행 한뒤 DataTable 형식으로 값을 가져온다.
        /// </summary>
        /// <param name="query">문자열 기반의 쿼리</param>
        /// <returns>DataTable 형식의 결과값</returns>
        public DataTable GetDataTable(string query)
        {
            return GetDataTable(query, CommandType.Text, null);
        }

        public DataTable GetDataTable(string query, CommandType cmdType)
        {
            return GetDataTable(query, cmdType, null);
        }

        public DataTable GetDataTable(string query, params SQLiteParameter[] sqlParameters)
        {
            return GetDataTable(query, CommandType.Text, sqlParameters);
        }

        /// <summary>
        /// 쿼리를 실행 한뒤 DataTable 형식으로 값을 가져온다.
        /// </summary>
        /// <param name="set">쿼리값이 담긴 개체</param>
        /// <returns>DataTable 형식의 결과값</returns>
        public DataTable GetDataTable(string query, CommandType cmdType, params SQLiteParameter[] sqlParameters)
        {
            DataTable dtResult = new DataTable();

            SQLiteConnection connection = new SQLiteConnection(this.ConnectionString);
            SQLiteDataAdapter sqlAdapter = new SQLiteDataAdapter();
            SQLiteCommand sqlCommand = null;
            try
            {
                connection.Open();
                sqlCommand = new SQLiteCommand();
                sqlCommand.Connection = connection;
                sqlCommand.CommandText = query;
                sqlCommand.CommandType = cmdType;
                if (sqlParameters != null && sqlParameters.Length > 0)
                    sqlCommand.Parameters.AddRange(sqlParameters);

                sqlAdapter.SelectCommand = sqlCommand;
                sqlAdapter.Fill(dtResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                try
                {
                    if (sqlCommand != null)
                        sqlCommand.Dispose();

                    if (sqlAdapter != null)
                        sqlAdapter.Dispose();

                    if (connection != null && connection.State != ConnectionState.Closed)
                        connection.Close();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            if (dtResult != null)
            {
                log.Debug("DataTable result get Success");
            }
            return dtResult;
        }

        public DataSet GetDataSet(string query)
        {
            return GetDataSet(query, null);
        }

        public DataSet GetDataSet(string query, CommandType cmdType)
        {
            return GetDataSet(query, cmdType, null);
        }

        public DataSet GetDataSet(string query, params SQLiteParameter[] sqlParameters)
        {
            return GetDataSet(query, CommandType.Text, sqlParameters);
        }

        public DataSet GetDataSet(string query, CommandType cmdType, params SQLiteParameter[] sqlParameters)
        {
            DataSet dsResult = new DataSet();
            SQLiteConnection connection = new SQLiteConnection(this.ConnectionString);
            SQLiteDataAdapter sqlAdapter = new SQLiteDataAdapter();
            SQLiteCommand sqlCommand = null;
            try
            {
                connection.Open();
                sqlCommand = new SQLiteCommand();
                sqlCommand.CommandType = cmdType;
                sqlCommand.CommandText = query;
                sqlCommand.Connection = connection;
                if (sqlParameters != null)
                    sqlCommand.Parameters.AddRange(sqlParameters);

                sqlAdapter.SelectCommand = sqlCommand;

                sqlAdapter.Fill(dsResult);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                try
                {
                    if (sqlCommand != null)
                        sqlCommand.Dispose();

                    if (sqlAdapter != null)
                        sqlAdapter.Dispose();

                    if (connection != null && connection.State != ConnectionState.Closed)
                        connection.Close();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            if (dsResult != null)
            {
                log.Debug("DataTable result get Success");
            }
            return dsResult;
        }


        public int ExecuteQuery(QuerySet querySet)
        {
            return ExecuteQuery(querySet.Query, CommandType.Text, querySet.Parameters);
        }


        /// <summary>
        ///  쿼리를 실행 한다.
        /// </summary>
        /// <param name="set">쿼리값이 담긴 개체</param>
        /// <returns>실행 결과 변경된 Row 갯수</returns>
        public int ExecuteQuery(string query)
        {
            return ExecuteQuery(query, CommandType.Text, null);
        }

        /// <summary>
        ///  쿼리를 실행 한다.
        /// </summary>
        /// <param name="set">쿼리값이 담긴 개체</param>
        /// <returns>실행 결과 변경된 Row 갯수</returns>
        public int ExecuteQuery(string query, CommandType cmdType)
        {
            return ExecuteQuery(query, cmdType, null);
        }

        /// <summary>
        /// 쿼리를 실행 한다.
        /// </summary>
        /// <param name="aryQuerySet">실행할 쿼리들이 담긴 배열 개체</param>
        /// <returns>실행 결과 변경된 Row 갯수</returns>
        public int ExecuteQuery(string query, params SQLiteParameter[] sqlParameters)
        {
            return ExecuteQuery(query, CommandType.Text, sqlParameters);
        }

        /// <summary>
        /// 쿼리를 실행 한다.
        /// </summary>
        /// <param name="aryQuerySet">쿼리 셋 목록</param>
        /// <returns>실행 결과 변경된 Row 갯수</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2200:RethrowToPreserveStackDetails")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public int ExecuteQuery(string query, CommandType cmdType, params SQLiteParameter[] sqlParameters)
        {
            int affectedRows = 0;
            SQLiteConnection connection = new SQLiteConnection(this.ConnectionString);
            SQLiteCommand sqlCommand = null;
            string sCurrentExecuteQuery = string.Empty;
            try
            {
                connection.Open();
                sqlCommand = new SQLiteCommand();
                sqlCommand.CommandType = cmdType;
                sqlCommand.CommandText = query;
                sqlCommand.Connection = connection;
                if (sqlParameters != null && sqlParameters.Length > 0)
                {
                    foreach (SQLiteParameter param in sqlParameters)
                    {
                        sqlCommand.Parameters.Add(param);
                    }
                }

                affectedRows = sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                try
                {
                    if (sqlCommand != null)
                        sqlCommand.Dispose();

                    if (connection != null && connection.State != ConnectionState.Closed)
                        connection.Close();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            return affectedRows;
        }

        public T GetValue<T>(QuerySet query)
        {
            return GetValue<T>(query.Query, CommandType.Text, query.Parameters);
        }

        public T GetValue<T>(string query)
        {
            return GetValue<T>(query, CommandType.Text);
        }

        public T GetValue<T>(string query, CommandType cmdType)
        {
            return GetValue<T>(query, CommandType.Text, null);
        }

        public T GetValue<T>(string query, params SQLiteParameter[] sqlParameters)
        {
            return GetValue<T>(query, CommandType.Text, sqlParameters);
        }

        public T GetValue<T>(string query, CommandType cmdType, params SQLiteParameter[] sqlParameters)
        {
            T resultValue = default(T);
            SQLiteConnection connection = new SQLiteConnection(this.ConnectionString);
            SQLiteCommand sqlCommand = null;
            string sCurrentExecuteQuery = string.Empty;
            try
            {
                connection.Open();
                sqlCommand = new SQLiteCommand();
                sqlCommand.CommandType = cmdType;
                sqlCommand.CommandText = query;
                sqlCommand.Connection = connection;
                if (sqlParameters != null && sqlParameters.Length > 0)
                {
                    foreach (SQLiteParameter param in sqlParameters)
                    {
                        sqlCommand.Parameters.Add(param);
                    }
                }
                object value = sqlCommand.ExecuteScalar();
                if(value != null && value != DBNull.Value)
                {
                    resultValue = (T)Convert.ChangeType(value, typeof(T));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                try
                {
                    if (sqlCommand != null)
                        sqlCommand.Dispose();

                    if (connection != null && connection.State != ConnectionState.Closed)
                        connection.Close();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }

            return resultValue;
        }

    }
}
