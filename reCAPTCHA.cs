

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;

namespace dotNetCoreV2.Providers
{
    public class reCAPTCHAAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var validationResult = CheckReCaptchaResponseAsync(context.HttpContext.Request?.Form["g-recaptcha-response"]).Result;

                if (validationResult == false)
                {
                    context.HttpContext.Response.StatusCode = 403;
                }
            }
            catch (Exception e)
            {
                context.Result = new ContentResult()
                {
                    Content = "Try again"
                };
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        private static async Task<bool> CheckReCaptchaResponseAsync(string rcResponse)
        {
            var secret = "6Ldrf8UUAAAAAAHm3kJ-Q_laqytBT149SA5rmKvR";

            if (string.IsNullOrWhiteSpace(secret))
                return true;
            if (string.IsNullOrWhiteSpace(rcResponse))
                throw new ApplicationException("reCaptcha response is missing.");

            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(
                    new Dictionary<string, string>
                    {
                        { "secret", secret },
                        { "response", rcResponse }
                    });
                var response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
                var responseString = await response.Content.ReadAsStringAsync();
                var jReponse = JObject.Parse(responseString);

                if (!bool.TryParse((string)jReponse["success"], out bool success) || !success)
                    return false;
                return true;
            }
        }
    }
}
