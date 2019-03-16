using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.DataLayer;

namespace WEBPOS.DataAccess.BusinessLayer
{
    public static class BlTax
    {
        public static IEnumerable<DeTax> ReadAll()
        {
            var dl = new DlTax();
            return dl.ReadAll();
        }
        public static IEnumerable<DeTax> ReadAllQueryable()
        {
            var dl = new DlTax();
            return dl.ReadAll();
        }
        public static IEnumerable<DeTax> Read(DeTax obj)
        {
            var dl = new DlTax();
            return dl.Read(obj);
        }

        public static void Save(DeTax obj)
        {
            var dl = new DlTax();
            dl.Save(obj);
        }

        public static void Delete(DeTax obj)
        {
            var dl = new DlTax();
            dl.Delete(obj.TaxCode);
        }
    }
}
