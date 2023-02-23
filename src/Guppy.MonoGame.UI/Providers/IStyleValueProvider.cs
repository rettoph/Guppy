using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Providers
{
    public interface IStyleValueProvider
    {
        Style Style { get; }
        IStyleProvider Provider { get; }

        public void Set(ElementState state, object? value);

        public bool TryGet(ElementState state, out object? value);

        public void Clear(ElementState state);
    }

    public interface IStyleValueProvider<T> : IStyleValueProvider
    {
        public void Set(ElementState state, T? value);

        public bool TryGet(ElementState state, out T? value);
    }
}
