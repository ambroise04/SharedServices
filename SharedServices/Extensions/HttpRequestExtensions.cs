using Microsoft.AspNetCore.Http;
using System;

namespace SharedServices.UI.Extensions
{
    public static class HttpRequestExtensions
    {
        public static bool IsAjax(this HttpRequest request, string httpVerb = "")
        {
            if (request == null)
            {
                throw new ArgumentNullException("Request object is Null.");
            }

            if (!string.IsNullOrEmpty(httpVerb))
            {
                if (request.Method.ToLower() != httpVerb.ToLower())
                {
                    return false;
                }
            }

            if (request.Headers != null)
            {
                return request.Headers["X-Requested-With"].Equals("XMLHttpRequest");
            }

            return false;
        }
    }
}