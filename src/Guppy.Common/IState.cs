using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface IState
    {
        Type[] Types { get; }

        bool Matches(Type type, object? value);

        object? GetValue(Type type);
    }
    public interface IState<T> : IState
    {
        T GetValue();

        bool Matches(T value);
    }
}
