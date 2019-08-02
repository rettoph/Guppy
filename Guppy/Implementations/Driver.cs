using Guppy.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Implementations
{
    public class Driver<TDriven> : Frameable, IDriver
        where TDriven : IDriven
    {
        private Boolean _created;

        protected TDriven parent { get; private set; }
        protected ILogger logger { get; private set; }

        public Driver(TDriven parent)
        {
            _created = false;

            this.parent = parent;
        }

        public void TryCreate(IServiceProvider provider)
        {
            if (_created)
                throw new Exception($"Unable to create more than once");

            this.Create(provider);
            _created = true;
        }

        protected virtual void Create(IServiceProvider provider) {
            this.logger = provider.GetService<ILogger>();
        }
    }
}
