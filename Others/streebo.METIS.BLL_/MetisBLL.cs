using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using streebo.core.DAL;

namespace streebo.METIS.BLL
{
    public class MetisBLL
    {



        private dbConnection conn;

        public MetisBLL()
        {
            conn = new dbConnection();
        }
        
        public DataTable getRightNow()
        {
            string query = string.Format("select GETDATE() as RightNow;");
            return conn.executeSelectQuery(query);
        }

        
        #region "Department Related Methods"
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

        public Boolean insertDepartment(string p_DepartmentName, string p_ReportsTo, out string p_message)
        {
            string departmentID = getNextDeparmentID();

            string sp_return_message = "";
            string query = string.Format("insertDepartment");
            SqlParameter[] sqlParameters = new SqlParameter[4];

            sqlParameters[0] = new SqlParameter("@DepartmentID", SqlDbType.VarChar);
            sqlParameters[0].Value = Convert.ToString(departmentID);
            sqlParameters[1] = new SqlParameter("@DepartmentName", SqlDbType.VarChar, 255);
            sqlParameters[1].Value = Convert.ToString(p_DepartmentName);
            sqlParameters[2] = new SqlParameter("@ReportsTo", SqlDbType.VarChar, 255);
            sqlParameters[2].Value = Convert.ToString(p_ReportsTo);
            sqlParameters[3] = new SqlParameter("@ReturnMessage", SqlDbType.VarChar,255);
            sqlParameters[3].Direction = ParameterDirection.Output;
            sqlParameters[3].Value = Convert.ToString(sp_return_message);
            
            try
            {
                conn.executeInsertStoredProcedure(query, sqlParameters, out p_message);
                return true;
            }
            catch (Exception e)
            {
                if (sp_return_message == "")
                {p_message = e.ToString();}
                else
                {p_message = sp_return_message;}

                return false;
            }
        }

        public Boolean updateDepartment(string p_DepartmentID, string p_DepartmentName, string p_ReportsTo, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("updateDepartment");
            SqlParameter[] sqlParameters = new SqlParameter[4];

            sqlParameters[0] = new SqlParameter("@DepartmentID", SqlDbType.VarChar,10);
            sqlParameters[0].Value = Convert.ToString(p_DepartmentID);
            sqlParameters[1] = new SqlParameter("@DepartmentName", SqlDbType.VarChar,255);
            sqlParameters[1].Value = Convert.ToString(p_DepartmentName);
            sqlParameters[2] = new SqlParameter("@ReportsTo", SqlDbType.VarChar,10);
            sqlParameters[2].Value = Convert.ToString(p_ReportsTo);
            sqlParameters[3] = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 255);
            sqlParameters[3].Direction = ParameterDirection.Output;
            sqlParameters[3].Value = Convert.ToString(sp_return_message);

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
        
        public DataTable getRoles()
        {
            string query = string.Format("getAllRoles");
            return conn.executeSelectStoredProcedure(query);
        }
  
        #endregion
        
        #region "Resource Association Methods"

        public DataTable getAllResourceAssociations()
        {
            string query = string.Format("getAllResourceAssociations");
            return conn.executeSelectStoredProcedure(query);
        }

        public DataTable getAllResources()
        {
            string query = string.Format("getAllResources");
            return conn.executeSelectStoredProcedure(query);
        }

        public string getNextResourceAssociationID()
        {
            string ResourceAssociationID = "";

            string query = string.Format("getNextResourceAssociationID");
            SqlParameter[] sqlParameters = new SqlParameter[1];

            sqlParameters[0] = new SqlParameter("@ResourceAssociationID", SqlDbType.NVarChar, 255);
            sqlParameters[0].Direction = ParameterDirection.Output;
            sqlParameters[0].Value = Convert.ToString(ResourceAssociationID);

            try
            {
                ResourceAssociationID = conn.executeGetStringStoredProcedure(query, sqlParameters);
                return ResourceAssociationID;
            }
            catch (System.FormatException)
            {
                return "";
            }
        }

