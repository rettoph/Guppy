using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection
{
    /// <summary>
    /// Custom index for Services that can be inherited.
    /// </summary>
    public struct ServiceConfigurationKey
    {
        public readonly Type Type;
        public readonly String Name;

        public ServiceConfigurationKey(Type type, string name)
        {
            this.Type = type;
            this.Name = name;
        }

        /// <summary>
        /// Determins if the current key includes a recieved service type & name
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Boolean Includes(Type type, String name)
            => this.Type.IsAssignableFrom(type) && name.StartsWith(this.Name);
    }
}
