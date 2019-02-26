using Guppy.Extensions;
using Guppy.Loaders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Demo
{
    public class DemoGame : Game
    {
        protected override void Boot()
        {
            base.Boot();

            this.services.AddScene<DemoScene>();
            this.services.AddLayer<DemoLayer>();
        }

        protected override void PreInitialize()
        {
            base.PreInitialize();
        }

        protected override void PostInitialize()
        {
            base.PostInitialize();

            this.logger.LogInformation($"PostInitiazlizing DemoGame...");

            this.scenes.Create<DemoScene>();
        }
    }
}
