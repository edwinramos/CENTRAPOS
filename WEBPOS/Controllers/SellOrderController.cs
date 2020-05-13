using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBPOS.DataAccess.BusinessLayer;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.Models;
using WEBPOS.Models;
using WEBPOS.Utils;

namespace WEBPOS.Controllers
{
    public class SellOrderController : Controller
    {
        // GET: PriceList
        public ActionResult Index()
        {
            return PartialView();
        }

        public ActionResult SellOrderLoadData()
        {
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

                //Paging Size (10,20,50,100)    
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all Customer data    
                //var model = BlSellOrder.ReadAllQueryable($"DATEPART(YEAR, DocDateTime) = {DateTime.Today.Year}").OrderByDescending(x=>x.DocDateTime).Select(x=> new
                var model = BlSellOrder.ReadAllQueryable().OrderByDescending(x=>x.DocDateTime).Select(x=> new
                {
                    x.SellOrderId,
                    x.DocDateTime,
                    State = x.IsClosed ? "Cerrado" : "Abierto",
                    Customer = x.ClientDescription,
                    x.DocTotal
                });

                //Sorting    
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                //{
                //    model = model.OrderBy(sortColumn + " " + sortColumnDir);
                //}
                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    model = model.Where(m => m.Customer.ToUpper().Contains(searchValue.ToUpper()));
                }

                //total number of rows count     
                recordsTotal = model.Count();
                //Paging     
                var data = model.ToList().Skip(skip).Take(pageSize);
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult SellOrderManage(DeSellOrder model)
        {
            model.ClientDescription = BlBusinessPartner.ReadAllQueryable().FirstOrDefault(x => x.BusinessPartnerCode == model.ClientCode).BusinessPartnerDescription;

            var detail = BlSellOrderDetail.ReadAllQueryable($"SellOrderId = {model.SellOrderId}");
            model.VatSum = detail.Sum(x => x.VatValue * x.Quantity);
            model.TotalDiscount = detail.Sum(x => x.DiscountValue);
            model.DocTotal = detail.Sum(x => x.TotalRowValue - x.DiscountValue);

            BlSellOrder.Save(model);
            
            return RedirectToAction("SellOrderDetail", new { id = model.SellOrderId });
        }

        public ActionResult SellOrderDelete(int id)
        {
            BlSellOrder.Delete(id);

            return null;
        }

        public ActionResult CloseSellOrder(int id)
        {
            try
            {
                var obj = BlSellOrder.ReadAllQueryable($"SellOrderId = {id}").FirstOrDefault();
                var list = BlSellOrderDetail.ReadAllQueryable($"SellOrderId = {obj.SellOrderId}");

                var head = new DeSellTransactionHead
                {
                    CustomerCode = obj.ClientCode,
                    PosCode = obj.StorePosCode,
                    PriceListCode = obj.PriceListCode,
                    StoreCode = BlStore.ReadAll().FirstOrDefault().StoreCode,
                    TotalValue = obj.DocTotal,
                    TransactionNumber = BlSellTransactionHead.GetNextTransactionNumber(obj.StoreCode, obj.StorePosCode),
                    NCF = BlSellTransactionHead.GetNextNCF(DocType.ConsumidorFinal),
                    TransactionDateTime = DateTime.Now,
                    DocType = DocType.ConsumidorFinal,
                    IsPrinted = false,
                    TotalDiscount = obj.TotalDiscount,
                    SellOrderId = id
                };

                BlSellTransactionHead.Save(head);

                foreach (var item in list)
                {
                    var ob = new DeSellTransactionDetail
                    {
                        ItemCode = item.ItemCode,
                        ItemDescription = item.ItemDescription,
                        BasePrice = item.PriceBefDiscounts,
                        SellPrice = item.Price,
                        Quantity = item.Quantity,
                        RowValue = item.TotalRowValue - item.DiscountValue,
                        TaxCode = item.VatCode,
                        PriceListCode = head.PriceListCode,
                        TotalValue = item.TotalRowValue,
                        Barcode = item.Barcode,
                        TransactionNumber = head.TransactionNumber,
                        TransactionDateTime = head.TransactionDateTime,
                        StoreCode = head.StoreCode,
                        PosCode = head.PosCode,
                        DiscountOnItem = item.DiscountValue,
                        RowNumber = BlSellTransactionDetail.GetNextRowNumberNumber(head.StoreCode, head.PosCode, head.TransactionNumber, head.TransactionDateTime)
                    };
                    BlSellTransactionDetail.Save(ob);
                }

                var payment = new DeSellTransactionPayment
                {
                    PaymentTypeCode = obj.PaymentTypeCode,
                    PaymentValue = obj.DocTotal - obj.TotalDiscount,
                    PosCode = head.PosCode,
                    StoreCode = head.StoreCode,
                    TransactionNumber = head.TransactionNumber,
                    TransactionDateTime = head.TransactionDateTime,
                    RowNumber = 0
                };

                BlSellTransactionPayment.Save(payment);

                foreach (var ob in list)
                {
                    var item = BlItem.ReadByCode(ob.ItemCode);
                    var store = BlStore.ReadAll().FirstOrDefault();
                    var itemWarehouses = BlItemWarehouse.ReadAllQueryable().Where(x => x.ItemCode == ob.ItemCode && x.WarehouseCode == store.WarehouseCode);
                    foreach (var iw in itemWarehouses)
                    {
                        iw.QuantityOnHand = iw.QuantityOnHand - ob.Quantity;

                        BlItemWarehouse.Save(iw);
                    }
                }

                obj.IsClosed = true;
                var detail = BlSellOrderDetail.ReadAllQueryable($"SellOrderId = {obj.SellOrderId}");
                obj.VatSum = detail.Sum(x => x.VatValue);
                obj.TotalDiscount = detail.Sum(x => x.DiscountValue);
                obj.DocTotal = detail.Sum(x => x.TotalRowValue);
                BlSellOrder.Save(obj);
            }
            catch (Exception e)
            {
                throw;
            }
            return null;
        }

