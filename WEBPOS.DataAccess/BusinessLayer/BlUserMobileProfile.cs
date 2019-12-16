using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.DataLayer;

namespace WEBPOS.DataAccess.BusinessLayer
{
    public static class BlUserMobileProfile
    {
        public static IEnumerable<DeUserMobileProfile> ReadAll()
        {
            var dl = new DlUserMobileProfile();
            return dl.ReadAll();
        }
        public static DeUserMobileProfile ReadByCode(string userCode, MobileProfileType mobileProfileCode)
        {
            var dl = new DlUserMobileProfile();
            return dl.ReadByCode(userCode, mobileProfileCode);
        }
        public static IQueryable<DeUserMobileProfile> ReadAllQueryable()
        {
            var dl = new DlUserMobileProfile();
            return dl.ReadAllQueryable();
        }
        public static IEnumerable<DeUserMobileProfile> Read(DeUserMobileProfile obj)
        {
            var dl = new DlUserMobileProfile();
            return dl.Read(obj);
        }

        public static void Save(DeUserMobileProfile obj)
        {
            var dl = new DlUserMobileProfile();
            dl.Save(obj);
        }

        public static void Delete(DeUserMobileProfile obj)
        {
            var dl = new DlUserMobileProfile();
            dl.Delete(obj.UserCode, obj.MobileProfileType);
        }
    }
}
