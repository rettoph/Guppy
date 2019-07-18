using Guppy.Factories;
using Guppy.Network.Configurations;
using Guppy.Network.Groups;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Factories
{
    public class GroupFactory
    {
        private IServiceProvider _provider;

        public GroupFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public Group Create(Guid id)
        {
            var configuration = _provider.GetRequiredService<NetworkConfiguration>();
            var group = ActivatorUtilities.CreateInstance(_provider, configuration.Group, id) as Group;

            // Initialize the group.
            group.TryBoot();
            group.TryPreInitialize();
            group.TryInitialize();
            group.TryPostInitialize();

            return group;
        }
    }
}
