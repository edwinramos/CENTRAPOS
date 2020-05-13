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
    public class DlUnitMeasure
    {
        private CentraPosDBContext context = new CentraPosDBContext();
        public IEnumerable<DeUnitMeasure> ReadAll()
        {
            return context.UnitMeasures.ToList();
        }
        public IQueryable<DeUnitMeasure> ReadAllQueryable()
        {
            return context.UnitMeasures;
        }
        public IEnumerable<DeUnitMeasure> Read(DeUnitMeasure obj)
        {
            var data = context.UnitMeasures.ToList();

            if (!string.IsNullOrEmpty(obj.UnitMeasureCode))
                data = data.Where(x=>x.UnitMeasureCode == obj.UnitMeasureCode).ToList();

            if (!string.IsNullOrEmpty(obj.UnitMeasureDescription))
                data = data.Where(x => x.UnitMeasureDescription == obj.UnitMeasureDescription).ToList();

            return data;
        }

        public void Save(DeUnitMeasure obj)
        {
            var val = context.UnitMeasures.FirstOrDefault(x => x.UnitMeasureCode == obj.UnitMeasureCode);
            if (val != null)
            {
                val.UnitMeasureDescription = obj.UnitMeasureDescription;
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.UPDATE), "Unidad de medida", obj.UnitMeasureCode)
                };
                BlActivityLog.Save(activity);
            }
            else
            {
                context.UnitMeasures.Add(obj);
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Unidad de medida", obj.UnitMeasureCode)
                };
                BlActivityLog.Save(activity);
            }
            
            context.SaveChanges();
        }

        public void Delete(string UnitMeasureCode)
        {
            var obj = context.UnitMeasures.FirstOrDefault(x=>x.UnitMeasureCode == UnitMeasureCode);
            if(obj != null)
            {
                context.UnitMeasures.Remove(obj);
                context.SaveChanges();

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Unidad de medida", obj.UnitMeasureCode)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
