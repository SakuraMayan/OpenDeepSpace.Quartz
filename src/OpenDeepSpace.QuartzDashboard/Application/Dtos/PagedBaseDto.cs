using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.QuartzDashboard.Application.Dtos
{
    /// <summary>
    /// 分页基类
    /// </summary>
    public class PagedBaseDto
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage { get; set; } = 1;

        /// <summary>
        /// 每页显示数量
        /// </summary>
        public int PageSize { get; set; } = 20;

    }
}
