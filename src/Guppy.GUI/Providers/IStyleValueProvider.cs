using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Providers
{
    internal interface IStyleValueProvider
    {
        bool Get<T>(Property<T> property, Selector selector, ElementState state, [MaybeNullWhen(false)] out T value);
    }
}
