using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using streebo.METIS.BLL;
using System.Data;

namespace streebo.METIS.UI
{
  
    
    public partial class ResourceSummary : System.Web.UI.Page
    {

        private MetisBLL objBLL;
        public int iWeekHeaderCount = 0;
        string[] Weekending_array;
        static int iisLogin;
        
       
    
        protected void Page_Load(object sender, EventArgs e)
        {
            
            try
            {
                if (!IsPostBack)
                {
                    iisLogin = (int)Session["isLogin"];

                    if (iisLogin != 1)
                        Response.Redirect("Login.aspx");

                  //LoadGrid
                    LoadControl(rgWeekly, "Weekly");
                  
                  
                   

                }
                
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }
          }



        #region FilterControl Events
        protected void ddlDepartment_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

             //   lblcmbDepartment_selectedValue.Text = e.Value.ToString();
            //    lblcmbDepartment_selectedText.Text = e.Text.ToString();
            //    all = "";
            //    notAssigned = "--";

            //    if (e.Text == "All")
            //    { all = "--"; notAssigned = "--"; }

            //    if (e.Text == "Not Assigned")
            //    {
            //        all = ""; notAssigned = "";
            //    }
            //    string q = string.Format(Settings.Default.qrytest, dpWeekEnding.SelectedDate, all, e.Value, notAssigned);
            //    rg1DataSet = SQLDataConnection.GetInstance().ExecuteDataSet(q);
            //    RadToolTipManager1.TargetControls.Clear();
            //    RadGrid1.DataSource = rg1DataSet;
            //    RadGrid1.Rebind();
            //    int c = RadToolTipManager1.TargetControls.Count;


            //    RadGrid1.Rebind();
            //    RadGrid_weekly.Rebind();

            //    if (cmbReportType.SelectedValue == "2")
            //        RadGrid1.Focus();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }

        }

        protected void Calendar_OnDayRender(object sender, Telerik.Web.UI.Calendar.DayRenderEventArgs e)
        {
            try
            {
                // modify the cell rendered content for the days we want to be disabled (e.g. every Saturday and Sunday)
                if (e.Day.Date.DayOfWeek != DayOfWeek.Saturday) //|| e.Day.Date.DayOfWeek == DayOfWeek.Sunday)
                {
                    // if you are using the skin bundled as a webresource("Default"), the Skin property returns empty string
                    string calendarSkin = dpWeekEnding.Calendar.Skin != "" ? dpWeekEnding.Calendar.Skin : "Default";
                    string otherMonthCssClass = "rcOutOfRange";

                    // clear the default cell content (anchor tag) as we need to disable the hover effect for this cell
                    e.Cell.Text = "";
                    e.Cell.CssClass = otherMonthCssClass; //set new CssClass for the disabled calendar day cells (e.g. look like other month days here)

                    // render a span element with the processed calendar day number instead of the removed anchor -- necessary for the calendar skinning mechanism 
                    Label label = new Label();
                    label.Text = e.Day.Date.Day.ToString();
                    e.Cell.Controls.Add(label);

                    // disable the selection for the specific day
                    RadCalendarDay calendarDay = new RadCalendarDay();
                    calendarDay.Date = e.Day.Date;
                    calendarDay.IsSelectable = false;
                    calendarDay.ItemStyle.CssClass = otherMonthCssClass;
                    dpWeekEnding.Calendar.SpecialDays.Add(calendarDay);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }
        }
        
        protected void OnSelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            try
            {

                //Department_chk();
                //string s = string.Format(Settings.Default.qrytest, dpWeekEnding.SelectedDate, all, lblcmbDepartment_selectedValue.Text, notAssigned);
                //rg1DataSet = SQLDataConnection.GetInstance().ExecuteDataSet(s);
                //RadToolTipManager1.TargetControls.Clear();
                //RadGrid1.DataSource = rg1DataSet;
                //RadGrid1.DataBind();
                //RadGrid_weekly.Rebind();
                //RadGrid1.Focus();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }

        }

        protected void rcReportType_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {

            try
            {
                //RadGrid1.Controls.Clear();
                //RadGrid_weekly.Controls.Clear();

                //if (e.Value.ToString() == "1")
                //{

                //    RadGrid1.Visible = false;
                //    RadGrid_weekly.Visible = true;
                //    RadGrid_weekly.Rebind();
                //    RadGrid_weekly.Focus();


                //}
                //if (e.Value.ToString() == "2")
                //{

                //    RadGrid1.Visible = true;
                //    RadGrid_weekly.Visible = false;
                //    RadGrid1.Rebind();
                //    RadGrid1.Focus();

                //}

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }


        }

        protected void rcResc_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {


        }
        #endregion

        #region FilterControl Methods
        protected void LoadControl(DropDownList ddl, string ctrlIdentity)
        {
            objBLL = new MetisBLL();

            switch (ctrlIdentity)
            {
                case "Department":
                    DataTable dt = objBLL.getDeparments();
                    ddl.DataSource = dt;
                    ddl.DataTextField = dt.Columns[1].ToString();
                    ddl.DataValueField = dt.Columns[0].ToString();
                    ddl.DataBind();
                    break;

            }
        }

        protected void LoadControl(RadGrid rg, string ctrlIdentity)
        {
            objBLL = new MetisBLL();
            DataTable dt = objBLL.getWeeklyData();
            rgWeekly.DataSource = dt;
            rgWeekly.DataBind();
        }
        #endregion

        #region Grid Events
        protected void rgWeekly_PreRender_detail(object sender, EventArgs e)
        {

        }

        protected void rgWeekly_PreRender(object sender, EventArgs e)
        {
            try
            {
                GridColumn gridCol = rgWeekly.MasterTableView.GetColumn("Resource_name");
                gridCol.Visible = false;
                GridColumn gridCol1 = rgWeekly.MasterTableView.GetColumn("Resource_id");
                gridCol1.Visible = false;
                gridCol1 = rgWeekly.MasterTableView.GetColumn("Resource_id");
                gridCol1.Visible = false;
             }
            
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }

          }

