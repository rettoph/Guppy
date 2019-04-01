using Guppy.UI.Elements;
using Guppy.UI.Entities;
using Guppy.UI.Enums;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Client.Elements
{
    public class LobbyChatWindow
    {
        private Stage _stage;
        private Container _container;
        private TextInput _input;

        public LobbyChatWindow(Stage stage)
        {
            _stage = stage;
            _container = _stage.Content.Add(new SimpleContainer(0, 0, 1f, 200));

            _input = _container.Add(new TextInput(0, new Unit[] { 1f, -20 }, 1f, 20));
            _input.StyleSheet.SetProperty<Alignment>(StyleProperty.TextAlignment, Alignment.CenterLeft);
            _input.StyleSheet.SetProperty<Unit>(StyleProperty.PaddingTop, 0);
            _input.StyleSheet.SetProperty<Unit>(StyleProperty.PaddingBottom, 0);
            _input.StyleSheet.SetProperty<Color>(StyleProperty.FontColor, Color.White);
        }
    }
}
