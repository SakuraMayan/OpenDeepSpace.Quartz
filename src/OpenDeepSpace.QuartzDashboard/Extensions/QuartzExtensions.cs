using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using OpenDeepSpace.QuartzDashboard.Application;
using OpenDeepSpace.QuartzDashboard.Application.Contracts;
using OpenDeepSpace.QuartzDashboard.Middlewares;
using OpenDeepSpace.QuartzDashboard.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.QuartzDashboard.Extensions
{
    /// <summary>
    /// Quartz拓展
    /// </summary>
    public static class QuartzExtensions
    {

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddQuartzDashboard(this IServiceCollection services)
        {
            services.AddRazorPages(opt => {

                //添加允许匿名的页面或区域
                opt.Conventions.AllowAnonymousToPage("/QuartzDashboard");
            });
            services.AddTransient<IQuartzService, QuartzService>();
            var configuration=services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            services.Configure<QuartzDashboardOptions>(configuration.GetSection("Quartz:Dashboard"));
            services.AddTransient<IMiddleware, QuartzDashboardMiddleware>();

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseQuartzDashboard(this IApplicationBuilder builder)
        {
            
            //通过添加中间件来完成安全认证
            builder.UseMiddleware<QuartzDashboardMiddleware>();
            builder.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

            });


            return builder;
        }
    }
}
