using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentraPos.DataAccess.DataEntities;
using CentraPos.DataAccess.DataLayer;

namespace CentraPos.DataAccess.BusinessLayer
{
    public static class BlPaymentType
    {
        public static IEnumerable<DePaymentType> ReadAll()
        {
            var dl = new DlPaymentType();
            return dl.ReadAll();
        }
        public static IQueryable<DePaymentType> ReadAllQueryable()
        {
            var dl = new DlPaymentType();
            return dl.ReadAllQueryable();
        }
        public static IEnumerable<DePaymentType> Read(DePaymentType obj)
        {
            var dl = new DlPaymentType();
            return dl.Read(obj);
        }

        public static void Save(DePaymentType obj)
        {
            var dl = new DlPaymentType();
            dl.Save(obj);
        }

        public static void Delete(DePaymentType obj)
        {
            var dl = new DlPaymentType();
            dl.Delete(obj.PaymentTypeCode);
        }
    }
}
