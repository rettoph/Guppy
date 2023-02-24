using Guppy.MonoGame.UI.Elements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Providers
{
    public interface IStyleProvider
    {
        Selector Selector { get; }
        StyleSheet Source { get; }

        bool TryGet<T>(Style style, ElementState state, [MaybeNullWhen(false)] out T value);
    }
}
