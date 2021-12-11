using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace agro_proba
{
    public partial class ReportHeavyMetalBySoilRegion : System.Web.UI.Page
    {
        /*private void SetDataSourceForReportViewer(String NameDataSet, ObjectDataSource TabSource)
        {
            ReportDataSource oReportDataSource = new ReportDataSource();
            oReportDataSource.Name = NameDataSet;//Имя ДатаСета(того что в отчете)
            oReportDataSource.Value = TabSource;//Источник данных(тут много вариантов)
            this.ReportViewer1.LocalReport.DataSources.Add(oReportDataSource);//Добавление источника данных(а именно отчета)
        }*/

        void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            //Передаем источник данных в отчет
            //e.DataSources.Add(new ReportDataSource(Имя DataSet в SubReport'e, объект с данными));
            e.DataSources.Add(new ReportDataSource("SelectHeavyMetalBySoilFromRegion", this.ObjectDataSource1));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Browser.Cookies)
            {
                //ReportViewer1.LocalReport.DisplayName = "";
                //Пишем путь к главному отчету
                //ReportViewer1.LocalReport.ReportEmbeddedResource = "SignificativeBySoilOrganization.rdlc";
                //ReportViewer1.LocalReport.ReportPath = "SignificativeBySoilOrganization.rdlc";
                HttpCookie cookie_report_tours = Request.Cookies["Agrochim31_ReportTours"];
                ObjectDataSource1.SelectParameters["id_region"].DefaultValue = cookie_report_tours["id"].ToString();
                ObjectDataSource1.SelectParameters["tour"].DefaultValue = cookie_report_tours["tour"].ToString();
                ObjectDataSource1.DataBind();
                ObjectDataSource2.SelectParameters["id_region"].DefaultValue = cookie_report_tours["id"].ToString();
                ObjectDataSource2.SelectParameters["tour"].DefaultValue = cookie_report_tours["tour"].ToString();
                ObjectDataSource2.DataBind();

                //этот метод вызвать нужно для главного отчета, для Subreport'ов работает событие
                //SetDataSourceForReportViewer("SelectSignificativeBySoilFromOrganization", this.ObjectDataSource2);
                
                this.ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);
            }
        }
    }
}