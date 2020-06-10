using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using streebo.METIS.BLL;
using System.Data;

namespace streebo.METIS.UI
{
    public partial class AppWindow : System.Web.UI.Page
    {
        private readonly DepartmentManager depManager = DepartmentManager.Instance;
        #region Object Declaration
        private MetisBLL objBLL;
        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {

            Boolean b_CanInsert= false;

            objBLL = new MetisBLL();
            System.Data.DataTable dt = objBLL.getAccessRights(Convert.ToString(Session["user"]));
            foreach (System.Data.DataRow row in dt.Rows)
            {
                if (row["EntityName"].ToString() == "ResSum") { b_CanInsert = Convert.ToBoolean(row["Can_Insert"]); }
            }
            //If Admin thn bypass security
            if (b_CanInsert == false)
            {
                objBLL = new MetisBLL();
                if (Convert.ToBoolean(objBLL.IsAdmin(Convert.ToString(Session["user"]))))
                    b_CanInsert = true;


            }
            if (b_CanInsert)
            {

                
                string id = Request.QueryString["id"].ToString();
                if (string.IsNullOrEmpty(id) || id.ToLower() == "null")
                {
                    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "abc", "<script type='text/javascript'>window.close();</script>");

                }
                else
                {


                    //Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "abc", "<script type='text/javascript'>window.close();</script>");
                    objBLL = new MetisBLL();
                    DataTable dat = objBLL.getBulkName(Convert.ToInt16(id));
                    comboProjectName.DataSource = dat;
                    comboProjectName.DataTextField = dat.Columns[1].ToString();
                    comboProjectName.DataValueField = dat.Columns[0].ToString();
                    comboProjectName.DataBind();

                    dWeekEnding.SelectedDate = DateTime.Today;


                    objBLL = new MetisBLL();
                    dat = objBLL.getBulkResource(Convert.ToInt16(id));
                    comboResourceName.DataSource = dat;
                    comboResourceName.DataValueField = dat.Columns[0].ToString();
                    comboResourceName.DataBind();
                    dWeekEnding.Enabled = false;
                    Label7.Visible = false;
                }
                
                //objBLL = new MetisBLL();
                //DataTable dat = objBLL.getBulkName(Convert.ToInt16(id));
                //comboProjectName.DataSource = dat;
                //comboProjectName.DataTextField = dat.Columns[1].ToString();
                //comboProjectName.DataValueField = dat.Columns[0].ToString();
                //comboProjectName.DataBind();

                //dWeekEnding.SelectedDate = DateTime.Today;


                //objBLL = new MetisBLL();
                //dat = objBLL.getBulkResource(Convert.ToInt16(id));
                //comboResourceName.DataSource = dat;
                //comboResourceName.DataValueField = dat.Columns[0].ToString();
                //comboResourceName.DataBind();

              
               
            }
            
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "abc", "<script type='text/javascript'>window.close();</script>");
            
            }

           
            //string script = "clientClose('');";

            //ScriptManager.RegisterStartupScript(Page, typeof(Page),
            //   "closeScript", script, true);
        }
        protected DateTime GetDateInCurrentWeek(DayOfWeek dw)
        {
            DayOfWeek now = DateTime.Now.DayOfWeek;
            int daysDif = dw - now;
            return DateTime.Now.AddDays(daysDif);
        }
        protected int GetNoOfWeeks(DateTime start, DateTime end)
        {
            int counter = 0;
            while (start <= end)
            {
                start = start.AddDays(7);
                counter++;
            }
            return counter;
        }
        protected void LoadControl(DropDownList ddl, string ctrlIdentity)
        {
            objBLL = new MetisBLL();

            switch (ctrlIdentity)
            {
                case "Department":
                    DataTable dt = depManager.getDeparments();
                    ddl.DataSource = dt;
                    ddl.DataTextField = dt.Columns[1].ToString();
                    ddl.DataValueField = dt.Columns[0].ToString();
                    ddl.DataBind();
                    break;

                case "Resource":
                    dt = objBLL.getAllResources(Session["user"].ToString());
                    ddl.DataSource = dt;
                    ddl.DataTextField = dt.Columns[1].ToString();
                    ddl.DataValueField = dt.Columns[0].ToString();
                    ddl.DataBind();
                    break;

            }

        }


        //protected void InsertButton_Click(object sender, EventArgs e, DateTime dWeekEnding, string comboResourceName, string comboProjectName, DateTime calendarStartDate, DateTime calendarEndDate, float txtWorkLoad)
        //{



        //}

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            CancelButton.CausesValidation = false;
        }

        protected void InsertButton_Click(object sender, EventArgs e)
        {
           

                DateTime dayWeekEnding = (DateTime)dWeekEnding.SelectedDate;
                string sResourceID = Request.QueryString["id"].ToString();
                string sProjectID = comboProjectName.SelectedValue;
                DateTime dStartDate = (DateTime)calenderStartDate.SelectedDate;
                DateTime dEndDate = (DateTime)calenderEndDate.SelectedDate;
                float fWorkLoad = float.Parse(txtWorkLoad.Text);

                //BulkInsert(dayWeekEnding, sResourceID, sProjectID, dStartDate, dEndDate, fWorkLoad);

                
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "abc", "<script type='text/javascript'>window.close();</script>");

                
            
            
        }
        //protected void InsertButton_Click(object sender, EventArgs e)
        //{

        //  BulkInsert((DateTime)dWeekEnding.SelectedDate, comboResourceName.SelectedItem.ToString(), comboProjectName.SelectedItem.ToString(), (DateTime)calenderStartDate.SelectedDate, (DateTime)calenderEndDate.SelectedDate, float.Parse (txtWorkLoad.Text));

        //}

        //protected void BulkInsert(DateTime dWeekEnding, string sResourceID, string sProjectID, DateTime dStartDate, DateTime dEndDate, float fWorkLoad)
        //{
        //    try
        //    {
        //        float Sunday = 0;
        //        float Monday = 0;
        //        float Tuesday = 0;
        //        float Wednesday = 0;
        //        float Thursday = 0;
        //        float Friday = 0;
        //        float Saturday = 0;
        //        string BulkAss = "true";
        //        string IsDeleted = "";
        //        string WorkDays = "";
        //        string AvailableDays = "";
        //        string p_message = "";


        //        double work_load = new double();
        //        string[] workloadPercent;
        //        if (fWorkLoad.ToString().Contains('%'))
        //        {
        //            workloadPercent = fWorkLoad.ToString().Split('%');
        //            if (workloadPercent[0] != null)
        //            {
        //                work_load = (double.Parse(workloadPercent[0]) / 100) * 9;
        //            }
        //        }

        //        else
        //            work_load = double.Parse(fWorkLoad.ToString());


        //        /*Parent Entry*/
        //        objBLL = new MetisBLL();
        //        objBLL.insertBulkAssignment(sResourceID, sProjectID, WorkDays, AvailableDays, dWeekEnding, Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, BulkAss, work_load.ToString(), dStartDate.ToString(), dEndDate.ToString(), IsDeleted, out p_message);




        //        /*Child Entries*/
        //        TimeSpan diff = dEndDate - dStartDate;
        //        int days = diff.Days;
        //        for (var i = 0; i <= days; i++)
        //        {
        //            var tempDate = dStartDate.AddDays(i);
        //            switch (tempDate.DayOfWeek)
        //            {
        //                case DayOfWeek.Monday:
        //                    Monday = float.Parse(work_load.ToString());
        //                    break;

        //                case DayOfWeek.Tuesday:
        //                    Tuesday = float.Parse(work_load.ToString());
        //                    break;

        //                case DayOfWeek.Wednesday:
        //                    Wednesday = float.Parse(work_load.ToString());
        //                    break;

        //                case DayOfWeek.Thursday:
        //                    Thursday = float.Parse(work_load.ToString());
        //                    break;

        //                case DayOfWeek.Friday:
        //                    Friday = float.Parse(work_load.ToString());
        //                    break;

        //                case DayOfWeek.Saturday:
        //                    objBLL.insertBulkAssignment(sResourceID,
        //                    sProjectID, WorkDays, AvailableDays, tempDate,
        //                    Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday,
        //                    "", "", "", "", "", out p_message);
        //                    Monday = 0;
        //                    Tuesday = 0;
        //                    Wednesday = 0;
        //                    Thursday = 0;
        //                    Friday = 0;
        //                    break;
        //            }


        //        }

        //        /*Last Week Entry*/
        //        DateTime WeekEnding = new DateTime();
        //        switch (dEndDate.DayOfWeek)
        //        {

        //            case DayOfWeek.Monday:
        //                Monday = float.Parse(work_load.ToString());
        //                WeekEnding = dEndDate.AddDays(5);
        //                break;


        //            case DayOfWeek.Tuesday:
        //                Tuesday = float.Parse(work_load.ToString());
        //                WeekEnding = dEndDate.AddDays(4);
        //                break;

        //            case DayOfWeek.Wednesday:
        //                Wednesday = float.Parse(work_load.ToString());
        //                WeekEnding = dEndDate.AddDays(3);
        //                break;

        //            case DayOfWeek.Thursday:
        //                Thursday = float.Parse(work_load.ToString());
        //                WeekEnding = dEndDate.AddDays(2);
        //                break;

        //            case DayOfWeek.Friday:
        //                Friday = float.Parse(work_load.ToString());
        //                WeekEnding = dEndDate.AddDays(1);
        //                break;

        //        }
        //        objBLL.insertBulkAssignment(sResourceID,
        //                 sProjectID, WorkDays, AvailableDays, WeekEnding,
        //                 Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday,
        //                 "", "", "", "", "", out p_message);


        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

        //    }
        //}

     

        protected void comboProjectName_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

        protected void calenderStartDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {

        }

        protected void calenderEndDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            if (calenderStartDate.SelectedDate >= calenderEndDate.SelectedDate)
            {
                Label7.Visible = true;
                InsertButton.Enabled = false;
            }
            else
            {

                Label7.Visible = false;
                InsertButton.Enabled = true;
            }

        }

        protected void txtWorkLoad_TextChanged(object sender, EventArgs e)
        {

        }

        protected void dWeekEnding_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {

        }

        protected void comboResourceName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

     


    }

}





        
       





