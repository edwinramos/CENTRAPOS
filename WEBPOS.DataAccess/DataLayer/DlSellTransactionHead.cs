using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.BusinessLayer;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.Helpers;
using WEBPOS.DataAccess.Repository;

namespace WEBPOS.DataAccess.DataLayer
{
    public class DlSellTransactionHead : BaseRepository<WEBPOSContext, DeSellTransactionHead>
    {
        public DlSellTransactionHead(WEBPOSContext context = null) : base(context) { }
        public IEnumerable<DeSellTransactionHead> ReadAll()
        {
            return Context.SellTransactionHeads.ToList();
        }
        public IEnumerable<DeSellTransactionHead> ReadAllQueryableCustom(string filters)
        {
            var queryString = $@"SELECT *
FROM srSellTransactionHead
WHERE {filters}";
            return Context.Database.SqlQuery<DeSellTransactionHead>(queryString);
        }
        public IQueryable<DeSellTransactionHead> ReadAllQueryable()
        {
            return Context.SellTransactionHeads;
        }
        public IEnumerable<DeSellTransactionHead> Read(DeSellTransactionHead obj)
        {
            var data = Context.SellTransactionHeads.ToList();
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
            Context.SellTransactionHeads.Add(obj);
            var activity = new DeActivityLog
            {
                ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Transacción", obj.PosCode + "|" + obj.TransactionNumber)
            };
            BlActivityLog.Save(activity);

            Context.SaveChanges();
        }

        public void Delete(string storeCode, string posCode, double transactionNumber, DateTime transactionDateTime)
        {
            var obj = Context.SellTransactionHeads.FirstOrDefault(x => x.StoreCode == storeCode && x.PosCode == posCode && x.TransactionNumber == transactionNumber && x.TransactionDateTime == transactionDateTime);
            if (obj != null)
            {
                Context.SellTransactionHeads.Remove(obj);
                Context.SaveChanges();

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Transacción", obj.PosCode + "|" + obj.TransactionNumber)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
