using Guppy.Core.Logging.Common;

namespace Guppy.Core.Logging.Serilog
{
    public class SerilogGuppyLogger<TContext>(ISerilogLogger serilog) : ILogger<TContext>
    {
        private readonly ISerilogLogger _serilog = serilog;

        public Type Context => typeof(TContext);

        public void Verbose(string message)
        {
            this._serilog.Verbose(message);
        }

        public void Verbose<T>(string messageTemplate, T propertyValue)
        {
            this._serilog.Verbose(messageTemplate, propertyValue);
        }

        public void Verbose<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            this._serilog.Verbose(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Verbose<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            this._serilog.Verbose(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Verbose(string messageTemplate, params object?[]? propertyValues)
        {
            this._serilog.Verbose(messageTemplate, propertyValues);
        }

        public void Verbose(Exception? exception, string messageTemplate)
        {
            this._serilog.Verbose(exception, messageTemplate);
        }

        public void Verbose<T>(Exception? exception, string messageTemplate, T propertyValue)
        {
            this._serilog.Verbose(exception, messageTemplate, propertyValue);
        }

        public void Verbose<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            this._serilog.Verbose(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Verbose<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            this._serilog.Verbose(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Verbose(Exception? exception, string messageTemplate, params object?[]? propertyValues)
        {
            this._serilog.Verbose(exception, messageTemplate, propertyValues);
        }

        public void Debug(string message)
        {
            this._serilog.Debug(message);
        }

        public void Debug<T>(string messageTemplate, T propertyValue)
        {
            this._serilog.Debug(messageTemplate, propertyValue);
        }

        public void Debug<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            this._serilog.Debug(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Debug<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            this._serilog.Debug(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Debug(string messageTemplate, params object?[]? propertyValues)
        {
            this._serilog.Debug(messageTemplate, propertyValues);
        }

        public void Debug(Exception? exception, string messageTemplate)
        {
            this._serilog.Debug(exception, messageTemplate);
        }

        public void Debug<T>(Exception? exception, string messageTemplate, T propertyValue)
        {
            this._serilog.Debug(exception, messageTemplate, propertyValue);
        }

        public void Debug<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            this._serilog.Debug(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Debug<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            this._serilog.Debug(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Debug(Exception? exception, string messageTemplate, params object?[]? propertyValues)
        {
            this._serilog.Debug(exception, messageTemplate, propertyValues);
        }

        public void Information(string message)
        {
            this._serilog.Information(message);
        }

        public void Information<T>(string messageTemplate, T propertyValue)
        {
            this._serilog.Information(messageTemplate, propertyValue);
        }

        public void Information<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            this._serilog.Information(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Information<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            this._serilog.Information(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Information(string messageTemplate, params object?[]? propertyValues)
        {
            this._serilog.Information(messageTemplate, propertyValues);
        }

        public void Information(Exception? exception, string messageTemplate)
        {
            this._serilog.Information(exception, messageTemplate);
        }

        public void Information<T>(Exception? exception, string messageTemplate, T propertyValue)
        {
            this._serilog.Information(exception, messageTemplate, propertyValue);
        }

        public void Information<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            this._serilog.Information(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Information<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            this._serilog.Information(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Information(Exception? exception, string messageTemplate, params object?[]? propertyValues)
        {
            this._serilog.Information(exception, messageTemplate, propertyValues);
        }

        public void Warning(string message)
        {
            this._serilog.Warning(message);
        }

        public void Warning<T>(string messageTemplate, T propertyValue)
        {
            this._serilog.Warning(messageTemplate, propertyValue);
        }

        public void Warning<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            this._serilog.Warning(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Warning<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            this._serilog.Warning(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Warning(string messageTemplate, params object?[]? propertyValues)
        {
            this._serilog.Warning(messageTemplate, propertyValues);
        }

        public void Warning(Exception? exception, string messageTemplate)
        {
            this._serilog.Warning(exception, messageTemplate);
        }

        public void Warning<T>(Exception? exception, string messageTemplate, T propertyValue)
        {
            this._serilog.Warning(exception, messageTemplate, propertyValue);
        }

        public void Warning<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            this._serilog.Warning(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Warning<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            this._serilog.Warning(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Warning(Exception? exception, string messageTemplate, params object?[]? propertyValues)
        {
            this._serilog.Warning(exception, messageTemplate, propertyValues);
        }

        public void Error(string message)
        {
            this._serilog.Error(message);
        }

        public void Error<T>(string messageTemplate, T propertyValue)
        {
            this._serilog.Error(messageTemplate, propertyValue);
        }

        public void Error<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            this._serilog.Error(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Error<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            this._serilog.Error(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Error(string messageTemplate, params object?[]? propertyValues)
        {
            this._serilog.Error(messageTemplate, propertyValues);
        }

        public void Error(Exception? exception, string messageTemplate)
        {
            this._serilog.Error(exception, messageTemplate);
        }

        public void Error<T>(Exception? exception, string messageTemplate, T propertyValue)
        {
            this._serilog.Error(exception, messageTemplate, propertyValue);
        }

        public void Error<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            this._serilog.Error(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Error<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            this._serilog.Error(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Error(Exception? exception, string messageTemplate, params object?[]? propertyValues)
        {
            this._serilog.Error(exception, messageTemplate, propertyValues);
        }

        public void Fatal(string message)
        {
            this._serilog.Fatal(message);
        }

        public void Fatal<T>(string messageTemplate, T propertyValue)
        {
            this._serilog.Fatal(messageTemplate, propertyValue);
        }

        public void Fatal<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            this._serilog.Fatal(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Fatal<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            this._serilog.Fatal(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Fatal(string messageTemplate, params object?[]? propertyValues)
        {
            this._serilog.Fatal(messageTemplate, propertyValues);
        }

        public void Fatal(Exception? exception, string messageTemplate)
        {
            this._serilog.Fatal(exception, messageTemplate);
        }

        public void Fatal<T>(Exception? exception, string messageTemplate, T propertyValue)
        {
            this._serilog.Fatal(exception, messageTemplate, propertyValue);
        }

        public void Fatal<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            this._serilog.Fatal(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Fatal<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            this._serilog.Fatal(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Fatal(Exception? exception, string messageTemplate, params object?[]? propertyValues)
        {
            this._serilog.Fatal(exception, messageTemplate, propertyValues);
        }
    }
}