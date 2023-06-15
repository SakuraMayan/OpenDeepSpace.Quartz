using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using OpenDeepSpace.Quartz.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.Quartz.Extensions
{
    /// <summary>
    /// Quartz拓展
    /// </summary>
    public static class QuartzExtensions
    {

        /// <summary>
        /// 添加集成的Quartz
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddIntegrationQuartz(this IServiceCollection services)
        {

            var configuration=services.BuildServiceProvider().GetRequiredService<IConfiguration>();

            //初始化Quartz所需持久化的表到库
            QuartzRepositoryFactory.GetQuartzRepository(configuration["Quartz:AdoProvider:DbProviderName"],configuration["Quartz:AdoProvider:ConnectionString"]).InitTables();

            //QuartzOptions配置
            services.Configure<QuartzOptions>(configuration.GetSection("Quartz:Options"));

            //QuartzOptions的其他配置 具体配置哪些还需要具体参考
            services.Configure<QuartzOptions>(opt =>
            {
                opt.Scheduling.OverWriteExistingData = true;
                opt.Scheduling.IgnoreDuplicates = true;
            });

            //配置持久化的数据库的其他选项
            services.AddQuartz(opt =>
            {
                opt.UsePersistentStore(opt =>
                {
                    opt.UseProperties = configuration.GetValue<bool>("Quartz:PersistentStore:UseProperties");
                    opt.RetryInterval = TimeSpan.FromSeconds(configuration.GetValue<int>("Quartz:PersistentStore:RetryInterval"));
                    
                    //序列化
                    opt.UseJsonSerializer();

                    opt.UseClustering(opt => { //集群配置
                        opt.CheckinInterval = TimeSpan.FromSeconds(configuration.GetValue<int>("Quartz:Cluster:CheckinInterval"));
                        opt.CheckinMisfireThreshold = TimeSpan.FromSeconds(configuration.GetValue<int>("Quartz:Cluster:CheckinMisfireThreshold"));
                    });
                });

                //以来注入工厂
                opt.UseMicrosoftDependencyInjectionJobFactory();

                //配置线程池大小
                opt.UseDefaultThreadPool(opt => {
                    opt.MaxConcurrency = configuration.GetValue<int>("Quartz:ThreadPool:MaxConcurrency");
                });

            });


            //添加Quartz服务
            services.AddQuartzServer(opt =>
            {
                opt.WaitForJobsToComplete = configuration.GetValue<bool>("Quartz:QuartzHostedService:WaitForJobsToComplete");
            });

            return services;

        }

    }
}
