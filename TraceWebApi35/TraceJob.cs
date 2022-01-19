using Common.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace TraceWebApi
{
    class TraceJob : IJob
    {
        private readonly ILog Logger = LogManager.GetLogger<TraceJob>();

        void IJob.Execute(IJobExecutionContext context)
        {
            var data = context.JobDetail.JobDataMap;
            var target = data.GetString("Target");
            var trace = ExecuteTesting(target);
            context.Result = trace;
        }

        private TraceResult ExecuteTesting(string url)
        {
            var trace = new TraceResult();

            var uid = Math.Abs(DateTime.Now.ToBinary() % 1000000);
            Logger.Info($"job-{uid} running");

            trace.Start();
            var client = CreateClient();
            try
            {
                var resp = client.DownloadString(url);
                trace.End();

                trace.Ok();
            }
            catch (Exception ex)
            {
                trace.Ng(ex);
                Logger.Error(ex.Message, ex);
                return trace;
            }

            if (trace.Result == ResultType.OK)
                Logger.Info($"job-{uid} finish. elapsed: {trace.ElapsedMS} ms, result={trace.Result}");
            else
                Logger.Warn($"job-{uid} finish. elapsed: {trace.ElapsedMS} ms, result={trace.Result}: {trace.Message}");
            return trace;
        }


        private WebClient CreateClient()
        {
            var client = new WebClient();

            return client;
        }
    }
}
