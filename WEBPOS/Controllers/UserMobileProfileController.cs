using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBPOS.DataAccess.BusinessLayer;
using WEBPOS.DataAccess.DataEntities;

namespace WEBPOS.Controllers
{
    public class UserMobileProfileController : Controller
    {
        // GET: UserMobileProfile
        public ActionResult Index()
        {
            var obj = BlTable.Read(new DeTable { KeyFixed= "ServerPassword" });
            return PartialView(obj.FirstOrDefault());
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
                var model = BlUserMobileProfile.ReadAllQueryable().Select(x=> new
                {
                    Store = BlStore.ReadByCode(x.StoreCode).StoreDescription,
                    x.UserCode,
                    ProfileType = x.MobileProfileType.ToString()
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
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult UserMobileProfileManage(DeUserMobileProfile model)
        {
            BlUserMobileProfile.Save(model);

            return null;
        }

        public ActionResult SettingsSave(string srvPass)
        {
            if(!string.IsNullOrEmpty(srvPass))
            {
                BlTable.Save(new DeTable { KeyFixed = "ServerPassword", KeyVariable = srvPass });
            }
            return null;
        }

        public ActionResult ProfileEditPartial(string userCode, string profileType)
        {
            if (userCode == "0")
                return PartialView(new DeUserMobileProfile());

            var pl = BlUserMobileProfile.Read(new DeUserMobileProfile { UserCode = userCode, MobileProfileType = (MobileProfileType)Enum.Parse(typeof(MobileProfileType), profileType) });
        
            return PartialView(pl.FirstOrDefault());
        }

        public ActionResult ProfileDelete(string userCode, string profileType)
        {
            var profile = (MobileProfileType)Enum.Parse(typeof(MobileProfileType), profileType);
            var pl = BlUserMobileProfile.ReadByCode(userCode, profile);

            BlUserMobileProfile.Delete(pl);

            return null;
        }
    }
}