        protected void rgWeekly_ItemCreated(object sender, GridItemEventArgs e)
        {
           

        }

        protected void rgWeekly_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {

                if (e.Item is GridDataItem && e.Item.OwnerTableView.Name == "DetailTable")
                {
                    GridTableView detailTable = (GridTableView)e.Item.OwnerTableView;
                    detailTable.GetColumn("Resource_id").Visible = false;

                    detailTable = (GridTableView)e.Item.OwnerTableView;
                    detailTable.GetColumn("Resource_Name").Visible = false;

                    detailTable = (GridTableView)e.Item.OwnerTableView;
                    detailTable.GetColumn("Project_id").Visible = false;


                }
                if (e.Item.OwnerTableView.Name == "MasterTable")
                {
                    if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
                    {
                        Control target = e.Item.FindControl("targetControl2");
                        if (!Object.Equals(target, null))
                        {
                            if (!Object.Equals(this.RadToolTipManager1, null))
                            {
                               //Add the button (target) id to the tooltip manager
                               // this.radRadToolTipManager1.TargetControls.Add(target.ClientID, (e.Item as GridDataItem).GetDataKeyValue("Resource_id").ToString(), true);

                            }
                        }
                    }
                    if (e.Item is GridDataItem)
                    {
                        GridDataItem dataBoundItem = e.Item as GridDataItem;

                        if (iWeekHeaderCount == Weekending_array.Length)
                            iWeekHeaderCount = 0;

                        while (iWeekHeaderCount < Weekending_array.Length)
                        {
                            string headerName = Weekending_array[iWeekHeaderCount];
                            if (dataBoundItem[headerName].Text != "&nbsp;")
                            {

                                if (Convert.ToDouble(dataBoundItem[headerName].Text) >= 40.1)
                                {
                                    dataBoundItem[headerName].BackColor = System.Drawing.ColorTranslator.FromHtml("#E34234");
                                    dataBoundItem[headerName].ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                                    dataBoundItem[headerName].Style.Add("font-weight", "bold");
                                }
                                if (Convert.ToDouble(dataBoundItem[headerName].Text) >= 20 && Convert.ToDouble(dataBoundItem[headerName].Text) <= 40)
                                {
                                    dataBoundItem[headerName].BackColor = System.Drawing.ColorTranslator.FromHtml("#1C881C");
                                    dataBoundItem[headerName].ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                                    dataBoundItem[headerName].Style.Add("font-weight", "bold");


                                }
                                if (Convert.ToDouble(dataBoundItem[headerName].Text) >= 0.1 && Convert.ToDouble(dataBoundItem[headerName].Text) <= 19.9)
                                {
                                    dataBoundItem[headerName].BackColor = System.Drawing.ColorTranslator.FromHtml("#F4F41E");
                                    dataBoundItem[headerName].ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                                    dataBoundItem[headerName].Style.Add("font-weight", "normal");

                                }
                                if (dataBoundItem[headerName].Text == "&nbsp;" || Convert.ToDouble(dataBoundItem[headerName].Text) == 0)
                                {
                                    dataBoundItem[headerName].BackColor = System.Drawing.ColorTranslator.FromHtml("#E34234");
                                    dataBoundItem[headerName].ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                                    dataBoundItem[headerName].Style.Add("font-weight", "bold");

                                }
                            }
                            iWeekHeaderCount++;
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }

        }

        protected void rgWeekly_UpdateCommand(object source, GridCommandEventArgs e)
        {
            
        }

        protected void rgWeekly_ItemCommand(object sender, GridCommandEventArgs e)
        {
           


        }

        protected void rgWeekly_DetailTableDataBind(object source, GridDetailTableDataBindEventArgs e)
        {

        }
        #endregion

        protected DateTime GetStartOfWeek()
        {
            DayOfWeek now = DateTime.Now.DayOfWeek;
            int daysDif = now - DayOfWeek.Sunday;
            return DateTime.Now.AddDays(-daysDif);
        }

        #region Misc
        protected void btnCollapseAllClick(object sender, EventArgs e)
        {
            try
            {
                rgWeekly.MasterTableView.HierarchyDefaultExpanded = false;
                //RadGrid1.MasterTableView.HierarchyDefaultExpanded = false;
                //RadGrid_weekly.Rebind();
                //RadGrid1.Rebind();
                // If you not used Advance data binding then again Bind // RadGrid1.DataSource = ""; RadGrid1.DataBind();


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }
        }

        protected void btnExpandAllClick(object sender, EventArgs e)
        {
            try
            {
                rgWeekly.MasterTableView.HierarchyDefaultExpanded = true;
                //RadGrid1.MasterTableView.HierarchyDefaultExpanded = true;
                //RadGrid_weekly.Rebind();
                //RadGrid1.Rebind();
                // If you not used Advance data binding then again Bind // RadGrid1.DataSource = ""; RadGrid1.DataBind();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }

        }

        protected void lbLogout_OnClick(object sender, EventArgs e)
        {
            try
            {
                Session["isLogin"] = 0;
                Response.Redirect("Login.aspx");

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }
        }

        protected void btnClose_OnClick(object sender, EventArgs e)
        {
           // radwindowPopup.VisibleOnPageLoad = false;
            //rgWeekly.Rebind();

        }
        #endregion
    
    }
}
