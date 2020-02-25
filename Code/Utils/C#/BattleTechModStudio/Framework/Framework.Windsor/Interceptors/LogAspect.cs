﻿using System;
using System.Diagnostics;
using Castle.Core.Logging;
using Castle.DynamicProxy;
using Framework.Utils.DPHelper;

namespace Framework.Windsor.Interceptors
{
    public class LogAspect : IInterceptor
    {
        public ILogger Logger { get; set; } = NullLogger.Instance;

        public bool SkipLog { get; set; } = false;

        public void Intercept(IInvocation invocation)
        {
            var targetName = null != invocation.TargetType
                ? invocation.TargetType.Name
                : invocation.Method.DeclaringType.FullName;
            var invocationAppelation = string.Format("{0}.{1}", targetName, invocation.Method.Name);

            if (SkipLog)
            {
                try
                {
                    invocation.Proceed();
                }
                catch (Exception ex)
                {
                    Logger.ErrorFormat(
                        "An exception occurred calling [{0}], Error = [{1}], rethrowing...",
                        invocationAppelation,
                        ex.Message);
                    throw;
                }

                return;
            }

            var invocationString = DPHelper.CreateInvocationLogString(invocation);

            // this.Logger.DebugFormat("Calling [{0}]", invocationString);
            Logger.DebugFormat("Calling [{0}]", invocationAppelation);
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat(
                    ex,
                    "An exception occurred calling [{0}], Error = [{1}], rethrowing...",
                    invocationAppelation,
                    ex.Message);
                throw;
            }

            stopWatch.Stop();

            var invocationExecutionTime = stopWatch.ElapsedMilliseconds;
            Logger.DebugFormat(
                "Invocation [{0}] Completed in [{1}] ms.",
                invocationAppelation,
                invocationExecutionTime);
        }
    }
}