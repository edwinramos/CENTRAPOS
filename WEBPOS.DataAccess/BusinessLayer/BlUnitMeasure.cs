using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.DataLayer;

namespace WEBPOS.DataAccess.BusinessLayer
{
    public static class BlUnitMeasure
    {
        public static IEnumerable<DeUnitMeasure> ReadAll()
        {
            var dl = new DlUnitMeasure();
            return dl.ReadAll();
        }
        public static IQueryable<DeUnitMeasure> ReadAllQueryable()
        {
            var dl = new DlUnitMeasure();
            return dl.ReadAllQueryable();
        }
        public static IEnumerable<DeUnitMeasure> Read(DeUnitMeasure obj)
        {
            var dl = new DlUnitMeasure();
            return dl.Read(obj);
        }

        public static void Save(DeUnitMeasure obj)
        {
            var dl = new DlUnitMeasure();
            dl.Save(obj);
        }

        public static void Delete(DeUnitMeasure obj)
        {
            var dl = new DlUnitMeasure();
            dl.Delete(obj.UnitMeasureCode);
        }
    }
}
