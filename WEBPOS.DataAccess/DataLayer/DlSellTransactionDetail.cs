using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.DataEntities;

namespace WEBPOS.DataAccess.DataLayer
{
    public class DlSellTransactionDetail
    {
        private WEBPOSContext context = new WEBPOSContext();
        public IEnumerable<DeSellTransactionDetail> ReadAll()
        {
            return context.SellTransactionDetails.ToList();
        }
        public IQueryable<DeSellTransactionDetail> ReadAllQueryable()
        {
            return context.SellTransactionDetails;
        }
        public IEnumerable<DeSellTransactionDetail> Read(DeSellTransactionDetail obj)
        {
            var data = context.SellTransactionDetails.ToList();
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
            //var val = context.SellTransactionDetails.FirstOrDefault(x => x.StoreCode == obj.StoreCode && x.StorePosCode == obj.StorePosCode && x.TransactionNumber == obj.TransactionNumber && x.TransactionDateTime == obj.TransactionDateTime);

            context.SellTransactionDetails.Add(obj);

            context.SaveChanges();
        }

        public void Delete(string storeCode, string posCode, double transactionNumber, double rowNumber, DateTime transactionDateTime)
        {
            var obj = context.SellTransactionDetails.FirstOrDefault(x => x.StoreCode == storeCode && x.PosCode == posCode && x.TransactionNumber == transactionNumber && x.RowNumber == rowNumber && x.TransactionDateTime == transactionDateTime);
            if (obj != null)
            {
                context.SellTransactionDetails.Remove(obj);
                context.SaveChanges();
            }
        }
    }
}
