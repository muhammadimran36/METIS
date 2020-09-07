using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using streebo.core.DAL;
using System.CodeDom.Compiler; //
using System.Reflection; //
using Microsoft.CSharp; //
using System.Collections;

namespace streebo.METIS.BLL
{
    public class MetisBLL
    {

        private DatabaseConnections conn;
        private readonly DepartmentManager depManager = DepartmentManager.Instance;

        public MetisBLL()
        {
            conn = DatabaseConnections.Instance;
        }
        
        public DataTable getRightNow()
        {
            string query = string.Format("select GETDATE() as RightNow;");
            return conn.executeSelectQuery(query);
        }

        #region User Access Rights

        public DataTable getAccessRights(string p_email)
        {
            int ret = 0;
            string query = string.Format("getAccessRights");
            SqlParameter[] sqlParameters = new SqlParameter[1];


            sqlParameters[0] = new SqlParameter("@email", SqlDbType.VarChar);
            sqlParameters[0].Value = p_email;

            try
            {
                return conn.executeSelectStoredProcedure(query, sqlParameters);

            }
            catch (System.FormatException)
            {
                return null;
            }
        }

        public int IsAdmin(string user)
        {
            int isAdmin = 0;

            string query = string.Format("IsAdmin");
            SqlParameter[] sqlParameters = new SqlParameter[2];

           
            
            sqlParameters[0] = new SqlParameter("@isAdmin", SqlDbType.Int);
            sqlParameters[0].Direction = ParameterDirection.Output;
            sqlParameters[0].Value = isAdmin;

            sqlParameters[1] = new SqlParameter("@email", SqlDbType.VarChar, 50);
            sqlParameters[1].Value = user;
            
            
            
            try
            {
                isAdmin = conn.executeGetIntStoredProcedure(query, sqlParameters);
                return isAdmin;
            }
            
            catch (System.FormatException)
            {
                return 0;
            }
        }

        public int IsValid(string username, string pwd)
        {
            int isValid = 0;

            string query = string.Format("getVerification");
            SqlParameter[] sqlParameters = new SqlParameter[3];



            sqlParameters[0] = new SqlParameter("@IsValid", SqlDbType.Int);
            sqlParameters[0].Direction = ParameterDirection.Output;
            sqlParameters[0].Value = isValid;

            sqlParameters[1] = new SqlParameter("@UserName", SqlDbType.VarChar, 50);
            sqlParameters[1].Value = username;

            sqlParameters[2] = new SqlParameter("@Password", SqlDbType.VarChar, 50);
            sqlParameters[2].Value = pwd;



            try
            {
                isValid = conn.executeGetIntStoredProcedure(query, sqlParameters);
                return isValid;
            }

            catch (System.FormatException)
            {
                return 0;
            }
        }

       #endregion
    
        
        //public DataTable getRoles()
        //{
        //    string query = string.Format("getAllRoles");
        // return conn.executeSelectStoredProcedure(query);
            
        //}

        //public DataTable getRoles(string Dept_id)
        //{
        //    string query = string.Format("getRoles");


        //    SqlParameter[] sqlParameters = new SqlParameter[1];

        //    sqlParameters[0] = new SqlParameter("@Dept_id", SqlDbType.VarChar, 10);
        //    sqlParameters[0].Value = Convert.ToString(Dept_id);
        //    return conn.executeSelectStoredProcedure(query, sqlParameters);

        //}

        //public DataTable getRolesfrmResID(string Res_id)
        //{
        //    string query = string.Format("getRolesfromResID");


        //    SqlParameter[] sqlParameters = new SqlParameter[1];

        //    sqlParameters[0] = new SqlParameter("@resId", SqlDbType.Int);
        //    sqlParameters[0].Value = Convert.ToInt16(Res_id);
        //    return conn.executeSelectStoredProcedure(query, sqlParameters);

        //}  
        
        #region "Resource Association Methods"

        public DataTable getAllResourceAssociations(string user)
        {
            string query = string.Format("getAllResourceAssociations");
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@user", SqlDbType.VarChar, 255);
            sqlParameters[0].Value = user;
            return conn.executeSelectStoredProcedure(query, sqlParameters);
        }

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
        public DataTable getAllCompanyProjects()
        {
            string query = string.Format("getAllProjects");
            return conn.executeSelectStoredProcedure(query);
        }

        public DataTable getAllResources(string user)
        {
            string query = string.Format("getAllResources");
            SqlParameter[] sqlParameters = new SqlParameter[1];

            sqlParameters[0] = new SqlParameter("@user", SqlDbType.VarChar,255);
            sqlParameters[0].Value = user;
            return conn.executeSelectStoredProcedure(query,sqlParameters);
        }
        public DataTable getAllCompanyResources(string user)
        {
            string query = string.Format("getAllCompanyResources");
            SqlParameter[] sqlParameters = new SqlParameter[1];

            sqlParameters[0] = new SqlParameter("@user", SqlDbType.VarChar, 255);
            sqlParameters[0].Value = user;
            return conn.executeSelectStoredProcedure(query, sqlParameters);
        }


