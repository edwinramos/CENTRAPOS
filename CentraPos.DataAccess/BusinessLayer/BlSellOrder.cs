﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentraPos.DataAccess.DataEntities;
using CentraPos.DataAccess.DataLayer;
using CentraPos.DataAccess.Models;

namespace CentraPos.DataAccess.BusinessLayer
{
    public static class BlSellOrder
    {
        public static IEnumerable<DeSellOrder> ReadAll()
        {
            var dl = new DlSellOrder();
            return dl.ReadAll();
        }
        public static IQueryable<DeSellOrder> ReadAllQueryable()
        {
            var dl = new DlSellOrder();
            return dl.ReadAllQueryable();
        }
        public static IQueryable<DeSellOrder> ReadByGroupCode(string groupCode)
        {
            var dl = new DlSellOrder();
            return dl.ReadByGroupCode(groupCode);
        }
        public static IQueryable<SoldQuantityModel> GetSoldQtyByDate(string userCode, DateTime dateFrom, DateTime dateTo)
        {
            var dl = new DlSellOrder();
            return dl.GetSoldQtyByDate(userCode, dateFrom, dateTo);
        }
        public static IEnumerable<DeSellOrder> Read(DeSellOrder obj)
        {
            var dl = new DlSellOrder();
           
            return dl.Read(obj);
        }

        public static void Save(DeSellOrder obj)
        {
            var dl = new DlSellOrder();
            var count = 1;
            if (obj.SellOrderId != 0)
                count = obj.SellOrderId;
            else
            {
                count = dl.ReadAllQueryable().Any() ? dl.ReadAllQueryable().Max(x => x.SellOrderId) + 1 : 1;
            }
            obj.SellOrderId = count;
            dl.Save(obj);
        }

        public static void Delete(int sellOrderId)
        {
            var dl = new DlSellOrder();
            var dlDetail = new DlSellOrderDetail();

            foreach (var o in dlDetail.ReadAll().Where(x => x.SellOrderId == sellOrderId))
            {
                dlDetail.Delete(sellOrderId, o.LineNumber);
            }
            dl.Delete(sellOrderId);
        }
    }
}
