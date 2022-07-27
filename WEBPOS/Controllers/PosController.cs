using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WEBPOS.DataAccess.BusinessLayer;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.Models;
using WEBPOS.Utils;

namespace WEBPOS.Controllers
{
    public class PosController : Controller
    {
        // GET: Pos
        public ActionResult Index()
        {
            var userCode = CookiesUtility.ReadCookieAsString("UserCode");
            if (userCode == null || string.IsNullOrEmpty(userCode))
                return RedirectToAction("LogIn", "User");

            var deviceId = Request.UserHostName;
            try
            {
                string[] computer_name = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName.Split(new Char[] { '.' });
                deviceId = computer_name[0];
            }
            catch (Exception ex) { }

            var terminal = BlStorePos.ReadAllQueryable().FirstOrDefault(x => x.DeviceId == deviceId);
            var usr = BlUser.ReadAllQueryable().FirstOrDefault(x => x.UserCode == userCode);

            ViewBag.UserName = usr.Name + " " + usr.LastName;

            if (terminal == null)
                return RedirectToAction("SelectTerminal", "User", new { userType = usr.UserType, forPos = true });

            try
            {
                var dic = HttpContext.Cache["PosGridList"] as Dictionary<string, List<PosGridModel>>;
                dic[CookiesUtility.ReadCookieAsString("UserCode")] = new List<PosGridModel>();
            }
            catch (Exception ex)
            {
                var dic = new Dictionary<string, List<PosGridModel>>();
                dic.Add(userCode, new List<PosGridModel>());
                HttpContext.Cache["PosGridList"] = dic;
            }

            var store = BlStore.ReadByCode(terminal.StoreCode);

            var posClosure = BlPosClosureHead.Read(new DePosClosureHead
            {
                StoreCode = store.StoreCode,
                StorePosCode = terminal.StorePosCode,
                UserCode = usr.UserCode
            }).FirstOrDefault(x => x.StartDateTime.Date == DateTime.Today.Date && x.EndDateTime == new DateTime(1900, 1, 1));

            var model = new PosModel
            {
                StoreCode = terminal.StoreCode,
                StoreDescription = store.StoreDescription,
                StorePosCode = terminal.StorePosCode,
                StorePosDescription = terminal.StorePosDescription,
                MaxDiscAmount = store.MaxDiscAmount,
                MaxDiscPercent = store.MaxDiscPercent,
                PosClosureId = posClosure != null ? posClosure.PosClosureHeadId : 0,
                IsOpenPos = posClosure != null
            };
            return View(model);
        }

