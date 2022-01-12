using Quartz;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TraceWebApi
{
    public partial class Form1 : Form
    {
        private readonly IScheduler Scheduler;

        public Form1()
        {
            InitializeComponent();

            var factory = new Quartz.Impl.StdSchedulerFactory();
            var sch = factory.GetScheduler().GetAwaiter().GetResult();
            this.Scheduler = sch;
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            var sch = this.Scheduler;

            btnExecute.Enabled = false;

            if (!sch.IsStarted || sch.InStandbyMode)
            {
                var intervalSec = (int)numInterval.Value;
                var job = JobBuilder.Create<TraceJob>()
                    .UsingJobData("Target", txtTestWebApi.Text)
                    .Build();
                var trigger = TriggerBuilder.Create()
                    .StartNow()
                    .WithSimpleSchedule(s => s.WithIntervalInSeconds(intervalSec).RepeatForever())
                    .Build();

                WriteLine("timer Start");
                sch.ScheduleJob(job, trigger);
                sch.Start();

                btnExecute.Text = "Stop";
                btnExecute.Enabled = true;
            }
            else
            {
                WriteLine("timer Stopping");
                btnExecute.Text = "Stopping";

                sch.Standby().Wait();

                var task = Task.Run(() =>
                {
                    while (sch.GetCurrentlyExecutingJobs().GetAwaiter().GetResult().Count > 0)
                        Thread.Sleep(100);

                    sch.Clear();

                    btnExecute.Invoke((MethodInvoker)(() =>
                    {
                        btnExecute.Text = "Start";
                        btnExecute.Enabled = true;
                    }));

                    WriteLine("timer Stopped");
                });
            }
        }

        private void WriteLine(string msg)
        {
            Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}: {msg}");
        }
    }
}
