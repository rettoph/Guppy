using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions;
using Guppy.Loaders;
using Microsoft.Extensions.Logging;

namespace Guppy.Demo
{
    public class DemoScene : Scene
    {
        public DemoScene(IServiceProvider provider) : base(provider)
        {
        }

        protected override void PreInitialize()
        {
            base.PreInitialize();

            var stringLoader = this.provider.GetLoader<StringLoader>();
            stringLoader.Register("test", "Hello World");
        }
        protected override void PostInitialize()
        {
            base.PostInitialize();

            this.layers.Create<DemoLayer>();
        }
    }
}
