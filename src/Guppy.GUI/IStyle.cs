using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI
{
    public interface IStyle
    {
        Property Property { get; }
        Selector Selector { get; }
    }

    public interface IStyle<T> : IStyle
    {
        new Property<T> Property { get; }
        T? Value { get; }
        T? this[ElementState state] { get; }

        T? GetValue(ElementState state);

        bool TryGetValue(ElementState state, [MaybeNullWhen(false)] out T value);
    }
}
