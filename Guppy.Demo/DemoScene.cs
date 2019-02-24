using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Guppy.Demo
{
    public class DemoScene : Scene
    {
        public DemoScene(IServiceProvider provider) : base(provider)
        {
        }

        protected override void PostInitialize()
        {
            base.PostInitialize();

            this.layers.Create<DemoLayer>();
        }
    }
}
