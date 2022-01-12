using System;
using System.Collections.Generic;
using System.Linq;
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

        public DateTime StartAt { get; private set; }
        public DateTime EndAt { get; private set; }
        public long ElapsedMS => this.Watcher.ElapsedMilliseconds;
        public bool IsRunning => this.Watcher.IsRunning;
    }
}
