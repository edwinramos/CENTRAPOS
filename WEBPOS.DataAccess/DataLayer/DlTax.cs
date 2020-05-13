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
    public class DlTax : BaseRepository<WEBPOSContext, DeTax>
    {
        public DlTax(WEBPOSContext context = null) : base(context) { }
        public IEnumerable<DeTax> ReadAll()
        {
            return Context.Taxes.ToList();
        }
        public IQueryable<DeTax> ReadAllQueryable()
        {
            return Context.Taxes;
        }
        public DeTax ReadByCode(string taxCode)
        {
            return Context.Taxes.FirstOrDefault(x => x.TaxCode == taxCode);
        }
        public DeTax ReadByValue(double value)
        {
            return Context.Taxes.FirstOrDefault(x => x.TaxPercent == value);
        }
        public IEnumerable<DeTax> Read(DeTax obj)
        {
            var data = Context.Taxes.ToList();

            if (!string.IsNullOrEmpty(obj.TaxCode))
                data = data.Where(x=>x.TaxCode == obj.TaxCode).ToList();

            if (!string.IsNullOrEmpty(obj.TaxDescription))
                data = data.Where(x => x.TaxDescription == obj.TaxDescription).ToList();

            return data;
        }

        public void Save(DeTax obj)
        {
            var val = Context.Taxes.FirstOrDefault(x => x.TaxCode == obj.TaxCode);
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
                Context.Taxes.Add(obj);
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Impuesto", obj.TaxCode)
                };
                BlActivityLog.Save(activity);
            }
            Context.SaveChanges();
        }

        public void Delete(string TaxCode)
        {
            var obj = Context.Taxes.FirstOrDefault(x=>x.TaxCode == TaxCode);
            if(obj != null)
            {
                Context.Taxes.Remove(obj);
                Context.SaveChanges();
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Impuesto", obj.TaxCode)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
