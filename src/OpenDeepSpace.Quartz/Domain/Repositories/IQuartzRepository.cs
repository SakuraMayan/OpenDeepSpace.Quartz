using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.Quartz.Domain.Repositories
{
    /// <summary>
    /// Quartz仓储
    /// </summary>
    public interface IQuartzRepository
    {
        /// <summary>
        /// 初始化表
        /// </summary>
        /// <returns></returns>
        public Task<int> InitTables();
    }
}
