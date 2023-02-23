using Guppy.MonoGame.UI.Providers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI
{
    public interface IStyleStack
    {
        bool TryGet<T>(Style style, ElementState state, [MaybeNullWhen(false)] out T value);

        void Push(IStyleProvider styles);
        void Pop();
    }
}
