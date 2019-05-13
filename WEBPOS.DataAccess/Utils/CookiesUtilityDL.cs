using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBPOS.Utils
{
    public class CookiesUtilityDL
    {
        public static void SaveCookieAsString(string key, string value)
        {
            string newValue = "";
            if (key == "MenuExpand")
                newValue = value;
            else
                newValue = EncryptionUtility.Encrypt(value);

            var cookie = new HttpCookie(key) { Expires = DateTime.Now.AddDays(365), Value = newValue };
            HttpContext.Current.Response.SetCookie(cookie);
        }

        public static string ReadCookieAsString(string key)
        {
            var myCookie = HttpContext.Current.Request.Cookies[key];
            if (myCookie?.Value != null)
            {
                if (key == "MenuExpand")
                    return myCookie?.Value;
                else
                    return EncryptionUtility.Decrypt(myCookie?.Value);
            }
            else
                return myCookie?.Value;
        }

        public static void ExpireCookie(string key)
        {
            var myCookie = HttpContext.Current.Request.Cookies[key];
            if (myCookie?.Value != null)
                myCookie.Expires = DateTime.Now.AddYears(-1);
        }
    }
}