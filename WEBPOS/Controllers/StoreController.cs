using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBPOS.DataAccess.BusinessLayer;
using WEBPOS.DataAccess.DataEntities;

namespace WEBPOS.Controllers
{
    public class StoreController : Controller
    {
        // GET: Store
        public ActionResult Index()
        {
            var model = BlStore.ReadAll().FirstOrDefault();
            return View(model);
        }

        public ActionResult StoreManage(DeStore model)
        {
            if (string.IsNullOrEmpty(model.StoreCode))
                model.StoreCode = BlStore.GetNextStoreCode();

            BlStore.Save(model);

            return null;
        }
    }
}