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
    public class DlPosClosureDetail
    {
        private WEBPOSContext context = new WEBPOSContext();
        public IEnumerable<DePosClosureDetail> ReadAll()
        {
            return context.PosClosureDetails.ToList();
        }
        public IQueryable<DePosClosureDetail> ReadAllQueryable()
        {
            return context.PosClosureDetails;
        }
        public IEnumerable<DePosClosureDetail> Read(DePosClosureDetail obj)
        {
            var data = context.PosClosureDetails.ToList();

            if (obj.PosClosureHeadId != 0)
                data = data.Where(x => x.PosClosureHeadId == obj.PosClosureHeadId).ToList();

            if (!string.IsNullOrEmpty(obj.StoreCode))
                data = data.Where(x => x.StoreCode == obj.StoreCode).ToList();

            if (!string.IsNullOrEmpty(obj.StorePosCode))
                data = data.Where(x => x.StorePosCode == obj.StorePosCode).ToList();

            return data;
        }

        public void Save(DePosClosureDetail obj)
        {
            context.PosClosureDetails.Add(obj);
            //var activity = new DeActivityLog
            //{
            //    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Cierre de caja", obj.PosCode + "|" + obj.TransactionNumber)
            //};
            //BlActivityLog.Save(activity);

            context.SaveChanges();
        }
    }
}
