using System;
using System.Collections.Generic;
using System.Text;
using xxHashSharp;

namespace Guppy.DependencyInjection
{
    public class Configuration
    {
        public UInt32 Id { get; private set; }
        public String Name { get; private set; }
        public Type ServiceType { get; private set; }

        public ConfigurationDescriptor[] Descriptors { get; private set; }

        internal Configuration() : this("", new ConfigurationDescriptor[0])
        {

        }
        internal Configuration(String name, params ConfigurationDescriptor[] descriptors)
        {
            this.Id = xxHash.CalculateHash(Encoding.UTF8.GetBytes(name));
            this.Name = name;
            this.Descriptors = descriptors;
        }
    }
}
