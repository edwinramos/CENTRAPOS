using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.DataLayer;

namespace WEBPOS.DataAccess.BusinessLayer
{
    public static class BlSellTransactionHead
    {
        public static IEnumerable<DeSellTransactionHead> ReadAll()
        {
            var dl = new DlSellTransactionHead();
            return dl.ReadAll();
        }
        public static IEnumerable<DeSellTransactionHead> ReadAllQueryable(string filters)
        {
            var dl = new DlSellTransactionHead();
            return dl.ReadAllQueryableCustom(filters);
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
            switch (obj.DocType)
            {
                case DocType.CreditoFiscal:
                    var creditoFiscal = BlTable.Read(new DeTable { KeyFixed = "NCFCreditoFiscal" }).FirstOrDefault();
                    var num = obj.NCF.Replace(creditoFiscal.KeyVariable, "");
                    string count = num.TrimStart(new Char[] { '0' });

                    creditoFiscal.Value = count;
                    BlTable.Save(creditoFiscal);
                    break;
                case DocType.ConsumidorFinal:
                    var consumidorFinal = BlTable.Read(new DeTable { KeyFixed = "NCFConsumidorFinal" }).FirstOrDefault();
                    num = obj.NCF.Replace(consumidorFinal.KeyVariable, "");
                    count = num.TrimStart(new Char[] { '0' });

                    consumidorFinal.Value = count;
                    BlTable.Save(consumidorFinal);
                    break;
                case DocType.Gubernamental:
                    var gubernamental = BlTable.Read(new DeTable { KeyFixed = "NCFGubernamental" }).FirstOrDefault();
                    num = obj.NCF.Replace(gubernamental.KeyVariable, "");
                    count = num.TrimStart(new Char[] { '0' });

                    gubernamental.Value = count;
                    BlTable.Save(gubernamental);
                    break;
            }
        }

        public static void Delete(DeSellTransactionHead obj)
        {
            var dl = new DlSellTransactionHead();
            dl.Delete(obj.StoreCode, obj.PosCode, obj.TransactionNumber, obj.TransactionDateTime);
        }

        public static double GetNextTransactionNumber(string storeCode, string posCode)
        {
            //var list = ReadAllQueryable().Where(x => x.StoreCode == storeCode && x.PosCode == posCode);
            var list = ReadAllQueryable($"StoreCode = '{storeCode}' AND PosCode = '{posCode}'");
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
            //var list = ReadAllQueryable().Where(x => x.DocType == docType);
            var list = ReadAllQueryable($"DocType = {(int)docType}");
            var count = list.Count() + 1;

            if (!list.Any())
                count = docType == DocType.CreditoFiscal ? BlStore.ReadAll().FirstOrDefault().NCFSequence1 : BlStore.ReadAll().FirstOrDefault().NCFSequence2;

            string prefix = "";

            switch (docType)
            {
                case DocType.CreditoFiscal:
                    var creditoFiscal = BlTable.Read(new DeTable { KeyFixed = "NCFCreditoFiscal" }).FirstOrDefault();
                    prefix = creditoFiscal.KeyVariable;
                    break;
                case DocType.ConsumidorFinal:
                    var consumidorFinal = BlTable.Read(new DeTable { KeyFixed = "NCFConsumidorFinal" }).FirstOrDefault();
                    prefix = consumidorFinal.KeyVariable;
                    break;
                case DocType.Gubernamental:
                    var gubernamental = BlTable.Read(new DeTable { KeyFixed = "NCFGubernamental" }).FirstOrDefault();
                    prefix = gubernamental.KeyVariable;
                    count = Convert.ToInt32(gubernamental.Value) + 1;
                    break;
            }

            string str = count.ToString().PadLeft(8, '0');
            //string prefix = docType == DocType.CreditoFiscal ? "B01" : "B02";

            if (list.Any() && docType != DocType.Gubernamental)
            {
                var lastNCF = list.OrderByDescending(x => x.TransactionDateTime).FirstOrDefault().NCF;
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
