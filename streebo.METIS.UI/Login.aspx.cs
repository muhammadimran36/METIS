using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.DirectoryServices;
using streebo.METIS.BLL;

namespace streebo.METIS.UI
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PropertyLayer.ResourceFileNameEN = "ResourceEN";
            LoginImg.ImageUrl = HttpContext.GetGlobalResourceObject(PropertyLayer.ResourceFileNameEN, "LoginIMG").ToString();
            //
            Session["isLogin"] = 0;
            // testing chekc in this is should update
        }

        public int Validate_Login(String Username, String Password)
        {
            SqlConnection con = new SqlConnection(@"User id=sa;Password=Inbox@1234;Server=10.0.100.115;Database=ProMan");
            SqlCommand cmdselect = new SqlCommand();
            cmdselect.CommandType = CommandType.StoredProcedure;
            cmdselect.CommandText = "[dbo].[Login_temp]";
            cmdselect.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = Username;
            cmdselect.Parameters.Add("@UPassword", SqlDbType.NVarChar, 50).Value = Password;
            cmdselect.Parameters.Add("@OutRes", SqlDbType.Int, 4);
            cmdselect.Parameters["@OutRes"].Direction = ParameterDirection.Output;
            cmdselect.Connection = con;
            int Results = 0; try
            {
                con.Open();
                cmdselect.ExecuteNonQuery();
                Results = (int)cmdselect.Parameters["@OutRes"].Value;
            }
            catch (SqlException ex)
            {
                lblMessage.Text = ex.Message;
            }
            finally
            {
                cmdselect.Dispose();
                if (con != null)
                {
                    con.Close();
                }
            }
            return Results;
        }

        protected void btnlogin_Click(object sender, EventArgs e)
        {

            
              
          
                                 Session["user"] = txtUsername.Text;
            Session["isLogin"] = 1;
            Response.Redirect("ResSum.aspx");
            
            //int Results = 0;
            //if (txtUsername.Text != string.Empty && txtPassword.Text != string.Empty)
            //{
            //    Results = Validate_Login(txtUsername.Text.Trim(), txtPassword.Text.Trim());

            //    if (Results == 1)
            //    {
            //        lblMessage.Text = "Loged IN";
            //        string url = "ResSum.aspx";
            //        Session["isLogin"] = 1;
            //        Response.Redirect(url);
            //        //ClientScript.RegisterStartupScript(this.GetType(), "openWindow", "<script>openWindow('" + url + "')</script>");
            //    }
            //    else
            //    {
            //        Session["isLogin"] = 0;
            //        lblMessage.Text = "Invalid Login";
            //        lblMessage.ForeColor = System.Drawing.Color.Red;
            //        //Dont Give too much information this might tell a hacker what is wrong in the login
            //    }
            //}
            //else
            //{
            //    lblMessage.Text = "Please make sure that the username and the password is Correct";
            //}
            try
            {

                string path = "LDAP://ssl.local";
                //string path = "LDAP://10.0.100.10";
                string domainAndUsername = "streebo" + @"\" + txtUsername.Text;


                DirectoryEntry entry = new DirectoryEntry(path, domainAndUsername, txtPassword.Text);
                //Bind to the native AdsObject to force authentication.
                object obj = entry.NativeObject;
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + txtUsername.Text + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();

                if (null == result)
                {
                    lblMessage.Text = "User authentication failed";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    Session["user"] = String.Empty;
                    Session["isLogin"] = 0;
                }
                else
                {
                    Session["user"] = txtUsername.Text;
                    Session["isLogin"] = 1;
                    //Response.Redirect("ResSum.aspx");
                    Response.Redirect("ResSum.aspx", false);
                }
            }

           //     /*To be used in future*/
            //     // Update the new path to the user in the directory.
            //     path = result.Path;
            //     string filterAttribute = (string)result.Properties["cn"][0];
            // }
            catch (Exception ex)
            {
                String x = ex.Message;
                lblMessage.Text = "User authentication failed. Please enter the correct username and password";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                Session["user"] = String.Empty;
                Session["isLogin"] = 0;
            }

            // Comment all area below if you are on  live 
            //MetisBLL bll = new MetisBLL();
            //int i = bll.IsValid(txtUsername.Text, txtPassword.Text);
            //if (i == 0)
            //{
            //    lblMessage.Text = "User authentication failed. Please enter the correct username and password";
            //    lblMessage.ForeColor = System.Drawing.Color.Red;
            //    Session["user"] = String.Empty;
            //    Session["isLogin"] = 0;
            //}
            //else
            //{
            //    Session["user"] = txtUsername.Text;
            //    Session["isLogin"] = 1;
            //    Response.Redirect("ResSum.aspx");
            //}
        }
        //    }
        //    lblMessage.Text = ex.Message;
        //    lblMessage.ForeColor = System.Drawing.Color.Red;
        //    Session["user"] = String.Empty;
        //    Session["
        //}

        protected void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }
    }
}