using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Input
{
    public interface ICursor
    {
        string Id { get; }
        Vector2 Position { get; }
        Vector2 Delta { get; }
    }
}
