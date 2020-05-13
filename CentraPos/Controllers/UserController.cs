using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CentraPos.DataAccess.BusinessLayer;
using CentraPos.DataAccess.DataEntities;
using CentraPos.DataAccess.Helpers;
using CentraPos.Models;
using CentraPos.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CentraPos.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LogIn()
        {
            if (!string.IsNullOrEmpty(CookiesUtility.ReadCookieAsString(Request, "UserCode")))
                return RedirectToAction("Index", "Home");

            var obj = BlTable.ReadAllQueryable().FirstOrDefault(x => x.KeyFixed == "ServerName");
            var serverId = System.Environment.MachineName;

            if (obj != null)
            {
                if (obj.KeyVariable != serverId)
                {
                    foreach (var item in BlUser.ReadAll())
                    {
                        BlUser.Delete(item);
                    }
                    return View("~/Views/Shared/Error.cshtml");
                }
            }
            else
                BlTable.Save(new DeTable { KeyFixed = "ServerName", KeyVariable = serverId });

            return View();
        }

        public JsonResult UserLogin(string userCode, string userPassword)
        {
            var dpass = BlUser.EncryptString(userPassword, userCode);
            var usr = BlUser.ReadAllQueryable().FirstOrDefault(x => x.UserCode == userCode && x.Password == dpass);
            if (usr != null)
            {
                CookiesUtility.SaveCookieAsString(Response, "UserCode", usr.UserCode);

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.SIGNIN))
                };
                BlActivityLog.Save(activity);
                var deviceId = Request.Host.Value;
                var deviceType = DeviceType.MOBILE;
                try
                {
                    //string[] computer_name = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName.Split(new Char[] { '.' });
                    //deviceId = computer_name[0];
                    deviceType = DeviceType.DESKTOP;
                }
                catch (Exception ex) { }

                var terminal = BlStorePos.ReadAllQueryable().FirstOrDefault(x => x.DeviceId == deviceId && x.DeviceType == deviceType);

                if (terminal != null)
                    CookiesUtility.SaveCookieAsString(Response, "PosCode", terminal.StorePosCode);

                switch (usr.UserType)
                {
                    case UserType.ADMINISTRADOR:
                        return Json(new { url = Url.Action("Index", "Home") });
                    case UserType.GERENTE:
                        return Json(new { url = Url.Action("Index", "Home") });
                    case UserType.CAJERO:
                        if (terminal == null)
                            return Json(new { url = Url.Action("SelectTerminal", "User", new { userType = usr.UserType }) });
                        else
                            return Json(new { url = Url.Action("Index", "Pos") });
                    default:
                        break;
                }
            }
            return Json(new { userType = "null" });
        }

        public ActionResult SelectTerminal(UserType userType, bool forPos = false)
        {
            CookiesUtility.SaveCookieAsString(Response, "forPos", forPos.ToString());
            return View(userType);
        }

        public JsonResult UserSelectTerminal(string posCode, string userType)
        {
            CookiesUtility.SaveCookieAsString(Response, "PosCode", posCode);
            bool forPos = Convert.ToBoolean(CookiesUtility.ReadCookieAsString(Request, "forPos"));
            var type = (UserType)System.Enum.Parse(typeof(UserType), userType);

            var deviceId = Request.Host.Value;
            var deviceType = DeviceType.MOBILE;
            try
            {
                //string[] computer_name = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName.Split(new Char[] { '.' });
                //deviceId = computer_name[0];
                deviceType = DeviceType.DESKTOP;
            }
            catch (Exception ex) { }

            var pos = BlStorePos.ReadAll().FirstOrDefault(x => x.StoreCode == BlStore.ReadAll().FirstOrDefault().StoreCode && x.StorePosCode == posCode);

            pos.DeviceId = deviceId;
            pos.DeviceType = deviceType;

            BlStorePos.Save(pos);

            switch (type)
            {
                case UserType.ADMINISTRADOR:
                    if (forPos)
                        return Json(new { url = Url.Action("Index", "Pos") });
                    else
                        return Json(new { url = Url.Action("Index", "Home") });
                case UserType.GERENTE:
                    if (forPos)
                        return Json(new { url = Url.Action("Index", "Pos") });
                    else
                        return Json(new { url = Url.Action("Index", "Home") });
                case UserType.CAJERO:
                    return Json(new { url = Url.Action("Index", "Pos") });
                default:
                    break;
            }
            return Json(new { url = "null" });
        }

        public ActionResult UserLogout()
        {
            var activity = new DeActivityLog
            {
                ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.SIGNOUT))
            };
            BlActivityLog.Save(activity);
            CookiesUtility.SaveCookieAsString(Response, "UserCode", "");

            return RedirectToAction("LogIn");
        }

        //public ActionResult LoadData()
        //{
        //    var userCode = CookiesUtility.ReadCookieAsString(Request, "UserCode");
        //    try
        //    {
        //        var draw = Request.Form.GetValues("draw").FirstOrDefault();
        //        var start = Request.Form.GetValues("start").FirstOrDefault();
        //        var length = Request.Form.GetValues("length").FirstOrDefault();
        //        var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
        //        var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
        //        var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();


        //        //Paging Size (10,20,50,100)    
        //        int pageSize = length != null ? Convert.ToInt32(length) : 0;
        //        int skip = start != null ? Convert.ToInt32(start) : 0;
        //        int recordsTotal = 0;

        //        // Getting all Customer data    
        //        var model = BlUser.ReadAllQueryable().Select(a => new
        //        {
        //            a.UserCode,
        //            UserType = a.UserType.ToString(),
        //            a.Name
        //        });

        //        var user = BlUser.ReadAllQueryable().FirstOrDefault(x => x.UserCode == userCode.ToString());
        //        if (user.UserType != UserType.ADMINISTRADOR)
        //            model = model.Where(x => x.UserType != UserType.ADMINISTRADOR.ToString());
        //        //Sorting    
        //        //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
        //        //{
        //        //    model = model.OrderBy(sortColumn + " " + sortColumnDir);
        //        //}
        //        //Search    
        //        if (!string.IsNullOrEmpty(searchValue))
        //        {
        //            model = model.Where(m => m.UserCode.ToUpper().Contains(searchValue.ToUpper()));
        //        }

        //        //total number of rows count     
        //        recordsTotal = model.Count();
        //        //Paging     
        //        var data = model.ToList().Skip(skip).Take(pageSize);
        //        //Returning Json Data    
        //        return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public ActionResult UserManage(UserModel model)
        {

            var user = Mapper.Map<DeUser>(model);
            user.IsEditing = model.IsEditingString == "Si";

            BlUser.Save(user);

            return null;
        }

        public ActionResult UserEditPartial(string id)
        {
            var pl = BlUser.Read(new DeUser { UserCode = id });
            var user = pl.FirstOrDefault();

            var model = new UserModel();
            if (id == "0")
                model = new UserModel { UserCode = "", UserType = UserType.CAJERO, Gender = Gender.HOMBRE, IsEditingString = "Si" };
            else
            {
                model = Mapper.Map<UserModel>(user);
                model.IsEditingString = user.IsEditing ? "Si" : "No";
                model.Password = BlUser.DecryptString(user.Password, user.UserCode);
            }

            return PartialView(model);
        }

        public ActionResult UserDelete(string id)
        {
            var pl = BlUser.Read(new DeUser { UserCode = id });

            BlUser.Delete(pl.FirstOrDefault());

            return null;
        }
    }
}
