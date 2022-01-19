using Common.Logging;
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
        private readonly Queue<TraceResult> Results;
        private readonly CancellationTokenSource MonitorCancelSource;
        const int MaxResultCount = 100;

        private readonly ILog Logger = LogManager.GetLogger<Form1>();

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
            var sch = factory.GetScheduler();
            this.Scheduler = sch;

            var results = new Queue<TraceResult>(MaxResultCount);
            this.Results = results;

            var monitorCancelSource = new CancellationTokenSource();
            CancellationToken ct = monitorCancelSource.Token;

            var monitor = Task.Factory.StartNew(() =>
            {
                while (!ct.IsCancellationRequested)
                {
                    if (sch.IsStarted && !sch.InStandbyMode)
                    {
                        foreach (var job in sch.GetCurrentlyExecutingJobs())
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
                                    if (result.Result != ResultType.OK)
                                    {
                                        var pt = series.Points.Last();
                                        pt.Label = result.Result.ToString();
                                        pt.LabelToolTip = result.Message;
                                        pt.IsVisibleInLegend = true;
                                        txtMessage.Text = $"elapsed: {result.ElapsedMS:N0} ms. {result.Result}, {result.Message}";
                                    }
                                    else
                                    {
                                        txtMessage.Text = $"elapsed: {result.ElapsedMS:N0} ms. {result.Result}";
                                    }
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

                sch.Standby();

                var task = Task.Factory.StartNew(() =>
                {
                    while (sch.GetCurrentlyExecutingJobs().Count > 0)
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

        private void btnOpenLogDir_Click(object sender, EventArgs e)
        {
            var logDir = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"TraceWebApi\logs");
            System.Diagnostics.Process.Start("explorer.exe", logDir);
        }

        private void WriteLine(string msg)
        {
            Logger.Info(msg);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.MonitorCancelSource.Cancel();
            Scheduler.Shutdown();
        }
    }
}
