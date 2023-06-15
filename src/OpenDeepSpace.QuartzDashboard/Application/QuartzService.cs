using Newtonsoft.Json;
using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;
using OpenDeepSpace.QuartzDashboard.Application.Contracts;
using OpenDeepSpace.QuartzDashboard.Application.Dtos;
using OpenDeepSpace.QuartzDashboard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.QuartzDashboard.Application
{
    /// <summary>
    /// 
    /// </summary>
    public class QuartzService : IQuartzService
    {
        public ISchedulerFactory schedulerFactory;

        public QuartzService(ISchedulerFactory schedulerFactory)
        {
            this.schedulerFactory = schedulerFactory;
        }

        public async Task<ResultBaseDto> DeleteJob(JobKey jobKey)
        {
            var scheduler = await schedulerFactory.GetScheduler();
            var result=await SuspendJob(jobKey);
            if(!result.Success) return result;
            ResultBaseDto resultBaseDto=new ResultBaseDto();
            try
            {
                resultBaseDto.Success = await scheduler.DeleteJob(jobKey);

            }
            catch (Exception ex)
            {

                resultBaseDto.Success=false;
                resultBaseDto.FailedMsg=ex.Message;
            }

            return resultBaseDto;
        }

        public async Task<JobDetailPageOutDto> GetAllJobs(JobDetailPagedQueryDto jobDetailPagedQueryDto)
        {
            JobDetailPageOutDto jobDetailPageOutDto = new JobDetailPageOutDto();    

            var scheduler = await schedulerFactory.GetScheduler();

            List<JobKey> jobKeyList = new List<JobKey>();
            List<JobDetail> jobDetailList = new List<JobDetail>();
            var groupNames = await scheduler.GetJobGroupNames();
            foreach (var groupName in groupNames.OrderBy(t => t))
            {
                jobKeyList.AddRange(await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupName)));
            }
            foreach (var jobKey in jobKeyList.OrderBy(t => t.Name))
            {
                var jobDetail = await scheduler.GetJobDetail(jobKey);
                if (jobDetail == null)
                    continue;
                var triggersList = await scheduler.GetTriggersOfJob(jobKey);
                var triggers = triggersList.AsEnumerable().FirstOrDefault();
                if (triggers == null)
                    continue;

                var interval = string.Empty;
                if (triggers is SimpleTriggerImpl)
                    interval = (triggers as SimpleTriggerImpl)?.RepeatInterval.ToString();
                else
                    interval = (triggers as CronTriggerImpl)?.CronExpressionString;

                jobDetailList.Add(new JobDetail()
                {
                    JobGroupName = jobKey.Group??"",
                    JobName = jobKey.Name??"",
                    TriggerState = await scheduler.GetTriggerState(triggers.Key),
                    PreviousFireTime = triggers.GetPreviousFireTimeUtc()?.LocalDateTime,
                    NextFireTime = triggers.GetNextFireTimeUtc()?.LocalDateTime,
                    StartTime = triggers.StartTimeUtc.LocalDateTime,
                    EndTime = triggers.EndTimeUtc?.LocalDateTime,
                    JobDescription = jobDetail.Description??"",
                    ScheduleExpression = interval??"",
                    ScheduleExpressionDescription = triggers.Description??"",
                    JobData=JsonConvert.SerializeObject(jobDetail.JobDataMap)
                    
                });
            }

           
            //按条件搜索
            if (!string.IsNullOrWhiteSpace(jobDetailPagedQueryDto.JobGroupName))
            { 
                jobDetailList=jobDetailList.Where(x => x.JobGroupName == jobDetailPagedQueryDto.JobGroupName).ToList();
            }
            if (!string.IsNullOrWhiteSpace(jobDetailPagedQueryDto.JobName))
            { 
                jobDetailList=jobDetailList.Where(x => x.JobName == jobDetailPagedQueryDto.JobName).ToList();
            }

            jobDetailPageOutDto.TotalCount=jobDetailList.Count;
            //分页查询
            jobDetailList = jobDetailList.Skip((jobDetailPagedQueryDto.CurrentPage - 1) * jobDetailPagedQueryDto.PageSize).Take(jobDetailPagedQueryDto.PageSize).ToList();

            jobDetailPageOutDto.JobDetails = jobDetailList;

            return jobDetailPageOutDto;
        }

        public async Task<ResultBaseDto> ImmediatelyExecuteJob(JobKey jobKey)
        {
            ResultBaseDto resultBaseDto = new ResultBaseDto();
            var scheduler = await schedulerFactory.GetScheduler();

            try
            {

                await scheduler.TriggerJob(jobKey);
                resultBaseDto.Success = true;
            }
            catch (Exception ex)
            { 
                resultBaseDto.Success=false;
                resultBaseDto.FailedMsg = ex.Message;
            }
            return resultBaseDto;
        }

        public async Task<ResultBaseDto> ResumeJob(JobKey jobKey)
        {
            var scheduler = await schedulerFactory.GetScheduler();
            ResultBaseDto resultBaseDto = new ResultBaseDto();
            if (await scheduler.CheckExists(jobKey))//检查job是否存在
            {
                var jobDetail = await scheduler.GetJobDetail(jobKey);
                var endTime = jobDetail.JobDataMap.GetString("EndAt");
                if (!string.IsNullOrWhiteSpace(endTime) && DateTime.Parse(endTime) <= DateTime.Now)
                {
                    resultBaseDto.Success = false;
                    resultBaseDto.FailedMsg = "Job的结束时间已过期。";
                }
                else
                {
                    try
                    {
                        //任务已经存在则暂停任务
                        await scheduler.ResumeJob(jobKey);
                        resultBaseDto.Success = true;

                    }
                    catch (Exception ex)
                    {
                        resultBaseDto.Success = false;
                        resultBaseDto.FailedMsg=ex.Message;
                    }
                }

            }
            else 
            {
                resultBaseDto.Success = false;
                resultBaseDto.FailedMsg = "该Job已经不存在";
            }

            return resultBaseDto;
        }

        public async Task<ResultBaseDto> SuspendJob(JobKey jobKey)
        {
            ResultBaseDto resultBaseDto = new ResultBaseDto();
            var scheduler = await schedulerFactory.GetScheduler();

            try
            {

                await scheduler.PauseJob(jobKey);
                resultBaseDto.Success = true;
            }
            catch (Exception ex)
            {
                resultBaseDto.Success=false;
                resultBaseDto.FailedMsg=ex.Message;
            }
            return resultBaseDto;
        }
    }
}
