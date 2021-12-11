using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace agro_proba
{
    public partial class Report_analys_P_K : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Browser.Cookies)
            {
                HttpCookie cookie_report_analys = Request.Cookies["Agrochim31_Report_Analys"];
                ObjectDataSource1.SelectParameters["id_region"].DefaultValue = cookie_report_analys["id_region"].ToString();
                ObjectDataSource1.SelectParameters["id_organization"].DefaultValue = cookie_report_analys["id_organization"].ToString();
                ObjectDataSource1.SelectParameters["id_department"].DefaultValue = cookie_report_analys["id_department"].ToString();
                ObjectDataSource1.SelectParameters["tour"].DefaultValue = cookie_report_analys["tour_analys"].ToString();
                ObjectDataSource1.SelectParameters["year"].DefaultValue = cookie_report_analys["year_analys"].ToString();
                ObjectDataSource1.SelectParameters["date_from_analys_long"].DefaultValue = cookie_report_analys["date_from_analys"].ToString();
                ObjectDataSource1.SelectParameters["date_to_analys_long"].DefaultValue = cookie_report_analys["date_to_analys"].ToString();
                ObjectDataSource1.DataBind();
            }
        }
    }
}