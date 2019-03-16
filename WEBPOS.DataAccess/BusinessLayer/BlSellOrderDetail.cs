using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.DataLayer;

namespace WEBPOS.DataAccess.BusinessLayer
{
    public static class BlSellOrderDetail
    {
        public static IEnumerable<DeSellOrderDetail> ReadAll()
        {
            var dl = new DlSellOrderDetail();
            return dl.ReadAll();
        }
        public static IEnumerable<DeSellOrderDetail> ReadAllQueryable()
        {
            var dl = new DlSellOrderDetail();
            return dl.ReadAll();
        }
        public static IEnumerable<DeSellOrderDetail> Read(DeSellOrderDetail obj)
        {
            var dl = new DlSellOrderDetail();
            return dl.Read(obj);
        }

        public static void Save(DeSellOrderDetail obj)
        {
            var dl = new DlSellOrderDetail();
            var list = dl.ReadAll().Where(x => x.SellOrderId == obj.SellOrderId);
            var count = 1;
            
            if (obj.LineNumber == 0)
            {
                count = list.Count() + 1;
                obj.LineNumber = count;
            }
            else
            {
                var detail = dl.ReadAll().FirstOrDefault(x => x.SellOrderId == obj.SellOrderId && x.LineNumber == obj.LineNumber);
                dl.Delete(detail.SellOrderId, detail.LineNumber);
            }

            dl.Save(obj);
        }

        public static void Delete(DeSellOrderDetail obj)
        {
            var dl = new DlSellOrderDetail();
            dl.Delete(obj.SellOrderId, obj.LineNumber);
        }
    }
}
