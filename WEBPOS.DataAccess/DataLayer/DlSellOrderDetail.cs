using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.Repository;

namespace WEBPOS.DataAccess.DataLayer
{
    public class DlSellOrderDetail : BaseRepository<WEBPOSContext, DeSellOrderDetail>
    {
        public DlSellOrderDetail(WEBPOSContext context = null) : base(context) { }
        public IEnumerable<DeSellOrderDetail> ReadAll()
        {
            return Context.SellOrderDetails.ToList();
        }
        public IEnumerable<DeSellOrderDetail> ReadAllQueryableCustom(string filters)
        {
            var queryString = $@"SELECT *
FROM srSellOrderDetail
WHERE {filters}";
            return Context.Database.SqlQuery<DeSellOrderDetail>(queryString);
        }
        public IQueryable<DeSellOrderDetail> ReadAllQueryable()
        {
            return Context.SellOrderDetails;
        }
        public IEnumerable<DeSellOrderDetail> Read(DeSellOrderDetail obj)
        {
            var data = Context.SellOrderDetails.Where(x=>x.SellOrderId == obj.SellOrderId).ToList();

            if (obj.LineNumber != 0)
                data = data.Where(x => x.LineNumber == obj.LineNumber).ToList();

            return data;
        }

        public void Save(DeSellOrderDetail obj)
        {
            //var val = Context.SellOrderDetails.FirstOrDefault(x => x.StoreCode == obj.StoreCode && x.StorePosCode == obj.StorePosCode && x.TransactionNumber == obj.TransactionNumber && x.TransactionDateTime == obj.TransactionDateTime);

            Context.SellOrderDetails.Add(obj);

            Context.SaveChanges();
        }

        public void Delete(int sellOrderId, double lineNumber)
        {
            var obj = Context.SellOrderDetails.FirstOrDefault(x => x.SellOrderId == sellOrderId && x.LineNumber == lineNumber);
            if (obj != null)
            {
                Context.SellOrderDetails.Remove(obj);
                Context.SaveChanges();
            }
        }
    }
}
