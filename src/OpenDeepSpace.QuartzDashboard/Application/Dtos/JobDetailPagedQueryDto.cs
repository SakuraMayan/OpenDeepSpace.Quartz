using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.QuartzDashboard.Application.Dtos
{

    /// <summary>
    /// Job分页查询Dto
    /// </summary>
    public class JobDetailPagedQueryDto:PagedBaseDto
    {
        /// <summary>
        /// Job组名
        /// </summary>
        public string JobGroupName { get; set; }

        /// <summary>
        /// Job名称
        /// </summary>
        public string JobName { get; set; }

    }
}
