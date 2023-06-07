using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Providers
{
    public interface IStateProvider
    {
        bool TryGet<T>([MaybeNullWhen(false)] out T state);

        bool Matches<T>(T state);

        IStateProvider Custom(IState[] states);
    }
}
