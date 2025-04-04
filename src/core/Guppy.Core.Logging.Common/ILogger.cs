﻿namespace Guppy.Core.Logging.Common
{
    public interface ILogger
    {
        Type Context { get; }

        void Verbose(string message);
        void Verbose<T>(string messageTemplate, T propertyValue);
        void Verbose<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);
        void Verbose<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
        void Verbose(string messageTemplate, params object?[]? propertyValues);
        void Verbose(Exception? exception, string messageTemplate);
        void Verbose<T>(Exception? exception, string messageTemplate, T propertyValue);
        void Verbose<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);
        void Verbose<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
        void Verbose(Exception? exception, string messageTemplate, params object?[]? propertyValues);

        void Debug(string message);
        void Debug<T>(string messageTemplate, T propertyValue);
        void Debug<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);
        void Debug<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
        void Debug(string messageTemplate, params object?[]? propertyValues);
        void Debug(Exception? exception, string messageTemplate);
        void Debug<T>(Exception? exception, string messageTemplate, T propertyValue);
        void Debug<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);
        void Debug<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
        void Debug(Exception? exception, string messageTemplate, params object?[]? propertyValues);

        void Information(string message);
        void Information<T>(string messageTemplate, T propertyValue);
        void Information<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);
        void Information<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
        void Information(string messageTemplate, params object?[]? propertyValues);
        void Information(Exception? exception, string messageTemplate);
        void Information<T>(Exception? exception, string messageTemplate, T propertyValue);
        void Information<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);
        void Information<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
        void Information(Exception? exception, string messageTemplate, params object?[]? propertyValues);

        void Warning(string message);
        void Warning<T>(string messageTemplate, T propertyValue);
        void Warning<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);
        void Warning<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
        void Warning(string messageTemplate, params object?[]? propertyValues);
        void Warning(Exception? exception, string messageTemplate);
        void Warning<T>(Exception? exception, string messageTemplate, T propertyValue);
        void Warning<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);
        void Warning<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
        void Warning(Exception? exception, string messageTemplate, params object?[]? propertyValues);

        void Error(string message);
        void Error<T>(string messageTemplate, T propertyValue);
        void Error<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);
        void Error<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
        void Error(string messageTemplate, params object?[]? propertyValues);
        void Error(Exception? exception, string messageTemplate);
        void Error<T>(Exception? exception, string messageTemplate, T propertyValue);
        void Error<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);
        void Error<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
        void Error(Exception? exception, string messageTemplate, params object?[]? propertyValues);

        void Fatal(string message);
        void Fatal<T>(string messageTemplate, T propertyValue);
        void Fatal<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);
        void Fatal<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
        void Fatal(string messageTemplate, params object?[]? propertyValues);
        void Fatal(Exception? exception, string messageTemplate);
        void Fatal<T>(Exception? exception, string messageTemplate, T propertyValue);
        void Fatal<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);
        void Fatal<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
        void Fatal(Exception? exception, string messageTemplate, params object?[]? propertyValues);
    }

    public interface ILogger<out TContext> : ILogger
    {

    }
}
