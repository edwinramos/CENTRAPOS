﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBPOS.DataAccess.BusinessLayer;
using WEBPOS.DataAccess.DataEntities;

namespace WEBPOS.Controllers
{
    public class PriceListController : Controller
    {
        // GET: PriceList
        public ActionResult Index()
        {
            return PartialView();
        }

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
                var model = BlPriceList.ReadAllQueryable().Select(x=> new
                {
                    x.PriceListCode,
                    x.PriceListDescription,
                    x.UpdateUser,
                    x.LastUpdate
                });

                //Sorting    
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                //{
                //    model = model.OrderBy(sortColumn + " " + sortColumnDir);
                //}
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    model = model.Where(m => m.PriceListDescription.ToUpper().Contains(searchValue.ToUpper()));
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

        public ActionResult PriceListManage(DePriceList model)
        {
            BlPriceList.Save(model);

            return null;
        }

        public ActionResult PriceListEditPartial(string id)
        {
            var pl = BlPriceList.Read(new DePriceList { PriceListCode = id });

            if (id == "0")
                return PartialView(new DePriceList { PriceListCode = "", PriceListDescription = "" });

            return PartialView(pl.FirstOrDefault());
        }

        public ActionResult PriceListDelete(string id)
        {
            var pl = BlPriceList.Read(new DePriceList { PriceListCode = id });

            BlPriceList.Delete(pl.FirstOrDefault());

            return null;
        }
    }
}