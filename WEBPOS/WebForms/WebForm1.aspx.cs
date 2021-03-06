﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.Reporting.WebForms;
using WEBPOS.DataAccess.BusinessLayer;
using WEBPOS.DataAccess.DataEntities;

namespace WEBPOS.WebForms
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var queryString = ((WebForm1)sender).ClientQueryString.Split('&');

                bool isSellTransaction = Convert.ToBoolean(queryString[3].Split('=')[1]);

                if (isSellTransaction)
                    PrintSellTransaction(queryString);
                else
                    PrintSellOrder(queryString);
            }
        }

        private void PrintSellOrder(string[] queryString)
        {
            var sellOrderId = int.Parse(queryString[0].Split('=')[1]);
            var storeCode = queryString[1].Split('=')[1];
            var posCode = queryString[2].Split('=')[1];

            var sellOrder = BlSellOrder.ReadAllQueryable($"SellOrderId = {sellOrderId}").FirstOrDefault();
            var detail = BlSellOrderDetail.ReadAllQueryable($"SellOrderId = {sellOrderId}");
            var client = BlBusinessPartner.ReadAllQueryable().FirstOrDefault(x => x.BusinessPartnerCode == sellOrder.ClientCode);

            var total = detail.Sum(x => x.TotalRowValue);
            var res = new List<DeSellOrderDetail>();
            foreach (var obj in detail)
            {
                obj.TotalRowValue = obj.TotalRowValue - (obj.VatValue * obj.Quantity);
                res.Add(obj);
                if (obj.DiscountValue > 0)
                {
                    res.Add(new DeSellOrderDetail
                    {
                        ItemCode = obj.ItemCode,
                        ItemDescription = "(" + obj.ItemDescription + ")",
                        Quantity = -1,
                        Price = (-1) * obj.DiscountValue,
                        PriceBefDiscounts = obj.PriceBefDiscounts,
                        TotalRowValue = (-1) * obj.DiscountValue
                    });
                }
            }

            //ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/SellOrderReport.rdlc");
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/SellOrderReportSmall.rdlc");

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource RDS = new ReportDataSource("DataSet1", res);

            ReportViewer1.LocalReport.DataSources.Add(RDS);

            var store = BlStore.ReadAll().FirstOrDefault();

            var transactionDate = new ReportParameter("DocDateTime", sellOrder.DocDateTime.ToString("dd/MM/yyyy"), true);
            var transactionNumber = new ReportParameter("SellOrderId", sellOrder.SellOrderId.ToString().PadLeft(7, '0'), true);
            var storeName = new ReportParameter("StoreName", store.StoreDescription, true);
            var storeAddress = new ReportParameter("StoreAddress", $"{store.Address}, {store.City}", true);
            var rnc = new ReportParameter("RNC", store.RNC, true);
            var tel = new ReportParameter("Telephone", store.Telephone ?? "", true);
            var clientName = new ReportParameter("ClientName", client?.BusinessPartnerDescription != null ? (client.BusinessPartnerDescription) : "", true);
            var totalValue = new ReportParameter("Total", "RD$" + total.ToString(), true);

            ReportParameterCollection reportParameters = new ReportParameterCollection();
            reportParameters.Add(transactionDate);
            reportParameters.Add(transactionNumber);
            reportParameters.Add(storeName);
            reportParameters.Add(storeAddress);
            reportParameters.Add(rnc);
            reportParameters.Add(tel);
            reportParameters.Add(clientName);
            reportParameters.Add(totalValue);

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

        private void PrintSellTransaction(string[] queryString)
        {
            var ncf = queryString[0].Split('=')[1];
            var storeCode = queryString[1].Split('=')[1];
            var posCode = queryString[2].Split('=')[1];

            var head = BlSellTransactionHead.ReadAllQueryable($"NCF = '{ncf}' AND StoreCode = '{storeCode}' AND PosCode = '{posCode}'").FirstOrDefault();
            var detail = BlSellTransactionDetail.ReadAllQueryable($"TransactionNumber = {head.TransactionNumber} AND StoreCode = '{head.StoreCode}' AND PosCode = '{posCode}'");
            var headPayment = BlSellTransactionPayment.ReadByCode(head.TransactionNumber, head.TransactionDateTime, head.StoreCode, head.PosCode);
            var client = BlBusinessPartner.ReadAllQueryable().FirstOrDefault(x => x.BusinessPartnerCode == head.CustomerCode);
            var user = BlUser.ReadAllQueryable().FirstOrDefault(x => x.UserCode == head.UpdateUser);

            var res = new List<DeSellTransactionDetail>();
            double vatTotals = 0;
            foreach (var obj in detail)
            {
                var taxPercent = BlTax.ReadByCode(obj.TaxCode).TaxPercent / 100;
                //obj.TotalValue = obj.TotalValue - ((obj.SellPrice - obj.BasePrice) * obj.Quantity);
                if (taxPercent > 0)
                {
                    obj.TaxPercent = obj.Quantity * (obj.BasePrice * taxPercent);
                    vatTotals += obj.TaxPercent;
                }
                else
                    obj.TaxPercent = 0;

                res.Add(obj);
                if (obj.DiscountOnItem > 0)
                {
                    switch (obj.DiscountType)
                    {
                        case 1:
                            res.Add(new DeSellTransactionDetail
                            {
                                ItemCode = obj.ItemCode,
                                ItemDescription = "   ",
                                Quantity = -1,
                                SellPrice = 0,
                                BasePrice = obj.DiscountOnItem * obj.Quantity,
                                TotalValue = (-1) * (obj.DiscountOnItem * obj.Quantity),
                                TaxPercent = 0
                            });
                            break;
                        case 0:
                            res.Add(new DeSellTransactionDetail
                            {
                                ItemCode = obj.ItemCode,
                                ItemDescription = "   ",
                                Quantity = -1,
                                SellPrice = 0,
                                BasePrice = obj.DiscountOnItem,
                                TotalValue = (-1) * obj.DiscountOnItem,
                                TaxPercent = 0
                            });
                            break;
                    }
                }
            }

            //ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/InvoiceReport.rdlc");
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/InvoiceReportLogo.rdlc");

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource RDS = new ReportDataSource("DataSet1", res);

            ReportViewer1.LocalReport.DataSources.Add(RDS);

            var store = BlStore.ReadAll().FirstOrDefault();

            var headerStr = head.NCF.Contains("B02") ? "FACTURA PARA CONSUMIDOR FINAL" : "FACTURA PARA CREDITO FISCAL";

            if (head.NCF.Contains("B02"))
                headerStr = "FACTURA PARA CONSUMIDOR FINAL";

            if (head.NCF.Contains("B01"))
                headerStr = "FACTURA PARA CREDITO FISCAL";

            if (head.NCF.Contains("B15"))
                headerStr = "FACTURA GUBERNAMENTAL";

            var posOperator = new ReportParameter("OperatorName", (user.Name + " " + user.LastName), true);
            var transactionDate = new ReportParameter("TransactionDate", head.TransactionDateTime.ToString("dd/MM/yyyy hh:mm:ss").ToUpper(), true);
            var transactionNumber = new ReportParameter("TransactionNumber", head.TransactionNumber.ToString(), true);
            var storeName = new ReportParameter("StoreName", store.StoreDescription, true);
            var storeAddress = new ReportParameter("StoreAddress", $"{store.Address}, {store.City}", true);
            var rnc = new ReportParameter("RNC", store.RNC, true);
            var nif = new ReportParameter("NIF", store.NIF, true);
            var pNcf = new ReportParameter("NCF", head.NCF, true);
            var tel = new ReportParameter("Telephone", store.Telephone ?? "", true);
            var netTotal = new ReportParameter("NetTotal", "RD$" + detail.Sum(x => x.TotalValue - x.DiscountOnItem).ToString("n2"), true);
            var paymentType = new ReportParameter("PaymentType", BlPaymentType.ReadAll().FirstOrDefault(x => x.PaymentTypeCode == headPayment.PaymentTypeCode).PaymentTypeDescription, true);
            var paymentValue = new ReportParameter("PaymentValue", "RD$" + headPayment.PaymentValue.ToString("n2"), true);
            var paymentRest = new ReportParameter("PaymentRest", "RD$" + (headPayment.PaymentValue - head.TotalValue).ToString("n2"), true);
            var sequenceDueDate = new ReportParameter("SequenceDueDate", head.NCF.Contains("B02") ? "" : ("FECHA DE VENCIMIENTO: " + store.SequenceDueDate.ToString("dd/MM/yyyy")), true);
            var clientRNC = new ReportParameter("ClientRNC", client?.RNC != null ? ("RNC CLIENTE: " + client.RNC) : "", true);
            var clientName = new ReportParameter("ClientName", client?.BusinessPartnerDescription != null ? ("NOMBRE: " + client.BusinessPartnerDescription) : "", true);
            var header = new ReportParameter("Header", headerStr, true);
            var clientGroup = new ReportParameter("ClientGroup", head.NCF.Contains("B02") ? "" : (("GRUPO CLIENTE: " + client.BusinessPartnerGroup?.BusinessPartnerGroupDescription ?? "")), true);

            var subTotal = new ReportParameter("SubTotal", "RD$" + (detail.Sum(x => x.TotalValue - x.DiscountOnItem) + vatTotals).ToString("n2"), true);
            var total = new ReportParameter("Total", "RD$" + (detail.Sum(x => x.TotalValue) + head.TotalDiscount + detail.Sum(x => (x.SellPrice - x.BasePrice) * x.Quantity)).ToString("n2"), true);
            var vatTotal = new ReportParameter("VatTotal", "RD$" + vatTotals.ToString("n2"), true);
            var discountTotal = new ReportParameter("DiscTotal", "RD$" + head.TotalDiscount.ToString("n2"), true);

            ReportParameterCollection reportParameters = new ReportParameterCollection();
            reportParameters.Add(posOperator);
            reportParameters.Add(transactionDate);
            reportParameters.Add(transactionNumber);
            reportParameters.Add(storeName);
            reportParameters.Add(storeAddress);
            reportParameters.Add(rnc);
            reportParameters.Add(nif);
            reportParameters.Add(pNcf);
            reportParameters.Add(tel);
            reportParameters.Add(total);
            reportParameters.Add(paymentType);
            reportParameters.Add(paymentValue);
            reportParameters.Add(paymentRest);
            reportParameters.Add(clientRNC);
            reportParameters.Add(clientName);
            reportParameters.Add(netTotal);
            reportParameters.Add(vatTotal);
            reportParameters.Add(discountTotal);
            reportParameters.Add(subTotal);
            //if (head.DocType == DocType.CreditoFiscal)
            //{
            //    clientRNC = new ReportParameter("ClientRNC", "", true);
            //    clientName
            //        = new ReportParameter("ClientName", "", true);
            //    reportParameters.Add(clientRNC);
            //    reportParameters.Add(clientName);
            //}
            reportParameters.Add(sequenceDueDate);
            reportParameters.Add(clientGroup);
            reportParameters.Add(header);

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