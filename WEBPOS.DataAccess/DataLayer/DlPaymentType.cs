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
    public class DlPaymentType : BaseRepository<WEBPOSContext, DePaymentType>
    {
        public DlPaymentType(WEBPOSContext context = null) : base(context) { }
        public IEnumerable<DePaymentType> ReadAll()
        {
            return Context.PaymentTypes.ToList();
        }
        public IQueryable<DePaymentType> ReadAllQueryable()
        {
            return Context.PaymentTypes;
        }
        public IEnumerable<DePaymentType> Read(DePaymentType obj)
        {
            var data = Context.PaymentTypes.ToList();

            if (!string.IsNullOrEmpty(obj.PaymentTypeCode))
                data = data.Where(x=>x.PaymentTypeCode == obj.PaymentTypeCode).ToList();

            if (!string.IsNullOrEmpty(obj.PaymentTypeDescription))
                data = data.Where(x => x.PaymentTypeDescription == obj.PaymentTypeDescription).ToList();

            return data;
        }

        public void Save(DePaymentType obj)
        {
            var val = Context.PaymentTypes.FirstOrDefault(x => x.PaymentTypeCode == obj.PaymentTypeCode);
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
                Context.PaymentTypes.Add(obj);
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Tipo de Pago", obj.PaymentTypeCode)
                };
                BlActivityLog.Save(activity);
            }
            Context.SaveChanges();
        }

        public void Delete(string PaymentTypeCode)
        {
            var obj = Context.PaymentTypes.FirstOrDefault(x=>x.PaymentTypeCode == PaymentTypeCode);
            if(obj != null)
            {
                Context.PaymentTypes.Remove(obj);
                Context.SaveChanges();
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Tipo de Pago", obj.PaymentTypeCode)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
