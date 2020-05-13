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
    public class DlPosClosureHead : BaseRepository<WEBPOSContext, DePosClosureHead>
    {
        public DlPosClosureHead(WEBPOSContext context = null) : base(context) { }
        public IEnumerable<DePosClosureHead> ReadAll()
        {
            return Context.PosClosureHeads.ToList();
        }
        public IQueryable<DePosClosureHead> ReadAllQueryable()
        {
            return Context.PosClosureHeads;
        }
        public IEnumerable<DePosClosureHead> Read(DePosClosureHead obj)
        {
            var data = Context.PosClosureHeads.ToList();

            if (obj.PosClosureHeadId != 0)
                data = data.Where(x => x.PosClosureHeadId == obj.PosClosureHeadId).ToList();

            if (!string.IsNullOrEmpty(obj.StoreCode))
                data = data.Where(x => x.StoreCode == obj.StoreCode).ToList();

            if (!string.IsNullOrEmpty(obj.StorePosCode))
                data = data.Where(x => x.StorePosCode == obj.StorePosCode).ToList();

            if (!string.IsNullOrEmpty(obj.UserCode))
                data = data.Where(x => x.UserCode == obj.UserCode).ToList();

            return data;
        }

        public void Save(DePosClosureHead obj)
        {
            var db = Context.PosClosureHeads.FirstOrDefault(x => x.PosClosureHeadId == obj.PosClosureHeadId);
            if (db == null)
                Context.PosClosureHeads.Add(obj);
            else
            {
                db.StartDateTime = obj.StartDateTime;
                db.EndDateTime = obj.EndDateTime;
                db.Total = obj.Total;
            }
            //var activity = new DeActivityLog
            //{
            //    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Cierre de caja", obj.PosCode + "|" + obj.TransactionNumber)
            //};
            //BlActivityLog.Save(activity);

            Context.SaveChanges();
        }
    }
}
