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

        private static readonly List<Style> _items;
        public static readonly ReadOnlyCollection<Style> Items;

        public static readonly StyleDefinition<SpriteFont> Font = Create<SpriteFont>(nameof(Font), true);
        public static readonly StyleDefinition<Color> Color = Create<Color>(nameof(Color), true);
        public static readonly StyleDefinition<Color> BackgroundColor = Create<Color>(nameof(BackgroundColor), false);

        static Style()
        {
            _items = new List<Style>();
            Items = new ReadOnlyCollection<Style>(_items);
        }

        public static StyleDefinition<T> Create<T>(string name, bool inherit)
        {
            var definition = new StyleDefinition<T>(name, inherit);
            _items.Add(definition);

            return definition;
        }
    }

    public sealed class StyleDefinition<T> : Style
    {
        internal StyleDefinition(string name, bool inherit) : base(name, typeof(T), inherit)
        {
        }
    }
}
