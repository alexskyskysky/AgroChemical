using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace agro_proba
{
    public partial class ReportSignificativeBySlopeOrganization : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Browser.Cookies)
            {
                HttpCookie cookie_report_tours = Request.Cookies["Agrochim31_ReportTours"];
                ObjectDataSource1.SelectParameters["id_organization"].DefaultValue = cookie_report_tours["id"].ToString();
                ObjectDataSource1.SelectParameters["tour"].DefaultValue = cookie_report_tours["tour"].ToString();
                ObjectDataSource1.DataBind();
                ObjectDataSource2.SelectParameters["id_organization"].DefaultValue = cookie_report_tours["id"].ToString();
                ObjectDataSource2.SelectParameters["tour"].DefaultValue = cookie_report_tours["tour"].ToString();
                ObjectDataSource2.DataBind();
            }
        }
    }
}