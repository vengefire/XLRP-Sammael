﻿using System;

namespace Framework.Interfaces.Logging
{
    /// <summary>
    ///     Log exceptions without throwing.
    /// </summary>
    public interface IExceptionLogger
    {
        /// <summary>
        ///     Logs the specified exception without throwing it.
        /// </summary>
        /// <param name="ex">The exception.</param>
        void Log(Exception ex);
    }
}