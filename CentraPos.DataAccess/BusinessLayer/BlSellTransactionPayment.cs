using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentraPos.DataAccess.DataEntities;
using CentraPos.DataAccess.DataLayer;

namespace CentraPos.DataAccess.BusinessLayer
{
    public static class BlSellTransactionPayment
    {
        public static DeSellTransactionPayment ReadByCode(double transactionNumber, DateTime transactionDateTime, string storeCode, string posCode)
        {
            var dlAssort = new DlSellTransactionPayment();
            return dlAssort.ReadByCode(transactionNumber, transactionDateTime, storeCode, posCode);
        }

        public static IEnumerable<DeSellTransactionPayment> Read(DeSellTransactionPayment deAssort)
        {
            var dlAssort = new DlSellTransactionPayment();
            return dlAssort.Read(deAssort);
        }

        public static IQueryable<DeSellTransactionPayment> ReadQueryable()
        {
            var dlAssort = new DlSellTransactionPayment();
            return dlAssort.ReadQueryable();
        }

        public static void Delete(long transactionNumber, DateTime transactionDateTime, string storeCode, string posCode)
        {
            var dlAssort = new DlSellTransactionPayment();
            dlAssort.Delete(transactionNumber, transactionDateTime, storeCode, posCode);
        }

        public static IEnumerable<DeSellTransactionPayment> ReadAll()
        {
            var dlAssort = new DlSellTransactionPayment();
            return dlAssort.ReadAll();
        }

        public static void Save(DeSellTransactionPayment dept)
        {
            var dlAssort = new DlSellTransactionPayment();
            dlAssort.Save(dept);
        }
    }
}
