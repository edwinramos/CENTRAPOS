using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.DataLayer;

namespace WEBPOS.DataAccess.BusinessLayer
{
    public static class BlSellTransactionDetail
    {
        public static IEnumerable<DeSellTransactionDetail> ReadAll()
        {
            var dl = new DlSellTransactionDetail();
            return dl.ReadAll();
        }
        public static IEnumerable<DeSellTransactionDetail> ReadAllQueryable()
        {
            var dl = new DlSellTransactionDetail();
            return dl.ReadAll();
        }
        public static IEnumerable<DeSellTransactionDetail> Read(DeSellTransactionDetail obj)
        {
            var dl = new DlSellTransactionDetail();
            return dl.Read(obj);
        }

        public static void Save(DeSellTransactionDetail obj)
        {
            var dl = new DlSellTransactionDetail();
            dl.Save(obj);
        }

        public static void Delete(DeSellTransactionDetail obj)
        {
            var dl = new DlSellTransactionDetail();
            dl.Delete(obj.StoreCode, obj.PosCode, obj.TransactionNumber, obj.RowNumber, obj.TransactionDateTime);
        }

        public static double GetNextRowNumberNumber(string storeCode, string posCode, double transactionNumber, DateTime transactionDateTime)
        {
            var list = ReadAllQueryable().Where(x => x.StoreCode == storeCode && x.PosCode == posCode && x.TransactionNumber == transactionNumber);
            var count = list.Count() + 1;
            double str = count;

            while (list.Any(x => x.RowNumber == str))
            {
                count++;
                str = count;
            }

            return str;
        }
    }
}
