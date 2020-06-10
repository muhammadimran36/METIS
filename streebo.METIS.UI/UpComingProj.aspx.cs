using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using streebo.METIS.BLL;
using Telerik.Web.UI;
using Telerik.Web.UI.Skins;
using System.Collections;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Drawing;
using Telerik.Web.UI.GridExcelBuilder;
using System.Globalization;


namespace streebo.METIS.UI
{
    public partial class UpComingProj : System.Web.UI.Page
    {
        private MetisBLL objBLL;
        private Dictionary<String, String> distinctEmails;
        private Dictionary<String, String> emailBodyList;
        private Boolean b_CanView;
        private readonly DepartmentManager depManager = DepartmentManager.Instance;
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!IsPostBack)
        //    {

        //        objBLL = new MetisBLL();
        //        DataTable dt = objBLL.getDeparments();
        //        ddlDepartment.DataSource = dt;
        //        ddlDepartment.DataTextField = dt.Columns[1].ToString();
        //        ddlDepartment.DataValueField = dt.Columns[0].ToString();
        //        ddlDepartment.DataBind();
        //        ddlDepartment.SelectedValue = "dpt_02";

        //        dt = new DataTable();
        //        dt = objBLL.getAllActionItem();
        //        DataView dv = new DataView(dt);
        //        dv.RowFilter = "Dept_name like '%" + (ddlDepartment.SelectedItem.ToString().Replace("'", "''") == "All" ? "" : ddlDepartment.SelectedItem.ToString().Replace("'", "''")) + "%'";
        //        rgActionItem.DataSource = dv;
        //        rgActionItem.DataBind();

        //        dt = new DataTable();
        //        dt = objBLL.getAllUpComingProject();
        //        dv = new DataView(dt);
        //        dv.RowFilter = "Dept_name like '%" + (ddlDepartment.SelectedItem.ToString().Replace("'", "''") == "All" ? "" : ddlDepartment.SelectedItem.ToString().Replace("'", "''")) + "%'";
        //        rgUpComingProject.DataSource = dv;



        //    }
        //}

          protected void Page_Load(object sender, EventArgs e)
        {
            //int x = Convert.ToInt32(Session["isLogin"]);

            //if (x == 0)
            //    RFSs.Visible = false;
            //else
            //    RFSs.Visible = true;   

            if (!IsPostBack)
            {
                #region Check Login
               
                    if (Convert.ToString(Session["user"]) == "")
                        Response.Redirect("Login.aspx");
                #endregion
                #region Check Rights
                b_CanView = false;

                    objBLL = new MetisBLL();
                    System.Data.DataTable dt = objBLL.getAccessRights(Convert.ToString(Session["user"]));
                    foreach (System.Data.DataRow row in dt.Rows)
                    {
                        if (row["EntityName"].ToString() == "UpcomingProj") { b_CanView = Convert.ToBoolean(row["Can_View"]); }
                    }
                  //  If Admin thn bypass security
                    if (b_CanView == false)
                    {
                        objBLL = new MetisBLL();
                        if (Convert.ToBoolean(objBLL.IsAdmin(Convert.ToString(Session["user"]))))
                            b_CanView = true;
                    }
                  #endregion
                    if (b_CanView)
                    {
                        objBLL = new MetisBLL();
                        dt = depManager.getDeparments();
                        ddlDepartment.DataSource = dt;
                        ddlDepartment.DataTextField = dt.Columns[1].ToString();
                        ddlDepartment.DataValueField = dt.Columns[0].ToString();
                        ddlDepartment.DataBind();
                        ddlDepartment.SelectedValue = "dpt_01"; // IM - SSL By Default
                        if (Session["ddlDepartment"] != null)
                        {
                            ddlDepartment.SelectedValue = Session["ddlDepartment"].ToString();
                        }
                        Session["ddlDepartment"] = ddlDepartment.SelectedValue;
                        

                        dt = new DataTable();
                        dt = objBLL.getAllActionItem();
                        DataView dv = new DataView(dt);
                        dv.RowFilter = "Dept_name like '%" + (ddlDepartment.SelectedItem.ToString().Replace("'", "''") == "All" ? "" : ddlDepartment.SelectedItem.ToString().Replace("'", "''")) + "%'";
                        rgActionItem.DataSource = dv;
                        rgActionItem.DataBind();

                        dt = new DataTable();
                        dt = objBLL.getAllUpComingProject();
                        dv = new DataView(dt);
                        dv.RowFilter = "Dept_name like '%" + (ddlDepartment.SelectedItem.ToString().Replace("'", "''") == "All" ? "" : ddlDepartment.SelectedItem.ToString().Replace("'", "''")) + "%'";
                        rgUpComingProject.DataSource = dv;

                    }
                    else
                    {
                        main.Visible = false;
                        lblErr.Text = "403 Forbidden";
                        
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

        #region UpComingProject

        protected void rgUpComingProject_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            
                objBLL = new MetisBLL();
                DataTable dt = new DataTable();
                dt = objBLL.getAllUpComingProject();
                DataView dv = new DataView(dt);
                dv.RowFilter = "Dept_name like '%" + (ddlDepartment.SelectedItem.ToString().Replace("'", "''") == "All" ? "" : ddlDepartment.SelectedItem.ToString().Replace("'", "''")) + "%'";
                rgUpComingProject.DataSource = dv;
            
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
                objBLL.insertUpComingProject(project, dDesiredStart, dPlannedStart, resource, comment, ddlDepartment.SelectedValue, out p_message1);


                rgUpComingProject.Rebind();
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
                Session["Resource"] = resource;
                Session["Comment"] = comment;
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
                objBLL.updateUpComingProject(Convert.ToInt32(item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["pkID"].ToString()), project, Convert.ToDateTime(dDesiredStart), Convert.ToDateTime(dPlannedStart), resource, comment, ddlDepartment.SelectedValue, out p_message1);


                rgUpComingProject.Rebind();

            }


            if (e.CommandName.Equals("Cancel"))
            {
                rgUpComingProject.Rebind();
            }
        }

        protected void rgUpComingProject_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {

            try
            {

                string p_message = "";
                MetisBLL objBll = new MetisBLL();
                objBll.deleteUpComingProject(Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["pkID"].ToString()), out p_message);
                rgUpComingProject.Rebind();
            }
            catch (Exception ex)
            {
                rgUpComingProject.Controls.Add(new LiteralControl("Unable to delete record. Reason: " + ex.Message));
                e.Canceled = true;
            }
        }
        #endregion