        #region Detail
        public ActionResult SellOrderDetail(int id)
        {
            if (id == 0)
            {
                return View(new DeSellOrder { SellOrderId = 0, DocDateTime = DateTime.Today, ClosedDateTime = new DateTime(1900,1,1) });
            }
            var model = BlSellOrder.ReadAllQueryable($"SellOrderId = {id}").FirstOrDefault();
            return View(model);
        }

        public ActionResult SellOrderDetailLoadData(int id)
        {
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();


                //Paging Size (10,20,50,100)    
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all Customer data    
                var model = BlSellOrderDetail.Read(new DeSellOrderDetail { SellOrderId = id }).Select(x=>new
                {
                    x.LineNumber,
                    x.ItemDescription,
                    x.Quantity,
                    x.DiscountValue,
                    x.TotalRowValue
                });

                //total number of rows count     
                recordsTotal = model.Count();
                //Paging     
                var data = model.ToList().Skip(skip).Take(pageSize);
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        public ActionResult SellOrderDetailManage(DeSellOrderDetail model)
        {
            var order = BlSellOrder.ReadAllQueryable($"SellOrderId = {model.SellOrderId}").FirstOrDefault();
            var item = BlItem.ReadByCode(model.ItemCode);
            var itemPrice = BlPrice.ReadAllQueryable().FirstOrDefault(x => x.ItemCode == model.ItemCode && x.PriceListCode == order.PriceListCode);

            model.ItemDescription = BlItem.ReadByCode(model.ItemCode).ItemDescription;
            model.VatPercent = item.Tax.TaxPercent;
            model.VatValue = (itemPrice.SellPrice * (item.Tax.TaxPercent / 100));

            BlSellOrderDetail.Save(model);

            return null;
        }

        public ActionResult SellOrderDetailPartial(int sellOrderId, int lineNumber, string whsCode, string plCode)
        {
            var model = new DeSellOrderDetail { SellOrderId = sellOrderId, WarehouseCode = whsCode, Quantity = 1 };

            var obj = BlSellOrderDetail.ReadAllQueryable($"SellOrderId = {sellOrderId} AND LineNumber = {lineNumber}");
            if (obj.Any())
                model = obj.FirstOrDefault();

            return PartialView(model);
        }

        public JsonResult SellOrderDetailDelete(int sellOrderId, int lineNumber)
        {
            var list = BlSellOrderDetail.Read(new DeSellOrderDetail { SellOrderId = sellOrderId, LineNumber = lineNumber });

            BlSellOrderDetail.Delete(list.FirstOrDefault());

            return Json(new { VatSum = list.Sum(x => x.VatValue), DiscSum = list.Sum(x => x.DiscountValue), TotalSum = list.Sum(x=>x.TotalRowValue - x.DiscountValue) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PriceItemInfo(string code, string priceListCode)
        {
            var item = BlItem.ReadAllQueryable().FirstOrDefault(x=>x.ItemCode == code || x.Barcode == code);
            if (item != null)
            {
                var itemPrice = BlPrice.ReadAllQueryable().FirstOrDefault(x => x.PriceListCode == priceListCode && (x.ItemCode == item.ItemCode));

                double vatPrice = 0;
                if (itemPrice != null)
                    vatPrice = itemPrice.SellPrice + (itemPrice.SellPrice * (item.Tax.TaxPercent / 100));

                return Json(new { itemDescription = item.ItemDescription, priceBefDiscount = itemPrice.SellPrice, priceAftTax = vatPrice, taxCode = item.TaxCode, isSuccess = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { isSuccess = false }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrderTotals(int sellOrderId)
        {
            var list = BlSellOrderDetail.ReadAllQueryable($"SellOrderId = {sellOrderId}");
            
            return Json(new { totalDiscount = list.Sum(x=>x.DiscountValue), docTotal = list.Sum(x=>x.TotalRowValue), taxSum = list.Sum(x=>x.VatValue) }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult ItemsLoadData(string id, string warehouseCode)
        {
            try
            {
                if (CookiesUtility.ReadCookieAsString("UserCode") == null || string.IsNullOrEmpty(CookiesUtility.ReadCookieAsString("UserCode")))
                    return RedirectToAction("LogIn", "User");

                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                
                var list = BlItem.ReadSearch(searchValue, id, warehouseCode);

                var model = list.ToList();

                var data = model.ToList().Skip(skip).Take(pageSize);

                return Json(new { draw = model, recordsFiltered = model.Count(), recordsTotal = model.Count(), data = data });
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Uploaded Orders

        public ActionResult UploadedSellOrderLoadData(string userCode, string dateFrom, string dateTo)
        {
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

                //Paging Size (10,20,50,100)    
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all Customer data    
                var fromDate = Convert.ToDateTime(dateFrom);
                var toDate = Convert.ToDateTime(dateTo);
                var model = BlSellOrder.GetSoldQtyByDate(userCode, fromDate, toDate);

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    model = model.Where(m => m.ItemDescription.ToUpper().Contains(searchValue.ToUpper()) || m.Quantity.ToString().Contains(searchValue));
                }

                //total number of rows count     
                recordsTotal = model.Count();
                //Paging     
                var data = model.ToList().Skip(skip).Take(pageSize);
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

                return Json(new { draw = draw, recordsFiltered = 0, recordsTotal = 0, data = new List<SoldQuantityModel>() });
            }
        }

        public ActionResult UploadedOrders()
        {
            return PartialView();
        }
        #endregion
    }
}