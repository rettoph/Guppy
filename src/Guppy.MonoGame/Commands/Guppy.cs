using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Commands
{
    public readonly struct Guppy : ICommandData
    {
        public readonly int Age;

        public Guppy(int age)
        {
            this.Age = age;
        }
    }
}
