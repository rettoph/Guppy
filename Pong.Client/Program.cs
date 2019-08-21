using Guppy;
using Guppy.Extensions.DependencyInjection;
using Guppy.Utilities.Loggers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var guppy = new GuppyLoader();
            guppy.ConfigureLogger(new ConsoleLogger())
                .Initialize();

            var pong = guppy.BuildGame<PongGame>();
            var test = guppy.BuildGame<TestGame>();

            Console.ReadLine();
        }
    }
}
