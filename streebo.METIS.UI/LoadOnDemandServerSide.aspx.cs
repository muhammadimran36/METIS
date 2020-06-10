using System;
using System.Web.UI;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class LoadOnDemandServerSide : System.Web.UI.Page 
{
		
		private const int ItemsPerRequest = 10;

		protected void RadComboBox1_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
		{
			DataTable data = GetData(e.Text);

			int itemOffset = e.NumberOfItems;
			int endOffset = Math.Min(itemOffset + ItemsPerRequest, data.Rows.Count);
			e.EndOfItems = endOffset == data.Rows.Count;
			
			for (int i = itemOffset; i < endOffset; i++)
			{
				RadComboBox1.Items.Add(new RadComboBoxItem(data.Rows[i]["CompanyName"].ToString(), data.Rows[i]["CompanyName"].ToString()));
			}
			
			e.Message = GetStatusMessage(endOffset, data.Rows.Count);
		}

		private static string GetStatusMessage(int offset, int total)
    		{
        		if (total <= 0)
            		return "No matches";

		        return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
    		}
	
		private static DataTable GetData(string text)
		{
			SqlDataAdapter adapter = new SqlDataAdapter("SELECT * from Customers WHERE CompanyName LIKE @text + '%'",
				ConfigurationManager.ConnectionStrings["TelerikVSXConnectionString"].ConnectionString);
			adapter.SelectCommand.Parameters.AddWithValue("@text", text);
			
			DataTable data = new DataTable();
			adapter.Fill(data);
			
			return data;
		}
		private static void DisplaySelection(RadComboBox comboBox, Label label)
		{
			if (comboBox.Text != String.Empty)
			{
				label.Text = "You selected text: <b>" + comboBox.Text + "</b> and value: <b>" + comboBox.SelectedValue + "</b>";
			}
			else
			{
				label.Text = "RadComboBox is empty";
			}
		}
		protected void Button1_Click(object sender, EventArgs e)
		{
			DisplaySelection(RadComboBox1, Label1);
		}
}
