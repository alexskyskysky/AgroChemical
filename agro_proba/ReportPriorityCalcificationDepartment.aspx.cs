using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace agro_proba
{
    public partial class ReportPriorityCalcificationDepartment : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Browser.Cookies)
            {
                //ReportViewer1.LocalReport.DisplayName = "";

                HttpCookie cookie_report_tours = Request.Cookies["Agrochim31_ReportTours"];
                ObjectDataSource1.SelectParameters["id_department"].DefaultValue = cookie_report_tours["id"].ToString();
                ObjectDataSource1.SelectParameters["tour"].DefaultValue = cookie_report_tours["tour"].ToString();
                ObjectDataSource1.DataBind();
                ObjectDataSource2.SelectParameters["id_department"].DefaultValue = cookie_report_tours["id"].ToString();
                ObjectDataSource2.SelectParameters["tour"].DefaultValue = cookie_report_tours["tour"].ToString();
                ObjectDataSource2.DataBind();

            }
        }
    }
}