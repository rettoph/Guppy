﻿using Serilog;

namespace Guppy.Core.Common.Providers
{
    public interface ILoggerService
    {
        ILogger GetOrCreate(Type? contextType = null);
        ILogger GetOrCreate<TContext>()
        {
            return this.GetOrCreate(typeof(TContext));
        }

        Lazy<ILogger> GetOrCreateLazy(Type contextType)
        {
            return new(() => this.GetOrCreate(contextType));
        }

        Lazy<ILogger> GetOrCreateLazy<TContext>()
        {
            return new(this.GetOrCreate<TContext>);
        }
    }
}