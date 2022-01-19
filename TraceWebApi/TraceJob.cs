using Quartz;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
                var trace = ExecuteTesting(target);
                context.Result = trace;
            });
        }

        private TraceResult ExecuteTesting(string url)
        {
            var trace = new TraceResult();

            var uid = Math.Abs(DateTime.Now.ToBinary() % 1000000);
            TraceLog.Logger.Information("job-{uid} running", uid);

            trace.Start();
            var client = CreateClient();
            try
            {
                var resp = client.GetAsync(url).GetAwaiter().GetResult();
                trace.End();

                if (resp.IsSuccessStatusCode)
                    trace.Ok();
                else
                    trace.Ng(resp);
            }
            catch (Exception ex)
            {
                trace.Ng(ex);
                TraceLog.Logger.Error(ex, ex.Message);
                return trace;
            }

            if (trace.Result == ResultType.OK)
                TraceLog.Logger.Information("job-{uid} finish. elapsed: {elapsed} ms, result={result}", uid, trace.ElapsedMS, trace.Result);
            else
                TraceLog.Logger.Warning("job-{uid} finish. elapsed: {elapsed} ms, result={result}: {msg}", uid, trace.ElapsedMS, trace.Result, trace.Message);
            return trace;
        }


        private HttpClient CreateClient()
        {
            var client = new HttpClient();
            client.Timeout = new TimeSpan(0, 1, 0);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json, text/plain, */*");

            client.DefaultRequestHeaders.AcceptEncoding.Clear();
            client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip, deflate, br");

            client.DefaultRequestHeaders.AcceptLanguage.Clear();
            client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("zh-TW,zh;q=0.9,en-US;q=0.8,en;q=0.7,zh-CN;q=0.6");

            client.DefaultRequestHeaders.UserAgent.Clear();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.212 Safari/537.36");

            return client;
        }
    }
}
