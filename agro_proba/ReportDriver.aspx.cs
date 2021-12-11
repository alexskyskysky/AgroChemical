using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace agro_proba
{
    public partial class ReportDriver : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Browser.Cookies)
            {
                HttpCookie cookie_report_driver = Request.Cookies["Agrochim31_Report_Driver"];
                ObjectDataSource1.SelectParameters["id_region"].DefaultValue = cookie_report_driver["id_region"].ToString();
                ObjectDataSource1.SelectParameters["id_worker"].DefaultValue = cookie_report_driver["id_worker"].ToString();
                ObjectDataSource1.SelectParameters["id_mission"].DefaultValue = cookie_report_driver["id_mission"].ToString();
                ObjectDataSource1.SelectParameters["date_from_plan_long"].DefaultValue = cookie_report_driver["date_from_plan"].ToString();
                ObjectDataSource1.SelectParameters["date_to_plan_long"].DefaultValue = cookie_report_driver["date_to_plan"].ToString();
                ObjectDataSource1.DataBind();
            }
        }
    }
}