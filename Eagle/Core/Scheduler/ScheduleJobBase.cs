using EntityFrameworkCore.BootKit;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Scheduler
{
    public abstract class ScheduleJobBase : IScheduleJob, IJob
    {
        protected Database Dc { get; set; }

        public ScheduleJobBase()
        {
            Dc = new DefaultDataContextLoader().GetDefaultDc();
        }

        /// <summary>
        /// http://www.quartz-scheduler.net/documentation/quartz-2.x/tutorial/crontriggers.html
        /// </summary>
        /// <param name="context"></param>
        public abstract Task Execute(IJobExecutionContext context);

        /// <summary>
        /// 继续执行上一次中断的Job
        /// </summary>
        public virtual void ResumeJob()
        {

        }
    }

    public interface IScheduleJob
    {
        /// <summary>
        /// 继续执行上一次中断的Job
        /// </summary>
        void ResumeJob();
    }
}
