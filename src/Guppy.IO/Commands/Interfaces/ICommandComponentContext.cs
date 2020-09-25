using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Commands.Interfaces
{
    public interface ICommandComponentContext
    {
        /// <summary>
        /// The primary invoker for this component.
        /// </summary>
        String Name { get; }

        /// <summary>
        /// A human readable description/overview for
        /// this component.
        /// </summary>
        String Description { get; }
    }
}
