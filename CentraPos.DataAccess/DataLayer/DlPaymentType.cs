using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentraPos.DataAccess.BusinessLayer;
using CentraPos.DataAccess.DataEntities;
using CentraPos.DataAccess.Helpers;

namespace CentraPos.DataAccess.DataLayer
{
    public class DlPaymentType
    {
        private CentraPosDBContext context = new CentraPosDBContext();
        public IEnumerable<DePaymentType> ReadAll()
        {
            return context.PaymentTypes.ToList();
        }
        public IQueryable<DePaymentType> ReadAllQueryable()
        {
            return context.PaymentTypes;
        }
        public IEnumerable<DePaymentType> Read(DePaymentType obj)
        {
            var data = context.PaymentTypes.ToList();

            if (!string.IsNullOrEmpty(obj.PaymentTypeCode))
                data = data.Where(x=>x.PaymentTypeCode == obj.PaymentTypeCode).ToList();

            if (!string.IsNullOrEmpty(obj.PaymentTypeDescription))
                data = data.Where(x => x.PaymentTypeDescription == obj.PaymentTypeDescription).ToList();

            return data;
        }

        public void Save(DePaymentType obj)
        {
            var val = context.PaymentTypes.FirstOrDefault(x => x.PaymentTypeCode == obj.PaymentTypeCode);
            if (val != null)
            {
                val.PaymentTypeDescription = obj.PaymentTypeDescription;

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.UPDATE), "Tipo de Pago", obj.PaymentTypeCode)
                };
                BlActivityLog.Save(activity);
            }
            else
            {
                context.PaymentTypes.Add(obj);
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Tipo de Pago", obj.PaymentTypeCode)
                };
                BlActivityLog.Save(activity);
            }
            context.SaveChanges();
        }

        public void Delete(string PaymentTypeCode)
        {
            var obj = context.PaymentTypes.FirstOrDefault(x=>x.PaymentTypeCode == PaymentTypeCode);
            if(obj != null)
            {
                context.PaymentTypes.Remove(obj);
                context.SaveChanges();
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Tipo de Pago", obj.PaymentTypeCode)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
