using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection
{
    public struct ServiceActionKey
    {
        public readonly Type Type;
        public readonly String Name;

        public ServiceActionKey(Type type, string name)
        {
            this.Type = type ?? typeof(Object);
            this.Name = name ?? "";
        }

        /// <summary>
        /// Determins if the current key includes a recieved service type & name
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Boolean Includes(Type type, String name)
            => this.Type.IsAssignableFrom(type) && name.StartsWith(this.Name);

        /// <summary>
        /// Create a new key instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ServiceActionKey From(Type type = null, String name = "")
            => new ServiceActionKey(type, name);

        /// <summary>
        /// Create a new key instance.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ServiceActionKey From<T>(String name = "")
            => new ServiceActionKey(typeof(T), name);
    }
}
