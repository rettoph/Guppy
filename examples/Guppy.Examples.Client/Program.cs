using Guppy.Example.Library;
using System;
using Guppy.Extensions;

namespace Guppy.Examples.Client
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
