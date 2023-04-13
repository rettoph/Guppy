using Guppy.GUI.Constants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Elements
{
    public class TextInput : Input<Label>
    {
        private readonly Label _label;

        public string Value
        {
            get => _label.Text;
            set => _label.Text = value;
        }

        public SpriteFont? Font => _label.Font;
        public Color? Color => _label.Color;

        public TextInput(params string[] names) : base(names)
        {
            _label = new Label(ElementNames.TextInputLabel);
            this.Add(_label);
        }

        protected internal override void Initialize(Stage stage, Element? parent)
        {
            base.Initialize(stage, parent);

            this.OnStateChanged += this.HandleStateChanged;
        }

        protected internal override void Uninitialize()
        {
            base.Uninitialize();

            this.OnStateChanged -= this.HandleStateChanged;
        }

        private void HandleStateChanged(Element sender, ElementState old, ElementState value)
        {
            bool wasActive = old.HasFlag(ElementState.Active);
            bool isActive = value.HasFlag(ElementState.Active);

            if (wasActive && !isActive)
            {
                this.stage.Screen.Window.TextInput -= this.HandleTextInput;
                return;
            }

            if (!wasActive && isActive)
            {
                this.stage.Screen.Window.TextInput += this.HandleTextInput;
                return;
            }
        }

        private void HandleTextInput(object? sender, TextInputEventArgs e)
        {
            switch(e.Key)
            {
                case Keys.Back:
                    if(this.Value.Length > 0)
                    {
                        this.Value = this.Value.Remove(this.Value.Length - 1, 1);
                    }
                    break;
                default:
                    if(this.Font?.Characters.Contains(e.Character) ?? false)
                    {
                        this.Value += e.Character;
                    }
                    break;
            }

            
            this.Clean();
        }
    }
}
