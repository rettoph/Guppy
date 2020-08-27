using System;
using System.Collections.Generic;
using System.Text;
using Guppy.DependencyInjection;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace Guppy.IO
{
    public class Logger : Service
    {
        #region Private Fields
        private Hierarchy _hierarchy;
        private ILog _log;
        private List<IAppender> _appenders;
        #endregion

        #region Initialization Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _log = LogManager.GetLogger(typeof(GuppyLoader));
            _hierarchy = (Hierarchy)_log.Logger.Repository;

            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%date [%thread] %-5level %logger - %message%newline";
            patternLayout.ActivateOptions();

            var test = new ConsoleAppender();
            test.Layout = patternLayout;
            test.ActivateOptions();

            _hierarchy.Root.Level = Level.All;
        }

        protected override void PostInitialize(ServiceProvider provider)
        {
            base.PostInitialize(provider);

            // Add each appender...
            _appenders.ForEach(a => _hierarchy.Root.AddAppender(a));
            _hierarchy.Configured = true;
        }
        #endregion

        public void LogDebug(Func<string> message)
        {
            _log.Debug(message);
        }

        public void LogTrace(String message)
        {
            _log.Debug(message);
        }
        public void LogTrace(Func<String> message)
        {
            _log.Debug(message());
        }

        public void LogInformation(Func<String> message)
        {
            _log.Info(message());
        }
        public void LogInformation(String message)
        {
            _log.Info(message);
        }

        public void LogWarning(String message)
        {
            _log.Warn(message);
        }
    }
}
