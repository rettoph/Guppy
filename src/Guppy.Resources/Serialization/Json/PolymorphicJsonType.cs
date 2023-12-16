using Guppy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Serialization.Json
{
    public class PolymorphicJsonType
    {
        public string Key { get; }
        public Type InstanceType { get; }
        public Type BaseType { get; }

        public PolymorphicJsonType(string key, Type instanceType, Type baseType)
        {
            ThrowIf.Type.IsNotAssignableFrom(baseType, instanceType);

            this.Key = key;
            this.InstanceType = instanceType;
            this.BaseType = baseType;
        }
    }

    public sealed class PolymorphicJsonType<TInstance, TBase> : PolymorphicJsonType
        where TInstance : TBase
    {
        public PolymorphicJsonType(string key) : base(key, typeof(TInstance), typeof(TBase))
        {
        }
    }
}
