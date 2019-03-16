using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.BusinessLayer;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.Helpers;

namespace WEBPOS.DataAccess.DataLayer
{
    public class DlDepartment
    {
        private WEBPOSContext context = new WEBPOSContext();
        public IEnumerable<DeDepartment> ReadAll()
        {
            return context.Departments.ToList();
        }
        public IEnumerable<DeDepartment> ReadAllQueryable()
        {
            return context.Departments;
        }
        public IEnumerable<DeDepartment> Read(DeDepartment obj)
        {
            var data = context.Departments.ToList();

            if (!string.IsNullOrEmpty(obj.DepartmentCode))
                data = data.Where(x=>x.DepartmentCode == obj.DepartmentCode).ToList();

            if (!string.IsNullOrEmpty(obj.DepartmentDescription))
                data = data.Where(x => x.DepartmentDescription == obj.DepartmentDescription).ToList();

            return data;
        }

        public void Save(DeDepartment obj)
        {
            var val = context.Departments.FirstOrDefault(x => x.DepartmentCode == obj.DepartmentCode);
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
                context.Departments.Add(obj);

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Departamento", obj.DepartmentCode)
                };
                BlActivityLog.Save(activity);
            }
            context.SaveChanges();
        }

        public void Delete(string DepartmentCode)
        {
            var obj = context.Departments.FirstOrDefault(x=>x.DepartmentCode == DepartmentCode);
            if(obj != null)
            {
                context.Departments.Remove(obj);
                context.SaveChanges();

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Departamento", obj.DepartmentCode)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
