using Guppy.GUI.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI
{
    public class Property
    {
        public static readonly Property<SpriteFont> Font = new();
        public static readonly Property<Color> Color = new();

        public static readonly Property<Color> BackgroundColor = new();

        public static readonly Property<bool> Inline = new();

        public static readonly Property<Padding> Padding = new();
        public static readonly Property<Unit> Width = new();
        public static readonly Property<Unit> Height = new();

        public static readonly Property<Alignment> Alignment = new();

        public static readonly Property<int> ScrollTrackWidth = new();
        public static readonly Property<Color> ScrollTrackColor = new();
        public static readonly Property<Color> ScrollThumbColor = new();

        public static readonly Property<Unit> BorderThickness = new();
        public static readonly Property<Color> BorderColor = new();

        public readonly Type Type;
        public readonly PropertyFlags Flags;

        protected Property(Type type, PropertyFlags flags)
        {
            this.Type = type;
            this.Flags = flags;
        }
    }

    public class Property<T> : Property
    {
        public Property(PropertyFlags flags = PropertyFlags.None) : base(typeof(T), flags)
        {
        }
    }
}
