﻿using Autofac;
using Guppy.MonoGame.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Extensions
{
    public static class GuppyEngineExtensions
    {
        public static IGame StartGame(
            this GuppyEngine engine, 
            Action<ContainerBuilder>? build = null,
            Assembly? entry = null)
        {
            return engine.Start(build, entry).Scope.Resolve<IGame>();
        }
    }
}
