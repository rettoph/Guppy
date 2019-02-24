using Guppy.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Demo
{
    public class DemoGame : Game
    {
        protected override void PreInitialize()
        {
            base.PreInitialize();

            this.services.AddScene<DemoScene>();
            this.services.AddLayer<DemoLayer>();
        }
        protected override void PostInitialize()
        {
            base.PostInitialize();

            this.Logger.LogInformation($"PostInitiazlizing DemoGame...");

            this.scenes.Create<DemoScene>();
        }
    }
}
