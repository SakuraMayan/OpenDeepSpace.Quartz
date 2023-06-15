using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.QuartzDashboard.Options
{
    /// <summary>
    /// QuartzDashboardOptions
    /// </summary>
    public class QuartzDashboardOptions
    {
        /// <summary>
        /// 是否需要认证
        /// </summary>
        public bool IsAuthtication { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

    }
}
