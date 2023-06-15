using Microsoft.AspNetCore.Http;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.Quartz.Demo.Jobs
{
    /// <summary>
    /// 后台处理文件的Job
    /// </summary>
    public class FileQuartzJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            //获取数据
            var formfile= context.JobDetail.JobDataMap.Get("file") as byte[];

            //处理文件保存
        }
    }
}
