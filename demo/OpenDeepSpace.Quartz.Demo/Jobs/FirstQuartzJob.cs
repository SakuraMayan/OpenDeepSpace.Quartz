using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.Quartz.Demo.Jobs
{
    /// <summary>
    /// 第一个QuartzJob
    /// </summary>
    public class FirstQuartzJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var result = context.JobDetail.JobDataMap.ContainsKey("haha");
            if (result)
                Console.WriteLine(DateTime.Now + context.JobDetail.JobDataMap.GetString("haha") + Environment.NewLine);
            else
                Console.WriteLine(DateTime.Now + Environment.NewLine);

        }
    }
}
