using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Attributes
{
    /// <summary>
    /// Used to mark a specific type as a game.
    /// Any game with this attribute will automatically
    /// be registered via the GameServiceLoader
    /// </summary>
    public class IsGameAttribute : GuppyAttribute
    {
        public IsGameAttribute(ushort priority = 100) : base(priority)
        {
        }
    }
}