        public Boolean insertResourceAssociation(string p_ResourceID, string p_DepartmentID, out string p_message)
        {
            string ResourceAssociationID = getNextResourceAssociationID();

            string sp_return_message = "";
            string query = string.Format("insertResourceAssociation");
            SqlParameter[] sqlParameters = new SqlParameter[4];

            sqlParameters[0] = new SqlParameter("@ResourceAssociationID", SqlDbType.VarChar);
            sqlParameters[0].Value = Convert.ToString(ResourceAssociationID);
            sqlParameters[1] = new SqlParameter("@ResourceID", SqlDbType.VarChar, 255);
            sqlParameters[1].Value = Convert.ToString(p_ResourceID);
            sqlParameters[2] = new SqlParameter("@DepartmentID", SqlDbType.VarChar, 255);
            sqlParameters[2].Value = Convert.ToString(p_DepartmentID);
            sqlParameters[3] = new SqlParameter("@ReturnMessage", SqlDbType.VarChar,255);
            sqlParameters[3].Direction = ParameterDirection.Output;
            sqlParameters[3].Value = Convert.ToString(sp_return_message);
            
            try
            {
                conn.executeInsertStoredProcedure(query, sqlParameters, out p_message);
                return true;
            }
            catch (Exception e)
            {
                if (sp_return_message == "")
                {p_message = e.ToString();}
                else
                {p_message = sp_return_message;}

                return false;
            }
        }

        public Boolean updateResourceAssociation(string p_ResourceAssociationID, string p_ResourceName, string p_DepartmentName, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("updateResourceAssociation");
            SqlParameter[] sqlParameters = new SqlParameter[4];

            sqlParameters[0] = new SqlParameter("@ResourceAssociationID", SqlDbType.VarChar, 100);
            sqlParameters[0].Value = Convert.ToString(p_ResourceAssociationID);
            sqlParameters[1] = new SqlParameter("@ResourceName", SqlDbType.VarChar, 100);
            sqlParameters[1].Value = Convert.ToString(p_ResourceName);
            sqlParameters[2] = new SqlParameter("@DepartmentName", SqlDbType.VarChar, 100);
            sqlParameters[2].Value = Convert.ToString(p_DepartmentName);
            sqlParameters[3] = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 255);
            sqlParameters[3].Direction = ParameterDirection.Output;
            sqlParameters[3].Value = Convert.ToString(sp_return_message);

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

