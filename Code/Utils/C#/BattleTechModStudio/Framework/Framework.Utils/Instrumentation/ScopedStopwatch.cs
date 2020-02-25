using System;
using System.Diagnostics;
using Castle.Core.Logging;

namespace Framework.Utils.Instrumentation
{
    public class ScopedStopwatch : IDisposable
    {
        private readonly ILogger logger;

        private readonly Stopwatch stopwatch = new Stopwatch();

        public ScopedStopwatch(ILogger logger)
        {
            this.logger = logger;
            stopwatch.Start();
        }

        private long ElapsedMilliseconds => stopwatch.ElapsedMilliseconds;

        public void Dispose()
        {
            stopwatch.Stop();
            logger.Info($"Operation took [{ElapsedMilliseconds}]ms to complete.");
        }
    }
}