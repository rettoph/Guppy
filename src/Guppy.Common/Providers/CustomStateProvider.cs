using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Providers
{
    internal sealed class CustomStateProvider : StateProvider
    {
        private readonly IStateProvider _defaults;

        public CustomStateProvider(IStateProvider defaults, IEnumerable<IState> states) : base(states)
        {
            _defaults = defaults;
        }

        public override bool Matches<T>(T value)
        {
            if(base.Matches(value))
            {
                return true;
            }

            return _defaults.Matches(value);
        }

        public override bool TryGet<T>([MaybeNullWhen(false)] out T value)
        {
            if(base.TryGet(out value))
            {
                return true;
            }

            return _defaults.TryGet(out value);
        }
    }
}
