using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Commands.Interfaces
{
    public interface ICommandGroupContext : ICommandGroupContextParent, ICommandComponentContext
    {
        /// <summary>
        /// The current command group's parent (if any)
        /// </summary>
        ICommandGroupContext ParentContext { get; }

        /// <summary>
        /// The full name of the current item, including all
        /// parent ICommandGroupContext's
        /// </summary>
        String FullName { get; }

        Boolean TrySetParent(ICommandGroupContext parentContext);
    }
}
