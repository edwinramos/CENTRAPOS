using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.Repository;

namespace WEBPOS.DataAccess.DataLayer
{
    public class DlSellTransactionDetail : BaseRepository<WEBPOSContext, DeSellTransactionDetail>
    {
        public DlSellTransactionDetail(WEBPOSContext context = null) : base(context) { }
        public IEnumerable<DeSellTransactionDetail> ReadAll()
        {
            return Context.SellTransactionDetails.ToList();
        }
        public IEnumerable<DeSellTransactionDetail> ReadAllQueryableCustom(string filters)
        {
            var queryString = $@"SELECT *
FROM srSellTransactionDetail
WHERE {filters}";
            return Context.Database.SqlQuery<DeSellTransactionDetail>(queryString);
        }
        public IQueryable<DeSellTransactionDetail> ReadAllQueryable()
        {
            return Context.SellTransactionDetails;
        }
        public IEnumerable<DeSellTransactionDetail> Read(DeSellTransactionDetail obj)
        {
            var data = Context.SellTransactionDetails.ToList();
            if (!string.IsNullOrEmpty(obj.StoreCode))
                data = data.Where(x => x.StoreCode == obj.StoreCode).ToList();

            if (obj.TransactionNumber != 0)
                data = data.Where(x => x.TransactionNumber == obj.TransactionNumber).ToList();

            if (obj.TransactionDateTime != DateTime.MinValue)
                data = data.Where(x => x.TransactionDateTime == obj.TransactionDateTime).ToList();

            return data;
        }

        public void Save(DeSellTransactionDetail obj)
        {
            //var val = Context.SellTransactionDetails.FirstOrDefault(x => x.StoreCode == obj.StoreCode && x.StorePosCode == obj.StorePosCode && x.TransactionNumber == obj.TransactionNumber && x.TransactionDateTime == obj.TransactionDateTime);

            Context.SellTransactionDetails.Add(obj);

            Context.SaveChanges();
        }

        public void Delete(string storeCode, string posCode, double transactionNumber, double rowNumber, DateTime transactionDateTime)
        {
            var obj = Context.SellTransactionDetails.FirstOrDefault(x => x.StoreCode == storeCode && x.PosCode == posCode && x.TransactionNumber == transactionNumber && x.RowNumber == rowNumber && x.TransactionDateTime == transactionDateTime);
            if (obj != null)
            {
                Context.SellTransactionDetails.Remove(obj);
                Context.SaveChanges();
            }
        }
    }
}
