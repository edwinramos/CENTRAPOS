using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.BusinessLayer;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.Helpers;

namespace WEBPOS.DataAccess.DataLayer
{
    public class DlTax
    {
        private WEBPOSContext context = new WEBPOSContext();
        public IEnumerable<DeTax> ReadAll()
        {
            return context.Taxes.ToList();
        }
        public IQueryable<DeTax> ReadAllQueryable()
        {
            return context.Taxes;
        }
        public DeTax ReadByCode(string taxCode)
        {
            return context.Taxes.FirstOrDefault(x => x.TaxCode == taxCode);
        }
        public DeTax ReadByValue(double value)
        {
            return context.Taxes.FirstOrDefault(x => x.TaxPercent == value);
        }
        public IEnumerable<DeTax> Read(DeTax obj)
        {
            var data = context.Taxes.ToList();

            if (!string.IsNullOrEmpty(obj.TaxCode))
                data = data.Where(x=>x.TaxCode == obj.TaxCode).ToList();

            if (!string.IsNullOrEmpty(obj.TaxDescription))
                data = data.Where(x => x.TaxDescription == obj.TaxDescription).ToList();

            return data;
        }

        public void Save(DeTax obj)
        {
            var val = context.Taxes.FirstOrDefault(x => x.TaxCode == obj.TaxCode);
            if (val != null)
            {
                val.TaxDescription = obj.TaxDescription;
                val.TaxPercent = obj.TaxPercent;
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.UPDATE), "Impuesto", obj.TaxCode)
                };
                BlActivityLog.Save(activity);
            }
            else
            {
                context.Taxes.Add(obj);
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Impuesto", obj.TaxCode)
                };
                BlActivityLog.Save(activity);
            }
            context.SaveChanges();
        }

        public void Delete(string TaxCode)
        {
            var obj = context.Taxes.FirstOrDefault(x=>x.TaxCode == TaxCode);
            if(obj != null)
            {
                context.Taxes.Remove(obj);
                context.SaveChanges();
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Impuesto", obj.TaxCode)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
