using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Commands.Interfaces
{
    public interface ICommandGroupContextParent
    {
        /// <summary>
        /// A list of all commands & groups embeded within the
        /// current command group context parent.
        /// </summary>
        IReadOnlyDictionary<String, ICommandGroupContext> Groups { get; }

        void Add(ICommandGroupContext context);
        void Remove(ICommandGroupContext context);
    }
}
