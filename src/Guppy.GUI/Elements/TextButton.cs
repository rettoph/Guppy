using Guppy.GUI.Constants;
using Guppy.MonoGame.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Elements
{
    public class TextButton : Button<Label>
    {
        private readonly Label _label;

        public string Text
        {
            get => _label.Text;
            set => _label.Text = value;
        }

        public SpriteFont? Font => _label.Font;
        public Color Color
        {
            get => _label.Color;
            set => _label.Color = value;
        }


        public TextButton(params string[] names) : this((IEnumerable<string>)names)
        {
        }
        public TextButton(IEnumerable<string> names) : base(names)
        {
            _label = new Label(ElementNames.TextInputLabel);
            this.Add(_label);
        }
    }
}
