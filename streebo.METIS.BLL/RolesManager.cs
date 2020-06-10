using streebo.core.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace streebo.METIS.BLL
{
    public sealed class RolesManager
    {
        private DatabaseConnections conn = DatabaseConnections.Instance;

        #region RolesManager Singleton

        private static readonly object padlock = new object();
        private static RolesManager instance = null;
        private RolesManager()
        {

        }

        public static RolesManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new RolesManager();
                        }
                    }
                }
                return instance;
            } 
        }
        #endregion

        #region Methods

        public DataTable GetAllRoles()
        {
            string query = string.Format("getAllRoles");
            return conn.executeSelectStoredProcedure(query);
        }

        public DataTable GetRolesByDepartmentId(string Dept_id)
        {
            string query = string.Format("getRoles");
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@Dept_id", SqlDbType.VarChar, 10);
            sqlParameters[0].Value = Convert.ToString(Dept_id);
            return conn.executeSelectStoredProcedure(query, sqlParameters);
        }

        public DataTable GetRolesfrmResID(string Res_id)
        {
            string query = string.Format("getRolesfromResID");
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@resId", SqlDbType.Int);
            sqlParameters[0].Value = Convert.ToInt16(Res_id);
            return conn.executeSelectStoredProcedure(query, sqlParameters);
        }

        public Boolean InsertRole(string p_RoleName, string p_ReportsTo, string p_DepartmentId, string p_Active, out string p_message)
        {
            string RoleId = GetNextRoleID();

            string sp_return_message = "";
            string query = string.Format("USP_ADD_NEW_ROLE");
            SqlParameter[] sqlParameters = new SqlParameter[6];

            sqlParameters[0] = new SqlParameter("@RoleId", SqlDbType.VarChar);
            sqlParameters[0].Value = Convert.ToString(RoleId);
            sqlParameters[1] = new SqlParameter("@RoleTitle", SqlDbType.VarChar, 255);
            sqlParameters[1].Value = Convert.ToString(p_RoleName);
            sqlParameters[2] = new SqlParameter("@ReportsTo", SqlDbType.VarChar, 255);
            sqlParameters[2].Value = Convert.ToString(p_ReportsTo);
            sqlParameters[3] = new SqlParameter("@DepartmentId", SqlDbType.VarChar, 255);
            sqlParameters[3].Value = Convert.ToString(p_DepartmentId);
            sqlParameters[4] = new SqlParameter("@Active", SqlDbType.Bit);
            sqlParameters[4].Value = Convert.ToUInt16(p_Active.ToLower() == "false" ? 0 : 1);
            sqlParameters[5] = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 255);
            sqlParameters[5].Direction = ParameterDirection.Output;
            sqlParameters[5].Value = Convert.ToString(sp_return_message);

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

        public Boolean UpdateRole(string p_RoleID, string p_RoleName, string p_ReportsTo, string p_DepartmentId, string p_Active, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("USP_UPDATE_ROLE");
            SqlParameter[] sqlParameters = new SqlParameter[6];

            sqlParameters[0] = new SqlParameter("@RoleId", SqlDbType.VarChar, 10);
            sqlParameters[0].Value = Convert.ToString(p_RoleID);
            sqlParameters[1] = new SqlParameter("@RoleTitle", SqlDbType.VarChar, 255);
            sqlParameters[1].Value = Convert.ToString(p_RoleName);
            sqlParameters[2] = new SqlParameter("@ReportsTo", SqlDbType.VarChar, 10);
            sqlParameters[2].Value = Convert.ToString(p_ReportsTo);
            sqlParameters[3] = new SqlParameter("@DepartmentId", SqlDbType.VarChar, 10);
            sqlParameters[3].Value = Convert.ToString(p_DepartmentId);
            sqlParameters[4] = new SqlParameter("@Active", SqlDbType.Bit);
            sqlParameters[4].Value = Convert.ToUInt16(p_Active.ToLower() == "false" ? 0 : 1);
            sqlParameters[5] = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 255);
            sqlParameters[5].Direction = ParameterDirection.Output;
            sqlParameters[5].Value = Convert.ToString(sp_return_message);

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

        public Boolean DeleteRoleById(string p_RoleID, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("USP_DELETE_ROLE_BY_ID");
            SqlParameter[] sqlParameters = new SqlParameter[2];

            sqlParameters[0] = new SqlParameter("@RoleId", SqlDbType.VarChar, 10);
            sqlParameters[0].Value = Convert.ToString(p_RoleID);
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

        public string GetNextRoleID()
        {
            string DepartmentID = "";

            string query = string.Format("USP_GET_NEXT_ROLE_ID");
            SqlParameter[] sqlParameters = new SqlParameter[1];

            sqlParameters[0] = new SqlParameter("@RoleID", SqlDbType.NVarChar, 255);
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

        #endregion
    }
}
