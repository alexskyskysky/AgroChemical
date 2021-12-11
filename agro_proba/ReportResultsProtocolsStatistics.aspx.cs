using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace agro_proba
{
    public partial class ReportResultsProtocolsStatistics : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Browser.Cookies)
            {
                HttpCookie cookie_report_protocols = Request.Cookies["Agrochim31_Report_ProtocolsFertilizer"];
                ObjectDataSource1.SelectParameters["id_farm_organization"].DefaultValue = cookie_report_protocols["id_farm_organization"].ToString();
                ObjectDataSource1.SelectParameters["id_region"].DefaultValue = cookie_report_protocols["id_region"].ToString();
                ObjectDataSource1.SelectParameters["id_farm"].DefaultValue = cookie_report_protocols["id_farm"].ToString();
                ObjectDataSource1.SelectParameters["id_fertilizer"].DefaultValue = cookie_report_protocols["id_fertilizer"].ToString();
                ObjectDataSource1.SelectParameters["date_from_long"].DefaultValue = cookie_report_protocols["date_from_protocols"].ToString();
                ObjectDataSource1.SelectParameters["date_to_long"].DefaultValue = cookie_report_protocols["date_to_protocols"].ToString();
                ObjectDataSource1.DataBind();
            }
        }
    }
}