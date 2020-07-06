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

    }
}