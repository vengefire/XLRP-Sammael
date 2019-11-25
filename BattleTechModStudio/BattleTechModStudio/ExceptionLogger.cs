using System;
using Castle.Core.Logging;
using Framework.Interfaces.Logging;

namespace BattleTechModStudio
{
    public class ExceptionLogger : IExceptionLogger
    {
        private readonly ILogger logger;

        public ExceptionLogger(ILogger logger)
        {
            this.logger = logger;
        }

        public void Log(Exception ex)
        {
            logger.Error(ex.ToString(), ex);
        }
    }
}