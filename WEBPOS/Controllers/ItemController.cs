using AutoMapper;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBPOS.DataAccess.BusinessLayer;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.Models;
//using Excel = Microsoft.Office.Interop.Excel;

namespace WEBPOS.Controllers
{
    public class ItemController : Controller
    {
        // GET: Item
        public ActionResult Index()
        {
            var user = BlUser.ReadAllQueryable().FirstOrDefault(x => x.UserCode == Session["UserCode"].ToString());
            ViewBag.IsEditing = user.IsEditing;
            return PartialView();
        }
        #region Items

        public ActionResult LoadData()
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
                var model = BlItem.ReadAllQueryable().Select(x=> new { x.ItemCode, x.ItemDescription, x.LastUpdate });
                
                //Sorting    
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                //{
                //    model = model.OrderBy(sortColumn + " " + sortColumnDir);
                //}
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    model = model.Where(m => m.ItemDescription.ToUpper().Contains(searchValue.ToUpper())).ToList();
                }

                //total number of rows count     
                recordsTotal = model.Count();
                //Paging     
                var data = model.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult ItemDetail(string id)
        {
            var model = new ItemModel { ItemCode = BlItem.GetNextItemCode() };

            //if (id == "0")
            //    model = Mapper.Map<ItemModel>(new DeItem { ItemCode = BlItem.GetNextItemCode() });
            if (id != "0")
                model = Mapper.Map<ItemModel>(BlItem.Read(new DeItem { ItemCode = id }).FirstOrDefault());

            var user = BlUser.ReadAllQueryable().FirstOrDefault(x => x.UserCode == Session["UserCode"].ToString());
            model.canEdit = user.IsEditing;
            return View(model);
        }

        public ActionResult ItemManage(DeItem model)
        {
            if (!string.IsNullOrEmpty(model.Barcode) && !BlItem.ReadAllQueryable().Any(x => x.Barcode == model.Barcode))
                model.Barcode = model.Barcode;
            BlItem.Save(model);

            return RedirectToAction("ItemDetail", new { id = model.ItemCode });
        }

