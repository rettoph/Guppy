using Guppy.Core.Common.Collections;
using Microsoft.Xna.Framework;
using System.Runtime.InteropServices;

namespace Guppy.Game.Console.Helpers
{
    public static class ConsoleHelper
    {
        private static Map<ConsoleColor, Color> _map = new Map<ConsoleColor, Color>([
            (ConsoleColor.Black, Color.Black),
            (ConsoleColor.DarkBlue, Color.DarkBlue),
            (ConsoleColor.DarkGreen, Color.DarkGreen),
            (ConsoleColor.DarkCyan, Color.DarkCyan),
            (ConsoleColor.DarkRed, Color.DarkRed),
            (ConsoleColor.DarkMagenta, Color.DarkMagenta),
            (ConsoleColor.DarkYellow, new Color(139, 128, 0)),
            (ConsoleColor.Gray, Color.Gray),
            (ConsoleColor.DarkGray, Color.DarkGray),
            (ConsoleColor.Blue, Color.Blue),
            (ConsoleColor.Green, Color.Green),
            (ConsoleColor.Cyan, Color.Cyan),
            (ConsoleColor.Red, Color.Red),
            (ConsoleColor.Magenta, Color.Magenta),
            (ConsoleColor.Yellow, Color.Yellow),
            (ConsoleColor.White, Color.White)
        ]);

        private static Dictionary<Color, ConsoleColor> _colorToConsoleColorCache = new Dictionary<Color, ConsoleColor>();
        public static ConsoleColor GetConsolColorFromColor(Color color)
        {
            ref ConsoleColor console = ref CollectionsMarshal.GetValueRefOrAddDefault(_colorToConsoleColorCache, color, out bool exists);
            if (exists)
            {
                return console;
            }

            (ConsoleColor console, Color color) result = default;
            int leastDistance = int.MaxValue;
            foreach ((ConsoleColor console, Color color) map in _map)
            {
                int alphaDistance = map.color.A - color.A;
                int redDistance = map.color.R - color.R;
                int greenDistance = map.color.G - color.G;
                int blueDistance = map.color.B - color.B;

                int distance = (alphaDistance * alphaDistance) + (redDistance * redDistance) + (greenDistance * greenDistance) + (blueDistance * blueDistance);
                if (distance < leastDistance)
                {
                    result = map;
                    leastDistance = distance;

                    if (distance == 0)
                    {
                        console = result.console;
                        return console;
                    }
                }
            }
            console = result.console;
            return console;
        }

        public static IDisposable ApplyForegroundColor(ConsoleColor color)
        {
            return RestConsoleForeground.Instance.Push(color);
        }
        public static IDisposable ApplyForegroundColor(Color color)
        {
            return RestConsoleForeground.Instance.Push(ConsoleHelper.GetConsolColorFromColor(color));
        }

        private class RestConsoleForeground : IDisposable
        {
            public static RestConsoleForeground Instance = new RestConsoleForeground();

            private Stack<ConsoleColor> _stack;

            private RestConsoleForeground()
            {
                _stack = new Stack<ConsoleColor>();
            }

            public RestConsoleForeground Push(ConsoleColor color)
            {
                _stack.Push(System.Console.ForegroundColor);
                System.Console.ForegroundColor = color;

                return this;
            }

            public void Dispose()
            {
                System.Console.ForegroundColor = _stack.Pop();
            }
        }
    }
}
