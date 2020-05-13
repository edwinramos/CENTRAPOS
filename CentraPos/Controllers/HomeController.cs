using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CentraPos.Models;
using CentraPos.Utils;
using CentraPos.DataAccess.BusinessLayer;
using CentraPos.DataAccess.DataEntities;
using System.Data;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace CentraPos.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public ActionResult Index()
        {
            SaveCookieAsString(Response, "UserCode", "admin");
            var userCode = ReadCookieAsString(Request, "UserCode");

            if (userCode == null || string.IsNullOrEmpty(userCode))
                return RedirectToAction("LogIn", "User");

            var usr = BlUser.ReadAllQueryable().FirstOrDefault(x => x.UserCode == userCode.ToString());
            if (usr.UserType == UserType.CAJERO)
                return RedirectToAction("Index", "Pos");

            ViewBag.IsEditing = true;
            ViewBag.UserName = usr.Name + " " + usr.LastName;
            if (usr.UserType != UserType.ADMINISTRADOR)
            {
                ViewBag.IsEditing = usr.IsEditing;
            }
            return View();
        }

        public ActionResult Charts()
        {
            return View();
        }

        public JsonResult WeekChart()
        {
            List<object> iData = new List<object>();
            //Creating sample data  
            DataTable dt = new DataTable();
            dt.Columns.Add("Dia", System.Type.GetType("System.String"));
            dt.Columns.Add("Total", System.Type.GetType("System.Int32"));

            var list = BlSellTransactionHead.ReadAllQueryable();

            var date = DateTime.Today;
            DateTime start = date.Date.AddDays(-(int)date.DayOfWeek), // prev sunday 00:00
       end = start.AddDays(7);

            var actualWeek = from record in list
                             where record.TransactionDateTime >= start // include start
                              && record.TransactionDateTime <= end // exclude end
                             select record;

            var dataByDay = actualWeek
    .AsEnumerable() // After this everything uses LINQ to Objects and is executed locally, not on your SQL server
    .GroupBy(o => o.TransactionDateTime.DayOfWeek)
    .Select(g => new { DayOfWeek = g.Key, Value = g.Sum(x => x.TotalValue) })
    .ToList();

            foreach (var item in dataByDay.OrderBy(x => x.DayOfWeek).ToList())
            {
                CultureInfo ci = new CultureInfo("Es-Es");
                DataRow dr = dt.NewRow();
                dr["Dia"] = ci.DateTimeFormat.GetDayName(item.DayOfWeek);
                dr["Total"] = item.Value;
                dt.Rows.Add(dr);
            }

            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }

            return Json(iData);
        }

        public JsonResult MonthChart()
        {
            List<object> iData = new List<object>();
            //Creating sample data  
            DataTable dt = new DataTable();
            dt.Columns.Add("Week", System.Type.GetType("System.String"));
            dt.Columns.Add("Total", System.Type.GetType("System.Int32"));

            var list = BlSellTransactionHead.ReadAllQueryable().ToList();

            var date = DateTime.Today;

            var actualWeek = from record in list
                             where record.TransactionDateTime.Month == date.Month// exclude end
                             select record;

            var firstDay = new DateTime(date.Year, date.Month, 1);

            var dataByWeek =
    from t in list
    where t.TransactionDateTime.Month == date.Month
    group t by new { WeekNumber = (t.TransactionDateTime - firstDay).Days / 7 } into ut
    select new
    {
        WeekNumber = ut.Key.WeekNumber,
        Value = ut.Sum(x => x.TotalValue)
    };

            foreach (var item in dataByWeek.OrderBy(x => x.WeekNumber).ToList())
            {
                if (item.WeekNumber >= 0)
                {
                    CultureInfo ci = new CultureInfo("Es-Es");
                    DataRow dr = dt.NewRow();
                    dr["Week"] = "Semana " + (item.WeekNumber + 1);
                    dr["Total"] = item.Value;
                    dt.Rows.Add(dr);
                }
            }

            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }

            return Json(iData);
        }

        public JsonResult YearChart()
        {
            List<object> iData = new List<object>();
            //Creating sample data  
            DataTable dt = new DataTable();
            dt.Columns.Add("Mes", System.Type.GetType("System.String"));
            dt.Columns.Add("Total", System.Type.GetType("System.Int32"));

            var list = BlSellTransactionHead.ReadAllQueryable();

            var date = DateTime.Today;


            var actualYear = from record in list
                             where record.TransactionDateTime.Year == date.Year// exclude end
                             select record;

            var dataByYear = actualYear
    .AsEnumerable() // After this everything uses LINQ to Objects and is executed locally, not on your SQL server
    .GroupBy(o => o.TransactionDateTime.Month)
    .Select(g => new { Month = g.Key, Value = g.Sum(x => x.TotalValue) })
    .ToList();

            foreach (var item in dataByYear.OrderBy(x => x.Month).ToList())
            {
                CultureInfo ci = new CultureInfo("Es-Es");
                DataRow dr = dt.NewRow();
                dr["Mes"] = ci.DateTimeFormat.GetMonthName(item.Month);
                dr["Total"] = item.Value;
                dt.Rows.Add(dr);
            }

            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }

            return Json(iData);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult EndangeredItemsPartial()
        {
            return PartialView();
        }

        public void SaveCookieAsString(HttpResponse response, string key, string value)
        {
            CookieOptions option = new CookieOptions();
            var m = EncryptionUtility.Encrypt(value);
            option.Expires = DateTime.Now.AddDays(365);
            response.Cookies.Append(key, m, option);
        }

        public string ReadCookieAsString(HttpRequest request, string key)
        {
            var cookie = request.Cookies[key];
            return EncryptionUtility.Decrypt(cookie);
        }
    }
}
