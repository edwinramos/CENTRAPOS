using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentraPos.DataAccess.BusinessLayer;
using CentraPos.DataAccess.DataEntities;
using CentraPos.DataAccess.Helpers;

namespace CentraPos.DataAccess.DataLayer
{
    public class DlUserMobileProfile
    {
        private CentraPosDBContext context = new CentraPosDBContext();
        public IEnumerable<DeUserMobileProfile> ReadAll()
        {
            return context.UserMobileProfiles.ToList();
        }
        public IQueryable<DeUserMobileProfile> ReadAllQueryable()
        {
            return context.UserMobileProfiles;
        }

        public DeUserMobileProfile ReadByCode(string userCode, MobileProfileType mobileProfileCode)
        {
            return context.UserMobileProfiles.FirstOrDefault(x => x.UserCode == userCode && x.MobileProfileType == mobileProfileCode);
        }

        public IEnumerable<DeUserMobileProfile> Read(DeUserMobileProfile obj)
        {
            var data = context.UserMobileProfiles.ToList();

            if (!string.IsNullOrEmpty(obj.UserCode))
                data = data.Where(x => x.UserCode == obj.UserCode).ToList();

            data = data.Where(x => x.MobileProfileType == obj.MobileProfileType).ToList();

            if (!data.All(x => x.UserCode == obj.UserCode))
                return new List<DeUserMobileProfile>();

            return data;
        }

        public void Save(DeUserMobileProfile obj)
        {
            var val = context.UserMobileProfiles.FirstOrDefault(x => x.UserCode == obj.UserCode && x.UserCode == obj.UserCode);
            if (val != null)
            {
                val.MobileProfileType = obj.MobileProfileType;
                val.Param1 = obj.Param1;
                val.Param2 = obj.Param2;
            }
            else
                context.UserMobileProfiles.Add(obj);

            context.SaveChanges();
        }

        public void Delete(string userCode, MobileProfileType mobileProfileCode)
        {
            var obj = context.UserMobileProfiles.FirstOrDefault(x => x.UserCode == userCode && x.MobileProfileType == mobileProfileCode);
            if (obj != null)
            {
                context.UserMobileProfiles.Remove(obj);
                context.SaveChanges();
            }
        }
    }
}
