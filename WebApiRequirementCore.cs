using Newtonsoft.Json;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Providers
{
    public class WebApiRequirementCore
    {
        /**
         *  services.AddAntiforgery(x => x.HeaderName = "X-XSRF-TOKEN");
            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });
         * 
         * */
        public static string CreateCsrfToken(HttpContext context, IAntiforgery antiforgery)
        {
            var tokens = antiforgery.GetAndStoreTokens(context);
            /*string RequestToken = tokens.RequestToken;
            string HeaderName = tokens.HeaderName;*/

            //return tokens.RequestToken + ":" + tokens.HeaderName;
            return tokens.RequestToken;
        }
    }
}