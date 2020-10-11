using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBPOS.DataAccess.BusinessLayer;
using WEBPOS.DataAccess.DataEntities;

namespace WEBPOS.Controllers
{
    public class EcommerceConfigController : Controller
    {
        // GET: EcommerceConfig
        public ActionResult Index()
        {
            if (!BlTable.Read(new DeTable { KeyFixed = "ECommerce", KeyVariable = "EcommerceIP" }).Any())
            {
                BlTable.Save(new DeTable { KeyFixed = "ECommerce", KeyVariable = "EcommerceIP", Value = "" });
            }

            if (!BlTable.Read(new DeTable { KeyFixed = "ECommerce", KeyVariable = "EcommercePassword" }).Any())
            {
                BlTable.Save(new DeTable { KeyFixed = "ECommerce", KeyVariable = "EcommercePassword", Value = "" });
            }

            var objConnection = BlTable.Read(new DeTable { KeyFixed = "ECommerce" }).ToList();
            return View(objConnection);
        }

        public ActionResult LoadData()
        {
            try
            {
                var objConnection = new List<DeTable>();

                if (BlTable.Read(new DeTable { KeyFixed = "ECommerce" }) == null)
                {
                    BlTable.Save(new DeTable { KeyFixed = "ECommerce", KeyVariable = "EcommerceIP", Value = "" });
                    BlTable.Save(new DeTable { KeyFixed = "ECommerce", KeyVariable = "EcommercePassword", Value = "" });
                }

                objConnection = BlTable.Read(new DeTable { KeyFixed = "ECommerce" }).ToList();

                return PartialView(objConnection);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult PostData(string ip, string password)
        {
            try
            {
                BlTable.Save(new DeTable { KeyFixed = "ECommerce", KeyVariable = "EcommerceIP", Value = ip });
                BlTable.Save(new DeTable { KeyFixed = "ECommerce", KeyVariable = "EcommercePassword", Value = password });

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}