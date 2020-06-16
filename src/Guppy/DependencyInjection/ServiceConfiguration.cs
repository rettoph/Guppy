using Guppy.Extensions.Collections;
using Guppy.Extensions.System;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xxHashSharp;

namespace Guppy.DependencyInjection
{
    public class ServiceConfiguration
    {
        public UInt32 Id { get; private set; }
        public String Name { get; private set; }
        public ServiceTypeDescriptor ServiceTypeDescriptor { get; private set; }

        public ConfigurationDescriptor[] ConfigurationDescriptors { get; private set; }

        internal ServiceConfiguration(String name, ServiceTypeDescriptor service, params ConfigurationDescriptor[] configurationDescriptors)
        {
            this.Id = ServiceConfiguration.GetId(name);
            this.Name = name;
            this.ServiceTypeDescriptor = service;
            this.ConfigurationDescriptors = configurationDescriptors;
        }

        public void Build(ServiceProvider provider, Action<Object, ServiceProvider, ServiceConfiguration> setup, Action<Object> cacher = null)
        {
            // Create new instance...
            var instance = this.ServiceTypeDescriptor.Get(provider);
            cacher?.Invoke(instance);
            ExceptionHelper.ValidateAssignableFrom(this.ServiceTypeDescriptor.ServiceType, instance.GetType());

            // Apply recieved configurations...
            var ranSetup = setup == null;
            this.ConfigurationDescriptors.ForEach(c =>
            {
                if (!ranSetup && c.Priority >= 0 && (ranSetup = true))
                    setup.Invoke(instance, provider, this); // Run custom setup as default 0 priority configuration

                c.Configure(instance, provider, this);
            });

            if(!ranSetup) // Run custom setup if needed...
                setup.Invoke(instance, provider, this);
        }

        public static UInt32 GetId(String name) => name.xxHash();
    }
}
