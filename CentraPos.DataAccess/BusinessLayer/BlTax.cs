using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentraPos.DataAccess.DataEntities;
using CentraPos.DataAccess.DataLayer;

namespace CentraPos.DataAccess.BusinessLayer
{
    public static class BlTax
    {
        public static IEnumerable<DeTax> ReadAll()
        {
            var dl = new DlTax();
            return dl.ReadAll();
        }
        public static IQueryable<DeTax> ReadAllQueryable()
        {
            var dl = new DlTax();
            return dl.ReadAllQueryable();
        }
        public static DeTax ReadByValue(double value)
        {
            var dl = new DlTax();
            return dl.ReadByValue(value);
        }
        public static DeTax ReadByCode(string taxCode)
        {
            var dl = new DlTax();
            return dl.ReadByCode(taxCode);
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
