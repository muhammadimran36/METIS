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
using streebo.METIS.UI.KMDMAPI;

namespace streebo.METIS.UI
{
    public partial class Project_Creation : System.Web.UI.Page
    {
        private MetisBLL objBLL;

        ApplicationService kService = null;
        LeaseInfo lease = null;
        KMDMAPI.Session kSession = null;

        string masterNumber = string.Empty;

        public enum FieldTypes
        {
            Reference,
            DrillDown,
            Value
        }

        public class SubjectFields
        {
            public string sFieldValue;
            public string sFieldText;
            public FieldTypes enumFieldType;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            loadDdl();


        }

        private void loadDdl()
        {
            objBLL = new MetisBLL();
            DataTable dt = new DataTable();
            dt = objBLL.getProjectType();
            ProjectType.DataSource = dt;
            ProjectType.DataTextField = dt.Columns[1].ToString();
            ProjectType.DataValueField = dt.Columns[0].ToString();
            ProjectType.DataBind();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

          // string id = ProjectID.Text;
            string name = ProjectName.Text;
            string type = ProjectType.SelectedValue;
           // string p_message = "";

            //objBLL = new MetisBLL();
            //objBLL.insertProject(id, name, type, out p_message);
            //string tt = id;
            //string[] str = tt.Split('_');
            //int test = Convert.ToInt32(str[1]);
            //test += 1;
            //id = str[0] + Convert.ToString(test);

            try
            {
                kService = new ApplicationService();
                kService.Timeout = 300000;
                if (lease == null)
                    lease = kService.CreateLease("sysadmin", "kalido", "StreeboEDW", null, LeaseInfoDetail.All);
                lblStartTime.Text = DateTime.Now.ToString();
                CreateSubjects();
                lblEndTime.Text = DateTime.Now.ToString();

                kService.DestroyLease(lease.Token);
            }
            catch (Exception Ex)
            {
                lblError.Text = "This exception occured: " + Ex.Message;
            }



        }



         public void CreateSubjects()
        {
            try
            {
                kSession = kService.BeginTransaction(lease.Token, TransactionExceptionHandling.Discard);

                //Creating subject
                kService.PingLease(lease.Token);
                Subject mySubject = CreateSubject();
                if (mySubject != null)
                    SubmitSubject(ref mySubject);
                kService.PingLease(lease.Token);


                //Committing transaction
                kService.PingLease(lease.Token);
                kService.CompleteTransaction(kSession);
            }
            catch (System.Web.Services.Protocols.SoapException Ex)
            {
                //Showing error message
                string OuterDetail = @Ex.Detail.OuterXml;
                int startingIndex = OuterDetail.IndexOf("Message");
                OuterDetail = OuterDetail.Substring(startingIndex, OuterDetail.IndexOf(@"/>") - startingIndex);
                lblError.Text = "Exception: " + Ex.Message + "<br/> Node: " + Ex.Node + " <br/> Detail: " + OuterDetail;

                //RollingBack transaction, in cse of failure
                kService.DiscardTransaction(kSession);
            }
            catch (Exception Ex)
            {
                kService.DiscardTransaction(kSession);
                lblError.Text = "This exception occured: " + Ex.Message;
            }
        }

        public void SubmitSubject(ref Subject sSubject)
        {
           
            kService.PingLease(lease.Token);
            //kSession = kService.BeginTransaction(lease.Token, TransactionExceptionHandling.Discard);

           SubjectResult resultSubj= kService.SubmitSubject(kSession, sSubject);

            if (resultSubj.SubmitResult.Status == SubmitStatus.ProcessedOk)
            {
                lblEndTime.Text = resultSubj.SubmitResult.Status.ToString();
            }
            //kSession = kService.CompleteTransaction(kSession);
        }

        public Subject CreateSubject()
        {
            //Creating Subject result for TPI ctegory using category's sybjectId
            SubjectResult subResult = kService.CreateSubject(kSession, "1-1021");
            Subject tempSubject = subResult.Subject;

            List<Field> subjectFieldList = new List<Field>();
            //List<ValueField> subjectFieldList = new List<ValueField>();


            //Creating Value fields

            ////getting next project id
            objBLL = new MetisBLL();
            DataTable dt = new DataTable();
            dt = objBLL.getNextProjectID();
            string prj_id = dt.Rows[0]["ProjectID"].ToString();


           
            subjectFieldList.Add(CreateValField("Project Name", ProjectName.Text));
            subjectFieldList.Add(CreateValField("Project_ID", prj_id));

            // subjectFieldList.Add(CreateValField("ContactName", ""));

            ////Creating Reference fields
            //// get the subject id from the drop down
            //  subjectFieldList.Add(CreateRefField(ProjectType.SelectedValue.ToString(), "Project Type")); // Item Status (Active)
            //subjectFieldList.Add(CreateRefField("", "Customer")); //Daymon Represented Flag
            //subjectFieldList.Add(CreateRefField("", "PracticeDetail")); //Organic


            ////Creating AutoGeneratedFields
            //subjectFieldList.Add(CreateAutoGeneratedValField("TradingPartnerMDMId")); //Kalido Generated ID            

            //Assigning fields to subject
            tempSubject.Fields = subjectFieldList.ToArray();


            return tempSubject;
        }

        public SubjectReferenceField CreateRefField(string fieldSubjectIDValue, string fieldID)
        {
            //Creating Ref fields
            SubjectReference subRef = new SubjectReference();
            subRef.ReferenceType = SubjectReferenceType.BySubjectId;
            subRef.SubjectIdOrName = fieldSubjectIDValue;

            // create a  SubjectReferenceField attribute field object
            SubjectReferenceField refField = new SubjectReferenceField();
            refField.FieldId = fieldID;

            // create a SubjectRefFieldValue object.
            SubjectReferenceFieldValue refFieldValue = new SubjectReferenceFieldValue();
            refFieldValue.Value = subRef;

            // this can also be OneOfReferences
            refField.FieldValues = new SubjectReferenceFieldValue[1];
            refField.FieldValues[0] = refFieldValue;

            return refField;
        }

        public ValueField CreateValField(string FieldId, string Value)
        {
            SingleFieldValue sfValue = new SingleFieldValue();
            //sfValue.Value = Value;

            ValueField vField = new ValueField();
            vField.FieldId = FieldId;
            vField.Values = new FieldValue[1];
            vField.Values[0] = sfValue;
            ((SingleFieldValue)vField.Values[0]).Value = Value;
            return vField;
        }

        public ValueField CreateAutoGeneratedValField(string FieldId)
        {
            ValueField vField = new ValueField();
            vField.FieldId = FieldId;
            vField.Values = new FieldValue[1];
            vField.Values[0] = new SingleFieldValue();

            return vField;
        }
    }
}