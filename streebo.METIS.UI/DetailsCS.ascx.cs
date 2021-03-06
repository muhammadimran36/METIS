﻿using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using streebo.METIS.BLL;

namespace streebo.METIS.UI
{
    public partial class DetailsCS : System.Web.UI.UserControl
    {
        string resourceId = "";
        public string ResourceId
        {
            get
            {
                return resourceId;
            }
            set
            {

                resourceId = value;

            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            MetisBLL bll = new MetisBLL();
            // calling the getResourceById procedure using ResourceId.
            DataTable dt = new DataTable();
            dt = bll.getResourceDetailById(ResourceId);
            ResourceView.DataSource = dt;
            ResourceView.DataBind();

        }

        protected void ResourceView_DataBound(object sender, EventArgs e)
        {
            Image image = (System.Web.UI.WebControls.Image)ResourceView.FindControl("imgGenderIcon");
            Label gender = (System.Web.UI.WebControls.Label)ResourceView.FindControl("lblgender");
            //if (image == null) return;
            image.ImageUrl = "employeImages/" + resourceId + ".jpg";

            if (gender.Text == "F")
            {
                image.Attributes.Add("onerror", "this.src='../employeImages/female.jpg';");
            }
            else
            {
                image.Attributes.Add("onerror", "this.src='../employeImages/male.jpg';");                
            }

            //Label lblCV_link = (System.Web.UI.WebControls.Label)ResourceView.FindControl("lblCV_link");
            HyperLink HL_Cv_link = (System.Web.UI.WebControls.HyperLink)ResourceView.FindControl("HL_Cv_link");

            if (String.IsNullOrEmpty(HL_Cv_link.NavigateUrl))
            {
                HL_Cv_link.Text = "N/A";
            }

            //Label lblProfile = (System.Web.UI.WebControls.Label)ResourceView.FindControl("lblProfile");
            HyperLink HL_Profile = (System.Web.UI.WebControls.HyperLink)ResourceView.FindControl("HL_Profile");

            if (String.IsNullOrEmpty(HL_Profile.NavigateUrl))
            {
                HL_Profile.Text = "N/A";
            }
        }

        protected void ResourceView_PageIndexChanging(object sender, FormViewPageEventArgs e)
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}