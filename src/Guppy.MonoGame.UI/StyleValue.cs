using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI
{
    public record StyleValue(Selector Selector, Style Style, ElementState State, bool Defined);
    public record StyleValue<T>(Selector Selector, Style Style, ElementState State, bool Defined, T Value) : StyleValue(Selector, Style, State, Defined);
}
