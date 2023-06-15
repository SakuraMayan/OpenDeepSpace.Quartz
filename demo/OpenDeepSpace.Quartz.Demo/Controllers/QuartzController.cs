using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDeepSpace.Autofacastle.DependencyInjection.Attributes;
using OpenDeepSpace.Quartz.Demo.Jobs;
using Quartz;


namespace OpenDeepSpace.Quartz.Demo.Controllers
{
    
    /// <summary>
    /// Quartz测试
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    [AutomaticInjection]
    public class QuartzController:ControllerBase
    {
        
        private readonly ISchedulerFactory schedulerFactory;

        /// <summary>
        /// 测试文件在Quartz后台的处理
        /// </summary>
        /// <param name="formFile"></param>
        [HttpPost]
        public async Task TestFileInQuartz(IFormFile formFile)
        {
            //获取一个调度器
            var scheduler = await schedulerFactory.GetScheduler();

            //删除Job看是否会从数据库删除 会从数据库删除job
            await scheduler.DeleteJob(new JobKey("filejob"));

            //Job数据Map
            JobDataMap jobDataMap = new JobDataMap();

            var stream=formFile.OpenReadStream();
            byte[] fileBytes=new byte[stream.Length];
            await stream.ReadAsync(fileBytes, 0, fileBytes.Length);

            //放入文件 byte数据
            jobDataMap.Put("file",fileBytes);
            //创建Job明细
            IJobDetail jobDetail = JobBuilder.Create<FileQuartzJob>().SetJobData(jobDataMap).Build();

            //立即执行
            ITrigger trigger = TriggerBuilder.Create().StartNow().Build();
            await scheduler.ScheduleJob(jobDetail, trigger);
        }

    }
}
