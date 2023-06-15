using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Quartz;
using OpenDeepSpace.QuartzDashboard.Application.Contracts;
using OpenDeepSpace.QuartzDashboard.Application.Dtos;
using OpenDeepSpace.QuartzDashboard.Domain.Entities;

namespace OpenDeepSpace.QuartzDashboard.Pages
{
    /// <summary>
    /// 
    /// </summary>
    public class QuartzDashboardModel : PageModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="quartzService"></param>
        public QuartzDashboardModel(IQuartzService quartzService)
        {
            this.quartzService = quartzService;
        }

        /// <summary>
        /// 
        /// </summary>
        public IQuartzService quartzService { get; set; }

       
        /// <summary>
        /// 绑定的数据
        /// </summary>
        [BindProperty]
        public JobDetail JobDetail { get; set; }

        //OnPost OnGet 分别对应请求方式
        //OnPost的时候会提示 某个字段必须要求字符串 这时候需要给它一个初始值
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobDetailPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAllJobs([FromBody]JobDetailPagedQueryDto jobDetailPagedQueryDto)
        { 
            var jobInfos=await quartzService.GetAllJobs(jobDetailPagedQueryDto);

            return new JsonResult(jobInfos);
        }

        public async Task<IActionResult> OnPostDeleteJob([FromBody] JobKey jobKey)
        { 
            var result=await quartzService.DeleteJob(jobKey);
            return new JsonResult(result);
        }

        public async Task<IActionResult> OnPostSuspendJob([FromBody] JobKey jobKey)
        {
            var result= await quartzService.SuspendJob(jobKey);
            return new JsonResult(result);  
        }

        public async Task<IActionResult> OnPostResumeJob([FromBody] JobKey jobKey)
        { 
            var result=await quartzService.ResumeJob(jobKey);
            return new JsonResult(result);
        }

        public async Task<IActionResult> OnPostImmediatelyExecuteJob([FromBody] JobKey jobKey)
        { 
            var result=await quartzService.ImmediatelyExecuteJob(jobKey);
            return new JsonResult(result);  
        }

    }
}
