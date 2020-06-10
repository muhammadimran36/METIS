using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.Drawing;
using System.Collections;
using System.IO;
using streebo.METIS.BLL;
using System.Globalization;


namespace streebo.METIS.UI
{
    public partial class RFS : System.Web.UI.Page
    {
        #region Object Declaration
        private MetisBLL objBLL;
        #endregion        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region Check Login
                if (Convert.ToString(Session["user"]) == "")
                    Response.Redirect("Login.aspx");
                #endregion

                Boolean b_CanView = false;

                objBLL = new MetisBLL();

                System.Data.DataTable dt = objBLL.getAccessRights(Convert.ToString(Session["user"]));
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    if (row["EntityName"].ToString() == "RFS") { b_CanView = Convert.ToBoolean(row["Can_View"]); }
                }
                //If Admin thn bypass security
                if (b_CanView == false)
                {
                    objBLL = new MetisBLL();
                    if (Convert.ToBoolean(objBLL.IsAdmin(Convert.ToString(Session["user"]))))
                        b_CanView = true;
                }
                if (!b_CanView)
                {
                    MainProject.Visible = false;
                    lblErr.Text = "403 Forbidden";
                }
                
                System.Globalization.DateTimeFormatInfo d = new System.Globalization.DateTimeFormatInfo();

                string[] fields = { "Time Year" };
                DataTable dtb = SelectDistinct(objBLL.getTimes(), fields);
                ddlYear.DataSource = dtb;
                ddlYear.DataTextField = dtb.Columns[0].ToString();
                ddlYear.DataValueField = dtb.Columns[0].ToString();
                ddlYear.DataBind();                    
                ddlYear.Items.FindByText(DateTime.Now.Year.ToString()).Selected = true;
                //ddlYear.SelectedItem.Text = "All";

