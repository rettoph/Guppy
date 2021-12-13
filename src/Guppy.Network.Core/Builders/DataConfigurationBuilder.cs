using DotNetUtils.General.Interfaces;
using Guppy.Network.Enums;
using Guppy.Network.Interfaces;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Builders
{
    public abstract class DataConfigurationBuilder<TConfigurationBuilder, TConfiguration> : DynamicIdConfigurationBuilder<TConfigurationBuilder>, IFluentPrioritizable<TConfigurationBuilder>
        where TConfigurationBuilder : DataConfigurationBuilder<TConfigurationBuilder, TConfiguration>
    {
        

        protected abstract TConfiguration Build(Byte[] idBytes);
        #endregion
    }
}
