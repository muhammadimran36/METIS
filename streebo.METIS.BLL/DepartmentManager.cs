using streebo.core.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace streebo.METIS.BLL
{
    /// <summary>
    /// This class is used to deal with Department CRUD
    /// </summary>
    public sealed class DepartmentManager
    {
        private DatabaseConnections conn = DatabaseConnections.Instance;

        #region DepartmentManager Singleton

        private static readonly object padlock = new object();
        private static DepartmentManager instance = null;
        private DepartmentManager()
        {

        }
        
        public static DepartmentManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new DepartmentManager();
                        }
                    }
                }
                return instance;
            } 
        }
        #endregion

        #region Methods
        public DataTable getDeparments()
        {
            string query = string.Format("getAllDepartments");
            return conn.executeSelectStoredProcedure(query);
        }

        public string getNextDeparmentID()
        {
            string DepartmentID = "";

            string query = string.Format("getNextDepartmentID");
            SqlParameter[] sqlParameters = new SqlParameter[1];

            sqlParameters[0] = new SqlParameter("@departmentID", SqlDbType.NVarChar, 255);
            sqlParameters[0].Direction = ParameterDirection.Output;
            sqlParameters[0].Value = Convert.ToString(DepartmentID);

            try
            {
                DepartmentID = conn.executeGetStringStoredProcedure(query, sqlParameters);
                return DepartmentID;
            }
            catch (System.FormatException)
            {
                return "";
            }
        }

        public Boolean insertDepartment(string p_DepartmentName, string p_ReportsTo, string p_Active, out string p_message)
        {
            string departmentID = getNextDeparmentID();

            string sp_return_message = "";
            string query = string.Format("insertDepartment");
            SqlParameter[] sqlParameters = new SqlParameter[5];

            sqlParameters[0] = new SqlParameter("@DepartmentID", SqlDbType.VarChar);
            sqlParameters[0].Value = Convert.ToString(departmentID);
            sqlParameters[1] = new SqlParameter("@DepartmentName", SqlDbType.VarChar, 255);
            sqlParameters[1].Value = Convert.ToString(p_DepartmentName);
            sqlParameters[2] = new SqlParameter("@ReportsTo", SqlDbType.VarChar, 255);
            sqlParameters[2].Value = Convert.ToString(p_ReportsTo);
            sqlParameters[3] = new SqlParameter("@Active", SqlDbType.Bit);
            sqlParameters[3].Value = Convert.ToUInt16(p_Active.ToLower() == "false" ? 0 : 1);
            sqlParameters[4] = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 255);
            sqlParameters[4].Direction = ParameterDirection.Output;
            sqlParameters[4].Value = Convert.ToString(sp_return_message);

            try
            {
                conn.executeInsertStoredProcedure(query, sqlParameters, out p_message);
                return true;
            }
            catch (Exception e)
            {
                if (sp_return_message == "")
                { p_message = e.ToString(); }
                else
                { p_message = sp_return_message; }

                return false;
            }
        }

        public Boolean updateDepartment(string p_DepartmentID, string p_DepartmentName, string p_ReportsTo, string p_Active, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("updateDepartment");
            SqlParameter[] sqlParameters = new SqlParameter[5];

            sqlParameters[0] = new SqlParameter("@DepartmentID", SqlDbType.VarChar, 10);
            sqlParameters[0].Value = Convert.ToString(p_DepartmentID);
            sqlParameters[1] = new SqlParameter("@DepartmentName", SqlDbType.VarChar, 255);
            sqlParameters[1].Value = Convert.ToString(p_DepartmentName);
            sqlParameters[2] = new SqlParameter("@ReportsTo", SqlDbType.VarChar, 10);
            sqlParameters[2].Value = Convert.ToString(p_ReportsTo);
            sqlParameters[3] = new SqlParameter("@Active", SqlDbType.Bit);
            sqlParameters[3].Value = Convert.ToUInt16(p_Active.ToLower() == "false" ? 0 : 1);
            sqlParameters[4] = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 255);
            sqlParameters[4].Direction = ParameterDirection.Output;
            sqlParameters[4].Value = Convert.ToString(sp_return_message);

            try
            {
                conn.executeStoredProcedure(query, sqlParameters, out p_message);
                return true;
            }
            catch (Exception e)
            {
                if (sp_return_message == "")
                { p_message = e.ToString(); }
                else
                { p_message = sp_return_message; }

                return false;
            }
        }

        public Boolean deleteDepartment(string p_DepartmentID, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("deleteDepartment");
            SqlParameter[] sqlParameters = new SqlParameter[2];


            sqlParameters[0] = new SqlParameter("@DepartmentID", SqlDbType.VarChar, 10);
            sqlParameters[0].Value = Convert.ToString(p_DepartmentID);
            sqlParameters[1] = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 255);
            sqlParameters[1].Direction = ParameterDirection.Output;
            sqlParameters[1].Value = Convert.ToString(sp_return_message);

            try
            {
                conn.executeStoredProcedure(query, sqlParameters, out p_message);
                return true;
            }
            catch (Exception e)
            {
                if (sp_return_message == "")
                { p_message = e.ToString(); }
                else
                { p_message = sp_return_message; }

                return false;
            }
        }
        #endregion
    }
}