        public DataTable getAllResourcesAgainstID(string resID)
        {
            string query = string.Format("getAllResourcesAgainstID");
            SqlParameter[] sqlParameters = new SqlParameter[1];

            sqlParameters[0] = new SqlParameter("@rid", SqlDbType.Int);
            sqlParameters[0].Value = resID;
            return conn.executeSelectStoredProcedure(query, sqlParameters);
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

        //public Boolean insertResourceAssociation(string p_ResourceID, string p_DepartmentID, out string p_message)
        //{
        //    string ResourceAssociationID = getNextResourceAssociationID();

        //    string sp_return_message = "";
        //    string query = string.Format("insertResourceAssociation");
        //    SqlParameter[] sqlParameters = new SqlParameter[4];

        //    sqlParameters[0] = new SqlParameter("@ResourceAssociationID", SqlDbType.VarChar);
        //    sqlParameters[0].Value = Convert.ToString(ResourceAssociationID);
        //    sqlParameters[1] = new SqlParameter("@ResourceID", SqlDbType.VarChar, 255);
        //    sqlParameters[1].Value = Convert.ToString(p_ResourceID);
        //    sqlParameters[2] = new SqlParameter("@DepartmentID", SqlDbType.VarChar, 255);
        //    sqlParameters[2].Value = Convert.ToString(p_DepartmentID);
        //    sqlParameters[3] = new SqlParameter("@ReturnMessage", SqlDbType.VarChar,255);
        //    sqlParameters[3].Direction = ParameterDirection.Output;
        //    sqlParameters[3].Value = Convert.ToString(sp_return_message);
            
        //    try
        //    {
        //        conn.executeInsertStoredProcedure(query, sqlParameters, out p_message);
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        if (sp_return_message == "")
        //        {p_message = e.ToString();}
        //        else
        //        {p_message = sp_return_message;}

        //        return false;
        //    }
        //}

        public Boolean insertResourceAssociation(string p_DepartmentName, string p_ResourceName, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("insertResourceOnDepartment");
            SqlParameter[] sqlParameters = new SqlParameter[3];


            sqlParameters[0] = new SqlParameter("@departmentId", SqlDbType.VarChar, 100);
            sqlParameters[0].Value = Convert.ToString(p_DepartmentName);
            sqlParameters[1] = new SqlParameter("@resourceId", SqlDbType.VarChar, 100);
            sqlParameters[1].Value = Convert.ToString(p_ResourceName); 
            sqlParameters[2] = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 255);
            sqlParameters[2].Direction = ParameterDirection.Output;
            sqlParameters[2].Value = Convert.ToString(sp_return_message);

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
        public Boolean updateCompanyProject(string projectId, string projectName, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("UpdateCompanyProject");
            SqlParameter[] sqlParameters = new SqlParameter[3];

            sqlParameters[0] = new SqlParameter("@PROJECT_ID", SqlDbType.VarChar, 8000);
            sqlParameters[0].Value = Convert.ToString(projectId);
            sqlParameters[1] = new SqlParameter("@PROJECT_NAME", SqlDbType.VarChar, 255);
            sqlParameters[1].Value = Convert.ToString(projectName);
            sqlParameters[2] = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 255);
            sqlParameters[2].Direction = ParameterDirection.Output;
            sqlParameters[2].Value = Convert.ToString(sp_return_message);

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
        public Boolean updateCompanyResource(string employeeId, string resourceName, string email, string designation, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("UpdateCompanyResource");
            SqlParameter[] sqlParameters = new SqlParameter[5];

            sqlParameters[0] = new SqlParameter("@employeeId", SqlDbType.VarChar, 255);
            sqlParameters[0].Value = Convert.ToString(employeeId);
            sqlParameters[1] = new SqlParameter("@resourceName", SqlDbType.VarChar, 255);
            sqlParameters[1].Value = Convert.ToString(resourceName);
            sqlParameters[2] = new SqlParameter("@email", SqlDbType.VarChar, 255);
            sqlParameters[2].Value = Convert.ToString(email);
            sqlParameters[3] = new SqlParameter("@designation", SqlDbType.VarChar, 255);
            sqlParameters[3].Value = Convert.ToString(designation);
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

        public DataTable getResourceSummary(DateTime WeekStarting, DateTime WeekEnding, string user)
        {
            string query = string.Format("getResourceSummary");
            SqlParameter[] sqlParameters = new SqlParameter[3];

            sqlParameters[0] = new SqlParameter("@WeekStarting", SqlDbType.Date);
            sqlParameters[0].Value = WeekStarting;
            sqlParameters[1] = new SqlParameter("@WeekEnding", SqlDbType.Date);
            sqlParameters[1].Value = WeekEnding;
            sqlParameters[2] = new SqlParameter("@user", SqlDbType.VarChar,255);
            sqlParameters[2].Value = user;
            return RemoveDuplicateRows(conn.executeSelectStoredProcedure(query, sqlParameters), "Resource_name");

        }

        public DataTable getResourceDetailById(string id)
        {
            string query = string.Format("getEmployeeInfo");
            SqlParameter[] sqlParameters = new SqlParameter[1];

            sqlParameters[0] = new SqlParameter("@id", SqlDbType.NVarChar,10);
            sqlParameters[0].Value = id;
            return conn.executeSelectStoredProcedure(query, sqlParameters);
        }

        public DataTable getResourceAvailSummary(DateTime WeekStarting, DateTime WeekEnding,string user)
        {
            string query = string.Format("getResourceAvailSummary");
            SqlParameter[] sqlParameters = new SqlParameter[3];

            sqlParameters[0] = new SqlParameter("@WeekStarting", SqlDbType.Date);
            sqlParameters[0].Value = WeekStarting;
            sqlParameters[1] = new SqlParameter("@WeekEnding", SqlDbType.Date);
            sqlParameters[1].Value = WeekEnding;
            sqlParameters[2] = new SqlParameter("@user", SqlDbType.VarChar,255);
            sqlParameters[2].Value = user;
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

        public DataTable getProjectDetail(string pid, DateTime WeekStarting, DateTime WeekEnding)
        {
            string query = string.Format("getProjectDetail");
            SqlParameter[] sqlParameters = new SqlParameter[3];

            sqlParameters[0] = new SqlParameter("@WeekStarting", SqlDbType.Date);
            sqlParameters[0].Value = WeekStarting;
            sqlParameters[1] = new SqlParameter("@WeekEnding", SqlDbType.Date);
            sqlParameters[1].Value = WeekEnding;
            sqlParameters[2] = new SqlParameter("@pid", SqlDbType.VarChar, 10);
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
            SqlParameter[] sqlParameters = new SqlParameter[3];
            sqlParameters[0] = new SqlParameter("@WeekStarting", SqlDbType.Date);
            sqlParameters[0].Value = WeekStarting;
            sqlParameters[1] = new SqlParameter("@WeekEnding", SqlDbType.Date);
            sqlParameters[1].Value = WeekEnding;
            //sqlParameters[2] = new SqlParameter("@diff", SqlDbType.VarChar, 255);
            //sqlParameters[2].Value = diff;
            return conn.executeSelectStoredProcedure(query, sqlParameters);
        }

        public DataTable getProjectSummary(DateTime WeekStarting, DateTime WeekEnding,string user)
        {
            string query = string.Format("getProjectSummary");
            SqlParameter[] sqlParameters = new SqlParameter[3];
            sqlParameters[0] = new SqlParameter("@WeekStarting", SqlDbType.Date);
            sqlParameters[0].Value = WeekStarting;
            sqlParameters[1] = new SqlParameter("@WeekEnding", SqlDbType.Date);
            sqlParameters[1].Value = WeekEnding;
            sqlParameters[2] = new SqlParameter("@user", SqlDbType.VarChar,255);
            sqlParameters[2].Value = user;


            return RemoveDuplicateRows(conn.executeSelectStoredProcedure(query, sqlParameters),"Project");
        }

        public DataTable RemoveDuplicateRows(DataTable table, string DistinctColumn)
        {
            try
            {
                ArrayList UniqueRecords = new ArrayList();
                ArrayList DuplicateRecords = new ArrayList();

                // Check if records is already added to UniqueRecords otherwise,
                // Add the records to DuplicateRecords
                foreach (DataRow dRow in table.Rows)
                {
                    if (UniqueRecords.Contains(dRow[DistinctColumn]))
                        DuplicateRecords.Add(dRow);
                    else
                        UniqueRecords.Add(dRow[DistinctColumn]);
                }

                // Remove duplicate rows from DataTable added to DuplicateRecords
                foreach (DataRow dRow in DuplicateRecords)
                {
                    table.Rows.Remove(dRow);
                }

                // Return the clean DataTable which contains unique records.
                return table;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //public DataTable getProjectSummary(DateTime WeekStarting, DateTime WeekEnding)
        //{
        //    string query = string.Format("getProjectSummaryWithDateRange");
        //    SqlParameter[] sqlParameters = new SqlParameter[3];
        //    sqlParameters[0] = new SqlParameter("@WeekStarting", SqlDbType.Date);
        //    sqlParameters[0].Value = WeekStarting;
        //    sqlParameters[1] = new SqlParameter("@WeekEnding", SqlDbType.Date);
        //    sqlParameters[1].Value = WeekEnding;
        //    sqlParameters[2] = new SqlParameter("@diff", SqlDbType.VarChar, 255);
        //    sqlParameters[2].Value = diff;

        //    return conn.executeSelectStoredProcedure(query, sqlParameters);
        //}

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

        public DataTable getAllResourceAssignments(string user)
        {
            string query = string.Format("getAllResourceAssignments");

            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@user", SqlDbType.VarChar, 255);
            sqlParameters[0].Value = user;
            return conn.executeSelectStoredProcedure(query, sqlParameters);
        }

        //[getEmailfromResID]

        public DataTable getEmailfromResID(string resid)
        {
            string query = string.Format("getEmailfromResID");

            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@resourceId", SqlDbType.VarChar, 50);
            sqlParameters[0].Value = resid;
            return conn.executeSelectStoredProcedure(query, sqlParameters);
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

        public DataTable getAllBulkAssignments(string user)
        {
            string query = string.Format("getAllBulkAssignments");
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@user", SqlDbType.VarChar, 255);
            sqlParameters[0].Value = user;
            return conn.executeSelectStoredProcedure(query,sqlParameters);
        }

        public DataTable getAllBulkAssignmentsHistory(string user)
        {
            string query = string.Format("getAllBulkAssignmentsHistory");
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@user", SqlDbType.VarChar, 255);
            sqlParameters[0].Value = user;
            return conn.executeSelectStoredProcedure(query, sqlParameters);
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
            string p_WorkLoad, string p_StartBulk, string p_EndBulk, string p_isDeleted, string p_AssignmentTypeID, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("insertBulkAssignments");
            SqlParameter[] sqlParameters = new SqlParameter[19];

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
            sqlParameters[17] = new SqlParameter("@AssignmentTypeID", SqlDbType.NVarChar, 25);
            sqlParameters[17].Value = Convert.ToString(p_AssignmentTypeID);

            sqlParameters[18] = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 255);
            sqlParameters[18].Direction = ParameterDirection.Output;
            sqlParameters[18].Value = Convert.ToString(sp_return_message);

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

        public Boolean deleteBulkAssignment(string p_ResourceName, string p_ProjectName, DateTime p_BulkStartDate, DateTime p_BulkEndDate, DateTime p_BulkAssign, DateTime p_BulkStartActual, DateTime p_BulkEndActual, String AssignmentType, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("deleteBulkAssignmentRange");
            SqlParameter[] sqlParameters = new SqlParameter[9];

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
            sqlParameters[7] = new SqlParameter("@AssignmentType", SqlDbType.VarChar, 25);
            sqlParameters[7].Value = Convert.ToString(AssignmentType);
            sqlParameters[8] = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 255);
            sqlParameters[8].Direction = ParameterDirection.Output;
            sqlParameters[8].Value = Convert.ToString(sp_return_message);

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

        public DataTable getBulkName(int id)
        {
          

            string query = string.Format("getBulkName");
            SqlParameter[] sqlParameters = new SqlParameter[1];

            sqlParameters[0] = new SqlParameter("@ResourceID", SqlDbType.Int);
            sqlParameters[0].Value = id;
            try
            {
                return conn.executeSelectStoredProcedure(query, sqlParameters);
                
            }
            catch (System.FormatException)
            {
                return conn.executeSelectStoredProcedure(query, sqlParameters);
            }
        }
       
        public DataTable getBulkResource(int id)
        {
            string query = string.Format("getBulkResource");
            SqlParameter[] sqlParameters = new SqlParameter[1];

            sqlParameters[0] = new SqlParameter("@ResourceID", SqlDbType.Int);
            sqlParameters[0].Value = id;
            try
            {
                return conn.executeSelectStoredProcedure(query, sqlParameters);

            }
            catch (System.FormatException)
            {
                return conn.executeSelectStoredProcedure(query, sqlParameters);
            } 
        }


        public DataTable getBulkProject(string id)
        {
            string query = string.Format("getBulkProject");
            SqlParameter[] sqlParameters = new SqlParameter[1];

            sqlParameters[0] = new SqlParameter("@ProjectID", SqlDbType.NVarChar);
            sqlParameters[0].Value = id;
            try
            {
                return conn.executeSelectStoredProcedure(query, sqlParameters);

            }
            catch (System.FormatException)
            {
                return conn.executeSelectStoredProcedure(query, sqlParameters);
            }
        }


        #endregion

        #region "Project Related Methods"

        public DataTable getProjects()
        {
            string query = string.Format("getAllProjects");
            return conn.executeSelectStoredProcedure(query);
        }

        public DataTable getProject()
        {
            string query = string.Format("getAllProject");
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


        public DataTable getRolesFromResourceID(int p_rid)
        {
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@rid", SqlDbType.NVarChar, 10);
            sqlParameters[0].Value = Convert.ToString(p_rid);
            string query = string.Format("getRolesfromResID");
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

            sqlParameters[0] = new SqlParameter("@Project", SqlDbType.VarChar);
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
            sqlParameters[1] = new SqlParameter("@Project", SqlDbType.VarChar);
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
        public Boolean insertCompanyResource(string resourceName, DateTime joiningDate, bool status, string employeeId,string email,string designation, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("insertCompanyResource");
            SqlParameter[] sqlParameters = new SqlParameter[7];

            
            sqlParameters[0] = new SqlParameter("@resourceName", SqlDbType.VarChar, 255);
            sqlParameters[0].Value = resourceName;
            sqlParameters[1] = new SqlParameter("@JOINING_DATE", SqlDbType.Date);
            sqlParameters[1].Value = joiningDate;
            sqlParameters[2] = new SqlParameter("@status", SqlDbType.Bit);
            sqlParameters[2].Value = status;
            sqlParameters[3] = new SqlParameter("@employeeId", SqlDbType.VarChar, 255);
            sqlParameters[3].Value = employeeId;
            sqlParameters[4] = new SqlParameter("@email", SqlDbType.VarChar, 255);
            sqlParameters[4].Value = email;
            sqlParameters[5] = new SqlParameter("@designation", SqlDbType.VarChar, 255);
            sqlParameters[5].Value = designation;
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

        public Boolean insertCompanyProject(string projectName,string projectId, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("insertCompanyProject");
            SqlParameter[] sqlParameters = new SqlParameter[3];


            sqlParameters[0] = new SqlParameter("@Project_name", SqlDbType.VarChar, 255);
            sqlParameters[0].Value = projectName;

            sqlParameters[1] = new SqlParameter("@Project_id", SqlDbType.VarChar,8000);
            sqlParameters[1].Value = projectId;
           
            sqlParameters[2] = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 255);
            sqlParameters[2].Direction = ParameterDirection.Output;
            sqlParameters[2].Value = Convert.ToString(sp_return_message);

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
        public Boolean deleteCompanyProject(string p_ID, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("deleteCompanyProject");
            SqlParameter[] sqlParameters = new SqlParameter[2];

            sqlParameters[0] = new SqlParameter("@id", SqlDbType.VarChar, 255);
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
        public Boolean deleteCompanyResource(string employeeId, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("deleteCompanyResource");
            SqlParameter[] sqlParameters = new SqlParameter[2];

            sqlParameters[0] = new SqlParameter("@id", SqlDbType.VarChar, 255);
            sqlParameters[0].Value = employeeId;

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


        #region "Targets Related Methods"
        public DataTable getAllTarget_Actual()
        {
            string query = string.Format("getAllTarget_Actual");
            return conn.executeSelectStoredProcedure(query);
        }

        //public float converting_To_Float(string data) {
        //    char[] a = data.ToCharArray();
        //    char fn = '0';
        //    string[] val = new string[2];
        //    int j = 0;
        //    for (int i = 0; i < a.Length; i++)
        //    {
        //        if (a[i] == '+') { fn = a[i]; j++; continue; }
        //        if (a[i] == '-') { fn = a[i]; j++; continue; }
        //        if (a[i] == '*' || a[i] == 'x') { fn = a[i]; j++; continue; }
        //        if (a[i] == '/') { fn = a[i]; j++; continue; }

        //        val[j] = val[j] + a[i].ToString();
        //    }


        //    float answer = float.Parse(val[0]);
        //    if (fn == '+') { answer = float.Parse(val[0]) + float.Parse(val[1]); }
        //    if (fn == '-') { answer = float.Parse(val[0]) - float.Parse(val[1]); }
        //    if (fn == '*' || answer == 'x') { answer = float.Parse(val[0]) * float.Parse(val[1]); }
        //    if (fn == '/') { answer = float.Parse(val[0]) / float.Parse(val[1]); }


        //    return answer;
        //}

        #region "Converting"
        public double converting_To_Float(string expression)
        {
            //try
            //{
            //    double result = EvaluateExpression(expression);
            //    return result;
            //}

            //catch (Exception)
            //{
            //    return 0;
            //}
            try
            {
                double result = EvaluateExpression(expression);
                return result;
            }

            catch (Exception)
            {
                return 0;
            }
        }

        public double EvaluateExpression(string expression)
        {
            string code = string.Format  // Note: Use "{{" to denote a single "{"  
            (
                "public static class Func{{ public static double func(){{ return {0};}}}}",
                expression
            );

            CompilerResults compilerResults = CompileScript(code);

            //if (compilerResults.Errors.HasErrors)
            //{
            //    throw new InvalidOperationException("Expression has a syntax error.");
            //}

            Assembly assembly = compilerResults.CompiledAssembly;
            MethodInfo method = assembly.GetType("Func").GetMethod("func");

            return (double)method.Invoke(null, null);
            //string code = string.Format  // Note: Use "{{" to denote a single "{"  
            //(
            //    "public static class Func{{ public static double func(){{ return {0};}}}}",
            //    expression
            //);

            //CompilerResults compilerResults = CompileScript(code);

            ////if (compilerResults.Errors.HasErrors)
            ////{
            ////    throw new InvalidOperationException("Expression has a syntax error.");
            ////}

            //Assembly assembly = compilerResults.CompiledAssembly;
            //MethodInfo method = assembly.GetType("Func").GetMethod("func");

            //return (int)method.Invoke(null, null);
        }

        public CompilerResults CompileScript(string source)
        {
            CompilerParameters parms = new CompilerParameters();

            parms.GenerateExecutable = false;
            parms.GenerateInMemory = true;
            parms.IncludeDebugInformation = false;

            CodeDomProvider compiler = CSharpCodeProvider.CreateProvider("CSharp");

            return compiler.CompileAssemblyFromSource(parms, source);
        }
        #endregion

        public Boolean deleteTarget_Actual(int tid, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("deleteTarget_Actual");
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@tid", SqlDbType.Int);
            sqlParameters[0].Value = Convert.ToString(tid);
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

        public Boolean insertTarget_Actual(int CustomerID, int ProjectID, int Owner, int Type, int TimeID,
        float Q1_Target, double January, double February, double March, string January_Formula, string February_Formula, string March_Formula,
        float Q2_Target, double April, double May, double June, string April_Formula, string May_Formula, string June_Formula,
        float Q3_Target, double July, double August, double September, string July_Formula, string August_Formula, string September_Formula,
        float Q4_Target, double October, double November, double December, string October_Formula, string November_Formula, string December_Formula, string Comments, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("insertTarget_Actual");
            SqlParameter[] sqlParameters = new SqlParameter[35];

            sqlParameters[0] = new SqlParameter("@CustomerID", SqlDbType.Int);
            sqlParameters[0].Value = Convert.ToString(CustomerID);

            sqlParameters[1] = new SqlParameter("@ProjectID", SqlDbType.Int);
            sqlParameters[1].Value = Convert.ToString(ProjectID);

            sqlParameters[2] = new SqlParameter("@Owner", SqlDbType.Int);
            sqlParameters[2].Value = Convert.ToString(Owner);

            sqlParameters[3] = new SqlParameter("@Type", SqlDbType.Int);
            sqlParameters[3].Value = Convert.ToString(Type);

            sqlParameters[4] = new SqlParameter("@TimeID", SqlDbType.Int);
            sqlParameters[4].Value = Convert.ToString(TimeID);

            sqlParameters[5] = new SqlParameter("@Q1_Target", SqlDbType.Float);
            sqlParameters[5].Value = Convert.ToString(Q1_Target);

            sqlParameters[6] = new SqlParameter("@January", SqlDbType.Float);
            sqlParameters[6].Value = Convert.ToString(January);

            sqlParameters[7] = new SqlParameter("@February", SqlDbType.Float);
            sqlParameters[7].Value = Convert.ToString(February);

            sqlParameters[8] = new SqlParameter("@March", SqlDbType.Float);
            sqlParameters[8].Value = Convert.ToString(March);

            sqlParameters[9] = new SqlParameter("@January_Formula", SqlDbType.NVarChar, 255);
            sqlParameters[9].Value = Convert.ToString(January_Formula);

            sqlParameters[10] = new SqlParameter("@February_Formula", SqlDbType.NVarChar, 255);
            sqlParameters[10].Value = Convert.ToString(February_Formula);

            sqlParameters[11] = new SqlParameter("@March_Formula", SqlDbType.NVarChar, 255);
            sqlParameters[11].Value = Convert.ToString(March_Formula);

            sqlParameters[12] = new SqlParameter("@Q2_Target", SqlDbType.Float);
            sqlParameters[12].Value = Convert.ToString(Q2_Target);

            sqlParameters[13] = new SqlParameter("@April", SqlDbType.Float);
            sqlParameters[13].Value = Convert.ToString(April);

            sqlParameters[14] = new SqlParameter("@May", SqlDbType.Float);
            sqlParameters[14].Value = Convert.ToString(May);

            sqlParameters[15] = new SqlParameter("@June", SqlDbType.Float);
            sqlParameters[15].Value = Convert.ToString(June);

            sqlParameters[16] = new SqlParameter("@April_Formula", SqlDbType.NVarChar, 255);
            sqlParameters[16].Value = Convert.ToString(April_Formula);

            sqlParameters[17] = new SqlParameter("@May_Formula", SqlDbType.NVarChar, 255);
            sqlParameters[17].Value = Convert.ToString(May_Formula);

            sqlParameters[18] = new SqlParameter("@June_Formula", SqlDbType.NVarChar, 255);
            sqlParameters[18].Value = Convert.ToString(June_Formula);

            sqlParameters[19] = new SqlParameter("@Q3_Target", SqlDbType.Float);
            sqlParameters[19].Value = Convert.ToString(Q3_Target);

            sqlParameters[20] = new SqlParameter("@July", SqlDbType.Float);
            sqlParameters[20].Value = Convert.ToString(July);

            sqlParameters[21] = new SqlParameter("@August", SqlDbType.Float);
            sqlParameters[21].Value = Convert.ToString(August);

            sqlParameters[22] = new SqlParameter("@September", SqlDbType.Float);
            sqlParameters[22].Value = Convert.ToString(September);

            sqlParameters[23] = new SqlParameter("@July_Formula", SqlDbType.NVarChar, 255);
            sqlParameters[23].Value = Convert.ToString(July_Formula);

            sqlParameters[24] = new SqlParameter("@August_Formula", SqlDbType.NVarChar, 255);
            sqlParameters[24].Value = Convert.ToString(August_Formula);

            sqlParameters[25] = new SqlParameter("@September_Formula", SqlDbType.NVarChar, 255);
            sqlParameters[25].Value = Convert.ToString(September_Formula);

            sqlParameters[26] = new SqlParameter("@Q4_Target", SqlDbType.Float);
            sqlParameters[26].Value = Convert.ToString(Q4_Target);

            sqlParameters[27] = new SqlParameter("@October", SqlDbType.Float);
            sqlParameters[27].Value = Convert.ToString(October);

            sqlParameters[28] = new SqlParameter("@November", SqlDbType.Float);
            sqlParameters[28].Value = Convert.ToString(November);

            sqlParameters[29] = new SqlParameter("@December", SqlDbType.Float);
            sqlParameters[29].Value = Convert.ToString(December);

            sqlParameters[30] = new SqlParameter("@October_Formula", SqlDbType.NVarChar, 255);
            sqlParameters[30].Value = Convert.ToString(October_Formula);

            sqlParameters[31] = new SqlParameter("@November_Formula", SqlDbType.NVarChar, 255);
            sqlParameters[31].Value = Convert.ToString(November_Formula);

            sqlParameters[32] = new SqlParameter("@December_Formula", SqlDbType.NVarChar, 255);
            sqlParameters[32].Value = Convert.ToString(December_Formula);


            sqlParameters[33] = new SqlParameter("@Comments", SqlDbType.VarChar);
            sqlParameters[33].Value = Comments;

            sqlParameters[34] = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 255);
            sqlParameters[34].Direction = ParameterDirection.Output;
            sqlParameters[34].Value = Convert.ToString(sp_return_message);

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

        public Boolean updateTarget_Actual(int Target_Actual_PK, int CustomerID, int ProjectID, int Owner, int Type, int TimeID,
        float Q1_Target, double January, double February, double March, string January_Formula, string February_Formula, string March_Formula,
        float Q2_Target, double April, double May, double June, string April_Formula, string May_Formula, string June_Formula,
        float Q3_Target, double July, double August, double September, string July_Formula, string August_Formula, string September_Formula,
        float Q4_Target, double October, double November, double December, string October_Formula, string November_Formula, string December_Formula,
        string Comments, out string p_message)
        {
            string sp_return_message = "";
            string query = string.Format("updateTarget_Actual");
            SqlParameter[] sqlParameters = new SqlParameter[36];

            sqlParameters[0] = new SqlParameter("@Target_Actual_PK", SqlDbType.Int);
            sqlParameters[0].Value = Convert.ToString(Target_Actual_PK);

            sqlParameters[1] = new SqlParameter("@CustomerID", SqlDbType.Int);
            sqlParameters[1].Value = Convert.ToString(CustomerID);

            sqlParameters[2] = new SqlParameter("@ProjectID", SqlDbType.Int);
            sqlParameters[2].Value = Convert.ToString(ProjectID);

            sqlParameters[3] = new SqlParameter("@Owner", SqlDbType.Int);
            sqlParameters[3].Value = Convert.ToString(Owner);

            sqlParameters[4] = new SqlParameter("@Type", SqlDbType.Int);
            sqlParameters[4].Value = Convert.ToString(Type);

            sqlParameters[5] = new SqlParameter("@TimeID", SqlDbType.Int);
            sqlParameters[5].Value = Convert.ToString(TimeID);

            sqlParameters[6] = new SqlParameter("@Q1_Target", SqlDbType.Float);
            sqlParameters[6].Value = Convert.ToString(Q1_Target);

            sqlParameters[7] = new SqlParameter("@January", SqlDbType.Float);
            sqlParameters[7].Value = Convert.ToString(January);

            sqlParameters[8] = new SqlParameter("@February", SqlDbType.Float);
            sqlParameters[8].Value = Convert.ToString(February);

            sqlParameters[9] = new SqlParameter("@March", SqlDbType.Float);
            sqlParameters[9].Value = Convert.ToString(March);

            sqlParameters[10] = new SqlParameter("@January_Formula", SqlDbType.NVarChar, 255);
            sqlParameters[10].Value = Convert.ToString(January_Formula);

            sqlParameters[11] = new SqlParameter("@February_Formula", SqlDbType.NVarChar, 255);
            sqlParameters[11].Value = Convert.ToString(February_Formula);

            sqlParameters[12] = new SqlParameter("@March_Formula", SqlDbType.NVarChar, 255);
            sqlParameters[12].Value = Convert.ToString(March_Formula);

            sqlParameters[13] = new SqlParameter("@Q2_Target", SqlDbType.Float);
            sqlParameters[13].Value = Convert.ToString(Q2_Target);

            sqlParameters[14] = new SqlParameter("@April", SqlDbType.Float);
            sqlParameters[14].Value = Convert.ToString(April);

            sqlParameters[15] = new SqlParameter("@May", SqlDbType.Float);
            sqlParameters[15].Value = Convert.ToString(May);

            sqlParameters[16] = new SqlParameter("@June", SqlDbType.Float);
            sqlParameters[16].Value = Convert.ToString(June);

            sqlParameters[17] = new SqlParameter("@April_Formula", SqlDbType.NVarChar, 255);
            sqlParameters[17].Value = Convert.ToString(April_Formula);

            sqlParameters[18] = new SqlParameter("@May_Formula", SqlDbType.NVarChar, 255);
            sqlParameters[18].Value = Convert.ToString(May_Formula);

            sqlParameters[19] = new SqlParameter("@June_Formula", SqlDbType.NVarChar, 255);
            sqlParameters[19].Value = Convert.ToString(June_Formula);

            sqlParameters[20] = new SqlParameter("@Q3_Target", SqlDbType.Float);
            sqlParameters[20].Value = Convert.ToString(Q3_Target);

            sqlParameters[21] = new SqlParameter("@July", SqlDbType.Float);
            sqlParameters[21].Value = Convert.ToString(July);

            sqlParameters[22] = new SqlParameter("@August", SqlDbType.Float);
            sqlParameters[22].Value = Convert.ToString(August);

            sqlParameters[23] = new SqlParameter("@September", SqlDbType.Float);
            sqlParameters[23].Value = Convert.ToString(September);

            sqlParameters[24] = new SqlParameter("@July_Formula", SqlDbType.NVarChar, 255);
            sqlParameters[24].Value = Convert.ToString(July_Formula);

            sqlParameters[25] = new SqlParameter("@August_Formula", SqlDbType.NVarChar, 255);
            sqlParameters[25].Value = Convert.ToString(August_Formula);

            sqlParameters[26] = new SqlParameter("@September_Formula", SqlDbType.NVarChar, 255);
            sqlParameters[26].Value = Convert.ToString(September_Formula);

            sqlParameters[27] = new SqlParameter("@Q4_Target", SqlDbType.Float);
            sqlParameters[27].Value = Convert.ToString(Q4_Target);

            sqlParameters[28] = new SqlParameter("@October", SqlDbType.Float);
            sqlParameters[28].Value = Convert.ToString(October);

            sqlParameters[29] = new SqlParameter("@November", SqlDbType.Float);
            sqlParameters[29].Value = Convert.ToString(November);

            sqlParameters[30] = new SqlParameter("@December", SqlDbType.Float);
            sqlParameters[30].Value = Convert.ToString(December);

            sqlParameters[31] = new SqlParameter("@October_Formula", SqlDbType.NVarChar, 255);
            sqlParameters[31].Value = Convert.ToString(October_Formula);

            sqlParameters[32] = new SqlParameter("@November_Formula", SqlDbType.NVarChar, 255);
            sqlParameters[32].Value = Convert.ToString(November_Formula);

            sqlParameters[33] = new SqlParameter("@December_Formula", SqlDbType.NVarChar, 255);
            sqlParameters[33].Value = Convert.ToString(December_Formula);

            sqlParameters[34] = new SqlParameter("@Comments", SqlDbType.VarChar);
            sqlParameters[34].Value = Comments;

            sqlParameters[35] = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 255);
            sqlParameters[35].Direction = ParameterDirection.Output;
            sqlParameters[35].Value = Convert.ToString(sp_return_message);

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

        public DataTable getCustomers()
        {
            string query = string.Format("getAllCustomers");
            return conn.executeSelectStoredProcedure(query);
        }
        public DataTable getAllTimes_Actual()
        {
            string query = string.Format("getAllTimes_Actual");
            return conn.executeSelectStoredProcedure(query);
        }

        public DataTable getConsultants()
        {
            string query = string.Format("getAllConsultants");
            return conn.executeSelectStoredProcedure(query);
        }

        public DataTable getServices()
        {
            string query = string.Format("getAllServices");
            return conn.executeSelectStoredProcedure(query);
        }



        #endregion


        public DataTable getAllResourcesWithEmail()
        {
            string query = string.Format("getAllResourcesWithEmail");
            return conn.executeSelectStoredProcedure(query);
        }

        public DataTable getProjectType()
        {
            string query = string.Format("getProjectType");
            return conn.executeSelectStoredProcedure(query);
        }

        public Boolean insertProject(string p_ProjectID, string p_ProjectName, string p_ProjectType, out string p_message)
        {
            string departmentID = depManager.getNextDeparmentID();

            string sp_return_message = "";
            string query = string.Format("insertProject");
            SqlParameter[] sqlParameters = new SqlParameter[4];

            sqlParameters[0] = new SqlParameter("@ProjectID", SqlDbType.VarChar);
            sqlParameters[0].Value = Convert.ToString(p_ProjectID);
            sqlParameters[1] = new SqlParameter("@ProjectName", SqlDbType.VarChar, 255);
            sqlParameters[1].Value = Convert.ToString(p_ProjectName);
            sqlParameters[2] = new SqlParameter("@ProjectType", SqlDbType.VarChar, 255);
            sqlParameters[2].Value = Convert.ToString(p_ProjectType);
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

        public DataTable getNextProjectID()
        {
            string query = string.Format("getNextProjectID");
            return conn.executeSelectStoredProcedure(query);
        }

        public DataTable getTimes()
        {
            string query = string.Format("getAllTimes");
            return conn.executeSelectStoredProcedure(query);
        }



        public DataTable getAssignmentType()
        {
            string query = string.Format("getAssignmentType");
            //SqlParameter[] sqlParameters = new SqlParameter[1];
            //sqlParameters[0] = new SqlParameter("@user", SqlDbType.VarChar, 255);
            //sqlParameters[0].Value = user;
            return conn.executeSelectStoredProcedure(query);
        }
    }
}
