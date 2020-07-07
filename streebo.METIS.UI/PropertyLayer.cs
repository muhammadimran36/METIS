using System.Web;

namespace streebo.METIS.UI
{
    public class PropertyLayer
    {

        public static string ResourceFileNameEN
        {
            get; set;
        }
      //  public static string ResourceFileNameEN { get { return "ResourceRU"; }  set { } }

        public static string ResourceSummary { get { return HttpContext.GetGlobalResourceObject(PropertyLayer.ResourceFileNameEN, "ResourceSummary").ToString(); } }

        public static string ProjectSummary { get { return HttpContext.GetGlobalResourceObject(PropertyLayer.ResourceFileNameEN, "ProjectSummary").ToString(); } }

        public static string Assignments { get { return HttpContext.GetGlobalResourceObject(PropertyLayer.ResourceFileNameEN, "Assignments").ToString(); } }


        public static string AddNewRec { get { return HttpContext.GetGlobalResourceObject(PropertyLayer.ResourceFileNameEN, "AddNewRec").ToString(); } }

        public static string GrdAsingmentResourceName 
        { get { return HttpContext.GetGlobalResourceObject(PropertyLayer.ResourceFileNameEN, "GrdAsingmentResourceName").ToString(); } }

        public static string GrdAsingmentDateOfAssignemnt
        { get { return HttpContext.GetGlobalResourceObject(PropertyLayer.ResourceFileNameEN, "GrdAsingmentDateOfAssignemnt").ToString(); } }

        public static string GrdAsingmentProjectName
        { get { return HttpContext.GetGlobalResourceObject(PropertyLayer.ResourceFileNameEN, "GrdAsingmentProjectName").ToString(); } }

        public static string GrdAsingmentTypeName
        { get { return HttpContext.GetGlobalResourceObject(PropertyLayer.ResourceFileNameEN, "GrdAsingmentTypeName").ToString(); } }

        public static string GrdStartDate
        { get { return HttpContext.GetGlobalResourceObject(PropertyLayer.ResourceFileNameEN, "GrdStartDate").ToString(); } }

        public static string GrdEndDate
        { get { return HttpContext.GetGlobalResourceObject(PropertyLayer.ResourceFileNameEN, "GrdEndDate").ToString(); } }

        public static string GrdWorkload
        { get { return HttpContext.GetGlobalResourceObject(PropertyLayer.ResourceFileNameEN, "GrdWorkload").ToString(); } }

        public static string DepartmentName
        { get { return HttpContext.GetGlobalResourceObject(PropertyLayer.ResourceFileNameEN, "DepartmentName").ToString(); } }
        public static string ReportTo
        { get { return HttpContext.GetGlobalResourceObject(PropertyLayer.ResourceFileNameEN, "ReportTo").ToString(); } }
        public static string Status
        { get { return HttpContext.GetGlobalResourceObject(PropertyLayer.ResourceFileNameEN, "Status").ToString(); } }
        public static string RoleName
        { get { return HttpContext.GetGlobalResourceObject(PropertyLayer.ResourceFileNameEN, "RoleName").ToString(); } }
        public static string BulkStartDate 
        { get { return HttpContext.GetGlobalResourceObject(PropertyLayer.ResourceFileNameEN, "BulkStartDate").ToString(); } }

        public static string BulkEndDate
        { get { return HttpContext.GetGlobalResourceObject(PropertyLayer.ResourceFileNameEN, "BulkEndDate").ToString(); } }

        public static string DepartmentID
        { get { return HttpContext.GetGlobalResourceObject(PropertyLayer.ResourceFileNameEN, "DepartmentID").ToString(); } }

        public static string ShowFilterItemText
        { get { return HttpContext.GetGlobalResourceObject(PropertyLayer.ResourceFileNameEN, "ShowFilterItemText").ToString(); } }
        public static string LabelTextYes
        { get { return HttpContext.GetGlobalResourceObject(PropertyLayer.ResourceFileNameEN, "LabelTextYes").ToString(); } }
        public static string LabelTextNo
        { get { return HttpContext.GetGlobalResourceObject(PropertyLayer.ResourceFileNameEN, "LabelTextNo").ToString(); } }

        public static string ShowHistory
        { get { return HttpContext.GetGlobalResourceObject(PropertyLayer.ResourceFileNameEN, "ShowHistory").ToString(); } }

        public static string Name
        { get { return HttpContext.GetGlobalResourceObject(PropertyLayer.ResourceFileNameEN, "Name").ToString(); } }
    }
}