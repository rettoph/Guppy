using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Configurations
{
    /// <summary>
    /// When registering a new entity, the ServiceCollection.AddEntity<TEntity>()
    /// method may be called, and an instance of RegisteredEntityConfiguration
    /// should be provided. This stored unloaded entity data about the given entity
    /// and can be used to store static entity specific values based on key, rather
    /// than type.
    /// </summary>
    public class RegisteredEntityConfiguration
    {
        /// <summary>
        /// The Entity's name handle (using the StringLoader)
        /// </summary>
        public String NameHandle { get; set; }

        /// <summary>
        /// The Entity's description handle (using the StringLoader)
        /// </summary>
        public String DescriptionHandle { get; set; }

        /// <summary>
        /// The Entity's type
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// The Entity's custom configuration
        /// null by default
        /// </summary>
        public IEntityData Data { get; set; }
    }
}
