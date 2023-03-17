using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI
{
    public static class IStyleExtensions
    {
        public static bool TryGetValue<T>(this IStyle<T> style, [MaybeNullWhen(false)] out T value)
        {
            return style.TryGetValue(ElementState.None, out value);
        }
    }
}
