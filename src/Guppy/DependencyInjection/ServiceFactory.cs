using Guppy.Extensions.Collections;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xxHashSharp;

namespace Guppy.DependencyInjection
{
    public class ServiceFactory
    {
        public UInt32 Id { get; private set; }
        public String Name { get; private set; }
        public ServiceDescriptor ServiceDescriptor { get; private set; }

        public ConfigurationDescriptor[] ConfigurationDescriptors { get; private set; }

        internal ServiceFactory(String name, ServiceDescriptor service, params ConfigurationDescriptor[] configurationDescriptors)
        {
            this.Id = ServiceFactory.GetId(name);
            this.Name = name;
            this.ServiceDescriptor = service;
            this.ConfigurationDescriptors = configurationDescriptors;
        }

        public Object Build(ServiceProvider provider)
        {
            // Create new instance...
            var instance = this.ServiceDescriptor.Factory(provider);
            ExceptionHelper.ValidateAssignableFrom(this.ServiceDescriptor.ServiceType, instance.GetType());

            // Apply recieved configurations...
            this.ConfigurationDescriptors.ForEach(c => instance = c.Configure(instance, provider, this));

            // Return configured instance...
            return instance;
        }
        /// <summary>
        /// Builds the service with custom one time configurations.
        /// 
        /// These configurations will be used once then forgotten.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="customConfigurations"></param>
        /// <returns></returns>
        public Object CustomBuild(ServiceProvider provider, params ConfigurationDescriptor[] customConfigurations)
        {
            // Create new instance...
            var instance = this.ServiceDescriptor.Factory(provider);
            ExceptionHelper.ValidateAssignableFrom(this.ServiceDescriptor.ServiceType, instance.GetType());

            // Apply recieved configurations...
            this.ConfigurationDescriptors
                .Concat(customConfigurations)
                .OrderBy(c => c.Priority)
                .ForEach(c => instance = c.Configure(instance, provider, this));

            // Return configured instance...
            return instance;
        }


        public static UInt32 GetId(String name) => xxHash.CalculateHash(Encoding.UTF8.GetBytes(name));
    }
}
