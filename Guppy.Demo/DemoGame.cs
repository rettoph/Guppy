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
        }
        protected override void PostInitialize()
        {
            base.PostInitialize();

            this.Logger.LogInformation($"PostInitiazlizing DemoGame...");

            this.Scenes.Create<DemoScene>();
        }
    }
}
