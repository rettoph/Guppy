using Guppy.Example.Library;
using System;
using Guppy.Extensions;

namespace Guppy.Examples.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var game = new Game1())
                game.Run();
        }
    }
}
