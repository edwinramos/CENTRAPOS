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
    public class DlUnitMeasure : BaseRepository<WEBPOSContext, DeUnitMeasure>
    {
        public DlUnitMeasure(WEBPOSContext context = null) : base(context) { }
        public IEnumerable<DeUnitMeasure> ReadAll()
        {
            return Context.UnitMeasures.ToList();
        }
        public IQueryable<DeUnitMeasure> ReadAllQueryable()
        {
            return Context.UnitMeasures;
        }
        public IEnumerable<DeUnitMeasure> Read(DeUnitMeasure obj)
        {
            var data = Context.UnitMeasures.ToList();

            if (!string.IsNullOrEmpty(obj.UnitMeasureCode))
                data = data.Where(x=>x.UnitMeasureCode == obj.UnitMeasureCode).ToList();

            if (!string.IsNullOrEmpty(obj.UnitMeasureDescription))
                data = data.Where(x => x.UnitMeasureDescription == obj.UnitMeasureDescription).ToList();

            return data;
        }

        public void Save(DeUnitMeasure obj)
        {
            var val = Context.UnitMeasures.FirstOrDefault(x => x.UnitMeasureCode == obj.UnitMeasureCode);
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
                Context.UnitMeasures.Add(obj);
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Unidad de medida", obj.UnitMeasureCode)
                };
                BlActivityLog.Save(activity);
            }
            
            Context.SaveChanges();
        }

        public void Delete(string UnitMeasureCode)
        {
            var obj = Context.UnitMeasures.FirstOrDefault(x=>x.UnitMeasureCode == UnitMeasureCode);
            if(obj != null)
            {
                Context.UnitMeasures.Remove(obj);
                Context.SaveChanges();

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Unidad de medida", obj.UnitMeasureCode)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
