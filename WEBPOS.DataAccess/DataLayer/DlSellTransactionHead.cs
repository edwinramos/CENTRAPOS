using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.BusinessLayer;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.Helpers;

namespace WEBPOS.DataAccess.DataLayer
{
    public class DlSellTransactionHead
    {
        private WEBPOSContext context = new WEBPOSContext();
        public IEnumerable<DeSellTransactionHead> ReadAll()
        {
            return context.SellTransactionHeads.ToList();
        }
        public IEnumerable<DeSellTransactionHead> ReadAllQueryable()
        {
            return context.SellTransactionHeads;
        }
        public IEnumerable<DeSellTransactionHead> Read(DeSellTransactionHead obj)
        {
            var data = context.SellTransactionHeads.ToList();
            if (!string.IsNullOrEmpty(obj.StoreCode))
                data = data.Where(x => x.StoreCode == obj.StoreCode).ToList();

            if (obj.TransactionNumber != 0)
                data = data.Where(x => x.TransactionNumber == obj.TransactionNumber).ToList();

            if (obj.TransactionDateTime != DateTime.MinValue)
                data = data.Where(x => x.TransactionDateTime == obj.TransactionDateTime).ToList();

            return data;
        }

        public void Save(DeSellTransactionHead obj)
        {
            context.SellTransactionHeads.Add(obj);
            var activity = new DeActivityLog
            {
                ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Transacción", obj.PosCode + "|" + obj.TransactionNumber)
            };
            BlActivityLog.Save(activity);

            context.SaveChanges();
        }

        public void Delete(string storeCode, string posCode, double transactionNumber, DateTime transactionDateTime)
        {
            var obj = context.SellTransactionHeads.FirstOrDefault(x => x.StoreCode == storeCode && x.PosCode == posCode && x.TransactionNumber == transactionNumber && x.TransactionDateTime == transactionDateTime);
            if (obj != null)
            {
                context.SellTransactionHeads.Remove(obj);
                context.SaveChanges();

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Transacción", obj.PosCode+"|"+obj.TransactionNumber)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
