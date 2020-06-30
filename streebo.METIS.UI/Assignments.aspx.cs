using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Telerik.Web.UI.Skins;
using streebo.METIS.BLL;
using System.Collections;
using System.Data;
using System.Globalization;

namespace streebo.METIS.UI
{
    public partial class Assignments : System.Web.UI.Page
    {
        private MetisBLL objBLL;
        private readonly DepartmentManager depManager = DepartmentManager.Instance;
        private readonly RolesManager rolesManager = RolesManager.Instance;
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
                    if (row["EntityName"].ToString() == "Assignment") { b_CanView = Convert.ToBoolean(row["Can_View"]); }
                 }
                 //If Admin thn bypass security
                 if(b_CanView == false)
                 {
                 objBLL = new MetisBLL();
                 if(Convert.ToBoolean(objBLL.IsAdmin(Convert.ToString(Session["user"]))))
                     b_CanView = true;
                 }
                 if (b_CanView)
                 {
                     divBulkAssignment.Visible = true;
                     divDepartment.Visible = false;
                     divResourceInDepartment.Visible = false;
                     divResourceOnProjects.Visible = false;
                     divResourceLeaves.Visible = false;
                     divUpComingProjects.Visible = false;
                     divAssignmentHistory.Visible = true;

                    
                 }
                 else
                 {
                     Response.Redirect("Login.aspx");
                 }
            }
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

        protected void ddlSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            objBLL = new MetisBLL();
            switch (ddlSelection.SelectedValue)
            {
                case "BulkAssignment":
                    divBulkAssignment.Visible = true;
                    divDepartment.Visible = false;
                    divResourceInDepartment.Visible = false;
                    divResourceLeaves.Visible = false;
                    divResourceOnProjects.Visible = false;
                    divUpComingProjects.Visible = false;
                    CheckBox1.Visible = true;
                    divRoles.Visible = false;
                    //rgBulkAssignment.DataSource = objBLL.getAllBulkAssignments(Session["user"].ToString());
                    //rgBulkAssignment.DataBind();
                    break;
                case "Departments":
                    divDepartment.Visible = true;
                    divBulkAssignment.Visible = false;
                    divResourceInDepartment.Visible = false;
                    divResourceOnProjects.Visible = false;
                    divResourceLeaves.Visible = false;
                    divUpComingProjects.Visible = false;
                    CheckBox1.Visible = false;
                    divRoles.Visible = false;
                    rgDepartments.DataSource = depManager.getDeparments();
                    rgDepartments.DataBind();
                    break;
                case "Assign Department":
                    divBulkAssignment.Visible = false;
                    divDepartment.Visible = false;
                    divResourceInDepartment.Visible = true;
                    divResourceOnProjects.Visible = false;
                    divUpComingProjects.Visible = false;
                    CheckBox1.Visible = false;
                    divRoles.Visible = false;
                    rgResourceInDepartment.DataSource = objBLL.getAllResourceAssociations();
                    rgResourceInDepartment.DataBind();
                    break;
                case "Assign Resource To Project":
                    divResourceOnProjects.Visible = true;
                    divBulkAssignment.Visible = false;
                    divDepartment.Visible = false;
                    divResourceInDepartment.Visible = false;
                    divResourceLeaves.Visible = false;
                    divUpComingProjects.Visible = false;
                    CheckBox1.Visible = false;
                    divRoles.Visible = false;
                    rgResourceOnProjects.DataSource = objBLL.getAllResourceAssignments(Session["user"].ToString());
                    rgResourceOnProjects.DataBind();
                    break;
                case "Mark Leaves":
                    divResourceLeaves.Visible = true;
                    divBulkAssignment.Visible = false;
                    divDepartment.Visible = false;
                    divResourceInDepartment.Visible = false;
                    divResourceOnProjects.Visible = false;
                    divUpComingProjects.Visible = false;
                    CheckBox1.Visible = false;
                    divRoles.Visible = false;
                    rgResourceLeaves.DataSource = objBLL.getAllResourceLeaves();
                    rgResourceLeaves.DataBind();
                    break;
                case "UpComing Projects":
                    divUpComingProjects.Visible = true;
                    divBulkAssignment.Visible = false;
                    divDepartment.Visible = false;
                    divResourceInDepartment.Visible = false;
                    divResourceOnProjects.Visible = false;
                    divResourceLeaves.Visible = false;
                    CheckBox1.Visible = false;
                    divRoles.Visible = false;
                    rgUpComingProject.DataSource = objBLL.getAllUpComingProject();
                    rgUpComingProject.DataBind();
                    break;
                case "Roles":
                    divRoles.Visible = true;
                    divUpComingProjects.Visible = false;
                    divBulkAssignment.Visible = false;
                    divDepartment.Visible = false;
                    divResourceInDepartment.Visible = false;
                    divResourceOnProjects.Visible = false;
                    divResourceLeaves.Visible = false;
                    CheckBox1.Visible = false;
                    rgRoles.DataSource = objBLL.getAllUpComingProject();
                    rgRoles.DataBind();
                    break;
                case "Add Project":
                   
                    break;
                case "Add Resource":
                   
                    break;
                default:
                    break;
            }
        }

        #region Roles
        protected void rgRoles_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            rgDepartments.DataSource = rolesManager.GetAllRoles();
        }
        protected void rgRoles_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridDataItem item = (GridDataItem)e.Item;

            string roleId = item.OwnerTableView.DataKeyValues[item.ItemIndex]["RoleId"].ToString();

            try
            {
                string p_message = "";
                rolesManager.DeleteRoleById(roleId, out p_message);
            }
            catch (Exception ex)
            {
                rgDepartments.Controls.Add(new LiteralControl("Unable to delete Role. Reason: " + ex.Message));
                e.Canceled = true;
            }
        }
        protected void rgRoles_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
        }
        protected void rgRoles_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
        }
        protected void rgRoles_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {

                    DropDownList tb = (e.Item as GridEditableItem)["ReportsTo"].FindControl("comboReportsTo") as DropDownList;
                    tb.Focus();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);
            }
        }
        protected void rgRoles_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem && e.Item.IsInEditMode)
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    TextBox txtDepartmentID = (e.Item as GridEditableItem)["DepartmentID"].FindControl("txtDepartmentID") as TextBox;
                    TextBox txtDepartmentName = (e.Item as GridEditableItem)["DepartmentName"].FindControl("txtDepartmentName") as TextBox;
                    DropDownList comboReportsTo = (e.Item as GridEditableItem)["ReportsTo"].FindControl("comboReportsTo") as DropDownList;
                    // CheckBox chkActive = (e.Item as GridEditableItem)["Active"].FindControl("chkActive") as CheckBox;

                    //objBLL = new MetisBLL();
                    comboReportsTo.DataSource = depManager.getDeparments();
                    comboReportsTo.DataTextField = depManager.getDeparments().Columns[1].ToString();
                    comboReportsTo.DataValueField = depManager.getDeparments().Columns[0].ToString();
                    comboReportsTo.DataBind();
                    comboReportsTo.Width = Unit.Pixel(240); // Set the width  

                    txtDepartmentID.Text = Session["DepartmentID"].ToString();
                    txtDepartmentName.Text = Session["DepartmentName"].ToString();
                    comboReportsTo.Items.FindByText(Session["ReportsTo"].ToString()).Selected = true;
                    // chkActive.Checked = true;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }

        }

        protected void rgRoles_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("PerformInsert"))
            {
                TextBox txtDepartmentID = (TextBox)e.Item.FindControl("txtDepartmentID");
                TextBox txtDepartmentName = (TextBox)e.Item.FindControl("txtDepartmentName");
                DropDownList comboReportsTo = (DropDownList)e.Item.FindControl("comboReportsTo");
                CheckBox chkActive = (CheckBox)e.Item.FindControl("chkActive");
                string p_message = "";
                //objBLL = new MetisBLL();
                depManager.insertDepartment(txtDepartmentName.Text, comboReportsTo.SelectedValue, chkActive.Checked.ToString(), out p_message);

                rgDepartments.DataSource = depManager.getDeparments();
                rgDepartments.DataBind();
            }
            if (e.CommandName.Equals("Edit"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                string DepartmentID = ((DataBoundLiteralControl)item.Controls[3].Controls[0]).Text.Trim();
                string DepartmentName = ((DataBoundLiteralControl)item.Controls[4].Controls[0]).Text.Trim();
                string ReportsTo = ((DataBoundLiteralControl)item.Controls[5].Controls[0]).Text.Trim();
                string Active = ((DataBoundLiteralControl)item.Controls[6].Controls[0]).Text.Trim();
                Session["DepartmentID"] = DepartmentID;
                Session["DepartmentName"] = DepartmentName;
                Session["ReportsTo"] = ReportsTo;
                Session["Active"] = Active;
                //rgDepartments.DataBind();
            }
            if (e.CommandName.Equals("Update"))
            {
                TextBox txtDepartmentID = (TextBox)e.Item.FindControl("txtDepartmentID");
                TextBox txtDepartmentName = (TextBox)e.Item.FindControl("txtDepartmentName");
                DropDownList comboReportsTo = (DropDownList)e.Item.FindControl("comboReportsTo");
                CheckBox chkBox = (CheckBox)e.Item.FindControl("chkActive");
                string p_message = "";
                //objBLL = new MetisBLL();
                depManager.updateDepartment(txtDepartmentID.Text, txtDepartmentName.Text, comboReportsTo.SelectedValue, chkBox.Checked.ToString(), out p_message);

                rgDepartments.DataSource = depManager.getDeparments();
                rgDepartments.DataBind();
            }
            if (e.CommandName.Equals("Cancel"))
            {
                rgDepartments.DataBind();
            }
        }
        #endregion

        #region "Departments"

        protected void rgDepartments_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            rgDepartments.DataSource = depManager.getDeparments();
        }
        protected void rgDepartments_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridDataItem item = (GridDataItem)e.Item;

            string DepartmentID = item.OwnerTableView.DataKeyValues[item.ItemIndex]["DepartmentID"].ToString();

            try
            {
                string p_message = "";
                objBLL = new MetisBLL();
                depManager.deleteDepartment(DepartmentID, out p_message);
            }
            catch (Exception ex)
            {
                rgDepartments.Controls.Add(new LiteralControl("Unable to delete Department. Reason: " + ex.Message));
                e.Canceled = true;
            }
        }
        protected void rgDepartments_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
        }
        protected void rgDepartments_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
        }
        protected void rgDepartments_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {

                    DropDownList tb = (e.Item as GridEditableItem)["ReportsTo"].FindControl("comboReportsTo") as DropDownList;
                    tb.Focus();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);
            }
        }
        protected void rgDepartments_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem && e.Item.IsInEditMode)
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    TextBox txtDepartmentID = (e.Item as GridEditableItem)["DepartmentID"].FindControl("txtDepartmentID") as TextBox;
                    TextBox txtDepartmentName = (e.Item as GridEditableItem)["DepartmentName"].FindControl("txtDepartmentName") as TextBox;
                    DropDownList comboReportsTo = (e.Item as GridEditableItem)["ReportsTo"].FindControl("comboReportsTo") as DropDownList;
                   // CheckBox chkActive = (e.Item as GridEditableItem)["Active"].FindControl("chkActive") as CheckBox;

                    //objBLL = new MetisBLL();
                    comboReportsTo.DataSource = depManager.getDeparments();
                    comboReportsTo.DataTextField = depManager.getDeparments().Columns[1].ToString();
                    comboReportsTo.DataValueField = depManager.getDeparments().Columns[0].ToString();
                    comboReportsTo.DataBind();
                    comboReportsTo.Width = Unit.Pixel(240); // Set the width  

                    txtDepartmentID.Text = Session["DepartmentID"].ToString();
                    txtDepartmentName.Text = Session["DepartmentName"].ToString();
                    comboReportsTo.Items.FindByText(Session["ReportsTo"].ToString()).Selected = true;
                   // chkActive.Checked = true;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }

        }

        protected void rgDepartments_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("PerformInsert"))
            {
                TextBox txtDepartmentID = (TextBox)e.Item.FindControl("txtDepartmentID");
                TextBox txtDepartmentName = (TextBox)e.Item.FindControl("txtDepartmentName");
                DropDownList comboReportsTo = (DropDownList)e.Item.FindControl("comboReportsTo");
                CheckBox chkActive = (CheckBox)e.Item.FindControl("chkActive");
                string p_message = "";
                //objBLL = new MetisBLL();
                depManager.insertDepartment(txtDepartmentName.Text, comboReportsTo.SelectedValue, chkActive.Checked.ToString(), out p_message);

                rgDepartments.DataSource = depManager.getDeparments();
                rgDepartments.DataBind();
            }
            if (e.CommandName.Equals("Edit"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                string DepartmentID = ((DataBoundLiteralControl)item.Controls[3].Controls[0]).Text.Trim();
                string DepartmentName = ((DataBoundLiteralControl)item.Controls[4].Controls[0]).Text.Trim();
                string ReportsTo = ((DataBoundLiteralControl)item.Controls[5].Controls[0]).Text.Trim();
                string Active = ((DataBoundLiteralControl)item.Controls[6].Controls[0]).Text.Trim();
                Session["DepartmentID"] = DepartmentID;
                Session["DepartmentName"] = DepartmentName;
                Session["ReportsTo"] = ReportsTo;
                Session["Active"] = Active;
                //rgDepartments.DataBind();
            }
            if (e.CommandName.Equals("Update"))
            {
                TextBox txtDepartmentID = (TextBox)e.Item.FindControl("txtDepartmentID");
                TextBox txtDepartmentName = (TextBox)e.Item.FindControl("txtDepartmentName");
                DropDownList comboReportsTo = (DropDownList)e.Item.FindControl("comboReportsTo");
                CheckBox chkBox = (CheckBox)e.Item.FindControl("chkActive");
                string p_message = "";
                //objBLL = new MetisBLL();
                depManager.updateDepartment(txtDepartmentID.Text, txtDepartmentName.Text, comboReportsTo.SelectedValue, chkBox.Checked.ToString(), out p_message);

                rgDepartments.DataSource = depManager.getDeparments();
                rgDepartments.DataBind();
            }
            if (e.CommandName.Equals("Cancel"))
            {
                rgDepartments.DataBind();
            }
        }
        #endregion

        #region "Resource In Department"
        protected void rgResourceInDepartment_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {

            objBLL = new MetisBLL();
            rgResourceInDepartment.DataSource = objBLL.getAllResourceAssociations();
        }
        protected void rgResourceInDepartment_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridDataItem item = (GridDataItem)e.Item;

            //string DepartmentID = item.OwnerTableView.DataKeyValues[item.ItemIndex]["DepartmentID"].ToString();

            try
            {
                //string p_message = "";
                //objBLL = new MetisBLL();
                //objBLL.deleteDepartment(DepartmentID, out p_message);
            }
            catch (Exception ex)
            {
                rgResourceInDepartment.Controls.Add(new LiteralControl("Unable to delete Department. Reason: " + ex.Message));
                e.Canceled = true;
            }
        }
        protected void rgResourceInDepartment_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
        }
        protected void rgResourceInDepartment_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
        }
        protected void rgResourceInDepartment_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {
                    DropDownList tb = (e.Item as GridEditableItem)["comboResourceName"].FindControl("comboResourceName") as DropDownList;
                    tb.Focus();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }
        }
        protected void rgResourceInDepartment_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem && e.Item.IsInEditMode)
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    DropDownList comboResourceName = (e.Item as GridEditableItem)["ResourceName"].FindControl("comboResourceName") as DropDownList;
                    objBLL = new MetisBLL();
                    comboResourceName.DataSource = objBLL.getAllResources(Convert.ToString(Session["user"]));
                    comboResourceName.DataTextField = objBLL.getAllResources(Convert.ToString(Session["user"])).Columns[1].ToString();
                    comboResourceName.DataValueField = objBLL.getAllResources(Convert.ToString(Session["user"])).Columns[0].ToString();
                    comboResourceName.DataBind();
                    comboResourceName.Width = Unit.Pixel(240); // Set the width  

                    DropDownList comboDepartmentName = (e.Item as GridEditableItem)["DepartmentName"].FindControl("comboDepartmentName") as DropDownList;
                    objBLL = new MetisBLL();
                    comboDepartmentName.DataSource = depManager.getDeparments();
                    comboDepartmentName.DataTextField = depManager.getDeparments().Columns[1].ToString();
                    comboDepartmentName.DataValueField = depManager.getDeparments().Columns[0].ToString();
                    comboDepartmentName.DataBind();
                    comboDepartmentName.Width = Unit.Pixel(240); // Set the width  

                    comboResourceName.Items.FindByText(Session["ResourceName"].ToString()).Selected = true;
                    comboDepartmentName.Items.FindByText(Session["DepartmentName"].ToString()).Selected = true;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }

        }
        protected void rgResourceInDepartment_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("PerformInsert"))
            {
                DropDownList comboResourceName = (DropDownList)e.Item.FindControl("comboResourceName");
                DropDownList comboDepartmentName = (DropDownList)e.Item.FindControl("comboDepartmentName");

                string p_message = "";
                objBLL = new MetisBLL();
                objBLL.insertResourceAssociation(comboDepartmentName.SelectedValue, comboResourceName.Text, out p_message);

                rgResourceInDepartment.DataSource = objBLL.getAllResourceAssociations();
                rgResourceInDepartment.DataBind();
            }

            if (e.CommandName.Equals("Edit"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                string ResourceName = ((DataBoundLiteralControl)item.Controls[3].Controls[0]).Text.Trim();
                string DepartmentName = ((DataBoundLiteralControl)item.Controls[4].Controls[0]).Text.Trim();
                //Session["DepartmentID"] = DepartmentID;
                Session["ResourceName"] = ResourceName;
                Session["DepartmentName"] = DepartmentName;

                rgResourceInDepartment.DataBind();
            }

            if (e.CommandName.Equals("Update"))
            {
                DropDownList comboResourceName = (DropDownList)e.Item.FindControl("comboResourceName");
                DropDownList comboDepartmentName = (DropDownList)e.Item.FindControl("comboDepartmentName");
                string ResourceAssociationID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ResourceAssociationID"].ToString();
                string p_message = "";
                objBLL = new MetisBLL();
                objBLL.updateResourceAssociation(ResourceAssociationID, comboResourceName.SelectedItem.Text, comboDepartmentName.SelectedItem.Text, out p_message);

                rgResourceInDepartment.DataSource = objBLL.getAllResourceAssociations();
                rgResourceInDepartment.DataBind();
            }
            if (e.CommandName.Equals("Cancel"))
            {
                rgResourceInDepartment.DataBind();
            }
        }
        #endregion

        #region "Resource On Project"

        protected void rgResourceOnProjects_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            objBLL = new MetisBLL();
            rgResourceOnProjects.DataSource = objBLL.getAllResourceAssignments(Session["user"].ToString());
            
        }
        
        protected void rgResourceOnProjects_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridDataItem item = (GridDataItem)e.Item;

            string ResourceAssignmentID = item.OwnerTableView.DataKeyValues[item.ItemIndex]["ResourceAssignmentID"].ToString();

            try
            {
                string p_message = "";
                objBLL = new MetisBLL();
                objBLL.deleteResourceAssignment(ResourceAssignmentID, out p_message);
            }
            catch (Exception ex)
            {
                rgResourceOnProjects.Controls.Add(new LiteralControl("Unable to delete Assignment. Reason: " + ex.Message));
                e.Canceled = true;
            }
        }
        
        protected void rgResourceOnProjects_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
        }
        
        protected void rgResourceOnProjects_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
        }
        
        protected void rgResourceOnProjects_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {
                    //Telerik.Web.UI.RadComboBox comboResourceName = (e.Item as GridEditableItem)["ResourceName"].Controls[1] as Telerik.Web.UI.RadComboBox;
                    DropDownList comboResourceName = (e.Item as GridEditableItem)["ResourceName"].Controls[1] as DropDownList;
                    comboResourceName.Focus();
                    comboResourceName.SelectedIndexChanged += new EventHandler(ddlrgResourceOnProjects_SelectedIndexChanged);


                    DropDownList comboProjectName = (e.Item as GridEditableItem)["ProjectName"].Controls[1] as DropDownList;
                    comboProjectName.SelectedIndexChanged += new EventHandler(ddlrgResourceOnProjects_SelectedIndexChanged);

                    DropDownList comboRole = (e.Item as GridEditableItem)["RoleName"].Controls[1] as DropDownList;
                    comboRole.SelectedIndexChanged += new EventHandler(ddlrgResourceOnProjects_SelectedIndexChanged);
                    //DropDownList tb = (e.Item as GridEditableItem)["comboResourceName"].FindControl("comboResourceName") as DropDownList;
                    //tb.Focus();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }
        }

        protected void ddlrgResourceOnProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = sender as DropDownList;
            String ddlName = ddl.ID;
            if (ddlName.Contains("comboResourceName"))
            {
                Session["ResourceName"] = ddl.SelectedValue.ToString();
            }
            if (ddlName.Contains("comboProjectName"))
            {
                Session["DepartmentName"] = ddl.SelectedValue.ToString();
            }
            //if (ddlName.Contains("comboRoleName"))
            if (ddlName.Contains("comboRole"))
            {
                Session["RoleName"] = ddl.SelectedValue.ToString();
            }
            rgResourceOnProjects.Rebind();
            ////GridDataItem item = (GridDataItem)e.Item;
            //DropDownList comboResourceName = (e.Item as GridEditableItem)["ResourceName"].FindControl("comboResourceName") as DropDownList;
            //objBLL = new MetisBLL();
            //DataTable dt = objBLL.getAllResources(Session["user"].ToString());
            //comboResourceName.DataSource = dt;
            //comboResourceName.DataTextField = dt.Columns[1].ToString();
            //comboResourceName.DataValueField = dt.Columns[0].ToString();
            //comboResourceName.DataBind();
            //comboResourceName.Width = Unit.Pixel(240); // Set the width  
            //if (Session["ResourceName"] != null)
            //{
            //    comboResourceName.Items.FindByText(Session["ResourceName"].ToString()).Selected = true;
            //}

            //DropDownList comboDepartmentName = (e.Item as GridEditableItem)["ProjectName"].FindControl("comboProjectName") as DropDownList;
            //objBLL = new MetisBLL();
            //comboDepartmentName.DataSource = objBLL.getProjects();
            //comboDepartmentName.DataTextField = objBLL.getProjects().Columns[1].ToString();
            //comboDepartmentName.DataValueField = objBLL.getProjects().Columns[0].ToString();
            //comboDepartmentName.DataBind();
            //comboDepartmentName.Width = Unit.Pixel(240); // Set the width  
            //if (Session["DepartmentName"] != null)
            //{
            //    comboDepartmentName.Items.FindByText(Session["DepartmentName"].ToString()).Selected = true;
            //}


            //DropDownList comboRoleName = (e.Item as GridEditableItem)["RoleName"].FindControl("comboRole") as DropDownList;
            //objBLL = new MetisBLL();
            //DataTable dat = objBLL.getRolesfrmResID(comboResourceName.SelectedValue);
            //comboRoleName.DataSource = dat;
            //comboRoleName.DataTextField = dat.Columns[1].ToString();
            //comboRoleName.DataValueField = dat.Columns[0].ToString();
            //comboRoleName.DataBind();
            //comboRoleName.Width = Unit.Pixel(240); // Set the width
            //if (Session["RoleName"] != null)
            //{
            //    comboRoleName.Items.FindByText(Session["RoleName"].ToString()).Selected = true;
            //}
        }
        
        protected void rgResourceOnProjects_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem && e.Item.IsInEditMode)
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    DropDownList comboResourceName = (e.Item as GridEditableItem)["ResourceName"].FindControl("comboResourceName") as DropDownList;
                    objBLL = new MetisBLL();
                    DataTable dt = objBLL.getAllResources(Session["user"].ToString());
                    comboResourceName.DataSource = dt;
                    comboResourceName.DataTextField = dt.Columns[1].ToString();
                    comboResourceName.DataValueField = dt.Columns[0].ToString();
                    comboResourceName.DataBind();
                    comboResourceName.Width = Unit.Pixel(240); // Set the width  
                    if (Session["ResourceName"] != null)
                    {
                        string ResourceName = Session["ResourceName"].ToString();
                        if ((comboResourceName.Items.FindByValue(Session["ResourceName"].ToString()) != null))
                        {
                            comboResourceName.Items.FindByValue(Session["ResourceName"].ToString()).Selected = true;
                            //comboResourceName.SelectedValue = Session["ResourceName"].ToString();
                        }

                        if ((comboResourceName.Items.FindByText(Session["ResourceName"].ToString()) != null))
                        {
                            comboResourceName.Items.FindByText(Session["ResourceName"].ToString()).Selected = true;
                            //comboResourceName.SelectedValue = Session["ResourceName"].ToString();
                        }
                    }
                    Session["ResourceName"] = comboResourceName.SelectedValue.Trim();

                    DropDownList comboDepartmentName = (e.Item as GridEditableItem)["ProjectName"].FindControl("comboProjectName") as DropDownList;
                    objBLL = new MetisBLL();
                    comboDepartmentName.DataSource = objBLL.getProjects();
                    comboDepartmentName.DataTextField = objBLL.getProjects().Columns[1].ToString();
                    comboDepartmentName.DataValueField = objBLL.getProjects().Columns[0].ToString();
                    comboDepartmentName.DataBind();
                    comboDepartmentName.Width = Unit.Pixel(240); // Set the width  
                    if (Session["DepartmentName"] != null)
                    {
                        string DepartmentName = Session["DepartmentName"].ToString();
                        if ((comboDepartmentName.Items.FindByValue(Session["DepartmentName"].ToString()) != null))
                        {
                            comboDepartmentName.Items.FindByValue(Session["DepartmentName"].ToString()).Selected = true;
                            //comboResourceName.SelectedValue = Session["DepartmentName"].ToString();
                        }
                        if ((comboDepartmentName.Items.FindByText(Session["DepartmentName"].ToString()) != null))
                        {
                            comboDepartmentName.Items.FindByText(Session["DepartmentName"].ToString()).Selected = true;
                            //comboResourceName.SelectedValue = Session["DepartmentName"].ToString();
                        }
                    }
                    Session["DepartmentName"] = comboDepartmentName.Text.Trim();
                    

                    DropDownList comboRoleName = (e.Item as GridEditableItem)["RoleName"].FindControl("comboRole") as DropDownList;
                    //objBLL = new MetisBLL();
                    DataTable dat = rolesManager.GetRolesfrmResID(comboResourceName.SelectedValue);
                    comboRoleName.DataSource = dat;
                    comboRoleName.DataTextField = dat.Columns[1].ToString();
                    comboRoleName.DataValueField = dat.Columns[0].ToString();
                    comboRoleName.DataBind();
                    comboRoleName.Width = Unit.Pixel(240); // Set the width
                    if (Session["RoleName"] != null)
                    {
                        string RoleName = Session["RoleName"].ToString();
                        if ((comboRoleName.Items.FindByValue(Session["RoleName"].ToString()) != null))
                        {
                            comboRoleName.Items.FindByValue(Session["RoleName"].ToString()).Selected = true;
                            //comboRoleName.SelectedValue = Session["RoleName"].ToString();
                        }
                        if ((comboRoleName.Items.FindByText(Session["RoleName"].ToString()) != null))
                        {
                            comboRoleName.Items.FindByText(Session["RoleName"].ToString()).Selected = true;
                            //comboRoleName.SelectedValue = Session["RoleName"].ToString();
                        }
                    }
                    Session["RoleName"] = comboRoleName.Text.Trim();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }

        }
        
        protected void rgResourceOnProjects_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("PerformInsert"))
            {
                DropDownList comboResourceName = (DropDownList)e.Item.FindControl("comboResourceName");
                DropDownList comboProjectName = (DropDownList)e.Item.FindControl("comboProjectName");
                DropDownList comboRoleName = (DropDownList)e.Item.FindControl("comboRole");

                string p_message = "";
                objBLL = new MetisBLL();
                objBLL.insertResourceAssignment(comboResourceName.SelectedValue, comboProjectName.SelectedValue, comboRoleName.SelectedValue, out p_message);

                rgResourceOnProjects.DataSource = objBLL.getAllResourceAssignments(Session["user"].ToString());
                rgResourceOnProjects.DataBind();
            }
            if (e.CommandName.Equals("Edit"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                string ResourceName = ((DataBoundLiteralControl)item.Controls[3].Controls[0]).Text.Trim();
                string DepartmentName = ((DataBoundLiteralControl)item.Controls[4].Controls[0]).Text.Trim();
                string RoleName = ((DataBoundLiteralControl)item.Controls[5].Controls[0]).Text.Trim();
                //Session["DepartmentID"] = DepartmentID;
                Session["ResourceName"] = ResourceName;
                Session["DepartmentName"] = DepartmentName;
                Session["RoleName"] = RoleName;

                rgResourceOnProjects.DataBind();
            }
            if (e.CommandName.Equals("Update"))
            {
                DropDownList comboResourceName = (DropDownList)e.Item.FindControl("comboResourceName");
                DropDownList comboProjectName = (DropDownList)e.Item.FindControl("comboProjectName");
                DropDownList comboRoleName = (DropDownList)e.Item.FindControl("comboRole");
                string ResourceAssignmentID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ResourceAssignmentID"].ToString();
                string p_message = "";
                objBLL = new MetisBLL();
                objBLL.updateResourceAssignment(ResourceAssignmentID, comboResourceName.SelectedItem.Text, comboProjectName.SelectedItem.Text, comboRoleName.SelectedItem.Text, out p_message);

                rgResourceOnProjects.DataSource = objBLL.getAllResourceAssignments(Session["user"].ToString());
                rgResourceOnProjects.DataBind();
            }
            if (e.CommandName.Equals("Cancel"))
            {
                rgResourceOnProjects.DataBind();
            }
        }

        #endregion

        #region "Bulk Assignment"

        protected void rgBulkAssignment_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            objBLL = new MetisBLL();
            rgBulkAssignment.DataSource = objBLL.getAllBulkAssignments(Session["user"].ToString());
        }

        protected void rgBulkAssignment_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridDataItem item = (GridDataItem)e.Item;
            try
            {
                string BulkAssignmentID = item.OwnerTableView.DataKeyValues[item.ItemIndex]["BulkAssignmentID"].ToString();
                string WeekEnding = ((DataBoundLiteralControl)e.Item.Controls[3].Controls[0]).Text.Trim();
                string ResourceName = ((DataBoundLiteralControl)e.Item.Controls[4].Controls[0]).Text.Trim();
                string ProjectName = ((DataBoundLiteralControl)e.Item.Controls[5].Controls[0]).Text.Trim();
                string AssignmentTypeName = ((DataBoundLiteralControl)e.Item.Controls[6].Controls[0]).Text.Trim();
                string BulkStartDate = ((DataBoundLiteralControl)e.Item.Controls[7].Controls[0]).Text.Trim();
                string BulkEndDate = ((DataBoundLiteralControl)e.Item.Controls[8].Controls[0]).Text.Trim();
                string WorkLoad = ((DataBoundLiteralControl)e.Item.Controls[9].Controls[0]).Text.Trim();
                
                DateTime weekEnding = DateTime.Parse(WeekEnding);
                DateTime start_bulk = DateTime.Parse(BulkStartDate);
                DateTime end_bulk = DateTime.Parse(BulkEndDate);
                double work_load = double.Parse(WorkLoad);

                //DateTime temp_start = start_bulk;
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

                string p_message = "";
                MetisBLL objBll = new MetisBLL();
                /*objBll.deleteBulkAssignment(ResourceName, ProjectName,AssignmentTypeName, start, end, weekEnding, start_bulk, end_bulk, out p_message);*/
                objBll.deleteBulkAssignment(ResourceName, ProjectName, start, end, weekEnding, start_bulk, end_bulk, AssignmentTypeName, out p_message);

                //#region format

                #region "Old"
                //DateTime temp_start = start_bulk;
                //DateTime temp_prev = start_bulk;
                //DateTime temp_end = end_bulk;

                //////
                //DayOfWeek day = start_bulk.DayOfWeek;
                //int days = day - DayOfWeek.Saturday;
                //DateTime start = temp_prev.AddDays(-days);
                /////////////
                //DayOfWeek day_end = end_bulk.DayOfWeek;
                //int days_end = day_end - DayOfWeek.Saturday;
                //DateTime end = temp_end.AddDays(-days_end);
                ////////////////

                //int nOfDays = 5, nOfDays_2 = 1;

                //days = 6 + days;
                //days_end = 6 + days_end;

                //if (days == 6)
                //    days = nOfDays_2;
                //if (days_end == 6)
                //    days_end = days_end - nOfDays_2;

                //#endregion


                //string p_message = "";
                //objBLL = new MetisBLL();
                //objBLL.deleteBulkAssignment(BulkAssignmentID, out p_message);

                ////string QryDelete = string.Format("DELETE FROM [ProMan].[dbo].[Weekly_Reports] where id='{0}'", BulkAssignmentID);
                ////int rowsInserted = SQLDataConnection.GetInstance().ExecuteQuery(QryDelete);

                //string[] week_begin = new string[7];
                //string[] week_last = new string[7];

                //#region Bulk erase Query
                //while (end_bulk.Date > temp_start.Date)
                //{

                //    if (temp_start.Date == start_bulk.Date)
                //    {

                //        while (nOfDays >= days)
                //        {

                //            week_begin[days] = "";//work_load.ToString();
                //            days++;
                //        }

                //        objBLL.deleteBulkAssignment(ResourceName, ProjectName, weekEnding, out p_message);
                //        //string QryErase_bulk_being = string.Format(Settings.Default.qryDeleteBulk, newValues["Resource_id"], newValues["Project_id"], start.Date);
                //        //rowsInserted = SQLDataConnection.GetInstance().ExecuteQuery(QryErase_bulk_being);

                //        while (days_end >= nOfDays_2)
                //        {

                //            week_last[days_end] = "";//work_load.ToString();
                //            days_end--;
                //        }
                //        if (start.Date != end.Date)
                //        {

                //            objBLL.deleteBulkAssignment(ResourceName, ProjectName, end.Date, out p_message);
                //            //string QryErase_bulk_last = string.Format(Settings.Default.qryDeleteBulk, newValues["Resource_id"], newValues["Project_id"], end.Date);
                //            //rowsInserted = SQLDataConnection.GetInstance().ExecuteQuery(QryErase_bulk_last);
                //        }
                //    }
                //    else if (temp_start.DayOfWeek == DayOfWeek.Saturday && temp_start.Date != start.Date && temp_start.Date != end.Date)
                //    {

                //        objBLL.deleteBulkAssignment(ResourceName, ProjectName, temp_start.Date, out p_message);
                //        //string QryFormat_bulk = string.Format(Settings.Default.qryDeleteBulk, currentRow.SavedOldValues["Resource_id"], currentRow.SavedOldValues["Project_id"], temp_start.Date);
                //        //rowsInserted = SQLDataConnection.GetInstance().ExecuteQuery(QryFormat_bulk);
                //    }


                //    int daysInMonth = DateTime.DaysInMonth(temp_start.Year, temp_start.Month);
                //    if (temp_start.Day == daysInMonth)
                //        temp_start.AddMonths(1);
                //    temp_start = temp_start.AddDays(1);

                //}
                //#endregion

                //RadGrid_bulk.DataSource = SQLDataConnection.GetInstance().ExecuteDataSet(Settings.Default.qryBulk_assignment);
                //RadGrid_bulk.Rebind();

                #endregion


            }
            catch (Exception ex)
            {
                rgBulkAssignment.Controls.Add(new LiteralControl("Unable to delete Assignment. Reason: " + ex.Message));
                e.Canceled = true;
            }
        }

        // EMPTY
        protected void rgBulkAssignment_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
        }
        
        // EMPTY
        protected void rgBulkAssignment_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
           
        }
        
        protected void rgBulkAssignment_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {
                    
                    //if (rgBulkAssignment.EditIndexes.Count <= 1)
                    //{
                        //Telerik.Web.UI.RadComboBox comResourceName = (e.Item as GridEditableItem)["ResourceName"].Controls[1] as Telerik.Web.UI.RadComboBox;
                        //comResourceName.Focus();
                        //comResourceName.ItemChecked += new RadComboBoxItemEventHandler(comResourceName_ItemChecked);
                        ////ddlResourceName.AutoPostBack = true;

                        DropDownList comResourceName = (e.Item as GridEditableItem)["ResourceName"].Controls[1] as DropDownList;
                        comResourceName.Focus();

                        DropDownList ddlProjectName = (e.Item as GridEditableItem)["ProjectName"].Controls[1] as DropDownList;
                        RadAjaxManager1.AjaxSettings.AddAjaxSetting(comResourceName, ddlProjectName);

                        DropDownList comboAssignmentTypeName = (e.Item as GridEditableItem)["AssignmentTypeName"].Controls[1] as DropDownList;

                        Telerik.Web.UI.RadDatePicker calenderWeekEnding = (e.Item as GridEditableItem)["WeekEnding"].FindControl("calenderWeekEnding") as Telerik.Web.UI.RadDatePicker;
                        //calenderWeekEnding.SelectedDate = Session["WeekEnding"] != null ? Convert.ToDateTime(Session["WeekEnding"].ToString()) : DateTime.Now;
                        if (Session["WeekEnding"] != null)
                        {
                            calenderWeekEnding.SelectedDate = Convert.ToDateTime(Session["WeekEnding"].ToString());
                        }
                        else
                        {
                            //calenderWeekEnding.SelectedDate = NextSaturday(DateTime.Now);
                            calenderWeekEnding.SelectedDate = DateTime.Now;
                        }

                        ////calenderWeekEnding.SelectedDate = DateTime.Now;

                        //Telerik.Web.UI.RadDatePicker calenderStartDate = (e.Item as GridEditableItem)["BulkStartDate"].FindControl("calenderStartDate") as Telerik.Web.UI.RadDatePicker;
                        //calenderWeekEnding.SelectedDate = Session["BulkStartDate"] != null ? Convert.ToDateTime(Session["BulkStartDate"].ToString()) : DateTime.Now;
                        ////calenderStartDate.SelectedDate = (calenderStartDate.SelectedDate == DateTime.Now || calenderStartDate.SelectedDate == null) ? DateTime.Now : calenderStartDate.SelectedDate;

                        //Telerik.Web.UI.RadDatePicker calenderEndDate = (e.Item as GridEditableItem)["BulkEndDate"].FindControl("calenderEndDate") as Telerik.Web.UI.RadDatePicker;
                        //calenderWeekEnding.SelectedDate = Session["BulkEndDate"] != null ? Convert.ToDateTime(Session["BulkEndDate"].ToString()) : DateTime.Now;
                        ////calenderEndDate.SelectedDate = (calenderEndDate.SelectedDate == DateTime.Now || calenderEndDate.SelectedDate == null) ? DateTime.Now : calenderEndDate.SelectedDate;

                        //TextBox txtWorkLoad = (e.Item as GridEditableItem)["BulkWorkLoad"].FindControl("txtWorkLoad") as TextBox;
                        //txtWorkLoad.Text = Session["WorkLoad"] != null ? Session["WorkLoad"].ToString() : "0";
                        ////txtWorkLoad.Text = String.IsNullOrEmpty(txtWorkLoad.Text) ? "0" : txtWorkLoad.Text;                    

                        //String WeekEnding = Session["WeekEnding"].ToString();
                        //String ResourceName = Session["ResourceName"].ToString();
                        //String ProjectName = Session["ProjectName"].ToString();
                        //String BulkStartDate = Session["BulkStartDate"].ToString();
                        //String BulkEndDate = Session["BulkEndDate"].ToString();
                        //String WorkLoad = Session["WorkLoad"].ToString();
                    //}
                    //else
                    //{
                    //    e.Item.Edit = false;
                    //}
                }
            }
                
            
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }
        }

        // EXTRA
        protected void comResourceName_ItemChecked(object sender, Telerik.Web.UI.RadComboBoxItemEventArgs e)
        {
            Telerik.Web.UI.RadComboBox comResourceName = new Telerik.Web.UI.RadComboBox();
            string keywordsCondition = String.Empty;
            string sValues = "";
            int totalChekdItems = comResourceName.CheckedItems.Count;
            int OrWord = comResourceName.CheckedItems.Count - 1;

            for (int x = 0; x < comResourceName.Items.Count; x++)
            {
                if ((comResourceName.Items[x].Checked))
                {
                    sValues += "[ResourceName] like '%" + comResourceName.Items[x].Text.Replace("'", "''") + "%'";

                    while (OrWord > 0)
                    {
                        sValues += " OR ";
                        OrWord--;
                        break;
                    }
                }
            }


            if (sValues.Contains("All") || sValues == "")
                keywordsCondition = "[ResourceName] like '%%'";
            else
                keywordsCondition = sValues;

            //MetisBLL objBll = new MetisBLL();
            //System.Data.DataTable dt = new System.Data.DataTable();
            ////dt = objBll.getProjects();
            ////dt = objBll.getAllResources(Session["user"].ToString());
            //rgBulkAssignment.DataSource = objBLL.getAllBulkAssignments(Session["user"].ToString());
            //DataView dv = new DataView(dt);
            //string query = string.Empty;
            //// string nameCondition = ddlFilter.SelectedItem.ToString().Replace("'", "''") == "All" ? "[Employee Name] like '%%' " : "[Employee Name] like '%" + ddlFilter.SelectedItem.ToString().Replace("'", "''") + "%' ";

            ////query = nameCondition;
            //dv.RowFilter = keywordsCondition;
            //rgBulkAssignment.DataSource = dv;
            //rgBulkAssignment.Rebind();
            ////Session["dataViewLeaveSummary"] = dv;

        }

        protected void rgBulkAssignment_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem && e.Item.IsInEditMode)
                {
                    //if (rgBulkAssignment.EditIndexes.Count <= 1)
                    //{
                        GridDataItem item = (GridDataItem)e.Item;

                        Telerik.Web.UI.RadDatePicker calenderWeekEnding = (e.Item as GridEditableItem)["WeekEnding"].FindControl("calenderWeekEnding") as Telerik.Web.UI.RadDatePicker;

                        //Telerik.Web.UI.RadComboBox comResourceName = (e.Item as GridEditableItem)["ResourceName"].FindControl("comResourceName") as Telerik.Web.UI.RadComboBox;
                        DropDownList comResourceName = (e.Item as GridEditableItem)["ResourceName"].FindControl("comResourceName") as DropDownList;
                        objBLL = new MetisBLL();
                        DataTable dt = new DataTable();
                        dt = objBLL.getAllResources(Session["user"].ToString());
                        comResourceName.DataSource = dt;
                        comResourceName.DataTextField = dt.Columns[1].ToString();
                        comResourceName.DataValueField = dt.Columns[0].ToString();
                        comResourceName.DataBind();
                        comResourceName.Width = Unit.Pixel(240); // Set the width  
                        comResourceName.Focus();


                        if (Session["ResourceName"] != null)
                            ////comResourceName.Items.FindItemByText(Session["ResourceName"].ToString()).Selected = true;
                            //if ((comResourceName.Items.FindItemByValue(Session["ResourceName"].ToString()) != null))
                            //{
                            //    comResourceName.Items.FindItemByValue(Session["ResourceName"].ToString()).Selected = true;
                            //}
                            //comResourceName.Items.FindItemByText(Session["ResourceName"].ToString()).Selected = true;
                            if ((comResourceName.Items.FindByValue(Session["ResourceName"].ToString()) != null))
                            {
                                comResourceName.Items.FindByValue(Session["ResourceName"].ToString()).Selected = true;
                            }

                        DropDownList ddlDepartmentName = (e.Item as GridEditableItem)["ProjectName"].FindControl("comboProjectName") as DropDownList;
                        objBLL = new MetisBLL();

                        dt = objBLL.getProjects();
                        ddlDepartmentName.DataSource = dt;
                        ddlDepartmentName.DataTextField = dt.Columns[1].ToString();
                        ddlDepartmentName.DataValueField = dt.Columns[0].ToString();
                        ddlDepartmentName.DataBind();
                        ddlDepartmentName.Width = Unit.Pixel(240); // Set the width  

                        DropDownList comboAssignmentTypeName = (e.Item as GridEditableItem)["AssignmentTypeName"].FindControl("comboAssignmentTypeName") as DropDownList;
                        objBLL = new MetisBLL();

                        dt = objBLL.getAssignmentType();
                        comboAssignmentTypeName.DataSource = dt;
                        comboAssignmentTypeName.DataTextField = dt.Columns[1].ToString();
                        comboAssignmentTypeName.DataValueField = dt.Columns[0].ToString();
                        comboAssignmentTypeName.DataBind();
                        comboAssignmentTypeName.Width = Unit.Pixel(240); // Set the width  


                        Telerik.Web.UI.RadDatePicker calenderStartDate = (e.Item as GridEditableItem)["BulkStartDate"].FindControl("calenderStartDate") as Telerik.Web.UI.RadDatePicker;
                        Telerik.Web.UI.RadDatePicker calenderEndDate = (e.Item as GridEditableItem)["BulkEndDate"].FindControl("calenderEndDate") as Telerik.Web.UI.RadDatePicker;
                        TextBox txtWorkLoad = (e.Item as GridEditableItem)["BulkWorkLoad"].FindControl("txtWorkLoad") as TextBox;

                        if (Session["WeekEnding"] != null)
                        {
                            calenderWeekEnding.SelectedDate = Convert.ToDateTime(Session["WeekEnding"].ToString());
                            //comResourceName.Items.FindItemByText(Session["ResourceName"].ToString()).Selected = true;
                            comResourceName.Items.FindByText(Session["ResourceName"].ToString()).Selected = true;
                            ddlDepartmentName.Items.FindByText(Session["ProjectName"].ToString()).Selected = true;
                            //if (Session["AssignmentTypeName"] != null && !String.IsNullOrEmpty(Session["AssignmentTypeName"].ToString()))
                            //{
                                comboAssignmentTypeName.Items.FindByText(Session["AssignmentTypeName"].ToString()).Selected = true;
                            //}
                            
                            txtWorkLoad.Text = (Session["WorkLoad"] == null ? "" : Session["WorkLoad"]).ToString();
                            // Let these have Errors about 1900-01-01
                            calenderStartDate.SelectedDate = Session["BulkStartDate"] != null ? Convert.ToDateTime(Session["BulkStartDate"].ToString()) : DateTime.Now;
                            calenderEndDate.SelectedDate = Session["BulkEndDate"] != null ? Convert.ToDateTime(Session["BulkEndDate"].ToString()) : DateTime.Now;
                        }
                    //}
                    //else
                    //{
                    //    e.Item.Edit = false;
                    //}
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }

        }

        protected void rgBulkAssignment_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("PerformInsert"))
            {
                //Telerik.Web.UI.RadComboBox comResourceName = (Telerik.Web.UI.RadComboBox)e.Item.FindControl("comResourceName");
                DropDownList comResourceName = (DropDownList)e.Item.FindControl("comResourceName");
                DropDownList comboProjectName = (DropDownList)e.Item.FindControl("comboProjectName");
                DropDownList comboAssignmentTypeName = (DropDownList)e.Item.FindControl("comboAssignmentTypeName");
                Telerik.Web.UI.RadDatePicker calenderStartDate = (Telerik.Web.UI.RadDatePicker)e.Item.FindControl("calenderStartDate");
                Telerik.Web.UI.RadDatePicker calenderEndDate = (Telerik.Web.UI.RadDatePicker)e.Item.FindControl("calenderEndDate");
                Telerik.Web.UI.RadDatePicker calenderWeekEnding = (Telerik.Web.UI.RadDatePicker)e.Item.FindControl("calenderWeekEnding");
                TextBox txtWorkLoad = (TextBox)e.Item.FindControl("txtWorkLoad");

                DateTime dWeekEnding = (DateTime)calenderWeekEnding.SelectedDate;

                //string sResourceID = comResourceName.SelectedValue;
                string sProjectID = comboProjectName.SelectedValue;
                DateTime dStartDate = (DateTime)calenderStartDate.SelectedDate;
                DateTime dEndDate = (DateTime)calenderEndDate.SelectedDate;
                float fWorkLoad = float.Parse(txtWorkLoad.Text);

                ////BulkInsert(e, dWeekEnding, sResourceID, sProjectID, dStartDate, dEndDate, fWorkLoad);
                //for (int i = 0; i < comResourceName.CheckedItems.Count; i++)
                //{
                //    string sResourceID = comResourceName.CheckedItems[i].Value;
                //    BulkInsert(e, dWeekEnding, sResourceID, sProjectID, dStartDate, dEndDate, fWorkLoad);
                //}                
                ////rgBulkAssignment.DataSource = objBLL.getAllBulkAssignments();
                ////rgBulkAssignment.DataBind();
                string sResourceID = comResourceName.SelectedValue;
                string AssignmentTypeID = comboAssignmentTypeName.SelectedValue;
                BulkInsert(e, dWeekEnding, sResourceID, sProjectID, dStartDate, dEndDate, fWorkLoad, AssignmentTypeID);

                rgBulkAssignment.Rebind();
            }
            if (e.CommandName.Equals("Edit"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                string WeekEnding = ((DataBoundLiteralControl)item.Controls[3].Controls[0]).Text.Trim();
                string ResourceName = ((DataBoundLiteralControl)item.Controls[4].Controls[0]).Text.Trim();
                string ProjectName = ((DataBoundLiteralControl)item.Controls[5].Controls[0]).Text.Trim();
                string AssignmentTypeName = ((DataBoundLiteralControl)item.Controls[6].Controls[0]).Text.Trim();
                string BulkStartDate = ((DataBoundLiteralControl)item.Controls[7].Controls[0]).Text.Trim();
                string BulkEndDate = ((DataBoundLiteralControl)item.Controls[8].Controls[0]).Text.Trim();
                string WorkLoad = ((DataBoundLiteralControl)item.Controls[9].Controls[0]).Text.Trim();
                //Session["DepartmentID"] = DepartmentID;
                Session["BulkAssignmentID"] = item.OwnerTableView.DataKeyValues[item.ItemIndex]["BulkAssignmentID"].ToString();
                Session["WeekEnding"] = WeekEnding;
                Session["ResourceName"] = ResourceName;
                Session["ProjectName"] = ProjectName;
                Session["AssignmentTypeName"] = AssignmentTypeName;
                Session["BulkStartDate"] = BulkStartDate;
                Session["BulkEndDate"] = BulkEndDate;
                Session["WorkLoad"] = WorkLoad;

                rgBulkAssignment.DataBind();
            }
            if (e.CommandName.Equals("Update"))
            {
                
                /*First Delete and then Insert*/

                DateTime weekEnding = DateTime.Parse(Session["WeekEnding"].ToString());
                DateTime start_bulk = DateTime.Parse(Session["BulkStartDate"].ToString());
                DateTime end_bulk = DateTime.Parse(Session["BulkEndDate"].ToString());
                double work_load = double.Parse(Session["WorkLoad"].ToString());
                String assignmentTypeName = Session["AssignmentTypeName"].ToString();

                //DateTime temp_start = start_bulk;
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

                string p_message = "";
                MetisBLL objBll = new MetisBLL();
                /*objBll.deleteBulkAssignment(Session["ResourceName"].ToString(), Session["ProjectName"].ToString(), Session["AssignmentTypeName"].ToString(), start, end, weekEnding, start_bulk, end_bulk, out p_message);*/
                objBll.deleteBulkAssignment(Session["ResourceName"].ToString(), Session["ProjectName"].ToString(),  start, end, weekEnding, start_bulk, end_bulk, assignmentTypeName, out p_message);

                Telerik.Web.UI.RadDatePicker calenderWeekEnding = (Telerik.Web.UI.RadDatePicker)e.Item.FindControl("calenderWeekEnding");
                //Telerik.Web.UI.RadComboBox comResourceName = (Telerik.Web.UI.RadComboBox)e.Item.FindControl("comResourceName");
                DropDownList comResourceName = (DropDownList)e.Item.FindControl("comResourceName");
                DropDownList comboProjectName = (DropDownList)e.Item.FindControl("comboProjectName");
                DropDownList comboAssignmentTypeName = (DropDownList)e.Item.FindControl("comboAssignmentTypeName");
                Telerik.Web.UI.RadDatePicker calenderStartDate = (Telerik.Web.UI.RadDatePicker)e.Item.FindControl("calenderStartDate");
                Telerik.Web.UI.RadDatePicker calenderEndDate = (Telerik.Web.UI.RadDatePicker)e.Item.FindControl("calenderEndDate");
                string ResourceAssignmentID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BulkAssignmentID"].ToString();
                TextBox txtWorkLoad = (TextBox)e.Item.FindControl("txtWorkLoad");

                DateTime dWeekEnding = (DateTime)calenderWeekEnding.SelectedDate;
                string sResourceID = comResourceName.SelectedValue;
                string sProjectID = comboProjectName.SelectedValue;
                string sAssignmentTypeName = comboAssignmentTypeName.SelectedValue;
                
                DateTime dStartDate = (DateTime)calenderStartDate.SelectedDate;
                DateTime dEndDate = (DateTime)calenderEndDate.SelectedDate;
                float fWorkLoad = float.Parse(txtWorkLoad.Text);

                BulkInsert(e, dWeekEnding, sResourceID, sProjectID, dStartDate, dEndDate, fWorkLoad, sAssignmentTypeName);

                //Clear Session
                Session["BulkAssignmentID"] = null;
                Session["ResourceName"] = null;
                Session["ProjectName"] = null;
                Session["AssignmentTypeName"] = null;
                Session["BulkStartDate"] = null;
                Session["BulkEndDate"] = null;
                Session["WorkLoad"] = null;
                Session["WeekEnding"] = null;

                //rgBulkAssignment.DataSource = objBLL.getAllBulkAssignments();
                //rgBulkAssignment.DataBind();
                rgBulkAssignment.Rebind();
            }
            if (e.CommandName.Equals("Cancel"))
            {
                
                //Clear Session
                Session["BulkAssignmentID"] = null;
                Session["ResourceName"] = null;
                Session["ProjectName"] = null;
                Session["AssignmentTypeName"] = null;
                Session["BulkStartDate"] = null;
                Session["BulkEndDate"] = null;
                Session["WorkLoad"] = null;
                Session["WeekEnding"] = null;
                rgBulkAssignment.Rebind();
            }
        }

        protected void BulkInsert(GridCommandEventArgs e, DateTime dWeekEnding, string sResourceID, string sProjectID, DateTime dStartDate, DateTime dEndDate, float fWorkLoad, string AssignmentTypeID)
        {
            try
            {
                float Sunday = 0;
                float Monday = 0;
                float Tuesday = 0;
                float Wednesday = 0;
                float Thursday = 0;
                float Friday = 0;
                float Saturday = 0;
                string BulkAss = "true";
                string IsDeleted = "";
                string WorkDays = "";
                string AvailableDays = "";
                string p_message = "";


                double work_load = new double();
                string[] workloadPercent;
                if (fWorkLoad.ToString().Contains('%'))
                {
                    workloadPercent = fWorkLoad.ToString().Split('%');
                    if (workloadPercent[0] != null)
                    {
                        work_load = (double.Parse(workloadPercent[0]) / 100) * 9;
                    }
                }
                else
                    work_load = double.Parse(fWorkLoad.ToString());

                /*Parent Entry*/
                objBLL = new MetisBLL();
                objBLL.insertBulkAssignment(
                    sResourceID, sProjectID, WorkDays, AvailableDays, dWeekEnding, 
                    Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, 
                    BulkAss, work_load.ToString(), dStartDate.ToString(), dEndDate.ToString(), IsDeleted, AssignmentTypeID,out p_message);

                /*Child Entries*/
                TimeSpan diff = dEndDate - dStartDate;
                int days = diff.Days;
                for (var i = 0; i <= days; i++)
                {
                    var tempDate = dStartDate.AddDays(i);
                    switch (tempDate.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            Monday = float.Parse(work_load.ToString());
                            break;

                        case DayOfWeek.Tuesday:
                            Tuesday = float.Parse(work_load.ToString());
                            break;

                        case DayOfWeek.Wednesday:
                            Wednesday = float.Parse(work_load.ToString());
                            break;

                        case DayOfWeek.Thursday:
                            Thursday = float.Parse(work_load.ToString());
                            break;

                        case DayOfWeek.Friday:
                            Friday = float.Parse(work_load.ToString());
                            break;

                        case DayOfWeek.Saturday:
                            objBLL.insertBulkAssignment(sResourceID,
                            sProjectID, WorkDays, AvailableDays, tempDate,
                            Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday,
                            "", "", "", "", "", AssignmentTypeID, out p_message);
                            Monday = 0;
                            Tuesday = 0;
                            Wednesday = 0;
                            Thursday = 0;
                            Friday = 0;
                            break;
                    }


                }

                /*Last Week Entry*/
                DateTime WeekEnding = new DateTime();
                switch (dEndDate.DayOfWeek)
                {

                    case DayOfWeek.Monday:
                        Monday = float.Parse(work_load.ToString());
                        WeekEnding = dEndDate.AddDays(5);
                        break;


                    case DayOfWeek.Tuesday:
                        Tuesday = float.Parse(work_load.ToString());
                        WeekEnding = dEndDate.AddDays(4);
                        break;

                    case DayOfWeek.Wednesday:
                        Wednesday = float.Parse(work_load.ToString());
                        WeekEnding = dEndDate.AddDays(3);
                        break;

                    case DayOfWeek.Thursday:
                        Thursday = float.Parse(work_load.ToString());
                        WeekEnding = dEndDate.AddDays(2);
                        break;

                    case DayOfWeek.Friday:
                        Friday = float.Parse(work_load.ToString());
                        WeekEnding = dEndDate.AddDays(1);
                        break;

                }
                objBLL.insertBulkAssignment(sResourceID,
                         sProjectID, WorkDays, AvailableDays, WeekEnding,
                         Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday,
                         "", "", "", "", "", AssignmentTypeID, out p_message);


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }
            #region Old Code
            /* string sWorkDays = "";
            string sAvailableDays = "";
            float fSunday = 0;
            float fMonday = 0;
            float fTuesday = 0;
            float fWednesday = 0;
            float fThursday = 0;
            float fFriday = 0;
            float fSaturday = 0;
            string sBulkAssignment = "";
            string sIsDeleted = "";
            string sStartDate = dStartDate.ToString();
            string sEndDate = dEndDate.ToString();
            
            try
            {
                GridEditableItem editedItem = e.Item as GridEditableItem;
                Hashtable newValues = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

                DateTime weekEnding = DateTime.Now;

                if (dWeekEnding != null)
                    weekEnding = dWeekEnding;

                DateTime start_bulk = dStartDate;
                DateTime end_bulk = dEndDate;

                double work_load = new double();
                string[] workloadPercent;
                if (fWorkLoad.ToString().Contains('%'))
                {
                    workloadPercent = fWorkLoad.ToString().Split('%');
                    if (workloadPercent[0] != null)
                    {
                        work_load = (double.Parse(workloadPercent[0]) / 100) * 9;
                    }
                }

                else
                    work_load = double.Parse(fWorkLoad.ToString());

                DateTime temp_start = start_bulk;
                DateTime temp_prev = start_bulk;
                DateTime temp_end = end_bulk;

                ////
                DayOfWeek day = start_bulk.DayOfWeek;
                int days = day - (DayOfWeek.Saturday);
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


                string p_message = "";
                objBLL = new MetisBLL();
                objBLL.insertBulkAssignment(sResourceID, sProjectID, sWorkDays, sAvailableDays, weekEnding, fSunday, fMonday, fTuesday, fWednesday, fThursday, fFriday, fSaturday, "true", fWorkLoad.ToString(), sStartDate, sEndDate, sIsDeleted, out p_message);


                //string QryInsert = string.Format(Settings.Default.qryInsert_bulk, newValues["Resource_id"], newValues["Project_id"], newValues["Work_days"], newValues["Available_days"], weekEnding.Date, newValues["Sunday"], newValues["Monday"], newValues["Tuesday"], newValues["Wednesday"], newValues["Thursday"], newValues["Friday"], newValues["Saturday"], "true", work_load.ToString(), newValues["start_bulk"], newValues["end_bulk"]);
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

                        objBLL.insertBulkAssignment(sResourceID,
                            sProjectID, sWorkDays, sAvailableDays, start.Date,
                            float.Parse((week_begin[0] == null ? "0" : week_begin[0]).ToString()),
                            float.Parse((week_begin[1] == null ? "0" : week_begin[1]).ToString()),
                            float.Parse((week_begin[2] == null ? "0" : week_begin[2]).ToString()),
                            float.Parse((week_begin[3] == null ? "0" : week_begin[3]).ToString()),
                            float.Parse((week_begin[4] == null ? "0" : week_begin[4]).ToString()),
                            float.Parse((week_begin[5] == null ? "0" : week_begin[5]).ToString()),
                            float.Parse((week_begin[6] == null ? "0" : week_begin[6]).ToString()),
                            "", "", "", "", "", out p_message);

                        //string QryInsert_bulk_being = string.Format(Settings.Default.qryInsert_bulk, newValues["Resource_id"], newValues["Project_id"], newValues["Work_days"], newValues["Available_days"], start.Date, week_begin[0], week_begin[1], week_begin[2], week_begin[3], week_begin[4], week_begin[5], week_begin[6], "", "", "", "");
                        //rowsInserted = SQLDataConnection.GetInstance().ExecuteQuery(QryInsert_bulk_being);
                        while (days_end >= nOfDays_2)
                        {

                            week_last[days_end] = work_load.ToString();
                            days_end--;
                        }
                        if (start.Date != end.Date)
                        {
                            objBLL.insertBulkAssignment(sResourceID, sProjectID, sWorkDays, sAvailableDays, end.Date,
                                float.Parse((week_begin[0] == null ? "0" : week_begin[0]).ToString()),
                                float.Parse((week_begin[1] == null ? "0" : week_begin[1]).ToString()),
                                float.Parse((week_begin[2] == null ? "0" : week_begin[2]).ToString()),
                                float.Parse((week_begin[3] == null ? "0" : week_begin[3]).ToString()),
                                float.Parse((week_begin[4] == null ? "0" : week_begin[4]).ToString()),
                                float.Parse((week_begin[5] == null ? "0" : week_begin[5]).ToString()),
                                float.Parse((week_begin[6] == null ? "0" : week_begin[6]).ToString()),
                                "", "", "", "", "", out p_message);

                            //string QryInsert_bulk_last = string.Format(Settings.Default.qryInsert_bulk, newValues["Resource_id"], newValues["Project_id"], newValues["Work_days"], newValues["Available_days"], end.Date, week_last[0], week_last[1], week_last[2], week_last[3], week_last[4], week_last[5], week_last[6], "", "", "", "");
                            //rowsInserted = SQLDataConnection.GetInstance().ExecuteQuery(QryInsert_bulk_last);
                        }
                    }
                    else if (temp_start.DayOfWeek == DayOfWeek.Saturday && temp_start.Date != start.Date && temp_start.Date != end.Date)
                    {
                        objBLL.insertBulkAssignment(sResourceID, sProjectID, sWorkDays, sAvailableDays, temp_start.Date,
                            fSunday,
                            float.Parse(work_load.ToString()),
                            float.Parse(work_load.ToString()),
                            float.Parse(work_load.ToString()),
                            float.Parse(work_load.ToString()),
                            float.Parse(work_load.ToString()),
                            fSaturday,
                            "", "", "", "", "", out p_message);

                        //string QryInsert_bulk = string.Format(Settings.Default.qryInsert_bulk, newValues["Resource_id"], newValues["Project_id"], newValues["Work_days"], newValues["Available_days"], temp_start.Date, newValues["Sunday"], work_load, work_load, work_load, work_load, work_load, newValues["Saturday"], "", "", "", "");
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

            }*/
            #endregion


        }
        #endregion

        #region ResourceLeaves

        protected void rgResourceLeaves_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            objBLL = new MetisBLL();
            rgResourceLeaves.DataSource = objBLL.getAllResourceLeaves();
        }
        
        // EMPTY
        protected void rgResourceLeaves_PreRender(object sender, EventArgs e)
        {

        }

        // EMPTY
        protected void rgResourceLeaves_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
        }

        // EMPTY
        protected void rgResourceLeaves_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {

        }

        protected void rgResourceLeaves_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {



                //Telerik.Web.UI.RadDatePicker BulkEndDate = (e.Item as GridEditableItem)["BulkEndDate"].Controls[1] as Telerik.Web.UI.RadDatePicker;
                //BulkEndDate.SelectedDateChanged += new Telerik.Web.UI.Calendar.SelectedDateChangedEventHandler(BulkEndDate_SelectedDateChanged);

            }
        }

        protected void BulkEndDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            GridEditableItem editedItem = (sender as Telerik.Web.UI.RadDatePicker).NamingContainer as GridEditableItem;
            DropDownList comboResourceName = editedItem["ResourceName"].Controls[1] as DropDownList;
            Telerik.Web.UI.RadDatePicker calenderStartDate = editedItem["BulkStartDate"].Controls[1] as Telerik.Web.UI.RadDatePicker;
            Telerik.Web.UI.RadDatePicker calenderEndDate = editedItem["BulkEndDate"].Controls[1] as Telerik.Web.UI.RadDatePicker;

            string sResourceID = comboResourceName.SelectedValue;
            DateTime dStartDate = (DateTime)calenderStartDate.SelectedDate;
            DateTime dEndDate = (DateTime)calenderEndDate.SelectedDate;

            string p_message1 = String.Empty;

            objBLL = new MetisBLL();
            objBLL.CheckProjectAllocation(sResourceID, dStartDate, dEndDate, out p_message1);
            if (p_message1.Length > 0)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", p_message1, true);

            }



        }


        protected void rgResourceLeaves_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem && e.Item.IsInEditMode)
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    DropDownList ddlResourceName = (e.Item as GridEditableItem)["ResourceName"].FindControl("comboResourceName") as DropDownList;
                    objBLL = new MetisBLL();
                    ddlResourceName.DataSource = objBLL.getAllResources(Convert.ToString(Session["user"]));
                    ddlResourceName.DataTextField = objBLL.getAllResources(Convert.ToString(Session["user"])).Columns[1].ToString();
                    ddlResourceName.DataValueField = objBLL.getAllResources(Convert.ToString(Session["user"])).Columns[0].ToString();
                    ddlResourceName.DataBind();
                    ddlResourceName.Width = Unit.Pixel(240); // Set the width  
                    ddlResourceName.Focus();

                    Telerik.Web.UI.RadDatePicker calenderStartDate = (e.Item as GridEditableItem)["BulkStartDate"].FindControl("calenderStartDate") as Telerik.Web.UI.RadDatePicker;
                    Telerik.Web.UI.RadDatePicker calenderEndDate = (e.Item as GridEditableItem)["BulkEndDate"].FindControl("calenderEndDate") as Telerik.Web.UI.RadDatePicker;


                    if (e.Item is GridEditFormInsertItem || e.Item is GridDataInsertItem)
                    {
                        // insert item
                    }
                    else
                    {
                        ddlResourceName.Items.FindByText(Session["ResourceName"].ToString()).Selected = true;
                        calenderStartDate.SelectedDate = Convert.ToDateTime(Session["LeaveStart"].ToString());
                        calenderEndDate.SelectedDate = Convert.ToDateTime(Session["LeaveEnd"].ToString());
                    }

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }

        }

        protected void rgResourceLeaves_ItemCommand(object sender, GridCommandEventArgs e)
        {

            if (e.CommandName.Equals("PerformInsert"))
            {
                DropDownList comboResourceName = (DropDownList)e.Item.FindControl("comboResourceName");
                Telerik.Web.UI.RadDatePicker calenderStartDate = (Telerik.Web.UI.RadDatePicker)e.Item.FindControl("calenderStartDate");
                Telerik.Web.UI.RadDatePicker calenderEndDate = (Telerik.Web.UI.RadDatePicker)e.Item.FindControl("calenderEndDate");

                string sResourceID = comboResourceName.SelectedValue;
                DateTime dStartDate = (DateTime)calenderStartDate.SelectedDate;
                DateTime dEndDate = (DateTime)calenderEndDate.SelectedDate;

                string p_message1 = String.Empty;
                string p_message2 = String.Empty;
                objBLL = new MetisBLL();
                objBLL.CheckProjectAllocation(sResourceID, dStartDate, dEndDate, out p_message1);
                if (p_message1.Length == 0)
                {

                    objBLL.insertResourceLeave(sResourceID, dStartDate, dEndDate, out p_message2);
                }
                else
                {
                    objBLL.insertResourceLeave(sResourceID, dStartDate, dEndDate, out p_message2);
                }
                rgResourceLeaves.DataSource = objBLL.getAllResourceLeaves();
                rgResourceLeaves.DataBind();
            }

            if (e.CommandName.Equals("Edit"))
            {
                GridDataItem item = (GridDataItem)e.Item;

                string ResourceName = ((DataBoundLiteralControl)item.Controls[3].Controls[0]).Text.Trim();
                string LeaveStart = ((DataBoundLiteralControl)item.Controls[4].Controls[0]).Text.Trim();
                string LeaveEnd = ((DataBoundLiteralControl)item.Controls[5].Controls[0]).Text.Trim();

                Session["ResourceName"] = ResourceName;
                Session["LeaveStart"] = LeaveStart;
                Session["LeaveEnd"] = LeaveEnd;

            }


            if (e.CommandName.Equals("Update"))
            {

                //To Avoid DateRange Complexity First Delete and then Insert
                string p_message = "";
                MetisBLL objBll = new MetisBLL();
                objBll.deleteResourceLeave(Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ResourceLeaveHeaderID"].ToString()), out p_message);

                DropDownList comboResourceName = (DropDownList)e.Item.FindControl("comboResourceName");
                Telerik.Web.UI.RadDatePicker calenderStartDate = (Telerik.Web.UI.RadDatePicker)e.Item.FindControl("calenderStartDate");
                Telerik.Web.UI.RadDatePicker calenderEndDate = (Telerik.Web.UI.RadDatePicker)e.Item.FindControl("calenderEndDate");

                string sResourceID = comboResourceName.SelectedValue;
                DateTime dStartDate = (DateTime)calenderStartDate.SelectedDate;
                DateTime dEndDate = (DateTime)calenderEndDate.SelectedDate;


                string p_message1 = String.Empty;
                string p_message2 = String.Empty;
                objBll.CheckProjectAllocation(sResourceID, dStartDate, dEndDate, out p_message1);

                if (p_message1.Length == 0)
                {

                    objBll.insertResourceLeave(sResourceID, dStartDate, dEndDate, out p_message2);
                }
                else
                {
                    objBll.insertResourceLeave(sResourceID, dStartDate, dEndDate, out p_message2);
                }
                rgResourceLeaves.DataSource = objBll.getAllResourceLeaves();
                rgResourceLeaves.DataBind();

            }


            if (e.CommandName.Equals("Cancel"))
            {
                rgResourceLeaves.DataBind();
            }
        }

        protected void rgResourceLeaves_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {

            try
            {

                string p_message = "";
                MetisBLL objBll = new MetisBLL();
                objBll.deleteResourceLeave(Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ResourceLeaveHeaderID"].ToString()), out p_message);

            }
            catch (Exception ex)
            {
                rgResourceLeaves.Controls.Add(new LiteralControl("Unable to delete Resource Leaves. Reason: " + ex.Message));
                e.Canceled = true;
            }
        }
        #endregion

        #region UpComingProject

        protected void rgUpComingProject_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            objBLL = new MetisBLL();
            rgUpComingProject.DataSource = objBLL.getAllUpComingProject();
        }

        protected void rgUpComingProject_PreRender(object sender, EventArgs e)
        {

        }

        protected void rgUpComingProject_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
        }

        protected void rgUpComingProject_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {

        }

        protected void rgUpComingProject_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {





            }
        }

        protected void rgUpComingProject_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem && e.Item.IsInEditMode)
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    TextBox txtProject = (e.Item as GridEditableItem)["Project"].FindControl("txtProject") as TextBox;
                    Telerik.Web.UI.RadDatePicker DesiredStart = (e.Item as GridEditableItem)["DesiredStart"].FindControl("DesiredStart") as Telerik.Web.UI.RadDatePicker;
                    Telerik.Web.UI.RadDatePicker PlannedStart = (e.Item as GridEditableItem)["PlannedStart"].FindControl("PlannedStart") as Telerik.Web.UI.RadDatePicker;
                    TextBox txtResource = (e.Item as GridEditableItem)["Resources"].FindControl("txtResource") as TextBox;
                    TextBox txtComment = (e.Item as GridEditableItem)["Comments"].FindControl("txtComment") as TextBox;

                    if (e.Item is GridEditFormInsertItem || e.Item is GridDataInsertItem)
                    {
                        // insert item
                    }
                    else
                    {
                        txtProject.Text = Session["Project"].ToString();
                        DesiredStart.SelectedDate = Convert.ToDateTime(Session["DesiredStart"].ToString());
                        PlannedStart.SelectedDate = Convert.ToDateTime(Session["PlannedStart"].ToString());
                        txtResource.Text = Session["Resource"].ToString();
                        txtComment.Text = Session["Comment"].ToString();
                    }

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }

        }

        protected void rgUpComingProject_ItemCommand(object sender, GridCommandEventArgs e)
        {

            if (e.CommandName.Equals("PerformInsert"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                TextBox txtProject = (e.Item as GridEditableItem)["Project"].FindControl("txtProject") as TextBox;
                Telerik.Web.UI.RadDatePicker DesiredStart = (e.Item as GridEditableItem)["DesiredStart"].FindControl("DesiredStart") as Telerik.Web.UI.RadDatePicker;
                Telerik.Web.UI.RadDatePicker PlannedStart = (e.Item as GridEditableItem)["PlannedStart"].FindControl("PlannedStart") as Telerik.Web.UI.RadDatePicker;
                TextBox txtResource = (e.Item as GridEditableItem)["Resources"].FindControl("txtResource") as TextBox;
                TextBox txtComment = (e.Item as GridEditableItem)["Comments"].FindControl("txtComment") as TextBox;

                string project = txtProject.Text;
                DateTime dDesiredStart = (DateTime)DesiredStart.SelectedDate;
                DateTime dPlannedStart = (DateTime)PlannedStart.SelectedDate;
                string resource = txtResource.Text;
                string comment = txtComment.Text;
                string p_message1 = String.Empty;

                objBLL = new MetisBLL();
                //objBLL.insertUpComingProject(project, dDesiredStart, dPlannedStart, resource, comment,out p_message1);

                rgUpComingProject.DataSource = objBLL.getAllUpComingProject();
                rgUpComingProject.DataBind();
            }

            if (e.CommandName.Equals("Edit"))
            {
                GridDataItem item = (GridDataItem)e.Item;

                string project = ((DataBoundLiteralControl)item.Controls[3].Controls[0]).Text.Trim();
                string desiredStart = ((DataBoundLiteralControl)item.Controls[4].Controls[0]).Text.Trim();
                string plannedStart = ((DataBoundLiteralControl)item.Controls[5].Controls[0]).Text.Trim();
                string resource = ((DataBoundLiteralControl)item.Controls[6].Controls[0]).Text.Trim();
                string comment = ((DataBoundLiteralControl)item.Controls[7].Controls[0]).Text.Trim();

                Session["Project"] = project;
                Session["DesiredStart"] = desiredStart;
                Session["PlannedStart"] = plannedStart;
                Session["Resource"] = project;
                Session["Comment"] = project;
            }


            if (e.CommandName.Equals("Update"))
            {

                GridDataItem item = (GridDataItem)e.Item;

                TextBox txtProject = (e.Item as GridEditableItem)["Project"].FindControl("txtProject") as TextBox;
                Telerik.Web.UI.RadDatePicker DesiredStart = (e.Item as GridEditableItem)["DesiredStart"].FindControl("DesiredStart") as Telerik.Web.UI.RadDatePicker;
                Telerik.Web.UI.RadDatePicker PlannedStart = (e.Item as GridEditableItem)["PlannedStart"].FindControl("PlannedStart") as Telerik.Web.UI.RadDatePicker;
                TextBox txtResource = (e.Item as GridEditableItem)["Resources"].FindControl("txtResource") as TextBox;
                TextBox txtComment = (e.Item as GridEditableItem)["Comments"].FindControl("txtComment") as TextBox;

                string project = txtProject.Text;
                DateTime dDesiredStart = (DateTime)DesiredStart.SelectedDate;
                DateTime dPlannedStart = (DateTime)PlannedStart.SelectedDate;
                string resource = txtResource.Text;
                string comment = txtComment.Text;
                string p_message1 = String.Empty;

                objBLL = new MetisBLL();
                //objBLL.updateUpComingProject(Convert.ToInt32(item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["pkID"].ToString()), project, Convert.ToDateTime(dDesiredStart), Convert.ToDateTime(dPlannedStart), resource, comment, out p_message1);

                rgUpComingProject.DataSource = objBLL.getAllUpComingProject();
                rgUpComingProject.DataBind();

            }


            if (e.CommandName.Equals("Cancel"))
            {
                rgUpComingProject.DataBind();
            }
        }

        protected void rgUpComingProject_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {

            try
            {

                string p_message = "";
                MetisBLL objBll = new MetisBLL();
                objBll.deleteUpComingProject(Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["pkID"].ToString()), out p_message);

            }
            catch (Exception ex)
            {
                rgUpComingProject.Controls.Add(new LiteralControl("Unable to delete record. Reason: " + ex.Message));
                e.Canceled = true;
            }
        }
        #endregion

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

        #region "Assignment History"
        protected void rgAssignmentHistory_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem item = (GridDataItem)e.Item;
            try
            {

                string BulkAssignmentID = item.OwnerTableView.DataKeyValues[item.ItemIndex]["BulkAssignmentID"].ToString();
                //string WeekEnding = ((DataBoundLiteralControl)e.Item.Controls[3].Controls[0]).Text.Trim();
                //string ResourceName = ((DataBoundLiteralControl)e.Item.Controls[4].Controls[0]).Text.Trim();
                //string ProjectName = ((DataBoundLiteralControl)e.Item.Controls[5].Controls[0]).Text.Trim();
                //string BulkStartDate = ((DataBoundLiteralControl)e.Item.Controls[6].Controls[0]).Text.Trim();
                //string BulkEndDate = ((DataBoundLiteralControl)e.Item.Controls[7].Controls[0]).Text.Trim();
                //string WorkLoad = ((DataBoundLiteralControl)e.Item.Controls[8].Controls[0]).Text.Trim();

                string WeekEnding = ((DataBoundLiteralControl)item.Controls[3].Controls[0]).Text.Trim();
                string ResourceName = ((DataBoundLiteralControl)item.Controls[4].Controls[0]).Text.Trim();
                string ProjectName = ((DataBoundLiteralControl)item.Controls[5].Controls[0]).Text.Trim();
                string AssignmentTypeName = ((DataBoundLiteralControl)item.Controls[6].Controls[0]).Text.Trim();
                string BulkStartDate = ((DataBoundLiteralControl)item.Controls[7].Controls[0]).Text.Trim();
                string BulkEndDate = ((DataBoundLiteralControl)item.Controls[8].Controls[0]).Text.Trim();
                string WorkLoad = ((DataBoundLiteralControl)item.Controls[9].Controls[0]).Text.Trim();
                String assignmentTypeName = Session["AssignmentTypeName"].ToString();

                DateTime weekEnding = DateTime.Parse(WeekEnding);
                DateTime start_bulk = DateTime.Parse(BulkStartDate);
                DateTime end_bulk = DateTime.Parse(BulkEndDate);
                double work_load = double.Parse(WorkLoad);

                //DateTime temp_start = start_bulk;
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

                string p_message = "";
                MetisBLL objBll = new MetisBLL();
                /*objBll.deleteBulkAssignment(ResourceName, ProjectName, AssignmentTypeName, start, end, weekEnding, start_bulk, end_bulk, out p_message);*/
                objBll.deleteBulkAssignment(ResourceName, ProjectName, start, end, weekEnding, start_bulk, end_bulk, assignmentTypeName, out p_message);

                //#region format

                #region "Old"
                //DateTime temp_start = start_bulk;
                //DateTime temp_prev = start_bulk;
                //DateTime temp_end = end_bulk;

                //////
                //DayOfWeek day = start_bulk.DayOfWeek;
                //int days = day - DayOfWeek.Saturday;
                //DateTime start = temp_prev.AddDays(-days);
                /////////////
                //DayOfWeek day_end = end_bulk.DayOfWeek;
                //int days_end = day_end - DayOfWeek.Saturday;
                //DateTime end = temp_end.AddDays(-days_end);
                ////////////////

                //int nOfDays = 5, nOfDays_2 = 1;

                //days = 6 + days;
                //days_end = 6 + days_end;

                //if (days == 6)
                //    days = nOfDays_2;
                //if (days_end == 6)
                //    days_end = days_end - nOfDays_2;

                //#endregion


                //string p_message = "";
                //objBLL = new MetisBLL();
                //objBLL.deleteBulkAssignment(BulkAssignmentID, out p_message);

                ////string QryDelete = string.Format("DELETE FROM [ProMan].[dbo].[Weekly_Reports] where id='{0}'", BulkAssignmentID);
                ////int rowsInserted = SQLDataConnection.GetInstance().ExecuteQuery(QryDelete);

                //string[] week_begin = new string[7];
                //string[] week_last = new string[7];

                //#region Bulk erase Query
                //while (end_bulk.Date > temp_start.Date)
                //{

                //    if (temp_start.Date == start_bulk.Date)
                //    {

                //        while (nOfDays >= days)
                //        {

                //            week_begin[days] = "";//work_load.ToString();
                //            days++;
                //        }

                //        objBLL.deleteBulkAssignment(ResourceName, ProjectName, weekEnding, out p_message);
                //        //string QryErase_bulk_being = string.Format(Settings.Default.qryDeleteBulk, newValues["Resource_id"], newValues["Project_id"], start.Date);
                //        //rowsInserted = SQLDataConnection.GetInstance().ExecuteQuery(QryErase_bulk_being);

                //        while (days_end >= nOfDays_2)
                //        {

                //            week_last[days_end] = "";//work_load.ToString();
                //            days_end--;
                //        }
                //        if (start.Date != end.Date)
                //        {

                //            objBLL.deleteBulkAssignment(ResourceName, ProjectName, end.Date, out p_message);
                //            //string QryErase_bulk_last = string.Format(Settings.Default.qryDeleteBulk, newValues["Resource_id"], newValues["Project_id"], end.Date);
                //            //rowsInserted = SQLDataConnection.GetInstance().ExecuteQuery(QryErase_bulk_last);
                //        }
                //    }
                //    else if (temp_start.DayOfWeek == DayOfWeek.Saturday && temp_start.Date != start.Date && temp_start.Date != end.Date)
                //    {

                //        objBLL.deleteBulkAssignment(ResourceName, ProjectName, temp_start.Date, out p_message);
                //        //string QryFormat_bulk = string.Format(Settings.Default.qryDeleteBulk, currentRow.SavedOldValues["Resource_id"], currentRow.SavedOldValues["Project_id"], temp_start.Date);
                //        //rowsInserted = SQLDataConnection.GetInstance().ExecuteQuery(QryFormat_bulk);
                //    }


                //    int daysInMonth = DateTime.DaysInMonth(temp_start.Year, temp_start.Month);
                //    if (temp_start.Day == daysInMonth)
                //        temp_start.AddMonths(1);
                //    temp_start = temp_start.AddDays(1);

                //}
                //#endregion

                //RadGrid_bulk.DataSource = SQLDataConnection.GetInstance().ExecuteDataSet(Settings.Default.qryBulk_assignment);
                //RadGrid_bulk.Rebind();

                #endregion


            }
            catch (Exception ex)
            {
                rgAssignmentHistory.Controls.Add(new LiteralControl("Unable to delete Assignment. Reason: " + ex.Message));
                e.Canceled = true;
            }
        }
        // EMPTY
        protected void rgAssignmentHistory_InsertCommand(object sender, GridCommandEventArgs e)
        {

        }

        protected void rgAssignmentHistory_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("PerformInsert"))
            {
                DropDownList comboResourceName = (DropDownList)e.Item.FindControl("comboResourceName");
                DropDownList comboProjectName = (DropDownList)e.Item.FindControl("comboProjectName");
                DropDownList comboAssignmentTypeName = (DropDownList)e.Item.FindControl("comboAssignmentTypeName");
                Telerik.Web.UI.RadDatePicker calenderStartDate = (Telerik.Web.UI.RadDatePicker)e.Item.FindControl("calenderStartDate");
                Telerik.Web.UI.RadDatePicker calenderEndDate = (Telerik.Web.UI.RadDatePicker)e.Item.FindControl("calenderEndDate");
                Telerik.Web.UI.RadDatePicker calenderWeekEnding = (Telerik.Web.UI.RadDatePicker)e.Item.FindControl("calenderWeekEnding");
                TextBox txtWorkLoad = (TextBox)e.Item.FindControl("txtWorkLoad");

                DateTime dWeekEnding = (DateTime)calenderWeekEnding.SelectedDate;
                string sResourceID = comboResourceName.SelectedValue;
                string sProjectID = comboProjectName.SelectedValue;
                string sAssignmentTypeName = comboAssignmentTypeName.SelectedValue;
                DateTime dStartDate = (DateTime)calenderStartDate.SelectedDate;
                DateTime dEndDate = (DateTime)calenderEndDate.SelectedDate;
                float fWorkLoad = float.Parse(txtWorkLoad.Text);

                BulkInsert(e, dWeekEnding, sResourceID, sProjectID, dStartDate, dEndDate, fWorkLoad, sAssignmentTypeName);

                //rgBulkAssignment.DataSource = objBLL.getAllBulkAssignments();
                //rgBulkAssignment.DataBind();
                rgBulkAssignment.Rebind();
            }
            if (e.CommandName.Equals("Edit"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                string WeekEnding = ((DataBoundLiteralControl)item.Controls[3].Controls[0]).Text.Trim();
                string ResourceName = ((DataBoundLiteralControl)item.Controls[4].Controls[0]).Text.Trim();
                string ProjectName = ((DataBoundLiteralControl)item.Controls[5].Controls[0]).Text.Trim();
                string BulkStartDate = ((DataBoundLiteralControl)item.Controls[6].Controls[0]).Text.Trim();
                string BulkEndDate = ((DataBoundLiteralControl)item.Controls[7].Controls[0]).Text.Trim();
                string WorkLoad = ((DataBoundLiteralControl)item.Controls[8].Controls[0]).Text.Trim();
                //Session["DepartmentID"] = DepartmentID;
                Session["BulkAssignmentID"] = item.OwnerTableView.DataKeyValues[item.ItemIndex]["BulkAssignmentID"].ToString();
                Session["WeekEnding"] = WeekEnding;
                Session["ResourceName"] = ResourceName;

                Session["ProjectName"] = ProjectName;
                Session["BulkStartDate"] = BulkStartDate;
                Session["BulkEndDate"] = BulkEndDate;
                Session["WorkLoad"] = WorkLoad;

                rgAssignmentHistory.DataBind();
            }
            if (e.CommandName.Equals("Update"))
            {

                /*First Delete and then Insert*/

                DateTime weekEnding = DateTime.Parse(Session["WeekEnding"].ToString());
                DateTime start_bulk = DateTime.Parse(Session["BulkStartDate"].ToString());
                DateTime end_bulk = DateTime.Parse(Session["BulkEndDate"].ToString());
                double work_load = double.Parse(Session["WorkLoad"].ToString());
                String assignmentTypeName = Session["AssignmentTypeName"].ToString();

                //DateTime temp_start = start_bulk;
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
                
                string p_message = "";
                MetisBLL objBll = new MetisBLL();
/*                objBll.deleteBulkAssignment(Session["ResourceName"].ToString(), Session["ProjectName"].ToString(), Session["AssignmentTypeName"].ToString(), start, end, weekEnding, start_bulk, end_bulk, out p_message);*/
                objBll.deleteBulkAssignment(Session["ResourceName"].ToString(), Session["ProjectName"].ToString(),  start, end, weekEnding, start_bulk, end_bulk, assignmentTypeName, out p_message);

                Telerik.Web.UI.RadDatePicker calenderWeekEnding = (Telerik.Web.UI.RadDatePicker)e.Item.FindControl("calenderWeekEnding");
                DropDownList comboResourceName = (DropDownList)e.Item.FindControl("comboResourceName");
                DropDownList comboProjectName = (DropDownList)e.Item.FindControl("comboProjectName");
                DropDownList comboAssignmentTypeName = (DropDownList)e.Item.FindControl("comboAssignmentTypeName");
                Telerik.Web.UI.RadDatePicker calenderStartDate = (Telerik.Web.UI.RadDatePicker)e.Item.FindControl("calenderStartDate");
                Telerik.Web.UI.RadDatePicker calenderEndDate = (Telerik.Web.UI.RadDatePicker)e.Item.FindControl("calenderEndDate");
                string ResourceAssignmentID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BulkAssignmentID"].ToString();
                TextBox txtWorkLoad = (TextBox)e.Item.FindControl("txtWorkLoad");

                DateTime dWeekEnding = (DateTime)calenderWeekEnding.SelectedDate;
                string sResourceID = comboResourceName.SelectedValue;
                string sProjectID = comboProjectName.SelectedValue;
                string sAssignmentTypeID = comboAssignmentTypeName.SelectedValue;
                DateTime dStartDate = (DateTime)calenderStartDate.SelectedDate;
                DateTime dEndDate = (DateTime)calenderEndDate.SelectedDate;
                float fWorkLoad = float.Parse(txtWorkLoad.Text);

                BulkInsert(e, dWeekEnding, sResourceID, sProjectID, dStartDate, dEndDate, fWorkLoad, sAssignmentTypeID);

                //Clear Session
                Session["BulkAssignmentID"] = null;
                Session["ResourceName"] = null;
                Session["ProjectName"] = null;
                Session["BulkStartDate"] = null;
                Session["BulkEndDate"] = null;
                Session["WorkLoad"] = null;
                Session["WeekEnding"] = null;

                //rgBulkAssignment.DataSource = objBLL.getAllBulkAssignments();
                //rgBulkAssignment.DataBind();
                rgAssignmentHistory.Rebind();
            }
            if (e.CommandName.Equals("Cancel"))
            {
                //Clear Session
                Session["BulkAssignmentID"] = null;
                Session["ResourceName"] = null;
                Session["ProjectName"] = null;
                Session["BulkStartDate"] = null;
                Session["BulkEndDate"] = null;
                Session["WorkLoad"] = null;
                Session["WeekEnding"] = null;
                rgAssignmentHistory.Rebind();
            }
        }

        protected void rgAssignmentHistory_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {

                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {
                    //Telerik.Web.UI.RadComboBox comResourceName = (e.Item as GridEditableItem)["ResourceName"].Controls[1] as Telerik.Web.UI.RadComboBox;
                    //comResourceName.Focus();
                    ////ddlResourceName.AutoPostBack = true;
                    //comResourceName.ItemChecked += new RadComboBoxItemEventHandler(comResourceName_ItemChecked);
                    DropDownList comResourceName = (e.Item as GridEditableItem)["ResourceName"].Controls[1] as DropDownList;
                    comResourceName.Focus();

                    DropDownList ddlProjectName = (e.Item as GridEditableItem)["ProjectName"].Controls[1] as DropDownList;
                    RadAjaxManager1.AjaxSettings.AddAjaxSetting(comResourceName, ddlProjectName);

                    Telerik.Web.UI.RadDatePicker calenderWeekEnding = (e.Item as GridEditableItem)["WeekEnding"].FindControl("calenderWeekEnding") as Telerik.Web.UI.RadDatePicker;
                    calenderWeekEnding.SelectedDate = DateTime.Now;

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }
        }

        protected void rgAssignmentHistory_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem && e.Item.IsInEditMode)
                {
                    GridDataItem item = (GridDataItem)e.Item;

                    Telerik.Web.UI.RadDatePicker calenderWeekEnding = (e.Item as GridEditableItem)["WeekEnding"].FindControl("calenderWeekEnding") as Telerik.Web.UI.RadDatePicker;

                    DropDownList ddlResourceName = (e.Item as GridEditableItem)["ResourceName"].FindControl("comboResourceName") as DropDownList;
                    objBLL = new MetisBLL();
                    DataTable dt = new DataTable();
                    dt = objBLL.getAllResources(Session["user"].ToString());
                    ddlResourceName.DataSource = dt;
                    ddlResourceName.DataTextField = dt.Columns[1].ToString();
                    ddlResourceName.DataValueField = dt.Columns[0].ToString();
                    ddlResourceName.DataBind();
                    ddlResourceName.Width = Unit.Pixel(240); // Set the width  
                    ddlResourceName.Focus();

                    if (Session["ResourceName"] != null)
                        ddlResourceName.Items.FindByText(Session["ResourceName"].ToString()).Selected = true;

                    DropDownList ddlDepartmentName = (e.Item as GridEditableItem)["ProjectName"].FindControl("comboProjectName") as DropDownList;
                    objBLL = new MetisBLL();

                    dt = objBLL.getProject(ddlResourceName.SelectedValue);
                    ddlDepartmentName.DataSource = dt;
                    ddlDepartmentName.DataTextField = dt.Columns[1].ToString();
                    ddlDepartmentName.DataValueField = dt.Columns[0].ToString();
                    ddlDepartmentName.DataBind();
                    ddlDepartmentName.Width = Unit.Pixel(240); // Set the width  


                    Telerik.Web.UI.RadDatePicker calenderStartDate = (e.Item as GridEditableItem)["BulkStartDate"].FindControl("calenderStartDate") as Telerik.Web.UI.RadDatePicker;
                    Telerik.Web.UI.RadDatePicker calenderEndDate = (e.Item as GridEditableItem)["BulkEndDate"].FindControl("calenderEndDate") as Telerik.Web.UI.RadDatePicker;
                    TextBox txtWorkLoad = (e.Item as GridEditableItem)["BulkWorkLoad"].FindControl("txtWorkLoad") as TextBox;

                    if (Session["WeekEnding"] != null)
                    {
                        calenderWeekEnding.SelectedDate = Convert.ToDateTime(Session["WeekEnding"].ToString());
                        ddlResourceName.Items.FindByText(Session["ResourceName"].ToString()).Selected = true;
                        ddlDepartmentName.Items.FindByText(Session["ProjectName"].ToString()).Selected = true;
                        calenderStartDate.SelectedDate = Convert.ToDateTime(Session["BulkStartDate"].ToString());
                        calenderEndDate.SelectedDate = Convert.ToDateTime(Session["BulkEndDate"].ToString());
                        txtWorkLoad.Text = (Session["WorkLoad"] == null ? "" : Session["WorkLoad"]).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }
        }

        protected void rgAssignmentHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            objBLL = new MetisBLL();
            rgAssignmentHistory.DataSource = objBLL.getAllBulkAssignmentsHistory(Session["user"].ToString());
        }
        // EMPTY
        protected void rgAssignmentHistory_UpdateCommand(object sender, GridCommandEventArgs e)
        {

        }

        #endregion 

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {

            if (CheckBox1.Checked)
            {
                objBLL = new MetisBLL();
                rgAssignmentHistory.DataSource = objBLL.getAllBulkAssignmentsHistory(Session["user"].ToString());
              //divAssignmentHistory.Attributes.Remove("style");
               divAssignmentHistory.Attributes.Add("style","visibility:visible;");
             
               
                
                
            }
            else if (!CheckBox1.Checked)
            {
                //Response.Redirect("Assignments.aspx");

                divAssignmentHistory.Attributes.Add("style", "display:none;");
               
                
            }
            
        }

        DateTime NextSaturday(DateTime now)
        {
            while (now.DayOfWeek != DayOfWeek.Saturday)
                now = now.AddDays(1);
            return now;
        }
    }
}