using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBPOS.DataAccess.BusinessLayer;
using WEBPOS.DataAccess.DataEntities;

namespace WEBPOS.Controllers
{
    public class PosClosureController : Controller
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
                var model = BlPosClosureHead.ReadAllQueryable().OrderByDescending(x=>x.StartDateTime).Select(x=> new
                {
                    x.PosClosureHeadId,
                    x.StartDateTime,
                    x.EndDateTime,
                    x.BeginAmount,
                    x.UserCode,
                    Estado = (x.EndDateTime.Date == new DateTime(1900, 1, 1)) ? "Abierto" : "Cerrado",
                    x.Total
                });

                //Sorting    
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                //{
                //    model = model.OrderBy(sortColumn + " " + sortColumnDir);
                //}
                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    model = model.Where(m => m.UserCode.ToUpper().Contains(searchValue.ToUpper()));
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

        public ActionResult CloseTurn(int posClosureId)
        {
            var deviceId = Request.UserHostName;
            try
            {
                string[] computer_name = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName.Split(new Char[] { '.' });
                deviceId = computer_name[0];
            }
            catch (Exception ex) { }

            var usr = BlUser.ReadAllQueryable().FirstOrDefault(x => x.UserCode == Session["UserCode"].ToString());
            var terminal = BlStorePos.ReadAllQueryable().FirstOrDefault(x => x.DeviceId == deviceId);
            var store = BlStore.ReadByCode(terminal.StoreCode);

            var posClosure = BlPosClosureHead.ReadAllQueryable().FirstOrDefault(x => x.PosClosureHeadId == posClosureId);

            posClosure.EndDateTime = new DateTime(posClosure.StartDateTime.Year, posClosure.StartDateTime.Month, posClosure.StartDateTime.Day, 23, 59, 50);

            var sales = BlSellTransactionHead.ReadAllQueryable().Where(x => x.TransactionDateTime >= posClosure.StartDateTime && x.TransactionDateTime <= posClosure.EndDateTime && x.UpdateUser == usr.UserCode);

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
            return null;
        }
    }
}