using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OpenDeepSpace.QuartzDashboard.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.QuartzDashboard.Middlewares
{
    /// <summary>
    /// QuartzDashboard中间件
    /// </summary>
    public class QuartzDashboardMiddleware : IMiddleware
    {


        private readonly QuartzDashboardOptions quartzDashboardOption;

        public QuartzDashboardMiddleware(IOptions<QuartzDashboardOptions> quartzDashboardOptions)
        {
            this.quartzDashboardOption = quartzDashboardOptions.Value;
        }

        public async Task InvokeAsync(HttpContext context,RequestDelegate next)
        {
            //Make sure we are hitting the quartzdashboard path, and not doing it locally as it just gets annoying :-)
            if (context.Request.Path.Value=="/QuartzDashboard" && quartzDashboardOption.IsAuthtication)
            {
                string authHeader = context.Request.Headers["Authorization"];
                if (authHeader != null && authHeader.StartsWith("Basic "))
                {
                    // Get the encoded username and password
                    var encodedUsernamePassword = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();

                    // Decode from Base64 to string
                    var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));

                    // Split username and password
                    var username = decodedUsernamePassword.Split(':', 2)[0];
                    var password = decodedUsernamePassword.Split(':', 2)[1];

                    // Check if login is correct
                    if (IsAuthorized(username, password))
                    {
                        await next(context);
                        return;
                    }
                }

                // Return authentication type (causes browser to show login dialog)
                context.Response.Headers["WWW-Authenticate"] = "Basic";

                // Return unauthorized
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                await next.Invoke(context);
            }
        }

        public bool IsAuthorized(string username, string password)
        {
            // Check that username and password are correct
            return username.Equals(quartzDashboardOption.UserName)
                    && password.Equals(quartzDashboardOption.Password);
        }

        public bool IsLocalRequest(HttpContext context)
        {
            //Handle running using the Microsoft.AspNetCore.TestHost and the site being run entirely locally in memory without an actual TCP/IP connection
            if (context.Connection.RemoteIpAddress == null && context.Connection.LocalIpAddress == null)
            {
                return true;
            }
            if (context.Connection.RemoteIpAddress.Equals(context.Connection.LocalIpAddress))
            {
                return true;
            }
            if (IPAddress.IsLoopback(context.Connection.RemoteIpAddress))
            {
                return true;
            }
            return false;
        }
    }
}
