using Guppy.Configurations;
using Guppy.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.Pools
{
    public class EntityPool : ServicePool<Entity>
    {
        private EntityConfiguration _configuration;
        private EntityFactory _factory;

        public EntityPool(EntityConfiguration configuration, EntityFactory factory, IServiceProvider provider) : base(provider)
        {
            _configuration = configuration;
            _factory = factory;
        }
    }
}
