using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.Data;
using System.Configuration;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using Telerik.Web.UI.GridExcelBuilder;
using System.Globalization;
using streebo.METIS.BLL;


namespace streebo.METIS.UI
{


    public partial class ResSum : System.Web.UI.Page
    {
        private readonly DepartmentManager depManager = DepartmentManager.Instance;
        #region Object Declaration
        private MetisBLL objBLL;
        #endregion

        #region Variable Declaration
        //static int iisLogin;
        public int iWeekHeaderCount = 0;
        public int iWeekDetailHeaderCount = 0;
        public int emptyCellsCounter = 0; //For counting empty cells in a row in detail table
        public int hideRowCounter = 0; //How many rows are hided in detail table out of total rows
        public int rowsInDetailTable;
        public List<int> expandRecordList = new List<int>();
        int NoOfWeeks;
        int WeekHeaderCount;
        int iTotalNoofDT = 0;
        int iDivCount;
        Hashtable htWeekEndings;
        public static string sQry = "";
        public static string sQry_bulk = "";
        string[] Weekending_array;
        DateTime WeekStarting;

        #endregion

        protected void OnAjaxUpdate(object sender, ToolTipUpdateEventArgs args)
        {
            this.UpdateToolTip(args.Value, args.UpdatePanel);
        }
        
        private void UpdateToolTip(string elementID, UpdatePanel panel)
        {
            Control ctrl = Page.LoadControl("DetailsCS.ascx");
            panel.ContentTemplateContainer.Controls.Add(ctrl);
            DetailsCS details = (DetailsCS)ctrl;
            details.ResourceId = elementID;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            

            //int x = Convert.ToInt32(Session["isLogin"]);

            //if (x == 0)
            //    RFSs.Visible = false;
            //else            
            //    RFSs.Visible = true;            

            try
            {
                
                if (ddlResourceView.SelectedValue == "2")
                    chkavailres.Visible = true;
                else
                    chkavailres.Visible = false;
                #region Check Login
                if (!IsPostBack)
                {
                    if (Convert.ToString(Session["user"]) == "")
                        Response.Redirect("Login.aspx");
                }

                #endregion

                if (!IsPostBack)
                {

                    if (PropertyLayer.ResourceFileNameEN == "ResourceEN") DropDownListLanguage.SelectedValue = "English";
                    if (PropertyLayer.ResourceFileNameEN == "ResourceRU") DropDownListLanguage.SelectedValue = "Russian";
                    if (PropertyLayer.ResourceFileNameEN == "ResourceKZ") DropDownListLanguage.SelectedValue = "kazakh";

                    lblResourceSummary.Text = PropertyLayer.ResourceSummary;

                    lblProjectSummary.Text = PropertyLayer.ProjectSummary;
                    lblAssignments.Text = PropertyLayer.Assignments;
                    lbLogout.Text = PropertyLayer.Logout;
                    LabelLanguage.Text = PropertyLayer.Language;


                    Label6.Text = PropertyLayer.NoEmployee;
                    //No.of Employees:
                    Label8.Text = PropertyLayer.UtilizePersentage;
                    //Utilization Percentage(For Selected Period):
                    Label10.Text = PropertyLayer.UtilizePersentage4Week;
                    //Utilization Percentage(Next 4 Weeks):
                    Label1.Text = PropertyLayer.Overloaded;
                    //Overloaded
                    Label2.Text = PropertyLayer.PartiallyLoaded;
                    //Partially Loaded
                    Label3.Text = PropertyLayer.Underloaded;
                    //Underloaded
                    Label4.Text = PropertyLayer.FullLoaded;
                    //Full Loaded


                    Boolean b_CanView = false;

                    objBLL = new MetisBLL();

                    System.Data.DataTable dt = objBLL.getAccessRights(Convert.ToString(Session["user"]));
                    foreach (System.Data.DataRow row in dt.Rows)
                    {
                        if (row["EntityName"].ToString() == "ResSum") { b_CanView = Convert.ToBoolean(row["Can_View"]); }
                    }
                    //If Admin thn bypass security
                    if (b_CanView == false)
                    {
                        objBLL = new MetisBLL();
                        if (Convert.ToBoolean(objBLL.IsAdmin(Convert.ToString(Session["user"]))))
                            b_CanView = true;
                    }
                    if (b_CanView)
                    {
                        dpWeekStarting.SelectedDate = GetDateInCurrentWeek(DayOfWeek.Monday);
                        dpEnding.SelectedDate = GetDateInCurrentWeek(DayOfWeek.Monday).AddDays(7 * 8);
                   

                        #region Load Controls
                        LoadControl(comDepartment, "Departments");
                        LoadControl(comResource, "Resources");
                        
                        #endregion

                        #region Load Grid
                        LoadControl(RadGrid_weekly, "RadGrid_Weekly");

                        #endregion
                    }
                    else
                    {
                        Response.Redirect("ProjectSum.aspx", false);
                    }
                }
                WeekStarting = (DateTime)dpWeekStarting.SelectedDate;
                // RadToolTipManager1.TargetControls.Clear();

                #region Creating Hashtable
                NoOfWeeks = GetNoOfWeeks((DateTime)dpWeekStarting.SelectedDate, (DateTime)dpEnding.SelectedDate);
                Weekending_array = new string[NoOfWeeks];
                htWeekEndings = new Hashtable(NoOfWeeks);
                #endregion

                #region Preparing WeekEndings

                for (int i = 0; i < NoOfWeeks; i++)
                {

                    Weekending_array[i] = WeekStarting.ToString("dd") + " " + WeekStarting.ToString("MMM") + " " + WeekStarting.ToString("yyyy");
                    WeekStarting = WeekStarting.AddDays(7);

                }
                #endregion

              
            }

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);
                //System.Diagnostics.Trace.TraceInformation("Information messge");
                //System.Diagnostics.Trace.TraceError("Error messge");
                //System.Diagnostics.Trace.Write(ex.Message);
                String date = System.DateTime.Today.Day.ToString() + "-" + System.DateTime.Today.Month.ToString() + "-" + System.DateTime.Today.Year.ToString();
                 // date.Replace(' ', '\0'); 
                date = "C:/logs/metis_" + date + ".txt";

                TextWriter check = new StreamWriter(date, true);

                check.WriteLine(System.DateTime.Now);
                check.WriteLine();
                check.WriteLine(ex.Message);
                check.Close();
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
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
    
        //protected void LoadControl(DropDownList ddl, string ctrlIdentity)
        //{
        //    objBLL = new MetisBLL();

        //    switch (ctrlIdentity)
        //    {
        //        case "Department":
        //            DataTable dt = objBLL.getDeparments();
        //            ddl.DataSource = dt;
        //            ddl.DataTextField = dt.Columns[1].ToString();
        //            ddl.DataValueField = dt.Columns[0].ToString();
        //            ddl.DataBind();
        //            break;

        //        case "Resource":
        //            dt = objBLL.getAllResources(Session["user"].ToString());
        //            ddl.DataSource = dt;
        //            ddl.DataTextField = dt.Columns[1].ToString();
        //            ddl.DataValueField = dt.Columns[0].ToString();
        //            ddl.DataBind();
        //            break;

        //    }
        //}

        protected void LoadControl(RadComboBox combo, string ctrlIdentity)
        {
            switch (ctrlIdentity)
            {
                case "Departments":
                    DataTable dt = depManager.getDeparments();
                    combo.DataSource = dt;
                    combo.DataTextField = dt.Columns[1].ToString();
                    combo.DataValueField = dt.Columns[0].ToString();
                    combo.DataBind();
                    combo.AllowCustomText = true;
                    if (Session["ddlDepartment"] != null)
                    {
                        combo.SelectedValue = Session["ddlDepartment"].ToString();
                    }
                    Session["ddlDepartment"] = combo.SelectedValue;
                   
                    break;

                case "Resources":
                    dt = objBLL.getAllResources(Session["user"].ToString());
                    combo.DataSource = dt;
                    combo.DataTextField = dt.Columns[1].ToString();
                    combo.DataValueField = dt.Columns[0].ToString();
                    combo.DataBind();
                    combo.AllowCustomText = true;
              
                    break;
            }
        }

        protected void LoadControl(RadGrid rg, string ctrlIdentity)
        {
            

            objBLL = new MetisBLL();

            switch (ctrlIdentity)
            {
                case "RadGrid_Weekly":

                    DataTable dt = objBLL.getResourceSummary((DateTime)dpWeekStarting.SelectedDate, (DateTime)dpEnding.SelectedDate, Session["user"].ToString());
                    //RadGrid_weekly.DataSource = dt;
                    // Sarfaraz 02Jan19
                    // Added following code to load resources in grid based on selected department and selected Resource.
                    // When user come back from other tab, combo shows selected department but the resources in grid are not filtered based on selected department.
                    Session["ResourceList"] = dt.Columns[1]; 

                    DataView dv = new DataView(dt);
                    dv.RowFilter = "Dept_name like '%" + (comDepartment.Text.ToString().Replace("'", "''") == "All" ? "" : comDepartment.Text.ToString().Replace("'", "''")) + "%'";
                    dv.RowFilter += "AND Resource_name LIKE '%" + (comResource.Text.ToString().Replace("'", "''") == "All" ? "" : comResource.Text.ToString().Replace("'", "''")) + "%'";
                    Session["dview"] = dv;
                    RadGrid_weekly.DataSource = dv;
                    showUtilizationPercentage(dv.ToTable());
                    /*
                    decimal t = 0;
                    decimal z = 0;
                    String test;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 3; j < dt.Columns.Count; j++)
                        {
                            test = dt.Rows[i][j].ToString();

                            String[] t1 = test.Split('(');

                            t += Decimal.Parse(t1[0]);

                        }
                    }
                    // Change By: Sarfaraz Dated: 12 Dec18
                    // added condition to handle zero rows or zero columns to avoid divide by zero error

                    decimal talha = dt.Rows.Count > 0 && dt.Columns.Count > 0 ? (decimal)Math.Round((t / ((dt.Columns.Count - 3) * dt.Rows.Count * 40)) * 100, 3) :0;
                    Label7.Text = Convert.ToString(talha) + " %";
                   
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 4; j <= dt.Columns.Count - 5; j++)
                        {
                            test = dt.Rows[i][j].ToString();

                            String[] t1 = test.Split('(');

                            z += Decimal.Parse(t1[0]);

                        }
                    }
                    // Change By: Sarfaraz Dated: 12 Dec18
                    // added condition to handle zero rows or zero columns to avoid divide by zero error

                    decimal talhaz = dt.Rows.Count > 0 && dt.Columns.Count > 0 ? (decimal)Math.Round((z / ((dt.Columns.Count - 8) * dt.Rows.Count * 40)) * 100, 3) :0;
                    Label9.Text = Convert.ToString(talhaz) + " %";
                   */
                   
           

                    break;
            }
        }

        protected DateTime GetDateInCurrentWeek(DayOfWeek dw)
        {
            DayOfWeek now = DateTime.Now.DayOfWeek;
            int daysDif = dw - now;
            return DateTime.Now.AddDays(daysDif);
        }

        public string PrintUserName(string username)
        {
            char[] delimiterChars = { '.' };
            string[] words = username.Split(delimiterChars);
            words[0] = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(words[0]);
            words[1] = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(words[1]);
            username = words[0] + " " + words[1];
            return username;
        }


        protected void DropDownListLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strLang = DropDownListLanguage.SelectedValue;
            if (strLang == "English")
                PropertyLayer.ResourceFileNameEN = "ResourceEN";
            else
            if (strLang == "Russian")
                PropertyLayer.ResourceFileNameEN = "ResourceRU";
            else
            if (strLang == "kazakh")
                PropertyLayer.ResourceFileNameEN = "ResourceKZ";

            Response.Redirect(Request.RawUrl);
        }

        #region Weekly Master & DetailTable