        #region ActionItem

        protected void rgActionItem_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
           
                objBLL = new MetisBLL();
                DataTable dt = new DataTable();

                if (chkbArchive.Checked)
                    dt = objBLL.getAllArchiveActionItem();
                else
                    dt = objBLL.getAllActionItem();
                DataView dv = new DataView(dt);
                dv.RowFilter = "Dept_name like '%" + (ddlDepartment.SelectedItem.ToString().Replace("'", "''") == "All" ? "" : ddlDepartment.SelectedItem.ToString().Replace("'", "''")) + "%'";
                rgActionItem.DataSource = dv;
           
        }

        protected void rgActionItem_PreRender(object sender, EventArgs e)
        {

        }

        protected void rgActionItem_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
        }

        protected void rgActionItem_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {

        }

        protected void rgActionItem_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {





            }
            else
            {
                GridDataItem item = e.Item as GridDataItem;
                if (item != null)
                {
                    GridButtonColumn gBtnCol = item.OwnerTableView.GetColumn("ArchiveColumn") as GridButtonColumn;

                    ImageButton ImgBtn = item["ArchiveColumn"].Controls[0] as ImageButton;
                    if (chkbArchive.Checked)
                    {
                        ImgBtn.ImageUrl = "images/show.png";
                        ImgBtn.CommandName = "unArchive";
                        gBtnCol.ConfirmText = "Are you sure you want to UnArchive?";
                        gBtnCol.ConfirmTitle = "UnArchive";
                        ImgBtn.ToolTip = "UnArchive";
                    }
                    else
                    {
                        ImgBtn.ImageUrl = "images/hide.png";
                        ImgBtn.CommandName = "Archive";
                        gBtnCol.ConfirmText = "Are you sure you want to Archive?";
                        gBtnCol.ConfirmTitle = "Archive";
                        ImgBtn.ToolTip = "Archive";
                    }

                }
            }
        }

        protected void rgActionItem_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem && e.Item.IsInEditMode)
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    TextBox txtActionItem = (e.Item as GridEditableItem)["ActionItem"].FindControl("txtActionItem") as TextBox;
                    DropDownList ddlResourceName = (e.Item as GridEditableItem)["Resource_name"].FindControl("comboResourceName") as DropDownList;
                    objBLL = new MetisBLL();
                    ddlResourceName.DataSource = objBLL.getAllResources(Session["user"].ToString());
                    ddlResourceName.DataTextField = objBLL.getAllResources(Session["user"].ToString()).Columns[1].ToString();
                    ddlResourceName.DataValueField = objBLL.getAllResources(Session["user"].ToString()).Columns[0].ToString();
                    ddlResourceName.DataBind();
                    ddlResourceName.Width = Unit.Pixel(240); // Set the width  
                    ddlResourceName.Focus();
                    Telerik.Web.UI.RadDatePicker Target = (e.Item as GridEditableItem)["Target"].FindControl("Target") as Telerik.Web.UI.RadDatePicker;
                    TextBox txtStatus = (e.Item as GridEditableItem)["Status"].FindControl("txtStatus") as TextBox;
                    if (e.Item is GridEditFormInsertItem || e.Item is GridDataInsertItem)
                    {

                    }
                    else
                    {
                        txtActionItem.Text = Session["ActionItem"].ToString();
                        ddlResourceName.Items.FindByText(Session["Resource_name"].ToString()).Selected = true;
                        Target.SelectedDate = Convert.ToDateTime(Session["Target"].ToString());
                        txtStatus.Text = Session["Status"].ToString();
                    }

                }
                else
                {
                    //GridDataItem item = e.Item as GridDataItem;
                    //if (item != null)
                    //{
                    //    GridButtonColumn gBtnCol = item.OwnerTableView.GetColumn("ArchiveColumn") as GridButtonColumn;

                    //    ImageButton ImgBtn = item["ArchiveColumn"].Controls[0] as ImageButton;
                    //    if (chkbArchive.Checked)
                    //    {
                    //        ImgBtn.ImageUrl = "images/show.png";
                    //        ImgBtn.CommandName = "unArchive";
                    //        gBtnCol.ConfirmText = "Are you sure you want to UnArchive?";
                    //        gBtnCol.ConfirmTitle = "UnArchive";
                    //        ImgBtn.ToolTip = "UnArchive";
                    //    }
                    //    else
                    //    {
                    //        ImgBtn.ImageUrl = "images/hide.png";
                    //        ImgBtn.CommandName = "Archive";
                    //        gBtnCol.ConfirmText = "Are you sure you want to Archive?";
                    //        gBtnCol.ConfirmTitle = "Archive";
                    //        ImgBtn.ToolTip = "Archive";
                    //    }

                    //}
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "validation", "alert('" + ex.Message + "')", true);

            }

        }

        protected void rgActionItem_ItemCommand(object sender, GridCommandEventArgs e)
        {

            if (e.CommandName.Equals("PerformInsert"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                TextBox txtActionItem = (e.Item as GridEditableItem)["ActionItem"].FindControl("txtActionItem") as TextBox;
                DropDownList ddlResourceName = (e.Item as GridEditableItem)["Resource_name"].FindControl("comboResourceName") as DropDownList;
                Telerik.Web.UI.RadDatePicker Target = (e.Item as GridEditableItem)["Target"].FindControl("Target") as Telerik.Web.UI.RadDatePicker;
                TextBox txtStatus = (e.Item as GridEditableItem)["Status"].FindControl("txtStatus") as TextBox;

                string ActionItem = txtActionItem.Text;
                string ResourceID = ddlResourceName.SelectedValue;
                DateTime DTarget = (DateTime)Target.SelectedDate;
                string Status = txtStatus.Text;
                string p_message1 = String.Empty;

                objBLL = new MetisBLL();
                objBLL.insertActionItem(ActionItem, ResourceID, DTarget, Status, ddlDepartment.SelectedValue, out p_message1);

                rgActionItem.Rebind();
            }

            if (e.CommandName.Equals("Edit"))
            {
                GridDataItem item = (GridDataItem)e.Item;

                string ActionItem = ((DataBoundLiteralControl)item.Controls[3].Controls[0]).Text.Trim();
                string Resource = ((DataBoundLiteralControl)item.Controls[4].Controls[0]).Text.Trim();
                string Target = ((DataBoundLiteralControl)item.Controls[5].Controls[0]).Text.Trim();
                string Status = ((DataBoundLiteralControl)item.Controls[6].Controls[0]).Text.Trim();


                Session["ActionItem"] = ActionItem;
                Session["Resource_name"] = Resource;
                Session["Target"] = Target;
                Session["Status"] = Status;

            }


            if (e.CommandName.Equals("Update"))
            {

                GridDataItem item = (GridDataItem)e.Item;
                TextBox txtActionItem = (e.Item as GridEditableItem)["ActionItem"].FindControl("txtActionItem") as TextBox;
                DropDownList ddlResourceName = (e.Item as GridEditableItem)["Resource_name"].FindControl("comboResourceName") as DropDownList;
                Telerik.Web.UI.RadDatePicker Target = (e.Item as GridEditableItem)["Target"].FindControl("Target") as Telerik.Web.UI.RadDatePicker;
                TextBox txtStatus = (e.Item as GridEditableItem)["Status"].FindControl("txtStatus") as TextBox;

                string ActionItem = txtActionItem.Text;
                string ResourceID = ddlResourceName.SelectedValue;
                DateTime DTarget = (DateTime)Target.SelectedDate;
                string Status = txtStatus.Text;
                string p_message1 = String.Empty;

                objBLL = new MetisBLL();
                objBLL.updateActionItem(Convert.ToInt32(item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["pkID"].ToString()), ActionItem, ResourceID, DTarget, Status, ddlDepartment.SelectedValue, out p_message1);

                rgActionItem.Rebind();

            }


            if (e.CommandName.Equals("Cancel"))
            {
                rgActionItem.DataBind();
            }
            if (e.CommandName.Equals("Archive"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                string p_message1 = String.Empty;
                objBLL = new MetisBLL();
                objBLL.archiveActionItem(int.Parse(item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["pkID"].ToString()), out p_message1);
                rgActionItem.Rebind();
            }
            if (e.CommandName.Equals("unArchive"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                string p_message1 = String.Empty;
                objBLL = new MetisBLL();
                objBLL.unArchiveActionItem(int.Parse(item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["pkID"].ToString()), out p_message1);
                rgActionItem.Rebind();
            }
        }

        protected void rgActionItem_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {

            try
            {

                string p_message = "";
                MetisBLL objBll = new MetisBLL();
                objBll.deleteActionItem(Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["pkID"].ToString()), out p_message);
                rgActionItem.Rebind();
            }
            catch (Exception ex)
            {
                rgUpComingProject.Controls.Add(new LiteralControl("Unable to delete record. Reason: " + ex.Message));
                e.Canceled = true;
            }
        }
        #endregion

        protected void ddlDepartment_OnSelectedIndexChanged(object sender, EventArgs e)
        {

            rgUpComingProject.Rebind();
            rgActionItem.Rebind();

            Session["ddlDepartment"] = ddlDepartment.SelectedValue;
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

        protected void isArchive_CheckedChanged(object sender, EventArgs e)
        {
            rgUpComingProject.Rebind();
        }

        public string FindKeyByValue<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TValue value)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
                if (value.Equals(pair.Value)) return pair.Key.ToString();

            return "";

        }

        public void SendMail(string smtpAddress, string from, string to, string cc, string replyTo, string body, string subject, bool isHtml)
        {

            MailMessage email = new MailMessage();
            email.From = new MailAddress("metis.streebo@gmail.com", "Streebo Metis");
            if (replyTo != String.Empty)
                email.ReplyTo = new MailAddress(replyTo);

            Dictionary<string, string> lstResourceWithEmail = new Dictionary<string, string>();

            objBLL = new MetisBLL();
            DataTable dtable = objBLL.getAllResourcesWithEmail();

            email.To.Add(to);

            if (cc != String.Empty)
                email.CC.Add(cc);

            email.Body = body;
            email.Subject = subject;
            email.IsBodyHtml = isHtml;
            //email.Attachments.Add(new Attachment(attachmentStream, fileName));

            SmtpClient smtp = new SmtpClient("localhost", 587);
            smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address  
            smtp.Credentials = new System.Net.NetworkCredential
                 ("metis.streebo@gmail.com", "Inbox@1234");
            //Or your Smtp Email ID and Password  
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;
            smtp.Send(email);

        }

        protected void btnEmail_Click(object sender, ImageClickEventArgs e)
        {

            objBLL = new MetisBLL();
            DataTable dtActionItem = new DataTable();
            DataTable dtDistinctResourceEmail = new DataTable();
            if (chkbArchive.Checked)
                dtActionItem = objBLL.getAllArchiveActionItem();
            else
                dtActionItem = objBLL.getAllActionItem();
            dtDistinctResourceEmail = objBLL.getAllActionItemsResourceEmail(); // get distinct email
            DataView dv = new DataView(dtActionItem);
            DataView dv2 = new DataView(dtDistinctResourceEmail);
            GridColumn gridResourceColumn = rgActionItem.MasterTableView.GetColumnSafe("Resource_name");
            GridColumn gridActionItemColumn = rgActionItem.MasterTableView.GetColumnSafe("ActionItem");
            dv.RowFilter = "Dept_name like '%" + (ddlDepartment.SelectedItem.ToString().Replace("'", "''") == "All" ? "" : ddlDepartment.SelectedItem.ToString().Replace("'", "''")) + "%'" +
                " AND Resource_name like '%" + (gridResourceColumn.CurrentFilterValue.ToString().Replace("'", "''") == String.Empty ? "" : gridResourceColumn.CurrentFilterValue.ToString().Replace("'", "''")) + "%'" +
                 " AND Action_Item like '%" + (gridActionItemColumn.CurrentFilterValue.ToString().Replace("'", "''") == String.Empty ? "" : gridActionItemColumn.CurrentFilterValue.ToString().Replace("'", "''")) + "%'";
            dv2.RowFilter = "Dept_name like '%" + (ddlDepartment.SelectedItem.ToString().Replace("'", "''") == "All" ? "" : ddlDepartment.SelectedItem.ToString().Replace("'", "''")) + "%'";

            //Storing ResourceName and Email in Dictionary
            mapDataTableToDictionary(dv2);
            /// loop through DV (DataView) and transform datarows to html Table
            foreach (DataRowView drv in dv)
            {
                transformRowsToHTMLTable(drv);
            }
            string cc = ConfigurationSettings.AppSettings["ActionItemsCC"];
            string from = ConfigurationSettings.AppSettings["ActionItemsFrom"];
            string replyto = ConfigurationSettings.AppSettings["ActionItemsReplyto"];
            for (int i = 0; i < emailBodyList.Count; i++)
                SendMail("smtp.gmail.com", from, emailBodyList.ElementAt(i).Key + "@streebo.com", cc, replyto, "<b>To do List</b><br/>" + emailBodyList.ElementAt(i).Value + "</table>", "Your Action Items", true);
            
        }

        private void mapDataTableToDictionary(DataView dv)
        {
            distinctEmails = new Dictionary<string, string>();

            foreach (DataRowView drv in dv)
            {
                distinctEmails.Add(drv.Row["Resource_name"].ToString(), drv.Row["Email"].ToString());
            }
        }

        private void transformRowsToHTMLTable(DataRowView drv)
        {
            if (emailBodyList == null)
                emailBodyList = new Dictionary<string, string>();

            if (!emailBodyList.ContainsKey(getEmailAgainstName(drv.Row["Resource_name"].ToString())))
            {
                emailBodyList.Add(getEmailAgainstName(drv.Row["Resource_name"].ToString()), "<table  border='1' style='border:1px dotted black;width:90%;border-collapse:collapse;'>" +
                  "<tr  style='background-color:#85C2E0;color:white;'>" +
                  "<th style='width:50%;padding:3px;'>Action Items</th><th style='padding:3px;'>Assign To</th><th style='padding:3px;'>Target Date</th><th style='padding:3px;'>Status</th>" +
                  "</tr>");

            }

            emailBodyList[getEmailAgainstName(drv.Row["Resource_name"].ToString())] += "<tr style='text-align:center;'>" +
                                                                                        "<td style='padding:3px;'>" + drv.Row["Action_Item"] + "</td>" +
                                                                                        "<td style='padding:3px;'>" + drv.Row["Resource_name"] + "</td>" +
                                                                                        "<td style='padding:3px;'>" + drv.Row["Target"] + "</td>" +
                                                                                        "<td style='padding:3px;'>" + drv.Row["Status"] + "</td>" +
                                                                                        "</tr>";


        }

        private string getEmailAgainstName(string name)
        {
            if (distinctEmails.ContainsKey(name))
                return distinctEmails[name];
            else return null;
        }

        protected void isArchive_CheckedChanged1(object sender, EventArgs e)
        {
            rgActionItem.Rebind();
        }
    }
}