        [HttpPost]
        public ActionResult ImportExcel(HttpPostedFileBase excelFile, string WarehouseCode, string PriceListCode)
        {
            if (excelFile.ContentLength != 0)
            {
                string fileExtension = System.IO.Path.GetExtension(excelFile.FileName);
                if(fileExtension.EndsWith(".xls") || fileExtension.EndsWith(".xlsx"))
                {
                    string path = Server.MapPath($"~/Docs/{excelFile.FileName}");
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    excelFile.SaveAs(path);
                    List<ImportedItem> list = new List<ImportedItem>();
                    using (var stream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            do
                            {
                                while (reader.Read())
                                {
                                    // reader.GetDouble(0);
                                }
                            } while (reader.NextResult());

                            var result = reader.AsDataSet();
                            var sheet = result.Tables[0];
                            var rows = sheet.Rows;

                            for (int row = 2; row < rows.Count; row++)
                            {
                                var m = rows[row];
                                var item = new ImportedItem();

                                if (m.ItemArray[1].ToString().Trim().ToUpper() == "Total".ToUpper())
                                    break;

                                item.Producto = m.ItemArray[1].ToString();
                                item.Cantidad = m.ItemArray[2].ToString();
                                item.PrecioList = m.ItemArray[3].ToString();
                                item.Barcode = m.ItemArray[4].ToString();
                                item.TaxPercent = m.ItemArray[5].ToString();
                                                            

                                list.Add(item);
                            }
                        }
                    }

                    #region Interop

                    foreach (var item in list)
                    {
                        var obj = new DeItem { ItemCode = "", ItemDescription = item.Producto };
                        var dbItem = BlItem.ReadAllQueryable().FirstOrDefault(x => x.ItemDescription == item.Producto);
                        if (dbItem == null)
                        {
                            obj.ItemCode = BlItem.GetNextItemCode();
                            obj.SupplierCode = BlBusinessPartner.ReadAllQueryable().Where(x => x.BusinessPartnerType == "S").FirstOrDefault()?.BusinessPartnerCode ?? "";
                            obj.DepartmentCode = BlDepartment.ReadAllQueryable().FirstOrDefault()?.DepartmentCode ?? "";
                            obj.TaxCode = BlTax.ReadAllQueryable().FirstOrDefault()?.TaxCode ?? "";
                            if (!string.IsNullOrEmpty(item.TaxPercent))
                            {
                                var tPercent = Convert.ToDouble(item.TaxPercent);
                                var tax = BlTax.ReadAllQueryable().FirstOrDefault(x => x.TaxPercent == tPercent);
                                obj.TaxCode = tax?.TaxCode ?? obj.TaxCode;
                            }

                            obj.UnitMeasureCode = BlUnitMeasure.ReadAllQueryable().FirstOrDefault()?.UnitMeasureCode ?? "";
                            if (!string.IsNullOrEmpty(item.Barcode) && !BlItem.ReadAllQueryable().Any(x => x.Barcode == item.Barcode))
                                obj.Barcode = item.Barcode;
                            BlItem.Save(obj);

                            dbItem = BlItem.ReadAllQueryable().FirstOrDefault(x => x.ItemDescription == item.Producto);
                        }
                        else
                        {
                            dbItem.TaxCode = BlTax.ReadAllQueryable().FirstOrDefault()?.TaxCode ?? "";
                            if (!string.IsNullOrEmpty(item.TaxPercent))
                            {
                                var tPercent = Convert.ToDouble(item.TaxPercent);
                                var tax = BlTax.ReadAllQueryable().FirstOrDefault(x => x.TaxPercent == tPercent);
                                dbItem.TaxCode = tax?.TaxCode ?? dbItem.TaxCode;
                                BlItem.Save(dbItem);
                            }
                        }
                        if (!string.IsNullOrEmpty(item.Barcode) && !BlItem.ReadAllQueryable().Any(x => x.Barcode == item.Barcode))
                        {
                            dbItem.Barcode = item.Barcode;
                            BlItem.Save(dbItem);
                        }

                        var warehouses = BlItemWarehouse.Read(new DeItemWarehouse { ItemCode = dbItem.ItemCode, WarehouseCode = WarehouseCode });
                        if (warehouses.Any())
                        {
                            foreach (var warehouse in warehouses)
                            {
                                if (warehouse.WarehouseCode == WarehouseCode)
                                {
                                    warehouse.QuantityOnHand += Convert.ToDouble(item.Cantidad);
                                    BlItemWarehouse.Save(warehouse);
                                }
                            }
                        }
                        else
                        {
                            BlItemWarehouse.Save(new DeItemWarehouse
                            {
                                ItemCode = dbItem.ItemCode,
                                WarehouseCode = WarehouseCode,
                                QuantityOnHand = Convert.ToDouble(item.Cantidad)
                            });
                        }

                        var prices = BlPrice.Read(new DePrice { ItemCode = dbItem.ItemCode, PriceListCode = PriceListCode });
                        if (prices.Any())
                        {
                            foreach (var price in prices)
                            {
                                if (price.PriceListCode == PriceListCode)
                                {
                                    price.SellPrice = Convert.ToDouble(item.PrecioList);
                                    BlPrice.Save(price);
                                }
                            }
                        }
                        else
                        {
                            BlPrice.Save(new DePrice
                            {
                                ItemCode = dbItem.ItemCode,
                                PriceListCode = PriceListCode,
                                SellPrice = Convert.ToDouble(item.PrecioList)
                            });
                        }
                    }
                    #endregion
                }

            }
            return RedirectToAction("Index", "Home");
        }
        #endregion
        #region Prices
        public ActionResult PriceLoadData(string id)
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
                var model = new List<PriceModel>();

                foreach (var price in BlPrice.Read(new DePrice { ItemCode = id }))
                {
                    //var pl=BlPriceList.Read(new DePriceList { PriceListCode=price.PriceListCode})
                    model.Add(new PriceModel { PriceListCode = price.PriceListCode,PriceList = price.PriceList.PriceListDescription, ItemCode = price.ItemCode, SellPrice = price.SellPrice });
                }

                //total number of rows count     
                recordsTotal = model.Count();
                //Paging     
                var data = model.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult PriceManage(DePrice model)
        {
            BlPrice.Save(model);

            return null;
        }

        public ActionResult PriceEditPartial(string itemCode, string priceListCode)
        {
            var user = BlUser.ReadAllQueryable().FirstOrDefault(x => x.UserCode == Session["UserCode"].ToString());
            ViewBag.IsEditing = user.IsEditing;

            if (string.IsNullOrEmpty(priceListCode) || priceListCode == "0")
                return PartialView(new DePrice { ItemCode = itemCode, PriceListCode = "", SellPrice = 0 });

            var pl = BlPrice.Read(new DePrice { PriceListCode = priceListCode, ItemCode = itemCode });

            
            return PartialView(pl.FirstOrDefault());
        }

        public ActionResult PriceDelete(string itemCode, string priceListCode)
        {
            var pl = BlPrice.Read(new DePrice { ItemCode = itemCode, PriceListCode = priceListCode });

            BlPrice.Delete(pl.FirstOrDefault());

            return null;
        }
        #endregion

        #region Warehouse
        public ActionResult WarehouseLoadData(string id)
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
                var model = new List<ItemWarehouseModel>();

                foreach (var obj in BlItemWarehouse.Read(new DeItemWarehouse { ItemCode = id }))
                {
                    //var pl=BlPriceList.Read(new DePriceList { PriceListCode=price.PriceListCode})
                    model.Add(new ItemWarehouseModel { WarehouseCode = obj.WarehouseCode, Warehouse = obj.Warehouse.WarehouseDescription, ItemCode = obj.ItemCode, QuantityOnHand = obj.QuantityOnHand });
                }

                //total number of rows count     
                recordsTotal = model.Count();
                //Paging     
                var data = model.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult WarehouseManage(DeItemWarehouse model)
        {
            BlItemWarehouse.Save(model);

            return null;
        }

        public ActionResult WarehouseEditPartial(string itemCode, string warehouseCode)
        {
            var user = BlUser.ReadAllQueryable().FirstOrDefault(x => x.UserCode == Session["UserCode"].ToString());
            ViewBag.IsEditing = user.IsEditing;

            if (string.IsNullOrEmpty(warehouseCode) || warehouseCode == "0")
                return PartialView(new DeItemWarehouse { ItemCode = itemCode, WarehouseCode = "", QuantityOnHand = 0 });

            var pl = BlItemWarehouse.Read(new DeItemWarehouse { WarehouseCode = warehouseCode, ItemCode = itemCode });

            return PartialView(pl.FirstOrDefault());
        }

        public ActionResult WarehouseDelete(string itemCode, string warehouseCode)
        {
            var pl = BlItemWarehouse.Read(new DeItemWarehouse { ItemCode = itemCode, WarehouseCode = warehouseCode });

            BlItemWarehouse.Delete(pl.FirstOrDefault());

            return null;
        }
        #endregion
    }
}