        public Boolean deleteResourceAssociation(string p_ResourceAssociationID, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("deleteResourceAssociation");
            SqlParameter[] sqlParameters = new SqlParameter[2];


            sqlParameters[0] = new SqlParameter("@ResourceAssociationID", SqlDbType.VarChar, 10);
            sqlParameters[0].Value = Convert.ToString(p_ResourceAssociationID);
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

        #region Weekly Grid Methods
        public DataTable getResourceSummary(DateTime WeekStarting,DateTime WeekEnding)
        {
            string query = string.Format("getResourceSummary");
            SqlParameter[] sqlParameters = new SqlParameter[2];

            sqlParameters[0] = new SqlParameter("@WeekStarting", SqlDbType.Date);
            sqlParameters[0].Value = WeekStarting;
            sqlParameters[1] = new SqlParameter("@WeekEnding", SqlDbType.Date);
            sqlParameters[1].Value = WeekEnding;
            return conn.executeSelectStoredProcedure(query,sqlParameters);
        }

        public DataTable getResourceDetailById(string id)
        {
            string query = string.Format("getEmployeeInfo");
            SqlParameter[] sqlParameters = new SqlParameter[1];

            sqlParameters[0] = new SqlParameter("@id", SqlDbType.NVarChar,10);
            sqlParameters[0].Value = id;
            return conn.executeSelectStoredProcedure(query, sqlParameters);
        }

        public DataTable getResourceAvailSummary(DateTime WeekStarting, DateTime WeekEnding)
        {
            string query = string.Format("getResourceAvailSummary");
            SqlParameter[] sqlParameters = new SqlParameter[2];

            sqlParameters[0] = new SqlParameter("@WeekStarting", SqlDbType.Date);
            sqlParameters[0].Value = WeekStarting;
            sqlParameters[1] = new SqlParameter("@WeekEnding", SqlDbType.Date);
            sqlParameters[1].Value = WeekEnding;
            return conn.executeSelectStoredProcedure(query, sqlParameters);
        }

        public DataTable getProjectDetail(string pid,DateTime WeekStarting, DateTime WeekEnding)
        {
            string query = string.Format("getProjectDetail");
            SqlParameter[] sqlParameters = new SqlParameter[3];

            sqlParameters[0] = new SqlParameter("@WeekStarting", SqlDbType.Date);
            sqlParameters[0].Value = WeekStarting;
            sqlParameters[1] = new SqlParameter("@WeekEnding", SqlDbType.Date);
            sqlParameters[1].Value = WeekEnding;
            sqlParameters[2] = new SqlParameter("@pid", SqlDbType.NVarChar,10);
            sqlParameters[2].Value = pid;
            return conn.executeSelectStoredProcedure(query, sqlParameters);
        }

        public DataTable getResourceDetail(int rid, DateTime WeekStarting, DateTime WeekEnding)
        {
            string query = string.Format("getResourceDetail");
            SqlParameter[] sqlParameters = new SqlParameter[3];

            sqlParameters[0] = new SqlParameter("@rid", SqlDbType.Int);
            sqlParameters[0].Value = rid;
            sqlParameters[1] = new SqlParameter("@WeekStarting", SqlDbType.Date);
            sqlParameters[1].Value = WeekStarting;
            sqlParameters[2] = new SqlParameter("@WeekEnding", SqlDbType.Date);
            sqlParameters[2].Value = WeekEnding;
            return conn.executeSelectStoredProcedure(query,sqlParameters);
        }

        public DataTable getProjectSummary(DateTime WeekStarting, DateTime WeekEnding)
        {
            string query = string.Format("getProjectSummary");
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@WeekStarting", SqlDbType.Date);
            sqlParameters[0].Value = WeekStarting;
            sqlParameters[1] = new SqlParameter("@WeekEnding", SqlDbType.Date);
            sqlParameters[1].Value = WeekEnding;
            return conn.executeSelectStoredProcedure(query, sqlParameters);
        }

        public Boolean updateWeeklyReport(string p_ResourceID, string p_ProjectID,DateTime p_WeekEnding ,float p_HourPerDay, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("updateWeeklyReport");
            SqlParameter[] sqlParameters = new SqlParameter[5];

            sqlParameters[0] = new SqlParameter("@resourceID", SqlDbType.NVarChar,50);
            sqlParameters[0].Value = Convert.ToString(p_ResourceID);
            sqlParameters[1] = new SqlParameter("@projectID", SqlDbType.NVarChar, 50);
            sqlParameters[1].Value = Convert.ToString(p_ProjectID);
            sqlParameters[2] = new SqlParameter("@weekEnding", SqlDbType.Date);
            sqlParameters[2].Value = p_WeekEnding;
            sqlParameters[3] = new SqlParameter("@hourPerDay", SqlDbType.Float);
            sqlParameters[3].Value = p_HourPerDay;
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

        #endregion

        #region "Resource Assignment Methods"

        public DataTable getAllResourceAssignments()
        {
            string query = string.Format("getAllResourceAssignments");
            return conn.executeSelectStoredProcedure(query);
        }

        public string getNextResourceAssignmentID()
        {
            string ResourceAssignmentID = "";

            string query = string.Format("getNextResourceAssignmentID");
            SqlParameter[] sqlParameters = new SqlParameter[1];

            sqlParameters[0] = new SqlParameter("@ResourceAssignmentID", SqlDbType.NVarChar, 255);
            sqlParameters[0].Direction = ParameterDirection.Output;
            sqlParameters[0].Value = Convert.ToString(ResourceAssignmentID);

            try
            {
                ResourceAssignmentID = conn.executeGetStringStoredProcedure(query, sqlParameters);
                return ResourceAssignmentID;
            }
            catch (System.FormatException)
            {
                return "";
            }
        }

        public Boolean insertResourceAssignment(string p_ResourceID, string p_ProjectID, string p_RoleID, out string p_message)
        {
            //string ResourceAssignmentID = getNextResourceAssignmentID();

            string sp_return_message = "";
            string query = string.Format("insertResourceAssignment");
            SqlParameter[] sqlParameters = new SqlParameter[4];

            //sqlParameters[0] = new SqlParameter("@ResourceAssignmentID", SqlDbType.VarChar);
            //sqlParameters[0].Value = Convert.ToString(ResourceAssignmentID);
            sqlParameters[0] = new SqlParameter("@ResourceID", SqlDbType.VarChar, 255);
            sqlParameters[0].Value = Convert.ToString(p_ResourceID);
            sqlParameters[1] = new SqlParameter("@ProjectID", SqlDbType.VarChar, 255);
            sqlParameters[1].Value = Convert.ToString(p_ProjectID);
            sqlParameters[2] = new SqlParameter("@RoleID", SqlDbType.VarChar, 255);
            sqlParameters[2].Value = Convert.ToString(p_RoleID);
            sqlParameters[3] = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 255);
            sqlParameters[3].Direction = ParameterDirection.Output;
            sqlParameters[3].Value = Convert.ToString(sp_return_message);

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

        public Boolean updateResourceAssignment(string p_ResourceAssignmentID, string p_ResourceName, string p_ProjectName, string p_RoleName, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("updateResourceAssignment");
            SqlParameter[] sqlParameters = new SqlParameter[5];

            sqlParameters[0] = new SqlParameter("@ResourceAssignmentID", SqlDbType.VarChar, 100);
            sqlParameters[0].Value = Convert.ToString(p_ResourceAssignmentID);
            sqlParameters[1] = new SqlParameter("@ResourceName", SqlDbType.VarChar, 100);
            sqlParameters[1].Value = Convert.ToString(p_ResourceName);
            sqlParameters[2] = new SqlParameter("@ProjectName", SqlDbType.VarChar, 100);
            sqlParameters[2].Value = Convert.ToString(p_ProjectName);
            sqlParameters[3] = new SqlParameter("@RoleName", SqlDbType.VarChar, 100);
            sqlParameters[3].Value = Convert.ToString(p_RoleName);
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

        public Boolean deleteResourceAssignment(string p_ResourceAssignmentID, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("deleteResourceAssignment");
            SqlParameter[] sqlParameters = new SqlParameter[2];


            sqlParameters[0] = new SqlParameter("@ResourceAssignmentID", SqlDbType.VarChar, 10);
            sqlParameters[0].Value = Convert.ToString(p_ResourceAssignmentID);
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

        #region "Bulk Assignment Methods"

        public DataTable getAllBulkAssignments()
        {
            string query = string.Format("getAllBulkAssignments");
            return conn.executeSelectStoredProcedure(query);
        }

        public string getNextBulkAssignmentID()
        {
            string ResourceAssignmentID = "";

            string query = string.Format("getNextBulkAssignmentID");
            SqlParameter[] sqlParameters = new SqlParameter[1];

            sqlParameters[0] = new SqlParameter("@BulkAssignmentID", SqlDbType.NVarChar, 255);
            sqlParameters[0].Direction = ParameterDirection.Output;
            sqlParameters[0].Value = Convert.ToString(ResourceAssignmentID);

            try
            {
                ResourceAssignmentID = conn.executeGetStringStoredProcedure(query, sqlParameters);
                return ResourceAssignmentID;
            }
            catch (System.FormatException)
            {
                return "";
            }
        }

        public Boolean insertBulkAssignment(string p_ResourceID, string p_ProjectID, string p_WorkDays,
            string p_AvailableDays, DateTime p_WeekEnding, float p_Sunday, float p_Monday, float p_Tuesday,
            float p_Wednesday, float p_Thursday, float p_Friday, float p_Saturday, string p_BulkAssignment,
            string p_WorkLoad, string p_StartBulk, string p_EndBulk, string p_isDeleted, out string p_message)
        {
            //string ResourceAssignmentID = getNextResourceAssignmentID();

            string sp_return_message = "";
            string query = string.Format("insertBulkAssignments");
            SqlParameter[] sqlParameters = new SqlParameter[18];

            //sqlParameters[0] = new SqlParameter("@ResourceAssignmentID", SqlDbType.VarChar);
            //sqlParameters[0].Value = Convert.ToString(ResourceAssignmentID);

            sqlParameters[0] = new SqlParameter("@ResourceID", SqlDbType.VarChar, 100);
            sqlParameters[0].Value = Convert.ToString(p_ResourceID);
            sqlParameters[1] = new SqlParameter("@ProjectID", SqlDbType.VarChar, 100);
            sqlParameters[1].Value = Convert.ToString(p_ProjectID);
            sqlParameters[2] = new SqlParameter("@WorkDays", SqlDbType.VarChar, 100);
            sqlParameters[2].Value = Convert.ToString(p_WorkDays);
            sqlParameters[3] = new SqlParameter("@AvailableDays", SqlDbType.VarChar, 100);
            sqlParameters[3].Value = Convert.ToString(p_AvailableDays);
            sqlParameters[4] = new SqlParameter("@WeekEnding", SqlDbType.Date);
            sqlParameters[4].Value = Convert.ToString(p_WeekEnding);
            sqlParameters[5] = new SqlParameter("@Sunday", SqlDbType.Float);
            sqlParameters[5].Value = Convert.ToString(p_Sunday);
            sqlParameters[6] = new SqlParameter("@Monday", SqlDbType.Float);
            sqlParameters[6].Value = Convert.ToString(p_Monday);
            sqlParameters[7] = new SqlParameter("@Tuesday", SqlDbType.Float);
            sqlParameters[7].Value = Convert.ToString(p_Tuesday);
            sqlParameters[8] = new SqlParameter("@Wednesday", SqlDbType.Float);
            sqlParameters[8].Value = Convert.ToString(p_Wednesday);
            sqlParameters[9] = new SqlParameter("@Thursday", SqlDbType.Float);
            sqlParameters[9].Value = Convert.ToString(p_Thursday);
            sqlParameters[10] = new SqlParameter("@Friday", SqlDbType.Float);
            sqlParameters[10].Value = Convert.ToString(p_Friday);
            sqlParameters[11] = new SqlParameter("@Saturday", SqlDbType.Float);
            sqlParameters[11].Value = Convert.ToString(p_Saturday);
            sqlParameters[12] = new SqlParameter("@BulkAssignment", SqlDbType.VarChar, 255);
            sqlParameters[12].Value = Convert.ToString(p_BulkAssignment);
            sqlParameters[13] = new SqlParameter("@WorkLoad", SqlDbType.VarChar, 255);
            sqlParameters[13].Value = Convert.ToString(p_WorkLoad);
            sqlParameters[14] = new SqlParameter("@StartBulk", SqlDbType.VarChar, 100);
            sqlParameters[14].Value = Convert.ToString(p_StartBulk);
            sqlParameters[15] = new SqlParameter("@EndBulk", SqlDbType.VarChar, 100);
            sqlParameters[15].Value = Convert.ToString(p_EndBulk);
            sqlParameters[16] = new SqlParameter("@isDeleted", SqlDbType.VarChar, 100);
            sqlParameters[16].Value = Convert.ToString(p_isDeleted);

            sqlParameters[17] = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 255);
            sqlParameters[17].Direction = ParameterDirection.Output;
            sqlParameters[17].Value = Convert.ToString(sp_return_message);

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

        public Boolean updateBulkAssignment(string p_BulkAssignmentID, string p_ResourceName, string p_ProjectName, string p_RoleName, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("updateBulkAssignment");
            SqlParameter[] sqlParameters = new SqlParameter[5];

            sqlParameters[0] = new SqlParameter("@BulkAssignmentID", SqlDbType.VarChar, 100);
            sqlParameters[0].Value = Convert.ToString(p_BulkAssignmentID);
            sqlParameters[1] = new SqlParameter("@ResourceName", SqlDbType.VarChar, 100);
            sqlParameters[1].Value = Convert.ToString(p_ResourceName);
            sqlParameters[2] = new SqlParameter("@ProjectName", SqlDbType.VarChar, 100);
            sqlParameters[2].Value = Convert.ToString(p_ProjectName);
            sqlParameters[3] = new SqlParameter("@RoleName", SqlDbType.VarChar, 100);
            sqlParameters[3].Value = Convert.ToString(p_RoleName);
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

        public Boolean deleteBulkAssignment(string p_BulkAssignmentID, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("deleteBulkAssignmentByID");
            SqlParameter[] sqlParameters = new SqlParameter[2];


            sqlParameters[0] = new SqlParameter("@BulkAssignmentID", SqlDbType.VarChar, 10);
            sqlParameters[0].Value = Convert.ToString(p_BulkAssignmentID);
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

        public Boolean deleteBulkAssignment(string p_ResourceName, string p_ProjectName, DateTime p_WeekEnding, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("deleteBulkAssignment");
            SqlParameter[] sqlParameters = new SqlParameter[4];

            sqlParameters[0] = new SqlParameter("@ResourceName", SqlDbType.VarChar, 100);
            sqlParameters[0].Value = Convert.ToString(p_ResourceName);
            sqlParameters[1] = new SqlParameter("@ProjectName", SqlDbType.VarChar, 100);
            sqlParameters[1].Value = Convert.ToString(p_ProjectName);
            sqlParameters[2] = new SqlParameter("@WeekEnding", SqlDbType.VarChar, 100);
            sqlParameters[2].Value = Convert.ToString(p_WeekEnding.ToShortDateString());
            sqlParameters[3] = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 255);
            sqlParameters[3].Direction = ParameterDirection.Output;
            sqlParameters[3].Value = Convert.ToString(sp_return_message);

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

        public Boolean deleteBulkAssignment(string p_ResourceName, string p_ProjectName, DateTime p_BulkStartDate, DateTime p_BulkEndDate, DateTime p_BulkAssign, DateTime p_BulkStartActual, DateTime p_BulkEndActual, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("deleteBulkAssignmentRange");
            SqlParameter[] sqlParameters = new SqlParameter[8];

            sqlParameters[0] = new SqlParameter("@ResourceName", SqlDbType.VarChar, 100);
            sqlParameters[0].Value = Convert.ToString(p_ResourceName);
            sqlParameters[1] = new SqlParameter("@ProjectName", SqlDbType.VarChar, 100);
            sqlParameters[1].Value = Convert.ToString(p_ProjectName);
            sqlParameters[2] = new SqlParameter("@BulkStartDate", SqlDbType.Date);
            sqlParameters[2].Value = p_BulkStartDate;
            sqlParameters[3] = new SqlParameter("@BulkEndDate", SqlDbType.Date);
            sqlParameters[3].Value = p_BulkEndDate;
            sqlParameters[4] = new SqlParameter("@BulkAssign", SqlDbType.Date);
            sqlParameters[4].Value = p_BulkAssign;
            sqlParameters[5] = new SqlParameter("@BulkStartActual", SqlDbType.Date);
            sqlParameters[5].Value = p_BulkStartActual;
            sqlParameters[6] = new SqlParameter("@BulkEndActual", SqlDbType.Date);
            sqlParameters[6].Value = p_BulkEndActual;
            sqlParameters[7] = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 255);
            sqlParameters[7].Direction = ParameterDirection.Output;
            sqlParameters[7].Value = Convert.ToString(sp_return_message);

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

        #region "Project Related Methods"

        public DataTable getProjects()
        {
            string query = string.Format("getAllProjects");
            return conn.executeSelectStoredProcedure(query);
        }

        public DataTable getProject(string p_rid)
        {
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@rid", SqlDbType.NVarChar, 10);
            sqlParameters[0].Value = Convert.ToString(p_rid);
            string query = string.Format("getProject");
            return conn.executeSelectStoredProcedure(query, sqlParameters);
        }

        public string CheckProjectAllocation(string p_ResourceID, DateTime p_BulkStartDate, DateTime p_BulkEndDate, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("CheckProjectAllocation");
            SqlParameter[] sqlParameters = new SqlParameter[4];

            sqlParameters[0] = new SqlParameter("@ResourceID", SqlDbType.NVarChar, 10);
            sqlParameters[0].Value = Convert.ToString(p_ResourceID);
            sqlParameters[1] = new SqlParameter("@StartDate", SqlDbType.Date);
            sqlParameters[1].Value = p_BulkStartDate;
            sqlParameters[2] = new SqlParameter("@EndDate", SqlDbType.Date);
            sqlParameters[2].Value = p_BulkEndDate;
            sqlParameters[3] = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 255);
            sqlParameters[3].Direction = ParameterDirection.Output;
            sqlParameters[3].Value = Convert.ToString(sp_return_message);

            conn.executeStoredProcedure(query, sqlParameters, out p_message);
            return p_message;

        }

        #endregion

        #region ResourceLeaves

        public DataTable getAllResourceLeaves()
        {
            string query = string.Format("getAllResourceLeaves");
            return conn.executeSelectStoredProcedure(query);
        }

        //@ResourceID nvarchar(10),
        //@LeaveDate date,
        //@ReturnMessage varchar(255) out

        public Boolean insertResourceLeave(string p_ResourceID, DateTime p_LeaveStart, DateTime p_LeaveEnd, out string p_message)
        {


            string sp_return_message = "";
            string query = string.Format("insertResourceLeave");
            SqlParameter[] sqlParameters = new SqlParameter[4];

            sqlParameters[0] = new SqlParameter("@ResourceID", SqlDbType.NVarChar, 10);
            sqlParameters[0].Value = Convert.ToString(p_ResourceID);
            sqlParameters[1] = new SqlParameter("@LeaveStart", SqlDbType.Date);
            sqlParameters[1].Value = p_LeaveStart;
            sqlParameters[2] = new SqlParameter("@LeaveEnd", SqlDbType.Date);
            sqlParameters[2].Value = p_LeaveEnd;
            sqlParameters[3] = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 255);
            sqlParameters[3].Direction = ParameterDirection.Output;
            sqlParameters[3].Value = Convert.ToString(sp_return_message);

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

        /*@ResourceName nvarchar(10),
          @LeaveDate date,*/

        public Boolean deleteResourceLeave(int p_HeaderID, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("deleteResourceLeave");
            SqlParameter[] sqlParameters = new SqlParameter[2];

            sqlParameters[0] = new SqlParameter("@HeaderID", SqlDbType.Int);
            sqlParameters[0].Value = p_HeaderID;

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

        #region UpComingProjects

        public DataTable getAllUpComingProject()
        {
            string query = string.Format("getAllUpComingProject");
            return conn.executeSelectStoredProcedure(query);
        }

        public Boolean insertUpComingProject(string p_Project, DateTime p_DesiredStart, DateTime p_PlannedStart, string p_Resource, string p_Comment,string p_DepartmentID, out string p_message)
        {


            /*
             
            @Project nvarchar(10),
            @DesiredStart date,
            @PlannedStart date,
            @Resources nvarchar(max),
            @Comments nvarchar(max),
            @ReturnMessage varchar(255) out 
             
            */

            string sp_return_message = "";
            string query = string.Format("insertUpComingProject");
            SqlParameter[] sqlParameters = new SqlParameter[7];

            sqlParameters[0] = new SqlParameter("@Project", SqlDbType.NVarChar, 10);
            sqlParameters[0].Value = Convert.ToString(p_Project);
            sqlParameters[1] = new SqlParameter("@DesiredStart", SqlDbType.Date);
            sqlParameters[1].Value = p_DesiredStart;
            sqlParameters[2] = new SqlParameter("@PlannedStart", SqlDbType.Date);
            sqlParameters[2].Value = p_PlannedStart;
            sqlParameters[3] = new SqlParameter("@Resources", SqlDbType.VarChar);
            sqlParameters[3].Value = p_Resource;
            sqlParameters[4] = new SqlParameter("@Comments", SqlDbType.VarChar);
            sqlParameters[4].Value = p_Comment;
            sqlParameters[5] = new SqlParameter("@DepartmentID", SqlDbType.VarChar);
            sqlParameters[5].Value = p_DepartmentID;
            sqlParameters[6] = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 255);
            sqlParameters[6].Direction = ParameterDirection.Output;
            sqlParameters[6].Value = Convert.ToString(sp_return_message);

            try
            {
                conn.executeInsertStoredProcedure(query, sqlParameters, out p_message);
                return true;
            }
            catch (Exception e)
            {
                if (sp_return_message == "")
                {
                    p_message = e.ToString();
                }
                else
                {
                    p_message = sp_return_message;
                }
                return false;
            }
        }

        public Boolean updateUpComingProject(int p_id, string p_Project, DateTime p_DesiredStart, DateTime p_PlannedStart, string p_Resource, string p_Comment,string p_DepartmentID, out string p_message)
        {


            /*
            @id
            @Project nvarchar(10),
            @DesiredStart date,
            @PlannedStart date,
            @Resources nvarchar(max),
            @Comments nvarchar(max),
            @ReturnMessage varchar(255) out 
             
            */

            string sp_return_message = "";
            string query = string.Format("updateUpComingProject");
            SqlParameter[] sqlParameters = new SqlParameter[8];


            sqlParameters[0] = new SqlParameter("@id", SqlDbType.NVarChar, 10);
            sqlParameters[0].Value = Convert.ToString(p_id);
            sqlParameters[1] = new SqlParameter("@Project", SqlDbType.NVarChar, 10);
            sqlParameters[1].Value = Convert.ToString(p_Project);
            sqlParameters[2] = new SqlParameter("@DesiredStart", SqlDbType.Date);
            sqlParameters[2].Value = p_DesiredStart;
            sqlParameters[3] = new SqlParameter("@PlannedStart", SqlDbType.Date);
            sqlParameters[3].Value = p_PlannedStart;
            sqlParameters[4] = new SqlParameter("@Resources", SqlDbType.VarChar);
            sqlParameters[4].Value = p_Resource;
            sqlParameters[5] = new SqlParameter("@Comments", SqlDbType.VarChar);
            sqlParameters[5].Value = p_Comment;
            sqlParameters[6] = new SqlParameter("@DepartmentID", SqlDbType.NVarChar,50);
            sqlParameters[6].Value = p_DepartmentID;
            sqlParameters[7] = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 255);
            sqlParameters[7].Direction = ParameterDirection.Output;
            sqlParameters[7].Value = Convert.ToString(sp_return_message);

            try
            {
                conn.executeInsertStoredProcedure(query, sqlParameters, out p_message);
                return true;
            }
            catch (Exception e)
            {
                if (sp_return_message == "")
                {
                    p_message = e.ToString();
                }
                else
                {
                    p_message = sp_return_message;
                }
                return false;
            }
        }

        public Boolean deleteUpComingProject(int p_ID, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("deleteUpComingProject");
            SqlParameter[] sqlParameters = new SqlParameter[2];

            sqlParameters[0] = new SqlParameter("@id", SqlDbType.Int);
            sqlParameters[0].Value = p_ID;

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

        #region ActionItems

        public DataTable getAllActionItem()
        {
            string query = string.Format("getAllActionItem");
            return conn.executeSelectStoredProcedure(query);
        }

        public DataTable getAllArchiveActionItem()
        {
            string query = string.Format("getAllArchiveActionItem");
            return conn.executeSelectStoredProcedure(query);
        }

        public Boolean insertActionItem(string p_ActionItem, string p_Resource, DateTime p_Target, string p_Status,string p_DepartmentID, out string p_message)
        {


            /*
             
           @ActionItem varchar(max),
           @Resource varchar(50),
           @Target date,
           @Status varchar(5),
           @ReturnMessage varchar(255) out
             
            */

            string sp_return_message = "";
            string query = string.Format("insertActionItem");
            SqlParameter[] sqlParameters = new SqlParameter[6];

            sqlParameters[0] = new SqlParameter("@ActionItem", SqlDbType.VarChar);
            sqlParameters[0].Value = Convert.ToString(p_ActionItem);
            sqlParameters[1] = new SqlParameter("@Resource", SqlDbType.VarChar, 50);
            sqlParameters[1].Value = p_Resource;
            sqlParameters[2] = new SqlParameter("@Target", SqlDbType.Date);
            sqlParameters[2].Value = p_Target;
            sqlParameters[3] = new SqlParameter("@Status", SqlDbType.VarChar, 5);
            sqlParameters[3].Value = p_Status;
            sqlParameters[4] = new SqlParameter("@DepartmentID", SqlDbType.NVarChar, 50);
            sqlParameters[4].Value = p_DepartmentID;
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
                {
                    p_message = e.ToString();
                }
                else
                {
                    p_message = sp_return_message;
                }
                return false;
            }
        }

        public Boolean updateActionItem(int p_id, string p_ActionItem, string p_Resource, DateTime p_Target, string p_Status,string p_DepartmentID, out string p_message)
        {


            /*
            @id int,
            @ActionItem varchar(MAX),
            @Resource nvarchar(50),
            @Target date,
            @Status varchar(5),
            @ReturnMessage varchar(255) out
            */
            string sp_return_message = "";
            string query = string.Format("updateActionItem");
            SqlParameter[] sqlParameters = new SqlParameter[7];
            sqlParameters[0] = new SqlParameter("@id", SqlDbType.Int);
            sqlParameters[0].Value = Convert.ToString(p_id);
            sqlParameters[1] = new SqlParameter("@ActionItem", SqlDbType.VarChar);
            sqlParameters[1].Value = Convert.ToString(p_ActionItem);
            sqlParameters[2] = new SqlParameter("@Resource", SqlDbType.VarChar, 50);
            sqlParameters[2].Value = p_Resource;
            sqlParameters[3] = new SqlParameter("@Target", SqlDbType.Date);
            sqlParameters[3].Value = p_Target;
            sqlParameters[4] = new SqlParameter("@Status", SqlDbType.VarChar, 5);
            sqlParameters[4].Value = p_Status;
            sqlParameters[5] = new SqlParameter("@DepartmentID", SqlDbType.NVarChar, 50);
            sqlParameters[5].Value = p_DepartmentID;
            sqlParameters[6] = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 255);
            sqlParameters[6].Direction = ParameterDirection.Output;
            sqlParameters[6].Value = Convert.ToString(sp_return_message);
            try
            {
                conn.executeInsertStoredProcedure(query, sqlParameters, out p_message);
                return true;
            }
            catch (Exception e)
            {
                if (sp_return_message == "")
                {
                    p_message = e.ToString();
                }
                else
                {
                    p_message = sp_return_message;
                }
                return false;
            }
        }

        public Boolean deleteActionItem(int p_ID, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("deleteActionItem");
            SqlParameter[] sqlParameters = new SqlParameter[2];

            sqlParameters[0] = new SqlParameter("@id", SqlDbType.Int);
            sqlParameters[0].Value = p_ID;

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

        public DataTable getAllActionItemsResourceEmail()
        {
            string _spName = string.Format("getAllActionItemsResourceEmail");
            return conn.executeSelectStoredProcedure(_spName);
        }

        public Boolean archiveActionItem(int p_ID, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("archiveActionItem");
            SqlParameter[] sqlParameters = new SqlParameter[2];

            sqlParameters[0] = new SqlParameter("@id", SqlDbType.Int);
            sqlParameters[0].Value = p_ID;

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

        public Boolean unArchiveActionItem(int p_ID, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("unArchiveActionItem");
            SqlParameter[] sqlParameters = new SqlParameter[2];

            sqlParameters[0] = new SqlParameter("@id", SqlDbType.Int);
            sqlParameters[0].Value = p_ID;

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

        public DataTable getAllResourcesWithEmail()
        {
            string query = string.Format("getAllResourcesWithEmail");
            return conn.executeSelectStoredProcedure(query);
        }




    }
}
