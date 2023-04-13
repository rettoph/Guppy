using Guppy.GUI.Constants;
using Guppy.MonoGame.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
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
        private readonly GuppyTimer _timer;
        private bool _carret;

        public string Value
        {
            get => _label.Text;
            set => _label.Text = value;
        }

        public SpriteFont? Font => _label.Font;
        public Color? Color => _label.Color;

        public event OnEventDelegate<TextInput, string>? OnEntered;

        public TextInput(params string[] names) : base(names)
        {
            _timer = new GuppyTimer(TimeSpan.FromSeconds(0.75f));
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

        protected override void DrawContent(GameTime gameTime, Vector2 position)
        {
            base.DrawContent(gameTime, position);

            if(this.State.HasFlag(ElementState.Focus))
            {
                _timer.Update(gameTime);
                while(_timer.Step(out _))
                {
                    _carret = !_carret;
                }

                if(_carret)
                {
                    Vector2 offset = new Vector2()
                    {
                        X = this.Font!.MeasureString(this.Value).X + _label.ContentOffset.X,
                        Y = 0
                    };

                    this.stage.SpriteBatch.Draw(
                        texture: this.stage.Pixel,
                        position: position + offset,
                        sourceRectangle: null,
                        color: this.Color!.Value,
                        rotation: 0,
                        origin: Vector2.Zero,
                        scale: new Vector2(this.Font!.Spacing, this.Font!.LineSpacing),
                        effects: SpriteEffects.None,
                        layerDepth: 0);
                }
            }
        }

        private void HandleStateChanged(Element sender, ElementState old, ElementState value)
        {
            bool wasActive = old.HasFlag(ElementState.Focus);
            bool isActive = value.HasFlag(ElementState.Focus);

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
                case Keys.Enter:
                    this.OnEntered?.Invoke(this, this.Value);
                    break;
                default:
                    if(this.Font?.Characters.Contains(e.Character) ?? false)
                    {
                        this.Value += e.Character;
                    }
                    break;
            }

            _timer.Reset();
            _carret = true;
            
            this.Clean();
        }
    }
}