        public ActionResult TextSearch(string text, string priceListCode, double quantity)
        {
            if (CookiesUtility.ReadCookieAsString("UserCode") == null || string.IsNullOrEmpty(CookiesUtility.ReadCookieAsString("UserCode")))
                return RedirectToAction("LogIn", "User");

            var dic = HttpContext.Cache["PosGridList"] as Dictionary<string, List<PosGridModel>>;
            var list = dic[CookiesUtility.ReadCookieAsString("UserCode")];

            var items = BlItem.ReadAllQueryable().Where(x => x.ItemCode == text || x.ItemDescription.ToUpper() == text.ToUpper() || x.Barcode == text);
            if (items.Count() == 1)
            {
                var item = items.FirstOrDefault();
                var itemPrice = BlPrice.ReadByCode(item.ItemCode, priceListCode);
                double vatPrice = 0;
                if (itemPrice != null)
                    vatPrice = itemPrice.SellPrice + (itemPrice.SellPrice * (item.Tax.TaxPercent / 100));

                var obj = list.FirstOrDefault(x => x.ItemCode == item.ItemCode);
                if (obj == null)
                {
                    var newItem = new PosGridModel
                    {
                        ItemCode = item.ItemCode,
                        ItemDescription = item.ItemDescription,
                        VatPrice = vatPrice,
                        SellPrice = itemPrice != null ? itemPrice.SellPrice : 0,
                        PriceListCode = priceListCode,
                        PriceListDescription = itemPrice != null ? itemPrice.PriceList.PriceListDescription : "",
                        Quantity = quantity,
                        TaxDescription = item.Tax.TaxDescription,
                        TaxCode = item.Tax.TaxCode,
                        TaxPercent = item.Tax.TaxPercent,
                        Barcode = item.Barcode,
                        DiscountType = 0
                    };
                    newItem.Total = newItem.Quantity * newItem.VatPrice;
                    list.Add(newItem);
                }
                else
                {
                    foreach (var art in list)
                    {
                        if (art.ItemCode == item.ItemCode)
                        {
                            art.Quantity = art.Quantity + 1;
                            art.Total = art.Quantity * art.VatPrice;
                        }
                    }
                }
            }
            dic[CookiesUtility.ReadCookieAsString("UserCode")] = list;
            HttpContext.Cache["PosGridList"] = dic;
            return Json(new { TotalItems = list.Sum(x => x.Quantity), TotalValue = list.Sum(x => x.Total), isSingle = (items.ToList().Count() == 1) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveTransaction(PaymentModel model)
        {
            var dic = HttpContext.Cache["PosGridList"] as Dictionary<string, List<PosGridModel>>;
            var list = dic[CookiesUtility.ReadCookieAsString("UserCode")];

            #region Saving
            var head = new DeSellTransactionHead
            {
                CustomerCode = model.CustomerCode,
                PosCode = model.PosCode,
                PriceListCode = model.PriceListCode,
                StoreCode = model.StoreCode,
                TotalValue = model.Total,
                TransactionNumber = BlSellTransactionHead.GetNextTransactionNumber(model.StoreCode, model.PosCode),
                NCF = BlSellTransactionHead.GetNextNCF(model.DocType),
                TransactionDateTime = DateTime.Now,
                DocType = model.DocType,
                IsPrinted = false,
                TotalDiscount = list.Sum(x => x.Discount * x.Quantity)
            };
            if (model.DocType != DocType.ConsumidorFinal && string.IsNullOrEmpty(model.CustomerCode))
            {
                return Json(new { hasNoClient = true }, JsonRequestBehavior.AllowGet);
            }

            BlSellTransactionHead.Save(head);

            foreach (var item in list)
            {
                var detail = new DeSellTransactionDetail
                {
                    ItemCode = item.ItemCode,
                    ItemDescription = item.ItemDescription,
                    BasePrice = item.SellPrice,
                    SellPrice = item.VatPrice,
                    Quantity = item.Quantity,
                    RowValue = item.Total,
                    TaxCode = item.TaxCode,
                    TaxPercent = BlTax.ReadByCode(item.TaxCode).TaxPercent,
                    PriceListCode = item.PriceListCode,
                    TotalValue = item.Total + item.Discount,
                    Barcode = item.Barcode,
                    TransactionNumber = head.TransactionNumber,
                    TransactionDateTime = head.TransactionDateTime,
                    StoreCode = head.StoreCode,
                    PosCode = head.PosCode,
                    DiscountOnItem = item.Discount,
                    DiscountType = item.DiscountType,
                    RowNumber = BlSellTransactionDetail.GetNextRowNumberNumber(head.StoreCode, head.PosCode, head.TransactionNumber, head.TransactionDateTime)
                };
                BlSellTransactionDetail.Save(detail);
            }

            var payment = new DeSellTransactionPayment
            {
                PaymentTypeCode = model.PaymentTypeCode,
                PaymentValue = model.PayedAmount,
                PosCode = head.PosCode,
                StoreCode = head.StoreCode,
                TransactionNumber = head.TransactionNumber,
                TransactionDateTime = head.TransactionDateTime,
                RowNumber = 0
            };

            BlSellTransactionPayment.Save(payment);

            #endregion

            #region Update Warehouses

            foreach (var obj in list)
            {
                var item = BlItem.ReadByCode(obj.ItemCode);
                var store = BlStore.ReadAll().FirstOrDefault();
                var itemWarehouses = BlItemWarehouse.ReadAllQueryable().Where(x => x.ItemCode == obj.ItemCode && x.WarehouseCode == store.WarehouseCode);
                foreach (var iw in itemWarehouses)
                {
                    //if (iw.QuantityOnHand - obj.Quantity >= 0)
                    //{
                    iw.QuantityOnHand = iw.QuantityOnHand - obj.Quantity;
                    //}
                    //else
                    //    break;

                    BlItemWarehouse.Save(iw);
                }
            }

            #endregion
            return Json(new { NCF = head.NCF, TransactionNumber = head.TransactionNumber, StoreCode = head.StoreCode, PosCode = head.PosCode }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PrintLastRecept(string storeCode, string posCode)
        {
            //var head = BlSellTransactionHead.ReadAllQueryable().LastOrDefault(x => x.PosCode == posCode && x.StoreCode == storeCode);
            var head = BlSellTransactionHead.ReadAllQueryable($"PosCode = '{posCode}' AND StoreCode = '{storeCode}'").LastOrDefault();
            return Json(new { NCF = head.NCF, StoreCode = head.StoreCode, PosCode = head.PosCode }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadData()
        {
            try
            {
                if (CookiesUtility.ReadCookieAsString("UserCode") == null || string.IsNullOrEmpty(CookiesUtility.ReadCookieAsString("UserCode")))
                    return RedirectToAction("LogIn", "User");

                var dic = HttpContext.Cache["PosGridList"] as Dictionary<string, List<PosGridModel>>;
                var model = dic[CookiesUtility.ReadCookieAsString("UserCode")];

                foreach (var obj in model)
                {
                    var item = BlItem.ReadByCode(obj.ItemCode);
                    if (obj.TaxPercent >= 0)
                    {
                        var itemPrice = BlPrice.ReadByCode(item.ItemCode, obj.PriceListCode);
                        double vatPrice = obj.SellPrice;
                        if (itemPrice != null)
                            vatPrice = itemPrice.SellPrice + (itemPrice.SellPrice * (item.Tax.TaxPercent / 100));

                        obj.VatPrice = vatPrice - obj.Discount;
                    }
                }

                return Json(new { draw = model, recordsFiltered = model.Count(), recordsTotal = model.Count(), data = model });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult ClearAll()
        {
            if (CookiesUtility.ReadCookieAsString("UserCode") == null || string.IsNullOrEmpty(CookiesUtility.ReadCookieAsString("UserCode")))
                return RedirectToAction("LogIn", "User");

            var dic = HttpContext.Cache["PosGridList"] as Dictionary<string, List<PosGridModel>>;
            dic[CookiesUtility.ReadCookieAsString("UserCode")] = new List<PosGridModel>();
            HttpContext.Cache["PosGridList"] = dic;
            return null;
        }

        public ActionResult ItemInfoPartial()
        {
            if (CookiesUtility.ReadCookieAsString("UserCode") == null || string.IsNullOrEmpty(CookiesUtility.ReadCookieAsString("UserCode")))
                return RedirectToAction("LogIn", "User");

            return PartialView();
        }

        public JsonResult GetPriceList(string priceListCode)
        {
            if (priceListCode == "")
            {
                return null;
            }

            var priceList = new DePriceList();

            if (priceListCode == "0")
                priceList = BlStore.ReadAllQueryable().FirstOrDefault().PriceList;

            if (!string.IsNullOrEmpty(priceListCode) && priceListCode != "0")
                priceList = BlPriceList.ReadAllQueryable().FirstOrDefault(x => x.PriceListCode == priceListCode);

            var dic = HttpContext.Cache["PosGridList"] as Dictionary<string, List<PosGridModel>>;
            var list = dic[CookiesUtility.ReadCookieAsString("UserCode")];
            foreach (var obj in list)
            {
                obj.SellPrice = BlPrice.ReadByCode(obj.ItemCode, priceListCode)?.SellPrice ?? 0;
                obj.VatPrice = obj.SellPrice + (obj.SellPrice * (obj.TaxPercent / 100));
                obj.PriceListCode = priceList.PriceListCode;
                obj.Total = obj.Quantity * obj.VatPrice;
            }
            dic[CookiesUtility.ReadCookieAsString("UserCode")] = list;
            HttpContext.Cache["PosGridList"] = dic;
            return Json(new { PriceListCode = priceList.PriceListCode, PriceListDescription = priceList.PriceListDescription, TotalItems = list.Sum(x => x.Quantity), TotalValue = list.Sum(x => x.Total) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PriceListSelectPartial()
        {
            if (CookiesUtility.ReadCookieAsString("UserCode") == null || string.IsNullOrEmpty(CookiesUtility.ReadCookieAsString("UserCode")))
                return RedirectToAction("LogIn", "User");

            var list = BlPriceList.ReadAllQueryable();
            return PartialView(list);
        }

        public JsonResult GetCustomers(string bpCode)
        {
            var client = new DeBusinessPartner();
            var dic = HttpContext.Cache["PosGridList"] as Dictionary<string, List<PosGridModel>>;
            var list = dic[CookiesUtility.ReadCookieAsString("UserCode")];

            if (!string.IsNullOrEmpty(bpCode))
                client = BlBusinessPartner.ReadAllQueryable().FirstOrDefault(x => x.BusinessPartnerCode == bpCode);
            else
                client = BlBusinessPartner.ReadAllQueryable().FirstOrDefault(x => x.BusinessPartnerType == "C");

            var priceList = BlPriceList.ReadAllQueryable().FirstOrDefault(x => x.PriceListCode == client.PriceListCode);

            foreach (var obj in list)
            {
                obj.SellPrice = BlPrice.ReadByCode(obj.ItemCode, priceList.PriceListCode)?.SellPrice ?? 0;
                obj.VatPrice = obj.SellPrice + (obj.SellPrice * (obj.TaxPercent / 100));
                obj.PriceListCode = priceList.PriceListCode;
                obj.Total = obj.Quantity * obj.VatPrice;
            }

            return Json(new { TotalItems = list.Sum(x => x.Quantity), TotalValue = list.Sum(x => x.Total), BusinessPartnerCode = client.BusinessPartnerCode, BusinessPartnerDescription = client.BusinessPartnerDescription, PriceListCode = client.PriceListCode, PriceLisrDescription = client.PriceList.PriceListDescription }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomerSelectPartial()
        {
            if (CookiesUtility.ReadCookieAsString("UserCode") == null || string.IsNullOrEmpty(CookiesUtility.ReadCookieAsString("UserCode")))
                return RedirectToAction("LogIn", "User");

            var list = BlBusinessPartner.ReadAllQueryable().Where(x => x.BusinessPartnerType == "C");
            return PartialView(list);
        }

        public ActionResult CustomerLoadData()
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


                //Paging Size (10,20,50,100)    
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all Customer data    
                var model = new List<BusinessPartnerModel>();

                foreach (var obj in BlBusinessPartner.ReadAllQueryable().Where(x => x.BusinessPartnerType == "C"))
                {
                    model.Add(new BusinessPartnerModel
                    {
                        PriceListCode = obj.PriceListCode,
                        PriceListDescription = BlPriceList.ReadAllQueryable().FirstOrDefault(x => x.PriceListCode == obj.PriceListCode).PriceListDescription,
                        BusinessPartnerCode = obj.BusinessPartnerCode,
                        BusinessPartnerDescription = obj.BusinessPartnerDescription
                    });
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    model = model.Where(m => m.BusinessPartnerDescription.ToUpper().Contains(searchValue.ToUpper())).ToList();
                }

                //total number of rows count     
                recordsTotal = model.Count();
                //Paging     
                var data = model.ToList().Skip(skip).Take(pageSize);
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult PaymentPartial(string priceListCode, string storeCode)
        {
            if (CookiesUtility.ReadCookieAsString("UserCode") == null || string.IsNullOrEmpty(CookiesUtility.ReadCookieAsString("UserCode")))
                return RedirectToAction("LogIn", "User");

            var viewer = new ReportViewer();
            var dic = HttpContext.Cache["PosGridList"] as Dictionary<string, List<PosGridModel>>;
            var list = dic[CookiesUtility.ReadCookieAsString("UserCode")];
            var posCode = CookiesUtility.ReadCookieAsString("PosCode")?.ToString() ?? "";
            ModelState.Clear();
            var model = new PaymentModel
            {
                StoreCode = storeCode,
                PosCode = posCode,
                PriceListCode = priceListCode,
                PaymentTypeCode = BlPaymentType.ReadAllQueryable().FirstOrDefault().PaymentTypeCode,
                PayedAmount = list.Sum(x => x.Total),
                Rest = 0,
                Total = list.Sum(x => x.Total),
                DocType = DocType.ConsumidorFinal
            };
            return PartialView(model);
        }

        public ActionResult NewQuantityPartial()
        {
            if (CookiesUtility.ReadCookieAsString("UserCode") == null || string.IsNullOrEmpty(CookiesUtility.ReadCookieAsString("UserCode")))
                return RedirectToAction("LogIn", "User");

            return PartialView();
        }

        public JsonResult RemoveItem(string itemCode)
        {
            var dic = HttpContext.Cache["PosGridList"] as Dictionary<string, List<PosGridModel>>;
            var list = dic[CookiesUtility.ReadCookieAsString("UserCode")];
            var item = list.FirstOrDefault(x => x.ItemCode == itemCode);
            list.Remove(item);

            dic[CookiesUtility.ReadCookieAsString("UserCode")] = list;
            HttpContext.Cache["PosGridList"] = dic;
            return Json(new { TotalItems = list.Sum(x => x.Quantity), TotalValue = list.Sum(x => x.Total) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangeQuantity(string itemCode, double quantity)
        {
            var dic = HttpContext.Cache["PosGridList"] as Dictionary<string, List<PosGridModel>>;
            var list = dic[CookiesUtility.ReadCookieAsString("UserCode")];
            var itm = list.FirstOrDefault(x => x.ItemCode == itemCode);
            itm.Quantity = quantity;

            foreach (var item in list)
            {
                item.Total = item.Quantity * item.VatPrice;
            }

            dic[CookiesUtility.ReadCookieAsString("UserCode")] = list;
            HttpContext.Cache["PosGridList"] = dic;
            return Json(new { TotalItems = list.Sum(x => x.Quantity), TotalValue = list.Sum(x => x.Total) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DiscountsPartial(string itemCode)
        {
            if (CookiesUtility.ReadCookieAsString("UserCode") == null || string.IsNullOrEmpty(CookiesUtility.ReadCookieAsString("UserCode")))
                return RedirectToAction("LogIn", "User");

            var dic = HttpContext.Cache["PosGridList"] as Dictionary<string, List<PosGridModel>>;
            var list = dic[CookiesUtility.ReadCookieAsString("UserCode")];
            var item = list.FirstOrDefault(x => x.ItemCode == itemCode);
            var model = new DiscountsModel
            {
                ItemCode = itemCode,
                ItemPrice = item.VatPrice,
                DiscountAmount = 0,
                Quantity = item.Quantity,
                Result = item.VatPrice
            };
            return PartialView(model);
        }

        public ActionResult TotalDiscountsPartial()
        {
            if (CookiesUtility.ReadCookieAsString("UserCode") == null || string.IsNullOrEmpty(CookiesUtility.ReadCookieAsString("UserCode")))
                return RedirectToAction("LogIn", "User");

            var dic = HttpContext.Cache["PosGridList"] as Dictionary<string, List<PosGridModel>>;
            var list = dic[CookiesUtility.ReadCookieAsString("UserCode")];

            var model = new DiscountsModel
            {
                DiscountAmount = 0,
                ItemPrice = list.Sum(x => x.VatPrice * x.Quantity)
            };
            return PartialView(model);
        }

        public JsonResult SetTotalDiscounts(double discount, int discType)
        {
            var dic = HttpContext.Cache["PosGridList"] as Dictionary<string, List<PosGridModel>>;
            var list = dic[CookiesUtility.ReadCookieAsString("UserCode")];
            var totalQty = list.Sum(x => x.Quantity);
            var totalPrice = list.Sum(x => x.VatPrice);

            foreach (var item in list)
            {
                var obj = BlItem.ReadByCode(item.ItemCode);
                var itemPrice = BlPrice.ReadByCode(item.ItemCode, item.PriceListCode);
                double vatPrice = 0;
                if (itemPrice != null)
                    vatPrice = itemPrice.SellPrice + (itemPrice.SellPrice * (obj.Tax.TaxPercent / 100));

                if (discount == 0)
                {
                    item.Discount = 0;
                    item.VatPrice = vatPrice;
                }
                else
                {
                    double discountedValue = 0;
                    switch (discType)
                    {
                        case 0://Percentage
                            discountedValue = vatPrice * (discount / 100);
                            break;
                        case 1://Amount
                            discountedValue = (discount / totalQty);
                            break;
                    }
                    item.DiscountType = discType;
                    item.Discount = discountedValue;
                }
                item.Total = item.Quantity * (item.VatPrice - item.Discount);
            }

            dic[CookiesUtility.ReadCookieAsString("UserCode")] = list;
            HttpContext.Cache["PosGridList"] = dic;

            return Json(new { TotalItems = list.Sum(x => x.Quantity), TotalValue = list.Sum(x => x.Total) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetDiscounts(string itemCode, double result, int discType)
        {
            var dic = HttpContext.Cache["PosGridList"] as Dictionary<string, List<PosGridModel>>;
            var list = dic[CookiesUtility.ReadCookieAsString("UserCode")];

            foreach (var item in list)
            {
                var obj = BlItem.ReadByCode(item.ItemCode);
                var itemPrice = BlPrice.ReadByCode(item.ItemCode, item.PriceListCode);
                double vatPrice = 0;
                if (itemPrice != null)
                    vatPrice = itemPrice.SellPrice + (itemPrice.SellPrice * (obj.Tax.TaxPercent / 100));
                if (item.ItemCode == itemCode)
                {
                    item.DiscountType = discType;
                    if (result == 0)
                    {
                        item.Discount = 0;
                        item.VatPrice = vatPrice;
                    }
                    else
                    {
                        double discountedValue = 0;
                        switch (discType)
                        {
                            case 0://Percentage
                                discountedValue = item.VatPrice * (result / 100);
                                break;
                            case 1://Amount
                                discountedValue = (result / 1);
                                break;
                        }
                        item.DiscountType = discType;
                        item.Discount = discountedValue;
                    }
                    //item.Discount = item.VatPrice - result;
                }

                item.Total = item.Quantity * (item.VatPrice - item.Discount);
            }

            dic[CookiesUtility.ReadCookieAsString("UserCode")] = list;
            HttpContext.Cache["PosGridList"] = dic;
            return Json(new { TotalItems = list.Sum(x => x.Quantity), TotalValue = list.Sum(x => x.Total) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PosClosureManage(string storeCode, string storePosCode, int posClosureId, bool isClosing)
        {
            if (isClosing)
            {
                var deviceId = Request.UserHostName;
                try
                {
                    string[] computer_name = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName.Split(new Char[] { '.' });
                    deviceId = computer_name[0];
                }
                catch (Exception ex) { }
                var userCode = CookiesUtility.ReadCookieAsString("UserCode");
                var usr = BlUser.ReadAllQueryable().FirstOrDefault(x => x.UserCode == userCode);
                var terminal = BlStorePos.ReadAllQueryable().FirstOrDefault(x => x.DeviceId == deviceId);
                var store = BlStore.ReadByCode(terminal.StoreCode);

                var posClosure = BlPosClosureHead.ReadAllQueryable().FirstOrDefault(x => x.PosClosureHeadId == posClosureId);

                posClosure.EndDateTime = DateTime.Now;

                //var sales = BlSellTransactionHead.ReadAllQueryable().Where(x => x.TransactionDateTime >= posClosure.StartDateTime && x.UpdateUser == userCode);
                var sales = BlSellTransactionHead.ReadAllQueryable($"TransactionDateTime >= '{posClosure.StartDateTime.ToString("yyyy-MM-dd HH:mm:ss")}' AND UpdateUser = '{userCode}'");
                foreach (var sale in sales)
                {
                    BlPosClosureDetail.Save(new DePosClosureDetail
                    {
                        PosClosureHeadId = posClosure.PosClosureHeadId,
                        StoreCode = sale.StoreCode,
                        StorePosCode = sale.PosCode,
                        TransactionNumber = sale.TransactionNumber,
                        TransactionDateTime = sale.TransactionDateTime,
                        NCF = sale.NCF,
                        TotalValue = sale.TotalValue
                    });
                    posClosure.Total += sale.TotalValue;
                }

                BlPosClosureHead.Save(posClosure);

                CookiesUtility.SaveCookieAsString("UserCode", "");
                return Json(new { posClosureId = posClosure.PosClosureHeadId }, JsonRequestBehavior.AllowGet);
            }

            return PartialView("OpenPosQuantity");
        }

        public ActionResult SavePosOpenQuantity(string storeCode, string storePosCode, double quantity)
        {
            var posClosure = new DePosClosureHead
            {
                BeginAmount = quantity,
                StartDateTime = DateTime.Now,
                EndDateTime = new DateTime(1900, 1, 1),
                StoreCode = storeCode,
                StorePosCode = storePosCode,
                UserCode = CookiesUtility.ReadCookieAsString("UserCode").ToString()
            };

            BlPosClosureHead.Save(posClosure);
            return null;
        }

        #region ItemsSearch

        public ActionResult ItemsLoadData(string id, string storeCode)
        {
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;

                var warehouseCode = BlStore.ReadByCode(storeCode)?.WarehouseCode ?? "";

                if (CookiesUtility.ReadCookieAsString("UserCode") == null || string.IsNullOrEmpty(CookiesUtility.ReadCookieAsString("UserCode")))
                    return RedirectToAction("LogIn", "User");

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

        public ActionResult ItemSearchPartial()
        {
            return PartialView();
        }

        #endregion

        public ActionResult ItemsGiftLoadData(string id, string storeCode)
        {
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;

                var warehouseCode = BlStore.ReadByCode(storeCode)?.WarehouseCode ?? "";

                if (CookiesUtility.ReadCookieAsString("UserCode") == null || string.IsNullOrEmpty(CookiesUtility.ReadCookieAsString("UserCode")))
                    return RedirectToAction("LogIn", "User");

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

        public ActionResult ItemGift(string itemCode, string priceListCode, double quantity)
        {
            if (CookiesUtility.ReadCookieAsString("UserCode") == null || string.IsNullOrEmpty(CookiesUtility.ReadCookieAsString("UserCode")))
                return RedirectToAction("LogIn", "User");

            var dic = HttpContext.Cache["PosGridList"] as Dictionary<string, List<PosGridModel>>;
            var list = dic[CookiesUtility.ReadCookieAsString("UserCode")];

            var item = BlItem.ReadByCode(itemCode);
            if (item != null)
            {
                var newItem = new PosGridModel
                {
                    ItemCode = item.ItemCode,
                    ItemDescription = item.ItemDescription,
                    VatPrice = 0,
                    SellPrice = 0,
                    PriceListCode = priceListCode,
                    PriceListDescription = "",
                    Quantity = quantity,
                    TaxDescription = item.Tax.TaxDescription,
                    TaxCode = item.Tax.TaxCode,
                    TaxPercent = -1,
                    Barcode = item.Barcode
                };
                newItem.Total = newItem.Quantity * newItem.VatPrice;
                list.Add(newItem);
            }
            dic[CookiesUtility.ReadCookieAsString("UserCode")] = list;
            HttpContext.Cache["PosGridList"] = dic;
            return Json(new { TotalItems = list.Sum(x => x.Quantity), TotalValue = list.Sum(x => x.Total) }, JsonRequestBehavior.AllowGet);
        }
    }
}