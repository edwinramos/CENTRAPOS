﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.DataLayer;

namespace WEBPOS.DataAccess.BusinessLayer
{
    public static class BlPriceList
    {
        public static IEnumerable<DePriceList> ReadAll()
        {
            var dl = new DlPriceList();
            return dl.ReadAll();
        }
        public static IEnumerable<DePriceList> ReadAllQueryable()
        {
            var dl = new DlPriceList();
            return dl.ReadAll();
        }
        public static IEnumerable<DePriceList> Read(DePriceList obj)
        {
            var dl = new DlPriceList();
            return dl.Read(obj);
        }

        public static void Save(DePriceList obj)
        {
            var dl = new DlPriceList();
            dl.Save(obj);
        }

        public static void Delete(DePriceList obj)
        {
            var dl = new DlPriceList();
            dl.Delete(obj.PriceListCode);
        }
    }
}
