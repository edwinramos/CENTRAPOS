using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.Repository;

namespace WEBPOS.DataAccess.DataLayer
{
    public class DlActivityLog : BaseRepository<WEBPOSContext, DeActivityLog>
    {
        public DlActivityLog(WEBPOSContext context = null) : base(context) { }
        public IEnumerable<DeActivityLog> ReadAll()
        {
            return Context.ActivityLoges.ToList();
        }
        public IQueryable<DeActivityLog> ReadAllQueryable()
        {
            return Context.ActivityLoges;
        }

        public void Save(DeActivityLog obj)
        {
            Context.ActivityLoges.Add(obj);
            Context.SaveChanges();
        }

        public void Delete(int id)
        {
            var obj = Context.ActivityLoges.FirstOrDefault(x => x.ID== id);
            if (obj != null)
            {
                Context.ActivityLoges.Remove(obj);
                Context.SaveChanges();
            }
        }
    }
}
