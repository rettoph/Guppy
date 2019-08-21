using Guppy.Attributes;
using Guppy.Implementations;
using Guppy.Interfaces;
using Guppy.Utilities.Pools;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Loaders
{
    /// <summary>
    /// The pool loader is a loader used to generate
    /// pools of initializable objects automatically. 
    /// Each pool registered via this loader will be 
    /// scoped within the service provider.
    /// </summary>
    [IsLoader]
    public class PoolLoader : Loader<Type, Type, Type>
    {
        internal Dictionary<Type, Type> ValuesTable { get { return this.valuesTable; } }

        public PoolLoader(ILogger logger) : base(logger)
        {
        }

        #region Registration Methods
        public void TryRegister(Type targetType, Type poolType, UInt16 priority = 100)
        {
            if (!typeof(Pool).IsAssignableFrom(poolType))
                throw new Exception($"Unable to register pool. Type<{poolType.Name}> does not extend Pool.");
             
            this.Register(targetType, poolType, priority);
        }
        #endregion

        protected override Type GetOutValue(Type value)
        {
            return value;
        }
    }
}
