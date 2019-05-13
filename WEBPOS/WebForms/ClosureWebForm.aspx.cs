using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using WEBPOS.DataAccess.BusinessLayer;
using WEBPOS.DataAccess.DataEntities;

namespace WEBPOS.WebForms
{
    public partial class ClosureWebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var queryString = ((ClosureWebForm1)sender).ClientQueryString.Split('&');

                PrintPosClosure(queryString);
            }
        }

        private void PrintPosClosure(string[] queryString)
        {
            var posClosureId = Convert.ToInt32(queryString[0].Split('=')[1]);

            var closure = BlPosClosureHead.ReadAllQueryable().FirstOrDefault(x => x.PosClosureHeadId == posClosureId);
            var closureDetail = BlPosClosureDetail.ReadAllQueryable().Where(x => x.PosClosureHeadId == posClosureId);
            var terminal = BlStorePos.ReadAllQueryable().FirstOrDefault(x => x.StorePosCode == closure.StorePosCode);

            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/PosClosureReport.rdlc");

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource RDS = new ReportDataSource("DataSet2", closureDetail);

            ReportViewer1.LocalReport.DataSources.Add(RDS);

            var store = BlStore.ReadAll().FirstOrDefault();

            var storeName = new ReportParameter("StoreName", store.StoreDescription, true);
            var storeAddress = new ReportParameter("StoreAddress", $"{store.Address}, {store.City}", true);
            var rnc = new ReportParameter("RNC", store.RNC, true);
            var tel = new ReportParameter("Telephone", store.Telephone ?? "", true);
            var startDate = new ReportParameter("StartDate", closure.StartDateTime.ToString("dd-MM-yy"), true);
            var startTime = new ReportParameter("StartTime", closure.StartDateTime.ToString("hh:mm:ss"), true);
            var endDate = new ReportParameter("EndDate", closure.EndDateTime.ToString("dd-MM-yy"), true);
            var endTime = new ReportParameter("EndTime", closure.EndDateTime.ToString("hh:mm:ss"), true);
            var pTerminal = new ReportParameter("Terminal", terminal.StorePosDescription, true);
            var pOperator = new ReportParameter("OperatorName", closure.UserCode, true);
            var closureId = new ReportParameter("PosClosureId", closure.PosClosureHeadId.ToString(), true);
            var beginAmount = new ReportParameter("BeginAmount", closure.BeginAmount.ToString("N2"), true);
            var totalDocs = new ReportParameter("TotalDocs", closureDetail.Count().ToString(), true);
            var finalCount = new ReportParameter("FinalCount", (closure.BeginAmount + closure.Total).ToString("N2"), true);
            var difference = new ReportParameter("TotalDifference", (closure.BeginAmount - closure.Total).ToString("N2"), true);

            var resultText = (closure.BeginAmount - closure.Total) < 0 ? "cuadre CORRECTO" : "cuadre INCORRECTO";

            var result = new ReportParameter("FinalResult", resultText, true);

            ReportParameterCollection reportParameters = new ReportParameterCollection();
            reportParameters.Add(startDate);
            reportParameters.Add(startTime);
            reportParameters.Add(endDate);
            reportParameters.Add(endTime);
            reportParameters.Add(storeName);
            reportParameters.Add(storeAddress);
            reportParameters.Add(rnc);
            reportParameters.Add(tel);
            reportParameters.Add(pTerminal);
            reportParameters.Add(pOperator);
            reportParameters.Add(closureId);
            reportParameters.Add(beginAmount);
            reportParameters.Add(totalDocs);
            reportParameters.Add(finalCount);
            reportParameters.Add(difference);
            reportParameters.Add(result);

            ReportViewer1.LocalReport.SetParameters(reportParameters);

            ReportViewer1.LocalReport.Refresh();

            //string filePath = $"~/Docs/Prints/{head.NCF}.pdf";
            string filePath = System.IO.Path.GetTempFileName();
            Export(ReportViewer1.LocalReport, filePath);
            //CLOSE REPORT OBJECT           
            //ReportViewer1.LocalReport.Dispose();

            WebClient webClient = new WebClient();
            Byte[] buffer = webClient.DownloadData(filePath);
            if (buffer != null)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", buffer.Length.ToString());
                Response.BinaryWrite(buffer);
            }
        }

        public string Export(LocalReport rpt, string filePath)
        {
            string ack = "";
            try
            {
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;

                byte[] bytes = rpt.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);
                using (FileStream stream = System.IO.File.OpenWrite(filePath))
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                return ack;
            }
            catch (Exception ex)
            {
                ack = ex.InnerException.Message;
                return ack;
            }
        }
    }
}