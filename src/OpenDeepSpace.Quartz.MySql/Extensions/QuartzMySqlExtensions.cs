using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.Quartz.MySql.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class QuartzMySqlExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddQuartzMySql(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

            services.AddQuartz(opt =>
            {
                opt.UsePersistentStore(opt => {

                    opt.UseMySql(mysql =>
                    {
                        mysql.ConnectionString = configuration["Quartz:AdoProvider:ConnectionString"];
                        mysql.TablePrefix = configuration["Quartz:AdoProvider:TablePrefix"];
                    });
                });
            });

            return services;
        }
    }
}
