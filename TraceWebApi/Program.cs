using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TraceWebApi
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            InitLogger();

            InitServiceSettings();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            //flush buffered
            Log.CloseAndFlush();
        }

        private static void InitServiceSettings()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }

        private static void InitLogger()
        {
            TraceLog.Logger = new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .Enrich.WithProperty("app", "TraceWebApi")
                .Enrich.WithProperty("host", Environment.MachineName)
                .CreateLogger();
        }
    }
}
