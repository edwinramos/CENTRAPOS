using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.DataLayer;

namespace WEBPOS.DataAccess.BusinessLayer
{
    public static class BlPosClosureHead
    {
        public static IEnumerable<DePosClosureHead> ReadAll()
        {
            var dl = new DlPosClosureHead();
            return dl.ReadAll();
        }
        public static IEnumerable<DePosClosureHead> ReadAllQueryable()
        {
            var dl = new DlPosClosureHead();
            return dl.ReadAll();
        }
        public static IEnumerable<DePosClosureHead> Read(DePosClosureHead obj)
        {
            var dl = new DlPosClosureHead();
            return dl.Read(obj);
        }

        public static void Save(DePosClosureHead obj)
        {
            var dl = new DlPosClosureHead();
            dl.Save(obj);
        }
    }
}