                Session["Bind"] = true;
                BindData();        
            }
        }

        protected void refreshHeader()
        {
            for (int i = 0; i < this.Controls.Count; i++)
            {
                switch (this.Controls[i].GetType().ToString())
                {
                    case "System.Web.UI.WebControls.TextBox":
                        HtmlGenericControl hgcHtmlGenericControl = (HtmlGenericControl)this.FindControl(this.Controls[i].ID);
                        hgcHtmlGenericControl.Attributes.Remove("class");
                        break;
                    default:
                        break;
                }
            }

            HtmlGenericControl listRevenue = (HtmlGenericControl)this.FindControl("Header1").FindControl("listRevenue");
            listRevenue.Attributes.Add("class", "active");
        }


        #region "AS GRIDVIEW"
        

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {

            BindData();

        }

        private static DataTable SelectDistinct(DataTable SourceTable, params string[] FieldNames)
        {
            object[] lastValues;
            DataTable newTable;
            DataRow[] orderedRows;

            if (FieldNames == null || FieldNames.Length == 0)
                throw new ArgumentNullException("FieldNames");

            lastValues = new object[FieldNames.Length];
            newTable = new DataTable();

            foreach (string fieldName in FieldNames)
                newTable.Columns.Add(fieldName, SourceTable.Columns[fieldName].DataType);

            orderedRows = SourceTable.Select("", string.Join(", ", FieldNames));

            foreach (DataRow row in orderedRows)
            {
                if (!fieldValuesAreEqual(lastValues, row, FieldNames))
                {
                    newTable.Rows.Add(createRowClone(row, newTable.NewRow(), FieldNames));

                    setLastValues(lastValues, row, FieldNames);
                }
            }

            return newTable;
        }
        private static void setLastValues(object[] lastValues, DataRow sourceRow, string[] fieldNames)
        {
            for (int i = 0; i < fieldNames.Length; i++)
                lastValues[i] = sourceRow[fieldNames[i]];
        }

        private static bool fieldValuesAreEqual(object[] lastValues, DataRow currentRow, string[] fieldNames)
        {
            bool areEqual = true;

            for (int i = 0; i < fieldNames.Length; i++)
            {
                if (lastValues[i] == null || !lastValues[i].Equals(currentRow[fieldNames[i]]))
                {
                    areEqual = false;
                    break;
                }
            }

            return areEqual;
        }

        private static DataRow createRowClone(DataRow sourceRow, DataRow newRow, string[] fieldNames)
        {
            foreach (string field in fieldNames)
                newRow[field] = sourceRow[field];

            return newRow;
        }

        private string GetSortDirection(string column)
        {

            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }
            // Save new values in ViewState.
            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }

        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void ddlConsultant_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }
        #endregion


        void BindData()
        {
            objBLL = new MetisBLL();
            DataTable dt = new DataTable();
            dt = objBLL.getAllTarget_Actual();
            ViewState["Target_Actual"] = dt;
            Session["Target_Actual"] = null;
            DataView dv = new DataView(dt);
            dv.RowFilter = "NewTimeYear like '%" + (ddlYear.SelectedItem.ToString().Replace("'", "''") == "All" ? "" : ddlYear.SelectedItem.ToString().Replace("'", "''")) + "%'";
            if (ViewState["SortExpression"] != null)
            {
                dv.Sort = ViewState["SortExpression"].ToString() + " " + ViewState["SortDirection"].ToString();
            }
            RadGrid1.DataSource = dv;
            if (Convert.ToBoolean(Session["Bind"]) == true)
            {
                RadGrid1.DataBind();

            }
            Session["Bind"] = true;
            
        }

        void BindData(DataView dv)
        {
            Session["Target_Actual"] = dv;
            RadGrid1.DataSource = dv;
            RadGrid1.DataBind();
        }

        void BindData(DataTable dt)
        {

            RadGrid1.DataSource = dt;
            RadGrid1.DataBind();
        }
        
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            Session["Bind"] = false;
            objBLL = new MetisBLL();
            RadGrid1.DataSource = objBLL.getAllTarget_Actual();
            BindData();
        }

        protected void RadGrid1_UpdateCommand(object sender, GridCommandEventArgs e)
        {

        }

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem && e.Item.IsInEditMode)
            {
                GridDataItem item = (GridDataItem)e.Item;
                RadComboBox dropCustomerName = (e.Item as GridEditableItem)["CustomerName"].FindControl("dropCustomerName") as RadComboBox;
                objBLL = new MetisBLL();
                dropCustomerName.DataSource = objBLL.getCustomers();
                dropCustomerName.DataTextField = objBLL.getCustomers().Columns[1].ToString();
                dropCustomerName.DataValueField = objBLL.getCustomers().Columns[0].ToString();
                dropCustomerName.DataBind();

                DropDownList dropProjectName = (e.Item as GridEditableItem)["ProjectName"].FindControl("dropProjectName") as DropDownList;
                objBLL = new MetisBLL();
                dropProjectName.DataSource = objBLL.getProject();
                dropProjectName.DataTextField = objBLL.getProject().Columns[1].ToString();
                dropProjectName.DataValueField = objBLL.getProject().Columns[0].ToString();
                dropProjectName.DataBind();

                DropDownList dropTimeYear = (e.Item as GridEditableItem)["TimeYear"].FindControl("dropTimeYear") as DropDownList;
                objBLL = new MetisBLL();
                dropTimeYear.DataSource = objBLL.getAllTimes_Actual();
                dropTimeYear.DataTextField = objBLL.getAllTimes_Actual().Columns[1].ToString();
                dropTimeYear.DataValueField = objBLL.getAllTimes_Actual().Columns[0].ToString();
                dropTimeYear.DataBind();

                DropDownList dropConsultant = (e.Item as GridEditableItem)["Consultant"].FindControl("dropConsultant") as DropDownList;
                objBLL = new MetisBLL();
                dropConsultant.DataSource = objBLL.getConsultants();
                dropConsultant.DataTextField = objBLL.getConsultants().Columns[1].ToString();
                dropConsultant.DataValueField = objBLL.getConsultants().Columns[0].ToString();
                dropConsultant.DataBind();

                DropDownList dropService = (e.Item as GridEditableItem)["Services"].FindControl("dropService") as DropDownList;
                objBLL = new MetisBLL();
                dropService.DataSource = objBLL.getServices();
                dropService.DataTextField = objBLL.getServices().Columns[1].ToString();
                dropService.DataValueField = objBLL.getServices().Columns[0].ToString();
                dropService.DataBind();


                RadNumericTextBox txtQ1Target = (e.Item as GridEditableItem)["Q1Target"].FindControl("txtQ1Target") as RadNumericTextBox;

                // RadNumericTextBox txtJanuary = (e.Item as GridEditableItem)["Q1Actual"].FindControl("txtJanuary") as RadNumericTextBox;
                // RadNumericTextBox txtFebruary = (e.Item as GridEditableItem)["Q1Actual"].FindControl("txtFebruary") as RadNumericTextBox;
                // RadNumericTextBox txtMarch = (e.Item as GridEditableItem)["Q1Actual"].FindControl("txtMarch") as RadNumericTextBox;

                TextBox txtJanuaryFormula = (e.Item as GridEditableItem)["Q1ActualFormula"].FindControl("txtJanuaryFormula") as TextBox;
                TextBox txtFebruaryFormula = (e.Item as GridEditableItem)["Q1ActualFormula"].FindControl("txtFebruaryFormula") as TextBox;
                TextBox txtMarchFormula = (e.Item as GridEditableItem)["Q1ActualFormula"].FindControl("txtMarchFormula") as TextBox;

                RadNumericTextBox txtQ2Target = (e.Item as GridEditableItem)["Q2Target"].FindControl("txtQ2Target") as RadNumericTextBox;

                // RadNumericTextBox txtApril = (e.Item as GridEditableItem)["Q2Actual"].FindControl("txtApril") as RadNumericTextBox;
                // RadNumericTextBox txtMay = (e.Item as GridEditableItem)["Q2Actual"].FindControl("txtMay") as RadNumericTextBox;
                // RadNumericTextBox txtJune = (e.Item as GridEditableItem)["Q2Actual"].FindControl("txtJune") as RadNumericTextBox;

                TextBox txtAprilFormula = (e.Item as GridEditableItem)["Q2ActualFormula"].FindControl("txtAprilFormula") as TextBox;
                TextBox txtMayFormula = (e.Item as GridEditableItem)["Q2ActualFormula"].FindControl("txtMayFormula") as TextBox;
                TextBox txtJuneFormula = (e.Item as GridEditableItem)["Q2ActualFormula"].FindControl("txtJuneFormula") as TextBox;

                RadNumericTextBox txtQ3Target = (e.Item as GridEditableItem)["Q3Target"].FindControl("txtQ3Target") as RadNumericTextBox;

                // RadNumericTextBox txtJuly = (e.Item as GridEditableItem)["Q3Actual"].FindControl("txtJuly") as RadNumericTextBox;
                // RadNumericTextBox txtAugust = (e.Item as GridEditableItem)["Q3Actual"].FindControl("txtAugust") as RadNumericTextBox;
                // RadNumericTextBox txtSeptember = (e.Item as GridEditableItem)["Q3Actual"].FindControl("txtSeptember") as RadNumericTextBox;

                TextBox txtJulyFormula = (e.Item as GridEditableItem)["Q3ActualFormula"].FindControl("txtJulyFormula") as TextBox;
                TextBox txtAugustFormula = (e.Item as GridEditableItem)["Q3ActualFormula"].FindControl("txtAugustFormula") as TextBox;
                TextBox txtSeptemberFormula = (e.Item as GridEditableItem)["Q3ActualFormula"].FindControl("txtSeptemberFormula") as TextBox;

                RadNumericTextBox txtQ4Target = (e.Item as GridEditableItem)["Q4Target"].FindControl("txtQ4Target") as RadNumericTextBox;

                // RadNumericTextBox txtOctober = (e.Item as GridEditableItem)["Q4Actual"].FindControl("txtOctober") as RadNumericTextBox;
                // RadNumericTextBox txtNovember = (e.Item as GridEditableItem)["Q4Actual"].FindControl("txtNovember") as RadNumericTextBox;
                // RadNumericTextBox txtDecember = (e.Item as GridEditableItem)["Q4Actual"].FindControl("txtDecember") as RadNumericTextBox;

                TextBox txtOctoberFormula = (e.Item as GridEditableItem)["Q4ActualFormula"].FindControl("txtOctoberFormula") as TextBox;
                TextBox txtNovemberFormula = (e.Item as GridEditableItem)["Q4ActualFormula"].FindControl("txtNovemberFormula") as TextBox;
                TextBox txtDecemberFormula = (e.Item as GridEditableItem)["Q4ActualFormula"].FindControl("txtDecemberFormula") as TextBox;

                RadNumericTextBox txtTotalTarget = (e.Item as GridEditableItem)["TotalTarget"].FindControl("txtTotalTarget") as RadNumericTextBox;
                RadNumericTextBox txtTotal_Actual = (e.Item as GridEditableItem)["Total_Actual"].FindControl("txtTotal_Actual") as RadNumericTextBox;
                TextBox txtComments = (e.Item as GridEditableItem)["Comments"].FindControl("txtComments") as TextBox;

                //if (Session["CustomerName"] != null)
                //{
                //    dropCustomerName.SelectedItem.Text = Session["CustomerName"].ToString();
                //    //dropCustomerName.Items.FindItemByText(Session["CustomerName"].ToString()).Selected = true;
                //    //dropCustomerName.FindItemByText(Session["CustomerName"].ToString()).Selected = true;
                //}
                if (Session["CustomerName"] != null)
                {
                    if (Session["CustomerName"].ToString() == "Abbvie")
                        dropCustomerName.SelectedItem.Text = "Abbvie";
                    else
                        //dropCustomerName.SelectedItem.Text = Session["CustomerName"].ToString();
                        dropCustomerName.Items.FindItemByText(Session["CustomerName"].ToString()).Selected = true;
                    //dropCustomerName.FindItemByText(Session["CustomerName"].ToString()).Selected = true;
                }

                if (Session["ProjectName"] != null)
                    dropProjectName.Items.FindByText(Session["ProjectName"].ToString()).Selected = true;
                if (Session["Consultant"] != null)
                    dropConsultant.Items.FindByText(Session["Consultant"].ToString()).Selected = true;
                if (Session["Services"] != null)
                    dropService.Items.FindByText(Session["Services"].ToString()).Selected = true;
                if (Session["TimeYear"] != null)
                    dropTimeYear.Items.FindByText(Session["TimeYear"].ToString()).Selected = true;
                if (Session["Q1Target"] != null)
                    txtQ1Target.Text = Session["Q1Target"].ToString();
                // Q1_ Actual Months Wise Sessions
                //if (Session["Jan"] != null)
                //    txtJanuary.Text = Session["Jan"].ToString();
                //if (Session["Feb"] != null)
                //    txtFebruary.Text = Session["Feb"].ToString();
                //if (Session["Mar"] != null)
                //    txtMarch.Text = Session["Mar"].ToString();


                if (Session["JanF"] != null)
                    txtJanuaryFormula.Text = Session["JanF"].ToString();
                if (Session["FebF"] != null)
                    txtFebruaryFormula.Text = Session["FebF"].ToString();
                if (Session["MarF"] != null)
                    txtMarchFormula.Text = Session["MarF"].ToString();


                //if (Session["Q2Target"] != null)
                //    txtQ2Target.Text = Session["Q2Target"].ToString();
                //// Q2_ Actual Months Wise Sessions
                //if (Session["Apr"] != null)
                //    txtApril.Text = Session["Apr"].ToString();
                //if (Session["May"] != null)
                //    txtMay.Text = Session["May"].ToString();
                //if (Session["Jun"] != null)
                //    txtJune.Text = Session["Jun"].ToString();

                if (Session["Q2Target"] != null)
                    txtQ2Target.Text = Session["Q2Target"].ToString();

                // Q2_ Actual Months Wise Sessions
                if (Session["AprF"] != null)
                    txtAprilFormula.Text = Session["AprF"].ToString();
                if (Session["MayF"] != null)
                    txtMayFormula.Text = Session["MayF"].ToString();
                if (Session["JunF"] != null)
                    txtJuneFormula.Text = Session["JunF"].ToString();


                if (Session["Q3Target"] != null)
                    txtQ3Target.Text = Session["Q3Target"].ToString();
                // Q3_ Actual Months Wise Sessions
                //if (Session["Jul"] != null)
                //    txtJuly.Text = Session["Jul"].ToString();
                //if (Session["Aug"] != null)
                //    txtAugust.Text = Session["Aug"].ToString();
                //if (Session["Sep"] != null)
                //    txtSeptember.Text = Session["Sep"].ToString();

                if (Session["JulF"] != null)
                    txtJulyFormula.Text = Session["JulF"].ToString();
                if (Session["AugF"] != null)
                    txtAugustFormula.Text = Session["AugF"].ToString();
                if (Session["SepF"] != null)
                    txtSeptemberFormula.Text = Session["SepF"].ToString();

                if (Session["Q4Target"] != null)
                    txtQ4Target.Text = Session["Q4Target"].ToString();

                //// Q3_ Actual Months Wise Sessions
                //if (Session["Oct"] != null)
                //    txtOctober.Text = Session["Oct"].ToString();
                //if (Session["Nov"] != null)
                //    txtNovember.Text = Session["Nov"].ToString();
                //if (Session["Dec"] != null)
                //    txtDecember.Text = Session["Dec"].ToString();

                if (Session["OctF"] != null)
                    txtOctoberFormula.Text = Session["OctF"].ToString();
                if (Session["NovF"] != null)
                    txtNovemberFormula.Text = Session["NovF"].ToString();
                if (Session["DecF"] != null)
                    txtDecemberFormula.Text = Session["DecF"].ToString();

                if (Session["TotalTarget"] != null)
                    txtTotalTarget.Text = Session["TotalTarget"].ToString();
                if (Session["Total_Actual"] != null)
                    txtTotal_Actual.Text = Session["Total_Actual"].ToString();
                if (Session["Comments"] != null)
                    txtComments.Text = Session["Comments"].ToString();
            }
        }

        protected void RadGrid1_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem item = (GridDataItem)e.Item;
            string tid = item.OwnerTableView.DataKeyValues[item.ItemIndex]["Target_Actual_PK"].ToString();
            try
            {
                objBLL = new MetisBLL();
                string returnMessage = "";
                objBLL.deleteTarget_Actual(Convert.ToInt32(tid), out returnMessage);
                BindData();
            }
            catch (Exception ex)
            {
                RadGrid1.Controls.Add(new LiteralControl("Unable To Delete" + ex.Message));
            }

        }

        protected void RadGrid1_InsertCommand(object sender, GridCommandEventArgs e)
        {

        }

        protected void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {

                RadComboBox CN = (e.Item as GridEditableItem)["CustomerName"].FindControl("dropCustomerName") as RadComboBox;
                DropDownList pN = (e.Item as GridEditableItem)["ProjectName"].FindControl("dropProjectName") as DropDownList;
                DropDownList TY = (e.Item as GridEditableItem)["Consultant"].FindControl("dropConsultant") as DropDownList;
                DropDownList Services = (e.Item as GridEditableItem)["Services"].FindControl("dropService") as DropDownList;
                DropDownList PD = (e.Item as GridEditableItem)["TimeYear"].FindControl("dropTimeYear") as DropDownList;

                RadNumericTextBox txtQ1Target = (e.Item as GridEditableItem)["Q1Target"].FindControl("txtQ1Target") as RadNumericTextBox;
                txtQ1Target.Text = "0";

                //MonthWise Q1
                // RadNumericTextBox txtJanuary = (e.Item as GridEditableItem)["Q1Actual"].FindControl("txtJanuary") as RadNumericTextBox;
                // if (Session["Jan"] == null)
                // txtJanuary.Text = "0";
                // else
                // txtJanuary.Text = Session["Jan"].ToString();

                //RadNumericTextBox txtFebruary = (e.Item as GridEditableItem)["Q1Actual"].FindControl("txtFebruary") as RadNumericTextBox;
                //if (Session["Feb"] == null)
                //    txtFebruary.Text = "0";
                //else
                //    txtFebruary.Text = Session["Feb"].ToString();

                //RadNumericTextBox txtMarch = (e.Item as GridEditableItem)["Q1Actual"].FindControl("txtMarch") as RadNumericTextBox;
                //if (Session["Mar"] == null)
                //    txtMarch.Text = "0";
                //else
                //    txtMarch.Text = Session["Mar"].ToString();                

                //MonthWise Q1 Formula
                TextBox txtJanuaryFormula = (e.Item as GridEditableItem)["Q1ActualFormula"].FindControl("txtJanuaryFormula") as TextBox;
                if (Session["JanF"] == null)
                    txtJanuaryFormula.Text = "0";
                else
                    txtJanuaryFormula.Text = Session["JanF"].ToString();

                TextBox txtFebruaryFormula = (e.Item as GridEditableItem)["Q1ActualFormula"].FindControl("txtFebruaryFormula") as TextBox;
                if (Session["FebF"] == null)
                    txtFebruaryFormula.Text = "0";
                else
                    txtFebruaryFormula.Text = Session["FebF"].ToString();

                TextBox txtMarchFormula = (e.Item as GridEditableItem)["Q1ActualFormula"].FindControl("txtMarchFormula") as TextBox;
                if (Session["MarF"] == null)
                    txtMarchFormula.Text = "0";
                else
                    txtMarchFormula.Text = Session["MarF"].ToString();


                RadNumericTextBox txtQ2Target = (e.Item as GridEditableItem)["Q2Target"].FindControl("txtQ2Target") as RadNumericTextBox;
                txtQ2Target.Text = "0";

                ////MonthWise Q2
                //RadNumericTextBox txtApril = (e.Item as GridEditableItem)["Q2Actual"].FindControl("txtApril") as RadNumericTextBox;
                //if (Session["Apr"] == null)
                //    txtApril.Text = "0";
                //else
                //    txtApril.Text = Session["Apr"].ToString();
                //RadNumericTextBox txtMay = (e.Item as GridEditableItem)["Q2Actual"].FindControl("txtMay") as RadNumericTextBox;
                //if (Session["May"] == null)
                //    txtMay.Text = "0";
                //else
                //    txtMay.Text = Session["May"].ToString();
                //RadNumericTextBox txtJune = (e.Item as GridEditableItem)["Q2Actual"].FindControl("txtJune") as RadNumericTextBox;
                //if (Session["Jun"] == null)
                //    txtJune.Text = "0";
                //else
                //    txtJune.Text = Session["Jun"].ToString();

                //MonthWise Q2 Formula
                TextBox txtAprilFormula = (e.Item as GridEditableItem)["Q2ActualFormula"].FindControl("txtAprilFormula") as TextBox;
                if (Session["AprF"] == null)
                    txtAprilFormula.Text = "0";
                else
                    txtAprilFormula.Text = Session["AprF"].ToString();

                TextBox txtMayFormula = (e.Item as GridEditableItem)["Q2ActualFormula"].FindControl("txtMayFormula") as TextBox;
                if (Session["MayF"] == null)
                    txtMayFormula.Text = "0";
                else
                    txtMayFormula.Text = Session["MayF"].ToString();

                TextBox txtJuneFormula = (e.Item as GridEditableItem)["Q2ActualFormula"].FindControl("txtJuneFormula") as TextBox;
                if (Session["JunF"] == null)
                    txtJuneFormula.Text = "0";
                else
                    txtJuneFormula.Text = Session["JunF"].ToString();


                RadNumericTextBox txtQ3Target = (e.Item as GridEditableItem)["Q3Target"].FindControl("txtQ3Target") as RadNumericTextBox;
                txtQ3Target.Text = "0";

                ////MonthWise Q3
                //RadNumericTextBox txtJuly = (e.Item as GridEditableItem)["Q3Actual"].FindControl("txtJuly") as RadNumericTextBox;
                //if (Session["Jul"] == null)
                //    txtJuly.Text = "0";
                //else
                //    txtJuly.Text = Session["Jul"].ToString();
                //RadNumericTextBox txtAugust = (e.Item as GridEditableItem)["Q3Actual"].FindControl("txtAugust") as RadNumericTextBox;
                //if (Session["Aug"] == null)
                //    txtAugust.Text = "0";
                //else
                //    txtAugust.Text = Session["Aug"].ToString();
                //RadNumericTextBox txtSeptember = (e.Item as GridEditableItem)["Q3Actual"].FindControl("txtSeptember") as RadNumericTextBox;
                //if (Session["Sep"] == null)
                //    txtSeptember.Text = "0";
                //else
                //    txtSeptember.Text = Session["Sep"].ToString();
                //MonthWise Q3
                TextBox txtJulyFormula = (e.Item as GridEditableItem)["Q3ActualFormula"].FindControl("txtJulyFormula") as TextBox;
                if (Session["JulF"] == null)
                    txtJulyFormula.Text = "0";
                else
                    txtJulyFormula.Text = Session["JulF"].ToString();
                TextBox txtAugustFormula = (e.Item as GridEditableItem)["Q3ActualFormula"].FindControl("txtAugustFormula") as TextBox;
                if (Session["AugF"] == null)
                    txtAugustFormula.Text = "0";
                else
                    txtAugustFormula.Text = Session["AugF"].ToString();
                TextBox txtSeptemberFormula = (e.Item as GridEditableItem)["Q3ActualFormula"].FindControl("txtSeptemberFormula") as TextBox;
                if (Session["SepF"] == null)
                    txtSeptemberFormula.Text = "0";
                else
                    txtSeptemberFormula.Text = Session["SepF"].ToString();

                RadNumericTextBox txtQ4Target = (e.Item as GridEditableItem)["Q4Target"].FindControl("txtQ4Target") as RadNumericTextBox;
                txtQ4Target.Text = "0";

                ////MonthWise Q4
                //RadNumericTextBox txtOctober = (e.Item as GridEditableItem)["Q4Actual"].FindControl("txtOctober") as RadNumericTextBox;
                //if (Session["Oct"] == null)
                //    txtOctober.Text = "0";
                //else
                //    txtOctober.Text = Session["Oct"].ToString();
                //RadNumericTextBox txtNovember = (e.Item as GridEditableItem)["Q4Actual"].FindControl("txtNovember") as RadNumericTextBox;
                //if (Session["Nov"] == null)
                //    txtNovember.Text = "0";
                //else
                //    txtNovember.Text = Session["Nov"].ToString();
                //RadNumericTextBox txtDecember = (e.Item as GridEditableItem)["Q4Actual"].FindControl("txtDecember") as RadNumericTextBox;
                //if (Session["Dec"] == null)
                //    txtDecember.Text = "0";
                //else
                //    txtDecember.Text = Session["Dec"].ToString();
                //MonthWise Q4
                TextBox txtOctoberFormula = (e.Item as GridEditableItem)["Q4ActualFormula"].FindControl("txtOctoberFormula") as TextBox;
                if (Session["OctF"] == null)
                    txtOctoberFormula.Text = "0";
                else
                    txtOctoberFormula.Text = Session["OctF"].ToString();
                TextBox txtNovemberFormula = (e.Item as GridEditableItem)["Q4ActualFormula"].FindControl("txtNovemberFormula") as TextBox;
                if (Session["NovF"] == null)
                    txtNovemberFormula.Text = "0";
                else
                    txtNovemberFormula.Text = Session["NovF"].ToString();

                TextBox txtDecemberFormula = (e.Item as GridEditableItem)["Q4ActualFormula"].FindControl("txtDecemberFormula") as TextBox;
                if (Session["DecF"] == null)
                    txtDecemberFormula.Text = "0";
                else
                    txtDecemberFormula.Text = Session["DecF"].ToString();


                RadNumericTextBox txtTotalTarget = (e.Item as GridEditableItem)["TotalTarget"].FindControl("txtTotalTarget") as RadNumericTextBox;
                txtTotalTarget.Text = "0";

                RadNumericTextBox txtTotal_Actual = (e.Item as GridEditableItem)["Total_Actual"].FindControl("txtTotal_Actual") as RadNumericTextBox;
                txtTotal_Actual.Text = "0";

                TextBox txtComments = (e.Item as GridEditableItem)["Comments"].FindControl("txtComments") as TextBox;
                txtComments.Text = "";
            }
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("PerformInsert"))
            {
                RadComboBox dropCustomerName = (RadComboBox)e.Item.FindControl("dropCustomerName");
                DropDownList dropProjectName = (DropDownList)e.Item.FindControl("dropProjectName");
                DropDownList dropConsultant = (DropDownList)e.Item.FindControl("dropConsultant");
                DropDownList dropService = (DropDownList)e.Item.FindControl("dropService");
                DropDownList dropTimeYear = (DropDownList)e.Item.FindControl("dropTimeYear");
                RadNumericTextBox txtQ1Target = (RadNumericTextBox)e.Item.FindControl("txtQ1Target");

                // RadNumericTextBox txtJanuary = (RadNumericTextBox)e.Item.FindControl("txtJanuary");
                // RadNumericTextBox txtFebruary = (RadNumericTextBox)e.Item.FindControl("txtFebruary");
                // RadNumericTextBox txtMarch = (RadNumericTextBox)e.Item.FindControl("txtMarch");

                TextBox txtJanuaryFormula = (TextBox)e.Item.FindControl("txtJanuaryFormula");
                TextBox txtFebruaryFormula = (TextBox)e.Item.FindControl("txtFebruaryFormula");
                TextBox txtMarchFormula = (TextBox)e.Item.FindControl("txtMarchFormula");

                RadNumericTextBox txtQ2Target = (RadNumericTextBox)e.Item.FindControl("txtQ2Target");

                //RadNumericTextBox txtApril = (RadNumericTextBox)e.Item.FindControl("txtApril");
                //RadNumericTextBox txtMay = (RadNumericTextBox)e.Item.FindControl("txtMay");
                //RadNumericTextBox txtJune = (RadNumericTextBox)e.Item.FindControl("txtJune");

                TextBox txtAprilFormula = (TextBox)e.Item.FindControl("txtAprilFormula");
                TextBox txtMayFormula = (TextBox)e.Item.FindControl("txtMayFormula");
                TextBox txtJuneFormula = (TextBox)e.Item.FindControl("txtJuneFormula");

                RadNumericTextBox txtQ3Target = (RadNumericTextBox)e.Item.FindControl("txtQ3Target");

                //RadNumericTextBox txtJuly = (RadNumericTextBox)e.Item.FindControl("txtJuly");
                //RadNumericTextBox txtAugust = (RadNumericTextBox)e.Item.FindControl("txtAugust");
                //RadNumericTextBox txtSeptember = (RadNumericTextBox)e.Item.FindControl("txtSeptember");
                TextBox txtJulyFormula = (TextBox)e.Item.FindControl("txtJulyFormula");
                TextBox txtAugustFormula = (TextBox)e.Item.FindControl("txtAugustFormula");
                TextBox txtSeptemberFormula = (TextBox)e.Item.FindControl("txtSeptemberFormula");

                RadNumericTextBox txtQ4Target = (RadNumericTextBox)e.Item.FindControl("txtQ4Target");

                //RadNumericTextBox txtOctober = (RadNumericTextBox)e.Item.FindControl("txtOctober");
                //RadNumericTextBox txtNovember = (RadNumericTextBox)e.Item.FindControl("txtNovember");
                //RadNumericTextBox txtDecember = (RadNumericTextBox)e.Item.FindControl("txtDecember");
                TextBox txtOctoberFormula = (TextBox)e.Item.FindControl("txtOctoberFormula");
                TextBox txtNovemberFormula = (TextBox)e.Item.FindControl("txtNovemberFormula");
                TextBox txtDecemberFormula = (TextBox)e.Item.FindControl("txtDecemberFormula");


                TextBox txtComments = (TextBox)e.Item.FindControl("txtComments");
                objBLL = new MetisBLL();
                string returnMessage = "";
                objBLL.insertTarget_Actual(Convert.ToInt32(dropCustomerName.SelectedValue), Convert.ToInt32(dropProjectName.SelectedValue),
                    Convert.ToInt32(dropConsultant.SelectedValue), Convert.ToInt32(dropService.SelectedValue), Convert.ToInt32(dropTimeYear.SelectedValue),
                    float.Parse(txtQ1Target.Text),
                    objBLL.converting_To_Float((txtJanuaryFormula.Text).ToString()), objBLL.converting_To_Float((txtFebruaryFormula.Text).ToString()), objBLL.converting_To_Float((txtMarchFormula.Text).ToString()),
                    Convert.ToString(txtJanuaryFormula.Text), Convert.ToString(txtFebruaryFormula.Text), Convert.ToString(txtMarchFormula.Text),
                    float.Parse(txtQ2Target.Text),
                    objBLL.converting_To_Float((txtAprilFormula.Text).ToString()), objBLL.converting_To_Float((txtMayFormula.Text).ToString()), objBLL.converting_To_Float((txtJuneFormula.Text).ToString()),
                    Convert.ToString(txtAprilFormula.Text), Convert.ToString(txtMayFormula.Text), Convert.ToString(txtJuneFormula.Text),
                    float.Parse(txtQ3Target.Text),
                    objBLL.converting_To_Float((txtJulyFormula.Text).ToString()), objBLL.converting_To_Float((txtAugustFormula.Text).ToString()), objBLL.converting_To_Float((txtSeptemberFormula.Text).ToString()),
                    Convert.ToString(txtJulyFormula.Text), Convert.ToString(txtAugustFormula.Text), Convert.ToString(txtSeptemberFormula.Text),
                    float.Parse(txtQ4Target.Text),
                    objBLL.converting_To_Float((txtOctoberFormula.Text).ToString()), objBLL.converting_To_Float((txtNovemberFormula.Text).ToString()), objBLL.converting_To_Float((txtDecemberFormula.Text).ToString()),
                    Convert.ToString(txtOctoberFormula.Text), Convert.ToString(txtNovemberFormula.Text), Convert.ToString(txtDecemberFormula.Text),
                    Convert.ToString(txtComments.Text), out returnMessage);
                BindData();

            }

            if (e.CommandName.Equals("Edit"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                string CustomerName = ((DataBoundLiteralControl)item.Controls[2].Controls[0]).Text.Trim();
                string ProjectName = ((DataBoundLiteralControl)item.Controls[3].Controls[0]).Text.Trim();
                string Consultant = ((DataBoundLiteralControl)item.Controls[4].Controls[0]).Text.Trim();
                string Services = ((DataBoundLiteralControl)item.Controls[5].Controls[0]).Text.Trim();
                string TimeYear = ((DataBoundLiteralControl)item.Controls[6].Controls[0]).Text.Trim();
                //  string Q1Target = ((LiteralControl)item.Controls[7].Controls[0]).Text.Trim();
                string Q1Target = ((RadNumericTextBox)item.Controls[7].Controls[1]).Text;
                // string Q1Actual = ((LiteralControl)item.Controls[8].Controls[0]).Text.Trim();
                string Q1ActualFormula = ((LiteralControl)item.Controls[8].Controls[0]).Text.Trim();
                //Label lblJan = (Label)e.Item.FindControl("lblJan");
                //string lblJanuary = lblJan.Text;
                //Label lblFeb = (Label)e.Item.FindControl("lblFeb");
                //string lblFebruary = lblFeb.Text;
                //Label lblMar = (Label)e.Item.FindControl("lblMar");

                Label lblJanF = (Label)e.Item.FindControl("lblJanF");
                Label lblFebF = (Label)e.Item.FindControl("lblFebF");
                Label lblMarF = (Label)e.Item.FindControl("lblMarF");

                // Label lblApr = (Label)e.Item.FindControl("lblApr");
                // Label lblMay = (Label)e.Item.FindControl("lblMay");
                // Label lblJun = (Label)e.Item.FindControl("lblJun");
                Label lblAprF = (Label)e.Item.FindControl("lblAprF");
                Label lblMayF = (Label)e.Item.FindControl("lblMayF");
                Label lblJunF = (Label)e.Item.FindControl("lblJunF");

                //Label lblJul = (Label)e.Item.FindControl("lblJul");
                //Label lblAug = (Label)e.Item.FindControl("lblAug");
                //Label lblSep = (Label)e.Item.FindControl("lblSep");
                //Label lblOct = (Label)e.Item.FindControl("lblOct");
                //Label lblNov = (Label)e.Item.FindControl("lblNov");
                //Label lblDec = (Label)e.Item.FindControl("lblDec");
                Label lblJulF = (Label)e.Item.FindControl("lblJulF");
                Label lblAugF = (Label)e.Item.FindControl("lblAugF");
                Label lblSepF = (Label)e.Item.FindControl("lblSepF");
                Label lblOctF = (Label)e.Item.FindControl("lblOctF");
                Label lblNovF = (Label)e.Item.FindControl("lblNovF");
                Label lblDecF = (Label)e.Item.FindControl("lblDecF");
                string Q2Target = ((RadNumericTextBox)item.Controls[9].Controls[1]).Text.Trim();
                string Q2ActualFormula = ((LiteralControl)item.Controls[10].Controls[0]).Text.Trim();
                string Q3Target = ((RadNumericTextBox)item.Controls[11].Controls[1]).Text.Trim();
                string Q3ActualFormula = ((LiteralControl)item.Controls[12].Controls[0]).Text.Trim();
                string Q4Target = ((RadNumericTextBox)item.Controls[13].Controls[1]).Text.Trim();
                string Q4ActualFormula = ((LiteralControl)item.Controls[14].Controls[0]).Text.Trim();
                string Comments = ((DataBoundLiteralControl)item.Controls[17].Controls[0]).Text.Trim();
                Session["Target_Actual_PK"] = item.OwnerTableView.DataKeyValues[item.ItemIndex]["Target_Actual_PK"].ToString();
                Session["CustomerName"] = CustomerName;
                Session["ProjectName"] = ProjectName;
                Session["Consultant"] = Consultant;
                Session["Services"] = Services;
                Session["TimeYear"] = TimeYear;
                Session["Q1Target"] = Q1Target;
                // Session["Q1Actual"] = Q1Actual;
                Session["Q1ActualFormula"] = Q1ActualFormula;
                Session["Q2Target"] = Q2Target;
                Session["Q2ActualFormula"] = Q2ActualFormula;
                Session["Q3Target"] = Q3Target;
                Session["Q3ActualFormula"] = Q3ActualFormula;
                Session["Q4Target"] = Q4Target;
                Session["Q4ActualFormula"] = Q4ActualFormula;
                Session["Comments"] = Comments;
                //Session["Jan"] = lblJanuary;
                //Session["Feb"] = lblFeb.Text;
                //Session["Mar"] = lblMar.Text;
                Session["JanF"] = lblJanF.Text;
                Session["FebF"] = lblFebF.Text;
                Session["MarF"] = lblMarF.Text;
                // Session["Apr"] = lblApr.Text;
                // Session["May"] = lblMay.Text;
                // Session["Jun"] = lblJun.Text;
                Session["AprF"] = lblAprF.Text;
                Session["MayF"] = lblMayF.Text;
                Session["JunF"] = lblJunF.Text;
                //Session["Jul"] = lblJul.Text;
                //Session["Aug"] = lblAug.Text;
                //Session["Sep"] = lblSep.Text;
                //Session["Oct"] = lblOct.Text;
                //Session["Nov"] = lblNov.Text;
                //Session["Dec"] = lblDec.Text;
                Session["JulF"] = lblJulF.Text;
                Session["AugF"] = lblAugF.Text;
                Session["SepF"] = lblSepF.Text;
                Session["OctF"] = lblOctF.Text;
                Session["NovF"] = lblNovF.Text;
                Session["DecF"] = lblDecF.Text;


                BindData();


            }

            if (e.CommandName.Equals("Update"))
            {
                RadComboBox dropCustomerName = (RadComboBox)e.Item.FindControl("dropCustomerName");
                DropDownList dropProjectName = (DropDownList)e.Item.FindControl("dropProjectName");
                DropDownList dropConsultant = (DropDownList)e.Item.FindControl("dropConsultant");
                DropDownList dropService = (DropDownList)e.Item.FindControl("dropService");
                DropDownList dropTimeYear = (DropDownList)e.Item.FindControl("dropTimeYear");
                RadNumericTextBox txtQ1Target = (RadNumericTextBox)e.Item.FindControl("txtQ1Target");

                // RadNumericTextBox txtJanuary = (RadNumericTextBox)e.Item.FindControl("txtJanuary");
                // RadNumericTextBox txtFebruary = (RadNumericTextBox)e.Item.FindControl("txtFebruary");
                // RadNumericTextBox txtMarch = (RadNumericTextBox)e.Item.FindControl("txtMarch");


                TextBox txtJanuaryFormula = (TextBox)e.Item.FindControl("txtJanuaryFormula");
                TextBox txtFebruaryFormula = (TextBox)e.Item.FindControl("txtFebruaryFormula");
                TextBox txtMarchFormula = (TextBox)e.Item.FindControl("txtMarchFormula");

                RadNumericTextBox txtQ2Target = (RadNumericTextBox)e.Item.FindControl("txtQ2Target");

                // RadNumericTextBox txtApril = (RadNumericTextBox)e.Item.FindControl("txtApril");
                // RadNumericTextBox txtMay = (RadNumericTextBox)e.Item.FindControl("txtMay");
                // RadNumericTextBox txtJune = (RadNumericTextBox)e.Item.FindControl("txtJune");

                TextBox txtAprilFormula = (TextBox)e.Item.FindControl("txtAprilFormula");
                TextBox txtMayFormula = (TextBox)e.Item.FindControl("txtMayFormula");
                TextBox txtJuneFormula = (TextBox)e.Item.FindControl("txtJuneFormula");

                RadNumericTextBox txtQ3Target = (RadNumericTextBox)e.Item.FindControl("txtQ3Target");

                //RadNumericTextBox txtJuly = (RadNumericTextBox)e.Item.FindControl("txtJuly");
                //RadNumericTextBox txtAugust = (RadNumericTextBox)e.Item.FindControl("txtAugust");
                //RadNumericTextBox txtSeptember = (RadNumericTextBox)e.Item.FindControl("txtSeptember");
                TextBox txtJulyFormula = (TextBox)e.Item.FindControl("txtJulyFormula");
                TextBox txtAugustFormula = (TextBox)e.Item.FindControl("txtAugustFormula");
                TextBox txtSeptemberFormula = (TextBox)e.Item.FindControl("txtSeptemberFormula");


                RadNumericTextBox txtQ4Target = (RadNumericTextBox)e.Item.FindControl("txtQ4Target");

                //RadNumericTextBox txtOctober = (RadNumericTextBox)e.Item.FindControl("txtOctober");
                //RadNumericTextBox txtNovember = (RadNumericTextBox)e.Item.FindControl("txtNovember");
                //RadNumericTextBox txtDecember = (RadNumericTextBox)e.Item.FindControl("txtDecember");
                TextBox txtOctoberFormula = (TextBox)e.Item.FindControl("txtOctoberFormula");
                TextBox txtNovemberFormula = (TextBox)e.Item.FindControl("txtNovemberFormula");
                TextBox txtDecemberFormula = (TextBox)e.Item.FindControl("txtDecemberFormula");


                TextBox txtComments = (TextBox)e.Item.FindControl("txtComments");
                objBLL = new MetisBLL();
                string returnMessage = "";

                objBLL.updateTarget_Actual(Convert.ToInt32((e.Item as GridDataItem).GetDataKeyValue("Target_Actual_PK")), Convert.ToInt32(dropCustomerName.SelectedValue.ToString()),
                 Convert.ToInt32(dropProjectName.SelectedValue.ToString()),
                 Convert.ToInt32(dropConsultant.SelectedValue.ToString()),
                 Convert.ToInt32(dropService.SelectedValue.ToString()),
                 Convert.ToInt32(dropTimeYear.SelectedValue.ToString()),
                 float.Parse(txtQ1Target.Text),
                 objBLL.converting_To_Float((txtJanuaryFormula.Text).ToString()), objBLL.converting_To_Float((txtFebruaryFormula.Text).ToString()), objBLL.converting_To_Float((txtMarchFormula.Text).ToString()),
                 Convert.ToString(txtJanuaryFormula.Text), Convert.ToString(txtFebruaryFormula.Text), Convert.ToString(txtMarchFormula.Text),
                 float.Parse(txtQ2Target.Text),
                 objBLL.converting_To_Float((txtAprilFormula.Text).ToString()), objBLL.converting_To_Float((txtMayFormula.Text).ToString()), objBLL.converting_To_Float((txtJuneFormula.Text).ToString()),
                 Convert.ToString(txtAprilFormula.Text), Convert.ToString(txtMayFormula.Text), Convert.ToString(txtJuneFormula.Text),
                 float.Parse(txtQ3Target.Text),
                 objBLL.converting_To_Float((txtJulyFormula.Text).ToString()), objBLL.converting_To_Float((txtAugustFormula.Text).ToString()), objBLL.converting_To_Float((txtSeptemberFormula.Text).ToString()),
                 Convert.ToString(txtJulyFormula.Text), Convert.ToString(txtAugustFormula.Text), Convert.ToString(txtSeptemberFormula.Text),
                 float.Parse(txtQ4Target.Text),
                 objBLL.converting_To_Float((txtOctoberFormula.Text).ToString()), objBLL.converting_To_Float((txtNovemberFormula.Text).ToString()), objBLL.converting_To_Float((txtDecemberFormula.Text).ToString()),
                 Convert.ToString(txtOctoberFormula.Text), Convert.ToString(txtNovemberFormula.Text), Convert.ToString(txtDecemberFormula.Text),
                 Convert.ToString(txtComments.Text),
                 out  returnMessage);

                Session["Target_Actual_PK"] = null;
                Session["CustomerName"] = null;
                Session["ProjectName"] = null;
                Session["Consultant"] = null;
                Session["Services"] = null;
                Session["TimeYear"] = null;
                Session["Q1Target"] = null;
                // Session["Q1Actual"] = null;
                Session["Q1ActualFormula"] = null;
                Session["Q2Target"] = null;
                Session["Q2ActualFormula"] = null;
                Session["Q3Target"] = null;
                Session["Q3ActualFormula"] = null;
                Session["Q4Target"] = null;
                Session["Q4ActualFormula"] = null;
                Session["Comments"] = null;
                // Session["Jan"] = null;
                // Session["Feb"] = null;
                // Session["Mar"] = null;
                Session["JanF"] = null;
                Session["FebF"] = null;
                Session["MarF"] = null;
                // Session["Apr"] = null;
                // Session["May"] = null;
                // Session["Jun"] = null;
                Session["AprF"] = null;
                Session["MayF"] = null;
                //Session["JunF"] = null;
                //Session["Jul"] = null;
                //Session["Aug"] = null;
                //Session["Sep"] = null;
                //Session["Oct"] = null;
                //Session["Nov"] = null;
                //Session["Dec"] = null;
                Session["JulF"] = null;
                Session["AugF"] = null;
                Session["SepF"] = null;
                Session["OctF"] = null;
                Session["NovF"] = null;
                Session["DecF"] = null;
                BindData();
            }
            if (e.CommandName.Equals("Cancel"))
            {
                Session["Target_Actual_PK"] = null;
                Session["CustomerName"] = null;
                Session["ProjectName"] = null;
                Session["Consultant"] = null;
                Session["Services"] = null;
                Session["TimeYear"] = null;
                Session["Q1Target"] = null;
                // Session["Q1Actual"] = null;
                Session["Q1ActualFormula"] = null;
                Session["Q2Target"] = null;
                Session["Q2ActualFormula"] = null;
                Session["Q3Target"] = null;
                Session["Q3ActualFormula"] = null;
                Session["Q4Target"] = null;
                Session["Q4ActualFormula"] = null;
                Session["Comments"] = null;
                // Session["Jan"] = null;
                // Session["Feb"] = null;
                // Session["Mar"] = null;
                Session["JanF"] = null;
                Session["FebF"] = null;
                Session["MarF"] = null;
                //Session["Apr"] = null;
                //Session["May"] = null;
                //Session["Jun"] = null;
                Session["AprF"] = null;
                Session["MayF"] = null;
                Session["JunF"] = null;
                //Session["Jul"] = null;
                //Session["Aug"] = null;
                //Session["Sep"] = null;
                //Session["Oct"] = null;
                //Session["Nov"] = null;
                //Session["Dec"] = null;
                Session["JulF"] = null;
                Session["AugF"] = null;
                Session["SepF"] = null;
                Session["OctF"] = null;
                Session["NovF"] = null;
                Session["DecF"] = null;

                BindData();
            }

        }

        
        #region ToBeChecked

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

        #endregion

        
    }
}




