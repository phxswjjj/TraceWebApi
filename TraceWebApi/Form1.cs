using Quartz;
using Serilog;
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
        private readonly CancellationTokenSource MonitorCancelSource;
        private readonly Queue<TraceResult> Results;
        const int MaxResultCount = 100;

        public Form1()
        {
            InitializeComponent();

            var chartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea("elapsed ms");
            chartArea.AxisX.Minimum = 0;
            chartArea.AxisX.ScaleView.Size = MaxResultCount;
            chartArea.AxisX.Interval = 5;
            chartArea.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.FixedCount;
            //chartArea.AxisX.ScrollBar.ButtonStyle = System.Windows.Forms.DataVisualization.Charting.ScrollBarButtonStyles.All;
            chart1.ChartAreas[0] = chartArea;

            var factory = new Quartz.Impl.StdSchedulerFactory();
            var sch = factory.GetScheduler().GetAwaiter().GetResult();
            this.Scheduler = sch;

            var results = new Queue<TraceResult>(MaxResultCount);
            this.Results = results;

            var monitorCancelSource = new CancellationTokenSource();
            CancellationToken ct = monitorCancelSource.Token;
            var monitor = Task.Run(() =>
            {
                while (!ct.IsCancellationRequested)
                {
                    if (sch.IsStarted && !sch.InStandbyMode)
                    {
                        foreach (var job in sch.GetCurrentlyExecutingJobs().GetAwaiter().GetResult())
                        {
                            var result = job.Result as TraceResult;
                            if (result != null)
                            {
                                if (this.Results.Contains(result))
                                    continue;
                                //too much
                                if (this.Results.Count >= MaxResultCount)
                                    this.Results.Dequeue();
                                this.Results.Enqueue(result);
                                WriteLine($"Monitor({this.Results.Count}): {result.ElapsedMS:N0}");

                                chart1.Invoke((MethodInvoker)(() =>
                                {
                                    var series = chart1.Series[0];
                                    if (series.Points.Count >= MaxResultCount)
                                    {
                                        series.Points.RemoveAt(0);
                                    }
                                    series.Points.AddY(result.ElapsedMS);
                                }));
                            }
                        }
                    }
                }
            }, monitorCancelSource.Token);
            this.MonitorCancelSource = monitorCancelSource;
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            var sch = this.Scheduler;

            btnExecute.Enabled = false;
            gbxSetting.Enabled = false;

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
                        gbxSetting.Enabled = true;
                    }));

                    WriteLine("timer Stopped");
                });
            }
        }

        private void WriteLine(string msg)
        {
            TraceLog.Logger.Information(msg);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.MonitorCancelSource.Cancel();
        }
    }
}
