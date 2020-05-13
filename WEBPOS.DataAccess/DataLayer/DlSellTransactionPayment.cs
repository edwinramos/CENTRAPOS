using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.Repository;

namespace WEBPOS.DataAccess.DataLayer
{
    public class DlSellTransactionPayment : BaseRepository<WEBPOSContext, DeSellTransactionPayment>
    {
        public DlSellTransactionPayment(WEBPOSContext context = null) : base(context) { }
        public IEnumerable<DeSellTransactionPayment> Read(DeSellTransactionPayment value)
        {
            var query = (from cd in Context.SellTransactionPayments select cd);
            query = query.Where(q => q.TransactionNumber == value.TransactionNumber);
            if (!string.IsNullOrEmpty(value.StoreCode))
            {
                query = query.Where(q => q.StoreCode == value.StoreCode);
            }
            if (!string.IsNullOrEmpty(value.PosCode))
            {
                query = query.Where(q => q.PosCode == value.PosCode);
            }
            if (!string.IsNullOrEmpty(value.PaymentTypeCode))
            {
                query = query.Where(q => q.PaymentTypeCode == value.PaymentTypeCode);
            }
            if (value.TransactionDateTime != DateTime.MinValue)
            {
                query = query.Where(q => q.TransactionDateTime == value.TransactionDateTime);
            }
            
            return query.ToList();
        }

        public DeSellTransactionPayment ReadByCode(double transactionNumber, DateTime transactionDateTime, string storeCode, string posCode)
        {
            return Context.SellTransactionPayments.FirstOrDefault(
                x => x.TransactionNumber == transactionNumber &&
                x.TransactionDateTime == transactionDateTime &&
                x.StoreCode == storeCode &&
                x.PosCode == posCode);
        }

        public IEnumerable<DeSellTransactionPayment> ReadAll()
        {
            return Context.SellTransactionPayments.ToList();
        }

        public IQueryable<DeSellTransactionPayment> ReadQueryable()
        {
            return Context.SellTransactionPayments;
        }

        public void Save(DeSellTransactionPayment value)
        {
            Context.SellTransactionPayments.Add(value);
            Context.SaveChanges();
        }

        public void Delete(long transactionNumber, DateTime transactionDateTime, string storeCode, string posCode)
        {
            var record = Context.SellTransactionPayments.FirstOrDefault(x => x.TransactionNumber == transactionNumber && x.TransactionDateTime == transactionDateTime && x.StoreCode == storeCode && x.PosCode == posCode);
            if (record == null)
            {
                return;
            }
            Context.SellTransactionPayments.Remove(record);
            Context.SaveChanges();
        }
    }
}