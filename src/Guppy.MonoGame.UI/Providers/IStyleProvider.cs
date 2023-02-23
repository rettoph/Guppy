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
        IElement Element { get; }

        bool TryGet<T>(Style style, ElementState state, [MaybeNullWhen(false)] out T value);

        void Set<T>(Style style, T? value);
        void Set<T>(Style style, ElementState state, T? value);

        void Clear<T>(Style style);
        void Clear<T>(Style style, ElementState state);

        void Inherit(IStyleProvider parent);
        void Clean();

        IEnumerable<IStyleValueProvider> All();
    }
}
