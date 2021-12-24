using Guppy.Example.Library;
using System;
using Guppy.Extensions;

namespace Guppy.Example.Client
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
