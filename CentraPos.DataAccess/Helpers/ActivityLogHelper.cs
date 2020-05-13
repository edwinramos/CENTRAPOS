using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentraPos.DataAccess.Helpers
{
    public static class ActivityLogHelper
    {
        public static string GetActivityText(LogActivities activity)
        {
            string msg = "";
            switch (activity)
            {
                case LogActivities.SIGNOUT:
                    msg = "cerró sesión.";
                    break;
                case LogActivities.SIGNIN:
                    msg = "inició sesión.";
                    break;
                case LogActivities.CREATE:
                    msg = "{0} '{1}' ha sido creado.";
                    break;
                case LogActivities.UPDATE:
                    msg = "{0} '{1}' ha sido modificado.";
                    break;
                case LogActivities.DELETE:
                    msg = "{0} '{1}' ha sido eliminado.";
                    break;
                default:
                    break;
            }
            return msg;
        }
    }

    public enum LogActivities
    {
        SIGNOUT=0,
        SIGNIN=1,
        CREATE=2,
        UPDATE=3,
        DELETE=4
    }
}