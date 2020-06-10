using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace streebo.core.DAL
{
    /// <summary>
    /// This is central class, deal all kind of communication with database.
    /// </summary>
    public sealed class DatabaseConnections
    {
        private SqlDataAdapter myAdapter;
        private SqlConnection conn;

        #region DatabaseConnections-Singleton
        private static readonly object padlock = new object();
        private static DatabaseConnections instance = null;
        private DatabaseConnections()
        {
            myAdapter = new SqlDataAdapter();
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MetisConnectionString"].ConnectionString);
        }
        public static DatabaseConnections Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new DatabaseConnections();
                        }
                    }
                }
                return instance;
            }
        } 
        #endregion


        private void closeConnection()
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }

        /// <method>
        /// Open Database Connection if Closed or Broken
        /// </method>
        private SqlConnection openConnection()
        {
            if (conn.State == ConnectionState.Closed || conn.State ==
                        ConnectionState.Broken)
            {
                conn.Open();
            }
            return conn;
        }

        #region "Selects"

        /// <method>
        /// Execute Select Store Procedure without parameters
        /// </method>
        public DataTable executeSelectStoredProcedure(String _spName)
        {
            SqlCommand myCommand = new SqlCommand();
            DataTable dataTable = new DataTable();
            dataTable = null;
            DataSet ds = new DataSet();

            myCommand.Connection = openConnection();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = _spName;
            myCommand.ExecuteNonQuery();
            myAdapter.SelectCommand = myCommand;
            myAdapter.Fill(ds);
            dataTable = ds.Tables[0];
            closeConnection();
            return dataTable;
        }

        /// <method>
        /// Execute Select Store Procedure without parameters
        /// </method>
        public DataTable executeSelectStoredProcedure(String _spName, SqlParameter[] sqlParameter, out string _message)
        {
            SqlCommand myCommand = new SqlCommand();
            DataTable dataTable = new DataTable();
            dataTable = null;
            DataSet ds = new DataSet();

            myCommand.Connection = openConnection();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = _spName;
            myCommand.Parameters.AddRange(sqlParameter);

            try
            {
                myCommand.ExecuteNonQuery();
                myAdapter.SelectCommand = myCommand;
                myAdapter.Fill(ds);
                dataTable = ds.Tables[0];

                _message = sqlParameter[sqlParameter.Length - 1].Value.ToString();
            }
            catch (Exception e)
            {
                _message = e.ToString();
            }
            closeConnection();
            return dataTable;

        }
        public DataTable executeSelectStoredProcedure(String _spName, SqlParameter[] sqlParameter)
        {
            SqlCommand myCommand = new SqlCommand();
            DataTable dataTable = new DataTable();
            dataTable = null;
            DataSet ds = new DataSet();

            myCommand.Connection = openConnection();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = _spName;
            myCommand.CommandTimeout = 120;
            myCommand.Parameters.AddRange(sqlParameter);
            //string x = sqlParameter[0].Value.ToString();

            try
            {
                myCommand.ExecuteNonQuery();
                myAdapter.SelectCommand = myCommand;
                myAdapter.Fill(ds);
                dataTable = ds.Tables[0];

                // _message = sqlParameter[sqlParameter.Length - 1].Value.ToString();
            }
            catch (Exception e)
            {
                // _message = e.ToString();
            }
            closeConnection();
            return dataTable;

        }



        #endregion

        #region "Return Values"

        /// <method>
        /// Execute Select Store Procedure with parameters
        /// </method>
        public int executeGetIntStoredProcedure(String _spName, SqlParameter[] sqlParameter)
        {
            SqlCommand myCommand = new SqlCommand();
            DataTable dataTable = new DataTable();
            dataTable = null;
            DataSet ds = new DataSet();

            myCommand.Connection = openConnection();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = _spName;
            myCommand.Parameters.AddRange(sqlParameter);
            myCommand.ExecuteNonQuery();
            closeConnection();

            return (int)myCommand.Parameters[0].Value;

        }
        public string executeGetStringStoredProcedure(String _spName, SqlParameter[] sqlParameter)
        {
            SqlCommand myCommand = new SqlCommand();
            DataTable dataTable = new DataTable();
            dataTable = null;
            DataSet ds = new DataSet();

            myCommand.Connection = openConnection();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = _spName;
            myCommand.Parameters.AddRange(sqlParameter);
            myCommand.ExecuteNonQuery();
            closeConnection();

            return (string)myCommand.Parameters[0].Value;

        }

        #endregion

        #region "DMLs"

        /// <method>
        /// Execute Select Store Procedure with parameters
        /// </method>
        public Boolean executeInsertStoredProcedure(String _spName, SqlParameter[] sqlParameter, out string _message)
        {
            bool bStatus = false;
            SqlCommand myCommand = new SqlCommand();

            myCommand.Connection = openConnection();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = _spName;
            myCommand.Parameters.AddRange(sqlParameter);

            try
            {
                myCommand.ExecuteNonQuery();
                _message = sqlParameter[sqlParameter.Length - 1].Value.ToString();
                bStatus = true;
            }
            catch (Exception e)
            {
                _message = e.ToString();
            }
            closeConnection();
            return bStatus;
        }

        /// <method>
        /// Execute Update Stored Procedure with parameters
        /// </method>
        public Boolean executeStoredProcedure(String _spName, SqlParameter[] sqlParameter, out string _message)
        {
            bool bStatus = false;
            SqlCommand myCommand = new SqlCommand();

            myCommand.Connection = openConnection();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = _spName;
            myCommand.Parameters.AddRange(sqlParameter);

            try
            {
                myCommand.ExecuteNonQuery();
                _message = sqlParameter[sqlParameter.Length - 1].Value.ToString();
                bStatus = true;
            }
            catch (Exception e)
            {
                _message = e.ToString();
            }

            closeConnection();
            return bStatus;
        }

        public Boolean executeStoredProcedure(String _spName, SqlParameter[] sqlParameter)
        {
            bool bStatus = false;
            SqlCommand myCommand = new SqlCommand();

            myCommand.Connection = openConnection();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = _spName;
            myCommand.Parameters.AddRange(sqlParameter);

            try
            {
                myCommand.ExecuteNonQuery();
                // _message = sqlParameter[sqlParameter.Length - 1].Value.ToString();
                bStatus = true;
            }
            catch (Exception e)
            {
                //_message = e.ToString();
            }

            closeConnection();
            return bStatus;
        }

        #endregion

        #region "Misc"

        /// <method>
        /// Select Query With Out Parameters
        /// </method>
        public DataTable executeSelectQuery(String _query)
        {
            SqlCommand myCommand = new SqlCommand();
            DataTable dataTable = new DataTable();
            dataTable = null;
            DataSet ds = new DataSet();

            myCommand.Connection = openConnection();
            myCommand.CommandText = _query;
            myCommand.ExecuteNonQuery();
            myAdapter.SelectCommand = myCommand;
            myAdapter.Fill(ds);
            dataTable = ds.Tables[0];
            closeConnection();
            return dataTable;
        }

        /// <method>
        /// Select Query with Parameters
        /// </method>
        public DataTable executeSelectQuery(String _query, SqlParameter[] sqlParameter)
        {
            SqlCommand myCommand = new SqlCommand();
            DataTable dataTable = new DataTable();
            dataTable = null;
            DataSet ds = new DataSet();

            myCommand.Connection = openConnection();
            myCommand.CommandText = _query;
            myCommand.Parameters.AddRange(sqlParameter);
            myCommand.ExecuteNonQuery();
            myAdapter.SelectCommand = myCommand;
            myAdapter.Fill(ds);
            dataTable = ds.Tables[0];
            closeConnection();

            return dataTable;
        }


        /// <method>
        /// Insert Query with Parameters
        /// </method>
        public bool executeInsertQuery(String _query, SqlParameter[] sqlParameter)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.Connection = openConnection();
            myCommand.CommandText = _query;
            myCommand.Parameters.AddRange(sqlParameter);
            myAdapter.InsertCommand = myCommand;
            myCommand.ExecuteNonQuery();
            closeConnection();
            return true;
        }

        /// <method>
        /// Update Query with parameters
        /// </method>
        public bool executeUpdateQuery(String _query, SqlParameter[] sqlParameter)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.Connection = openConnection();
            myCommand.CommandText = _query;
            myCommand.Parameters.AddRange(sqlParameter);
            myAdapter.UpdateCommand = myCommand;
            myCommand.ExecuteNonQuery();
            closeConnection();
            return true;
        }

        #endregion

    }
}
