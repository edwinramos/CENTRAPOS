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
    public class DlTable : BaseRepository<WEBPOSContext, DeTable>
    {
        public DlTable(WEBPOSContext context = null) : base(context) { }
        public IEnumerable<DeTable> ReadAll()
        {
            return Context.Tables.ToList();
        }
        public IQueryable<DeTable> ReadAllQueryable()
        {
            return Context.Tables;
        }
        public IEnumerable<DeTable> Read(DeTable obj)
        {
            var data = Context.Tables.ToList();

            if (!string.IsNullOrEmpty(obj.KeyFixed))
                data = data.Where(x=>x.KeyFixed == obj.KeyFixed).ToList();

            if (!string.IsNullOrEmpty(obj.KeyVariable))
                data = data.Where(x => x.KeyVariable == obj.KeyVariable).ToList();

            return data;
        }

        public void Save(DeTable obj)
        {
            var val = Context.Tables.FirstOrDefault(x => x.KeyFixed == obj.KeyFixed);
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
                Context.Tables.Add(obj);
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Tabla", obj.KeyFixed)
                };
                BlActivityLog.Save(activity);
            }
            Context.SaveChanges();
        }

        public void Delete(string keyFixed)
        {
            var obj = Context.Tables.FirstOrDefault(x=>x.KeyFixed == keyFixed);
            if(obj != null)
            {
                Context.Tables.Remove(obj);
                Context.SaveChanges();
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Impuesto", obj.KeyFixed)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
