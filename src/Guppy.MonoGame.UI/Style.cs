using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI
{
    public class Style
    {
        public readonly string Name;
        public readonly Type Type;
        public readonly bool Inherit;

        internal Style(string name, Type type, bool inherit)
        {
            this.Name = name;
            this.Type = type;
            this.Inherit = inherit;
        }

        private static readonly List<Style> _items = new();
        public static readonly ReadOnlyCollection<Style> Items = new(_items);

        public static readonly Style<SpriteFont> Font = Create<SpriteFont>(nameof(Font), true);
        public static readonly Style<Color> FontColor = Create<Color>(nameof(FontColor), true);

        public static readonly Style<Color> BackgroundColor = Create<Color>(nameof(BackgroundColor), false);

        public static readonly Style<Display> DisplayHorizontal = Create<Display>(nameof(DisplayHorizontal), false);
        public static readonly Style<Display> DisplayVertical = Create<Display>(nameof(DisplayVertical), false);

        public static Style<T> Create<T>(string name, bool inherit)
        {
            var definition = new Style<T>(name, inherit);
            _items.Add(definition);

            return definition;
        }
    }

    public sealed class Style<T> : Style
    {
        internal Style(string name, bool inherit) : base(name, typeof(T), inherit)
        {
        }
    }
}
