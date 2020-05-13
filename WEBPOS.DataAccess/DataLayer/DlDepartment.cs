using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.BusinessLayer;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.Helpers;
using WEBPOS.DataAccess.Repository;

namespace WEBPOS.DataAccess.DataLayer
{
    public class DlDepartment : BaseRepository<WEBPOSContext, DeDepartment>
    {
        public DlDepartment(WEBPOSContext context = null) : base(context) { }
        public IEnumerable<DeDepartment> ReadAll()
        {
            return Context.Departments.ToList();
        }
        public IQueryable<DeDepartment> ReadAllQueryable()
        {
            return Context.Departments;
        }
        public IEnumerable<DeDepartment> Read(DeDepartment obj)
        {
            var data = Context.Departments.ToList();

            if (!string.IsNullOrEmpty(obj.DepartmentCode))
                data = data.Where(x=>x.DepartmentCode == obj.DepartmentCode).ToList();

            if (!string.IsNullOrEmpty(obj.DepartmentDescription))
                data = data.Where(x => x.DepartmentDescription == obj.DepartmentDescription).ToList();

            return data;
        }

        public void Save(DeDepartment obj)
        {
            var val = Context.Departments.FirstOrDefault(x => x.DepartmentCode == obj.DepartmentCode);
            if (val != null)
            {
                val.DepartmentDescription = obj.DepartmentDescription;

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.UPDATE), "Departamento", obj.DepartmentCode)
                };
                BlActivityLog.Save(activity);
            }
            else
            {
                Context.Departments.Add(obj);

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Departamento", obj.DepartmentCode)
                };
                BlActivityLog.Save(activity);
            }
            Context.SaveChanges();
        }

        public void Delete(string DepartmentCode)
        {
            var obj = Context.Departments.FirstOrDefault(x=>x.DepartmentCode == DepartmentCode);
            if(obj != null)
            {
                Context.Departments.Remove(obj);
                Context.SaveChanges();

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Departamento", obj.DepartmentCode)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
