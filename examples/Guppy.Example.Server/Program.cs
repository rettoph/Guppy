using System;
using Guppy.Example.Library;
using Guppy.Extensions;

namespace Guppy.Example.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new GuppyLoader()
                .Initialize()
                .BuildGame<ExampleGame>();

            game.TryStart(false);
        }
    }
}
