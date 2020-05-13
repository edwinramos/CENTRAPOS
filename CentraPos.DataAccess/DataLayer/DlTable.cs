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
    public class DlTable
    {
        private CentraPosDBContext context = new CentraPosDBContext();
        public IEnumerable<DeTable> ReadAll()
        {
            return context.Tables.ToList();
        }
        public IQueryable<DeTable> ReadAllQueryable()
        {
            return context.Tables;
        }
        public IEnumerable<DeTable> Read(DeTable obj)
        {
            var data = context.Tables.ToList();

            if (!string.IsNullOrEmpty(obj.KeyFixed))
                data = data.Where(x=>x.KeyFixed == obj.KeyFixed).ToList();

            if (!string.IsNullOrEmpty(obj.KeyVariable))
                data = data.Where(x => x.KeyVariable == obj.KeyVariable).ToList();

            return data;
        }

        public void Save(DeTable obj)
        {
            var val = context.Tables.FirstOrDefault(x => x.KeyFixed == obj.KeyFixed);
            if (val != null)
            {
                val.KeyFixed = obj.KeyFixed;
                val.KeyVariable = obj.KeyVariable;
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.UPDATE), "Tabla", obj.KeyFixed)
                };
                BlActivityLog.Save(activity);
            }
            else
            {
                context.Tables.Add(obj);
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Tabla", obj.KeyFixed)
                };
                BlActivityLog.Save(activity);
            }
            context.SaveChanges();
        }

        public void Delete(string keyFixed)
        {
            var obj = context.Tables.FirstOrDefault(x=>x.KeyFixed == keyFixed);
            if(obj != null)
            {
                context.Tables.Remove(obj);
                context.SaveChanges();
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Impuesto", obj.KeyFixed)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
