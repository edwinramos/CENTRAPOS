using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBPOS.DataAccess.BusinessLayer;
using WEBPOS.DataAccess.DataEntities;

namespace WEBPOS.Controllers
{
    public class UserSellOrderController : Controller
    {
        // GET: UserSellOrder
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
                var model = BlUserSellOrder.ReadAllQueryable().Where(x => x.UserOrderState == UserOrderState.ASIGNADA).OrderByDescending(x=>x.LastUpdate).Select(x => new
                {
                    x.UserCode,
                    x.SellOrderId,
                    Total = BlSellOrder.ReadAllQueryable().FirstOrDefault(p => p.SellOrderId == x.SellOrderId).DocTotal,
                    State = BlUserSellOrder.ReadByCode(x.UserCode, x.SellOrderId).UserOrderState.ToString()
                });

                //Sorting    
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                //{
                //    model = model.OrderBy(sortColumn + " " + sortColumnDir);
                //}
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    model = model.Where(m => m.SellOrderId.ToString().ToUpper().Contains(searchValue.ToUpper()) || m.UserCode.ToUpper().Contains(searchValue.ToUpper()) || m.State.ToUpper().Contains(searchValue.ToUpper()));
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

        public ActionResult LoadDataInProgress()
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
                var model = BlUserSellOrder.ReadAllQueryable().Where(x=>x.UserOrderState == UserOrderState.TRABAJANDO).OrderByDescending(x => x.LastUpdate).Select(x => new
                {
                    x.UserCode,
                    x.SellOrderId,
                    Total = BlSellOrder.ReadAllQueryable().FirstOrDefault(p => p.SellOrderId == x.SellOrderId).DocTotal,
                    State = BlUserSellOrder.ReadByCode(x.UserCode, x.SellOrderId).UserOrderState.ToString(),
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
                    model = model.Where(m => m.SellOrderId.ToString().ToUpper().Contains(searchValue.ToUpper()) || m.UserCode.ToUpper().Contains(searchValue.ToUpper()) || m.State.ToUpper().Contains(searchValue.ToUpper()));
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

        public ActionResult LoadDataCompleted()
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
                var model = BlUserSellOrder.ReadAllQueryable().Where(x=>x.UserOrderState == UserOrderState.CONCLUIDO).OrderByDescending(x => x.LastUpdate).Select(x => new
                {
                    x.UserCode,
                    x.SellOrderId,
                    Total = BlSellOrder.ReadAllQueryable().FirstOrDefault(p => p.SellOrderId == x.SellOrderId).DocTotal,
                    State = BlUserSellOrder.ReadByCode(x.UserCode, x.SellOrderId).UserOrderState.ToString(),
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
                    model = model.Where(m => m.SellOrderId.ToString().ToUpper().Contains(searchValue.ToUpper()) || m.UserCode.ToUpper().Contains(searchValue.ToUpper()) || m.State.ToUpper().Contains(searchValue.ToUpper()));
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

        public ActionResult UserSellOrderManage(DeUserSellOrder model)
        {
            BlUserSellOrder.Save(model);

            return null;
        }

        public ActionResult UserSellOrderEditPartial(string id)
        {
            return PartialView(new DeUserSellOrder());
        }

        public ActionResult UserSellOrderDelete(string userCode, int sellOrderId)
        {
            var pl = BlUserSellOrder.ReadByCode(userCode, sellOrderId);

            BlUserSellOrder.Delete(pl);

            return null;
        }

        public JsonResult GetSellOrderInfo(int sellOrderId)
        {
            double total = 0;
            var pl = BlSellOrder.ReadAllQueryable().FirstOrDefault(x => x.SellOrderId == sellOrderId);
            if (pl != null)
                total = pl.DocTotal;

            return Json(new { orderTotal = total }, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult UserSellOrderByGroupPartial()
        {
            return PartialView(new DeUserSellOrder());
        }

        public ActionResult UserSellOrderByGroupInsert(string userCode, string groupCode)
        {
            foreach (var order in BlSellOrder.ReadByGroupCode(groupCode))
            {
                var model = new DeUserSellOrder
                {
                    UserCode = userCode,
                    SellOrderId = order.SellOrderId
                };
                BlUserSellOrder.Save(model);
            }           

            return null;
        }
    }
}