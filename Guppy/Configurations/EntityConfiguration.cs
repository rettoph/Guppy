using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Factories;
using Guppy.Loaders;

namespace Guppy.Configurations
{
    public class EntityConfiguration
    {
        public readonly String Handle;
        public readonly String Name;
        public readonly String Description;
        public readonly Type Type;
        public readonly Object Data;


        public EntityConfiguration(String handle, RegisteredEntityConfiguration configuration, StringLoader stringLoader)
        {
            this.Handle = handle;
            this.Name = stringLoader.GetValue(configuration.NameHandle);
            this.Description = stringLoader.GetValue(configuration.DescriptionHandle);
            this.Type = configuration.Type;
            this.Data = configuration.Data;
        }
    }
}
