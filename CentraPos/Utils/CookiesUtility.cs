using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;


namespace CentraPos.Utils
{
    public class CookiesUtility
    {
        public static void SaveCookieAsString(HttpResponse response, string key, string value)
        {
            CookieOptions option = new CookieOptions();
            var m = EncryptionUtility.Encrypt(value);
            option.Expires = DateTime.Now.AddDays(365);
            response.Cookies.Append(key, m, option);
        }

        public static string ReadCookieAsString(HttpRequest request, string key)
        {
            var cookie = request.Cookies[key];
            return EncryptionUtility.Decrypt(cookie);
        }

        public static void ExpireCookie(string key)
        {
            //var myCookie = HttpContext.Current.Request.Cookies[key];
            //if (myCookie?.Value != null)
            //    myCookie.Expires = DateTime.Now.AddYears(-1);
        }
    }
}