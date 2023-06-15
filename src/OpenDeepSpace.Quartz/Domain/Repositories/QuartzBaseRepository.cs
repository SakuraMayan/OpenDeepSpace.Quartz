using Quartz.Impl.AdoJobStore.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.Quartz.Domain.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class QuartzBaseRepository : IQuartzRepository
    {
        IDbProvider dbProvider;

        public QuartzBaseRepository(IDbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
        }

        public abstract Task<int> InitTables();
        
    }
}
