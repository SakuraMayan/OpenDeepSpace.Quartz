using OpenDeepSpace.QuartzDashboard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.QuartzDashboard.Application.Dtos
{
    /// <summary>
    /// 返回分页结果Dto
    /// </summary>
    public class JobDetailPageOutDto
    {
        public int TotalCount { get; set; }
        public List<JobDetail> JobDetails { get; set; }

    }
}
