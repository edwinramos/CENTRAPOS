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
    public class StorePosController : Controller
    {
        // GET: StorePos
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
                var model = new List<StorePosModel>();

                foreach (var x in BlStorePos.ReadAll())
                {
                    var storeDesc = BlStore.ReadByCode(x.StoreCode)?.StoreDescription ?? "";
                    model.Add(new StorePosModel
                    {
                        StoreCode = x.StoreCode,
                        StoreDescription = storeDesc,
                        StorePosCode = x.StorePosCode,
                        StorePosDescription = x.StorePosDescription,
                        DeviceId = x.DeviceId,
                        DeviceType = x.DeviceType.ToString()
                    });
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    model = model.Where(m => m.StorePosDescription.ToUpper().Contains(searchValue.ToUpper())).ToList();
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

        public ActionResult StorePosManage(DeStorePos model)
        {
            if (string.IsNullOrEmpty(model.StorePosCode))
                model.StorePosCode = BlStorePos.GetNextStorePosCode(model.StoreCode);

            BlStorePos.Save(model);

            return null;
        }

        public ActionResult StorePosEditPartial(string storeCode, string storePosCode)
        {
            var pl = BlStorePos.Read(new DeStorePos { StoreCode = storeCode, StorePosCode = storePosCode });

            if (storeCode == "0")
                return PartialView(new DeStorePos { StoreCode = "", StorePosCode = "", StorePosDescription = "" });

            return PartialView(pl.FirstOrDefault());
        }

        public ActionResult StorePosDelete(string storeCode, string storePosCode)
        {
            var pl = BlStorePos.Read(new DeStorePos { StoreCode = storeCode, StorePosCode = storePosCode });

            BlStorePos.Delete(pl.FirstOrDefault());

            return null;
        }
    }
}