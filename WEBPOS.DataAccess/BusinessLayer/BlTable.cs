using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.DataLayer;

namespace WEBPOS.DataAccess.BusinessLayer
{
    public static class BlTable
    {
        public static IEnumerable<DeTable> ReadAll()
        {
            var dl = new DlTable();
            return dl.ReadAll();
        }
        public static IEnumerable<DeTable> ReadAllQueryable()
        {
            var dl = new DlTable();
            return dl.ReadAll();
        }
        public static IEnumerable<DeTable> Read(DeTable obj)
        {
            var dl = new DlTable();
            return dl.Read(obj);
        }

        public static void Save(DeTable obj)
        {
            var dl = new DlTable();
            dl.Save(obj);
        }

        public static void Delete(DeTable obj)
        {
            var dl = new DlTable();
            dl.Delete(obj.KeyFixed);
        }
    }
}
