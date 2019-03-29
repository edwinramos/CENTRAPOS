using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WEBPOS.DataAccess.DataEntities;

namespace WEBPOS.Models
{
    public class UserModel
    {
        public string UserCode { get; set; }
        
        public UserType UserType { get; set; }
        
        public string Password { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }
        
        public Gender Gender { get; set; }

        public string IsEditingString { get; set; }
    }
}