using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TraceWebApi
{
    class TraceJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            var data = context.JobDetail.JobDataMap;
            var target = data.GetString("Target");
            return Task.Run(() =>
            {
                ExecuteTesting(target);
            });
        }

        private void ExecuteTesting(string url)
        {
            var uid = Math.Abs(DateTime.Now.ToBinary() % 1000000);
            WriteLine($"job-{uid} running");
            Thread.Sleep(new Random().Next(5000) + 5000);
            WriteLine($"job-{uid} finish");
        }

        private void WriteLine(string msg)
        {
            Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}: {msg}");
        }
    }
}
