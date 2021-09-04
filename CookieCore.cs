using Microsoft.AspNetCore.Http;
using System;

namespace Providers
{
    public class CookieCore 
    {
        public static void SetCookie(IHttpContextAccessor _httpContextAccessor, string name, string value, string date, int time)
        {
            CookieOptions Option = new CookieOptions();

            switch (date)
            {
                case "second":
                    Option.Expires = DateTime.Now.AddSeconds(time);
                    break;
                case "minute":
                    Option.Expires = DateTime.Now.AddMinutes(time);
                    break;
                case "hour":
                    Option.Expires = DateTime.Now.AddHours(time);
                    break;
                case "day":
                    Option.Expires = DateTime.Now.AddDays(time);
                    break;
                case "month":
                    Option.Expires = DateTime.Now.AddMonths(time);
                    break;
                case "year":
                    Option.Expires = DateTime.Now.AddYears(time);
                    break;
            }

            _httpContextAccessor.HttpContext.Response.Cookies.Append(name, value,
                new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(2),
                    HttpOnly = true
                }
            );
        }



        public static string GetCookie(IHttpContextAccessor _httpContextAccessor, string name)
        {
            return _httpContextAccessor.HttpContext.Request.Cookies[name].ToString();
        }


        
        public static bool ExistCookie(IHttpContextAccessor _httpContextAccessor, string name)
        {
            string cookie = _httpContextAccessor.HttpContext.Request.Cookies[name];

            if (cookie != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        
        public static void RemoveCookie(IHttpContextAccessor _httpContextAccessor, string name)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(name);
        }
    }
}