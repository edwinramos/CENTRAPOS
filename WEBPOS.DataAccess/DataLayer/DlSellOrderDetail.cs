﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.DataEntities;

namespace WEBPOS.DataAccess.DataLayer
{
    public class DlSellOrderDetail
    {
        private WEBPOSContext context = new WEBPOSContext();
        public IEnumerable<DeSellOrderDetail> ReadAll()
        {
            return context.SellOrderDetails.ToList();
        }
        public IEnumerable<DeSellOrderDetail> ReadAllQueryable()
        {
            return context.SellOrderDetails;
        }
        public IEnumerable<DeSellOrderDetail> Read(DeSellOrderDetail obj)
        {
            var data = context.SellOrderDetails.ToList();

            if (obj.SellOrderId != 0)
                data = data.Where(x => x.SellOrderId == obj.SellOrderId).ToList();

            if (obj.LineNumber != 0)
                data = data.Where(x => x.LineNumber == obj.LineNumber).ToList();

            return data;
        }

        public void Save(DeSellOrderDetail obj)
        {
            //var val = context.SellOrderDetails.FirstOrDefault(x => x.StoreCode == obj.StoreCode && x.StorePosCode == obj.StorePosCode && x.TransactionNumber == obj.TransactionNumber && x.TransactionDateTime == obj.TransactionDateTime);

            context.SellOrderDetails.Add(obj);

            context.SaveChanges();
        }

        public void Delete(int sellOrderId, double lineNumber)
        {
            var obj = context.SellOrderDetails.FirstOrDefault(x => x.SellOrderId == sellOrderId && x.LineNumber == lineNumber);
            if (obj != null)
            {
                context.SellOrderDetails.Remove(obj);
                context.SaveChanges();
            }
        }
    }
}