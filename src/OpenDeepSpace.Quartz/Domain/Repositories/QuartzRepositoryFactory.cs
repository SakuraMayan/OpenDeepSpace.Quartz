using Quartz.Impl.AdoJobStore;
using Quartz.Impl.AdoJobStore.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.Quartz.Domain.Repositories
{
    /// <summary>
    /// Quartz仓储工厂
    /// </summary>
    public class QuartzRepositoryFactory
    {
        /// <summary>
        /// 获取Quartz仓储 
        /// </summary>
        /// <param name="DbProviderName"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public static IQuartzRepository GetQuartzRepository(string DbProviderName,string ConnectionString)
        {
            IDbProvider dbProvider = new DbProvider(DbProviderName, ConnectionString);
            IQuartzRepository quartzRepository = null;

            switch (DbProviderName)
            {
                case "MySql"://
                    quartzRepository = new QuartzMySqlRepository(dbProvider);
                    break;
                case "SqlServer":
                    quartzRepository=new QuartzSqlServerRepository(dbProvider);
                    break;
                default:
                    break;
            }


            if (quartzRepository == null)
                throw new ArgumentNullException("未找到匹配的数据提供者:"+nameof(quartzRepository));

            return quartzRepository;
        }
    }
}
