﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBPOS.DataAccess.BusinessLayer;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.Models;

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
                var data = model.Skip(skip).Take(pageSize).ToList();
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
                var obj = BlSellOrder.ReadAllQueryable().FirstOrDefault(x => x.SellOrderId == id);
                var list = BlSellOrderDetail.ReadAllQueryable().Where(x => x.SellOrderId == obj.SellOrderId);

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
                    var detail = new DeSellTransactionDetail
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
                    BlSellTransactionDetail.Save(detail);
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
            var model = BlSellOrder.ReadAllQueryable().FirstOrDefault(x => x.SellOrderId == id);
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
                var data = model.Skip(skip).Take(pageSize).ToList();
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
            var order = BlSellOrder.ReadAllQueryable().FirstOrDefault(x=>x.SellOrderId == model.SellOrderId);
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

            var obj = BlSellOrderDetail.ReadAllQueryable().Where(x => x.SellOrderId == sellOrderId && x.LineNumber == lineNumber);
            if (obj.Any())
                model = obj.FirstOrDefault();

            return PartialView(model);
        }

        public JsonResult SellOrderDetailDelete(int sellOrderId, int lineNumber)
        {
            var list = BlSellOrderDetail.Read(new DeSellOrderDetail { SellOrderId = sellOrderId, LineNumber = lineNumber });

            BlSellOrderDetail.Delete(list.FirstOrDefault());

            return Json(new { VatSum = list.Sum(x => x.VatValue), DiscSum = list.Sum(x => x.DiscountValue), TotalSum = list.Sum(x=>x.TotalRowValue) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PriceItemInfo(string itemCode, string priceListCode)
        {
            var item = BlItem.ReadByCode(itemCode);
            var itemPrice = BlPrice.ReadAllQueryable().FirstOrDefault(x => x.ItemCode == itemCode && x.PriceListCode == priceListCode);

            double vatPrice = 0;
            if (itemPrice != null)
                vatPrice = itemPrice.SellPrice + (itemPrice.SellPrice * (item.Tax.TaxPercent / 100));

            return Json(new { priceBefDiscount = itemPrice.SellPrice, priceAftTax = vatPrice, taxCode = item.TaxCode }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrderTotals(int sellOrderId)
        {
            var list = BlSellOrderDetail.ReadAllQueryable().Where(x=>x.SellOrderId == sellOrderId);
            
            return Json(new { totalDiscount = list.Sum(x=>x.DiscountValue), docTotal = list.Sum(x=>x.TotalRowValue), taxSum = list.Sum(x=>x.VatValue) }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}