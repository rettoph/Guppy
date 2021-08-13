using Guppy.Extensions.System.Collections;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Guppy.Extensions.log4net
{
    public static class ILogExtensions
    {
        #region Static Fields
        private static PatternLayout PatternLayout = new PatternLayout();

        static ILogExtensions()
        {
            ILogExtensions.PatternLayout.ConversionPattern = "[%d{HH:mm:ss}] [%level] %message%n";
            ILogExtensions.PatternLayout.ActivateOptions();
        }
        #endregion

        #region Level Methods
        /// <summary>
        /// Get the current log level.
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public static Level GetLevel(this ILog log)
            => ((Hierarchy)log.Logger.Repository).Root.Level;

        /// <summary>
        /// Set a new log level value.
        /// </summary>
        /// <param name="log"></param>
        /// <param name="level"></param>
        public static void SetLevel(this ILog log, Level level)
        {
            ((Hierarchy)log.Logger.Repository).Root.Level = level;
            ((Hierarchy)log.Logger.Repository).RaiseConfigurationChanged(EventArgs.Empty);
        }
        #endregion

        #region Appender Methods
        /// <summary>
        /// Add a new appender to the log configuration.
        /// </summary>
        /// <param name="log"></param>
        /// <param name="appender"></param>
        public static void AddAppender(this ILog log, IAppender appender)
        {
            ((Hierarchy)log.Logger.Repository).Root.AddAppender(appender);
            ((Hierarchy)log.Logger.Repository).RaiseConfigurationChanged(EventArgs.Empty);
        }

        public static ILog ConfigureManagedColoredConsoleAppender(this ILog log, params ManagedColoredConsoleAppender.LevelColors[] mapping)
        {
            var appender = new ManagedColoredConsoleAppender();
            appender.Layout = ILogExtensions.PatternLayout;
            mapping.ForEach(m => appender.AddMapping(m));
            appender.ActivateOptions();

            log.AddAppender(appender);

            return log;
        }

        public static ILog ConfigureFileAppender(this ILog log, String file)
        {
            var appender = new FileAppender();
            appender.Layout = ILogExtensions.PatternLayout;
            appender.File = file;
            appender.ActivateOptions();

            log.AddAppender(appender);

            return log;
        }
        #endregion

        #region Verbose
        public static void Verbose(this ILog log, String message)
            => log.Logger.Log(
                MethodBase.GetCurrentMethod().DeclaringType,
                Level.Verbose,
                message,
                null);

        public static void Verbose(this ILog log, Func<String> message)
        {
            if(log.GetLevel() <= Level.Verbose)
                log.Logger.Log(
                    MethodBase.GetCurrentMethod().DeclaringType,
                    Level.Verbose,
                    message(),
                    null);
        }
        #endregion

        #region Debug
        public static void Debug(this ILog log, Func<String> message)
        {
            if (log.GetLevel() <= Level.Debug)
                log.Logger.Log(
                    MethodBase.GetCurrentMethod().DeclaringType,
                    Level.Debug,
                    message(),
                    null);
        }
        #endregion

        #region Info
        public static void Info(this ILog log, Func<String> message)
        {
            if (log.GetLevel() <= Level.Info)
                log.Logger.Log(
                    MethodBase.GetCurrentMethod().DeclaringType,
                    Level.Info,
                    message(),
                    null);
        }
        #endregion

        #region Warn
        public static void Warn(this ILog log, Func<String> message)
        {
            if (log.GetLevel() <= Level.Warn)
                log.Logger.Log(
                    MethodBase.GetCurrentMethod().DeclaringType,
                    Level.Warn,
                    message(),
                    null);
        }
        #endregion

        #region Error
        public static void Error(this ILog log, Func<String> message)
        {
            if (log.GetLevel() <= Level.Error)
                log.Logger.Log(
                    MethodBase.GetCurrentMethod().DeclaringType,
                    Level.Error,
                    message(),
                    null);
        }
        #endregion

        #region Fatal
        public static void Fatal(this ILog log, Func<String> message)
        {
            if (log.GetLevel() <= Level.Fatal)
                log.Logger.Log(
                    MethodBase.GetCurrentMethod().DeclaringType,
                    Level.Fatal,
                    message(),
                    null);
        }
        #endregion
    }
}
