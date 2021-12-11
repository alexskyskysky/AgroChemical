using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace agro_proba
{
    public partial class ReportPlanRegion : System.Web.UI.Page
    {
        void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            //Передаем источник данных в отчет
            //e.DataSources.Add(new ReportDataSource(Имя DataSet в SubReport'e, объект с данными));
            e.DataSources.Add(new ReportDataSource("SelectPlansFromTo", this.ObjectDataSource2));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Browser.Cookies)
            {
                HttpCookie cookie_report_plan = Request.Cookies["Agrochim31_Report_Plan"];
                ObjectDataSource1.SelectParameters["id_region"].DefaultValue = cookie_report_plan["id_region"].ToString();
                ObjectDataSource1.SelectParameters["id_worker"].DefaultValue = cookie_report_plan["id_worker"].ToString();
                ObjectDataSource1.SelectParameters["id_mission"].DefaultValue = cookie_report_plan["id_mission"].ToString();
                ObjectDataSource1.SelectParameters["date_from_plan_long"].DefaultValue = cookie_report_plan["date_from_plan"].ToString();
                ObjectDataSource1.SelectParameters["date_to_plan_long"].DefaultValue = cookie_report_plan["date_to_plan"].ToString();
                ObjectDataSource1.DataBind();
                ObjectDataSource2.SelectParameters["date_from_plan_long"].DefaultValue = cookie_report_plan["date_from_plan"].ToString();
                ObjectDataSource2.SelectParameters["date_to_plan_long"].DefaultValue = cookie_report_plan["date_to_plan"].ToString();
                ObjectDataSource2.DataBind();

                this.ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);
            }
        }
    }
}