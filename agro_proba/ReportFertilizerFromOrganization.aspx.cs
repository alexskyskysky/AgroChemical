using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace agro_proba
{
    public partial class ReportFertilizerFromOrganization : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Browser.Cookies)
            {
                HttpCookie cookie_report_plan = Request.Cookies["Agrochim31_ReportTours"];
                ObjectDataSource1.SelectParameters["id_organization"].DefaultValue = cookie_report_plan["id"].ToString();
                ObjectDataSource1.SelectParameters["year"].DefaultValue = cookie_report_plan["year"].ToString();
                ObjectDataSource1.DataBind();
                ObjectDataSource2.SelectParameters["id_organization"].DefaultValue = cookie_report_plan["id"].ToString();
                ObjectDataSource2.SelectParameters["year"].DefaultValue = cookie_report_plan["year"].ToString();
                ObjectDataSource2.DataBind();
                ObjectDataSource3.SelectParameters["id_organization"].DefaultValue = cookie_report_plan["id"].ToString();
                ObjectDataSource3.SelectParameters["year"].DefaultValue = cookie_report_plan["year"].ToString();
                ObjectDataSource3.DataBind();
                ObjectDataSource4.SelectParameters["id_organization"].DefaultValue = cookie_report_plan["id"].ToString();
                ObjectDataSource4.SelectParameters["year"].DefaultValue = cookie_report_plan["year"].ToString();
                ObjectDataSource4.DataBind();
            }
        }
    }
}