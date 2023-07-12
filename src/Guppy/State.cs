﻿using Guppy.Attributes;
using Guppy.Common;
using Guppy.Enums;

namespace Guppy
{
    [Service<IState>(ServiceLifetime.Scoped, true)]
    public abstract class State<T> : IState<T>
    {
        private static readonly Type[] _types = new[] { typeof(T) };
        public Type[] Types => _types;

        object? IState.GetValue(Type type)
        {
            throw new NotImplementedException();
        }

        bool IState.Matches(Type type, object? value)
        {
            throw new NotImplementedException();
        }

        public abstract T GetValue();

        public virtual bool Matches(T value)
        {
            return this.GetValue()?.Equals(value) ?? false;
        }
    }
}