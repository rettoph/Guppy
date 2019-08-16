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
        }
    }

    class Test
    {
        public Guid Id;

        public Test()
        {
            this.Id = Guid.NewGuid();
            Console.WriteLine("New Test => " + this.Id.ToString());
        }
    }
}
