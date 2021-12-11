using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace agro_proba
{
    public partial class ReportMacroStatisticsForSelectedPlots : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Browser.Cookies)
            {
                HttpCookie cookie_report = Request.Cookies["Agrochim31_SelectedPlots"];
                ObjectDataSource1.SelectParameters["plots_str"].DefaultValue = cookie_report["plots_str"].ToString();
                ObjectDataSource1.DataBind();
            }
        }
    }
}