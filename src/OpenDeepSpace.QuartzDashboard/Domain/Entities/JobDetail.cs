using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.QuartzDashboard.Domain.Entities
{
   

    /// <summary>
    /// Job的详细信息
    /// </summary>
    public class JobDetail
    {
        /// <summary>
        /// 任务组名
        /// </summary>
        public string JobGroupName { get; set; } = "";

        /// <summary>
        /// 任务名称
        /// </summary>
        public string JobName { get; set; } = "";

        /// <summary>
        /// 开始执行时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 上次执行时间
        /// </summary>
        public DateTime? PreviousFireTime { get; set; }

        /// <summary>
        /// 下次执行时间
        /// </summary>
        public DateTime? NextFireTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }


        /// <summary>
        /// 任务状态 触发器状态
        /// </summary>
        public TriggerState TriggerState { get; set; }

        /// <summary>
        /// 显示任务状态
        /// </summary>
        public string DisplayState
        {
            get
            {
                var state = string.Empty;
                switch (TriggerState)
                {
                    case TriggerState.Normal:
                        state = "正常";
                        break;
                    case TriggerState.Paused:
                        state = "暂停";
                        break;
                    case TriggerState.Complete:
                        state = "完成";
                        break;
                    case TriggerState.Error:
                        state = "异常";
                        break;
                    case TriggerState.Blocked:
                        state = "阻塞";
                        break;
                    case TriggerState.None:
                        state = "不存在";
                        break;
                    default:
                        state = "未知";
                        break;
                }
                return state;
            }
        }

        /// <summary>
        /// 执行计划 即任务执行的Cron或Simple表达式
        /// </summary>
        public string ScheduleExpression { get; set; } = "";

        /// <summary>
        /// 执行计划描述
        /// </summary>
        public string ScheduleExpressionDescription { get; set; } = "";

        /// <summary>
        /// 任务描述
        /// </summary>
        public string JobDescription { get; set; } = "";

        /// <summary>
        /// Job的数据
        /// </summary>
        public string JobData { get; set; } = "";
    }
}
