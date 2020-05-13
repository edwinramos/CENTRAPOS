using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentraPos.DataAccess.DataEntities;
using CentraPos.DataAccess.DataLayer;

namespace CentraPos.DataAccess.BusinessLayer
{
    public static class BlSellTransactionHead
    {
        public static IEnumerable<DeSellTransactionHead> ReadAll()
        {
            var dl = new DlSellTransactionHead();
            return dl.ReadAll();
        }
        public static IQueryable<DeSellTransactionHead> ReadAllQueryable()
        {
            var dl = new DlSellTransactionHead();
            return dl.ReadAllQueryable();
        }
        public static IEnumerable<DeSellTransactionHead> Read(DeSellTransactionHead obj)
        {
            var dl = new DlSellTransactionHead();
            obj.NCF = "B020000000X";


            return dl.Read(obj);
        }

        public static void Save(DeSellTransactionHead obj)
        {
            var dl = new DlSellTransactionHead();
            dl.Save(obj);
        }

        public static void Delete(DeSellTransactionHead obj)
        {
            var dl = new DlSellTransactionHead();
            dl.Delete(obj.StoreCode, obj.PosCode, obj.TransactionNumber, obj.TransactionDateTime);
        }

        public static double GetNextTransactionNumber(string storeCode, string posCode)
        {
            var list = ReadAllQueryable().Where(x => x.StoreCode == storeCode && x.PosCode == posCode);
            var count = list.Count() + 1;
            double str = count;

            while (list.Any(x => x.TransactionNumber == str))
            {
                count++;
                str = count;
            }

            return str;
        }

        public static string GetNextNCF(DocType docType)
        {
            var list = ReadAllQueryable().Where(x => x.DocType == docType);
            var count = list.Count() + 1;
            
            if (!list.Any())
                count = docType == DocType.CreditoFiscal ? BlStore.ReadAll().FirstOrDefault().NCFSequence1 : BlStore.ReadAll().FirstOrDefault().NCFSequence2;


            string str = count.ToString().PadLeft(8, '0');
            //string prefix = docType == DocType.CreditoFiscal ? "B01" : "B02";
            string prefix = "";

            switch (docType)
            {
                case DocType.CreditoFiscal:
                    prefix = "B01";
                    break;
                case DocType.ConsumidorFinal:
                    prefix = "B02";
                    break;
                case DocType.Gubernamental:
                    prefix = "B15";
                    break;
            }

            if (list.Any())
            {
                var lastNCF = list.OrderByDescending(x=>x.TransactionDateTime).FirstOrDefault().NCF;
                var sequence = lastNCF.Replace(prefix, "");
                count = Convert.ToInt32(sequence);
                count++;
                str = count.ToString().PadLeft(8, '0');
            }
            
            str = prefix + str;

            return str;
        }
    }
}
