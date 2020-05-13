using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentraPos.DataAccess.DataEntities;

namespace CentraPos.DataAccess.DataLayer
{
    public class DlActivityLog
    {
        private CentraPosDBContext context = new CentraPosDBContext();
        public IEnumerable<DeActivityLog> ReadAll()
        {
            return context.ActivityLoges.ToList();
        }
        public IQueryable<DeActivityLog> ReadAllQueryable()
        {
            return context.ActivityLoges;
        }

        public void Save(DeActivityLog obj)
        {
            context.ActivityLoges.Add(obj);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var obj = context.ActivityLoges.FirstOrDefault(x => x.ID== id);
            if (obj != null)
            {
                context.ActivityLoges.Remove(obj);
                context.SaveChanges();
            }
        }
    }
}
