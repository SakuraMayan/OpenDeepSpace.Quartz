using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.QuartzDashboard.Application.Dtos
{
    /// <summary>
    /// 结果基类Dto
    /// </summary>
    public class ResultBaseDto
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 失败的消息
        /// </summary>
        public string FailedMsg { get; set; }
    }
}
