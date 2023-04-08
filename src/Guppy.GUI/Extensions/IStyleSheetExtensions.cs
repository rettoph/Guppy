using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI
{
    public static class IStyleSheetExtensions
    {
        public static IStyleSheet Set<T>(this IStyleSheet styleSheet, Property<T> property, Selector selector, T value, int priority = 0)
        {
            return styleSheet.Set<T>(property, selector, ElementState.None, value, priority);
        }

        public static IStyleSheet Set<T>(this IStyleSheet styleSheet, Property<T> property, Selector selector, string resource, int priority = 0)
        {
            return styleSheet.Set<T>(property, selector, ElementState.None, resource, priority);
        }

        public static IStyleSheet Configure(this IStyleSheet styleSheet, Selector selector, Action<SelectorManager> configuration)
        {
            SelectorManager manager = new SelectorManager(styleSheet, selector);
            configuration(manager);

            return styleSheet;
        }
    }
}
