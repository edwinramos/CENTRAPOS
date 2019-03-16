using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBPOS.DataAccess.BusinessLayer;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.Models;

namespace WEBPOS.Controllers
{
    public class BusinessPartnerController : Controller
    {
        // GET: BusinessPartner
        public ActionResult Index(string bsType)
        {
            return PartialView("Index", bsType);
        }

        public ActionResult LoadData(string id)
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
                var model = new List<BusinessPartnerModel>();
                foreach (var BusinessPartner in BlBusinessPartner.ReadAllQueryable().Where(x=>x.BusinessPartnerType==id))
                {
                    model.Add(new BusinessPartnerModel { BusinessPartnerCode = BusinessPartner.BusinessPartnerCode, BusinessPartnerDescription = BusinessPartner.BusinessPartnerDescription, LastUpdate = BusinessPartner.LastUpdate });
                }

                //Sorting    
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                //{
                //    model = model.OrderBy(sortColumn + " " + sortColumnDir);
                //}
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    model = model.Where(m => m.BusinessPartnerDescription.ToUpper().Contains(searchValue.ToUpper())).ToList();
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

        public ActionResult BusinessPartnerDetail(string id, string bsType)
        {
            if (id == "0")
            {
                return View(new DeBusinessPartner{ BusinessPartnerCode = BlBusinessPartner.GetNextBusinessPartnerCode(), BusinessPartnerType = bsType });
            }
            var model = BlBusinessPartner.Read(new DeBusinessPartner { BusinessPartnerCode = id }).FirstOrDefault();
            return View(model);
        }

        public ActionResult BusinessPartnerManage(DeBusinessPartner model)
        {
            BlBusinessPartner.Save(model);

            return RedirectToAction("BusinessPartnerDetail", new { id = model.BusinessPartnerCode, bsType = model.BusinessPartnerType });
        }

        #region Business Partner Contact
        public ActionResult BPContactLoadData(string id)
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
                var model = new List<DeBusinessPartnerContact>();

                foreach (var obj in BlBusinessPartnerContact.Read(new DeBusinessPartnerContact { BusinessPartnerCode = id }))
                {
                    //var pl=BlPriceList.Read(new DePriceList { PriceListCode=price.PriceListCode})
                    model.Add(obj);
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

        public ActionResult BPContactManage(DeBusinessPartnerContact model)
        {
            BlBusinessPartnerContact.Save(model);

            return null;
        }

        public ActionResult BusinessPartnerContactEditPartial(string bpCode, string bpContactCode)
        {
            if (string.IsNullOrEmpty(bpContactCode) || bpContactCode == "0")
                return PartialView(new DeBusinessPartnerContact { BusinessPartnerCode = bpCode });

            var pl = BlBusinessPartnerContact.Read(new DeBusinessPartnerContact { BusinessPartnerContactCode = bpContactCode, BusinessPartnerCode = bpCode });

            return PartialView(pl.FirstOrDefault());
        }

        public ActionResult BusinessPartnerContactDelete(string bpCode, string bpContactCode)
        {
            var pl = BlBusinessPartnerContact.Read(new DeBusinessPartnerContact { BusinessPartnerCode = bpCode, BusinessPartnerContactCode = bpContactCode });

            BlBusinessPartnerContact.Delete(pl.FirstOrDefault());

            return null;
        }
        #endregion
    }
}