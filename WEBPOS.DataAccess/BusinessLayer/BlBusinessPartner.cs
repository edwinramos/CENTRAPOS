using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.DataLayer;

namespace WEBPOS.DataAccess.BusinessLayer
{
    public static class BlBusinessPartner
    {
        public static IEnumerable<DeBusinessPartner> ReadAll()
        {
            var dl = new DlBusinessPartner();
            return dl.ReadAll();
        }
        public static IEnumerable<DeBusinessPartner> ReadAllQueryable()
        {
            var dl = new DlBusinessPartner();
            return dl.ReadAll();
        }
        public static IEnumerable<DeBusinessPartner> Read(DeBusinessPartner obj)
        {
            var dl = new DlBusinessPartner();
            return dl.Read(obj);
        }

        public static void Save(DeBusinessPartner obj)
        {
            var dl = new DlBusinessPartner();
            dl.Save(obj);
        }

        public static void Delete(DeBusinessPartner obj)
        {
            var dl = new DlBusinessPartner();
            dl.Delete(obj.BusinessPartnerCode);
        }

        public static string GetNextBusinessPartnerCode()
        {
            var count = ReadAll().Count() + 1;
            string str = count.ToString().PadLeft(7, '0');

            while (ReadAllQueryable().Any(x => x.BusinessPartnerCode == str))
            {
                count++;
                str = count.ToString().PadRight(7, '0');
            }

            return str;
        }
    }
}
