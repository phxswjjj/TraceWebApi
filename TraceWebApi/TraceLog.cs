using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraceWebApi
{
    public class TraceLog
    {
        public static ILogger Logger { get; internal set; }
    }
}
