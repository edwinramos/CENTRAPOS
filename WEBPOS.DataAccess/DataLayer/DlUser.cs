using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.BusinessLayer;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.Helpers;
using WEBPOS.DataAccess.Repository;

namespace WEBPOS.DataAccess.DataLayer
{
    public class DlUser : BaseRepository<WEBPOSContext, DeUnitMeasure>
    {
        public DlUser(WEBPOSContext context = null) : base(context) { }
        public IEnumerable<DeUser> ReadAll()
        {
            return Context.Users.ToList();
        }
        public IQueryable<DeUser> ReadAllQueryable()
        {
            return Context.Users;
        }
        public IEnumerable<DeUser> Read(DeUser obj)
        {
            var data = Context.Users.ToList();

            if (!string.IsNullOrEmpty(obj.UserCode))
                data = data.Where(x=>x.UserCode == obj.UserCode).ToList();

            return data;
        }

        public void Save(DeUser obj)
        {
            var val = Context.Users.FirstOrDefault(x => x.UserCode == obj.UserCode);
            var activity = new DeActivityLog();
            if (val != null)
            {
                val.Gender = obj.Gender;
                val.UserType = obj.UserType;
                val.Name = obj.Name;
                val.LastName = obj.LastName;
                val.IsEditing = obj.IsEditing;

                activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.UPDATE), "Usuario", obj.UserCode)
                };

            }
            else
            {
                Context.Users.Add(obj);
                activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Usuario", obj.UserCode)
                };
            }

            BlActivityLog.Save(activity);
            Context.SaveChanges();
        }

        public void Delete(string userCode)
        {
            var obj = Context.Users.FirstOrDefault(x=>x.UserCode == userCode);
            if(obj != null)
            {
                Context.Users.Remove(obj);
                Context.SaveChanges();

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Usuario", obj.UserCode)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
