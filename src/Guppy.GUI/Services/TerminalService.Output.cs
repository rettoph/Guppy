using Guppy.GUI.Constants;
using Guppy.GUI.Elements;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Services
{
    internal partial class TerminalService
    {
        public class Output : ScrollBox<Label>
        {
            private class TextWriter : System.IO.TextWriter
            {
                private readonly Output _output;
                private readonly Func<Color> _color;
                public override Encoding Encoding { get; } = Encoding.Default;

                public TextWriter(Output output, Func<Color> color)
                {
                    _output = output;
                    _color = color;
                }

                public override void Write(char value)
                {
                    _output.Write(value, _color());
                }

                public static Color MapConsoleColor()
                {
                    return Console.ForegroundColor switch
                    {
                        ConsoleColor.Black => Color.Black,
                        ConsoleColor.DarkBlue => Color.DarkBlue,
                        ConsoleColor.DarkGreen => Color.DarkGreen,
                        ConsoleColor.DarkCyan => Color.DarkCyan,
                        ConsoleColor.DarkRed => Color.DarkRed,
                        ConsoleColor.DarkMagenta => Color.DarkMagenta,
                        ConsoleColor.DarkYellow => new Color(204, 119, 34),
                        ConsoleColor.Gray => Color.Gray,
                        ConsoleColor.DarkGray => Color.DarkGray,
                        ConsoleColor.Blue => Color.Blue,
                        ConsoleColor.Green => Color.Green,
                        ConsoleColor.Cyan => Color.Cyan,
                        ConsoleColor.Red => Color.Red,
                        ConsoleColor.Magenta => Color.Magenta,
                        ConsoleColor.Yellow => Color.Yellow,
                        ConsoleColor.White => Color.White,
                        _ => throw new UnreachableException()
                    };
                }
            }

            private const char NewLineChar = '\n';
            private const char CarriageReturnChar = '\r';

            private Label _label;

            public Output(Stage stage) : base(ElementNames.TerminalOutputContainer)
            {
                _label = this.CreateLabel(default);

                Console.SetError(new TextWriter(this, () => Color.Red));
                Console.SetOut(new TextWriter(this, TextWriter.MapConsoleColor));
            }

            internal void Write(char c, Color color)
            {
                switch(c)
                {
                    case NewLineChar:
                        this.CreateLabel(color);
                        break;
                    case CarriageReturnChar:
                        break;
                    default:
                        this.GetLabel(color).Text += c;

                        if (_label.Text.Length == 1)
                        {
                            this.Add(_label);
                        }
                    break;
                }
                
            }

            private Label GetLabel(Color? color)
            {
                if(color is null || color.Value == _label.Color)
                {
                    return _label;
                }

                if(_label.Text.Length == 0)
                {
                    _label.Color = color.Value;
                    return _label;
                }

                return this.CreateLabel(color.Value);
            }

            private Label CreateLabel(Color color)
            {
                _label = new Label(ElementNames.TerminalOutput);
                _label.Color = color;

                return _label;
            }
        }
    }
}
