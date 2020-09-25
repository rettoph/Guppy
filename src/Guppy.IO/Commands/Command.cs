using Guppy.IO.Commands.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Commands
{
    public struct Command
    {
        public readonly Segment Segment;
        public readonly Object Data;

        public Command(Segment segment, object data)
        {
            Segment = segment;
            Data = data;
        }
    }
}
