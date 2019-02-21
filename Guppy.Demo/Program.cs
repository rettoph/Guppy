using System;

namespace Guppy.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new DemoGame();

            game.Start();

            Console.ReadLine();
        }
    }
}
