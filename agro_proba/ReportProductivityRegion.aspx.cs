using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace agro_proba
{
    public partial class ReportProductivityRegion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Browser.Cookies)
            {
                HttpCookie cookie_report_tours = Request.Cookies["Agrochim31_ReportTours"];
                ObjectDataSource1.SelectParameters["id_region"].DefaultValue = cookie_report_tours["id"].ToString();
                ObjectDataSource1.SelectParameters["year"].DefaultValue = cookie_report_tours["year"].ToString();
                ObjectDataSource1.SelectParameters["id_culture"].DefaultValue = cookie_report_tours["id_culture"].ToString();
                ObjectDataSource1.DataBind();
            }
        }
    }
}