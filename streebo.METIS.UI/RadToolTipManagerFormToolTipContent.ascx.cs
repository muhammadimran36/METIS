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

public partial class RadToolTipManagerFormToolTipContent : System.Web.UI.UserControl 
{
    protected void Page_Load(object sender, EventArgs e)
    {
		
    }
    
    public void SetValue(string value)
    {
        Label1.Text = value;
    }
}
