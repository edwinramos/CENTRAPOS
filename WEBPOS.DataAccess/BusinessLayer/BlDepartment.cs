﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.DataLayer;

namespace WEBPOS.DataAccess.BusinessLayer
{
    public static class BlDepartment
    {
        public static IEnumerable<DeDepartment> ReadAll()
        {
            var dl = new DlDepartment();
            return dl.ReadAll();
        }
        public static IEnumerable<DeDepartment> ReadAllQueryable()
        {
            var dl = new DlDepartment();
            return dl.ReadAll();
        }
        public static IEnumerable<DeDepartment> Read(DeDepartment obj)
        {
            var dl = new DlDepartment();
            return dl.Read(obj);
        }

        public static void Save(DeDepartment obj)
        {
            var dl = new DlDepartment();
            dl.Save(obj);
        }

        public static void Delete(DeDepartment obj)
        {
            var dl = new DlDepartment();
            dl.Delete(obj.DepartmentCode);
        }
    }
}