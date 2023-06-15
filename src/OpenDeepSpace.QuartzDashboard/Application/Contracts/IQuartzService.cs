using Quartz;
using OpenDeepSpace.QuartzDashboard.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.QuartzDashboard.Application.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IQuartzService
    {

        /// <summary>
        /// 获取所有Job
        /// </summary>
        /// <returns></returns>
        public Task<JobDetailPageOutDto> GetAllJobs(JobDetailPagedQueryDto jobDetailPagedQueryDto);

        /// <summary>
        /// 暂停job
        /// </summary>
        /// <param name="jobKey"></param>
        /// <returns></returns>
        public Task<ResultBaseDto> SuspendJob(JobKey jobKey);

        /// <summary>
        /// 恢复job
        /// </summary>
        /// <param name="jobKey"></param>
        /// <returns></returns>
        public Task<ResultBaseDto> ResumeJob(JobKey jobKey);

        /// <summary>
        /// 立即执行
        /// </summary>
        /// <param name="jobKey"></param>
        /// <returns></returns>
        public Task<ResultBaseDto> ImmediatelyExecuteJob(JobKey jobKey);

        /// <summary>
        /// 删除Job
        /// </summary>
        /// <param name="jobKey"></param>
        /// <returns></returns>
        public Task<ResultBaseDto> DeleteJob(JobKey jobKey);
    }
}
