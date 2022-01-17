using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TraceWebApi
{
    class TraceResult
    {
        private readonly System.Diagnostics.Stopwatch Watcher;

        public TraceResult()
        {
            StartAt = DateTime.Now;
            var sw = new System.Diagnostics.Stopwatch();
            this.Watcher = sw;
        }

        internal void Start()
        {
            this.Watcher.Start();
        }
        internal void End()
        {
            this.EndAt = DateTime.Now;
            this.Watcher.Stop();
        }

        internal void Ng(HttpResponseMessage resp)
        {
            this.Ng(resp.ReasonPhrase);
        }
        internal void Ng(Exception ex)
        {
            if (this.IsRunning)
                this.End();
            this.Ng(ex.Message);
        }
        private void Ng(string message)
        {
            this.Result = ResultType.NG;
            this.Message = message;
        }

        internal void Ok()
        {
            this.Result = ResultType.OK;
        }

        public DateTime StartAt { get; private set; }
        public DateTime EndAt { get; private set; }
        public long ElapsedMS => this.Watcher.ElapsedMilliseconds;
        public bool IsRunning => this.Watcher.IsRunning;

        public ResultType Result { get; private set; }
        public string Message { get; private set; }
    }

    enum ResultType
    {
        NA,
        OK,
        NG,
    }
}
