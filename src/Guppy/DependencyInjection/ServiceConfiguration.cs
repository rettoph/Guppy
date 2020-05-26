using Guppy.Extensions.Collections;
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
        public ServiceDescriptor ServiceDescriptor { get; private set; }

        public ConfigurationDescriptor[] ConfigurationDescriptors { get; private set; }

        internal ServiceConfiguration(String name, ServiceDescriptor service, params ConfigurationDescriptor[] configurationDescriptors)
        {
            this.Id = ServiceConfiguration.GetId(name);
            this.Name = name;
            this.ServiceDescriptor = service;
            this.ConfigurationDescriptors = configurationDescriptors;
        }

        public Object Build(ServiceProvider provider, Action<Object, ServiceProvider, ServiceConfiguration> setup)
        {
            // Create new instance...
            var instance = this.ServiceDescriptor.Factory(provider);
            ExceptionHelper.ValidateAssignableFrom(this.ServiceDescriptor.ServiceType, instance.GetType());

            // Apply recieved configurations...
            var ranSetup = setup == null;
            this.ConfigurationDescriptors.ForEach(c =>
            {
                if (!ranSetup && c.Priority >= 0 && (ranSetup = true))
                    setup.Invoke(instance, provider, this); // Run custom setup as default 0 priority configuration

                instance = c.Configure(instance, provider, this);
            });

            if(!ranSetup) // Run custom setup if needed...
                setup.Invoke(instance, provider, this);

            // Return configured instance...
            return instance;
        }

        public static UInt32 GetId(String name) => xxHash.CalculateHash(Encoding.UTF8.GetBytes(name));
    }
}