        protected void RadGrid_weekly_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (e.IsFromDetailTable)
            {
                objBLL = new MetisBLL();
                //DataTable dtable = objBLL.getResourceDetail(Convert.ToInt32(((string)Session["Resource_id"])), (DateTime)dpWeekStarting.SelectedDate, (DateTime)dpEnding.SelectedDate);

                DataTable dtable = objBLL.getResourceDetail(Convert.ToInt32(((string)Session["Resource_id"])), (DateTime)dpWeekStarting.SelectedDate, (DateTime)dpEnding.SelectedDate);
                RadGrid_weekly.DataSource = dtable;
            }
            else
            {
                Session["expandRecordList"] = null;
                objBLL = new MetisBLL();
                DataTable dtable = new DataTable();

                if (ddlResourceView.SelectedValue == "1")
                {
                    dtable = objBLL.getResourceSummary((DateTime)dpWeekStarting.SelectedDate, (DateTime)dpEnding.SelectedDate, Session["user"].ToString());
                     Session["ResourceList"] = dtable.Columns[1]; 

                    DataView dv = new DataView(dtable);
                    dv.RowFilter = "Dept_name like '%" + (comDepartment.Text.ToString().Replace("'", "''") == "All" ? "" : comDepartment.Text.ToString().Replace("'", "''")) + "%'";
                    dv.RowFilter += "AND Resource_name LIKE '%" + (comResource.Text.ToString().Replace("'", "''") == "All" ? "" : comResource.Text.ToString().Replace("'", "''")) + "%'";
                    Session["dview"] = dv;
                    RadGrid_weekly.DataSource = dv;
                }
                else
                {
                    dtable = objBLL.getResourceAvailSummary((DateTime)dpWeekStarting.SelectedDate, (DateTime)dpEnding.SelectedDate, Session["user"].ToString());
                    Session["ResourceList"] = dtable.Columns[1];

                    DataView dv = new DataView(dtable);
                    dv.RowFilter = "Dept_name like '%" + (comDepartment.Text.ToString().Replace("'", "''") == "All" ? "" : comDepartment.Text.ToString().Replace("'", "''")) + "%'";
                    dv.RowFilter += "AND Resource_name LIKE '%" + (comResource.Text.ToString().Replace("'", "''") == "All" ? "" : comResource.Text.ToString().Replace("'", "''")) + "%'";

                    if (chkavailres.Checked)
                    {
                        WeekHeaderCount = 0;
                        while (WeekHeaderCount < NoOfWeeks)
                        {
                            string headerName = Weekending_array[WeekHeaderCount];
                            dv.RowFilter += "AND [" + headerName + "] > 1";
                            WeekHeaderCount++;
                        }
                    }
                    Session["dview"] = dv;
                    RadGrid_weekly.DataSource = dv;

                }

            }
        }

        protected void RadGrid_weekly_PreRender(object sender, EventArgs e)
        {
            try
            {
                GridColumn gridCol = RadGrid_weekly.MasterTableView.GetColumn("Resource_name");
                gridCol.Visible = false;
                gridCol = RadGrid_weekly.MasterTableView.GetColumn("Resource_id");
                gridCol.Visible = false;
                gridCol = RadGrid_weekly.MasterTableView.GetColumn("Dept_name");
                gridCol.Visible = false;

                if (Session["expandRecordList"] != null)
                {
                    expandRecordList = (List<int>)Session["expandRecordList"];

                    foreach (GridDataItem item in RadGrid_weekly.MasterTableView.Items)
                    {
                        if (expandRecordList.Contains(item.ItemIndex))
                            item.Expanded = true;

                    }
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }


        }

        protected void RadGrid_weekly_ItemCreated(object sender, GridItemEventArgs e)
        {

            if (e.Item is GridNestedViewItem)
            {
                GridNestedViewItem nestedItem = e.Item as GridNestedViewItem;
                nestedItem.NestedViewCell.PreRender += new EventHandler(NestedViewCell_PreRender);
            }

        }

        protected void RadGrid_weekly_ItemDataBound(object sender, GridItemEventArgs e)
        {

            try
            {
                // RadGrid_weekly.MasterTableView.GetColumn("URole").HeaderText = PropertyLayer.RoleName;
               

                int i = RadGrid_weekly.Items.Count;
                Label5.Text = i.ToString();
                if (e.Item is GridHeaderItem && e.Item.OwnerTableView.Name == "DetailTable")
                {
                    // Hide Useless Columns
                    GridTableView detailTable = (GridTableView)e.Item.OwnerTableView;
                    detailTable.GetColumn("Resource_id").Visible = false;
                    detailTable.GetColumn("Resource_Name").Visible = false;
                    detailTable.GetColumn("Project_id").Visible = false;
                    detailTable.GetColumn("Project").Visible = false;
                    detailTable.GetColumn("Role_Title").Visible = false;
                }

                if (e.Item is GridItem && e.Item.OwnerTableView.Name == "DetailTable")
                {
                    if (e.Item is GridHeaderItem)
                    {
                        GridHeaderItem itemHeader = (GridHeaderItem)e.Item;
                        Session["itemHeader"] = itemHeader;
                    }

                    if (e.Item is GridDataItem)
                    {
                        GridDataItem item = (GridDataItem)e.Item;


                        if (iWeekDetailHeaderCount == Weekending_array.Length)
                        {
                            iWeekDetailHeaderCount = 0;
                            emptyCellsCounter = 0;
                        }

                        while (iWeekDetailHeaderCount < NoOfWeeks)
                        {
                            string headerName = Weekending_array[iWeekDetailHeaderCount];

                            if (item[headerName].Text == "&nbsp;")
                            {
                                emptyCellsCounter++;
                                if (emptyCellsCounter == NoOfWeeks)
                                {
                                    //item.Display = false;
                                    hideRowCounter++;
                                }
                                if (hideRowCounter == rowsInDetailTable)
                                {
                                    //((GridHeaderItem)Session["itemHeader"]).Display = false;
                                }

                            }
                            iWeekDetailHeaderCount++;
                        }

                    }
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
                                this.RadToolTipManager1.TargetControls.Add(target.ClientID, (e.Item as GridDataItem).GetDataKeyValue("Resource_id").ToString(), true);

                            }
                        }
            
                    }
                    if (e.Item is GridDataItem)
                    {
                        GridDataItem dataBoundItem = e.Item as GridDataItem;

                        if (Weekending_array != null)
                        {
                            if (iWeekHeaderCount == Weekending_array.Length)
                                iWeekHeaderCount = 0;
                        }
                        while (iWeekHeaderCount < NoOfWeeks)
                        {
                            string headerName = Weekending_array[iWeekHeaderCount];

                            if (dataBoundItem[headerName].Text != "&nbsp;")
                            {
                                if (ddlResourceView.SelectedValue == "1")
                                {
                                    string[] words = dataBoundItem[headerName].Text.Split(' ');

                                    string test = words[0] + words[1];

                                    //Console.WriteLine(test);

                                    String[] spliter = test.Split('(');
                                    spliter[1] = spliter[1].Remove(spliter[1].Length - 1);
                                    String t1 = Convert.ToString(Convert.ToDecimal(spliter[0]) + Convert.ToInt32(spliter[1]));
           

                                   // words[0] = t1;
                                    /*string cals = words[1].Remove(1);
                                    cals = words[1].Remove(words[1].Length - 1);
                                    cals = Convert.ToString((Convert.ToInt32(words[0]) + Convert.ToInt32(cals)));
                                 */
                                    if (Convert.ToDouble(t1) > 40)
                                    {
                                        dataBoundItem[headerName].BackColor = System.Drawing.ColorTranslator.FromHtml("#E34234"); //Red
                                        dataBoundItem[headerName].ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                                        dataBoundItem[headerName].Style.Add("font-weight", "normal");
                                    }

                                    
                                    if (Convert.ToDouble(t1) < 24)
                                    {
                                        dataBoundItem[headerName].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFC7CE"); //Pink
                                        dataBoundItem[headerName].ForeColor = System.Drawing.ColorTranslator.FromHtml("#9C0006");
                                        dataBoundItem[headerName].Style.Add("font-weight", "normal");
                                    }
                                    if (Convert.ToDouble(t1) == 40)
                                    {
                                        dataBoundItem[headerName].BackColor = System.Drawing.ColorTranslator.FromHtml("#1C881C");  //Green
                                        dataBoundItem[headerName].ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                                        dataBoundItem[headerName].Style.Add("font-weight", "normal");


                                    }
                                    if (Convert.ToDouble(t1) >= 24 && Convert.ToDouble(t1) < 40)
                                    {
                                        dataBoundItem[headerName].BackColor = System.Drawing.ColorTranslator.FromHtml("#F4F41E");  //Yellow
                                        dataBoundItem[headerName].ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                                        dataBoundItem[headerName].Style.Add("font-weight", "normal");

                                    }
                                    dataBoundItem[headerName].Text = dataBoundItem[headerName].Text.Replace("(0)", "");
                                }
                                else
                                {

                                  
                                    if (Convert.ToDouble(dataBoundItem[headerName].Text) > 40)
                                    {
                                        dataBoundItem[headerName].BackColor = System.Drawing.ColorTranslator.FromHtml("#E34234"); //Red
                                        dataBoundItem[headerName].ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                                        dataBoundItem[headerName].Style.Add("font-weight", "normal");
                                    }

                                    if (Convert.ToDouble(dataBoundItem[headerName].Text) < 24)
                                    {
                                        dataBoundItem[headerName].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFC7CE"); //Pink
                                        dataBoundItem[headerName].ForeColor = System.Drawing.ColorTranslator.FromHtml("#9C0006");
                                        dataBoundItem[headerName].Style.Add("font-weight", "normal");
                                    }

                                    if (Convert.ToDouble(dataBoundItem[headerName].Text) == 40)
                                    {
                                        dataBoundItem[headerName].BackColor = System.Drawing.ColorTranslator.FromHtml("#1C881C");  //Green
                                        dataBoundItem[headerName].ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                                        dataBoundItem[headerName].Style.Add("font-weight", "normal");


                                    }
                                    if (Convert.ToDouble(dataBoundItem[headerName].Text) >= 24 && Convert.ToDouble(dataBoundItem[headerName].Text) < 40)
                                    {
                                        dataBoundItem[headerName].BackColor = System.Drawing.ColorTranslator.FromHtml("#F4F41E");  //Yellow
                                        dataBoundItem[headerName].ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                                        dataBoundItem[headerName].Style.Add("font-weight", "normal");

                                    }
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
        
        protected void RadGrid_weekly_UpdateCommand(object source, GridCommandEventArgs e)
        {
            try
            {

                if (e.Item.OwnerTableView.Name == "DetailTable")
                {
                    GridDataItem childItem = e.Item.OwnerTableView.ChildEditItems[0] as GridDataItem;
                    string resID = childItem.SavedOldValues["Resource_id"].ToString();
                    string prjId = childItem.SavedOldValues["Project_id"].ToString();
                    GridEditableItem editedItem = e.Item as GridEditableItem;
                    Hashtable newValues = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);
                    Hashtable HTEdited = new Hashtable(newValues.Count);

                    foreach (DictionaryEntry Edited in newValues)
                    {
                        if (Edited.Value != null)
                        {
                            HTEdited.Add(Edited.Key, Edited.Value);//// Get all Fields those inputs are not null

                        }
                    }
                    foreach (DictionaryEntry Edited in HTEdited)
                    {
                        if (Edited.Key != null && Edited.Key.ToString() != "Resource_name" && Edited.Key.ToString() != "Project_id" && Edited.Key.ToString() != "Resource_id" && Edited.Key.ToString() != "Project")
                        {
                            string sHoursOfWeek = newValues[Edited.Key.ToString()].ToString();
                            //float hoursOfWeek = float.Parse(sHoursOfWeek);
                            float hoursOfWeek = 0;
                            bool hoursOfWeek_Check = float.TryParse(sHoursOfWeek, out hoursOfWeek);

                            if (hoursOfWeek_Check)
                            {
                                int hoursPerDay = 0;
                                //if (hoursOfWeek > 0)
                                //{
                                    hoursPerDay = (int) hoursOfWeek / 5;
                                    string weekEndingDate = Edited.Key.ToString();

                                    string p_message = "";
                                    objBLL = new MetisBLL();
                                    objBLL.updateWeeklyReport(resID, prjId, Convert.ToDateTime(weekEndingDate).AddDays(5), hoursPerDay, out p_message);
                                //}
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }
        }

        protected void RadGrid_weekly_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "InitInsert" && e.Item.OwnerTableView.Name == "MasterTable")
            {
                e.Canceled = true;
            }


            if (e.CommandName == "Edit" && e.Item.OwnerTableView.Name == "MasterTable")
            {
                e.Canceled = true;
            }

            if (e.CommandName == "Edit" && e.Item.OwnerTableView.Name == "DetailTable")
            {
                //Response.Write("Primary key for the clicked item from ItemCommand: " + (e.Item as GridDataItem).GetDataKeyValue("CustomerID").ToString() + "<br>");
                //GridDataItem gdi = e.Item as GridDataItem;
                //Session["Resource_id"] = (gdi).GetDataKeyValue("Resource_id").ToString();
                //Session["Resource_id"] = RadGrid_weekly.MasterTableView.DataKeyValues[0].ToString();
                //string x = Session["Resource_id"].ToString();
                Session["Resource_id"] = RadGrid_weekly.MasterTableView.Items[e.Item.RowIndex].GetDataKeyValue("Resource_id").ToString();
                string x = Session["Resource_id"].ToString();
            }
        }

        protected void RadGrid_weekly_DetailTableDataBind(object source, GridDetailTableDataBindEventArgs e)
        {
            try
            {

                GridDataItem parentItem = e.DetailTableView.ParentItem as GridDataItem;
                lblResourceID.Text = parentItem["Resource_id"].Text;


                objBLL = new MetisBLL();
                DataTable dtable = objBLL.getResourceDetail(Convert.ToInt32(lblResourceID.Text), (DateTime)dpWeekStarting.SelectedDate, (DateTime)dpEnding.SelectedDate);             
                
                e.DetailTableView.DataSource = dtable;
                rowsInDetailTable = dtable.Rows.Count;
                hideRowCounter = 0;
                
             }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }
        }
        
        //protected void NestedViewTable_Render(HtmlTextWriter writer, Control control)
        //{
        //    try
        //    {


        //        int divheight = 85;
        //        divheight = 100; //fixed height
        //        control.SetRenderMethodDelegate(null);
        //        iDivCount++;
        //        string genDIV = "<div id=\"div_dt" + iDivCount + "\" style=\"height:" + divheight.ToString() + "px; overflow: auto; overflow-x: scroll; overflow-y: scroll;\" >";
        //        writer.Write("<div id=\"div_dt" + iDivCount + "\" style=\" width: 100%; height:" + divheight.ToString() + "px; overflow: auto; overflow-x: scroll; overflow-y: scroll; border-style:solid;  border-color:#ff9900;  border-width:1px; border-style:solid;\" >");
        //        control.RenderControl(writer);
        //        writer.Write("</div>");
        //        string intervalScript = "var MyInterval= window.setInterval(\"divScroll(" + iDivCount + ")\", 1);";
        //        if (iDivCount == iTotalNoofDT)
        //            ScriptManager.RegisterStartupScript(this, typeof(string), "divScrollKey", "var MyInterval= window.setInterval(\"divScroll(" + iDivCount + ")\", 1);", true); //testing
        //        Session["NoOfDivs"] = iDivCount;

        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

        //    }
        //}

        //private void NestedViewCell_PreRender(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        iTotalNoofDT++;
        //        Control ctrl = sender as Control;
        //        ((Control)sender).Controls[0].SetRenderMethodDelegate(new RenderMethod(this.NestedViewTable_Render));

        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

        //    }
        //}

        private void NestedViewCell_PreRender(object sender, EventArgs e)
        {
            //try
            //{
            //    iTotalNoofDT++;
            //    Control ctrl = sender as Control;
            //    ((Control)sender).Controls[0].SetRenderMethodDelegate(new RenderMethod(this.NestedViewTable_Render));

            //}
            //catch (Exception ex)
            //{
            //    ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            //}
            ((Control)sender).Controls[0].SetRenderMethodDelegate(new RenderMethod(this.NestedViewTable_Render));

           
        }

        protected void NestedViewTable_Render(HtmlTextWriter writer, Control control)
        {
            //try
            //{
            //    //ScriptManager.RegisterStartupScript(this, typeof(string), "new window", "clearInterval(MyInterval);", true);

            //    //DataSet dsforNoofRows = SQLDataConnection.GetInstance().ExecuteDataSet(string.Format(Settings.Default.qryWeeklyDetailNoOfrows,lblResourceID.Text));

            //    //int iRowCount = dsforNoofRows.Tables[0].Rows.Count;

            //    int divheight;//= 110;
            //    //if(iRowCount>1)
            //    //   divheight  = iRowCount * 42; //dynamic hight of detail table div

            //    divheight = 160; //fixed height
            //    control.SetRenderMethodDelegate(null);

            //    iDivCount++;




            //    string genDIV = "<div id=\"div_dt" + iDivCount + "\" style=\"height:" + divheight.ToString() + "px; overflow: auto; overflow-x: hidden; overflow-y: scroll;\" >";

            //    writer.Write("<div id=\"div_dt" + iDivCount + "\" style=\" width: 100%; height:" + divheight.ToString() + "px; overflow: auto; overflow-x: hidden; overflow-y: scroll; border-style:solid;  border-color:#ff9900;  border-width:1px; border-style:solid;\" >");
            //    control.RenderControl(writer);
            //    writer.Write("</div>");


            //    string intervalScript = "var MyInterval= window.setInterval(\"divScroll(" + iDivCount + ")\", 1);";
            //    if (iDivCount == iTotalNoofDT)
            //        ScriptManager.RegisterStartupScript(this, typeof(string), "divScrollKey", "var MyInterval= window.setInterval(\"divScroll(" + iDivCount + ")\", 1);", true); //testing


            //    Session["NoOfDivs"] = iDivCount;

            //}
            //catch (Exception ex)
            //{
            //    ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            //}
            control.SetRenderMethodDelegate(null);

            writer.Write("<div style='overflow-x:scroll;'>");
            control.RenderControl(writer);
            writer.Write("</div>");

          
        }

        protected void Export_Click(object sender, EventArgs e)
        {
            RadGrid_weekly.MasterTableView.HierarchyDefaultExpanded = true;
            RadGrid_weekly.MasterTableView.Rebind();

            RadGrid_weekly.ExportSettings.ExportOnlyData = true;
            RadGrid_weekly.ExportSettings.OpenInNewWindow = true;
            RadGrid_weekly.ExportSettings.IgnorePaging = true;
            RadGrid_weekly.ExportSettings.FileName = "Project Allocations-" + (comDepartment.SelectedItem.ToString().Replace("'", "''") == "All" ? comResource.SelectedItem.ToString().Replace("'", "''") : comDepartment.SelectedItem.ToString().Replace("'", "''")) + "-" + ((DateTime)dpWeekStarting.SelectedDate).ToString("dd-MMM-yyyy");
            RadGrid_weekly.MasterTableView.ExportToExcel();

            Session["email"] = false;
        }

        protected void Email_Click(object sender, EventArgs e)
        {
            RadGrid_weekly.MasterTableView.HierarchyDefaultExpanded = true;
            RadGrid_weekly.MasterTableView.Rebind();
            RadGrid_weekly.ExportSettings.ExportOnlyData = true;
            RadGrid_weekly.ExportSettings.OpenInNewWindow = true;
            RadGrid_weekly.ExportSettings.IgnorePaging = true;
            RadGrid_weekly.ExportSettings.FileName = "Project Allocations-" + (comDepartment.SelectedItem.ToString().Replace("'", "''") == "All" ? comResource.SelectedItem.ToString().Replace("'", "''") : comDepartment.SelectedItem.ToString().Replace("'", "''")) + "-" + ((DateTime)dpWeekStarting.SelectedDate).ToString("dd-MMM-yyyy");
            RadGrid_weekly.MasterTableView.ExportToExcel();

            Session["email"] = true;

        }

        protected void RadGrid_weekly_Export(object source, GridExportingArgs e)
        {
            if ((bool)Session["email"])
            {
                if (e.ExportType == ExportType.Excel)
                {

                    MemoryStream ms = new MemoryStream(new ASCIIEncoding().GetBytes(e.ExportOutput));
                    SendMail("smtp.gmail.com", "metis.streebo@streebo.com", "", "salman.kasbati@streebo.com", "PFA", "Project Allocations-" + (comDepartment.SelectedItem.ToString().Replace("'", "''") == "All" ? comResource.SelectedItem.ToString().Replace("'", "''") : comDepartment.SelectedItem.ToString().Replace("'", "''")) + "-" + ((DateTime)dpWeekStarting.SelectedDate).ToString("dd-MMM-yyyy"), false, ms, (DataView)Session["dview"], "Project Allocations-" + (comDepartment.SelectedItem.ToString().Replace("'", "''") == "All" ? comResource.SelectedItem.ToString().Replace("'", "''") : comDepartment.SelectedItem.ToString().Replace("'", "''")) + "-" + ((DateTime)dpWeekStarting.SelectedDate).ToString("dd-MMM-yyyy") + ".xls");
                }
            }

        }

        public void SendMail(string smtpAddress, string from, string to, string cc, string body, string subject, bool isHtml, Stream attachmentStream, DataView dv, string fileName)
        {

            MailMessage email = new MailMessage();
            email.From = new MailAddress("metis.streebo@gmail.com", "Streebo Metis");
            email.ReplyTo = new MailAddress(cc);

            Dictionary<string, string> lstResourceWithEmail = new Dictionary<string, string>();

            objBLL = new MetisBLL();
            DataTable dtable = objBLL.getAllResourcesWithEmail();

            foreach (DataRow row in dtable.Rows)
            {
                lstResourceWithEmail.Add(row[1].ToString() + "@Streebo.com", row[0].ToString());
            }

            foreach (DataRowView drv in dv)
            {
                if (FindKeyByValue(lstResourceWithEmail, drv[1].ToString()) != "")
                    email.To.Add(FindKeyByValue(lstResourceWithEmail, drv[1].ToString()));
            }

            email.CC.Add(cc);
            email.Body = body;
            email.Subject = subject;
            email.IsBodyHtml = isHtml;
            email.Attachments.Add(new Attachment(attachmentStream, fileName));

            SmtpClient smtp = new SmtpClient("localhost", 587);
            smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address  
            smtp.Credentials = new System.Net.NetworkCredential
                 ("metis.streebo@gmail.com", "Inbox@1234");
            //Or your Smtp Email ID and Password  
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;
            smtp.Send(email);
        }
        
        public string FindKeyByValue<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TValue value)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
                if (value.Equals(pair.Value)) return pair.Key.ToString();

            return "";

        }


        protected void RadGrid_weekly_RowClick(object sender, EventArgs e)
        {

        }
        #endregion

        # region Filter Related Methods/Events
       
        //protected void ddlResource_OnSelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Session["expandRecordList"] = null;
        //    objBLL = new MetisBLL();
        //    DataTable dtable = new DataTable();

        //    if (ddlResourceView.SelectedValue == "1")
        //    {
        //        dtable = objBLL.getResourceSummary((DateTime)dpWeekStarting.SelectedDate, (DateTime)dpEnding.SelectedDate, Session["user"].ToString());
        //        DataView dv = new DataView(dtable);
        //        dv.RowFilter = "Dept_name like '%" + (ddlDepartment.SelectedItem.ToString().Replace("'", "''") == "All" ? "" : ddlDepartment.SelectedItem.ToString().Replace("'", "''")) + "%'";
        //        dv.RowFilter += "AND Resource_name LIKE '%" + (ddlResource.SelectedItem.ToString().Replace("'", "''") == "All" ? "" : ddlResource.SelectedItem.ToString().Replace("'", "''")) + "%'";
        //        RadGrid_weekly.DataSource = dv;
        //        RadGrid_weekly.Rebind();

        //    }
        //    else
        //    {
        //        dtable = objBLL.getResourceAvailSummary((DateTime)dpWeekStarting.SelectedDate, (DateTime)dpEnding.SelectedDate, Session["user"].ToString());
        //        DataView dv = new DataView(dtable);
        //        dv.RowFilter = "Dept_name like '%" + (ddlDepartment.SelectedItem.ToString().Replace("'", "''") == "All" ? "" : ddlDepartment.SelectedItem.ToString().Replace("'", "''")) + "%'";
        //        dv.RowFilter += "AND Resource_name LIKE '%" + (ddlResource.SelectedItem.ToString().Replace("'", "''") == "All" ? "" : ddlResource.SelectedItem.ToString().Replace("'", "''")) + "%'";

        //        if (chkavailres.Checked)
        //        {
        //            WeekHeaderCount = 0;
        //            while (WeekHeaderCount < NoOfWeeks)
        //            {
        //                string headerName = Weekending_array[WeekHeaderCount];
        //                dv.RowFilter += "AND [" + headerName + "] > 0";
        //                WeekHeaderCount++;
        //            }
        //        }

        //        RadGrid_weekly.DataSource = dv;
        //        RadGrid_weekly.Rebind();
        //    }

        //}

        //protected void ddlDepartment_OnSelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Session["expandRecordList"] = null;
        //    objBLL = new MetisBLL();
        //    DataTable dtable = new DataTable();

        //    if (ddlResourceView.SelectedValue == "1")
        //    {
        //        dtable = objBLL.getResourceSummary((DateTime)dpWeekStarting.SelectedDate, (DateTime)dpEnding.SelectedDate, Session["user"].ToString());
        //        DataView dv = new DataView(dtable);
        //        dv.RowFilter = "Dept_name like '%" + (ddlDepartment.SelectedItem.ToString().Replace("'", "''") == "All" ? "" : ddlDepartment.SelectedItem.ToString().Replace("'", "''")) + "%'";
        //        dv.RowFilter += "AND Resource_name LIKE '%" + (ddlResource.SelectedItem.ToString().Replace("'", "''") == "All" ? "" : ddlResource.SelectedItem.ToString().Replace("'", "''")) + "%'";
        //        RadGrid_weekly.DataSource = dv;
        //        RadGrid_weekly.Rebind();

        //    }
        //    else
        //    {
        //        dtable = objBLL.getResourceAvailSummary((DateTime)dpWeekStarting.SelectedDate, (DateTime)dpEnding.SelectedDate, Session["user"].ToString());
        //        DataView dv = new DataView(dtable);
        //        dv.RowFilter = "Dept_name like '%" + (ddlDepartment.SelectedItem.ToString().Replace("'", "''") == "All" ? "" : ddlDepartment.SelectedItem.ToString().Replace("'", "''")) + "%'";
        //        dv.RowFilter += "AND Resource_name LIKE '%" + (ddlResource.SelectedItem.ToString().Replace("'", "''") == "All" ? "" : ddlResource.SelectedItem.ToString().Replace("'", "''")) + "%'";

        //        if (chkavailres.Checked)
        //        {
        //            WeekHeaderCount = 0;
        //            while (WeekHeaderCount < NoOfWeeks)
        //            {
        //                string headerName = Weekending_array[WeekHeaderCount];
        //                dv.RowFilter += "AND [" + headerName + "] > 0";
        //                WeekHeaderCount++;
        //            }
        //        }

        //        RadGrid_weekly.DataSource = dv;
        //        RadGrid_weekly.Rebind();
        //    }

        //}

        protected void ddlResourceView_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Session["expandRecordList"] = null;
            objBLL = new MetisBLL();
            DataTable dtable = new DataTable();

            if (ddlResourceView.SelectedValue == "1")
            {
                dtable = objBLL.getResourceSummary((DateTime)dpWeekStarting.SelectedDate, (DateTime)dpEnding.SelectedDate, Session["user"].ToString());
                DataView dv = new DataView(dtable);
                dv.RowFilter = "Dept_name like '%" + (comDepartment.Text.ToString().Replace("'", "''") == "All" ? "" : comDepartment.Text.ToString().Replace("'", "''")) + "%'";
                dv.RowFilter += "AND Resource_name LIKE '%" + (comResource.Text.ToString().Replace("'", "''") == "All" ? "" : comResource.Text.ToString().Replace("'", "''")) + "%'";
                RadGrid_weekly.DataSource = dv;
                RadGrid_weekly.Rebind();
                Label7.Visible = true;
                Label8.Visible = true;
                Label9.Visible = true;
                Label10.Visible = true;

            }
            else
            {
                dtable = objBLL.getResourceAvailSummary((DateTime)dpWeekStarting.SelectedDate, (DateTime)dpEnding.SelectedDate, Session["user"].ToString());
                DataView dv = new DataView(dtable);
                dv.RowFilter = "Dept_name like '%" + (comDepartment.Text.ToString().Replace("'", "''") == "All" ? "" : comDepartment.Text.ToString().Replace("'", "''")) + "%'";
                dv.RowFilter += "AND Resource_name LIKE '%" + (comResource.Text.ToString().Replace("'", "''") == "All" ? "" : comResource.Text.ToString().Replace("'", "''")) + "%'";
                Label7.Visible = false;
                Label8.Visible = false;
                Label9.Visible = false;
                Label10.Visible = false;
                if (chkavailres.Checked)
                {
                    WeekHeaderCount = 0;
                    while (WeekHeaderCount < NoOfWeeks)
                    {
                        string headerName = Weekending_array[WeekHeaderCount];
                        dv.RowFilter += "AND [" + headerName + "] > 0";
                        WeekHeaderCount++;
                    }
                }

                RadGrid_weekly.DataSource = dv;
                RadGrid_weekly.Rebind();
            }

        }

        protected void OnSelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            try
            {
                Session["expandRecordList"] = null;
                RadToolTipManager1.TargetControls.Clear();
                objBLL = new MetisBLL();
                DataTable dtable = new DataTable();

                if (ddlResourceView.SelectedValue == "1")
                {
                    dtable = objBLL.getResourceSummary((DateTime)dpWeekStarting.SelectedDate, (DateTime)dpEnding.SelectedDate, Session["user"].ToString());
                    DataView dv = new DataView(dtable);
                    dv.RowFilter = "Dept_name like '%" + (comDepartment.Text.ToString().Replace("'", "''") == "All" ? "" : comDepartment.Text.ToString().Replace("'", "''")) + "%'";
                    dv.RowFilter += "AND Resource_name LIKE '%" + (comResource.Text.ToString().Replace("'", "''") == "All" ? "" : comResource.Text.ToString().Replace("'", "''")) + "%'";
                    // Sarfaraz 14 Dec18:
                    // Issue after selection of resource if date changes, it reset to all resources.
                    // fix is datagrid needs to be binded with dv;
                    RadGrid_weekly.DataSource = dv;// objBLL.getResourceSummary((DateTime)dpWeekStarting.SelectedDate, (DateTime)dpEnding.SelectedDate, Session["user"].ToString()); ;
                    RadGrid_weekly.Rebind();
                    showUtilizationPercentage(dv.ToTable());
                    /*
                    decimal t = 0;
                    decimal z = 0;
                    String test;
                


                    for (int i = 0; i < dtable.Rows.Count; i++)
                    {
                        for (int j = 3; j < dtable.Columns.Count; j++)
                        {
                            test = dtable.Rows[i][j].ToString();
                            String[] t1 = test.Split('(');
                            t += Convert.ToDecimal(t1[0]);

                        }
                    }
                    // Change By: Sarfaraz Dated: 12 Dec18
                    // added condition to handle zero rows or zero columns to avoid divide by zero error

                    decimal talha = dtable.Rows.Count > 0 && dtable.Columns.Count > 0 ? Math.Round((t / ((dtable.Columns.Count - 3) * dtable.Rows.Count * 40)) * 100, 3): 0;
                    Label7.Text = Convert.ToString(talha) + " %";

                    int totalVisibleColumns = dtable.Columns.Count;

                    for (int i = 0; i < dtable.Rows.Count; i++)
                    {
                        for (int j = 4; j <= dtable.Columns.Count - 5; j++)
                        {
                            test = dtable.Rows[i][j].ToString();

                            String[] t1 = test.Split('(');

                            z += Decimal.Parse(t1[0]);

                        }
                    }
                    // Change By: Sarfaraz Dated: 12 Dec18
                    // added condition to handle zero rows or zero columns to avoid divide by zero error

                    decimal talhaz = dtable.Rows.Count > 0 && dtable.Columns.Count > 0 ? (decimal)Math.Round((z / ((dtable.Columns.Count - 8) * dtable.Rows.Count * 40)) * 100, 3): 0;
                    Label9.Text = Convert.ToString(talhaz) + " %";
                    */
                   
                }
                else
                {
                    dtable = objBLL.getResourceAvailSummary((DateTime)dpWeekStarting.SelectedDate, (DateTime)dpEnding.SelectedDate, Session["user"].ToString());
                    DataView dv = new DataView(dtable);
                    dv.RowFilter = "Dept_name like '%" + (comDepartment.Text.ToString().Replace("'", "''") == "All" ? "" : comDepartment.Text.ToString().Replace("'", "''")) + "%'";
                    dv.RowFilter += "AND Resource_name LIKE '%" + (comResource.Text.ToString().Replace("'", "''") == "All" ? "" : comResource.Text.ToString().Replace("'", "''")) + "%'";

                    if (chkavailres.Checked)
                    {
                        WeekHeaderCount = 0;
                        while (WeekHeaderCount < NoOfWeeks)
                        {
                            string headerName = Weekending_array[WeekHeaderCount];
                            dv.RowFilter += "AND [" + headerName + "] > 0";
                            WeekHeaderCount++;
                        }
                    }

                    RadGrid_weekly.DataSource = dv;
                    RadGrid_weekly.Rebind();
                    
                }



            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }

        }

        private void showUtilizationPercentage(DataTable dt)
        {
            decimal t = 0;
            decimal z = 0;
            String test;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 3; j < dt.Columns.Count; j++)
                {
                    test = dt.Rows[i][j].ToString();
                    String[] t1 = test.Split('(');
                    t += Convert.ToDecimal(t1[0]);
                }
            }

            // Change By: Sarfaraz Dated: 12 Dec18
            // added condition to handle zero rows or zero columns to avoid divide by zero error
            decimal talha = dt.Rows.Count > 0 && dt.Columns.Count > 0 ? Math.Round((t / ((dt.Columns.Count - 3) * dt.Rows.Count * 40)) * 100, 3) : 0;
            Label7.Text = Convert.ToString(talha) + " %";

            int totalVisibleColumns = dt.Columns.Count;
            int nextHowManyWeekVisible = totalVisibleColumns - 4;
            // 3 columns visible = next 0 weeks.
            // 4 columns visible = next 1 weeks.
            // 5 columns visible = next 2 weeks.
            // 6 columns visible = next 3 weeks.
            // 7 columns visible = next 4 weeks.
            // 8 columns visible = next 4 weeks only.
            int showWeekCount = nextHowManyWeekVisible > 4 ? 4 : nextHowManyWeekVisible < 0 ? 0 : nextHowManyWeekVisible;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 4; j <= 3 + showWeekCount; j++)
                {
                    test = dt.Rows[i][j].ToString();
                    String[] t1 = test.Split('(');
                    z += Decimal.Parse(t1[0]);
                }
            }

            // Change By: Sarfaraz Dated: 12 Dec18
            // added condition to handle zero rows or zero columns to avoid divide by zero error

            decimal talhaz = dt.Rows.Count > 0 && showWeekCount > 0 ? (decimal)Math.Round((z / ((showWeekCount) * dt.Rows.Count * 40)) * 100, 3) : 0;
            Label9.Text = Convert.ToString(talhaz) + " %";
            Label10.Text = PropertyLayer.UtlizePersonNext + " " + showWeekCount + " " + PropertyLayer.weeks;
            //Label10.Text = "Utilization Percentage (Next " + showWeekCount + " Weeks):";

        }
        protected void Check(object sender, EventArgs e)
        {
            Session["expandRecordList"] = null;
            if (!chkavailres.Checked)
            {
                objBLL = new MetisBLL();
                DataTable dtable = new DataTable();
                if (ddlResourceView.SelectedValue == "1")
                {
                    dtable = objBLL.getResourceSummary((DateTime)dpWeekStarting.SelectedDate, (DateTime)dpEnding.SelectedDate, Session["user"].ToString());
                    Label7.Visible = true;
                    Label8.Visible = true;
                    Label9.Visible = true;
                    Label10.Visible = true;
                }
                else
                {
                    dtable = objBLL.getResourceAvailSummary((DateTime)dpWeekStarting.SelectedDate, (DateTime)dpEnding.SelectedDate, Session["user"].ToString());
                    Label7.Visible = false;
                    Label8.Visible = false;
                    Label9.Visible = false;
                    Label10.Visible = false;
                }
                DataView dv = new DataView(dtable);
                dv.RowFilter = "Dept_name like '%" + (comDepartment.Text.ToString().Replace("'", "''") == "All" ? "" : comDepartment.Text.ToString().Replace("'", "''")) + "%'";
                dv.RowFilter += "AND Resource_name LIKE '%" + (comResource.Text.ToString().Replace("'", "''") == "All" ? "" : comResource.Text.ToString().Replace("'", "''")) + "%'";
                RadGrid_weekly.DataSource = dv;
                RadGrid_weekly.Rebind();

            }
            else
            {
                objBLL = new MetisBLL();
                DataTable dtable = new DataTable();
                if (ddlResourceView.SelectedValue == "1")
                {
                    dtable = objBLL.getResourceSummary((DateTime)dpWeekStarting.SelectedDate, (DateTime)dpEnding.SelectedDate, Session["user"].ToString());
                    Label7.Visible = true;
                    Label8.Visible = true;
                    Label9.Visible = true;
                    Label10.Visible = true;
                }
                else
                {
                    dtable = objBLL.getResourceAvailSummary((DateTime)dpWeekStarting.SelectedDate, (DateTime)dpEnding.SelectedDate, Session["user"].ToString());
                    Label7.Visible = false;
                    Label8.Visible = false;
                    Label9.Visible = false;
                    Label10.Visible = false;
                }
                DataView dv = new DataView(dtable);
                dv.RowFilter = "Dept_name like '%" + (comDepartment.Text.ToString().Replace("'", "''") == "All" ? "" : comDepartment.Text.ToString().Replace("'", "''")) + "%'";
                dv.RowFilter += "AND Resource_name LIKE '%" + (comResource.Text.ToString().Replace("'", "''") == "All" ? "" : comResource.Text.ToString().Replace("'", "''")) + "%'";

                WeekHeaderCount = 0;
                while (WeekHeaderCount < NoOfWeeks)
                {
                    string headerName = Weekending_array[WeekHeaderCount];
                    dv.RowFilter += "AND [" + headerName + "] > 0";
                    WeekHeaderCount++;
                }


                RadGrid_weekly.DataSource = dv;
                RadGrid_weekly.Rebind();


            }


        }

         //necessary to disable the weekend days on first page load
        protected void Calendar_OnDayRender(object sender, Telerik.Web.UI.Calendar.DayRenderEventArgs e)
        {
            try
            {
                // modify the cell rendered content for the days we want to be disabled (e.g. every Saturday and Sunday)
                if (e.Day.Date.DayOfWeek != DayOfWeek.Monday) //|| e.Day.Date.DayOfWeek == DayOfWeek.Sunday)
                {
                    // if you are using the skin bundled as a webresource("Default"), the Skin property returns empty string
                    string calendarSkin = dpWeekStarting.Calendar.Skin != "" ? dpWeekStarting.Calendar.Skin : "Default";
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
                    dpWeekStarting.Calendar.SpecialDays.Add(calendarDay);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }
        }

        protected void EndCalendar_OnDayRender(object sender, Telerik.Web.UI.Calendar.DayRenderEventArgs e)
        {
            try
            {
                // modify the cell rendered content for the days we want to be disabled (e.g. every Saturday and Sunday)
                if (e.Day.Date.DayOfWeek != DayOfWeek.Monday) //|| e.Day.Date.DayOfWeek == DayOfWeek.Sunday)
                {
                    // if you are using the skin bundled as a webresource("Default"), the Skin property returns empty string
                    string calendarSkin = dpEnding.Calendar.Skin != "" ? dpWeekStarting.Calendar.Skin : "Default";
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
                    dpEnding.Calendar.SpecialDays.Add(calendarDay);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }
        }

        protected void Refresh(object sender, EventArgs e)
        {

            objBLL = new MetisBLL();
            DataTable dtable = new DataTable();

            if (ddlResourceView.SelectedValue == "1")
            {
                dtable = objBLL.getResourceSummary((DateTime)dpWeekStarting.SelectedDate, (DateTime)dpEnding.SelectedDate, Session["user"].ToString());
                DataView dv = new DataView(dtable);
                dv.RowFilter = "Dept_name like '%" + (comDepartment.Text.ToString().Replace("'", "''") == "All" ? "" : comDepartment.Text.ToString().Replace("'", "''")) + "%'";
                dv.RowFilter += "AND Resource_name LIKE '%" + (comResource.Text.ToString().Replace("'", "''") == "All" ? "" : comResource.Text.ToString().Replace("'", "''")) + "%'";
                RadGrid_weekly.DataSource = dv;
                RadGrid_weekly.Rebind();

            }
            else
            {
                dtable = objBLL.getResourceAvailSummary((DateTime)dpWeekStarting.SelectedDate, (DateTime)dpEnding.SelectedDate, Session["user"].ToString());
                DataView dv = new DataView(dtable);
                dv.RowFilter = "Dept_name like '%" + (comDepartment.Text.ToString().Replace("'", "''") == "All" ? "" : comDepartment.Text.ToString().Replace("'", "''")) + "%'";
                dv.RowFilter += "AND Resource_name LIKE '%" + (comResource.Text.ToString().Replace("'", "''") == "All" ? "" : comResource.Text.ToString().Replace("'", "''")) + "%'";

                if (chkavailres.Checked)
                {
                    WeekHeaderCount = 0;
                    while (WeekHeaderCount < NoOfWeeks)
                    {
                        string headerName = Weekending_array[WeekHeaderCount];
                        dv.RowFilter += "AND [" + headerName + "] > 0";
                        WeekHeaderCount++;
                    }
                }

                RadGrid_weekly.DataSource = dv;
                RadGrid_weekly.Rebind();
            }

        }

        protected void comResource_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {

            Session["expandRecordList"] = null;
            objBLL = new MetisBLL();
            DataTable dtable = new DataTable();
           
            if (ddlResourceView.SelectedValue == "1")
            {
                if (comResource.Text == "All")
                {
                    dtable = objBLL.getResourceSummary((DateTime)dpWeekStarting.SelectedDate, (DateTime)dpEnding.SelectedDate, Session["user"].ToString());
                    DataView dv = new DataView(dtable);
                    dv.RowFilter = "Dept_name like '%" + (comDepartment.Text.ToString().Replace("'", "''") == "All" ? "" : comDepartment.Text.ToString().Replace("'", "''")) + "%'";

                    dv.RowFilter += "AND Resource_name LIKE '%" + (comResource.Text.ToString().Replace("'", "''") == "All" ? "" : comResource.Text.ToString().Replace("'", "''")) + "%'";
                    RadGrid_weekly.DataSource = objBLL.getResourceSummary((DateTime)dpWeekStarting.SelectedDate, (DateTime)dpEnding.SelectedDate, Session["user"].ToString());

                    RadGrid_weekly.Rebind();

                    showUtilizationPercentage(dtable);
                    /*
                    decimal t = 0;
                    decimal z = 0;
                    String test;
                    


                    for (int i = 0; i < dtable.Rows.Count; i++)
                    {
                        for (int j = 3; j < dtable.Columns.Count; j++)
                        {
                            test = dtable.Rows[i][j].ToString();
                            String[] t1 = test.Split('(');
                            t += Convert.ToDecimal(t1[0]);

                        }
                    }
                    // Change By: Sarfaraz Dated: 12 Dec18
                    // added condition to handle zero rows or zero columns to avoid divide by zero error
                    decimal talha = dtable.Rows.Count > 0 && dtable.Columns.Count > 0 ? Math.Round((t / ((dtable.Columns.Count - 3) * dtable.Rows.Count * 40)) * 100, 3) : 0;
                    Label7.Text = Convert.ToString(talha) + " %";

                    for (int i = 0; i < dtable.Rows.Count; i++)
                    {
                        for (int j = 4; j <= dtable.Columns.Count - 5; j++)
                        {
                            test = dtable.Rows[i][j].ToString();

                            String[] t1 = test.Split('(');

                            z += Decimal.Parse(t1[0]);

                        }
                    }
                    // Change By: Sarfaraz Dated: 12 Dec18
                    // added condition to handle zero rows or zero columns to avoid divide by zero error

                    decimal talhaz = dtable.Rows.Count > 0 && dtable.Columns.Count > 0 ?  (decimal)Math.Round((z / ((dtable.Columns.Count - 8) * dtable.Rows.Count * 40)) * 100, 3) : 0 ;
                    Label9.Text = Convert.ToString(talhaz) + " %";
                    */

                }
                else
                {
                    dtable = objBLL.getResourceSummary((DateTime)dpWeekStarting.SelectedDate, (DateTime)dpEnding.SelectedDate, Session["user"].ToString());
                    DataView dv = new DataView(dtable);
                    dv.RowFilter = "Dept_name like '%" + (comDepartment.Text.ToString().Replace("'", "''") == "All" ? "" : comDepartment.Text.ToString().Replace("'", "''")) + "%'";

                    // dv.RowFilter +=  "AND Resource_name LIKE '%" + (comResource.Text.ToString().Replace("'", "''") == "All" ? "" : comResource.Text.ToString().Replace("'", "''")) + "%'";

                    objBLL = new MetisBLL();
                    DataTable d1table = new DataTable();
                    d1table = objBLL.getEmailfromResID(comResource.SelectedValue.ToString());
                    string email = d1table.Rows[0]["email"].ToString();

                    RadGrid_weekly.DataSource = objBLL.getResourceSummary((DateTime)dpWeekStarting.SelectedDate, (DateTime)dpEnding.SelectedDate, email);
                    RadGrid_weekly.Rebind();
                    DataTable dtt = new DataTable();
                    dtt = objBLL.getResourceSummary((DateTime)dpWeekStarting.SelectedDate, (DateTime)dpEnding.SelectedDate, email);
                    showUtilizationPercentage(dtt);
                    /*
                        decimal t = 0;
                        decimal z = 0;
                        String test;
                       


                        for (int i = 0; i < dtt.Rows.Count; i++)
                        {
                            for (int j = 3; j < dtt.Columns.Count; j++)
                            {
                                test = dtt.Rows[i][j].ToString();
                                String[] t1 = test.Split('(');
                                t += Convert.ToDecimal(t1[0]);

                            }
                        }
                    // Change By: Sarfaraz Dated: 12 Dec18
                    // added condition to handle zero rows or zero columns to avoid divide by zero error
                    
                    decimal talha = dtt.Rows.Count > 0 && dtt.Columns.Count > 0  ? Math.Round((t / ((dtt.Columns.Count - 3) * dtt.Rows.Count * 40)) * 100, 3):0;
                        Label7.Text = Convert.ToString(talha) + " %";

                        for (int i = 0; i < dtt.Rows.Count; i++)
                        {
                            for (int j = 4; j <= dtt.Columns.Count - 5; j++)
                            {
                                test = dtt.Rows[i][j].ToString();

                                String[] t1 = test.Split('(');

                                z += Decimal.Parse(t1[0]);

                            }
                        }
                        // Change By: Sarfaraz Dated: 12 Dec18
                        // added condition to handle zero rows or zero columns to avoid divide by zero error

                    decimal talhaz = dtt.Rows.Count > 0 && dtt.Columns.Count > 0  ?  (decimal)Math.Round((z / ((dtt.Columns.Count - 8) * dtt.Rows.Count * 40)) * 100, 3): 0 ;
                        Label9.Text = Convert.ToString(talhaz) + " %";
                     */
                    }
             

            }
            else
            {
                dtable = objBLL.getResourceAvailSummary((DateTime)dpWeekStarting.SelectedDate, (DateTime)dpEnding.SelectedDate, Session["user"].ToString());
                DataView dv = new DataView(dtable);
                dv.RowFilter = "Dept_name like '%" + (comDepartment.Text.ToString().Replace("'", "''") == "All" ? "" : comDepartment.Text.ToString().Replace("'", "''")) + "%'";
                dv.RowFilter += "AND Resource_name LIKE '%" + (comResource.Text.ToString().Replace("'", "''") == "All" ? "" : comResource.Text.ToString().Replace("'", "''")) + "%'";

                if (chkavailres.Checked)
                {
                    WeekHeaderCount = 0;
                    while (WeekHeaderCount < NoOfWeeks)
                    {
                        string headerName = Weekending_array[WeekHeaderCount];
                        dv.RowFilter += "AND [" + headerName + "] > 0";
                        WeekHeaderCount++;
                    }
                }

                RadGrid_weekly.DataSource = dv;
                RadGrid_weekly.Rebind();
            }
        }

        protected void comDepartment_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            //string test;
            //test = comDepartment.Text;

            Session["expandRecordList"] = null;
            objBLL = new MetisBLL();
            DataTable dtable = new DataTable();

            if (ddlResourceView.SelectedValue == "1")
            {
                if (comDepartment.Text == "All")
                {
                    DataTable dt = objBLL.getResourceSummary((DateTime)dpWeekStarting.SelectedDate, (DateTime)dpEnding.SelectedDate, Session["user"].ToString());
                    RadGrid_weekly.DataSource = dt;
                    RadGrid_weekly.Rebind();
                    showUtilizationPercentage(dt);
                    /*
                     decimal t = 0;
                    decimal z = 0;
                    String test;
                  


                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 3; j < dt.Columns.Count; j++)
                        {
                            test = dt.Rows[i][j].ToString();
                            String[] t1 = test.Split('(');
                            t += Convert.ToDecimal(t1[0]);

                        }
                    }
                    // Change By: Sarfaraz Dated: 12 Dec18
                    // added condition to handle zero rows or zero columns to avoid divide by zero error
                    decimal talha = dt.Rows.Count > 0 && dt.Columns.Count > 0  ?  Math.Round((t / ((dt.Columns.Count - 3) * dt.Rows.Count * 40)) * 100, 3) : 0;
                    Label7.Text = Convert.ToString(talha) + " %";

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 4; j <= dt.Columns.Count - 5; j++)
                        {
                            test = dt.Rows[i][j].ToString();

                            String[] t1 = test.Split('(');

                            z += Decimal.Parse(t1[0]);

                        }
                    }
                    // Change By: Sarfaraz Dated: 12 Dec18
                    // added condition to handle zero rows or zero columns to avoid divide by zero error
                    decimal talhaz =  dt.Rows.Count > 0 && dt.Columns.Count > 0  ?  (decimal)Math.Round((z / ((dt.Columns.Count - 8) * dt.Rows.Count * 40)) * 100, 3): 0;
                    Label9.Text = Convert.ToString(talhaz) + " %";
                    */
                }
                else
                {
                    dtable = objBLL.getResourceSummary((DateTime)dpWeekStarting.SelectedDate, (DateTime)dpEnding.SelectedDate, Session["user"].ToString());
                    DataView dv = new DataView(dtable);
                    dv.RowFilter = "Dept_name like '%" + (comDepartment.Text.Replace("'", "''") == "All" ? "" : comDepartment.Text.Replace("'", "''")) + "%'";
                    dv.RowFilter += "AND Resource_name LIKE '%" + (comResource.Text.Replace("'", "''") == "All" ? "" : comResource.Text.Replace("'", "''")) + "%'";
                    RadGrid_weekly.DataSource = dv;
                    DataTable dtt = new DataTable();
                    dtt = dv.ToTable();

                    RadGrid_weekly.Rebind();

                    showUtilizationPercentage(dtt);
                    /*
                     decimal t = 0;
                    decimal z = 0;
                    String test;
                  


                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        for (int j = 3; j < dtt.Columns.Count; j++)
                        {
                            test = dtt.Rows[i][j].ToString();
                            String[] t1 = test.Split('(');
                            t += Convert.ToDecimal(t1[0]);

                        }
                    }
                    // Change By: Sarfaraz Dated: 12 Dec18
                    // added condition to handle zero rows or zero columns to avoid divide by zero error
                    decimal talha =  dtt.Rows.Count > 0 && dtt.Columns.Count > 0  ? Math.Round((t / ((dtt.Columns.Count - 3) * dtt.Rows.Count * 40)) * 100, 3) : 0;
                    Label7.Text = Convert.ToString(talha) + " %";

                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        for (int j = 4; j <= dtt.Columns.Count - 5; j++)
                        {
                            test = dtt.Rows[i][j].ToString();

                            String[] t1 = test.Split('(');

                            z += Decimal.Parse(t1[0]);

                        }
                    }
                    // Change By: Sarfaraz Dated: 12 Dec18
                    // added condition to handle zero rows or zero columns to avoid divide by zero error
                    decimal talhaz =  dtt.Rows.Count > 0 && dtt.Columns.Count > 0  ?  (decimal)Math.Round((z / ((dtt.Columns.Count - 8) * dtt.Rows.Count * 40)) * 100, 3): 0;
                    Label9.Text = Convert.ToString(talhaz) + " %";
                     */
                }

            }
            else
            {
                dtable = objBLL.getResourceAvailSummary((DateTime)dpWeekStarting.SelectedDate, (DateTime)dpEnding.SelectedDate, Session["user"].ToString());
                DataView dv = new DataView(dtable);
                dv.RowFilter = "Dept_name like '%" + (comDepartment.Text.Replace("'", "''") == "All" ? "" : comDepartment.Text.Replace("'", "''")) + "%'";
                dv.RowFilter += "AND Resource_name LIKE '%" + (comResource.Text.Replace("'", "''") == "All" ? "" : comResource.Text.Replace("'", "''")) + "%'";

                if (chkavailres.Checked)
                {
                    WeekHeaderCount = 0;
                    while (WeekHeaderCount < NoOfWeeks)
                    {
                        string headerName = Weekending_array[WeekHeaderCount];
                        dv.RowFilter += "AND [" + headerName + "] > 0";
                        WeekHeaderCount++;
                    }
                }

                RadGrid_weekly.DataSource = dv;
                RadGrid_weekly.Rebind();
            }
            Session["ddlDepartment"] = comDepartment.SelectedValue;
        }



        #endregion

        #region ToBeChecked

        protected void cmbDepartment_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {

                //lblcmbDepartment_selectedValue.Text = e.Value.ToString();
                //lblcmbDepartment_selectedText.Text = e.Text.ToString();
                //all = "";
                //notAssigned = "--";

                //if (e.Text == "All")
                //{ all = "--"; notAssigned = "--"; }

                //if (e.Text == "Not Assigned")
                //{
                //    all = ""; notAssigned = "";
                //}
                //string q = string.Format(Settings.Default.qrytest, dpWeekEnding.SelectedDate, all, e.Value, notAssigned);
                //rg1DataSet = SQLDataConnection.GetInstance().ExecuteDataSet(q);
                //RadToolTipManager1.TargetControls.Clear();
                //RadGrid1.DataSource = rg1DataSet;
                //RadGrid1.Rebind();
                //int c = RadToolTipManager1.TargetControls.Count;


                //RadGrid1.Rebind();
                //RadGrid_weekly.Rebind();

                //if (cmbReportType.SelectedValue == "2")
                //    RadGrid1.Focus();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }

        }

        protected void cmbReportType_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {

            try
            {
                RadGrid1.Controls.Clear();
                RadGrid_weekly.Controls.Clear();

                if (e.Value.ToString() == "1")
                {

                    RadGrid1.Visible = false;
                    RadGrid_weekly.Visible = true;
                    RadGrid_weekly.Rebind();
                    RadGrid_weekly.Focus();


                }
                if (e.Value.ToString() == "2")
                {

                    RadGrid1.Visible = true;
                    RadGrid_weekly.Visible = false;
                    RadGrid1.Rebind();
                    RadGrid1.Focus();

                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }


        }

        protected void rblAssignmentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListItem selectedItem = rblAssignmentType.SelectedItem;
                switch (selectedItem.Value)
                {
                    case "Bulk_Assignment":
                        RadGrid2.Visible = false;
                        RadGrid_bulk.Visible = true;
                        break;

                    case "Normal_Assignment":
                        RadGrid2.Visible = true;
                        RadGrid_bulk.Visible = false;
                        break;

                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }
        }

        //protected void Resource_id_SelectedIndexChanged(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        //first reference the edited grid item through the NamingContainer attribute
        //        GridEditableItem editedItem = (sender as RadComboBox).NamingContainer as GridEditableItem;
        //        //the dropdown list will be the first control in the Controls collection of the corresponding cell
        //        //for custom edit forms (WebUserControl/FormTemplate) you can find the column editor with the FindControl(controlId) method
        //        RadComboBox ddList = editedItem["Project_id"].Controls[0] as RadComboBox;

        //        // change the data source for ContactTitle with custom code here
        //        ddList.Width = 200;
        //        // string sResourceProject = string.Format(Settings.Default.qryResourceProject, (editedItem["Resource_id"].Controls[0] as RadComboBox).SelectedItem.Value);
        //        // DataSet dt = SQLDataConnection.GetInstance().ExecuteDataSet(sResourceProject);


        //        ddList.DataTextField = "Project_name";
        //        ddList.DataValueField = "Project_id";
        //        // ddList.DataSource = dt.Tables[0];
        //        ddList.DataBind();

        //        // RadGrid_bulk.Controls.Add(new LiteralControl("<b>the available options for Contact Title has been changed</b>"));

        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

        //    }
        //}

        protected void lbWeek_ending_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (tbEnterBool.Text == "true")
                {
                    //dailyQry(lbRowID.Text, dpWeekEnding.SelectedDate.ToString(), "monday,w.tuesday,w.wednesday,w.thursday,w.friday,w.saturday,w.sunday ", 0);
                    //  radwindowPopup.VisibleOnPageLoad = true;
                    tbEnterBool.Text = "false";

                }
                else
                {
                    LinkButton link = (LinkButton)sender;
                    GridDataItem gdt = (GridDataItem)(link.Parent.Parent);


                    //  dailyQry(gdt.Cells[4].Text, gdt.Cells[5].Text, "monday,w.tuesday,w.wednesday,w.thursday,w.friday,w.saturday,w.sunday ", 0);

                    radwindowPopup.Title = gdt.Cells[2].Text + "'s Daily Project Wise Report";
                    radwindowPopup.VisibleOnPageLoad = true;
                    // refresh_grid();
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }
        }

        protected void ExpandAll_weeklyDetails()
        {
            try
            {
                //string sQryWeekly = qryWeeklyDetail + string.Format(Settings.Default.qryDetailWeeklyDynamic_part2.ToString(), "", "--");
                // DataSet DTWeeklyDetailTable = SQLDataConnection.GetInstance().ExecuteDataSet(sQryWeekly);
                //RadGrid_weekly.MasterTableView.DetailTables[0].DataSource = DTWeeklyDetailTable.Tables[0];

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }

        }

        protected void btnExpandAll_Click(object sender, EventArgs e)
        {
            ExpandAll_weeklyDetails();
            ExpandAllClick(sender, e);

        }

        protected void btnCollapseAll_Click(object sender, EventArgs e)
        {
            //  ExpandAll_weeklyDetails();
            CollapseAllClick(sender, e);

        }

        protected void CollapseAllClick(object sender, EventArgs e)
        {
            try
            {
                RadGrid_weekly.MasterTableView.HierarchyDefaultExpanded = false;
                RadGrid1.MasterTableView.HierarchyDefaultExpanded = false;
                RadGrid_weekly.Rebind();
                RadGrid1.Rebind();
                // If you not used Advance data binding then again Bind // RadGrid1.DataSource = ""; RadGrid1.DataBind();


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }
        }

        protected void ExpandAllClick(object sender, EventArgs e)
        {
            try
            {
                RadGrid_weekly.MasterTableView.HierarchyDefaultExpanded = true;
                RadGrid1.MasterTableView.HierarchyDefaultExpanded = true;
                RadGrid_weekly.Rebind();
                RadGrid1.Rebind();
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
                Session["user"] = String.Empty;
                Response.Redirect("Login.aspx");

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {

            RadGrid1.Rebind();
        }

        protected void btnClose_OnClick(object sender, EventArgs e)
        {
            radwindowPopup.VisibleOnPageLoad = false;
            RadGrid1.Rebind();

        }

        protected void lbMonday_OnClick(object sender, EventArgs e)
        {

            LinkButton link = (LinkButton)sender;
            GridDataItem gdt = (GridDataItem)(link.Parent.Parent);
            //dailyQry(gdt.Cells[4].Text, gdt.Cells[5].Text, "monday", 5);
            radwindowPopup.Title = gdt.Cells[2].Text + "'s Daily Project Wise Report";
        }

        protected void lbTuesday_Click(object sender, EventArgs e)
        {
            LinkButton link = (LinkButton)sender;
            GridDataItem gdt = (GridDataItem)(link.Parent.Parent);
            // dailyQry(gdt.Cells[4].Text, gdt.Cells[5].Text, "tuesday", 6);

            radwindowPopup.Title = gdt.Cells[2].Text + "'s Daily Project Wise Report";
            radwindowPopup.VisibleOnPageLoad = true;

        }

        protected void lbWednesday_OnClick(object sender, EventArgs e)
        {
            LinkButton link = (LinkButton)sender;
            GridDataItem gdt = (GridDataItem)(link.Parent.Parent);
            //dailyQry(gdt.Cells[4].Text, gdt.Cells[5].Text, "wednesday", 7);
            radwindowPopup.Title = gdt.Cells[2].Text + "'s Daily Project Wise Report";
        }

        protected void lbThursday_OnClick(object sender, EventArgs e)
        {
            LinkButton link = (LinkButton)sender;
            GridDataItem gdt = (GridDataItem)(link.Parent.Parent);
            //dailyQry(gdt.Cells[4].Text, gdt.Cells[5].Text, "thursday", 8);
            radwindowPopup.Title = gdt.Cells[2].Text + "'s Daily Project Wise Report";
            radwindowPopup.VisibleOnPageLoad = true;
        }

        protected void lbFriday_OnClick(object sender, EventArgs e)
        {
            LinkButton link = (LinkButton)sender;
            GridDataItem gdt = (GridDataItem)(link.Parent.Parent);
            //dailyQry(gdt.Cells[4].Text, gdt.Cells[5].Text, "friday", 9);
            radwindowPopup.Title = gdt.Cells[2].Text + "'s Daily Project Wise Report";

        }

        protected void lbSaturday_OnClick(object sender, EventArgs e)
        {
            LinkButton link = (LinkButton)sender;
            GridDataItem gdt = (GridDataItem)(link.Parent.Parent);

            radwindowPopup.Title = gdt.Cells[2].Text + "'s Daily Project Wise Report";
            //dailyQry(gdt.Cells[4].Text, gdt.Cells[5].Text, "saturday", 10);
            radwindowPopup.VisibleOnPageLoad = true;

        }

        protected void lbSunday_OnClick(object sender, EventArgs e)
        {
            LinkButton link = (LinkButton)sender;
            GridDataItem gdt = (GridDataItem)(link.Parent.Parent);
            radwindowPopup.Title = gdt.Cells[2].Text + "'s Daily Project Wise Report";
            //dailyQry(gdt.Cells[4].Text, gdt.Cells[5].Text, "sunday", 11);

        }

        //protected void OnAjaxUpdate(object sender, ToolTipUpdateEventArgs args)
        //{
        //    try
        //    {
        //        RadGrid1.Rebind();
        //        RadGrid_weekly.Rebind();
        //        this.UpdateToolTip(args.Value, args.UpdatePanel);

        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

        //    }

        //}

        protected void refresh_grid()
        {
            try
            {


                //string s = string.Format(Settings.Default.qrytest, dpWeekEnding.SelectedDate, all, lblcmbDepartment_selectedValue.Text, notAssigned);
                //rg1DataSet = SQLDataConnection.GetInstance().ExecuteDataSet(s);
                //rg1DataSet = SQLDataConnection.GetInstance().ExecuteDataSet(string.Format(Settings.Default.qryReport, dpWeekEnding.SelectedDate));
                //RadToolTipManager1.TargetControls.Clear(); 
                //RadGrid1.DataSource = rg1DataSet;
                //RadGrid1.DataBind();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }

        }

        //protected void dailyQry(string ResourceID, string WeekEnding, string day, int i)
        //{
        //    try
        //    {
        //        lblResourceID.Text = ResourceID; lblWeekending.Text = WeekEnding; lblDay.Text = day;
        //        sQry = "";
        //        sQry_bulk = "";

        //        string cols = "Bulk_Ass,Work_load,start_bulk,end_bulk ";

        //        if (i == 0)
        //            sQry_bulk = string.Format(Settings.Default.qryBulk, cols, ResourceID);
        //        else if (i != 0)
        //            sQry_bulk = string.Format(Settings.Default.qryBulk, cols, ResourceID);

        //       DataSet dt_bulk = SQLDataConnection.GetInstance().ExecuteDataSet(sQry_bulk);
        //        RadGrid_bulk.DataSource = dt_bulk;
        //        RadGrid_bulk.DataBind();


        //        if (i == 0)
        //            sQry = string.Format(Settings.Default.qrySelectDetailedReport, day, ResourceID, WeekEnding, "Week_endings");
        //        else if (i != 0)
        //            sQry = string.Format(Settings.Default.qrySelectDetailedReport, day, ResourceID, WeekEnding, day);

        //        DataSet dt = SQLDataConnection.GetInstance().ExecuteDataSet(sQry);
        //        RadGrid2.DataSource = dt;
        //        RadGrid2.DataBind();
        //        if (i != 0)
        //        {
        //            RadGrid2.Columns[11].Visible = false;
        //            RadGrid2.Columns[5].Visible = false;
        //            RadGrid2.Columns[6].Visible = false;
        //            RadGrid2.Columns[7].Visible = false;
        //            RadGrid2.Columns[8].Visible = false;
        //            RadGrid2.Columns[9].Visible = false;
        //            RadGrid2.Columns[10].Visible = false;
        //            RadGrid2.Columns[i].Visible = true;
        //        }
        //        else if (i == 0)
        //        {
        //            RadGrid2.Columns[11].Visible = true;
        //            RadGrid2.Columns[5].Visible = true;
        //            RadGrid2.Columns[6].Visible = true;
        //            RadGrid2.Columns[7].Visible = true;
        //            RadGrid2.Columns[8].Visible = true;
        //            RadGrid2.Columns[9].Visible = true;
        //            RadGrid2.Columns[10].Visible = true;
        //            RadGrid2.Columns[i].Visible = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

        //    }
        //}

        protected void RadGrid_bulk_ItemDataBound(object sender, GridItemEventArgs e)
        {

        }

        protected void RadGrid_bulk_UpdateCommand(object source, GridCommandEventArgs e)
        {
        }

        protected void RadGrid_bulk_ItemCommand(object sender, GridCommandEventArgs e)
        {

            //RadGrid_bulk.DataSource = SQLDataConnection.GetInstance().ExecuteDataSet(sQry_bulk);
            //RadGrid_bulk.Rebind();
            //if (e.CommandName == RadGrid.InitInsertCommandName)
            //{
            //    e.Canceled = true;
            //    RadGrid_bulk.EditIndexes.Clear();

            //    e.Item.OwnerTableView.InsertItem();
            //}
            //else if (e.CommandName == RadGrid.EditCommandName)
            //{
            //    e.Item.OwnerTableView.IsItemInserted = false;
            //}
        }

        protected void RadGrid_bulk_OnDeleteCommand(object source, GridCommandEventArgs e)
        {
        }

        protected void RadGrid_bulk_InsertCommand(object source, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem editedItem = e.Item as GridEditableItem;
                Hashtable newValues = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);


                DateTime weekEnding = DateTime.Parse(newValues["Week_endings"].ToString());
                DateTime start_bulk = DateTime.Parse(newValues["start_bulk"].ToString());
                DateTime end_bulk = DateTime.Parse(newValues["end_bulk"].ToString());
                double work_load = double.Parse(newValues["Work_load"].ToString());

                DateTime temp_start = start_bulk;
                DateTime temp_prev = start_bulk;
                DateTime temp_end = end_bulk;

                ////
                DayOfWeek day = start_bulk.DayOfWeek;
                int days = day - DayOfWeek.Saturday;
                DateTime start = temp_prev.AddDays(-days);
                ///////////
                DayOfWeek day_end = end_bulk.DayOfWeek;
                int days_end = day_end - DayOfWeek.Saturday;
                DateTime end = temp_end.AddDays(-days_end);
                //////////////

                int nOfDays = 5, nOfDays_2 = 1;

                days = 6 + days;
                days_end = 6 + days_end;

                if (days == 6)
                    days = nOfDays_2;
                if (days_end == 6)
                    days_end = days_end - nOfDays_2;



                //string QryInsert = string.Format(Settings.Default.qryInsert_bulk, newValues["Resource_id"], newValues["Project_id"], newValues["Work_days"], newValues["Available_days"], newValues["Week_endings"], newValues["Sunday"], newValues["Monday"], newValues["Tuesday"], newValues["Wednesday"], newValues["Thursday"], newValues["Friday"], newValues["Saturday"], "true", newValues["Work_load"], newValues["start_bulk"], newValues["end_bulk"]);
                //int rowsInserted = SQLDataConnection.GetInstance().ExecuteQuery(QryInsert);

                string[] week_begin = new string[7];
                string[] week_last = new string[7];

                while (end_bulk.Date > temp_start.Date)
                {

                    if (temp_start.Date == start_bulk.Date)
                    {

                        while (nOfDays >= days)
                        {

                            week_begin[days] = work_load.ToString();
                            days++;
                        }
                        //string QryInsert_bulk_being = string.Format(Settings.Default.qryInsert_bulk, newValues["Resource_id"], newValues["Project_id"], newValues["Work_days"], newValues["Available_days"], start.Date, week_begin[0], week_begin[1], week_begin[2], week_begin[3], week_begin[4], week_begin[5], week_begin[6], "", "", "", "");
                        //rowsInserted = SQLDataConnection.GetInstance().ExecuteQuery(QryInsert_bulk_being);
                        while (days_end >= nOfDays_2)
                        {

                            week_last[days_end] = work_load.ToString();
                            days_end--;
                        }
                        // string QryInsert_bulk_last = string.Format(Settings.Default.qryInsert_bulk, newValues["Resource_id"], newValues["Project_id"], newValues["Work_days"], newValues["Available_days"], end.Date, week_last[0], week_last[1], week_last[2], week_last[3], week_last[4], week_last[5], week_last[6], "", "", "", "");
                        //rowsInserted = SQLDataConnection.GetInstance().ExecuteQuery(QryInsert_bulk_last);
                    }
                    else if (temp_start.DayOfWeek == DayOfWeek.Saturday && temp_start.Date != start.Date && temp_start.Date != end.Date)
                    {
                        //  string QryInsert_bulk = string.Format(Settings.Default.qryInsert_bulk, newValues["Resource_id"], newValues["Project_id"], newValues["Work_days"], newValues["Available_days"], temp_start.Date, newValues["Sunday"], work_load, work_load, work_load, work_load, work_load, newValues["Saturday"], "", "", "", "");
                        //rowsInserted = SQLDataConnection.GetInstance().ExecuteQuery(QryInsert_bulk);
                    }


                    int daysInMonth = DateTime.DaysInMonth(temp_start.Year, temp_start.Month);
                    if (temp_start.Day == daysInMonth)
                        temp_start.AddMonths(1);
                    temp_start = temp_start.AddDays(1);

                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }

        }

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {

                if (e.Item.OwnerTableView.Name == "MasterTable")
                {

                    if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
                    {
                        Control target = e.Item.FindControl("targetControl1");
                        if (!Object.Equals(target, null))
                        {
                            if (!Object.Equals(this.RadToolTipManager1, null))
                            {
                                //Add the button (target) id to the tooltip manager
                                this.RadToolTipManager1.TargetControls.Add(target.ClientID, (e.Item as GridDataItem).GetDataKeyValue("Resource_id").ToString(), true);

                            }
                        }
                    }


                    if (e.Item is GridDataItem)
                    {
                        //Get the instance of the right type
                        // ImageButton imgbtn = (ImageButton)item["TemplateColumn"].FindControl("ImageButton_Edit"); 
                        GridDataItem dataBoundItem = e.Item as GridDataItem;
                        //HyperLink hp = (HyperLink)dataBoundItem["MondayTemp"].FindControl("targetControl1");
                        //if(dataBoundItem.GetDataKeyValue("ID").ToString() == "you Compared Text") // you can also use datakey also

                        if (dataBoundItem["Monday"].Text != "&nbsp;")
                        {

                            if (Convert.ToDouble(dataBoundItem["Monday"].Text) >= 9)
                            {

                                // dataBoundItem["Monday"].ForeColor = Color.Red; // chanmge particuler cell
                                dataBoundItem["Monday"].BackColor = Color.Red;
                                dataBoundItem["MondayTemplate"].BackColor = Color.Red;
                                //  e.Item.BackColor = System.Drawing.Color.LightGoldenrodYellow; // for whole row
                                //dataItem.CssClass = "MyMexicoRowClass"; 
                            }
                            if (Convert.ToDouble(dataBoundItem["Monday"].Text) >= 5 && Convert.ToDouble(dataBoundItem["Monday"].Text) <= 8.9)
                            {
                                // dataBoundItem["Monday"].ForeColor = Color.Red; // chanmge particuler cell
                                dataBoundItem["Monday"].BackColor = Color.LightGreen;
                                dataBoundItem["MondayTemplate"].BackColor = Color.LightGreen;
                                //  e.Item.BackColor = System.Drawing.Color.LightGoldenrodYellow; // for whole row
                                //dataItem.CssClass = "MyMexicoRowClass"; 
                            }
                            if (Convert.ToDouble(dataBoundItem["Monday"].Text) >= 0.1 && Convert.ToDouble(dataBoundItem["Monday"].Text) <= 4.9)
                            {
                                // dataBoundItem["Monday"].ForeColor = Color.Red; // chanmge particuler cell
                                dataBoundItem["Monday"].BackColor = Color.LightGreen;
                                dataBoundItem["MondayTemplate"].BackColor = Color.Gray;
                                //  e.Item.BackColor = System.Drawing.Color.LightGoldenrodYellow; // for whole row
                                //dataItem.CssClass = "MyMexicoRowClass"; 
                            }
                            //else
                            //    dataBoundItem["Monday"].BackColor = Color.LightYellow;
                        }
                        if (dataBoundItem["Tuesday"].Text.ToString() != "&nbsp;")
                        {
                            if (Convert.ToDouble(dataBoundItem["Tuesday"].Text.ToString()) >= 9)
                            {
                                //  dataBoundItem["Tuesday"].ForeColor = Color.Red; // chanmge particuler cell
                                dataBoundItem["Tuesday"].BackColor = Color.Red;
                                dataBoundItem["TuesdayTemplate"].BackColor = Color.Red;
                                //  e.Item.Cells[4].BackColor = Color.Red; 
                                //  e.Item.BackColor = System.Drawing.Color.LightGoldenrodYellow; // for whole row
                                //dataItem.CssClass = "MyMexicoRowClass"; 
                            }
                            if (Convert.ToDouble(dataBoundItem["Tuesday"].Text.ToString()) >= 5 && Convert.ToDouble(dataBoundItem["Tuesday"].Text.ToString()) <= 8.9)
                            {
                                //  dataBoundItem["Tuesday"].ForeColor = Color.Red; // chanmge particuler cell
                                dataBoundItem["Tuesday"].BackColor = Color.LightGreen;
                                dataBoundItem["TuesdayTemplate"].BackColor = Color.LightGreen;
                                //  e.Item.Cells[4].BackColor = Color.Red; 
                                //  e.Item.BackColor = System.Drawing.Color.LightGoldenrodYellow; // for whole row
                                //dataItem.CssClass = "MyMexicoRowClass"; 
                            }
                            if (Convert.ToDouble(dataBoundItem["Tuesday"].Text.ToString()) >= 0.1 && Convert.ToDouble(dataBoundItem["Tuesday"].Text.ToString()) <= 4.9)
                            {
                                //  dataBoundItem["Tuesday"].ForeColor = Color.Red; // chanmge particuler cell
                                dataBoundItem["Tuesday"].BackColor = Color.LightGreen;
                                dataBoundItem["TuesdayTemplate"].BackColor = Color.Gray;
                                //  e.Item.Cells[4].BackColor = Color.Red; 
                                //  e.Item.BackColor = System.Drawing.Color.LightGoldenrodYellow; // for whole row
                                //dataItem.CssClass = "MyMexicoRowClass"; 
                            }
                            //else
                            //    dataBoundItem["Tuesday"].BackColor = Color.LightYellow;
                        }
                        if (dataBoundItem["Wednesday"].Text != "&nbsp;")
                        {
                            if (Convert.ToDouble(dataBoundItem["Wednesday"].Text) >= 9)
                            {
                                // dataBoundItem["Wednesday"].ForeColor = Color.Red; // chanmge particuler cell
                                dataBoundItem["Wednesday"].BackColor = Color.Red;
                                dataBoundItem["WednesdayTemplate"].BackColor = Color.Red;
                                //  e.Item.Cells[4].BackColor = Color.Red;
                                //  e.Item.BackColor = System.Drawing.Color.LightGoldenrodYellow; // for whole row
                                //dataItem.CssClass = "MyMexicoRowClass"; 
                            }
                            if (Convert.ToDouble(dataBoundItem["Wednesday"].Text) >= 5 && Convert.ToDouble(dataBoundItem["Wednesday"].Text) <= 8.9)
                            {
                                // dataBoundItem["Wednesday"].ForeColor = Color.Red; // chanmge particuler cell
                                dataBoundItem["Wednesday"].BackColor = Color.LightGreen;
                                dataBoundItem["WednesdayTemplate"].BackColor = Color.LightGreen;
                                //  e.Item.Cells[4].BackColor = Color.Red;
                                //  e.Item.BackColor = System.Drawing.Color.LightGoldenrodYellow; // for whole row
                                //dataItem.CssClass = "MyMexicoRowClass"; 
                            }
                            if (Convert.ToDouble(dataBoundItem["Wednesday"].Text) >= 0.1 && Convert.ToDouble(dataBoundItem["Wednesday"].Text) <= 4.9)
                            {
                                // dataBoundItem["Wednesday"].ForeColor = Color.Red; // chanmge particuler cell
                                dataBoundItem["Wednesday"].BackColor = Color.LightGreen;
                                dataBoundItem["WednesdayTemplate"].BackColor = Color.Gray;
                                //  e.Item.Cells[4].BackColor = Color.Red;
                                //  e.Item.BackColor = System.Drawing.Color.LightGoldenrodYellow; // for whole row
                                //dataItem.CssClass = "MyMexicoRowClass"; 
                            }
                            //else
                            //    dataBoundItem["Wednesday"].BackColor = Color.LightYellow;

                        }
                        if (dataBoundItem["Thursday"].Text != "&nbsp;")
                        {
                            if (Convert.ToDouble(dataBoundItem["Thursday"].Text) >= 9)
                            {
                                // dataBoundItem["Thursday"].ForeColor = Color.Red; // chanmge particuler cell
                                dataBoundItem["Thursday"].BackColor = Color.Red;
                                dataBoundItem["ThursdayTemplate"].BackColor = Color.Red;
                                //  e.Item.BackColor = System.Drawing.Color.LightGoldenrodYellow; // for whole row
                                //dataItem.CssClass = "MyMexicoRowClass"; 
                            }
                            if (Convert.ToDouble(dataBoundItem["Thursday"].Text) >= 5 && Convert.ToDouble(dataBoundItem["Thursday"].Text) <= 8.9)
                            {
                                // dataBoundItem["Thursday"].ForeColor = Color.Red; // chanmge particuler cell
                                dataBoundItem["Thursday"].BackColor = Color.LightGreen;
                                dataBoundItem["ThursdayTemplate"].BackColor = Color.LightGreen;
                                //  e.Item.BackColor = System.Drawing.Color.LightGoldenrodYellow; // for whole row
                                //dataItem.CssClass = "MyMexicoRowClass"; 
                            }
                            if (Convert.ToDouble(dataBoundItem["Thursday"].Text) >= 0.1 && Convert.ToDouble(dataBoundItem["Thursday"].Text) <= 4.9)
                            {
                                // dataBoundItem["Thursday"].ForeColor = Color.Red; // chanmge particuler cell
                                dataBoundItem["Thursday"].BackColor = Color.LightGreen;
                                dataBoundItem["ThursdayTemplate"].BackColor = Color.Gray;
                                //  e.Item.BackColor = System.Drawing.Color.LightGoldenrodYellow; // for whole row
                                //dataItem.CssClass = "MyMexicoRowClass"; 
                            }
                            //else
                            //    dataBoundItem["Thursday"].BackColor = Color.LightYellow;
                        }
                        if (dataBoundItem["Friday"].Text != "&nbsp;")
                        {
                            if (Convert.ToDouble(dataBoundItem["Friday"].Text) >= 9)
                            {
                                // dataBoundItem["Friday"].ForeColor = Color.Red; // chanmge particuler cell
                                dataBoundItem["Friday"].BackColor = Color.Red;
                                dataBoundItem["FridayTemplate"].BackColor = Color.Red;
                                //  e.Item.BackColor = System.Drawing.Color.LightGoldenrodYellow; // for whole row
                                //dataItem.CssClass = "MyMexicoRowClass"; 
                            }
                            if (Convert.ToDouble(dataBoundItem["Friday"].Text) >= 5 && Convert.ToDouble(dataBoundItem["Friday"].Text) <= 8.9)
                            {
                                // dataBoundItem["Friday"].ForeColor = Color.Red; // chanmge particuler cell
                                dataBoundItem["Friday"].BackColor = Color.LightGreen;
                                dataBoundItem["FridayTemplate"].BackColor = Color.LightGreen;
                                //  e.Item.BackColor = System.Drawing.Color.LightGoldenrodYellow; // for whole row
                                //dataItem.CssClass = "MyMexicoRowClass"; 
                            }
                            if (Convert.ToDouble(dataBoundItem["Friday"].Text) >= 0.1 && Convert.ToDouble(dataBoundItem["Friday"].Text) <= 4.9)
                            {
                                // dataBoundItem["Friday"].ForeColor = Color.Red; // chanmge particuler cell
                                dataBoundItem["Friday"].BackColor = Color.LightGreen;
                                dataBoundItem["FridayTemplate"].BackColor = Color.Gray;
                                //  e.Item.BackColor = System.Drawing.Color.LightGoldenrodYellow; // for whole row
                                //dataItem.CssClass = "MyMexicoRowClass"; 
                            }


                        }
                        if (dataBoundItem["Saturday"].Text != "&nbsp;")
                        {
                            if (Convert.ToDouble(dataBoundItem["Saturday"].Text) >= 9)
                            {
                                // dataBoundItem["Saturday"].ForeColor = Color.Red; // chanmge particuler cell
                                dataBoundItem["Saturday"].BackColor = Color.Red;
                                dataBoundItem["SaturdayTemplate"].BackColor = Color.Red;
                                //  e.Item.BackColor = System.Drawing.Color.LightGoldenrodYellow; // for whole row
                                //dataItem.CssClass = "MyMexicoRowClass"; 
                            }
                            if (Convert.ToDouble(dataBoundItem["Saturday"].Text) >= 5 && Convert.ToDouble(dataBoundItem["Saturday"].Text) <= 8.9)
                            {
                                // dataBoundItem["Saturday"].ForeColor = Color.Red; // chanmge particuler cell
                                dataBoundItem["Saturday"].BackColor = Color.LightGreen;
                                dataBoundItem["SaturdayTemplate"].BackColor = Color.LightGreen;
                                //  e.Item.BackColor = System.Drawing.Color.LightGoldenrodYellow; // for whole row
                                //dataItem.CssClass = "MyMexicoRowClass"; 
                            }
                            if (Convert.ToDouble(dataBoundItem["Saturday"].Text) >= 0.1 && Convert.ToDouble(dataBoundItem["Saturday"].Text) <= 4.9)
                            {
                                // dataBoundItem["Saturday"].ForeColor = Color.Red; // chanmge particuler cell
                                dataBoundItem["Saturday"].BackColor = Color.LightGreen;
                                dataBoundItem["SaturdayTemplate"].BackColor = Color.Gray;
                                //  e.Item.BackColor = System.Drawing.Color.LightGoldenrodYellow; // for whole row
                                //dataItem.CssClass = "MyMexicoRowClass"; 
                            }


                        }
                        if (dataBoundItem["Sunday"].Text != "&nbsp;")
                        {
                            if (Convert.ToDouble(dataBoundItem["Sunday"].Text) >= 9)
                            {
                                // dataBoundItem["Sunday"].ForeColor = Color.Red; // chanmge particuler cell
                                dataBoundItem["Sunday"].BackColor = Color.Red;
                                dataBoundItem["SundayTempalte"].BackColor = Color.Red;
                                //  e.Item.BackColor = System.Drawing.Color.LightGoldenrodYellow; // for whole row
                                //dataItem.CssClass = "MyMexicoRowClass"; 
                            }
                            if (Convert.ToDouble(dataBoundItem["Sunday"].Text) >= 5 && Convert.ToDouble(dataBoundItem["Sunday"].Text) <= 8.9)
                            {
                                // dataBoundItem["Sunday"].ForeColor = Color.Red; // chanmge particuler cell
                                dataBoundItem["Sunday"].BackColor = Color.LightGreen;
                                dataBoundItem["SundayTemplate"].BackColor = Color.LightGreen;
                                //  e.Item.BackColor = System.Drawing.Color.LightGoldenrodYellow; // for whole row
                                //dataItem.CssClass = "MyMexicoRowClass"; 
                            }
                            if (Convert.ToDouble(dataBoundItem["Sunday"].Text) >= 0.1 && Convert.ToDouble(dataBoundItem["Sunday"].Text) <= 4.9)
                            {
                                // dataBoundItem["Sunday"].ForeColor = Color.Red; // chanmge particuler cell
                                dataBoundItem["Sunday"].BackColor = Color.LightGreen;
                                dataBoundItem["SundayTemplate"].BackColor = Color.Gray;
                                //  e.Item.BackColor = System.Drawing.Color.LightGoldenrodYellow; // for whole row
                                //dataItem.CssClass = "MyMexicoRowClass"; 
                            }


                        }
                        if (dataBoundItem["Friday"].Text == "&nbsp;" || Convert.ToDouble(dataBoundItem["Friday"].Text) == 0)
                        {
                            dataBoundItem["Friday"].BackColor = Color.Yellow;
                            // dataBoundItem["FridayTemplate"].BackColor = Color.LightYellow;

                        }
                        if (dataBoundItem["Thursday"].Text == "&nbsp;" || Convert.ToDouble(dataBoundItem["Thursday"].Text) == 0)
                        {
                            dataBoundItem["Thursday"].BackColor = Color.Yellow;
                            //   dataBoundItem["ThursdayTemplate"].BackColor = Color.LightYellow;

                        }
                        if (dataBoundItem["Wednesday"].Text == "&nbsp;" || Convert.ToDouble(dataBoundItem["Wednesday"].Text) == 0)
                        {
                            dataBoundItem["Wednesday"].BackColor = Color.Yellow;
                            //   dataBoundItem["WednesdayTemplate"].BackColor = Color.LightYellow;

                        }
                        if (dataBoundItem["Wednesday"].Text == "&nbsp;" || Convert.ToDouble(dataBoundItem["Wednesday"].Text) == 0)
                        {
                            dataBoundItem["Wednesday"].BackColor = Color.Yellow;
                            //   dataBoundItem["WednesdayTemplate"].BackColor = Color.LightYellow;

                        }
                        if (dataBoundItem["Tuesday"].Text.ToString() == "&nbsp;" || Convert.ToDouble(dataBoundItem["Tuesday"].Text) == 0)
                        {
                            dataBoundItem["Tuesday"].BackColor = Color.Yellow;
                            //   dataBoundItem["TuesdayTemplate"].BackColor = Color.LightYellow;

                        }
                        if (dataBoundItem["Monday"].Text == "&nbsp;" || Convert.ToDouble(dataBoundItem["Monday"].Text) == 0)
                        {
                            dataBoundItem["Monday"].BackColor = Color.Yellow;
                            //    dataBoundItem["MondayTemplate"].BackColor = Color.LightYellow;
                        }
                        if (dataBoundItem["Saturday"].Text == "&nbsp;" || Convert.ToDouble(dataBoundItem["Saturday"].Text) == 0)
                        {
                            dataBoundItem["Saturday"].BackColor = Color.Yellow;
                            //   dataBoundItem["SaturdayTemplate"].BackColor = Color.LightYellow;
                        }
                        if (dataBoundItem["Sunday"].Text == "&nbsp;" || Convert.ToDouble(dataBoundItem["Sunday"].Text) == 0)
                        {
                            dataBoundItem["Sunday"].BackColor = Color.Yellow;
                            //   dataBoundItem["SundayTemplate"].BackColor = Color.LightYellow;
                        }
                    }
                }
                else if (e.Item.OwnerTableView.Name == "DetailTable")
                {
                    if ((e.Item is GridDataInsertItem && e.Item.IsInEditMode))
                    {


                        //   string sResourceProject = string.Format(Settings.Default.qryResourceProject, lblResourceID.Text);
                        //  DataSet dt = SQLDataConnection.GetInstance().ExecuteDataSet(sResourceProject);
                        GridEditableItem editedItem = e.Item as GridEditableItem;
                        GridEditManager editMan = editedItem.EditManager;
                        GridDropDownListColumnEditor DDLDevice = editMan.GetColumnEditor("Project_id") as GridDropDownListColumnEditor;
                        DDLDevice.DataSource = null;
                        DDLDevice.DataTextField = "Project_name";
                        DDLDevice.DataValueField = "Project_id";
                        DDLDevice.DataBind();
                        GridDropDownListColumnEditor DDLResource = editMan.GetColumnEditor("Resource_id") as GridDropDownListColumnEditor;
                        DDLResource.SelectedValue = lblResourceID.Text;
                        //editor.DropDownListControl.SelectedValue = "2";
                    }

                    if (e.Item is GridDataItem && e.Item.IsInEditMode)
                    {
                        GridDataItem item = (GridDataItem)e.Item;
                        RadComboBox combo = (RadComboBox)item["Resource_id"].Controls[0]; // Access the radcombobox in EditMode  
                        // Set the width  
                        combo.Width = Unit.Pixel(120);

                        RadComboBox Project_id = (RadComboBox)item["Project_id"].Controls[0]; // Access the radcombobox in EditMode  
                        // Set the width  
                        Project_id.Width = Unit.Pixel(100);

                        RadDatePicker weekendding = (RadDatePicker)item["Week_endings"].Controls[0]; // Access the radcombobox in EditMode 
                        weekendding.Width = Unit.Pixel(100);

                        TextBox mon = (TextBox)item["Monday"].Controls[0]; // Access the radcombobox in EditMode 
                        mon.Width = Unit.Pixel(25);

                        TextBox Tuesday = (TextBox)item["Tuesday"].Controls[0]; // Access the radcombobox in EditMode 
                        Tuesday.Width = Unit.Pixel(25);

                        TextBox Wednesday = (TextBox)item["Wednesday"].Controls[0]; // Access the radcombobox in EditMode 
                        Wednesday.Width = Unit.Pixel(25);

                        TextBox Thursday = (TextBox)item["Thursday"].Controls[0]; // Access the radcombobox in EditMode 
                        Thursday.Width = Unit.Pixel(25);

                        TextBox Friday = (TextBox)item["Friday"].Controls[0]; // Access the radcombobox in EditMode 
                        Friday.Width = Unit.Pixel(25);

                        TextBox Saturday = (TextBox)item["Saturday"].Controls[0]; // Access the radcombobox in EditMode 
                        Saturday.Width = Unit.Pixel(25);

                        TextBox Sunday = (TextBox)item["Sunday"].Controls[0]; // Access the radcombobox in EditMode 
                        Sunday.Width = Unit.Pixel(25);
                    }
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }
        }

        protected void RadGrid1_ItemCommand(object source, GridCommandEventArgs e)
        {

            try
            {

                if (e.CommandName == "Sort" || e.CommandName == "Page")
                {
                    RadToolTipManager1.TargetControls.Clear();
                }

                if (e.Item.OwnerTableView.Name == "MasterTable")
                {
                    if (tbEnterBool.Text == "true")
                    {
                        RadGrid selected = (RadGrid)source;
                        int i = Convert.ToInt32(tbCurrentRow.Text);
                        lbRowID.Text = selected.MasterTableView.DataKeyValues[i]["Resource_id"].ToString();
                        lbWeek_ending_OnClick(source, e);

                    }
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }
        }

        protected void RadGrid1_OnDeleteCommand(object source, GridCommandEventArgs e)
        {
            try
            {
                if (e.Item.OwnerTableView.Name == "DetailTable")
                {
                    GridDataItem currentRow = (GridDataItem)e.Item;
                    int end = currentRow.KeyValues.Substring(7).Length - 2;
                    String skey = currentRow.KeyValues.Substring(7, end);
                    string sDeleteQry = string.Format("DELETE FROM [ProMan].[dbo].[Weekly_Reports]  WHERE id='{0}'", skey);

                    //  int rowsInserted = SQLDataConnection.GetInstance().ExecuteQuery(sDeleteQry);

                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }
        }

        protected void RadGrid1_InsertCommand(object source, GridCommandEventArgs e)
        {
            try
            {
                //if (e.Item.OwnerTableView.Name == "DetailTable")
                //{

                //    GridEditableItem editedItem = e.Item as GridEditableItem;
                //    Hashtable newValues = new Hashtable();
                //    e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

                //    string QryInsert = string.Format(Settings.Default.qryInsert, newValues["Resource_id"], newValues["Project_id"], newValues["Work_days"], newValues["Available_days"], newValues["Week_endings"], newValues["sunday"], newValues["monday"], newValues["tuesday"], newValues["wednesday"], newValues["thursday"], newValues["friday"], newValues["saturday"]);
                //    int rowsInserted = SQLDataConnection.GetInstance().ExecuteQuery(QryInsert);

                //}

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }

        }

        protected void RadGrid1_UpdateCommand(object source, GridCommandEventArgs e)
        {
            //if (e.Item.OwnerTableView.Name == "DetailTable")
            //{
            //    string sUpdateQry = "UPDATE [ProMan].[dbo].[Weekly_Reports]   SET ";
            //    GridDataItem currentRow = (GridDataItem)e.Item;
            //    // GridEditFormItem currentRow = (GridEditFormItem)e.Item;
            //    int end = currentRow.KeyValues.Substring(7).Length - 2;
            //    String skey = currentRow.KeyValues.Substring(7, end);

            //    int chk = sUpdateQry.Length;
            //    GridEditableItem editedItem = e.Item as GridEditableItem;


            //    Hashtable newValues = new Hashtable();
            //    e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);
            //    Hashtable HTEdited = new Hashtable(newValues.Count);

            //    foreach (DictionaryEntry Edited in newValues)
            //    {
            //        if (Edited.Value != null)
            //        {
            //            HTEdited.Add(Edited.Key, Edited.Value);
            //            if (sUpdateQry.Length > chk)
            //                sUpdateQry += ",";
            //            sUpdateQry += string.Format("[{0}] = '{1}'", Edited.Key, Edited.Value);
            //        }
            //    }
            //    sUpdateQry += string.Format("WHERE id='{0}'", skey);
            //    int rowsInserted = SQLDataConnection.GetInstance().ExecuteQuery(sUpdateQry);
            //    //RadGrid2.DataSource = SQLDataConnection.GetInstance().ExecuteDataSet(sQry);

            //    // RadGrid2.Rebind();
            //    RadGrid1.MasterTableView.ClearEditItems();
            //    // RadGrid1.Rebind();
            //}
        }

        protected void RadGrid1_DetailTableDataBind(object source, GridDetailTableDataBindEventArgs e)
        {

            try
            {

                //DataSet DTResourceDetailTable = SQLDataConnection.GetInstance().ExecuteDataSet(string.Format(Settings.Default.qryResourceDetailTableBind.ToString(), dpWeekEnding.SelectedDate.ToString()));
                //e.DetailTableView.DataSource = DTResourceDetailTable;

            }


            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }
        }

        protected void RadGrid2_ItemDataBound(object sender, GridItemEventArgs e)
        {

            //if ((e.Item is GridDataInsertItem && e.Item.IsInEditMode))
            //{


            //    string sResourceProject = string.Format(Settings.Default.qryResourceProject, lblResourceID.Text);
            //    DataSet dt = SQLDataConnection.GetInstance().ExecuteDataSet(sResourceProject);

            //    GridEditableItem editedItem = e.Item as GridEditableItem;
            //    GridEditManager editMan = editedItem.EditManager;

            //    GridDropDownListColumnEditor DDLDevice = editMan.GetColumnEditor("Project_id") as GridDropDownListColumnEditor;
            //    DDLDevice.DataSource = dt;
            //    DDLDevice.DataTextField = "Project_name";
            //    DDLDevice.DataValueField = "Project_id";
            //    DDLDevice.DataBind();

            //    GridDropDownListColumnEditor DDLResource = editMan.GetColumnEditor("Resource_id") as GridDropDownListColumnEditor;
            //    DDLResource.SelectedValue = lblResourceID.Text;

            //    //editor.DropDownListControl.SelectedValue = "2";
            //}

            //if (e.Item is GridDataItem && e.Item.IsInEditMode)
            //{
            //    GridDataItem item = (GridDataItem)e.Item;
            //    RadComboBox combo = (RadComboBox)item["Resource_id"].Controls[0]; // Access the radcombobox in EditMode  
            //    // Set the width  
            //    combo.Width = Unit.Pixel(120);

            //    RadComboBox Project_id = (RadComboBox)item["Project_id"].Controls[0]; // Access the radcombobox in EditMode  
            //    // Set the width  
            //    Project_id.Width = Unit.Pixel(100);

            //    RadDatePicker weekendding = (RadDatePicker)item["Week_endings"].Controls[0]; // Access the radcombobox in EditMode 
            //    weekendding.Width = Unit.Pixel(100);



            //    TextBox mon = (TextBox)item["Monday"].Controls[0]; // Access the radcombobox in EditMode 
            //    mon.Width = Unit.Pixel(25);
            //    TextBox Tuesday = (TextBox)item["Tuesday"].Controls[0]; // Access the radcombobox in EditMode 
            //    Tuesday.Width = Unit.Pixel(25);
            //    TextBox Wednesday = (TextBox)item["Wednesday"].Controls[0]; // Access the radcombobox in EditMode 
            //    Wednesday.Width = Unit.Pixel(25);
            //    TextBox Thursday = (TextBox)item["Thursday"].Controls[0]; // Access the radcombobox in EditMode 
            //    Thursday.Width = Unit.Pixel(25);
            //    TextBox Friday = (TextBox)item["Friday"].Controls[0]; // Access the radcombobox in EditMode 
            //    Friday.Width = Unit.Pixel(25);
            //    TextBox Saturday = (TextBox)item["Saturday"].Controls[0]; // Access the radcombobox in EditMode 
            //    Saturday.Width = Unit.Pixel(25);
            //    TextBox Sunday = (TextBox)item["Sunday"].Controls[0]; // Access the radcombobox in EditMode 
            //    Sunday.Width = Unit.Pixel(25);
            //}

        }

        protected void RadGrid2_InsertCommand(object source, GridCommandEventArgs e)
        {


            //GridEditableItem editedItem = e.Item as GridEditableItem;
            //Hashtable newValues = new Hashtable();
            //e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

            //string QryInsert = string.Format(Settings.Default.qryInsert, newValues["Resource_id"], newValues["Project_id"], newValues["Work_days"], newValues["Available_days"], newValues["Week_endings"], newValues["Sunday"], newValues["Monday"], newValues["Tuesday"], newValues["Wednesday"], newValues["Thursday"], newValues["Friday"], newValues["Saturday"]);
            //int rowsInserted = SQLDataConnection.GetInstance().ExecuteQuery(QryInsert);
            //RadGrid2.DataSource = SQLDataConnection.GetInstance().ExecuteDataSet(sQry);
            //RadGrid2.Rebind();
            //RadGrid1.Rebind();



        }

        protected void RadGrid2_UpdateCommand(object source, GridCommandEventArgs e)
        {
            //string sUpdateQry = "UPDATE [ProMan].[dbo].[Weekly_Reports]   SET ";
            //GridDataItem currentRow = (GridDataItem)e.Item;
            //// GridEditFormItem currentRow = (GridEditFormItem)e.Item;
            //int end = currentRow.KeyValues.Substring(7).Length - 2;
            //String skey = currentRow.KeyValues.Substring(7, end);

            //int chk = sUpdateQry.Length;
            //GridEditableItem editedItem = e.Item as GridEditableItem;


            //Hashtable newValues = new Hashtable();
            //e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);
            //Hashtable HTEdited = new Hashtable(newValues.Count);

            //foreach (DictionaryEntry Edited in newValues)
            //{
            //    if (Edited.Value != null)
            //    {
            //        HTEdited.Add(Edited.Key, Edited.Value);
            //        if (sUpdateQry.Length > chk)
            //            sUpdateQry += ",";
            //        sUpdateQry += string.Format("[{0}] = '{1}'", Edited.Key, Edited.Value);
            //    }
            //}
            //sUpdateQry += string.Format("WHERE id='{0}'", skey);
            //int rowsInserted = SQLDataConnection.GetInstance().ExecuteQuery(sUpdateQry);
            //RadGrid2.DataSource = SQLDataConnection.GetInstance().ExecuteDataSet(sQry);
            //RadGrid2.Rebind();
            //RadGrid2.MasterTableView.ClearEditItems();
            //RadGrid1.Rebind();

        }

        protected void RadGrid2_OnDeleteCommand(object source, GridCommandEventArgs e)
        {
            //    GridDataItem currentRow = (GridDataItem)e.Item;
            //    int end = currentRow.KeyValues.Substring(7).Length - 2;
            //    String skey = currentRow.KeyValues.Substring(7, end);
            //    string sDeleteQry = string.Format("DELETE FROM [ProMan].[dbo].[Weekly_Reports]  WHERE id='{0}'", skey);

            //    int rowsInserted = SQLDataConnection.GetInstance().ExecuteQuery(sDeleteQry);
            //    RadGrid2.DataSource = SQLDataConnection.GetInstance().ExecuteDataSet(sQry);
            //    RadGrid2.Rebind();
            //    RadGrid1.Rebind();
        }

        protected void RadGrid2_ItemCommand(object sender, GridCommandEventArgs e)
        {
            //RadGrid2.DataSource = SQLDataConnection.GetInstance().ExecuteDataSet(sQry);
            //RadGrid2.Rebind();
            //if (e.CommandName == RadGrid.InitInsertCommandName)
            //{
            //    e.Canceled = true;
            //    RadGrid2.EditIndexes.Clear();

            //    e.Item.OwnerTableView.InsertItem();
            //}
            //else if (e.CommandName == RadGrid.EditCommandName)
            //{
            //    e.Item.OwnerTableView.IsItemInserted = false;
            //}

            //RadGrid2.Focus();


        }

        #endregion

        protected void RadGrid_weekly_DataBinding(object sender, EventArgs e)
        {
            RadGrid_weekly.MasterTableView.GetColumn("UName").HeaderText = PropertyLayer.Name;
            //RadGrid_weekly.MasterTableView.GetColumn("UProject").HeaderText = PropertyLayer.GrdAsingmentProjectName;
            RadGrid_weekly.MasterTableView.DetailTables[0].GetColumn("UProject").HeaderText = PropertyLayer.GrdAsingmentProjectName;
            RadGrid_weekly.MasterTableView.DetailTables[0].GetColumn("URole").HeaderText = PropertyLayer.RoleName;



        }
    }
}